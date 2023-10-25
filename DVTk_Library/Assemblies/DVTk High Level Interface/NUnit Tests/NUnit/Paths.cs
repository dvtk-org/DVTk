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



using System.IO;
using System.Reflection;

namespace DvtkHighLevelInterface.NUnit
{
    class Paths
    {
        private static string NUnitDirectoryFullPath
        {
            get
            {
                Paths paths = new Paths();
                Assembly assembly = Assembly.GetAssembly(paths.GetType());
                string nUnitDirectoryFullPath = assembly.CodeBase;
                if (nUnitDirectoryFullPath.StartsWith("file:///"))
                {
                    nUnitDirectoryFullPath = nUnitDirectoryFullPath.Substring(8);
                }

                nUnitDirectoryFullPath = Path.GetDirectoryName(nUnitDirectoryFullPath);
                //nUnitDirectoryFullPath = Path.GetDirectoryName(nUnitDirectoryFullPath);
                //nUnitDirectoryFullPath = Path.GetDirectoryName(nUnitDirectoryFullPath);
                return nUnitDirectoryFullPath;
            }
        }


        public static string DataDirectoryFullPath
        {
            get
            {
                string dataDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, "Data");
                dataDirectoryFullPath = Path.GetFullPath(dataDirectoryFullPath);
                return dataDirectoryFullPath;
            }
        }


        public static string ResultsDirectoryFullPath
        {
            get
            {
                string resultsDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, "Results");
                resultsDirectoryFullPath = Path.GetFullPath(resultsDirectoryFullPath);
                return resultsDirectoryFullPath;
            }
        }


        public static string DefinitionsDirectoryFullPath
        {
            get
            {
                string defDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, @"Resources\Definitions");
                defDirectoryFullPath = Path.GetFullPath(defDirectoryFullPath);
                return defDirectoryFullPath;
            }
        }


        public static string SessionDirectoryFullPath
        {
            get
            {
                string resultsDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, @"Resources\Sessions");
                resultsDirectoryFullPath = Path.GetFullPath(resultsDirectoryFullPath);
                return resultsDirectoryFullPath;
            }
        }


        public static string SQATestsResourcesDirectoryFullPath
        {
            get
            {
                string sqaTestsDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, @"Resources\SQATests");
                sqaTestsDirectoryFullPath = Path.GetFullPath(sqaTestsDirectoryFullPath);
                return (sqaTestsDirectoryFullPath);
            }
        }
    }
}