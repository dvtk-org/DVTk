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

namespace DvtkApplicationLayer
{
    /// <summary>
    /// Summary description for SessionManager.
    /// </summary>
    public class SessionManager
    {
        IList sessionObjects = new ArrayList();
        #region Private Members

        private static SessionManager singleInstance = null;

        #endregion

        #region Properties
        /// <summary>
        /// Method to create an instance of session manager.
        /// </summary>
        public static SessionManager Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    singleInstance = new SessionManager();
                }

                return singleInstance;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        private SessionManager()
        {
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Create the instance of a seesion according to the session type.
        /// </summary>
        /// <param name="fileName">represents the sessionfile name.</param>
        /// <returns>instance of a session.</returns>
        public Session CreateSession(string fileName)
        {
            Session session = null;
            try
            {
                // Open the file and determine the SessionType
                Session.SessionType sessionType = DetermineSessionType(fileName);

                switch (sessionType)
                {
                    case Session.SessionType.ST_MEDIA:
                        session = new MediaSession(fileName);
                        break;
                    case Session.SessionType.ST_SCRIPT:
                        session = new ScriptSession(fileName);
                        break;
                    case Session.SessionType.ST_EMULATOR:
                        session = new EmulatorSession(fileName);
                        break;
                    case Session.SessionType.ST_UNKNOWN:
                        break;
                }

                if (session != null)
                    sessionObjects.Add(session);
            }
            catch (Exception ex)
            {
                session = null;
                Exception excp;
                throw excp = new Exception(ex.Message);
            }
            return session;
        }

        # endregion

        #region Private Methods

        // This is file Specific Code if the Format 
        //of file changes it need to be altered in that way.
        private Session.SessionType DetermineSessionType(string filename)
        {
            Session.SessionType sessionType = Session.SessionType.ST_UNKNOWN;
            using (StreamReader sr = new StreamReader(filename))
            {
                string line = "";
                string sessionTypeName = "unknown";
                while (sr.Peek() != -1)
                {
                    line = sr.ReadLine();
                    if ((line != null) && (line.StartsWith("SESSION-TYPE")))
                    {
                        string[] lineStrings = line.Split(new char[1] { ' ' });
                        sessionTypeName = lineStrings[1];
                    }
                }
                sr.Close();

                switch (sessionTypeName)
                {
                    case "media":
                        sessionType = Session.SessionType.ST_MEDIA;
                        break;
                    case "script":
                        sessionType = Session.SessionType.ST_SCRIPT;
                        break;
                    case "emulator":
                        sessionType = Session.SessionType.ST_EMULATOR;
                        break;
                    case "unknown":
                        sessionType = Session.SessionType.ST_UNKNOWN;
                        break;
                }
                return sessionType;
            }
        }
        #endregion
    }
}





