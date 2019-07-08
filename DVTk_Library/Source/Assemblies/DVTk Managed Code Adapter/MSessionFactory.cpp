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
#include "MSessionFactory.h"
#include "MEmulatorSession.h"
#include "MMediaSession.h"
#include "MScriptSession.h"
#include "MSnifferSession.h"
#include < vcclr.h >
#using <mscorlib.dll>

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;
    MBaseSession __gc* MSessionFactory::Load (System::String __gc* sessionFileName)
    {
        ABSTRACT_SESSION_CLASS  abstractSession;
        bool definitionFileLoaded = true;
        MBaseSession __gc* session = NULL;
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(sessionFileName);
        bool sessionLoaded = abstractSession.load (pAnsiString, definitionFileLoaded, false);
        Marshal::FreeHGlobal(pAnsiString);

        if (!sessionLoaded) 
            throw new System::ApplicationException(
            System::String::Format("Could not load session {0}.", sessionFileName)
            );

        switch (abstractSession.getSessionType())
        {
        case ST_SCRIPT:
            session = new MScriptSession ();
            break;
        case ST_EMULATOR:
            session = new MEmulatorSession ();
            break;
        case ST_MEDIA:
            session = new MMediaSession ();
            break;
		case ST_SNIFFER:
            session = new MSnifferSession ();
            break;
        default:
            // Unknown session type
            throw new System::NotImplementedException();
        }
        session->Load (sessionFileName, definitionFileLoaded, true);
        return session;
    }
}