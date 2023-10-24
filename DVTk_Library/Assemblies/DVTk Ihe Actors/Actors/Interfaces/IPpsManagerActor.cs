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

using DvtkHighLevelInterface.Dicom.Messages;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Dicom;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for IPpsManagerActor.
	/// </summary>
	interface IPpsManagerActor
	{
		#region SendModalityProcedureStepInProgress() overloads
        bool SendModalityProcedureStepInProgress();

		#endregion SendModalityProcedureStepInProgress() overloads

		#region SendModalityProcedureStepCompleted() overloads
        bool SendModalityProcedureStepCompleted();

		#endregion SendModalityProcedureStepCompleted() overloads

		#region SendModalityProcedureStepDiscontinued() overloads
        bool SendModalityProcedureStepDiscontinued();

		#endregion SendModalityProcedureStepDiscontinued() overloads

		#region SendCreatorProcedureStepInProgress() overloads
        bool SendCreatorProcedureStepInProgress();

		#endregion SendCreatorProcedureStepInProgress() overloads

		#region SendCreatorProcedureStepCompleted() overloads
        bool SendCreatorProcedureStepCompleted();

		#endregion SendCreatorProcedureStepCompleted() overloads

		#region SendCreatorProcedureStepDiscontinued() overloads
        bool SendCreatorProcedureStepDiscontinued();

		#endregion SendCreatorProcedureStepDiscontinued() overloads
	}
}
