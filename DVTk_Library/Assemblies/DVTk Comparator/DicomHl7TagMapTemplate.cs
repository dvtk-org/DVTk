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
using Dvtk.Hl7;
using Dvtk.Comparator.Bases;
using Dvtk.Comparator.Convertors;

namespace Dvtk.Comparator
{
    /// <summary>
    /// Summary description for DicomHl7TagMapTemplate.
    /// </summary>
    public class DicomHl7TagMapTemplate
    {
        private static DicomHl7TagMapCollection _dicomHl7TagMapCollection = new DicomHl7TagMapCollection();

        /// <summary>
        /// Class constructor
        /// </summary>
        static DicomHl7TagMapTemplate()
        {
            // HL7 PID Segment
            AddEntry(Tag.PATIENT_ID, "PID", 3, 1, "Patient Identifier List", new IdConvertor());
            AddEntry(Tag.PATIENTS_NAME, "PID", 5, "Patient Name", new NameConvertor());
            AddEntry(Tag.PATIENTS_BIRTH_DATE, "PID", 7, "Date/time of Patient Birth", new DateConvertor());
            AddEntry(Tag.PATIENTS_BIRTH_TIME, "PID", 7, "Date/time of Patient Birth", new TimeConvertor());
            AddEntry(Tag.PATIENTS_SEX, "PID", 8, "Administrative Sex");

            // HL7 PV1 Segment
            AddEntry(Tag.CURRENT_PATIENT_LOCATION, "PV1", 3, "Assigned Patient Location");
            AddEntry(Tag.REFERRING_PHYSICIANS_NAME, "PV1", 8, "Referring Doctor", new NameConvertor());

            // HL7 ORC Segment

            // HL7 OBR Segment
            AddEntry(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, "OBR", 4, 5, "Universal Service ID", new StringConvertor());
            AddEntry(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.SCHEDULED_ACTION_ITEM_CODE_SEQUENCE, Tag.CODE_VALUE, "OBR", 4, 4, "Universal Service ID", new StringConvertor());
            AddEntry(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.SCHEDULED_ACTION_ITEM_CODE_SEQUENCE, Tag.CODE_MEANING, "OBR", 4, 5, "Universal Service ID", new StringConvertor());
            AddEntry(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.SCHEDULED_ACTION_ITEM_CODE_SEQUENCE, Tag.CODING_SCHEME_DESIGNATOR, "OBR", 4, 6, "Universal Service ID", new StringConvertor());

            AddEntry(Tag.ACCESSION_NUMBER, "OBR", 18, "Placer Field 1", new IdConvertor());
            AddEntry(Tag.REQUESTED_PROCEDURE_ID, "OBR", 19, "Placer Field 2", new IdConvertor());
            AddEntry(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_ID, "OBR", 20, "Filler Field 1", new IdConvertor());
            AddEntry(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.MODALITY, "OBR", 24, "Diagnostic Service Sect ID");

            AddEntry(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_START_DATE, "OBR", 27, 4, "Quantity/Timing", new DateConvertor());
            AddEntry(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, Tag.SCHEDULED_PROCEDURE_STEP_START_TIME, "OBR", 27, 4, "Quantity/Timing", new TimeConvertor());

            AddEntry(Tag.REQUESTED_PROCEDURE_CODE_SEQUENCE, Tag.CODE_VALUE, "OBR", 44, 1, "Procedure Code", new StringConvertor());
            AddEntry(Tag.REQUESTED_PROCEDURE_CODE_SEQUENCE, Tag.CODE_MEANING, "OBR", 44, 2, "Procedure Code", new StringConvertor());
            AddEntry(Tag.REQUESTED_PROCEDURE_CODE_SEQUENCE, Tag.CODING_SCHEME_DESIGNATOR, "OBR", 44, 3, "Procedure Code", new StringConvertor());
            AddEntry(Tag.REQUESTED_PROCEDURE_DESCRIPTION, "OBR", 44, 5, "Procedure Code", new StringConvertor());

            // HL7 OBX Segment

            // HL7 ZDS Segment
            AddEntry(Tag.STUDY_INSTANCE_UID, "ZDS", 1, 1, "Reference Pointer", new UidConvertor());

            // HL7 MRG Segment
            AddEntry(Tag.OTHER_PATIENT_IDS, "MRG", 1, "Merge Patient Identifier List", new IdConvertor());
        }

