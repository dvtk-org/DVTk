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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace QR_SCU_Emulator {
    public partial class LoadConfiguration : Form {
        string configFile = Path.Combine(Application.StartupPath, "Network_Config.xml");
        XmlDocument doc = null;
        public static string retVal = string.Empty;
        static LoadConfiguration loadConfigDialog;
        public LoadConfiguration() {
            InitializeComponent();
            this.Text = "Select QR SCP";
            this.remoteSystemsList.Items.AddRange(GetRemoteSystems());
        }

        public static string showDialog() {
            loadConfigDialog = new LoadConfiguration();
            loadConfigDialog.ShowDialog();
            return retVal;
        }

        private string[] GetRemoteSystems() {
            doc = new XmlDocument();
            doc.Load(configFile);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("RemoteSystems/*");
            string[] remoteSystems = new string[nodes.Count];
            for (int i = 0; i < nodes.Count; i++) {
                remoteSystems[i] = nodes[i].Name;
            }
            return remoteSystems;
        }

        private void loadConfigOkButton_Click(object sender, System.EventArgs e) {
            retVal = (string)remoteSystemsList.SelectedItem;
            loadConfigDialog.Dispose();
        }

        private void remoteSystemsList_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            retVal = (string)remoteSystemsList.SelectedItem;
            loadConfigDialog.Dispose();
        }

        private void loadConfigCancelBut_Click(object sender, EventArgs e) {
            loadConfigDialog.Dispose();
        }

        private void loadConfigDeleteBut_Click(object sender, EventArgs e) {
                        
            XmlNode remoteSystems = doc.DocumentElement.SelectSingleNode("//RemoteSystems");
            XmlNodeList remoteSystemNodes = remoteSystems.ChildNodes;

            foreach (XmlNode remoteSystem in remoteSystemNodes) {
                if (remoteSystem.Name == (string)remoteSystemsList.SelectedItem) {
                    remoteSystems.RemoveChild(remoteSystem);
                    break;
                }
            }
            doc.Save(configFile);

            remoteSystemsList.Items.Remove(remoteSystemsList.SelectedItem);
        }

    }

}