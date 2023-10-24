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



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Represent the logical NOT that is applied to another boolean expressions.
	/// </summary>
	public class ConditionNot: Condition
	{
        //
        // - Fields -
        //

        /// <summary>
        /// The boolean expression, supplied in the constructor, on which the logical NOT operator is applied.
        /// </summary>
		private Condition condition = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
		private ConditionNot()
		{
			// Do nothing.
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="condition">The boolean expression on which the logical NOT operator is applied.</param>
        public ConditionNot(Condition condition)
		{
			this.condition = condition;
		}



        //
        // - Methods -
        //

        /// <summary>
        /// Evaluates the boolean expression using the supplied instance.
        /// </summary>
        /// <param name="theObject">The supplied instance to evaluate the boolean expression with.</param>
        /// <returns>The result of evaluating the boolean expression with the supplied instance.</returns>
		public override bool Evaluate(Object theObject)
		{
			return(!condition.Evaluate(theObject));
		}
	}
}
