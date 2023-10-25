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
//  DESCRIPTION     :	Accepted (Negotiated) SOP Classes Class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "accepted.h"
#include "assoc_ac.h"		// ASSOCIATE-AC
#include "assoc_rq.h"		// ASSOCIATE-RQ
#include "Idefinition.h"	// Definition File component interface
#include "Ilog.h"

T_TS_MAP	TTrnStx[MAX_TRN_STX_NAMES] = {
	{IMPLICIT_VR_LITTLE_ENDIAN,							"Implicit VR Little Endian",		TS_IMPLICIT_VR | TS_LITTLE_ENDIAN},
	{EXPLICIT_VR_LITTLE_ENDIAN,							"Explicit VR Little Endian",		TS_EXPLICIT_VR | TS_LITTLE_ENDIAN},
	{EXPLICIT_VR_BIG_ENDIAN,							"Explicit VR Big Endian",			TS_EXPLICIT_VR | TS_BIG_ENDIAN},
	{JPEG_BASELINE_PROCESS1,							"JPEG Baseline Process 1",			TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_EXTENDED_PROCESS2AND4,						"JPEG Extended Process 2 And 4",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_EXTENDED_PROCESS3AND5,						"JPEG Extended Process 3 And 5",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_SPECTRAL_PROCESS6AND8,						"JPEG Spectral Process 6 And 8",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_SPECTRAL_PROCESS7AND9,						"JPEG Spectral Process 7 And 9",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_FULL_PROCESS10AND12,							"JPEG Full Process 10 And 12",		TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_FULL_PROCESS11AND13,							"JPEG Full Process 11 And 13",		TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_LOSSLESS14,									"JPEG Lossless 14",					TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_LOSSLESS15,									"JPEG Lossless 15",					TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_EXTENDED_PROCESS16AND18,						"JPEG Extended Process 16 And 18",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_EXTENDED_PROCESS17AND19,						"JPEG Extended Process 17 And 19",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_SPECTRAL_PROCESS20AND22,						"JPEG Spectral Process 20 And 22",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_SPECTRAL_PROCESS21AND23,						"JPEG Spectral Process 21 And 23",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_FULL_PROCESS24AND26,							"JPEG Full Process 24 And 26",		TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_FULL_PROCESS25AND27,							"JPEG Full Process 25 And 27",		TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_LOSSLESS28,									"JPEG Lossless 28",					TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_LOSSLESS29,									"JPEG Lossless 29",					TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_LOSSLESS_FIRST_ORDER14,						"JPEG Lossless First Order 14",		TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_LS_LOSSLESS,									"JPEG-LS Lossless",					TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_LS_LOSSY,										"JPEG-LS Lossy (Near-Lossless)",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_2000_IC_LOSSLESS_ONLY,						"JPEG 2000 (Lossless Only)",		TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_2000_IC,										"JPEG 2000",						TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_2000_MULTICOMPONENT_LOSSLESS2,				"JPEG 2000 Multi-component lossless",TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPEG_2000_MULTICOMPONENT2,							"JPEG 2000 Part 2 Multi-component",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPIP_REFERENCED,									"JPIP Referenced",					TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{JPIP_REFERENCED_DEFLATE,							"JPIP Referenced Deflate",			TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{MPEG2_MAIN_PROFILE_LEVEL,							"MPEG2 Main Profile",				TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{MPEG2_HIGH_PROFILE_LEVEL,							"MPEG2 High Profile",				TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{RFC_2557_MIME_ENCAPSULATION,						"RFC 2557 MIME encapsulation",		TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{XML_Encoding,										"XML Encoding",						TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{DEFLATED_EXPLICIT_LITTLE_ENDIAN,					"Deflated ExplicitVR Little Endian",TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{RLE_LOSSLESS,										"Run Length Lossless",				TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{MPEG4_AVC_H_264_High_Profile41,					"MPEG4 AVC H264 High Profile Level 41",				TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{MPEG4_AVC_H_264_BD_compatible_High_Profile41,		"MPEG4 AVC H264 BD compatible High Profile Level 41",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{MPEG4_AVC_H_264_High_Profile42For2D_Video,			"MPEG4 AVC H264 High Profile Level 42 For 2D Video",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{MPEG4_AVC_H_264_High_Profile42For3D_Video,			"MPEG4 AVC H264 High Profile Level 42 For 3D Video",	TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{MPEG4_AVC_H_264_Stereo_High_Profile42,				"MPEG4 AVC H264 Stereo High Profile Level 42",			TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{HEVC_H265_Main_Profile51,							"HEVC H265 Main Profile Level 51",						TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED},
	{HEVC_H265_Main_10_Profile51,						"HEVC H265 Main 10 Profile Level 51",					TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED}
};

T_CHAR_TEXT_MAP	TAbsStx[MAX_APP_CTX_NAMES] = {
	{APPLICATION_CONTEXT_NAME,			"DICOM Application Context Name"}
};


//>>===========================================================================

BASE_PRESENTATION_CONTEXT_CLASS::BASE_PRESENTATION_CONTEXT_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	loggerM_ptr = NULL;
}


//>>===========================================================================

BASE_PRESENTATION_CONTEXT_CLASS::~BASE_PRESENTATION_CONTEXT_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// release any Presentation Contexts
	while (presentationContextM.getSize()) 
	{
		presentationContextM.removeAt(0);
	}
}


//>>===========================================================================

bool BASE_PRESENTATION_CONTEXT_CLASS::getTransferSyntaxUid(BYTE pcId, UID_CLASS &uid)

//  DESCRIPTION     : Method to search the Presentation Contexts
//					  for a matching pcId. The Transfer Syntax of the match
//					  id returned.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// simple search through list
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{
		// if presentation context id matches and it is accepted
		if ((presentationContextM[i].getPresentationContextId() == pcId)
		 && (presentationContextM[i].getResultReason() == ACCEPTANCE)) 
		{
			// match found - return Transfer Syntax
			uid = presentationContextM[i].getTransferSyntaxName();
			result = true;
			break;
		}
	}

	// return result
	return result;
}


//>>===========================================================================

BYTE BASE_PRESENTATION_CONTEXT_CLASS::getPresentationContextId(UID_CLASS uid)

//  DESCRIPTION     : Method to search the Presentation Contexts
//					  for a matching Abstract Syntax Name. The pcId of the
//					  match is returned.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// use Abstract Syntax Name to search through list
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{
		// if abstract syntax names match and it is accepted
		if ((presentationContextM[i].getAbstractSyntaxName() == uid)
		 && (presentationContextM[i].getResultReason() == ACCEPTANCE)) 
		{
			// match found - return Presentation Context Id
			return presentationContextM[i].getPresentationContextId();
		}
	}
	
	// maybe the Abstract Syntax Name is a member of a Meta SOP Class
	// there could be more than 1 metasop class the sop class belongs to
	//hence we have to loop through them
	
	BYTE pcId = 0;
	int idx = 0;
	while (pcId == 0 && idx < DEFINITION->GetNrMetaSopClasses() )
	{
        DEF_METASOPCLASS_CLASS *metasop_ptr = DEFINITION->GetMetaSopClass(idx);

		if (metasop_ptr->HasSopClass((char*) uid.get()))
		{
            string metaSopUid = metasop_ptr->GetUid();

			// use Abstract Syntax Name to search through list
			for (UINT i = 0; i < presentationContextM.getSize(); i++) 
			{
				// if abstract syntax names match and it is accepted
				string acceptedSopUid = (char*) presentationContextM[i].getAbstractSyntaxName().get();
				if ((metaSopUid == acceptedSopUid) &&
					(presentationContextM[i].getResultReason() == ACCEPTANCE)) 
				{
					// match found - return Presentation Context Id
					pcId = presentationContextM[i].getPresentationContextId();
				}
			}
		}
		++idx;
	}

	return pcId;
}

//>>===========================================================================

void BASE_PRESENTATION_CONTEXT_CLASS::setPresentationContextId(int index, BYTE pcId)

//  DESCRIPTION     : Method to set the indexed presentation context id to the
//					  given value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for validf index
	if ((index != -1) &&
		(index < (int) presentationContextM.getSize())) 
	{
		// set the pc Id
		presentationContextM[index].setPresentationContextId(pcId);
	}
}

//>>===========================================================================

bool BASE_PRESENTATION_CONTEXT_CLASS::getAbstractSyntaxName(BYTE pcId, UID_CLASS &uid)

//  DESCRIPTION     : Method to search the Presentation Contexts
//					  for a matching pcId. The Abstract Syntax Name of the
//					  match is returned.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// use Abstract Syntax Name to search through list
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{
		// if presentation context Ids match
		if (presentationContextM[i].getPresentationContextId() == pcId) 
		{
			// match found - return Presentation Context Id
			uid = presentationContextM[i].getAbstractSyntaxName();
			result = true;
			break;
		}
	}
	
	// return result
	return result;
}

//>>===========================================================================

void BASE_PRESENTATION_CONTEXT_CLASS::clear()
//  DESCRIPTION     : Clears all presentation contexts
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// release any Presentation Contexts
	while (presentationContextM.getSize()) 
	{
		presentationContextM.removeAt(0);
	}
	
	// reset the global PC id counter
	resetUniq8odd();
}


//>>===========================================================================

ACCEPTED_PC_CLASS::ACCEPTED_PC_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}


//>>===========================================================================

ACCEPTED_PC_CLASS::~ACCEPTED_PC_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}


//>>===========================================================================

void ACCEPTED_PC_CLASS::initialiseAcceptedPCs(ASSOCIATE_RQ_CLASS *assocRq_ptr)

//  DESCRIPTION     : Method to initialise the Accepted Presentation
//					  Context list with all Presentation Contexts proposed
//					  in the given Associate Request.
//					  Initially they will be all be rejected.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (assocRq_ptr == NULL) return;

	// remove any previously accepted Presentation Contexts
	while (presentationContextM.getSize()) 
	{
		presentationContextM.removeAt(0);
	}

	// loop through all proposed Presentation Contexts
	for (UINT i = 0; i < assocRq_ptr->noPresentationContexts(); i++) 
	{
		// initialise Abstract Syntax as not supported
		PRESENTATION_CONTEXT_AC_CLASS pcAccept;

		pcAccept.setAbstractSyntaxName(assocRq_ptr->getPresentationContext(i).getAbstractSyntaxName());
		pcAccept.setResultReason(ABSTRACT_SYNTAX_NOT_SUPPORTED);
		pcAccept.setPresentationContextId(assocRq_ptr->getPresentationContext(i).getPresentationContextId());

		// copy transfer syntax directly when only one offered - helps when
		// trying to handle updating the Associate Accept PDU sent
		if (assocRq_ptr->getPresentationContext(i).noTransferSyntaxNames() == 1) 
		{
			pcAccept.setTransferSyntaxName(assocRq_ptr->getPresentationContext(i).getTransferSyntaxName(0));
		}

		// save the Proposed Presentation Context
		presentationContextM.add(pcAccept);
	}
}


//>>===========================================================================

void ACCEPTED_PC_CLASS::updateAcceptedPCsOnReceive(ASSOCIATE_AC_CLASS *assocAc_ptr)

//  DESCRIPTION     : Method to update the received Associate Accept from
//					  the Accepted Presentation Context list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (assocAc_ptr == NULL) return;

	// loop through proposed Presentation Contexts
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{	
		// loop through accepted Presentation Contexts
		for (UINT j = 0; j < assocAc_ptr->noPresentationContexts(); j++) 
		{
			// when Presentation Context IDs match
			if (presentationContextM[i].getPresentationContextId() == 
				assocAc_ptr->getPresentationContext(j).getPresentationContextId()) 
			{
				// copy the Abstract Syntax Name
				assocAc_ptr->getPresentationContext(j).setAbstractSyntaxName(presentationContextM[i].getAbstractSyntaxName());

				// copy the Result
				presentationContextM[i].setResultReason(assocAc_ptr->getPresentationContext(j).getResultReason());

				// are we accepting the Abstract Syntax ?
				if (presentationContextM[i].getResultReason() == ACCEPTANCE) 
				{
					// copy the Transfer Syntax
					presentationContextM[i].setTransferSyntaxName(assocAc_ptr->getPresentationContext(j).getTransferSyntaxName());
				}
				break;
			}
		}
	}
}


//>>===========================================================================

void ACCEPTED_PC_CLASS::updateAcceptedPCsOnSend(ASSOCIATE_AC_CLASS *assocAc_ptr)

//  DESCRIPTION     : Method to update the Associate Accept with any
//					  additional (proposed) Presentation Contexts from the
//					  received Associate Request. This is done to ensure that
//					  the SCU receives a complete and correct Associate
//					  Accept. Any additional Presentation Contexts are
//					  refused.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (assocAc_ptr == NULL) return;

	// loop through proposed Presentation Contexts
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{
		// set marker
		presentationContextM[i].setReserved1(0xFF);

		// loop through accepted Presentation Contexts
		for (UINT j = 0; j < assocAc_ptr->noPresentationContexts(); j++) 
		{
			// find matching (unused) Abstract Syntax
			if ((assocAc_ptr->getPresentationContext(j).getReserved1() == 0)
			 && (presentationContextM[i].getAbstractSyntaxName()
			  == assocAc_ptr->getPresentationContext(j).getAbstractSyntaxName())) 
			{
				// check if Abstract Syntax accepted - but Transfer Syntax not defined by script (VTS)
				if ((assocAc_ptr->getPresentationContext(j).getResultReason() == ACCEPTANCE) &&
					(assocAc_ptr->getPresentationContext(j).getTransferSyntaxName().getLength() == 0))
				{
					// copy the proposed Transfer Syntax from the accepted list
					assocAc_ptr->getPresentationContext(j).setTransferSyntaxName(presentationContextM[i].getTransferSyntaxName());
				}

				// check if the presentation context has been rejected due to the transfer
				// syntax not being supported
				if (assocAc_ptr->getPresentationContext(j).getResultReason() != TRANSFER_SYNTAXES_NOT_SUPPORTED)
				{
					// perform equality test
					if ((assocAc_ptr->getPresentationContext(j).getResultReason() == ACCEPTANCE)
					 && (presentationContextM[i].getTransferSyntaxName().getLength())
					 && (presentationContextM[i].getTransferSyntaxName() !=
							assocAc_ptr->getPresentationContext(j).getTransferSyntaxName())) 
					{
						// do not use this presentation context - the transfer syntax
						// names do not match
						break;
					}
				}

				// copy Presentation Context Id - if not set in script
				if (assocAc_ptr->getPresentationContext(j).getPresentationContextId() == 0)
				{
					assocAc_ptr->getPresentationContext(j).setPresentationContextId(presentationContextM[i].getPresentationContextId());
				}

				// copy the Result
				presentationContextM[i].setResultReason(assocAc_ptr->getPresentationContext(j).getResultReason());

				// are we accepting the Abstract Syntax ?
				if (assocAc_ptr->getPresentationContext(j).getResultReason() == ACCEPTANCE) 
				{
					// copy the Transfer Syntax
					presentationContextM[i].setTransferSyntaxName(assocAc_ptr->getPresentationContext(j).getTransferSyntaxName());
				}

				// set marker
				assocAc_ptr->getPresentationContext(j).setReserved1(0xFF);

				// reset marker
				presentationContextM[i].setReserved1(0);
				break;
			}
		}
	}

	BYTE maxPcId = 0;

	// have we dealt with all the accepted Presentation Contexts ?
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{
		// check marker
		if (presentationContextM[i].getReserved1() != 0) 
		{
			UID_CLASS	abstractSyntaxUid(presentationContextM[i].getAbstractSyntaxName());
			PRESENTATION_CONTEXT_AC_CLASS	pcAccept;

			// we are going to automatically reject this one
			if (loggerM_ptr) 
			{
				if (!isUID((char*) abstractSyntaxUid.get()))
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Automatically Rejecting Abstract Syntax Name (SOP Class): \"%s\" with PC ID: %d.",
					(char*) abstractSyntaxUid.get(),
					presentationContextM[i].getPresentationContextId());
					loggerM_ptr->text(LOG_NONE, 1, "The SOP Class UID string is not translated into a real UID - has the required Definition File been loaded?");
				}
				else
				{
					loggerM_ptr->text(LOG_INFO, 1, "Automatically Rejecting Abstract Syntax Name (SOP Class): \"%s\" with PC ID: %d",
					(char*) abstractSyntaxUid.get(),
					presentationContextM[i].getPresentationContextId());
				}
			}

			// reject Abstract Syntax
			pcAccept.setAbstractSyntaxName(abstractSyntaxUid);
			pcAccept.setResultReason(ABSTRACT_SYNTAX_NOT_SUPPORTED);
			pcAccept.setPresentationContextId(presentationContextM[i].getPresentationContextId());
			assocAc_ptr->addPresentationContext(pcAccept);

			// reset marker
			presentationContextM[i].setReserved1(0);
		}

		// get largest pcId in use
		if (presentationContextM[i].getPresentationContextId() > maxPcId)
		{
			maxPcId = presentationContextM[i].getPresentationContextId();
		}
	}

	// move to next free pcId (must be odd)
	if (maxPcId == 0)
	{
		maxPcId++;
	}
	else
	{
		maxPcId += 2;
	}

	// update any additional Presentation Contexts
	for (UINT j = 0; j < assocAc_ptr->noPresentationContexts(); j++) 
	{
		// reset marker
		assocAc_ptr->getPresentationContext(j).setReserved1(0);

		// set any undefined pcIds
		if (assocAc_ptr->getPresentationContext(j).getPresentationContextId() == 0) 
		{
			// set pcId
			assocAc_ptr->getPresentationContext(j).setPresentationContextId(maxPcId);
			
			// increment to next odd number
			maxPcId += 2;
		}
	}
}


