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
using System.Windows.Forms;
using System.IO;
using System.Collections;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using Dvtk.Sessions;

namespace SnifferUI
{
	/// <summary>
	/// Summary description for HLI Main Thread.
	/// </summary>
	public class HLIThread : DicomThread
	{
		private SnifferSession dvtkSnifferSession = null;
		private ArrayList connectionList = null;
		DICOMSniffer snifferObj = null;

        public HLIThread(DICOMSniffer mainObj, SnifferSession snifferSession, ArrayList list)
		{
			snifferObj = mainObj;
			dvtkSnifferSession = snifferSession;
			connectionList = list;
		}

		protected override void Execute()
		{
			// loop through all associations
			foreach( object connection in connectionList)
			{
				// process a single association
				processPDUFiles((string)connection);
			}
		}		

		public void processPDUFiles(string signature)
		{
			DirectoryInfo theDirectoryInfo = null;

            string pduDirectory = snifferObj.CurrentBaseFileName + signature + @"\PDUs";
			try
			{
				if(pduDirectory != null)
				{
					theDirectoryInfo = new DirectoryInfo(pduDirectory);
					if(theDirectoryInfo.Exists)
					{
						FileInfo[] files = theDirectoryInfo.GetFiles();

						string [] pduFileNames = null;
						if(files.Length != 0)
						{
							// PDU files list will be set from Association class during dumping of all PDU files
							// in the same order as written so we've to order the files in which order we received.
							FileInfo[] OrderedFiles = orderPDUFiles(files);

							ArrayList pduFilesList = new ArrayList();
							foreach(FileInfo pduFileInfo in OrderedFiles)
							{
								pduFilesList.Add(pduFileInfo.FullName);
							}
							pduFileNames = (string[]) pduFilesList.ToArray(typeof(string));
						}
						else
						{
							string msg = string.Format("No PDUs are saved in {0}",pduDirectory);
                            snifferObj.OutputHandler("Error: " + msg, true, true);
						}

						if(dvtkSnifferSession != null)
						{
							// Store the original data directory
							string originalDataDirectory = dvtkSnifferSession.DataDirectory;

							// Read all the PDU files in the file stream
							if(pduFileNames.Length != 0)
							{
								// Set the Data directory per association
								dvtkSnifferSession.DataDirectory = originalDataDirectory + signature;
								Directory.CreateDirectory(dvtkSnifferSession.DataDirectory);
								dvtkSnifferSession.ReadPDUsInFileStream(pduFileNames);
							}
							else
							{
								string msg = string.Format("No DICOM PDUs are captured.\n Please capture again.\n");
                                snifferObj.OutputHandler("\nWarning: " + msg, true, true);
							}
						
							// Start the child thread & set the thread options
                            ChildThread childThread = new ChildThread(snifferObj,dvtkSnifferSession, originalDataDirectory, signature);
							childThread.Initialize(this);
                            childThread.Options.ResultsDirectory = snifferObj.CurrentBaseFileName;
							childThread.Options.Identifier = signature.Replace("-","_");
							childThread.Options.ResultsFileNameOnlyWithoutExtension = childThread.Options.Identifier;
                            childThread.Options.StartAndStopResultsGatheringEnabled = true;
							childThread.Options.LogChildThreadsOverview = false;
							childThread.Options.LogThreadStartingAndStoppingInParent = false;
							childThread.Options.LogWaitingForCompletionChildThreads  = false;

                            if (!snifferObj.generateDetailedValidation)
                                childThread.Options.GenerateDetailedResults = false;

							childThread.Start();
							childThread.WaitForCompletion();

							//Display results only in case of single association selected
                            if (!snifferObj.evaluateAllAssociations)
							{
                                snifferObj.summaryXmlFullFileName = childThread.Options.SummaryResultsFullFileName;
                                snifferObj.detailXmlFullFileName = childThread.Options.DetailResultsFullFileName;
							}
						}					
					}
					else
					{
						string msg = string.Format("No DICOM PDUs are captured.\n The tool may be running on localhost.\r\n");
                        snifferObj.OutputHandler("\nError: " + msg, true, true);
					}
				}
				else
				{
					string msg = string.Format("No DICOM PDUs are captured.\n The tool may be running on localhost.\r\n");
                    snifferObj.OutputHandler("\nError: " + msg, true, true);
				}
			}
			catch (Exception except) 
			{
				string msg = string.Format("Exception:{0}\n", except.Message);
				MessageBox.Show(msg, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error );				
			}				
		}

		private FileInfo[] orderPDUFiles(FileInfo[] files)
		{
			FileInfo[] orderedFileArray = new FileInfo[files.Length];
			foreach(FileInfo pduFile in files)
			{
				string pduFileName = pduFile.Name;
				int index = pduFileName.LastIndexOf("_");
				int pduNr = 0;
				if(index != -1)
					pduNr = int.Parse(pduFileName.Substring(0,index));

				orderedFileArray.SetValue(pduFile,pduNr);
			}

			return orderedFileArray;
		}
	}

