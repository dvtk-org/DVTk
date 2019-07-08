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

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using DvtkData.Dimse;
using DvtkData.ComparisonResults;
using Dvtk.CommonDataFormat;
using Dvtk.Results;

namespace Dvtk.Comparator
{
	/// <summary>
	/// Summary description for DicomComparator.
	/// </summary>
	public class DicomComparator : BaseComparator
	{
		private System.String _name = System.String.Empty;
		private DicomComparisonTemplate _template = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public DicomComparator(System.String name)
		{
			_name = name;
		}

		/// <summary>
		/// Property - Name
		/// </summary>
		public System.String Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// Property - Template
		/// </summary>
		public DicomComparisonTemplate Template
		{
			get
			{
				return _template;
			}
		}

		/// <summary>
		/// Initialize the DicomComparator
		/// </summary>
		/// <param name="dicomMessage"></param>
		/// <returns>bool - true = template initialized, false template not initialized</returns>
		public bool Initialize(DvtkData.Dimse.DicomMessage dicomMessage)
		{
			// Set up the comparator using the templates
			bool setUpOk = SetUp(dicomMessage);
			if (setUpOk == true)
			{
				// Load the template with the corresponding attribute values
				setUpOk = LoadTemplate(dicomMessage.DataSet);
			}

			return setUpOk;
		}

		public bool PopulateMessage(DvtkData.Dimse.DicomMessage dicomMessage, DicomComparator sourceComparator)
		{
			// Check for comparator equality
			if (this == sourceComparator)
			{
				return false;
			}

			// Set up the comparator using the templates
			bool setUpOk = SetUp(dicomMessage);
			if (setUpOk == true)
			{
				// Copy the source comparator values into the dicom message if the tags are in this comparator
				setUpOk = CopyToDicomMessage(dicomMessage, sourceComparator);
			}

			return setUpOk;
		}

		public bool SetUp(DvtkData.Dimse.DicomMessage dicomMessage)
		{
			DvtkData.Dimse.DimseCommand command = dicomMessage.CommandField;
			System.String sopClassUid = System.String.Empty;

			// To be fixed - why is the SOP Class UID not always filled in?
			// RB: TODO
			if (command == DvtkData.Dimse.DimseCommand.CSTORERQ)
			{
				sopClassUid = "1.2.840.10008.5.1.4.1.1.7";
			}

			DvtkData.Dimse.Attribute attribute = dicomMessage.CommandSet.GetAttribute(DvtkData.Dimse.Tag.AFFECTED_SOP_CLASS_UID);
			if (attribute == null)
			{
				attribute = dicomMessage.CommandSet.GetAttribute(DvtkData.Dimse.Tag.REQUESTED_SOP_CLASS_UID);
			}
			if ((attribute != null) &&
				(attribute.Length != 0))
			{
				UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
				sopClassUid = uniqueIdentifier.Values[0];
			}

			// Try to initialise a template
			_template = new DicomComparisonTemplate();
			bool setUpDone = _template.Initialize(command, sopClassUid);

			return setUpDone;
		}

		private bool LoadTemplate(DvtkData.Dimse.DataSet dataset)
		{
			if (dataset == null) return false;

			// try to find the template tag in the dataset
			foreach (DicomComparisonTag comparisonTag in this.Template.ComparisonTags)
			{
				DvtkData.Dimse.Tag tag = comparisonTag.Tag;
				DvtkData.Dimse.Tag parentSequenceTag = comparisonTag.ParentSequenceTag;
				System.String attributeValue = System.String.Empty;

				if (parentSequenceTag != Tag.UNDEFINED)
				{
					DvtkData.Dimse.Attribute sequenceAttribute = dataset.GetAttribute(parentSequenceTag);
					if ((sequenceAttribute != null) &&
						(sequenceAttribute.ValueRepresentation == DvtkData.Dimse.VR.SQ))
					{
						SequenceOfItems sequenceOfItems = (SequenceOfItems)sequenceAttribute.DicomValue;
						if (sequenceOfItems.Sequence.Count == 1)
						{
							SequenceItem item = sequenceOfItems.Sequence[0];

							if (item != null)
							{
								DvtkData.Dimse.Attribute attribute = item.GetAttribute(tag);
								attributeValue = GetAttributeValue(attribute);
							}
						}
					}
				}
				else
				{
					DvtkData.Dimse.Attribute attribute = dataset.GetAttribute(tag);
					attributeValue = GetAttributeValue(attribute);
				}

				if (attributeValue != System.String.Empty)
				{
					comparisonTag.DataFormat.FromDicomFormat(attributeValue);
				}
			}

			return true;
		}

