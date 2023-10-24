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

using VR = DvtkData.Dimse.VR;

namespace Dvtk.Dicom.StructuredReporting.Validation
{
    /// <summary>
    /// Class containing different methods to validate a Concept Code.
    /// </summary>
    internal class ConceptCodeValidationRule
    {
        //
        // - Fields -
        //

        /// <summary>
        /// List of all available Context Groups.
        /// </summary>
        Specification.ContextGroups contextGroups = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ConceptCodeValidationRule()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contextGroups">List of all available Context Groups.</param>
        public ConceptCodeValidationRule(Specification.ContextGroups contextGroups)
        {
            //
            // Sanity checks.
            //
            
            if (contextGroups == null)
            {
                throw new ArgumentNullException("contextGroups");
            }


            //
            // Store the supplied instance.
            //

            this.contextGroups = contextGroups;
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Validate the Code Meaning.
        /// </summary>
        /// <param name="matchingCodedConcepts">The matching Coded Concepts for the supplied Concept Code.</param>
        /// <param name="conceptCode">The Concept Code.</param>
        private void ValidateCodeMeaning(IList<Specification.CodedConcept> matchingCodedConcepts, ConceptCode conceptCode)
        {
            string conceptCodeCodeMeaning = conceptCode.CodeMeaning;

            if (conceptCodeCodeMeaning == null)
            {
                conceptCode.ValidationResults.Add(new ValidationResult("Code Meaning attribute does not exist."));
            }
            else if (conceptCodeCodeMeaning == "")
            {
                conceptCode.ValidationResults.Add(new ValidationResult("First value of Code Meaning attribute is empty."));
            }
            else
            {
                bool matchingCodeMeaningFound = false;

                foreach (Specification.CodedConcept codedConcept in matchingCodedConcepts)
                {
                    string codedConceptCodeMeaning = Convert.ToTrimmedString(codedConcept.CodeMeaning, VR.LO);

                    if (conceptCodeCodeMeaning == codedConceptCodeMeaning)
                    {
                        matchingCodeMeaningFound = true;
                        break;
                    }
                }

                if (!matchingCodeMeaningFound)
                {
                    conceptCode.ValidationResults.Add(new ValidationResult("Code meaning not correct for Coding Scheme Designator and Code Value pair."));
                }
            }
        }

        /// <summary>
        /// Validate the Code Value on its own.
        /// </summary>
        /// <param name="conceptCode">The Concept Code.</param>
        private void ValidateCodeValue(ConceptCode conceptCode)
        {
            string codeValue = conceptCode.CodeValue;

            if (codeValue == null)
            {
                conceptCode.ValidationResults.Add(new ValidationResult("Code Value attribute does not exist."));
            }
            else if (codeValue == "")
            {
                conceptCode.ValidationResults.Add(new ValidationResult("First value of Code Value attribute is empty."));
            }
        }

        /// <summary>
        /// Validate the Coding Scheme Designator on its own.
        /// </summary>
        /// <param name="conceptCode">The Concept Code.</param>
        private void ValidateCodingSchemeDesignator(ConceptCode conceptCode)
        {
            string codingSchemeDesignator = conceptCode.CodingSchemeDesignator;

            if (codingSchemeDesignator == null)
            {
                conceptCode.ValidationResults.Add(new ValidationResult("Coding Scheme Designator attribute does not exist."));
            }
            else if (codingSchemeDesignator == "")
            {
                conceptCode.ValidationResults.Add(new ValidationResult("First value of Coding Scheme Designator attribute is empty."));
            }
        }

        /// <summary>
        /// Validate the Concept Code using one specific Context Group (if loaded).
        /// </summary>
        /// <param name="cid">The Context ID of the Context Group to use.</param>
        /// <param name="conceptCode">The Concept Code to validate.</param>
        public void ValidateUsingContextGroup(string cid, ConceptCode conceptCode)
        {
            //
            // Sanity checks.
            //

            if (cid == null)
            {
                throw new ArgumentNullException("cid");
            }

            if (conceptCode == null)
            {
                throw new ArgumentNullException("conceptCode");
            }


            //
            // Validate the Coding Scheme Designator on its own.
            //

            ValidateCodingSchemeDesignator(conceptCode);


            //
            // Validate the Code Value on its own.
            //

            ValidateCodeValue(conceptCode);


            //
            // Validate the Coding Scheme Designator and Code Value pair: 
            // check if this pair is present in the Context Group that is specified by the supplied Content ID.
            //

            string codingSchemeDesignator = conceptCode.CodingSchemeDesignator;
            string codeValue = conceptCode.CodeValue;

            IList<Specification.CodedConcept> matchingCodedConcepts = null;

            if ((codingSchemeDesignator != null) && (codeValue != null) && (codingSchemeDesignator != "") && (codeValue != ""))
            {
                matchingCodedConcepts = this.contextGroups.GetCodedConcepts(cid, codingSchemeDesignator, codeValue);

                if (matchingCodedConcepts == null)
                {
                    ValidationResult validationResult =
                        new ValidationResult
                            ("Unable to validate Coding Scheme Designator and Code Value pair because specific Context Group is not loaded.",
                             "Unable to validate Coding Scheme Designator and Code Value pair because Context Group \"" + cid + "\" is not loaded.");

                    conceptCode.ValidationResults.Add(validationResult);
                }
                else

                if (matchingCodedConcepts.Count == 0)
                {
                    ValidationResult validationResult =
                        new ValidationResult
                            ("For the Coding Scheme Designator and Code Value pair, no Coded Concept could be found in specific Context Group.",
                             "For the Coding Scheme Designator and Code Value pair, no Coded Concept could be found in Context Group \"" + cid + "\".");

                    conceptCode.ValidationResults.Add(validationResult);
                }
            }


            //
            // Validate the Code Meaning.
            //

            if ((matchingCodedConcepts != null) && (matchingCodedConcepts.Count > 0))
            {
                ValidateCodeMeaning(matchingCodedConcepts, conceptCode);
            }
        }

        /// <summary>
        /// Validate the Concept Code using all loaded Context Groups).
        /// </summary>
        /// <param name="conceptCode">The Concept Code to validate.</param>
        public void ValidateUsingContextGroups(ConceptCode conceptCode)
        {
            //
            // Sanity checks.
            //

            if (conceptCode == null)
            {
                throw new ArgumentNullException("conceptCode");
            }


            //
            // Validate the Coding Scheme Designator on its own.
            //

            ValidateCodingSchemeDesignator(conceptCode);

 
            //
            // Validate the Code Value on its own.
            //

            ValidateCodeValue(conceptCode);


            //
            // Validate the Coding Scheme Designator and Code Value pair: check if this pair is present in any loaded Context Group.
            //

            string codingSchemeDesignator = conceptCode.CodingSchemeDesignator;
            string codeValue = conceptCode.CodeValue;

            IList<Specification.CodedConcept> matchingCodedConcepts = null;

            if ((codingSchemeDesignator != null) && (codeValue != null) && (codingSchemeDesignator != "") && (codeValue != ""))
            {
                matchingCodedConcepts = this.contextGroups.GetCodedConcepts(codingSchemeDesignator, codeValue);

                if (matchingCodedConcepts.Count == 0)
                {
                    conceptCode.ValidationResults.Add(new ValidationResult("For the Coding Scheme Designator and Code Value pair, no Coded Concept could be found in the loaded Context Groups."));
                }
            }

          
            //
            // Validate the Code Meaning.
            //

            if ((matchingCodedConcepts != null) && (matchingCodedConcepts.Count > 0))
            {
                ValidateCodeMeaning(matchingCodedConcepts, conceptCode);
            }
        }
    }
}
