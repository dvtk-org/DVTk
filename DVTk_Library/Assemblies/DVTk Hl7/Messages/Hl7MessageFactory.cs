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
	/// Summary description for Hl7MessageFactory.
	/// </summary>
	public class Hl7MessageFactory
	{
		/// <summary>
		/// Create a correctly typed HL7 message instance based on the incoming Hl7 message.
		/// </summary>
		/// <param name="inHl7Message">Incoming HL7 message.</param>
		/// <param name="messageDelimiters">HL7 message delimiters.</param>
		/// <returns>Correctly typed HL7 message instance.</returns>
		public static Hl7Message CreateHl7Message(Hl7Message inHl7Message, Hl7MessageDelimiters messageDelimiters)
		{
			Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.MSH);
			Hl7Segment mshSegment = (Hl7Segment)inHl7Message.Segments[segmentId.Id];

			// can not determine what kind of message we have - so return the inHl7Message
			if (mshSegment == null)
			{
				return inHl7Message;
			}
			System.String messageType = mshSegment[9];

			Hl7Message hl7Message = null;

			// check for ACK message
			if (messageType == "ACK")
			{
				// now try to get the ORC segment
				segmentId = new Hl7SegmentId(Hl7SegmentEnum.ORC);
				Hl7Segment orcSegment = (Hl7Segment)inHl7Message.Segments[segmentId.Id];
				if (orcSegment != null)
				{
					hl7Message = new OrrMessage();
				}
				else
				{
					hl7Message = new AckMessage();
				}
			}
			else
			{
				System.String []messageTypeComponent = new System.String[3];
				messageTypeComponent = messageType.Split(messageDelimiters.ComponentDelimiter[0]);
				System.String messageMainType = System.String.Empty;
				if (messageTypeComponent.Length > 0)
				{
					messageMainType = messageTypeComponent[0];
				}

				switch (messageMainType)
				{
					case "ADR" :
						// ADR message
						hl7Message = new AdrMessage();
						break;
					case "ADT" :
						// ADT message
						hl7Message = new AdtMessage();
						break;
					case "ORM" :
						// ORM message
						hl7Message = new OrmMessage();
						break;
					case "ORU" :
						// ORU message
						hl7Message = new OruMessage();
						break;
					case "QRY" :
						// QRY message
						hl7Message = new QryMessage();
						break;
					default:
						// do not know what kind of HL7 message this is - simply return it
						return inHl7Message;
				}
			}

			// add the segments from the inMessage to the new hl7Message
			ICollection segments = inHl7Message.Segments.Values;
			foreach (Hl7Segment segment in segments)
			{
				hl7Message.CopySegment(segment);
			}

			return hl7Message;
		}
	}
}
