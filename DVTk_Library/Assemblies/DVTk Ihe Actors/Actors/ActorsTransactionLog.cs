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

using Dvtk.Comparator;
using Dvtk.Results;
using DvtkData.Dimse;
using Dvtk.IheActors.Bases;
using DvtkHighLevelInterface.Common.Other;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for ActorsTransactionLog.
	/// </summary>
	public class ActorsTransactionLog : CollectionBase
	{
		Dvtk.Comparator.BaseComparatorCollection _comparatorCollection = new Dvtk.Comparator.BaseComparatorCollection();
        ArrayList _testAssertionResults = new ArrayList();

		/// <summary>
		/// Gets or sets an <see cref="ActorsTransaction"/> from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the collection member to get or set.</param>
		/// <value>The <see cref="ActorsTransactionLog"/> at the specified index.</value>
		public ActorsTransaction this[int index]  
		{
			get  
			{
				return ((ActorsTransaction) List[index]);
			}
			set  
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Adds an object to the end of the <see cref="ActorsTransactionLog"/>.
		/// </summary>
		/// <param name="value">The <see cref="ActorsTransaction"/> to be added to the end of the <see cref="ActorsTransactionLog"/>.</param>
		/// <returns>The <see cref="ActorsTransactionLog"/> index at which the value has been added.</returns>
		public int Add(ActorsTransaction value)  
		{
			return (List.Add(value));
		}

		/// <summary>
		/// Searches for the specified <see cref="ActorsTransaction"/> and 
		/// returns the zero-based index of the first occurrence within the entire <see cref="ActorsTransactionLog"/>.
		/// </summary>
		/// <param name="value">The <see cref="ActorsTransaction"/> to locate in the <see cref="ActorsTransactionLog"/>.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of value within the entire <see cref="ActorsTransactionLog"/>, 
		/// if found; otherwise, -1.
		/// </returns>
		public int IndexOf(ActorsTransaction value)  
		{
			return (List.IndexOf(value));
		}

		/// <summary>
		/// Inserts an <see cref="ActorsTransaction"/> element into the <see cref="ActorsTransactionLog"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="value">The <see cref="ActorsTransaction"/> to insert.</param>
		public void Insert(int index, ActorsTransaction value)  
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="ActorsTransaction"/> from the <see cref="ActorsTransactionLog"/>.
		/// </summary>
		/// <param name="value">The <see cref="ActorsTransaction"/> to remove from the <see cref="ActorsTransactionLog"/>.</param>
		public void Remove(ActorsTransaction value)  
		{
			List.Remove(value);
		}

		/// <summary>
		/// Determines whether the <see cref="ActorsTransactionLog"/> contains a specific element.
		/// </summary>
		/// <param name="value">The <see cref="ActorsTransaction"/> to locate in the <see cref="ActorsTransactionLog"/>.</param>
		/// <returns>
		/// <c>true</c> if the <see cref="ActorsTransactionLog"/> contains the specified value; 
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(ActorsTransaction value)  
		{
			// If value is not of type Code, this will return false.
			return (List.Contains(value));
		}

		/// <summary>
		/// Performs additional custom processes before inserting a new element into the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert value.</param>
		/// <param name="value">The new value of the element at index.</param>
		protected override void OnInsert(int index, Object value)  
		{
			if (!(value is ActorsTransaction))
				throw new ArgumentException("value must be of type ActorsTransaction.", "value");
		}

		/// <summary>
		/// Performs additional custom processes when removing an element from the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found.</param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemove(int index, Object value)  
		{
			if (!(value is ActorsTransaction))
				throw new ArgumentException("value must be of type ActorsTransaction.", "value");
		}

		/// <summary>
		/// Performs additional custom processes before setting a value in the collection instance.
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found.</param>
		/// <param name="oldValue">The value to replace with newValue.</param>
		/// <param name="newValue">The new value of the element at index.</param>
		protected override void OnSet(int index, Object oldValue, Object newValue)  
		{
			if (!(newValue is ActorsTransaction))
				throw new ArgumentException("newValue must be of type ActorsTransaction.", "newValue");
		}

		/// <summary>
		/// Performs additional custom processes when validating a value.
		/// </summary>
		/// <param name="value">The object to validate.</param>
		protected override void OnValidate(Object value)  
		{
			if (!(value is ActorsTransaction))
				throw new ArgumentException("value must be of type ActorsTransaction.");
		}
	
		/// <summary>
		/// Copies the elements of the <see cref="ICollection"/> to a strong-typed <c>ActorsTransaction[]</c>, 
		/// starting at a particular <c>ActorsTransaction[]</c> index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional <c>ActorsTransaction[]</c> that is the destination of the elements 
		/// copied from <see cref="ICollection"/>.
		/// The <c>ActorsTransaction[]</c> must have zero-based indexing. 
		/// </param>
		/// <param name="index">
		/// The zero-based index in array at which copying begins.
		/// </param>
		/// <remarks>
		/// Provides the strongly typed member for <see cref="ICollection"/>.
		/// </remarks>
		public void CopyTo(ActorsTransaction[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// Get the total number of errors occuring the Transaction Log.
		/// </summary>
		/// <returns>uint - Total number of errors.</returns>
		public uint NrErrors()
		{
			uint nrErrors = 0;

			foreach (ActorsTransaction actorsTransaction in this)
			{
				nrErrors += actorsTransaction.NrErrors;
			}

			return nrErrors;
		}

		/// <summary>
		/// Get the total number of warnings occuring the Transaction Log.
		/// </summary>
		/// <returns>uint - Total number of warnings.</returns>
		public uint NrWarnings()
		{
			uint nrWarnings = 0;

			foreach (ActorsTransaction actorsTransaction in this)
			{
				nrWarnings += actorsTransaction.NrWarnings;
			}

			return nrWarnings;
		}

		/// <summary>
		/// Add a Tag Value filter for the comparator.
		/// Only compare messages which contain the same values for this filter.
		/// </summary>
		/// <param name="tagValueFilter">Tag Value Filter.</param>
		public void AddComparisonTagValueFilter(DicomTagValue tagValueFilter)
		{
			_comparatorCollection.AddComparisonTagValueFilter(tagValueFilter);
        }

        #region Scenario Assertions
        /// <summary>
        /// Assert that the correct number of the given DICOM message has been seen between the two given actors.
        /// Stop after the first occurence of the given dimseCommandName in the transaction log.
        /// </summary>
        /// <param name="actorName1">First actor name in transaction.</param>
        /// <param name="actorName2">Second actor name in transaction.</param>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <param name="expectedCount">Number of times the given DICOM command was expected to occur between these actors.</param>
        /// <returns>bool - indication if the assertion was true or false.</returns>
        public bool AssertMessageCountBetweenActors(ActorName actorName1, ActorName actorName2, String dimseCommandName, int expectedCount)
        {
            bool assertion = true;
            bool matchFound = false;

            // check all transactions - until the first occurence of the dimseCommandName - then break 
            foreach (ActorsTransaction actorsTransaction in this)
            {
                // check if a transaction exists between the required actors
                String actorsTransactionFromActorNameTypeId = actorsTransaction.FromActorName.TypeId;
                String actorsTransactionToActorNameTypeId = actorsTransaction.ToActorName.TypeId;
                if (((actorsTransactionFromActorNameTypeId == actorName1.TypeId) &&
                    (actorsTransactionFromActorNameTypeId == actorName2.TypeId)) ||
                    ((actorsTransactionFromActorNameTypeId == actorName2.TypeId) &&
                    (actorsTransactionToActorNameTypeId == actorName1.TypeId)))
                {
                    // get the number of messages in the transaction with the given DICOM command name
                    int actualCount = actorsTransaction.GetNumberOfDicomMessages(dimseCommandName);
                    if (actualCount != -1)
                    {
                        matchFound = true;

                        if (actualCount != expectedCount)
                        {
                            // the expected count does not equal the actual count
                            String testAssertionResult = String.Format("Assertion Failure: AssertMessageCountBetweenActors {0} and {1} for DICOM Command {2} - expected: {3} - actual: {4}",
                                actorName1.TypeId, actorName2.TypeId, dimseCommandName, expectedCount, actualCount);
                            _testAssertionResults.Add(testAssertionResult);
                            assertion = false;
                        }
                        break;
                    }
                }                            
            }

            if (matchFound == false)
            {
                // the expected count does not equal the actual count
                String testAssertionResult = String.Format("Assertion Failure: AssertMessageCountBetweenActors {0} and {1} for DICOM Command {2} - expected: {3} - no corresponding messages found.",
                    actorName1.TypeId, actorName2.TypeId, dimseCommandName, expectedCount);
                _testAssertionResults.Add(testAssertionResult);
                assertion = false;
            }
            return assertion;
        }

        /// <summary>
        /// Assert that the given attribute value in the given DICOM message has the expected value between the two given actors.
        /// Check all occurences of the given dimseCommandName in the transaction log.
        /// </summary>
        /// <param name="actorName1">First actor name in transaction.</param>
        /// <param name="actorName2">Second actor name in transaction.</param>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <param name="tag">tag - Attribute tag whose first value is the actual value.</param>
        /// <param name="expectedValue">The expected attribute value - this will be compared with the actual attribute value.</param>
        /// <returns>bool - indication if the assertion was true or false.</returns>
        public bool AssertDicomAttributeValueBetweenActors(ActorName actorName1, ActorName actorName2, String dimseCommandName, DvtkData.Dimse.Tag tag, int expectedValue)
        {
            bool assertion = true;
            bool matchFound = false;

            // check all transactions 
            // - this assertion will test the value of the attribute tag in all occurences of the give dimseCommandName.
            foreach (ActorsTransaction actorsTransaction in this)
            {
                // check if a transaction exists between the required actors
                if (((actorsTransaction.FromActorName.TypeId == actorName1.TypeId) ||
                    (actorsTransaction.ToActorName.TypeId == actorName1.TypeId)) &&
                    ((actorsTransaction.FromActorName.TypeId == actorName2.TypeId) ||
                    (actorsTransaction.ToActorName.TypeId == actorName2.TypeId)))
                {
                    // get the value of the given attribute in the DICOM message with the given DICOM command name
                    String attributeValue = String.Empty;
                    if ((actorsTransaction.GetFirstDicomAttributeValue(dimseCommandName, tag, out attributeValue) == true) &&
                        (attributeValue != String.Empty))
                    {
                        matchFound = true;

                        int actualValue = int.Parse(attributeValue);
                        if (actualValue != expectedValue)
                        {
                            // the expected value does not equal the actual value
                            String testAssertionResult = String.Format("Assertion Failure: AssertDicomAttributeValueBetweenActors {0} and {1} for DICOM Command {2} - Tag ({3},{4}) - expected: {5} - actual: {6}",
                                actorName1.TypeId, 
                                actorName2.TypeId,
                                dimseCommandName, 
                                tag.GroupNumber.ToString("X").PadLeft(4, '0'),
                                tag.ElementNumber.ToString("X").PadLeft(4, '0'),
                                expectedValue, 
                                actualValue);
                            _testAssertionResults.Add(testAssertionResult);
                            assertion = false;
                        }
                    }
                }
            }

            if (matchFound == false)
            {
                // the expected count does not equal the actual count
                String testAssertionResult = String.Format("Assertion Failure: AssertDicomAttributeValueBetweenActors {0} and {1} for DICOM Command {2} - Tag ({3},{4}) - expected: {5} - no corresponding messages found.",
                    actorName1.TypeId, 
                    actorName2.TypeId, 
                    dimseCommandName, 
                    tag.GroupNumber.ToString("X").PadLeft(4, '0'), 
                    tag.ElementNumber.ToString("X").PadLeft(4, '0'),
                    expectedValue);
                _testAssertionResults.Add(testAssertionResult);
                assertion = false;
            }

            return assertion;
        }

        /// <summary>
        /// Assert that the given attribute value in the given DICOM message has the expected value between the two given actors.
        /// Check all occurences of the given dimseCommandName in the transaction log.
        /// </summary>
        /// <param name="actorName1">First actor name in transaction.</param>
        /// <param name="actorName2">Second actor name in transaction.</param>
        /// <param name="dimseCommandName">DICOM command name.</param>
        /// <param name="tag">tag - Attribute tag whose first value is the actual value.</param>
        /// <param name="expectedValue">The expected attribute value - this will be compared with the actual attribute value.</param>
        /// <returns>bool - indication if the assertion was true or false.</returns>
        public bool AssertDicomAttributeValueBetweenActors(ActorName actorName1, ActorName actorName2, String dimseCommandName, DvtkData.Dimse.Tag tag, String expectedValue)
        {
            bool assertion = true;
            bool matchFound = false;

            // check all transactions 
            // - this assertion will test the value of the attribute tag in all occurences of the give dimseCommandName.
            foreach (ActorsTransaction actorsTransaction in this)
            {
                // check if a transaction exists between the required actors
                if (((actorsTransaction.FromActorName.TypeId == actorName1.TypeId) ||
                    (actorsTransaction.ToActorName.TypeId == actorName1.TypeId)) &&
                    ((actorsTransaction.FromActorName.TypeId == actorName2.TypeId) ||
                    (actorsTransaction.ToActorName.TypeId == actorName2.TypeId)))
                {
                    // get the value of the given attribute in the DICOM message with the given DICOM command name
                    String actualValue = String.Empty;
                    if ((actorsTransaction.GetFirstDicomAttributeValue(dimseCommandName, tag, out actualValue) == true) &&
                        (actualValue != String.Empty))
                    {
                        matchFound = true;

                        if (actualValue != expectedValue)
                        {
                            // the expected value does not equal the actual value
                            String testAssertionResult = String.Format("Assertion Failure: AssertDicomAttributeValueBetweenActors {0} and {1} for DICOM Command {2} - Tag ({3},{4}) - expected: {5} - actual: {6}",
                                actorName1.TypeId,
                                actorName2.TypeId,
                                dimseCommandName,
                                tag.GroupNumber.ToString("X").PadLeft(4, '0'),
                                tag.ElementNumber.ToString("X").PadLeft(4, '0'),
                                expectedValue,
                                actualValue);
                            _testAssertionResults.Add(testAssertionResult);
                            assertion = false;
                        }
                    }
                }
            }

            if (matchFound == false)
            {
                // the expected count does not equal the actual count
                String testAssertionResult = String.Format("Assertion Failure: AssertDicomAttributeValueBetweenActors {0} and {1} for DICOM Command {2} - Tag ({3},{4}) - expected: {5} - no corresponding messages found.",
                    actorName1.TypeId,
                    actorName2.TypeId,
                    dimseCommandName,
                    tag.GroupNumber.ToString("X").PadLeft(4, '0'),
                    tag.ElementNumber.ToString("X").PadLeft(4, '0'),
                    expectedValue);
                _testAssertionResults.Add(testAssertionResult);
                assertion = false;
            }

            return assertion;
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
            bool assertion = true;
            bool matchFound = false;

            // check all transactions 
            // - this assertion will test the presence of the attribute tag in all occurences of the give dimseCommandName.
            foreach (ActorsTransaction actorsTransaction in this)
            {
                // check if a transaction exists between the required actors
                if (((actorsTransaction.FromActorName.TypeId == actorName1.TypeId) ||
                    (actorsTransaction.ToActorName.TypeId == actorName1.TypeId)) &&
                    ((actorsTransaction.FromActorName.TypeId == actorName2.TypeId) ||
                    (actorsTransaction.ToActorName.TypeId == actorName2.TypeId)))
                {
                    // get the value of the given attribute in the DICOM message with the given DICOM command name
                    bool attributePresent = false;
                    if (actorsTransaction.IsDicomAttributePresent(dimseCommandName, tag, out attributePresent) == true)
                    {
                        matchFound = true;

                        if (attributePresent == false)
                        {
                            // the attribute tag is not present
                            String testAssertionResult = String.Format("Assertion Failure: AssertDicomAttributePresentBetweenActors {0} and {1} for DICOM Command {2} - Tag ({3},{4}) - tag not found.",
                                actorName1.TypeId, 
                                actorName2.TypeId, 
                                dimseCommandName, 
                                tag.GroupNumber.ToString("X").PadLeft(4, '0'), 
                                tag.ElementNumber.ToString("X").PadLeft(4, '0'));
                            _testAssertionResults.Add(testAssertionResult);
                            assertion = false;
                        }
                    }
                }
            }

            if (matchFound == false)
            {
                // the expected count does not equal the actual count
                String testAssertionResult = String.Format("Assertion Failure: AssertDicomAttributePresentBetweenActors {0} and {1} for DICOM Command {2} - Tag ({3},{4}) - no corresponding messages found.",
                    actorName1.TypeId,
                    actorName2.TypeId,
                    dimseCommandName,
                    tag.GroupNumber.ToString("X").PadLeft(4, '0'),
                    tag.ElementNumber.ToString("X").PadLeft(4, '0'));
                _testAssertionResults.Add(testAssertionResult);
                assertion = false;
            }

            return assertion;
        }

        #endregion Scenario Assertions

        /// <summary>
		/// Evaluate the transaction log results using the associated comparators.
		/// </summary>
		/// <param name="resultsReporter">Results reporter.</param>
		public void Evaluate(ResultsReporter resultsReporter)
		{
			ArrayList allActorsTransactions = new ArrayList();

			for (int i = 0; i < this.Count; i++)
			{
				foreach (ActorsTransaction actorsTransaction in this)
				{
					if (actorsTransaction.TransactionNumber == i + 1)
					{
						allActorsTransactions.Add(actorsTransaction);
					}
				}
			}

			ArrayList ActorsTransactionsToCompare = this.compareCondition.Filter(allActorsTransactions);

			foreach (ActorsTransaction actorsTransaction in ActorsTransactionsToCompare)
			{
				actorsTransaction.SetComparators(_comparatorCollection);
			}


			if (_comparatorCollection.TagValueCollection.Count > 0)
			{
                resultsReporter.WriteHtmlInformation("<h2>Message Comparison</h2>");
				resultsReporter.WriteValidationInformation("Comparators Filtered on:");
				foreach(DicomTagValue tagValue in _comparatorCollection.TagValueCollection)
				{
					string group = tagValue.Tag.GroupNumber.ToString("X").PadLeft(4, '0');
                    string element = tagValue.Tag.ElementNumber.ToString("X").PadLeft(4, '0');
					System.String message;
					if (tagValue.Value == System.String.Empty)
					{
						message = System.String.Format("Attribute Tag: ({0},{1}) - Value: Universal Match",
							group,
							element);
					}
					else
					{
						message = System.String.Format("Attribute Tag: ({0},{1}) - Value: \"{2}\"",
						group,
						element,
						tagValue.Value);
					}
					resultsReporter.WriteValidationInformation(message);
				}
			}

			_comparatorCollection.Compare(resultsReporter);

            if (_testAssertionResults.Count > 0)
            {
                resultsReporter.WriteHtmlInformation("<br/><br/><h2>Scenario Assertions</h2>");

                foreach (String testAssertionResult in _testAssertionResults)
                {
                    resultsReporter.WriteValidationError(testAssertionResult);
                }
            }

            // update the error and warnings counters
            resultsReporter.AddValidationErrors(this.NrErrors());
            resultsReporter.AddValidationWarnings(this.NrWarnings());
		}

		/// <summary>
		/// Log the Transactions to the given file.
		/// </summary>
        /// <param name="filename">The filename used to store the transaction log.</param>
		public void LogToFile(String filename)
		{
            StreamWriter sw = new StreamWriter(filename);
            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    // iterate over a second time to get the transaction in the correct order
                    foreach (ActorsTransaction actorsTransaction in this)
                    {
                        if (actorsTransaction.TransactionNumber == i + 1)
                        {
                            actorsTransaction.LogToStream(sw);
                        }
                    }
                }

                sw.WriteLine("Total {0} errors, {1} warnings", this.NrErrors(), this.NrWarnings());
            }
            catch (System.Exception)
            {
            }
            sw.Close();
		}

		private Condition compareCondition = new ConditionTrue();

		public Condition CompareCondition
		{
			get
			{
				return(this.compareCondition);
			}
			set
			{
				this.compareCondition = value;
			}
		}
	}
}
