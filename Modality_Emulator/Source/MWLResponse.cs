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
using Dvtk.IheActors.Dicom;
using DvtkHighLevelInterface.Dicom.Files;
using DvtkHighLevelInterface.Dicom.Messages;

namespace ModalityEmulator
{
	using HLI = DvtkHighLevelInterface.Dicom.Other;

	/// <summary>
	/// Summary description for MWLResponse.
	/// </summary>
	public class MWLResponse : System.Windows.Forms.Form
	{		
		private DicomQueryItemCollection queryRspItems = null;
		DicomQueryItem dummyPatientMWLItem = null;
		DicomQueryItem selectedModalityWorklistItem = null;
		private ArrayList wlmQueryRspItems = new ArrayList();
		private System.Windows.Forms.ListView listViewMWLRsp;
		private System.Windows.Forms.ColumnHeader columnPatName;
		private System.Windows.Forms.ColumnHeader columnPatID;
		private System.Windows.Forms.ColumnHeader columnAccNr;
		private System.Windows.Forms.ColumnHeader columnReqProcID;
		private System.Windows.Forms.ColumnHeader columnSchProcDt;
		private System.Windows.Forms.ColumnHeader columnModality;
		private System.Windows.Forms.ColumnHeader columnSchStnAETitle;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MWLResponse(DicomQueryItemCollection mwlRspItems)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			if(mwlRspItems.Count != 0)
				queryRspItems = mwlRspItems;

