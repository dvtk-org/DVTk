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
#ifndef GENERATEDICOMDIR_H
#define GENERATEDICOMDIR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Iemulator.h"		// Definition component interface
#include "Imedia.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_CLASS;
#define fileMetaIdentifier  "FILE METAINFO"

//>>***************************************************************************

class IMAGE_INFO_CLASS

//  DESCRIPTION     : Image Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string instanceUid;
	string refFileId;
	string refSOPClassUid;
	string refSOPClassInstanceUid;
	string refTSUid;
	INT32 instanceNr;
	vector<string> charSets;
	string identifier;
	UINT count;

public:
	IMAGE_INFO_CLASS(string, string, string, string, string, INT32, vector<string>, string);

	~IMAGE_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceUid() 
		{ return instanceUid; }

	string getRefFileId() 
		{ return refFileId; }

	string getRefSOPClassUid() 
		{ return refSOPClassUid; }

	string getRefSOPClassInstanceUid() 
		{ return refSOPClassInstanceUid; }

	string getRefTSUid() 
		{ return refTSUid;	 }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (IMAGE_INFO_CLASS&);
};

//>>***************************************************************************

class WAVEFORM_INFO_CLASS

//  DESCRIPTION     : Waveform Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string wfCreationDate;
	string wfCreationTime;
	string identifier;
	UINT count;

public:
	WAVEFORM_INFO_CLASS(INT32, vector<string>,string, string, string);

	~WAVEFORM_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getWFCreationDate() 
		{ return wfCreationDate;	 }

	string getWFCreationTime() 
		{ return wfCreationTime;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (WAVEFORM_INFO_CLASS&);
};

//>>***************************************************************************

class RAWDATA_INFO_CLASS

//  DESCRIPTION     : Raw Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string rdCreationDate;
	string rdCreationTime;
	string identifier;
	UINT count;

public:
	RAWDATA_INFO_CLASS(INT32, vector<string>,string, string, string);

	~RAWDATA_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getRDCreationDate() 
		{ return rdCreationDate;	 }

	string getRDCreationTime() 
		{ return rdCreationTime;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (RAWDATA_INFO_CLASS&);
};

//>>***************************************************************************

class SPECTROSCOPY_INFO_CLASS

//  DESCRIPTION     : SPECTROSCOPY Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string imageType;
	string ssCreationDate;
	string ssCreationTime;
	DCM_VALUE_SQ_CLASS *refImageEvidenceSeqValuePtr;
	string identifier;
	UINT count;

public:
	SPECTROSCOPY_INFO_CLASS(INT32, vector<string>,string,string, string, DCM_VALUE_SQ_CLASS *,string);

	~SPECTROSCOPY_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getImageType() 
		{ return imageType;	 }

	string getSSCreationDate() 
		{ return ssCreationDate;	 }

	string getSSCreationTime() 
		{ return ssCreationTime;	 }

	DCM_VALUE_SQ_CLASS * getRefImageEvidenceSeqValue() 
		{ return refImageEvidenceSeqValuePtr; }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (SPECTROSCOPY_INFO_CLASS&);
};

//>>***************************************************************************

class SRDOC_INFO_CLASS

//  DESCRIPTION     : SPECTROSCOPY Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string compFlag;
	string verFlag;
	string contentDate;
	string contentTime;
	string verDateTime;
	DCM_VALUE_SQ_CLASS *conCodeSeqValuePtr;
	DCM_VALUE_SQ_CLASS *conSeqValuePtr;
	string identifier;
	UINT count;

public:
	SRDOC_INFO_CLASS(INT32, vector<string>,string,string, string,string,string,DCM_VALUE_SQ_CLASS *,DCM_VALUE_SQ_CLASS *,string);

	~SRDOC_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getCompFlag() 
		{ return compFlag;	 }

	string getVerFlag() 
		{ return verFlag;	 }

	string getContentDate() 
		{ return contentDate;	 }

	string getContentTime() 
		{ return contentTime;	 }

	string getVerDateTime() 
		{ return verDateTime;	 }

	DCM_VALUE_SQ_CLASS * getConCodeSeqValue() 
		{ return conCodeSeqValuePtr; }

	DCM_VALUE_SQ_CLASS * getConSeqValue() 
		{ return conSeqValuePtr; }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (SRDOC_INFO_CLASS&);
};

