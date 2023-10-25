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



namespace DvtkHighLevelInterface.Dicom.Threads
{
	/// <summary>
	/// This class implements basic functionality for a SCU that is executed in a seperate thread.
	/// It is controlled by calling the available Trigger... methods.
	/// </summary>
	public class SCU: DicomThreadTriggerLoop
	{
		//
		// - Inernal classes -
		//		
		
		private class SendAssociationTrigger
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



		//
		// - Methods -
		//		

        /// <summary>
        /// Processed a trigger.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
		protected override void ProcessTrigger(Object trigger)
		{
			if (trigger is SendAssociationTrigger)
			{
				SendAssociationTrigger sendAssociationTrigger = trigger as SendAssociationTrigger;

				try
				{
					sendAssociationTrigger.returnValue = SendAssociation(sendAssociationTrigger.dicomMessageCollection, sendAssociationTrigger.presentationContexts);
				}
				catch(Exception exception)
				{
					sendAssociationTrigger.returnValue = false;
					throw(exception);
				}
			}
			else
			{
				// Do nothing.
			}
		}

        /// <summary>
        /// Triggers sending of an association.
        /// </summary>
        /// <param name="dicomMessage">The DICOM message to send.</param>
        /// <param name="presentationContexts">The presentation contexts to use for setting up the DICOM association.</param>
		public void TriggerSendAssociation(DicomMessage dicomMessage, params PresentationContext[] presentationContexts)
		{
			DicomMessageCollection dicomMessageCollection = new DicomMessageCollection();
			dicomMessageCollection.Add(dicomMessage);

			SendAssociationTrigger sendAssociationTrigger = new SendAssociationTrigger(dicomMessageCollection, presentationContexts);

			Trigger(sendAssociationTrigger);
		}

		/// <summary>
		/// Trigger a SendAssociation.
		/// 
		/// ...
		/// </summary>
		/// <param name="dicomMessageCollection">The DicomMessages to send.</param>
		/// <param name="presentationContexts">The presentation contexts used to set up an association.</param>
		public void TriggerSendAssociation(DicomMessageCollection dicomMessageCollection, params PresentationContext[] presentationContexts)
		{
			SendAssociationTrigger sendAssociationTrigger = new SendAssociationTrigger(dicomMessageCollection, presentationContexts);

			Trigger(sendAssociationTrigger);
		}

        /// <summary>
        /// Trigger a send association and wait until it has been completed.
        /// </summary>
        /// <param name="dicomMessage">The DICOM message to send.</param>
        /// <param name="presentationContexts">The presentation contexts to propose.</param>
        /// <returns>
        /// True indicates the other side has accepted the association, false indicates the other side
        /// has rejected the association.
        /// </returns>
		public bool TriggerSendAssociationAndWait(DicomMessage dicomMessage, params PresentationContext[] presentationContexts)
		{
			DicomMessageCollection dicomMessageCollection = new DicomMessageCollection();
			dicomMessageCollection.Add(dicomMessage);

			SendAssociationTrigger sendAssociationTrigger = new SendAssociationTrigger(dicomMessageCollection, presentationContexts);

			Trigger(sendAssociationTrigger);

			WaitForLastTriggerCallProcessed();

			return(sendAssociationTrigger.returnValue);
		}


		/// <summary>
        /// Trigger a send association and wait until it has been completed.
		/// </summary>
        /// <param name="dicomMessageCollection">The DICOM messages to send.</param>
        /// <param name="presentationContexts">The presentation contexts to propose.</param>
        /// <returns>
        /// True indicates the other side has accepted the association, false indicates the other side
        /// has rejected the association.
        /// </returns>
		public bool TriggerSendAssociationAndWait(DicomMessageCollection dicomMessageCollection, params PresentationContext[] presentationContexts)
		{
			SendAssociationTrigger sendAssociationTrigger = new SendAssociationTrigger(dicomMessageCollection, presentationContexts);
			Trigger(sendAssociationTrigger);

			WaitForLastTriggerCallProcessed();

			return(sendAssociationTrigger.returnValue);
		}
	}
}
