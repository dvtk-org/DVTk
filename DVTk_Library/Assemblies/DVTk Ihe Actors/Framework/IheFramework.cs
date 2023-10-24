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
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

using DvtkHighLevelInterface;
using DvtkHighLevelInterface.Common.Other;
using Dvtk.Results;
using Dvtk.Comparator;
using DvtkData.Dimse;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Actors;
using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Hl7;
using Dvtk.Dicom.InformationEntity.DefaultValues;

namespace Dvtk.IheActors.IheFramework
{
	/// <summary>
	/// Summary description for IheFramework.
	/// </summary>
	public class IheFramework : IIheFramework
	{
		private IheFrameworkConfig _iheFrameworkConfig = null;
		private ActorCollection _actors = null;
		private ActorsTransactionLog _actorsTransactionLog = null;
		private ResultsReporter _resultsReporter = null;
		private System.String _resultsDirectory = System.String.Empty;
		private System.String _resultsFilename = System.String.Empty;
		private DefaultValueManager _defaultValueManager = new DefaultValueManager();
		private uint _nrErrors = 0;
		private uint _nrWarnings = 0;
        private Dvtk.IheActors.UserInterfaces.Form _uiForm = null;
		private ArrayList _attachedUserInterfaces = new ArrayList();

		/// <summary>
		/// Class Constructor.
		/// </summary>
		/// <param name="profileName">Integration Profile Name.</param>
		public IheFramework(System.String profileName)
		{
			_iheFrameworkConfig = new IheFrameworkConfig(profileName);
			_actors = new ActorCollection();
			_actorsTransactionLog = new ActorsTransactionLog();
		}

		/// <summary>
		/// Property - Config.
		/// </summary>
		public IheFrameworkConfig Config
		{
			get
			{
				return _iheFrameworkConfig;
			}
		}

		/// <summary>
		/// Property - TransactionLog
		/// </summary>
		public ActorsTransactionLog TransactionLog
		{
			get
			{
				return _actorsTransactionLog;
			}
		}

		/// <summary>
		/// Property - Number of Errors.
		/// </summary>
		public uint NrErrors
		{
			get
			{
				uint nrErrors = _nrErrors;
				if (_resultsReporter != null)
				{
					nrErrors += _resultsReporter.NrErrors;
				}
				return nrErrors;
			}
		}

		/// <summary>
		/// Property - Number of Warnings.
		/// </summary>
		public uint NrWarnings
		{
			get
			{
				uint nrWarnings = _nrWarnings;
				if (_resultsReporter != null)
				{
					nrWarnings += _resultsReporter.NrWarnings;
				}
				return nrWarnings;
			}
		}

		private void AttachActorToActorUserInterfaces(BaseActor actor)
		{
			foreach (Dvtk.IheActors.UserInterfaces.IIheFrameworkUserInterface iheFrameworkUserInterface in this._attachedUserInterfaces)
			{
				if (iheFrameworkUserInterface is Dvtk.IheActors.UserInterfaces.IActorUserInterface)
				{
					Dvtk.IheActors.UserInterfaces.IActorUserInterface actorUserInterface = iheFrameworkUserInterface as Dvtk.IheActors.UserInterfaces.IActorUserInterface;

					actorUserInterface.Attach(actor);
				}
			}
		}

		/// <summary>
		/// Get the Actor with the given actor name.
		/// </summary>
		/// <param name="actorName">Actor Name to seach for.</param>
		/// <returns>Actor with given name - maybe null.</returns>
		public BaseActor GetActor(ActorName actorName)
		{
			BaseActor lActor = null;

			// search all the actors for one with the given name
			foreach (BaseActor actor in _actors)
			{
				if (actor.ActorName.TypeId == actorName.TypeId)
				{
					lActor = actor;
					break;
				}
			}

			return lActor;
		}

		/// <summary>
		/// Add a Tag Value filter for the comparator.
		/// Only compare messages which contain the same values for this filter.
		/// </summary>
		/// <param name="tagValueFilter">Tag Value Filter.</param>
		public void AddComparisonTagValueFilter(DicomTagValue tagValueFilter)
		{
			_actorsTransactionLog.AddComparisonTagValueFilter(tagValueFilter);
		}

