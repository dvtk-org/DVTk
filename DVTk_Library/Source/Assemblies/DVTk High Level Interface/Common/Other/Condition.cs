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



using System;
using System.Collections;



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
	/// Represents a boolean expression that can evaluate another instance.
	/// </summary>
	abstract public class Condition
	{
        //
        // - Constructors -
        //

        /// <summary>
        /// Default constructor.
        /// </summary>
		public Condition()
		{
		}



        //
        // - Methods -
        //

        /// <summary>
        /// Returns a boolean expression which is the logical AND operation performed on the two supplied boolean expressions.
        /// </summary>
        /// <param name="condition1">The first boolean expression.</param>
        /// <param name="condition2">The second boolean expression.</param>
        /// <returns>The boolean expression which is the logical AND operation performed on the two supplied boolean expressions.</returns>
        public static Condition And(Condition condition1, Condition condition2)
        {
            return (new ConditionAnd(condition1, condition2));
        }

        /// <summary>
        /// Evaluates the boolean expression using the supplied instance.
        /// </summary>
        /// <param name="theObject">The supplied instance to evaluate the boolean expression with.</param>
        /// <returns>The result of evaluating the boolean expression with the supplied instance.</returns>
        public abstract bool Evaluate(Object theObject);

        /// <summary>
        /// Returns a boolean expression which is the logical NOT operation performed on the supplied boolean expression.
        /// </summary>
        /// <param name="condition">The first boolean expression.</param>
        /// <returns>The boolean expression which is the logical NOT operation performed on the supplied boolean expression.</returns>
        public static Condition Not(Condition condition)
        {
            return (new ConditionNot(condition));
        }

        /// <summary>
        /// Returns a boolean expression which is the logical OR operation performed on the two supplied boolean expressions.
        /// </summary>
        /// <param name="condition1">The first boolean expression.</param>
        /// <param name="condition2">The second boolean expression.</param>
        /// <returns>The boolean expression which is the logical OR operation performed on the two supplied boolean expressions.</returns>
        public static Condition Or(Condition condition1, Condition condition2)
		{
			return(new ConditionOr(condition1, condition2));
		}

        /// <summary>
        /// Evaluates all instances that are present in the supplied collection with this boolean
        /// expression and returns those instances that evaluate to true.
        /// </summary>
        /// <param name="theCollection">The instances to evaluate with this boolean expression.</param>
        /// <returns>All instances that evaluate to true with this boolean expression.</returns>
		public ArrayList Filter(ICollection theCollection)
		{
			ArrayList filteredCollection = new ArrayList();

			foreach (Object theObject in theCollection)
			{
				if (Evaluate(theObject))
				{
					filteredCollection.Add(theObject);
				}
			}

			return(filteredCollection);
		}
	}
}
