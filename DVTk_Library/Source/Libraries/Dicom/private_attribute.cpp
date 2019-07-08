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
#include "private_attribute.h"

#include "Idefinition.h"		// Definition component interface


//>>===========================================================================

PRIVATE_RECOGNITION_CODE_CLASS::PRIVATE_RECOGNITION_CODE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	streamedInM = false;
	recognitionCodeM_ptr = NULL;
	inGroupM = 0;
	inElementM = 0;
	mappedGroupM = 0;
	mappedElementM = 0;
}

//>>===========================================================================

PRIVATE_RECOGNITION_CODE_CLASS::PRIVATE_RECOGNITION_CODE_CLASS(BYTE *data_ptr, UINT16 inGroup, UINT16 inElement, UINT16 mappedGroup, UINT16 mappedElement)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_LO);
	value_ptr->Set(data_ptr, byteStrLen(data_ptr));

	streamedInM = true;
	recognitionCodeM_ptr = value_ptr;
	inGroupM = inGroup;
	inElementM = inElement;
	mappedGroupM = mappedGroup;
	mappedElementM = mappedElement;
}

//>>===========================================================================

PRIVATE_RECOGNITION_CODE_CLASS::PRIVATE_RECOGNITION_CODE_CLASS(PRIVATE_RECOGNITION_CODE_CLASS &recCode)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = recCode;
}

//>>===========================================================================

PRIVATE_RECOGNITION_CODE_CLASS::~PRIVATE_RECOGNITION_CODE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (recognitionCodeM_ptr)
	{
		delete recognitionCodeM_ptr;
	}
}

//>>===========================================================================

void PRIVATE_RECOGNITION_CODE_CLASS::setRecognitionCode(BYTE *data_ptr)

//  DESCRIPTION     : Set the recognition code.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the recognition code.
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_LO);
	value_ptr->Set(data_ptr, byteStrLen(data_ptr));

	if (recognitionCodeM_ptr)
	{
		delete recognitionCodeM_ptr;
	}

	recognitionCodeM_ptr = value_ptr;
}

//>>===========================================================================

BYTE *PRIVATE_RECOGNITION_CODE_CLASS::getRecognitionCode()

//  DESCRIPTION     : Get the recognition code.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	BYTE *data_ptr = NULL;
	if (recognitionCodeM_ptr)
	{
		UINT32 length;
		recognitionCodeM_ptr->Get(&data_ptr, length);
	}
	return data_ptr;
}

//>>===========================================================================

bool PRIVATE_RECOGNITION_CODE_CLASS::operator = (PRIVATE_RECOGNITION_CODE_CLASS &recCode)

//  DESCRIPTION     : Operator equal.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// copy recognition code data
	streamedInM = recCode.isStreamedIn();
	recognitionCodeM_ptr = NULL;

	if (recCode.getRecognitionCode())
	{
		setRecognitionCode(recCode.getRecognitionCode());
	}

	inGroupM = recCode.getInGroup();
	inElementM = recCode.getInElement();
	mappedGroupM = recCode.getMappedGroup();
	mappedElementM = recCode.getMappedElement();

	// return result
	return true;
}


//>>===========================================================================

PRIVATE_RECOGNITION_CODE_TABLE_CLASS::PRIVATE_RECOGNITION_CODE_TABLE_CLASS()

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

PRIVATE_RECOGNITION_CODE_TABLE_CLASS::PRIVATE_RECOGNITION_CODE_TABLE_CLASS(PRIVATE_RECOGNITION_CODE_TABLE_CLASS &recCodeTable)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = recCodeTable;
}

//>>===========================================================================

PRIVATE_RECOGNITION_CODE_TABLE_CLASS::~PRIVATE_RECOGNITION_CODE_TABLE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	while (recCodeM.getSize())
	{
		// remove entries
		recCodeM.removeAt(0);
	}
}

//>>===========================================================================

bool PRIVATE_RECOGNITION_CODE_TABLE_CLASS::operator = (PRIVATE_RECOGNITION_CODE_TABLE_CLASS &recCodeTable)

//  DESCRIPTION     : Operator equal.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// copy recognition code table data
	while (recCodeM.getSize())
	{
		// remove any old mappings
		recCodeM.removeAt(0);
	}

	// copy new mappings
	for (UINT i = 0; i < recCodeTable.noRecCodes(); i++)
	{
		PRIVATE_RECOGNITION_CODE_CLASS recCode = recCodeTable.getRecCode(i);
		recCodeM.add(recCode);
	}

	// return result
	return true;
}

//>>===========================================================================

bool PRIVATE_RECOGNITION_CODE_TABLE_CLASS::setStreamedIn(UINT i, bool streamedIn)

//  DESCRIPTION     : Set the indexed entry streamed in bool.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    bool result = false;

    if (i < recCodeM.getSize())
    {
        recCodeM[i].setStreamedIn(streamedIn);
        result = true;
    }

    return result;
}

//>>===========================================================================

