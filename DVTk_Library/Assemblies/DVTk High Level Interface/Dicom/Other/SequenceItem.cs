// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>



using System;
using System.Diagnostics;

using VR = DvtkData.Dimse.VR;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// An object of this class represents a Sequence Item.
	/// </summary>
	public class SequenceItem: AttributeSet
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor. Use it to construct a new empty sequence item.
		/// </summary>
		public SequenceItem(): base(new DvtkData.Dimse.SequenceItem())
		{

		}

		/// <summary>
		/// Constructor.
		/// 
		/// Use this to encapsulate an existing DvtkData sequence item.
		/// </summary>
		/// <param name="tagSequence">The tag sequence of this sequence item.</param>
		/// <param name="dvtkDataSequenceItem">The encapsulated DvtkData SequenceItem.</param>
		internal SequenceItem(TagSequence tagSequence, DvtkData.Dimse.SequenceItem dvtkDataSequenceItem): base(tagSequence, dvtkDataSequenceItem)
		{
		}



		//
		// - Properties -
		//

		/// <summary>
		/// The encapsulated DvtkData sequence item.
		/// </summary>
		internal DvtkData.Dimse.SequenceItem DvtkDataSequenceItem
		{
			get
			{
				Debug.Assert(this.dvtkDataAttributeSet is DvtkData.Dimse.SequenceItem, "Object of type DvtkData.Dimse.SequenceItem expected.");

				return(this.dvtkDataAttributeSet as DvtkData.Dimse.SequenceItem);
			}		
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Clone this SequenceItem using deep copy.
		/// </summary>
		/// <returns>The cloned SequenceItem.</returns>
		public SequenceItem Clone()
		{
			SequenceItem cloneSequenceItem = new SequenceItem();

			for (int index = 0; index < this.Count; index++)
			{
				// The Add method will take care of the deep copy.
				cloneSequenceItem.Add(this[index]);
			}

			return(cloneSequenceItem);
		}

		/// <summary>
		/// Clear all attributes from this SequenceItem and clone all attributes
		/// from the supplied SequenceItem.
		/// </summary>
		/// <param name="sequenceItem">The SequenceItem to clone from.</param>
		public void CloneFrom(SequenceItem sequenceItem)
		{
			Clear();

			for (int index = 0; index < sequenceItem.Count; index++)
			{
				// The Add method will take care of the deep copy.
				Add(sequenceItem[index]);
			}
		}

        /// <summary>
        /// Adds a single attribute with the tag, VR and value specified.
        /// </summary>
        /// <remarks>
        /// Only use this method for setting an attribute with VR OB, OF or OW.
        /// <br></br>br>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag of the attribute.</param>
        /// <param name="vR">The VR (may only be OB, OF or OW) of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// dvtkDataTag is not valid for setting a DataSet attribute.<br></br>
        /// -or-<br></br>
        /// vR is unequal to OB, OF or OW.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is a null reference.
        /// </exception>
        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Byte[] value)
        {
            TagSequence internalTagSequence = new TagSequence();

            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


            //
            // Sanity checks.
            //

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            // Check the supplied VR.

            if ((vR != VR.OB) && (vR != VR.OF) && (vR != VR.OW))
            {
                throw new ArgumentException("Supplied VR is " + vR.ToString() + ". VR may only be OB, OF or OW.", "vR");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, value);
        }

        /// <summary>
        /// Adds a single attribute with the tag, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead supply
        /// multiple values arguments for this method when adding a single attribute with multiple values.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, params Object[] values)
        {
            TagSequence internalTagSequence = new TagSequence();

            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


            //
            // Sanity checks.
            //

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
        }

        /// <summary>
        /// Adds a single attribute with the tag, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">The values, which will be copied from another attribute, for this attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Values values)
        {
            TagSequence internalTagSequence = new TagSequence();

            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


            //
            // Sanity checks.
            //

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
        }

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and value specified.
        /// </summary>
        /// <remarks>
        /// Only use this method for setting an attribute with VR OB, OF or OW.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence of the attribute.</param>
        /// <param name="vR">The VR (may only be OB, OF or OW) of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="vR"/> is unequal to OB, OF or OW.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is a null reference.
        /// </exception>
        public override void Set(String tagSequence, VR vR, Byte[] value)
        {
            TagSequence internalTagSequence = new TagSequence(tagSequence);


            //
            // Sanity checks.
            //

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
            }

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            // Check the supplied VR.

            if ((vR != VR.OB) && (vR != VR.OF) && (vR != VR.OW))
            {
                throw new ArgumentException("Supplied VR is " + vR.ToString() + ". VR may only be OB, OF or OW.", "vR");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, value);
        }

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead supply
        /// multiple values arguments for this method when adding a single attribute with multiple values.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(String tagSequence, VR vR, params Object[] values)
        {
            TagSequence internalTagSequence = new TagSequence(tagSequence);


            //
            // Sanity checks.
            //

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
            }

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("parameters");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
        }

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">The values, which will be copied from another attribute, for this attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(String tagSequence, VR vR, Values values)
        {
            TagSequence internalTagSequence = new TagSequence(tagSequence);


            //
            // Sanity checks.
            //

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
            }

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
        }
	}
}
