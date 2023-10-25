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
	/// This class implements a MessageIterator descendant that handles each association in
	/// a different sub DicomThread. Multiple associations may be handled concurrently: when
	/// a sub DicomThread accepts an association, this object almost immediately starts
	/// a new sub DicomThread that starts listening to the same port.
	/// 
	/// An instance of this class constructed from outside the HLI interface only contains an
	/// overview of all started sub DicomThreads in the corresponding results files.
	/// 
	/// A user of this class outside the HLI interface is only concerned with creating an object
	/// of this class, calling the correct Initialize method, set the correct options, add MessageHandlers
	/// and calling the Start method.
	/// </summary>
    [Obsolete("Use the ConcurrentMessageIterator class instead.")]
	public class ConcurrentSCP: SCP
	{
		//
		// - Fields -
		//

		/// <summary>
		/// Number used to determine the results files name of a sub DicomThread.
		/// </summary>
		internal int subDicomThreadNumber = 1;

		/// <summary>
		/// Used to lock the subDicomThreadNumber field.
		/// </summary>
		private Object lockObject = new Object();

		/// <summary>
		/// The DicomThread (that may be constructed from outside the HLI interface) that contains 
		/// the sub DicomThreads that handle the actual associations.
		/// 
		/// If this object is the overview Thread, this field is null.
		/// If this object is a sub DicomThread, this field contains the overview Thread.
		/// </summary>
		protected ConcurrentSCP overviewThread = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor.
		/// </summary>
		public ConcurrentSCP()
		{
			// Do nothing.
		}



		//
		// - Methods -
		//


		/// <summary>
		/// This override makes sure that this SCP object only nadles one association and
		/// exists after that.
		/// </summary>
		/// <param name="releaseRq">The received A-RELEASE-RQ.</param>
		public override void AfterHandlingReleaseRequest(ReleaseRq releaseRq)
		{
			base.AfterHandlingReleaseRequest(releaseRq);			
			this.receiveMessages = false;
		}

		/// <summary>
		/// This method is called after an A-ASSOCIATE-RQ has been received but before it
		/// (possibly) will be handled by the (zero or more) MessageHandler objects that
		/// are attached to this object.
		/// 
		/// This implementation makes sure that before the A-ASSOCIATE-RQ is handled, a new
		/// sub DicomThread is started that listens to the same port as the current object.
		/// This method will only be called by a sub DicomThread.
		/// </summary>
		/// <param name="associateRq">The received A-ASSOCIATE-RQ</param>
		public override void BeforeHandlingAssociateRequest(AssociateRq associateRq)
		{
			CreateAndStartChildDicomThread(this.overviewThread);
		}

		/// <summary>
		/// Create and start a sub DicomThread.
		/// </summary>
		/// <param name="overviewThread">The overview Thread.</param>
		internal void CreateAndStartChildDicomThread(ConcurrentSCP overviewThread)
		{
			// Create a new sub DicomThread.
			Type[] types = new Type[0];

			ConstructorInfo constructorInfo = overviewThread.GetType().GetConstructor(types);

			ConcurrentSCP subDicomThread = constructorInfo.Invoke(null) as ConcurrentSCP;

			// The new sub threads must become a child of the overview Thread.
			subDicomThread.Initialize(overviewThread);

			subDicomThread.inboundDicomMessageFilters.AddRange(overviewThread.inboundDicomMessageFilters);
			subDicomThread.outboundDicomMessageFilters.AddRange(overviewThread.outboundDicomMessageFilters);

			subDicomThread.overviewThread = overviewThread;

			// Copy all options from the overview Thread.
			subDicomThread.Options.DeepCopyFrom(overviewThread.Options);

			// Set the correct identifier for the new sub thread.
			lock(this.lockObject)
			{
				subDicomThread.Options.Identifier = overviewThread.Options.Identifier + "_SubDicomThread" + overviewThread.subDicomThreadNumber.ToString();
				overviewThread.subDicomThreadNumber++;
			}

			// Copy all handlers from the overview Thread.
			foreach(MessageHandler messageHandler in overviewThread.MessagesHandlers)
			{
				subDicomThread.AddToFront(messageHandler);
			}

			subDicomThread.AssociationReleasedEvent+= new AssociationReleasedEventHandler(HandleChildAssociationReleasedEvent);

			// Start the new sub DicomThread.
			subDicomThread.Start();
		}


// !!!! Duidelijk maken dat onderstaande event wordt geraised vanwege childs.




		private void HandleChildAssociationReleasedEvent(DicomThread dicomThread)
		{
			TriggerAssociationReleasedEvent(dicomThread);
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
