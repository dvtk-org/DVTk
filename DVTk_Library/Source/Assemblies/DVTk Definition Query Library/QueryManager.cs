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
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Dvtk.Definition_Query_Library
{
	/// <summary>
	///		Class manages the list of all pre-compiled XPath queries.
	/// </summary>
	/// <remarks>
	///		Only static queries are  handled by this class. Dynamic query
	///		support may be added later using the library from the XMLMVP
	///		project. See http://www.mvpxml.org for the project website and the
	///		blog postings of Daniel Cazzulino at
	///		http://clariusconsulting.net/blogs/kzu/archive/2003/10/07/82.aspx.
	/// </remarks>
	public class QueryManager
	{
		private Hashtable queryHashTable;
		
		/// <summary>
		///		Constructor for the
		///		<see cref="T:Dvtk.Definition_Query_Library.QueryManager">QueryManager
		///		</see>class.
		/// </summary>
		public QueryManager()
		{
			// Create in HashTable instance for the queryHashTable.
			queryHashTable = new Hashtable();

			// Fill the queryHashTable.
			initQueryTable();
		}

		/// <summary>
		///		Get the XPathExpression instance containing the pre-compiled
		///		query for the QueryKey.
		/// </summary>
		/// <param name="QueryKey">A string identifying the query.</param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get a specific pre-compiled query expression.
		///			
		///			QueryManager myQueryManager;
		///			XPathExpression myExpression;
		///			
		///			myQueryManager = new QueryManager;
		///			myExpression = myQueryManager.GetQuery("myQueryKeyName");
		///		</code>
		/// </example>
		/// <returns>
		///		A XPathExpression containing the pre-compiled query expression.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the QueryKey can't be found.
		/// </exception>
		public XPathExpression GetQuery(string QueryKey)
		{
			// Check if the QueryKey is in the queryHashTable.
			if (queryHashTable.ContainsKey(QueryKey) == true)
			{
				// Return the XPathExpression for the QueryKey.
				return (XPathExpression) queryHashTable[QueryKey];
			}
			else
			{
				throw new Exception("Could not find the query for QueryKey: " + QueryKey);
			}
		}
		
		/// <summary>
		///		Fills the
		///		<see cref="Dvtk.Definition_Query_Library.QueryManager.queryHashTable">
		///		queryHashTable</see> with pre-compiled queries.
		/// </summary>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			initQueryTable();
		///		</code>
		/// </example>
		private void initQueryTable()
		{
			XmlDocument dummyDocument;
			XPathNavigator dummyNavigator;
			XPathExpression preCompiledExpression;
			
			// Init the document and navigator.
			dummyDocument = new XmlDocument();
			dummyNavigator = dummyDocument.CreateNavigator();

			// Create pre-compiled static query for the function:
			// GetSystemName()
			preCompiledExpression = dummyNavigator.Compile("/System/@Name");
			queryHashTable.Add("GetSystemName()", preCompiledExpression);

			// Create pre-compiled static query for the function:
			// GetSystemVersion()
			preCompiledExpression = dummyNavigator.Compile("/System/@Version");
			queryHashTable.Add("GetSystemVersion()", preCompiledExpression);

			// Create pre-compiled static query for the function:
			// GetApplicationName()
			preCompiledExpression = dummyNavigator.Compile("/System/*/@Name");
			queryHashTable.Add("GetApplicationName()", preCompiledExpression);

			// Create pre-compiled static query for the function:
			// GetAllDimseCommands()
			preCompiledExpression = dummyNavigator.Compile("/System/*/SOPClass/Dimse/@Name");
			queryHashTable.Add("GetAllDimseCommands()", preCompiledExpression);

			// Create pre-compiled static query for the function:
			// GetAllModules()
			preCompiledExpression = dummyNavigator.Compile("/System/*/SOPClass/Dimse/Dataset/Module/@Name");
			queryHashTable.Add("GetAllModules()", preCompiledExpression);

			// Create pre-compiled static query for the function:
			// GetAllAttributes(subitems = false)
			preCompiledExpression = dummyNavigator.Compile("/System/*/SOPClass/Dimse/Dataset/Module/Attribute/Group");
			queryHashTable.Add("GetAllAttributes(subitems = false)", preCompiledExpression);

			// Create pre-compiled static query for the function:
			// GetAllAttributes(subitems = true)
			preCompiledExpression = dummyNavigator.Compile("//Attribute/Group");
			queryHashTable.Add("GetAllAttributes(subitems = true)", preCompiledExpression);
		}
	}
}
