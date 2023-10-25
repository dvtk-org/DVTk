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
    using DvtkData.Dimse;
    using DvtkData.Dul;

    // Aliases for types
    using SessionFileName = System.String;
    using System;

    /// <summary>
    /// Return codes for a DIMSE message receive action in sniffer.
    /// </summary>
    public enum ReceivedMsgReturnCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 0,
        /// <summary>
        /// Failure
        /// </summary>
        Failure,
        /// <summary>
        /// Incomplete byte stream
        /// </summary>
        IncompleteByteStream,
        /// <summary>
        /// Decoding error
        /// </summary>
        DecodingError,
    };

    /// <summary>
    /// Sniffer interface
    /// </summary>
    public interface ISniffer
    {
        /// <summary>
        /// Read a the PDU files sniffed from the network into file stream.
        /// </summary>
        /// <remarks>
        /// The file typically has the extension <c>.pdu</c>.
        /// </remarks>
        /// <param name="PDUFileNames">file names to read from</param>
        /// <returns>pdu file object read</returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>PDUFileNames</c> is a <see langword="null"/> reference.</exception>
        void ReadPDUsInFileStream(System.String[] PDUFileNames);

        /// <summary>
        /// Receive a DIMSE or ACSE message from the PDU files stream sniffed from network.
        /// </summary>
        /// <p>
        /// This corresponds with a DIMSE Confirmation Primitive or ACSE response.
        /// </p>
        /// <p>
        /// This call allows handling of A_ABORT and A_RELEASE_RQ messages without 
        /// causing an application exception.
        /// </p>
        /// <param name="message">
        /// Abstract message of type <see cref="DvtkData.Message"/> the user can 
        /// use reflection <see cref="System.Object.GetType()"/> to determine whether the message
        /// is of type <see cref="DvtkData.Dimse.DicomMessage"/> or <see cref="DvtkData.Dul.DulMessage"/>.
        /// </param>
        /// <returns>
        /// The return code
        /// </returns>
        ReceivedMsgReturnCode ReceiveMessage(out DvtkData.Message message);

        /// <summary>
        /// Validate a Dul Message
        /// </summary>
        /// <param name="message">Dul Message to be validated</param>
        /// <param name="validationControlFlags">Control flags to steer the validation process</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>file</c> is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">
        /// Argument <c>validationControlFlags</c> is not set to 
        /// <see cref="ValidationControlFlags.UseReferences"/>.
        /// </exception>
        bool Validate(DulMessage message, ValidationControlFlags validationControlFlags);

        /// <summary>
        /// Validate a Dicom Message
        /// </summary>
        /// <param name="message">Dicom Message to be validated</param>
        /// <param name="validationControlFlags">Control flags to steer the validation process</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>file</c> is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">
        /// Argument <c>validationControlFlags</c> is not set to 
        /// <see cref="ValidationControlFlags.UseReferences"/>.
        /// </exception>
        bool Validate(DicomMessage message, ValidationControlFlags validationControlFlags);
    }

    /// <summary>
    /// Summary description for Session.
    /// </summary>
    public class SnifferSession
        : Session
        , ISniffer
    {

        // Validation services
        private Wrappers.MSnifferSession m_adaptee = null;
        override internal Wrappers.MBaseSession m_MBaseSession
        {
            get { return m_adaptee; }
        }
        internal Wrappers.MSnifferSession m_MSnifferSession
        {
            get { return m_adaptee; }
        }

        //
        // Touch the AppUnloadListener abstract class to trigger its static-constructor.
        //
        static SnifferSession()
        {
            AppUnloadListener.Touch();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SnifferSession()
        {
            m_adaptee = new Wrappers.MSnifferSession();
            Wrappers.MDisposableResources.AddDisposable(m_adaptee);
            _Initialize();
        }
        /// <summary>
        /// Finalizer
        /// </summary>
        ~SnifferSession()
        {
            Wrappers.MDisposableResources.RemoveDisposable(m_adaptee);
            m_adaptee = null;
        }
        internal SnifferSession(Wrappers.MBaseSession adaptee)
        {
            if (adaptee == null) throw new System.ArgumentNullException("adaptee");
            // Check type
            m_adaptee = (Wrappers.MSnifferSession)adaptee;
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
        static public SnifferSession LoadFromFile(SessionFileName sessionFileName)
        {
            if (sessionFileName == null) throw new System.ArgumentNullException("sessionFileName");
            Session session = SessionFactory.TheInstance.Load(sessionFileName);
            System.Diagnostics.Trace.Assert(session is SnifferSession);
            return (SnifferSession)session;
        }

        #region ISniffer
        /// <summary>
        /// <see cref="ISniffer.ReadPDUsInFileStream(System.String[])"/>
        /// </summary>
        public void ReadPDUsInFileStream(
            System.String[] pduFileNames)
        {
            //
            // Check argument pduFileNames
            //
            if (pduFileNames == null) throw new System.ArgumentNullException("pduFileNames");
            if (pduFileNames.Length == 0) throw new System.ArgumentException("No PDU files specified", "pduFileNames");
            foreach (System.String pduFileName in pduFileNames)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(pduFileName);
                if (!fileInfo.Exists) throw new System.ArgumentException();
            }
            (this.m_adaptee as Wrappers.MSnifferSession).ReadPDUs(pduFileNames);
        }

        /// <summary>
        /// <see cref="ISniffer.ReceiveMessage"/>
        /// </summary>
        public ReceivedMsgReturnCode ReceiveMessage(out DvtkData.Message message)
        {
            message = null;
            Wrappers.ReceivedMsgReturnCode wrappersReceiveReturnCode =
                (this.m_adaptee as Wrappers.MSnifferSession).ReceiveMessage(out message);
            return _Convert(wrappersReceiveReturnCode);
        }

        /// <summary>
        /// <see cref="ISniffer.Validate(DicomMessage, ValidationControlFlags)"/>
        /// </summary>
        public bool Validate(
            DicomMessage message,
            ValidationControlFlags validationControlFlags)
        {
            if (message == null) throw new System.ArgumentNullException();

            //Set the validation options
            Wrappers.ValidationControlFlags
                wrappersValidationControlFlags = Wrappers.ValidationControlFlags.None;
            if ((validationControlFlags & ValidationControlFlags.UseDefinitions) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseDefinitions;
            if ((validationControlFlags & ValidationControlFlags.UseValueRepresentations) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseValueRepresentations;

            return (this.m_adaptee as Wrappers.MSnifferSession).Validate(
                message,
                wrappersValidationControlFlags);
        }

        /// <summary>
        /// <see cref="ISniffer.Validate(DulMessage , ValidationControlFlags )"/>
        /// </summary>
        public bool Validate(
            DulMessage message,
            ValidationControlFlags validationControlFlags)
        {
            if (message == null) throw new System.ArgumentNullException();

            //Set the validation options
            Wrappers.ValidationControlFlags
                wrappersValidationControlFlags = Wrappers.ValidationControlFlags.None;
            if ((validationControlFlags & ValidationControlFlags.UseDefinitions) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseDefinitions;
            if ((validationControlFlags & ValidationControlFlags.UseValueRepresentations) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseValueRepresentations;

            return (this.m_adaptee as Wrappers.MSnifferSession).Validate(
                message,
                wrappersValidationControlFlags);
        }
        #endregion

        private ReceivedMsgReturnCode _Convert(Wrappers.ReceivedMsgReturnCode receiveReturnCode)
        {
            switch (receiveReturnCode)
            {
                case Wrappers.ReceivedMsgReturnCode.Success: return ReceivedMsgReturnCode.Success;
                case Wrappers.ReceivedMsgReturnCode.Failure: return ReceivedMsgReturnCode.Failure;
                case Wrappers.ReceivedMsgReturnCode.IncompleteByteStream: return ReceivedMsgReturnCode.IncompleteByteStream;
                case Wrappers.ReceivedMsgReturnCode.DecodingError: return ReceivedMsgReturnCode.DecodingError;
                default:
                    // Unknown Wrappers.ReceiveReturnCode
                    throw new System.NotImplementedException();
            }
        }
    }
}