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
using System.Collections;

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using VR = DvtkData.Dimse.VR;



namespace DvtkHighLevelInterface.Dicom.Threads
{
	/// <summary>
	/// Class used to change an attribute for a received DICOM message.
	/// </summary>
	public class InboundDicomMessageFilterSetAttribute
	{
		private String tagSequence = "";

		private VR vR = VR.UN;

		private ArrayList values = null;
		
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="vR">The VR.</param>
        /// <param name="values">The values.</param>
		public InboundDicomMessageFilterSetAttribute(String tagSequence, VR vR, params Object[] values)
		{
			this.tagSequence = tagSequence;
			this.vR = vR;
			this.values = new ArrayList(values);
		}

        /// <summary>
        /// Applies this filter to a DICOM message.
        /// </summary>
        /// <param name="dicomMessage">The DICOM message.</param>
		public void Apply(DicomMessage dicomMessage)
		{
			dicomMessage.Set(tagSequence, vR, this.values.ToArray());
		}
	}
}
