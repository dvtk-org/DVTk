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
using Dvtk.CommonDataFormat;

namespace Dvtk.Comparator
{
	/// <summary>
	/// Summary description for DicomComparisonTemplate.
	/// </summary>
	public class DicomComparisonTemplate
	{
		private DvtkData.Dimse.DimseCommand _command = DvtkData.Dimse.DimseCommand.UNDEFINED;
		private System.String _sopClassUid = System.String.Empty;
		private DicomComparisonTagCollection _comparisonTags = new DicomComparisonTagCollection();

		/// <summary>
		/// Class constructor.
		/// </summary>
		public DicomComparisonTemplate() {}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="command">DIMSE Command ID</param>
		/// <param name="sopClassUid">SOP Class UID</param>
		/// <returns>bool - true = template initialized, false template not initialized</returns>
		public bool Initialize(DvtkData.Dimse.DimseCommand command, System.String sopClassUid)
		{
			bool initialized = true;
			_command = command;
			_sopClassUid = sopClassUid;

			// Only certain templates available
			// Use command and sopClassUid to determine if we can set one up
			if ((command == DvtkData.Dimse.DimseCommand.CFINDRSP) &&
				(sopClassUid == DvtkData.Dul.AbstractSyntax.Modality_Worklist_Information_Model_FIND.UID))
			{
				// Add the comparison tags - these tags will be used to extract the values out of the
				// Dicom Dataset for comparison with other Datasets containing the same tags
				_comparisonTags.Add(new DicomComparisonTag(Tag.SPECIFIC_CHARACTER_SET, VR.CS, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.ACCESSION_NUMBER, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.MODALITY, VR.CS, new CommonStringFormat()));
//				_comparisonTags.Add(new DicomComparisonTag(Tag.REFERENCED_STUDY_SEQUENCE, VR.SQ, new CommonUidFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENTS_NAME, VR.PN, new CommonNameFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENT_ID, VR.LO, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENTS_BIRTH_DATE, VR.DA, new CommonDateFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENTS_SEX, VR.CS, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.STUDY_INSTANCE_UID, VR.UI, new CommonUidFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.REQUESTED_PROCEDURE_ID, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_ID, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.REQUESTED_PROCEDURE_DESCRIPTION, VR.LO, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, VR.LO, new CommonStringFormat()));
			}
			else if (command == DvtkData.Dimse.DimseCommand.CSTORERQ)
			{
				// Add the comparison tags - these tags will be used to extract the values out of the
				// Dicom Dataset for comparison with other Datasets containing the same tags
				_comparisonTags.Add(new DicomComparisonTag(Tag.SPECIFIC_CHARACTER_SET, VR.CS, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.ACCESSION_NUMBER, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.MODALITY, VR.CS, new CommonStringFormat()));
//				_comparisonTags.Add(new DicomComparisonTag(Tag.REFERENCED_STUDY_SEQUENCE, VR.SQ, new CommonUidFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENTS_NAME, VR.PN, new CommonNameFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENT_ID, VR.LO, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENTS_BIRTH_DATE, VR.DA, new CommonDateFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENTS_SEX, VR.CS, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.STUDY_INSTANCE_UID, VR.UI, new CommonUidFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.REQUEST_ATTRIBUTES_SEQUENCE, Tag.REQUESTED_PROCEDURE_ID, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.REQUEST_ATTRIBUTES_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_ID, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.REQUEST_ATTRIBUTES_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, VR.LO, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PERFORMED_PROCEDURE_STEP_ID, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PERFORMED_PROCEDURE_STEP_DESCRIPTION, VR.LO, new CommonStringFormat()));
			}
			else if ((command == DvtkData.Dimse.DimseCommand.NCREATERQ) &&
				(sopClassUid == DvtkData.Dul.AbstractSyntax.Modality_Performed_Procedure_Step.UID))
			{
				// Add the comparison tags - these tags will be used to extract the values out of the
				// Dicom Dataset for comparison with other Datasets containing the same tags
				_comparisonTags.Add(new DicomComparisonTag(Tag.SPECIFIC_CHARACTER_SET, VR.CS, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE, Tag.ACCESSION_NUMBER, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.MODALITY, VR.CS, new CommonStringFormat()));
//				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE, Tag.REFERENCED_STUDY_SEQUENCE, VR.SQ, new CommonUidFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENTS_NAME, VR.PN, new CommonNameFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENT_ID, VR.LO, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENTS_BIRTH_DATE, VR.DA, new CommonDateFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PATIENTS_SEX, VR.CS, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE, Tag.STUDY_INSTANCE_UID, VR.UI, new CommonUidFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE, Tag.REQUESTED_PROCEDURE_ID, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_ID, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE, Tag.REQUESTED_PROCEDURE_DESCRIPTION, VR.LO, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, VR.LO, new CommonStringFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PERFORMED_PROCEDURE_STEP_ID, VR.SH, new CommonIdFormat()));
				_comparisonTags.Add(new DicomComparisonTag(Tag.PERFORMED_PROCEDURE_STEP_DESCRIPTION, VR.LO, new CommonStringFormat()));
			}
			else
			{
				initialized = false;
			}

			return initialized;
		}

		#region properties
		/// <summary>
		/// Command property.
		/// </summary>
		public DvtkData.Dimse.DimseCommand Command
		{
			set
			{
				_command = value;
			}
			get
			{
				return _command;
			}
		}

		/// <summary>
		/// SopClassUid property.
		/// </summary>
		public System.String SopClassUid
		{
			set
			{
				_sopClassUid = value;
			}
			get
			{
				return _sopClassUid;
			}
		}

		/// <summary>
		/// ComparisonTags property.
		/// </summary>
		public DicomComparisonTagCollection ComparisonTags
		{
			get
			{
				return _comparisonTags;
			}
		}
		#endregion
	}
}
