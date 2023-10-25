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
#include "ACSE_associate_accept_validator.h"
#include "Iglobal.h"      // Global component interface file
#include "Ilog.h"         // Logging component interface file
#include "Inetwork.h"     // Network component interface file
#include "ACSE_properties.h"

//>>===========================================================================		

ASSOCIATE_AC_VALIDATOR_CLASS::ASSOCIATE_AC_VALIDATOR_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// constructor activities
	// - initialise the used pc id array to false
	for (int i = 0; i < MAX_PC_ID; i++)
	{
		usedPcIdM[i] = false;
	}
} 

//>>===========================================================================		

ASSOCIATE_AC_VALIDATOR_CLASS::~ASSOCIATE_AC_VALIDATOR_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// destructor activities
	while(presentationContextM.getSize())
	{
		presentationContextM.removeAt(0);
	}
} 

//>>===========================================================================		

ACSE_APPL_CTX_NAME_CLASS& ASSOCIATE_AC_VALIDATOR_CLASS::getApplicationContextName()

//  DESCRIPTION     : Get Application Context Name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return applicationContextNameM;
}

//>>===========================================================================		

UINT ASSOCIATE_AC_VALIDATOR_CLASS::noPresentationContexts()

//  DESCRIPTION     : Get number of Presentation Contexts
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return presentationContextM.getSize(); 
}

//>>===========================================================================		

ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS& ASSOCIATE_AC_VALIDATOR_CLASS::getPresentationContext(UINT i)

//  DESCRIPTION     : Get Presentation Context
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return presentationContextM[i]; 
}

//>>===========================================================================		

ACSE_USER_INFORMATION_VALIDATOR_CLASS& ASSOCIATE_AC_VALIDATOR_CLASS::getUserInformation()

//  DESCRIPTION     : Get User Information
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return userInformationM;
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ASSOCIATE_AC_VALIDATOR_CLASS::getProtocolVersionParameter()

//  DESCRIPTION     : Get Protocol Version
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return &protocolVersionM;
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ASSOCIATE_AC_VALIDATOR_CLASS::getCalledAeTitleParameter()

//  DESCRIPTION     : Get Called AE Title
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return &calledAeTitleM;
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ASSOCIATE_AC_VALIDATOR_CLASS::getCallingAeTitleParameter()

//  DESCRIPTION     : Get Calling AE Title
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &callingAeTitleM;
}

//>>===========================================================================		

bool ASSOCIATE_AC_VALIDATOR_CLASS::validate(ASSOCIATE_AC_CLASS *srcAssocAc_ptr,
											ASSOCIATE_AC_CLASS *refAssocAc_ptr,
											ACSE_PROPERTIES_CLASS * acseProp_ptr)

