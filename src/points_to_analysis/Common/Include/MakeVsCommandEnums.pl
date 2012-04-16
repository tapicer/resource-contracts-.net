open(INFILE,  "$ARGV[0]") or die "Can't open $ARGV[0] : $!";

while (<INFILE>) {
  if (/#define cmdid\s*([^\s]*)\s*([^\s]*)(.*)/) {
    print "    $1 = $2, $3\n";
  } elsif (/#define ECMD_\s*([^\s]*)\s*([^\s]*)(.*)/) {
    print "    $1 = $2, $3\n";
  } elsif (/#define\s*([^\s]*)\s*([^\s]*)(.*)/) {
    print "    $1 = $2, $3\n";
  } else {
    print $_;
  }
}