using System;

namespace Dvtk.Xml.Transformation
{
    public class WorkItem
    {
        #region Internal Variables
        /// <summary>
        ///     Contains the full path to the Xml File.
        /// </summary>
        private string xmlFile;
        
        /// <summary>
        ///     Contains the full path to the Xslt file.
        /// </summary>
        private string xsltFile;
        
        /// <summary>
        ///     Contains the full path to the Output file.
        /// </summary>
        private string outputFile;
        #endregion

        #region Properties
        /// <summary>
        ///     Get or set the Xml File.
        /// </summary>
        public string XmlFile
        {
            get
            {
                return xmlFile;
            }
            set
            {
                xmlFile = value;
            }
        }

        /// <summary>
        ///     Get or set the Xslt File.
        /// </summary>
        public string XsltFile
        {
            get
            {
                return xsltFile;
            }
            set
            {
                XsltFile = value;
            }
        }

        /// <summary>
        ///     Get or set the Output File.
        /// </summary>
        public string OutputFile
        {
            get
            {
                return outputFile;
            }
            set
            {
                OutputFile = value;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        ///     Create a new instance of the <see cref="WorkItem" /> class with the given values.
        /// </summary>
        /// <param name="XmlFile">
        ///     Path of the Xml File.
        /// </param>
        /// <param name="XsltFile">
        ///     Path of the Xslt File.
        /// </param>
        /// <param name="OutputFile">
        ///     Patg of the Output File.
        /// </param>
        public WorkItem(string XmlFile, string XsltFile, string OutputFile)
        {
            this.xmlFile = XmlFile;
            this.xsltFile = XsltFile;
            this.outputFile = OutputFile;
        }
        #endregion
    }
}
