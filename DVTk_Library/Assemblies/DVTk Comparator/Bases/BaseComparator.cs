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

using Dvtk.Results;
using DvtkData.Dimse;

namespace Dvtk.Comparator
{
    /// <summary>
    /// Summary description for BaseComparator.
    /// </summary>
    public abstract class BaseComparator
    {
        /// <summary>
        /// Compare the two messages.
        /// </summary>
        /// <param name="tagValueFilterCollection">Tag Value Filter.</param>
        /// <param name="resultsReporter">Results reporter.</param>
        /// <param name="thatBaseComparator">Reference comparator.</param>
        /// <returns>bool - true = messages compared, false messages not compared</returns>
        public abstract bool Compare(TagValueCollection tagValueFilterCollection, ResultsReporter resultsReporter, BaseComparator thatBaseComparator);
    }
}
