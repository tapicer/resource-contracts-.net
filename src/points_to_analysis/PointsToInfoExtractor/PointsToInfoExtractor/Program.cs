using System;
using System.Compiler;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace PointsToInfoExtractor
{
    class Program
    {
        static int Main(string[] args)
        {
            ErrorNodeList enl = new ErrorNodeList();
            ErrorHandler eh = new ErrorHandler(enl);
            TypeSystem ts = new TypeSystem(eh);

            //args = new string[]
            //    {
            //        //@"C:\Users\Jonathan Tapicer\Documents\Visual Studio 2010\Projects\escape_test\escape_test\bin\Debug\escape_test.dll",
            //        //@"C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\example_with_contracts\Example\obj\Debug\Decl\Example.dll",
            //        @"C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\experiments\experiments\obj\Debug\Decl\experiments.dll",
            //    };

            if (args.Length == 0)
            {
                Console.WriteLine("Provide assembly full path as an argument.");
                return 1;
            }

            AssemblyNode assembly = AssemblyNode.GetAssembly(
                args[0],
                true, true, true);

            PointsToAnalysis pta = new PointsToAnalysis(ts, assembly, true, true, 3);

            pta.Visit(assembly);

            XmlTextWriter xml = new XmlTextWriter(Console.Out);

            xml.Formatting = Formatting.Indented;

            xml.WriteStartElement("ptg");

            foreach (var method in pta.AnalyzedMethods())
            {
                xml.WriteStartElement("method");
                xml.WriteAttributeString("type", method.DeclaringType.ToString());
                xml.WriteAttributeString("name", method.Name.ToString());

                var summ = pta.GetSummaryForMethod(method);

                var ptg = summ.PointsToGraph;

                //if (method.Name.ToString() == "M1") ptg.GenerateDotGraph("c:\\tmp\\ptg.dot");

                //nodes escape

                xml.WriteStartElement("escape");

                var added = new List<string>();
                foreach (var n in ptg.ReachableFromParametersReturnAndGlobals())
                {
                    if (!added.Contains(n.Name))
                    {
                        xml.WriteStartElement("node");
                        xml.WriteAttributeString("name", n.Name);
                        if (n.Label != null && n.Label.Method != null)
                        {
                            xml.WriteAttributeString("method", n.Label.Method.DeclaringType.ToString() + "::" + n.Label.Method.Name);
                        }
                        xml.WriteEndElement(); //</node>
                        added.Add(n.Name);
                    }
                }

                xml.WriteEndElement(); //</escape>

                //locals, params, ret, globals, fields inside nodes

                xml.WriteStartElement("expressions");

                Edges edges = new Edges();
                edges.AddEdges(ptg.I);
                edges.AddEdges(ptg.O);

                foreach (var v in ptg.LV)
                {
                    xml.WriteStartElement("expr");
                    if (ptg.RetValue == v.Key.Variable)
                    {
                        xml.WriteAttributeString("isReturn", "true");
                    }
                    xml.WriteAttributeString("val", v.Key.Name.Name);

                    Nodes nodesReacheable = Nodes.Empty;
                    var reacheableNodesExpressions = new Dictionary<string, Set<string>>(); //key are exprs, values are reacheable nodes by the expr

                    foreach (var addr in v.Value)
                    {
                        var nodesReacheableFromAddr = ptg.NodesForwardReachableFrom(addr);

                        nodesReacheable.AddRange(nodesReacheableFromAddr);

                        foreach (var nodeReacheableFromAddr in nodesReacheableFromAddr)
                        {
                            Set<IEnumerable<Edge>> pathsFromAddrToReacheableNode = (Set<IEnumerable<Edge>>)ptg.DFSPathFromTo(addr, nodeReacheableFromAddr, edges);
                            foreach (var path in pathsFromAddrToReacheableNode)
                            {
                                var pathFields = new List<string>();
                                foreach (var edge in path)
                                {
                                    if (edge.Field.Name.Name != "*" &&
                                        edge.Field.Name.Name != "?" &&
                                        edge.Field.Name.Name != "$")
                                    {
                                        pathFields.Add(edge.Field.Name.Name);
                                    }
                                }
                                if (!reacheableNodesExpressions.ContainsKey(nodeReacheableFromAddr.Name))
                                {
                                    reacheableNodesExpressions.Add(nodeReacheableFromAddr.Name, new Set<string>());
                                }
                                var expr = "." + string.Join(".", pathFields);
                                if (!reacheableNodesExpressions[nodeReacheableFromAddr.Name].Contains(expr))
                                {
                                    reacheableNodesExpressions[nodeReacheableFromAddr.Name].Add(expr);
                                }
                            }
                        }
                    }

                    xml.WriteStartElement("reaches");
                    foreach (var r in nodesReacheable)
                    {
                        if (r.Name != v.Key.Name.Name)
                        {
                            xml.WriteStartElement("node");
                            xml.WriteAttributeString("name", r.Name);
                            if (r.Label != null && r.Label.Method != null)
                            {
                                xml.WriteAttributeString("method", r.Label.Method.DeclaringType.ToString() + "::" + r.Label.Method.Name);
                            }

                            if (reacheableNodesExpressions.ContainsKey(r.Name))
                            {
                                foreach (var expr in reacheableNodesExpressions[r.Name])
                                {
                                    xml.WriteStartElement("expr");
                                    xml.WriteAttributeString("val", expr);
                                    xml.WriteEndElement(); //</expr>
                                }
                            }

                            xml.WriteEndElement(); //</node>
                        }
                    }
                    xml.WriteEndElement(); //</reaches>

                    xml.WriteEndElement(); //</expr>
                }

                xml.WriteEndElement(); //</expressions>

                xml.WriteEndElement(); //</method>
            }

            xml.WriteEndElement(); //</ptg>

            return 0;
        }
    }
}
