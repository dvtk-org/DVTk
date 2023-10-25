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

namespace Dvtk.Dicom.StructuredReporting
{
    /// <summary>
    /// This class represents a Structured Report Object as is defined in the DICOM standard.
    /// </summary>
    public class StructuredReport
    {
        //
        // - Constants -
        //

        /// <summary>
        /// A description of the context of an instance of this class.
        /// </summary>
        private const string context = "DICOM - Structured Reporting - Structured Report";

        
        
        //
        // - Fields -
        //


        /// <summary>
        /// See property DataSet.
        /// </summary>
        private DvtkHighLevelInterface.Dicom.Other.DataSet dataSet;

        /// <summary>
        /// See property RootContentItem.
        /// </summary>
        private ContentItem rootContentItem;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private StructuredReport()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataSet">The Structured Report encoded as a Data Set.</param>
        public StructuredReport(DvtkHighLevelInterface.Dicom.Other.DataSet dataSet)
        {
            this.dataSet = dataSet;

            this.rootContentItem = ContentItem.CreateContentItems(dataSet);
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the DataSet instance from which the ContentItems of this instance have been
        /// constructed.
        /// </summary>
        public DvtkHighLevelInterface.Dicom.Other.DataSet DataSet
        {
            get 
            { 
                return dataSet; 
            }
        }

        /// <summary>
        /// Gets the root Content Item that was constructed from the associated DataSet instance.
        /// </summary>
        public ContentItem RootContentItem
        {
            get 
            { 
                return rootContentItem; 
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Serialize this instance (including validation results) to xml.
        /// </summary>
        public void ToXml(string fullFileName)
        {
            XmlTextWriter xmlTextWriter = new XmlTextWriter(fullFileName, null);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.Indentation = 4;

            xmlTextWriter.WriteStartElement("structuredReports", "http://www.dvtk.org/schemas/SRValidationResult.xsd");
            xmlTextWriter.WriteStartElement("structuredReport");

            SerializationContext serializationContext = new SerializationContext();
            this.rootContentItem.ToXml(xmlTextWriter, serializationContext);
            serializationContext.ToXml(xmlTextWriter);

            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.Close();
        }
    }
}
