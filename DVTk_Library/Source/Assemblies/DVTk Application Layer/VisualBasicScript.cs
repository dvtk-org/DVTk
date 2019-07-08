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
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;
using DvtkSession = Dvtk.Sessions;


namespace DvtkApplicationLayer
{
	/// <summary>
	/// Represents a Visual Basic Script.
	/// </summary>
	public class VisualBasicScript 
	{
		/// <summary>
		/// The full name of the file that has been included.
		/// </summary>
		private String includedFileFullName = "";

		/// <summary>
		/// Files that have already be included.
		/// </summary>
		private ArrayList alreadyIncludedFilesFullNames = new ArrayList();

		/// <summary>
		/// Filled by the callback method ReplaceFirstInclude(...).
		/// </summary>
		private String includeErrors;
		
		/// <summary>
		/// Filled by the callback method OnCompileError(...).
		/// </summary>
		private String compileErrors;

        private DvtkSession.ScriptSession scriptSession = null ;
        private String scriptFileName ;
        private string scriptFullFileName;
        private String scriptRootDirectory ;
				
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="session">The script Session the script belongs to.</param>
        /// <param name="fileName">The script file name (not including path).</param>
        public VisualBasicScript(DvtkSession.ScriptSession session, String fileName) {
            this.scriptSession = session ;
            this.scriptFileName = fileName ;
			this.scriptRootDirectory = scriptSession.DicomScriptRootDirectory ;
            this.scriptFullFileName = Path.Combine(scriptSession.DicomScriptRootDirectory , fileName) ;
        }

        /// <summary>
        /// Constructor.
        /// 
        /// Use this constructor when the script file is located in a directory, other then the
        /// Session script directory.
        /// </summary>
        /// <param name="session">The script Session the script belongs to.</param>
        /// <param name="directory">The directory of the script file.</param>
        /// <param name="scriptFileName">The script file name (not including path).</param>
        public VisualBasicScript(DvtkSession.ScriptSession  session, String directory, String scriptFileName) {
            this.scriptSession = session ;
			this.scriptFileName = scriptFileName ;
            if (directory == "") {
                this.scriptRootDirectory = scriptSession.DicomScriptRootDirectory ;                 
            }
            else {
                this.scriptRootDirectory = directory ;
            }
            this.scriptFullFileName = Path.Combine(this.scriptRootDirectory , scriptFileName) ;
        }


		/// <summary>
		/// Execute the Visual Basic Script if no include errors and no compile erors exist.
		/// If present, include errors and compile errors will be logged in the Results file(s).
		/// </summary>
		public void Execute()
		{
			String includeErrors = "";
			String compileErrors = "";

			// Start the results gathering the the Results file(s).
			scriptSession.StartResultsGatheringWithExpandedFileNaming(this.scriptFileName);

			// Get the script host that will do the actual execution of the Visual Basic Script.
			DvtkScriptSupport.DvtkScriptHost dvtkScriptHost = GetDvtkScriptHost();

			// Set the source code.
			dvtkScriptHost.SourceCode = GetExpandedContent(out includeErrors);

			// If include errors exist, log in the Results file(s) and stop execution.
			if (includeErrors.Length > 0)
			{
				scriptSession.WriteError(includeErrors);
			}
			else
				// If no include errors exist...
			{
				Compile(dvtkScriptHost, out compileErrors);

				// If compile errors exist, log in the Results file(s) and stop execution.
				if (compileErrors.Length > 0)
				{
					scriptSession.WriteError(compileErrors);
				}
					// If no compile error exist, execute the script.
				else
				{
					try
					{
						// Run the script using the script host.
						dvtkScriptHost.Invoke ("DvtkScript", "Main");
					}
                    catch (Exception theException)
					{
                        String invokeError = string.Format("An exception occured while executing Visual Basic Script \"{0}\":\r\n{1}", scriptFullFileName, theException.Message);
                        scriptSession.WriteError(invokeError);
                    }
				}
			}

			// End the results gathering the the Results file(s).
			scriptSession.EndResultsGathering();
		}

