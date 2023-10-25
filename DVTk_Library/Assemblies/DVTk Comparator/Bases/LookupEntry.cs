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

namespace Dvtk.Comparator.Bases
{
    /// <summary>
    /// Summary description for LookupEntry.
    /// </summary>
    public class LookupEntry
    {
        private System.String _value1 = System.String.Empty;
        private System.String _value2 = System.String.Empty;

        public LookupEntry(System.String value1, System.String value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public System.String Value1
        {
            get
            {
                return _value1;
            }
        }

        public System.String Value2
        {
            get
            {
                return _value2;
            }
        }
    }
}
