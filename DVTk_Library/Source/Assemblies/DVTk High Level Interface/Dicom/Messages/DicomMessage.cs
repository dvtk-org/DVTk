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

using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Other;
using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;
using VR = DvtkData.Dimse.VR;



namespace DvtkHighLevelInterface.Dicom.Messages
{
	/// <summary>
	/// Represents a Dicom Message.
	/// </summary>
	public class DicomMessage: DicomProtocolMessage
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property CommandSet.
		/// </summary>
		private CommandSet commandSet = null;

		/// <summary>
		/// See property DataSet.
		/// </summary>
		private DataSet dataSet = null;

        /// <summary>
        /// See property EncodedPresentationContextID.
        /// </summary>
        private Byte encodedPCID = 1;


		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor.
		/// 
		/// Use this constructor to create a DicomMessage with an empty command set
		/// and empty data set.
		/// </summary>
		/// <param name="dimseCommand">The Dimse command.</param>
		public DicomMessage(DvtkData.Dimse.DimseCommand dimseCommand)
		{		
			this.commandSet = new CommandSet(dimseCommand);
			this.dataSet = new DataSet();
		}

        /// <summary>
        /// Constructor.
        /// 
        /// Use this constructor to create a DicomMessage with an empty command set
        /// and the given Dataset.
        /// </summary>
        /// <param name="dimseCommand">The Dimse command.</param>
        /// <param name="dataSet">The Dataset.</param>
        public DicomMessage(DvtkData.Dimse.DimseCommand dimseCommand, DataSet dataSet)
        {
            this.commandSet = new CommandSet(dimseCommand);
            this.dataSet = dataSet;
        }
	
		internal DicomMessage(CommandSet commandSet, DataSet dataSet)
		{
			this.commandSet = commandSet;
			this.dataSet = dataSet;
		}

		/// <summary>
		/// Constructor.
		/// 
		/// Use this constructor to construct the command set and data set based on the 
		/// command set and data set contained in the encapsulated DvtkData DicomMessage.
		/// </summary>
		/// <param name="dvtkDataDicomMessage">The encapsulated DvtkData DicomMessage.</param>
		internal DicomMessage(DvtkData.Dimse.DicomMessage dvtkDataDicomMessage)
		{
			// Sanity check.
			if (dvtkDataDicomMessage == null)
			{
				throw new HliException("Parameter may not be null.");
			}

			// Create the CommandSet object.
			this.commandSet = new CommandSet(dvtkDataDicomMessage.CommandSet);

			// Create the DataSet object.
			if (dvtkDataDicomMessage.DataSet == null)
			{
				this.dataSet = new DataSet();
			}
			else
			{
				this.dataSet = new DataSet(dvtkDataDicomMessage.DataSet);
			}

            this.encodedPCID = dvtkDataDicomMessage.EncodedPresentationContextID;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// The CommandSet of this DicomMessage.
		/// </summary>
		public CommandSet CommandSet
		{
			get
			{
				return commandSet;
			}
		}

		/// <summary>
		/// The DataSet of this DicomMessage.
		/// </summary>
		public DataSet DataSet
		{
			get
			{
				return dataSet;
			}
		}

        /// <summary>
        /// The presentation context ID represents by which Dicom Message is received from the network.
        /// </summary>
        public Byte EncodedPresentationContextID
        {
            get
            {
                return encodedPCID;
            }            
        }

		/// <summary>
		/// The encapsulated DvtkData DicomMessage.
		/// 
		/// This DvtkData DicomMessage is not stored inside this class but is reconstructed from
		/// the encapsulated DvtkData CommandSet and encapsulated DvtkData DataSet.
		/// </summary>
		internal DvtkData.Dimse.DicomMessage DvtkDataDicomMessage
		{
			get
			{
				DvtkData.Dimse.DicomMessage dvtkDataDicomMessage = new DvtkData.Dimse.DicomMessage();

				if (this.dataSet.DvtkDataDataSet.Count > 0)
				{
                    dvtkDataDicomMessage.Apply(this.commandSet.DvtkDataCommandSet, this.dataSet.DvtkDataDataSet, this.EncodedPresentationContextID);
				}
				else
				{
                    dvtkDataDicomMessage.Apply(this.commandSet.DvtkDataCommandSet, this.EncodedPresentationContextID);
				}

				return dvtkDataDicomMessage;
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Indicates if the specified attribute exists.
		/// </summary>
        /// <param name="tagSequenceString">The tag sequence (see class TagSequence for the format).</param>
		/// <returns>Boolean indicating if the specified attribute exists.</returns>
		public bool Exists(String tagSequenceString)
		{
			bool exists = false;

			TagSequence tagSequence = new TagSequence(tagSequenceString);

			if (!tagSequence.IsSingleAttributeMatching)
			{
				throw new HliException(tagSequenceString.ToString() + " not valid for the Exists method.");
			}
			
			if (tagSequence.IsValidForCommandSet)
			{
				exists = this.CommandSet.Exists(tagSequence);
			}
			else if (tagSequence.IsValidForDataSet)
			{
				exists = this.DataSet.Exists(tagSequence);
			}
			else
			{
				throw new HliException(tagSequenceString.ToString() + " not valid for a DicomMessage attribute.");
			}

			return(exists);
		}

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
		public DicomMessage Clone()
		{
			CommandSet cloneCommandSet = this.CommandSet.Clone();
			DataSet cloneDataSet = this.DataSet.Clone();

			DicomMessage cloneDicomMessage = new DicomMessage(cloneCommandSet, cloneDataSet);

			return(cloneDicomMessage);
		}

		/// <summary>
		/// Display all attributes on the console.
		/// </summary>
		public void ConsoleDisplay()
		{
			if (CommandSet != null)
			{
				Console.WriteLine("Command:");
				CommandSet.DvtkDataCommandSet.ConsoleDisplay();
			}
			if (DataSet != null)
			{
				Console.WriteLine("Dataset:");
				DataSet.DvtkDataDataSet.ConsoleDisplay();
			}
		}

        /// <summary>
        /// Gets the attribute that has the supplied tag sequence string.
        /// </summary>
        /// <param name="tagSequenceString"></param>
        /// <returns></returns>
		public Attribute this[String tagSequenceString]
		{
			get 
			{
				Attribute attribute = null;

				TagSequence tagSequence = new TagSequence(tagSequenceString);

				if (!tagSequence.IsSingleAttributeMatching)
				{
					throw new HliException(tagSequenceString.ToString() + " not valid for the index property.");				}

				if (tagSequence.IsValidForCommandSet)
				{
					attribute = this.CommandSet[tagSequence];
				}
				else if (tagSequence.IsValidForDataSet)
				{
					attribute = this.DataSet[tagSequence];
				}
				else
				{
					throw new HliException(tagSequenceString.ToString() + " not valid for a DicomMessage attribute.");
				}

				return(attribute);
			}
		}

        /// <summary>
        /// Adds a single attribute with the tag, VR and values specified.
        /// </summary>
        /// <remarks>
        /// Depending on the group number of the tag, the attribute
        /// is set in the CommandSet or DataSet of this instance.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag, it is removed first before it is 
        /// again set.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag that uniquely identifies the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="parameters">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead use
        /// multiple parameter arguments for this method when adding a single attribute with multiple values.
        /// </param>
        public void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, params Object[] parameters)
		{
			TagSequence tagSequence = new TagSequence();

			tagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));

			Set(tagSequence, vR, parameters);
		}

