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

namespace Dvtk.DvtkDicomEmulators.WorklistMessageHandlers
{
	/// <summary>
	/// Summary description for CFindHandler.
	/// </summary>
	public class CFindHandler : MessageHandler
	{
		private ModalityWorklistInformationModel _modalityWorklistInformationModel = new ModalityWorklistInformationModel();

		/// <summary>
		/// Class Constructor
		/// </summary>
		public CFindHandler(ModalityWorklistInformationModel modalityWorklistInformationModel)
		{
            _modalityWorklistInformationModel = modalityWorklistInformationModel;
		}

		/// <summary>
		/// Overridden C-FIND-RQ message handler that makes use of the appropriate Information Model to handle the query.
		/// </summary>
		/// <param name="queryMessage">C-FIND-RQ Identifier (Dataset) containing query attributes.</param>
		/// <returns>Boolean - true if dicomMessage handled here.</returns>
		public override bool HandleCFindRequest(DicomMessage queryMessage)
		{
			// Validate the received message
			//System.String iodName = DicomThread.GetIodNameFromDefinition(queryMessage);
			//DicomThread.Validate(queryMessage, iodName);

			// perform query
			DicomMessageCollection responseMessages = _modalityWorklistInformationModel.QueryInformationModel(queryMessage);

			if(responseMessages.Count > 1)
			{
				WriteInformation(string.Format("Sending {0} C-FIND responses after performing query.\r\n",responseMessages.Count));
			}
			else
			{
				WriteWarning("No response from MWL information model after performing query.\r\n");
			}

			// handle responses
			foreach (DicomMessage responseMessage in responseMessages)
			{
                int waitedTime = 0;

                // Check for cancel message from SCU
                if (WaitForPendingDataInNetworkInputBuffer(100, ref waitedTime))
                {
                    DicomMessage cancelRq = ReceiveDicomMessage();

                    if (cancelRq.CommandSet.DimseCommand == DvtkData.Dimse.DimseCommand.CCANCELRQ)
                    {
                        // set up the C-FIND-RSP with cancel status
                        DicomMessage respMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CFINDRSP);
                        respMessage.Set("0x00000900", DvtkData.Dimse.VR.US, 0xFE00);

                        // send the response
                        this.Send(respMessage);
                        break;
                    }
                }

				this.Send(responseMessage);
			}

			// message handled
			return true;
		}
	}
}
