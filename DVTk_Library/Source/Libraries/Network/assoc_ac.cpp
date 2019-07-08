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
//  DESCRIPTION     :	Associate Accept classes.
//*****************************************************************************
#pragma warning( disable : 4244 )

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "assoc_ac.h"
#include "pdu.h"			// PDU

//>>===========================================================================

PRESENTATION_CONTEXT_AC_CLASS::PRESENTATION_CONTEXT_AC_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_PRESENTATION_CONTEXT_AC;
	reservedM = 0;
	presentationContextIdM = 0;
	reserved1M = 0;
	resultReasonM = NO_REASON;
	reserved2M = 0;
	abstractSyntaxNameM.setUid("");
	transferSyntaxNameM.setUid("");
	setSubLength();
}

//>>===========================================================================

PRESENTATION_CONTEXT_AC_CLASS::PRESENTATION_CONTEXT_AC_CLASS(PRESENTATION_CONTEXT_AC_CLASS& presentationContext)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = presentationContext;
}

//>>===========================================================================

PRESENTATION_CONTEXT_AC_CLASS::PRESENTATION_CONTEXT_AC_CLASS(TRANSFER_SYNTAX_NAME_CLASS& transferSyntaxName)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_PRESENTATION_CONTEXT_AC;
	reservedM = 0;
	presentationContextIdM = 0;
	reserved1M = 0;
	resultReasonM = ACCEPTANCE;
	reserved2M = 0;
	abstractSyntaxNameM.setUid("");
	transferSyntaxNameM = transferSyntaxName;
	setSubLength();
}

//>>===========================================================================

PRESENTATION_CONTEXT_AC_CLASS::~PRESENTATION_CONTEXT_AC_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up resources
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_AC_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// update length before encode operation
	setSubLength();

	// encode the item length and presentation context ID
	pdu << itemTypeM;
	pdu << reservedM;		
	pdu << lengthM;		
	pdu << presentationContextIdM;
	pdu << reserved1M;	
	pdu << resultReasonM;
	pdu << reserved2M;

	// encode the transfer syntax name
	transferSyntaxNameM.encode(pdu);

	return true;
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_AC_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);	
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_AC_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item length and presentation context ID
	pdu >> reservedM;	
	pdu >> lengthM;		
	pdu >> presentationContextIdM;
	pdu >> reserved1M;
	pdu >> resultReasonM;
	pdu >> reserved2M;

	INT32 remainingLength = lengthM - (sizeof(presentationContextIdM) + sizeof(reserved1M) + sizeof(resultReasonM) + sizeof(reserved2M));

	// decode the transfer syntax name
	if (remainingLength != 0)
	{
		transferSyntaxNameM.decode(pdu);
		remainingLength -= transferSyntaxNameM.getLength();
	}
	else
	{
		// this is done as a fix for PR 427 - transfer syntax sub-item is mandatory and when no transfer syntax uid is defined should
		// still be present with a zero length
		// - further checks should be made an enocding errors reported to the user here (and in other parts where data is missing
		transferSyntaxNameM.setUid("");
	}

	return (remainingLength == 0) ? true : false;
}

//>>===========================================================================

UINT32 PRESENTATION_CONTEXT_AC_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// update length
	setSubLength();

	// compute the total length of the item
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}

//>>===========================================================================

void PRESENTATION_CONTEXT_AC_CLASS::setSubLength()

//  DESCRIPTION     : Set item sub-length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the length of all sub items in this pdu item
	lengthM = sizeof(presentationContextIdM) + sizeof(reserved1M) +  sizeof(resultReasonM) + sizeof(reserved2M);

	// append all transfer syntax name length
	lengthM += (UINT16) transferSyntaxNameM.getLength();
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_AC_CLASS::operator = (PRESENTATION_CONTEXT_AC_CLASS& presentationContext)

//  DESCRIPTION     : Operator assignment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// update length
	presentationContext.setSubLength();

	// copy individual fields
	itemTypeM = presentationContext.itemTypeM;
	reservedM = presentationContext.reservedM;		
	lengthM = presentationContext.lengthM;
	presentationContextIdM = presentationContext.presentationContextIdM;
	reserved1M = presentationContext.reserved1M;
	resultReasonM = presentationContext.resultReasonM;
	reserved2M = presentationContext.reserved2M;
	abstractSyntaxNameM = presentationContext.abstractSyntaxNameM;
	transferSyntaxNameM = presentationContext.transferSyntaxNameM;

	// update copied length
	setSubLength();

	return result;
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_AC_CLASS::operator == (PRESENTATION_CONTEXT_AC_CLASS& presentationContext)

