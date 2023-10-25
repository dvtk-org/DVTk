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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;



namespace DvtkHighLevelInterface.Common.Other
{
    /// <summary>
    /// Hidden class that stores all central used instances from the HLI assembly.
    /// </summary>
    public class HighLevelInterface
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property TempFileCollection.
        /// </summary>
        private static TempFileCollection tempFileCollection = new TempFileCollection();



        //
        // - Properties -
        //

        /// <summary>
        /// Contains the full file names of all temporary files (files that need to
        /// be deleted automatically when unloading the HLI assembly). Only use this
        /// collection when you know what you are doing!
        /// </summary>
        public static TempFileCollection TempFileCollection
        {
            get
            {
                return tempFileCollection;
            }
        }
    }
}
