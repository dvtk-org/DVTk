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
using System.Text;
using Dvtk.Hl7;

namespace Dvtk.Hl7.Messages
{
	/// <summary>
	/// Summary description for Hl7Message.
	/// </summary>
	public class Hl7Message
	{
		private System.String _messageType = System.String.Empty;
		private System.String _messageSubType = System.String.Empty;
		private System.Collections.Hashtable _segments = null;
 
		private const char _EndOfSegmentChar = (char)0x0D;
        private const char _NewLineChar = (char)0x0A;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public Hl7Message()
		{
			// constructor activities
			_segments = new Hashtable();
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="messageType">Message Type</param>
		/// <param name="messageSubType">Message Sub Type</param>
		public Hl7Message(System.String messageType, System.String messageSubType)
		{
			// constructor activities
			_messageType = messageType;
			_messageSubType = messageSubType;
			_segments = new Hashtable();
			Hl7Segment segment = new Hl7Segment(0, Hl7SegmentEnum.MSH);
			_segments.Add(segment.SegmentId.Id, segment);

			// Set up MSH segment
			// - use default message delimiters for now
			Hl7MessageDelimiters messageDelimiters = new Hl7MessageDelimiters();
			FieldDelimiter = messageDelimiters.FieldDelimiter;
			EncodingCharacters = messageDelimiters.ComponentDelimiter + messageDelimiters.RepetitionSeparator + messageDelimiters.EscapeCharacter + messageDelimiters.SubComponentDelimiter;
			SendingApplication = "DVTK-IHE";
			SendingFacility = "DVTK";
			ReceivingApplication = "DVTK-IHE";
			ReceivingFacility = "DVTK";
			ProcessingId = "P"; 
			VersionId = "2.3.1";

			if (messageSubType == System.String.Empty)
			{
				this.MSH[9] = messageType;
			}
			else
			{
				this.MSH[9] = messageType + messageDelimiters.ComponentDelimiter + messageSubType;
			}
		}

        /// <summary>
		/// Generate string representation to HL7 Message - include the end of segment character.
		/// </summary>
		/// <param name="messageDelimiters">HL7 message delimiters.</param>
		/// <returns>string - HL7 ER7 format string.</returns>
        public System.String ToString(Hl7MessageDelimiters messageDelimiters)
        {
            return ToString(messageDelimiters, false);
        }

        /// <summary>
        /// Generate string representation to HL7 Message for display - include the end of 
        /// segment character and new line.
        /// </summary>
        /// <returns>string - HL7 ER7 format string - for display.</returns>
        public System.String DisplayAsString()
        {
            return ToString(new Hl7MessageDelimiters(), true);
        }

        /// <summary>
		/// Generate string representation to HL7 Message - include the end of segment character.
		/// </summary>
		/// <param name="messageDelimiters">HL7 message delimiters.</param>
        /// <param name="addNewLine">Add new line boolean - for display purposes.</param>
		/// <returns>string - HL7 ER7 format string.</returns>
		private System.String ToString(Hl7MessageDelimiters messageDelimiters, bool addNewLine)
		{
			System.String stringValue = System.String.Empty;

			// stream all the segments - ordered by sequence number and then segment index
			int sequenceNumber = 0;
			int segmentIndex = 1;
			bool streaming = true;
			while (streaming == true)
			{
				bool segmentStreamed = false;
				ICollection segments = _segments.Values;
				foreach (Hl7Segment hl7Segment in segments)
				{
					if (hl7Segment.SequenceNumber == sequenceNumber)
					{
						if (hl7Segment.SegmentId.SegmentIndex == segmentIndex)
						{
							System.String encodedStream = hl7Segment.Encode(messageDelimiters);
							if (encodedStream != System.String.Empty)
							{
								stringValue += (encodedStream + _EndOfSegmentChar);

                                if (addNewLine == true)
                                {
                                    // for display purposes only
                                    stringValue += _NewLineChar;
                                }
                            }

							segmentIndex++;
							segmentStreamed = true;
							break;
						}
					}
				}

				if (segmentStreamed == false)
				{
					if (sequenceNumber < _segments.Count)
					{
						sequenceNumber++;
						segmentIndex = 1;
					}
					else
					{
						streaming = false;
					}
				}
			}

			return stringValue;
		}

		/// <summary>
		/// Property - Field Delimiter
		/// </summary>
		private System.String FieldDelimiter
		{
			set
			{
				this.MSH[1] = value;
			}
		}

		/// <summary>
		/// Property - Encoding Characters
		/// </summary>
		private System.String EncodingCharacters
		{
			set
			{
				this.MSH[2] = value;
			}
		}

		/// <summary>
		/// Property - Sending Application
		/// </summary>
		public System.String SendingApplication
		{
			set
			{
				this.MSH[3] = value;
			}
			get
			{
				return this.MSH[3];
			}
		}

		/// <summary>
		/// Property - Sending Facility
		/// </summary>
		public System.String SendingFacility
		{
			set
			{
				this.MSH[4] = value;
			}
			get
			{
				return this.MSH[4];
			}
		}

		/// <summary>
		/// Property - Receiving Application
		/// </summary>
		public System.String ReceivingApplication
		{
			set
			{
				this.MSH[5] = value;
			}
			get
			{
				return this.MSH[5];
			}
		}

		/// <summary>
		/// Property - Receiving Facility
		/// </summary>
		public System.String ReceivingFacility
		{
			set
			{
				this.MSH[6] = value;
			}
			get
			{
				return this.MSH[6];
			}
		}

		/// <summary>
		/// Property - Date/Time Of Message
		/// </summary>
		public System.String DateTimeOfMessage
		{
			set
			{
				this.MSH[7] = value;
			}
			get
			{
				return this.MSH[7];
			}
		}

		/// <summary>
		/// Property - Message Control Id
		/// </summary>
		public System.String MessageControlId
		{
			set
			{
				this.MSH[10] = value;
			}
			get
			{
				return this.MSH[10];
			}
		}

		/// <summary>
		/// Property - Processing Id
		/// </summary>
		public System.String ProcessingId
		{
			set
			{
				this.MSH[11] = value;
			}
			get
			{
				return this.MSH[11];
			}
		}

		/// <summary>
		/// Property - Version Id
		/// </summary>
		public System.String VersionId
		{
			set
			{
				this.MSH[12] = value;
			}
			get
			{
				return this.MSH[12];
			}
		}

		/// <summary>
		/// Property - MSH Segment
		/// </summary>
		public Hl7Segment MSH
		{
			get
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.MSH);
				return (Hl7Segment)_segments[segmentId.Id];
			}
		}

