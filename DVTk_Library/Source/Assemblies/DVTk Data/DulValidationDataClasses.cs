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
using System.IO;
using DvtkData.Validation;
using DvtkData.DvtDetailToXml;

namespace DvtkData.Validation
{
    using TypeSafeCollections;
    using SubItems;

    /// <summary>
    /// Validation information about the A_ABORT_RQ.
    /// </summary>
    public class ValidationAbortRq : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
    {
		/// <summary>
		/// Message Unique Identifier
		/// </summary>
		public UInt32 MessageUID 
		{
			get 
			{ 
				return _MsgId; 
			}
			set 
			{ 
				_MsgId = value; 
			}
		} 
		private UInt32 _MsgId = 0;

        /// <summary>
        /// Validation information about the source parameter.
        /// </summary>
        public ValidationAcseParameter Source 
		{
			get 
            { 
                return _Source; 
            }
			set 
            { 
                _Source = value; 
            }
		} private ValidationAcseParameter _Source;
    
        /// <summary>
        /// Validation information about the reason parameter.
        /// </summary>
        public ValidationAcseParameter Reason 
		{
			get 
            { 
                return _Reason; 
            }
			set 
            { 
                _Reason = value; 
            }
		} private ValidationAcseParameter _Reason;

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
                streamWriter.WriteLine("<ValidationResults>");
				streamWriter.WriteLine("<ValidationAbortRq>");
				streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<Source Value=\"{0}\" Meaning=\"{1}\">", Source.Value, Source.Meaning);
                Source.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</Source>");
                streamWriter.WriteLine("<Reason Value=\"{0}\" Meaning=\"{1}\">", Reason.Value, Reason.Meaning);
                Reason.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</Reason>");
                streamWriter.WriteLine("</ValidationAbortRq>");
                streamWriter.WriteLine("</ValidationResults>");
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
                (this.ContainsMessages() == true))
			{
				streamWriter.WriteLine("<ValidationResults>");
				streamWriter.WriteLine("<ValidationAbortRq>");
				if (Source.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<Source Value=\"{0}\" Meaning=\"{1}\">", Source.Value, Source.Meaning);
					Source.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</Source>");
				}
				if (Reason.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<Reason Value=\"{0}\" Meaning=\"{1}\">", Reason.Value, Reason.Meaning);
					Reason.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</Reason>");
				}
				streamWriter.WriteLine("</ValidationAbortRq>");
				streamWriter.WriteLine("</ValidationResults>");
			}
			return true;
		}

		/// <summary>
		/// Check if this contains any validation messages
		/// </summary>
		/// <returns>bool - contains validation messages true/false</returns>
		private bool ContainsMessages()
		{
			bool containsMessages = false;
			if ((Source.Messages.ErrorWarningCount() != 0) ||
				(Reason.Messages.ErrorWarningCount() != 0))
			{
				containsMessages = true;
			}
			return containsMessages;
		}

        public void DvtTestLogToXml(StreamWriter streamWriter, string name)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<TestLog Type=\"DicomNetworkMessageValidation\">");
                streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<Name>{0}</Name>", name);
                // serialize parameter validations
                Reason.DvtTestLogToXml(streamWriter, "Reason");
                Source.DvtTestLogToXml(streamWriter, "Source");

                streamWriter.WriteLine("</TestLog>");
            }
        }
    }

    namespace SubItems
    {

        /// <summary>
        /// Validation information about the ACSE related parameter.
        /// </summary>
		public class ValidationAcseParameter : IDvtDetailToXml, IDvtTestLogToXml
		{
			/// <summary>
			/// List of validation messages about the parameter.
			/// </summary>
			public ValidationMessageCollection Messages 
			{
				get 
                { 
                    return _Messages; 
                }
				set 
                { 
                    _Messages = value; 
                }
			} private ValidationMessageCollection _Messages = new ValidationMessageCollection();
    
			/// <summary>
			/// Relevant value information for the parameter.
			/// </summary>
			public string Value 
			{
				get 
                { 
                    return _Value; 
                }
				set 
                { 
                    _Value = value; 
                }
			} private string _Value;

            /// <summary>
            /// Relevant meaning information for the parameter.
            /// </summary>
            public string Meaning 
            {
                get 
                { 
                    return _Meaning; 
                }
                set 
                { 
                    _Meaning = value; 
                }
            } private string _Meaning;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				Messages.DvtDetailToXml(streamWriter, level);
				return true;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                streamWriter.WriteLine("<Test Name=\"NetworkMessageParameterValidation\">");
                streamWriter.WriteLine("<TestItem ItemType=\"DicomMessageParameter\" Name=\"{0}\" Value=\"{1}\" />", name, Value);
                //streamWriter.WriteLine("<Value>{0}</Value>", Value);
                SerializeHelper.serilalizeValidationMessages(Messages, streamWriter);
                streamWriter.WriteLine("</Test>");
            }
        }
	}

    /// <summary>
    /// Validation information about the A_ASSOCIATE_RQ.
    /// </summary>
    public class ValidationAssociateRq : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
    {
		/// <summary>
		/// Message Unique Identifier
		/// </summary>
		public UInt32 MessageUID 
		{
			get 
			{ 
				return _MsgId; 
			}
			set 
			{ 
				_MsgId = value; 
			}
		} 
		private UInt32 _MsgId = 0;

        /// <summary>
        /// Validation information about the protocol version parameter.
        /// </summary>
        public ValidationAcseParameter ProtocolVersion 
		{
			get 
            { 
                return _ProtocolVersion; 
            }
			set 
            { 
                _ProtocolVersion = value; 
            }
		} 
        private ValidationAcseParameter _ProtocolVersion;
    
        /// <summary>
        /// Validation information about the called application entity title parameter.
        /// </summary>
        public ValidationAcseParameter CalledAETitle 
		{
			get 
            { 
                return _CalledAeTitle; 
            }
			set 
            { 
                _CalledAeTitle = value; 
            }
		} 
        private ValidationAcseParameter _CalledAeTitle;
    
        /// <summary>
        /// Validation information about the calling application entity title parameter.
        /// </summary>
        public ValidationAcseParameter CallingAETitle 
		{
			get 
            { 
                return _CallingAeTitle; 
            }
			set 
            { 
                _CallingAeTitle = value; 
            }
		} 
        private ValidationAcseParameter _CallingAeTitle;
    
        /// <summary>
        /// Validation information about the application context name parameter.
        /// </summary>
        public ValidationAcseParameter ApplicationContextName 
		{
			get 
            { 
                return _ApplicationContextName; 
            }
			set 
            { 
                _ApplicationContextName = value; 
            }
		} 
        private ValidationAcseParameter _ApplicationContextName;
    
        /// <summary>
        /// Validation information about the requested presentation contexts.
        /// </summary>
        public ValidationAcsePresentationContextRequestCollection PresentationContexts 
		{
			get 
            { 
                return _PresentationContexts; 
            }
			set 
            { 
                _PresentationContexts = value; 
            }
		} 
        private ValidationAcsePresentationContextRequestCollection _PresentationContexts = new ValidationAcsePresentationContextRequestCollection();
    
        /// <summary>
        /// Validation information about the user information.
        /// </summary>
        public ValidationAcseUserInformation UserInformation 
		{
			get 
            { 
                return _UserInformation; 
            }
			set 
            { 
                _UserInformation = value; 
            }
		} 
        private ValidationAcseUserInformation _UserInformation;

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
                streamWriter.WriteLine("<ValidationResults>");
                streamWriter.WriteLine("<ValidationAssociateRq>");
				streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<ProtocolVersion Value=\"{0}\">", ProtocolVersion.Value);
                ProtocolVersion.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</ProtocolVersion>");
                streamWriter.WriteLine("<CalledAETitle Value=\"{0}\">", CalledAETitle.Value);
                CalledAETitle.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</CalledAETitle>");
                streamWriter.WriteLine("<CallingAETitle Value=\"{0}\">", CallingAETitle.Value);
                CallingAETitle.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</CallingAETitle>");
                streamWriter.WriteLine("<ApplicationContextName Value=\"{0}\">", ApplicationContextName.Value);
                ApplicationContextName.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</ApplicationContextName>");
                PresentationContexts.DvtDetailToXml(streamWriter, level);
                UserInformation.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</ValidationAssociateRq>");
                streamWriter.WriteLine("</ValidationResults>");
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
                (this.ContainsMessages() == true))
			{
				streamWriter.WriteLine("<ValidationResults>");
				streamWriter.WriteLine("<ValidationAssociateRq>");
				if (ProtocolVersion.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<ProtocolVersion Value=\"{0}\">", ProtocolVersion.Value);
					ProtocolVersion.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</ProtocolVersion>");
				}
				if (CalledAETitle.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<CalledAETitle Value=\"{0}\">", CalledAETitle.Value);
					CalledAETitle.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</CalledAETitle>");
				}
				if (CallingAETitle.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<CallingAETitle Value=\"{0}\">", CallingAETitle.Value);
					CallingAETitle.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</CallingAETitle>");
				}
				if (ApplicationContextName.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<ApplicationContextName Value=\"{0}\">", ApplicationContextName.Value);
					ApplicationContextName.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</ApplicationContextName>");
				}
				if (PresentationContexts.ContainsMessages() == true)
				{
					PresentationContexts.DvtSummaryToXml(streamWriter, level);
				}
				if (UserInformation.ContainsMessages() == true)
				{
					UserInformation.DvtSummaryToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</ValidationAssociateRq>");
				streamWriter.WriteLine("</ValidationResults>");
			}
			return true;
		}    

		/// <summary>
		/// Check if this contains any validation messages
		/// </summary>
		/// <returns>bool - contains validation messages true/false</returns>
		private bool ContainsMessages()
		{
			bool containsMessages = false;
			if ((ProtocolVersion.Messages.ErrorWarningCount() != 0) ||
				(CalledAETitle.Messages.ErrorWarningCount() != 0) ||
				(CallingAETitle.Messages.ErrorWarningCount() != 0) ||
				(ApplicationContextName.Messages.ErrorWarningCount() != 0) ||
				(PresentationContexts.ContainsMessages() == true) ||
				(UserInformation.ContainsMessages() == true))
			{
				containsMessages = true;
			}
			return containsMessages;
		}

        public void DvtTestLogToXml(StreamWriter streamWriter, string name)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<TestLog Type=\"DicomNetworkMessageValidation\">");
                streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<Name>{0}</Name>", name);
                // serialize parameter validations
                ProtocolVersion.DvtTestLogToXml(streamWriter, "Protocol Version");
                CalledAETitle.DvtTestLogToXml(streamWriter, "Called AE Title");
                CallingAETitle.DvtTestLogToXml(streamWriter, "Calling AE Title");
                ApplicationContextName.DvtTestLogToXml(streamWriter, "Application Context Name");
                // serialize requested transfer syntax validations
                foreach (ValidationAcsePresentationContextRequest presentationContextRq in PresentationContexts)
                {
                    presentationContextRq.DvtTestLogToXml(streamWriter, "");
                }

                // serialize user information validations
                UserInformation.DvtTestLogToXml(streamWriter, "");

                streamWriter.WriteLine("</TestLog>");
            }
        }
    }

    namespace SubItems
    {

        /// <summary>
        /// Validation information about the requested presentation context.
        /// </summary>
        public class ValidationAcsePresentationContextRequest : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
        {
    
            /// <summary>
            /// Validation information about the presentation context identifier parameter.
            /// </summary>
            public ValidationAcseParameter PresentationContextId 
            {
                get 
                { 
                    return _PresentationContextId; 
                }
                set 
                { 
                    _PresentationContextId = value; 
                }
            } 
            private ValidationAcseParameter _PresentationContextId;
    
            /// <summary>
            /// Validation information about the abstract syntax name parameter.
            /// </summary>
            public ValidationAcseParameter AbstractSyntaxName 
            {
                get 
                { 
                    return _AbstractSyntaxName; 
                }
                set 
                { 
                    _AbstractSyntaxName = value; 
                }
            } 
            private ValidationAcseParameter _AbstractSyntaxName;
    
            /// <summary>
            /// Validation information about the requested transfer syntax names.
            /// </summary>
            public ValidationAcseParameterCollection RequestedTransferSyntaxNames 
            {
                get 
                { 
                    return _RequestedTransferSyntaxNames; 
                }
                set 
                { 
                    _RequestedTransferSyntaxNames = value; 
                }
            } 
            private ValidationAcseParameterCollection _RequestedTransferSyntaxNames = new ValidationAcseParameterCollection();

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<PresentationContext>");
				streamWriter.WriteLine("<Id  Value=\"{0}\">", PresentationContextId.Value);
				PresentationContextId.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</Id>");
				streamWriter.WriteLine("<AbstractSyntaxName Value=\"{0}\" Meaning=\"{1}\">", AbstractSyntaxName.Value, AbstractSyntaxName.Meaning);
				AbstractSyntaxName.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</AbstractSyntaxName>");
				streamWriter.WriteLine("<TransferSyntaxes>");
				foreach (ValidationAcseParameter transferSyntax in RequestedTransferSyntaxNames)
				{
					streamWriter.WriteLine("<TransferSyntax Value=\"{0}\" Meaning=\"{1}\">", transferSyntax.Value, transferSyntax.Meaning);
					transferSyntax.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</TransferSyntax>");
				}
				streamWriter.WriteLine("</TransferSyntaxes>");
				streamWriter.WriteLine("</PresentationContext>");
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
				if (this.ContainsMessages())
				{
					streamWriter.WriteLine("<PresentationContext>");
					// always serialize the Id - used to uniquely identify a presentation context
					streamWriter.WriteLine("<Id  Value=\"{0}\">", PresentationContextId.Value);
					PresentationContextId.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</Id>");
					if (AbstractSyntaxName.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<AbstractSyntaxName Value=\"{0}\" Meaning=\"{1}\">", AbstractSyntaxName.Value, AbstractSyntaxName.Meaning);
						AbstractSyntaxName.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</AbstractSyntaxName>");
					}
					if (RequestedTransferSyntaxNames.ContainsMessages() == true)
					{
						streamWriter.WriteLine("<TransferSyntaxes>");
						foreach (ValidationAcseParameter transferSyntax in RequestedTransferSyntaxNames)
						{
							streamWriter.WriteLine("<TransferSyntax Value=\"{0}\" Meaning=\"{1}\">", transferSyntax.Value, transferSyntax.Meaning);
							transferSyntax.DvtDetailToXml(streamWriter, level);
							streamWriter.WriteLine("</TransferSyntax>");
						}
						streamWriter.WriteLine("</TransferSyntaxes>");
					}
					streamWriter.WriteLine("</PresentationContext>");
				}
				return true;
			}   

     		/// <summary>
     		/// Check if this contains any validation messages
     		/// </summary>
     		/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				if ((PresentationContextId.Messages.ErrorWarningCount() != 0) ||
					(AbstractSyntaxName.Messages.ErrorWarningCount() != 0) ||
					(RequestedTransferSyntaxNames.ContainsMessages() == true))
				{
					containsMessages = true;
				}
				return containsMessages;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                AbstractSyntaxName.DvtTestLogToXml(streamWriter, "Abstract Syntax Name");
                PresentationContextId.DvtTestLogToXml(streamWriter, "Presentation Context Id");
                foreach (ValidationAcseParameter parameter in RequestedTransferSyntaxNames)
                {
                    parameter.DvtTestLogToXml(streamWriter, "Requested Transfer Syntax");
                }
            }
        }
    }

    namespace SubItems
    {

        /// <summary>
        /// Validation information about the user information.
        /// </summary>
        public class ValidationAcseUserInformation : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
        {
    
            /// <summary>
            /// Validation information about the MaximumLengthReceived parameter.
            /// </summary>
            public ValidationAcseParameter MaximumLengthReceived 
            {
                get 
                { 
                    return _MaximumLengthReceived; 
                }
                set 
                { 
                    _MaximumLengthReceived = value; 
                }
            } 
            private ValidationAcseParameter _MaximumLengthReceived;
    
            /// <summary>
            /// Validation information about the ImplementationClassUid parameter.
            /// </summary>
            public ValidationAcseParameter ImplementationClassUid 
            {
                get 
                { 
                    return _ImplementationClassUid; 
                }
                set 
                { 
                    _ImplementationClassUid = value; 
                }
            } 
            private ValidationAcseParameter _ImplementationClassUid;
    
            /// <summary>
            /// Validation information about the ImplementationVersionName parameter.
            /// </summary>
            public ValidationAcseParameter ImplementationVersionName 
            {
                get 
                { 
                    return _ImplementationVersionName; 
                }
                set 
                { 
                    _ImplementationVersionName = value; 
                }
            } 
            private ValidationAcseParameter _ImplementationVersionName;
    
            /// <summary>
            /// Validation information about the scp scu role selections.
            /// </summary>
            public ValidationAcseScpScuRoleSelectCollection ScpScuRoleSelection 
            {
                get 
                { 
                    return _ScpScuRoleSelection; 
                }
                set 
                { 
                    _ScpScuRoleSelection = value; 
                }
            } 
            private ValidationAcseScpScuRoleSelectCollection _ScpScuRoleSelection;
    
            /// <summary>
            /// Validation information about the AsynchronousOperationWindow.
            /// </summary>
            public ValidationAcseAsynchronousOperationWindow AsynchronousOperationWindow 
            {
                get 
                { 
                    return _AsynchronousOperationWindow; 
                }
                set 
                { 
                    _AsynchronousOperationWindow = value; 
                }
            } 
            private ValidationAcseAsynchronousOperationWindow _AsynchronousOperationWindow;
    
            /// <summary>
            /// Validation information about the ExtendedSopClasses.
            /// </summary>
            public ValidationAcseSopClassExtendedCollection ExtendedSopClasses 
            {
                get 
                { 
                    return _ExtendedSopClasses; 
                }
                set 
                { 
                    _ExtendedSopClasses = value; 
                }
            } 
            private ValidationAcseSopClassExtendedCollection _ExtendedSopClasses;

			/// <summary>
			/// Validation information about the UserIdentityNegotiation.
			/// </summary>
			public ValidationAcseUserIdentityNegotiation UserIdentityNegotiation 
			{
				get 
				{ 
					return _UserIdentityNegotiation; 
				}
				set 
				{ 
					_UserIdentityNegotiation = value; 
				}
			} 
			private ValidationAcseUserIdentityNegotiation _UserIdentityNegotiation;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<UserInformation>");
				streamWriter.WriteLine("<MaximumLengthReceived Value=\"{0}\">", MaximumLengthReceived.Value);
				MaximumLengthReceived.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</MaximumLengthReceived>");
				streamWriter.WriteLine("<ImplementationClassUid Value=\"{0}\">", ImplementationClassUid.Value);
				ImplementationClassUid.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</ImplementationClassUid>");
				if (ImplementationVersionName != null)
				{
					streamWriter.WriteLine("<ImplementationVersionName Value=\"{0}\">", ImplementationVersionName.Value);
					ImplementationVersionName.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</ImplementationVersionName>");
				}
				if (ScpScuRoleSelection != null)
				{
					ScpScuRoleSelection.DvtDetailToXml(streamWriter, level);
				}
				if (AsynchronousOperationWindow != null)
				{
					AsynchronousOperationWindow.DvtDetailToXml(streamWriter, level);
				}
				if (ExtendedSopClasses != null)
				{
					ExtendedSopClasses.DvtDetailToXml(streamWriter, level);
				}
				if (UserIdentityNegotiation != null)
				{
					UserIdentityNegotiation.DvtDetailToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</UserInformation>");
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
				if (this.ContainsMessages() == true)
				{
					streamWriter.WriteLine("<UserInformation>");
					if (MaximumLengthReceived.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<MaximumLengthReceived Value=\"{0}\">", MaximumLengthReceived.Value);
						MaximumLengthReceived.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</MaximumLengthReceived>");
					}
					if (ImplementationClassUid.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<ImplementationClassUid Value=\"{0}\">", ImplementationClassUid.Value);
						ImplementationClassUid.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</ImplementationClassUid>");
					}
					if ((ImplementationVersionName != null) &&
						(ImplementationVersionName.Messages.ErrorWarningCount() != 0))
					{
						streamWriter.WriteLine("<ImplementationVersionName Value=\"{0}\">", ImplementationVersionName.Value);
						ImplementationVersionName.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</ImplementationVersionName>");
					}
					if ((ScpScuRoleSelection != null) &&
						(ScpScuRoleSelection.ContainsMessages() == true))
					{
						ScpScuRoleSelection.DvtSummaryToXml(streamWriter, level);
					}
					if ((AsynchronousOperationWindow != null) &&
						(AsynchronousOperationWindow.ContainsMessages() == true))
					{
						AsynchronousOperationWindow.DvtSummaryToXml(streamWriter, level);
					}
					if ((ExtendedSopClasses != null) &&
						(ExtendedSopClasses.ContainsMessages() == true))
					{
						ExtendedSopClasses.DvtSummaryToXml(streamWriter, level);
					}
					if ((UserIdentityNegotiation != null) &&
						(UserIdentityNegotiation.ContainsMessages() == true))
					{
						UserIdentityNegotiation.DvtSummaryToXml(streamWriter, level);
					}
					streamWriter.WriteLine("</UserInformation>");
				}
				return true;
			}        		

			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				bool implementationVersionNameMessages = false;
				if ((ImplementationVersionName != null) &&
					(ImplementationVersionName.Messages.ErrorWarningCount() != 0))
				{
					implementationVersionNameMessages = true;
				}
				bool scpScuRoleSelectionMessages = false;
				if ((ScpScuRoleSelection != null) &&
					(ScpScuRoleSelection.ContainsMessages() == true))
				{
					scpScuRoleSelectionMessages = true;
				}
				bool asynchronousOperationWindowMessages = false;
				if ((AsynchronousOperationWindow != null) &&
					(AsynchronousOperationWindow.ContainsMessages() == true))
				{
					asynchronousOperationWindowMessages = true;
				}
				bool extendedSopClassesMessages = false;
				if ((ExtendedSopClasses != null) &&
					(ExtendedSopClasses.ContainsMessages() == true))
				{
					extendedSopClassesMessages = true;
				}
				bool userIdentityNegotiationMessages = false;
				if ((UserIdentityNegotiation != null) &&
					(UserIdentityNegotiation.ContainsMessages() == true))
				{
					userIdentityNegotiationMessages = true;
				}

				if ((MaximumLengthReceived.Messages.ErrorWarningCount() != 0) ||
					(ImplementationClassUid.Messages.ErrorWarningCount() != 0) ||
					(implementationVersionNameMessages == true) ||
					(scpScuRoleSelectionMessages == true) ||
					(asynchronousOperationWindowMessages == true) ||
					(extendedSopClassesMessages == true) ||
					(userIdentityNegotiationMessages == true))
				{
					containsMessages = true;
				}
				return containsMessages;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                if (AsynchronousOperationWindow != null)
                {
                    AsynchronousOperationWindow.DvtTestLogToXml(streamWriter, "");
                }
                if (ExtendedSopClasses != null)
                {
                    foreach (ValidationAcseSopClassExtended sopClassExtended in ExtendedSopClasses)
                    {
                        sopClassExtended.DvtTestLogToXml(streamWriter, "");
                    }
                }
                ImplementationClassUid.DvtTestLogToXml(streamWriter, "Implementation Class UID");
                ImplementationVersionName.DvtTestLogToXml(streamWriter, "Implementation Version Name");
                MaximumLengthReceived.DvtTestLogToXml(streamWriter, "Maximum Length Received");
                if (ScpScuRoleSelection != null)
                {
                    foreach (ValidationAcseScpScuRoleSelect scpScuRoleSelect in ScpScuRoleSelection)
                    {
                        scpScuRoleSelect.DvtTestLogToXml(streamWriter, "");
                    }
                }
                if (UserIdentityNegotiation != null)
                {
                    UserIdentityNegotiation.DvtTestLogToXml(streamWriter, "");
                }
            }
        }
    }

    namespace SubItems
    {

        /// <summary>
        /// Validation information about the ValidationAcseScpScuRoleSelect.
        /// </summary>
        public class ValidationAcseScpScuRoleSelect : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
        {
    
            /// <summary>
            /// Validation information about the Unique Indentifier parameter.
            /// </summary>
            public ValidationAcseParameter Uid 
            {
                get 
                { 
                    return _Uid; 
                }
                set 
                { 
                    _Uid = value; 
                }
            }
            private ValidationAcseParameter _Uid;
    
            /// <summary>
            /// Validation information about the scp role parameter.
            /// </summary>
            public ValidationAcseParameter ScpRole 
            {
                get 
                { 
                    return _ScpRole; 
                }
                set 
                { 
                    _ScpRole = value; 
                }
            } 
            private ValidationAcseParameter _ScpRole;
    
            /// <summary>
            /// Validation information about the scu role parameter.
            /// </summary>
            public ValidationAcseParameter ScuRole 
            {
                get 
                { 
                    return _ScuRole; 
                }
                set 
                { 
                    _ScuRole = value; 
                }
            } 
            private ValidationAcseParameter _ScuRole;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<ScpScuRoleSelection>");
				streamWriter.WriteLine("<Uid Value=\"{0}\">", Uid.Value);
				Uid.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</Uid>");
				streamWriter.WriteLine("<ScpRole Value=\"{0}\" Meaning=\"{1}\">", ScpRole.Value, ScpRole.Meaning);
				ScpRole.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</ScpRole>");
				streamWriter.WriteLine("<ScuRole Value=\"{0}\" Meaning=\"{1}\">", ScuRole.Value, ScuRole.Meaning);
				ScuRole.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</ScuRole>");
				streamWriter.WriteLine("</ScpScuRoleSelection>");
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
				if (this.ContainsMessages())
				{
					streamWriter.WriteLine("<ScpScuRoleSelection>");
					// always serialize the Uid - used to uniquely identify a ScpScuRoleSelection
					streamWriter.WriteLine("<Uid Value=\"{0}\">", Uid.Value);
					Uid.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</Uid>");
					if (ScpRole.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<ScpRole Value=\"{0}\" Meaning=\"{1}\">", ScpRole.Value, ScpRole.Meaning);
						ScpRole.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</ScpRole>");
					}
					if (ScuRole.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<ScuRole Value=\"{0}\" Meaning=\"{1}\">", ScuRole.Value, ScuRole.Meaning);
						ScuRole.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</ScuRole>");
					}
					streamWriter.WriteLine("</ScpScuRoleSelection>");
				}
				return true;
			}

			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				if ((Uid.Messages.ErrorWarningCount() != 0) ||
					(ScpRole.Messages.ErrorWarningCount() != 0) ||
					(ScuRole.Messages.ErrorWarningCount() != 0))
				{
					containsMessages = true;
				}
				return containsMessages;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                this.ScpRole.DvtTestLogToXml(streamWriter, "SCP Role");
                this.ScuRole.DvtTestLogToXml(streamWriter, "SCU Role");
                this.Uid.DvtTestLogToXml(streamWriter, "UID");
            }
        }
    }

    namespace SubItems
    {

        /// <summary>
        /// Validation information about the asynchronous operation window.
        /// </summary>
        public class ValidationAcseAsynchronousOperationWindow : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
        {
    
            /// <summary>
            /// Validation information about the operations invoked parameter.
            /// </summary>
            public ValidationAcseParameter OperationsInvoked 
            {
                get 
                { 
                    return _OperationsInvoked; 
                }
                set 
                { 
                    _OperationsInvoked = value; 
                }
            } 
            private ValidationAcseParameter _OperationsInvoked;
    
            /// <summary>
            /// Validation information about the operations performed parameter.
            /// </summary>
            public ValidationAcseParameter OperationsPerformed 
            {
                get 
                { 
                    return _OperationsPerformed; 
                }
                set 
                { 
                    _OperationsPerformed = value; 
                }
            } 
            private ValidationAcseParameter _OperationsPerformed;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<AsynchronousOperationsWindow>");
				streamWriter.WriteLine("<Invoked Value=\"{0}\">", OperationsInvoked.Value);
				OperationsInvoked.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</Invoked>");
				streamWriter.WriteLine("<Performed Value=\"{0}\">", OperationsPerformed.Value);
				OperationsPerformed.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</Performed>");
				streamWriter.WriteLine("</AsynchronousOperationsWindow>");
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
				if (this.ContainsMessages() == true)
				{
					streamWriter.WriteLine("<AsynchronousOperationsWindow>");
					if (OperationsInvoked.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<Invoked Value=\"{0}\">", OperationsInvoked.Value);
						OperationsInvoked.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</Invoked>");
					}
					if (OperationsPerformed.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<Performed Value=\"{0}\">", OperationsPerformed.Value);
						OperationsPerformed.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</Performed>");
					}
					streamWriter.WriteLine("</AsynchronousOperationsWindow>");
				}
				return true;
			}             
        		
			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				if ((OperationsInvoked.Messages.ErrorWarningCount() != 0) ||
					(OperationsPerformed.Messages.ErrorWarningCount() != 0)) 
				{
					containsMessages = true;
				}
				return containsMessages;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                OperationsInvoked.DvtTestLogToXml(streamWriter, "Operations Invoked");
                OperationsPerformed.DvtTestLogToXml(streamWriter, "Operations Performed");
            }
        }
    }

    namespace SubItems
    {

        /// <summary>
        /// Validation information about the sop class extended.
        /// </summary>
        public class ValidationAcseSopClassExtended : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
        {
    
            /// <summary>
            /// Validation information about the Unique Indentifier parameter.
            /// </summary>
            public ValidationAcseParameter Uid 
            {
                get 
                { 
                    return _Uid; 
                }
                set 
                { 
                    _Uid = value; 
                }
            } 
            private ValidationAcseParameter _Uid;
    
            /// <summary>
            /// Validation information about the application information parameter
            /// </summary>
            public ValidationAcseParameterCollection ApplicationInformation 
            {
                get 
                { 
                    return _ApplicationInformation; 
                }
                set 
                { 
                    _ApplicationInformation = value; 
                }
            } 
            private ValidationAcseParameterCollection _ApplicationInformation = new ValidationAcseParameterCollection();

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<SopClassExtendedNegotiation>");
				streamWriter.WriteLine("<Uid Value=\"{0}\">", Uid.Value);
				Uid.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</Uid>");
				foreach (ValidationAcseParameter appInfo in ApplicationInformation)
				{
					streamWriter.WriteLine("<AppInfo Value=\"{0}\">", appInfo.Value);
					appInfo.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</AppInfo>");
				}
				streamWriter.WriteLine("</SopClassExtendedNegotiation>");
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
				if (this.ContainsMessages() == true)
				{
					streamWriter.WriteLine("<SopClassExtendedNegotiation>");
					// always serialize the Uid - used to uniquely identify a SopClassExtendedNegotiation
					streamWriter.WriteLine("<Uid Value=\"{0}\">", Uid.Value);
					Uid.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</Uid>");
					foreach (ValidationAcseParameter appInfo in ApplicationInformation)
					{
						if (appInfo.Messages.ErrorWarningCount() != 0)
						{
							streamWriter.WriteLine("<AppInfo Value=\"{0}\">", appInfo.Value);
							appInfo.DvtDetailToXml(streamWriter, level);
							streamWriter.WriteLine("</AppInfo>");
						}
					}
					streamWriter.WriteLine("</SopClassExtendedNegotiation>");
				}
				return true;
			}                      		

			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				if ((Uid.Messages.ErrorWarningCount() != 0) ||
					(ApplicationInformation.ContainsMessages() == true))
				{
					containsMessages = true;
				}
				return containsMessages;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                foreach (ValidationAcseParameter parameter in ApplicationInformation)
                {
                    parameter.DvtTestLogToXml(streamWriter, "Application Information");
                }
                Uid.DvtTestLogToXml(streamWriter, "UID");
            }
        }
    }

	namespace SubItems
	{

		/// <summary>
		/// Validation information about the user identity negotiation parameter.
		/// </summary>
		public class ValidationAcseUserIdentityNegotiation : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
		{
    
			/// <summary>
			/// Validation information about the user identity type.
			/// </summary>
			public ValidationAcseParameter UserIdentityType 
			{
				get 
				{ 
					return _UserIdentityType; 
				}
				set 
				{ 
					_UserIdentityType = value; 
				}
			} 
			private ValidationAcseParameter _UserIdentityType;
    
			/// <summary>
			/// Validation information about the positive response requested parameter.
			/// </summary>
			public ValidationAcseParameter PositiveResponseRequested 
			{
				get 
				{ 
					return _PositiveResponseRequested; 
				}
				set 
				{ 
					_PositiveResponseRequested = value; 
				}
			} 
			private ValidationAcseParameter _PositiveResponseRequested;

			/// <summary>
			/// Validation information about the primary field parameter.
			/// </summary>
			public ValidationAcseParameter PrimaryField 
			{
				get 
				{ 
					return _PrimaryField; 
				}
				set 
				{ 
					_PrimaryField = value; 
				}
			} 
			private ValidationAcseParameter _PrimaryField = null;

			/// <summary>
			/// Validation information about the secondary field requested parameter.
			/// </summary>
			public ValidationAcseParameter SecondaryField 
			{
				get 
				{ 
					return _SecondaryField; 
				}
				set 
				{ 
					_SecondaryField = value; 
				}
			} 
			private ValidationAcseParameter _SecondaryField = null;

			/// <summary>
			/// Validation information about the server response parameter.
			/// </summary>
			public ValidationAcseParameter ServerResponse 
			{
				get 
				{ 
					return _ServerResponse; 
				}
				set 
				{ 
					_ServerResponse = value; 
				}
			} 
			private ValidationAcseParameter _ServerResponse = null;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<UserIdentityNegotiation>");
				if ((PrimaryField != null) &&
					(PrimaryField.Value.Length > 0))
				{
					streamWriter.WriteLine("<UserIdentityType Value=\"{0}\" Meaning=\"{1}\">", UserIdentityType.Value, UserIdentityType.Meaning);
					UserIdentityType.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</UserIdentityType>");
					streamWriter.WriteLine("<PositiveResponseRequested Value=\"{0}\" Meaning=\"{1}\">", PositiveResponseRequested.Value, PositiveResponseRequested.Meaning);
					PositiveResponseRequested.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</PositiveResponseRequested>");
					streamWriter.WriteLine("<PrimaryField Value=\"{0}\">", PrimaryField.Value);
					PrimaryField.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</PrimaryField>");
					streamWriter.WriteLine("<SecondaryField Value=\"{0}\">", SecondaryField.Value);
					SecondaryField.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</SecondaryField>");
				}
				else
				{
					streamWriter.WriteLine("<ServerResponse Value=\"{0}\">", ServerResponse.Value);
					ServerResponse.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</ServerResponse>");
				}
				streamWriter.WriteLine("</UserIdentityNegotiation>");
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
				if (this.ContainsMessages() == true)
				{
					streamWriter.WriteLine("<UserIdentityNegotiation>");
					if ((PrimaryField != null) &&
						(PrimaryField.Value.Length > 0))
					{
						if (UserIdentityType.Messages.ErrorWarningCount() != 0)
						{
							streamWriter.WriteLine("<UserIdentityType Value=\"{0}\" Meaning=\"{1}\">", UserIdentityType.Value, UserIdentityType.Meaning);
							UserIdentityType.DvtDetailToXml(streamWriter, level);
							streamWriter.WriteLine("</UserIdentityType>");
						}
						if (PositiveResponseRequested.Messages.ErrorWarningCount() != 0)
						{
							streamWriter.WriteLine("<PositiveResponseRequested Value=\"{0}\" Meaning=\"{1}\">", PositiveResponseRequested.Value, PositiveResponseRequested.Meaning);
							PositiveResponseRequested.DvtDetailToXml(streamWriter, level);
							streamWriter.WriteLine("</PositiveResponseRequested>");
						}
						if (PrimaryField.Messages.ErrorWarningCount() != 0)
						{
							streamWriter.WriteLine("<PrimaryField Value=\"{0}\">", PrimaryField.Value);
							PrimaryField.DvtDetailToXml(streamWriter, level);
							streamWriter.WriteLine("</PrimaryField>");
						}
						if (SecondaryField.Messages.ErrorWarningCount() != 0)
						{
							streamWriter.WriteLine("<SecondaryField Value=\"{0}\">", SecondaryField.Value);
							SecondaryField.DvtDetailToXml(streamWriter, level);
							streamWriter.WriteLine("</SecondaryField>");
						}
					}
					else
					{
						if (ServerResponse.Messages.ErrorWarningCount() != 0)
						{
							streamWriter.WriteLine("<ServerResponse Value=\"{0}\">", ServerResponse.Value);
							ServerResponse.DvtDetailToXml(streamWriter, level);
							streamWriter.WriteLine("</ServerResponse>");
						}
					}
					streamWriter.WriteLine("</UserIdentityNegotiation>");
				}
				return true;
			}             
        		
			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				if ((PrimaryField != null) &&
					(PrimaryField.Value.Length > 0))
				{
					if ((UserIdentityType.Messages.ErrorWarningCount() != 0) ||
						(PositiveResponseRequested.Messages.ErrorWarningCount() != 0) || 
						(PrimaryField.Messages.ErrorWarningCount() != 0) || 
						(SecondaryField.Messages.ErrorWarningCount() != 0)) 
					{
						containsMessages = true;
					}
				}
				else
				{
					if (ServerResponse.Messages.ErrorWarningCount() != 0)
					{
						containsMessages = true;
					}
				}
				return containsMessages;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                PositiveResponseRequested.DvtTestLogToXml(streamWriter, "Positive Response Requested");
                PrimaryField.DvtTestLogToXml(streamWriter, "Primary Field");
                SecondaryField.DvtTestLogToXml(streamWriter, "Secondary Field");
                ServerResponse.DvtTestLogToXml(streamWriter, "Server Response");
                UserIdentityType.DvtTestLogToXml(streamWriter, "User Identity Type");
            }
        }
	}

    /// <summary>
    /// Validation information about the A_ASSOCIATE_AC.
    /// </summary>
    public class ValidationAssociateAc : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
    {
		/// <summary>
		/// Message Unique Identifier
		/// </summary>
		public UInt32 MessageUID 
		{
			get 
			{ 
				return _MsgId; 
			}
			set 
			{ 
				_MsgId = value; 
			}
		} 
		private UInt32 _MsgId = 0;

        /// <summary>
        /// Validation information about the protocol version parameter.
        /// </summary>
        public ValidationAcseParameter ProtocolVersion 
		{
			get 
            { 
                return _ProtocolVersion; 
            }
			set 
            { 
                _ProtocolVersion = value; 
            }
		} 
        private ValidationAcseParameter _ProtocolVersion;
    
        /// <summary>
        /// Validation information about the called application entity title parameter.
        /// </summary>
        public ValidationAcseParameter CalledAETitle 
		{
			get 
            { 
                return _CalledAeTitle; 
            }
			set 
            { 
                _CalledAeTitle = value; 
            }
		} 
        private ValidationAcseParameter _CalledAeTitle;
    
        /// <summary>
        /// Validation information about the calling application entity title parameter.
        /// </summary>
        public ValidationAcseParameter CallingAETitle 
		{
			get 
            { 
                return _CallingAeTitle; 
            }
			set 
            { 
                _CallingAeTitle = value; 
            }
		} 
        private ValidationAcseParameter _CallingAeTitle;
    
        /// <summary>
        /// Validation information about the application context name parameter.
        /// </summary>
        public ValidationAcseParameter ApplicationContextName 
		{
			get 
            { 
                return _ApplicationContextName; 
            }
			set 
            { 
                _ApplicationContextName = value; 
            }
		} 
        private ValidationAcseParameter _ApplicationContextName;
    
        /// <summary>
        /// Validation information about the accepted presentation contexts.
        /// </summary>
        public ValidationAcsePresentationContextAcceptCollection PresentationContexts 
		{
			get 
            { 
                return _PresentationContexts; 
            }
			set 
            { 
                _PresentationContexts = value; 
            }
		} 
        private ValidationAcsePresentationContextAcceptCollection _PresentationContexts = new ValidationAcsePresentationContextAcceptCollection();
    
        /// <summary>
        /// Validation information about the user information.
        /// </summary>
        public ValidationAcseUserInformation UserInformation 
		{
			get 
            { 
                return _UserInformation; 
            }
			set 
            { 
                _UserInformation = value; 
            }
		} 
        private ValidationAcseUserInformation _UserInformation;

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
                streamWriter.WriteLine("<ValidationResults>");
                streamWriter.WriteLine("<ValidationAssociateAc>");
				streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<ProtocolVersion Value=\"{0}\">", ProtocolVersion.Value);
                ProtocolVersion.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</ProtocolVersion>");
                streamWriter.WriteLine("<CalledAETitle Value=\"{0}\">", CalledAETitle.Value);
                CalledAETitle.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</CalledAETitle>");
                streamWriter.WriteLine("<CallingAETitle Value=\"{0}\">", CallingAETitle.Value);
                CallingAETitle.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</CallingAETitle>");
                streamWriter.WriteLine("<ApplicationContextName Value=\"{0}\">", ApplicationContextName.Value);
                ApplicationContextName.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</ApplicationContextName>");
                PresentationContexts.DvtDetailToXml(streamWriter, level);
                UserInformation.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</ValidationAssociateAc>");
                streamWriter.WriteLine("</ValidationResults>");
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
                (this.ContainsMessages() == true))
			{
				streamWriter.WriteLine("<ValidationResults>");
				streamWriter.WriteLine("<ValidationAssociateAc>");
				if (ProtocolVersion.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<ProtocolVersion Value=\"{0}\">", ProtocolVersion.Value);
					ProtocolVersion.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</ProtocolVersion>");
				}
				if (CalledAETitle.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<CalledAETitle Value=\"{0}\">", CalledAETitle.Value);
					CalledAETitle.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</CalledAETitle>");
				}
				if (CallingAETitle.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<CallingAETitle Value=\"{0}\">", CallingAETitle.Value);
					CallingAETitle.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</CallingAETitle>");
				}
				if (ApplicationContextName.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<ApplicationContextName Value=\"{0}\">", ApplicationContextName.Value);
					ApplicationContextName.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</ApplicationContextName>");
				}
				if (PresentationContexts.ContainsMessages() == true)
				{
					PresentationContexts.DvtSummaryToXml(streamWriter, level);
				}
				if (UserInformation.ContainsMessages() == true)
				{
					UserInformation.DvtSummaryToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</ValidationAssociateAc>");
				streamWriter.WriteLine("</ValidationResults>");
			}
			return true;
		}		

		/// <summary>
		/// Check if this contains any validation messages
		/// </summary>
		/// <returns>bool - contains validation messages true/false</returns>
		private bool ContainsMessages()
		{
			bool containsMessages = false;
			if ((ProtocolVersion.Messages.ErrorWarningCount() != 0) ||
				(CalledAETitle.Messages.ErrorWarningCount() != 0) ||
				(CallingAETitle.Messages.ErrorWarningCount() != 0) ||
				(ApplicationContextName.Messages.ErrorWarningCount() != 0) ||
				(PresentationContexts.ContainsMessages() == true) ||
				(UserInformation.ContainsMessages() == true))
			{
				containsMessages = true;
			}
			return containsMessages;
		}

        public void DvtTestLogToXml(StreamWriter streamWriter, string name)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<TestLog Type=\"DicomNetworkMessageValidation\">");
                streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<Name>{0}</Name>", name);
                // serialize parameter validations
                ProtocolVersion.DvtTestLogToXml(streamWriter, "Protocol Version");
                CalledAETitle.DvtTestLogToXml(streamWriter, "Called AE Title");
                CallingAETitle.DvtTestLogToXml(streamWriter, "Calling AE Title");
                ApplicationContextName.DvtTestLogToXml(streamWriter, "Application Context Name");
                // serialize requested transfer syntax validations
                foreach (ValidationAcsePresentationContextAccept presentationContextAc in PresentationContexts)
                {
                    presentationContextAc.DvtTestLogToXml(streamWriter, "");
                }

                // serialize user information validations
                UserInformation.DvtTestLogToXml(streamWriter, "");

                streamWriter.WriteLine("</TestLog>");
            }
        }
    }

    namespace SubItems
    {

        /// <summary>
        /// Validation information about the accepted presentation context.
        /// </summary>
        public class ValidationAcsePresentationContextAccept : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
        {
    
            /// <summary>
            /// Validation information about the presentation context identifier parameter.
            /// </summary>
            public ValidationAcseParameter PresentationContextId 
            {
                get 
                { 
                    return _PresentationContextId; 
                }
                set 
                { 
                    _PresentationContextId = value; 
                }
            } 
            private ValidationAcseParameter _PresentationContextId;
    
            /// <summary>
            /// Validation information about the abstract syntax name parameter.
            /// </summary>
            public ValidationAcseParameter AbstractSyntaxName 
            {
                get 
                { 
                    return _AbstractSyntaxName; 
                }
                set 
                { 
                    _AbstractSyntaxName = value; 
                }
            } 
            private ValidationAcseParameter _AbstractSyntaxName;
    
            /// <summary>
            /// Validation information about the result parameter.
            /// </summary>
            public ValidationAcseParameter Result 
            {
                get 
                { 
                    return _Result; 
                }
                set 
                { 
                    _Result = value; 
                }
            } 
            private ValidationAcseParameter _Result;
    
            /// <summary>
            /// Validation information about the transfer syntax name parameter.
            /// </summary>
            public ValidationAcseParameter TransferSyntaxName 
            {
                get 
                { 
                    return _TransferSyntaxName; 
                }
                set 
                { 
                    _TransferSyntaxName = value; 
                }
            } 
            private ValidationAcseParameter _TransferSyntaxName;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<PresentationContext>");
				streamWriter.WriteLine("<Id Value=\"{0}\">", PresentationContextId.Value);
				PresentationContextId.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</Id>");
				streamWriter.WriteLine("<Result Value=\"{0}\" Meaning=\"{1}\">", Result.Value, Result.Meaning);
				Result.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</Result>");
				streamWriter.WriteLine("<AbstractSyntaxName Value=\"{0}\" Meaning=\"{1}\">", AbstractSyntaxName.Value, AbstractSyntaxName.Meaning);
				AbstractSyntaxName.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</AbstractSyntaxName>");
				streamWriter.WriteLine("<TransferSyntax Value=\"{0}\" Meaning=\"{1}\">", TransferSyntaxName.Value, TransferSyntaxName.Meaning);
				TransferSyntaxName.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</TransferSyntax>");
				streamWriter.WriteLine("</PresentationContext>");
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
				if (this.ContainsMessages() == true)
				{
					streamWriter.WriteLine("<PresentationContext>");
					// always serialize the Id - used to uniquely identify a presentation context
					streamWriter.WriteLine("<Id Value=\"{0}\">", PresentationContextId.Value);
					PresentationContextId.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</Id>");
					if (Result.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<Result Value=\"{0}\" Meaning=\"{1}\">", Result.Value, Result.Meaning);
						Result.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</Result>");
					}
					if (AbstractSyntaxName.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<AbstractSyntaxName Value=\"{0}\" Meaning=\"{1}\">", AbstractSyntaxName.Value, AbstractSyntaxName.Meaning);
						AbstractSyntaxName.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</AbstractSyntaxName>");
					}
					if (TransferSyntaxName.Messages.ErrorWarningCount() != 0)
					{
						streamWriter.WriteLine("<TransferSyntax Value=\"{0}\" Meaning=\"{1}\">", TransferSyntaxName.Value, TransferSyntaxName.Meaning);
						TransferSyntaxName.DvtDetailToXml(streamWriter, level);
						streamWriter.WriteLine("</TransferSyntax>");
					}
					streamWriter.WriteLine("</PresentationContext>");
				}
				return true;
			}        	
	
			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				if ((PresentationContextId.Messages.ErrorWarningCount() != 0) ||
					(Result.Messages.ErrorWarningCount() != 0) ||
					(AbstractSyntaxName.Messages.ErrorWarningCount() != 0) ||
					(TransferSyntaxName.Messages.ErrorWarningCount() != 0))
				{
					containsMessages = true;
				}
				return containsMessages;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                AbstractSyntaxName.DvtTestLogToXml(streamWriter, "Abstract Syntax Name");
                PresentationContextId.DvtTestLogToXml(streamWriter, "Presentation Context Id");
                Result.DvtTestLogToXml(streamWriter, "Result");
                TransferSyntaxName.DvtTestLogToXml(streamWriter, "Transfer Syntax Name");
            }
        }
    }

    /// <summary>
    /// Validation information about the A_ASSOCIATE_RJ.
    /// </summary>
    public class ValidationAssociateRj : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
    {
		/// <summary>
		/// Message Unique Identifier
		/// </summary>
		public UInt32 MessageUID 
		{
			get 
			{ 
				return _MsgId; 
			}
			set 
			{ 
				_MsgId = value; 
			}
		} 
		private UInt32 _MsgId = 0;

        /// <summary>
        /// Validation information about the result parameter.
        /// </summary>
        public ValidationAcseParameter Result 
		{
			get 
            { 
                return _Result; 
            }
			set 
            { 
                _Result = value; 
            }
		} 
        private ValidationAcseParameter _Result;
    
        /// <summary>
        /// Validation information about the source parameter.
        /// </summary>
        public ValidationAcseParameter Source 
		{
			get 
            { 
                return _Source; 
            }
			set 
            {
                _Source = value; 
            }
		} 
        private ValidationAcseParameter _Source;
    
        /// <summary>
        /// Validation information about the reason parameter.
        /// </summary>
        public ValidationAcseParameter Reason 
		{
			get 
            { 
                return _Reason; 
            }
			set 
            { 
                _Reason = value; 
            }
		} 
        private ValidationAcseParameter _Reason;

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
                streamWriter.WriteLine("<ValidationResults>");
                streamWriter.WriteLine("<ValidationAssociateRj>");
				streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<Result Value=\"{0}\" Meaning=\"{1}\">", Result.Value, Result.Meaning);
                Result.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</Result>");
                streamWriter.WriteLine("<Source Value=\"{0}\" Meaning=\"{1}\">", Source.Value, Source.Meaning);
                Source.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</Source>");
                streamWriter.WriteLine("<Reason Value=\"{0}\" Meaning=\"{1}\">", Reason.Value, Reason.Meaning);
                Reason.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</Reason>");
                streamWriter.WriteLine("</ValidationAssociateRj>");
                streamWriter.WriteLine("</ValidationResults>");
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
                (this.ContainsMessages() == true))
			{
				streamWriter.WriteLine("<ValidationResults>");
				streamWriter.WriteLine("<ValidationAssociateRj>");
				if (Result.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<Result Value=\"{0}\" Meaning=\"{1}\">", Result.Value, Result.Meaning);
					Result.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</Result>");
				}
				if (Source.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<Source Value=\"{0}\" Meaning=\"{1}\">", Source.Value, Source.Meaning);
					Source.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</Source>");
				}
				if (Reason.Messages.ErrorWarningCount() != 0)
				{
					streamWriter.WriteLine("<Reason Value=\"{0}\" Meaning=\"{1}\">", Reason.Value, Reason.Meaning);
					Reason.DvtDetailToXml(streamWriter, level);
					streamWriter.WriteLine("</Reason>");
				}
				streamWriter.WriteLine("</ValidationAssociateRj>");
				streamWriter.WriteLine("</ValidationResults>");
			}
			return true;
		}		

		/// <summary>
		/// Check if this contains any validation messages
		/// </summary>
		/// <returns>bool - contains validation messages true/false</returns>
		private bool ContainsMessages()
		{
			bool containsMessages = false;
			if ((Result.Messages.ErrorWarningCount() != 0) ||
				(Source.Messages.ErrorWarningCount() != 0) ||
				(Reason.Messages.ErrorWarningCount() != 0))
			{
				containsMessages = true;
			}
			return containsMessages;
		}

        public void DvtTestLogToXml(StreamWriter streamWriter, string name)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<TestLog Type=\"DicomNetworkMessageValidation\">");
                streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<Name>{0}</Name>", name);
                // serialize parameter validations
                Reason.DvtTestLogToXml(streamWriter, "Reason");
                Result.DvtTestLogToXml(streamWriter, "Result");
                Source.DvtTestLogToXml(streamWriter, "Source");
                streamWriter.WriteLine("</TestLog>");
            }
        }
    }

    /// <summary>
    /// Validation information about the A_RELEASE_RQ.
    /// </summary>
    public class ValidationReleaseRq : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
    {
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
                streamWriter.WriteLine("<ValidationResults>");
                streamWriter.WriteLine("<ValidationReleaseRq>");
                streamWriter.WriteLine("</ValidationReleaseRq>");
                streamWriter.WriteLine("</ValidationResults>");
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
			return true;
		}

        public void DvtTestLogToXml(StreamWriter streamWriter, string name)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<TestLog Type=\"DicomNetworkMessageValidation\">");
                streamWriter.WriteLine("<Name>{0}</Name>", name);
                streamWriter.WriteLine("</TestLog>");
            }
        }
    }

    /// <summary>
    /// Validation information about the A_RELEASE_RP.
    /// </summary>
    public class ValidationReleaseRp : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
    {
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
                streamWriter.WriteLine("<ValidationResults>");
                streamWriter.WriteLine("<ValidationReleaseRp>");
                streamWriter.WriteLine("</ValidationReleaseRp>");
                streamWriter.WriteLine("</ValidationResults>");
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
			return true;
		}

        public void DvtTestLogToXml(StreamWriter streamWriter, string name)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<TestLog Type=\"DicomNetworkMessageValidation\">");
                streamWriter.WriteLine("<Name>{0}</Name>", name);
                streamWriter.WriteLine("</TestLog>");
            }
        }
    }

    namespace TypeSafeCollections
    {
        #region Type-safe collections
        /// <summary>
        /// Type safe ValidationAcsePresentationContextRequestCollection
        /// </summary>
        public sealed class ValidationAcsePresentationContextRequestCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml, IDvtSummaryToXml
        {

            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationAcsePresentationContextRequestCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationAcsePresentationContextRequestCollection(
                ValidationAcsePresentationContextRequest[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException("arrayOfValues");
                foreach (ValidationAcsePresentationContextRequest value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationAcsePresentationContextRequest this[int index]
            {
                get 
                { 
                    return (ValidationAcsePresentationContextRequest)base[index]; 
                }
                set 
                { 
                    base.Insert(index,value); 
                }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationAcsePresentationContextRequest value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationAcsePresentationContextRequest value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationAcsePresentationContextRequest value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationAcsePresentationContextRequest value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationAcsePresentationContextRequest value)
            {
                return base.Add(value);
            }

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<PresentationContexts>");
				foreach (ValidationAcsePresentationContextRequest validationAcsePresentationContextRequest in this)
				{
					validationAcsePresentationContextRequest.DvtDetailToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</PresentationContexts>");

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
				if (this.ContainsMessages() == true)
				{
					streamWriter.WriteLine("<PresentationContexts>");
					foreach (ValidationAcsePresentationContextRequest validationAcsePresentationContextRequest in this)
					{
						validationAcsePresentationContextRequest.DvtSummaryToXml(streamWriter, level);
					}
					streamWriter.WriteLine("</PresentationContexts>");
				}
				return true;
			}        		

			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				foreach (ValidationAcsePresentationContextRequest validationAcsePresentationContextRequest in this)
				{
					if (validationAcsePresentationContextRequest.ContainsMessages() == true)
					{
						containsMessages = true;
						break;
					}
				}
				return containsMessages;
			}			
		}

        /// <summary>
        /// Type safe ValidationAcsePresentationContextAcceptCollection
        /// </summary>
        public sealed class ValidationAcsePresentationContextAcceptCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml, IDvtSummaryToXml
        {

            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationAcsePresentationContextAcceptCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationAcsePresentationContextAcceptCollection(
                ValidationAcsePresentationContextAccept[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException("arrayOfValues");
                foreach (ValidationAcsePresentationContextAccept value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationAcsePresentationContextAccept this[int index]
            {
                get 
                { 
                    return (ValidationAcsePresentationContextAccept)base[index]; 
                }
                set 
                { 
                    base.Insert(index,value); 
                }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationAcsePresentationContextAccept value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationAcsePresentationContextAccept value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationAcsePresentationContextAccept value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationAcsePresentationContextAccept value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationAcsePresentationContextAccept value)
            {
                return base.Add(value);
            }

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<PresentationContexts>");
				foreach (ValidationAcsePresentationContextAccept validationAcsePresentationContextAccept in this)
				{
					validationAcsePresentationContextAccept.DvtDetailToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</PresentationContexts>");
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
				if (this.ContainsMessages() == true)
				{
					streamWriter.WriteLine("<PresentationContexts>");
					foreach (ValidationAcsePresentationContextAccept validationAcsePresentationContextAccept in this)
					{
						validationAcsePresentationContextAccept.DvtSummaryToXml(streamWriter, level);
					}
					streamWriter.WriteLine("</PresentationContexts>");
				}
				return true;
			}      
  
			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				foreach (ValidationAcsePresentationContextAccept validationAcsePresentationContextAccept in this)
				{
					if (validationAcsePresentationContextAccept.ContainsMessages() == true)
					{
						containsMessages = true;
						break;
					}
				}
				return containsMessages;
			}			
		}

        /// <summary>
        /// Type safe ValidationAcseParameterCollection
        /// </summary>
        public sealed class ValidationAcseParameterCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml, IDvtSummaryToXml
        {

            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationAcseParameterCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationAcseParameterCollection(
                ValidationAcseParameter[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException("arrayOfValues");
                foreach (ValidationAcseParameter value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationAcseParameter this[int index]
            {
                get 
                { 
                    return (ValidationAcseParameter)base[index]; 
                }
                set 
                { 
                    base.Insert(index,value); 
                }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationAcseParameter value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationAcseParameter value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationAcseParameter value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationAcseParameter value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationAcseParameter value)
            {
                return base.Add(value);
            }

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
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
				return true;
			}        
		
			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				foreach (ValidationAcseParameter validationAcseParameter in this)
				{
					if (validationAcseParameter.Messages.ErrorWarningCount() != 0)
					{
						containsMessages = true;
						break;
					}
				}
				return containsMessages;
			}						
		}

        /// <summary>
        /// Type safe ValidationAcseScpScuRoleSelectCollection
        /// </summary>
        public sealed class ValidationAcseScpScuRoleSelectCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml, IDvtSummaryToXml
        {

            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationAcseScpScuRoleSelectCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationAcseScpScuRoleSelectCollection(
                ValidationAcseScpScuRoleSelect[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException("arrayOfValues");
                foreach (ValidationAcseScpScuRoleSelect value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationAcseScpScuRoleSelect this[int index]
            {
                get 
                { 
                    return (ValidationAcseScpScuRoleSelect)base[index]; 
                }
                set 
                { 
                    base.Insert(index,value); 
                }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationAcseScpScuRoleSelect value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationAcseScpScuRoleSelect value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationAcseScpScuRoleSelect value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationAcseScpScuRoleSelect value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationAcseScpScuRoleSelect value)
            {
                return base.Add(value);
            }

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<ScpScuRoleSelections>");
				foreach (ValidationAcseScpScuRoleSelect validationAcseScpScuRoleSelect in this)
				{
					validationAcseScpScuRoleSelect.DvtDetailToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</ScpScuRoleSelections>");
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
				if (this.ContainsMessages() == true)
				{
					streamWriter.WriteLine("<ScpScuRoleSelections>");
					foreach (ValidationAcseScpScuRoleSelect validationAcseScpScuRoleSelect in this)
					{
						validationAcseScpScuRoleSelect.DvtSummaryToXml(streamWriter, level);
					}
					streamWriter.WriteLine("</ScpScuRoleSelections>");
				}
				return true;
			}        

			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				foreach (ValidationAcseScpScuRoleSelect validationAcseScpScuRoleSelect in this)
				{
					if (validationAcseScpScuRoleSelect.ContainsMessages() == true)
					{
						containsMessages = true;
						break;
					}
				}
				return containsMessages;
			}					
		}

        /// <summary>
        /// Type safe ValidationAcseSopClassExtendedCollection
        /// </summary>
        public sealed class ValidationAcseSopClassExtendedCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml, IDvtSummaryToXml
        {

            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationAcseSopClassExtendedCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationAcseSopClassExtendedCollection(
                ValidationAcseSopClassExtended[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException("arrayOfValues");
                foreach (ValidationAcseSopClassExtended value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationAcseSopClassExtended this[int index]
            {
                get 
                { 
                    return (ValidationAcseSopClassExtended)base[index]; 
                }
                set 
                { 
                    base.Insert(index,value); 
                }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationAcseSopClassExtended value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationAcseSopClassExtended value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationAcseSopClassExtended value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationAcseSopClassExtended value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationAcseSopClassExtended value)
            {
                return base.Add(value);
            }

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				streamWriter.WriteLine("<SopClassExtendedNegotiations>");
				foreach (ValidationAcseSopClassExtended validationAcseSopClassExtended in this)
				{
					validationAcseSopClassExtended.DvtDetailToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</SopClassExtendedNegotiations>");
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
				if (this.ContainsMessages() == true)
				{
					streamWriter.WriteLine("<SopClassExtendedNegotiations>");
					foreach (ValidationAcseSopClassExtended validationAcseSopClassExtended in this)
					{
						validationAcseSopClassExtended.DvtSummaryToXml(streamWriter, level);
					}
					streamWriter.WriteLine("</SopClassExtendedNegotiations>");
				}
				return true;
			}        
		
			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				foreach (ValidationAcseSopClassExtended validationAcseSopClassExtended in this)
				{
					if (validationAcseSopClassExtended.ContainsMessages() == true)
					{
						containsMessages = true;
						break;
					}
				}
				return containsMessages;
			}					
		}
        #endregion
    }
}