			PreparePatientList();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MWLResponse));
            this.listViewMWLRsp = new System.Windows.Forms.ListView();
            this.columnPatName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPatID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAccNr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnReqProcID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSchProcDt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnModality = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSchStnAETitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listViewMWLRsp
            // 
            this.listViewMWLRsp.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnPatName,
            this.columnPatID,
            this.columnAccNr,
            this.columnReqProcID,
            this.columnSchProcDt,
            this.columnModality,
            this.columnSchStnAETitle});
            this.listViewMWLRsp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMWLRsp.FullRowSelect = true;
            this.listViewMWLRsp.GridLines = true;
            this.listViewMWLRsp.Location = new System.Drawing.Point(0, 0);
            this.listViewMWLRsp.MultiSelect = false;
            this.listViewMWLRsp.Name = "listViewMWLRsp";
            this.listViewMWLRsp.Size = new System.Drawing.Size(730, 352);
            this.listViewMWLRsp.TabIndex = 9;
            this.listViewMWLRsp.UseCompatibleStateImageBehavior = false;
            this.listViewMWLRsp.View = System.Windows.Forms.View.Details;
            this.listViewMWLRsp.SelectedIndexChanged += new System.EventHandler(this.listViewMWLRsp_SelectedIndexChanged);
            // 
            // columnPatName
            // 
            this.columnPatName.Text = "Patient Name";
            this.columnPatName.Width = 93;
            // 
            // columnPatID
            // 
            this.columnPatID.Text = "Patient ID";
            this.columnPatID.Width = 74;
            // 
            // columnAccNr
            // 
            this.columnAccNr.Text = "Accession Nr";
            this.columnAccNr.Width = 82;
            // 
            // columnReqProcID
            // 
            this.columnReqProcID.Text = "Requested Proc ID";
            this.columnReqProcID.Width = 103;
            // 
            // columnSchProcDt
            // 
            this.columnSchProcDt.Text = "Scheduled Proc Step Start Date";
            this.columnSchProcDt.Width = 165;
            // 
            // columnModality
            // 
            this.columnModality.Text = "Modality";
            // 
            // columnSchStnAETitle
            // 
            this.columnSchStnAETitle.Text = "Scheduled Station AE Title";
            this.columnSchStnAETitle.Width = 200;
            // 
            // MWLResponse
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(730, 352);
            this.Controls.Add(this.listViewMWLRsp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MWLResponse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Select MWL Response";
            this.TopMost = true;
            this.ResumeLayout(false);

		}
		#endregion

		private void PreparePatientList()
		{
			//Read the dataset of dummy patient and add this as default WLM Response
			DicomFile dcmFile = new DicomFile();
            string definitionDir = Environment.GetEnvironmentVariable("COMMONPROGRAMFILES") + @"\DVTk\Definition Files\DICOM\";
			string defFile = definitionDir + "Allotherattributes.def";
			dcmFile.Read(Application.StartupPath + @"\data\worklist\WLM RSP\DummyPatient.dcm",defFile);
			DicomMessage dummyPatient = new DicomMessage(DvtkData.Dimse.DimseCommand.CFINDRSP);
			dummyPatient.Set("0x00000002", DvtkData.Dimse.VR.UI, "1.2.840.10008.5.1.4.31");
			dummyPatient.DataSet.DvtkDataDataSet = dcmFile.DataSet.DvtkDataDataSet;

			dummyPatientMWLItem = new DicomQueryItem(-1,dummyPatient.Clone());
			wlmQueryRspItems.Add(dummyPatientMWLItem);

			//Add all received WLM Query response items
			if((queryRspItems != null) && (queryRspItems.Count > 0))
			{
				foreach(DicomQueryItem mwlItem in queryRspItems)
				{
					wlmQueryRspItems.Add(mwlItem);
				}
			}
            else
                this.Text = "Please Select Dummy patient";

			//Update the patient listbox for display
			foreach(DicomQueryItem mwlItem in wlmQueryRspItems)
			{
				HLI.Attribute schAETitleAttr = null;
                if (mwlItem.DicomMessage.DataSet.Exists("0x00400100[1]/0x00400001"))
                {
                    schAETitleAttr = mwlItem.DicomMessage.DataSet["0x00400100[1]/0x00400001"];
                    if (schAETitleAttr.Values.Count > 1)
                    {
                        ListViewItem emptyItem = new ListViewItem("");
                        emptyItem.SubItems.Add("");
                        emptyItem.SubItems.Add("");
                        emptyItem.SubItems.Add("");
                        emptyItem.SubItems.Add("");
                        emptyItem.SubItems.Add("");
                        emptyItem.SubItems.Add("");
                        listViewMWLRsp.Items.Add(emptyItem);
                        continue;
                    }
                }

                HLI.Attribute patientNameAttr = null;
                if (mwlItem.DicomMessage.DataSet.Exists("0x00100010"))
                {
                    patientNameAttr = mwlItem.DicomMessage.DataSet["0x00100010"];
                }

                ListViewItem item = new ListViewItem(patientNameAttr.Values[0]);

				if(mwlItem.DicomMessage.DataSet.Exists("0x00100020"))
				{
					HLI.Attribute patientIDAttr   =  mwlItem.DicomMessage.DataSet["0x00100020"];
					item.SubItems.Add(patientIDAttr.Values[0]);
				}

				if(mwlItem.DicomMessage.DataSet.Exists("0x00080050"))
				{
					HLI.Attribute accNrAttr   =  mwlItem.DicomMessage.DataSet["0x00080050"];
					item.SubItems.Add(accNrAttr.Values[0]);
				}

				if(mwlItem.DicomMessage.DataSet.Exists("0x00401001"))
				{
					HLI.Attribute reqProcIDAttr   =  mwlItem.DicomMessage.DataSet["0x00401001"];
					item.SubItems.Add(reqProcIDAttr.Values[0]);
				}

                if (mwlItem.DicomMessage.DataSet.Exists("0x00400100[1]/0x00400002"))
				{
                    HLI.Attribute schProcAttr = mwlItem.DicomMessage.DataSet["0x00400100[1]/0x00400002"];
					item.SubItems.Add(schProcAttr.Values[0]);
				}

                if (mwlItem.DicomMessage.DataSet.Exists("0x00400100[1]/0x00080060"))
				{
                    HLI.Attribute modalityAttr = mwlItem.DicomMessage.DataSet["0x00400100[1]/0x00080060"];
					item.SubItems.Add(modalityAttr.Values[0]);
				}

                if (mwlItem.DicomMessage.DataSet.Exists("0x00400100[1]/0x00400001"))
				{
                    HLI.Attribute schStationAETitleAttr = mwlItem.DicomMessage.DataSet["0x00400100[1]/0x00400001"];
					item.SubItems.Add(schStationAETitleAttr.Values[0]);
				}

				listViewMWLRsp.Items.Add(item);				
			}
		}		

		/// <summary>
		/// Patient(WLM Response Item) selected by user
		/// </summary>
		/// <returns></returns>
		public DicomQueryItem SelectedPatient
		{
			get
			{
				return selectedModalityWorklistItem;
			}
		}

		private void listViewMWLRsp_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(listViewMWLRsp.SelectedIndices[0] != -1)
				selectedModalityWorklistItem = (DicomQueryItem)wlmQueryRspItems[listViewMWLRsp.SelectedIndices[0]];
			else
				selectedModalityWorklistItem = dummyPatientMWLItem;

            this.Close();
            this.DialogResult = DialogResult.OK;
		}
	}
}
