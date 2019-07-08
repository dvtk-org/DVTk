using System;
using System.Collections.Generic;
using System.Text;

using AttributeSpecification = Dvtk.Dicom.AttributeLayer.Specification.Attribute;
using Dvtk.Dicom.AttributeLayer;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Common.Other;

namespace Dvtk.Dicom.AttributeLayer.Validation
{
    /// <summary>
    /// Visitor that adds the mapping information in the summary and detailed results files using
    /// two different tables. The table for the detailed results contains mapping information for
    /// all actual attributes. The table for the summary results contains only the actual 
    /// Attributes that could not be mapped and their parent Attributes for context information.
    /// </summary>
    public class AttributeSetResultsLoggingVisitor: Dvtk.Dicom.AttributeLayer.AttributeSetVisitor
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// The DicomThread instance for which to add the mapping results in the summary and
        /// detailed results files.
        /// </summary>
        private DicomThread dicomThread = null;

        /// <summary>
        /// The table for the detailed results.
        /// </summary>
        private Table tableForDetailedResults = new Table(4);

        /// <summary>
        /// The table for the summary results.
        /// </summary>
        private Table tableForSummaryResults = new Table(4);

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
        private AttributeSetResultsLoggingVisitor()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dicomThread">The DicomThread to add the mapping information to in it's results.</param>
        public AttributeSetResultsLoggingVisitor(DvtkHighLevelInterface.Dicom.Threads.DicomThread dicomThread)
        {
            this.dicomThread = dicomThread;
        }

        // ---------------------------
        // - End constructors region -
        // ---------------------------
        #endregion



        #region - Private methods -
        // --------------------------------
        // - Begin private methods region -
        // --------------------------------

        /// <summary>
        /// Common results logging for both a single and Sequence Attribute.
        /// </summary>
        /// <param name="attribute">The Attribute.</param>
        private void VisitAttribute(Attribute attribute)
        {
            AttributeData attributeData = (AttributeData)attribute.Data;

            //
            // Detailed Results: show all attributes.
            //

            WriteCommonInformation(attribute, this.tableForDetailedResults);


            //
            // Sumary results: show only attributes that are not mapped or have child attributes that are not mapped.
            //

            if (!attributeData.AttributesMapped)
            {
                WriteCommonInformation(attribute, this.tableForSummaryResults);
            }
        }

        /// <summary>
        /// Method used to write an extra row of Atttribute information in both a summary and detailed table.
        /// </summary>
        /// <param name="attribute">The Atttribute.</param>
        /// <param name="table">The table.</param>
        private void WriteCommonInformation(Attribute attribute, Table table)
        {
            table.NewRow();
            table.AddBlackItem(1, string.Empty.PadLeft(attribute.NestingLevel, '>') + attribute.Tag.ToString(Tag.Format.Dicom));
            table.AddBlackItem(2, attribute.VR.ToString());

            AttributeData attributeData = (AttributeData)attribute.Data;

            if (attributeData.Mapping.Count == 0)
            {
                table.AddRedItem(4, "Attribute " + attribute.TagSequenceAsString + " could not be mapped to any attribute specification!\n");

                bool continueToFindMappedParent = true;
                Attribute attributeToCheck = attribute;
                Attribute mappedAttribute = null;

                do
                {
                    if (attributeToCheck.Parent is SequenceItem)
                    {
                        attributeToCheck = (attributeToCheck.Parent as SequenceItem).Parent;
                    }
                    else if (attributeToCheck.Parent is DataSet)
                    {
                        // No more parent attributes available.
                        continueToFindMappedParent = false;
                    }

                    if ((attributeToCheck.Data as AttributeData).Mapping.Count > 0)
                    {
                        mappedAttribute = attributeToCheck;
                        continueToFindMappedParent = false;
                    }
                }
                while (continueToFindMappedParent);

                if (mappedAttribute != null)
                {
                    table.AddBlackItem(4, "First parent attribute that can be mapped is " + mappedAttribute.TagSequenceAsString + " and is mapped to:");

                    foreach (AttributeSpecification attributeSpecification in (mappedAttribute.Data as AttributeData).Mapping)
                    {
                        table.AddBlackItem(4, "Mapped to " + attributeSpecification.Path);
                    }

                }
                else
                {
                    if (attribute.NestingLevel > 0)
                    {
                        table.AddBlackItem(4, "None of the parent attributes can be mapped also");
                    }
                }
            }
            else
            {
                table.AddBlackItem(3, attributeData.Mapping[0].Name);

                foreach (AttributeSpecification attributeSpecification in attributeData.Mapping)
                {
                    table.AddBlackItem(4, "Mapped to " + attributeSpecification.Path);
                }
            }
        }

