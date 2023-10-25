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

#ifndef ATTRIBUTE_GROUP_H
#define ATTRIBUTE_GROUP_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"	// Global component interface
#include "Ilog.h"       // Log component interface
#include <string>
#include <vector>


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class ATTRIBUTE_CLASS;
class SPECIFIC_CHARACTER_SET_CLASS;


//*****************************************************************************
//  Type definitions
//*****************************************************************************
typedef vector<ATTRIBUTE_CLASS *>  ATTRIBUTE_VECTOR;


class ATTRIBUTE_GROUP_CLASS  
{
public:
                                ATTRIBUTE_GROUP_CLASS();
    virtual                    ~ATTRIBUTE_GROUP_CLASS();

			void				DeleteAttributes();

            string              GetName (void);
            DVT_STATUS          SetName (string name);

            DVT_STATUS          AddAttribute (ATTRIBUTE_CLASS * attribute);

            ATTRIBUTE_CLASS   * GetAttribute (unsigned short group,
                                              unsigned short element,
											  bool parentOnly = false);
            ATTRIBUTE_CLASS   * GetMappedAttribute (unsigned short group,
                                                    unsigned short element,
													bool parentOnly = false);
            ATTRIBUTE_CLASS   * GetAttribute (int index);
            ATTRIBUTE_CLASS   * GetAttributeByTag (unsigned int tag);

            DVT_STATUS          DeleteAttribute (unsigned short group,
                                                 unsigned short element);
            DVT_STATUS          DeleteMappedAttribute (unsigned short group,
                                                       unsigned short element);
            DVT_STATUS          DeleteAttribute (int index);

            DVT_STATUS          DeleteAttributeIndex (unsigned short group,
                                                      unsigned short element);
            DVT_STATUS          DeleteMappedAttributeIndex (unsigned short group,
                                                      unsigned short element);
            DVT_STATUS          DeleteAttributeIndex (int index);

            int                 GetNrAttributes (void);

            bool                IsSorted (void);
            DVT_STATUS          SortAttributes();

            DVT_STATUS          Check (UINT32 flags,
                                       ATTRIBUTE_GROUP_CLASS * ref_ag,
                                       LOG_MESSAGE_CLASS *messages,
                                       SPECIFIC_CHARACTER_SET_CLASS *specific_character_set = NULL);

protected:
    ATTRIBUTE_VECTOR            attributesM;

private:
    string                      nameM;
};

#endif /* ATTRIBUTE_GROUP_H */
