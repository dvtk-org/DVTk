using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer
{
    /// <summary>
    /// A single Attribute from an actual DICOM message or DICOM file.
    /// </summary>
    public class SingleAttribute : Attribute
    {
        #region - Constructors -
        // -----------------------------
        // - Begin constructors region -
        // -----------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tag">The tag of this instance.</param>
        /// <param name="vr">The VR of this instance.</param>
        /// <param name="parent">The parent of this instance.</param>
        public SingleAttribute(Tag tag, VR vr, AttributeSet parent)
            : base(tag, vr, parent)
        {
            // Do nothing.
        }

        // ---------------------------
        // - End constructors region -
        // ---------------------------
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
            return (attributeSetVisitor.VisitSingleAttribute(this));
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
