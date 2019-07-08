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

using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Hl7;

namespace Dvtk.IheActors.Bases
{
	#region ClientServer Types
	/// <summary>
	/// Dicom Client Type Enum.
	/// </summary>
	public enum DicomClientTypeEnum
	{
		DicomMppsClient,
		DicomPrintClient,
		DicomQueryRetrieveClient,
		DicomStorageCommitClient,
		DicomStorageClient,
		DicomWorklistClient,
		Unknown
	}

	/// <summary>
	/// Dicom Server Type Enum.
	/// </summary>
	public enum DicomServerTypeEnum
	{
		DicomMppsServer,
		DicomPrintServer,
		DicomQueryRetrieveServer,
		DicomStorageCommitServer,
		DicomStorageServer,
		DicomWorklistServer,
		Unknown
	}

	/// <summary>
	/// Hl7 Client Type Enum.
	/// </summary>
	public enum Hl7ClientTypeEnum
	{
		Hl7Client,
		Unknown
	}

	/// <summary>
	/// Hl7 Server Type Enum.
	/// </summary>
	public enum Hl7ServerTypeEnum
	{
		Hl7Server,
		Hl7QueryServer,
		Unknown
	}

	#endregion ClientServer Types

	#region ClientServer Factory
	/// <summary>
	/// Summary description for ClientServerFactory.
	/// </summary>
	public class ClientServerFactory
	{
		/// <summary>
		/// Create a Dicom Client of the given type.
		/// </summary>
		/// <param name="dicomClientType">Dicom Client Type - enum.</param>
		/// <param name="fromActor">From Actor instance.</param>
		/// <param name="toActorName">To Actor Name.</param>
		/// <returns>Dicom Client.</returns>
		public static DicomClient CreateDicomClient(DicomClientTypeEnum dicomClientType, BaseActor fromActor, ActorName toActorName)
		{
			DicomClient dicomClient = null;

			switch (dicomClientType)
			{
				case DicomClientTypeEnum.DicomMppsClient:
					dicomClient = new DicomMppsClient(fromActor, toActorName);
					break;
				case DicomClientTypeEnum.DicomPrintClient:
                    dicomClient = new DicomPrintClient(fromActor, toActorName);
					break;
				case DicomClientTypeEnum.DicomQueryRetrieveClient:
                    dicomClient = new DicomQueryRetrieveClient(fromActor, toActorName);
					break;
				case DicomClientTypeEnum.DicomStorageCommitClient:
                    dicomClient = new DicomStorageCommitClient(fromActor, toActorName);
					break;
				case DicomClientTypeEnum.DicomStorageClient:
                    dicomClient = new DicomStorageClient(fromActor, toActorName);
					break;
				case DicomClientTypeEnum.DicomWorklistClient:
                    dicomClient = new DicomWorklistClient(fromActor, toActorName);
					break;
				case DicomClientTypeEnum.Unknown:
				default:
					break;
			}

			return dicomClient;
		}

		/// <summary>
		/// Create a Dicom Server of the given type.
		/// </summary>
		/// <param name="dicomServerType">Dicom Server Type - enum.</param>
		/// <param name="toActor">To Actor instance.</param>
		/// <param name="fromActorName">From Actor Name.</param>
		/// <returns>Dicom Server.</returns>
		public static DicomServer CreateDicomServer(DicomServerTypeEnum dicomServerType, BaseActor toActor, ActorName fromActorName)
		{
			DicomServer dicomServer = null;

			switch (dicomServerType)
			{
				case DicomServerTypeEnum.DicomMppsServer:
					dicomServer = new DicomMppsServer(toActor, fromActorName);
					break;
				case DicomServerTypeEnum.DicomPrintServer:
					dicomServer = new DicomPrintServer(toActor, fromActorName);
					break;
				case DicomServerTypeEnum.DicomQueryRetrieveServer:
					dicomServer = new DicomQueryRetrieveServer(toActor, fromActorName);
					break;
				case DicomServerTypeEnum.DicomStorageCommitServer:
					dicomServer = new DicomStorageCommitServer(toActor, fromActorName);
					break;
				case DicomServerTypeEnum.DicomStorageServer:
					dicomServer = new DicomStorageServer(toActor, fromActorName);
					break;
				case DicomServerTypeEnum.DicomWorklistServer:
					dicomServer = new DicomWorklistServer(toActor, fromActorName);
					break;
				case DicomServerTypeEnum.Unknown:
				default:
					break;
			}

			return dicomServer;
		}

		/// <summary>
		/// Create an Hl7 Client of the given type.
		/// </summary>
		/// <param name="hl7ClientType">Hl7 Client Type - enum.</param>
		/// <param name="fromActor">From Actor instance.</param>
		/// <param name="toActorName">To Actor Name.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">HL7 Configuration.</param>
		/// <returns>Hl7 Client.</returns>
		public static Hl7Client CreateHl7Client(Hl7ClientTypeEnum hl7ClientType, BaseActor fromActor, ActorName toActorName, CommonConfig commonConfig, Hl7PeerToPeerConfig config)
		{
			Hl7Client hl7Client = null;

			switch (hl7ClientType)
			{
				case Hl7ClientTypeEnum.Hl7Client:
					hl7Client = new Hl7Client(fromActor, toActorName, commonConfig, config);
					break;
				case Hl7ClientTypeEnum.Unknown:
				default:
					break;
			}

			return hl7Client;
		}

		/// <summary>
		/// Create a Hl7 Server of the given type.
		/// </summary>
		/// <param name="hl7ServerType">Hl7 Server Type - enum.</param>
		/// <param name="toActor">To Actor instance.</param>
		/// <param name="fromActorName">From Actor Name.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">HL7 Configuration.</param>
		/// <returns>Hl7 Server.</returns>
		public static Hl7Server CreateHl7Server(Hl7ServerTypeEnum hl7ServerType, BaseActor toActor, ActorName fromActorName, CommonConfig commonConfig, Hl7PeerToPeerConfig config)
		{
			Hl7Server hl7Server = null;

			switch (hl7ServerType)
			{
				case Hl7ServerTypeEnum.Hl7QueryServer:
					hl7Server = new Hl7QueryServer(toActor, fromActorName, commonConfig, config);
					break;
				case Hl7ServerTypeEnum.Hl7Server:
					hl7Server = new Hl7Server(toActor, fromActorName, commonConfig, config);
					break;
				case Hl7ServerTypeEnum.Unknown:
				default:
					break;
			}

			return hl7Server;
		}

		#endregion ClientServer Factory
	}
}