//>>***************************************************************************

class RT_DOSE_INFO_CLASS

//  DESCRIPTION     : RT Dose Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string doseSummType;
	string identifier;
	UINT count;

public:
	RT_DOSE_INFO_CLASS(INT32, vector<string>,string, string);

	~RT_DOSE_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getDoseSummationType() 
		{ return doseSummType;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (RT_DOSE_INFO_CLASS&);
};

//>>***************************************************************************

class RT_STRUC_SET_INFO_CLASS

//  DESCRIPTION     : RT Dose Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string rtStrucSetLable;
	string rtStrucSetDate;
	string rtStrucSetTime;
	string identifier;
	UINT count;

public:
	RT_STRUC_SET_INFO_CLASS(INT32, vector<string>,string, string, string,string);

	~RT_STRUC_SET_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getRTStrucSetLable() 
		{ return rtStrucSetLable;	 }

	string getRTStrucSetDate() 
		{ return rtStrucSetDate;	 }

	string getRTStrucSetTime() 
		{ return rtStrucSetTime;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (RT_STRUC_SET_INFO_CLASS&);
};

//>>***************************************************************************

class RT_PLAN_INFO_CLASS

//  DESCRIPTION     : RT Plan Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string rtPlanLable;
	string rtPlanDate;
	string rtPlanTime;
	string identifier;
	UINT count;

public:
	RT_PLAN_INFO_CLASS(INT32, vector<string>,string, string, string,string);

	~RT_PLAN_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getRTPlanLable() 
		{ return rtPlanLable;	 }

	string getRTPlanDate() 
		{ return rtPlanDate;	 }

	string getRTPlanTime() 
		{ return rtPlanTime;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (RT_PLAN_INFO_CLASS&);
};

//>>***************************************************************************

class RT_TREATMENT_INFO_CLASS

//  DESCRIPTION     : RT TREATMENT Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string rtTreatDate;
	string rtTreatTime;
	string identifier;
	UINT count;

public:
	RT_TREATMENT_INFO_CLASS(INT32, vector<string>,string, string, string);

	~RT_TREATMENT_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getRTTreatDate() 
		{ return rtTreatDate;	 }

	string getRTTreatTime() 
		{ return rtTreatTime;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (RT_TREATMENT_INFO_CLASS&);
};

//>>***************************************************************************

class REGISTRATION_INFO_CLASS

//  DESCRIPTION     : REGISTRATION Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	vector<string> charSets;
	string contentDate;
	string contentTime;
	string identifier;
	UINT count;

public:
	REGISTRATION_INFO_CLASS(vector<string>,string,string, string);

	~REGISTRATION_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getContentDate() 
		{ return contentDate;	 }

	string getContentTime() 
		{ return contentTime;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (REGISTRATION_INFO_CLASS&);
};

//>>***************************************************************************

class FIDUCIAL_INFO_CLASS

//  DESCRIPTION     : FIDUCIAL Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	vector<string> charSets;
	string contentDate;
	string contentTime;
	string identifier;
	UINT count;

public:
	FIDUCIAL_INFO_CLASS(vector<string>,string,string, string);

	~FIDUCIAL_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getContentDate() 
		{ return contentDate;	 }

	string getContentTime() 
		{ return contentTime;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (FIDUCIAL_INFO_CLASS&);
};

//>>***************************************************************************

class VALUE_MAP_INFO_CLASS

//  DESCRIPTION     : Value Map Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	vector<string> charSets;
	string contentDate;
	string contentTime;
	string identifier;
	UINT count;

public:
	VALUE_MAP_INFO_CLASS(vector<string>,string,string, string);

	~VALUE_MAP_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getContentDate() 
		{ return contentDate;	 }

	string getContentTime() 
		{ return contentTime;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (VALUE_MAP_INFO_CLASS&);
};

