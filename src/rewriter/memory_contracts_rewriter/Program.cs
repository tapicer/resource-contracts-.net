using System;
using Microsoft.Cci;
using System.IO;
using Microsoft.Cci.ILToCodeModel;
using Microsoft.Cci.MutableCodeModel;
using Microsoft.Cci.MutableContracts;
using System.Collections.Generic;

namespace memory_contracts_rewriter
{
    class Program
    {
        static int Main(string[] args)
        {
            //for debugging
            //args = new string[] { @"C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\example_with_contracts\Example\obj\Debug\Decl\Example.dll", @"C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\Contracts.Contract.Memory\;C:\Program Files (x86)\Microsoft\Contracts\PublicAssemblies\v3.5\;C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.5\;C:\Windows\Microsoft.NET\Framework\v2.0.50727\;C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\Contracts.Contract.Memory\CodeContracts;C:\Program Files (x86)\Microsoft\Contracts\PublicAssemblies\v3.5\CodeContracts;C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.5\CodeContracts;C:\Windows\Microsoft.NET\Framework\v2.0.50727\CodeContracts;C:\Program Files (x86)\Microsoft\Contracts\Contracts\v3.5" };
            //args = new string[] { @"C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\example_with_contracts_Person\PersonExample\obj\Debug\Decl\PersonExample.dll" };
            //args = new string[] { @"C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\experiments\experiments\obj\Debug\Decl\experiments.dll" };
            //args = new string[] { @"C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\example_cross_assembly\Assembly1\obj\Debug\Decl\Assembly1.dll", @"C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\example_cross_assembly\Assembly2\bin\Debug\;C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\Contracts.Contract.Memory\;C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\;C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\example_cross_assembly\Assembly2\bin\Debug\CodeContracts;C:\Users\Jonathan Tapicer\Desktop\tesis\cci_rewriter\Contracts.Contract.Memory\CodeContracts;C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\CodeContracts;C:\Program Files (x86)\Microsoft\Contracts\Contracts\.NETFramework\v4.0" };
            //args = new string[] { @"C:\TESIS\tesis\cci_rewriter\example_with_contracts_Person\PersonExample\obj\Debug\Decl\PersonExample.dll", @"C:\TESIS\tesis\cci_rewriter\Contracts.Contract.Memory\;C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\;C:\TESIS\tesis\cci_rewriter\Contracts.Contract.Memory\CodeContracts;C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\CodeContracts;C:\Program Files\Microsoft\Contracts\Contracts\.NETFramework\v4.0" };

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: memory_contracts_rewriter.exe [assembly_full_path] [lib_paths]");
                return 1;
            }

            var assemblyFullPath = args[0];
            var libPaths = new string[0];
            if (args.Length > 1)
            {
                libPaths = args[1].Split(Path.PathSeparator);
            }

            try
            {
                var host = new CodeContractAwareHostEnvironment(libPaths, true, true);

                var hasMemoryContracts = false;
                var module = host.LoadUnitFrom(assemblyFullPath) as IModule;
                foreach (var assemblyReference in module.AssemblyReferences)
                {
                    if (assemblyReference.Name.Value == "Contracts")
                    {
                        hasMemoryContracts = true;
                        break;
                    }
                }

                if (hasMemoryContracts)
                {
                    var memoryContractsInformation = ExtractMemoryContracts(host, assemblyFullPath);

                    var pointsToInformation = new PointsToInformation(assemblyFullPath);

                    List<ErrorReportItem> errorReportItems = RewriteAssembly(host, assemblyFullPath, memoryContractsInformation, pointsToInformation);

                    if (errorReportItems != null)
                    {
                        foreach (var errorReportItem in errorReportItems)
                        {
                            Console.WriteLine("OutputError,{0},,,false,False,False,error,{1}", errorReportItem.Message, errorReportItem.Location);
                        }
                    }
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }

            return 0;
        }

        private static MemoryContractsInformation ExtractMemoryContracts(CodeContractAwareHostEnvironment host, string assemblyFullPath)
        {
            var module = host.LoadUnitFrom(assemblyFullPath) as IModule;

            return ExtractMemoryContractsFromModule(host, module);
        }

