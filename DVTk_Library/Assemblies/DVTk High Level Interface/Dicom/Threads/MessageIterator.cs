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
using System.Diagnostics;
using DvtkHighLevelInterface.Dicom.Messages;



namespace DvtkHighLevelInterface.Dicom.Threads
{
	/// <summary>
	/// This class represents a framework to loop through received messages and
	/// react on these. How to react on messsages may both be implemented by a
	/// descendant of this class and by attached MessageHandlers. This class makes it
	/// easier to implements SCP's.
	/// 
	/// Typical behaviour that is implemented in a descendant of this class and not 
	/// in a MessageHandler is control of flow (e.g. determining when to stop listening
	/// for new messages, handling the A-ASSOCIATE-RQ, ...).
	/// 
	/// Advantages of using this class instead of looping through received messages in your
	/// own classes:
	/// - Single way to loop through received messages resulting in more code resembling each other
	///   and avoiding code duplication.
	/// - More easier to re-use code: MessageHandlers may be used in more then one MessageIteraror
	///   descendant.
	/// - "Simpel" MessageHandlers may be combined to implement more complex behaviour in a
	///   MessageIterator descendant they are attached to.
	/// </summary>
	public abstract class MessageIterator: DicomThread
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property IsMessageHandled.
		/// </summary>
		private bool isMessageHandled = false;

		/// <summary>
		/// The list of attached MessageHandlers. The order in which the 
		/// MessageHandlers are stored is of importance. When a message is
		/// received, this object tries to handle the message with a MessageHandler
		/// starting at the beginning of the list. When the message is handled by
		/// a MessageHandler, the MessageHandlers following in the list are not
		/// applied anymore for this specific message.
		/// </summary>
		private ArrayList messageHandlers = new ArrayList();

