#!perl.exe

use Try::Tiny;
try {

    use  File::BOM;
    use cxx Config::IniFiles;

} catch {
    exit 1;    
};

exit 0;
