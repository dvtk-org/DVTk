// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright  2009 DVTk
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



namespace DvtkApplicationLayer.StoredFiles
{
    public class PixFileGroup: ApplicationConfigurableFileGroup
    {
        //
        // - Fields -
        //

        private ReceivedDicomMessagesFileGroup receivedDicomMessagesFileGroup = null;

        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private PixFileGroup(): base("Pix")
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="receivedDicomMessagesFileGroup">The associated instance from which the Directory setting is used.</param>
        public PixFileGroup(ReceivedDicomMessagesFileGroup receivedDicomMessagesFileGroup): base("Pix")
        {
            this.receivedDicomMessagesFileGroup = receivedDicomMessagesFileGroup;

            this.Wildcards.Add("*.pix");

            RemoveModeCurrentApplicationRun = RemoveMode.Remove;
            RemoveModePreviousApplicationRuns = RemoveMode.Remove;
            AskPermissionToRemoveCurrentApplicationRun = false;
            AskPermissionToRemovePreviousApplicationRuns = false;
        }

        /// <summary>
        /// Hide this constructor.
        /// </summary>
        /// <param name="name">The name.</param>
        private PixFileGroup(String name): base("Pix")
        {
            // Do nothing.
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Get will return the same directory as the Directory property from the associated
        /// ReceivedDicomMessagesFileGroup instance.
        /// <br></br><br></br>
        /// Set will not do anything.
        /// </summary>
        public override string Directory
        {
            get
            {
                return this.receivedDicomMessagesFileGroup.Directory;
            }
            set
            {
                // Do nothing.
            }
        }
    }
}
