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
	/// Summary description for ImagingServiceRequestInformationEntity.
	/// </summary>
	public class ImagingServiceRequestInformationEntity : BaseInformationEntity
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public ImagingServiceRequestInformationEntity() : base("IMAGING SERVICE REQUEST") {}

		/// <summary>
		/// Set the Default Tag Type List for this Entity.
		/// </summary>
		protected override void SetDefaultTagTypeList()
		{
			// attributes from the Imaging Service Request Module
			TagTypeList.Add(new TagType(Tag.ACCESSION_NUMBER, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REQUESTING_PHYSICIAN, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REFERRING_PHYSICIANS_NAME, VR.PN, TagTypeEnum.TagOptional));
			// plus all other attributes from the Imaging Service Request Module
			TagTypeList.Add(new TagType(Tag.REQUESTING_PHYSICIAN_IDENTIFICATION_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REFERRING_PHYSICIAN_IDENTIFICATION_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REQUESTING_SERVICE, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PLACER_ORDER_NUMBER_IMAGING_SERVICE_REQUEST, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.FILLER_ORDER_NUMBER_IMAGING_SERVICE_REQUEST, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.IMAGING_SERVICE_REQUEST_COMMENTS, VR.LT, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ISSUE_DATE_OF_IMAGING_SERVICE_REQUEST, VR.DA, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ISSUE_TIME_OF_IMAGING_SERVICE_REQUEST, VR.TM, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ORDER_ENTERED_BY, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ORDER_ENTERERS_LOCATION, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.ORDER_CALLBACK_PHONE_NUMBER, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REASON_FOR_THE_IMAGING_SERVICE_REQUEST, VR.LO, TagTypeEnum.TagOptional));
		}
	}
}
