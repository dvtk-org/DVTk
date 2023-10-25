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
using System.Collections.Specialized;

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Dicom.Messages
{
	/// <summary>
	/// Represents a Dicom A_ASSOCIATE_RQ.
	/// </summary>
	public class AssociateRq: DulMessage
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private AssociateRq(): base(new DvtkData.Dul.A_ASSOCIATE_RQ())
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor to encapsulate an existing DvtkData A_ASSOCIATE_RQ.
		/// </summary>
		/// <param name="dvtkDataAssociateRq">The encapsulated DvtkData A_ASSOCIATE_RQ</param>
		internal AssociateRq(DvtkData.Dul.A_ASSOCIATE_RQ dvtkDataAssociateRq): base(dvtkDataAssociateRq)
		{
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
                return (DvtkDataAssociateRq.ApplicationContext.Name);
            }
        }

        /// <summary>
        /// Gets the called AE title.
        /// </summary>
        public string CalledAETitle
        {
            get
            {
                return (DvtkDataAssociateRq.CalledAETitle);
            }
        }

        /// <summary>
        /// Gets the calling AE title.
        /// </summary>
        public string CallingAETitle
        {
            get
            {
                return (DvtkDataAssociateRq.CallingAETitle);
            }
        }

		/// <summary>
		/// Gets the encapsulated DvtkData A_ASSOCIATE_RQ.
		/// </summary>
		internal DvtkData.Dul.A_ASSOCIATE_RQ DvtkDataAssociateRq
		{
			get
			{
				return(DvtkDataDulMessage as DvtkData.Dul.A_ASSOCIATE_RQ);
			}
		}

		/// <summary>
		/// Gets the presentation contexts.
		/// </summary>
		public PresentationContextCollection PresentationContexts
		{
			get
			{
				return(new PresentationContextCollection(DvtkDataAssociateRq.PresentationContexts));
			}
		}

        /// <summary>
        /// Gets the Protocol Version.
        /// </summary>
        public System.UInt16 ProtocolVersion
        {
            get
            {
                return (DvtkDataAssociateRq.ProtocolVersion);
            }
        }

        /// <summary>
        /// Gets the User Information.
        /// </summary>
        public DvtkData.Dul.UserInformation UserInformation
        {
            get
            {
                return (DvtkDataAssociateRq.UserInformation);
            }
        }



		//
		// - Methods -
		//

		/// <summary>
		/// Creates presentation contexts to be used in an A-ASSOCIATE-AC that are based on the
		/// presentation contexts of this instance.
		/// </summary>
		/// <remarks>
		/// The following holds for the returned presentation contexts:
		/// - All requested presentation contexts will be accepted (have result field 0).
		/// - For each requested presentation context, the first proposed transfer syntax will be used.
		/// </remarks>
		/// <returns>The created presentation contexts.</returns>
		public PresentationContextCollection CreatePresentationContextsForAssociateAc()
		{
			PresentationContextCollection presentationContextsForAssociateAc = new PresentationContextCollection();

			PresentationContextCollection presentationContexts = PresentationContexts;

			foreach(PresentationContext presentationContextInAssociateRq in presentationContexts)
			{
				PresentationContext presentationContextForAssociateAc = new PresentationContext
					(presentationContextInAssociateRq.AbstractSyntax,
					 0,
					 presentationContextInAssociateRq.TransferSyntaxes[0]);

				presentationContextForAssociateAc.SetId(presentationContextInAssociateRq.ID);

				presentationContextsForAssociateAc.Add(presentationContextForAssociateAc);
			}

			return(presentationContextsForAssociateAc);
		}

		/// <summary>
		/// Creates presentation contexts to be used in an A-ASSOCIATE-AC that are based on the
		/// presentation contexts of this instance.
		/// </summary>
		/// <remarks>
		/// The following holds for the returned presentation contexts:
		/// - All requested presentation contexts with an abstract syntax contained in the supplied
		///   SOP classes will be accepted (have result field 0). The rest will be rejected
		///   (have result field 3).
		/// - For each accepted requested presentation context, the first proposed transfer syntax 
		///   will be used.
		/// </remarks>
		/// <param name="sopClasses">The SOP Classes to accept.</param>
		/// <returns>The created presentation contexts.</returns>
		public PresentationContextCollection CreatePresentationContextsForAssociateAc(SopClasses sopClasses)
		{
			PresentationContextCollection presentationContextsForAssociateAc = new PresentationContextCollection();

			PresentationContextCollection presentationContexts = PresentationContexts;

			foreach(PresentationContext presentationContextInAssociateRq in presentationContexts)
			{
				String abstractSyntaxInAssociateRq = presentationContextInAssociateRq.AbstractSyntax;

				PresentationContext presentationContextForAssociateAc = null;

				if (sopClasses.List.Contains(abstractSyntaxInAssociateRq))
				{
					presentationContextForAssociateAc = new PresentationContext
						(presentationContextInAssociateRq.AbstractSyntax,
						 0,
						 presentationContextInAssociateRq.TransferSyntaxes[0]);
				}
				else
				{
					presentationContextForAssociateAc = new PresentationContext
						(presentationContextInAssociateRq.AbstractSyntax,
						 3,
						 "");
				}

				presentationContextForAssociateAc.SetId(presentationContextInAssociateRq.ID);

				presentationContextsForAssociateAc.Add(presentationContextForAssociateAc);
			}

			return(presentationContextsForAssociateAc);
		}

		/// <summary>
		/// Creates presentation contexts to be used in an A-ASSOCIATE-AC that are based on the
		/// presentation contexts of this instance.
		/// </summary>
		/// <remarks>
		/// The following holds for the returned presentation contexts:<br></br>
		/// - All requested presentation contexts with an abstract syntax not contained in the supplied
		///   SOP classes will be rejected (have result field 3).<br></br>
		/// - For each other requested presentation contex that has an abstract syntax contained in
		///   the supplied SOP classes, do the following:<br></br>
		///   1)<br></br>
		///   Check if one or more of the requested transfer syntaxes is present in the first supplied
		///   TransferSyntaxes instance. If this is the case, use the requested transfer syntax that is
		///   requested before the other ones in the accepted presentation context counterpart (has
		///   result field 0).<br></br>
		///   2)<br></br>
		///   If no requested transfer syntaxes was present, try this with the second supplied
		///   TransferSyntaxes instance.<br></br>
		///   3) If no requested transfer syntaxes was present is in any supplied TransferSyntaxes
		///   instance, reject the presentation context with result 4.<br></br>
		///   
		///   Note that a difference exists between supplying one TransferSyntaxes instance with all
		///   transfer syntaxes to accept and supplying multiple TransferSyntaxes instances each containing
		///   only one transfer syntax. In the first case, the preference (order of proposed transfer
		///   syntaxes) of the SCU will be used, in the second case the preference of the caller of this
		///   method will be used.
		/// </remarks>
		/// <param name="sopClasses">The SOP Classes to accept.</param>
		/// <param name="transferSyntaxesList">The transfer syntaxes to accept.</param>
		/// <returns>The created presentation contexts.</returns>
		public PresentationContextCollection CreatePresentationContextsForAssociateAc(SopClasses sopClasses, params TransferSyntaxes[] transferSyntaxesList)
		{
			PresentationContextCollection presentationContextsForAssociateAc = new PresentationContextCollection();

			PresentationContextCollection presentationContexts = PresentationContexts;

			foreach(PresentationContext presentationContextInAssociateRq in presentationContexts)
			{
				String abstractSyntaxInAssociateRq = presentationContextInAssociateRq.AbstractSyntax;

				PresentationContext presentationContextForAssociateAc = null;

				if (sopClasses.List.Contains(abstractSyntaxInAssociateRq))
				{
					String transferSyntaxForAssociateAc = DetermineTransferSyntaxToAccept
						(presentationContextInAssociateRq.TransferSyntaxes, 
						transferSyntaxesList);
					
					if (transferSyntaxForAssociateAc.Length == 0)
					{
						presentationContextForAssociateAc = new PresentationContext
							(presentationContextInAssociateRq.AbstractSyntax,
							4,
							"");
					}
					else
					{
						presentationContextForAssociateAc = new PresentationContext
							(presentationContextInAssociateRq.AbstractSyntax,
							0,
							transferSyntaxForAssociateAc);
					}
				}
				else
				{
					presentationContextForAssociateAc = new PresentationContext
						(presentationContextInAssociateRq.AbstractSyntax,
						3,
						"");
				}

				presentationContextForAssociateAc.SetId(presentationContextInAssociateRq.ID);

				presentationContextsForAssociateAc.Add(presentationContextForAssociateAc);
			}

			return(presentationContextsForAssociateAc);
		}

		/// <summary>
		/// Creates presentation contexts to be used in an A-ASSOCIATE-AC that are based on the
		/// presentation contexts of this instance.
		/// </summary>
		/// <remarks>
		/// The following holds for the returned presentation contexts:<br></br>
		/// - For each requested presentation contex, do the following:<br></br>
		///   1)<br></br>
		///   Check if one or more of the requested transfer syntaxes is present in the first supplied
		///   TransferSyntaxes instance. If this is the case, use the requested transfer syntax that is
		///   requested before the other ones in the accepted presentation context counterpart (has
		///   result field 0).<br></br>
		///   2)<br></br>
		///   If no requested transfer syntaxes was present, try this with the second supplied
		///   TransferSyntaxes instance.<br></br>
		///   3) If no requested transfer syntaxes was present is in any supplied TransferSyntaxes
		///   instance, reject the presentation context with result 4.<br></br><br></br>
		///   
		///   Note that a difference exists between supplying one TransferSyntaxes instance with all
		///   transfer syntaxes to accept and supplying multiple TransferSyntaxes instances each containing
		///   only one transfer syntax. In the first case, the preference (order of proposed transfer
		///   syntaxes) of the SCU will be used, in the second case the preference of the caller of this
		///   method will be used.
		/// </remarks>
		/// <param name="transferSyntaxesList">The transfer syntaxes to accept.</param>
		/// <returns>The created presentation contexts.</returns>
		public PresentationContextCollection CreatePresentationContextsForAssociateAc(params TransferSyntaxes[] transferSyntaxesList)
		{
			PresentationContextCollection presentationContextsForAssociateAc = new PresentationContextCollection();

			PresentationContextCollection presentationContexts = PresentationContexts;

			foreach(PresentationContext presentationContextInAssociateRq in presentationContexts)
			{
				String abstractSyntaxInAssociateRq = presentationContextInAssociateRq.AbstractSyntax;

				PresentationContext presentationContextForAssociateAc = null;

				String transferSyntaxForAssociateAc = DetermineTransferSyntaxToAccept
					(presentationContextInAssociateRq.TransferSyntaxes, 
					transferSyntaxesList);
					
				if (transferSyntaxForAssociateAc.Length == 0)
				{
					presentationContextForAssociateAc = new PresentationContext
						(presentationContextInAssociateRq.AbstractSyntax,
						4,
						"");
				}
				else
				{
					presentationContextForAssociateAc = new PresentationContext
						(presentationContextInAssociateRq.AbstractSyntax,
						0,
						transferSyntaxForAssociateAc);
				}

				presentationContextForAssociateAc.SetId(presentationContextInAssociateRq.ID);

				presentationContextsForAssociateAc.Add(presentationContextForAssociateAc);
			}

			return(presentationContextsForAssociateAc);
		}

		/// <summary>
		/// Determines the transfer syntax to accept in an A-ASSOCIATE-AC presentation context,
		/// based on the proposed transfer syntaxes for one A-ASSOCIATE-RQ presentation context
		/// and the supplied transferSyntaxesList.
		/// </summary>
		/// <param name="transferSyntaxesFromAssociateRq">The proposed transfer syntaxes in an A-ASSOCIATE-AC presentation context.</param>
		/// <param name="transferSyntaxesList">The preference of accepting transfer syntaxes for the caller of this method.</param>
		/// <returns>
		/// The transfer syntax to accept in the associated A-ASSOCIATE-AC presentation context.<br></br><br></br>
		/// 
		/// If no transfer syntax can be accepted, "" is returned.
		/// </returns>
		private String DetermineTransferSyntaxToAccept(StringCollection transferSyntaxesFromAssociateRq, params TransferSyntaxes[] transferSyntaxesList)
		{
			String transferSyntaxForAssociateAc = "";

			foreach(TransferSyntaxes transferSyntaxes in transferSyntaxesList)
			{
				foreach(String transferSyntaxFromAssociateRq in transferSyntaxesFromAssociateRq)
				{
					if (transferSyntaxes.List.Contains(transferSyntaxFromAssociateRq))
					{
						transferSyntaxForAssociateAc = transferSyntaxFromAssociateRq;
						break;
					}
				}

				if (transferSyntaxForAssociateAc.Length > 0)
				{
					break;
				}
			}

			return(transferSyntaxForAssociateAc);
		}

		/// <summary>
		/// Returns a String that represents this instance.
		/// </summary>
		/// <returns>A String that represents this instance.</returns>
		public override string ToString()
		{
			return "A-ASSOCIATE-RQ";
		}

	}
}
