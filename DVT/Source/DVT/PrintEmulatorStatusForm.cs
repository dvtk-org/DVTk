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

namespace Dvt
{
	/// <summary>
	/// Summary description for PrintEmulatorStatusForm.
	/// </summary>
	public class PrintEmulatorStatusForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TextBoxPrinterName;
        private System.Windows.Forms.Button ButtonUpdate;
        private System.Windows.Forms.ComboBox ComboBoxPrinterStatusInfo;
        private System.Windows.Forms.ComboBox ComboBoxPrinterStatus;
        private System.Windows.Forms.DateTimePicker DateTimeCalibrationTime;
        private System.Windows.Forms.DateTimePicker DateTimeCalibrationDate;
        private System.Windows.Forms.TextBox TextBoxManufacturer;
        private System.Windows.Forms.TextBox TextBoxModelName;
        private System.Windows.Forms.TextBox TextBoxSerialNumber;
        private System.Windows.Forms.TextBox TextBoxSoftwareVersions;
		private System.Windows.Forms.Button Ok;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PrintEmulatorStatusForm(DvtkApplicationLayer.EmulatorSession emulator_session)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.session = emulator_session;

            this.TextBoxManufacturer.Text = this.session.EmulatorSessionImplementation.Printer.Manufacturer;
            this.TextBoxModelName.Text = this.session.EmulatorSessionImplementation.Printer.ManufacturerModelName;
            this.TextBoxPrinterName.Text = this.session.EmulatorSessionImplementation.Printer.PrinterName;
            this.TextBoxSerialNumber.Text = this.session.EmulatorSessionImplementation.Printer.DeviceSerialNumber;
            this.TextBoxSoftwareVersions.Text = this.session.EmulatorSessionImplementation.Printer.SoftwareVersions;
            this.DateTimeCalibrationDate.Value = this.session.EmulatorSessionImplementation.Printer.DateOfLastCalibration.Date;
            this.DateTimeCalibrationTime.Value = this.session.EmulatorSessionImplementation.Printer.TimeOfLastCalibration.ToLocalTime ();

            // add the 3 possible Printer Status values
            this.ComboBoxPrinterStatus.Items.Add("NORMAL");
            this.ComboBoxPrinterStatus.Items.Add("WARNING");
            this.ComboBoxPrinterStatus.Items.Add("FAILURE");
            string status = this.session.EmulatorSessionImplementation.Printer.Status.ToString();
            foreach (object o in this.ComboBoxPrinterStatus.Items)
            {
                if (o.ToString() == status)
                {
                    this.ComboBoxPrinterStatus.SelectedItem = o;
                    break;
                }
            }

            string statusInfo = this.session.EmulatorSessionImplementation.Printer.StatusInfo.ToString();
            foreach (string info_dt in this.session.EmulatorSessionImplementation.Printer.StatusInfoDefinedTerms)
            {
                this.ComboBoxPrinterStatusInfo.Items.Add (info_dt);
            }

