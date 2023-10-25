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
	/// ADT Message class
	/// </summary>
	public class AdtMessage : Hl7Message
	{
        /// <summary>
		/// Class constructor - ADT
		/// </summary>
        public AdtMessage() : base() { }

		/// <summary>
		/// Class constructor - ADT
		/// </summary>
		/// <param name="messageSubType">ADT Message Sub Type.</param>
		public AdtMessage(System.String messageSubType) : base("ADT", messageSubType)
		{
			Hl7Segment segment = new Hl7Segment(1, Hl7SegmentEnum.EVN);
			Segments.Add(segment.SegmentId.Id, segment);
			segment = new Hl7Segment(2, Hl7SegmentEnum.PID);
			Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(3, Hl7SegmentEnum.PD1);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(4, Hl7SegmentEnum.NK1);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(5, Hl7SegmentEnum.PV1);
			Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(6, Hl7SegmentEnum.PV2);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(7, Hl7SegmentEnum.DB1);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(8, Hl7SegmentEnum.OBX);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(9, Hl7SegmentEnum.AL1);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(10, Hl7SegmentEnum.DG1);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(11, Hl7SegmentEnum.DRG);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(12, Hl7SegmentEnum.PR1);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(13, Hl7SegmentEnum.ROL);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(14, Hl7SegmentEnum.GT1);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(15, Hl7SegmentEnum.IN1);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(16, Hl7SegmentEnum.IN2);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(17, Hl7SegmentEnum.IN3);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(18, Hl7SegmentEnum.ACC);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(19, Hl7SegmentEnum.UB1);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(20, Hl7SegmentEnum.UB2);
            Segments.Add(segment.SegmentId.Id, segment);
            segment = new Hl7Segment(21, Hl7SegmentEnum.MRG);
			Segments.Add(segment.SegmentId.Id, segment);
        }

        #region Segment Properties
        /// <summary>
		/// Property - EVN Segment
		/// </summary>
		public Hl7Segment EVN
		{
			get
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.EVN);
				return (Hl7Segment)Segments[segmentId.Id];
			}
		}

		/// <summary>
		/// Property - PID Segment
		/// </summary>
		public Hl7Segment PID
		{
			get
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.PID);
				return (Hl7Segment)Segments[segmentId.Id];
			}
		}

        /// <summary>
        /// Property - PD1 Segment
        /// </summary>
        public Hl7Segment PD1
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.PD1);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - NK1 Segment
        /// </summary>
        public Hl7Segment NK1
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.NK1);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

		/// <summary>
		/// Property - PV1 Segment
		/// </summary>
		public Hl7Segment PV1
		{
			get
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.PV1);
				return (Hl7Segment)Segments[segmentId.Id];
			}
		}

        /// <summary>
        /// Property - PV2 Segment
        /// </summary>
        public Hl7Segment PV2
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.PV2);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - DB1 Segment
        /// </summary>
        public Hl7Segment DB1
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.DB1);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - OBX Segment
        /// </summary>
        public Hl7Segment OBX
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.OBX);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - AL1 Segment
        /// </summary>
        public Hl7Segment AL1
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.AL1);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - DG1 Segment
        /// </summary>
        public Hl7Segment DG1
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.DG1);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - DRG Segment
        /// </summary>
        public Hl7Segment DRG
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.DRG);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - PR1 Segment
        /// </summary>
        public Hl7Segment PR1
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.PR1);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - ROL Segment
        /// </summary>
        public Hl7Segment ROL
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.ROL);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - GT1 Segment
        /// </summary>
        public Hl7Segment GT1
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.GT1);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - IN1 Segment
        /// </summary>
        public Hl7Segment IN1
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.IN1);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - IN2 Segment
        /// </summary>
        public Hl7Segment IN2
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.IN2);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - IN3 Segment
        /// </summary>
        public Hl7Segment IN3
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.IN3);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - ACC Segment
        /// </summary>
        public Hl7Segment ACC
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.ACC);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - UB1 Segment
        /// </summary>
        public Hl7Segment UB1
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.UB1);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
        /// Property - UB2 Segment
        /// </summary>
        public Hl7Segment UB2
        {
            get
            {
                Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.UB2);
                return (Hl7Segment)Segments[segmentId.Id];
            }
        }

        /// <summary>
		/// Property - MRG Segment
		/// </summary>
		public Hl7Segment MRG
		{
			get
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.MRG);
				return (Hl7Segment)Segments[segmentId.Id];
			}
		}

		/// <summary>
		/// Set the OBX value given at the segment/field index.
		/// </summary>
		/// <param name="segmentIndex">OBX Segment Index.</param>
		/// <param name="fieldIndex">OBX Field Index (within Segment).</param>
		/// <param name="stringValue">Value to set.</param>
		public void obxSeg(int segmentIndex, int fieldIndex, System.String stringValue)
		{
			Hl7Segment segment = null;
			if (segmentIndex > 0)
			{
				Hl7SegmentId segmentId = new Hl7SegmentId(Hl7SegmentEnum.OBX, segmentIndex);
				segment = (Hl7Segment)Segments[segmentId.Id];
				if (segment == null)
				{
					segment = new Hl7Segment(0, Hl7SegmentEnum.OBX);
					segment.SegmentId.SegmentIndex = segmentIndex;
					AddSegment(segment);
				}
			}
			segment[fieldIndex] = stringValue;
		}
        #endregion Segment Properties
    }
}
