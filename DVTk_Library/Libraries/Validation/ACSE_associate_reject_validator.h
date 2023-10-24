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

#ifndef ACSE_ASSOCIATE_REJECT_VALIDATOR_H
#define ACSE_ASSOCIATE_REJECT_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_rj_reason.h"
#include "ACSE_rj_result.h"
#include "ACSE_rj_source.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class ASSOCIATE_RJ_CLASS;
class LOG_CLASS;

//>>***************************************************************************
class ASSOCIATE_RJ_VALIDATOR_CLASS
//  DESCRIPTION     : Associate Reject validation class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ASSOCIATE_RJ_VALIDATOR_CLASS();
	~ASSOCIATE_RJ_VALIDATOR_CLASS();
	
	ACSE_PARAMETER_CLASS *getResultParameter();
	ACSE_PARAMETER_CLASS *getSourceParameter();
	ACSE_PARAMETER_CLASS *getReasonParameter();

	bool validate(ASSOCIATE_RJ_CLASS*, ASSOCIATE_RJ_CLASS*);

private:
	ACSE_RJ_RESULT_CLASS	resultM;
	ACSE_RJ_SOURCE_CLASS	sourceM;
	ACSE_RJ_REASON_CLASS	reasonM;
};

#endif /* ACSE_ASSOCIATE_REJECT_VALIDATOR_H */