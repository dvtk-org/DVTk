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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SetVersion
{
	/// <summary>
	/// The application sets the correct version number in all available AssemblyInfo.cs files.
	/// </summary>
	class SetVersion
	{
		private String version = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine("Usage: SetVersion <Directory relative to path of this executable> <Version>");
			}
			else
			{
				SetVersion setVersion = new SetVersion(args[1]);

				setVersion.SetInDirectory(Path.Combine(Application.StartupPath, args[0]));		
			}

		}

		public SetVersion(String version)
		{
			this.version = version;
		}

		void SetInFile(String fullFileName)
		{
			// Read the content of the file.
			System.IO.StreamReader streamReader;
			String textFileContent;

			FileInfo fileInfo = new FileInfo(fullFileName);

			streamReader = fileInfo.OpenText();
			textFileContent = streamReader.ReadToEnd();
			streamReader.Close();

			// Replace the old version with the new version.
			String regexString = @"\[assembly:[\s]*AssemblyVersion";
			regexString+= @"[Attribute]*";
			regexString+= @"\("; // The '(' character.
			regexString+= "\""; // The '"' character.
			regexString+= @"[\s\w\.\*]+"; // The old version.
			regexString+= "\""; // The '"' character.
			regexString+= @"\)\]"; // The ')' and ']' character.

			Regex regexInclude = new Regex(regexString, RegexOptions.Multiline);
			MatchEvaluator replaceVersionHandler = new MatchEvaluator(this.replaceVersion);

			String newTextFileContent = regexInclude.Replace(textFileContent, replaceVersionHandler); 

			// Write the updated content back to file.
			using (StreamWriter sw = new StreamWriter(fullFileName)) 
			{
				sw.Write(newTextFileContent);
			}

			Console.WriteLine("Version changed in file " + fullFileName);
		}

		private string replaceVersion(Match match)
		{
			return("[assembly: AssemblyVersion(\"" + this.version + "\")]");
		}

		private void SetInDirectory(String fullDirectoryName)
		{
			String[] assemblyInfoFiles = Directory.GetFiles(fullDirectoryName, "AssemblyInfo.c*");

			foreach(String assemblyInfoFile in assemblyInfoFiles)
			{
				SetInFile(assemblyInfoFile);
			}
			
			String[] directories = Directory.GetDirectories(fullDirectoryName);

			foreach(String directory in directories)
			{
				SetInDirectory(Path.Combine(fullDirectoryName, directory));
			}
		}
	}
}