//>>***************************************************************************

class HL7_SRDOC_INFO_CLASS

//  DESCRIPTION     : HL7 SR DOC Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string hl7InstanceIdentifier;
	string hl7EffectiveTime;
	DCM_VALUE_SQ_CLASS *hl7DocTypeCodeSeqPtr;
	string docTitle;
	string identifier;
	UINT count;

public:
	HL7_SRDOC_INFO_CLASS(INT32, vector<string>,string,string, string,DCM_VALUE_SQ_CLASS *,string);

	~HL7_SRDOC_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getHL7InstanceIdentifier() 
		{ return hl7InstanceIdentifier;	 }

	string getHL7EffectiveTime() 
		{ return hl7EffectiveTime;	 }

	DCM_VALUE_SQ_CLASS * getHL7DocTypeCodeSeq() 
		{ return hl7DocTypeCodeSeqPtr; }

	string getDocTitle() 
		{ return docTitle;	 }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (HL7_SRDOC_INFO_CLASS&);
};

//>>***************************************************************************

class ENCAP_DOC_INFO_CLASS

//  DESCRIPTION     : Encapsulated Doc Info Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string contentDate;
	string contentTime;
	string docTitle;
	string hl7InstanceIdentifier;
	string varMIMETypeEncapDoc;
	DCM_VALUE_SQ_CLASS *conNameCodeSeqValuePtr;
	string identifier;
	UINT count;

public:
	ENCAP_DOC_INFO_CLASS(INT32, vector<string>,string,string, string,string,string,DCM_VALUE_SQ_CLASS *,string);

	~ENCAP_DOC_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getContentDate() 
		{ return contentDate;	 }

	string getContentTime() 
		{ return contentTime;	 }

	string getDocTitle() 
		{ return docTitle;	 }

	string getHL7InstanceIdentifier() 
		{ return hl7InstanceIdentifier;	 }

	string getMIMETypeEncapDoc() 
		{ return varMIMETypeEncapDoc;	 }

	DCM_VALUE_SQ_CLASS * getConNameCodeSeqValue() 
		{ return conNameCodeSeqValuePtr; }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (ENCAP_DOC_INFO_CLASS&);
};

//>>***************************************************************************

class HANGING_PROTOCOL_INFO_CLASS

//  DESCRIPTION     : HANGING PROTOCOL Info Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string hangingProtoName;
	string hangingProtoDesc;
	string hangingProtoLevel;
	string hangingProtoCreator;
	string hangingProtoCreationDtTime;
	DCM_VALUE_SQ_CLASS *hangProtoDefSeqPtr;
	string nrOfPriorsRef;
	DCM_VALUE_SQ_CLASS *hangProtoUserIdentificSeqPtr;
	string identifier;
	UINT count;

public:
	HANGING_PROTOCOL_INFO_CLASS(INT32, vector<string>,string,string, string,string,string,DCM_VALUE_SQ_CLASS *,string,DCM_VALUE_SQ_CLASS *,string);

	~HANGING_PROTOCOL_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getHangingProtoName() 
		{ return hangingProtoName;	 }

	string getHangingProtoDesc() 
		{ return hangingProtoDesc;	 }

	string getHangingProtoLevel() 
		{ return hangingProtoLevel;	 }

	string getHangingProtoCreator() 
		{ return hangingProtoCreator;	 }

	string getHangingProtoCreationDtTime() 
		{ return hangingProtoCreationDtTime;	 }

	DCM_VALUE_SQ_CLASS * getHangProtoDefSeqPtr() 
		{ return hangProtoDefSeqPtr; }

	string getNrOfPriorsRef() 
		{ return nrOfPriorsRef;	 }

	DCM_VALUE_SQ_CLASS * getHangProtoUserIdentificSeqPtr() 
		{ return hangProtoUserIdentificSeqPtr; }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (HANGING_PROTOCOL_INFO_CLASS&);
};

//>>***************************************************************************

class KEY_OBJECT_DOC_INFO_CLASS

