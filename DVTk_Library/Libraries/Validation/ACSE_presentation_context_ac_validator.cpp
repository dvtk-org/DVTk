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
#include "ACSE_presentation_context_ac_validator.h"
#include "Iglobal.h"      // Global component interface file
#include "Ilog.h"         // Logging component interface file
#include "Idefinition.h"  // Definition component interface file.
#include "Inetwork.h"     // Network component interface file
#include "valrules.h"

//*****************************************************************************
//  STATIC DECLARATIONS
//*****************************************************************************
static T_BYTE_TEXT_MAP	TAACResult[] = {
{ACCEPTANCE,						"acceptance"},
{USER_REJECTION,					"user-rejection"},
{NO_REASON,							"no-reason (provider rejection)"},
{ABSTRACT_SYNTAX_NOT_SUPPORTED,		"abstract-syntax-not-supported (provider rejection)"},
{TRANSFER_SYNTAXES_NOT_SUPPORTED,	"transfer-syntaxes-not-supported (provider rejection)"},
{BYTE_SENTINAL,						"unknown"}
};


//>>===========================================================================

char *AACResult(BYTE result)

//  DESCRIPTION     : Presentation Context - acceptance result LUT.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int	index = 0;

	while ((TAACResult[index].code != result)
	  && (TAACResult[index].code != BYTE_SENTINAL))
		index++;

	return TAACResult[index].text;
}

//>>===========================================================================		

ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS()

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

ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::~ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS()

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

string ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::getPresentationContextId()

//  DESCRIPTION     : Get Presentation Context Id value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return presentationContextIdM.getValue(); 
}
	
//>>===========================================================================		

string ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::getAbstractSyntaxName()

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

string ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::getResult()

//  DESCRIPTION     : Get Result value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return resultM.getValue(); 
}
	
//>>===========================================================================		

string ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::getTransferSyntaxName()

//  DESCRIPTION     : Get Transfer Syntax Name value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return transferSyntaxNameM.getValue(); 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::getPresentationContextIdParameter()

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

ACSE_PARAMETER_CLASS *ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::getAbstractSyntaxNameParameter()

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

ACSE_PARAMETER_CLASS *ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::getResultParameter()

//  DESCRIPTION     : Get Result
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &resultM; 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::getTransferSyntaxNameParameter()

//  DESCRIPTION     : Get Transfer Syntax Name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &transferSyntaxNameM; 
}

//>>===========================================================================		

bool ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::validate(PRESENTATION_CONTEXT_AC_CLASS *srcPC_ptr, ASSOCIATE_AC_CLASS *refAssocAc_ptr)

