using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace DefinitionFilesRenamer
{
    class Program
    {
        private const string extension = "*.def";
        private static string inputFolderPath = null;
        private static string outputFolderPath = null;
        private static string mappingFilePath = null;
        private static string ignoreFilePath = null;
        private static int counterTotalDefinitionFiles = 0;
        private static int counterProcessedDefinitionFiles = 0;
        private static int counterIgnoredDefinitionFiles = 0;
        private static int counterUnhandledDefinitionFiles = 0;
        private static StringDictionary mappings = new StringDictionary();
        private static StringCollection unhandledFiles = new StringCollection();
        private static StringCollection ignoredFiles = new StringCollection();

        static int Main(string[] args)
        {
            // Display help text if not enough arguments are given.
            if (args.Length != 4)
            {
                Console.WriteLine("Rename definition files based on the SOP Class UID found in the definition files");
                Console.WriteLine();
                Console.WriteLine("Usage: DefinitionFilesRenamer [Source Folder] [Destination Folder]");
                Console.WriteLine("  [Mappings File] [Ignore File]");
                Console.WriteLine();
                Console.WriteLine("- Source Folder contains all the definition files, filtering is done based on");
                Console.WriteLine("  the .def extension");
                Console.WriteLine("- Destination Folder will be used to copy and rename all the definition files");
                Console.WriteLine("  to based on the mappings file.");
                Console.WriteLine("- Mappings File contains the SOP Class UID and SOP Class Name.");
                Console.WriteLine("- Ignore File is used for ignoring files that are not present in the Mappings");
                Console.WriteLine("  File but do occur in the Source Folder.");
                Console.WriteLine();
                Console.WriteLine("Tool will report the amount of files found, copied and files that where not");
                Console.WriteLine("processed. Files in the ignore file will not be reported at completion.");
                Console.WriteLine();
                return 0;
            }

            // Retrieve command line arguments.
            inputFolderPath = Path.GetFullPath(args[0]);
            outputFolderPath = Path.GetFullPath(args[1]);
            mappingFilePath = Path.GetFullPath(args[2]);
            ignoreFilePath = Path.GetFullPath(args[3]);


            // Verify and check arguments.
            if (!Directory.Exists(inputFolderPath))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid Source Folder.");
                Console.WriteLine();
                return -1;
            }
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }
            if (!File.Exists(mappingFilePath))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid Mappings File.");
                Console.WriteLine();
                return -1;
            }
            if (!File.Exists(ignoreFilePath))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid Ignore File.");
                Console.WriteLine();
                return -1;
            }

            // Load mappings.
            try
            {
                int mappingLineCount = 0;
                foreach (string line in File.ReadAllLines(mappingFilePath))
                {
                    mappingLineCount++;
                    // Verify that this line contains an mappings entry.
                    if (line.Contains("|"))
                    {
                        // Format is SOP Class UID|SOP Class Name.
                        // mapping[0] will contain SOP Class UID, mapping[1] will contain SOP Class Name.
                        string[] mapping = line.Split('|');
                        mappings.Add(mapping[0], mapping[1]);
                    }
                    else
                    {
                        // Log unparsible lines.
                        Console.WriteLine(string.Format("Warning: Line {0} contains an incorrect value:", mappingLineCount));
                        Console.WriteLine(line);
                        Console.WriteLine();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error: Unable to load the mappings.");
                Console.WriteLine();
                return -1;
            }

            // Load ignores.
            try
            {
                ignoredFiles.AddRange(File.ReadAllLines(ignoreFilePath));
            }
            catch
            {
                Console.WriteLine("Error: Unable to load ignore file.");
                Console.WriteLine();
                return -1;
            }

            // Get all the Definition Files from the input folder.
            string[] definitionFiles = Directory.GetFiles(inputFolderPath, extension);
            counterTotalDefinitionFiles = definitionFiles.Length;

            foreach (string definitionFilename in definitionFiles)
            {
                // Read in the definition file.
                string[] definitionFile = File.ReadAllLines(definitionFilename);

                // Get the SOP Class UID of the definition file.
                string sopClassUID = getSOPClassUID(definitionFile);
                if (sopClassUID == null)
                {
                    Console.WriteLine(string.Format("Error: Unabled to find the SOP Class UID for the definition file: {0}", definitionFilename));
                    Console.WriteLine();
                    continue;
                }

                // Check if the file is on the ignore list.
                if (ignoredFiles.Contains(sopClassUID))
                {
                    // Increment the counter of definition files that were ignored.
                    counterIgnoredDefinitionFiles++;
                    continue;
                }

                // Locate an mapping entry.
                if (mappings.ContainsKey(sopClassUID))
                {
                    counterProcessedDefinitionFiles++;
                    // Create an output file.
                    string outputFilename = Path.Combine(outputFolderPath, mappings[sopClassUID]);
                    TextWriter textWriter = File.CreateText(outputFilename);

                    // Write new header for the file.
                    textWriter.WriteLine("#######################################################");
                    textWriter.WriteLine("# DVTk Defintion File created on {0} #", DateTime.Now.ToString());
                    textWriter.WriteLine("#######################################################");

                    // Write the rest of the original Definition File.
                    foreach (string line in definitionFile)
                    {
                        textWriter.WriteLine(line);
                    }

                    // Nicely flush the buffer and close the file.
                    textWriter.Flush();
                    textWriter.Close();
                }
                else
                {
                    unhandledFiles.Add(definitionFilename);
                    counterUnhandledDefinitionFiles++;
                }
            }

            // Report data.
            Console.WriteLine(string.Format("Definition Files: {0}", counterTotalDefinitionFiles));
            Console.WriteLine(string.Format("Files Processed:  {0}", counterProcessedDefinitionFiles));
            Console.WriteLine(string.Format("Files Ignored:    {0}", counterIgnoredDefinitionFiles));
            Console.WriteLine(string.Format("Files Unhandled:  {0}", counterUnhandledDefinitionFiles));
            foreach (string unhandled in unhandledFiles)
            {
                Console.WriteLine(string.Format("-{0}", unhandled));
            }
            Console.WriteLine();
            Console.WriteLine("Done.");
            Console.WriteLine();
            return 0;
        }

        /// <summary>
        ///     Looks for the SOP Class UID in a Definition File.
        /// </summary>
        /// <param name="definitionFile">
        ///     Contents of a definition file.
        /// </param>
        /// <returns>
        ///     The UID as string or null if no UID was found.
        /// </returns>
        private static string getSOPClassUID(string[] definitionFile)
        {
            foreach (string line in definitionFile)
            {
                if (line.StartsWith("SOPCLASS"))
                {
                    string sopClassUID = Regex.Matches(line, "\"(.*?)\"")[0].Value;
                    sopClassUID = sopClassUID.TrimStart('"');
                    sopClassUID = sopClassUID.TrimEnd('\"');
                    return sopClassUID;
                }
            }
            return null;
        }
    }
}
