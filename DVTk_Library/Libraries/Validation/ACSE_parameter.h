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

#ifndef ACSE_PARAMETER_HPP
#define ACSE_PARAMETER_HPP

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"
#include "valdefinitions.h"


//>>***************************************************************************

class ACSE_PARAMETER_CLASS 

//  DESCRIPTION     : Base class for ACSE parameter validation.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	virtual ~ACSE_PARAMETER_CLASS() = 0;
		
	void setName(const string);
	
	void setValue(const string);
	void setValue(char*, ...);
	string getValue();
	
    void setMeaning(const string);
	string getMeaning();
	
	void addMessage(const string);
	void addMessage(const int, const string);
	void addMessage(char*, ...);
	void addMessage(const int, char*, ...);
	
	int noMessages();
    UINT32 getIndex(const int);
	UINT32 getCode(const int);
	string getMessage(const int);
		
	bool validate(string, string);
	
protected:
	string			nameM;
	string			valueM;
    string          meaningM;
	bool			quotedValueM;
	
	bool checkIntegerSyntax(int);
	
	bool checkStringDifferences(char*, char*, int, bool, bool);
	
	bool checkIntegerReference(string);
	
	virtual bool checkSyntax() = 0;
	
	virtual bool checkRange() = 0;
	
	virtual bool checkReference(string) = 0;
	
private:
	vector<CODE_MESSAGE_STRUCT> messagesM;
}; 

#endif /* ACSE_PARAMETER_HPP */
