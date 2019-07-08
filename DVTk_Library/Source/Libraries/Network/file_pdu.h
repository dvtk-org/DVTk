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
//  DESCRIPTION     :	File PDU class.
//*****************************************************************************
#ifndef FILE_PDU_H
#define FILE_PDU_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Iutility.h"		// Utility component interface

#include "base_io.h"		// Base IO Class


//>>***************************************************************************

class FILE_PDU_CLASS

//  DESCRIPTION     : Class handling a file containing a single PDU.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	FILE	*fileM_ptr;
	string	filenameM;

public:
	FILE_PDU_CLASS();
	FILE_PDU_CLASS(string filename);

	~FILE_PDU_CLASS();

	bool open(string);

	void setFilename(string pduFile){filenameM = pduFile;}

	string getFilename(){return filenameM;}

	bool isOpen() 
		{ return (fileM_ptr == NULL) ? false : true; }

	void close();

	bool write(const BYTE*, UINT);
		
	INT	read(BYTE*, UINT);

	UINT getLength();

	UINT getOffset();
};

//>>***************************************************************************

class FILE_STREAM_CLASS : public BASE_IO_CLASS

//  DESCRIPTION     : Class for reading PDU data from a File Stream.
//					: Each file in the stream contains 1 PDU.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	ARRAY<FILE_PDU_CLASS>	filePduM;
	LOG_CLASS				*loggerM_ptr;
	UINT32					logLengthM;
	UINT					filePDUIndexM;

public:
	FILE_STREAM_CLASS();

	~FILE_STREAM_CLASS();

	void addFileToStream(string filename)
		{
			FILE_PDU_CLASS	filePdu(filename);
			filePduM.add(filePdu);
		}

	virtual	bool writeBinary(const BYTE*, UINT);
		
	virtual	INT	readBinary(BYTE*, UINT);

	void setLogger(LOG_CLASS *logger_ptr)
		{ loggerM_ptr = logger_ptr; }

	void setLogLength(UINT32 length)
		{ logLengthM = length; }

	LOG_CLASS *getLogger() { return loggerM_ptr; }

	UINT getNrOfPDUs() { return filePduM.getSize(); }

	UINT getCurrentPDUFileIndex() { return filePDUIndexM; }
};

#endif /* FILE_PDU_H */

