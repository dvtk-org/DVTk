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
using System.Collections;



namespace DvtkHighLevelInterface.InformationModel
{
	/// <summary>
	/// Summary description for ReferencedSopItemCollection.
	/// </summary>
	public class ReferencedSopItemCollection : CollectionBase
	{
		/// <summary>
		/// Gets or sets an <see cref="ReferencedSopItem"/> from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the collection member to get or set.</param>
		/// <value>The <see cref="ReferencedSopItemCollection"/> at the specified index.</value>
		public ReferencedSopItem this[int index]  
		{
			get  
			{
				return ((ReferencedSopItem) List[index]);
			}
			set  
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Adds an object to the end of the <see cref="ReferencedSopItemCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="ReferencedSopItem"/> to be added to the end of the <see cref="ReferencedSopItemCollection"/>.</param>
		/// <returns>The <see cref="ReferencedSopItemCollection"/> index at which the value has been added.</returns>
		public int Add(ReferencedSopItem value)  
		{
			return (List.Add(value));
		}

		/// <summary>
		/// Searches for the specified <see cref="ReferencedSopItem"/> and 
		/// returns the zero-based index of the first occurrence within the entire <see cref="ReferencedSopItemCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="ReferencedSopItem"/> to locate in the <see cref="ReferencedSopItemCollection"/>.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of value within the entire <see cref="ReferencedSopItemCollection"/>, 
		/// if found; otherwise, -1.
		/// </returns>
		public int IndexOf(ReferencedSopItem value)  
		{
			return (List.IndexOf(value));
		}

		/// <summary>
		/// Inserts an <see cref="ReferencedSopItem"/> element into the <see cref="ReferencedSopItemCollection"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="value">The <see cref="ReferencedSopItem"/> to insert.</param>
		public void Insert(int index, ReferencedSopItem value)  
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="ReferencedSopItem"/> from the <see cref="ReferencedSopItemCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="ReferencedSopItem"/> to remove from the <see cref="ReferencedSopItemCollection"/>.</param>
		public void Remove(ReferencedSopItem value)  
		{
			List.Remove(value);
		}

		/// <summary>
		/// Determines whether the <see cref="ReferencedSopItemCollection"/> contains a specific element.
		/// </summary>
		/// <param name="value">The <see cref="ReferencedSopItem"/> to locate in the <see cref="ReferencedSopItemCollection"/>.</param>
		/// <returns>
		/// <c>true</c> if the <see cref="ReferencedSopItemCollection"/> contains the specified value; 
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(ReferencedSopItem value)  
		{
			// If value is not of type Code, this will return false.
			return (List.Contains(value));
		}

		/// <summary>
		/// Performs additional custom processes before inserting a new element into the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert value.</param>
		/// <param name="value">The new value of the element at index.</param>
		protected override void OnInsert(int index, Object value)  
		{
			if (!(value is ReferencedSopItem))
				throw new ArgumentException("value must be of type ReferencedSopItem.", "value");
		}

		/// <summary>
		/// Performs additional custom processes when removing an element from the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found.</param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemove(int index, Object value)  
		{
			if (!(value is ReferencedSopItem))
				throw new ArgumentException("value must be of type ReferencedSopItem.", "value");
		}

		/// <summary>
		/// Performs additional custom processes before setting a value in the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found.</param>
		/// <param name="oldValue">The value to replace with newValue.</param>
		/// <param name="newValue">The new value of the element at index.</param>
		protected override void OnSet(int index, Object oldValue, Object newValue)  
		{
			if (!(newValue is ReferencedSopItem))
				throw new ArgumentException("newValue must be of type ReferencedSopItem.", "newValue");
		}

		/// <summary>
		/// Performs additional custom processes when validating a value.
		/// </summary>
		/// <param name="value">The object to validate.</param>
		protected override void OnValidate(Object value)  
		{
			if (!(value is ReferencedSopItem))
				throw new ArgumentException("value must be of type ReferencedSopItem.");
		}
	
		/// <summary>
		/// Copies the elements of the <see cref="ICollection"/> to a strong-typed <c>ReferencedSopItem[]</c>, 
		/// starting at a particular <c>ReferencedSopItem[]</c> index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional <c>ReferencedSopItem[]</c> that is the destination of the elements 
		/// copied from <see cref="ICollection"/>.
		/// The <c>ReferencedSopItem[]</c> must have zero-based indexing. 
		/// </param>
		/// <param name="index">
		/// The zero-based index in array at which copying begins.
		/// </param>
		/// <remarks>
		/// Provides the strongly typed member for <see cref="ICollection"/>.
		/// </remarks>
		public void CopyTo(ReferencedSopItem[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// Try to find a a referenced item with the same SOP Class UID and SOP Instance UID as those given.
		/// </summary>
		/// <param name="sopClassUid">SOP Class UID to find.</param>
		/// <param name="sopInstanceUid">SOP Instance UID to find.</param>
		/// <returns>Matching referenced sop item - or null if no match.</returns>
		public ReferencedSopItem Find(System.String sopClassUid, System.String sopInstanceUid)
		{
			ReferencedSopItem referencedSopItem = null;

			foreach(ReferencedSopItem lReferencedSopItem in this)
			{
				if ((lReferencedSopItem.SopClassUid == sopClassUid) &&
					(lReferencedSopItem.SopInstanceUid == sopInstanceUid))
				{
					referencedSopItem = lReferencedSopItem;
					break;
				}
			}

			return referencedSopItem;
		}
	}
}
