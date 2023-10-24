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

namespace Dvtk.IheActors.IntegrationProfile
{
	/// <summary>
	/// DEPRICATED - Summary description for IntegrationProfile.
    /// 
    /// This class remains for backwards compatibility reasons.
    /// It was originally used as the container for the IHE Actors. This container
    /// class has now been named IheFramework and is implemented as the base class
    /// for this IntegrationProfile class.
    /// Please use the IheFramework class instead of this class.
	/// </summary>
    public class IntegrationProfile : Dvtk.IheActors.IheFramework.IheFramework
	{
		/// <summary>
		/// Class Constructor.
		/// </summary>
		/// <param name="profileName">Integration Profile Name.</param>
        [System.Obsolete("This class is no longer to be used due to class renaming - use base IheFramework class directly instead.")]
        public IntegrationProfile(System.String profileName) : base(profileName) { }
	}
}
