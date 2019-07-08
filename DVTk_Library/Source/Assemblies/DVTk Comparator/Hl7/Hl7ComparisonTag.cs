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
using Dvtk.Hl7.Messages;
using Dvtk.CommonDataFormat;

namespace Dvtk.Comparator
{
	/// <summary>
	/// Summary description for Hl7ComparisonTag.
	/// </summary>
	public class Hl7ComparisonTag
	{
		private Hl7Tag _tag = null;
		private BaseCommonDataFormat _commonDataFormat = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="tag">Comparison Tag</param>
		/// <param name="commonDataFormat">Data Format for Tag</param>
		public Hl7ComparisonTag(Hl7Tag tag, BaseCommonDataFormat commonDataFormat)
		{
			_tag = tag;
			_commonDataFormat = commonDataFormat;
		}

		#region properties
		/// <summary>
		/// Tag property.
		/// </summary>
		public Hl7Tag Tag
		{
			get
			{
				return _tag;
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
