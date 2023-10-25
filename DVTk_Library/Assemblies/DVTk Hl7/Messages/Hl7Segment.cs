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
using Dvtk.Hl7;

namespace Dvtk.Hl7.Messages
{
	/// <summary>
	/// Summary description for Hl7Segment.
	/// </summary>
	public class Hl7Segment
	{
		private int _sequenceNumber = 0;
		private Hl7SegmentId _segmentId = null;
		private ArrayList _values = null;

		/// <summary>
		/// Class constructor
		/// </summary>
		public Hl7Segment()
		{
			_sequenceNumber = 0;
			_segmentId = new Hl7SegmentId(Hl7SegmentEnum.Unknown);
			_values = new ArrayList();			
		}

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="sequenceNumber">Segment Sequence Number in HL7 message.</param>
		/// <param name="name">Enumerated Segment Name</param>
		public Hl7Segment(int sequenceNumber, Hl7SegmentEnum name)
		{
			_sequenceNumber = sequenceNumber;
			_segmentId = new Hl7SegmentId(name);
			_values = new ArrayList();
			System.String val = SegmentNames.Name(name);
			_values.Insert(0, val);
			
		}

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="sequenceNumber">Segment Sequence Number in HL7 message.</param>
		/// <param name="setId">Set ID</param>
		public Hl7Segment(int sequenceNumber, System.String setId) : this(sequenceNumber, SegmentNames.NameEnum(setId)) {}

		/// <summary>
		/// Property - Count - Number of fields defined in segment
		/// </summary>
		public int Count
		{
			get
			{
				return _values.Count;
			}
		}

		/// <summary>
		/// Property - Segment Sequence Number
		/// </summary>
		public int SequenceNumber
		{
			get
			{
				return _sequenceNumber;
			}
			set
			{
				_sequenceNumber = value;
			}
		}

		/// <summary>
		/// Property - Segment Id
		/// </summary>
		public Hl7SegmentId SegmentId
		{
			get
			{
				return _segmentId;
			}
		}

		/// <summary>
		/// Property - Segment Value Array
		/// </summary>
		public String this[int index]
		{
			get
			{
				return (String)_values[index];
			}
			set
			{
				System.String val = value;
				if (_values.Count < index)
				{
					for (int i = _values.Count; i < index; i++)
					{
						System.String emptyValue = System.String.Empty;
						_values.Insert(i, emptyValue);
					}
				}
				else if (_values.Count > index)
				{
					_values.RemoveAt(index);
				}
				_values.Insert(index, val);
			}
		}

		/// <summary>
		/// Encode the segment.
		/// </summary>
		/// <param name="messageDelimiters">HL7 message delimiters to use to encode the message.</param>
		/// <returns>String - encoded segment.</returns>
		public System.String Encode(Hl7MessageDelimiters messageDelimiters)
		{
			System.String segment = System.String.Empty;

			if (_values.Count > 1)
			{
				if (_segmentId.SegmentName == Hl7SegmentEnum.MSH)
				{
					// initialize the first fields of the MSH segment
					segment = (System.String)_values[0] + messageDelimiters.ToString() + messageDelimiters.FieldDelimiter;

					// ecode the rest of the MSH segment from the 3rd field
					for (int index = 3; index < _values.Count; index++)
					{
						segment += ReplaceString((System.String)_values[index], messageDelimiters);;					
						if (index + 1 != _values.Count)
						{
							segment += messageDelimiters.FieldDelimiter;
						}
					}
				}
				else
				{
					int index = 0;
					foreach (System.String val in _values)
					{
						segment += ReplaceString(val, messageDelimiters);					
						index++;
						if (index != _values.Count)
						{
							segment += messageDelimiters.FieldDelimiter;
						}
					}
				}
			}

			return segment;
		}

		private System.String ReplaceString(System.String inputString, Hl7MessageDelimiters messageDelimiters)
		{
			// get the default message delimiters - these will have been used in the HL7 scripts, etc
			Hl7MessageDelimiters defaultMessageDelimiters = new Hl7MessageDelimiters();

			// update the string to use the given message delimiters
			System.String localString1 = inputString.Replace(defaultMessageDelimiters.ComponentDelimiter, messageDelimiters.ComponentDelimiter);
			System.String localString2 = localString1.Replace(defaultMessageDelimiters.SubComponentDelimiter, messageDelimiters.SubComponentDelimiter);
			System.String outputString = localString2.Replace(defaultMessageDelimiters.RepetitionSeparator, messageDelimiters.RepetitionSeparator);

			return outputString;
		}

		/// <summary>
		/// Decode the segment. The HL7 message delimiters has been set to the correct value before calling this method.
		/// </summary>
		/// <param name="segmentString">Encoded segment string.</param>
		/// <param name="messageDelimiters">HL7 message delimiters to use to encode the message.</param>
		public void Decode(System.String segmentString, Hl7MessageDelimiters messageDelimiters)
		{
			int index = 0;
			System.String val = System.String.Empty;
			int i = 0;
			bool isVal = false;
			while (i < segmentString.Length)
			{
				if (segmentString.Substring(i,1) == messageDelimiters.FieldDelimiter)
				{
					_values.Insert(index, val);
					index++;
				
					if ((index == 1) &&
						((System.String)_values[0] == "MSH"))
					{
						// special insert for the field delimiter itself.
						_values.Insert(index, messageDelimiters.FieldDelimiter);
						index++;
					}

					val = System.String.Empty;
					isVal = false;
				}
				else
				{
					val += segmentString[i];
					isVal = true;
				}
				i++;
			}

			if (isVal == true)
			{
				_values.Insert(index, val);
			}

			if (_values.Count > 0)
			{
				// use segment index 0 here - the correct value will be determined when the segment is added to the message
				_segmentId = new Hl7SegmentId(SegmentNames.NameEnum((System.String)_values[0]), 0);
			}
		}
	}
}
