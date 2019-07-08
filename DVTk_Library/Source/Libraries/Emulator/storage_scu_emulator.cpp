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

//*****************************************************************************
//  DESCRIPTION     :	Storage SCU emulator class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "storage_scu_emulator.h"
#include "commitment.h"
#include "Idefinition.h"			// Definition component interface
#include "Imedia.h"					// Media component interface
#include "Isession.h"				// Session component interface
#include "Ivalidation.h"			// Validation component interface

#include <time.h>

//>>===========================================================================

STORAGE_SCU_EMULATOR_CLASS::STORAGE_SCU_EMULATOR_CLASS(EMULATOR_SESSION_CLASS *session_ptr)

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sessionM_ptr = session_ptr;
	autoType2AttributesM = false;
	defineSqLengthM = false;
	addGroupLengthM = false;

	LOG_CLASS *logger_ptr = session_ptr->getLogger();
	if (logger_ptr)
	{
		// enable the base level logging
		UINT32 logMask = logger_ptr->getLogMask();
		logMask |= (LOG_NONE | LOG_SCRIPT | LOG_MEDIA_FILENAME);
		logger_ptr->setLogMask(logMask);
	}
	setLogger(logger_ptr);
    setSerializer(session_ptr->getSerializer());

	associationM.createSocket(session_ptr->getSocketParameters());

	// set default options
	optionsM = 0;
	optionsM |= SCU_STORAGE_EML_OPTION_ASSOC_MULTI | SCU_STORAGE_EML_OPTION_VALIDATE_ON_IMPORT;
	nr_repetitionsM = 1;
	message_idM = 316;  // magic number, totally arbitrary
	associatedM = false;
}

//>>===========================================================================

STORAGE_SCU_EMULATOR_CLASS::~STORAGE_SCU_EMULATOR_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	//cleanup();
}

//>>===========================================================================

bool STORAGE_SCU_EMULATOR_CLASS::emulate()

//  DESCRIPTION     : Start emulation
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// set the storage root as the data directory
    if (loggerM_ptr)
    {
    	loggerM_ptr->setStorageRoot(sessionM_ptr->getDataDirectory());
    }

	// import files we only do this once
	vector<string>::iterator it;
	for (it = filenamesM.begin(); it < filenamesM.end(); ++it)
	{
		//Check for export abort
		if (sessionM_ptr->isEmulationAborted())
		{
			if (loggerM_ptr)
            {
    			loggerM_ptr->text(LOG_INFO, 1, "Export operation of DICOM data is aborted from SCU.");
            }
			return true;
		}

        // successful reads are stored in filedatasetsM
		importFile(*it);
	}

	// check if we can export the files
    if (filedatasetsM.size() != 0)
	{
		// loop sending the files for the count required
		for (UINT i = 0; i < nr_repetitionsM; ++i)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_INFO, 3, "Starting iteration %d of %d...", i + 1, nr_repetitionsM);
			}

			// do we send all files within 1 or more associations
			if (optionsM & SCU_STORAGE_EML_OPTION_ASSOC_SINGLE)
			{
				// send all files in a single association
				result = sendFilesInSingleAssociation(i);
			}
			else
			{
				// send files in multiple associations
				result = sendFilesInMultipleAssociations(i);
			}
			if (!result) break;
		}
	}

	// clean up resources
	cleanup();

	// return emulation result
	return result;
}

//>>===========================================================================

bool STORAGE_SCU_EMULATOR_CLASS::sendFilesInMultipleAssociations(int repetition_index)

