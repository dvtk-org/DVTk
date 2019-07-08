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
using System.IO;



namespace DvtkHighLevelInterface.Common.Threads
{
	/// <summary>
	/// The options for the abstract Thread class.
	/// </summary>
	public abstract class ThreadOptions
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property AttachChildsToUserInterfaces.
		/// </summary>
		private bool attachChildsToUserInterfaces = true;
		
		/// <summary>
		/// See property Identifier.
		/// </summary>
		private String identifier = null;

		/// <summary>
		/// See property Name.
		/// </summary>
		private String name = null;

		/// <summary>
		/// See property ResultsFileName.
		/// </summary>
		private String resultsFileNameExtension = ".xml";
		
		/// <summary>
		/// See property ResultsFileNameIndex.
		/// </summary>
		private int resultsFileNameIndex = 0;

		/// <summary>
		/// See property ResultsFileNameWithoutExtension.
		/// </summary>
		private String resultsFileNameOnlyWithoutExtension = null;

		/// <summary>
		/// See property ShowResults.
		/// </summary>
		internal bool showResults = false;

        /// <summary>
        /// See property ShowTestLog.
        /// </summary>
        internal bool showTestLog = false;

        /// <summary>
        /// See property ShowSummary.
        /// </summary>
        internal bool showSummary = true;

        /// <summary>
        /// See property StartAndStopResultsGatheringEnabled.
        /// </summary>
        private bool startAndStopResultsGatheringEnabled = true;

		/// <summary>
		/// See property UseResultsFileNameIndex.
		/// </summary>
		private bool useResultsFileNameIndex = false;



		//
		// - Properties -
		//

        /// <summary>
        /// Gets or sets a boolean indicating if childs of a thread will automatically be attached
        /// to a user interface to which the thread itself is already attachted.
        /// </summary>
		public bool AttachChildsToUserInterfaces
		{
			get
			{
				return(this.attachChildsToUserInterfaces);
			}
			set
			{
				this.attachChildsToUserInterfaces = value;
			}
		}

		/// <summary>
		/// The unique identifier of this object.
		/// It is used to set the name of the underlying .Net thread.
		/// When displaying activity logging (if enabled) for this thread in a dialog,
		/// the Identifier property is also used to uniquely identify logging from this
		/// thread. Besides this, in a DicomThread, the Identifier may also be used
		/// to create a unique results file name.
		/// 
		/// If this property is set, the calling code has to make sure that this Identifier
		/// is unique.
		/// If not set, the Name property is appended with a number to create a unique identifier.
		/// </summary>
		public String Identifier
		{
			get
			{
				return(this.identifier);
			}
			set
			{
				this.identifier = value;
			}
		}

		/// <summary>
		/// The (possible not unique) name of this object.
		/// If this property is not set, the Class name will be used as the name.
		/// 
		/// See also the property Identifier for the usage of this property.
		/// </summary>
		public String Name
		{
			get
			{
				return(this.name);
			}
			set
			{
				this.name = value;
			}
		}

        /// <summary>
        /// gets or sets the directory in which the results file(s) will be written.
        /// </summary>
		public abstract String ResultsDirectory
		{
			get;
			set;
		}

        /// <summary>
        /// Gets the file name only of the results files.
        /// </summary>
		public String ResultsFileNameOnly
		{
			get
			{
				String resultsFileName = "";

				if (this.useResultsFileNameIndex || (this.resultsFileNameIndex > 1))
				{
					if (this.resultsFileNameOnlyWithoutExtension.IndexOf("{0}") == -1)
					{
						resultsFileName = this.resultsFileNameOnlyWithoutExtension + this.resultsFileNameIndex.ToString() + this.resultsFileNameExtension;
					}
					else
					{
						resultsFileName = String.Format(this.resultsFileNameOnlyWithoutExtension, this.resultsFileNameIndex.ToString()) + this.resultsFileNameExtension;
					}
				}
				else
				{
					if (this.resultsFileNameOnlyWithoutExtension.IndexOf("{0}") == -1)
					{
						resultsFileName = this.resultsFileNameOnlyWithoutExtension + this.resultsFileNameExtension;
					}
					else
					{
						resultsFileName = String.Format(this.resultsFileNameOnlyWithoutExtension, "") + this.resultsFileNameExtension;
					}
				}
		
				return(resultsFileName);
			}
		}

