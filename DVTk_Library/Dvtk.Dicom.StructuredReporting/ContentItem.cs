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
    /// This class represents the non-value-type-specific part of a Content Item that is present
    /// in a Structured Report Object.
    /// </summary>
    public class ContentItem
    {
        //
        // - Constants -
        //

        /// <summary>
        /// A description of the context of an instance of this class.
        /// </summary>
        private const string context = "DICOM - Structured Reporting - Content Item";

        /// <summary>
        /// A description of the context of the conceptName field of this class.
        /// </summary>
        private const string conceptNameContext = "DICOM - Structured Reporting - Content Item - Concept Name";



        //
        // - Fields -
        //

        /// <summary>
        /// The Data Set or Sequence Item containing the DICOM attributes that encode the Content
        /// Item.
        /// </summary>
        private AttributeSet attributeSet = null;

        /// <summary>
        /// See property ConceptName.
        /// </summary>
        private ConceptCode conceptName = null;

        /// <summary>
        /// If this is a root Content Item, this field contains 1. In other cases, it contains the
        /// ordinal position of the associated Sequence Item in it's contained Content Sequence
        /// Attribute.
        /// </summary>
        private uint position = 0;

        /// <summary>
        /// If this instance is the Target of a another Content Item, this field contains a
        /// reference to this other Content Item. If this is not the case (which implies that this
        /// instance represents a Root Content Item), this field contains null.
        /// </summary>
        private ContentItem parentContentItem = null;

        /// <summary>
        /// See property ChildContentItems.
        /// </summary>
        private List<ContentItem> childContentItems = null;

        /// <summary>
        /// See property ValidationResults.
        /// </summary>
        private ValidationResults validationResults = new ValidationResults(context);

        /// <summary>
        /// Contains the list of unhandled exceptions from visitors that have visited this instance.
        /// </summary>
        private List<Exception> visitorExceptions = new List<Exception>();



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ContentItem()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sequenceItem">
        /// The attributeSet instance containing the DICOM attributes to construct this instance with.
        /// </param>
        /// <param name="parentContentItem">The parent Content Item.</param>
        /// <param name="position">
        /// The ordinal position of the associated Sequence Item in it's contained Content Sequence
        /// Item.
        /// </param>
        internal ContentItem(AttributeSet attributeSet, ContentItem parentContentItem, uint position)
        {
            this.attributeSet = attributeSet;
            this.conceptName = ConceptCode.CreateConceptCode(attributeSet, "0x0040A043", conceptNameContext);
            this.position = position;
            this.parentContentItem = parentContentItem;
            childContentItems = new List<ContentItem>();
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the child Content Items of this instance, i.e. all Content Item instances that are
        /// the Target of this this instance.
        /// </summary>
        /// <remarks>
        /// If no child Content Items exist, an empty IList is returned.
        /// </remarks>
        public IList<ContentItem> ChildContentItems
        {
            get
            {
                return this.childContentItems;
            }
        }

        /// <summary>
        /// Gets the Concept Name of this instance.
        /// </summary>
        /// <remarks>
        /// If no Concepts Name is available, null is returned.
        /// </remarks>
        public ConceptCode ConceptName
        {
            get 
            { 
                return this.conceptName; 
            }
        }

        /// <summary>
        /// Gets the (implicit) Identifier of this instance.
        /// </summary>
        /// <remarks>
        /// The returned list is created by this instance, so it may be changed by the calling code.
        /// </remarks>
        public IList<uint> Identifier
        {
            get 
            {
                IList<uint> identifier = null;

                if (this.parentContentItem == null)
                    // This is a Root Content Item.
                {
                    identifier = new List<uint>();
                    identifier.Add(1);
                }
                else
                    // This is a non-root Content Item.
                {
                    identifier = this.parentContentItem.Identifier;
                    identifier.Add(this.position);
                }

                return identifier; 
            }
        }

        /// <summary>
        /// Gets the (implicit) Identifier of this instance as a string.
        /// </summary>
        private string IdentifierAsString
        {
            get
            {
                string identifierAsString = "";

                IList<uint> identifier = Identifier;

                foreach (uint position in identifier)
                {
                    if (identifierAsString == "")
                    {
                        identifierAsString = position.ToString();
                    }
                    else
                    {
                        identifierAsString += "." + position.ToString();
                    }
                }

                return (identifierAsString);
            }
        }

        /// <summary>
        /// Gets the Relationship Type of this instance.
        /// </summary>
        /// <remarks>
        /// If the associated DICOM attribute is not present, null is returned.
        /// If the associated DICOM attribute is present and has no values, "" is returned.
        /// If the associated DICOM attribute is present and has values, the first value is returned.
        /// </remarks>
        public String RelationshipType
        {
            get
            {
                return (Convert.FirstAttributeValueToString(this.attributeSet, "0x0040A010"));
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

        /// <summary>
        /// Gets the Value Type of this instance.
        /// </summary>
        /// <remarks>
        /// If the associated DICOM attribute is not present, null is returned.
        /// If the associated DICOM attribute is present and has no values, "" is returned.
        /// If the associated DICOM attribute is present and has values, the first value is returned.
        /// </remarks>
        public String ValueType
        {
            get
            {
                return (Convert.FirstAttributeValueToString(this.attributeSet, "0x0040A040"));
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Accepts a visitor to visit this instance and all child Content Item instances.
        /// </summary>
        /// <remarks>
        /// When an exception is thrown while this instance is visited, the exception will be
        /// caught and stored in the visitorExceptions field. All the exceptions from visitors
        /// will also be stored while serializing to xml.
        /// </remarks>
        /// <param name="contentItemVisitor">The visitor.</param>
        public void Accept(IContentItemVisitor contentItemVisitor)
        {
            try
            {
                contentItemVisitor.Visit(this);
            }
            catch (Exception exception)
            {
                Exception visitorException = new Exception(contentItemVisitor.GetType().FullName + " instance has thrown an exception while visiting this Content Item.", exception);
                this.visitorExceptions.Add(visitorException);
            }

            foreach (ContentItem childContentItem in ChildContentItems)
            {
                childContentItem.Accept(contentItemVisitor);
            }
        }

        /// <summary>
        /// Create a single Content Item instance without creating (possible) child Content Item instances.
        /// </summary>
        /// <remarks>
        /// The child Content Items will be created by the CreateContentItems methods.
        /// </remarks>
        /// <param name="attributeSet">The Attribute Set from which the Content Item is constructed.</param>
        /// <returns>A single Content Item instance.</returns>
        private static ContentItem CreateContentItem(AttributeSet attributeSet, ContentItem parentContentItem, uint position)
        {
            ContentItem contentItem = null;

            String valueType = Convert.FirstAttributeValueToString(attributeSet, "0x0040A040");

            valueType = valueType.TrimStart(' ');
            valueType = valueType.TrimEnd(' ');

            switch (valueType)
            {
                case "CODE":
                    contentItem = new ContentItemWithValueTypeCode(attributeSet, parentContentItem, position);
                    break;

                case "NUM":
                    contentItem = new ContentItemWithValueTypeNum(attributeSet, parentContentItem, position);
                    break;

                default:
                    contentItem = new ContentItem(attributeSet, parentContentItem, position);
                    break;
            }

            return (contentItem);
        }

        /// <summary>
        /// Create both a Content Item instance and all its direct and indirect child Content Item instances.
        /// </summary>
        /// <param name="attributeSet">
        /// <param name="dataSet">
        /// The AttributeSet instance containing the DICOM attributes to construct this Content 
        /// Item instance and its direct and indirect child Content Item instances with.
        /// </param>
        /// </param>
        /// <param name="parentContentItem">
        /// The parent Content Item instance.
        /// If no parent exists, supply null.
        /// </param>
        /// <param name="position">
        /// The ordinal position of the associated Sequence Item in it's contained Content Sequence.
        /// Item.
        /// </param>
        /// <returns>A Content Item instance.</returns>
        private static ContentItem CreateContentItems(AttributeSet attributeSet, ContentItem parentContentItem, uint position)
        {
            //
            // Create the single Content Item instance using the Attribute Set instance supplied.
            //

            ContentItem contentItem = CreateContentItem(attributeSet, parentContentItem, position);


            //
            // If existing, create its child Content Item instances.
            //

            DvtkHighLevelInterface.Dicom.Other.Attribute attribute = attributeSet["0x0040A730"]; // Content Sequence Attribute.

            if (attribute.Exists)
            {
                if (attribute.VR == VR.SQ)
                {
                    for (uint sequenceItemIndex = 1; sequenceItemIndex <= attribute.ItemCount; sequenceItemIndex++)
                    {
                        SequenceItem sequenceItem = attribute.GetItem(System.Convert.ToInt32(sequenceItemIndex));

                        ContentItem childContentItem = CreateContentItems(sequenceItem, contentItem, sequenceItemIndex);
                        contentItem.childContentItems.Add(childContentItem);
                    }
                }
            }

            return (contentItem);
        }

        /// <summary>
        /// Create both the root Content Item instance and all its direct and indirect child Content Item instances.
        /// </summary>
        /// <param name="dataSet">
        /// The DataSet instance containing the DICOM attributes to construct this root Content 
        /// Item instance and its direct and indirect child Content Item instances with.
        /// </param>
        /// <returns>The root Content Item instance.</returns>
        public static ContentItem CreateContentItems(DataSet dataSet)
        {
            return(CreateContentItems(dataSet, null, 1));
        }

        /// <summary>
        /// Method used by the library to serialize this instance to xml.
        /// </summary>
        /// <param name="xmlTextWriter">The xml text writer.</param>
        /// <param name="serializationContext">The serialization context.</param>
        internal void ToXml(XmlTextWriter xmlTextWriter, SerializationContext serializationContext)
        {
            //
            // Start of Content Item.
            //

            xmlTextWriter.WriteStartElement("contentItem");


            //
            // Identifier.
            //

            xmlTextWriter.WriteAttributeString("identifier", IdentifierAsString);


            //
            // Relationship Type.
            //

            String relationshipType = RelationshipType;

            if (relationshipType != null)
            {
                xmlTextWriter.WriteStartElement("relationshipType");
                xmlTextWriter.WriteStartElement("value");
                xmlTextWriter.WriteString(relationshipType);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
            }


            //
            // Value Type
            //

            String valueType = ValueType;

            if (valueType != null)
            {
                xmlTextWriter.WriteStartElement("valueType");
                xmlTextWriter.WriteStartElement("value");
                xmlTextWriter.WriteString(valueType);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
            }


            //
            // Value Type specific.
            //

            xmlTextWriter.WriteStartElement("valueTypeSpecific");
            ValueToXml(xmlTextWriter, serializationContext);
            xmlTextWriter.WriteEndElement();


            //
            // Concept Name
            //

            if (this.conceptName != null)
            {
                xmlTextWriter.WriteStartElement("conceptName");
                ConceptName.ToXml(xmlTextWriter, serializationContext);
                xmlTextWriter.WriteEndElement();
            }


            //
            // Validation results.
            //

            this.validationResults.ToXml(xmlTextWriter, serializationContext);


            //
            // Visitor exceptions.
            //

            if (this.visitorExceptions.Count > 0)
            {
                xmlTextWriter.WriteStartElement("visitorExceptions");

                foreach (Exception exception in this.visitorExceptions)
                {
                    xmlTextWriter.WriteStartElement("visitorException");
                    xmlTextWriter.WriteStartElement("message");
                    xmlTextWriter.WriteString(exception.Message);
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement("innerException");
                    xmlTextWriter.WriteStartElement("message");
                    xmlTextWriter.WriteString(exception.InnerException.Message);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteStartElement("stackTrace");
                    xmlTextWriter.WriteString(exception.InnerException.StackTrace);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteEndElement();
                }

                xmlTextWriter.WriteEndElement();
            }


            //
            // Child Content Items.
            //

            if (this.childContentItems.Count > 0)
            {
                xmlTextWriter.WriteStartElement("childContentItems");

                foreach (ContentItem childContentItem in this.childContentItems)
                {
                    childContentItem.ToXml(xmlTextWriter, serializationContext);
                }

                xmlTextWriter.WriteEndElement();

            }


            //
            // End of Content Item.
            //

            xmlTextWriter.WriteEndElement(); 
        }

        /// <summary>
        /// Method used by the library to serialize the value of this instance to xml.
        /// </summary>
        /// <param name="xmlTextWriter">The xml text writer.</param>
        /// <param name="serializationContext">The serialization context.</param>
        internal virtual void ValueToXml(XmlTextWriter xmlTextWriter, SerializationContext serializationContext)
        {
            // Do nothing.
        }
    }
}
