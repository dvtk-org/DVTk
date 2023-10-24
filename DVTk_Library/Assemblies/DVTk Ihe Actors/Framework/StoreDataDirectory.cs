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
using System.Collections;
using System.Collections.Specialized;
using System.IO;

using Dvtk.CommonDataFormat;
using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using DvtkHighLevelInterface.Common.Messages;
using DvtkHighLevelInterface.Common.Compare;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Files;
using DvtkHighLevelInterface.InformationModel;

namespace Dvtk.IheActors.IheFramework
{
    /// <summary>
    /// StoreDataDirectory class.
    /// </summary>
    public class StoreDataDirectory
    {
        private String _htmlOutputFilename = String.Empty;
        private bool _unVrDefinitionLookUpWhenReading = false;
        private bool _compareVr = true;
        private bool _displayGroupLength = false;
        private bool _includeDetailedResults = false;

        /// <summary>
        /// Property - HtmlOutputFilename - to set output filename.
        /// </summary>
        public String HtmlOutputFilename
        {
            set
            {
                _htmlOutputFilename = value;
            }
        }

        /// <summary>
        /// Property - UnVrDefinitionLookUpWhenReading - to set the attribute
        /// VR look-up via the definition file for any received VR UN values.
        /// </summary>
        public bool UnVrDefinitionLookUpWhenReading
        {
            set
            {
                _unVrDefinitionLookUpWhenReading = value;
            }
        }

        /// <summary>
        /// Property - CompareVR - set to include VR comparison between
        /// attributes.
        /// </summary>
        public bool CompareVR
        {
            set
            {
                _compareVr = value;
            }
        }

        /// <summary>
        /// Property - DisplayGroupLength - set to include Group Lengths
        /// in comparison.
        /// </summary>
        public bool DisplayGroupLength
        {
            set
            {
                _displayGroupLength = value;
            }
        }

        /// <summary>
        /// Property - IncludeDetailedResults - set to include a full
        /// comparison of the attributes of each file compared.
        /// </summary>
        public bool IncludeDetailedResults
        {
            set
            {
                _includeDetailedResults = value;
            }
        }

