@echo off
: Part of Dvtk Libraries - Internal Native Library Code
: Copyright © 2001-2005
: Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

: Save environment
: echo Saving environment
setlocal
: Arguments
if "%3" == "" goto twoargs
set InputDir=%1 %2
set InputName=%3
goto doneargs
:twoargs
set InputDir=%1
set InputName=%2
:doneargs
path=%path%;..\..\..\tools\div

: echo Input directory : %InputDir%
: echo Input name      : %InputName%

: echo First call y.bat
call y.bat "%InputDir%" %InputName%

: echo First call y.bat
call l.bat "%InputDir%" imagedisplay_lex

: echo Restoring environment
endlocal
