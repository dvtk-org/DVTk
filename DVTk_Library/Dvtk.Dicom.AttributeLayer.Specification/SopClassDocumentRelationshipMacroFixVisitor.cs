using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// Visitor used to complete the (recursive) usage of the Document Relationship Macro that is
    /// only partly present in the raw xml files.
    /// </summary>
    public class SopClassDocumentRelationshipMacroFixVisitor: SopClassVisitor1
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// When not null, the Document Relationship Macro that has already been encountered.
        /// </summary>
        private Macro documentRelationshipMacro = null;

        // ---------------------
        // - End fields region -
        // ---------------------
        #endregion



        #region - Public methods -
        // -------------------------------
        // - Begin public methods region -
        // -------------------------------

        /// <summary>
        /// VisitEnter method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="Macro">The Macro instance to visit.</param>
        /// <returns>
        /// true: traverse the children of this instance.
        /// false: do not traverse the children of this instance.
        /// </returns>
        public override bool VisitEnterMacro(Macro macro)
        {
            if (macro.Name == "Document Relationship Macro")
            {
                this.documentRelationshipMacro = macro;
            }

            return (true);
        }

        /// <summary>
        /// VisitLeave method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="sequenceAttribute">The SequenceAttribute instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public override bool VisitLeaveSequenceAttribute(SequenceAttribute sequenceAttribute)
        {
            if ((sequenceAttribute.Tag.GroupNumber == 0x0040) && (sequenceAttribute.Tag.ElementNumber == 0xA730))
            {
                if (this.documentRelationshipMacro != null)
                {
                    sequenceAttribute.SequenceItem.Add(this.documentRelationshipMacro);
                }
            }

            return (true);
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
