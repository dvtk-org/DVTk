using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer
{
    /// <summary>
    /// A Sequence Attribute from an actual DICOM message or DICOM file.
    /// </summary>
    public class SequenceAttribute : Attribute
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// See associated property.
        /// </summary>
        private List<SequenceItem> sequenceItems = new List<SequenceItem>();

        // ---------------------
        // - End fields region -
        // ---------------------
        #endregion



        #region - Constructors -
        // -----------------------------
        // - Begin constructors region -
        // -----------------------------

        public SequenceAttribute(Tag tag, AttributeSet parent)
            : base(tag, VR.SQ, parent)
        {
            // Do nothing.
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
        /// Gets the list of Sequence Items for this instance.
        /// </summary>
        public List<SequenceItem> SequenceItems
        {
            get
            {
                return (this.sequenceItems);
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
        public override bool Accept(AttributeSetVisitor attributeSetVisitor)
        {
            if (attributeSetVisitor.VisitEnterSequenceAttribute(this))
            {
                foreach (SequenceItem sequenceItem in this.sequenceItems)
                {
                    if (!sequenceItem.Accept(attributeSetVisitor))
                    {
                        break;
                    }
                }
            }

            return (attributeSetVisitor.VisitLeaveSequenceAttribute(this));
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