        private static List<IModule> _visitedModules = new List<IModule>();
        private static MemoryContractsInformation ExtractMemoryContractsFromModule(CodeContractAwareHostEnvironment host, IModule module)
        {
            var memoryContractsInformation = new MemoryContractsInformation();

            if (_visitedModules.Contains(module))
            {
                return memoryContractsInformation;
            }

            _visitedModules.Add(module);

            if (module != null && module != Dummy.Module && module != Dummy.Assembly)
            {
                PdbReader pdbReader = null;
                string pdbFile = Path.ChangeExtension(module.Location, "pdb");
                if (File.Exists(pdbFile))
                {
                    using (var pdbStream = File.OpenRead(pdbFile))
                    {
                        pdbReader = new PdbReader(pdbStream, host);
                    }
                }
                module = Decompiler.GetCodeModelFromMetadataModel(host, module, pdbReader);

                var extractor = new MemoryContractsExtractor();

                extractor.Visit(module);

                memoryContractsInformation.MethodsTmpTypes = extractor.MethodsTmpTypes;
                memoryContractsInformation.MethodsTmpContracts = extractor.MethodsTmpContracts;
                memoryContractsInformation.MethodsTmpContractsWithConds = extractor.MethodsTmpContractsWithConds;
                memoryContractsInformation.MethodsRsdTypes = extractor.MethodsRsdTypes;
                memoryContractsInformation.MethodsRsdContracts = extractor.MethodsRsdContracts;
                memoryContractsInformation.MethodsRsdContractsWithConds = extractor.MethodsRsdContractsWithConds;
                memoryContractsInformation.MethodsRsdExprs = extractor.MethodsRsdExprs;

                //merge the contracts of the referenced assemblies
                foreach (var assemblyReference in module.AssemblyReferences)
                {
                    //skip assemblies that we know don't have memory contracts
                    if (!new List<string>() { "mscorlib", "Contracts", "System", "Microsoft" }.Exists(s => assemblyReference.Name.Value.StartsWith(s)))
                    {
                        var referencedAssembly = host.LoadAssembly(assemblyReference.AssemblyIdentity);

                        var referencedModuleMemoryContractsInformation = ExtractMemoryContractsFromModule(host, referencedAssembly);

                        if (referencedModuleMemoryContractsInformation.MethodsTmpTypes != null)
                        {
                            foreach (var methodName in referencedModuleMemoryContractsInformation.MethodsTmpTypes.Keys)
                            {
                                memoryContractsInformation.MethodsTmpTypes[methodName] = referencedModuleMemoryContractsInformation.MethodsTmpTypes[methodName];
                            }
                        }

                        if (referencedModuleMemoryContractsInformation.MethodsRsdTypes != null)
                        {
                            foreach (var methodName in referencedModuleMemoryContractsInformation.MethodsRsdTypes.Keys)
                            {
                                memoryContractsInformation.MethodsRsdTypes[methodName] = referencedModuleMemoryContractsInformation.MethodsRsdTypes[methodName];
                            }
                        }
                    }
                }
            }

            return memoryContractsInformation;
        }

        private static List<ErrorReportItem> RewriteAssembly(CodeContractAwareHostEnvironment host, string assemblyFullPath, MemoryContractsInformation memoryContractsInformation, PointsToInformation pointsToInformation)
        {
            var module = host.LoadUnitFrom(assemblyFullPath) as IModule;

            PdbReader pdbReader = null;
            string pdbFile = Path.ChangeExtension(module.Location, "pdb");
            if (File.Exists(pdbFile))
            {
                using (var pdbStream = File.OpenRead(pdbFile))
                {
                    pdbReader = new PdbReader(pdbStream, host);
                }
            }

            if (module != null && module != Dummy.Module && module != Dummy.Assembly)
            {
                var mutableModule = Decompiler.GetCodeModelFromMetadataModel(host, module, pdbReader);

                using (pdbReader)
                {
                    var contractProvider = Microsoft.Cci.MutableContracts.ContractHelper.ExtractContracts(host, mutableModule, pdbReader, pdbReader);

                    var mutator = new MemoryVisitor(host, pdbReader, contractProvider, memoryContractsInformation, pointsToInformation);

                    mutator.Visit(mutableModule);

                    Microsoft.Cci.MutableContracts.ContractHelper.InjectContractCalls(host, mutableModule, contractProvider, pdbReader);

                    Stream peStream = File.Create(Path.ChangeExtension(mutableModule.Location, ".mod.dll"));

                    if (pdbReader == null)
                    {
                        PeWriter.WritePeToStream(mutableModule, host, peStream);
                    }
                    else
                    {
                        using (var pdbWriter = new PdbWriter(Path.ChangeExtension(pdbFile, ".mod.pdb"), pdbReader))
                        {
                            PeWriter.WritePeToStream(mutableModule, host, peStream, pdbReader, pdbReader, pdbWriter);
                        }
                    }

                    peStream.Close();

                    return mutator.ErrorReportItems;
                }
            }

            return null;
        }
    }
}
