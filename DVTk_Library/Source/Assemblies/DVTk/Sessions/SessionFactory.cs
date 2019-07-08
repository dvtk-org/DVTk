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
using Wrappers;

namespace Dvtk.Sessions
{
    using Dvtk;
    using System.Collections;
    using DvtkData.Dimse;
    using DvtkData.Dul;
    //
    // Aliases for types
    //
    using FileName = System.String;
    using DefinitionFileRootDirectory = System.String;
    using DefinitionFileName = System.String;

    /// <summary>
    /// Factory class used to load Sessions from file with extension <c>.SES</c>.
    /// </summary>
    /// <remarks>
    /// Loaded session types are;
    /// <list type="bullet">
    /// <item>Dvtk.Sessions.ScriptSession</item>
    /// <item>Dvtk.Sessions.EmulatorSession</item>
    /// <item>Dvtk.Sessions.MediaSession</item>
    /// </list>
    /// </remarks>
    public sealed class SessionFactory
    {
        private SessionFactory () {}
        /// <summary>
        /// Singleton instance handle
        /// </summary>
        public static readonly SessionFactory TheInstance = new SessionFactory();
        /// <summary>
        /// Load a session from file.
        /// </summary>
        /// <param name="fileName">file with extension <c>.SES</c></param>
        /// <returns>
        /// Returns a session of type;
        /// <list type="bullet">
        /// <item>Dvtk.Sessions.ScriptSession</item>
        /// <item>Dvtk.Sessions.EmulatorSession</item>
        /// <item>Dvtk.Sessions.MediaSession</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// The type of session is dynamically determined.
        /// </remarks>
        public Session Load(FileName fileName)
        {
            if (fileName == null) throw new ArgumentNullException();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            if (!fileInfo.Exists) throw new ArgumentException();
            //
            // Transform fileName to fully qualified file name.
            //
            fileName = fileInfo.FullName;
            Session session = null;
            Wrappers.MBaseSession adaptee =
                Wrappers.MSessionFactory.Load(fileName);
            switch (adaptee.SessionType)
            {
                case Wrappers.SessionType.SessionTypeEmulator:
                    session = new EmulatorSession(adaptee);
                    break;
                case Wrappers.SessionType.SessionTypeMedia:
                    session = new MediaSession(adaptee);
                    break;
                case Wrappers.SessionType.SessionTypeScript:
                    session = new ScriptSession(adaptee);
                    break;
				case Wrappers.SessionType.SessionTypeSniffer:
					session = new SnifferSession(adaptee);
					break;
                case Wrappers.SessionType.SessionTypeUnknown:
                    // Unknown Wrappers.SessionType
                    throw new System.NotImplementedException();
            }
            return session;
        }
    }
}
