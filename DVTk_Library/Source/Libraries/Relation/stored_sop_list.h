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

#ifndef STORED_SOP_LIST_H
#define STORED_SOP_LIST_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_DATASET_CLASS;
class LOG_CLASS;
class STORED_SOP_INSTANCE_CLASS;


//>>***************************************************************************

class STORED_SOP_LIST_CLASS

//  DESCRIPTION     : Class used to store the sop class/instance detail.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	ARRAY<STORED_SOP_INSTANCE_CLASS*>	sopInstanceM;

public:
	STORED_SOP_LIST_CLASS();
	
	~STORED_SOP_LIST_CLASS();

	void						cleanup			(void);

	STORED_SOP_INSTANCE_CLASS *	search			(string			  sopClassUid,
												 string			  sopInstanceUid,
												 LOG_CLASS		* logger_ptr = NULL);

	UINT16				analyseStorageDataset	(DCM_DATASET_CLASS	* dataset_ptr,
												 string&		  msg,
												 LOG_CLASS		* logger_ptr,
												 bool isAccept = false);

	void						log				(LOG_CLASS		* logger_ptr);
};

#endif /* STORED_SOP_LIST_H */