        /// <summary>
        /// Gets the full file name of the results files.
        /// </summary>
		public String ResultsFullFileName
		{
			get
			{
				return(Path.Combine(ResultsDirectory, ResultsFileNameOnly));
			}
		}

        /// <summary>
        /// Gets or sets the results file name extension.
        /// </summary>
		public String ResultsFileNameExtension
		{
			get
			{
				return(this.resultsFileNameExtension);
			}
			set
			{
				this.resultsFileNameExtension = value;
			}
		}

        /// <summary>
        /// Gets or sets the results file name index.
        /// </summary>
		public int ResultsFileNameIndex
		{
			get
			{
				return(this.resultsFileNameIndex);
			}
			set
			{
				this.resultsFileNameIndex = value;
			}
		}

        /// <summary>
        /// Gets or sets the results file name without extension.
        /// </summary>
		public String ResultsFileNameOnlyWithoutExtension
		{
			get
			{
				return(this.resultsFileNameOnlyWithoutExtension);
			}
			set
			{
				this.resultsFileNameOnlyWithoutExtension = value;
			}
		}

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
		public abstract ushort SessionId
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets a boolean indicating if the results should be displayed after the thread
        /// has stopped and closed the results files.
        /// </summary>
		public abstract bool ShowResults
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets a boolean indicating if the tset log should be displayed after the thread
        /// has stopped and closed the results files.
        /// </summary>
        public abstract bool ShowTestLog
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean indicating if the tset log should be displayed after the thread
        /// has stopped and closed the results files.
        /// </summary>
        public abstract bool ShowSummary
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean indicating if the thread will automatically start results gathering when started and
        /// stop results gathering when stopped.
        /// </summary>
        public bool StartAndStopResultsGatheringEnabled
        {
            get
            {
                return (this.startAndStopResultsGatheringEnabled);
            }
            set
            {
                this.startAndStopResultsGatheringEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if an index should be used as part of a results file name.
        /// </summary>
		public bool UseResultsFileNameIndex
		{
			get
			{
				return(this.useResultsFileNameIndex);
			}
			set
			{
				this.useResultsFileNameIndex = value;
			}
		}

        /// <summary>
        /// Gets the full file name of the detailed results file.
        /// </summary>
		public String DetailResultsFullFileName
		{
			get
			{
				return(Path.Combine(ResultsDirectory, DetailResultsFileNameOnly));
			}
		}

        /// <summary>
        /// Gets the file name of the detailed results file.
        /// </summary>
        public String DetailResultsFileNameOnly
		{
			get
			{
				return("Detail_" + ResultsFileNameOnly);
			}
		}

        /// <summary>
        /// Gets the file name of the detailed results file.
        /// </summary>
        public String TestLogFileNameOnly
        {
            get
            {
                return ("TestLog_" + ResultsFileNameOnly);
            }
        }

        /// <summary>
        /// Gets the full file name of the detailed results file.
        /// </summary>
        public String TestLogFullFileName
        {
            get
            {
                return (Path.Combine(ResultsDirectory, TestLogFileNameOnly));
            }
        }

        /// <summary>
        /// Gets the full file name of the detailed results file.
        /// </summary>
        public String SummaryResultsFullFileName
        {
            get
            {
                return (Path.Combine(ResultsDirectory, SummaryResultsFileNameOnly));
            }
        }

        /// <summary>
        /// Gets the file name of the detailed results file.
        /// </summary>
        public String SummaryResultsFileNameOnly
        {
            get
            {
                return ("Summary_" + ResultsFileNameOnly);
            }
        }

	

		//
		// - Methods -
		//

        /// <summary>
        /// Copies all options from another ThreadOptions instance.
        /// </summary>
        /// <param name="threadOptions"></param>
		protected void CopyFrom(ThreadOptions threadOptions)
		{
			AttachChildsToUserInterfaces = threadOptions.AttachChildsToUserInterfaces;
			ResultsFileNameExtension = threadOptions.ResultsFileNameExtension;
			StartAndStopResultsGatheringEnabled = threadOptions.StartAndStopResultsGatheringEnabled;
			UseResultsFileNameIndex = threadOptions.UseResultsFileNameIndex;
		}
	}
}