//  DESCRIPTION     : KEY OBJECT DOC Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	vector<string> charSets;
	string contentDate;
	string contentTime;
	DCM_VALUE_SQ_CLASS *conCodeSeqValuePtr;
	DCM_VALUE_SQ_CLASS *conSeqValuePtr;
	string identifier;
	UINT count;

public:
	KEY_OBJECT_DOC_INFO_CLASS(INT32, vector<string>,string,string, DCM_VALUE_SQ_CLASS *,DCM_VALUE_SQ_CLASS *,string);

	~KEY_OBJECT_DOC_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getContentDate() 
		{ return contentDate;	 }

	string getContentTime() 
		{ return contentTime;	 }

	DCM_VALUE_SQ_CLASS * getConCodeSeqValue() 
		{ return conCodeSeqValuePtr; }

	DCM_VALUE_SQ_CLASS * getConSeqValue() 
		{ return conSeqValuePtr; }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (KEY_OBJECT_DOC_INFO_CLASS&);
};

//>>***************************************************************************

class PRESENTATION_STATE_INFO_CLASS

//  DESCRIPTION     : Presentation State Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	INT32 instanceNr;
	string refFileId;
	string refSOPClassUid;
	string refSOPClassInstanceUid;
	string refTSUid;
	DCM_VALUE_SQ_CLASS *refSeriesSeqValuePtr;
	string psCreationDate;
	string psCreationTime;
	string contentLable;
	string contentDesc;
	string contentCreator;
	string identifier;
	UINT count;

public:
	PRESENTATION_STATE_INFO_CLASS(INT32, string, string, string, string, DCM_VALUE_SQ_CLASS *,string, string, string, string, string, string);

	~PRESENTATION_STATE_INFO_CLASS();

	void incrementCount() 
		{ count++; }

	string getInstanceNr() 
		{
			char base[100];
			if(instanceNr == -1)return "";
			_itoa(instanceNr,base,10);
			string instanceStr = base;
			return instanceStr;
		}

	string getRefFileId() 
		{ return refFileId; }

	string getRefSOPClassUid() 
		{ return refSOPClassUid; }

	string getRefSOPClassInstanceUid() 
		{ return refSOPClassInstanceUid; }

	string getRefTSUid() 
		{ return refTSUid;	 }

	DCM_VALUE_SQ_CLASS * getRefSeriesSeqValue() 
		{ return refSeriesSeqValuePtr; }

	string getPSCreationDate() 
		{ return psCreationDate;	 }

	string getPSCreationTime() 
		{ return psCreationTime;	 }

	string getContentLable() 
		{ return contentLable; }

	string getContentDesc() 
		{ return contentDesc; }

	string getContentCreator() 
		{ return contentCreator; }

	UINT getCount() 
		{ return count; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	bool operator = (PRESENTATION_STATE_INFO_CLASS&);
};

//>>***************************************************************************

class SERIES_INFO_CLASS

//  DESCRIPTION     : Study Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string instanceUid;
	string modality;
	INT32 seriesNr;
	string identifier;
	ARRAY<IMAGE_INFO_CLASS*> sopInstanceData;
	ARRAY<PRESENTATION_STATE_INFO_CLASS*> presentationStateData;
	ARRAY<WAVEFORM_INFO_CLASS*> waveformData;
	ARRAY<RAWDATA_INFO_CLASS*> rawData;
	ARRAY<SPECTROSCOPY_INFO_CLASS*> spectData;
	ARRAY<SRDOC_INFO_CLASS*> srDocData;
	ARRAY<ENCAP_DOC_INFO_CLASS*> encapDocData;
	ARRAY<REGISTRATION_INFO_CLASS*> registrationData;
	ARRAY<FIDUCIAL_INFO_CLASS*> fiducialData;
	ARRAY<RT_DOSE_INFO_CLASS*> rtDoseData;
	ARRAY<RT_STRUC_SET_INFO_CLASS*> rtStructData;
	ARRAY<RT_PLAN_INFO_CLASS*> rtPlanData;
	ARRAY<RT_TREATMENT_INFO_CLASS*> rtTreatData;
	ARRAY<VALUE_MAP_INFO_CLASS*> valueMapData;
	ARRAY<KEY_OBJECT_DOC_INFO_CLASS*> keyObjData;

