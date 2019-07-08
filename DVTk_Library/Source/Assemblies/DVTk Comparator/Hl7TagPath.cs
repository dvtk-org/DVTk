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

namespace Dvtk.Comparator
{
	/// <summary>
	/// Summary description for Hl7TagPath.
	/// </summary>
	public class Hl7TagPath
	{
		private Hl7Tag _tag = null;
		private int _componentIndex = -1;
		private System.String _name = System.String.Empty;

		public Hl7TagPath(Hl7Tag tag, System.String name)
		{
			_tag = tag;
			_name = name;
		}

		public Hl7TagPath(Hl7Tag tag, int componentIndex, System.String name)
		{
			_tag = tag;
			_componentIndex = componentIndex;
			_name = name;
		}

		public Hl7Tag Tag
		{
			get
			{
				return _tag;
			}
		}

		public int ComponentIndex
		{
			get
			{
				return _componentIndex;
			}
		}

		/// <summary>
		/// Property - Name.
		/// </summary>
		public System.String Name
		{
			get
			{
				return _name;
			}
		}
	}
}
