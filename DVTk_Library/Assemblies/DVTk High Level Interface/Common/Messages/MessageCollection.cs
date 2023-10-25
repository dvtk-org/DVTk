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

using DvtkData.Collections;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Hl7.Messages;



namespace DvtkHighLevelInterface.Common.Messages
{
	/// <summary>
	/// Collection of messages.
	/// </summary>
	public sealed class MessageCollection : DvtkData.Collections.NullSafeCollectionBase
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public MessageCollection()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor with initialization. Shallow copy.
		/// </summary>
		/// <param name="arrayOfValues">values to copy.</param>
		public MessageCollection(Message[] arrayOfValues)
		{
			if (arrayOfValues == null)
			{
				throw new ArgumentNullException();
			}

			foreach (Message value in arrayOfValues)
			{
				this.Add(value);
			}
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets the DICOM protocol messages in this collection.
		/// </summary>
		public DicomProtocolMessageCollection DicomProtocolMessages
		{
			get
			{
				DicomProtocolMessageCollection dicomProtocolMessages = new DicomProtocolMessageCollection();

				foreach(Message message in this)
				{
					if (message is DicomProtocolMessage)
					{
						dicomProtocolMessages.Add(message as DicomProtocolMessage);
					}
				}

				return(dicomProtocolMessages);
			}
		}	

		/// <summary>
		/// Gets the HL7 protocol messages in this collection.
		/// </summary>
		public Hl7ProtocolMessageCollection Hl7ProtocolMessages
		{
			get
			{
				Hl7ProtocolMessageCollection hl7ProtocolMessages = new Hl7ProtocolMessageCollection();

				foreach(Message message in this)
				{
					if (message is Hl7ProtocolMessage)
					{
						hl7ProtocolMessages.Add(message as Hl7ProtocolMessage);
					}
				}

				return(hl7ProtocolMessages);
			}
		}	

		/// <summary>
		/// Gets or sets the item at the specified index.
		/// </summary>
		/// <value>The item at the specified <c>index</c>.</value>
		public new Message this[int index]
		{
			get 
			{
				return (Message)base[index];
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
		public int Add(Message value)
		{
			return base.Add(value);
		}

		/// <summary>
		/// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
		public bool Contains(Message value)
		{
			return base.Contains(value);
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
		public int IndexOf(Message value)
		{
			return base.IndexOf(value);
		}

		/// <summary>
		/// Inserts an item to the IList at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
		/// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
		public void Insert(int index, Message value)
		{
			base.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific item from the IList.
		/// </summary>
		/// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
		public void Remove(Message value)
		{
			base.Remove(value);
		}

        /// <summary>
        /// Returns a String that represents the current Object. 
        /// </summary>
        /// <returns>A String that represents the current instance.</returns>
		public override String ToString()
		{
			String toString = "";

			foreach(Message message in this)
			{
				toString+= message.ToString() + "\r\n";
			}

			return(toString);
		}
	}	
}
