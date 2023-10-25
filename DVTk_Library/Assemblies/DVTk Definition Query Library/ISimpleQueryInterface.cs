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
using System.Collections.Specialized;

namespace Dvtk.Definition_Query_Library
{
    /// <summary>
    ///		The ISimpleQueryInterface provides functions that allow an user to
    ///		query definition files in a commando way. Each function covers a
    ///		specific query. The purpose of the interface is to provide users
    ///		with a set of simple functions that can easy be used to retrieve
    ///		specific information from Xml Definition files.
    /// </summary>
    interface ISimpleQueryInterface
    {
        #region Global Functions.
        string GetSOPClassUID();
        void SetSOPClassUID(string SOPClassUID);
        string GetSystemName();
        void SetSystemName(string SystemName);
        #endregion

        #region System/Application related function.
        string GetSystemVersion();
        string GetApplicationName();
        #endregion

        #region Dimse-Command related functions.
        StringCollection GetAllDimseCommands();
        #endregion

        #region Module related functions.
        StringCollection GetAllModules();
        StringCollection GetAllModules(string DimseCommand);
        string GetModuleUsage(string ModuleName);
        #endregion

        #region Attribute Related Functions.

        StringCollection GetAllAttributes(bool Subitems);
        StringCollection GetAllAttributesInDimseCommand(string DimseCommand, bool Subitems);
        StringCollection GetAllAttributesInModule(string ModuleName, bool Subitems);
        StringCollection GetAllAttributesInAttribute(string Attribute, bool Recursive, bool Subitems);
        StringCollection GetAttributeTree();
        string GetAttributeType(string Attribute, bool Recursive);
        string GetAttributeVM(string Attribute, bool Recursive);
        string GetAttributeVR(string Attribute, bool Recursive);
        string GetAttributeCondition(string Attribute, bool Recursive);
        string GetAttributeComment(string Attribute, bool Recursive);
        string GetAttributeName(string Attribute, bool Recursive);
        string GetAttributeSource(string Attribute, bool Recursive);
        string GetAttributePresenceOfValue(string Attribute, bool Recursive);
        string GetAttributeMatchingKey(string Attribute, bool Recursive);
        string GetAttributeReturnKey(string Attribute, bool Recursive);
        string GetAttributeQueryKey(string Attribute, bool Recursive);
        string GetAttributeDisplayedKey(string Attribute, bool Recursive);
        string GetAttributeIOD(string Attribute, bool Recursive);
        string GetAttributeTypeOfMatching(string Attribute, bool Recursive);
        StringCollection GetAttributeValues(string Attribute, bool Recursive);
        StringCollection GetAttributeDefinedValues(string Attribute, bool Recursive);
        StringCollection GetAttributeEnumeratedValues(string Attribute, bool Recursive);
        #endregion
    }
}
