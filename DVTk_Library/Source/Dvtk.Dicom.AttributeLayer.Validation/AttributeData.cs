using System;
using System.Collections.Generic;
using System.Text;

using AttributeSpecification = Dvtk.Dicom.AttributeLayer.Specification.Attribute;

namespace Dvtk.Dicom.AttributeLayer.Validation
{
    /// <summary>
    /// Extra user data that will be added to an Dvtk.Dicom.AttributeLayer.Attribute instance for
    /// validation purposes.
    /// </summary>
    class AttributeData
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// See associated property.
        /// </summary>
        private bool attributesMapped = true;

        /// <summary>
        /// See associated property.
        /// </summary>
        private List<AttributeSpecification> mapping = new List<Dvtk.Dicom.AttributeLayer.Specification.Attribute>();

        // ---------------------
        // - End fields region -
        // ---------------------
        #endregion



        #region - Public properties -
        // ----------------------------------
        // - Begin public properties region -
        // ----------------------------------

        /// <summary>
        /// Gets of sets a boolean indicating if this attribute and all it's direct and indirect
        /// childs have been mapped.
        /// </summary>
        public bool AttributesMapped
        {
            get
            {
                return (this.attributesMapped);
            }
            set
            {
                this.attributesMapped = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of Attribute specifications that the actual Attribute (that
        /// contains an instance of this AttributeData class) is mapped to.
        /// </summary>
        public List<AttributeSpecification> Mapping
        {
            get
            {
                return (this.mapping);
            }
            set
            {
                this.mapping = value;
            }
        }

        // --------------------------------
        // - End public properties region -
        // --------------------------------
        #endregion
    }
}
