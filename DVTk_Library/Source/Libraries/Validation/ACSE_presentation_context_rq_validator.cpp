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
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_presentation_context_rq_validator.h"
#include "valrules.h"
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"           // Logging component interface file
#include "Idefinition.h"    // Definition component interface file.
#include "Inetwork.h"       // Network component interface file


//>>===========================================================================		

ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// constructor activities
} 

//>>===========================================================================		

ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS(ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS& presentationContext)

//  DESCRIPTION     : Copy Constructor
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

ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::~ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// destructor activities
	while (transferSyntaxNameM.getSize())
	{
		transferSyntaxNameM.removeAt(0);
	}
} 

//>>===========================================================================		

string ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::getPresentationContextId()

//  DESCRIPTION     : Get Presentation Context value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return presentationContextIdM.getValue(); 
}
	
//>>===========================================================================		

string ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::getAbstractSyntaxName()

//  DESCRIPTION     : Get Abstract Syntax Name value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return abstractSyntaxNameM.getValue(); 
}
	
//>>===========================================================================		

UINT ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::noTransferSyntaxNames()

//  DESCRIPTION     : Get number of Transfer Syntax Names
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return transferSyntaxNameM.getSize(); 
}
	
//>>===========================================================================		

string ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::getTransferSyntaxName(UINT i)

//  DESCRIPTION     : Get Transfer Syntax Name value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return transferSyntaxNameM[i].getValue(); 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::getPresentationContextIdParameter() 

//  DESCRIPTION     : Get Presentation Context Id
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &presentationContextIdM; 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::getAbstractSyntaxNameParameter()

//  DESCRIPTION     : Get Abstract Syntax Name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &abstractSyntaxNameM; 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::getTransferSyntaxNameParameter(UINT i)

//  DESCRIPTION     : Get Transfer Syntax Name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &transferSyntaxNameM[i]; 
}

//>>===========================================================================

bool ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::operator = (ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS& presentationContext)

//  DESCRIPTION     : Operator assignment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;
	
	// copy individual fields
	presentationContextIdM = presentationContext.presentationContextIdM;
	abstractSyntaxNameM = presentationContext.abstractSyntaxNameM;
	for (UINT i = 0; i < presentationContext.transferSyntaxNameM.getSize(); i++)
	{
		ACSE_UID_CLASS transferSyntaxName = presentationContext.transferSyntaxNameM[i];
		transferSyntaxNameM.add(transferSyntaxName);
	}
	
	return result;
}

//>>===========================================================================		

void ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::tooManyPresentationContexts()

//  DESCRIPTION     : Indicate that there are too many presentation contexts defined.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// add a warning message
	presentationContextIdM.addMessage(VAL_RULE_R_PARAM_3, "There are more that 127 Presentation Contexts defined in this Associate Request.");
}

//>>===========================================================================		

void ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::notReferenced()

//  DESCRIPTION     : Indicate that presentation context has not been validated against a reference.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// add a warning message
	presentationContextIdM.addMessage(VAL_RULE_R_PARAM_1, "This Presentation Context has not been validated against a reference Presentation Context.");
}

//>>===========================================================================		

void ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::notUniquePcId()

//  DESCRIPTION     : Indicate that presentation context id is not unique.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// add a warning message
	presentationContextIdM.addMessage(VAL_RULE_D_PARAM_6, "This Presentation Context ID is not unique. It has already been used.");
}

//>>===========================================================================		

void ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::notUniqueSOP(string pcId1, string pcId2)

//  DESCRIPTION     : Indicate that SOP class is not unique.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// add a warning message
	presentationContextIdM.addMessage(VAL_RULE_D_PARAM_7, "Presentation Contexts %s and %s are identical.", pcId1.c_str(), pcId2.c_str());
}

//>>===========================================================================		

void ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::notSourced(PRESENTATION_CONTEXT_RQ_CLASS *refPC_ptr)

//  DESCRIPTION     : Indicate that presentation context has not been validated against a reference.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	char buffer[MAXIMUM_LINE_LENGTH];
	
	// add reference values
	if (refPC_ptr->getPresentationContextId())
	{
		sprintf(buffer, "%d", refPC_ptr->getPresentationContextId());
		presentationContextIdM.setValue(buffer);
	}
	abstractSyntaxNameM.setValue((char*) refPC_ptr->getAbstractSyntaxName().get());
    abstractSyntaxNameM.setMeaning(DEFINITION->GetSopName((char*) refPC_ptr->getAbstractSyntaxName().get()));
	
	for (UINT i = 0; i < refPC_ptr->noTransferSyntaxNames(); i++)
	{
		ACSE_UID_CLASS tsValidator;
		tsValidator.setValue((char*) refPC_ptr->getTransferSyntaxName(i).get());
        tsValidator.setMeaning(transferSyntaxUidToName((char*) refPC_ptr->getTransferSyntaxName(i).get()));

		transferSyntaxNameM.add(tsValidator);
	}
	
	// add a warning message
	presentationContextIdM.addMessage(VAL_RULE_R_PARAM_2, "This reference Presentation Context is not present in the source Presentation Context.");
}

//>>===========================================================================		

