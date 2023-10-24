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

#include "StdAfx.h"
#include ".\MAttributeUtilities.h"
#include "MDIMSEConvertors.h"
#using <mscorlib.dll>

namespace Wrappers
{
	MAttributeUtilities::MAttributeUtilities(void)
	{
	}

	MAttributeUtilities::~MAttributeUtilities(void)
	{
	}

	bool MAttributeUtilities::ComparePixelAttributes(
		DvtkData::Dimse::Attribute^ pSourceAttr,
		DvtkData::Dimse::Attribute^ pRefAttr)
	{
		DCM_ATTRIBUTE_CLASS *pUMAttribute = nullptr;
        DCM_ATTRIBUTE_CLASS *pUMReferenceAttribute = nullptr;
        if ((pSourceAttr == nullptr) || (pRefAttr == nullptr))
			throw gcnew System::ArgumentNullException();
        
        pUMAttribute =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pSourceAttr);
        pUMReferenceAttribute =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pRefAttr);
        
        bool success = true;
		DVT_STATUS dvtStatus = ::MSG_OK;
        if ((pUMAttribute != nullptr) || (pUMReferenceAttribute != nullptr))
        {
            dvtStatus = pUMAttribute->Compare( nullptr, pUMReferenceAttribute);
        }
		if(dvtStatus != ::MSG_OK)
			success = false;
        return success;
	}
}