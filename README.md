# DVTk
DICOM Validation Toolkit

This GitHub project contains the complete and latest source code of the DVTk (DICOM Validation Toolkit) Open Source project. The Windows installers and forum are located on [https://www.dvtk.org/](https://www.dvtk.org/).

Note that the DVTk library and DVTk based applications are **not** for clinical use, they are only meant for testing purposes!

## Build instructions for DICOM solution (DICOM.sln)

### Build steps
1.	Make sure you have installed Visual Studio with following dependencies:

    a.	Individual components:

        i.	C++ ATL for v142
        ii.	C++ MFC for v142
    ![Visual Studio Individual Components](VisualStudioIndividualComponentsToInstall.png)
 
    b.	Desktop development with C++ with following Optional components:

        i.	MSVC v142 – VS 2019 C++ x64/x86 build tools

    c.	.NET desktop development

        i.	Development tools for .NET
        ii.	.NET Framework 4.8 development tools

2.	If you want to add or update the setup files of the installers, then in Visual Studio install Visual Studio extension: Microsoft Visual Studio Installer Projects 2022. The installers are created in the build pipeline.

3.	Clone the DICOM project from Github repository into a path which does not contain spaces (for eg: C:\Github\DICOM)

4.	Open Visual Studio and open DICOM.sln solution file. Do **NOT** upgrade/use latest version, if Visual Studio asks to upgrade a project.

5.	Right click on the DICOM solution and click ‘Build’.

### Errors

If you get error regarding ‘afx.h’ (“Cannot open include file 'afx.h': No such file or directory.”), make sure to check Output tab in Visual Studio and locate the error related to afx.h. Make sure the file exists and the path is correct. If the file does not exist, download the correct MSVC version from Microsoft.