//  DESCRIPTION     : Validate Accepted Presentation Context.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	char buffer[MAXIMUM_LINE_LENGTH];
	string refPcId;
	string refAbstractSyntaxName;
	string refResult;
	string refTransferSyntaxName;
	
	// check for valid presentation context
	if (srcPC_ptr == NULL) return false;
	
	// check for matching reference values
	if (refAssocAc_ptr)
	{
		bool found = false;
		
		// loop through all reference presentation contexts to see if we can match on
		// the presentation context id
		for (UINT i = 0; i < refAssocAc_ptr->noPresentationContexts(); i++)
		{
			PRESENTATION_CONTEXT_AC_CLASS *refPC_ptr = &refAssocAc_ptr->getPresentationContext(i);
			
			// try matching on the presentation context id
			if (refPC_ptr->getPresentationContextId())
			{
				// check if presentation context ids match
				if (srcPC_ptr->getPresentationContextId() == refPC_ptr->getPresentationContextId())
				{
					sprintf(buffer, "%d", refPC_ptr->getPresentationContextId());
					refPcId = buffer;
					refAbstractSyntaxName = (char*) refPC_ptr->getAbstractSyntaxName().get();
					sprintf(buffer, "%d", refPC_ptr->getResultReason());
					refResult = buffer;
					refTransferSyntaxName = (char*) refPC_ptr->getTransferSyntaxName().get();
					
					// indicate that a validation match has been found
					refPC_ptr->setReserved1(0);
					srcPC_ptr->setReserved1(0);
					
					// indicate that we have found a match and don't need to look any further
					found = true;
					break;
				}
			}
		}
		
		// if we didn't match by presentation context id the next best thing that we can try
		// is to match by the abstract syntax name
		if (!found)
		{
			// loop through all reference presentation contexts again
			for (UINT i = 0; i < refAssocAc_ptr->noPresentationContexts(); i++)
			{
				PRESENTATION_CONTEXT_AC_CLASS *refPC_ptr = &refAssocAc_ptr->getPresentationContext(i);
				
				// try matching on the abstract syntax name - not part of actual PDU encoding
				if (srcPC_ptr->getAbstractSyntaxName() == refPC_ptr->getAbstractSyntaxName())
				{
					// don't get reference presentation context id - it is zero
					refAbstractSyntaxName = (char*) refPC_ptr->getAbstractSyntaxName().get();
					sprintf(buffer, "%d", refPC_ptr->getResultReason());
					refResult = buffer;
					refTransferSyntaxName = (char*) refPC_ptr->getTransferSyntaxName().get();
					
					// indicate that a validation match has been found
					refPC_ptr->setReserved1(0);
					srcPC_ptr->setReserved1(0);
					break;
				}
			}
		}
	}
	
	// validate parameters
	sprintf(buffer, "%d", srcPC_ptr->getPresentationContextId());
	bool result1 = presentationContextIdM.validate(buffer, refPcId);
	bool result2 = abstractSyntaxNameM.validate((char*) srcPC_ptr->getAbstractSyntaxName().get(), refAbstractSyntaxName);
    abstractSyntaxNameM.setMeaning(DEFINITION->GetSopName((char*) srcPC_ptr->getAbstractSyntaxName().get()));

	sprintf(buffer, "%d", srcPC_ptr->getResultReason());
	bool result3 = resultM.validate(buffer, refResult);
    resultM.setMeaning(AACResult(srcPC_ptr->getResultReason()));
	
	// only check transfer syntax if the results are the same and successful
	bool result4 = true;
	if ((result3) &&
		(srcPC_ptr->getResultReason() == ACCEPTANCE))
	{
		UID_CLASS transferSyntax = srcPC_ptr->getTransferSyntaxName();
		if (transferSyntax.getLength() == 0)
		{
			//Transfer syntax sub item is not present at all. It should be present with 
			//a zero-length. See DICOM Part 8 – section 9.3.3.2
			transferSyntaxNameM.addMessage(VAL_RULE_D_PARAM_1, "At least one Transfer syntax sub item should be present with zero length");
		}
		else
		{
			result4 = transferSyntaxNameM.validate((char*) transferSyntax.get(), refTransferSyntaxName);
			transferSyntaxNameM.setMeaning(transferSyntaxUidToName((char*) transferSyntax.get()));
		}
	}
	
	// set result
	bool result = true;
	if ((!result1) ||
		(!result2) ||
		(!result3) ||
		(!result4))
	{
		result = false;
	}
	
	// return result
	return result;
}

//>>===========================================================================		

void ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::tooManyPresentationContexts()

//  DESCRIPTION     : Indicate that there are too many presentation contexts defined.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// add a warning message
	presentationContextIdM.addMessage(VAL_RULE_R_PARAM_3, "There are more that 127 Presentation Contexts defined in this Associate Accept.");
}

//>>===========================================================================		

void ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::notReferenced()

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

void ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::notUniquePcId()

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
void ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::notUniqueSOP(string pcId1, string pcId2)
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

void ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS::notSourced(PRESENTATION_CONTEXT_AC_CLASS *refPC_ptr)

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

	sprintf(buffer, "%d", refPC_ptr->getResultReason());
	resultM.setValue(buffer);
	transferSyntaxNameM.setValue((char*) refPC_ptr->getTransferSyntaxName().get());
    transferSyntaxNameM.setMeaning(transferSyntaxUidToName((char*) refPC_ptr->getTransferSyntaxName().get()));
	
	// add an error message
	presentationContextIdM.addMessage(VAL_RULE_R_PARAM_2, "This reference Presentation Context is not present in the source Presentation Context.");
}
