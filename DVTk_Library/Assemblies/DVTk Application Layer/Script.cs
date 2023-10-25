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
using DvtkSession = Dvtk.Sessions;

namespace DvtkApplicationLayer
{
    /// <summary>
    /// Summary description for Script.
    /// </summary>

    public class Script : PartOfSession
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="scriptFileName"></param>
        public Script(Session session, String scriptFileName) : base(session)
        {
            this.scriptFileName = scriptFileName;
        }

        private string scriptFileName;
        private IList resultScripts = new ArrayList();

        /// <summary>
        /// represents the script name.
        /// </summary>
        public string ScriptFileName
        {
            get
            {
                return scriptFileName;
            }
            set
            {
                scriptFileName = value;
            }
        }

        /// <summary>
        /// Collection of results for a script.
        /// </summary>
		public IList Result
        {
            get
            {
                return resultScripts;
            }
            set
            {
                resultScripts = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveResultFiles() { }
        /// <summary>
        /// Method to Create the results for a script.
        /// </summary>
        /// <param name="scriptFile"> Name of the script.</param>


        public void CreateScriptResult(Script scriptFile)
        {
            ArrayList scripts = new ArrayList();
            DirectoryInfo directoryInf = new DirectoryInfo(session.ResultsRootDirectory);
            FileInfo[] filesInf = directoryInf.GetFiles("summary_" + "*" + scriptFile.ScriptFileName.Replace(".", "_") + "*.xml");
            ArrayList tempResultFiles = new ArrayList();


            foreach (FileInfo fileInf in filesInf)
            {
                Result correctResult = null;
                String sessionId = session.GetSessionId(fileInf.Name);

                foreach (Result result in scriptFile.Result)
                {
                    if (result.SessionId == sessionId)
                    {
                        correctResult = result;
                        break;
                    }
                }

                if (correctResult == null)
                {
                    correctResult = new Result(this.ParentSession);
                    correctResult.SessionId = sessionId;

                    scriptFile.Result.Add(correctResult);
                }

                bool isSummaryFile = true;
                bool isMainResult = true;

                if (fileInf.Name.ToLower().StartsWith("summary_"))
                {
                    isSummaryFile = true;
                }
                else
                {
                    isSummaryFile = false;
                }

                if (fileInf.Name.ToLower().EndsWith(scriptFile.ScriptFileName.Replace(".", "_").ToLower() + "_res.xml"))
                {
                    isMainResult = true;
                }
                else
                {
                    isMainResult = false;
                }

                if (isSummaryFile)
                {
                    if (isMainResult)
                    {
                        correctResult.SummaryFile = fileInf.Name;
                    }
                    else
                    {
                        correctResult.SubSummaryResultFiles.Add(fileInf.Name);
                    }
                }
                else
                {
                    if (isMainResult)
                    {
                        correctResult.DetailFile = fileInf.Name;
                    }
                    else
                    {
                        correctResult.SubDetailResultFiles.Add(fileInf.Name);
                    }
                }
            }
        }
    }
}

