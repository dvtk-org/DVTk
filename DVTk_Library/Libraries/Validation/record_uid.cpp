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
#include "record_uid.h"
#include "uid_defining.h"
#include "uid_referring.h"
#include "uid_ref.h"

#include "Idicom.h"         // Dicom component interface

//>>===========================================================================

RECORD_UID_CLASS::RECORD_UID_CLASS()

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

RECORD_UID_CLASS::~RECORD_UID_CLASS()

//  DESCRIPTION     : Class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (UINT i = 0; i < uidRefsM.size(); i++)
    {
        delete uidRefsM[i];
    }
    uidRefsM.clear();
}

//>>===========================================================================

bool RECORD_UID_CLASS::InstallDefiningUid(DCM_ATTRIBUTE_GROUP_CLASS *record_ptr,
                                      UID_DEFINING_CLASS *uidDef_ptr)

//  DESCRIPTION     : Install all defining instance UID/identifier
//                    combinations in case it's not installed already.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // Loop over all defining UIDs.
    // If there's an attribute with the UID tag, add the UID to the record
    // UID class. If the UID is already present, just add the defining
    // attribute to the respective record UID.
    for (int i = 0; i < uidDef_ptr->GetNrUids(); i++)
    {
        DCM_ATTRIBUTE_CLASS * attr_ptr = record_ptr->GetAttributeByTag(uidDef_ptr->GetUidStruct(i)->uidTag);
        if (attr_ptr != NULL)
        {
            BASE_VALUE_CLASS *value_ptr = attr_ptr->GetValue();
            if (value_ptr != NULL)
            {
				string uid;
                if (value_ptr->Get(uid) == MSG_OK)
                {
                    UINT j = 0;
                    UID_REF_CLASS *uidRef_ptr = NULL;

                    // Try to find a uid reference class in the record uid
                    while ((j < uidRefsM.size()) &&
						(uidRef_ptr == NULL))
                    {
                        if (uidRefsM[j]->GetUid() == uid)
                        {
                            uidRef_ptr = uidRefsM[j];
                        }
                        j++;
                    }
                    if (uidRef_ptr == NULL)
                    {
                        uidRef_ptr = new UID_REF_CLASS();
                        uidRef_ptr->SetUid(uid);
                        uidRefsM.push_back(uidRef_ptr);
                    }

                    ENTITY_NAME_STRUCT *entityName_ptr = new ENTITY_NAME_STRUCT();
                    entityName_ptr->tag = uidDef_ptr->GetUidStruct(i)->uidTag;
                    entityName_ptr->iodType = uidDef_ptr->GetUidStruct(i)->uidType;
                    entityName_ptr->name = record_ptr->GetName();
                    uidRef_ptr->AddDefining(entityName_ptr);
                }
            }
        }
    }

    return true;
}

//>>===========================================================================

bool RECORD_UID_CLASS::InstallReferringUid(DCM_ATTRIBUTE_GROUP_CLASS *record_ptr,
                                       UID_REFERRING_CLASS *uidRef_ptr)

