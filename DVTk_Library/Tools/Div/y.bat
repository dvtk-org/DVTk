@echo off

: Save environment
: echo Saving environment
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

: echo Changing directory to : %InputDir%
cd %InputDir%

: echo Delete y.tab.c if exist
if not exist y.tab.c goto continue2
del y.tab.c
:continue2

: echo Delete y.tab.h if exist
if not exist y.tab.h goto continue3
del y.tab.h
:continue3

: debug line
set processing=bison -d -y -v %InputName%.y
: set processing=bison -d %InputName%.y
echo Processing : "%processing%"
%processing%

if exist y_tab.c goto continue4
echo Generation y_tab.c failed
exit 1
:continue4

if exist y_tab.h goto continue5
echo Generation y_tab.h failed
exit 1
:continue5
: echo Outputs : y_tab.c and y_tab.h

: echo Use sed to rename function names in y.tab.c file
: echo Processing : sed -f yy-sed y_tab.c redirected to  %InputName%.cpp
sed -f yy-sed y_tab.c > %InputName%.cpp

: echo Removing y_tab.c
del y_tab.c

: echo Use sed to rename function names in y_tab.h file
: echo Processing : sed -f yy-sed y_tab.h redirected to y_tab.h.new
sed -f yy-sed y_tab.h > y_tab.h.new

: echo Copy y_tab.h.new to %InputName%.cpp.h
set processing=copy y_tab.h.new %InputName%.cpp.h
: echo Processing %processing%
%processing% 1>nul

: echo Delete y_tab.h.new
set processing=del y_tab.h.new
: echo Processing %processing%
%processing%

: echo Delete y_tab.h
set processing=del y_tab.h
: echo Processing %processing%
%processing%

:end

: echo Restoring environment
endlocal
