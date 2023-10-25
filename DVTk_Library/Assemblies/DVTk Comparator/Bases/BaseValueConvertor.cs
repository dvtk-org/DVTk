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
    /// Summary description for BaseValueConvertor.
    /// </summary>
    public abstract class BaseValueConvertor
    {
        public System.String FromHl7ToDicom(System.String hl7Value, int componentIndex)
        {
            return FromHl7ToDicom(GetSubString(hl7Value, componentIndex));
        }

        public abstract System.String FromHl7ToDicom(System.String hl7Value);

        public abstract System.String FromDicomToHl7(System.String dicomValue);

        protected System.String GetSubString(System.String hl7Value, int componentIndex)
        {
            System.String subHl7Value = hl7Value;
            if (componentIndex != -1)
            {
                System.String[] hl7ValueComponent = new System.String[componentIndex]; // Initial size
                hl7ValueComponent = hl7Value.Split('^');
                if (hl7ValueComponent.Length >= componentIndex)
                {
                    subHl7Value = hl7ValueComponent[componentIndex - 1];
                }
            }

            return subHl7Value;
        }
    }
}
