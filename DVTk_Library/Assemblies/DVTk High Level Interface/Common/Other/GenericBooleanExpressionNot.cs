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
using System.Text;



namespace DvtkHighLevelInterface.Common.Other
{
    /// <summary>
    /// Represent the logical NOT that is applied to another boolean expressions.
    /// <br></br><br></br>
    /// STILL WORK IN PROGRESS!
    /// </summary>
    /// <typeparam name="T">The type of the instance to evaluate.</typeparam>
    /// <typeparam name="TCollection">A collection of the type specified.</typeparam>
    internal class GenericBooleanExpressionNot<T, TCollection> : GenericBooleanExpression<T, TCollection>
        where TCollection : IList, new()
    {
        //
        // - Fields -
        //

        /// <summary>
        /// The boolean expression, supplied in the constructor, on which the logical NOT operator is applied.
        /// </summary>
        private GenericBooleanExpression<T, TCollection> booleanExpression = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private GenericBooleanExpressionNot()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="booleanExpression">The boolean expression on which the logical AND operator is applied.</param>
        public GenericBooleanExpressionNot(GenericBooleanExpression<T, TCollection> booleanExpression)
        {
            this.booleanExpression = booleanExpression;
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Evaluates the boolean expression using the supplied instance.
        /// </summary>
        /// <param name="t">The instance to evaluate this boolean expression with.</param>
        /// <returns>The result of evaluating the boolean expression with the supplied instance.</returns>
        public override bool Evaluate(T t)
        {
            return(!this.booleanExpression.Evaluate(t)); 
        }
    }



    /// <summary>
    /// Represent the logical OR that is applied to another boolean expressions.
    /// <br></br><br></br>
    /// STILL WORK IN PROGRESS!
    /// </summary>
    /// <typeparam name="T1">The type of the first instance of the pair to evaluate.</typeparam>
    /// <typeparam name="TCollection1">A collection of the first type specified.</typeparam>
    /// <typeparam name="T2">The type of the second instance of the pair to evaluate.</typeparam>
    /// <typeparam name="TCollection2">A collection of the second type specified.</typeparam>
    internal class GenericBooleanExpressionNot<T1, TCollection1, T2, TCollection2> : GenericBooleanExpression<T1, TCollection1, T2, TCollection2>
        where T1 : class
        where TCollection1 : IList, new()
        where T2 : class
        where TCollection2 : IList, new()
    {
        //
        // - Fields -
        //

        /// <summary>
        /// The boolean expression, supplied in the constructor, on which the logical NOT operator is applied.
        /// </summary>
        private GenericBooleanExpression<T1, TCollection1, T2, TCollection2> booleanExpression = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private GenericBooleanExpressionNot()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="booleanExpression">The boolean expression on which the logical NOT operator is applied.</param>
        public GenericBooleanExpressionNot(GenericBooleanExpression<T1, TCollection1, T2, TCollection2> booleanExpression)
        {
            this.booleanExpression = booleanExpression;
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Evaluates the boolean expression using the supplied instances.
        /// </summary>
        /// <param name="t1">The first supplied instance of the pair to evaluate the boolean expression with.</param>
        /// <param name="t2">The second supplied instance of the pair to evaluate the boolean expression with.</param>
        /// <returns>The result of evaluating the boolean expression with the supplied instances.</returns>
        public override bool Evaluate(T1 t1, T2 t2)
        {
            return (!this.booleanExpression.Evaluate(t1, t2));
        }
    }
}
