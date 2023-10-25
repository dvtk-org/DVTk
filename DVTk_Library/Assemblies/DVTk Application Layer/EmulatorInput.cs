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

namespace DvtkApplicationLayer
{
    /// <summary>
    /// Summary description for EmulatorInput.
    /// </summary>
    public class EmulatorInput : BaseInput
    {
        # region Private Members

        private ArrayList emulatorFileNames = new ArrayList();
        private bool modeOfAssociation = false;
        private bool validateOnImport = false;
        private UInt16 nosOfRepetitons = 1;
        private Boolean dataUnderNewStudy = false;

        # endregion

        # region Properties
        /// <summary>
        /// Represents emulator files to be validated.
        /// </summary>
        public ArrayList FileNames
        {
            get
            {
                return emulatorFileNames;
            }
            set
            {
                emulatorFileNames = value;
            }
        }
        /// <summary>
        /// Determines the mode of association (i.e single or multiple)
        ///<returns>
        ///True means Multiple Association.
        ///False means Single Association.
        ///</returns>
        /// </summary>
        public bool ModeOfAssociation
        {
            get
            {
                return modeOfAssociation;
            }
            set
            {
                modeOfAssociation = value;
            }
        }
        /// <summary>
        /// Method that specifies whether to validate before import of an image. 
        /// </summary>
        public bool ValidateOnImport
        {
            get
            {
                return validateOnImport;
            }
            set
            {
                validateOnImport = value;
            }
        }
        /// <summary>
        /// Represents the number of times thigs are send.
        /// </summary>
        public UInt16 NosOfRepetitions
        {
            get
            {
                return nosOfRepetitons;
            }
            set
            {
                nosOfRepetitons = value;
            }
        }
        /// <summary>
        /// Boolean DataUnderStudy.
        /// </summary>
        public Boolean DataUnderNewStudy
        {
            get
            {
                return dataUnderNewStudy;
            }
            set
            {
                dataUnderNewStudy = value;
            }
        }


        #endregion

        # region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public EmulatorInput()
        {
        }

        # endregion
    }
}
