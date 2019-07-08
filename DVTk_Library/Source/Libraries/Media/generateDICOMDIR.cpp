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
//  DESCRIPTION     :	File based DICOM Dataset class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "generateDICOMDIR.h"
#include "Idefinition.h"		// Definition component interface
#include "Inetwork.h"			// Network component interface

//>>===========================================================================

PATIENT_INFO_CLASS::PATIENT_INFO_CLASS(string id, string name, vector<string> spChrSets, string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	patientId = id;
	patientName = name;
	charSets = spChrSets;
	identifier = Identifier;
}
	
//>>===========================================================================

PATIENT_INFO_CLASS::~PATIENT_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any study data
	while (studyData.getSize())
	{
		delete studyData[0];
		studyData.removeAt(0);
	}
}

//>>===========================================================================

STUDY_INFO_CLASS *PATIENT_INFO_CLASS::searchStudy(string instanceUid)

//  DESCRIPTION     : Search the Patient for Study Data with an instance
//					: uid matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search study data
	for (UINT i = 0; i < studyData.getSize(); i++)
	{
		STUDY_INFO_CLASS *studyData_ptr = studyData[i];

		// check for match
		if ((studyData_ptr != NULL) && 
			(instanceUid == studyData_ptr->getInstanceUid()))
		{
			// match found - return it
			return studyData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

bool PATIENT_INFO_CLASS::operator = (PATIENT_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this patient to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	patientId = sourceData.getPatientId();
	patientName = sourceData.getPatientName();
	charSets = sourceData.getSpCharSetValues();
	identifier = sourceData.getIdentifier();
	for (UINT i = 0; i < sourceData.noStudies(); i++)
	{
		studyData[i] = sourceData.getStudyData(i);
	}
	return true;
}

//>>===========================================================================

HANGING_PROTOCOL_INFO_CLASS::HANGING_PROTOCOL_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string name, string desc, string level, string creator, string creationTime, DCM_VALUE_SQ_CLASS * protoDefSq,string nrPriors,DCM_VALUE_SQ_CLASS * protoUserIdSq,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	hangingProtoName = name;
	hangingProtoDesc = desc;
	hangingProtoLevel = level;
	hangingProtoCreator = creator;
	hangingProtoCreationDtTime = creationTime;
	hangProtoDefSeqPtr = protoDefSq;
	nrOfPriorsRef = nrPriors;
	hangProtoUserIdentificSeqPtr = protoUserIdSq;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

HANGING_PROTOCOL_INFO_CLASS::~HANGING_PROTOCOL_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool HANGING_PROTOCOL_INFO_CLASS::operator = (HANGING_PROTOCOL_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets = sourceData.getSpCharSetValues();
	hangingProtoName = sourceData.getHangingProtoName();
	hangingProtoDesc = sourceData.getHangingProtoDesc();
	hangingProtoLevel = sourceData.getHangingProtoLevel();
	hangingProtoCreator = sourceData.getHangingProtoCreator();
	hangingProtoCreationDtTime = sourceData.getHangingProtoCreationDtTime();
	hangProtoDefSeqPtr = sourceData.getHangProtoDefSeqPtr();
	nrOfPriorsRef = sourceData.getNrOfPriorsRef();
	hangProtoUserIdentificSeqPtr = sourceData.getHangProtoUserIdentificSeqPtr();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

STUDY_INFO_CLASS::STUDY_INFO_CLASS(string Uid, string Id,string Date,string Time,string Descr,string AccessionNr,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	instanceUid = Uid;
	studyId = Id;
	studyDate = Date;
	studyTime = Time;
	studyDescr = Descr;
	accessionNr = AccessionNr;
	identifier = Identifier;
}
	
//>>===========================================================================

STUDY_INFO_CLASS::~STUDY_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any series data
	while (seriesData.getSize()) 
	{
		delete seriesData[0];
		seriesData.removeAt(0);
	}
}

//>>===========================================================================

SERIES_INFO_CLASS *STUDY_INFO_CLASS::searchSeries(string instanceUid)

//  DESCRIPTION     : Search Study for Series Data with an instance uid
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search series data
	for (UINT i = 0; i < seriesData.getSize(); i++)
	{
		SERIES_INFO_CLASS *seriesData_ptr = seriesData[i];

		// check for match
		if ((seriesData_ptr != NULL) &&
			(instanceUid == seriesData_ptr->getInstanceUid())) 
		{
			// match found - return it
			return seriesData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

bool STUDY_INFO_CLASS::operator = (STUDY_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this study to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceUid = sourceData.getInstanceUid();
	studyId = sourceData.getStudyId();
	studyDate = sourceData.getStudyDate();
	studyTime = sourceData.getStudyTime();
	studyDescr = sourceData.getStudyDescr();
	accessionNr = sourceData.getAccessionNr();
	identifier = sourceData.getIdentifier();
	for (UINT i = 0; i < sourceData.noSeries(); i++)
	{
		seriesData[i] = sourceData.getSeriesData(i);
	}
	return true;
}

//>>===========================================================================

SERIES_INFO_CLASS::SERIES_INFO_CLASS(string InstanceUid, string Modality,INT32 Nr,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	instanceUid = InstanceUid;
	modality = Modality;
	seriesNr = Nr;
	identifier = Identifier;
}
	
//>>===========================================================================

SERIES_INFO_CLASS::~SERIES_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any sop instance data
	while (sopInstanceData.getSize())
	{
		delete sopInstanceData[0];
		sopInstanceData.removeAt(0);
	}
}

//>>===========================================================================

IMAGE_INFO_CLASS *SERIES_INFO_CLASS::searchImage(string instanceUid)

//  DESCRIPTION     : Serach the series for SOP Instance Data with an instance uid
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < sopInstanceData.getSize(); i++) 
	{
		IMAGE_INFO_CLASS *sopInstanceData_ptr = sopInstanceData[i];

		// check for match
		if ((sopInstanceData_ptr != NULL) &&
			(instanceUid == sopInstanceData_ptr->getInstanceUid()))
		{
			// match found - return it
			return sopInstanceData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

PRESENTATION_STATE_INFO_CLASS *SERIES_INFO_CLASS::searchPS(string instanceUid)

//  DESCRIPTION     : Serach the series for PS Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < presentationStateData.getSize(); i++) 
	{
		PRESENTATION_STATE_INFO_CLASS *psData_ptr = presentationStateData[i];

		// check for match
		if ((psData_ptr != NULL) &&
			(instanceUid == psData_ptr->getRefSOPClassInstanceUid()))
		{
			// match found - return it
			return psData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

WAVEFORM_INFO_CLASS *SERIES_INFO_CLASS::searchWF(string date, string time)

//  DESCRIPTION     : Serach the series for WF Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < waveformData.getSize(); i++) 
	{
		WAVEFORM_INFO_CLASS *wfData_ptr = waveformData[i];

		// check for match
		if ((wfData_ptr != NULL) &&
			(date == wfData_ptr->getWFCreationDate())&&
			(time == wfData_ptr->getWFCreationTime()))
		{
			// match found - return it
			return wfData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

RAWDATA_INFO_CLASS *SERIES_INFO_CLASS::searchRawData(string date, string time)

//  DESCRIPTION     : Serach the series for PS Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < rawData.getSize(); i++) 
	{
		RAWDATA_INFO_CLASS *rawData_ptr = rawData[i];

		// check for match
		if ((rawData_ptr != NULL) &&
			(date == rawData_ptr->getRDCreationDate())&&
			(time == rawData_ptr->getRDCreationTime()))
		{
			// match found - return it
			return rawData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

SPECTROSCOPY_INFO_CLASS *SERIES_INFO_CLASS::searchSpectData(string date, string time)

//  DESCRIPTION     : Serach the series for Spect Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < spectData.getSize(); i++) 
	{
		SPECTROSCOPY_INFO_CLASS *spectData_ptr = spectData[i];

		// check for match
		if ((spectData_ptr != NULL) &&
			(date == spectData_ptr->getSSCreationDate())&&
			(time == spectData_ptr->getSSCreationTime()))
		{
			// match found - return it
			return spectData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

SRDOC_INFO_CLASS *SERIES_INFO_CLASS::searchSRDoc(string date, string time)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < srDocData.getSize(); i++) 
	{
		SRDOC_INFO_CLASS *srDocData_ptr = srDocData[i];

		// check for match
		if ((srDocData_ptr != NULL) &&
			(date == srDocData_ptr->getContentDate()) &&
			(time == srDocData_ptr->getContentTime()))
		{
			// match found - return it
			return srDocData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

ENCAP_DOC_INFO_CLASS *SERIES_INFO_CLASS::searchEncapDocData(string date, string time)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < encapDocData.getSize(); i++) 
	{
		ENCAP_DOC_INFO_CLASS *encapDocData_ptr = encapDocData[i];

		// check for match
		if ((encapDocData_ptr != NULL) &&
			(date == encapDocData_ptr->getContentDate()) &&
			(time == encapDocData_ptr->getContentTime()))
		{
			// match found - return it
			return encapDocData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

REGISTRATION_INFO_CLASS *SERIES_INFO_CLASS::searchRegistration(string date, string time)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < registrationData.getSize(); i++) 
	{
		REGISTRATION_INFO_CLASS *regData_ptr = registrationData[i];

		// check for match
		if ((regData_ptr != NULL) &&
			(date == regData_ptr->getContentDate()) &&
			(time == regData_ptr->getContentTime()))
		{
			// match found - return it
			return regData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

FIDUCIAL_INFO_CLASS *SERIES_INFO_CLASS::searchFiducial(string date, string time)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < fiducialData.getSize(); i++) 
	{
		FIDUCIAL_INFO_CLASS *fiducialData_ptr = fiducialData[i];

		// check for match
		if ((fiducialData_ptr != NULL) &&
			(date == fiducialData_ptr->getContentDate()) &&
			(time == fiducialData_ptr->getContentTime()))
		{
			// match found - return it
			return fiducialData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

RT_DOSE_INFO_CLASS *SERIES_INFO_CLASS::searchRTDose(string dose)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < rtDoseData.getSize(); i++) 
	{
		RT_DOSE_INFO_CLASS *rtDoseData_ptr = rtDoseData[i];

		// check for match
		if ((rtDoseData_ptr != NULL) &&
			(dose == rtDoseData_ptr->getDoseSummationType()))
		{
			// match found - return it
			return rtDoseData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

RT_STRUC_SET_INFO_CLASS *SERIES_INFO_CLASS::searchRTStructSet(string date, string time)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < rtStructData.getSize(); i++) 
	{
		RT_STRUC_SET_INFO_CLASS *rtStructData_ptr = rtStructData[i];

		// check for match
		if ((rtStructData_ptr != NULL) &&
			(date == rtStructData_ptr->getRTStrucSetDate()) &&
			(time == rtStructData_ptr->getRTStrucSetTime()))
		{
			// match found - return it
			return rtStructData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

RT_PLAN_INFO_CLASS *SERIES_INFO_CLASS::searchRTPlan(string date, string time)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < rtPlanData.getSize(); i++) 
	{
		RT_PLAN_INFO_CLASS *rtPlanData_ptr = rtPlanData[i];

		// check for match
		if ((rtPlanData_ptr != NULL) &&
			(date == rtPlanData_ptr->getRTPlanDate()) &&
			(time == rtPlanData_ptr->getRTPlanTime()))
		{
			// match found - return it
			return rtPlanData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

RT_TREATMENT_INFO_CLASS *SERIES_INFO_CLASS::searchRTTreat(string date, string time)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < rtTreatData.getSize(); i++) 
	{
		RT_TREATMENT_INFO_CLASS *rtTreatData_ptr = rtTreatData[i];

		// check for match
		if ((rtTreatData_ptr != NULL) &&
			(date == rtTreatData_ptr->getRTTreatDate()) &&
			(time == rtTreatData_ptr->getRTTreatTime()))
		{
			// match found - return it
			return rtTreatData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

VALUE_MAP_INFO_CLASS *SERIES_INFO_CLASS::searchValueMap(string date, string time)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < valueMapData.getSize(); i++) 
	{
		VALUE_MAP_INFO_CLASS *valueMapData_ptr = valueMapData[i];

		// check for match
		if ((valueMapData_ptr != NULL) &&
			(date == valueMapData_ptr->getContentDate()) &&
			(time == valueMapData_ptr->getContentTime()))
		{
			// match found - return it
			return valueMapData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

KEY_OBJECT_DOC_INFO_CLASS *SERIES_INFO_CLASS::searchKeyobject(string date, string time)

//  DESCRIPTION     : Serach the series for SR Doc Data with an SOP Instance UID
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < keyObjData.getSize(); i++) 
	{
		KEY_OBJECT_DOC_INFO_CLASS *keyObjData_ptr = keyObjData[i];

		// check for match
		if ((keyObjData_ptr != NULL) &&
			(date == keyObjData_ptr->getContentDate()) &&
			(time == keyObjData_ptr->getContentTime()))
		{
			// match found - return it
			return keyObjData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

bool SERIES_INFO_CLASS::operator = (SERIES_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this series to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceUid = sourceData.getInstanceUid();
	modality = sourceData.getModality();
	seriesNr = atoi(sourceData.getSeriesNr().c_str());
	identifier = sourceData.getIdentifier();
	for (UINT i = 0; i < sourceData.noSopInstances(); i++)
	{
		sopInstanceData[i] = sourceData.getSopInstanceData(i);
	}
	return true;
}

//>>===========================================================================

PRESENTATION_STATE_INFO_CLASS::PRESENTATION_STATE_INFO_CLASS(INT32 instanceNumber, string refFile, string sopClassUid, string sopClassInstanceUid, string tsUid, DCM_VALUE_SQ_CLASS * refSeriesSq,string date, string time, string lable, string desc, string creator, string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	refFileId = refFile;
	refSOPClassUid = sopClassUid;
	refSOPClassInstanceUid = sopClassInstanceUid;
	refTSUid = tsUid;
	refSeriesSeqValuePtr = refSeriesSq;
	psCreationDate = date;
	psCreationTime = time;
	contentLable = lable;
	contentDesc = desc;
	contentCreator = creator;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

PRESENTATION_STATE_INFO_CLASS::~PRESENTATION_STATE_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool PRESENTATION_STATE_INFO_CLASS::operator = (PRESENTATION_STATE_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	refFileId = sourceData.getRefFileId();
	refSOPClassUid = sourceData.getRefSOPClassUid();
	refSOPClassInstanceUid = sourceData.getRefSOPClassInstanceUid();
	refTSUid = sourceData.getRefTSUid();
	refSeriesSeqValuePtr = sourceData.getRefSeriesSeqValue();
	psCreationDate = sourceData.getPSCreationDate();
	psCreationTime = sourceData.getPSCreationTime();
	contentLable = sourceData.getContentLable();
	contentDesc = sourceData.getContentDesc();
	contentCreator = sourceData.getContentCreator();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

IMAGE_INFO_CLASS::IMAGE_INFO_CLASS(string uid, string fileId, string sopClassUid, string sopClassInstanceUid, string tsUid, INT32 instanceNumber,vector<string> spCharSets, string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceUid = uid;
	refFileId = fileId;
	refSOPClassUid = sopClassUid;
	refSOPClassInstanceUid = sopClassInstanceUid;
	refTSUid = tsUid;
	instanceNr = instanceNumber;
	charSets = spCharSets;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

IMAGE_INFO_CLASS::~IMAGE_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool IMAGE_INFO_CLASS::operator = (IMAGE_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceUid = sourceData.getInstanceUid();
	refFileId = sourceData.getRefFileId();
	refSOPClassUid = sourceData.getRefSOPClassUid();
	refSOPClassInstanceUid = sourceData.getRefSOPClassInstanceUid();
	refTSUid = sourceData.getRefTSUid();
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

WAVEFORM_INFO_CLASS::WAVEFORM_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string date, string time, string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	wfCreationDate = date;
	wfCreationTime = time;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

WAVEFORM_INFO_CLASS::~WAVEFORM_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool WAVEFORM_INFO_CLASS::operator = (WAVEFORM_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	wfCreationDate = sourceData.getWFCreationDate();
	wfCreationTime = sourceData.getWFCreationTime();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

RAWDATA_INFO_CLASS::RAWDATA_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string date, string time, string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	rdCreationDate = date;
	rdCreationTime = time;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

RAWDATA_INFO_CLASS::~RAWDATA_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool RAWDATA_INFO_CLASS::operator = (RAWDATA_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	rdCreationDate = sourceData.getRDCreationDate();
	rdCreationTime = sourceData.getRDCreationTime();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

SPECTROSCOPY_INFO_CLASS::SPECTROSCOPY_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string type,string date, string time, DCM_VALUE_SQ_CLASS * refIESq,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	imageType = type;
	ssCreationDate = date;
	ssCreationTime = time;
	refImageEvidenceSeqValuePtr = refIESq;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

SPECTROSCOPY_INFO_CLASS::~SPECTROSCOPY_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool SPECTROSCOPY_INFO_CLASS::operator = (SPECTROSCOPY_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	imageType = sourceData.getImageType();
	ssCreationDate = sourceData.getSSCreationDate();
	ssCreationTime = sourceData.getSSCreationTime();
	refImageEvidenceSeqValuePtr = sourceData.getRefImageEvidenceSeqValue();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

SRDOC_INFO_CLASS::SRDOC_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string cflag,string vflag, string date, string time,string datetime,DCM_VALUE_SQ_CLASS * refCSSq,DCM_VALUE_SQ_CLASS * refSSq,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	compFlag = cflag;
	verFlag = vflag;
	contentDate = date;
	contentTime = time;
	verDateTime = datetime;
	conCodeSeqValuePtr = refCSSq;
	conSeqValuePtr = refSSq;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

SRDOC_INFO_CLASS::~SRDOC_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool SRDOC_INFO_CLASS::operator = (SRDOC_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	compFlag = sourceData.getCompFlag();
	verFlag = sourceData.getVerFlag();
	contentDate = sourceData.getContentDate();
	contentTime = sourceData.getContentTime();
	verDateTime = sourceData.getVerDateTime();
	conCodeSeqValuePtr = sourceData.getConCodeSeqValue();
	conSeqValuePtr = sourceData.getConSeqValue();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

KEY_OBJECT_DOC_INFO_CLASS::KEY_OBJECT_DOC_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string date, string time,DCM_VALUE_SQ_CLASS * refCSSq,DCM_VALUE_SQ_CLASS * refSSq,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	contentDate = date;
	contentTime = time;
	conCodeSeqValuePtr = refCSSq;
	conSeqValuePtr = refSSq;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

KEY_OBJECT_DOC_INFO_CLASS::~KEY_OBJECT_DOC_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool KEY_OBJECT_DOC_INFO_CLASS::operator = (KEY_OBJECT_DOC_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	contentDate = sourceData.getContentDate();
	contentTime = sourceData.getContentTime();
	conCodeSeqValuePtr = sourceData.getConCodeSeqValue();
	conSeqValuePtr = sourceData.getConSeqValue();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

VALUE_MAP_INFO_CLASS::VALUE_MAP_INFO_CLASS(vector<string>spCharSets,string date, string time,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	charSets = spCharSets;
	contentDate = date;
	contentTime = time;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

VALUE_MAP_INFO_CLASS::~VALUE_MAP_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool VALUE_MAP_INFO_CLASS::operator = (VALUE_MAP_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	charSets  = sourceData.getSpCharSetValues();
	contentDate = sourceData.getContentDate();
	contentTime = sourceData.getContentTime();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

FIDUCIAL_INFO_CLASS::FIDUCIAL_INFO_CLASS(vector<string>spCharSets,string date, string time,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	charSets = spCharSets;
	contentDate = date;
	contentTime = time;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

FIDUCIAL_INFO_CLASS::~FIDUCIAL_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool FIDUCIAL_INFO_CLASS::operator = (FIDUCIAL_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	charSets  = sourceData.getSpCharSetValues();
	contentDate = sourceData.getContentDate();
	contentTime = sourceData.getContentTime();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

REGISTRATION_INFO_CLASS::REGISTRATION_INFO_CLASS(vector<string>spCharSets,string date, string time,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	charSets = spCharSets;
	contentDate = date;
	contentTime = time;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

REGISTRATION_INFO_CLASS::~REGISTRATION_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool REGISTRATION_INFO_CLASS::operator = (REGISTRATION_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	charSets  = sourceData.getSpCharSetValues();
	contentDate = sourceData.getContentDate();
	contentTime = sourceData.getContentTime();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

RT_TREATMENT_INFO_CLASS::RT_TREATMENT_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string date, string time,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	rtTreatDate = date;
	rtTreatTime = time;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

RT_TREATMENT_INFO_CLASS::~RT_TREATMENT_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool RT_TREATMENT_INFO_CLASS::operator = (RT_TREATMENT_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	rtTreatDate = sourceData.getRTTreatDate();
	rtTreatTime = sourceData.getRTTreatTime();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

RT_PLAN_INFO_CLASS::RT_PLAN_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string lable,string date, string time,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	rtPlanLable = lable;
	rtPlanDate = date;
	rtPlanTime = time;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

RT_PLAN_INFO_CLASS::~RT_PLAN_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool RT_PLAN_INFO_CLASS::operator = (RT_PLAN_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	rtPlanLable = sourceData.getRTPlanLable();
	rtPlanDate = sourceData.getRTPlanDate();
	rtPlanTime = sourceData.getRTPlanTime();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

RT_STRUC_SET_INFO_CLASS::RT_STRUC_SET_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string lable,string date, string time,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	rtStrucSetLable = lable;
	rtStrucSetDate = date;
	rtStrucSetTime = time;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

RT_STRUC_SET_INFO_CLASS::~RT_STRUC_SET_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool RT_STRUC_SET_INFO_CLASS::operator = (RT_STRUC_SET_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	rtStrucSetLable = sourceData.getRTStrucSetLable();
	rtStrucSetDate = sourceData.getRTStrucSetDate();
	rtStrucSetTime = sourceData.getRTStrucSetTime();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

RT_DOSE_INFO_CLASS::RT_DOSE_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string dose, string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	doseSummType = dose;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

RT_DOSE_INFO_CLASS::~RT_DOSE_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool RT_DOSE_INFO_CLASS::operator = (RT_DOSE_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	doseSummType = sourceData.getDoseSummationType();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

HL7_SRDOC_INFO_CLASS::HL7_SRDOC_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string id,string time,string title,DCM_VALUE_SQ_CLASS * refCSSq,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	hl7InstanceIdentifier = id;
	hl7EffectiveTime = time;
	docTitle = title;
	hl7DocTypeCodeSeqPtr = refCSSq;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

HL7_SRDOC_INFO_CLASS::~HL7_SRDOC_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool HL7_SRDOC_INFO_CLASS::operator = (HL7_SRDOC_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	hl7InstanceIdentifier = sourceData.getHL7InstanceIdentifier();
	hl7EffectiveTime = sourceData.getHL7EffectiveTime();
	docTitle = sourceData.getDocTitle();
	hl7DocTypeCodeSeqPtr = sourceData.getHL7DocTypeCodeSeq();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

ENCAP_DOC_INFO_CLASS::ENCAP_DOC_INFO_CLASS(INT32 instanceNumber, vector<string>spCharSets,string date, string time,string id,string title,string mime,DCM_VALUE_SQ_CLASS * refCSSq,string Identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// Initialise class elements
	instanceNr = instanceNumber;
	charSets = spCharSets;
	contentDate = date;
	contentTime = time;
	hl7InstanceIdentifier = id;
	docTitle = title;
	varMIMETypeEncapDoc = mime;
	conNameCodeSeqValuePtr = refCSSq;
	identifier = Identifier;
	count = 1;
}

//>>===========================================================================

ENCAP_DOC_INFO_CLASS::~ENCAP_DOC_INFO_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

bool ENCAP_DOC_INFO_CLASS::operator = (ENCAP_DOC_INFO_CLASS& sourceData)

//  DESCRIPTION     : Operator assignment - for assigning this image to the 
//					  same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	instanceNr = atoi(sourceData.getInstanceNr().c_str());
	charSets  = sourceData.getSpCharSetValues();
	contentDate = sourceData.getContentDate();
	contentTime = sourceData.getContentTime();
	hl7InstanceIdentifier = sourceData.getHL7InstanceIdentifier();
	docTitle = sourceData.getDocTitle();
	varMIMETypeEncapDoc = sourceData.getMIMETypeEncapDoc();
	conNameCodeSeqValuePtr = sourceData.getConNameCodeSeqValue();
	identifier = sourceData.getIdentifier();
	count = sourceData.count;
	return true;
}

//>>===========================================================================

GENERATE_DICOMDIR_CLASS::GENERATE_DICOMDIR_CLASS(string resultRootDirectory)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{	
	dicomdirPathM = resultRootDirectory;
	isPSPresent = false;
	isImagePresent = false;
	isWaveFormPresent = false;
	isRawDataPresent = false;
	isSpectroscopyPresent = false;
	isSrDocPresent = false;
}

//>>===========================================================================

GENERATE_DICOMDIR_CLASS::~GENERATE_DICOMDIR_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// destructor activities    
}

//>>===========================================================================

bool GENERATE_DICOMDIR_CLASS::generateDICOMDIR(vector<string>* filenames)

//  DESCRIPTION     : Set the media filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
	bool result = false;

	if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_INFO, 2, "Reading the DCM files.");
    }

	// Read DCM files we only do this once
	vector<string>::iterator it;
	for (it = filenames->begin(); it < filenames->end(); ++it)
	{
        // successful reads are stored in filedatasetsM
		result = readDCMFiles(*it);		
	}

	if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_INFO, 2, "Building the DICOMDIR directory structure in the Warehouse.");
    }

	// Analyze all the successfully read DCM files and fill all necessary information
	vector<FMI_DATASET_STRUCT>::iterator datasetIt;
	for (datasetIt = filedatasetsM.begin(); datasetIt < filedatasetsM.end(); ++datasetIt)
	{
		// successful reads are stored in filedatasetsM
		analyseStorageDataset((*datasetIt).dat_ptr, (*datasetIt).filename, (*datasetIt).transferSyntax);
	}	

	// Provide identifier to each record(dataset)
	char base[10];
	string baseString;
	for (UINT i = 0; i < patientData.getSize(); i++)
	{
		PATIENT_INFO_CLASS* patientData_ptr = patientData[i];

		if (patientData_ptr != NULL)
		{
			_itoa(i,base,10);
			baseString = base;
			string identifier = "PATIENT" + baseString;
			patientData[i]->setIdentifier(identifier);
		}

		for (UINT j = 0; j < patientData_ptr->noStudies(); j++)
		{
			STUDY_INFO_CLASS* studyData_ptr = patientData_ptr->getStudyData(j);

			if (studyData_ptr != NULL)
			{
				_itoa((i*10)+j,base,10);
				baseString = base;
				string identifier = "STUDY" + baseString;
				studyData_ptr->setIdentifier(identifier);
			}

			/*HL7_SRDOC_INFO_CLASS* hl7StructDocData_ptr = patientData_ptr->getHL7StructDocData(j);

			if (hl7StructDocData_ptr != NULL)
			{
				_itoa((i*10)+j,base,10);
				baseString = base;
				string identifier = "HL7SRDOC" + baseString;
				hl7StructDocData_ptr->setIdentifier(identifier);
			}*/

			for (UINT k = 0; k < studyData_ptr->noSeries(); k++)
			{
				SERIES_INFO_CLASS* seriesData_ptr = studyData_ptr->getSeriesData(k);

				if (seriesData_ptr != NULL)
				{
					_itoa((((i*10)+j)*10)+k,base,10);
					baseString = base;
					string identifier = "SERIES" + baseString;
					seriesData_ptr->setIdentifier(identifier);
				}

				for (UINT l = 0; l < seriesData_ptr->noSopInstances(); l++)
				{
					IMAGE_INFO_CLASS* imageData_ptr = seriesData_ptr->getSopInstanceData(l);

					if (imageData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+l,base,10);
						baseString = base;
						string identifier = "IMAGE" + baseString;
						imageData_ptr->setIdentifier(identifier);
					}					
				}

				// Check if Presentation Data is present, if it presents provide 
				// identifier to PS record.
				for (UINT m = 0; (m < seriesData_ptr->noPresentationStates()) ; m++)
				{
					PRESENTATION_STATE_INFO_CLASS* psData_ptr = seriesData_ptr->getPSData(m);
					if (psData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "PRESENTATION" + baseString;
						psData_ptr->setIdentifier(identifier);
					}
				}

				// Check if Waveform Data is present, if it presents provide 
				// identifier to Waveform record.
				for (UINT m = 0; (m < seriesData_ptr->noWaveforms()) ; m++)
				{
					WAVEFORM_INFO_CLASS* wfData_ptr = seriesData_ptr->getWFData(m);
					if (wfData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "WAVEFORM" + baseString;
						wfData_ptr->setIdentifier(identifier);
					}
				}

				// Check if Raw Data is present, if it presents provide 
				// identifier to Raw Data record.
				for (UINT m = 0; (m < seriesData_ptr->noRawDatas()) ; m++)
				{
					RAWDATA_INFO_CLASS* rawData_ptr = seriesData_ptr->getRawData(m);
					if (rawData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "RAWDATA" + baseString;
						rawData_ptr->setIdentifier(identifier);
					}
				}

				// Check if Spectroscopy Data is present, if it presents provide 
				// identifier to Spectroscopy record.
				for (UINT m = 0; (m < seriesData_ptr->noSpectDatas()) ; m++)
				{
					SPECTROSCOPY_INFO_CLASS* spectData_ptr = seriesData_ptr->getSpectData(m);
					if (spectData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "SPECTROSCOPY" + baseString;
						spectData_ptr->setIdentifier(identifier);
					}
				}

				// Check if SR Doc Data is present, if it presents provide 
				// identifier to SR Doc record.
				for (UINT m = 0; (m < seriesData_ptr->noSRDocDatas()) ; m++)
				{
					SRDOC_INFO_CLASS* srData_ptr = seriesData_ptr->getSRDocData(m);
					if (srData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "SRDOC" + baseString;
						srData_ptr->setIdentifier(identifier);
					}
				}

				// Check if Encapsulated Doc Data is present, if it presents provide 
				// identifier to Encapsulated Doc record.
				for (UINT m = 0; (m < seriesData_ptr->noEncapDocDatas()) ; m++)
				{
					ENCAP_DOC_INFO_CLASS* encapData_ptr = seriesData_ptr->getEncapDocData(m);
					if (encapData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "ENCAPDOC" + baseString;
						encapData_ptr->setIdentifier(identifier);
					}
				}

				// Check if Registration data is present, if it presents provide 
				// identifier to Registration record.
				for (UINT m = 0; (m < seriesData_ptr->noRegistrationDatas()) ; m++)
				{
					REGISTRATION_INFO_CLASS* regData_ptr = seriesData_ptr->getRegistrationData(m);
					if (regData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "REGISTRATION" + baseString;
						regData_ptr->setIdentifier(identifier);
					}
				}

				// Check if Fiducial Data is present, if it presents provide 
				// identifier to Fiducial record.
				for (UINT m = 0; (m < seriesData_ptr->noFiducialDatas()) ; m++)
				{
					FIDUCIAL_INFO_CLASS* fiducialData_ptr = seriesData_ptr->getFiducialData(m);
					if (fiducialData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "FIDUCIAL" + baseString;
						fiducialData_ptr->setIdentifier(identifier);
					}
				}

				// Check if RT Dose Data is present, if it presents provide 
				// identifier to RT Dose record.
				for (UINT m = 0; (m < seriesData_ptr->noRTDoseDatas()) ; m++)
				{
					RT_DOSE_INFO_CLASS* rtDoseData_ptr = seriesData_ptr->getRTDoseData(m);
					if (rtDoseData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "RTDOSE" + baseString;
						rtDoseData_ptr->setIdentifier(identifier);
					}
				}

				// Check if RT Struct Set Data is present, if it presents provide 
				// identifier to RT Struct Set record.
				for (UINT m = 0; (m < seriesData_ptr->noRTStructSetDatas()) ; m++)
				{
					RT_STRUC_SET_INFO_CLASS* rtStructSetData_ptr = seriesData_ptr->getRTStructSetData(m);
					if (rtStructSetData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "RTSTRUCTSET" + baseString;
						rtStructSetData_ptr->setIdentifier(identifier);
					}
				}

				// Check if RT Plan Data is present, if it presents provide 
				// identifier to RT Plan record.
				for (UINT m = 0; (m < seriesData_ptr->noRTPlanDatas()) ; m++)
				{
					RT_PLAN_INFO_CLASS* rtPlanData_ptr = seriesData_ptr->getRTPlanData(m);
					if (rtPlanData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "RTPLAN" + baseString;
						rtPlanData_ptr->setIdentifier(identifier);
					}
				}

				// Check if RT Treatment Data is present, if it presents provide 
				// identifier to RT Treatment record.
				for (UINT m = 0; (m < seriesData_ptr->noRTTreatDatas()) ; m++)
				{
					RT_TREATMENT_INFO_CLASS* rtTreatData_ptr = seriesData_ptr->getRTTreatData(m);
					if (rtTreatData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "RTTREATMENT" + baseString;
						rtTreatData_ptr->setIdentifier(identifier);
					}
				}

				// Check if Value Map Data is present, if it presents provide 
				// identifier to Value Map record.
				for (UINT m = 0; (m < seriesData_ptr->noValueMapDatas()) ; m++)
				{
					VALUE_MAP_INFO_CLASS* valueMapData_ptr = seriesData_ptr->getValueMapData(m);
					if (valueMapData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "VALUEMAP" + baseString;
						valueMapData_ptr->setIdentifier(identifier);
					}
				}

				// Check if Key object Data is present, if it presents provide 
				// identifier to Key object record.
				for (UINT m = 0; (m < seriesData_ptr->noKeyobjectDatas()) ; m++)
				{
					KEY_OBJECT_DOC_INFO_CLASS* KeyobjData_ptr = seriesData_ptr->getKeyobjectData(m);
					if (KeyobjData_ptr != NULL)
					{
						_itoa((((((i*10)+j)*10)+k)*10)+m,base,10);
						baseString = base;
						string identifier = "KEYOBJECT" + baseString;
						KeyobjData_ptr->setIdentifier(identifier);
					}
				}
			}
		}
	}

	for (UINT i = 0; i < hangingProtocolData.getSize(); i++)
	{
		HANGING_PROTOCOL_INFO_CLASS* hangingProtocolData_ptr = hangingProtocolData[i];

		if (hangingProtocolData_ptr != NULL)
		{
			_itoa(i,base,10);
			baseString = base;
			string identifier = "HANGINGPROTOCOL" + baseString;
			hangingProtocolData[i]->setIdentifier(identifier);
		}
	}

	result = CreateDICOMObjects();
	
	if(result)
	{
		result = CreateAndStoreRecords();
	}
	else
	{
		if (loggerM_ptr)
		{
    		loggerM_ptr->text(LOG_ERROR, 1, "Error in creating DICOMDIR Header, FMI.");
		}
		return false;
	}

	if(result)
	{
		result = CreateAndStoreDirectorySequenceObject();
	}
	else
	{
		if (loggerM_ptr)
		{
    		loggerM_ptr->text(LOG_ERROR, 1, "Error in creating Records.");
		}
		return false;
	}

	if(result)
	{
		result = writeDICOMDIR((dicomdirPathM + "DICOMDIR"));
	}
	else
	{
		if (loggerM_ptr)
		{
    		loggerM_ptr->text(LOG_ERROR, 1, "Error in creating Directory Sequence Object.");
		}
		return false;
	}

	if(result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 2, "DICOMDIR created sucessfully and it is available in directory: \"%s\".", dicomdirPathM.c_str());
		}
	}

	return result;
}

//>>===========================================================================

bool GENERATE_DICOMDIR_CLASS::readDCMFiles(string filename)

//  DESCRIPTION     : Reads a file, and add pointers to the fmi and
//                    the dataset to the filedataset list. 
//                    Optionally the file is validated
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// create local file object 
	FILE_DATASET_CLASS fileDataset(filename);

	// cascade the logger 
	fileDataset.setLogger(loggerM_ptr);

	// log action
    if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_INFO, 2, "Parsing the DCM file: \"%s\"", filename.c_str());
    }

	result = fileDataset.read();
	if (!result)
	{
		if (loggerM_ptr)
		{
    		loggerM_ptr->text(LOG_ERROR, 1, "Error in reading DCM file %s ", filename.c_str());
		}
		return false;
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
    		loggerM_ptr->text(LOG_INFO, 1, "Media file dataset was encoded with Transfer Syntax: \"%s\"", file_info.transferSyntax.c_str());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

PATIENT_INFO_CLASS *GENERATE_DICOMDIR_CLASS::searchPatient(string id, string name)

//  DESCRIPTION     : Search the Patient List for Patient Data with an id (and
//					: optional name) matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// search patient data
	for (UINT i = 0; i < patientData.getSize(); i++)
	{
		PATIENT_INFO_CLASS *patientData_ptr = patientData[i];

		// check for match
		if ((patientData_ptr != NULL) &&
			(id == patientData_ptr->getPatientId()))
		{
			// check if name defined
			if ((name.length() != 0) &&
				(patientData_ptr->getPatientName().length() != 0) &&
				(name != patientData_ptr->getPatientName()))
			{
				// patient ids match - but names do not
				// - issue warning
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_WARNING, 1,"(0010,0010) Patient Name mis-match during Object Relationship Analysis: \"%s\" and \"%s\"", name.c_str(), patientData_ptr->getPatientName().c_str());
				}
			}

			// match on Patient Id
			return patientData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

HANGING_PROTOCOL_INFO_CLASS *GENERATE_DICOMDIR_CLASS::searchHangingProtocol(string name)

//  DESCRIPTION     : Search the Hanging Protocol record List for Hanging Protocol 
//						Data with name matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// search Hanging Protocol data
	for (UINT i = 0; i < hangingProtocolData.getSize(); i++)
	{
		HANGING_PROTOCOL_INFO_CLASS *hangingProtocol_ptr = hangingProtocolData[i];

		// check for match
		if ((hangingProtocol_ptr != NULL) &&
			(name == hangingProtocol_ptr->getHangingProtoName()))
		{
			// match on Hanging Protocol name
			return hangingProtocol_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

void GENERATE_DICOMDIR_CLASS::analyseStorageDataset(DCM_DATASET_CLASS* dataset_ptr, string fileName, string transferSyntax)

//  DESCRIPTION     : Analyse the DCM dataset. The identification information 
//					: will be used to check whether any relationship exists
//					: between this and previous/future objects.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	PATIENT_INFO_CLASS *patientData_ptr = NULL;
	//HANGING_PROTOCOL_INFO_CLASS *hangingProtocol_ptr = NULL;
	STUDY_INFO_CLASS *studyData_ptr = NULL;
	//HL7_SRDOC_INFO_CLASS* hl7StructDocData_ptr = NULL;
	SERIES_INFO_CLASS *seriesData_ptr = NULL;
	PRESENTATION_STATE_INFO_CLASS *psData_ptr = NULL;
	IMAGE_INFO_CLASS *sopInstanceData_ptr = NULL;
	WAVEFORM_INFO_CLASS *wfData_ptr = NULL;
	RAWDATA_INFO_CLASS *rawData_ptr = NULL;
	SPECTROSCOPY_INFO_CLASS *ssData_ptr = NULL;
	SRDOC_INFO_CLASS *srDocData_ptr = NULL;
	ENCAP_DOC_INFO_CLASS* encapData_ptr = NULL;
	REGISTRATION_INFO_CLASS* regData_ptr = NULL;
	FIDUCIAL_INFO_CLASS* fiducialData_ptr = NULL;
	RT_DOSE_INFO_CLASS* rtDoseData_ptr = NULL;
	RT_STRUC_SET_INFO_CLASS* rtStructSetData_ptr = NULL;
	RT_PLAN_INFO_CLASS* rtPlanData_ptr = NULL;
	RT_TREATMENT_INFO_CLASS* rtTreatData_ptr = NULL;
	VALUE_MAP_INFO_CLASS* valueMapData_ptr = NULL;
	KEY_OBJECT_DOC_INFO_CLASS* KeyobjData_ptr = NULL;

	isPSPresent = false;
	isImagePresent = false;
	isWaveFormPresent = false;
	isRawDataPresent = false;
	isSpectroscopyPresent = false;
	isSrDocPresent = false;
	isRTDosePresent = false;
	isRTStructSetPresent = false;
	isRTPlanPresent = false;
	isRTTreatPresent = false;
	isEncapDocPresent = false;
	isRegistrationPresent = false;
	isFiducialPresent = false;
	isKeyObjDocPresent = false;
	isValueMapPresent = false;
	
	// check that the appriopriate attributes are available
	// Patient ID
	string patientId;
	dataset_ptr->getLOValue(TAG_PATIENT_ID, patientId);
	if (patientId.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0010,0020) Patient ID is not available");
		}
	}

	// Patient Name
	string patientName;
	dataset_ptr->getPNValue(TAG_PATIENTS_NAME, patientName);
	if (patientName.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0010,0010) Patient Name is not available");
		}
	}

	// Study Instance UID
	string studyInstanceUid = "";
	dataset_ptr->getUIValue(TAG_STUDY_INSTANCE_UID, studyInstanceUid);
	if (studyInstanceUid.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0020,000D) Study Instance UID is not available");
		}
	}

	// Study ID
	string studyId;
	dataset_ptr->getSHValue(TAG_STUDY_ID, studyId);
	if (studyId.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0020,0010) Study ID is not available");
		}
	}

	// Study Description
	string studyDescr;
	dataset_ptr->getLOValue(TAG_STUDY_DESCRIPTION, studyDescr);
	if (studyDescr.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0008,1030) Study Description is not available");
		}
	}

	// Accession Nr
	string accessionNr;
	dataset_ptr->getSHValue(TAG_ACCESSION_NUMBER, accessionNr);
	if (accessionNr.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0008,0050) Accession Nr is not available");
		}
	}

	// Study Date
	char study_date[DA_LENGTH + 1];
	study_date[0] = NULLCHAR;
	dataset_ptr->getDAValue(TAG_STUDY_DATE, (BYTE*)study_date, DA_LENGTH);
	string studyDate = study_date;
	if (studyDate.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0008,0020) Study Date is not available");
		}
	}

	// Study Time
	char study_time[TM_LENGTH + 1];
	study_time[0] = NULLCHAR;
	dataset_ptr->getTMValue(TAG_STUDY_TIME, (BYTE*)study_time, TM_LENGTH);
	string studyTime = study_time;
	if (studyTime.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0008,0030) Study Time is not available");
		}
	}

	// Series Instance UID
	string seriesInstanceUid = "";
	dataset_ptr->getUIValue(TAG_SERIES_INSTANCE_UID, seriesInstanceUid);
	if (seriesInstanceUid.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0020,000E) Series Instance UID is not available");
		}
	}

	// Series Modality
	string seriesModality;
	dataset_ptr->getCSValue(TAG_MODALITY, seriesModality);
	if (seriesModality.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0008,0060) Series Modality is not available");
		}
	}

	// Specific character sets
	vector<string> spCharSetsValues;

	// get the Specific character set attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = dataset_ptr->GetAttributeByTag(TAG_SPECIFIC_CHARACTER_SET);
	if(attribute_ptr != NULL)
	{
		for(int i=0; i< attribute_ptr->GetNrValues(); i++)
		{
			string value;
			dataset_ptr->getCSValue(TAG_SPECIFIC_CHARACTER_SET, value, i);
			spCharSetsValues.push_back(value);
		}
	}		
	
	// Series Nr
	INT32 seriesNr = -1;
	dataset_ptr->getISValue(TAG_SERIES_NUMBER, &seriesNr);
	if (seriesNr == -1)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0020,0011) Series number is not available");
		}
	}

	// Object SOP Class UID
	string sopClassUid;
	dataset_ptr->getUIValue(TAG_SOP_CLASS_UID, sopClassUid);
	if (sopClassUid.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0008,0016) SOP Class UID is not available");
		}
	}
	
	// Object SOP Instance UID
	string sopInstanceUid;
	dataset_ptr->getUIValue(TAG_SOP_INSTANCE_UID, sopInstanceUid);
	if (sopInstanceUid.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0008,0018) SOP (Image) Instance UID is not available");
		}
	}

	// Instance Nr
	INT32 instanceNr = -1;
	dataset_ptr->getISValue(TAG_IMAGE_NUMBER, &instanceNr);
	if (instanceNr == -1)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0020,0013) Instance number is not available");
		}
	}

	// Image type
	string imageType;
	dataset_ptr->getCSValue(TAG_IMAGE_TYPE, imageType);
	if (imageType.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0008,0008) Image type is not available");
		}
	}

	// Rows
	UINT16 rows = 0;
	dataset_ptr->getUSValue(TAG_ROWS, &rows);
	if (rows == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0028,0010) Rows is not available");
		}
	}

	// Columns
	UINT16 columns = 0;
	dataset_ptr->getUSValue(TAG_COLUMNS, &columns);
	if (columns == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0028,0011) Columns is not available");
		}
	}

	bool isPixelDataPresent = dataset_ptr->containsAttributesFromGroup(0x7FE0);

	//Check for presence of Image
	if( isPixelDataPresent && 
	    ((rows != 0) &&
	    (columns != 0)))
	{
		isImagePresent = true;
	}

	// Content Lable
	string contLable;
	dataset_ptr->getCSValue(TAG_CONTENT_LABLE, contLable);
	if (contLable.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0070,0080) Content Lable is not available");
		}
	}

	// Content Description
	string contDesc;
	dataset_ptr->getLOValue(TAG_CONTENT_DESCRIPTION, contDesc);
	if (contDesc.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0070,0081) Content Description is not available");
		}
	}

	// Content Creator name
	string contCreator;
	dataset_ptr->getPNValue(TAG_CONTENT_CREATORNAME, contCreator);
	if (contCreator.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0070,0084) Content Creator name is not available");
		}
	}

	// PS Creation Date
	char ps_date[DA_LENGTH + 1];
	ps_date[0] = NULLCHAR;
	dataset_ptr->getDAValue(TAG_PS_CREATION_DATE, (BYTE*)ps_date, DA_LENGTH);
	string psDate = ps_date;
	if (psDate.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0070,0082) PS Creation Date is not available");
		}
	}

	// PS Creation Time
	char ps_time[TM_LENGTH + 1];
	ps_time[0] = NULLCHAR;
	dataset_ptr->getTMValue(TAG_PS_CREATION_TIME, (BYTE*)ps_time, TM_LENGTH);
	string psTime = ps_time;
	if (psTime.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(0070,0083) PS Creation Time is not available");
		}
	}

	//Referenced Series Sequence
	DCM_VALUE_SQ_CLASS *refSeriesSeqValuePtr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
	dataset_ptr->getSQValue(TAG_REFERENCED_SERIES_SEQUENCE, &refSeriesSeqValuePtr);

	//Check for presence of Presentation state SOPs
	if((contLable.length() != 0) || 
	   (refSeriesSeqValuePtr->GetNrItems() != 0) ||
	   (psDate.length() != 0) ||
	   (psTime.length() != 0))
	{
		isPSPresent = true;
	}

	string dataDate;
	string dataTime;
	// Data Creation Date
	char data_date[DA_LENGTH + 1];
	data_date[0] = NULLCHAR;
	dataset_ptr->getDAValue(TAG_IMAGE_DATE, (BYTE*)data_date, DA_LENGTH);
	dataDate = data_date;
	
	// Data Creation Time
	char data_time[TM_LENGTH + 1];
	data_time[0] = NULLCHAR;
	dataset_ptr->getTMValue(TAG_IMAGE_TIME, (BYTE*)data_time, TM_LENGTH);
	dataTime = data_time;
	
	//Check for presence of Waveform SOPs
	if((sopClassUid == Waveform_Storage_SOP_CLASS_UID) ||
	   (sopClassUid == General_ECG_Waveform_Storage_SOP_CLASS_UID) ||
	   (sopClassUid == Ambulatory_ECG_Waveform_Storage_SOP_CLASS_UID) ||
	   (sopClassUid == Basic_Cardiac_ECG_Waveform_Storage_SOP_CLASS_UID) ||
	   (sopClassUid == Basic_Voice_Audio_Waveform_Storage_SOP_CLASS_UID) ||
	   (sopClassUid == Hemodynamic_Waveform_Storage_SOP_CLASS_UID))
	{
		isWaveFormPresent = true;		

		wfData_ptr = new WAVEFORM_INFO_CLASS(instanceNr, spCharSetsValues, dataDate, dataTime, "");
	}

	//Check for presence of Raw Data SOP
	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.66")
	{
		isRawDataPresent = true;		

		rawData_ptr = new RAWDATA_INFO_CLASS(instanceNr, spCharSetsValues, dataDate, dataTime, "");
	}

	//Check for presence of Spectroscopy SOP
	DCM_VALUE_SQ_CLASS *refImageEvidenceSeqValuePtr = NULL;
	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.4.2")
	{
		isSpectroscopyPresent = true;

		//Referenced Series Sequence
		refImageEvidenceSeqValuePtr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
		dataset_ptr->getSQValue(0x00089092, &refImageEvidenceSeqValuePtr);

		ssData_ptr = new SPECTROSCOPY_INFO_CLASS(instanceNr, spCharSetsValues, imageType, dataDate, dataTime, refImageEvidenceSeqValuePtr, "");
	}

	//Check for presence of SR Doc SOP
	string compFlag;
	string verFlag;
	string verDT;
	DCM_VALUE_SQ_CLASS *conNameSeqValuePtr = NULL;
	DCM_VALUE_SQ_CLASS *conSeqValuePtr = NULL;

	//Concept name code Sequence
	conNameSeqValuePtr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
	dataset_ptr->getSQValue(0x0040A043, &conNameSeqValuePtr);

	//Content Sequence
	conSeqValuePtr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
	dataset_ptr->getSQValue(0x0040A730, &conSeqValuePtr);

	if((sopClassUid == "1.2.840.10008.5.1.4.1.1.88.11") ||
	   (sopClassUid == "1.2.840.10008.5.1.4.1.1.88.22") ||
	   (sopClassUid == "1.2.840.10008.5.1.4.1.1.88.33"))
	{
		isSrDocPresent = true;

		// Completion flag
		dataset_ptr->getCSValue(0x0040A491, compFlag);
		
		// Verification flag
		dataset_ptr->getCSValue(0x0040A493, verFlag);

		// Verification date time
		dataset_ptr->getCSValue(0x0040A030, verDT);		

		srDocData_ptr = new SRDOC_INFO_CLASS(instanceNr, spCharSetsValues, compFlag, verFlag, dataDate, dataTime, verDT,conNameSeqValuePtr, conSeqValuePtr,"");
	}

	// Dose summation type
	string doseSumType;
	dataset_ptr->getCSValue(TAG_DOSE_SUMMATION_TYPE, doseSumType);
	if (doseSumType.length() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_IMAGE_RELATION, 1, "(3004,000A) Dose summation type is not available");
		}
	}

	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.481.2")
	{
		isRTDosePresent = true;
		rtDoseData_ptr = new RT_DOSE_INFO_CLASS(instanceNr, spCharSetsValues, doseSumType,"");
	}

	// RT Structure Set Label
	string rtStrucLable;
	dataset_ptr->getSHValue(TAG_STRUCTURE_SET_LABEL, rtStrucLable);
	
	string rtStrucDate;
	string rtStrucTime;
	// RT Structure Set Date
	char rtStruc_Date[DA_LENGTH + 1];
	rtStruc_Date[0] = NULLCHAR;
	dataset_ptr->getDAValue(TAG_STRUCTURE_SET_DATE, (BYTE*)rtStruc_Date, DA_LENGTH);
	rtStrucDate = rtStruc_Date;
	
	// RT Structure Set Time
	char rtStruc_Time[TM_LENGTH + 1];
	rtStruc_Time[0] = NULLCHAR;
	dataset_ptr->getTMValue(TAG_STRUCTURE_SET_TIME, (BYTE*)rtStruc_Time, TM_LENGTH);
	rtStrucTime = rtStruc_Time;

	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.481.3")
	{
		isRTStructSetPresent = true;
		rtStructSetData_ptr = new RT_STRUC_SET_INFO_CLASS(instanceNr, spCharSetsValues, rtStrucLable,rtStrucDate,rtStrucTime,"");
	}

	// Structure Set Label
	string rtPlanLable;
	dataset_ptr->getSHValue(TAG_RT_PLAN_LABEL, rtPlanLable);
	
	string rtPlanDate;
	string rtPlanTime;
	// Structure Set Date
	char rtPlan_Date[DA_LENGTH + 1];
	rtPlan_Date[0] = NULLCHAR;
	dataset_ptr->getDAValue(TAG_RT_PLAN_DATE, (BYTE*)rtPlan_Date, DA_LENGTH);
	rtPlanDate = rtPlan_Date;
	
	// Structure Set Time
	char rtPlan_Time[TM_LENGTH + 1];
	rtPlan_Time[0] = NULLCHAR;
	dataset_ptr->getTMValue(TAG_RT_PLAN_TIME, (BYTE*)rtPlan_Time, TM_LENGTH);
	rtPlanTime = rtPlan_Time;

	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.481.5")
	{
		isRTPlanPresent = true;
		rtPlanData_ptr = new RT_PLAN_INFO_CLASS(instanceNr, spCharSetsValues, rtPlanLable,rtPlanDate,rtPlanTime,"");
	}

	string rtTreatDate;
	string rtTreatTime;
	// Structure Set Date
	char rtTreat_Date[DA_LENGTH + 1];
	rtTreat_Date[0] = NULLCHAR;
	dataset_ptr->getDAValue(TAG_RT_TREATMENT_DATE, (BYTE*)rtTreat_Date, DA_LENGTH);
	rtTreatDate = rtTreat_Date;
	
	// Structure Set Time
	char rtTreat_Time[TM_LENGTH + 1];
	rtTreat_Time[0] = NULLCHAR;
	dataset_ptr->getTMValue(TAG_RT_TREATMENT_TIME, (BYTE*)rtTreat_Time, TM_LENGTH);
	rtTreatTime = rtTreat_Time;

	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.481.7")
	{
		isRTTreatPresent = true;
		rtTreatData_ptr = new RT_TREATMENT_INFO_CLASS(instanceNr, spCharSetsValues, rtTreatDate,rtTreatTime,"");
	}

	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.66.1")
	{
		isRegistrationPresent = true;
		regData_ptr = new REGISTRATION_INFO_CLASS(spCharSetsValues, dataDate,dataTime,"");
	}

	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.66.2")
	{
		isFiducialPresent = true;
		fiducialData_ptr = new FIDUCIAL_INFO_CLASS(spCharSetsValues, dataDate,dataTime,"");
	}

	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.67")
	{
		isValueMapPresent = true;
		valueMapData_ptr = new VALUE_MAP_INFO_CLASS(spCharSetsValues, dataDate,dataTime,"");
	}

	// Document Title
	string docTitle;
	char doc_Title[ST_LENGTH + 1];
	doc_Title[0] = NULLCHAR;
	dataset_ptr->getSTValue(TAG_DOC_TITLE, (BYTE*)doc_Title, ST_LENGTH);
	docTitle = doc_Title;

	// HL7 Instance Identifier
	string hl7InstId;
	char hl7_Instance_Id[ST_LENGTH + 1];
	hl7_Instance_Id[0] = NULLCHAR;
	dataset_ptr->getSTValue(TAG_HL7_INSTANCE_IDENTIFIER, (BYTE*)hl7_Instance_Id, ST_LENGTH);
	hl7InstId = hl7_Instance_Id;

	string mimeType;
	dataset_ptr->getLOValue(TAG_MIME_TYPE_ENCAP_DOC, mimeType);

	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.104.1")
	{
		isEncapDocPresent = true;
		encapData_ptr = new ENCAP_DOC_INFO_CLASS(instanceNr,spCharSetsValues, dataDate,dataTime,hl7InstId,docTitle,mimeType,conNameSeqValuePtr,"");
	}

	if(sopClassUid == "1.2.840.10008.5.1.4.1.1.88.59")
	{
		isKeyObjDocPresent = true;
		KeyobjData_ptr = new KEY_OBJECT_DOC_INFO_CLASS(instanceNr,spCharSetsValues, dataDate,dataTime,conNameSeqValuePtr,conSeqValuePtr,"");
	}

	// Reference File ID
	int prefix_pos = dicomdirPathM.length();
	int fileLength = fileName.length() - 1;
	string refFileId;

	// Extract the Reference file name
    if ((prefix_pos > 0) && (fileLength > prefix_pos))
    {
        refFileId = fileName.substr (prefix_pos, fileLength);
    }

	// Reference Transfer Syntax UID
	string refTSUid = transferSyntax;
	
	// now check if a matching Patient has already been set up
	if ((patientData_ptr = searchPatient(patientId, patientName)) == NULL)
	{
		// create Presentation Data if it present
		if(isPSPresent)
		{
			psData_ptr = new PRESENTATION_STATE_INFO_CLASS(instanceNr, refFileId, sopClassUid, sopInstanceUid, refTSUid, refSeriesSeqValuePtr,psDate, psTime, contLable,contDesc,contCreator, "");
		}

		// create SOP Instance Data if it present
		if(isImagePresent) 
		{
			sopInstanceData_ptr = new IMAGE_INFO_CLASS(sopInstanceUid, refFileId, sopClassUid, sopInstanceUid, refTSUid, instanceNr, spCharSetsValues, "");
		}

		// create Series Data
		seriesData_ptr = new SERIES_INFO_CLASS(seriesInstanceUid, seriesModality,seriesNr, "");

		// add SOP Instance Data
		if(sopInstanceData_ptr != NULL)
		{
			seriesData_ptr->addSopInstanceData(sopInstanceData_ptr);
		}
		
		// add Presentation Data
		if(psData_ptr != NULL)
		{
			seriesData_ptr->addPSData(psData_ptr);
		}

		if(wfData_ptr != NULL)
		{
			seriesData_ptr->addWFData(wfData_ptr);
		}

		if(rawData_ptr != NULL)
		{
			seriesData_ptr->addRawData(rawData_ptr);
		}

		if(ssData_ptr != NULL)
		{
			seriesData_ptr->addSpectData(ssData_ptr);
		}

		if(srDocData_ptr != NULL)
		{
			seriesData_ptr->addSRDocData(srDocData_ptr);
		}

		if(rtDoseData_ptr != NULL)
		{
			seriesData_ptr->addRTDoseData(rtDoseData_ptr);
		}

		if(rtPlanData_ptr != NULL)
		{
			seriesData_ptr->addRTPlanData(rtPlanData_ptr);
		}

		if(rtStructSetData_ptr != NULL)
		{
			seriesData_ptr->addRTStructSetData(rtStructSetData_ptr);
		}

		if(rtTreatData_ptr != NULL)
		{
			seriesData_ptr->addRTTreatData(rtTreatData_ptr);
		}

		if(encapData_ptr != NULL)
		{
			seriesData_ptr->addEncapDocData(encapData_ptr);
		}

		if(fiducialData_ptr != NULL)
		{
			seriesData_ptr->addFiducialData(fiducialData_ptr);
		}

		if(KeyobjData_ptr != NULL)
		{
			seriesData_ptr->addKeyobjectData(KeyobjData_ptr);
		}

		if(regData_ptr != NULL)
		{
			seriesData_ptr->addRegistrationData(regData_ptr);
		}

		if(valueMapData_ptr != NULL)
		{
			seriesData_ptr->addValueMapData(valueMapData_ptr);
		}
	
		// create Study Data
		studyData_ptr = new STUDY_INFO_CLASS(studyInstanceUid, studyId, studyDate, studyTime, studyDescr, accessionNr,"");
		studyData_ptr->addSeriesData(seriesData_ptr);

		// create Patient Data
		patientData_ptr = new PATIENT_INFO_CLASS(patientId, patientName, spCharSetsValues,"");
		patientData_ptr->addStudyData(studyData_ptr);

		// add Patient Data
		patientData.add(patientData_ptr);
	}
	else 
	{
		// patient already exists - check for study
		if ((studyData_ptr = patientData_ptr->searchStudy(studyInstanceUid)) == NULL) 
		{
			// create Presentation Data if it present
			if(isPSPresent)
			{
				psData_ptr = new PRESENTATION_STATE_INFO_CLASS(instanceNr, refFileId, sopClassUid, sopInstanceUid, refTSUid, refSeriesSeqValuePtr, psDate, psTime, contLable,contDesc,contCreator, "");
			}

			// create SOP Instance Data if it present
			if(isImagePresent)
			{
				sopInstanceData_ptr = new IMAGE_INFO_CLASS(sopInstanceUid, refFileId, sopClassUid, sopInstanceUid, refTSUid, instanceNr, spCharSetsValues, "");
			}

			// create Series Data
			seriesData_ptr = new SERIES_INFO_CLASS(seriesInstanceUid, seriesModality,seriesNr, "");

			// add SOP Instance/Presentation Data
			if(sopInstanceData_ptr != NULL)
			{
				seriesData_ptr->addSopInstanceData(sopInstanceData_ptr);
			}
			if(psData_ptr != NULL)
			{
				seriesData_ptr->addPSData(psData_ptr);
			}

			if(wfData_ptr != NULL)
			{
				seriesData_ptr->addWFData(wfData_ptr);
			}

			if(rawData_ptr != NULL)
			{
				seriesData_ptr->addRawData(rawData_ptr);
			}

			if(ssData_ptr != NULL)
			{
				seriesData_ptr->addSpectData(ssData_ptr);
			}

			if(srDocData_ptr != NULL)
			{
				seriesData_ptr->addSRDocData(srDocData_ptr);
			}

			if(rtDoseData_ptr != NULL)
			{
				seriesData_ptr->addRTDoseData(rtDoseData_ptr);
			}

			if(rtPlanData_ptr != NULL)
			{
				seriesData_ptr->addRTPlanData(rtPlanData_ptr);
			}

			if(rtStructSetData_ptr != NULL)
			{
				seriesData_ptr->addRTStructSetData(rtStructSetData_ptr);
			}

			if(rtTreatData_ptr != NULL)
			{
				seriesData_ptr->addRTTreatData(rtTreatData_ptr);
			}

			if(encapData_ptr != NULL)
			{
				seriesData_ptr->addEncapDocData(encapData_ptr);
			}

			if(fiducialData_ptr != NULL)
			{
				seriesData_ptr->addFiducialData(fiducialData_ptr);
			}

			if(KeyobjData_ptr != NULL)
			{
				seriesData_ptr->addKeyobjectData(KeyobjData_ptr);
			}

			if(regData_ptr != NULL)
			{
				seriesData_ptr->addRegistrationData(regData_ptr);
			}

			if(valueMapData_ptr != NULL)
			{
				seriesData_ptr->addValueMapData(valueMapData_ptr);
			}
		
			// create Study Data
			studyData_ptr = new STUDY_INFO_CLASS(studyInstanceUid, studyId, studyDate, studyTime, studyDescr, accessionNr,"");
			studyData_ptr->addSeriesData(seriesData_ptr);

			// add Study Data
			patientData_ptr->addStudyData(studyData_ptr);
		}
		else 
		{
			// study already exists - check for series
			if ((seriesData_ptr = studyData_ptr->searchSeries(seriesInstanceUid)) == NULL)
			{
				// create Presentation Data if it present
				if(isPSPresent)
				{
					psData_ptr = new PRESENTATION_STATE_INFO_CLASS(instanceNr, refFileId, sopClassUid, sopInstanceUid, refTSUid, refSeriesSeqValuePtr,psDate, psTime, contLable,contDesc,contCreator, "");
				}

				// create SOP Instance Data if it present
				if(isImagePresent)
				{
					sopInstanceData_ptr = new IMAGE_INFO_CLASS(sopInstanceUid, refFileId, sopClassUid, sopInstanceUid, refTSUid, instanceNr, spCharSetsValues, "");
				}

				// create Series Data
				seriesData_ptr = new SERIES_INFO_CLASS(seriesInstanceUid, seriesModality,seriesNr, "");

				// add SOP Instance/Presentation Data
				if(sopInstanceData_ptr != NULL)
				{
					seriesData_ptr->addSopInstanceData(sopInstanceData_ptr);
				}
				if(psData_ptr != NULL)
				{
					seriesData_ptr->addPSData(psData_ptr);
				}

				if(wfData_ptr != NULL)
				{
					seriesData_ptr->addWFData(wfData_ptr);
				}

				if(rawData_ptr != NULL)
				{
					seriesData_ptr->addRawData(rawData_ptr);
				}

				if(ssData_ptr != NULL)
				{
					seriesData_ptr->addSpectData(ssData_ptr);
				}

				if(srDocData_ptr != NULL)
				{
					seriesData_ptr->addSRDocData(srDocData_ptr);
				}

				if(rtDoseData_ptr != NULL)
				{
					seriesData_ptr->addRTDoseData(rtDoseData_ptr);
				}

				if(rtPlanData_ptr != NULL)
				{
					seriesData_ptr->addRTPlanData(rtPlanData_ptr);
				}

				if(rtStructSetData_ptr != NULL)
				{
					seriesData_ptr->addRTStructSetData(rtStructSetData_ptr);
				}

				if(rtTreatData_ptr != NULL)
				{
					seriesData_ptr->addRTTreatData(rtTreatData_ptr);
				}

				if(encapData_ptr != NULL)
				{
					seriesData_ptr->addEncapDocData(encapData_ptr);
				}

				if(fiducialData_ptr != NULL)
				{
					seriesData_ptr->addFiducialData(fiducialData_ptr);
				}

				if(KeyobjData_ptr != NULL)
				{
					seriesData_ptr->addKeyobjectData(KeyobjData_ptr);
				}

				if(regData_ptr != NULL)
				{
					seriesData_ptr->addRegistrationData(regData_ptr);
				}

				if(valueMapData_ptr != NULL)
				{
					seriesData_ptr->addValueMapData(valueMapData_ptr);
				}

				// add Series Data
				studyData_ptr->addSeriesData(seriesData_ptr);
			}
			else 
			{
				if(isRTDosePresent)
				{
					if ((rtDoseData_ptr = seriesData_ptr->searchRTDose(doseSumType)) == NULL)
					{
						rtDoseData_ptr = new RT_DOSE_INFO_CLASS(instanceNr, spCharSetsValues, doseSumType,"");
						
						if(rtDoseData_ptr != NULL)
						{
							seriesData_ptr->addRTDoseData(rtDoseData_ptr);
						}
					}
					else 
					{
						rtDoseData_ptr->incrementCount();
					}
				}

				if(isRTStructSetPresent)
				{
					if ((rtStructSetData_ptr = seriesData_ptr->searchRTStructSet(rtStrucDate,rtStrucTime)) == NULL)
					{
						rtStructSetData_ptr = new RT_STRUC_SET_INFO_CLASS(instanceNr, spCharSetsValues, rtStrucLable,rtStrucDate,rtStrucTime,"");
						
						if(rtStructSetData_ptr != NULL)
						{
							seriesData_ptr->addRTStructSetData(rtStructSetData_ptr);
						}
					}
					else 
					{
						rtStructSetData_ptr->incrementCount();
					}
				}

				if(isRTPlanPresent)
				{
					if ((rtPlanData_ptr = seriesData_ptr->searchRTPlan(rtPlanDate,rtPlanTime)) == NULL)
					{
						rtPlanData_ptr = new RT_PLAN_INFO_CLASS(instanceNr, spCharSetsValues, rtPlanLable,rtPlanDate,rtPlanTime,"");
						
						if(rtPlanData_ptr != NULL)
						{
							seriesData_ptr->addRTPlanData(rtPlanData_ptr);
						}
					}
					else 
					{
						rtPlanData_ptr->incrementCount();
					}
				}

				if(isRTTreatPresent)
				{
					if ((rtTreatData_ptr = seriesData_ptr->searchRTTreat(rtTreatDate,rtTreatTime)) == NULL)
					{
						rtTreatData_ptr = new RT_TREATMENT_INFO_CLASS(instanceNr, spCharSetsValues, rtTreatDate,rtTreatTime,"");
						
						if(rtTreatData_ptr != NULL)
						{
							seriesData_ptr->addRTTreatData(rtTreatData_ptr);
						}
					}
					else 
					{
						rtTreatData_ptr->incrementCount();
					}
				}

				if(isKeyObjDocPresent)
				{
					if ((KeyobjData_ptr = seriesData_ptr->searchKeyobject(dataDate,dataTime)) == NULL)
					{
						KeyobjData_ptr = new KEY_OBJECT_DOC_INFO_CLASS(instanceNr,spCharSetsValues, dataDate,dataTime,conNameSeqValuePtr,conSeqValuePtr,"");
						
						if(KeyobjData_ptr != NULL)
						{
							seriesData_ptr->addKeyobjectData(KeyobjData_ptr);
						}
					}
					else 
					{
						KeyobjData_ptr->incrementCount();
					}
				}

				if(isEncapDocPresent)
				{
					if ((encapData_ptr = seriesData_ptr->searchEncapDocData(dataDate,dataTime)) == NULL)
					{
						encapData_ptr = new ENCAP_DOC_INFO_CLASS(instanceNr, spCharSetsValues, dataDate, dataTime, hl7InstId,docTitle,mimeType,conNameSeqValuePtr,"");
						
						if(encapData_ptr != NULL)
						{
							seriesData_ptr->addEncapDocData(encapData_ptr);
						}
					}
					else 
					{
						encapData_ptr->incrementCount();
					}
				}

				if(isFiducialPresent)
				{
					if ((fiducialData_ptr = seriesData_ptr->searchFiducial(dataDate,dataTime)) == NULL)
					{
						fiducialData_ptr = new FIDUCIAL_INFO_CLASS(spCharSetsValues, dataDate,dataTime,"");
						
						if(fiducialData_ptr != NULL)
						{
							seriesData_ptr->addFiducialData(fiducialData_ptr);
						}
					}
					else 
					{
						fiducialData_ptr->incrementCount();
					}
				}

				if(isRegistrationPresent)
				{
					if ((regData_ptr = seriesData_ptr->searchRegistration(dataDate,dataTime)) == NULL)
					{
						regData_ptr = new REGISTRATION_INFO_CLASS(spCharSetsValues, dataDate,dataTime,"");
						
						if(regData_ptr != NULL)
						{
							seriesData_ptr->addRegistrationData(regData_ptr);
						}
					}
					else 
					{
						regData_ptr->incrementCount();
					}
				}

				if(isValueMapPresent)
				{
					if ((valueMapData_ptr = seriesData_ptr->searchValueMap(dataDate,dataTime)) == NULL)
					{
						valueMapData_ptr = new VALUE_MAP_INFO_CLASS(spCharSetsValues, dataDate,dataTime,"");
						
						if(valueMapData_ptr != NULL)
						{
							seriesData_ptr->addValueMapData(valueMapData_ptr);
						}
					}
					else 
					{
						valueMapData_ptr->incrementCount();
					}
				}

				if(isSrDocPresent)
				{
					if ((srDocData_ptr = seriesData_ptr->searchSRDoc(dataDate,dataTime)) == NULL)
					{
						srDocData_ptr = new SRDOC_INFO_CLASS(instanceNr, spCharSetsValues, compFlag, verFlag, dataDate, dataTime, verDT,conNameSeqValuePtr, conSeqValuePtr,"");
						
						if(srDocData_ptr != NULL)
						{
							seriesData_ptr->addSRDocData(srDocData_ptr);
						}
					}
					else 
					{
						srDocData_ptr->incrementCount();
					}
				}

				if(isSpectroscopyPresent)
				{
					if ((ssData_ptr = seriesData_ptr->searchSpectData(dataDate,dataTime)) == NULL)
					{
						ssData_ptr = new SPECTROSCOPY_INFO_CLASS(instanceNr, spCharSetsValues, imageType, dataDate, dataTime, refImageEvidenceSeqValuePtr, "");
						
						if(ssData_ptr != NULL)
						{
							seriesData_ptr->addSpectData(ssData_ptr);
						}
					}
					else 
					{
						ssData_ptr->incrementCount();
					}
				}

				if(isRawDataPresent)
				{
					if ((rawData_ptr = seriesData_ptr->searchRawData(dataDate,dataTime)) == NULL)
					{
						rawData_ptr = new RAWDATA_INFO_CLASS(instanceNr, spCharSetsValues, dataDate, dataTime, "");
						
						if(rawData_ptr != NULL)
						{
							seriesData_ptr->addRawData(rawData_ptr);
						}
					}
					else 
					{
						rawData_ptr->incrementCount();
					}
				}

				if(isWaveFormPresent)
				{
					if ((wfData_ptr = seriesData_ptr->searchWF(dataDate,dataTime)) == NULL)
					{
						wfData_ptr = new WAVEFORM_INFO_CLASS(instanceNr, spCharSetsValues, dataDate, dataTime, "");
						
						if(wfData_ptr != NULL)
						{
							seriesData_ptr->addWFData(wfData_ptr);
						}
					}
					else 
					{
						wfData_ptr->incrementCount();
					}
				}

				// series already exists - check for sop instance
				if(isPSPresent)
				{
					if ((psData_ptr = seriesData_ptr->searchPS(sopInstanceUid)) == NULL)
					{
						psData_ptr = new PRESENTATION_STATE_INFO_CLASS(instanceNr, refFileId, sopClassUid, sopInstanceUid, refTSUid, refSeriesSeqValuePtr,psDate, psTime, contLable,contDesc,contCreator, "");
						
						if(psData_ptr != NULL)
						{
							seriesData_ptr->addPSData(psData_ptr);
						}
					}
					else 
					{
						psData_ptr->incrementCount();
					}
				}

				// create SOP Instance Data if it present
				if(isImagePresent)
				{
					if ((sopInstanceData_ptr = seriesData_ptr->searchImage(sopInstanceUid)) == NULL)
					{
						// create SOP Instance Data
						sopInstanceData_ptr = new IMAGE_INFO_CLASS(sopInstanceUid, refFileId, sopClassUid, sopInstanceUid, refTSUid, instanceNr,spCharSetsValues,"");

						// add SOP Instance Data
						if(sopInstanceData_ptr != NULL)
						{
							seriesData_ptr->addSopInstanceData(sopInstanceData_ptr);
						}						
					}
					else 
					{
						// sop instance exists - we've got the same one again!!
						sopInstanceData_ptr->incrementCount();
					}
				}
			}
		}
	}
	spCharSetsValues.clear();
}

