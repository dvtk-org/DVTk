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

namespace Dvtk.Hl7
{
	/// <summary>
	/// Summary description for BaseHl7TagValue.
	/// </summary>
	public abstract class BaseHl7TagValue : BaseTagValue
	{
		private Hl7Tag _tag = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="tag">HL7 Tag.</param>
		public BaseHl7TagValue(Hl7Tag tag)
		{
			_tag = tag;
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
		#endregion
	}

	/// <summary>
	/// Summary description for Hl7TagValue.
	/// </summary>
	public class Hl7TagValue : BaseHl7TagValue
	{
		/// <summary>
		/// Class constructor.
		/// Value can be empty - universal match.
		/// </summary>
		/// <param name="tag">Tag</param>
		public Hl7TagValue(Hl7Tag tag) : base(tag)
		{
			_value = System.String.Empty;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="lValue">Value</param>
		public Hl7TagValue(Hl7Tag tag, System.String lValue) : base(tag)
		{
			_value = lValue;
		}
	}
}