        /// <summary>
        /// Boolean indicating if this instance should still listen for incoming messages.
        /// </summary>
		internal protected bool receiveMessages = true;



		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor.
		/// </summary>
		public MessageIterator()
		{
			// Do nothing.
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Indicates if the last received message has already been handled by this
		/// object or by its attached MessageHandlers. If a descendant of this class overrides
		/// an AfterHandling... or BeforeHandling... method and handles the received Message, it
		/// should set this property to true.
		/// </summary>
		public bool IsMessageHandled
		{
			get
			{
				return(this.isMessageHandled);
			}
			set
			{
				this.isMessageHandled = value;
			}
		}

		internal ArrayList MessagesHandlers
		{
			get
			{
				return(this.messageHandlers);		
			}
		}


		//
		// - Methods -
		//

		/// <summary>
		/// Add a MessageHandler to the back. When a MessageHandler is at the back,
		/// all MessageHandlers in front may try to handle a received messages first.
		/// </summary>
		/// <param name="messageHandler">The MessageHandler to add.</param>
		public void AddToBack(MessageHandler messageHandler)
		{
			this.messageHandlers.Add(messageHandler);
		}

		/// <summary>
		/// Add a MessageHandler to the front. When a MessageHandler is at the front,
		/// this MessageHandlers may try to handle a received messages first.
		/// </summary>
		/// <param name="messageHandler">The MessageHandler to add.</param>
		public void AddToFront(MessageHandler messageHandler)
		{
			this.messageHandlers.Insert(0, messageHandler);
		}

		/// <summary>
		/// This method is called after an A-ABORT has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="abort">The received A-ABORT.</param>
		public virtual void AfterHandlingAbort(Abort abort)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-ASSOCIATE-AC has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="associateAc">The received A-ASSOCIATE-AC.</param>
		public virtual void AfterHandlingAssociateAccept(AssociateAc associateAc)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-ASSOCIATE-RJ has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="associateRj">The received A-ASSOCIATE-RJ.</param>
		public virtual void AfterHandlingAssociateReject(AssociateRj associateRj)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-ASSOCIATE-RQ has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="associateRq">The received A-ASSOCIATE-RQ.</param>
		public virtual void AfterHandlingAssociateRequest(AssociateRq associateRq)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-CANCEL-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-CANCEL-RQ message.</param>
		protected virtual void AfterHandlingCCancelRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-ECHO-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-ECHO-RQ message.</param>
		protected virtual void AfterHandlingCEchoRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-ECHO-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-ECHO-RSP message.</param>
		protected virtual void AfterHandlingCEchoResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-FIND-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-FIND-RQ message.</param>
		protected virtual void AfterHandlingCFindRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-FIND-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-FIND-RSP message.</param>
		protected virtual void AfterHandlingCFindResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-GET-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-GET-RQ message.</param>
		protected virtual void AfterHandlingCGetRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-GET-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-GET-RSP message.</param>
		protected virtual void AfterHandlingCGetResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-MOVE-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-MOVE-RQ message.</param>
		protected virtual void AfterHandlingCMoveRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-MOVE-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-MOVE-RSP message.</param>
		protected virtual void AfterHandlingCMoveResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-STORE-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-STORE-RQ message.</param>
		protected virtual void AfterHandlingCStoreRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-STORE-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-STORE-RSP message.</param>
		protected virtual void AfterHandlingCStoreResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a Dicom message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// </summary>
		/// <param name="dicomMessage">The received Dicom message.</param>
		private void AfterHandlingDicomMessage(DicomMessage dicomMessage)
		{
			switch(dicomMessage.CommandSet.DimseCommand)
			{
				case DvtkData.Dimse.DimseCommand.CCANCELRQ:
					AfterHandlingCCancelRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CECHORQ:
					AfterHandlingCEchoRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CECHORSP:
					AfterHandlingCEchoResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CFINDRQ:
					AfterHandlingCFindRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CFINDRSP:
					AfterHandlingCFindResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CGETRQ:
					AfterHandlingCGetRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CGETRSP:
					AfterHandlingCGetResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CMOVERQ:
					AfterHandlingCMoveRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CMOVERSP:
					AfterHandlingCMoveResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CSTORERQ:
					AfterHandlingCStoreRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CSTORERSP:
					AfterHandlingCStoreResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NACTIONRQ:
					AfterHandlingNActionRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NACTIONRSP:
					AfterHandlingNActionResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NCREATERQ:
					AfterHandlingNCreateRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NCREATERSP:
					AfterHandlingNCreateResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NDELETERQ:
					AfterHandlingNDeleteRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NDELETERSP:
					AfterHandlingNDeleteResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NEVENTREPORTRQ:
					AfterHandlingNEventReportRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NEVENTREPORTRSP:
					AfterHandlingNEventReportResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NGETRQ:
					AfterHandlingHandleNGetRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NGETRSP:
					AfterHandlingHandleNGetResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NSETRQ:
					AfterHandlingNSetRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NSETRSP:
					AfterHandlingNSetResponse(dicomMessage);
					break;

				default:
					Debug.Assert(true, "Not yet implemented.");
					break;
			}
		}

		/// <summary>
		/// This method is called after a DulMessage has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// </summary>
		/// <param name="dulMessage">The received DulMessage.</param>
		private void AfterHandlingDulMessage(DulMessage dulMessage)
		{
			if (dulMessage is Abort)
			{
				AfterHandlingAbort(dulMessage as Abort);
			}
			else if (dulMessage is AssociateAc)
			{
				AfterHandlingAssociateAccept(dulMessage as AssociateAc);
			}
			else if (dulMessage is AssociateRj)
			{
				AfterHandlingAssociateReject(dulMessage as AssociateRj);
			}
			else if (dulMessage is AssociateRq)
			{
				AfterHandlingAssociateRequest(dulMessage as AssociateRq);
			}
			else if (dulMessage is ReleaseRq)
			{
				AfterHandlingReleaseRequest(dulMessage as ReleaseRq);
			}
			else if (dulMessage is ReleaseRp)
			{
				AfterHandlingReleaseResponse(dulMessage as ReleaseRp);
			}
			else
			{
				Debug.Assert(true, "Not implemented yet.");
			}
		}

		/// <summary>
		/// This method is called after a Message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// </summary>
		/// <param name="message">The received Message.</param>
		private void AfterHandlingMessage(DicomProtocolMessage message)
		{
			if (message is DulMessage)
			{
				AfterHandlingDulMessage(message as DulMessage);
			}
			else if (message is DicomMessage)
			{
				AfterHandlingDicomMessage(message as DicomMessage);
			}
			else
			{
				Debug.Assert(true, "Not supposed to get here.");
			}
		}

		/// <summary>
		/// This method is called after a N-ACTION-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-ACTION-RQ message.</param>
		protected virtual void AfterHandlingNActionRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-ACTION-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-ACTION-RSP message.</param>
		protected virtual void AfterHandlingNActionResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-CREATE-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-CREATE-RQ message.</param>
		protected virtual void AfterHandlingNCreateRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-CREATE-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-CREATE-RSP message.</param>
		protected virtual void AfterHandlingNCreateResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-DELETE-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-DELETE-RQ message.</param>
		protected virtual void AfterHandlingNDeleteRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-DELETE-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-DELETE-RSP message.</param>
		protected virtual void AfterHandlingNDeleteResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-EVENT-REPORT-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-EVENT-REPORT-RQ message.</param>
		protected virtual void AfterHandlingNEventReportRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-EVENT-REPORT-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-EVENT-REPORT-RSP message.</param>
		protected virtual void AfterHandlingNEventReportResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-GET-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-GET-RQ message.</param>
		protected virtual void AfterHandlingHandleNGetRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-GET-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-GET-RSP message.</param>
		protected virtual void AfterHandlingHandleNGetResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-SET-RQ message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-SET-RQ message.</param>
		protected virtual void AfterHandlingNSetRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-SET-RSP message has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-SET-RSP message.</param>
		protected virtual void AfterHandlingNSetResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-RELEASE-RQ has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="releaseRq">The received A-RELEASE-RQ.</param>
		public virtual void AfterHandlingReleaseRequest(ReleaseRq releaseRq)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-RELEASE-RP has been received and has 
		/// (possibly) been handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="releaseRp">The received A-RELEASE-RP.</param>
		public virtual void AfterHandlingReleaseResponse(ReleaseRp releaseRp)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-ABORT has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="abort">The received A-ABORT.</param>
		public virtual void BeforeHandlingAbort(Abort abort)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-ASSOCIATE-AC has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="associateAc">The received A-ASSOCIATE-AC</param>
		public virtual void BeforeHandlingAssociateAccept(AssociateAc associateAc)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-ASSOCIATE-RJ has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="associateRj">The received A-ASSOCIATE-RJ</param>
		public virtual void BeforeHandlingAssociateReject(AssociateRj associateRj)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-ASSOCIATE-RQ has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="associateRq">The received A-ASSOCIATE-RQ</param>
		public virtual void BeforeHandlingAssociateRequest(AssociateRq associateRq)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-CANCEL-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-CANCEL-RQ message.</param>
		protected virtual void BeforeHandlingCCancelRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-ECHO-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-ECHO-RQ message.</param>
		protected virtual void BeforeHandlingCEchoRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-ECHO-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-ECHO-RSP message.</param>
		protected virtual void BeforeHandlingCEchoResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-FIND-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-FIND-RQ message.</param>
		protected virtual void BeforeHandlingCFindRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-FIND-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-FIND-RSP message.</param>
		protected virtual void BeforeHandlingCFindResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-GET-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-GET-RQ message.</param>
		protected virtual void BeforeHandlingCGetRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-GET-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-GET-RSP message.</param>
		protected virtual void BeforeHandlingCGetResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-MOVE-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-MOVE-RQ message.</param>
		protected virtual void BeforeHandlingCMoveRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-MOVE-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-MOVE-RSP message.</param>
		protected virtual void BeforeHandlingCMoveResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-STORE-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-STORE-RQ message.</param>
		protected virtual void BeforeHandlingCStoreRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a C-STORE-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received C-STORE-RSP message.</param>
		protected virtual void BeforeHandlingCStoreResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a Dicom message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// </summary>
		/// <param name="dicomMessage">The received Dicom message.</param>
		private void BeforeHandlingDicomMessage(DicomMessage dicomMessage)
		{
			switch(dicomMessage.CommandSet.DimseCommand)
			{
				case DvtkData.Dimse.DimseCommand.CCANCELRQ:
					BeforeHandlingCCancelRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CECHORQ:
					BeforeHandlingCEchoRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CECHORSP:
					BeforeHandlingCEchoResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CFINDRQ:
					BeforeHandlingCFindRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CFINDRSP:
					BeforeHandlingCFindResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CGETRQ:
					BeforeHandlingCGetRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CGETRSP:
					BeforeHandlingCGetResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CMOVERQ:
					BeforeHandlingCMoveRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CMOVERSP:
					BeforeHandlingCMoveResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CSTORERQ:
					BeforeHandlingCStoreRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.CSTORERSP:
					BeforeHandlingCStoreResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NACTIONRQ:
					BeforeHandlingNActionRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NACTIONRSP:
					BeforeHandlingNActionResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NCREATERQ:
					BeforeHandlingNCreateRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NCREATERSP:
					BeforeHandlingNCreateResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NDELETERQ:
					BeforeHandlingNDeleteRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NDELETERSP:
					BeforeHandlingNDeleteResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NEVENTREPORTRQ:
					BeforeHandlingNEventReportRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NEVENTREPORTRSP:
					BeforeHandlingNEventReportResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NGETRQ:
					BeforeHandlingHandleNGetRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NGETRSP:
					BeforeHandlingHandleNGetResponse(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NSETRQ:
					BeforeHandlingNSetRequest(dicomMessage);
					break;

				case DvtkData.Dimse.DimseCommand.NSETRSP:
					BeforeHandlingNSetResponse(dicomMessage);
					break;

				default:
					Debug.Assert(true, "Not yet implemented.");
					break;
			}
		}

		/// <summary>
		/// This method is called after a DulMessage has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// </summary>
		/// <param name="dulMessage">The received DulMessage.</param>
		private void BeforeHandlingDulMessage(DulMessage dulMessage)
		{
			if (dulMessage is Abort)
			{
				BeforeHandlingAbort(dulMessage as Abort);
			}
			else if (dulMessage is AssociateAc)
			{
				BeforeHandlingAssociateAccept(dulMessage as AssociateAc);
			}
			else if (dulMessage is AssociateRj)
			{
				BeforeHandlingAssociateReject(dulMessage as AssociateRj);
			}
			else if (dulMessage is AssociateRq)
			{
				BeforeHandlingAssociateRequest(dulMessage as AssociateRq);
			}
			else if (dulMessage is ReleaseRq)
			{
				BeforeHandlingReleaseRequest(dulMessage as ReleaseRq);
			}
			else if (dulMessage is ReleaseRp)
			{
				BeforeHandlingReleaseResponse(dulMessage as ReleaseRp);
			}
			else
			{
				Debug.Assert(true, "Not implemented yet.");
			}
		}

		/// <summary>
		/// This method is called after a Message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// </summary>
		/// <param name="message">The received message.</param>
		private void BeforeHandlingMessage(DicomProtocolMessage message)
		{
			if (message is DulMessage)
			{
				BeforeHandlingDulMessage(message as DulMessage);
			}
			else if (message is DicomMessage)
			{
				BeforeHandlingDicomMessage(message as DicomMessage);
			}
			else
			{
				Debug.Assert(true, "Not supposed to get here.");
			}
		}

		/// <summary>
		/// This method is called after a N-ACTION-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-ACTION-RQ message.</param>
		protected virtual void BeforeHandlingNActionRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-ACTION-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-ACTION-RSP message.</param>
		protected virtual void BeforeHandlingNActionResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-CREATE-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-CREATE-RQ message.</param>
		protected virtual void BeforeHandlingNCreateRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-CREATE-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-CREATE-RSP message.</param>
		protected virtual void BeforeHandlingNCreateResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-DELETE-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-DELETE-RQ message.</param>
		protected virtual void BeforeHandlingNDeleteRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-DELETE-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-DELETE-RSP message.</param>
		protected virtual void BeforeHandlingNDeleteResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-EVENT-REPORT-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-EVENT-REPORT-RQ message.</param>
		protected virtual void BeforeHandlingNEventReportRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-EVENT-REPORT-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-EVENT-REPORT-RSP message.</param>
		protected virtual void BeforeHandlingNEventReportResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-GET-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-GET-RQ message.</param>
		protected virtual void BeforeHandlingHandleNGetRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-GET-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-GET-RSP message.</param>
		protected virtual void BeforeHandlingHandleNGetResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-SET-RQ message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-SET-RQ message.</param>
		protected virtual void BeforeHandlingNSetRequest(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after a N-SET-RSP message has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="dicomMessage">The received N-SET-RSP message.</param>
		protected virtual void BeforeHandlingNSetResponse(DicomMessage dicomMessage)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-RELEASE-RQ has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="releaseRq">The received A-RELEASE-RQ.</param>
		public virtual void BeforeHandlingReleaseRequest(ReleaseRq releaseRq)
		{
			// Do nothing.
		}

		/// <summary>
		/// This method is called after an A-RELEASE-RP has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// Default, nothing is done in this method. Override if needed.
		/// </summary>
		/// <param name="releaseRp">The received A-RELEASE-RP.</param>
		public virtual void BeforeHandlingReleaseResponse(ReleaseRp releaseRp)
		{
			// Do nothing.
		}

		/// <summary>
		/// Implements the loop that loops through the received messages and tries to
		/// handle them with the implementation of a descendant of this class and
		/// attached MessageHandlers.
		/// 
		/// This method will be executed in a seperate thread when the Start method is called on
		/// an instance of this class.
		/// </summary>
		protected override void Execute()
		{
			InitialAction();

			if (this.receiveMessages)
			{
				DicomProtocolMessage lastReceivedMessage = ReceiveMessage();

				// In each iteration of this loop, the last received message from the ScriptSession is handled.
				// Calling of the Stop method while break through this loop.
				while (this.receiveMessages)
				{
					this.isMessageHandled = false;

                    BeforeHandlingMessage(lastReceivedMessage);
			
					if (!this.isMessageHandled)
					{
						foreach(MessageHandler messageHandler in this.messageHandlers)
						{	
							messageHandler.DicomThread = this;
							this.isMessageHandled = messageHandler.HandleMessage(lastReceivedMessage);

							if (this.isMessageHandled)
							{
								break;
							}
						}
					}

					AfterHandlingMessage(lastReceivedMessage);

					if (!this.isMessageHandled)
					{
						WriteError("Last received Message of type " + lastReceivedMessage.ToString() + " is not handled.");
						Stop();
					}

					if (this.receiveMessages)
					{
						lastReceivedMessage = ReceiveMessage();
					}
				}
			}
		}	

		/// <summary>
		/// An descendant of this class may override this method to perform some extra
		/// actions before the main loop is entered.
		/// 
		/// A reason for putting code in this place instead of the constructor is that all Write... methods
		/// work (if results are gathered).
		/// 
		/// Default, nothing is done in this method.
		/// </summary>
		protected virtual void InitialAction()
		{
			// Do nothing.
		}
	}
}
