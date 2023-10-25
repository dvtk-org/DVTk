using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace Dvtk.Dicom.AttributeLayer
{
    /// <summary>
    /// A Tag from an actual DICOM message or DICOM file.
    /// </summary>
    public class Tag
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// See associated property.
        /// </summary>
        private UInt16 elementNumber = 0;

        /// <summary>
        /// See associated property.
        /// </summary>
        private UInt16 groupNumber = 0;

        // ---------------------
        // - End fields region -
        // ---------------------
        #endregion
        


        #region - Constructors -
        // -----------------------------
        // - Begin constructors region -
        // -----------------------------

        /// <summary>
        /// Default constructor to construct a Tag with group number 0 and element number 0.
        /// </summary>
        public Tag()
        {
            this.elementNumber = 0;
            this.groupNumber = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="groupNumber">The group number of this instance.</param>
        /// <param name="elementNumber">The element number of this instance.</param>
        public Tag(UInt16 groupNumber, UInt16 elementNumber)
        {
            this.elementNumber = elementNumber;
            this.groupNumber = groupNumber;
        }

        // ---------------------------
        // - End constructors region -
        // ---------------------------
        #endregion



        #region - Public enums -
        // -----------------------------
        // - Begin public enums region -
        // -----------------------------

        public enum Format
        {
            Dicom
        }

        // ---------------------------
        // - End public enums region -
        // ---------------------------
        #endregion



        #region - Public properties -
        // ----------------------------------
        // - Begin public properties region -
        // ----------------------------------

        /// <summary>
        /// Gets the element number of this instance.
        /// </summary>
        public UInt16 ElementNumber
        {
            get
            {
                return (this.elementNumber);
            }
        }

        /// <summary>
        /// Gets the element number of this instance.
        /// </summary>
        public UInt16 GroupNumber
        {
            get
            {
                return (this.groupNumber);
            }
        }

        // --------------------------------
        // - End public properties region -
        // --------------------------------
        #endregion



        #region - Public methods -
        // -------------------------------
        // - Begin public methods region -
        // -------------------------------

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <param name="format">The format of the string representation.</param>
        /// <returns>A string that represents this instance.</returns>
        public string ToString(Format format)
        {
            string returnString = string.Empty;

            switch (format)
            {
                case Format.Dicom:
                    returnString = "(" + this.groupNumber.ToString("X4") + "," + this.elementNumber.ToString("X4") + ")";
                    break;
            }

            return (returnString);
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
