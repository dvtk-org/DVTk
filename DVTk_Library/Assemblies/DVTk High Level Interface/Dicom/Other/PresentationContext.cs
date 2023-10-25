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
using System.Collections.Specialized;

using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
    /// Represents a Dicom presentation context.<br></br><br></br>
    /// 
    /// This class is used used both for an A-ASSOCIATE-RQ and an A-ASSOCIATE-AC.
	/// </summary>
	public class PresentationContext
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property DvtkDataAcceptedPresentationContext
		/// </summary>
		private	DvtkData.Dul.AcceptedPresentationContext dvtkDataAcceptedPresentationContext = null;

		/// <summary>
		/// See property DvtkDataRequestedPresentationContext
		/// </summary>
		private	DvtkData.Dul.RequestedPresentationContext dvtkDataRequestedPresentationContext = null;



		//
		// - Constructors -
		//

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private PresentationContext()
        {
            // Do nothing.
        }

		/// <summary>
		/// Constructor.
		/// 
		/// Use this constructor to encapsulate an existing DvtkData AcceptedPresentationContext instance.
		/// </summary>
		/// <param name="dvtkDataAcceptedPresentationContext">The DvtkData AcceptedPresentationContext instance to encapsulate.</param>
		internal PresentationContext(DvtkData.Dul.AcceptedPresentationContext dvtkDataAcceptedPresentationContext)
		{
			this.dvtkDataAcceptedPresentationContext = dvtkDataAcceptedPresentationContext;
		}

		/// <summary>
		/// Constructor.
		/// 
		/// Use this constructor to encapsulate an existing DvtkData RequestedPresentationContext instance.
		/// </summary>
		/// <param name="dvtkDataRequestedPresentationContext">The DvtkData RequestedPresentationContext instance to encapsulate.</param>
		internal PresentationContext(DvtkData.Dul.RequestedPresentationContext dvtkDataRequestedPresentationContext)
		{
			this.dvtkDataRequestedPresentationContext = dvtkDataRequestedPresentationContext;
		}

		/// <summary>
		/// Constructor.
		/// 
		/// Use this constructor to create a new presentation context instance that is to be used in an A-ASSOCIATE-RQ.
		/// </summary>
		/// <param name="abstractSyntax">The abstract syntax.</param>
		/// <param name="transferSyntaxes">The transfer syntaxes (must contain at least one transfer syntax).</param>
		/// <exception cref="System.ArgumentException">No transfer syntaxes have been supplied.</exception>
		public PresentationContext(String abstractSyntax, params String[] transferSyntaxes)
		{
			//
			// Check parameter(s).
			//

			if (transferSyntaxes.Length < 1)
			{
				throw new System.ArgumentException("At least of transfer syntax must be supplied in the PresentationContext constructor.");
			}


			//
			// Create the encapsulated DvtkData instance.
			//

			DvtkData.Dul.AbstractSyntax dvtkDataAbstractSyntax = new DvtkData.Dul.AbstractSyntax(abstractSyntax);

			DvtkData.Dul.TransferSyntax[] dvtkDataTransferSyntaxes = new DvtkData.Dul.TransferSyntax[transferSyntaxes.Length];
			for (int index = 0; index < transferSyntaxes.Length; index++)
			{
				dvtkDataTransferSyntaxes[index] = new DvtkData.Dul.TransferSyntax(transferSyntaxes[index]);
			}

			this.dvtkDataRequestedPresentationContext = new DvtkData.Dul.RequestedPresentationContext(dvtkDataAbstractSyntax, dvtkDataTransferSyntaxes);
		}

		/// <summary>
		/// Constructor.
		/// 
		/// Use this constructor to create a new presentation context instance that is to be used in an A-ASSOCIATE-AC.
		/// </summary>
		/// <param name="abstractSyntax">The abstract syntax.</param>
		/// <param name="result">The result.</param>
		/// <param name="transferSyntax">The transfer syntax. May be empty when result is unequal to 0.</param>
		public PresentationContext(String abstractSyntax, int result, String transferSyntax)
		{
			//
			// Create the encapsulated DvtkData instance.
			//

			this.dvtkDataAcceptedPresentationContext = new DvtkData.Dul.AcceptedPresentationContext();

			this.dvtkDataAcceptedPresentationContext.AbstractSyntax = new DvtkData.Dul.AbstractSyntax(abstractSyntax);
			this.dvtkDataAcceptedPresentationContext.Result = (Byte)result;
			this.dvtkDataAcceptedPresentationContext.TransferSyntax = new DvtkData.Dul.TransferSyntax(transferSyntax);
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets the abstract syntax.
		/// </summary>
		public String AbstractSyntax
		{
			get
			{
				String abstractSyntax = "";
		
				if (this.dvtkDataAcceptedPresentationContext != null)
				{
					abstractSyntax = this.dvtkDataAcceptedPresentationContext.AbstractSyntax.UID;
				}
				else
				{
					abstractSyntax = this.dvtkDataRequestedPresentationContext.AbstractSyntax.UID;
				}

				return(abstractSyntax);
			}
		}

		/// <summary>
		/// Gets the encapsulated DvtkData AcceptedPresentationContext instance.
		/// </summary>
		internal DvtkData.Dul.AcceptedPresentationContext DvtkDataAcceptedPresentationContext
		{
			get
			{
				return(this.dvtkDataAcceptedPresentationContext);
			}
		}
			
		/// <summary>
		/// Gets the encapsulated DvtkData RequestedPresentationContext instance.
		/// </summary>
		internal DvtkData.Dul.RequestedPresentationContext DvtkDataARequestedPresentationContext
		{
			get
			{
				return(this.dvtkDataRequestedPresentationContext);
			}
		}

        /// <summary>
        /// Gets the presentation context ID.
        /// </summary>
        public int ID
        {
            get
            {
                int id = 0;

                if (this.dvtkDataAcceptedPresentationContext != null)
                {
                    id = this.dvtkDataAcceptedPresentationContext.ID;
                }
                else
                {
                    id = this.dvtkDataRequestedPresentationContext.ID;
                }

                return (id);
            }
        }

		/// <summary>
        /// Indicates if this instance has been constructed for an A-ASSOCIATE-AC.
		/// </summary>
		internal bool IsForAssociateAccept
		{
			get
			{
				return(this.dvtkDataAcceptedPresentationContext != null);
			}
		}

		/// <summary>
        /// Indicates if this instance has been constructed for an A-ASSOCIATE-RQ.
		/// </summary>
		internal bool IsForAssociateRequest
		{
			get
			{
				return(this.dvtkDataRequestedPresentationContext != null);
			}
		}

		/// <summary>
		/// Gets the Result.
		/// </summary>
        /// <remarks>
        /// Only use this property for an A-ASSOCIATE-AC presentation context.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">This instance is not an A-ASSOCIATE-AC presentation context.</exception>
		public int Result
		{
			get
			{
				int result = 0;

				if (this.dvtkDataAcceptedPresentationContext != null)
				{
					result = this.dvtkDataAcceptedPresentationContext.Result;
				}
				else
				{
                    throw new System.InvalidOperationException("Only use the Result property for an A-ASSOCIATE-AC presentation context.");
				}

				return(result);
			}
		}

		/// <summary>
		/// Gets the transfer syntax.
		/// </summary>
        /// <remarks>
        /// Only use this property for an A-ASSOCIATE-AC presentation context.
        /// </remarks>
		/// <exception cref="System.InvalidOperationException">This instance is not an A-ASSOCIATE-AC presentation context.</exception>
		public String TransferSyntax
		{
			get
			{
				String transferSyntax = "";

				if (this.dvtkDataAcceptedPresentationContext != null)
				{
					transferSyntax = this.dvtkDataAcceptedPresentationContext.TransferSyntax.UID;
				}
				else
				{
                    throw new System.InvalidOperationException("Only use the TransferSyntax property for an A-ASSOCIATE-AC presentation context.");
				}

				return(transferSyntax);
			}
		}

		/// <summary>
		/// Gets the transfer syntaxes.
		/// </summary>
        /// <remarks>
        /// Only use this property for an A-ASSOCIATE-RQ presentation context.
        /// </remarks>
		/// <exception cref="System.InvalidOperationException">This instance is not an A-ASSOCIATE-RQ presentation context.</exception>
		public StringCollection TransferSyntaxes
		{
			get
			{
				StringCollection transferSyntaxes = new StringCollection();

				if (this.dvtkDataRequestedPresentationContext != null)
				{
					foreach(DvtkData.Dul.TransferSyntax dvtkDataTransferSyntax in this.dvtkDataRequestedPresentationContext.TransferSyntaxes)
					{
						transferSyntaxes.Add(dvtkDataTransferSyntax.UID);
					}
				}
				else
				{
                    throw new System.InvalidOperationException("Only use the TransferSyntaxes property for an A-ASSOCIATE-RQ presentation context.");
				}

				return(transferSyntaxes);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Sets the presentation context ID. 
		/// </summary>
		/// <param name="id">The presentation context ID.</param>
		internal void SetId(int id)
		{
			if (this.dvtkDataAcceptedPresentationContext != null)
			{
				this.dvtkDataAcceptedPresentationContext.ID = (byte)id;
			}
			else
			{
				this.dvtkDataRequestedPresentationContext.ID = (byte)id;
			}
		}
	}
}
