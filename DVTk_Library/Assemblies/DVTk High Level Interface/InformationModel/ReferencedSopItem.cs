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



namespace DvtkHighLevelInterface.InformationModel
{
    /// <summary>
    /// Possible states of an instance.
    /// </summary>
	public enum InstanceStateEnum
	{
        /// <summary>
        /// State of this instance.
        /// </summary>
		InstanceCreated,
        /// <summary>
        /// State of this instance.
        /// </summary>
        InstanceStored,
        /// <summary>
        /// State of this instance.
        /// </summary>
        InstanceMppsCompleted,
        /// <summary>
        /// State of this instance.
        /// </summary>
        InstanceStorageCommitRequested,
        /// <summary>
        /// State of this instance.
        /// </summary>
		InstanceStorageCommitReportedSuccess,
        /// <summary>
        /// State of this instance.
        /// </summary>
        InstanceStorageCommitReportedFailure
	}

	/// <summary>
	/// Summary description for ReferencedSopItem.
	/// </summary>
	public class ReferencedSopItem
	{
		private InstanceStateEnum _instanceState;
		private System.String _sopClassUid = System.String.Empty;
		private System.String _sopInstanceUid = System.String.Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sopClassUid">SOP class UID.</param>
        /// <param name="sopInstanceUid">SOP instance UID.</param>
		public ReferencedSopItem(System.String sopClassUid, System.String sopInstanceUid)
		{
			_instanceState = InstanceStateEnum.InstanceCreated;
			_sopClassUid = sopClassUid;
			_sopInstanceUid = sopInstanceUid;
		}

        /// <summary>
        /// Gets the SOP class UID.
        /// </summary>
		public System.String SopClassUid
		{
			get 
			{
				return _sopClassUid;
			}
		}

        /// <summary>
        /// Gets the SOP instance UID.
        /// </summary>
        public System.String SopInstanceUid
		{
			get 
			{
				return _sopInstanceUid;
			}
		}

        /// <summary>
        /// Gets the instance state.
        /// </summary>
        public InstanceStateEnum InstanceState
		{
			get
			{
				return _instanceState;
			}
			set
			{
				_instanceState = value;
			}
		}
	}
}
