using System;
using System.Diagnostics;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.ILToCodeModel;

namespace memory_contracts_rewriter
{
    class PTNode
    {
        public PTNode(string name, string method)
        {
            Name = name;
            Method = method;
        }

        public PTNode(XmlNode xmlNode)
        {
            Name = xmlNode.Attributes["name"].Value;
            if (xmlNode.Attributes["method"] != null)
            {
                Method = xmlNode.Attributes["method"].Value;
            }
        }

        public string Name { get; set; }
        public string Method { get; set; }

        public override bool Equals(object obj)
        {
            return Name == ((PTNode)obj).Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

    class PointsToInformation
    {
        protected XmlDocument _xml;

        public PointsToInformation(string assemblyPath)
        {
            ProcessStartInfo start = new ProcessStartInfo();

            start.FileName = GetCodeContractsInstallDir() + @"Bin\PointsToInfoExtractor.exe";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.Arguments = "\"" + assemblyPath + "\"";

            Process process = Process.Start(start);

            string xmlStr = process.StandardOutput.ReadToEnd();

            _xml = new XmlDocument();
            _xml.LoadXml(xmlStr);

            /**
             * Example of the XML returned:
             * 
                <ptg>
                  <method type="class" name="method1">
                    <escape> <!-- nodes that escape the method -->
                      <node name="node1" method="method1" />
                      <node name="node2" method="method2" />
                    </escape>
                    <expressions>
                      <expr val="expr1">
                        <reaches> <!-- nodes reached by expr1 -->
                          <node name="node1" method="method1">
                            <expr val="." /> <!-- node1 is reached by expr1 itself -->
                          </node>
                        </reaches>
                      </expr>
                      <expr val="expr2">
                        <reaches>
                          <node name="node1" method="method1">
                            <expr val="f" /> <!-- node1 is reached by expr2.f -->
                          </node>
                          <node name="node2" method="method2">
                            <expr val="." />
                            <expr val="f" /> <!-- node2 is reached by both expr and expr2.f -->
                          </node>
                        </reaches>
                      </expr>
                    </expressions>
                  </method>
                  <method type="class" name="method1">
                    ...
                  </method>
                </ptg>
             * 
             **/
        }

        protected static string GetCodeContractsInstallDir()
        {
            return Environment.GetEnvironmentVariable("CodeContractsInstallDir");
        }

        protected Dictionary<string, List<PTNode>> _escapingNodes = new Dictionary<string, List<PTNode>>();

        protected List<PTNode> GetEscapingNodes(string type, string method)
        {
            string key = type + "." + method;
            if (_escapingNodes.ContainsKey(key))
            {
                return _escapingNodes[key];
            }

            List<PTNode> nodes = new List<PTNode>();

            foreach (XmlNode xmlNode in _xml.SelectNodes(string.Format("/ptg/method[@type='{0}' and @name='{1}']/escape/node", type, method)))
            {
                nodes.Add(new PTNode(xmlNode));
            }

            _escapingNodes.Add(key, nodes);

            return nodes;
        }

        protected Dictionary<string, List<PTNode>> _expressionReachingNodes = new Dictionary<string, List<PTNode>>();

        protected List<PTNode> GetExpressionReachingNodes(string type, string method, string expression, bool exact)
        {
            exact = false;

            string key = type + "." + method + "::" + expression;
            if (_expressionReachingNodes.ContainsKey(key))
            {
                return _expressionReachingNodes[key];
            }

            var nodes = new List<PTNode>();

            var exprParts = expression.Split(new char[] { '.' }, 2);

            var exprInit = exprParts[0];
            var exprEnd = "." + (exprParts.Length > 1 ? exprParts[1] : "");

            string xpathExpr =
                string.Format("/ptg/method[@type='{0}' and @name='{1}']/expressions/expr[@val='{2}']/reaches/node/expr[{3}]",
                type, method, exprInit, string.Format(exact ? "@val = '{0}'" : "starts-with(@val, '{0}')", exprEnd));

            PTNode node;
            foreach (XmlNode xmlNode in _xml.SelectNodes(xpathExpr))
            {
                node = new PTNode(xmlNode.ParentNode);
                if (!nodes.Contains(node))
                {
                    nodes.Add(node);
                }
            }

            _expressionReachingNodes.Add(key, nodes);

            return nodes;
        }

        /// <summary>
        /// Returns true iff the node corresponding to the statement given escapes through the expression given
        /// </summary>
        public bool Escapes(string type, string method, IStatement statement, string expression)
        {
            expression = NormalizeExpression(expression);

            var statementEscapingNode = GetStatementEscapingNode(type, method, statement);
            var expressionReachingNodes = GetExpressionReachingNodes(type, method, expression, false);

            return expressionReachingNodes.Contains(statementEscapingNode);
        }

        /// <summary>
        /// Returns true iff some node corresponding to the expression given escapes from the method
        /// </summary>
        public bool Escapes(string type, string method, string expression)
        {
            expression = NormalizeExpression(expression);

            var escapingNodes = GetEscapingNodes(type, method);
            var expressionReachingNodes = GetExpressionReachingNodes(type, method, expression, true);

            return escapingNodes.Intersect(expressionReachingNodes).Count() > 0;
        }

        /// <summary>
        /// Returns true iff the node corresponding to the statement given escapes
        /// </summary>
        public bool Escapes(string type, string method, IStatement statement)
        {
            return GetStatementEscapingNode(type, method, statement) != null;
        }

        /// <summary>
        /// Returns true iff some node reacheable by expr1 escapes through expr2
        /// </summary>
        public bool Escapes(string type, string method, string expr1, string expr2)
        {
            expr1 = NormalizeExpression(expr1);
            expr2 = NormalizeExpression(expr2);

            var expr1ReachingNodes = GetExpressionReachingNodes(type, method, expr1, true);
            var expr2ReachingNodes = GetExpressionReachingNodes(type, method, expr2, false);

            return expr1ReachingNodes.Intersect(expr2ReachingNodes).Count() > 0;
        }

        private Dictionary<IStatement, PTNode> _statementsNodes = new Dictionary<IStatement, PTNode>();

        private PTNode GetStatementEscapingNode(string type, string method, IStatement statement)
        {
            if (_statementsNodes.ContainsKey(statement))
            {
                return _statementsNodes[statement];
            }

            PTNode escNode = null;
            IEnumerator<ILocation> i = statement.Locations.GetEnumerator();
            if (i.MoveNext())
            {
                var loc = (IPrimarySourceLocation)i.Current;
                var id = String.Format("({0},{1},", loc.StartLine, loc.StartColumn);
                foreach (var node in GetEscapingNodes(type, method))
                {
                    if (node.Name.StartsWith(id) && node.Method == type + "::" + method)
                    {
                        escNode = node;
                        break;
                    }
                }
            }

            _statementsNodes.Add(statement, escNode);

            return escNode;
        }

        public class CurrentMethodInfoData
        {
            public ITypeDefinition ContainingType { get; set; }
            public IEnumerable<IParameterDefinition> Parameters { get; set; }
        }

        public CurrentMethodInfoData CurrentMethodInfo;

        /// Normalizes the given expression so that getters and setters are changed
        /// to the corresponding field, eg:
        /// ---
        /// private T _Field;
        /// public T Field { get { return _Field; } set { _Field = value; } }
        /// 
        /// If the given expression is this.Field, it is normalized to this._Field
        /// ---
        private string NormalizeExpression(string expr)
        {
            string[] exprParts = expr.Split('.');
            if (exprParts.Length > 0)
            {
                var part = exprParts[0];
                ITypeDefinition prevPartType = null;
                if (part == "this")
                {
                    prevPartType = CurrentMethodInfo.ContainingType;
                }
                else
                {
                    //search for param
                    foreach (var param in CurrentMethodInfo.Parameters)
                    {
                        if (param.Name.Value == part)
                        {
                            prevPartType = param.Type.ResolvedType;
                            break;
                        }
                    }
                }
                if (prevPartType != null)
                {
                    for (int i = 1; i < exprParts.Length; i++)
                    {
                        part = exprParts[i];
                        ITypeDefinition currPartType = null;
                        foreach (var field in prevPartType.Fields)
                        {
                            if (field.Name.Value == part)
                            {
                                currPartType = field.Type.ResolvedType;
                                break;
                            }
                        }
                        if (currPartType == null)
                        {
                            foreach (var method in prevPartType.Methods)
                            {
                                if (method.Name.Value == "get_" + part)
                                {
                                    var methodStatements = ((BasicBlock)((Microsoft.Cci.ILToCodeModel.SourceMethodBody)method.Body).Block).Statements;
                                    if (methodStatements.Count == 1 && methodStatements[0] is IReturnStatement)
                                    {
                                        var retStmt = (IReturnStatement)methodStatements[0];
                                        var retExpr = retStmt.Expression;
                                        if (retExpr is IBoundExpression)
                                        {
                                            var retExprBound = (IBoundExpression)retExpr;
                                            if (retExprBound.Instance is IThisReference)
                                            {
                                                if (retExprBound.Definition is IFieldDefinition)
                                                {
                                                    var retDef = (IFieldDefinition)retExprBound.Definition;
                                                    exprParts[i] = retDef.Name.Value;
                                                    currPartType = retDef.Type.ResolvedType;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (currPartType == null)
                        {
                            break;
                        }
                        else
                        {
                            prevPartType = currPartType;
                        }
                    }
                }
            }
            return string.Join(".", exprParts);
        }
    }
}
