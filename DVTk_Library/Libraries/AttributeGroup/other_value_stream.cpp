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
#include "other_value_stream.h"


//>>===========================================================================

OTHER_VALUE_STREAM_CLASS::~OTHER_VALUE_STREAM_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	// - terminate the streaming
	FILE_TF_CLASS::close();
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::Initialise()

//  DESCRIPTION     : Initialise the base class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	file_versionM = OV_HEADER_VERSION3;
	data_sourceM = GENERATE;
	SetLogger(NULL);

	bits_allocatedM = 0;
	lengthM = 0;

	file_offsetM = 0;
	length_offsetM = 0;

	rowsM = 0;
	columnsM = 0;
	start_valueM = 0;
	rows_incrementM = 0;
	columns_incrementM = 0;
	rows_sameM = 0;
	columns_sameM = 0;

	UID_CLASS transfer_syntax = IMPLICIT_VR_LITTLE_ENDIAN;
	SetMemoryTransferSyntax(transfer_syntax);
	SetFileTransferSyntax(transfer_syntax);
}

//>>===========================================================================

bool OTHER_VALUE_STREAM_CLASS::SetFilename(string filename)

//  DESCRIPTION     : Set filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// save the filename
	filenameM = filename;

	// set the source to file
	data_sourceM = INFILE;

	return true;
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::SetPatternValues(UINT rows, 
					UINT columns, 
					UINT start_value, 
					UINT rows_increment, 
					UINT columns_increment, 
					UINT rows_same, 
					UINT columns_same)

//  DESCRIPTION     : Set other data pattern values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	rowsM = rows;
	columnsM = columns;
	start_valueM = start_value;
	rows_incrementM = rows_increment;
	columns_incrementM = columns_increment;
	rows_sameM = rows_same;
	columns_sameM = columns_same;

	// set the source to generate
	data_sourceM = GENERATE;
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::SetMemoryTransferSyntax(UID_CLASS& transfer_syntax) 

//  DESCRIPTION     : Set memory transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	memory_transfer_syntaxM = transfer_syntax; 
	ms_codeM = transferSyntaxUidToCode(memory_transfer_syntaxM);
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::SetFileTransferSyntax(UID_CLASS& transfer_syntax) 

//  DESCRIPTION     : Set file transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	file_transfer_syntaxM = transfer_syntax; 
	fs_codeM = transferSyntaxUidToCode(file_transfer_syntaxM);
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::SetBitsAllocated(UINT16 bits_allocated) 

//  DESCRIPTION     : Set bits allocated.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	bits_allocatedM = bits_allocated; 
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::SetLength(UINT32 length) 

//  DESCRIPTION     : Set data length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	lengthM = length;
}

//>>===========================================================================

INT OTHER_VALUE_STREAM_CLASS::GetVersion() 

//  DESCRIPTION     : Return file version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return file_versionM; 
}

//>>===========================================================================

string OTHER_VALUE_STREAM_CLASS::GetFilename() 

//  DESCRIPTION     : Return filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return filenameM; 
}

//>>===========================================================================

bool OTHER_VALUE_STREAM_CLASS::GetPatternValues(UINT& rows, 
					UINT& columns, 
					UINT& start_value, 
					UINT& rows_increment, 
					UINT& columns_increment, 
					UINT& rows_same, 
					UINT& columns_same)

//  DESCRIPTION     : Return Other Value pattern values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	rows = rowsM;
	columns = columnsM;
	start_value = start_valueM;
	rows_increment = rows_incrementM;
	columns_increment = columns_incrementM;
	rows_same = rows_sameM;
	columns_same = columns_sameM;
	return true;
}

//>>===========================================================================

UID_CLASS OTHER_VALUE_STREAM_CLASS::GetMemoryTransferSyntax()

//  DESCRIPTION     : Return memory transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return memory_transfer_syntaxM;
}

//>>===========================================================================

UID_CLASS OTHER_VALUE_STREAM_CLASS::GetFileTransferSyntax()

//  DESCRIPTION     : Return file transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return file_transfer_syntaxM;
}

//>>===========================================================================

 TS_CODE OTHER_VALUE_STREAM_CLASS::GetFileTSCode()
 
//  DESCRIPTION     : Return file transfer syntax code.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
     return fs_codeM;
}

//>>===========================================================================

ATTR_VR_ENUM OTHER_VALUE_STREAM_CLASS::GetFileVR()

//  DESCRIPTION     : Return file VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    ATTR_VR_ENUM vr = ATTR_VR_UN;

    // check that we can stream the data from file
    if (data_sourceM == INFILE)
    {
        // try reading the header to get the actual file VR
        if (ReadFileHeader())
        {
            // take the ve found in file
            vr = file_vrM;
        }

        // close it
        FILE_TF_CLASS::close();
    }

    // return the file vr
    return vr;
}

//>>===========================================================================

UINT16 OTHER_VALUE_STREAM_CLASS::GetBitsAllocated()

//  DESCRIPTION     : Return bits allocated.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return bits_allocatedM;
}

//>>===========================================================================

UINT32 OTHER_VALUE_STREAM_CLASS::GetLength(bool *lengthOk)

