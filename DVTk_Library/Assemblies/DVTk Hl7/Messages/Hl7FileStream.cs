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

namespace Dvtk.Hl7.Messages
{
	/// <summary>
	/// Class that implements the creation of a HL7 message from the content of a file.
	/// </summary>
	public class Hl7FileStream: Hl7Stream
	{
		//
		// - Methods -
		//

		/// <summary>
		/// Create a HL7 message from the content of a file.
		/// </summary>
		/// <param name="fullFileName">The full file name.</param>
		/// <returns>The HL7 messages created from the content of the file.</returns>
		public Hl7Message In(String fullFileName)
		{
			// set up the file stream for reading
			FileStream fileStream = new FileStream(fullFileName, FileMode.Open);

			// Decode the file stream into an HL7 message
			Hl7Message hl7Message = Decode(fileStream);

			// Return the HL7 message
			return hl7Message;
		}

		// Todo: implement the out method.
//		public static void Out(Hl7Message hl7Message)
//		{
//
//		}
	}
}
