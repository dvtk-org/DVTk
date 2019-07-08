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
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

using DvtkHighLevelInterface.Common.Messages;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using ThreadState = DvtkHighLevelInterface.Common.Threads.ThreadState;
using System.Collections.Generic;



namespace DvtkHighLevelInterface.Dicom.Threads
{
	/// <summary>
	/// Represents a single thread in which Dicom communication may be tested.
	/// </summary>
	public abstract class DicomThread: Thread
	{		
		//
		// - Fields -
		//

		/// <summary>
		/// The event handler that will handle ActivityReportEvents that are 
		/// fired by the encapsulated Dvtk ScriptSession.
		/// </summary>
		private Dvtk.Events.ActivityReportEventHandler activityReportEventHandler = null;

		/// <summary>
		/// See property DvtkScriptSession.
		/// </summary>
		private Dvtk.Sessions.ScriptSession dvtkScriptSession = null;

        /// <summary>
        /// Indicates if this instance has een open connection.
        /// </summary>
        private bool hasOpenConnection = false;

        /// <summary>
        /// Under construction.
        /// </summary>
        internal ArrayList inboundDicomMessageFilters = new ArrayList();

        private AssociateAc lastAssociateAc = null;

        /// <summary>
        /// Under construction.
        /// </summary>
        internal ArrayList outboundDicomMessageFilters = new ArrayList();



        //
        // - Delegates -
        //

        /// <summary>
        /// Delegate used for the AssociationReleasedEvent event.
        /// </summary>
        /// <param name="dicomThread"></param>
        [System.Obsolete("Use more generic MessageReceivedEvent or SendingMessageEvent.")]
        public delegate void AssociationReleasedEventHandler(DicomThread dicomThread);

		/// <summary>
		/// Delegate used for the MessageReceivedEvent event.
		/// </summary>
		public delegate void MessageReceivedEventHandler(DicomProtocolMessage dicomProtocolMessage);

		/// <summary>
		/// Delegate used for the SendingMessageEvent event.
		/// </summary>
		public delegate void SendingMessageEventHandler(DicomProtocolMessage dicomProtocolMessage);



        //
        // - Events -
        //

        /// <summary>
        /// Event is fired when an A-RELEASE-RQ has been sent or received.
        /// </summary>
        [System.Obsolete("Use more generic MessageReceivedEvent or SendingMessageEvent.")]
        public event AssociationReleasedEventHandler AssociationReleasedEvent;

		/// <summary>
		/// This event is triggered when this instance has received a message.
		/// </summary>
		public event MessageReceivedEventHandler MessageReceivedEvent;

