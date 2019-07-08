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

namespace Sniffer
{
	public class Const
	{
		public const int NORMAL = 0;
		public const int VALUE = 1;

		public static uint INFINITE = 0xFFFF;
		public static uint ERROR_ALREADY_EXISTS = 183;

		public static uint OPEN_EXISTING = 3;
		public static uint GENERIC_READ = 0x80000000;
		public static uint GENERIC_WRITE = 0x40000000;

		public const uint VER_PLATFORM_WIN32_NT = 2;
		public const uint VER_PLATFORM_WIN32_WINDOWS = 1;
		public const uint VER_PLATFORM_WIN32s = 0;

		public static uint Packet_ALIGNMENT = 4;
	}
}
