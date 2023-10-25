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
	/// Summary description for DicomTrigger.
	/// </summary>
	public class DicomTrigger : BaseTrigger
	{
		private DicomTriggerItemCollection _triggerItems = new DicomTriggerItemCollection();
		private bool _handleInSingleAssociation = true;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="triggerName">Trigger Name</param>
		public DicomTrigger(TransactionNameEnum triggerName) : base(triggerName) {}

		/// <summary>
		/// Add the trigger item details to the trigger.
		/// </summary>
		/// <param name="message">DICOM Message to be triggered.</param>
		/// <param name="sopClassUid">SOP Class UID of DICOM Message.</param>
		/// <param name="transferSyntax">Transfer Syntax of DICOM Message</param>
		public void AddItem(DicomMessage message, System.String sopClassUid, System.String transferSyntax)
		{
			System.String[] transferSyntaxes = new System.String[1];
			transferSyntaxes[0] = transferSyntax;

			AddItem(message, sopClassUid, transferSyntaxes);
		}

		/// <summary>
		/// Add the trigger item details to the trigger.
		/// </summary>
		/// <param name="message">DICOM Message to be triggered.</param>
		/// <param name="sopClassUid">SOP Class UID of DICOM Message.</param>
        /// <param name="transferSyntaxes">Transfer Syntaxes of DICOM Message</param>
		public void AddItem(DicomMessage message, System.String sopClassUid, System.String[] transferSyntaxes)
		{
			DicomTriggerItem triggerItem = new DicomTriggerItem(message, sopClassUid, transferSyntaxes);
			
			_triggerItems.Add(triggerItem);
		}

		/// <summary>
		/// Property - TriggerItems.
		/// </summary>
		public DicomTriggerItemCollection TriggerItems
		{
			get 
			{ 
				return _triggerItems; 
			}
		}

		/// <summary>
		/// Property - HandleInSingleAssociation
		/// </summary>
		public bool HandleInSingleAssociation
		{
			get
			{
				return _handleInSingleAssociation;
			}
			set
			{
				_handleInSingleAssociation = value;
			}
		}
	}
}
