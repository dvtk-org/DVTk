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
//  FILENAME        : ConditionNode.h
//  PACKAGE         : DVT
//  COMPONENT       : DEFINITION 
//  DESCRIPTION     : Condition Node Classes
//  COPYRIGHT(c)    : 2000, Philips Electronics N.V.
//                    2000, Agfa Gevaert N.V.
//*****************************************************************************
#ifndef CONDITION_NODE_H
#define CONDITION_NODE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_CLASS;
class ATTRIBUTE_GROUP_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

enum SEARCH_DIRECTION_ENUM
{
	SEARCH_DIRECTION_ALL,	// Doesn't matter, anywhere in object or nested objects
	SEARCH_DIRECTION_UP,    // Only in the parent object of the attribute
	SEARCH_DIRECTION_DOWN,	// Only in the child object of the attribute
	SEARCH_DIRECTION_HERE	// Only in the object on this level.
};

// define structure for search direction mapping
struct T_SEARCH_DIRECTION_MAP
{
	SEARCH_DIRECTION_ENUM direction;
	char*                 directionName;
};

#define MAX_CONDITION_LENGTH 1024


//>>***************************************************************************

class DEF_COND_NODE_CLASS

//  DESCRIPTION     : Abstract Base Node Class
//                    Defines a node used for condition specification
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	private:
		LOG_CLASS *loggerM_ptr;

	protected:
		string result_messageM;

	public:
		virtual ~DEF_COND_NODE_CLASS();

		virtual bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*) = 0;

		virtual string GetResultMessage();

		void AddMessage(char* format_ptr, ...);
	
		void ClearMessage();

		void SetLogger(LOG_CLASS* logger_ptr) { loggerM_ptr = logger_ptr; }

		LOG_CLASS* GetLogger() { return loggerM_ptr; }

		virtual void Log(LOG_CLASS*) = 0;
};

//>>***************************************************************************

class DEF_COND_BINARY_NODE_CLASS : public DEF_COND_NODE_CLASS

//  DESCRIPTION     : Binary Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_BINARY_NODE_CLASS();
		DEF_COND_BINARY_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		virtual ~DEF_COND_BINARY_NODE_CLASS();

		virtual bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void SetLeft(DEF_COND_NODE_CLASS* left_ptr);
		void SetRight(DEF_COND_NODE_CLASS* right_ptr);

		DEF_COND_NODE_CLASS* GetLeft() { return leftM_ptr; };
		DEF_COND_NODE_CLASS* GetRight() { return rightM_ptr; };

		virtual void Log(LOG_CLASS*);

	private:
		DEF_COND_NODE_CLASS* leftM_ptr;
		DEF_COND_NODE_CLASS* rightM_ptr;
};

//>>***************************************************************************

class DEF_COND_OR_NODE_CLASS : public DEF_COND_BINARY_NODE_CLASS

//  DESCRIPTION     : OR Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_OR_NODE_CLASS();
		DEF_COND_OR_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		~DEF_COND_OR_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_AND_NODE_CLASS : public DEF_COND_BINARY_NODE_CLASS

//  DESCRIPTION     : AND Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_AND_NODE_CLASS();
		DEF_COND_AND_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		~DEF_COND_AND_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_PRESENT_NODE_CLASS : public DEF_COND_BINARY_NODE_CLASS

//  DESCRIPTION     : PRESENT Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_PRESENT_NODE_CLASS();
		DEF_COND_PRESENT_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		~DEF_COND_PRESENT_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_VALUE_NODE_CLASS : public DEF_COND_BINARY_NODE_CLASS

//  DESCRIPTION     : VALUE Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_VALUE_NODE_CLASS();
		DEF_COND_VALUE_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		~DEF_COND_VALUE_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		UINT16 GetGroup();  
		UINT16 GetElement();
		UINT16 GetValueNr();

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_LESS_NODE_CLASS : public DEF_COND_BINARY_NODE_CLASS

//  DESCRIPTION     : < Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_LESS_NODE_CLASS();
		DEF_COND_LESS_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		~DEF_COND_LESS_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_LESS_EQ_NODE_CLASS : public DEF_COND_BINARY_NODE_CLASS

//  DESCRIPTION     : <= Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_LESS_EQ_NODE_CLASS();
		DEF_COND_LESS_EQ_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		~DEF_COND_LESS_EQ_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_EQ_NODE_CLASS : public DEF_COND_BINARY_NODE_CLASS