        /// <summary>
        /// Adds a single attribute with the tag sequence string, VR and values specified.
        /// </summary>
        /// <remarks>
        /// Depending on the group number of the last tag in the tag sequence string, the attribute
        /// is set in the CommandSet or DataSet of this instance.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag sequence string, it is removed first before it is 
        /// again set.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence string,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequenceString">The tag sequence string that uniquely identifies the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="parameters">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead use
        /// multiple parameter arguments for this method when adding a single attribute with multiple values.
        /// </param>
		public void Set(String tagSequenceString, VR vR, params Object[] parameters)
		{
			TagSequence tagSequence = new TagSequence(tagSequenceString);

			Set(tagSequence, vR, parameters);
		}

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and values specified.
        /// </summary>
        /// <remarks>
        /// Depending on the group number of the last tag in the tag sequence, the attribute
        /// is set in the CommandSet or DataSet of this instance.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag sequence, it is removed first before it is 
        /// again set.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence that uniquely identifies the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="parameters">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead use
        /// multiple parameter arguments for this method when adding a single attribute with multiple values.
        /// </param>
		internal void Set(TagSequence tagSequence, VR vR, params Object[] parameters)
		{
			// Check if the TagSequence supplied uniquely identifies one attribute.
			if (!tagSequence.IsSingleAttributeMatching)
			{
				throw new HliException(tagSequence.ToString() + " not valid for setting an attribute.");
			}

			if (tagSequence.IsValidForCommandSet)
			{
				CommandSet.Set(tagSequence, vR, parameters);
			}
			else if (tagSequence.IsValidForDataSet)
			{
				DataSet.Set(tagSequence, vR, parameters);
			}
			else
			{
				throw new HliException(tagSequence.ToString() + " not valid for setting a DicomMessage attribute.");
			}
		}

        /// <summary>
        /// Returns a String that represents this instance.
        /// </summary>
        /// <returns>A String that represents this instance.</returns>
		public override string ToString()
		{
			return "Dicom message with Dimse Command " + this.CommandSet.DimseCommand.ToString(); 
		}
	}
}