		/// <summary>
		/// Get a new DvtkScriptHost object and set all relevant properties.
		/// </summary>
		/// <returns>The new DvtkScriptHost object.</returns>
		private DvtkScriptSupport.DvtkScriptHost GetDvtkScriptHost()
		{
			DvtkScriptSupport.DvtkScriptHost dvtkScriptHost = null;
			
			Random randomValue = new Random();

			// Create a new script host in which the script will run.
			dvtkScriptHost = new DvtkScriptSupport.DvtkScriptHost(
				DvtkScriptSupport.DvtkScriptLanguage.VB,
				"dvtk://session/" + this.scriptFileName + randomValue.Next().ToString(), 
				Application.StartupPath);

			// Set the current active session in the script host
			dvtkScriptHost.Session = scriptSession;
			dvtkScriptHost.DvtkScriptHostScriptFullFileName = this.scriptFullFileName;

			return(dvtkScriptHost);
		}

		/// <summary>
		/// Get the content of a text file.
		/// </summary>
		/// <param name="fullFileName">The full file name of thetext file.</param>
		/// <returns>
		/// The content of the text file. 
		/// The empty string is returned if the text file does not exist.
		/// </returns>
		private String GetTextFileContent(String fullFileName)
		{
			System.IO.StreamReader streamReader;
			String textFileContent;

			FileInfo fileInfo = new FileInfo (fullFileName);

			if (!fileInfo.Exists)
			{
				// Text file does not exist.
				return "";
			}

			// Text file exists, read and return the content.
			streamReader = fileInfo.OpenText();
			textFileContent = streamReader.ReadToEnd();
			streamReader.Close();

			return textFileContent;
		}

		/// <summary>
		/// Compile the script to check if the syntax is OK.
		/// </summary>
		/// <param name="dvtkScriptHost">The dvtk script host.</param>
		/// <param name="compileErrors">If existing, the compile errors.</param>
		private void Compile(DvtkScriptSupport.DvtkScriptHost dvtkScriptHost, out String compileErrors)
		{
			this.compileErrors = "";

			// Add the compiler error event handler.
			DvtkScriptSupport.CompilerErrorEventHandler compilerErrorEventHandler =
				new DvtkScriptSupport.CompilerErrorEventHandler(OnCompilerError);
			dvtkScriptHost.CompilerErrorEvent += compilerErrorEventHandler;

			dvtkScriptHost.Compile();

			// Remove the compiler error event handler.
			dvtkScriptHost.CompilerErrorEvent -= compilerErrorEventHandler;

			// When a compile error exists, this has been stored in this.compileErrors by the callback mathod.
			compileErrors = this.compileErrors;
		}
	
		/// <summary>
		/// Expand and compile the script to check if the syntax is OK.
		/// This is a "standalone" method. Use this when no Session object is available.
		/// </summary>
		/// <param name="scriptFullFileName">The full script file name.</param>
		/// <param name="includeAndCompileErrors">String containing include and compile errors if existing.</param>
		/// <returns>Boolean indicating if the script is correct.</returns>
		public static bool ExpandAndCompile(String scriptFullFileName, out String includeAndCompileErrors)
		{
			includeAndCompileErrors = "";
			String includeErrors;
			String compileErrors;

			// Construct the following objects because this is a "standalone" method.
			DvtkSession.ScriptSession scriptSession = new Dvtk.Sessions.ScriptSession();
			VisualBasicScript visualBasicScript = new VisualBasicScript(scriptSession , Path.GetDirectoryName(scriptFullFileName), Path.GetFileName(scriptFullFileName));

			// Get the script host.
			DvtkScriptSupport.DvtkScriptHost dvtkScriptHost = visualBasicScript.GetDvtkScriptHost();

			// Set the source code.
			dvtkScriptHost.SourceCode = visualBasicScript.GetExpandedContent(out includeErrors);

			// Add expand results.
			includeAndCompileErrors+= includeErrors;

			// Compile.
			visualBasicScript.Compile(dvtkScriptHost, out compileErrors);

			// Add compile results.
			includeAndCompileErrors+= compileErrors;

			return(includeAndCompileErrors.Length == 0);
		}

