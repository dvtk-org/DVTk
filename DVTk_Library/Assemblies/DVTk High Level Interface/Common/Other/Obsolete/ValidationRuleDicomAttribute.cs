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



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
    /// </summary>
	public class ValidationRuleDicomAttribute
	{
		private String tagSequenceAsString = "";

		private CompareFlags compareFlags = CompareFlags.None;

		private DicomAttributeFlags dicomAttributeFlags = DicomAttributeFlags.None;

		private bool displayFullTagSequence = false;

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// The tag sequence of the attribute.
		/// An empty string means don't display information about this attribute.
		/// </summary>
		public String TagSequence
		{
			get
			{
				return(this.tagSequenceAsString);
			}
			set
			{
				this.tagSequenceAsString = value;
			}
		}
	
        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		public CompareFlags CompareFlags
		{
			get
			{
				return(this.compareFlags);
			}
			set
			{
				this.compareFlags = value;
			}
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		public DicomAttributeFlags DicomAttributeFlags
		{
			get
			{
				return(this.dicomAttributeFlags);
			}
			set
			{
				this.dicomAttributeFlags = value;
			}
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		internal bool DisplayFullTagSequence
		{
			get
			{
				return(this.displayFullTagSequence);
			}
			set
			{
				this.displayFullTagSequence = value;
			}
		}
	}
}
