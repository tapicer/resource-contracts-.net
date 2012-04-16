var fso = new ActiveXObject("Scripting.FileSystemObject")
var text = fso.OpenTextFile(WScript.Arguments(0)).ReadAll()

var pattern = /@=\".*\\mscoree.dll\"/g
WScript.Echo(text.replace(pattern, "@=\"[SystemFolder]mscoree.dll\""))
