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
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

using DvtkApplicationLayer.UserInterfaces;

namespace DvtkApplicationLayer
{
    /// <summary>
    /// The Error Handler class.
    /// Use this class to be able to show more information in case of an exception in a release version
    /// (the same information is shown for a debug version).
    /// This class is now most suitable for a windows application.
    /// We need a class because event handling methods can't be static.
    /// 
    /// Usage of this class:
    /// The first lines of code of the entry point of the application should be:
    /// 
    /// 	// Only one of the exception handlers below will be used:
    ///		// - Application.ThreadException will be fired for a windows application
    ///		// - System.AppDomain.CurrentDomain.UnhandledException will be fired for other applications.
    ///		CustomExceptionHandler eh = new CustomExceptionHandler();
    ///		Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(eh.OnThreadException);
    ///		System.AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(eh.OnAppDomainUnhandledException);
    ///
    ///
    /// At the end of the outer catch block of a .Net thread, add the following code
    /// (don't do this for a HLI Thread, there is a different exception handler mechanism already present):
    /// 
    ///		// Problem:
    ///		// Errors thrown from a workerthread are eaten by the .NET 1.x CLR.
    ///		// Workaround:
    ///		// Directly call the global (untrapped) exception handler callback.
    ///		// Do NOT rely on 
    ///		// either
    ///		// - System.AppDomain.CurrentDomain.UnhandledException
    ///		// or
    ///		// - System.Windows.Forms.Application.ThreadException
    ///		// These events will only be triggered for the main thread not for worker threads.
    ///		//
    ///		CustomExceptionHandler eh = new CustomExceptionHandler();
    ///		System.Threading.ThreadExceptionEventArgs args = new ThreadExceptionEventArgs(ex);
    ///		eh.OnThreadException(this, args);
    ///		//
    ///		// Rethrow. This rethrow may work in the future .NET 2.x CLR.
    ///		// Currently eaten.
    ///		//
    ///		throw ex;
    ///
    /// </summary>
    public class CustomExceptionHandler
    {
        /// <summary>
        /// Handle the exception event.
        /// </summary>
        /// <param name="sender">The sender of the Exception.</param>
        /// <param name="t">The Exception event arguments.</param>
        public void OnThreadException(object sender, ThreadExceptionEventArgs t)
        {
            if (showExceptions)
            {
                try
                {
                    ShowThreadExceptionDialog(t.Exception);
                }
                catch
                {
                    try
                    {
                        MessageBox.Show("Fatal Error",
                            "Fatal Error",
                            MessageBoxButtons.AbortRetryIgnore,
                            MessageBoxIcon.Stop);
                    }
                    finally
                    {
                        Application.Exit();
                    }
                }

                Application.Exit();
            }
        }

        /// <summary>
        /// The handler of the unhandled exception.
        /// </summary>
        /// <param name="sender">The sender of the Exception.</param>
        /// <param name="e">The Exception event arguments.</param>
        public void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (showExceptions)
            {
                try
                {
                    ShowThreadExceptionDialog(e.ExceptionObject as Exception);
                }
                catch
                {
                    try
                    {
                        MessageBox.Show("Fatal Error",
                            "Fatal Error",
                            MessageBoxButtons.AbortRetryIgnore,
                            MessageBoxIcon.Stop);
                    }
                    finally
                    {
                        Application.Exit();
                    }
                }
                Application.Exit();
            }
        }

        /// <summary>
        /// The simple dialog that is displayed when this class catches and exception.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <returns>The dialog result.</returns>
        public static void ShowThreadExceptionDialog(Exception e)
        {
            string errorMsg =
                "Please contact the administrator with" +
                " the following information:\r\n\r\n";

            // The executable name.
            errorMsg += "Executable name:\r\n" + Application.ExecutablePath + "\r\n\r\n";

            // Error type.
            errorMsg += "Error type:\r\n" + e.GetType().ToString() + "\r\n\r\n";

            // Stack trace.
            errorMsg += "Error description:\r\n" + e.Message + "\r\n\r\nStack Trace:\r\n" + e.StackTrace + "\r\n\r\n";

            // Assemblies information.
            errorMsg += "Assembly information:\r\n";

            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in loadedAssemblies)
            {
                errorMsg += assembly.GetName().Name + " (" + assembly.GetName().Version.ToString() + ")\r\n";
            }

            // OS version information.
            String osName = Environment.OSVersion.ToString();

            if (Environment.OSVersion.Version.Major == 4)
            {
                osName += " (Windows NT 4)";
            }
            else if (Environment.OSVersion.Version.Major == 5)
            {
                if (Environment.OSVersion.Version.Minor == 0)
                {
                    osName += " (Windows 2000)";
                }
                else if (Environment.OSVersion.Version.Minor == 1)
                {
                    osName += " (Windows XP 32 bit)";
                }
                else if (Environment.OSVersion.Version.Minor == 2)
                {
                    osName += " (Windows Server 2003 or Windows XP 64 bit)";
                }
            }

            errorMsg += "\r\nThe OS version of the host computer is:\r\n" + osName + "\r\n\r\n";

            CustomExceptionHandlerForm customExceptionHandlerForm = new CustomExceptionHandlerForm(errorMsg);

            customExceptionHandlerForm.ShowDialog();
        }

        private bool showExceptions = true;

        /// <summary>
        /// 
        /// </summary>
        public bool ShowExceptions
        {
            set
            {
                showExceptions = value;
            }
        }
    }
}
