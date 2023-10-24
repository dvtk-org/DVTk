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

namespace Dvtk.CommonDataFormat
{
    /// <summary>
    /// Summary description for CommonNameFormat.
    /// </summary>
    public class CommonNameFormat : BaseCommonDataFormat
    {
        private System.String _surname = System.String.Empty;
        private System.String _firstName = System.String.Empty;
        private System.String _middleName = System.String.Empty;
        private System.String _prefix = System.String.Empty;
        private System.String _suffix = System.String.Empty;
        private System.String _degree = System.String.Empty;
        private System.String _ideographicName = System.String.Empty;
        private System.String _phoneticName = System.String.Empty;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public CommonNameFormat() { }

        #region base class overrides
        /// <summary>
        /// Convert from Common Data Format to DICOM format.
        /// </summary>
        /// <returns>DICOM format.</returns>
        public override System.String ToDicomFormat()
        {
            // format the name using all the components
            System.String nameComponents = _surname + "^" +
                _firstName + "^" +
                _middleName + "^" +
                _prefix + "^" +
                _suffix;

            // trim off any trailing component delimiters
            System.String dicomName = nameComponents.TrimEnd('^');

            // add the ideographic name representation
            if (_ideographicName != System.String.Empty)
            {
                dicomName += ("=" + _ideographicName);
            }

            // add the phonetic name representation
            if (_phoneticName != System.String.Empty)
            {
                if (_ideographicName == System.String.Empty)
                {
                    // add empty ideographic name
                    dicomName += "=";
                }
                dicomName += ("=" + _phoneticName);
            }

            return dicomName;
        }

        /// <summary>
        /// Convert from DICOM format to Common Data Format.
        /// </summary>
        /// <param name="dicomFormat">DICOM format.</param>
        public override void FromDicomFormat(System.String dicomFormat)
        {
            // remove any trailing spaces from the name
            dicomFormat = dicomFormat.TrimEnd(' ');

            // split the incoming dicom format into the three component groups
            System.String[] nameComponentGroups = new System.String[3];
            nameComponentGroups = dicomFormat.Split('=');

            // split the first component group into the five components
            if (nameComponentGroups.Length > 0)
            {
                System.String[] nameComponents = new System.String[5];
                nameComponents = nameComponentGroups[0].Split('^');

                // save the individual components
                switch (nameComponents.Length)
                {
                    case 1:
                        _surname = nameComponents[0];
                        break;
                    case 2:
                        _surname = nameComponents[0];
                        _firstName = nameComponents[1];
                        break;
                    case 3:
                        _surname = nameComponents[0];
                        _firstName = nameComponents[1];
                        _middleName = nameComponents[2];
                        break;
                    case 4:
                        _surname = nameComponents[0];
                        _firstName = nameComponents[1];
                        _middleName = nameComponents[2];
                        _prefix = nameComponents[3];
                        break;
                    case 5:
                        _surname = nameComponents[0];
                        _firstName = nameComponents[1];
                        _middleName = nameComponents[2];
                        _prefix = nameComponents[3];
                        _suffix = nameComponents[4];
                        break;
                    default:
                        break;
                }

                // save the ideographic name - if present
                if (nameComponentGroups.Length > 1)
                {
                    _ideographicName = nameComponentGroups[1];
                }

                // save the phonetic name - if present
                if (nameComponentGroups.Length > 2)
                {
                    _phoneticName = nameComponentGroups[2];
                }
            }
        }

        /// <summary>
        /// Convert from Common Data Format to HL7 format.
        /// </summary>
        /// <returns>HL7 format.</returns>
        public override System.String ToHl7Format()
        {
            // format the name using all the components
            System.String nameComponents = _surname + "^" +
                _firstName + "^" +
                _middleName + "^" +
                _suffix + "^" +
                _prefix + "^" +
                _degree;

            // trim off any trailing component delimiters
            System.String hl7Name = nameComponents.TrimEnd('^');

            return hl7Name;
        }

