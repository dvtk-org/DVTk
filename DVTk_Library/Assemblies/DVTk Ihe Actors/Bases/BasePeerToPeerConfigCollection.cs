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
using System.Xml;

using Dvtk.IheActors.Bases;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for BasePeerToPeerConfigCollection.
	/// </summary>
	public class BasePeerToPeerConfigCollection : CollectionBase
	{
		/// <summary>
		/// Class Constructor.
		/// </summary>
		public BasePeerToPeerConfigCollection() {}

		/// <summary>
		/// Gets or sets an <see cref="BasePeerToPeerConfig"/> from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the collection member to get or set.</param>
		/// <value>The <see cref="BasePeerToPeerConfig"/> at the specified index.</value>
		public BasePeerToPeerConfig this[int index]  
		{
			get  
			{
				return ((BasePeerToPeerConfig) List[index]);
			}
			set  
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Adds an object to the end of the <see cref="BasePeerToPeerConfigCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="BasePeerToPeerConfig"/> to be added to the end of the <see cref="BasePeerToPeerConfigCollection"/>.</param>
		/// <returns>The <see cref="BasePeerToPeerConfigCollection"/> index at which the value has been added.</returns>
		public int Add(BasePeerToPeerConfig value)  
		{
			return (List.Add(value));
		}

		/// <summary>
		/// Searches for the specified <see cref="BasePeerToPeerConfig"/> and 
		/// returns the zero-based index of the first occurrence within the entire <see cref="BasePeerToPeerConfigCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="BasePeerToPeerConfig"/> to locate in the <see cref="BasePeerToPeerConfigCollection"/>.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of value within the entire <see cref="BasePeerToPeerConfigCollection"/>, 
		/// if found; otherwise, -1.
		/// </returns>
		public int IndexOf(BasePeerToPeerConfig value)  
		{
			return (List.IndexOf(value));
		}

		/// <summary>
		/// Inserts an <see cref="BasePeerToPeerConfig"/> element into the <see cref="BasePeerToPeerConfigCollection"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="value">The <see cref="BasePeerToPeerConfig"/> to insert.</param>
		public void Insert(int index, BasePeerToPeerConfig value)  
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="BasePeerToPeerConfig"/> from the <see cref="BasePeerToPeerConfigCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="BasePeerToPeerConfig"/> to remove from the <see cref="BasePeerToPeerConfigCollection"/>.</param>
		public void Remove(BasePeerToPeerConfig value)  
		{
			List.Remove(value);
		}

		/// <summary>
		/// Determines whether the <see cref="BasePeerToPeerConfigCollection"/> contains a specific element.
		/// </summary>
		/// <param name="value">The <see cref="BasePeerToPeerConfig"/> to locate in the <see cref="BasePeerToPeerConfigCollection"/>.</param>
		/// <returns>
		/// <c>true</c> if the <see cref="BasePeerToPeerConfigCollection"/> contains the specified value; 
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(BasePeerToPeerConfig value)  
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
			if (!(value is BasePeerToPeerConfig))
				throw new ArgumentException("value must be of type BasePeerToPeerConfig.", "value");
		}

		/// <summary>
		/// Performs additional custom processes when removing an element from the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found.</param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemove(int index, Object value)  
		{
			if (!(value is BasePeerToPeerConfig))
				throw new ArgumentException("value must be of type BasePeerToPeerConfig.", "value");
		}

		/// <summary>
		/// Performs additional custom processes before setting a value in the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found.</param>
		/// <param name="oldValue">The value to replace with newValue.</param>
		/// <param name="newValue">The new value of the element at index.</param>
		protected override void OnSet(int index, Object oldValue, Object newValue)  
		{
			if (!(newValue is BasePeerToPeerConfig))
				throw new ArgumentException("newValue must be of type BasePeerToPeerConfig.", "newValue");
		}

		/// <summary>
		/// Performs additional custom processes when validating a value.
		/// </summary>
		/// <param name="value">The object to validate.</param>
		protected override void OnValidate(Object value)  
		{
			if (!(value is BasePeerToPeerConfig))
				throw new ArgumentException("value must be of type BasePeerToPeerConfig.");
		}
	
		/// <summary>
		/// Copies the elements of the <see cref="ICollection"/> to a strong-typed <c>BasePeerToPeerConfig[]</c>, 
		/// starting at a particular <c>BasePeerToPeerConfig[]</c> index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional <c>BasePeerToPeerConfig[]</c> that is the destination of the elements 
		/// copied from <see cref="ICollection"/>.
		/// The <c>BasePeerToPeerConfig[]</c> must have zero-based indexing. 
		/// </param>
		/// <param name="index">
		/// The zero-based index in array at which copying begins.
		/// </param>
		/// <remarks>
		/// Provides the strongly typed member for <see cref="ICollection"/>.
		/// </remarks>
		public void CopyTo(BasePeerToPeerConfig[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// Write Configuration data in XML format to the XML Text Writer.
		/// </summary>
		/// <param name="writer">XML Text Writer</param>
		public void WriteXmlConfig(XmlTextWriter writer)
		{
			foreach(BasePeerToPeerConfig basePeerToPeerConfig in this)
			{
				basePeerToPeerConfig.WriteXmlConfig(writer);
			}
		}
	}
}