//  DESCRIPTION     : Perform a verification SOP Class association/send/release
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;
	MEDIA_FILE_HEADER_CLASS *fmi_ptr = NULL;
	DCM_DATASET_CLASS *dat_ptr = NULL;
	string filename;
    string transferSyntax;
	string sop_uid;

	// process all file datasets one by one
	vector<FMI_DATASET_STRUCT>::iterator it; 
	for (it = filedatasetsM.begin(); it < filedatasetsM.end(); ++it)
	{
		filename = (*it).filename;
        transferSyntax = (*it).transferSyntax;
		fmi_ptr = (*it).fmi_ptr;
		dat_ptr = (*it).dat_ptr;

        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_SCRIPT, 1, "Will try to send Dataset stored in: \"%s\"", filename.c_str());
        }

		// cascade logger
		dat_ptr->setLogger(loggerM_ptr);

		// if fmi present try to find the sop class from fmi
		if (fmi_ptr)
		{
			result = fmi_ptr->getUIValue(TAG_MEDIA_STORAGE_SOP_CLASS_UID, sop_uid);
		}
		else
		{
			// try to find sop class uid from dataset
			result = dat_ptr->getUIValue(TAG_SOP_CLASS_UID, sop_uid);
		}

		if ((!result) &&
            (loggerM_ptr))
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't determine SOP Class UID from file: %s", filename.c_str());
		}

		if (result)
		{
			// cascade logger
			associationM.setLogger(loggerM_ptr);

            //
            // As we are sending images - it is safe to assume that the SUT is the Accepter
            // and DVT is the Requester.
            //
	        associationM.setCallingAeTitle(sessionM_ptr->getDvtAeTitle());
	        associationM.setCalledAeTitle(sessionM_ptr->getSutAeTitle());
	        associationM.setMaximumLengthReceived(sessionM_ptr->getDvtMaximumLengthReceived());
	        associationM.setImplementationClassUid(sessionM_ptr->getDvtImplementationClassUid());
	        associationM.setImplementationVersionName(sessionM_ptr->getDvtImplementationVersionName());

			// if the file transfer syntax is ile, ele or ebe then we can convert to an other transfer syntax if need be
			// - so propose all three and see what is then supported by the SCP
			//if ((transferSyntax == IMPLICIT_VR_LITTLE_ENDIAN) ||
			//	(transferSyntax == EXPLICIT_VR_LITTLE_ENDIAN) ||
			//	(transferSyntax == EXPLICIT_VR_BIG_ENDIAN))
			//{
			//	// add this sop class to the presentation contexts
			//	if (loggerM_ptr)
			//	{
			//		loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), IMPLICIT_VR_LITTLE_ENDIAN);
			//		loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), EXPLICIT_VR_LITTLE_ENDIAN);
			//		loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), EXPLICIT_VR_BIG_ENDIAN);
			//	}

			//	associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), IMPLICIT_VR_LITTLE_ENDIAN);
			//	associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), EXPLICIT_VR_LITTLE_ENDIAN);
			//	associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), EXPLICIT_VR_BIG_ENDIAN);
			//}
			//else
			//{
			if (!associationM.isSupportedPresentationContext((char*) sop_uid.c_str(), (char*) transferSyntax.c_str()))
			{
				// set presentation context
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), transferSyntax.c_str());
				}
				associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), (char*) transferSyntax.c_str());

				// add support for this sop class to the presentation contexts
				/*for (int i = 0 ; i < sessionM_ptr->noSupportedTransferSyntaxes(); i++)
				{
					char* ts = (char*) sessionM_ptr->getSupportedTransferSyntax(i);

					// set presentation context
					if (!associationM.isSupportedPresentationContext((char*) sop_uid.c_str(), ts))
					{
						if(strcmp(ts,""))
						{
							// add this sop class to the presentation contexts
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), ts);
							}
							associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), ts);
						}
					}
				}*/				
			}

			// make association
			result = associationM.makeAssociation();
			if (result)
			{
				// build store request object
				DCM_COMMAND_CLASS cmd(DIMSE_CMD_CSTORE_RQ);
				cmd.setLogger(loggerM_ptr);

				// group length is calulated on export
				cmd.setUSValue(TAG_COMMAND_FIELD, C_STORE_RQ);
				cmd.setUIValue(TAG_AFFECTED_SOP_CLASS_UID, sop_uid);

				char generated_study_uid[UI_LENGTH+1];
				char generated_series_uid[UI_LENGTH+1];
				char generated_accession_nr[SH_LENGTH+1];
				char generated_uid[UI_LENGTH+1];
				string study_instance_uid;
				string series_instance_uid;
				string accession_nr;
				string instance_uid;
						
				if (optionsM & SCU_STORAGE_EML_OPTION_DATA_UNDER_NEWSTUDY)
				{
					// All the DICOMDIR references/DCM images will be sent with  
					// new SOP/study/series instance UID & new accession nr.
					if (loggerM_ptr)
					{
        				loggerM_ptr->text(LOG_SCRIPT, 2, "Sending the DATASET under new Study");
					}

					createUID(generated_uid, (char*) sessionM_ptr->getImplementationClassUid());
					createUID(generated_study_uid, (char*) sessionM_ptr->getImplementationClassUid());
					createUID(generated_series_uid, (char*) sessionM_ptr->getImplementationClassUid());
					createSS(generated_accession_nr);

					study_instance_uid = generated_study_uid;
					series_instance_uid = generated_series_uid;
					accession_nr = generated_accession_nr;
					instance_uid = generated_uid;

					// Set the study/series instance uid & accession nr in the dataset
					dat_ptr->setUIValue(TAG_STUDY_INSTANCE_UID, study_instance_uid);
					dat_ptr->setUIValue(TAG_SERIES_INSTANCE_UID, series_instance_uid);
					dat_ptr->setSHValue(TAG_ACCESSION_NUMBER, accession_nr);					
				}
				else
				{
					// First run with unchanged UID's and second run onwards with new UID's
					if (repetition_index > 0)
					{
						// we need to generate new instance uids as we're resending the same files
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_DEBUG, 1, "Generating a new SOP Instance UID for File IOD for second run onwards...");
						}
						char generated_uid[UI_LENGTH+1];
						createUID(generated_uid, (char*) sessionM_ptr->getImplementationClassUid());
						instance_uid = generated_uid;
					}
					else
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_DEBUG, 1, "Sending the DATASET with unchanged UID's in First run...");
						}
						if (!dat_ptr->getUIValue(TAG_SOP_INSTANCE_UID, instance_uid))
						{
							if (loggerM_ptr)
							{
    							loggerM_ptr->text(LOG_WARNING, 2, "File IOD doesn't contain a SOP Instance UID! - using a generated UID for Store Command...");
							}
							char generated_uid[UI_LENGTH+1];
							createUID(generated_uid, (char*) sessionM_ptr->getImplementationClassUid());
							instance_uid = generated_uid;
						}
					}
				}

				cmd.setUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, instance_uid);
				cmd.setUSValue(TAG_MESSAGE_ID, (UINT16) ++message_idM);
				cmd.setUSValue(TAG_PRIORITY, 0);
				cmd.setUSValue(TAG_DATA_SET_TYPE, DATA_SET);

				// make sure the SOP instance UID and Ref SOP instance UID in the dataset matches the
				// affected sop instance UID in the command
				dat_ptr->setUIValue(TAG_SOP_INSTANCE_UID, instance_uid);

				// make sure the SOP instance uid in the dataset matches the
				// media storage sop instance uid in the FMI
				if((fmi_ptr) && (fmi_ptr->GetAttributeByTag(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID)) != NULL)
					fmi_ptr->setUIValue(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID, instance_uid);

				//Modify the series instance UID in Dataset
				modifyDicomObjectWithSeriesUID(dat_ptr, series_instance_uid, instance_uid);
				
				// disable the type 2 attribute addition
				dat_ptr->setPopulateWithAttributes(false);

				// log action
                if (loggerM_ptr)
                {
    				loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s for IOD with SOP Instance UID \"%s\" (%s)", mapCommandName(cmd.getCommandId()), instance_uid.c_str(), timeStamp());
                }

				// send the request
				result = associationM.send(&cmd, dat_ptr);

				// handle response
				if (result)
				{
					RECEIVE_MSG_ENUM status;
					DCM_COMMAND_CLASS *cmd_ptr = NULL;
					DCM_DATASET_CLASS *dat_ptr = NULL;
					AE_SESSION_CLASS ae_session;

					// set ae session
					ae_session.SetName(sessionM_ptr->getApplicationEntityName());
					ae_session.SetVersion(sessionM_ptr->getApplicationEntityVersion());

					status = associationM.receiveCommandDataset(&cmd_ptr, &dat_ptr, &ae_session, true, false);
					if (status == RECEIVE_MSG_SUCCESSFUL)
					{
						// log action
						if (cmd_ptr)
						{
                            if (loggerM_ptr)
                            {
    							loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", mapCommandName(cmd_ptr->getCommandId()), timeStamp());
                            }
							cmd_ptr->setLogger(loggerM_ptr);

                            // serialize it
                            if (serializerM_ptr)
                            {
                                serializerM_ptr->SerializeReceive(cmd_ptr, NULL);
                            }
						}

						// release association
						result = associationM.releaseAssociation();
						if ((!result) &&
                            (loggerM_ptr))
						{
							loggerM_ptr->text(LOG_ERROR, 1, "Failed to release association");
						}
					}
					else
					{
                        if (loggerM_ptr)
                        {
    						loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive C-STORE-RSP");
                        }
						result = false;
					}
				}
				else
				{
                    if (loggerM_ptr)
                    {
    					loggerM_ptr->text(LOG_ERROR, 1, "Failed to send C-STORE-RQ");
                    }
				}
			}
			else
			{
                if (loggerM_ptr)
                {
    				loggerM_ptr->text(LOG_ERROR, 1, "Failed to make association");
                }
			}
		}

		// cleanup and reset association
		associationM.erase();
		associationM.reset();
	}

	// return result
	return result;
}

//>>===========================================================================

bool STORAGE_SCU_EMULATOR_CLASS::sendFilesInSingleAssociation(int repetition_index)

