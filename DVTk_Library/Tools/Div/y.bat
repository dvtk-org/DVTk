@echo off
@REM Arguments
set InputDir=%1
set InputName=%2
set bison_command=%cd%\..\..\Tools\DIV\bison

echo === BISON ===
echo Input directory : %InputDir%
echo Input name      : %InputName%

if not exist %InputDir% mkdir %InputDir%
cd %InputDir%

@REM Clean up left over files
if exist y.tab.c del y.tab.c
if exist y.tab.h del y.tab.h
if exist %InputName%.cpp del %InputName%.cpp
if exist %InputName%.cpp.h del %InputName%.cpp.h

set processing=%bison_command% -d -y -v %InputName%.y
echo Processing: %processing%
%processing%

if not exist y_tab.c echo Generation y_tab.c failed && exit 1
if not exist y_tab.h echo Generation y_tab.h failed && exit 1
@REM if exist y_tab.h echo y_tab.h generated

@REM Use sed to rename function names in y.tab.c file

echo Creating: %InputName%.cpp
sed -f yy-sed y_tab.c > %InputName%.cpp
del y_tab.c

@REM Use sed to rename function names in y_tab.h file
echo Creating: %InputName%.cpp.h
sed -f yy-sed y_tab.h > %InputName%.cpp.h
del y_tab.h

FOR /F %%A in ("%InputName%.cpp") DO SET _existingFileSize=%%~zA
ECHO File size of %InputName%.cpp: %_existingFileSize%
if %_existingFileSize% LEQ 100 echo %InputName%.cpp is empty && exit 1