public:
	SERIES_INFO_CLASS(string,string,INT32,string);
		
	~SERIES_INFO_CLASS();
		
	string getInstanceUid() 
		{ return instanceUid; }

	string getModality() 
		{ return modality; }

	string getSeriesNr() 
		{
			char base[100];
			if(seriesNr == -1)return "";
			_itoa(seriesNr,base,10);
			string seriesStr = base;
			return seriesStr; 
		}

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	UINT noSopInstances() 
		{ return sopInstanceData.getSize(); }

	IMAGE_INFO_CLASS *getSopInstanceData(UINT i) 
		{ return sopInstanceData[i]; }

	void addSopInstanceData(IMAGE_INFO_CLASS *sopInstanceData_ptr)
		{ sopInstanceData.add(sopInstanceData_ptr); }

	IMAGE_INFO_CLASS *searchImage(string);

	UINT noPresentationStates() 
		{ return presentationStateData.getSize(); }

	PRESENTATION_STATE_INFO_CLASS *getPSData(UINT i) 
		{ return presentationStateData[i]; }

	void addPSData(PRESENTATION_STATE_INFO_CLASS *psData_ptr)
		{ presentationStateData.add(psData_ptr); }

	PRESENTATION_STATE_INFO_CLASS *searchPS(string);

	UINT noWaveforms() 
		{ return waveformData.getSize(); }

	WAVEFORM_INFO_CLASS *getWFData(UINT i) 
		{ return waveformData[i]; }

	void addWFData(WAVEFORM_INFO_CLASS *wfData_ptr)
		{ waveformData.add(wfData_ptr); }

	WAVEFORM_INFO_CLASS *searchWF(string,string);

	UINT noRawDatas() 
		{ return rawData.getSize(); }

	RAWDATA_INFO_CLASS *getRawData(UINT i) 
		{ return rawData[i]; }

	void addRawData(RAWDATA_INFO_CLASS *rawData_ptr)
		{ rawData.add(rawData_ptr); }

	RAWDATA_INFO_CLASS *searchRawData(string,string);

	UINT noSpectDatas() 
		{ return spectData.getSize(); }

	SPECTROSCOPY_INFO_CLASS *getSpectData(UINT i) 
		{ return spectData[i]; }

	void addSpectData(SPECTROSCOPY_INFO_CLASS *spectData_ptr)
		{ spectData.add(spectData_ptr); }

	SPECTROSCOPY_INFO_CLASS *searchSpectData(string,string);

	UINT noSRDocDatas() 
		{ return srDocData.getSize(); }

	SRDOC_INFO_CLASS *getSRDocData(UINT i) 
		{ return srDocData[i]; }

	void addSRDocData(SRDOC_INFO_CLASS *srDocData_ptr)
		{ srDocData.add(srDocData_ptr); }

	SRDOC_INFO_CLASS *searchSRDoc(string,string);

	UINT noEncapDocDatas() 
		{ return encapDocData.getSize(); }

	ENCAP_DOC_INFO_CLASS *getEncapDocData(UINT i) 
		{ return encapDocData[i]; }

	void addEncapDocData(ENCAP_DOC_INFO_CLASS *encapDocData_ptr)
		{ encapDocData.add(encapDocData_ptr); }

	ENCAP_DOC_INFO_CLASS *searchEncapDocData(string,string);

	UINT noRegistrationDatas() 
		{ return registrationData.getSize(); }

	REGISTRATION_INFO_CLASS *getRegistrationData(UINT i) 
		{ return registrationData[i]; }

	void addRegistrationData(REGISTRATION_INFO_CLASS *regData_ptr)
		{ registrationData.add(regData_ptr); }

	REGISTRATION_INFO_CLASS *searchRegistration(string,string);

	UINT noFiducialDatas() 
		{ return fiducialData.getSize(); }

	FIDUCIAL_INFO_CLASS *getFiducialData(UINT i) 
		{ return fiducialData[i]; }

	void addFiducialData(FIDUCIAL_INFO_CLASS *fiducialData_ptr)
		{ fiducialData.add(fiducialData_ptr); }

	FIDUCIAL_INFO_CLASS *searchFiducial(string,string);

	UINT noRTDoseDatas() 
		{ return rtDoseData.getSize(); }

	RT_DOSE_INFO_CLASS *getRTDoseData(UINT i) 
		{ return rtDoseData[i]; }

	void addRTDoseData(RT_DOSE_INFO_CLASS *rtDoseData_ptr)
		{ rtDoseData.add(rtDoseData_ptr); }

	RT_DOSE_INFO_CLASS *searchRTDose(string);

	UINT noRTStructSetDatas() 
		{ return rtStructData.getSize(); }

	RT_STRUC_SET_INFO_CLASS *getRTStructSetData(UINT i) 
		{ return rtStructData[i]; }

	void addRTStructSetData(RT_STRUC_SET_INFO_CLASS *rtStructData_ptr)
		{ rtStructData.add(rtStructData_ptr); }

	RT_STRUC_SET_INFO_CLASS *searchRTStructSet(string,string);

	UINT noRTPlanDatas() 
		{ return rtPlanData.getSize(); }

	RT_PLAN_INFO_CLASS *getRTPlanData(UINT i) 
		{ return rtPlanData[i]; }

	void addRTPlanData(RT_PLAN_INFO_CLASS *rtPlanData_ptr)
		{ rtPlanData.add(rtPlanData_ptr); }

	RT_PLAN_INFO_CLASS *searchRTPlan(string,string);

	UINT noRTTreatDatas() 
		{ return rtTreatData.getSize(); }

	RT_TREATMENT_INFO_CLASS *getRTTreatData(UINT i) 
		{ return rtTreatData[i]; }

	void addRTTreatData(RT_TREATMENT_INFO_CLASS *rtTreatData_ptr)
		{ rtTreatData.add(rtTreatData_ptr); }

	RT_TREATMENT_INFO_CLASS *searchRTTreat(string,string);

	UINT noValueMapDatas() 
		{ return valueMapData.getSize(); }

	VALUE_MAP_INFO_CLASS *getValueMapData(UINT i) 
		{ return valueMapData[i]; }

	void addValueMapData(VALUE_MAP_INFO_CLASS *valueMapData_ptr)
		{ valueMapData.add(valueMapData_ptr); }

	VALUE_MAP_INFO_CLASS *searchValueMap(string,string);

	UINT noKeyobjectDatas() 
		{ return srDocData.getSize(); }

	KEY_OBJECT_DOC_INFO_CLASS *getKeyobjectData(UINT i) 
		{ return keyObjData[i]; }

	void addKeyobjectData(KEY_OBJECT_DOC_INFO_CLASS *keyObjData_ptr)
		{ keyObjData.add(keyObjData_ptr); }

	KEY_OBJECT_DOC_INFO_CLASS *searchKeyobject(string,string);

	bool operator = (SERIES_INFO_CLASS&);
};

