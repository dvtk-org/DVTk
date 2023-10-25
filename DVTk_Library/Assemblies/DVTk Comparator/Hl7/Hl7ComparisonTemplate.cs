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
using Dvtk.CommonDataFormat;

namespace Dvtk.Comparator
{
    /// <summary>
    /// Summary description for Hl7ComparisonTemplate.
    /// </summary>
    public class Hl7ComparisonTemplate
    {
        private System.String _messageType = System.String.Empty;
        private System.String _messageSubType = System.String.Empty;
        private Hl7ComparisonTagCollection _comparisonTags = new Hl7ComparisonTagCollection();

        /// <summary>
        /// Class constructor.
        /// </summary>
        public Hl7ComparisonTemplate() { }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="messageType">HL7 Message Type</param>
        /// <param name="messageSubType">HL7 Message Subtype</param>
        /// <returns>bool - true = template initialized, false template not initialized</returns>
        public bool Initialize(System.String messageType, System.String messageSubType)
        {
            bool initialized = true;
            _messageType = messageType;
            _messageSubType = messageSubType;

            // Only certain templates available
            // Use command and sopClassUid to determine if we can set one up
            switch (_messageType)
            {
                case "ADT":
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 3), new CommonIdFormat())); // Patient Internal ID
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 5), new CommonNameFormat())); // Patient Name
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 7), new CommonDateFormat())); // Patient Birth Date/Time
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 8), new CommonStringFormat())); // Patient Sex
                    break;
                case "ORM":
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 3), new CommonIdFormat())); // Patient Internal ID
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 5), new CommonNameFormat())); // Patient Name
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 7), new CommonDateFormat())); // Patient Birth Date/Time
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 8), new CommonStringFormat())); // Patient Sex
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("OBR", 2), new CommonIdFormat())); // Placer Order Number
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("OBR", 3), new CommonIdFormat())); // Filler Order Number
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("OBR", 4), new CommonStringFormat())); // Universal Service ID
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("OBR", 18), new CommonIdFormat())); // Accession Number
                    break;
                case "ORU":
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 3), new CommonIdFormat())); // Patient Internal ID
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 5), new CommonNameFormat())); // Patient Name
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 7), new CommonDateFormat())); // Patient Birth Date/Time
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("PID", 8), new CommonStringFormat())); // Patient Sex
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("OBR", 2), new CommonIdFormat())); // Placer Order Number
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("OBR", 3), new CommonIdFormat())); // Filler Order Number
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("OBR", 4), new CommonStringFormat())); // Universal Service ID
                    _comparisonTags.Add(new Hl7ComparisonTag(new Hl7Tag("OBR", 18), new CommonIdFormat())); // Accession Number
                    break;
                default:
                    initialized = false;
                    break;
            }

            return initialized;
        }

        #region properties
        /// <summary>
        /// MessageType property.
        /// </summary>
        public System.String MessageType
        {
            set
            {
                _messageType = value;
            }
            get
            {
                return _messageType;
            }
        }

        /// <summary>
        /// MessageSubType property.
        /// </summary>
        public System.String MessageSubType
        {
            set
            {
                _messageSubType = value;
            }
            get
            {
                return _messageSubType;
            }
        }

        /// <summary>
        /// ComparisonTags property.
        /// </summary>
        public Hl7ComparisonTagCollection ComparisonTags
        {
            get
            {
                return _comparisonTags;
            }
        }
        #endregion
    }
}
