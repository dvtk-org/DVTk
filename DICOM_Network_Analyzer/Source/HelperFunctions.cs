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
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Sniffer
{
	using SECURITY_DESCRIPTOR = System.Int32;

	public class Function
	{
		public static uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100;
		public static uint FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
		public static uint LANG_NEUTRAL = 0x0;
		public static uint SUBLANG_DEFAULT = 0x1;

		public struct OSVERSIONINFO
		{  
			public uint dwOSVersionInfoSize; 
			public uint dwMajorVersion; 
			public uint dwMinorVersion; 
			public uint dwBuildNumber; 
			public uint dwPlatformId; 
			[MarshalAs(UnmanagedType.ByValTStr , SizeConst=128)] public string szCSDVersion; 
		}

		//**********************************************************************
		[DllImport("kernel32.dll")] public extern static int
			GetVersionEx( ref Function.OSVERSIONINFO lpVersionInformation );	// pointer to version information structure

		//**********************************************************************
		[DllImport("kernel32.dll", EntryPoint="GetFileAttributesW")] public static extern int
			GetFileAttributes( string lpFileName ); 	// address of the name of a file or directory  
		//**********************************************************************

		[DllImport("kernel32.dll")] public static extern int
			CopyFile(
			int lpExistingFileName,	// pointer to name of an existing file 
			int lpNewFileName,	// pointer to filename to copy to 
			bool bFailIfExists ); // flag for operation if file exists 

		[DllImport("kernel32.dll")] public static extern int
			CopyFile(
			string lpExistingFileName,	// pointer to name of an existing file 
			string lpNewFileName,	// pointer to filename to copy to 
			bool bFailIfExists ); // flag for operation if file exists 
		//**********************************************************************

		[DllImport("kernel32.dll")] public extern static int
			GetLastError();
		//**********************************************************************
		[DllImport("kernel32.dll")] public extern static void
			SetLastError( ulong dwErrCode ); // per-thread error code  

		[DllImport("kernel32.dll")] public extern static void
			SetLastError( int dwErrCode ); // per-thread error code  

		[DllImport("kernel32.dll")] public extern static int
			FormatMessage(
			int dwFlags,	// source and processing options 
			int lpSource,	// pointer to  message source 
			int dwMessageId,	// requested message identifier 
			int dwLanguageId,	// language identifier for requested message 
			ref int lpBuffer,	// pointer to message buffer 
			int nSize,	// maximum size of message buffer 
			int Arguments 	// address of array of message inserts 
			);
		//**********************************************************************
		[DllImport("kernel32.dll")] public extern static int
			LocalFree( int hMem ); 	// handle of local memory object 
		//**********************************************************************

		public static string GetIpAddress( string SrvName )
		{
			IPHostEntry ipEntry = new IPHostEntry();
			IPAddress Ip = new IPAddress(0);

			if( SrvName.Trim() == "" )
			{
				ipEntry = Dns.GetHostEntry( Dns.GetHostName() );
			}
			else
			{
                ipEntry = Dns.GetHostEntry(SrvName);
			}

			Ip = ipEntry.AddressList[0];

			return Ip.ToString();
		}

		public static bool IsFileExists( string FileName )
		{
			bool IsExists = false;
			int LastError = -1;

			uint FileAttr = (uint) GetFileAttributes( FileName );
			if( FileAttr == 0xFFFFFFFF )
			{
				LastError = GetLastError();
				if( LastError == 2 )
					IsExists = false;
				else
					IsExists = true;

			}
			else
				IsExists = true;

			return IsExists;
		}

		public static bool CopyFileTo( string OsName )
		{
			string SrcFileNameNT = "npf_NT.sys";
			string SrcFileName2KXP = "npf_2KXP.sys";
			string DstFileName = "c:\\winnt\\system32\\drivers\\npf.sys";
			int Result = -1;
			

			if( OsName == "WINNT" )
			{

				if( !IsFileExists( DstFileName ) )
				{
					Result = CopyFile( SrcFileNameNT , DstFileName , false );
					if( Result == 0 )
					{
						Result = GetLastError();
					}
				}
			}
			else if( ( OsName == "WIN2000" ) || ( OsName == "WINXP" ) )
			{

				if( !IsFileExists( DstFileName ) )
				{
					Result = CopyFile( SrcFileName2KXP , DstFileName , false );
					if( Result == 0 )
					{
						Result = GetLastError();
					}
				}
			}


			return true;
		}

		public static void GetOSInfo( ref OSVERSIONINFO mOSInfo , ref string mOSName )
		{
			int Result = 0;

			mOSInfo.szCSDVersion = Function.Space( 128 );
			mOSInfo.dwOSVersionInfoSize = (uint) Marshal.SizeOf( mOSInfo );
			Result = GetVersionEx( ref mOSInfo );
			mOSName = "";

			if( Result == 0 ) return;

			switch( mOSInfo.dwPlatformId )
			{
				case Const.VER_PLATFORM_WIN32_NT:
					
					if( mOSInfo.dwMajorVersion <= 4 )
						mOSName = "WINNT";

					if( mOSInfo.dwMajorVersion == 5 && mOSInfo.dwMinorVersion == 0 )
						mOSName = "WIN2000";

					if( mOSInfo.dwMajorVersion == 5 && mOSInfo.dwMinorVersion == 1 )
						mOSName = "WINXP";

                    if (mOSInfo.dwMajorVersion == 5 && mOSInfo.dwMinorVersion == 2)
                        mOSName = "WIN2003";

					break;

				case Const.VER_PLATFORM_WIN32_WINDOWS:

					if( ( mOSInfo.dwMajorVersion == 4 ) && ( mOSInfo.dwMinorVersion == 0 ) )
						mOSName = "WIN95";
					if( ( mOSInfo.dwMajorVersion == 4 ) && ( mOSInfo.dwMinorVersion == 10 ) )
						mOSName = "WIN98";
					if( ( mOSInfo.dwMajorVersion == 4 ) && ( mOSInfo.dwMinorVersion == 90 ) )
						mOSName = "WIN98ME";
					
					break;

				case Const.VER_PLATFORM_WIN32s:

					mOSName = "WIN32S";
				
					break;
			}

		}

		public static string Space( int num )
		{
			string tmp = "";
			for( int i = 0; i < num; i ++ )
				tmp += " ";

			return tmp;

		}

		public static string ReturnErrorMessage( Exception Ex )
		{
			string Err = "";

			Err = "Error Type is " + Ex.GetType().ToString() + (char) 13 + (char) 10;
			Err += "Error Source is " + Ex.Source + (char) 13 + (char) 10;
			Err += "Error Trace is " + Ex.StackTrace + (char) 13 + (char) 10;
			Err += "Error Message is " + Ex.Message;

			return Err;
		}

		public static int MakeLangId( uint p, uint s)
		{ return (int) ( s * 1024 + p ); }


		public static string GetSystemErrorMessage( int ErrorNo )
		{
			string Tmp = "";
			int error = 0;
			int Buffer = 0;
			byte b = 0;

			int Result = FormatMessage( 
				(int) ( FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM ),
				0,
				ErrorNo,
				MakeLangId( LANG_NEUTRAL, SUBLANG_DEFAULT), // Default language
				ref Buffer,
				4096,
				0 
				);

			if( Result == 0 )
			{
				error = GetLastError();
				return "";
			}

			b = Marshal.ReadByte( ( IntPtr ) ( Buffer + 1 ) );
			if( b == 0 )
				Tmp = Marshal.PtrToStringUni( ( IntPtr ) Buffer );
			else
				Tmp = Marshal.PtrToStringAnsi( ( IntPtr ) Buffer );

			LocalFree( Buffer );

			return Tmp;

		}

		public static ushort Get2Bytes( byte [] ptr , ref int Index , int Type )
		{
			ushort u = 0;

			if( Type == Const.NORMAL )
			{
				u = ( ushort ) ptr[ Index++ ];
				u *= 256;
				u += ( ushort ) ptr[ Index++ ];
			}
			else if( Type == Const.VALUE )
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

			if( Type == Const.NORMAL )
			{
				ui = ( (uint) ptr[ Index++ ] ) << 24; 
				ui += ( (uint) ptr[ Index++ ] ) << 16;
				ui += ( (uint) ptr[ Index++ ] ) << 8;
				ui += (uint) ptr[ Index++ ];
			}
			else if( Type == Const.VALUE )
			{
				ui = ( (uint) ptr[ Index + 3 ] ) << 24; 
				ui += ( (uint) ptr[ Index + 2 ] ) << 16;
				ui += ( (uint) ptr[ Index + 1 ] ) << 8;
				ui += (uint) ptr[ Index ]; Index += 4;
			}

			return ui;
		}

		public static ushort Get2BytesFromStream(  Stream s , int Type )
		{
			ushort u = 0;

			if( Type == Const.NORMAL )
			{
				u = ( ushort ) (s.ReadByte() << 8);
				u += ( ushort )s.ReadByte();
			}
			else if( Type == Const.VALUE )
			{
				u = ( ushort )s.ReadByte() ;
				u += ( ushort )(s.ReadByte() << 8) ;
			}

			return u;
		}

		public static uint Get4BytesFromStream( Stream s , int Type )
		{
			uint ui = 0;

			if( Type == Const.NORMAL )
			{
				ui = ( (uint) s.ReadByte() ) << 24; 
				ui += ( (uint) s.ReadByte()  ) << 16;
				ui += ( (uint) s.ReadByte()  ) << 8;
				ui += (uint) s.ReadByte()  ;
			}
			else if( Type == Const.VALUE )
			{
				ui = (uint) s.ReadByte() ; 
				ui += ( (uint) s.ReadByte()  ) << 8;
				ui += ( (uint) s.ReadByte()  ) << 16;
				ui += ( (uint) s.ReadByte()  ) << 24; 
			}

			return ui;
		}

		public static ulong Get8BytesFromStream( Stream s , int Type )
		{
			ulong ul = 0;

			if( Type == Const.NORMAL )
			{
				ul = ( (ulong) Get4BytesFromStream(s,Type) ) << 32;
				ul += (ulong) Get4BytesFromStream(s,Type);
			}
			else if( Type == Const.VALUE )
			{
				ul = (ulong) Get4BytesFromStream(s,Type);
				ul += ( (ulong) Get4BytesFromStream(s,Type) ) << 32;
			}

			return ul;
		}

		public static void Set4Bytes( ref byte [] ptr , int Index , uint NewValue , int Type )
		{
			if( Type == Const.NORMAL )
			{
				ptr[ Index + 1 ] = (byte) ( NewValue >> 24 );
				ptr[ Index + 2 ] = (byte) ( NewValue >> 16 );
				ptr[ Index + 3 ] = (byte) ( NewValue >> 8 );
				ptr[ Index + 4 ] = (byte) NewValue ;
			}
			else if( Type == Const.VALUE )
			{
				ptr[ Index + 0 ] = (byte) NewValue;
				ptr[ Index + 1 ] = (byte) ( NewValue >> 8 );
				ptr[ Index + 2 ] = (byte) ( NewValue >> 16 );
				ptr[ Index + 3 ] = (byte) ( NewValue >> 24 );
			}
		}

		public static void Set8Bytes( ref byte [] ptr , int Index , ulong NewValue , int Type )
		{
			if( Type == Const.NORMAL )
			{
				ptr[ Index ] =  (byte) ( NewValue >> 56 ); 
				ptr[ Index + 1 ] =  (byte) ( NewValue >> 48 ); 
				ptr[ Index + 2 ] =  (byte) ( NewValue >> 40 ); 
				ptr[ Index + 3 ] =  (byte) ( NewValue >> 32 ); 
				ptr[ Index + 4 ] =  (byte) ( NewValue >> 24 ); 
				ptr[ Index + 5 ] =  (byte) ( NewValue >> 16 ); 
				ptr[ Index + 6 ] =  (byte) ( NewValue >> 8 ); 
				ptr[ Index + 7 ] =  (byte) NewValue;
			}
			else if( Type == Const.VALUE )
			{
				ptr[ Index + 7 ] =  (byte) ( NewValue >> 56 ); 
				ptr[ Index + 6 ] =  (byte) ( NewValue >> 48 ); 
				ptr[ Index + 5 ] =  (byte) ( NewValue >> 40 ); 
				ptr[ Index + 4 ] =  (byte) ( NewValue >> 32 ); 
				ptr[ Index + 3 ] =  (byte) ( NewValue >> 24 ); 
				ptr[ Index + 2 ] =  (byte) ( NewValue >> 16 ); 
				ptr[ Index + 1 ] =  (byte) ( NewValue >> 8 ); 
				ptr[ Index ] =  (byte) NewValue;
			}
		}

		public static string GetIpAddress( byte [] ptr , ref int Index )
		{
			string str = "";

			str += ptr[ Index++ ].ToString() + ".";
			str += ptr[ Index++ ].ToString() + ".";
			str += ptr[ Index++ ].ToString() + ".";
			str += ptr[ Index++ ].ToString();

			return str;
		}		
	}
}
