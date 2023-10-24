using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dvtk.Dicom.StructuredReporting.Specification
{
    /// <summary>
    /// A collection of loaded context groups.
    /// </summary>
    public class ContextGroups
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Dictionary to able to look up Coded Concepts, using as key a Coding Scheme Designator and Code Value pair, very fast.
        /// </summary>
        private Dictionary<CodingSchemeDesignatorCodeValuePair, List<CodedConcept>> codedConceptDictionary = new Dictionary<CodingSchemeDesignatorCodeValuePair, List<CodedConcept>>();

        /// <summary>
        /// Dictionary to able to look up a Context Group, using as key the the CID , very fast.
        /// </summary>
        private Dictionary<String, ContextGroup> contextGroupDictionary = new Dictionary<string,ContextGroup>();



        //
        // - Constructors -
        //

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ContextGroups()
        {
            // Do nothing.
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Add a Context Group this this instance.
        /// </summary>
        /// <param name="contextGroup">The Context Group to add.</param>
        private void AddCodedConceptsToDictionary(ContextGroup contextGroup)
        {
            foreach (CodedConceptOrInclude codedConceptOrInclude in contextGroup.CodedConceptsOrIncludes)
            {
                if (codedConceptOrInclude is CodedConcept)
                {
                    CodedConcept codedConcept = codedConceptOrInclude as CodedConcept;

                    CodingSchemeDesignatorCodeValuePair codingSchemeDesignatorCodeValuePair = new CodingSchemeDesignatorCodeValuePair(codedConcept.CodingSchemeDesignator, codedConcept.CodeValue);

                    List<CodedConcept> codedConcepts = null;

                    if (this.codedConceptDictionary.TryGetValue(codingSchemeDesignatorCodeValuePair, out codedConcepts))
                    {
                        codedConcepts.Add(codedConcept);
                    }
                    else
                    {
                        codedConcepts = new List<CodedConcept>();
                        codedConcepts.Add(codedConcept);
                        this.codedConceptDictionary.Add(codingSchemeDesignatorCodeValuePair, codedConcepts);
                    }
                }
            }
        }

        /// <summary>
        /// Get a list of all Coded Concepts from all loaded Context Groups that have the supplied
        /// Coding Scheme Designator and Code Value.
        /// </summary>
        /// <remarks>
        /// A dictionary is used internally for this purpose, so this method call should be fast.
        /// </remarks>
        /// <param name="codingSchemeDesignator">The Coding Scheme Designator.</param>
        /// <param name="codeValue">The Code Value.</param>
        /// <returns>A (possible empty) list of Coded Concepts.</returns>
        public IList<CodedConcept> GetCodedConcepts(string codingSchemeDesignator, string codeValue)
        {
            List<CodedConcept> codedConcepts = null;

            CodingSchemeDesignatorCodeValuePair codingSchemeDesignatorCodeValuePair = new CodingSchemeDesignatorCodeValuePair(codingSchemeDesignator, codeValue);

            if (!this.codedConceptDictionary.TryGetValue(codingSchemeDesignatorCodeValuePair, out codedConcepts))
            // Key not found and codedConcepts contains null;
            {
                codedConcepts = new List<CodedConcept>();
            }

            return (codedConcepts);
        }

        /// <summary>
        /// Get a list of all Coded Concepts from the Context Group specified by the supplied CID
        /// that have the supplied Coding Scheme Designator and Code Value.
        /// </summary>
        /// <param name="cid">The Context ID.</param>
        /// <param name="codingSchemeDesignator">The Coding Scheme Designator.</param>
        /// <param name="codeValue">The Code Value.</param>
        /// <returns>
        /// Null of the Content Group specified by the supplied CID does not exist.
        /// Otherwise a list of Coded Concepts.
        /// </returns>
        public IList<CodedConcept> GetCodedConcepts(string cid, string codingSchemeDesignator, string codeValue)
        {
            List<CodedConcept> codedConcepts = null;

            ContextGroup contextGroup = null;

            if (!this.contextGroupDictionary.TryGetValue(cid, out contextGroup))
            // Key not found and contextGroup contains null;
            {
                codedConcepts = null;
            }
            else
            {
                codedConcepts = new List<CodedConcept>();

                foreach (CodedConceptOrInclude codedConceptOrInclude in contextGroup.CodedConceptsOrIncludes)
                {
                    if (codedConceptOrInclude is CodedConcept)
                    {
                        CodedConcept codedConcept = codedConceptOrInclude as CodedConcept;

                        if ((codingSchemeDesignator == codedConcept.CodingSchemeDesignator) && (codeValue == codedConcept.CodeValue))
                        {
                            codedConcepts.Add(codedConcept);
                        }
                    }
                    else if (codedConceptOrInclude is IncludedContextGroup)
                    {
                        IncludedContextGroup includedContextGroup = codedConceptOrInclude as IncludedContextGroup;

                        IList<CodedConcept> matchingCodedConceptsFromIncludedContentGroup = GetCodedConcepts(includedContextGroup.Id, codingSchemeDesignator, codeValue);

                        if (matchingCodedConceptsFromIncludedContentGroup != null)
                        {
                            codedConcepts.AddRange(matchingCodedConceptsFromIncludedContentGroup);
                        }
                    }
                }
            }

            return (codedConcepts);
        }

        /// <summary>
        /// Load Context Group instances from xml files.
        /// </summary>
        /// <param name="path">The path to the xml files.</param>
        /// <param name="searchPattern">
        /// The search pattern for the file names to use to determine which xml files to load.
        /// </param>
        public void LoadContextGroupsFromXml(String path, String searchPattern)
        {
            String[] fullFileNames = Directory.GetFiles(path, searchPattern);

            foreach (String fullFileName in fullFileNames)
            {
                try
                {
                    ContextGroup contextGroup = ContextGroup.LoadInstanceFromXml(fullFileName);


                    //
                    // Add this Context Group to the contextGroupDictionary.
                    //

                    this.contextGroupDictionary.Add(contextGroup.Id, contextGroup);


                    //
                    // Add all Coded Concepts in the Context Group to the codedConceptDictionary.
                    //

                    AddCodedConceptsToDictionary(contextGroup);
                }
                catch
                {
                    // TODO: log this somehow.
                }
            }
        }

        /// <summary>
        /// Creates a string dump from this instance.
        /// </summary>
        /// <param name="prefix">Will be added to the beginning of each line.</param>
        /// <returns>The string dump.</returns>
        public String StringDump(String prefix)
        {
            StringBuilder stringDump = new StringBuilder();

            foreach (KeyValuePair<String, ContextGroup> keyValuePair in this.contextGroupDictionary)
            {
                if (stringDump.Length > 0)
                {
                    stringDump.Append("\r\n\r\n");
                }

                stringDump.Append(ContextGroupStringDump(prefix, keyValuePair.Value));
            }

            return (stringDump.ToString());

        }

        /// <summary>
        /// Creates a string dump for a single Context Group.
        /// </summary>
        /// <param name="prefix">The prefix to use in the string dump.</param>
        /// <param name="contextGroup">The Context Group.</param>
        /// <returns>The string dump.</returns>
        private static String ContextGroupStringDump(String prefix, ContextGroup contextGroup)
        {
            StringBuilder stringDump = new StringBuilder();

            stringDump.Append(prefix + "Context ID \"" + contextGroup.Id + "\", Context Group Name \"" + contextGroup.Name + "\"\r\n");
            stringDump.Append(prefix + "  CODING SCHEME DESIGNATOR, CODE VALUE, CODE MEANING\r\n");
            stringDump.Append(prefix + "  ------------------------  ----------  ------------");


            foreach (CodedConceptOrInclude codedConceptOrInclude in contextGroup.CodedConceptsOrIncludes)
            {
                if (codedConceptOrInclude is CodedConcept)
                {
                    CodedConcept codedConcept = codedConceptOrInclude as CodedConcept;
                    stringDump.Append("\r\n" + prefix + "  " + codedConcept.CodingSchemeDesignator);

                    if (codedConcept.CodingSchemeVersion != null)
                    {
                        stringDump.Append(" [" + codedConcept.CodingSchemeVersion + "]");
                    }

                    stringDump.Append(", " + codedConcept.CodeValue + ", \"" + codedConcept.CodeMeaning + "\"");
                }
                else if (codedConceptOrInclude is IncludedContextGroup)
                {
                    IncludedContextGroup includedContextGroup = codedConceptOrInclude as IncludedContextGroup;

                    stringDump.Append("\r\n" + prefix + "  Include CONTEXT ID " + includedContextGroup.Id);
                }
                else
                {
                    throw new Exception("Not supposed to get here.");
                }
            }

            return (stringDump.ToString());
        }
    }
}
