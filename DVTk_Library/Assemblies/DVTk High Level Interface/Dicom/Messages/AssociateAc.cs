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

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Dicom.Messages
{
	/// <summary>
	/// Represents a Dicom A_ASSOCIATE_AC.
	/// </summary>
	public class AssociateAc: DulMessage
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private AssociateAc(): base(new DvtkData.Dul.A_ASSOCIATE_AC())
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="callingAETitle">The calling AE title.</param>
		/// <param name="calledAETitle">The called AE title.</param>
		/// <param name="maximumPduSizeToReceive">Maximum pdu size to receive.</param>
		/// <param name="implementationClassUid">The implementation class UID.</param>
        /// <param name="ImplementationVersionName">The implementation version UID.</param>
        internal AssociateAc(String callingAETitle, String calledAETitle, uint maximumPduSizeToReceive, String implementationClassUid, String ImplementationVersionName)
            : base(new DvtkData.Dul.A_ASSOCIATE_AC())
		{
			DvtkDataAssociateAc.CallingAETitle = callingAETitle;
			DvtkDataAssociateAc.CalledAETitle = calledAETitle;
			DvtkDataAssociateAc.UserInformation.MaximumLength.MaximumLengthReceived = maximumPduSizeToReceive;
			DvtkData.Dul.ImplementationClassUid dvtkDataImplementationClassUid = new DvtkData.Dul.ImplementationClassUid();
			dvtkDataImplementationClassUid.UID = implementationClassUid;
			DvtkDataAssociateAc.UserInformation.ImplementationClassUid = dvtkDataImplementationClassUid;
            DvtkData.Dul.ImplementationVersionName dvtkDataImplementationVersionName = new DvtkData.Dul.ImplementationVersionName();
            dvtkDataImplementationVersionName.Name = ImplementationVersionName;
            DvtkDataAssociateAc.UserInformation.ImplementationVersionName = dvtkDataImplementationVersionName;
		}

		/// <summary>
		/// Constructor to encapsulate an existing DvtkData A_ASSOCIATE_AC.
		/// </summary>
		/// <param name="dvtkDataAssociateAc">The encapsulated DvtkData A_ASSOCIATE_AC</param>
		internal AssociateAc(DvtkData.Dul.A_ASSOCIATE_AC dvtkDataAssociateAc): base(dvtkDataAssociateAc)
		{
			// Do nothing.
		}



		//
		// - Properties -
		//

        /// <summary>
        /// Gets the Application Context.
        /// </summary>
        public string ApplicationContext
        {
            get
            {
                return (DvtkDataAssociateAc.ApplicationContext.Name);
            }
        }

		/// <summary>
		/// Gets the called AE title.
		/// </summary>
		public String CalledAETitle
		{
			get
			{
				return(DvtkDataAssociateAc.CalledAETitle);
			}
		}

		/// <summary>
		/// Gets the calling AE title.
		/// </summary>
		public String CallingAETitle
		{
			get
			{
				return(DvtkDataAssociateAc.CallingAETitle);
			}
		}

		/// <summary>
		/// Gets the encapsulated DvtkData A_ASSOCIATE_AC.
		/// </summary>
		public DvtkData.Dul.A_ASSOCIATE_AC DvtkDataAssociateAc
		{
			get
			{
				return(DvtkDataDulMessage as DvtkData.Dul.A_ASSOCIATE_AC);
			}
		}

		/// <summary>
		/// Gets the presentation contexts.
		/// </summary>
		public PresentationContextCollection PresentationContexts
		{
			get
			{
				return(new PresentationContextCollection(DvtkDataAssociateAc.PresentationContexts));
			}
		}

        /// <summary>
        /// Gets the Protocol Version.
        /// </summary>
        public System.UInt16 ProtocolVersion
        {
            get
            {
                return (DvtkDataAssociateAc.ProtocolVersion);
            }
        }

        /// <summary>
        /// Gets the User Information.
        /// </summary>
        public DvtkData.Dul.UserInformation UserInformation
        {
            get
            {
                return (DvtkDataAssociateAc.UserInformation);
            }
        }



		//
		// - Methods -
		//

		/// <summary>
		/// Returns a String that represents this instance.
		/// </summary>
		/// <returns>A String that represents this instance.</returns>
		public override string ToString()
		{
			return "A-ASSOCIATE-AC";
		}
	}
}
