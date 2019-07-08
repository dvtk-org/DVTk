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

using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
    /// <br></br>
    /// Summary description for DicomAttributeToCompare.
	/// </summary>
	internal class DicomAttributeToValidate
	{
		private ValidationRuleDicomAttribute validationRuleDicomAttribute = new ValidationRuleDicomAttribute();

		private Attribute attribute = new InvalidAttribute();

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		public ValidationRuleDicomAttribute ValidationRuleDicomAttribute
		{
			get
			{
				return(this.validationRuleDicomAttribute);
			}
			set
			{
				this.validationRuleDicomAttribute = value;
			}
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		public Attribute Attribute
		{
			get
			{
				return(this.attribute);
			}
			set
			{
				this.attribute = value;
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Indicates if the information for this attribute should be displayed.
		/// </summary>
		public bool Display
		{
			get
			{
				bool display = (this.ValidationRuleDicomAttribute.TagSequence.Length > 0);

				return(display);
			}
		}
	}
}
