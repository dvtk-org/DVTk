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
using System.Diagnostics;

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Dicom.Threads
{
	/// <summary>
	/// A descendant of this class is used in combination with a MessageIterator
	/// to handle specific Messages that are received by the MessageIterator. An
	/// instance of this class will typically implement sending one or more messages
	/// as a reaction on specific received message. This class should normally not
	/// be concerned with control of flow e.g. like accepting/rejecting an association.
	/// </summary>
	abstract public class MessageHandler
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property DicomThread.
		/// </summary>
		private DicomThread dicomThread = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor.
		/// </summary>
		public MessageHandler()
		{
			// Do nothing.
		}



		//
		// - Properties -
		//

		/// <summary>
		/// The DicomThread this object operates on. It is the responsibility of the
		/// MessageIterator class to set this to the correct DicomThread before using
		/// this object.
		/// </summary>
		public DicomThread DicomThread
		{
			get
			{
				return(this.dicomThread);
			}
			set
			{
				this.dicomThread = value;
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Override this method to handle an A-ABORT.
		/// </summary>
		/// <param name="abort">The received A-ABORT.</param>
		/// <returns>Return true when this methods has handled the received A-ABORT, otherwise false.</returns>
		public virtual bool HandleAbort(Abort abort)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle an A-ASSOCIATE-AC.
		/// </summary>
		/// <param name="associateAc">The received A-ASSOCIATE-AC.</param>
		/// <returns>Return true when this methods has handled the received A-ASSOCIATE-AC, otherwise false.</returns>
		public virtual bool HandleAssociateAccept(AssociateAc associateAc)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle an A-ASSOCIATE-RJ.
		/// </summary>
		/// <param name="associateRj">The received A-ASSOCIATE-RJ.</param>
		/// <returns>Return true when this methods has handled the received A-ASSOCIATE-RJ, otherwise false.</returns>
		public virtual bool HandleAssociateReject(AssociateRj associateRj)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle an A-ASSOCIATE-RQ.
		/// </summary>
		/// <param name="associateRq">The received A-ASSOCIATE-RQ.</param>
		/// <returns>Return true when this methods has handled the received A-ASSOCIATE-RQ, otherwise false.</returns>
		public virtual bool HandleAssociateRequest(AssociateRq associateRq)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-CANCEL-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received C-CANCEL-RQ message.</param>
		/// <returns>Return true when this methods has handled the received C-CANCEL-RQ message , otherwise false.</returns>
		public virtual bool HandleCCancelRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-ECHO-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received C-ECHO-RQ message.</param>
		/// <returns>Return true when this methods has handled the received C-ECHO-RQ message , otherwise false.</returns>
		public virtual bool HandleCEchoRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-ECHO-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received C-ECHO-RSP message.</param>
		/// <returns>Return true when this methods has handled the received C-ECHO-RSP message , otherwise false.</returns>
		public virtual bool HandleCEchoResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-FIND-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received C-FIND-RQ message.</param>
		/// <returns>Return true when this methods has handled the received C-FIND-RQ message , otherwise false.</returns>
		public virtual bool HandleCFindRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-FIND-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received C-FIND-RSP message.</param>
		/// <returns>Return true when this methods has handled the received C-FIND-RSP message , otherwise false.</returns>
		public virtual bool HandleCFindResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-GET-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received C-GET-RQ message.</param>
		/// <returns>Return true when this methods has handled the received C-GET-RQ message , otherwise false.</returns>
		public virtual bool HandleCGetRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-GET-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received C-GET-RSP message.</param>
		/// <returns>Return true when this methods has handled the received C-GET-RSP message , otherwise false.</returns>
		public virtual bool HandleCGetResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-MOVE-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received C-MOVE-RQ message.</param>
		/// <returns>Return true when this methods has handled the received C-MOVE-RQ message , otherwise false.</returns>
		public virtual bool HandleCMoveRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-MOVE-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received C-MOVE-RSP message.</param>
		/// <returns>Return true when this methods has handled the received C-MOVE-RSP message , otherwise false.</returns>
		public virtual bool HandleCMoveResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-STORE-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received C-STORE-RQ message.</param>
		/// <returns>Return true when this methods has handled the received C-STORE-RQ message , otherwise false.</returns>
		public virtual bool HandleCStoreRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a C-STORE-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received C-STORE-RSP message.</param>
		/// <returns>Return true when this methods has handled the received C-STORE-RSP message , otherwise false.</returns>
		public virtual bool HandleCStoreResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// This method is indirectly called by a MessageIterator to let this object try
		/// to handle the received DicomMessage.
		/// </summary>
		/// <param name="dicomMessage">The received DicomMessage.</param>
		/// <returns>Returns true if this object has handled the DicomMessage, otherwise false.</returns>
		private bool HandleDicomMessage(DicomMessage dicomMessage)
		{
			bool handled = false;

			switch(dicomMessage.CommandSet.DimseCommand)
			{
				case DvtkData.Dimse.DimseCommand.CCANCELRQ:
					handled = HandleCCancelRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CECHORQ:
					handled = HandleCEchoRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CECHORSP:
					handled = HandleCEchoResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CFINDRQ:
                    string privateAtrrib = "";
                    foreach (DvtkData.Dimse.Attribute a in dicomMessage.DataSet.DvtkDataDataSet)
                    {
                        if (a.Tag.GroupNumber % 2 != 0)
                        {
                            privateAtrrib = privateAtrrib + " 0x" + a.Tag.GroupNumber.ToString("X4") + a.Tag.ElementNumber.ToString("X4") + ",";
                        }
                    }
                    if (privateAtrrib != "")
                        WriteWarning("Warning:The follwoing private attributes are present in the C-FIND-RQ" + privateAtrrib);
					handled = HandleCFindRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CFINDRSP:
					handled = HandleCFindResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CGETRQ:
					handled = HandleCGetRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CGETRSP:
					handled = HandleCGetResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CMOVERQ:
					handled = HandleCMoveRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CMOVERSP:
					handled = HandleCMoveResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CSTORERQ:
					handled = HandleCStoreRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CSTORERSP:
					handled = HandleCStoreResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NACTIONRQ:
					handled = HandleNActionRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NACTIONRSP:
					handled = HandleNActionResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NCREATERQ:
					handled = HandleNCreateRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NCREATERSP:
					handled = HandleNCreateResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NDELETERQ:
					handled = HandleNDeleteRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NDELETERSP:
					handled = HandleNDeleteResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NEVENTREPORTRQ:
					handled = HandleNEventReportRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NEVENTREPORTRSP:
					handled = HandleNEventReportResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NGETRQ:
					handled = HandleNGetRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NGETRSP:
					handled = HandleNGetResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NSETRQ:
					handled = HandleNSetRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NSETRSP:
					handled = HandleNSetResponse(dicomMessage);
					break;

				default:
					Debug.Assert(true, "Not yet implemented.");
					break;
			}

			return(handled);
		}

		/// <summary>
		/// This method is indirectly called by a MessageIterator to let this object try
		/// to handle the received DulMessage.
		/// </summary>
		/// <param name="dulMessage">The received DulMessage.</param>
		/// <returns>Returns true if this object has handled the DulMessage, otherwise false.</returns>
		private bool HandleDulMessage(DulMessage dulMessage)
		{
			bool handled = false;

			if (dulMessage is Abort)
			{
				handled = HandleAbort(dulMessage as Abort);
			}
			else if (dulMessage is AssociateAc)
			{
				handled = HandleAssociateAccept(dulMessage as AssociateAc);
			}
			else if (dulMessage is AssociateRj)
			{
				handled = HandleAssociateReject(dulMessage as AssociateRj);
			}
			else if (dulMessage is AssociateRq)
			{
				handled = HandleAssociateRequest(dulMessage as AssociateRq);
			}
			else if (dulMessage is ReleaseRp)
			{
				handled = HandleReleaseResponse(dulMessage as ReleaseRp);
			}
			else if (dulMessage is ReleaseRq)
			{
				handled = HandleReleaseRequest(dulMessage as ReleaseRq);
			}
			else
			{
				Debug.Assert(true, "Not implemented yet.");
			}

			return(handled);
		}

		/// <summary>
		/// This method is called by a MessageIterator to let this object try
		/// to handle the received message.
		/// </summary>
		/// <param name="message">The received message.</param>
		/// <returns>Returns true if this object has handled the message, otherwise false.</returns>
		internal bool HandleMessage(DicomProtocolMessage message)
		{
			bool handled = false;

			if (message is DulMessage)
			{
				handled = HandleDulMessage(message as DulMessage);
			}
			else if (message is DicomMessage)
			{
				handled = HandleDicomMessage(message as DicomMessage);
			}
			else
			{
				Debug.Assert(true, "Not supposed to get here.");
			}

			return(handled);
		}

		/// <summary>
		/// Override this method to handle a N-ACTION-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received N-ACTION-RQ message.</param>
		/// <returns>Return true when this methods has handled the received N-ACTION-RQ message , otherwise false.</returns>
		public virtual bool HandleNActionRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-ACTION-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received N-ACTION-RSP message.</param>
		/// <returns>Return true when this methods has handled the received N-ACTION-RSP message , otherwise false.</returns>
		public virtual bool HandleNActionResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-CREATE-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received N-CREATE-RQ message.</param>
		/// <returns>Return true when this methods has handled the received N-CREATE-RQ message , otherwise false.</returns>
		public virtual bool HandleNCreateRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-CREATE-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received N-CREATE-RSP message.</param>
		/// <returns>Return true when this methods has handled the received N-CREATE-RSP message , otherwise false.</returns>
		public virtual bool HandleNCreateResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-DELETE-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received N-DELETE-RQ message.</param>
		/// <returns>Return true when this methods has handled the received N-DELETE-RQ message , otherwise false.</returns>
		public virtual bool HandleNDeleteRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-DELETE-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received N-DELETE-RSP message.</param>
		/// <returns>Return true when this methods has handled the received N-DELETE-RSP message , otherwise false.</returns>
		public virtual bool HandleNDeleteResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-EVENT-REPORT-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received N-EVENT-REPORT-RQ message.</param>
		/// <returns>Return true when this methods has handled the received N-EVENT-REPORT-RQ message , otherwise false.</returns>
		public virtual bool HandleNEventReportRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-EVENT-REPORT-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received N-EVENT-REPORT-RSP message.</param>
		/// <returns>Return true when this methods has handled the received N-EVENT-REPORT-RSP message , otherwise false.</returns>
		public virtual bool HandleNEventReportResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-GET-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received N-GET-RQ message.</param>
		/// <returns>Return true when this methods has handled the received N-GET-RQ message , otherwise false.</returns>
		public virtual bool HandleNGetRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-GET-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received N-GET-RSP message.</param>
		/// <returns>Return true when this methods has handled the received N-GET-RSP message , otherwise false.</returns>
		public virtual bool HandleNGetResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-SET-RQ message.
		/// </summary>
		/// <param name="dicomMessage">The received N-SET-RQ message.</param>
		/// <returns>Return true when this methods has handled the received N-SET-RQ message , otherwise false.</returns>
		public virtual bool HandleNSetRequest(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle a N-SET-RSP message.
		/// </summary>
		/// <param name="dicomMessage">The received N-SET-RSP message.</param>
		/// <returns>Return true when this methods has handled the received N-SET-RSP message , otherwise false.</returns>
		public virtual bool HandleNSetResponse(DicomMessage dicomMessage)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle an A-RELEASE-RQ.
		/// </summary>
		/// <param name="releaseRq">The received A-RELEASE-RQ.</param>
		/// <returns>Return true when this methods has handled the received A-RELEASE-RQ, otherwise false.</returns>
		public virtual bool HandleReleaseRequest(ReleaseRq releaseRq)
		{
			return false;
		}

		/// <summary>
		/// Override this method to handle an A-RELEASE-RP.
		/// </summary>
		/// <param name="releaseRp">The received A-RELEASE-RP.</param>
		/// <returns>Return true when this methods has handled the received A-RELEASE-RP, otherwise false.</returns>
		public virtual bool HandleReleaseResponse(ReleaseRp releaseRp)
		{
			return false;
		}

















		/// <summary>
		/// Receives an A-ASSOCIATE-RJ
		/// </summary>
		/// <returns>The received A-ASSOCIATE-RJ</returns>
		/// <exception cref="System.Exception">
		///	Receiving of a message fails or the received message is not an A-ASSOCIATE-RJ.
		/// </exception> 
		/// <example>
		///		<b>VB.NET</b>
		///		<code>
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>
		protected AssociateRj ReceiveAssociateRj()
		{
			return(this.dicomThread.ReceiveAssociateRj());
		}

		/// <summary>
		/// Receives an A-ASSOCIATE-AC or A-ASSOCIATE-RJ.
		/// </summary>
		/// <returns>The received A-ASSOCIATE-AC or A-ASSOCIATE-RJ.</returns>
		/// <exception cref="System.Exception">
		///	Receiving of a message fails or the received message is not an A-ASSOCIATE-AC or A-ASSOCIATE-RJ.
		/// </exception> 
		protected DulMessage ReceiveAssociateRp()
		{
			return(this.dicomThread.ReceiveAssociateRp());
		}

		/// <summary>
		/// Receives a A-ASSOCIATE-RQ.
		/// </summary>
		/// <returns>The received A-ASSOCIATE-RQ.</returns>
		/// <exception cref="System.Exception">
		///	Receiving of a message fails or the received message is not an A-ASSOCIATE-RQ.
		/// </exception> 
		protected AssociateRq ReceiveAssociateRq()
		{
			return(this.dicomThread.ReceiveAssociateRq());
		}

		/// <summary>
		/// Receives a Dicom Message.
		/// </summary>
		/// <returns>The received Dicom Message.</returns>
		/// <exception cref="System.Exception">
		///	Receiving of a message fails or the received message is not a Dicom message.
		/// </exception> 
		/// <example>
		///		<b>VB.NET</b>
		///		<code>
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>
		protected DicomMessage ReceiveDicomMessage()
		{
			return(this.dicomThread.ReceiveDicomMessage());
		}

		/// <summary>
		/// Receives a messages (can be a Dicom or Dul message).
		/// </summary>
		/// <returns>The received message.</returns>
		/// <exception cref="System.Exception">
		///	Receiving of a message fails.
		/// </exception> 
		/// <example>
		///		<b>VB.NET</b>
		///		<code>
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>
		protected DicomProtocolMessage ReceiveMessage()
		{
			return(this.dicomThread.ReceiveMessage());
		}

		/// <summary>
		/// Receives a A-RELEASE-RP.
		/// </summary>
		/// <returns>The received A-RELEASE-RP.</returns>
		/// <exception cref="System.Exception">
		///	Receiving of a message fails or the received message is not an A-RELEASE-RP.
		/// </exception> 
		/// <example>
		///		<b>VB.NET</b>
		///		<code>
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>
		protected ReleaseRp ReceiveReleaseRp()
		{
			return(this.dicomThread.ReceiveReleaseRp());
		}

		/// <summary>
		/// Receives a A-RELEASE-RQ.
		/// </summary>
		/// <returns>The received A-RELEASE-RQ.</returns>
		/// <exception cref="System.Exception">
		///	Receiving of a message fails or the received message is not an A-RELEASE-RQQ.
		/// </exception> 
		protected ReleaseRq ReceiveReleaseRq()
		{
			return(this.dicomThread.ReceiveReleaseRq());
		}

		/// <summary>
		/// Sends a Dicom Message.
		/// </summary>
		/// <param name="dicomMessage">The DicomMessage.</param>
		/// <exception cref="System.Exception">
		///	Sending of the DicomMessage fails.
		/// </exception> 
		/// <example>
		///		<b>VB.NET</b>
		///		<code>
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>		
		protected void Send(DicomMessage dicomMessage)
		{
			this.dicomThread.Send(dicomMessage);
		}

        /// <summary>
        /// Sends a Dicom Message.
        /// </summary>
        /// <param name="dicomMessage">The DicomMessage.</param>
        /// <param name="presentationContextId">The ID of the presentation context to use.</param>
        protected internal void Send(DicomMessage dicomMessage, int presentationContextId)
        {
            this.dicomThread.Send(dicomMessage, presentationContextId);
        }


		/// <summary>
		/// Sends a Dicom A_ABORT.
		/// </summary>
		/// <param name="source">The Abort source.</param>
		/// <param name="reason">The Abort reason.</param>
		/// <returns>The sent A_ABORT.</returns>
		/// <exception cref="System.Exception">
		///	Sending of the A_ABORT fails.
		/// </exception> 
		protected Abort SendAbort(Byte source, Byte reason)
		{
			return(this.dicomThread.SendAbort(source, reason));
		}

		/// <overloads>
		/// Sends an A-ASSOCIATE-AC.
		/// </overloads>
		/// <summary>
		///	Sends an A-ASSOCIATE-AC based on the previously received A-ASSOCIATE-RQ.
		/// </summary>
		/// <remarks>
		///	The following holds for the presentation contexts in the sent A-ASSOCIATE-AC:<br></br>
		///	- All requested presentation contexts will be accepted (have result field 0).<br></br>
		///	- For each requested presentation context, the first proposed transfer syntax will be used.<br></br><br></br>
		///	
		/// If an A-ASSOCIATE-RJ should be sent when none of the requested presentation contexts is accepted,
		/// use the SendAssociateRp method instead.	
		/// </remarks>
		/// <returns>The sent AssociateAc.</returns>
		/// <exception cref="System.Exception">
		///	Last received message is not an A-ASSOCIATE-RQ or no message has been received at all.
		/// </exception> 
		protected AssociateAc SendAssociateAc()
		{
			return(this.dicomThread.SendAssociateAc());
		}

		/// <summary>
		/// Sends an A-ASSOCIATE-AC based on the supplied presentation contexts.
		/// </summary>
		/// <remarks>
		/// Any requested presentation context that has no counterpart in the supplied presentation
		/// contexts will automaticaly be added to the A-ASSOCIATE-AC with the presentation context rejected.<br></br><br></br>
		/// 
		/// If an A-ASSOCIATE-RJ should be sent when none of the requested presentation contexts is accepted,
		/// use the <see cref="SendAssociateRp(PresentationContextCollection)"/> method instead.	
		/// </remarks>
		/// <param name="presentationContextCollection">The presentation contexts.</param>
		/// <returns>The sent AssociateAc.</returns>
		protected AssociateAc SendAssociateAc(PresentationContextCollection presentationContextCollection)
		{
			return(this.dicomThread.SendAssociateAc(presentationContextCollection));
		}

		/// <summary>
		/// Sends an A-ASSOCIATE-AC based on the supplied presentation contexts.
		/// </summary>
		/// <remarks>
		/// Any requested presentation context that has no counterpart in the supplied presentation
		/// contexts will automaticaly be added to the A-ASSOCIATE-AC with the presentation context rejected.<br></br><br></br>
		/// 
		/// If an A-ASSOCIATE-RJ should be sent when none of the requested presentation contexts is accepted,
		/// use the <see cref="SendAssociateRp(PresentationContext[])"/> method instead.	
		/// </remarks>
		/// <param name="presentationContexts">The presentation contexts.</param>
		/// <returns>The sent AssociateAc.</returns>
		protected AssociateAc SendAssociateAc(params PresentationContext[] presentationContexts)
		{
			return(this.dicomThread.SendAssociateAc(presentationContexts));
		}

		/// <summary>
		///	Sends an A-ASSOCIATE-AC based on the previously received A-ASSOCIATE-RQ.
		/// </summary>
		/// <remarks>
		///	The following holds for the presentation contexts in the sent A-ASSOCIATE-AC:<br></br>
		/// - All requested presentation contexts with an abstract syntax contained in the supplied
		///   SOP classes will be accepted (have result field 0). The rest will be rejected
		///   (have result field 3).<br></br>
		/// - For each accepted requested presentation context, the first proposed transfer syntax 
		///   will be used.<br></br><br></br>
		///	
		/// If an A-ASSOCIATE-RJ should be sent when none of the requested presentation contexts is accepted,
		/// use the <see cref="SendAssociateRp(SopClasses)"/> method instead.	
		/// </remarks>
		/// <param name="sopClasses">The SOP Classes to accept.</param>
		/// <returns>The sent AssociateAc.</returns>
		/// <exception cref="System.Exception">
		///	Last received message is not an A-ASSOCIATE-RQ or no message has been received at all.
		/// </exception> 
		protected AssociateAc SendAssociateAc(SopClasses sopClasses)
		{
			return(this.dicomThread.SendAssociateAc(sopClasses));
		}

		/// <summary>
		///	Sends an A-ASSOCIATE-AC based on the previously received A-ASSOCIATE-RQ.
		/// </summary>
		/// <remarks>
		///	The following holds for the presentation contexts in the sent A-ASSOCIATE-AC:<br></br>
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
		///   instance, reject the presentation context with result 4.<br></br><br></br>
		///	
		///   Note that a difference exists between supplying one TransferSyntaxes instance with all
		///   transfer syntaxes to accept and supplying multiple TransferSyntaxes instances each containing
		///   only one transfer syntax. In the first case, the preference (order of proposed transfer
		///   syntaxes) of the SCU will be used, in the second case the preference of the caller of this
		///   method will be used.<br></br><br></br>
		///	
		/// If an A-ASSOCIATE-RJ should be sent when none of the requested presentation contexts is accepted,
		/// use the <see cref="SendAssociateRp(SopClasses, TransferSyntaxes[])"/> method instead.	
		/// </remarks>
		/// <param name="sopClasses">The SOP Classes to accept.</param>
		/// <param name="transferSyntaxesList">The transfer syntaxes to accept.</param>
		/// <returns>The sent AssociateAc.</returns>
		/// <exception cref="System.Exception">
		///	Last received message is not an A-ASSOCIATE-RQ or no message has been received at all.
		/// </exception> 
		protected AssociateAc SendAssociateAc(SopClasses sopClasses, params TransferSyntaxes[] transferSyntaxesList)
		{
			return(this.dicomThread.SendAssociateAc(sopClasses, transferSyntaxesList));
		}

		/// <summary>
		///	Sends an A-ASSOCIATE-AC based on the previously received A-ASSOCIATE-RQ.
		/// </summary>
		/// <remarks>
		///	The following holds for the presentation contexts in the sent A-ASSOCIATE-AC:<br></br>
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
		///   method will be used.<br></br><br></br>
		///	
		/// If an A-ASSOCIATE-RJ should be sent when none of the requested presentation contexts is accepted,
		/// use the <see cref="SendAssociateRp(TransferSyntaxes[])"/> method instead.	
		/// </remarks>
		/// <param name="transferSyntaxesList">The transfer syntaxes to accept.</param>
		/// <returns>The sent AssociateAc.</returns>
		/// <exception cref="System.Exception">
		///	Last received message is not an A-ASSOCIATE-RQ or no message has been received at all.
		/// </exception> 
		protected AssociateAc SendAssociateAc(params TransferSyntaxes[] transferSyntaxesList)
		{
			return(this.dicomThread.SendAssociateAc(transferSyntaxesList));
		}

		/// <summary>
		/// Sends a Dicom A_ASSOCIATE_RJ.
		/// </summary>
		/// <returns>The sent A_ASSOCIATE_RJ.</returns>
		/// <exception cref="System.Exception">
		///	Sending of the A_ASSOCIATE_RJ fails.
		/// </exception> 
		protected AssociateRj SendAssociateRj()
		{
			return(this.dicomThread.SendAssociateRj());
		}

		/// <summary>
		/// Sends a Dicom A_ASSOCIATE_RJ.
		/// </summary>
		/// <param name="result">The result as defined in the Dicom standard.</param>
		/// <param name="source">The source as defined in the Dicom standard.</param>
		/// <param name="reason">The reason as defined in the Dicom standard.</param>
		/// <returns>The sent A_ASSOCIATE_RJ.</returns>
		/// <exception cref="System.Exception">
		///	Sending of the A_ASSOCIATE_RJ fails.
		/// </exception> 
		protected AssociateRj SendAssociateRj(Byte result, Byte source, Byte reason)
		{
			return(this.dicomThread.SendAssociateRj(result, source, reason));
		}

		/// <overloads>
		/// Sends an A-ASSOCIATE-AC or A-ASSOCIATE-RJ.
		/// </overloads>
		/// <summary>
		/// Sends an A-ASSOCIATE-AC or A-ASSOCIATE-RJ based on the previously received A-ASSOCIATE-RQ.
		/// </summary>
		/// <remarks>
		/// This method will send an A-ASSOCIATE-AC if at least one presentation context will be accepted
		/// according to the rules specified below. Otherwise an A-ASSOCIATE-RJ will be sent. <br></br><br></br>
		/// 
		///	The following holds for the presentation contexts in the A-ASSOCIATE-AC:<br></br>
		///	- All requested presentation contexts will be accepted (have result field 0).<br></br>
		///	- For each requested presentation context, the first proposed transfer syntax will be used.
		/// </remarks>
		/// <returns>The sent AssociateAc or AssociateRj.</returns>
		/// <exception cref="System.Exception">
		///	Last received message is not an A-ASSOCIATE-RQ or no message has been received at all.
		/// </exception> 
		protected DulMessage SendAssociateRp()
		{
			return(this.dicomThread.SendAssociateRp());
		}

		/// <summary>
		/// Sends an A-ASSOCIATE-AC or A-ASSOCIATE-RJ based on the supplied presentation contexts.
		/// </summary>
		/// <remarks>
		/// This method will send an A-ASSOCIATE-AC if at least one presentation context will be accepted
		/// in the supplied presentation contexts. Otherwise an A-ASSOCIATE-RJ will be sent. <br></br><br></br>
		/// 
		/// Any requested presentation context that has no counterpart in the supplied presentation
		/// contexts will automaticaly be added to the A-ASSOCIATE-AC with the presentation context rejected.<br></br><br></br>
		/// </remarks>
		/// <param name="presentationContextCollection">The presentation contexts.</param>
		/// <returns>The sent AssociateAc or AssociateRj.</returns>
		protected DulMessage SendAssociateRp(PresentationContextCollection presentationContextCollection)
		{
			return(this.dicomThread.SendAssociateRp(presentationContextCollection));
		}

		/// <summary>
		/// Sends an A-ASSOCIATE-AC or A-ASSOCIATE-RJ based on the supplied presentation contexts.
		/// </summary>
		/// <remarks>
		/// This method will send an A-ASSOCIATE-AC if at least one presentation context will be accepted
		/// in the supplied presentation contexts. Otherwise an A-ASSOCIATE-RJ will be sent. <br></br><br></br>
		/// 
		/// Any requested presentation context that has no counterpart in the supplied presentation
		/// contexts will automaticaly be added to the A-ASSOCIATE-AC with the presentation context rejected.<br></br><br></br>
		/// </remarks>
		/// <param name="presentationContexts">The presentation contexts.</param>
		/// <returns>The sent AssociateAc or AssociateRj.</returns>
		protected DulMessage SendAssociateRp(params PresentationContext[] presentationContexts)
		{
			return(this.dicomThread.SendAssociateRp(presentationContexts));
		}

		/// <overloads>
		/// Sends an A-ASSOCIATE-AC or A-ASSOCIATE-RJ.
		/// </overloads>
		/// <summary>
		/// Sends an A-ASSOCIATE-AC or A-ASSOCIATE-RJ based on the previously received A-ASSOCIATE-RQ.
		/// </summary>
		/// <remarks>
		/// This method will send an A-ASSOCIATE-AC if at least one presentation context will be accepted
		/// according to the rules specified below. Otherwise an A-ASSOCIATE-RJ will be sent. <br></br><br></br>
		/// 
		///	The following holds for the presentation contexts in the A-ASSOCIATE-AC:<br></br>
		/// - All requested presentation contexts with an abstract syntax contained in the supplied
		///   SOP classes will be accepted (have result field 0). The rest will be rejected
		///   (have result field 3).<br></br>
		/// - For each accepted requested presentation context, the first proposed transfer syntax 
		///   will be used.<br></br><br></br>
		/// </remarks>
		/// <param name="sopClasses">The SOP Classes to accept.</param>
		/// <returns>The sent AssociateAc or AssociateRj.</returns>
		/// <exception cref="System.Exception">
		///	Last received message is not an A-ASSOCIATE-RQ or no message has been received at all.
		/// </exception> 
		protected DulMessage SendAssociateRp(SopClasses sopClasses)
		{
			return(this.dicomThread.SendAssociateRp(sopClasses));
		}

		/// <overloads>
		/// Sends an A-ASSOCIATE-AC or A-ASSOCIATE-RJ.
		/// </overloads>
		/// <summary>
		/// Sends an A-ASSOCIATE-AC or A-ASSOCIATE-RJ based on the previously received A-ASSOCIATE-RQ.
		/// </summary>
		/// <remarks>
		/// This method will send an A-ASSOCIATE-AC if at least one presentation context will be accepted
		/// according to the rules specified below. Otherwise an A-ASSOCIATE-RJ will be sent. <br></br><br></br>
		/// 
		///	The following holds for the presentation contexts in the A-ASSOCIATE-AC:<br></br>
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
		///   instance, reject the presentation context with result 4.<br></br><br></br>
		///   
		///   Note that a difference exists between supplying one TransferSyntaxes instance with all
		///   transfer syntaxes to accept and supplying multiple TransferSyntaxes instances each containing
		///   only one transfer syntax. In the first case, the preference (order of proposed transfer
		///   syntaxes) of the SCU will be used, in the second case the preference of the caller of this
		///   method will be used.
		/// </remarks>
		/// <param name="sopClasses">The SOP Classes to accept.</param>
		/// <param name="transferSyntaxesList">The transfer syntaxes to accept.</param>
		/// <returns>The sent AssociateAc or AssociateRj.</returns>
		/// <exception cref="System.Exception">
		///	Last received message is not an A-ASSOCIATE-RQ or no message has been received at all.
		/// </exception> 
		protected DulMessage SendAssociateRp(SopClasses sopClasses, params TransferSyntaxes[] transferSyntaxesList)
		{
			return(this.dicomThread.SendAssociateRp(sopClasses, transferSyntaxesList));
		}

		/// <summary>
		/// Sends an A-ASSOCIATE-AC or A-ASSOCIATE-RJ based on the previously received A-ASSOCIATE-RQ.
		/// </summary>
		/// <remarks>
		/// This method will send an A-ASSOCIATE-AC if at least one presentation context will be accepted
		/// according to the rules specified below. Otherwise an A-ASSOCIATE-RJ will be sent. <br></br><br></br>
		/// 
		///	The following holds for the presentation contexts in the A-ASSOCIATE-AC:<br></br>
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
		/// <returns>The sent AssociateAc or AssociateRj.</returns>
		/// <exception cref="System.Exception">
		///	Last received message is not an A-ASSOCIATE-RQ or no message has been received at all.
		/// </exception> 
		protected DulMessage SendAssociateRp(params TransferSyntaxes[] transferSyntaxesList)
		{
			return(this.dicomThread.SendAssociateRp(transferSyntaxesList));
		}

		/// <summary>
		/// Sends an A-ASSOCIATE-RQ.
		/// </summary>
		/// <param name="presentationContexts">One or more PresentationContext objects.</param>
		/// <returns>The sent A-ASSOCIATE-RQ</returns>
		/// <exception cref="System.Exception">
		///	One or more of the supplied presentation contexts is an A_ASSOCIATE_AC presentation 
		/// context or sending of the A-ASSOCIATE-RQ fails.
		/// </exception> 
		/// <example>
		///		<b>VB.NET</b>
		///		<code>
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>		
		protected AssociateRq SendAssociateRq(params PresentationContext[] presentationContexts)
		{
			return(this.dicomThread.SendAssociateRq(presentationContexts));
		}

		/// <summary>
		/// Sends a single Dicom Message while taking care of setting up and releasing an
		/// association for it.
		/// </summary>
		/// <remarks>
		/// 1. An A-ASSOCIATE-RQ is sent.<br></br>
		/// 2. An A-ASSOCIATE-AC or A-ASSOCIATE-RJ is received.<br></br>
		/// Only when an A-ASSOCIATE-AC is received, steps 3, 4 and 5 are executed.<br></br>
		/// 3. The Dicom Message is send and responses are received until the status is not pending anymore.<br></br>
		/// 4. An A-RELEASE-RQ is sent.<br></br>
		/// 5. An A-RELEASE-RP is received.<br></br>
		/// </remarks>
		/// <param name="dicomMessage">The Dicom Message to send.</param>
		/// <param name="presentationContexts">The presentation contexts to propose in the A-ASSOCIATE-RQ.</param>
		/// <returns>
		/// True indicates the other side has accepted the association, false indicates the other side
		/// has rejected the association.
		/// </returns>
		/// <exception cref="System.Exception">
		///	Sending or receiving of one of the Dul or Dicom messages fails or the flow of messages differs
		/// from the flow described.
		/// </exception> 
		protected bool SendAssociation(DicomMessage dicomMessage, params PresentationContext[] presentationContexts)
		{
			return(this.dicomThread.SendAssociation(dicomMessage, presentationContexts));
		}

		/// <summary>
		/// Sends a Dicom Messages while taking care of setting up and releasing an
		/// association for it.
		/// </summary>
		/// <remarks>
		/// 1. An A-ASSOCIATE-RQ is sent.<br></br>
		/// 2. An A-ASSOCIATE-AC or A-ASSOCIATE-RJ is received.<br></br>
		/// Only when an A-ASSOCIATE-AC is received, steps 3, 4 and 5 are executed.<br></br>
		/// 3. For each Dicom Message to send, it is sent and responses are received until the status is not pending anymore.<br></br>
		/// 4. An A-RELEASE-RQ is sent.<br></br>
		/// 5. An A-RELEASE-RP is received.<br></br>
		/// </remarks>
		/// <param name="dicomMessages">The Dicom Messages to send.</param>
		/// <param name="presentationContexts">The presentation contexts to propose in the A-ASSOCIATE-RQ.</param>
		/// <returns>
		/// True indicates the other side has accepted the association, false indicates the other side
		/// has rejected the association.
		/// </returns>
		/// <exception cref="System.Exception">
		///	Sending or receiving of one of the Dul or Dicom messages fails or the flow of messages differs
		/// from the flow described.
		/// </exception> 
		protected bool SendAssociation(DicomMessageCollection dicomMessages, params PresentationContext[] presentationContexts)
		{
			return(this.dicomThread.SendAssociation(dicomMessages, presentationContexts));
		}

		/// <summary>
		/// Sends a Dicom A_RELEASE_RP.
		/// </summary>
		/// <returns>The sent A_RELEASE_RP.</returns>
		/// <exception cref="System.Exception">
		///	Sending of the A_RELEASE_RP fails.
		/// </exception> 		
		protected ReleaseRp SendReleaseRp()
		{
			return(this.dicomThread.SendReleaseRp());
		}

		/// <summary>
		/// Sends an A-RELEASE-RQ.
		/// </summary>
		/// <returns>The sent A-RELEASE-RQ.</returns>
		/// <exception cref="System.Exception">
		///	Sending of the A-RELEASE-RQ fails.
		/// </exception> 
		/// <example>
		///		<b>VB.NET</b>
		///		<code>
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>
		protected ReleaseRq SendReleaseRq()
		{
			return(this.dicomThread.SendReleaseRq());
		}

        /// <summary>
        /// Waits until pending data is available in the network input buffer.
        /// </summary>
        /// <param name="timeOut">The maximum amout of time in milliseconds to wait for pending data.</param>
        /// <param name="waitedTime">Return amout of time in milliseconds waited for pending data.</param>
        /// <returns>Returns a boolean indicating if pending data is available.</returns>
        protected internal bool WaitForPendingDataInNetworkInputBuffer(int timeOut, ref int waitedTime)
        {
            return (DicomThread.WaitForPendingDataInNetworkInputBuffer(timeOut,ref waitedTime));
        }



























		/// <summary>
		/// Write an error to the results.
		/// </summary>
		/// <param name="text">The error text.</param>
		public void WriteError(String text)
		{
			this.DicomThread.WriteError(text);
		}

		/// <summary>
		/// Write information to the results.
		/// </summary>
		/// <param name="text">The information text.</param>
		public void WriteInformation(String text)
		{
			this.DicomThread.WriteInformation(text);
		}

		/// <summary>
		/// Write a warning to the results.
		/// </summary>
		/// <param name="text">The warning text.</param>
		public void WriteWarning(String text)
		{
			this.DicomThread.WriteWarning(text);
		}
	}
}
