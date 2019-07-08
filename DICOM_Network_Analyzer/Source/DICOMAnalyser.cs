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
using TCP;
using System.Collections.Specialized;

namespace DICOM
{
	/// <summary>
	/// This class creates an Association object for each TCPState 
	/// (held in its UserInfo property) and every time data is received, it checks if there 
	/// is a full PDU.  If there is, then Association.HandlePDU is called to analyze it.
	/// </summary>
	public class DICOMAnalyser
	{
		public string BaseFileName;
		public delegate void OutputEvent(string s, bool ToLog, bool ToScreen);
		public OutputEvent Output;
        SnifferUI.DICOMSniffer snifferObj = null;
		private ListDictionary list = new ListDictionary();
        
		public DICOMAnalyser(SnifferUI.DICOMSniffer mainObj )
		{
            snifferObj = mainObj;
		}

        private void OutputHandler(string s, bool a, bool b)
		{
			if(Output != null)
				Output(s,a,b);
		}

		/// <summary>
		/// This is the main routine called from TCP layer for handling DICOM data
		/// </summary>
		/// <param name="state"></param>
		/// <param name="direction"></param>
		public void ReceiveTCPData(TCPState state, int direction) 
		{
			byte[] p = state.data[direction];
			int length = (int)state.Position[direction];
			uint Position = 0;
			Association Assoc = (Association)state.UserInfo;
			bool TryAnotherPDU = true;
			
			while(length >= Position + 6 && TryAnotherPDU)
			{
				TryAnotherPDU = false;
				byte PDUType = p[Position];
				uint Index = Position + 2;
				uint PDULength = DICOMUtility.Get4BytesBigEndian( p , ref Index);

				if(Assoc == null && PDUType == 1)
				{
                    Assoc = new Association(state, BaseFileName + state.Signature[0], System.DateTime.Now);
					Assoc.Output += new Association.OutputEvent(OutputHandler);
					state.UserInfo = Assoc;
					string signature = state.Signature[0];
					list.Add(signature,Assoc);
					OutputHandler("New : " + signature + "\r\n",true,true);
//                    snifferObj.Invoke(snifferObj.AddConnectionHandler, new object[] { signature });
				}

				if(Assoc !=null && length >= Index + PDULength)
				{
					lock(Assoc)
					{
						Assoc.HandlePDU(p,Position,PDULength,direction, state);
						TryAnotherPDU = true;
					}
				}
				else if(length>64000 && Assoc==null) // non-DICOM
				{
					OutputHandler("Non-DICOM data : " + state.Signature[0] + "\r\n",true,true);
					break;  // don't process any more if we have a currently unhandleable PDU
				}
				else
				{
					break;  // don't prcess any more if we have a currently unhandleable PDU
				}
				Position += PDULength + 6;
			}

			if(Position > 0 )
			{
				if(state.data[direction] != null)
				{
					state.UsedData(Position, direction);
				}

				// Check if there is anything we have "stacked up" on the other direction
				ReceiveTCPData(state, 1-direction);
			}		
	
			return;
		}

		public void EndOfStream(TCPState state, int direction, string result) 
		{
			Association Assoc = (Association)state.UserInfo;
			if(Assoc != null)  // i.e. if DICOM
			{
				lock(Assoc)
				{				
					OutputHandler("End : " + state.Signature[0] + "\r\n" + result + "\r\n",true,true);
					Assoc.Out(result + "\r\n",true);
					Assoc.HandleEnd();

					if((state.State[0] != TCPState.States.RUNNING) && (state.State[1] != TCPState.States.RUNNING))  // i.e. if DICOM
					{
						
						state.Clear(); // should have been done elsewhere, but no harm to repeat

						// Select the first association from the list by default
                        snifferObj.Invoke(snifferObj.SelectConnectionHandler);                        
					}

					//Handle incomplete byte stream
					if((result.IndexOf("DEAD data seen") != -1) &&
						(((state.State[0] != TCPState.States.RUNNING) && (state.State[1] != TCPState.States.FINISHED)) ||
						((state.State[0] != TCPState.States.FINISHED) && (state.State[1] != TCPState.States.RUNNING))))
					{
						Assoc.Dispose(true);
						state.Clear();
					}

					//Handle unexpected Abort
					if(result == "Forced Close")
					{
						Assoc.Dispose(true);
						state.Clear();
					}
				}
			}
		}

		public ListDictionary ConnectionsList
		{
			get
			{
				return list;
			}
		}
	}
}
