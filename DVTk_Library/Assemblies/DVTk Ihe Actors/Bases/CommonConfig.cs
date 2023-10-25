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

namespace Dvtk.IheActors.Bases
{
	/// <summary>
	/// Summary description for CommonConfig.
	/// </summary>
	public class CommonConfig
	{
		private System.String _rootedBaseDirectory = System.String.Empty;
		private System.String _resultsDirectory = System.String.Empty;
		private System.String _resultsSubdirectory = System.String.Empty; // optionally set by application
        private bool _overwriteResults = false;
        private System.String _credentialsFilename = System.String.Empty;
		private System.String _certificateFilename = System.String.Empty;
		private System.String _nistWebServiceUrl = System.String.Empty;
		private System.String _hl7ProfileDirectory = System.String.Empty;
		private System.String _hl7ProfileStoreName = System.String.Empty;
		private System.String _hl7ValidationContextFilename = System.String.Empty;
		private bool _interactive = false;
 
		/// <summary>
		/// Class constructor.
		/// </summary>
		public CommonConfig() {}

		/// <summary>
		/// Property - Rooted Base Directory.
		/// </summary>
		public System.String RootedBaseDirectory
		{
			get
			{
				return _rootedBaseDirectory;
			}
			set
			{
				_rootedBaseDirectory = value;
			}
		}

		/// <summary>
		/// Property - Results Directory.
		/// </summary>
		public System.String ResultsDirectory
		{
			get
			{
				return _resultsDirectory;
			}
			set
			{
				_resultsDirectory = value;
			}
		}

		/// <summary>
		/// Property - Results Sub-directory. 
		/// Maybe defined to produce a subdirectory for the results at runtime. This must be set by the application - it does not
		/// come from any kind of config file.
		/// </summary>
		public System.String ResultsSubdirectory
		{
			get
			{
				return _resultsSubdirectory;
			}
			set
			{
				_resultsSubdirectory = value;
			}
		}

        /// <summary>
        /// Property - Overwrite results files.
        /// True - the results will be written directly into the results subdirectory specified by ResultsSubdirectory. Any existing
        /// results files may be overwritten if a test case is re-run.
        /// False - a new subdirectory with a date / time stamp as the directory name will be created in the results subdirectory s
        /// pecified by ResultsSubdirectory and the results files written in this directory. 
        /// </summary>
        public bool OverwriteResults
        {
            get
            {
                return _overwriteResults;
            }
            set
            {
                _overwriteResults = value;
            }
        }

		/// <summary>
		/// Property - Credentials Filename.
		/// </summary>
		public System.String CredentialsFilename
		{
			get
			{
				return _credentialsFilename;
			}
			set
			{
				_credentialsFilename = value;
			}
		}

		/// <summary>
		/// Property - Certificate Filename.
		/// </summary>
		public System.String CertificateFilename
		{
			get
			{
				return _certificateFilename;
			}
			set
			{
				_certificateFilename = value;
			}
		}

		/// <summary>
		/// Property - NIST Web Service URL.
		/// </summary>
		public System.String NistWebServiceUrl
		{
			get
			{
				return _nistWebServiceUrl;
			}
			set
			{
				_nistWebServiceUrl = value;
			}
		}

		/// <summary>
		/// Property - Hl7 Profile Directory.
		/// </summary>
		public System.String Hl7ProfileDirectory
		{
			get
			{
				return _hl7ProfileDirectory;
			}
			set
			{
				_hl7ProfileDirectory = value;
			}
		}

		/// <summary>
		/// Property - Hl7 Profile Store Name.
		/// </summary>
		public System.String Hl7ProfileStoreName
		{
			get
			{
				return _hl7ProfileStoreName;
			}
			set
			{
				_hl7ProfileStoreName = value;
			}
		}

		/// <summary>
		/// Property - Hl7 Validation Context Filename.
		/// </summary>
		public System.String Hl7ValidationContextFilename
		{
			get
			{
				return _hl7ValidationContextFilename;
			}
			set
			{
				_hl7ValidationContextFilename = value;
			}
		}

		/// <summary>
		/// Property - Interactive.
        /// This property can be accessed from within the test scripts to determine if the script 
        /// should be run in an interactive mode or not. True indicates that the activity log will
        /// be displayed together with any user input when the script is being run.
		/// </summary>
		public bool Interactive
		{
			get 
			{ 
				return _interactive; 
			}
			set 
			{ 
				_interactive = value; 
			}
		}
	}
}
