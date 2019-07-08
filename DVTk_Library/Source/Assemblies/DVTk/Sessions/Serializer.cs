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
using DvtkData.DvtDetailToXml;
using DvtkData.ComparisonResults;

namespace Dvtk.Sessions
{

    /// <summary>
    /// Serialization Target class for the detailed and summary results
    /// </summary>
    internal class SerializationWriter
    {
        internal enum State
        {
            Init,
            DocumentStarted,
            RootElementStarted,
            RootElementEnded,
            DocumentEnded,
        }
        private State state = State.Init;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fullPathname">Results pathname.</param>
        /// <param name="parent">bool indicating if the targets are for parent (true) or child (false) documents</param> 
        /// <param name="index">Results index - 0 = parent, non-zero = child.</param>
        /// <param name="detailRequired">bool indicating if detailed results should be generated.</param>
        /// <param name="summaryRequired">bool indicating if summary results should be generated.</param>
        public SerializationWriter(string fullPathname, bool parent, uint index , bool detailRequired, bool summaryRequired, bool testLogRequired)
        {
            this.state = State.Init;
            //
            // Generate the required pathnames
            //
            string dirName = System.IO.Path.GetDirectoryName(fullPathname);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(fullPathname);
            string extension = System.IO.Path.GetExtension(fullPathname);
            //
            // Default to null.
            //
            m_detailedPathname = null;
            m_summaryPathname = null;
            m_testLogPathName = null;
            //
            // set the detailed pathname
            //
            if (detailRequired)
            {
                m_detailedPathname = string.Empty;
                m_detailedPathname += "Detail_";
                m_detailedPathname += fileName;
                m_detailedPathname += parent ? string.Empty : index.ToString();
                m_detailedPathname += extension;
                m_detailedPathname = System.IO.Path.Combine(dirName, m_detailedPathname);
                m_detailStreamWriter = new StreamWriter(m_detailedPathname);
            }
            //
            // set the summary pathname
            //
            if (summaryRequired)
            {
                m_summaryPathname = string.Empty;
                m_summaryPathname += "Summary_";
                m_summaryPathname += fileName;
                m_summaryPathname += parent ? string.Empty : index.ToString();
                m_summaryPathname += extension;
                m_summaryPathname = System.IO.Path.Combine(dirName, m_summaryPathname);
                m_summaryStreamWriter = new StreamWriter(m_summaryPathname);
            }
            //
            // set the test log pathname
            //
            if (false)
            {
                m_testLogPathName = string.Empty;
                m_testLogPathName += "TestLog_";
                m_testLogPathName += fileName;
                m_testLogPathName += parent ? string.Empty : index.ToString();
                m_testLogPathName += extension;
                m_testLogPathName = System.IO.Path.Combine(dirName, m_testLogPathName);
                m_testLogStreamWriter = new StreamWriter(m_testLogPathName);
            }
        }
        private readonly string m_detailedPathname;
        private readonly string m_summaryPathname;
        private readonly string m_testLogPathName;

        public void WriteStartDocument()
        {
            if (this.state != State.Init) 
            {
                throw new System.ApplicationException(
                    string.Format("Expected State.Init instead found state: {0}", this.state));
            }
            this.state = State.DocumentStarted;
            //
            // set up the detailed stream writer
            //
            if (m_detailedPathname != null)
            {
                m_detailStreamWriter.WriteLine("<?xml version=\"1.0\"?>");
            }
            //
            // set up the summary stream writer
            //
            if (m_summaryPathname != null)
            {
                m_summaryStreamWriter.WriteLine("<?xml version=\"1.0\"?>");
            }
            //
            // set up the test log stream writer
            //
            if (m_testLogPathName != null)
            {
                m_testLogStreamWriter.WriteLine("<?xml version=\"1.0\"?>");
            }
        }

        public void WriteStartElement()
        {
            if (this.state != State.DocumentStarted) 
            {
                throw new System.ApplicationException(
                    string.Format("Expected State.DocumentStarted instead found state: {0}", this.state));
            }
            this.state = State.RootElementStarted;
            //
            // set up the detailed stream writer
            //
            if (m_detailedPathname != null)
            {
                m_detailStreamWriter.WriteLine("<DvtDetailedResultsFile>");
				m_detailStreamWriter.WriteLine("<DvtDetailedResultsFilename>{0}</DvtDetailedResultsFilename>", m_detailedPathname);
                if (m_summaryPathname != null)
                {
                    //
                    // add link to summary file
                    //
                    m_detailStreamWriter.WriteLine("<DvtSummaryResultsFilename>{0}</DvtSummaryResultsFilename>", m_summaryPathname);
                }
            }
            //
            // set up the summary stream writer
            //
            if (m_summaryPathname != null)
            {
                m_summaryStreamWriter.WriteLine("<DvtSummaryResultsFile>");
				m_summaryStreamWriter.WriteLine("<DvtSummaryResultsFilename>{0}</DvtSummaryResultsFilename>", m_summaryPathname);
                if (m_detailedPathname != null)
                {
                    //
                    // add link to detail file
                    //
                    m_summaryStreamWriter.WriteLine("<DvtDetailedResultsFilename>{0}</DvtDetailedResultsFilename>", m_detailedPathname);
                }
            }
            //
            // set up the test log stream writer
            //
            if (m_testLogPathName != null)
            {
                m_testLogStreamWriter.WriteLine("<DvtTestLogFile>");
                m_testLogStreamWriter.WriteLine("<DvtTestLogFilename>{0}</DvtTestLogFilename>", m_testLogPathName);
            }
        }

