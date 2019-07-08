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

namespace Dvtk.CommonDataFormat
{
	/// <summary>
	/// Summary description for CommonTimeFormat.
	/// </summary>
	public class CommonTimeFormat : BaseCommonDataFormat
	{
		private int _hour = -1;
		private int _minute = -1;
		private int _second = -1;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public CommonTimeFormat() {}

		#region base class overrides
		/// <summary>
		/// Convert from Common Data Format to DICOM format.
		/// </summary>
		/// <returns>DICOM format.</returns>
		public override System.String ToDicomFormat()
		{
			System.String dicomTime = System.String.Empty;

			// Check on parts of the time that are defined
			// - we will explicitly not check for a valid time.
			if ((_hour == -1) &&
				(_minute == -1) &&
				(_second == -1))
			{
				// return some initial time
				dicomTime = System.String.Empty;
			}
			else if ((_minute == -1) &&
				(_second == -1))
			{
				// Format is HH
				dicomTime = String.Format("{0:00}", _hour);
			}
			else if (_second == -1)
			{
				// Format is HHMM
				dicomTime = String.Format("{0:00}{1:00}", _hour, _minute);
			}
			else
			{
				// Format is HHMMSS
				dicomTime = String.Format("{0:00}{1:00}{2:00}", _hour, _minute, _second);
			}

			return dicomTime;
		}

		/// <summary>
		/// Convert from DICOM format to Common Data Format.
		/// </summary>
		/// <param name="dicomFormat">DICOM format.</param>
		public override void FromDicomFormat(System.String dicomFormat)
		{
			// Get the hour
			if (dicomFormat.Length >= 2)
			{
				_hour = int.Parse(dicomFormat.Substring(0, 2));
				_minute = -1;
				_second = -1;
			}

			// Get the minute
			if (dicomFormat.Length >= 4)
			{
				_minute = int.Parse(dicomFormat.Substring(2, 2));
				_second = -1;
			}

			// Get the second
			if (dicomFormat.Length >= 6)
			{
				_second = int.Parse(dicomFormat.Substring(4, 2));
			}
		}

		/// <summary>
		/// Convert from Common Data Format to HL7 format.
		/// </summary>
		/// <returns>HL7 format.</returns>
		public override System.String ToHl7Format()
		{
			System.String hl7Time = System.String.Empty;

			// Check on parts of the time that are defined
			// - we will explicitly not check for a valid time.
			if ((_hour == -1) &&
				(_minute == -1) &&
				(_second == -1))
			{
				// return some initial time
				hl7Time = System.String.Empty;
			}
			else if ((_minute == -1) &&
				(_second == -1))
			{
				// Format is HH
				hl7Time = String.Format("{0:00}", _hour);
			}
			else if (_second == -1)
			{
				// Format is HHMM
				hl7Time = String.Format("{0:00}{1:00}", _hour, _minute);
			}
			else
			{
				// Format is HHMMSS
				hl7Time = String.Format("{0:00}{1:00}{2:00}", _hour, _minute, _second);
			}

			return hl7Time;
		}

		/// <summary>
		/// Convert from HL7 format to Common Data Format.
		/// </summary>
		/// <param name="hl7Format">HL7 format.</param>
		public override void FromHl7Format(System.String hl7Format)
		{
			// Date/Time = yyyymmddHHMMSS
			// Get the hour
			if (hl7Format.Length >= 10)
			{
				_hour = int.Parse(hl7Format.Substring(8, 2));
				_minute = -1;
				_second = -1;
			}

			// Get the minute
			if (hl7Format.Length >= 12)
			{
				_minute = int.Parse(hl7Format.Substring(10, 2));
				_second = -1;
			}

			// Get the second
			if (hl7Format.Length >= 14)
			{
				_second = int.Parse(hl7Format.Substring(12, 2));
			}
		}

		/// <summary>
		/// Check if the objects are equal.
		/// </summary>
		/// <param name="obj">Comparison object.</param>
		/// <returns>bool indicating true = equal or false  = not equal.</returns>
		public override bool Equals(object obj)
		{
			bool equals = false;
			if (obj is CommonTimeFormat)
			{
				CommonTimeFormat thatCommonTime = (CommonTimeFormat) obj;
				if ((this.Hour == thatCommonTime.Hour) &&
					(this.Minute == thatCommonTime.Minute) &&
					(this.Second == thatCommonTime.Second))
					equals = true;
			}
			return equals;
		}

		/// <summary>
		/// Get HashCode.
		/// </summary>
		/// <returns>Base HashCode value.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Console Display the Common Data Format content - for debugging purposes.
		/// </summary>
		public override void ConsoleDisplay()
		{
			Console.WriteLine("CommonTimeFormat...");
			Console.WriteLine("Hour: \"{0}\"", _hour);
			Console.WriteLine("Minute: \"{0}\"", _minute);
			Console.WriteLine("Second: \"{0}\"", _second);
			Console.WriteLine("DicomTimeFormat...");
			Console.WriteLine("Time: \"{0}\"", this.ToDicomFormat());
			Console.WriteLine("Hl7TimeFormat...");
			Console.WriteLine("Time: \"{0}\"", this.ToHl7Format());
		}	
		#endregion

		#region properties
		/// <summary>
		/// Hour Property
		/// </summary>
		public int Hour
		{
			set
			{
				_hour = value;
			}
			get
			{
				return _hour;
			}
		}

		/// <summary>
		/// Minute Property
		/// </summary>
		public int Minute
		{
			set
			{
				_minute = value;
			}
			get
			{
				return _minute;
			}
		}

		/// <summary>
		/// Second Property
		/// </summary>
		public int Second
		{
			set
			{
				_second = value;
			}
			get
			{
				return _second;
			}
		}
		#endregion
	}
}
