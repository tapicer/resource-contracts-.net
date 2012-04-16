var now = new Date();
var year = now.getFullYear();
now = new Date(year, now.getMonth(), now.getDate());
var nowValue = now.valueOf() / 60000 - now.getTimezoneOffset();
var newDate;
if (year >= 1998) {
  var base = new Date(1998, 0, 1);
  var baseValue = base.valueOf() / 60000 - base.getTimezoneOffset();
  newDate = (nowValue - baseValue) / 1440 + 8001;
} else if (year >= 1990) {
  var base = new Date(year, 0, 1);
  var baseValue = base.valueOf() / 60000 - base.getTimezoneOffset();
  newDate = (nowValue - baseValue) / 1440 + (year - 1990) * 1000 + 1;
} else
  newDate = 1;

if (WScript.Arguments.length < 1)
  WScript.Echo(newDate);
else {
  var fso = new ActiveXObject("Scripting.FileSystemObject");
  var name = WScript.Arguments(0);
  var oldDate;
  try {
    var file = fso.OpenTextFile(name, 1);
    oldDate = file.ReadAll();
    file.Close();
  } catch (err) { }
  if (oldDate != newDate) {
    var file = fso.CreateTextFile(name, true);
    file.WriteLine(newDate);
    file.Close();
  }
}