//>>===========================================================================

void ACCEPTED_PC_CLASS::getPresentationContext(ASSOCIATE_AC_CLASS *assocAc_ptr)

//  DESCRIPTION     : Method to get the Presentation Context Items of
//					  the Associate Accept from the list maintained here.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (assocAc_ptr == NULL) return;

	// loop through all accepted presentation contexts
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{
		// copy the presentation context
		PRESENTATION_CONTEXT_AC_CLASS pcAccept;

		pcAccept.setAbstractSyntaxName(presentationContextM[i].getAbstractSyntaxName());
		pcAccept.setResultReason(presentationContextM[i].getResultReason());
		pcAccept.setPresentationContextId(presentationContextM[i].getPresentationContextId());

		// handle the transfer syntax
		if (presentationContextM[i].getResultReason() == ACCEPTANCE) 
		{
			pcAccept.setTransferSyntaxName(presentationContextM[i].getTransferSyntaxName());
		}
		else 
		{
			pcAccept.setTransferSyntaxName("");
		}

		// add presentation context to Associate Accept list
		assocAc_ptr->addPresentationContext(pcAccept);
	}
}


//>>===========================================================================

int ACCEPTED_PC_CLASS::getPresentationContextIdWithTransferSyntax(UID_CLASS abstractSyntaxName, UID_CLASS transferSyntax)

