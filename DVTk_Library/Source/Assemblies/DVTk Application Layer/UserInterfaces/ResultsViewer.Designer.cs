namespace DvtkApplicationLayer.UserInterfaces
{
    partial class ResultsViewer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsViewer));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonForward = new System.Windows.Forms.Button();
            this.buttonBackward = new System.Windows.Forms.Button();
            this.buttonTop = new System.Windows.Forms.Button();
            this.dvtkWebBrowser = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOK.Location = new System.Drawing.Point(845, 650);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonForward
            // 
            this.buttonForward.Image = ((System.Drawing.Image)(resources.GetObject("buttonForward.Image")));
            this.buttonForward.Location = new System.Drawing.Point(52, 36);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(40, 23);
            this.buttonForward.TabIndex = 12;
            this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
            // 
            // buttonBackward
            // 
            this.buttonBackward.Image = ((System.Drawing.Image)(resources.GetObject("buttonBackward.Image")));
            this.buttonBackward.Location = new System.Drawing.Point(12, 36);
            this.buttonBackward.Name = "buttonBackward";
            this.buttonBackward.Size = new System.Drawing.Size(40, 23);
            this.buttonBackward.TabIndex = 11;
            this.buttonBackward.Click += new System.EventHandler(this.buttonBackward_Click);
            // 
            // buttonTop
            // 
            this.buttonTop.Image = ((System.Drawing.Image)(resources.GetObject("buttonTop.Image")));
            this.buttonTop.Location = new System.Drawing.Point(28, 12);
            this.buttonTop.Name = "buttonTop";
            this.buttonTop.Size = new System.Drawing.Size(40, 23);
            this.buttonTop.TabIndex = 10;
            this.buttonTop.Click += new System.EventHandler(this.buttonTop_Click);
            // 
            // dvtkWebBrowser
            // 
            this.dvtkWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dvtkWebBrowser.Location = new System.Drawing.Point(107, 12);
            this.dvtkWebBrowser.Name = "dvtkWebBrowser";
            this.dvtkWebBrowser.Size = new System.Drawing.Size(813, 622);
            this.dvtkWebBrowser.TabIndex = 0;
            this.dvtkWebBrowser.XmlStyleSheetFullFileName = "";
            // 
            // ResultsViewer
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOK;
            this.ClientSize = new System.Drawing.Size(932, 685);
            this.Controls.Add(this.buttonForward);
            this.Controls.Add(this.buttonBackward);
            this.Controls.Add(this.buttonTop);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.dvtkWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "ResultsViewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ResultsViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DvtkWebBrowserNew dvtkWebBrowser;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonForward;
        private System.Windows.Forms.Button buttonBackward;
        private System.Windows.Forms.Button buttonTop;
    }
}