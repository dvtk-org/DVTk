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
	/// Summary description for Hl7SegmentId.
	/// </summary>
	public class Hl7SegmentId
	{
		private Hl7SegmentEnum _segmentName = Hl7SegmentEnum.Unknown;
		private int _segmentIndex = 0;


		/// <summary>
		/// Needed for serialization.
		/// </summary>
		public Hl7SegmentId()
		{

		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="segmentName">Segment name.</param>
		/// <param name="segmentIndex">One-based segment index.</param>
		public Hl7SegmentId(Hl7SegmentEnum segmentName, int segmentIndex)
		{
			_segmentName = segmentName;
			_segmentIndex = segmentIndex;
		}

		/// <summary>
		/// Class constructor - the segment index defaults to 1.
		/// </summary>
		/// <param name="segmentName">Segment name.</param>
		public Hl7SegmentId(Hl7SegmentEnum segmentName) : this(segmentName, 1) {}

		/// <summary>
		/// Property - Segment Name.
		/// </summary>
		public Hl7SegmentEnum SegmentName
		{
			get
			{
				return _segmentName;
			}
			set
			{
				_segmentName = value;
			}
		}

		/// <summary>
		/// Property - Segment Index.
		/// </summary>
		public int SegmentIndex
		{
			get
			{
				return _segmentIndex;
			}
			set
			{
				_segmentIndex = value;
			}
		}

		public System.String Id
		{
			get
			{
				System.String id = System.String.Format("{0}:{1}", SegmentNames.Name(_segmentName), _segmentIndex);
				return id;
			}
		}
	}
}
