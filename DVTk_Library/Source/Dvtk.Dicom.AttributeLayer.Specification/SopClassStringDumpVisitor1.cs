using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// Visitor used to create a string dump of the Module/Macro/Sequence Item structure.
    /// </summary>
    public class SopClassStringDumpVisitor1: SopClassVisitor1
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// Needed to make sure that recursive macros will not result in a infinite loop while
        /// creating a string dump.
        /// </summary>
        private Dictionary<Macro, int> parentMacros = new Dictionary<Macro, int>();

        /// <summary>
        /// The prefix of the current line in the string dump.
        /// </summary>
        private string prefix = string.Empty;

        /// <summary>
        /// See the StringDump property.
        /// </summary>
        private StringBuilder stringBuilder = new StringBuilder();

        // ---------------------
        // - End fields region -
        // ---------------------
        #endregion



        #region - Public properties -
        // ----------------------------------
        // - Begin public properties region -
        // ----------------------------------

        /// <summary>
        /// Gets the string dump (first visit a SOP Class before using this property).
        /// </summary>
        public string StringDump
        {
            get
            {
                return (this.stringBuilder.ToString());
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
        /// VisitEnter method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="dimseDataSetPair">The DimseDataSetPair instance to visit.</param>
        /// <returns>
        /// true: traverse the children of this instance.
        /// false: do not traverse the children of this instance.
        /// </returns>
        public override bool VisitEnterDimseDataSetPair(DimseDataSetPair dimseDataSetPair)
        {
            stringBuilder.Append(this.prefix + "DimseDataSetPair\n");
            stringBuilder.Append(this.prefix + "  Dimse name: " + dimseDataSetPair.DimseName + "\n");
            stringBuilder.Append(this.prefix + "  DataSet name: " + dimseDataSetPair.DataSetName + "\n");
            stringBuilder.Append(this.prefix + "  Modules\n");

            this.prefix += "    ";

            return (true);
        }

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
            bool visitChilds = true;

            stringBuilder.Append(this.prefix + "Macro\n");
            stringBuilder.Append(this.prefix + "  Name: " + macro.Name + "\n");

            if (this.parentMacros.ContainsKey(macro))
            {
                stringBuilder.Append(this.prefix + "  Macro has already been visited. Stopping string dump of this Macro to avoid infinite loop.\n");

                visitChilds = false;
            }
            else
            {
                stringBuilder.Append(this.prefix + "  Attributes and Macros\n");

                this.parentMacros.Add(macro, 0);
            }

            this.prefix += "    ";

            return (visitChilds);
        }

        /// <summary>
        /// VisitEnter method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="module">The Module instance to visit.</param>
        /// <returns>
        /// true: traverse the children of this instance.
        /// false: do not traverse the children of this instance.
        /// </returns>
        public override bool VisitEnterModule(Module module)
        {
            stringBuilder.Append(this.prefix + "Module\n");
            stringBuilder.Append(this.prefix + "  Name: " + module.Name + "\n");
            stringBuilder.Append(this.prefix + "  Usage: " + module.Usage + "\n");
            stringBuilder.Append(this.prefix + "  Attributes and Macros\n");

            this.prefix += "    ";

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
        public override bool VisitEnterSequenceAttribute(SequenceAttribute sequenceAttribute)
        {
            stringBuilder.Append(this.prefix + "Sequence Attribute\n");
            stringBuilder.Append(this.prefix + "  Tag: (" + sequenceAttribute.Tag.GroupNumber.ToString("X4") + "," + sequenceAttribute.Tag.ElementNumber.ToString("X4") + ")\n");
            stringBuilder.Append(this.prefix + "  Name: " + sequenceAttribute.Name + "\n");
            stringBuilder.Append(this.prefix + "  VR: " + sequenceAttribute.ValueRepresentations + "\n");
            stringBuilder.Append(this.prefix + "  Path\n");
            stringBuilder.Append(this.prefix + "    " + sequenceAttribute.ToString2(prefix + "    ") + "\n");

            this.prefix += "    ";

            return (true);
        }

        /// <summary>
        /// VisitEnter method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="SequenceItem">The SequenceItem instance to visit.</param>
        /// <returns>
        /// true: traverse the children of this instance.
        /// false: do not traverse the children of this instance.
        /// </returns>
        public override bool VisitEnterSequenceItem(SequenceItem SequenceItem)
        {
            stringBuilder.Append(this.prefix + "Sequence Item\n");
            stringBuilder.Append(this.prefix + "  Attributes and Macros\n");

            this.prefix += "    ";

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
        public override bool VisitEnterSopClass(SopClass sopClass)
        {
            stringBuilder.Append("Stringdump using SopClassStringDumpVisitor1\n");

            stringBuilder.Append(prefix + "SOP Class:\n");
            stringBuilder.Append(prefix + "  Path: \"" + sopClass.Path + "\"\n");
            stringBuilder.Append(prefix + "  System name: " + sopClass.SystemName + "\n");
            stringBuilder.Append(prefix + "  System version: " + sopClass.SystemVersion + "\n");
            stringBuilder.Append(prefix + "  Name: " + sopClass.Name + "\n");
            stringBuilder.Append(prefix + "  UID: " + sopClass.Uid + "\n");
            stringBuilder.Append(prefix + "  DimseDataSetsPairs\n");

            this.prefix += "    ";

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
        public override bool VisitLeaveDimseDataSetPair(DimseDataSetPair dimseDataSetPair)
        {
            this.prefix = this.prefix.Substring(0, this.prefix.Length - 4);

            return (true);
        }

        /// <summary>
        /// VisitLeave method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="macro">The Macro instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public override bool VisitLeaveMacro(Macro macro)
        {
            this.prefix = this.prefix.Substring(0, this.prefix.Length - 4);

            this.parentMacros.Remove(macro);

            return (true);
        }

        /// <summary>
        /// VisitLeave method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="module">The Module instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public override bool VisitLeaveModule(Module module)
        {
            this.prefix = this.prefix.Substring(0, this.prefix.Length - 4);

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
            this.prefix = this.prefix.Substring(0, this.prefix.Length - 4);

            return (true);
        }

        /// <summary>
        /// VisitLeave method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="SequenceItem">The SequenceItem instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public override bool VisitLeaveSequenceItem(SequenceItem SequenceItem)
        {
            this.prefix = this.prefix.Substring(0, this.prefix.Length - 4);

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
        public override bool VisitLeaveSopClass(SopClass sopClass)
        {
            this.prefix = this.prefix.Substring(0, this.prefix.Length - 4);

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
        public override bool VisitSingleAttribute(SingleAttribute singleAttribute)
        {
            stringBuilder.Append(this.prefix + "Single Attribute\n");
            stringBuilder.Append(this.prefix + "  Tag: (" + singleAttribute.Tag.GroupNumber.ToString("X4") + "," + singleAttribute.Tag.ElementNumber.ToString("X4") + ")\n");
            stringBuilder.Append(this.prefix + "  Name: " + singleAttribute.Name + "\n");
            stringBuilder.Append(this.prefix + "  VR: " + singleAttribute.ValueRepresentations + "\n");
            stringBuilder.Append(this.prefix + "  Path\n");
            stringBuilder.Append(this.prefix + "    " + singleAttribute.ToString2(prefix + "    ") + "\n");

            return (true);
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
