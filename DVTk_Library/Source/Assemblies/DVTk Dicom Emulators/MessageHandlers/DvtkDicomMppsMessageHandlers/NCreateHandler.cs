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

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Threads;
using VR = DvtkData.Dimse.VR;



namespace Dvtk.DvtkDicomEmulators.MppsMessageHandlers
{
	/// <summary>
	/// Summary description for NActionHandler.
	/// </summary>
	public class NCreateHandler : MessageHandler
	{
		public NCreateHandler() {}

		public override bool HandleNCreateRequest(DicomMessage dicomMessage)
		{
			// Try to get the IOD Name
			System.String iodName = DicomThread.GetIodNameFromDefinition(dicomMessage);

			// Try to get the Patient Name			
			DvtkHighLevelInterface.Dicom.Other.Values attributeValues = dicomMessage["0x00100010"].Values;
			System.String patientName = attributeValues[0];
			System.String messsage = String.Format("Processed N-CREATE-RQ {0}: \"{1}\"", iodName, patientName);
			WriteInformation(messsage);

			DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.NCREATERSP);

			responseMessage.Set("0x00000900", VR.US, 0);

			this.Send(responseMessage);
			return true;
		}
	}
}
