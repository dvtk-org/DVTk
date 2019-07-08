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
using System.Threading;
using DvtkApplicationLayer;

namespace DvtCmd
{
	/// <summary>
	/// Application class for this commandline executable.
	/// </summary>
	class DvtCmd
	{
		private static string[] _MainArgs;
		private static bool _OptionCompileOnly = false;
		private static bool _OptionValidateMediaFile = false;
		private static bool _OptionValidateMediaDirectory = false;
		private static bool _OptionEmulatorStorageSCP = false;
		private static bool _OptionEmulatorStorageSCU = false;
		private static bool _OptionEmulatorPrintSCP = false;
		private static bool _OptionVerbose = false;
		private static bool _OptionHelp = false;
		private static ArrayList _NonOptions = new ArrayList();
		private static DvtkApplicationLayer.EmulatorSession SCPEmulator = null;
		private static int _exitCode = 0;


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] arguments)
		{
#if !DEBUG
			try
			{
#endif
			_MainArgs = arguments;

			Console.WriteLine();
			Console.WriteLine("> Executing DvtCmd");
			
			//
			// Initialise dvtk library.
			//
			Dvtk.Setup.Initialize();

			//
			// Determine the options and non-options parameters.
			//
			for (int index = 0; index < arguments.Length; index++)
			{
				string argument = (string)arguments[index];
				//
				// Each option starts with '-'.
				// For instance;
				// '-c' <=> Compile Only option.
				//
				if (argument.StartsWith("-"))
				{
					string optionsString = argument.TrimStart('-');
					optionsString = optionsString.ToLower();

					if (optionsString == "c")
					{
						_OptionCompileOnly = true;
					}

					if (optionsString == "v")
					{
						_OptionVerbose = true;
					}

					if (optionsString == "m")
					{
						_OptionValidateMediaFile = true;
					}

					if (optionsString == "d")
					{
						_OptionValidateMediaDirectory = true;
					}

					if (optionsString == "estscp")
					{
						_OptionEmulatorStorageSCP = true;
					}

					if (optionsString == "estscu")
					{
						_OptionEmulatorStorageSCU = true;
					}

					if (optionsString == "eprscp")
					{
						_OptionEmulatorPrintSCP = true;
					}

					if (optionsString == "h")
					{
						_OptionHelp = true;
					}
				}
				else
				{
					_NonOptions.Add(argument);
				}
			}

			///
			/// Do the actual thing.
			///
			if (_OptionHelp)
			{
				ShowCommandLineArguments();
			}
			else if (_OptionCompileOnly)
			{
				if (_NonOptions.Count == 1)
				{
					String scriptFullFileName = (string)_NonOptions[0];

					// If this is a Visual Basic Script...
					if (Path.GetExtension(scriptFullFileName).ToLower() == ".vbs")
					{
						// Compile the Visual Basic Script and display progress and errors.
						ExpandAndCompileVisualBasicScript(scriptFullFileName);
					}
					else
					{
						ShowCommandLineArguments();
					}
				}
			}
			else if (_OptionValidateMediaDirectory)
			{
				if (_NonOptions.Count == 2)
				{
					ValidateMediaDirectory();
				}
				else
				{
					ShowCommandLineArguments();
				}
				
			}
			else if (_OptionValidateMediaFile)
			{
				if (_NonOptions.Count == 2)

				{
					ValidateMediaDirectory();
				}
				else
				{
					ShowCommandLineArguments();
				}
			}
			else if (_OptionEmulatorStorageSCP)
			{
				if (_NonOptions.Count == 1) 
				{
					FileInfo firstArg = new FileInfo((string)_NonOptions[0]);
					if (!firstArg.Exists)
					{
						Console.WriteLine("Error : Session File does not exists");
					}
					else 
					{
						EmulateSCP();
					}
				}
				else 
				{
					ShowCommandLineArguments();
				}
			}
			else if (_OptionEmulatorStorageSCU)
			{
				if (_NonOptions.Count >= 2)
				{
					EmulateStorageSCU();
				}
				else
				{
					ShowCommandLineArguments();
				}
			}
			else if (_OptionEmulatorPrintSCP)
			{
				if (_NonOptions.Count == 3)
				{
					FileInfo firstArg = new FileInfo((string)_NonOptions[0]);
					if (!firstArg.Exists)
					{
						Console.WriteLine("Error : Session File does not exists");
					}
					else 
					{
						EmulateSCP();
					}
				}
				else
				{
					ShowCommandLineArguments();
				}
			}
			else
				// Execute script.
			{
				if (_NonOptions.Count == 2)
				{
					ExecuteScript();
				}
				else
				{
					ShowCommandLineArguments();
					
				}
			}

			// Don't show exceptions anymore when cleaning up resources from this application.
//			CustomExceptionHandler eh = new CustomExceptionHandler();
//			eh.ShowExceptions = false;
//			System.AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(eh.OnAppDomainUnhandledException);
#if !DEBUG
			}
			catch(Exception exception)
			{
				if (exception is System.NullReferenceException)
				{
					// Do nothing.
				}
				else
				{
					CustomExceptionHandler.ShowThreadExceptionDialog(exception);
				}
			}
			finally
			{
				// Clean up
				if( SCPEmulator != null)
				{
					SCPEmulator = null;
				}

				//
				// Terminate the setup
				//
				Dvtk.Setup.Terminate();
			}
#endif
			Console.WriteLine("Exit Code: {0}", _exitCode);
			return _exitCode;
		}

        private static void ExecuteScript() 
		{
            ScriptSession scriptSession = new ScriptSession();
            scriptSession.SessionFileName = (string)_NonOptions[0];
			scriptSession.OptionVerbose = _OptionVerbose;

			ScriptInput scriptInput = new ScriptInput();
			scriptInput.FileName = (string)_NonOptions[1];
			scriptInput.Arguments = _MainArgs;

			if((scriptSession.SessionFileName == "") || (scriptSession.SessionFileName == ""))
			{
				Console.WriteLine("Warning : Provide proper arguments.\n");
				return;
			}            
            
            string scriptFullFileName = "";  
            if (Path.GetDirectoryName(scriptInput.FileName )!= "") 
			{
                    scriptFullFileName = scriptInput.FileName;
            }
            else 
			{
                scriptFullFileName = Path.Combine(scriptSession.DicomScriptRootDirectory ,scriptInput.FileName);
            }
            
            FileInfo fileInfoFirstArg = new FileInfo(scriptSession.SessionFileName);
            if(!fileInfoFirstArg.Exists) 
			{
                    Console.WriteLine("Error : Session File does not exists.\n");
				return;
            }
            else 
			{ 
                string fileExtension = Path.GetExtension(scriptInput.FileName);
                FileInfo fileInfoSecondArg = new FileInfo(scriptFullFileName);
                if (((fileExtension == ".ds") ||(fileExtension == ".dss")||(fileExtension == ".vbs")||(fileExtension == ".vb")) && (fileInfoSecondArg.Exists))
				{
                    Console.WriteLine("> Executing  Script {0}...", scriptFullFileName);
                    scriptSession.Execute(scriptInput);
                    if (scriptSession.Result) 
					{
                        Console.WriteLine("> Execution succeeded.\n");
                        DisplayResultCounters(scriptSession);
                    }
                    else {
                        Console.WriteLine("> Execution failed.\n");
                        DisplayResultCounters(scriptSession);
                    }
					DetermineExitCode(scriptSession);
                }
				else if (((fileExtension == ".ds") ||(fileExtension == ".dss")||(fileExtension == ".vbs")||(fileExtension == ".vb")) && (!fileInfoSecondArg.Exists)) 
				{
					Console.WriteLine("Error : Script File does not exists.\n");
				}
                else 
				{
					Console.WriteLine("Error : Script File does not exists.\n");
                    ShowCommandLineArguments();
                }
            }
        }

		private static void ValidateMediaDirectory()
		{
			MediaSession mediaSession = new MediaSession();
			mediaSession.OptionVerbose = _OptionVerbose;
			mediaSession.SessionFileName = (string)_NonOptions[0];
			FileInfo sessionFileName = null;
			ArrayList allDCMFilesTemp = new ArrayList();
			FileInfo mediaFileInfo = null;
			if(mediaSession.SessionFileName == "")
			{
				Console.WriteLine("Warning : Provide proper arguments.\n");
				return;
			}
			else
			{
				sessionFileName = new FileInfo(mediaSession.SessionFileName);
			}

			if (!sessionFileName.Exists)
			{
				Console.WriteLine(" Error : Session File  does not exists.\n");
			}
			else 
			{
           
				MediaInput mediaInput = new MediaInput();
				string mediaFile = (string)_NonOptions[1];
				mediaFileInfo = new FileInfo(mediaFile);


				if(mediaFile == "")
				{
					Console.WriteLine("Warning : Provide proper arguments.\n");
					return;
				}
				else if (mediaFileInfo.Exists)
				{
					allDCMFilesTemp.Add (mediaFileInfo.FullName);
					mediaInput.FileNames = allDCMFilesTemp;
					Console.WriteLine();
					Console.WriteLine("> Validating media file {0}...", mediaInput.FileNames[0]);
					mediaSession.Execute(mediaInput);
					DisplayResultCounters(mediaSession);
					DetermineExitCode(mediaSession);
				}
				else
				{
					DirectoryInfo theDirectoryInfo = new DirectoryInfo(mediaFile);
					if (!theDirectoryInfo.Exists)
					{
						Console.WriteLine("Error : Directory or File mentioned does not exists.\n");
					}
					else 
					{
					
						mediaInput.FileNames = GetFilesRecursively(theDirectoryInfo);
						Console.WriteLine();
						Console.WriteLine("> Validating media files ...");
						_exitCode = mediaSession.ExecuteDir(mediaInput);
						//DisplayResultCounters(mediaSession);
						//DetermineExitCode(mediaSession);
					}
				}				
			}
		}

		private static ArrayList GetFilesRecursively(DirectoryInfo directory) {
			// Get all the subdirectories
			FileSystemInfo[] infos = directory.GetFileSystemInfos();
					
			ArrayList allDCMFilesTemp = new ArrayList();
			foreach (FileSystemInfo f in infos)
			{
				if (f is FileInfo) 
				{
					if ((f.Extension.ToLower() == ".dcm")||(f.Extension == null) || (f.Extension == ""))
					{
						allDCMFilesTemp.Add (f.FullName);
					}
				} 
				else 
				{
					allDCMFilesTemp.AddRange(GetFilesRecursively((DirectoryInfo)f));
				}
			}
			return allDCMFilesTemp;
		}

        private static void ValidateMediaFile() {	
            MediaSession mediaSession = new MediaSession();
            mediaSession.OptionVerbose = _OptionVerbose;
            mediaSession.SessionFileName = (string)_NonOptions[0];
			FileInfo sessionFileName = null;
			if(mediaSession.SessionFileName == "")
			{
				Console.WriteLine("Warning : Provide proper arguments.\n");
				return;
			}
			else
			{
				sessionFileName = new FileInfo(mediaSession.SessionFileName);
			}

            if (!sessionFileName.Exists){
                Console.WriteLine(" Error : Session File  does not exists.\n");
            }else {
           
                MediaInput mediaInput = new MediaInput();
				string mediaFile = (string)_NonOptions[1];
				FileInfo mediaFileInfo = null;
				if(mediaFile == "")
				{
					Console.WriteLine("Warning : Provide proper arguments.\n");
					return;
				}
				else
				{
					mediaFileInfo = new FileInfo(mediaFile);
				}
                if (!mediaFileInfo.Exists){
                    Console.WriteLine("Error : Media File does not exists.\n");
                }else {
                    mediaInput.FileNames.Add(mediaFile);
                    Console.WriteLine();
                    Console.WriteLine("> Validating media file {0}...", mediaInput.FileNames[0]);
                    mediaSession.Execute(mediaInput);
                    DisplayResultCounters(mediaSession);
					DetermineExitCode(mediaSession);
                }
            }
        }

		private static void EmulateSCP()
		{
			try
			{
				// Create the thread object and execute the Storage SCP Emulator in the thread.
				Thread workerThread = new Thread(new ThreadStart(ExecuteSCP));
				Console.WriteLine();
				Console.WriteLine("> Running SCP Emulator");

				// Start the thread.
				workerThread.Start();

				// Send the Printer Status if Print SCP is running 
				if (_OptionEmulatorPrintSCP)
				{
					Thread.Sleep(1000);
					string printerStatusString = (string)_NonOptions[1];
					string printerStatusInfo = (string)_NonOptions[2];
					DvtkApplicationLayer.EmulatorSession.PrinterStatus printerStatus = DvtkApplicationLayer.EmulatorSession.PrinterStatus.FAILURE;
					if(printerStatusString == "")
					{
						printerStatus = DvtkApplicationLayer.EmulatorSession.PrinterStatus.FAILURE;
					}
					if (System.String.Compare(printerStatusString.ToUpper(), "NORMAL") == 0)
					{
						printerStatus = DvtkApplicationLayer.EmulatorSession.PrinterStatus.NORMAL;
					}
					else if (System.String.Compare(printerStatusString.ToUpper(), "WARNING") == 0) 
					{
						printerStatus = DvtkApplicationLayer.EmulatorSession.PrinterStatus.WARNING;
					}
					else
					{
						printerStatus = DvtkApplicationLayer.EmulatorSession.PrinterStatus.FAILURE;
					}
					if(SCPEmulator != null)
					{
						SCPEmulator.ApplyStatus (printerStatus, printerStatusInfo, true);
					}
				}

				Console.WriteLine("\n> Press ENTER to Stop the SCP Emulator");
				Console.ReadLine();

				// Abort the thread.
				workerThread.Abort();
			}
			catch(System.Exception theException)
			{
				throw(theException);
			}
			finally
			{
				SCPEmulator.StopingEmulator();
                DisplayResultCounters(SCPEmulator);
				DetermineExitCode(SCPEmulator);
			}
		}

        private static void EmulateStorageSCU() 
		{
            EmulatorSession emulatorSession = new EmulatorSession();
            emulatorSession.OptionVerbose = _OptionVerbose;
			string sessionFileName = (string)_NonOptions[0];
			FileInfo sessionFileInfo = null;
			if(sessionFileName == "")
			{
				Console.WriteLine("Warning : Provide proper arguments.\n");
				return;
			}
			else
			{
				sessionFileInfo = new FileInfo(sessionFileName);
			}

            if (!sessionFileInfo.Exists)
			{
                Console.WriteLine("Error : Session File does not exists.\n");
            }
			else 
			{
                emulatorSession.SessionFileName = sessionFileName;
                
                emulatorSession.scuEmulatorType = DvtkApplicationLayer.EmulatorSession.ScuEmulatorType.Storage;
                EmulatorInput emulatorInput = new EmulatorInput();
                //
                // Determine all media file names by parsing the string.
                //
                string mediaFiles = (string)_NonOptions[1];
				string[] mediaFileNames = null;
				if(mediaFiles != "")
				{
					mediaFileNames = mediaFiles.Split(new char[] {','});
				}
				else
				{
					Console.WriteLine("Warning : Provide proper arguments.\n");
					return;
				}

                //
                // Trim whitespaces from both ends of the Media File Names
                //
                for (int i = 0; i < mediaFileNames.Length; i++)
				{
                    emulatorInput.FileNames.Add(mediaFileNames[i].Trim());
				}

                //
                // Determine Association option.
                //
				if(_NonOptions.Count > 2) 
				{
					string modeOfAssoc = (string)_NonOptions[2];
					if((modeOfAssoc.ToLower() == "true"))
						emulatorInput.ModeOfAssociation = true;
                    else if ((modeOfAssoc.ToLower() == "false"))
						emulatorInput.ModeOfAssociation = false;
					else
					{
						Console.WriteLine("Warning : Provide true or false.\n");
						return;
					}
				}

                //
                // Determine Validate on import option.
                //
                if(_NonOptions.Count > 3) 
				{
					string validateOnImport = (string)_NonOptions[3];
                    if ((validateOnImport.ToLower() == "true"))
						emulatorInput.ValidateOnImport = true;
                    else if ((validateOnImport.ToLower() == "false"))
						emulatorInput.ValidateOnImport = false;
					else
					{
						Console.WriteLine("Warning : Provide true or false.\n");
						return;
					}
                }

                //
                // Determine send data under new study option.
                //
                if(_NonOptions.Count > 4) 
				{
					string dataUnderNewStudy = (string)_NonOptions[4];
                    if ((dataUnderNewStudy.ToLower() == "true"))
						emulatorInput.DataUnderNewStudy = true;
                    else if ((dataUnderNewStudy.ToLower() == "false"))
						emulatorInput.DataUnderNewStudy = false;
					else
					{
						Console.WriteLine("Warning : Provide true or false.\n");
						return;
					}
                }

                //
                // Determine Nr. of Repetiitons.
                //
                if(_NonOptions.Count > 5) 
				{
					string nrOfRepetitions = (string)_NonOptions[5];
                    emulatorInput.NosOfRepetitions = UInt16.Parse(nrOfRepetitions);
                }
                emulatorSession.Execute(emulatorInput);
				Thread.Sleep(500);
                if (emulatorSession.Result) 
				{
                    Console.WriteLine("> Started Storage SCU Emulator successfully.");
					Console.WriteLine("> Sent the Data through Storage SCU Emulator successfully.");
                    DisplayResultCounters(emulatorSession);
					DetermineExitCode(emulatorSession);
                }
                else 
				{
                    Console.WriteLine("> Error in starting Storage SCU Emulator.");
                }                
            }
        }

        private static void ExecuteSCP()
		{
            EmulatorSession emulatorSession = new EmulatorSession();
            emulatorSession.OptionVerbose = _OptionVerbose;
            
			string sessionFileName = (string)_NonOptions[0];
			if(sessionFileName == "")
			{
				Console.WriteLine("Warning : Provide proper arguments.\n");
				return;
			}
			else
			{
				emulatorSession.SessionFileName = sessionFileName;
			}

            SCPEmulator = emulatorSession;

            emulatorSession.scpEmulatorType = DvtkApplicationLayer.EmulatorSession.ScpEmulatorType.Storage;
            if (_OptionEmulatorPrintSCP) 
			{
                emulatorSession.scpEmulatorType = DvtkApplicationLayer.EmulatorSession.ScpEmulatorType.Printing;
            }
            EmulatorInput emulatorInput = new EmulatorInput();
            emulatorSession.Execute(emulatorInput);
        }        
        
		/// <summary>
		/// Expand and compile a Visual Basic script to check for errors.
		/// 
		/// Show a popup dialog when errors exist.
		/// </summary>
		/// <param name="fullScriptFileName">The full script file name.</param>
		private static void ExpandAndCompileVisualBasicScript(string fullScriptFileName)
        {
			String expandAndCompileErrors;

			Console.WriteLine();
			Console.WriteLine("> Compiling {0}...", fullScriptFileName);
			
			// If no expand and compile errors exist...
			if (DvtkApplicationLayer.VisualBasicScript.ExpandAndCompile(fullScriptFileName, out expandAndCompileErrors))
			{
				Console.WriteLine("Script contains no errors.\n");
			}
				// If expand and compile errors exist...
			else
			{
				Console.WriteLine(expandAndCompileErrors);

				// Show a popup dialog.
				String caption = String.Format("Compile error(s) in \"{0}\"", fullScriptFileName);
				MessageBox.Show(expandAndCompileErrors, caption);
			}
        }

        private const string commandLineInfo =
