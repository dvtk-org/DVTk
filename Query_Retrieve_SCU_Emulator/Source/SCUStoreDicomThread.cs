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
using DIMSE = DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Common.Threads;
//using DvtkData.Dimse;
using System.Collections;
using System.Collections.Generic;

namespace QR_SCU_Emulator
{
    using HLI = DvtkHighLevelInterface.Dicom.Other;

    public class SCUStoreDicomThread : ConcurrentMessageIterator
    {
        private ArrayList supportedTS = new ArrayList();
   
        /// <summary>
		/// Class constructor.
		/// </summary>
        public SCUStoreDicomThread(String identifierBasisChildThreads)
                : base(identifierBasisChildThreads)
		{
            supportedTS.Add("1.2.840.10008.1.2");
            supportedTS.Add("1.2.840.10008.1.2.1");
            supportedTS.Add("1.2.840.10008.1.2.2");
            supportedTS.Add("1.2.840.10008.1.2.4.70");
            supportedTS.Add("1.2.840.10008.1.2.4.50");
            supportedTS.Add("1.2.840.10008.1.2.5");
		}

        /// <summary>
        /// Handle Association Req
        /// </summary>
        /// <param name="associateRq"></param>
        public override void AfterHandlingAssociateRequest(AssociateRq associateRq)
        {
            if (!IsMessageHandled)
            {
                string [] tsList = (System.String[])supportedTS.ToArray(typeof(System.String));
                SendAssociateRp(new TransferSyntaxes(tsList));
                IsMessageHandled = true;
            }
        }

        /// <summary>
        /// Method to handle the workflow after receiving a Release Request.
        /// </summary>
        /// <param name="releaseRq">Release Request message.</param>
        public override void AfterHandlingReleaseRequest(ReleaseRq releaseRq)
        {
            if (IsMessageHandled == false)
            {
                // send a release response
                SendReleaseRp();

                // message has now been handled
                IsMessageHandled = true;
            }
        }

        /// <summary>
        /// Method to handle the workflow after receiving a C-EHO-RQ.
        /// </summary>
        /// <param name="dicomMessage">C-ECHO-RQ message.</param>
        protected override void AfterHandlingCEchoRequest(DicomMessage dicomMessage)
        {
            if (IsMessageHandled == false)
            {
                DicomMessage dicomMessageToSend = new DicomMessage(DvtkData.Dimse.DimseCommand.CECHORSP);

                dicomMessageToSend.Set("0x00000002", DvtkData.Dimse.VR.UI, "1.2.840.10008.1.1");
                dicomMessageToSend.Set("0x00000900", DvtkData.Dimse.VR.US, 0);

                Send(dicomMessageToSend);

                // message has now been handled
                IsMessageHandled = true;
            }
        }

        /// <summary>
        /// Overridden C-STORE-RQ message handler.
        /// </summary>
        /// <param name="dicomMessage">C-STORE-RQ and Dataset.</param>
        /// <returns>Boolean - true if dicomMessage handled here.</returns>
        protected override void AfterHandlingCStoreRequest(DicomMessage dicomMessage)
        {
            // set up the default C-STORE-RSP with a successful status
            DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERSP);
            responseMessage.Set("0x00000900", DvtkData.Dimse.VR.US, 0);

            // send the response
            this.Send(responseMessage);

            // message handled
            IsMessageHandled = true;
        }

        /// <summary>
        /// Handle Abort Rq
        /// </summary>
        /// <param name="abort"></param>
        public override void AfterHandlingAbort(Abort abort)
        {
            if (!IsMessageHandled)
            {
                StopResultsGathering();
                StartResultsGathering();

                IsMessageHandled = true;
            }
        }

        protected override void DisplayException(System.Exception exception)
        {
            string error = "Error in C-Store message handling due to exception:" + exception.Message;
            WriteError(error);
        }
    }
}