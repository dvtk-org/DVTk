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
//  DESCRIPTION     :   Base value results class for validation.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "val_attribute.h"
#include "val_base_value.h"
#include "IAttributeGroup.h"    // AttributeGroup component interface file.
#include "Idefinition.h"        // Definition component interface file.
#include "Idicom.h"             // Dicom component interface file.

//>>===========================================================================

VAL_BASE_VALUE_CLASS::VAL_BASE_VALUE_CLASS()

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    messagesM_ptr = NULL;
    parentM_ptr = NULL;
    dcmValueM_ptr = NULL;
    defValueM_ptr = NULL;
    refValueM_ptr = NULL;
}

//>>===========================================================================

VAL_BASE_VALUE_CLASS::~VAL_BASE_VALUE_CLASS()

//  DESCRIPTION     : Class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    delete messagesM_ptr;
}

//>>===========================================================================

void VAL_BASE_VALUE_CLASS::SetParent(VAL_ATTRIBUTE_CLASS *parent_ptr)

//  DESCRIPTION     : Set parent.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    parentM_ptr = parent_ptr;
}

//>>===========================================================================

void VAL_BASE_VALUE_CLASS::SetDefValueList(VALUE_LIST_CLASS *defValueList_ptr)

//  DESCRIPTION     : Set the definition attribute value list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    defValueM_ptr = defValueList_ptr;
}

//>>===========================================================================

void VAL_BASE_VALUE_CLASS::SetRefValue(BASE_VALUE_CLASS *refValue_ptr)

//  DESCRIPTION     : Set the reference attribute value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    refValueM_ptr = refValue_ptr;
}

//>>===========================================================================

void VAL_BASE_VALUE_CLASS::SetDcmValue(BASE_VALUE_CLASS *dcmValue_ptr)

//  DESCRIPTION     : Set the dicom attribute value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    dcmValueM_ptr = dcmValue_ptr;
}

//>>===========================================================================

VALUE_LIST_CLASS *VAL_BASE_VALUE_CLASS::GetDefValueList()

//  DESCRIPTION     : Get the definition attribute value list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return defValueM_ptr;
}

//>>===========================================================================

BASE_VALUE_CLASS *VAL_BASE_VALUE_CLASS::GetRefValue()

//  DESCRIPTION     : Get the reference attribute value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return refValueM_ptr;
}

//>>===========================================================================

BASE_VALUE_CLASS *VAL_BASE_VALUE_CLASS::GetDcmValue()

//  DESCRIPTION     : Get the dicom attribute value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return dcmValueM_ptr;
}

//>>===========================================================================

DVT_STATUS VAL_BASE_VALUE_CLASS::Check(UINT32 flags, SPECIFIC_CHARACTER_SET_CLASS *specificCharacterSet_ptr)

//  DESCRIPTION     : Check the dicom attribute value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    DVT_STATUS status = MSG_NO_VALUE;
	UINT16 group = TAG_UNDEFINED_GROUP;
	UINT16 element = TAG_UNDEFINED_ELEMENT;

	if (parentM_ptr != NULL)
	{
		DEF_ATTRIBUTE_CLASS *defAttribute_ptr = parentM_ptr->GetDefAttribute();
		DCM_ATTRIBUTE_CLASS *dcmAttribute_ptr = parentM_ptr->GetDcmAttribute();
		DCM_ATTRIBUTE_CLASS *refAttribute_ptr = parentM_ptr->GetRefAttribute();
		if (dcmAttribute_ptr != NULL)
		{
			group = dcmAttribute_ptr->GetGroup();
			element = dcmAttribute_ptr->GetElement();
		}
		else if (defAttribute_ptr != NULL)
		{
			group = defAttribute_ptr->GetGroup();
			element = defAttribute_ptr->GetElement();
		}
		else if (refAttribute_ptr != NULL)
		{
			group = refAttribute_ptr->GetGroup();
			element = refAttribute_ptr->GetElement();
		}
	}

    if (dcmValueM_ptr != NULL)
    {
        LOG_MESSAGE_CLASS *logMessage_ptr = new LOG_MESSAGE_CLASS();

		// Perform special validation for Ref File ID
		
		//DICOM part 10; 8.5 CHARACTER SET
		//File IDs and File-set IDs shall be character strings made of characters from a subset 
		//of the G0 repertoire of ISO 8859. The following characters form this subset:
		//A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z (uppercase)
		//1, 2, 3, 4, 5, 6, 7, 8, 9, 0 and _ (underscore)

		//Annex F Basic Directory Information Object Definition (Normative), Table F.3-3 DIRECTORY 
		//INFORMATION MODULE says about attribute Referenced File ID (0004,1500): "A Multiple Value 
		//(See PS 3.5) which represents the ordered components of the File ID containing a "referenced object" 
		//or Referenced SOP Instance. A maximum of 8 components, each from 1 to 8 characters shall be used 
		//(see Section 8.2)."
		if((group == 0x0004) && (element == 0x1500))
		{
			char        message[1024];
			UINT   length  = dcmValueM_ptr->GetLength();
			string value;
			dcmValueM_ptr->Get(value,true);

			if (length > 0)
			{
				// check for A..Z, 0..9, UNDERSCORE
				for (UINT index = 0; index < length; index++) 
				{
					if (value[index] == NULLCHAR)
					{
						break;
					}

					if ( ( (value[index] < 'A')  ||
						   (value[index] > 'Z')
						 ) &&
						 ( (value[index] < '0')  ||
						   (value[index] > '9')
						 ) &&
						 (value[index] != UNDERSCORE) 
					   )
					{
						sprintf (message,"unexpected character %c=0x%02X at offset %d",
								(int) value[index], (int) value[index], index+1);
						logMessage_ptr->AddMessage (VAL_RULE_D_CS_2, message);

						status = MSG_ERROR;
					}
				}
			}
		}
		else
		{
			status = dcmValueM_ptr->Check(flags, NULL, logMessage_ptr, specificCharacterSet_ptr);
		}

		if (logMessage_ptr->GetNrMessages() > 0)
		{
			for (int i = 0; i < logMessage_ptr->GetNrMessages(); i++)
			{
				if (group != TAG_UNDEFINED_GROUP)
				{
					char message[1024];
					sprintf(message,
						"Attribute (%04X,%04X) %s",
						group,
						element,
						logMessage_ptr->GetMessage(i).c_str());
					GetMessages()->AddMessage(logMessage_ptr->GetMessageId(i), message);
				}
				else
				{
					GetMessages()->AddMessage(logMessage_ptr->GetMessageId(i), logMessage_ptr->GetMessage(i));
				}
			}
		}

		delete logMessage_ptr;
    }

    return status;
}

