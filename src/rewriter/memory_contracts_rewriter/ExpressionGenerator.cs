using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Cci;
using Microsoft.Cci.MutableCodeModel;

namespace memory_contracts_rewriter
{

    #region custom enumerator

    internal enum RPNValueType
    {
        OPERAND,
        OPERATION
    }

    internal struct RPNValue
    {
        public string val;
        public RPNValueType type;
    }

    internal class RPNEnumerator : IEnumerator
    {
        #region Fields

        private int _index;
        private object _current;
        private string _rpn;

        #endregion

        public RPNEnumerator(string rpn)
        {
            _index = 0;
            _rpn = rpn;
        }

        public bool MoveNext()
        {
            bool result = false;
            if (_rpn.Length > _index)
            {
                string val = String.Empty;
                RPNValue rpnVal = new RPNValue();
                if (_rpn[_index] == '{')
                {
                    _index++;
                    while (_rpn[_index] != '}')
                    {
                        val += _rpn[_index].ToString();
                        _index++;
                    }
                    rpnVal.val = val;
                    rpnVal.type = RPNValueType.OPERAND;
                }
                else
                {
                    rpnVal.val = _rpn[_index].ToString();
                    rpnVal.type = RPNValueType.OPERATION;
                }
                _current = rpnVal;
                _index++;
                result = true;
            }
            return result;
        }

        public void Reset()
        {
            _index = 0;
            _current = null;
        }

        public object Current
        {
            get
            {
                return _current;
            }
        }
    }

    #endregion

    public sealed class ExpressionGenerator
    {
        private static bool IsOperation(char atom)
        {
            return (atom == '-' || atom == '+' || atom == '*' || atom == '/' || atom == '>' || atom == '<' || atom == '=' || atom == '#' || atom == '%' || atom == '&' || atom == '|');
        }
        private static bool IsOperand(char atom)
        {
            return (atom >= '0' && atom <= '9' || atom == ':' || atom == '.' || atom == '{' || atom == '}' || atom >= 'a' && atom <= 'z' || atom >= 'A' && atom <= 'Z' || atom == '.' || atom == '_');
        }
        private static bool IsBracket(char atom)
        {
            return (atom == '(' || atom == ')');
        }
        private static bool IsLeftWeaker(char lop, char rop)
        {
            return ((lop == '+' || lop == '-') && (rop == '/' || rop == '*')) ||
                   ((lop == '&' || lop == '|') && (rop == '>' || rop == '<' || rop == '%' || rop == '#'));
        }
        private static bool IsLeftStronger(char lop, char rop)
        {
            return ((lop == '/' || lop == '*') && (rop == '+' || rop == '-')) ||
                   ((lop == '>' || lop == '<' || lop == '%' || lop == '#') && (rop == '&' || rop == '|')) ||
                   rop == '(';
        }
        private static bool CompareOperations(char lop, char rop)
        {
            return IsLeftWeaker(lop, rop) || !IsLeftStronger(lop, rop);
        }