		/// <summary>
		/// Add user defined default Tag Values. Used to help define the message tag/values 
		/// used during the tests.
		/// </summary>
		/// <param name="defaultTagValue">Default Tag Value pair.</param>
		public void AddUserDefinedDefaultTagValue(BaseDicomTagValue defaultTagValue)
		{
			_defaultValueManager.AddUserDefinedDefaultTagValue(defaultTagValue);
		}

		/// <summary>
		/// Update the instantiated default tag values with the given tag value.
		/// </summary>
		/// <param name="tagValueUpdate">Tag/Value pair to update.</param>
        public void UpdateInstantiatedDefaultTagValue(DicomTagValue tagValueUpdate)
		{
			_defaultValueManager.UpdateInstantiatedDefaultTagValues(tagValueUpdate);
		}

        /// <summary>
        /// Update the default DICOM Tag Values grouped by the given affected entity.
        /// Any 'auto' default value in the affected entity will get it's next value.
        /// </summary>
        /// <param name="affectedEntity">Affected Entity enum - to update.</param>
        public void UpdateInstantiatedDefaultTagValues(AffectedEntityEnum affectedEntity)
        {
            _defaultValueManager.UpdateInstantiatedDefaultTagValues(affectedEntity);
        }

        internal void Attach(Dvtk.IheActors.UserInterfaces.IIheFrameworkUserInterface iheFrameworkUserInterface)
        {
            this._attachedUserInterfaces.Add(iheFrameworkUserInterface);
        }

		/// <summary>
		/// Open the Results reporting.
		/// </summary>
		public void OpenResults()
		{
			// results directory is only defined after loading the configuration
			_resultsReporter = new ResultsReporter(_resultsDirectory);
			_resultsFilename = Config.ProfileName.Replace(" ","") + "ResultsIndex.xml";
			_resultsReporter.Start(_resultsFilename);
		}

		/// <summary>
		/// Close the Results reporting.
		/// </summary>
		/// <returns>System.String - results filename.</returns>
		public System.String CloseResults()
		{
			_resultsReporter.Stop();

			// convert the results file
			System.String resultsFilename = Xslt.Transform(_resultsDirectory, _resultsFilename);

            // create the result overview 
            CreateResultOverview(resultsFilename);

            return resultsFilename;
		}