//  DESCRIPTION     : Perform a verification SOP Class association/send/release
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;
	MEDIA_FILE_HEADER_CLASS *fmi_ptr = NULL;
	DCM_DATASET_CLASS *dat_ptr = NULL;
	string filename;
    string transferSyntax;
	string sop_uid;

	// fill in the association parameters
	associationM.setLogger(loggerM_ptr);

    //
    // As we are sending images - it is safe to assume that the SUT is the Accepter
    // and DVT is the Requester.
    //
	associationM.setCallingAeTitle(sessionM_ptr->getDvtAeTitle());
	associationM.setCalledAeTitle(sessionM_ptr->getSutAeTitle());
	associationM.setMaximumLengthReceived(sessionM_ptr->getDvtMaximumLengthReceived());
	associationM.setImplementationClassUid(sessionM_ptr->getDvtImplementationClassUid());
	associationM.setImplementationVersionName(sessionM_ptr->getDvtImplementationVersionName());

	// loop over all file datasets to determine sop uid & transfer syntax
	vector<FMI_DATASET_STRUCT>::iterator it; 
	for (it = filedatasetsM.begin(); it < filedatasetsM.end(); ++it)
	{
		filename = (*it).filename;
        transferSyntax = (*it).transferSyntax;
		fmi_ptr  = (*it).fmi_ptr;
		dat_ptr  = (*it).dat_ptr;

		// if fmi present try to find the sop class from fmi
		if (fmi_ptr)
		{
			result = fmi_ptr->getUIValue(TAG_MEDIA_STORAGE_SOP_CLASS_UID, sop_uid);
		}
		else
		{
			// try to find sop class uid from dataset
			result = dat_ptr->getUIValue(TAG_SOP_CLASS_UID, sop_uid);
		}

		if (result)
		{
			// set presentation context(s)
			// - first check if the presentation context has already been supported by SCU.
            if (!associationM.isSupportedPresentationContext((char*) sop_uid.c_str(), (char*) transferSyntax.c_str()))
			{
				// if the file transfer syntax is ile, ele or ebe then we can convert to an other transfer syntax if need be
				// - so propose all three and see what is then supported by the SCP
				//if ((transferSyntax == IMPLICIT_VR_LITTLE_ENDIAN) ||
				//	(transferSyntax == EXPLICIT_VR_LITTLE_ENDIAN) ||
				//	(transferSyntax == EXPLICIT_VR_BIG_ENDIAN))
				//{
				//	// add this sop class to the presentation contexts
				//	if (loggerM_ptr)
				//	{
				//		loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), IMPLICIT_VR_LITTLE_ENDIAN);
				//		loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), EXPLICIT_VR_LITTLE_ENDIAN);
				//		loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), EXPLICIT_VR_BIG_ENDIAN);
				//	}

				//	associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), IMPLICIT_VR_LITTLE_ENDIAN);
				//	associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), EXPLICIT_VR_LITTLE_ENDIAN);
				//	associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), EXPLICIT_VR_BIG_ENDIAN);
				//}
				//else
				//{
				// add this sop class to the presentation contexts using DCM file TS
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), transferSyntax.c_str());
				}
				associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), (char*) transferSyntax.c_str());

				// add support for this sop class to the presentation contexts
				/*for (int i = 0 ; i < sessionM_ptr->noSupportedTransferSyntaxes(); i++)
				{
					char* ts = (char*) sessionM_ptr->getSupportedTransferSyntax(i);

					if(strcmp(ts,""))
					{
						// add this sop class to the presentation contexts
						if (!associationM.isSupportedPresentationContext((char*) sop_uid.c_str(), ts))
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_SCRIPT, 1, "Requested Presentation Context is SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), ts);
							}
							associationM.setSupportedPresentationContext((char*) sop_uid.c_str(), ts);
						}
					}
				}*/				
		    }			
		}
		else 
		{
            if (loggerM_ptr)
            {
    			loggerM_ptr->text(LOG_ERROR, 1, "Can't determine SOP Class UID for file: %s", filename.c_str());
            }
		}
	}

	//Check for export abort
	if (sessionM_ptr->isEmulationAborted())
    {
		if (loggerM_ptr)
        {
    		loggerM_ptr->text(LOG_INFO, 1, "Export operation of DICOM data is aborted from SCU.");
        }
	    goto done;
	}

	// make association
	result = associationM.makeAssociation();
	if (result)
	{
		associatedM = true;

		// iterate through the file list
		for (it = filedatasetsM.begin(); it < filedatasetsM.end(); ++it)
		{
			filename = (*it).filename;
            transferSyntax = (*it).transferSyntax;
			fmi_ptr = (*it).fmi_ptr;
			dat_ptr = (*it).dat_ptr;

			// cascade logger
			dat_ptr->setLogger(loggerM_ptr);

			// build store request object
			DCM_COMMAND_CLASS cmd(DIMSE_CMD_CSTORE_RQ);
			cmd.setLogger(loggerM_ptr);

			// group length is calulated on export
			cmd.setUSValue(TAG_COMMAND_FIELD, C_STORE_RQ);

			// set up sop uid
			if (fmi_ptr)
			{
				// from FMI
				fmi_ptr->getUIValue(TAG_MEDIA_STORAGE_SOP_CLASS_UID, sop_uid);
			}
			else
			{
				// or from dataset
				dat_ptr->getUIValue(TAG_SOP_CLASS_UID, sop_uid);
			}
			cmd.setUIValue(TAG_AFFECTED_SOP_CLASS_UID, sop_uid);

			char generated_study_uid[UI_LENGTH+1];
			char generated_series_uid[UI_LENGTH+1];
			char generated_accession_nr[SH_LENGTH+1];
			char generated_uid[UI_LENGTH+1];
			string study_instance_uid;
			string series_instance_uid;
			string accession_nr;
			string instance_uid;
						
			if (optionsM & SCU_STORAGE_EML_OPTION_DATA_UNDER_NEWSTUDY)
			{
				// All the DICOMDIR references/DCM images will be sent with  
				// new SOP/study/series instance UID & new accession nr.
				if (loggerM_ptr)
				{
        			loggerM_ptr->text(LOG_SCRIPT, 2, "Sending the DATASET under new Study");
				}

				createUID(generated_uid, (char*) sessionM_ptr->getImplementationClassUid());
				createUID(generated_study_uid, (char*) sessionM_ptr->getImplementationClassUid());
				createUID(generated_series_uid, (char*) sessionM_ptr->getImplementationClassUid());
				createSS(generated_accession_nr);

				study_instance_uid = generated_study_uid;
				series_instance_uid = generated_series_uid;
				accession_nr = generated_accession_nr;
				instance_uid = generated_uid;

				// Set the study/series instance uid & accession nr in the dataset
				dat_ptr->setUIValue(TAG_STUDY_INSTANCE_UID, study_instance_uid);
				dat_ptr->setUIValue(TAG_SERIES_INSTANCE_UID, series_instance_uid);
				dat_ptr->setSHValue(TAG_ACCESSION_NUMBER, accession_nr);				
			}
			else
			{
				// First run with unchanged UID's and second run onwards with new UID's
				if (repetition_index > 0)
				{
					// we need to generate new instance uids as we're resending the same files
					if (loggerM_ptr)
					{
    					loggerM_ptr->text(LOG_DEBUG, 1, "Generating a new SOP Instance UID for File IOD for second run onwards...");
					}
					char generated_uid[UI_LENGTH+1];
					createUID(generated_uid, (char*) sessionM_ptr->getImplementationClassUid());
					instance_uid = generated_uid;
				}
				else
				{
					if (loggerM_ptr)
					{
    					loggerM_ptr->text(LOG_DEBUG, 1, "Sending the DATASET with unchanged UID's in First run...");
					}
					if (!dat_ptr->getUIValue(TAG_SOP_INSTANCE_UID, instance_uid))
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_WARNING, 2, "File IOD doesn't contain a SOP Instance UID! - using a generated UID for Store Command...");
						}
						char generated_uid[UI_LENGTH+1];
						createUID(generated_uid, (char*) sessionM_ptr->getImplementationClassUid());
						instance_uid = generated_uid;
					}
				}
			}

			cmd.setUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, instance_uid);
			cmd.setUSValue(TAG_MESSAGE_ID, (UINT16) ++message_idM);
			cmd.setUSValue(TAG_PRIORITY, 0);
			cmd.setUSValue(TAG_DATA_SET_TYPE, DATA_SET);

			// make sure the SOP instance uid in the dataset matches the
			// affected sop instance uid in the command
			dat_ptr->setUIValue(TAG_SOP_INSTANCE_UID, instance_uid);

			// make sure the SOP instance uid in the dataset matches the
			// media storage sop instance uid in the FMI
			if((fmi_ptr) && (fmi_ptr->GetAttributeByTag(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID)) != NULL)
					fmi_ptr->setUIValue(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID, instance_uid);

			//Modify the series instance UID in Dataset
			modifyDicomObjectWithSeriesUID(dat_ptr, series_instance_uid, instance_uid);

			// disable the type 2 attribute addition
			dat_ptr->setPopulateWithAttributes(false);

			int pcId = -1;

			// if the file transfer syntax is one of those below then we can convert to another transfer synatx if
			// need be - see what is accepted by the SCP.
			if ((transferSyntax == IMPLICIT_VR_LITTLE_ENDIAN) ||
				(transferSyntax == EXPLICIT_VR_LITTLE_ENDIAN) ||
				(transferSyntax == EXPLICIT_VR_BIG_ENDIAN))
			{
				// try to get the transfer syntax directly
				pcId = associationM.getAcceptedPresentationContextId((char*) sop_uid.c_str(), (char*) transferSyntax.c_str());
				if (pcId == -1)
				{
					// try to get the Implicit VR Little Endian Transfer Syntax
					pcId = associationM.getAcceptedPresentationContextId((char*) sop_uid.c_str(), IMPLICIT_VR_LITTLE_ENDIAN);
					if (pcId != -1)
					{
		                if (loggerM_ptr)
				        {
    						loggerM_ptr->text(LOG_SCRIPT, 2, "Converting File Transfer Syntax of \"%s\" to Network Transfer Syntax of \"%s\"", 
										(char*) transferSyntax.c_str(), IMPLICIT_VR_LITTLE_ENDIAN);
						}
					}
					else
					{
						// try to get the Explicit VR Little Endian Transfer Syntax
						pcId = associationM.getAcceptedPresentationContextId((char*) sop_uid.c_str(), EXPLICIT_VR_LITTLE_ENDIAN);
						if (pcId != -1)
						{
							if (loggerM_ptr)
						    {
    							loggerM_ptr->text(LOG_SCRIPT, 2, "Converting File Transfer Syntax of \"%s\" to Network Transfer Syntax of \"%s\"", 
											(char*) transferSyntax.c_str(), EXPLICIT_VR_LITTLE_ENDIAN);
							}
						}
						else
						{
							// try to get the Explicit VR Big Endian Transfer Syntax
							pcId = associationM.getAcceptedPresentationContextId((char*) sop_uid.c_str(), EXPLICIT_VR_BIG_ENDIAN);
							if (pcId != -1)
							{
								if (loggerM_ptr)
							    {
    								loggerM_ptr->text(LOG_SCRIPT, 2, "Converting File Transfer Syntax of \"%s\" to Network Transfer Syntax of \"%s\"", 
											(char*) transferSyntax.c_str(), EXPLICIT_VR_BIG_ENDIAN);
								}
							}
						}
					}
				}
			}
			else
			{
				// try to get the transfer syntax directly
				pcId = associationM.getAcceptedPresentationContextId((char*) sop_uid.c_str(), (char*) transferSyntax.c_str());
			}

			// send the request
            if (pcId != -1)
            {
    			// log action
                if (loggerM_ptr)
                {
    		    	loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s for IOD with SOP Instance UID \"%s\" using Presentation Context ID %d (%s)", mapCommandName(cmd.getCommandId()), instance_uid.c_str(), pcId, timeStamp());
                }

                // set the presentation context id to be used to transfer the command and dataset
				cmd.setEncodePresentationContextId((BYTE)pcId);
                dat_ptr->setEncodePresentationContextId((BYTE)pcId);

				if (loggerM_ptr)
				{
        			loggerM_ptr->text(LOG_SCRIPT, 2, "SENT DATASET stored in: %s", filename.c_str());
				}

                // try sending the command and dataset
    			result = associationM.send(&cmd, dat_ptr);

    			// handle response
	    		if (result)
		    	{
			    	RECEIVE_MSG_ENUM status;
				    DCM_COMMAND_CLASS *cmd_ptr = NULL;
    				DCM_DATASET_CLASS *dat_ptr = NULL;
	    			AE_SESSION_CLASS ae_session;
					
		    		// set ae session
			    	ae_session.SetName(sessionM_ptr->getApplicationEntityName());
				    ae_session.SetVersion(sessionM_ptr->getApplicationEntityVersion());

    				status = associationM.receiveCommandDataset(&cmd_ptr, &dat_ptr, &ae_session, true, false);
	    			if (status == RECEIVE_MSG_SUCCESSFUL)
		    		{
						if((cmd_ptr) && (cmd_ptr->getCommandId()) == DIMSE_CMD_CSTORE_RSP)
						{
							UINT16 status;
							string sopClassUID;
							string sopInstanceUID;
							cmd_ptr->getUSValue(TAG_STATUS, &status);
                            cmd_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUID);
							cmd_ptr->getUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, sopInstanceUID);

							// log action
				    		if (loggerM_ptr)
                            {
    						    loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", mapCommandName(cmd_ptr->getCommandId()), timeStamp());
                            }
						    cmd_ptr->setLogger(loggerM_ptr);

                            // serialize it
                            if (serializerM_ptr)
                            {
                                serializerM_ptr->SerializeReceive(cmd_ptr, NULL);
                            }

							performStatusLogging(status,sopClassUID,sopInstanceUID);
						}
			    	}
				    else
				    {
						result = false;
						if (status == RECEIVE_MSG_ASSOC_ABORTED)
						{
							// association aborted
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_WARNING, 1, "Association has been aborted");
							}
							goto done;
						}
						else
						{
							if (loggerM_ptr)
							{
    							loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive C-STORE-RSP");
							}
						}					    
				    }					
			    }
			    else
			    {
                    if (loggerM_ptr)
                    {
    				    loggerM_ptr->text(LOG_ERROR, 1, "Failed to send C-STORE-RQ");
                    }
			    }
            }
            else
            {
                // failed to find a valid presentation context id - cannot continue.
                if (loggerM_ptr)
                {
                    loggerM_ptr->text(LOG_ERROR, 1, "No Presentation Context accepted by SCP for sending SOP Class UID: \"%s\" with Transfer Syntax: \"%s\"", sop_uid.c_str(), transferSyntax.c_str());
                }
                result = false;
            }
		}

		// release association
		result = associationM.releaseAssociation();
		if ((!result) &&
            (loggerM_ptr))
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to release association");
		}
		else
		{
			associatedM = false;
		}
	}
	else
	{
		associatedM = false;
        if (loggerM_ptr)
        {
    		loggerM_ptr->text(LOG_ERROR, 1, "Failed to make association");
        }
	}

