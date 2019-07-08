using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DCMEditor
{
    partial class FindAttribute : Form
    {
        string attributeTag = "";
        string attributeValue = "";
        string attributeName = "";
        public FindAttribute()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            attributeTag = textBox_attrTag.Text;
            attributeValue = textBoxAttrValue.Text;
            attributeName = textBoxAttrName.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string AttributeTag
        {
            get { return this.attributeTag; }
        }

        public string AttributeValue
        {
            get { return this.attributeValue; }
        }
        
        public string AttributeName
        {
            get { return this.attributeName; }
        }

        public bool IsAnd
        {
            get { return radioButtonAnd.Checked; }
        }

        public bool IsAttributeTag
        {
            get { return radioButtonAttrTag.Checked; }
        }

        public bool IsAttributeValue
        {
            get { return radioButtonAttrValue.Checked; }
        }

        public bool IsAttributeName
        {
            get { return radioButtonAttrName.Checked; }
        }
            
        private void radioButtonAttrTag_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAttrName.Enabled = false;
            textBoxAttrValue.Enabled = false;
            textBox_attrTag.Enabled = true;
            textBoxAttrValue.Text = "";
            textBoxAttrName.Text = "";
        }

        private void radioButtonAttrValue_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAttrName.Enabled = false;
            textBoxAttrValue.Enabled = true;
            textBox_attrTag.Enabled = false;
            textBox_attrTag.Text = "";
            textBoxAttrName.Text = "";
        }

        private void radioButtonAnd_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAttrValue.Enabled = true;
            textBox_attrTag.Enabled = true;
            textBoxAttrName.Enabled = false;
        }

        private void radioButtonAttrName_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAttrName.Enabled = true;
            textBoxAttrValue.Enabled = false;
            textBox_attrTag.Enabled = false;
            textBoxAttrValue.Text = "";
            textBox_attrTag.Text = "";
        }                              
    }
}