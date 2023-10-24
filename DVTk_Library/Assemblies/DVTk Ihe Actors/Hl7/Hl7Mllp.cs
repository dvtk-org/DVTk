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
using System.Net;
using System.Net.Sockets;

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;

namespace Dvtk.IheActors.Hl7
{
	/// <summary>
	/// Summary description for Hl7Mllp.
	/// </summary>
	public class Hl7Mllp
	{
		private enum MllpStateEnum
		{
			WaitingForStartOfMessageChar,
			WaitingForEndOfSegmentChar,
			WaitingForEndOfMessageChar,
			DoneWaiting
		}

		// TODO - make properties to set these values...
		private char _StartOfMessageChar = (char)0x0B;
		private char _EndOfSegmentChar = (char)0x0D;
		private char _EndOfMessageChar1 = (char)0x1C;
		private char _EndOfMessageChar2 = (char)0x0D;

		private TcpClient _tcpClient = null;
		private TcpListener _tcpListener = null;
		private NetworkStream _networkStream = null;
        private int _readTimeout = 0;
        private int _writeTimeout = 0;
        private bool _normalShutdown = false;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public Hl7Mllp() {}

		/// <summary>
		/// Make a network connection to a server with the given hostname and port.
		/// </summary>
		/// <param name="hostname">Server hostname.</param>
		/// <param name="port">Connection port - Server listen port.</param>
		/// <returns>bool - true if connection is made.</returns>
		public bool Connect(System.String hostname, int port)
		{
            bool connected = false;
            _networkStream = null;
            _normalShutdown = false;

            try
            {
                _tcpClient = new TcpClient(hostname, port);
                _networkStream = _tcpClient.GetStream();
                connected = true;
            }
            catch (System.Exception e)
            {
                // failed to connect
                if (_normalShutdown == false)
                {
                    Console.WriteLine("HL7 - MLLP: Connect exception: {0}", e.Message);
                }
            }

            return connected;
        }

		/// <summary>
		/// Listen for a connection to the given network port.
		/// </summary>
		/// <param name="port">Port number to listen on for connections.</param>
		/// <returns>bool - true if socket listening.</returns>
		public bool Listen(int port)
		{
            bool listening = false;
            _tcpListener = null;
            _normalShutdown = false;

            try
            {
                _tcpListener = new TcpListener(port);
                _tcpListener.Start();
                listening = true;
            }
            catch (System.Exception e)
            {
                // failed to listen
                if (_normalShutdown == false)
                {
                    Console.WriteLine("HL7 - MLLP: Listen exception: {0}", e.Message);
                }
            }

            return listening;
        }

		/// <summary>
		/// Accept a client connection to the given network port.
		/// </summary>
		/// <returns>Socket - connected socket.</returns>
		public bool Accept()
		{
            bool connected = false;
            _networkStream = null;
            _normalShutdown = false;

            try
            {
                // Listen for a connection
                Socket connectedSocket = _tcpListener.AcceptSocket();
                if (connectedSocket.Connected == true)
                {
                    _networkStream = new NetworkStream(connectedSocket);
                    connected = true;
                }
            }
            catch (System.Exception e)
            {
                // failed to accept
                if (_normalShutdown == false)
                {
                    Console.WriteLine("HL7 - MLLP: Accept exception: {0}", e.Message);
                }
            }

            return connected;
        }

		/// <summary>
		/// Close the network connection.
		/// </summary>
		public void Close()
		{
            _normalShutdown = true;

			if (_networkStream != null)
			{
				_networkStream.Close();
				_networkStream = null;
			}
		}

		/// <summary>
		/// Stop the network connection.
		/// </summary>
		public void Stop()
		{
            _normalShutdown = true;

			if (_networkStream != null)
			{
				_networkStream.Close();
			}

			if (_tcpClient != null)
			{
				_tcpClient.Close();
				_tcpClient = null;
			}

			if (_tcpListener != null)
			{
				_tcpListener.Stop();
				_tcpListener = null;
			}
		}