done:
	// reset association & cleanup
	associatedM = false;
	associationM.erase();
	associationM.reset();

	// return result
	return result;
}

//>>===========================================================================

void STORAGE_SCU_EMULATOR_CLASS::performStatusLogging(UINT16 status, 
													string sop_uid,
													string sop_instance_uid)

//  DESCRIPTION     : Status logging.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	REF_INSTANCES_STRUCT instance_info;

	if(status == 0x0000)
	{
		instance_info.sopClassUid = sop_uid;
		instance_info.sopInstanceUid = sop_instance_uid;

		loggerM_ptr->text(LOG_INFO, 1, "SOP Instance UID: %s is stored successfully.", sop_instance_uid.c_str());
		
		//Push the ref sop instance
		refSopInstancesM.push_back(instance_info);
	}
	else
	{
		loggerM_ptr->text(LOG_ERROR, 1, "Storage of SOP Instance UID: %s is failed with status:0x%04X", sop_instance_uid.c_str(), status);
	}	
}

//>>===========================================================================

void STORAGE_SCU_EMULATOR_CLASS::modifyDicomObjectWithSeriesUID(DCM_ATTRIBUTE_GROUP_CLASS* dat_ptr, 
																string series_instance_uid,
																string instance_uid)

//  DESCRIPTION     : Browse through Dicom object and replace Series Instance
//					  UID new value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (int i=0; i < dat_ptr->GetNrAttributes(); i++)
	{
		// get the Attribute
		DCM_ATTRIBUTE_CLASS *attribute_ptr = dat_ptr->GetAttribute(i);

		if(attribute_ptr->GetVR() == ATTR_VR_SQ)
		{
			// only interested if one SQ value available
			if (attribute_ptr->GetNrValues() == 1)
			{
				// get SQ value
				DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

				// run through items in sequence
				for (int j=0; j < sqValue_ptr->GetNrItems(); j++)
				{
					// get next item
					DCM_ITEM_CLASS *item_ptr = sqValue_ptr->getItem(i);
					if (!item_ptr) continue;

					// run through all attributes in an item
					for (int j=0; j < item_ptr->GetNrAttributes(); j++)
					{
						// get the Attribute
						DCM_ATTRIBUTE_CLASS *attributeInItem_ptr = item_ptr->GetAttribute(i);

						if(attributeInItem_ptr != NULL)
						{
							if(attributeInItem_ptr->GetVR() == ATTR_VR_SQ)
							{
								modifyDicomObjectWithSeriesUID(item_ptr, series_instance_uid, instance_uid);
							}
							else
							{
								//check for series and image instance uids attribute
								if((attributeInItem_ptr->GetGroup()== 0x0008) && 
									(attributeInItem_ptr->GetElement() == 0x1115))
									item_ptr->setUIValue(TAG_REFERENCED_SERIES_SEQUENCE, series_instance_uid);

								if((attributeInItem_ptr->GetGroup()== 0x0008) && 
									(attributeInItem_ptr->GetElement() == 0x1155))
									item_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, instance_uid);

								if((attributeInItem_ptr->GetGroup()== 0x0020) && 
									(attributeInItem_ptr->GetElement() == 0x000E))
									item_ptr->setUIValue(TAG_SERIES_INSTANCE_UID, series_instance_uid);
							}
						}
					}
				}
			}
		}
	}
}

//>>===========================================================================

bool STORAGE_SCU_EMULATOR_CLASS::importFile(string filename)

