using System;
using Microsoft.Cci;
using System.Collections.Generic;
using Microsoft.Cci.MutableCodeModel;
using System.Text.RegularExpressions;

namespace memory_contracts_rewriter
{
    class MemoryContractsExtractor : BaseCodeTraverser
    {
        IMethodDefinition _currentMethod;

        static Regex _reBindExpr = new Regex(@"BindRsd\([^,]+,([^)]+)\)", RegexOptions.Compiled);
        static Regex _reTmpContract = new Regex(@"Tmp<.+>\(([^,]+)\);", RegexOptions.Compiled);
        static Regex _reTmpContractWithCond = new Regex(@"Tmp<.+>\((.+),(.+)\);", RegexOptions.Compiled);
        static Regex _reRsdContract = new Regex(@"Rsd<.+>\([^,]+,([^,]+)\);", RegexOptions.Compiled);
        static Regex _reRsdContractWithCond = new Regex(@"Rsd<.+>\([^,]+,([^,]+),(.+)\);", RegexOptions.Compiled);

        public override void Visit(IMethodDefinition method)
        {
            _currentMethod = method;

            base.Visit(method);
        }

        IStatement _currentStatement;

        public override void Visit(IStatement statement)
        {
            _currentStatement = statement;

            base.Visit(statement);
        }

        //methodName -> types of tmps
        public Dictionary<string, List<ITypeReference>> MethodsTmpTypes = new Dictionary<string, List<ITypeReference>>();

        //methodName -> type of tmp -> contract
        public Dictionary<string, Dictionary<ITypeReference, string>> MethodsTmpContracts = new Dictionary<string, Dictionary<ITypeReference, string>>();

        //methodName -> type of tmp -> cond -> contract (string expr + code model expr)
        public Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>> MethodsTmpContractsWithConds = new Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>>();

        //methodName -> types of rsds
        public Dictionary<string, List<RsdType>> MethodsRsdTypes = new Dictionary<string, List<RsdType>>();

        //methodName -> rsd name -> type of rsd -> contract
        public Dictionary<string, Dictionary<string, Dictionary<ITypeReference, string>>> MethodsRsdContracts = new Dictionary<string, Dictionary<string, Dictionary<ITypeReference, string>>>();

        //methodName -> rsd name -> type of rsd -> cond -> contract (string expr + code model expr)
        public Dictionary<string, Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>>> MethodsRsdContractsWithConds = new Dictionary<string, Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>>>();

        //methodName -> rsd name -> rsd bind expr
        public Dictionary<string, Dictionary<string, string>> MethodsRsdExprs = new Dictionary<string, Dictionary<string, string>>();

