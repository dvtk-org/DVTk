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

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using VR = DvtkData.Dimse.VR;

namespace Dvtk.DvtkDicomEmulators.Bases
{
	/// <summary>
	/// Summary description for HliScp.
	/// </summary>
	public class HliScp: MessageIterator
	{
		/// <summary>
		/// Transfer Syntax definitions
		/// </summary>
		public const System.String IMPLICIT_VR_LITTLE_ENDIAN = "1.2.840.10008.1.2";
		public const System.String EXPLICIT_VR_LITTLE_ENDIAN = "1.2.840.10008.1.2.1";
		public const System.String EXPLICIT_VR_BIG_ENDIAN = "1.2.840.10008.1.2.2";

		/// <summary>
		/// Verification SOP Class UID definition
		/// </summary>
		public const System.String VERIFICATION_SOP_CLASS_UID = "1.2.840.10008.1.1";


		public enum ScpRespondToAssociateRequestEnum
		{
			WithAssociateAccept,
			WithAssociateReject,
			WithAbort
		}

		public enum ScpRespondToReleaseRequestEnum
		{
			WithReleaseResponse,
			WithAbort
		}

		private TransferSyntaxes _transferSyntaxes = null;
        private SopClasses       _sopClasses = null;

		private ScpRespondToAssociateRequestEnum _scpRespondToAssociateRequest = ScpRespondToAssociateRequestEnum.WithAssociateAccept;

		private Byte _rejectResult = 0;
		private Byte _rejectSource = 0;
		private Byte _rejectReason = 0;

		private ScpRespondToReleaseRequestEnum _scpRespondToReleaseRequest = ScpRespondToReleaseRequestEnum.WithReleaseResponse;

		private Byte _abortSource = 0;
		private Byte _abortReason = 0;

		private bool _resultsFilePerAssociation = false;


		/// <summary>
		/// Class constructor.
		/// </summary>
		public HliScp()
		{
			// set up the default transfer syntax support
			_transferSyntaxes = new TransferSyntaxes(IMPLICIT_VR_LITTLE_ENDIAN, 
				EXPLICIT_VR_LITTLE_ENDIAN, 
				EXPLICIT_VR_BIG_ENDIAN);

            // set up the default sop class support
            _sopClasses = new SopClasses(VERIFICATION_SOP_CLASS_UID);
		}

		#region DULP request overrides
		/// <summary>
		/// Method to handle the workflow after receiving an Associate Request.
		/// </summary>
		/// <param name="associateRq">Associate Request message.</param>
		public override void AfterHandlingAssociateRequest(AssociateRq associateRq)
		{
			if (IsMessageHandled == false)
			{
				// determine which workflow to follow
				switch(_scpRespondToAssociateRequest)
				{
					case ScpRespondToAssociateRequestEnum.WithAssociateAccept:
						// send an associate accept with the supported transfer syntaxes.
                        SendAssociateAc(_sopClasses,_transferSyntaxes);
						break;
					case ScpRespondToAssociateRequestEnum.WithAssociateReject:
						// send an associate reject with the given parameters
						SendAssociateRj(_rejectResult, _rejectSource, _rejectReason);

						// handle results files
						if (_resultsFilePerAssociation == true)
						{
							StopResultsGathering();
							StartResultsGathering();
						}
						break;
					case ScpRespondToAssociateRequestEnum.WithAbort:
					default:
						// send an abort request with the given parameters
						SendAbort(_abortSource, _abortReason);

						// handle results files
						if (_resultsFilePerAssociation == true)
						{
							StopResultsGathering();
							StartResultsGathering();
						}
						break;
				}

				// message has now been handled
				IsMessageHandled = true;
			}
		}

		/// <summary>
		/// Method to handle the workflow after receiving a Release Request.
		/// </summary>
		/// <param name="releaseRq">Release Request message.</param>
		public override void AfterHandlingReleaseRequest(ReleaseRq releaseRq)
		{
			if (IsMessageHandled == false)
			{
				// determine which workflow to follow
				switch(_scpRespondToReleaseRequest)
				{
					case ScpRespondToReleaseRequestEnum.WithReleaseResponse:
						// send a release response
						SendReleaseRp();
						break;
					case ScpRespondToReleaseRequestEnum.WithAbort:
					default:
						// send an abort request with the given parameters
						SendAbort(_abortSource, _abortReason);
						break;
				}

				// handle results files
				if (_resultsFilePerAssociation == true)
				{
					StopResultsGathering();
					StartResultsGathering();
				}

				// message has now been handled
				IsMessageHandled = true;
			}			
		}

