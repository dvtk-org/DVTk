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

namespace Dvtk.Hl7.Messages
{
	/// <summary>
	/// Class containing the common functionality of the Hl7FileStream and Hl7NetworkStream classes.
	/// </summary>
	abstract public class Hl7Stream
	{
		//
		// - Enumerates -
		//
		private enum MllpStateEnum
		{
			MllpNotUsed,
			WaitingForStartOfMessageChar,
			WaitingForEndOfSegmentChar,
			WaitingForEndOfMessageChar
		}

		//
		// - Constant fields -
		//
		private const char _EndOfMessageChar1 = (char)0x1C;
		private const char _EndOfMessageChar2 = (char)0x0D;
		private const char _EndOfSegmentChar = (char)0x0D;
        private const char _NewLineChar = (char)0x0A;
		private const char _StartOfMessageChar = (char)0x0B;

		//
		// - Fields -
		//
		private bool _continueReading = true;
		private Hl7MessageDelimiters _messageDelimiters = new Hl7MessageDelimiters();

		//
		// - Properties -
		//

		/// <summary>
		/// When the Decode method was called after calling the set, this contains the message delimiters found during decoding.
		/// 
		/// The message delimiters will be used during the encoding of a HL7 message.
		/// </summary>
		public Hl7MessageDelimiters MessageDelimiters
		{
			get
			{
				return _messageDelimiters;
			}
			set
			{
				_messageDelimiters = value;
			}
		}	

		//
		// - Methods -
		//

		/// <summary>
		/// Decode a Hl7Message object from a given stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>The decoded Hl7Message.</returns>
		internal Hl7Message Decode(System.IO.Stream stream)
		{
			Hl7Message hl7Message = new Hl7Message();

			// initialize the message delimiters to the default values
			_messageDelimiters = new Hl7MessageDelimiters();

			// initialise the segment content
			System.String rxSegment = System.String.Empty;

			// read the first character from the stream to determine if the MLLP is used
			int rxCode = stream.ReadByte();
			if (rxCode < 0)
			{
				throw new System.Exception("Incomplete HL7 stream.");
			}
			stream.Seek(0, System.IO.SeekOrigin.Begin);

			// initialise the MLLP state
			MllpStateEnum mllpState = MllpStateEnum.MllpNotUsed;
			if ((char)rxCode == _StartOfMessageChar)
			{
				// SOM read - use the MLLP protocol
				mllpState = MllpStateEnum.WaitingForStartOfMessageChar;
			}

			// loop waiting for the end of message character
			while (_continueReading)
			{
				// get the next character from the stream
				rxCode = stream.ReadByte();
				if (rxCode < 0)
				{
					// end of stream reached when not using the MLLP
					// - check if there is any data left in the rxSegment
					if ((rxSegment != System.String.Empty) && 
						(mllpState == MllpStateEnum.MllpNotUsed))
					{
						// segment is complete
						Hl7Segment segment = new Hl7Segment();
						segment.Decode(rxSegment, _messageDelimiters);

						// add the segment to the HL7 message
						hl7Message.AddSegment(segment);
					}

					// message is complete
					_continueReading = false;
                    break;
				}

				char rxChar = (char) rxCode;

				// switch on MLLP state
				switch(mllpState)
				{
					case MllpStateEnum.MllpNotUsed:
						// check if we have got the end of segment
						if (rxChar == _EndOfSegmentChar)
						{
							// check if we have the MSH segment 
							// - we need to get the message delimiters
							if (rxSegment.StartsWith("MSH") == true)
							{
								// set the message delimiters to the values received
								// - we assume that the MSH segment is formatted properly at least in the first few bytes
								_messageDelimiters = new Hl7MessageDelimiters(rxSegment.Substring(3,5));
							}

							// segment is complete
							Hl7Segment segment = new Hl7Segment();
							segment.Decode(rxSegment, _messageDelimiters);

							// add the segment to the HL7 message
							hl7Message.AddSegment(segment);

							// reset the segment
							rxSegment = System.String.Empty;
						}
                        else if (rxChar == _NewLineChar)
                        {
                            // ignore the line feed character
                        }
                        else
                        {
                            // save the received character in the current segment
                            rxSegment += rxChar;
                        }
						break;
					case MllpStateEnum.WaitingForStartOfMessageChar:
						// check if we have got the SOM
						if (rxChar == _StartOfMessageChar)
						{
							// reset the segment
							rxSegment = System.String.Empty;

							// change state to waiting for end of segment
							mllpState = MllpStateEnum.WaitingForEndOfSegmentChar;
						}
						else
						{
							Console.WriteLine("HL7 - MLLP: Waiting for SOM - got {0}...", rxChar);
						}
						break;
					case MllpStateEnum.WaitingForEndOfSegmentChar:
						// check if we have got the end of segment
						if (rxChar == _EndOfSegmentChar)
						{
							// check if we have the MSH segment 
							// - we need to get the message delimiters
							if (rxSegment.StartsWith("MSH") == true)
							{
								// set the message delimiters to the values received
								// - we assume that the MSH segment is formatted properly at least in the first few bytes
								_messageDelimiters = new Hl7MessageDelimiters(rxSegment.Substring(3,5));
							}

							// segment is complete
							Hl7Segment segment = new Hl7Segment();
							segment.Decode(rxSegment, _messageDelimiters);

							// add the segment to the HL7 message
							hl7Message.AddSegment(segment);

							// reset the segment
							rxSegment = System.String.Empty;
						}
						else if (rxChar == _EndOfMessageChar1)
						{
							// we have the first end of message - that's OK
							// check if any characters have been received since the last end of segment
							if (rxSegment == System.String.Empty)
							{
								// change state to waiting for second end of message
								mllpState = MllpStateEnum.WaitingForEndOfMessageChar;
							}
							else
							{
								Console.WriteLine("HL7 - MLLP: First EOM does not immediately follow an EOS");
								return null;
							}
						}
						else
						{
							// save the received character in the current segment
							rxSegment += rxChar;
						}
						break;
					case MllpStateEnum.WaitingForEndOfMessageChar:
						// check if we have got the second end of message
						if (rxChar == _EndOfMessageChar2)
						{
							// message is complete
							_continueReading = false;
						}
						else
						{
							Console.WriteLine("HL7 - MLLP: Second EOM does not immediately follow first EOM");
							return null;
						}
						break;
					default:
						break;
				}
			}

			// return the correct instantiation of the received HL7 message
			return Hl7MessageFactory.CreateHl7Message(hl7Message, _messageDelimiters);
		}
	}
}
