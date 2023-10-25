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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Dvtk.Definition_Query_Library
{
	/// <summary>
	///		The FileManager manages the loading and  unloading of definition
	///		files. It ensures that definition files are only loaded once. It
	///		provides access to definition files by using their full path name
	///		or by SOP Class UID.
	/// </summary>
	public class FileManager
	{
		/// <summary>
		///		Holds the XPathDocument instances of the Xml Definition file
		///		with as key the full path and filename to the Xml Definition
		///		file.
		/// </summary>
		private Hashtable fileHashTable;
		/// <summary>
		///		Holds a mapping between the Xlm Definition filename that is
		///		used as key in the
		///		<see cref="Dvtk.Definition_Query_Library.FileManager.fileHashTable">
		///		fileHashTable</see> and the combination of SOP Class UID and
		///		System Name. The mapping table is needed for selecting Xml
		///		Definition files by filename and a combination of SOP Class UID
		///		ans System Name.
		///		The key combination is created with the
		///		<see cref="Dvtk.Definition_Query_Library.FileManager.createKey">
		///		createKey</see> function.
		/// </summary>
		private Hashtable mappingHashTable;
		
		/// <summary>
		///		Constructor for the 
		///		<see cref="T:Dvtk.Definition_Query_Library.FileManager">FileManager
		///		</see>class.
		/// </summary>
		public FileManager()
		{
			// Create instances of the fileHashTable and mappingHashTable.
			fileHashTable = new Hashtable();
			mappingHashTable = new Hashtable();
		}

		/// <summary>
		///		Get the XPathDocument Instance for the Xml Definition file
		///		specified in the <paramref name="FileName">FileName</paramref>.
		/// </summary>
		/// <param name="FileName">
		///		Full path to the Xml Definition File. See also the example
		///		code.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the XPathDocument instance for a specific
		///			// Xml Definition file by providing the full file path to
		///			// the Xml Definition file.
		///			
		///			FileManager myFileManager = new FileManager();
		///			XPathDocument myDoc;
		///			myDoc = myFileManager.GetFile(@"C:\example.xml");
		///		</code>
		/// </example>
		/// <returns>
		///		An XPathDocument instance of the Xml Definition file.
		/// </returns>
		/// <exception cref="System.Exception">
		///		If the Xml Definition file is not loaded an Exception will be
		///		thrown.
		/// </exception>
		public XPathDocument GetFile(string FileName)
		{
			// Check if the file has been loaded into the fileHasTable.
			if (fileHashTable.ContainsKey(FileName) == true)
			{
				return (XPathDocument) fileHashTable[FileName];
			}
			else
			{
				// The requested Xml Definition file is not loaded so throw an
				// Exception.
				throw new Exception("The Xml Definition file: " + FileName + " is not loaded.");
			}
		}
		
		/// <summary>
		///		Get the XPathDocument Instance for the Xml Definition file
		///		with SOP Class UID <see cref="P:SOPCLassUID">SOPClassUID</see>
		///		and with System Name <see cref="P:SystemName">SystemName</see>.
		/// </summary>
		/// <param name="SOPClassUID">
		///		Contains the SOP Class UID of the Xml Definition file. See also
		///		the example code.
		/// </param>
		/// <param name="SystemName">
		///		Contains the System Name of the Xml Definition file. See also
		///		the example code.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Get the XPathDocument instance for a specific
		///			// Xml Definition file by providing the SOP Class UID and
		///			// System Name.
		///			
		///			FileManager myFileManager = new FileManager();
		///			XPathDocument myDoc;
		///			myDoc = myFileManager.GetFile("1.2.8.5.4.6.88.154", "DICOM");
		///		</code>
		/// </example>
		/// <returns>
		///		A XPathDocument instance of the Xml Definition file.
		/// </returns>
		/// <exception cref="System.Exception">
		///		If the Xml Definition file is not loaded an Exception will be
		///		thrown.
		/// </exception>
		public XPathDocument GetFile(string SOPClassUID, string SystemName)
		{
			// Create the mappingKey for checking the fileHashTable.
			string mappingKey = createKey(SOPClassUID, SystemName);
			
			// Check if the file has already been loaded into the
			// mappingHashTable.
			if (mappingHashTable.ContainsKey(mappingKey) == true)
			{
				// Get the filename mapping key from the mappingHashTable.
				string fileName = (string)mappingHashTable[mappingKey];

				// Return the XPathDocument instance for the file.
				return GetFile(fileName);
			}
			else
			{
				// No Xml Definition file loaded for the requested SOP Class
				// UID and System Name so throw an Exception.
				throw new Exception("No Xml Definition file loaded for SOP Class: " + SOPClassUID + " and System Name: " + SystemName);
			}
		}

		/// <summary>
		///		Creates a XPathDocument instance for the Xml Definition file.
		///		This can be used to preload definition files. If the file has
		///		already been loaded it will be ignored.
		/// </summary>
		/// <param name="FileName">Full path to the XML File</param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Preload a Xml Definition File.
		///			
		///			FileManager myFileManager = new FileManager();
		///			myFileManager.PreLoadFile(@"C:\example.xml");
		///		</code>
		///	</example>
		public void PreLoadFile(string FileName)
		{
			// Check if the file has already been loaded into the
			// infoHashTable by chekking the mappingHashTable.
			if (mappingHashTable.ContainsKey(FileName) == false)
			{
				// Load the file.
				loadFile(FileName);
			}
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
		///			FileManager myFileManager = new FileManager();
		///			Array myArray;
		///			
		///			myFileManager.PreLoadFile(@"C:\example.xml");
		///			myArray = myFileManager.GetLoadedFiles();
		///			
		///			foreach(string definitionFile in myArray)
		///			{
		///				System.Console.WriteLine(definitionFile);
		///			}
		///		</code>
		/// </example>
		public Array GetLoadedFiles()
		{
			// Return all keys of the fileHashTable as array.
			return (Array) fileHashTable.Keys;
		}

		#region FileManager Support Functions
		/// <summary>
		///		Create a XPathDocument instance of the Xml Definition file in
		///		FileName. Add the XPathDocument instance to the
		///		<see cref="Dvtk.Definition_Query_Library.FileManager.fileHashTable">
		///		fileHashTable</see>.
		///		Query the Xml Definition file for the SOP Class UID and System
		///		Name and create a mapping key of it with the
		///		<see cref="Dvtk.Definition_Query_Library.FileManager.createKey">
		///		createKey</see> function. Add the createKey to the
		///		<see cref="Dvtk.Definition_Query_Library.FileManager.mappingHashTable">
		///		mappingHashTable</see>.
		/// </summary>
		/// <param name="FileName">
		///		Full path to the Xml Definition File. See also the example
		///		code.
		/// </param>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Load Xml Definition File.
		///			loadFile("C:\example.xml");
		///		</code>
		/// </example>
		/// <remarks>
		///		This function will filter out the &lt;Macro&gt; and &lt;/Macro&gt;
		///		attributes from the Xml File.
		/// </remarks>
		/// <exception cref="System.Exception">
		///		If the Xml Definition file is incorrect or doesn't contain a
		///		SOP Class UID or System Name an Exception is thrown.
		/// </exception>
		private void loadFile(string FileName)
		{
			XPathDocument definitionFile;					// Holds the Xml Definition file.
			StreamReader definitionReader;					// For reading the Definition file.
			XmlDocument definitionXmlDocument;				// Holds the Xml Definition file.
			XPathNavigator definitionNavigator;				// Holds the Xml Definition navigator.
			XPathNodeIterator definitionNodeIterator;		// Holds the Xml Definition node iterator.
			StringBuilder buffer;							// Input buffer for the definitionXmlDocument.
			string inputLine = "";							// Input buffer for the definitionReader;
			string SOPClassUID = "";						// Holds the SOP Class UID found in the Xml Definition file.
			string SystemName = "";							// Holds the System Name found in the Xml Definition file.
			string mappingKey = "";							// Holds the mapping key for the mappingHashTable.
			
			try
			{
				FileInfo definitionFileInfo = new FileInfo(FileName);
				buffer = new StringBuilder((int)definitionFileInfo.Length);
				using (definitionReader = new StreamReader(FileName))
				{
					while ((inputLine = definitionReader.ReadLine()) != null)
					{
						// If the line contains <Macro Name=""> then remove the
						// <Macro Name=""> from the line and add the rest to
						// the buffer.
						if (inputLine.IndexOf("<Macro") != -1)
						{
							// Get everything before <Macro Name="">
							string left = inputLine.Substring(0, inputLine.IndexOf("<Macro"));
							// Get everything after <Macro Name="">
							string right = inputLine.Substring(inputLine.IndexOf(">", inputLine.IndexOf("<Macro")) + 1);
							buffer.Append(left + right);
						}
						// If the line contains </Macro> then remove the
						// </Macro> from the line and add the rest to the
						//	buffer.
						else if  (inputLine.IndexOf("</Macro") != -1)
						{
							// Get everything before </Macro>
							string left = inputLine.Substring(0, inputLine.IndexOf("</Macro"));
							// Get everything after </Macro>
							string right = inputLine.Substring(inputLine.IndexOf(">", inputLine.IndexOf("</Macro")) + 1);
							buffer.Append(left + right);
						}
						else
						{
							buffer.Append(inputLine);
						}
					}
				}

				// Create a XmlDocument from the buffer.
				definitionXmlDocument = new XmlDocument();
				definitionXmlDocument.LoadXml(buffer.ToString());

				// Convert the XmlDocument to an XPathDocument.
				definitionFile = new XPathDocument(new XmlNodeReader(definitionXmlDocument));

				// Create a Navigator for the definitionFile.
				definitionNavigator = definitionFile.CreateNavigator();

				// Get the SOP Class UID from the definitionFile.
				definitionNodeIterator = definitionNavigator.Select("/System/*/SOPClass/@UID");
				definitionNodeIterator.MoveNext();
				definitionNodeIterator.Current.MoveToFirstAttribute();
				SOPClassUID = definitionNodeIterator.Current.Value;

				// Get the System Name from the definitionFile.
				definitionNodeIterator = definitionNavigator.Select("/System/@Name");
				definitionNodeIterator.MoveNext();
				definitionNodeIterator.Current.MoveToFirstAttribute();
				SystemName = definitionNodeIterator.Current.Value;

				// Create the mapping key for the mappingHashTable.
				mappingKey = createKey(SOPClassUID, SystemName);
				
				// Add the new file and mapping key to the Hash Tables.
				if (fileHashTable.ContainsKey(FileName) == false)
				{
					fileHashTable.Add(FileName, definitionFile);
					mappingHashTable.Add(mappingKey, FileName);
				}

			}
			catch (Exception ex)
			{
				// Throw new exception.
				Exception myException;
				myException = new Exception("Failed to load the file: " + FileName, ex);
				throw myException;
			}
		}

		/// <summary>
		///		Creates a key for the <see cref="F:Dvtk.Definition_Query_Library.FileManager.fileHashTable">fileHashTable</see>
		///		to identify Xml Definition files. The key will consist of the
		///		SOPClassUID and SystemName.
		/// </summary>
		/// <param name="SOPClassUID">
		///		The UID of the SOP Class. See also the example code.
		/// </param>
		/// <param name="SystemName">
		///		The System Name. See also the example code.
		///	</param>
		/// <returns>
		///		A string forming the key to identify the Xml Definition file.
		/// </returns>
		/// <example>
		///		<b>C#</b>
		///		<code>
		///			// Example: Create a key.
		///			
		///			string myFileKey;
		///			myFileKey = createKey("1.2.4.5.6.11.4.88.654.45", "MySystemName");
		///		</code>
		/// </example>
		/// <remarks>
		///		Always use this function to create keys for the
		///		<see cref="F:Dvtk.Definition_Query_Library.FileManager.fileHashTable">fileHashTable</see>
		///		to ensure correct keys trough out the entire application.
		/// </remarks>
		private string createKey(string SOPClassUID, string SystemName)
		{
			// Create and return the key.
			return SOPClassUID + "##" + SystemName;
		}
		#endregion
	}
}