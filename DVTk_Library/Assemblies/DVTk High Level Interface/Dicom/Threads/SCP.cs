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
using DvtkHighLevelInterface.Dicom.Other;
using VR = DvtkData.Dimse.VR;



namespace DvtkHighLevelInterface.Dicom.Threads
{
	/// <summary>
	/// Summary description for SCP.
	/// </summary>
    [Obsolete("This class was only intended as an example of a MessageIterator derived class. Please use the MessageIterator class instead.")]
	public class SCP: MessageIterator
	{
        /// <summary>
        /// This class was only intended as an example of a MessageIterator derived class. Please use the MessageIterator class instead.
        /// </summary>
        /// <param name="associateRq">-</param>
		public override void AfterHandlingAssociateRequest(AssociateRq associateRq)
		{
			if (!IsMessageHandled)
			{
		        SendAssociateAc(new TransferSyntaxes("1.2.840.10008.1.2", "1.2.840.10008.1.2.1", "1.2.840.10008.1.2.2"));
				IsMessageHandled = true;
			}
		}

		private bool resultsFilePerAssociation = false;

        /// <summary>
        /// This class was only intended as an example of a MessageIterator derived class. Please use the MessageIterator class instead.
        /// </summary>
		public bool ResultsFilePerAssociation
		{
			get
			{
				return(this.resultsFilePerAssociation);
			}
			set
			{
				this.resultsFilePerAssociation = value;
			}
		}

        /// <summary>
        /// This class was only intended as an example of a MessageIterator derived class. Please use the MessageIterator class instead.
        /// </summary>
        /// <param name="releaseRq">-</param>
		public override void AfterHandlingReleaseRequest(ReleaseRq releaseRq)
		{
			if (!IsMessageHandled)
			{
				SendReleaseRp();

				if (this.resultsFilePerAssociation)
				{
					StopResultsGathering();
					StartResultsGathering();
				}

				IsMessageHandled = true;
			}			
		}

        /// <summary>
        /// This class was only intended as an example of a MessageIterator derived class. Please use the MessageIterator class instead.
        /// </summary>
        /// <param name="dicomMessage">-</param>
		protected override void AfterHandlingCEchoRequest(DicomMessage dicomMessage)
		{
			if (!IsMessageHandled)
			{
				DicomMessage dicomMessageToSend = new DicomMessage(DvtkData.Dimse.DimseCommand.CECHORSP);

				dicomMessageToSend.Set("0x00000002", VR.UI, "1.2.840.10008.1.1");
				dicomMessageToSend.Set("0x00000900", VR.US, 0);

				Send(dicomMessageToSend);
	
				IsMessageHandled = true;
			}
		}

		private bool handleCEchoRequest = true;

        /// <summary>
        /// This class was only intended as an example of a MessageIterator derived class. Please use the MessageIterator class instead.
        /// </summary>
		public bool HandleCEchoRequest
		{
			get
			{
				return(this.handleCEchoRequest);
			}
			set
			{
				this.handleCEchoRequest = value;
			}
		}
	}
}
