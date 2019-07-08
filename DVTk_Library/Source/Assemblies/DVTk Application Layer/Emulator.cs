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
	/// Summary description for Emulator.
	/// </summary>
	public class Emulator : PartOfSession
	{   
        /// <summary>
        /// Enum Describing various Types of Emulator.
        /// </summary>
        public enum EmulatorTypes {
            /// <summary>
            /// Emulator Type Storage SCP.
            /// </summary>
            STORAGE_SCP,
            /// <summary>
            /// Emulator Type Storage SCU.
            /// </summary>
            STORAGE_SCU,
            /// <summary>
            /// Emulator Type Print SCP.
            /// </summary>
            PRINT_SCP
        }

        /// <summary>
        /// Constructor For Emulator Class
        /// </summary>
        public Emulator() {

        }

        /// <summary>
        /// Constructor with parameters For Emulator Class
        /// </summary>
        /// <param name="session"></param>
        /// <param name="emulatorName"></param>
		public Emulator(Session session, String emulatorName): base(session)
		{
            this.emulatorName = emulatorName;
		}

		private string emulatorName; 
		private IList emulatorResultFiles = new ArrayList();
        private EmulatorTypes emulatorType ;
        /// <summary>
        /// Represents the name of the emulator.
        /// </summary>
		public string EmulatorName 
		{
			get 
			{
				return emulatorName ;}
			set 
			{
				emulatorName = value;}
		}
        /// <summary>
        /// Represents Collection of Results for an emulator.
        /// </summary>
		public IList Result
		{
			get 
			{ 
				return emulatorResultFiles ;}
			set 
			{
				emulatorResultFiles = value;
			}
		}
       /// <summary>
       /// Represent the type of emulator
       /// </summary>

        public EmulatorTypes EmulatorType{
            get { 
                return emulatorType ;
            }
            set {
                emulatorType = value;
            }
        }

	}
}

