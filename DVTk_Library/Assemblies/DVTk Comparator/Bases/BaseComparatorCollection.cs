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

using Dvtk.Results;
using DvtkData.Dimse;
using Dvtk.Hl7;

namespace Dvtk.Comparator
{
    /// <summary>
    /// Summary description for BaseComparatorCollection.
    /// </summary>
    public class BaseComparatorCollection : CollectionBase
    {
        private TagValueCollection _tagValueFilterCollection = new TagValueCollection();

        /// <summary>
        /// Class constructor
        /// </summary>
        public BaseComparatorCollection() { }

        /// <summary>
        /// Add a Tag Value filter for the comparator.
        /// Only compare messages which contain the same values for this filter.
        /// </summary>
        /// <param name="tagValueFilter">Tag Value Filter.</param>
        public void AddComparisonTagValueFilter(DicomTagValue tagValueFilter)
        {
            _tagValueFilterCollection.Add(tagValueFilter);
        }

        public TagValueCollection TagValueCollection
        {
            get
            {
                return _tagValueFilterCollection;
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="BaseComparator"/> from the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the collection member to get or set.</param>
        /// <value>The <see cref="BaseComparatorCollection"/> at the specified index.</value>
        public BaseComparator this[int index]
        {
            get
            {
                return ((BaseComparator)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="BaseComparatorCollection"/>.
        /// </summary>
        /// <param name="value">The <see cref="BaseComparator"/> to be added to the end of the <see cref="BaseComparatorCollection"/>.</param>
        /// <returns>The <see cref="BaseComparatorCollection"/> index at which the value has been added.</returns>
        public int Add(BaseComparator value)
        {
            return (List.Add(value));
        }

        /// <summary>
        /// Searches for the specified <see cref="BaseComparator"/> and 
        /// returns the zero-based index of the first occurrence within the entire <see cref="BaseComparatorCollection"/>.
        /// </summary>
        /// <param name="value">The <see cref="BaseComparator"/> to locate in the <see cref="BaseComparatorCollection"/>.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of value within the entire <see cref="BaseComparatorCollection"/>, 
        /// if found; otherwise, -1.
        /// </returns>
        public int IndexOf(BaseComparator value)
        {
            return (List.IndexOf(value));
        }

        /// <summary>
        /// Inserts an <see cref="BaseComparator"/> element into the <see cref="BaseComparatorCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The <see cref="BaseComparatorCollection"/> to insert.</param>
        public void Insert(int index, BaseComparator value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific <see cref="BaseComparator"/> from the <see cref="BaseComparatorCollection"/>.
        /// </summary>
        /// <param name="value">The <see cref="BaseComparator"/> to remove from the <see cref="BaseComparatorCollection"/>.</param>
        public void Remove(BaseComparator value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="BaseComparatorCollection"/> contains a specific element.
        /// </summary>
        /// <param name="value">The <see cref="BaseComparator"/> to locate in the <see cref="BaseComparatorCollection"/>.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="BaseComparatorCollection"/> contains the specified value; 
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(BaseComparator value)
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
            if (!(value is BaseComparator))
                throw new ArgumentException("value must be of type BaseComparator.", "value");
        }

        /// <summary>
        /// Performs additional custom processes when removing an element from the collection instance.
        /// </summary>
        /// <param name="index">The zero-based index at which value can be found.</param>
        /// <param name="value">The value of the element to remove from index.</param>
        protected override void OnRemove(int index, Object value)
        {
            if (!(value is BaseComparator))
                throw new ArgumentException("value must be of type BaseComparator.", "value");
        }

        /// <summary>
        /// Performs additional custom processes before setting a value in the collection instance.
        /// </summary>
        /// <param name="index">The zero-based index at which oldValue can be found.</param>
        /// <param name="oldValue">The value to replace with newValue.</param>
        /// <param name="newValue">The new value of the element at index.</param>
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (!(newValue is BaseComparator))
                throw new ArgumentException("newValue must be of type BaseComparator.", "newValue");
        }

        /// <summary>
        /// Performs additional custom processes when validating a value.
        /// </summary>
        /// <param name="value">The object to validate.</param>
        protected override void OnValidate(Object value)
        {
            if (!(value is BaseComparator))
                throw new ArgumentException("value must be of type BaseComparator.");
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection"/> to a strong-typed <c>BaseComparator[]</c>, 
        /// starting at a particular <c>BaseComparator[]</c> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <c>BaseComparator[]</c> that is the destination of the elements 
        /// copied from <see cref="ICollection"/>.
        /// The <c>BaseComparator[]</c> must have zero-based indexing. 
        /// </param>
        /// <param name="index">
        /// The zero-based index in array at which copying begins.
        /// </param>
        /// <remarks>
        /// Provides the strongly typed member for <see cref="ICollection"/>.
        /// </remarks>
        public void CopyTo(BaseComparator[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        /// <summary>
        /// Compare all comparators against eachother.
        /// </summary>
        /// <param name="resultsReporter">Results reporter.</param>
        public void Compare(ResultsReporter resultsReporter)
        {
            for (int i = 0; i < this.Count; i++)
            {
                for (int j = i + 1; j < this.Count; j++)
                {
                    this[i].Compare(_tagValueFilterCollection, resultsReporter, this[j]);
                }
            }
        }
    }
}
