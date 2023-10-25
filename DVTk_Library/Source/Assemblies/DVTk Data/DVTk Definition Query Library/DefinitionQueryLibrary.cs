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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

namespace Dvtk.Definition_Query_Library
{
	/// <summary>
	///		Summary description for DefinitionQueryLibrary.
	/// </summary>
	public class DefinitionQueryLibrary : IXPathQueryInterface, ISimpleQueryInterface
	{
		#region Class scope variables
		/// <summary>
		///		For managing Xml Definition files.
		///	</summary>
		private FileManager dqFileManager;
		/// <summary>
		///		For managing pre-compiled XPath Expression queries.
		///	</summary>
		private QueryManager dqQueryManager;
		/// <summary>
		///		For the SOP Class UID used in queries.
		///	</summary>
		private string sopClassUID;
		/// <summary>
		///		For the System Name used in queries.
		///	</summary>
		private string systemName;
		/// <summary>
		///		For the Xml Definition file that is used in the query.
		///	</summary>
		private XPathDocument queryDocument;
		/// <summary>
		///		For the navigator used to execute the query against the
		///		queryDocument
		///	</summary>
		private XPathNavigator queryNavigator;
		/// <summary>
		///		For a cloned copy of the pre-compiled expression used in the
		///		Query.
		///	</summary>
		private XPathExpression queryExpression;
		/// <summary>
		///		For iterating trough the result of the query.
		/// </summary>
		private XPathNodeIterator queryNodeIterator;
		#endregion
		
		/// <summary>
		///		Constructor for the DefinitionQueryLibrary class.
		/// </summary>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Create a new DefinitionQueryLibrary.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			dqLibrary = new DefinitionQueryLibrary();
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Create a new DefinitionQueryLibrary.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary
		///			dqLibrary = new DefinitionQueryLibrary()
		///		</code>
		/// </example>
		public DefinitionQueryLibrary()
		{
			// Create an instance of the FileManager.
			dqFileManager = new FileManager();

			// Create an instance of the QueryManager.
			dqQueryManager = new QueryManager();

			// Init default SOP Class UID and System Name.
			sopClassUID = "";
			systemName = "";
		}

		#region FileManager Function Mapping
		/// <summary>
		///		Loads the Xml Definition file to make it available for
		///		executing XPath queries or Simple Query Functions.
		/// </summary>
		/// <param name="FileName">
		///		Full path to the Xml Definition File.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Load a Xml Definition File.
		///			
		///			DefinitionQueryLibrary sqLibrary;
		///			dqLibrary = new DefinitionQueryLibrary();
		///			
		///			dqLibrary.LoadFile(@"C:\example.xml");
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Load a Xml Definition File.
		///			
		///			Dim dqLibrary AS DefinitionQueryLibrary
		///			dqLibrary = new DefinitionQueryLibrary()
		///			
		///			dqLibrary.LoadFile(@"C:\example.xml")
		///		</code>
		///	</example>
		public void LoadFile(string FileName)
		{
			// Map to the PreLoadFile() function of the FileManager instance.
			dqFileManager.PreLoadFile(FileName);
		}

		/// <summary>
		///		Provides a list of all the Xml Definition files that are
		///		currently loaded.
		/// </summary>
		/// <returns>
		///		An Array of strings containing the full path of all Xml
		///		Definition files.
		/// </returns>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get Loaded Xml Definition files.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			Array myArray;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			dqLibrary.PreLoadFile(@"C:\example.xml");
		///			myArray = dqLibrary.GetLoadedFiles();
		///			
		///			foreach(string definitionFile in myArray)
		///			{
		///				System.Console.WriteLine(definitionFile);
		///			}
		///		</code>
		/// </example>
		public Array GetLoadedFiles()
		{
			return dqFileManager.GetLoadedFiles();
		}

		#endregion

