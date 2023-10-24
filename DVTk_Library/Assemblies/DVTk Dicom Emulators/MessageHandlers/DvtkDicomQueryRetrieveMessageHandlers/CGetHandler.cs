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
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;

namespace Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers
{
    /// <summary>
    /// Summary description for CGetHandler.
    /// </summary>
    public class CGetHandler : QueryRetrieveHandler
    {
        public CGetHandler() { }

        public override bool HandleCGetRequest(DicomMessage dicomMessage)
        {
            // Validate the received message
            //System.String iodName = DicomThread.GetIodNameFromDefinition(dicomMessage);
            //DicomThread.Validate(dicomMessage, iodName);

            // Storage suboperations should be sent over the same association as the CGET.
            // The CGET SCU becomes a Storage SCP for the duration of the storage suboperations.
            // The CGET SCU should ensure that all the necessary storage SOP Classes are negotiated
            // during association establishment.

            DicomMessage responseMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CGETRSP);
            this.Send(responseMessage);
            return true;
        }
    }
}
