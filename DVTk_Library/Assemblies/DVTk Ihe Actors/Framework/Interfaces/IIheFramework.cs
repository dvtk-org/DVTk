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

using DvtkHighLevelInterface;
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
	/// Summary description for IIheFramework.
	/// </summary>
	public interface IIheFramework
	{
		/// <summary>
		/// Get the Actor with the given actor name.
		/// </summary>
		/// <param name="actorName">Actor Name to seach for.</param>
		/// <returns>Actor with given name - maybe null.</returns>
		BaseActor GetActor(ActorName actorName);

		/// <summary>
		/// Add a Tag Value filter for the comparator.
		/// Only compare messages which contain the same values for this filter.
		/// </summary>
		/// <param name="tagValueFilter">Tag Value Filter.</param>
		void AddComparisonTagValueFilter(DicomTagValue tagValueFilter);

		/// <summary>
		/// Add user defined default Tag Values. Used to help define the message tag/values 
		/// used during the tests.
		/// </summary>
		/// <param name="defaultTagValue">Default Tag Value pair.</param>
		void AddUserDefinedDefaultTagValue(BaseDicomTagValue defaultTagValue);

		/// <summary>
		/// Open the Results reporting.
		/// </summary>
		void OpenResults();

		/// <summary>
		/// Close the Results reporting.
		/// </summary>
		/// <returns>System.String - results filename.</returns>
		System.String CloseResults();

		/// <summary>
		/// Apply the ActorConfigs by instantiating the corresponding actors.
		/// </summary>
		void ApplyConfig();

		/// <summary>
		/// Start the integration profile test by starting up all the
		/// configured actors.
		/// </summary>
		void StartTest();

		/// <summary>
		/// Stop the integration profile test by stopping all the configured
		/// actors.
		/// </summary>
		void StopTest();

		/// <summary>
		/// Evaluate the integration profile test.
		/// </summary>
		void EvaluateTest();

		/// <summary>
		/// Clean up the current working directory at the end of a test.
		/// PIX files are left in the current working directory after a test. These should be cleaned
		/// up by the Media File Class destructor in the C++ code but it seems that this is not always
		/// called. So a more robust approach is to delete the files explicitly.
		/// </summary>
		void CleanUpCurrentWorkingDirectory();
	}
}
