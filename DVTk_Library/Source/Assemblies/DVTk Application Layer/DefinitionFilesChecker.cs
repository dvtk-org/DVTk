using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;
using System.Web.UI.WebControls;
using Microsoft.Win32;

namespace DvtkApplicationLayer
{
    public class DefinitionFilesChecker
    {
        //
        // - Constructors -
        //

        /// <summary>
        /// // Don't instantiate.
        /// </summary>
        public DefinitionFilesChecker()
        {
            // Do nothing.
            
        }
        

        //
        // - Methods -
        //

        /// <summary>
        /// Retrives the Version of the Definition Files installed and compares
        /// the value with the Version Numbers that the application can support.
        /// </summary>
        /// 
        /// <returns>Boolean value</returns>
        public static bool CheckVersion(string minimumVersion, string maximumVersion)
        {
            
            RegistryKey key = Registry.LocalMachine;
            key = key.OpenSubKey(@"Software\DVTk\Definition Files");
            
            string version = (string)key.GetValue("Version");
            if (version == null)
            {
                MessageBox.Show("There are no definition files installed on the machine or the installed version is very old.Please install the latest definition files.The latest stable version can be found at http://www.dvtk.org/downloads/"
                                 , "Error", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return false;
            }
            else
            {
                string[] versionArray = version.Split('.');
                int definitionFilesVersion = Int32.Parse(versionArray[0] ,System.Globalization.NumberStyles.Any);
                int definitionFilesVersionMinor = Int32.Parse(versionArray[1], System.Globalization.NumberStyles.Any);

                string[] minVersionArray = minimumVersion.Split('.');
                int minVersion = Int32.Parse(minVersionArray[0],System.Globalization.NumberStyles.Any);
                int minVersionMinor = Int32.Parse(minVersionArray[1], System.Globalization.NumberStyles.Any);
                
                string[] maxVersionArray = maximumVersion.Split('.');
                int maxVersion = Int32.Parse(maxVersionArray[0] , System.Globalization.NumberStyles.Any);
                int maxVersionMinor = Int32.Parse(maxVersionArray[1], System.Globalization.NumberStyles.Any);

                if (definitionFilesVersion >= minVersion && ((definitionFilesVersion <= maxVersion)))
                {
                    if(definitionFilesVersion < maxVersion)
                    {
                    if (definitionFilesVersionMinor >= minVersionMinor)
                    {
                        return true;
                    }
                    else 
                    {
                        MessageBox.Show("The definition files need to be updated.The latest stable definition files can be found at http://www.dvtk.org/downloads/",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                        return false;
                    }
                    }
                    else
                    {
                        if (definitionFilesVersionMinor <= maxVersionMinor)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("The definition files are not compatible with the application.Check for an update of the application or downgrade the application.Use the following link http://www.dvtk.org/downloads/",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                }
                else if (definitionFilesVersion < minVersion)
                {
                    MessageBox.Show("The definition files need to be updated.The latest stable definition files can be found at http://www.dvtk.org/downloads/",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                    return false;

                }
                else
                {
                    MessageBox.Show("The definition files are not compatible with the application.Check for an update of the application or downgrade the application.Use the following link http://www.dvtk.org/downloads/",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            
        }
        
    }
     
}