        /// <summary>
        /// Closes one element and pops the corresponding namespace scope.
        /// </summary>
        public void WriteEndElement()
        {
            if (this.state != State.RootElementStarted) 
            {
                throw new System.ApplicationException(
                    string.Format("Expected State.RootElementStarted instead found state: {0}", this.state));
            }
            this.state = State.RootElementEnded;
            //
            // set up the detailed stream writer
            //
            if (m_detailStreamWriter != null)
            {
                m_detailStreamWriter.WriteLine("</DvtDetailedResultsFile>");
            }
            //
            // set up the summary stream writer
            //
            if (m_summaryStreamWriter != null)
            {
                m_summaryStreamWriter.WriteLine("</DvtSummaryResultsFile>");
            }
            //
            // set up the test log stream writer
            //
            if (m_testLogPathName != null)
            {
                m_testLogStreamWriter.WriteLine("</DvtTestLogFile>");
            }
        }

        /// <summary>
        /// Closes any open elements or attributes and puts the writer back in the Start state.
        /// </summary>
        public void WriteEndDocument()
        {
            if (this.state != State.RootElementEnded) 
            {
                throw new System.ApplicationException(
                    string.Format("Expected State.RootElementEnded instead found state: {0}", this.state));
            }
            this.state = State.DocumentEnded;
            //
            // set up the detailed stream writer
            //
            if (m_detailStreamWriter != null)
            {
                m_detailStreamWriter.Close();
            }
            //
            // set up the summary stream writer
            //
            if (m_summaryStreamWriter != null)
            {
                m_summaryStreamWriter.Close();
            }
            //
            // set up the test log stream writer
            //
            if (m_testLogStreamWriter != null)
            {
                m_testLogStreamWriter.Close();
            }
        }

        /// <summary>
        /// Property to get the test results stream writer.
        /// </summary>
        public StreamWriter TestLogStreamWriter
        {
            get
            {
                return this.m_testLogStreamWriter;
            }
        }
        private readonly StreamWriter m_testLogStreamWriter;

        /// <summary>
        /// Property to get the detail stream writer.
        /// </summary>
        public StreamWriter DetailStreamWriter
        {
            get 
            { 
                return this.m_detailStreamWriter; 
            }
        }
        private readonly StreamWriter m_detailStreamWriter;