//>>***************************************************************************

class STUDY_INFO_CLASS

//  DESCRIPTION     : Series Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string instanceUid;
	string studyId;
	string studyDate;
	string studyTime;
	string studyDescr;
	string accessionNr;
	string identifier;
	ARRAY<SERIES_INFO_CLASS*> seriesData;

public:
	STUDY_INFO_CLASS(string,string,string,string,string,string,string);

	~STUDY_INFO_CLASS();

	string getInstanceUid() 
		{ return instanceUid; }

	string getStudyId() 
		{ return studyId; }

	string getStudyDate() 
		{ return studyDate; }

	string getStudyTime() 
		{ return studyTime; }

	string getStudyDescr() 
		{ return studyDescr; }

	string getAccessionNr() 
		{ return accessionNr; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	UINT noSeries()
		{ return seriesData.getSize(); }

	SERIES_INFO_CLASS *getSeriesData(UINT i)
		{ return seriesData[i]; }

	void addSeriesData(SERIES_INFO_CLASS *seriesData_ptr)
		{ seriesData.add(seriesData_ptr); }

	SERIES_INFO_CLASS *searchSeries(string);

	bool operator = (STUDY_INFO_CLASS&);
};

//>>***************************************************************************

class PATIENT_INFO_CLASS

//  DESCRIPTION     : Patient Data Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string patientName;
	string patientId;
	vector<string> charSets;
	string identifier;
	ARRAY<STUDY_INFO_CLASS*> studyData;
	ARRAY<HL7_SRDOC_INFO_CLASS*> hl7StructDocData;