bool PRIVATE_RECOGNITION_CODE_TABLE_CLASS::setInTag(UINT i, UINT16 group, UINT16 element)

//  DESCRIPTION     : Set the indexed input tag - group and element.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    bool result = false;

    if (i < recCodeM.getSize())
    {
        recCodeM[i].setInTag(group, element);
        result = true;
    }

    return result;
}

//>>===========================================================================

UINT16 PRIVATE_RECOGNITION_CODE_TABLE_CLASS::getMappedGroup(UINT i)

//  DESCRIPTION     : Get the indexed mapped group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    UINT16 group = TAG_UNDEFINED_GROUP;
    
    if (i < recCodeM.getSize())
    {
        group = recCodeM[i].getMappedGroup();
    }

    return group;
}

//>>===========================================================================

UINT16 PRIVATE_RECOGNITION_CODE_TABLE_CLASS::getMappedElement(UINT i)

//  DESCRIPTION     : Get the indexed mapped element.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    UINT16 element = TAG_UNDEFINED_ELEMENT;
    
    if (i < recCodeM.getSize())
    {
        element = recCodeM[i].getMappedElement();
    }

    return element;
}


//>>===========================================================================

PRIVATE_ATTRIBUTE_HANDLER_CLASS::PRIVATE_ATTRIBUTE_HANDLER_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// - initialise the free recognition codes table
	for (UINT i = 0; i < MAX_FREE_CODES; i++)
	{
		freeCodesM[i] = true;
	}
}

//>>===========================================================================

PRIVATE_ATTRIBUTE_HANDLER_CLASS::~PRIVATE_ATTRIBUTE_HANDLER_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	remove();
}

//>>===========================================================================

bool PRIVATE_ATTRIBUTE_HANDLER_CLASS::install()

//  DESCRIPTION     : Install initial recognition code table on top of the stack.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise freeCodes array
	// - 0x01 to 0x10 cannot be recognition codes
	for (int i = 0; i <= 0x10; i++)
	{
		freeCodesM[i] = false;
	}

	for (int i = 0x11; i < MAX_FREE_CODES; i++)
	{
		freeCodesM[i] = true;
	}

	// free up any old recognition tables
	while(recCodeTableM.getSize())
	{
		recCodeTableM.removeAt(0);
	}

	// set up some test recognition codes
	PRIVATE_RECOGNITION_CODE_TABLE_CLASS recTable;

    for (UINT32 j = 0; j < DEFINITION->GetNrRecognitionCodes(); j++)
	{
		UINT32 tag;
		string recCode;

		// get the next recognition code from the definitions
		DEFINITION->GetRecognitionCode(j, &tag, recCode);

		// convert tag into group and element
		UINT16 group = (UINT16) (tag >> 16);
		UINT16 element = (UINT16) (tag & 0x0000FFFF);

		// initialise the private recognition code
		PRIVATE_RECOGNITION_CODE_CLASS privateRecCode((BYTE*) recCode.c_str(), group, element, group, element);	
		recTable.addRecCode(privateRecCode);
	}

	// store the first table
	recCodeTableM.add(recTable);

	// return result
	return true;
}

//>>===========================================================================

bool PRIVATE_ATTRIBUTE_HANDLER_CLASS::remove()

//  DESCRIPTION     : Clean up the recognition codes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// clean up
	while (recCodeTableM.getSize())
	{
		// remove entries
		recCodeTableM.removeAt(0);
	}

	// return result
	return true;
}

//>>===========================================================================

bool PRIVATE_ATTRIBUTE_HANDLER_CLASS::pushRecognitionCodeTable()

//  DESCRIPTION     : Initialise new recognition code table and push it onto the stack.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check that the private attribute handling has been initialised
	if (!recCodeTableM.getSize()) return false;

	// initialise a new recognition table and add to the stack
	PRIVATE_RECOGNITION_CODE_TABLE_CLASS recCodeTable = recCodeTableM[0];

	// add as last (top) entry
	recCodeTableM.add(recCodeTable);

	// return result
	return true;
}

//>>===========================================================================

bool PRIVATE_ATTRIBUTE_HANDLER_CLASS::popRecognitionCodeTable()

//  DESCRIPTION     : Pop current recognition code table off the stack (and delete it).
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// remove the last recognition code table
	if (recCodeTableM.getSize())
	{
		// remove last entry
		recCodeTableM.removeAt(recCodeTableM.getSize() - 1);
		result = true;
	}

	// return result
	return result;
}

//>>===========================================================================

bool PRIVATE_ATTRIBUTE_HANDLER_CLASS::registerRecognitionCode(BYTE *recognitionCode_ptr, UINT16 inGroup, UINT16 inElement, UINT16 *outGroup_ptr, UINT16 *outElement_ptr)

