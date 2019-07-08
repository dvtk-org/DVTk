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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;



using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.NUnit
{
    class Paths
    {
        private static String NUnitDirectoryFullPath
        {
            get
            {
                Paths paths = new Paths();

                Assembly assembly = Assembly.GetAssembly(paths.GetType());

                String nUnitDirectoryFullPath = assembly.CodeBase;

                if (nUnitDirectoryFullPath.StartsWith("file:///"))
                {
                    nUnitDirectoryFullPath = nUnitDirectoryFullPath.Substring(8);
                }

                nUnitDirectoryFullPath = Path.GetDirectoryName(nUnitDirectoryFullPath);
                nUnitDirectoryFullPath = Path.GetDirectoryName(nUnitDirectoryFullPath);
                nUnitDirectoryFullPath = Path.GetDirectoryName(nUnitDirectoryFullPath);

                return (nUnitDirectoryFullPath);
            }
        }

        public static String DataDirectoryFullPath
        {
            get
            {
                String dataDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, "Data");

                dataDirectoryFullPath = Path.GetFullPath(dataDirectoryFullPath);

                return (dataDirectoryFullPath);
            }
        }

        public static String ResultsDirectoryFullPath
        {
            get
            {
                String resultsDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, "Results");

                resultsDirectoryFullPath = Path.GetFullPath(resultsDirectoryFullPath);

                return (resultsDirectoryFullPath);
            }
        }

        public static String DefinitionsDirectoryFullPath
        {
            get
            {
                String defDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, @"Resources\Definitions");

                defDirectoryFullPath = Path.GetFullPath(defDirectoryFullPath);

                return (defDirectoryFullPath);
            }
        }

        public static String SessionDirectoryFullPath
        {
            get
            {
                String resultsDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, @"Resources\Sessions");

                resultsDirectoryFullPath = Path.GetFullPath(resultsDirectoryFullPath);

                return (resultsDirectoryFullPath);
            }
        }

        public static String SQATestsResourcesDirectoryFullPath
        {
            get
            {
                String sqaTestsDirectoryFullPath = Path.Combine(NUnitDirectoryFullPath, @"Resources\SQATests");

                sqaTestsDirectoryFullPath = Path.GetFullPath(sqaTestsDirectoryFullPath);

                return (sqaTestsDirectoryFullPath);
            }
        }
    }
}
