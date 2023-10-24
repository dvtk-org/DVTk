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

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Hl7;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for OrderPlacerActor.
	/// </summary>
	public class OrderPlacerActor : BaseActor, IOrderPlacerActor
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="id">Actor Id.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public OrderPlacerActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.OrderPlacer, id), iheFramework) { }

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			// for receiving Patient Registration [RAD-1]
			// for receiving Patient Update [RAD-12]
			AddHl7Server(Hl7ServerTypeEnum.Hl7Server, ActorTypeEnum.AdtPatientRegistration, commonConfig, peerToPeerConfigCollection);

			// for receiving Filler Order Management [RAD-3]
			// for receiving Appointment Notification [RAD-48]
			AddHl7Server(Hl7ServerTypeEnum.Hl7Server, ActorTypeEnum.DssOrderFiller, commonConfig, peerToPeerConfigCollection);

			// for sending Placer Order Management [RAD-2]
			AddHl7Client(Hl7ClientTypeEnum.Hl7Client, ActorTypeEnum.DssOrderFiller, commonConfig, peerToPeerConfigCollection);
		}

		/// <summary>
		/// Handle an HL7 Transation from the given Actor Name.
		/// </summary>
		/// <param name="actorName">Source Actor Name.</param>
		/// <param name="hl7Transaction">HL7 Transaction.</param>
        protected override void HandleTransactionFrom(ActorName actorName, Hl7Transaction hl7Transaction)
		{
			switch (actorName.Type)
			{
				case ActorTypeEnum.AdtPatientRegistration:
					// received Patient Registration [RAD-1] or
					// received Patient Update [RAD-12]
					break;
				case ActorTypeEnum.DssOrderFiller:
					// received Filler Order Management [RAD-3] or
					// received Appointment Notification [RAD-48]
					break;
				default:
					break;
			}
		}

		#region SendPlacerOrderManagement() overloads
		public bool SendPlacerOrderManagement(OrmMessage ormMessage)
		{
			// Generate trigger
			Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_2);
			trigger.Trigger = ormMessage;

			// RAD-2 - trigger the DssOrderFiller
			return TriggerActorInstances(ActorTypeEnum.DssOrderFiller, trigger, true);
		}
		#endregion
	}
}
