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

using Dvtk.CommonDataFormat;
using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using DvtkHighLevelInterface.Common.Threads;
using Dvtk.IheActors.Actors;
using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Hl7;
using Dvtk.Dicom.InformationEntity.DefaultValues;
using Dvtk.Comparator;

namespace Dvtk.IheActors.Bases
{
	#region actor types

	/// <summary>
	/// Actor Type Enum.
	/// </summary>
	public enum ActorTypeEnum
	{
		AdtPatientRegistration,
		OrderPlacer,
		DssOrderFiller,
		AcquisitionModality,
		ImageManager,
		ImageArchive,
		PerformedProcedureStepManager,
		ImageDisplay,
		EvidenceCreator,
		ReportManager,
		PrintComposer,
		PrintServer,
		Unknown
	}

	/// <summary>
	/// Summary description for ActorTypes.
	/// </summary>
	public class ActorTypes
	{
		/// <summary>
		/// Actor string based type from enum.
		/// </summary>
		/// <param name="actorType">Actor Type as Enum.</param>
		/// <returns>Actor Type as String.</returns>
		public static System.String Type(ActorTypeEnum actorType)
		{
			System.String type = "Unknown";

			switch(actorType)
			{
				case ActorTypeEnum.AdtPatientRegistration: type = "AdtPatientRegistration"; break;
				case ActorTypeEnum.OrderPlacer: type = "OrderPlacer"; break;
				case ActorTypeEnum.DssOrderFiller: type = "DssOrderFiller"; break;
				case ActorTypeEnum.AcquisitionModality: type = "AcquisitionModality"; break;
				case ActorTypeEnum.ImageManager: type = "ImageManager"; break;
				case ActorTypeEnum.ImageArchive: type = "ImageArchive"; break;
				case ActorTypeEnum.PerformedProcedureStepManager: type = "PerformedProcedureStepManager"; break;
				case ActorTypeEnum.ImageDisplay: type = "ImageDisplay"; break;
				case ActorTypeEnum.EvidenceCreator: type = "EvidenceCreator"; break;
				case ActorTypeEnum.ReportManager: type = "ReportManager"; break;
				case ActorTypeEnum.PrintComposer: type = "PrintComposer"; break;
				case ActorTypeEnum.PrintServer: type = "PrintServer"; break;
				default:
					break;
			}

			return type;
		}

		/// <summary>
		/// Actor enum based type from string.
		/// </summary>
		/// <param name="type">Actor type as String.</param>
		/// <returns>Actor Type as Enum.</returns>
		public static ActorTypeEnum TypeEnum(System.String type)
		{
			ActorTypeEnum typeEnum = ActorTypeEnum.Unknown;

			if (type == "AdtPatientRegistration")
			{
				typeEnum = ActorTypeEnum.AdtPatientRegistration;
			}
			else if (type == "OrderPlacer")
			{
				typeEnum = ActorTypeEnum.OrderPlacer;
			}
			else if (type == "DssOrderFiller")
			{
				typeEnum = ActorTypeEnum.DssOrderFiller;
			}
			else if (type == "AcquisitionModality")
			{
				typeEnum = ActorTypeEnum.AcquisitionModality;
			}
			else if (type == "ImageManager")
			{
				typeEnum = ActorTypeEnum.ImageManager;
			}
			else if (type == "ImageArchive")
			{
				typeEnum = ActorTypeEnum.ImageArchive;
			}
			else if (type == "PerformedProcedureStepManager")
			{
				typeEnum = ActorTypeEnum.PerformedProcedureStepManager;
			}
			else if (type == "ImageDisplay")
			{
				typeEnum = ActorTypeEnum.ImageDisplay;
			}
			else if (type == "EvidenceCreator")
			{
				typeEnum = ActorTypeEnum.EvidenceCreator;
			}
			else if (type == "ReportManager")
			{
				typeEnum = ActorTypeEnum.ReportManager;
			}
			else if (type == "PrintComposer")
			{
				typeEnum = ActorTypeEnum.PrintComposer;
			}
			else if (type == "PrintServer")
			{
				typeEnum = ActorTypeEnum.PrintServer;
			}

			return typeEnum;
		}
	}
	#endregion

	#region ActorName
	/// <summary>
	/// Summary description for ActorName.
	/// </summary>
	public class ActorName
	{
		private ActorTypeEnum _type = ActorTypeEnum.Unknown;
		private System.String _id = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="type">Actor Type.</param>
		/// <param name="id">Actor Id.</param>
		public ActorName(ActorTypeEnum type, System.String id)
		{
			_type = type;
			_id = id;
		}

		/// <summary>
		/// Property - Actor Type
		/// </summary>
		public ActorTypeEnum Type
		{
			get
			{
				return _type;
			}
		}

		/// <summary>
		/// Property - Actor Id
		/// </summary>
		public System.String Id
		{
			get
			{
				return _id;
			}
		}

		/// <summary>
		/// Property - TypeId combination - used as indexer
		/// </summary>
		public System.String TypeId
		{
			get
			{
				System.String typeId = System.String.Format("{0}_{1}", ActorTypes.Type(_type), _id);
				return typeId;
			}
		}
	}

	#endregion ActorName 
}
