// Copyright © 2010 DVTk
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
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Dvtk.Dicom.StructuredReporting
{
    /// <summary>
    /// This class contains the context needed when serializing instances to xml.
    /// </summary>
    public class SerializationContext
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property ValidationResultMessageIndex.
        /// </summary>
        private int validationResultCount = 0;

        /// <summary>
        /// See property ValidationResultContextAndMessageTypeCount.
        /// </summary>
        private Dictionary<string, int> validationResultContextAndMessageTypeCount = new Dictionary<string, int>();



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the number of ValidationResults encountered so far.
        /// </summary>
        /// <remarks>
        /// It is the responsibility of the user of this class to increase this index after
        /// serializing a ValidationResult instance.
        /// </remarks>
        public int ValidationResultCount
        {
            get
            {
                return (this.validationResultCount);
            }
            set
            {
                this.validationResultCount = value;
            }
        }

        /// <summary>
        /// Gets the dictionary, in which for each string combination of context and message type,
        /// the number encountered so far is stored.
        /// </summary>
        public Dictionary<string, int> ValidationResultContextAndMessageTypeCount
        {
            get
            {
                return (this.validationResultContextAndMessageTypeCount);
            }
        }

        /// <summary>
        /// Method used by the library to serialize this instance to xml.
        /// </summary>
        /// <param name="xmlTextWriter">The xml text writer.</param>
        internal void ToXml(XmlTextWriter xmlTextWriter)
        {
            xmlTextWriter.WriteStartElement("summary");

            xmlTextWriter.WriteStartElement("validationResultCount");
            xmlTextWriter.WriteString(this.validationResultCount.ToString());
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("contextAndMessageTypes");
            foreach (string contextAndMessageType in this.validationResultContextAndMessageTypeCount.Keys)
            {
                xmlTextWriter.WriteStartElement("contextAndMessageType");
                xmlTextWriter.WriteStartElement("string");
                xmlTextWriter.WriteString(contextAndMessageType);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteStartElement("count");
                xmlTextWriter.WriteString(this.validationResultContextAndMessageTypeCount[contextAndMessageType].ToString());
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
            }

            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteEndElement();
        }
    }
}
