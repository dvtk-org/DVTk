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
using DvtkData.Collections;
using Dvtk.Dicom.InformationEntity;

namespace Dvtk.Dicom.InformationEntity.QueryRetrieve
{
	/// <summary>
	/// Summary description for PatientStudyInformationEntity.
	/// </summary>
	public class PatientStudyInformationEntity : BaseInformationEntity
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public PatientStudyInformationEntity() : base("STUDY") {}

        /// <summary>
        /// Gets the filenames of all (indirect) child instance information entities.
        /// </summary>
        public StringCollection FileNames
        {
            get
            {
                StringCollection fileNames = new StringCollection();

                foreach (SeriesInformationEntity seriesInformationEntity in Children)
                {
                    StringCollection seriesInformationEntityFileNames = seriesInformationEntity.FileNames;

                    foreach (String fileName in seriesInformationEntityFileNames)
                    {
                        fileNames.Add(fileName);
                    }
                }

                return (fileNames);
            }
        }

		/// <summary>
		/// Set the Default Tag Type List for this Entity.
		/// </summary>
		protected override void SetDefaultTagTypeList()
		{
			TagTypeList.Add(new TagType(Tag.SPECIFIC_CHARACTER_SET, VR.CS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.STUDY_DATE, VR.DA, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.STUDY_TIME, VR.TM, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.ACCESSION_NUMBER, VR.SH, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.PATIENTS_NAME, VR.PN, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.PATIENT_ID, VR.LO, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.STUDY_ID, VR.SH, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.STUDY_INSTANCE_UID, VR.UI, TagTypeEnum.TagUnique));
			TagTypeList.Add(new TagType(Tag.MODALITIES_IN_STUDY, VR.CS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REFERRING_PHYSICIANS_NAME, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.STUDY_DESCRIPTION, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PROCEDURE_CODE_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NAME_OF_PHYSICIANS_READING_STUDY, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ADMITTING_DIAGNOSIS_DESCRIPTION, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REFERENCED_STUDY_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REFERENCED_PATIENT_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_BIRTH_DATE, VR.DA, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_BIRTH_TIME, VR.TM, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_SEX, VR.CS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.OTHER_PATIENT_IDS, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.OTHER_PATIENT_NAMES, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_AGE, VR.AS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_SIZE, VR.DS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_WEIGHT, VR.DS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ETHNIC_GROUP, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.OCCUPATION, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ADDITIONAL_PATIENT_HISTORY, VR.LT, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENT_COMMENTS, VR.LT, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.OTHER_STUDY_NUMBERS, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NUMBER_OF_PATIENT_RELATED_STUDIES, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NUMBER_OF_PATIENT_RELATED_SERIES, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NUMBER_OF_PATIENT_RELATED_INSTANCES, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NUMBER_OF_STUDY_RELATED_SERIES, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NUMBER_OF_STUDY_RELATED_INSTANCES, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.INTERPRETATION_AUTHOR, VR.PN, TagTypeEnum.TagOptional));

            for (int i = 0; i < CustomQueryAttributes.Instance.PatientStudyList.Count; i++)
                TagTypeList.Add(new TagType(new Tag(Convert.ToUInt16(CustomQueryAttributes.Instance.PatientStudyList[i].group, 16),
                                                    Convert.ToUInt16(CustomQueryAttributes.Instance.PatientStudyList[i].element, 16))
                                            , TagTypeEnum.TagOptional));

			// add the specific character set attribute as a conditonal attribute at this level
			// - used purely to return the correct value in the Specific Character Set attribute of the C-FIND-RSP dataset
			TagTypeList.Add(new TagType(Tag.SPECIFIC_CHARACTER_SET, VR.CS, TagTypeEnum.TagConditional));

			// Add the Query Retrieve Level Attribute
			DvtkData.Dimse.Attribute attribute = new DvtkData.Dimse.Attribute(0x00080052, VR.CS, "STUDY");
			DataSet.Add(attribute);
		}
	}
}
