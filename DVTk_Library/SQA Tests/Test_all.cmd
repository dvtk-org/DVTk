REM Argument 1: version of the release under test, supplied as a_b_c_d.

mkdir %1
mkdir %1\Results

call copy_bin.cmd %1
call copy_definitions %1

call Validation_tests.cmd %1
REM call Example_directory_tests.cmd %1
