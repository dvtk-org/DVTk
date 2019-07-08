using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    ///  A single Attribute specification from the DICOM standard.
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
        /// <param name="valueRepresentations">The VR(s) of this instance.</param>
        /// <param name="name">The name of this instance.</param>
        /// <param name="parent">The parent of this instance.</param>
        public SingleAttribute(Tag tag, VR valueRepresentations, string name, AttributesAndMacros parent)
            : base(tag, valueRepresentations, name, parent)
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
        /// <param name="sopClassVisitor">The SOP Class visitor.</param>
        /// <returns>
        /// true: continue traversing the siblings if this instance.
        /// false: stop traversing the siblings if this instance.
        /// </returns>
        public override bool Accept(SopClassVisitor1 sopClassVisitor)
        {
            return (sopClassVisitor.VisitSingleAttribute(this));
        }

        /// <summary>
        /// Accept method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="attributeVisitor">The Attribute visitor.</param>
        /// <returns>
        /// true: continue traversing the siblings if this instance.
        /// false: stop traversing the siblings if this instance.
        /// </returns>
        public override bool Accept(SopClassVisitor2 attributeVisitor)
        {
            return (attributeVisitor.VisitSingleAttribute(this));
        }

        /// <summary>
        /// Create an Attribute structure from the Module/Macro/Sequence Item structure.
        /// </summary>
        /// <param name="attributes">The list to add this Attribute to.</param>
        public override void CreateAttributeStructure(SortedAttributeList attributes)
        {
            attributes.Add(this);
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
