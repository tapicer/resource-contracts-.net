function JDate() {
  var now = new Date();
  var year = now.getFullYear();
  now = new Date(year, now.getMonth(), now.getDate());
  var nowValue = now.valueOf() / 60000 - now.getTimezoneOffset();
  if (year >= 1998) {
    var base = new Date(1998, 0, 1);
    var baseValue = base.valueOf() / 60000 - base.getTimezoneOffset();
    return (nowValue - baseValue) / 1440 + 8001;
  }
  if (year >= 1990) {
    var base = new Date(year, 0, 1);
    var baseValue = base.valueOf() / 60000 - base.getTimezoneOffset();
    return (nowValue - baseValue) / 1440 + (year - 1990) * 1000 + 1;
  }
  return 1;
}

var rmj = 7;
var rmm = 1;
var bld = JDate();

var args = WScript.Arguments;
var outStr = WScript.stdOut;
if (args.length >= 1) {
  var fso = new ActiveXObject("Scripting.FileSystemObject");
  outStr = fso.CreateTextFile(args(0), true);
}

outStr.WriteLine("using System.Reflection;");
outStr.WriteLine("using System.Runtime.CompilerServices;");
outStr.WriteLine();

outStr.WriteLine("[assembly:");
if (args.length >= 2)
  outStr.WriteLine("  AssemblyTitle(\"" + args(1) + "\"),");
if (args.length >= 3)
  outStr.WriteLine("  AssemblyDescription(\"" + args(2) + "\"),");
if (args.length >= 4)
  outStr.WriteLine("  AssemblyKeyFile(\"" + args(3) + "\"),");
if (args.length >= 5)
  outStr.WriteLine("  AssemblyDelaySign(" + args(4) + "),");
outStr.WriteLine("  AssemblyFileVersion(\"" + rmj + "." + rmm + "." + bld + ".0\"),");
outStr.WriteLine("  AssemblyVersion(\"" + rmj + "." + rmm + "." + bld + ".0\"),");
outStr.WriteLine("  AssemblyCompany(\"Microsoft Corporation\"),");
outStr.WriteLine("  AssemblyProduct(\"Microsoft® JScript .NET\"),");
outStr.WriteLine("  AssemblyCopyright(\"© Microsoft Corporation.  All rights reserved.\"),");
outStr.WriteLine("#if ASSEMBLY_ATTRIBUTE_CLS_COMPLIANT");
outStr.WriteLine("  System.CLSCompliant(true),");
outStr.WriteLine("#endif");
outStr.WriteLine("]");
outStr.WriteLine();

outStr.WriteLine("namespace Microsoft.JScript {");
outStr.WriteLine("  class BuildVersionInfo {");
outStr.WriteLine("    public const int MajorVersion=" + rmj + ";");
outStr.WriteLine("    public const int MinorVersion=" + rmm + ";");
outStr.WriteLine("    public const int Revision=0;");
outStr.WriteLine("    public const int Build=" + bld + ";");
outStr.WriteLine("  }");
outStr.WriteLine("}");

if (args.length >= 1)
  outStr.Close();
