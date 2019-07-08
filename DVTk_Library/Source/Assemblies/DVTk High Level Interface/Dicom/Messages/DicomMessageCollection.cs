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

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Dicom.Messages
{
	/// <summary>
	/// Collection of DicomMessages.
	/// </summary>
	public sealed class DicomMessageCollection : DvtkData.Collections.NullSafeCollectionBase
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DicomMessageCollection()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor with initialization. Shallow copy.
		/// </summary>
		/// <param name="arrayOfValues">values to copy.</param>
		public DicomMessageCollection(DicomMessage[] arrayOfValues)
		{
			if (arrayOfValues == null)
			{
				throw new ArgumentNullException();
			}

			foreach (DicomMessage value in arrayOfValues)
			{
				this.Add(value);
			}
		}



		//
		// - Properties -
		//

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CCANCELRQ.
		/// </summary>
		public DicomMessageCollection CCancelRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CCANCELRQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CECHORQ.
		/// </summary>
		public DicomMessageCollection CEchoRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CECHORQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CECHORSP.
		/// </summary>
		public DicomMessageCollection CEchoResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CECHORSP));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CFINDRQ.
		/// </summary>
		public DicomMessageCollection CFindRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CFINDRQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CFINDRSP.
		/// </summary>
		public DicomMessageCollection CFindResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CFINDRSP));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CGETRQ.
		/// </summary>
		public DicomMessageCollection CGetRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CGETRQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CGETRSP.
		/// </summary>
		public DicomMessageCollection CGetResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CGETRSP));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CMOVERQ.
		/// </summary>
		public DicomMessageCollection CMoveRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CMOVERQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CMOVERSP.
		/// </summary>
		public DicomMessageCollection CMoveResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CMOVERSP));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CSTORERQ.
		/// </summary>
		public DicomMessageCollection CStoreRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CSTORERQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand CSTORERSP.
		/// </summary>
		public DicomMessageCollection CStoreResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.CSTORERSP));
			}
		}

        /// <summary>
        /// Gets all data sets that are present in this collection.
        /// </summary>
        public DataSetCollection DataSets
        {
            get
            {
                DataSetCollection dataSets = new DataSetCollection();

                foreach (DicomMessage dicomMessage in this)
                {
                    dataSets.Add(dicomMessage.DataSet);
                }

                return (dataSets);
            }
        }

		/// <summary>
		/// Gets or sets the item at the specified index.
		/// </summary>
		/// <value>The item at the specified <c>index</c>.</value>
		public new DicomMessage this[int index]
		{
			get 
			{
				return (DicomMessage)base[index];
			}
			set
			{
				base.Insert(index, value);
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NACTIONRQ.
		/// </summary>
		public DicomMessageCollection NActionRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NACTIONRQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NACTIONRSP.
		/// </summary>
		public DicomMessageCollection NActionResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NACTIONRSP));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NCREATERQ.
		/// </summary>
		public DicomMessageCollection NCreateRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NCREATERQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NCREATERSP.
		/// </summary>
		public DicomMessageCollection NCreateResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NCREATERSP));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NDELETERQ.
		/// </summary>
		public DicomMessageCollection NDeleteRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NDELETERQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NDELETERSP.
		/// </summary>
		public DicomMessageCollection NDeleteResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NDELETERSP));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NEVENTREPORTRQ.
		/// </summary>
		public DicomMessageCollection NEventReportRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NEVENTREPORTRQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NEVENTREPORTRSP.
		/// </summary>
		public DicomMessageCollection NEventReportResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NEVENTREPORTRSP));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NGETRQ.
		/// </summary>
		public DicomMessageCollection NGetRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NGETRQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NGETRSP.
		/// </summary>
		public DicomMessageCollection NGetResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NGETRSP));
			}
		}
		
		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NSETRQ.
		/// </summary>
		public DicomMessageCollection NSetRequests
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NSETRQ));
			}
		}

		/// <summary>
		/// All the DicomMessages in this collection that have a DimseCommand NSETRSP.
		/// </summary>
		public DicomMessageCollection NSetResponses
		{
			get
			{
				return(DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand.NSETRSP));
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
		public int Add(DicomMessage value)
		{
			return base.Add(value);
		}

		/// <summary>
		/// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
		public bool Contains(DicomMessage value)
		{
			return base.Contains(value);
		}

		/// <summary>
		/// Return the DicomMessages from this collection that have the specified DimseCommand.
		/// </summary>
		/// <param name="dimseCommand">the DimseCommand.</param>
		/// <returns>The DicomMessages with the specified DimseCommand.</returns>
		private DicomMessageCollection DicomMessagesForDimseCommand(DvtkData.Dimse.DimseCommand dimseCommand)
		{
			DicomMessageCollection dicomMessages = new DicomMessageCollection();

			foreach(DicomMessage dicomMessage in this)
			{
				if (dicomMessage.CommandSet.DimseCommand == dimseCommand)
				{
					dicomMessages.Add(dicomMessage);
				}
			}

			return(dicomMessages);
		}

        /// <summary>
        /// Get all the DataSets that are present in the DICOM messages.
        /// </summary>
        /// <returns></returns>
		public DataSetCollection GetDataSets()
		{
			DataSetCollection dataSets = new DataSetCollection();

			foreach(DicomMessage dicomMessage in this)
			{
				dataSets.Add(dicomMessage.DataSet);
			}

			return(dataSets);
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
		public int IndexOf(DicomMessage value)
		{
			return base.IndexOf(value);
		}

		/// <summary>
		/// Inserts an item to the IList at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
		/// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
		public void Insert(int index, DicomMessage value)
		{
			base.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific item from the IList.
		/// </summary>
		/// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
		public void Remove(DicomMessage value)
		{
			base.Remove(value);
		}
	}
}
