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



namespace DvtkHighLevelInterface.Dicom.Messages
{
	/// <summary>
	/// Represents a Dicom A_ASSOCIATE_RJ.
	/// </summary>
	public class AssociateRj: DulMessage
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal AssociateRj(): base(new DvtkData.Dul.A_ASSOCIATE_RJ())
		{
		}
		
		/// <summary>
		/// Constructor to encapsulate an existing DvtkData A_ASSOCIATE_RJ.
		/// </summary>
		/// <param name="dvtkDataAssociateRj">The encapsulated DvtkData A_ASSOCIATE_RJ</param>
		internal AssociateRj(DvtkData.Dul.A_ASSOCIATE_RJ dvtkDataAssociateRj): base(dvtkDataAssociateRj)
		{
		}



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the encapsulated DvtkData Associate Reject.
        /// </summary>
        internal DvtkData.Dul.A_ASSOCIATE_RJ DvtkDataAssociateRj
        {
            get
            {
                return (DvtkDataDulMessage as DvtkData.Dul.A_ASSOCIATE_RJ);
            }
        }

        /// <summary>
        /// Gets the DICOM reject reason containing an integer value encoded as an unsigned binary number.
        /// </summary>
        /// <remarks>
        /// If the Source field has the value (1) “DICOM UL service-user”, 
        /// it shall take one of the following:<br></br>
        /// 1 - no-reason-given<br></br>
        /// 2 - application-context-name-not-supported<br></br>
        /// 3 - calling-AE-title-not-recognized<br></br>
        /// 4-6 - reserved<br></br>
        /// 7 - called-AE-title-not-recognized<br></br>
        /// 8-10 - reserved<br></br>
        /// </remarks>
        /// <remarks>
        /// If the Source field has the value (2) “DICOM UL service provided (ACSE related function),” 
        /// it shall take one of the following:<br></br>
        /// 1 - no-reason-given<br></br>
        /// 2 - protocol-version-not-supported<br></br>
        /// </remarks>
        /// <remarks>
        /// If the Source field has the value (3) “DICOM UL service provided (Presentation related function),” 
        /// it shall take one of the following:<br></br>
        /// 0 - reserved<br></br>
        /// 1 - temporary-congestion<br></br>
        /// 2 - local-limit-exceeded<br></br>
        /// 3-7 - reserved
        /// </remarks>
        public System.Byte Reason
        {
            get
            {
                return (DvtkDataAssociateRj.Reason);
            }
        } 

        /// <summary>
        /// Gets the DICOM reject result containing an integer value encoded as an unsigned binary number.
        /// </summary>
        /// <remarks>
        /// One of the following values shall be used:<br></br>
        /// 1 - rejected-permanent<br></br>
        /// 2 - rejected-transient
        /// </remarks>
        public System.Byte Result
        {
            get
            {
                return (DvtkDataAssociateRj.Result);
            }
        }

        /// <summary>
        /// Gets the DICOM reject source containing an integer value encoded as an unsigned binary number.
        /// </summary>
        /// <remarks>
        /// One of the following values shall be used:<br></br>
        /// 1 - DICOM UL service-user<br></br>
        /// 2 - DICOM UL service-provider (ACSE related function)<br></br>
        /// 3 - DICOM UL service-provider (Presentation related function)
        /// </remarks>
        public System.Byte Source
        {
            get
            {
                return (DvtkDataAssociateRj.Source);
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
			return "A-ASSOCIATE-RJ";
		}
	}
}
