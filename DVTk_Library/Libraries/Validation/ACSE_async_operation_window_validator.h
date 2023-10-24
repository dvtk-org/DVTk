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

#ifndef ACSE_ASYNC_OPERATION_WINDOW_VALIDATOR_H
#define ACSE_ASYNC_OPERATION_WINDOW_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_operation.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class USER_INFORMATION_CLASS;
class LOG_CLASS;

//>>***************************************************************************

class ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS

//  DESCRIPTION     : Asynchronous Operation Window class for ACSE parameter validation.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS();
	~ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS();
	
	ACSE_PARAMETER_CLASS *getOperationsInvokedParameter();
	ACSE_PARAMETER_CLASS *getOperationsPerformedParameter();

	bool validate(UINT16, UINT16, USER_INFORMATION_CLASS*);

private:
	ACSE_OPERATION_CLASS	operationsInvokedM;
	ACSE_OPERATION_CLASS	operationsPerformedM;
};

#endif /* ACSE_ASYNC_OPERATION_WINDOW_VALIDATOR_H */