		private System.String GetAttributeValue(DvtkData.Dimse.Attribute attribute)
		{
			System.String attributeValue = System.String.Empty;
			if ((attribute == null) ||
				(attribute.Length == 0))
			{
				return attributeValue;
			}

			switch(attribute.ValueRepresentation)
			{
				case VR.AE:
				{
					ApplicationEntity applicationEntity = (ApplicationEntity)attribute.DicomValue;
					attributeValue = applicationEntity.Values[0];
					break;
				}
				case VR.AS:
				{
					AgeString ageString = (AgeString)attribute.DicomValue;
					attributeValue = ageString.Values[0];
					break;
				}
				case VR.CS:
				{
					CodeString codeString = (CodeString)attribute.DicomValue;

					// Temp solution for more than one value
					for (int i = 0; i < codeString.Values.Count; i++)
					{
						attributeValue += (codeString.Values[i]);
						if ((i+1) < codeString.Values.Count)
						{
							attributeValue += "\\";
						}
					}
					
					break;
				}
				case VR.DA:
				{
					Date date = (Date)attribute.DicomValue;
					attributeValue = date.Values[0];
					break;
				}
				case VR.DS:
				{
					DecimalString decimalString = (DecimalString)attribute.DicomValue;
					attributeValue = decimalString.Values[0];
					break;
				}
				case VR.DT:
				{
					DvtkData.Dimse.DateTime dateTime = (DvtkData.Dimse.DateTime)attribute.DicomValue;
					attributeValue = dateTime.Values[0];
					break;
				}
				case VR.IS:
				{
					IntegerString integerString = (IntegerString)attribute.DicomValue;
					attributeValue = integerString.Values[0];
					break;
				}
				case VR.LO:
				{
					LongString longString = (LongString)attribute.DicomValue;
					attributeValue = longString.Values[0];
					break;
				}
				case VR.LT:
				{
					LongText longText = (LongText)attribute.DicomValue;
					attributeValue = longText.Value;
					break;
				}
				case VR.PN:
				{
					PersonName personName = (PersonName)attribute.DicomValue;
					attributeValue = personName.Values[0];
					break;
				}
				case VR.SH:
				{
					ShortString shortString = (ShortString)attribute.DicomValue;
					attributeValue = shortString.Values[0];
					break;
				}
				case VR.SQ:
				{
					// Special case looking for the SOP Class UID
					SequenceOfItems sequenceOfItems = (SequenceOfItems)attribute.DicomValue;
					if ((sequenceOfItems != null) &&
						(sequenceOfItems.Sequence.Count == 1))
					{
						// Special case looking for the SOP Class UID
						SequenceItem item = sequenceOfItems.Sequence[0];
						attribute = item.GetAttribute(new Tag(0x0008, 0x1150));
						attributeValue = GetAttributeValue(attribute);
					}
					break;
				}
				case VR.ST:
				{
					ShortText shortText = (ShortText)attribute.DicomValue;
					attributeValue = shortText.Value;
					break;
				}
				case VR.TM:
				{
					Time time = (Time)attribute.DicomValue;
					attributeValue = time.Values[0];
					break;
				}
				case VR.UI:
				{
					UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
					attributeValue = uniqueIdentifier.Values[0];
					break;
				}
				default:
					break;
			}

			return attributeValue;
		}

