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
using System.Diagnostics;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using TCP;
using Dvtk.Sessions;
using DICOMSniffer;
using DvtkData.Media;
using DvtkData.Dimse;

namespace DICOM
{
	/// <summary>
	/// Summary description for Association.
	/// </summary>
	public class Association
	{
		TCPState State;
		public delegate void OutputEvent(string s, bool ToLog, bool ToScreen) ;
		public OutputEvent Output;
		int PDUNumber = 0;
		ArrayList[] ByteData = new ArrayList[2] { new ArrayList(), new ArrayList()};
		ArrayList DatasetData = new ArrayList();
		static ASCIIEncoding ASCII = new ASCIIEncoding();
		byte LastPDU = 0;
		public String BaseFileName;
		string[] TransferSyntax = new string[256];
		StreamWriter textLog = null;
		StringBuilder ReqPCs = new StringBuilder(4096);
		StringBuilder AccPCs = new StringBuilder(4096);
		StringBuilder reqPduDetail = new StringBuilder(4096);
		StringBuilder accPduDetail = new StringBuilder(4096);
		string callingAETitle = "";
		string calledAETitle = "";
		string reqPduLength = "";
		string accPduLength = "";
		ArrayList pduList = new ArrayList();
		ArrayList cmdPdusList = new ArrayList();
		SOPClassMap sopClassMap = null;
		TransferSyntaxMap tsMap = null;
		string commandType;
		ArrayList pcList = new ArrayList();
		ArrayList sopList = new ArrayList();
		ArrayList reqScuRoleList = new ArrayList();
		ArrayList reqScpRoleList = new ArrayList();
		ArrayList accScuRoleList = new ArrayList();
		ArrayList accScpRoleList = new ArrayList();
		ArrayList reqTSList = new ArrayList();
		ArrayList accTSList = new ArrayList();
        System.DateTime startTimeForAssoc;
		bool isLastDatasetFragmentRecd = false;
		bool isDataSetRecd = false;
        bool disposed = false;


        public Association(TCPState state, String filename, System.DateTime time)
		{
			State = state;
			BaseFileName = filename;
			startTimeForAssoc = time;
			textLog= File.CreateText(BaseFileName + ".log");

			//Load SOP Class & Transfer Syntax maps
			sopClassMap = new SOPClassMap();
			tsMap = new TransferSyntaxMap();
		}

