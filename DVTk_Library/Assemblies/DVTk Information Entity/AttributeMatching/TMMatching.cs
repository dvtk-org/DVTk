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
using System.Text;
using System.Text.RegularExpressions;

namespace Dvtk.Dicom.InformationEntity.AttributeMatching
{
    class TMMatching
    {
        //
        // - Fields -
        //

        private const String regexStringTime1 = @"(?<Time1>(\d\d)(\d\d)?(\d\d)?(\.\d\d{0,5})?)";

        private const String regexStringTime2 = @"(?<Time2>(\d\d)(\d\d)?(\d\d)?(\.\d\d{0,5})?)";

        private static Regex regexTimeOnly = new Regex("^" + regexStringTime1 + " *$");

        private static Regex regexTimeFrom = new Regex("^" + regexStringTime1 + "- *$");

        private static Regex regexTimeTo = new Regex("^-" + regexStringTime1 + " *$");

        private static Regex regexTimeFromTo = new Regex("^" + regexStringTime1 + "-" + regexStringTime2 + " *$");



        //
        // - Methods -
        //

        /// <summary>
        /// Padd the supplied time in such a way, that the format becomes "HHMMSS.FFFFFF".
        /// </summary>
        /// <param name="time">The time that may be padded.</param>
        /// <returns>The padded time.</returns>
        private static String Padd(String time)
        {
            String paddedTime = time;

            //
            // If part of HHMMSS missing, fill this up.
            //

            if (paddedTime.Length < 6)
            {
                paddedTime = paddedTime.PadRight(6, '0');
            }


            //
            // If part of fractional seconds missing, fill this up.
            // 

            if (!time.Contains("."))
            {
                paddedTime += ".";
            }

            paddedTime = paddedTime.PadRight(13, '0');

            return (paddedTime);
        }

        /// <summary>
        /// Indicates if an TM attribute value matches a value in a C-FIND-RQ.
        /// </summary>
        /// <param name="attributeValue">Single value of an attribute.</param>
        /// <param name="requestValue">Value in a C-FIND-RQ.</param>
        /// <returns>Does it match. </returns>
        public static bool Matches(String attributeValue, String requestValue)
        {
            bool matches = false;

            // Make sure the attribute time has the format HHMMSS.FFFFFF.
            String attributeTime = Padd(attributeValue.Trim());

            if (regexTimeOnly.IsMatch(requestValue))
            // If request value has the format "HHMMSS.FFFFFF" ...
            {
                String requestTime = regexTimeOnly.Match(requestValue).Groups["Time1"].ToString();

                // Make sure the request time has the format HHMMSS.FFFFFF.
                requestTime = Padd(requestTime);

                if (String.Compare(attributeTime, requestTime) == 0)
                {
                    matches = true;
                }
                else
                {
                    matches = false;
                }
            }
            else if (regexTimeFrom.IsMatch(requestValue))
            // If request value has the format "HHMMSS.FFFFFF-" ...
            {
                String requestTime = regexTimeFrom.Match(requestValue).Groups["Time1"].ToString();

                // Make sure the request time has the format HHMMSS.FFFFFF.
                requestTime = Padd(requestTime);

                if (String.Compare(attributeTime, requestTime) >= 0)
                {
                    matches = true;
                }
                else
                {
                    matches = false;
                }
            }
            else if (regexTimeTo.IsMatch(requestValue))
            // If request value has the format "-HHMMSS.FFFFFF" ...
            {
                String requestTime = regexTimeTo.Match(requestValue).Groups["Time1"].ToString();

                // Make sure the request time has the format HHMMSS.FFFFFF.
                requestTime = Padd(requestTime);

                if (String.Compare(attributeTime, requestTime) <= 0)
                {
                    matches = true;
                }
                else
                {
                    matches = false;
                }
            }
            else if (regexTimeFromTo.IsMatch(requestValue))
            // If request value has the format "HHMMSS.FFFFFF-HHMMSS.FFFFFF" ...
            {
                String requestTime1 = regexTimeFromTo.Match(requestValue).Groups["Time1"].ToString();
                String requestTime2 = regexTimeFromTo.Match(requestValue).Groups["Time2"].ToString();

                // Make sure the request times have the format HHMMSS.FFFFFF.
                requestTime1 = Padd(requestTime1);
                requestTime2 = Padd(requestTime2);

                if ((String.Compare(attributeTime, requestTime1) >= 0) && (String.Compare(attributeTime, requestTime2) <= 0))
                {
                    matches = true;
                }
                else
                {
                    matches = false;
                }
            }
            else
            {
                // This format of matching is illegal.
                matches = false;
            }

            return (matches);
        }
    }
}