//  DESCRIPTION     : Validate Associate Accept.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	char buffer[MAXIMUM_LINE_LENGTH];
	string refProtocolVersion;
	string refCalledAeTitle;
	string refCallingAeTitle;
	string refApplicationContextName;
	USER_INFORMATION_CLASS *refUserInformation_ptr = NULL;
	
	// check for valid source
	if (srcAssocAc_ptr == NULL) return false;
	
	// check if reference provided
	if (refAssocAc_ptr)
	{
		// set up reference values
		if (refAssocAc_ptr->getProtocolVersion() != UNDEFINED_PROTOCOL_VERSION)
		{
			sprintf(buffer, "%d", refAssocAc_ptr->getProtocolVersion());
			refProtocolVersion = buffer;
		}
		
		if (refAssocAc_ptr->getCalledAeTitle())
		{
			refCalledAeTitle = refAssocAc_ptr->getCalledAeTitle();
		}
		if (refAssocAc_ptr->getCallingAeTitle())
		{
			refCallingAeTitle = refAssocAc_ptr->getCallingAeTitle();
		}
		if (refAssocAc_ptr->getApplicationContextName().get())
		{
			refApplicationContextName = (char*) refAssocAc_ptr->getApplicationContextName().get();
		}
		refUserInformation_ptr = &refAssocAc_ptr->getUserInformation();
	}
	
	// check if the ae titles have been defined from the reference values
	if (refCalledAeTitle.length() == 0)
	{
		refCalledAeTitle = acseProp_ptr->getCalledAeTitle();
	}
	if (refCallingAeTitle.length() == 0)
	{
		refCallingAeTitle = acseProp_ptr->getCallingAeTitle();
	}
	
	// validate the parameters
	sprintf(buffer, "%d", srcAssocAc_ptr->getProtocolVersion());
	bool result1 = protocolVersionM.validate(buffer, refProtocolVersion);
	bool result2 = calledAeTitleM.validate(srcAssocAc_ptr->getCalledAeTitle(), refCalledAeTitle);
	bool result3 = callingAeTitleM.validate(srcAssocAc_ptr->getCallingAeTitle(), refCallingAeTitle);
	bool result4 = applicationContextNameM.validate((char*) srcAssocAc_ptr->getApplicationContextName().get(), refApplicationContextName);
	
	// pre-process presentation contexts by flaging all as unvalidated
	setUnvalidatedFlags(srcAssocAc_ptr, refAssocAc_ptr, true);
	
	bool result5 = true;
	for (UINT i = 0; i < srcAssocAc_ptr->noPresentationContexts(); i++)
	{
		PRESENTATION_CONTEXT_AC_CLASS *pcContext_ptr = &srcAssocAc_ptr->getPresentationContext(i);
		ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS pcValidator;
		
		// validate individual presentation contexts
		if (!pcValidator.validate(pcContext_ptr, refAssocAc_ptr))
		{
			result5 = false;
		}
		
		// save the validator
		presentationContextM.add(pcValidator);
	}
	
	// check if either source or reference presentation contexts are still unvalidated
	bool result6 = checkIfValidated(srcAssocAc_ptr, refAssocAc_ptr);
	
	// post-process presentation contexts by flaging all as validated
	setUnvalidatedFlags(srcAssocAc_ptr, refAssocAc_ptr, false);
	
	// check that all the presentation context ids are unique
	bool result7 = checkUniquePresentationContextId();
	
	bool result8 = checkUniqueSOPClasses();
	
	// validate the user information
	bool result9 = userInformationM.validate(&srcAssocAc_ptr->getUserInformation(),
		refUserInformation_ptr,
		acseProp_ptr);
	
	// set result
	bool result = true;
	if ((!result1) ||
		(!result2) ||
		(!result3) ||
		(!result4) ||
		(!result5) ||
		(!result6) ||
		(!result7) ||
		(!result8) ||
		(!result9))
	{
		result = false;
	}
	
	// resturn result
	return result;
}

//>>===========================================================================		

void ASSOCIATE_AC_VALIDATOR_CLASS::setUnvalidatedFlags(ASSOCIATE_AC_CLASS *srcAssocAc_ptr, ASSOCIATE_AC_CLASS *refAssocAc_ptr, bool flag)

//  DESCRIPTION     : Set the unvalidated flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	BYTE reserved1 = (flag) ? 0xFF : 0x00;
	
	// loop through both source and reference setting the reserved1 field 
	// - we use this as the "unvalidated" flag
	if (srcAssocAc_ptr)
	{
		for (UINT i = 0; i < srcAssocAc_ptr->noPresentationContexts(); i++)
		{
			srcAssocAc_ptr->getPresentationContext(i).setReserved1(reserved1);
		}
	}
	
	if (refAssocAc_ptr)
	{
		for (UINT i = 0; i < refAssocAc_ptr->noPresentationContexts(); i++)
		{
			refAssocAc_ptr->getPresentationContext(i).setReserved1(reserved1);
		}
	}
}

//>>===========================================================================		

bool ASSOCIATE_AC_VALIDATOR_CLASS::checkIfValidated(ASSOCIATE_AC_CLASS *srcAssocAc_ptr, ASSOCIATE_AC_CLASS *refAssocAc_ptr)