        #region AddEntry methods
        private static void AddEntry(DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, System.String name)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), name)));
        }
        private static void AddEntry(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, System.String name)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(parent1Tag, tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), name)));
        }
        private static void AddEntry(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag parent2Tag, DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, System.String name)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(parent1Tag, parent2Tag, tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), name)));
        }

        private static void AddEntry(DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, System.String name, BaseValueConvertor valueConvertor)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), name), valueConvertor));
        }
        private static void AddEntry(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, System.String name, BaseValueConvertor valueConvertor)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(parent1Tag, tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), name), valueConvertor));
        }
        private static void AddEntry(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag parent2Tag, DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, System.String name, BaseValueConvertor valueConvertor)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(parent1Tag, parent2Tag, tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), name), valueConvertor));
        }

        private static void AddEntry(DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, int componentIndex, System.String name)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), componentIndex, name)));
        }
        private static void AddEntry(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, int componentIndex, System.String name)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(parent1Tag, tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), componentIndex, name)));
        }
        private static void AddEntry(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag parent2Tag, DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, int componentIndex, System.String name)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(parent1Tag, parent2Tag, tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), componentIndex, name)));
        }

        private static void AddEntry(DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, int componentIndex, System.String name, BaseValueConvertor valueConvertor)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), componentIndex, name), valueConvertor));
        }
        private static void AddEntry(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, int componentIndex, System.String name, BaseValueConvertor valueConvertor)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(parent1Tag, tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), componentIndex, name), valueConvertor));
        }
        private static void AddEntry(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag parent2Tag, DvtkData.Dimse.Tag tag, System.String segment, int fieldIndex, int componentIndex, System.String name, BaseValueConvertor valueConvertor)
        {
            _dicomHl7TagMapCollection.Add(new DicomHl7TagMap(new DicomTagPath(parent1Tag, parent2Tag, tag),
                new Hl7TagPath(new Hl7Tag(segment, fieldIndex), componentIndex, name), valueConvertor));
        }
        #endregion AddEntry methods

        public static Tag Hl7ToDicomTag(Hl7Tag hl7Tag)
        {
            Tag dicomTag = Tag.UNDEFINED;

            foreach (DicomHl7TagMap dicomHl7TagMap in _dicomHl7TagMapCollection)
            {
                if (dicomHl7TagMap.Hl7Tag == hl7Tag)
                {
                    dicomTag = dicomHl7TagMap.DicomTag;
                    break;
                }
            }

            return dicomTag;
        }

        public static Hl7Tag DicomToHl7Tag(Tag dicomTag)
        {
            Hl7Tag hl7Tag = new Hl7Tag(Hl7SegmentEnum.Unknown, -1);

            foreach (DicomHl7TagMap dicomHl7TagMap in _dicomHl7TagMapCollection)
            {
                if (dicomHl7TagMap.DicomTag == dicomTag)
                {
                    hl7Tag = dicomHl7TagMap.Hl7Tag;
                    break;
                }
            }

            return hl7Tag;
        }

        public static System.String Hl7NameFromHl7Tag(Hl7Tag hl7Tag)
        {
            System.String hl7Name = System.String.Empty;

            foreach (DicomHl7TagMap dicomHl7TagMap in _dicomHl7TagMapCollection)
            {
                if (dicomHl7TagMap.Hl7Tag == hl7Tag)
                {
                    hl7Name = dicomHl7TagMap.Hl7Name;
                    break;
                }
            }

            return hl7Name;
        }

        /// <summary>
        /// Try to find a DicomHl7TagMap in the collection using the HL7 Tag as index.
        /// </summary>
        /// <param name="hl7Tag">HL7 Tag used as index.</param>
        /// <returns>DicomHl7TagMap - null if no match found</returns>
        public static DicomHl7TagMap FindTagMap(Hl7Tag hl7Tag)
        {
            DicomHl7TagMap dicomHl7TagMap = null;

            foreach (DicomHl7TagMap lDicomHl7TagMap in _dicomHl7TagMapCollection)
            {
                if (lDicomHl7TagMap.Hl7Tag == hl7Tag)
                {
                    dicomHl7TagMap = lDicomHl7TagMap;
                    break;
                }
            }

            return dicomHl7TagMap;
        }

        /// <summary>
        /// Try to find a DicomHl7TagMap in the collection using the HL7 Tag as index.
        /// </summary>
        /// <param name="hl7Tag">HL7 Tag used as index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <returns>DicomHl7TagMap - null if no match found</returns>
        public static DicomHl7TagMap FindTagMap(Hl7Tag hl7Tag, int componentIndex)
        {
            DicomHl7TagMap dicomHl7TagMap = null;

            foreach (DicomHl7TagMap lDicomHl7TagMap in _dicomHl7TagMapCollection)
            {
                if ((lDicomHl7TagMap.Hl7Tag == hl7Tag) &&
                    (lDicomHl7TagMap.Hl7ComponentIndex == componentIndex))
                {
                    dicomHl7TagMap = lDicomHl7TagMap;
                    break;
                }
            }

            return dicomHl7TagMap;
        }

    }
}
