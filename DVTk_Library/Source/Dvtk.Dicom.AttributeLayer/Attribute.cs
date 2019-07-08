using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer
{
    /// <summary>
    /// An Attribute from an actual DICOM message or DICOM file.
    /// </summary>
    public abstract class Attribute
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// See associated property.
        /// </summary>
        private object data = null;

        /// <summary>
        /// See associated property.
        /// </summary>
        private AttributeSet parent = null;

        /// <summary>
        /// See associated property.
        /// </summary>
        private Tag tag = new Tag();

        /// <summary>
        /// See associated property.
        /// </summary>
        private VR vr = VR.None;

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
        private Attribute()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tag">The tag of this instance.</param>
        /// <param name="vr">The VR of this instance.</param>
        /// <param name="parent">The parent of this instance.</param>
        public Attribute(Tag tag, VR vr, AttributeSet parent)
        {
            this.tag = tag;
            this.vr = vr;
            this.parent = parent;
            this.parent.Add(this);
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
        /// Gets or sets the user defined object of this instance.
        /// </summary>
        public Object Data
        {
            get
            {
                return (this.data);
            }
            set
            {
                this.data = value;
            }
        }

        /// <summary>
        /// Gets the nesting level of this instance.
        /// </summary>
        public int NestingLevel
        {
            get
            {
                return (this.parent.NestingLevel);
            }
        }

        /// <summary>
        /// Gets the parent (the Data Set, Command Set or Sequence Item this instance is contained in)
        /// of of this instance.
        /// </summary>
        public AttributeSet Parent
        {
            get
            {
                return (this.parent);
            }
        }

        /// <summary>
        /// Gets the Tag of this instance.
        /// </summary>
        public Tag Tag
        {
            get
            {
                return tag;
            }
        }

        /// <summary>
        /// Gets the Tag sequence as a string of this instance.
        /// </summary>
        public string TagSequenceAsString
        {
            get
            {
                string tagSequenceAsString = string.Empty;

                string parentTagSequenceAsString = this.parent.TagSequenceAsString;

                tagSequenceAsString = this.Tag.ToString(Tag.Format.Dicom);

                if (parentTagSequenceAsString.Length > 0)
                {
                    tagSequenceAsString = parentTagSequenceAsString + "/" + tagSequenceAsString;
                }

                return (tagSequenceAsString);
            }
        }

        /// <summary>
        /// Gets the VR of this instance.
        /// </summary>
        public VR VR
        {
            get
            {
                return (this.vr);
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
        public abstract bool Accept(AttributeSetVisitor attributeSetVisitor);

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