            foreach (object o in this.ComboBoxPrinterStatusInfo.Items)
            {
                if (o.ToString() == statusInfo)
                {
                    this.ComboBoxPrinterStatusInfo.SelectedItem = o;
                    break;
                }
            }
        }

        private DvtkApplicationLayer.EmulatorSession session;

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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ComboBoxPrinterStatusInfo = new System.Windows.Forms.ComboBox();
            this.DateTimeCalibrationTime = new System.Windows.Forms.DateTimePicker();
            this.DateTimeCalibrationDate = new System.Windows.Forms.DateTimePicker();
            this.ComboBoxPrinterStatus = new System.Windows.Forms.ComboBox();
            this.TextBoxPrinterName = new System.Windows.Forms.TextBox();
            this.TextBoxManufacturer = new System.Windows.Forms.TextBox();
            this.TextBoxModelName = new System.Windows.Forms.TextBox();
            this.TextBoxSerialNumber = new System.Windows.Forms.TextBox();
            this.TextBoxSoftwareVersions = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ButtonUpdate = new System.Windows.Forms.Button();
            this.Ok = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Printer Name:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Manufacturer:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 23);
            this.label6.TabIndex = 0;
            this.label6.Text = "Model Name:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 104);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 23);
            this.label7.TabIndex = 0;
            this.label7.Text = "Serial Number:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 23);
            this.label8.TabIndex = 0;
            this.label8.Text = "Printer Status:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(305, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Calibration Date:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(305, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "Software Version(s):";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(303, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 23);
            this.label9.TabIndex = 0;
            this.label9.Text = "Printer Status Info:";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(305, 72);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 23);
            this.label10.TabIndex = 0;
            this.label10.Text = "Calibration Time:";
            // 
            // ComboBoxPrinterStatusInfo
            // 
            this.ComboBoxPrinterStatusInfo.Enabled = false;
            this.ComboBoxPrinterStatusInfo.Location = new System.Drawing.Point(419, 117);
            this.ComboBoxPrinterStatusInfo.Name = "ComboBoxPrinterStatusInfo";
            this.ComboBoxPrinterStatusInfo.Size = new System.Drawing.Size(176, 21);
            this.ComboBoxPrinterStatusInfo.TabIndex = 1;
            // 
            // DateTimeCalibrationTime
            // 
            this.DateTimeCalibrationTime.Enabled = false;
            this.DateTimeCalibrationTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DateTimeCalibrationTime.Location = new System.Drawing.Point(427, 80);
            this.DateTimeCalibrationTime.Name = "DateTimeCalibrationTime";
            this.DateTimeCalibrationTime.Size = new System.Drawing.Size(176, 20);
            this.DateTimeCalibrationTime.TabIndex = 6;
            // 
            // DateTimeCalibrationDate
            // 
            this.DateTimeCalibrationDate.Enabled = false;
            this.DateTimeCalibrationDate.Location = new System.Drawing.Point(427, 56);
            this.DateTimeCalibrationDate.Name = "DateTimeCalibrationDate";
            this.DateTimeCalibrationDate.Size = new System.Drawing.Size(176, 20);
            this.DateTimeCalibrationDate.TabIndex = 5;
            // 
            // ComboBoxPrinterStatus
            // 
            this.ComboBoxPrinterStatus.Location = new System.Drawing.Point(89, 120);
            this.ComboBoxPrinterStatus.Name = "ComboBoxPrinterStatus";
            this.ComboBoxPrinterStatus.Size = new System.Drawing.Size(200, 21);
            this.ComboBoxPrinterStatus.TabIndex = 0;
            this.ComboBoxPrinterStatus.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPrinterStatus_SelectedIndexChanged);
            // 
            // TextBoxPrinterName
            // 
            this.TextBoxPrinterName.Enabled = false;
            this.TextBoxPrinterName.Location = new System.Drawing.Point(97, 32);
            this.TextBoxPrinterName.Name = "TextBoxPrinterName";
            this.TextBoxPrinterName.Size = new System.Drawing.Size(200, 20);
            this.TextBoxPrinterName.TabIndex = 0;
            // 
            // TextBoxManufacturer
            // 
            this.TextBoxManufacturer.Enabled = false;
            this.TextBoxManufacturer.Location = new System.Drawing.Point(97, 56);
            this.TextBoxManufacturer.Name = "TextBoxManufacturer";
            this.TextBoxManufacturer.Size = new System.Drawing.Size(200, 20);
            this.TextBoxManufacturer.TabIndex = 1;
            // 
            // TextBoxModelName
            // 
            this.TextBoxModelName.Enabled = false;
            this.TextBoxModelName.Location = new System.Drawing.Point(97, 80);
            this.TextBoxModelName.Name = "TextBoxModelName";
            this.TextBoxModelName.Size = new System.Drawing.Size(200, 20);
            this.TextBoxModelName.TabIndex = 2;
            // 
            // TextBoxSerialNumber
            // 
            this.TextBoxSerialNumber.Enabled = false;
            this.TextBoxSerialNumber.Location = new System.Drawing.Point(97, 104);
            this.TextBoxSerialNumber.Name = "TextBoxSerialNumber";
            this.TextBoxSerialNumber.Size = new System.Drawing.Size(200, 20);
            this.TextBoxSerialNumber.TabIndex = 3;
            // 
            // TextBoxSoftwareVersions
            // 
            this.TextBoxSoftwareVersions.Enabled = false;
            this.TextBoxSoftwareVersions.Location = new System.Drawing.Point(426, 32);
            this.TextBoxSoftwareVersions.Name = "TextBoxSoftwareVersions";
            this.TextBoxSoftwareVersions.Size = new System.Drawing.Size(177, 20);
            this.TextBoxSoftwareVersions.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ComboBoxPrinterStatus);
            this.groupBox1.Controls.Add(this.ComboBoxPrinterStatusInfo);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(608, 152);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Printer";
            // 
            // ButtonUpdate
            // 
            this.ButtonUpdate.Location = new System.Drawing.Point(420, 168);
            this.ButtonUpdate.Name = "ButtonUpdate";
            this.ButtonUpdate.Size = new System.Drawing.Size(112, 23);
            this.ButtonUpdate.TabIndex = 12;
            this.ButtonUpdate.Text = "Send Status Event";
            this.ButtonUpdate.Click += new System.EventHandler(this.ButtonUpdate_Click);
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(552, 168);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(61, 23);
            this.Ok.TabIndex = 11;
            this.Ok.Text = "OK";
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // PrintEmulatorStatusForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(624, 200);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.TextBoxPrinterName);
            this.Controls.Add(this.DateTimeCalibrationTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.DateTimeCalibrationDate);
            this.Controls.Add(this.TextBoxManufacturer);
            this.Controls.Add(this.TextBoxModelName);
            this.Controls.Add(this.TextBoxSerialNumber);
            this.Controls.Add(this.TextBoxSoftwareVersions);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ButtonUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintEmulatorStatusForm";
            this.ShowInTaskbar = false;
            this.Text = "Print Emulator Status";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void ComboBoxPrinterStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (
                (this.ComboBoxPrinterStatus.SelectedItem.ToString() == "NORMAL") ||
                (this.session.EmulatorSessionImplementation.Printer.StatusInfoDefinedTerms.Length == 0)
                )
            {
                this.ComboBoxPrinterStatusInfo.Enabled = false;
                this.ComboBoxPrinterStatusInfo.SelectedItem = null;
            }
            else
                this.ComboBoxPrinterStatusInfo.Enabled = true;
        }

        private void ButtonUpdate_Click(object sender, System.EventArgs e)
        {
            string selectedPrinterStatusInfo;
            if (!this.ComboBoxPrinterStatusInfo.Enabled)
            {
                selectedPrinterStatusInfo = string.Empty;
            }
            else
            {
                if (this.ComboBoxPrinterStatusInfo.SelectedItem != null)
                {
                    selectedPrinterStatusInfo =
                        this.ComboBoxPrinterStatusInfo.SelectedItem.ToString();
                }
                else 
                    selectedPrinterStatusInfo = string.Empty;
            }
            switch (this.ComboBoxPrinterStatus.SelectedIndex)
            {
                case 0:
                    this.session.EmulatorSessionImplementation.Printer.ApplyStatus (Dvtk.Sessions.PrinterStatus.NORMAL, selectedPrinterStatusInfo, true);
                    break;
                case 1:
                    this.session.EmulatorSessionImplementation.Printer.ApplyStatus (Dvtk.Sessions.PrinterStatus.WARNING, selectedPrinterStatusInfo, true);
                    break;
                case 2:
                    this.session.EmulatorSessionImplementation.Printer.ApplyStatus (Dvtk.Sessions.PrinterStatus.FAILURE, selectedPrinterStatusInfo, true);
                    break;
                default:
                    break;
            }
            this.Close();
        }

		private void Ok_Click(object sender, System.EventArgs e)
		{
			string selectedPrinterStatusInfo;
			if (!this.ComboBoxPrinterStatusInfo.Enabled)
			{
				selectedPrinterStatusInfo = string.Empty;
			}
			else
			{
				if (this.ComboBoxPrinterStatusInfo.SelectedItem != null)
				{
					selectedPrinterStatusInfo =
						this.ComboBoxPrinterStatusInfo.SelectedItem.ToString();
				}
				else 
                    selectedPrinterStatusInfo = string.Empty;
			}
			switch (this.ComboBoxPrinterStatus.SelectedIndex)
			{
				case 0:
					this.session.EmulatorSessionImplementation.Printer.ApplyStatus (Dvtk.Sessions.PrinterStatus.NORMAL, selectedPrinterStatusInfo, false);
					break;
				case 1:
                    this.session.EmulatorSessionImplementation.Printer.ApplyStatus(Dvtk.Sessions.PrinterStatus.WARNING, selectedPrinterStatusInfo, false);
					break;
				case 2:
                    this.session.EmulatorSessionImplementation.Printer.ApplyStatus(Dvtk.Sessions.PrinterStatus.FAILURE, selectedPrinterStatusInfo, false);
					break;
				default:
					break;
			}
			this.Close();
		}
	}
}
