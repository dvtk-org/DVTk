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
using System.Threading;
using System.Collections;

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Actors;

namespace Dvtk.IheActors.Hl7
{
	/// <summary>
	/// Summary description for Hl7QueryServer.
	/// </summary>
	public class Hl7QueryServer : Hl7Server
	{
		private BaseTriggerCollection _responseList = new BaseTriggerCollection();

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - (containing actor).</param>
		/// <param name="actorName">Destination Actor Name.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">HL7 Configuration.</param>
		public Hl7QueryServer(BaseActor parentActor, ActorName actorName, CommonConfig commonConfig, Hl7PeerToPeerConfig config) : base(parentActor, actorName, commonConfig, config) {}
 
		/// <summary>
		/// Add response trigger to server.
		/// </summary>
		/// <param name="actorName">Destination Actor Name.</param>
		/// <param name="trigger">Trigger message.</param>
		public void AddResponseTrigger(ActorName actorName, BaseTrigger trigger)
		{
			Hl7Trigger hl7Trigger = (Hl7Trigger) trigger;
			_responseList.Add(hl7Trigger);
		}

		/// <summary>
		/// Query message handler for the received Hl7 message - send a query response.
		/// </summary>
		/// <param name="hl7RequestMessage">Received HL7 message.</param>
		/// <returns>Hl7Message response.</returns>
		public override Hl7Message MessageHandler(Hl7Message hl7RequestMessage)
		{
			AdrMessage hl7ResponseMessage = new AdrMessage("A19");

			if (hl7RequestMessage is QryMessage)
			{
				QryMessage qryMessage = (QryMessage)hl7RequestMessage;

				// Try to get the ADR message from those stored
				hl7ResponseMessage = GetMatchingResponse(qryMessage.QRD[8]);
				if (hl7ResponseMessage == null)
				{
					// return empty message
					hl7ResponseMessage = new AdrMessage("A19");
				}

				// copy the QRD segment
				for (int i = 0; i < qryMessage.QRD.Count; i++)
				{
					hl7ResponseMessage.QRD[i] = qryMessage.QRD[i];
				}
			}

			// fill in the MSA segment
			hl7ResponseMessage.MSA[1] = "AA";
			hl7ResponseMessage.MSA[2] = hl7RequestMessage.MessageControlId;
	
			return hl7ResponseMessage;
		}

		private AdrMessage GetMatchingResponse(System.String patientId)
		{
			AdrMessage hl7ResponseMessage = null;

			foreach (BaseTrigger baseTrigger in _responseList)
			{
				if (baseTrigger is Hl7Trigger)
				{
					Hl7Trigger hl7Trigger = (Hl7Trigger)baseTrigger;
					AdrMessage lHl7ResponseMessage = (AdrMessage)hl7Trigger.Trigger;

					if (lHl7ResponseMessage.PID[3] == patientId)
					{
						hl7ResponseMessage = lHl7ResponseMessage;
						break;
					}
				}
			}

			return hl7ResponseMessage;
		}
	}
}
