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

namespace Dvtk.Dicom.InformationEntity.DefaultValues
{
	/// <summary>
	/// Summary description for DefaultValueManager.
	/// </summary>
	public class DefaultValueManager
	{
		private TagValueCollection _userDefinedDefaultTagValues = new TagValueCollection();
		private TagValueCollection _defaultDicomTagValues = new TagValueCollection();
		private TagValueCollection _instantiatedDefaultTagValues = new TagValueCollection();

		/// <summary>
		/// 
		/// </summary>
		public DefaultValueManager() {}

		/// <summary>
		/// 
		/// </summary>
		public TagValueCollection InstantiatedDefaultTagValues
		{
			get
			{
				return _instantiatedDefaultTagValues;
			}
		}

		/// <summary>
		/// Add user defined default Tag Values. Used to help define the message tag/values 
		/// used during the tests.
		/// </summary>
		/// <param name="defaultTagValue">Default Tag Value pair.</param>
		public void AddUserDefinedDefaultTagValue(BaseDicomTagValue defaultTagValue)
		{
			_userDefinedDefaultTagValues.Add(defaultTagValue);
		}

		/// <summary>
		/// 
		/// </summary>
		public void CreateDefaultTagValues()
		{
			// Create a default value list that is the combination of the user defined
			// default values and the in-built default values. The user defined values
			// take precedence.
			_defaultDicomTagValues = new TagValueCollection();

			// Add all the user defined default values provided 
			foreach(BaseDicomTagValue userDefinedTagValue in _userDefinedDefaultTagValues)
			{
				if (userDefinedTagValue is DicomTagValueDelete)
				{
					// Do not add this to the list as the default should be deleted
				}
				else
				{
					_defaultDicomTagValues.Add(userDefinedTagValue);
				}
			}
			
			// Generate all the in-built default values
			InBuiltDefaultTagValues inBuiltDefaultTagValues = new InBuiltDefaultTagValues();

			// Add the in-built default values - but only if not defined by the user
			foreach(BaseDicomTagValue inBuiltTagValue in inBuiltDefaultTagValues.TagValueDefaults)
			{
				BaseDicomTagValue userDefinedTagValue = _userDefinedDefaultTagValues.Find(inBuiltTagValue.Tag);
				if ((userDefinedTagValue != null) &&
					(userDefinedTagValue is DicomTagValueDelete))
				{
					// Do not add this to the list as the default should be deleted
				}
				else if (_defaultDicomTagValues.Find(inBuiltTagValue.Tag) == null)
				{
					_defaultDicomTagValues.Add(inBuiltTagValue);
				}
			}

			// Now instantiate the default tag value list
			_instantiatedDefaultTagValues = new TagValueCollection();
			foreach(BaseDicomTagValue defaultTagValue in _defaultDicomTagValues)
			{
				// The Value property of defaultTagValue returns the next instantiated value based on the
				// how the default value was defined at setup.
				_instantiatedDefaultTagValues.Add(new DicomTagValue(defaultTagValue.Tag, defaultTagValue.Value));
			}
		}

