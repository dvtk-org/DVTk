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

using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// Represents a set of Dicom Attributes.
	/// </summary>
	abstract public class AttributeSet
	{
		//
		// - Fields -
		//

		/// <summary>
		/// The encapsulated AttributeSet from the DvtkData librbary.
		/// </summary>
		protected DvtkData.Dimse.AttributeSet dvtkDataAttributeSet = null;

		/// <summary>
		/// See property TagSequence.
		/// </summary>
		private TagSequence tagSequence = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor for a root AttributeSet (like a Command Set, Data Set or File Meta information).
		/// </summary>
		/// <param name="dvtkDataAttributeSet">The encapsulated DvtkData Attribute Set.</param>
		internal AttributeSet(DvtkData.Dimse.AttributeSet dvtkDataAttributeSet)
		{
			this.dvtkDataAttributeSet = dvtkDataAttributeSet;
			this.tagSequence = new TagSequence();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dvtkDataAttributeSet">The encapsulated DvtkData Attribute Set.</param>
		/// <param name="tagSequence">The TagSequence (see property TagSequence).</param>
		internal AttributeSet(TagSequence tagSequence, DvtkData.Dimse.AttributeSet dvtkDataAttributeSet)
		{
			this.dvtkDataAttributeSet = dvtkDataAttributeSet;
			this.tagSequence = tagSequence;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets the number of attributes.
		/// </summary>
		public int Count
		{
			get
			{
				int count = 0;

				count = dvtkDataAttributeSet.Count;

				return(count);
			}
		}

		/// <summary>
		/// Gets the encapsulated DvtkData AttributeSet.
		/// </summary>
		internal DvtkData.Dimse.AttributeSet DvtkDataAttributeSet
		{
			get
			{
				return(this.dvtkDataAttributeSet);
			}
		}

		/// <summary>
		/// Gets the TagSequence of this AttributeSet.
		/// </summary>
		internal TagSequence TagSequence
		{
			get
			{
				return(this.tagSequence);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
        /// Adds (using deep copy) an Attribute.
		/// </summary>
		/// <param name="attribute">The Attribute to add.</param>
		public void Add(Attribute attribute)
		{
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            ValidAttribute validAttribute = attribute as ValidAttribute;

			if (validAttribute == null)
			{
				// Do nothing.
				Thread.WriteWarningCurrentThread("Adding an invalid attribute. AttributeSet will remain the same.");
			}
			else
			{
				ValidAttribute cloneValidAttribute = attribute.Clone(this) as ValidAttribute;

				if (cloneValidAttribute != null)
				{
					this.dvtkDataAttributeSet.Add(cloneValidAttribute.DvtkDataAttribute);
					MakeAscending(false);
				}
			}
		}

        /// <summary>
        /// Adds (using deep copy) the <paramref name="sequenceItem"/> to all Sequence Attributes
        /// indicated by <paramref name="tagSequence"/>.
        /// </summary>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="sequenceItem">The Sequence Item to add.</param>
		public void AddItem(String tagSequence, SequenceItem sequenceItem)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			AttributeCollection attributeCollection = GetAttributes(internalTagSequence);

			foreach(Attribute attribute in attributeCollection)
			{
				attribute.AddItem(sequenceItem);
			}
		}






        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagSequence"></param>
        /// <param name="values"></param>
		public void AddValues(String tagSequence, Values values)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			AttributeCollection attributeCollection = GetAttributes(internalTagSequence);

			foreach(Attribute attribute in attributeCollection)
			{
				attribute.Values.Add(values);
			}
		}

        /// <summary>
        /// Adds values to all attributes that are refered to by the supplied tag sequence.
        /// </summary>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="parameters">The values to add.</param>
		public void AddValues(String tagSequence, params Object[] parameters)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			AttributeCollection attributeCollection = GetAttributes(internalTagSequence);

			foreach(Attribute attribute in attributeCollection)
			{
				attribute.Values.Add(parameters);
			}
		}

		/// <summary>
		/// When the tag sequence is single attribute specifying, this method does the following:
		/// - When the attribute already exists and also has VR SQ, the values or sequence items
		///   are appended to the already existing values or sequence items.
		/// - When the attribute already exists but has a different VR, an error is displayed and
		///   nothing is appended.
		/// - When the attribute does not already exists, it is created and the values or sequence
		///   items are added.
		/// 
		/// When the tag sequence contains wildcards, the rules above are applied to each existing attribute
		/// specified by the tag sequence.
		/// 
		/// Whenever the VR specified is SQ and the parameters supplied contain non sequence
		/// item(s), and exception is thrown.
		/// Whenever the VR specified is unequal SQ and the parameters contain sequence item(s),
		/// and exception is thrown.
		/// </summary>
		/// <param name="tagSequence">The tag sequence.</param>
		/// <param name="vR">The VR.</param>
		/// <param name="parameters">The parameters to append.</param>
		public void Append(String tagSequence, VR vR, params Object[] parameters)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsSingleAttributeMatching)
			{
				throw new Exception("Invalid tag sequence supplied.");
			}

			Append(internalTagSequence, vR, parameters);
		}

		
		internal void Append(TagSequence tagSequence, VR vR, params Object[] parameters)
		{
			// Check if the supplied parameters correspond with the supplied VR.
			// If the check fails, an exception is thrown.
			CheckParameters(vR, parameters);

			if (tagSequence.IsSingleAttributeMatching)
				// Supplied TagSequence is single attribute matching.
			{
				if (Exists(tagSequence))
					// Attribute already exists.
				{
					Attribute alreadyExistingAttribute = this[tagSequence];

					if (alreadyExistingAttribute.VR == vR)
						// Attribute already exists and has the same VR.
					{
						if (vR == VR.SQ)
							// Attribute already exists and also has VR SQ.
						{
							foreach(SequenceItem itemToAdd in parameters)
							{
								alreadyExistingAttribute.AddItem(itemToAdd);
							}
						}
						else
							// Attribute already exists and has the same VR (unequal to SQ).
						{
							alreadyExistingAttribute.Values.Add(parameters);
						}
					}
					else
						// Attribute already exists but has a different VR.
					{
						Thread.WriteErrorCurrentThread("While appending, attribute with tag sequence " + tagSequence.ToString() + " already existed with a different VR. Nothing will be appended.");
					}
				}
				else
					// Attribute does not already exists.
				{
					// In this case, behaviour should be the same as the Set method.
					Set(tagSequence, vR, parameters);
				}
			}
			else
				// Supplied TagSequence contains wildcards.
			{
				AttributeCollection existingAttributes = GetAttributes(tagSequence);

				foreach(ValidAttribute existingAttribute in existingAttributes)
				{
					Append(existingAttribute.TagSequence, vR, parameters);
				}
			}
		}

		/// <summary>
		/// Get an attribute given the TagSequence.
		/// 
		/// The TagSequence supplied must be single attribute matching.
		/// </summary>
		internal Attribute this[TagSequence tagSequence]
		{
			get 
			{
				Attribute attribute = null;

				AttributeCollection attributeCollection = GetAttributes(tagSequence);

				if (attributeCollection.Count == 0)
				{
					attribute = new InvalidAttribute(tagSequence);
				}
				else
				{
					attribute = attributeCollection[0];
				}

				return(attribute);
			}
		}

		/// <summary>
		/// Get an attribute given the TagSequence.
		/// 
		/// The TagSequence supplied must be single attribute matching.
		/// </summary>
		public Attribute this[String tagSequence]
		{
			get 
			{
				TagSequence internalTagSequence = new TagSequence(tagSequence);

				if (!internalTagSequence.IsSingleAttributeMatching)
				{
					throw new HliException("Tag sequence supplied invalid for this operation.");
				}

				return(this[internalTagSequence]);
			}
		}

		/// <summary>
		/// Get an attribute given the zero based index for this AttributeSet.
		/// </summary>
		public Attribute this[int zeroBasedIndex]
		{
			get 
			{
				Attribute attribute = null;

				if ((zeroBasedIndex >= 0) && (zeroBasedIndex < Count))
				{
					DvtkData.Dimse.Attribute dvtkDataAttribute = this.dvtkDataAttributeSet[zeroBasedIndex];

					Tag tag = new Tag(dvtkDataAttribute.Tag.GroupNumber, dvtkDataAttribute.Tag.ElementNumber);

					TagSequence attributeTagSequence = this.tagSequence.Clone();
					attributeTagSequence.Add(tag);

					attribute = new ValidAttribute(attributeTagSequence, dvtkDataAttribute, this);
				}
				else
				{
					throw new System.Exception("Set with tag sequence \"" + this.tagSequence.ToString() + "\" contains " + this.Count.ToString() + "Index for attribute supplied is " + zeroBasedIndex.ToString() + ".");
				}
			
				return attribute;
			}
		}

		/// <summary>
		/// Check if the supplied parameters correspond with the supplied VR.
		/// This functionality is used in e.g. the Append method.
		/// If the check fails, an exception is thrown.
		/// 
		/// For now, the only check performed is looking if the supplied parameters
		/// are SequenceItems or not.
		/// </summary>
		/// <param name="vR">The VR.</param>
		/// <param name="parameters">The parameters.</param>
		internal void CheckParameters(VR vR, ICollection parameters)
		{
			int numberOfSequenceItems = 0;
			int numberOfNonSequenceItems = 0;

			foreach(Object parameter in parameters)
			{
				if (parameter is SequenceItem)
				{
					numberOfSequenceItems++;
				}
				else
				{
					numberOfNonSequenceItems++;
				}
			}

			if (vR == VR.SQ)
			{
				if (numberOfNonSequenceItems > 0)
				{
					throw new System.Exception(numberOfNonSequenceItems.ToString() + " non SequenceItems supplied for VR " + vR.ToString() + ".");
				}
			}
			else
			{
				if (numberOfSequenceItems > 0)
				{
					throw new System.Exception(numberOfSequenceItems.ToString() + " SequenceItems supplied for VR " + vR.ToString() + ".");
				}
			}
		}

		/// <summary>
		/// Remove all attributes from the AttributeSet.
		/// </summary>
		public void Clear()
		{
			this.dvtkDataAttributeSet.Clear();
		}

        /// <summary>
        /// Clear all values that are refered to by the supplied tag sequence.
        /// </summary>
        /// <param name="tagSequence">The tag sequence.</param>
		public void ClearValues(String tagSequence)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			AttributeCollection attributeCollection = GetAttributes(internalTagSequence);

			foreach(Attribute attribute in attributeCollection)
			{
				attribute.Values.Clear();
			}
		}

        /// <summary>
        /// Dumps the content of this instance to text.
        /// </summary>
        /// <param name="objectName">The name of the AttributeSet variable to use in the dump.</param>
        /// <returns>The content of this instance as text.</returns>
		public String DumpUsingVisualBasicNotation(String objectName)
		{
            if (objectName == null)
            {
                throw new ArgumentNullException("objectName");
            }
            else if (objectName.Length == 0)
            {
                throw new ArgumentException("String may not be empty", "objectName");
            }

			return(DumpUsingTagSequenceNotation(objectName, "'"));
		}
        /// <summary>
        /// Dumps the content of this instance to text
        /// </summary>
        /// <param name="appendString">Custom string to appent infront of the dump strings</param>
        /// <returns></returns>
        public string Dump(string appendString)
        {
            if (appendString == null)
            {
                throw new ArgumentNullException("appendString");
            }
            return (DumpUsingTagSequenceNotation(appendString, "'","",""));
        }

		/// <summary>
		/// Create a new Attribute in this AttributeSet containing no values.
		/// 
		/// Precondition: this Attribute may not exist in the AttributeSet.
		/// </summary>
		/// <param name="tagAsUInt32"></param>
		/// <param name="vR"></param>
		internal ValidAttribute Create(UInt32 tagAsUInt32, VR vR)
		{
			TagSequence newTagSequence = this.tagSequence.Clone();
			newTagSequence.Add(new Tag(tagAsUInt32));

			DvtkData.Dimse.Attribute dvtkDataAttribute = new DvtkData.Dimse.Attribute(tagAsUInt32, (DvtkData.Dimse.VR)vR);
			this.dvtkDataAttributeSet.Add(dvtkDataAttribute);
			MakeAscending(false);

			ValidAttribute validAttribute = new ValidAttribute(newTagSequence, dvtkDataAttribute, this);

			return(validAttribute);
		}

		/// <summary>
		/// Delete Attributes.
		/// 
		/// The tag sequence supplied can be both single attribute matching and
		/// wildcard attribute matching.
		/// </summary>
		/// <param name="tagSequence"></param>
		public void Delete(String tagSequence)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsAttributeMatching)
			{
				throw new HliException("Invalid tag sequence.");
			}

			AttributeCollection attributeCollection = GetAttributes(internalTagSequence);

			foreach(Attribute attribute in attributeCollection)
			{
				attribute.Delete();
			}
		}

        internal string DumpUsingTagSequenceNotation(String objectName, string remarkString)
        {
            return DumpUsingTagSequenceNotation(objectName, remarkString, ".Set", ".AddItem");
        }

		internal String DumpUsingTagSequenceNotation(String objectName, String remarkString,string appendString, String sequenceAppendString)
		{
			String dump = "";

			for (int index = 0; index < Count; index++)
			{
				ValidAttribute validAttribute = this[index] as ValidAttribute;

				if (validAttribute.VR != VR.SQ)
				{
                    dump += objectName + appendString + "(\"" + validAttribute.TagSequence.ToString() + "\", VR." + validAttribute.VR.ToString();

					// Show all the values.
					Values values = validAttribute.Values;

					if (values.Count > 0)
					{
						dump+= ", " + values.ToString();
					}

					dump+= ")";

					// If a name exists, add this as a comment.
					if (validAttribute.Name.Length > 0)
					{
						dump+= " " + remarkString + " " + validAttribute.Name;
						// No need to have a remark string for this line anymore.
						remarkString = "";
					}

					//if (validAttribute.VR == VR.UN)
					//{
					//	dump+= " " + remarkString + " (value not displayed, consists of " + validAttribute.Values.ByteArrayImplementation.Length.ToString() + " bytes)";
					//}

					dump+= "\r\n";
				}
				else
				{
                    dump += objectName + appendString + "(\"" + validAttribute.TagSequence.ToString() + "\", VR." + validAttribute.VR.ToString() + ")";

					// If a name exists, add this as a comment.
					if (validAttribute.Name.Length > 0)
					{
						dump+= " " + remarkString + " " + validAttribute.Name;
					}
	
					dump+= "\r\n";
						
					for (int itemIndex = 1; itemIndex <= validAttribute.ItemCount; itemIndex++)
					{
						SequenceItem item = validAttribute.GetItem(itemIndex);
                        dump += objectName + sequenceAppendString + "(\"" + validAttribute.TagSequence.ToString() + "\", new SequenceItem)\r\n";
						dump+= item.DumpUsingTagSequenceNotation(objectName, remarkString,appendString,sequenceAppendString);
					}
				}
			}

			return(dump);
		}

		/// <summary>
		/// Indicates if the attribute with the supplied tag sequence exists.
		/// </summary>
		/// <param name="tagSequence">The tag sequence.</param>
		/// <returns>Indicates if the attribute exists.</returns>
		public bool Exists(String tagSequence)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsSingleAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			return(Exists(internalTagSequence));
		}

		/// <summary>
		/// Indicates if the attribute with the supplied tag sequence exists.
		/// </summary>
		/// <param name="tagSequence">The tag sequence.</param>
		/// <returns>Indicates if the attribute exists.</returns>
		internal bool Exists(TagSequence tagSequence)
		{
			bool exists = true;

			AttributeCollection attributeCollection = GetAttributes(tagSequence);

			if (attributeCollection.Count == 0)
			{
				exists = false;
			}
			else
			{
				exists = true;
			}

			return(exists);
		}

		/// <summary>
		/// Get an attribute given the tag as UInt32.
		/// </summary>
		internal Attribute GetAttribute(UInt32 tagAsUInt32)
		{
			Attribute attribute = null;

			TagSequence attributeTagSequence = this.tagSequence.Clone();
			attributeTagSequence.Add(new Tag(tagAsUInt32));

			DvtkData.Dimse.Attribute dvtkDataAttribute = this.dvtkDataAttributeSet.GetAttribute(tagAsUInt32);

			if (dvtkDataAttribute == null)
			{
				attribute = new InvalidAttribute(attributeTagSequence);
			}
			else
			{
				attribute = new ValidAttribute(attributeTagSequence, dvtkDataAttribute, this);
			}

			return(attribute);
		}

		/// <summary>
		/// Get all attributes that are refered to by this tag sequence.
		/// 
		/// Precondition for the supplied TagSequence:
		/// All but the last tag contains an index.
		/// The last tag doesn't contain an index.
		/// </summary>
		/// <param name="tagSequence">The tag sequence.</param>
		/// <returns>The attributes.</returns>
		internal AttributeCollection GetAttributes(TagSequence tagSequence)
		{
			AttributeCollection attributeCollection = new AttributeCollection();

			Tag firstTag = (tagSequence.Tags[0]) as Tag;

			Attribute firstAttribute = GetAttribute(firstTag.AsUInt32);

			if (firstAttribute is InvalidAttribute)
			{
				// Do nothing, just return an empty set.
			}
			else
			{
				if (tagSequence.Tags.Count > 1)
					// Expecting a sequence attribute as first tag.
				{
					if (firstAttribute.VR == VR.SQ)
					{
						int fromIndex = 0;
						int toIndex = 0;

						if (firstTag.ContainsWildcardIndex)
						{
							fromIndex = 1;
							toIndex = firstAttribute.ItemCount;
						}
						else
						{
							fromIndex = firstTag.IndexNumber;
							toIndex = firstTag.IndexNumber;
						}

						TagSequence itemTagSequence = tagSequence.Clone();
						itemTagSequence.Tags.RemoveAt(0);

						for (int index = fromIndex; index <= toIndex; index++)
						{
							SequenceItem item =  firstAttribute.GetItem(index);

							AttributeCollection itemAttributeCollection = item.GetAttributes(itemTagSequence);

							attributeCollection.AddRange(itemAttributeCollection);
						}
					}
					else
					{
						// Do nothing, just return an empty set.
					}
				}
				else
				// Expecting a non-sequence attribute as tag.
				{
					attributeCollection.Add(firstAttribute);
				}
			}

			return(attributeCollection);
		}

        /// <summary>
		///		Get a SequenceItem by specifying a tagSequence
		///		<paramref name="tagSequence">tagSequence</paramref>.
		/// </summary>
		/// <param name="tagSequence">
		///		The (tag)Sequence of which you want a SequenceItem.
		/// </param>
		/// <param name="oneBasedIndex">
		///		Specify which SequenceItem is requested. (1 for the first SequenceItem in the sequence)
		/// </param>
		/// <example>
		///		<b>VB .NET</b>
		///		<code>
		///			'Get a SequenceItem by specifying a tagSequence
		///			
		///			'DataSet is inherited from AttributeSet
        ///			Dim myDataSet As DvtkHighLevelInterface.Dicom.Other.DataSet
		///
		///			myDataSet.Read("c:\Somefile.dcm")	
		///	        		
		///			'Get the first Sequence item from the specified Sequence
        ///			Dim mySequenceItem As DvtkHighLevelInterface.Dicom.Other.SequenceItem 
		///
		///        		If myDataSet.Exists("0x00080096") Then
        ///   	 			If myDataSet.GetitemCount("0x00080096") > 0 Then
		///					mySequenceItem = myDataSet.Getitem("0x00080096", 1)
		///				End If
		///			End If
		///		</code>
		/// </example>
		/// <returns>
		///		A Values Object containing a list of all values the attribute contains.
		/// </returns>
		/// <exception cref="HliException">
		///		Tag sequence supplied invalid for this operation.
		/// </exception>
		///<remarks>
		///	If the attribute requested by the tagSequence is non existent or invalid an empty SequenceItem will be returned.
		///</remarks>
		public SequenceItem Getitem(String tagSequence, int oneBasedIndex)
		{
            //
            // Sanity checks.
            //

            if (tagSequence == null)
            {
                throw new ArgumentNullException("tagSequence");
            }

            TagSequence internalTagSequence = new TagSequence(tagSequence);

            if (!internalTagSequence.IsValid)
            {
                throw new ArgumentException("Tag sequence is not valid", "tagSequence");
            }

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException("Tag sequence must specify a single attribute", "tagSequence");
            }

            if (oneBasedIndex == 0)
            {
                throw new ArgumentException("Index is one based", "oneBasedIndex");
            }


            //
            // Perform the actual operation.
            //

			SequenceItem sequenceItem = null;

			ValidAttribute validAttribute = this[internalTagSequence] as ValidAttribute;

			if (validAttribute == null)
			{
				sequenceItem = new SequenceItem();
				Thread.WriteWarningCurrentThread("Trying to get a sequence item for an invalid attribute with tag sequence " + tagSequence + ". Returning an empty sequence item.");
			}
			else
			{
				sequenceItem = validAttribute.GetItem(oneBasedIndex);
			}

			return(sequenceItem);
		}

        /// <summary>
		///		Get the number of SequenceItems by specifying a tagSequence
		///		<paramref name="tagSequence">tagSequence</paramref>.
		/// </summary>
		/// <param name="tagSequence">
		///		The (tag)Sequence of which you want a SequenceItem.
		/// </param>
		/// <example>
		///		<b>VB .NET</b>
		///		<code>
		///			'Get a SequenceItem by specifying a tagSequence
		///			
		///			'DataSet is inherited from AttributeSet
        ///			Dim myDataSet As DvtkHighLevelInterface.Dicom.Other.DataSet
		///
		///			myDataSet.Read("c:\Somefile.dcm")
		///	        		
		///			'Get the number of sequence items 
		///			Dim nrOfSequenceItems as integer
        ///			If myDataSet.Exists(("0x00080096")) Then
        ///	 	  	 	nrOfSequenceItems = myDataSet.GetitemCount("0x00080096")
        ///			End If		
		///		</code>
		/// </example>
		/// <returns>
		///		A Values Object containing a list of all values the attribute contains.
		/// </returns>
		/// <exception cref="HliException">
		///		Tag sequence supplied invalid for this operation..
		/// </exception>
		///<remarks>
		///	If the attribute requested by the tagSequence is non existent or invalid 0 will be returned.
		///</remarks>
		public int GetitemCount(String tagSequence)
		{
            //
            // Sanity checks.
            //

            if (tagSequence == null)
            {
                throw new ArgumentNullException("tagSequence");
            }

            TagSequence internalTagSequence = new TagSequence(tagSequence);

            if (!internalTagSequence.IsValid)
            {
                throw new ArgumentException("Tag sequence is not valid", "tagSequence");
            }

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException("Tag sequence must specify a single attribute", "tagSequence");
            }


            //
            // Perform the actual operation.
            //

			int itemCount = 0;

			ValidAttribute validAttribute = this[internalTagSequence] as ValidAttribute;

			if (validAttribute == null)
			{
				itemCount = 0;
				Thread.WriteWarningCurrentThread("Getting the item count for an invalid attribute. Returning 0.");
			}
			else
			{
				itemCount = validAttribute.ItemCount;
			}

			return(itemCount);
		}

        /// <summary>
		///		Get the Values of the attribute	specified in the 
		///		<paramref name="tagSequence">tagSequence</paramref>.
		/// </summary>
		/// <param name="tagSequence">
		///		The tagsequence of the attribute of which you want the values.
		/// </param>
		/// <example>
		///		<b>VB .NET</b>
		///		<code>
		///			' Example: Get the Values of the attribute with a 
		///			' specified tagSequence
		///			
		///			'DataSet is inherited from AttributeSet
        ///			Dim myDataSet As DvtkHighLevelInterface.Dicom.Other.DataSet
		///			myDataSet.Read("c:\Somefile.dcm")		
        ///
		///			Dim myValues As DvtkHighLevelInterface.Dicom.Other.Values
		///
		///			If mydataset.Exists(("0x00100010")) Then
        ///   	 			myValues = myDataSet.GetValues("0x00100010")
		///				'Now we have a Values object which contains all the values of the specified attribute
        ///			End If				
		///			
		///
		///			'To get the first value from the list (use this also when the attribute can only contain 1 value)
        ///			Dim firstValue As String 
		///			If mydataset.Exists(("0x00100010")) Then
		///				if mydataset("0x00100010").Values.Count >0 then
		///					firstValue = myValues(0)
		///				End If
		///			End If
		///		</code>
		/// </example>
		/// <returns>
		///		A Values Object containing a list of all values the attribute contains.
		/// </returns>
		/// <exception cref="HliException">
		///		Tag sequence supplied invalid for this operation..
		/// </exception>
		public Values GetValues(String tagSequence)
		{
			Values values = null;

			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsSingleAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			Attribute attribute = this[internalTagSequence];

			values = new Values(attribute);

			return(values);
		}

        /// <summary>
		///		Get the Value Multiplicity of the attribute specified in the 
		///		<paramref name="tagSequence">tagSequence</paramref>.
		/// </summary>
		/// <param name="tagSequence">
		///		The tagsequence of the attribute of which you want the Value Multiplicity.
		/// </param>
		/// <example>
		///		<b>VB .NET</b>
		///		<code>
		///			' Example: Get the Value Multiplicity of an attribute with a 
		///			'specified tagSequence
		///			
		///			'DataSet is inherited from AttributeSet
        ///			Dim myDataSet As DvtkHighLevelInterface.Dicom.Other.DataSet
		///			myDataSet.Read("c:\Somefile.dcm")
		///
		///      		Dim myVM As Integer
		///        		If myDataSet.Exists("0x00100010") Then
		///      	 		myVM = myDataSet.GetVM("0x00100010")
		///			End If
		///        		'alternatively you can use
		///			If myDataSet.Exists("0x00100010") Then
		///        			myVM = myDataSet("0x00100010").VM
		///			End If
		///		</code>
		/// </example>
		/// <returns>
		///		The Value Multiplicty.
		/// </returns>
		/// <exception cref="HliException">
		///		Tag sequence supplied invalid for this operation..
		/// </exception>
		public int GetVM(String tagSequence)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsSingleAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			Attribute attribute = this[internalTagSequence];

			return(attribute.VM);
		}

        /// <summary>
		///		Get the Value Representation of the attribute specified in the 
		///		<paramref name="tagSequence">tagSequence</paramref>.
		/// </summary>
		/// <param name="tagSequence">
		///		The tagsequence of the attribute of which you want the Value Representation.
		/// </param>
		/// <example>
		///		<b>VB .NET</b>
		///		<code>
		///			' Example: Get the Value Representation of an attribute with a 
		///			' specified tagSequence
		///			
		///			'DataSet is inherited from AttributeSet
        ///			Dim myDataSet As DvtkHighLevelInterface.Dicom.Other.DataSet
		///			myDataSet.Read("c:\Somefile.dcm")
        ///			
        ///			Dim myVR As DvtkData.Dimse.VR
		///			If myDataSet.Exists("0x00100010") Then
        ///				myVR = myDataSet.GetVR("0x00100010")
		///			End If
		///
        ///			'alternatively you can use
		///			If myDataSet.Exists("0x00100010") Then
        ///				myVR = myDataSet("0x00100010").VR
		///			End If
		///
		///		</code>
		/// </example>
		/// <returns>
		///		The Value Representation.
		/// </returns>
		/// <exception cref="HliException">
		///		Tag sequence supplied invalid for this operation..
		/// </exception>
		public VR GetVR(String tagSequence)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsSingleAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			Attribute attribute = this[internalTagSequence];

			return(attribute.VR);
		}

        /// <summary>
        /// Inserts values for all attributes that are refered to by the supplied taf sequence.
        /// </summary>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="zeroBasedIndex">The zero-based index to insert the values at.</param>
        /// <param name="values">The values to insert.</param>
		public void InsertValues(String tagSequence, int zeroBasedIndex, Values values)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			AttributeCollection attributeCollection = GetAttributes(internalTagSequence);

			foreach(Attribute attribute in attributeCollection)
			{
				attribute.Values.Insert(zeroBasedIndex, values);
			}
		}

        /// <summary>
        /// Inserts values for all attributes that are refered to by the supplied taf sequence.
        /// </summary>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="zeroBasedIndex">The zero-based index to insert the values at.</param>
        /// <param name="parameters">The values to insert.</param>
		public void InsertValues(String tagSequence, int zeroBasedIndex, params Object[] parameters)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			AttributeCollection attributeCollection = GetAttributes(internalTagSequence);

			foreach(Attribute attribute in attributeCollection)
			{
				attribute.Values.Insert(zeroBasedIndex, parameters);
			}
		}

		/// <summary>
		/// Make attributes in this AttributeSet ascending (Dicom compliant).
		/// </summary>
		/// <param name="recursive">
		/// When this is true, all attributes are made ascending resursively,
		/// i.e. all contained sequence items are also sorted.
		/// </param>
		public void MakeAscending(bool recursive)
		{
			this.dvtkDataAttributeSet.MakeAscending();

			if (recursive)
			{
				for (int index = 0; index < Count; index++)
				{
					Attribute attribute = this[index];

					if (attribute.VR == VR.SQ)
					{
						for (int sequenceItemIndex = 1; sequenceItemIndex <= attribute.ItemCount; sequenceItemIndex++)
						{
							attribute.GetItem(sequenceItemIndex).MakeAscending(true);
						}
					}
				}
			}
		}

        /// <summary>
        /// Randomizes all attributes contained in this instance.
        /// </summary>
        /// <remarks>
        /// Randomization is performed by replacing each <paramref name="stringToReplace"/> in the values of the attributes contained
        /// with a random digit between 0 and 9.
        /// </remarks>
        /// <param name="stringToReplace">The String to replace (may not be empty).</param>
		public void Randomize(String stringToReplace)
		{
            //
            // Sanity checks.
            //

            if (stringToReplace == null)
            {
                throw new ArgumentNullException("stringToReplace");
            }
            else
            {
                if (stringToReplace.Length == 0)
                {
                    throw new ArgumentException("String may not be empty", "stringToReplace");
                }
            }


            //
            // Perform the randomization.
            //
            
            Random random = new Random();

			Randomize(stringToReplace, random);
		}
		
        /// <summary>
        /// Randomizes all attributes contained in this instance.
        /// </summary>
        /// <remarks>
        /// Randomization is performed by replacing each <paramref name="stringToReplace"/> in the values of the attributes contained
        /// with a random digit between 0 and 9.
        /// </remarks>
        /// <param name="stringToReplace">The String to replace (may not be empty).</param>
        /// <param name="random">A Random instance.</param>
		public void Randomize(String stringToReplace, Random random)
		{
            //
            // Sanity checks.
            //
             
            if (stringToReplace == null)
            {
                throw new ArgumentNullException("stringToReplace");
            }
            else
            {
                if (stringToReplace.Length == 0)
                {
                    throw new ArgumentException("String may not be empty", "stringToReplace");
                }
            }

            if (random == null)
            {
                throw new ArgumentNullException("random");
            }


            //
            // Perform the randomization.
            //

			for (int index = 0; index < Count; index++)
			{
				ValidAttribute validAttribute = this[index] as ValidAttribute;

				if (validAttribute.VR == VR.SQ)
				{
					for (int itemIndex = 1; itemIndex <= validAttribute.ItemCount; itemIndex++)
					{
						validAttribute.GetItem(itemIndex).Randomize(stringToReplace, random);
					}
				}
				else
				{
					validAttribute.Values.Randomize(stringToReplace, random);
				}
			}
		}

        /// <summary>
        /// Remove values for all attributes that are refered to by the supplied tag sequence.
        /// </summary>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="zeroBasedIndex">The zero-based index to remove the value.</param>
		public void RemoveValueAt(String tagSequence, int zeroBasedIndex)
		{
			TagSequence internalTagSequence = new TagSequence(tagSequence);

			if (!internalTagSequence.IsAttributeMatching)
			{
				throw new HliException("Tag sequence supplied invalid for this operation.");
			}

			AttributeCollection attributeCollection = GetAttributes(internalTagSequence);

			foreach(Attribute attribute in attributeCollection)
			{
				attribute.Values.RemoveAt(zeroBasedIndex);
			}
		}



        internal virtual void Set<T>(TagSequence tagSequence, VR vR, T genericsParameter)
        {
            AttributeSet currentAttributeSetInLoop = this;

            for (int index = 0; index < tagSequence.Tags.Count; index++)
            {
                Tag tag = tagSequence[index];

                if (index < tagSequence.Tags.Count - 1)
                // If this is not the last tag from the TagSequence...
                {
                    Attribute attribute = currentAttributeSetInLoop.SetSequenceAttribute(tag);

                    currentAttributeSetInLoop = attribute.GetItem(tag.IndexNumber);
                }
                else
                // If this is the last tag from the TagSequence...
                {
                    Attribute attribute = currentAttributeSetInLoop.GetAttribute(tag.AsUInt32);

                    if (attribute is ValidAttribute)
                    {
                        attribute.Delete();
                    }

                    attribute = currentAttributeSetInLoop.Create(tag.AsUInt32, vR);


                    // The type of genericsParameter (generics type) is determined run-time.
                    // Maybe this is the reason for a direct call to attribute.Values.Add 
                    // (has a overloaded method with params parameter) with the argument 
                    // genericsParameter not to work. Needed to explicitly cast to a specfic type
                    // before calling the Add method.
                    if (genericsParameter is Values)
                    {
                        Values values = genericsParameter as Values;

                        attribute.Values.Add(values);
                    }
                    else if (genericsParameter is Byte[])
                    {
                        Byte[] value = genericsParameter as Byte[];

                        attribute.Values.Set(value);
                    }
                    else if (genericsParameter is Object[])
                    {
                        Object[] values = genericsParameter as Object[];

                        attribute.Values.Add(values);
                    }
                    else
                    {
                        throw new HliException("Not supposed to get here.");
                    }
                }
            }
        }

        /// <summary>
        /// See comments for this overridden method in the classes CommandSet, DataSet, SequenceItem and FileMetaInformation.
        /// </summary>
        /// <param name="dvtkDataTag">-</param>
        /// <param name="vR">-</param>
        /// <param name="value">-</param>
        public abstract void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Byte[] value);
        
        /// <summary>
        /// See comments for this overridden method in the classes CommandSet, DataSet, SequenceItem and FileMetaInformation.
        /// </summary>
        /// <param name="dvtkDataTag">-</param>
        /// <param name="vR">-</param>
        /// <param name="values">-</param>
        public abstract void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, params Object[] values);

        /// <summary>
        /// See comments for this overridden method in the classes CommandSet, DataSet, SequenceItem and FileMetaInformation.
        /// </summary>
        /// <param name="dvtkDataTag">-</param>
        /// <param name="vR">-</param>
        /// <param name="values">-</param>
        public abstract void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Values values);

        /// <summary>
        /// See comments for this overridden method in the classes CommandSet, DataSet, SequenceItem and FileMetaInformation.
        /// </summary>
        /// <param name="tagSequence">-</param>
        /// <param name="vR">-</param>
        /// <param name="value">-</param>
        public abstract void Set(String tagSequence, VR vR, Byte[] value);

        /// <summary>
        /// See comments for this overridden method in the classes CommandSet, DataSet, SequenceItem and FileMetaInformation.
        /// </summary>
        /// <param name="tagSequence">-</param>
        /// <param name="vR">-</param>
        /// <param name="values">-</param>
        public abstract void Set(String tagSequence, VR vR, params Object[] values);

        /// <summary>
        /// See comments for this overridden method in the classes CommandSet, DataSet, SequenceItem and FileMetaInformation.
        /// </summary>
        /// <param name="tagSequence">-</param>
        /// <param name="vR">-</param>
        /// <param name="values">-</param>
        public abstract void Set(String tagSequence, VR vR, Values values);

		/// <summary>
		/// Private method used by the Set method. This methods sets an attribute
		/// to a Sequence Attribute and makes sure it contains enough sequence items.
		/// </summary>
		/// <param name="tag">The tag of the Attribute.</param>
		/// <returns>The already existing or newly created sequence item.</returns>
		private Attribute SetSequenceAttribute(Tag tag)
		{
			Attribute attribute = GetAttribute(tag.AsUInt32);

			if (!(attribute is ValidAttribute))
			{
				attribute = Create(tag.AsUInt32, VR.SQ);
			}
			else if (attribute.VR != VR.SQ)
			{
				attribute.Delete();

				attribute = Create(tag.AsUInt32, VR.SQ);
			}

			// At this time, we have made sure that attribute is a Sequence Attribute.
			// Now take care that it contains enough Sequence Items.

			for (int sequenceIndex = attribute.ItemCount; sequenceIndex < tag.IndexNumber; sequenceIndex++)
			{
				attribute.AddItem(new SequenceItem());
			}

			return(attribute);
		}
	}
}
