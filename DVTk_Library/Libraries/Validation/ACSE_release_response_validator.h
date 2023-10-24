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

#ifndef ACSE_RELEASE_RESPONSE_VALIDATOR_H
#define ACSE_RELEASE_RESPONSE_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class RELEASE_RP_CLASS;
class LOG_CLASS;

//>>***************************************************************************
class RELEASE_RP_VALIDATOR_CLASS
//  DESCRIPTION     : Release Response validation class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	RELEASE_RP_VALIDATOR_CLASS();
	~RELEASE_RP_VALIDATOR_CLASS();
	
	bool validate(RELEASE_RP_CLASS*, RELEASE_RP_CLASS*);
};

#endif /* ACSE_RELEASE_RESPONSE_VALIDATOR_H */