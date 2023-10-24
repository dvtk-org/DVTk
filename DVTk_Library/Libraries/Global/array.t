//*****************************************************************************
//  FILENAME        :	ARRAY.THH
//  PACKAGE         :	DVT
//  COMPONENT       :	INCLUDE
//  DESCRIPTION     :	Array Template Class
//  COPYRIGHT(c)    :   2000, Philips Electronics N.V.
//                      2000, Agfa Gevaert N.V.
//*****************************************************************************
#ifndef ARRAY_THH
#define ARRAY_THH

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

//>>***************************************************************************


//  DESCRIPTION     :
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************


/*********************************************************************
 * Array Unit Class
 *
 * ANSI (ARM) C++ Compatible / Templates Required
 *
 *
 * usage:
 *
 *	# include "array.thh"
 *	# include "array.tcc"
 *
 *
 *	Array<ClassType>  VarName;
 *
 * 
 *********************************************************************/
//
// DATA_LINK_CLASS
//
template <class DATATYPE>

class DATA_LINK_CLASS
{
public:
	DATATYPE					dataM;
	DATA_LINK_CLASS<DATATYPE>	*prevM_ptr, *nextM_ptr;
					
	DATA_LINK_CLASS(DATA_LINK_CLASS<DATATYPE> *p_ptr, DATA_LINK_CLASS<DATATYPE> *n_ptr) 
		{ prevM_ptr = p_ptr; nextM_ptr = n_ptr; }
	DATA_LINK_CLASS() 
		{ prevM_ptr = NULL; nextM_ptr = NULL; }
};


//
// ARRAY
//
template <class	DATATYPE>

class ARRAY
{
public:
	DATA_LINK_CLASS<DATATYPE>	*firstM_ptr;	
	UINT						arraySizeM;
	bool						clearTypeM;

	ARRAY() 
		{ arraySizeM = 0; 
		firstM_ptr = NULL; 
		clearTypeM = true; }

	ARRAY(bool);

	virtual	~ARRAY();

	inline DATATYPE& operator [] (UINT i) 
		{ return get(i); }

	inline UINT	getSize() 
		{ return arraySizeM; }

	DATATYPE& add(DATATYPE&);

	DATATYPE& get(UINT);

	bool removeAt(UINT);

	void operator =	(ARRAY<DATATYPE> &array)
		{ firstM_ptr = array.firstM_ptr; 
		arraySizeM = array.arraySizeM; 
		clearTypeM = false; }
};


#endif /* ARRAY_THH */


