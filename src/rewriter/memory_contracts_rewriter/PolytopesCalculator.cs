using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace memory_contracts_rewriter
{
    public class PolyCond
    {
        public string Poly { get; set; }
        public string Cond { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Poly, Cond);
        }
    }

    class PolytopesCalculator
    {
        #region Public

        //PolyCond res;
        //res = PolytopesCalculator.CountPoints("1 <= i <= n", new List<string>() { "n" });
        //Console.WriteLine(res == null ? "<null>" : res.ToString());
        //res = PolytopesCalculator.CountPoints("1 <= i <= n and 1 <= j <= m", new List<string>() { "n", "m" });
        //Console.WriteLine(res == null ? "<null>" : res.ToString());
        //Console.ReadLine();
        //return 1;

        //card { [n] -> [i] : 1 <= i <= n };                          -->      { [n] -> n : n >= 1 }
        //card { [n, m] -> [i, j] : 1 <= i <= n and 1 <= j <= m };    -->      { [n, m] -> n * m : m >= 1 and n >= 1 }

        public static PolyCond CountPoints(string inv, List<string> freeVars)
        {
            inv = ToBarvinokConds(inv);
            VarsReplacementsIn(ref inv, freeVars);
            var boundVars = ExtractVars(inv).Except(freeVars);

            string freeVarsStr = string.Join(", ", freeVars);
            string boundVarsStr = string.Join(", ", boundVars);

            var query = string.Format("card {{ [{0}] -> [{1}] : {2} }};", freeVarsStr, boundVarsStr, inv);

            var barvinokResult = InvokeBarvinok(query);
        
            Regex reExtractOutput = new Regex(string.Format(@"\{{ \[{0}\] -> (.*) : (.*) \}}", freeVarsStr));

            Match m = reExtractOutput.Match(barvinokResult);

            if (m.Success)
            {
                var poly = m.Groups[1].Value;
                if (poly.Contains(';'))
                {
                    //several candidates, not useful
                    return null;
                }
                if (poly == "infty")
                {
                    //infinite points, not useful
                    return null;
                }
                return new PolyCond() { Poly = VarsReplacementsOut(PotsToMult(poly)), Cond = VarsReplacementsOut(ToCConds(m.Groups[2].Value)) };
            }

            return null;
        }

        //PolyCond res;
        //res = PolytopesCalculator.MaxOver("i * i", "1 <= i <= n", new List<string>() { "n" });
        //Console.WriteLine(res == null ? "<null>" : res.ToString());
        //res = PolytopesCalculator.MaxOver("i * j", "1 <= i <= n and 1 <= j <= m", new List<string>() { "n", "m" });
        //Console.WriteLine(res == null ? "<null>" : res.ToString());
        //Console.ReadLine();
        //return 1;

        //ub { [[n] -> [i]] -> i * i : 1 <= i <= n };                           -->    ({ [n] -> max(n^2) : n >= 1 }, True)
        //ub { [[n, m] -> [i, j]] -> i * j : 1 <= i <= n and 1 <= j <= m };     -->    ({ [n, m] -> max(n * m) : m >= 1 and n >= 1 }, True)

        public static PolyCond MaxOver(string poly, string inv, List<string> freeVars)
        {
            inv = ToBarvinokConds(inv);
            VarsReplacementsIn(ref poly, ref inv, freeVars);
            var boundVars = ExtractVars(inv).Except(freeVars);

            string freeVarsStr = string.Join(", ", freeVars);
            string boundVarsStr = string.Join(", ", boundVars);

            var query = string.Format("ub {{ [[{0}] -> [{1}]] -> {2} : {3} }};", freeVarsStr, boundVarsStr, ToBarvinokMults(poly), inv);

            var barvinokResult = InvokeBarvinok(query);

            Regex reExtractOutput = new Regex(string.Format(@"\({{ \[{0}\] -> max\((.*)\) : (.*) \}}, True\)", freeVarsStr));

            Match m = reExtractOutput.Match(barvinokResult);

            if (m.Success)
            {
                if (m.Groups[1].Value.Contains(';'))
                {
                    //several candidates, not useful
                    return null;
                }
                return new PolyCond() { Poly = VarsReplacementsOut(PotsToMult(m.Groups[1].Value)), Cond = VarsReplacementsOut(ToCConds(m.Groups[2].Value)) };
            }

            return null;
        }

        //PolyCond res;
        //res = PolytopesCalculator.SumOver("i", "1 <= i <= n && n == 10", new List<string>() { "n" });
        //Console.WriteLine(res == null ? "<null>" : res.ToString());
        //res = PolytopesCalculator.SumOver("i", "1 <= i <= n", new List<string>() { "n" });
        //Console.WriteLine(res == null ? "<null>" : res.ToString());
        //res = PolytopesCalculator.SumOver("i * i", "1 <= i <= n", new List<string>() { "n" });
        //Console.WriteLine(res == null ? "<null>" : res.ToString());
        //res = PolytopesCalculator.SumOver("i * j", "1 <= i <= n and 1 <= j <= m", new List<string>() { "n", "m" });
        //Console.WriteLine(res == null ? "<null>" : res.ToString());
        //Console.ReadLine();
        //return 1;

        //sum { [[n] -> [i]] -> i : 1 <= i <= n, n = 10 };                       -->    { [n] -> 55 : n = 10 }
        //sum { [[n] -> [i]] -> i : 1 <= i <= n };                               -->    { [n] -> (1/2 * n + 1/2 * n^2) : n >= 1 }
        //sum { [[n] -> [i]] -> i * i : 1 <= i <= n };                           -->    { [n] -> (1/6 * n + 1/2 * n^2 + 1/3 * n^3) : n >= 1 }
        //sum { [[n, m] -> [i, j]] -> i * j : 1 <= i <= n and 1 <= j <= m };     -->    { [n, m] -> ((1/4 * n + 1/4 * n^2) * m + (1/4 * n + 1/4 * n^2) * m^2) : m >= 1 and n >= 1 }

        public static PolyCond SumOver(string poly, string inv, List<string> freeVars)
        {
            inv = ToBarvinokConds(inv);
            VarsReplacementsIn(ref poly, ref inv, freeVars);
            var boundVars = ExtractVars(inv).Except(freeVars);

            string freeVarsStr = string.Join(", ", freeVars);
            string boundVarsStr = string.Join(", ", boundVars);

            var query = string.Format("sum {{ [[{0}] -> [{1}]] -> {2} : {3} }};", freeVarsStr, boundVarsStr, ToBarvinokMults(poly), inv);

            var barvinokResult = InvokeBarvinok(query);

            Regex reExtractOutput = new Regex(string.Format(@"{{ \[{0}\] -> (.*) : (.*) \}}", freeVarsStr));

            Match m = reExtractOutput.Match(barvinokResult);

            if (m.Success)
            {
                if (m.Groups[1].Value.Contains(';'))
                {
                    //several candidates, not useful
                    return null;
                }
                return new PolyCond() { Poly = VarsReplacementsOut(PotsToMult(m.Groups[1].Value)), Cond = VarsReplacementsOut(ToCConds(m.Groups[2].Value)) };
            }

            return null;
        }

        //List<PolyCond> res;
        //res = PolytopesCalculator.MaxPoly(new List<string>() { "n * n", "n" }, new List<string>() { "n >= 1" }, new List<string>() { "n" });
        //Console.WriteLine(res == null ? "<null>" : res[0].ToString());
        //res = PolytopesCalculator.MaxPoly(new List<string>() { "n * n", "n + 2" }, new List<string>() { "n >= 1" }, new List<string>() { "n" });
        //Console.WriteLine(res == null ? "<null>" : res[0].ToString());
        //res = PolytopesCalculator.MaxPoly(new List<string>() { "n * n + m", "n + m" }, new List<string>() { "n >= 1 && m >= 1" }, new List<string>() { "n", "m" });
        //Console.WriteLine(res == null ? "<null>" : res[0].ToString());
        //Console.ReadLine();
        //return 1;

        //{ [n] -> max(n * n) : n >= 1 } . { [n] -> max(n) : n >= 1 };                                 -->     { [n] -> max(n^2) : n >= 1 }
        //{ [n] -> max(n * n) : n >= 1 } . { [n] -> max(n + 2) : n >= 1 };                             -->     { [n] -> max(n^2, (2 + n)) : n >= 1 } --> null
        //{ [n, m] -> max(n * n + m) : n >= 1, m >= 1 } . { [n, m] -> max(n + m) : n >= 1, m >= 1 };   -->     { [n, m] -> max((n^2 + m)) : n >= 1 and m <= n and m >= 1 }

        protected static Regex reTokens = new Regex(@"[a-zA-Z0-9_]+", RegexOptions.Compiled);

        public static PolyCond MaxPoly(List<string> polys, List<string> conds, List<string> vars)
        {
            VarsReplacementsIn(polys, conds, new List<string>());
            var newConds = new List<string>();
            foreach (var cond in conds.Distinct())
            {
                var newCond = ToBarvinokConds(cond);
                newConds.Add(newCond);
                var condVars = ExtractVars(newCond);
                foreach (var var in condVars)
                {
                    if (!vars.Contains(var))
                    {
                        vars.Add(var);
                    }
                }
            }

            foreach (var poly in polys)
            {
                foreach (Match match in reTokens.Matches(poly))
                {
                    var token = match.Groups[0].Value;
                    if (!reNum.IsMatch(token) && !vars.Contains(token))
                    {
                        vars.Add(token);
                    }
                }
            }

            var queries = new List<string>();
            var strConds = string.Join(" and ", newConds);
            var strVars = string.Join(", ", vars);

            if (strConds != "")
            {
                strConds = ": " + strConds;
            }

            foreach (var poly in polys)
            {
                queries.Add(string.Format("{{ [{0}] -> max({1}) {2} }}", strVars, ToBarvinokMults(poly), strConds));
            }

            var query = string.Join(" . ", queries);
            query += ";";

            var barvinokResult = InvokeBarvinok(query);

            Regex reExtractOutput = new Regex(string.Format(@"{{ \[{0}\] -> max\((.*)\) : (.*) \}}", strVars));

            Match m = reExtractOutput.Match(barvinokResult);

            if (m.Success)
            {
                var poly = m.Groups[1].Value;

                if (poly.Contains(',')) //several max polys candidates, not useful
                {
                    return null;
                }

                return new PolyCond() { Poly = VarsReplacementsOut(PotsToMult(poly)), Cond = VarsReplacementsOut(ToCConds(m.Groups[2].Value)) };
            }

            return null;
        }

        //returns true iff poly1 <= poly2
        public static bool LowerOrEqual(string poly1, string poly2, List<string> conds, List<string> vars, out string cond)
        {
            var max = MaxPoly(new List<string> { poly1, poly2 }, conds, vars);

            cond = "false";

            if (max != null)
            {
                var maxPoly = max.Poly;

                var stdPoly2 = MaxPoly(new List<string> { poly2 }, conds, vars);

                if (stdPoly2 != null &&
                    maxPoly == stdPoly2.Poly)
                {
                    cond = max.Cond;
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Protected

        protected static string ToBarvinokConds(string expr)
        {
            return expr
                       .Replace("==", " = ")
                       .Replace("&&", " and ")
                       .Replace("||", " or ");
        }

        protected static string ToCConds(string expr)
        {
            return expr
                       .Replace(" = ", " == ")
                       .Replace(" and ", " && ")
                       .Replace(" or ", " || ");
        }

        protected static Regex rePot = new Regex(@"([a-zA-Z_0-9]+)\^([0-9]+)", RegexOptions.Compiled);
        protected static Regex reDiv = new Regex(@"1/([^ ]*)[ ]\*[ ]([a-zA-Z_0-9]+(\^[0-9]+)?)", RegexOptions.Compiled);

        protected static string PotsToMult(string expr)
        {
            //converts 1/x * b^p to b^p / x
            expr = reDiv.Replace(expr, "$2 / $1");

            //converts every b^p to b*...*b with p multiplications
            expr = rePot.Replace(expr, delegate(Match m)
                {
                    var elems = new List<string>();
                    var b = m.Groups[1].Value;
                    var p = int.Parse(m.Groups[2].Value);
                    for (int i = 1; i <= p; i++)
                    {
                        elems.Add(b);
                    }
                    return string.Join(" * ", elems);
                });

            return expr;
        }

        protected static Regex reToBarvinokMultsPar = new Regex(@"(\(.*?\))[ ]*/[ ]*([\d]+)", RegexOptions.Compiled);
        protected static Regex reToBarvinokMultsNoPar = new Regex(@"([a-zA-Z_0-9\.]+)[ ]*/[ ]*(\d)+", RegexOptions.Compiled);

        //converts (expr) / x to 1/x * (expr)
        protected static string ToBarvinokMults(string expr)
        {
            expr = reToBarvinokMultsPar.Replace(expr, "1/$2 * $1");
            expr = reToBarvinokMultsNoPar.Replace(expr, "1/$2 * $1");
            return expr;
        }

        protected static Regex reExtractVars = new Regex("<=?|>=?|and|or|=", RegexOptions.Compiled);
        protected static Regex reNum = new Regex("[0-9]+", RegexOptions.Compiled);

        protected static List<string> ExtractVars(string expr)
        {
            List<string> vars = new List<string>();
            string[] parts = reExtractVars.Split(expr);

            string var;
            foreach (var part in parts)
            {
                var = part.Trim();
                if (!reNum.Match(var).Success)
                {
                    vars.Add(var);
                }
            }

            return vars;
        }

        protected static Regex reTokensWithDot = new Regex(@"([a-zA-Z_0-9]+)\.[a-zA-Z_0-9\.]+", RegexOptions.Compiled);

        protected static void VarsReplacementsIn(ref string cond, List<string> freeVars)
        {
            var polys = new List<string>();
            var conds = new List<string>();
            conds.Add(cond);
            VarsReplacementsIn(polys, conds, freeVars);
            cond = conds[0];
        }

        protected static void VarsReplacementsIn(ref string poly, ref string cond, List<string> freeVars)
        {
            var polys = new List<string>();
            var conds = new List<string>();
            polys.Add(poly);
            conds.Add(cond);
            VarsReplacementsIn(polys, conds, freeVars);
            poly = polys[0];
            cond = conds[0];
        }

        protected static void VarsReplacementsIn(List<string> polys, List<string> conds, List<string> freeVars)
        {
            for (int i = 0; i < polys.Count; i++)
            {
                foreach (Match match in reTokensWithDot.Matches(polys[i]))
                {
                    var token = match.Groups[0].Value.Replace(".", "__dot__");
                    var pref = match.Groups[1].Value;
                    if (freeVars.Contains(pref) && !freeVars.Contains(token))
                    {
                        freeVars.Add(token);
                    }
                }
                polys[i] = polys[i].Replace(".", "__dot__");
            }
            for (int i = 0; i < conds.Count; i++)
            {
                foreach (Match match in reTokensWithDot.Matches(conds[i]))
                {
                    var token = match.Groups[0].Value.Replace(".", "__dot__");
                    var pref = match.Groups[1].Value;
                    if (freeVars.Contains(pref) && !freeVars.Contains(token))
                    {
                        freeVars.Add(token);
                    }
                }
                conds[i] = conds[i].Replace(".", "__dot__");
            }
        }

        protected static List<string> VarsReplacementsOut(List<string> exprs)
        {
            List<string> retExprs = new List<string>();
            foreach (var expr in exprs)
            {
                retExprs.Add(VarsReplacementsOut(expr));
            }
            return retExprs;
        }

        protected static string VarsReplacementsOut(string expr)
        {
            return expr.Replace("__dot__", ".");
        }

        protected static string InvokeBarvinok(string query)
        {
            ProcessStartInfo start = new ProcessStartInfo();

            start.FileName = GetCodeContractsInstallDir() + @"Bin\iscc.exe";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardInput = true;
            start.RedirectStandardError = true;

            Process process = Process.Start(start);

            process.StandardInput.WriteLine(query);

            process.StandardInput.Close();

            string errOutput = process.StandardError.ReadToEnd();

            if (errOutput != null && errOutput.Trim().Length > 0)
            {
                return "";
            }

            return process.StandardOutput.ReadToEnd();
        }

        protected static string GetCodeContractsInstallDir()
        {
            return Environment.GetEnvironmentVariable("CodeContractsInstallDir");
        }

        #endregion
    }
}