//>>===========================================================================

bool GENERATE_DICOMDIR_CLASS::CreateDICOMObjects()

//  DESCRIPTION     : Create DICOMDIR Head, File Meta & Tail DICOM and store them
//					: into warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	//Create DICOMDIR Header & store into Data Warehouse.
	if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the DICOMDIR Header in Warehouse.");
    }

	FILEHEAD_CLASS* fileHead_ptr = new FILEHEAD_CLASS();

	// cascade the logger
	if (fileHead_ptr)
	{
		fileHead_ptr->setLogger(loggerM_ptr);
		fileHead_ptr->setPreambleValue("");
		fileHead_ptr->setPrefix("DICM");
		UID_CLASS uid(EXPLICIT_VR_LITTLE_ENDIAN);
		fileHead_ptr->setTransferSyntaxUid(uid);
	}

	// try to store the DICOMDIR Header in the warehouse
	bool result = WAREHOUSE->store("", fileHead_ptr);
	if (result)
	{
		// log the action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 2, "CREATE %s (in Data Warehouse)", WIDName(fileHead_ptr->getWidType()));
		}
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s in Data Warehouse", WIDName(fileHead_ptr->getWidType()));
		}
	}

	//Create File Meta Info object & store into Data Warehouse.
	if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the DICOMDIR directory meta information in Warehouse.");
    }

	DCM_DATASET_CLASS* fileMetaInfo_ptr = new DCM_DATASET_CLASS();

	// cascade the logger
	if (fileMetaInfo_ptr)
	{
		fileMetaInfo_ptr->setLogger(loggerM_ptr);
		fileMetaInfo_ptr->setDefineGroupLengths(true);
		fileMetaInfo_ptr->setOBValue(TAG_FILE_META_INFORMATION_VERSION, 0x0001, 0x0002);
		fileMetaInfo_ptr->setUIValue(TAG_MEDIA_STORAGE_SOP_CLASS_UID, MEDIA_STORAGE_DIRECTORY_SOP_CLASS_UID);

		char generated_uid[UI_LENGTH+1];
		createUID(generated_uid, IMPLEMENTATION_CLASS_UID);
		string instance_uid = generated_uid;
		fileMetaInfo_ptr->setUIValue(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID, instance_uid);

		fileMetaInfo_ptr->setUIValue(TAG_TRANSFER_SYNTAX_UID, EXPLICIT_VR_LITTLE_ENDIAN);
		fileMetaInfo_ptr->setUIValue(TAG_IMPLEMENTATION_CLASS_UID, IMPLEMENTATION_CLASS_UID);
		fileMetaInfo_ptr->setSHValue(TAG_IMPLEMENTATION_VERSION_NAME, IMPLEMENTATION_VERSION_NAME);
		fileMetaInfo_ptr->setAEValue(TAG_SOURCE_APPLICATION_ENTITY_TITLE,"DVT");
		fileMetaInfo_ptr->setWidType(WID_META_INFO);
	}

	// try to store the File Meta Info object in the warehouse
	result = WAREHOUSE->store(fileMetaIdentifier, fileMetaInfo_ptr);
	if (result)
	{
		// log the action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 2, "CREATE %s %s (in Data Warehouse)", WIDName(fileMetaInfo_ptr->getWidType()), fileMetaIdentifier);
		}
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(fileMetaInfo_ptr->getWidType()), fileMetaIdentifier);
		}
	}

	//Create DICOMDIR tail & store into Data Warehouse.
	if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the DICOMDIR Tail in Warehouse.");
    }
	FILETAIL_CLASS* fileTail_ptr = new FILETAIL_CLASS();

	// cascade the logger
	if (fileTail_ptr)
	{
		fileTail_ptr->setLogger(loggerM_ptr);
		fileTail_ptr->setTrailingPadding(true);
		fileTail_ptr->setSectorSize(2048);
		fileTail_ptr->setPaddingValue((BYTE)0);
	}

	// try to store the DICOMDIR Tail in the warehouse
	result = WAREHOUSE->store("", fileTail_ptr);
	if (result)
	{
		// log the action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 2, "CREATE %s (in Data Warehouse)", WIDName(fileTail_ptr->getWidType()));
		}
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s in Data Warehouse", WIDName(fileTail_ptr->getWidType()));
		}
	}

	return result;
}

