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
 
    b.	Desktop development with C++ with following Optional components:

        i.	MSVC v142 - VS 2019 C++ x64/x86 build tools

    c.	.NET desktop development

        i.	Development tools for .NET
        ii.	.NET Framework 4.8 development tools
        iii.	.NET Framework 3.5 SP1 (required to build DVTk Script Support)

2.	If you want to add or update the setup files of the installers, then in Visual Studio install Visual Studio extension: Microsoft Visual Studio Installer Projects 2022. The installers are created in the build pipeline.

3.	Clone the DICOM project from Github repository into a path which does not contain spaces (for eg: C:\Github\DICOM)

4.	Open Visual Studio and open DICOM.sln solution file. Do **NOT** upgrade/use latest version, if Visual Studio asks to upgrade a project.

5.	Right click on the DICOM solution and click *Build*.

### Generated source files

Some lexer/parser outputs are generated during toolchain runs (for example `*_lexer.cpp`, `*_parser.cpp`, `*_parser.cpp.h`, and `*_lex.cpp`, `*_parse.cpp`, `*_parse.cpp.h`).
These generated files are intentionally excluded via `.gitignore` and should not be committed.

### Errors

If you get error regarding *afx.h* (*Cannot open include file 'afx.h': No such file or directory.*), make sure to check Output tab in Visual Studio and locate the error related to afx.h. Make sure the file exists and the path is correct. If the file does not exist, download the correct MSVC version from Microsoft.
If you get error HRESULT 8000000A when building .vdproj (setup) projects, apply the following registry fix in PowerShell as Administrator:

```powershell
$instanceId = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -version "[17.0,18.0)" -property instanceId
$registryPath = "HKCU:\SOFTWARE\Microsoft\VisualStudio\17.0_$($instanceId)_Config\MSBuild"
if (!(Test-Path $registryPath)) {
    New-Item -Path $registryPath -Force | Out-Null
}
Set-ItemProperty -Path $registryPath -Name "EnableOutOfProcBuild" -Value 0 -Type DWord
```

The setup projects can only be built after all other projects have been successfully compiled.

## Building from VS Code or VS Code-derived IDEs (Cursor, Antigravity)

You **do not need to open or use the Visual Studio IDE GUI** to develop or build DVTk—you can write code and trigger builds entirely within **VS Code**, **Cursor**, **Antigravity**, or any terminal-focused editor.

### Do I still need to install Visual Studio or VS Build Tools?

**Yes, but only the background compiler toolchain and SDKs (no IDE GUI required).**

Windows C++ and .NET Framework builds require the underlying Microsoft C++ compiler (`cl.exe`), C++ MFC/ATL headers, and Windows SDK. These tools can be installed via either **Visual Studio Community/Professional** OR the standalone **Build Tools for Visual Studio**. 

Once installed, you can perform all coding, building, and debugging entirely inside **VS Code**, **Cursor**, **Antigravity**, or your terminal.

### 1. Prerequisites Check
Ensure the required build components are installed via the **Visual Studio Installer** (or **VS Build Tools Installer**):
- **Desktop development with C++** (specifically **C++ MFC for v142/v143** and **C++ ATL**)
- **Windows 10 SDK (10.0.19041.0)**
- **.NET desktop development** (.NET Framework 4.8 & 3.5 SP1)

### 2. Full Solution CLI Build
Run the following PowerShell command in the integrated terminal to compile all C++ libraries, C# assemblies, emulators, and executables:

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe' DICOM.sln /p:Configuration=Debug /p:Platform=x86 /p:WindowsTargetPlatformVersion=10.0.19041.0 /p:BscMakeEnabled=false /m
```

> **Note on Flags:**
> * `/p:Platform=x86`: DVTk targets 32-bit (`x86`) binaries.
> * `/p:WindowsTargetPlatformVersion=10.0.19041.0`: Resolves SDK header compatibility issues with `WinDNS.h`.
> * `/p:BscMakeEnabled=false`: Disables legacy source browser database generation to prevent heap errors during parallel builds (`/m`).

### 3. Single Component Build Example
To build an individual component (for example, the `Session` library):

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe' DVTk_Library\Libraries\Session\Session.vcxproj /p:Configuration=Debug /p:Platform=Win32 /p:WindowsTargetPlatformVersion=10.0.19041.0 /p:BscMakeEnabled=false
```

### 4. Output Location
All compiled binaries (executables, DLLs, static `.lib` files, and PDB debug symbols) are generated in:
```text
bin\Debug\  (or bin\Release\)
```

## NuGet package

The GitHub Actions workflow builds a NuGet package for `DvtkHighLevelInterface` and publishes it to GitHub Packages for the `dvtk-org` owner.

Feed URL:

```text
https://nuget.pkg.github.com/dvtk-org/index.json
```

Example consumer configuration is available in [NuGet.config.example](NuGet.config.example). The GitHub Packages feed requires authentication with a GitHub username and a Personal Access Token that has permission to read packages.

After adding the feed, install the package by ID:

```powershell
nuget install DvtkHighLevelInterface -Source https://nuget.pkg.github.com/dvtk-org/index.json
```

