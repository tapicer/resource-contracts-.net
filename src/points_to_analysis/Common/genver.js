/*
cscript //nologo genver.js <verfile> [<namespace>]

This script extracts the version info from a slm version file and emits an
assembly version command line option for the C# compiler.

 e.g. 
    cscript //nologo genver.js D:\vs\src\common\inc\version.h Microsoft.JScript
  The following is written to stdout.
    
    namespace Microsoft.JScript {
    class BuildVersionInfo {
        public const int MajorVersion=7;
        public const int MinorVersion=0;
        public const int Revision=0;
        public const int Build=8903;
    }
    }
    
  The <verfile> is a standard SLM version file and should have info like the following.
    #define rmj     7
    #define rmm     0
    #define rup     8903
    #define szVerName   ""
    #define szVerUser   "ROKYU01-DEV"
*/

var args = new Object;

var verfile, namespace;
if (2 < WScript.Arguments.length) {
    WScript.Echo("USAGE: cscript //nologo genver.js <verfile> [<namespace>]");
    WScript.Quit(10);
}
if (1 <= WScript.Arguments.length)
    verfile = WScript.Arguments(0);
if (2 == WScript.Arguments.length)
    namespace = WScript.Arguments(1);

var fso = new ActiveXObject("Scripting.FileSystemObject")
var text = fso.OpenTextFile(verfile).ReadAll()

var rmj = /#define rmj[\s]*([\d]*)/i.exec(text)[1]
var rmm = /#define rmm[\s]*([\d]*)/i.exec(text)[1]
var rup = /#define rup[\s]*([\d]*)/i.exec(text)[1]

if (typeof(namespace) == "string")
    WScript.Echo('namespace '+ namespace + ' {');
WScript.Echo(
    'class BuildVersionInfo {\n'
    +'    public const int MajorVersion='+rmj+';\n'
    +'    public const int MinorVersion='+rmm+';\n'
    +'    public const int Revision=0;\n'
    +'    public const int Build='+rup+';\n'
    +'}');
if (typeof(namespace) == "string")
    WScript.Echo('}');

