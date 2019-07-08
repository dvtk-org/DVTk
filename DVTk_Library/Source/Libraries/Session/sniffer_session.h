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

// Media Test Session class.

#ifndef SNIFFER_SESSION_H
#define SNIFFER_SESSION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "session.h"			// Base Session include
#include "Iglobal.h"			// Global component interface
#include "Inetwork.h"			// Network component interface

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_COMMAND_CLASS;
class DCM_DATASET_CLASS;

//>>***************************************************************************

class SNIFFER_SESSION_CLASS : public BASE_SESSION_CLASS

    //  DESCRIPTION     : Media Test Session Class.
    //  INVARIANT       :
    //  NOTES           :
    //<<***************************************************************************
{
public:
    SNIFFER_SESSION_CLASS();

    ~SNIFFER_SESSION_CLASS();

    bool serialise(FILE*);

    bool validate(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*, VALIDATION_CONTROL_FLAG_ENUM, AE_SESSION_CLASS*);
    bool validate(ASSOCIATE_AC_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(ASSOCIATE_RJ_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(ASSOCIATE_RQ_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(RELEASE_RP_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(RELEASE_RQ_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(ABORT_RQ_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);

	void readPduFiles(vector<string>*);

	bool receive(RECEIVE_MESSAGE_UNION_CLASS**);

private:
    // DVT ACSE Properties
    string							dvtAeTitleM;
    
    // SUT ACSE Properties
    string							sutAeTitleM;
    UINT32							sutMaximumLengthReceivedM;
    string							sutImplementationClassUidM;
    string							sutImplementationVersionNameM;

	SNIFFER_PDUS_CLASS				sniffPdusM;
};

#endif /* SNIFFER_SESSION_H */


