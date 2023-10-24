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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Dvtk.Hl7
{
	/// <summary>
	/// Summary description for Hl7ValidationContext.
	/// </summary>
	public class Hl7ValidationContext
	{
		private System.String _filename = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="filename">HL7 Validation Context Filename.</param>
		public Hl7ValidationContext(System.String filename)
		{
			_filename = filename;
		}

		/// <summary>
		/// Property XmlValidationContext.
		/// </summary>
		public System.String XmlValidationContext
		{
			get
			{
				System.String xmlValidationContext = null;
				try
				{
					System.String input = null;
					System.IO.StreamReader sr = new System.IO.StreamReader(_filename);
					while ((input = sr.ReadLine()) != null)
					{
						xmlValidationContext += input;
					}
					sr.Close();
				}
				catch (System.Exception e)
				{
					System.String message = System.String.Format("Failed to read XML HL7 Validation Context: \"{0}\"", _filename);
					throw new System.SystemException(message, e);
				}

				return xmlValidationContext;
			}
		}
	}
}
