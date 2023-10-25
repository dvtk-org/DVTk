// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright � 2009 DVTk
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
	/// Represents a Dicom A_RELEASE_RP.
	/// </summary>
	public class ReleaseRp: DulMessage
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal ReleaseRp(): base(new DvtkData.Dul.A_RELEASE_RP())
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor to encapsulate an existing DvtkData A_RELEASE_RP.
		/// </summary>
		/// <param name="dvtkDataReleaseRp">The encapsulated DvtkData A_RELEASE_RP</param>
		internal ReleaseRp(DvtkData.Dul.A_RELEASE_RP dvtkDataReleaseRp): base(dvtkDataReleaseRp)
		{
			// Do nothing.
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
			return "A_RELEASE_RP";
		}
	}
}
