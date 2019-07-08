using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    public class DataSet: SortedAttributeList
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// See associated property.
        /// </summary>
        private DimseDataSetPair parent = null;

        // ---------------------
        // - End fields region -
        // ---------------------
        #endregion



        #region - Constructors -
        // -----------------------------
        // - Begin constructors region -
        // -----------------------------

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private DataSet()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The DimseDataSetPair this instance is part of.</param>
        public DataSet(DimseDataSetPair parent)
        {
            this.parent = parent;
        }

        // ---------------------------
        // - End constructors region -
        // ---------------------------
        #endregion



        #region - Public properties -
        // ----------------------------------
        // - Begin public properties region -
        // ----------------------------------

        /// <summary>
        /// Gets the DimseDataSetPair this instance is part of.
        /// </summary>
        public DimseDataSetPair Parent
        {
            get
            {
                return (this.parent);
            }
        }

        // --------------------------------
        // - End public properties region -
        // --------------------------------
        #endregion
    }
}