//  DESCRIPTION     : Equality operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool equal = false;

	// check for equality
	if ((presentationContextIdM == presentationContext.getPresentationContextId()) &&
		(getAbstractSyntaxName() == presentationContext.getAbstractSyntaxName()) &&
		(resultReasonM == presentationContext.getResultReason()))
	{
		// check if the presentation context has been accepted
		if (resultReasonM == ACCEPTANCE)
		{
			// need to check that the transfer systaxes are the same
			if (getTransferSyntaxName() == presentationContext.getTransferSyntaxName())
			{
				// accepted presentation contexts are equal
				equal = true;
			}
		}
		else
		{
			// unaccepted presentation contexts are equal
			equal = true;
		}
	}

	// return result
	return equal;
}

//>>===========================================================================

ASSOCIATE_AC_CLASS::ASSOCIATE_AC_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ASSOCIATE_AC;
	itemTypeM = PDU_ASSOCIATE_AC;
	reservedM = 0;
	protocolVersionM = UNDEFINED_PROTOCOL_VERSION;
	reserved1M = 0;
	setCalledAeTitle(NULL);
	setCallingAeTitle(NULL);
	setApplicationContextName("");
	byteZero(reserved2M, 32);
}

//>>===========================================================================

ASSOCIATE_AC_CLASS::ASSOCIATE_AC_CLASS(char* calledAeTitle_ptr, char* callingAeTitle_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ASSOCIATE_AC;
	itemTypeM = PDU_ASSOCIATE_AC;
	reservedM = 0;
	protocolVersionM = PROTOCOL_VERSION;
	reserved1M = 0;
	setCalledAeTitle(calledAeTitle_ptr);
	setCallingAeTitle(callingAeTitle_ptr);
	setApplicationContextName(APPLICATION_CONTEXT_NAME);
	byteZero(reserved2M, 32);
}

//>>===========================================================================

ASSOCIATE_AC_CLASS::~ASSOCIATE_AC_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up resources
	while (presentationContextM.getSize())
	{
		presentationContextM.removeAt(0);
	}
}		

//>>===========================================================================

void ASSOCIATE_AC_CLASS::setCalledAeTitle(char* calledAeTitle_ptr)

//  DESCRIPTION     : Set Called Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	calledAeTitleM.set(calledAeTitle_ptr);
}

//>>===========================================================================

void ASSOCIATE_AC_CLASS::setCallingAeTitle(char* callingAeTitle_ptr)

//  DESCRIPTION     : Set Calling Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	callingAeTitleM.set(callingAeTitle_ptr);
}

//>>===========================================================================
	
bool ASSOCIATE_AC_CLASS::getScpScuRoleSelect(UID_CLASS sopClassUid, BYTE *scpRole_ptr, BYTE *scuRole_ptr)

//  DESCRIPTION     : Get the SCP and SCU Role for the given SOP Class UID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// loop through the entries for a matching SOP Class UID
	for (UINT i = 0; i < userInformationM.noScpScuRoleSelects(); i++)
	{
		SCP_SCU_ROLE_SELECT_CLASS scpScuRoleSelect = userInformationM.getScpScuRoleSelect(i);

		// match found - return role values
		if (scpScuRoleSelect.getUid() == sopClassUid)
		{
			*scpRole_ptr = scpScuRoleSelect.getScpRole();
			*scuRole_ptr = scpScuRoleSelect.getScuRole();
			result = true;
			break;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

void ASSOCIATE_AC_CLASS::setScpScuRoleSelect(UID_CLASS &sopClassUid, BYTE scpRole, BYTE scuRole)

//  DESCRIPTION     : Set SCP SCU Role Selection.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	SCP_SCU_ROLE_SELECT_CLASS scpScuRoleSelect;
	scpScuRoleSelect.setUid(sopClassUid);
	scpScuRoleSelect.setScpRole(scpRole);
	scpScuRoleSelect.setScuRole(scuRole);

	userInformationM.addScpScuRoleSelect(scpScuRoleSelect);
}

//>>===========================================================================

void ASSOCIATE_AC_CLASS::sortPresentationContexts()

//  DESCRIPTION     : Sort the presentation contexts is ascending presentation
//					: context id order.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// anything to do ?
	if (presentationContextM.getSize() == 0) return;

	// sort the presentation context ids into ascending order
	for (UINT i = 0; i < presentationContextM.getSize() - 1; i++) 
	{
		for (UINT j = presentationContextM.getSize() - 1; i < j; j--) 
		{
			if (presentationContextM[j].getPresentationContextId() < presentationContextM[j-1].getPresentationContextId()) 
			{
				PRESENTATION_CONTEXT_AC_CLASS tempPresentationContext = presentationContextM[j];
				presentationContextM[j] = presentationContextM[j-1];
				presentationContextM[j-1] = tempPresentationContext;
			}
		}
	}
}

