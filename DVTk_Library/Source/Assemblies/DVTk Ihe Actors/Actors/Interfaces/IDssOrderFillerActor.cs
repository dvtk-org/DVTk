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
using DvtkHighLevelInterface.Dicom.Messages;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Hl7;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for IDssOrderFillerActor.
	/// </summary>
	interface IDssOrderFillerActor
	{
		#region SendFillerOrderManagement() overloads
        bool SendFillerOrderManagement(OrmMessage ormMessage);

		#endregion

		#region SendProcedureScheduled() overloads
        bool SendProcedureScheduled(OrmMessage ormMessage);

		#endregion

		#region SendImageAvailabilityQuery() overloads
        bool SendImageAvailabilityQuery();

		#endregion

		#region SendProcedureUpdated() overloads
        bool SendProcedureUpdated(OrmMessage ormMessage);

		#endregion

		#region SendPerformedWorkStatusUpdate() overloads
        bool SendPerformedWorkStatusUpdate();

		#endregion

		#region SendAppointmentNotification() overloads
        bool SendAppointmentNotification();

		#endregion
	}
}
