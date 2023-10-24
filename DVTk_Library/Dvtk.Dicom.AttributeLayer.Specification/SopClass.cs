using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.XPath;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// A SOP Class specification from the DICOM standard.
    /// </summary>
    public class SopClass
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// See associated property.
        /// </summary>
        private List<DimseDataSetPair> dimseDataSetPairs = new List<DimseDataSetPair>(0);

        /// <summary>
        /// See associated property.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// See associated property.
        /// </summary>
        private string path = string.Empty;

        /// <summary>
        /// See associated property.
        /// </summary>
        private string systemName = string.Empty;

        /// <summary>
        /// See associated property.
        /// </summary>
        private string systemVersion = string.Empty;

        /// <summary>
        /// See associated property.
        /// </summary>
        private string uid = string.Empty;

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
        private SopClass()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of this instance.</param>
        /// <param name="uid">The UID of this instance.</param>
        /// <param name="systemName">The system name of this instance.</param>
        /// <param name="systemVersion">The system version of this instance.</param>
        public SopClass(string name, string uid, string systemName, string systemVersion)
        {
            this.name = name;
            this.uid = uid;
            this.systemName = systemName;
            this.systemVersion = systemVersion;
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
        /// Gets the DimseDataSetPair instances that are part of this instance.
        /// </summary>
        public List<DimseDataSetPair> DimseDataSetPairs
        {
            get
            {
                return (this.dimseDataSetPairs);
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return (this.name);
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path
        {
            get
            {
                return (this.path);
            }
        }

        /// <summary>
        /// Gets the system name.
        /// </summary>
        public string SystemName
        {
            get
            {
                return (this.systemName);
            }
        }

        /// <summary>
        /// Gets the system version.
        /// </summary>
        public string SystemVersion
        {
            get
            {
                return (this.systemVersion);
            }
        }

        /// <summary>
        /// Gets the UID.
        /// </summary>
        public string Uid
        {
            get
            {
                return (this.uid);
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
            if (sopClassVisitor.VisitEnterSopClass(this))
            {
                foreach (DimseDataSetPair dimseDataSetPair in this.dimseDataSetPairs)
                {
                    if (!dimseDataSetPair.Accept(sopClassVisitor))
                    {
                        break;
                    }
                }
            }

            return (sopClassVisitor.VisitLeaveSopClass(this));
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
            if (sopClassVisitor.VisitEnterSopClass(this))
            {
                foreach (DimseDataSetPair dimseDataSetPair in this.dimseDataSetPairs)
                {
                    if (!dimseDataSetPair.Accept(sopClassVisitor))
                    {
                        break;
                    }
                }
            }

            return (sopClassVisitor.VisitLeaveSopClass(this));
        }

        /// <summary>
        /// Create a SOP Class instance using a raw xml file (that is generated by the ODD).
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>The created SopClass instance.</returns>
        public static SopClass Create(string path)
        {
            SopClass sopClass = new SopClass();

            try
            {
                XPathNodeIterator systemNodes = null;
                XPathNodeIterator sopClassNodes = null;
                XPathNodeIterator dimseNodes = null;

                sopClass.path = path;

                XPathDocument document = new XPathDocument(path);
                XPathNavigator navigator = document.CreateNavigator();


                //
                // Determine system name and system version.
                //

                systemNodes = navigator.Select("/System");
                if (systemNodes.MoveNext())
                {
                    sopClass.systemName = systemNodes.Current.GetAttribute("Name", "");
                    sopClass.systemVersion = systemNodes.Current.GetAttribute("Version", "");

                    if ((sopClass.systemName.Length == 0) || (sopClass.systemVersion.Length == 0))
                    {
                        throw new Exception("System name and/or system version not found.");
                    }
                }
                else
                {
                    throw new Exception("/System node not found in \"" + path + "\"");
                }


                //
                // Determine name and UID.
                //

                if (sopClass != null)
                {
                    sopClassNodes = systemNodes.Current.Select("ApplicationEntity/SOPClass");
                    if (sopClassNodes.MoveNext())
                    {
                        sopClass.name = sopClassNodes.Current.GetAttribute("Name", "");
                        sopClass.uid = sopClassNodes.Current.GetAttribute("UID", "");

                        if ((sopClass.name.Length == 0) || (sopClass.uid.Length == 0))
                        {
                            throw new Exception("Name and/or uid not found.");
                        }
                    }
                    else
                    {
                        throw new Exception("SOPClass node not found.");
                    }
                }


                //
                // Determine DimseDataSetPairs.
                //

                if (sopClass != null)
                {
                    dimseNodes = sopClassNodes.Current.Select("Dimse");

                    foreach (XPathNavigator dimseNode in dimseNodes)
                    {
                        string dimseName = dimseNode.GetAttribute("Name", "");

                        if (dimseName.Substring(dimseName.Length - 2, 2) == "RQ")
                        {
                            DimseDataSetPair dimseDataSetPair = DimseDataSetPair.Create(sopClass, dimseNode);
                            sopClass.DimseDataSetPairs.Add(dimseDataSetPair);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Exception occured while reading file \"" + path + "\".\n" + exception.Message);
            }

            return (sopClass);
        }

        /// <summary>
        /// Create an Attribute structure from the Module/Macro/Sequence Item structure.
        /// </summary>
        public void CreateAttributeReferencesRecursively()
        {
            foreach (DimseDataSetPair dimseDataSetPair in this.dimseDataSetPairs)
            {
                dimseDataSetPair.CreateAttributeStructure();
            }
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
