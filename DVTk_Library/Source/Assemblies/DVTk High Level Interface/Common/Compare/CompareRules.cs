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
using System.IO;
using System.Xml.Serialization;


using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// All validation rules that will be applied to a list of attribute collections.
	/// </summary>
	[XmlInclude(typeof(CompareRule))] 
	public class CompareRules
	{
		//
		// - Fields -
		//

		/// <summary>
		/// The internal hidden non-type-safe list of CcompareRules implemented as an ArrayList.
		/// </summary>
		private ArrayList compareRules = new ArrayList();

        /// <summary>
        /// See property GeneralAttributeCollectionDescriptions.
        /// </summary>
        private String[] generalAttributeCollectionDescriptions = new String[0];

        /// <summary>
        /// Indicates the version of the xml structure used to serialize and deserialize an instance of this class.
        /// </summary>
		private const String xmlStructureVersion = "1";

        /// <summary>
        /// Contains a list indicating the types of attributes used in the different data sets to compare.
        /// </summary>
		private AttributeTypes[] typeOfAttributes = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks><b>Do not use this constructor. It may only be used indirectly for serialization purposes.</b></remarks>
		[Obsolete("Do not use this constructor. It may only be used indirectly for serialization purposes.")]
		public CompareRules()
		{

		}

		/// <summary>
		/// Constructor to create an empty instances.
		/// </summary>
		public CompareRules(params AttributeTypes[] typeOfAttributes)
		{
			this.typeOfAttributes = typeOfAttributes;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets the number of CompareRule instances.
		/// </summary>
		public int Count
		{
			get
			{
				return(this.compareRules.Count);
			}
		}

		/// <summary>
		/// Gets a specific CompareRule instance.
		/// </summary>
		public CompareRule this[int zeroBasedIndex]
		{
			get
			{
				if ((zeroBasedIndex < 0) || (zeroBasedIndex >= Count))
				{
					throw new HliException("Wrong index used for CompareRule.");
				}
				
				return(this.compareRules[zeroBasedIndex] as CompareRule);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Add a CompareRule the this object.
		/// <br></br><br></br>
		/// A check is performed if the compareRule is compatible with earlier added one.
		/// If it is not compatible, an exception is thrown.
		/// </summary>
        /// <param name="compareRule">The CompareRule instance to add.</param>
		public void Add(CompareRule compareRule)
		{
			if (compareRule.Count != this.typeOfAttributes.Length)
			{
				throw new System.ArgumentException("Number of validation rules in supplied CompareRule differs from number of supplied attribute types in the constructor.");
			}

			for (int index = 0; index < this.typeOfAttributes.Length; index++)
			{
				if (compareRule[index] != null)
				{
					if (this.typeOfAttributes[index] == AttributeTypes.DicomAttribute)
					{
						if (compareRule[index].GetType().Name != "ValidationRuleDicomAttribute")
						{
							throw new System.ArgumentException("Validation rule in compare rule should be of type ValidationRuleDicomAttribute.");
						}
					}
					else if (this.typeOfAttributes[index] == AttributeTypes.Hl7Attribute)
					{
						if (compareRule[index].GetType().Name != "ValidationRuleHl7Attribute")
						{
							throw new System.ArgumentException("Validation rule in compare rule should be of type ValidationRuleHl7Attribute.");
						}
					}
					else
					{
						throw new System.Exception("Not supposed to get here.");
					}
				}
			}

			this.compareRules.Add(compareRule);
		}

        /// <summary>
        /// Cretaes a CompareRules instance using an xml file as input.
        /// </summary>
        /// <param name="fullFileName">Full file name of the .xml file.</param>
        /// <returns>The created CompareRules instance.</returns>
		public static CompareRules Deserialize(String fullFileName)
		{
			FileStream myFileStream = null;
			CompareRules compareRules = null;

			try
			{
				XmlSerializer mySerializer = new XmlSerializer(typeof(CompareRules));

				// To read the file, creates a FileStream.
				myFileStream = new FileStream(fullFileName, FileMode.Open);

				// Calls the Deserialize method and casts to the object type.
				compareRules = (CompareRules) mySerializer.Deserialize(myFileStream);
			}
			catch(System.Exception exception)
			{
				// Rethrow exception.
				throw(exception);
			}
			finally
			{
				if (myFileStream != null)
				{
					myFileStream.Close();
				}
			}

			myFileStream.Close();

			return(compareRules);
		}
        
        /// <summary>
        /// Serializes this CompareRules instance.
        /// </summary>
        /// <param name="fullFileName">Full file name of the .xml file.</param>
		public void Serialize(String fullFileName)
		{
			StreamWriter myWriter = null;

			try
			{
				XmlSerializer mySerializer = new XmlSerializer(typeof(CompareRules));

				// To write to a file, create a StreamWriter object.
				myWriter = new StreamWriter(fullFileName);

				mySerializer.Serialize(myWriter, this);
			}
			catch (System.Exception exception)
			{
				throw exception;
			}
			finally
			{
				if (myWriter != null)
				{
					myWriter.Close();}
			}
		}

        /// <summary>
        /// Are representation of this instance.
        /// </summary>
		public CompareRule[] ArrayRepresentation
		{
			get
			{
				return((CompareRule[])this.compareRules.ToArray(typeof(CompareRule)));
			}
			set
			{
				this.compareRules.AddRange(value);
			}
		}

        /// <summary>
        /// Gets or sets the general descriptions of the attribute collections.
        /// </summary>
		public String[] GeneralAttributeCollectionDescriptions
		{
			get
			{
				return(this.generalAttributeCollectionDescriptions);
			}
			set
			{
				this.generalAttributeCollectionDescriptions = value;
			}
		}

        /// <summary>
        /// Gets a specific description for an attribute collection.
        /// </summary>
        /// <param name="index">The index specifying the attribute collection.</param>
        /// <returns>The description.</returns>
		public String GetGeneralAttributeCollectionDescription(int index)
		{
			String description = "";

			if (this.generalAttributeCollectionDescriptions.Length == this.typeOfAttributes.Length)
			{
				description = this.generalAttributeCollectionDescriptions[index];
			}
			else
			{
				int previousDicomAttributeCollectionsCount = 0;
				int previousHl7AttributeCollectionsCount = 0;

				for (int previousIndex = 0; previousIndex < index; previousIndex++)
				{
					if (this.typeOfAttributes[previousIndex] == AttributeTypes.DicomAttribute)
					{
						previousDicomAttributeCollectionsCount++;
					}
					else if (this.typeOfAttributes[previousIndex] == AttributeTypes.Hl7Attribute)
					{
						previousHl7AttributeCollectionsCount++;
					}
				}

				AttributeTypes attributeType = this.typeOfAttributes[index];

				if (attributeType == AttributeTypes.DicomAttribute)
				{
					description = "Dicom attribute collection " + (previousDicomAttributeCollectionsCount+1).ToString();
				}
				else if (attributeType == AttributeTypes.Hl7Attribute)
				{
					description = "Hl7 attribute collection " + (previousHl7AttributeCollectionsCount+1).ToString();
				}
			}

			return(description);
		}





		internal CompareRules Clone()
        {
#pragma warning disable 0618
            CompareRules clonedCompareRules = new CompareRules();
#pragma warning restore 0618

            clonedCompareRules.generalAttributeCollectionDescriptions = this.generalAttributeCollectionDescriptions;
			clonedCompareRules.compareRules = this.compareRules.Clone() as ArrayList;

			return(clonedCompareRules);
		}

        /// <summary>
        /// Gets or sets the type of attributes that may be compared with each other.
        /// </summary>
		public AttributeTypes[] TypeOfAttributes
		{
			get
			{
				return(this.typeOfAttributes);
			}
			set
			{
				this.typeOfAttributes = value;
			}
		}

        /// <summary>
        /// Gets or sets the xml structure version.
        /// </summary>
		public String XmlStructureVersion
		{
			get
			{
				return(xmlStructureVersion);
			}
			set
			{
				// Do nothing.
			}
		}

	}
}
