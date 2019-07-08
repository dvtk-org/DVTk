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
using DvtkData.Collections;
using DvtkData.Dimse;
using Dvtk.Dicom.InformationEntity;

namespace Dvtk.Dicom.InformationEntity.QueryRetrieve
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
        /// Gets the filenames of all (indirect) child instance information entities.
        /// </summary>
        public StringCollection FileNames
        {
            get
            {
                StringCollection fileNames = new StringCollection();

                foreach (StudyInformationEntity studyInformationEntity in Children)
                {
                    StringCollection studyInformationEntityFileNames = studyInformationEntity.FileNames;

                    foreach (String fileName in studyInformationEntityFileNames)
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
			TagTypeList.Add(new TagType(Tag.PATIENTS_NAME, VR.PN, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.PATIENT_ID, VR.LO, TagTypeEnum.TagUnique));
            TagTypeList.Add(new TagType(Tag.ISSUER_OF_PATIENT_ID, VR.LO, TagTypeEnum.TagOptional));            
			TagTypeList.Add(new TagType(Tag.REFERENCED_PATIENT_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_BIRTH_DATE, VR.DA, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_BIRTH_TIME, VR.TM, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENTS_SEX, VR.CS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.OTHER_PATIENT_IDS, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.OTHER_PATIENT_NAMES, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ETHNIC_GROUP, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENT_COMMENTS, VR.LT, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NUMBER_OF_PATIENT_RELATED_STUDIES, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NUMBER_OF_PATIENT_RELATED_SERIES, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NUMBER_OF_PATIENT_RELATED_INSTANCES, VR.IS, TagTypeEnum.TagOptional));
            for (int i = 0; i < CustomQueryAttributes.Instance.PatientList.Count; i++)
                TagTypeList.Add(new TagType(new Tag(Convert.ToUInt16(CustomQueryAttributes.Instance.PatientList[i].group, 16),
                                                    Convert.ToUInt16(CustomQueryAttributes.Instance.PatientList[i].element, 16))
                                            , TagTypeEnum.TagOptional));

            
			// add the specific character set attribute as a conditonal attribute at this level
			// - used purely to return the correct value in the Specific Character Set attribute of the C-FIND-RSP dataset
			TagTypeList.Add(new TagType(Tag.SPECIFIC_CHARACTER_SET, VR.CS, TagTypeEnum.TagConditional));

			// Add the Query Retrieve Level Attribute
			DvtkData.Dimse.Attribute attribute = new DvtkData.Dimse.Attribute(0x00080052, VR.CS, "PATIENT");
			DataSet.Add(attribute);
		}
	}
}
