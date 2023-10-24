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

namespace Dvtk.Dicom.InformationEntity.QueryRetrieve
{
	/// <summary>
	/// Summary description for InstanceInformationEntity.
	/// </summary>
	public class InstanceInformationEntity : BaseInformationEntity
	{
		private System.String _filename;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="filename">Filename containing this entity and all parent entities (as a composite).</param>
		public InstanceInformationEntity(System.String filename) : base("IMAGE")
		{
			_filename = filename;
		}

		/// <summary>
		/// Get the name of the file containing this entity and all parents as a composite.
		/// </summary>
		public System.String Filename	
		{
			get 
			{
				return _filename;
			}
		}

		/// <summary>
		/// Set the Default Tag Type List for this Entity.
		/// </summary>
		protected override void SetDefaultTagTypeList()
		{
			TagTypeList.Add(new TagType(Tag.SPECIFIC_CHARACTER_SET, VR.CS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.INSTANCE_NUMBER, VR.IS, TagTypeEnum.TagRequired));
			TagTypeList.Add(new TagType(Tag.OVERLAY_NUMBER, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.CURVE_NUMBER, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.LUT_NUMBER, VR.IS, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.SOP_INSTANCE_UID, VR.UI, TagTypeEnum.TagUnique));
			// plus all other attributes at an instance level!
            TagTypeList.Add(new TagType(Tag.SOP_CLASS_UID, VR.UI, TagTypeEnum.TagOptional));
            TagTypeList.Add(new TagType(new Tag(0x0008,0x001A), VR.UI, TagTypeEnum.TagOptional));
            TagTypeList.Add(new TagType(Tag.ROWS, VR.US, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.COLUMNS, VR.US, TagTypeEnum.TagOptional));

            for (int i = 0; i < CustomQueryAttributes.Instance.InstanceList.Count; i++)
                TagTypeList.Add(new TagType(new Tag(Convert.ToUInt16(CustomQueryAttributes.Instance.InstanceList[i].group, 16),
                                                    Convert.ToUInt16(CustomQueryAttributes.Instance.InstanceList[i].element, 16))
                                            , TagTypeEnum.TagOptional));

			// add the specific character set attribute as a conditonal attribute at this level
			// - used purely to return the correct value in the Specific Character Set attribute of the C-FIND-RSP dataset
			TagTypeList.Add(new TagType(Tag.SPECIFIC_CHARACTER_SET, VR.CS, TagTypeEnum.TagConditional));

			// Add the Query Retrieve Level Attribute
			DvtkData.Dimse.Attribute attribute = new DvtkData.Dimse.Attribute(0x00080052, VR.CS, "IMAGE");
			DataSet.Add(attribute);
		}
	}
}