        public void VisitMethodCall(IMethodReference methodReference, IEnumerable<IExpression> arguments)
        {
            if (methodReference.ContainingType.ToString() == "Contracts.Contract.Memory")
            {
                var currentMethodName = _currentMethod.ToString();

                if (methodReference is Microsoft.Cci.MutableCodeModel.GenericMethodInstanceReference)
                {
                    var methodToCall = (Microsoft.Cci.MutableCodeModel.GenericMethodInstanceReference)methodReference;
                    var tmpOrRsdType = methodToCall.GenericArguments[0];

                    if (methodReference.Name.Value == "Tmp")
                    {
                        //fill MethodsTmpTypes
                        if (!MethodsTmpTypes.ContainsKey(currentMethodName))
                        {
                            MethodsTmpTypes.Add(currentMethodName, new List<ITypeReference>());
                        }
                        if (!MethodsTmpTypes[currentMethodName].Contains(tmpOrRsdType))
                        {
                            MethodsTmpTypes[currentMethodName].Add(tmpOrRsdType);
                        }
                        //fill MethodsTmpContracts
                        var expr = "";
                        var locsIt = _currentStatement.Locations.GetEnumerator();
                        if (locsIt.MoveNext())
                        {
                            var source = ((IPrimarySourceLocation)locsIt.Current).Source;
                            var matches = _reTmpContract.Matches(source);
                            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                            {
                                expr = matches[0].Groups[1].Value.Trim();
                                if (expr != "")
                                {
                                    if (!MethodsTmpContracts.ContainsKey(currentMethodName))
                                    {
                                        MethodsTmpContracts.Add(currentMethodName, new Dictionary<ITypeReference, string>());
                                    }
                                    if (!MethodsTmpContracts[currentMethodName].ContainsKey(tmpOrRsdType))
                                    {
                                        MethodsTmpContracts[currentMethodName][tmpOrRsdType] = expr;
                                    }
                                    //also add to MethodsTmpContractsWithConds with condition true
                                    var cond = "true";
                                    if (!MethodsTmpContractsWithConds.ContainsKey(currentMethodName))
                                    {
                                        MethodsTmpContractsWithConds.Add(currentMethodName, new Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>());
                                    }
                                    if (!MethodsTmpContractsWithConds[currentMethodName].ContainsKey(tmpOrRsdType))
                                    {
                                        MethodsTmpContractsWithConds[currentMethodName].Add(tmpOrRsdType, new Dictionary<string, Tuple<string, IExpression>>());
                                    }
                                    if (!MethodsTmpContractsWithConds[currentMethodName][tmpOrRsdType].ContainsKey(cond))
                                    {
                                        var argsIt = arguments.GetEnumerator();
                                        argsIt.MoveNext();
                                        MethodsTmpContractsWithConds[currentMethodName][tmpOrRsdType][cond] = new Tuple<string, IExpression>(expr, argsIt.Current);
                                    }
                                }
                            }
                            matches = _reTmpContractWithCond.Matches(source);
                            if (matches.Count > 0 && matches[0].Groups.Count > 2)
                            {
                                expr = matches[0].Groups[1].Value.Trim();
                                var cond = matches[0].Groups[2].Value.Trim();
                                if (expr != "" && cond != "")
                                {
                                    if (!MethodsTmpContractsWithConds.ContainsKey(currentMethodName))
                                    {
                                        MethodsTmpContractsWithConds.Add(currentMethodName, new Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>());
                                    }
                                    if (!MethodsTmpContractsWithConds[currentMethodName].ContainsKey(tmpOrRsdType))
                                    {
                                        MethodsTmpContractsWithConds[currentMethodName].Add(tmpOrRsdType, new Dictionary<string, Tuple<string, IExpression>>());
                                    }
                                    if (!MethodsTmpContractsWithConds[currentMethodName][tmpOrRsdType].ContainsKey(cond))
                                    {
                                        var argsIt = arguments.GetEnumerator();
                                        argsIt.MoveNext();
                                        MethodsTmpContractsWithConds[currentMethodName][tmpOrRsdType][cond] = new Tuple<string, IExpression>(expr, argsIt.Current);
                                    }
                                }
                            }
                        }
                    }

                    if (methodReference.Name.Value == "Rsd")
                    {
                        //fill MethodsRsdTypes
                        if (!MethodsRsdTypes.ContainsKey(currentMethodName))
                        {
                            MethodsRsdTypes.Add(currentMethodName, new List<RsdType>());
                        }
                        var argsIt = arguments.GetEnumerator();
                        argsIt.MoveNext();
                        var rsdNameLocal = ((BoundExpression)argsIt.Current).Definition.ToString();
                        var rsdType = new RsdType() { Type = tmpOrRsdType, Name = rsdNameLocal };
                        if (!MethodsRsdTypes[currentMethodName].Contains(rsdType))
                        {
                            MethodsRsdTypes[currentMethodName].Add(rsdType);
                        }
                        //fill MethodsRsdContracts
                        var expr = "";
                        var locsIt = _currentStatement.Locations.GetEnumerator();
                        if (locsIt.MoveNext())
                        {
                            var source = ((IPrimarySourceLocation)locsIt.Current).Source;
                            var matches = _reRsdContract.Matches(source);
                            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                            {
                                expr = matches[0].Groups[1].Value.Trim();
                                if (expr != "")
                                {
                                    if (!MethodsRsdContracts.ContainsKey(currentMethodName))
                                    {
                                        MethodsRsdContracts.Add(currentMethodName, new Dictionary<string, Dictionary<ITypeReference, string>>());
                                    }
                                    if (!MethodsRsdContracts[currentMethodName].ContainsKey(rsdNameLocal))
                                    {
                                        MethodsRsdContracts[currentMethodName][rsdNameLocal] = new Dictionary<ITypeReference, string>();
                                    }
                                    if (!MethodsRsdContracts[currentMethodName][rsdNameLocal].ContainsKey(tmpOrRsdType))
                                    {
                                        MethodsRsdContracts[currentMethodName][rsdNameLocal][tmpOrRsdType] = expr;
                                    }
                                    //also add to MethodsRsdContractsWithConds with condition true
                                    var cond = "true";
                                    if (!MethodsRsdContractsWithConds.ContainsKey(currentMethodName))
                                    {
                                        MethodsRsdContractsWithConds.Add(currentMethodName, new Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>>());
                                    }
                                    if (!MethodsRsdContractsWithConds[currentMethodName].ContainsKey(rsdNameLocal))
                                    {
                                        MethodsRsdContractsWithConds[currentMethodName].Add(rsdNameLocal, new Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>());
                                    }
                                    if (!MethodsRsdContractsWithConds[currentMethodName][rsdNameLocal].ContainsKey(tmpOrRsdType))
                                    {
                                        MethodsRsdContractsWithConds[currentMethodName][rsdNameLocal].Add(tmpOrRsdType, new Dictionary<string, Tuple<string, IExpression>>());
                                    }
                                    if (!MethodsRsdContractsWithConds[currentMethodName][rsdNameLocal][tmpOrRsdType].ContainsKey(cond))
                                    {
                                        argsIt = arguments.GetEnumerator();
                                        argsIt.MoveNext();
                                        argsIt.MoveNext();
                                        MethodsRsdContractsWithConds[currentMethodName][rsdNameLocal][tmpOrRsdType][cond] = new Tuple<string, IExpression>(expr, argsIt.Current);
                                    }
                                }
                            }
                            matches = _reRsdContractWithCond.Matches(source);
                            if (matches.Count > 0 && matches[0].Groups.Count > 2)
                            {
                                expr = matches[0].Groups[1].Value.Trim();
                                var cond = matches[0].Groups[2].Value.Trim();
                                if (expr != "" && cond != "")
                                {
                                    if (!MethodsRsdContractsWithConds.ContainsKey(currentMethodName))
                                    {
                                        MethodsRsdContractsWithConds.Add(currentMethodName, new Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>>());
                                    }
                                    if (!MethodsRsdContractsWithConds[currentMethodName].ContainsKey(rsdNameLocal))
                                    {
                                        MethodsRsdContractsWithConds[currentMethodName].Add(rsdNameLocal, new Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>());
                                    }
                                    if (!MethodsRsdContractsWithConds[currentMethodName][rsdNameLocal].ContainsKey(tmpOrRsdType))
                                    {
                                        MethodsRsdContractsWithConds[currentMethodName][rsdNameLocal].Add(tmpOrRsdType, new Dictionary<string, Tuple<string, IExpression>>());
                                    }
                                    if (!MethodsRsdContractsWithConds[currentMethodName][rsdNameLocal][tmpOrRsdType].ContainsKey(cond))
                                    {
                                        argsIt = arguments.GetEnumerator();
                                        argsIt.MoveNext();
                                        argsIt.MoveNext();
                                        MethodsRsdContractsWithConds[currentMethodName][rsdNameLocal][tmpOrRsdType][cond] = new Tuple<string, IExpression>(expr, argsIt.Current);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (methodReference.Name.Value == "BindRsd")
                {
                    //fill MethodsRsdExprs
                    var argsIt = arguments.GetEnumerator();
                    argsIt.MoveNext();
                    var rsdNameLocal = ((BoundExpression)argsIt.Current).Definition.ToString();
                    var expr = "";
                    var locsIt = _currentStatement.Locations.GetEnumerator();
                    if (locsIt.MoveNext())
                    {
                        var source = ((IPrimarySourceLocation)locsIt.Current).Source;
                        var matches = _reBindExpr.Matches(source);
                        if (matches.Count > 0 && matches[0].Groups.Count > 1)
                        {
                            expr = matches[0].Groups[1].Value.Trim();
                        }
                    }
                    if (expr != "")
                    {
                        if (!MethodsRsdExprs.ContainsKey(currentMethodName))
                        {
                            MethodsRsdExprs.Add(currentMethodName, new Dictionary<string, string>());
                        }
                        if (!MethodsRsdExprs[currentMethodName].ContainsKey(rsdNameLocal))
                        {
                            MethodsRsdExprs[currentMethodName][rsdNameLocal] = expr;
                        }
                    }
                }
            }
        }

        public override void Visit(IMethodCall methodCall)
        {
            VisitMethodCall(methodCall.MethodToCall, methodCall.Arguments);

            base.Visit(methodCall);
        }

        public override void Visit(ICreateObjectInstance createObjectInstance)
        {
            VisitMethodCall(createObjectInstance.MethodToCall, createObjectInstance.Arguments);

            base.Visit(createObjectInstance);
        }
    }
}