//>>===========================================================================

LOG_MESSAGE_CLASS *VAL_BASE_VALUE_CLASS::GetMessages()

//  DESCRIPTION     : Get the log messages.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (messagesM_ptr == NULL)
    {
        messagesM_ptr = new LOG_MESSAGE_CLASS();
    }

    return messagesM_ptr;
}

//>>===========================================================================

bool VAL_BASE_VALUE_CLASS::HasMessages()

//  DESCRIPTION     : Check if the value has log messages.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return (messagesM_ptr == NULL) ? false : true;
}

//>>===========================================================================

DVT_STATUS VAL_BASE_VALUE_CLASS::CompareRef()

//  DESCRIPTION     :
//  PRECONDITIONS   : The ref and dcm values are both available.
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	char message[1200];
	UINT16 group = TAG_UNDEFINED_GROUP;
	UINT16 element = TAG_UNDEFINED_ELEMENT;

	if (parentM_ptr != NULL)
	{
		DEF_ATTRIBUTE_CLASS *defAttribute_ptr = parentM_ptr->GetDefAttribute();
		DCM_ATTRIBUTE_CLASS *dcmAttribute_ptr = parentM_ptr->GetDcmAttribute();
		DCM_ATTRIBUTE_CLASS *refAttribute_ptr = parentM_ptr->GetRefAttribute();
		if (dcmAttribute_ptr != NULL)
		{
			group = dcmAttribute_ptr->GetGroup();
			element = dcmAttribute_ptr->GetElement();
		}
		else if (defAttribute_ptr != NULL)
		{
			group = defAttribute_ptr->GetGroup();
			element = defAttribute_ptr->GetElement();
		}
		else if (refAttribute_ptr != NULL)
		{
			group = refAttribute_ptr->GetGroup();
			element = refAttribute_ptr->GetElement();
		}
	}

    LOG_MESSAGE_CLASS *logMessage_ptr = new LOG_MESSAGE_CLASS();
    DVT_STATUS status = dcmValueM_ptr->Compare(logMessage_ptr, refValueM_ptr);
        
    if (logMessage_ptr->GetNrMessages() > 0)
    {
        for (int i = 0; i < logMessage_ptr->GetNrMessages(); i++)
        {
			if (group != TAG_UNDEFINED_GROUP)
			{
				char message[1024];
				sprintf(message,
					"Attribute (%04X,%04X) %s",
					group,
					element,
					logMessage_ptr->GetMessage(i).c_str());
				GetMessages()->AddMessage(logMessage_ptr->GetMessageId(i), message);
			}
			else
			{
				GetMessages()->AddMessage(logMessage_ptr->GetMessageId(i), logMessage_ptr->GetMessage(i));
			}
        }
    }
    delete logMessage_ptr;

    if ((status != MSG_OK) && 
        (status != MSG_EQUAL))
    {
        string dcmStringValue;
        string refStringvalue;

        dcmValueM_ptr->Get(dcmStringValue);
        refValueM_ptr->Get(refStringvalue);

		if (group != TAG_UNDEFINED_GROUP)
		{
	        if ((dcmStringValue.length() + refStringvalue.length()) < 1024)
			{
				sprintf(message,
					"Attribute (%04X,%04X) value of \"%s\" different from reference value of \"%s\"",
					group,
					element,
					dcmStringValue.c_str(), 
				    refStringvalue.c_str());
			}
			else
			{
				sprintf(message,
					"Attribute (%04X,%04X) value is different from reference value",
					group,
					element);
			}
		}
		else
		{
	        if ((dcmStringValue.length() + refStringvalue.length()) < 1024)
			{
				sprintf(message,
					"Attribute value of \"%s\" different from reference value of \"%s\"",
					dcmStringValue.c_str(), 
		            refStringvalue.c_str());
			}
			else
			{
				sprintf(message,
					"Attribute value is different from reference value");
			}
		}
        GetMessages()->AddMessage(VAL_RULE_R_VALUE_1, message);
    }

    return status;
}