//>>===========================================================================

bool GENERATE_DICOMDIR_CLASS::CreateAndStoreRecords()

//  DESCRIPTION     : Create DICOMDIR records and store them into warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	//Create File Meta Info object & store into Data Warehouse.	
	bool result = false;

	if (patientData.getSize() != 0)
	{
		for (UINT i = 0; i < patientData.getSize(); i++)
		{
			if (loggerM_ptr)
			{
    			loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the patient records with unique identifier in Warehouse.");
			}
			DCM_ITEM_CLASS* patientRecord_ptr = new DCM_ITEM_CLASS();

			// cascade the logger
			patientRecord_ptr->setLogger(loggerM_ptr);
			patientRecord_ptr->setDefineGroupLengths(true);
			patientRecord_ptr->setIdentifier(patientData[i]->getIdentifier());
			//patientRecord_ptr->setPopulateWithAttributes(false);

			if((1 == patientData.getSize()) || ((i+1) == patientData.getSize()))
			{
				patientRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
			}
			else
			{
				DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(patientData[i+1]->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
				patientRecord_ptr->addAttribute(nextRecordAttribute_ptr);
			}			

			patientRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
			patientRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "PATIENT");

			// Set Sp character set values
			vector<string>::iterator it;
			vector<string> values = patientData[i]->getSpCharSetValues();
			if(values.size() != 0)
			{
				for (it = values.begin(); it < values.end(); ++it)
				{
					patientRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it);		
				}
			}

			patientRecord_ptr->setPNValue(TAG_PATIENTS_NAME, patientData[i]->getPatientName());
			patientRecord_ptr->setLOValue(TAG_PATIENT_ID, patientData[i]->getPatientId());

			for (UINT j = 0; j < patientData[i]->noStudies(); j++)
			{
				if (loggerM_ptr)
				{
    				loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the study records with unique identifier in Warehouse.");
				}
				DCM_ITEM_CLASS* studyRecord_ptr = new DCM_ITEM_CLASS();
				STUDY_INFO_CLASS *studyData_ptr = patientData[i]->getStudyData(j);

				if((j == 0) && (studyData_ptr != NULL))
				{
					DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(studyData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
					patientRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
				}

				// cascade the logger
				studyRecord_ptr->setLogger(loggerM_ptr);
				studyRecord_ptr->setDefineGroupLengths(true);
				studyRecord_ptr->setIdentifier(studyData_ptr->getIdentifier());
				//studyRecord_ptr->setPopulateWithAttributes(false);

				if((1 == patientData[i]->noStudies()) || ((j+1) == patientData[i]->noStudies()))
				{
					studyRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
				}
				else
				{
					DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(patientData[i]->getStudyData(j+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
					studyRecord_ptr->addAttribute(nextRecordAttribute_ptr);
				}

				studyRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
				studyRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "STUDY");
				studyRecord_ptr->setUIValue(TAG_STUDY_INSTANCE_UID, studyData_ptr->getInstanceUid());
				studyRecord_ptr->setSHValue(TAG_STUDY_ID, studyData_ptr->getStudyId());
				studyRecord_ptr->setLOValue(TAG_STUDY_DESCRIPTION, studyData_ptr->getStudyDescr());
				studyRecord_ptr->setSHValue(TAG_ACCESSION_NUMBER, studyData_ptr->getAccessionNr());
				studyRecord_ptr->setDAValue(TAG_STUDY_DATE, studyData_ptr->getStudyDate());
				studyRecord_ptr->setTMValue(TAG_STUDY_TIME, studyData_ptr->getStudyTime());

				for (UINT k = 0; k < studyData_ptr->noSeries(); k++)
				{
					if (loggerM_ptr)
					{
    					loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the series records with unique identifier in Warehouse.");
					}
					DCM_ITEM_CLASS* seriesRecord_ptr = new DCM_ITEM_CLASS();
					SERIES_INFO_CLASS *seriesData_ptr = studyData_ptr->getSeriesData(k);

					if((k == 0) && (seriesData_ptr != NULL))
					{
						DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(seriesData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
						studyRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
					}

					// cascade the logger
					seriesRecord_ptr->setLogger(loggerM_ptr);
					seriesRecord_ptr->setDefineGroupLengths(true);
					seriesRecord_ptr->setIdentifier(seriesData_ptr->getIdentifier());
					//seriesRecord_ptr->setPopulateWithAttributes(false);

					if((1 == studyData_ptr->noSeries()) || ((k+1) == studyData_ptr->noSeries()))
					{
						seriesRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
					}
					else
					{
						DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(studyData_ptr->getSeriesData(k+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
						seriesRecord_ptr->addAttribute(nextRecordAttribute_ptr);
					}

					seriesRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG,0xFFFF);
					seriesRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE,"SERIES");
					seriesRecord_ptr->setUIValue(TAG_SERIES_INSTANCE_UID, seriesData_ptr->getInstanceUid());
					seriesRecord_ptr->setCSValue(TAG_MODALITY, seriesData_ptr->getModality());
					seriesRecord_ptr->setISValue(TAG_SERIES_NUMBER, seriesData_ptr->getSeriesNr());

					for (UINT l = 0; l < seriesData_ptr->noSopInstances(); l++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the image records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* imageRecord_ptr = new DCM_ITEM_CLASS();
						IMAGE_INFO_CLASS *imageData_ptr = seriesData_ptr->getSopInstanceData(l);

						if((l == 0) && (imageData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(imageData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							seriesRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						imageRecord_ptr->setLogger(loggerM_ptr);
						imageRecord_ptr->setDefineGroupLengths(true);
						imageRecord_ptr->setIdentifier(imageData_ptr->getIdentifier());
						//imageRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noSopInstances()) || ((l+1) == seriesData_ptr->noSopInstances()))
						{
							imageRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getSopInstanceData(l+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							imageRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						imageRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						imageRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						imageRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "IMAGE");
						imageRecord_ptr->setCSValue(TAG_REFERENCED_FILE_ID, imageData_ptr->getRefFileId());
						imageRecord_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID_IN_FILE, imageData_ptr->getRefSOPClassUid());
						imageRecord_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE, imageData_ptr->getRefSOPClassInstanceUid());
						imageRecord_ptr->setUIValue(TAG_REFERENCED_TRANSFER_SYNTAX_UID_IN_FILE, imageData_ptr->getRefTSUid());

						// Set Sp character set values
						vector<string>::iterator it_img;
						vector<string> values_img = imageData_ptr->getSpCharSetValues();
						if(values_img.size() != 0)
						{
							for (it_img = values_img.begin(); it_img < values_img.end(); ++it_img)
							{
								imageRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_img);		
							}
						}

						//imageRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET,imageData_ptr->getSpCharSetValues());
						imageRecord_ptr->setUIValue(TAG_SOP_INSTANCE_UID, imageData_ptr->getInstanceUid());
						imageRecord_ptr->setISValue(TAG_IMAGE_NUMBER, imageData_ptr->getInstanceNr());

						// try to store the Image record in the warehouse
						string imageIdentifier = imageData_ptr->getIdentifier();
						imageRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(imageIdentifier, imageRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(imageRecord_ptr->getWidType()), imageIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(imageRecord_ptr->getWidType()), imageIdentifier.c_str());
							}
						}
					}

					for (UINT m = 0; (m < seriesData_ptr->noPresentationStates()) /*&& isPSPresent*/; m++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the presentation records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* psRecord_ptr = new DCM_ITEM_CLASS();
						PRESENTATION_STATE_INFO_CLASS *psData_ptr = seriesData_ptr->getPSData(m);

						if((m == 0) && (psData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(psData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							seriesRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						psRecord_ptr->setLogger(loggerM_ptr);
						psRecord_ptr->setDefineGroupLengths(true);
						psRecord_ptr->setIdentifier(psData_ptr->getIdentifier());
						//psRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noPresentationStates()) || ((m+1) == seriesData_ptr->noPresentationStates()))
						{
							psRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getPSData(m+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							psRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						psRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						psRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						psRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "PRESENTATION");
						psRecord_ptr->setCSValue(TAG_REFERENCED_FILE_ID, psData_ptr->getRefFileId());
						psRecord_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID_IN_FILE, psData_ptr->getRefSOPClassUid());
						psRecord_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE, psData_ptr->getRefSOPClassInstanceUid());
						psRecord_ptr->setUIValue(TAG_REFERENCED_TRANSFER_SYNTAX_UID_IN_FILE, psData_ptr->getRefTSUid());
						psRecord_ptr->setUIValue(TAG_SOP_INSTANCE_UID, psData_ptr->getRefSOPClassInstanceUid());
						psRecord_ptr->setCSValue(TAG_CONTENT_LABLE, psData_ptr->getContentLable());
						psRecord_ptr->setSQValue(TAG_REFERENCED_SERIES_SEQUENCE, psData_ptr->getRefSeriesSeqValue());
						psRecord_ptr->setDAValue(TAG_PS_CREATION_DATE, psData_ptr->getPSCreationDate());
						psRecord_ptr->setTMValue(TAG_PS_CREATION_TIME,psData_ptr->getPSCreationTime());
						psRecord_ptr->setLOValue(TAG_CONTENT_DESCRIPTION, psData_ptr->getContentDesc());
						psRecord_ptr->setPNValue(TAG_CONTENT_CREATORNAME,psData_ptr->getContentCreator());
						psRecord_ptr->setISValue(TAG_IMAGE_NUMBER, psData_ptr->getInstanceNr());

						// try to store the Presentation record in the warehouse
						string psIdentifier = psData_ptr->getIdentifier();
						psRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(psIdentifier, psRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(psRecord_ptr->getWidType()), psIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(psRecord_ptr->getWidType()), psIdentifier.c_str());
							}
						}
					}

					for (UINT n = 0; (n < seriesData_ptr->noWaveforms()) ; n++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the waveform records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* wfRecord_ptr = new DCM_ITEM_CLASS();
						WAVEFORM_INFO_CLASS *wfData_ptr = seriesData_ptr->getWFData(n);

						if((n == 0) && (wfData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(wfData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							seriesRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						wfRecord_ptr->setLogger(loggerM_ptr);
						wfRecord_ptr->setDefineGroupLengths(true);
						wfRecord_ptr->setIdentifier(wfData_ptr->getIdentifier());
						//psRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noWaveforms()) || ((n+1) == seriesData_ptr->noWaveforms()))
						{
							wfRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getWFData(n+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							wfRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						wfRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						wfRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						wfRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "WAVEFORM");
						wfRecord_ptr->setDAValue(TAG_IMAGE_DATE, wfData_ptr->getWFCreationDate());
						wfRecord_ptr->setTMValue(TAG_IMAGE_TIME, wfData_ptr->getWFCreationTime());
						
						// Set Sp character set values
						vector<string>::iterator it_wf;
						vector<string> values_wf = wfData_ptr->getSpCharSetValues();
						if(values_wf.size() != 0)
						{
							for (it_wf = values_wf.begin(); it_wf < values_wf.end(); ++it_wf)
							{
								wfRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_wf);		
							}
						}

						// try to store the Waveform record in the warehouse
						string wfIdentifier = wfData_ptr->getIdentifier();
						wfRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(wfIdentifier, wfRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(wfRecord_ptr->getWidType()), wfIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(wfRecord_ptr->getWidType()), wfIdentifier.c_str());
							}
						}
					}

					for (UINT p = 0; (p < seriesData_ptr->noRawDatas()); p++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the raw data records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* rdRecord_ptr = new DCM_ITEM_CLASS();
						RAWDATA_INFO_CLASS *rdData_ptr = seriesData_ptr->getRawData(p);

						if((p == 0) && (rdData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(rdData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							seriesRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						rdRecord_ptr->setLogger(loggerM_ptr);
						rdRecord_ptr->setDefineGroupLengths(true);
						rdRecord_ptr->setIdentifier(rdData_ptr->getIdentifier());
						//rdRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noRawDatas()) || ((p+1) == seriesData_ptr->noRawDatas()))
						{
							rdRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getRawData(p+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							rdRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						rdRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						rdRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						rdRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "RAW DATA");
						rdRecord_ptr->setDAValue(TAG_IMAGE_DATE, rdData_ptr->getRDCreationDate());
						rdRecord_ptr->setTMValue(TAG_IMAGE_TIME, rdData_ptr->getRDCreationTime());

						// Set Sp character set values
						vector<string>::iterator it_rd;
						vector<string> values_rd = rdData_ptr->getSpCharSetValues();
						if(values_rd.size() != 0)
						{
							for (it_rd = values_rd.begin(); it_rd < values_rd.end(); ++it_rd)
							{
								rdRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_rd);		
							}
						}

						// try to store the Raw data record in the warehouse
						string rdIdentifier = rdData_ptr->getIdentifier();
						rdRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(rdIdentifier, rdRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(rdRecord_ptr->getWidType()), rdIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(rdRecord_ptr->getWidType()), rdIdentifier.c_str());
							}
						}
					}

					for (UINT q = 0; (q < seriesData_ptr->noSpectDatas()); q++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the spectroscopy records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* ssRecord_ptr = new DCM_ITEM_CLASS();
						SPECTROSCOPY_INFO_CLASS *ssData_ptr = seriesData_ptr->getSpectData(q);

						if((q == 0) && (ssData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(ssData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							seriesRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						ssRecord_ptr->setLogger(loggerM_ptr);
						ssRecord_ptr->setDefineGroupLengths(true);
						ssRecord_ptr->setIdentifier(ssData_ptr->getIdentifier());
						//ssRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noSpectDatas()) || ((q+1) == seriesData_ptr->noSpectDatas()))
						{
							ssRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getSpectData(q+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							ssRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						ssRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						ssRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						ssRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "SPECTROSCOPY");
						ssRecord_ptr->setCSValue(TAG_IMAGE_TYPE, ssData_ptr->getImageType());
						ssRecord_ptr->setDAValue(TAG_IMAGE_DATE, ssData_ptr->getSSCreationDate());
						ssRecord_ptr->setTMValue(TAG_IMAGE_TIME, ssData_ptr->getSSCreationTime());
						ssRecord_ptr->setSQValue(0x00089092, ssData_ptr->getRefImageEvidenceSeqValue());

						// Set Sp character set values
						vector<string>::iterator it_ss;
						vector<string> values_ss = ssData_ptr->getSpCharSetValues();
						if(values_ss.size() != 0)
						{
							for (it_ss = values_ss.begin(); it_ss < values_ss.end(); ++it_ss)
							{
								ssRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_ss);		
							}
						}

						// try to store the Presentation record in the warehouse
						string ssIdentifier = ssData_ptr->getIdentifier();
						ssRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(ssIdentifier, ssRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(ssRecord_ptr->getWidType()), ssIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(ssRecord_ptr->getWidType()), ssIdentifier.c_str());
							}
						}
					}

					for (UINT s = 0; (s < seriesData_ptr->noSRDocDatas()); s++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the SR doc records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* srRecord_ptr = new DCM_ITEM_CLASS();
						SRDOC_INFO_CLASS *srData_ptr = seriesData_ptr->getSRDocData(s);

						if((s == 0) && (srData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(srData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							seriesRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						srRecord_ptr->setLogger(loggerM_ptr);
						srRecord_ptr->setDefineGroupLengths(true);
						srRecord_ptr->setIdentifier(srData_ptr->getIdentifier());
						//srRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noSRDocDatas()) || ((s+1) == seriesData_ptr->noSRDocDatas()))
						{
							srRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getSRDocData(s+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							srRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						srRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						srRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						srRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "SR DOCUMENT");
						srRecord_ptr->setCSValue(0x0040A491, srData_ptr->getCompFlag());
						srRecord_ptr->setCSValue(0x0040A493, srData_ptr->getVerFlag());
						srRecord_ptr->setCSValue(0x0040A030, srData_ptr->getVerDateTime());
						srRecord_ptr->setDAValue(TAG_IMAGE_DATE, srData_ptr->getContentDate());
						srRecord_ptr->setTMValue(TAG_IMAGE_TIME, srData_ptr->getContentTime());
						srRecord_ptr->setSQValue(0x0040A043, srData_ptr->getConCodeSeqValue());
						srRecord_ptr->setSQValue(0x0040A730, srData_ptr->getConSeqValue());
						
						// Set Sp character set values
						vector<string>::iterator it_sr;
						vector<string> values_sr = srData_ptr->getSpCharSetValues();
						if(values_sr.size() != 0)
						{
							for (it_sr = values_sr.begin(); it_sr < values_sr.end(); ++it_sr)
							{
								srRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_sr);		
							}
						}

						// try to store the SR Doc record in the warehouse
						string srIdentifier = srData_ptr->getIdentifier();
						srRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(srIdentifier, srRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(srRecord_ptr->getWidType()), srIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(srRecord_ptr->getWidType()), srIdentifier.c_str());
							}
						}
					}

					for (UINT t = 0; (t < seriesData_ptr->noRTDoseDatas()); t++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the RT Dose records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* rtDoseRecord_ptr = new DCM_ITEM_CLASS();
						RT_DOSE_INFO_CLASS *rtDoseData_ptr = seriesData_ptr->getRTDoseData(t);

						if((t == 0) && (rtDoseData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(rtDoseData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							seriesRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						rtDoseRecord_ptr->setLogger(loggerM_ptr);
						rtDoseRecord_ptr->setDefineGroupLengths(true);
						rtDoseRecord_ptr->setIdentifier(rtDoseData_ptr->getIdentifier());
						//srRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noRTDoseDatas()) || ((t+1) == seriesData_ptr->noRTDoseDatas()))
						{
							rtDoseRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getRTDoseData(t+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							rtDoseRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						rtDoseRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						rtDoseRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						rtDoseRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "RT DOSE");
						rtDoseRecord_ptr->setISValue(TAG_IMAGE_NUMBER, rtDoseData_ptr->getInstanceNr());
						rtDoseRecord_ptr->setCSValue(TAG_DOSE_SUMMATION_TYPE, rtDoseData_ptr->getDoseSummationType());
						
						// Set Sp character set values
						vector<string>::iterator it_sr;
						vector<string> values_sr = rtDoseData_ptr->getSpCharSetValues();
						if(values_sr.size() != 0)
						{
							for (it_sr = values_sr.begin(); it_sr < values_sr.end(); ++it_sr)
							{
								rtDoseRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_sr);		
							}
						}

						// try to store the RT Dose record in the warehouse
						string rtDoseIdentifier = rtDoseData_ptr->getIdentifier();
						rtDoseRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(rtDoseIdentifier, rtDoseRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(rtDoseRecord_ptr->getWidType()), rtDoseIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(rtDoseRecord_ptr->getWidType()), rtDoseIdentifier.c_str());
							}
						}
					}

					for (UINT w = 0; (w < seriesData_ptr->noRTPlanDatas()); w++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the RT Plan data records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* rtPlanRecord_ptr = new DCM_ITEM_CLASS();
						RT_PLAN_INFO_CLASS *rtPlanData_ptr = seriesData_ptr->getRTPlanData(w);

						if((w == 0) && (rtPlanData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(rtPlanData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							rtPlanRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						rtPlanRecord_ptr->setLogger(loggerM_ptr);
						rtPlanRecord_ptr->setDefineGroupLengths(true);
						rtPlanRecord_ptr->setIdentifier(rtPlanData_ptr->getIdentifier());
						//rdRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noRTPlanDatas()) || ((w+1) == seriesData_ptr->noRTPlanDatas()))
						{
							rtPlanRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getRTPlanData(w+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							rtPlanRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						rtPlanRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						rtPlanRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						rtPlanRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "RT PLAN");
						rtPlanRecord_ptr->setISValue(TAG_IMAGE_NUMBER, rtPlanData_ptr->getInstanceNr());
						rtPlanRecord_ptr->setDAValue(TAG_RT_PLAN_DATE, rtPlanData_ptr->getRTPlanDate());
						rtPlanRecord_ptr->setTMValue(TAG_RT_PLAN_TIME, rtPlanData_ptr->getRTPlanTime());
						rtPlanRecord_ptr->setSHValue(TAG_RT_PLAN_LABEL, rtPlanData_ptr->getRTPlanLable());

						// Set Sp character set values
						vector<string>::iterator it_rd;
						vector<string> values_rd = rtPlanData_ptr->getSpCharSetValues();
						if(values_rd.size() != 0)
						{
							for (it_rd = values_rd.begin(); it_rd < values_rd.end(); ++it_rd)
							{
								rtPlanRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_rd);		
							}
						}

						// try to store the RT Plan record in the warehouse
						string rtPlanIdentifier = rtPlanData_ptr->getIdentifier();
						rtPlanRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(rtPlanIdentifier, rtPlanRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(rtPlanRecord_ptr->getWidType()), rtPlanIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(rtPlanRecord_ptr->getWidType()), rtPlanIdentifier.c_str());
							}
						}
					}

					for (UINT x = 0; (x < seriesData_ptr->noRTTreatDatas()); x++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the RT Treatment data records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* rtTreatRecord_ptr = new DCM_ITEM_CLASS();
						RT_TREATMENT_INFO_CLASS *rtTreatData_ptr = seriesData_ptr->getRTTreatData(x);

						if((x == 0) && (rtTreatData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(rtTreatData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							rtTreatRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						rtTreatRecord_ptr->setLogger(loggerM_ptr);
						rtTreatRecord_ptr->setDefineGroupLengths(true);
						rtTreatRecord_ptr->setIdentifier(rtTreatData_ptr->getIdentifier());
						//rdRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noRTTreatDatas()) || ((x+1) == seriesData_ptr->noRTTreatDatas()))
						{
							rtTreatRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getRTTreatData(x+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							rtTreatRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						rtTreatRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						rtTreatRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						rtTreatRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "RT TREAT RECORD");
						rtTreatRecord_ptr->setISValue(TAG_IMAGE_NUMBER, rtTreatData_ptr->getInstanceNr());
						rtTreatRecord_ptr->setDAValue(TAG_RT_TREATMENT_DATE, rtTreatData_ptr->getRTTreatDate());
						rtTreatRecord_ptr->setTMValue(TAG_RT_TREATMENT_TIME, rtTreatData_ptr->getRTTreatTime());

						// Set Sp character set values
						vector<string>::iterator it_rd;
						vector<string> values_rd = rtTreatData_ptr->getSpCharSetValues();
						if(values_rd.size() != 0)
						{
							for (it_rd = values_rd.begin(); it_rd < values_rd.end(); ++it_rd)
							{
								rtTreatRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_rd);		
							}
						}

						// try to store the RT Treatment record in the warehouse
						string rtTreatIdentifier = rtTreatData_ptr->getIdentifier();
						rtTreatRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(rtTreatIdentifier, rtTreatRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(rtTreatRecord_ptr->getWidType()), rtTreatIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(rtTreatRecord_ptr->getWidType()), rtTreatIdentifier.c_str());
							}
						}
					}

					for (UINT y = 0; (y < seriesData_ptr->noRTStructSetDatas()); y++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the RT Structure set data records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* rtStrucRecord_ptr = new DCM_ITEM_CLASS();
						RT_STRUC_SET_INFO_CLASS *rtStrucData_ptr = seriesData_ptr->getRTStructSetData(y);

						if((y == 0) && (rtStrucData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(rtStrucData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							rtStrucRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						rtStrucRecord_ptr->setLogger(loggerM_ptr);
						rtStrucRecord_ptr->setDefineGroupLengths(true);
						rtStrucRecord_ptr->setIdentifier(rtStrucData_ptr->getIdentifier());
						//rdRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noRTStructSetDatas()) || ((y+1) == seriesData_ptr->noRTStructSetDatas()))
						{
							rtStrucRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getRTStructSetData(y+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							rtStrucRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						rtStrucRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						rtStrucRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						rtStrucRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "RT STRUCTURE SET");
						rtStrucRecord_ptr->setISValue(TAG_IMAGE_NUMBER, rtStrucData_ptr->getInstanceNr());
						rtStrucRecord_ptr->setDAValue(TAG_STRUCTURE_SET_DATE, rtStrucData_ptr->getRTStrucSetDate());
						rtStrucRecord_ptr->setTMValue(TAG_STRUCTURE_SET_TIME, rtStrucData_ptr->getRTStrucSetTime());
						rtStrucRecord_ptr->setSHValue(TAG_STRUCTURE_SET_LABEL, rtStrucData_ptr->getRTStrucSetLable());

						// Set Sp character set values
						vector<string>::iterator it_rd;
						vector<string> values_rd = rtStrucData_ptr->getSpCharSetValues();
						if(values_rd.size() != 0)
						{
							for (it_rd = values_rd.begin(); it_rd < values_rd.end(); ++it_rd)
							{
								rtStrucRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_rd);		
							}
						}

						// try to store the RT Struc set record in the warehouse
						string rtStrucSetIdentifier = rtStrucData_ptr->getIdentifier();
						rtStrucRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(rtStrucSetIdentifier, rtStrucRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(rtStrucRecord_ptr->getWidType()), rtStrucSetIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(rtStrucRecord_ptr->getWidType()), rtStrucSetIdentifier.c_str());
							}
						}
					}

					for (UINT u = 0; (u < seriesData_ptr->noRegistrationDatas()); u++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the registration data records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* regRecord_ptr = new DCM_ITEM_CLASS();
						REGISTRATION_INFO_CLASS *regData_ptr = seriesData_ptr->getRegistrationData(u);

						if((u == 0) && (regData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(regData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							regRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						regRecord_ptr->setLogger(loggerM_ptr);
						regRecord_ptr->setDefineGroupLengths(true);
						regRecord_ptr->setIdentifier(regData_ptr->getIdentifier());
						//rdRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noRegistrationDatas()) || ((u+1) == seriesData_ptr->noRegistrationDatas()))
						{
							regRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getRegistrationData(u+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							regRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						regRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						regRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						regRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "REGISTRATION");
						regRecord_ptr->setDAValue(TAG_IMAGE_DATE, regData_ptr->getContentDate());
						regRecord_ptr->setTMValue(TAG_IMAGE_TIME, regData_ptr->getContentTime());

						// Set Sp character set values
						vector<string>::iterator it_rd;
						vector<string> values_rd = regData_ptr->getSpCharSetValues();
						if(values_rd.size() != 0)
						{
							for (it_rd = values_rd.begin(); it_rd < values_rd.end(); ++it_rd)
							{
								regRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_rd);		
							}
						}

						// try to store the Registration record in the warehouse
						string regIdentifier = regData_ptr->getIdentifier();
						regRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(regIdentifier, regRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(regRecord_ptr->getWidType()), regIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(regRecord_ptr->getWidType()), regIdentifier.c_str());
							}
						}
					}

					for (UINT v = 0; (v < seriesData_ptr->noFiducialDatas()); v++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the fiducial data records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* fedRecord_ptr = new DCM_ITEM_CLASS();
						FIDUCIAL_INFO_CLASS *fedData_ptr = seriesData_ptr->getFiducialData(v);

						if((v == 0) && (fedData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(fedData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							fedRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						fedRecord_ptr->setLogger(loggerM_ptr);
						fedRecord_ptr->setDefineGroupLengths(true);
						fedRecord_ptr->setIdentifier(fedData_ptr->getIdentifier());
						//rdRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noFiducialDatas()) || ((v+1) == seriesData_ptr->noFiducialDatas()))
						{
							fedRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getFiducialData(v+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							fedRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						fedRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						fedRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						fedRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "FIDUCIAL");
						fedRecord_ptr->setDAValue(TAG_IMAGE_DATE, fedData_ptr->getContentDate());
						fedRecord_ptr->setTMValue(TAG_IMAGE_TIME, fedData_ptr->getContentTime());

						// Set Sp character set values
						vector<string>::iterator it_rd;
						vector<string> values_rd = fedData_ptr->getSpCharSetValues();
						if(values_rd.size() != 0)
						{
							for (it_rd = values_rd.begin(); it_rd < values_rd.end(); ++it_rd)
							{
								fedRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_rd);		
							}
						}

						// try to store the Fiducial record in the warehouse
						string fedIdentifier = fedData_ptr->getIdentifier();
						fedRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(fedIdentifier, fedRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(fedRecord_ptr->getWidType()), fedIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(fedRecord_ptr->getWidType()), fedIdentifier.c_str());
							}
						}
					}

					for (UINT z = 0; (z < seriesData_ptr->noValueMapDatas()); z++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the Value map data records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* valueMapRecord_ptr = new DCM_ITEM_CLASS();
						VALUE_MAP_INFO_CLASS *valueMapData_ptr = seriesData_ptr->getValueMapData(z);

						if((z == 0) && (valueMapData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(valueMapData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							valueMapRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						valueMapRecord_ptr->setLogger(loggerM_ptr);
						valueMapRecord_ptr->setDefineGroupLengths(true);
						valueMapRecord_ptr->setIdentifier(valueMapData_ptr->getIdentifier());
						//rdRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noValueMapDatas()) || ((z+1) == seriesData_ptr->noValueMapDatas()))
						{
							valueMapRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getValueMapData(z+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							valueMapRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						valueMapRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						valueMapRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						valueMapRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "VALUE MAP");
						valueMapRecord_ptr->setDAValue(TAG_IMAGE_DATE, valueMapData_ptr->getContentDate());
						valueMapRecord_ptr->setTMValue(TAG_IMAGE_TIME, valueMapData_ptr->getContentTime());

						// Set Sp character set values
						vector<string>::iterator it_rd;
						vector<string> values_rd = valueMapData_ptr->getSpCharSetValues();
						if(values_rd.size() != 0)
						{
							for (it_rd = values_rd.begin(); it_rd < values_rd.end(); ++it_rd)
							{
								valueMapRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_rd);		
							}
						}

						// try to store the Value Map record in the warehouse
						string valueMapIdentifier = valueMapData_ptr->getIdentifier();
						valueMapRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(valueMapIdentifier, valueMapRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(valueMapRecord_ptr->getWidType()), valueMapIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(valueMapRecord_ptr->getWidType()), valueMapIdentifier.c_str());
							}
						}
					}

					for (UINT yy = 0; (yy < seriesData_ptr->noEncapDocDatas()); yy++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the Encap Doc records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* encapDocRecord_ptr = new DCM_ITEM_CLASS();
						ENCAP_DOC_INFO_CLASS *encapDocData_ptr = seriesData_ptr->getEncapDocData(yy);

						if((yy == 0) && (encapDocData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(encapDocData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							seriesRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						encapDocRecord_ptr->setLogger(loggerM_ptr);
						encapDocRecord_ptr->setDefineGroupLengths(true);
						encapDocRecord_ptr->setIdentifier(encapDocData_ptr->getIdentifier());
						//srRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noEncapDocDatas()) || ((yy+1) == seriesData_ptr->noEncapDocDatas()))
						{
							encapDocRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getEncapDocData(yy+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							encapDocRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						encapDocRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						encapDocRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						encapDocRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "ENCAP DOC");
						encapDocRecord_ptr->setISValue(TAG_IMAGE_NUMBER, encapDocData_ptr->getInstanceNr());
						encapDocRecord_ptr->setDAValue(TAG_IMAGE_DATE, encapDocData_ptr->getContentDate());
						encapDocRecord_ptr->setTMValue(TAG_IMAGE_TIME, encapDocData_ptr->getContentTime());
						encapDocRecord_ptr->setSTValue(TAG_DOC_TITLE, encapDocData_ptr->getDocTitle());
						encapDocRecord_ptr->setSTValue(TAG_HL7_INSTANCE_IDENTIFIER, encapDocData_ptr->getHL7InstanceIdentifier());
						encapDocRecord_ptr->setSQValue(0x0040A043, encapDocData_ptr->getConNameCodeSeqValue());
						encapDocRecord_ptr->setLOValue(TAG_MIME_TYPE_ENCAP_DOC, encapDocData_ptr->getMIMETypeEncapDoc());
						
						// Set Sp character set values
						vector<string>::iterator it_sr;
						vector<string> values_sr = encapDocData_ptr->getSpCharSetValues();
						if(values_sr.size() != 0)
						{
							for (it_sr = values_sr.begin(); it_sr < values_sr.end(); ++it_sr)
							{
								encapDocRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_sr);		
							}
						}

						// try to store the Encap Doc record in the warehouse
						string encapDocIdentifier = encapDocData_ptr->getIdentifier();
						encapDocRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(encapDocIdentifier, encapDocRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(encapDocRecord_ptr->getWidType()), encapDocIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(encapDocRecord_ptr->getWidType()), encapDocIdentifier.c_str());
							}
						}
					}

					for (UINT zz = 0; (zz < seriesData_ptr->noKeyobjectDatas()); zz++)
					{
						if (loggerM_ptr)
						{
    						loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the Key Object doc records with unique identifier in Warehouse.");
						}
						DCM_ITEM_CLASS* keyObjRecord_ptr = new DCM_ITEM_CLASS();
						KEY_OBJECT_DOC_INFO_CLASS *keyObjData_ptr = seriesData_ptr->getKeyobjectData(zz);

						if((zz == 0) && (keyObjData_ptr != NULL))
						{
							DCM_ATTRIBUTE_CLASS *lowerRecordAttribute_ptr = getULAttribute(keyObjData_ptr->getIdentifier(),
																TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY);
							seriesRecord_ptr->addAttribute(lowerRecordAttribute_ptr);
						}

						// cascade the logger
						keyObjRecord_ptr->setLogger(loggerM_ptr);
						keyObjRecord_ptr->setDefineGroupLengths(true);
						keyObjRecord_ptr->setIdentifier(keyObjData_ptr->getIdentifier());
						//srRecord_ptr->setPopulateWithAttributes(false);

						if((1 == seriesData_ptr->noKeyobjectDatas()) || ((zz+1) == seriesData_ptr->noKeyobjectDatas()))
						{
							keyObjRecord_ptr->setULValue(TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD, 0x00000000);
						}
						else
						{
							DCM_ATTRIBUTE_CLASS *nextRecordAttribute_ptr = getULAttribute(seriesData_ptr->getKeyobjectData(zz+1)->getIdentifier(),
																TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD);
							keyObjRecord_ptr->addAttribute(nextRecordAttribute_ptr);
						}

						keyObjRecord_ptr->setULValue(TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY, 0x00000000);
						keyObjRecord_ptr->setUSValue(TAG_RECORD_IN_USE_FLAG, 0xFFFF);
						keyObjRecord_ptr->setCSValue(TAG_DIRECTORY_RECORD_TYPE, "KEY OBJECT DOC");
						keyObjRecord_ptr->setISValue(TAG_IMAGE_NUMBER, keyObjData_ptr->getInstanceNr());
						keyObjRecord_ptr->setDAValue(TAG_IMAGE_DATE, keyObjData_ptr->getContentDate());
						keyObjRecord_ptr->setTMValue(TAG_IMAGE_TIME, keyObjData_ptr->getContentTime());
						keyObjRecord_ptr->setSQValue(0x0040A043, keyObjData_ptr->getConCodeSeqValue());
						keyObjRecord_ptr->setSQValue(0x0040A730, keyObjData_ptr->getConSeqValue());
						
						// Set Sp character set values
						vector<string>::iterator it_sr;
						vector<string> values_sr = keyObjData_ptr->getSpCharSetValues();
						if(values_sr.size() != 0)
						{
							for (it_sr = values_sr.begin(); it_sr < values_sr.end(); ++it_sr)
							{
								keyObjRecord_ptr->setCSValue(TAG_SPECIFIC_CHARACTER_SET, *it_sr);		
							}
						}

						// try to store the Key obj Doc record in the warehouse
						string keyObjIdentifier = keyObjData_ptr->getIdentifier();
						keyObjRecord_ptr->SortAttributes();
						result = WAREHOUSE->store(keyObjIdentifier, keyObjRecord_ptr);
						if (result)
						{
							// log the action
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(keyObjRecord_ptr->getWidType()), keyObjIdentifier.c_str());
							}
						}
						else
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(keyObjRecord_ptr->getWidType()), keyObjIdentifier.c_str());
							}
						}
					}

					// try to store the Series record in the warehouse
					string seriesIdentifier = seriesData_ptr->getIdentifier();
					seriesRecord_ptr->SortAttributes();
					result = WAREHOUSE->store(seriesIdentifier, seriesRecord_ptr);
					if (result)
					{
						// log the action
						if (loggerM_ptr)
						{
							loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(seriesRecord_ptr->getWidType()), seriesIdentifier.c_str());
						}
					}
					else
					{
						if (loggerM_ptr)
						{
							loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(seriesRecord_ptr->getWidType()), seriesIdentifier.c_str());
						}
					}
				}

				// try to store the Study record in the warehouse
				string studyIdentifier = studyData_ptr->getIdentifier();
				studyRecord_ptr->SortAttributes();
				result = WAREHOUSE->store(studyIdentifier, studyRecord_ptr);
				if (result)
				{
					// log the action
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(studyRecord_ptr->getWidType()), studyIdentifier.c_str());
					}
				}
				else
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(studyRecord_ptr->getWidType()), studyIdentifier.c_str());
					}
				}
			}

			// try to store the Patient record in the warehouse
			string patIdentifier = patientData[i]->getIdentifier();
			patientRecord_ptr->SortAttributes();
			result = WAREHOUSE->store(patIdentifier, patientRecord_ptr);
			if (result)
			{
				// log the action
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(patientRecord_ptr->getWidType()), patIdentifier.c_str());
				}
			}
			else
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(patientRecord_ptr->getWidType()), patIdentifier.c_str());
				}
			}
		}
	}

	return result;
}

