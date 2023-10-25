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
	/// Summary description for SeriesInformationEntity.
	/// </summary>
	public class SeriesInformationEntity : BaseInformationEntity
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public SeriesInformationEntity() : base("SERIES") {}

        /// <summary>
        /// Gets the filenames of all child instance information entities.
        /// </summary>
        public StringCollection FileNames
        {
            get
            {
                StringCollection fileNames = new StringCollection();

                foreach (InstanceInformationEntity instanceInformationEntity in Children)
                {
                    if (instanceInformationEntity.Filename != null)
                        fileNames.Add(instanceInformationEntity.Filename);
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
			TagTypeList.Add(new TagType(Tag.MODALITY, VR.CS, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.SERIES_NUMBER, VR.IS, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.SERIES_INSTANCE_UID, VR.UI, TagTypeEnum.TagUnique));
			TagTypeList.Add(new TagType(Tag.NUMBER_OF_SERIES_RELATED_INSTANCES, VR.IS, TagTypeEnum.TagOptional));
			// plus all other attributes at a series level!
            for (int i = 0; i < CustomQueryAttributes.Instance.SeriesList.Count; i++)
                TagTypeList.Add(new TagType(new Tag(Convert.ToUInt16(CustomQueryAttributes.Instance.SeriesList[i].group,16),
                                                    Convert.ToUInt16(CustomQueryAttributes.Instance.SeriesList[i].element,16))
                                            , TagTypeEnum.TagOptional));
           



			// add the specific character set attribute as a conditonal attribute at this level
			// - used purely to return the correct value in the Specific Character Set attribute of the C-FIND-RSP dataset
			TagTypeList.Add(new TagType(Tag.SPECIFIC_CHARACTER_SET, VR.CS, TagTypeEnum.TagConditional));

			// Add the Query Retrieve Level Attribute
			DvtkData.Dimse.Attribute attribute = new DvtkData.Dimse.Attribute(0x00080052, VR.CS, "SERIES");
			DataSet.Add(attribute);
		}	
	}
}
