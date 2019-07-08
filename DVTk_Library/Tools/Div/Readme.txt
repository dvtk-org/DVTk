Tools Project shared files readme.txt :
=======================================

This directory contains the following files :


	l.bat
	y.bat


1. l.bat

	Description : Wrapper commandfile which takes a lex syntax file
                      (*.l) as input and generates a C source code file
		      which can be used within Visual Studio IDE.


	Arguments   : 1 : $(InputDir) : full pathname relative to 
                                    project <project>
                                    directory.
		          2 : $(InputName): filename without extension


	Input condition :
		      0. Tool must be located within the 
			     Projects\Shared\bin directory
		      1. $(InputDir)\$(InputName).l exist
		      2. $(InputDir)\yy-sed exist
		      3. $(Shared)\bin\sed.exe exist
		      4. $(Shared)\bin\flex.exe exist
		

	Output condition :
		      1. $(InputDir)\$(InputName).c has been created.


2. y.bat

	Description : Wrapper commandfile which takes a yacc syntax file
                  (*.y) as input and generates a C source code file
		          and a y.tab.h header file, both files can be used
		          within the visual studio IDE.


	Arguments   : 1 : $(InputDir) : full pathname relative to 
                                    project <project>
                                    directory.
		          2 : $(InputName): filename without extension


	Input condition :
		      0. Tool must be located within the 
			     Projects\<project> directory
		      1. $(InputDir)\$(InputName).y exist
		      2. $(InputDir)\yy-sed exist
		      3. $(Shared)\bin\sed.exe exist
		      4. $(Shared)\bin\bison.exe exist
		

	Output condition :
		      1. $(InputDir)\$(InputName).c has been created.




3. Additonal info :
	1. $(SHARED) must be set to the project Shared directory
	2. The tool environment must have been created.
	   This will provide the correct lex- and yacc tools
       for the software development environment.