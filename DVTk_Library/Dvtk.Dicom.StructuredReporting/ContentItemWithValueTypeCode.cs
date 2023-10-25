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

using DvtkHighLevelInterface.Dicom.Other;

namespace Dvtk.Dicom.StructuredReporting
{
    /// <summary>
    /// This class represents a Content Item with value type CODE that is present in a Structured Report Object.
    /// </summary>
    public class ContentItemWithValueTypeCode: ContentItem
    {
        //
        // - Constants -
        //

        /// <summary>
        /// A description of the context of the conceptCode field of this class.
        /// </summary>
        private const string conceptCodeContext = "DICOM - Structured Reporting - Content Item - Concept Code";



        //
        // - Fields -
        //

        /// <summary>
        /// See property ConceptCode.
        /// </summary>
        private ConceptCode conceptCode = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ContentItemWithValueTypeCode(): 
            base(null, null, 1)
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sequenceItem">
        /// The AttributeSet instance containing the DICOM attributes to construct this instance with.
        /// </param>
        /// <param name="parentContentItem">The parent Content Item.</param>
        /// <param name="position">The ordinal position of the associated Sequence Item in it's contained Content Sequence Item.</param>
        public ContentItemWithValueTypeCode(AttributeSet attributeSet, ContentItem parentContentItem, uint position):
            base(attributeSet, parentContentItem, position)
        {
            this.conceptCode = ConceptCode.CreateConceptCode(attributeSet, "0x0040A168", conceptCodeContext);
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the Concept Code.
        /// </summary>
        public ConceptCode ConceptCode
        {
            get
            {
                return this.conceptCode;
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
        internal override void ValueToXml(XmlTextWriter xmlTextWriter, SerializationContext serializationContext)
        {
            if (this.conceptCode != null)
            {
                xmlTextWriter.WriteStartElement("code");
                this.conceptCode.ToXml(xmlTextWriter, serializationContext);
                xmlTextWriter.WriteEndElement();
            }
        }
    }
}
