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
#include "dcm_attribute.h"
#include "dcm_attribute_group.h"
#include "dcm_value_ul.h"
#include "dcm_value_sq.h"
#include "private_attribute.h"

#include "Idefinition.h"		// Definition component interface
#include "IAttributeGroup.h"	// Attribute Group component interface


//>>===========================================================================

bool DCM_ATTRIBUTE_CLASS::encode(DATA_TF_CLASS& dataTransfer)

//  DESCRIPTION     : Encode the attribute into the dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if attribute should be present in encoded stream
	if (!IsPresent()) return true;

	// get the attribute tag
	UINT16 group = GetGroup();
	UINT16 element = GetElement();

	// get length of the attribute
	UINT32 length32 = getPaddedLength();
	UINT32 logLength32 = length32;

	// encode group
	dataTransfer << group;

	// encode element
	dataTransfer << element;

	// check if this attribute is an SQ - then check if an undefined length should be used
	// - getPaddedLength() must return the actual length as this is used to compute group lengths, etc
	if (GetVR() == ATTR_VR_SQ)
	{
		definedLengthM = (length32 == UNDEFINED_LENGTH) ? false : true;
		if(!definedLengthM)
		{
			if (loggerM_ptr)
			{
	    		loggerM_ptr->text(LOG_DEBUG, 1, "Encoding attribute (%04X,%04X) with SQ length to UNDEFINED", group, element);
			}
		}		
	}

	// on explicit VR
	if (dataTransfer.isExplicitVR()) 
	{
		BYTE sl[2];
		UINT16 length16;
		ATTR_VR_ENUM vr = GetVR();

		// check if special vr encoding required
		if (transferVrM == TRANSFER_ATTR_VR_UNKNOWN)
		{
			// force VR to unknown
			vr = ATTR_VR_UN;
		}
		else if (transferVrM == TRANSFER_ATTR_VR_QUESTION)
		{
			// force VR to ??
			vr = ATTR_VR_QQ;
		}

		// get the VR of the attribute in 16 bit format
		UINT16 vr16 = dataTransfer.vrToVr16(vr);

		// encode VR
		sl[0] = ((BYTE) (vr16 >> 8));
		sl[1] = ((BYTE) (vr16 & 0x00FF));
		dataTransfer << sl[0];
		dataTransfer << sl[1];
			
		// check for special OB, OF, OW, OL, OV, OD, SQ, UN, UR & UT encoding
		if ((vr == ATTR_VR_OB) || 
			(vr == ATTR_VR_OF) || 
			(vr == ATTR_VR_OW) || 
			(vr == ATTR_VR_OL) || 
			(vr == ATTR_VR_OD) || 
			(vr == ATTR_VR_OV) || 
			(vr == ATTR_VR_SQ) || 
			(vr == ATTR_VR_UN) || 
			(vr == ATTR_VR_UR) ||
			(vr == ATTR_VR_UT)) 
		{
			// encode 16 bit padding
			length16 = 0;
			dataTransfer << length16;

			// check if we have to encode compressed pixel data
			if ((GetGroup() == PIXEL_GROUP) && 
				(GetElement() == PIXEL_DATA) && 
				(dataTransfer.isCompressed())) 
			{
				// check the attribute value to know how to handle the pixel data length
				// - uncompressed data must be encoded with a real length
				// - compressed data must be encoded with an UNDEFINED length
				if ((vr == ATTR_VR_OB) &&
					(GetNrValues() == 1))
				{
					VALUE_OB_CLASS *value_ptr = static_cast<VALUE_OB_CLASS*>(GetValue(0));
					if (value_ptr == NULL) return false; // cannot happen
					value_ptr->SetLogger(loggerM_ptr);

					// set up the data stream
					OB_VALUE_STREAM_CLASS stream;
					stream.SetLogger(loggerM_ptr);

					// check if data in a file
					string filename;
					if (value_ptr->Get(filename) == MSG_OK)
					{
						// set to stream from file
						stream.SetFilename(filename);

						// check if the file contents are compressed
						if ((stream.GetFileTSCode() & TS_COMPRESSED) &&
							(value_ptr->GetDecodedLengthUndefined() == true))
						{
							// compressed file content must be encoded with UNDEFINED length
							length32 = UNDEFINED_LENGTH;
						}
					}
					else
					{
						// all others encoded with undefined length
						length32 = UNDEFINED_LENGTH;
					}
				}
				else if ((vr == ATTR_VR_OW) &&
					(GetNrValues() == 1))
				{
					VALUE_OW_CLASS *value_ptr = static_cast<VALUE_OW_CLASS*>(GetValue(0));
					if (value_ptr == NULL) return false; // cannot happen
					value_ptr->SetLogger(loggerM_ptr);

					// set up the data stream
					OW_VALUE_STREAM_CLASS stream;
					stream.SetLogger(loggerM_ptr);

					// check if data in a file
					string filename;
					if (value_ptr->Get(filename) == MSG_OK)
					{
						// set to stream from file
						stream.SetFilename(filename);

						// check if the file contents are compressed
						if (stream.GetFileTSCode() & TS_COMPRESSED)
						{
							// compressed file content must be encoded with UNDEFINED length
							length32 = UNDEFINED_LENGTH;
						}
					}
					else
					{
						// all others encoded with undefined length
						length32 = UNDEFINED_LENGTH;
					}
				}
				else if ((vr == ATTR_VR_OL) &&
					(GetNrValues() == 1))
				{
					VALUE_OL_CLASS *value_ptr = static_cast<VALUE_OL_CLASS*>(GetValue(0));
					if (value_ptr == NULL) return false; // cannot happen
					value_ptr->SetLogger(loggerM_ptr);

					// set up the data stream
					OL_VALUE_STREAM_CLASS stream;
					stream.SetLogger(loggerM_ptr);

					// check if data in a file
					string filename;
					if (value_ptr->Get(filename) == MSG_OK)
					{
						// set to stream from file
						stream.SetFilename(filename);

						// check if the file contents are compressed
						if (stream.GetFileTSCode() & TS_COMPRESSED)
						{
							// compressed file content must be encoded with UNDEFINED length
							length32 = UNDEFINED_LENGTH;
						}
					}
					else
					{
						// all others encoded with undefined length
						length32 = UNDEFINED_LENGTH;
					}
				}
				else if ((vr == ATTR_VR_OV) &&
					(GetNrValues() == 1))
				{
					VALUE_OV_CLASS *value_ptr = static_cast<VALUE_OV_CLASS*>(GetValue(0));
					if (value_ptr == NULL) return false; // cannot happen
					value_ptr->SetLogger(loggerM_ptr);

					// set up the data stream
					OV_VALUE_STREAM_CLASS stream;
					stream.SetLogger(loggerM_ptr);

					// check if data in a file
					string filename;
					if (value_ptr->Get(filename) == MSG_OK)
					{
						// set to stream from file
						stream.SetFilename(filename);

						// check if the file contents are compressed
						if (stream.GetFileTSCode() & TS_COMPRESSED)
						{
							// compressed file content must be encoded with UNDEFINED length
							length32 = UNDEFINED_LENGTH;
						}
					}
					else
					{
						// all others encoded with undefined length
						length32 = UNDEFINED_LENGTH;
					}
				}
				else if (GetNrValues() == 0)
				{
					length32 = 0;
				}
				else
				{
					// all others encoded with undefined length
					length32 = UNDEFINED_LENGTH;
				}
			}

			// encode 32 bit length
			dataTransfer << length32;
		}
		else 
		{
			// force 32 bit length to 16 bit and encode
			length16 = (UINT16) length32;
			dataTransfer << length16;
		}
	}
	else
	{
		// implicit VR - encode 32 bit length directly
		dataTransfer << length32;
	}

	// log action
	if (loggerM_ptr)
	{
		if (length32 != UNDEFINED_LENGTH)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Encode - Attribute (%04X,%04X), vr is %s, length is %08X", group, element, stringVr(GetVR()), logLength32);
		}
		else
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Encode - Attribute (%04X,%04X), vr is %s, length is %d, with undefined length (0xFFFFFFFF)", group, element, stringVr(GetVR()), logLength32);
		}
	}

	// finally encode any values
	return encodeValue(dataTransfer, length32);
}