		private bool CopyToDicomMessage(DvtkData.Dimse.DicomMessage dicomMessage, DicomComparator sourceComparator)
		{
			bool messagePopulated = true;

			// Check if both templates have been initialized correctly
			if ((this.Template == null) ||
				(sourceComparator.Template == null))
			{
				return false;
			}

			// Iterate over this comparator
			foreach (DicomComparisonTag thisComparisonTag in this.Template.ComparisonTags)
			{
				// try to get the equivalent tag in the sourceComparator
				DicomComparisonTag sourceComparisonTag = sourceComparator.Template.ComparisonTags.Find(thisComparisonTag.Tag);
				if (sourceComparisonTag != null)
				{
					System.String stringValue = sourceComparisonTag.DataFormat.ToDicomFormat();
					DvtkData.Dimse.DataSet dataset = dicomMessage.DataSet;
					if (dataset != null)
					{
						// we need to see if the parent sequence has been set up in the dataset
						if (thisComparisonTag.ParentSequenceTag != Tag.UNDEFINED)
						{
							// set up the parent sequence and add it to the dataset
							SequenceOfItems sequenceOfItems = null;
							DvtkData.Dimse.Attribute sequenceAttribute = dataset.GetAttribute(thisComparisonTag.ParentSequenceTag);
							if (sequenceAttribute == null)
							{
								// add in an empty item
								DvtkData.Dimse.SequenceItem item = new SequenceItem();
								dataset.AddAttribute(thisComparisonTag.ParentSequenceTag.GroupNumber,
									thisComparisonTag.ParentSequenceTag.ElementNumber,
									VR.SQ,
									item);
								sequenceAttribute = dataset.GetAttribute(thisComparisonTag.ParentSequenceTag);
							}

							// get the sequence item and add in the required attribute
							sequenceOfItems = (SequenceOfItems)sequenceAttribute.DicomValue;
							if (sequenceOfItems.Sequence.Count == 1)
							{
								DvtkData.Dimse.SequenceItem item = sequenceOfItems.Sequence[0];
								if (item != null)
								{
									// add the attribute to the item
									if (sourceComparisonTag.Vr == VR.SQ)
									{
										// add in an empty item
										// TODO - fix this properly
										DvtkData.Dimse.SequenceItem item1 = new SequenceItem();
										item.AddAttribute(sourceComparisonTag.Tag.GroupNumber,
											sourceComparisonTag.Tag.ElementNumber,
											VR.SQ,
											item1);
									}
									else
									{
										// if the attribute already exists - then we need to remove it
										// - it was probably set to the default value
										DvtkData.Dimse.Attribute attribute = item.GetAttribute(sourceComparisonTag.Tag);
										if (attribute != null)
										{
											item.Remove(attribute);
										}

										// add the attribute to the item
										item.AddAttribute(sourceComparisonTag.Tag.GroupNumber,
											sourceComparisonTag.Tag.ElementNumber,
											sourceComparisonTag.Vr,
											stringValue);
									}
								}
							}
						}
						else
						{
							// if the attribute already exists - then we need to remove it
							// - it was probably set to the default value
							DvtkData.Dimse.Attribute attribute = dataset.GetAttribute(sourceComparisonTag.Tag);
							if (attribute != null)
							{
								dataset.Remove(attribute);
							}

							// add the attribute at the top level
							dataset.AddAttribute(sourceComparisonTag.Tag.GroupNumber,
							sourceComparisonTag.Tag.ElementNumber,
							sourceComparisonTag.Vr,
							stringValue);
						}
					}
				}
			}

			return messagePopulated;
		}

