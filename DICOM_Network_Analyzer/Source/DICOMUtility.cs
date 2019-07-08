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
using System.Text;

namespace DICOM
{
	/// <summary>
	/// 
	/// </summary>
	public class DICOMUtility
	{
		const int BIGENDIAN = 0;
		const int LITTLEENDIAN = 1;

		public static uint Get4BytesBigEndian(byte [] p, ref uint Position)
		{
			int pos = (int)Position;
			uint result = Get4Bytes(p, ref pos,BIGENDIAN); 
			Position = (uint)pos;
			return result;
		}
		public static ushort Get2BytesBigEndian(byte [] p, ref uint Position)
		{
			int pos = (int)Position;
			ushort result = Get2Bytes(p, ref pos,BIGENDIAN); 
			Position = (uint)pos;
			return result;
		}		
		public static ushort Get2Bytes(byte [] p, ref uint Position, bool isBigEndian)
		{
			if(isBigEndian)
				return Get2BytesBigEndian(p, ref Position);
			else
			{
				ushort result = BitConverter.ToUInt16(p,(int)Position);
				Position += 2;
				return result;
			}
		}
		public static uint Get4Bytes(byte [] p, ref uint Position, bool isBigEndian)
		{
			if(isBigEndian)
				return Get4BytesBigEndian(p, ref Position);
			else
			{
				uint result = BitConverter.ToUInt32(p,(int)Position);
				Position += 4;
				return result;
			}
		}
		public static ushort Get2Bytes( byte [] ptr , ref int Index , int Type )
		{
			ushort u = 0;

			if( Type == BIGENDIAN )
			{
				u = ( ushort ) ptr[ Index++ ];
				u *= 256;
				u += ( ushort ) ptr[ Index++ ];
			}
			else if( Type == LITTLEENDIAN )
			{
				u = ( ushort ) ptr[ ++Index ];
				u *= 256; Index--;
				u += ( ushort ) ptr[ Index++ ]; Index++;
			}

			return u;
		}

		public static uint Get4Bytes( byte [] ptr , ref int Index , int Type )
		{
			uint ui = 0;

			if( Type == BIGENDIAN )
			{
				ui = ( (uint) ptr[ Index++ ] ) << 24; 
				ui += ( (uint) ptr[ Index++ ] ) << 16;
				ui += ( (uint) ptr[ Index++ ] ) << 8;
				ui += (uint) ptr[ Index++ ];
			}
			else if( Type == LITTLEENDIAN )
			{
				ui = ( (uint) ptr[ Index + 3 ] ) << 24; 
				ui += ( (uint) ptr[ Index + 2 ] ) << 16;
				ui += ( (uint) ptr[ Index + 1 ] ) << 8;
				ui += (uint) ptr[ Index ]; Index += 4;
			}

			return ui;
		}

        public static string GetHexString(byte[] mData, uint Position)
        {
            StringBuilder HexField = new StringBuilder();
            StringBuilder TextField = new StringBuilder();
            string HeaderStr = "";
            string HeaderStr2 = "";
            byte[] Data;
            int i = 0;

            int newrow = 0;
            int global = (int)(Position);
            string hex = " ";
            string numb = " ";
            string Tmp = "";
            int LastRow = mData.GetLength(0) / 16;
            int RemainingBytes = (LastRow * 16) - mData.GetLength(0);

            if (RemainingBytes < 0)
            {
                LastRow++;
                RemainingBytes += 16;
            }

            Data = new byte[LastRow * 16];
            for (i = 0; i < mData.Length; ++i)
            {
                Data[i] = mData[i];
            }

            HeaderStr = " Offset       ";
            HeaderStr2 = " ------------";
            for (i = 0; i < 16; i++)
            {
                HeaderStr += " " + i.ToString("d02");
                HeaderStr2 += "----";
            }

            HexField.Append(HeaderStr + "                  \n");
            HexField.Append(HeaderStr2 + "---------------------\n");

            for (i = 0; i < Data.Length; ++i)
            {
                if (newrow == 0)
                {
                    numb = PadZeros(global);
                    HexField.Append(" " + numb + " ");
                    global += 16;
                }

                hex = ConvertByteToHexString(Data[i]);

                HexField.Append(" " + hex);	// 3 characters

                int g = Data[i];
                if (g > 31 && g < 127)
                {
                    TextField.Append((char)Data[i]);
                }
                else
                {
                    TextField.Append(".");
                }

                ++newrow;

                if (newrow >= 16)
                {
                    HexField.Append("   " + TextField.ToString() + "\n");
                    TextField = new StringBuilder();
                    newrow = 0;
                }
            }

            HexField.Append("\n\n");
            Tmp = HexField.ToString();

            return Tmp;
        }

        private static string PadZeros(int inInt)
        {
            StringBuilder TextField = new StringBuilder();

            string hex = Convert.ToString(inInt, 16);

            if (hex.Length < 8)
            {
                int ix = 8 - hex.Length;
                for (int i = 0; i < ix; ++i)
                {
                    TextField.Append("0");
                }
            }
            TextField.Append(hex);
            return TextField.ToString().ToUpper();
        }

        private static string ConvertByteToHexString(byte inByte)
        {
            StringBuilder TextField = new StringBuilder();

            string hex = Convert.ToString(inByte, 16);

            if (hex.Length == 1)
                TextField.Append("0");

            TextField.Append(hex);

            return TextField.ToString().ToLower();
        }
	}
}
