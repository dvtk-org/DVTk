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
//  DESCRIPTION     :	Extended Character Set Definition Classes
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ext_char_set_file.h"
#include "ext_char_set_definition.h"
#include "specific_character_set.h"
#include "Iutility.h"
#include "../validation/valrules.h"

#define MAX_MESSAGE_LENGTH  2048

/*--------------------------------------------------------------------------*/
/*	CLASS:	CODE_ELEMENT_CLASS												*/
/*--------------------------------------------------------------------------*/

//>>===========================================================================

CODE_ELEMENT_CLASS::CODE_ELEMENT_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Parameters are set assuming default character repertoire
//                    single byte, no code extensions
//<<===========================================================================
{
	// constructor activities
	defined_termM = "none";	
	std_for_code_extM = "";
	escape_sequence_crM = "";
	iso_reg_nrM = "ISO-IR 6";	
	nr_charsM = 94;		
	code_elementM = CS_CODE_ELEMENT_NAME_G0;	
	character_setM = "ISO 646";	
}

//>>===========================================================================

CODE_ELEMENT_CLASS::CODE_ELEMENT_CLASS(const string& defined_term)

//  DESCRIPTION     : Constructor with name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	defined_termM = defined_term;
}

//>>===========================================================================

CODE_ELEMENT_CLASS::~CODE_ELEMENT_CLASS()

//  DESCRIPTION     : Default destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

void CODE_ELEMENT_CLASS::SetMultiByte(bool b)

//  DESCRIPTION     : Sets multi-byte character set 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	multi_byteM = b;
}

//>>===========================================================================

void CODE_ELEMENT_CLASS::SetDefinedTerm(const string& term)

//  DESCRIPTION     : Sets defined term
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	defined_termM = term;
}

//>>===========================================================================

void CODE_ELEMENT_CLASS::SetStdForCodeExt(const string& std)

//  DESCRIPTION     : Sets standard for code extension
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	std_for_code_extM = std;
}

//>>===========================================================================

void CODE_ELEMENT_CLASS::SetEscSequenceCRFormat(const string& esc)

//  DESCRIPTION     : Sets escape sequence
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	escape_sequence_crM = esc;

	// also set escape sequence in ASCII format
	// assume the passed escape sequence is in the correct format
	// that is column/row format
	// for example: 02/08 04/02
	// first insert space in front to create symmetry
	// this simplifies algorithm
	string str = ' ' + esc;
	UINT length = str.length();

	if ((length % 6) != 0)
	{
		// TODO error: illegal escape sequence
		return;
	}

	string col;
	string row;
	char c[2];
	string cr_code;
	UINT i = 0;
	while (i < length)
	{
		cr_code = str.substr(i, 6);
		if (cr_code[0] != ' ')
		{
			// illegal formed escape sequence
			++i;
		}
		else if ( cr_code[3] != '/')
		{
			// illegal formed escape sequence
			return;
		}
		else
		{
			// copy column
			col = cr_code.substr(1, 2);

			// copy row
			row = cr_code.substr(4,2);
		
			// convert row/column value to number
			c[0] = (char) ((atoi(col.c_str()) * 16) + atoi(row.c_str()));
			c[1] = '\0';
			escape_sequenceM += c;
		}
	    i+=6;
	}
}

//>>===========================================================================

void CODE_ELEMENT_CLASS::SetISORegNr(const string& regnr)

//  DESCRIPTION     : Sets ISO registration number
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	iso_reg_nrM = regnr;	
}

//>>===========================================================================

void CODE_ELEMENT_CLASS::SetNrChars(const string& nr)

//  DESCRIPTION     : Sets nr of characters
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	nr_charsM = nr;
}

//>>===========================================================================

void CODE_ELEMENT_CLASS::SetCodeElementName(const string& name)

//  DESCRIPTION     : Sets code element name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	code_elementM = name;
}

//>>===========================================================================

void CODE_ELEMENT_CLASS::SetCharacterSet(const string& cs)

//  DESCRIPTION     : Sets character set
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	character_setM = cs;
}

//>>===========================================================================

bool CODE_ELEMENT_CLASS::GetMultiByte()

//  DESCRIPTION     : returns multi-byte flag
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return multi_byteM;
}

//>>===========================================================================

string CODE_ELEMENT_CLASS::GetDefinedTerm()