@"
Usage:
  DvtCmd [-v]
         <Full session file name>
         <Script file name (session scriptdirectory used when file name only)>
  Script filename must end with extension .dss/.ds or .vbs

  or

  DvtCmd -c 
         <Full script file name>
  Script file name must end with extension .vbs

  or

  DvtCmd -m 
         <Full session file name>
         <Full media file name>

  or

  DvtCmd -d 
         <Full session file name>
         <Full media directory path>

  or

  DvtCmd -estscp 
         <Full session file name>

  or

  DvtCmd -estscu 
         <Full session file name>
		 <List of full media file names>
		 <Multiple/Single association option>
		 <Validate on Import option>
		 <Send data under new study option>
		 <Nr. of Repetitions>
		 
  Media files can also contain DICOMDIR.
  The third, fourth, fifth and sixth parameters are optional.
  The value of the third, fourth and fifth parameters should 
  be provided true/false

  or

  DvtCmd -eprscp 
         <Full session file name>
		 <Printer status>
		 <Printer status info>

 The Printer status string should have one of the following values:
 NORMAL,WARNING,FAILURE.
 The Printer status info string can have any string value.

Options:
  -c: Only Expand and Compile, No Execute
  -m: Validate media file
  -d: Validate media directory
  -estscp: Run SCP Storage emulator
  -estscu: Execute SCU Storage emulator
  -eprscp: Run SCP Printer emulator
  -v: Verbose
  -h: Help

