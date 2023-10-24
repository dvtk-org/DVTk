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
using Dvtk.Dicom.StructuredReporting.Validation;

namespace Dvtk.Dicom.StructuredReporting
{
    /// <summary>
    /// This class represents a Code in a Structured Report object that refers to a coded concept. 
    /// An instance of this class is associated with a single Sequence Item that contains DICOM 
    /// attributes that are defined in the “Code Sequence Macro”.
    /// </summary>
    public class ConceptCode
    {       
        //
        // - Fields -
        //

        /// <summary>
        /// A description of the context of an instance of this class.
        /// E.g. "DICOM - Structured Reporting - Content Item - Concept Name"
        /// </summary>
        private string context = null;

        /// <summary>
        /// Sequence Item that contains the "Code Sequence Macro" DICOM attributes that encode the
        /// Concept Code.
        /// </summary>
        private SequenceItem sequenceItem = null;

        /// <summary>
        /// See property ValidationResults.
        /// </summary>
        private ValidationResults validationResults = null;
        
        

        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ConceptCode()
        {
            // Do nothing.
        }




        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sequenceItem">
        /// Sequence Item that contains the "Code Sequence Macro" DICOM attributes that encode the
        /// Concept Code.
        /// </param>
        /// <param name="context">A description of the context of an instance of this class.</param>
        private ConceptCode(SequenceItem sequenceItem, string context)
        {
            this.sequenceItem = sequenceItem;
            this.context = context;
            validationResults = new ValidationResults(context);
        }
        
        

        //
        // - Properties -
        //

        /// <summary>
        /// Gets the Code Meaning of this instance.
        /// </summary>
        /// <remarks>
        /// If the associated DICOM attribute is not present, null is returned.
        /// If the associated DICOM attribute is present and has no values, "" is returned.
        /// If the associated DICOM attribute is present and has values, the first value is returned.
        /// </remarks>
        public String CodeMeaning
        {
            get 
            {
                return(Convert.FirstAttributeValueToString(this.sequenceItem, "0x00080104"));
            }
        }

        /// <summary>
        /// Gets the Coding Scheme Designator of this instance.
        /// </summary>
        /// <remarks>
        /// If the associated DICOM attribute is not present, null is returned.
        /// If the associated DICOM attribute is present and has no values, "" is returned.
        /// If the associated DICOM attribute is present and has values, the first value is returned.
        /// </remarks>
        public String CodingSchemeDesignator
        {
            get
            {
                return (Convert.FirstAttributeValueToString(this.sequenceItem, "0x00080102"));
            }
        }

        /// <summary>
        /// Gets the Coding Scheme Version of this instance.
        /// </summary>
        /// <remarks>
        /// If the associated DICOM attribute is not present, null is returned.
        /// If the associated DICOM attribute is present and has no values, "" is returned.
        /// If the associated DICOM attribute is present and has values, the first value is returned.
        /// </remarks>
        public String CodingSchemeVersion
        {
            get
            {
                return (Convert.FirstAttributeValueToString(this.sequenceItem, "0x00080103"));
            }
        }

        /// <summary>
        /// Gets the Code Value of this instance.
        /// </summary>
        /// <remarks>
        /// If the associated DICOM attribute is not present, null is returned.
        /// If the associated DICOM attribute is present and has no values, "" is returned.
        /// If the associated DICOM attribute is present and has values, the first value is returned.
        /// </remarks>
        public String CodeValue
        {
            get 
            {
                return (Convert.FirstAttributeValueToString(this.sequenceItem, "0x00080100")); 
            }
        }

        /// <summary>
        /// Gets the list of validation results for this instance.
        /// </summary>
        public IList<ValidationResult> ValidationResults
        {
            get
            {
                return (this.validationResults);
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Create a ConceptCode instance, if the specified Sequence Attribute is present in the
        /// supplied Attribute Set and the Sequence Attribute contains at least one Sequence Item.
        /// </summary>
        /// <param name="attributeSet">
        /// The Attribute Set in which the Sequence Attribute may be present.
        /// </param>
        /// <param name="tag">The Tag of the Sequence Attribute.</param>
        /// <returns>
        /// The created ConceptCode instance if the first Sequence Item exists.
        /// Null otherwise.
        /// </returns>
        internal static ConceptCode CreateConceptCode(AttributeSet attributeSet, String tag, string context)
        {
            ConceptCode conceptCode = null;

            DvtkHighLevelInterface.Dicom.Other.Attribute attribute = attributeSet[tag];

            if (attribute.Exists)
            {
                if (attribute.VR == VR.SQ)
                {
                    if (attribute.ItemCount > 0)
                    {
                        conceptCode = new ConceptCode(attribute.GetItem(1), context);
                    }
                }
            }

            return (conceptCode);
        }

        /// <summary>
        /// Method used by the library to serialize this instance to xml.
        /// </summary>
        /// <param name="xmlTextWriter">The xml text writer.</param>
        /// <param name="serializationContext">The serialization context.</param>
        internal void ToXml(XmlTextWriter xmlTextWriter, SerializationContext serializationContext)
        {
            //
            // Start of Concept Code.
            //

            xmlTextWriter.WriteStartElement("conceptCode");


            //
            // Code Value.
            //

            String codeValue = CodeValue;

            if (codeValue != null)
            {
                xmlTextWriter.WriteStartElement("codeValue");
                xmlTextWriter.WriteStartElement("value");
                xmlTextWriter.WriteString(codeValue);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
            }


            //
            // Coding Scheme Designator.
            //

            String codingSchemeDesignator = CodingSchemeDesignator;

            if (codingSchemeDesignator != null)
            {
                xmlTextWriter.WriteStartElement("codingSchemeDesignator");
                xmlTextWriter.WriteStartElement("value");
                xmlTextWriter.WriteString(codingSchemeDesignator);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
            }


            //
            // Coding Scheme Version.
            //

            String codingSchemeVersion = CodingSchemeVersion;

            if (codingSchemeVersion != null)
            {
                xmlTextWriter.WriteStartElement("codingSchemeVersion");
                xmlTextWriter.WriteStartElement("value");
                xmlTextWriter.WriteString(codingSchemeVersion);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
            }


            //
            // Code Meaning.
            //

            String codeMeaning = CodeMeaning;

            if (codeMeaning != null)
            {
                xmlTextWriter.WriteStartElement("codeMeaning");
                xmlTextWriter.WriteStartElement("value");
                xmlTextWriter.WriteString(codeMeaning);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
            }


            //
            // Validation results.
            //

            this.validationResults.ToXml(xmlTextWriter, serializationContext);


            //
            // Start of Concept Code.
            //

            xmlTextWriter.WriteEndElement();
        }
    }
}
