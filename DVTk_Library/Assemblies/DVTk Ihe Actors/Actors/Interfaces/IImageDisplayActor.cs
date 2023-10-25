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

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Comparator;
using Dvtk.Dicom.InformationEntity.DefaultValues;
using DvtkData.Dimse;
using Dvtk.Comparator;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Dicom;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for IImageDisplayActor.
	/// </summary>
	interface IImageDisplayActor
	{
		#region SendQueryImages() overloads
        /// <summary>
        /// Send a C-FIND-RQ Information Model Query.
        /// Query based on the informationModel provided and the query/retrieve level. Take
        /// the query tags from the queryTags provided.
        /// 
        /// The C-FIND-RSP messages returned are stored in a DicomQueryItemCollection named QueryItems.
        /// </summary>
        /// <param name="informationModel">Q/R Information Model to be used in the query operation.</param>
        /// <param name="level">Query / retrieve level.</param>
        /// <param name="queryTags">List of Query Tags.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendQueryImages(QueryRetrieveInformationModelEnum informationModel, QueryRetrieveLevelEnum level, TagValueCollection queryTags);

        /// <summary>
		/// Property - QueryItems
		/// </summary>
        /// <param name="level">Q/R Information Model that was used in the corresponding SendQueryImages() method call.</param>
        /// <returns>Collection of C-FIND-RSP messages that contain a C-FIND-RSP Dataset. The number of entries in the collection
        /// indicates the number of query matches.</returns>
        DicomQueryItemCollection QueryItems(QueryRetrieveLevelEnum level);

		#endregion

		#region SendRetrieveImages() overloads
        /// <summary>
        /// Send a C-MOVE-RQ Information Model Retrieve.
        /// Retrieve based on the informationModel provided and the query/retrieve level. Take
        /// the retrieve tags from the retrieveTags provided. The retrieve is done to the move
        /// destination.
        /// 
        /// The C-MOVE-RSP messages returned are stored in a DicomQueryItemCollection named RetrieveItems.
        /// </summary>
        /// <param name="informationModel">Q/R Information Model to be used in the retrieve operation.</param>
        /// <param name="level">Query / retrieve level.</param>
        /// <param name="moveDestination">AE Title of the "move" destination.</param>
        /// <param name="retrieveTags">List of Retrieve Tags.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendRetrieveImages(QueryRetrieveInformationModelEnum informationModel, QueryRetrieveLevelEnum level, System.String moveDestination, TagValueCollection retrieveTags);
		#endregion
	}
}
