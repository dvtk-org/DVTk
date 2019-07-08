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

using DvtkData.Dimse;

namespace Dvtk.Comparator
{
	/// <summary>
	/// Summary description for DicomTagVrLookup.
	/// </summary>
	public class DicomTagVrLookup
	{
		private static DicomTagVrCollection _dicomTagVrCollection = new DicomTagVrCollection();

		static DicomTagVrLookup()
		{
			#region Data Dictionary
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.PATIENT_ID, VR.LO));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.PATIENTS_NAME, VR.PN));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.PATIENTS_BIRTH_DATE, VR.DA));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.PATIENTS_BIRTH_TIME, VR.TM));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.PATIENTS_SEX, VR.CS));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.CURRENT_PATIENT_LOCATION, VR.LO));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.REFERRING_PHYSICIANS_NAME, VR.PN));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, VR.SQ));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.SCHEDULED_ACTION_ITEM_CODE_SEQUENCE, VR.SQ));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.CODE_VALUE, VR.SH));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.CODE_MEANING, VR.LO));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.CODING_SCHEME_DESIGNATOR, VR.SH));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.ACCESSION_NUMBER, VR.SH));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.REQUESTED_PROCEDURE_ID, VR.SH));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.SCHEDULED_PROCEDURE_STEP_ID, VR.SH));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.MODALITY, VR.CS));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.SCHEDULED_PROCEDURE_STEP_START_DATE, VR.DA));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.SCHEDULED_PROCEDURE_STEP_START_TIME, VR.TM));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.REQUESTED_PROCEDURE_CODE_SEQUENCE, VR.SQ));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.REQUESTED_PROCEDURE_DESCRIPTION, VR.LO));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, VR.LO));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.STUDY_INSTANCE_UID, VR.UI));
			_dicomTagVrCollection.Add(new DicomTagVr(Tag.OTHER_PATIENT_IDS, VR.LO));
			#endregion Data Dictionary
		}

		public static VR GetVR(Tag tag)
		{
			return _dicomTagVrCollection.GetVR(tag);
		}
	}
}
