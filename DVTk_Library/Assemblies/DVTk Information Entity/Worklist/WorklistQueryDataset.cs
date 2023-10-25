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
	/// Summary description for WorklistQueryDataset.
	/// </summary>
	public class WorklistQueryDataset
	{
        /// <summary>
        /// Create a Worklist Response Dataset from the given user defined tags and teh default manager values.
        /// </summary>
        /// <param name="userDefinedTags">User defined Tags.</param>
        /// <returns>DvtkData.Dimse.DataSet - Modality Worklist Response Dataset.</returns>
        public static DvtkData.Dimse.DataSet CreateWorklistItemDataset(TagValueCollection userDefinedTags)
        {
            DvtkData.Dimse.DataSet dataset = new DvtkData.Dimse.DataSet("Modality Worklist Information Model - FIND SOP Class");

            // update the user defined tags with any parent sequence information needed
            UpdateTagParentSequences(userDefinedTags);

            // add the default return keys
            AddDefaultReturnKeys(dataset);

            // add the user defined keys
            AddTagsToDataset(userDefinedTags, dataset);

            return dataset;
        }

		/// <summary>
		/// Create a Worklist Query Dataset from the given query tags and the default return keys.
		/// </summary>
		/// <param name="queryTags">Query Tags.</param>
		/// <returns>DvtkData.Dimse.DataSet - Modality Worklist Query Dataset.</returns>
		public static DvtkData.Dimse.DataSet CreateWorklistQueryDataset(TagValueCollection queryTags)
		{
			DvtkData.Dimse.DataSet dataset = new DvtkData.Dimse.DataSet("Modality Worklist Information Model - FIND SOP Class");

			// update the query tags with any parent sequence information needed
            UpdateTagParentSequences(queryTags);

			// add the default return keys
			AddDefaultReturnKeys(dataset);

			// add the query keys
            AddTagsToDataset(queryTags, dataset);

			return dataset;
		}

        private static void UpdateTagParentSequences(TagValueCollection tags)
		{
			ScheduledProcedureStepInformationEntity scheduledProcedureStepIe = new ScheduledProcedureStepInformationEntity();
			foreach(DicomTagValue tag in tags)
			{
                foreach (TagType tagType in scheduledProcedureStepIe.TagTypeList)
                {
                    // special check to be made on the Specific Character Set tag
                    // - this tag is added to the scheduledProcedureStepIe.TagTypeList purely to
                    // be able to return the correct values in the query response - we should not set
                    // the ParentSequenceTag for this attribute
                    // - this is a workaround - but please do not change it!
                    if ((tag.Tag == tagType.Tag) &&
                        (tag.Tag != Tag.SPECIFIC_CHARACTER_SET))
                    {
                        // set the parent sequence in the tag
                        tag.ParentSequenceTag = Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE;
                        break;
                    }
                }
			}
		}

		private static void AddDefaultReturnKeys(DvtkData.Dimse.DataSet dataset)
		{
			// use the Worklist Information Entities to generate the default Return Key attribute set
			PatientInformationEntity patientIe = new PatientInformationEntity();
			foreach(TagType tagType in patientIe.TagTypeList)
			{
				dataset.AddAttribute(tagType.Tag.GroupNumber, tagType.Tag.ElementNumber, tagType.Vr);
			}
			VisitInformationEntity visitIe = new VisitInformationEntity();
			foreach(TagType tagType in visitIe.TagTypeList)
			{
				dataset.AddAttribute(tagType.Tag.GroupNumber, tagType.Tag.ElementNumber, tagType.Vr);
			}
			ImagingServiceRequestInformationEntity imagingServiceRequestIe = new ImagingServiceRequestInformationEntity();
			foreach(TagType tagType in imagingServiceRequestIe.TagTypeList)
			{
				dataset.AddAttribute(tagType.Tag.GroupNumber, tagType.Tag.ElementNumber, tagType.Vr);
			}
			RequestedProcedureInformationEntity requestedProcedureIe = new RequestedProcedureInformationEntity();
			foreach(TagType tagType in requestedProcedureIe.TagTypeList)
			{
				dataset.AddAttribute(tagType.Tag.GroupNumber, tagType.Tag.ElementNumber, tagType.Vr);
			}

			SequenceItem item = new SequenceItem();
			ScheduledProcedureStepInformationEntity scheduledProcedureStepIe = new ScheduledProcedureStepInformationEntity();
			foreach(TagType tagType in scheduledProcedureStepIe.TagTypeList)
			{
                if (tagType.Tag != Tag.SPECIFIC_CHARACTER_SET)
                {
                    item.AddAttribute(tagType.Tag.GroupNumber, tagType.Tag.ElementNumber, tagType.Vr);
                }
			}
			dataset.AddAttribute(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE.GroupNumber,
				Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE.ElementNumber, VR.SQ, item);
		}

        private static void AddTagsToDataset(TagValueCollection tags, DvtkData.Dimse.DataSet dataset)
		{
			// iterate over the tags
			foreach(DicomTagValue tag in tags)
			{
				if (tag.ParentSequenceTag != Tag.UNDEFINED)
				{
					// try to get the sequence tag in the dataset
					DvtkData.Dimse.Attribute sequenceAttribute = dataset.GetAttribute(tag.ParentSequenceTag);
					if ((sequenceAttribute != null) &&
						(sequenceAttribute.ValueRepresentation == DvtkData.Dimse.VR.SQ))
					{
						SequenceOfItems sequenceOfItems = (SequenceOfItems)sequenceAttribute.DicomValue;
						if (sequenceOfItems.Sequence.Count == 1)
						{
							SequenceItem item = sequenceOfItems.Sequence[0];

							if (item != null)
							{
								VR vr = VR.UN;

								// try to get the attribute in the item
								DvtkData.Dimse.Attribute attribute = item.GetAttribute(tag.Tag);
								if (attribute != null)
								{
									vr = attribute.ValueRepresentation;
									item.Remove(attribute);
								}

								// add the query value
								item.AddAttribute(tag.Tag.GroupNumber,
                                    tag.Tag.ElementNumber,
									vr,
									tag.Value);
							}
						}
					}
				}
				else
				{
					VR vr = VR.UN;

					// try to get the attribute in the dataset
					DvtkData.Dimse.Attribute attribute = dataset.GetAttribute(tag.Tag);
					if (attribute != null)
					{
						vr = attribute.ValueRepresentation;
						dataset.Remove(attribute);
					}

                    // special check for the SPECIFIC CHARACTER SET attribute
                    if (tag.Tag == Tag.SPECIFIC_CHARACTER_SET)
                    {
                        vr = VR.CS;
                    }

					// add the query value
					dataset.AddAttribute(tag.Tag.GroupNumber,
						tag.Tag.ElementNumber,
						vr,
						tag.Value);
				}
			}
		}

		/// <summary>
		/// Create a Worklist Query Dataset from the given DCM file. We assume that the DCM file
		/// contains the appropriate MWL Query Dataset. If the scheduled procedure step start date
		/// is present in the DCM file, it will be overwritten with the value given by scheduledProcedureStepStartDate.
		/// </summary>
		/// <param name="mwlQueryDcmFilename">MWL Query Dcm Filename.</param>
		/// <param name="userDefinedScheduledProcedureStepStartDate">User Defined Scheduled Procedure Step Start Date.</param>
		/// <returns>DvtkData.Dimse.DataSet - Modality Worklist Query Dataset.</returns>
		public static DvtkData.Dimse.DataSet CreateWorklistQueryDataset(System.String mwlQueryDcmFilename, System.String userDefinedScheduledProcedureStepStartDate)
		{
			// Read the DCM file
			DataSet dataset = Dvtk.DvtkDataHelper.ReadDataSetFromFile(mwlQueryDcmFilename);
			dataset.IodId = "Modality Worklist Information Model - FIND SOP Class";

			// Update the scheduled procedure step start date (if present)
			if (userDefinedScheduledProcedureStepStartDate != System.String.Empty)
			{
				DvtkData.Dimse.Attribute scheduledProcedureStepSequence = dataset.GetAttribute(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE);
				if (scheduledProcedureStepSequence != null)
				{
					SequenceOfItems sequenceOfItems = (SequenceOfItems)scheduledProcedureStepSequence.DicomValue;
					if (sequenceOfItems.Sequence.Count == 1)
					{
						SequenceItem queryItem = sequenceOfItems.Sequence[0];

						// Try to get the Scheduled Procedure Step Start Date
						// - update it if there is a value defined in the dataset
						DvtkData.Dimse.Attribute scheduledProcedureStepStartDate = queryItem.GetAttribute(Tag.SCHEDULED_PROCEDURE_STEP_START_DATE);
						if ((scheduledProcedureStepStartDate != null) &&
							(scheduledProcedureStepStartDate.Length != 0))
						{
							// Remove the existing attribute
							queryItem.Remove(scheduledProcedureStepStartDate);

							// Modify value to today's date
							queryItem.AddAttribute(Tag.SCHEDULED_PROCEDURE_STEP_START_DATE.GroupNumber,
								Tag.SCHEDULED_PROCEDURE_STEP_START_DATE.ElementNumber, 
								DvtkData.Dimse.VR.DA,
								userDefinedScheduledProcedureStepStartDate);
						}
					}
				}
			}

			return dataset;
		}
	}
}
