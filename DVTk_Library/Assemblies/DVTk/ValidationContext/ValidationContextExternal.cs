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

namespace DicomValidationToolKit
{
    using System.Collections;
    using System.Text;
    /// <summary>
    /// Summary description for DicomValidationContext.
    /// </summary>
    public class DicomValidationContext
    {
        private DicomValidationContext()
        {
        }
        static Hashtable m_attributeSetHashTable = new Hashtable();
        static Hashtable m_sessionHashTable = new Hashtable();
        static public Hashtable AttributeSets
        {
            get { return m_attributeSetHashTable; }
        }
        static public string AttributeSetsToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry myEntry in m_attributeSetHashTable)
            {
                sb.AppendFormat("key:\t{0}\n", myEntry.Key.ToString());
            }
            return sb.ToString();
        }
        static public Hashtable Sessions
        {
            get { return m_sessionHashTable; }
        }
        static public string SessionsToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry myEntry in m_sessionHashTable)
            {
                sb.AppendFormat("key:\t{0}\n", myEntry.Key.ToString());
            }
            return sb.ToString();
        }
    }
}
