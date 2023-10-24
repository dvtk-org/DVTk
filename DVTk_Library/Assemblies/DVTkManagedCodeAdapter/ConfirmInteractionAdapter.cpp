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
#include "ConfirmInteractionAdapter.h"
#include < vcclr.h >
#using <mscorlib.dll>

namespace Wrappers
{
    // ctor
    ConfirmInteractionAdapter::ConfirmInteractionAdapter(IConfirmInteractionTarget^ value)
    {
        m_pConfirmInteractionTarget = value;
    }
    // dtor
    ConfirmInteractionAdapter::~ConfirmInteractionAdapter(void)
    {}

    void ConfirmInteractionAdapter::ConfirmInteraction()
    {
        // Translation from unmanaged -> managed CLR
        m_pConfirmInteractionTarget->ConfirmInteraction();
    } 
}