		/// <summary>
		/// Called when a compiler error exists in a Visual Basic Script, when Compile(...) is called.
		/// </summary>
		/// <param name="sender">The sended.</param>
		/// <param name="e">Arguments.</param>
		private void OnCompilerError(object sender, DvtkScriptSupport.CompilerErrorEventArgs e)
		{	
			this.compileErrors = "\r\n";
			this.compileErrors+= "Compile error in \"" + scriptFullFileName + "\"\r\n";
			this.compileErrors+= String.Format("- Line number (in expanded Visual Basic Script): {0}\r\n", e.Line);
			this.compileErrors+= String.Format("- Line text: {0}", e.LineText);
			this.compileErrors+= String.Format("- Error description: {0}", e.Description);
		}

		/// <summary>
		/// Get the non expanded content (#include lines not replaced) of the Visual Basic Script.
		/// </summary>
		/// <returns>The content.</returns>
		public String GetContent()
		{
			return(GetTextFileContent(scriptFullFileName));
		}

		/// <summary>
		/// Get the expanded content (#include lines replaced with content of referred file) of the 
		/// Visual Basic Script.
		/// </summary>
		public String GetExpandedContent(out String includeErrors)
		{
			this.includeErrors = "";
			String ExpandedContent = "";
			this.alreadyIncludedFilesFullNames.Clear();

			// Get the original content (not expanded) of the Visual Basic Script.
			String content = GetContent();
	
			this.alreadyIncludedFilesFullNames.Add(scriptFullFileName);

			ExpandedContent = Expand(content);

			// When include errors are present, the method ReplaceFirstInclude(...) will 
			// have stored this in this.includeErrors;
			if (this.includeErrors.Length > 0)
			{
				includeErrors = "\r\nInclude error: unable to find file(s):\r\n" + this.includeErrors;
			}
			else
			{
				includeErrors = "";
			}
			

			return(ExpandedContent);
		}

		/// <summary>
		/// Expand the content by replacing all #include lines with content of referred file.
		/// This is done recursively.
		/// </summary>
		/// <param name="content">
		/// The content of the Visual Basic Script that needs to be expanded.
		/// </param>
		/// <returns>The expanded content of the Visual Basic Script.</returns>
		private String Expand(String content)
		{
			String expandedString = "";

			// Regular expression finds any #include "..." starting at the beginning of a line.
			String regexString = @"^#include";
			regexString+= @"\s*"; // Zero or more whitespace characters.
			regexString+= "\""; // The '"' character.
			regexString+= @"[\w\s_\.]*"; // Zero or more whitespace characters, alphanumeric characters, '.' and '_'.
			regexString+= "\""; // The '"' character.

			Regex regexInclude = new Regex(regexString, RegexOptions.Multiline);
			MatchEvaluator delegateReplaceFirstInclude = new MatchEvaluator(ReplaceFirstInclude);

			this.includedFileFullName = "";
			expandedString = regexInclude.Replace(content, delegateReplaceFirstInclude); 

			// If an #include has been replaced...
			if (this.includedFileFullName.Length > 0)
			{
				// Call the Expanded method recusively.
				expandedString = Expand(expandedString);
			}
			// If no #include has been found...
			else
			{
				expandedString = content;
			}

			return(expandedString);
		}

