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
#include "MetaSopClassDefinition.h"


//>>===========================================================================

DEF_METASOPCLASS_CLASS::DEF_METASOPCLASS_CLASS(const string UID, const string Name)

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	uidM = UID;
	nameM = Name;
}

//>>===========================================================================

DEF_METASOPCLASS_CLASS::~DEF_METASOPCLASS_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	sopClassesM.clear();
}

//>>===========================================================================

void DEF_METASOPCLASS_CLASS::AddSopClass(const string uid, const string name)

//  DESCRIPTION     : Function to add a SOP Class to the Meta SOP Class list
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// add SOP Class definition to Meta SOP Class
	sopClassesM[uid] = name;
}

//>>===========================================================================

bool DEF_METASOPCLASS_CLASS::HasSopClass(const string uid)

//  DESCRIPTION     : Return true if the metasopclass definition contains
//                    a sopclass with the given uid
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // check if we have already been given the meta sop class uid	
    if (uid == uidM) return true;

    bool result = false;

	METASOPCLASS_MAP::iterator sopClass_it = sopClassesM.find(uid);
	if (sopClass_it != sopClassesM.end())
	{
        result = true;
	}

	return result;
}

//>>===========================================================================

UINT DEF_METASOPCLASS_CLASS::GetNoSopClasses()

//  DESCRIPTION     : Returns the number of Sop Classes in the MetaSopClass
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return the size of the map
	return sopClassesM.size();
}

//>>===========================================================================

void DEF_METASOPCLASS_CLASS::GetSopClass(UINT index, string& sopClassUid, string& sopClassName)

//  DESCRIPTION     : Get indexed SopClassUid and Name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT i = 0;
	METASOPCLASS_MAP::iterator sopClass_it = sopClassesM.begin();
	while (sopClass_it != sopClassesM.end() && i < index)
	{
		// increment i
		i++;
	}
	sopClassUid = (*sopClass_it).first;
	sopClassName = (*sopClass_it).second;
}

//>>===========================================================================

string DEF_METASOPCLASS_CLASS::GetImageBoxSopUid()

//  DESCRIPTION     : Returns the uid of the Image Box Sop Class
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string uid;
	char box_uid[UI_LENGTH];
    bool found = false;

	METASOPCLASS_MAP::iterator sopClass_it = sopClassesM.begin();

	while (sopClass_it != sopClassesM.end() && !found)
	{
        strcpy(box_uid, ((*sopClass_it).first).c_str());

        if (strncmp(box_uid, ABSTRACT_IMAGE_BOX_SOP_CLASS_UID_STEM, 
			        strlen(ABSTRACT_IMAGE_BOX_SOP_CLASS_UID_STEM)) == 0)
		{
            found = true;
			uid = box_uid;
		}
		++sopClass_it;
	}

	return uid;
}

//>>===========================================================================

string DEF_METASOPCLASS_CLASS::GetImageBoxSopName()

//  DESCRIPTION     : Returns the name of the Image Box Sop Class
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string name;
	char box_uid[UI_LENGTH];
    bool found = false;

	METASOPCLASS_MAP::iterator sopClass_it = sopClassesM.begin();

	while (sopClass_it != sopClassesM.end() && !found)
	{
        strcpy(box_uid, ((*sopClass_it).first).c_str());

        if (strncmp(box_uid, ABSTRACT_IMAGE_BOX_SOP_CLASS_UID_STEM, 
			        strlen(ABSTRACT_IMAGE_BOX_SOP_CLASS_UID_STEM)) == 0)
		{
            found = true;
			name = (*sopClass_it).second;
		}
		++sopClass_it;
	}

	return name;
}

