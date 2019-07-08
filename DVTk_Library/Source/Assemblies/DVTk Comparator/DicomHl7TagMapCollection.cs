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

using DvtkData.Dimse;
using Dvtk.Hl7;

namespace Dvtk.Comparator
{
	/// <summary>
	/// Summary description for DicomHl7TagMapCollection.
	/// </summary>
	public class DicomHl7TagMapCollection : CollectionBase
	{
		/// <summary>
		/// Gets or sets an <see cref="DicomHl7TagMap"/> from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the collection member to get or set.</param>
		/// <value>The <see cref="DicomHl7TagMapCollection"/> at the specified index.</value>
		public DicomHl7TagMap this[int index]  
		{
			get  
			{
				return ((DicomHl7TagMap) List[index]);
			}
			set  
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Adds an object to the end of the <see cref="DicomHl7TagMapCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="DicomHl7TagMap"/> to be added to the end of the <see cref="DicomHl7TagMapCollection"/>.</param>
		/// <returns>The <see cref="DicomHl7TagMapCollection"/> index at which the value has been added.</returns>
		public int Add(DicomHl7TagMap value)  
		{
			return (List.Add(value));
		}

		/// <summary>
		/// Searches for the specified <see cref="DicomHl7TagMap"/> and 
		/// returns the zero-based index of the first occurrence within the entire <see cref="DicomHl7TagMapCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="DicomHl7TagMap"/> to locate in the <see cref="DicomHl7TagMapCollection"/>.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of value within the entire <see cref="DicomHl7TagMapCollection"/>, 
		/// if found; otherwise, -1.
		/// </returns>
		public int IndexOf(DicomHl7TagMap value)  
		{
			return (List.IndexOf(value));
		}

		/// <summary>
		/// Inserts an <see cref="DicomHl7TagMap"/> element into the <see cref="DicomHl7TagMap"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="value">The <see cref="DicomHl7TagMapCollection"/> to insert.</param>
		public void Insert(int index, DicomHl7TagMap value)  
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="DicomHl7TagMap"/> from the <see cref="DicomHl7TagMapCollection"/>.
		/// </summary>
		/// <param name="value">The <see cref="DicomHl7TagMap"/> to remove from the <see cref="DicomHl7TagMapCollection"/>.</param>
		public void Remove(DicomHl7TagMap value)  
		{
			List.Remove(value);
		}

		/// <summary>
		/// Determines whether the <see cref="DicomHl7TagMapCollection"/> contains a specific element.
		/// </summary>
		/// <param name="value">The <see cref="DicomHl7TagMap"/> to locate in the <see cref="DicomHl7TagMapCollection"/>.</param>
		/// <returns>
		/// <c>true</c> if the <see cref="DicomHl7TagMapCollection"/> contains the specified value; 
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(DicomHl7TagMap value)  
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
			if (!(value is DicomHl7TagMap))
				throw new ArgumentException("value must be of type DicomHl7TagMap.", "value");
		}

		/// <summary>
		/// Performs additional custom processes when removing an element from the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found.</param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemove(int index, Object value)  
		{
			if (!(value is DicomHl7TagMap))
				throw new ArgumentException("value must be of type DicomHl7TagMap.", "value");
		}

		/// <summary>
		/// Performs additional custom processes before setting a value in the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found.</param>
		/// <param name="oldValue">The value to replace with newValue.</param>
		/// <param name="newValue">The new value of the element at index.</param>
		protected override void OnSet(int index, Object oldValue, Object newValue)  
		{
			if (!(newValue is DicomHl7TagMap))
				throw new ArgumentException("newValue must be of type DicomHl7TagMap.", "newValue");
		}

		/// <summary>
		/// Performs additional custom processes when validating a value.
		/// </summary>
		/// <param name="value">The object to validate.</param>
		protected override void OnValidate(Object value)  
		{
			if (!(value is DicomHl7TagMap))
				throw new ArgumentException("value must be of type DicomHl7TagMap.");
		}
	
		/// <summary>
		/// Copies the elements of the <see cref="ICollection"/> to a strong-typed <c>DicomHl7TagMap[]</c>, 
		/// starting at a particular <c>DicomHl7TagMap[]</c> index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional <c>DicomHl7TagMap[]</c> that is the destination of the elements 
		/// copied from <see cref="ICollection"/>.
		/// The <c>DicomHl7TagMap[]</c> must have zero-based indexing. 
		/// </param>
		/// <param name="index">
		/// The zero-based index in array at which copying begins.
		/// </param>
		/// <remarks>
		/// Provides the strongly typed member for <see cref="ICollection"/>.
		/// </remarks>
		public void CopyTo(DicomHl7TagMap[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}	

		/// <summary>
		/// Try to find an HL7 tag in the collection using the DICOM Tag as index.
		/// </summary>
		/// <param name="dicomTag">DICOM Tag used as index.</param>
		/// <returns>Hl7Tag - null if no match found</returns>
		public Hl7Tag Find(DvtkData.Dimse.Tag dicomTag)
		{
			Hl7Tag hl7Tag = null;

			foreach(DicomHl7TagMap lDicomHl7TagMap in this)
			{
				if (lDicomHl7TagMap.DicomTag == dicomTag)
				{
					hl7Tag = lDicomHl7TagMap.Hl7Tag;
					break;
				}
			}

			return hl7Tag;
		}

		/// <summary>
		/// Try to find a DICOM tag in the collection using the HL7 Tag as index.
		/// </summary>
		/// <param name="hl7Tag">HL7 Tag used as index.</param>
		/// <returns>DicomTag - null if no match found</returns>
		public DvtkData.Dimse.Tag Find(Hl7Tag hl7Tag)
		{
			DvtkData.Dimse.Tag dicomTag = null;

			foreach(DicomHl7TagMap lDicomHl7TagMap in this)
			{
				if (lDicomHl7TagMap.Hl7Tag == hl7Tag)
				{
					dicomTag = lDicomHl7TagMap.DicomTag;
					break;
				}
			}

			return dicomTag;
		}
	}
}
