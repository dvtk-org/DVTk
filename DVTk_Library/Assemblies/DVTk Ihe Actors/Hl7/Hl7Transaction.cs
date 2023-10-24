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
using Dvtk.IheActors.Bases;

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;

namespace Dvtk.IheActors.Hl7
{
	/// <summary>
	/// Summary description for Hl7Transaction.
	/// </summary>
	public class Hl7Transaction : BaseTransaction
	{
		public Hl7Transaction(TransactionNameEnum transactionName, TransactionDirectionEnum direction) : base(transactionName, direction)
		{
		}

		public Hl7Message Request
		{
			get 
			{ 
				return _request; 
			}
			set
			{
				_request = value;
			}
		} 
		private Hl7Message _request;

		public Hl7Message Response
		{
			get 
			{ 
				return _response; 
			}
			set
			{
				_response = value;
			}
		} 
		private Hl7Message _response;
	}
}