The VBS scipts use entry-point Sub: DvtCmdMain in Module: DvtkScript

  DvtCmd returns an exit code based on the validation errors and warnings:
  0  = no errors / no warnings
  -1 = no errors / some warnings
  -2 = some errors / no warnings
  -3 = some errors and warnings
";

        private static void ShowCommandLineArguments()
        {
            Console.Write(commandLineInfo);
        }
        private static void DisplayResultCounters(Session session) {
            if ((session.Implementation.CountingTarget.TotalNrOfValidationErrors == 0) &&
                (session.Implementation.CountingTarget.TotalNrOfUserErrors == 0) &&
                (session.Implementation.CountingTarget.TotalNrOfGeneralErrors == 0)) {
                Console.WriteLine ("");
                Console.WriteLine("RESULT: PASSED");
            }
            else {
                Console.WriteLine ("");
                Console.WriteLine("RESULT: FAILED");
            }
            Console.WriteLine("Number of Validation Errors: {0} - Number of Validation Warnings: {1}",
                session.Implementation.CountingTarget.TotalNrOfValidationErrors, 
                session.Implementation.CountingTarget.TotalNrOfValidationWarnings);
            Console.WriteLine("Number of User Errors: {0} - Number of User Warnings: {1}",
                session.Implementation.CountingTarget.TotalNrOfUserErrors, 
                session.Implementation.CountingTarget.TotalNrOfUserWarnings);
            Console.WriteLine("Number of General Errors: {0} - Number of General Warnings: {1}",
                session.Implementation.CountingTarget.TotalNrOfGeneralErrors, 
                session.Implementation.CountingTarget.TotalNrOfGeneralWarnings);
            Console.WriteLine("");
        }

		private static void DetermineExitCode(Session session) 
		{
			if ((session.Implementation.CountingTarget.TotalNrOfValidationErrors == 0) &&
				(session.Implementation.CountingTarget.TotalNrOfUserErrors == 0) &&
				(session.Implementation.CountingTarget.TotalNrOfGeneralErrors == 0) &&
				(session.Implementation.CountingTarget.TotalNrOfValidationWarnings == 0) &&
				(session.Implementation.CountingTarget.TotalNrOfUserWarnings == 0) &&
				(session.Implementation.CountingTarget.TotalNrOfGeneralWarnings == 0)) 
			{
				// No errors / no warnings
				_exitCode = 0;
			}
			else if ((session.Implementation.CountingTarget.TotalNrOfValidationErrors == 0) &&
				(session.Implementation.CountingTarget.TotalNrOfUserErrors == 0) &&
				(session.Implementation.CountingTarget.TotalNrOfGeneralErrors == 0))
			{
				// No errors / some warnings
				_exitCode = -1;
			}
			else if ((session.Implementation.CountingTarget.TotalNrOfValidationWarnings == 0) &&
				(session.Implementation.CountingTarget.TotalNrOfUserWarnings == 0) &&
				(session.Implementation.CountingTarget.TotalNrOfGeneralWarnings == 0))
			{
				// Some errors / no warnings
				_exitCode = -2;
			}
			else
			{
				// Some errors and warnings
				_exitCode = -3;
			}
		}

    }
}
