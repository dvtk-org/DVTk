using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer
{
    /// <summary>
    /// Common functionality for the DataSet and SequenceItem classes.
    /// </summary>
    public abstract class AttributeSet : List<Attribute>
    {
        #region - Public properties -
        // ----------------------------------
        // - Begin public properties region -
        // ----------------------------------

        /// <summary>
        /// Gets the nesting level of this instance.
        /// </summary>
        public abstract int NestingLevel
        {
            get;
        }

        /// <summary>
        /// Gets the Tag sequence as a string of this instance.
        /// </summary>
        public abstract string TagSequenceAsString
        {
            get;
        }

        // --------------------------------
        // - End public properties region -
        // --------------------------------
        #endregion
    }
}
