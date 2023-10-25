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
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "sop_instance_data.h"
#include "Ilog.h"			       // Log component interface


//>>===========================================================================

SOP_INSTANCE_DATA_CLASS::SOP_INSTANCE_DATA_CLASS(string uid, string forUid, string itv3, string riUid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// - initialise class elements
	instanceUidM = uid;

	// optional Frame Of Reference
	if (forUid.length()) 
	{
		frameOfReferenceUidM = forUid;
	}

	// optional Image Type Value 3
	if (itv3.length()) 
	{
		imageTypeValue3M = itv3;
	}

	// optional Referenced Image SOP Instance UID
	if (riUid.length()) 
	{
		refImageInstanceUidM = riUid;
	}

	countM = 1;
}

//>>===========================================================================

SOP_INSTANCE_DATA_CLASS::~SOP_INSTANCE_DATA_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup
}

//>>===========================================================================

void SOP_INSTANCE_DATA_CLASS::log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log SOP Instance data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid logger
	if (logger_ptr == NULL)
	{
		return;
	}

	// save old log level - and set new
	UINT32 oldLogLevel = logger_ptr->logLevel(LOG_IMAGE_RELATION);

	// display object data
	logger_ptr->text(LOG_NONE, 1, "\t\t\t\t(0008,0018) Object(Image) SOP Instance UID: %s", instanceUidM.c_str());

	if (frameOfReferenceUidM.length()) 
	{
		logger_ptr->text(LOG_NONE, 1, "\t\t\t\t(0020,0052) Frame Of Reference UID: %s", frameOfReferenceUidM.c_str());
	}
	
	if (imageTypeValue3M.length()) 
	{
		logger_ptr->text(LOG_NONE, 1, "\t\t\t\t(0008,0008) Image Type Third Value: %s", imageTypeValue3M.c_str());
	}

	if (refImageInstanceUidM.length()) 
	{
		logger_ptr->text(LOG_NONE, 1, "\t\t\t\t(0008,1140) Referenced Image Sequence");
		logger_ptr->text(LOG_NONE, 1, "\t\t\t\t>(0008,1155) Referenced SOP Instance UID: %s", refImageInstanceUidM.c_str());
	}
	
	if (countM > 1) 
	{
		logger_ptr->text(1, "%d objects with identical identification", countM);
	}

	// restore original log level
	logger_ptr->logLevel(oldLogLevel);
}
