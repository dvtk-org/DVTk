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

using DvtkHighLevelInterface.Common.Threads;


namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// Represents a Dicom tag.
    /// 
    /// May also represent a specific sequence item in a Sequence Attribute or all
    /// sequence items in a Sequence Attribute when used in a TagSequence (that is
    /// combining multiple tags).
	/// </summary>
	public class Tag
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property ContainsIndex.
		/// </summary>
		private bool containsIndex = false;

		/// <summary>
		/// See property ContainsWildcardIndex.
		/// </summary>
		private bool containsWildcardIndex = false;

		/// <summary>
		/// See property ElementNumber.
		/// </summary>
		private UInt16 elementNumber = 0;

		/// <summary>
		///  See property GroupNumber.
		/// </summary>
		private UInt16 groupNumber = 0;

		/// <summary>
		/// See property IndexNumber.
		/// </summary>
		private int indexNumber = 0;

		/// <summary>
		/// See property IsValid.
		/// </summary>
		private bool isValid = true;



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default Constructor.
		/// </summary>
		private Tag()
		{
		}

		/// <summary>
		/// Constructor.
        /// 
        /// This instance will represent a Dicom Tag.
		/// </summary>
		/// <param name="tagAsUInt32">The tag specified as an UInt32.</param>
		public Tag(UInt32 tagAsUInt32)
		{
			this.elementNumber = Convert.ToUInt16(tagAsUInt32%65536);
			this.groupNumber = Convert.ToUInt16(tagAsUInt32/65536);
		}

		/// <summary>
		/// Constructor.
        /// 
        /// This instance will represent a Dicom Tag.
		/// </summary>
		/// <param name="groupNumber">Group number of the tag.</param>
		/// <param name="elementNumber">Element number of the tag.</param>
		public Tag(UInt16 groupNumber, UInt16 elementNumber)
		{
			this.elementNumber = elementNumber;
			this.groupNumber = groupNumber;
		}

		/// <summary>
		/// Constructor.
        /// 
        /// Specifies the tag as a String.
        /// If no index is used, this instance will represent a Dicom Tag: "0xggggeeee".
        /// If an index is used, this instance will represent a specific Dicom Sequence Item
        /// in a Sequence Attribute: "0xggggeeee[i]".
        /// If a wildcard index is used, this instance will represent all Dicom Sequence Item present
        /// in a Sequence Attribute: "0xggggeeee[]".
        /// 
        /// - gggg is a four digit hexadecimal group number.
        /// - eeee is a four digit hexadecimal element number.
        /// - i is a one-based sequence item index.
		/// </summary>
		/// <param name="tagAsString">The tag specified as a String.</param>
		public Tag(String tagAsString)
		{
			String elementAndGroupNumberOnly = "";

			// Check if the tag starts with "0x".
			if (!tagAsString.StartsWith("0x"))
			{
				this.isValid = false;
				Thread.WriteWarningCurrentThread("Invalid tag: " + tagAsString + " does not start with 0x.");
			}

			if (this.isValid)
			{
				// If this tag contains an index, determine it.
				int indexOfBracket = tagAsString.IndexOf("[");

				if (indexOfBracket == -1)
				{
					// No index exists.
					elementAndGroupNumberOnly = tagAsString;
				}
				else
				{
					// Index exists.

					this.containsIndex = true;

					try
					{
						elementAndGroupNumberOnly = tagAsString.Substring(0, indexOfBracket);

						String indexNumberAsString = tagAsString.Substring(indexOfBracket + 1);

						indexNumberAsString = indexNumberAsString.Substring(0, indexNumberAsString.Length - 1);

						if (indexNumberAsString.Length == 0)
						{
							this.containsWildcardIndex = true;
						}
						else
						{
							this.indexNumber = Convert.ToInt32(indexNumberAsString);

							if (this.indexNumber < 1)
							{
								this.isValid = false;
								Thread.WriteWarningCurrentThread("Invalid tag: item number of " + tagAsString + " is < 1.");
							}
						}
					}
					catch (System.Exception exception)
					{
						Thread.WriteWarningCurrentThread("Invalid tag: " + tagAsString + " (" + exception.Message + ")");
						this.isValid = false;
					}
				}
			}

			try
			{
				UInt32 tagAsUInt32 = Convert.ToUInt32(elementAndGroupNumberOnly, 16);

				this.elementNumber = Convert.ToUInt16(tagAsUInt32%65536);
				this.groupNumber = Convert.ToUInt16(tagAsUInt32/65536);
			}
			catch (System.Exception exception)
			{
				Thread.WriteWarningCurrentThread("Invalid tag: " + tagAsString + " (" + exception.Message + ")");
				this.isValid = false;
			}
		}



		//
		// - Properties -
		//

        /// <summary>
        /// Gets the String representation of this instance.
        /// </summary>
		public String AsString
		{
			get
			{
				String asString = "";

				String groupNumberAsAtring = Convert.ToString(this.GroupNumber, 16);
				groupNumberAsAtring = groupNumberAsAtring.PadLeft(4, '0');

				String elementNumberAsAtring = Convert.ToString(this.ElementNumber, 16);
				elementNumberAsAtring = elementNumberAsAtring.PadLeft(4, '0');

				if (ContainsIndex)
				{
					asString = String.Format("0x{0}{1}[{2}]", groupNumberAsAtring, elementNumberAsAtring, this.indexNumber);
				}
				else if (ContainsWildcardIndex)
				{
					asString = String.Format("0x{0}{1}[]", groupNumberAsAtring, elementNumberAsAtring);
				}
				else
				{
					asString = String.Format("0x{0}{1}", groupNumberAsAtring, elementNumberAsAtring);
				}

				return(asString);
			}	
		}

		/// <summary>
        /// Gets the UInt32 representation of this instance.
		/// </summary>
		public UInt32 AsUInt32
		{
			get
			{
				return(Convert.ToUInt32((Convert.ToUInt32(this.GroupNumber) * 65536) + this.elementNumber));
			}
		}

		/// <summary>
		/// Indicates if this instance contains an index.
		/// </summary>
		public bool ContainsIndex
		{
			get
			{
				return (this.containsIndex);
			}
		}

		/// <summary>
		/// Indicates if this instance contains a wildcard index.
		/// </summary>
		public bool ContainsWildcardIndex
		{
			get
			{
				return (this.containsWildcardIndex);
			}
		}

		/// <summary>
        /// Gets the Dicom Standard String representation of this instance,
        /// i.e. using the format "(gggg,eeee)".
		/// </summary>
		public String DicomNotation
		{
			get
			{
				String dicomNotation = "";

				String groupNumberAsAtring = Convert.ToString(this.GroupNumber, 16);
				groupNumberAsAtring = groupNumberAsAtring.PadLeft(4, '0');

				String elementNumberAsAtring = Convert.ToString(this.ElementNumber, 16);
				elementNumberAsAtring = elementNumberAsAtring.PadLeft(4, '0');

				dicomNotation = String.Format("({0},{1})", groupNumberAsAtring, elementNumberAsAtring).ToUpper();

				return(dicomNotation);
			}
		}


		/// <summary>
		/// Gets the element number of this instance.
		/// </summary>
		public UInt16 ElementNumber
		{
			get
			{
				return (this.elementNumber);
			}
		}

		/// <summary>
		/// Gets the group number of this instance.
		/// </summary>
		public UInt16 GroupNumber
		{
			get
			{
				return (this.groupNumber);
			}
		}

		/// <summary>
		/// Gets or sets the index number of this instance.
		/// The get property is only valid if ContainsIndex returns true.
		/// </summary>
		public int IndexNumber
		{
			get
			{
				return (this.indexNumber);
			}
			set
			{
				this.indexNumber = value;
				this.containsIndex = true;
			}
		}

		/// <summary>
		/// Indicates if this instance has been constructed with valid parameters.
		/// </summary>
		public bool IsValid
		{
			get
			{
				return (this.isValid);
			}
		}

		/// <summary>
		/// Indicates if this instance is valid for a CommandSet attribute.
		/// </summary>
		public bool IsValidForCommandSet
		{
			get
			{
				bool isValid = true;

				if (GroupNumber == 0x0000)
				{
					isValid = true;
				}
				else
				{
					isValid = false;
				}

				return(isValid);
			}
		}

		/// <summary>
		/// Indicates if this instance is valid for a DataSet attribute.
		/// </summary>
		public bool IsValidForDataSet
		{
			get
			{
				bool isValid = true;

				if ((GroupNumber == 0x0000) ||
					(GroupNumber == 0x0001) ||
					(GroupNumber == 0x0002) ||
					(GroupNumber == 0x0003) ||
					(GroupNumber == 0x0005) ||
					(GroupNumber == 0x0006) ||
					(GroupNumber == 0x0007) ||
					(GroupNumber == 0xFFFF))
				{
					isValid = false;
				}
				else
				{
					isValid = true;
				}

				return(isValid);
			}
		}

        /// <summary>
        /// Indicates if this instance is valid for a DirectoryRecord attribute.
        /// </summary>
        public bool IsValidForDirectoryRecord
        {
            get
            {
                bool isValid = true;

                if ((GroupNumber == 0x0000) ||
                    (GroupNumber == 0x0001) ||
                    (GroupNumber == 0x0002) ||
                    (GroupNumber == 0x0003) ||
                    (GroupNumber == 0x0005) ||
                    (GroupNumber == 0x0006) ||
                    (GroupNumber == 0x0007) ||
                    (GroupNumber == 0xFFFF))
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }

                return (isValid);
            }
        }

		/// <summary>
		/// Indicates if this instance is valid for a FileMetaInformation attribute.
		/// </summary>
		public bool IsValidForFileMetaInformation
		{
			get
			{
				bool isValid = true;

				if (GroupNumber == 0x0002)
				{
					isValid = true;
				}
				else
				{
					isValid = false;
				}

				return(isValid);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Create a deep copy of this tag.
		/// </summary>
		/// <returns>The created deep copy.</returns>
		internal Tag Clone()
		{
			Tag cloneTag = new Tag();

			cloneTag.containsIndex = this.containsIndex;
			cloneTag.containsWildcardIndex = this.containsWildcardIndex;
			cloneTag.elementNumber = this.elementNumber;
			cloneTag.groupNumber = this.groupNumber;
			cloneTag.indexNumber = this.indexNumber;
			cloneTag.isValid = this.isValid;

			return (cloneTag);
		}

        /// <summary>
        /// Returns a String that represents this instance. 
        /// </summary>
        /// <returns>A String that represents this instance.</returns>
		public override String ToString()
		{
			String toString = "";

			String groupAndElement = Convert.ToString(AsUInt32, 16).ToUpper();
			groupAndElement = groupAndElement.PadLeft(8, '0');
			toString = groupAndElement = "0x" + groupAndElement;

			if (this.containsIndex)
			{
				if(this.containsWildcardIndex)
				{
					toString+= @"[]";
				}
				else
				{
					toString+= @"[" + this.IndexNumber.ToString() + @"]";
				}
			}
			
			return(toString);
		}
	}
}
