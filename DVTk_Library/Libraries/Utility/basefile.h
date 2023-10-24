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

#ifndef BASEFILE_H
#define BASEFILE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// C Header Files / Base Data Templates


//>>***************************************************************************

class BASE_FILE_CLASS

//  DESCRIPTION     : Abstract Base Class for various "Script" Files.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	string	pathnameM;
	string	filenameM;
	string	absoluteFilenameM;
	FILE	*fdM_ptr;
	bool	onlyParseFileM;
	UINT    nr_errorsM;
	UINT    nr_warningsM;
		
public:
	virtual ~BASE_FILE_CLASS() = 0;

	const char *getFilename() 
		{ return filenameM.c_str(); }

	const char *getAbsoluteFilename();

	bool isFullPathname();

	void setOnlyParseFile(bool onlyParseFile)
		{ onlyParseFileM = onlyParseFile; }

	void setNrErrors(UINT nr)   { nr_errorsM = nr; }
	void setNrWarnings(UINT nr) { nr_warningsM = nr; }
	UINT getNrErrors()   { return nr_errorsM; }
	UINT getNrWarnings() { return nr_warningsM; }

	virtual bool open(bool forReading = true);

	virtual bool execute() = 0;

	virtual bool save();

	virtual void close();
};

#endif /* BASEFILE_H */


