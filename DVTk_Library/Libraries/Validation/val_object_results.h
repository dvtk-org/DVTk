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

#ifndef VAL_OBJECT_RESULTS_H
#define VAL_OBJECT_RESULTS_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_COMMAND_CLASS;
class LOG_MESSAGE_CLASS;
class VAL_ATTRIBUTE_GROUP_CLASS;
class VAL_ATTRIBUTE_CLASS;
class SPECIFIC_CHARACTER_SET_CLASS;
class DCM_ATTRIBUTE_GROUP_CLASS;

//>>***************************************************************************

class VAL_OBJECT_RESULTS_CLASS

//  DESCRIPTION     : Validation Object Results Class
//  NOTES           :
//<<***************************************************************************
{
    public:
        VAL_OBJECT_RESULTS_CLASS();
        virtual ~VAL_OBJECT_RESULTS_CLASS();

        DVT_STATUS AddModuleResults(VAL_ATTRIBUTE_GROUP_CLASS*);
        int GetNrModuleResults(void);
        VAL_ATTRIBUTE_GROUP_CLASS *GetModuleResults(int);
        VAL_ATTRIBUTE_CLASS *GetAttributeResults(UINT16, UINT16);
        VAL_ATTRIBUTE_CLASS *GetAttributeResults(UINT32);
		string GetSpecificCharacterSetValues();
        bool GetListOfAttributeResults(UINT16, UINT16, vector <VAL_ATTRIBUTE_CLASS*>*);

        VAL_ATTRIBUTE_GROUP_CLASS *GetAGWithAttributeInGroup(UINT16);
        VAL_ATTRIBUTE_GROUP_CLASS *GetAdditionalAttributeGroup();

        void SetName(string);
        string GetName();

		void SetDICOMDIRName(string);
        string GetDICOMDIRName();

        void SetCommand(DCM_COMMAND_CLASS*);
        DCM_COMMAND_CLASS *GetCommand();

		void SetFmi(DCM_ATTRIBUTE_GROUP_CLASS*);
		DCM_ATTRIBUTE_GROUP_CLASS *GetFmi();

        DVT_STATUS ValidateAgainstDef(UINT32);
        DVT_STATUS ValidateAgainstRef(UINT32);
        DVT_STATUS ValidateVR(UINT32, SPECIFIC_CHARACTER_SET_CLASS*);
        void HasReferenceObject(bool);

        LOG_MESSAGE_CLASS *GetMessages();
        bool HasMessages();

        void CleanUp();

    private:
        vector<VAL_ATTRIBUTE_GROUP_CLASS*> modulesM;
        VAL_ATTRIBUTE_GROUP_CLASS *additionalAttributesM_ptr;
        DCM_COMMAND_CLASS *commandM_ptr;
		DCM_ATTRIBUTE_GROUP_CLASS *fileMetaInfoM_ptr;
        LOG_MESSAGE_CLASS *messagesM_ptr;
		string nameM;
		string dicomdirNameM;
        bool hasReferenceObjectM;
};

#endif /* VAL_OBJECT_RESULTS_H */