//  DESCRIPTION     : Register a private recognition code.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool found = false;

    UINT recognitionCodeLength = byteStrLen(recognitionCode_ptr);
    if (recognitionCodeLength > LO_LENGTH) return false;

    // make local copy and remove any trailing space characters
    BYTE recCodeCopy[LO_LENGTH +1];
    byteCopy(recCodeCopy, recognitionCode_ptr, recognitionCodeLength);
    recCodeCopy[recognitionCodeLength] = NULLCHAR;
    while ((recognitionCodeLength > 0) &&
        (recCodeCopy[recognitionCodeLength - 1] == SPACECHAR))
    {
        recCodeCopy[recognitionCodeLength - 1] = NULLCHAR;
        recognitionCodeLength--;
    }

	// check that the private attribute handling has been initialised
	if (!recCodeTableM.getSize()) return false;

	// last entry is the current top of stack
	UINT index = recCodeTableM.getSize() - 1;

	// search through table to see if recognition code already mapped
    UINT i;
	for (i = 0; i < recCodeTableM[index].noRecCodes(); i++)
	{
		PRIVATE_RECOGNITION_CODE_CLASS recCode = recCodeTableM[index].getRecCode(i);

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Private attribute recognition code looking for \"%s\" - found \"%s\"", recCodeCopy, recCode.getRecognitionCode());
		}

		// check for match
		if ((byteStrLen(recCodeCopy) == byteStrLen(recCode.getRecognitionCode())) &&
			(byteCompare(recCodeCopy, recCode.getRecognitionCode(), byteStrLen(recCodeCopy))) &&
			(inGroup == recCode.getInGroup()))
		{
			// found a match
			found = true;
			break;
		}
	}

	// if a match was found
	if (found)
	{
		// remap the tags
        recCodeTableM[index].setStreamedIn(i, true);
        recCodeTableM[index].setInTag(i, inGroup, inElement);
        *outGroup_ptr = recCodeTableM[index].getMappedGroup(i);
        *outElement_ptr = recCodeTableM[index].getMappedElement(i);

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Private attribute recognition code \"%s\" - remapping from (%04X,%04X) to (%04X,%04X)", recCodeCopy, inGroup, inElement, *outGroup_ptr, *outElement_ptr);
		}

		// return result
		return true;
	}

	// not found in current recognition table
	// - so not present in stacked tables either
	// - install in all stacked tables so that private tags will be the same mapping everywhere
	// - try to find an unused freeCode
	UINT16 j;
	for (j = MAX_FREE_CODES - 1; (!freeCodesM[j]) && (j >=0); j--)
	{
		; // void
	}

	// check if a free code was found
	if (j < 0) return false;

	// signal code is used
	freeCodesM[j] = false;
	*outGroup_ptr = inGroup;
	*outElement_ptr = j;

	// set up a new mapping
	PRIVATE_RECOGNITION_CODE_CLASS recCode(recCodeCopy, inGroup, inElement, *outGroup_ptr, *outElement_ptr);
	for (i = 0; i < recCodeTableM.getSize(); i++)
	{
		// add recognition code to each table in stack
		recCodeTableM[i].addRecCode(recCode);
	}

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Private attribute recognition code \"%s\" - mapping from (%04X,%04X) to (%04X,%04X)", recCodeCopy, inGroup, inElement, *outGroup_ptr, *outElement_ptr);
	}

	// return result
	return true;
}

//>>===========================================================================
bool PRIVATE_ATTRIBUTE_HANDLER_CLASS::usePrivateAttributeMapping = true;

bool PRIVATE_ATTRIBUTE_HANDLER_CLASS::mapTagValue(UINT16 inGroup, UINT16 inElement, UINT16 *outGroup_ptr, UINT16 *outElement_ptr)

//  DESCRIPTION     : Map private tag to base private tag using recognition code provided.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check that the private attribute handling has been initialised
	if (!recCodeTableM.getSize()) return false;

	// last entry is the current top of stack
	UINT index = recCodeTableM.getSize() - 1;

	// search through table to see if recognition code already mapped
	for (UINT i = 0; i < recCodeTableM[index].noRecCodes(); i++)
	{
		PRIVATE_RECOGNITION_CODE_CLASS recCode = recCodeTableM[index].getRecCode(i);

		// check for match
		if ((recCode.isStreamedIn()) &&
			(inGroup == recCode.getInGroup()) &&
			(((inElement & 0xFF00) >> 8) == recCode.getInElement()))
		{
			// match found
			if (PRIVATE_ATTRIBUTE_HANDLER_CLASS::usePrivateAttributeMapping)
			{
				*outGroup_ptr = recCode.getMappedGroup();
				*outElement_ptr = ((recCode.getMappedElement() & 0x00FF) << 8) | (inElement & 0x00FF);
			}
			else
			{
				*outGroup_ptr = inGroup;
				*outElement_ptr = inElement;
			}
			result = true;

			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Private attribute mapping from (%04X,%04X) to (%04X,%04X)", inGroup, inElement, *outGroup_ptr, *outElement_ptr);
			}

			break;
		}
	}
			
	// return result;
	return result;
}


