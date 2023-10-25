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
using System.Xml;
using System.Xml.XPath;

namespace Dvtk.Definition_Query_Library
{
    /// <summary>
    /// The IXPathQueryInterface provides functions that allow an user to query
    /// definition files by using XPath Queries.
    /// The purpose of the interface is to provide users with a set of
    /// functions that allow advanced custom queries.
    /// </summary>
    interface IXPathQueryInterface
    {
        XPathNodeIterator Query(string Expression, string FileName);
    }
}