//  DESCRIPTION     : Get the presentation context id matching the given
//                  : abstractSyntaxName and transferSyntax. The PC must be accepted.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    int presentationContextId = -1;

	// loop through all accepted presentation contexts
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{
		// try for a match
		if ((presentationContextM[i].getResultReason() == ACCEPTANCE) &&
            (presentationContextM[i].getAbstractSyntaxName() == abstractSyntaxName) &&
            (presentationContextM[i].getTransferSyntaxName() == transferSyntax))
        {
            // get the presentation context id
            presentationContextId = (int) presentationContextM[i].getPresentationContextId();
            break;
        }
	}

	// return the presentation context id - can be -1
    return presentationContextId;
}

//>>===========================================================================

SUPPORTED_PC_CLASS::SUPPORTED_PC_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}


//>>===========================================================================

SUPPORTED_PC_CLASS::~SUPPORTED_PC_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}


//>>===========================================================================

void SUPPORTED_PC_CLASS::setPresentationContext(UID_CLASS abstractSyntaxName, UID_CLASS transferSyntaxUid)

//  DESCRIPTION     : Method to add the given presentation context (Abstract 
//					  Syntax Name (SOP Class Uid) and Transfer Syntax Uid) to
//					  the list supported.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// copy the presentation context
	PRESENTATION_CONTEXT_AC_CLASS pcAccept;

	pcAccept.setAbstractSyntaxName(abstractSyntaxName);
	pcAccept.setResultReason(ACCEPTANCE);
	pcAccept.setPresentationContextId(0);
	pcAccept.setTransferSyntaxName(transferSyntaxUid);

	// save the supported Presentation Context
	presentationContextM.add(pcAccept);
}