//  DESCRIPTION     : return defined term
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return defined_termM;
}

//>>===========================================================================

string CODE_ELEMENT_CLASS::GetStdForCodeExt()

//  DESCRIPTION     : returns standard for code extension
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return std_for_code_extM;
}

//>>===========================================================================

string CODE_ELEMENT_CLASS::GetEscSequence()

//  DESCRIPTION     : Returns escape sequence
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return escape_sequenceM;
}

//>>===========================================================================

string CODE_ELEMENT_CLASS::GetEscSequenceCRFormat()

//  DESCRIPTION     : Returns escape sequence in cr format
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return escape_sequence_crM;
}

//>>===========================================================================

string CODE_ELEMENT_CLASS::GetISORegNr()

//  DESCRIPTION     : Return ISO registration number
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return iso_reg_nrM;
}

//>>===========================================================================

string CODE_ELEMENT_CLASS::GetNrChars()

//  DESCRIPTION     : Returns number of characters
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return nr_charsM;
}

//>>===========================================================================

string CODE_ELEMENT_CLASS::GetCodeElementName()

//  DESCRIPTION     : Return code element name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return code_elementM;
}

//>>===========================================================================

string CODE_ELEMENT_CLASS::GetCharacterSet()

//  DESCRIPTION     : Returns character set
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return character_setM;
}


/*--------------------------------------------------------------------------*/
/*	CLASS:	CHARACTER_SET_CLASS												*/
/*--------------------------------------------------------------------------*/

//>>===========================================================================

CHARACTER_SET_CLASS::CHARACTER_SET_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	multi_byteM	= false;      
	code_extensionsM = false; 
}

//>>===========================================================================

CHARACTER_SET_CLASS::CHARACTER_SET_CLASS(const string& d, bool code_ext)
: descriptionM(d), code_extensionsM(code_ext)

//  DESCRIPTION     : Construction with description, code extensions
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CHARACTER_SET_CLASS::~CHARACTER_SET_CLASS()

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

void CHARACTER_SET_CLASS::SetMultiByte(bool b)

//  DESCRIPTION     : Sets multi-byte character set efault destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	multi_byteM = b;
}

//>>===========================================================================

void CHARACTER_SET_CLASS::SetCodeExtensions(bool b)

//  DESCRIPTION     : Sets code extensions
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	code_extensionsM = b;
}

//>>===========================================================================

bool CHARACTER_SET_CLASS::GetMultiByte()

//  DESCRIPTION     : returns multi-byte flag
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return multi_byteM;
}

//>>===========================================================================

bool CHARACTER_SET_CLASS::GetCodeExtensions()

//  DESCRIPTION     : returns code extensions flag
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return code_extensionsM;
}

//>>===========================================================================

void CHARACTER_SET_CLASS::AddCodeElement(CODE_ELEMENT_CLASS* ce_ptr)

//  DESCRIPTION     : Adds a code element to a characterset definition
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	code_elementsM.push_back(ce_ptr);
}

//>>===========================================================================

int CHARACTER_SET_CLASS::GetNrCodeElements()

//  DESCRIPTION     : Returns the number of code elements in a character set
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return code_elementsM.size();
}

//>>===========================================================================

CODE_ELEMENT_CLASS* CHARACTER_SET_CLASS::GetCodeElement(int i)

//  DESCRIPTION     : returns requested code element
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return code_elementsM[i];
}

//>>===========================================================================

CODE_ELEMENT_CLASS* CHARACTER_SET_CLASS::GetCodeElementByDefinedTerm(const string& term)

//  DESCRIPTION     : returns requested code element
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CODE_ELEMENT_CLASS* ce_ptr = NULL;

	for (UINT i = 0; i < code_elementsM.size(); i++)
	{
		if (code_elementsM[i]->GetDefinedTerm() == term)
		{
			ce_ptr = code_elementsM[i];
			break;
		}
	}

	return ce_ptr;
}

//>>===========================================================================

CODE_ELEMENT_CLASS* CHARACTER_SET_CLASS::GetCodeElementByEscSequence(BYTE* esc_sq_ptr, UINT maxLength)

