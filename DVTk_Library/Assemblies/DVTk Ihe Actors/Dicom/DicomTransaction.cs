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
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkData.Dul;
using DvtkData.Dimse;
using Dvtk.IheActors.Bases;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomTransaction.
	/// </summary>
	public class DicomTransaction : BaseTransaction
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="transactionName">Transaction Name.</param>
		/// <param name="direction">Transaction direction - sent/received.</param>
		public DicomTransaction(TransactionNameEnum transactionName, TransactionDirectionEnum direction) : base(transactionName, direction) {}

		/// <summary>
		/// Property - AssocRequest.
		/// </summary>
		public DvtkData.Dul.A_ASSOCIATE_RQ AssocRequest
		{
			get 
			{ 
				return _assocRequest; 
			}
			set
			{
				_assocRequest = value;
			}
		} 
		private DvtkData.Dul.A_ASSOCIATE_RQ _assocRequest;

		/// <summary>
		/// Property - AssocAccept
		/// </summary>
		public DvtkData.Dul.A_ASSOCIATE_AC AssocAccept
		{
			get 
			{ 
				return _assocAccept; 
			}
			set
			{
				_assocAccept = value;
			}
		} 
		private DvtkData.Dul.A_ASSOCIATE_AC _assocAccept;

		/// <summary>
		/// Property - DicomMessages.
		/// </summary>
		public DicomMessageCollection DicomMessages
		{
			get
			{
				return _dicomMessages;
			}
		}
		private DicomMessageCollection _dicomMessages = new DicomMessageCollection();
	}
}
