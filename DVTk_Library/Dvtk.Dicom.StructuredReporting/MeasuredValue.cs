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
using Dvtk.Dicom.StructuredReporting.Validation;

namespace Dvtk.Dicom.StructuredReporting
{
    /// <summary>
    /// This class represents the Measured Value of a Content Item with value type NUM that is
    /// present in a Structured Report Object.
    /// </summary>
    public class MeasuredValue
    {
        //
        // - Constants -
        //

        /// <summary>
        /// A description of the context of an instance of this class.
        /// </summary>
        private const string context = "DICOM - Structured Reporting - Content Item - Measured Value";

        /// <summary>
        /// A description of the context of the measurementUnits field of this class.
        /// </summary>
        private const string measurementUnitsContext = "DICOM - Structured Reporting - Content Item - Measured Value - MeasurementUnits";



        //
        // - Fields -
        //

        /// <summary>
        /// See property MeasurementUnits.
        /// </summary>
        ConceptCode measurementUnits = null;

        /// <summary>
        /// The single sequence item in the Measured Value Sequence that encodes this Measured
        /// Value.
        /// </summary>
        private SequenceItem sequenceItem = null;

        private ValidationResults validationResults = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private MeasuredValue()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sequenceItem">The single sequence item that encodes this Measured Value.</param>
        public MeasuredValue(SequenceItem sequenceItem)
        {
            this.sequenceItem = sequenceItem;

            this.measurementUnits = ConceptCode.CreateConceptCode(sequenceItem, "0x004008EA", measurementUnitsContext);

            this.validationResults = new ValidationResults(context);
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the Measurement Units for this instance.
        /// </summary>
        public ConceptCode MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
        }

        /// <summary>
        /// Gets the Numeric Value for this instance.
        /// </summary>
        public String NumericValue
        {
            get
            {
                return (Convert.FirstAttributeValueToString(this.sequenceItem, "0x0040A30A"));
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
        internal void ToXml(XmlTextWriter xmlTextWriter, SerializationContext serializationContext)
        {
            xmlTextWriter.WriteStartElement("numericValue");
            xmlTextWriter.WriteStartElement("value");
            xmlTextWriter.WriteString(NumericValue);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndElement();

            if (this.measurementUnits != null)
            {
                xmlTextWriter.WriteStartElement("measurementUnits");
                this.measurementUnits.ToXml(xmlTextWriter, serializationContext);
                xmlTextWriter.WriteEndElement();
            }
        }
    }
}
