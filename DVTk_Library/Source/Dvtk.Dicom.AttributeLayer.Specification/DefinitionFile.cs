using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// Class containing common functionality when dealing with raw xml files.
    /// </summary>
    public static class DefinitionFile
    {
        #region - Public methods -
        // -------------------------------
        // - Begin public methods region -
        // -------------------------------

        /// <summary>
        /// Create an exception describing a problem in a raw xml file.
        /// </summary>
        /// <param name="node">The node the problem is occuring in.</param>
        /// <param name="className">
        /// The class name of the instance that cannot be constructed because of the problem in the
        /// raw xml file.
        /// </param>
        /// <returns>The created exception.</returns>
        public static Exception CreateException(XPathNavigator node, string className)
        {
            return (CreateException(node, className, string.Empty, null));
        }

        /// <summary>
        /// Create an exception describing a problem in a raw xml file.
        /// </summary>
        /// <param name="node">The node the problem is occuring in.</param>
        /// <param name="className">
        /// The class name of the instance that cannot be constructed because of the problem in the
        /// raw xml file.
        /// </param>
        /// <param name="extraInfo">Extra information about this exception.</param>
        /// <param name="innerException">
        /// Another exception that becomes the inner exception of the created exception.
        /// </param>
        /// <returns>The created exception.</returns>
        public static Exception CreateException(XPathNavigator node, string className, string extraInfo, Exception innerException)
        {
            IXmlLineInfo lineInfo = node as IXmlLineInfo;
            string exceptionText = string.Empty;

            exceptionText = "Unable to create an instance of class " + className + " while parsing line " + lineInfo.LineNumber.ToString() + " and position " + lineInfo.LinePosition.ToString() + ".";
            if (extraInfo.Length > 0)
            {
                exceptionText += " " + extraInfo;
            }

            Exception exception = new Exception(exceptionText, innerException);

            return (exception);
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
