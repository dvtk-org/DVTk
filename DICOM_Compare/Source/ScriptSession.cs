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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using DvtkHighLevelInterface.Common.Messages;
using DvtkHighLevelInterface.Common.Compare;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Files;
using DvtkData;
using DvtkApplicationLayer;

namespace DCMCompare
{
    using HLI = DvtkHighLevelInterface.Dicom.Other;
    using HLIStaticCompare = DvtkHighLevelInterface.Common.Compare;

    /// <summary>
	/// Summary description for ScriptSession.
	/// </summary>
	public class MainThread : DvtkHighLevelInterface.Dicom.Threads.DicomThread
    {
        public MainThread()
        { }

        protected override void Execute()
        {
            try
            {
                WriteHtmlInformation("<br />");
                WriteInformation(string.Format("Reading reference media file from {0}", DCMCompareForm.firstDCMFile));
                WriteHtmlInformation("<br />");

                // Read the DCM File
                DicomFile dcmFile = new DicomFile();

                DataSet refDataset = new DataSet();

                if ((DCMCompareForm.firstDCMFile.ToLower().IndexOf("dicomdir")) != -1)
                {
                    // Read the DICOMDIR dataset
                    refDataset.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(DCMCompareForm.firstDCMFile);
                }
                else
                {
                    dcmFile.Read(DCMCompareForm.firstDCMFile, this);
                    refDataset = dcmFile.DataSet;
                }

                refDataset.UnVrDefinitionLookUpWhenReading = false;

                FileMetaInformation refFMI = dcmFile.FileMetaInformation;

                WriteInformation(string.Format("Reading source media file from {0}", DCMCompareForm.secondDCMFile));
                WriteHtmlInformation("<br />");

                DataSet srcDataset = new DataSet();

                if ((DCMCompareForm.secondDCMFile.ToLower().IndexOf("dicomdir")) != -1)
                {
                    // Read the DICOMDIR dataset
                    srcDataset.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(DCMCompareForm.secondDCMFile);
                }
                else
                {
                    dcmFile.Read(DCMCompareForm.secondDCMFile, this);
                    srcDataset = dcmFile.DataSet;
                }

                srcDataset.UnVrDefinitionLookUpWhenReading = false;

                FileMetaInformation srcFMI = dcmFile.FileMetaInformation;

                // Now get the list of filtered attribute
                if (DCMCompareForm.attributesTagList.Count != 0)
                {
                    foreach (string filterAttr in DCMCompareForm.attributesTagList)
                    {
                        try
                        {
                            if (srcDataset.Exists(filterAttr))
                            {
                                srcDataset.Delete(filterAttr);
                            }
                            if (refDataset.Exists(filterAttr))
                            {
                                refDataset.Delete(filterAttr);
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    DCMCompareForm.attributesTagList.Clear();
                }

                WriteInformation(string.Format("Comparing the Media files - {0} and {1}", DCMCompareForm.firstDCMFile, DCMCompareForm.secondDCMFile));
                WriteHtmlInformation("<br />");

                StaticDicomCompare staticDicomCompare = new StaticDicomCompare();

                //Determine the VR display based on Transfer syntax				
                string srcTransferSyntax = "";
                string refTransferSyntax = "";

                if ((srcFMI != null) && srcFMI.Exists("0x00020010"))
                {
                    // Get the Transfer syntax
                    DvtkHighLevelInterface.Dicom.Other.Attribute tranferSyntaxAttr = srcFMI["0x00020010"];
                    srcTransferSyntax = tranferSyntaxAttr.Values[0];
                }
                else
                {
                    WriteHtmlInformation(string.Format("Couldn't retrieve the Transfer syntax from DCM File {0}", DCMCompareForm.secondDCMFile));
                    WriteHtmlInformation("<br />");
                }

                if ((refFMI != null) && refFMI.Exists("0x00020010"))
                {
                    // Get the Transfer syntax
                    DvtkHighLevelInterface.Dicom.Other.Attribute tranferSyntaxAttr = refFMI["0x00020010"];
                    refTransferSyntax = tranferSyntaxAttr.Values[0];
                }
                else
                {
                    WriteHtmlInformation(string.Format("Couldn't retrieve the Transfer syntax from DCM File {0}", DCMCompareForm.firstDCMFile));
                    WriteHtmlInformation("<br />");
                }

                FlagsDicomAttribute flags = FlagsDicomAttribute.Compare_values | FlagsDicomAttribute.Compare_present | FlagsDicomAttribute.Include_sequence_items;
                if ((srcTransferSyntax == "1.2.840.10008.1.2") ||
                    (refTransferSyntax == "1.2.840.10008.1.2"))
                {
                    staticDicomCompare.DisplayAttributeVR = false;
                }
                else
                {
                    flags |= FlagsDicomAttribute.Compare_VR;
                }

                //Check for group length attributes option
                if (DCMCompareForm.filterGroupLengthAttributes)
                {
                    /*for( int i=0; i < srcDataset.Count; i++ )
					{
						HLI.Attribute attribute   = srcDataset[i];						
						if(attribute.ElementNumber == 0x0000)
							srcDataset.Delete(DCMCompareForm.TagString(attribute.GroupNumber,attribute.ElementNumber));
					}

					for( int i=0; i < refDataset.Count; i++ )
					{
						HLI.Attribute attribute   = refDataset[i];						
						if(attribute.ElementNumber == 0x0000)
							refDataset.Delete(DCMCompareForm.TagString(attribute.GroupNumber,attribute.ElementNumber));
					}*/
                    staticDicomCompare.DisplayGroupLength = false;
                }

                int differences = 0;

                if ((refFMI != null) && (srcFMI != null))
                {
                    AttributeCollections fmis = new AttributeCollections();
                    fmis.Add(refFMI);
                    fmis.Add(srcFMI);

                    StringCollection fmiDescriptions = new StringCollection();
                    fmiDescriptions.Add("Ref FMI");
                    fmiDescriptions.Add("Src FMI");

                    HLIStaticCompare.CompareResults fmiCompareResults = staticDicomCompare.CompareAttributeSets("FMI compare results", fmis, fmiDescriptions, flags);
                    WriteHtmlInformation(fmiCompareResults.Table.ConvertToHtml());

                    differences += fmiCompareResults.DifferencesCount;
                    NrOfValidationErrors += (uint)fmiCompareResults.DifferencesCount;
                }

                AttributeCollections datasets = new AttributeCollections();
                datasets.Add(refDataset);
                datasets.Add(srcDataset);

                StringCollection dsDescriptions = new StringCollection();
                dsDescriptions.Add("Ref Dataset");
                dsDescriptions.Add("Src Dataset");

                HLIStaticCompare.CompareResults dsCompareResults = staticDicomCompare.CompareAttributeSets("DataSet compare results", datasets, dsDescriptions, flags);

                WriteHtmlInformation(dsCompareResults.Table.ConvertToHtml());

                differences += dsCompareResults.DifferencesCount;

                WriteHtmlInformation("<b>");
                WriteInformation("Differences found: " + differences.ToString());
                WriteHtmlInformation("</b><br />");

                NrOfValidationErrors += (uint)dsCompareResults.DifferencesCount;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                DvtkApplicationLayer.DefinitionFilesChecker.CheckVersion("1.0.0", "2.0.0");
            }
            catch (Exception exception)
            {
                Exception customException = new Exception("PLEASE INSTALL DVTK DEFINITION FILES FROM www.dvtk.org website", exception);
                CustomExceptionHandler.ShowThreadExceptionDialog(customException);
                return;
            }
#if !DEBUG
            try
            {
#endif

                // Checks the version of both the application and the DVTk library.
                // If one or both are a non-official or Alpha version, a warning message box is displayed.
                DvtkApplicationLayer.VersionChecker.CheckVersion();

                // Initialize the Dvtk library
                Dvtk.Setup.Initialize();

                // Invoke the form and start the processing
                DCMCompareForm form = new DCMCompareForm();
                form.ShowDialog();

                // Terminate the Dvtk library
                Dvtk.Setup.Terminate();

#if !DEBUG
            }
            catch (Exception exception)
            {
                CustomExceptionHandler.ShowThreadExceptionDialog(exception);
            }
#endif
        }
    }
}
