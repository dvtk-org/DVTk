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

#ifndef MEDIA_VALIDATION_FILTER_H
#define MEDIA_VALIDATION_FILTER_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

//>>***************************************************************************

class MEDIA_VALIDATION_FILTER_CLASS

//  DESCRIPTION     : Media Validation Filter Class
//  NOTES           :
//<<***************************************************************************
{
    public:
		MEDIA_VALIDATION_FILTER_CLASS(string, int);
		~MEDIA_VALIDATION_FILTER_CLASS();

        bool ShouldFilterOut(string);

    private:
		string filterDirectoryRecordTypeM;
		int	maximumRecordsToValidateM;
		int recordsValidatedM;
};

#endif /* MEDIA_VALIDATION_FILTER_H */