//  DESCRIPTION     : Install all referring instance UID/identifier
//                    combinations in case it's not installed already.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (int i = 0; i < uidRef_ptr->GetNrUids(); i++)
    {
        UINT32 uidTag = uidRef_ptr->GetUidStruct(i)->uidTag;
        if (uidRef_ptr->GetUidStruct(i)->sequenceTag == TAG_UNDEFINED)
        {
            DCM_ATTRIBUTE_CLASS *attr_ptr = record_ptr->GetAttributeByTag(uidTag);
            if (attr_ptr != NULL)
            {
                BASE_VALUE_CLASS *value_ptr = attr_ptr->GetValue ();
                if (value_ptr != NULL)
                {
					string uid;
                    if (value_ptr->Get(uid) == MSG_OK)
                    {
                        StoreReferringUid(uid, uidRef_ptr->GetUidStruct(i), record_ptr->GetName());
                    }
                }
            }
        }
        else
        {
            // We're dealing with a SQ attribute. So we need to loop over
            // all items in the attribute.
            DCM_ATTRIBUTE_CLASS *attr_ptr = record_ptr->GetAttributeByTag(uidRef_ptr->GetUidStruct(i)->sequenceTag);
            if (attr_ptr != NULL)
            {
                VALUE_SQ_CLASS  *sqValue_ptr = static_cast<VALUE_SQ_CLASS*>(attr_ptr->GetValue());
                if (sqValue_ptr != NULL)
                {
                    for (int j = 0; j < sqValue_ptr->GetNrItems(); j++)
                    {
                        ATTRIBUTE_GROUP_CLASS *item_ptr;
                        if (sqValue_ptr->Get(&item_ptr, j) == MSG_OK)
                        {
                            ATTRIBUTE_CLASS *itemAttr_ptr = item_ptr->GetAttributeByTag(uidTag);
                            if (itemAttr_ptr != NULL)
                            {
                                BASE_VALUE_CLASS *value_ptr = itemAttr_ptr->GetValue();
                                if (value_ptr != NULL)
                                {
 									string uid;
									if (value_ptr->Get(uid) == MSG_OK)
                                    {
                                        StoreReferringUid(uid, uidRef_ptr->GetUidStruct(i), record_ptr->GetName());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Check for UID references in nested objects.
    for (int i = 0; i < record_ptr->GetNrAttributes(); i++)
    {
        DCM_ATTRIBUTE_CLASS *attr_ptr = record_ptr->GetAttribute(i);
        if (attr_ptr != NULL)
        {
            if (attr_ptr->GetVR() == ATTR_VR_SQ)
            {
#ifdef NDEBUG
                DCM_VALUE_SQ_CLASS  *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attr_ptr->GetValue());
#else
                DCM_VALUE_SQ_CLASS  *sqValue_ptr = dynamic_cast<DCM_VALUE_SQ_CLASS*>(attr_ptr->GetValue());
#endif

                if (sqValue_ptr != NULL)
                {
                    for (int j = 0; j < sqValue_ptr->GetNrItems(); j++)
                    {
                        DCM_ATTRIBUTE_GROUP_CLASS *item_ptr = sqValue_ptr->getItem(j);
                        if (item_ptr != NULL)
						{
                            InstallReferringUid(item_ptr, uidRef_ptr);
						}
                    }
                }
            }
        }
    }
    return true;
}

//>>===========================================================================

void RECORD_UID_CLASS::Check (LOG_MESSAGE_CLASS *messages_ptr)

//  DESCRIPTION     : Check the log messages.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (UINT i = 0; i < uidRefsM.size(); i++)
    {
        uidRefsM[i]->Check(messages_ptr);
    }
}

//>>===========================================================================

void RECORD_UID_CLASS::StoreReferringUid(string uid,
                                     UID_REFERENCE_STRUCT *uidRefStruct_ptr,
                                     string identifier)

//  DESCRIPTION     : Install all referring instance UID/identifier
//                    combinations in case it's not installed already.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    UINT i = 0;
    UID_REF_CLASS *uidRef_ptr = NULL;

    /* Try to find a uid reference class in the record uid. */
    while ((i < uidRefsM.size()) && 
        (uidRef_ptr == NULL))
    {
        if (uidRefsM[i]->GetUid() == uid)
        {
            uidRef_ptr = uidRefsM[i];
        }
        i++;
    }

    if (uidRef_ptr == NULL)
    {
        uidRef_ptr = new UID_REF_CLASS();
        uidRef_ptr->SetUid(uid);
        uidRefsM.push_back(uidRef_ptr);
    }

    ENTITY_NAME_STRUCT *entityName_ptr = new ENTITY_NAME_STRUCT();
    entityName_ptr->tag = uidRefStruct_ptr->uidTag;
    entityName_ptr->iodType = uidRefStruct_ptr->uidType;
    entityName_ptr->name = identifier;
    uidRef_ptr->AddReferring(entityName_ptr);
}
