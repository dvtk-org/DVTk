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
class DCM_ATTRIBUTE_GROUP_CLASS;
class DCM_ATTRIBUTE_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define MAX_CONDITION_LENGTH 1024           // maximum condition text length

#define APPLY_TO_ANY_VALUE 0xFFFF           // apply condition to any attribute value

//
// Value Representation enumerates
//
enum CONDITION_TYPE_ENUM
{
	CONDITION_TYPE_NORMAL,
	CONDITION_TYPE_WARNING
};

//>>***************************************************************************

class CONDITION_NODE_CLASS

//  DESCRIPTION     : Abstract Base Node Class
//                    Defines a node used for condition specification
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	private:
		CONDITION_TYPE_ENUM conditionTypeM;
		LOG_CLASS *loggerM_ptr;

	protected:
		string result_messageM;

		CONDITION_RESULT_ENUM DetermineFinalEvaluationResult(CONDITION_RESULT_ENUM);

	public:
		virtual ~CONDITION_NODE_CLASS();

		virtual CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*) = 0;

        bool HasResultMessage();

		virtual string GetResultMessage();

		void AddMessage(char* format_ptr, ...);
	
		void ClearMessage();

		void SetConditionType(CONDITION_TYPE_ENUM conditionType) { conditionTypeM = conditionType; }

		CONDITION_TYPE_ENUM GetConditionType() { return conditionTypeM; }

		void SetLogger(LOG_CLASS* logger_ptr) { loggerM_ptr = logger_ptr; }

		LOG_CLASS* GetLogger() { return loggerM_ptr; }

		virtual void Log(LOG_CLASS*) = 0;
};

//>>***************************************************************************

class CONDITION_BINARY_NODE_CLASS : public CONDITION_NODE_CLASS

//  DESCRIPTION     : Binary Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_BINARY_NODE_CLASS();
		CONDITION_BINARY_NODE_CLASS(CONDITION_NODE_CLASS*, CONDITION_NODE_CLASS*);
		virtual ~CONDITION_BINARY_NODE_CLASS();

		virtual CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void SetLeft(CONDITION_NODE_CLASS* left_ptr);
		void SetRight(CONDITION_NODE_CLASS* right_ptr);

		CONDITION_NODE_CLASS* GetLeft() { return leftM_ptr; };
		CONDITION_NODE_CLASS* GetRight() { return rightM_ptr; };

		virtual void Log(LOG_CLASS*);

	private:
		CONDITION_NODE_CLASS* leftM_ptr;
		CONDITION_NODE_CLASS* rightM_ptr;
};

//>>***************************************************************************

class CONDITION_OR_NODE_CLASS : public CONDITION_BINARY_NODE_CLASS

//  DESCRIPTION     : OR Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_OR_NODE_CLASS();
		CONDITION_OR_NODE_CLASS(CONDITION_NODE_CLASS*, CONDITION_NODE_CLASS*);
		~CONDITION_OR_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_AND_NODE_CLASS : public CONDITION_BINARY_NODE_CLASS

//  DESCRIPTION     : AND Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_AND_NODE_CLASS();
		CONDITION_AND_NODE_CLASS(CONDITION_NODE_CLASS*, CONDITION_NODE_CLASS*);
		~CONDITION_AND_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_VALUE_NODE_CLASS : public CONDITION_BINARY_NODE_CLASS

//  DESCRIPTION     : VALUE Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_VALUE_NODE_CLASS();
		CONDITION_VALUE_NODE_CLASS(CONDITION_NODE_CLASS*, CONDITION_NODE_CLASS*);
		~CONDITION_VALUE_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		DCM_ATTRIBUTE_CLASS *GetAttribute(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);
		UINT16 GetValueNr();

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_LESS_NODE_CLASS : public CONDITION_BINARY_NODE_CLASS

//  DESCRIPTION     : < Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_LESS_NODE_CLASS();
		CONDITION_LESS_NODE_CLASS(CONDITION_NODE_CLASS*, CONDITION_NODE_CLASS*);
		~CONDITION_LESS_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_LESS_EQ_NODE_CLASS : public CONDITION_BINARY_NODE_CLASS

//  DESCRIPTION     : <= Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_LESS_EQ_NODE_CLASS();
		CONDITION_LESS_EQ_NODE_CLASS(CONDITION_NODE_CLASS*, CONDITION_NODE_CLASS*);
		~CONDITION_LESS_EQ_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_EQ_NODE_CLASS : public CONDITION_BINARY_NODE_CLASS

//  DESCRIPTION     : = Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_EQ_NODE_CLASS();
		CONDITION_EQ_NODE_CLASS(CONDITION_NODE_CLASS*, CONDITION_NODE_CLASS*);
		~CONDITION_EQ_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_GREATER_EQ_NODE_CLASS : public CONDITION_BINARY_NODE_CLASS

//  DESCRIPTION     : >= Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_GREATER_EQ_NODE_CLASS();
		CONDITION_GREATER_EQ_NODE_CLASS(CONDITION_NODE_CLASS*, CONDITION_NODE_CLASS*);
		~CONDITION_GREATER_EQ_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_GREATER_NODE_CLASS : public CONDITION_BINARY_NODE_CLASS

//  DESCRIPTION     : > Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_GREATER_NODE_CLASS();
		CONDITION_GREATER_NODE_CLASS(CONDITION_NODE_CLASS*, CONDITION_NODE_CLASS*);
		~CONDITION_GREATER_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_UNARY_NODE_CLASS : public CONDITION_NODE_CLASS

