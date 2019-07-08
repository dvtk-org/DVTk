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
//  DESCRIPTION     :   SQ value results include file for validation.
//*****************************************************************************
#ifndef VAL_VALUE_SQ_H
#define VAL_VALUE_SQ_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"
#include "val_base_value.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class VAL_ATTRIBUTE_GROUP_CLASS;
class SPECIFIC_CHARACTER_SET_CLASS;

//>>***************************************************************************

class VAL_VALUE_SQ_CLASS : public VAL_BASE_VALUE_CLASS

//  DESCRIPTION     : Validation SQ Value Class
//  NOTES           :
//<<***************************************************************************
{
    public:
        VAL_VALUE_SQ_CLASS();
        virtual ~VAL_VALUE_SQ_CLASS();

        void AddValItem(VAL_ATTRIBUTE_GROUP_CLASS*);
        VAL_ATTRIBUTE_GROUP_CLASS *GetValItem(int);
        int GetNrValItems();

        DVT_STATUS ValidateAgainstDef(UINT32);
        DVT_STATUS ValidateAgainstRef(UINT32);

        DVT_STATUS Check (UINT32, SPECIFIC_CHARACTER_SET_CLASS*);

    private:
        vector<VAL_ATTRIBUTE_GROUP_CLASS*> valItemsM;
};

#endif /* VAL_VALUE_SQ_H */