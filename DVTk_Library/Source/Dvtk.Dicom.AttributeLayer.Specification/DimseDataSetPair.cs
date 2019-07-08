using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.XPath;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// The combination of a Dimse Command and a Data Set.
    /// </summary>
    public class DimseDataSetPair
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// See associated property.
        /// </summary>
        private DataSet dataSet = null;

        /// <summary>
        /// See associated property.
        /// </summary>
        private string dataSetName = string.Empty;

        /// <summary>
        /// See associated property.
        /// </summary>
        private string dimseName = string.Empty;

        /// <summary>
        /// See associated property.
        /// </summary>
        private List<Module> modules = new List<Module>();

        /// <summary>
        /// See associated property.
        /// </summary>
        private SopClass parent = null;

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
        private DimseDataSetPair()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">The parent SOP Class that contains this instance.</param>
        private DimseDataSetPair(SopClass parent)
        {
            this.parent = parent;
            this.dataSet = new DataSet(this);
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
        /// Gets the DataSet.
        /// </summary>
        public SortedAttributeList DataSet
        {
            get
            {
                return (this.dataSet);
            }
        }

        /// <summary>
        /// Gets the DataSet name.
        /// </summary>
        public string DataSetName
        {
            get
            {
                return (this.dataSetName);
            }
        }

        /// <summary>
        /// Gets the Dimse name.
        /// </summary>
        public string DimseName
        {
            get
            {
                return (this.dimseName);
            }
        }

        /// <summary>
        /// Gets the list of Modules contained in this instance.
        /// </summary>
        public List<Module> Modules
        {
            get
            {
                return (this.modules);
            }
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        public SopClass Parent
        {
            get
            {
                return (this.parent);
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
        /// <param name="sopClassVisitor">The SOP Class visitor.</param>
        /// <returns>
        /// true: continue traversing the siblings if this instance.
        /// false: stop traversing the siblings if this instance.
        /// </returns>
        public bool Accept(SopClassVisitor1 sopClassVisitor)
        {
            if (sopClassVisitor.VisitEnterDimseDataSetPair(this))
            {
                foreach (Module module in this.modules)
                {
                    if (!module.Accept(sopClassVisitor))
                    {
                        break;
                    }
                }
            }

            return (sopClassVisitor.VisitLeaveDimseDataSetPair(this));
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
        public bool Accept(SopClassVisitor2 sopClassVisitor)
        {
            if (sopClassVisitor.VisitEnterDimseDataSetPair(this))
            {
                List<Attribute> attributes = this.DataSet.GetAttributes();

                foreach (Attribute attribute in attributes)
                {
                    if (!attribute.Accept(sopClassVisitor))
                    {
                        break;
                    }
                }
            }

            return (sopClassVisitor.VisitLeaveDimseDataSetPair(this));
        }

        /// <summary>
        /// Create a DimseDataSetPair instance using a raw xml file (that is generated by the ODD).
        /// </summary>
        /// <param name="parent">The parent of the instance to create.</param>
        /// <param name="dimseNode">A Dimse node.</param>
        /// <returns>The created DimseDataSetPair instance.</returns>
        public static DimseDataSetPair Create(SopClass parent, XPathNavigator dimseNode)
        {
            DimseDataSetPair dimseDataSetPair = new DimseDataSetPair(parent);

            dimseDataSetPair.dimseName = dimseNode.GetAttribute("Name", "");

            XPathNodeIterator dataSetNodes = dimseNode.Select("Dataset");

            if (dataSetNodes.MoveNext())
            {
                dimseDataSetPair.dataSetName = dataSetNodes.Current.GetAttribute("Name", "");

                XPathNodeIterator moduleNodes = dataSetNodes.Current.Select("Module");

                foreach (XPathNavigator moduleNode in moduleNodes)
                {
                    Module module = Module.Create(moduleNode);
                    dimseDataSetPair.modules.Add(module);
                }
            }
            else
            {
                dimseDataSetPair = null;
            }

            return (dimseDataSetPair);

        }

        /// <summary>
        /// Create an Attribute structure from the Module/Macro/Sequence Item structure.
        /// </summary>
        public void CreateAttributeStructure()
        {
            foreach (Module module in this.modules)
            {
                module.CreateAttributeStructure(this.DataSet);
            }
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
