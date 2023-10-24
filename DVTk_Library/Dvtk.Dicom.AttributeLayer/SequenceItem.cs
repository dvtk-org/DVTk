using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer
{
    /// <summary>
    /// An Sequence Item from an actual DICOM message or DICOM file.
    /// </summary>
    public class SequenceItem : AttributeSet
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// See associated property.
        /// </summary>
        private SequenceAttribute parent = null;

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
        private SequenceItem()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">
        /// The parent of this instance (the Sequence Attribute this instance is contained in)</param>
        public SequenceItem(SequenceAttribute parent)
        {
            this.parent = parent;
            parent.SequenceItems.Add(this);
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
        /// Gets the nesting level of this instance.
        /// </summary>
        public override int NestingLevel
        {
            get
            {
                return (this.parent.NestingLevel + 1);
            }
        }

        /// <summary>
        /// Gets the 1-based index number of this instance.
        /// </summary>
        public int Number
        {
            get
            {
                return (this.parent.SequenceItems.IndexOf(this) + 1);
            }
        }

        /// <summary>
        /// Gets the parent of this instance (the Sequence Attribute this instance is contained in)
        /// </summary>
        public SequenceAttribute Parent
        {
            get
            {
                return (this.parent);
            }
        }

        /// <summary>
        /// Gets the Tag sequence as a string of this instance.
        /// </summary>
        public override string TagSequenceAsString
        {
            get
            {
                return (this.parent.TagSequenceAsString + "(" + Number + ")");
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
            if (attributeSetVisitor.VisitEnterSequenceItem(this))
            {
                foreach (Attribute attribute in this)
                {
                    if (!attribute.Accept(attributeSetVisitor))
                    {
                        break;
                    }
                }
            }

            return (attributeSetVisitor.VisitLeaveSequenceItem(this));
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
