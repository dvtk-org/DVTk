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
using Dvtk.Hl7.Messages;

namespace Dvtk.Hl7
{
	#region segment names
	public enum Hl7SegmentEnum
	{
		MSH,
		MSA,
		ERR,
		EVN,
		PID,
        PD1,
        NK1,
		PV1,
        PV2,
        DB1,
        DG1,
        DRG,
        PR1,
        ROL,
        GT1,
        IN1,
        IN2,
        IN3,
        ACC,
        UB1,
        UB2,
		MRG,
		ORC,
		OBR,
		OBX,
		AL1,
        NTE,
        CTI,
        DSC,
        BLG,
		ZDS,
		QRD,
		Unknown
	}

	public class SegmentNames
	{
		public static System.String Name(Hl7SegmentEnum segmentName)
		{
			System.String name = "Unknown";

			switch(segmentName)
			{
				case Hl7SegmentEnum.MSH: name = "MSH"; break;
				case Hl7SegmentEnum.MSA: name = "MSA"; break;
				case Hl7SegmentEnum.ERR: name = "ERR"; break;
				case Hl7SegmentEnum.EVN: name = "EVN"; break;
				case Hl7SegmentEnum.PID: name = "PID"; break;
                case Hl7SegmentEnum.PD1: name = "PD1"; break;
                case Hl7SegmentEnum.NK1: name = "NK1"; break;
                case Hl7SegmentEnum.PV1: name = "PV1"; break;
                case Hl7SegmentEnum.PV2: name = "PV2"; break;
                case Hl7SegmentEnum.DB1: name = "DB1"; break;
                case Hl7SegmentEnum.DG1: name = "DG1"; break;
                case Hl7SegmentEnum.DRG: name = "DRG"; break;
                case Hl7SegmentEnum.PR1: name = "PR1"; break;
                case Hl7SegmentEnum.ROL: name = "ROL"; break;
                case Hl7SegmentEnum.GT1: name = "GT1"; break;
                case Hl7SegmentEnum.IN1: name = "IN1"; break;
                case Hl7SegmentEnum.IN2: name = "IN2"; break;
                case Hl7SegmentEnum.IN3: name = "IN3"; break;
                case Hl7SegmentEnum.ACC: name = "ACC"; break;
                case Hl7SegmentEnum.UB1: name = "UB1"; break;
                case Hl7SegmentEnum.UB2: name = "UB2"; break;
                case Hl7SegmentEnum.MRG: name = "MRG"; break;
				case Hl7SegmentEnum.ORC: name = "ORC"; break;
				case Hl7SegmentEnum.OBR: name = "OBR"; break;
				case Hl7SegmentEnum.OBX: name = "OBX"; break;
				case Hl7SegmentEnum.AL1: name = "AL1"; break;
                case Hl7SegmentEnum.NTE: name = "NTE"; break;
                case Hl7SegmentEnum.CTI: name = "CTI"; break;
                case Hl7SegmentEnum.DSC: name = "DSC"; break;
                case Hl7SegmentEnum.BLG: name = "BLG"; break;
                case Hl7SegmentEnum.ZDS: name = "ZDS"; break;
				case Hl7SegmentEnum.QRD: name = "QRD"; break;
				default: break;
			}

			return name;
		}

		public static Hl7SegmentEnum NameEnum(System.String name)
		{
			Hl7SegmentEnum nameEnum = Hl7SegmentEnum.Unknown;

			switch(name)
			{
				case "MSH":	nameEnum = Hl7SegmentEnum.MSH; break;
				case "MSA": nameEnum = Hl7SegmentEnum.MSA; break;
				case "ERR": nameEnum = Hl7SegmentEnum.ERR; break;
				case "EVN": nameEnum = Hl7SegmentEnum.EVN; break;
				case "PID": nameEnum = Hl7SegmentEnum.PID; break;
                case "PD1": nameEnum = Hl7SegmentEnum.PD1; break;
                case "NK1": nameEnum = Hl7SegmentEnum.NK1; break;
                case "PV1": nameEnum = Hl7SegmentEnum.PV1; break;
                case "PV2": nameEnum = Hl7SegmentEnum.PV2; break;
                case "DB1": nameEnum = Hl7SegmentEnum.DB1; break;
                case "DG1": nameEnum = Hl7SegmentEnum.DG1; break;
                case "DRG": nameEnum = Hl7SegmentEnum.DRG; break;
                case "PR1": nameEnum = Hl7SegmentEnum.PR1; break;
                case "ROL": nameEnum = Hl7SegmentEnum.ROL; break;
                case "GT1": nameEnum = Hl7SegmentEnum.GT1; break;
                case "IN1": nameEnum = Hl7SegmentEnum.IN1; break;
                case "IN2": nameEnum = Hl7SegmentEnum.IN2; break;
                case "IN3": nameEnum = Hl7SegmentEnum.IN3; break;
                case "ACC": nameEnum = Hl7SegmentEnum.ACC; break;
                case "UB1": nameEnum = Hl7SegmentEnum.UB1; break;
                case "UB2": nameEnum = Hl7SegmentEnum.UB2; break;
                case "MRG": nameEnum = Hl7SegmentEnum.MRG; break;
				case "ORC": nameEnum = Hl7SegmentEnum.ORC; break;
				case "OBR": nameEnum = Hl7SegmentEnum.OBR; break;
				case "OBX": nameEnum = Hl7SegmentEnum.OBX; break;
				case "AL1": nameEnum = Hl7SegmentEnum.AL1; break;
                case "NTE": nameEnum = Hl7SegmentEnum.NTE; break;
                case "CTI": nameEnum = Hl7SegmentEnum.CTI; break;
                case "DSC": nameEnum = Hl7SegmentEnum.DSC; break;
                case "BLG": nameEnum = Hl7SegmentEnum.BLG; break;
				case "ZDS": nameEnum = Hl7SegmentEnum.ZDS; break;
                case "QRD": nameEnum = Hl7SegmentEnum.QRD; break;
                default: break;
			}

			return nameEnum;
		}
	}
	#endregion

