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

namespace Dvtk.IheActors.Bases
{
	#region transaction names
	public enum TransactionNameEnum
	{
		RAD_1,
		RAD_2,
		RAD_3,
		RAD_4,
		RAD_5,
		RAD_6,
		RAD_7,
		RAD_8,
		RAD_9,
		RAD_10,
		RAD_11,
		RAD_12,
		RAD_13,
		RAD_14,
		RAD_15,
		RAD_16,
		RAD_17,
		RAD_18,
		RAD_19,
		RAD_20,
		RAD_21,
		RAD_23,
		RAD_42,
		RAD_48,
		RAD_49,
		RAD_UNKNOWN,
	}
	#endregion

	#region transaction directions
	public enum TransactionDirectionEnum
	{
		TransactionSent,
		TransactionReceived
	}
	#endregion

	/// <summary>
	/// Summary description for BaseTransaction.
	/// </summary>
	public abstract class BaseTransaction
	{
		private TransactionNameEnum _transactionName;
		private TransactionDirectionEnum _direction;
		private bool _processed;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="transactionName">IHE Transaction Name.</param>
		/// <param name="direction">Transaction direction - sent/received.</param>
		public BaseTransaction(TransactionNameEnum transactionName, TransactionDirectionEnum direction)
		{
			_transactionName = transactionName;
			_direction = direction;
		}

		/// <summary>
		/// Property - Direction.
		/// </summary>
		public TransactionDirectionEnum Direction
		{
			get
			{
				return _direction;
			}
		}

		/// <summary>
		/// Property - TransactionName.
		/// </summary>
		public TransactionNameEnum TransactionName
		{
			get 
			{ 
				return _transactionName; 
			}
		} 

		/// <summary>
		/// Property - Processed.
		/// </summary>
		public bool Processed
		{
			get
			{
				return _processed;
			}
			set
			{
				_processed = value;
			}
		}
	}
}
