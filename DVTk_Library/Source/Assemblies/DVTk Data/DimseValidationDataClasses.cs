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

using DvtkDicomUnicodeConversion;

namespace DvtkData.Validation
{
    using TypeSafeCollections;
    using SubItems;

    public class SerializeHelper
    {
        public static void serilalizeValidationMessages(ValidationMessageCollection validationMessages, StreamWriter streamWriter)
        {
            string result = "PASS";
            // Check if there are any messages
            if (validationMessages != null && validationMessages.Count > 0)
            {
                foreach (ValidationMessage validationMessage in validationMessages)
                {
                    // Check type of the message
                    switch (validationMessage.Type)
                    {
                        case MessageType.Error:
                            streamWriter.WriteLine("<Message Type=\"Error\">{0}</Message>", validationMessage.Message);
                            result = "FAIL";
                            break;
                        case MessageType.Warning:
                            streamWriter.WriteLine("<Message Type=\"Warning\">{0}</Message>", validationMessage.Message);
                            if (result != "FAIL")
                                result = "PASS_WITH_WARNING";
                            break;
                        case MessageType.Info:
                            streamWriter.WriteLine("<Message Type=\"Info\">{0}</Message>", validationMessage.Message);
                            break;
                        case MessageType.ConditionText:
                            if (validationMessage.Message.StartsWith("Condition not defined"))
                            {
                                streamWriter.WriteLine("<Message Type=\"Warning\">{0}</Message>", validationMessage.Message);
                                if (result != "FAIL")
                                    result = "PASS_WITH_WARNING";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            // write the result to the stream
            streamWriter.WriteLine("<Result>{0}</Result>", result);
        }
    }

    /// <summary>
    /// Validation information about the Information Object.
    /// </summary>
    /// <remarks>
    /// A Information Object may be validated against reference and/or definition objects.
    /// </remarks>
    public class ValidationObjectResult : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
    {
    
        /// <summary>
        /// Validation information about attributes from modules of the Information Object Definition.
        /// </summary>
        public ValidationAttributeGroupResultCollection Modules 
        {
            get 
            { 
                return _Modules; 
            }
            set 
            { 
                _Modules = value; 
            }
        } 
        private ValidationAttributeGroupResultCollection _Modules
            = new ValidationAttributeGroupResultCollection();
    
        /// <summary>
        /// Validation information about the additional dicom attributes, not part of the Information Object Definition.
        /// </summary>
        public ValidationAttributeGroupResult AdditionalAttributes 
        {
            get 
            { 
                return _AdditionalAttributes; 
            }
            set 
            { 
                _AdditionalAttributes = value; 
            }
        } 
        private ValidationAttributeGroupResult _AdditionalAttributes
            = new ValidationAttributeGroupResult();
    
        /// <summary>
        /// Validation informative messages.
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
        } 
        private ValidationMessageCollection _Messages
            = new ValidationMessageCollection();
    
        /// <summary>
        /// Identifying name.
        /// </summary>
        public string Name 
        {
            get 
            { 
                return _Name; 
            }
            set 
            { 
                _Name = value; 
            }
        } 
        private string _Name
            = System.String.Empty;

		/// <summary>
		/// Display Condition.
		/// </summary>
		public bool ConditionTextDisplay 
		{
			get 
			{ 
				return _Display; 
			}
			set 
			{ 
				_Display = value; 
			}
		} 
		private bool _Display = true;

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
		/// Identifying DICOMDIR name.
		/// </summary>
		public string DICOMDIRName 
		{
			get 
			{ 
				return _DICOMDIRName; 
			}
			set 
			{ 
				_DICOMDIRName = value; 
			}
		} 
		private string _DICOMDIRName
			= System.String.Empty;

        /// <summary>
        /// SpecificCharacterSetValues.
        /// </summary>
        public string SpecificCharacterSetValues
        {
            get
            {
                return _SpecificCharacterSetValues;
            }
            set
            {
                _SpecificCharacterSetValues = value;
            }
        }
        private string _SpecificCharacterSetValues
            = System.String.Empty;

        /// <summary>
        /// Table of Contents for Directory Records
        /// </summary>
        public ValidationDirectoryRecordLinkCollection DirectoryRecordTOC
        {
            get
            {
                return _DirectoryRecordTOC;
            }
            set
            {
                _DirectoryRecordTOC = value;
            }
        }
        private ValidationDirectoryRecordLinkCollection _DirectoryRecordTOC = null;

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
                // try to get the specific character set attribute
                // - this is needed to instantiate the DICOM to Unicode converter.
                // used fixed location for Unicode character sets based on the application start-up directory
                //String baseDirectory = String.Format("{0}CHARACTERSETS", System.AppDomain.CurrentDomain.BaseDirectory);
                String baseDirectory = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "CHARACTERSETS");
                DicomUnicodeConverter dicomUnicodeConverter = new DicomUnicodeConverter(baseDirectory);
                if (dicomUnicodeConverter.Install(_SpecificCharacterSetValues) == false)
                {
                    dicomUnicodeConverter = null;
                }

                streamWriter.WriteLine("<ValidationResults>");
                streamWriter.WriteLine("<ValidationDicomMessage>");
				streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<Name>{0}</Name>", Name);
				if(Name == "Media Directory")
					streamWriter.WriteLine("<DICOMDIRName>{0}</DICOMDIRName>", DICOMDIRName);
                Modules.DicomUnicodeConverter = dicomUnicodeConverter;
                Modules.DvtDetailToXml(streamWriter, level);
                if (AdditionalAttributes != null)
                {
                    AdditionalAttributes.DicomUnicodeConverter = dicomUnicodeConverter;
                    AdditionalAttributes.DvtDetailToXml(streamWriter, level);
                }
                Messages.DvtDetailToXml(streamWriter, level);
                if (DirectoryRecordTOC != null)
                {
                    DirectoryRecordTOC.DvtDetailToXml(streamWriter, level);
                }
                streamWriter.WriteLine("</ValidationDicomMessage>");
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
            if (
				(streamWriter != null) &&
				this.ContainsMessages()
				)
            {
                // try to get the specific character set attribute
                // - this is needed to instantiate the DICOM to Unicode converter.
                // used fixed location for Unicode character sets based on the application start-up directory
                //String baseDirectory = String.Format("{0}CHARACTERSETS", System.AppDomain.CurrentDomain.BaseDirectory);
                String baseDirectory = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "CHARACTERSETS");
                DicomUnicodeConverter dicomUnicodeConverter = new DicomUnicodeConverter(baseDirectory);
                if (dicomUnicodeConverter.Install(_SpecificCharacterSetValues) == false)
                {
                    dicomUnicodeConverter = null;
                }

                streamWriter.WriteLine("<ValidationResults>");
                streamWriter.WriteLine("<ValidationDicomMessage>");
				streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", MessageUID);
                streamWriter.WriteLine("<Name>{0}</Name>", Name);
				if(Name == "Media Directory")
					streamWriter.WriteLine("<DICOMDIRName>{0}</DICOMDIRName>", DICOMDIRName);

                if (Modules.ContainsMessages())
				{
                    Modules.DisplayConditionText = ConditionTextDisplay;
                    Modules.DicomUnicodeConverter = dicomUnicodeConverter;
                    Modules.DvtSummaryToXml(streamWriter, level);
				}
				if ((AdditionalAttributes != null) && 
					(AdditionalAttributes.ContainsMessages() == true))
                {
					AdditionalAttributes.DisplayConditionText = ConditionTextDisplay;
                    AdditionalAttributes.DicomUnicodeConverter = dicomUnicodeConverter;
                    AdditionalAttributes.DvtSummaryToXml(streamWriter, level);
                }
				
				Messages.DvtDetailToXml(streamWriter, level);
				
                if (DirectoryRecordTOC != null)
                {
                    DirectoryRecordTOC.DvtSummaryToXml(streamWriter, level);
                }
                streamWriter.WriteLine("</ValidationDicomMessage>");
                streamWriter.WriteLine("</ValidationResults>");
            }
			return true;
		}

