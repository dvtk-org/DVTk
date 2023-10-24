// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
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
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Dvtk.Dicom.StructuredReporting.Validation
{
    /// <summary>
    /// Contains a single validation result.
    /// </summary>
    public class ValidationResult
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property Message.
        /// </summary>
        private string message = null;

        /// <summary>
        /// See property MessageType.
        /// </summary>
        private string messageType = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ValidationResult()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messageType">
        /// String describing the validation result that does not contain any actual data. This
        /// string will be used to group ValidationResult instances describing the same problem.
        /// </param>
        /// <param name="message">String describing the validation result.</param>
        public ValidationResult(string messageType, string message)
        {
            this.messageType = messageType;
            this.message = message;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">
        /// String describing the validation result that does not contain any actual data. The same
        /// string will also be used for the message type.
        /// </param>
        public ValidationResult(string message)
        {
            this.messageType = message;
            this.message = message;
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the full description of this instance.
        /// </summary>
        public string Message
        {
            get
            {
                return (this.message);
            }
        }

        /// <summary>
        /// Gets the type of message of this instance.
        /// </summary>
        public string MessageType
        {
            get
            {
                return (this.messageType);
            }
        }
	


        //
        // - Methods -
        //

        /// <summary>
        /// Method used by the library to serialize this instance to xml.
        /// </summary>
        /// <param name="xmlTextWriter">The xml text writer.</param>
        /// <param name="serializationContext">The serialization context.</param>
        internal void ToXml(XmlTextWriter xmlTextWriter, string context, SerializationContext serializationContext)
        {
            //
            // Start element.
            //

            xmlTextWriter.WriteStartElement("validationResult");


            //
            // Context.
            //

            xmlTextWriter.WriteStartElement("context");
            xmlTextWriter.WriteString(context);
            xmlTextWriter.WriteEndElement();


            //
            // Message type.
            //

            xmlTextWriter.WriteStartElement("messageType");
            xmlTextWriter.WriteString(this.messageType);
            xmlTextWriter.WriteEndElement();


            //
            // Context and message type index.
            //

            string contextAndMessageType = context + " - " + messageType;
            int contextAndMessageTypeCount = 0;

            if (serializationContext.ValidationResultContextAndMessageTypeCount.TryGetValue(contextAndMessageType, out contextAndMessageTypeCount))
            {
                contextAndMessageTypeCount++;
            }
            else
            {
                contextAndMessageTypeCount = 1;
            }
            serializationContext.ValidationResultContextAndMessageTypeCount[contextAndMessageType] = contextAndMessageTypeCount;

            xmlTextWriter.WriteStartElement("contextAndMessageTypeIndex");
            xmlTextWriter.WriteString(contextAndMessageTypeCount.ToString());
            xmlTextWriter.WriteEndElement();


            //
            // Message.
            //

            xmlTextWriter.WriteStartElement("message");
            xmlTextWriter.WriteString(this.message);
            xmlTextWriter.WriteEndElement();


            //
            // Message index.
            //

            xmlTextWriter.WriteStartElement("index");
            serializationContext.ValidationResultCount = serializationContext.ValidationResultCount + 1;
            xmlTextWriter.WriteString(serializationContext.ValidationResultCount.ToString());
            xmlTextWriter.WriteEndElement();


            //
            // End element.
            //

            xmlTextWriter.WriteEndElement();
        }
    }
}