//  DESCRIPTION     : Imports a file, and add pointers to the fmi and
//                    the dataset to the filedataset list. 
//                    Optionally the file is validated
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	AE_SESSION_CLASS ae_session;
	bool result = false;

	// set ae session
	ae_session.SetName(sessionM_ptr->getApplicationEntityName());
	ae_session.SetVersion(sessionM_ptr->getApplicationEntityVersion());

	// create local file object 
	FILE_DATASET_CLASS fileDataset(filename);

	// cascade the logger 
	fileDataset.setLogger(loggerM_ptr);

	// log action
    if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_SCRIPT, 2, "Reading media file: \"%s\"", filename.c_str());
    }

	// read the dataset from the file
	// - store the pixel data read temporarily for the transmission to the SCP
	fileDataset.setStorageMode(SM_TEMPORARY_PIXEL_ONLY);

	result = fileDataset.read();
	if (!result)
	{
		if (loggerM_ptr)
		{
    		loggerM_ptr->text(LOG_ERROR, 1, "Can't read file %s, it's not a valid media file.", filename.c_str());
		}
		return false;
	}

	// check if we are importing a DICOMDIR or ordinary media file
    DCM_DIR_DATASET_CLASS *dicomdirDataset_ptr = fileDataset.getDicomdirDataset();
	if (dicomdirDataset_ptr)
    {
		result = importRefFilesFromDicomdir(&fileDataset,filename);
	}
	else
	{
		// check if file should be validated on after import
		if (optionsM & SCU_STORAGE_EML_OPTION_VALIDATE_ON_IMPORT)
		{
			VALIDATION->validate(&fileDataset, NULL, ALL, sessionM_ptr->getSerializer(), &ae_session);
		}

		FMI_DATASET_STRUCT file_info;
		file_info.filename = filename;

		// copy the FMI pointer from the file dataset
		file_info.fmi_ptr = fileDataset.getFileMetaInformation();
		fileDataset.clearFileMetaInformationPtr();

		// copy the dataset pointer from the file dataset
		// - remove any Dataset Trailing Padding
		DCM_DATASET_CLASS	*dataset_ptr = fileDataset.getDataset();
		if (dataset_ptr)
		{
			dataset_ptr->removeTrailingPadding();
		}

		file_info.dat_ptr = dataset_ptr;
		fileDataset.clearDatasetPtr();
		file_info.transferSyntax = fileDataset.getTransferSyntax();

		if (file_info.dat_ptr == NULL)
		{
			if (loggerM_ptr)
			{
    			loggerM_ptr->text(LOG_ERROR, 1, "File %s does not contain a dataset.", filename.c_str());
			}
			result = false;
		}
		else
		{
			filedatasetsM.push_back(file_info);

			// log action
			if (loggerM_ptr)
			{
    			loggerM_ptr->text(LOG_SCRIPT, 1, "Media file dataset was encoded with Transfer Syntax: \"%s\"", file_info.transferSyntax.c_str());
			}
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool STORAGE_SCU_EMULATOR_CLASS::importRefFilesFromDicomdir(FILE_DATASET_CLASS* fileDataset,
														string filename)

//  DESCRIPTION     : Import reference files, and add pointers to the fmi and
//                    the dataset to the filedataset list. 
//                    Optionally the DICOMDIR is validated
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// Start on the directory records
    DCM_ITEM_CLASS *record = NULL;
    do 
	{
		// get the next Directory Record
		record = fileDataset->getNextDicomdirRecord();
		if (record)
		{
			DICOMDIR_RECORD_TYPE_ENUM record_type = DICOMDIR_RECORD_TYPE_UNKNOWN;
			DCM_ATTRIBUTE_CLASS* record_type_attr = record->GetAttributeByTag (TAG_DIRECTORY_RECORD_TYPE);
			if (record_type_attr != NULL)
			{
				string  record_type_name = "";
				DVT_STATUS status = record_type_attr->GetValue()->Get(record_type_name);

				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_SCRIPT, 1, "Reading Directory record: \"%s\"", record_type_name.c_str());
				}
				
				if (status == MSG_OK)
				{
					record_type = RECORD_TYPES->GetRecordTypeIndexWithRecordName (record_type_attr->GetValue());
				}
			}

			// We are interested in only Image records and associated referenced
			// dataset
			if ((record_type == DICOMDIR_RECORD_TYPE_IMAGE) || (record_type == DICOMDIR_RECORD_TYPE_PRESENTATION))
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_SCRIPT, 1, "Scanning the Image Directory record for reference images...");
				}
				DCM_ATTRIBUTE_CLASS* ref_file_attr = record->GetAttributeByTag (TAG_REFERENCED_FILE_ID);
				string    refFileName = "";
				if (ref_file_attr != NULL)
				{
					for (int i=0 ; i<ref_file_attr->GetNrValues() ; i++)
					{
						BASE_VALUE_CLASS* dcm_value = ref_file_attr->GetValue (i);
						if (dcm_value != NULL)
						{
							string   ref_File;
							if (dcm_value->Get (ref_File, true) == MSG_OK)
							{
								if (i > 0)
								{
									// This is not the first part of the filename. Add a slash '/'.
									refFileName += PATH_SEP;
								}
								refFileName += ref_File;
							}
						}
					}
				}
				else
				{
					// Attribute not present in the DICOMDIR.
					if (loggerM_ptr)
					{
    					loggerM_ptr->text(LOG_ERROR, 1, "File %s does not contain a dataset.", filename.c_str());
					}
					return false;
				}

				string ref_file_name = extractDirectoryName(filename);
				if (ref_file_name[ref_file_name.length()-1] != '\\')
				{
					ref_file_name += "\\";
				}
				ref_file_name += refFileName;

				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_SCRIPT, 1, "Sending the reference image %s " , ref_file_name.c_str());
				}

				//Check the file existance
				result = CheckFileExistance(ref_file_name);
				if (!result)
				{
					if (loggerM_ptr)
					{
    					loggerM_ptr->text(LOG_ERROR, 1, "Can't open file %s ", ref_file_name.c_str());
					}
					return false;
				}

				FMI_DATASET_STRUCT file_info;
				file_info.filename = ref_file_name;

				// create local file object 
				FILE_DATASET_CLASS refFileDataset(ref_file_name);

				// cascade the logger 
				refFileDataset.setLogger(loggerM_ptr);

				// read the dataset from the file
				// - store the pixel data read temporarily for the transmission to the SCP
				refFileDataset.setStorageMode(SM_TEMPORARY_PIXEL_ONLY);
				result = refFileDataset.read();
				if (!result)
				{
					if (loggerM_ptr)
					{
    					loggerM_ptr->text(LOG_ERROR, 1, "Can't read file %s ", ref_file_name.c_str());
					}
					return false;
				}

				// copy the FMI pointer from the file dataset
				file_info.fmi_ptr = refFileDataset.getFileMetaInformation();
				refFileDataset.clearFileMetaInformationPtr();

				// copy the dataset pointer from the file dataset
				// - remove any Dataset Trailing Padding
				DCM_DATASET_CLASS	*dataset_ptr = refFileDataset.getDataset();
				if (dataset_ptr)
				{
					dataset_ptr->removeTrailingPadding();
				}

				file_info.dat_ptr = dataset_ptr;
				refFileDataset.clearDatasetPtr();
				file_info.transferSyntax = refFileDataset.getTransferSyntax();

				if (file_info.dat_ptr == NULL)
				{
					if (loggerM_ptr)
					{
    					loggerM_ptr->text(LOG_ERROR, 1, "File %s does not contain a dataset.", ref_file_name.c_str());
					}
					result = false;
				}
				else
				{
					filedatasetsM.push_back(file_info);
					// log action
					if (loggerM_ptr)
					{
    					loggerM_ptr->text(LOG_SCRIPT, 1, "DICOMDIR reference file dataset was encoded with Transfer Syntax: \"%s\"", file_info.transferSyntax.c_str());
					}
				}
			}
			delete record;
		}
	} while (record != NULL);

	// check if file should be validated on after import
	if (optionsM & SCU_STORAGE_EML_OPTION_VALIDATE_ON_IMPORT)
	{
		AE_SESSION_CLASS ae_session;
		DEF_ATTRIBUTE_GROUP_CLASS   * def_record = NULL;

		// set ae session
		ae_session.SetName(sessionM_ptr->getApplicationEntityName());
		ae_session.SetVersion(sessionM_ptr->getApplicationEntityVersion());

		MEDIA_VALIDATOR_CLASS * validator = new MEDIA_VALIDATOR_CLASS (filename);

		DCM_DIR_DATASET_CLASS *dicomdirDataset_ptr = fileDataset->getDicomdirDataset();
		MEDIA_FILE_HEADER_CLASS *fileMetaInfo_ptr = fileDataset->getFileMetaInformation();

		// validate the DICOMDIR header 
		if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_INFO, 1, "VALIDATING the DICOMDIR header...");
        }
        validator->Validate (dicomdirDataset_ptr, NULL, fileMetaInfo_ptr, ALL, &ae_session, &def_record);
		validator->CleanResults();

		// now start on the directory records
        DCM_ITEM_CLASS *item_ptr = NULL;
        //
        // Use zero-based indexer.
        //
        int index = 0;
        do {
            // get the next Directory Record
            item_ptr = fileDataset->getNextDicomdirRecord();
            if (item_ptr)
            {
				if (loggerM_ptr) 
                {
                    loggerM_ptr->text(
                        LOG_INFO, 
                        1, 
                        "VALIDATE Directory Record, index %d", 
                        index);
                }
                //
                // validate the Directory Record.
                //
                result = validator->ValidateRecord (item_ptr, NULL, def_record, ALL, &ae_session, NULL, sessionM_ptr->getSerializer(), index, SM_TEMPORARY_PIXEL_ONLY, true, false);
                validator->CleanResults();
                delete item_ptr;

                index++;
            }
        } while (item_ptr != NULL);

		// validate the record references
		if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_INFO, 1, "VALIDATE record links...");
        }
		validator->ValidateRecordReferences();
		if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_INFO, 1, "VALIDATE complete DICOMDIR validation...");
        }
	}

	return result;
}

