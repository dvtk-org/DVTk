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
using System.IO;

using DvtkData.Validation;
using DvtkData.DvtDetailToXml;
using DvtkData.Dimse;
using DvtkData.Validation.TypeSafeCollections;

namespace DvtkData.ComparisonResults
{
	/// <summary>
	/// Summary description for AttributeComparisonResults.
	/// </summary>
	public class AttributeComparisonResults : IDvtDetailToXml, IDvtSummaryToXml
	{
		private Tag _tag = null;
		private System.String _segment = System.String.Empty;
		private int _fieldIndex = -1;
		private System.String _name = System.String.Empty;
		private System.String _value1 = System.String.Empty;
		private System.String _value2 = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="tag">Attribute Tag.</param>
		/// <param name="value1">First Compared Attribute Value.</param>
		/// <param name="value2">Second Compared Attribute Value.</param>
		public AttributeComparisonResults(Tag tag, System.String value1, System.String value2)
		{
			_tag = tag;
			_value1 = value1;
			_value2 = value2;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="segment">HL7 Segment Name.</param>
		/// <param name="fieldIndex">Hl7 Segment Field Index.</param>
		/// <param name="value1">First Compared Attribute Value.</param>
		/// <param name="value2">Second Compared Attribute Value.</param>
		public AttributeComparisonResults(System.String segment, int fieldIndex, System.String value1, System.String value2)
		{
			_tag = Tag.UNDEFINED;
			_segment = segment;
			_fieldIndex = fieldIndex;
			_value1 = value1;
			_value2 = value2;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="tag">Attribute Tag.</param>
		/// <param name="segment">HL7 Segment Name.</param>
		/// <param name="fieldIndex">Hl7 Segment Field Index.</param>
		/// <param name="value1">First Compared Attribute Value.</param>
		/// <param name="value2">Second Compared Attribute Value.</param>
		public AttributeComparisonResults(Tag tag, System.String segment, int fieldIndex, System.String value1, System.String value2)
		{
			_tag = tag;
			_segment = segment;
			_fieldIndex = fieldIndex;
			_value1 = value1;
			_value2 = value2;
		}

		/// <summary>
		/// Tag property - Get only
		/// </summary>
		public Tag Tag
		{
			get
			{
				return _tag;
			}
		}

		/// <summary>
		/// Name property - Set only
		/// </summary>
		public System.String Name
		{
			set
			{
				_name = value;
			}
		}

		/// <summary>
		/// Validation informative messages.
		/// </summary>
		public ValidationMessageCollection Messages 
		{
			get 
			{ 
				return _messages; 
			}
		} 
		private ValidationMessageCollection _messages
			= new ValidationMessageCollection();

		/// <summary>
		/// Serialize DVT Detail Data to Xml.
		/// </summary>
		/// <param name="streamWriter">Stream writer to serialize to.</param>
		/// <param name="level">Recursion level. 0 = Top.</param>
		/// <returns>bool - success/failure</returns>
		public bool DvtDetailToXml(StreamWriter streamWriter, int level)
		{
			if (streamWriter != null)
			{
				System.String dicomTag = System.String.Empty;
				if (_tag != Tag.UNDEFINED)
				{
					string group = _tag.GroupNumber.ToString("X").PadLeft(4,'0');
                    string element = _tag.ElementNumber.ToString("X").PadLeft(4, '0');
					dicomTag = System.String.Format("({0},{1})", group, element);
				}

				System.String hl7Tag = System.String.Empty;
				if (_segment != System.String.Empty)
				{
					hl7Tag = System.String.Format("{0}-{1}", _segment, _fieldIndex);
				}

				System.String id = System.String.Empty;

				if ((dicomTag != System.String.Empty) &&
					(hl7Tag != System.String.Empty))
				{
					id = hl7Tag + "=" + dicomTag;
				}
				else if (dicomTag != System.String.Empty)
				{
					id = dicomTag;
				}
				else if (hl7Tag != System.String.Empty)
				{
					id = hl7Tag;
				}

				streamWriter.WriteLine("<AttributeComparisonResults>");
				streamWriter.WriteLine("<AttributeComparison Id=\"{0}\" Name=\"{1}\">", id, DvtToXml.ConvertString(_name,false));
				streamWriter.WriteLine("<Value1>{0}</Value1>", _value1);
				streamWriter.WriteLine("<Value2>{0}</Value2>", _value2);
				streamWriter.WriteLine("</AttributeComparison>");
				Messages.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</AttributeComparisonResults>");
			}

			return true;
		}    

		/// <summary>
		/// Serialize DVT Summary Data to Xml.
		/// </summary>
		/// <param name="streamWriter">Stream writer to serialize to.</param>
		/// <param name="level">Recursion level. 0 = Top.</param>
		/// <returns>bool - success/failure</returns>
		public bool DvtSummaryToXml(StreamWriter streamWriter, int level)
		{
			if (
				(streamWriter != null) &&
				this.ContainsMessages()
				)
			{
				System.String dicomTag = System.String.Empty;
				if (_tag != Tag.UNDEFINED)
				{
                    string group = _tag.GroupNumber.ToString("X").PadLeft(4, '0');
                    string element = _tag.ElementNumber.ToString("X").PadLeft(4, '0');
					dicomTag = System.String.Format("({0},{1})", group, element);
				}

				System.String hl7Tag = System.String.Empty;
				if (_segment != System.String.Empty)
				{
					hl7Tag = System.String.Format("{0}-{1}", _segment, _fieldIndex);
				}

				System.String id = System.String.Empty;

				if ((dicomTag != System.String.Empty) &&
					(hl7Tag != System.String.Empty))
				{
					id = hl7Tag + "=" + dicomTag;
				}
				else if (dicomTag != System.String.Empty)
				{
					id = dicomTag;
				}
				else if (hl7Tag != System.String.Empty)
				{
					id = hl7Tag;
				}

				streamWriter.WriteLine("<AttributeComparisonResults>");
				streamWriter.WriteLine("<AttributeComparison Id=\"{0}\" Name=\"{1}\">", id, DvtToXml.ConvertString(_name,false));
				streamWriter.WriteLine("<Value1>{0}</Value1>", _value1);
				streamWriter.WriteLine("<Value2>{0}</Value2>", _value2);
				streamWriter.WriteLine("</AttributeComparison>");
				Messages.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</AttributeComparisonResults>");
			}
			return true;
		}    	

		/// <summary>
		/// Check if this contains any validation messages
		/// </summary>
		/// <returns>bool - contains validation messages true/false</returns>
		public bool ContainsMessages()
		{
			bool containsMessages = false;
			if (Messages.ErrorWarningCount() != 0)
			{
				containsMessages = true;
			}
			return containsMessages;
		}
	}

	/// <summary>
	/// Summary description for MessageComparisonResults.
	/// </summary>
	public class MessageComparisonResults : CollectionBase, IDvtDetailToXml, IDvtSummaryToXml
	{
		private System.String _objectName1 = System.String.Empty;
		private System.String _objectName2 = System.String.Empty;
		private DvtkData.Dimse.DimseCommand _dimseCommand1 = DvtkData.Dimse.DimseCommand.UNDEFINED;
		private DvtkData.Dimse.DimseCommand _dimseCommand2 = DvtkData.Dimse.DimseCommand.UNDEFINED;
		private System.String _iodName1 = System.String.Empty;
		private System.String _iodName2 = System.String.Empty;
		private System.String _sopClassUid1 = System.String.Empty;
		private System.String _sopClassUid2 = System.String.Empty;
		private System.String _messageType1 = System.String.Empty;
		private System.String _messageType2 = System.String.Empty;
		private System.String _messageSubType1 = System.String.Empty;
		private System.String _messageSubType2 = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="objectName1">Object1 name</param>
		/// <param name="objectName2">Object2 name</param>
		/// <param name="dimseCommand1">Dimse Command1</param>
		/// <param name="dimseCommand2">Dimse Command2</param>
		/// <param name="sopClassUid1">Sop Class Uid1</param>
		/// <param name="sopClassUid2">Sop Class Uid2</param>
		public MessageComparisonResults(System.String objectName1,
										System.String objectName2,
										DvtkData.Dimse.DimseCommand dimseCommand1,
										DvtkData.Dimse.DimseCommand dimseCommand2,
										System.String sopClassUid1,
										System.String sopClassUid2)
		{
			_objectName1 = objectName1;
			_objectName2 = objectName2;
			_dimseCommand1 = dimseCommand1;
			_dimseCommand2 = dimseCommand2;
			_sopClassUid1 = sopClassUid1;
			_sopClassUid2 = sopClassUid2;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="objectName1">Object1 name</param>
		/// <param name="objectName2">Object2 name</param>
		/// <param name="dimseCommand1">Dimse Command1</param>
		/// <param name="messageType2">HL7 Message Type2</param>
		/// <param name="sopClassUid1">Sop Class Uid1</param>
		/// <param name="messageSubType2">HL7 Message Subtype2</param>
		public MessageComparisonResults(System.String objectName1,
			System.String objectName2,
			DvtkData.Dimse.DimseCommand dimseCommand1,
			System.String messageType2,
			System.String sopClassUid1,
			System.String messageSubType2)
		{
			_objectName1 = objectName1;
			_objectName2 = objectName2;
			_dimseCommand1 = dimseCommand1;
			_messageType2 = messageType2;
			_sopClassUid1 = sopClassUid1;
			_messageSubType2 = messageSubType2;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="objectName1">Object1 name</param>
		/// <param name="objectName2">Object2 name</param>
		/// <param name="messageType1">HL7 Message Type1</param>
		/// <param name="dimseCommand2">Dimse Command2</param>
		/// <param name="messageSubType1">HL7 Message Subtype1</param>
		/// <param name="sopClassUid2">Sop Class Uid2</param>
		public MessageComparisonResults(System.String objectName1,
			System.String objectName2,
			System.String messageType1,
			DvtkData.Dimse.DimseCommand dimseCommand2,
			System.String messageSubType1,
			System.String sopClassUid2)
		{
			_objectName1 = objectName1;
			_objectName2 = objectName2;
			_messageType1 = messageType1;
			_dimseCommand2 = dimseCommand2;
			_messageSubType1 = messageSubType1;
			_sopClassUid2 = sopClassUid2;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="objectName1">Object1 name</param>
		/// <param name="objectName2">Object2 name</param>
		/// <param name="messageType1">HL7 Message Type1</param>
		/// <param name="messageType2">HL7 Message Type2</param>
		/// <param name="messageSubType1">HL7 Message Subtype1</param>
		/// <param name="messageSubType2">HL7 Message Subtype2</param>
		public MessageComparisonResults(System.String objectName1,
			System.String objectName2,
			System.String messageType1,
			System.String messageType2,
			System.String messageSubType1,
			System.String messageSubType2)
	{
			_objectName1 = objectName1;
			_objectName2 = objectName2;
			_messageType1 = messageType1;
			_messageType2 = messageType2;
			_messageSubType1 = messageSubType1;
			_messageSubType2 = messageSubType2;
		}

		/// <summary>
		/// Command1 property - Get only
		/// </summary>
		public DvtkData.Dimse.DimseCommand Command1
		{
			get
			{
				return _dimseCommand1;
			}
		}

		/// <summary>
		/// Command2 property - Get only
		/// </summary>
		public DvtkData.Dimse.DimseCommand Command2
		{
			get
			{
				return _dimseCommand2;
			}
		}

		/// <summary>
		/// SopClassUid1 property - Get only
		/// </summary>
		public System.String SopClassUid1
		{
			get
			{
				return _sopClassUid1;
			}
		}

		/// <summary>
		/// SopClassUid2 property - Get only
		/// </summary>
		public System.String SopClassUid2
		{
			get
			{
				return _sopClassUid2;
			}
		}

		/// <summary>
		/// IodName1 property - Set only
		/// </summary>
		public System.String IodName1
		{
			set
			{
				_iodName1 = value;
			}
		}

		/// <summary>
		/// IodName1 property - Set only
		/// </summary>
		public System.String IodName2
		{
			set
			{
				_iodName2 = value;
			}
		}

		/// <summary>
		/// Gets or sets an <see cref="AttributeComparisonResults"/> from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the collection member to get or set.</param>
		/// <value>The <see cref="MessageComparisonResults"/> at the specified index.</value>
		public AttributeComparisonResults this[int index]  
		{
			get  
			{
				return ((AttributeComparisonResults) List[index]);
			}
			set  
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Adds an object to the end of the <see cref="MessageComparisonResults"/>.
		/// </summary>
		/// <param name="value">The <see cref="AttributeComparisonResults"/> to be added to the end of the <see cref="MessageComparisonResults"/>.</param>
		/// <returns>The <see cref="MessageComparisonResults"/> index at which the value has been added.</returns>
		public int Add(AttributeComparisonResults value)  
		{
			return (List.Add(value));
		}

		/// <summary>
		/// Searches for the specified <see cref="AttributeComparisonResults"/> and 
		/// returns the zero-based index of the first occurrence within the entire <see cref="MessageComparisonResults"/>.
		/// </summary>
		/// <param name="value">The <see cref="AttributeComparisonResults"/> to locate in the <see cref="MessageComparisonResults"/>.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of value within the entire <see cref="MessageComparisonResults"/>, 
		/// if found; otherwise, -1.
		/// </returns>
		public int IndexOf(AttributeComparisonResults value)  
		{
			return (List.IndexOf(value));
		}

		/// <summary>
		/// Inserts an <see cref="AttributeComparisonResults"/> element into the <see cref="MessageComparisonResults"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="value">The <see cref="MessageComparisonResults"/> to insert.</param>
		public void Insert(int index, AttributeComparisonResults value)  
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="AttributeComparisonResults"/> from the <see cref="MessageComparisonResults"/>.
		/// </summary>
		/// <param name="value">The <see cref="AttributeComparisonResults"/> to remove from the <see cref="MessageComparisonResults"/>.</param>
		public void Remove(AttributeComparisonResults value)  
		{
			List.Remove(value);
		}

		/// <summary>
		/// Determines whether the <see cref="MessageComparisonResults"/> contains a specific element.
		/// </summary>
		/// <param name="value">The <see cref="AttributeComparisonResults"/> to locate in the <see cref="MessageComparisonResults"/>.</param>
		/// <returns>
		/// <c>true</c> if the <see cref="MessageComparisonResults"/> contains the specified value; 
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(AttributeComparisonResults value)  
		{
			// If value is not of type Code, this will return false.
			return (List.Contains(value));
		}

		/// <summary>
		/// Performs additional custom processes before inserting a new element into the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert value.</param>
		/// <param name="value">The new value of the element at index.</param>
		protected override void OnInsert(int index, Object value)  
		{
			if (!(value is AttributeComparisonResults))
				throw new ArgumentException("value must be of type AttributeComparisonResults.", "value");
		}

		/// <summary>
		/// Performs additional custom processes when removing an element from the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found.</param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemove(int index, Object value)  
		{
			if (!(value is AttributeComparisonResults))
				throw new ArgumentException("value must be of type AttributeComparisonResults.", "value");
		}

		/// <summary>
		/// Performs additional custom processes before setting a value in the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found.</param>
		/// <param name="oldValue">The value to replace with newValue.</param>
		/// <param name="newValue">The new value of the element at index.</param>
		protected override void OnSet(int index, Object oldValue, Object newValue)  
		{
			if (!(newValue is AttributeComparisonResults))
				throw new ArgumentException("newValue must be of type AttributeComparisonResults.", "newValue");
		}

		/// <summary>
		/// Performs additional custom processes when validating a value.
		/// </summary>
		/// <param name="value">The object to validate.</param>
		protected override void OnValidate(Object value)  
		{
			if (!(value is AttributeComparisonResults))
				throw new ArgumentException("value must be of type AttributeComparisonResults.");
		}
	
		/// <summary>
		/// Copies the elements of the <see cref="ICollection"/> to a strong-typed <c>AttributeComparisonResults[]</c>, 
		/// starting at a particular <c>AttributeComparisonResults[]</c> index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional <c>AttributeComparisonResults[]</c> that is the destination of the elements 
		/// copied from <see cref="ICollection"/>.
		/// The <c>AttributeComparisonResults[]</c> must have zero-based indexing. 
		/// </param>
		/// <param name="index">
		/// The zero-based index in array at which copying begins.
		/// </param>
		/// <remarks>
		/// Provides the strongly typed member for <see cref="ICollection"/>.
		/// </remarks>
		public void CopyTo(AttributeComparisonResults[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// Serialize DVT Detail Data to Xml.
		/// </summary>
		/// <param name="streamWriter">Stream writer to serialize to.</param>
		/// <param name="level">Recursion level. 0 = Top.</param>
		/// <returns>bool - success/failure</returns>
		public bool DvtDetailToXml(StreamWriter streamWriter, int level)
		{
			if (streamWriter != null)
			{
				streamWriter.WriteLine("<MessageComparisonResults>");
				streamWriter.WriteLine("<Object1>{0}</Object1>", _objectName1);
				streamWriter.WriteLine("<Object2>{0}</Object2>", _objectName2);
				if (_dimseCommand1 != DvtkData.Dimse.DimseCommand.UNDEFINED)
				{
					streamWriter.WriteLine("<Message1>{0} {1}</Message1>", _dimseCommand1.ToString(), _iodName1);
				}
				else
				{
					streamWriter.WriteLine("<Message1>{0}^{1}</Message1>", _messageType1, _messageSubType1);
				}

				if (_dimseCommand2 != DvtkData.Dimse.DimseCommand.UNDEFINED)
				{
					streamWriter.WriteLine("<Message2>{0} {1}</Message2>", _dimseCommand2.ToString(), _iodName2);
				}
				else
				{
					streamWriter.WriteLine("<Message2>{0}^{1}</Message2>", _messageType2, _messageSubType2);
				}

				foreach (AttributeComparisonResults acr in this)
				{
					acr.DvtDetailToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</MessageComparisonResults>");
			}
			return true;
		}    

		/// <summary>
		/// Serialize DVT Summary Data to Xml.
		/// </summary>
		/// <param name="streamWriter">Stream writer to serialize to.</param>
		/// <param name="level">Recursion level. 0 = Top.</param>
		/// <returns>bool - success/failure</returns>
		public bool DvtSummaryToXml(StreamWriter streamWriter, int level)
		{
			if (
				(streamWriter != null) &&
				this.ContainsMessages()
				)
			{
				streamWriter.WriteLine("<MessageComparisonResults>");
				streamWriter.WriteLine("<Object1>{0}</Object1>", _objectName1);
				streamWriter.WriteLine("<Object2>{0}</Object2>", _objectName2);
				streamWriter.WriteLine("<Object2>{0}</Object2>", _objectName2);
				if (_dimseCommand1 != DvtkData.Dimse.DimseCommand.UNDEFINED)
				{
					streamWriter.WriteLine("<Message1>{0} {1}</Message1>", _dimseCommand1.ToString(), _iodName1);
				}
				else
				{
					streamWriter.WriteLine("<Message1>{0}^{1}</Message1>", _messageType1, _messageSubType1);
				}

				if (_dimseCommand2 != DvtkData.Dimse.DimseCommand.UNDEFINED)
				{
					streamWriter.WriteLine("<Message2>{0} {1}</Message2>", _dimseCommand2.ToString(), _iodName2);
				}
				else
				{
					streamWriter.WriteLine("<Message2>{0}^{1}</Message2>", _messageType2, _messageSubType2);
				}

				foreach (AttributeComparisonResults acr in this)
				{
					acr.DvtSummaryToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</MessageComparisonResults>");
			}
			return true;
		}    	

		/// <summary>
		/// Check if this contains any validation messages
		/// </summary>
		/// <returns>bool - contains validation messages true/false</returns>
		private bool ContainsMessages()
		{
			bool containsMessages = false;

			foreach (AttributeComparisonResults acr in this)
			{
				if (acr.ContainsMessages() == true)
				{
					containsMessages = true;
					break;
				}
			}
			return containsMessages;
		}
	}
}
