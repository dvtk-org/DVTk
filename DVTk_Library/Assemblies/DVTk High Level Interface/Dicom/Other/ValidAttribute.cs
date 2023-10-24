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

using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Common.Threads;
using VR = DvtkData.Dimse.VR;
using AttributeType = DvtkData.Dimse.AttributeType;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
    /// Represents a Dicom attribute that is part of an AttributeSet.
	/// </summary>
	internal class ValidAttribute: Attribute
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property DvtkDataAttribute.
		/// </summary>
		private DvtkData.Dimse.Attribute dvtkDataAttribute = null;

		/// <summary>
		/// See property ParentAttributeSet.
		/// </summary>
		private AttributeSet parentAttributeSet = null;



		//
		// - Constructors -
		//

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ValidAttribute()
        {
            // Do nothing.
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tagSequence">The tag sequence of the attribute.</param>
		/// <param name="dvtkDataAttribute">The encapsulated DvtkData attribute.</param>
        /// <param name="parentAttributeSet">The AttributeSet this instance is part of.</param>
        public ValidAttribute(TagSequence tagSequence, DvtkData.Dimse.Attribute dvtkDataAttribute, AttributeSet parentAttributeSet)
		{
			// Sanity check.
			if (dvtkDataAttribute == null)
			{
				throw new ArgumentException("Internal error: dvtkDataAttribute may not be null.");
			}

			// Sanity check.
			if (parentAttributeSet == null)
			{
				throw new ArgumentException("Internal error: parentAttributeSet may not be null.");
			}

			TagSequence = tagSequence;
			this.dvtkDataAttribute = dvtkDataAttribute;
			this.parentAttributeSet = parentAttributeSet;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets the encapsulated DvtkData attribute.
		/// </summary>
		public DvtkData.Dimse.Attribute DvtkDataAttribute
		{
			get
			{
				return this.dvtkDataAttribute;
			}
		}

		/// <summary>
		/// Gets the encapsulated DvtkData sequence.
		/// </summary>
		/// <remarks>
		/// This property is only meaningfull if this instance has a VR SQ.
		/// </remarks>
		private DvtkData.Dimse.Sequence DvtkDataSequence
		{
			get
			{
				DvtkData.Dimse.SequenceOfItems dvtkDataSequenceValue = this.dvtkDataAttribute.DicomValue as DvtkData.Dimse.SequenceOfItems;

				// Sanity check.
				if (dvtkDataSequenceValue == null)
				{
					throw new Exception("Internal error. Expecting field this.dvtkDataAttribute.DicomValue to be of type DvtkData.Dimse.SequenceOfItems.");
				}

				return(dvtkDataSequenceValue.Sequence);
			}
		}

		/// <summary>
		/// Indicates if the attribute exists.
		/// </summary>
		public override bool Exists
		{
			get
			{
				return (true);
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
				int itemCount = 0;

				if (this.VR == VR.SQ)
				{
					itemCount = DvtkDataSequence.Count;
				}
				else
				{
					Thread.WriteWarningCurrentThread(" Getting item count attribute for attribute with tag sequence " + TagSequence.ToString() + " and VR " + this.VR.ToString() + ". Returning 0.");
					itemCount = 0;
				}
				
				return(itemCount);
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
				return(this.dvtkDataAttribute.Name);
			}
			set
			{
				this.dvtkDataAttribute.Name = value;
			}
		}


		/// <summary>
		/// Gets the AttributeSet that this instance is part of.
		/// </summary>
		public AttributeSet ParentAttributeSet
		{
			get
			{
				return this.parentAttributeSet;
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
				Values values = new Values(this);

				return(values);
			}
		}

		/// <summary>
        /// Gets the value multiplicity of the attribute.
		/// </summary>
		public override int VM
		{
			get
			{
				int vm = 0;

				if (this.VR == VR.SQ)
				{
					vm = 0;
					Thread.WriteWarningCurrentThread("Getting the VM of an attribute with tag sequence " + TagSequence.ToString() + " and VR SQ. Returning 0.");
				}
				else if (this.VR == VR.UN)
				{
					vm = 1;
					Thread.WriteWarningCurrentThread("Getting the VM of an attribute with tag sequence " + TagSequence.ToString() + " and VR UN. Returning 1.");
				}
				else
				{
					vm = Values.Count;
				}

				return(vm);
			}
		}

		/// <summary>
        /// Gets the VR of the attribute.
		/// </summary>
		public override VR VR
		{
			get
			{
				return(this.dvtkDataAttribute.ValueRepresentation);
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
			if (this.VR == VR.SQ)
			{
				SequenceItem cloneSequenceItem = item.Clone();

				DvtkDataSequence.Add(cloneSequenceItem.DvtkDataSequenceItem);
			}
			else
			{
				Thread.WriteWarningCurrentThread("Adding a sequence item to an attribute with tag sequence " + TagSequence.ToString() + " and VR " + this.VR.ToString() + ". Doing nothing.");
			}
		}

		/// <summary>
		/// Clears all sequence items present in the attribute.
		/// </summary>
		/// <remarks>
		/// This method is only meaningfull if this instance has a VR SQ.
		/// </remarks>
		public override void ClearItems()
		{
			if (this.VR == VR.SQ)
			{
				DvtkDataSequence.Clear();
			}
			else
			{
				Thread.WriteWarningCurrentThread("Clearing the sequence items of an attribute with tag sequence " + TagSequence.ToString() + " and VR " + this.VR.ToString() + ". Doing nothing.");
			}
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
			//
			// Clone the attribute without values.
			//

			TagSequence newTagSequence = parentAttributeSetToCloneTo.TagSequence.Clone();
			
			Tag lastTagCurrentAttribute = TagSequence.Tags[TagSequence.Tags.Count - 1] as Tag;
			newTagSequence.Add(lastTagCurrentAttribute);

			ValidAttribute cloneAttribute = new ValidAttribute(newTagSequence, new DvtkData.Dimse.Attribute(lastTagCurrentAttribute.AsUInt32, (DvtkData.Dimse.VR)this.VR), parentAttributeSetToCloneTo);


			//
			// Add the values or items to the cloned attribute.
			//

			Values currentValues = this.Values;
			Values cloneValues = cloneAttribute.Values;
			
			if (currentValues.IsImplementedWithCollection)
			{
				Object[] collectionAsArray = new Object[currentValues.CollectionImplementation.Count];

				currentValues.CollectionImplementation.CopyTo(collectionAsArray, 0);

				cloneValues.Add(collectionAsArray);
			}
			else if (currentValues.IsImplementedWithString)
			{
				cloneValues.Add(currentValues.StringImplementation);
			}
			else if ((currentValues.Attribute.VR == VR.OB) || (currentValues.Attribute.VR == VR.OF) || (currentValues.Attribute.VR == VR.OW))
			{
				cloneValues.Add(currentValues);
			}
			else if (this.VR == VR.UN)
			{
				cloneValues.ByteArrayImplementation = currentValues.ByteArrayImplementation;
			}
			else if (this.VR == VR.SQ)
			{
				for (int index = 1; index <= this.ItemCount; index++)
				{
					// The AddItem will take care that the item is cloned.
					cloneAttribute.AddItem(this.GetItem(index));
				}
			}
			else
			{
				// Do nothing.
			}


			//
			// Set the Name of the attribute.
			//

			cloneAttribute.Name = Name;

			return(cloneAttribute);
		}

		/// <summary>
		/// Deletes this instance from the AttributeSet it is contained in.
		/// </summary>
		/// <remarks>
		/// <b>Don't use this instance anymore after calling this method!</b>
		/// </remarks>
		public override void Delete()
		{
			if (this.parentAttributeSet != null)
			{
				this.parentAttributeSet.DvtkDataAttributeSet.Remove(this.dvtkDataAttribute);
				TagSequence = new TagSequence();
				this.parentAttributeSet = null;
			}
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
			SequenceItem sequenceItem = null;

			if (this.VR == VR.SQ)
			{
				if ((oneBasedIndex >= 1) && (oneBasedIndex <= ItemCount))
				{
					TagSequence sequenceItemTagSequence = TagSequence.Clone();
					Tag lastTag = sequenceItemTagSequence.Tags[sequenceItemTagSequence.Tags.Count - 1] as Tag;
					lastTag.IndexNumber = oneBasedIndex;

					sequenceItem = new SequenceItem(sequenceItemTagSequence, DvtkDataSequence[oneBasedIndex - 1]);
				}
				else
				{
					sequenceItem = new SequenceItem();
					Thread.WriteWarningCurrentThread("Getting sequence item " + oneBasedIndex.ToString() + " from attribute with tag sequence " + TagSequence.ToString() + " containing " + ItemCount.ToString() + " items. Returning an empty sequence item.");
				}
			}
			else
			{
				sequenceItem = new SequenceItem();
				Thread.WriteWarningCurrentThread("Getting a sequence item from attribute with tag sequence " + TagSequence.ToString() + " and VR " + this.VR.ToString() + ". Returning an empty sequence item.");
			}
			
			return sequenceItem;
		}

        /// <summary>
        /// Insert a Sequence Item at a specified position.
        /// </summary>
        /// <remarks>
        /// This method is only meaningfull if this instance has a VR SQ. The inserted Sequence
        /// Item will get item number oneBasedIndex.
        /// </remarks>
        /// <param name="oneBasedIndex">The one based index of the position to insert.</param>
        /// <param name="item">The Sequence item to insert.</param>
        public override void InsertItem(int oneBasedIndex, SequenceItem item)
        {
            if (this.VR == VR.SQ)
            {
                if ((oneBasedIndex >= 1) && (oneBasedIndex <= (ItemCount + 1)))
                {
                    SequenceItem cloneSequenceItem = item.Clone();

                    DvtkDataSequence.Insert(oneBasedIndex - 1, cloneSequenceItem.DvtkDataSequenceItem);
                }
                else
                {
                    Thread.WriteWarningCurrentThread("Inserting sequence item at one based position " + oneBasedIndex.ToString() + " fom attribute with tag sequence " + TagSequence.ToString() + " containing " + ItemCount.ToString() + " items. Doing nothing.");
                }
            }
            else
            {
                Thread.WriteWarningCurrentThread("Adding a sequence item to an attribute with tag sequence " + TagSequence.ToString() + " and VR " + this.VR.ToString() + ". Doing nothing.");
            }
        }

        /// <summary>
        /// Removes a sequence item.
        /// </summary>
        /// <param name="oneBasedIndex">The one based index</param>
		public override void RemoveItemAt(int oneBasedIndex)
		{
			if (this.VR == VR.SQ)
			{
				if ((oneBasedIndex >= 1) && (oneBasedIndex <= ItemCount))
				{
					DvtkDataSequence.RemoveAt(oneBasedIndex - 1);
				}
				else
				{
					Thread.WriteWarningCurrentThread("Removing sequence item " + oneBasedIndex.ToString() + " from attribute with tag sequence " + TagSequence.ToString() + " containing " + ItemCount.ToString() + " items. Doing nothing.");
				}
			}
			else
			{
				Thread.WriteWarningCurrentThread("Removing a sequence item from attribute with tag sequence " + TagSequence.ToString() + " and VR " + this.VR.ToString() + ". Doing nothing.");
			}
		}
	}
}