        // ------------------------------
        // - End private methods region -
        // ------------------------------
        #endregion



        #region - Public methods -
        // -------------------------------
        // - Begin public methods region -
        // -------------------------------

        /// <summary>
        /// VisitEnter method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="dataSet">The DataSet instance to visit.</param>
        /// <returns>
        /// true: traverse the children of this instance.
        /// false: do not traverse the children of this instance.
        /// </returns>
        public override bool VisitEnterDataSet(DataSet dataSet)
        {
            string description = "Check for attributes in the DICOM file that are not definined in the raw xml definition files";

            this.tableForDetailedResults.AddHeader(description, description, description, description);
            this.tableForDetailedResults.AddHeader("______Tag______", "__VR__", " Name ", " Comments ");
            this.tableForDetailedResults.CellItemSeperator = "<br><br>";

            this.tableForSummaryResults.AddHeader(description, description, description, description);
            this.tableForSummaryResults.AddHeader("______Tag______", "__VR__", " Name ", " Comments ");
            this.tableForSummaryResults.CellItemSeperator = "<br><br>";

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
            VisitAttribute(sequenceAttribute);

            return (true);
        }

        /// <summary>
        /// VisitEnter method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="sequenceItem">The SequenceItem instance to visit.</param>
        /// <returns>
        /// true: traverse the children of this instance.
        /// false: do not traverse the children of this instance.
        /// </returns>
        public override bool VisitEnterSequenceItem(SequenceItem sequenceItem)
        {
            this.tableForDetailedResults.NewRow();
            this.tableForDetailedResults.AddBlackItem(1, string.Empty.PadLeft(sequenceItem.Parent.NestingLevel + 1, '>') + "Begin Item " + sequenceItem.Number.ToString());

            AttributeData attributeData = (AttributeData)sequenceItem.Parent.Data;

            if (!attributeData.AttributesMapped)
            {
                this.tableForSummaryResults.NewRow();
                this.tableForSummaryResults.AddBlackItem(1, string.Empty.PadLeft(sequenceItem.Parent.NestingLevel + 1, '>') + "Begin Item " + sequenceItem.Number.ToString());
            }

            return (true);
        }

        /// <summary>
        /// VisitLeave method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="dataSet">The DataSet instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public override bool VisitLeaveDataSet(DataSet dataSet)
        {
            this.dicomThread.WriteHtml(this.tableForSummaryResults.ConvertToHtml(), true, false);
            this.dicomThread.WriteHtml(this.tableForDetailedResults.ConvertToHtml(), false, true);

            return (true);
        }

        /// <summary>
        /// VisitLeave method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="sequenceItem">The SequenceItem instance to visit.</param>
        /// <returns>
        /// true: continue traversing the siblings of the supplied instance.
        /// false: stop traversing the siblings of the supplied instance.
        /// </returns>
        public override bool VisitLeaveSequenceItem(SequenceItem sequenceItem)
        {
            this.tableForDetailedResults.NewRow();
            this.tableForDetailedResults.AddBlackItem(1, string.Empty.PadLeft(sequenceItem.Parent.NestingLevel + 1, '>') + "End Item " + sequenceItem.Number.ToString());

            AttributeData attributeData = (AttributeData)sequenceItem.Parent.Data;

            if (!attributeData.AttributesMapped)
            {
                this.tableForSummaryResults.NewRow();
                this.tableForSummaryResults.AddBlackItem(1, string.Empty.PadLeft(sequenceItem.Parent.NestingLevel + 1, '>') + "End Item " + sequenceItem.Number.ToString());
            }

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