//>>===========================================================================

string STORAGE_SCU_EMULATOR_CLASS::extractDirectoryName (string filename)

//  DESCRIPTION     : This function parses a given filename and extracts the
//                    directory.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    int     prefix_pos;
    string  dirName = "";

    // Find the last '\' or '/', if any
    prefix_pos=filename.length()-1;
    while ((prefix_pos >= 0) && 
        ((filename[prefix_pos] != '/') && (filename[prefix_pos] != '\\')))
    {
        prefix_pos--;
    }

    // Extract the directory name - if applicable
    if (prefix_pos > 0)
    {
        dirName = filename.substr (0, prefix_pos);
    }

	return dirName;
}

//>>===========================================================================

bool STORAGE_SCU_EMULATOR_CLASS::CheckFileExistance (string filename)

//  DESCRIPTION     : Check the file existance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Add a special check when the file is not found. For some
//                    record types, it's not an error when the referenced file
//                    is not on the same media.
//<<===========================================================================
{
    FILE      * file;

    if (filename.length() == 0) return false;
    file = fopen (filename.c_str(), "rb");

    if (file == NULL)
    {
        return false;
    }

    fclose (file);
    return true;
}

//>>===========================================================================

bool STORAGE_SCU_EMULATOR_CLASS::verify()

//  DESCRIPTION     : Perform a verification SOP Class association/send/release
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result;

	// cascade logger
	associationM.setLogger(loggerM_ptr);

	// log action
    if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_SCRIPT, 2, "Verifying connection with a C-ECHO");
    }

    //
    // As we are sending the C-ECHO-RQ - it is safe to assume that the SUT is the Accepter
    // and DVT is the Requester.
    //
	associationM.setCallingAeTitle(sessionM_ptr->getDvtAeTitle());
	associationM.setCalledAeTitle(sessionM_ptr->getSutAeTitle());
	associationM.setMaximumLengthReceived(sessionM_ptr->getDvtMaximumLengthReceived());
	associationM.setImplementationClassUid(sessionM_ptr->getDvtImplementationClassUid());
	associationM.setImplementationVersionName(sessionM_ptr->getDvtImplementationVersionName());

	// set presentation context(s)
	associationM.setSupportedPresentationContext(VERIFICATION_SOP_CLASS_UID, IMPLICIT_VR_LITTLE_ENDIAN);

	// make association
	result = associationM.makeAssociation();
	if (result)
	{
		// build echo request object
		DCM_COMMAND_CLASS cmd(DIMSE_CMD_CECHO_RQ);
		cmd.setLogger(loggerM_ptr);

		// group length is calulated on export
		cmd.setUSValue(TAG_COMMAND_FIELD, C_ECHO_RQ);
		cmd.setUIValue(TAG_AFFECTED_SOP_CLASS_UID, VERIFICATION_SOP_CLASS_UID);
		cmd.setUSValue(TAG_MESSAGE_ID, 0x0001);
		cmd.setUSValue(TAG_DATA_SET_TYPE, NO_DATA_SET);

		// log action
        if (loggerM_ptr)
        {
    		loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", mapCommandName(cmd.getCommandId()), timeStamp());
        }

		// send request
		result = associationM.send(&cmd);
		if (result)
		{
			// handle response
			RECEIVE_MSG_ENUM status;
			DCM_COMMAND_CLASS *cmd_ptr = NULL;
			DCM_DATASET_CLASS *dat_ptr = NULL;
			AE_SESSION_CLASS ae_session;

			// set ae session
			ae_session.SetName(sessionM_ptr->getApplicationEntityName());
			ae_session.SetVersion(sessionM_ptr->getApplicationEntityVersion());
		
			status = associationM.receiveCommandDataset(&cmd_ptr, &dat_ptr, &ae_session, true, false);
			if (status == RECEIVE_MSG_SUCCESSFUL)
			{
				// log action
				if (cmd_ptr)
				{
                    if (loggerM_ptr)
                    {
    					loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", mapCommandName(cmd_ptr->getCommandId()), timeStamp());
                    }
					cmd_ptr->setLogger(loggerM_ptr);

                    // serialize it
                    if (serializerM_ptr)
                    {
                        serializerM_ptr->SerializeReceive(cmd_ptr, NULL);
                    }
                }

				// release association
				result = associationM.releaseAssociation();
				if ((!result) &&
                    (loggerM_ptr))
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to release association");
				}
			}
			else
			{
                if (loggerM_ptr)
                {
    				loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive C-ECHO-RSP");
                }
				result = false;
			}
		}
		else
		{
            if (loggerM_ptr)
            {
    			loggerM_ptr->text(LOG_ERROR, 1, "Failed to send C-ECHO-RQ");
            }
		}
	}
	else
	{
        if (loggerM_ptr)
        {
    		loggerM_ptr->text(LOG_ERROR, 1, "Failed to make association");
        }
	}

	associationM.erase();
	associationM.reset();

	// return result
	return result;
}

//>>===========================================================================

bool STORAGE_SCU_EMULATOR_CLASS::sendNActionReq(int delay)

