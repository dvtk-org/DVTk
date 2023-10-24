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
using System.Management;
using System.Reflection;

using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// This class offers Unique Identifier functionality.
	/// </summary>
	public class UID
	{
		//
		// - Constant fields -
		//

		/// <summary>
		/// The DVTk UID root (26 characters).
		/// </summary>
		public const String DVTkRoot = "1.2.826.0.1.3680043.2.1545";



		//
		// - Fields -
		//

		/// <summary>
		/// Indicates if specific fields have been initialized.
		/// </summary>
		private static bool isInitialized = false;

        /// <summary>
        /// Contains last created UID.
        /// </summary>
		private static String lastCreatedUid = "";

		/// <summary>
		/// Used to lock a set of related static fields.
		/// </summary>
		private static Object lockObject = new Object();

		/// <summary>
		/// The MAC address or random number (in case no MAC address can be determined).
		/// (maximum 15 characters).
		/// </summary>
		private static String macNumberOrRandomNumber = "";

		/// <summary>
		/// Used when no MAC address is present.
		/// This field is Thread Safe because it is a public static field.
		/// </summary>
		private static Random random = new Random();

		/// <summary>
		/// Version of HLI Assembly (max 11 characters).
		/// </summary>
		private static String version = "";



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private UID()
		{
			// Do nothing.
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Create a UID.
		/// 
		/// The length of the supplied string will be maximum 64 characters, as required by the Dicom standard.
		/// </summary>
		/// <returns></returns>
		public static String Create()
		{
			String createdUID = "";

			// Lock because we access the version and macNumberOrRandomNumber field.
			lock(lockObject)
			{
				if (!isInitialized)
				{
					Initialize();
				}

				isInitialized = true;

				bool tryToCreate = true; 

				while (tryToCreate)
				{

					// Take care that the timer gets time to increment by going to sleep for 1 millisecond.
					System.Threading.Thread.Sleep(1);

					String ticks = DateTime.Now.Ticks.ToString();
			
					// Make sure it contains at most 13 digits.
					ticks = ticks.Substring(ticks.Length - 16);
					ticks = ticks.Substring(0, ticks.Length - 3);

					createdUID = DVTkRoot + "." + version + "." + macNumberOrRandomNumber + "." + ticks;

					if (lastCreatedUid != createdUID)
					{
						tryToCreate = false;
					}
				}

				lastCreatedUid = createdUID;
			}

			return(createdUID);
		}

        /// <summary>
        /// Get the (unique) MAC address of one of the network cards.
        /// When no network card is available, an empty String is returned.
        /// </summary>
        /// <returns>The MAC address.</returns>
        public static String GetNetworkCardMacAddress()
        {
            String firstMacAddress = "";

            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                if (firstMacAddress == "")
                {
                    if (((bool)managementObject["IPEnabled"]) && (managementObject["MacAddress"] != null) && (managementObject["IPAddress"] != null))
                    {
                        firstMacAddress = managementObject["MacAddress"].ToString();
                    }
                }

                if (firstMacAddress != "")
                {
                    break;
                }
            }

            return (firstMacAddress);
        }

		/// <summary>
		/// Contains code that needs to be executed only once.
		/// </summary>
		private static void Initialize()
		{
			macNumberOrRandomNumber = GetNetworkCardMacAddress();

			if (macNumberOrRandomNumber == "")
			{
				Thread.WriteWarningCurrentThread("MAC address not found. Using random number instead.");
				macNumberOrRandomNumber = random.Next(int.MaxValue).ToString();	
			}
			else
			{
				// Remove the seperators from the MAC address.
				macNumberOrRandomNumber = macNumberOrRandomNumber.Replace(":", "");

				// Convert the hexadecimal value to a decimal value.
				macNumberOrRandomNumber = UInt64.Parse(macNumberOrRandomNumber, System.Globalization.NumberStyles.AllowHexSpecifier).ToString();
			}

			Assembly assembly = Assembly.GetAssembly(new DvtkHighLevelInterface.Common.Threads.ThreadState().GetType());

			AssemblyName assemblyName = assembly.GetName();

			version = String.Format("{0}.{1}.{2}", assemblyName.Version.Major, assemblyName.Version.Minor, assemblyName.Version.Build, assemblyName.Version.Revision);
		}
	}
}
