#!perl.exe

use Try::Tiny;
try {

    use  File::BOM;
    use  Config::IniFiles;

} catch {
    exit 1;    
};

exit 0;
