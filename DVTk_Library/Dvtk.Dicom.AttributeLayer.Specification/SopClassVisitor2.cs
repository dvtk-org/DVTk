using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// Implementation of the "Hierarchical Visitor Pattern",
    /// to visit the Attribute structure.
    /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
    /// </summary>
    public class SopClassVisitor2
    {
        #region - Public methods -
        // -------------------------------
        // - Begin public methods region -
        // -------------------------------

        /// <summary>
        /// VisitEnter method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="dimseDataSetPair">The DimseDataSetPair instance to visit.</param>
        /// <returns>
        /// true: traverse the children of this instance.
        /// false: do not traverse the children of this instance.
        /// </returns>
        public virtual bool VisitEnterDimseDataSetPair(DimseDataSetPair dimseDataSetPair)
        {
            return (true);
        }

        /// <summary>
        /// VisitEnter method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="sequenceAttribute">The SequenceAttribute instance to visit.</param>
        /// <returns>
        /// true: traverse the children of this instance.
        /// false: do not traverse the children of this instance.
        /// </returns>
        public virtual bool VisitEnterSequenceAttribute(SequenceAttribute sequenceAttribute)
        {
            return (true);
        }

        /// <summary>
        /// VisitEnter method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="sopClass">The SopClass instance to visit.</param>
        /// <returns>
        /// true: traverse the children of this instance.
        /// false: do not traverse the children of this instance.
        /// </returns>
        public virtual bool VisitEnterSopClass(SopClass sopClass)
        {
            return (true);
        }

        /// <summary>
        /// VisitLeave method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="dimseDataSetPair">The DimseDataSetPair instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public virtual bool VisitLeaveDimseDataSetPair(DimseDataSetPair dimseDataSetPair)
        {
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
        public virtual bool VisitLeaveSequenceAttribute(SequenceAttribute sequenceAttribute)
        {
            return (true);
        }

        /// <summary>
        /// VisitLeave method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="sopClass">The SopClass instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public virtual bool VisitLeaveSopClass(SopClass sopClass)
        {
            return (true);
        }

        /// <summary>
        /// VisitLeave method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="singleAttribute">The SingleAttribute instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public virtual bool VisitSingleAttribute(SingleAttribute singleAttribute)
        {
            return (true);
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
