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

using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
    /// Represents a non existing Dicom attribute.
	/// </summary>
	internal class InvalidAttribute: Attribute
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public InvalidAttribute()
		{
			TagSequence = new TagSequence("0x00000000");
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tagSequence">Tag sequence used to retrieve the attribute.</param>
		public InvalidAttribute(TagSequence tagSequence)
		{
			TagSequence = tagSequence;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Indicates if the attribute exists.
		/// </summary>
		public override bool Exists
		{
			get
			{
				return (false);
			}
		}

		/// <summary>
		/// Gets the number of items of the attribute.
		/// </summary>
		/// <remarks>
		/// This property is only meaningfull if this instance has a VR SQ.
		/// </remarks>
		public override int ItemCount
		{
			get
			{
				Thread.WriteWarningCurrentThread("Getting the item count for non existing attribute \"" + TagSequenceString + "\". Returning 0.");
				return(0);
			}
		}

		/// <summary>
		/// Gets or sets the name of the attribute.
		/// </summary>
		/// <remarks>
		/// An empty string means no name available.
		/// </remarks>
		public override String Name
		{
			get
			{
				Thread.WriteWarningCurrentThread("Getting the name for non existing attribute \"" + TagSequenceString + "\". Returning the empty string.");
				return("");
			}
			set
			{
                Thread.WriteWarningCurrentThread("Setting the name for non existing attribute \"" + TagSequenceString + "\".");
			}
		}

		/// <summary>
		/// Gets the values of the attribute.
		/// </summary>
		/// <remarks>
		/// This property is only meaningfull if this instance does not have a VR SQ, OB, OW, OF or UN.
		/// </remarks>
		public override Values Values
		{
			get
			{
                Thread.WriteWarningCurrentThread("Getting the values for non existing attribute \"" + TagSequenceString + "\". Values added to this instance will not be stored.");
				return(new Values(this));
			}
		}

		/// <summary>
        /// Gets the value multiplicity of the attribute.
		/// </summary>
		public override int VM
		{
			get
			{
				Thread.WriteWarningCurrentThread("Getting the VM for non existing attribute \"" + TagSequenceString + "\". Returning 0.");
				return(0);
			}
		}

		/// <summary>
        /// Gets the VR of the attribute.
		/// </summary>
		public override VR VR
		{
			get
			{
				Thread.WriteWarningCurrentThread("Getting the VR for non existing attribute \"" + TagSequenceString + "\". Returning VR UN.");
				return(VR.UN);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Adds a sequence item to the end of the item list.
		/// </summary>
		/// <remarks>
		/// This method is only meaningfull if this instance has a VR SQ.
		/// </remarks>
        /// <param name="item">The sequence item to add.</param>
		public override void AddItem(SequenceItem item)
		{
			Thread.WriteWarningCurrentThread("Adding a sequence item to non existing attribute \"" + TagSequenceString + "\". Doing nothing.");
		}

		/// <summary>
		/// Clears all sequence items present in the attribute.
		/// </summary>
		/// <remarks>
		/// This method is only meaningfull if this instance has a VR SQ.
		/// </remarks>
		public override void ClearItems()
		{
			Thread.WriteWarningCurrentThread("Clearing the sequence items of non existing attribute \"" + TagSequenceString + "\". Doing nothing.");
			// Do nothing.
		}

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <param name="parentAttributeSetToCloneTo">
        /// The AttributeSet the new cloned Attribute wil become part of.
        /// </param>
        /// <returns>The created deep copy of this instance.</returns>
        internal override Attribute Clone(AttributeSet parentAttributeSetToCloneTo)
        {
            InvalidAttribute cloneInvalidAttribute = new InvalidAttribute();

            return (cloneInvalidAttribute);
        }

		/// <summary>
		/// Deletes this instance from the AttributeSet it is contained in.
		/// </summary>
		/// <remarks>
		/// <b>Don't use this instance anymore after calling this method!</b>
		/// </remarks>
		public override void Delete()
		{
			Thread.WriteWarningCurrentThread("Deleting non existing attribute \"" + TagSequenceString + "\". Doing nothing.");
		}

		/// <summary>
		/// Gets a sequence item.
		/// </summary>
		/// <remarks>
		/// This method is only meaningfull if this instance has a VR SQ.
		/// </remarks>
		/// <param name="oneBasedIndex">The one based index.</param>
		/// <returns>The sequence item.</returns>
        public override SequenceItem GetItem(int oneBasedIndex)
		{
			Thread.WriteWarningCurrentThread("Getting a sequence item of non existing attribute \"" + TagSequenceString + "\". Returning an emptry sequence item.");
			return(new SequenceItem());
		}

        /// <summary>
        /// Insert a Sequence Item at a specified position.
        /// </summary>
        /// <param name="oneBasedIndex">The one based index.</param>
        /// <param name="item">The Sequence item to insert.</param>
        public override void InsertItem(int oneBasedIndex, SequenceItem item)
        {
            Thread.WriteWarningCurrentThread("Inserting a sequence item to non existing attribute \"" + TagSequenceString + "\". Doing nothing.");
        }

        /// <summary>
        /// Removes a sequence item.
        /// </summary>
        /// <param name="oneBasedIndex">The one based index</param>
		public override void RemoveItemAt(int oneBasedIndex)
		{
			Thread.WriteWarningCurrentThread("Removing an sequence item for non existing attribute \"" + TagSequenceString + "\". Doing nothing.");
			// Do nothing.
		}
	}
}
