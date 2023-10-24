using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// The interface that needs to be implemented by the Attribute and Macro class.
    /// </summary>
    public interface IAttributeOrMacro
    {
        /// <summary>
        /// Accept method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="sopClassVisitor">The SOP Class visitor.</param>
        /// <returns>
        /// true: continue traversing the siblings if this instance.
        /// false: stop traversing the siblings if this instance.
        /// </returns>
        bool Accept(SopClassVisitor1 sopClassVisitor);

        /// <summary>
        /// Create an Attribute structure from the Module/Macro/Sequence Item structure.
        /// </summary>
        /// <param name="attributes">The list to add the Attributes to.</param>
        void CreateAttributeStructure(SortedAttributeList attributes);
    }
}
