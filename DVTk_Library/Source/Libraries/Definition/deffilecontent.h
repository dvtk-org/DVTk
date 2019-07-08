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

#ifndef DEFFILECONTENT_H
#define DEFFILECONTENT_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEF_METASOPCLASS_CLASS;
class DEF_SOPCLASS_CLASS;
class DEF_COMMAND_CLASS;
class DEF_DATASET_CLASS;
class DEF_ATTRIBUTE_CLASS;
class DEF_ITEM_CLASS;
class DEF_MACRO_CLASS;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************


//>>***************************************************************************

class DEF_FILE_CONTENT_CLASS

//  DESCRIPTION     : Definition File Content Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_FILE_CONTENT_CLASS();
	~DEF_FILE_CONTENT_CLASS();

	void SetSystem(const string, const string);

	void SetAE(const string, const string);

	void SetMetaSop(DEF_METASOPCLASS_CLASS*);

	void AddSopClass(DEF_SOPCLASS_CLASS*);

	void AddCommand(DEF_COMMAND_CLASS*);

	bool Register();

	bool Unregister();

	string GetSystemName();

	string GetSystemVersion();

	string GetAEName();

	string GetAEVersion();

	string GetSOPClassName();

	string GetSOPClassUID();

	bool IsMetaSOPClass();

	DEF_METASOPCLASS_CLASS* GetMetaSop();

	DEF_METASOPCLASS_CLASS* GetMetaSopByUid(const string);

	DEF_SOPCLASS_CLASS* GetSopByUid(const string);
    
	DEF_SOPCLASS_CLASS* GetSopByName(const string);
	
	DEF_SOPCLASS_CLASS* GetFirstSop();

	DEF_DATASET_CLASS* GetDataset(const string);
	DEF_DATASET_CLASS* GetDataset(DIMSE_CMD_ENUM, const string);

private:
	string							systemNameM;
	string							systemVersionM;
	string							aeNameM;
	string							aeVersionM;
	DEF_METASOPCLASS_CLASS*			metaSopClassM_ptr;
	vector<DEF_SOPCLASS_CLASS*>		sopClassM;
	vector<DEF_COMMAND_CLASS*>		commandM;

	void RegisterIod(DEF_SOPCLASS_CLASS*, bool);

	void RegisterMacroInMacro(DEF_MACRO_CLASS*, bool);

	void RegisterAttributesInSequence(DEF_ATTRIBUTE_CLASS*, bool);

	void RegisterAttributesInItem(DEF_ITEM_CLASS*, bool);

	bool IsPrivateAttribute(UINT16, UINT16);
};

#endif /* DEFFILECONTENT_H */

