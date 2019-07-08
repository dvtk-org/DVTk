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
	/// Outbound DICOM message filter to set exactly one attribute.
	/// </summary>
	public class OutboundDicomMessageFilterSetAttribute: OutboundDicomMessageFilter
	{
        //
        // - Fields -
        //

		private String tagSequence = "";

		private VR vR = VR.UN;

		private ArrayList values;



        //
        // - Constructors -
        //

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tag">The tag of the attribute to set.</param>
        /// <param name="vr">The VR of the attribute to set.</param>
        /// <param name="values">The values of the attribute to set.</param>
		public OutboundDicomMessageFilterSetAttribute(String tag, VR vr, params Object[] values)
		{
			this.tagSequence = tag;
			this.vR = vr;
			this.values = new ArrayList(values);
		}

        /// <summary>
        /// Set one attribute in the outbound DICOM message.
        /// </summary>
        /// <param name="dicomMessage">The outbound DICOM message.</param>
        override public void Apply(DicomMessage dicomMessage)
		{
			dicomMessage.Set(tagSequence, vR, this.values.ToArray());
		}
	}
}