//>>===========================================================================

bool GENERATE_DICOMDIR_CLASS::CreateAndStoreDirectorySequenceObject()

//  DESCRIPTION     : Create Directory Sequence Object and store them into warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	//Create Directory Sequence Object object & store into Data Warehouse.
	if (loggerM_ptr)
	{
    	loggerM_ptr->text(LOG_INFO, 2, "Creating and storing the Directory Sequence Object in Warehouse.");
	}
	DCM_DATASET_CLASS* directoryObject_ptr = new DCM_DATASET_CLASS();

	// cascade the logger
	directoryObject_ptr->setLogger(loggerM_ptr);
	if (directoryObject_ptr)
	{
		directoryObject_ptr->setDefineGroupLengths(true);
		directoryObject_ptr->setCSValue(TAG_FILE_SET_ID, "DVT");

		int nrOfPatients = patientData.getSize();
		string firstRecordIdentifier;
		string lastRecordIdentifier;

		if (nrOfPatients == 1)
		{
			firstRecordIdentifier = patientData[0]->getIdentifier();
			lastRecordIdentifier = patientData[0]->getIdentifier();
		}
		else if(nrOfPatients > 1)
		{
			firstRecordIdentifier = patientData[0]->getIdentifier();
			lastRecordIdentifier = patientData[nrOfPatients-1]->getIdentifier();
		}
		else
		{
			if (loggerM_ptr)
			{
    			loggerM_ptr->text(LOG_ERROR, 1, "There is no patient data in the DCM files.");
			}
			return false;
		}

		DCM_ATTRIBUTE_CLASS *firstRecordAttribute_ptr = getULAttribute(firstRecordIdentifier,
						TAG_OFFSET_OF_THE_FIRST_DIRECTORY_RECORD_OF_THE_ROOT_DIRECTORY_ENTITY);
		directoryObject_ptr->addAttribute(firstRecordAttribute_ptr);
		DCM_ATTRIBUTE_CLASS *lastRecordAttribute_ptr = getULAttribute(lastRecordIdentifier,
						TAG_OFFSET_OF_THE_LAST_DIRECTORY_RECORD_OF_THE_ROOT_DIRECTORY_ENTITY);
		directoryObject_ptr->addAttribute(lastRecordAttribute_ptr);

		directoryObject_ptr->setUSValue(TAG_FILE_SET_CONSISTENCY_FLAG, 0x0000);

		directoryObject_ptr->addAttribute(getSQAttribute());
		directoryObject_ptr->setWidType(WID_DATASET);
	}

	// try to store the File Meta Info object in the warehouse
	string directoryObjectIdentifier = "DICOMDIR";
	result = WAREHOUSE->store(directoryObjectIdentifier, directoryObject_ptr);
	if (result)
	{
		// log the action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 2, "CREATE %s %s (in Data Warehouse)", WIDName(directoryObject_ptr->getWidType()), directoryObjectIdentifier.c_str());
		}
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(directoryObject_ptr->getWidType()), directoryObjectIdentifier.c_str());
		}
	}

	return result;
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *GENERATE_DICOMDIR_CLASS::getULAttribute(string identifier, UINT32 tag)