        /// <summary>
        /// Property to get the summary stream writer.
        /// </summary>
        public StreamWriter SummaryStreamWriter
        {
            get 
            { 
                return this.m_summaryStreamWriter; 
            }
        }
        private readonly StreamWriter m_summaryStreamWriter;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>The \n characters are replaced by &#xA; in the XML output!
    /// #x9 (tab)
    /// #xA (line feed) 
    /// #xD (carriage return)
    /// #x20 (space)
    /// see ms-help://MS.MSDNQTR.2003JUL.1033/cpgenref/html/xsdrefsinglecharacterescape.htm</remarks>
    internal class Serializer
        : Wrappers.ISerializationTarget
    {
        /// <summary>
        /// Status of the serialization in the workflow;
        /// Start, Stop
        /// </summary>
        internal enum SerializationStatus
        {
            Running,
            Stopped,
        }

        private SerializationWriter m_serializationWriter = null;
        private readonly System.Collections.ArrayList m_childSerializers = new System.Collections.ArrayList();
        public DvtkData.Results.Counters m_endCounters = new DvtkData.Results.Counters();
        private bool m_bEndCountsApplied = false;
        private readonly bool m_bIsTopSerializer = false;
        private readonly Session m_parentSession;
        private readonly Serializer m_parentSerializer = null;
        //
        // Intermediate count tables are used during the serialization of directory records.
        // This counting is performed on the top-level serializer.
        // All serialization of child directory records updates the counts on the top.
        // The parent top-serializer generates the TOC for the directory records.
        //
        private readonly System.Collections.Hashtable topDirectoryRecordErrorCountTable;
        private readonly System.Collections.Hashtable topDirectoryRecordWarningCountTable;

		private static UInt32 m_messageIndex = 1;
		private static UInt32 m_dimsedul_messageIndex = 1;

		private UInt32 GetNextMessageIndex()
		{
			return m_messageIndex++;
		}

		private UInt32 GetNextDIMSEDULMessageIndex()
		{
			return m_dimsedul_messageIndex++;
		}

        internal Serializer(
            Serializer parentSerializer,
            Session session, 
            bool isTopSerializer,
            Wrappers.WrappedSerializerNodeType reason)
        {
            this.m_serializerNodeType = reason;
            this.m_parentSerializer = parentSerializer;
            this.m_parentSession = session;
            this.m_bIsTopSerializer = isTopSerializer;
            if (this.m_bIsTopSerializer)
            {
                this.topDirectoryRecordErrorCountTable = new System.Collections.Hashtable();
                this.topDirectoryRecordWarningCountTable = new System.Collections.Hashtable();
            }
            else
            {
                this.topDirectoryRecordErrorCountTable = null;
                this.topDirectoryRecordWarningCountTable = null;
            }
        }

        private readonly Wrappers.WrappedSerializerNodeType m_serializerNodeType = 
            Wrappers.WrappedSerializerNodeType.TopParent;

        private void Init(
            int indexOfChildInParentCollection,
            Wrappers.WrappedSerializerNodeType reason)
        {
            //
            // Use same settings for detailed and summary validation as in the parent serializer.
            //
            bool detailedValidationResultsEnabled = this.m_parentSession.DetailedValidationResults;
            bool detailedSummaryValidationResultsEnabled = this.m_parentSession.SummaryValidationResults;
            bool testLogValidationResultsEnabled = this.m_parentSession.TestLogValidationResults;
            //
            // Create a output writer based on the same targetname as the parent serializer.
            //
            this.m_serializationWriter = 
                new SerializationWriter(
                this.m_targetFileName,
                this.m_bIsTopSerializer,
                (uint)indexOfChildInParentCollection,
                detailedValidationResultsEnabled,
                detailedSummaryValidationResultsEnabled,
                testLogValidationResultsEnabled);
            //
            // TODO: Determine specific processing tasks for Thread and DirectoryRecord child-nodes.
            //
            switch (reason)
            {
                case Wrappers.WrappedSerializerNodeType.Thread:
                    break;
                case Wrappers.WrappedSerializerNodeType.DirectoryRecord:
                    break;
                case Wrappers.WrappedSerializerNodeType.TopParent:
                    break;
                default:
                    // Unknown WrappedSerializerNodeType
                    throw new System.NotImplementedException();
            }
            //
            // Start the serializer
            //
            this.m_serializationWriter.WriteStartDocument();
            this.m_serializationWriter.WriteStartElement();
            this.m_SerializationStatus = SerializationStatus.Running;
        }

        /// <summary>
        /// Factory method to create child-node for this parent-node.
        /// </summary>
        /// <param name="reason"></param>
        /// <returns>child-node</returns>
        public Wrappers.ISerializationTarget CreateChildSerializationTarget(
            Wrappers.WrappedSerializerNodeType reason)
        {
            if (!this.m_bIsTopSerializer)
            {
                throw new System.ApplicationException(
                    string.Concat(
                    "Is is not possible to create child-documents nested deeper than one-level, ",
                    "due to limitations in the target file naming-scheme."));
            }
            //
            // Create child-serializer for the same (parent)-session.
            //
            Serializer childSerializer = 
                new Serializer(this, this.m_parentSession, false, reason);
            childSerializer.m_targetFileName = this.m_targetFileName;
            //
            // Register child-serializer in child-collection of this parent-serializer.
            //
            this.m_childSerializers.Add(childSerializer);
            childSerializer.Init(
                this.m_childSerializers.IndexOf(childSerializer),
                reason);
            return childSerializer;
        }

        /// <summary>
        /// Stop serialization process. 
        /// Writes a closure to the serialization target stream.
        /// Performs internal cleanup.
        /// </summary>
        public void StopTopSerializer()
        {
            // End Serialization BottomUp!!!
            // Delegate stopping the serialization process to the adapter-layer.
            // The adapter-layer has object-handles for the lower unmanaged-layers as well as 
            // call-back interface-handles to the upper managed-layers.
            // It is the task of the adapter layer to request actual cleanup.
            // The adapter layer is the master.
            //
            // Terminating the targets causes the counts to be updated to the end counts.
            //
            this.m_parentSession.m_MBaseSession.StopSerializationProcessing();
        }

        /// <summary>
        /// DO NOT CALL FROM UI-LAYER. Call between managed adapter and manages layers.
        /// </summary>
        public void EndSerializationTarget()
        {
            if (this.m_SerializationStatus == SerializationStatus.Stopped)
            {
                //
                // Avoid re-entry.
                //
                throw new System.ApplicationException(
                    "Trying to stop a serialization target that is already stopped.");
            }
            //
            // Set to ended state.
            //
            this.m_SerializationStatus = SerializationStatus.Stopped;
            //
            // Precondition: Check that childs have been closed.
            //
            foreach (Serializer childSerializer in this.m_childSerializers)
            {
                if (childSerializer.m_SerializationStatus != SerializationStatus.Stopped)
                {
                    throw new System.ApplicationException(
                        "Parent SerializationTarget node is being ended while Child SerializationTarget node has ended yet.");
                }
            }
            //
            // Write sub results index reference in parent of this child.
            //
            if (this.m_parentSerializer != null)
            {
                DvtkData.Results.SubResultsLink subResultsLink = new DvtkData.Results.SubResultsLink();
                int index = this.m_parentSerializer.m_childSerializers.IndexOf(this);
				switch (this.m_serializerNodeType)
				{
					case Wrappers.WrappedSerializerNodeType.DirectoryRecord:
					case Wrappers.WrappedSerializerNodeType.Thread:
					{
						subResultsLink.SubResultsIndex = (uint)index;
						subResultsLink.NrOfErrors = this.m_endCounters.NrOfErrors;
						subResultsLink.NrOfWarnings = this.m_endCounters.NrOfWarnings;
						//
						// Write sub results link to parent document.
						//
						if (this.m_serializerNodeType == Wrappers.WrappedSerializerNodeType.Thread)
						{
							subResultsLink.DvtDetailToXml(
								this.m_parentSerializer.m_serializationWriter.DetailStreamWriter, 0);
							subResultsLink.DvtSummaryToXml(
								this.m_parentSerializer.m_serializationWriter.SummaryStreamWriter, 0);
						}
					}
						break;
					case Wrappers.WrappedSerializerNodeType.TopParent:
					default:
						throw new System.ApplicationException(
							System.String.Format(
							"child serializer can not be of type:{0}", 
							this.m_serializerNodeType.ToString()));
                }
            }
            //
            // Add counters at the end of the output file of each parent.
            //
            this.SerializeCounts();
            this.m_serializationWriter.WriteEndElement();
            this.m_serializationWriter.WriteEndDocument();
            m_serializationWriter = null;
        }

        /// <summary>
        /// Apply counts at the end of the serialization process.
        /// These counts are used by EndSerializationTarget to serialize a counts tag.
        /// </summary>
        /// <param name="endNrOfGeneralErrors"></param>
        /// <param name="endNrOfGeneralWarnings"></param>
        /// <param name="endNrOfUserErrors"></param>
        /// <param name="endNrOfUserWarnings"></param>
        /// <param name="endNrOfValidationErrors"></param>
        /// <param name="endNrOfValidationWarnings"></param>
        public void AddEndCounts(
            System.UInt32 endNrOfGeneralErrors,
            System.UInt32 endNrOfGeneralWarnings,
            System.UInt32 endNrOfUserErrors,
            System.UInt32 endNrOfUserWarnings,
            System.UInt32 endNrOfValidationErrors,
            System.UInt32 endNrOfValidationWarnings)
        {
            this.m_endCounters.NrOfGeneralErrors += endNrOfGeneralErrors;
            this.m_endCounters.NrOfGeneralWarnings += endNrOfGeneralWarnings;
            this.m_endCounters.NrOfUserErrors += endNrOfUserErrors;
            this.m_endCounters.NrOfUserWarnings += endNrOfUserWarnings;
            this.m_endCounters.NrOfValidationErrors += endNrOfValidationErrors;
            this.m_endCounters.NrOfValidationWarnings += endNrOfValidationWarnings;
            m_bEndCountsApplied = true;
        }

        private SerializationStatus m_SerializationStatus = SerializationStatus.Stopped;

        public void StartTopSerializer(string fileName)
        {
            if (!this.m_bIsTopSerializer)
            {
                throw new System.ApplicationException(
                    "Trying to start a non top serializer.");
            }
            if (this.m_SerializationStatus != SerializationStatus.Stopped)
            {
                throw new System.ApplicationException(
                    "Trying to start a top serializer that is not in stopped status.");
            }
            this.m_bEndCountsApplied = false;
            if (fileName == null) throw new System.ArgumentNullException();
            this._ApplyTargetFileName(fileName);
            this.Init(
                0,
                Wrappers.WrappedSerializerNodeType.TopParent);
            //
            // Create root-tag for child document
            //
            DvtkData.Results.Results results = new DvtkData.Results.Results();
            //
            // Create 1st level tag (results header)
            //
            results.Details = new DvtkData.Results.Details();
            results.Details.ApplicationEntityName =
                this.m_parentSession.DefinitionManagement.ApplicationEntityName;
            results.Details.ApplicationEntityVersion =
                this.m_parentSession.DefinitionManagement.ApplicationEntityVersion;
            results.Details.Manufacturer = this.m_parentSession.Manufacturer;
            results.Details.ModelName = this.m_parentSession.ModelName;
            results.Details.SessionId = this.m_parentSession.SessionId;
            results.Details.SessionTitle = this.m_parentSession.SessionTitle;
            if (this.m_parentSession is Dvtk.Sessions.EmulatorSession)
            {
                results.Details.ScpEmulatorType = 
                    (this.m_parentSession as Dvtk.Sessions.EmulatorSession).ScpEmulatorType;
                results.Details.ScuEmulatorType = 
                    (this.m_parentSession as Dvtk.Sessions.EmulatorSession).ScuEmulatorType;
            }
            results.Details.SoftwareVersions = this.m_parentSession.SoftwareVersions;
            if (this.m_parentSession is IConfigurableSut)
            {
                results.Details.SutRole =
                    (this.m_parentSession as IConfigurableSut).SutSystemSettings.CommunicationRole.ToString();
            }
            results.Details.TestDate = System.DateTime.Now; //this.m_parentSession.Date;
            results.Details.Tester = this.m_parentSession.TestedBy;
            //
            // stream session details to detailed output
            //
            results.Details.DvtDetailToXml(m_serializationWriter.DetailStreamWriter, 0);
            //
            // stream session details to summary output
            //
            results.Details.DvtDetailToXml(m_serializationWriter.SummaryStreamWriter, 0);
            //
            // stream session details to test log output
            //
            results.Details.DvtDetailToXml(m_serializationWriter.TestLogStreamWriter, 0);
        }

        private void _ApplyTargetFileName(string fileName)
        {
            string inputPath = fileName;
            if (System.IO.Path.IsPathRooted(inputPath))
            {
                throw new System.ArgumentException(
                    "Please supply a relative path string instead of an absolute path string for "+
                    string.Format("TargetFileName {0}", inputPath));
            }
            string filePath = string.Concat(
                this.m_parentSession.ResultsRootDirectory, inputPath);
            if (!System.IO.Path.IsPathRooted(filePath))
                throw new System.ArgumentException();
            this.m_targetFileName = filePath;
        }
        private string m_targetFileName = null;

        #region Wrappers.ISerializationTarget
        public void SerializeSend(DvtkData.Dimse.DicomMessage dicomMessage)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.Send send = new DvtkData.Results.Send();
            send.DicomMessage = dicomMessage;
            send.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeSend(DvtkData.Dul.DulMessage dulMessage)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.Send send = new DvtkData.Results.Send();
            send.DulMessage = dulMessage;
            send.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeReceive(DvtkData.Dimse.DicomMessage dicomMessage)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.Receive receive = new DvtkData.Results.Receive();
            receive.DicomMessage = dicomMessage;
            receive.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeReceive(DvtkData.Dul.DulMessage dulMessage)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.Receive receive = new DvtkData.Results.Receive();
            receive.DulMessage = dulMessage;
            receive.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeValidate(DvtkData.Validation.ValidationObjectResult validationObjectResult)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            //
            // Determine whether to process a Directory Record Table of Contents
            // within this Validation Object Results.
            //
            if (validationObjectResult.DirectoryRecordTOC != null)
            {
                //
                // An Validation Object Results with a Directory Record Table of Contents
                // should only be serialized via the top parent-serializer.
                //
                if (!this.m_bIsTopSerializer)
                {
                    throw new System.ApplicationException(
                        string.Concat(
                        "An Validation Object Results with a Directory Record Table of Contents",
                        "should only be serialized via the top parent-serializer."));
                }
                //
                // Check that intermediate counting tables exist of the top parent-serializer.
                //
                if (
                    this.topDirectoryRecordErrorCountTable == null ||
                    this.topDirectoryRecordWarningCountTable == null)
                {
                    throw new System.ApplicationException(
                        string.Concat(
                        "Did not find the intermediate error and warning ",
                        "counting tables for directory records."));
                }
                //
                // Add the missing information about errors and warnings to the serialized data.
                // This error and warning counting was done at the top parent-serializer level.
                //
                foreach (
                    DvtkData.Validation.SubItems.ValidationDirectoryRecordLink link
                        in validationObjectResult.DirectoryRecordTOC)
                {
                    //
                    // copy temporary data for errors and warnings to link.
                    //
                    object o;
                    object key;
                    key = link.DirectoryRecordIndex;
                    o = this.topDirectoryRecordErrorCountTable[key];
                    link.NrOfErrors = (o == null) ? 0 : (System.UInt32)o;
                    this.topDirectoryRecordErrorCountTable.Remove(key);
                    o = this.topDirectoryRecordWarningCountTable[key];
                    link.NrOfWarnings = (o == null) ? 0 : (System.UInt32)o;
                    this.topDirectoryRecordWarningCountTable.Remove(key);
                }
            }

			//Assign Unique ID to the message
			validationObjectResult.MessageUID = GetNextDIMSEDULMessageIndex();

			// stream detailed validation results
            validationObjectResult.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);

