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



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// Represents a DICOM attribute.
	/// </summary>
	abstract public class Attribute
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property TagSequence.
		/// </summary>
		private TagSequence tagSequence = null;



		//
		// - Properties -
		//

		/// <summary>
		/// Gets the element number of the attribute tag.
		/// </summary>
		public UInt16 ElementNumber
		{
            get
            {
                Tag lastTag = TagSequence.Tags[TagSequence.Tags.Count - 1] as Tag;

                return (lastTag.ElementNumber);
            }
        }

		/// <summary>
		/// Indicates if the attribute exists.
		/// </summary>
		public abstract bool Exists
		{
			get;
		}

		/// <summary>
		/// Gets the group number of the attribute tag.
		/// </summary>
		public UInt16 GroupNumber
		{
            get
            {
                Tag lastTag = TagSequence.Tags[TagSequence.Tags.Count - 1] as Tag;

                return (lastTag.GroupNumber);
            }
		}

		/// <summary>
		/// Gets the number of items of the attribute.
		/// </summary>
		/// <remarks>
		/// This property is only meaningfull if this instance has a VR SQ.
		/// </remarks>
		public abstract int ItemCount
		{
			get;
		}

		/// <summary>
		/// Gets or sets the name of the attribute.
		/// </summary>
		/// <remarks>
		/// An empty string means no name available.
		/// </remarks>
		public abstract String Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the tag sequence for the attribute.
		/// </summary>
		internal TagSequence TagSequence
		{
			get
			{
				return this.tagSequence;
			}
			set
			{
				this.tagSequence = value;
			}
		}

		/// <summary>
		/// Gets the tag sequence string for the attribute.
		/// </summary>
		public String TagSequenceString
		{
			get
			{
				return this.tagSequence.ToString();
			}
		}

		/// <summary>
		/// Gets the values of the attribute.
		/// </summary>
		/// <remarks>
		/// This property is only meaningfull if this instance does not have a VR SQ, OB, OW, OF or UN.
		/// </remarks>
		public abstract Values Values
		{
			get;
		}

		/// <summary>
		/// Gets the value multiplicity of the attribute.
		/// </summary>
		public abstract int VM
		{
			get;
		}

		/// <summary>
		/// Gets the VR of the attribute.
		/// </summary>
		public abstract VR VR
		{
			get;
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
		public abstract void AddItem(SequenceItem item);

        /// <summary>
        /// Clears all sequence items present in the attribute.
        /// </summary>
        /// <remarks>
        /// This method is only meaningfull if this instance has a VR SQ.
        /// </remarks>
		public abstract void ClearItems();

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <param name="parentAttributeSetToCloneTo">
        /// The AttributeSet the new cloned Attribute wil become part of.
        /// </param>
        /// <returns>The created deep copy of this instance.</returns>
        internal abstract Attribute Clone(AttributeSet parentAttributeSetToCloneTo);

		/// <summary>
		/// Deletes this instance from the AttributeSet it is contained in.
		/// </summary>
		/// <remarks>
		/// <b>Do not use this instance anymore after calling this method!</b>
		/// </remarks>
		public abstract void Delete();

		/// <summary>
		/// Gets a sequence item.
		/// </summary>
		/// <remarks>
		/// This method is only meaningfull if this instance has a VR SQ.
		/// </remarks>
		/// <param name="oneBasedIndex">The one based index.</param>
		/// <returns>The sequence item.</returns>
		public abstract SequenceItem GetItem(int oneBasedIndex);

        /// <summary>
        /// Insert a Sequence Item at a specified position.
        /// </summary>
        /// <param name="oneBasedIndex">The one based index.</param>
        /// <param name="item">The Sequence item to insert.</param>
        public abstract void InsertItem(int oneBasedIndex, SequenceItem item);

		/// <summary>
		/// Determines if a VR is a "simple" one, i.e. is not SQ, OB, OW, OF or UN.
		/// </summary>
		/// <param name="vR">The VR.</param>
		/// <returns>Boolean indicating if it is a simple VR or not.</returns>
		internal static bool IsSimpleVR(VR vR)
		{
			bool isSimpleVR = true;

			switch(vR)
			{
				case VR.AE: // Application Entity
				case VR.AS: // Age String
				case VR.AT: // Attribute Tag
				case VR.CS: // Code String
				case VR.DA: // Date
				case VR.DS: // Decimal String
				case VR.DT: // Date Time
				case VR.FL: // Floating Point Single
				case VR.FD: // Floating Point Double
				case VR.IS: // Integer String
				case VR.LO: // Long String
				case VR.LT: // Long Text
				case VR.PN: // Person Name
				case VR.SH: // Short String
				case VR.SL: // Signed Long
				case VR.SS: // Signed Short
				case VR.ST: // Short Text
				case VR.TM: // Time
				case VR.UI: // Unique Identifier (UID)
				case VR.UL: // Unsigned Long
				case VR.US: // Unsigned Short
				case VR.UT: // Unlimited Text
                case VR.UC: // Unlimited Characters
                case VR.UR: // Universal Resource Locator
					isSimpleVR = true;
					break;

				default:
					isSimpleVR = false;
					break;
			}

			return(isSimpleVR);
		}

        /// <summary>
        /// Removes a sequence item.
        /// </summary>
        /// <param name="oneBasedIndex">The one based index</param>
		public abstract void RemoveItemAt(int oneBasedIndex);
	}
}