		/// <summary>
		/// Compare the two messages.
		/// </summary>
		/// <param name="tagValueFilterCollection">Tag Value Filter.</param>
		/// <param name="resultsReporter">Results reporter.</param>
		/// <param name="thatBaseComparator">Reference comparator.</param>
		/// <returns>bool - true = messages compared, false messages not compared</returns>
		public override bool Compare(TagValueCollection tagValueFilterCollection, ResultsReporter resultsReporter, BaseComparator thatBaseComparator)
		{
			bool compared = false;

			if (thatBaseComparator is DicomComparator)
			{
				DicomComparator thatDicomComparator = (DicomComparator)thatBaseComparator;

				// Check if both templates have been initialized correctly
				if ((this.Template == null) ||
					(thatDicomComparator.Template == null))
				{
					return false;
				}

				// Check for comparator equality
				if (this == thatDicomComparator)
				{
					return true;
				}

				// filter out comparators for the same message types
				if ((this.Template.Command == thatDicomComparator.Template.Command) &&
					(this.Template.SopClassUid == thatDicomComparator.Template.SopClassUid))
				{
					return false;
				}

				// generate a local Tag Value collection from this
				// - this collection will include any tag value pair from the original collection and
				// the tag value pairs of any tags only (in the original collection) that match from this
				// comparator - that is the values are taken from this comparator.
				TagValueCollection lTagValueFilterCollection = GenerateTagValueCollection(tagValueFilterCollection);

				// check to see if the comparision filters match - without Univeral Matching
				// - now try to match this local filter collection against thatDicomComparator
				// - comparators that match will have the same tag value pairs (including the value) as
				// each other.
				if ((tagValueFilterCollection.Count == lTagValueFilterCollection.Count) &&
					(thatDicomComparator.UseComparator(lTagValueFilterCollection, false) == true))
				{
					MessageComparisonResults messageComparisonResults 
						= new MessageComparisonResults(this.Name, 
						thatDicomComparator.Name, 
						this.Template.Command,
						thatDicomComparator.Template.Command, 
						this.Template.SopClassUid,
						thatDicomComparator.Template.SopClassUid);

					// Iterate over this comparator
					foreach (DicomComparisonTag thisComparisonTag in this.Template.ComparisonTags)
					{
						// try to get the equivalent tag in thatDicomComparator
						DicomComparisonTag thatComparisonTag = thatDicomComparator.Template.ComparisonTags.Find(thisComparisonTag.Tag);
						if (thatComparisonTag != null)
						{
							AttributeComparisonResults attributeComparisonResults 
								= new AttributeComparisonResults(thisComparisonTag.Tag, 
								thisComparisonTag.DataFormat.ToDicomFormat(), 
								thatComparisonTag.DataFormat.ToDicomFormat());

							if (thisComparisonTag.DataFormat.Equals(thatComparisonTag.DataFormat) == false)
							{
								DvtkData.Validation.ValidationMessage validationMessage = new DvtkData.Validation.ValidationMessage();
								validationMessage.Type = DvtkData.Validation.MessageType.Error;
								validationMessage.Message = "Attribute values do not match.";

								attributeComparisonResults.Messages.Add(validationMessage);
							}
							messageComparisonResults.Add(attributeComparisonResults);
						}
					}

					resultsReporter.WriteMessageComparisonResults(messageComparisonResults);

					compared = true;
				}
			}
			else if (thatBaseComparator is Hl7Comparator)
			{
				Hl7Comparator thatHl7Comparator = (Hl7Comparator)thatBaseComparator;

				// Check if both templates have been initialized correctly
				if ((this.Template == null) ||
					(thatHl7Comparator.Template == null))
				{
					return false;
				}

				// generate a local Tag Value collection from this
				// - this collection will include any tag value pair from the original collection and
				// the tag value pairs of any tags only (in the original collection) that match from this
				// comparator - that is the values are taken from this comparator.
				TagValueCollection lTagValueFilterCollection = GenerateTagValueCollection(tagValueFilterCollection);

				// check to see if the comparision filters match - without Univeral Matching
				// - now try to match this local filter collection against thatDicomComparator
				// - comparators that match will have the same tag value pairs (including the value) as
				// each other.
				if ((tagValueFilterCollection.Count == lTagValueFilterCollection.Count) &&
					(thatHl7Comparator.UseComparator(lTagValueFilterCollection, false) == true))
				{
					MessageComparisonResults messageComparisonResults 
						= new MessageComparisonResults(this.Name, 
						thatHl7Comparator.Name, 
						this.Template.Command,
						thatHl7Comparator.Template.MessageType, 
						this.Template.SopClassUid,
						thatHl7Comparator.Template.MessageSubType);

					// Iterate over this comparator
					foreach (DicomComparisonTag thisComparisonTag in this.Template.ComparisonTags)
					{
						// try to get the equivalent tag in thatHl7Comparator
						Hl7ComparisonTag thatComparisonTag = thatHl7Comparator.Template.ComparisonTags.Find(DicomHl7TagMapTemplate.DicomToHl7Tag(thisComparisonTag.Tag));
						if (thatComparisonTag != null)
						{
							AttributeComparisonResults attributeComparisonResults 
								= new AttributeComparisonResults(thisComparisonTag.Tag,
								SegmentNames.Name(thatComparisonTag.Tag.Segment),
								thatComparisonTag.Tag.FieldIndex,
								thisComparisonTag.DataFormat.ToDicomFormat(), 
								thatComparisonTag.DataFormat.ToHl7Format());

							if (thisComparisonTag.DataFormat.Equals(thatComparisonTag.DataFormat) == false)
							{
								DvtkData.Validation.ValidationMessage validationMessage = new DvtkData.Validation.ValidationMessage();
								validationMessage.Type = DvtkData.Validation.MessageType.Error;
								validationMessage.Message = "Attribute values do not match.";

								attributeComparisonResults.Messages.Add(validationMessage);
							}
							messageComparisonResults.Add(attributeComparisonResults);
						}
					}

					resultsReporter.WriteMessageComparisonResults(messageComparisonResults);

					compared = true;
				}
			}

			return compared;
		}

