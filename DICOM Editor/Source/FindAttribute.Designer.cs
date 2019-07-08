namespace DCMEditor
{
    partial class FindAttribute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindAttribute));
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_attrTag = new System.Windows.Forms.TextBox();
            this.Ok = new System.Windows.Forms.Button();
            this.textBoxAttrValue = new System.Windows.Forms.TextBox();
            this.radioButtonAnd = new System.Windows.Forms.RadioButton();
            this.radioButtonAttrTag = new System.Windows.Forms.RadioButton();
            this.radioButtonAttrValue = new System.Windows.Forms.RadioButton();
            this.radioButtonAttrName = new System.Windows.Forms.RadioButton();
            this.textBoxAttrName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(22, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(296, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "Note: Specify Attribute Tag as:(gggg,eeee) or gggg,eeee";
            // 
            // textBox_attrTag
            // 
            this.textBox_attrTag.Location = new System.Drawing.Point(151, 15);
            this.textBox_attrTag.Name = "textBox_attrTag";
            this.textBox_attrTag.Size = new System.Drawing.Size(128, 20);
            this.textBox_attrTag.TabIndex = 14;
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(124, 183);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 17;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // textBoxAttrValue
            // 
            this.textBoxAttrValue.Enabled = false;
            this.textBoxAttrValue.Location = new System.Drawing.Point(151, 104);
            this.textBoxAttrValue.Name = "textBoxAttrValue";
            this.textBoxAttrValue.Size = new System.Drawing.Size(128, 20);
            this.textBoxAttrValue.TabIndex = 18;
            // 
            // radioButtonAnd
            // 
            this.radioButtonAnd.AutoSize = true;
            this.radioButtonAnd.Location = new System.Drawing.Point(51, 67);
            this.radioButtonAnd.Name = "radioButtonAnd";
            this.radioButtonAnd.Size = new System.Drawing.Size(137, 17);
            this.radioButtonAnd.TabIndex = 20;
            this.radioButtonAnd.Text = "Attribute Tag and Value";
            this.radioButtonAnd.UseVisualStyleBackColor = true;
            this.radioButtonAnd.CheckedChanged += new System.EventHandler(this.radioButtonAnd_CheckedChanged);
            // 
            // radioButtonAttrTag
            // 
            this.radioButtonAttrTag.AutoSize = true;
            this.radioButtonAttrTag.Checked = true;
            this.radioButtonAttrTag.Location = new System.Drawing.Point(51, 15);
            this.radioButtonAttrTag.Name = "radioButtonAttrTag";
            this.radioButtonAttrTag.Size = new System.Drawing.Size(86, 17);
            this.radioButtonAttrTag.TabIndex = 21;
            this.radioButtonAttrTag.TabStop = true;
            this.radioButtonAttrTag.Text = "Attribute Tag";
            this.radioButtonAttrTag.UseVisualStyleBackColor = true;
            this.radioButtonAttrTag.CheckedChanged += new System.EventHandler(this.radioButtonAttrTag_CheckedChanged);
            // 
            // radioButtonAttrValue
            // 
            this.radioButtonAttrValue.AutoSize = true;
            this.radioButtonAttrValue.Location = new System.Drawing.Point(51, 105);
            this.radioButtonAttrValue.Name = "radioButtonAttrValue";
            this.radioButtonAttrValue.Size = new System.Drawing.Size(94, 17);
            this.radioButtonAttrValue.TabIndex = 22;
            this.radioButtonAttrValue.Text = "Attribute Value";
            this.radioButtonAttrValue.UseVisualStyleBackColor = true;
            this.radioButtonAttrValue.CheckedChanged += new System.EventHandler(this.radioButtonAttrValue_CheckedChanged);
            // 
            // radioButtonAttrName
            // 
            this.radioButtonAttrName.AutoSize = true;
            this.radioButtonAttrName.Location = new System.Drawing.Point(52, 144);
            this.radioButtonAttrName.Name = "radioButtonAttrName";
            this.radioButtonAttrName.Size = new System.Drawing.Size(95, 17);
            this.radioButtonAttrName.TabIndex = 23;
            this.radioButtonAttrName.TabStop = true;
            this.radioButtonAttrName.Text = "Attribute Name";
            this.radioButtonAttrName.UseVisualStyleBackColor = true;
            this.radioButtonAttrName.CheckedChanged += new System.EventHandler(this.radioButtonAttrName_CheckedChanged);
            // 
            // textBoxAttrName
            // 
            this.textBoxAttrName.Enabled = false;
            this.textBoxAttrName.Location = new System.Drawing.Point(151, 144);
            this.textBoxAttrName.Name = "textBoxAttrName";
            this.textBoxAttrName.Size = new System.Drawing.Size(128, 20);
            this.textBoxAttrName.TabIndex = 24;
            // 
            // FindAttribute
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 232);
            this.Controls.Add(this.textBoxAttrName);
            this.Controls.Add(this.radioButtonAttrName);
            this.Controls.Add(this.radioButtonAttrValue);
            this.Controls.Add(this.radioButtonAttrTag);
            this.Controls.Add(this.radioButtonAnd);
            this.Controls.Add(this.textBoxAttrValue);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_attrTag);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindAttribute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Attribute";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_attrTag;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.TextBox textBoxAttrValue;
        private System.Windows.Forms.RadioButton radioButtonAnd;
        private System.Windows.Forms.RadioButton radioButtonAttrTag;
        private System.Windows.Forms.RadioButton radioButtonAttrValue;
        private System.Windows.Forms.RadioButton radioButtonAttrName;
        private System.Windows.Forms.TextBox textBoxAttrName;
    }
}