//  DESCRIPTION     : Return data length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	lengthM = 0;

	// check data source
	switch(data_sourceM) 
	{
	case INFILE:
		// compute length from file contents
		// - read DVT header
		*lengthOk = ReadFileHeader();
		break;

	case GENERATE:
		// compute length from pattern values
		lengthM = (rowsM * columnsM * bits_allocatedM) / 8;
        *lengthOk = true;
		break;

	default:
		break;
	}

	return lengthM;
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::GenerateFilename()

//  DESCRIPTION     : Function to generate a filename for storing Other data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char filename[32];
	string storage_root;
	unsigned long pixel_index = 1;
	ATTR_VR_ENUM vr = GetVR();

	if (loggerM_ptr)
	{
		// get the storage root and pixel index
		storage_root = loggerM_ptr->getStorageRoot();
		pixel_index = getPixIndex(storage_root);
	}
	else
	{
		pixel_index = uniq8odd();
	}

	// generate a filename for the image pixel data
	if (vr == ATTR_VR_OB)
	{
		if (bits_allocatedM == 8) 
		{
			if (fs_codeM & TS_COMPRESSED) 
			{
				sprintf(filename, "B08C%04d.pix", pixel_index);
			}
			else 
			{
				sprintf(filename, "B08_%04d.pix", pixel_index);
			}
		}
		else if (bits_allocatedM == 16) 
		{
			if (fs_codeM & TS_COMPRESSED) 
			{
				sprintf(filename, "B16C%04d.pix", pixel_index);
			}
			else 
			{
				sprintf(filename, "B16_%04d.pix", pixel_index);
			}
		}
		else
		{
			sprintf(filename, "%08d.pix", pixel_index);
		}
	}
	else if (vr == ATTR_VR_OF) 
	{
		if (fs_codeM & TS_BIG_ENDIAN) 
		{
			sprintf(filename, "F32B%04d.pix", pixel_index);
		}
		else 
		{
			sprintf(filename, "F32L%04d.pix", pixel_index);
		}
	}
	else if (vr == ATTR_VR_OV) 
	{
		if (fs_codeM & TS_BIG_ENDIAN) 
		{
			sprintf(filename, "VL64B%04d.pix", pixel_index);
		}
		else 
		{
			sprintf(filename, "VL64L%04d.pix", pixel_index);
		}
	}
	else if (vr == ATTR_VR_OW) 
	{
		if (bits_allocatedM == 8) 
		{
			if (fs_codeM & TS_BIG_ENDIAN) 
			{
				sprintf(filename, "W08B%04d.pix", pixel_index);
			}
			else 
			{
				sprintf(filename, "W08L%04d.pix", pixel_index);
			}
		}
		else if (bits_allocatedM == 16) 
		{
			if (fs_codeM & TS_BIG_ENDIAN) 
			{
				sprintf(filename, "W16B%04d.pix", pixel_index);
			}
			else 
			{
				sprintf(filename, "W16L%04d.pix", pixel_index);
			}
		}
		else 
		{
			sprintf(filename, "%08d.pix", pixel_index);
		}
	}
	else 
	{
		sprintf(filename, "%08d.pix", pixel_index);
	}

	// see if a data file root is defined
	if (loggerM_ptr)
	{
        // see if the storage root has been defined
        if (storage_root.length())
        {
            // set up filename by including path
            filenameM = storage_root;
            if (storage_root[storage_root.length()-1] != '\\') filenameM += "\\";
            filenameM += filename;
        }
		else
		{
			// local filename
			filenameM = filename;
		}
	}
	else
	{
		// local filename
		filenameM = filename;
	}
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::SwapBytes(BYTE* buffer_ptr, UINT32 length)

//  DESCRIPTION     : Perform byte swap in 16-bit data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// byte swap data
	UINT32 i = 0;

	while (i < length) 
	{
		BYTE temp = buffer_ptr[i];
		buffer_ptr[i] = buffer_ptr[i+1];
		buffer_ptr[i+1] = temp;
		i += 2;
	}
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::UpdateData(UINT16 bits_allocated, UINT16 samples_per_pixel, UINT16 planar_configuration)

//  DESCRIPTION     : Update the Other Data pattern values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// only make update for data to be generated
	if (data_sourceM == GENERATE) 
	{
		if (samples_per_pixel > 1) 
		{
			if (planar_configuration == FRAME_INTERLEAVE) 
			{
				// adjust rows by the given value
				rowsM *= samples_per_pixel;
			}
			else 
			{
				// adjust columns by given value
				// make RGB pixels same value
				columnsM *= samples_per_pixel;

				// dataM[5] = columnsSame
				columns_sameM = samples_per_pixel;
			}
		}

		// set up the bitsAllocated - and hence the total length
		bits_allocatedM = bits_allocated;
		lengthM = rowsM * columnsM;

		if (bits_allocated == 16) 
		{
			// for 16 bits - double length
			lengthM *= 2;
		}
		else if (bits_allocated == 32) 
		{
			// for 32 bits - quadruple length
			lengthM *= 4;
		}
	}
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::SetLogger(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Set the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// save the logger addres
	loggerM_ptr = logger_ptr;
	FILE_TF_CLASS::setLogger(logger_ptr);
}