		/// <summary>
		/// Property - Segments
		/// </summary>
		public System.Collections.Hashtable Segments
		{
			get
			{
				return _segments;
			}
		}

		/// <summary>
		/// Property - Message Type
		/// </summary>
		public System.String MessageType
		{
			get
			{
				return _messageType;
			}
		}

		/// <summary>
		/// Property - Message Sub Type.
		/// </summary>
		public System.String MessageSubType
		{
			get
			{
				return _messageSubType;
			}
		}

		/// <summary>
		/// Add segment to HL7 message
		/// </summary>
		/// <param name="segment"></param>
		public void AddSegment(Hl7Segment segment)
		{
			ICollection segments = _segments.Values;
			int nextSequenceNumber = segments.Count;

			// check if the segment is already present
			if (_segments.Contains(segment.SegmentId.Id) == true)
			{
				// remove the segment
				Hl7Segment tempSegment = (Hl7Segment)_segments[segment.SegmentId.Id];
				_segments.Remove(segment.SegmentId.Id);

				// use the current sequence number
				segment.SequenceNumber = tempSegment.SequenceNumber;
			}
			else
			{
				// check to see if a segment already exists of the same type
				int existingSequenceNumber = 0;
				int nextSegmentIndex = 0;
				if (GetNextSegmentIndex(segment.SegmentId.SegmentName, out existingSequenceNumber, out nextSegmentIndex) == true)
				{
					// segment exists - this new one must get the existing sequence number and next segment index
					segment.SequenceNumber = existingSequenceNumber;
					segment.SegmentId.SegmentIndex = nextSegmentIndex;
				}
				else
				{
					// segment does not exist - simply give it the next sequence number and start the segment index at 1.
					segment.SequenceNumber = nextSequenceNumber;
					segment.SegmentId.SegmentIndex = 1;
				}
			}

			// add the new segment
			_segments.Add(segment.SegmentId.Id, segment);
		}