//>>===========================================================================

bool SUPPORTED_PC_CLASS::getPresentationContext(int index, UID_CLASS &abstractSyntaxName, UID_CLASS &transferSyntaxName)

//  DESCRIPTION     : Method to return the indexed presentation context.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	assert (index >= 0);

	// check for valid index
	if ((unsigned int) index < presentationContextM.getSize())
	{
		// get the names
		abstractSyntaxName = presentationContextM[index].getAbstractSyntaxName();
		transferSyntaxName = presentationContextM[index].getTransferSyntaxName();

		// return success
		return true;
	}

	// return failure
	return false;
}


//>>===========================================================================

int SUPPORTED_PC_CLASS::isPresentationContext(UID_CLASS abstractSyntaxName, UID_CLASS transferSyntaxUid)

//  DESCRIPTION     : Method to check the list of accepted presentation contexts
//					  for the Abstract Syntax Name (SOP Class Uid) and 
//					  Transfer Syntax Uid given. If accepted return it's index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// loop through proposed presentation contexts
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{
		// find matching abstract syntax
		if (presentationContextM[i].getAbstractSyntaxName() == abstractSyntaxName) 
		{
			// perform equality test
			if ((presentationContextM[i].getResultReason() == ACCEPTANCE)
			 && (presentationContextM[i].getTransferSyntaxName().getLength())
			 && (presentationContextM[i].getTransferSyntaxName() == transferSyntaxUid))
			{
				// presentation context is accepted - return it's index
				return i;
			}
		}
	}

	// not accepted - return invalid index
	return -1;
}


