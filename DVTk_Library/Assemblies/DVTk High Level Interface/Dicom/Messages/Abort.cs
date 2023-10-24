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
	/// Represents a Dicom A_ABORT.
	/// </summary>
	public class Abort: DulMessage
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private Abort(): base (new DvtkData.Dul.A_ABORT())
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor to encapsulate an existing DvtkData A_ABORT.
		/// </summary>
		/// <param name="dvtkDataAbort">The encapsulated DvtkData A_ABORT.</param>
		internal Abort(DvtkData.Dul.A_ABORT dvtkDataAbort): base(dvtkDataAbort)
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="source">The Abort source.</param>
		/// <param name="reason">The Abort reason.</param>
		internal Abort(Byte source, Byte reason): base (new DvtkData.Dul.A_ABORT(reason, source))
		{
			// Do nothing.
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets the encapsulated DvtkData A_ABORT.
		/// </summary>
		internal DvtkData.Dul.A_ABORT DvtkDataAAbort
		{
			get
			{
				return(DvtkDataDulMessage as DvtkData.Dul.A_ABORT);
			}
		}

		/// <summary>
		/// Gets the Abort reason.
		/// </summary>
		public Byte Reason
		{
			get
			{
				return(DvtkDataAAbort.Reason);
			}
		}

		/// <summary>
		/// Gets the Abort source.
		/// </summary>
		public Byte Source
		{
			get
			{
				return(DvtkDataAAbort.Source);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Returns a String that represents this instance.
		/// </summary>
		/// <returns>A String that represents this instance.</returns>
		public override string ToString()
		{
			return "A-ABORT";
		}
	}
}
