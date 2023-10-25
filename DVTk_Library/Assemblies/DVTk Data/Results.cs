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

using System.IO;
using DvtkData.DvtDetailToXml;

namespace DvtkData.Results
{
    using DvtkData.Dimse;
    using DvtkData.Dul;
    using DvtkData.Media;

    /// <summary>
    /// Top-level element of the Dvtk output.
    /// Container for the various results nodes.
    /// </summary>
    /// <remarks>
    /// This forms the top-level Xml-element in the Xml output.
    /// </remarks>
    public class Results
    {

        /// <summary>
        /// First node in the results. Specifies details over the session.
        /// </summary>
        public Details Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
            }
        }
        private Details _Details;
    }

    /// <summary>
    /// Reference to another results document.
    /// </summary>
    /// <remarks>
    /// Used in parent-child relationship for results documents.
    /// For emulators, the results of individual associations are
    /// outputted to seperate child results documents.
    /// </remarks>
    public class SubResultsLink : IDvtDetailToXml, IDvtSummaryToXml
    {
        /// <summary>
        /// Reference to a child results document.
        /// </summary>
        public System.UInt32 SubResultsIndex
        {
            get
            {
                return _SubResultsIndex;
            }
            set
            {
                _SubResultsIndex = value;
            }
        }
        private System.UInt32 _SubResultsIndex;

        /// <summary>
        /// Number of validation errors for this sub results.
        /// </summary>
        public System.UInt32 NrOfErrors
        {
            get
            {
                return _NrOfErrors;
            }
            set
            {
                _NrOfErrors = value;
            }
        }
        private System.UInt32 _NrOfErrors;

        /// <summary>
        /// Number of validation warnings for this sub results.
        /// </summary>
        public System.UInt32 NrOfWarnings
        {
            get
            {
                return _NrOfWarnings;
            }
            set
            {
                _NrOfWarnings = value;
            }
        }
        private System.UInt32 _NrOfWarnings;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<SubResultsLink Index=\"{0}\" NrOfErrors=\"{1}\" NrOfWarnings=\"{2}\">",
                    SubResultsIndex.ToString(),
                    NrOfErrors.ToString(),
                    NrOfWarnings.ToString());
                streamWriter.WriteLine("</SubResultsLink>");
            }

            return true;
        }

        /// <summary>
        /// Serialize DVT Summary Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtSummaryToXml(StreamWriter streamWriter, int level)
        {
            if ((streamWriter != null) &&
                (NrOfErrors != 0))
            {
                streamWriter.WriteLine("<SubResultsLink Index=\"{0}\" NrOfErrors=\"{1}\" NrOfWarnings=\"{2}\">",
                    SubResultsIndex.ToString(),
                    NrOfErrors.ToString(),
                    NrOfWarnings.ToString());
                streamWriter.WriteLine("</SubResultsLink>");
            }

            return true;
        }
    }

    /// <summary>
    /// First node in the results. Specifies details over the session.
    /// </summary>
    public class Details : IDvtDetailToXml
    {

        /// <summary>
        /// Session Identifier.
        /// </summary>
        public System.UInt16 SessionId
        {
            get
            {
                return _SessionId;
            }
            set
            {
                _SessionId = value;
            }
        }
        private System.UInt16 _SessionId;

        /// <summary>
        /// Session Title.
        /// </summary>
        public string SessionTitle
        {
            get
            {
                return _SessionTitle;
            }
            set
            {
                _SessionTitle = value;
            }
        }
        private string _SessionTitle;

        /// <summary>
        /// Scp emulator type.
        /// </summary>
        public ScpEmulatorType ScpEmulatorType
        {
            get
            {
                return _ScpEmulatorType;
            }
            set
            {
                _ScpEmulatorType = value;
            }
        }
        private ScpEmulatorType _ScpEmulatorType = ScpEmulatorType.Unknown;

        /// <summary>
        /// Scu emulator type.
        /// </summary>
        public ScuEmulatorType ScuEmulatorType
        {
            get
            {
                return _ScuEmulatorType;
            }
            set
            {
                _ScuEmulatorType = value;
            }
        }
        private ScuEmulatorType _ScuEmulatorType = ScuEmulatorType.Unknown;

        /// <summary>
        /// Manufacturer. Specifies the manufacturer for the SUT (System Under Test).
        /// </summary>
        public string Manufacturer
        {
            get
            {
                return _Manufacturer;
            }
            set
            {
                _Manufacturer = value;
            }
        }
        private string _Manufacturer;

        /// <summary>
        /// Model Name. Specifies the model name for the SUT (System Under Test).
        /// </summary>
        public string ModelName
        {
            get
            {
                return _ModelName;
            }
            set
            {
                _ModelName = value;
            }
        }
        private string _ModelName;

        /// <summary>
        /// Software Versions. Specifies the software versions for the SUT (System Under Test).
        /// </summary>
        public string SoftwareVersions
        {
            get
            {
                return _SoftwareVersions;
            }
            set
            {
                _SoftwareVersions = value;
            }
        }
        private string _SoftwareVersions;

        /// <summary>
        /// Application Entity Name.
        /// Specifies the application entity name used as identifier into the used Information Object Definitions.
        /// </summary>
        public string ApplicationEntityName
        {
            get
            {
                return _ApplicationEntityName;
            }
            set
            {
                _ApplicationEntityName = value;
            }
        }
        private string _ApplicationEntityName;

        /// <summary>
        /// Application Entity Version.
        /// Specifies the application entity version used as identifier into the used Information Object Definitions.
        /// </summary>
        public string ApplicationEntityVersion
        {
            get
            {
                return _ApplicationEntityVersion;
            }
            set
            {
                _ApplicationEntityVersion = value;
            }
        }
        private string _ApplicationEntityVersion;

        /// <summary>
        /// System Under Test (SUT) Role. Specifies the dicom communication role for the SUT (System Under Test).
        /// </summary>
        public string SutRole
        {
            get
            {
                return _SutRole;
            }
            set
            {
                _SutRole = value;
            }
        }
        private string _SutRole;

        /// <summary>
        /// Tester. Name of the person performing the test.
        /// </summary>
        public string Tester
        {
            get
            {
                return _Tester;
            }
            set
            {
                _Tester = value;
            }
        }
        private string _Tester;

        /// <summary>
        /// Test Date. Date on which the test is performed.
        /// </summary>
        public System.DateTime TestDate
        {
            get
            {
                return _TestDate;
            }
            set
            {
                _TestDate = value;
            }
        }
        private System.DateTime _TestDate = System.DateTime.Now;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<SessionDetails>");
                streamWriter.WriteLine("<SessionId>{0}</SessionId>", SessionId.ToString());
                streamWriter.WriteLine("<SessionTitle>{0}</SessionTitle>", SessionTitle);
                if (ScpEmulatorType != ScpEmulatorType.Unknown)
                {
                    string scpEmulatorTypeString = string.Empty;
                    switch (ScpEmulatorType)
                    {
                        case ScpEmulatorType.Printing: scpEmulatorTypeString = "Printing"; break;
                        case ScpEmulatorType.Storage: scpEmulatorTypeString = "Storage"; break;
                        case ScpEmulatorType.StorageCommit: scpEmulatorTypeString = "Storage Commit"; break;
                        case ScpEmulatorType.Mpps: scpEmulatorTypeString = "Mpps"; break;
                        case ScpEmulatorType.Worklist: scpEmulatorTypeString = "Worklist"; break;
                        case ScpEmulatorType.QueryRetrieve: scpEmulatorTypeString = "Query/Retrieve"; break;
                        default: break;
                    }
                    streamWriter.WriteLine("<ScpEmulatorType>{0}</ScpEmulatorType>", scpEmulatorTypeString);
                }
                if (ScuEmulatorType != ScuEmulatorType.Unknown)
                {
                    string scuEmulatorTypeString = string.Empty;
                    switch (ScuEmulatorType)
                    {
                        case ScuEmulatorType.Storage: scuEmulatorTypeString = "Storage"; break;
                        default: break;
                    }
                    streamWriter.WriteLine("<ScuEmulatorType>{0}</ScuEmulatorType>", scuEmulatorTypeString);
                }
                streamWriter.WriteLine("<Manufacturer>{0}</Manufacturer>", Manufacturer);
                streamWriter.WriteLine("<ModelName>{0}</ModelName>", ModelName);
                streamWriter.WriteLine("<SoftwareVersions>{0}</SoftwareVersions>", SoftwareVersions);
                streamWriter.WriteLine("<ApplicationEntityName>{0}</ApplicationEntityName>", ApplicationEntityName);
                streamWriter.WriteLine("<ApplicationEntityVersion>{0}</ApplicationEntityVersion>", ApplicationEntityVersion);
                streamWriter.WriteLine("<SutRole>{0}</SutRole>", SutRole);
                streamWriter.WriteLine("<Tester>{0}</Tester>", Tester);
                streamWriter.WriteLine("<TestDate>{0}</TestDate>", TestDate);
                // Assembly info.
                System.Reflection.Assembly entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
                if (entryAssembly != null)
                {
                    streamWriter.WriteLine("<EntryAssembly>{0}</EntryAssembly>", entryAssembly.GetName().Name);
                }
                System.Reflection.Assembly[] loadedAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();

                foreach (System.Reflection.Assembly assembly in loadedAssemblies)
                {
                    System.String assemblyInfo = assembly.GetName().Name + " (" + assembly.GetName().Version.ToString() + ")";
                    streamWriter.WriteLine("<Assembly>{0}</Assembly>", assemblyInfo);
                }

                streamWriter.WriteLine("</SessionDetails>");
            }

            return true;
        }

        /*
		private void AddAssemblyInfoAndReferencedAssemblyInfo(StreamWriter streamWriter, System.Reflection.Assembly assembly, String prefix)
		{
			String xmlValue = prefix + assembly.GetName().Name + " (" + assembly.GetName().Version.ToString() + ")";

			streamWriter.WriteLine("<AssemblyInfo>{0}</AssemblyInfo>", xmlValue);

			foreach (System.Reflection.Assembly referencedAssembly in assembly.GetReferencedAssemblies(
		}
		*/
    }

    /// <summary>
    /// Counters to maintain count of messages at various severity levels. 
    /// </summary>
    public class Counters : IDvtDetailToXml
    {
        /// <summary>
        /// Total Number of Errors.
        /// </summary>
        public System.UInt32 NrOfErrors
        {
            get
            {
                return
                    this.NrOfValidationErrors +
                    this.NrOfUserErrors +
                    this.NrOfGeneralErrors;
            }
        }

        /// <summary>
        /// Total Number of Warnings.
        /// </summary>
        public System.UInt32 NrOfWarnings
        {
            get
            {
                return
                    this.NrOfValidationWarnings +
                    this.NrOfUserWarnings +
                    this.NrOfGeneralWarnings;
            }
        }

        /// <summary>
        /// Number of Validation Errors.
        /// </summary>
        /// <remarks>
        /// These are errors related to the conformance to the Information Object Definitions.
        /// </remarks>
        public System.UInt32 NrOfValidationErrors
        {
            get
            {
                return _NrOfValidationErrors;
            }
            set
            {
                _NrOfValidationErrors = value;
            }
        }
        private System.UInt32 _NrOfValidationErrors;

        /// <summary>
        /// Number of Validation Warnings.
        /// </summary>
        /// <remarks>
        /// These are warnings related to the conformance to the Information Object Definitions (IODs).
        /// </remarks>
        public System.UInt32 NrOfValidationWarnings
        {
            get
            {
                return _NrOfValidationWarnings;
            }
            set
            {
                _NrOfValidationWarnings = value;
            }
        }
        private System.UInt32 _NrOfValidationWarnings;

        /// <summary>
        /// Number of General Errors.
        /// </summary>
        /// <remarks>
        /// These are errors related to the communication and other aspects outside the scope of IODs.
        /// </remarks>
        public System.UInt32 NrOfGeneralErrors
        {
            get
            {
                return _NrOfGeneralErrors;
            }
            set
            {
                _NrOfGeneralErrors = value;
            }
        }
        private System.UInt32 _NrOfGeneralErrors;

        /// <summary>
        /// Number of General Warnings.
        /// </summary>
        /// <remarks>
        /// These are warnings related to the communication and other aspects outside the scope of IODs.
        /// </remarks>
        public System.UInt32 NrOfGeneralWarnings
        {
            get
            {
                return _NrOfGeneralWarnings;
            }
            set
            {
                _NrOfGeneralWarnings = value;
            }
        }
        private System.UInt32 _NrOfGeneralWarnings;

        /// <summary>
        /// Number of User Errors.
        /// </summary>
        /// <remarks>
        /// <p>
        /// These are errors explicitly invoked by the user of the Dvtk component.
        /// The user can use Dvtk to report his/her errors into the output results.
        /// </p>
        /// <p>
        /// The user can use the return codes of Dvtk calls to build his/her own test logic.
        /// </p>
        /// </remarks>
        public System.UInt32 NrOfUserErrors
        {
            get
            {
                return _NrOfUserErrors;
            }
            set
            {
                _NrOfUserErrors = value;
            }
        }
        private System.UInt32 _NrOfUserErrors;

        /// <summary>
        /// Number of User Warnings.
        /// </summary>
        /// <remarks>
        /// <p>
        /// These are warnings explicitly invoked by the user of the Dvtk component.
        /// The user can use Dvtk to report his/her errors into the output results.
        /// </p>
        /// <p>
        /// The user can use the return codes of Dvtk calls to build his/her own test logic.
        /// </p>
        /// </remarks>
        public System.UInt32 NrOfUserWarnings
        {
            get
            {
                return _NrOfUserWarnings;
            }
            set
            {
                _NrOfUserWarnings = value;
            }
        }
        private System.UInt32 _NrOfUserWarnings;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<ValidationCounters>");
                streamWriter.WriteLine("<NrOfValidationErrors>{0}</NrOfValidationErrors>", NrOfValidationErrors.ToString());
                streamWriter.WriteLine("<NrOfValidationWarnings>{0}</NrOfValidationWarnings>", NrOfValidationWarnings.ToString());
                streamWriter.WriteLine("<NrOfGeneralErrors>{0}</NrOfGeneralErrors>", NrOfGeneralErrors.ToString());
                streamWriter.WriteLine("<NrOfGeneralWarnings>{0}</NrOfGeneralWarnings>", NrOfGeneralWarnings.ToString());
                streamWriter.WriteLine("<NrOfUserErrors>{0}</NrOfUserErrors>", NrOfUserErrors.ToString());
                streamWriter.WriteLine("<NrOfUserWarnings>{0}</NrOfUserWarnings>", NrOfUserWarnings.ToString());
                if ((NrOfValidationErrors == 0) &&
                    (NrOfGeneralErrors == 0) &&
                    (NrOfUserErrors == 0))
                {
                    streamWriter.WriteLine("<ValidationTest>PASSED</ValidationTest>");
                }
                else
                {
                    streamWriter.WriteLine("<ValidationTest>FAILED</ValidationTest>");
                }
                streamWriter.WriteLine("</ValidationCounters>");
            }

            return true;
        }
    }

    /// <summary>
    /// Results node for a performed send action.
    /// </summary>
    public class Send : IDvtDetailToXml
    {

        /// <summary>
        /// The item to Send.
        /// </summary>
        public object Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        private object _Item;

        /// <summary>
        /// Set <see cref="Send.Item"/> to a <see cref="DvtkData.Dimse.DicomMessage"/>
        /// </summary>
        public DicomMessage DicomMessage
        {
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Set <see cref="Send.Item"/> to a <see cref="DvtkData.Dul.DulMessage"/>
        /// </summary>
        public DulMessage DulMessage
        {
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter == null) return true;

            bool result = false;
            streamWriter.WriteLine("<Send>");
            if (Item is DicomMessage)
            {
                DicomMessage dicomMessage = (DicomMessage)Item;
                result = dicomMessage.DvtDetailToXml(streamWriter, level);
            }
            else
            {
                DulMessage dulMessage = (DulMessage)Item;
                result = dulMessage.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</Send>");

            return result;
        }
    }

    /// <summary>
    /// Results node for a performed receive action.
    /// </summary>
    public class Receive : IDvtDetailToXml
    {

        /// <summary>
        /// The item to Receive.
        /// </summary>
        public object Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        private object _Item;

        /// <summary>
        /// Set <see cref="Receive.Item"/> to a <see cref="DvtkData.Dimse.DicomMessage"/>
        /// </summary>
        public DicomMessage DicomMessage
        {
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Set <see cref="Receive.Item"/> to a <see cref="DvtkData.Dul.DulMessage"/>
        /// </summary>
        public DulMessage DulMessage
        {
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter == null) return true;

            bool result = false;
            streamWriter.WriteLine("<Receive>");
            if (Item is DicomMessage)
            {
                DicomMessage dicomMessage = (DicomMessage)Item;
                result = dicomMessage.DvtDetailToXml(streamWriter, level);
            }
            else
            {
                DulMessage dulMessage = (DulMessage)Item;
                result = dulMessage.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</Receive>");

            return result;
        }
    }

    /// <summary>
    /// Results node for a performed import action.
    /// </summary>
    public class Import : IDvtDetailToXml
    {

        /// <summary>
        /// Dicom message.
        /// </summary>
        public DicomMessage DicomMessage
        {
            get
            {
                return _DicomMessage;
            }
            set
            {
                _DicomMessage = value;
            }
        }
        private DicomMessage _DicomMessage;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter == null) return true;

            bool result = false;
            streamWriter.WriteLine("<Import>");
            result = DicomMessage.DvtDetailToXml(streamWriter, level);
            streamWriter.WriteLine("</Import>");

            return result;
        }
    }

    /// <summary>
    /// Results node for a performed display action.
    /// </summary>
    public class Display : IDvtDetailToXml
    {

        /// <summary>
        /// The item to Display.
        /// </summary>
        public object Item;

        /// <summary>
        /// Set <see cref="Display.Item"/> to a <see cref="DvtkData.Results.Display.Attribute"/>
        /// </summary>
        public Attribute Attribute
        {
            set
            {
                this.Item = value;
            }
            get
            {
                return this.Item as Attribute;
            }
        }

        /// <summary>
        /// Set <see cref="Display.Item"/> to a <see cref="DvtkData.Results.Display.DicomFile"/>
        /// </summary>
        public DicomFile DicomFile
        {
            set
            {
                this.Item = value;
            }
            get
            {
                return this.Item as DicomFile;
            }
        }

        /// <summary>
        /// Set <see cref="Display.Item"/> to a <see cref="DvtkData.Results.Display.DicomMessage"/>
        /// </summary>
        public DicomMessage DicomMessage
        {
            set
            {
                this.Item = value;
            }
            get
            {
                return this.Item as DicomMessage;
            }
        }

        /// <summary>
        /// Set <see cref="Display.Item"/> to a <see cref="DvtkData.Results.Display.DataSet"/>
        /// </summary>
        public DataSet DataSet
        {
            set
            {
                this.Item = value;
            }
            get
            {
                return this.Item as DataSet;
            }
        }

        /// <summary>
        /// Set <see cref="Display.Item"/> to a <see cref="DvtkData.Results.Display.SequenceItem"/>
        /// </summary>
        public SequenceItem SequenceItem
        {
            set
            {
                this.Item = value;
            }
            get
            {
                return this.Item as SequenceItem;
            }
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter == null) return true;
            bool result = false;
            streamWriter.WriteLine("<Display>");
            if (Item is DicomMessage)
            {
                DicomMessage dicomMessage = (DicomMessage)Item;
                result = dicomMessage.DvtDetailToXml(streamWriter, level);
            }
            else if (Item is DataSet)
            {
                DataSet dataSet = (DataSet)Item;
                result = dataSet.DvtDetailToXml(streamWriter, level);
            }
            else if (Item is Attribute)
            {
                Attribute attribute = (Attribute)Item;
                result = attribute.DvtDetailToXml(streamWriter, level);
            }
            else if (Item is DicomFile)
            {
                DicomFile dicomFile = (DicomFile)Item;
                result = dicomFile.DvtDetailToXml(streamWriter, level);
            }
            else if (Item is SequenceItem)
            {
                SequenceItem sequenceItem = (SequenceItem)Item;
                result = sequenceItem.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</Display>");
            return result;
        }
    }

    /// <summary>
    /// Type of emulator run in the emulator session.
    /// </summary>
    public enum ScpEmulatorType
    {
        /// <summary>
        /// Print SCP Emulator - emulate command.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of Print SCP for the Print Definition File(s) loaded.
        /// </p>
        /// <p>
        /// Before running this emulator, load the proper definition files corresponding to the
        /// printing. For instance: Basic Grayscale Print Management Meta SOP Class.
        /// </p>
        /// <p>
        /// The Print SCP Emulator supports the Verification SOP Class.
        /// </p>
        /// <p>
        /// If the Print Job SOP Class Definition File is loaded, DVT will support the Print Job Service Class.
        /// </p>
        /// <p>
        /// If SLIDE, SUPERSLIDE or CUSTOM Image Display Formats [DICOM tag (2010,0010)] are to be sent, 
        /// the number of images for each format must be defined in the ImageDisplayFormat.def definition 
        /// file and the file must be loaded.
        /// This file must also be modified and loaded to indicate the number of annotation boxes defined if the 
        /// Annotation Display Format ID [DICOM tag (2010,0030)] is to be used.
        /// </p>
        /// </remarks>
        Printing,
        /// <summary>
        /// Storage SCP Emulator - emulate command.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of Storage SCP for the Storage Definition File(s) loaded.
        /// </p>
        /// <p>
        /// Before running this emulator, load the proper definition files corresponding to the
        /// storage SCP.
        /// </p>
        /// <p>
        /// The Storage Emulator SCP supports the Verification SOP Class.
        /// </p>
        /// <p>
        /// The Storage Emulator SCP supports the Storage Commitment PUSH Model SOP Class.
        /// </p>
        /// </remarks>
        Storage,
        /// <summary>
        /// Storage Commit SCP Emulator - emulate command.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of Storage Commit SCP.
        /// </p>
        /// <p>
        /// The Storage Commit Emulator SCP supports the Storage Commitment PUSH Model SOP Class.
        /// </p>
        /// </remarks>
        StorageCommit,
        /// <summary>
        /// Mpps Emulator - emulate command.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of MPPS SCP for the MPPS Definition File(s) loaded.
        /// </p>
        /// <p>
        /// Before running this emulator, load the proper definition files corresponding to the
        /// mpps SCP.
        /// </p>
        /// <p>
        /// The MPPS Emulator SCP supports the Verification SOP Class.
        /// </p>
        /// </remarks>
        Mpps,
        /// <summary>
        /// Worklist Emulator - emulate command.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of Worklist SCP for the Worklist Definition File(s) loaded.
        /// </p>
        /// <p>
        /// Before running this emulator, load the proper definition files corresponding to the
        /// worklist SCP.
        /// </p>
        /// <p>
        /// The Worklist Emulator SCP supports the Verification SOP Class.
        /// </p>
        /// </remarks>
        Worklist,
        /// <summary>
        /// Query/Retrieve SCP Emulator - emulate command.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of Query Retrieve SCP for the Query/Retrieve Definition File(s) loaded.
        /// </p>
        /// <p>
        /// Before running this emulator, load the proper definition files corresponding to the
        /// query/retrieve SCP.
        /// </p>
        /// <p>
        /// The Query Retrieve Emulator SCP supports the Verification SOP Class.
        /// </p>
        /// </remarks>
        QueryRetrieve,
        /// <summary>
        /// Unspecified emulator
        /// </summary>
        Unknown,
    }

    /// <summary>
    /// Type of emulator run in the emulator session.
    /// </summary>
    public enum ScuEmulatorType
    {
        /// <summary>
        /// Storage SCU Emulator - emulate command.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of Storage SCU for the Storage Definition File(s) loaded.
        /// </p>
        /// <p>
        /// Before running this emulator, load the proper definition files corresponding to the
        /// storage SCU.
        /// </p>
        /// </remarks>
        Storage,
        /// <summary>
        /// Unspecified emulator
        /// </summary>
        Unknown,
    }

    /// <summary>
    /// Results node for a performed media write action.
    /// </summary>
    public class MediaWrite : IDvtDetailToXml
    {

        /// <summary>
        /// File name
        /// </summary>
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }
        private string _FileName;

        /// <summary>
        /// Dicom file contents
        /// </summary>
        public DicomFile DicomFile
        {
            get
            {
                return _DicomFile;
            }
            set
            {
                _DicomFile = value;
            }
        }
        private DicomFile _DicomFile;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter == null) return true;
            bool result = false;
            streamWriter.WriteLine("<Write>");
            streamWriter.WriteLine("<Filename>{0}</Filename>", FileName);
            result = DicomFile.DvtDetailToXml(streamWriter, level);
            streamWriter.WriteLine("</Write>");
            return result;
        }
    }

    /// <summary>
    /// Results node for a performed media read action.
    /// </summary>
    public class MediaRead : IDvtDetailToXml
    {

        /// <summary>
        /// File name
        /// </summary>
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }
        private string _FileName;

        /// <summary>
        /// Dicom file contents
        /// </summary>
        public DicomFile DicomFile
        {
            get
            {
                return _DicomFile;
            }
            set
            {
                _DicomFile = value;
            }
        }
        private DicomFile _DicomFile;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter == null) return true;
            bool result = false;
            streamWriter.WriteLine("<Read>");
            streamWriter.WriteLine("<Filename>{0}</Filename>", FileName);
            result = DicomFile.DvtDetailToXml(streamWriter, level);
            streamWriter.WriteLine("</Read>");
            return result;
        }
    }
    /// <summary>
    /// Xml node for a Dicom Script (.DS) script CREATE command.
    /// </summary>
    public class DicomScriptCreate
        : DicomScriptCreateSetDeleteActionBase
    {
        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<Create>");
                base.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</Create>");
            }
            return true;
        }
    }
    /// <summary>
    /// Xml node for a Dicom Script (.DS) script SET command.
    /// </summary>
    public class DicomScriptSet
        : DicomScriptCreateSetDeleteActionBase
    {
        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<Set>");
                base.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</Set>");
            }
            return true;
        }
    }
    /// <summary>
    /// Xml node for a Dicom Script (.DS) script DELETE command.
    /// </summary>
    public class DicomScriptDelete
        : DicomScriptCreateSetDeleteActionBase
    {
        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<Delete>");
                base.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</Delete>");
            }
            return true;
        }
    }
    /// <summary>
    /// Abstract base class for Xml node for a Dicom Script (.DS) script CREATE/SET/DELETE commands.
    /// </summary>
    public abstract class DicomScriptCreateSetDeleteActionBase : IDvtDetailToXml
    {
        /// <summary>
        /// Command field
        /// </summary>
        public DimseCommand CommandField
        {
            get
            {
                return this._CommandSet.CommandField;
            }
        }
        /// <summary>
        /// List of attributes SET in the set.
        /// </summary>
        /// <remarks>
        /// Known bug: The command field attribute 0x0000, 0x0010 is always listed
        /// even if it is not SET.
        /// </remarks>
        public CommandSet CommandSet
        {
            get
            {
                return _CommandSet;
            }
            set
            {
                _CommandSet = value;
            }
        }
        private CommandSet _CommandSet;
        /// <summary>
        /// Reference Identifier for the command set in the dataware house.
        /// </summary>
        public System.String CommandSetRefId
        {
            get
            {
                return _CommandSetReferenceIdentifier;
            }
            set
            {
                _CommandSetReferenceIdentifier = value;
            }
        }
        private System.String _CommandSetReferenceIdentifier;
        /// <summary>
        /// List of attributes SET in the set.
        /// </summary>
        public DataSet DataSet
        {
            get
            {
                return _DataSet;
            }
            set
            {
                _DataSet = value;
            }
        }
        private DataSet _DataSet;
        /// <summary>
        /// Reference Identifier for the data set in the dataware house.
        /// </summary>
        public System.String DataSetRefId
        {
            get
            {
                return _DataSetReferenceIdentifier;
            }
            set
            {
                _DataSetReferenceIdentifier = value;
            }
        }
        private System.String _DataSetReferenceIdentifier;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public virtual bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (this._CommandSet != null)
            {
                streamWriter.WriteLine("<CommandSetRefId>{0}</CommandSetRefId>", CommandSetRefId);
                CommandSet.DvtDetailToXml(streamWriter, level);
            }
            if (this._DataSet != null)
            {
                streamWriter.WriteLine("<DataSetRefId>{0}</DataSetRefId>", DataSetRefId);
                DataSet.DvtDetailToXml(streamWriter, level);
            }
            return true;
        }
    }

    /// <summary>
    /// Display a byte dump of DVT socket communication.
    /// </summary>
    /// <remarks>
    /// This byte dump information may be used to debug communication problems.
    /// </remarks>
    public class ByteDump : IDvtDetailToXml
    {
        /// <summary>
        /// Literal description for the byte dump.
        /// </summary>
        public System.String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
        private System.String _Description;

        /// <summary>
        /// The bytes dumped.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public System.Byte[] Bytes
        {
            get
            {
                return _Bytes;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _Bytes = value;
            }
        }
        private System.Byte[] _Bytes;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (streamWriter == null) return true;
            streamWriter.WriteLine("<ByteDump>");
            streamWriter.WriteLine("<Description>{0}</Description>", Description);
            streamWriter.Write("<ByteStream>");
            foreach (System.Byte byteValue in Bytes)
            {
                string byteValueString = byteValue.ToString("X");
                while (byteValueString.Length < 2)
                {
                    byteValueString = "0" + byteValueString;
                }
                streamWriter.Write(byteValueString);
            }
            streamWriter.WriteLine("</ByteStream>");
            streamWriter.WriteLine("</ByteDump>");
            return true;
        }
    }
}