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

using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Hl7.Threads
{
	/// <summary>
	/// Summary description for Hl7ThreadOptions.
	/// </summary>
	public class Hl7ThreadOptions: ThreadOptions
	{
		private ushort sessionId = 1;

		private String resultsDirectory = "";

        /// <summary>
        /// Gets or sets the results directory.
        /// </summary>
		public override String ResultsDirectory
		{
			get
			{
				return this.resultsDirectory;
			}
			set
			{
				this.resultsDirectory = value;
			}
		}

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
		public override ushort SessionId
		{
			get
			{
				return this.sessionId;
			}
			set
			{
				this.sessionId = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if results should be displayed.
        /// </summary>
        public override bool ShowResults
		{
			get
			{
				return(this.showResults);
			}
			set
			{
				this.showResults = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if results should be displayed.
        /// </summary>
        public override bool ShowTestLog
        {
            get
            {
                return (this.showTestLog);
            }
            set
            {
                this.showTestLog = value;
            }
        }

        public override bool ShowSummary
        {
            get
            {
                return (this.showSummary);
            }
            set
            {
                this.showSummary = value;
            }
        }
    }
}