//>>===========================================================================

bool SUPPORTED_PC_CLASS::isAbstractSyntaxName(UID_CLASS abstractSyntaxName)

//  DESCRIPTION     : Method to check the list of accepted presentation contexts
//					  for the Abstract Syntax Name given - we are not interested in
//					  the Transfer Syntax at the moment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// loop through presentation contexts
	for (UINT i = 0; i < presentationContextM.getSize(); i++) 
	{
		// find matching abstract syntax
		if ((presentationContextM[i].getAbstractSyntaxName() == abstractSyntaxName) &&
			(presentationContextM[i].getResultReason() == ACCEPTANCE))
		{
			// abstract syntax name is supported
			return true;
		}
	}

	// not supported
	return false;
}


//>>===========================================================================

TS_CODE transferSyntaxUidToCode(UID_CLASS &uid)

//  DESCRIPTION     : Function to map the given Transfer Syntax Uid into the
//					  internal Transfer Syntax Code used for decode / encode.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through Table for a match
	for (int i = 0; i < MAX_TRN_STX_NAMES; i++) 
	{
		if (strcmp(TTrnStx[i].uid, (char *) uid.get()) == 0)
			// return match
			return TTrnStx[i].code;
	}

	// when no match is found assume that the transfer syntax
    // is an Explicit VR, Little Endian and Compressed in some form or other.
    return TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED;
}

//>>===========================================================================

TS_CODE transferSyntaxUidToCode(string uid)

//  DESCRIPTION     : Function to map the given Transfer Syntax Uid into the
//					  internal Transfer Syntax Code used for decode / encode.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through Table for a match
	for (int i = 0; i < MAX_TRN_STX_NAMES; i++) 
	{
		if (strcmp(TTrnStx[i].uid, (char *) uid.c_str()) == 0)
			// return match
			return TTrnStx[i].code;
	}

	// when no match is found assume that the transfer syntax
    // is an Explicit VR, Little Endian and Compressed in some form or other.
    return TS_EXPLICIT_VR | TS_LITTLE_ENDIAN | TS_COMPRESSED;
}

//>>===========================================================================

string transferSyntaxUidToName(string uid)

//  DESCRIPTION     : Function to map the given Transfer Syntax Uid into the
//					  Transfer Syntax name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    string name = "";

	// loop through Table for a match
	for (int i = 0; i < MAX_TRN_STX_NAMES; i++) 
	{
		if (strcmp(TTrnStx[i].uid, (char *) uid.c_str()) == 0)
        {
			// return match
            name = TTrnStx[i].text;
            break;
        }
	}

	// return name
	return name;
}