        private static string GetRPN(string expression)
        {
            Stack stOps = new Stack();
            StringBuilder sbRpn = new StringBuilder();
            for (int i = 0; i < expression.Length; i++)
            {
                if (IsOperand(expression[i]))
                {
                    sbRpn.Append(expression[i]);
                }
                else if (IsOperation(expression[i]))
                {
                    if (stOps.Count == 0)
                    {
                        stOps.Push(expression[i]);
                    }
                    else
                    {
                        if (CompareOperations(expression[i], (char)stOps.Peek()))
                        {
                            sbRpn.Append((char)stOps.Pop());
                        }
                        stOps.Push(expression[i]);
                    }
                }
                else if (IsBracket(expression[i]))
                {
                    if (expression[i] == '(')
                    {
                        stOps.Push('(');
                    }
                    else
                    {
                        while (stOps.Count > 0 && (char)stOps.Peek() != '(')
                        {
                            sbRpn.Append((char)stOps.Pop());
                        }
                        stOps.Pop(); //just like a trash
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("expression", "Wrong parameter value!");
                }
            }
            while (stOps.Count > 0)
            {
                sbRpn.Append((char)stOps.Pop());
            }
            return sbRpn.ToString();
        }

        private static Regex reNegNum = new Regex(@"-([0-9]+)", RegexOptions.Compiled);
        private static Regex reNum = new Regex(@"(^:?[0-9]+$)", RegexOptions.Compiled);
        private static Regex reVar = new Regex(@"([a-zA-Z_0-9\.:]+)", RegexOptions.Compiled);

        private static string Normalize(string expr)
        {
            expr = reNegNum.Replace(expr, ":$1"); //:nnn para -nnn
            expr = reVar.Replace(expr, "{$1}");
            return expr
                .Replace(" ", "")
                .Replace("<=", "#")
                .Replace(">=", "%")
                .Replace("&&", "&")
                .Replace("||", "|")
                .Replace("==", "=")
                ;
        }

        private static IExpression GetExprWithAccesor(IExpression expr, string accesor, IMetadataHost host)
        {
            var candMethod = "get_" + accesor;
            if (expr.Type.ResolvedType is Vector && accesor == "Length")
            {
                var int32Type = host.PlatformType.SystemInt32;
                var intPtr = host.PlatformType.SystemIntPtr;
                return new Conversion()
                    {
                        Type = int32Type,
                        TypeAfterConversion = int32Type,
                        CheckNumericRange = false,
                        ValueToConvert = new VectorLength()
                            {
                                Type = intPtr,
                                Vector = expr
                            }
                    };
            }
            foreach (var field in expr.Type.ResolvedType.Fields)
            {
                if (field.Name.Value == accesor)
                {
                    return new BoundExpression() { Type = field.Type, Instance = expr, Definition = field };
                }
            }
            foreach (var method in expr.Type.ResolvedType.Methods)
            {
                if (method.Name.ToString().EndsWith(candMethod))
                {
                    return new MethodCall() { ThisArgument = expr, MethodToCall = method, Type = method.Type };
                }
            }
            return null;
        }

        public static IExpression GenerateExpressionFromString(string strExpr, IMetadataHost host, IMethodDefinition method)
        {
            strExpr = Normalize(strExpr);
            string rpn = GetRPN(strExpr);
            Stack<IExpression> stVals = new Stack<IExpression>();
            RPNEnumerator rpnEnum = new RPNEnumerator(rpn);
            var int32Type = host.PlatformType.SystemInt32;
            var boolType = host.PlatformType.SystemBoolean;
            while (rpnEnum.MoveNext())
            {
                RPNValue rpnVal = (RPNValue)rpnEnum.Current;
                if (rpnVal.type == RPNValueType.OPERAND)
                {
                    if (reNum.IsMatch(rpnVal.val))
                    {
                        stVals.Push(new CompileTimeConstant() { Type = int32Type, Value = int.Parse(rpnVal.val.Replace(':', '-')) });
                    }
                    else
                    {
                        IExpression expr = null;
                        var exprParts = rpnVal.val.Split('.');
                        if (exprParts[0] == "this")
                        {
                            expr = new ThisReference() { Type = method.ContainingType.ResolvedType };
                        }
                        else
                        {
                            foreach (var arg in method.Parameters)
                            {
                                if (arg.Name.Value == exprParts[0])
                                {
                                    expr = new BoundExpression() { Type = arg.Type.ResolvedType, Definition = arg };
                                    break;
                                }
                            }
                        }
                        if (expr != null && exprParts.Length > 1)
                        {
                            for (int i = 1; i < exprParts.Length; i++)
                            {
                                expr = GetExprWithAccesor(expr, exprParts[i], host);
                            }
                        }
                        if (expr == null || expr.Type.ResolvedType != int32Type.ResolvedType)
                        {
                            return null;
                        }
                        stVals.Push(expr);
                    }
                }
                else
                {
                    var v1 = stVals.Pop();
                    var v2 = stVals.Pop();
                    if      (rpnVal.val == "+") stVals.Push(new Addition() { Type = int32Type, LeftOperand = v2, RightOperand = v1 });
                    else if (rpnVal.val == "-") stVals.Push(new Subtraction() { Type = int32Type, LeftOperand = v2, RightOperand = v1 });
                    else if (rpnVal.val == "*") stVals.Push(new Multiplication() { Type = int32Type, LeftOperand = v2, RightOperand = v1 });
                    else if (rpnVal.val == "/") stVals.Push(new Division() { Type = int32Type, LeftOperand = v2, RightOperand = v1 });
                    else if (rpnVal.val == "=") stVals.Push(new Equality() { Type = int32Type, LeftOperand = v2, RightOperand = v1 });
                    else if (rpnVal.val == "&") stVals.Push(new Conditional() { Type = boolType, Condition = v2, ResultIfTrue = v1, ResultIfFalse = new CompileTimeConstant() { Type = int32Type, Value = 0 } });
                    else if (rpnVal.val == "|") stVals.Push(new Conditional() { Type = boolType, Condition = v2, ResultIfTrue = new CompileTimeConstant() { Type = int32Type, Value = 1 }, ResultIfFalse = v1 });
                    else if (rpnVal.val == ">") stVals.Push(new GreaterThan() { Type = boolType, LeftOperand = v2, RightOperand = v1 });
                    else if (rpnVal.val == "<") stVals.Push(new LessThan() { Type = boolType, LeftOperand = v2, RightOperand = v1 });
                    else if (rpnVal.val == "%") stVals.Push(new GreaterThanOrEqual() { Type = boolType, LeftOperand = v2, RightOperand = v1 });
                    else if (rpnVal.val == "#") stVals.Push(new LessThanOrEqual() { Type = boolType, LeftOperand = v2, RightOperand = v1 });
                    else return null;
                }
            }
            if (stVals.Count > 0)
            {
                return stVals.Pop();
            }
            return null;
        }

        public static double Eval(string expression)
        {
            expression = Normalize(expression);
            string rpn = GetRPN(expression);
            Stack stVals = new Stack();
            RPNEnumerator rpnEnum = new RPNEnumerator(rpn);
            while (rpnEnum.MoveNext())
            {
                RPNValue rpnVal = (RPNValue)rpnEnum.Current;
                if (rpnVal.type == RPNValueType.OPERAND)
                {
                    stVals.Push(Double.Parse(rpnVal.val, new CultureInfo("en-US")));
                }
                else
                {
                    double v2 = (double)stVals.Pop();
                    double v1 = (double)stVals.Pop();
                    if (rpnVal.val == "+")
                    {
                        stVals.Push(v1 + v2);
                    }
                    else if (rpnVal.val == "-")
                    {
                        stVals.Push(v1 - v2);
                    }
                    else if (rpnVal.val == "*")
                    {
                        stVals.Push(v1 * v2);
                    }
                    else if (rpnVal.val == "/")
                    {
                        stVals.Push(v1 / v2);
                    }
                }
            }
            return (double)stVals.Pop();
        }
    }
}