	/// <summary>
	/// Summary description for Hl7Tag.
	/// Initially we will use just the Segment Name and Offset - e.g. OBR-1 or OBX-5.
	/// </summary>
	public class Hl7Tag
	{
		private Hl7SegmentId _segmentId = null;
		private int _fieldIndex = 0;

		/// <summary>
		/// Needed for serialization.
		/// </summary>
		public Hl7Tag()
		{

		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="segment">Enumerated Segment Name.</param>
		/// <param name="fieldIndex">Zero-based segment field index.</param>
		public Hl7Tag(Hl7SegmentEnum segment, int fieldIndex)
		{
			_segmentId = new Hl7SegmentId(segment);
			_fieldIndex = fieldIndex;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="segment">Enumerated Segment Name.</param>
		/// <param name="segmentIndex">One-based segment index.</param>
		/// <param name="fieldIndex">Zero-based segment field index.</param>
		public Hl7Tag(Hl7SegmentEnum segment, int segmentIndex, int fieldIndex)
		{
			_segmentId = new Hl7SegmentId(segment, segmentIndex);
			_fieldIndex = fieldIndex;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="segment">String representation of Segment Name.</param>
        /// <param name="fieldIndex">Zero-based segment field index.</param>
		public Hl7Tag(System.String segment, int fieldIndex) : this(SegmentNames.NameEnum(segment), fieldIndex) {}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="segment">String representation of Segment Name.</param>
		/// <param name="segmentIndex">One-based segment index.</param>
        /// <param name="fieldIndex">Zero-based segment field index.</param>
		public Hl7Tag(System.String segment, int segmentIndex, int fieldIndex) : this(SegmentNames.NameEnum(segment), segmentIndex, fieldIndex) {}

		/// <summary>
		/// Property - Segment Id.
		/// </summary>
		public Hl7SegmentId SegmentId
		{
			get
			{
				return _segmentId;
			}
			set
			{
				_segmentId = value;
			}
		}

		/// <summary>
		/// Property - Segment.
		/// </summary>
		public Hl7SegmentEnum Segment
		{
			get
			{
				return _segmentId.SegmentName;
			}
		}

		/// <summary>
		/// Property - Segment Index.
		/// </summary>
		public int SegmentIndex
		{
			get
			{
				return _segmentId.SegmentIndex;
			}
		}

		/// <summary>
		/// Property - Zero-based segment field index.
		/// </summary>
		public int FieldIndex
		{
			get
			{
				return _fieldIndex;
			}
			set
			{
				_fieldIndex = value;
			}
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object
		/// </summary>
		/// <param name="obj">An <see cref="object"/> to compare with this instance, or a <see langword="null"/> reference.</param>
		/// <returns><see langword="true"/> if other is an instance of <see cref="Hl7Tag"/> and equals the value of this instance; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(System.Object obj) 
		{
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			Hl7Tag tag = (Hl7Tag)obj;
			return (
				this._segmentId.Id == tag._segmentId.Id &&
				this._fieldIndex == tag._fieldIndex);
		}

		/// <summary>
		/// Returns the object Hashcode.
		/// </summary>
		/// <returns>Hash code.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Determines whether two specified Hl7Tags are equal.
		/// </summary>
		/// <param name="tag1">A <see cref="Hl7Tag"/></param>
		/// <param name="tag2">A <see cref="Hl7Tag"/></param>
		/// <returns>
		/// <see langword="true"/> if <c>tag1</c> and <c>tag2</c> 
		/// represent the same <see cref="Hl7Tag"/>; 
		/// otherwise, <see langword="false"/>.
		/// </returns>
		public static bool operator ==(Hl7Tag tag1, Hl7Tag tag2) 
		{
			return (tag1.Equals(tag2));
		}

		/// <summary>
		/// Determines whether two specified Tags are not equal.
		/// </summary>
		/// <param name="tag1">A <see cref="Hl7Tag"/></param>
		/// <param name="tag2">A <see cref="Hl7Tag"/></param>
		/// <returns>
		/// <see langword="true"/> if <c>tag1</c> and <c>tag2</c> 
		/// do not represent the same <see cref="Hl7Tag"/>; 
		/// otherwise, <see langword="false"/>.
		/// </returns>
		public static bool operator !=(Hl7Tag tag1, Hl7Tag tag2) 
		{
			return (!tag1.Equals(tag2));
		}
	}
}
