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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using DICOM;

namespace SnifferUI
{
	/// <summary>
	/// Summary description for ServiceElementPDUs.
	/// </summary>
	public class ServiceElementPDUs : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox cmdDataPduslistBox;
		private System.Windows.Forms.Label lablePDUs;
		private System.Windows.Forms.Label cmdNamelabel;
		private System.Windows.Forms.TextBox nametextBox;
		private ArrayList pduList = new ArrayList();
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ServiceElementPDUs(PDU_DETAIL pdu, DateTime time)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			nametextBox.Text = pdu.CmdType;
			this.Text = pdu.CmdType + " PDUs List";
			foreach(PDU_DETAIL dataPdu in pdu.CmdPdusList)
			{
                //TimeSpan ts = dataPdu.timeStamp - time;
                //string timeStampStr = string.Format("{0:000}.{1:000}", (ts.Minutes * 60 + ts.Seconds), ts.Milliseconds);
                int pduIndex = dataPdu.PduIndex + 1;
                string displayStr = "[" + pduIndex.ToString() + "]" + " P-DATA-TF";
				cmdDataPduslistBox.Items.Add(displayStr);
				pduList.Add(dataPdu);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ServiceElementPDUs));
			this.cmdDataPduslistBox = new System.Windows.Forms.ListBox();
			this.lablePDUs = new System.Windows.Forms.Label();
			this.cmdNamelabel = new System.Windows.Forms.Label();
			this.nametextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cmdDataPduslistBox
			// 
			this.cmdDataPduslistBox.Location = new System.Drawing.Point(0, 64);
			this.cmdDataPduslistBox.Name = "cmdDataPduslistBox";
			this.cmdDataPduslistBox.Size = new System.Drawing.Size(328, 251);
			this.cmdDataPduslistBox.TabIndex = 0;
			this.cmdDataPduslistBox.SelectedIndexChanged += new System.EventHandler(this.cmdDataPduslistBox_SelectedIndexChanged);
			// 
			// lablePDUs
			// 
			this.lablePDUs.Location = new System.Drawing.Point(8, 40);
			this.lablePDUs.Name = "lablePDUs";
			this.lablePDUs.Size = new System.Drawing.Size(48, 16);
			this.lablePDUs.TabIndex = 1;
			this.lablePDUs.Text = "PDUs:";
			// 
			// cmdNamelabel
			// 
			this.cmdNamelabel.Location = new System.Drawing.Point(0, 8);
			this.cmdNamelabel.Name = "cmdNamelabel";
			this.cmdNamelabel.TabIndex = 2;
			this.cmdNamelabel.Text = "Command Name:";
			// 
			// nametextBox
			// 
			this.nametextBox.Location = new System.Drawing.Point(104, 8);
			this.nametextBox.Name = "nametextBox";
			this.nametextBox.Size = new System.Drawing.Size(152, 20);
			this.nametextBox.TabIndex = 3;
			this.nametextBox.Text = "";
			// 
			// ServiceElementPDUs
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(330, 320);
			this.Controls.Add(this.nametextBox);
			this.Controls.Add(this.cmdNamelabel);
			this.Controls.Add(this.lablePDUs);
			this.Controls.Add(this.cmdDataPduslistBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ServiceElementPDUs";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PDUs List";
			this.ResumeLayout(false);

		}
		#endregion

		private void cmdDataPduslistBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PDU_DETAIL pdu = (PDU_DETAIL)pduList[cmdDataPduslistBox.SelectedIndex];
			string pduStr = DumpDataPDU(pdu.PduData, 6, pdu.PduLength);
			PDUOverview pduOverview = new PDUOverview(pduStr, "P-DATA-TF", pdu.PduLength.ToString());
			pduOverview.ShowDialog();
		}

		private string DumpDataPDU(byte[] p, uint Position, uint length)
		{
			StringBuilder pDataPduDetail = new StringBuilder(1024);
			
			pDataPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t" + "4H" + "\r\n");
			pDataPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			pDataPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n\n", length.ToString()));
			
			while(Position < length)
			{
				uint PDVLen = DICOMUtility.Get4BytesBigEndian(p, ref Position);
				byte PCID = p[Position++];
				byte Flags = p[Position++];

                byte[] byteData = new byte[PDVLen-2];
                Array.Copy(p, Position, byteData, 0, PDVLen - 2);

				string pdvType = (Flags & 0x01)>0?"Command PDV(Type 1)":"Data PDV(Type 0)";
				string pdvState = (Flags & 0x02)>0?"  Last Fragment":"  Data Continues...";

				pDataPduDetail.Append(string.Format("\t\t" + "  {0}  " + "\r\n\n", pdvType));
				pDataPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "PDV Length" + "\t\t" + "{2}\r\n", (Position-6+1),(Position-6+4),PDVLen));
				pDataPduDetail.Append(string.Format("  {0}" + "\t\t" + "Presentation Context ID" + "\t" + "{1}\r\n",(Position-6+5),PCID));
				pDataPduDetail.Append(string.Format("  {0}" + "\t\t" + "Message Control Header" + "\t" + "{1:X2}H\r\n",(Position-6+6),Flags));
				pDataPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Data:" + "\t" + "\r\n\n",(Position-6+7),(Position-6+PDVLen+4)));
                pDataPduDetail.Append(DICOMUtility.GetHexString(byteData, Position));
				pDataPduDetail.Append(string.Format("\t\t" + "  {0}  " + "\r\n\n", pdvState));
				Position += (PDVLen - 2);
			}
			
			return pDataPduDetail.ToString();
		}        
	}
}
