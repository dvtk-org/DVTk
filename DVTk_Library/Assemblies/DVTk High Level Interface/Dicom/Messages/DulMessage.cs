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
	/// An object of this class represents a Dul message.
	/// </summary>
	public abstract class DulMessage: DicomProtocolMessage
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property DvtkDataDulMessage.
		/// </summary>
		private DvtkData.Dul.DulMessage dvtkDataDulMessage = null;



		//
		// - Constructors -
		//		

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dvtkDataDulMessage">The encapsulated DvtkData DulMessage.</param>
		internal DulMessage(DvtkData.Dul.DulMessage dvtkDataDulMessage)
		{
			this.dvtkDataDulMessage = dvtkDataDulMessage;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// The encapsulated DvtkData DulMessage.
		/// </summary>
		public DvtkData.Dul.DulMessage DvtkDataDulMessage
		{
			get
			{
				return(this.dvtkDataDulMessage);
			}
		}
	}
}
