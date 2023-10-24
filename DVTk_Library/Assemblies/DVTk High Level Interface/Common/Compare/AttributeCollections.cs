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

using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Other;
using Dvtk.Hl7.Messages;



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// Summary description for AttributeCollections.
	/// </summary>
	public class AttributeCollections
	{
		//
		// - Fields -
		//

		/// <summary>
		/// The internal hidden non-type-safe list of AttributeCollections implemented as an ArrayList.
		/// </summary>
		internal ArrayList attributeCollections = new ArrayList();



		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor to create an empty instances.
		/// </summary>
		public AttributeCollections()
		{
			// Do nothing.
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Number of AttributeCollections.
		/// </summary>
		public int Count
		{
			get
			{
				return(this.attributeCollections.Count);
			}
		}

		/// <summary>
		/// Property to get a specific AttributeCollection.
		/// </summary>
		internal AttributeCollectionBase this[int zeroBasedIndex]
		{
			get
			{
				if ((zeroBasedIndex < 0) || (zeroBasedIndex >= Count))
				{
					throw new HliException("Wrong index used for AttributeCollection.");
				}
				
				return(this.attributeCollections[zeroBasedIndex] as AttributeCollectionBase);
			}
		}



		//
		// - Methods -
		//

		internal void Add(AttributeCollectionBase attributeCollection)
		{
			this.attributeCollections.Add(attributeCollection);
		}

        /// <summary>
        /// Clear this collection.
        /// </summary>
		public void Clear()
		{
			this.attributeCollections.Clear();
		}

        /// <summary>
        /// Add an AttributeSet instance to this collection.
        /// </summary>
        /// <param name="attributeSet">The attribute set.</param>
		public void Add(AttributeSet attributeSet)
		{
			DicomAttributeCollection dicomAttributeCollection = new DicomAttributeCollection(attributeSet);

			Add(dicomAttributeCollection);
		}

        /// <summary>
        /// Add an AttributeSet instance with specified flags to this collection.
        /// </summary>
        /// <param name="attributeSet">The attribute set.</param>
        /// <param name="flags">The flags.</param>
		public void Add(AttributeSet attributeSet, FlagsDicomAttribute flags)
		{
			DicomAttributeCollection dicomAttributeCollection = new DicomAttributeCollection(attributeSet, flags);

			Add(dicomAttributeCollection);
		}

        /// <summary>
        /// Add a non-existing attribute colection.
        /// </summary>
		public void AddNull()
		{
			this.attributeCollections.Add(null);
		}

        /// <summary>
        /// Add a HL7 message to this collection.
        /// </summary>
        /// <param name="hl7Message">The HL7 message.</param>
		public void Add(Hl7Message hl7Message)
		{
			Hl7AttributeCollection hl7AttributeCollection = new Hl7AttributeCollection(hl7Message);

			Add(hl7AttributeCollection);
		}

        /// <summary>
        /// Add an HL7 message with specified flags to this collection.
        /// </summary>
        /// <param name="hl7Message">The HL7 message.</param>
        /// <param name="flags">The flags.</param>
		public void Add(Hl7Message hl7Message, FlagsHl7Attribute flags)
		{
			Hl7AttributeCollection hl7AttributeCollection = new Hl7AttributeCollection(hl7Message, flags);

			Add(hl7AttributeCollection);
		}
		
        /// <summary>
        /// Make all DICOM attributes within a DICOM attribute sets ascending.
        /// </summary>
		public void DicomMakeAscending()
		{
			for (int index = 0; index < Count; index++)
			{
				AttributeCollectionBase attributeCollection = this[index] as DicomAttributeCollection;

				if (attributeCollection != null)
				{
					if (attributeCollection is DicomAttributeCollection)
					{
						(attributeCollection as DicomAttributeCollection).AttributeSetOnly.MakeAscending(true);
					}
				}
			}
		}

        /// <summary>
        /// Remove an attribute collection at a specified indix.
        /// </summary>
        /// <param name="index">The index.</param>
		internal void RemoveAt(int index)
		{
			this.attributeCollections.RemoveAt(index);
		}

		internal AttributeCollections Clone()
		{
			AttributeCollections clonedAttributeCollections = new AttributeCollections();

			clonedAttributeCollections.attributeCollections = this.attributeCollections.Clone() as ArrayList;

			return(clonedAttributeCollections);
		}

        /// <summary>
        /// Indicates if the attribute collection at the spefified index is a HL7 message.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Boolean indicating if the attribute collection at the spefified index is a HL7 message.</returns>
		public bool IsHl7Message(int index)
		{
			return(this[index] is Hl7AttributeCollection);
		}

        /// <summary>
        /// Returns a boolean given an index indicating if the !!!
        /// </summary>
		public bool IsAttributeSet(int index)
		{
			return(this[index] is DicomAttributeCollection);
		}

        /// <summary>
        /// Gets a boolean indicating a attribute collection is present at a specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A boolean indicating a attribute collection is present at a specified index.</returns>
		public bool IsNull(int index)
		{
			return(this[index] == null);
		}

        /// <summary>
        /// Get a HL7 message at a specific index.
        /// </summary>
        /// <param name="index">The index/</param>
        /// <returns>The HL7 message at a specific index.</returns>
		public Hl7Message GetHl7Message(int index)
		{
			return((this[index] as Hl7AttributeCollection).Hl7MessageOnly);
		}

        /// <summary>
        /// Gets an attribute set at a specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The attribute set at a specified index.</returns>
		public AttributeSet GetAttributeSet(int index)
		{
			return((this[index] as DicomAttributeCollection).AttributeSetOnly);
		}

        /// <summary>
        /// Gets the number of actual attribute sets, omitting the null (not present) attributesets.
        /// </summary>
		public int NotNullCount
		{
			get
			{
				int notNullCount = 0;

				for(int index = 0; index < this.attributeCollections.Count; index++)
				{
					if (this.attributeCollections[index] != null)
					{
						notNullCount++;
					}
				}

				return(notNullCount);
			}
		}
	}
}