//  DESCRIPTION     : Check if all source and reference presentation contexts have been validated.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	bool result = true;
	
	// loop through both source and reference checking if validated
	if ((srcAssocAc_ptr) &&
		(refAssocAc_ptr) &&
		(refAssocAc_ptr->noPresentationContexts()))
	{
		// only need to check source if a reference is defined
		for (UINT i = 0; i < srcAssocAc_ptr->noPresentationContexts(); i++)
		{
			PRESENTATION_CONTEXT_AC_CLASS pcContext = srcAssocAc_ptr->getPresentationContext(i);
			
			// check if source validated against reference
			if (pcContext.getReserved1())
			{
				// not validated against reference 
				// - this presentation context is either unexpected or
				// the reference presentation context is badly defined
				updatePresentationContext(&pcContext);
				
				// indicate problem
				result = false;
			}
		}
	}
	
	// check if reference has presentation contexts not found in the source
	if (refAssocAc_ptr)
	{
		for (UINT i = 0; i < refAssocAc_ptr->noPresentationContexts(); i++)
		{
			PRESENTATION_CONTEXT_AC_CLASS pcContext = refAssocAc_ptr->getPresentationContext(i);
			
			// check if reference validated against source
			// - presentation context id must be defined
			if ((pcContext.getReserved1()) &&
				(pcContext.getPresentationContextId() != 0))
			{
				// not validated against source
				// - this presentation context is either badly defined
				// or the source is incorrect
				ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS pcValidator;
				
				// indicate that no source was found
				pcValidator.notSourced(&pcContext);
				
				// save the validator
				presentationContextM.add(pcValidator);
				
				// indicate problem
				result = false;
			}
		}
	}
	
	// return result
	return result;
}

//>>===========================================================================		

void ASSOCIATE_AC_VALIDATOR_CLASS::updatePresentationContext(PRESENTATION_CONTEXT_AC_CLASS *srcPC_ptr)

//  DESCRIPTION     : Update the matching presentation context.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	char buffer[MAXIMUM_LINE_LENGTH];
	
	sprintf(buffer, "%d", srcPC_ptr->getPresentationContextId());
	string pcId = buffer;
	sprintf(buffer, "%d", srcPC_ptr->getResultReason());
	string result = buffer;
	
	// search for matching presentation context
	for (UINT i = 0; i < noPresentationContexts(); i++)
	{
		ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS& pcValidator = getPresentationContext(i);
		
		// check for match
		if ((pcValidator.getPresentationContextId() == pcId) &&
			(pcValidator.getResult() == result) &&
			(pcValidator.getAbstractSyntaxName() == (char*) srcPC_ptr->getAbstractSyntaxName().get()))
		{
			// indicate that no reference was found
			pcValidator.notReferenced();
		}
	}
}

//>>===========================================================================		

bool ASSOCIATE_AC_VALIDATOR_CLASS::checkUniquePresentationContextId()

//  DESCRIPTION     : Check that the presentation context ids are all unique.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	bool result = true;
	
	// loop through all the presentation contexts
	for (UINT i = 0; i < noPresentationContexts(); i++)
	{
		ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS& pcValidator = getPresentationContext(i);
		int pcId = atoi((char*) pcValidator.getPresentationContextId().c_str());
		
		// check if pc id is in range
		if ((pcId > 0) &&
			(pcId < MAX_PC_ID))
		{
			// check if this pc id has already been seen
			if (usedPcIdM[pcId])
			{
				// pc id is not unique
				pcValidator.notUniquePcId();
				result = false;
			}
			else
			{
				// indicate that this pc id has been used
				usedPcIdM[pcId] = true;
			}
		}
	}

    // check the number of presentation contexts
    if (noPresentationContexts() > 127)
    {
		ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS& pcValidator = getPresentationContext(0);
        pcValidator.tooManyPresentationContexts();
    }

	// return result
	return result;
}

//>>===========================================================================		

bool ASSOCIATE_AC_VALIDATOR_CLASS::checkUniqueSOPClasses(void)

//  DESCRIPTION     : Check that the presentation context ids contain unique
//                    SOP classes with Transfer Syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	bool	result = true;
	int		i;
	int		j;
	int		noPC;
	
	noPC = noPresentationContexts();
	
	// loop through all the presentation contexts
	for (i=0 ; i<noPC ; i++)
	{
		ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS	pcValidator1 = getPresentationContext(i);
		
		for (j=i+1 ; j<noPC ; j++)
		{
			ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS&	pcValidator2 = getPresentationContext(j);
			
			// check for match
			if ((pcValidator1.getAbstractSyntaxName() == pcValidator2.getAbstractSyntaxName()) &&
				(pcValidator1.getResult() == pcValidator2.getResult()) &&
				(pcValidator1.getTransferSyntaxName() == pcValidator2.getTransferSyntaxName()) &&
				(pcValidator1.getTransferSyntaxName().length() > 0))
			{
				// indicate that the a SOP class with a transfer syntax has
				// been specified multiple times.
				pcValidator2.notUniqueSOP(pcValidator1.getPresentationContextId(), pcValidator2.getPresentationContextId());
				result = false;
			}
		}
	}
	
	// return result
	return result;
}
