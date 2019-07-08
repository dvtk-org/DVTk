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

#pragma warning( disable : 4244 )

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "assoc_rq.h"
#include "pdu.h"			// PDU


//>>===========================================================================

PRESENTATION_CONTEXT_RQ_CLASS::PRESENTATION_CONTEXT_RQ_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_PRESENTATION_CONTEXT_RQ;
	reservedM = 0;
	presentationContextIdM = 0;
	reserved1M = 0;
	reserved2M = 0;
	reserved3M = 0;
	setSubLength();
}

//>>===========================================================================

PRESENTATION_CONTEXT_RQ_CLASS::PRESENTATION_CONTEXT_RQ_CLASS(PRESENTATION_CONTEXT_RQ_CLASS& presentationContext)

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

PRESENTATION_CONTEXT_RQ_CLASS::PRESENTATION_CONTEXT_RQ_CLASS(ABSTRACT_SYNTAX_NAME_CLASS& abstractSyntaxName, TRANSFER_SYNTAX_NAME_CLASS& transferSyntaxName)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_PRESENTATION_CONTEXT_RQ;
	reservedM = 0;
	presentationContextIdM = 0;
	reserved1M = 0;
	reserved2M = 0;
	reserved3M = 0;
	abstractSyntaxNameM = abstractSyntaxName;
	transferSyntaxNameM.add(transferSyntaxName);
	setSubLength();
}

//>>===========================================================================

PRESENTATION_CONTEXT_RQ_CLASS::~PRESENTATION_CONTEXT_RQ_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up resources
	cleanup();
}

//>>===========================================================================

void PRESENTATION_CONTEXT_RQ_CLASS::cleanup()

//  DESCRIPTION     : Clean up resources.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up resources
	while(transferSyntaxNameM.getSize())
	{
		transferSyntaxNameM.removeAt(0);
	}
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_RQ_CLASS::isTransferSyntaxName(UID_CLASS transferSyntaxName)

//  DESCRIPTION     : Check if the given transfer syntax name is present.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool found = false;

	// run through all the transfer syntax names
	for (UINT i = 0; i < transferSyntaxNameM.getSize(); i++)
	{
		// check for match
		if (transferSyntaxName == transferSyntaxNameM[i].getUid())
		{
			found = true;
			break;
		}
	}

	// return result;
	return found;
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_RQ_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to pdu.
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
	pdu << reserved2M;
	pdu << reserved3M;

	// encode the abstract syntax name
	abstractSyntaxNameM.encode(pdu);

	// encode the transfer syntax names
	for (UINT i = 0; i < transferSyntaxNameM.getSize(); i++)
	{
		transferSyntaxNameM[i].encode(pdu);
	}

	return true;
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_RQ_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from pdu.
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

bool PRESENTATION_CONTEXT_RQ_CLASS::decodeBody(PDU_CLASS& pdu)

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
	pdu >> reserved2M;
	pdu >> reserved3M;

	// decode the abstract syntax name
	abstractSyntaxNameM.decode(pdu);

	INT32 remainingLength = lengthM - (sizeof(presentationContextIdM) + sizeof(reserved1M) + sizeof(reserved2M) + sizeof(reserved3M) + abstractSyntaxNameM.getLength());
	
	// decode the transfer syntax names
	while (remainingLength > 0)
	{
		TRANSFER_SYNTAX_NAME_CLASS	transferSyntax;
		
		transferSyntax.decode(pdu);
		remainingLength -= transferSyntax.getLength();

		// add transfer syntax to list
		transferSyntaxNameM.add(transferSyntax);
	}

	return (remainingLength == 0) ? true : false;
}

//>>===========================================================================

UINT32 PRESENTATION_CONTEXT_RQ_CLASS::getLength()

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

void PRESENTATION_CONTEXT_RQ_CLASS::setSubLength()