public:
	PATIENT_INFO_CLASS(string, string, vector<string>, string);

	~PATIENT_INFO_CLASS();

	string getPatientName() 
		{ return patientName; }

	string getPatientId() 
		{ return patientId; }

	vector<string> getSpCharSetValues() 
		{ return charSets; }

	string getIdentifier() 
		{ return identifier; }

	void setIdentifier(string Identifier) 
		{ identifier = Identifier; }

	UINT noStudies()
		{ return studyData.getSize(); }

	STUDY_INFO_CLASS *getStudyData(UINT i) 
		{ return studyData[i]; }

	void addStudyData(STUDY_INFO_CLASS *studyData_ptr)
		{ studyData.add(studyData_ptr); }

	STUDY_INFO_CLASS *searchStudy(string);

	UINT noHL7StructDocs()
		{ return hl7StructDocData.getSize(); }

	HL7_SRDOC_INFO_CLASS *getHL7StructDocData(UINT i) 
		{ return hl7StructDocData[i]; }

	void addHL7StructDocData(HL7_SRDOC_INFO_CLASS *hl7StructDocData_ptr)
		{ hl7StructDocData.add(hl7StructDocData_ptr); }

	HL7_SRDOC_INFO_CLASS *searchHL7StructDoc(string);

	bool operator = (PATIENT_INFO_CLASS&);
};

//>>***************************************************************************

class GENERATE_DICOMDIR_CLASS

//  DESCRIPTION     : File based DICOM Dataset
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	LOG_CLASS				*loggerM_ptr;
	ARRAY<PATIENT_INFO_CLASS*>	patientData;
	ARRAY<HANGING_PROTOCOL_INFO_CLASS*>	hangingProtocolData;
    vector<FMI_DATASET_STRUCT>  filedatasetsM;
	string dicomdirPathM;
	bool isPSPresent;
	bool isImagePresent;
	bool isWaveFormPresent;
	bool isRawDataPresent;
	bool isSpectroscopyPresent;
	bool isSrDocPresent;
	bool isRTDosePresent;
	bool isRTStructSetPresent;
	bool isRTPlanPresent;
	bool isRTTreatPresent;
	bool isEncapDocPresent;
	bool isRegistrationPresent;
	bool isFiducialPresent;
	bool isKeyObjDocPresent;
	bool isValueMapPresent;

	bool readDCMFiles(string filename);

	PATIENT_INFO_CLASS *searchPatient(string id, string name);

	HANGING_PROTOCOL_INFO_CLASS *searchHangingProtocol(string name);

	void analyseStorageDataset(DCM_DATASET_CLASS* dataset_ptr, string fileName, string ts);

	bool CreateDICOMObjects();

	bool CreateAndStoreRecords();

	bool CreateAndStoreDirectorySequenceObject();

	DCM_ATTRIBUTE_CLASS *getULAttribute(string identifier, UINT32 tag);

	DCM_ATTRIBUTE_CLASS *getSQAttribute();

	bool writeDICOMDIR(string filename);

public:
	GENERATE_DICOMDIR_CLASS(string);

	~GENERATE_DICOMDIR_CLASS();

	bool generateDICOMDIR(vector<string>* filenames);

	void setLogger(LOG_CLASS*);
};

#endif /* FILEDATASET_H */
