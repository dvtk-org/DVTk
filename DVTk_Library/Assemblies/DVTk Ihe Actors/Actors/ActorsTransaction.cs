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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

using Dvtk.Comparator;
using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Comparator;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Hl7;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for ActorsTransaction.
	/// </summary>
	public class ActorsTransaction
	{
		private int _transactionNumber;
		private ActorName _fromActorName;
		private ActorName _toActorName;
		private BaseTransaction _transaction = null;
		private System.String _resultsFilename;
        private System.String _resultsPathname;
		private uint _nrErrors = 0;
		private uint _nrWarnings = 0;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="transactionNumber">Transaction Number.</param>
		/// <param name="fromActorName">Name of Actor sending Transaction.</param>
		/// <param name="toActorName">Name of Actor receiving Transaction.</param>
		/// <param name="transaction">Transaction itself.</param>
		/// <param name="resultsFilename">Results filename.</param>
        /// <param name="resultsPathname">Full Results filename - including directory.</param>
		/// <param name="nrErrors">Number of Errors in Transaction.</param>
		/// <param name="nrWarnings">Number of Warnings in Transaction.</param>
		public ActorsTransaction(int transactionNumber,
								ActorName fromActorName, 
								ActorName toActorName, 
								BaseTransaction transaction,
								System.String resultsFilename,
                                System.String resultsPathname,
                                uint nrErrors,
								uint nrWarnings)
		{
			_transactionNumber = transactionNumber;
			_fromActorName = fromActorName;
			_toActorName = toActorName;
			_transaction = transaction;
			_resultsFilename = resultsFilename;
            _resultsPathname = resultsPathname;
            _nrErrors = nrErrors;
			_nrWarnings = nrWarnings;
		}

		/// <summary>
		/// Property - TransactionNumber
		/// </summary>
		public int TransactionNumber
		{
			get
			{
				return _transactionNumber;
			}
		}

		/// <summary>
		/// Property - FromActorName
		/// </summary>
		public ActorName FromActorName
		{
			get
			{
				return _fromActorName;
			}
		}

		/// <summary>
		/// Property - ToActorName
		/// </summary>
		public ActorName ToActorName
		{
			get
			{
				return _toActorName;
			}
		}

		/// <summary>
		/// Property - Transaction
		/// </summary>
		public BaseTransaction Transaction
		{
			get
			{
				return _transaction;
			}
		}

		/// <summary>
		/// Property - Direction
		/// </summary>
		public TransactionDirectionEnum Direction
		{
			get
			{
				return _transaction.Direction;
			}
		}

		/// <summary>
		/// Property - ResultsFilename
		/// </summary>
		public System.String ResultsFilename
		{
			get
			{
				return _resultsFilename;
			}
		}

        /// <summary>
        /// Property - ResultsPathname
        /// </summary>
        public System.String ResultsPathname
        {
            get
            {
                return _resultsPathname;
            }
        }

		/// <summary>
		/// Property - NrErrors
		/// </summary>
		public uint NrErrors
		{
			get
			{
				return _nrErrors;
			}
		}

		/// <summary>
		/// Property - NrWarnings
		/// </summary>
		public uint NrWarnings
		{
			get
			{
				return _nrWarnings;
			}
		}

		/// <summary>
		/// Log the transaction to the given stream.
		/// </summary>
        /// <param name="sw">Stream Writer - log the transaction to this stream.</param>
		public void LogToStream(StreamWriter sw)
		{
            sw.WriteLine("<<- {0} -------------------------------------------------------------->>", _transactionNumber);
			if (_transaction is DicomTransaction)
			{
				DicomTransaction dicomTransaction = (DicomTransaction)_transaction;
				switch(dicomTransaction.Direction)
				{
					case TransactionDirectionEnum.TransactionSent:
                        sw.WriteLine("DICOM Transaction received by {0}:{1}", ActorTypes.Type(_fromActorName.Type), _fromActorName.Id);
                        sw.WriteLine("from {0}:{1}", ActorTypes.Type(_toActorName.Type), _toActorName.Id);
						break;
                    case TransactionDirectionEnum.TransactionReceived:
                        sw.WriteLine("DICOM Transaction sent from {0}:{1}", ActorTypes.Type(_fromActorName.Type), _fromActorName.Id);
                        sw.WriteLine("to {0}:{1}", ActorTypes.Type(_toActorName.Type), _toActorName.Id);
						break;
					default:
						break;
				}
                sw.WriteLine("{0} errors, {1} warnings", _nrErrors, _nrWarnings);
				for (int i = 0; i < dicomTransaction.DicomMessages.Count; i++)
				{
                    sw.WriteLine("DICOM Message {0}...", i + 1);
					DicomMessage dicomMessage = (DicomMessage)dicomTransaction.DicomMessages[i];
					if (dicomMessage.CommandSet != null)
					{
                        sw.WriteLine("Command: {0} \"{1}\"", dicomMessage.CommandSet.DimseCommandName, dicomMessage.CommandSet.GetSopClassUid());
					}
					if (dicomMessage.DataSet != null)
					{
                        sw.WriteLine("Dataset Attributes: {0}", dicomMessage.DataSet.Count);
					}
				}
			}
			else
			{
				Hl7Transaction hl7Transaction = (Hl7Transaction)_transaction;
                switch (hl7Transaction.Direction)
                {
                    case TransactionDirectionEnum.TransactionSent:
                        sw.WriteLine("HL7 Transaction received by {0}:{1}", ActorTypes.Type(_fromActorName.Type), _fromActorName.Id);
                        sw.WriteLine("from {0}:{1}", ActorTypes.Type(_toActorName.Type), _toActorName.Id);
                        break;
                    case TransactionDirectionEnum.TransactionReceived:
                        sw.WriteLine("HL7 Transaction sent from {0}:{1}", ActorTypes.Type(_fromActorName.Type), _fromActorName.Id);
                        sw.WriteLine("to {0}:{1}", ActorTypes.Type(_toActorName.Type), _toActorName.Id);
                        break;
                    default:
                        break;
                }
                sw.WriteLine("{0} errors, {1} warnings", _nrErrors, _nrWarnings);
            }
            sw.WriteLine("Results Filename: \"{0}\"", _resultsFilename);
            sw.WriteLine("Results Pathname: \"{0}\"", _resultsPathname);

            sw.WriteLine("<<------------------------------------------------------------------>>");
        }

        #region assertion method helpers
        /// <summary>
        /// Get the number of DICOM messages in the transaction that match the DICOM command name.
        /// </summary>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <returns>int - number of times the given DICOM command name is found in the transaction.</returns>
        public int GetNumberOfDicomMessages(String dimseCommandName)
        {
            bool matchFound = false;
            int numberOfMessages = 0;
            if (_transaction is DicomTransaction)
            {
                DicomTransaction dicomTransaction = (DicomTransaction)_transaction;
                for (int i = 0; i < dicomTransaction.DicomMessages.Count; i++)
                {
                    DicomMessage dicomMessage = (DicomMessage)dicomTransaction.DicomMessages[i];
                    if (dicomMessage.CommandSet != null)
                    {
                        if (dicomMessage.CommandSet.DimseCommandName == dimseCommandName)
                        {
                            matchFound = true;
                            numberOfMessages++;
                        }
                    }
                }
            }

            if (matchFound == false)
            {
                numberOfMessages = -1;
            }
            return numberOfMessages;
        }

        /// <summary>
        /// Get the first value of the given attribute in the DICOM message with the given DICOM command name. 
        /// First search the command set and then the dataset (if present).
        /// </summary>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <param name="tag">Tag identifying attribute whose first value will be returned.</param>
        /// <param name="attributeValue">Out - returned attribute value.</param>
        /// <returns>bool - indicates whether the dimseCommandName was found in the transaction or not - true / false.</returns>
        public bool GetFirstDicomAttributeValue(String dimseCommandName, DvtkData.Dimse.Tag tag, out String attributeValue)
        {
            attributeValue = String.Empty;
            bool dimseCommandFound = false;

            if (_transaction is DicomTransaction)
            {
                String group = tag.GroupNumber.ToString("X").PadLeft(4, '0');
                String element = tag.ElementNumber.ToString("X").PadLeft(4, '0');
                String tagString = String.Format("0x{0}{1}", group, element);

                DicomTransaction dicomTransaction = (DicomTransaction)_transaction;
                for (int i = 0; i < dicomTransaction.DicomMessages.Count; i++)
                {
                    DicomMessage dicomMessage = (DicomMessage)dicomTransaction.DicomMessages[i];
                    if (dicomMessage.CommandSet != null)
                    {
                        if (dicomMessage.CommandSet.DimseCommandName == dimseCommandName)
                        {
                            dimseCommandFound = true;

                            // check the command set
                            if (dicomMessage.CommandSet.Exists(tagString) == true)
                            {
                                attributeValue = dicomMessage.CommandSet.GetValues(tagString)[0];
                            }
                            else if ((dicomMessage.DataSet != null) &&
                                (dicomMessage.DataSet.Exists(tagString) == true))
                            {
                                // check the dataset
                                attributeValue = dicomMessage.DataSet.GetValues(tagString)[0];
                            }
                            break;
                        }
                    }
                }
            }

            return dimseCommandFound;
        }

        /// <summary>
        /// Check if the given attribute is present in the DICOM message with the given DICOM command name. 
        /// First search the command set and then the dataset (if present).
        /// </summary>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <param name="tag">Tag identifying attribute whose first value will be returned.</param>
        /// <param name="attributePresent">Out - bool attribute present - true / false.</param>
        /// <returns>bool - indicates whether the dimseCommandName was found in the transaction or not - true / false.</returns>
        public bool IsDicomAttributePresent(String dimseCommandName, DvtkData.Dimse.Tag tag, out bool attributePresent)
        {
            attributePresent = false;
            bool dimseCommandFound = false;
            if (_transaction is DicomTransaction)
            {
                String group = tag.GroupNumber.ToString("X").PadLeft(4, '0');
                String element = tag.ElementNumber.ToString("X").PadLeft(4, '0');
                String tagString = String.Format("0x{0}{1}", group, element);

                DicomTransaction dicomTransaction = (DicomTransaction)_transaction;
                for (int i = 0; i < dicomTransaction.DicomMessages.Count; i++)
                {
                    DicomMessage dicomMessage = (DicomMessage)dicomTransaction.DicomMessages[i];
                    if (dicomMessage.CommandSet != null)
                    {
                        if (dicomMessage.CommandSet.DimseCommandName == dimseCommandName)
                        {
                            dimseCommandFound = true;

                            // check the command set
                            attributePresent = dicomMessage.CommandSet.Exists(tagString);
                            if ((dicomMessage.DataSet != null) &&
                                (attributePresent == false))
                            {
                                // check the dataset
                                attributePresent = dicomMessage.DataSet.Exists(tagString);
                            }
                            break;
                        }
                    }
                }
            }

            return dimseCommandFound;
        }

        #endregion assertion methods

        /// <summary>
		/// Set the Comparators needed to handle each message in the Transaction.
		/// </summary>
		/// <param name="comparatorCollection">Comparator collection to fill.</param>
		public void SetComparators(Dvtk.Comparator.BaseComparatorCollection comparatorCollection)
		{
			if (_transaction is DicomTransaction)
			{
				System.String name = System.String.Empty;

				DicomTransaction dicomTransaction = (DicomTransaction)_transaction;
				switch(dicomTransaction.Direction)
				{
					case TransactionDirectionEnum.TransactionReceived:
						name = System.String.Format("Received by {0}:{1} from {2}:{3}", 
							ActorTypes.Type(_toActorName.Type), 
							_toActorName.Id, 
							ActorTypes.Type(_fromActorName.Type), 
							_fromActorName.Id);
						break;
					case TransactionDirectionEnum.TransactionSent:
						name = System.String.Format("Sent from {0}:{1} to {2}:{3}", 
							ActorTypes.Type(_toActorName.Type), 
							_toActorName.Id, 
							ActorTypes.Type(_fromActorName.Type), 
							_fromActorName.Id);
						break;
					default:
						break;
				}

				for (int i = 0; i < dicomTransaction.DicomMessages.Count; i++)
				{
					DicomMessage dicomMessage = (DicomMessage)dicomTransaction.DicomMessages[i];

					DvtkHighLevelInterface.Comparator.Comparator comparator = new DvtkHighLevelInterface.Comparator.Comparator(name);
					Dvtk.Comparator.DicomComparator dicomComparator = comparator.InitializeDicomComparator(dicomMessage);
					if (dicomComparator != null)
					{
						comparatorCollection.Add(dicomComparator);
					}
				}
			}
			else if (_transaction is Hl7Transaction)
			{
				System.String name = System.String.Empty;

				Hl7Transaction hl7Transaction = (Hl7Transaction)_transaction;
				switch(hl7Transaction.Direction)
				{
					case TransactionDirectionEnum.TransactionReceived:
						name = System.String.Format("Received by {0}:{1} from {2}:{3}", 
							ActorTypes.Type(_toActorName.Type), 
							_toActorName.Id, 
							ActorTypes.Type(_fromActorName.Type), 
							_fromActorName.Id);
						break;
					case TransactionDirectionEnum.TransactionSent:
						name = System.String.Format("Sent from {0}:{1} to {2}:{3}", 
							ActorTypes.Type(_toActorName.Type), 
							_toActorName.Id, 
							ActorTypes.Type(_fromActorName.Type),
							_fromActorName.Id);
						break;
					default:
						break;
				}

				Hl7Message hl7Message= hl7Transaction.Request;

				DvtkHighLevelInterface.Comparator.Comparator comparator = new DvtkHighLevelInterface.Comparator.Comparator(name);
				Dvtk.Comparator.Hl7Comparator hl7Comparator = comparator.InitializeHl7Comparator(hl7Message);
				if (hl7Comparator != null)
				{
					comparatorCollection.Add(hl7Comparator);
				}
			}
		}
	}
}
