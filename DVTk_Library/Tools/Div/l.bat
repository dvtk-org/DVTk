: @echo off

: Save environment
setlocal
: Arguments
set InputDir=%1
set InputName=%2
PATH=%PATH%;%cd%;%cd%\..\..\Tools\DIV

echo Input directory : %InputDir%
echo Input name      : %InputName%

: Check InputDir exist
if exist %InputDir% goto continue1
mkdir %InputDir%
:continue1

cd %InputDir%

:process
echo Processing : "flex -l -t %InputName%.l | sed -f yy-sed > %InputName%.cpp"
echo ****** %PATH%
flex -l -t %InputName%.l | sed -f yy-sed > %InputName%.cpp

:end
: restore environment
endlocal