//  DESCRIPTION     : returns requested code element
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CODE_ELEMENT_CLASS* ce_ptr = NULL;

	for (UINT i = 0; i < code_elementsM.size(); i++)
	{
		string esc_sq = code_elementsM[i]->GetEscSequence();
        UINT length = esc_sq.length();
		if (length <= maxLength && length != 0)
		{
			// the escape sequence is short enough
			if (strncmp(esc_sq.c_str(), (char*)esc_sq_ptr, length) == 0)
			{
				// got it
				ce_ptr = code_elementsM[i];
				break;
			}
		}
	}

	return ce_ptr;
}

/*--------------------------------------------------------------------------*/
/*	CLASS:	CHARACTER_SET_REGISTER_CLASS									*/
/*--------------------------------------------------------------------------*/
// initialise static pointer
CHARACTER_SET_REGISTER_CLASS* CHARACTER_SET_REGISTER_CLASS::instanceM_ptr = NULL;

//>>===========================================================================

CHARACTER_SET_REGISTER_CLASS::CHARACTER_SET_REGISTER_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
    characterSetDataLoadedM = false;
}

//>>===========================================================================

CHARACTER_SET_REGISTER_CLASS* CHARACTER_SET_REGISTER_CLASS::Instance()

//  DESCRIPTION     : Get the singleton instance
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) 
	{
		instanceM_ptr = new CHARACTER_SET_REGISTER_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

void CHARACTER_SET_REGISTER_CLASS::Cleanup()

//  DESCRIPTION     : Cleanup the singleton.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up resources
	for (UINT i = 0; i < character_setsM.size(); i++)
	{
		delete character_setsM[i];
	}
	character_setsM.clear();
}

//>>===========================================================================

bool CHARACTER_SET_REGISTER_CLASS::LoadCharacterSets(string filename)

//  DESCRIPTION     : Load the character sets from the given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // check if the character set data has already been loaded
    if (!characterSetDataLoadedM)
    {
    	EXT_CHAR_SET_FILE_CLASS ext_char_set_file(filename);

        // parse the character set file
	    characterSetDataLoadedM = ext_char_set_file.execute();
    }

    return characterSetDataLoadedM;
}

//>>===========================================================================

void CHARACTER_SET_REGISTER_CLASS::AddCharacterSet(CHARACTER_SET_CLASS* cs_ptr)

//  DESCRIPTION     : Add a character set to the register.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    character_setsM.push_back(cs_ptr);
}

//>>===========================================================================

int CHARACTER_SET_REGISTER_CLASS::GetNrCharacterSets()

//  DESCRIPTION     : Get the number of character sets installed.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return character_setsM.size();
}

//>>===========================================================================

CHARACTER_SET_CLASS* CHARACTER_SET_REGISTER_CLASS::GetCharacterSet(int i)

//  DESCRIPTION     : Get the indexed character set.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return character_setsM[i];
}

//>>===========================================================================

CODE_ELEMENT_CLASS* CHARACTER_SET_REGISTER_CLASS::GetCodeElementByDefinedTerm(const string& term)

//  DESCRIPTION     : Get the code element identified by the defined term.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CODE_ELEMENT_CLASS* ce_ptr = NULL;

	for (UINT i = 0; i < character_setsM.size(); i++)
	{
		ce_ptr = character_setsM[i]->GetCodeElementByDefinedTerm(term);
		if (ce_ptr)
		{
			break;
		}
	}

	return ce_ptr;
}

//>>===========================================================================

CODE_ELEMENT_CLASS* CHARACTER_SET_REGISTER_CLASS::GetCodeElementByEscSequence(BYTE* esc_sq_ptr, UINT maxLength)

//  DESCRIPTION     : Get the code element identified by the escape sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CODE_ELEMENT_CLASS* ce_ptr = NULL;

	for (UINT i = 0; i < character_setsM.size(); i++)
	{
		ce_ptr = character_setsM[i]->GetCodeElementByEscSequence(esc_sq_ptr, maxLength);
		if (ce_ptr)
		{
			break;
		}
	}

	return ce_ptr;
}

//>>===========================================================================

DVT_STATUS 
CHARACTER_SET_REGISTER_CLASS::IsValidExtendedChar(BYTE *value_ptr,
                                                  UINT length,
                                                  ATTR_VR_ENUM vr, 
                                                  UINT max_chars, 
                                                  LOG_MESSAGE_CLASS *messages, 
                                                  SPECIFIC_CHARACTER_SET_CLASS *specific_character_set)