		/// <summary>
		/// Send an HL7 message over the connected network stream.
		/// </summary>
		/// <param name="hl7Message">HL7 message to encode.</param>
		/// <param name="messageDelimiters">HL7 message delimiters.</param>
		/// <returns>bool - successful sent = true else false.</returns>
		public bool SendMessage(Hl7Message hl7Message, Hl7MessageDelimiters messageDelimiters)
		{
			if (_networkStream == null) return false;

            // set the read / write timeouts for this stream - zero means no timeout.
            if (_readTimeout != 0)
            {
                _networkStream.ReadTimeout = _readTimeout;
            }
            if (_writeTimeout != 0)
            {
                _networkStream.WriteTimeout = _writeTimeout;
            }

            // write the start of message character
			_networkStream.WriteByte((byte)_StartOfMessageChar);

 			// stream all the segments - ordered by sequence number and then segment index
			int sequenceNumber = 0;
			int segmentIndex = 1;
			bool streaming = true;
			while (streaming == true)
			{
				bool segmentStreamed = false;
				ICollection segments = hl7Message.Segments.Values;
				foreach (Hl7Segment hl7Segment in segments)
				{
					if (hl7Segment.SequenceNumber == sequenceNumber)
					{
						if (hl7Segment.SegmentId.SegmentIndex == segmentIndex)
						{
							System.String encodedStream = hl7Segment.Encode(messageDelimiters);
							if (encodedStream != System.String.Empty)
							{
								byte[] buffer = new byte[encodedStream.Length];
								for (int j = 0; j < encodedStream.Length; j++)
								{
									buffer[j] = (byte) encodedStream[j];
								}

								// write the segment
								_networkStream.Write(buffer, 0, encodedStream.Length);

								// write the end of segment character
								_networkStream.WriteByte((byte)_EndOfSegmentChar);
							}
							
							segmentIndex++;
							segmentStreamed = true;
							break;
						}
					}
				}

				if (segmentStreamed == false)
				{
					if (sequenceNumber < hl7Message.Segments.Count)
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

			// write the end of message characters
			_networkStream.WriteByte((byte)_EndOfMessageChar1);
			_networkStream.WriteByte((byte)_EndOfMessageChar2);
			_networkStream.Flush();

			return true;
		}

		/// <summary>
		/// Receive an HL7 message over the connected network stream.
		/// </summary>
		/// <param name="messageDelimiters">Initial HL7 message delimiters - updated to actual delimters during method.</param>
		/// <returns>Correctly instantiated HL7 message.</returns>
		public Hl7Message ReceiveMessage(out Hl7MessageDelimiters messageDelimiters)
		{
			// initialize the message delimiters to the default values
			messageDelimiters = new Hl7MessageDelimiters();

			Hl7Message hl7Message = new Hl7Message();
			if (_networkStream == null) return null;

            // set the read / write timeouts for this stream - zero means no timeout.
            if (_readTimeout != 0)
            {
                _networkStream.ReadTimeout = _readTimeout;
            }
            if (_writeTimeout != 0)
            {
                _networkStream.WriteTimeout = _writeTimeout;
            }

            // initialise the segment content
			System.String rxSegment = System.String.Empty;

			// initialise the MLLP state
			MllpStateEnum mllpState = MllpStateEnum.WaitingForStartOfMessageChar;

			// loop waiting for the end of message character
			while (mllpState != MllpStateEnum.DoneWaiting)
			{
				// check if data is available on network
                try
				{
					// get the next character from the stream
					int rxCode = _networkStream.ReadByte();
					if (rxCode < 0)
					{
						return null;
					}

					char rxChar = (char) rxCode;

					// switch on MLLP state
					switch(mllpState)
					{
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
									messageDelimiters = new Hl7MessageDelimiters(rxSegment.Substring(3,5));
								}

								// segment is complete
								Hl7Segment segment = new Hl7Segment();
								segment.Decode(rxSegment, messageDelimiters);

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
								mllpState = MllpStateEnum.DoneWaiting;
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
                catch (System.Exception e)
                {
                    Console.WriteLine("HL7 - MLLP: ReceiveMessage() Exception: {0}", e.Message);
                    return null;
                }
            }

			// return the correct instantiation of the received HL7 message
			return Hl7MessageFactory.CreateHl7Message(hl7Message, messageDelimiters);
		}

        /// <summary>
        /// Property - ReadTimeout.
        /// </summary>
        public int ReadTimeout
        {
            set
            {
                _readTimeout = value;
            }
        }

        /// <summary>
        /// Property - WriteTimeout.
        /// </summary>
        public int WriteTimeout
        {
            set
            {
                _writeTimeout = value;
            }
        }
	}
}
