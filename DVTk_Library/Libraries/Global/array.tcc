//*****************************************************************************
//  FILENAME        :	ARRAY.TCC
//  PACKAGE         :	DVT
//  COMPONENT       :	INCLUDE
//  DESCRIPTION     :	Array Template Class.
//  COPYRIGHT(c)    :   2000, Philips Electronics N.V.
//                      2000, Agfa Gevaert N.V.
//*****************************************************************************
#ifndef ARRAY_TCC
#define ARRAY_TCC

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************


//*****************************************************************************
//  INTERNAL DECLARATIONS
//*****************************************************************************
namespace
{

}

//*****************************************************************************
//  INTERNAL DEFINITIONS
//*****************************************************************************
namespace
{

}

namespace
{
	//=========================================================================
	//  DESCRIPTION :
	//  NOTES       :
	//=========================================================================
}

//*****************************************************************************
//  EXTERNAL DEFINITIONS
//*****************************************************************************

//>>===========================================================================



//  DESCRIPTION     :
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================


template <class DATATYPE>
ARRAY<DATATYPE>::ARRAY(bool clearType)
{
	arraySizeM = 0;
	firstM_Ptr = NULL;
	clearTypeM = clearType;
}


template <class DATATYPE>
ARRAY<DATATYPE>::~ARRAY()
{
	if (clearTypeM)
	{
		while(arraySizeM)
			removeAt(0);
	}
}


template <class DATATYPE>
DATATYPE& ARRAY<DATATYPE>::add(DATATYPE &value)
{
	DATA_LINK_CLASS<DATATYPE>	*dl_ptr;

	if (!firstM_ptr)
	{
		firstM_ptr = new DATA_LINK_CLASS<DATATYPE>;
		if (!firstM_ptr)
		{
			// Returning NULL Data
			dl_ptr = NULL;
			return (DATATYPE &) *dl_ptr;	// invoke seg fault
		}

		firstM_ptr->dataM = value;
		arraySizeM++;

		return firstM_ptr->dataM;
	}

	dl_ptr = firstM_ptr;
	while(dl_ptr->nextM_ptr)
		dl_ptr = dl_ptr->nextM_ptr;

	dl_ptr->nextM_ptr = new DATA_LINK_CLASS<DATATYPE>;
	if (!dl_ptr->nextM_ptr)
	{
		// Returning NULL Data
		return (DATATYPE &) *dl_ptr;	// invoke seg fault
	}

	dl_ptr->nextM_ptr->prevM_ptr = dl_ptr;
	dl_ptr->nextM_ptr->dataM = value;
	arraySizeM++;

	return dl_ptr->nextM_ptr->dataM;
}


template <class DATATYPE>
DATATYPE & ARRAY<DATATYPE>::get(UINT i)
{
	DATA_LINK_CLASS<DATATYPE>	*dl_ptr;

	if (i >= arraySizeM)
	{
		// Returning NULL Data
		dl_ptr = NULL;
		return (DATATYPE &) *dl_ptr;	// Invoke a seg fault
	}

	dl_ptr = firstM_ptr;
	while(i)
	{
		dl_ptr = dl_ptr->nextM_ptr;
		i--;
	}

	return dl_ptr->dataM;
}


template <class	DATATYPE>
bool ARRAY<DATATYPE>::removeAt(UINT i)
{
	DATA_LINK_CLASS<DATATYPE>	*dl_ptr;

	if (i >= arraySizeM)
	{
		// Attempting to remove non-existance node
		return false;
	}

	dl_ptr = firstM_ptr;
	if (!i)
	{
		if (firstM_ptr->nextM_ptr)
		{
			firstM_ptr = firstM_ptr->nextM_ptr;
			firstM_ptr->prevM_ptr = NULL;
		}
		else
			firstM_ptr = NULL;

		delete dl_ptr;		
		arraySizeM--;

		return true;
	}

	while(i)
	{
		dl_ptr = dl_ptr->nextM_ptr;
		i--;
	}

	if (dl_ptr->prevM_ptr)
		dl_ptr->prevM_ptr->nextM_ptr = dl_ptr->nextM_ptr;

	if (dl_ptr->nextM_ptr)
		dl_ptr->nextM_ptr->prevM_ptr = dl_ptr->prevM_ptr;

	delete dl_ptr;
	arraySizeM--;

	return true;
}

#endif /* ARRAY_TCC */


