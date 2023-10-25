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

using DvtkHighLevelInterface.Common.Other;



namespace DvtkHighLevelInterface.Dicom.Other
{
    /// <summary>
    /// The exception that is thrown when sending of a Dicom protocol message fails.
    /// </summary>
    public class DicomProtocolMessageSendException: System.Exception
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property SendReturnCode.
        /// </summary>
        private Dvtk.Sessions.SendReturnCode sendReturnCode = Dvtk.Sessions.SendReturnCode.Success;



        //
        // - Constructors -
        //

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="exceptionText">The exception text.</param>
        /// <param name="sendReturnCode">The send return code indicating the reason for the exception.</param>
        public DicomProtocolMessageSendException(String exceptionText, Dvtk.Sessions.SendReturnCode sendReturnCode)
            : base(exceptionText)
        {
            this.sendReturnCode = sendReturnCode;
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the send return code indicating the reason for the exception.
        /// </summary>
        public Dvtk.Sessions.SendReturnCode SendReturnCode
        {
            get
            {
                return (this.sendReturnCode);
            }
        }
    }
}
