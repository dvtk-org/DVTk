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


namespace Dvtk.IheActors.Hl7.WebService
{
	/// <summary>
	/// Summary description for ErrorWarningCounters.
	/// </summary>
	public class NistXmlResultsParser
	{
		private int _errorCount = 0;
		private int _warningCount = 0;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="xmlResults">Xml Results Stream</param>
		public NistXmlResultsParser(System.String xmlResults)
		{
			if (xmlResults != System.String.Empty)
			{
				System.IO.StringReader stringReader = new StringReader(xmlResults);
				XmlTextReader xmlReader = new XmlTextReader(stringReader);

				xmlReader.ReadStartElement("ValidationReport");

				//+ <profile name="The profile was created from a string source">
				xmlReader.Skip();

				//+ <validationContext defaultAction="error">
				xmlReader.Skip();

				//+ <message errorCnt="12" ignorableCnt="0" warningCnt="0">
				xmlReader.ReadAttributeValue();

				System.String errorCountString = xmlReader.GetAttribute("errorCnt");
				if (errorCountString != System.String.Empty)
				{
					_errorCount = int.Parse(errorCountString);
				}

				System.String warningCountString = xmlReader.GetAttribute("warningCnt");
				if (warningCountString != System.String.Empty)
				{
					_warningCount = int.Parse(warningCountString);
				}

				xmlReader.Close();
			}
		}

		/// <summary>
		/// Remove the first line from the xml string.
		/// - so that we can embed this string in the DVT results xml string.
		/// </summary>
		/// <param name="xmlString">Input string.</param>
		/// <returns>Modified String - without the first line.</returns>
		public System.String RemoveHeader(System.String xmlString)
		{
			// <?xml version="1.0" encoding="UTF-8"?> 
			System.String returnString = xmlString.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n", System.String.Empty);
			return returnString;
		}

		/// <summary>
		/// Property - ErrorCount
		/// </summary>
		public int ErrorCount
		{
			get
			{
				return _errorCount;
			}
		}

		/// <summary>
		/// Property - WarningCount
		/// </summary>
		public int WarningCount
		{
			get
			{
				return _warningCount;
			}
		}
	}
}