		public void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (textLog != null)
                    {
                        lock (textLog)
                        {
                            textLog.Close();
                            textLog.Dispose();
                            textLog = null;
                        }
                    }
                }                
            }
            disposed = true;
        }

		~Association()
		{
            Dispose(false);
		}

		/// <summary>
		/// Dump PDU in the file for further reading and analysis.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="Position"></param>
		/// <param name="filename"></param>
		/// <param name="direction"></param>
		/// <param name="length"></param>
		/// <param name="number"></param>
		void DumpPDU(byte[] data, uint Position, string filename, int direction, uint length, int number)
		{
            string dir = filename + @"\PDUs\";
            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (System.UnauthorizedAccessException)
            {
                string msg = "Please check authorization and log-in with Administrator previlages.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
			int pduLength = (int)length + 6; //include length of PDU header
			string file = dir + number.ToString() + "_" + (direction==1?"OUT":"IN") + ".pdu";

			byte[] pduData = null;

            if (Position != 0)
            {
                pduData = new byte[pduLength];
                Array.Copy(data, Position, pduData, 0, pduLength);
            }

			using(FileStream f=new FileStream(file,FileMode.Create))
			{
                if (Position != 0)
                    f.Write(pduData, 0, pduLength);
                else
                {
                    f.Write(data, 0, pduLength);
                }
				f.Close();
			}
            
			string message = string.Format("PDU logged to file {0} at {1}\r\n",file,System.DateTime.Now);
			Out(message,true);
		}

		/// <summary>
		/// Save selected dataset as DCM FIle
		/// </summary>
		/// <param name="data"></param>
		/// <param name="filename"></param>
		public bool DumpAsDCMFile(byte[] data, string tranferSyntax, string filename)
		{
			bool ok = false;

			//Set Dataset
            string dataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\DICOM Network Analyzer";
            string tempFile = dataDirectory + @"\Dcm\temp.dcm";
			using(FileStream f=new FileStream(tempFile,FileMode.Create))
			{
				f.Write(data,0,data.Length);
				f.Close();
			}

			//Read Dataset from the file
			DataSet dataset = Dvtk.DvtkDataHelper.ReadDataSetFromFile(tempFile,true);

			//Delete the temp file
			FileInfo tempfile = new FileInfo(tempFile);
			tempfile.Delete();

			if(dataset == null)
			{
				return false;
			}			

			DicomFile dicomMediaFile = new DicomFile();

			// set up the file head
			FileHead fileHead = new FileHead();

			// add the Transfer Syntax UID
			DvtkData.Dul.TransferSyntax transferSyntax = new DvtkData.Dul.TransferSyntax(DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian.UID);
			fileHead.TransferSyntax = transferSyntax;

			// set up the file meta information
			FileMetaInformation fileMetaInformation = new FileMetaInformation();

			// add the FMI version
			fileMetaInformation.AddAttribute(Tag.FILE_META_INFORMATION_VERSION.GroupNumber,
				Tag.FILE_META_INFORMATION_VERSION.ElementNumber, VR.OB, 1, 2);

			// add the SOP Class UID
			System.String sopClassUid = "";
			DvtkData.Dimse.Attribute attribute = dataset.GetAttribute(Tag.SOP_CLASS_UID);
			if (attribute != null)
			{
				UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
				if (uniqueIdentifier.Values.Count > 0)
				{
					sopClassUid = uniqueIdentifier.Values[0];
				}
			}
			fileMetaInformation.AddAttribute(Tag.MEDIA_STORAGE_SOP_CLASS_UID.GroupNumber,
				Tag.MEDIA_STORAGE_SOP_CLASS_UID.ElementNumber, VR.UI, sopClassUid);

			// add the SOP Instance UID
			System.String sopInstanceUid = "";
			attribute = dataset.GetAttribute(Tag.SOP_INSTANCE_UID);
			if (attribute != null)
			{
				UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
				if (uniqueIdentifier.Values.Count > 0)
				{
					sopInstanceUid = uniqueIdentifier.Values[0];
				}
			}
			fileMetaInformation.AddAttribute(Tag.MEDIA_STORAGE_SOP_INSTANCE_UID.GroupNumber,
				Tag.MEDIA_STORAGE_SOP_INSTANCE_UID.ElementNumber, VR.UI, sopInstanceUid);

			// add the Transfer Syntax UID
			fileMetaInformation.AddAttribute(Tag.TRANSFER_SYNTAX_UID.GroupNumber,
				Tag.TRANSFER_SYNTAX_UID.ElementNumber, VR.UI, tranferSyntax);

			// add the Implemenation Class UID
			fileMetaInformation.AddAttribute(Tag.IMPLEMENTATION_CLASS_UID.GroupNumber,
                Tag.IMPLEMENTATION_CLASS_UID.ElementNumber, VR.UI, "1.2.826.0.1.3680043.2.1545.1");

			// add the Implementation Version Name
			fileMetaInformation.AddAttribute(Tag.IMPLEMENTATION_VERSION_NAME.GroupNumber,
				Tag.IMPLEMENTATION_VERSION_NAME.ElementNumber, VR.SH, "DNA");

			// set up the dicomMediaFile contents
			dicomMediaFile.FileHead = fileHead;
			dicomMediaFile.FileMetaInformation = fileMetaInformation;
			dicomMediaFile.DataSet = dataset;

			// write the dicomMediaFile to file
			if(Dvtk.DvtkDataHelper.WriteDataSetToFile(dicomMediaFile, filename))
			{
				string message = string.Format("DataSet logged to file {0} at {1}\r\n",filename,System.DateTime.Now);
				Out(message,true);
				ok = true;
			}
			
			//Clear all temporary pix files
			FileInfo file = new FileInfo(filename);
			ArrayList theFilesToRemove = new ArrayList();
			DirectoryInfo theDirectoryInfo = new DirectoryInfo(file.DirectoryName);
			FileInfo[] thePixFilesInfo;

			if (theDirectoryInfo.Exists)
			{
				thePixFilesInfo = theDirectoryInfo.GetFiles("*.pix");

				foreach (FileInfo theFileInfo in thePixFilesInfo)
				{
					string thePixFileName = theFileInfo.Name;

					theFilesToRemove.Add(thePixFileName);
				}				
			}

			//Delete all pix files
			foreach(string theFileName in theFilesToRemove)
			{
				string theFullFileName = System.IO.Path.Combine(theDirectoryInfo.FullName, theFileName);

				if (File.Exists(theFullFileName))
				{
					try
					{
						File.Delete(theFullFileName);
					}
					catch(Exception exception)
					{
						string theWarningText = string.Format("Could not be delete the {0} temporary file.\n due to exception: {1}\n\n", theFullFileName, exception.Message);

						Out(theWarningText,true);
					}
				}				
			}

			return ok;
		}

		/// <summary>
		/// Dump  All DICOM data on the screen(activity logging tab) and in the log file.
		/// </summary>
		/// <param name="ByteData"></param>
		/// <param name="direction"></param>
		/// <param name="TransferSyntax"></param>
		/// <returns></returns>
		string DICOMDump(ArrayList ByteData,string TransferSyntax)
		{
			// This could be made a lot more efficient, but copying seems quick 
			// and easy for now!
			uint DatasetLength = 0;
			foreach(byte[] p in ByteData)
				DatasetLength += (uint)p.Length;

			byte[] Bytes = new byte[DatasetLength];

			DatasetLength = 0;
			foreach(byte[] p in ByteData)
			{
				p.CopyTo(Bytes,DatasetLength);
				DatasetLength += (uint)p.Length;
			}
			uint position = 0;

			return DumpDataSet(Bytes, ref position, DatasetLength, TransferSyntax, "");
		}

		/// <summary>
		/// Dump Commandset
		/// </summary>
		/// <param name="ByteData"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		string DICOMCommandDump(ArrayList ByteData)
		{
			// This could be made a lot more efficient, but copying seems quick 
			// and easy for now!
			uint CommandLength = 0;
			foreach(byte[] p in ByteData)
				CommandLength += (uint)p.Length;

			byte[] Bytes = new byte[CommandLength];

			CommandLength = 0;
			foreach(byte[] p in ByteData)
			{
				p.CopyTo(Bytes,CommandLength);
				CommandLength += (uint)p.Length;
			}
			uint position = 0;

			return DumpCmdSet(Bytes, ref position, CommandLength);
		}

		/// <summary>
		/// Dump Commandset
		/// </summary>
		/// <param name="Bytes"></param>
		/// <param name="CurrentPosition"></param>
		/// <param name="EndPosition"></param>
		/// <returns></returns>
		string DumpCmdSet(byte[] Bytes, ref uint CurrentPosition, uint EndPosition)
		{
			StringBuilder s = new StringBuilder();
			try
			{			
				while(CurrentPosition < EndPosition)
				{
					ushort	Group   = DICOMUtility.Get2Bytes(Bytes,ref CurrentPosition,false);
					ushort	Element = DICOMUtility.Get2Bytes(Bytes,ref CurrentPosition,false);
					uint	Length;
					CVRType VR;

					VR=DataDictionary.FindVR(Group,Element);
					Length= DICOMUtility.Get4Bytes(Bytes,ref CurrentPosition,false);
					
					s.Append(string.Format("({0:X4},{1:X4}) {2:G} {3:X4}: ",Group,Element,VR.ToString(),Length));

					uint pos = CurrentPosition;  // discardable copy
				
					if(Group==0x0000 && Element==0x0100)//Command Field
					{
						ushort cmdType = DICOMUtility.Get2Bytes(Bytes,ref pos,false);
						switch(cmdType)
						{
							case 0x0001:
							{
								commandType = "C_STORE_RQ";
								break;
							}
							case 0x8001:
							{
								commandType = "C_STORE_RSP";
								break;
							}
							case 0x0021:
							{
								commandType = "C_MOVE_RQ";
								break;
							}
							case 0x8021:
							{
								commandType = "C_MOVE_RSP";
								break;
							}
							case 0x0010:
							{
								commandType = "C_GET_RQ";
								break;
							}
							case 0x8010:
							{
								commandType = "C_GET_RSP";
								break;
							}
							case 0x0020:
							{
								commandType = "C_FIND_RQ";
								break;
							}
							case 0x8020:
							{
								commandType = "C_FIND_RSP";
								break;
							}
							case 0x0030:
							{
								commandType = "C_ECHO_RQ";
								break;
							}
							case 0x8030:
							{
								commandType = "C_ECHO_RSP";
								break;
							}
							case 0x0130:
							{
								commandType = "N_ACTION_RQ";
								break;
							}
							case 0x8130:
							{
								commandType = "N_ACTION_RSP";
								break;
							}
							case 0x0140:
							{
								commandType = "N_CREATE_RQ";
								break;
							}
							case 0x8140:
							{
								commandType = "N_CREATE_RSP";
								break;
							}
							case 0x0150:
							{
								commandType = "N_DELETE_RQ";
								break;
							}
							case 0x8150:
							{
								commandType = "N_DELETE_RSP";
								break;
							}
							case 0x0100:
							{
								commandType = "N_EVENT_REPORT_RQ";
								break;
							}
							case 0x8100:
							{
								commandType = "N_EVENT_REPORT_RSP";
								break;
							}
							case 0x0110:
							{
								commandType = "N_GET_RQ";
								break;
							}
							case 0x8110:
							{
								commandType = "N_GET_RSP";
								break;
							}
							case 0x0120:
							{
								commandType = "N_SET_RQ";
								break;
							}
							case 0x8120:
							{
								commandType = "N_SET_RSP";
								break;
							}
							case 0x0FFF:
							{
								commandType = "C_CANCEL_RQ";
								break;
							}
							default:
							{
								commandType = "CMD_UNKNOWN";
								break;
							}
						}						
					}
					else
					{
						switch(VR)
						{
							case CVRType.OW:
							case CVRType.US:
							case CVRType.AT:
							case CVRType.SS:
								for(int i=0;i<Math.Min(20,Length);i+=2)
									s.Append(string.Format("{0:X4} ",DICOMUtility.Get2Bytes(Bytes,ref pos,false)));
								break;

							case CVRType.UL:
							case CVRType.SL:
								for(int i=0;i<Math.Min(20,Length);i+=4)
									s.Append(string.Format("{0:X8} ",DICOMUtility.Get4Bytes(Bytes,ref pos,false)));
								break;

							case CVRType.FL:
							case CVRType.OF:
								for(int i=0;i<Math.Min(20,Length);i+=4)
								{
									s.Append(string.Format("{0:G} ",BitConverter.ToSingle(Bytes,(int)pos)));
									pos += 4;
								}
								break;

							case CVRType.FD:
								for(int i=0;i<Math.Min(24,Length);i+=8)
								{
									s.Append(string.Format("{0:G} ",BitConverter.ToDouble(Bytes,(int)pos)));
									pos += 8;
								}
								break;

							case CVRType.DT:		// String max 12
							case CVRType.TM:		// String max 12
							case CVRType.DA:		// String max 12
							case CVRType.IS:		// String max 12
							case CVRType.AE:		// String max 16
							case CVRType.CS:
							case CVRType.DS:
							case CVRType.SH:
							case CVRType.LO:		// String max 64
							case CVRType.PN:
							case CVRType.UI:		// String max 64 pad zero
							case CVRType.ST:		// String max 1024
							case CVRType.LT:		// String max 10240
							case CVRType.AS:		// String fixed 4
							case CVRType.UT:		// unlimited string
							{
								uint len = Math.Min(64,Length);
								pos += len;
								while(Bytes[CurrentPosition+len-1]==0 && len>0)
									len--;
								s.Append(ASCII.GetString(Bytes,(int)CurrentPosition,(int)len));

								break;
							}							
							default:
							{
								uint len = Math.Min(20,Length);
								s.Append(BitConverter.ToString(Bytes,(int)CurrentPosition,(int)len));
								pos += len;
								break;
							}
						}
					}
					CurrentPosition += Length;
					if(pos < CurrentPosition)
						s.Append("...");
					s.Append("\r\n");
				}
			}
			catch(Exception e)
			{
				s.Append("\r\n***********ERROR****************\r\n" + e.Message + "\r\n" + e.StackTrace + "\r\n");
			}
			return s.ToString();			
		}

		/// <summary>
		/// Dump Dataset on the screen(activity logging tab) and in the log file.
		/// </summary>
		/// <param name="Bytes"></param>
		/// <param name="CurrentPosition"></param>
		/// <param name="EndPosition"></param>
		/// <param name="TransferSyntax"></param>
		/// <param name="prefix"></param>
		/// <returns></returns>
		string DumpDataSet(byte[] Bytes, ref uint CurrentPosition, uint EndPosition, string TransferSyntax, string prefix)
		{
			StringBuilder s = new StringBuilder();
			try
			{
				// This seciton is here (not top level) to allow for CP165 oddities and UN sequence (always Imp VR)
				bool isBigEndian = (TransferSyntax == "1.2.840.10008.1.2.2");
				bool isExplicitVR = (TransferSyntax != "1.2.840.10008.1.2");
				bool isEncapsulated = !(TransferSyntax == "1.2.840.10008.1.2" || TransferSyntax == "1.2.840.10008.1.2.1" || TransferSyntax == "1.2.840.10008.1.2.2");
			
				while(CurrentPosition < EndPosition)
				{
					s.Append(prefix);
					ushort	Group   = DICOMUtility.Get2Bytes(Bytes,ref CurrentPosition,isBigEndian);
					ushort	Element = DICOMUtility.Get2Bytes(Bytes,ref CurrentPosition,isBigEndian);
					uint	Length;
					CVRType VR;

					if(isExplicitVR)
					{
						ushort vr = DICOMUtility.Get2BytesBigEndian(Bytes,ref CurrentPosition);
						VR=new CVRType(vr);
						if(VR.isLongHeader())
						{
							CurrentPosition +=2;  // reserved
							Length= DICOMUtility.Get4Bytes(Bytes,ref CurrentPosition,isBigEndian);
						}
						else
							Length= DICOMUtility.Get2Bytes(Bytes,ref CurrentPosition,isBigEndian);
					}
					else
					{
						VR=DataDictionary.FindVR(Group,Element);
						Length= DICOMUtility.Get4Bytes(Bytes,ref CurrentPosition,isBigEndian);
					}

					s.Append(string.Format("({0:X4},{1:X4}) {2:G} {3:X4}: ",Group,Element,VR.ToString(),Length));

					// Bale out if end of sequence
					if(Group == 0xFFFE && Element == 0xE00D)
					{
						s.Append("\r\n");
						return s.ToString();
					}

					uint pos = CurrentPosition;  // discardable copy
				
					if(Group==0x7FE0 && Element==0x0010 && isEncapsulated)
					{
						bool finished = false;
						while(pos < Length+CurrentPosition || (Length==0xFFFFFFFF && !finished))
						{
							s.Append("\r\n");
							ushort PDTag1 = DICOMUtility.Get2Bytes(Bytes,ref pos,isBigEndian);
							ushort PDTag2 = DICOMUtility.Get2Bytes(Bytes,ref pos,isBigEndian);
							uint  PDLength= DICOMUtility.Get4Bytes(Bytes,ref pos,isBigEndian);
							finished = PDTag2 == 0xE0DD;
						
							if(!finished)
							{
								uint len = Math.Min(20,PDLength);
								s.Append(string.Format("Fragment - length = {0:X8} : ",PDLength));
								s.Append(BitConverter.ToString(Bytes,(int)pos,(int)len));
							}
							pos += PDLength;
						}
						if(Length == 0xFFFFFFFF) 
							Length = pos - CurrentPosition;
						break;
					}
					else
					{
						switch(VR)
						{
							case CVRType.OW:
							case CVRType.US:
							case CVRType.AT:
							case CVRType.SS:
								for(int i=0;i<Math.Min(20,Length);i+=2)
									s.Append(string.Format("{0:X4} ",DICOMUtility.Get2Bytes(Bytes,ref pos,isBigEndian)));
								break;

							case CVRType.UL:
							case CVRType.SL:
								for(int i=0;i<Math.Min(20,Length);i+=4)
									s.Append(string.Format("{0:X8} ",DICOMUtility.Get4Bytes(Bytes,ref pos,isBigEndian)));
								break;

							case CVRType.FL:
							case CVRType.OF:
								for(int i=0;i<Math.Min(20,Length);i+=4)
								{
									s.Append(string.Format("{0:G} ",BitConverter.ToSingle(Bytes,(int)pos)));
									pos += 4;
								}
								break;

							case CVRType.FD:
								for(int i=0;i<Math.Min(24,Length);i+=8)
								{
									s.Append(string.Format("{0:G} ",BitConverter.ToDouble(Bytes,(int)pos)));
									pos += 8;
								}
								break;

							case CVRType.DT:		// String max 12
							case CVRType.TM:		// String max 12
							case CVRType.DA:		// String max 12
							case CVRType.IS:		// String max 12
							case CVRType.AE:		// String max 16
							case CVRType.CS:
							case CVRType.DS:
							case CVRType.SH:
							case CVRType.LO:		// String max 64
							case CVRType.PN:
							case CVRType.UI:		// String max 64 pad zero
							case CVRType.ST:		// String max 1024
							case CVRType.LT:		// String max 10240
							case CVRType.AS:		// String fixed 4
							case CVRType.UT:		// unlimited string
							{
								uint len = Math.Min(64,Length);
								pos += len;
								while(Bytes[CurrentPosition+len-1]==0 && len>0)
									len--;
								s.Append(ASCII.GetString(Bytes,(int)CurrentPosition,(int)len));

								break;
							}
							case CVRType.SQ:		// unlimited string
							{
								bool finished = false;
								while((pos < (Length+CurrentPosition)) || ((Length==0xFFFFFFFF) && (!finished)))
								{
									s.Append("\r\n");
									ushort SQTag1 = DICOMUtility.Get2Bytes(Bytes,ref pos,isBigEndian);
									ushort SQTag2 = DICOMUtility.Get2Bytes(Bytes,ref pos,isBigEndian);
									uint  SQLength= DICOMUtility.Get4Bytes(Bytes,ref pos,isBigEndian);
									finished = (SQTag2 == 0xE0DD);
									uint end = (SQLength == 0xFFFFFFFF) ? 0xFFFFFFFF : SQLength+pos;
									if(!finished)
									{
										s.Append(DumpDataSet(Bytes,ref pos,end,TransferSyntax,prefix + ">"));
									}
									s.Append("-------------");
								}
								if(Length == 0xFFFFFFFF) 
									Length = pos - CurrentPosition;
								break;
							}
							default:
							{
								uint len = Math.Min(20,Length);
								s.Append(BitConverter.ToString(Bytes,(int)CurrentPosition,(int)len));
								pos += len;
								break;
							}
						}
					}
					CurrentPosition += Length;
					if(pos < CurrentPosition)
						s.Append("...");
					s.Append("\r\n");
				}
			}
			catch(Exception e)
			{
				s.Append("\r\n***********ERROR****************\r\n" + e.Message + "\r\n" + e.StackTrace + "\r\n");
			}
			return s.ToString();			
		}

		public void HandleEnd()
		{
			if( LastPDU == 6 || LastPDU == 7 )
				Out("This TCP direction closed after proper PDU\r\n",true);
			else
				Out("This TCP direction closed prematurely\r\n",true);
		}

		public string ReadShortString(byte[]p, ref uint Position)
		{
			ushort len = DICOMUtility.Get2Bytes(p, ref Position, true);
			string result = ASCII.GetString(p,(int)Position,len);
			Position += len;
			return result;
		}

		/// <summary>
		/// This helper method for handling Association Rq & Accept PDUs
		/// </summary>
		/// <param name="s"></param>
		/// <param name="p"></param>
		/// <param name="Position"></param>
		/// <param name="length"></param>
		/// <param name="direction"></param>
		/// <param name="PDUtype"></param>
		private void HandleRequestPDU(StringBuilder s, byte[] p, uint Position, uint length, int direction, int PDUtype)
		{
			uint index = Position;
			ushort version = DICOMUtility.Get2Bytes(p, ref index, true);
			Position +=4;
			uint PDUEnd = Position + length;
			string CallingAET="", CalledAET="";
			CallingAET = ASCII.GetString(p,(int)Position,16);
			CalledAET  = ASCII.GetString(p,(int)Position+16,16);
			
			Position +=64;

			s.Append((PDUtype==1)?"Association Request\r\n":"Association Acceptance\r\n");
			s.Append("  From " + CallingAET + "\r\n");
			s.Append("  To   " + CalledAET + "\r\n");

			// Populate PDU details
			callingAETitle = CallingAET;
			calledAETitle = CalledAET;
			if(PDUtype==1)
			{
				reqPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t\t" + "1H" + "\r\n");
				reqPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
				
				//Add for PDU header
				reqPduLength = (length+6).ToString();
				reqPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n", length.ToString()));
				reqPduDetail.Append(string.Format("  7-8" + "\t\t" + "Protocol Version" + "\t\t" + "{0}\r\n", version.ToString()));
				reqPduDetail.Append("  9-10" + "\t\t" + "Reserved" + "\t\t\t" + "00 00(not tested)" + "\r\n");
				reqPduDetail.Append(string.Format("  11-26" + "\t\t" + "Called AE Title" + "\t\t" + "{0}\r\n", CallingAET));
				reqPduDetail.Append(string.Format("  27-42" + "\t\t" + "Calling AE Title" + "\t\t" + "{0}\r\n", CalledAET));
				reqPduDetail.Append("  43-74" + "\t\t" + "Reserved" + "\t\t\t" + "00 00...(not tested)" + "\r\n\n");
			}
			else
			{
				accPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t\t" + "2H" + "\r\n");
				accPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
				
				//Add for PDU header
				accPduLength = (length+6).ToString();
				accPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n", length.ToString()));
				accPduDetail.Append(string.Format("  7-8" + "\t\t" + "Protocol Version" + "\t\t" + "{0}\r\n", version.ToString()));
				accPduDetail.Append("  9-10" + "\t\t" + "Reserved" + "\t\t\t" + "00 00(not tested)" + "\r\n");
				accPduDetail.Append(string.Format("  11-26" + "\t\t" + "Called AE Title" + "\t\t" + "{0}\r\n", CallingAET));
				accPduDetail.Append(string.Format("  27-42" + "\t\t" + "Calling AE Title" + "\t\t" + "{0}\r\n", CalledAET));
				accPduDetail.Append("  43-74" + "\t\t" + "Reserved" + "\t\t\t" + "00 00...(not tested)" + "\r\n\n");
			}

			while(Position < PDUEnd)
			{
				byte Type = p[Position++];
				Position++;
				
				uint indexLen = Position;
				ushort itemLen = DICOMUtility.Get2Bytes(p, ref indexLen, true);
				uint itemEnd = indexLen + itemLen;
				
				switch(Type)
				{
					case 0x10:
					{
						string applContextName = ReadShortString(p,ref Position);

						s.Append(string.Format("DICOM UID: {0}\r\n",applContextName));
						if(PDUtype==1)
						{
							reqPduDetail.Append("\t\t" + "  Application Context Item  " + "\r\n\n");
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "10H" + "\r\n",indexLen-3));
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
							reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexLen-1,indexLen,itemLen));
							reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Application Context Name" + "\t" + "{2}\r\n\n", indexLen+1,itemEnd,applContextName));
						}
						else
						{
							accPduDetail.Append("\t\t" + "  Application Context Item  " + "\r\n\n");
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "10H" + "\r\n",indexLen-3));
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
							accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexLen-1,indexLen,itemLen));
							accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Application Context Name" + "\t" + "{2}\r\n\n", indexLen+1,itemEnd,applContextName));
						}
						break;
					}

					case 0x20:
					case 0x21:
					{
						s.Append(string.Format("Outer Type {0:X2} \r\n",Type));
						ushort Len = DICOMUtility.Get2Bytes(p, ref Position, true);
						uint End = Position + Len;

						if(PDUtype==1)
						{
							reqPduDetail.Append("\t\t" + "  Requested Presentation Context Item  " + "\r\n\n");
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "20H" + "\r\n",indexLen-3));
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
							reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexLen-1,indexLen,itemLen));
						}
						else
						{
							accPduDetail.Append("\t\t" + "  Accepted Presentation Context Item  " + "\r\n\n");
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "21H" + "\r\n",indexLen-3));
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
							accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexLen-1,indexLen,itemLen));
						}

						byte PCID = p[Position++];
						Position ++;
						byte result = p[Position++];
						string resultStr = AssocAcResultStr(result);
						Position ++;

						s.Append(string.Format("  Presentation Context {0:G}\r\n", PCID));
																		
						if(Type == 0x21)
						{
							s.Append(string.Format("    Result/Reason = {0:X2}\r\n",result));
							//AccPCs.Append(string.Format("Presentation Context ID: {0:G}\r\n", PCID));
							if(result != 0)
								//AccPCs.Append(string.Format("	{0}\r\n", resultStr));
								accTSList.Add(string.Format("	{0}\r\n", resultStr));
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Presentation Context ID" + "\t" + "{1}\r\n", indexLen+1,PCID));
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen+2));
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Result/Reason" + "\t\t" + "{1}({2})\r\n",indexLen+3,result,resultStr));
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n\n",indexLen+4));
						}
						else
						{
							//ReqPCs.Append(string.Format("Presentation Context ID: {0:G}\r\n", PCID));
							pcList.Add(string.Format("Presentation Context ID: {0:G}\r\n", PCID));
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Presentation Context ID" + "\t" + "{1}\r\n", indexLen+1,PCID));
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen+2));
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen+3));
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n\n",indexLen+4));
						}

						ArrayList proposedTSList = new ArrayList();

						while(Position < End)
						{
							byte Type2 = p[Position++];
							Position++;

							uint indexSubItem = Position;
							ushort subItemLen = DICOMUtility.Get2Bytes(p, ref indexSubItem, true);
							uint subItemEnd = indexSubItem + subItemLen;

							string UID = ReadShortString(p,ref Position);
							s.Append(string.Format("    Type {0:X2} : {1}\r\n",Type2,UID));

							//SOP Class
							if(Type2==0x30)
							{
								object sopClassName = sopClassMap.SOPClassNames[UID];
								if(sopClassName != null)
									//ReqPCs.Append(string.Format("  SOP Class: {0}[{1}]\r\n", sopClassName.ToString(),UID));
									sopList.Add(string.Format("SOP Class: {0}[{1}]\r\n", sopClassName.ToString(),UID));
								else
									//ReqPCs.Append(string.Format("  SOP Class: {0}[{1}]\r\n", "Private SOP Class",UID));
									sopList.Add(string.Format("SOP Class: {0}[{1}]\r\n", "Private SOP Class",UID));
								//ReqPCs.Append(string.Format("	SCU Role: {0}\r\n", ReqScuSupport));
								reqPduDetail.Append("\t\t" + "  SOP Class Sub-item  " + "\r\n\n");
								reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "30H" + "\r\n",indexSubItem-3));
								reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
								reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
								reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "SOP Class" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));								
							}
							
							//Transfer Syntax
							if(Type2==0x40)
							{
								if(PDUtype==1)
								{
									object tsNameProposed = tsMap.TransferSyntaxNames[UID];
									if(tsNameProposed != null)
										//ReqPCs.Append(string.Format("	Transfer Syntax(proposed): {0}[{1}]\r\n", tsNameProposed.ToString(),UID));
										proposedTSList.Add(string.Format("	Transfer Syntax(proposed): {0}[{1}]\r\n", tsNameProposed.ToString(),UID));
									else
										//ReqPCs.Append(string.Format("	Transfer Syntax(proposed): {0}[{1}]\r\n", "Unknown Transfer Syntax",UID));
										proposedTSList.Add(string.Format("	Transfer Syntax(proposed): {0}[{1}]\r\n", "Unknown Transfer Syntax",UID));
									reqPduDetail.Append("\t\t" + "  Transfer Syntax Sub-item  " + "\r\n\n");
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "40H" + "\r\n",indexSubItem-3));
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									if(indexSubItem > 999)
										reqPduDetail.Append(string.Format("  {0,-5}-{1,-5}" + "\t" + "Item Length" + "\t\t" + "{2}\r\n\n", indexSubItem-1,indexSubItem,subItemLen));
									else
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
									if(indexSubItem > 999)
										reqPduDetail.Append(string.Format("  {0,-5}-{1,-5}" + "\t" + "Item Length" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
									else
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Transfer Syntax" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
								}
								else
								{
									TransferSyntax[PCID] = UID;
									object tsNameAccepted = tsMap.TransferSyntaxNames[UID];
									//AccPCs.Append(string.Format("	SCP Role: {0}\r\n", AccScpSupport));
									if(result == 0)
									{
										if(tsNameAccepted != null)
											//AccPCs.Append(string.Format("	Transfer Syntax(accepted): {0}[{1}]\r\n", tsNameAccepted.ToString(),UID));
											accTSList.Add(string.Format("	Transfer Syntax(accepted): {0}[{1}]\r\n", tsNameAccepted.ToString(),UID));
										else
											//AccPCs.Append(string.Format("	Transfer Syntax(accepted): {0}[{1}]\r\n", "Unknown Transfer Syntax",UID));
											accTSList.Add(string.Format("	Transfer Syntax(accepted): {0}[{1}]\r\n", "Unknown Transfer Syntax",UID));
									}
									
									accPduDetail.Append("\t\t" + "  Transfer Syntax Sub-item  " + "\r\n\n");
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "40H" + "\r\n",indexSubItem-3));
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Transfer Syntax" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
								}
							}							
						}
						reqTSList.Add(proposedTSList);
						break;
					}
					case 0x50:
					{
						s.Append(string.Format("  Outer Type {0:X2} \r\n",Type));
						ushort Len = DICOMUtility.Get2Bytes(p, ref Position, true);
						uint End = Position + Len;

						if(PDUtype==1)
						{
							reqPduDetail.Append("\t\t" + "  User Information Item  " + "\r\n\n");
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "50H" + "\r\n",indexLen-3));
							reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
							if(indexLen > 999)
								reqPduDetail.Append(string.Format("  {0,-5}-{1,-5}" + "\t" + "Item Length" + "\t\t" + "{2}\r\n\n", indexLen-1,indexLen,itemLen));
							else
								reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n\n", indexLen-1,indexLen,itemLen));
						}
						else
						{
							accPduDetail.Append("\t\t" + "  User Information Item  " + "\r\n\n");
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "50H" + "\r\n",indexLen-3));
							accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
							if(indexLen > 999)
								accPduDetail.Append(string.Format("  {0,-5}-{1,-5}" + "\t" + "Item Length" + "\t\t" + "{2}\r\n\n", indexLen-1,indexLen,itemLen));
							else
								accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n\n", indexLen-1,indexLen,itemLen));
						}

						while(Position < End)
						{
							byte Type2 = p[Position++];
							Position++;

							uint indexSubItem = Position;
							ushort subItemLen = DICOMUtility.Get2Bytes(p, ref indexSubItem, true);
							uint subItemEnd = indexSubItem + subItemLen;

							switch(Type2)
							{
								case 0x51: // Max PDU Length
									ushort len = DICOMUtility.Get2Bytes(p, ref Position, true);;
									uint MaxPDU = DICOMUtility.Get4Bytes(p, ref Position, true);
									s.Append(string.Format("    Max PDU : {0}\r\n",MaxPDU));
									if(PDUtype==1)
									{
										reqPduDetail.Append("\t\t" + "  Max PDU Length Sub-item  " + "\r\n\n");
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "51H" + "\r\n",indexSubItem-3));
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Max PDU Length" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,MaxPDU));
									}
									else
									{
										accPduDetail.Append("\t\t" + "  Max PDU Length Sub-item  " + "\r\n\n");
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "51H" + "\r\n",indexSubItem-3));
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
										accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
										accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Max PDU Length" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,MaxPDU));
									}
									break;

								case 0x52:  // Name /UID

									string UID = ReadShortString(p,ref Position);
									s.Append(string.Format("    Type {0:X2} : {1}\r\n",Type2,UID));
									if(PDUtype==1)
									{
										reqPduDetail.Append("\t\t" + "  Implementation Class UID Sub-item  " + "\r\n\n");
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "52H" + "\r\n",indexSubItem-3));
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Implementation Class UID" + "\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
									}
									else
									{
										accPduDetail.Append("\t\t" + "  Implementation Class UID Sub-item  " + "\r\n\n");
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "52H" + "\r\n",indexSubItem-3));
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
										accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
										accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Implementation Class UID" + "\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
									}
									break;

								case 0x54: // SCU SCP Role Selection

									ushort scuscplen = DICOMUtility.Get2Bytes(p, ref Position, true);
									ushort uidlen = DICOMUtility.Get2Bytes(p, ref Position, true);
									Position -= 2;
									UID = ReadShortString(p,ref Position);
									//Position += uidlen;
									byte scuRole = p[Position++];
									byte scpRole = p[Position++];
									string ReqScuSupport = "Support";
									string ReqScpSupport = "Non Support";
									string AccScuSupport = "Accepted proposed role";
									string AccScpSupport = "Rejected proposed role";
									
									if(PDUtype==1)
									{
										if(scuRole == 0)
										{
											ReqScuSupport = "Non Support";
										}

										if(scpRole == 1)
										{
											ReqScpSupport = "Support";
										}
										reqScuRoleList.Add(string.Format("	SCU Role: {0}\r\n", ReqScuSupport));
										reqScpRoleList.Add(string.Format("	SCP Role: {0}\r\n", ReqScpSupport));

										reqPduDetail.Append("\t\t" + "  SCU/SCP Role Selection Sub-item  " + "\r\n\n");
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "54H" + "\r\n",indexSubItem-3));
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,scuscplen));
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "UID Length" + "\t\t" + "{2}\r\n", indexSubItem+1,indexSubItem+2,uidlen));
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "SOP Class UID" + "\t\t" + "{2}\r\n", indexSubItem+3,indexSubItem+2+uidlen,UID));
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "SCU Role" + "\t\t" + "{1}({2})\r\n", indexSubItem+3+uidlen,scuRole,ReqScuSupport));
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "SCP Role" + "\t\t" + "{1}({2})\r\n\n", indexSubItem+4+uidlen,scpRole,ReqScpSupport));
									}
									else
									{
										if(scuRole == 0)
										{
											AccScuSupport = "Rejected proposed role";
										}

										if(scpRole == 1)
										{
											AccScpSupport = "Accepted proposed role";
										}
										accScuRoleList.Add(string.Format("	SCU Role: {0}\r\n", AccScuSupport));
										accScpRoleList.Add(string.Format("	SCP Role: {0}\r\n", AccScpSupport));

										accPduDetail.Append("\t\t" + "  SCU/SCP Role Selection Sub-item  " + "\r\n\n");
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "54H" + "\r\n",indexSubItem-3));
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
										accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,scuscplen));
										accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "UID Length" + "\t\t" + "{2}\r\n", indexSubItem+1,indexSubItem+2,uidlen));
										accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "SOP Class UID" + "\t\t" + "{2}\r\n", indexSubItem+3,indexSubItem+2+uidlen,UID));
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "SCU Role" + "\t\t" + "{1}({2})\r\n", indexSubItem+3+uidlen,scuRole,AccScuSupport));
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "SCP Role" + "\t\t" + "{1}({2})\r\n\n", indexSubItem+4+uidlen,scpRole,AccScpSupport));
									}
									break;

								case 0x55:

									UID = ReadShortString(p,ref Position);
									s.Append(string.Format("    Type {0:X2} : {1}\r\n",Type2,UID));
									if(PDUtype==1)
									{
										reqPduDetail.Append("\t\t" + "  Implementation Version Name Sub-item  " + "\r\n\n");
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "55H" + "\r\n",indexSubItem-3));
										reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
										reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Implementation Version Name" + "\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
									}
									else
									{
										accPduDetail.Append("\t\t" + "  Implementation Version Name Sub-item  " + "\r\n\n");
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "55H" + "\r\n",indexSubItem-3));
										accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
										accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
										accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Implementation Version Name" + "\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
									}
									break;

								default:
									ushort Len2 = DICOMUtility.Get2Bytes(p, ref Position, true);
									s.Append(string.Format("    Type {0:X2} : {1}\r\n",Type2,"????"));
									Position += Len2;
									break;
							}
						}
						break;
					}
				}				
			}
		}

		private void PopulateAssociationInfo(byte pduType)
		{
			if(pduType == 1)
			{
				for(int i=0; i<pcList.Count; i++)
				{
					ReqPCs.Append(pcList[i]);
					ReqPCs.Append(sopList[i]);
					if((reqScuRoleList.Count != 0) && (reqScpRoleList.Count != 0))
					{
						ReqPCs.Append(reqScuRoleList[0]);
						ReqPCs.Append(reqScpRoleList[0]);
					}
					else
					{
						ReqPCs.Append(string.Format("	SCU Role: {0}\r\n", "Implicit"));
					}

					ArrayList proposedTS = (ArrayList)reqTSList[i];
					foreach(string ts in proposedTS)
					{
						ReqPCs.Append(ts);
					}
				
					ReqPCs.Append("\n");
				}
			}
			else
			{
				for(int i=0; i<pcList.Count; i++)
				{
					AccPCs.Append(pcList[i]);
					AccPCs.Append(sopList[i]);
					if((accScuRoleList.Count != 0) && (accScpRoleList.Count != 0))
					{
						AccPCs.Append(accScuRoleList[0]);
						AccPCs.Append(accScpRoleList[0]);
					}
					else
					{
						AccPCs.Append(string.Format("	SCP Role: {0}\r\n", "Implicit"));
					}
					AccPCs.Append(accTSList[i]);
					AccPCs.Append("\n");
				}
			}
		}

		private string AssocAcResultStr(byte result)
		{
			string resultStr = "";
			switch(result)
			{
				case 0:
					resultStr = "Accepted";
					break;
				case 1:
					resultStr = "Rejected:User Rejection";
					break;
				case 2:	
					resultStr = "Rejected:No Reason Provided";
					break;
				case 3:
					resultStr = "Rejected:Abstract Syntax Not Supported";
					break;
				case 4:
					resultStr = "Rejected:Transfer Syntaxes Not Supported";
					break;
			}
			return resultStr;
		}

		public string CallingAETitle
		{
			get
			{
				return callingAETitle;
			}
		}

		public string CalledAETitle
		{
			get
			{
				return calledAETitle;
			}
		}

		public string RequestedPresentationContexts
		{
			get
			{
				return ReqPCs.ToString();
			}
		}

		public string AcceptedPresentationContexts
		{
			get
			{
				return AccPCs.ToString();
			}
		}

		public string AssoRqPDUDetail
		{
			get
			{
				return reqPduDetail.ToString();
			}
		}

		public string AssoAcPDUDetail
		{
			get
			{
				return accPduDetail.ToString();;
			}
		}

		public string AssoRqPDULength
		{
			get
			{
				return reqPduLength;
			}
		}

		public string AssoAcPDULength
		{
			get
			{
				return accPduLength;
			}
		}

		public ArrayList PDUList
		{
			get
			{
				return pduList;
			}
		}		

		/// <summary>
		/// This is the main routine called from DICOM Analyzer layer for handling each PDU
		/// </summary>
		/// <param name="p"></param>
		/// <param name="Position"></param>
		/// <param name="length"></param>
		/// <param name="direction"></param>
		/// <param name="state"></param>
		public void HandlePDU(byte[] p, uint Position, uint length, int direction, TCPState state)
		{
			byte PDUType = p[Position];
			uint Index = Position + 2;
			uint PDULength = DICOMUtility.Get4BytesBigEndian( p , ref Index);
			uint FinalPosition = Position + length +6 ;  // include length of PDU header
			StringBuilder s = new StringBuilder(500);
			System.DateTime dt1 = state.GetTime(direction,Position);
			System.DateTime dt2 = state.GetTime(direction,Position+length-1);

			int pduLength = (int)PDULength + 6;
			byte[] pduData = new byte[pduLength];
			Array.Copy(p,Position,pduData,0,pduLength);
			
			//Populate PDU list
			PDU_DETAIL pduDetail = new PDU_DETAIL();
			pduDetail.PduType = PDUType;
			pduDetail.PduDirection = direction;
			pduDetail.PduIndex = PDUNumber;
			pduDetail.PduData = pduData;
			pduDetail.PduLength = PDULength + 6; //include length of PDU header
            pduDetail.startTime = startTimeForAssoc;
            pduDetail.timeStamp = System.DateTime.Now;

            if(PDUType != 4)
			{	
				pduList.Add(pduDetail);
			}

            DumpPDU(p, Position, BaseFileName, direction, PDULength,PDUNumber++);

			s.Append(string.Format("{0} Type {1} PDU : Length = {2} : Direction = {3}\r\n",state.Signature[direction],PDUType,PDULength,(direction==0)?"OUT":"IN"));
			s.Append(string.Format("{0:dd-MMM-yy HH:mm:ss.ffffff} to {1:HH:mm:ss.ffffff}\r\n",dt1,dt2));

			bool send = true;

			switch(PDUType)
			{
				case 1:	
				case 2:	
				{
					isLastDatasetFragmentRecd = false;
					isDataSetRecd = false;
					HandleRequestPDU(s, p,Position+6, PDULength, direction,PDUType);
					PopulateAssociationInfo(PDUType);
					break;
				}
				case 3:
					s.Append("Association Rejection\r\n");
					break;

				case 4:
				{
					if(PDULength==6)
					{
						s.Append("Illegal empty PDV\r\n");					
					}

					isDataSetRecd = true;
					
					Position +=6;
					bool isCmdDataPDU = false;
					while(Position < FinalPosition)
					{
						uint PDVLen = DICOMUtility.Get4BytesBigEndian(p, ref Position);
						byte PCID = p[Position++];
						byte Flags = p[Position++];

						s.Append("  PDV Length=" + PDVLen.ToString() );
						s.Append("  PCID = " + PCID.ToString());
						s.Append("  Type = " + ((Flags & 0x01)>0?"COMMAND":"DATA"));
						s.Append((Flags & 0x02)>0?"  Last Fragment\r\n":"  Continues...\r\n");
						send = false;

						byte[] thisData = new byte[PDVLen-2];
						Array.Copy(p,Position,thisData,0,PDVLen-2);
						ByteData[direction].Add(thisData);
						
						//Add only Dataset data
						if((Flags & 0x01)== 0)
						{
							DatasetData.Add(thisData);											
						}
						
						if((Position + (PDVLen - 2)) == FinalPosition)
						{
							if((Flags & 0x01)== 0)
							{
								if(commandType != "")
								{
									if(isCmdDataPDU)
										pduDetail.CmdType = commandType + "[Command,Data]";
									else
										pduDetail.CmdType = commandType + "[Data]";
								}

								cmdPdusList.Add(pduDetail);
							}

							if((Flags & 0x02)>0)  // Last fragment
							{
								if((Flags & 0x01)>0)
								{
									s.Append(DICOMCommandDump(ByteData[direction]));
									if(commandType != "")
										pduDetail.CmdType = commandType + "[Command]";

									pduList.Add(pduDetail);									
								}
								else
								{
									foreach(PDU_DETAIL pdu in cmdPdusList)
									{
										pduDetail.CmdPdusList.Add(pdu);
									}
									pduDetail.CmdType = ((PDU_DETAIL)cmdPdusList[0]).CmdType;
									pduDetail.TransferSyntaxDataset = TransferSyntax[PCID];

									foreach(byte[] data in DatasetData)
									{
										pduDetail.ByteDataDump.Add(data);
									}
									pduList.Add(pduDetail);

									cmdPdusList.Clear();
									isLastDatasetFragmentRecd = true;
									s.Append(DICOMDump(ByteData[direction],TransferSyntax[PCID]));
								}

								ByteData[direction].Clear();
								DatasetData.Clear();
								send = true;
							}
						}
						else
						{
							//PDU contains both Command & Data PDV
							isCmdDataPDU = true;
							if((Flags & 0x01)>0)
							{
								s.Append(DICOMCommandDump(ByteData[direction]));													
							}
							else
							{
								s.Append(DICOMDump(ByteData[direction],TransferSyntax[PCID]));								
							}
						}
						Position += (PDVLen - 2);
					}
					break;
				}
				case 5:
				{
					//Check for incomplete byte stream
					if( isDataSetRecd && (!isLastDatasetFragmentRecd))
					{
						HandleIncompleteByteStream();
					}
					s.Append("Association Release Request\r\n");
					break;
				}
				case 6:
				{
					//Check for incomplete byte stream
					if(isDataSetRecd && (!isLastDatasetFragmentRecd))
					{
						HandleIncompleteByteStream();
					}
					s.Append("Association Release Acceptance\r\n");
					break;
				}
				case 7:
				{
					s.Append("Association Abort\r\n");
					break;
				}

				default:
					s.Append("Unknown PDU Type\r\n");
					break;
			}
			Out(s.ToString(),send);
			LastPDU = PDUType;
		}

		void HandleIncompleteByteStream()
		{
			//Populate PDU list
			PDU_DETAIL pduDetail = new PDU_DETAIL();

			foreach(PDU_DETAIL pdu in cmdPdusList)
			{
				pduDetail.CmdPdusList.Add(pdu);
			}

            if (cmdPdusList.Count != 0)
            {
			    pduDetail.PduType = ((PDU_DETAIL)cmdPdusList[0]).PduType;
			    pduDetail.PduDirection = ((PDU_DETAIL)cmdPdusList[0]).PduDirection;
			    pduDetail.CmdType = ((PDU_DETAIL)cmdPdusList[0]).CmdType;

			    pduDetail.TransferSyntaxDataset = TransferSyntax[1];

			    foreach(byte[] data in DatasetData)
			    {
				    pduDetail.ByteDataDump.Add(data);
			    }
			    pduList.Add(pduDetail);

			    cmdPdusList.Clear();
            }
			//s.Append(DICOMDump(ByteData[direction],TransferSyntax[PCID]));
		}

		/// <summary>
		/// Event handler for logging all the information
		/// </summary>
		/// <param name="s"></param>
		/// <param name="ToScreen"></param>
		public void Out(string s, bool ToScreen)
		{
			if(Output != null)
				Output(s,false,ToScreen);
			if(textLog != null)
			{
				textLog.Write(s);
				textLog.Flush();
			}
			else
				Output("Log closed prematurely: " + State.Signature[0],true,true);				
		}
	}

	public class PDU_DETAIL
	{
		public PDU_DETAIL(){}
		public byte PduType;
		public string CmdType; // This member will be used only in case of P-DATA PDU
		public ArrayList CmdPdusList = new ArrayList(); // This member will be used only in case of P-DATA PDU
		public int PduDirection;
		public int PduIndex;
		public uint PduLength;
		public byte [] PduData;
		public ArrayList ByteDataDump = new ArrayList();
		public string TransferSyntaxDataset;
        public System.DateTime startTime;
        public System.DateTime timeStamp;
	}	

	public class SOPClassMap 
	{
		void CreateSOPClassMap()
		{
			SOPClassNames = new Hashtable();
			SOPClassNames.Add("1.2.840.10008.1.1",				"Verification");
			SOPClassNames.Add("1.2.840.10008.1.3.10",			"Media Storage Directory Storage");
			SOPClassNames.Add("1.2.840.10008.1.9",				"Basic Study Content Notification");
			SOPClassNames.Add("1.2.840.10008.1.20.1",			"Storage Commitment Push Model");
			SOPClassNames.Add("1.2.840.10008.1.20.2",			"Storage Commitment Pull Model(Retired)");
			SOPClassNames.Add("1.2.840.10008.1.40",				"Procedural Event Logging");
			SOPClassNames.Add("1.2.840.10008.3.1.2.1.1" ,		"Detached Patient Management");
			SOPClassNames.Add("1.2.840.10008.3.1.2.1.4" ,		"Detached Patient Management Meta");
			SOPClassNames.Add("1.2.840.10008.3.1.2.2.1" ,		"Detached Visit Management");
			SOPClassNames.Add("1.2.840.10008.3.1.2.3.1" ,		"Detached Study Management");
			SOPClassNames.Add("1.2.840.10008.3.1.2.3.2",		"Study Component Management");
			SOPClassNames.Add("1.2.840.10008.3.1.2.3.3",		"Modality Performed Procedure Step");
			SOPClassNames.Add("1.2.840.10008.3.1.2.3.4",		"Modality Performed Procedure Step Retrieve");
			SOPClassNames.Add("1.2.840.10008.3.1.2.3.5",		"Modality Performed Procedure Step Notification");
			SOPClassNames.Add("1.2.840.10008.3.1.2.5.1",		"Detached Results Management");
			SOPClassNames.Add("1.2.840.10008.3.1.2.5.4",		"Detached Results Management Meta");
			SOPClassNames.Add("1.2.840.10008.3.1.2.5.5",		"Detached Study Management Meta");
			SOPClassNames.Add("1.2.840.10008.3.1.2.6.1",		"Detached Interpretation Management");
			SOPClassNames.Add("1.2.840.10008.5.1.1.1",			"Basic Film Session");
			SOPClassNames.Add("1.2.840.10008.5.1.1.2",			"Basic Film Box");
			SOPClassNames.Add("1.2.840.10008.5.1.1.4",			"Basic Grayscale Image Box");
			SOPClassNames.Add("1.2.840.10008.5.1.1.4.1",		"Basic Color Image Box");
			SOPClassNames.Add("1.2.840.10008.5.1.1.4.2",		"Referenced Image Box(Retired)");
			SOPClassNames.Add("1.2.840.10008.5.1.1.9",			"Basic Grayscale Print Management Meta");
			SOPClassNames.Add("1.2.840.10008.5.1.1.14",			"Print Job");
			SOPClassNames.Add("1.2.840.10008.5.1.1.15",			"Basic Annotation Box");
			SOPClassNames.Add("1.2.840.10008.5.1.1.16",			"Printer");
			SOPClassNames.Add("1.2.840.10008.5.1.1.16.376",		"Printer Configuration Retrieval");
			SOPClassNames.Add("1.2.840.10008.5.1.1.18",			"Basic Color Print Management Meta");
			SOPClassNames.Add("1.2.840.10008.5.1.1.18.1",		"Referenced Color Print");
			SOPClassNames.Add("1.2.840.10008.5.1.1.22",			"VOI LUT Box");
			SOPClassNames.Add("1.2.840.10008.5.1.1.23",			"Presentation LUT");
			SOPClassNames.Add("1.2.840.10008.5.1.1.24",			"Image Overlay Box(Retired)");
			SOPClassNames.Add("1.2.840.10008.5.1.1.24.1",		"Basic Print Image Overlay Box");
			SOPClassNames.Add("1.2.840.10008.5.1.1.26",			"Print Queue Management");
			SOPClassNames.Add("1.2.840.10008.5.1.1.27",			"Stored Print Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.1.29",			"Hardcopy Grayscale Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.1.30",			"Hardcopy Color Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.1.31",			"Pull Print Request");
			SOPClassNames.Add("1.2.840.10008.5.1.1.32",			"Pull Stored Print Management Meta");
			SOPClassNames.Add("1.2.840.10008.5.1.1.33",			"Media Creation Management");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.1",		"Computed Radiography Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.1.1",	"Digital XRay Image Storage For Presentation");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.1.1.1",	"Digital XRay Image Storage For Processing");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.1.2" ,	"Digital Mammography XRay Image Storage For Presentation");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.1.2.1",	"Digital Mammography XRay Image Storage For Processing");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.1.3",	"Digital Intraoral XRay Image Storage For Presentation");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.1.3.1",	"Digital Intraoral XRay Image Storage For Processing");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.2",		"CT Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.2.1",	"Enhanced CT Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.3.1",	"Ultrasound Multiframe Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.4",		"MR Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.4.1",	"Enhanced MR Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.4.2",	"MR Spectroscopy Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.6.1",	"Ultrasound Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.7",		"Secondary Capture Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.7.1",	"Multiframe Single Bit Secondary Capture Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.7.2",	"Multiframe Grayscale Byte Secondary Capture Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.7.3",	"Multiframe Grayscale Word Secondary Capture Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.7.4",	"Multiframe True Color Secondary Capture Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.8",		"Standalone Overlay Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.9",		"Standalone Curve Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.9.1.1",	"Twelve lead ECG Waveform Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.9.1.2",	"General ECG Waveform Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.9.1.3",	"Ambulatory ECG Waveform Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.9.2.1",	"Hemodynamic Waveform Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.9.3.1",	"Cardiac Electrophysiology Waveform Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.9.4.1",	"Basic Voice Audio Waveform Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.10"   ,	"Standalone Modality LUT Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.11"   ,	"Standalone VOI LUT Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.11.1" ,	"Grayscale Softcopy Presentation State Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.12.1" ,	"XRay Angiographic Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.12.2" ,	"XRay Radiofluoroscopic Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.12.3" ,	"XRay Angiographic Bi-Plane Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.20"   ,	"Nuclear Medicine Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.66"   ,	"Raw Data Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.66.1"  ,	"Spatial Registration Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.66.2"  ,	"Spatial Fiducials Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.1",	"VL Endoscopic Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.1.1","Video Endoscopic Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.2",	"VL Microscopic Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.2.1","Video Microscopic Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.3",	"VL Slide Coordinates Microscopic Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.4",	"VL Photographic Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.4.1","Video Photographic Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.5.1","Ophthalmic Photographic 8 Bit Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.5.2","Ophthalmic Photographic 16 Bit Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.77.1.5.3","Stereometric Relationship Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.88.11",	"Basic Text SR");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.88.22",	"Enhanced SR");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.88.33",	"Comprehensive SR");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.88.40",	"Procedure Log Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.88.50",	"Mammography CAD SR");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.88.59",	"Key Object Selection Document");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.88.65",	"Chest CAD SR");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.128",	"Positron Emission Tomography Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.129",	"Standalone PET Curve Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.481.1",	"RT Image Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.481.2",	"RT Dose Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.481.3",	"RT Structure Set Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.481.4",	"RT Beams Treatment Record Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.481.5",	"RT Plan Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.481.6",	"RT Brachy Treatment Record Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.1.481.7",	"RT Treatment Summary Record Storage");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.2.1.1",	"Patient Root Query Retrieve Information Model FIND");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.2.1.2",	"Patient Root Query Retrieve Information Model MOVE");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.2.1.3",	"Patient Root Query Retrieve Information Model GET");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.2.2.1",	"Study Root Query Retrieve Information Model FIND");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.2.2.2",	"Study Root Query Retrieve Information Model MOVE");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.2.2.3",	"Study Root Query Retrieve Information Model GET");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.2.3.1",	"Patient Study Only Query Retrieve Information Model FIND");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.2.3.2",	"Patient Study Only Query Retrieve Information Model MOVE");
			SOPClassNames.Add("1.2.840.10008.5.1.4.1.2.3.3",	"Patient Study Only Query Retrieve Information Model GET");
			SOPClassNames.Add("1.2.840.10008.5.1.4.31"     ,	"Modality Worklist Information Model FIND");
			SOPClassNames.Add("1.2.840.10008.5.1.4.32.1"   ,	"General Purpose Worklist Information Model FIND");
			SOPClassNames.Add("1.2.840.10008.5.1.4.32.2"   ,	"General Purpose Scheduled Procedure Step");
			SOPClassNames.Add("1.2.840.10008.5.1.4.32.3"   ,	"General Purpose Performed Procedure Step");
			SOPClassNames.Add("1.2.840.10008.5.1.4.32"     ,	"General Purpose Worklist Management Meta");
			SOPClassNames.Add("1.2.840.10008.5.1.4.33"     ,	"Instance Availability Notification");
			SOPClassNames.Add("1.2.840.10008.5.1.4.37.1"     ,	"General Relavent Patient Information Query");
			SOPClassNames.Add("1.2.840.10008.5.1.4.37.2"     ,	"Breast Imaging Relavent Patient Information Query");
			SOPClassNames.Add("1.2.840.10008.5.1.4.37.3"     ,	"Cardiac Relavent Patient Information Query");
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks>Required by Xml serialization.</remarks>
		public SOPClassMap()
		{
			CreateSOPClassMap();
		}

		/// <summary>
		/// Hashtable collection of predefined <see cref="AbstractSyntax"/> items.
		/// </summary>
		public Hashtable SOPClassNames;		
	}

	public class TransferSyntaxMap 
	{
		void CreateTransferSyntaxMap()
		{
			TransferSyntaxNames = new Hashtable();
			TransferSyntaxNames.Add("1.2.840.10008.1.2",			"Implicit VR Little Endian");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.1",			"Explicit VR Little Endian");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.1.99",		"Deflated Explicit VR Little Endian");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.2",			"Explicit VR Big Endian");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.50" ,		"JPEG Baseline Process 1");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.51" ,		"JPEG Extended Process 2 And 4");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.52" ,		"JPEG Extended Process 3 And 5");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.53" ,		"JPEG Spectral Selection Non Hierarchical 6 And 8");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.54",		"JPEG Spectral Selection Non Hierarchical 7 And 9");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.55",		"JPEG Full Progression Non Hierarchical 10 And 12");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.56",		"JPEG Full Progression Non Hierarchical 11 And 13");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.57",		"JPEG Lossless Non Hierarchical 14");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.58",		"JPEG Lossless Non Hierarchical 15");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.59",		"JPEG Extended Hierarchical 16 And 18");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.60",		"JPEG Extended Hierarchical 17 And 19");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.61",		"JPEG Spectral Selection Hierarchical 20 And 22");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.62",		"JPEG Spectral Selection Hierarchical 21 And 23");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.63",		"JPEG Full Progression Hierarchical 24 And 26");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.64",		"JPEG Full Progression Hierarchical 25 And 27");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.65",		"JPEG Lossless Hierarchical 28");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.66",		"JPEG Lossless Hierarchical 29");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.70",		"JPEG Lossless Non Hierarchical 1st Order Prediction");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.80",		"JPEG LS Lossless Image Compression");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.81",		"JPEG LS Lossy Image Compression");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.90",		"JPEG 2000 Image Compression Lossless Only");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.4.91",		"JPEG 2000 Image Compression");
			TransferSyntaxNames.Add("1.2.840.10008.1.2.5",			"RLE Lossless");
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks>Required by Xml serialization.</remarks>
		public TransferSyntaxMap()
		{
			CreateTransferSyntaxMap();
		}

		/// <summary>
		/// Hashtable collection of predefined <see cref="AbstractSyntax"/> items.
		/// </summary>
		public Hashtable TransferSyntaxNames;		
	}
}
