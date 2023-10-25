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
using System.IO;
using System.Collections;
using DvtkData.DvtDetailToXml;

using DvtkDicomUnicodeConversion;

namespace DvtkData.Collections
{
    using SIGNED_SHORT = System.Int16;
    using UNSIGNED_SHORT = System.UInt16;
    using SIGNED_LONG = System.Int32;
    using UNSIGNED_LONG = System.UInt32;
    using GROUP_NUMBER = System.UInt16;
    using ELEMENT_NUMBER = System.UInt16;
    using FPD = System.Double;
    using FPS = System.Single;
    /// <summary>
    /// A null-safe collection.
    /// This class is meant to be extended by a type-safe collection classess.
	/// </summary>
	public abstract class NullSafeCollectionBase : System.Collections.CollectionBase
    {
        #region IList Members

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Collections.IList"/> is read-only.
        /// </summary>
        /// <value>Always <see langword="false"/>.</value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        /// <remarks>
        /// Method access is protected. Only to be accessed by subclass.
        /// </remarks>
        protected object this[int index]
        {
            get
            {
                //throws ArgumentOutOfRangeException
                return List[index];
            }
            set
            {
                //throws ArgumentOutOfRangeException
                Insert(index, value);
            }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted.</param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        /// <remarks>
        /// Method access is protected. Only to be accessed by subclass.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        protected void Insert(int index, object value)
        {
            if (value == null) throw new ArgumentNullException();
            //throws ArgumentOutOfRangeException
            List.Insert(index, value);
            return;
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        /// <remarks>
        /// Method access is protected. Only to be accessed by subclass.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        protected void Remove(object value)
        {
            if (value == null) throw new ArgumentNullException();
            this.List.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// Method access is protected. Only to be accessed by subclass.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        protected bool Contains(object value)
        {
            if (value == null) throw new ArgumentNullException();
            return this.List.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        /// <remarks>
        /// Method access is protected. Only to be accessed by subclass.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        protected int IndexOf(object value)
        {
            if (value == null) throw new ArgumentNullException();
            return this.List.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        /// <remarks>
        /// Method access is protected. Only to be accessed by subclass.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        protected int Add(object value)
        {
            if (value == null) throw new ArgumentNullException();
            return this.List.Add(value);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <value>Always <see langword="false"/>.</value>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// Gets a value indicating whether access to the ICollection is synchronized (thread-safe).
        /// </summary>
        /// <value>Always <see langword="false"/>.</value>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection"/> to an <see cref="Array"/>, starting at a particular <c>array</c> <c>index</c>.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ICollection"/>. The <c>array</c> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <c>array</c> at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            this.List.CopyTo(array, index);
        }

        /// <summary>
        /// Gets an <see cref="object"/> that can be used to synchronize access to the <see cref="ICollection"/>.
        /// </summary>
        /// <value>Not implemented. Always returns <see langword="null"/>.</value>
        public object SyncRoot
        {
            get
            {
                return null;
            }
        }

        #endregion
    }

    #region Strong-typed ValueType Collections

    /// <summary>
    /// Type safe StringCollection
    /// </summary>
    public sealed class StringCollection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StringCollection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public StringCollection(System.String[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (System.String value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new System.String this[int index]
        {
            get { return (System.String)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, System.String value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(System.String value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(System.String value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(System.String value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.String value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// DICOM to Unicode Converter
        /// </summary>
        public DicomUnicodeConverter DicomUnicodeConverter
        {
            set
            {
                _dicomUnicodeConverter = value;
            }
        }
        private DicomUnicodeConverter _dicomUnicodeConverter = null;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (System.String value in this)
            {
                streamWriter.WriteLine("<Value>{0}</Value>", DvtToXml.ConvertString(value, true));

                // try to convert the string to Unicode - if possible
                if (_dicomUnicodeConverter != null)
                {
                    String outString = DvtToXml.ConvertStringToXmlUnicode(_dicomUnicodeConverter, value);
                    if (outString != String.Empty)
                    {
                        streamWriter.WriteLine("<Unicode>{0}</Unicode>", outString);
                    }
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Type safe Int16Collection
    /// </summary>
    public sealed class Int16Collection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Int16Collection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public Int16Collection(System.Int16[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (System.Int16 value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new System.Int16 this[int index]
        {
            get { return (System.Int16)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, System.Int16 value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(System.Int16 value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(System.Int16 value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(System.Int16 value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.Int16 value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (System.Int16 value in this)
            {
                streamWriter.WriteLine("<Value>{0}</Value>", value.ToString());
            }

            return true;
        }
    }

    /// <summary>
    /// Type safe UInt16Collection
    /// </summary>
    public sealed class UInt16Collection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UInt16Collection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public UInt16Collection(System.UInt16[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (System.UInt16 value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new System.UInt16 this[int index]
        {
            get { return (System.UInt16)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, System.UInt16 value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(System.UInt16 value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(System.UInt16 value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(System.UInt16 value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.UInt16 value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (System.UInt16 value in this)
            {
                string uint16Value = value.ToString("X");

                while (uint16Value.Length < 4)
                {
                    uint16Value = "0" + uint16Value;
                }
                streamWriter.WriteLine("<Value>0x{0}</Value>", uint16Value);
            }

            return true;
        }
    }

    /// <summary>
    /// Type safe Int32Collection
    /// </summary>
    public sealed class Int32Collection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Int32Collection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public Int32Collection(System.Int32[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (System.Int32 value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new System.Int32 this[int index]
        {
            get { return (System.Int32)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, System.Int32 value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(System.Int32 value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(System.Int32 value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(System.Int32 value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.Int32 value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (System.Int32 value in this)
            {
                streamWriter.WriteLine("<Value>{0}</Value>", value.ToString());
            }

            return true;
        }
    }

    /// <summary>
    /// Type safe Int64Collection
    /// </summary>
    public sealed class Int64Collection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Int64Collection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public Int64Collection(System.Int64[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (System.Int64 value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new System.Int64 this[int index]
        {
            get { return (System.Int64)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, System.Int64 value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(System.Int64 value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(System.Int64 value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(System.Int64 value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.Int32 value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.Int64 value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (System.Int64 value in this)
            {
                streamWriter.WriteLine("<Value>{0}</Value>", value.ToString());
            }

            return true;
        }
    }

    /// <summary>
    /// Type safe UInt32Collection
    /// </summary>
    public sealed class UInt32Collection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UInt32Collection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public UInt32Collection(System.UInt32[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (System.UInt32 value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new System.UInt32 this[int index]
        {
            get { return (System.UInt32)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, System.UInt32 value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(System.UInt32 value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(System.UInt32 value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(System.UInt32 value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.UInt32 value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (System.UInt32 value in this)
            {
                string uint32Value = value.ToString("X");

                while (uint32Value.Length < 8)
                {
                    uint32Value = "0" + uint32Value;
                }
                streamWriter.WriteLine("<Value>0x{0}</Value>", uint32Value);
            }

            return true;
        }
    }

    /// <summary>
    /// Type safe UInt64Collection
    /// </summary>
    public sealed class UInt64Collection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UInt64Collection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public UInt64Collection(System.UInt64[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (System.UInt64 value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new System.UInt64 this[int index]
        {
            get { return (System.UInt64)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, System.UInt64 value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(System.UInt64 value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(System.UInt64 value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(System.UInt64 value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.UInt64 value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (System.UInt64 value in this)
            {
                string uint64Value = value.ToString("X");

                while (uint64Value.Length < 8)
                {
                    uint64Value = "0" + uint64Value;
                }
                streamWriter.WriteLine("<Value>0x{0}</Value>", uint64Value);
            }

            return true;
        }
    }

    /// <summary>
    /// Type safe SingleCollection
    /// </summary>
    public sealed class SingleCollection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SingleCollection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public SingleCollection(System.Single[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (System.Single value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new System.Single this[int index]
        {
            get { return (System.Single)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, System.Single value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(System.Single value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(System.Single value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(System.Single value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.Single value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (System.Single value in this)
            {
                streamWriter.WriteLine("<Value>{0}</Value>", value.ToString());
            }

            return true;
        }
    }

    /// <summary>
    /// Type safe DoubleCollection
    /// </summary>
    public sealed class DoubleCollection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DoubleCollection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public DoubleCollection(System.Double[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (System.Double value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new System.Double this[int index]
        {
            get { return (System.Double)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, System.Double value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(System.Double value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(System.Double value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(System.Double value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(System.Double value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (System.Double value in this)
            {
                streamWriter.WriteLine("<Value>{0}</Value>", value.ToString());
            }

            return true;
        }
    }

    /// <summary>
    /// Type safe TagCollection
    /// </summary>
    public sealed class TagCollection : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TagCollection() { }

        /// <summary>
        /// Constructor with initialization. Shallow copy.
        /// </summary>
        /// <param name="arrayOfValues">values to copy.</param>
        public TagCollection(DvtkData.Dimse.Tag[] arrayOfValues)
        {
            if (arrayOfValues == null) throw new ArgumentNullException();
            foreach (DvtkData.Dimse.Tag value in arrayOfValues) this.Add(value);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new DvtkData.Dimse.Tag this[int index]
        {
            get { return (DvtkData.Dimse.Tag)base[index]; }
            set { base.Insert(index, value); }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, DvtkData.Dimse.Tag value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(DvtkData.Dimse.Tag value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(DvtkData.Dimse.Tag value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(DvtkData.Dimse.Tag value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(DvtkData.Dimse.Tag value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            foreach (DvtkData.Dimse.Tag tag in this)
            {
                string group = tag.GroupNumber.ToString("X");
                while (group.Length < 4)
                {
                    group = "0" + group;
                }
                string element = tag.ElementNumber.ToString("X");
                while (element.Length < 4)
                {
                    element = "0" + element;
                }
                streamWriter.WriteLine("<Value>0x{0}{1}</Value>", group, element);
            }

            return true;
        }
    }
    #endregion
}