        /// <summary>
        /// Compare the DICOM files in the two directories based on there being corresponding
        /// files in each directory with the same attribute value for the matching Tag.
        /// </summary>
        /// <param name="directory1">Full directory name of first directory.</param>
        /// <param name="directory2">Full directory name of second directory.</param>
        /// <param name="matchingTag">DICOM Tag value to match in a file in each directory.</param>
        /// <returns>Total number of differences between compared files.</returns>
        public int Compare(String directory1, String directory2, DvtkData.Dimse.Tag matchingTag)
        {
            int totalDifferences = 0;
                
            StreamWriter htmlOutput = null;

            try
            {
                // create the output file writer if necessary
                if (_htmlOutputFilename != String.Empty)
                {
                     htmlOutput = new StreamWriter(_htmlOutputFilename);
                }

                // compare all files in directory 1 with corresponding files in directory 2
                // - the files compared is based on them having the same attribute value for the
                // Tag given.
                DirectoryInfo directoryInfo = new DirectoryInfo(directory1);
                FileInfo[] fileInfo = directoryInfo.GetFiles();
                for (int i = 0; i < fileInfo.Length; i++)
                {
                    DataSet dataset1 = new DataSet();
                    String filename1 = fileInfo[i].FullName;
                    dataset1.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(filename1);

                    // get the attribute value for the matching tag from this dataset
                    String valueToMatch = GetAttributeValueFromDataset(dataset1, matchingTag);
                    if (valueToMatch != String.Empty)
                    {
                        // try to find a dataset (file) containing the same value in directory 2
                        String filename2 = String.Empty;
                        DataSet dataset2 = GetMatchingDatasetFromStoreDataDirectory(directory2, matchingTag, valueToMatch, out filename2);

                        // if a dataset is returned that contains the matching attribute value go on to compare all the attributes
                        // in both datasets with eachother
                        if (dataset2 != null)
                        {
                            // get a new compare instance and set the comparison flags
                            StaticDicomCompare staticDicomCompare = new StaticDicomCompare();
                            FlagsDicomAttribute flags = FlagsDicomAttribute.Compare_values | FlagsDicomAttribute.Compare_present | FlagsDicomAttribute.Include_sequence_items;
                            if (_compareVr == false)
                            {
                                staticDicomCompare.DisplayAttributeVR = false;
                            }
                            else
                            {
                                flags |= FlagsDicomAttribute.Compare_VR;
                            }
                            staticDicomCompare.DisplayGroupLength = _displayGroupLength;

                            dataset1.UnVrDefinitionLookUpWhenReading = _unVrDefinitionLookUpWhenReading;
                            dataset2.UnVrDefinitionLookUpWhenReading = _unVrDefinitionLookUpWhenReading;

                            AttributeCollections datasets = new AttributeCollections();
                            datasets.Add(dataset1);
                            datasets.Add(dataset2);

                            StringCollection datasetDescriptions = new StringCollection();
                            datasetDescriptions.Add(filename1);
                            datasetDescriptions.Add(filename2);

                            String title = String.Format("Comparison Results with matching using Tag with {0}", matchingTag.ToString());
                            DvtkHighLevelInterface.Common.Compare.CompareResults datasetCompareResults = staticDicomCompare.CompareAttributeSets(title, datasets, datasetDescriptions, flags);
                            if (htmlOutput != null)
                            {
                                if (_includeDetailedResults == true)
                                {
                                    htmlOutput.WriteLine(datasetCompareResults.Table.ConvertToHtml());
                                }
                                else
                                {
                                    htmlOutput.WriteLine("<br />");
                                    String message = String.Format("Compared {0} with {1} - number of differences: {2}", filename1, filename2, datasetCompareResults.DifferencesCount);
                                    htmlOutput.WriteLine(message);
                                    htmlOutput.WriteLine("<br />");
                                }
                            }

                            // update the total differences counter
                            totalDifferences += datasetCompareResults.DifferencesCount;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
            }
            if (htmlOutput != null)
            {
                htmlOutput.Flush();
                htmlOutput.Close();
            }
            return totalDifferences;
        }

        private DataSet GetMatchingDatasetFromStoreDataDirectory(String directory, DvtkData.Dimse.Tag matchingTag, String matchingTagValue, out String filename)
        {
            DataSet dataset = null;
            filename = String.Empty;

            try 
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                FileInfo[] fileInfo = directoryInfo.GetFiles();
                for (int i = 0; i < fileInfo.Length; i++)
                {
                    DataSet lDataset = new DataSet();
                    lDataset.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(fileInfo[i].FullName);

                    // check if this dataset contains the required matching Tag
                    if (DatasetContainsMatchingTag(lDataset, matchingTag, matchingTagValue) == true)
                    {
                        filename = fileInfo[i].FullName;
                        dataset = lDataset;
                        break;
                    }
                }

            }
            catch (System.Exception)
            {
            }

            return dataset;
        }

        private String GetAttributeValueFromDataset(DataSet dataset, DvtkData.Dimse.Tag tag)
        {
            DicomMessage dicomMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERQ, dataset);
            String attributeValue = GenerateTriggers.GetValueFromMessageUsingTag(dicomMessage, tag);

            return attributeValue;
        }

        private bool DatasetContainsMatchingTag(DataSet dataset, DvtkData.Dimse.Tag matchingTag, String matchingAttributeValue)
        {
            bool matchingTagFound = false;

            String fileAttributeValue = GetAttributeValueFromDataset(dataset, matchingTag);
            if ((fileAttributeValue != String.Empty) &&
                (fileAttributeValue == matchingAttributeValue))
            {
                matchingTagFound = true;
            }

            return matchingTagFound;
        }
    }
}