//  DESCRIPTION     : Build N-Action Req command & send it over association
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	if(refSopInstancesM.size() == 0)
	{
		// log action
		if (loggerM_ptr)
		{
    		loggerM_ptr->text(LOG_INFO, 2, "Not sending N-ACTION-RQ command because no SOP instance is stored successfully.");
		}

		return false;
	}

	// cascade logger
	associationM.setLogger(loggerM_ptr);

	//
    // As we are sending the N-ACTION-RQ - it is safe to assume that the SUT is the Accepter
    // and DVT is the Requester.
    //
	associationM.setCallingAeTitle(sessionM_ptr->getDvtAeTitle());
	associationM.setCalledAeTitle(sessionM_ptr->getSutAeTitle());
	associationM.setRemoteHostname((char*)sessionM_ptr->getRemoteHostname());
	associationM.setRemoteConnectPort((UINT16)(sessionM_ptr->getRemoteConnectPort()));
	associationM.setMaximumLengthReceived(sessionM_ptr->getDvtMaximumLengthReceived());
	associationM.setImplementationClassUid(sessionM_ptr->getDvtImplementationClassUid());
	associationM.setImplementationVersionName(sessionM_ptr->getDvtImplementationVersionName());
	associationM.setScpScuRoleSelect(STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID, 0, 1);

	// set presentation context(s)
	associationM.setSupportedPresentationContext(STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID, EXPLICIT_VR_LITTLE_ENDIAN);
	associationM.setSupportedPresentationContext(STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID, IMPLICIT_VR_LITTLE_ENDIAN);

	// make association
	result = associationM.makeAssociation();
	if (result)
	{
		// log action
		if (loggerM_ptr)
		{
    		loggerM_ptr->text(LOG_INFO, 2, "Build N-ACTION-RQ command for Storage Commitment");
		}

		// build action request command object
		DCM_COMMAND_CLASS cmd(DIMSE_CMD_NACTION_RQ);
		cmd.setLogger(loggerM_ptr);

		// group length is calulated on export
		cmd.setUIValue(TAG_REQUESTED_SOP_CLASS_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID);
		cmd.setUSValue(TAG_COMMAND_FIELD, N_ACTION_RQ);
		cmd.setUSValue(TAG_MESSAGE_ID, 0x0001);
		cmd.setUSValue(TAG_DATA_SET_TYPE, DATA_SET);
		cmd.setUIValue(TAG_REQUESTED_SOP_INSTANCE_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_INSTANCE_UID);
		cmd.setUSValue(TAG_ACTION_TYPE_ID, 0x0001);


		// build action request dataset object
		DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();

		// set up the command id and iod name fields
		dataset_ptr->setCommandId(DIMSE_CMD_NACTION_RQ);
		dataset_ptr->setIodName("Commitment Push");

		// add the Transaction UID
		char generated_transactionUid[UI_LENGTH+1];
		createUID(generated_transactionUid, (char*) sessionM_ptr->getImplementationClassUid());
		string transactionUid = generated_transactionUid;
		dataset_ptr->setUIValue(TAG_TRANSACTION_UID, transactionUid);

		// Create the referenced sop sequence attribute
		DCM_ATTRIBUTE_CLASS *refSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_REFERENCED_SOP_SEQUENCE, ATTR_VR_SQ);
		dataset_ptr->addAttribute(refSq_ptr);
		DCM_VALUE_SQ_CLASS *sqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
		refSq_ptr->AddValue(sqValue_ptr);

		// loop over all stored instances
		vector<REF_INSTANCES_STRUCT>::iterator it; 
		for (it = refSopInstancesM.begin(); it < refSopInstancesM.end(); ++it)
		{
			// generate an item
			DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

			// add SOP Class UID
			item_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, (*it).sopClassUid);

			// add SOP Instance UID
			item_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, (*it).sopInstanceUid);

			// add the item to the dataset
			sqValue_ptr->Set(item_ptr);
		}

		// Create the referenced sop sequence attribute
		DCM_ATTRIBUTE_CLASS *studyCompSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_REFERENCED_STUDY_COMPONENT_SEQUENCE, ATTR_VR_SQ);
		dataset_ptr->addAttribute(studyCompSq_ptr);
		DCM_VALUE_SQ_CLASS *studySeqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
		studyCompSq_ptr->AddValue(studySeqValue_ptr);

		// generate an item
		DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

		// add SOP Class UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, "1.2.840.10008.3.1.2.3.3");

		// add SOP Instance UID
		char generated_sopInstanceUID[UI_LENGTH+1];
		createUID(generated_sopInstanceUID, (char*) sessionM_ptr->getImplementationClassUid());
		string sopInstanceUID = generated_sopInstanceUID;
		item_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, sopInstanceUID);

		// add the item to the dataset
		studySeqValue_ptr->Set(item_ptr);

        // log action
        if (loggerM_ptr)
        {
    		loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s %s (%s)", mapCommandName(cmd.getCommandId()), dataset_ptr->getIodName(), timeStamp());
        }

		// send request
		result = associationM.send(&cmd, dataset_ptr);
		if (result)
		{
			// handle response
			RECEIVE_MSG_ENUM status;
			DCM_COMMAND_CLASS *cmd_ptr = NULL;
			DCM_DATASET_CLASS *dat_ptr = NULL;
			AE_SESSION_CLASS ae_session;

			// set ae session
			ae_session.SetName(sessionM_ptr->getApplicationEntityName());
			ae_session.SetVersion(sessionM_ptr->getApplicationEntityVersion());		
		
			status = associationM.receiveCommandDataset(&cmd_ptr, &dat_ptr, &ae_session, true, false);

			if (status == RECEIVE_MSG_SUCCESSFUL)
			{
				// log action
				if (cmd_ptr)
				{
					if (loggerM_ptr)
					{
    					loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", mapCommandName(cmd_ptr->getCommandId()), timeStamp());
					}
					cmd_ptr->setLogger(loggerM_ptr);

					// serialize it
					if (serializerM_ptr)
					{
						serializerM_ptr->SerializeReceive(cmd_ptr, NULL);
					}
				}
			}
			else
			{
				result = false;
				if (status == RECEIVE_MSG_ASSOC_ABORTED)
				{
					// association aborted
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_WARNING, 1, "Association has been aborted");
					}
					goto done;
				}
				if (loggerM_ptr)
				{
    				loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive N-ACTION-RSP command");
				}				
			}
			
			if((delay > 0) && (result))
			{
				// log action
				if (loggerM_ptr)
				{
    				loggerM_ptr->text(LOG_INFO, 2, "Waiting for %d seconds for N-EVENTREPORT command..", delay);
				}

				// get the start time in seconds
				time_t startTime;
				time(&startTime);

				long timeDiff = 0;

				// Loop while waiting for N-EVENTREPORT-RQ - until it is either timeout
				while (result)
				{
					if(timeDiff < delay)
					{
						status = associationM.receiveCommandDataset(&cmd_ptr, &dat_ptr, &ae_session, true, false);

						if (status == RECEIVE_MSG_SUCCESSFUL)
						{
							if((cmd_ptr->getCommandId() == DIMSE_CMD_NEVENTREPORT_RQ) && (status == RECEIVE_MSG_SUCCESSFUL))
							{
								// log action
								if (cmd_ptr)
								{
									if (loggerM_ptr)
									{
    									loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", mapCommandName(cmd_ptr->getCommandId()), timeStamp());
									}
									cmd_ptr->setLogger(loggerM_ptr);

									// serialize it
									if (serializerM_ptr)
									{
										serializerM_ptr->SerializeReceive(cmd_ptr, NULL);
									}
								}

								// Build N-EVENT-RSP object
								DCM_COMMAND_CLASS cmdEventRsp(DIMSE_CMD_NEVENTREPORT_RSP);
								cmdEventRsp.setLogger(loggerM_ptr);

								// group length is calulated on export
								cmdEventRsp.setUSValue(TAG_COMMAND_FIELD, N_EVENT_REPORT_RSP);
								//cmdEventRsp.setUSValue(TAG_MESSAGE_ID, 0x0001);
								cmdEventRsp.setUSValue(TAG_DATA_SET_TYPE, NO_DATA_SET);
								cmdEventRsp.setUIValue(TAG_AFFECTED_SOP_CLASS_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID);
								cmdEventRsp.setUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_INSTANCE_UID);

								// Set the status value
								cmdEventRsp.setUSValue(TAG_STATUS, 0x0000);

								// log action
								if (loggerM_ptr)
								{
    								loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", mapCommandName(cmdEventRsp.getCommandId()), timeStamp());
								}

								//Send N-EVENT-REPORT-RSP
								result = associationM.send(&cmdEventRsp);
								break;
							}
						}
						else
						{
							result = false;
							if (status == RECEIVE_MSG_ASSOC_ABORTED)
							{
								// association aborted
								if (loggerM_ptr)
								{
									loggerM_ptr->text(LOG_WARNING, 1, "Association has been aborted");
								}
								goto done;
							}
							if (loggerM_ptr)
							{
    							loggerM_ptr->text(LOG_ERROR, 1, "Received unexpected command");
							}				
						}        
					}
					else
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive Storage Commitment N-EVENT-REPORT-RQ");
						}
						result = false;
						break;
					}

					// Get the current time
					time_t currentTime;
					time(&currentTime);

					timeDiff = (unsigned)(currentTime - startTime);					
				}

				// Release association
				result = associationM.releaseAssociation();
				if ((!result) &&
					(loggerM_ptr))
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to release association");
				}
			}
			else if((delay == 0) && (result))
			{
				// log action
				if (loggerM_ptr)
				{
    				loggerM_ptr->text(LOG_INFO, 2, "Waiting infinitely for N-EVENTREPORT command..");
				}

				// Loop while waiting for N-EVENTREPORT-RQ
				while (result)
				{
					status = associationM.receiveCommandDataset(&cmd_ptr, &dat_ptr, &ae_session, true, false);

					if (status == RECEIVE_MSG_SUCCESSFUL)
					{
						if((cmd_ptr->getCommandId() == DIMSE_CMD_NEVENTREPORT_RQ) && (status == RECEIVE_MSG_SUCCESSFUL))
						{
							// log action
							if (cmd_ptr)
							{
								if (loggerM_ptr)
								{
    								loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", mapCommandName(cmd_ptr->getCommandId()), timeStamp());
								}
								cmd_ptr->setLogger(loggerM_ptr);

								// serialize it
								if (serializerM_ptr)
								{
									serializerM_ptr->SerializeReceive(cmd_ptr, NULL);
								}
							}

							// Build N-EVENT-RSP object
							DCM_COMMAND_CLASS cmdEventRsp(DIMSE_CMD_NEVENTREPORT_RSP);
							cmdEventRsp.setLogger(loggerM_ptr);

							// group length is calulated on export
							cmdEventRsp.setUSValue(TAG_COMMAND_FIELD, N_EVENT_REPORT_RSP);
							//cmdEventRsp.setUSValue(TAG_MESSAGE_ID, 0x0001);
							cmdEventRsp.setUSValue(TAG_DATA_SET_TYPE, NO_DATA_SET);
							cmdEventRsp.setUIValue(TAG_AFFECTED_SOP_CLASS_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID);
							cmdEventRsp.setUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_INSTANCE_UID);

							// Set the status value
							cmdEventRsp.setUSValue(TAG_STATUS, 0x0000);

							// log action
							if (loggerM_ptr)
							{
    							loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", mapCommandName(cmdEventRsp.getCommandId()), timeStamp());
							}

							//Send N-EVENT-REPORT-RSP
							result = associationM.send(&cmdEventRsp);
							break;
						}

						if (sessionM_ptr->isSessionStopped())
						{
							break;
						}
					}
					else
					{
						result = false;
						if (status == RECEIVE_MSG_ASSOC_ABORTED)
						{
							// association aborted
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_WARNING, 1, "Association has been aborted");
							}
							goto done;
						}
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_ERROR, 1, "Received unexpected command");
						}				
					}   
				}

				// Release association
				result = associationM.releaseAssociation();
				if ((!result) &&
					(loggerM_ptr))
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to release association");
				}
			}
			else
			{			
				// Release association
				result = associationM.releaseAssociation();
				if ((!result) &&
					(loggerM_ptr))
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to release association");
				}
			}
		}
		else
		{
            if (loggerM_ptr)
            {
    			loggerM_ptr->text(LOG_ERROR, 1, "Failed to send N-ACTION-RQ");
            }
		}
	}
	else
	{
        if (loggerM_ptr)
        {
    		loggerM_ptr->text(LOG_ERROR, 1, "Failed to make association");
        }
	}

