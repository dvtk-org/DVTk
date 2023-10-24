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

namespace DicomValidationToolKit.IodId
{
    using Dvtk.Dimse;
    using DicomValidationToolKit;
    using DicomValidationToolKit.Sessions;

    /// <summary>
    /// IOD identifier forms an DVT application specific identifier that specifies the
    /// Information Object Class Definition to use during the validation of Dicom message exchange.
    /// </summary>
    /// <remarks>
    /// Each Information Object Class definition consists of a description of its purpose and 
    /// the Attributes which define it.
    /// </remarks>
    public class IodId
    {
        string id = new Guid().ToString();
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            IodId iodId = (IodId)obj;
            // Use Equals to compare instance variables.
            return id.Equals(iodId.id);
        }
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
        public IodId(string id)
        {
            this.id = id;
        }
        public IodId(DimseCommand m_DimseCommand, System.String sopClassUID)
        {
            // TODO: determine IodId from definition component
        }
    }
}
