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

#ifndef ACSE_ABORT_REQUEST_VALIDATOR_H
#define ACSE_ABORT_REQUEST_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_ab_reason.h"
#include "ACSE_ab_source.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class ABORT_RQ_CLASS;
class LOG_CLASS;

//>>***************************************************************************
class ABORT_RQ_VALIDATOR_CLASS
//  DESCRIPTION     : Abort Request validation class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ABORT_RQ_VALIDATOR_CLASS();
	~ABORT_RQ_VALIDATOR_CLASS();

	ACSE_PARAMETER_CLASS *getSourceParameter();
	ACSE_PARAMETER_CLASS *getReasonParameter();
	
	bool    validate(ABORT_RQ_CLASS*, ABORT_RQ_CLASS*);
	
private:
	ACSE_AB_SOURCE_CLASS    sourceM;
	ACSE_AB_REASON_CLASS    reasonM;
};

#endif /* ACSE_ABORT_REQUEST_VALIDATOR_H */