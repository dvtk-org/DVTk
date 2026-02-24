@echo off
: Save environment
setlocal
: Arguments
set InputDir=%1
set InputName=%2
set flex_command=%cd%\..\..\Tools\DIV\flex

echo === FLEX ===
echo Input directory : %InputDir%
echo Input name      : %InputName%

: Check InputDir exist
if not exist %InputDir% mkdir %InputDir%
cd %InputDir%

@REM Clean up left over files
if exist %InputName%.cpp del %InputName%.cpp

: Use sed to rename function names in yy-sed file
echo Creating: %InputName%.cpp
%flex_command% -l -t %InputName%.l | sed -f yy-sed > %InputName%.cpp

: restore environment
endlocal
