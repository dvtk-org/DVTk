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
using Dvtk.Hl7;

namespace Dvtk.Hl7.Messages
{
	/// <summary>
	/// ACK Message class
	/// </summary>
	public class AckMessage : Hl7Message
	{
        /// <summary>
		/// Class constructor - ACK
		/// </summary>
        public AckMessage() : base() { }

		/// <summary>
		/// Class constructor - ACK
		/// </summary>
		/// <param name="messageSubType">ACK Message Sub Type.</param>
		public AckMessage(System.String messageSubType) : base("ACK", messageSubType)
		{
			Hl7Segment segment = new Hl7Segment(1, Hl7SegmentEnum.MSA);
			Segments.Add(segment.SegmentId.Id, segment);
			segment = new Hl7Segment(2, Hl7SegmentEnum.ERR);
			Segments.Add(segment.SegmentId.Id, segment);
		}

        #region Segment Properties
        /// <summary>
		/// Property - MSA Segment
		/// </summary>
		public Hl7Segment MSA
		{
			get
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.MSA);
				return (Hl7Segment)Segments[segmentId.Id];
			}
		}

		/// <summary>
		/// Property - ERR Segment
		/// </summary>
		public Hl7Segment ERR
		{
			get
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.ERR);
				return (Hl7Segment)Segments[segmentId.Id];
			}
		}
        #endregion Segment Properties
    }
}
