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
	/// ORR Message class
	/// </summary>
	public class OrrMessage : Hl7Message
	{
        /// <summary>
		/// Class constructor - ORR
		/// </summary>
        public OrrMessage() : base() { }

		/// <summary>
		/// Class constructor - ORR
		/// </summary>
        /// <param name="messageSubType">ADT Message Sub Type.</param>
        public OrrMessage(System.String messageSubType)
            : base("ACK", messageSubType)
		{
			Hl7Segment segment = new Hl7Segment(1, Hl7SegmentEnum.MSA);
			Segments.Add(segment.SegmentId.Id, segment);
			segment = new Hl7Segment(2, Hl7SegmentEnum.ORC);
			Segments.Add(segment.SegmentId.Id, segment);
			segment = new Hl7Segment(3, Hl7SegmentEnum.OBR);
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
		/// Property - ORC Segment
		/// </summary>
		public Hl7Segment ORC
		{
			get
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.ORC);
				return (Hl7Segment)Segments[segmentId.Id];
			}
		}

		/// <summary>
		/// Property - OBR Segment
		/// </summary>
		public Hl7Segment OBR
		{
			get
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.OBR);
				return (Hl7Segment)Segments[segmentId.Id];
			}
		}
        #endregion Segment Properties
    }
}