		/// <summary>
		/// Replace the first #include in a string by the content of the referred file.
		/// This method is called by the Regex.Replace method used.
		/// </summary>
		/// <param name="match">Matching string</param>
		/// <returns>String used to replace the matching string</returns>
		private string ReplaceFirstInclude(Match match)
		{
			String replaceString = match.Value;

			// If this is the first replacement of an #include...
			if (this.includedFileFullName.Length == 0)
			{
				int firstDoubleQuoteIndex = match.Value.IndexOf("\"");
				int secondDoubleQuoteIndex = match.Value.IndexOf("\"", firstDoubleQuoteIndex + 1);

				String includeFileName = match.Value.Substring(firstDoubleQuoteIndex + 1, secondDoubleQuoteIndex - firstDoubleQuoteIndex - 1);

				this.includedFileFullName = Path.Combine(scriptSession.DicomScriptRootDirectory, includeFileName);

				String doubleLine = "' ".PadRight(secondDoubleQuoteIndex + 12, '=') + "\r\n";
				String singleLine = "' ".PadRight(secondDoubleQuoteIndex + 12, '-') + "\r\n";

				// If include file does not exist...
				if (!File.Exists(this.includedFileFullName))
				{
					replaceString = "\r\n";
					replaceString+= doubleLine;
					replaceString+= "' [INCLUDE] " + match.Value + "\r\n";
					replaceString+= singleLine;
					replaceString+= "' [INCLUDE] ERROR: unable to find file \"" + this.includedFileFullName + "\"\r\n";
					replaceString+= doubleLine;
					replaceString+= "\r\n";

					this.includeErrors+= "- \"" + this.includedFileFullName + "\"\r\n";
				}
					// If include file exists...
				else
				{
					bool alreadyIncluded = false;

					foreach(String alreadyIncludedFileFullName in this.alreadyIncludedFilesFullNames)
					{
						if (this.includedFileFullName == alreadyIncludedFileFullName)
						{
							alreadyIncluded = true;
							break;
						}
					}

					// If this file has already been included, skip it.
					if (alreadyIncluded)
					{
						replaceString = "\r\n";
						replaceString+= doubleLine;
						replaceString+= "' [INCLUDE] " + match.Value + "\r\n";
						replaceString+= singleLine;
						replaceString+= "' [INCLUDE] INFO: skipping already included file \"" + this.includedFileFullName + "\"\r\n";
						replaceString+= doubleLine;
						replaceString+= "\r\n";
					}
						// If this file has not already been included, insert it.
					else
					{
						replaceString = "\r\n";
						replaceString+= doubleLine;
						replaceString+= "' [INCLUDE] " + match.Value + "\r\n";
						replaceString+= singleLine;
						replaceString+= "' [INCLUDE] INFO: begin inserting file \"" + this.includedFileFullName + "\"\r\n";
						replaceString+= doubleLine;
						replaceString+= "'\r\n";
						replaceString+= "'\r\n";
						replaceString+= "\r\n";

						replaceString+= GetTextFileContent(this.includedFileFullName) + "\r\n\r\n";

						replaceString+= "\r\n";
						replaceString+= "'\r\n";
						replaceString+= "'\r\n";
						replaceString+= "' [INCLUDE] " + match.Value + "\r\n";
						replaceString+= singleLine;
						replaceString+= "' [INCLUDE] INFO: end inserting file \"" + this.includedFileFullName + "\"\r\n";
						replaceString+= doubleLine;
						replaceString+= "\r\n";

						this.alreadyIncludedFilesFullNames.Add(this.includedFileFullName);
					}
				}
			}

			return(replaceString);
		}

		/// <summary>
		/// View the expanded content with notepad.
		/// </summary>
		public void ViewExpanded()
		{
			// Write the expanded content to a temporary file.
			String includeErrors;
			String expandedContent = GetExpandedContent(out includeErrors);

			String tempFileName = Path.GetTempFileName() + " expanded " + this.scriptFileName;

			FileStream fileStream = new FileStream(tempFileName, FileMode.Create, FileAccess.Write);
			StreamWriter fileWriter = new StreamWriter(fileStream);
			try
			{
				fileWriter.Write(expandedContent);
			}
			catch(IOException ioException)
			{
				Debug.Assert(false, ioException.ToString());
			}
			finally
			{
				fileWriter.Close();
			}

			// Make the temp file read-only. In this way, when the user tries to save
			// a changed expanded file with notepad, he will get a read-only warning and
			// this will probably make him aware not to change it this way.
			try
			{
				File.SetAttributes(tempFileName, File.GetAttributes(tempFileName) | FileAttributes.ReadOnly);
			}
			catch
			{
				// Do nothing.
			}

			System.Diagnostics.Process theProcess  = new System.Diagnostics.Process();

			theProcess.StartInfo.FileName= "Notepad.exe";
			theProcess.StartInfo.Arguments = tempFileName;

			theProcess.Start();
		}
	}
}
