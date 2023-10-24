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
using DvtkHighLevelInterface.InformationModel;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomQueryItem.
	/// </summary>
	public class DicomQueryItem
	{
		private int _itemId = 0;
		private DicomMessage _dicomMessage;

		public DicomQueryItem(int itemId, DicomMessage dicomMessage)
		{
			_itemId = itemId;
			_dicomMessage = dicomMessage;
		}

		public int Id
		{
			get
			{
				return _itemId;
			}
		}

		public DicomMessage DicomMessage
		{
			get
			{
				return _dicomMessage;
			}
		}

		public System.String GetValue(DvtkData.Dimse.Tag tag)
		{
			System.String lValue = System.String.Empty;
	
			if (_dicomMessage != null)
			{
				lValue = GenerateTriggers.GetValueFromMessageUsingTag(_dicomMessage, tag);

			}

			return lValue;
		}

		public System.String GetValue(DvtkData.Dimse.Tag sequenceTag, DvtkData.Dimse.Tag tag)
		{
			System.String lValue = System.String.Empty;
	
			if (_dicomMessage != null)
			{
				lValue = GenerateTriggers.GetValueFromMessageUsingTag(_dicomMessage, sequenceTag, tag);

			}

			return lValue;
		}

		public override System.String ToString()
		{
			System.String patientId = this.GetValue(DvtkData.Dimse.Tag.PATIENT_ID);
			System.String patientName = this.GetValue(DvtkData.Dimse.Tag.PATIENTS_NAME);
			System.String procedureDescription = this.GetValue(DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION);
			return patientId + " : " + patientName + " : " + procedureDescription;
		}
	}
}
