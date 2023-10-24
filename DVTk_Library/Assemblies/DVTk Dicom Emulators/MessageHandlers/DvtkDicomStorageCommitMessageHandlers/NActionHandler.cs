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
using DvtkHighLevelInterface.Dicom.Threads;
using VR = DvtkData.Dimse.VR;

namespace Dvtk.DvtkDicomEmulators.StorageCommitMessageHandlers
{
    /// <summary>
    /// Summary description for NActionHandler.
    /// </summary>
    public class NActionHandler : MessageHandler
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public NActionHandler() { }

        /// <summary>
        /// Overridden N-ACTION-RQ message handler.
        /// </summary>
        /// <param name="dicomMessage">N-ACTION-RQ and Dataset.</param>
        /// <returns>Boolean - true if dicomMessage handled here.</returns>
        public override bool HandleNActionRequest(DicomMessage dicomMessage)
        {
            // Validate the received message
            //			System.String iodName = DicomThread.GetIodNameFromDefinition(dicomMessage);
            //			DicomThread.Validate(dicomMessage, iodName);

            // set up the default N-ACTION-RSP with a successful status
            DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.NACTIONRSP);
            responseMessage.Set("0x00000900", VR.US, 0);

            // send the response
            this.Send(responseMessage);

            // message handled
            return true;
        }
    }
}
