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
    /// Summary description for Hl7Comparator.
    /// </summary>
    public class Hl7Comparator : BaseComparator
    {
        private System.String _name = System.String.Empty;
        private Hl7ComparisonTemplate _template = null;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public Hl7Comparator(System.String name)
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
        public Hl7ComparisonTemplate Template
        {
            get
            {
                return _template;
            }
        }

        /// <summary>
        /// Initialize the Hl7Comparator
        /// </summary>
        /// <param name="hl7Message"></param>
        /// <returns>bool - true = template initialized, false template not initialized</returns>
        public bool Initialize(Hl7Message hl7Message)
        {
            // Set up the comparator using the templates
            bool setUpOk = SetUp(hl7Message);
            if (setUpOk == true)
            {
                // Load the template with the corresponding attribute values
                setUpOk = LoadTemplate(hl7Message);
            }

            return setUpOk;
        }

        public bool PopulateMessage(Hl7Message hl7Message, Hl7Comparator sourceComparator)
        {
            // Check for comparator equality
            if (this == sourceComparator)
            {
                return false;
            }

            // Set up the comparator using the templates
            bool setUpOk = SetUp(hl7Message);
            if (setUpOk == true)
            {
                // Copy the source comparator values into the HL7 message if the tags are in this comparator
                setUpOk = CopyToHl7Message(hl7Message, sourceComparator);
            }

            return setUpOk;
        }

        public bool SetUp(Hl7Message hl7Message)
        {
            System.String messageType = hl7Message.MessageType;
            System.String messageSubType = hl7Message.MessageSubType;

            // Try to initialise a template
            _template = new Hl7ComparisonTemplate();
            bool setUpDone = _template.Initialize(messageType, messageSubType);

            return setUpDone;
        }

        private bool LoadTemplate(Hl7Message hl7Message)
        {
            if (hl7Message == null) return false;

            // try to find the template tag in the dataset
            foreach (Hl7ComparisonTag comparisonTag in this.Template.ComparisonTags)
            {
                Hl7Tag tag = comparisonTag.Tag;
                System.String attributeValue = hl7Message.Value(tag.Segment, tag.FieldIndex);

                if (attributeValue != System.String.Empty)
                {
                    comparisonTag.DataFormat.FromHl7Format(attributeValue);
                }
            }

            return true;
        }

        private bool CopyToHl7Message(Hl7Message hl7Message, Hl7Comparator sourceComparator)
        {
            bool messagePopulated = true;

            // Check if both templates have been initialized correctly
            if ((this.Template == null) ||
                (sourceComparator.Template == null))
            {
                return false;
            }

            // Iterate over this comparator
            foreach (Hl7ComparisonTag thisComparisonTag in this.Template.ComparisonTags)
            {
                // try to get the equivalent tag in the sourceComparator
                Hl7ComparisonTag sourceComparisonTag = sourceComparator.Template.ComparisonTags.Find(thisComparisonTag.Tag);
                if (sourceComparisonTag != null)
                {
                    System.String stringValue = sourceComparisonTag.DataFormat.ToHl7Format();
                    if (hl7Message != null)
                    {
                        // add the value
                        hl7Message.AddValue(sourceComparisonTag.Tag.Segment,
                                            sourceComparisonTag.Tag.FieldIndex,
                                            stringValue);
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

            if (thatBaseComparator is Hl7Comparator)
            {
                Hl7Comparator thatHl7Comparator = (Hl7Comparator)thatBaseComparator;

                // Check if both templates have been initialized correctly
                if ((this.Template == null) ||
                    (thatHl7Comparator.Template == null))
                {
                    return false;
                }

                // Check for comparator equality
                if (this == thatHl7Comparator)
                {
                    return true;
                }

                // filter out comparators for the same message types
                if ((this.Template.MessageType == thatHl7Comparator.Template.MessageType) &&
                    (this.Template.MessageSubType == thatHl7Comparator.Template.MessageSubType))
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
                        this.Template.MessageType,
                        thatHl7Comparator.Template.MessageType,
                        this.Template.MessageSubType,
                        thatHl7Comparator.Template.MessageSubType);

                    // Iterate over this comparator
                    foreach (Hl7ComparisonTag thisComparisonTag in this.Template.ComparisonTags)
                    {
                        // try to get the equivalent tag in thatHl7Comparator
                        Hl7ComparisonTag thatComparisonTag = thatHl7Comparator.Template.ComparisonTags.Find(thisComparisonTag.Tag);
                        if (thatComparisonTag != null)
                        {
                            AttributeComparisonResults attributeComparisonResults
                                = new AttributeComparisonResults(SegmentNames.Name(thisComparisonTag.Tag.Segment),
                                thisComparisonTag.Tag.FieldIndex,
                                thisComparisonTag.DataFormat.ToHl7Format(),
                                thatComparisonTag.DataFormat.ToHl7Format());
                            attributeComparisonResults.Name = DicomHl7TagMapTemplate.Hl7NameFromHl7Tag(thisComparisonTag.Tag);
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
            else if (thatBaseComparator is DicomComparator)
            {
                DicomComparator thatDicomComparator = (DicomComparator)thatBaseComparator;

                // Check if both templates have been initialized correctly
                if ((this.Template == null) ||
                    (thatDicomComparator.Template == null))
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
                        this.Template.MessageType,
                        thatDicomComparator.Template.Command,
                        this.Template.MessageSubType,
                        thatDicomComparator.Template.SopClassUid);

                    // Iterate over this comparator
                    foreach (Hl7ComparisonTag thisComparisonTag in this.Template.ComparisonTags)
                    {
                        // try to get the equivalent tag in thatDicomComparator
                        DicomComparisonTag thatComparisonTag = thatDicomComparator.Template.ComparisonTags.Find(DicomHl7TagMapTemplate.Hl7ToDicomTag(thisComparisonTag.Tag));
                        if (thatComparisonTag != null)
                        {
                            AttributeComparisonResults attributeComparisonResults
                                = new AttributeComparisonResults(thatComparisonTag.Tag,
                                SegmentNames.Name(thisComparisonTag.Tag.Segment),
                                thisComparisonTag.Tag.FieldIndex,
                                thisComparisonTag.DataFormat.ToHl7Format(),
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

            return compared;
        }

        private TagValueCollection GenerateTagValueCollection(TagValueCollection tagValueFilterCollection)
        {
            TagValueCollection localTagValueCollection = new TagValueCollection();

            // Check if the comparator can be used with a Universal Match on the Value of the Tag
            // - that is zero-length Value.
            if (this.UseComparator(tagValueFilterCollection, true) == true)
            {
                foreach (BaseTagValue baseTagValue in tagValueFilterCollection)
                {
                    Hl7TagValue hl7TagValue = null;

                    if (baseTagValue is Hl7TagValue)
                    {
                        hl7TagValue = (Hl7TagValue)baseTagValue;
                    }
                    else if (baseTagValue is DicomTagValue)
                    {
                        DicomTagValue dicomTagValue = (DicomTagValue)baseTagValue;
                        hl7TagValue = new Hl7TagValue(DicomHl7TagMapTemplate.DicomToHl7Tag(dicomTagValue.Tag), dicomTagValue.Value);
                    }

                    // If the Value is empty (Universal Match) then try to get the actual
                    // value from the comparator so that is actual value will be used against
                    // other comparators.
                    if (hl7TagValue.Tag != null)
                    {
                        if (hl7TagValue.Value == System.String.Empty)
                        {
                            // try to get a value for this Tag from this comparator
                            System.String lValue = getValue(hl7TagValue.Tag);

                            // Add a new Tag Value - with Value coming from this comparator
                            // to the local filter.
                            Hl7TagValue lHl7TagValue = new Hl7TagValue(hl7TagValue.Tag, lValue);
                            localTagValueCollection.Add(lHl7TagValue);
                        }
                        else
                        {
                            // Just add the given Tag Value pair to the local filter
                            localTagValueCollection.Add(hl7TagValue);
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
            foreach (BaseTagValue baseTagValue in tagValueFilterCollection)
            {
                Hl7TagValue hl7TagValue = null;

                if (baseTagValue is Hl7TagValue)
                {
                    hl7TagValue = (Hl7TagValue)baseTagValue;
                }
                else if (baseTagValue is DicomTagValue)
                {
                    DicomTagValue dicomTagValue = (DicomTagValue)baseTagValue;
                    hl7TagValue = new Hl7TagValue(DicomHl7TagMapTemplate.DicomToHl7Tag(dicomTagValue.Tag), dicomTagValue.Value);
                }

                if (hl7TagValue.Tag != null)
                {
                    foreach (Hl7ComparisonTag thisComparisonTag in this.Template.ComparisonTags)
                    {
                        // Tags are the same
                        if (thisComparisonTag.Tag == hl7TagValue.Tag)
                        {
                            if (universalMatchAllowed == true)
                            {
                                // When universal matching either a zero-length or exact match are OK
                                if ((hl7TagValue.Value == System.String.Empty) ||
                                    (thisComparisonTag.DataFormat.ToHl7Format() == hl7TagValue.Value))
                                {
                                    filterCounter++;
                                }
                            }
                            else if ((universalMatchAllowed == false) &&
                                (thisComparisonTag.DataFormat.ToHl7Format() == hl7TagValue.Value))
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

        private System.String getValue(Hl7Tag tag)
        {
            System.String lValue = System.String.Empty;

            // Try to gat a value for the gievn tag from this comparator.
            foreach (Hl7ComparisonTag thisComparisonTag in this.Template.ComparisonTags)
            {
                if (thisComparisonTag.Tag == tag)
                {
                    lValue = thisComparisonTag.DataFormat.ToHl7Format();
                    break;
                }
            }

            return lValue;
        }
    }
}
