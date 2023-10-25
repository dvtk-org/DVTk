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
	/// Summary description for RequestedProcedureInformationEntity.
	/// </summary>
	public class RequestedProcedureInformationEntity : BaseInformationEntity
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public RequestedProcedureInformationEntity() : base("REQUESTED PROCEDURE") {}

		/// <summary>
		/// Set the Default Tag Type List for this Entity.
		/// </summary>
		protected override void SetDefaultTagTypeList()
		{
			// attributes from the Requested Procedure Module
			TagTypeList.Add(new TagType(Tag.REQUESTED_PROCEDURE_ID, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REQUESTED_PROCEDURE_DESCRIPTION, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REQUESTED_PROCEDURE_CODE_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.STUDY_INSTANCE_UID, VR.UI, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REFERENCED_STUDY_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REQUESTED_PROCEDURE_PRIORITY, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.PATIENT_TRANSPORT_ARRANGEMENTS, VR.LO, TagTypeEnum.TagOptional));
			// plus all other attributes from the Requested Procedure Module
			TagTypeList.Add(new TagType(Tag.REASON_FOR_THE_REQUESTED_PROCEDURE, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REQUESTED_PROCEDURE_LOCATION, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REQUESTED_PROCEDURE_COMMENTS, VR.LT, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.CONFIDENTIALITY_CODE, VR.LO, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.REPORTING_PRIORITY, VR.SH, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.NAMES_OF_INTENDED_RECIPIENTS_OF_RESULTS, VR.PN, TagTypeEnum.TagOptional));
			TagTypeList.Add(new TagType(Tag.INTENDED_RECIPIENTS_OF_RESULTS_IDENTIFICATION_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
		}

		/// <summary>
		/// Copy from the given source Dataset into the local Dataset as defined by the
		/// default Tag Type list. In addition to the base copy we need to copy attributes from the
		/// Request Attributes Sequence (if present).
		/// </summary>
		/// <param name="sourceDataset">Source Dataset used to populate the local Dataset.</param>
		public override void CopyFrom(AttributeSet sourceDataset)
		{
			// perform base copy
			base.CopyFrom(sourceDataset);

			// check if the Request Attributes Sequence is available in the source dataset
			DvtkData.Dimse.Attribute requestAttributesSequence = sourceDataset.GetAttribute(Tag.REQUEST_ATTRIBUTES_SEQUENCE);
			if (requestAttributesSequence != null)
			{
				SequenceOfItems sequenceOfItems = (SequenceOfItems)requestAttributesSequence.DicomValue;
				if (sequenceOfItems.Sequence.Count == 1)
				{
					DvtkData.Dimse.SequenceItem item = sequenceOfItems.Sequence[0];

					// copy item attributes too
					base.CopyFrom(item);
				}
			}
		}

		/// <summary>
		/// Check if the given match dataset is found in the local dataset using the default Tag Type list. 
		/// A check is made to see if all the attributes in the given match dataset are present in the local
		/// dataset. In addition to the base match we need to try to match attributes from the
		/// Request Attributes Sequence (if present).
		/// </summary>
		/// <param name="matchDataset">Match dataset to check.</param>
		/// <returns>Boolean indicating if the match attributes are present in the local dataset.</returns>
		public override bool IsFoundIn(AttributeSet matchDataset)
		{
			bool isFoundIn = base.IsFoundIn(matchDataset);

			if (isFoundIn == false)
			{
				// check if the Request Attributes Sequence is available in the match dataset
				DvtkData.Dimse.Attribute requestAttributesSequence = matchDataset.GetAttribute(Tag.REQUEST_ATTRIBUTES_SEQUENCE);
				if (requestAttributesSequence != null)
				{
					SequenceOfItems sequenceOfItems = (SequenceOfItems)requestAttributesSequence.DicomValue;
					if (sequenceOfItems.Sequence.Count == 1)
					{
						// set up a temporary tag list to check the relevant tags in the Request Attributes Sequence
						TagTypeList itemTagTypeList = new TagTypeList();
						itemTagTypeList.Add(new TagType(Tag.REQUESTED_PROCEDURE_ID, TagTypeEnum.TagOptional));
						
						DvtkData.Dimse.SequenceItem item = sequenceOfItems.Sequence[0];

						// check if found in item
						isFoundIn = base.IsFoundIn(itemTagTypeList, item);
					}
				}
			}

			return isFoundIn;
		}
	}
}
