using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dvtk.Dicom.AttributeLayer.Specification
{
    /// <summary>
    /// Collection of SOP Classes.
    /// </summary>
    public class SopClasses: List<SopClass>
    {
        #region - Public methods -
        // -------------------------------
        // - Begin public methods region -
        // -------------------------------

        /// <summary>
        /// Get all DimseDataSetPair instances from all loaded SOP classes that have the supplied
        /// SOP Class UID and Dimse name.
        /// </summary>
        /// <param name="uid">The SOP Class UID.</param>
        /// <param name="dimseName">The Dimse name.</param>
        /// <returns>The DimseDataSetPair instances.</returns>
        public List<DimseDataSetPair> GetDimseDataSetPairs(string uid, string dimseName)
        {
            List<DimseDataSetPair> dimseDataSetPairs = new List<DimseDataSetPair>();

            foreach (SopClass sopClass in this)
            {
                if (sopClass.Uid == uid)
                {
                    foreach (DimseDataSetPair dimseDataSetPair in sopClass.DimseDataSetPairs)
                    {
                        if (dimseDataSetPair.DimseName == dimseName)
                        {
                            dimseDataSetPairs.Add(dimseDataSetPair);
                        }
                    }
                }
            }

            return (dimseDataSetPairs);
        }

        /// <summary>
        /// Load SOP classes using raw xml file using the supplied path and search pattern.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        public void Load(string path, string searchPattern)
        {
            string[] filePaths = Directory.GetFiles(path, searchPattern);

            foreach (string filePath in filePaths)
            {
                SopClass sopClass = SopClass.Create(filePath);
                this.Add(sopClass);
            }
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
