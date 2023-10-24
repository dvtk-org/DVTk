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
    /// List of ValidationResult instances.
    /// </summary>
    public class ValidationResults: List<ValidationResult>
    {
        //
        // - Fields -
        //

        /// <summary>
        /// A description of the context of the instance that contains this list of validations 
        /// results.
        /// </summary>
        private string context = null;



        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ValidationResults()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="validationContext">
        /// The shared validation context of all containted items of this instance.
        /// </param>
        public ValidationResults(string context)
        {
            this.context = context;
        }



        /// <summary>
        /// Method used by the library to serialize this instance to xml.
        /// </summary>
        /// <param name="xmlTextWriter">The xml text writer.</param>
        /// <param name="serializationContext">The serialization context.</param>
        internal void ToXml(XmlTextWriter xmlTextWriter, SerializationContext serializationContext)
        {
            if (Count > 0)
            {
                xmlTextWriter.WriteStartElement("validationResults");

                xmlTextWriter.WriteStartElement("context");
                xmlTextWriter.WriteString(context);
                xmlTextWriter.WriteEndElement();

                foreach (ValidationResult validationResult in this)
                {
                    validationResult.ToXml(xmlTextWriter, this.context, serializationContext);
                }

                xmlTextWriter.WriteEndElement();
            }
        }
    }
}
