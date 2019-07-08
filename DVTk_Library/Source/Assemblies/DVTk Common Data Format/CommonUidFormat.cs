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
	/// Summary description for CommonUidFormat.
	/// </summary>
	public class CommonUidFormat : BaseCommonDataFormat
	{
		private System.String _uid = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public CommonUidFormat() {}

		#region base class overrides
		/// <summary>
		/// Convert from Common Data Format to DICOM format.
		/// </summary>
		/// <returns>DICOM format.</returns>
		public override System.String ToDicomFormat()
		{
			return _uid;
		}

		/// <summary>
		/// Convert from DICOM format to Common Data Format.
		/// </summary>
		/// <param name="dicomFormat">DICOM format.</param>
		public override void FromDicomFormat(System.String dicomFormat)
		{
			_uid = dicomFormat.Trim();
		}

		/// <summary>
		/// Convert from Common Data Format to HL7 format.
		/// </summary>
		/// <returns>HL7 format.</returns>
		public override System.String ToHl7Format()
		{
			return _uid;
		}

		/// <summary>
		/// Convert from HL7 format to Common Data Format.
		/// </summary>
		/// <param name="hl7Format">HL7 format.</param>
		public override void FromHl7Format(System.String hl7Format)
		{
			_uid = hl7Format.Trim();
		}

		/// <summary>
		/// Check if the objects are equal.
		/// </summary>
		/// <param name="obj">Comparison object.</param>
		/// <returns>bool indicating true = equal or false  = not equal.</returns>
		public override bool Equals(object obj)
		{
			bool equals = false;
			if (obj is CommonUidFormat)
			{
				CommonUidFormat thatCommonUid = (CommonUidFormat) obj;
				if (this.Uid == thatCommonUid.Uid)
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
			Console.WriteLine("CommonUidFormat...");
			Console.WriteLine("Uid: \"{0}\"", _uid);
			Console.WriteLine("DicomUidFormat...");
			Console.WriteLine("Uid: \"{0}\"", this.ToDicomFormat());
			Console.WriteLine("Hl7UidFormat...");
			Console.WriteLine("Uid: \"{0}\"", this.ToHl7Format());
		}
		#endregion

		#region properties
		/// <summary>
		/// Uid Property
		/// </summary>
		public System.String Uid
		{
			set
			{
				_uid = value.Trim();
			}
			get
			{
				return _uid;
			}
		}
		#endregion
	}
}