//>>===========================================================================

bool ASSOCIATE_AC_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode associate accept as PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for undefined fields
	if (protocolVersionM == UNDEFINED_PROTOCOL_VERSION)
	{
		protocolVersionM = PROTOCOL_VERSION;
	}

	// update length
	setSubLength();

	// encode the pdu type and length
	pdu.setType(itemTypeM);
	pdu.setReserved(reservedM);
	if (!pdu.allocateBody(lengthM)) return false;

	// encode the protocol version
	pdu << protocolVersionM;	
	pdu << reserved1M;			

	// encode the called and calling Ae titles
	calledAeTitleM.encode(pdu);
	callingAeTitleM.encode(pdu);
	pdu.writeBinary(reserved2M, 32);

	// encode the application context name
	applicationContextNameM.encode(pdu);

	// encode the presentation context list
	for (UINT i = 0; i < presentationContextM.getSize(); i++)
	{
		presentationContextM[i].encode(pdu);
	}

	// encode the user information
	userInformationM.encode(pdu);

	return true;
}

//>>===========================================================================

bool ASSOCIATE_AC_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode associate accept from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the Associate Accept PDU
	itemTypeM = pdu.getType();
	reservedM = pdu.getReserved();			
	lengthM = pdu.getLength();
				
	pdu >> protocolVersionM;	
	pdu >> reserved1M;
	
	// decode the called and calling Ae titles
	calledAeTitleM.decode(pdu, AE_LENGTH);
	callingAeTitleM.decode(pdu, AE_LENGTH);
	pdu.readBinary(reserved2M, 32);
	
	// decode the remaining PDU
	INT32 remainingLength = lengthM - (sizeof(protocolVersionM) + sizeof(reserved1M) + calledAeTitleM.getLength() + callingAeTitleM.getLength() + sizeof(reserved2M));

	while (remainingLength > 0)
	{
		BYTE itemType;

		pdu >> itemType;	

		switch(itemType)
		{
			case ITEM_APPLICATION_CONTEXT_NAME: // Application Context Name
				applicationContextNameM.decodeBody(pdu);
				remainingLength -= applicationContextNameM.getLength();
				break;

			case ITEM_PRESENTATION_CONTEXT_AC: // Presentation Context List
				{
					PRESENTATION_CONTEXT_AC_CLASS presentationContext;
				
					presentationContext.decodeBody(pdu);
					remainingLength -= presentationContext.getLength();

					presentationContextM.add(presentationContext);
				}
				break;

			case ITEM_USER_INFORMATION: // User Information
				userInformationM.decodeBody(pdu);
				remainingLength -= userInformationM.getRawLength();
				break;

			default: // unknown item type
				// do something
				break;
		}
	}

	return (remainingLength == 0) ? true : false;
}

//>>===========================================================================

UINT32 ASSOCIATE_AC_CLASS::getLength()

//  DESCRIPTION     : Get PDU length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// update length
	setSubLength();

	// compute PDU length
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}

//>>===========================================================================

void ASSOCIATE_AC_CLASS::setSubLength()

//  DESCRIPTION     : set PDU sub-length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the PDU sub-length
	lengthM = sizeof(protocolVersionM) + sizeof(reserved1M) + calledAeTitleM.getLength() + callingAeTitleM.getLength() + sizeof(reserved2M);

	lengthM += applicationContextNameM.getLength();

	for (UINT i = 0; i < presentationContextM.getSize(); i++)
	{
		lengthM += presentationContextM[i].getLength();
	}

	lengthM += userInformationM.getLength();
}

//>>===========================================================================

