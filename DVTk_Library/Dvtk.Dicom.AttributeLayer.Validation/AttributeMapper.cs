using System;
using System.Collections.Generic;
using System.Text;

using Attribute = Dvtk.Dicom.AttributeLayer.Attribute;
using AttributeSpecification = Dvtk.Dicom.AttributeLayer.Specification.Attribute;
using AttributeSet = Dvtk.Dicom.AttributeLayer.AttributeSet;
using SequenceAttribute = Dvtk.Dicom.AttributeLayer.SequenceAttribute;
using SequenceItem = Dvtk.Dicom.AttributeLayer.SequenceItem;
using SequenceAttributeSpecification = Dvtk.Dicom.AttributeLayer.Specification.SequenceAttribute;
using SortedAttributeListSpecification = Dvtk.Dicom.AttributeLayer.Specification.SortedAttributeList;
using TagSpecification = Dvtk.Dicom.AttributeLayer.Specification.Tag;

namespace Dvtk.Dicom.AttributeLayer.Validation
{
    /// <summary>
    /// Class that tries the map actual Attributes to Attribute specifications. The outcome of this
    /// mapping is stored by setting an AttributeData instance in the Data property of an actual 
    /// Attribute.
    /// </summary>
    public static class AttributeMapper
    {

        #region - Private methods -
        // --------------------------------
        // - Begin private methods region -
        // --------------------------------

        /// <summary>
        /// Gets the list of all possible Attribute specifications that can be used to map Attributes in a Sequence Item to.
        /// </summary>
        /// <param name="mapping">The mapping of the current Attribute.</param>
        /// <returns></returns>
        private static SortedAttributeListSpecification GetSequenceItemSpecification(List<AttributeSpecification> mapping)
        {
            SortedAttributeListSpecification sequenceItemSpecification = new SortedAttributeListSpecification();

            foreach (AttributeSpecification attributeSpecification in mapping)
            {
                if (attributeSpecification is SequenceAttributeSpecification)
                {
                    SequenceAttributeSpecification sequenceAttributeSpecification = attributeSpecification as SequenceAttributeSpecification;

                    List<AttributeSpecification> attributeSpecificationsInSequenceItem = sequenceAttributeSpecification.SortedAttributeList.GetAttributes();

                    foreach (AttributeSpecification attributeSpecificationInSequenceItem in attributeSpecificationsInSequenceItem)
                    {
                        sequenceItemSpecification.Add(attributeSpecificationInSequenceItem);
                    }
                }
            }

            return (sequenceItemSpecification);
        }

        // ------------------------------
        // - End private methods region -
        // ------------------------------
        #endregion



        #region - Public methods -
        // -------------------------------
        // - Begin public methods region -
        // -------------------------------

        /// <summary>
        /// Try to map actual Attributes to Attribute specifications.
        /// </summary>
        /// <param name="attributeSet">The actual Attributes to map.</param>
        /// <param name="sortedAttributeListSpecification">The Attribute specifications to map to.</param>
        /// <returns>
        /// Indicates if all supplied actual Attributes and their direct and indirect
        /// Attribute childs have been mapped.
        /// </returns>
        public static bool Map(AttributeSet attributeSet, SortedAttributeListSpecification sortedAttributeListSpecification)
        {
            bool attributesMapped = true;

            foreach (Attribute attribute in attributeSet)
            {
                bool attributeMapped = Map(attribute, sortedAttributeListSpecification);
                attributesMapped = (attributesMapped && attributeMapped);
            }

            return (attributesMapped);
        }

        /// <summary>
        /// Try to map one Attribute to Attribute specifications.
        /// </summary>
        /// <param name="attribute">The actual Attribute to map.</param>
        /// <param name="sortedAttributeListSpecification">The Attribute specifications to map to.</param>
        /// <returns>
        /// Indicates if the supplied actual Attribute and it's direct and indirect
        /// Attribute childs have been mapped.
        /// </returns>
        public static bool Map(Attribute attribute, SortedAttributeListSpecification sortedAttributeListSpecification)
        {
            bool attributesMapped = true;

            //
            // Determine which attribute specification instances have the same tag and level.
            //

            AttributeData attributeData = new AttributeData();

            attribute.Data = attributeData;

            attributeData.Mapping = sortedAttributeListSpecification.GetAttributes(attribute.Tag.GroupNumber, attribute.Tag.ElementNumber);

            if (attributeData.Mapping.Count == 0)
            {
                attributesMapped = false;
            }

            //
            // If this attribute is a Sequence Attribute, also map its children.
            //

            if (attribute is SequenceAttribute)
            {
                SequenceAttribute sequenceAttribute = attribute as SequenceAttribute;

                SortedAttributeListSpecification sequenceItemSpecification = GetSequenceItemSpecification(attributeData.Mapping);

                foreach (SequenceItem sequenceItem in sequenceAttribute.SequenceItems)
                {
                    bool sequenceItemMapped = Map(sequenceItem, sequenceItemSpecification);

                    attributesMapped = (attributesMapped && sequenceItemMapped);
                }
            }

            attributeData.AttributesMapped = attributesMapped;

            return (attributesMapped);
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