        /// <summary>
        /// Convert from HL7 format to Common Data Format.
        /// </summary>
        /// <param name="hl7Format">HL7 format.</param>
        public override void FromHl7Format(System.String hl7Format)
        {
            // <family name (ST)> ^ 
            // <given name (ST)> ^ 
            // <middle initial or name (ST)> ^ 
            // <suffix (e.g., JR or III) (ST)> ^ 
            // <prefix (e.g., DR) (ST)> ^ 
            // <degree (e.g., MD) (ST)>
            // remove any trailing spaces from the name
            hl7Format = hl7Format.TrimEnd(' ');

            // split the HL7 format into the six components
            System.String[] nameComponents = new System.String[6];
            nameComponents = hl7Format.Split('^');

            // save the individual components
            switch (nameComponents.Length)
            {
                case 1:
                    _surname = nameComponents[0];
                    break;
                case 2:
                    _surname = nameComponents[0];
                    _firstName = nameComponents[1];
                    break;
                case 3:
                    _surname = nameComponents[0];
                    _firstName = nameComponents[1];
                    _middleName = nameComponents[2];
                    break;
                case 4:
                    _surname = nameComponents[0];
                    _firstName = nameComponents[1];
                    _middleName = nameComponents[2];
                    _suffix = nameComponents[3];
                    break;
                case 5:
                    _surname = nameComponents[0];
                    _firstName = nameComponents[1];
                    _middleName = nameComponents[2];
                    _suffix = nameComponents[3];
                    _prefix = nameComponents[4];
                    break;
                case 6:
                    _surname = nameComponents[0];
                    _firstName = nameComponents[1];
                    _middleName = nameComponents[2];
                    _suffix = nameComponents[3];
                    _prefix = nameComponents[4];
                    _degree = nameComponents[5];
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Check if the objects are equal.
        /// </summary>
        /// <param name="obj">Comparison object.</param>
        /// <returns>bool indicating true = equal or false  = not equal.</returns>
        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj is CommonNameFormat)
            {
                CommonNameFormat thatCommonName = (CommonNameFormat)obj;
                if ((this.Surname == thatCommonName.Surname) &&
                    (this.FirstName == thatCommonName.FirstName) &&
                    (this.MiddleName == thatCommonName.MiddleName) &&
                    (this.Prefix == thatCommonName.Prefix) &&
                    (this.Suffix == thatCommonName.Suffix) &&
                    (this.Degree == thatCommonName.Degree))
                    equals = true;
            }
            return equals;
        }

        /// <summary>
        /// Get HashCode.
        /// </summary>
        /// <returns>Base HashCode value.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Console Display the Common Data Format content - for debugging purposes.
        /// </summary>
        public override void ConsoleDisplay()
        {
            Console.WriteLine("CommonNameFormat...");
            Console.WriteLine("Surname: \"{0}\"", _surname);
            Console.WriteLine("FirstName: \"{0}\"", _firstName);
            Console.WriteLine("MiddleName: \"{0}\"", _middleName);
            Console.WriteLine("Prefix: \"{0}\"", _prefix);
            Console.WriteLine("Suffix: \"{0}\"", _suffix);
            Console.WriteLine("Degree: \"{0}\"", _degree);
            Console.WriteLine("DicomNameFormat...");
            Console.WriteLine("PersonName: \"{0}\"", this.ToDicomFormat());
            Console.WriteLine("Hl7NameFormat...");
            Console.WriteLine("PersonName: \"{0}\"", this.ToHl7Format());
        }
        #endregion

        #region properties
        /// <summary>
        /// Surname Property
        /// </summary>
        public System.String Surname
        {
            set
            {
                _surname = value.Trim();
            }
            get
            {
                return _surname;
            }
        }

        /// <summary>
        /// FirstName Property
        /// </summary>
        public System.String FirstName
        {
            set
            {
                _firstName = value.Trim();
            }
            get
            {
                return _firstName;
            }
        }

        /// <summary>
        /// MiddleName Property
        /// </summary>
        public System.String MiddleName
        {
            set
            {
                _middleName = value.Trim();
            }
            get
            {
                return _middleName;
            }
        }

        /// <summary>
        /// Prefix Property
        /// </summary>
        public System.String Prefix
        {
            set
            {
                _prefix = value.Trim();
            }
            get
            {
                return _prefix;
            }
        }

        /// <summary>
        /// Suffix Property
        /// </summary>
        public System.String Suffix
        {
            set
            {
                _suffix = value.Trim();
            }
            get
            {
                return _suffix;
            }
        }

        /// <summary>
        /// Degree Property
        /// </summary>
        public System.String Degree
        {
            set
            {
                _degree = value.Trim();
            }
            get
            {
                return _degree;
            }
        }

        /// <summary>
        /// Ideographic Name Representation Property
        /// </summary>
        public System.String IdeographicName
        {
            set
            {
                _ideographicName = value;
            }
            get
            {
                return _ideographicName;
            }
        }

        /// <summary>
        /// Phonetic Name Representation Property
        /// </summary>
        public System.String PhoneticName
        {
            set
            {
                _phoneticName = value;
            }
            get
            {
                return _phoneticName;
            }
        }

        #endregion
    }
}
