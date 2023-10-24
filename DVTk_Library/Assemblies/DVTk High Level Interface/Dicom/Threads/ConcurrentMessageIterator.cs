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
using System.Reflection;

using DvtkHighLevelInterface.Dicom.Messages;



namespace DvtkHighLevelInterface.Dicom.Threads
{
    /// <summary>
    /// This class implements a MessageIterator descendant that handles each single association in
    /// a different child DicomThread. Multiple associations may be handled concurrently: when
    /// a child DicomThread accepts an association, this instance almost immediately starts
    /// a new child DicomThread that starts listening to the same port.
    /// <br></br><br></br>
    /// An instance of this class constructed from outside the HLI interface only contains an
    /// overview of all started child DicomThreads in the corresponding results files.
    /// <br></br><br></br>
    /// A user of this class (outside the HLI interface) is only concerned with creating an instance
    /// of this class, calling the correct Initialize method, set the correct options, add MessageHandlers
    /// and/or override MessageHandler methods and calling the Start method.
    /// </summary>
    /// <remarks>
    /// When the BeforeHandlingAssociateRequest method of this class needs to be overriden, make
    /// sure that in the derived class the method of this class is called first.
    /// <br></br><br></br>
    /// Make sure that a descendant class of this class always implements the constructor with 
    /// single parameter (String identifierBasisChildThreads), that needs to call the constructor
    /// with the same parameter of his class.
    /// <br></br><br></br>
    /// When creating a descendant, say DescClass, of this class, note the following.
    /// Because an instance of DescClass implicitly created new instances of DescClass, make sure 
    /// any fields declared in DescClass are declared static.
    /// </remarks>
    public class ConcurrentMessageIterator : MessageIterator
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Number used to determine the results files name of a child DicomThread.
        /// </summary>
        internal int childDicomThreadNumber = 1;

        /// <summary>
        /// To actual identifier used for a child DicomThread will be the
        /// identifierBasisChildThreads appended with a number (starting from 1).
        /// </summary>
        private String identifierBasisChildThreads = String.Empty;

        /// <summary>
        /// Used to lock the childDicomThreadNumber field.
        /// </summary>
        private Object lockObject = new Object();

        /// <summary>
        /// The DicomThread (that may be constructed from outside the HLI interface) that contains 
        /// the child DicomThreads that handle the actual associations.
        /// 
        /// If this object is the overview Thread, this field is null.
        /// If this object is a child DicomThread, this field contains the overview Thread.
        /// </summary>
        protected ConcurrentMessageIterator overviewThread = null;

        /// <summary>
        /// If unequal to null,
        /// the actual resultsFileNameOnlyWithoutExtension used for a child DicomThread will be the
        /// resultsFileNameOnlyWithoutExtensionBasisChildThreads appended with a number (starting from 1).
        /// </summary>
        private String resultsFileNameOnlyWithoutExtensionBasisChildThreads = null;



