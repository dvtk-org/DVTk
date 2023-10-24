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

using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Dicom.Other;

namespace Dvtk.Dicom.StructuredReporting
{
    /// <summary>
    /// This class represents a Content Item with value type NUM that is present in a Structured
    /// Report Object.
    /// </summary>
    public class ContentItemWithValueTypeNum: ContentItem
    {
        //
        // - Constants -
        //

        /// <summary>
        /// A description of the context of the numericValueQualifier field of this class.
        /// </summary>
        private const string numericValueQualifierContext = "DICOM - Structured Reporting - Content Item - Numeric Value Qualifier";



        //
        // - Fields -
        //

        /// <summary>
        /// See property MeasuredValue.
        /// </summary>
        MeasuredValue measuredValue = null;

        /// <summary>
        /// See property NumericValueQualifier.
        /// </summary>
        ConceptCode numericValueQualifier = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ContentItemWithValueTypeNum():
            base(null, null, 1)
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sequenceItem">
        /// The SequenceItem instance containing the DICOM attributes to construct this instance with.
        /// </param>
        /// <param name="parentContentItem">The parent Content Item.</param>
        /// <param name="position">The ordinal position of the associated Sequence Item in it's contained Content Sequence Item.</param>
        public ContentItemWithValueTypeNum(AttributeSet attributeSet, ContentItem parentContentItem, uint position):
            base(attributeSet, parentContentItem, position)
        {
            DvtkHighLevelInterface.Dicom.Other.Attribute attribute = attributeSet["0x0040A300"];

            if (attribute.Exists)
            {
                if (attribute.VR == VR.SQ)
                {
                    if (attribute.ItemCount > 0)
                    {
                        this.measuredValue  = new MeasuredValue(attribute.GetItem(1));
                    }
                }
            }

            this.numericValueQualifier = ConceptCode.CreateConceptCode(attributeSet, "0x0040A301", numericValueQualifierContext);
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the Measured Value.
        /// </summary>
        public MeasuredValue MeasuredValue
        {
            get
            {
                return this.measuredValue;
            }
        }

        /// <summary>
        /// Gets the Numeric Value Qualifier.
        /// </summary>
        public ConceptCode NumericValueQualifier
        {
            get
            {
                return this.numericValueQualifier;
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
            xmlTextWriter.WriteStartElement("num");

            if (this.measuredValue != null)
            {
                xmlTextWriter.WriteStartElement("measuredValue");
                this.measuredValue.ToXml(xmlTextWriter, serializationContext);
                xmlTextWriter.WriteEndElement();
            }

            if (this.numericValueQualifier != null)
            {
                xmlTextWriter.WriteStartElement("numericValueQualifier");
                this.numericValueQualifier.ToXml(xmlTextWriter, serializationContext);
                xmlTextWriter.WriteEndElement();
            }

            xmlTextWriter.WriteEndElement();
        }
    }
}