//  DESCRIPTION     : Set item sub-length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the length of all sub items in this pdu item
	lengthM = sizeof(presentationContextIdM) + sizeof(reserved1M) +  sizeof(reserved2M) + sizeof(reserved3M);

	// - get abstract syntax name length
	lengthM += (UINT16) abstractSyntaxNameM.getLength();

	// append all transfer syntax name lengths
	for (UINT i = 0; i < transferSyntaxNameM.getSize(); i++)
	{
		lengthM += (UINT16) transferSyntaxNameM[i].getLength();
	}
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_RQ_CLASS::operator = (PRESENTATION_CONTEXT_RQ_CLASS& presentationContext)

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
	reserved2M = presentationContext.reserved2M;
	reserved3M = presentationContext.reserved3M;
	abstractSyntaxNameM = presentationContext.abstractSyntaxNameM;

	// copy transfer syntax names
	cleanup();
	for (UINT i = 0; i < presentationContext.noTransferSyntaxNames(); i++)
	{
		TRANSFER_SYNTAX_NAME_CLASS transferSyntaxName(presentationContext.getTransferSyntaxName(i));
		addTransferSyntaxName(transferSyntaxName);
	}

	// update copied length
	setSubLength();

	return result;
}

//>>===========================================================================

bool PRESENTATION_CONTEXT_RQ_CLASS::operator == (PRESENTATION_CONTEXT_RQ_CLASS &presentationContext)

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
		(noTransferSyntaxNames() == presentationContext.noTransferSyntaxNames()))
	{
		// check if the transfer syntax names are the same
		// loop through - stop if different
		for (UINT i = 0; i < presentationContext.noTransferSyntaxNames(); i++)
		{
			// get the transfer syntax uid
			UID_CLASS transferSyntaxUid = presentationContext.getTransferSyntaxName(i);
			equal = false;

			// loop through this - order may be different
			for (UINT j = 0; j < noTransferSyntaxNames(); j++)
			{
				if (transferSyntaxUid == getTransferSyntaxName(j))
				{
					// uids are the same
					equal = true;
				}
			}

			// check if not equal
			if (!equal) return false;
		}

		// presentation contexts are equal
		equal = true;
	}

	// return result
	return equal;
}


//>>===========================================================================

ASSOCIATE_RQ_CLASS::ASSOCIATE_RQ_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ASSOCIATE_RQ;
	itemTypeM = PDU_ASSOCIATE_RQ;
	reservedM = 0;
	protocolVersionM = UNDEFINED_PROTOCOL_VERSION;
	reserved1M = 0;
	setCalledAeTitle(NULL);
	setCallingAeTitle(NULL);
	setApplicationContextName("");
	byteZero(reserved2M, 32);
}

//>>===========================================================================

ASSOCIATE_RQ_CLASS::ASSOCIATE_RQ_CLASS(char* calledAeTitle_ptr, char* callingAeTitle_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ASSOCIATE_RQ;
	itemTypeM = PDU_ASSOCIATE_RQ;
	reservedM = 0;
	protocolVersionM = PROTOCOL_VERSION;
	reserved1M = 0;
	setCalledAeTitle(calledAeTitle_ptr);
	setCallingAeTitle(callingAeTitle_ptr);
	setApplicationContextName(APPLICATION_CONTEXT_NAME);
	byteZero(reserved2M, 32);
}

//>>===========================================================================

ASSOCIATE_RQ_CLASS::~ASSOCIATE_RQ_CLASS()

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

void ASSOCIATE_RQ_CLASS::setCalledAeTitle(char* calledAeTitle_ptr)

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

void ASSOCIATE_RQ_CLASS::setCallingAeTitle(char* callingAeTitle_ptr)

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

void ASSOCIATE_RQ_CLASS::setScpScuRoleSelect(UID_CLASS &sopClassUid, BYTE scpRole, BYTE scuRole)

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

void ASSOCIATE_RQ_CLASS::sortPresentationContexts()

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
				PRESENTATION_CONTEXT_RQ_CLASS tempPresentationContext = presentationContextM[j];
				presentationContextM[j] = presentationContextM[j-1];
				presentationContextM[j-1] = tempPresentationContext;
			}
		}
	}
}

//>>===========================================================================

bool ASSOCIATE_RQ_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode associate request as PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
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

