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
    /// Represents a boolean expression that can evaluate another instance.
    /// <br></br><br></br>
    /// STILL WORK IN PROGRESS!
    /// </summary>
    /// <typeparam name="T">The type of the instance to evaluate.</typeparam>
    /// <typeparam name="TCollection">A collection of the type specified.</typeparam>
    public abstract class GenericBooleanExpression<T, TCollection>
        where TCollection : IList, new()
    {
        //
        // - Constructors -
        //

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenericBooleanExpression()
        {
            // Do nothing.
        }



        //
        // - Operators -
        //

        /// <summary>
        /// Returns a boolean expression which is the logical AND operation performed on the two supplied boolean expressions.
        /// </summary>
        /// <param name="booleanExpression1">The first boolean expression.</param>
        /// <param name="booleanExpression2">The second boolean expression.</param>
        /// <returns>The boolean expression which is the logical AND operation performed on the two supplied boolean expressions.</returns>
        public static GenericBooleanExpression<T, TCollection> operator &(GenericBooleanExpression<T, TCollection> booleanExpression1, GenericBooleanExpression<T, TCollection> booleanExpression2)
        {
            return (new GenericBooleanExpressionAnd<T, TCollection>(booleanExpression1, booleanExpression2));
        }

        /// <summary>
        /// Returns a boolean expression which is the logical NOT operation performed on the supplied boolean expression.
        /// </summary>
        /// <param name="booleanExpression">The boolean expression.</param>
        /// <returns>The boolean expression which is the logical NOT operation performed on the supplied boolean expression.</returns>
        public static GenericBooleanExpression<T, TCollection> operator !(GenericBooleanExpression<T, TCollection> booleanExpression)
        {
            return (new GenericBooleanExpressionNot<T, TCollection>(booleanExpression));
        }

        /// <summary>
        /// Returns a boolean expression which is the logical OR operation performed on the two supplied boolean expressions.
        /// </summary>
        /// <param name="booleanExpression1">The first boolean expression.</param>
        /// <param name="booleanExpression2">The second boolean expression.</param>
        /// <returns>The boolean expression which is the logical OR operation performed on the two supplied boolean expressions.</returns>
        public static GenericBooleanExpression<T, TCollection> operator |(GenericBooleanExpression<T, TCollection> booleanExpression1, GenericBooleanExpression<T, TCollection> booleanExpression2)
        {
            return (new GenericBooleanExpressionOr<T, TCollection>(booleanExpression1, booleanExpression2));
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Evaluates the boolean expression using the supplied instance.
        /// </summary>
        /// <param name="t">The supplied instance to evaluate the boolean expression with.</param>
        /// <returns>The result of evaluating the boolean expression with the supplied instance.</returns>
        abstract public bool Evaluate(T t);

        /// <summary>
        /// Returns a subset of the supplied collection containing those elements that evaluate to true.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>The subset of the supplied collection.</returns>
        public TCollection Evaluate(TCollection collection)
        {
            TCollection evaluatedCollection = new TCollection();

            foreach (T element in collection)
            {
                if (Evaluate(element))
                {
                    evaluatedCollection.Add(element);
                }
            }

            return (evaluatedCollection);
        }
    }



    /// <summary>
    /// Represents a boolean expression that can evaluate a pair of two other instance.
    /// <br></br><br></br>
    /// STILL WORK IN PROGRESS!
    /// </summary>
    /// <typeparam name="T1">The type of the first instance of the pair to evaluate.</typeparam>
    /// <typeparam name="TCollection1">A collection of the first type specified.</typeparam>
    /// <typeparam name="T2">The type of the second instance of the pair to evaluate.</typeparam>
    /// <typeparam name="TCollection2">A collection of the second type specified.</typeparam>
    public abstract class GenericBooleanExpression<T1, TCollection1, T2, TCollection2>
        where T1 : class
        where TCollection1 : IList, new()
        where T2 : class
        where TCollection2 : IList, new()
    {
        //
        // - Constructors -
        //

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenericBooleanExpression()
        {
            // Do nothing.
        }



        //
        // - Operators -
        //

        /// <summary>
        /// Returns a boolean expression which is the logical AND operation performed on the two supplied boolean expressions.
        /// </summary>
        /// <param name="booleanExpression1">The first boolean expression.</param>
        /// <param name="booleanExpression2">The second boolean expression.</param>
        /// <returns>The boolean expression which is the logical AND operation performed on the two supplied boolean expressions.</returns>
        public static GenericBooleanExpression<T1, TCollection1, T2, TCollection2> operator &(GenericBooleanExpression<T1, TCollection1, T2, TCollection2> booleanExpression1, GenericBooleanExpression<T1, TCollection1, T2, TCollection2> booleanExpression2)
        {
            return (new GenericBooleanExpressionAnd<T1, TCollection1, T2, TCollection2>(booleanExpression1, booleanExpression2));
        }

        /// <summary>
        /// Returns a boolean expression which is the logical NOT operation performed on the supplied boolean expression.
        /// </summary>
        /// <param name="booleanExpression">The boolean expression.</param>
        /// <returns>The boolean expression which is the logical NOT operation performed on the supplied boolean expression.</returns>
        public static GenericBooleanExpression<T1, TCollection1, T2, TCollection2> operator !(GenericBooleanExpression<T1, TCollection1, T2, TCollection2> booleanExpression)
        {
            return (new GenericBooleanExpressionNot<T1, TCollection1, T2, TCollection2>(booleanExpression));
        }

        /// <summary>
        /// Returns a boolean expression which is the logical OR operation performed on the two supplied boolean expressions.
        /// </summary>
        /// <param name="booleanExpression1">The first boolean expression.</param>
        /// <param name="booleanExpression2">The second boolean expression.</param>
        /// <returns>The boolean expression which is the logical OR operation performed on the two supplied boolean expressions.</returns>
        public static GenericBooleanExpression<T1, TCollection1, T2, TCollection2> operator |(GenericBooleanExpression<T1, TCollection1, T2, TCollection2> booleanExpression1, GenericBooleanExpression<T1, TCollection1, T2, TCollection2> booleanExpression2)
        {
            return (new GenericBooleanExpressionOr<T1, TCollection1, T2, TCollection2>(booleanExpression1, booleanExpression2));
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
        abstract public bool Evaluate(T1 t1, T2 t2);

        /// <summary>
        /// Returns all possible combinations of the two supplied collections that evaluate to true.
        /// </summary>
        /// <param name="collection1">The first collecttion.</param>
        /// <param name="collection2">The second collecttion.</param>
        /// <returns>All possible combinations of the two supplied collections that evaluate to true.</returns>
        public GenericCollection<GenericPair<T1, T2>> Evaluate(TCollection1 collection1, TCollection2 collection2)
        {
            GenericCollection<GenericPair<T1, T2>> evaluatedPairCollection = new GenericCollection<GenericPair<T1, T2>>();

            foreach (T1 element1 in collection1)
            {
                foreach (T2 element2 in collection2)
                {
                    if (Evaluate(element1, element2))
                    {
                        GenericPair<T1, T2> pair = new GenericPair<T1, T2>(element1, element2);
                        evaluatedPairCollection.Add(pair);
                    }
                }
            }

            return (evaluatedPairCollection);
        }
    }
}
