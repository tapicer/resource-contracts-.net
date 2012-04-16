using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace cccheck_mod
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 1 && args[0].Length > 0 && args[0][0] == '@')
            {
                StreamReader sr = new StreamReader(args[0].Substring(1));
                string strArgs = sr.ReadToEnd().Trim();
                sr.Close();
                var resByPass = InvokeProc(System.Reflection.Assembly.GetExecutingAssembly().Location, strArgs);
                Console.Error.WriteLine(resByPass.Item1);
                return resByPass.Item2;
            }
            string assemblyPath = args[args.Length - 1];
            var libPaths = new List<string>();
            var libPathsPrefix = "-libPaths:";
            foreach (var arg in args)
            {
                if (arg.StartsWith(libPathsPrefix))
                {
                    var paths = arg.Substring(libPathsPrefix.Length).Split(Path.PathSeparator);
                    foreach (var path in paths)
                    {
                        if (path.Length > 0)
                        {
                            libPaths.Add(path.Trim());
                        }
                    }
                }
            }
            var libPathsStr = String.Join(Path.PathSeparator.ToString(), libPaths.ToArray<string>());

            //rewrite the assembly reading memory contracts and injecting native contracts
            var resRewriter = InvokeProc(GetCodeContractsInstallDir() + @"Bin\memory_contracts_rewriter.exe", String.Format("\"{0}\" \"{1}\"", assemblyPath, libPathsStr));

            var checkModifiedAssembly = true;
            if (resRewriter.Item2 == 1)
            {
                Console.Error.WriteLine(resRewriter.Item1);
                return 1;
            }
            else if (resRewriter.Item2 == 2)
            {
                checkModifiedAssembly = false;
            }

            if (checkModifiedAssembly)
            {
                //regenerate the contracts assembly (if existing) because it may have new contracts
                var contractsAssemblyPath = assemblyPath
                    .Replace(@"obj\Debug\Decl\", @"bin\Debug\CodeContracts\")
                    .Replace(@"obj\Release\Decl\", @"bin\Debug\CodeContracts\");
                contractsAssemblyPath = Path.ChangeExtension(contractsAssemblyPath, ".Contracts.dll");

                if (File.Exists(contractsAssemblyPath))
                {
                    var resCcrefgen = InvokeProc(
                        GetCodeContractsInstallDir() + @"Bin\ccrefgen.exe",
                        String.Format("/pdb /out:\"{0}\" \"{1}\"", contractsAssemblyPath, Path.ChangeExtension(assemblyPath, ".mod.dll")));

                    if (resCcrefgen.Item2 == 1)
                    {
                        Console.Error.WriteLine(resCcrefgen.Item1);
                        return 1;
                    }
                }
            }

            //call cccheck with the rewritten assembly
            var argsStr = String.Join("\" \"", args).Replace(assemblyPath, Path.ChangeExtension(assemblyPath, (checkModifiedAssembly ? ".mod" : "") + ".dll"));
            var resCccheck = InvokeProc(GetCodeContractsInstallDir() + @"Bin\cccheck.exe", "\"" + argsStr + "\"");

            if (resCccheck.Item2 == 0)
            {
                if (checkModifiedAssembly)
                {
                    var lines = resCccheck.Item1.Split(Environment.NewLine.ToCharArray());
                    var newLines = new List<string>();

                    var addLines = resRewriter.Item1.Split(Environment.NewLine.ToCharArray());
                    var linesAdded = false;

                    foreach (var line in lines)
                    {
                        if (!linesAdded && line == "Close")
                        {
                            foreach (var addLine in addLines)
                            {
                                if (addLine.Trim().Length > 0)
                                {
                                    newLines.Add(addLine);
                                }
                            }
                            linesAdded = true;
                        }
                        if (line.Trim().Length > 0)
                        {
                            newLines.Add(line);
                        }
                    }

                    Console.Error.WriteLine(string.Join(Environment.NewLine, newLines));
                }
                else
                {
                    Console.Error.WriteLine(resCccheck.Item1);
                }
            }
            else
            {
                Console.Error.WriteLine(resCccheck.Item1);
            }

            return resCccheck.Item2;
        }

        protected static string GetCodeContractsInstallDir()
        {
            return Environment.GetEnvironmentVariable("CodeContractsInstallDir");
        }

        protected static Tuple<string, int> InvokeProc(string path, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();

            start.FileName = path;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.Arguments = args;

            Process process = Process.Start(start);

            string stdOutput = process.StandardOutput.ReadToEnd();
            string errOutput = process.StandardError.ReadToEnd();

            return new Tuple<string, int>(errOutput.Trim().Length > 0 ? errOutput : stdOutput, process.ExitCode);
        }
    }
}
