using System;
using System.Collections.Generic;
using System.Text;

namespace Dvtk.Dicom
{
    /// <summary>
    /// VALUE REPRESENTATION (VR)
    /// </summary>
    /// <remarks>
    /// <p>
    /// Specifies the data type and format of the Value(s) contained in the Value Field of an Attribute.</p>
    /// <p>
    /// The Value Representation of an Attribute describes the data type and format of that Attribute's
    /// Value(s).
    /// </p>
    /// The reason to assign bit values to all possible VR's is that for some attributes defined by
    /// the DICOM standard multiple VR's are allowed. This will allow for efficient storage of 
    /// possible VR's for an attribute. For attributes in actual DICOM object only one VR is allowed.
    /// </remarks>
    public enum VR
    {
        None = 0x00000000,
        /// <summary>
        /// Application Entity
        /// </summary>
        AE = 0x00000001,
        /// <summary>
        /// Age String
        /// </summary>
        AS = 0x00000002,
        /// <summary>
        /// Attribute Tag
        /// </summary>
        AT = 0x00000004,
        /// <summary>
        /// Code String
        /// </summary>
        CS = 0x00000008,
        /// <summary>
        /// Date
        /// </summary>
        DA = 0x00000010,
        /// <summary>
        /// Decimal String
        /// </summary>
        DS = 0x00000020,
        /// <summary>
        /// Date Time
        /// </summary>
        DT = 0x00000040,
        /// <summary>
        /// Floating Point Single
        /// </summary>
        FL = 0x00000080,
        /// <summary>
        /// Floating Point Double
        /// </summary>
        FD = 0x00000100,
        /// <summary>
        /// Integer String
        /// </summary>
        IS = 0x00000200,
        /// <summary>
        /// Long String
        /// </summary>
        LO = 0x00000400,
        /// <summary>
        /// Long Text
        /// </summary>
        LT = 0x00000800,
        /// <summary>
        /// Other Byte String
        /// </summary>
        OB = 0x00001000,
        /// <summary>
        /// Other Float String
        /// </summary>
        OF = 0x00002000,
        /// <summary>
        /// Other Word String
        /// </summary>
        OW = 0x00004000,
        /// <summary>
        /// Person Name
        /// </summary>
        PN = 0x00008000,
        /// <summary>
        /// Short String
        /// </summary>
        SH = 0x00010000,
        /// <summary>
        /// Signed Long
        /// </summary>
        SL = 0x00020000,
        /// <summary>
        /// Sequence of Items
        /// </summary>
        SQ = 0x00040000,
        /// <summary>
        /// Signed Short
        /// </summary>
        SS = 0x00080000,
        /// <summary>
        /// Short Text
        /// </summary>
        ST = 0x00100000,
        /// <summary>
        /// Time
        /// </summary>
        TM = 0x00200000,
        /// <summary>
        /// Unique Identifier (UID)
        /// </summary>
        UI = 0x00400000,
        /// <summary>
        /// Unsigned Long
        /// </summary>
        UL = 0x00800000,
        /// <summary>
        /// Unknown
        /// </summary>
        UN = 0x01000000,
        /// <summary>
        /// Unsigned Short
        /// </summary>
        US = 0x02000000,
        /// <summary>
        /// Unlimited Text
        /// </summary>
        UT = 0x04000000
    }

    public static class VRHelper
    {
        public static string ToString(VR valueRepresentations, string seperator)
        {
            string stringRepresentation = string.Empty;

            foreach(VR vr in Enum.GetValues(typeof (VR)))
            {
                if (vr != VR.None)
                {
                    if ((valueRepresentations & vr) == vr)
                    {
                        if (stringRepresentation.Length == 0)
                        {
                            stringRepresentation = vr.ToString();
                        }
                        else
                        {
                            stringRepresentation += seperator + vr.ToString();
                        }
                    }
                }
            }


            return(stringRepresentation);
        }
    }
}