        //
        // - Constructors -
        //      

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ConcurrentMessageIterator()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identifierBasisChildThreads">
        /// To actual identifier used for a child DicomThread will be the
        /// identifierBasisChildThreads appended with a number (starting from 1).
        /// </param>
        public ConcurrentMessageIterator(String identifierBasisChildThreads)
        {
            this.identifierBasisChildThreads = identifierBasisChildThreads;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identifierBasisChildThreads">
        /// To actual identifier used for a child DicomThread will be the
        /// identifierBasisChildThreads appended with a number (starting from 1).
        /// </param>
        /// <param name="resultsFileNameOnlyWithoutExtensionBasisChildThreads">
        /// The actual resultsFileNameOnlyWithoutExtension used for a child DicomThread will be the
        /// resultsFileNameOnlyWithoutExtensionBasisChildThreads appended with a number (starting from 1).
        /// </param>
        public ConcurrentMessageIterator(String identifierBasisChildThreads, String resultsFileNameOnlyWithoutExtensionBasisChildThreads)
        {
            this.identifierBasisChildThreads = identifierBasisChildThreads;
            this.resultsFileNameOnlyWithoutExtensionBasisChildThreads = resultsFileNameOnlyWithoutExtensionBasisChildThreads;
        }



        //
        // - Methods -
        //

        /// <summary>
        /// This method is called after an A-ASSOCIATE-RQ has been received but before it
        /// (possibly) will be handled by the (zero or more) MessageHandler instances that
        /// are attached to this object.
        /// 
        /// This implementation makes sure that before the A-ASSOCIATE-RQ is handled, a new
        /// child DicomThread is started that listens to the same port as the current instance.
        /// This method will only be called by a child DicomThread.
        /// </summary>
        /// <param name="associateRq">The received A-ASSOCIATE-RQ</param>
        public override void BeforeHandlingAssociateRequest(AssociateRq associateRq)
        {
            CreateAndStartChildDicomThread(this.overviewThread);
        }

        /// <summary>
        /// Create and start a child DicomThread.
        /// </summary>
        /// <param name="overviewThread">The overview Thread.</param>
        private void CreateAndStartChildDicomThread(ConcurrentMessageIterator overviewThread)
        {
            ConcurrentMessageIterator childDicomThread = null;

            if (overviewThread.resultsFileNameOnlyWithoutExtensionBasisChildThreads == null)
            {
                // Create a new child DicomThread.
                Type[] types = new Type[1];
                types[0] = typeof(String);

                ConstructorInfo constructorInfo = overviewThread.GetType().GetConstructor(types);

                Object[] constructorArguments = new Object[1];
                constructorArguments[0] = overviewThread.identifierBasisChildThreads;

                childDicomThread = constructorInfo.Invoke(constructorArguments) as ConcurrentMessageIterator;
            }
            else
            {
                // Create a new child DicomThread.
                Type[] types = new Type[2];
                types[0] = typeof(String);
                types[1] = typeof(String);

                ConstructorInfo constructorInfo = overviewThread.GetType().GetConstructor(types);

                Object[] constructorArguments = new Object[2];
                constructorArguments[0] = overviewThread.identifierBasisChildThreads;
                constructorArguments[1] = overviewThread.resultsFileNameOnlyWithoutExtensionBasisChildThreads;

                childDicomThread = constructorInfo.Invoke(constructorArguments) as ConcurrentMessageIterator;
            }

            // The new sub threads must become a child of the overview Thread.
            childDicomThread.Initialize(overviewThread);

            childDicomThread.inboundDicomMessageFilters.AddRange(overviewThread.inboundDicomMessageFilters);
            childDicomThread.outboundDicomMessageFilters.AddRange(overviewThread.outboundDicomMessageFilters);

            childDicomThread.overviewThread = overviewThread;

            // Copy all options from the overview Thread.
            childDicomThread.Options.DeepCopyFrom(overviewThread.Options);

            // Set the correct identifier for the new sub thread.
            lock (this.lockObject)
            {
                childDicomThread.Options.Identifier = this.identifierBasisChildThreads + overviewThread.childDicomThreadNumber.ToString();

                if (overviewThread.resultsFileNameOnlyWithoutExtensionBasisChildThreads != null)
                {
                    childDicomThread.Options.ResultsFileNameOnlyWithoutExtension = overviewThread.resultsFileNameOnlyWithoutExtensionBasisChildThreads + overviewThread.childDicomThreadNumber.ToString();
                }

                overviewThread.childDicomThreadNumber++;
            }

            // Copy all handlers from the overview Thread.
            foreach (MessageHandler messageHandler in overviewThread.MessagesHandlers)
            {
                childDicomThread.AddToFront(messageHandler);
            }

            // Make sure that only one association is handled by a child DicomThread.
            childDicomThread.MessageReceivedEvent += new MessageReceivedEventHandler(childDicomThread.HandleMessageReceivedEvent);
            childDicomThread.SendingMessageEvent += new SendingMessageEventHandler(childDicomThread.HandleSendingMessageEvent);

            // Start the new sub DicomThread.
            childDicomThread.Start();
        }

        /// <summary>
        /// Make sure that only one association is handled by a child DicomThread.
        /// </summary>
        /// <param name="dicomProtocolMessage">The message received.</param>
        private void HandleMessageReceivedEvent(DicomProtocolMessage dicomProtocolMessage)
        {
            if ((dicomProtocolMessage is AssociateRj) || (dicomProtocolMessage is Abort) || (dicomProtocolMessage is ReleaseRp))
            {
                // Stop receiving messages.
                this.receiveMessages = false;
            }
        }

        /// <summary>
        /// Make sure that only one association is handled by a child DicomThread.
        /// </summary>
        /// <param name="dicomProtocolMessage">The message received.</param>
        private void HandleSendingMessageEvent(DicomProtocolMessage dicomProtocolMessage)
        {
            if ((dicomProtocolMessage is AssociateRj) || (dicomProtocolMessage is Abort) || (dicomProtocolMessage is ReleaseRp))
            {
                // Stop receiving messages.
                this.receiveMessages = false;
            }
        }

        /// <summary>
        /// The following implementation makes sure that a new sub DicomThread is created and started by
        /// the overview Thread. After doing this, this objects waits until all subthreads are stopped.
        /// 
        /// When this object is a sub DicomThread, nothing is done.
        /// </summary>
        protected override void InitialAction()
        {
            // If this is the overview DicomThread, create and start a new sub DicomThread.
            if (this.overviewThread == null)
            {
                this.receiveMessages = false;

                CreateAndStartChildDicomThread(this);
            }
        }
    }
}
