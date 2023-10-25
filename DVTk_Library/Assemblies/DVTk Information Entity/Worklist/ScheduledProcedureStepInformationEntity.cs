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
	/// Summary description for ScheduledProcedureStepInformationEntity.
	/// </summary>
	public class ScheduledProcedureStepInformationEntity : BaseInformationEntity
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public ScheduledProcedureStepInformationEntity() : base("SCHEDULED PROCEDURE STEP") {}

		/// <summary>
		/// Set the Default Tag Type List for this Entity.
		/// </summary>
		protected override void SetDefaultTagTypeList()
		{
			// attributes from the Scheduled Procedure Step Module
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_STATION_AE_TITLE, VR.AE, TagTypeEnum.TagRequired));
            TagTypeList.Add(new TagType(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, VR.SQ, TagTypeEnum.TagRequired));
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_PROCEDURE_STEP_START_DATE, VR.DA, TagTypeEnum.TagRequired));
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_PROCEDURE_STEP_START_TIME, VR.TM, TagTypeEnum.TagRequired));
            //TagTypeList.Add(new TagType(Tag.MODALITY, VR.CS, TagTypeEnum.TagRequired));
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_PERFORMING_PHYSICIANS_NAME, VR.PN, TagTypeEnum.TagRequired));
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, VR.LO, TagTypeEnum.TagOptional));
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_STATION_NAME, VR.SH, TagTypeEnum.TagOptional));
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_PROCEDURE_STEP_LOCATION, VR.SH, TagTypeEnum.TagOptional));
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_ACTION_ITEM_CODE_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
            //TagTypeList.Add(new TagType(Tag.PRE_MEDICATION, VR.LO, TagTypeEnum.TagOptional));
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_PROCEDURE_STEP_ID, VR.SH, TagTypeEnum.TagOptional));
            //TagTypeList.Add(new TagType(Tag.REQUESTED_CONTRAST_AGENT, VR.LO, TagTypeEnum.TagOptional));
            //TagTypeList.Add(new TagType(Tag.SCHEDULED_PROCEDURE_STEP_STATUS, VR.CS, TagTypeEnum.TagOptional));
            // plus all other attributes from the Scheduled Procedure Step Module
            TagTypeList.Add(new TagType(Tag.SCHEDULED_PERFORMING_PHYSICIANS_IDENTIFICATION_SEQUENCE, VR.SQ, TagTypeEnum.TagOptional));
            TagTypeList.Add(new TagType(Tag.COMMENTS_ON_THE_SCHEDULED_PROCEDURE_STEP, VR.LT, TagTypeEnum.TagOptional));

            // add the specific character set attribute as a conditonal attribute at this level
            // - used purely to return the correct value in the Specific Character Set attribute of the C-FIND-RSP dataset
            TagTypeList.Add(new TagType(Tag.SPECIFIC_CHARACTER_SET, VR.CS, TagTypeEnum.TagConditional));
		}

		/// <summary>
		/// Copy from the given source Dataset into the local Dataset as defined by the
		/// default Tag Type list. In addition to the base copy we need to copy attributes from the
		/// Request Attributes Sequence (if present) and the Scheduled Procedure Step (if present).
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

			// check if the Scheduled Procedure Step Sequence is available in the source dataset
			DvtkData.Dimse.Attribute scheduledProcedureStepSequence = sourceDataset.GetAttribute(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE);
			if (scheduledProcedureStepSequence != null)
			{
				SequenceOfItems sequenceOfItems = (SequenceOfItems)scheduledProcedureStepSequence.DicomValue;
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
		/// Request Attributes Sequence (if present) or Scheduled Procedure Step Sequence (if present).
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
						itemTagTypeList.Add(new TagType(Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, TagTypeEnum.TagOptional));
						itemTagTypeList.Add(new TagType(Tag.SCHEDULED_ACTION_ITEM_CODE_SEQUENCE, TagTypeEnum.TagOptional));
						itemTagTypeList.Add(new TagType(Tag.SCHEDULED_PROCEDURE_STEP_ID, TagTypeEnum.TagOptional));
					
						DvtkData.Dimse.SequenceItem item = sequenceOfItems.Sequence[0];

						// check if found in item
						isFoundIn = base.IsFoundIn(itemTagTypeList, item);
					}
				}

				// check if the Scheduled Procedure Step Sequence is available in the match dataset
				DvtkData.Dimse.Attribute scheduledProcedureStepSequence = matchDataset.GetAttribute(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE);
				if (scheduledProcedureStepSequence != null)
				{
					SequenceOfItems sequenceOfItems = (SequenceOfItems)scheduledProcedureStepSequence.DicomValue;
					if (sequenceOfItems.Sequence.Count == 1)
					{	
						DvtkData.Dimse.SequenceItem item = sequenceOfItems.Sequence[0];

						// check if found in item - use default tag list
						isFoundIn = base.IsFoundIn(item);
					}
				}
			}

			return isFoundIn;
		}
	}
}
