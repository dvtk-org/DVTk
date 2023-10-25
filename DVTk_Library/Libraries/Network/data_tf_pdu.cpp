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
#include "data_tf_pdu.h"
#include "Ilog.h"				// Log interface component


//>>===========================================================================

DATA_TF_PDU_CLASS::DATA_TF_PDU_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	pduTypeM = PDU_PDATA;
	currentPdvM.lengthM = 0;
	currentPdvM.pcIdM = 0;
	currentPdvM.mchM = 0;
	currentPdvM.dataM = NULL;
	loggerM_ptr = NULL;
}
	
//>>===========================================================================

DATA_TF_PDU_CLASS::DATA_TF_PDU_CLASS(UINT32 maxLength)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	pduTypeM = PDU_PDATA;

	// check for an undefined length PDU
	if (maxLength == 0)
	{
		// set maximum length to the DVT "infinite" length
		maxLength = INFINITE_MAXIMUM_LENGTH_RECEIVED;
	}
	else if (maxLength > INFINITE_MAXIMUM_LENGTH_RECEIVED)
	{
		// set maximum length to the DVT "infinite" length
		maxLength = INFINITE_MAXIMUM_LENGTH_RECEIVED;
	}
	else if (maxLength < MINIMUM_LENGTH_RECEIVED)
	{
		// force minimum PDU size if necessary
		maxLength = MINIMUM_LENGTH_RECEIVED;
	}

	// reduce maximum length by PDU header size
	maxLength -= (sizeof(pduTypeM) + sizeof(reservedM) + sizeof(lengthM));
	allocateBody(maxLength);

	// initialise PDV length
	currentPdvM.lengthM = maxLength - sizeof(currentPdvM.lengthM);
	currentPdvM.pcIdM = 0;
	currentPdvM.mchM = 0;

	// set PDU offset to beginning of space reserved for DICOM data
	setOffset(sizeof(currentPdvM.lengthM) + sizeof(currentPdvM.pcIdM) + sizeof(currentPdvM.mchM));
	currentPdvM.dataM =  getData() + getOffset();

	loggerM_ptr = NULL;
}

//>>===========================================================================

DATA_TF_PDU_CLASS::~DATA_TF_PDU_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up resources
}

//>>===========================================================================

bool DATA_TF_PDU_CLASS::moveToFirstPdv(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Move index to first PDV in PDU and set up currentPdvM content.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the PDU index to the beginning
	setOffset(0);

	// set up first PDV content
	(*this) >> currentPdvM.lengthM;
	(*this) >> currentPdvM.pcIdM;
	(*this) >> currentPdvM.mchM;
	currentPdvM.dataM = getData() + getOffset();

	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 1, "DATA_TF_PDU_CLASS::moveToFirstPdv()");
		logger_ptr->text(LOG_DEBUG, 1, "P-DATA-TF PDU offset: %d", getOffset());
		logger_ptr->text(LOG_DEBUG, 1, "PDV length: %d", currentPdvM.lengthM);
		logger_ptr->text(LOG_DEBUG, 1, "PDV Pc Id: %d", currentPdvM.pcIdM);
		logger_ptr->text(LOG_DEBUG, 1, "PDV Mch: %d", currentPdvM.mchM);
	}

	// return result
	return true;
}

//>>===========================================================================

