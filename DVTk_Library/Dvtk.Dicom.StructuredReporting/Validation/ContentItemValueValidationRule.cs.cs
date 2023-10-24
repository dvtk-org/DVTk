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

using Dvtk.Dicom.StructuredReporting;
using Specification = Dvtk.Dicom.StructuredReporting.Specification;

namespace Dvtk.Dicom.StructuredReporting.Validation
{
    public class ContentItemValueValidationRule : IContentItemVisitor
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Internal validation rule to validate a Concept Code (Concept name or part of value of Content Item) with.
        /// </summary>
        private ConceptCodeValidationRule conceptCodeValidationRule = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ContentItemValueValidationRule()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contextGroups">Contains all loaded Context Groups.</param>
        public ContentItemValueValidationRule(Specification.ContextGroups contextGroups)
        {
            this.conceptCodeValidationRule = new ConceptCodeValidationRule(contextGroups);
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Validate the value for a Content Item with Value Type Code.
        /// </summary>
        /// <param name="contentItemWithValueTypeCode">The Content Item.</param>
        private void ValidateValue(ContentItemWithValueTypeCode contentItemWithValueTypeCode)
        {
            ConceptCode conceptCode = contentItemWithValueTypeCode.ConceptCode;

            if (conceptCode == null)
            {
                contentItemWithValueTypeCode.ValidationResults.Add(new ValidationResult("Sequence Item encoding the Concept Code does not exist."));
            }
            else
            {
                conceptCodeValidationRule.ValidateUsingContextGroups(conceptCode);
            }
        }

        /// <summary>
        /// Validate the value for a Content Item with Value Type Num.
        /// </summary>
        /// <param name="contentItemWithValueTypeNum">The Content Item.</param>
        private void ValidateValue(ContentItemWithValueTypeNum contentItemWithValueTypeNum)
        {
            //
            // Validate the Measured Value.
            //

            MeasuredValue measuredValue = contentItemWithValueTypeNum.MeasuredValue;

            if (measuredValue != null)
            {
                ConceptCode measurementUnits = measuredValue.MeasurementUnits;

                if (measurementUnits == null)
                {
                    contentItemWithValueTypeNum.ValidationResults.Add(new ValidationResult("Sequence Item encoding the Measurement Units does not exist."));
                }
                else
                {
                    if (measurementUnits.CodingSchemeDesignator != "UCUM")
                    {
                        measurementUnits.ValidationResults.Add(new ValidationResult("Coding Scheme Designator is not equal to UCUM."));
                    }

                    if (!Dvtk.Ucum.Tools.IsValidTerm(measurementUnits.CodeValue, true, false))
                    {
                        measurementUnits.ValidationResults.Add(new ValidationResult("Code Value does not contain a valid Units of Measurement."));
                    }
                }
            }


            //
            // Validate the Numeric Value qualifier.
            //

            ConceptCode numericValueQualifier = contentItemWithValueTypeNum.NumericValueQualifier;

            if (numericValueQualifier != null)
            {
                this.conceptCodeValidationRule.ValidateUsingContextGroup("42", numericValueQualifier);
            }
        }

        /// <summary>
        /// Visit the supplied Content Item instance to validate its value.
        /// </summary>
        /// <param name="contentItem">The ContentItem instance to visit.</param>
        public void Visit(ContentItem contentItem)
        {
            if (contentItem is ContentItemWithValueTypeCode)
            {
                ValidateValue(contentItem as ContentItemWithValueTypeCode);
            }
            else if (contentItem is ContentItemWithValueTypeNum)
            {
                ValidateValue(contentItem as ContentItemWithValueTypeNum);
            }
            else
            {
                // Do nothing for the first increment.
            }
        }
    }
}