        /// <summary>
        /// Update the default DICOM Tag Values grouped by the given affected entity.
        /// Any 'auto' default value in the affected entity will get it's next value.
        /// </summary>
        /// <param name="affectedEntity">Affected Entity enum - to update.</param>
        public void UpdateInstantiatedDefaultTagValues(AffectedEntityEnum affectedEntity)
		{
			foreach(BaseDicomTagValue defaultTagValue in _defaultDicomTagValues)
			{
				if (defaultTagValue.AffectedEntity == affectedEntity)
				{
					// Try to get the instantiated default tag value
					BaseDicomTagValue instantiatedDefaultTagValue = _instantiatedDefaultTagValues.Find(defaultTagValue.Tag);
					if (instantiatedDefaultTagValue != null)
					{
						// Remove the existing tag value
						_instantiatedDefaultTagValues.Remove(instantiatedDefaultTagValue);

						// Add the updated value
						// The Value property of defaultTagValue returns the next instantiated value based on the
						// how the default value was defined at setup.
						_instantiatedDefaultTagValues.Add(new DicomTagValue(defaultTagValue.Tag, defaultTagValue.Value));
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tagValue"></param>
		public void UpdateInstantiatedDefaultTagValues(DicomTagValue tagValue)
		{
			// Try to get the instantiated default tag value
			BaseDicomTagValue instantiatedDefaultTagValue = _instantiatedDefaultTagValues.Find(tagValue.Tag);
			if (instantiatedDefaultTagValue != null)
			{
				// Remove the existing tag value
				_instantiatedDefaultTagValues.Remove(instantiatedDefaultTagValue);
			}

			// Add the updated value
			_instantiatedDefaultTagValues.Add(tagValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public System.String GetInstantiatedValue(Tag tag)
		{
			System.String lValue = System.String.Empty;

			// Try to get the instantiated default tag value
			BaseDicomTagValue instantiatedDefaultTagValue = _instantiatedDefaultTagValues.Find(tag);
			if (instantiatedDefaultTagValue != null)
			{
				if (instantiatedDefaultTagValue is DicomTagValue)
				{
					DicomTagValue tagValue = (DicomTagValue)instantiatedDefaultTagValue;
					lValue = tagValue.Value;
				}
			}

			return lValue;
		}
	}

	/// <summary>
	/// Summary description for InBuiltDefaultTagValues.
	/// </summary>
	public class InBuiltDefaultTagValues
	{
		private System.String _uidRoot = "1.2.826.0.1.3680043.2.1545.1.2.1.7";
		private TagValueCollection _defaultDicomTagValues = new TagValueCollection();

		public InBuiltDefaultTagValues()
		{
			// Patient Entity default values
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.PATIENTS_NAME, "Doe^John"));
			_defaultDicomTagValues.Add(new DicomTagValueAutoIncrement(AffectedEntityEnum.PatientEntity, Tag.PATIENT_ID, "PatId:", 1, 1, 6));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.PATIENTS_BIRTH_DATE, "19600606"));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.PATIENTS_SEX, "M"));

			// Study Entity default values
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.STUDY_DESCRIPTION, "StudyDescription"));
			_defaultDicomTagValues.Add(new DicomTagValueAutoIncrement(AffectedEntityEnum.StudyEntity, Tag.STUDY_ID, "StdyId:", 1, 1, 3));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetDate(AffectedEntityEnum.StudyEntity, Tag.STUDY_DATE));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetTime(AffectedEntityEnum.StudyEntity, Tag.STUDY_TIME));

			// Series Entity default values
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.PROTOCOL_NAME, "ProtocolName"));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetUid(AffectedEntityEnum.SeriesEntity, Tag.SERIES_INSTANCE_UID, _uidRoot, 1));
			_defaultDicomTagValues.Add(new DicomTagValueAutoIncrement(AffectedEntityEnum.SeriesEntity, Tag.SERIES_NUMBER, "", 1, 1, 0));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.RETRIEVE_AE_TITLE, "RetrieveAET"));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.SERIES_DESCRIPTION, "SeriesDescription"));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.PERFORMING_PHYSICIANS_NAME, "Performing^Physician"));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.OPERATORS_NAME, "Operators^Name"));

			// Instance Entity default values
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetUid(AffectedEntityEnum.InstanceEntity, Tag.SOP_INSTANCE_UID, _uidRoot, 1));
			_defaultDicomTagValues.Add(new DicomTagValueAutoIncrement(AffectedEntityEnum.InstanceEntity, Tag.INSTANCE_NUMBER, "", 1, 1, 0));

			// Image Service Request Entity default values
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.REFERENCED_STUDY_SEQUENCE, ""));
			_defaultDicomTagValues.Add(new DicomTagValueAutoIncrement(AffectedEntityEnum.ImageServiceRequestEntity, Tag.ACCESSION_NUMBER, "AccNo:", 1, 1, 6));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.REFERRING_PHYSICIANS_NAME, "Referring^Physician"));

			// Requested Procedure Entity default values
			_defaultDicomTagValues.Add(new DicomTagValueAutoIncrement(AffectedEntityEnum.RequestedProcedureEntity, Tag.REQUESTED_PROCEDURE_ID, "RPId:", 1, 1, 3));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.REQUESTED_PROCEDURE_DESCRIPTION, "RequestedProcedureDescription"));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetUid(AffectedEntityEnum.RequestedProcedureEntity, Tag.STUDY_INSTANCE_UID, _uidRoot, 1));

			// Scheduled Procedure Step Entity default values
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.MODALITY, "OT"));
			_defaultDicomTagValues.Add(new DicomTagValueAutoIncrement(AffectedEntityEnum.ScheduledProcedureStepEntity, Tag.SCHEDULED_PROCEDURE_STEP_ID, "SPSId:", 1, 1, 3));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, "ScheduledProcedureStepDescription"));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetDate(AffectedEntityEnum.ScheduledProcedureStepEntity, Tag.SCHEDULED_PROCEDURE_STEP_START_DATE));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetTime(AffectedEntityEnum.ScheduledProcedureStepEntity, Tag.SCHEDULED_PROCEDURE_STEP_START_TIME));

			// Performed Procedure Step Entity default values
			_defaultDicomTagValues.Add(new DicomTagValueAutoIncrement(AffectedEntityEnum.PerformedProcedureStepEntity, Tag.PERFORMED_PROCEDURE_STEP_ID, "PPSId:", 1, 1, 3));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.PERFORMED_PROCEDURE_STEP_DESCRIPTION, "PerformedProcedureStepDescription"));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetDate(AffectedEntityEnum.PerformedProcedureStepEntity, Tag.PERFORMED_PROCEDURE_STEP_START_DATE));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetTime(AffectedEntityEnum.PerformedProcedureStepEntity, Tag.PERFORMED_PROCEDURE_STEP_START_TIME));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetDate(AffectedEntityEnum.PerformedProcedureStepEntity, Tag.PERFORMED_PROCEDURE_STEP_END_DATE));
			_defaultDicomTagValues.Add(new DicomTagValueAutoSetTime(AffectedEntityEnum.PerformedProcedureStepEntity, Tag.PERFORMED_PROCEDURE_STEP_END_TIME));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.PERFORMED_STATION_AE_TITLE, "PerformedAET"));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.PERFORMED_STATION_NAME, "PerformedStationName"));
			_defaultDicomTagValues.Add(new DicomTagValue(Tag.PERFORMED_LOCATION, "PerformedLocation"));
		}

		public TagValueCollection TagValueDefaults
		{
			get
			{
				return _defaultDicomTagValues;
			}
		}

		public System.String UidRoot
		{
			get
			{
				return _uidRoot;
			}
		}
	}
}