		/// <summary>
		/// Copy segment to HL7 message
		/// </summary>
		/// <param name="segment"></param>
        public void CopySegment(Hl7Segment segment)
        {
            // copy the segment by simply adding it to the message
            _segments.Add(segment.SegmentId.Id, segment);
        }

		/// <summary>
		/// Get the value at the indexed segment/field.
		/// </summary>
		/// <param name="name">Segment Name.</param>
		/// <param name="segmentIndex">One-based segment index.</param>
		/// <param name="fieldIndex">Zero-based field index.</param>
		/// <returns>String - value at the indexed segment/field.</returns>
		public System.String Value(Hl7SegmentEnum name, int segmentIndex, int fieldIndex)
		{
			Hl7Tag hl7Tag = new Hl7Tag(name, segmentIndex, fieldIndex);
			return Value(hl7Tag);
		}

		/// <summary>
		/// Get the value at the indexed field.
		/// </summary>
		/// <param name="name">Segment Name.</param>
		/// <param name="fieldIndex">Zero-based field index.</param>
		/// <returns>String - value at the indexed field.</returns>
		public System.String Value(Hl7SegmentEnum name, int fieldIndex)
		{
			Hl7Tag hl7Tag = new Hl7Tag(name, fieldIndex);
			return Value(hl7Tag);
		}

		/// <summary>
		/// Get the value at the segment/field identified by the tag.
		/// </summary>
		/// <param name="tag">Hl7 Tag.</param>
		/// <returns>String - value at the segment/field identified by the tag.</returns>
		public System.String Value(Hl7Tag tag)
		{
			System.String val = System.String.Empty;

			Hl7Segment hl7Segment = (Hl7Segment)_segments[tag.SegmentId.Id];
			if (hl7Segment != null)
			{
				if (tag.FieldIndex < hl7Segment.Count)
				{
					val = (System.String)hl7Segment[tag.FieldIndex];
				}
			}

			return val;
		}

		/// <summary>
		/// Set the value at the indexed segment.
		/// </summary>
		/// <param name="name">Segment Name.</param>
		/// <param name="fieldIndex">Field Index.</param>
		/// <param name="val">Field Value.</param>
		public void AddValue(Hl7SegmentEnum name, int fieldIndex, System.String val)
		{
			Hl7SegmentId segmentId = new Hl7SegmentId(name);
			Hl7Segment hl7Segment = (Hl7Segment)_segments[segmentId.Id];
			if (hl7Segment != null)
			{
				hl7Segment[fieldIndex] = val;
			}
		}

		/// <summary>
		/// Search for an existing segment in the collection.
		/// Get the segment sequence number.
		/// Find the maximum segment index available for that segment so far.
		/// The next segment index will be one greater.
		/// </summary>
		/// <param name="name">Segment Name.</param>
		/// <param name="sequenceNumber">Segment Sequence Number.</param>
		/// <param name="nextSegmentIndex">nextSegmentIndex - 0 if no matching segment found.</param>
		/// <returns>bool - true if a matching segment is found.</returns>
		private bool GetNextSegmentIndex(Hl7SegmentEnum name, out int sequenceNumber, out int nextSegmentIndex)
		{
			sequenceNumber = 0;
			nextSegmentIndex = 0;
			bool matchingSegment = false;

			// interate over all segments
			ICollection segments = _segments.Values;
			foreach (Hl7Segment segment in segments)
			{
				if (segment.SegmentId.SegmentName == name)
				{
					if (segment.SegmentId.SegmentIndex > nextSegmentIndex)
					{
						sequenceNumber = segment.SequenceNumber;
						nextSegmentIndex = segment.SegmentId.SegmentIndex;
						matchingSegment = true;
					}
				}
			}

			// check if we have found a matching segment
			if (matchingSegment == true)
			{
				// the next index is one greater
				nextSegmentIndex++;
			}

			return matchingSegment;
		}
	}
}
