@echo off
: Part of Dvtk Libraries - Internal Native Library Code
: Copyright � 2001-2005
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
call l.bat "%InputDir%" %InputName%

echo Creating multiple lexers
:
: NOTE
: All 'script*' entries are changed in 'script1*', 'script2*' etc for 
: the corresponding generated lexers.
: Then the 'script1_lexer.l' entries are renamed back to 'script_lexer.l'
: to aid debugging.
: (The generated script1_lexer.cpp contains #line nnn script1_lexer.l directives such that the
:  debugger knows which file the generated cpp files originates from. 
:  As we are duplicating the generated file and we only have 1 original file (script_lexer.l)
:  we have to rename the script1_lexer.l entry back! This holds for all copied lexers.
:
sed -e s/script/script1/g -e s/script1_lexer/script_lexer/g "%InputDir%\script_lexer.cpp" > "%InputDir%\script1_lexer.cpp"


sed -e s/script/script2/g -e s/script2_lexer/script_lexer/g "%InputDir%\script_lexer.cpp" > "%InputDir%\script2_lexer.cpp"


sed -e s/script/script3/g -e s/script3_lexer/script_lexer/g "%InputDir%\script_lexer.cpp" > "%InputDir%\script3_lexer.cpp"


sed -e s/script/script4/g -e s/script4_lexer/script_lexer/g "%InputDir%\script_lexer.cpp" > "%InputDir%\script4_lexer.cpp"


: echo Restoring environment
endlocal
