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

namespace DvtkApplicationLayer
{
    /// <summary>
    /// Summary description for ScriptInputObj.
    /// </summary>
    public class ScriptInput : BaseInput
    {

        #region Private Members

        private string scriptFileName = "";
        private bool continueOnError = true;
        private object[] arguments;

        # endregion

        # region Properties
        /// <summary>
        /// Represents name of the script.
        /// </summary>
        public string FileName
        {
            get
            {
                return scriptFileName;
            }
            set
            {
                scriptFileName = value;
            }
        }
        /// <summary>
        /// Determines whether to continue on error.
        /// <return>
        /// return value = true, i.e continue validating even after encountering an error. 
        /// </return>
        /// </summary>
        public bool ContinueOnError
        {
            get
            {
                return continueOnError;
            }
            set
            {
                continueOnError = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object[] Arguments
        {
            get
            {
                return arguments;
            }
            set
            {
                arguments = value;
            }
        }

        # endregion

        # region Constructor 
        /// <summary>
        /// Constructor.
        /// </summary>
        public ScriptInput()
        {
        }

        #  endregion
    }
}
