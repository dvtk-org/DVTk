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
    using Dvtk;
    using DvtkData.Media;                               // Data classes
    //
    // Aliases for types
    //
    using SessionFileName = System.String;
    using MediaFileName = System.String;

    using System;
    using System.Threading;

    /// <summary>
    /// Access to commands that read and write media files.
    /// </summary>
    public interface IMediaStorage
    {
        /// <summary>
        /// Asynchronously begin DICOMDIR Generation.
        /// </summary>
        /// <param name="mediaFileNames"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        System.IAsyncResult BeginGenerationDICOMDIR(System.String[] mediaFileNames, AsyncCallback cb);

        /// <summary>
        /// Generate a DICOMDIR for the given media file names.
        /// </summary>
        /// <param name="mediaFileNames">List of media fully qualified file names to use for DICOMDIR generation.</param>
        /// <returns><see langword="false"/> if the generation process failed.</returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>mediaFileNames</c> is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">
        /// Argument <c>mediaFileNames</c> is an empty array of media file names.
        /// </exception>
        bool GenerateDICOMDIR(System.String[] mediaFileNames);

        /// <summary>
        /// End asynchronous DICOMDIR Generation.
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        bool EndGenerationDICOMDIR(IAsyncResult ar);
    }
    /// <summary>
    /// Access to commands that validate media related dicom-file objects.
    /// </summary>
    public interface IMediaValidation
    {
        /// <summary>
        /// Validate a media related dicom-file object
        /// </summary>
        /// <param name="file">dicom file object to be validated</param>
        /// <param name="validationControlFlags">Control flags to steer the validation process</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>file</c> is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">
        /// Argument <c>validationControlFlags</c> is not set to 
        /// <see cref="ValidationControlFlags.UseReferences"/>.
        /// </exception>
        bool Validate(DicomFile file, ValidationControlFlags validationControlFlags);
        bool Validate(DicomFile file, ValidationControlFlags validationControlFlags, string fileName);
    }
    /// <summary>
    /// Validation of a Media Storage File (.DCM or other).
    /// </summary>
    /// <remarks>
    /// <p>
    /// Before running this validation, load the proper definition files corresponding to the
    /// storage SOP classes.
    /// </p>
    /// </remarks>
    public interface IMediaValidator
    {
        /// <summary>
        /// Validate Media Storage Files.
        /// </summary>
        /// <remarks>
        /// Typically these files have the file-extension DCM. DVT does not check the file-extension.
        /// The file should have an internal byte-prefix with byte-values 'DICM'.
        /// </remarks>
        /// <param name="mediaFileNames">List of media fully qualified file names to validate.</param>
        /// <returns><see langword="false"/> if the validation process failed.</returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>mediaFileNames</c> is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">
        /// Argument <c>mediaFileNames</c> is an empty array of media file names.
        /// </exception>
        System.Boolean ValidateMediaFiles(System.String[] mediaFileNames);

        /// <summary>
        /// Validate Media Storage File.
        /// </summary>
        /// <remarks>
        /// Typically this file should have the file-extension DCM. DVT does not check the file-extension.
        /// The file can contain one of three types of data:
        /// a) DICOM Media File with File Meta Information - "Part 10 format".
        /// b) DIMSE CommandSet
        /// c) DICOM DataSet
        /// 
        /// The file content is defined by the fileContentType argument.
        /// If the file is either a DIMSE CommandSet or DataSet then the additional arguments sopClassUid,
        /// sopInstanceUid and transferSyntaxUid are expected to be defined.
        /// 
        /// </remarks>
        /// <param name="mediaFileName">List of media fully qualified file name to validate.</param>
        /// <param name="fileContentType">Description of the file content.</param>
        /// <param name="sopClassUid">Sop Class Uid of file content.</param>
        /// <param name="sopInstanceUid">Sop Instance Uid of file content.</param>
        /// <param name="transferSyntaxUid">Transfer Syntax Uid of file content.</param>
        /// <returns><see langword="false"/> if the validation process failed.</returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>mediaFileNames</c> is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">
        /// Argument <c>mediaFileNames</c> is an empty array of media file names.
        /// </exception>
        System.Boolean ValidateMediaFiles(System.String mediaFileName,
            MediaFileContentType fileContentType,
            System.String sopClassUid,
            System.String sopInstanceUid,
            System.String transferSyntaxUid);

        /// <summary>
        /// Validate Media Storage Files.
        /// </summary>
        /// <remarks>
        /// Typically these files have the file-extension DCM. DVT does not check the file-extension.
        /// The file should have an internal byte-prefix with byte-values 'DICOM'.
        /// 
        /// The directoryRecordsToFilter parameter can be any combination of the following Directory Record Types:
        ///  
        /// "ROOT"
        /// "PATIENT"
        /// "STUDY"
        /// "SERIES"
        /// "IMAGE"
        /// "OVERLAY"
        /// "MODALITY LUT"
        /// "VOI LUT"
        /// "CURVE"
        /// "STORED PRINT"
        /// "RT DOSE"
        /// "RT STRUCTURE SET"
        /// "RT PLAN"
        /// "RT TREAT RECORD"
        /// "PRESENTATION"
        /// "WAVEFORM"
        /// "SR DOCUMENT"
        /// "KEY OBJECT DOC"
        /// "TOPIC"
        /// "VISIT""RESULTS"
        /// "INTERPRETATION"
        /// "STUDY COMPONENT"
        /// "PRIVATE"
        /// "PRINT QUEUE"
        /// "FILM SESSION"
        /// "FILM BOX"
        /// "IMAGE BOX"
        /// 
        /// To include more than one record type - delimit the record types with the pipe "|" character
        /// example: "IMAGE|PRIVATE|PRESENTATION" will filter on these 3 record types.
        /// 
        /// </remarks>
        /// <param name="mediaFileNames">List of media fully qualified file names to validate.</param>
        /// <param name="directoryRecordsToFilter">Pipe delimited string indicating the Directory Records that should be filtered.</param>
        /// <param name="numberDirectoryRecordsToFilter">Number of Directory Records to validate before filtering records of the same type out.</param>
        /// <returns><see langword="false"/> if the validation process failed.</returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>mediaFileNames</c> is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">
        /// Argument <c>mediaFileNames</c> is an empty array of media file names.
        /// </exception>
        System.Boolean ValidateMediaFiles(System.String[] mediaFileNames, System.String directoryRecordsToFilter, int numberDirectoryRecordsToFilter);

        /// <summary>
        /// Asynchronously begin ValidateMediaFiles.
        /// </summary>
        /// <param name="mediaFileNames"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        System.IAsyncResult BeginValidateMediaFiles(System.String[] mediaFileNames, AsyncCallback cb);

        /// <summary>
        /// End asynchronous ValidateMediaFiles
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        bool EndValidateMediaFiles(IAsyncResult ar);
    }

    /// <summary>
    /// Enumerated values for the content of the Media File.
    /// </summary>
    public enum MediaFileContentType
    {
        /// <summary>
        /// DICOM File
        /// </summary>
        MediaFile,
        /// <summary>
        /// CommandSet
        /// </summary>
        CommandSet,
        /// <summary>
        /// DataSet
        /// </summary>
        DataSet
    }

    /// <summary>
    /// Summary description for Session.
    /// </summary>
    public class MediaSession
        : Session
        /* Not available: IDataSetEncodingSettings applies to writes. Media only reads.
         * , IDataSetEncodingSettings
         */
        , IMediaStorage
        , IMediaValidation
        , IMediaValidator
    /* Not available: IConfigurableDvt, IConfigurableSut, ISecure applies to network iso media.
     * , IConfigurableDvt
     * , IConfigurableSut
     * , ISecure
     */
    {
        // Validation services

        private Wrappers.MMediaSession m_adaptee = null;
        override internal Wrappers.MBaseSession m_MBaseSession
        {
            get { return m_adaptee; }
        }
        internal Wrappers.MMediaSession m_MMediaSession
        {
            get { return m_adaptee; }
        }
        //
        // Touch the AppUnloadListener abstract class to trigger its static-constructor.
        //
        static MediaSession()
        {
            AppUnloadListener.Touch();
        }
        /// <summary>
        /// Validate DICOM media files.
        /// </summary>
        /// <remarks>
        /// The Dicom Validation Tool (DVT) can also create and validate DICOM media files.
        /// </remarks>
        public MediaSession()
        {
            m_adaptee = new Wrappers.MMediaSession();
            Wrappers.MDisposableResources.AddDisposable(m_adaptee);
            _Initialize();
        }
        /// <summary>
        /// Finalizer
        /// </summary>
        ~MediaSession()
        {
            Wrappers.MDisposableResources.RemoveDisposable(m_adaptee);
            m_adaptee = null;
        }
        internal MediaSession(Wrappers.MBaseSession adaptee)
        {
            if (adaptee == null) throw new System.ArgumentNullException("adaptee");
            // Check type
            m_adaptee = (Wrappers.MMediaSession)adaptee;
            Wrappers.MDisposableResources.AddDisposable(m_adaptee);
            _Initialize();
        }
        /// <summary>
        /// Load a new session from file.
        /// </summary>
        /// <param name="sessionFileName">file with extension <c>.ses</c>.</param>
        /// <returns>new session</returns>
        /// <remarks>
        /// Session settings may be written to a file with extension <c>.ses</c> by
        /// means of <see cref="Dvtk.Sessions.ISessionFileManagement"/>.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument <c>sessionFileName</c> is a <see langword="null"/> reference.</exception>
        static public MediaSession LoadFromFile(SessionFileName sessionFileName)
        {
            if (sessionFileName == null) throw new System.ArgumentNullException("sessionFileName");
            Session session = SessionFactory.TheInstance.Load(sessionFileName);
            System.Diagnostics.Trace.Assert(session is MediaSession);
            return (MediaSession)session;
        }

        #region IMediaStorage

        /// <summary>
        /// Asynchronous DICOMDIR Generation delegate.
        /// </summary>
        /// <remarks>
        /// The delegate must have the same signature as the method you want to call asynchronously.
        /// </remarks>
        private delegate bool AsyncDICOMDIRGenerationDelegate(System.String[] mediaFileNames);

        /// <summary>
        /// <see cref="IMediaStorage.BeginGenerationDICOMDIR"/>
        /// </summary>
        public System.IAsyncResult BeginGenerationDICOMDIR(
            System.String[] mediaFileNames,
            AsyncCallback cb)
        {
            // Create the delegate.
            AsyncDICOMDIRGenerationDelegate dlgt = new AsyncDICOMDIRGenerationDelegate(this.GenerateDICOMDIR);

            // Initiate the asychronous call.
            object asyncState = dlgt;
            IAsyncResult ar = dlgt.BeginInvoke(
                mediaFileNames,
                cb,
                asyncState);
            return ar;
        }

        /// <summary>
        /// <see cref="IMediaStorage.GenerateDICOMDIR"/>
        /// </summary>
        public bool GenerateDICOMDIR(System.String[] mediaFileNames)
        {
            bool success = false;
            try
            {
                //
                // Check argument mediaFileNames
                //
                if (mediaFileNames == null) throw new System.ArgumentNullException("mediaFileNames");
                if (mediaFileNames.Length == 0) throw new System.ArgumentException("No media files specified", "mediaFileNames");
                foreach (System.String mediaFileName in mediaFileNames)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(mediaFileName);
                    if (!fileInfo.Exists) throw new System.ArgumentException();
                }
                //
                // Generate the DICOMDIR.
                //
                success = this.m_adaptee.GenerateDICOMDIR(mediaFileNames);
            }
            catch (System.Exception e)
            {
                String msg = String.Format("System.Exception in MediaSession.cs: {0}", e.Message);
                this.WriteError(msg);
            }

            return success;
        }

        /// <summary>
        /// <see cref="IMediaStorage.EndGenerationDICOMDIR"/>
        /// </summary>
        public bool EndGenerationDICOMDIR(
            IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncDICOMDIRGenerationDelegate dlgt = (AsyncDICOMDIRGenerationDelegate)ar.AsyncState;

            // Call EndInvoke to retrieve the results.
            bool retValue = dlgt.EndInvoke(ar);
            return retValue;
        }

        #endregion

        #region IMediaValidation
        /// <summary>
        /// <see cref="IMediaValidation.Validate"/>
        /// </summary>
        public bool Validate(DicomFile file, ValidationControlFlags validationControlFlags)
        {
            if (file == null) throw new System.ArgumentNullException("file");
            if ((validationControlFlags & ValidationControlFlags.UseReferences) != 0)
                throw new System.ArgumentException();
            //
            // Convert flags
            //
            Wrappers.ValidationControlFlags
                wrappersValidationControlFlags = Wrappers.ValidationControlFlags.None;
            if ((validationControlFlags & ValidationControlFlags.UseDefinitions) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseDefinitions;
            if ((validationControlFlags & ValidationControlFlags.UseReferences) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseReferences;
            if ((validationControlFlags & ValidationControlFlags.UseValueRepresentations) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseValueRepresentations;

            return this.m_adaptee.Validate(file, wrappersValidationControlFlags);
        }


        /// <summary>
        /// <see cref="IMediaValidation.Validate"/>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="validationControlFlags"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Validate(DicomFile file, ValidationControlFlags validationControlFlags, string fileName)
        {
            if (file == null) throw new System.ArgumentNullException("file");
            if ((validationControlFlags & ValidationControlFlags.UseReferences) != 0)
                throw new System.ArgumentException();
            if (fileName == null) throw new System.ArgumentNullException("fileName");
            //
            // Convert flags
            //
            Wrappers.ValidationControlFlags
                wrappersValidationControlFlags = Wrappers.ValidationControlFlags.None;
            if ((validationControlFlags & ValidationControlFlags.UseDefinitions) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseDefinitions;
            if ((validationControlFlags & ValidationControlFlags.UseReferences) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseReferences;
            if ((validationControlFlags & ValidationControlFlags.UseValueRepresentations) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseValueRepresentations;

            return this.m_adaptee.Validate(file, wrappersValidationControlFlags, fileName);
        }
        #endregion

        #region IMediaValidator

        private Mutex validateMediaFilesMutex = new Mutex();

        /// <summary>
        /// <see cref="IMediaValidator.ValidateMediaFiles(System.String[] )"/>
        /// </summary>
        public System.Boolean ValidateMediaFiles(System.String[] mediaFileNames)
        {
            bool success = false;
            try
            {
                // Wait until it is safe to enter.
                validateMediaFilesMutex.WaitOne();
                //
                // Check argument mediaFileNames
                //
                if (mediaFileNames == null) throw new System.ArgumentNullException("mediaFileNames");
                if (mediaFileNames.Length == 0) throw new System.ArgumentException("No media files specified", "mediaFileNames");
                foreach (System.String mediaFileName in mediaFileNames)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(mediaFileName);
                    if (!fileInfo.Exists) throw new System.ArgumentException();
                }
                //
                // Begin validation
                //
                success = this.m_adaptee.BeginMediaValidation();

                try
                {
                    //
                    // Validate each media file in turn.
                    //
                    foreach (System.String mediaFileName in mediaFileNames)
                    {
                        success &= this.m_adaptee.ValidateMediaFile(mediaFileName);
                        if (!success) break;
                    }
                }
                catch (System.Exception e)
                {
                    String msg = String.Format("System.Exception in MediaSession.cs: {0}", e.Message);
                    this.WriteError(msg);
                }
                success &= this.m_adaptee.EndMediaValidation();
            }
            catch (System.Exception e)
            {
                String msg = String.Format("System.Exception in MediaSession.cs: {0}", e.Message);
                this.WriteError(msg);
            }
            finally
            {
                // Release the Mutex.
                validateMediaFilesMutex.ReleaseMutex();
            }
            return success;
        }

        /// <summary>
        /// <see cref="IMediaValidator.ValidateMediaFiles(System.String , 
        /// MediaFileContentType ,
        /// System.String , 
        /// System.String , 
        /// System.String )"/>
        /// </summary>
        public System.Boolean ValidateMediaFiles(System.String mediaFileName,
            MediaFileContentType fileContentType,
            System.String sopClassUid,
            System.String sopInstanceUid,
            System.String transferSyntaxUid)
        {
            bool success = false;
            try
            {
                // Wait until it is safe to enter.
                validateMediaFilesMutex.WaitOne();
                //
                // Check argument mediaFileName
                //
                if (mediaFileName == null) throw new System.ArgumentNullException("mediaFileName");
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(mediaFileName);
                if (!fileInfo.Exists) throw new System.ArgumentException();

                //
                // Begin validation
                //
                success = this.m_adaptee.BeginMediaValidation();

                try
                {
                    // Set the media content type
                    Wrappers.FileContentType wrapperFileContentType = Wrappers.FileContentType.FileContentTypeMediaFile;
                    switch (fileContentType)
                    {
                        case MediaFileContentType.MediaFile:
                            wrapperFileContentType = Wrappers.FileContentType.FileContentTypeMediaFile;
                            break;
                        case MediaFileContentType.CommandSet:
                            wrapperFileContentType = Wrappers.FileContentType.FileContentTypeCommandSet;
                            break;
                        case MediaFileContentType.DataSet:
                            wrapperFileContentType = Wrappers.FileContentType.FileContentTypeDataSet;
                            break;
                        default: break;
                    }

                    //
                    // Validate the media file.
                    //
                    success &= this.m_adaptee.ValidateMediaFile(mediaFileName, wrapperFileContentType, sopClassUid, sopInstanceUid, transferSyntaxUid);
                }
                catch (System.Exception e)
                {
                    String msg = String.Format("System.Exception in MediaSession.cs: {0}", e.Message);
                    this.WriteError(msg);
                }
                success &= this.m_adaptee.EndMediaValidation();
            }
            catch (System.Exception e)
            {
                String msg = String.Format("System.Exception in MediaSession.cs: {0}", e.Message);
                this.WriteError(msg);
            }
            finally
            {
                // Release the Mutex.
                validateMediaFilesMutex.ReleaseMutex();
            }
            return success;
        }

        /// <summary>
        /// <see cref="IMediaValidator.ValidateMediaFiles(System.String[] , System.String , int )"/>
        /// </summary>
        public System.Boolean ValidateMediaFiles(System.String[] mediaFileNames, System.String directoryRecordsToFilter, int numberDirectoryRecordsToFilter)
        {
            bool success = false;
            try
            {
                // Wait until it is safe to enter.
                validateMediaFilesMutex.WaitOne();
                //
                // Check argument mediaFileNames
                //
                if (mediaFileNames == null) throw new System.ArgumentNullException("mediaFileNames");
                if (mediaFileNames.Length == 0) throw new System.ArgumentException("No media files specified", "mediaFileNames");
                foreach (System.String mediaFileName in mediaFileNames)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(mediaFileName);
                    if (!fileInfo.Exists) throw new System.ArgumentException();
                }
                //
                // Begin validation
                //
                success = this.m_adaptee.BeginMediaValidation();

                try
                {
                    //
                    // Validate each media file in turn.
                    //
                    foreach (System.String mediaFileName in mediaFileNames)
                    {
                        success &= this.m_adaptee.ValidateMediaFile(mediaFileName, directoryRecordsToFilter, numberDirectoryRecordsToFilter);
                        if (!success) break;
                    }
                }
                catch (System.Exception e)
                {
                    String msg = String.Format("System.Exception in MediaSession.cs: {0}", e.Message);
                    this.WriteError(msg);
                }
                success &= this.m_adaptee.EndMediaValidation();
            }
            catch (System.Exception e)
            {
                String msg = String.Format("System.Exception in MediaSession.cs: {0}", e.Message);
                this.WriteError(msg);
            }
            finally
            {
                // Release the Mutex.
                validateMediaFilesMutex.ReleaseMutex();
            }
            return success;
        }

        /// <summary>
        /// Asynchronous ValidateMediaFiles delegate.
        /// </summary>
        /// <remarks>
        /// The delegate must have the same signature as the method you want to call asynchronously.
        /// </remarks>
        private delegate bool AsyncValidateMediaFilesDelegate(System.String[] mediaFileNames);

        /// <summary>
        /// <see cref="IMediaValidator.BeginValidateMediaFiles"/>
        /// </summary>
        public System.IAsyncResult BeginValidateMediaFiles(
            System.String[] mediaFileNames,
            AsyncCallback cb)
        {
            // Create the delegate.
            AsyncValidateMediaFilesDelegate dlgt = new AsyncValidateMediaFilesDelegate(this.ValidateMediaFiles);
            // Initiate the asychronous call.
            object asyncState = dlgt;
            IAsyncResult ar = dlgt.BeginInvoke(
                mediaFileNames,
                cb,
                asyncState);
            return ar;
        }

        /// <summary>
        /// <see cref="IMediaValidator.EndValidateMediaFiles"/>
        /// </summary>
        public bool EndValidateMediaFiles(
            IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncValidateMediaFilesDelegate dlgt = (AsyncValidateMediaFilesDelegate)ar.AsyncState;
            // Call EndInvoke to retrieve the results.
            bool retValue = dlgt.EndInvoke(ar);
            return retValue;
        }
        #endregion
    }
}