//  DESCRIPTION     : = Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_EQ_NODE_CLASS();
		DEF_COND_EQ_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		~DEF_COND_EQ_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_GREATER_EQ_NODE_CLASS : public DEF_COND_BINARY_NODE_CLASS

//  DESCRIPTION     : >= Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_GREATER_EQ_NODE_CLASS();
		DEF_COND_GREATER_EQ_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		~DEF_COND_GREATER_EQ_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_GREATER_NODE_CLASS : public DEF_COND_BINARY_NODE_CLASS

//  DESCRIPTION     : > Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_GREATER_NODE_CLASS();
		DEF_COND_GREATER_NODE_CLASS(DEF_COND_NODE_CLASS*, DEF_COND_NODE_CLASS*);
		~DEF_COND_GREATER_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_UNARY_NODE_CLASS : public DEF_COND_NODE_CLASS

//  DESCRIPTION     : Unary Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_UNARY_NODE_CLASS();
		DEF_COND_UNARY_NODE_CLASS(DEF_COND_NODE_CLASS*);
		~DEF_COND_UNARY_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);

	protected:
		DEF_COND_NODE_CLASS* nodeM_ptr;
};

//>>***************************************************************************

class DEF_COND_EMPTY_NODE_CLASS : public DEF_COND_UNARY_NODE_CLASS

//  DESCRIPTION     : EMPTY Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_EMPTY_NODE_CLASS();
		DEF_COND_EMPTY_NODE_CLASS(DEF_COND_NODE_CLASS*);
		~DEF_COND_EMPTY_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_NOT_NODE_CLASS : public DEF_COND_UNARY_NODE_CLASS

//  DESCRIPTION     : NOT Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_NOT_NODE_CLASS();
		DEF_COND_NOT_NODE_CLASS(DEF_COND_NODE_CLASS*);
		~DEF_COND_NOT_NODE_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class DEF_COND_LEAF_CLASS : public DEF_COND_NODE_CLASS

//  DESCRIPTION     : Abstract Base Leaf Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		virtual ~DEF_COND_LEAF_CLASS();

		virtual bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*) = 0;

		virtual void Log(LOG_CLASS*) = 0;
};

//>>***************************************************************************

class DEF_COND_LEAF_TAG_CLASS : public DEF_COND_LEAF_CLASS

//  DESCRIPTION     : Leaf Attribute Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_LEAF_TAG_CLASS();
		DEF_COND_LEAF_TAG_CLASS(UINT16, UINT16);
		~DEF_COND_LEAF_TAG_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		UINT16 GetGroup()   { return tag_groupM; }
		UINT16 GetElement() { return tag_elementM; }

		void Log(LOG_CLASS*);

	private:
		UINT16	tag_groupM;
		UINT16	tag_elementM;
};

//>>***************************************************************************

class DEF_COND_LEAF_CONST_CLASS : public DEF_COND_LEAF_CLASS

//  DESCRIPTION     :
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_LEAF_CONST_CLASS();
		DEF_COND_LEAF_CONST_CLASS(string);
		~DEF_COND_LEAF_CONST_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		string GetConstant() { return constantM; }

		void Log(LOG_CLASS*);

	private:
		string   constantM;
};

//>>***************************************************************************

class DEF_COND_LEAF_VALUE_NR_CLASS : public DEF_COND_LEAF_CLASS

//  DESCRIPTION     : Ocurrence Leaf Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_LEAF_VALUE_NR_CLASS();
		DEF_COND_LEAF_VALUE_NR_CLASS(UINT16);
		~DEF_COND_LEAF_VALUE_NR_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		UINT16 GetValueNr() { return value_nrM; }

		void Log(LOG_CLASS*);

	private:
		UINT16	value_nrM;
};

//>>***************************************************************************

class DEF_COND_LEAF_DIRECTION_CLASS : public DEF_COND_LEAF_CLASS

//  DESCRIPTION     : Search Direction Leaf Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		DEF_COND_LEAF_DIRECTION_CLASS();
		DEF_COND_LEAF_DIRECTION_CLASS(SEARCH_DIRECTION_ENUM);
		~DEF_COND_LEAF_DIRECTION_CLASS();

		bool Evaluate(ATTRIBUTE_GROUP_CLASS*, ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		SEARCH_DIRECTION_ENUM GetSearchDirection() { return directionM;}

		void Log(LOG_CLASS*);

	private:
		SEARCH_DIRECTION_ENUM	directionM;
};

#endif /* CONDITION_NODE_H */
