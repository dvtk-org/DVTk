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

namespace Dvtk.Sessions
{
    using System;
    //
    // Aliases for types
    //
    using FileName = System.String;
    using Directory = System.String;

    /// <summary>
    /// Storage mode
    /// </summary>
    public enum StorageMode
    {
        /// <summary>
        /// Any received storage IOD will be stored in 
        /// the DICOM Media Storage File format (i.e., including the File Prefix, 
        /// DICOM Preamble and File Meta Information) with a file extension .DCM. 
        /// <p>
        /// Any OB/OF/OW data associated with the stored media will be stored in .PIX files.
        /// </p>
        /// </summary>
        AsMedia,
        /// <summary>
        /// Any received storage IOD will be stored in 
        /// the DICOM Media Storage File format (i.e., including the File Prefix, 
        /// DICOM Preamble and File Meta Information) with a file extension .DCM. 
        /// </summary>
        AsMediaOnly,
        /// <summary>
        /// Any received storage IOD will be stored in 
        /// a “raw” format with only the Dataset being saved. 
        /// The file extension is .RAW. 
        /// <p>
        /// Any OB/OF/OW data associated with the stored dataset will be stored in .PIX files.
        /// </p>
        /// </summary>
        AsDataSet,
        /// <summary>
        /// DO NOT USE! INTERNAL USE ONLY!
        /// </summary>
        TemporaryPixelOnly,
        /// <summary>
        /// Do not store any received storage IOD. 
        /// <p>
        /// Any OB/OF/OW attributes will be displayed with the value “DATA NOT STORED” 
        /// when this storage mode is selected.
        /// </p>
        /// </summary>
        NoStorage,
    };
    /// <summary>
    /// Access to validation settings.
    /// </summary>
    public interface IValidationSettings
    {
        /// <summary>
        /// The StrictValidation option, 
        /// when enabled (set true), 
        /// causes DVT to perform a strict check between the 
        /// received ACSE and DICOM messages and those programmed (expected) in the DICOMScripts. 
        /// If any parameter of the received message does not match that programmed, then DVT reports a FAILED validation and aborts further DICOMScript interpretation.
        /// If the STRICT-VALIDATION flag is disabled (set false), 
        /// then DVT will WARN the User when a mismatch occurs between received 
        /// and programmed values. 
        /// DVT will continue interpreting the DICOMScript.
        /// </summary>
        /// <remarks>
        /// This feature is particularly useful when the User wishes to test 
        /// a specific (range of) SOP Class(es). 
        /// Any additional SOP Classes proposed by the Product 
        /// will be automatically rejected by DVT, allowing the User 
        /// to concentrate on the SOP Class(es) of interest to the Test Scenario.
        /// </remarks>
        Boolean StrictValidation { get; set; }

        /// <summary>
        /// Indicate whether DVT should produce Detailed Validation Results or not.
        /// </summary>
        Boolean DetailedValidationResults { get; set; }

        /// <summary>
        /// Indicate whether DVT should produce Detailed Validation Results or not.
        /// </summary>
        Boolean TestLogValidationResults { get; set; }

        /// <summary>
        ///  Indicate whether DVT should produce Summary Validation Results or not.
        /// </summary>
        Boolean SummaryValidationResults { get; set; }

        /// <summary>
        ///  Indicate whether DVT should include Type 3 attributes in the validation
        ///  results that are not present in the dataset being validated or not.
        /// </summary>
        Boolean IncludeType3NotPresentInResults { get; set; }

        /// <summary>
        ///  Indicate whether DVT should include Type 3 attributes in the validation
        ///  results that are not present in the dataset being validated or not.
        /// </summary>
        Boolean DumpAttributesOfRefFiles { get; set; }

        /// <summary>
        /// Indicate whether DVT should try to look up any attribute decoded with an
        /// explicit VR of UN for the real VR - and use this real VR for further attribute
        /// value decoding and validation.
        /// </summary>
        Boolean UnVrDefinitionLookUp { get; set; }

        /// <summary>
        /// Determines if the referenced files should be validated when validating a DICOMDIR. 
        /// </summary>
        Boolean ValidateReferencedFile { get; set; }