done:
	associationM.erase();
	associationM.reset();

	// return result
	return result;
}

//>>===========================================================================

bool STORAGE_SCU_EMULATOR_CLASS::eventReportStorageCommitment(UINT16 eventTypeId, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Report the given storage commitment event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for a valid dataset
	if (dataset_ptr == NULL) return false;

	// cascade logger
	associationM.setLogger(loggerM_ptr);

    //
    // As we are sending the storage commit event - it is safe to assume that the SUT is the Accepter
    // and DVT is the Requester.
    //
	associationM.setCallingAeTitle(sessionM_ptr->getDvtAeTitle());
	associationM.setCalledAeTitle(sessionM_ptr->getSutAeTitle());
	associationM.setMaximumLengthReceived(sessionM_ptr->getDvtMaximumLengthReceived());
	associationM.setImplementationClassUid(sessionM_ptr->getDvtImplementationClassUid());
	associationM.setImplementationVersionName(sessionM_ptr->getDvtImplementationVersionName());
	associationM.setScpScuRoleSelect(STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID, 1, 0);

	// set presentation context(s)
	associationM.setSupportedPresentationContext(STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID, EXPLICIT_VR_LITTLE_ENDIAN);
	associationM.setSupportedPresentationContext(STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID, IMPLICIT_VR_LITTLE_ENDIAN);

	// make association
	bool result = associationM.makeAssociation();
	if (result)
	{
		// build event report request object
		DCM_COMMAND_CLASS cmd(DIMSE_CMD_NEVENTREPORT_RQ);
		cmd.setLogger(loggerM_ptr);

		// group length is calulated on export
		cmd.setUSValue(TAG_COMMAND_FIELD, N_EVENT_REPORT_RQ);
		cmd.setUIValue(TAG_AFFECTED_SOP_CLASS_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID);
		cmd.setUSValue(TAG_MESSAGE_ID, 0x0001);
		cmd.setUSValue(TAG_DATA_SET_TYPE, DATA_SET);
		cmd.setUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_INSTANCE_UID);
		cmd.setUSValue(TAG_EVENT_TYPE_ID, eventTypeId);

		// log action
        if (loggerM_ptr)
        {
    		loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s %s (%s)", mapCommandName(cmd.getCommandId()), dataset_ptr->getIodName(), timeStamp());
        }

		// send request
		result = associationM.send(&cmd, dataset_ptr);
		if (result)
		{
			// handle response
			RECEIVE_MSG_ENUM status;
			DCM_COMMAND_CLASS *cmd_ptr = NULL;
			DCM_DATASET_CLASS *dat_ptr = NULL;
			AE_SESSION_CLASS ae_session;

			// set ae session
			ae_session.SetName(sessionM_ptr->getApplicationEntityName());
			ae_session.SetVersion(sessionM_ptr->getApplicationEntityVersion());
		
			status = associationM.receiveCommandDataset(&cmd_ptr, &dat_ptr, &ae_session, true, false);
			if (status == RECEIVE_MSG_SUCCESSFUL)
			{
				// log action
				if (cmd_ptr)
				{
                    if (loggerM_ptr)
                    {
    					loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", mapCommandName(cmd_ptr->getCommandId()), timeStamp());
                    }
					cmd_ptr->setLogger(loggerM_ptr);

                    // serialize it
                    if (serializerM_ptr)
                    {
                        serializerM_ptr->SerializeReceive(cmd_ptr, NULL);
                    }
                }

				// release association
				result = associationM.releaseAssociation();
				if ((!result) &&
                    (loggerM_ptr))
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to release association");
				}
			}
			else
			{
                if (loggerM_ptr)
                {
    				loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive N-EVENT-REPORT-RSP");
                }
				result = false;
			}
		}
		else
		{
            if (loggerM_ptr)
            {
    			loggerM_ptr->text(LOG_ERROR, 1, "Failed to send N-EVENT-REPORT-RQ");
            }
		}
	}
	else
	{
        if (loggerM_ptr)
        {
    		loggerM_ptr->text(LOG_ERROR, 1, "Failed to make association");
        }
	}

	associationM.erase();
	associationM.reset();

	// return result
	return result;
}

//>>===========================================================================

void STORAGE_SCU_EMULATOR_CLASS::addFile(string filename)

//  DESCRIPTION     : Add file
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	filenamesM.push_back(filename);
}

//>>===========================================================================

void STORAGE_SCU_EMULATOR_CLASS::removeFile(string filename)

//  DESCRIPTION     : Remove file
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	vector<string>::iterator it;
	for (it = filenamesM.begin(); it < filenamesM.end(); ++it)
	{
		if (*it == filename)
		{
			filenamesM.erase(it);
		}
	}
}

//>>===========================================================================

UINT STORAGE_SCU_EMULATOR_CLASS::resetOptions()

//  DESCRIPTION     : Resets options
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// restore defaults
	optionsM = 0;
	optionsM |= SCU_STORAGE_EML_OPTION_ASSOC_MULTI;
	optionsM |= SCU_STORAGE_EML_OPTION_VALIDATE_ON_IMPORT;

	return optionsM;
}

//>>===========================================================================

UINT STORAGE_SCU_EMULATOR_CLASS::setOption(UINT option)

//  DESCRIPTION     : Set options mask
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	optionsM = option;
	return optionsM;
}

//>>===========================================================================

UINT STORAGE_SCU_EMULATOR_CLASS::addOption(UINT option)

//  DESCRIPTION     : Adds option to mask
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	optionsM |= option;
	return optionsM;
}

//>>===========================================================================

void STORAGE_SCU_EMULATOR_CLASS::setNrRepetitions(UINT nr)

//  DESCRIPTION     : Set nr repetitions
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	nr_repetitionsM = nr;
}

//>>===========================================================================

void STORAGE_SCU_EMULATOR_CLASS::cleanup()

//  DESCRIPTION     : Cleanup function
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	vector<FMI_DATASET_STRUCT>::iterator it; 
	for (it = filedatasetsM.begin(); it < filedatasetsM.end(); ++it)
	{
		MEDIA_FILE_HEADER_CLASS *fmi_ptr = (*it).fmi_ptr;
		DCM_DATASET_CLASS *dat_ptr = (*it).dat_ptr;

		// release the FMI - .PIX file will be deleted
		if (fmi_ptr != NULL)
		{
			delete fmi_ptr;
			fmi_ptr = NULL;
		}

		// release the dataset - .PIX file will be deleted
		if (dat_ptr != NULL)
		{
			delete dat_ptr;
			dat_ptr = NULL;
		}
	}
}

//>>===========================================================================

void STORAGE_SCU_EMULATOR_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
    loggerM_ptr = logger_ptr;
	associationM.setLogger(logger_ptr); 
}

//>>===========================================================================

void STORAGE_SCU_EMULATOR_CLASS::setSerializer(BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Set the serializer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
    serializerM_ptr = serializer_ptr;
	associationM.setSerializer(serializer_ptr); 
}
