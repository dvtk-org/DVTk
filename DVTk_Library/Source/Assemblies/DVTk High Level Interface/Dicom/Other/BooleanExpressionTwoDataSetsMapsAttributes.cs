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
using System.Collections.Generic;
using System.Text;

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Dicom.Other
{
    /// <summary>
    /// Class that is able to evaluate two datasets by comparing the values of two specific attributes of those datasets.
    /// </summary>
    class BooleanExpressionTwoDataSetsMapsAttributes : BooleanExpressionTwoDataSets
    {
        //
        // - Fields -
        //

        private String tagSequence1 = null;

        private String tagSequence2 = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Default constructor. 
        /// </summary>
        public BooleanExpressionTwoDataSetsMapsAttributes(String tagSequence1, String tagSequence2)
        {
            this.tagSequence1 = tagSequence1;
            this.tagSequence2 = tagSequence2;
        }

        /// <summary>
        /// Evaluates two datasets by comparing the values of two specific attributes of those datasets.
        /// </summary>
        /// <param name="dataSet1">Dataset 1.</param>
        /// <param name="dataSet2">Dataset 2.</param>
        /// <returns>Boolean indicating if the values of two specific attributes of the supplied datasets are the same.</returns>
        public override bool Evaluate(DataSet dataSet1, DataSet dataSet2)
        {
            Values values1 = dataSet1[this.tagSequence1].Values;
            Values values2 = dataSet2[this.tagSequence2].Values;

            return (values1.Equals(values2));
        }
    }
}
