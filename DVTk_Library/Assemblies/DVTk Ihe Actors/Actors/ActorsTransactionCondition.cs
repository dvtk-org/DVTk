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

using Dvtk.IheActors.Bases;
using DvtkHighLevelInterface.Common.Other;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for ActorsTransactionCondition.
	/// </summary>
	abstract public class ActorsTransactionCondition: Condition
	{
		public ActorsTransactionCondition()
		{
			// Do nothing.
		}

		public override bool Evaluate(Object theObject)
		{
			bool isConditionTrue = true;

			ActorsTransaction actorsTransaction = theObject as ActorsTransaction;

			if (actorsTransaction == null)
			{
				isConditionTrue = true;
			}
			else
			{
				isConditionTrue = Evaluate(actorsTransaction);
			}

			return(isConditionTrue);

		}

		public abstract bool Evaluate(ActorsTransaction actorsTransaction);
	}

	public class ActorsTransactionConditionDirection: ActorsTransactionCondition
	{
		private TransactionDirectionEnum transactionDirection;

		public ActorsTransactionConditionDirection(TransactionDirectionEnum transactionDirection)
		{
			this.transactionDirection = transactionDirection;
		}

		public override bool Evaluate(ActorsTransaction actorsTransaction)
		{
			bool isConditionTrue = true;

			if (actorsTransaction.Direction == this.transactionDirection)
			{
				isConditionTrue = true;
			}
			else
			{
				isConditionTrue = false;
			}

			return(isConditionTrue);
		}
	}

	public class ActorsTransactionConditionFromActor: ActorsTransactionCondition
	{
		private ActorName actorName;

		public ActorsTransactionConditionFromActor(ActorName actorName)
		{
			this.actorName = actorName;
		}

		public override bool Evaluate(ActorsTransaction actorsTransaction)
		{
			bool isConditionTrue = true;

			if (actorsTransaction.FromActorName == this.actorName)
			{
				isConditionTrue = true;
			}
			else
			{
				isConditionTrue = false;
			}

			return isConditionTrue;
		}
	}

	public class ActorsTransactionConditionToActor: ActorsTransactionCondition
	{
		private ActorName actorName;

		public ActorsTransactionConditionToActor(ActorName actorName)
		{
			this.actorName = actorName;
		}

		public override bool Evaluate(ActorsTransaction actorsTransaction)
		{
			bool isConditionTrue = true;

			if (actorsTransaction.ToActorName == this.actorName)
			{
				isConditionTrue = true;
			}
			else
			{
				isConditionTrue = false;
			}

			return isConditionTrue;
		}
	}
}