//  DESCRIPTION     : Get the UL attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = new DCM_ATTRIBUTE_CLASS(tag, ATTR_VR_UL);
	attribute_ptr->SetType(ATTR_TYPE_1);

	DCM_VALUE_UL_CLASS *valueUL_ptr = new DCM_VALUE_UL_CLASS();
	valueUL_ptr->setIdentifier(identifier);
	attribute_ptr->AddValue(valueUL_ptr);
	return attribute_ptr;
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *GENERATE_DICOMDIR_CLASS::getSQAttribute()

//  DESCRIPTION     : Get the UL attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	DCM_ATTRIBUTE_CLASS *sqAttribute_ptr = new DCM_ATTRIBUTE_CLASS(TAG_DIRECTORY_RECORD_SEQUENCE, ATTR_VR_SQ);
	sqAttribute_ptr->SetType(ATTR_TYPE_1);
	
	// get SQ value
	DCM_VALUE_SQ_CLASS* sq_value_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);

	// cascade the logger
	sq_value_ptr->setLogger(loggerM_ptr);

	sq_value_ptr->setDefinedLength(true);
	
	for (UINT i = 0; i < patientData.getSize(); i++)
	{
		PATIENT_INFO_CLASS *patientData_ptr = patientData[i];

		if (patientData_ptr != NULL)
		{
			// instantiate the item
			DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

			// cascade the logger
			item_ptr->setLogger(loggerM_ptr);
			item_ptr->setDefinedLength(true);
			item_ptr->setIdentifier(patientData_ptr->getIdentifier());
			item_ptr->setValueByReference(true);

			sq_value_ptr->addItem(item_ptr);
		}

		for (UINT j = 0; j < patientData_ptr->noStudies(); j++)
		{
			STUDY_INFO_CLASS *studyData_ptr = patientData_ptr->getStudyData(j);

			if (studyData_ptr != NULL)
			{
				// instantiate the item
				DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

				// cascade the logger
				item_ptr->setLogger(loggerM_ptr);
				item_ptr->setDefinedLength(true);
				item_ptr->setIdentifier(studyData_ptr->getIdentifier());
				item_ptr->setValueByReference(true);

				sq_value_ptr->addItem(item_ptr);
			}

			for (UINT k = 0; k < studyData_ptr->noSeries(); k++)
			{
				SERIES_INFO_CLASS *seriesData_ptr = studyData_ptr->getSeriesData(k);

				if (seriesData_ptr != NULL)
				{
					// instantiate the item
					DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

					// cascade the logger
					item_ptr->setLogger(loggerM_ptr);
					item_ptr->setDefinedLength(true);
					item_ptr->setIdentifier(seriesData_ptr->getIdentifier());
					item_ptr->setValueByReference(true);

					sq_value_ptr->addItem(item_ptr);
				}

				for (UINT l = 0; l < seriesData_ptr->noSopInstances(); l++)
				{
					IMAGE_INFO_CLASS *imageData_ptr = seriesData_ptr->getSopInstanceData(l);

					if (imageData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(imageData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT m = 0; (m < seriesData_ptr->noPresentationStates()); m++)
				{
					PRESENTATION_STATE_INFO_CLASS *psData_ptr = seriesData_ptr->getPSData(m);

					if (psData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(psData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT n = 0; (n < seriesData_ptr->noWaveforms()); n++)
				{
					WAVEFORM_INFO_CLASS *wfData_ptr = seriesData_ptr->getWFData(n);

					if (wfData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(wfData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT o = 0; (o < seriesData_ptr->noRawDatas()); o++)
				{
					RAWDATA_INFO_CLASS *rawData_ptr = seriesData_ptr->getRawData(o);

					if (rawData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(rawData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT p = 0; (p < seriesData_ptr->noSpectDatas()); p++)
				{
					SPECTROSCOPY_INFO_CLASS *spectData_ptr = seriesData_ptr->getSpectData(p);

					if (spectData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(spectData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT q = 0; (q < seriesData_ptr->noSRDocDatas()); q++)
				{
					SRDOC_INFO_CLASS *srData_ptr = seriesData_ptr->getSRDocData(q);

					if (srData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(srData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT s = 0; (s < seriesData_ptr->noEncapDocDatas()); s++)
				{
					ENCAP_DOC_INFO_CLASS *encapData_ptr = seriesData_ptr->getEncapDocData(s);

					if (encapData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(encapData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT t = 0; (t < seriesData_ptr->noRegistrationDatas()); t++)
				{
					REGISTRATION_INFO_CLASS *regData_ptr = seriesData_ptr->getRegistrationData(t);

					if (regData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(regData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT u = 0; (u < seriesData_ptr->noFiducialDatas()); u++)
				{
					FIDUCIAL_INFO_CLASS *fedData_ptr = seriesData_ptr->getFiducialData(u);

					if (fedData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(fedData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT v = 0; (v < seriesData_ptr->noRTDoseDatas()); v++)
				{
					RT_DOSE_INFO_CLASS *rtDoseData_ptr = seriesData_ptr->getRTDoseData(v);

					if (rtDoseData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(rtDoseData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT w = 0; (w < seriesData_ptr->noRTStructSetDatas()); w++)
				{
					RT_STRUC_SET_INFO_CLASS *rtStrucData_ptr = seriesData_ptr->getRTStructSetData(w);

					if (rtStrucData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(rtStrucData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT x = 0; (x < seriesData_ptr->noRTPlanDatas()); x++)
				{
					RT_PLAN_INFO_CLASS *rtPlanData_ptr = seriesData_ptr->getRTPlanData(x);

					if (rtPlanData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(rtPlanData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT y = 0; (y < seriesData_ptr->noRTTreatDatas()); y++)
				{
					RT_TREATMENT_INFO_CLASS *rtTreatData_ptr = seriesData_ptr->getRTTreatData(y);

					if (rtTreatData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(rtTreatData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT z = 0; (z < seriesData_ptr->noValueMapDatas()); z++)
				{
					VALUE_MAP_INFO_CLASS *valueData_ptr = seriesData_ptr->getValueMapData(z);

					if (valueData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(valueData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}

				for (UINT zz = 0; (zz < seriesData_ptr->noKeyobjectDatas()); zz++)
				{
					KEY_OBJECT_DOC_INFO_CLASS *keyData_ptr = seriesData_ptr->getKeyobjectData(zz);

					if (keyData_ptr != NULL)
					{
						// instantiate the item
						DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

						// cascade the logger
						item_ptr->setLogger(loggerM_ptr);
						item_ptr->setDefinedLength(true);
						item_ptr->setIdentifier(keyData_ptr->getIdentifier());
						item_ptr->setValueByReference(true);

						sq_value_ptr->addItem(item_ptr);
					}
				}
			}
		}
	}

	sqAttribute_ptr->addSqValue(sq_value_ptr);

	return sqAttribute_ptr;
}

//>>===========================================================================

bool GENERATE_DICOMDIR_CLASS::writeDICOMDIR(string filename)

//  DESCRIPTION     : Function to write the DICOMDIR to the given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_INFO, 2, "Writing DICOMDIR to the file %s", filename.c_str());
	}

	// try to retrive the file head from the warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *widFileHead_ptr = WAREHOUSE->retrieve("", WID_FILEHEAD);
	if (widFileHead_ptr)
	{
		// log the action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 2, "WRITE %s (from Data Warehouse) to %s", WIDName(WID_FILEHEAD), filename.c_str());
		}

		FILEHEAD_CLASS *filehead_ptr = static_cast<FILEHEAD_CLASS*>(widFileHead_ptr);

		// set up the write file
		filehead_ptr->setFilename(correctPathnameForOS(filename));

		// write the file head to the file
		result = filehead_ptr->write(false);
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Can't write %s from Data Warehouse to %s", WIDName(WID_FILEHEAD), filename.c_str());
		}
	}

	// try to retrive the file meta from the warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *widFileMeta_ptr = WAREHOUSE->retrieve(fileMetaIdentifier, WID_META_INFO);
	if (widFileMeta_ptr)
	{
		// log the action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 2, "WRITE %s (from Data Warehouse) to %s", WIDName(WID_META_INFO), filename.c_str());
		}

		DCM_DATASET_CLASS *filemeta_ptr = static_cast<DCM_DATASET_CLASS*>(widFileMeta_ptr);

		filemeta_ptr->setPopulateWithAttributes(false);

		// set up the write file
		FILE_DATASET_CLASS	fileDataset(correctPathnameForOS(filename));

		// cascade the logger
		fileDataset.setLogger(loggerM_ptr);

		// write the dataset to the file
		result = fileDataset.write(filemeta_ptr);
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Can't write %s from Data Warehouse to %s", WIDName(WID_META_INFO), filename.c_str());
		}
	}

	// try to retrive the file meta from the warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *widMediaDirectory_ptr = WAREHOUSE->retrieve("DICOMDIR", WID_DATASET);
	if (widMediaDirectory_ptr)
	{
		// log the action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 2, "WRITE %s (from Data Warehouse) to %s", WIDName(WID_DATASET), filename.c_str());
		}

		DCM_DATASET_CLASS *mediaDirectory_ptr = static_cast<DCM_DATASET_CLASS*>(widMediaDirectory_ptr);

		mediaDirectory_ptr->setPopulateWithAttributes(false);

		// set up the write file
		FILE_DATASET_CLASS	mediaDirectoryDataset(correctPathnameForOS(filename));

		// cascade the logger
		mediaDirectoryDataset.setLogger(loggerM_ptr);

		// write the dataset to the file
		result = mediaDirectoryDataset.write(mediaDirectory_ptr);
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Can't write %s from Data Warehouse to %s", WIDName(WID_DATASET), filename.c_str());
		}
	}

	// try to retrive the file tail from the warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *widFileTail_ptr = WAREHOUSE->retrieve("", WID_FILETAIL);
	if (widFileTail_ptr)
	{
		// log the action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 2, "WRITE %s (from Data Warehouse) to %s", WIDName(WID_FILETAIL), filename.c_str());
		}

		FILETAIL_CLASS *filetail_ptr = static_cast<FILETAIL_CLASS*>(widFileTail_ptr);

		// set up the write file
		filetail_ptr->setFilename(correctPathnameForOS(filename));

		// write the file tail to the file
		result = filetail_ptr->write(false);
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Can't write %s from Data Warehouse to %s", WIDName(WID_FILETAIL), filename.c_str());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

void GENERATE_DICOMDIR_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set the Logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    loggerM_ptr = logger_ptr; 
}
