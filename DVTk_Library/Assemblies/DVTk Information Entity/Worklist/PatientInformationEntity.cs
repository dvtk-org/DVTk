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
using Dvtk.Dicom.InformationEntity;

namespace Dvtk.Dicom.InformationEntity.Worklist
{
	/// <summary>
	/// Summary description for PatientInformationEntity.
	/// </summary>
	public class PatientInformationEntity : BaseInformationEntity
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public PatientInformationEntity() : base("PATIENT") {}

		/// <summary>
		/// Set the Default Tag Type List for this Entity.
		/// </summary>
		protected override void SetDefaultTagTypeList()
		{
			// plus all other attributes from the Patient Relationship Module

			// attributes from the Patient Identification Module
			TagTypeList.Add(new TagType(Tag.PATIENTS_NAME, VR.PN, TagTypeEnum.TagRequired));
			// set the patient id tag to unique so that the patient registration, patient update and patient merge work - requires a search for
			// the unique patient id key (as in Query/Retrieve).
			TagTypeList.Add(new TagType(Tag.PATIENT_ID, VR.LO, TagTypeEnum.TagUnique));
			// plus all other attributes from the Patient Identification Module
			TagTypeList.Add(new TagType(Tag.OTHER_PATIENT_IDS, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ISSUER_OF_PATIENT_ID, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.OTHER_PATIENT_NAMES, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_BIRTH_NAME, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_MOTHERS_BIRTH_NAME, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.MEDICAL_RECORD_LOCATOR, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REFERENCED_PATIENT_ALIAS_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));

			// attributes from the Patient Demographic Module
			TagTypeList.Add(new TagType(Tag.PATIENTS_BIRTH_DATE, VR.DA, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_SEX, VR.CS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_WEIGHT, VR.DS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.CONFIDENTIALITY_CONSTRAINT_ON_PATIENT_DATA_DESCRIP, VR.LO, TagTypeEnum.TagOptional));
			// plus all other attributes from the Patient Demographic Module
			TagTypeList.Add(new TagType(Tag.PATIENTS_BIRTH_TIME, VR.TM, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_PRIMARY_LANGUAGE_CODE_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_SIZE, VR.DS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_AGE, VR.AS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.MILITARY_RANK, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.BRANCH_OF_SERVICE, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ETHNIC_GROUP, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.OCCUPATION, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENT_COMMENTS, VR.LT, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_INSURANCE_PLAN_CODE_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_ADDRESS, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.COUNTRY_OF_RESIDENCE, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REGION_OF_RESIDENCE, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_TELEPHONE_NUMBERS, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_RELIGIOUS_PREFERENCE, VR.LO, TagTypeEnum.TagOptional));

			// attributes from the Patient Medical Module
			TagTypeList.Add(new TagType(Tag.PATIENT_STATE, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PREGNANCY_STATUS, VR.US, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.MEDICAL_ALERTS, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.CONTRAST_ALLERGIES, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.SPECIAL_NEEDS, VR.LO, TagTypeEnum.TagOptional));
			// plus all other attributes from the Patient Medical Module
			TagTypeList.Add(new TagType(Tag.ADDITIONAL_PATIENT_HISTORY, VR.LT, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.LAST_MENSTRUAL_DATE, VR.DA, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.SMOKING_STATUS, VR.CS, TagTypeEnum.TagOptional));

			// attributes for a Veterinary Application
			TagTypeList.Add(new TagType(Tag.RESPONSIBLE_PERSON, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.RESPONSIBLE_PERSON_ROLE, VR.CS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.RESPONSIBLE_ORGANIZATION, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENT_SPECIES_DESCRIPTION, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENT_BREED_DESCRIPTION, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.BREED_REGISTRATION_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_SEX_NEUTERED, VR.CS, TagTypeEnum.TagOptional));
		}
	}
}
