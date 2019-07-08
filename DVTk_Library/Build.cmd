REM argument 1: major version
REM argument 2: minor version
REM argument 3: build version
REM argument 4: revision version

cd /d %~dp0

SET DEVENV="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"
SET ZIP="C:\Program Files\7-zip\7z.exe"

SET RELEASE_DIR=Releases\%1_%2_%3_%4
IF EXIST %RELEASE_DIR% (
@echo Directory %RELEASE_DIR% already exists. Ending batch file.
goto END
)
mkdir Releases\%1_%2_%3_%4

SET ZIP_ROOT_DIR=Releases\Zip
IF EXIST %ZIP_ROOT_DIR% (
rmdir /S /Q %ZIP_ROOT_DIR%
)
mkdir Releases\Zip

SET ZIP_RELEASE_DIR=%ZIP_ROOT_DIR%\DVTk_Library\Releases\%1_%2_%3_%4
mkdir %ZIP_ROOT_DIR%\DVTk_Library
mkdir %ZIP_ROOT_DIR%\DVTk_Library\Releases
mkdir %ZIP_ROOT_DIR%\DVTk_Library\Releases\%1_%2_%3_%4



REM --- Set the correct version of all AssemblyInfo.cs files.

SetVersion source %1.%2.%3.%4



REM --- Create directories and copy source files.

copy "DVTk Library.sln" %RELEASE_DIR%
copy "ChangeHistoryLibrary.txt" %RELEASE_DIR%

mkdir %RELEASE_DIR%\Debug
mkdir %RELEASE_DIR%\Release


mkdir %RELEASE_DIR%\Source
xcopy /E /I Source %RELEASE_DIR%\Source

mkdir %RELEASE_DIR%\Resources
xcopy /E /I Resources %RELEASE_DIR%\Resources

mkdir %RELEASE_DIR%\Documentation\API
xcopy /E /I Documentation\API\API.shfbproj %RELEASE_DIR%\Documentation\API

mkdir %RELEASE_DIR%\Tools
mkdir %RELEASE_DIR%\Tools\Div
xcopy Tools\Div %RELEASE_DIR%\Tools\Div



REM --- Build the Release configuration.

echo >> "Releases\release_build_logging_%1_%2_%3_%4.txt"
%DEVENV% "%RELEASE_DIR%\DVTk Library.sln" /rebuild Release /project "DVTk Library" /out "Releases\release_build_logging_%1_%2_%3_%4.txt"



REM --- Build the Debug configuration.

echo >> "Releases\debug_build_logging_%1_%2_%3_%4.txt"
%DEVENV% "%RELEASE_DIR%\DVTk Library.sln" /rebuild Debug /project "DVTk Library" /out "Releases\debug_build_logging_%1_%2_%3_%4.txt"



REM --- Build the NUnit tests.

%DEVENV% "%RELEASE_DIR%\Source\Assemblies\DVTk High Level Interface\DVTk High Level Interface.sln" /rebuild Release /project "DVTk High Level Interface - NUnit Tests"



REM --- Create a zip file for the normal release.

mkdir %ZIP_RELEASE_DIR%\Release

xcopy /E %RELEASE_DIR%\Release %ZIP_RELEASE_DIR%\Release
copy %RELEASE_DIR%\"DVTk Library.sln" %ZIP_RELEASE_DIR%

cd %ZIP_ROOT_DIR%
%ZIP% a -tzip -r DVTk_minimum_Library_%1_%2_%3_%4.zip DVTk_Library
cd ..
cd ..
copy %ZIP_ROOT_DIR%\DVTk_minimum_Library_%1_%2_%3_%4.zip Releases\DVTk_minimum_Library_%1_%2_%3_%4.zip



REM --- Create a zip file for the developer release.
REM --- The release files are already present.

mkdir %ZIP_RELEASE_DIR%\Debug
xcopy /E %RELEASE_DIR%\Debug %ZIP_RELEASE_DIR%\Debug

mkdir %ZIP_RELEASE_DIR%\Source
xcopy /S Source %ZIP_RELEASE_DIR%\Source

mkdir %ZIP_RELEASE_DIR%\Resources
xcopy /S Resources %ZIP_RELEASE_DIR%\Resources

mkdir %ZIP_RELEASE_DIR%\Tools
mkdir %ZIP_RELEASE_DIR%\Tools\Div
xcopy /S Tools\Div %ZIP_RELEASE_DIR%\Tools\Div

cd %ZIP_ROOT_DIR%
%ZIP% a -tzip -r DVTk_full_Library_%1_%2_%3_%4.zip DVTk_Library
cd ..
cd ..
copy %ZIP_ROOT_DIR%\DVTk_full_Library_%1_%2_%3_%4.zip Releases\DVTk_full_Library_%1_%2_%3_%4.zip


:END

