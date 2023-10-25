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

namespace Dvtk
{
    using Wrappers;
    /// <summary>
    /// This internal abstract class (Singleton) is introduced as workaround for
    /// <p>
    /// BUG: AppDomainUnloadedException is thrown when you call a virtual 
    /// destructor on a __nogc class during an AppDomain unload
    /// See http://support.microsoft.com/default.aspx?scid=kb;EN-US;837317
    /// </p>
    /// <p>
    /// BUG: AppDomainUnloaded Exception When You Use Managed Extensions for C++ Components
    /// See http://support.microsoft.com/default.aspx?kbid=309694
    /// </p>
    /// This is not a workaround suggested by MS.
    /// All their suggested workarounds failed to remove the bug.
    /// </summary>
    /// <remarks>
    /// <p>
    /// This class forms a listener for the DomainUnload event.
    /// It will dispose all managed classes with unmanaged resources which are listed in the
    /// MDisposableResources container.
    /// </p>
    /// <p>
    /// The actual registration of resources needs to be done carefully to avoid holding
    /// references.
    /// </p>
    /// <p>
    /// <see cref="Wrappers.MDisposableResources.AddDisposable"/> in
    /// <see cref="Sessions.ScriptSession"/> constructors
    /// <see cref="Sessions.MediaSession"/> constructors
    /// <see cref="Sessions.EmulatorSession"/> constructors
    /// </p>
    /// <p>
    /// <see cref="Wrappers.MDisposableResources.RemoveDisposable"/> in
    /// <see cref="Sessions.ScriptSession"/> destructor
    /// <see cref="Sessions.MediaSession"/> destructor
    /// <see cref="Sessions.EmulatorSession"/> destructor
    /// </p>
    /// </remarks>
    internal abstract class AppUnloadListener
    {
        /// <summary>
        /// Allows other static classes to Touch this static class to
        /// cause the static constructor to be invoked as side-effect.
        /// </summary>
        internal static void Touch()
        {
        }
        /// <summary>
        /// Installs the listener for the DomainUnLoad event.
        /// Using a static constructor ensures that only one event subscription
        /// is layed down.
        /// </summary>
        static AppUnloadListener()
        {
            System.AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
        }
        /// <summary>
        /// Call-back delegate to be invoked on DomainUnLoad.
        /// This provides us with a means to dispose the unmanaged Native C++
        /// resources hold by the managed C++ wrapper classes before the
        /// unloading of the AppDomain corrupts the <c>thunks</c> between the
        /// unmanaged Native C++ and the managed C++ wrappers classes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            Wrappers.MDisposableResources.Unload();
        }
    }
    /// <summary>
    /// Summary description for Setup.
    /// </summary>
    /// <remarks>
    /// This public abstract class (Singleton) is introduced as workaround for
    /// <p>
    /// Managed Extensions for C++ Reference<br></br>
    /// Converting Managed Extensions for C++ Projects from Pure Intermediate Language to Mixed Mode<br></br>
    /// See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/vcmex/html/vcconconvertingmanagedextensionsforcprojectsfrompureintermediatelanguagetomixedmode.asp
    /// <br></br>
    /// This is done because linking with an entry point causes managed code to run during DllMain, which is not safe (see DllMain for the limited set of things you can do during its scope).
    /// </p>
    /// <p>
    /// WARNING: Be sure to only call <c>Initialize</c> once. 
    /// When you make use of scripting within the DVT GUI application, the initialization will be performed the the DVT GUI application.
    /// The scripts do NOT need to initialize the DVTK in that case.
    /// Therefore, avoid the calls Initialize and Terminate in your scripts!
    /// </p>
    /// <p>
    /// GUIDELINE: Only introduce the these calls in a Main routine.
    /// </p>
    /// </remarks>
    public abstract class Setup
    {
        /// <summary>
        /// Initialize the component before use.
        /// </summary>
        /// <remarks>
        /// Needed to ensure that the C-runtime is correctly started.
        /// </remarks>
        /// <returns>not used</returns>
        public static int Initialize()
        {
            int retval = 0;
            retval = CrtWrapper.minitialize();
            return retval;
        }
        /// <summary>
        /// Terminate the component after use.
        /// </summary>
        /// <remarks>
        /// Needed to ensure that the C-runtime is correctly ended.
        /// </remarks>
        /// <returns>not used</returns>
        public static int Terminate()
        {
            int retval = 0;
            retval = CrtWrapper.mterminate();
            return retval;
        }
    }
}
