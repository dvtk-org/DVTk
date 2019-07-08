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

namespace DICOM

{
	/// <summary>
	/// Summary description for CVRType.
	/// </summary>
	internal class CVRType
	{
		private ushort value;
		public const ushort US=0x5553;
		public const ushort UN=0x554E;
		public const ushort	FD=0x4644;
		public const ushort SL=0x534C;
		public const ushort UL=0x554C;
		public const ushort FL=0x464C;
		public const ushort SS=0x5353;
		public const ushort OW=0x4F57;
		public const ushort AT=0x4154;
		public const ushort SQ=0x5351;
		public const ushort OB=0x4F42;

		public const ushort	IS=0x4953;
		public const ushort AE=0x4145;
		public const ushort	CS=0x4353;
		public const ushort DS=0x4453;
		public const ushort SH=0x5348;
		public const ushort LO=0x4C4F;
		public const ushort PN=0x504E;
		public const ushort UI=0x5549;
		public const ushort ST=0x5354;
		public const ushort LT=0x4C54;
		public const ushort UT=0x5554;
		public const ushort AS=0x4153;
		public const ushort DA=0x4441;
		public const ushort DT=0x4454;
		public const ushort TM=0x544D;
		public const ushort Z2=0x5A32;
		public const ushort Z3=0x5A33;
		public const ushort Z4=0x5A34;
		public const ushort Z5=0x5A35;
		public const ushort PD=0x5044;
		public const ushort OF=0x4F46;

		public const ushort Blank=0x2020;
		public const ushort QuQu=0x3F3F; //??

		public CVRType()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public CVRType(ushort val)
		{
			value=val;
		}
		public static bool operator ==(CVRType vr1, ushort value)
		{
			return vr1.value==value;
		}

		public static bool operator !=(CVRType vr1, ushort value)
		{
			return vr1.value!=value;
		}	
		public override bool Equals(object a) { return (a as CVRType).value==value;}
		public override int GetHashCode() { return value;}

		public static implicit operator CVRType (ushort value )
		{
			CVRType newval=new CVRType();
			newval.value=value;
			return newval;
		}
		public static implicit operator CVRType (string value )
		{
			CVRType newval=new CVRType();
			newval.value=(ushort)((value[0]<<8)+value[1]);
			return newval;
		}
		public static implicit operator ushort  (CVRType value )
		{
			return value.value;
		}
		public override string ToString()
		{
			return new string((char)(value>>8),1) + new string((char)(value & 0xFF),1);
		}
		public bool isLongHeader()
		{
			switch(value)
			{
				case IS:		// String max 12
				case AE:		// String max 16
				case CS:
				case DS:
				case SH:
				case LO:		// String max 64
				case PN:
				case UI:		// String max 64 pad zero
				case ST:		// String max 1024
				case LT:		// String max 10240
				case AS:		// String fixed 4
				case DA:
				case TM:
				case DT:
				case SS:
				case US:
				case SL:
				case UL:
				case FL:
				case OF:
				case FD:
				case AT:
				case Z3:
				case Z2:
				case QuQu:		// Siemens Plus 4 hack
					return false;

				default:  // i.e. 'UN':'SQ':'UT':'OB':'OW' + other as yet unknown
					return true;
			}
		}
	}
}

