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
	/// Summary description for VisitInformationEntity.
	/// </summary>
	public class VisitInformationEntity : BaseInformationEntity
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public VisitInformationEntity() : base("VISIT") {}

		/// <summary>
		/// Set the Default Tag Type List for this Entity.
		/// </summary>
		protected override void SetDefaultTagTypeList()
		{
			// attributes from the Visit Identification Module
			TagTypeList.Add(new TagType(Tag.ADMISSION_ID, VR.LO, TagTypeEnum.TagOptional));
			// plus all other attributes from the Visit Identification Module
			TagTypeList.Add(new TagType(Tag.INSTITUTION_NAME, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.INSTITUTION_ADDRESS, VR.ST, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.INSTITUTION_CODE_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ISSUER_OF_ADMISSION_ID, VR.LO, TagTypeEnum.TagOptional));

			// attributes from the Visit Status Module
			TagTypeList.Add(new TagType(Tag.CURRENT_PATIENT_LOCATION, VR.LO, TagTypeEnum.TagOptional));
			// plus all other attributes from the Visit Status Module
			TagTypeList.Add(new TagType(Tag.VISIT_STATUS_ID, VR.CS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_INSTITUTION_RESIDENCE, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.VISIT_COMMENTS, VR.LT, TagTypeEnum.TagOptional));

			// attributes from the Visit Relationship Module
			TagTypeList.Add(new TagType(Tag.REFERENCED_PATIENT_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			// plus all other attributes from the Visit Relationship Module

			// plus all other attributes from the Visit Admission Module
			TagTypeList.Add(new TagType(Tag.ROUTE_OF_ADMISSIONS, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ADMITTING_DATE, VR.DA, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ADMITTING_TIME, VR.TM, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.SCHEDULED_ADMISSION_DATE, VR.DA, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.SCHEDULED_ADMISSION_TIME, VR.TM, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ADMITTING_DIAGNOSIS_DESCRIPTION, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ADMITTING_DIAGNOSIS_CODE_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
		}
	}
}