bool ASSOCIATE_RQ_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode associate request from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the Associate Request PDU
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
				{
					applicationContextNameM.decodeBody(pdu);
					remainingLength -= applicationContextNameM.getLength();
				}
				break;

			case ITEM_PRESENTATION_CONTEXT_RQ: // Presentation Context List
				{
					PRESENTATION_CONTEXT_RQ_CLASS presentationContext;
				
					presentationContext.decodeBody(pdu);
					remainingLength -= presentationContext.getLength();

					presentationContextM.add(presentationContext);
				}
				break;

			case ITEM_USER_INFORMATION: // User Information
				{
					userInformationM.decodeBody(pdu);
					remainingLength -= userInformationM.getRawLength();
				}
				break;

			default: // unknown item type
				// do something
				break;
		}
	}

	return (remainingLength == 0) ? true : false;
}

//>>===========================================================================

UINT32 ASSOCIATE_RQ_CLASS::getLength()

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

void ASSOCIATE_RQ_CLASS::setSubLength()

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

bool ASSOCIATE_RQ_CLASS::checkPresentationContextIds()

//  DESCRIPTION     : check that the presentation context ids are all odd and
//					  unique.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool pcIdOk = true;

	// perform final check on presentation context ids - must be odd and unique
	for (UINT i = 0; i < presentationContextM.getSize(); i++)
	{	
		if (!(presentationContextM[i].getPresentationContextId() & 0x01))
		{
			// presentation context id is even
			pcIdOk = false;
		}
		else
		{
			// check that presentation context id is unique
			for (UINT j = i + 1; j < presentationContextM.getSize(); j++) 
			{
				if (presentationContextM[i].getPresentationContextId() == presentationContextM[j].getPresentationContextId())
				{
					pcIdOk = false;
				}
			}

			// leave on first failure
			if (!pcIdOk)
			{
				break;
			}
		}
	}

	return pcIdOk;
}

//>>===========================================================================

bool ASSOCIATE_RQ_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Update this object with the contents of the object given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// ensure update WID is an associate request
	if (wid_ptr->getWidType() == widTypeM)
	{
		ASSOCIATE_RQ_CLASS *updateAssociateRq_ptr = static_cast<ASSOCIATE_RQ_CLASS*>(wid_ptr);

		// update parameters
		// - protocol version
		if (updateAssociateRq_ptr->getProtocolVersion() != UNDEFINED_PROTOCOL_VERSION)
		{
			protocolVersionM = updateAssociateRq_ptr->getProtocolVersion();
		}

		// - called AE title
		if (updateAssociateRq_ptr->getCalledAeTitle())
		{
			setCalledAeTitle(updateAssociateRq_ptr->getCalledAeTitle());
		}

		// - calling AE title
		if (updateAssociateRq_ptr->getCallingAeTitle())
		{
			setCallingAeTitle(updateAssociateRq_ptr->getCallingAeTitle());
		}

		// - application context name
		UID_CLASS	applicationContextName = updateAssociateRq_ptr->getApplicationContextName();
		if (applicationContextName.getLength())
		{
			setApplicationContextName(applicationContextName);
		}

		// - presentation context
		result = updatePresentationContext(updateAssociateRq_ptr);

		// - user information
		if (result)
		{
			result = userInformationM.update(updateAssociateRq_ptr->getUserInformation());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

void ASSOCIATE_RQ_CLASS::updateDefaults()

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

bool ASSOCIATE_RQ_CLASS::updatePresentationContext(ASSOCIATE_RQ_CLASS *updateAssociateRq_ptr)

//  DESCRIPTION     : Update this presentation context with the contents of the update 
//					: presentation context.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through any updates
	for (UINT i = 0; i < updateAssociateRq_ptr->noPresentationContexts(); i++)
	{
		// get the presentation context
		PRESENTATION_CONTEXT_RQ_CLASS updatePc = updateAssociateRq_ptr->getPresentationContext(i);
		bool found = false;

		// loop through this
		for (UINT j = 0; j < noPresentationContexts(); j++)
		{
			// if presentation context id's are equal but not 0 -> overwrite
			PRESENTATION_CONTEXT_RQ_CLASS thisPc = getPresentationContext(j);
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
