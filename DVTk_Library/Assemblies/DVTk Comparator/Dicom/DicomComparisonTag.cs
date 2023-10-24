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

using DvtkData.Dimse;
using Dvtk.CommonDataFormat;

namespace Dvtk.Comparator
{
	/// <summary>
	/// Summary description for DicomComparisonTag.
	/// </summary>
	public class DicomComparisonTag
	{
		private DvtkData.Dimse.Tag _parentSequenceTag = Tag.UNDEFINED;
		private DvtkData.Dimse.Tag _tag = null;
		private DvtkData.Dimse.VR _vr = VR.UN;
		private BaseCommonDataFormat _commonDataFormat = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="tag">Comparison Tag</param>
		/// <param name="vr">Tag VR.</param>
		/// <param name="commonDataFormat">Data Format for Tag</param>
		public DicomComparisonTag(DvtkData.Dimse.Tag tag, VR vr, BaseCommonDataFormat commonDataFormat)
		{
			_tag = tag;
			_vr = vr;
			_commonDataFormat = commonDataFormat;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentSequenceTag">Parent Sequence Tag</param>
		/// <param name="tag">Comparison Tag</param>
        /// <param name="vr">Tag VR</param>
		/// <param name="commonDataFormat">Data Format for Tag</param>
		public DicomComparisonTag(DvtkData.Dimse.Tag parentSequenceTag, 
									DvtkData.Dimse.Tag tag, 
									VR vr,
									BaseCommonDataFormat commonDataFormat)
		{
			_parentSequenceTag = parentSequenceTag;
			_tag = tag;
			_vr = vr;
			_commonDataFormat = commonDataFormat;
		}

		#region properties
		/// <summary>
		/// ParentSequenceTag property.
		/// </summary>
		public DvtkData.Dimse.Tag ParentSequenceTag
		{
			get
			{
				return _parentSequenceTag;
			}
		}

		/// <summary>
		/// Tag property.
		/// </summary>
		public DvtkData.Dimse.Tag Tag
		{
			get
			{
				return _tag;
			}
		}

		public DvtkData.Dimse.VR Vr
		{
			get
			{
				return _vr;
			}
		}

		/// <summary>
		/// DataFormat property.
		/// </summary>
		public BaseCommonDataFormat DataFormat
		{
			get
			{
				return _commonDataFormat;
			}
		}
		#endregion
	}
}
