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

#ifndef DCM_DIR_DATASET_H
#define DCM_DIR_DATASET_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Idicom.h"     // DICOM library interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_ITEM_CLASS;

//>>***************************************************************************

class DCM_DIR_DATASET_CLASS : public DCM_DATASET_CLASS

//  DESCRIPTION     : DICOM dataset class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
    DCM_VALUE_SQ_CLASS *sqValueM_ptr;

public:
    DCM_DIR_DATASET_CLASS();
	~DCM_DIR_DATASET_CLASS();

    bool decodeToFirstRecord(DATA_TF_CLASS&);

    DCM_ITEM_CLASS *getNextDirRecord(DATA_TF_CLASS&);
};

#endif /* DCM_DIR_DATASET_H */

