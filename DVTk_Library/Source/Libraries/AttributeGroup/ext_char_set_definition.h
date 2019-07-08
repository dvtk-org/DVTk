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

#ifndef EXT_CHAR_SET_DEFINITION_H
#define EXT_CHAR_SET_DEFINITION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface

#define EXTCHARACTERSET	CHARACTER_SET_REGISTER_CLASS::Instance()


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class SPECIFIC_CHARACTER_SET_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

enum CS_AREA_ENUM
{
    CS_AREA_CL,
	CS_AREA_GL,
    CS_AREA_CR,
	CS_AREA_GR,
};

enum CS_CODE_ELEMENT_NAME_ENUM
{
    CS_CODE_ELEMENT_NAME_G0,
	CS_CODE_ELEMENT_NAME_G1,
	CS_CODE_ELEMENT_NAME_G2,
	CS_CODE_ELEMENT_NAME_G3,
};


//>>***************************************************************************

class CODE_ELEMENT_CLASS

//  DESCRIPTION     : Code Element Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CODE_ELEMENT_CLASS();
		CODE_ELEMENT_CLASS(const string& d);
		~CODE_ELEMENT_CLASS();

		void SetMultiByte(bool b);
		void SetDefinedTerm(const string& term);
		void SetStdForCodeExt(const string& std);
		void SetEscSequenceCRFormat(const string& esc);
		void SetISORegNr(const string& regnr);
		void SetNrChars(const string& nr);
		void SetCodeElementName(const string& name);
		void SetCharacterSet(const string& cs);

		bool GetMultiByte();		
		string GetDefinedTerm();
		string GetStdForCodeExt();
		string GetEscSequenceCRFormat();
		string GetEscSequence();
		string GetISORegNr();
		string GetNrChars();
		string GetCodeElementName();
		string GetCharacterSet();


	private:
		bool multi_byteM;           // true if multi byte
		string defined_termM;	    // defined term
		string std_for_code_extM;   // standard for code extension
		string escape_sequence_crM;	// the escape sequence (literal), in row col format
        string escape_sequenceM;
		string iso_reg_nrM;			// ISO registration number
		string nr_charsM;			// Number of characters
		string code_elementM;		// code element
		string character_setM;		// character set
};

//>>***************************************************************************

class CHARACTER_SET_CLASS

//  DESCRIPTION     : Character Set Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CHARACTER_SET_CLASS();
		CHARACTER_SET_CLASS(const string& d, bool  code_ext);
		~CHARACTER_SET_CLASS();

		void SetMultiByte(bool b);
		void SetCodeExtensions(bool b);

		void AddCodeElement(CODE_ELEMENT_CLASS* el);
		int GetNrCodeElements();
		CODE_ELEMENT_CLASS* GetCodeElement(int i);
		CODE_ELEMENT_CLASS* GetCodeElementByDefinedTerm(const string& term);
		CODE_ELEMENT_CLASS* GetCodeElementByEscSequence(BYTE* esc_sq_ptr, UINT maxLength);

		bool GetMultiByte();
		bool GetCodeExtensions();

	private:
		string descriptionM;   // character set description
		bool multi_byteM;      // indicates multi-byte or not
		bool code_extensionsM; // indicates code extensions allowed
		vector<CODE_ELEMENT_CLASS*> code_elementsM;
};


//>>***************************************************************************

class CHARACTER_SET_REGISTER_CLASS

//  DESCRIPTION     : Character Set Register Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		static CHARACTER_SET_REGISTER_CLASS *Instance();

		void Cleanup();

		bool LoadCharacterSets(string filename);

		void AddCharacterSet(CHARACTER_SET_CLASS* cs_ptr);

        DVT_STATUS IsValidExtendedChar(BYTE*, UINT, ATTR_VR_ENUM, UINT, LOG_MESSAGE_CLASS*, SPECIFIC_CHARACTER_SET_CLASS*);

	protected:
		CHARACTER_SET_REGISTER_CLASS();

	private:
		static CHARACTER_SET_REGISTER_CLASS *instanceM_ptr; // Singleton

		// character set definition
        bool characterSetDataLoadedM;
		vector<CHARACTER_SET_CLASS*> character_setsM;

		int GetNrCharacterSets();
		CHARACTER_SET_CLASS* GetCharacterSet(int i);

		CODE_ELEMENT_CLASS* GetCodeElementByDefinedTerm(const string& term);
		CODE_ELEMENT_CLASS* GetCodeElementByEscSequence(BYTE* esc_sq_ptr, UINT maxLength);
};

#endif /* EXT_CHAR_SET_DEFINITION_H */
