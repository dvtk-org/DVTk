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
using System.Collections;

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using Dvtk.Comparator;

namespace Dvtk.Comparator.Convertors
{
    /// <summary>
    /// Summary description for Hl7MessageToDicomMessageConvertor.
    /// </summary>
    public class Hl7MessageToDicomMessageConvertor
    {
        static Hl7MessageToDicomMessageConvertor() { }

        public static DvtkData.Dimse.DataSet Convert(Hl7Message message)
        {
            DvtkData.Dimse.DataSet dataset = new DvtkData.Dimse.DataSet("Transient");

            try
            {
                if (message != null)
                {
                    // iterate over all the segments in the HL7 message
                    ICollection segments = message.Segments.Values;
                    foreach (Hl7Segment hl7Segment in segments)
                    {
                        // iterate over all the fields in the HL7 segments
                        for (int i = 1; i < hl7Segment.Count; i++)
                        {
                            System.String hl7Value = hl7Segment[i];
                            if (hl7Value != System.String.Empty)
                            {
                                // check if there is an Hl7 Tag corresponding to the value in the DicomHl7Template
                                Hl7Tag hl7Tag = new Hl7Tag(hl7Segment[0], i);
                                DicomHl7TagMap dicomHl7TagMap = DicomHl7TagMapTemplate.FindTagMap(hl7Tag);
                                if (dicomHl7TagMap != null)
                                {
                                    System.String dicomValue = hl7Value;
                                    if (dicomHl7TagMap.ValueConvertor != null)
                                    {
                                        dicomValue = dicomHl7TagMap.ValueConvertor.FromHl7ToDicom(hl7Value, dicomHl7TagMap.Hl7ComponentIndex);
                                    }
                                    AddDicomAttribute(dataset, dicomHl7TagMap.DicomTagPath, dicomValue);
                                }

                                for (int componentIndex = 2; componentIndex < 7; componentIndex++)
                                {
                                    dicomHl7TagMap = DicomHl7TagMapTemplate.FindTagMap(hl7Tag, componentIndex);
                                    if (dicomHl7TagMap != null)
                                    {
                                        System.String dicomValue = hl7Value;
                                        if (dicomHl7TagMap.ValueConvertor != null)
                                        {
                                            dicomValue = dicomHl7TagMap.ValueConvertor.FromHl7ToDicom(hl7Value, dicomHl7TagMap.Hl7ComponentIndex);
                                        }
                                        AddDicomAttribute(dataset, dicomHl7TagMap.DicomTagPath, dicomValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("HL7 to DICOM conversion exception: {0} - {1}", e.Message, e.StackTrace);
            }

            return dataset;
        }

        private static void AddDicomAttribute(DvtkData.Dimse.AttributeSet dataset, DicomTagPath dicomTagPath, System.String dicomValue)
        {
            if (dicomTagPath.Next != null)
            {
                // Try to get the sequence identified by this Tag
                DvtkData.Dimse.Attribute sequenceAttribute = dataset.GetAttribute(dicomTagPath.Tag);
                if (sequenceAttribute == null)
                {
                    // Need to add the sequence
                    DvtkData.Dimse.SequenceItem item = new DvtkData.Dimse.SequenceItem();
                    dataset.AddAttribute(dicomTagPath.Tag.GroupNumber,
                        dicomTagPath.Tag.ElementNumber,
                        DvtkData.Dimse.VR.SQ, item);

                    // Get the newly added sequence
                    sequenceAttribute = dataset.GetAttribute(dicomTagPath.Tag);
                }

                // Get the contained item
                DvtkData.Dimse.SequenceOfItems sequenceOfItems = (DvtkData.Dimse.SequenceOfItems)sequenceAttribute.DicomValue;
                if (sequenceOfItems.Sequence.Count == 1)
                {
                    DvtkData.Dimse.SequenceItem item = sequenceOfItems.Sequence[0];

                    // Call recursively
                    AddDicomAttribute(item, dicomTagPath.Next, dicomValue);
                }
            }
            else
            {
                // Try to get the attribute identified by this Tag
                DvtkData.Dimse.Attribute attribute = dataset.GetAttribute(dicomTagPath.Tag);
                if (attribute != null)
                {
                    // If present - remove the attribute - we want to update the value
                    dataset.Remove(attribute);
                }

                // Add the new value
                dataset.AddAttribute(dicomTagPath.Tag.GroupNumber, dicomTagPath.Tag.ElementNumber, DicomTagVrLookup.GetVR(dicomTagPath.Tag), dicomValue);
            }
        }
    }
}
