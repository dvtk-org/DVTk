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

#ifndef OTHER_VALUE_STREAM_H
#define OTHER_VALUE_STREAM_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"				// Global component interface
#include "Ilog.h"					// Log component interface
#include "Iutility.h"				// Utility component interface
#include "Inetwork.h"				// Network component interface


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define MAX_OV_DATA				7
#define MAX_OV_BLOCK_LENGTH		0x80000		// 512kB

//
// tag values for the PIX header tags
//
#define VERSION_TAG				0x0000		// always 1st tag
#define OV_VR_TAG				0x0001		// always 2nd tag
#define OV_BITS_ALLOCATED_TAG	0x0002		// always 3rd tag
#define OV_TRANSFER_SYNTAX_TAG	0x0003		// always 4th tag
#define OV_SIZE_TAG				0x7FE0		// always the last tag

#define OV_HEADER_VERSION1		1			// VTS OV header version 1
#define OV_HEADER_VERSION2		2			// DVT OV header version 2
											// - added OV_VR_TAG & OV_BITS_ALLOCATED_TAG 
											// to version 1 format
#define OV_HEADER_VERSION3		3			// DVT OV header version 3
											// - added OV_TRANSFER_SYNTAX to version 2 format

#define OV_VR_OB				0x4242		// "BB" = OB VR
#define OV_VR_OF				0x4646		// "FF" = OF VR
#define OV_VR_OW				0x5757		// "WW" = OW VR

//
// other value data source 
//
enum OV_DATA_SOURCE_ENUM
{
	GENERATE, 
	INFILE 
};


//>>***************************************************************************

class OTHER_VALUE_STREAM_CLASS : protected FILE_TF_CLASS

//  DESCRIPTION     : Other base value stream class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	INT							file_versionM;
	OV_DATA_SOURCE_ENUM			data_sourceM;
	LOG_CLASS*					loggerM_ptr;

	// memory transfer syntax
	UID_CLASS					memory_transfer_syntaxM;
	TS_CODE						ms_codeM;

	// general ov data attributes
	UINT16						bits_allocatedM;
	UINT32						lengthM;

	// ov file data
	string						filenameM;
	UID_CLASS					file_transfer_syntaxM;
	TS_CODE						fs_codeM;
    ATTR_VR_ENUM                file_vrM;
	UINT32						file_offsetM;
	UINT32						length_offsetM;

	// ov pattern generation data
	UINT						rowsM;
	UINT						columnsM;
	UINT						start_valueM;
	UINT						rows_incrementM;
	UINT						columns_incrementM;
	UINT						rows_sameM;
	UINT						columns_sameM;

	// private methods
	void GenerateFilename();

	bool StreamInData(DATA_TF_CLASS& dataTransfer, UINT32 length);

	bool WriteFileHeader();

public:
	virtual ~OTHER_VALUE_STREAM_CLASS() = 0;

	void Initialise();

	virtual bool SetFilename(string filename);
	void SetPatternValues(UINT rows, 
						UINT columns, 
						UINT start_value, 
						UINT rows_increment, 
						UINT columns_increment, 
						UINT rows_same, 
						UINT columns_same);
	void SetMemoryTransferSyntax(UID_CLASS& transfer_syntax);
	void SetFileTransferSyntax(UID_CLASS& transfer_syntax);
	void SetBitsAllocated(UINT16 bits_allocated);
	void SetLength(UINT32 length);

	INT GetVersion();
	string GetFilename();
	bool GetPatternValues(UINT& rows, 
						UINT& columns, 
						UINT& start_value, 
						UINT& rows_increment, 
						UINT& columns_increment, 
						UINT& rows_same, 
						UINT& columns_same);
	UID_CLASS GetMemoryTransferSyntax();
	UID_CLASS GetFileTransferSyntax();
    ATTR_VR_ENUM GetFileVR();
    TS_CODE GetFileTSCode();
	virtual ATTR_VR_ENUM GetVR() = 0;
	UINT16 GetBitsAllocated();
	UINT32 GetLength(bool*);

	void UpdateData(UINT16 bits_allocated, UINT16 samples_per_pixel, UINT16 planar_configuration);

	virtual void SwapBytes(BYTE* buffer_ptr, UINT32 length);

	virtual bool IsByteSwapRequired() = 0;

	virtual bool StreamPatternTo(DATA_TF_CLASS& data_transfer) = 0;

	bool StreamTo(DATA_TF_CLASS& data_transfer);
	bool StreamFrom(DATA_TF_CLASS& data_transfer);

	bool ReadFileHeader();

	void DisplayHeaderDetail();

	void SetLogger(LOG_CLASS* logger_ptr);
};


#endif /* OTHER_VALUE_STREAM_H */