        /// <summary>
        /// Determines if conditional text will be displayed in summary result. 
        /// </summary>
        Boolean DisplayConditionText { get; set; }
    }
    /// <summary>
    /// Access to general session settings.
    /// </summary>
    public interface IGeneralSessionSettings
    {
        /// <summary>
        /// File name with extension <c>.ses</c> used during load and save.
        /// </summary>
        FileName SessionFileName { get; set; }
        /// <summary>
        /// Directory used to store results output.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">Directory may not be an empty string. Use ".\" for current directory."</exception>
        Directory ResultsRootDirectory { get; set; }
        /// <summary>
        /// Directory used to store data generated by DVT - DCM files, etc.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">Directory may not be an empty string. Use ".\" for current directory."</exception>
        Directory DataDirectory { get; set; }

        /// <summary>
        /// Date of test.
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        DateTime Date { get; set; }
        /// <summary>
        /// Title for the session.
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        String SessionTitle { get; set; }
        /// <summary>
        /// Identification for the session.
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        UInt16 SessionId { get; set; }
        /// <summary>
        /// Software versions for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        String SoftwareVersions { get; set; }
        /// <summary>
        /// Name of the tester.
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        String TestedBy { get; set; }
        /// <summary>
        /// If the storage mode is set to as-media, 
        /// DVT will store a received Image Dataset (Group 0008 up to and including Group 7FE0) 
        /// in a file with the extension .DCM. 
        /// </summary>
        /// <remarks>
        /// <p>
        /// The Image Dataset is stored in the .DCM file in the format 
        /// described in DICOM - part 10. 
        /// </p>
        /// <p>
        /// The File Preamble, DICOM Prefix and File Meta Information are added by DVT.
        /// </p>
        /// <p>
        /// The filename is generated from the Session ID and a media storage file index. 
        /// The filename used for the media storage is recorded in the corresponding Results File. 
        /// The following filenames are generated:
        /// <c>nnnIiiii.DCM</c> where <c>nnn</c> is the Session ID, 
        /// <c>I</c> signifies image information and <c>iiii</c> is the file index.
        /// </p>
        /// Examples:
        /// <list type="bullet">
        /// <item>
        /// <term>1I0123.DCM</term>
        /// <description>Media Storage File 123 created in Test Session 1.</description>
        /// </item>
        /// <item>
        /// <term>4I0012.DCM</term>
        /// <description>Media Storage File 12 created in Test Session 4.</description>
        /// </item>
        /// </list>
        /// </remarks>
        StorageMode StorageMode { get; set; }
        /// <summary>
        /// Manufacturer for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        String Manufacturer { get; set; }
        /// <summary>
        /// Model name for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        String ModelName { get; set; }
        /// <summary>
        /// Set LogLevel Enabled flags.
        /// </summary>
        /// <summary>
        /// Enabled loglevels
        /// </summary>
        LogLevelFlags LogLevelFlags
        {
            get;
            set;
        }
        /// <summary>
        /// Determines if a directory is created when this does not yet existing, when writing a Dicom file.
        /// </summary>
        Boolean AutoCreateDirectory { get; set; }
        /// <summary>
        /// Determines of execution should continue when an error has occured. 
        /// </summary>
        Boolean ContinueOnError { get; set; }
    }
    /// <summary>
    /// Possible log level flags.
    /// </summary>
    [Flags]
    public enum LogLevelFlags : uint // System.UInt32
    {
        /// <summary>
        /// None
        /// </summary>
        None = 1 << 0,            // 0x00000001
        /// <summary>
        /// Error
        /// </summary>
        Error = 1 << 1,           // 0x00000002
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 1 << 2,           // 0x00000004
        /// <summary>
        /// Warning
        /// </summary>
        Warning = 1 << 3,         // 0x00000008
        /// <summary>
        /// Info
        /// </summary>
        Info = 1 << 4,            // 0x00000010
        /// <summary>
        /// Script
        /// </summary>
        Script = 1 << 5,          // 0x00000020
        /// <summary>
        /// Script name
        /// </summary>
        ScriptName = 1 << 6,      // 0x00000040
        /// <summary>
        /// PduBytes
        /// </summary>
        PduBytes = 1 << 7,        // 0x00000080
        /// <summary>
        /// DulpFsm
        /// </summary>
        DulpFsm = 1 << 8,         // 0x00000100
        /// <summary>
        /// ImageRelation
        /// </summary>
        ImageRelation = 1 << 9,   // 0x00000200
        /// <summary>
        /// Print
        /// </summary>
        Print = 1 << 10,           // 0x00000400
        /// <summary>
        /// Label
        /// </summary>
        Label = 1 << 11,          // 0x00000800
        /// <summary>
        /// Media filename
        /// </summary>
        MediaFilename = 1 << 12,	// 0x00001000
    }
}