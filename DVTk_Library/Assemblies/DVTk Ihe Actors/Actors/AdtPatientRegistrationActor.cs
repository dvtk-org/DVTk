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
	/// Summary description for AdtPatientRegistrationActor.
	/// </summary>
	public class AdtPatientRegistrationActor : BaseActor, IAdtPatientRegistrationActor
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="id">Actor Id.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public AdtPatientRegistrationActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.AdtPatientRegistration, id), iheFramework) { }

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			// for sending Patient Registration [RAD-1]
			// for sending Patient Update [Rad-12]
			AddHl7Client(Hl7ClientTypeEnum.Hl7Client, ActorTypeEnum.OrderPlacer, commonConfig, peerToPeerConfigCollection);

			// for sending Patient Registration [RAD-1]
			// for sending Patient Update [Rad-12]
			AddHl7Client(Hl7ClientTypeEnum.Hl7Client, ActorTypeEnum.DssOrderFiller, commonConfig, peerToPeerConfigCollection);

			// for receiving Patient Query
			AddHl7Server(Hl7ServerTypeEnum.Hl7QueryServer, ActorTypeEnum.DssOrderFiller, commonConfig, peerToPeerConfigCollection);
		}

		#region SendPatientRegistration() overloads
		public bool SendPatientRegistration(AdtMessage adtMessage)
		{
			// Generate trigger
			Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_1);
			trigger.Trigger = adtMessage;

			// RAD-1 - trigger the OrderPlacer
            bool triggerResult = TriggerActorInstances(ActorTypeEnum.OrderPlacer, trigger, true);

			// RAD-1 - trigger the DssOrderFiller
            if (triggerResult == true)
            {
                triggerResult = TriggerActorInstances(ActorTypeEnum.DssOrderFiller, trigger, true);
            }

            return triggerResult;
		}
		#endregion

		#region SendPatientUpdate() overloads
		public bool SendPatientUpdate(AdtMessage adtMessage)
		{
			// Generate trigger
			Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_12);
			trigger.Trigger = adtMessage;

			// RAD-12 - trigger the OrderPlacer
            bool triggerResult = TriggerActorInstances(ActorTypeEnum.OrderPlacer, trigger, true);

			// RAD-12 - trigger the DssOrderFiller
            if (triggerResult == true)
            {
                triggerResult = TriggerActorInstances(ActorTypeEnum.DssOrderFiller, trigger, true);
            }

            return triggerResult;
        }
		#endregion

		#region AddPatientQueryResponse
		public void AddPatientQueryResponse(AdrMessage adrMessage)
		{
			// Generate response trigger
			Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_UNKNOWN);
			trigger.Trigger = adrMessage;

			// Add a response trigger to the DssOrderFiller server
			AddResponseTriggerToActor(ActorTypeEnum.DssOrderFiller, trigger);
		}
		#endregion

	}
}