//  DESCRIPTION     : Checks whether the input string is valid regarding 
//                    extended character sets
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	DVT_STATUS result = MSG_OK;		    	    // overall results of function
	CODE_ELEMENT_CLASS* base_ce_ptr = NULL;     // base character set (code element)
	CODE_ELEMENT_CLASS* active_ce_ptr = NULL;   // active code element
	bool multi_byte_supported = false;		    // indicates multi-byte is supported
	bool extensions_allowed   = false;		    // indicates code extensions are allowed
	UINT char_count	= 0;			            // character counter
	UINT component = 1;			                // component counter for PN value
	UINT component_group = 1;			        // component group counter for PN value    
    bool crMessageDisplayed = false;            // display CR error message only once
    bool lfMessageDisplayed = false;            // display LF error message only once
	bool validChSetMessageDisplayed = false;    // display valid character set message only once
	bool codeExtMessageDisplayed = false;		// display code extension message only once
	bool unknownEscSeqMessageDisplayed = false;	// display Unknown Esc Seq message only once
    char message[MAX_MESSAGE_LENGTH];

	// extended character set validation can only be done if the character set
	// definitions are actually loaded
	if ((GetNrCharacterSets() == 0) ||
        (specific_character_set == NULL))
	{
		// no character sets loaded - can't continue
		// - don't flag warning here as this will be displayed for every
		// extended character attribute value being validated
		// - a warning will be displayed as the session is loaded
		// return silently for now - this needs further work
		return MSG_OK;
	}

	// First get an appropriate code element definition
	// The the number of values defined by the specific 
	// character set attribute
	UINT nr_defined_char_sets = specific_character_set->GetNrCharacterSets();
	string code_element = specific_character_set->GetCharacterSetName(1);
	if ((nr_defined_char_sets == 0) || (code_element.length() == 0))
	{
		// No value was specified, load default
		base_ce_ptr = GetCodeElementByDefinedTerm("none");
	}
	else if (nr_defined_char_sets == 1)
	{
		// In this case only single byte code tables are supported
		// and no code extensions are allowed
		// Note: It would be better if errors resulting from this check 
		//      are added to the results for the specific character set attribute
		base_ce_ptr = GetCodeElementByDefinedTerm(code_element);
		if (base_ce_ptr)
		{
			// Only single byte code tables are supported
			if (base_ce_ptr->GetMultiByte())
			{
                sprintf (message, "For single value in specific character set, only single byte codes allowed");
                messages->AddMessage (VAL_RULE_D_EXT_1, message);
			}

			// No code extensions are allowed
			if (base_ce_ptr->GetEscSequence().length())
			{
				sprintf (message, "For single value in specific character set, no code extensions allowed");
                messages->AddMessage (VAL_RULE_D_EXT_1, message);
            }
		}
	}
	else if (nr_defined_char_sets > 1)
	{
		// check if the code element has been defined
		if (code_element.length() == 0)
		{
			// try to get default
			base_ce_ptr = GetCodeElementByDefinedTerm("ISO 2022 IR 6");
		}
		else
		{
			// try to get defined code element
			base_ce_ptr = GetCodeElementByDefinedTerm(code_element);
		}

		// In principle code extensions and multi-byte allowed
		multi_byte_supported = true;
		extensions_allowed = true;
	}

	if (base_ce_ptr == NULL)
	{
		sprintf (message, "Could not check extended character set properties");
        messages->AddMessage (VAL_RULE_D_EXT_1, message);
		return MSG_ERROR;
	}
	else
	{
		//default
		active_ce_ptr = base_ce_ptr;
	}

	//check value
	for (UINT i = 0; i < length; ++i)
	{
		if (value_ptr[i] == ESCAPE)
		{
			//Start of an escape sequence
			//Try to find a character set definition with this escape sequence
			active_ce_ptr = GetCodeElementByEscSequence(&(value_ptr[i+1]), (length - i - 1));
			if (active_ce_ptr == NULL)
			{
				//It is unknown if escape sequence may be used for other purposes
				//than code extension
				if(!unknownEscSeqMessageDisplayed)
				{
					sprintf (message, "Unknown escape sequence found in PN group:%d", component_group);
					messages->AddMessage (VAL_RULE_D_EXT_1, message);
					unknownEscSeqMessageDisplayed = true;
				}

				//switch to base character set
				active_ce_ptr = base_ce_ptr;
			}
			else
			{
				//We found a character set definition with this escape sequence.
				//Check if is a valid one, in relation to the values specified
				//in the specific character set
				if (!specific_character_set->IsValidCharacterSet(active_ce_ptr->GetDefinedTerm()))
				{
					if(!validChSetMessageDisplayed)
					{
						sprintf (message, "Referenced character set %s not specified via specific character set attribute (0x00080005)",
																 active_ce_ptr->GetDefinedTerm().c_str());	
						messages->AddMessage (VAL_RULE_D_EXT_7, message);
						validChSetMessageDisplayed = true;
					}
				}

				//Check if code extensions allowed
				if (extensions_allowed)
				{
					//check if the refered character sets actually supports extensions
					//if an escape sequence is defined extensions are allowed
					//it would be better to indicate this with a separate attribute
					if (!(active_ce_ptr->GetEscSequence().length()))
					{
						sprintf (message, "Character set %s does not support code extensions",
							                                      active_ce_ptr->GetDefinedTerm().c_str());
                        messages->AddMessage (VAL_RULE_D_EXT_1, message);
					}

					//Check exception for PN attributes
					//No code extensions allowed for first
					//component group of PN attribute
					if ((vr == ATTR_VR_PN) && (component_group == 1))
					{
						if(!codeExtMessageDisplayed)
						{
							sprintf (message, "Code extension not allowed in the first component of a PN attribute");
							messages->AddMessage (VAL_RULE_D_EXT_1, message);
							codeExtMessageDisplayed = true;
						}
					}
				}

				//skip over the rest of the escape sequence
				i += active_ce_ptr->GetEscSequence().length();
			}
		}
		else //other than ESCChar
		{
			//Check for multi-byte properties
			if (active_ce_ptr->GetMultiByte())
			{
				//check for length
				if ( i+1 > length )
				{
					sprintf (message, "String ends in the middle of a two byte character");
                    messages->AddMessage (VAL_RULE_D_EXT_9, message);

					//we can't validate any further
					return MSG_ERROR;
				}

				//Here a check can be done on each byte to check if it's
				//in the correct range
				//To do this properly the character range needs to be added 
				//to the character set definition. 
				//The valid range for the first byte is usually different 
				//from the valid range for the second byte.
				//Some of the chinese encodings have a split range 
				//(40-7E, A1-FE for the second byte in Big 5)

				//skip the next byte
				++i;

				char_count++;
			}
			else //single byte character
			{
				//check for control characters
				if (  ((value_ptr[i] == CARET) && (vr == ATTR_VR_PN))
				   || ((value_ptr[i] == EQUALCHAR) && (vr == ATTR_VR_PN))
				   || (value_ptr[i] == LINEFEED)
				   || (value_ptr[i] == CARRIAGERETURN)
				   || (value_ptr[i] == FORMFEED)
				   )
				{
					//check to make sure the base character set is active

					//Control characters and delimiters force
					//the character set to the base character set only
					active_ce_ptr = base_ce_ptr;
				}

				//Do some additional checks on contol characters
				if (  (vr == ATTR_VR_LO)
				   || (vr == ATTR_VR_PN)
				   || (vr == ATTR_VR_SH)
				   )
				{
					if (  value_ptr[i] == CARRIAGERETURN)
					{
						sprintf (message, "Carriage Return not allowed");
                        messages->AddMessage (VAL_RULE_D_EXT_1, message);
					}
					else if (value_ptr[i] == LINEFEED)
					{
						sprintf (message, "Line Feed not allowed");
                        messages->AddMessage (VAL_RULE_D_EXT_1, message);
					}
					else if (value_ptr[i] == FORMFEED)
					{
						sprintf (message, "Form Feed not allowed");						
                        messages->AddMessage (VAL_RULE_D_EXT_1, message);
					}
				}

				//check for CR/LF pair
				if ((value_ptr[i] == CARRIAGERETURN) &&
					(vr != ATTR_VR_UT))
				{
					if ( (i+1 >= length) || (value_ptr[i+1] != LINEFEED))
					{
                        // display message only once
                        if (!crMessageDisplayed)
                        {
						    sprintf (message, "Value contains at least one Carriage Return [CR] without a following Line Feed [LF]");
                            messages->AddMessage (VAL_RULE_D_EXT_1, message);
                    
                            UINT displayLength = length;
                            if ((displayLength * 4) > MAX_MESSAGE_LENGTH)
                            {
                                displayLength = MAX_MESSAGE_LENGTH / 4;
                                sprintf(message, "String is (Length:%d / Only displaying first %d bytes): ", length, displayLength);
                            }
                            else
                            {
                                sprintf(message, "String is (Length:%d): ", length);
                            }

                            for (UINT j = 0; j < displayLength; j++)
                            {
                                char hexValue[4];
                                sprintf(hexValue, " %02X", value_ptr[j]);
                                strcat(message, hexValue);
                            }
                            messages->AddMessage (VAL_RULE_D_EXT_1, message);
                            crMessageDisplayed = true;
                        }
					}
				}
				if ((value_ptr[i] == LINEFEED) &&
					(vr != ATTR_VR_UT))
				{
					if ( (i < 1) || (value_ptr[i-1] != CARRIAGERETURN))
					{
                        // display message only once
                        if (!lfMessageDisplayed)
                        {
						    sprintf (message, "Value contains at least one Line Feed [LF] without a preceeding Carriage Return [CR]");
                            messages->AddMessage (VAL_RULE_D_EXT_1, message);

                           UINT displayLength = length;
                            if ((displayLength * 4) > MAX_MESSAGE_LENGTH)
                            {
                                displayLength = MAX_MESSAGE_LENGTH / 4;
                                sprintf(message, "String is (Length:%d / Only displaying first %d bytes): ", length, displayLength);
                            }
                            else
                            {
                                sprintf(message, "String is (Length:%d): ", length);
                            }

                            for (UINT j = 0; j < displayLength; j++)
                            {
                                char hexValue[4];
                                sprintf(hexValue, " %02X", value_ptr[j]);
                                strcat(message, hexValue);
                            }
                            messages->AddMessage (VAL_RULE_D_EXT_1, message);
                            lfMessageDisplayed = true;
                        }
                    }
				}

				if (vr == ATTR_VR_PN)
				{
					if (value_ptr[i] == CARET)
					{
						//start of a new component
						component++;
						if (component > 5)
						{
							sprintf (message, "More than 5 components found in PN group %d", 
								                                     component_group);
                            messages->AddMessage (VAL_RULE_D_EXT_5, message);
						}
					}
					else if (value_ptr[i] == EQUALCHAR)
					{
						//start of a new component group
						//check the length of the last component group
						if (char_count > max_chars)
						{
							sprintf (message, "PN[component group %d] - value length %i exceeds maximum length %i", 
								component_group, char_count, max_chars);
                            messages->AddMessage (VAL_RULE_D_EXT_10, message);
						}

						component = 1;
						char_count = 0;
						component_group++;
						if (component_group > 3)
						{
							sprintf (message, "More than 3 groups found in PN value - found %d", component_group);
                            messages->AddMessage (VAL_RULE_D_EXT_4, message);
						}

					}
				}
			
				if (value_ptr[i] == END_G0_CHAR_SET)
				{
					sprintf (message, "DELETE control Character not allowed in DICOM strings");
                    messages->AddMessage (VAL_RULE_D_EXT_1, message);
				}

				char_count++;
			}
		}
	}

	//check to make sure the base character set is active

	//check the length of the string (or last component group in PN)
	//don't count trailing spaces in the character count (any spaces at the end should be in the base character set)
	if (vr == ATTR_VR_PN)
	{
		for (UINT j = (length - 1); j >= 0; j--)
		{
			if (value_ptr[j] != SPACECHAR)
			{
				break;
			}

			char_count--;
		}
	}

	if (char_count > max_chars)
	{
		if (vr == ATTR_VR_PN)
		{
			sprintf (message, "PN[component group %d] - value length %i exceeds maximum length %i", 
				component_group, char_count, max_chars);
            messages->AddMessage (VAL_RULE_D_EXT_10, message);
		}
		else
		{
			sprintf (message, "%s - value length %i exceeds maximum length %i", 
				stringVr(vr), char_count, max_chars);
            messages->AddMessage (VAL_RULE_D_EXT_10, message);
		}
	}

	return result;
}	
