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

using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Common.Other;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// Represents a Dicom Command Set.
	/// </summary>
	public class CommandSet: AttributeSet
	{
        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private CommandSet(): base(new DvtkData.Dimse.CommandSet(DvtkData.Dimse.DimseCommand.CSTORERQ))
        {
            // Do nothing.
        }
        
        /// <summary>
        /// Constructor. Creates an empty Command Set.
		/// </summary>
		/// <param name="dimseCommand">The Dimse Command.</param>
		internal CommandSet(DvtkData.Dimse.DimseCommand dimseCommand): base(new DvtkData.Dimse.CommandSet(dimseCommand))
		{
            // Do nothing.
		}

        /// <summary>
        /// Constructor. Encapsulates a DvtkData.Dimse.CommandSet instance.
        /// </summary>
        /// <param name="dvtkDataCommandSet">The DvtkData.Dimse.CommandSet instance to encapsulate.</param>
		internal CommandSet(DvtkData.Dimse.CommandSet dvtkDataCommandSet): base(dvtkDataCommandSet)
		{
			if (dvtkDataCommandSet == null)
			{
				throw new HliException("Parameter may not be null/Nothing.");
			}
		}



        //
        // - Properties -
        //

		/// <summary>
		/// Gets the Dimse Command.
		/// </summary>
		public DvtkData.Dimse.DimseCommand DimseCommand
		{
			get
			{
				return DvtkDataCommandSet.CommandField;
			}
		}

        /// <summary>
        /// Gets the Dimse Command as a readable string.
        /// </summary>
        public String DimseCommandName
        {
            get
            {
                return DvtkDataCommandSet.ToString(DvtkDataCommandSet.CommandField);
            }
        }

        /// <summary>
        /// Gets the encapsulated DvtkData.Dimse.CommandSet instance.
        /// </summary>
		internal DvtkData.Dimse.CommandSet DvtkDataCommandSet
		{
			get
			{
				return this.dvtkDataAttributeSet as DvtkData.Dimse.CommandSet;
			}
		}



        //
        // - Methods -
        //		

		/// <summary>
		/// Creates a deep copy of this instance.
		/// </summary>
        /// <returns>A deep copy of this instance.</returns>
		public CommandSet Clone()
		{
            // Create an empty Command Set.
			CommandSet cloneCommandSet = new CommandSet(this.DvtkDataCommandSet.CommandField);

            // Copy all attributes.
			for (int index = 0; index < this.Count; index++)
			{
				// The Add method will take care of the deep copy.
				cloneCommandSet.Add(this[index]);
			}

            // Return the deep copy.
			return(cloneCommandSet);
		}

		/// <summary>
        /// Makes this instance a deep copy of <paramref name="commandSet"/>.
		/// </summary>
		/// <param name="commandSet">The Command Set to copy from.</param>
		public void CloneFrom(CommandSet commandSet)
		{
			Clear();

			for (int index = 0; index < commandSet.Count; index++)
			{
				// The Add method will take care of the deep copy.
				Add(commandSet[index]);
			}
		}

        /// <summary>
        /// Returns the SOP Class UID.
        /// </summary>
        /// <remarks>
        /// Returns empty string if SOP Class UID not found. 
        /// </remarks>
        /// <returns>The SOP Class UID.</returns>
		public String GetSopClassUid()
		{
			String sopClassUid = String.Empty;

			// Try to get the SOP Class UID
			DvtkData.Dimse.Attribute sopClassUidAttribute = this.DvtkDataCommandSet.GetAttribute(DvtkData.Dimse.Tag.AFFECTED_SOP_CLASS_UID);

			if (sopClassUidAttribute == null)
			{
				sopClassUidAttribute = this.DvtkDataCommandSet.GetAttribute(DvtkData.Dimse.Tag.REQUESTED_SOP_CLASS_UID);
			}
			if (sopClassUidAttribute != null)
			{
				// Get the value of the SOP Class UID
				DvtkData.Dimse.UniqueIdentifier uniqueIdentifier = (DvtkData.Dimse.UniqueIdentifier)sopClassUidAttribute.DicomValue;
				if (uniqueIdentifier.Values.Count > 0)
				{
					sopClassUid = uniqueIdentifier.Values[0];
				}
			}

			return(sopClassUid);
		}

        /// <summary>
        /// Adds a single attribute with the tag, VR and value specified.
        /// </summary>
        /// <remarks>
        /// Only use this method for setting an attribute with VR OB, OF or OW.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag of the attribute.</param>
        /// <param name="vR">The VR (may only be OB, OF or OW) of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting a CommandSet attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="vR"/> is unequal to OB, OF or OW.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is a null reference.
        /// </exception>
        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Byte[] value)
        {
            TagSequence internalTagSequence = new TagSequence();

            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


            //
            // Sanity checks.
            //

            // Check if the tag supplied is valid for a CommandSet.
            if (!internalTagSequence.IsValidForCommandSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a CommandSet attribute.", "dvtkDataTag");
            }

            // Check the supplied VR.

            if ((vR != VR.OB) && (vR != VR.OF) && (vR != VR.OW))
            {
                throw new ArgumentException("Supplied VR is " + vR.ToString() + ". VR may only be OB, OF or OW.", "vR");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, value);
        }

        /// <summary>
        /// Adds a single attribute with the tag, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead supply
        /// multiple values arguments for this method when adding a single attribute with multiple values.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting a CommandSet attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, params Object[] values)
        {
            TagSequence internalTagSequence = new TagSequence();

            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


            //
            // Sanity checks.
            //

            // Check if the tag supplied is valid for a CommandSet.
            if (!internalTagSequence.IsValidForCommandSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a CommandSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
        }

        /// <summary>
        /// Adds a single attribute with the tag, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">The values, which will be copied from another attribute, for this attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting a CommandSet attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Values values)
        {
            TagSequence internalTagSequence = new TagSequence();

            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


            //
            // Sanity checks.
            //

            // Check if the tag supplied is valid for a CommandSet.
            if (!internalTagSequence.IsValidForCommandSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a CommandSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
        }

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and value specified.
        /// </summary>
        /// <remarks>
        /// Only use this method for setting an attribute with VR OB, OF or OW.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence of the attribute.</param>
        /// <param name="vR">The VR (may only be OB, OF or OW) of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a CommandSet attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="vR"/> is unequal to OB, OF or OW.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is a null reference.
        /// </exception>
        public override void Set(String tagSequence, VR vR, Byte[] value)
        {
            TagSequence internalTagSequence = new TagSequence(tagSequence);


            //
            // Sanity checks.
            //

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
            }

            // Check if the tag supplied is valid for a CommandSet.
            if (!internalTagSequence.IsValidForCommandSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a CommandSet attribute.", "dvtkDataTag");
            }

            // Check the supplied VR.

            if ((vR != VR.OB) && (vR != VR.OF) && (vR != VR.OW))
            {
                throw new ArgumentException("Supplied VR is " + vR.ToString() + ". VR may only be OB, OF or OW.", "vR");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, value);
        }

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead supply
        /// multiple values arguments for this method when adding a single attribute with multiple values.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a CommandSet attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(String tagSequence, VR vR, params Object[] values)
        {
            TagSequence internalTagSequence = new TagSequence(tagSequence);


            //
            // Sanity checks.
            //

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
            }

            // Check if the tag supplied is valid for a CommandSet.
            if (!internalTagSequence.IsValidForCommandSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a CommandSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("parameters");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
        }

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">The values, which will be copied from another attribute, for this attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a CommandSet attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(String tagSequence, VR vR, Values values)
        {
            TagSequence internalTagSequence = new TagSequence(tagSequence);


            //
            // Sanity checks.
            //

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
            }

            // Check if the tag supplied is valid for a CommandSet.
            if (!internalTagSequence.IsValidForCommandSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a CommandSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
        }
	}
}
