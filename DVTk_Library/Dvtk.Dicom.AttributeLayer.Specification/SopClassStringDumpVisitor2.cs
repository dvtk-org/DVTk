using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// Visitor used to create a string dump of the Attribute structure.
    /// </summary>
    public class SopClassStringDumpVisitor2: SopClassVisitor2
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// The nesting level of the current Attribute.
        /// </summary>
        private int nestingLevel = 0;

        /// <summary>
        /// Field needed to make sure that recursive Attribute references wil not result in an
        /// infinite loop while creating the string dump.
        /// </summary>
        private Dictionary<SequenceAttribute, int> parentSequenceAttributes = new Dictionary<SequenceAttribute, int>();

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
        /// Common string dump logging for both a single and Sequence Attribute.
        /// </summary>
        /// <param name="attribute">The Attribute.</param>
        private void VisitAttribute(Attribute attribute)
        {
            stringBuilder.Append(this.prefix + string.Empty.PadLeft(this.nestingLevel, '>') + "(" + attribute.Tag.GroupNumber.ToString("X4") + "," + attribute.Tag.ElementNumber.ToString("X4") + ")");
            stringBuilder.Append(", " + VRHelper.ToString(attribute.ValueRepresentations, "/"));
            stringBuilder.Append(", \"" + attribute.Name + "\"");
            stringBuilder.Append(" ...................." + attribute.ToString3() + "\n");
        }

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
            stringBuilder.Append(this.prefix + "  Attributes\n");

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
            bool visitChilds = true;

            VisitAttribute(sequenceAttribute);

            if (this.parentSequenceAttributes.ContainsKey(sequenceAttribute))
            {
                stringBuilder.Append(this.prefix + "  Sequence Attribute has already been visited. Stopping string dump at this point to avoid infinite loop.\n");

                visitChilds = false;
            }
            else
            {
                this.parentSequenceAttributes.Add(sequenceAttribute, 0);
            }

            this.prefix += "  ";
            this.nestingLevel++;

            return (visitChilds);
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
            stringBuilder.Append("Stringdump using SopClassStringDumpVisitor2\n");

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
        /// <param name="sequenceAttribute">The SequenceAttribute instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public override bool VisitLeaveSequenceAttribute(SequenceAttribute sequenceAttribute)
        {
            this.parentSequenceAttributes.Remove(sequenceAttribute);

            this.prefix = this.prefix.Substring(0, this.prefix.Length - 2);
            this.nestingLevel--;

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
            VisitAttribute(singleAttribute);

            return (true);
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
