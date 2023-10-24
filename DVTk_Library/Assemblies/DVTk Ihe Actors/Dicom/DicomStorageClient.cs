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
using DvtkHighLevelInterface.Dicom.Other;
using Dvtk.IheActors.Bases;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomStorageClient.
	/// </summary>
	public class DicomStorageClient : DicomClient
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - containing actor.</param>
		/// <param name="actorName">Destination Actor Name.</param>
        public DicomStorageClient(BaseActor parentActor, ActorName actorName)
            : base(parentActor, actorName, false)
		{
			_presentationContexts = new PresentationContext[1];

			String[] transferSyntaxes = new String[1];
			transferSyntaxes[0] = "1.2.840.10008.1.2"; // Transfer Syntax Name(s)
			PresentationContext presentationContext = new PresentationContext("1.2.840.10008.5.1.4.1.1.7", // Abstract Syntax Name
				transferSyntaxes);
			_presentationContexts[0] = presentationContext;
		}
	}
}
