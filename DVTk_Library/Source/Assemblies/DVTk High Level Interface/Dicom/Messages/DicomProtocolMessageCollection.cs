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



namespace DvtkHighLevelInterface.Dicom.Messages
{
	/// <summary>
	/// Collection of DicomProtocolMessages.
	/// </summary>
	public sealed class DicomProtocolMessageCollection : DvtkData.Collections.NullSafeCollectionBase
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DicomProtocolMessageCollection()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor with initialization. Shallow copy.
		/// </summary>
		/// <param name="arrayOfValues">values to copy.</param>
		public DicomProtocolMessageCollection(DicomProtocolMessage[] arrayOfValues)
		{
			if (arrayOfValues == null)
			{
				throw new ArgumentNullException();
			}

			foreach (DicomProtocolMessage value in arrayOfValues)
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
		public new DicomProtocolMessage this[int index]
		{
			get 
			{
				return (DicomProtocolMessage)base[index];
			}
			set
			{
				base.Insert(index, value);
			}
		}

		/// <summary>
		/// The DicomMessages in this collection.
		/// </summary>
		public DicomMessageCollection DicomMessages
		{
			get
			{
				DicomMessageCollection dicomMessages = new DicomMessageCollection();

				foreach(DicomProtocolMessage dicomProtocolMessage in this)
				{
					if (dicomProtocolMessage is DicomMessage)
					{
						dicomMessages.Add(dicomProtocolMessage as DicomMessage);
					}
				}

				return(dicomMessages);
			}
		}

		/// <summary>
		/// The DulMessages in this collection.
		/// </summary>
		public DulMessageCollection DulMessages
		{
			get
			{
				DulMessageCollection dulMessages = new DulMessageCollection();

				foreach(DicomProtocolMessage dicomProtocolMessage in this)
				{
					if (dicomProtocolMessage is DulMessage)
					{
						dulMessages.Add(dicomProtocolMessage as DulMessage);
					}
				}

				return(dulMessages);
			}
		}

		/// <summary>
		/// The received messages in this collection.
		/// </summary>
		public DicomProtocolMessageCollection ReceivedMessages
		{
			get
			{
				DicomProtocolMessageCollection receivedMessages = new DicomProtocolMessageCollection();

				foreach(DicomProtocolMessage dicomProtocolMessage in this)
				{
					if (dicomProtocolMessage.IsReceived)
					{
						receivedMessages.Add(dicomProtocolMessage);
					}
				}

				return(receivedMessages);
			}
		}

		/// <summary>
		/// The send messages in this collection.
		/// </summary>
		public DicomProtocolMessageCollection SendMessages
		{
			get
			{
				DicomProtocolMessageCollection sendMessages = new DicomProtocolMessageCollection();

				foreach(DicomProtocolMessage dicomProtocolMessage in this)
				{
					if (dicomProtocolMessage.IsSend)
					{
						sendMessages.Add(dicomProtocolMessage);
					}
				}

				return(sendMessages);
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
		public int Add(DicomProtocolMessage value)
		{
			return base.Add(value);
		}

		/// <summary>
		/// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
		public bool Contains(DicomProtocolMessage value)
		{
			return base.Contains(value);
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
		public int IndexOf(DicomProtocolMessage value)
		{
			return base.IndexOf(value);
		}

		/// <summary>
		/// Inserts an item to the IList at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
		/// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
		public void Insert(int index, DicomProtocolMessage value)
		{
			base.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific item from the IList.
		/// </summary>
		/// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
		public void Remove(DicomProtocolMessage value)
		{
			base.Remove(value);
		}

        /// <summary>
        /// Returns a String that represents this instance.
        /// </summary>
        /// <returns>A String that represents this instance.</returns>
		public override String ToString()
		{
			String toString = "";

			foreach(DicomProtocolMessage dicomProtocolMessage in this)
			{
				toString+= dicomProtocolMessage.ToString() + "\r\n";
			}

			return(toString);
		}

        /// <summary>
        /// Gets the last received messages that is part of this collection.
        /// </summary>
		internal DicomProtocolMessage LastReceivedMessage
		{
			get
			{
				DicomProtocolMessage lastReceivedMessage = null;

				DicomProtocolMessageCollection receivedMessage = ReceivedMessages;

				if (receivedMessage.Count > 0)
				{
					lastReceivedMessage = receivedMessage[receivedMessage.Count - 1];
				}

				return(lastReceivedMessage);
			}
		}
	}
}
