using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer
{
    /// <summary>
    /// A DataSet from an actual DICOM message or DICOM file.
    /// </summary>
    public class DataSet : AttributeSet
    {
        #region - Public properties -
        // ----------------------------------
        // - Begin public properties region -
        // ----------------------------------

        /// <summary>
        /// Gets the nesting level of this instance.
        /// </summary>
        public override int NestingLevel
        {
            get
            {
                return (0);
            }
        }

        /// <summary>
        /// Gets the Tag sequence as a string of this instance.
        /// </summary>
        public override string TagSequenceAsString
        {
            get
            {
                return (string.Empty);
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
        /// Accept method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="attributeSetVisitor">The Attribute Set visitor.</param>
        /// <returns>
        /// true: continue traversing the siblings of this instance.
        /// false: stop traversing the siblings of this instance.
        /// </returns>
        public bool Accept(AttributeSetVisitor attributeSetVisitor)
        {
            if (attributeSetVisitor.VisitEnterDataSet(this))
            {
                foreach (Attribute attribute in this)
                {
                    if (!attribute.Accept(attributeSetVisitor))
                    {
                        break;
                    }
                }
            }

            return (attributeSetVisitor.VisitLeaveDataSet(this));
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
