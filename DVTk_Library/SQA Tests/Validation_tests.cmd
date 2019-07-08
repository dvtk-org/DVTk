REM Argument 1: version of the release under test.

mkdir %1\Validation_tests
xcopy Validation_tests %1\Validation_tests /S

REM - Build the solution.
cd %1\Validation_tests\Solution
devenv "Dvtk Validation.sln" /REBUILD RELEASE /OUT Out.txt

cd ..
cd ..
cd ..

copy %1\Validation_tests\Solution\bin\x86\Release\DvtkValidation.exe %1\bin
copy Validation_tests\DvtkValidation.txt %1\bin
copy Validation_tests\DvtkValidation.xslt %1\bin

copy Validation_tests\DvtkValidation.pdvt %1\Results
copy Validation_tests\DvtkValidation.ses %1\Results
mkdir %1\Results\DvtkValidation
copy Validation_tests\ValidateDvt.vbs %1\Results\DvtkValidation

cd %1\bin
DvtkValidation.exe
cd ..
cd ..

REM PAUSE