		#region IXpathQueryInterface Implementation
		/// <summary>
		///		Execute a XPath Query against the Xml Definition file with
		///		the SOP Class UID and System Name that are set as default.
		/// </summary>
		/// <param name="Expression">XPath compatible query.</param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Execute a raw XPath Query on the default
		///			// SOP Class and System Name.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			XPathNodeIterator myNodeIterator;
		///			string myExpression;
		///			string version;
		///			
		///			dqLibrary = new DefinitionQueryLibrary();
		///			
		///			// Set the default SOP Class UID and System Name.
		///			dqLibrary.SetDefaultSOPClassUID("1.2.3.4.55.145.9.2.1");
		///			dqLibrary.SetDefaultSystemName("DICOM");
		///			
		///			// Create an expression that queries for the System
		///			// Version.
		///			myExpression = "/System/@Version";
		///			
		///			myNodeIterator = dqLibrary.Query(myExpression);
		///			
		///			// Move to the result.
		///			myNodeIterator.MoveNext();
		///			myNodeIterator.Current.MoveToFirstAttribute();
		///			
		///			// Show the result.
		///			Console.WriteLine(myNodeIterator.Current.Value);
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Execute a raw XPath Query on the default
		///			' SOP Class and System Name.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary
		///			Dim myNodeIterator As XPathNodeIterator
		///			Dim myExpression As string
		///			Dim version As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary()
		///			
		///			' Set the default SOP Class UID and System Name.
		///			dqLibrary.SetDefaultSOPClassUID("1.2.3.4.55.145.9.2.1")
		///			dqLibrary.SetDefaultSystemName("DICOM")
		///			
		///			' Create an expression that queries for the System Version.
		///			myExpression = "/System/@Version"
		///			
		///			myNodeIterator = dqLibrary.Query(myExpression)
		///			
		///			' Move to the result.
		///			myNodeIterator.MoveNext()
		///			myNodeIterator.Current.MoveToFirstAttribute()
		///			
		///			' Show the result.
		///			Console.WriteLine(myNodeIterator.Current.Value)
		///		</code>
		/// </example>
		/// <returns>
		///		A XPathNodeIterator with the query results.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the XPath query is invalid or the Xml
		///		Definition file is not loaded.
		/// </exception>
		public XPathNodeIterator Query(string Expression)
		{
			try
			{
				// Redirect the function call.
				return Query(Expression, sopClassUID, systemName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		///		Execute a XPath Query against the Xml Definition file with
		///		the provided filename.
		/// </summary>
		/// <param name="Expression">XPath compatible query.</param>
		/// <param name="FileName">
		///		Full path to the Xml Definition file. See also the example
		///		code.
		///	</param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Execute a raw XPath Query on the provided Xml
		///			// Definition file.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			XPathNodeIterator myNodeIterator;
		///			string myExpression;
		///			string version;
		///			
		///			dqLibrary = new DefinitionQueryLibrary();
		///			
		///			// Create an expression that queries for the System
		///			// Version.
		///			myExpression = "/System/@Version";
		///			
		///			myNodeIterator = dqLibrary.Query(myExpression, @"C:\example.xml");
		///			
		///			// Move to the result.
		///			myNodeIterator.MoveNext();
		///			myNodeIterator.Current.MoveToFirstAttribute();
		///			
		///			// Show the result.
		///			Console.WriteLine(myNodeIterator.Current.Value);
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Execute a raw XPath Query on the provided Xml
		///			' Definition file.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary
		///			Dim myNodeIterator As XPathNodeIterator
		///			Dim myExpression As string
		///			Dim version As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary()
		///			
		///			' Create an expression that queries for the System Version.
		///			myExpression = "/System/@Version"
		///			
		///			myNodeIterator = dqLibrary.Query(myExpression, @"C:\example.xml")
		///			
		///			' Move to the result.
		///			myNodeIterator.MoveNext()
		///			myNodeIterator.Current.MoveToFirstAttribute()
		///			
		///			' Show the result.
		///			Console.WriteLine(myNodeIterator.Current.Value)
		///		</code>
		/// </example>
		/// <returns>
		///		A XPathNodeIterator with the query results.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the XPath query is invalid or the Xml
		///		Definition file is not loaded.
		/// </exception>
		public XPathNodeIterator Query(string Expression, string FileName)
		{
			XPathDocument definitionFile;							// Holds a copy of the definition file from the dqFileManager.
			XPathNavigator definitionNavigator;						// Holds a new navigator instance for the definitionFile.
			XPathNodeIterator definitionNodeIterator;				// Holds the iterator of the query result.

			try
			{
				// Get the definition file from the dqFileManager.
				definitionFile = (XPathDocument) dqFileManager.GetFile(FileName);
				
				// Create a new navigator.
				definitionNavigator = definitionFile.CreateNavigator();
				
				// Execute the query and return the result.
				definitionNodeIterator = definitionNavigator.Select(Expression);
				return definitionNodeIterator;
			}
			catch (Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute XPath Query: " + Expression, ex);
				throw myException;
			}
		}

		/// <summary>
		///		Execute a XPath Query against the Xml Definition file with
		///		the provided SOP Class UID and System Name.
		/// </summary>
		/// <param name="Expression">XPath compatible query.</param>
		/// <param name="SOPClassUID">
		///		SOP Class UID of the Xml Definition file to run the query on.
		///		See also the example code.
		///	</param>
		///	<param name="SystemName">
		///		System Name of the Xml Definition file to run the query on. See
		///		also the example code.
		///	</param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Execute a raw XPath Query on the Xml Definition
		///			// file with the given SOP Class UID and System Name.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			XPathNodeIterator myNodeIterator;
		///			string myExpression;
		///			string version;
		///			
		///			dqLibrary = new DefinitionQueryLibrary();
		///			
		///			// Create an expression that queries for the System
		///			// Version.
		///			myExpression = "/System/@Version";
		///			
		///			myNodeIterator = dqLibrary.Query(myExpression, "1.2.545.877.144.9.2.1", "Dicom");
		///			
		///			// Move to the result.
		///			myNodeIterator.MoveNext();
		///			myNodeIterator.Current.MoveToFirstAttribute();
		///			
		///			// Show the result.
		///			Console.WriteLine(myNodeIterator.Current.Value);
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Execute a raw XPath Query on the Xml Definition
		///			' file with the given SOP Class UID and System Name.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary
		///			Dim myNodeIterator As XPathNodeIterator
		///			Dim myExpression As string
		///			Dim version As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary()
		///			
		///			' Create an expression that queries for the System Version.
		///			myExpression = "/System/@Version"
		///			
		///			myNodeIterator = dqLibrary.Query(myExpression, "1.2.545.877.144.9.2.1", "Dicom")
		///			
		///			' Move to the result.
		///			myNodeIterator.MoveNext()
		///			myNodeIterator.Current.MoveToFirstAttribute()
		///			
		///			' Show the result.
		///			Console.WriteLine(myNodeIterator.Current.Value)
		///		</code>
		/// </example>
		/// <returns>
		///		A XPathNodeIterator with the query results.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the XPath query is invalid or the Xml
		///		Definition file is not loaded.
		/// </exception>
		public XPathNodeIterator Query(string Expression, string SOPClassUID, string SystemName)
		{
			XPathDocument definitionFile;							// Holds a copy of the definition file from the dqFileManager.
			XPathNavigator definitionNavigator;						// Holds a new navigator instance for the definitionFile.
			XPathNodeIterator definitionNodeIterator;				// Holds the iterator of the query result.

			try
			{
				// Get the definition file from the dqFileManager.
				definitionFile = (XPathDocument) dqFileManager.GetFile(SOPClassUID, SystemName);
				
				// Create a new navigator.
				definitionNavigator = definitionFile.CreateNavigator();
				
				// Execute the query and return the result.
				definitionNodeIterator = definitionNavigator.Select(Expression);
				return definitionNodeIterator;
			}
			catch (Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute XPath Query: " + Expression, ex);
				throw myException;
			}
		}
		#endregion

		#region ISimpleQueryInterface Implementation
		#region Global Functions.

		/// <summary>
		///		Gets the SOP Class UID that is used in a query functions.
		/// </summary>
		/// <returns>
		///		A string containing the SOP Class UID.
		/// </returns>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the SOP Class UID.
		///			DefinitionQueryLibrary dqLibrary;
		///			string myUID;
		///			
		///			dqLibrary = new DefinitionQueryLibrary();
		///			myUID = dqLibrary.GetSOPClassUID();
		///			
		///			Console.WriteLine("The SOP Class UID is: " + myUID);
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the SOP Class UID.
		///			Dim dqLibrary As DefinitionQueryLibrary
		///			Dim myUID As string
		///			
		///			dqLibrary - new DefinitionQueryLibrary()
		///			myUID = dqLibrary.GetSOPClassUID()
		///			
		///			Console.WriteLine("The SOP Class UID is: " + myUID)
		///		</code>
		/// </example>
		public string GetSOPClassUID()
		{
			return sopClassUID;
		}
		
		/// <summary>
		///		Sets the SOP Class UID that is used in query functions.
		/// </summary>
		/// <param name="SOPClassUID">
		///		SOP Class UID. See also the code example.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Set the SOP Class UID.
		///			DefinitionQueryLibrary dqLibrary;
		///			
		///			dqLibrary = new DefinitionQueryLirbary();
		///			dqLibrary.SetSOPClassUID("1.2.3.8.14.65.88.1.2");
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Set the SOP Class UID.
		///			Dim dqLibrary As DefinitionQueryLibrary
		///			
		///			dqLibrary = new DefinitionQueryLirbary()
		///			dqLibrary.SetSOPClassUID("1.2.3.8.14.65.88.1.2")
		///		</code>
		/// </example>
		public void SetSOPClassUID(string SOPClassUID)
		{
			sopClassUID = SOPClassUID;
		}

		/// <summary>
		///		Gets the System Name that is used in query functions.
		/// </summary>
		/// <returns>
		///		A string containing the System Name.
		/// </returns>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the System Name.
		///			DefinitionQueryLibrary dqLibrary;
		///			string mySystemName;
		///			
		///			dqLibrary = new DefinitionQueryLibrary();
		///			mySystemName = dqLibrary.GetSystemName();
		///			
		///			Console.WriteLine("The System Name is: " + myUID);
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the System Name.
		///			Dim dqLibrary As DefinitionQueryLibrary
		///			Dim mySystemName As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary()
		///			mySystemName = dqLibrary.GetSystemName()
		///			
		///			Console.WriteLine("The System Name is: " + myUID)
		///		</code>
		/// </example>
		public string GetSystemName()
		{
			return systemName;
		}

		/// <summary>
		///		Sets the System Name that is used in query functions.
		/// </summary>
		/// <param name="SystemName">
		///		System Name. See also the code example.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Set the System Name.
		///			DefinitionQueryLibrary dqLibrary;
		///			
		///			dqLibrary = new DefinitionQueryLirbary();
		///			dqLibrary.SetSystemName("DICOM");
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Set the System Name.
		///			Dim dqLibrary As DefinitionQueryLibrary
		///			
		///			dqLibrary = new DefinitionQueryLirbary()
		///			dqLibrary.SetSystemName("DICOM")
		///		</code>
		/// </example>
		public void SetSystemName(string SystemName)
		{
			systemName = SystemName;
		}

		#endregion

		#region System/Application Related Function.
		/// <summary>
		///		Get the System Version from the Xml Definition file with the
		///		SOP Class UID and System Name that are set.
		/// </summary>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the System Version from the Xml Definition
		///			// file with the SOP Class UID and System Name that are
		///			// set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string systemVersion;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			systemVersion = dqLibrary.GetSystemVersion();
		///			
		///			if (systemVersion != null)
		///			{
		///				Console.WriteLine("The System Version = " + systemVersion);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the System Version from the Xml Definition
		///			' file with the SOP Class UID and System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim systemVersion As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			systemVersion = dqLibrary.GetSystemVersion()
		///			
		///			if (systemVersion &lt;&gt; nothing) then
		///				Console.WriteLine("The System Version = " + systemVersion)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		A string containing the System Version. If for some reason no
		///		System Version was found, null will be returned.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetSystemVersion()
		{
			string systemVersion;									// Holds the result.
			
			// Get the pre-compiled query from the dqQueryManager.
			queryExpression = (dqQueryManager.GetQuery("GetSystemVersion()")).Clone();
			
			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(queryExpression);

				// Extract the result with the queryNodeIterator.
				if (queryNodeIterator.MoveNext() == false)
				{
					systemVersion = null;
				}
				else
				{
					queryNodeIterator.Current.MoveToFirstAttribute();
					systemVersion = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetSystemVersion(). This can occur if the loaded Xml Definition file is incompleet or corrupt.", ex);
				throw myException;
			}

			// Return the result.
			return systemVersion;
		}

		/// <summary>
		///		Get the Application Name from the Xml Definition file with the
		///		SOP Class UID and System Name that are set.
		/// </summary>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Application Name from the Xml
		///			// Definition file with the SOP Class UID and System Name
		///			// that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string appplicationName;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			if (applicatioName != null)
		///			{
		///				applicationName = dqLibrary.GetApplicationName();
		///			}
		///			
		///			Console.WriteLine("The Application Name = " + applicationName);
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Application Name from the Xml Definition
		///			' file with the SOP Class UID and System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim applicationName As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			applicationName = dqLibrary.GetApplicationName()
		///			
		///			if (applicationName &lt;&gt; nothing) then
		///				Console.WriteLine("The Application Name = " + applicationName)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		A string containing the Application Name. If for some reason no
		///		Application Name was found, null will be returned.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetApplicationName()
		{
			string applicationName;									// Holds the result.
			
			// Get the pre-compiled query from the dqQueryManager.
			queryExpression = (dqQueryManager.GetQuery("GetApplicationName()")).Clone();
			
			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(queryExpression);

				// Extract the result with the queryNodeIterator.
				if (queryNodeIterator.MoveNext() == false)
				{
					applicationName = null;
				}
				else
				{
					queryNodeIterator.Current.MoveToFirstAttribute();
					applicationName = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetApplicationName(). This can occur if the loaded Xml Definition file is incompleet or corrupt.", ex);
				throw myException;
			}

			// Return the result.
			return applicationName;
		}

		#endregion

		#region Dimse-Command Related Functions.
		/// <summary>
		///		Get all Dimse Commands for the Xml Definition file with the SOP
		///		Class UID and System Name that are set.
		/// </summary>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get all Dimse Commands from the Xml Definition
		///			// file with the SOP Class UID and System Name that are
		///			// set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection dimseCommands;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			dimseCommands = dqLibrary.GetAllDimseCommands();
		///			
		///			// Show all the results by iterating trough the array.
		///			foreach(string dimseCommand in dimseCommands)
		///			{
		///				Console.WriteLine("The Dimse Command = " + dimseCommand);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get all Dimse Commands from the Xml Definition
		///			' file with the SOP Class UID and System Name that are set
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim dimseCommands As StringCollection
		///			Dim dimseCommand As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			dimseCommands = dqLibrary.GetAllDimseCommands()
		///			
		///			// Show all the results by iterating trough the array.
		///			For Each dimseCommand in dimseCommands
		///				Console.WriteLine("The Dimse Command = " + dimseCommand)
		///			Next dimseCommand
		///		</code>
		/// </example>
		/// <returns>
		///		A StringCollection containing all the Dimse Commands as string.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAllDimseCommands()
		{
			StringCollection dimseCommands = new StringCollection();	// Holds the result.
			
			// Get the pre-compiled query from the dqQueryManager.
			queryExpression = (dqQueryManager.GetQuery("GetAllDimseCommands()")).Clone();
			
			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(queryExpression);

				// Extract the result with the queryNodeIterator by iterating
				// trough the results and adding them to the ArrayList.
				while (queryNodeIterator.MoveNext())
				{
					queryNodeIterator.Current.MoveToFirstAttribute();
					dimseCommands.Add(queryNodeIterator.Current.Value);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAllDimseCommands(). This can occur if the loaded Xml Definition file is incompleet or corrupt.", ex);
				throw myException;
			}

			// Return the result.
			return dimseCommands;
		}

		#endregion

		#region Module Related Functions.
		/// <summary>
		///		Get all Modules for the Xml Definition file with the SOP Class
		///		UID and System Name that are set.
		/// </summary>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get all Modules from the Xml Definition file
		///			// with the SOP Class UID and System Name that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection modules;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			modules = dqLibrary.GetAllModules();
		///			
		///			// Show all the results by iterating trough the array.
		///			foreach(string module in modules)
		///			{
		///				Console.WriteLine("The Module = " + module);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get all Modules from the Xml Definition file
		///			' with the SOP Class UID and System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim modules As StringCollection
		///			Dim module As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			modules = dqLibrary.GetAllModules()
		///			
		///			' Show all the results by iterating trough the array.
		///			For Each module in modules
		///				Console.WriteLine("The Module = " + module)
		///			Next module
		///		</code>
		/// </example>
		/// <returns>
		///		A StringCollection containing all the Modules as string.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAllModules()
		{
			StringCollection modules = new StringCollection();			// Holds the result.
			
			// Get the pre-compiled query from the dqQueryManager.
			queryExpression = (dqQueryManager.GetQuery("GetAllModules()")).Clone();
			
			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(queryExpression);

				// Extract the result with the queryNodeIterator by iterating
				// trough the results and adding them to the ArrayList.
				while (queryNodeIterator.MoveNext())
				{
					queryNodeIterator.Current.MoveToFirstAttribute();
					modules.Add(queryNodeIterator.Current.Value);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAllModules(). This can occur if the loaded Xml Definition file is incompleet or corrupt.", ex);
				throw myException;
			}

			// Return the result.
			return modules;
		}

		/// <summary>
		///		Get all Modules for the specific <paramref name="DimseCommand">
		///		Dimse Command</paramref> for the Xml Definition file with the
		///		SOP Class UID and System Name that are set.
		/// </summary>
		/// <param name="DimseCommand">
		///		The Dimse Command. See also the example code.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get all Modules for the specific Dimse Command
		///			// from the Xml Definition file with the SOP Class UID and
		///			// System Name that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection modules;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			modules = dqLibrary.GetAllModules("C-STORE-RQ");
		///			
		///			// Show all the results by iterating trough the array.
		///			foreach(string module in modules)
		///			{
		///				Console.WriteLine("The C-STORE-RQ Dimse Command has the Module = " + module);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get all Modules for the specific Dimse Command
		///			' from the Xml Definition file with the SOP Class UID and
		///			' System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim modules As StringCollection
		///			Dim module As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			modules = dqLibrary.GetAllModules("C-STORE-RQ")
		///			
		///			// Show all the results by iterating trough the array.
		///			For Each module in modules
		///				Console.WriteLine("The C-STORE-RQ Dimse Command has the Module = " + module)
		///			Next module
		///		</code>
		/// </example>
		/// <returns>
		///		A StringCollection containing all the Modules of the Dimse Command as
		///		string.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAllModules(string DimseCommand)
		{
			StringCollection modules = new StringCollection();			// Holds the result.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select("/System/*/SOPClass/Dimse[@Name = '" + DimseCommand + "']/Dataset/Module/@Name");

				// Extract the result with the queryNodeIterator by iterating
				// trough the results and adding them to the ArrayList.
				while (queryNodeIterator.MoveNext())
				{
					queryNodeIterator.Current.MoveToFirstAttribute();
					modules.Add(queryNodeIterator.Current.Value);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAllModules(string DimseCommand). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter DimseCommand contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return modules;
		}

		/// <summary>
		///		Get the Usage information for the Module specified in
		///		<paramref name="ModuleName">ModuleName</paramref> from the Xml
		///		Definition file with the SOP Class UID and System Name that are
		///		set.
		/// </summary>
		/// <param name="ModuleName">
		///		The Module Name to get the Usage for. See also the example
		///		code.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Module Usage for the specific Module
		///			// from the Xml Definition file with the SOP Class UID and
		///			// System Name that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string usage;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			usage = dqLibrary.GetModuleUsage("Patient Module");
		///			
		///			// Show the result.
		///			if (usage != null)
		///			{
		///				Console.WriteLine("The Usage for the Patient Module = " + usage);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Module Usage for the specific Module
		///			' from the Xml Definition file with the SOP Class UID and
		///			' System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim usage As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			usage = dqLibrary.GetModuleUsage("Patient Module")
		///			
		///			// Show the result.
		///			if (usage &lt;&gt; nothing) then
		///				Console.WriteLine("The Usage for the Patient Module = " + usage)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		A string containing the module usage. If the Module
		///		<paramref name="ModuleName">ModuleName</paramref> was not found
		///		null will be returned for Module Usage.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetModuleUsage(string ModuleName)
		{
			string moduleUsage;										// Holds the result.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select("/System/*/SOPClass/Dimse/Dataset/Module[@Name = '" + ModuleName + "']/Usage");

				// Extract the result with the queryNodeIterator.
				if (queryNodeIterator.MoveNext() == false)
				{

					moduleUsage = null;
				}
				else
				{
					moduleUsage = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetModuleUsage(string ModuleName). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter DimseCommand contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return moduleUsage;
		}

		#endregion

		#region Attribute Related Functions.
		/// <summary>
		///		Get all Attributes for the Xml Definition file with the SOP
		///		Class UID and System Name that are set. If the
		///		<paramref name="Subitems">Subitems</paramref> is set to True
		///		also subitems will be included.
		/// </summary>
		/// <param name="Subitems">
		///		Determines whether subitems are also included. See also the
		///		example code.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get all Attributes from the Xml Definition file
		///			// with the SOP Class UID and System Name that are set. Not
		///			// including subitems.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection attributes;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			attributes = dqLibrary.GetAllAttributes(false);
		///			
		///			// Show all the results by iterating trough the array.
		///			foreach(string attribute in attributes)
		///			{
		///				Console.WriteLine("The Attribute = " + attribute);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get all Attributes from the Xml Definition file
		///			' with the SOP Class UID and System Name that are set. Not
		///			' including subitems.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributes As StringCollection
		///			Dim attribute As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			attributes = dqLibrary.GetAllAttributes(false)
		///			
		///			// Show all the results by iterating trough the array.
		///			For Each attribute in attributes
		///				Console.WriteLine("The Attribute = " + attribute)
		///			Next attribute
		///		</code>
		/// </example>
		/// <returns>
		///		A StringCollection containing all the Attributes as string. The
		///		format of the string will be like 0x012345678.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAllAttributes(bool Subitems)
		{
			StringCollection attributes = new StringCollection();		// Holds the result.

			// Get the pre-compiled query from the dqQueryManager.
			if (Subitems == true)
			{
				queryExpression = (dqQueryManager.GetQuery("GetAllAttributes(subitems = true)")).Clone();
			}
			else
			{
				queryExpression = (dqQueryManager.GetQuery("GetAllAttributes(subitems = false)")).Clone();
			}

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(queryExpression);

				// Extract the result with the queryNodeIterator by iterating
				// trough the results and adding them to the ArrayList.
				while (queryNodeIterator.MoveNext())
				{
					string group = queryNodeIterator.Current.Value;
					queryNodeIterator.Current.MoveToNext();
					string element = queryNodeIterator.Current.Value;
					attributes.Add("0x" + group + element);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAllAttributes(bool Subitems). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter DimseCommand contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributes;
		}

		/// <summary>
		///		Get all Attributes for the specific Dimse Command for the Xml
		///		Definition file with the SOP Class UID and System Name that are
		///		set. This also includes attributes that are nested in
		///		sequences. If the <paramref name="Subitems">Subitems</paramref>
		///		is set to True also subitems will be included.
		/// </summary>
		/// <param name="DimseCommand">
		///		The Dimse Command containing the Attributes. See also the
		///		example code.
		/// </param>
		/// <param name="Subitems">
		///		Determines whether subitems are also included. See also the
		///		example code.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get all Attributes for the specific Dimse 
		///			// Command from the Xml Definition file with the SOP
		///			// Class UID and System Name that are set. Not including
		///			// subitems.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection attributes;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			attributes = dqLibrary.GetAllAttributesInDimseCommand("C-STORE-RQ", false);
		///			
		///			// Show all the results by iterating trough the array.
		///			foreach(string attribute in attributes)
		///			{
		///				Console.WriteLine("The Attribute = " + attribute);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get all Attributes for the specific Dimse
		///			' Command from the Xml Definition file with the SOP Class
		///			' UID and System Name that are set. Not including subitems.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributes As StringCollection
		///			Dim attribute As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			attributes = dqLibrary.GetAllAttributesInDimseCommand("C-STORE-RQ", false)
		///			
		///			// Show all the results by iterating trough the array.
		///			For Each attribute in attributes
		///				Console.WriteLine("The Attribute = " + attribute)
		///			Next attribute
		///		</code>
		/// </example>
		/// <returns>
		///		A StringCollection containing all the Attributes as string. The
		///		format of the string will be like 0x012345678.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAllAttributesInDimseCommand(string DimseCommand, bool Subitems)
		{
			StringCollection attributes = new StringCollection();		// Holds the result.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			try
			{
				// Execute the query.
				if (Subitems == true)
				{
					queryNodeIterator = queryNavigator.Select("/System/*/SOPClass/Dimse[@Name='" + DimseCommand + "']/Dataset/Module//Attribute/Group");
				}
				else
				{
					queryNodeIterator = queryNavigator.Select("/System/*/SOPClass/Dimse[@Name='" + DimseCommand + "']/Dataset/Module/Attribute/Group");
				}

				// Extract the result with the queryNodeIterator by iterating
				// trough the results and adding them to the ArrayList.
				while (queryNodeIterator.MoveNext())
				{
					string group = queryNodeIterator.Current.Value;
					queryNodeIterator.Current.MoveToNext();
					string element = queryNodeIterator.Current.Value;
					attributes.Add("0x" + group + element);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAllAttributesInDimseCommand(string DimseCommand, bool Subitems). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter DimseCommand contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributes;
		}

		/// <summary>
		///		Get all Attributes for the specific Module for the Xml
		///		Definition file with the SOP Class UID and System Name that are
		///		set. This also includes attributes that are nested in
		///		sequences. If the <paramref name="Subitems">Subitems</paramref>
		///		is set to True also subitems will be included.
		/// </summary>
		/// <param name="Module">
		///		The Module containing the Attributes. See also the example
		///		code.
		/// </param>
		/// <param name="Subitems">
		///		Determines whether subitems are also included. See also the
		///		example code.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get all Attributes for the specific Module from
		///			// the Xml Definition file with the SOP Class UID and
		///			// System Name that are set. Not including subitems.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection attributes;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			attributes = dqLibrary.GetAllAttributesInModule("Patient Module", false);
		///			
		///			// Show all the results by iterating trough the array.
		///			foreach(string attribute in attributes)
		///			{
		///				Console.WriteLine("The Attribute = " + attribute);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get all Attributes for the specific Module from
		///			' the Xml Definition file with the SOP Class UID and System
		///			' Name that are set. Not including subitems.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributes As StringCollection
		///			Dim attribute As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			attributes = dqLibrary.GetAllAttributesInModule("Patient Module", false)
		///			
		///			// Show all the results by iterating trough the array.
		///			For Each attribute in attributes
		///				Console.WriteLine("The Attribute = " + attribute)
		///			Next attribute
		///		</code>
		/// </example>
		/// <returns>
		///		A StringCollection containing all the Attributes as string. The
		///		format of the string will be like 0x012345678.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAllAttributesInModule(string Module, bool Subitems)
		{
			StringCollection attributes = new StringCollection();		// Holds the result.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			try
			{
				// Execute the query.
				if (Subitems == true)
				{
					queryNodeIterator = queryNavigator.Select("/System/*/SOPClass/Dimse/Dataset/Module[@Name='" + Module + "']//Attribute/Group");
				}
				else
				{
					queryNodeIterator = queryNavigator.Select("/System/*/SOPClass/Dimse/Dataset/Module[@Name='" + Module + "']/Attribute/Group");
				}

				// Extract the result with the queryNodeIterator by iterating
				// trough the results and adding them to the ArrayList.
				while (queryNodeIterator.MoveNext())
				{
					string group = queryNodeIterator.Current.Value;
					queryNodeIterator.Current.MoveToNext();
					string element = queryNodeIterator.Current.Value;
					attributes.Add("0x" + group + element);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAllAttributesInModule(string Module, bool Subitems). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter DimseCommand contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributes;
		}

		/// <summary>
		///		Get the Attributes in the <paramref name="Attribute">
		///		Attribute</paramref> using the SOP Class UID and System Name
		///		that are set. With <paramref name="Recursive">Recursive
		///		</paramref> it is also possible to look inside other
		///		Attributes. If the <paramref name="Subitems">Subitems
		///		</paramref> is set to True also subitems will be included.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Enumerated Values for in
		///		the 0x12345678 or 0x10101010/0x20202020/0x30303030 format. See
		///		also the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <param name="Subitems">
		///		Determines whether subitems are also included. See also the
		///		example code.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attributes in the given Attribute
		///			// using the SOP Class UID and System Name that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection attributes;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute, look also inside other Attributes (Recursive)
		///			// and return all sub Attributes.
		///			attributes = dqLibrary.GetAllAttributesinAttribute("0x00101010", true, true);
		///			
		///			// Show the result.
		///			foreach (string attribute in attributes)
		///			{
		///				Console.WriteLine("The Attribute 0x00101010 contains Attribute = " + attribute);
		///			}
		///			
		///			// Attribute path, don't look inside other Attributes (Recursive)
		///			// but return all sub Attributes.
		///			attributes = dqLibrary.GetAllAttributesinAttribute("0x00101010/0x20205555", false, true);
		///			
		///			// Show the result.
		///			foreach (string attribute in attributes)
		///			{
		///				Console.WriteLine("The Attribute 0x20205555 contains Attribute = " + attribute);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attributes in the given Attribute
		///			' using the SOP Class UID and System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributes As StringCollection
		///			Dim attribute As String
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute, look also inside other Attributes (Recursive)
		///			' and return all sub Attributes.
		///			attributes = dqLibrary.GetAllAttributesinAttribute("0x00101010", true, true)
		///			
		///			' Show the result.
		///			For Each attribute in attributes
		///				Console.WriteLine("The Attribute 0x00101010 contains Attribute = " + attribute)
		///			Next attribute
		///			
		///			' Attribute path, don't look inside other Attributes (Recursive)
		///			' but return all sub Attributes.
		///			attributes = dqLibrary.GetAllAttributesinAttribute("0x00101010/0x20205555", false, true)
		///			
		///			' Show the result.
		///			For Each attribute in attributes
		///				Console.WriteLine("The Attribute 0x20205555 contains Attribute = " + attribute)
		///			Next attribute
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a StringCollection containing the Attributes as String.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAllAttributesInAttribute(string Attribute, bool Recursive, bool Subitems)
		{
			StringCollection attributes = new StringCollection();		// Holds the result.
			string query;												// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true && Subitems == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "//Values/Sequence/Item/Attribute/Group";
			}
			else if (Recursive == true && Subitems == false)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/Values/Sequence/Item/Attribute/Group";
			}
			else if (Recursive == false && Subitems == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "//Values/Sequence/Item/Attribute/Group";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/Values/Sequence/Item/Attribute/Group";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Extract the result with the queryNodeIterator by iterating
				// trough the results and adding them to the ArrayList.
				while (queryNodeIterator.MoveNext())
				{
					string group = queryNodeIterator.Current.Value;
					queryNodeIterator.Current.MoveToNext();
					string element = queryNodeIterator.Current.Value;
					attributes.Add("0x" + group + element);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAllAttributes(bool Subitems). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter DimseCommand contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributes;
		}

		/// <summary>
		///		Get a list of all the Attributes as tree. See also the example
		///		code.
		/// </summary>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute tree.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection attributes;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			attributes = dqLibrary.GetAttributeTree();
		///			
		///			// Show the result.
		///			foreach (string attribute in attributes)
		///			{
		///				Console.WriteLine(attribute);
		///			}
		///			
		///			// This will output:
		///			//
		///			// 0x01234567
		///			// 0x01234567/0x15465478
		///			// 0x01234567/0x25478963
		///			// 0x01234567/0x65487632
		///			// 0x01234567/0x65487632/0x12547885
		///			// 0x01234567/0x65487632/0x12547896
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute tree.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributes As StringCollection
		///			Dim attribute As String
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			attributes = dqLibrary.GetAttributeTree()
		///			
		///			' Show the result.
		///			For Each attribute in attributes
		///				Console.WriteLine(attribute)
		///			Next attribute
		///			
		///			' This will output:
		///			'
		///			' 0x01234567
		///			' 0x01234567/0x15465478
		///			' 0x01234567/0x25478963
		///			' 0x01234567/0x65487632
		///			' 0x01234567/0x65487632/0x12547885
		///			' 0x01234567/0x65487632/0x12547896
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a StringCollection containing the Attribute tree.
		/// </returns>
		public StringCollection GetAttributeTree()
		{
			StringCollection attributes = new StringCollection();		// Holds the result.
			
			// Get all root Attributes.
			foreach (string currentAttribute in GetAllAttributes(false))
			{
				// Call the recursive support function GetAttributeTree() for
				// all root Attributes.
				foreach (string currentSubAttribute in GetAttributeTree(currentAttribute))
				{
					// Add to the results.
					attributes.Add(currentSubAttribute);
				}
			}

			// Return the result.
			return attributes;
		}

		/// <summary>
		///		Get the Type for the <paramref name="Attribute">Attribute
		///		</paramref> using the SOP Class UID and System Name that are
		///		set. With <paramref name="Recursive">Recursive</paramref> it is
		///		also possible to look inside other Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Type for in the 
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Type for the given Attribute
		///			// using the SOP Class UID and System Name that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeType;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeType = dqLibrary.GetAttributeType("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeType != null)
		///			{
		///				Console.WriteLine("The Attribute Type for Attribute 0x00101010 = " + attributeType);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeType = dqLibrary.GetAttributeType("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeType != null)
		///			{
		///				Console.WriteLine("The Attribute Type for Attribute 0x20205555 = " + attributeType);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Type for the given Attribute
		///			' using the SOP Class UID and System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeType As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeType = dqLibrary.GetAttributeType("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeType &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Type for Attribute 0x00101010 = " + attributeType)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeType = dqLibrary.GetAttributeType("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeType &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Type for Attribute 0x20205555 = " + attributeType)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Type. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		null wil be returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeType(string Attribute, bool Recursive)
		{
			string attributeType;									// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/Type";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/Type";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeType = null;
				}
				// Extract the result with the queryNodeIterator.
				else
				{
					attributeType = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeType(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeType;
		}

		/// <summary>
		///		Get the VM for the <paramref name="Attribute">Attribute
		///		</paramref> using the SOP Class UID and System Name that are
		///		set. With <paramref name="Recursive">Recursive</paramref> it is
		///		also possible to look inside other Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the VM for in the 
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute VM for the given Attribute
		///			// using the SOP Class UID and System Name that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeVM;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeVM = dqLibrary.GetAttributeVM("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeVM != null)
		///			{
		///				Console.WriteLine("The Attribute VM for Attribute 0x00101010 = " + attributeVM);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeVM = dqLibrary.GetAttributeVM("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeVM != null)
		///			{
		///				Console.WriteLine("The Attribute VM for Attribute 0x20205555 = " + attributeVM);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute VM for the given Attribute
		///			' using the SOP Class UID and System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeVM As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeVM = dqLibrary.GetAttributeVM("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeVM &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute VM for Attribute 0x00101010 = " + attributeVM)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeVM = dqLibrary.GetAttributeVM("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeVM &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute VM for Attribute 0x20205555 = " + attributeVM)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the VM. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		null wil be returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeVM(string Attribute, bool Recursive)
		{
			string attributeVM;										// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/VM";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/VM";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeVM = null;
				}
				// Extract the result with the queryNodeIterator.
				else
				{
					attributeVM = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeVM(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeVM;
		}

		/// <summary>
		///		Get the VR for the <paramref name="Attribute">Attribute
		///		</paramref> using the SOP Class UID and System Name that are
		///		set. With <paramref name="Recursive">Recursive</paramref> it is
		///		also possible to look inside other Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the VR for in the 
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute VR for the given Attribute
		///			// using the SOP Class UID and System Name that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeVR;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeVR = dqLibrary.GetAttributeVR("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeVR != null)
		///			{
		///				Console.WriteLine("The Attribute VR for Attribute 0x00101010 = " + attributeVR);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeVR = dqLibrary.GetAttributeVR("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeVR != null)
		///			{
		///				Console.WriteLine("The Attribute VR for Attribute 0x20205555 = " + attributeVR);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute VR for the given Attribute
		///			' using the SOP Class UID and System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeVR As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeVR = dqLibrary.GetAttributeVR("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeVR &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute VR for Attribute 0x00101010 = " + attributeVR)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeVR = dqLibrary.GetAttributeVR("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeVR &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute VR for Attribute 0x20205555 = " + attributeVR)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the VR. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		null wil be returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeVR(string Attribute, bool Recursive)
		{
			string attributeVR;										// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/VR";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/VR";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeVR = null;
				}
				// Extract the result with the queryNodeIterator.
				else
				{
					attributeVR = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeVR(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeVR;
		}

		/// <summary>
		///		Get the Condition for the <paramref name="Attribute">Attribute
		///		</paramref> using the SOP Class UID and System Name that are
		///		set. With <paramref name="Recursive">Recursive</paramref> it is
		///		also possible to look inside other Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Condition for in the 
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Condition for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeCondition;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeCondition = dqLibrary.GetAttributeCondition("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeCondition != null)
		///			{
		///				Console.WriteLine("The Attribute Condition for Attribute 0x00101010 = " + attributeCondition);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeCondition = dqLibrary.GetAttributeCondition("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeCondition != null)
		///			{
		///				Console.WriteLine("The Attribute Condition for Attribute 0x20205555 = " + attributeCondition);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Condition for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeCondition As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeCondition = dqLibrary.GetAttributeCondition("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeCondition &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Condition for Attribute 0x00101010 = " + attributeCondition)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeCondition = dqLibrary.GetAttributeCondition("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeCondition &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Condition for Attribute 0x20205555 = " + attributeCondition)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Condition. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		or there is no Condition for the <paramref name="Attribute">
		///		Attribute</paramref> null wil be returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeCondition(string Attribute, bool Recursive)
		{
			string attributeCondition;								// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/Condition";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/Condition";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeCondition = null;
				}
				// Extract the result with the queryNodeIterator.
				else
				{
					attributeCondition = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeCondition(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeCondition;
		}

		/// <summary>
		///		Get the Comment for the <paramref name="Attribute">Attribute
		///		</paramref> using the SOP Class UID and System Name that are
		///		set. With <paramref name="Recursive">Recursive</paramref> it is
		///		also possible to look inside other Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the COmment for in the 
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Comment for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeComment;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeComment = dqLibrary.GetAttributeComment("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeComment != null)
		///			{
		///				Console.WriteLine("The Attribute Comment for Attribute 0x00101010 = " + attributeComment);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeComment = dqLibrary.GetAttributeComment("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeComment != null)
		///			{
		///				Console.WriteLine("The Attribute Comment for Attribute 0x20205555 = " + attributeComment);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Comment for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeComment As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeComment = dqLibrary.GetAttributeComment("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeComment &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Comment for Attribute 0x00101010 = " + attributeComment)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeComment = dqLibrary.GetAttributeComment("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeComment &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Comment for Attribute 0x20205555 = " + attributeComment)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Comment. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		or there is no Comment for the <paramref name="Attribute">
		///		Attribute</paramref> null wil be returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeComment(string Attribute, bool Recursive)
		{
			string attributeComment;								// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/Comment";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/Comment";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeComment = null;
				}
				// Extract the result with the queryNodeIterator.
				else
				{
					attributeComment = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeComment(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeComment;
		}

		/// <summary>
		///		Get the Name for the <paramref name="Attribute">Attribute
		///		</paramref> using the SOP Class UID and System Name that are
		///		set. With <paramref name="Recursive">Recursive</paramref> it is
		///		also possible to look inside other Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Name for in the 
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Name for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeName;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeName = dqLibrary.GetAttributeName("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeName != null)
		///			{
		///				Console.WriteLine("The Attribute Name for Attribute 0x00101010 = " + attributeName);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeName = dqLibrary.GetAttributeName("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeName != null)
		///			{
		///				Console.WriteLine("The Attribute Name for Attribute 0x20205555 = " + attributeName);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Name for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeName As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeName = dqLibrary.GetAttributeName("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeName &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Name for Attribute 0x00101010 = " + attributeName)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeName = dqLibrary.GetAttributeName("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeName &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Name for Attribute 0x20205555 = " + attributeName)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Name. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		null wil be returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeName(string Attribute, bool Recursive)
		{
			string attributeName;									// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/@Name";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/@Name";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeName = null;
				}
				// Extract the result with the queryNodeIterator.
				else
				{
					queryNodeIterator.Current.MoveToNextAttribute();
					attributeName = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeName(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeName;
		}

		/// <summary>
		///		Get the Source for the <paramref name="Attribute">Attribute
		///		</paramref> using the SOP Class UID and System Name that are
		///		set. With <paramref name="Recursive">Recursive</paramref> it is
		///		also possible to look inside other Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Source for in the 
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Source for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeSource;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeSource = dqLibrary.GetAttributeSource("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeSource != null)
		///			{
		///				Console.WriteLine("The Attribute Source for Attribute 0x00101010 = " + attributeSource);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeSource = dqLibrary.GetAttributeSource("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeSource != null)
		///			{
		///				Console.WriteLine("The Attribute Source for Attribute 0x20205555 = " + attributeSource);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Source for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeSource As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeSource = dqLibrary.GetAttributeSource("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeSource &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Source for Attribute 0x00101010 = " + attributeSource)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeSource = dqLibrary.GetAttributeSource("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeSource &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Source for Attribute 0x20205555 = " + attributeSource)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Source. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		or there is no Source for the <paramref name="Attribute">
		///		Attribute</paramref> null wil be returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeSource(string Attribute, bool Recursive)
		{
			string attributeSource;									// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/Source";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/Source";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeSource = null;
				}
					// Extract the result with the queryNodeIterator.
				else
				{
					attributeSource = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeSource(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeSource;
		}

		/// <summary>
		///		Get the Presence Of Value for the <paramref name="Attribute">
		///		Attribute</paramref> using the SOP Class UID and System Name
		///		that are set. With <paramref name="Recursive">Recursive
		///		</paramref> it is also possible to look inside other
		///		Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Presence Of Value for in
		///		the 0x12345678 or 0x10101010/0x20202020/0x30303030 format. See
		///		also the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Presence Of Value for the
		///			// given Attribute using the SOP Class UID and System Name
		///			// that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributePresenceOfValue;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeSource = dqLibrary.GetAttributePresenceOfValue("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributePresenceOfValue != null)
		///			{
		///				Console.WriteLine("The Attribute Presence Of Value for Attribute 0x00101010 = " + attributePresenceOfValue);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributePresenceOfValue = dqLibrary.GetAttributePresenceOfValue("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributePresenceOfValue != null)
		///			{
		///				Console.WriteLine("The Attribute Presence Of Value for Attribute 0x20205555 = " + attributePresenceOfValue);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Presence Of Value for the
		///			' given Attribute using the SOP Class UID and System Name
		///			' that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributePresenceOfValue As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributePresenceOfValue = dqLibrary.GetAttributePresenceOfValue("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributePresenceOfValue &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Presence Of Value for Attribute 0x00101010 = " + attributePresenceOfValue)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributePresenceOfValue = dqLibrary.GetAttributePresenceOfValue("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributePresenceOfValue &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Presence Of Value for Attribute 0x20205555 = " + attributePresenceOfValue)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Presence Of Value. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		or there is no Presence Of Value for the
		///		<paramref name="Attribute">Attribute</paramref> null wil be
		///		returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributePresenceOfValue(string Attribute, bool Recursive)
		{
			string attributePresenceOfValue;						// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/PresenceOfValue";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/PresenceOfValue";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributePresenceOfValue = null;
				}
					// Extract the result with the queryNodeIterator.
				else
				{
					attributePresenceOfValue = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributePresenceOfValue(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributePresenceOfValue;
		}

		/// <summary>
		///		Get the Matching Key for the <paramref name="Attribute">
		///		Attribute</paramref> using the SOP Class UID and System Name
		///		that are set. With <paramref name="Recursive">Recursive
		///		</paramref> it is also possible to look inside other
		///		Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Matching Key for in
		///		the 0x12345678 or 0x10101010/0x20202020/0x30303030 format. See
		///		also the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Matching Key for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeMatchingKey;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeMatchingKey = dqLibrary.GetAttributeMatchingKey("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeMatchingKey != null)
		///			{
		///				Console.WriteLine("The Attribute Matching Key for Attribute 0x00101010 = " + attributeMatchingKey);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeMatchingKey = dqLibrary.GetAttributeMatchingKey("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeMatchingKey != null)
		///			{
		///				Console.WriteLine("The Attribute Matching Key for Attribute 0x20205555 = " + attributeMatchingKey);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Matching Key for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeMatchingKey As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeMatchingKey = dqLibrary.GetAttributeMatchingKey("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeMatchingKey &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Matching Key for Attribute 0x00101010 = " + attributeMatchingKey)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeMatchingKey = dqLibrary.GetAttributeMatchingKey("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeMatchingKey &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Matching Key for Attribute 0x20205555 = " + attributeMatchingKey)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Matching Key. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		or there is no Matching Key for the
		///		<paramref name="Attribute">Attribute</paramref> null wil be
		///		returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeMatchingKey(string Attribute, bool Recursive)
		{
			string attributeMatchingKey;							// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/MatchingKey";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/MatchingKey";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeMatchingKey = null;
				}
					// Extract the result with the queryNodeIterator.
				else
				{
					attributeMatchingKey = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeMatchingKey(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeMatchingKey;
		}

		/// <summary>
		///		Get the Return Key for the <paramref name="Attribute">
		///		Attribute</paramref> using the SOP Class UID and System Name
		///		that are set. With <paramref name="Recursive">Recursive
		///		</paramref> it is also possible to look inside other
		///		Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Return Key for in the
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Return Key for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeReturnKey;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeReturnKey = dqLibrary.GetAttributeReturnKey("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeReturnKey != null)
		///			{
		///				Console.WriteLine("The Attribute Return Key for Attribute 0x00101010 = " + attributeReturnKey);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeReturnKey = dqLibrary.GetAttributeReturnKey("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeReturnKey != null)
		///			{
		///				Console.WriteLine("The Attribute Return Key for Attribute 0x20205555 = " + attributeReturnKey);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Return Key for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeReturnKey As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeReturnKey = dqLibrary.GetAttributeReturnKey("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeReturnKey &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Return Key for Attribute 0x00101010 = " + attributeReturnKey)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeReturnKey = dqLibrary.GetAttributeReturnKey("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeReturnKey &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Return Key for Attribute 0x20205555 = " + attributeReturnKey)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Return Key. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		or there is no Return Key for the
		///		<paramref name="Attribute">Attribute</paramref> null wil be
		///		returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeReturnKey(string Attribute, bool Recursive)
		{
			string attributeReturnKey;								// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/ReturnKey";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/ReturnKey";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeReturnKey = null;
				}
					// Extract the result with the queryNodeIterator.
				else
				{
					attributeReturnKey = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeReturnKey(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeReturnKey;
		}

		/// <summary>
		///		Get the Query Key for the <paramref name="Attribute">
		///		Attribute</paramref> using the SOP Class UID and System Name
		///		that are set. With <paramref name="Recursive">Recursive
		///		</paramref> it is also possible to look inside other
		///		Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Query Key for in the
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Query Key for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeQueryKey;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeQueryKey = dqLibrary.GetAttributeQueryKey("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeQueryKey != null)
		///			{
		///				Console.WriteLine("The Attribute Query Key for Attribute 0x00101010 = " + attributeQueryKey);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeQueryKey = dqLibrary.GetAttributeQueryKey("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeQueryKey != null)
		///			{
		///				Console.WriteLine("The Attribute Query Key for Attribute 0x20205555 = " + attributeQueryKey);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Query Key for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeQueryKey As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeQueryKey = dqLibrary.GetAttributeQueryKey("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeQueryKey &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Query Key for Attribute 0x00101010 = " + attributeQueryKey)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeQueryKey = dqLibrary.GetAttributeQueryKey("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeQueryKey &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Query Key for Attribute 0x20205555 = " + attributeQueryKey)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Query Key. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		or there is no Query Key for the
		///		<paramref name="Attribute">Attribute</paramref> null wil be
		///		returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeQueryKey(string Attribute, bool Recursive)
		{
			string attributeQueryKey;								// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/QueryKey";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/QueryKey";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeQueryKey = null;
				}
					// Extract the result with the queryNodeIterator.
				else
				{
					attributeQueryKey = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeQueryKey(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeQueryKey;
		}

		/// <summary>
		///		Get the Displayed Key for the <paramref name="Attribute">
		///		Attribute</paramref> using the SOP Class UID and System Name
		///		that are set. With <paramref name="Recursive">Recursive
		///		</paramref> it is also possible to look inside other
		///		Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Displayed Key for in the
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Displayed Key for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeDisplayedKey;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeDisplayedKey = dqLibrary.GetAttributeDisplayedKey("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeDisplayedKey != null)
		///			{
		///				Console.WriteLine("The Attribute Displayed Key for Attribute 0x00101010 = " + attributeDisplayedKey);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeDisplayedKey = dqLibrary.GetAttributeDisplayedKey("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeDisplayedKey != null)
		///			{
		///				Console.WriteLine("The Attribute Displayed Key for Attribute 0x20205555 = " + attributeDisplayedKey);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Displayed Key for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeDisplayedKey As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeDisplayedKey = dqLibrary.GetAttributeDisplayedKey("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeDisplayedKey &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Displayed Key for Attribute 0x00101010 = " + attributeDisplayedKey)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeDisplayedKey = dqLibrary.GetAttributeDisplayedKey("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeDisplayedKey &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Displayed Key for Attribute 0x20205555 = " + attributeDisplayedKey)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Displayed Key. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		or there is no Displayed Key for the
		///		<paramref name="Attribute">Attribute</paramref> null wil be
		///		returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeDisplayedKey(string Attribute, bool Recursive)
		{
			string attributeDisplayedKey;							// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/DisplayedKey";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/DisplayedKey";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeDisplayedKey = null;
				}
					// Extract the result with the queryNodeIterator.
				else
				{
					attributeDisplayedKey = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeDisplayedKey(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeDisplayedKey;
		}

		/// <summary>
		///		Get the IOD for the <paramref name="Attribute">Attribute
		///		</paramref> using the SOP Class UID and System Name that are
		///		set. With <paramref name="Recursive">Recursive</paramref> it is
		///		also possible to look inside other Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the IOD for in the 
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute IOD for the given Attribute
		///			// using the SOP Class UID and System Name that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeIOD;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeIOD = dqLibrary.GetAttributeIOD("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeIOD != null)
		///			{
		///				Console.WriteLine("The Attribute IOD for Attribute 0x00101010 = " + attributeIOD);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeIOD = dqLibrary.GetAttributeIOD("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeIOD != null)
		///			{
		///				Console.WriteLine("The Attribute IOD for Attribute 0x20205555 = " + attributeIOD);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute IOD for the given Attribute
		///			' using the SOP Class UID and System Name that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeIOD As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeIOD = dqLibrary.GetAttributeIOD("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeIOD &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute IOD for Attribute 0x00101010 = " + attributeIOD)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeIOD = dqLibrary.GetAttributeIOD("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeIOD &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute IOD for Attribute 0x20205555 = " + attributeIOD)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the IOD. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		null wil be returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeIOD(string Attribute, bool Recursive)
		{
			string attributeIOD;									// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/IOD";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/IOD";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeIOD = null;
				}
					// Extract the result with the queryNodeIterator.
				else
				{
					attributeIOD = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeIOD(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeIOD;
		}

		
		/// <summary>
		///		Get the Type Of Matching for the <paramref name="Attribute">
		///		Attribute</paramref> using the SOP Class UID and System Name
		///		that are set. With <paramref name="Recursive">Recursive
		///		</paramref> it is also possible to look inside other
		///		Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the IOD for in the 
		///		0x12345678 or 0x10101010/0x20202020/0x30303030 format. See also
		///		the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Type Of Matching for the
		///			// given Attribute using the SOP Class UID and System Name
		///			// that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			string attributeTypeOfMatching;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			attributeTypeOfMatching = dqLibrary.GetAttributeTypeOfMatching("0x00101010", true);
		///			
		///			// Show the result.
		///			if (attributeTypeOfMatching != null)
		///			{
		///				Console.WriteLine("The Attribute Type Of Matching for Attribute 0x00101010 = " + attributeTypeOfMatching);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			attributeTypeOfMatching = dqLibrary.GetAttributeTypeOfMatching("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			if (attributeTypeOfMatching != null)
		///			{
		///				Console.WriteLine("The Attribute Type Of Matching for Attribute 0x20205555 = " + attributeTypeOfMatching);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Tyoe Of Matching for the
		///			' given Attribute using the SOP Class UID and System Name
		///			' that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim attributeTypeOfMatching As string
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			attributeTypeOfMatching = dqLibrary.GetAttributeTypeOfMatching("0x00101010", true)
		///			
		///			' Show the result.
		///			if (attributeTypeOfMatching &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Type Of Matching for Attribute 0x00101010 = " + attributeTypeOfMatching)
		///			end if
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			attributeTypeOfMatching = dqLibrary.GetAttributeTypeOfMatching("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			if (attributeTypeOfMatching &lt;&gt; nothing) then
		///				Console.WriteLine("The Attribute Type Of Matching for Attribute 0x20205555 = " + attributeTypeOfMatching)
		///			end if
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a String containing the Type Of Matching. If the
		///		<paramref name="Attribute">Attribute</paramref> is not found
		///		null wil be returned.
		/// </returns>
		/// <remarks>
		///		The function will always use the first matching Attribute.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public string GetAttributeTypeOfMatching(string Attribute, bool Recursive)
		{
			string attributeTypeOfMatching;							// Holds the result.
			string query;											// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/TypeOfMatching";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/TypeOfMatching";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Check the query result.
				if (queryNodeIterator.MoveNext() == false)
				{
					attributeTypeOfMatching = null;
				}
					// Extract the result with the queryNodeIterator.
				else
				{
					attributeTypeOfMatching = queryNodeIterator.Current.Value;
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeIOD(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return attributeTypeOfMatching;
		}
		
		/// <summary>
		///		Get the Values (Defined and Enumerated) for the 
		///		<paramref name="Attribute">Attribute</paramref> using the SOP
		///		Class UID and System Name that are set. With
		///		<paramref name="Recursive">Recursive</paramref> it is also
		///		possible to look inside other Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Values (Defined and
		///		Enumerated) for in the 0x12345678 or
		///		0x10101010/0x20202020/0x30303030 format. See also the example
		///		code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Values for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection values;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			values = dqLibrary.GetAttributeValues("0x00101010", true);
		///			
		///			// Show the result.
		///			foreach (string value in values)
		///			{
		///				Console.WriteLine("The Value for Attribute 0x00101010 = " + value);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			values = dqLibrary.GetAttributeValues("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			foreach (string value in values)
		///			{
		///				Console.WriteLine("The Value for Attribute 0x20205555 = " + value);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Values for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim values As StringCollection
		///			Dim value As String
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			values = dqLibrary.GetAttributeValues("0x00101010", true)
		///			
		///			' Show the result.
		///			For Each value in values
		///				Console.WriteLine("The Value for Attribute 0x00101010 = " + value)
		///			Next value
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			values = dqLibrary.GetAttributeValues("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			For Each value in values
		///				Console.WriteLine("The Value for Attribute 0x20205555 = " + value)
		///			Next value
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a StringCollection containing the Values as String.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAttributeValues(string Attribute, bool Recursive)
		{
			StringCollection values = new StringCollection();			// Holds the result.
			string query;												// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/Values/Value";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/Values/Value";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Extract the result with the queryNodeIterator and fill
				// the definedValues Array.
				while (queryNodeIterator.MoveNext())
				{
					values.Add(queryNodeIterator.Current.Value);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeValues(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return values;
		}

		/// <summary>
		///		Get the Defined Values for the <paramref name="Attribute">
		///		Attribute</paramref> using the SOP Class UID and System Name
		///		that are set. With <paramref name="Recursive">Recursive
		///		</paramref> it is also possible to look inside other
		///		Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Defined Values for in
		///		the 0x12345678 or 0x10101010/0x20202020/0x30303030 format. See
		///		also the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Defined Values for the given
		///			// Attribute using the SOP Class UID and System Name that
		///			// are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection definedValues;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			definedValues = dqLibrary.GetAttributeDefinedValues("0x00101010", true);
		///			
		///			// Show the result.
		///			foreach (string definedValue in definedValues)
		///			{
		///				Console.WriteLine("The Defined Value for Attribute 0x00101010 = " + definedValue);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			definedValues = dqLibrary.GetAttributeDefinedValues("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			foreach (string definedValue in definedValues)
		///			{
		///				Console.WriteLine("The Defined Value for Attribute 0x20205555 = " + definedValue);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Defined Values for the given
		///			' Attribute using the SOP Class UID and System Name that
		///			' are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim definedValues As StringCollection
		///			Dim definedValue As String
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			definedValues = dqLibrary.GetAttributeDefinedValues("0x00101010", true)
		///			
		///			' Show the result.
		///			For Each definedValue in definedValues
		///				Console.WriteLine("The Defined Value for Attribute 0x00101010 = " + definedValue)
		///			Next definedValue
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			definedValues = dqLibrary.GetAttributeDefinedValues("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			For Each definedValue in definedValues
		///				Console.WriteLine("The Defined Value for Attribute 0x20205555 = " + definedValue)
		///			Next definedValue
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a StringCollection containing the Defined Values as String.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAttributeDefinedValues(string Attribute, bool Recursive)
		{
			StringCollection definedValues = new StringCollection();	// Holds the result.
			string query;												// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/Values/Value[@Type='DEFINED']";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/Values/Value[@Type='DEFINED']";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Extract the result with the queryNodeIterator and fill
				// the definedValues Array.
				while (queryNodeIterator.MoveNext())
				{
                    definedValues.Add(queryNodeIterator.Current.Value);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeDefinedValues(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return definedValues;
		}

		/// <summary>
		///		Get the Enumerated Values for the <paramref name="Attribute">
		///		Attribute</paramref> using the SOP Class UID and System Name
		///		that are set. With <paramref name="Recursive">Recursive
		///		</paramref> it is also possible to look inside other
		///		Attributes.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to get the Enumerated Values for in
		///		the 0x12345678 or 0x10101010/0x20202020/0x30303030 format. See
		///		also the example code.
		/// </param>
		/// <param name="Recursive">
		///		If true the Attribute or Attribute path will also be searched
		///		inside other Attributes. If false the search will only take
		///		place direct at the top level.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the Attribute Enumerated Values for the
		///			// given Attribute using the SOP Class UID and System Name
		///			// that are set.
		///			
		///			DefinitionQueryLibrary dqLibrary;
		///			StringCollection enumeratedValues;
		///			
		///			dqLibrary = new DefinitionQueryLibrary;
		///			
		///			// Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1");
		///			dqLibrary.SetSystemName("DICOM");
		///			
		///			// Single Attribute and look also inside other Attributes (Recursive).
		///			enumeratedValues = dqLibrary.GetAttributeEnumeratedValues("0x00101010", true);
		///			
		///			// Show the result.
		///			foreach (string enumeratedValue in enumeratedValues)
		///			{
		///				Console.WriteLine("The Enumerated Value for Attribute 0x00101010 = " + enumeratedValue);
		///			}
		///			
		///			// Attribute path but don't look inside other Attributes (Recursive).
		///			enumeratedValues = dqLibrary.GetAttributeEnumeratedValues("0x00101010/0x20205555", false);
		///			
		///			// Show the result.
		///			foreach (string enumeratedValue in enumeratedValues)
		///			{
		///				Console.WriteLine("The Enumerated Value for Attribute 0x20205555 = " + enumeratedValue);
		///			}
		///		</code>
		///		<b>VB.Net</b>
		///		<code>
		///			' Example: Get the Attribute Enumerated Values for the
		///			' given Attribute using the SOP Class UID and System Name
		///			' that are set.
		///			
		///			Dim dqLibrary As DefinitionQueryLibrary 
		///			Dim enumeratedValues As StringCollection
		///			Dim enumeratedValue As String
		///			
		///			dqLibrary = new DefinitionQueryLibrary
		///			
		///			' Set the SOP Class UID and System Name.
		///			dqLibrary.SetSOPClassUID("1.2.87.478.56.99.3.2.1")
		///			dqLibrary.SetSystemName("DICOM")
		///			
		///			' Single Attribute and look also inside other Attributes (Recursive).
		///			enumeratedValues = dqLibrary.GetAttributeEnumeratedValues("0x00101010", true)
		///			
		///			' Show the result.
		///			For Each enumeratedValue in enumeratedValues
		///				Console.WriteLine("The Enumerated Value for Attribute 0x00101010 = " + enumeratedValue)
		///			Next enumeratedValue
		///			
		///			' Attribute path but don't look inside other Attributes (Recursive).
		///			enumeratedValues = dqLibrary.GetAttributeEnumeratedValues("0x00101010/0x20205555", false)
		///			
		///			' Show the result.
		///			For Each enumeratedValue in enumeratedValues
		///				Console.WriteLine("The Enumerated Value for Attribute 0x20205555 = " + enumeratedValue)
		///			Next enumeratedValue
		///		</code>
		/// </example>
		/// <returns>
		///		Returns a StringCollection containing the Enumerated Values as String.
		/// </returns>
		/// <exception cref="System.Exception">
		///		Throws an Exception if the query can't be executed succesfully.
		/// </exception>
		public StringCollection GetAttributeEnumeratedValues(string Attribute, bool Recursive)
		{
			StringCollection enumeratedValues = new StringCollection();	// Holds the result.
			string query;												// Holds the query.

			// Get the Xml Definition file from the dqFileManager.
			queryDocument = dqFileManager.GetFile(sopClassUID, systemName);

			// Create a navigator instance for the Xml Definition file.
			queryNavigator = queryDocument.CreateNavigator();

			// Generate the query.
			if (Recursive == true)
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module//" + parseAttributeParam(Attribute) + "/Values/Value[@Type='ENUMERATED']";
			}
			else
			{
				query = "/System/*/SOPClass/Dimse/Dataset/Module/" + parseAttributeParam(Attribute) + "/Values/Value[@Type='ENUMERATED']";
			}

			try
			{
				// Execute the query.
				queryNodeIterator = queryNavigator.Select(query);

				// Extract the result with the queryNodeIterator and fill
				// the definedValues Array.
				while (queryNodeIterator.MoveNext())
				{
					enumeratedValues.Add(queryNodeIterator.Current.Value);
				}
			}
			catch(Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to execute the query for the function GetAttributeEnumeratedValues(string Attribute, bool Recursive). This can occur if the loaded Xml Definition file is incompleet or corrupt or the parameter Attribute contains incorrect or illegal characters.", ex);
				throw myException;
			}

			// Return the result.
			return enumeratedValues;
		}
		#endregion

		#region Support Functions
		/// <summary>
		///		Parses an Attribute or Attribute path string into a XPath Query
		///		string. See also the example code.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute path to parse in the 0x12345678 or
		///		0x10101010/0x20202020/0x30303030 format.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example with a single Attribute.
		///			
		///			string query = "";
		///			string attribute = "0x10102020";
		///			
		///			query = parseAttributeParam(attribute);
		///			Console.WriteLine(query);
		///			// This will output: Attribute[Group='1010' and Element='2020']
		///		</code>
		///		<code>
		///			// Example with a Attribute path.
		///			
		///			string query = "";
		///			string attribute = "0x10102020/0x30304040/";
		///			
		///			query = parseAttributeParam(attribute);
		///			Console.WriteLine(query);
		///			// This will output: Attribute[Group='1010' and Element='2020']/Values/Sequence/Item/Attribute[Group='3030' and Element='4040']
		///		</code>
		/// </example>
		/// <exception cref="System.Exception">
		///		Throws an Exception of the Parameter can't be parsed.
		/// </exception>
		/// <returns>A string containing the XPath Query.</returns>
		private string parseAttributeParam(string Attribute)
		{
			string group;									// Holds the group part of the Attribute.
			string element;									// Holds the element part of the Attribute.
			string rValue = "";								// The parsed Attribute string to return.
			
			// Convert the attribute string to upper so that x will be always X.
			Attribute = Attribute.ToUpper();
			
			// Check if there is any / character in the Attribute string.
			if (Attribute.IndexOf("/") == -1)
			{
				try
				{
					group = Attribute.Substring(2, 4);
					element = Attribute.Substring(6, 4);
				}
				catch (Exception ex)
				{
					// Throw new exception.
					Exception myException;
					myException = new ArgumentException("Error while trying to parse the Attribute parameter: " + Attribute + ". Please make sure the format is correct.", ex);
					throw myException;
				}
				
				rValue = "Attribute[Group='" + group + "' and Element='" + element + "']";
			}
			// Check for / characters in the Attribute string that only
			// contains one Attribute.
			else if (Attribute.IndexOf("X") == Attribute.LastIndexOf("X"))
			{
				// Strip of / character at the beginning.
				if (Attribute.StartsWith("/") == true)
				{
					Attribute = Attribute.Substring(1, Attribute.Length -1);
				}
				// Strip of / character at the end.
				if (Attribute.EndsWith("/") == true)
				{
					Attribute = Attribute.Substring(0, Attribute.Length -1);
				}
				// Parse the Attribute parameter.
				try
				{
					group = Attribute.Substring(2, 4);
					element = Attribute.Substring(6, 4);
				}
				catch (Exception ex)
				{
					// Throw new exception.
					Exception myException;
					myException = new ArgumentException("Error while trying to parse the Attribute parameter: " + Attribute + ". Please make sure the format is correct.", ex);
					throw myException;
				}
				rValue = "Attribute[Group='" + group + "' and Element='" + element + "']";
			}
			else
			{
				bool parsingComplete = false;						// Stops the parsing loop.
				
				// Strip of / character at the beginning.
				if (Attribute.StartsWith("/") == true)
				{
					Attribute = Attribute.Substring(1, Attribute.Length -1);
				}
				// Strip of / character at the end.
				if (Attribute.EndsWith("/") == true)
				{
					Attribute = Attribute.Substring(0, Attribute.Length -1);
				}
				
				while (parsingComplete == false)
				{
					// Check for parsing the last Attribute.
					if (Attribute.Length == 10)
					{
						parsingComplete = true;
					}

					// Parse the Attribute parameter.
					try
					{
						group = Attribute.Substring(2, 4);
						element = Attribute.Substring(6, 4);
					}
					catch (Exception ex)
					{
						// Throw new exception.
						Exception myException;
						myException = new ArgumentException("Error while trying to parse the Attribute parameter: " + Attribute + ". Please make sure the format is correct.", ex);
						throw myException;
					}
					// Add the Attribute to the return value.
					rValue += "Attribute[Group='" + group + "' and Element='" + element + "']/Values/Sequence/Item/";

					// Strip the first Attribute off except on the last time of
					// the loop.
					if (parsingComplete == false)
					{
						try
						{
							Attribute = Attribute.Substring(Attribute.IndexOf("/") + 1, Attribute.Length - 11);
						}
						catch (Exception ex)
						{
							// Throw new exception.
							Exception myException;
							myException = new ArgumentException("Error while trying to parse the Attribute parameter: " + Attribute + ". Please make sure the format is correct.", ex);
							throw myException;
						}
					}
				}
				// Remove the /Values/Sequence/Item/ from the end of the return
				// value.
				rValue = rValue.Substring(0, rValue.Length -22);
			}
			
			return rValue;
		}
		
		/// <summary>
		///		Recursive support function for the public
		///		<see cref="GetAttributeTree()">GetAttributeTree()</see> function.
		///		This function will return for each top level Attribute a list
		///		of Attributes it includes in full Attribute Path notation. See
		///		also the example code.
		/// </summary>
		/// <param name="Attribute">
		///		Attribute or Attribute Path to look in for included Attributes
		///		in the 0x12345678 or 0x10101010/0x20202020/0x30303030 format.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			StringCollection myStringCollection;
		///			
		///			myStringCollection GetAttributeTree("0x01234567");
		///			foreach (string attribute in myStringCollection)
		///			{
		///				Console.WriteLine(attribute);
		///			}
		///			
		///			// This will output:
		///			//
		///			// 0x01234567
		///			// 0x01234567/0x15465478
		///			// 0x01234567/0x25478963
		///			// 0x01234567/0x65487632
		///			// 0x01234567/0x65487632/0x12547885
		///			// 0x01234567/0x65487632/0x12547896
		///		</code>
		/// </example>
		/// <returns>
		///		A StringCollection containing the Attribute and Attributes
		///		included.
		/// </returns>
		private StringCollection GetAttributeTree(string Attribute)
		{
			StringCollection attributes = new StringCollection();	// Holds the result.
			StringCollection subAttributes;							// Holds the sub result.
			
			// Check if the current Attribute is a Sequence Attribute.
			if (GetAttributeVR(Attribute, false) == "SQ")
			{
				// Add the Attribute to the result.
				attributes.Add(Attribute);
				
				// Get all Attributes in the current Sequence Attribute and iterate trough them.
				foreach (string currentAttribute in GetAllAttributesInAttribute(Attribute, false, false))
				{
					// Do a recursive call to this function to get the Attributes
					// and also the Attributes in possible Sequence Attributes.
					subAttributes = GetAttributeTree(Attribute + "/" + currentAttribute);
					
					// Merge the recursive result list with the result list.
					foreach (string currentSubAttribute in subAttributes)
					{
						attributes.Add(currentSubAttribute);
					}
				}
			}
			else
			{
				// Add the Attribute to the result.
				attributes.Add(Attribute);
			}

			// Return the result.
			return attributes;
		}
		#endregion
		
		#endregion

	}
}