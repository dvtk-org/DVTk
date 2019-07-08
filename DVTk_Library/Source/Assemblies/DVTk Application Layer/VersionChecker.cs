// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>



using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


namespace DvtkApplicationLayer
{
    /// <summary>
    /// Checks the version of both the application and the DVTk library and displays a warning
    /// if one or both are a non-official or Alpha version.
    /// </summary>
    public class VersionChecker
    {
        //
        // - Constructors -
        //

        /// <summary>
        /// // Don't instantiate.
        /// </summary>
        private VersionChecker()
        {
            // Do nothing.
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Checks the version of both the application and the DVTk library.
        /// <br></br>
        /// If one or both are a non-official or Alpha version, a warning message box is displayed.
        /// </summary>
        public static void CheckVersion()
        {
            String warningText = String.Empty;


            //
            // Check the version of the application.
            //

            Assembly applicationAssembly = Assembly.GetEntryAssembly();

            if (IsLocalBuild(applicationAssembly))
            {
                warningText = "This application with version number " + GetVersionString(applicationAssembly) + " is an untested non-official version!";
            }
            else if (IsAlphaBuild(applicationAssembly))
            {
                warningText = "This application with version number " + GetVersionString(applicationAssembly) + " is an untested Alpha version!";
            }
            

            //
            // Check the version of the library.
            //

            // Any class, enumerate from the DVTk assembly will work.
            Dvtk.Events.ReportLevel reportLevel = Dvtk.Events.ReportLevel.None;

            Assembly libraryAssembly = Assembly.GetAssembly(reportLevel.GetType());

            if (IsLocalBuild(libraryAssembly))
            {
                if (warningText.Length > 0)
                {
                    warningText += "\n\n";
                }

                warningText += "The DVTk library (used by this application) with version number " + GetVersionString(libraryAssembly) + " is an untested non-official version!";
            }
            else if (IsAlphaBuild(libraryAssembly))
            {
                if (warningText.Length > 0)
                {
                    warningText += "\n\n";
                }
                
                warningText += "The DVTk library (used by this application) with version number " + GetVersionString(libraryAssembly) + " is an untested Alpha version!";
            }


            //
            // Display a message box with a warning if needed.
            //

            if (warningText.Length > 0)
            {
                MessageBox.Show(warningText, "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Gets the version of the supplied assembly as a string.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>A string containing the version of the assembly.</returns>
        private static String GetVersionString(Assembly assembly)
        {
            string versionString = String.Empty;

            AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;

            if (IsLocalBuild(assembly) || IsAlphaBuild(assembly))
            {
                versionString = String.Format("{0:D}.{1:D}.{2:D}.{3:D}", version.Major, version.Minor, version.Build, version.Revision);
            }
            else
            {
                versionString = String.Format("{0:D}.{1:D}.{2:D}", version.Major, version.Minor, version.Build);
            }

            return (versionString);
        }

        /// <summary>
        /// Indicates if the version of the supplied assembly represents an Alpha build.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Boolean indicating if the version of the supplied assembly represents an Alpha build.</returns>
        private static bool IsAlphaBuild(Assembly assembly)
        {
            bool isAlphaBuild = true;

            AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;

            if (((version.Major > 0) || (version.Minor > 0) || (version.Build > 0)) && (version.Revision != 0))
            {
                isAlphaBuild = true;
            }
            else
            {
                isAlphaBuild = false;
            }

            return (isAlphaBuild);
        }

        /// <summary>
        /// Indicates if the version of the supplied assembly represents a local build.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Boolean indicating if the version of the supplied assembly represents a local build.</returns>
        private static bool IsLocalBuild(Assembly assembly)
        {
            bool isLocalBuild = true;

            AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;

            if ((version.Major == 0) && (version.Minor == 0) && (version.Build == 0) && (version.Revision == 0))
            {
                isLocalBuild = true;
            }
            else
            {
                isLocalBuild = false;
            }

            return (isLocalBuild);
        }
    }
}