//>>===========================================================================

bool DCM_ATTRIBUTE_CLASS::encodeValue(DATA_TF_CLASS& dataTransfer, UINT32 length)

//  DESCRIPTION     : encode the attribute value(s) into the dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;
	BYTE nullChar = NULLCHAR;
	BYTE spaceChar = SPACECHAR;
	BYTE backslash = BACKSLASH;
	ATTR_VR_ENUM vr = GetVR();

	// zero length is OK
	if (length == 0) return true;

	// check if special vr encoding required
	if (transferVrM == TRANSFER_ATTR_VR_UNKNOWN)
	{
		// force VR to unknown
		vr = ATTR_VR_UN;
	}

	// need to encode based on Attribute VR
	// - due to little/big endian, value multiplicity, etc
	switch (vr) 
	{
	case ATTR_VR_AE:
	case ATTR_VR_AS:
	case ATTR_VR_CS:
	case ATTR_VR_DA:
	case ATTR_VR_DS:
	case ATTR_VR_DT:
	case ATTR_VR_IS:
	case ATTR_VR_LO:
	case ATTR_VR_PN:
	case ATTR_VR_SH:
	case ATTR_VR_TM:
	case ATTR_VR_UI:
	case ATTR_VR_UC:
		{
			// encode [multi-valued] "string" attribute value
			for (int i = 0; i < GetNrValues();)
			{
				// get a single value
				BASE_VALUE_CLASS *value_ptr = GetValue(i);
				if (value_ptr == NULL) return false; // cannot happen

				string value;
				if (value_ptr->Get(value) != MSG_OK) return false;

				dataTransfer.writeBinary((BYTE*)value.c_str(), value.length());

				length -= value.length();

				// move to next value
				i++;

				if (i < GetNrValues())
				{
					// handle delimiter
					if (vr != ATTR_VR_AS) 
					{
						dataTransfer << backslash;
						length--;
					}
				}
			}

			if (length) 
			{
				// handle padding
				if (GetVR() == ATTR_VR_UI)
				{
					dataTransfer << nullChar;
				}
				else
				{
					dataTransfer << spaceChar;
				}

				length--;
			}
		}
		break;

	case ATTR_VR_LT:
	case ATTR_VR_ST:
    case ATTR_VR_UN:
	case ATTR_VR_UT:
	case ATTR_VR_UR:
		{
			// For ATTR_VR_UN encode single-valued "byte array" attribute value(s)
			// For ATTR_VR_LT, ATTR_VR_ST, ATTR_VR_UT
            // encode single-valued "string" attribute value(s)
			if (GetNrValues() != 1)
			{
				// can only be single value
				return false;
			}

			BASE_VALUE_CLASS *value_ptr = GetValue(0);
			if (value_ptr == NULL) return false; // cannot happen
			
			BYTE* data_ptr;
			UINT32 data_length;
			if (value_ptr->Get(&data_ptr, data_length) != MSG_OK) return false;

			dataTransfer.writeBinary(data_ptr, data_length);

			length -= data_length;

			if (length) 
			{
				// handle padding
				dataTransfer << spaceChar;

				length--;
			}
		}
		break;
	case ATTR_VR_OB:
		{
			// encode the other data attribute value
			if (GetNrValues() != 1)
			{
				// can only be single value
				return false;
			}

			VALUE_OB_CLASS *value_ptr = static_cast<VALUE_OB_CLASS*>(GetValue(0));
			if (value_ptr == NULL) return false; // cannot happen
            value_ptr->SetLogger(loggerM_ptr);

			// set up the data stream
			OB_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);

			// check if data in a file
			string filename;
			if (value_ptr->Get(filename) == MSG_OK)
			{
				// set to stream from file
				stream.SetFilename(filename);
			}
			else
			{
				UINT32 rows,  columns,  start_value, rows_increment, columns_increment, rows_same, columns_same;

				// get the pattern values
				value_ptr->Get((UINT32) 0, rows);
				value_ptr->Get((UINT32) 1, columns);
				value_ptr->Get((UINT32) 2, start_value);
				value_ptr->Get((UINT32) 3, rows_increment);
				value_ptr->Get((UINT32) 4, columns_increment);
				value_ptr->Get((UINT32) 5, rows_same);
				value_ptr->Get((UINT32) 6, columns_same);

				// set to stream from a generated pattern
				stream.SetPatternValues(rows, columns, start_value, rows_increment, columns_increment, rows_same, columns_same);
			}

            // update the stream with the appropriate attribute values
            stream.UpdateData(value_ptr->GetBitsAllocated(), value_ptr->GetSamplesPerPixel(), value_ptr->GetPlanarConfiguration());

			// stream the other data into the data transfer
			if (!stream.StreamTo(dataTransfer))
			{
				// should never fail
				return false;
			}

			// force length to zero		
			length = 0;
		}
		break;

	case ATTR_VR_OF:
		{
			// encode the other data attribute value
			if (GetNrValues() != 1)
			{
				// can only be single value
				return false;
			}

			VALUE_OF_CLASS *value_ptr = static_cast<VALUE_OF_CLASS*>(GetValue(0));
			if (value_ptr == NULL) return false; // cannot happen
            value_ptr->SetLogger(loggerM_ptr);

			// set up the data stream
			OF_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);

			// check if data in a file
			string filename;
			if (value_ptr->Get(filename) == MSG_OK)
			{
				// set to stream from file
				stream.SetFilename(filename);
			}
			else
			{
				UINT32 rows,  columns,  start_value, rows_increment, columns_increment, rows_same, columns_same;

				// get the pattern values
				value_ptr->Get((UINT32) 0, rows);
				value_ptr->Get((UINT32) 1, columns);
				value_ptr->Get((UINT32) 2, start_value);
				value_ptr->Get((UINT32) 3, rows_increment);
				value_ptr->Get((UINT32) 4, columns_increment);
				value_ptr->Get((UINT32) 5, rows_same);
				value_ptr->Get((UINT32) 6, columns_same);

				// set to stream from a generated pattern
				stream.SetPatternValues(rows, columns, start_value, rows_increment, columns_increment, rows_same, columns_same);
			}

            // update the stream with the appropriate attribute values
            stream.UpdateData(value_ptr->GetBitsAllocated(), value_ptr->GetSamplesPerPixel(), value_ptr->GetPlanarConfiguration());

            // stream the other data into the data transfer
			if (!stream.StreamTo(dataTransfer))
			{
				// should never fail
				return false;
			}

			// force length to zero		
			length = 0;
		}
		break;

		case ATTR_VR_OD:
		{
			// encode the other data attribute value
			if (GetNrValues() != 1)
			{
				// can only be single value
				return false;
			}

			VALUE_OD_CLASS *value_ptr = static_cast<VALUE_OD_CLASS*>(GetValue(0));
			if (value_ptr == NULL) return false; // cannot happen
            value_ptr->SetLogger(loggerM_ptr);

			// set up the data stream
			OD_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);

			// check if data in a file
			string filename;
			if (value_ptr->Get(filename) == MSG_OK)
			{
				// set to stream from file
				stream.SetFilename(filename);
			}
			else
			{
				UINT32 rows,  columns,  start_value, rows_increment, columns_increment, rows_same, columns_same;

				// get the pattern values
				value_ptr->Get((UINT32) 0, rows);
				value_ptr->Get((UINT32) 1, columns);
				value_ptr->Get((UINT32) 2, start_value);
				value_ptr->Get((UINT32) 3, rows_increment);
				value_ptr->Get((UINT32) 4, columns_increment);
				value_ptr->Get((UINT32) 5, rows_same);
				value_ptr->Get((UINT32) 6, columns_same);

				// set to stream from a generated pattern
				stream.SetPatternValues(rows, columns, start_value, rows_increment, columns_increment, rows_same, columns_same);
			}

            // update the stream with the appropriate attribute values
            stream.UpdateData(value_ptr->GetBitsAllocated(), value_ptr->GetSamplesPerPixel(), value_ptr->GetPlanarConfiguration());

            // stream the other data into the data transfer
			if (!stream.StreamTo(dataTransfer))
			{
				// should never fail
				return false;
			}

			// force length to zero		
			length = 0;
		}
		break;

	case ATTR_VR_OV:
		{
			// encode the other data attribute value
			if (GetNrValues() != 1)
			{
				// can only be single value
				return false;
			}

			VALUE_OV_CLASS *value_ptr = static_cast<VALUE_OV_CLASS*>(GetValue(0));
			if (value_ptr == NULL) return false; // cannot happen
            value_ptr->SetLogger(loggerM_ptr);

			// set up the data stream
			OV_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);

			// check if data in a file
			string filename;
			if (value_ptr->Get(filename) == MSG_OK)
			{
				// set to stream from file
				stream.SetFilename(filename);
			}
			else
			{
				UINT32 rows,  columns,  start_value, rows_increment, columns_increment, rows_same, columns_same;

				// get the pattern values
				value_ptr->Get((UINT32) 0, rows);
				value_ptr->Get((UINT32) 1, columns);
				value_ptr->Get((UINT32) 2, start_value);
				value_ptr->Get((UINT32) 3, rows_increment);
				value_ptr->Get((UINT32) 4, columns_increment);
				value_ptr->Get((UINT32) 5, rows_same);
				value_ptr->Get((UINT32) 6, columns_same);

				// set to stream from a generated pattern
				stream.SetPatternValues(rows, columns, start_value, rows_increment, columns_increment, rows_same, columns_same);
			}

            // update the stream with the appropriate attribute values
            stream.UpdateData(value_ptr->GetBitsAllocated(), value_ptr->GetSamplesPerPixel(), value_ptr->GetPlanarConfiguration());

            // stream the other data into the data transfer
			if (!stream.StreamTo(dataTransfer))
			{
				// should never fail
				return false;
			}

			// force length to zero		
			length = 0;
		}
		break;

	case ATTR_VR_OW:
		{
			// encode the other data attribute value
			if (GetNrValues() != 1)
			{
				// can only be single value
				return false;
			}

			VALUE_OW_CLASS *value_ptr = static_cast<VALUE_OW_CLASS*>(GetValue(0));
			if (value_ptr == NULL) return false; // cannot happen
            value_ptr->SetLogger(loggerM_ptr);

			// set up the data stream
			OW_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);

			// check if data in a file
			string filename;
			if (value_ptr->Get(filename) == MSG_OK)
			{
				// set to stream from file
				stream.SetFilename(filename);
			}
			else
			{
				UINT32 rows,  columns,  start_value, rows_increment, columns_increment, rows_same, columns_same;

				// get the pattern values
				value_ptr->Get((UINT32) 0, rows);
				value_ptr->Get((UINT32) 1, columns);
				value_ptr->Get((UINT32) 2, start_value);
				value_ptr->Get((UINT32) 3, rows_increment);
				value_ptr->Get((UINT32) 4, columns_increment);
				value_ptr->Get((UINT32) 5, rows_same);
				value_ptr->Get((UINT32) 6, columns_same);

				// set to stream from a generated pattern
				stream.SetPatternValues(rows, columns, start_value, rows_increment, columns_increment, rows_same, columns_same);
			}

            // update the stream with the appropriate attribute values
            stream.UpdateData(value_ptr->GetBitsAllocated(), value_ptr->GetSamplesPerPixel(), value_ptr->GetPlanarConfiguration());

            // stream the other data into the data transfer
			if (!stream.StreamTo(dataTransfer))
			{
				// should never fail
				return false;
			}

			// force length to zero		
			length = 0;
		}
		break;

		case ATTR_VR_OL:
		{
			// encode the other data attribute value
			if (GetNrValues() != 1)
			{
				// can only be single value
				return false;
			}

			VALUE_OL_CLASS *value_ptr = static_cast<VALUE_OL_CLASS*>(GetValue(0));
			if (value_ptr == NULL) return false; // cannot happen
            value_ptr->SetLogger(loggerM_ptr);

			// set up the data stream
			OL_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);

			// check if data in a file
			string filename;
			if (value_ptr->Get(filename) == MSG_OK)
			{
				// set to stream from file
				stream.SetFilename(filename);
			}
			else
			{
				UINT32 rows,  columns,  start_value, rows_increment, columns_increment, rows_same, columns_same;

				// get the pattern values
				value_ptr->Get((UINT32) 0, rows);
				value_ptr->Get((UINT32) 1, columns);
				value_ptr->Get((UINT32) 2, start_value);
				value_ptr->Get((UINT32) 3, rows_increment);
				value_ptr->Get((UINT32) 4, columns_increment);
				value_ptr->Get((UINT32) 5, rows_same);
				value_ptr->Get((UINT32) 6, columns_same);

				// set to stream from a generated pattern
				stream.SetPatternValues(rows, columns, start_value, rows_increment, columns_increment, rows_same, columns_same);
			}

            // update the stream with the appropriate attribute values
            stream.UpdateData(value_ptr->GetBitsAllocated(), value_ptr->GetSamplesPerPixel(), value_ptr->GetPlanarConfiguration());

            // stream the other data into the data transfer
			if (!stream.StreamTo(dataTransfer))
			{
				// should never fail
				return false;
			}

			// force length to zero		
			length = 0;
		}
		break;

	case ATTR_VR_AT: 
		{
			// encode attribute AT value(s)
			// attribute length must be a multiple of VR length
			if ((length % AT_LENGTH) != 0) return false;

			for (int i = 0; i < GetNrValues(); i++) 
			{
				// get a single value
				BASE_VALUE_CLASS *value_ptr = GetValue(i);
				if (value_ptr == NULL) return false; // cannot happen

				UINT32 value;
				if (value_ptr->Get(value) != MSG_OK) return false;

				// must decode AT as 2 times US
				UINT16 group = ((UINT16) (value >> 16));
				UINT16 element = ((UINT16) (value & 0x0000FFFF));
				dataTransfer << group;
				dataTransfer << element;

				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_FL: 
		{
			// encode attribute FL value(s)
			// attribute length must be a multiple of VR length
			if ((length % FL_LENGTH) != 0) return false;

			for (int i = 0; i < GetNrValues(); i++) 
			{
				// get a single value
				BASE_VALUE_CLASS *value_ptr = GetValue(i);
				if (value_ptr == NULL) return false; // cannot happen

				float value;
				if (value_ptr->Get(value) != MSG_OK) return false;

				dataTransfer << value;

				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_FD: 
		{
			// encode attribute FD value(s)
			// attribute length must be a multiple of VR length
			if ((length % FD_LENGTH) != 0) return false;

			for (int i = 0; i < GetNrValues(); i++) 
			{
				// get a single value
				BASE_VALUE_CLASS *value_ptr = GetValue(i);
				if (value_ptr == NULL) return false; // cannot happen

				double value;
				if (value_ptr->Get(value) != MSG_OK) return false;

				dataTransfer << value;

				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_SL: 
		{
			// encode attribute SL value(s)
			// attribute length must be a multiple of VR length
			if ((length % SL_LENGTH) != 0) return false;

			for (int i = 0; i < GetNrValues(); i++) 
			{
				// get a single value
				BASE_VALUE_CLASS *value_ptr = GetValue(i);
				if (value_ptr == NULL) return false; // cannot happen

				INT32 value;
				if (value_ptr->Get(value) != MSG_OK) return false;

				dataTransfer << value;

				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_SQ:
		{
			DCM_VALUE_SQ_CLASS *sqValue_ptr = NULL;

			// encode SQ attribute value(s)
			switch(GetNrValues())
			{
			case 0:
				// get a new sequence value to help us encode this
				sqValue_ptr = new DCM_VALUE_SQ_CLASS(length);
				break;
			case 1:
				// pick up existing value
				sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(GetValue(0));
				break;
			default:
				return false;
				break;
			}

			// check we have something to encode
			if (sqValue_ptr == NULL) return false; // cannot happen
		
			// encode the individual items
			result = sqValue_ptr->encode(dataTransfer);

			// free up locally allocated data
			if (GetNrValues() == 0)
			{
				delete sqValue_ptr;
			}

			length = 0;
		}
		break;

	case ATTR_VR_SS: 
		{
			// encode attribute SS value(s)
			// attribute length must be a multiple of VR length
			if ((length % SS_LENGTH) != 0) return false;

			for (int i = 0; i < GetNrValues(); i++) 
			{
				// get a single value
				BASE_VALUE_CLASS *value_ptr = GetValue(i);
				if (value_ptr == NULL) return false; // cannot happen

				INT16 value;
				if (value_ptr->Get(value) != MSG_OK) return false;

				dataTransfer << value;

				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_UL: 
		{
			// encode attribute UL value(s)
			// attribute length must be a multiple of VR length
			if ((length % UL_LENGTH) != 0) return false;
			for (int i = 0; i < GetNrValues(); i++) 
			{
				// get a single value
				DCM_VALUE_UL_CLASS *value_ptr = NULL;
				UINT32 base_value;
				string identifier;
#ifdef NDEBUG
				VALUE_UL_CLASS *base_value_ptr = static_cast<VALUE_UL_CLASS*>(GetValue(i));
				if (base_value_ptr == NULL)
				{					
					return false; // cannot happen
				}
				else
				{
					if (base_value_ptr->Get(base_value) != MSG_OK) return false;
					if(base_value != 0)
					{
						value_ptr = static_cast<DCM_VALUE_UL_CLASS*>(base_value_ptr);
						if (value_ptr == NULL)
						{					
							return false; // cannot happen
						}
						identifier = value_ptr->getIdentifier();
					}
					else
					{
						identifier = "";
					}
				}

#else
				VALUE_UL_CLASS *base_value_ptr = dynamic_cast<VALUE_UL_CLASS*>(GetValue(i));
				if (base_value_ptr == NULL)
				{					
					return false; // cannot happen
				}
				else
				{
					if (base_value_ptr->Get(base_value) != MSG_OK) return false;
					if(base_value != 0)
					{
						value_ptr = dynamic_cast<DCM_VALUE_UL_CLASS*>(base_value_ptr);
						if (value_ptr == NULL)
						{					
							return false; // cannot happen
						}
						identifier = value_ptr->getIdentifier();
					}
					else
					{
						identifier = "";
					}
				}
#endif
				// check if an identifier has been provided
				UINT32 value;
		
				if (identifier.length())
				{
					value = dataTransfer.getItemOffset(identifier);
				}
				else 
				{
					value = base_value;
				}

				dataTransfer << value;

				length -= sizeof(value);
			}			
		}
		break;
	
	case ATTR_VR_US: 
		{
			// encode attribute US value(s)
			// attribute length must be a multiple of VR length
			if ((length % US_LENGTH) != 0) return false;

			for (int i = 0; i < GetNrValues(); i++) 
			{
				// get a single value
				BASE_VALUE_CLASS *value_ptr = GetValue(i);
				if (value_ptr == NULL) return false; // cannot happen

				UINT16 value;
				if (value_ptr->Get(value) != MSG_OK) return false;

				dataTransfer << value;

				length -= sizeof(value);
			}
		}
		break;

	default:
		result = false;
		break;
	}

	// we should have encoded all the values
	// length should coincide with total attribute lengths encoded
	if (length) 
	{
		result = false;
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_CLASS::decode(DATA_TF_CLASS& dataTransfer, UINT16 lastGroup, UINT16 lastElement, UINT32 *length_ptr)

//  DESCRIPTION     : decode the attribute from the dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT32 length32;
	UINT32 thisLength = 0;
	bool result = false;
    bool switchToLittleEndian = false;

	// decode group
	UINT16 group;
	dataTransfer >> group;
	SetGroup(group);
	thisLength += sizeof(group);
	SetMappedGroup(group);

    // decode element
	UINT16 element;
	dataTransfer >> element;
	SetElement(element);
	thisLength += sizeof(element);
	SetMappedElement(element);

	// stop decode once we have reached the last group / element
	if (((GetGroup() >= lastGroup) &&
        (lastElement == LAST_ELEMENT)) ||
      ((GetGroup() == lastGroup) &&
        (GetElement() == lastElement)))
	{
		// rewind the dataTransfer by the length of data read
		dataTransfer.rewind(sizeof(group)+sizeof(element));
		return false;
	}

	// check for item / sequence delimiter
	if ((GetGroup() == ITEM_GROUP) &&
		((GetElement() == ITEM_DELIMITER) ||
		 (GetElement() == SQ_DELIMITER)))
	{
		// read the delimiter length
		dataTransfer >> length32;
		thisLength += sizeof(length32);
	
		// return attribute length
        if (length_ptr)
        {
    		*length_ptr = thisLength;
        }

        if (loggerM_ptr)
        {
            if (GetElement() == ITEM_DELIMITER)
            {
                loggerM_ptr->text(LOG_DEBUG, 1, "Item Delimiter reached. Length: %08X", length32);
            }
            else
            {
                loggerM_ptr->text(LOG_DEBUG, 1, "SQ Delimiter reached. Length: %08X", length32);
            }
        }

		return true;
	}

	// check for private attributes
	if (((GetGroup() & 0x0001) != 0) &&
		(GetElement() > 0x00FF))
	{
		// we have a private attribute - map the tag back from the recognition code table
		if (pahM_ptr)
		{
			UINT16 mapGroup, mapElement;
			bool result = pahM_ptr->mapTagValue(GetGroup(), GetElement(), &mapGroup, &mapElement);

			// on success - copy mapped tag values
			if (result)
			{
				SetMappedGroup(mapGroup);
				SetMappedElement(mapElement);
			}
		}
	}

	ATTR_VR_ENUM vr = ATTR_VR_UN;

	// on explicit VR
	if (dataTransfer.isExplicitVR()) 
	{
		BYTE	sl[2];
		UINT16	length16;

		// we get the VR given
		dataTransfer >> sl[0];
		dataTransfer >> sl[1];
		UINT16 vr16 = (((UINT16) sl[0]) << 8) + ((UINT16) sl[1]);
		vr = dataTransfer.vr16ToVr(vr16);
		SetVR(vr);
		thisLength += sizeof(vr16);

		// check if the vr exists
		if (vr == ATTR_VR_DOESNOTEXIST)
		{
			// attribute is also unknown in the current Defintion files
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Attribute (%04X,%04X) - non-existant explicit VR of %02X%02X found while decoding.", GetGroup(), GetElement(), sl[0], sl[1]);
                loggerM_ptr->text(LOG_INFO, 1, "Set Session properties LOG-DEBUG and PDU-DUMP true for more details.");
			}

			return false;
		}

		// check if unknown/?? VR used
		if (vr == ATTR_VR_UN)
		{
			// set transfer vr
			transferVrM = TRANSFER_ATTR_VR_UNKNOWN;
		}
		else if (vr == ATTR_VR_QQ)
		{
			// set transfer vr
			transferVrM = TRANSFER_ATTR_VR_QUESTION;

			// try to get VR by consulting Definitions
			//vr = DEFINITION->GetAttributeVr(GetMappedGroup(), GetMappedElement());
			//SetVR(vr);
		}
		else 
		{
			// set transfer vr
			transferVrM = TRANSFER_ATTR_VR_EXPLICIT;
		}

		// check for special OB, OF, OW, OL, OD, SQ, UN, UR & UT encoding
		if ((vr == ATTR_VR_OB) || 
			(vr == ATTR_VR_OF) || 
			(vr == ATTR_VR_OW) || 
			(vr == ATTR_VR_OL) || 
			(vr == ATTR_VR_OD) || 
			(vr == ATTR_VR_OV) || 
			(vr == ATTR_VR_SQ) || 
			(vr == ATTR_VR_UN) || 
			(vr == ATTR_VR_UR) ||
			(vr == ATTR_VR_UT))  
		{
			// decode 16 bit padding
			dataTransfer >> length16;
			thisLength += sizeof(length16);

			// decode 32 bit length
			dataTransfer >> length32;
			thisLength += sizeof(length32);

            // if we have a UN vr perhaps we can translate it using our Definitions
            if (vr == ATTR_VR_UN)
            {
				if (unVrDefinitionLookUpM == true)
				{
    				// try to get VR by consulting Definitions
	    			ATTR_VR_ENUM def_vr = DEFINITION->GetAttributeVr(GetMappedGroup(), GetMappedElement());
					if (def_vr != ATTR_VR_UN)
					{
					    // can change VR to that found in Definitions
					    if (loggerM_ptr)
					    {
						    loggerM_ptr->text(LOG_WARNING, 1, "VR conflict while decoding attribute (%04X,%04X) - explicit VR of UN found while definition lookup states VR of %s - swapping VR to %s", GetGroup(), GetElement(), stringVr(def_vr), stringVr(def_vr));
						    loggerM_ptr->text(LOG_INFO, 1, "Will use Little Endian byte ordering to decode the attribute value irrespective of the negotiated Transfer Syntax");
					    }

					    // save definition VR
    					vr = def_vr;
					    SetVR(vr);

						// set flag to indicate that the value should be decoded with Little Endian byte ordering
						switchToLittleEndian = true;
					}
				}
				else
				{
					// import the attribute value using a VR of UN - don't consult definitions
				    if (loggerM_ptr)
				    {
					    loggerM_ptr->text(LOG_INFO, 1, "Decoded attribute (%04X,%04X) has explicit VR of UN - will use VR of UN to import attribute value", GetGroup(), GetElement());
				    }
				}
            }
		}
		else 
		{
			// 16 bit length - force to 32bit
			dataTransfer >> length16;
			length32 = (UINT32) length16;
			thisLength += sizeof(length16);
		}
	}
	else
	{
		// set transfer vr
		transferVrM = TRANSFER_ATTR_VR_IMPLICIT;

		// check for group length attribute
		if (GetElement() == LENGTH_ELEMENT)
		{
			// set VR locally
			vr = ATTR_VR_UL;
		}
		else if ((GetGroup() == GROUP_TWENTY_EIGHT) &&
			((GetElement() == SMALLEST_IMAGE_PIXEL_VALUE) ||
			(GetElement() == LARGEST_IMAGE_PIXEL_VALUE) ||
			(GetElement() == SMALLEST_PIXEL_VALUE_IN_SERIES) ||
			(GetElement() == LARGEST_PIXEL_VALUE_IN_SERIES) ||
			(GetElement() == SMALLEST_IMAGE_PIXEL_VALUE_IN_PLANE) ||
			(GetElement() == LARGEST_IMAGE_PIXEL_VALUE_IN_PLANE) ||
			(GetElement() == PIXEL_PADDING_VALUE)))
		{
			// initialise to US
			vr = ATTR_VR_US;

			// VR depends on the value of the Pixel Representation attribute
			if ((parentM_ptr) &&
				(parentM_ptr->getPixelRepresentation() == 1))

			{
				// force VR to SS
				vr = ATTR_VR_SS;
			}
		}
		else
		{
			// implicit VR - need to look-up the VR in the definitions
			vr = DEFINITION->GetAttributeVr(GetMappedGroup(), GetMappedElement());
		}
		SetVR(vr);

		// check for unknown vr returned
		if (vr == ATTR_VR_UN)
		{
			// check for repeating groups 50xx and 60xx
			if (((GetMappedGroup() & REPEATING_GROUP_MASK) == REPEATING_GROUP_5000) ||
				((GetMappedGroup() & REPEATING_GROUP_MASK) == REPEATING_GROUP_6000))
			{
				// try to get VR again from definitions
				vr = DEFINITION->GetAttributeVr((GetMappedGroup() & REPEATING_GROUP_MASK), GetMappedElement());
				SetVR(vr);
			}
		}

		// finally check to see if we have a UN private recognition code
		if ((vr == ATTR_VR_UN) &&
			((GetGroup() & 0x0001) != 0) &&
			(GetElement() >= 0x0010) &&
			(GetElement() <= 0x00FF))
		{
			// force VR to LO
			vr = ATTR_VR_LO;
			SetVR(vr);
		}

		// decode 32 bit length
		dataTransfer >> length32;
		thisLength += sizeof(length32);
	}

	if (loggerM_ptr)
	{
		if ((GetGroup() != GetMappedGroup()) ||
			(GetElement() != GetMappedElement()))
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Decode - Attribute (%04X,%04X) mapped to (%04X,%04X), vr is %s, length is %08X", GetGroup(), GetElement(), GetMappedGroup(), GetMappedElement(), stringVr(GetVR()), length32);
		}
		else
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Decode - Attribute (%04X,%04X), vr is %s, length is %08X", GetGroup(), GetElement(), stringVr(GetVR()), length32);
		}
	}

	// save actual received length
	receivedLengthM = length32;

    // check if we need to switch to little endian to decode the value
    TS_CODE tsCode = dataTransfer.getTsCode();
    string transferSyntax = dataTransfer.getTransferSyntax();
    if (switchToLittleEndian)
    {
        // switch to Implicit VR Little Endian
        dataTransfer.setTsCode(TS_IMPLICIT_VR | TS_LITTLE_ENDIAN, IMPLICIT_VR_LITTLE_ENDIAN);
    }

	// check if we have an Unknown VR with an undefined length 
    // - if so we will assume that this is a sequence
	if ((vr == ATTR_VR_UN) &&
		(length32 == UNDEFINED_LENGTH))
    {
		switchToLittleEndian = true;

		// switch to Implicit VR Little Endian
        dataTransfer.setTsCode(TS_IMPLICIT_VR | TS_LITTLE_ENDIAN, IMPLICIT_VR_LITTLE_ENDIAN);

		// set the vr to SQ
		vr = ATTR_VR_SQ;
		SetVR(vr);

		// log action
		if (loggerM_ptr)
		{
            if ((GetGroup() & 0x0001) &&
		        (GetElement() > 0x00FF))
	        {
			    loggerM_ptr->text(LOG_INFO, 1, "Received UNDEFINED length Private Attribute (%04X,%04X) - don't know actual VR - will try to decode it with VR SQ", GetGroup(), GetElement());
		    }
            else
            {
				loggerM_ptr->text(LOG_INFO, 1, "Received UNDEFINED length Attribute (%04X,%04X) - don't know actual VR - will try to decode it with VR SQ", GetGroup(), GetElement());
		    }
        }
	}

	// finally decode any values
	result = decodeValue(dataTransfer, &length32);

    // may need to restore the transfer syntax
    if (switchToLittleEndian)
    {
        // switch back to negotiated transfer syntax
        dataTransfer.setTsCode(tsCode, transferSyntax.c_str());
    }

	// compute final attribute length
	if (result)
	{
		// add the actual value length too
		thisLength += length32;
	}

	// return attribute length
    if (length_ptr)
    {
    	*length_ptr = thisLength;
    }

	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_CLASS::decodeValue(DATA_TF_CLASS& dataTransfer, UINT32 *length_ptr)

//  DESCRIPTION     : decode the attribute value(s) from the dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool			result = true;
	UINT32			length = (*length_ptr);
	UINT16			group = GetGroup();
	UINT16			element = GetElement();
	ATTR_VR_ENUM	vr = GetVR();

	// zero length is OK
	if (length == 0) 
	{
		definedLengthM = true;
		return true;
	}
	
	// if the incoming data is corrupt - try to ensure that we don't allocate too much
	// space for the attribute value(s)
	if ((length != UNDEFINED_LENGTH) &&
		(length > REASONABLE_MAXIMUM_LENGTH))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Decoded Attribute (%04X,%04X) has an unexpectedly large length: 0x%X=%d", group, element, length, length);
			loggerM_ptr->text(LOG_ERROR, 1, "Cannot decode the dataset any further...");
		}
		return false;
	}

	if (vr == ATTR_VR_QQ)
	{
		// try to get VR by consulting Definitions
		vr = DEFINITION->GetAttributeVr(GetMappedGroup(), GetMappedElement());
		SetVR(vr);
	}

	// need to decode based on Attribute VR
	// - due to little/big endian, value multiplicity, etc
	switch (vr) 
	{
	case ATTR_VR_AE:
	case ATTR_VR_AS:
	case ATTR_VR_CS:
	case ATTR_VR_DA:
	case ATTR_VR_DS:
	case ATTR_VR_DT:
	case ATTR_VR_IS:
	case ATTR_VR_LO:
	case ATTR_VR_PN:
	case ATTR_VR_SH:
	case ATTR_VR_TM:
	case ATTR_VR_UI: 
	case ATTR_VR_UC: 
		{
			// decode attribute value(s)
			BASE_VALUE_CLASS *value_ptr;

			// allocate a temporary decode buffer
			BYTE *data_ptr = new BYTE [length];

			// read data into buffer
			dataTransfer.readBinary(data_ptr, length);
			
			// we now need to check for VM > 1
			UINT first = 0; 
			UINT last = 0;
			while (findBackslash(data_ptr, length, first, &last)) 
			{
				// save single attribute value
				value_ptr = CreateNewValue(vr);

				value_ptr->Set(&data_ptr[first], last - first);
				AddValue(value_ptr);
				first = last + 1;
			}

			// handle remaining value
			value_ptr = CreateNewValue(vr);

            // check if the maximum VR length is exceeded only by a single SPACE character
            // - if so this has probably been added by the peer encoder to produce an even 
            // length for the whole string
            if (((last - first - 1) == value_ptr->GetMaximumVrLength()) &&
                (data_ptr[last-1] == SPACECHAR))
            {
                // remove trailing SPACE
                last--;
            }

			value_ptr->Set(&data_ptr[first], last - first);
			AddValue(value_ptr);

			// check for private attribute recognition code - only one value allowed
			if ((vr == ATTR_VR_LO) &&
				((group & 0x0001) != 0) &&
				(element >= 0x0010) &&
				(element <= 0x00FF) &&
				(GetNrValues() == 1))
			{
				// register the recognition code
				if (pahM_ptr)
				{
					UINT16 mappedGroup, mappedElement;
					string value;
					if (value_ptr->Get(value) != MSG_OK) return false;
					result = pahM_ptr->registerRecognitionCode((BYTE*)value.c_str(), group, element, &mappedGroup, &mappedElement);

					// on success - copy the mapped tag values
					if (result)
					{
						SetMappedGroup(mappedGroup);
						SetMappedElement(mappedElement);
					}
				}
			}

			// clean up decode buffer
			delete[] data_ptr;
		}
		break;

	case ATTR_VR_LT:
	case ATTR_VR_ST:
    case ATTR_VR_UN:
	case ATTR_VR_UT: 
	case ATTR_VR_UR: 
		{
			// make check on length for UN VR
            if ((vr == ATTR_VR_UN) &&
				(length == UNDEFINED_LENGTH))
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Cannot decode Attribute (%04X,%04X) with VR = UN (Unknown) - length is undefined.", group, element);
				}
				return false;
			}

			// decode attribute value
			// allocate the Data buffer here
			BYTE *data_ptr = new BYTE [length];

			// read data into buffer
			dataTransfer.readBinary(data_ptr, length);

			// save attribute value
			BASE_VALUE_CLASS *value_ptr = CreateNewValue(vr);

			value_ptr->Set(data_ptr, length);
			AddValue(value_ptr);

			// clean up decode buffer
			delete[] data_ptr;
		}
		break;
 
	case ATTR_VR_AT: 
		{
			// decode attribute AT value(s)
			// attribute length must be a multiple of VR length
			if ((length % AT_LENGTH) != 0) return false;

			while (length) 
			{
				// must decode AT as 2 times US
				UINT16 at_group;
				dataTransfer >> at_group;

				UINT16 at_element;
				dataTransfer >> at_element;

				UINT32 value = (((UINT32) at_group) << 16) + ((UINT32) at_element);
				
				// save single attribute value
				BASE_VALUE_CLASS *value_ptr = CreateNewValue(vr);
				value_ptr->Set(value);
				AddValue(value_ptr);
				length -= (sizeof(at_group) + sizeof(at_element));
			}
		}
		break;

	case ATTR_VR_FL: 
		{
			// decode attribute FL value(s)
			// attribute length must be a multiple of VR length
			if ((length % FL_LENGTH) != 0) return false;

			while (length) 
			{
				float value;
				dataTransfer >> value;
				
				// save single attribute value
				BASE_VALUE_CLASS *value_ptr = CreateNewValue(vr);
				value_ptr->Set(value);
				AddValue(value_ptr);
				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_FD: 
		{
			// decode attribute FD value(s)
			// attribute length must be a multiple of VR length
			if ((length % FD_LENGTH) != 0) return false;

			while (length) 
			{
				double value;
				dataTransfer >> value;

				// save single attribute value
				BASE_VALUE_CLASS *value_ptr = CreateNewValue(vr);
				value_ptr->Set(value);
				AddValue(value_ptr);
				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_OB: 
		{
			// initialise the bits allocated
			UINT16 bits_allocated = 8;

			// try to get the parent in order to determine if this is pixel data or not
			if (parentM_ptr)
			{
				if ((GetGroup() == PIXEL_GROUP) &&
					(GetElement() == PIXEL_DATA))
				{
					// get the actual bits allocated
					parentM_ptr->getUSValue(TAG_BITS_ALLOCATED, &bits_allocated);
				}
			}

			// set up the data stream
			OB_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);
			stream.SetBitsAllocated(bits_allocated);
			stream.SetLength(length);

			// stream the other data from the data transfer
			if (!stream.StreamFrom(dataTransfer))
			{
				if (loggerM_ptr)
				{
                    if (length == UNDEFINED_LENGTH)
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OB data for Attribute (%04X,%04X) with UNDEFINED length: 0x%X", group, element, length);
                    }
                    else
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OB data for Attribute (%04X,%04X). Length expected: 0x%X=%d", group, element, length, length);
                    }
				}

				// should never fail
				return false;
			}

			// If data is compressed then save the stream length
			bool ok = true;
			if(dataTransfer.isCompressed())
			{
				*length_ptr = stream.GetLength(&ok);
			}

			// save OB data value
			VALUE_OB_CLASS *value_ptr = (VALUE_OB_CLASS*)CreateNewValue(vr);
            value_ptr->SetLogger(loggerM_ptr);
			value_ptr->Set(stream.GetFilename());
			value_ptr->SetCompressed(dataTransfer.isCompressed());
			value_ptr->SetDecodedLengthUndefined(length);
			AddValue(value_ptr);
		}
		break;

	case ATTR_VR_OF:
		{
			// set up the data stream
			OF_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);
			stream.SetLength(length);

			// stream the other data from the data transfer
			if (!stream.StreamFrom(dataTransfer))
			{
				if (loggerM_ptr)
				{
                    if (length == UNDEFINED_LENGTH)
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OF data for Attribute (%04X,%04X) with UNDEFINED length: 0x%X", group, element, length);
                    }
                    else
                    {
    					loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OF data for Attribute (%04X,%04X). Length expected: 0x%X=%d", group, element, length, length);
                    }
				}

				// should never fail
				return false;
			}

			// save other data value
			VALUE_OF_CLASS *value_ptr = (VALUE_OF_CLASS*)CreateNewValue(vr);
            value_ptr->SetLogger(loggerM_ptr);
			value_ptr->Set(stream.GetFilename());
			value_ptr->SetDecodedLengthUndefined(length);
			AddValue(value_ptr);
		}
		break;

	case ATTR_VR_OD:
		{
			// set up the data stream
			OD_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);
			stream.SetLength(length);

			// stream the other data from the data transfer
			if (!stream.StreamFrom(dataTransfer))
			{
				if (loggerM_ptr)
				{
                    if (length == UNDEFINED_LENGTH)
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OD data for Attribute (%04X,%04X) with UNDEFINED length: 0x%X", group, element, length);
                    }
                    else
                    {
    					loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OD data for Attribute (%04X,%04X). Length expected: 0x%X=%d", group, element, length, length);
                    }
				}

				// should never fail
				return false;
			}

			// save other data value
			VALUE_OD_CLASS *value_ptr = (VALUE_OD_CLASS*)CreateNewValue(vr);
            value_ptr->SetLogger(loggerM_ptr);
			value_ptr->Set(stream.GetFilename());
			value_ptr->SetDecodedLengthUndefined(length);
			AddValue(value_ptr);
		}
		break;

	case ATTR_VR_OW: 
		{
			// initialise the bits allocated
			UINT16 bits_allocated = 16;

			// try to get the parent in order to determine if this is pixel data or not
			if (parentM_ptr)
			{
				if ((GetGroup() == PIXEL_GROUP) &&
					(GetElement() == PIXEL_DATA))
				{
					// get the actual bits allocated
					parentM_ptr->getUSValue(TAG_BITS_ALLOCATED, &bits_allocated);
				}
			}

			// set up the data stream
			OW_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);
			stream.SetBitsAllocated(bits_allocated);
			stream.SetLength(length);

			// stream the other data from the data transfer
			if (!stream.StreamFrom(dataTransfer))
			{
				if (loggerM_ptr)
				{
                    if (length == UNDEFINED_LENGTH)
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OW data for Attribute (%04X,%04X) with UNDEFINED length: 0x%X", group, element, length);
                    }
                    else
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OW data for Attribute (%04X,%04X). Length expected: 0x%X=%d", group, element, length, length);
                    }
				}

				// should never fail
				return false;
			}

			// save OW data value
			VALUE_OW_CLASS *value_ptr = (VALUE_OW_CLASS*)CreateNewValue(vr);
            value_ptr->SetLogger(loggerM_ptr);
			value_ptr->Set(stream.GetFilename());
			value_ptr->SetCompressed(dataTransfer.isCompressed());
			value_ptr->SetDecodedLengthUndefined(length);
			AddValue(value_ptr);
		}
		break;

		case ATTR_VR_OL: 
		{
			// initialise the bits allocated
			UINT16 bits_allocated = 32;

			// try to get the parent in order to determine if this is pixel data or not
			if (parentM_ptr)
			{
				if ((GetGroup() == PIXEL_GROUP) &&
					(GetElement() == PIXEL_DATA))
				{
					// get the actual bits allocated
					parentM_ptr->getUSValue(TAG_BITS_ALLOCATED, &bits_allocated);
				}
			}

			// set up the data stream
			OL_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);
			stream.SetBitsAllocated(bits_allocated);
			stream.SetLength(length);

			// stream the other data from the data transfer
			if (!stream.StreamFrom(dataTransfer))
			{
				if (loggerM_ptr)
				{
                    if (length == UNDEFINED_LENGTH)
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OL data for Attribute (%04X,%04X) with UNDEFINED length: 0x%X", group, element, length);
                    }
                    else
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OL data for Attribute (%04X,%04X). Length expected: 0x%X=%d", group, element, length, length);
                    }
				}

				// should never fail
				return false;
			}

			// save OW data value
			VALUE_OL_CLASS *value_ptr = (VALUE_OL_CLASS*)CreateNewValue(vr);
            value_ptr->SetLogger(loggerM_ptr);
			value_ptr->Set(stream.GetFilename());
			value_ptr->SetCompressed(dataTransfer.isCompressed());
			value_ptr->SetDecodedLengthUndefined(length);
			AddValue(value_ptr);
		}
		break;
		
	case ATTR_VR_OV:
		{
			// set up the data stream
			OV_VALUE_STREAM_CLASS stream;
			stream.SetLogger(loggerM_ptr);
			stream.SetLength(length);

			// stream the other data from the data transfer
			if (!stream.StreamFrom(dataTransfer))
			{
				if (loggerM_ptr)
				{
                    if (length == UNDEFINED_LENGTH)
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OV data for Attribute (%04X,%04X) with UNDEFINED length: 0x%X", group, element, length);
                    }
                    else
                    {
    					loggerM_ptr->text(LOG_ERROR, 1, "Cannot read all OV data for Attribute (%04X,%04X). Length expected: 0x%X=%d", group, element, length, length);
                    }
				}

				// should never fail
				return false;
			}

			// save other data value
			VALUE_OV_CLASS *value_ptr = (VALUE_OV_CLASS*)CreateNewValue(vr);
            value_ptr->SetLogger(loggerM_ptr);
			value_ptr->Set(stream.GetFilename());
			value_ptr->SetDecodedLengthUndefined(length);
			AddValue(value_ptr);
		}
		break;

	case ATTR_VR_SL: 
		{
			// decode attribute SL value(s)
			// attribute length must be a multiple of VR length
			if ((length % SL_LENGTH) != 0) return false;

			while (length) 
			{
				INT32 value;
				dataTransfer >> value;
				
				// save single attribute value
				BASE_VALUE_CLASS *value_ptr = CreateNewValue(vr);
				value_ptr->Set(value);
				AddValue(value_ptr);
				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_SQ:
		{
			// decode attribute SQ value(s) - may involve recursion
			DCM_VALUE_SQ_CLASS *sqValue_ptr = new DCM_VALUE_SQ_CLASS(length);

			// cascade the logger
			sqValue_ptr->setLogger(loggerM_ptr);

			// cascade the private attribute handler
			sqValue_ptr->setPAH(pahM_ptr);

            // link parent
            sqValue_ptr->setParent(this);

			// decode the sequence data
			result = sqValue_ptr->decode(dataTransfer);
			definedLengthM = sqValue_ptr->isDefinedLength();
			*length_ptr = sqValue_ptr->GetLength();

			// save the attribute value
			addSqValue(sqValue_ptr);
		}
		break;

	case ATTR_VR_SS: 
		{
			// decode attribute SS value(s)
			// attribute length must be a multiple of VR length
			if ((length % SS_LENGTH) != 0) return false;

			while (length) 
			{
				INT16 value;
				dataTransfer >> value;
				
				// save single attribute value
				BASE_VALUE_CLASS *value_ptr = CreateNewValue(vr);
				value_ptr->Set(value);
				AddValue(value_ptr);
				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_UL: 
		{
			// decode attribute UL value(s)
			// attribute length must be a multiple of VR length
			if ((length % UL_LENGTH) != 0) return false;

			while (length) 
			{
				UINT32 value;
				dataTransfer >> value;

				// save single attribute value
				DCM_VALUE_UL_CLASS *value_ptr = new DCM_VALUE_UL_CLASS();
				value_ptr->Set(value);
				AddValue(value_ptr);
				length -= sizeof(value);
			}
		}
		break;

	case ATTR_VR_US: 
		{
			// decode attribute US value(s)
			// attribute length must be a multiple of VR length
			if ((length % US_LENGTH) != 0) return false;

			while (length) 
			{
				UINT16 value;
				dataTransfer >> value;
				
				// save single attribute value
				BASE_VALUE_CLASS *value_ptr = CreateNewValue(vr);
				value_ptr->Set(value);
				AddValue(value_ptr);
				length -= sizeof(value);
			}

			// need special check for pixel representation
			if ((group == GROUP_TWENTY_EIGHT) &&
				(element == PIXEL_REPRESENTATION))
			{
				// save first value in parent
				BASE_VALUE_CLASS *value_ptr = GetValue(0);
				if (value_ptr != NULL)
				{
					UINT16 pixelRepresentation;
					value_ptr->Get(pixelRepresentation);
					if (parentM_ptr)
					{
						parentM_ptr->setPixelRepresentation(pixelRepresentation);
					}
				}
			}
		}
		break;

	default:
		return false;
		break;
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_CLASS::findBackslash(BYTE *buffer, UINT length, UINT start, UINT *end_ptr)

//  DESCRIPTION     : find backslashes in a string, considering extended characters when needed
//  PRECONDITIONS   : buffer	- buffer to look in
//					: length	- length of the buffer
//					: start		- position in the buffer to start the search
//					: end_ptr	- return value of where the search ended
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : returns true if a backslash is found
//					: does not use the character set definitions, but uses the ISO
//					: character set rules
//<<===========================================================================
{
	UINT end; // where we are looking for the slash

	ATTR_VR_ENUM vr = GetVR();
	switch (vr)
	{
	case ATTR_VR_LO: // extended character VRs that support multiple values per attribute
	case ATTR_VR_PN:
	case ATTR_VR_SH:
		{
			bool glMultiByte = false; // indicates if the character set currently active in GL is multibyte 
			bool grMultiByte = false; // indicates if the character set currently active in GR is multibyte 

			for (end = start; end < length; end++ ) 
			{
				if (buffer[end] == ESCAPE) 
				{
					UINT seqLength;
					bool gr;
					bool multibyte;

					// Start of an escape sequence
					if (decodeEscSeq(&(buffer[end+1]),(length - end - 1), seqLength, gr, multibyte))
					{
						if (gr)
						{
							grMultiByte = multibyte;
						}
						else
						{
							glMultiByte = multibyte;
						}
						end += seqLength; // skip over the escape sequence
					}
				}
				else
				{
					bool multibyte;

					// determine if the character is in GL or GR
					if ((buffer[end] & 0x80) == 0)
					{
						multibyte = glMultiByte;
					}
					else
					{
						multibyte = grMultiByte;
					}

					if (multibyte) 
					{
						// Two byte character
						if ((end+1) < length)
						{
							end++;
						}
						else
						{
							// ended in the middle of a 2 byte character - just exit
						}
					}
					else if (((buffer[end] == CARET) && (vr == ATTR_VR_PN))
						  || ((buffer[end] == EQUALCHAR) && (vr == ATTR_VR_PN))
						  || (buffer[end] == LINEFEED)
						  || (buffer[end] == FORMFEED)
						  || (buffer[end] == CARRIAGERETURN) )
					{
						// Control characters and delimiters - forces the character set to just the base set
						glMultiByte = false;
						grMultiByte = false;
					}
					else 
					{
						// single byte character
						if (buffer[end] == BACKSLASH)
						{
							*end_ptr = end;
							return true;
						}
					}
				}
			}
		}
		break;

	default:
		{
			for (end = start; end < length; end++ ) 
			{
				if (buffer[end] == BACKSLASH)
				{
					*end_ptr = end;
					return true;
				}
			}
		}
		break;
	}

	*end_ptr = end;
	return false;
}
