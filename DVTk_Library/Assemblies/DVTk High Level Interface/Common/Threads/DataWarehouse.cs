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

using DvtkHighLevelInterface.Common.Messages;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Threads;



namespace DvtkHighLevelInterface.Common.Threads
{
	/// <summary>
	/// Summary description for DataWarehouse.
	/// Maak duidelijk dat het om snapshots gaat!!!
	/// </summary>
	public class DataWarehouse
	{
		private MessageCollection messages = new MessageCollection();

		public Object lockObject = new Object();

		public DataWarehouse()
		{
			// Nothing.
		}

		public DicomProtocolMessageCollection Messages(DicomThread dicomThread)
		{
			DicomProtocolMessageCollection dicomProtocolMessageCollection = new DicomProtocolMessageCollection();

			lock(lockObject)
			{
				foreach(DicomProtocolMessage dicomProtocolMessage in dicomThread.Messages)
				{
					dicomProtocolMessageCollection.Add(dicomProtocolMessage);
				}
			}

			return(dicomProtocolMessageCollection);
		}

		public void ClearMessages(DicomThread dicomThread)
		{
			lock(lockObject)
			{
				dicomThread.Messages.Clear();
			}
		}

		public MessageCollection GlobalMessages()
		{
			MessageCollection messageCollection = new MessageCollection();

			lock(lockObject)
			{
				foreach(Message message in globalMessages)
				{
					messageCollection.Add(message);
				}
			}

			return(messageCollection);
		}

		internal void AddMessage(DicomThread dicomThread, DicomProtocolMessage message)
		{
			lock(lockObject)
			{
				dicomThread.Messages.Add(message);
				this.messages.Add(message);
			}
		}
	}
}
