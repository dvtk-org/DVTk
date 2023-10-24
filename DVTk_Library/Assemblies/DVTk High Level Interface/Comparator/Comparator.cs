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

using Dvtk.Comparator;
using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using DvtkHighLevelInterface.Dicom.Messages;



namespace DvtkHighLevelInterface.Comparator
{
	/// <summary>
    /// Class that encapsulate the Dvtk.Comparator.DicomComparator and Dvtk.Comparator.Hl7Comparator classes.
	/// </summary>
	public class Comparator
	{
		private DicomComparator _dicomComparator = null;
		private Hl7Comparator _hl7Comparator = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name to supply to the encapsulated Dvtk.Comparator.DicomComparator and Dvtk.Comparator.Hl7Comparator classes.</param>
		public Comparator(System.String name)
		{
			_dicomComparator = new DicomComparator(name);
			_hl7Comparator = new Hl7Comparator(name);
		}

        /// <summary>
        /// Initializes the encapsulated Dvtk.Comparator.DicomComparator class with the supplied DICOM message.
        /// </summary>
        /// <param name="dicomMessage">The DICOM message.</param>
        /// <returns>The encapsulated Dvtk.Comparator.DicomComparator instance.</returns>
		public DicomComparator InitializeDicomComparator(DicomMessage dicomMessage)
		{
			_hl7Comparator = null;

			bool initialized = _dicomComparator.Initialize(dicomMessage.DvtkDataDicomMessage);
			if (initialized == false)
			{
				_dicomComparator = null;
			}

			return _dicomComparator;
		}

        /// <summary>
        /// Initializes the encapsulated Dvtk.Comparator.Hl7Comparator class with the supplied HL7 message.
        /// </summary>
        /// <param name="hl7Message">The HL7 message.</param>
        /// <returns>The encapsulated Dvtk.Comparator.Hl7Comparator instance.</returns>
		public Hl7Comparator InitializeHl7Comparator(Hl7Message hl7Message)
		{
			_dicomComparator = null;

			bool initialized = _hl7Comparator.Initialize(hl7Message);
			if (initialized == false)
			{
				_hl7Comparator = null;
			}

			return _hl7Comparator;
		}

        /// <summary>
        /// See the Dvtk.Comparator.DicomComparator.PopulateMessage method.
        /// </summary>
        /// <param name="dicomMessage">See the Dvtk.Comparator.DicomComparator.PopulateMessage method.</param>
        /// <param name="dicomSourceComparator">See the Dvtk.Comparator.DicomComparator.PopulateMessage method.</param>
        /// <returns>See the Dvtk.Comparator.DicomComparator.PopulateMessage method.</returns>
		public bool PopulateDicomMessage(DicomMessage dicomMessage, DicomComparator dicomSourceComparator)
		{
			bool messagePopulated = false;

			if (_dicomComparator != null)
			{
				messagePopulated = _dicomComparator.PopulateMessage(dicomMessage.DvtkDataDicomMessage, dicomSourceComparator);
			}

			return messagePopulated;
		}

        /// <summary>
        /// See the Dvtk.Comparator.Hl7Comparator.PopulateMessage method.
        /// </summary>
        /// <param name="hl7Message">See the Dvtk.Comparator.Hl7Comparator.PopulateMessage method.</param>
        /// <param name="hl7SourceComparator">See the Dvtk.Comparator.Hl7Comparator.PopulateMessage method.</param>
        /// <returns>See the Dvtk.Comparator.Hl7Comparator.PopulateMessage method.</returns>
		public bool PopulateHl7Message(Hl7Message hl7Message, Hl7Comparator hl7SourceComparator)
		{
			bool messagePopulated = false;

			if (_hl7Comparator != null)
			{
				messagePopulated = _hl7Comparator.PopulateMessage(hl7Message, hl7SourceComparator);
			}

			return messagePopulated;
		}
	}
}