            //Assign user selected choice from UI
            validationObjectResult.ConditionTextDisplay = this.m_parentSession.DisplayConditionText;

            // stream summary validation results
			validationObjectResult.DvtSummaryToXml(this.m_serializationWriter.SummaryStreamWriter, 0);

            // stream test log
            validationObjectResult.DvtTestLogToXml(this.m_serializationWriter.TestLogStreamWriter, "");
        }

        public void SerializeValidate(DvtkData.Validation.ValidationAbortRq validationAbortRq)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;

			//Assign Unique ID to the message
			validationAbortRq.MessageUID = GetNextDIMSEDULMessageIndex();

            // stream detailed validation results
            validationAbortRq.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);

            // stream summary validation results
            validationAbortRq.DvtSummaryToXml(this.m_serializationWriter.SummaryStreamWriter, 0);

            // stream test log
            validationAbortRq.DvtTestLogToXml(this.m_serializationWriter.TestLogStreamWriter, "Dicom Abort Request");
        }
        public void SerializeValidate(DvtkData.Validation.ValidationAssociateAc validationAssociateAc)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;

			//Assign Unique ID to the message
			validationAssociateAc.MessageUID = GetNextDIMSEDULMessageIndex();

            // stream detailed validation results
            validationAssociateAc.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);

            // stream summary validation results
            validationAssociateAc.DvtSummaryToXml(this.m_serializationWriter.SummaryStreamWriter, 0);

            // stream test log
            validationAssociateAc.DvtTestLogToXml(this.m_serializationWriter.TestLogStreamWriter, "Dicom Association Acknowledge");
        }
        public void SerializeValidate(DvtkData.Validation.ValidationAssociateRj validationAssociateRj)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;

			//Assign Unique ID to the message
			validationAssociateRj.MessageUID = GetNextDIMSEDULMessageIndex();

            // stream detailed validation results
            validationAssociateRj.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);

            // stream summary validation results
            validationAssociateRj.DvtSummaryToXml(this.m_serializationWriter.SummaryStreamWriter, 0);

            // stream test log
            validationAssociateRj.DvtTestLogToXml(this.m_serializationWriter.TestLogStreamWriter, "Dicom Association Reject");
        }
        public void SerializeValidate(DvtkData.Validation.ValidationAssociateRq validationAssociateRq)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;

			//Assign Unique ID to the message
			validationAssociateRq.MessageUID = GetNextDIMSEDULMessageIndex();

            // stream detailed validation results
            validationAssociateRq.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);

            // stream summary validation results
            validationAssociateRq.DvtSummaryToXml(this.m_serializationWriter.SummaryStreamWriter, 0);

            // stream test log
            validationAssociateRq.DvtTestLogToXml(this.m_serializationWriter.TestLogStreamWriter, "Dicom Association Request");
        }

        public void SerializeValidate(DvtkData.Validation.ValidationReleaseRp validationReleaseRp)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;

			// stream detailed validation results
            validationReleaseRp.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);

            // stream summary validation results
            validationReleaseRp.DvtSummaryToXml(this.m_serializationWriter.SummaryStreamWriter, 0);

            // stream test log
            validationReleaseRp.DvtTestLogToXml(this.m_serializationWriter.TestLogStreamWriter, "Dicom Release Response");
        }
        public void SerializeValidate(DvtkData.Validation.ValidationReleaseRq validationReleaseRq)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;

			// stream detailed validation results
            validationReleaseRq.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);

            // stream summary validation results
            validationReleaseRq.DvtSummaryToXml(this.m_serializationWriter.SummaryStreamWriter, 0);

            // stream test log
            validationReleaseRq.DvtTestLogToXml(this.m_serializationWriter.TestLogStreamWriter, "Dicom Release Request");
        }

        public void SerializeValidate(
            DvtkData.Validation.ValidationDirectoryRecordResult validationDirectoryRecordResult)
        {
            if (this.m_bIsTopSerializer)
            {
                //
                // The directory records should be serialized using child-serializers.
                // Not by means of the top parent-serializer.
                //
                throw new System.ApplicationException(
                    string.Concat(
                    "ValidationDirectoryRecordResult should not be serialized ", 
                    "by means of the top parent-serializer!"));
            }
            if (!this.m_parentSerializer.m_bIsTopSerializer)
            {
                //
                // The parent of this child serializer should be the top.
                //
                throw new System.ApplicationException(
                    "The parent-serializer of this child-serializer should be the top serializer.");
            }
            //
            // Store errors and warnings in the temporary count tables of the top parent-serializer.
            // To be used during serialization of record links.
            //
            this.m_parentSerializer.topDirectoryRecordErrorCountTable[validationDirectoryRecordResult.DirectoryRecordIndex]
                = validationDirectoryRecordResult.NrOfErrors;
            this.m_parentSerializer.topDirectoryRecordWarningCountTable[validationDirectoryRecordResult.DirectoryRecordIndex]
                = validationDirectoryRecordResult.NrOfWarnings;

            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;

            /* TODO: Remove obsolete, is now done by CreateAndRegisterChildSerializer.
            //
            // Split-of directory records into 
            // separate files post-fixed by DicomDirectoryIndex.
            //
            SerializationWriter serializationWriter = new SerializationWriter(
                this.m_targetFileName,
                isParentDocument,
                validationDirectoryRecordResult.DirectoryRecordIndex, 
                this.m_parentSession.DetailedValidationResults, 
                this.m_parentSession.SummaryValidationResults);
            serializationWriter.WriteStartDocument();
            serializationWriter.WriteStartElement();
            */

			// stream detailed validation results
            validationDirectoryRecordResult.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);

            validationDirectoryRecordResult.DisplayConditionText = this.m_parentSession.DisplayConditionText;
            if (validationDirectoryRecordResult.ReferencedFile != null)
            {
                validationDirectoryRecordResult.ReferencedFile.ConditionTextDisplay = this.m_parentSession.DisplayConditionText;
            }

            // stream summary validation results
            validationDirectoryRecordResult.DvtSummaryToXml(this.m_serializationWriter.SummaryStreamWriter, 0);

            /* TODO: Remove obsolete, is now done by CreateAndRegisterChildSerializer.
            // close the targets
            serializationWriter.WriteEndElement();
            serializationWriter.WriteEndDocument();
            */
        }
        private void _SerializeDisplay(DvtkData.Results.Display display)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            display.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeDisplay(DvtkData.Dimse.Attribute attribute)
        {
            DvtkData.Results.Display display = new DvtkData.Results.Display();
            display.Attribute = attribute;
            _SerializeDisplay(display);
        }
        public void SerializeDisplay(DvtkData.Media.DicomFile dicomFile)
        {
            DvtkData.Results.Display display = new DvtkData.Results.Display();
            display.DicomFile = dicomFile;
            _SerializeDisplay(display);
        }
        public void SerializeDisplay(DvtkData.Dimse.DicomMessage dicomMessage)
        {
            DvtkData.Results.Display display = new DvtkData.Results.Display();
            display.DicomMessage = dicomMessage;
            _SerializeDisplay(display);
        }
        public void SerializeDisplay(DvtkData.Dimse.DataSet dataSet)
        {
            DvtkData.Results.Display display = new DvtkData.Results.Display();
            display.DataSet = dataSet;
            _SerializeDisplay(display);
        }
		public void SerializeDisplay(DvtkData.Dimse.SequenceItem sequenceItem)
		{
			DvtkData.Results.Display display = new DvtkData.Results.Display();
			display.SequenceItem = sequenceItem;
			_SerializeDisplay(display);
		}
        public void SerializeImport(DvtkData.Dimse.DicomMessage dicomMessage)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.Import import = new DvtkData.Results.Import();
            import.DicomMessage = dicomMessage;
            import.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeMediaWrite(
            System.String fileName,
            DvtkData.Media.DicomFile dicomFile)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.MediaWrite mediaWrite = new DvtkData.Results.MediaWrite();
            mediaWrite.FileName = fileName;
            mediaWrite.DicomFile = dicomFile;
            mediaWrite.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeMediaRead(
            System.String fileName,
            DvtkData.Media.DicomFile dicomFile)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.MediaRead mediaRead = new DvtkData.Results.MediaRead();
            mediaRead.FileName = fileName;
            mediaRead.DicomFile = dicomFile;
            mediaRead.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeBytes(
            System.Byte[] bytes, System.String description)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.ByteDump byteDump = new DvtkData.Results.ByteDump();
            byteDump.Bytes = bytes;
            byteDump.Description = description;
            byteDump.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        private static DvtkData.Activities.ActivityReportLevel _Convert(
            Wrappers.WrappedValidationMessageLevel level)
        {
            switch (level)
            {
                case Wrappers.WrappedValidationMessageLevel.Debug: 
                    return DvtkData.Activities.ActivityReportLevel.Debug;
                case Wrappers.WrappedValidationMessageLevel.DicomObjectRelationship: 
                    return DvtkData.Activities.ActivityReportLevel.DicomObjectRelationship;
                case Wrappers.WrappedValidationMessageLevel.DulpStateMachine: 
                    return DvtkData.Activities.ActivityReportLevel.DulpMachineState;
                case Wrappers.WrappedValidationMessageLevel.Error: 
                    return DvtkData.Activities.ActivityReportLevel.Error;
                case Wrappers.WrappedValidationMessageLevel.Information: 
                    return DvtkData.Activities.ActivityReportLevel.Information;
				case Wrappers.WrappedValidationMessageLevel.ConditionText: 
					return DvtkData.Activities.ActivityReportLevel.ConditionText;
				case Wrappers.WrappedValidationMessageLevel.None: 
                    return DvtkData.Activities.ActivityReportLevel.None;
                case Wrappers.WrappedValidationMessageLevel.Scripting: 
                    return DvtkData.Activities.ActivityReportLevel.Scripting;
                case Wrappers.WrappedValidationMessageLevel.ScriptName: 
                    return DvtkData.Activities.ActivityReportLevel.ScriptName;
                case Wrappers.WrappedValidationMessageLevel.MediaFilename: 
                    return DvtkData.Activities.ActivityReportLevel.MediaFilename;
                case Wrappers.WrappedValidationMessageLevel.WareHouseLabel: 
                    return DvtkData.Activities.ActivityReportLevel.WareHouseLabel;
                case Wrappers.WrappedValidationMessageLevel.Warning: 
                    return DvtkData.Activities.ActivityReportLevel.Warning;
                default:
                    System.Diagnostics.Trace.Assert(false);
                    return DvtkData.Activities.ActivityReportLevel.Error;
            }
        }
        public void SerializeApplicationReport(
            Wrappers.WrappedValidationMessageLevel activityReportLevel,
            System.String message)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Activities.ApplicationActivityReport applicationActivityReport = 
                new DvtkData.Activities.ApplicationActivityReport();
            applicationActivityReport.Message = message;
            applicationActivityReport.Level = _Convert(activityReportLevel);

			UInt32 messageIndex = 0;
			switch(applicationActivityReport.Level)
			{
				case DvtkData.Activities.ActivityReportLevel.Error:
				case DvtkData.Activities.ActivityReportLevel.Warning:
					messageIndex = GetNextMessageIndex();
					break;
				default:
					break;
			}

            // stream application activity to detailed output
            applicationActivityReport.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, messageIndex, 0);

            // stream application activity to summary output
            applicationActivityReport.DvtSummaryToXml(this.m_serializationWriter.SummaryStreamWriter, messageIndex, 0);
        }
        public void SerializeDSCreate(
            System.String commandSetRefId, 
            DvtkData.Dimse.CommandSet commandSet)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.DicomScriptCreate dicomScriptCreate = 
                new DvtkData.Results.DicomScriptCreate();
            dicomScriptCreate.CommandSet = commandSet;
            dicomScriptCreate.CommandSetRefId = commandSetRefId;
            dicomScriptCreate.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeDSCreate(
            System.String commandSetRefId,
            DvtkData.Dimse.CommandSet commandSet,
            System.String dataSetRefId,
            DvtkData.Dimse.DataSet dataSet)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.DicomScriptCreate dicomScriptCreate = 
                new DvtkData.Results.DicomScriptCreate();
            dicomScriptCreate.CommandSet = commandSet;
            dicomScriptCreate.CommandSetRefId = commandSetRefId;
            dicomScriptCreate.DataSet = dataSet;
            dicomScriptCreate.DataSetRefId = dataSetRefId;
            dicomScriptCreate.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeDSSetCommandSet(
            System.String commandSetRefId,
            DvtkData.Dimse.CommandSet commandSet)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.DicomScriptSet dicomScriptSet = 
                new DvtkData.Results.DicomScriptSet();
            dicomScriptSet.CommandSet = commandSet;
            dicomScriptSet.CommandSetRefId = commandSetRefId;
            dicomScriptSet.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeDSSetDataSet(
            System.String dataSetRefId,
            DvtkData.Dimse.DataSet dataSet)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.DicomScriptSet dicomScriptSet = 
                new DvtkData.Results.DicomScriptSet();
            dicomScriptSet.DataSet = dataSet;
            dicomScriptSet.DataSetRefId = dataSetRefId;
            dicomScriptSet.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeDSDeleteCommandSet(
            System.String commandSetRefId,
            DvtkData.Dimse.CommandSet commandSet)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.DicomScriptDelete dicomScriptDelete = 
                new DvtkData.Results.DicomScriptDelete();
            dicomScriptDelete.CommandSet = commandSet;
            dicomScriptDelete.CommandSetRefId = commandSetRefId;
            dicomScriptDelete.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }
        public void SerializeDSDeleteDataSet(
            System.String dataSetRefId,
            DvtkData.Dimse.DataSet dataSet)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Results.DicomScriptDelete dicomScriptDelete = 
                new DvtkData.Results.DicomScriptDelete();
            dicomScriptDelete.DataSet = dataSet;
            dicomScriptDelete.DataSetRefId = dataSetRefId;
            dicomScriptDelete.DvtDetailToXml(this.m_serializationWriter.DetailStreamWriter, 0);
        }

        public bool Paused
        {
            get 
            { 
                return this.m_parentSession.ResultsGatheringPaused; 
            }
            set 
            { 
                this.m_parentSession.ResultsGatheringPaused = value; 
            }
        }

		public DvtkData.Results.Counters EndCounters
		{
			get
			{
				return this.m_endCounters;
			}
		}
        
        #endregion Wrappers.ISerializationTarget

        public void SerializeUserReport(
            Wrappers.WrappedValidationMessageLevel activityReportLevel,
            System.String message)
        {
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            DvtkData.Activities.UserActivityReport userActivityReport = 
                new DvtkData.Activities.UserActivityReport();
            userActivityReport.Message = DvtToXml.ConvertString(message,false);
            userActivityReport.Level = _Convert(activityReportLevel);

			UInt32 messageIndex = 0;
			switch(userActivityReport.Level)
			{
				case DvtkData.Activities.ActivityReportLevel.Error:
				case DvtkData.Activities.ActivityReportLevel.Warning:
					messageIndex = GetNextMessageIndex();
					break;
				default:
					break;
			}

            // stream user activity to detailed output
            userActivityReport.DvtDetailToXml(m_serializationWriter.DetailStreamWriter, messageIndex, 0);

            // stream user activity to summary output
            userActivityReport.DvtSummaryToXml(m_serializationWriter.SummaryStreamWriter, messageIndex, 0);

            // stream user activity to test log output
            userActivityReport.DvtTestLogToXml(m_serializationWriter.TestLogStreamWriter, messageIndex, 0);
        }

		public void SerializeValidationReport(
			Wrappers.WrappedValidationMessageLevel activityReportLevel,
			System.String message)
		{
			if (this.m_parentSession.ResultsGatheringPaused) return;
			if (this.m_serializationWriter == null) return;
			DvtkData.Validation.ValidationMessage validationMessage = new DvtkData.Validation.ValidationMessage();
			switch(activityReportLevel)
			{
				case Wrappers.WrappedValidationMessageLevel.Error:
					validationMessage.Type = DvtkData.Validation.MessageType.Error;
					break;
				case Wrappers.WrappedValidationMessageLevel.Warning:
					validationMessage.Type = DvtkData.Validation.MessageType.Warning;
					break;
				case Wrappers.WrappedValidationMessageLevel.Information:
					validationMessage.Type = DvtkData.Validation.MessageType.Info;
					break;
				case Wrappers.WrappedValidationMessageLevel.ConditionText:
					validationMessage.Type = DvtkData.Validation.MessageType.ConditionText;
					break;
				default:
					validationMessage.Type = DvtkData.Validation.MessageType.Info;
					break;
			}
			validationMessage.Message = DvtToXml.ConvertString(message,false);

			// stream validation message to detailed output
			validationMessage.DvtDetailToXml(m_serializationWriter.DetailStreamWriter, 0);

			// stream validation message to summary output
			validationMessage.DvtDetailToXml(m_serializationWriter.SummaryStreamWriter, 0);
		}

		public void SerializeMessageComparisonResults(MessageComparisonResults messageComparisonResults)
		{
			// stream message comparison results to detailed output
			messageComparisonResults.DvtDetailToXml(m_serializationWriter.DetailStreamWriter, 0);

			// stream message comparison results to summary output
			messageComparisonResults.DvtSummaryToXml(m_serializationWriter.SummaryStreamWriter, 0);
		}

		public void SerializeHtmlUserReport(
			Wrappers.WrappedValidationMessageLevel activityReportLevel,
			System.String message,
            bool writeToSummary, 
            bool writeToDetail,
            bool writeToTestlog)
		{
			if (this.m_parentSession.ResultsGatheringPaused) return;
			if (this.m_serializationWriter == null) return;

			DvtkData.Activities.UserActivityReport userActivityReport = 
				new DvtkData.Activities.UserActivityReport();
			userActivityReport.IsHtml = true;


			// All '[' and ']' will be filtered out.
			// After this, all '<' will be replaced by '[' and all '>' will be replaced by ']'.
			message = message.Replace("[", "");
			message = message.Replace("]", "");
			message = message.Replace("<", "[");
			message = message.Replace(">", "]");

			userActivityReport.Message = DvtToXml.ConvertString(message,false);
			userActivityReport.Level = _Convert(activityReportLevel);

            if (writeToDetail)
            {
                // stream user activity to detailed output
                userActivityReport.DvtDetailToXml(m_serializationWriter.DetailStreamWriter, 0, 0);
            }

            if (writeToSummary)
            {
                // stream user activity to summary output
                userActivityReport.DvtSummaryToXml(m_serializationWriter.SummaryStreamWriter, 0, 0);
            }

            if (writeToTestlog)
            {
                // stream user activity to summary output
                userActivityReport.DvtTestLogToXml(m_serializationWriter.TestLogStreamWriter, 0, 0);
            }
		}

        private void SerializeCounts()
        {
            if (!this.m_bEndCountsApplied) throw new System.ApplicationException("No end counts have been applied yet!");
            if (this.m_parentSession.ResultsGatheringPaused) return;
            if (this.m_serializationWriter == null) return;
            //
            // Serialize counters to detailed output
            //
            this.m_endCounters.DvtDetailToXml(m_serializationWriter.DetailStreamWriter, 0);
            //
            // Serialize counters to summary output
            //
            this.m_endCounters.DvtDetailToXml(m_serializationWriter.SummaryStreamWriter, 0);
        }

        private void SerializeSend()
        { }
    }
}
