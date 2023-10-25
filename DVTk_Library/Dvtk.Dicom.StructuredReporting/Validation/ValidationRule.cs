// Copyright © 2010 DVTk
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

namespace Dvtk.Dicom.StructuredReporting.Validation
{
    /// <summary>
    /// Contains the common functionality for all validation rules.
    /// </summary>
    public class ValidationRule
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Contains the first part of the message type. This is needed when creating a
        /// ValidationResult instance.
        /// </summary>
        private string messageTypePart1 = "";



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ValidationRule()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messageTypePart1">
        /// The first part of the message type that should always be present in the complete
        /// message type of a ValidationResult instance. This string should describe the context
        /// of the ValidationResult instance. An example of such a context description is
        /// "DICOM - Structured Reporting - Content Item - Concept Name".
        /// </param>
        public ValidationRule(string messageTypePart1)
        {
            this.messageTypePart1 = messageTypePart1;
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Create a ValidationResult instance.
        /// </summary>
        /// <remarks>
        /// Use this method when the second part of the message type should be the same as the
        /// message of the created ValidationResult instance.
        /// </remarks>
        /// <param name="message">The message of the created instance.</param>
        /// <returns></returns>
        protected ValidationResult CreateValidationResult(string message)
        {
            return (CreateValidationResult(message, message));
        }

        /// <summary>
        /// Create a ValidationResult instance.
        /// </summary>
        /// <remarks>
        /// Use this method when the second part of the message type should not be the same as the
        /// message of the created ValidationResult instance.
        /// </remarks>
        /// <param name="messageTypePart2">The second part of the message type of the created message.</param>
        /// <param name="message">The message of the created instance.</param>
        /// <returns></returns>
        protected ValidationResult CreateValidationResult(string messageTypePart2, string message)
        {
            return(new ValidationResult(this.messageTypePart1 + " - " + messageTypePart2, message));
        }
    }
}