	/// <summary>
	/// Summary description for HLI Child Thread.
	/// </summary>
	public class ChildThread : DicomThread
	{
		private SnifferSession dvtkSnifferSession = null;
		DICOMSniffer snifferObj = null;
		string orgDataDir = "";
		string associationName = "";
        public ChildThread(DICOMSniffer mainObj, SnifferSession snifferSession, string dataDir, string signature)
		{
			snifferObj = mainObj;
			dvtkSnifferSession = snifferSession;
			orgDataDir = dataDir;
			associationName = signature;
		}

		protected override void Execute()
		{
			// loop through all associations
			// Get all kind of DICOM messages from the sniffer file stream.
			DvtkData.Message message = null;
            DvtkData.Dimse.DicomMessage lastMessage = null;
            DvtkData.Dul.AcceptedPresentationContextList accPCs = null;
			ReceivedMsgReturnCode retcode = ReceivedMsgReturnCode.Failure;
			retcode = dvtkSnifferSession.ReceiveMessage(out message);
			try
			{
				while((retcode == ReceivedMsgReturnCode.Success) && (message != null))
				{
					// DicomMessage needs to be validated against a definition file and VR.
					ValidationControlFlags validationFlags = ValidationControlFlags.None;
					validationFlags |=	ValidationControlFlags.UseDefinitions;
					validationFlags |=	ValidationControlFlags.UseValueRepresentations;
		
					if (message is DvtkData.Dul.DulMessage)
					{
						//Extract Calling/Called AE Title from received message and save is to 
						// Dvtk Script session
						if(message is DvtkData.Dul.A_ASSOCIATE_RQ)
						{
							DvtkData.Dul.A_ASSOCIATE_RQ assocReq = (DvtkData.Dul.A_ASSOCIATE_RQ)message;
							Options.DvtkScriptSession.DvtSystemSettings.AeTitle = assocReq.CalledAETitle;
							Options.DvtkScriptSession.SutSystemSettings.AeTitle = assocReq.CallingAETitle;
						}

						if(message is DvtkData.Dul.A_ASSOCIATE_AC)
						{
							DvtkData.Dul.A_ASSOCIATE_AC assocAcc = (DvtkData.Dul.A_ASSOCIATE_AC)message;
							Options.DvtkScriptSession.DvtSystemSettings.AeTitle = assocAcc.CallingAETitle;
							Options.DvtkScriptSession.SutSystemSettings.AeTitle = assocAcc.CalledAETitle;
                            accPCs = assocAcc.PresentationContexts;                            
						}

						Options.DvtkScriptSession.Validate(message as DvtkData.Dul.DulMessage,null,validationFlags);
					}
					else
					{
                        DvtkData.Dimse.DicomMessage dimseMsg = (DvtkData.Dimse.DicomMessage)message;
                        if (dimseMsg.CommandField == DvtkData.Dimse.DimseCommand.CFINDRQ)
                            lastMessage = dimseMsg;

                        foreach (DvtkData.Dul.AcceptedPresentationContext acc in accPCs)
                        {
                            if ((acc.Result == 0) && (acc.ID == dimseMsg.EncodedPresentationContextID))
                            {
                                if (acc.TransferSyntax != DvtkData.Dul.TransferSyntax.Implicit_VR_Little_Endian)
                                    Options.DvtkScriptSession.IsDataTransferExplicit = true;
                                else
                                    Options.DvtkScriptSession.IsDataTransferExplicit = false;
                                break;
                            }
                        }

                        if ((dimseMsg.CommandField == DvtkData.Dimse.DimseCommand.CFINDRSP) && (lastMessage != null))
                        {
                            Options.DvtkScriptSession.Validate(dimseMsg, null, lastMessage, validationFlags);
                        }
                        else
                        {
                            Options.DvtkScriptSession.Validate(dimseMsg, null, validationFlags);
                        }
					}					

					message = null;
					retcode = dvtkSnifferSession.ReceiveMessage(out message);
					if((retcode == ReceivedMsgReturnCode.IncompleteByteStream) && (message == null))
					{
                        string msg = string.Format("Incomplete byte stream, unable to perform further validation, see {0} for detail logging in {1} directory.", (associationName+".log"),this.Options.ResultsDirectory);
						WriteHtmlInformation("<b><br />");
						WriteInformation(msg);
						WriteHtmlInformation("</b><br />");
					}
				}
			}
			catch (Exception except) 
			{
				string msg = string.Format("Validation error: {0}\n", except.Message);
				MessageBox.Show(msg, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error );				
			}

			//again set the original directory
			dvtkSnifferSession.DataDirectory = orgDataDir;
		}		
	}
}