		/// <summary>
		/// Apply the ActorConfigs by instantiating the corresponding actors.
		/// </summary>
		public void ApplyConfig()
		{
            // set the results subdirectory name - fixed location is "resultsfiles"
            Config.CommonConfig.ResultsSubdirectory = "resultsfiles";
            if (Config.CommonConfig.OverwriteResults == false)
            {
                // create a date/time stamp as the results subdirectory name so that the results are overwritten each time the framework is re-started
                Config.CommonConfig.ResultsSubdirectory = System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
			_resultsDirectory = RootedBaseDirectory.GetFullPathname(Config.CommonConfig.RootedBaseDirectory, Config.CommonConfig.ResultsDirectory + "\\" + Config.CommonConfig.ResultsSubdirectory);

			// create the results directory
			System.IO.DirectoryInfo directoryInfo = Directory.CreateDirectory(_resultsDirectory);

			// iterate over all the actors in the configuration
			foreach(ActorConfig actorConfig in Config.ActorConfig)
			{
				// only instantiate those that need to be emulated
				if (actorConfig.ConfigState == ActorConfigStateEnum.ActorBeingEmulated)
				{
					BaseActor actor = CreateActor(actorConfig.ActorName);
					actor.ConfigActor(Config.CommonConfig, Config.PeerToPeerConfig);
					_actors.Add(actor);
				}
			}


			// By inspecting which actors are emulated, build a CompareCondition in such a way that
			// for two "inversed" transactions (transaction that have the same actor but a different Direction)
			// only one of the transactions will be compared to other transactions.

			this._actorsTransactionLog.CompareCondition = new ConditionFalse();

			for (int index1 = 0; index1 < Config.ActorConfig.Count; index1++)
			{
				for(int index2 = index1 + 1; index2 < Config.ActorConfig.Count; index2++)
				{
					if ((Config.ActorConfig[index1].ConfigState == ActorConfigStateEnum.ActorBeingEmulated) && (Config.ActorConfig[index2].ConfigState == ActorConfigStateEnum.ActorBeingEmulated))
					{
						ActorName actorName1 = Config.ActorConfig[index1].ActorName;
						ActorName actorName2 = Config.ActorConfig[index2].ActorName;

						Condition actorPairCondition1 = Condition.And(
							new ActorsTransactionConditionFromActor(actorName1),
							new ActorsTransactionConditionToActor(actorName2));

						Condition actorPairCondition2 = Condition.And(
							new ActorsTransactionConditionFromActor(actorName2),
							new ActorsTransactionConditionToActor(actorName1));

						Condition combinedactorPairCondition = Condition.Or(actorPairCondition1, actorPairCondition2);

						Condition completeCondition = Condition.And(combinedactorPairCondition, new ActorsTransactionConditionDirection(TransactionDirectionEnum.TransactionSent));

						this._actorsTransactionLog.CompareCondition = Condition.Or(this._actorsTransactionLog.CompareCondition, completeCondition);			
					}
				}
			}

			this._actorsTransactionLog.CompareCondition = Condition.Not(this._actorsTransactionLog.CompareCondition);
		}

		private BaseActor CreateActor(ActorName actorName)
		{
			BaseActor actor = null;
			switch(actorName.Type)
			{
				case ActorTypeEnum.AdtPatientRegistration:
					actor = new AdtPatientRegistrationActor(actorName.Id, this);
					break;
				case ActorTypeEnum.OrderPlacer:
					actor = new OrderPlacerActor(actorName.Id, this);
					break;
				case ActorTypeEnum.DssOrderFiller:
					actor = new DssOrderFillerActor(actorName.Id, this);
					break;
				case ActorTypeEnum.AcquisitionModality:
					actor = new AcquisitionModalityActor(actorName.Id, this);
					break;
				case ActorTypeEnum.ImageManager:
					actor = new ImageManagerActor(actorName.Id, this);
					break;
				case ActorTypeEnum.ImageArchive:
					actor = new ImageArchiveActor(actorName.Id, this);
					break;
				case ActorTypeEnum.PerformedProcedureStepManager:
					actor = new PpsManagerActor(actorName.Id, this);
					break;
				case ActorTypeEnum.EvidenceCreator:
					actor = new EvidenceCreatorActor(actorName.Id, this);
					break;
				case ActorTypeEnum.ImageDisplay:
					actor = new ImageDisplayActor(actorName.Id, this);
					break;
//				case ActorTypeEnum.ReportManager:
//					actor = new ReportManagerActor(actorName.Id, this);
//					break;
				case ActorTypeEnum.PrintComposer:
					actor = new PrintComposerActor(actorName.Id, this);
					break;
				case ActorTypeEnum.PrintServer:
					actor = new PrintServerActor(actorName.Id, this);
					break;
				case ActorTypeEnum.Unknown:
				default:
					break;
			}

			return actor;
		}

		/// <summary>
		/// Start the integration profile test by starting up all the
		/// configured actors.
		/// </summary>
		public void StartTest()
		{
			_nrErrors = 0;
			_nrWarnings = 0;

			// if working interactively - set up the form
			if (Config.CommonConfig.Interactive == true)
			{
				// create a form for this integration profile
                _uiForm = new Dvtk.IheActors.UserInterfaces.Form();
                _uiForm.Attach(this);
			}

			// set up the default tag value list
			_defaultValueManager.CreateDefaultTagValues();

			// start all the actors
			foreach (BaseActor actor in _actors)
			{
				// If this Integration Profile is attached to UserInterfaces that implement the
				// IActorUserInterface, attach the actor to them.
				AttachActorToActorUserInterfaces(actor);

				actor.StartActor(_defaultValueManager);
			}
		}

		/// <summary>
		/// Stop the integration profile test by stopping all the configured
		/// actors.
		/// </summary>
		public void StopTest()
		{
			// stop all actors
			foreach (BaseActor actor in _actors)
			{
				actor.StopActor();
			}
		}

        /// <summary>
        /// Signal that the test has completed.
        /// </summary>
        public void SignalTestCompletion()
        {
            // signal file semaphore when not in interactive mode
            if (_iheFrameworkConfig.CommonConfig.Interactive == false)
            {
                FileSemaphore.SignalTestCompletion();
            }
        }

        /// <summary>
        /// Wait until told that the test has completed.
        /// </summary>
        public void PendTestCompletion()
        {
            // check if in interactive mode
            if (_iheFrameworkConfig.CommonConfig.Interactive == true)
            {
                // display message box that the user should close on test completion
                System.Windows.Forms.MessageBox.Show("Press OK when the test is complete.");
            }
            else
            {
                // pend on the file semaphore until test completion
                FileSemaphore.PendTestCompletion();
            }
        }
 
        /// <summary>
        /// Set the UI Status text to the given string.
        /// </summary>
        /// <param name="text">Text to write to UI status field.</param>
        public void SetUiStatusText(String text)
        {
            if (_uiForm != null)
            {
                String statusMessage = String.Empty;
                if (text != String.Empty)
                {
                    statusMessage = String.Format("Status: {0}", text);
                }

                _uiForm.Text = statusMessage;
            }
        }

        #region Test Assertions
        /// <summary>
        /// Assert that the correct number of the given DICOM message has been seen between the two given actors.
        /// </summary>
        /// <param name="actorName1">First actor name in transaction.</param>
        /// <param name="actorName2">Second actor name in transaction.</param>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <param name="expectedCount">Number of times the given DICOM command was expected to occur between these actors.</param>
        /// <returns>bool - indication if the assertion was true or false.</returns>
        public bool AssertMessageCountBetweenActors(ActorName actorName1, ActorName actorName2, String dimseCommandName, int expectedCount)
        {
            return _actorsTransactionLog.AssertMessageCountBetweenActors(actorName1, actorName2, dimseCommandName, expectedCount);
        }

        /// <summary>
        /// Assert that the given attribute value in the given DICOM message has the expected value between the two given actors.
        /// </summary>
        /// <param name="actorName1">First actor name in transaction.</param>
        /// <param name="actorName2">Second actor name in transaction.</param>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <param name="tag">tag - Attribute tag whose first value is the actual value.</param>
        /// <param name="expectedValue">The expected attribute value - this will be compared with the actual attribute value.</param>
        /// <returns>bool - indication if the assertion was true or false.</returns>
        public bool AssertDicomAttributeValueBetweenActors(ActorName actorName1, ActorName actorName2, String dimseCommandName, DvtkData.Dimse.Tag tag, int expectedValue)
        {
            return _actorsTransactionLog.AssertDicomAttributeValueBetweenActors(actorName1, actorName2, dimseCommandName, tag, expectedValue);
        }

        /// <summary>
        /// Assert that the given attribute value in the given DICOM message has the expected value between the two given actors.
        /// </summary>
        /// <param name="actorName1">First actor name in transaction.</param>
        /// <param name="actorName2">Second actor name in transaction.</param>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <param name="tag">tag - Attribute tag whose first value is the actual value.</param>
        /// <param name="expectedValue">The expected attribute value - this will be compared with the actual attribute value.</param>
        /// <returns>bool - indication if the assertion was true or false.</returns>
        public bool AssertDicomAttributeValueBetweenActors(ActorName actorName1, ActorName actorName2, String dimseCommandName, DvtkData.Dimse.Tag tag, String expectedValue)
        {
            return _actorsTransactionLog.AssertDicomAttributeValueBetweenActors(actorName1, actorName2, dimseCommandName, tag, expectedValue);
        }

        /// <summary>
        /// Assert that the given attribute is present in the given DICOM message between the two given actors.
        /// Check all occurences of the given dimseCommandName in the transaction log.
        /// </summary>
        /// <param name="actorName1">First actor name in transaction.</param>
        /// <param name="actorName2">Second actor name in transaction.</param>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <param name="tag">tag - Attribute tag to check for.</param>
        /// <returns>bool - indication if the assertion was true or false.</returns>
        public bool AssertDicomAttributePresentBetweenActors(ActorName actorName1, ActorName actorName2, String dimseCommandName, DvtkData.Dimse.Tag tag)
        {
            return _actorsTransactionLog.AssertDicomAttributePresentBetweenActors(actorName1, actorName2, dimseCommandName, tag);
        }
        #endregion Test Assertions

        /// <summary>
		/// Evaluate the integration profile test.
		/// </summary>
		public void EvaluateTest()
		{
			ReportTransactions();

			_actorsTransactionLog.Evaluate(_resultsReporter);
		}

		private void ReportTransactions()
		{
			System.String message = System.String.Format("<h1>DVTk IHE Framework - {0}</h1>", Config.ProfileName);
			_resultsReporter.WriteHtmlInformation(message);
			_resultsReporter.WriteHtmlInformation("<h2>HL7 and DICOM Actor Transactions</h2>");
			for (int i = 0; i < _actorsTransactionLog.Count; i++)
			{
				foreach (ActorsTransaction actorsTransaction in _actorsTransactionLog)
				{
					if (actorsTransaction.TransactionNumber == i + 1)
					{
						_nrErrors += actorsTransaction.NrErrors;
						_nrWarnings += actorsTransaction.NrWarnings;

						System.String testResult = System.String.Empty;
						System.String fontColor = System.String.Empty;
						if (actorsTransaction.NrErrors > 0)
						{
							fontColor = "red";
							testResult = "FAILED";
						}
						else
						{
							fontColor = "green";
							testResult = "PASSED";
						}

						message = System.String.Format("{0:000} <font color={1}>{2}</font>",
									actorsTransaction.TransactionNumber,
									fontColor,
									testResult);
						_resultsReporter.WriteHtmlInformation(message);

						System.String resultsFilename = Xslt.Transform(_resultsDirectory, actorsTransaction.ResultsFilename);
						switch(actorsTransaction.Direction)
						{
							case TransactionDirectionEnum.TransactionReceived:
								message = System.String.Format(" - <a href=\"{0}\">Received by {1} from {2}</a></br>",
									resultsFilename,
									actorsTransaction.ToActorName.TypeId,
									actorsTransaction.FromActorName.TypeId);
								_resultsReporter.WriteHtmlInformation(message);
								break;
							case TransactionDirectionEnum.TransactionSent:
								message = System.String.Format(" - <a href=\"{0}\">Sent from {1} to {2}</a></br>",
									resultsFilename,
									actorsTransaction.ToActorName.TypeId,
									actorsTransaction.FromActorName.TypeId);
								_resultsReporter.WriteHtmlInformation(message);
								break;
							default:
								break;
						}
						break;
					}
				}
			}
			_resultsReporter.WriteHtmlInformation("<br/><br/>");
		}

        private void CreateResultOverview(String resultsFilename)
        {
            try
            {
                // fixed result overview filename
                String xmlResultOverviewFilename = String.Format("{0}ResultOverview.xml", System.AppDomain.CurrentDomain.BaseDirectory);
                StreamWriter streamWriter = new StreamWriter(xmlResultOverviewFilename);
                streamWriter.WriteLine("<?xml version=\"1.0\" ?>");
                streamWriter.WriteLine("<TestResultOverview>");
                streamWriter.WriteLine("<NrOfErrors>{0}</NrOfErrors>", this.NrErrors.ToString());
                streamWriter.WriteLine("<NrOfWarnings>{0}</NrOfWarnings>", this.NrWarnings.ToString());
                streamWriter.WriteLine("<ResultsFilename>{0}</ResultsFilename>", resultsFilename);
                if (this.NrErrors == 0)
                {
                    streamWriter.WriteLine("<TestResult>PASSED</TestResult>");
                }
                else
                {
                    streamWriter.WriteLine("<TestResult>FAILED</TestResult>");
                }
                streamWriter.WriteLine("</TestResultOverview>");
                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (System.Exception)
            {
            }
        }

		/// <summary>
		/// Clean up the current working directory at the end of a test.
		/// PIX files are left in the current working directory after a test. These should be cleaned
		/// up by the Media File Class destructor in the C++ code but it seems that this is not always
		/// called. So a more robust approach is to delete the files explicitly.
		/// </summary>
		public void CleanUpCurrentWorkingDirectory()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
			foreach(FileInfo fileInfo in directoryInfo.GetFiles())
			{
				if (fileInfo.Extension.ToLower().Equals(".pix"))
				{
					fileInfo.Delete();
				}
			}

            // also clean up the rooted base directory
            directoryInfo = new DirectoryInfo(Config.CommonConfig.RootedBaseDirectory);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension.ToLower().Equals(".pix"))
                {
                    fileInfo.Delete();
                }
            }

		}
	}
}
