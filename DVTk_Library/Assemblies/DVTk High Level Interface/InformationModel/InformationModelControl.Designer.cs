using System.Windows.Forms;
namespace DvtkHighLevelInterface.InformationModel
{
    partial class InformationModelControl: UserControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.patientStudyselection = new System.Windows.Forms.RadioButton();
            this.studySelection = new System.Windows.Forms.RadioButton();
            this.patientSelection = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.imageGroup = new System.Windows.Forms.Panel();
            this.imageLabel = new System.Windows.Forms.Label();
            this.imageTable = new System.Windows.Forms.ListView();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.seriesGroup = new System.Windows.Forms.Panel();
            this.seriesLabel = new System.Windows.Forms.Label();
            this.seriesTable = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.studyGroup = new System.Windows.Forms.Panel();
            this.studyLabel = new System.Windows.Forms.Label();
            this.studyTable = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.patientGroup = new System.Windows.Forms.Panel();
            this.patientLabel = new System.Windows.Forms.Label();
            this.patientTable = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.panel7 = new System.Windows.Forms.Panel();
            this.attribTable = new System.Windows.Forms.ListView();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.imageGroup.SuspendLayout();
            this.seriesGroup.SuspendLayout();
            this.studyGroup.SuspendLayout();
            this.patientGroup.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(992, 47);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.patientStudyselection);
            this.groupBox1.Controls.Add(this.studySelection);
            this.groupBox1.Controls.Add(this.patientSelection);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(319, 37);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information Model";
            // 
            // patientStudyselection
            // 
            this.patientStudyselection.AutoSize = true;
            this.patientStudyselection.Dock = System.Windows.Forms.DockStyle.Left;
            this.patientStudyselection.Location = new System.Drawing.Point(165, 16);
            this.patientStudyselection.Name = "patientStudyselection";
            this.patientStudyselection.Size = new System.Drawing.Size(140, 18);
            this.patientStudyselection.TabIndex = 2;
            this.patientStudyselection.TabStop = true;
            this.patientStudyselection.Text = "Patient/Study Only Root";
            this.patientStudyselection.UseVisualStyleBackColor = true;
            this.patientStudyselection.CheckedChanged += new System.EventHandler(this.patientStudyselection_CheckedChanged);
            // 
            // studySelection
            // 
            this.studySelection.AutoSize = true;
            this.studySelection.Dock = System.Windows.Forms.DockStyle.Left;
            this.studySelection.Location = new System.Drawing.Point(87, 16);
            this.studySelection.Name = "studySelection";
            this.studySelection.Size = new System.Drawing.Size(78, 18);
            this.studySelection.TabIndex = 1;
            this.studySelection.TabStop = true;
            this.studySelection.Text = "Study Root";
            this.studySelection.UseVisualStyleBackColor = true;
            this.studySelection.CheckedChanged += new System.EventHandler(this.studySelection_CheckedChanged);
            // 
            // patientSelection
            // 
            this.patientSelection.AutoSize = true;
            this.patientSelection.Dock = System.Windows.Forms.DockStyle.Left;
            this.patientSelection.Location = new System.Drawing.Point(3, 16);
            this.patientSelection.Name = "patientSelection";
            this.patientSelection.Size = new System.Drawing.Size(84, 18);
            this.patientSelection.TabIndex = 0;
            this.patientSelection.TabStop = true;
            this.patientSelection.Text = "Patient Root";
            this.patientSelection.UseVisualStyleBackColor = true;
            this.patientSelection.CheckedChanged += new System.EventHandler(this.patientSelection_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.imageGroup);
            this.panel2.Controls.Add(this.seriesGroup);
            this.panel2.Controls.Add(this.studyGroup);
            this.panel2.Controls.Add(this.patientGroup);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 47);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel2.Size = new System.Drawing.Size(992, 226);
            this.panel2.TabIndex = 1;
            // 
            // imageGroup
            // 
            this.imageGroup.Controls.Add(this.imageLabel);
            this.imageGroup.Controls.Add(this.imageTable);
            this.imageGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.imageGroup.Location = new System.Drawing.Point(740, 5);
            this.imageGroup.Name = "imageGroup";
            this.imageGroup.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.imageGroup.Size = new System.Drawing.Size(245, 216);
            this.imageGroup.TabIndex = 3;
            // 
            // imageLabel
            // 
            this.imageLabel.AutoSize = true;
            this.imageLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.imageLabel.Location = new System.Drawing.Point(5, 5);
            this.imageLabel.Name = "imageLabel";
            this.imageLabel.Size = new System.Drawing.Size(65, 13);
            this.imageLabel.TabIndex = 1;
            this.imageLabel.Text = "Image Level";
            // 
            // imageTable
            // 
            this.imageTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8});
            this.imageTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.imageTable.FullRowSelect = true;
            this.imageTable.HideSelection = false;
            this.imageTable.Location = new System.Drawing.Point(5, 23);
            this.imageTable.MultiSelect = false;
            this.imageTable.Name = "imageTable";
            this.imageTable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.imageTable.Size = new System.Drawing.Size(240, 188);
            this.imageTable.TabIndex = 0;
            this.imageTable.UseCompatibleStateImageBehavior = false;
            this.imageTable.View = System.Windows.Forms.View.Details;
            this.imageTable.SelectedIndexChanged += new System.EventHandler(this.imageTable_SelectedIndexChanged);
            this.imageTable.GotFocus += new System.EventHandler(this.imageTable_GotFocus);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Instance Numbar";
            this.columnHeader7.Width = 120;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "SOP Instance UID";
            this.columnHeader8.Width = 110;
            // 
            // seriesGroup
            // 
            this.seriesGroup.Controls.Add(this.seriesLabel);
            this.seriesGroup.Controls.Add(this.seriesTable);
            this.seriesGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.seriesGroup.Location = new System.Drawing.Point(495, 5);
            this.seriesGroup.Name = "seriesGroup";
            this.seriesGroup.Padding = new System.Windows.Forms.Padding(5);
            this.seriesGroup.Size = new System.Drawing.Size(245, 216);
            this.seriesGroup.TabIndex = 2;
            // 
            // seriesLabel
            // 
            this.seriesLabel.AutoSize = true;
            this.seriesLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.seriesLabel.Location = new System.Drawing.Point(5, 5);
            this.seriesLabel.Name = "seriesLabel";
            this.seriesLabel.Size = new System.Drawing.Size(65, 13);
            this.seriesLabel.TabIndex = 1;
            this.seriesLabel.Text = "Series Level";
            // 
            // seriesTable
            // 
            this.seriesTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.seriesTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.seriesTable.FullRowSelect = true;
            this.seriesTable.HideSelection = false;
            this.seriesTable.Location = new System.Drawing.Point(5, 23);
            this.seriesTable.MultiSelect = false;
            this.seriesTable.Name = "seriesTable";
            this.seriesTable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.seriesTable.Size = new System.Drawing.Size(235, 188);
            this.seriesTable.TabIndex = 0;
            this.seriesTable.UseCompatibleStateImageBehavior = false;
            this.seriesTable.View = System.Windows.Forms.View.Details;
            this.seriesTable.SelectedIndexChanged += new System.EventHandler(this.seriesTable_SelectedIndexChanged);
            this.seriesTable.GotFocus += new System.EventHandler(this.seriesTable_GotFocus);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Series Number";
            this.columnHeader5.Width = 110;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Series Instance UID";
            this.columnHeader6.Width = 110;
            // 
            // studyGroup
            // 
            this.studyGroup.Controls.Add(this.studyLabel);
            this.studyGroup.Controls.Add(this.studyTable);
            this.studyGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.studyGroup.Location = new System.Drawing.Point(250, 5);
            this.studyGroup.Name = "studyGroup";
            this.studyGroup.Padding = new System.Windows.Forms.Padding(5);
            this.studyGroup.Size = new System.Drawing.Size(245, 216);
            this.studyGroup.TabIndex = 1;
            // 
            // studyLabel
            // 
            this.studyLabel.AutoSize = true;
            this.studyLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.studyLabel.Location = new System.Drawing.Point(5, 5);
            this.studyLabel.Name = "studyLabel";
            this.studyLabel.Size = new System.Drawing.Size(63, 13);
            this.studyLabel.TabIndex = 1;
            this.studyLabel.Text = "Study Level";
            // 
            // studyTable
            // 
            this.studyTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.studyTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.studyTable.FullRowSelect = true;
            this.studyTable.HideSelection = false;
            this.studyTable.Location = new System.Drawing.Point(5, 23);
            this.studyTable.MultiSelect = false;
            this.studyTable.Name = "studyTable";
            this.studyTable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.studyTable.Size = new System.Drawing.Size(235, 188);
            this.studyTable.TabIndex = 0;
            this.studyTable.UseCompatibleStateImageBehavior = false;
            this.studyTable.View = System.Windows.Forms.View.Details;
            this.studyTable.SelectedIndexChanged += new System.EventHandler(this.studyTable_SelectedIndexChanged);
            this.studyTable.GotFocus += new System.EventHandler(this.studyTable_GotFocus);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Study ID";
            this.columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Study Instance UID";
            this.columnHeader4.Width = 120;
            // 
            // patientGroup
            // 
            this.patientGroup.Controls.Add(this.patientLabel);
            this.patientGroup.Controls.Add(this.patientTable);
            this.patientGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.patientGroup.Location = new System.Drawing.Point(5, 5);
            this.patientGroup.Margin = new System.Windows.Forms.Padding(0);
            this.patientGroup.Name = "patientGroup";
            this.patientGroup.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.patientGroup.Size = new System.Drawing.Size(245, 216);
            this.patientGroup.TabIndex = 0;
            // 
            // patientLabel
            // 
            this.patientLabel.AutoSize = true;
            this.patientLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.patientLabel.Location = new System.Drawing.Point(0, 5);
            this.patientLabel.Name = "patientLabel";
            this.patientLabel.Size = new System.Drawing.Size(69, 13);
            this.patientLabel.TabIndex = 1;
            this.patientLabel.Text = "Patient Level";
            // 
            // patientTable
            // 
            this.patientTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.patientTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.patientTable.FullRowSelect = true;
            this.patientTable.HideSelection = false;
            this.patientTable.Location = new System.Drawing.Point(0, 23);
            this.patientTable.MultiSelect = false;
            this.patientTable.Name = "patientTable";
            this.patientTable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.patientTable.Size = new System.Drawing.Size(240, 188);
            this.patientTable.TabIndex = 0;
            this.patientTable.UseCompatibleStateImageBehavior = false;
            this.patientTable.View = System.Windows.Forms.View.Details;
            this.patientTable.SelectedIndexChanged += new System.EventHandler(this.patientTable_SelectedIndexChanged);
            this.patientTable.GotFocus += new System.EventHandler(this.patientTable_GotFocus);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Patient Name";
            this.columnHeader1.Width = 115;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Patient ID";
            this.columnHeader2.Width = 120;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.attribTable);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 273);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(5);
            this.panel7.Size = new System.Drawing.Size(992, 327);
            this.panel7.TabIndex = 2;
            // 
            // attribTable
            // 
            this.attribTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12});
            this.attribTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attribTable.FullRowSelect = true;
            this.attribTable.Location = new System.Drawing.Point(5, 5);
            this.attribTable.MultiSelect = false;
            this.attribTable.Name = "attribTable";
            this.attribTable.Size = new System.Drawing.Size(982, 317);
            this.attribTable.TabIndex = 3;
            this.attribTable.UseCompatibleStateImageBehavior = false;
            this.attribTable.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Tag";
            this.columnHeader9.Width = 100;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Name";
            this.columnHeader10.Width = 150;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "VR";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Value";
            this.columnHeader12.Width = 200;
            // 
            // InformationModelControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "InformationModelControl";
            this.Size = new System.Drawing.Size(992, 600);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.imageGroup.ResumeLayout(false);
            this.imageGroup.PerformLayout();
            this.seriesGroup.ResumeLayout(false);
            this.seriesGroup.PerformLayout();
            this.studyGroup.ResumeLayout(false);
            this.studyGroup.PerformLayout();
            this.patientGroup.ResumeLayout(false);
            this.patientGroup.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton patientStudyselection;
        private System.Windows.Forms.RadioButton studySelection;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel imageGroup;
        private System.Windows.Forms.Panel seriesGroup;
        private System.Windows.Forms.Panel studyGroup;
        private System.Windows.Forms.Panel patientGroup;
        private System.Windows.Forms.ListView imageTable;
        private System.Windows.Forms.ListView seriesTable;
        private System.Windows.Forms.ListView studyTable;
        private System.Windows.Forms.ListView patientTable;
        private Label studyLabel;
        private Label patientLabel;
        private Label imageLabel;
        private Label seriesLabel;
        private RadioButton patientSelection;
        private Panel panel7;
        private ListView attribTable;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
    }
}
