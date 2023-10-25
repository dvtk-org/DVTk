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

//*****************************************************************************
//  FILENAME        :	DCM_DIR_DATASET.H
//  PACKAGE         :	DVT
//  COMPONENT       :	DICOM
//  DESCRIPTION     :	DICOM Dataset Class.
//  COPYRIGHT(c)    :   2004, Philips Electronics N.V.
//                      2004, Agfa Gevaert N.V.
//*****************************************************************************
#ifndef DCM_DIR_DATASET_H
#define DCM_DIR_DATASET_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Idicom.h"     // DICOM library interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************


//>>***************************************************************************

class DCM_DIR_DATASET_CLASS : public DCM_DIR_DATASET_CLASS

//  DESCRIPTION     : DICOM dataset class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	DIMSE_CMD_ENUM	commandIdM;
	string			iodNameM;
	PRIVATE_ATTRIBUTE_HANDLER_CLASS	pahM;

	void populateWithAttributes();

public:
	DCM_DIR_DATASET_CLASS();
	~DCM_DIR_DATASET_CLASS();

	bool decode(DATA_TF_CLASS&, UINT16 lastGroup = 0xFFFF);
};

#endif /* DCM_DIR_DATASET_H */