bool DATA_TF_PDU_CLASS::moveToNextPdv(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Move index to next PDV in PDU and set up currentPdvM content.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// compute next PDV index in PDU
	UINT32 offset = getOffset() - sizeof(currentPdvM.pcIdM) - sizeof(currentPdvM.mchM) + currentPdvM.lengthM;

	// check if we have now reached the end of the PDU
	if (offset < getLength())
	{
		// move to next PDV
		setOffset(offset);

		// set up next PDV content
		(*this) >> currentPdvM.lengthM;
		(*this) >> currentPdvM.pcIdM;
		(*this) >> currentPdvM.mchM;
		currentPdvM.dataM = getData() + getOffset();

		if (logger_ptr)
		{
			logger_ptr->text(LOG_DEBUG, 1, "DATA_TF_PDU_CLASS::moveToNextPdv()");
			logger_ptr->text(LOG_DEBUG, 1, "P-DATA-TF PDU offset: %d", getOffset());
			logger_ptr->text(LOG_DEBUG, 1, "PDV length: %d", currentPdvM.lengthM);
			logger_ptr->text(LOG_DEBUG, 1, "PDV Pc Id: %d", currentPdvM.pcIdM);
			logger_ptr->text(LOG_DEBUG, 1, "PDV Mch: %d", currentPdvM.mchM);
		}

		result = true;
	}

	// return result
	return result;
}

//>>===========================================================================

bool DATA_TF_PDU_CLASS::updateFirstPdv(UINT32 length, BYTE pcId, BYTE messageControlHeader)

//  DESCRIPTION     : Update the first PDV data in the PDU.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// update the pvd values
	currentPdvM.lengthM = length + sizeof(currentPdvM.pcIdM) + sizeof(currentPdvM.mchM);
	currentPdvM.pcIdM = pcId;
	currentPdvM.mchM = messageControlHeader;

	// set the PDU index to the beginning
	setOffset(0);

	// set the first PDV content in the PDU
	(*this) << currentPdvM.lengthM;
	(*this) << currentPdvM.pcIdM;
	(*this) << currentPdvM.mchM;

	// return result
	return true;
}

//>>===========================================================================

bool DATA_TF_PDU_CLASS::readBody(BASE_IO_CLASS *io_ptr)

//  DESCRIPTION     : Read P-DATA-TF PDU Body from IO.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (io_ptr == NULL) return false;

	// read PDU using underlying base class
	if (!PDU_CLASS::readBody(io_ptr)) return false;

	// set up initial PDV address
	return moveToFirstPdv();
}

//>>===========================================================================

bool DATA_TF_PDU_CLASS::isThereMorePdvData()

//  DESCRIPTION     : Check if the PDU contains more PDV after current offset.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool morePDVs = false;

	// compute next PDV index in PDU
	UINT32 offset = getOffset() - sizeof(currentPdvM.pcIdM) - sizeof(currentPdvM.mchM) + currentPdvM.lengthM;

		// check if we have now reached the end of the PDU
	if (offset < getLength())
	{
		// not yet reached end of PDU
		morePDVs = true;
	}

	// return more PDVs
	return morePDVs;
}

//>>===========================================================================

bool DATA_TF_PDU_CLASS::isLast()

//  DESCRIPTION     : Check if PDU contains either the last Command or Dataset PDV.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           : There may be more than one PDV in this PDU - hence loop.
//<<===========================================================================
{
	bool lastFragment = false;

	// save the current offset
	UINT32 offset = getOffset();

	// start at first PDV
	moveToFirstPdv();

	// check if this is that last fragment
	do
	{
		if (IsThisTheLastFragment(currentPdvM.mchM))
		{
			lastFragment = true;
			break;
		}
	}
	while (moveToNextPdv());

	// reset the offset
	setOffset(offset);

	// return flag indicating if this is the last fragment
	return lastFragment;
}

//>>===========================================================================

bool DATA_TF_PDU_CLASS::moveToLastPdv()

//  DESCRIPTION     : Move to the last PDV in this PDU.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           : There may be more than one PDV in this PDU - hence loop.
//<<===========================================================================
{
	// start at first PDV
	moveToFirstPdv();

	// move along to the last one
	bool notLastPdv = true;
	while (notLastPdv == true)
	{
		// a side-effect of the moveToNextPdv method is that when it finally
		// returns false it will be pointing to the last PDV in this PDU.
		notLastPdv = moveToNextPdv();
	}

	return true;
}

