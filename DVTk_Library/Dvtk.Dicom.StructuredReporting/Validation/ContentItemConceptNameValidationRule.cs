// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2010 DVTk
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
using System.Collections.Generic;
using System.Text;

using Vampire.Support;

using Dvtk.Dicom.StructuredReporting;
using Specification = Dvtk.Dicom.StructuredReporting.Specification;

namespace Dvtk.Dicom.StructuredReporting.Validation
{
    /// <summary>
    /// Validation rule to validate the Concept Name of a Content Item.
    /// </summary>
    public class ContentItemConceptNameValidationRule : IContentItemVisitor
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Internal validation rule to validate a Concept Code (Concept name or part of value of Content Item) with.
        /// </summary>
        ConceptCodeValidationRule conceptCodeValidationRule = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ContentItemConceptNameValidationRule()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contextGroups">Contains all loaded Context Groups.</param>
        public ContentItemConceptNameValidationRule(Specification.ContextGroups contextGroups)
        {
            this.conceptCodeValidationRule = new ConceptCodeValidationRule(contextGroups);
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Visit the supplied Content Item instance to validate its Concept Name.
        /// </summary>
        /// <param name="contentItem">The ContentItem instance to visit.</param>
        public void Visit(ContentItem contentItem)
        {
            ConceptCode conceptName = contentItem.ConceptName;

            if (conceptName == null)
            {
                contentItem.ValidationResults.Add(new ValidationResult("Sequence Item encoding the Concept Name does not exist."));
            }
            else
            {
                conceptCodeValidationRule.ValidateUsingContextGroups(conceptName);
            }
        }
    }
}