        /// <summary>
        /// Serialize DVT Test Log to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        public void DvtTestLogToXml(StreamWriter streamWriter, string name)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<TestLog Type=\"DicomIODValidation\">");
                streamWriter.WriteLine("<MessageUID>{0}</MessageUID>", _MsgId);
                streamWriter.WriteLine("<Name>{0}</Name>", _Name);

                foreach (ValidationAttributeGroupResult module in _Modules)
                {
                    module.DvtTestLogToXml(streamWriter, "");
                }

                streamWriter.WriteLine("</TestLog>");
            }
        }

		/// <summary>
		/// Check if this contains any validation messages
		/// </summary>
		/// <returns>bool - contains validation messages true/false</returns>
		private bool ContainsMessages()
		{
			bool containsMessages = false;
			if ((Modules.ContainsMessages() == true) ||
				((AdditionalAttributes != null) && 
				 (AdditionalAttributes.ContainsMessages() == true)) ||
                (DirectoryRecordTOC != null) ||
				(Messages.ErrorWarningCount() != 0))
			{
				containsMessages = true;
			}
			return containsMessages;
		}
    }

    namespace SubItems
    {
        /// <summary>
        /// Validation information about the set of dicom attributes.
        /// </summary>
        public class ValidationAttributeGroupResult : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
        {

            /// <summary>
            /// Validation information about the set of dicom attributes.
            /// </summary>
            public ValidationAttributeResultCollection Attributes 
            {
                get 
                { 
                    return _Attributes; 
                }
                set 
                { 
                    _Attributes = value; 
                }
            } 
            private ValidationAttributeResultCollection _Attributes
                = new ValidationAttributeResultCollection();

            /// <summary>
            /// Validation informative messages.
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
            } 
            private ValidationMessageCollection _Messages
                = new ValidationMessageCollection();
    
            /// <summary>
            /// Identifying name.
            /// </summary>
            public string Name 
            {
                get 
                { 
                    return _Name; 
                }
                set 
                { 
                    _Name = value; 
                }
            } 
            private string _Name = System.String.Empty;
    
            /// <summary>
            /// Usage as defined in the Information Object Definition.
            /// </summary>
            public ModuleUsageType Usage 
            {
                get 
                { 
                    return _Usage; 
                }
                set 
                { 
                    _Usage = value; 
                }
            } 
            private ModuleUsageType _Usage;
        
            /// <summary>
            /// Holds a literal text specifying a condition which was not met during validation.
            /// </summary>
            public string ConditionText 
            {
                get 
                { 
                    return _ConditionText; 
                }
                set 
                {
                    _ConditionText = value; 
                }
            } 
            private string _ConditionText = System.String.Empty;

            /// <summary>
            /// DICOM to Unicode Converter
            /// </summary>
            public DicomUnicodeConverter DicomUnicodeConverter
            {
                set
                {
                    _dicomUnicodeConverter = value;
                }
            }
            private DicomUnicodeConverter _dicomUnicodeConverter = null;

            /// <summary>
            /// Display Condition text in summary result file
            /// </summary>
            public System.Boolean DisplayConditionText
            {
                set
                {
                    _DisplayConditionMsgs = value;
                }
            }
            private bool _DisplayConditionMsgs = false;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public virtual bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				string usage;
				switch(Usage)
				{
					case ModuleUsageType.Mandatory:
						usage = "M";
						break;
					case ModuleUsageType.Conditional:
						usage = "C";
						break;
					case ModuleUsageType.UserOptional:
					default:
						usage = "O";
						break;
				}
				if (level == 0)
				{
					streamWriter.WriteLine("<Module Name=\"{0}\" Usage=\"{1}\">", Name, usage);
				}
                Attributes.DicomUnicodeConverter = _dicomUnicodeConverter;
				Attributes.DvtDetailToXml(streamWriter, level);
				Messages.DvtDetailToXml(streamWriter, level);
				if (level == 0)
				{
					streamWriter.WriteLine("</Module>");
				}
				return true;
			}        

			/// <summary>
			/// Serialize DVT Summary Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public virtual bool DvtSummaryToXml(StreamWriter streamWriter, int level)
			{
				if (this.ContainsMessages() == true)
				{
					string usage;
					switch(Usage)
					{
						case ModuleUsageType.Mandatory:
							usage = "M";
							break;
						case ModuleUsageType.Conditional:
							usage = "C";
							break;
						case ModuleUsageType.UserOptional:
						default:
							usage = "O";
							break;
					}
					if (level == 0)
					{
						streamWriter.WriteLine("<Module Name=\"{0}\" Usage=\"{1}\">", Name, usage);
					}
					if (Attributes.ContainsMessages() == true)
					{
                        Attributes.DicomUnicodeConverter = _dicomUnicodeConverter;
                        Attributes.DisplayConditionText = _DisplayConditionMsgs;
                        Attributes.DvtSummaryToXml(streamWriter, level);
					}
					Messages.DvtDetailToXml(streamWriter, level);
					if (level == 0)
					{
						streamWriter.WriteLine("</Module>");
					}
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
				if ((Attributes.ContainsMessages() == true) ||
					(Messages.ErrorWarningCount() != 0))
				{
					containsMessages = true;
				}
				return containsMessages;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                // log module name and usage
                streamWriter.WriteLine("<Test Name=\"DicomModuleValidation\">");
                streamWriter.WriteLine("<TestItem ItemType=\"DicomModule\" Name=\"{0}\" Usage=\"{1}\" />", _Name, _Usage);
                // log module's test related messages
                SerializeHelper.serilalizeValidationMessages(_Messages, streamWriter);
                streamWriter.WriteLine("</Test>");

                // check attributes of the module
                foreach (ValidationAttributeResult attribute in _Attributes)
                {
                    // log attribute info
                    attribute.DvtTestLogToXml(streamWriter, "");
                }
            }
        }
    }

    namespace SubItems
    {
        /// <summary>
        /// Validation information about the dicom attribute.
        /// </summary>
        public class ValidationAttributeResult : IDvtDetailToXml, IDvtSummaryToXml, IDvtTestLogToXml
        {
    
            /// <summary>
            /// Collection of relevant values.
            /// </summary>
            public ValidationValueResultCollection Values 
            {
                get 
                { 
                    return _Values; 
                }
                set 
                { 
                    _Values = value; 
                }
            } 
            private ValidationValueResultCollection _Values = 
                new ValidationValueResultCollection();
    
            /// <summary>
            /// Validation informative messages.
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
            } 
            private ValidationMessageCollection _Messages = 
                new ValidationMessageCollection();
    
            /// <summary>
            /// Tag of the attribute.
            /// </summary>
            public DvtkData.Dimse.Tag Tag 
            {
                get 
                { 
                    return _Tag; 
                }
                set 
                { 
                    _Tag = value; 
                }
            } 
            private DvtkData.Dimse.Tag _Tag = new DvtkData.Dimse.Tag(0x0000, 0x0000);
    
            /// <summary>
            /// Value representation of the attribute as defined in the definition file
            /// </summary>
            public DvtkData.Dimse.VR ValueRepresentation 
            {
                get 
                { 
                    return _ValueRepresentation; 
                }
                set 
                { 
                    _ValueRepresentation = value; 
                }
            } 
            private DvtkData.Dimse.VR _ValueRepresentation = DvtkData.Dimse.VR.UN;

			/// <summary>
			/// This property decides about Actual VR display in attribute validation result
			/// </summary>
			public bool DisplayVR 
			{
				get 
				{ 
					return _Display; 
				}
				set 
				{ 
					_Display = value; 
				}
			} private bool _Display = true;
    
			/// <summary>
			/// Value representation of the attribute as defined in the dataset
			/// </summary>
			public DvtkData.Dimse.VR ActualValueRepresentation 
			{
				get 
				{ 
					return _ActualValueRepresentation; 
				}
				set 
				{ 
					_ActualValueRepresentation = value; 
				}
			} 
			private DvtkData.Dimse.VR _ActualValueRepresentation = DvtkData.Dimse.VR.UN;

            /// <summary>
            /// Type of the attribute according to the DataElement definition by the Information Object Definition.
            /// </summary>
            /// <remarks>Possible values are 1, 1C, 2, 2C, 3, 3C, 3R</remarks>
            public DataElementType DataElementType 
            {
                get 
                { 
                    return _DataElementType; 
                }
                set 
                { 
                    _DataElementType = value; 
                }
            } 
            private DataElementType _DataElementType = DataElementType.Item1;

            /// <summary>
            /// Attribute Length - only used for serialization.
            /// </summary>
            public System.UInt32 Length
            {
                get 
                { 
                    return _length; 
                }
                set 
                { 
                    _length = value; 
                }
            } private System.UInt32 _length = 0;

            /// <summary>
            /// Presence of the attribute in the message.
            /// </summary>
            public System.Boolean Presence
            {
                get 
                { 
                    return _Presence; 
                }
                set 
                { 
                    _Presence = value; 
                }
            }
            private System.Boolean _Presence = false;

            /// <summary>
            /// This is a literal string identifying the attribute by name.
            /// </summary>
            public string Name 
            {
                get 
                { 
                    return _Name; 
                }
                set 
                { 
                    _Name = value; 
                }
            } 
            private string _Name = string.Empty;

            /// <summary>
            /// DICOM to Unicode Converter
            /// </summary>
            public DicomUnicodeConverter DicomUnicodeConverter
            {
                set
                {
                    _dicomUnicodeConverter = value;
                }
            }
            private DicomUnicodeConverter _dicomUnicodeConverter = null;

            /// <summary>
            /// Display Condition text in summary result file
            /// </summary>
            public System.Boolean DisplayConditionText
            {
                get
                {
                    return _DisplayConditionMsgs;
                }
                set
                {
                    _DisplayConditionMsgs = value;
                }
            }
            private bool _DisplayConditionMsgs = false;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
                string group = Tag.GroupNumber.ToString("X").PadLeft(4, '0');
                string element = Tag.ElementNumber.ToString("X").PadLeft(4, '0');
				string elementType;
				switch(DataElementType)
				{
					case DataElementType.Item1:
						elementType = "1";
						break;
					case DataElementType.Item1C:
						elementType = "1C";
						break;
					case DataElementType.Item2:
						elementType = "2";
						break;
					case DataElementType.Item2C:
						elementType = "2C";
						break;
					case DataElementType.Item3:
						elementType = "3";
						break;
					case DataElementType.Item3C:
						elementType = "3C";
						break;
					case DataElementType.Item3R:
					default:
						elementType = "3R";
						break;
				}

                string length = Length.ToString();
                if (Length == 0xFFFFFFFF)
                {
                    length = "UNDEFINED";
                }

				string presence = "-";				
				if (Presence) 
				{
					presence = "+";
				}				

				string vr = "-";
				if(DisplayVR)
				{
					if (Presence) 
					{
						vr = ActualValueRepresentation.ToString();
					}					
				}					

				streamWriter.WriteLine("<Attribute Group=\"{0}\" Element=\"{1}\" Type=\"{2}\" DefVR=\"{3}\" DcmVR=\"{4}\" Length=\"{5}\" Present = \"{6}\" Name=\"{7}\">", 
					group, 
					element, 
					elementType, 
					ValueRepresentation.ToString(),
					vr,
					length, 
					presence, 
					Name);

                Values.DicomUnicodeConverter = _dicomUnicodeConverter;
                Values.DvtDetailToXml(streamWriter, level);
				Messages.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</Attribute>");
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
                    string group = Tag.GroupNumber.ToString("X").PadLeft(4, '0');
                    string element = Tag.ElementNumber.ToString("X").PadLeft(4, '0');
					string elementType;
					switch(DataElementType)
					{
						case DataElementType.Item1:
							elementType = "1";
							break;
						case DataElementType.Item1C:
							elementType = "1C";
							break;
						case DataElementType.Item2:
							elementType = "2";
							break;
						case DataElementType.Item2C:
							elementType = "2C";
							break;
						case DataElementType.Item3:
							elementType = "3";
							break;
						case DataElementType.Item3C:
							elementType = "3C";
							break;
						case DataElementType.Item3R:
						default:
							elementType = "3R";
							break;
					}
					string length = Length.ToString();
					if (Length == 0xFFFFFFFF)
					{
						length = "UNDEFINED";
					}
					string presence = "-";
					if (Presence) 
					{
						presence = "+";
					}
					streamWriter.WriteLine("<Attribute Group=\"{0}\" Element=\"{1}\" Type=\"{2}\" DefVR=\"{3}\" DcmVR=\"{4}\" Length=\"{5}\" Present = \"{6}\" Name=\"{7}\">", 
						group, 
						element, 
						elementType, 
						ValueRepresentation.ToString(), 
						ActualValueRepresentation.ToString(), 
						length, 
						presence, 
						Name);
                    Values.DicomUnicodeConverter = _dicomUnicodeConverter;
                    Values.DisplayConditionText = _DisplayConditionMsgs;
                    Values.DvtSummaryToXml(streamWriter, level);
					//Messages.DvtDetailToXml(streamWriter, level);
                    foreach (ValidationMessage msg in this.Messages)
                    {
                        if (msg.Type == MessageType.ConditionText)
                        {
                            if (DisplayConditionText)
                            {
                                msg.DvtDetailToXml(streamWriter, level);
                            }
                        }
                        else
                        {
                            msg.DvtDetailToXml(streamWriter, level);
                        }
                    }
					streamWriter.WriteLine("</Attribute>");
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
				if ((Values.ContainsMessages() == true) ||
					(Messages.ErrorWarningCount() != 0) ||
					(Messages.ConditionTextCount() != 0))
				{
					containsMessages = true;
				}
				return containsMessages;
			}

            public void DvtTestLogToXml(StreamWriter streamWriter, string name)
            {
                string group = _Tag.GroupNumber.ToString("X").PadLeft(4, '0');
                string element = _Tag.ElementNumber.ToString("X").PadLeft(4, '0');
                string elementType;
                switch (_DataElementType)
                {
                    case DataElementType.Item1:
                        elementType = "1";
                        break;
                    case DataElementType.Item1C:
                        elementType = "1C";
                        break;
                    case DataElementType.Item2:
                        elementType = "2";
                        break;
                    case DataElementType.Item2C:
                        elementType = "2C";
                        break;
                    case DataElementType.Item3:
                        elementType = "3";
                        break;
                    case DataElementType.Item3C:
                        elementType = "3C";
                        break;
                    case DataElementType.Item3R:
                    default:
                        elementType = "3R";
                        break;
                }

                string length = _length.ToString();
                if (_length == 0xFFFFFFFF)
                {
                    length = "UNDEFINED";
                }

                string presence = "-";
                if (_Presence)
                {
                    presence = "+";
                }

                string vr = "-";
                if (_Display)
                {
                    if (_Presence)
                    {
                        vr = _ActualValueRepresentation.ToString();
                    }
                }

                streamWriter.WriteLine("<Test Name=\"DicomAttributeValidation\">");
                streamWriter.WriteLine("<TestItem ItemType=\"DicomAttribute\" Group=\"{0}\" Element=\"{1}\" Type=\"{2}\" DefVR=\"{3}\" DcmVR=\"{4}\" Length=\"{5}\" Present = \"{6}\" Name=\"{7}\" />",
                    group,
                    element,
                    elementType,
                    _ValueRepresentation.ToString(),
                    vr,
                    length,
                    presence,
                    _Name);

                //this.Values.DicomUnicodeConverter = attribute._dicomUnicodeConverter;
                //this.Values.DvtDetailToXml(streamWriter, level);

                SerializeHelper.serilalizeValidationMessages(_Messages, streamWriter);
                streamWriter.WriteLine("</Test>");
            }
        }
    }

    namespace SubItems
    {

        /// <summary>
        /// Validation information about the dicom value.
        /// </summary>
        public class ValidationValueResult : IDvtDetailToXml, IDvtSummaryToXml
        {
            /// <summary>
            /// DICOM to Unicode Converter
            /// </summary>
            public DicomUnicodeConverter DicomUnicodeConverter
            {
                set
                {
                    _dicomUnicodeConverter = value;
                }
            }
            private DicomUnicodeConverter _dicomUnicodeConverter = null;

            /// <summary>
            /// Display Condition text in summary result file
            /// </summary>
            public System.Boolean DisplayConditionText
            {
                set
                {
                    _DisplayConditionMsgs = value;
                }
            }
            private bool _DisplayConditionMsgs = false;
    
            /// <summary>
            /// Validation informative messages.
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
            } 
            private ValidationMessageCollection _Messages = new ValidationMessageCollection();
    
            /// <summary>
            /// Validation information about the value item.
            /// </summary>
            /// <remarks>
            /// The <c>Item</c> is either a <c>ValueItem</c> <see cref="System.String"/> representing the value 
            /// or a <c>Sequence</c> array of <see cref="DvtkData.Validation.SubItems.ValidationAttributeGroupResult"/> items 
            /// holding validation information about teh sequence.
            /// </remarks>
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
            private object _Item = System.String.Empty;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				if (Item == null) return true;
				if (Item is ValidationAttributeGroupResult[])
				{
					ValidationAttributeGroupResult[] sequence = (ValidationAttributeGroupResult[])Item;
					streamWriter.WriteLine("<Sequence>");
					int itemCount = 1;
					foreach(ValidationAttributeGroupResult item in sequence)
					{
						streamWriter.WriteLine("<Item Number=\"{0}\">", itemCount++);
						item.DvtDetailToXml(streamWriter, level + 1);
						streamWriter.WriteLine("</Item>");
					}
					streamWriter.WriteLine("</Sequence>");
				}
				else if (Item is string)
				{
					string stringValue = (string)Item;
					streamWriter.WriteLine("<Value>{0}</Value>", DvtToXml.ConvertString(stringValue,true));

                    // try to convert the string to Unicode - if possible
                    if (_dicomUnicodeConverter != null)
                    {
                        String outString = DvtToXml.ConvertStringToXmlUnicode(_dicomUnicodeConverter, stringValue);
                        if (outString != String.Empty)
                        {
                            streamWriter.WriteLine("<Unicode>{0}</Unicode>", outString);
                        }
                    }
                }
				else
				{
					throw new System.NotSupportedException();
				}
				Messages.DvtDetailToXml(streamWriter, level);
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
				if (Item == null) return true;
				if (Item is ValidationAttributeGroupResult[])
				{
					if (this.ContainsMessages() == true)
					{
						ValidationAttributeGroupResult[] sequence = (ValidationAttributeGroupResult[])Item;
						streamWriter.WriteLine("<Sequence>");
						int itemCount = 1;
						foreach(ValidationAttributeGroupResult item in sequence)
						{
							if (item.ContainsMessages() == true)
							{
                                item.DisplayConditionText = _DisplayConditionMsgs;
								streamWriter.WriteLine("<Item Number=\"{0}\">", itemCount);
								item.DvtSummaryToXml(streamWriter, level + 1);
								streamWriter.WriteLine("</Item>");
							}
							itemCount++;
						}
						streamWriter.WriteLine("</Sequence>");
					}
				}
				else if (Item is string)
				{
					string stringValue = (string)Item;
					streamWriter.WriteLine("<Value>{0}</Value>", DvtToXml.ConvertString(stringValue,true));

                    // try to convert the string to Unicode - if possible
                    if (_dicomUnicodeConverter != null)
                    {
                        String outString = DvtToXml.ConvertStringToXmlUnicode(_dicomUnicodeConverter, stringValue);
                        if (outString != String.Empty)
                        {
                            streamWriter.WriteLine("<Unicode>{0}</Unicode>", outString);
                        }
                    }
				}
				else
				{
					throw new System.NotSupportedException();
				}
			    Messages.DvtDetailToXml(streamWriter, level);
				return true;
			}    
    		
			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{

				if (Item == null) 
				{
					return false;
				}

				bool containsMessages = false;
				if (Item is ValidationAttributeGroupResult[])
				{
					ValidationAttributeGroupResult[] sequence = (ValidationAttributeGroupResult[])Item;
					foreach(ValidationAttributeGroupResult item in sequence)
					{
						if (item.ContainsMessages() == true)
						{
							containsMessages = true;
							break;
						}
					}
				}
				else if (Item is string)
				{
					containsMessages = (Messages.ErrorWarningCount() > 0);
				}
				else
				{
					throw new System.NotSupportedException();
				}
				return containsMessages;
			}		
		}
    }

    /// <summary>
    /// Validation message
    /// </summary>
    /// <remarks>The basic output of the validation against a set of rules.</remarks>
    public class ValidationMessage : IDvtDetailToXml
    {
    
        /// <summary>
        /// Unique index number for the message.
        /// </summary>
        public System.UInt32 Index 
        {
            get 
            { 
                return _Index; 
            }
            set 
            { 
                _Index = value; 
            }
        } 
        private System.UInt32 _Index;

        /// <summary>
        /// Identifier depicting the validation rule that generated the message.
        /// </summary>
        public System.UInt32 Identifier 
        {
            get 
            { 
                return _Identifier; 
            }
            set 
            { 
                _Identifier = value; 
            }
        } 
        private System.UInt32 _Identifier;
    
        /// <summary>
        /// The type of message. Determines the message severity level.
        /// </summary>
        public MessageType Type 
        {
            get 
            { 
                return _Type; 
            }
            set 
            {
                _Type = value; 
            }
        }
        private MessageType _Type = MessageType.Info;
    
        /// <summary>
        /// The message literal string.
        /// </summary>
        public string Message 
        {
            get 
            { 
                return _Message; 
            }
            set 
            { 
                _Message = value; 
            }
        } 
        private string _Message = System.String.Empty;

		/// <summary>
		/// Serialize DVT Detail Data to Xml.
		/// </summary>
		/// <param name="streamWriter">Stream writer to serialize to.</param>
		/// <param name="level">Recursion level. 0 = Top.</param> 
		/// <returns>bool - success/failure</returns>
		public bool DvtDetailToXml(StreamWriter streamWriter, int level)
		{
			streamWriter.WriteLine("<Message Index=\"{0}\">", Index.ToString());
			streamWriter.WriteLine("<Id>0x{0}</Id>", Identifier.ToString("X"));
			switch(Type)
			{
				case MessageType.Error:
					streamWriter.WriteLine("<Type>Error</Type>");
					break;
				case MessageType.Warning:
					streamWriter.WriteLine("<Type>Warning</Type>");
					break;
				case MessageType.Info:
					streamWriter.WriteLine("<Type>Info</Type>");
					break;
				case MessageType.ConditionText:
					streamWriter.WriteLine("<Type>ConditionText</Type>");
					break;
				case MessageType.Debug:
					streamWriter.WriteLine("<Type>Debug</Type>");
					break;
				case MessageType.None:
				default:
					streamWriter.WriteLine("<Type>None</Type>");
					break;
			}
			streamWriter.WriteLine("<Meaning>{0}</Meaning>", Message);
			streamWriter.WriteLine("</Message>");
			return true;
		}
	}

    /// <summary>
    /// Message types. Mainly severity levels.
    /// </summary>
    public enum MessageType 
    {
    
        /// <summary>
        /// Error level.
        /// </summary>
        Error,    
        /// <summary>
        /// Warning level.
        /// </summary>
        Warning,
        /// <summary>
        /// Information level.
        /// </summary>
        Info,
        /// <summary>
        /// Debug related information.
        /// </summary>
        Debug,
        /// <summary>
        /// Text level.
        /// </summary>
		ConditionText,
		/// <summary>
		/// Unspecified level.
		/// </summary>
		None,
    }

    /// <summary>
    /// Type of the attribute according to the DataElement definition by the Information Object Definition.
    /// </summary>
    public enum DataElementType 
    {
        /// <summary>
        /// TYPE 1 REQUIRED DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// IODs and SOP Classes define Type 1 Data Elements that shall be included and are mandatory elements.
        /// The Value Field shall contain valid data as defined by the elements VR and VM as specified in PS 3.6.
        /// The Length of the Value Field shall not be zero. Absence of a valid Value in a Type 1 Data Element is a protocol violation.
        /// </remarks>
        Item1,
        /// <summary>
        /// TYPE 1 CONDITIONAL DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// <p>
        /// IODs and SOP Classes define Data Elements that shall be included under certain specified conditions.
        /// Type 1C elements have the same requirements as Type 1 elements under these conditions. It is a
        /// protocol violation if the specified conditions are met and the Data Element is not included.
        /// </p>
        /// <p>
        /// When the specified conditions are not met, Type 1C elements shall not be included in the Data Set.
        /// </p>
        /// </remarks>
        Item1C,
        /// <summary>
        /// TYPE 2 REQUIRED DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// <p>
        /// IODs and SOP Classes define Type 2 Data Elements that shall be included and are mandatory Data
        /// Elements. However, it is permissible that if a Value for a Type 2 element is unknown it can be encoded
        /// with zero Value Length and no Value. If the Value is known the Value Field shall contain that value as
        /// defined by the elements VR and VM as specified in PS 3.6. These Data Elements shall be included in the
        /// Data Set and their absence is a protocol violation.
        /// </p>
        /// <p>
        /// Note: The intent of Type 2 Data Elements is to allow a zero length to be conveyed when the operator or
        /// application does not know its value or has a specific reason for not specifying its value. It is the intent that
        /// the device should support these Data Elements.
        /// </p>
        /// </remarks>
        Item2,
        /// <summary>
        /// TYPE 2 CONDITIONAL DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// <p>
        /// IODs and SOP Classes define Type 2C elements that have the same requirements as Type 2 elements
        /// under certain specified conditions. It is a protocol violation if the specified conditions are met and the Data
        /// Element is not included.
        /// </p>
        /// <p>
        /// When the specified conditions are not met, Type 2C elements shall not be included in the Data Set.
        /// </p>
        /// <p>
        /// Note: An example of a Type 2C Data Element is Inversion Time (0018,0082). For several SOP Class
        /// Definitions, this Data Element is required only if the Scanning Sequence (0018,0020) has the Value “IR.”
        /// It is not required otherwise. See PS 3.3.
        /// </p>
        /// </remarks>
        Item2C,
        /// <summary>
        /// TYPE 3 OPTIONAL DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// IODs and SOP Classes define Type 3 Data Elements that are optional Data Elements. Absence of a
        /// Type 3 element from a Data Set does not convey any significance and is not a protocol violation. Type 3
        /// elements may also be encoded with zero length and no Value. The meaning of a zero length Type 3 Data
        /// Element shall be precisely the same as that element being absent from the Data Set.
        /// </remarks>
        Item3,    
        /// <summary>
        /// TYPE 3 OPTIONAL DATA ELEMENTS
        /// </summary>
        Item3C,    
        /// <summary>
        /// TYPE 3 OPTIONAL DATA ELEMENTS
        /// </summary>
        Item3R,
    }

    /// <summary>
    /// Usage as defined in the Information Object Definition.
    /// </summary>
    public enum ModuleUsageType 
    {
    
        /// <summary>
        /// Conditional
        /// </summary>
        Conditional,
        /// <summary>
        /// User optional
        /// </summary>
        UserOptional,
        /// <summary>
        /// Manadatory
        /// </summary>
        Mandatory,
    }

    /// <summary>
    /// Directory record type.
    /// </summary>
    public enum DirectoryRecordType 
    {
        /// <summary>
        /// First Record of the Root Directory Entity
        /// </summary>
        ROOT,
        /// <summary>
        /// Patient Directory Record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to 
        /// reference a Detached Patient Management SOP Instance.
        /// </remarks>
        PATIENT,
        /// <summary>
        /// Study Directory record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to
        /// reference a Detached Study Management SOP Instance.
        /// </remarks>
        STUDY,
        /// <summary>
        /// Series Directory Record
        /// </summary>
        /// <remarks>
        /// The description of these keys may be found 
        /// in the Modules related to the Series IE and Equipment IE of Image IODs.
        /// </remarks>
        SERIES,
        /// <summary>
        /// Image directory record
        /// </summary>
        /// <remarks>
        /// The description of these keys may be found in the Modules related to
        /// the Image IE of Image IODs.
        /// </remarks>
        IMAGE,
        /// <summary>
        /// Standalone overlay directory record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference a Standalone
        /// Overlay Image SOP Instance.
        /// </remarks>
        OVERLAY,
        /// <summary>
        /// Standalone modality LUT directory record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to
        /// reference a Standalone Modality LUT SOP Instance.
        /// </remarks>
        MODALITY_LUT,
        /// <summary>
        /// Standalone VOI LUT directory record
        /// </summary>
        /// <remarks>
        /// Directory Record shall be used to reference a Standalone
        /// VOI LUT SOP Instance.
        /// </remarks>
        VOI_LUT,
        /// <summary>
        /// Standalone curve directory record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference a Standalone
        /// Curve SOP Instance.
        /// </remarks>
        CURVE,
        /// <summary>
        /// Topic directory record
        /// </summary>
        /// <remarks>
        /// The Topic Directory Record is intended to collect a set of subordinate Directory Records. 
        /// It uses simple textual descriptions as keys.
        /// </remarks>
        TOPIC,
        /// <summary>
        /// Visit directory record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference a Detached Visit Management SOP Instance.
        /// </remarks>
        VISIT,
        /// <summary>
        /// Results directory record
        /// </summary>
        /// <remarks>
        /// Directory Record shall be used to reference a Detached Results
        /// Management SOP Instance.
        /// </remarks>
        RESULTS,
        /// <summary>
        /// Interpretation directory record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference an Interpretation
        /// SOP Instance.
        /// </remarks>
        INTERPRETATION,
        /// <summary>
        /// Study component directory record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference
        /// a Study Component SOP Instance.
        /// </remarks>
        STUDY_COMPONENT,
        /// <summary>
        /// Print Queue Directory Record
        /// </summary>
        /// <remarks>
        /// It is now retired.
        /// </remarks>
        PRINT_QUEUE,
        /// <summary>
        /// Film session directory record
        /// </summary>
        /// <remarks>
        /// It is now retired.
        /// </remarks>
        FILM_SESSION,
        /// <summary>
        /// Film box directory record
        /// </summary>
        /// <remarks>
        /// It is now retired.
        /// </remarks>
        FILM_BOX,
        /// <summary>
        /// Basic image box directory record
        /// </summary>
        /// <remarks>
        /// It is now retired.
        /// </remarks>
        IMAGE_BOX,
		/// <summary>
		/// Registration Directory Record
		/// </summary>
		/// <remarks>
		/// This Directory Record shall be used to reference a Registration
		/// SOP Instance.
		/// </remarks>
		REGISTRATION,
		/// <summary>
		/// Raw Data Directory Record
		/// </summary>
		/// <remarks>
		/// This Directory Record shall be used to reference a
		/// Raw Data SOP instance.
		/// </remarks>
		RAW_DATA,
		/// <summary>
		/// Encap Doc Directory Record
		/// </summary>
		/// <remarks>
		/// This Directory Record shall be used to reference a Encap Doc SOP instance.
		/// </remarks>
		ENCAP_DOC,
		/// <summary>
		/// Spectroscopy Directory Record
		/// </summary>
		/// <remarks>
		/// This Directory Record shall be used to reference a Spectroscopy SOP instance.
		/// </remarks>
		SPECTROSCOPY,
		/// <summary>
		/// Fiducial Directory Record
		/// </summary>
		/// <remarks>
		/// This Directory Record shall be used to reference an 
		/// Fiducial SOP instance, 
		/// </remarks>
		FIDUCIAL,
        /// <summary>
        /// Stored Print Directory Record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference a Stored
        /// Print SOP Instance.
        /// </remarks>
        STORED_PRINT,
        /// <summary>
        /// RT Dose Directory Record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference a
        /// RT Dose SOP instance.
        /// </remarks>
        RT_DOSE,
        /// <summary>
        /// RT Structure Set Directory Record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference a RT Structure Set SOP instance.
        /// </remarks>
        RT_STRUCTURE_SET,
        /// <summary>
        /// RT Plan Directory Record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference a RT Plan SOP instance.
        /// </remarks>
        RT_PLAN,
        /// <summary>
        /// RT Treatment Record Directory Record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference an 
        /// RT Beams Treatment Record SOP instance, 
        /// RT Brachy Treatment Record SOP instance, 
        /// or 
        /// RT Treatment Summary Record SOP instance.
        /// </remarks>
        RT_TREAT_RECORD,
        /// <summary>
        /// Presentation State Directory Record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference a 
        /// Grayscale Softcopy Presentation State Storage SOP Instance.
        /// </remarks>
        PRESENTATION,
        /// <summary>
        /// Waveform Directory Record
        /// </summary>
        /// <remarks>
        /// Directory Record shall be used to reference a Waveform SOP Instance.
        /// </remarks>
        WAVEFORM,
        /// <summary>
        /// SR Document Directory Record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference an SR Document.
        /// </remarks>
        SR_DOCUMENT,
        /// <summary>
        /// Key Object Document Directory Record
        /// </summary>
        /// <remarks>
        /// This Directory Record shall be used to reference a Key Object Selection Document.
        /// </remarks>
        KEY_OBJECT_DOC,
        /// <summary>
        /// SPECIAL DIRECTORY RECORDS: Private directory record
        /// </summary>
        /// <remarks>
        /// Private Directory Records may also be used in addition 
        /// to Standard defined Directory Records.
        /// </remarks>
        PRIVATE,
        /// <summary>
        /// SPECIAL DIRECTORY RECORDS: Multi-referenced file directory record
        /// </summary>
        /// <remarks>
        /// The Multi-Referenced File Directory Record (MRDR) is a special type of 
        /// Directory Record which allows indirect reference to a File by multiple Directory Records. 
        /// Such a Directory Record facilitates the management (deletion/update) of referenced 
        /// Files by keeping an explicit count of the number of references.
        /// </remarks>
        MRDR,
        /// <summary>
        /// This Directory Record shall be used to reference a Hanging Protocol SOP Instance.
        /// </summary>
        HANGING_PROTOCOL,
        /// <summary>
        /// This Directory Record shall be used to reference an HL7 Structured Document and any of its referenced
        /// content stored on the interchange media but not encapsulated in a DICOM SOP Instance. The
        /// document may be encoded as an XML document with in-line multimedia content, or may be encoded
        /// in a multi-part MIME wrapper (see PS3.10).
        /// </summary>
        HL7_STRUC_DOC,
        /// <summary>
        /// This Directory Record shall be used to reference a Stereometric Relationship SOP Instance.
        /// </summary>
        STEREOMETRIC,
        /// <summary>
        /// This Directory Record shall be used to reference a Real World Value Mapping SOP Instance.
        /// </summary>
        VALUE_MAP,
        /// <summary>
        /// Unknown directory record.
        /// </summary>
        UNKNOWN
    }

    namespace SubItems
    {
        /// <summary>
        /// (sematical) Link to the validation information of a directory record.
        /// </summary>
        /// <remarks>
        /// Validation of a dicom media file can result large quantities of
        /// validation results output. Therefore, the validation information for the individual
        /// dicom directory records has be split-off into seperate validation results output files.
        /// This link describes the link to the split-off validation results of the directory records.
        /// </remarks>
        public class ValidationDirectoryRecordLink : IDvtDetailToXml,  IDvtSummaryToXml
        {
            /// <summary>
            /// Index of the directory record.
            /// </summary>
            /// <remarks>
            /// This index may be used to find the corresponding validation output results
            /// for this directory record.
            /// </remarks>
            public System.UInt32 DirectoryRecordIndex
            {
                get
                {
                    return _DirectoryRecordIndex;
                }
                set
                {
                    _DirectoryRecordIndex = value;
                }
            }
            private System.UInt32 _DirectoryRecordIndex;

            /// <summary>
            /// Directory record type
            /// </summary>
            public DirectoryRecordType DirectoryRecordType
            {
                get
                {
                    return _DirectoryRecordType;
                }
                set
                {
                    _DirectoryRecordType = value;
                }
            }
            private DirectoryRecordType _DirectoryRecordType;
            
            /// <summary>
            /// Number of validation errors for this directory record.
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
            /// Number of validation warnings for this directory record.
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
            /// Offset of this directory record within the Directory Record Sequence 
            /// (of the DIRECTORY INFORMATION MODULE).
            /// </summary>
            public System.UInt32 RecordOffset
            {
                get
                {
                    return _RecordOffset;
                }
                set
                {
                    _RecordOffset = value;
                }
            }
            private System.UInt32 _RecordOffset;

            /// <summary>
            /// Number of times the directory record is referenced.
            /// </summary>
            public System.UInt16 ReferenceCount
            {
                get
                {
                    return _ReferenceCount;
                }
                set
                {
                    _ReferenceCount = value;
                }
            }
            private System.UInt16 _ReferenceCount;

            /// <summary>
            /// Validation informative messages.
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
            } 
            private ValidationMessageCollection _Messages
                = new ValidationMessageCollection();    

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				string recordOffset = RecordOffset.ToString("X");
				while (recordOffset.Length < 8)
				{
					recordOffset = "0" + recordOffset;
				}
				streamWriter.WriteLine("<DirectoryRecordLink Index=\"{0}\" Type=\"{1}\" NrOfErrors=\"{2}\" NrOfWarnings=\"{3}\" RecordOffset=\"0x{4}={5}\" ReferenceCount=\"{6}\">",
					DirectoryRecordIndex.ToString(), 
					DirectoryRecordType.ToString(),
					NrOfErrors.ToString(),
					NrOfWarnings.ToString(),
					recordOffset,
					RecordOffset.ToString(),
					ReferenceCount.ToString());
				Messages.DvtDetailToXml(streamWriter, level);
				streamWriter.WriteLine("</DirectoryRecordLink>");
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
                if ((NrOfErrors != 0) ||
                    (Messages.ErrorWarningCount() != 0))
                {
                    string recordOffset = RecordOffset.ToString("X");
                    while (recordOffset.Length < 8)
                    {
                        recordOffset = "0" + recordOffset;
                    }
                    streamWriter.WriteLine("<DirectoryRecordLink Index=\"{0}\" Type=\"{1}\" NrOfErrors=\"{2}\" NrOfWarnings=\"{3}\" RecordOffset=\"0x{4}={5}\" ReferenceCount=\"{6}\">",
                        DirectoryRecordIndex.ToString(), 
                        DirectoryRecordType.ToString(),
                        NrOfErrors.ToString(),
                        NrOfWarnings.ToString(),
                        recordOffset,
                        RecordOffset.ToString(),
                        ReferenceCount.ToString());
                    Messages.DvtDetailToXml(streamWriter, level);
                    streamWriter.WriteLine("</DirectoryRecordLink>");
                }
                return true;
            }
		}
    }

    /// <summary>
    /// Validation information about directory records.
    /// </summary>
    /// <remarks>Directory records are part of dicom media.</remarks>
    public class ValidationDirectoryRecordResult : ValidationAttributeGroupResult 
    {
        /// <summary>
        /// Identifier for the dicom directory
        /// </summary>
        public System.UInt32 DirectoryRecordIndex
        {
            get 
            { 
                return _DirectoryRecordIndex; 
            }
            set 
            { 
                _DirectoryRecordIndex = value; 
            }
        }
        private System.UInt32 _DirectoryRecordIndex;
        /// <summary>
        /// Identifies the type of directory record validated.
        /// </summary>
        public DirectoryRecordType DirectoryRecordType 
        {
            get 
            { 
                return _DirectoryRecordType; 
            }
            set 
            { 
                _DirectoryRecordType = value; 
            }
        } 
        private DirectoryRecordType _DirectoryRecordType;
        /// <summary>
        /// Corresponding referenced file
        /// </summary>
        public ValidationObjectResult ReferencedFile
        {
            get 
            { 
                return _ReferencedFile; 
            }
            set 
            { 
                _ReferencedFile = value; 
            }
        }
        private ValidationObjectResult _ReferencedFile;

        /// <summary>
        /// Number of validation errors for this directory record.
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
        /// Number of validation warnings for this directory record.
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
		public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
		{
            if (streamWriter != null)
            {
                streamWriter.WriteLine("<DirectoryRecord Index=\"{0}\" Type=\"{1}\">",
                    DirectoryRecordIndex.ToString(), 
                    DirectoryRecordType.ToString());
                streamWriter.WriteLine("<ValidationResults>");
                streamWriter.WriteLine("<ValidationDirectoryRecord>");
                Attributes.DvtDetailToXml(streamWriter, level);
                Messages.DvtDetailToXml(streamWriter, level);
                streamWriter.WriteLine("</ValidationDirectoryRecord>");
                streamWriter.WriteLine("</ValidationResults>");
                streamWriter.WriteLine("</DirectoryRecord>");
                if (ReferencedFile != null)
                {
                    streamWriter.WriteLine("<ReferencedFile>");
                    ReferencedFile.DvtDetailToXml(streamWriter, level);
                    streamWriter.WriteLine("</ReferencedFile>");
                }
            }

			return true;
		}        

		/// <summary>
		/// Serialize DVT Summary Data to Xml.
		/// </summary>
		/// <param name="streamWriter">Stream writer to serialize to.</param>
		/// <param name="level">Recursion level. 0 = Top.</param> 
		/// <returns>bool - success/failure</returns>
		public override bool DvtSummaryToXml(StreamWriter streamWriter, int level)
		{
            if (streamWriter != null)
            {
                if ((this.ContainsMessages() == true) ||
                    (NrOfErrors != 0))
                {
                    streamWriter.WriteLine("<DirectoryRecord Index=\"{0}\" Type=\"{1}\">",
                        DirectoryRecordIndex.ToString(), 
                        DirectoryRecordType.ToString());
                    streamWriter.WriteLine("<ValidationResults>");
                    streamWriter.WriteLine("<ValidationDirectoryRecord>");
                    if (Attributes.ContainsMessages() == true)
                    {
                        Attributes.DvtSummaryToXml(streamWriter, level);
                    }
                    Messages.DvtDetailToXml(streamWriter, level);
                    streamWriter.WriteLine("</ValidationDirectoryRecord>");
                    streamWriter.WriteLine("</ValidationResults>");
                    streamWriter.WriteLine("</DirectoryRecord>");
                    if (ReferencedFile != null)
                    {
                        streamWriter.WriteLine("<ReferencedFile>");
                        ReferencedFile.DvtSummaryToXml(streamWriter, level);
                        streamWriter.WriteLine("</ReferencedFile>");
                    }
                }
			}

			return true;
		}        	
    }

    namespace TypeSafeCollections
    {
        #region Type-safe collections
        /// <summary>
        /// Type-safe ValidationAttributeGroupResultCollection
        /// </summary>
        public sealed class ValidationAttributeGroupResultCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml, IDvtSummaryToXml
        {
            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationAttributeGroupResultCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationAttributeGroupResultCollection(
                ValidationAttributeGroupResult[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException();
                foreach (ValidationAttributeGroupResult value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationAttributeGroupResult this[int index]
            {
                get { return (ValidationAttributeGroupResult)base[index]; }
                set { base.Insert(index,value); }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationAttributeGroupResult value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationAttributeGroupResult value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationAttributeGroupResult value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationAttributeGroupResult value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationAttributeGroupResult value)
            {
                return base.Add(value);
            }

            /// <summary>
            /// DICOM to Unicode Converter
            /// </summary>
            public DicomUnicodeConverter DicomUnicodeConverter
            {
                set
                {
                    _dicomUnicodeConverter = value;
                }
            }
            private DicomUnicodeConverter _dicomUnicodeConverter = null;

            /// <summary>
            /// Display Condition text in summary result file
            /// </summary>
            public System.Boolean DisplayConditionText
            {                
                set
                {
                    _DisplayConditionMsgs = value;
                }
            }
            private bool _DisplayConditionMsgs = false;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				bool result = false;

				foreach (ValidationAttributeGroupResult validationAttributeGroupResult in this)
				{
                    validationAttributeGroupResult.DicomUnicodeConverter = _dicomUnicodeConverter;
					result = validationAttributeGroupResult.DvtDetailToXml(streamWriter, level);
				}

				return result;
			}    

			/// <summary>
			/// Serialize DVT Summary Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtSummaryToXml(StreamWriter streamWriter, int level)
			{
				bool result = true;

				if (this.ContainsMessages() == true)
				{
					foreach (ValidationAttributeGroupResult validationAttributeGroupResult in this)
					{
                        validationAttributeGroupResult.DicomUnicodeConverter = _dicomUnicodeConverter;
                        validationAttributeGroupResult.DisplayConditionText = _DisplayConditionMsgs;
                        result = validationAttributeGroupResult.DvtSummaryToXml(streamWriter, level);
					}
				}

				return result;
			}    		

			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				foreach (ValidationAttributeGroupResult validationAttributeGroupResult in this)
				{
					if (validationAttributeGroupResult.ContainsMessages() == true)
					{
						containsMessages = true;
						break;
					}
				}
				return containsMessages;
			}		
		}

        /// <summary>
        /// Type-safe ValidationDirectoryRecordLinkCollection
        /// </summary>
        public sealed class ValidationDirectoryRecordLinkCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml, IDvtSummaryToXml
        {
            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationDirectoryRecordLinkCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationDirectoryRecordLinkCollection(
                ValidationDirectoryRecordLink[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException();
                foreach (ValidationDirectoryRecordLink value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationDirectoryRecordLink this[int index]
            {
                get { return (ValidationDirectoryRecordLink)base[index]; }
                set { base.Insert(index,value); }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationDirectoryRecordLink value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationDirectoryRecordLink value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationDirectoryRecordLink value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationDirectoryRecordLink value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationDirectoryRecordLink value)
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
				bool result = true;

                if (this.Count != 0)
                {
                    streamWriter.WriteLine("<DirectoryRecordLinks>");
                    foreach (ValidationDirectoryRecordLink validationDirectoryRecordLink in this)
                    {
                        result = validationDirectoryRecordLink.DvtDetailToXml(streamWriter, level);
                    }
                    streamWriter.WriteLine("</DirectoryRecordLinks>");
                }

				return result;
			}  
  
            /// <summary>
            /// Serialize DVT Summary Data to Xml.
            /// </summary>
            /// <param name="streamWriter">Stream writer to serialize to.</param>
            /// <param name="level">Recursion level. 0 = Top.</param> 
            /// <returns>bool - success/failure</returns>
            public bool DvtSummaryToXml(StreamWriter streamWriter, int level)
            {
                bool result = true;

                if (this.Count != 0)
                {
                    streamWriter.WriteLine("<DirectoryRecordLinks>");
                    foreach (ValidationDirectoryRecordLink validationDirectoryRecordLink in this)
                    {
                        result = validationDirectoryRecordLink.DvtSummaryToXml(streamWriter, level);
                    }
                    streamWriter.WriteLine("</DirectoryRecordLinks>");
                }

                return result;
            }    
		}

        /// <summary>
        /// Type-safe ValidationMessageCollection
        /// </summary>
        public sealed class ValidationMessageCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
        {
            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationMessageCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationMessageCollection(
                ValidationMessage[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException();
                foreach (ValidationMessage value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationMessage this[int index]
            {
                get { return (ValidationMessage)base[index]; }
                set { base.Insert(index,value); }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationMessage value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationMessage value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationMessage value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationMessage value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationMessage value)
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
				bool result = false;

				foreach (ValidationMessage value in base.List)
				{
					result = value.DvtDetailToXml(streamWriter, level);
				}

				return result;
			}

			/// <summary>
			/// Get the number of Error and Warning messages in the collection.
			/// </summary>
			/// <returns>int - Number of Error and Warning messages in collection</returns>
			public int ErrorWarningCount()
			{
				int errorWarningCount = 0;

				foreach (ValidationMessage value in base.List)
				{
					if ((value.Type == MessageType.Error) ||
						(value.Type == MessageType.Warning))
					{
						errorWarningCount++;
					}
				}

				return errorWarningCount;
			}

			/// <summary>
			/// Get the number of ConditionText messages in the collection.
			/// </summary>
			/// <returns>int - Number of ConditionText messages in collection</returns>
			public int ConditionTextCount()
			{
				int conditionTextCount = 0;

				foreach (ValidationMessage value in base.List)
				{
					if (value.Type == MessageType.ConditionText)
					{
						conditionTextCount++;
					}
				}

				return conditionTextCount;
			}

		}

        /// <summary>
        /// Type-safe ValidationAttributeResultCollection
        /// </summary>
        public sealed class ValidationAttributeResultCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml, IDvtSummaryToXml
        {
            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationAttributeResultCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationAttributeResultCollection(
                ValidationAttributeResult[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException();
                foreach (ValidationAttributeResult value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationAttributeResult this[int index]
            {
                get { return (ValidationAttributeResult)base[index]; }
                set { base.Insert(index,value); }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationAttributeResult value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationAttributeResult value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationAttributeResult value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationAttributeResult value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationAttributeResult value)
            {
                return base.Add(value);
            }

            /// <summary>
            /// DICOM to Unicode Converter
            /// </summary>
            public DicomUnicodeConverter DicomUnicodeConverter
            {
                set
                {
                    _dicomUnicodeConverter = value;
                }
            }
            private DicomUnicodeConverter _dicomUnicodeConverter = null;

            /// <summary>
            /// Display Condition text in summary result file
            /// </summary>
            public System.Boolean DisplayConditionText
            {
                set
                {
                    _DisplayConditionMsgs = value;
                }
            }
            private bool _DisplayConditionMsgs = false;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				bool result = false;

				foreach (ValidationAttributeResult validationAttributeResult in this)
				{
                    validationAttributeResult.DicomUnicodeConverter = _dicomUnicodeConverter;
					result = validationAttributeResult.DvtDetailToXml(streamWriter, level);
				}

				return result;
			}                    

			/// <summary>
			/// Serialize DVT Summary Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtSummaryToXml(StreamWriter streamWriter, int level)
			{
				bool result = true;

				if (this.ContainsMessages() == true)
				{
					foreach (ValidationAttributeResult validationAttributeResult in this)
					{
                        validationAttributeResult.DicomUnicodeConverter = _dicomUnicodeConverter;
                        validationAttributeResult.DisplayConditionText = _DisplayConditionMsgs;
                        result = validationAttributeResult.DvtSummaryToXml(streamWriter, level);
					}
				}

				return result;
			}         
           		
			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				foreach (ValidationAttributeResult validationAttributeResult in this)
				{
					if (validationAttributeResult.ContainsMessages() == true)
					{
						containsMessages = true;
						break;
					}
				}
				return containsMessages;
			}				
		}

        /// <summary>
        /// Type-safe ValidationValueResultCollection
        /// </summary>
        public sealed class ValidationValueResultCollection
            : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml, IDvtSummaryToXml
        {
            /// <summary>
            /// Default constructor.
            /// </summary>
            public ValidationValueResultCollection() {}

            /// <summary>
            /// Constructor with initialization. Shallow copy.
            /// </summary>
            /// <param name="arrayOfValues">values to copy.</param>
            /// <exception cref="System.ArgumentNullException">Argument <c>arrayOfValues</c> is a <see langword="null"/> reference.</exception>
            public ValidationValueResultCollection(
                ValidationValueResult[] arrayOfValues)
            {
                if (arrayOfValues == null) throw new System.ArgumentNullException();
                foreach (ValidationValueResult value in arrayOfValues) this.Add(value);
            }

            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>The item at the specified <c>index</c>.</value>
            public new ValidationValueResult this[int index]
            {
                get { return (ValidationValueResult)base[index]; }
                set { base.Insert(index,value); }
            }

            /// <summary>
            /// Inserts an item to the IList at the specified position.
            /// </summary>
            /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
            /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
            public void Insert(int index, ValidationValueResult value)
            {
                base.Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific item from the IList.
            /// </summary>
            /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
            public void Remove(ValidationValueResult value)
            {
                base.Remove(value);
            }

            /// <summary>
            /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
            public bool Contains(ValidationValueResult value)
            {
                return base.Contains(value);
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
            /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
            public int IndexOf(ValidationValueResult value)
            {
                return base.IndexOf(value);
            }

            /// <summary>
            /// Adds an item to the <see cref="System.Collections.IList"/>.
            /// </summary>
            /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
            /// <returns>The position into which the new element was inserted.</returns>
            public int Add(ValidationValueResult value)
            {
                return base.Add(value);
            }

            /// <summary>
            /// DICOM to Unicode Converter
            /// </summary>
            public DicomUnicodeConverter DicomUnicodeConverter
            {
                set
                {
                    _dicomUnicodeConverter = value;
                }
            }
            private DicomUnicodeConverter _dicomUnicodeConverter = null;

            /// <summary>
            /// Display Condition text in summary result file
            /// </summary>
            public System.Boolean DisplayConditionText
            {
                set
                {
                    _DisplayConditionMsgs = value;
                }
            }
            private bool _DisplayConditionMsgs = false;

			/// <summary>
			/// Serialize DVT Detail Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtDetailToXml(StreamWriter streamWriter, int level)
			{
				bool result = false;

				streamWriter.WriteLine("<Values>");
				foreach (ValidationValueResult validationValueResult in this)
				{
                    validationValueResult.DicomUnicodeConverter = _dicomUnicodeConverter;
					result = validationValueResult.DvtDetailToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</Values>");

				return result;
			}                   

			/// <summary>
			/// Serialize DVT Summary Data to Xml.
			/// </summary>
			/// <param name="streamWriter">Stream writer to serialize to.</param>
			/// <param name="level">Recursion level. 0 = Top.</param> 
			/// <returns>bool - success/failure</returns>
			public bool DvtSummaryToXml(StreamWriter streamWriter, int level)
			{
				bool result = false;

				streamWriter.WriteLine("<Values>");
				foreach (ValidationValueResult validationValueResult in this)
				{
                    validationValueResult.DicomUnicodeConverter = _dicomUnicodeConverter;
                    validationValueResult.DisplayConditionText = _DisplayConditionMsgs;
                    result = validationValueResult.DvtSummaryToXml(streamWriter, level);
				}
				streamWriter.WriteLine("</Values>");

				return result;
			}           
        		
			/// <summary>
			/// Check if this contains any validation messages
			/// </summary>
			/// <returns>bool - contains validation messages true/false</returns>
			public bool ContainsMessages()
			{
				bool containsMessages = false;
				foreach (ValidationValueResult validationValueResult in this)
				{
					if (validationValueResult.ContainsMessages() == true)
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
