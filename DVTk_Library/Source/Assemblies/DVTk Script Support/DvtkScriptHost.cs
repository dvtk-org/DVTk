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
using System.Xml;
using System.Windows.Forms;
using Dvtk;

namespace DvtkScriptSupport
{
    /// <summary>
    /// Supported script languages.
    /// </summary>
    public enum DvtkScriptLanguage
    {
        VB,
        VisualBasic,
        JScript,
        JScriptNET,
    }

    /// <summary>
    /// Hosting environment, supporting execution of scripts against the
    /// Dicom Validation Tool(Kit).
    /// Scripts are run using a <see cref="Dvtk.Sessions.ScriptSession"/> 
    /// which is identified within the script by means of global a variable <c>session</c>.
    /// </summary>
    public class DvtkScriptHost
        : VsaScriptHost
    {
        /// <summary>
        /// List of reserved key words to be used in scripts run by this script environment.
        /// </summary>
        private static readonly string SessionKeyWord = "session";
        private static readonly string DvtkScriptHostSessionKeyWord = "dvtkScriptHostSession";
        private static readonly string DvtkScriptHostScriptFullFileNameKeyWord = "dvtkScriptHostScriptFullFileName";
        private static readonly string[] ReservedKeyWords = 
            new string[] {
                             SessionKeyWord,
							 DvtkScriptHostSessionKeyWord,
							 DvtkScriptHostScriptFullFileNameKeyWord
                         };

        /// <summary>
        /// Collection of reference namespaces. These namespaces are imported for each script.
        /// </summary>
        private static readonly System.Collections.Hashtable 
            References = new System.Collections.Hashtable();

        /// <summary>
        /// Static constructor.
        /// </summary>
        static DvtkScriptHost()
        {
            string AssemblyName = null;
            string AssemblyDllNameValue = null;
            int index = 0;
            try
            {

                XmlTextReader reader = new XmlTextReader(Application.StartupPath + "//DVTKdllList.xml");
                // Hashtable of identifiers as used by the VSA engine and
                // the referenced assemblies (DLLs).

                reader.ReadStartElement();
                while (reader.Read())
                {                   
                    /* only if its a dll */
                    if (reader.Name.Equals("Assembly_DllName"))
                    {
                        AssemblyDllNameValue = reader.ReadElementString("Assembly_DllName");
                        index = AssemblyDllNameValue.LastIndexOf(".dll");
                        AssemblyName = AssemblyDllNameValue.Substring(0, index);
                        References.Add(AssemblyName, AssemblyDllNameValue);
                    }
                }
            }
            catch(System.Exception e)
            {
                System.String message = System.String.Format("Failed to read Dll List.. \" Error: \"{0}\"", e.Message);
				throw new System.SystemException(message, e);
            }           
		}

        private static string _Convert(DvtkScriptLanguage language)
        {
            switch (language)
            {
                case DvtkScriptLanguage.VB          : return "VB";
                case DvtkScriptLanguage.VisualBasic : return "Visual Basic";
                case DvtkScriptLanguage.JScript     : return "JScript";
                case DvtkScriptLanguage.JScriptNET  : return "JScript.NET";
                default:
                    throw new System.ApplicationException();
            }
        }

        /// <summary>
        /// Collection of used monikers in the current application.
        /// Each ScriptHost instance must be identified by a unique moniker (nickname/alias/identifier).
        /// </summary>
        private static System.Collections.ArrayList MonikerList = new System.Collections.ArrayList();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="language">The language used in the script.</param>
        /// <param name="moniker">The unique identifier of the script engine.</param>
        /// <param name="importedAssemblyDir">The directory of the used DVTK component/assemblies.</param>
        public DvtkScriptHost(
            DvtkScriptLanguage language,
            string moniker,
            string importedAssemblyDir) 
            : base(_Convert(language), moniker, "DvtkScript")
        {
            if (MonikerList.Contains(moniker)) throw new System.ArgumentException("Moniker was already used.");
            MonikerList.Add(moniker);
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(importedAssemblyDir);
            if (!directoryInfo.Exists)
            {
                throw new System.ArgumentException("Imported Assembly Directory does not exist.");
            }
            foreach (object key in References.Keys)
            {
                string itemName = (string)key;
				string assemblyNameOnly = (string)References[key];
                string assemblyName = 
                    System.IO.Path.Combine(importedAssemblyDir, assemblyNameOnly);
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(assemblyName);
				if (!fileInfo.Exists)
				{
					// Specific dll's must be present, other may be.
					if ((assemblyNameOnly.ToLower() == "dvtk.dll") || (assemblyNameOnly.ToLower() == "dvtkdata.dll") || (assemblyNameOnly.ToLower() == "dvtkmanagedcodeadapter.dll"))
					{
						throw new System.ArgumentException(
							string.Format("Required assembly {0} was not found", assemblyName));
					}
				}
				else
				{
					bool ok = this.AddReference((string)key, assemblyName);
                    if (!ok)
                    {
                        throw new System.ArgumentException(string.Format("The assembly {0} couldn't be added as reference.", assemblyName));
                    }
				}
            }

			// It is less clear for a script writer when DVTK namespaces are "invisibly" imported. So disable this.
//            foreach (string nameSpace in NameSpaceImports)
//            {
//                this._AddImport(nameSpace);
//            }
        }

        /// <summary>
        /// The assigned session used by the scripts. The session is used in
        /// the script by means of a global variable keyword <c>session</c>.
        /// </summary>
        public Dvtk.Sessions.ScriptSession Session
        {
            get 
            {
                return this._Session;
            }
            set
            {
                RemoveGlobalInstance(SessionKeyWord);
				RemoveGlobalInstance(DvtkScriptHostSessionKeyWord);

                this._Session = value;

                AddGlobalInstance(SessionKeyWord, this.Session);
				AddGlobalInstance(DvtkScriptHostSessionKeyWord, this.Session);
            }
        }
        private Dvtk.Sessions.ScriptSession _Session = null;

		/// <summary>
		/// The name of the script file that is executed.
		/// </summary>
		public String DvtkScriptHostScriptFullFileName
		{
			get
			{
				return this._DvtkScriptHostScriptFullFileName;
			}
			set
			{
				RemoveGlobalInstance(DvtkScriptHostScriptFullFileNameKeyWord);

				this._DvtkScriptHostScriptFullFileName = value;

				AddGlobalInstance(DvtkScriptHostScriptFullFileNameKeyWord, value);
			}
		}

		private String _DvtkScriptHostScriptFullFileName = "";
    }
}