		/// <summary>
		/// This event is triggered just before this instance is going to send a message.
		/// </summary>
		public event SendingMessageEventHandler SendingMessageEvent;



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DicomThread()
		{
			// Do nothing.
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets or sets the encapsulated Dvtk ScriptSession.
		/// </summary>
		internal Dvtk.Sessions.ScriptSession DvtkScriptSession
		{
			get
			{
				return(this.dvtkScriptSession);
			}
			set
			{
				this.dvtkScriptSession = value;
			}
		}

		/// <summary>
		/// Gets the collection of received and send messages by this instance.
		/// 
		/// Note that messages may have been removed from this collection using the RemoveMessage or 
		/// ClearMessages methods.
		/// </summary>
		public DicomProtocolMessageCollection Messages
		{
			get
			{
				DicomProtocolMessageCollection copyOfList = new DicomProtocolMessageCollection();

				lock(ThreadManager.MessageLock)
				{
					foreach(Message message in this.messages)
					{
						DicomProtocolMessage dicomProtocolMessage = message as DicomProtocolMessage;
						copyOfList.Add(dicomProtocolMessage);
					}
				}

				return(copyOfList);
			}
		}

        public List<string> WarningMessages
        {
            get
            {
                return (this.dvtkScriptSession.WarningMessageList);
            }
        }

        public List<string> ErrorMessages
        {
            get
            {
                return (this.dvtkScriptSession.ErrorMessageList);
            }
        }

		/// <summary>
		/// Gets the number of errors found during execution of this thread and all child threads.
		/// </summary>
		public int NrOfErrors
		{
			get
			{
				return (int)this.dvtkScriptSession.NrOfErrors;
			}
		}

		/// <summary>
		/// Gets or sets the number of general errors found during execution of this thread and all child threads.
		/// </summary>
		public int NrOfGeneralErrors
		{
			get
			{
				return (int)this.dvtkScriptSession.NrOfGeneralErrors;
			}
			set
			{
				this.dvtkScriptSession.NrOfGeneralErrors = (uint)value;
			}
		}

		/// <summary>
		/// Gets or sets the number of general warnings found during execution of this thread and all child threads.
		/// </summary>
		public uint NrOfGeneralWarnings
		{
			get
			{
				return this.dvtkScriptSession.NrOfGeneralWarnings;
			}
			set
			{
				this.dvtkScriptSession.NrOfGeneralWarnings = value;
			}
		}


		/// <summary>
		/// Gets or sets the number of user errors found during execution of this thread and all child threads.
		/// </summary>
		public uint NrOfUserErrors
		{
			get
			{
				return this.dvtkScriptSession.NrOfUserErrors;
			}
			set
			{
				this.dvtkScriptSession.NrOfUserErrors = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of user warnings found during execution of this thread and all child threads.
		/// </summary>
		public uint NrOfUserWarnings
		{
			get
			{
				return this.dvtkScriptSession.NrOfUserWarnings;
			}
			set
			{
				this.dvtkScriptSession.NrOfUserWarnings = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of validation errors found during execution of this thread and all child threads.
		/// </summary>
		public uint NrOfValidationErrors
		{
			get
			{
				return this.dvtkScriptSession.NrOfValidationErrors;
			}
			set
			{
				this.dvtkScriptSession.NrOfValidationErrors = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of validation warnings found during execution of this thread and all child threads.
		/// </summary>
		public uint NrOfValidationWarnings
		{
			get
			{
				return this.dvtkScriptSession.NrOfValidationWarnings;
			}
			set
			{
				this.dvtkScriptSession.NrOfValidationWarnings = value;
			}
		}

		/// <summary>
		/// Gets the number of warnings found during execution of this thread and all child threads.
		/// </summary>
		public int NrOfWarnings
		{
			get
			{
				return (int)this.dvtkScriptSession.NrOfWarnings;
			}
		}

		/// <summary>
		/// Gets the options for this instance.
		/// </summary>
		public new DicomThreadOptions Options
		{
			get
			{				
				// this.threadOptions must be of type DicomThreadOptions,
				// because it is set in the constructor of this class.
				return(this.threadOptions as DicomThreadOptions);
			}
		}



		//
		// - Methods -
		//

        /// <summary>
        /// Under construction.
        /// </summary>
        /// <param name="outboundDicomMessageFilter">Under construction.</param>
        public void AddToBack(OutboundDicomMessageFilter outboundDicomMessageFilter)
        {
            this.outboundDicomMessageFilters.Add(outboundDicomMessageFilter);
        }

        /// <summary>
        /// Under construction.
        /// </summary>
        /// <param name="inboundDicomMessageFilter">Under construction.</param>
        public void AddToBack(InboundDicomMessageFilter inboundDicomMessageFilter)
        {
            this.inboundDicomMessageFilters.Add(inboundDicomMessageFilter);
        }

        /// <summary>
        /// Under construction.
        /// </summary>
        /// <param name="outboundDicomMessageFilter">Under construction.</param>
        public void AddToFront(OutboundDicomMessageFilter outboundDicomMessageFilter)
        {
            this.outboundDicomMessageFilters.Insert(0, outboundDicomMessageFilter);
        }

        /// <summary>
        /// Under construction.
        /// </summary>
        /// <param name="inboundDicomMessageFilter">Under construction.</param>
        public void AddToFront(InboundDicomMessageFilter inboundDicomMessageFilter)
        {
            this.inboundDicomMessageFilters.Insert(0, inboundDicomMessageFilter);
        }

		/// <summary>
		/// Called after the HandleException method in the base Thread class has been called.
		/// </summary>
        /// <param name="exception">Exception that has been handled.</param>
        protected override void AfterHandlingException(System.Exception exception)
		{
			// Reset the TCP/IP connection, just to be sure.
			// See if this improves the ability to again execute the same script after an exception.
			this.dvtkScriptSession.ResetAssociation();			
		}

		/// <summary>
		/// Called after the WaitForCompletionChildThreads method in the base Thread class has been called.
		/// </summary>
		internal override void AfterWaitingForCompletionChildThreads()
		{
			if (Options.LogWaitingForCompletionChildThreads)
			{
				lock(this.ThreadManager.ThreadManagerLock)
				{
					if (this.childs.Count > 0)
					{
						WriteInformation("...child Threads completed.");
					}
				}
			}

			UpdateErrorAndWarningCount();

			String text = String.Format("DicomThread \"{0}\" has ended with {1} errors and {2} warnings.", Options.Identifier, this.dvtkScriptSession.NrOfErrors, this.dvtkScriptSession.NrOfWarnings);

			// If this Thread has a parent Thread, report the ending to it.
			if (this.parent != null)
			{
				if (Options.LogThreadStartingAndStoppingInParent)
				{
					this.parent.WriteInformation(text);
				}
				else
				{
					TriggerInformationOutputEvent(text);
				}
			}
				// If this is the topmost thread make the ending through an
				// output event.
			else
			{
				if (this.dvtkScriptSession.NrOfErrors > 0)
				{
					TriggerErrorOutputEvent(text);
				}
				else if (this.dvtkScriptSession.NrOfWarnings > 0)
				{
					TriggerWarningOutputEvent(text);
				}
				else
				{
					TriggerInformationOutputEvent(text);
				}
			}

			if (Options.LogChildThreadsOverview)
			{
				//
				// Make a HTML table in which the different child Threads with hyperlinks (if possible) are stated.
				// If no child Threads exist, don't display the table.
				//

				String htmlText = "";

				lock(ThreadManager.ThreadManagerLock)
				{
					htmlText = childs.GetStartedThreadsOverviewAsHTML("Started Child Threads Overview", "No child Threads have been started.", Options.ResultsDirectory); 
				}

				WriteHtml(htmlText, true, true, true);
			}

			this.dvtkScriptSession.ActivityReportEvent -= activityReportEventHandler;
		}

		/// <summary>
		/// Called before the Execute method in the base Thread class will be called.
		/// </summary>
		internal override void BeforeCallingExecute()
		{
			this.dvtkScriptSession.ActivityReportEvent += activityReportEventHandler;

			if (Options.LogThreadStartingAndStoppingInParent)
			{
				if (this.parent != null)
				{
					this.parent.WriteInformation(String.Format("DicomThread \"{0}\" has started.", Options.Identifier));
				}
			}

			WriteHtml("<br /><b> DicomThread \"" + Options.Identifier + "\"</b><br />", true, true, true);
		}

		/// <summary>
		/// Called before the WaitForCompletionChildThreads method in the base Thread class will be called.
		/// </summary>
		internal override void BeforeWaitingForCompletionChildThreads()
		{
			if (Options.LogWaitingForCompletionChildThreads)
			{
				lock(ThreadManager.ThreadManagerLock)
				{
					if (this.childs.Count > 0)
					{
						WriteInformation("Waiting for child Threads to complete...");
					}
				}
			}
		}

        /// <summary>
        /// compares pixel data in two DICOM messages.
        /// </summary>
        /// <param name="dicomMessage1">The first Dicom Message.</param>
        /// <param name="dicomMessage2">The second Dicom Message.</param>
        /// <returns>Indicates if the pixel data is the same.</returns>
        public bool ComparePixelData(DicomMessage dicomMessage1, DicomMessage dicomMessage2)
        {
            return (this.DvtkScriptSession.ComparePixelData(dicomMessage1.DvtkDataDicomMessage, dicomMessage2.DvtkDataDicomMessage));
        }

        private String GetDefinitionFullFileName(DicomMessage dicomMessage)
        {
            String definitionFullFileName = String.Empty;

            // Get the DIMSE command
            DvtkData.Dimse.DimseCommand dimseCommand = dicomMessage.CommandSet.DvtkDataCommandSet.CommandField;

            // Get the SOP Class UID.
            String sopClassUid = dicomMessage.CommandSet.GetSopClassUid();

            if (sopClassUid.Length > 0)
            {
                // Try to get the defintion full file name from the loaded definition files that contains this
                // specific DimseCommand and SOP class UID.
                definitionFullFileName = this.dvtkScriptSession.DefinitionManagement.GetFileNameFromSOPUID(dimseCommand, sopClassUid);
            }

            // return the defintion full file name - is empty if no match found
            return definitionFullFileName;

        }

        /// <summary>
        /// Gets the IOD Name for the supplied DicomMessage using the loaded definition files.
        /// </summary>
        /// <remarks>
        /// The DimseCommand and SOP Class UID specified in the CommandSet of the DicomMessage is
        /// used to get the IOD Name using the loaded definition files.
        /// </remarks>
        /// <param name="dicomMessage">DicomMessage for which the IOD Name is being sought.</param>
        /// <returns>
        /// The IOD Name for the DicomMessage or an empty String if no matching definition file is
        /// found in the loaded definition files.
        /// </returns>
        public System.String GetIodNameFromDefinition(DicomMessage dicomMessage)
        {
            String iodName = String.Empty;

            // Get the DIMSE command
            DvtkData.Dimse.DimseCommand command = dicomMessage.CommandSet.DvtkDataCommandSet.CommandField;

            // Get the SOP Class UID.
            String sopClassUid = dicomMessage.CommandSet.GetSopClassUid();

            if (sopClassUid.Length > 0)
            {
                // Try to get the IOD Name from the loaded definition files
                iodName = this.dvtkScriptSession.DefinitionManagement.GetIodNameFromDefinition(command, sopClassUid);
            }

            // return the IOD Name - maybe empty if no match found
            return iodName;
        }

		/// <summary>
		/// This method will be called when the encapsulated Dvtk ScriptSession object raises
		/// an ActivityReportEvent.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The content of the event.</param>
		private void HandleActivityReportEvent(object sender, Dvtk.Events.ActivityReportEventArgs e)
		{
			switch(e.ReportLevel)
			{
				case Dvtk.Events.ReportLevel.Error:
					TriggerErrorOutputEvent(e.Message);
					break;

				case Dvtk.Events.ReportLevel.Warning:
					TriggerWarningOutputEvent(e.Message);
					break;

				default:
					TriggerInformationOutputEvent(e.Message);
					break;
			}			
		}

		/// <summary>
		/// Conatins common initialization code, called from the other Initialize methods from
		/// this class.
		/// </summary>
		/// <example>
		///		<b>C#</b>
		///		<code>
        /// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>		
		private void Initialize()
		{
			this.threadOptions = new DicomThreadOptions(this);

			this.activityReportEventHandler = new Dvtk.Events.ActivityReportEventHandler(this.HandleActivityReportEvent);
		}

		/// <summary>
		/// Initializes this instance as a DicomThread with no parent thread.
		/// </summary>
		/// <param name="threadManager">The threadManager.</param>
		/// <example>
		///		<b>VB.NET</b>
		///		<code>
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>
		public new void Initialize(ThreadManager threadManager)
		{
			// Initialize may only be called once, so check for this.
			if (this.isInitialized)
			{
				throw new HliException(alreadyInitializedErrorText);
			}

			base.Initialize(threadManager);
			this.dvtkScriptSession = new Dvtk.Sessions.ScriptSession();
			Initialize();
			this.isInitialized = true;
		}

		/// <summary>
		/// Initializes this instance as a DicomThread with a parent thread.
		/// </summary>
		/// <param name="parent">The parent thread.</param>
		public new void Initialize(Thread parent)
		{
			// Initialize may only be called once, so check for this.
			if (this.isInitialized)
			{
				throw new HliException(alreadyInitializedErrorText);
			}

			base.Initialize(parent);
			this.dvtkScriptSession = new Dvtk.Sessions.ScriptSession();
			Initialize();
			this.isInitialized = true;
		}

		/// <summary>
		/// Method must be called when a message has been received.
		/// </summary>
		/// <param name="dicomProtocolMessage">The message that has been received.</param>
		private void MessageReceived(DicomProtocolMessage dicomProtocolMessage)
		{
			dicomProtocolMessage.IsReceived = true;

			AddMessage(dicomProtocolMessage);

			if (MessageReceivedEvent != null)
			{
				MessageReceivedEvent(dicomProtocolMessage);
			}
		}

		/// <summary>
		/// Receives a An-ASSOCIATE-AC.
		/// </summary>
		/// <returns>The received A-ASSOCIATE-AC</returns>
		/// <example>
		///		<b>VB.NET</b>
		///		<code>
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
		/// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
		///		</code>
		/// </example>
		public AssociateAc ReceiveAssociateAc()
		{
			AssociateAc receivedAssociateAc = null;

			DicomProtocolMessage receivedMessage = ReceiveMessage();

			if (receivedMessage.IsAssociateAc)
			{
				receivedAssociateAc = receivedMessage.AssociateAc;
			}
			else
			{
				throw new HliException("Script expects AssociateAc. " + receivedMessage.ToString() + " however received.");
			}

			return receivedAssociateAc;
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
        protected internal AssociateRj ReceiveAssociateRj()
        {
            AssociateRj receivedAssociateRj = null;

            DicomProtocolMessage receivedMessage = ReceiveMessage();

            if (receivedMessage.IsAssociateRj)
            {
                receivedAssociateRj = receivedMessage.AssociateRj;
            }
            else
            {
                throw new System.Exception("Script expects AssociateRj. " + receivedMessage.ToString() + " however received.");
            }

            return receivedAssociateRj;
        }

        /// <summary>
        /// Receives an A-ASSOCIATE-AC or A-ASSOCIATE-RJ.
        /// </summary>
        /// <returns>The received A-ASSOCIATE-AC or A-ASSOCIATE-RJ.</returns>
        /// <exception cref="System.Exception">
        ///	Receiving of a message fails or the received message is not an A-ASSOCIATE-AC or A-ASSOCIATE-RJ.
        /// </exception> 
        protected internal DulMessage ReceiveAssociateRp()
        {
            DulMessage receivedDulMessage = null;

            DicomProtocolMessage receivedMessage = ReceiveMessage();

            if (receivedMessage.IsAssociateRj || receivedMessage.IsAssociateAc)
            {
                receivedDulMessage = receivedMessage as DulMessage;
            }
            else
            {
                throw new System.Exception("Script expects AssociateAc or AssociateRj. " + receivedMessage.ToString() + " however received.");
            }

            return receivedDulMessage;
        }

        /// <summary>
        /// Receives a A-ASSOCIATE-RQ.
        /// </summary>
        /// <returns>The received A-ASSOCIATE-RQ.</returns>
        /// <exception cref="System.Exception">
        ///	Receiving of a message fails or the received message is not an A-ASSOCIATE-RQ.
        /// </exception> 
        protected internal AssociateRq ReceiveAssociateRq()
        {
            AssociateRq receivedAssociateRq = null;

            DicomProtocolMessage receivedMessage = ReceiveMessage();

            if (receivedMessage.IsAssociateRq)
            {
                receivedAssociateRq = receivedMessage.AssociateRq;
            }
            else
            {
                throw new System.Exception("Script expects AssociateRq. " + receivedMessage.ToString() + " however received.");
            }

            return receivedAssociateRq;
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
        protected internal DicomMessage ReceiveDicomMessage()
        {
            DicomMessage receivedDicomMessage = null;

            DicomProtocolMessage receivedMessage = ReceiveMessage();

            if (receivedMessage.IsDicomMessage)
            {
                receivedDicomMessage = receivedMessage.DicomMessage;
            }
            else
            {
                throw new System.Exception("Script expects DicomMessage. " + receivedMessage.ToString() + " however received.");
            }

            return receivedDicomMessage;
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
        protected internal DicomProtocolMessage ReceiveMessage()
        {
            return (ReceiveMessage(""));
        }

        /// <summary>
        /// Receives a messages (can be a Dicom or Dul message).
        /// </summary>
        /// <param name="messageToExpect">Message to expect that is written in the results.</param>
        /// <returns>The received message.</returns>
        /// <exception cref="DicomProtocolMessageReceiveException">
        ///	Receiving of a message fails.
        /// </exception> 
        private DicomProtocolMessage ReceiveMessage(String messageToExpect)
        {
            DicomProtocolMessage receivedMessage = null;

            DvtkData.Message dvtkDataMessage = null;

            if (this.hasOpenConnection)
            {
                if (messageToExpect == "")
                {
                    WriteInformation("Receiving message...");
                }
                else
                {
					WriteInformation("Receiving message (expecting " +  messageToExpect + ")...");
                }
            }
            else
            {
                WriteInformation(String.Format("Listening for incoming Dicom connection on port {0}...", Options.LocalPort));
            }

            Dvtk.Sessions.ReceiveReturnCode receiveReturnCode = DvtkScriptSession.Receive(out dvtkDataMessage);

            if (receiveReturnCode != Dvtk.Sessions.ReceiveReturnCode.Success)
            {
                throw new DicomProtocolMessageReceiveException("Error while trying to receive a Message. Error code " + receiveReturnCode.ToString() + ".", receiveReturnCode);
            }
            else
            {
                if (dvtkDataMessage is DvtkData.Dimse.DicomMessage)
                {
                    DicomMessage receivedDicomMessage = new DicomMessage(dvtkDataMessage as DvtkData.Dimse.DicomMessage);

                    // Apply the inbound DicomMessage filters if this is a DicomMessage.
                    foreach (InboundDicomMessageFilter inboundDicomMessageFilter in this.inboundDicomMessageFilters)
                    {
                        inboundDicomMessageFilter.Apply(receivedDicomMessage);
                    }

                    receivedMessage = receivedDicomMessage;
                }
                else if (dvtkDataMessage is DvtkData.Dul.A_ASSOCIATE_RQ)
                {
                    receivedMessage = new AssociateRq(dvtkDataMessage as DvtkData.Dul.A_ASSOCIATE_RQ);
                    hasOpenConnection = true;
                }
                else if (dvtkDataMessage is DvtkData.Dul.A_ASSOCIATE_AC)
                {
                    receivedMessage = new AssociateAc(dvtkDataMessage as DvtkData.Dul.A_ASSOCIATE_AC);
                    this.lastAssociateAc = receivedMessage as AssociateAc;
                }
                else if (dvtkDataMessage is DvtkData.Dul.A_ASSOCIATE_RJ)
                {
                    receivedMessage = new AssociateRj(dvtkDataMessage as DvtkData.Dul.A_ASSOCIATE_RJ);
                    hasOpenConnection = false;
                }
                else if (dvtkDataMessage is DvtkData.Dul.A_RELEASE_RQ)
                {
                    receivedMessage = new ReleaseRq(dvtkDataMessage as DvtkData.Dul.A_RELEASE_RQ);
                }
                else if (dvtkDataMessage is DvtkData.Dul.A_RELEASE_RP)
                {
                    receivedMessage = new ReleaseRp(dvtkDataMessage as DvtkData.Dul.A_RELEASE_RP);
                    hasOpenConnection = false;
                }
                else if (dvtkDataMessage is DvtkData.Dul.A_ABORT)
                {
                    receivedMessage = new Abort(dvtkDataMessage as DvtkData.Dul.A_ABORT);
                    hasOpenConnection = false;
                }
                else
                {
                    Debug.Assert(true, "Unexpected DvtkData Message descendant type.");
                }

                WriteInformation("... " + receivedMessage.ToString() + " received.");

                // If the options AutoValidate is true, try to validate as much 
                // as possible for the received message.
                if (Options.AutoValidate)
                {
                    Validate(receivedMessage);
                }

				MessageReceived(receivedMessage);

                if (receivedMessage is ReleaseRq)
                {
                    if (AssociationReleasedEvent != null)
                    {
                        AssociationReleasedEvent(this);
                    }
                }
            }

            return (receivedMessage);
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
        protected internal ReleaseRp ReceiveReleaseRp()
        {
            ReleaseRp receivedReleaseRp = null;

            DicomProtocolMessage receivedMessage = ReceiveMessage();

            if (receivedMessage.IsReleaseRp)
            {
                receivedReleaseRp = receivedMessage.ReleaseRp;
            }
            else
            {
                throw new System.Exception("Script expects ReleaseRp. " + receivedMessage.ToString() + " however received.");
            }

            return receivedReleaseRp;
        }

        /// <summary>
        /// Receives a A-RELEASE-RQ.
        /// </summary>
        /// <returns>The received A-RELEASE-RQ.</returns>
        /// <exception cref="System.Exception">
        ///	Receiving of a message fails or the received message is not an A-RELEASE-RQQ.
        /// </exception> 
        protected internal ReleaseRq ReceiveReleaseRq()
        {
            ReleaseRq receivedReleaseRq = null;

            DicomProtocolMessage receivedMessage = ReceiveMessage();

            if (receivedMessage.IsReleaseRq)
            {
                receivedReleaseRq = receivedMessage.ReleaseRq;
            }
            else
            {
                throw new System.Exception("Script expects ReleaseRq. " + receivedMessage.ToString() + " however received.");
            }

            return receivedReleaseRq;
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
        protected internal void Send(DicomMessage dicomMessage)
        {
            // Apply the (possible) outbound filters to add/remove/change attributes in the Dicom message to send.
            foreach (OutboundDicomMessageFilter outboundDicomMessageFilter in this.outboundDicomMessageFilters)
            {
                outboundDicomMessageFilter.Apply(dicomMessage);
            }

			SendMessage(dicomMessage, -1);
        }

        /// <summary>
        /// Sends a Dicom Message.
        /// </summary>
        /// <param name="dicomMessage">The DicomMessage.</param>
        /// <param name="presentationContextId">The ID of the presentation context to use.</param>
        protected internal void Send(DicomMessage dicomMessage, int presentationContextId)
        {
            // Apply the (possible) outbound filters to add/remove/change attributes in the Dicom message to send.
            foreach (OutboundDicomMessageFilter outboundDicomMessageFilter in this.outboundDicomMessageFilters)
            {
                outboundDicomMessageFilter.Apply(dicomMessage);
            }

            SendMessage(dicomMessage, presentationContextId);
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
        protected internal Abort SendAbort(Byte source, Byte reason)
        {
            Abort abort = new Abort(source, reason);

			SendMessage(abort);

            return (abort);
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
		protected internal AssociateAc SendAssociateAc()
		{
			AssociateAc associateAc = null;

			DicomProtocolMessage lastReceivedMessage = Messages.LastReceivedMessage;

			if (lastReceivedMessage is AssociateRq)
			{
				PresentationContextCollection presentationContextsForAssociateAc = 
					(lastReceivedMessage as AssociateRq).CreatePresentationContextsForAssociateAc();

				// Send an A-ASSOCIATE-AC.
				associateAc = SendAssociateAc(presentationContextsForAssociateAc);
			}
			else
			{
				throw new System.Exception("Calling SendAssociateAc() while last received messages is not an A-ASSOCIATE-RQ.");
			}
	
			return(associateAc);
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
		protected internal AssociateAc SendAssociateAc(PresentationContextCollection presentationContextCollection)
		{
			//
			// Check the parameter(s).
			//

			foreach(PresentationContext presentationContext in presentationContextCollection)
			{
				if (!presentationContext.IsForAssociateAccept)
				{
					throw new System.Exception("PresentationContext instance supplied that is not suitable for an A-ASSOCIATE-AC.");
				}
			}


			//
			// Construct the AssociateAc instance.
			//

			AssociateAc associateAc = new AssociateAc
				(Options.RemoteAeTitle, 
				Options.LocalAeTitle,
				Options.LocalMaximumLength,
				Options.LocalImplementationClassUid,
                Options.LocalImplementationVersionName);

			foreach(PresentationContext presentationContext in presentationContextCollection)
			{
				associateAc.DvtkDataAssociateAc.AddPresentationContexts(presentationContext.DvtkDataAcceptedPresentationContext);
			}


			//
			// Send the A-ASSOCIATE-AC.
			//

			SendMessage(associateAc);

			return(associateAc);
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
		protected internal AssociateAc SendAssociateAc(params PresentationContext[] presentationContexts)
		{
			PresentationContextCollection presentationContextCollection	= new PresentationContextCollection();
	
			foreach(PresentationContext presentationContext in presentationContexts)
			{
				presentationContextCollection.Add(presentationContext);
			}

			return(SendAssociateAc(presentationContextCollection));
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
		protected internal AssociateAc SendAssociateAc(SopClasses sopClasses)
		{
			AssociateAc associateAc = null;

			DicomProtocolMessage lastReceivedMessage = Messages.LastReceivedMessage;

			if (lastReceivedMessage is AssociateRq)
			{
				PresentationContextCollection presentationContextsForAssociateAc = (lastReceivedMessage as AssociateRq).CreatePresentationContextsForAssociateAc(sopClasses);

				associateAc = SendAssociateAc(presentationContextsForAssociateAc);
			}
			else
			{
				throw new System.Exception("Calling SendAssociateAc() while last received messages is not an A-ASSOCIATE-RQ.");
			}
	
			return(associateAc);
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
		protected internal AssociateAc SendAssociateAc(SopClasses sopClasses, params TransferSyntaxes[] transferSyntaxesList)
		{
			AssociateAc associateAc = null;

			DicomProtocolMessage lastReceivedMessage = Messages.LastReceivedMessage;

			if (lastReceivedMessage is AssociateRq)
			{
				PresentationContextCollection presentationContextsForAssociateAc = (lastReceivedMessage as AssociateRq).CreatePresentationContextsForAssociateAc(sopClasses, transferSyntaxesList);

				associateAc = SendAssociateAc(presentationContextsForAssociateAc);
			}
			else
			{
				throw new System.Exception("Calling SendAssociateAc() while last received messages is not an A-ASSOCIATE-RQ.");
			}
	
			return(associateAc);
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
		protected internal AssociateAc SendAssociateAc(params TransferSyntaxes[] transferSyntaxesList)
		{
			AssociateAc associateAc = null;

			DicomProtocolMessage lastReceivedMessage = Messages.LastReceivedMessage;

			if (lastReceivedMessage is AssociateRq)
			{
				PresentationContextCollection presentationContextsForAssociateAc = (lastReceivedMessage as AssociateRq).CreatePresentationContextsForAssociateAc(transferSyntaxesList);

				associateAc = SendAssociateAc(presentationContextsForAssociateAc);
			}
			else
			{
				throw new System.Exception("Calling SendAssociateAc() while last received messages is not an A-ASSOCIATE-RQ.");
			}
	
			return(associateAc);
		}

        /// <summary>
        /// Sends a Dicom A_ASSOCIATE_RJ.
        /// </summary>
        /// <returns>The sent A_ASSOCIATE_RJ.</returns>
        /// <exception cref="System.Exception">
        ///	Sending of the A_ASSOCIATE_RJ fails.
        /// </exception> 
        protected internal AssociateRj SendAssociateRj()
        {
            AssociateRj associateRj = new AssociateRj(new DvtkData.Dul.A_ASSOCIATE_RJ());

			SendMessage(associateRj);

            return (associateRj);
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
		protected internal AssociateRj SendAssociateRj(Byte result, Byte source, Byte reason)
		{
			AssociateRj associateRj = new AssociateRj(new DvtkData.Dul.A_ASSOCIATE_RJ(result, source, reason));

			SendMessage(associateRj);

			return(associateRj);
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
		protected internal DulMessage SendAssociateRp()
		{
			DulMessage dulMessage = null;

			DicomProtocolMessage lastReceivedMessage = Messages.LastReceivedMessage;

			if (lastReceivedMessage is AssociateRq)
			{
				PresentationContextCollection presentationContextsForAssociateAc = 
					(lastReceivedMessage as AssociateRq).CreatePresentationContextsForAssociateAc();

				// If at least one presentation context is accepted, send an A-ASSOCIATE-AC.
				// Otherwise send an A-ASSOCIATE-RJ.
				if (presentationContextsForAssociateAc.AcceptedAbstractSyntaxes.Count > 0)
				{
					dulMessage = SendAssociateAc(presentationContextsForAssociateAc);
				}
				else
				{
					dulMessage = SendAssociateRj();
				}
			}
			else
			{
				throw new System.Exception("Calling SendAssociateRp() while last received messages is not an A-ASSOCIATE-RQ.");
			}

			return(dulMessage);
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
		protected internal DulMessage SendAssociateRp(PresentationContextCollection presentationContextCollection)
		{
			DulMessage dulMessage = null;

			// If at least one presentation context is accepted, send an A-ASSOCIATE-AC.
			// Otherwise send an A-ASSOCIATE-RJ.
			if (presentationContextCollection.AcceptedAbstractSyntaxes.Count > 0)
			{
				dulMessage = SendAssociateAc(presentationContextCollection);
			}
			else
			{
				dulMessage = SendAssociateRj();
			}

			return(dulMessage);
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
		protected internal DulMessage SendAssociateRp(params PresentationContext[] presentationContexts)
		{
			PresentationContextCollection presentationContextCollection	= new PresentationContextCollection();
	
			foreach(PresentationContext presentationContext in presentationContexts)
			{
				presentationContextCollection.Add(presentationContext);
			}

			return(SendAssociateRp(presentationContextCollection));
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
		protected internal DulMessage SendAssociateRp(SopClasses sopClasses)
		{
			DulMessage dulMessage = null;

			DicomProtocolMessage lastReceivedMessage = Messages.LastReceivedMessage;

			if (lastReceivedMessage is AssociateRq)
			{
				PresentationContextCollection presentationContextsForAssociateAc = 
					(lastReceivedMessage as AssociateRq).CreatePresentationContextsForAssociateAc(sopClasses);

				// If at least one presentation context is accepted, send an A-ASSOCIATE-AC.
				// Otherwise send an A-ASSOCIATE-RJ.
				if (presentationContextsForAssociateAc.AcceptedAbstractSyntaxes.Count > 0)
				{
					dulMessage = SendAssociateAc(presentationContextsForAssociateAc);
				}
				else
				{
					dulMessage = SendAssociateRj();
				}
			}
			else
			{
				throw new System.Exception("Calling SendAssociateRp() while last received messages is not an A-ASSOCIATE-RQ.");
			}

			return(dulMessage);
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
		protected internal DulMessage SendAssociateRp(SopClasses sopClasses, params TransferSyntaxes[] transferSyntaxesList)
		{
			DulMessage dulMessage = null;

			DicomProtocolMessage lastReceivedMessage = Messages.LastReceivedMessage;

			if (lastReceivedMessage is AssociateRq)
			{
				PresentationContextCollection presentationContextsForAssociateAc = 
					(lastReceivedMessage as AssociateRq).CreatePresentationContextsForAssociateAc(sopClasses, transferSyntaxesList);

				// If at least one presentation context is accepted, send an A-ASSOCIATE-AC.
				// Otherwise send an A-ASSOCIATE-RJ.
				if (presentationContextsForAssociateAc.AcceptedAbstractSyntaxes.Count > 0)
				{
					dulMessage = SendAssociateAc(presentationContextsForAssociateAc);
				}
				else
				{
					dulMessage = SendAssociateRj();
				}
			}
			else
			{
				throw new System.Exception("Calling SendAssociateRp() while last received messages is not an A-ASSOCIATE-RQ.");
			}

			return(dulMessage);
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
		protected internal DulMessage SendAssociateRp(params TransferSyntaxes[] transferSyntaxesList)
		{
			DulMessage dulMessage = null;

			DicomProtocolMessage lastReceivedMessage = Messages.LastReceivedMessage;

			if (lastReceivedMessage is AssociateRq)
			{
				PresentationContextCollection presentationContextsForAssociateAc = 
					(lastReceivedMessage as AssociateRq).CreatePresentationContextsForAssociateAc(transferSyntaxesList);

				// If at least one presentation context is accepted, send an A-ASSOCIATE-AC.
				// Otherwise send an A-ASSOCIATE-RJ.
				if (presentationContextsForAssociateAc.AcceptedAbstractSyntaxes.Count > 0)
				{
					dulMessage = SendAssociateAc(presentationContextsForAssociateAc);
				}
				else
				{
					dulMessage = SendAssociateRj();
				}
			}
			else
			{
				throw new System.Exception("Calling SendAssociateRp() while last received messages is not an A-ASSOCIATE-RQ.");
			}

			return(dulMessage);
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
        protected internal AssociateRq SendAssociateRq(params PresentationContext[] presentationContexts)
        {
            AssociateRq associateRq = null;

            // Create a DvtkData ASSOCIATE request and set default values.
            DvtkData.Dul.A_ASSOCIATE_RQ dvtkDataAssociateRq = new DvtkData.Dul.A_ASSOCIATE_RQ();
            dvtkDataAssociateRq.CallingAETitle = Options.LocalAeTitle;
            dvtkDataAssociateRq.CalledAETitle = Options.RemoteAeTitle;
            dvtkDataAssociateRq.UserInformation.MaximumLength.MaximumLengthReceived = Options.LocalMaximumLength;
            dvtkDataAssociateRq.UserInformation.ImplementationClassUid.UID = Options.LocalImplementationClassUid;
            dvtkDataAssociateRq.UserInformation.ImplementationVersionName.Name = Options.LocalImplementationVersionName;

            // Parse all parameters.
            foreach (PresentationContext presentationContext in presentationContexts)
            {
                if (!presentationContext.IsForAssociateRequest)
                {
                    throw new System.Exception("Error while interpreting parameters for PresentationContext");
                }

                DvtkData.Dul.RequestedPresentationContext requestedPresentationContext = new DvtkData.Dul.RequestedPresentationContext();

                requestedPresentationContext.AbstractSyntax = new DvtkData.Dul.AbstractSyntax(presentationContext.AbstractSyntax);

                foreach (String transferSyntax in presentationContext.TransferSyntaxes)
                {
                    requestedPresentationContext.AddTransferSyntaxes(new DvtkData.Dul.TransferSyntax(transferSyntax));
                }

                dvtkDataAssociateRq.AddPresentationContexts(requestedPresentationContext);
            }

			associateRq = new AssociateRq(dvtkDataAssociateRq);

			SendMessage(associateRq);

            hasOpenConnection = true;

            return (associateRq);
        }

        /// <summary>
        /// Sends an A-ASSOCIATE-RQ.The A-ASSOCIATE-RQ being passed must have the Presentation Contexts also.
        /// </summary>
        /// <param name="associateRequest">The A-Associate-Rq</param>
        /// <returns>The sent A-ASSOCIATE-RQ</returns>
        /// <exception cref="System.Exception">
        ///	The A-Associate-Rq message does not have any presentation contexts. 
        /// 
        /// </exception> 
        /// <example>
        ///		<b>VB.NET</b>
        ///		<code>
        /// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="IncludesDicomThreadScu"]' />
        /// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="DicomThreadScu"]' />
        /// 			<include file='Doc\VisualBasicExamples.xml' path='Examples/Example[@name="Main"]' />		
        ///		</code>
        /// </example>		
        protected internal AssociateRq SendAssociateRq(DvtkData.Dul.A_ASSOCIATE_RQ associateRequest)
        {
            AssociateRq associateRq= null;
              
            
            // Create the DvtkData ASSOCIATE request and set default values.
            associateRequest.CallingAETitle = Options.LocalAeTitle;
            associateRequest.CalledAETitle = Options.RemoteAeTitle;
            associateRequest.UserInformation.MaximumLength.MaximumLengthReceived = Options.LocalMaximumLength;
            associateRequest.UserInformation.ImplementationClassUid.UID = Options.LocalImplementationClassUid;
            associateRequest.UserInformation.ImplementationVersionName.Name = Options.LocalImplementationVersionName;

            if (associateRequest.PresentationContexts == null)
            {
                throw new System.Exception("The association Request Message does not have any Presentation Contexts to propose. ");
            }

            associateRq = new AssociateRq(associateRequest);

            SendMessage(associateRq);

            hasOpenConnection = true;

            return (associateRq);
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
        protected internal bool SendAssociation(DicomMessage dicomMessage, params PresentationContext[] presentationContexts)
        {
            DicomMessageCollection dicomMessages = new DicomMessageCollection();

            dicomMessages.Add(dicomMessage);

            return (SendAssociation(dicomMessages, presentationContexts));
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
        protected internal bool SendAssociation(DicomMessageCollection dicomMessages, params PresentationContext[] presentationContexts)
        {
            bool isAssociationAccepted = true;

            // Send the associate request.
            SendAssociateRq(presentationContexts);

            // Receive the associate repsonse (may be an accept or reject).
            DulMessage associateRp = ReceiveAssociateRp();
            try
            {
                // If an associate accept was received, send the collection of DicomMessages, receive a response and
                // release the association.
                if (associateRp is AssociateAc)
                {
                    // send each message
                    foreach (DicomMessage dicomMessage in dicomMessages)
                    {
                        Send(dicomMessage);

                        // handle all responses
                        bool pendingResponses = true;
                        while (pendingResponses == true)
                        {
                            DicomMessage dicomResponse = ReceiveDicomMessage();
                            Values values = dicomResponse.CommandSet["0x00000900"].Values;
                            System.String statusString = values[0];
                            uint status = uint.Parse(statusString);
                            if ((status != 0xFF00) && (status != 0xFF01))
                            {
                                pendingResponses = false;
                            }
                        }
                    }

                    SendReleaseRq();

                    ReceiveReleaseRp();
                }
                else
                {
                    isAssociationAccepted = false;
                }
            }
            catch (Exception e)
            {
                SendReleaseRq();
                ReceiveReleaseRp();
                throw e;
            }
            return (isAssociationAccepted);
        }

        
        // Contains common functionality when sending a message.


        private void SendMessage(DicomMessage dicomMessage, int presentationContextId)
        {
            //
            // Administration.
            //

            dicomMessage.IsSend = true;

            AddMessage(dicomMessage);

            if (SendingMessageEvent != null)
            {
                SendingMessageEvent(dicomMessage);
            }


            //
            // Perform the actual sending.
            //

            Dvtk.Sessions.SendReturnCode sendReturnCode = Dvtk.Sessions.SendReturnCode.Failure;

            if (presentationContextId == -1)
            {
                sendReturnCode = DvtkScriptSession.Send(dicomMessage.DvtkDataDicomMessage);
            }
            else
            {
                sendReturnCode = DvtkScriptSession.Send(dicomMessage.DvtkDataDicomMessage, presentationContextId);
            }


            //
            // If an error is encountered while sending, throw an exception.
            //

            if (sendReturnCode != Dvtk.Sessions.SendReturnCode.Success)
            {
                if ((presentationContextId != -1) && this.hasOpenConnection)
                {
                    PresentationContext presentationContextUsed = null;

                    foreach (PresentationContext presentationContext in this.lastAssociateAc.PresentationContexts)
                    {
                        if (presentationContext.ID == presentationContextId)
                        {
                            presentationContextUsed = presentationContext;
                            break;
                        }
                    }


                    //
                    // Check and log if the reason for the failure to send is because of an illegal presentation context ID used.
                    //

                    if (presentationContextUsed == null)
                    {
                        throw new DicomProtocolMessageSendException("Ilegal presentation context ID " + presentationContextId.ToString() + " used while sending Dicom Message.", sendReturnCode);
                    }
                    else if (presentationContextUsed.Result != 0)
                    {
                        throw new DicomProtocolMessageSendException("Not accepted presentation context used, with presentation context ID " + presentationContextId.ToString() + " and result " + presentationContextUsed.Result.ToString() + " while sending Dicom Message.", sendReturnCode);
                    }
                }

                throw new DicomProtocolMessageSendException("Error while trying to send a " + dicomMessage.ToString() + " (" + sendReturnCode.ToString() + ")", sendReturnCode);
            }
        }

        private void SendMessage(DulMessage dulMessage)
        {
            //
            // Administration.
            //

            dulMessage.IsSend = true;

            AddMessage(dulMessage);

            if (SendingMessageEvent != null)
            {
                SendingMessageEvent(dulMessage);
            }


            //
            // Perform the actual sending.
            //

            Dvtk.Sessions.SendReturnCode sendReturnCode = Dvtk.Sessions.SendReturnCode.Failure;

            sendReturnCode = DvtkScriptSession.Send(dulMessage.DvtkDataDulMessage);


            //
            // If an error is encountered while sending, throw an exception.
            //

            if (sendReturnCode != Dvtk.Sessions.SendReturnCode.Success)
            {
                throw new DicomProtocolMessageSendException("Error while trying to send a " + dulMessage.ToString() + " (" + sendReturnCode.ToString() + ")", sendReturnCode);
            }

            if (dulMessage is AssociateAc)
            {
                this.lastAssociateAc = dulMessage as AssociateAc;
            }

        }

        /// <summary>
        /// Sends a Dicom A_RELEASE_RP.
        /// </summary>
        /// <returns>The sent A_RELEASE_RP.</returns>
        /// <exception cref="System.Exception">
        ///	Sending of the A_RELEASE_RP fails.
        /// </exception> 		
        protected internal ReleaseRp SendReleaseRp()
        {
            ReleaseRp releaseRp = new ReleaseRp();

			SendMessage(releaseRp);

            hasOpenConnection = false;

            return (releaseRp);
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
        protected internal ReleaseRq SendReleaseRq()
        {
            ReleaseRq releaseRq = new ReleaseRq();

			SendMessage(releaseRq);

            if (AssociationReleasedEvent != null)
            {
                AssociationReleasedEvent(this);
            }

            return (releaseRq);
        }

        /// <summary>
        /// Shows the detailed results by starting the windows application associated with an xml file.
        /// </summary>
		internal override void ShowResults()
        {
            if (File.Exists(Options.DetailResultsFullFileName))
            {
                // Use the DVTK results viewer to display the results, so the hyperlinks will still work.
                // For this to work, .xml files need to be associated with the DVTK results viewer.
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Options.DetailResultsFullFileName;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
                process.Start();
            }
            else if (File.Exists(Options.TestLogFullFileName))
            {
                // Use the DVTK results viewer to display the results, so the hyperlinks will still work.
                // For this to work, .xml files need to be associated with the DVTK results viewer.
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Options.TestLogFullFileName;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
                process.Start();
            }
        }

        /// <summary>
        /// Starts the results gathering.
        /// </summary>
        public override void StartResultsGathering()
        {
            if (Options.StartAndStopResultsGatheringEnabled)
            {
                base.StartResultsGathering();

                this.dvtkScriptSession.StartResultsGathering(Options.ResultsFileNameOnly);
            }
        }

		/// <summary>
		/// Stops the current thread only:
		/// - Terminate any open TCP/IP connection
		/// - If that doesn't work, Abort the .netThread associated with this object.
		/// 
		/// This method will indirectly be called in a seperate .Net thread (not in the .Net thread of
		/// this object) when the Stop method of this instance is called. A seperate thread is used to
		/// make sure that other threads are not waiting on the Stop method while this code is executed.
		/// </summary>
		internal protected override void StopCurrentThread()
		{
			System.Threading.Thread.CurrentThread.Name = "StopCurrentThread thread for Thread \"" + Options.Identifier + "\"";

			bool terminateConnectionAndAbort = false;

			// Extra check to see if this Thread is still running.
			lock(ThreadManager.ThreadManagerLock)
			{
				if (ThreadState == ThreadState.Running)
				{
					terminateConnectionAndAbort = true;
				}
			}

			if (terminateConnectionAndAbort)
			{
				// If this thread is listening to a port, this will terminate it.
				// We need to do this because an .Net Abort will not always succeed when
				// this thread is listening to a port.
				this.dvtkScriptSession.TerminateConnection();

				// Give thread time to terminate the connection.
				System.Threading.Thread.Sleep(1000);

				lock(ThreadManager.ThreadManagerLock)
				{
					// If the TerminateConnection call did not make the thread stop or the thread
					// didn't end by itself, do this with a .Net Thread Abort.
					if (ThreadState == ThreadState.Running)
					{
						this.dotNetThread.Abort();
					}
				}

				hasOpenConnection = false;
			}
		}

        /// <summary>
        /// Stops the results gathering.
        /// </summary>
        public override void StopResultsGathering()
        {
            if (Options.StartAndStopResultsGatheringEnabled)
            {
                base.StopResultsGathering();

                this.dvtkScriptSession.EndResultsGathering();
            }
        }

        /// <summary>
        /// Used to trigger the AssociationReleasedEvent.
        /// </summary>
        /// <param name="dicomThread">The DicomThread which sends to received the A-RELEASE-RQ.</param>
        [System.Obsolete("Use more generic MessageReceivedEvent or SendingMessageEvent.")]
        protected void TriggerAssociationReleasedEvent(DicomThread dicomThread)
        {
            if (AssociationReleasedEvent != null)
            {
                AssociationReleasedEvent(dicomThread);
            }
        }

        /// <summary>
        /// Adds the error and warning counts of all childs Threads to the error and warning counts
        /// of this instance.
        /// </summary>
        private void UpdateErrorAndWarningCount()
        {
            //
            // Add all errors and warnings of the sub Threads to the totals of this object.
            //

            UInt32 nrOfGeneralErrorsToAdd = 0;
            UInt32 nrOfGeneralWarningsToAdd = 0;
            UInt32 nrOfUserErrorsToAdd = 0;
            UInt32 nrOfUserWarningsToAdd = 0;
            UInt32 nrOfValidationErrorsToAdd = 0;
            UInt32 nrOfValidationWarningsToAdd = 0;
            bool oneOrMoreChildThreadshasErrors = false;
            bool oneOrMoreChildThreadshasWarnings = false;

            lock (ThreadManager.ThreadManagerLock)
            {
                foreach (Thread childThread in this.childs)
                {
                    if (childThread is DicomThread)
                    {
                        DicomThread childDicomThread = childThread as DicomThread;

                        if (childDicomThread.DvtkScriptSession.NrOfErrors > 0)
                            oneOrMoreChildThreadshasErrors = true;
                        if (childDicomThread.dvtkScriptSession.NrOfWarnings > 0)
                            oneOrMoreChildThreadshasWarnings = true;
                        nrOfGeneralErrorsToAdd += childDicomThread.DvtkScriptSession.NrOfGeneralErrors;
                        nrOfGeneralWarningsToAdd += childDicomThread.DvtkScriptSession.NrOfGeneralWarnings;
                        nrOfUserErrorsToAdd += childDicomThread.DvtkScriptSession.NrOfUserErrors;
                        nrOfUserWarningsToAdd += childDicomThread.DvtkScriptSession.NrOfUserWarnings;
                        nrOfValidationErrorsToAdd += childDicomThread.DvtkScriptSession.NrOfValidationErrors;
                        nrOfValidationWarningsToAdd += childDicomThread.DvtkScriptSession.NrOfValidationWarnings;
                        this.DvtkScriptSession.ErrorMessageList.AddRange(childDicomThread.DvtkScriptSession.ErrorMessageList);
                    }
                }
            }
            if (oneOrMoreChildThreadshasErrors)
                WriteError("One or more child threads have an error");
            if (oneOrMoreChildThreadshasWarnings)
                WriteWarning("One or more child threads have a warning");
            this.DvtkScriptSession.NrOfGeneralErrors += nrOfGeneralErrorsToAdd;
            this.DvtkScriptSession.NrOfGeneralWarnings += nrOfGeneralWarningsToAdd;
            this.DvtkScriptSession.NrOfUserErrors += nrOfUserErrorsToAdd;
            this.DvtkScriptSession.NrOfUserWarnings += nrOfUserWarningsToAdd;
            this.DvtkScriptSession.NrOfValidationErrors += nrOfValidationErrorsToAdd;
            this.DvtkScriptSession.NrOfValidationWarnings += nrOfValidationWarningsToAdd;
        }

        /// <summary>
        /// Validates a message.
        /// </summary>
        /// <param name="dicomProtocolMessage">The message to validate.</param>
        public void Validate(DicomProtocolMessage dicomProtocolMessage)
        {
            if (dicomProtocolMessage is DulMessage)
            {
                Validate(dicomProtocolMessage as DulMessage);
            }
            else
            {
                Validate(dicomProtocolMessage as DicomMessage);
            }
        }

        /// <summary>
        /// Validates a Dicom Message.
        /// </summary>
        /// <remarks>
        /// If applicable for this Dicom Message, this method will automatically try
        /// to determine the correct definition file to use for validation. A warning
        /// will be logged if no definition file can be found.
        /// </remarks>
        /// <param name="dicomMessage">The Dicom Message to validate.</param>
        public void Validate(DicomMessage dicomMessage)
        {
            String originalDefinitionFileApplicationEntityName = Options.DefinitionFileApplicationEntityName;
            String originalDefinitionFileApplicationEntityVersion = Options.DefinitionFileApplicationEntityVersion;


            //
            // - Determine if a loaded definition file is present to validate this DicomMessage.
            // - Perform logging.
            //

            String definitionFullFileName = GetDefinitionFullFileName(dicomMessage);

            if (definitionFullFileName.Length > 0)
            {
                WriteInformation("Validate DICOM Message using definition file \"" + definitionFullFileName + "\" (auto determined using Dimse Command \"" + dicomMessage.CommandSet.DimseCommand.ToString() + "\" and SOP Class UID \"" + dicomMessage.CommandSet.GetSopClassUid() + "\")...");
            }
            else
            {
                if (dicomMessage.CommandSet.GetSopClassUid().Length == 0)
                {
                    WriteWarning("Skipping definition file validation: unable to determine correct definition file because SOP Class UID not set in DICOM Message.");
                }
                else
                {
                    if ((Options.DefinitionFileApplicationEntityName != "DICOM") || (Options.DefinitionFileApplicationEntityVersion != "3.0"))
                    {
                        //
                        // Try to see if a DICOM 3.0 definition file exists as a fallback mechanism.
                        // 

                        Options.DefinitionFileApplicationEntityName = "DICOM";
                        Options.DefinitionFileApplicationEntityVersion = "3.0";

                        definitionFullFileName = GetDefinitionFullFileName(dicomMessage);

                        if (definitionFullFileName.Length > 0)
                        {
                            String warningText = String.Empty;
                            
                            warningText+= "Unable to find correct definition file for\r\n";
                            warningText += "- Dimse Command \"" + dicomMessage.CommandSet.DimseCommand.ToString() + "\"\r\n";
                            warningText += "- SOP Class UID \"" + dicomMessage.CommandSet.GetSopClassUid() + "\"\r\n";
                            warningText += "- ApplicationEntityName \"" + originalDefinitionFileApplicationEntityName + "\"\r\n";
                            warningText += "- ApplicationEntityVersion \"" + originalDefinitionFileApplicationEntityVersion + "\"\r\n";
                            warningText += "Validate DICOM Message using DICOM 3.0 definition file \"" + definitionFullFileName + "\" instead...";

                            WriteWarning(warningText);
                        }
                    }

                    if (definitionFullFileName.Length == 0)
                    {
                        WriteWarning("Skipping definition file validation: unable to find correct definition file for Dimse Command \"" + dicomMessage.CommandSet.DimseCommand.ToString() + "\" and SOP Class UID \"" + dicomMessage.CommandSet.GetSopClassUid() + "\".");
                    }
                }
            }


            //
            // Determine validation flags.
            //

            Dvtk.Sessions.ValidationControlFlags validationFlags = Dvtk.Sessions.ValidationControlFlags.UseValueRepresentations;

            if (definitionFullFileName.Length > 0)
            {
                validationFlags |= Dvtk.Sessions.ValidationControlFlags.UseDefinitions;
            }


            //
            // Do the actual validation.
            //

            dicomMessage.DataSet.IodId = GetIodNameFromDefinition(dicomMessage);
            this.dvtkScriptSession.Validate(dicomMessage.DvtkDataDicomMessage, null, validationFlags);


            //
            // Restore.
            //

            Options.DefinitionFileApplicationEntityName = originalDefinitionFileApplicationEntityName;
            Options.DefinitionFileApplicationEntityVersion = originalDefinitionFileApplicationEntityVersion;
        }

        /// <summary>
        /// Validates a Dicom Message against a reference Dicom Message.
        /// </summary>
        /// <remarks>
        /// If applicable for this Dicom Message, this method will automatically try
        /// to determine the correct definition file to use for validation. A warning
        /// will be logged if no definition file can be found.
        /// </remarks>
        /// <param name="dicomMessageSrc">The Dicom Message to validate.</param>
        /// <param name="dicomMessageRef">The reference Message.</param>
        public void Validate(DicomMessage dicomMessageSrc, DicomMessage dicomMessageRef)
        {
            String originalDefinitionFileApplicationEntityName = Options.DefinitionFileApplicationEntityName;
            String originalDefinitionFileApplicationEntityVersion = Options.DefinitionFileApplicationEntityVersion;


            //
            // - Determine if a loaded definition file is present to validate this DicomMessage.
            // - Perform logging.
            //

            String definitionFullFileName = GetDefinitionFullFileName(dicomMessageSrc);

            if (definitionFullFileName.Length > 0)
            {
                WriteInformation("Validate DICOM Message against reference using definition file \"" + definitionFullFileName + "\" (auto determined using Dimse Command \"" + dicomMessageSrc.CommandSet.DimseCommand.ToString() + "\" and SOP Class UID \"" + dicomMessageSrc.CommandSet.GetSopClassUid() + "\")...");
            }
            else
            {
                if (dicomMessageSrc.CommandSet.GetSopClassUid().Length == 0)
                {
                    WriteInformation("Validate Dicom message against reference...");
                    WriteWarning("Skipping definition file validation: SOP Class UID not set in DICOM Message.");
                }
                else
                {
                    if ((Options.DefinitionFileApplicationEntityName != "DICOM") || (Options.DefinitionFileApplicationEntityVersion != "3.0"))
                    {
                        //
                        // Try to see if a DICOM 3.0 definition file exists as a fallback mechanism.
                        // 

                        Options.DefinitionFileApplicationEntityName = "DICOM";
                        Options.DefinitionFileApplicationEntityVersion = "3.0";

                        definitionFullFileName = GetDefinitionFullFileName(dicomMessageSrc);

                        if (definitionFullFileName.Length > 0)
                        {
                            String warningText = String.Empty;

                            warningText += "Unable to find correct definition file for\r\n";
                            warningText += "- Dimse Command \"" + dicomMessageSrc.CommandSet.DimseCommand.ToString() + "\"\r\n";
                            warningText += "- SOP Class UID \"" + dicomMessageSrc.CommandSet.GetSopClassUid() + "\"\r\n";
                            warningText += "- ApplicationEntityName \"" + originalDefinitionFileApplicationEntityName + "\"\r\n";
                            warningText += "- ApplicationEntityVersion \"" + originalDefinitionFileApplicationEntityVersion + "\"\r\n";
                            warningText += "Validate DICOM Message using DICOM 3.0 definition file \"" + definitionFullFileName + "\" instead.";

                            WriteInformation("Validate Dicom message against reference...");
                            WriteWarning(warningText);
                        }
                    }

                    if (definitionFullFileName.Length == 0)
                    {
                        WriteInformation("Validate Dicom message against reference...");
                        WriteWarning("Skipping definition file validation: unable to find correct definition file for Dimse Command \"" + dicomMessageSrc.CommandSet.DimseCommand.ToString() + "\" and SOP Class UID \"" + dicomMessageSrc.CommandSet.GetSopClassUid() + "\".");
                    }
                }
            }


            //
            // Determine validation flags.
            //

            Dvtk.Sessions.ValidationControlFlags validationFlags = Dvtk.Sessions.ValidationControlFlags.UseValueRepresentations | Dvtk.Sessions.ValidationControlFlags.UseReferences;

            if (definitionFullFileName.Length > 0)
            {
                validationFlags |= Dvtk.Sessions.ValidationControlFlags.UseDefinitions;
            }


            //
            // Do the actual validation.
            //

            String iodName = GetIodNameFromDefinition(dicomMessageSrc);

            dicomMessageSrc.DataSet.IodId = iodName;
            dicomMessageRef.DataSet.IodId = iodName;
            this.dvtkScriptSession.Validate(dicomMessageSrc.DvtkDataDicomMessage, dicomMessageRef.DvtkDataDicomMessage, validationFlags);


            //
            // Restore.
            //

            Options.DefinitionFileApplicationEntityName = originalDefinitionFileApplicationEntityName;
            Options.DefinitionFileApplicationEntityVersion = originalDefinitionFileApplicationEntityVersion;
        }

        /// <summary>
        /// Validates a Dicom Message against a reference Dicom Message.
        /// </summary>
        /// <param name="dicomMessageSrc">The Dicom Message to validate.</param>
        /// <param name="dicomMessageRef">The reference Dicom Message.</param>
        /// <param name="iodName">
        /// The IOD name used to select the definition file to use for validation.<br></br>
        /// When "" is supplied, no definition file is used for validation.
        /// </param>
        public void Validate(DicomMessage dicomMessageSrc, DicomMessage dicomMessageRef, String iodName)
        {
            //
            // Logging.
            //

            if (iodName.Length == 0)
            // Don't use definition file.
            {
                WriteInformation("Validate Dicom message against reference...");
            }
            else
            // Use definition file.
            {
                WriteInformation("Validate Dicom message against reference using dataset definition \"" + iodName + "\"...");
            }


            //
            // Determine validation flags.
            //

            Dvtk.Sessions.ValidationControlFlags validationFlags = Dvtk.Sessions.ValidationControlFlags.UseValueRepresentations | Dvtk.Sessions.ValidationControlFlags.UseReferences;

            if (iodName.Length > 0)
            {
                validationFlags |= Dvtk.Sessions.ValidationControlFlags.UseDefinitions;
            }


            //
            // Do the actual validation.
            //

            dicomMessageSrc.DataSet.IodId = iodName;
            dicomMessageRef.DataSet.IodId = iodName;
            this.dvtkScriptSession.Validate(dicomMessageSrc.DvtkDataDicomMessage, dicomMessageRef.DvtkDataDicomMessage, validationFlags);
        }

        /// <summary>
        /// Validates a Dicom Message.
        /// </summary>
        /// <param name="dicomMessage">The Dicom Message to validate.</param>
        /// <param name="iodName">
        /// The IOD name used to select the definition file to use for validation.<br></br>
        /// When "" is supplied, no definition file is used for validation.
        /// </param>
        public void Validate(DicomMessage dicomMessage, String iodName)
        {
            //
            // Logging.
            //

            if (iodName.Length == 0)
            // Don't use definition file.
            {
                WriteInformation("Validate Dicom message...");
            }
            else
            // Use definition file.
            {
                WriteInformation("Validate Dicom message using dataset definition \"" + iodName + "\"...");
            }


            //
            // Determine validation flags.
            //

            Dvtk.Sessions.ValidationControlFlags validationFlags = Dvtk.Sessions.ValidationControlFlags.UseValueRepresentations;

            if (iodName.Length > 0)
            {
                validationFlags |= Dvtk.Sessions.ValidationControlFlags.UseDefinitions;
            }


            //
            // Do the actual validation.
            //

            dicomMessage.DataSet.IodId = iodName;
            this.dvtkScriptSession.Validate(dicomMessage.DvtkDataDicomMessage, null, validationFlags);
        }

        /// <summary>
        /// Validates a Dul Message.
        /// </summary>
        /// <param name="dulMessage">The Dul Message to validate.</param>
        public void Validate(DulMessage dulMessage)
        {
            WriteInformation("Validate Dul message...");

            this.dvtkScriptSession.Validate(dulMessage.DvtkDataDulMessage, null, Dvtk.Sessions.ValidationControlFlags.UseValueRepresentations);
        }

        /// <summary>
        /// Waits until pending data is available in the network input buffer.
        /// </summary>
        /// <param name="timeOut">The maximum amout of time in milliseconds to wait for pending data.</param>
        /// <param name="waitedTime">Return amout of time in milliseconds waited for pending data.</param>
        /// <returns>Returns a boolean indicating if pending data is available.</returns>
        protected internal bool WaitForPendingDataInNetworkInputBuffer(int timeOut, ref int waitedTime)
        {
            waitedTime = 0;

            bool hasPendingDataInNetworkInputBuffer = this.dvtkScriptSession.HasPendingDataInNetworkInputBuffer;

            while ((!hasPendingDataInNetworkInputBuffer) && (waitedTime < timeOut))
            {
                int timeToSleep = Math.Min(250, timeOut - waitedTime);

                Sleep(timeToSleep);
                waitedTime += timeToSleep;

                hasPendingDataInNetworkInputBuffer = this.dvtkScriptSession.HasPendingDataInNetworkInputBuffer;
            }

            return (hasPendingDataInNetworkInputBuffer);
        }

        /// <summary>
        /// Writes an error text to the results and triggers an ErrorOutputEvent.
        /// </summary>
        /// <remarks>
        /// When this instance is e.g. attached to a HliForm, the supplied error text will be
        /// displayed in this Form (the triggered ErrorOutputEvent is used for this).
        /// </remarks>
        /// <param name="text">The error text.</param>
        public override void WriteError(String text)
        {
            if (ResultsGatheringStarted)
            {
                this.dvtkScriptSession.WriteHtmlInformation("<br>");
                this.dvtkScriptSession.WriteError(text);

                // The TriggerErrorOutputEvent method will implicitly be called as a results of 
                // calling the this.dvtkScriptSession.WriteError method.
            }
            else
            {
                TriggerErrorOutputEvent(text);
            }
        }

        /// <summary>
        /// Writes a HTML text to the results.
        /// </summary>
        /// <param name="html">The HTML text.</param>
        /// <param name="writeToSummary">Determines if the HTML text will be written to the summary results file.</param>
        /// <param name="writeToDetail">Determines if the HTML text will be written to the detail results file.</param>
        public void WriteHtml(String html, bool writeToSummary, bool writeToDetail, bool writeToTestlog)
        {
            if (ResultsGatheringStarted)
            {
                // Devide the HTML string in smaller portions:
                // it may contain a lot of converted '<' and '>' into '[' and ']'.
                // When it contains a lot of these, the stylesheet processing will use a lot
                // of memory (with a possible out of memory).

                int index = 0;
                int length = html.Length;
                String piece = null;

                while (index < length)
                {
                    int nextClosingBracket = html.IndexOf('>', index);

                    if (nextClosingBracket == -1)
                    {
                        piece = html.Substring(index);
                        index = length;
                    }
                    else
                    {
                        piece = html.Substring(index, nextClosingBracket - index + 1);
                        index = nextClosingBracket + 1;
                    }

                    this.dvtkScriptSession.WriteHtml(piece, writeToSummary, writeToDetail, writeToTestlog);
                }
            }
        }

        /// <summary>
        /// Writes a HTML text to the results.
        /// </summary>
        /// <param name="html">The HTML text.</param>
        public void WriteHtmlInformation(String html)
        {
            WriteHtml(html, false, true, false);
        }

        /// <summary>
        /// Writes an information text to the results and triggers an InformationOutputEvent.
        /// </summary>
        /// <remarks>
        /// When this instance is e.g. attached to a HliForm, the supplied information text will be
        /// displayed in this Form (the triggered InformationOutputEvent is used for this).
        /// </remarks>
        /// <param name="text">The information text.</param>
        public override void WriteInformation(String text)
        {
            if (ResultsGatheringStarted)
            {
                this.dvtkScriptSession.WriteHtmlInformation("<br>");
                this.dvtkScriptSession.WriteInformation(text);

                // The TriggerInformationOutputEvent method will implicitly be called as a results of 
                // calling the this.dvtkScriptSession.WriteInformation method.
            }
            else
            {
                TriggerInformationOutputEvent(text);
            }
        }

        /// <summary>
        /// Writes a warning text to the results and triggers a WarningOutputEvent.
        /// </summary>
        /// <remarks>
        /// When this instance is e.g. attached to a HliForm, the supplied warning text will be
        /// displayed in this Form (the triggered WarningOutputEvent is used for this).
        /// </remarks>
        /// <param name="text">The warning text.</param>
        public override void WriteWarning(String text)
        {
            if (ResultsGatheringStarted)
            {
                this.dvtkScriptSession.WriteHtmlInformation("<br>");
                this.dvtkScriptSession.WriteWarning(text);

                // The TriggerWarningOutputEvent method will implicitly be called as a results of 
                // calling the this.dvtkScriptSession.WriteWarning method.
            }
            else
            {
                TriggerWarningOutputEvent(text);
            }
        }
	}
}