		/// <summary>
		/// Method to handle the workflow after receiving an Abort Request.
		/// </summary>
		/// <param name="abort">Abort Request message.</param>
		public override void AfterHandlingAbort(Abort abort)
		{
			// message has now been handled
			IsMessageHandled = true;
		}
		#endregion DULP request overrides

		#region DIMSE request overrides
		/// <summary>
		/// Method to handle the workflow after receiving a C-EHO-RQ.
		/// </summary>
		/// <param name="dicomMessage">C-ECHO-RQ message.</param>
		protected override void AfterHandlingCEchoRequest(DicomMessage dicomMessage)
		{
			if (IsMessageHandled == false)
			{
				DicomMessage dicomMessageToSend = new DicomMessage(DvtkData.Dimse.DimseCommand.CECHORSP);

				dicomMessageToSend.Set("0x00000002", VR.UI, "1.2.840.10008.1.1");
				dicomMessageToSend.Set("0x00000900", VR.US, 0);

				Send(dicomMessageToSend);
	
				// message has now been handled
				IsMessageHandled = true;
			}
		}
		#endregion DIMSE request overrides

		#region workflow settings
		/// <summary>
		/// Clear the current transfer syntax list - reset contents to empty.
		/// </summary>
		public void ClearTransferSyntaxes()
		{
			_transferSyntaxes = null;
		}

        /// <summary>
        /// Clear the current sop class list - reset contents to empty.
        /// </summary>
        public void ClearSopClasses()
        {
            _sopClasses = null;
        }

		/// <summary>
		/// Add a single transfer syntax to the list.
		/// </summary>
		/// <param name="transferSyntax">Transfer Syntax UID.</param>
		public void AddTransferSyntax(System.String transferSyntax)
		{
			// first check if the class has been instantiated
			if (_transferSyntaxes == null)
			{
				_transferSyntaxes = new TransferSyntaxes();
			}

			// Add the transfer syntax
			_transferSyntaxes.Add(transferSyntax);
		}

        /// <summary>
        /// Add a single sop class to the list.
        /// </summary>
        /// <param name="sopClass">Sop Class UID.</param>
        public void AddSopClass(System.String sopClass)
        {
            // first check if the class has been instantiated
            if (_sopClasses == null)
            {
                _sopClasses = new SopClasses();
            }

            // Add the transfer syntax
            _sopClasses.Add(sopClass);
        }

		/// <summary>
		/// Property - define how to RespondToAssociateRequest.
		/// </summary>
		public ScpRespondToAssociateRequestEnum RespondToAssociateRequest
		{
			get
			{
				return _scpRespondToAssociateRequest;
			}
			set
			{
				_scpRespondToAssociateRequest = value;
			}
		}

		/// <summary>
		/// Property - define how to RespondToReleaseRequest
		/// </summary>
		public ScpRespondToReleaseRequestEnum RespondToReleaseRequest
		{
			get
			{
				return _scpRespondToReleaseRequest;
			}
			set
			{
				_scpRespondToReleaseRequest = value;
			}
		}

		/// <summary>
		///  Set Associate Reject parameters.
		/// </summary>
		/// <param name="result">DULP reject result.</param>
		/// <param name="source">DULP reject source.</param>
		/// <param name="reason">DULP reject reason.</param>
		public void SetRejectParameters(Byte result, Byte source, Byte reason)
		{
			_rejectResult = result;
			_rejectSource = source;
			_rejectReason = reason;
		}

		/// <summary>
		/// Set the Abort Request parameters.
		/// </summary>
		/// <param name="source">DULP abort source.</param>
		/// <param name="reason">DULP abort reason.</param>
		public void SetAbortParameters(Byte source, Byte reason)
		{
			_abortSource = source;
			_abortReason = reason;
		}

		/// <summary>
		/// Property - ResultsFilePerAssociation
		/// </summary>
		public bool ResultsFilePerAssociation
		{
			get
			{
				return _resultsFilePerAssociation;
			}
			set
			{
				_resultsFilePerAssociation = value;
			}
		}

        /// <summary>
        /// Property - Supported Sop Classes
        /// </summary>
        public SopClasses SupportedSopClasses
        {
            get
            {
                return _sopClasses;
            }            
        }

        /// <summary>
        /// Property - Supported Transfer Syntaxes
        /// </summary>
        public TransferSyntaxes SupportedTransferSyntaxes
        {
            get
            {
                return _transferSyntaxes;
            }            
        }
		#endregion workflow settings
	}
}
