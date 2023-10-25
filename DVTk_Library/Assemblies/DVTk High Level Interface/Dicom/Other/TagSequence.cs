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

using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// Unique identifier for
    /// - A single attribute.
    /// - An Attribute Set (CommandSet, DataSet, FileMetaInformation or Sequence Item).
    /// - All Sequence Items within a Sequence Attribute.
	/// 
    /// Examples of String representation of possible instances of this class:
    /// - "0x00100020": identifies a single attribute with Dicom Tag (0010,0020).
    /// - "0x00081199[1]/0x00081150": identifies a single attribute with Dicom Tag (0008,1150) present
    ///   within the first Sequence Item of Sequence Attribute (0008,1199).
    /// - "": identifies the root of a CommandSet, DataSet or FileMetaInformation.
    /// - "0x00081199[1]": first Sequence Item of Sequence Attribute (0008,1199).
    /// - "0x00081199[]": all Sequence Items of Sequence Attribute (0008,1199).
	/// </summary>
	internal class TagSequence
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property Tags.
		/// </summary>
		private ArrayList tags = new ArrayList();

		/// <summary>
		/// Boolean indicating if this TagSequence contains valid tags.
        /// 
		/// Note that although the contained tags may all be valid, the combination of these
		/// tags may not.
		/// </summary>
		private bool containsValidTags = true;



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// 
        /// Represents a CommandSet, DataSet or FileMetaInformation.
		/// </summary>
		public TagSequence()
		{
			// Do nothing.
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tagSequenceAsString">The TagSequence represented as a String.</param>
		public TagSequence(String tagSequenceAsString)
		{
			String[] tagsAsString = tagSequenceAsString.Split('/');

			foreach (String tagAsString in tagsAsString)
			{
				Tag tag = new Tag(tagAsString);

				if (!tag.IsValid)
				{
					this.containsValidTags = false;
					Thread.WriteWarningCurrentThread("Tag " + tagAsString + " in tag sequence " + tagSequenceAsString + " is not valid.");
					break;
				}
				else
				{
					Add(tag);
				}
			}
		}
		


		//
		// - Properties -
		//

        /// <summary>
        /// Gets the notation as used in the Dicom standard, i.e.
        /// use (xxxx,yyyy) as the tag format and '>' to indicate how many levels deep an attribute
        /// is in sequence attributes.
        /// </summary>
        public String DicomNotation
        {
            get
            {
                String dicomNotation = "";

                for (int index = 1; index < this.tags.Count; index++)
                {
                    dicomNotation += ">";
                }

                dicomNotation += (tags[tags.Count - 1] as Tag).DicomNotation;

                return (dicomNotation);
            }
        }

        /// <summary>
        /// Boolean indicating if this TagSequence is attribute matching i.e. it can
        /// be used to identify zero or more attributes.
        /// </summary>
        public bool IsAttributeMatching
        {
            get
            {
                bool isAttributeMatching = true;

                if (this.containsValidTags)
                {
                    if (this.tags.Count == 0)
                    {
                        isAttributeMatching = false;
                    }
                    else
                    {
                        for (int index = 0; index < this.tags.Count - 1; index++)
                        {
                            Tag tag = this.tags[index] as Tag;

                            if (!tag.ContainsIndex)
                            {
                                isAttributeMatching = false;
                                break;
                            }
                        }

                        if ((this.tags[this.Tags.Count - 1] as Tag).ContainsIndex)
                        {
                            isAttributeMatching = false;
                        }
                    }
                }
                else
                {
                    isAttributeMatching = false;
                }

                return (isAttributeMatching);
            }
        }

		/// <summary>
		/// Boolean indicating if this TagSequence is single attribute matching i.e. exactly
		/// one attribute is indicated by the TagSequence.
		/// </summary>
		public bool IsSingleAttributeMatching
		{
			get
			{
				bool isSingleAttributeMatching = true;

				if (!IsAttributeMatching)
				{
					isSingleAttributeMatching = false;
				}
				else
				{
					for (int index = 0; index < this.tags.Count - 1; index++)
					{
						Tag tag = this.tags[index] as Tag;

						if (tag.ContainsWildcardIndex)
						{
							isSingleAttributeMatching = false;
							break;
						}
					}
				}

				return(isSingleAttributeMatching);
			}
		}

        public bool IsValid
        {
            get { return this.containsValidTags ; }
            set { this.containsValidTags  = value; }
        }
	
		/// <summary>
		/// Indicates if this TagSequence is valid for a CommandSet attribute.
        /// 
		/// Only the group numbers of the individual tags contained in this TagSequence
		/// are checked by this property.
		/// </summary>
		public bool IsValidForCommandSet
		{
			get
			{
				bool isValid = true;

				foreach (Tag tag in this.Tags)
				{
					if (!tag.IsValidForCommandSet)
					{
						isValid = false;
						break;
					}
				}

				return(isValid);
			}
		}

		/// <summary>
		/// Indicates if this TagSequence is valid for a DataSet attribute.
        /// 
		/// Only the group numbers of the individual tags contained in this TagSequence
		/// are checked by this property.
		/// </summary>
		public bool IsValidForDataSet
		{
			get
			{
				bool isValid = true;

				foreach (Tag tag in this.Tags)
				{
					if (!tag.IsValidForDataSet)
					{
						isValid = false;
						break;
					}
				}

				return(isValid);
			}
		}

        /// <summary>
        /// Indicates if this TagSequence is valid for a DirectoryRecord attribute.
        /// 
        /// Only the group numbers of the individual tags contained in this TagSequence
        /// are checked by this property.
        /// </summary>
        public bool IsValidForDirectoryRecord
        {
            get
            {
                bool isValid = true;

                foreach (Tag tag in this.Tags)
                {
                    if (!tag.IsValidForDirectoryRecord)
                    {
                        isValid = false;
                        break;
                    }
                }

                return (isValid);
            }
        }

		/// <summary>
		/// Indicates if this TagSequence is valid for a FileMetaInformation attribute.
        /// 
		/// Only the group numbers of the individual tags contained in this TagSequence
		/// are checked by this property.
		/// </summary>
		public bool IsValidForFileMetaInformation
		{
			get
			{
				bool isValid = true;

				foreach (Tag tag in this.Tags)
				{
					if (!tag.IsValidForFileMetaInformation)
					{
						isValid = false;
						break;
					}
				}

				return(isValid);
			}
		}


        /// <summary>
        /// Gets the last Tag contained in this instance.
        /// 
        /// If this instance contains no Tags, a Tag with group number 0 and element number 0
        /// is returned.
        /// </summary>
		internal Tag LastTag
		{
			get
			{
				Tag tag = new Tag(0);

				if (Tags.Count > 0)
				{
					tag = Tags[Tags.Count - 1] as Tag;
				}

				return(tag);
			}
		}

		/// <summary>
		/// Gets the tags contained in this TagSequence.
		/// </summary>
		internal ArrayList Tags
		{
			get
			{
				return(this.tags);
			}
		}		

		/// <summary>
		/// Gets the tag with the specified zero based index.
		/// </summary>
		public Tag this[int zeroBasedIndex]
		{
			get 
			{
				return(this.tags[zeroBasedIndex] as Tag);
			}
		}

		
		
		//
		// - Methods -
		//		

		/// <summary>
		/// Adds a tag to the end of the list.
		/// </summary>
		/// <param name="tag">The tag to add.</param>
		internal void Add(Tag tag)
		{
			this.tags.Add(tag);
		}

		/// <summary>
		/// Creates a deep copy of this TagSequence.
		/// </summary>
		/// <returns>The created deep copy.</returns>
		internal TagSequence Clone()
		{
			TagSequence cloneTagSequence = new TagSequence();

			foreach(Tag tag in this.tags)
			{
				Tag cloneTag = tag.Clone();
				cloneTagSequence.Add(cloneTag);
			}

			return(cloneTagSequence);
		}

        /// <summary>
        /// Returns a String that represents this instance.
        /// </summary>
        /// <returns>A String that represents this instance.</returns>
		public override string ToString()
		{
			String toString = "";

			for (int index = 0; index < tags.Count; index++)
			{
				Tag tag = tags[index] as Tag;

				toString+= tag.ToString();

				if (index < tags.Count - 1)
				{
					toString+= "/";
				}
			}

			return(toString);
		}
	}
}
