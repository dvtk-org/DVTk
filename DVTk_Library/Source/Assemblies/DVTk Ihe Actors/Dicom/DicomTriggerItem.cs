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

using DvtkHighLevelInterface.Dicom.Messages;
using Dvtk.IheActors.Bases;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomTriggerItem.
	/// </summary>
	public class DicomTriggerItem
	{
		private DicomMessage _message = null;
		private System.String _sopClassUid = System.String.Empty;
		private System.String[] _transferSyntaxes = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="message">DICOM Message to be triggered.</param>
		/// <param name="sopClassUid">SOP Class UID of DICOM Message.</param>
        /// <param name="transferSyntaxes">Transfer Syntaxes of DICOM Message</param>
		public DicomTriggerItem(DicomMessage message, System.String sopClassUid, System.String[] transferSyntaxes)
		{
			_message = message;
			_sopClassUid = sopClassUid;
			_transferSyntaxes = new System.String[transferSyntaxes.Length];
			_transferSyntaxes = transferSyntaxes;
		}

		/// <summary>
		/// Property - Message.
		/// </summary>
		public DicomMessage Message
		{
			get 
			{ 
				return _message; 
			}
			set
			{
				_message = value;
			}
		} 

		/// <summary>
		/// Property - SOP Class UID / Abstract Syntax Name.
		/// </summary>
		public System.String SopClassUid
		{
			get
			{
				return _sopClassUid;
			}
			set
			{
				_sopClassUid = value;
			}
		}

		/// <summary>
		/// Property - Transfer Syntaxes UID.
		/// </summary>
		public System.String[] TransferSyntaxes
		{
			get
			{
				return _transferSyntaxes;
			}
			set
			{
				_transferSyntaxes = value;
			}
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object
		/// </summary>
		/// <param name="obj">An <see cref="object"/> to compare with this instance, or a <see langword="null"/> reference.</param>
        /// <returns><see langword="true"/> if other is an instance of <see cref="DicomTriggerItem"/> and equals the value of this instance; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(System.Object obj) 
		{
			// Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			DicomTriggerItem triggerItem = (DicomTriggerItem)obj;

			// Only interested in comparing the SOP Class UID and Transfer Syntaxes
			if (triggerItem.SopClassUid != this.SopClassUid) return false;
			if (triggerItem.TransferSyntaxes.Length != this.TransferSyntaxes.Length) return false;

			// The transfer syntaxes may be defined in a different order
			bool equal = true;
			for (int i = 0; i < triggerItem.TransferSyntaxes.Length; i++)
			{
				bool matchFound = false;
				for (int j = 0; j < this.TransferSyntaxes.Length; j++)
				{
					if (triggerItem.TransferSyntaxes[i] == this.TransferSyntaxes[j])
					{
						matchFound = true;
						break;
					}
				}
				if (matchFound == false)
				{
					equal = false;
					break;
				}
			}
			return equal;
		}
	}
}
