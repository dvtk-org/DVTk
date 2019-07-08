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
using System.Reflection;
using System.Text;
using System.IO;

namespace DvtkApplicationLayer.UserInterfaces
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class AboutForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button buttonOk;
        private Label label1;
        private Label label2;
        private Label labelVersions;
        private Label labelCopyRight;
        private Label labelLGPL;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private AboutForm()
		{
			// Do nothing.
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationName"></param>
		public AboutForm(String applicationName)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.applicationName = applicationName;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.buttonOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelVersions = new System.Windows.Forms.Label();
            this.labelCopyRight = new System.Windows.Forms.Label();
            this.labelLGPL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Location = new System.Drawing.Point(678, 344);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(77, 26);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(62, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(648, 3);
            this.label1.TabIndex = 5;
            this.label1.Text = "-----";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(62, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(648, 2);
            this.label2.TabIndex = 7;
            this.label2.Text = "-----";
            // 
            // labelVersions
            // 
            this.labelVersions.Location = new System.Drawing.Point(60, 14);
            this.labelVersions.Name = "labelVersions";
            this.labelVersions.Size = new System.Drawing.Size(695, 46);
            this.labelVersions.TabIndex = 8;
            // 
            // labelCopyRight
            // 
            this.labelCopyRight.Location = new System.Drawing.Point(60, 97);
            this.labelCopyRight.Name = "labelCopyRight";
            this.labelCopyRight.Size = new System.Drawing.Size(695, 53);
            this.labelCopyRight.TabIndex = 9;
            // 
            // labelLGPL
            // 
            this.labelLGPL.Location = new System.Drawing.Point(56, 179);
            this.labelLGPL.Name = "labelLGPL";
            this.labelLGPL.Size = new System.Drawing.Size(699, 156);
            this.labelLGPL.TabIndex = 10;
            // 
            // AboutForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(641, 333);
            this.Controls.Add(this.labelLGPL);
            this.Controls.Add(this.labelCopyRight);
            this.Controls.Add(this.labelVersions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Title";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private String applicationName = "";
		private String addlInfoToDisplay = "";

		/// <summary>
		/// String which displays license and other info
		/// </summary>
		public string InfoToDisplay
		{
			set
			{
				addlInfoToDisplay = value;
			}
		}

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void AboutForm_Load(object sender, System.EventArgs e)
		{
            //
            // Get the versions of the application and library.
            //
             
            Assembly applicationAssembly = Assembly.GetEntryAssembly();

            String applicationVersion = GetVersionString(applicationAssembly);

            // Any class, enumerate from the DVTk assembly will work.
            Dvtk.Events.ReportLevel reportLevel = Dvtk.Events.ReportLevel.None;

            Assembly libraryAssembly = Assembly.GetAssembly(reportLevel.GetType());

            String libraryVersion = GetVersionString(libraryAssembly);


            //
            // Set the caption text of this form.
            //

			this.Text = string.Format("About {0}", this.applicationName);


            //
            // Set the versions and the copyright statement in this form.
            //

            StringBuilder versionString = new StringBuilder();
            StringBuilder copyrightString = new StringBuilder();
            StringBuilder lGPLString = new StringBuilder();

            versionString.Append(applicationName + " " + applicationVersion + "\n\n");
            versionString.Append("DVTk Library " + libraryVersion + "\n");

            copyrightString.Append("DVTk - The Healthcare Validation Toolkit (www.dvtk.org)\n\n");
            FileInfo info = new FileInfo(Assembly.GetExecutingAssembly().Location);
            
            copyrightString.Append("Copyright © "+info.CreationTime.Year+" DVTk\n");
           
            lGPLString.Append("DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU\n");
            lGPLString.Append("Lesser General Public License as published by the Free Software Foundation; either version 3.0\n");
            lGPLString.Append("of the License, or (at your option) any later version.\n");
            lGPLString.Append("\n");
            lGPLString.Append("DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even\n");
            lGPLString.Append("the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser\n");
            lGPLString.Append("General Public License for more details.\n");
            lGPLString.Append("\n");
            lGPLString.Append("You should have received a copy of the GNU Lesser General Public License along with this\n");
            lGPLString.Append("application; if not, see <http://www.gnu.org/licenses/>\n");

            this.labelVersions.Text = versionString.ToString();

            this.labelCopyRight.Text = copyrightString.ToString();

            this.labelLGPL.Text = lGPLString.ToString();
		}

        /// <summary>
        /// Gets the version of the supplied assembly as a string.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>A string containing the version of the assembly.</returns>
        private String GetVersionString(Assembly assembly)
        {
            string versionString = String.Empty;

            AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;

            if (IsLocalBuild(assembly) || IsAlphaBuild(assembly))
            {
                versionString = String.Format("{0:D}.{1:D}.{2:D}.{3:D}", version.Major, version.Minor, version.Build, version.Revision);
            }
            else
            {
                versionString = String.Format("{0:D}.{1:D}.{2:D}", version.Major, version.Minor, version.Build);
            }

            return (versionString);
        }

        /// <summary>
        /// Indicates if the version of the supplied assembly represents an Alpha build.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Boolean indicating if the version of the supplied assembly represents an Alpha build.</returns>
        private bool IsAlphaBuild(Assembly assembly)
        {
            bool isAlphaBuild = true;

            AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;

            if (((version.Major > 0) || (version.Minor > 0) || (version.Build > 0)) && (version.Revision != 0))
            {
                isAlphaBuild = true;
            }
            else
            {
                isAlphaBuild = false;
            }

            return (isAlphaBuild);
        }

        /// <summary>
        /// Indicates if the version of the supplied assembly represents a local build.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Boolean indicating if the version of the supplied assembly represents a local build.</returns>
        private bool IsLocalBuild(Assembly assembly)
        {
            bool isLocalBuild = true;

            AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;

            if ((version.Major == 0) && (version.Minor == 0) && (version.Build == 0) && (version.Revision == 0))
            {
                isLocalBuild = true;
            }
            else
            {
                isLocalBuild = false;
            }

            return (isLocalBuild);
        }
	}
}
