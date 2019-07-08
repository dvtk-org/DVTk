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

#ifndef ACSE_PROPERTIES_HPP
#define ACSE_PROPERTIES_HPP

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "IGlobal.h"

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

//>>***************************************************************************
class ACSE_PROPERTIES_CLASS
//  DESCRIPTION     : Class for ACSE parameter validation.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ACSE_PROPERTIES_CLASS();
	~ACSE_PROPERTIES_CLASS();
	
	void	setCalledAeTitle (const string calledAeTitle);
	string	getCalledAeTitle (void);
	
	void	setCallingAeTitle (const string callingAeTitle);
	string	getCallingAeTitle (void);
	
	void	setMaximumLengthReceived (UINT32 maximumLengthReceived);
	UINT32	getMaximumLengthReceived (void);
	
	void	setImplementationClassUid (const string implementationClassUid);
	string	getImplementationClassUid (void);
	
	void	setImplementationVersionName (const string implementationVersionName);
	string	getImplementationVersionName (void);
	
protected:
	
private:
	string		calledAeTitleM;
	string		callingAeTitleM;
	UINT32		maximumLengthReceivedM;
	string		implementationClassUidM;
	string		implementationVersionNameM;
};

#endif /* ACSE_PROPERTIES_HPP */
