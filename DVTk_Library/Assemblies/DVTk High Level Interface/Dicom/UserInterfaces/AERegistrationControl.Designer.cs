namespace DvtkHighLevelInterface.Dicom.UserInterfaces
{
    partial class AERegistrationControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.verifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dICOMEchoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.newPeerGroup = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.peerPort = new System.Windows.Forms.MaskedTextBox();
            this.peerAE = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.peerIp = new System.Windows.Forms.TextBox();
            this.peerName = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.registeredPeerGroup = new System.Windows.Forms.GroupBox();
            this.registeredPeersGrid = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.newPeerGroup.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.registeredPeerGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verifyToolStripMenuItem,
            this.dICOMEchoToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 114);
            // 
            // verifyToolStripMenuItem
            // 
            this.verifyToolStripMenuItem.Name = "verifyToolStripMenuItem";
            this.verifyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.verifyToolStripMenuItem.Text = "Ping Destination";
            this.verifyToolStripMenuItem.Click += new System.EventHandler(this.pingToolStripMenuItem_Click);
            // 
            // dICOMEchoToolStripMenuItem
            // 
            this.dICOMEchoToolStripMenuItem.Name = "dICOMEchoToolStripMenuItem";
            this.dICOMEchoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dICOMEchoToolStripMenuItem.Text = "DICOM Echo";
            this.dICOMEchoToolStripMenuItem.Click += new System.EventHandler(this.dICOMEchoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Edit";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.OnEditClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "Remove";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.OnRemove);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.newPeerGroup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(501, 135);
            this.panel1.TabIndex = 3;
            // 
            // newPeerGroup
            // 
            this.newPeerGroup.Controls.Add(this.panel4);
            this.newPeerGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.newPeerGroup.Location = new System.Drawing.Point(0, 0);
            this.newPeerGroup.Name = "newPeerGroup";
            this.newPeerGroup.Size = new System.Drawing.Size(492, 135);
            this.newPeerGroup.TabIndex = 2;
            this.newPeerGroup.TabStop = false;
            this.newPeerGroup.Text = "New Peer";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(3, 16);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel4.Size = new System.Drawing.Size(448, 116);
            this.panel4.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(444, 62);
            this.panel2.TabIndex = 8;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.peerPort);
            this.panel6.Controls.Add(this.peerAE);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(249, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(147, 62);
            this.panel6.TabIndex = 30;
            // 
            // peerPort
            // 
            this.peerPort.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.peerPort.HidePromptOnLeave = true;
            this.peerPort.Location = new System.Drawing.Point(0, 42);
            this.peerPort.Mask = "00000";
            this.peerPort.Name = "peerPort";
            this.peerPort.PromptChar = ' ';
            this.peerPort.Size = new System.Drawing.Size(147, 20);
            this.peerPort.TabIndex = 19;
            // 
            // peerAE
            // 
            this.peerAE.Dock = System.Windows.Forms.DockStyle.Top;
            this.peerAE.Location = new System.Drawing.Point(0, 0);
            this.peerAE.Name = "peerAE";
            this.peerAE.Size = new System.Drawing.Size(147, 20);
            this.peerAE.TabIndex = 17;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(187, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.panel3.Size = new System.Drawing.Size(62, 62);
            this.panel3.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(10, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "AE Title";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.peerIp);
            this.panel5.Controls.Add(this.peerName);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(72, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(115, 62);
            this.panel5.TabIndex = 28;
            // 
            // peerIp
            // 
            this.peerIp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.peerIp.Location = new System.Drawing.Point(0, 42);
            this.peerIp.Name = "peerIp";
            this.peerIp.Size = new System.Drawing.Size(115, 20);
            this.peerIp.TabIndex = 15;
            // 
            // peerName
            // 
            this.peerName.Dock = System.Windows.Forms.DockStyle.Top;
            this.peerName.Location = new System.Drawing.Point(0, 0);
            this.peerName.Name = "peerName";
            this.peerName.Size = new System.Drawing.Size(115, 20);
            this.peerName.TabIndex = 14;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(72, 62);
            this.panel7.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(0, 49);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "IP Address";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 25, 0);
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Name";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.addButton);
            this.panel8.Controls.Add(this.cancelButton);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 62);
            this.panel8.Name = "panel8";
            this.panel8.Padding = new System.Windows.Forms.Padding(0, 20, 8, 0);
            this.panel8.Size = new System.Drawing.Size(448, 44);
            this.panel8.TabIndex = 7;
            // 
            // addButton
            // 
            this.addButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.addButton.Location = new System.Drawing.Point(290, 20);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 24);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.cancelButton.Location = new System.Drawing.Point(365, 20);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 24);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.OnCancel);
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.registeredPeerGroup);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(0, 135);
            this.panel9.Margin = new System.Windows.Forms.Padding(2);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(492, 246);
            this.panel9.TabIndex = 4;
            // 
            // registeredPeerGroup
            // 
            this.registeredPeerGroup.Controls.Add(this.registeredPeersGrid);
            this.registeredPeerGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.registeredPeerGroup.Location = new System.Drawing.Point(0, 0);
            this.registeredPeerGroup.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.registeredPeerGroup.Name = "registeredPeerGroup";
            this.registeredPeerGroup.Size = new System.Drawing.Size(492, 231);
            this.registeredPeerGroup.TabIndex = 3;
            this.registeredPeerGroup.TabStop = false;
            this.registeredPeerGroup.Text = "Registered Peers";
            // 
            // registeredPeersGrid
            // 
            this.registeredPeersGrid.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.registeredPeersGrid.ContextMenuStrip = this.contextMenuStrip1;
            this.registeredPeersGrid.FullRowSelect = true;
            this.registeredPeersGrid.Location = new System.Drawing.Point(9, 19);
            this.registeredPeersGrid.MultiSelect = false;
            this.registeredPeersGrid.Name = "registeredPeersGrid";
            this.registeredPeersGrid.Size = new System.Drawing.Size(472, 206);
            this.registeredPeersGrid.TabIndex = 0;
            this.registeredPeersGrid.UseCompatibleStateImageBehavior = false;
            this.registeredPeersGrid.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "AE Title";
            this.columnHeader2.Width = 215;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "IP Address";
            this.columnHeader3.Width = 105;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Port";
            this.columnHeader4.Width = 50;
            // 
            // AERegistrationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel1);
            this.Name = "AERegistrationControl";
            this.Size = new System.Drawing.Size(501, 381);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.newPeerGroup.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.registeredPeerGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox newPeerGroup;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.MaskedTextBox peerPort;
        private System.Windows.Forms.TextBox peerAE;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox peerIp;
        private System.Windows.Forms.TextBox peerName;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.GroupBox registeredPeerGroup;
        private System.Windows.Forms.ListView registeredPeersGrid;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripMenuItem verifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dICOMEchoToolStripMenuItem;
    }
}