//  DESCRIPTION     : Unary Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_UNARY_NODE_CLASS();
		CONDITION_UNARY_NODE_CLASS(CONDITION_NODE_CLASS*);
		~CONDITION_UNARY_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void SetNode(CONDITION_NODE_CLASS *node_ptr) { nodeM_ptr = node_ptr; }

		CONDITION_NODE_CLASS* GetNode() { return nodeM_ptr; };

		void Log(LOG_CLASS*);

	protected:
		CONDITION_NODE_CLASS* nodeM_ptr;
};

//>>***************************************************************************

class CONDITION_EMPTY_NODE_CLASS : public CONDITION_UNARY_NODE_CLASS

//  DESCRIPTION     : EMPTY Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_EMPTY_NODE_CLASS();
		CONDITION_EMPTY_NODE_CLASS(CONDITION_NODE_CLASS*);
		~CONDITION_EMPTY_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_NOT_NODE_CLASS : public CONDITION_UNARY_NODE_CLASS

//  DESCRIPTION     : NOT Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_NOT_NODE_CLASS();
		CONDITION_NOT_NODE_CLASS(CONDITION_NODE_CLASS*);
		~CONDITION_NOT_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_PRESENT_NODE_CLASS : public CONDITION_UNARY_NODE_CLASS

//  DESCRIPTION     : PRESENT Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_PRESENT_NODE_CLASS();
		CONDITION_PRESENT_NODE_CLASS(CONDITION_NODE_CLASS*);
		~CONDITION_PRESENT_NODE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_NAVIGATION_NODE_CLASS : public CONDITION_UNARY_NODE_CLASS

//  DESCRIPTION     : Navigation Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		virtual ~CONDITION_NAVIGATION_NODE_CLASS();

        CONDITION_NAVIGATION_NODE_CLASS *GetNavigationNode();

        virtual DCM_ATTRIBUTE_CLASS *Navigate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*) = 0;

        CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		virtual void Log(LOG_CLASS*) = 0;
};

//>>***************************************************************************

class CONDITION_NAVIGATION_HERE_NODE_CLASS : public CONDITION_NAVIGATION_NODE_CLASS

//  DESCRIPTION     : Navigation Here Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_NAVIGATION_HERE_NODE_CLASS();
		~CONDITION_NAVIGATION_HERE_NODE_CLASS();

        DCM_ATTRIBUTE_CLASS *Navigate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};


//>>***************************************************************************

class CONDITION_NAVIGATION_UP_NODE_CLASS : public CONDITION_NAVIGATION_NODE_CLASS

//  DESCRIPTION     : Navigation Up Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_NAVIGATION_UP_NODE_CLASS();
		~CONDITION_NAVIGATION_UP_NODE_CLASS();

        DCM_ATTRIBUTE_CLASS *Navigate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};

//>>***************************************************************************

class CONDITION_NAVIGATION_DOWN_NODE_CLASS : public CONDITION_NAVIGATION_NODE_CLASS

//  DESCRIPTION     : Navigation Down Node Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_NAVIGATION_DOWN_NODE_CLASS(UINT16, UINT16);
		~CONDITION_NAVIGATION_DOWN_NODE_CLASS();

        DCM_ATTRIBUTE_CLASS *Navigate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		UINT16 GetGroup()   { return tag_groupM; }
		UINT16 GetElement() { return tag_elementM; }

		void Log(LOG_CLASS*);

	private:
		UINT16	tag_groupM;
		UINT16	tag_elementM;
};

//>>***************************************************************************

class CONDITION_NAVIGATION_TAG_CLASS : public CONDITION_NAVIGATION_NODE_CLASS

//  DESCRIPTION     : Navigation Attribute Tag Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_NAVIGATION_TAG_CLASS();
		CONDITION_NAVIGATION_TAG_CLASS(UINT16, UINT16);
		~CONDITION_NAVIGATION_TAG_CLASS();

        DCM_ATTRIBUTE_CLASS *Navigate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		UINT16 GetGroup()   { return tag_groupM; }
		UINT16 GetElement() { return tag_elementM; }

		void Log(LOG_CLASS*);

	private:
		UINT16	tag_groupM;
		UINT16	tag_elementM;
};

//>>***************************************************************************

class CONDITION_LEAF_CLASS : public CONDITION_NODE_CLASS

//  DESCRIPTION     : Abstract Base Leaf Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		virtual ~CONDITION_LEAF_CLASS();

		virtual CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*) = 0;

		virtual void Log(LOG_CLASS*) = 0;
};

//>>***************************************************************************

class CONDITION_LEAF_CONST_CLASS : public CONDITION_LEAF_CLASS

//  DESCRIPTION     :
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_LEAF_CONST_CLASS();
		CONDITION_LEAF_CONST_CLASS(string);
		~CONDITION_LEAF_CONST_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		string GetConstant() { return constantM; }

		void Log(LOG_CLASS*);

	private:
		string   constantM;
};

//>>***************************************************************************

class CONDITION_LEAF_VALUE_NR_CLASS : public CONDITION_LEAF_CLASS

//  DESCRIPTION     : Ocurrence Leaf Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_LEAF_VALUE_NR_CLASS();
		CONDITION_LEAF_VALUE_NR_CLASS(UINT16);
		~CONDITION_LEAF_VALUE_NR_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		UINT16 GetValueNr() { return value_nrM; }

		void Log(LOG_CLASS*);

	private:
		UINT16	value_nrM;
};

//>>***************************************************************************

class CONDITION_LEAF_TRUE_CLASS : public CONDITION_LEAF_CLASS

//  DESCRIPTION     : Leaf True Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		CONDITION_LEAF_TRUE_CLASS(bool);
		~CONDITION_LEAF_TRUE_CLASS();

		CONDITION_RESULT_ENUM Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);

	private:
		bool valueM;
};


#endif /* CONDITION_NODE_H */
