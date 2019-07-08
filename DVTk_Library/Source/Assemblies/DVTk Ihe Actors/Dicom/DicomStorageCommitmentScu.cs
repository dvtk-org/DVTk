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
using System.Collections;

using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Dicom.Other;
using Dvtk.DvtkDicomEmulators.Bases;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Actors;
using VR = DvtkData.Dimse.VR;

namespace Dvtk.IheActors.Dicom
{
    /// <summary>
    /// Summary description of the DicomStorageCommitmentScu class.
    /// </summary>
    public class DicomStorageCommitmentScu : DicomScu
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="dicomClient"></param>
        public DicomStorageCommitmentScu(DicomClient dicomClient) : base(dicomClient) { }

        //
        // - Methods -
        //		
        protected override void ProcessTrigger(Object trigger)
        {
            if (trigger is SendAssociationTrigger)
            {
                SendAssociationTrigger sendAssociationTrigger = trigger as SendAssociationTrigger;

                try
                {
                    sendAssociationTrigger.returnValue = SendAssociationAndWait(sendAssociationTrigger.dicomMessageCollection, sendAssociationTrigger.presentationContexts);
                }
                catch (Exception exception)
                {
                    System.String message = "ProcessTrigger failure - unknown reason.";

                    // If no messages have been sent then assume a connection problem
                    if (Messages.Count == 0)
                    {
                        message = "Connection failure - message cannot be sent.";
                    }
                    else
                    {
                        foreach (DulMessage dulMessage in Messages.DulMessages)
                        {
                            if (dulMessage.IsAbort == true)
                            {
                                // Found ABORT message - reason for failure?
                                message = "DULP ABORT-RQ received.";
                                break;
                            }
                        }
                    }

                    Exception newException = new Exception(message, exception);

                    sendAssociationTrigger.returnValue = false;

                    //throw newException;
                }
            }
        }

        private bool SendAssociationAndWait(DicomMessageCollection dicomMessages, params PresentationContext[] presentationContexts)
        {
            bool isAssociationAccepted = true;

            // Send the associate request.
            SendAssociateRq(presentationContexts);

            // Receive the associate repsonse (may be an accept or reject).
            DulMessage associateRp = ReceiveAssociateRp();
            try
            {
                // If an associate accept was received, send the collection of DicomMessages, receive a response and
                // release the association.
                if (associateRp is AssociateAc)
                {
                    // send each message
                    foreach (DicomMessage dicomMessage in dicomMessages)
                    {
                        Send(dicomMessage);

                        DicomMessage responseMessage = ReceiveDicomMessage();

                        // use the common config option 1 to determine how the storage commitment should be handled
                        if (DicomClient.ConfigOption1.Equals("DO_STORAGE_COMMITMENT_ON_SINGLE_ASSOCIATION") == true)
                        {
                            DicomMessage dicomRequest = ReceiveDicomMessage();

                            // Validate the received message
                            //                        System.String iodName = GetIodNameFromDefinition(dicomRequest);
                            //                        Validate(dicomRequest, iodName);

                            // set up the default N-EVENT-REPORT-RSP with a successful status
                            responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.NEVENTREPORTRSP);
                            responseMessage.Set("0x00000900", VR.US, 0);

                            // send the response
                            Send(responseMessage);
                        }
                    }

                    SendReleaseRq();

                    ReceiveReleaseRp();
                }
                else
                {
                    isAssociationAccepted = false;
                }
            }
            catch (Exception e)
            {
                SendReleaseRq();
                ReceiveReleaseRp();
                throw e;
            }

            return isAssociationAccepted;
        }
    }

}
