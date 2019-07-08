using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// A sorted Attribute List, that is sorted on Tag.
    /// </summary>
    public class SortedAttributeList: SortedList<Tag, List<Attribute>>
    {
        #region - Constructors -
        // -----------------------------
        // - Begin constructors region -
        // -----------------------------

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SortedAttributeList()
            : base(new Tag.Comparer())
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
        /// Add an Attribute to the sorted list.
        /// </summary>
        /// <param name="attribute"></param>
        public void Add(Attribute attribute)
        {
            List<Attribute> attributes = null;

            if (!this.TryGetValue(attribute.Tag, out attributes))
            {
                attributes = new List<Attribute>();
                this.Add(attribute.Tag, attributes);
            }

            attributes.Add(attribute);
        }

        /// <summary>
        /// Get a normal unsorted list of Attributes.
        /// </summary>
        /// <returns></returns>
        public List<Attribute> GetAttributes()
        {
            List<Attribute> attributes = new List<Attribute>();

            foreach (Tag tag in this.Keys)
            {
                foreach (Attribute attribute in this[tag])
                {
                    attributes.Add(attribute);
                }
            }

            return (attributes);
        }

        /// <summary>
        /// Get a list of all attributes with the supplied group number and element number.
        /// </summary>
        /// <param name="groupNumber">The group number.</param>
        /// <param name="elementNumber">The element number.</param>
        /// <returns>A list of all attributes with the supplied group number and element number.</returns>
        public List<Attribute> GetAttributes(UInt16 groupNumber, UInt16 elementNumber)
        {
            List<Attribute> attributes = null;

            if (!this.TryGetValue(new Tag(groupNumber, elementNumber), out attributes))
            {
                attributes = new List<Attribute>();
            }

            return (attributes);
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
