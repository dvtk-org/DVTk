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

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;

namespace Dvtk.DvtkDicomEmulators.Bases
{
    /// <summary>
    /// This class implements basic functionality for a SCU that is executed in a seperate thread.
    /// It is controlled by calling the available Trigger... methods.
    /// </summary>
    public class HliScu : DicomThreadTriggerLoop
    {
        private bool _processTriggerResult = false;

        //
        // - Internal classes -
        //		
        protected class SendAssociationTrigger
        {
            public DicomMessageCollection dicomMessageCollection = null;
            public PresentationContext[] presentationContexts = null;
            public bool returnValue = true;

            public SendAssociationTrigger(DicomMessageCollection dicomMessageCollection, PresentationContext[] presentationContexts)
            {
                this.dicomMessageCollection = dicomMessageCollection;
                this.presentationContexts = presentationContexts;
                this.returnValue = true;
            }
        }

        /// <summary>
        /// Property - Process Trigger Result
        /// </summary>
        public bool ProcessTriggerResult
        {
            get
            {
                return _processTriggerResult;
            }
        }

        //
        // - Methods -
        //		
        protected override void ProcessTrigger(Object trigger)
        {
            if (trigger is SendAssociationTrigger)
            {
                SendAssociationTrigger sendAssociationTrigger = trigger as SendAssociationTrigger;

                try
                {
                    sendAssociationTrigger.returnValue = SendAssociation(sendAssociationTrigger.dicomMessageCollection, sendAssociationTrigger.presentationContexts);
                }
                catch (DicomProtocolMessageReceiveException)
                {
                    // Console.WriteLine("DicomProtocolMessageReceiveException: {0} - {1}", exception.Message, exception.ReceiveReturnCode); 
                    sendAssociationTrigger.returnValue = false;
                }
                catch (DicomProtocolMessageSendException)
                {
                    // Console.WriteLine("DicomProtocolMessageSendException: {0} - {1}", exception.Message, exception.SendReturnCode);
                    sendAssociationTrigger.returnValue = false;
                }
                catch (Exception)
                {
                    // Console.WriteLine("Exception: {0}", exception.Message);
                    sendAssociationTrigger.returnValue = false;
                }
            }
        }

        /// <summary>
        /// Trigger a SendAssociation. 
        /// </summary>
        /// <param name="dicomMessage">The DicomMessage to send.</param>
        /// <param name="presentationContexts">The presentation contexts used to set up an association.</param>
		public void TriggerSendAssociation(DicomMessage dicomMessage, params PresentationContext[] presentationContexts)
        {
            DicomMessageCollection dicomMessageCollection = new DicomMessageCollection();
            dicomMessageCollection.Add(dicomMessage);

            SendAssociationTrigger sendAssociationTrigger = new SendAssociationTrigger(dicomMessageCollection, presentationContexts);

            Trigger(sendAssociationTrigger);
        }

        /// <summary>
        /// Trigger a SendAssociation.
        /// </summary>
        /// <param name="dicomMessageCollection">The DicomMessages to send.</param>
        /// <param name="presentationContexts">The presentation contexts used to set up an association.</param>
        public void TriggerSendAssociation(DicomMessageCollection dicomMessageCollection, params PresentationContext[] presentationContexts)
        {
            SendAssociationTrigger sendAssociationTrigger = new SendAssociationTrigger(dicomMessageCollection, presentationContexts);

            Trigger(sendAssociationTrigger);
        }

        /// <summary>
        /// Trigger a SendAssociation.
        /// </summary>
        /// <param name="dicomMessage">The DicomMessage to send.</param>
        /// <param name="presentationContexts">The presentation contexts used to set up an association.</param>
        /// <returns>Boolean - indicating success or not.</returns>
		public bool TriggerSendAssociationAndWait(DicomMessage dicomMessage, params PresentationContext[] presentationContexts)
        {
            DicomMessageCollection dicomMessageCollection = new DicomMessageCollection();
            dicomMessageCollection.Add(dicomMessage);

            SendAssociationTrigger sendAssociationTrigger = new SendAssociationTrigger(dicomMessageCollection, presentationContexts);

            Trigger(sendAssociationTrigger);

            WaitForLastTriggerCallProcessed();

            return (sendAssociationTrigger.returnValue);
        }

        /// <summary>
        /// Only use this method when only one thread is calling the methods for this object.
        /// </summary>
        /// <param name="dicomMessageCollection"></param>
        /// <param name="presentationContexts"></param>
        /// <returns>Boolean - indicating success or not.</returns>
        public bool TriggerSendAssociationAndWait(DicomMessageCollection dicomMessageCollection, params PresentationContext[] presentationContexts)
        {
            SendAssociationTrigger sendAssociationTrigger = new SendAssociationTrigger(dicomMessageCollection, presentationContexts);
            Trigger(sendAssociationTrigger);

            WaitForLastTriggerCallProcessed();

            return (sendAssociationTrigger.returnValue);
        }

        /// <summary>
        /// May be overriden to implement things that need to be performed before
        /// processing a trigger.
        /// </summary>
        /// <param name="trigger">The trigger that will be processed.</param>
        public override void BeforeProcessTrigger(Object trigger)
        {
            // initialise the trigger result
            _processTriggerResult = false;

            // call the base implementation
            base.BeforeProcessTrigger(trigger);
        }

        /// <summary>
        /// May be overriden to implement things that need to be performed after
        /// processing a trigger.
        /// </summary>
        /// <param name="trigger">The trigger that will be processed.</param>
        public override void AfterProcessTrigger(Object trigger)
        {
            // set the process trigger result
            if (trigger is SendAssociationTrigger)
            {
                SendAssociationTrigger sendAssociationTrigger = trigger as SendAssociationTrigger;
                _processTriggerResult = sendAssociationTrigger.returnValue;
            }

            // call the base implementation
            base.AfterProcessTrigger(trigger);
        }

        /// <summary>
        /// Called after the HandleException method has been called.
        /// </summary>
        /// <remarks>
        /// Gives the descendant of this class the possibility to perform
        /// extra actions compared to this base class.
        /// </remarks>
        /// <param name="exception">Exception that has been handled.</param>
        protected override void AfterHandlingException(System.Exception exception)
        {
            // trigger processing failed due to the exception
            _processTriggerResult = false;

            // call the base implementation
            base.AfterHandlingException(exception);
        }
    }
}
