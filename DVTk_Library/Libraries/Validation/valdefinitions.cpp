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
#include "Iglobal.h"      // Global component interface
#include "valdefinitions.h"

//>>===========================================================================

int StringStrip(const char *string,		//Original string
			int maxLength,				//Maximum String length
			bool leadingSpace,			//Leading Spaces significant - true / false flag
			bool trailingSpace,			//Trailing Spaces significant - true / false flag
			char **string_ptr_ptr)      //Address of first significant character

//  DESCRIPTION     : Function to strip the given string down to the significant 
//				      characters (leading and tailing spaces may be stripped off).	
//					  Return a pointer to the first significant character and the	
//					  stripped string length.						
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// if there is no value, return
	if (string == NULL)
	{
		return 0;
	}

	// get normal string length					
	char *chr_ptr = (char*)string;
	int	length = 0;
	while ((*chr_ptr++ != NullChar) && 
        (length < maxLength)) 
	{
		length++;
	}

	// check for insignificant leading spaces		
	chr_ptr = (char*)string;
	if (leadingSpace == false) 
	{
		while ((*chr_ptr == ' ') && 
            (length > 0)) 
		{
			chr_ptr++;
			length--;
		}
	}
	*string_ptr_ptr = chr_ptr;

	// check for insignificant trailing spaces			
	if ((trailingSpace == false) && 
        (length > 0)) 
	{
		chr_ptr = (*string_ptr_ptr) + length - 1;
		while ((*chr_ptr == ' ') && 
            (length > 0)) 
		{
			chr_ptr--;
			length--;
		}
	}

	return length;
}

//>>===========================================================================

bool StringValuesEqual(const char *refValue_ptr,
				  const char *srcValue_ptr,
				  int maxLength,
				  bool leadingSpace,
				  bool trailingSpace)

//  DESCRIPTION     : Function to compare two "string" based VR types of the 
//                    (maximum) given length taking into account the 
//                    significance of any leading/trailing spaces.					
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	char *lSrcValue_ptr;
	char *lRefValue_ptr;

	// If 1 or both of the values is missing they are not equal!
	if ((!refValue_ptr) || 
        (!srcValue_ptr))
	{
		return false;
	}

	// get significant reference string content
	int	refLength = StringStrip(refValue_ptr, 
							maxLength, 
							leadingSpace,
			                trailingSpace, 
							&lRefValue_ptr);

	// get significant received string content			
	int srcLength = StringStrip(srcValue_ptr, 
							maxLength, 
							leadingSpace,
			                trailingSpace, 
							&lSrcValue_ptr);

	// significant lengths should be the same
	if (refLength != srcLength) 
	{
		return false;
	}

	// compare the significant string parts				
	if (memcmp(lRefValue_ptr, lSrcValue_ptr, refLength) == 0) 
	{
		return true;
	}

	return false;
}