		private TagValueCollection GenerateTagValueCollection(TagValueCollection tagValueFilterCollection)
		{
			TagValueCollection localTagValueCollection = new TagValueCollection();

			// Check if the comparator can be used with a Universal Match on the Value of the Tag
			// - that is zero-length Value.
			if (this.UseComparator(tagValueFilterCollection, true) == true)
			{
				foreach(BaseTagValue baseTagValue in tagValueFilterCollection)
				{
					DicomTagValue dicomTagValue = null;

					if (baseTagValue is DicomTagValue)
					{
						dicomTagValue = (DicomTagValue)baseTagValue;
					}
					else if (baseTagValue is Hl7TagValue)
					{
						Hl7TagValue hl7TagValue = (Hl7TagValue)baseTagValue;
						dicomTagValue = new DicomTagValue(DicomHl7TagMapTemplate.Hl7ToDicomTag(hl7TagValue.Tag), hl7TagValue.Value);
					}

					// If the Value is empty (Universal Match) then try to get the actual
					// value from the comparator so that is actual value will be used against
					// other comparators.
					if (dicomTagValue.Tag != Tag.UNDEFINED)
					{
						if (dicomTagValue.Value == System.String.Empty)
						{
							// try to get a value for this Tag from this comparator
							System.String lValue = getValue(dicomTagValue.Tag);

							// Add a new Tag Value - with Value coming from this comparator
							// to the local filter.
							DicomTagValue lDicomTagValue = new DicomTagValue(dicomTagValue.Tag, lValue);
							localTagValueCollection.Add(lDicomTagValue);
						}
						else
						{
							// Just add the given Tag Value pair to the local filter
							localTagValueCollection.Add(dicomTagValue);
						}
					}
				}
			}

			// Return the local filter
			return localTagValueCollection;
		}

		public bool UseComparator(TagValueCollection tagValueFilterCollection, bool universalMatchAllowed)
		{
			int filterCounter = 0;

			// Check if the filter values match those in the template
			foreach(BaseTagValue baseTagValue in tagValueFilterCollection)
			{
				DicomTagValue dicomTagValue = null;

				if (baseTagValue is DicomTagValue)
				{
					dicomTagValue = (DicomTagValue)baseTagValue;
				}
				else if (baseTagValue is Hl7TagValue)
				{
					Hl7TagValue hl7TagValue = (Hl7TagValue)baseTagValue;
					dicomTagValue = new DicomTagValue(DicomHl7TagMapTemplate.Hl7ToDicomTag(hl7TagValue.Tag), hl7TagValue.Value);
				}

				if (dicomTagValue.Tag != Tag.UNDEFINED)
				{
					foreach (DicomComparisonTag thisComparisonTag in this.Template.ComparisonTags)
					{
						// Tags are the same
						if (thisComparisonTag.Tag == dicomTagValue.Tag)
						{
							if (universalMatchAllowed == true)
							{
								// When universal matching either a zero-length or exact match are OK
								if ((dicomTagValue.Value == System.String.Empty) ||
									(thisComparisonTag.DataFormat.ToDicomFormat() == dicomTagValue.Value))
								{
									filterCounter++;
								}
							}
							else if ((universalMatchAllowed == false) &&
								(thisComparisonTag.DataFormat.ToDicomFormat() == dicomTagValue.Value))
							{
								// Not universal matching so only an exact match is OK
								filterCounter++;
							}
							break;
						}
					}
				}
			}

			bool useThisComparator = (tagValueFilterCollection.Count == filterCounter) ? true : false;

			return useThisComparator;
		}

		private System.String getValue(Tag tag)
		{
			System.String lValue = System.String.Empty;

			// Try to gat a value for the gievn tag from this comparator.
			foreach (DicomComparisonTag thisComparisonTag in this.Template.ComparisonTags)
			{
				if (thisComparisonTag.Tag == tag)
				{
					lValue = thisComparisonTag.DataFormat.ToDicomFormat();
					break;
				}
			}

			return lValue;
		}
	}
}
