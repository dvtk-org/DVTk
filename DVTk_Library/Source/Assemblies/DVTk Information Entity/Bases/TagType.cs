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

namespace Dvtk.Dicom.InformationEntity
{
	public enum TagTypeEnum 
	{
		TagRequired,
		TagUnique,
		TagOptional,
		TagConditional
	}

	/// <summary>
	/// Summary description for TagType.
	/// </summary>
	public class TagType
	{
		private Tag _tag;
		private VR _vr;
		private TagTypeEnum _type;

		/// <summary>
		/// Class Constructor.
		/// </summary>
		/// <param name="group">Tag - group number.</param>
		/// <param name="element">Tag - element number.</param>
		public TagType(System.UInt16 group, System.UInt16 element)
		{
			_tag = new Tag(group, element);
			_type = TagTypeEnum.TagOptional;
			_vr = VR.UN;
		}

		/// <summary>
		/// Class Constructor.		
		/// </summary>
		/// <param name="group">Tag - group number.</param>
		/// <param name="element">Tag - element number.</param>
		/// <param name="type">Tag Type.</param>
		public TagType(System.UInt16 group, System.UInt16 element, TagTypeEnum type)
		{
			_tag = new Tag(group, element);
			_type = type;
			_vr = VR.UN;
		}

		/// <summary>
		/// Class Constructor.
		/// </summary>
		/// <param name="tag">Tag (group/element combination).</param>
		public TagType(Tag tag)
		{
			_tag = new Tag();
			_tag.GroupNumber = tag.GroupNumber;
			_tag.ElementNumber = tag.ElementNumber;
			_type = TagTypeEnum.TagOptional;
			_vr = VR.UN;
		}

		/// <summary>
		/// Class Constructor.
		/// </summary>
		/// <param name="tag">Tag (group/element combination).</param>
		/// <param name="type">Tag Type.</param>
		public TagType(Tag tag, TagTypeEnum type)
		{
			_tag = new Tag();
			_tag.GroupNumber = tag.GroupNumber;
			_tag.ElementNumber = tag.ElementNumber;
			_type = type;
			_vr = VR.UN;
		}

		/// <summary>
		/// Class Constructor.
		/// </summary>
		/// <param name="tag">Tag (group/element combination).</param>
		/// <param name="vr">Tag VR.</param>
		/// <param name="type">Tag Type.</param>
		public TagType(Tag tag, VR vr, TagTypeEnum type)
		{
			_tag = new Tag();
			_tag.GroupNumber = tag.GroupNumber;
			_tag.ElementNumber = tag.ElementNumber;
			_vr = vr;
			_type = type;
		}

		/// <summary>
		/// Get the Tag.
		/// </summary>
		public Tag Tag
		{
			get
			{
				return _tag;
			}
		}

		/// <summary>
		/// Get the VR.
		/// </summary>
		public VR Vr
		{
			get
			{
				return _vr;
			}
		}

		/// <summary>
		/// Get the Tag Type.
		/// </summary>
		public TagTypeEnum Type
		{
			get
			{
				return _type;
			}
		}
	}
}