bool ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::validate(PRESENTATION_CONTEXT_RQ_CLASS *srcPC_ptr, ASSOCIATE_RQ_CLASS *refAssocRq_ptr)

//  DESCRIPTION     : Validate Requested Presentation Context.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	char buffer[MAXIMUM_LINE_LENGTH];
	string refPcId;
	string refAbstractSyntaxName;
	bool found = false;
	
	// check for valid presentation context
	if (srcPC_ptr == NULL) return false;
	
	PRESENTATION_CONTEXT_RQ_CLASS *refPC_ptr = NULL;
	
	// check for matching reference values
	if (refAssocRq_ptr)
	{
		for (UINT i = 0; i < refAssocRq_ptr->noPresentationContexts(); i++)
		{
			refPC_ptr = &refAssocRq_ptr->getPresentationContext(i);
			
			// try matching on the presentation context id
			if (refPC_ptr->getPresentationContextId())
			{
				// check if presentation context ids match
				if (srcPC_ptr->getPresentationContextId() == refPC_ptr->getPresentationContextId())
				{
					sprintf(buffer, "%d", refPC_ptr->getPresentationContextId());
					refPcId = buffer;
					refAbstractSyntaxName = (char*) refPC_ptr->getAbstractSyntaxName().get();
					
					if ((srcPC_ptr->noTransferSyntaxNames()) == refPC_ptr->noTransferSyntaxNames())
					{
						// indicate that the presentation contexts have been validated
						refPC_ptr->setReserved1(0);
						srcPC_ptr->setReserved1(0);
					}
					found = true;
				}
			}
			else
			{
				// try matching on other parameters
				if ((srcPC_ptr->getAbstractSyntaxName() == refPC_ptr->getAbstractSyntaxName()) &&
					(srcPC_ptr->noTransferSyntaxNames() == refPC_ptr->noTransferSyntaxNames()))
				{
					// check that all transfer syntaxes match 
					// - order of transfer syntaxes may not be the same
					bool matched = true;
					for (UINT j = 0; j < srcPC_ptr->noTransferSyntaxNames(); j++)
					{
						if (!refPC_ptr->isTransferSyntaxName(srcPC_ptr->getTransferSyntaxName(j)))
						{
							matched = false;
							break;
						}
					}
					if (matched)
					{
						// don't get reference presentation context id - it is zero
						refAbstractSyntaxName = (char*) refPC_ptr->getAbstractSyntaxName().get();
						
						// indicate that a validation match has been found
						refPC_ptr->setReserved1(0);
						srcPC_ptr->setReserved1(0);
						found = true;
					}
				}
			}
			
			// stop as soon as a match is found
			if (found) break;
		}
	}
	
	// validate parameters
	sprintf(buffer, "%d", srcPC_ptr->getPresentationContextId());
	bool result1 = presentationContextIdM.validate(buffer, refPcId);
	bool result2 = abstractSyntaxNameM.validate((char*) srcPC_ptr->getAbstractSyntaxName().get(), refAbstractSyntaxName);
	abstractSyntaxNameM.setMeaning(DEFINITION->GetSopName((char*) srcPC_ptr->getAbstractSyntaxName().get()));

	bool result3 = true;
	for (UINT i = 0; i < srcPC_ptr->noTransferSyntaxNames(); i++)
	{
		ACSE_UID_CLASS tsValidator;
		string refTs = "";
		
		if ((refAbstractSyntaxName.length()) &&
			(i < refPC_ptr->noTransferSyntaxNames()))
		{
			if (refPC_ptr->isTransferSyntaxName(srcPC_ptr->getTransferSyntaxName(i)))
			{
				// we already know the values match - but copy if for validation anyway
				refTs = (char*) srcPC_ptr->getTransferSyntaxName(i).get();
			}
		}
		
		if (!tsValidator.validate((char*) srcPC_ptr->getTransferSyntaxName(i).get(), refTs))
		{
			result3 = false;
		}
        tsValidator.setMeaning(transferSyntaxUidToName((char*) srcPC_ptr->getTransferSyntaxName(i).get()));

		transferSyntaxNameM.add(tsValidator);
	}
	
	// set result
	bool result = true;
	if ((!result1) ||
		(!result2) ||
		(!result3))
	{
		result = false;
	}
	
	// return result
	return result;
}

//>>===========================================================================		

bool ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS::equalTransferSyntaxes(ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS &refPc)

//  DESCRIPTION     : Check if the presentation context transfer syntaxes are the same.
//					: The order may be different.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// check if number of transfer syntaxes match
	if (noTransferSyntaxNames() != refPc.noTransferSyntaxNames()) return false;
	
	// run through all the transfer syntaxes in this and check if they are present in the
	// ref PC
	bool result = false;
	for (UINT i = 0; i < noTransferSyntaxNames(); i++)
	{
		result = false;
		
		for (UINT j = 0; j < refPc.noTransferSyntaxNames(); j++)
		{ 
			if (transferSyntaxNameM[i].getValue() == refPc.transferSyntaxNameM[j].getValue())
			{
				result = true;
			}
		}
		
		// check if match found
		if (!result) break;
	}
	
	// return result
	return result;
}
