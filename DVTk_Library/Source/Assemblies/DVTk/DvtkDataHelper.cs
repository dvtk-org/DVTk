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
using Wrappers;
using DvtkData.Dimse;
using DvtkData.Media;

namespace Dvtk
{
	/// <summary>
	/// 
	/// </summary>
	public class DvtkDataHelper
	{
		// Don't instantiate.
		private DvtkDataHelper()
		{
		}

		/// <summary>
		/// DEPRICATED - see similar method below with extra useUnVrDefinitionLookUp parameter.
		/// Read the FMI from the given Media file.
		/// </summary>
		/// <param name="dataSetFileName">Media filename to read.</param>
		/// <returns>Imported FileMetaInformation.</returns>
		public static FileMetaInformation ReadFMIFromFile(string dataSetFileName)
		{
			if (dataSetFileName == null)
			{
				throw new System.ArgumentNullException("dataSetFileName");
			}

			// Read FMI but consult definitions for any Attributes with VR UN to get real VR
			return MDataSet.ReadFMI(dataSetFileName, true);
		}

		/// <summary>
		/// Read the FMI from the given Media file. The useUnVrDefinitionLookUp parameter is used to indicate whether
		/// the definition files loaded should be used to get the real VR of an attributed decoded with a VR of UN.
		/// When set true a definition lookup for the real VR will be made and the corresponding attribute value
		/// decoded usign this looked up VR.
		/// When set false no look up will be done for attributes decoded with a UN VR. The attribute values will be
		/// decoded with VR of UN.
		/// </summary>
		/// <param name="dataSetFileName">Media filename to read.</param>
		/// <param name="useUnVrDefinitionLookUp">Boolean - UN VR Definition LookUp.</param>
		/// <returns>Imported FileMetaInformation.</returns>
		public static FileMetaInformation ReadFMIFromFile(string dataSetFileName, bool useUnVrDefinitionLookUp)
		{
			if (dataSetFileName == null)
			{
				throw new System.ArgumentNullException("dataSetFileName");
			}

			return MDataSet.ReadFMI(dataSetFileName, useUnVrDefinitionLookUp);
		}

        /// <summary>
		/// DEPRICATED - see similar method below with extra useUnVrDefinitionLookUp parameter.
		/// Read a DataSet from the given Media file.
        /// </summary>
        /// <param name="dataSetFileName">Media filename to read.</param>
        /// <returns>Imported Dataset.</returns>
		public static DataSet ReadDataSetFromFile(string dataSetFileName)
		{
			if (dataSetFileName == null)
			{
				throw new System.ArgumentNullException("dataSetFileName");
			}

			// Read File but consult definitions for any Attributes with VR UN to get real VR
			return MDataSet.ReadFile(dataSetFileName, true);
		}

		/// <summary>
		/// Read a DataSet from the given Media file. The useUnVrDefinitionLookUp parameter is used to indicate whether
		/// the definition files loaded should be used to get the real VR of an attributed decoded with a VR of UN.
		/// When set true a definition lookup for the real VR will be made and the corresponding attribute value
		/// decoded usign this looked up VR.
		/// When set false no look up will be done for attributes decoded with a UN VR. The attribute values will be
		/// decoded with VR of UN.
		/// </summary>
		/// <param name="dataSetFileName">Media filename to read.</param>
		/// <param name="useUnVrDefinitionLookUp">Boolean - UN VR Definition LookUp.</param>
		/// <returns>Imported Dataset.</returns>
		public static DataSet ReadDataSetFromFile(string dataSetFileName, bool useUnVrDefinitionLookUp)
		{
			if (dataSetFileName == null)
			{
				throw new System.ArgumentNullException("dataSetFileName");
			}

			return MDataSet.ReadFile(dataSetFileName, useUnVrDefinitionLookUp);
		}

		/// <summary>
		/// Write a dicom file object to a (persistent) Media Storage file.
		/// </summary>
		/// <param name="file">dicom file object to write</param>
		/// <param name="mediaFileName">file name to write to</param>
		/// <returns></returns>
		public static bool WriteDataSetToFile(
			DicomFile file,
			string mediaFileName)
		{
			if (file == null) throw new System.ArgumentNullException("file");
			if (mediaFileName == null) throw new System.ArgumentNullException("mediaFileName");

			FileMetaInformation fmi = file.FileMetaInformation;
			if (fmi == null) 
			{
				fmi = new FileMetaInformation();

				//Set the default transfer syntax(ELE) attribute in FMI
				fmi.AddAttribute("0x00020010",VR.UI,"1.2.840.10008.1.2.1");

				file.FileMetaInformation = fmi;
			}

			return MDataSet.WriteFile(file, mediaFileName);
		}

		/// <summary>
		/// Write DICOM object to a (persistent) Media Storage file.
		/// </summary>
		/// <param name="dataSet">dicom object to write</param>
		/// <param name="mediaFileName">file name to write to</param>
		/// <param name="transferSyntax">with transfer syntax to write</param>
		/// <returns></returns>
		public static bool WriteDataSet(
			DataSet dataSet,
			string mediaFileName,
			string transferSyntax)
		{
			if (dataSet == null) throw new System.ArgumentNullException("dataSet");
			if (mediaFileName == null) throw new System.ArgumentNullException("mediaFileName");

			return MDataSet.WriteDataSet(dataSet, mediaFileName, transferSyntax);
		}

		/// <summary>
		/// Compute the Item Offset for DICOMDIR.
		/// </summary>
		/// <param name="dataSet">DICOM Seq Item</param>
		/// <param name="transferSyntax">with transfer syntax</param>
		/// <returns>Offset</returns>
		public static UInt32 ComputeItemOffset(
			DataSet dataSet,
			string transferSyntax)
		{
			if (dataSet == null) throw new System.ArgumentNullException("dataSet");

			return MDataSet.ComputeItemOffset(dataSet, transferSyntax);
		}

		/// <summary>
		/// Compare two pixel attributes (i.e. attributes that have VR OB, OF or OW).
		/// </summary>
		/// <param name="attribute1">The first attribute.</param>
		/// <param name="attribute2">The second attribute.</param>
		/// <returns>Indicates if the two are equal or not.</returns>
		public static bool ComparePixelAttributes(
			DvtkData.Dimse.Attribute attribute1, 
			DvtkData.Dimse.Attribute attribute2)
		{
			bool equal = true;

			equal = MAttributeUtilities.ComparePixelAttributes(attribute1, attribute2);

			return(equal);
		}
	}
}
