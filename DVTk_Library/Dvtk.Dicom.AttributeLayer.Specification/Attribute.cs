using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// An Attribute specification from the DICOM standard.
    /// </summary>
    abstract public class Attribute : IAttributeOrMacro
    {
        #region - Fields -
        // -----------------------
        // - Begin fields region -
        // -----------------------

        /// <summary>
        /// See associated property.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// See associated property.
        /// </summary>
        private AttributesAndMacros parent = null;

        /// <summary>
        /// See associated property.
        /// </summary>
        private Tag tag = new Tag();

        /// <summary>
        /// See associated property.
        /// </summary>
        private VR valueRepresentations = VR.None;


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
        private Attribute()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tag">The Tag of this instance.</param>
        /// <param name="vR">The Value Representation(s) of this instance.</param>
        /// <param name="name">The name of this instance.</param>
        /// <param name="parent">The parent of this instance.</param>
        public Attribute(Tag tag, VR valueRepresentations, string name, AttributesAndMacros parent)
        {
            this.tag = tag;
            this.valueRepresentations = valueRepresentations;
            this.name = name;
            this.parent = parent;
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
        /// The name of this instance.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the parent of this instance (which can be a Module, Macro or Sequence Item).
        /// </summary>
        public AttributesAndMacros Parent
        {
            get
            {
                return (this.parent);
            }
        }

        /// <summary>
        /// Gets the path of this instance.
        /// </summary>
        public string Path
        {
            get
            {
                return (ToString3());
            }
        }

        /// <summary>
        /// The tag of this instance.
        /// </summary>
        public Tag Tag
        {
            get
            {
                return tag;
            }
        }

        /// <summary>
        /// The value representation(s) of this instance.
        /// </summary>
        public VR ValueRepresentations
        {
            get
            {
                return (this.valueRepresentations);
            }
        }

        // --------------------------------
        // - End public properties region -
        // --------------------------------
        #endregion



        #region - Internal methods -
        // --------------------------------
        // - Begin internal methods region -
        // --------------------------------

        /// <summary>
        /// Create a string representation of the path of this instance.
        /// </summary>
        /// <param name="prefix">The prefix to use of identation.</param>
        /// <returns>The string representation.</returns>
        internal string ToString2(string prefix)
        {
            return (this.parent.ToString2(prefix) + "\n" + prefix + "(" + this.tag.GroupNumber.ToString("X4") + "," + Tag.ElementNumber.ToString("X4") + "), " + this.valueRepresentations.ToString() + ", \"" + this.name + "\"");
        }

        /// <summary>
        /// Create a string representation of the path of this instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        internal string ToString3()
        {
            return (this.parent.ToString3() + "/(" + this.tag.GroupNumber.ToString("X4") + "," + Tag.ElementNumber.ToString("X4") + ") \"" + this.name + "\"");
        }

        // ------------------------------
        // - End internal methods region -
        // ------------------------------
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
        public abstract bool Accept(SopClassVisitor1 sopClassVisitor);

        /// <summary>
        /// Accept method in the context of the "Hierarchical Visitor Pattern".
        /// See "DVTk_Library\Documentation\Design\Hierarchical Visitor Pattern.htm".
        /// </summary>
        /// <param name="attributeVisitor">The Attribute visitor.</param>
        /// <returns>
        /// true: continue traversing the siblings if this instance.
        /// false: stop traversing the siblings if this instance.
        /// </returns>
        public abstract bool Accept(SopClassVisitor2 attributeVisitor);

        /// <summary>
        /// Create an (single or sequence) Attribute instance using a raw xml file (that is generated by the ODD).
        /// </summary>
        /// <param name="attributeNode">An Attribute node.</param>
        /// <returns>The created Attribute instance.</returns>
        public static Attribute Create(XPathNavigator attributeNode, AttributesAndMacros parent)
        {
            Attribute attribute = null; // Return value;
            string name = string.Empty;
            Tag tag = new Tag();


            //
            // Determine attribute name.
            //

            try
            {
                name = attributeNode.GetAttribute("Name", "");
            }
            catch (Exception exception)
            {
                throw (DefinitionFile.CreateException(attributeNode, "Attribute", "Unable to determine Name", exception));
            }


            //
            // Determine tag.
            //

            tag = Tag.Create(attributeNode);


            //
            // Construct a single attribute or a sequence attribute.
            //

            VR valueRepresentations = VR.None;
            XPathNodeIterator vRNodes = attributeNode.Select("VR");

            if (vRNodes.MoveNext())
            {
                try
                {
                    valueRepresentations = (VR)System.Enum.Parse(typeof(VR), vRNodes.Current.Value.Replace('/', ','));
                }
                catch
                {
                    throw (DefinitionFile.CreateException(attributeNode, "Attribute", "VR node does not contain a valid VR.", null));
                }
            }
            else
            {
                throw (DefinitionFile.CreateException(attributeNode, "Attribute", "VR node not found.", null));
            }

            if (valueRepresentations == VR.SQ)
            {
                attribute = SequenceAttribute.Create(tag, name, attributeNode, parent);
            }
            else
            {
                attribute = new SingleAttribute(tag, valueRepresentations, name, parent);
            }


            //
            // Return the constructed attribute.
            //

            return (attribute);
        }

        /// <summary>
        /// Create an Attribute structure from the Module/Macro/Sequence Item structure.
        /// </summary>
        /// <param name="attributes">The list to add this Attribute to.</param>
        public abstract void CreateAttributeStructure(SortedAttributeList attributes);

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
