// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright � 2009 DVTk
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



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// Collection of (possible unrelated) Attributes.
	/// </summary>
	public sealed class AttributeCollection: DvtkData.Collections.NullSafeCollectionBase
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public AttributeCollection()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor. 
        /// 
        /// Shallow copy.
		/// </summary>
		/// <param name="arrayOfValues">Values to copy.</param>
		public AttributeCollection(Attribute[] arrayOfValues)
		{
			if (arrayOfValues == null)
			{
				throw new ArgumentNullException();
			}

			foreach (Attribute value in arrayOfValues)
			{
				this.Add(value);
			}
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets or sets the item at the specified index.
		/// </summary>
		/// <value>The item at the specified <c>index</c>.</value>
		public new Attribute this[int index]
		{
			get 
			{
				return (Attribute)base[index];
			}
			set
			{
				base.Insert(index, value);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Adds an item to the <see cref="System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(Attribute value)
		{
			return base.Add(value);
		}

        /// <summary>
        /// Adds another collection to this collection.
        /// </summary>
        /// <param name="attributeCollection">The other collection to add.</param>
		public void AddRange(AttributeCollection attributeCollection)
		{
			foreach(Attribute attribute in attributeCollection)
			{
				Add(attribute);
			}
		}

		/// <summary>
		/// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
		public bool Contains(Attribute value)
		{
			return base.Contains(value);
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
		public int IndexOf(Attribute value)
		{
			return base.IndexOf(value);
		}

		/// <summary>
		/// Inserts an item to the IList at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
		/// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
		public void Insert(int index, Attribute value)
		{
			base.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific item from the IList.
		/// </summary>
		/// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
		public void Remove(Attribute value)
		{
			base.Remove(value);
		}
	}
}
