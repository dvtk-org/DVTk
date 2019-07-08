using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DvtkHighLevelInterface.Common.Threads;

namespace DvtkApplicationLayer.UserInterfaces
{
    /// <summary>
    /// Class that has the capabilities to view the results of a Thread instance.
    /// </summary>
    public partial class ResultsViewer : Form
    {
        //
        // - Fields -
        //

        /// <summary>
        /// The thread which results will be displayed.
        /// </summary>
        private Thread thread = null;

        private string resultsPath = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ResultsViewer()
        {
            InitializeComponent();

            this.dvtkWebBrowser.BackwardFormwardEnabledStateChangeEvent += new DvtkWebBrowserNew.BackwardFormwardEnabledStateChangeEventHandler(UpdateButtons);
            this.dvtkWebBrowser.XmlStyleSheetFullFileName = Path.Combine(Application.StartupPath, "DVT_RESULTS.xslt");
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="thread">The thread which results will be displayed.</param>
        public ResultsViewer(Thread thread): this()
        {
            this.thread = thread;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="thread">The thread which results will be displayed.</param>
        public ResultsViewer(string resultsPath)
            : this()
        {
            this.resultsPath = resultsPath;
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Navigate back.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event erguments.</param>
        private void buttonBackward_Click(object sender, EventArgs e)
        {
            this.dvtkWebBrowser.Back();
            Focus();
        }

        /// <summary>
        /// Navigate forward.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event erguments.</param>
        private void buttonForward_Click(object sender, EventArgs e)
        {
            this.dvtkWebBrowser.Forward();
            Focus();
        }

        /// <summary>
        /// Navigate to the original results.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event erguments.</param>
        private void buttonTop_Click(object sender, EventArgs e)
        {
            this.dvtkWebBrowser.Navigate(this.thread.Options.DetailResultsFullFileName);
            Focus();
        }

        /// <summary>
        /// Updates the buttons on this form.
        /// </summary>
        private void UpdateButtons()
        {
            this.buttonBackward.Enabled = this.dvtkWebBrowser.IsBackwardEnabled;
            this.buttonForward.Enabled = this.dvtkWebBrowser.IsForwardEnabled;
        }

        private void ResultsViewer_Load(object sender, EventArgs e)
        {
            Focus();
            this.dvtkWebBrowser.Navigate("about:blank");

            if (this.thread != null)
            {

                if (this.thread.ThreadState != ThreadState.Stopped)
                {
                    this.Text = "Thread \"" + this.thread.Options.Identifier + "\"";
                    MessageBox.Show("Unable to display the results because the thread is not in a stopped state.");
                }
                else
                {
                    string resultsFullFileName = this.thread.Options.DetailResultsFullFileName;

                    this.Text = "Thread \"" + this.thread.Options.Identifier + "\" - Results \"" + resultsFullFileName + "\"";

                    this.dvtkWebBrowser.Navigate(resultsFullFileName);
                }
            }

            if (this.resultsPath != null)
            {
                this.Text = "Results \"" + this.resultsPath + "\"";

                this.dvtkWebBrowser.Navigate(resultsPath);

            }

            UpdateButtons();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}