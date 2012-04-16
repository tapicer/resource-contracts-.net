using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace memory_contracts_installer
{
    class Program
    {
        static void Main(string[] args)
        {

            var xmlPath = GetCodeContractsInstallDir() + @"MsBuild\v4.0\Microsoft.CodeContractAnalysis.targets";

            if (File.Exists(xmlPath))
            {
                ModifyXml(xmlPath);

                xmlPath = GetCodeContractsInstallDir() + @"MsBuild\v3.5\Microsoft.CodeContractAnalysis.targets";

                if (File.Exists(xmlPath))
                {
                    ModifyXml(xmlPath);
                }
            }

            //copy the verifier files to code contracts dir
            string srcDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\memory_contracts_verifier_bin\";
            string dstDir = GetCodeContractsInstallDir() + @"Bin\";
            foreach (var file in Directory.GetFiles(srcDir))
            {
                File.Copy(file, dstDir + Path.GetFileName(file), true);
            }
        }

        private static string GetCodeContractsInstallDir()
        {
            return Environment.GetEnvironmentVariable("CodeContractsInstallDir");
        }

        private static void ModifyXml(string xmlPath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");
            XmlNode node = xml.SelectSingleNode("//ns:CodeContractRunCodeAnalysisCommand", nsmgr);
            if (node != null)
            {
                node.InnerText = "$(CodeContractsInstallDir)Bin\\cccheck_mod.exe";
                xml.Save(xmlPath);
            }
        }
    }
}
