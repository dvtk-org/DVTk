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

#ifndef MEDIA_SESSION_H
#define MEDIA_SESSION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "session.h"			// Base Session include

//*****************************************************************************
//  FORWARD DEFINITIONS
//*****************************************************************************
class MEDIA_VALIDATION_FILTER_CLASS;

//>>***************************************************************************

class MEDIA_SESSION_CLASS : public BASE_SESSION_CLASS

    //  DESCRIPTION     : Media Test Session Class.
    //  INVARIANT       :
    //  NOTES           :
    //<<***************************************************************************
{
private:
    // Test Properties
    bool	validateReferencedFileM;

public:
    MEDIA_SESSION_CLASS();

    ~MEDIA_SESSION_CLASS();

    bool serialise(FILE*);

    bool beginMediaValidation();

    bool validateMediaFile(string);

    bool validateMediaFile(string, MEDIA_FILE_CONTENT_TYPE_ENUM, string, string, string);

	bool validateMediaFile(string, string, int);

    bool endMediaValidation();

    bool validate(FILE_DATASET_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);

	bool generateDICOMDIR(vector<string>*);

	void setValidateReferencedFile(bool);

	bool getValidateReferencedFile();	

private:
	bool localValidateMediaFile(string, FILE_DATASET_CLASS*, MEDIA_VALIDATION_FILTER_CLASS*);

    int getNumberOfDirectoryRecords(string);
};

#endif /* MEDIA_SESSION_H */