bool ASSOCIATE_AC_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Update this object with the contents of the object given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// ensure update WID is an associate accept
	if (wid_ptr->getWidType() == widTypeM)
	{
		ASSOCIATE_AC_CLASS *updateAssociateAc_ptr = static_cast<ASSOCIATE_AC_CLASS*>(wid_ptr);

		// update parameters
		// - protocol version
		if (updateAssociateAc_ptr->getProtocolVersion() != UNDEFINED_PROTOCOL_VERSION)
		{
			protocolVersionM = updateAssociateAc_ptr->getProtocolVersion();
		}

		// - called AE title
		if (updateAssociateAc_ptr->getCalledAeTitle())
		{
			setCalledAeTitle(updateAssociateAc_ptr->getCalledAeTitle());
		}

		// - calling AE title
		if (updateAssociateAc_ptr->getCallingAeTitle())
		{
			setCallingAeTitle(updateAssociateAc_ptr->getCallingAeTitle());
		}

		// - application context name
		UID_CLASS	applicationContextName = updateAssociateAc_ptr->getApplicationContextName();
		if (applicationContextName.getLength())
		{
			setApplicationContextName(applicationContextName);
		}

		// - presentation context
		result = updatePresentationContext(updateAssociateAc_ptr);

		// - user information
		if (result)
		{
			result = userInformationM.update(updateAssociateAc_ptr->getUserInformation());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

void ASSOCIATE_AC_CLASS::updateDefaults()

//  DESCRIPTION     : Method to check that all parameters have been defined - if not use
//					: default values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// update defaults
	// - protocol version
	if (protocolVersionM == UNDEFINED_PROTOCOL_VERSION)
	{
		protocolVersionM = PROTOCOL_VERSION;
	}

	// - called ae title
	if (!getCalledAeTitle())
	{
		setCalledAeTitle(CALLED_AE_TITLE);
	}

	// - calling ae title
	if (!getCallingAeTitle())
	{
		setCallingAeTitle(CALLING_AE_TITLE);
	}

	// - application context name
	applicationContextNameM.updateDefaults();

	// - user information
	userInformationM.updateDefaults();
}

//>>===========================================================================

bool ASSOCIATE_AC_CLASS::updatePresentationContext(ASSOCIATE_AC_CLASS *updateAssociateAc_ptr)

//  DESCRIPTION     : Update this presentation context with the contents of the update 
//					: presentation context.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through any updates
	for (UINT i = 0; i < updateAssociateAc_ptr->noPresentationContexts(); i++)
	{
		// get the presentation context
		PRESENTATION_CONTEXT_AC_CLASS updatePc = updateAssociateAc_ptr->getPresentationContext(i);
		bool found = false;

		// loop through this
		for (UINT j = 0; j < noPresentationContexts(); j++)
		{
			// if presentation context id's are equal but not 0 -> overwrite
			PRESENTATION_CONTEXT_AC_CLASS thisPc = getPresentationContext(j);
			if (thisPc.getPresentationContextId() == 0)
			{
				if (thisPc == updatePc)
				{
				    // found equal presentation context
				    found = true;
				    break;
				}
				else 
				{
					// check if abstract syntax names are the same -> overwrite
					if (thisPc.getAbstractSyntaxName() == updatePc.getAbstractSyntaxName())
					{
					    // delete this pc
					    deletePresentationContextId(j);
					    break;
					}
				}

			}
			else if (thisPc.getPresentationContextId() == updatePc.getPresentationContextId())
			{
				found = true;
                // overwrite
                deletePresentationContextId(j);
				addPresentationContext(updatePc);
			}
		}

		// if not found add the update
		if (!found)
		{
			addPresentationContext(updatePc);
		}
	}

	// return result
	return true;
}

//>>===========================================================================

void ASSOCIATE_AC_CLASS::setZeroPresentationContextIds(ASSOCIATE_AC_CLASS *sourceAssociateAc_ptr)

//  DESCRIPTION     : Set the presentation context ids of "this" that have a value zero with
//					: the matching presentation context ids given in the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE processed = 0xFF;

	// loop through any updates
	for (UINT i = 0; i < noPresentationContexts(); i++)
	{
		// get the presentation context
		PRESENTATION_CONTEXT_AC_CLASS destPc = getPresentationContext(i);

		if (destPc.getPresentationContextId() == 0)
		{
			// try to find a matching presentation context in the source
			for (UINT j = 0; j < sourceAssociateAc_ptr->noPresentationContexts(); j++)
			{
				// check for matching parameters
				// match on abstract syntax name and result reason - but only if the source has not
				// already been processed - could be the same presentation context occurs more than once
				PRESENTATION_CONTEXT_AC_CLASS sourcePc = sourceAssociateAc_ptr->getPresentationContext(j);
				if ((sourcePc.getReserved1() != processed) &&
					(destPc.getAbstractSyntaxName() == sourcePc.getAbstractSyntaxName()) &&
					(destPc.getResultReason() == sourcePc.getResultReason()))
				{
					// if this pc is accepted - then we need to check that the transfer syntaxes are the same
					if (destPc.getResultReason() == ACCEPTANCE)
					{
						if (destPc.getTransferSyntaxName() == sourcePc.getTransferSyntaxName())
						{
							// accepted pc's are the same - set actual presentation context id
							setPresentationContextId(i, sourcePc.getPresentationContextId());

							// indicate that this source has been processed
							sourceAssociateAc_ptr->getPresentationContext(j).setReserved1(processed);
							break;
						}
					}
					else
					{
						// none accepted pc's are the same - set actual presentation context id
						setPresentationContextId(i, sourcePc.getPresentationContextId());

						// indicate that this source has been processed
						sourceAssociateAc_ptr->getPresentationContext(j).setReserved1(processed);
						break;
					}
				}
			}
		}
	}
}
