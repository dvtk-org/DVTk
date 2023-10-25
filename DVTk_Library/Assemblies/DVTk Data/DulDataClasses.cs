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

namespace DvtkData
{
    /// <summary>
    /// Generic placeholder for DulMessage and DicomMessage
    /// </summary>
    public abstract class Message
    {
    }
}
namespace DvtkData.Dul
{
    using System.IO;
    using System.Collections;
    using DvtkData.DvtDetailToXml;

    /// <summary>
    /// The Protocol Data Units (PDUs) are the message formats exchanged 
    /// between peer entities within a layer.
    /// </summary>
    /// <remarks>
    /// A PDU shall consist of protocol control information and user data. 
    /// PDUs are constructed by PS 3.8-2003 Page 31 mandatory fixed fields followed by 
    /// optional variable fields which contain one or more items and/or sub-items.
    /// </remarks>
    /// <remarks>
    /// Items of unrecognized types shall be ignored and skipped. 
    /// Items shall appear in an increasing order of their item types. 
    /// Several instances of the same item shall be acceptable or shall 
    /// not as specified by each item.
    /// </remarks>
    /// <remarks>
    /// The DICOM UL protocol consists of seven Protocol Data Units:<br></br>
    /// a) A-ASSOCIATE-RQ PDU<br></br>
    /// b) A-ASSOCIATE-AC PDU<br></br>
    /// c) A-ASSOCIATE-RJ PDU<br></br>
    /// d) P-DATA-TF PDU<br></br>
    /// e) A-RELEASE-RQ PDU<br></br>
    /// f) A-RELEASE-RP PDU<br></br>
    /// g) A-ABORT PDU<br></br>
    /// </remarks>
    /// <remarks>
    /// The encoding of the DICOM UL PDUs is defined as follows (Big Endian byte ordering):
    /// </remarks>
    /// <remarks>
    /// Note: The Big Endian byte ordering has been chosen for consistency with the 
    /// OSI and TCP/IP environment.
    /// This pertains to the DICOM UL PDU headers only. The encoding of the PDV message fragments is
    /// defined by the Transfer Syntax negotiated at association establishment.
    /// </remarks>
    /// <remarks>
    /// a) Each PDU type shall consist of one or more bytes that when represented, are numbered
    /// sequentially, with byte 1 being the lowest byte number.<br></br>
    /// b) Each byte within the PDU shall consist of eight bits that, 
    /// when represented, are numbered 7 to 0, where bit 0 is the low order bit.<br></br>
    /// c) When consecutive bytes are used to represent a string of characters, 
    /// the lowest byte numbers represent the first character.<br></br>
    /// d) When consecutive bytes are used to represent a binary number, 
    /// the lower byte number has the most significant value.<br></br>
    /// e) The lowest byte number is placed first in the transport service data flow.<br></br>
    /// f) An overview of the PDUs is shown in Figures 9-1 and 9-2.
    /// The detailed structure of each PDU is specified in the following sections.<br></br>
    /// </remarks>
    /// <remarks>
    /// Note: A number of parameters defined in the UL Service are not reflected in these PDUs (e.g. service
    /// parameters, fixed values, values not used by DICOM Application Entities.)
    /// </remarks>
    public abstract class DulMessage : DvtkData.Message, IDvtDetailToXml
    {

        /// <summary>
        /// Protocol Data Unit - Type Identifier.
        /// </summary>
        public abstract PduType PduType
        {
            get;
        }
        //private PduType _PduType = DvtkData.Dul.PduType.A_ABORT;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public abstract bool DvtDetailToXml(StreamWriter streamWriter, int level);
    }

    /// <summary>
    /// Index of PDU Types.
    /// ACSE/Presentation Service Primitives.
    /// </summary>
    public enum PduType
    {

        /// <summary>
        /// 0x01 Hex
        /// </summary>
        A_ASSOCIATE_RQ = 0x01,

        /// <summary>
        /// 0x02 Hex
        /// </summary>
        A_ASSOCIATE_AC = 0x02,

        /// <summary>
        /// 0x03 Hex
        /// </summary>
        A_ASSOCIATE_RJ = 0x03,

        /// <summary>
        /// 0x04 Hex
        /// </summary>
        P_DATA_TF = 0x04,

        /// <summary>
        /// 0x05 Hex
        /// </summary>
        A_RELEASE_RQ = 0x05,

        /// <summary>
        /// 0x06 Hex
        /// </summary>
        A_RELEASE_RP = 0x06,

        /// <summary>
        /// 0x07 Hex
        /// </summary>
        A_ABORT = 0x07,
    }

    /// <summary>
    /// Idex of Item types.
    /// </summary>
    public enum PduItemType
    {
        /// <summary>
        /// 0x10 Hex
        /// </summary>
        APPLICATION_CONTEXT = 0x10,
        /// <summary>
        /// 0x20 Hex
        /// </summary>
        RQ_PRESENTATION_CONTEXT = 0x20,
        /// <summary>
        /// 0x21 Hex
        /// </summary>
        AC_PRESENTATION_CONTEXT = 0x21,
        /// <summary>
        /// 0x30 Hex
        /// </summary>
        ABSTRACT_SYNTAX = 0x30,
        /// <summary>
        /// 0x40 Hex
        /// </summary>
        TRANSFER_SYNTAX = 0x40,
        /// <summary>
        /// 0x50 Hex
        /// </summary>
        USER_INFORMATION = 0x50,
        /// <summary>
        /// 0x51 Hex
        /// </summary>
        MAXIMUM_LENGTH = 0x51,
        /// <summary>
        /// 0x52 Hex
        /// </summary>
        IMPLEMENTATION_UID = 0x52,
        /// <summary>
        /// 0x53 Hex
        /// </summary>
        ASYNCHRONOUS_OPERATIONS_WINDOW = 0x53,
        /// <summary>
        /// 0x54 Hex
        /// </summary>
        SCP_SCU_ROLE_SELECTION = 0x54,
        /// <summary>
        /// 0x55 Hex
        /// </summary>
        IMPLEMENTATION_VERSION_NAME = 0x55,
        /// <summary>
        /// 0x56 Hex
        /// </summary>
        SOP_CLASS_EXTENDED_NEGOTIATION = 0x56,
        /// <summary>
        /// 0x58 Hex
        /// </summary>
        USER_IDENTITY_NEGOTIATION = 0x58,
        /// <summary>
        /// 0x00 Hex
        /// </summary>
        NOT_SPECIFIED = 0x00,
    }

    /// <summary>
    /// Protocol Data Unit Item.
    /// </summary>
    /// <remarks>
    /// This item is a part of a PDU.
    /// </remarks>
    public abstract class PduItem : IDvtDetailToXml
    {

        /// <summary>
        /// Protocol Data Unit - Type Identifier.
        /// </summary>
        public virtual PduItemType PduItemType
        {
            get
            {
                return _PduItemType;
            }
            set
            {
                _PduItemType = value;
            }
        }
        private PduItemType _PduItemType = PduItemType.NOT_SPECIFIED;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public abstract bool DvtDetailToXml(StreamWriter streamWriter, int level);
    }

    /// <summary>
    /// Type safe SopClassExtendedNegotiationList
    /// </summary>
    public sealed class SopClassExtendedNegotiationList :
        DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SopClassExtendedNegotiationList() { }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new SopClassExtendedNegotiation this[int index]
        {
            get
            {
                return (SopClassExtendedNegotiation)base[index];
            }
            set
            {
                base.Insert(index, value);
            }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted.</param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, SopClassExtendedNegotiation value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(SopClassExtendedNegotiation value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(SopClassExtendedNegotiation value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(SopClassExtendedNegotiation value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(SopClassExtendedNegotiation value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<SopClassExtendedNegotiations>");
            foreach (SopClassExtendedNegotiation sopClassExtendedNegotiation in this)
            {
                sopClassExtendedNegotiation.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</SopClassExtendedNegotiations>");

            return true;
        }
    }

    /// <summary>
    /// The SOP Class Extended Negotiation allows, at Association establishment, 
    /// peer DICOM AEs to exchange application information defined by specific 
    /// Service Class specifications. 
    /// </summary>
    /// <remarks>
    /// This is an optional feature that various Service Classes may or may not choose to support.
    /// </remarks>
    public class SopClassExtendedNegotiation : PduItem
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SopClassExtendedNegotiation()
        {
        }

        /// <summary>
        /// Specific constructor.
        /// </summary>
        /// <param name="sopClassUid">Service Object Pair Class Unique Identifier</param>
        /// <param name="serviceClassApplicationInformation">Service Class Application Information</param>
        public SopClassExtendedNegotiation(
            System.String sopClassUid,
            System.Byte[] serviceClassApplicationInformation)
        {
            this.SopClassUid = sopClassUid;
            this.ServiceClassApplicationInformation = serviceClassApplicationInformation;
        }

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.SOP_CLASS_EXTENDED_NEGOTIATION"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.SOP_CLASS_EXTENDED_NEGOTIATION;
            }
        }

        /// <summary>
        /// The SOP Class or Meta SOP Class identifier.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public string SopClassUid
        {
            get
            {
                return _SopClassUid;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _SopClassUid = value;
            }
        }
        private string _SopClassUid;

        /// <summary>
        /// This field shall contain the application information specific to
        /// the Service Class specification identified by the SOP-classuid.
        /// </summary>
        /// <remarks>
        /// The semantics and value of this field is defined in the identified 
        /// Service Class specification.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public System.Byte[] ServiceClassApplicationInformation
        {
            get
            {
                return _ServiceClassApplicationInformation;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _ServiceClassApplicationInformation = value;
            }
        }
        private System.Byte[] _ServiceClassApplicationInformation;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            string applicationInformation = string.Empty;
            foreach (System.Byte appInfo in ServiceClassApplicationInformation)
            {
                string appInfoByte = appInfo.ToString("X");
                while (appInfoByte.Length < 2)
                {
                    appInfoByte = "0" + appInfoByte;
                }

                applicationInformation += appInfoByte;
            }
            streamWriter.WriteLine("<SopClassExtendedNegotiation Uid=\"{0}\" AppInfo=\"{1}\"></SopClassExtendedNegotiation>", SopClassUid, applicationInformation);

            return true;
        }
    }

    /// <summary>
    /// Type safe ScpScuRoleSelectionList
    /// </summary>
    public sealed class ScpScuRoleSelectionList :
        DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScpScuRoleSelectionList() { }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new ScpScuRoleSelection this[int index]
        {
            get
            {
                return (ScpScuRoleSelection)base[index];
            }
            set
            {
                base.Insert(index, value);
            }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted.</param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, ScpScuRoleSelection value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(ScpScuRoleSelection value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(ScpScuRoleSelection value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(ScpScuRoleSelection value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(ScpScuRoleSelection value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<ScpScuRoleSelections>");
            foreach (ScpScuRoleSelection scpScuRoleSelection in this)
            {
                scpScuRoleSelection.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</ScpScuRoleSelections>");

            return true;
        }
    }

    /// <summary>
    /// The SCP/SCU role selection negotiation allows peer AEs to 
    /// negotiate the roles in which they will serve for 
    /// each SOP Class or Meta SOP Class supported on the Association.
    /// </summary>
    /// <remarks>
    /// This negotiation is optional.
    /// </remarks>
    public class ScpScuRoleSelection : PduItem
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>Required by Xml serialization.</remarks>
        public ScpScuRoleSelection()
        {
            return;
        }

        /// <summary>
        /// Specific constructor.
        /// </summary>
        /// <param name="sopClassUid">Service Object Pair Class Unique Identifier.</param>
        /// <param name="scpRole">Service Class Provider Role.</param>
        /// <param name="scuRole">Service Class User Role.</param>
        public ScpScuRoleSelection(
            System.String sopClassUid,
            System.Byte scpRole,
            System.Byte scuRole)
        {
            this.SopClassUid = sopClassUid;
            this.ScpRole = scpRole;
            this.ScuRole = scuRole;
            return;
        }

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.SCP_SCU_ROLE_SELECTION"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.SCP_SCU_ROLE_SELECTION;
            }
        }

        /// <summary>
        /// This variable field shall contain the 
        /// SOP Class UID or Meta SOP Class UID which may be used to identify the
        /// corresponding Abstract Syntax for which this Sub-Item pertains.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public string SopClassUid
        {
            get
            {
                return _SopClassUid;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _SopClassUid = value;
            }
        }
        private string _SopClassUid = string.Empty;

        /// <summary>
        /// This byte field shall contain the SCU-role.
        /// </summary>
        public System.Byte ScuRole
        {
            get
            {
                return _ScuRole;
            }
            set
            {
                _ScuRole = value;
            }
        }
        private System.Byte _ScuRole = ScuRole_Requestor_Support_Of_The_SCU_Role;

        /// <summary>
        /// This byte field shall contain the SCP-role.
        /// </summary>
        public System.Byte ScpRole
        {
            get
            {
                return _ScpRole;
            }
            set
            {
                _ScpRole = value;
            }
        }
        private System.Byte _ScpRole = ScpRoles_Acceptor_Accept_Proposed_SCP_Role;

        /// <summary>
        /// 0
        /// </summary>
        public const System.Byte ScuRole_Requestor_Propose_Non_Support_SCU_Role = 0;
        /// <summary>
        /// 1
        /// </summary>
        public const System.Byte ScuRole_Requestor_Support_Of_The_SCU_Role = 1;
        /// <summary>
        /// 0
        /// </summary>
        public const System.Byte ScuRole_Acceptor_Reject_Proposed_SCU_Role = 0;
        /// <summary>
        /// 1
        /// </summary>
        public const System.Byte ScuRole_Acceptor_Accept_Proposed_SCU_Role = 1;

        /// <summary>
        /// 0
        /// </summary>
        public const System.Byte ScpRoles_Requestor_Propose_Non_Support_SCP_Role = 0;
        /// <summary>
        /// 1
        /// </summary>
        public const System.Byte ScpRoles_Requestor_Propose_Support_Of_The_SCP_Role = 1;
        /// <summary>
        /// 0
        /// </summary>
        public const System.Byte ScpRoles_Acceptor_Reject_Proposed_SCP_Role = 0;
        /// <summary>
        /// 1
        /// </summary>
        public const System.Byte ScpRoles_Acceptor_Accept_Proposed_SCP_Role = 1;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<ScpScuRoleSelection Uid=\"{0}\" ScpRole=\"{1}\" ScuRole=\"{2}\"></ScpScuRoleSelection>", SopClassUid, ScpRole.ToString(), ScuRole.ToString());

            return true;
        }
    }

    /// <summary>
    /// The Asynchronous Operations Window is used to negotiate the maximum number of outstanding
    /// operation or sub-operation requests (i.e. command requests) for each direction.
    /// </summary>
    /// <remarks>
    /// The synchronous operations mode is the default mode and shall be support by all DICOM AEs.
    /// </remarks>
    /// <remarks>
    /// This negotiation is optional.
    /// </remarks>
    public class AsynchronousOperationsWindow : PduItem
    {
        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.ASYNCHRONOUS_OPERATIONS_WINDOW"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.ASYNCHRONOUS_OPERATIONS_WINDOW;
            }
        }

        /// <summary>
        /// Maximum Number of Operations Invoked.
        /// </summary>
        public System.UInt16 MaximumNumberOperationsInvoked
        {
            get
            {
                return _MaximumNumberOperationsInvoked;
            }
            set
            {
                _MaximumNumberOperationsInvoked = value;
            }
        }
        private System.UInt16 _MaximumNumberOperationsInvoked = 0;

        /// <summary>
        /// Maximum Number of Operations Performed.
        /// </summary>
        public System.UInt16 MaximumNumberOperationsPerformed
        {
            get
            {
                return _MaximumNumberOperationsPerformed;
            }
            set
            {
                _MaximumNumberOperationsPerformed = value;
            }
        }
        private System.UInt16 _MaximumNumberOperationsPerformed = 0;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<AsynchronousOperationsWindow Invoked=\"{0}\" Peformed=\"{1}\"></AsynchronousOperationsWindow>", MaximumNumberOperationsInvoked.ToString(), MaximumNumberOperationsPerformed.ToString());

            return true;
        }
    }

    /// <summary>
    /// The User Identity Negotiation is used to pass the username, password or kerberos service ticket
    /// across the assocation.
    /// </summary>
    /// <remarks>
    /// This negotiation is optional.
    /// </remarks>
    public class UserIdentityNegotiation : PduItem
    {
        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.USER_IDENTITY_NEGOTIATION"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.USER_IDENTITY_NEGOTIATION;
            }
        }

        /// <summary>
        /// User Identity Type.
        /// </summary>
        public System.Byte UserIdentityType
        {
            get
            {
                return _UserIdentityType;
            }
            set
            {
                _UserIdentityType = value;
            }
        }
        private System.Byte _UserIdentityType = 0;

        /// <summary>
        /// Positive Response Requested.
        /// </summary>
        public System.Byte PositiveResponseRequested
        {
            get
            {
                return _PositiveResponseRequested;
            }
            set
            {
                _PositiveResponseRequested = value;
            }
        }
        private System.Byte _PositiveResponseRequested = 0;

        /// <summary>
        /// Primary Field.
        /// </summary>
        public System.String PrimaryField
        {
            get
            {
                return _PrimaryField;
            }
            set
            {
                _PrimaryField = value;
            }
        }
        private System.String _PrimaryField = null;

        /// <summary>
        /// Secondary Field.
        /// </summary>
        public System.String SecondaryField
        {
            get
            {
                return _SecondaryField;
            }
            set
            {
                _SecondaryField = value;
            }
        }
        private System.String _SecondaryField = null;

        /// <summary>
        /// Server Response.
        /// </summary>
        public System.String ServerResponse
        {
            get
            {
                return _ServerResponse;
            }
            set
            {
                _ServerResponse = value;
            }
        }
        private System.String _ServerResponse = null;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if ((PrimaryField != null) &&
                (PrimaryField.Length > 0))
            {
                streamWriter.WriteLine("<UserIdentityNegotiation UserIdentityType=\"{0}\" PositiveResponseRequested=\"{1}\" PrimaryField=\"{2}\" SecondaryField=\"{3}\"></UserIdentityNegotiation>",
                                    UserIdentityType.ToString(),
                                    PositiveResponseRequested.ToString(),
                                    PrimaryField,
                                    SecondaryField);
            }
            else
            {
                streamWriter.WriteLine("<UserIdentityNegotiation ServerResponse=\"{0}\"></UserIdentityNegotiation>",
                                    ServerResponse);
            }
            return true;
        }
    }

    /// <summary>
    /// The implementation identification notification allows implementations of 
    /// communicating AEs to identify each other at Association establishment time. 
    /// </summary>
    /// <remarks>
    /// <p>
    /// It is intended to provide respective 
    /// (each network node knows the other's implementation identity) 
    /// and non-ambiguous identification in the event of communication problems 
    /// encountered between two nodes.
    /// </p>
    /// <p>
    /// This negotiation is required.
    /// </p>
    /// <p>
    /// Implementation identification relies on two pieces of information:<br></br>
    /// Implementation Class UID (required)<br></br>
    /// Implementation Version Name (optional)
    /// </p>
    /// <p>
    /// Only one Implementation Version Name Sub-Item shall be present in the 
    /// User Data Item of the A-ASSOCIATE-RQ.
    /// </p>
    /// <p>
    /// Only one Implementation Version Name Sub-Item shall be present in the 
    /// User Data Item of the A-ASSOCIATE-AC.
    /// </p>
    /// </remarks>
    public class ImplementationVersionName : PduItem
    {

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.IMPLEMENTATION_VERSION_NAME"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.IMPLEMENTATION_VERSION_NAME;
            }
        }

        /// <summary>
        /// This variable field shall contain the Implementation-versionname
        /// of the Association-requester.
        /// This variable field shall contain the Implementation-versionname 
        /// of the Association-acceptor.                                                                                                              646:1990 (basic G0 set ) characters.
        /// </summary>
        /// <remarks>
        /// It shall be encoded as a string of 1 to 16 ISO 646:1990 (basic G0 set ) characters.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _Name = value;
            }
        }
        private string _Name = string.Empty;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<ImplementationVersionName>{0}</ImplementationVersionName>", Name);

            return true;
        }
    }

    /// <summary>
    /// The implementation identification notification allows implementations of 
    /// communicating AEs to identify each other at Association establishment time. 
    /// </summary>
    /// <remarks>
    /// <p>
    /// It is intended to provide respective 
    /// (each network node knows the other's implementation identity) 
    /// and non-ambiguous identification in the event of communication problems 
    /// encountered between two nodes.
    /// </p>
    /// <p>
    /// This negotiation is required.
    /// </p>
    /// <p>
    /// Implementation identification relies on two pieces of information:<br></br>
    /// Implementation Class UID (required)<br></br>
    /// Implementation Version Name (optional)
    /// </p>
    /// <p>
    /// Only one Implementation Class UID Sub-Item shall be present in the User Data Item 
    /// of the A-ASSOCIATE-RQ.
    /// </p>
    /// </remarks>
    public class ImplementationClassUid : PduItem
    {

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.IMPLEMENTATION_UID"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.IMPLEMENTATION_UID;
            }
        }

        /// <summary>
        /// This variable field shall contain the Implementation-class-uid
        /// of the Association-requester as defined in Section D.3.3.2 of
        /// this part.
        /// </summary>
        /// <remarks>
        /// The Implementation-class-uid field is structured as a UID as defined in PS 3.5.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public string UID
        {
            get
            {
                return _UID;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _UID = value;
            }
        }
        private string _UID = "Implementation Class UID Not Specified";

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<ImplementationClassUid>{0}</ImplementationClassUid>", UID);

            return true;
        }
    }

    /// <summary>
    /// The Maximum Length notification allows communicating AEs to limit the 
    /// size of the data for each P-DATA indication. 
    /// </summary>
    /// <remarks>
    /// Each DICOM AE defines the maximum PDU size it can receive on this Association. 
    /// Therefore, different maximum lengths can be specified for each direction of 
    /// data flow on an Association.
    /// </remarks>
    public class MaximumLength : PduItem
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MaximumLength()
        {
            return;
        }

        /// <summary>
        /// Specific constructor.
        /// </summary>
        /// <param name="maximumLengthReceived">Maximum Length Received.</param>
        private MaximumLength(System.UInt32 maximumLengthReceived)
        {
            this.MaximumLengthReceived = maximumLengthReceived;
            return;
        }

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.MAXIMUM_LENGTH"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.MAXIMUM_LENGTH;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="MaximumLength"/> is read-only.
        /// </summary>
        /// <value><see langword="true"/> if the <see cref="MaximumLength"/> is read-only; otherwise, <see langword="false"/>.</value>
        public System.Boolean IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
        }
        private System.Boolean _IsReadOnly = false;

        /// <summary>
        /// Returns a <see cref="MaximumLength"/> wrapper that is read-only.
        /// </summary>
        /// <param name="maximumLength">The <see cref="MaximumLength"/> to wrap.</param>
        /// <returns>A read-only <see cref="MaximumLength"/> wrapper around <c>maximumLength</c>.</returns>
        private static MaximumLength ReadOnly(MaximumLength maximumLength)
        {
            maximumLength._IsReadOnly = true;
            return maximumLength;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="MaximumLength"/> is read-only.
        /// </summary>
        /// <value><see langword="true"/> if the <see cref="MaximumLength"/> is read-only; otherwise, <see langword="false"/>.</value>
        /// <exception cref="System.NotSupportedException">Set is not supported if <c>ReadOnly</c>.</exception>
        public System.UInt32 MaximumLengthReceived
        {
            get
            {
                return _MaximumLengthReceived;
            }
            set
            {
                if (_IsReadOnly) throw new System.NotSupportedException();
                _MaximumLengthReceived = value;
            }
        }
        private System.UInt32 _MaximumLengthReceived = 0;

        /// <summary>
        /// Predefined maximum length of size: unlimited.
        /// </summary>
        public static readonly MaximumLength Unlimited = MaximumLength.ReadOnly(new MaximumLength(0));
        /// <summary>
        /// Predefined maximum length of size: 16384 bytes.
        /// </summary>
        public static readonly MaximumLength K16 = MaximumLength.ReadOnly(new MaximumLength(16384));
        /// <summary>
        /// Predefined maximum length of size: 32768 bytes.
        /// </summary>
        public static readonly MaximumLength K32 = MaximumLength.ReadOnly(new MaximumLength(32768));
        /// <summary>
        /// Predefined maximum length of size: 65536 bytes.
        /// </summary>
        public static readonly MaximumLength K64 = MaximumLength.ReadOnly(new MaximumLength(65536));
        /// <summary>
        /// Predefined maximum length of size: 131072 bytes.
        /// </summary>
        public static readonly MaximumLength K128 = MaximumLength.ReadOnly(new MaximumLength(131072));
        /// <summary>
        /// Predefined maximum length of size: 262144 bytes.
        /// </summary>
        public static readonly MaximumLength K256 = MaximumLength.ReadOnly(new MaximumLength(262144));
        /// <summary>
        /// Predefined maximum length of size: 524288 bytes.
        /// </summary>
        public static readonly MaximumLength K512 = MaximumLength.ReadOnly(new MaximumLength(524288));
        /// <summary>
        /// Predefined maximum length of size: 1048576 bytes.
        /// </summary>
        public static readonly MaximumLength M1 = MaximumLength.ReadOnly(new MaximumLength(1048576));
        /// <summary>
        /// Predefined maximum length of size: 2097152 bytes.
        /// </summary>
        public static readonly MaximumLength M2 = MaximumLength.ReadOnly(new MaximumLength(2097152));
        /// <summary>
        /// Predefined maximum length of size: default = 16384 bytes.
        /// </summary>
        public static readonly MaximumLength Default = MaximumLength.K16;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<MaximumLengthReceived>{0}</MaximumLengthReceived>", MaximumLengthReceived.ToString());

            return true;
        }
    }

    /// <summary>
    /// Peer DICOM AEs negotiate, at Association establishment, 
    /// a number of features related to the Dimse protocol by using the 
    /// ACSE User Information Item of the A-ASSOCIATE request.
    /// </summary>
    public class UserInformation : PduItem
    {

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.USER_INFORMATION"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.USER_INFORMATION;
            }
        }

        /// <summary>
        /// Maximum length sub-item
        /// </summary>
        /// <remarks>
        /// Required negotiation sub-item. Created by default.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public MaximumLength MaximumLength
        {
            get
            {
                return _MaximumLength;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _MaximumLength = value;
            }
        }
        private MaximumLength _MaximumLength = new MaximumLength();

        /// <summary>
        /// Implementation class unique identifier sub-item
        /// </summary>
        /// <remarks>
        /// Required negotiation sub-item. Created by default.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public ImplementationClassUid ImplementationClassUid
        {
            get
            {
                return _ImplementationClassUid;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _ImplementationClassUid = value;
            }
        }
        private ImplementationClassUid _ImplementationClassUid = new ImplementationClassUid();

        /// <summary>
        /// Implementation version name sub-item
        /// </summary>
        /// <remarks>
        /// Optional negotiation sub-item. NOT created by default.
        /// </remarks>
        public ImplementationVersionName ImplementationVersionName
        {
            get
            {
                return _ImplementationVersionName;
            }
            set
            {
                _ImplementationVersionName = value;
            }
        }
        private ImplementationVersionName _ImplementationVersionName = new ImplementationVersionName();

        /// <summary>
        /// Asynchronous operations window sub-item
        /// </summary>
        /// <remarks>
        /// Optional negotiation sub-item. NOT created by default.
        /// </remarks>
        public AsynchronousOperationsWindow AsynchronousOperationsWindow
        {
            get
            {
                return _AsynchronousOperationsWindow;
            }
            set
            {
                _AsynchronousOperationsWindow = value;
            }
        }
        private AsynchronousOperationsWindow _AsynchronousOperationsWindow = null;

        /// <summary>
        /// Scp/Scu role selection sub-item
        /// </summary>
        /// <remarks>
        /// Optional negotiation sub-item. NOT created by default.
        /// </remarks>
        public ScpScuRoleSelectionList ScpScuRoleSelections
        {
            get
            {
                return _ScpScuRoleSelections;
            }
            set
            {
                _ScpScuRoleSelections = value;
            }
        }
        private ScpScuRoleSelectionList _ScpScuRoleSelections = null;

        /// <summary>
        /// Add an array of <see cref="DvtkData.Dul.ScpScuRoleSelection"/> items to <see cref="DvtkData.Dul.ScpScuRoleSelectionList"/>
        /// </summary>
        /// <param name="scpScuRoleSelections">Array of <see cref="DvtkData.Dul.ScpScuRoleSelection"/> items to add.</param>
        public void AddScpScuRoleSelections(
            params ScpScuRoleSelection[] scpScuRoleSelections)
        {
            if (_ScpScuRoleSelections == null) _ScpScuRoleSelections = new ScpScuRoleSelectionList();
            foreach (ScpScuRoleSelection scpScuRoleSelection in scpScuRoleSelections)
            {
                _ScpScuRoleSelections.Add(scpScuRoleSelection);
            }
        }

        /// <summary>
        /// Sop clas Extended negotation sub-item
        /// </summary>
        /// <remarks>
        /// Optional negotiation sub-item. NOT created by default.
        /// </remarks>
        public SopClassExtendedNegotiationList SopClassExtendedNegotiations
        {
            get
            {
                return _SopClassExtendedNegotiations;
            }
            set
            {
                _SopClassExtendedNegotiations = value;
            }
        }
        private SopClassExtendedNegotiationList _SopClassExtendedNegotiations = null;

        /// <summary>
        /// Add an array of <see cref="DvtkData.Dul.SopClassExtendedNegotiation"/> items to <see cref="DvtkData.Dul.SopClassExtendedNegotiationList"/>
        /// </summary>
        /// <param name="sopClassExtendedNegotiations">Array of <see cref="DvtkData.Dul.SopClassExtendedNegotiation"/> items to add.</param>
        public void AddSopClassExtendedNegotiations(
            params SopClassExtendedNegotiation[] sopClassExtendedNegotiations)
        {
            if (_SopClassExtendedNegotiations == null)
                _SopClassExtendedNegotiations = new SopClassExtendedNegotiationList();
            foreach (SopClassExtendedNegotiation sopClassExtendedNegotiation in sopClassExtendedNegotiations)
            {
                _SopClassExtendedNegotiations.Add(sopClassExtendedNegotiation);
            }
        }

        /// <summary>
        /// User identity negotiation sub-item
        /// </summary>
        /// <remarks>
        /// Optional negotiation sub-item. NOT created by default.
        /// </remarks>
        public UserIdentityNegotiation UserIdentityNegotiation
        {
            get
            {
                return _UserIdentityNegotiation;
            }
            set
            {
                _UserIdentityNegotiation = value;
            }
        }
        private UserIdentityNegotiation _UserIdentityNegotiation = null;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<UserInformation>");
            MaximumLength.DvtDetailToXml(streamWriter, level);
            ImplementationClassUid.DvtDetailToXml(streamWriter, level);
            if (ImplementationVersionName != null)
            {
                ImplementationVersionName.DvtDetailToXml(streamWriter, level);
            }
            if (AsynchronousOperationsWindow != null)
            {
                AsynchronousOperationsWindow.DvtDetailToXml(streamWriter, level);
            }
            if (ScpScuRoleSelections != null)
            {
                ScpScuRoleSelections.DvtDetailToXml(streamWriter, level);
            }
            if (SopClassExtendedNegotiations != null)
            {
                SopClassExtendedNegotiations.DvtDetailToXml(streamWriter, level);
            }
            if (UserIdentityNegotiation != null)
            {
                UserIdentityNegotiation.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</UserInformation>");

            return true;
        }
    }

    /// <summary>
    /// Specific AcceptedPresentationContext PduItem.
    /// </summary>
    public class AcceptedPresentationContext : PduItem
    {

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.AC_PRESENTATION_CONTEXT"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.AC_PRESENTATION_CONTEXT;
            }
        }

        /// <summary>
        /// Identifier.
        /// </summary>
        public System.Byte ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        private System.Byte _ID = 0;

        /// <summary>
        /// Predefined Result: Acceptance = 0
        /// </summary>
        public const System.Byte Result_Acceptance = 0;
        /// <summary>
        /// Predefined Result: User Rejection = 1
        /// </summary>
        public const System.Byte Result_User_Rejection = 1;
        /// <summary>
        /// Predefined Result: No Reason Provide Rejection = 2
        /// </summary>
        public const System.Byte Result_No_Reason_Provider_Rejection = 2;
        /// <summary>
        /// Predefined Result: Abstract Syntax Not Supported Provider Rejection = 3
        /// </summary>
        public const System.Byte Result_Abstract_Syntax_Not_Supported_Provider_Rejection = 3;
        /// <summary>
        /// Predefined Result: Transfer Syntaxes Not Supported Provider Rejection = 4
        /// </summary>
        public const System.Byte Result_Transfer_Syntaxes_Not_Supported_Provider_Rejection = 4;

        /// <summary>
        /// Result.
        /// </summary>
        public System.Byte Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value;
            }
        }
        private System.Byte _Result = 0;

        /// <summary>
        /// Abstract syntax.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public AbstractSyntax AbstractSyntax
        {
            get
            {
                return _AbstractSyntax;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _AbstractSyntax = value;
            }
        }
        private AbstractSyntax _AbstractSyntax = new AbstractSyntax();

        /// <summary>
        /// Transfer syntax.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public TransferSyntax TransferSyntax
        {
            get
            {
                return _TransferSyntax;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _TransferSyntax = value;
            }
        }
        private TransferSyntax _TransferSyntax = new TransferSyntax();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<PresentationContext Id=\"{0}\" Result=\"{1}\" AbstractSyntaxName=\"{2}\">", ID.ToString(), Result.ToString(), AbstractSyntax.UID);
            streamWriter.WriteLine("<TransferSyntax>{0}</TransferSyntax>", TransferSyntax.UID);
            streamWriter.WriteLine("</PresentationContext>");

            return true;
        }
    }

    /// <summary>
    /// DICOM AEs use the Abstract Syntax Name to identify and negotiate which 
    /// SOP Classes and related options are supported on a specific Association. 
    /// </summary>
    /// <remarks>
    /// Each Abstract Syntax shall be identified by an Abstract Syntax Name in the form of a UID. 
    /// </remarks>
    /// <remarks>
    /// Abstract Syntax Names shall be defined in the Service Class Definitions specified in PS 3.4.
    /// </remarks>
    /// <remarks>
    /// Each Abstract Syntax Name defined shall have a value of either:<br></br>
    /// a Service-Object Pair Class UID<br></br>
    /// a Meta Service-Object Pair Group UID
    /// </remarks>
    public class AbstractSyntax : PduItem
    {
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this._UID.GetHashCode();
        }
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare with this instance, or a null reference.</param>
        /// <returns><see langword="true"/> if other is an instance of <see cref="AbstractSyntax"/> and equals the value of this instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(System.Object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;
            AbstractSyntax abstractSyntax = (AbstractSyntax)obj;
            return (this._UID == abstractSyntax._UID);
        }

        static AbstractSyntax()
        {
            #region assign literal strings to abstract syntaxes
            AbstractSyntaxNames = new Hashtable();
            AbstractSyntaxNames.Add(Verification, "Verification");
            AbstractSyntaxNames.Add(Media_Storage_Directory_Storage, "Media Storage Directory Storage");
            AbstractSyntaxNames.Add(Basic_Study_Content_Notification, "Basic Study Content Notification");
            AbstractSyntaxNames.Add(Storage_Commitment_Push_Model, "Storage Commitment Push Model");
            AbstractSyntaxNames.Add(Detached_Patient_Management, "Detached Patient Management");
            AbstractSyntaxNames.Add(Detached_Patient_Management_Meta, "Detached Patient Management Meta");
            AbstractSyntaxNames.Add(Detached_Visit_Management, "Detached Visit Management");
            AbstractSyntaxNames.Add(Detached_Study_Management, "Detached Study Management");
            AbstractSyntaxNames.Add(Study_Component_Management, "Study Component Management");
            AbstractSyntaxNames.Add(Modality_Performed_Procedure_Step, "Modality Performed Procedure Step");
            AbstractSyntaxNames.Add(Modality_Performed_Procedure_Step_Retrieve, "Modality Performed Procedure Step Retrieve");
            AbstractSyntaxNames.Add(Modality_Performed_Procedure_Step_Notification, "Modality Performed Procedure Step Notification");
            AbstractSyntaxNames.Add(Detached_Results_Management, "Detached Results Management");
            AbstractSyntaxNames.Add(Detached_Results_Management_Meta, "Detached Results Management Meta");
            AbstractSyntaxNames.Add(Detached_Study_Management_Meta, "Detached Study Management Meta");
            AbstractSyntaxNames.Add(Detached_Interpretation_Management, "Detached Interpretation Management");
            AbstractSyntaxNames.Add(Basic_Film_Session, "Basic Film Session");
            AbstractSyntaxNames.Add(Basic_Film_Box, "Basic Film Box");
            AbstractSyntaxNames.Add(Basic_Grayscale_Image_Box, "Basic Grayscale Image Box");
            AbstractSyntaxNames.Add(Basic_Color_Image_Box, "Basic Color Image Box");
            AbstractSyntaxNames.Add(Basic_Grayscale_Print_Management_Meta, "Basic Grayscale Print Management Meta");
            AbstractSyntaxNames.Add(Print_Job, "Print Job");
            AbstractSyntaxNames.Add(Basic_Annotation_Box, "Basic Annotation Box");
            AbstractSyntaxNames.Add(Printer, "Printer");
            AbstractSyntaxNames.Add(Printer_Configuration_Retrieval, "Printer Configuration Retrieval");
            AbstractSyntaxNames.Add(Basic_Color_Print_Management_Meta, "Basic Color Print Management Meta");
            AbstractSyntaxNames.Add(Referenced_Color_Print, "Referenced Color Print");
            AbstractSyntaxNames.Add(Presentation_LUT, "Presentation LUT");
            AbstractSyntaxNames.Add(Basic_Print_Image_Overlay_Box, "Basic Print Image Overlay Box");
            AbstractSyntaxNames.Add(Print_Queue_Management, "Print Queue Management");
            AbstractSyntaxNames.Add(Stored_Print_Storage, "Stored Print Storage");
            AbstractSyntaxNames.Add(Hardcopy_Grayscale_Image_Storage, "Hardcopy Grayscale Image Storage");
            AbstractSyntaxNames.Add(Hardcopy_Color_Image_Storage, "Hardcopy Color Image Storage");
            AbstractSyntaxNames.Add(Pull_Print_Request, "Pull Print Request");
            AbstractSyntaxNames.Add(Pull_Stored_Print_Management_Meta, "Pull Stored Print Management Meta");
            AbstractSyntaxNames.Add(Computed_Radiography_Image_Storage, "Computed Radiography Image Storage");
            AbstractSyntaxNames.Add(Digital_XRay_Image_Storage_For_Presentation, "Digital XRay Image Storage For Presentation");
            AbstractSyntaxNames.Add(Digital_XRay_Image_Storage_For_Processing, "Digital XRay Image Storage For Processing");
            AbstractSyntaxNames.Add(Digital_Mammography_XRay_Image_Storage_For_Presentation, "Digital Mammography XRay Image Storage For Presentation");
            AbstractSyntaxNames.Add(Digital_Mammography_XRay_Image_Storage_For_Processing, "Digital Mammography XRay Image Storage For Processing");
            AbstractSyntaxNames.Add(Digital_Intraoral_XRay_Image_Storage_For_Presentation, "Digital Intraoral XRay Image Storage For Presentation");
            AbstractSyntaxNames.Add(Digital_Intraoral_XRay_Image_Storage_For_Processing, "Digital Intraoral XRay Image Storage For Processing");
            AbstractSyntaxNames.Add(CT_Image_Storage, "CT Image Storage");
            AbstractSyntaxNames.Add(Ultrasound_Multiframe_Image_Storage, "Ultrasound Multiframe Image Storage");
            AbstractSyntaxNames.Add(MR_Image_Storage, "MR Image Storage");
            AbstractSyntaxNames.Add(Enhanced_MR_Image_Storage, "Enhanced MR Image Storage");
            AbstractSyntaxNames.Add(MR_Spectroscopy_Storage, "MR Spectroscopy Storage");
            AbstractSyntaxNames.Add(Ultrasound_Image_Storage, "Ultrasound Image Storage");
            AbstractSyntaxNames.Add(Secondary_Capture_Image_Storage, "Secondary Capture Image Storage");
            AbstractSyntaxNames.Add(Multiframe_Single_Bit_Secondary_Capture_Image_Storage, "Multiframe Single Bit Secondary Capture Image Storage");
            AbstractSyntaxNames.Add(Multiframe_Grayscale_Byte_Secondary_Capture_Image_Storage, "Multiframe Grayscale Byte Secondary Capture Image Storage");
            AbstractSyntaxNames.Add(Multiframe_Grayscale_Word_Secondary_Capture_Image_Storage, "Multiframe Grayscale Word Secondary Capture Image Storage");
            AbstractSyntaxNames.Add(Multiframe_True_Color_Secondary_Capture_Image_Storage, "Multiframe True Color Secondary Capture Image Storage");
            AbstractSyntaxNames.Add(Standalone_Overlay_Storage, "Standalone Overlay Storage");
            AbstractSyntaxNames.Add(Standalone_Curve_Storage, "Standalone Curve Storage");
            AbstractSyntaxNames.Add(Twelve_lead_ECG_Waveform_Storage, "Twelve lead ECG Waveform Storage");
            AbstractSyntaxNames.Add(General_ECG_Waveform_Storage, "General ECG Waveform Storage");
            AbstractSyntaxNames.Add(Ambulatory_ECG_Waveform_Storage, "Ambulatory ECG Waveform Storage");
            AbstractSyntaxNames.Add(Hemodynamic_Waveform_Storage, "Hemodynamic Waveform Storage");
            AbstractSyntaxNames.Add(Cardiac_Electrophysiology_Waveform_Storage, "Cardiac Electrophysiology Waveform Storage");
            AbstractSyntaxNames.Add(Basic_Voice_Audio_Waveform_Storage, "Basic Voice Audio Waveform Storage");
            AbstractSyntaxNames.Add(Standalone_Modality_LUT_Storage, "Standalone Modality LUT Storage");
            AbstractSyntaxNames.Add(Standalone_VOI_LUT_Storage, "Standalone VOI LUT Storage");
            AbstractSyntaxNames.Add(Grayscale_Softcopy_Presentation_State_Storage, "Grayscale Softcopy Presentation State Storage");
            AbstractSyntaxNames.Add(XRay_Angiographic_Image_Storage, "XRay Angiographic Image Storage");
            AbstractSyntaxNames.Add(XRay_Radiofluoroscopic_Image_Storage, "XRay Radiofluoroscopic Image Storage");
            AbstractSyntaxNames.Add(Nuclear_Medicine_Image_Storage, "Nuclear Medicine Image Storage");
            AbstractSyntaxNames.Add(Raw_Data_Storage, "Raw Data Storage");
            AbstractSyntaxNames.Add(VL_Endoscopic_Image_Storage, "VL Endoscopic Image Storage");
            AbstractSyntaxNames.Add(VL_Microscopic_Image_Storage, "VL Microscopic Image Storage");
            AbstractSyntaxNames.Add(VL_Slide_Coordinates_Microscopic_Image_Storage, "VL Slide Coordinates Microscopic Image Storage");
            AbstractSyntaxNames.Add(VL_Photographic_Image_Storage, "VL Photographic Image Storage");
            AbstractSyntaxNames.Add(Basic_Text_SR, "Basic Text SR");
            AbstractSyntaxNames.Add(Enhanced_SR, "Enhanced SR");
            AbstractSyntaxNames.Add(Comprehensive_SR, "Comprehensive SR");
            AbstractSyntaxNames.Add(Mammography_CAD_SR, "Mammography CAD SR");
            AbstractSyntaxNames.Add(Key_Object_Selection_Document, "Key Object Selection Document");
            AbstractSyntaxNames.Add(Chest_CAD_SR, "Chest CAD SR");
            AbstractSyntaxNames.Add(Positron_Emission_Tomography_Image_Storage, "Positron Emission Tomography Image Storage");
            AbstractSyntaxNames.Add(Standalone_PET_Curve_Storage, "Standalone PET Curve Storage");
            AbstractSyntaxNames.Add(RT_Image_Storage, "RT Image Storage");
            AbstractSyntaxNames.Add(RT_Dose_Storage, "RT Dose Storage");
            AbstractSyntaxNames.Add(RT_Structure_Set_Storage, "RT Structure Set Storage");
            AbstractSyntaxNames.Add(RT_Beams_Treatment_Record_Storage, "RT Beams Treatment Record Storage");
            AbstractSyntaxNames.Add(RT_Plan_Storage, "RT Plan Storage");
            AbstractSyntaxNames.Add(RT_Brachy_Treatment_Record_Storage, "RT Brachy Treatment Record Storage");
            AbstractSyntaxNames.Add(RT_Treatment_Summary_Record_Storage, "RT Treatment Summary Record Storage");
            AbstractSyntaxNames.Add(Patient_Root_Query_Retrieve_Information_Model_FIND, "Patient Root Query Retrieve Information Model FIND");
            AbstractSyntaxNames.Add(Patient_Root_Query_Retrieve_Information_Model_MOVE, "Patient Root Query Retrieve Information Model MOVE");
            AbstractSyntaxNames.Add(Patient_Root_Query_Retrieve_Information_Model_GET, "Patient Root Query Retrieve Information Model GET");
            AbstractSyntaxNames.Add(Study_Root_Query_Retrieve_Information_Model_FIND, "Study Root Query Retrieve Information Model FIND");
            AbstractSyntaxNames.Add(Study_Root_Query_Retrieve_Information_Model_MOVE, "Study Root Query Retrieve Information Model MOVE");
            AbstractSyntaxNames.Add(Study_Root_Query_Retrieve_Information_Model_GET, "Study Root Query Retrieve Information Model GET");
            AbstractSyntaxNames.Add(Patient_Study_Only_Query_Retrieve_Information_Model_FIND, "Patient Study Only Query Retrieve Information Model FIND");
            AbstractSyntaxNames.Add(Patient_Study_Only_Query_Retrieve_Information_Model_MOVE, "Patient Study Only Query Retrieve Information Model MOVE");
            AbstractSyntaxNames.Add(Patient_Study_Only_Query_Retrieve_Information_Model_GET, "Patient Study Only Query Retrieve Information Model GET");
            AbstractSyntaxNames.Add(Modality_Worklist_Information_Model_FIND, "Modality Worklist Information Model FIND");
            AbstractSyntaxNames.Add(General_Purpose_Worklist_Information_Model_FIND, "General Purpose Worklist Information Model FIND");
            AbstractSyntaxNames.Add(General_Purpose_Scheduled_Procedure_Step, "General Purpose Scheduled Procedure Step");
            AbstractSyntaxNames.Add(General_Purpose_Performed_Procedure_Step, "General Purpose Performed Procedure Step");
            AbstractSyntaxNames.Add(General_Purpose_Worklist_Management_Meta, "General Purpose Worklist Management Meta");
            /*
            //
            // Retired SOP Classes
            //
            AbstractSyntaxNames.Add(Storage_Commitment_Pull_Model                           ,"Storage Commitment Pull Model");
            AbstractSyntaxNames.Add(Referenced_Image_Box_SOP                                ,"Referenced Image Box SOP");
            AbstractSyntaxNames.Add(Referenced_Grayscale_Print_Management_Meta              ,"Referenced Grayscale Print Management Meta");
            AbstractSyntaxNames.Add(Referenced_Color_Print_Management_Meta                  ,"Referenced Color Print Management Meta");
            AbstractSyntaxNames.Add(Image_Overlay_Box                                       ,"Image Overlay Box");
            AbstractSyntaxNames.Add(Ultrasound_Multiframe_Image_Storage                     ,"Ultrasound Multiframe Image Storage");
            AbstractSyntaxNames.Add(Nuclear_Medicine_Image_Storage                          ,"Nuclear Medicine Image Storage");
            AbstractSyntaxNames.Add(Ultrasound_Image_Storage                                ,"Ultrasound Image Storage");
            AbstractSyntaxNames.Add(XRay_Angiographic_BiPlane_Image_Storage                 ,"XRay Angiographic BiPlane Image Storage");
            AbstractSyntaxNames.Add(VL_Image_Storage                                        ,"VL Image Storage");
            AbstractSyntaxNames.Add(VL_Multiframe_Image_Storage                             ,"VL Multiframe Image Storage");
            */
            //
            // SOP Instances
            //
            AbstractSyntaxNames.Add(Storage_Commitment_Push_Model_Sop_Instance, "Storage Commitment Push Model - Sop Instance");
            AbstractSyntaxNames.Add(Printer_Sop_Instance, "Printer - Sop Instance");
            AbstractSyntaxNames.Add(Printer_Configuration_Retrieval_Sop_Instance, "Printer Configuration Retrieval - Sop Instance");
            AbstractSyntaxNames.Add(Print_Queue_Sop_Instance, "Print Queue - Sop Instance");
            /*
            //
            // Retired SOP Instances
            //
            AbstractSyntaxNames.Add(Storage_Commitment_Pull_Model                           ,"Storage Commitment Pull Model");
            */
            #endregion
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>Required by Xml serialization.</remarks>
        public AbstractSyntax() { }

        /// <summary>
        /// Specific constructor.
        /// </summary>
        /// <param name="uid">Unique identifier.</param>
        public AbstractSyntax(System.String uid)
        {
            this._UID = uid;
        }

        /// <summary>
        /// Obtains the <see cref="System.String"/> representation of this instance.
        /// </summary>
        /// <returns>The friendly name of the <see cref="AbstractSyntax"/>.</returns>
        public override string ToString()
        {
            string name = (string)AbstractSyntaxNames[this];
            if (name != null)
            {
                // return literal string
                return name;
            }
            else
            {
                // return UID string
                return this._UID;
            }
        }

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.ABSTRACT_SYNTAX"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.ABSTRACT_SYNTAX;
            }
        }

        /// <summary>
        /// Unique identifier abstract syntax
        /// </summary>
        /// <remarks>
        /// set-method is not supported! Throw
        /// Declaration of this public method is required to allow proper serialization!
        /// </remarks>
        public string UID
        {
            get
            {
                return _UID;
            }
        }
        private readonly string _UID = string.Empty;  // Only set during construction.
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.1.1
        /// </summary>
        public static readonly AbstractSyntax Verification = new AbstractSyntax("1.2.840.10008.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.1.3.10
        /// </summary>
        public static readonly AbstractSyntax Media_Storage_Directory_Storage = new AbstractSyntax("1.2.840.10008.1.3.10");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.1.9
        /// </summary>
        public static readonly AbstractSyntax Basic_Study_Content_Notification = new AbstractSyntax("1.2.840.10008.1.9");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.1.20.1
        /// </summary>
        public static readonly AbstractSyntax Storage_Commitment_Push_Model = new AbstractSyntax("1.2.840.10008.1.20.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.1.1
        /// </summary>
        public static readonly AbstractSyntax Detached_Patient_Management = new AbstractSyntax("1.2.840.10008.3.1.2.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.1.4
        /// </summary>
        public static readonly AbstractSyntax Detached_Patient_Management_Meta = new AbstractSyntax("1.2.840.10008.3.1.2.1.4");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.2.1
        /// </summary>
        public static readonly AbstractSyntax Detached_Visit_Management = new AbstractSyntax("1.2.840.10008.3.1.2.2.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.3.1
        /// </summary>
        public static readonly AbstractSyntax Detached_Study_Management = new AbstractSyntax("1.2.840.10008.3.1.2.3.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.3.2
        /// </summary>
        public static readonly AbstractSyntax Study_Component_Management = new AbstractSyntax("1.2.840.10008.3.1.2.3.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.3.3
        /// </summary>
        public static readonly AbstractSyntax Modality_Performed_Procedure_Step = new AbstractSyntax("1.2.840.10008.3.1.2.3.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.3.4
        /// </summary>
        public static readonly AbstractSyntax Modality_Performed_Procedure_Step_Retrieve = new AbstractSyntax("1.2.840.10008.3.1.2.3.4");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.3.5
        /// </summary>
        public static readonly AbstractSyntax Modality_Performed_Procedure_Step_Notification = new AbstractSyntax("1.2.840.10008.3.1.2.3.5");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.5.1
        /// </summary>
        public static readonly AbstractSyntax Detached_Results_Management = new AbstractSyntax("1.2.840.10008.3.1.2.5.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.5.4
        /// </summary>
        public static readonly AbstractSyntax Detached_Results_Management_Meta = new AbstractSyntax("1.2.840.10008.3.1.2.5.4");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.5.5
        /// </summary>
        public static readonly AbstractSyntax Detached_Study_Management_Meta = new AbstractSyntax("1.2.840.10008.3.1.2.5.5");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.3.1.2.6.1
        /// </summary>
        public static readonly AbstractSyntax Detached_Interpretation_Management = new AbstractSyntax("1.2.840.10008.3.1.2.6.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.1
        /// </summary>
        public static readonly AbstractSyntax Basic_Film_Session = new AbstractSyntax("1.2.840.10008.5.1.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.2
        /// </summary>
        public static readonly AbstractSyntax Basic_Film_Box = new AbstractSyntax("1.2.840.10008.5.1.1.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.4
        /// </summary>
        public static readonly AbstractSyntax Basic_Grayscale_Image_Box = new AbstractSyntax("1.2.840.10008.5.1.1.4");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.4.1
        /// </summary>
        public static readonly AbstractSyntax Basic_Color_Image_Box = new AbstractSyntax("1.2.840.10008.5.1.1.4.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.9
        /// </summary>
        public static readonly AbstractSyntax Basic_Grayscale_Print_Management_Meta = new AbstractSyntax("1.2.840.10008.5.1.1.9");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.14
        /// </summary>
        public static readonly AbstractSyntax Print_Job = new AbstractSyntax("1.2.840.10008.5.1.1.14");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.15
        /// </summary>
        public static readonly AbstractSyntax Basic_Annotation_Box = new AbstractSyntax("1.2.840.10008.5.1.1.15");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.16
        /// </summary>
        public static readonly AbstractSyntax Printer = new AbstractSyntax("1.2.840.10008.5.1.1.16");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.16.376
        /// </summary>
        public static readonly AbstractSyntax Printer_Configuration_Retrieval = new AbstractSyntax("1.2.840.10008.5.1.1.16.376");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.18
        /// </summary>
        public static readonly AbstractSyntax Basic_Color_Print_Management_Meta = new AbstractSyntax("1.2.840.10008.5.1.1.18");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.18.1
        /// </summary>
        public static readonly AbstractSyntax Referenced_Color_Print = new AbstractSyntax("1.2.840.10008.5.1.1.18.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.23
        /// </summary>
        public static readonly AbstractSyntax Presentation_LUT = new AbstractSyntax("1.2.840.10008.5.1.1.23");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.24.1
        /// </summary>
        public static readonly AbstractSyntax Basic_Print_Image_Overlay_Box = new AbstractSyntax("1.2.840.10008.5.1.1.24.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.26
        /// </summary>
        public static readonly AbstractSyntax Print_Queue_Management = new AbstractSyntax("1.2.840.10008.5.1.1.26");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.27
        /// </summary>
        public static readonly AbstractSyntax Stored_Print_Storage = new AbstractSyntax("1.2.840.10008.5.1.1.27");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.29
        /// </summary>
        public static readonly AbstractSyntax Hardcopy_Grayscale_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.1.29");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.30
        /// </summary>
        public static readonly AbstractSyntax Hardcopy_Color_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.1.30");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.31
        /// </summary>
        public static readonly AbstractSyntax Pull_Print_Request = new AbstractSyntax("1.2.840.10008.5.1.1.31");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.32
        /// </summary>
        public static readonly AbstractSyntax Pull_Stored_Print_Management_Meta = new AbstractSyntax("1.2.840.10008.5.1.1.32");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.1
        /// </summary>
        public static readonly AbstractSyntax Computed_Radiography_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.1.1
        /// </summary>
        public static readonly AbstractSyntax Digital_XRay_Image_Storage_For_Presentation = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.1.1.1
        /// </summary>
        public static readonly AbstractSyntax Digital_XRay_Image_Storage_For_Processing = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.1.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.1.2
        /// </summary>
        public static readonly AbstractSyntax Digital_Mammography_XRay_Image_Storage_For_Presentation = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.1.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.1.2.1
        /// </summary>
        public static readonly AbstractSyntax Digital_Mammography_XRay_Image_Storage_For_Processing = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.1.2.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.1.3
        /// </summary>
        public static readonly AbstractSyntax Digital_Intraoral_XRay_Image_Storage_For_Presentation = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.1.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.1.3.1
        /// </summary>
        public static readonly AbstractSyntax Digital_Intraoral_XRay_Image_Storage_For_Processing = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.1.3.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.2
        /// </summary>
        public static readonly AbstractSyntax CT_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.3.1
        /// </summary>
        public static readonly AbstractSyntax Ultrasound_Multiframe_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.3.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.4
        /// </summary>
        public static readonly AbstractSyntax MR_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.4");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.4.1
        /// </summary>
        public static readonly AbstractSyntax Enhanced_MR_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.4.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.4.2
        /// </summary>
        public static readonly AbstractSyntax MR_Spectroscopy_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.4.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.6.1
        /// </summary>
        public static readonly AbstractSyntax Ultrasound_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.6.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.7
        /// </summary>
        public static readonly AbstractSyntax Secondary_Capture_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.7");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.7.1
        /// </summary>
        public static readonly AbstractSyntax Multiframe_Single_Bit_Secondary_Capture_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.7.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.7.2
        /// </summary>
        public static readonly AbstractSyntax Multiframe_Grayscale_Byte_Secondary_Capture_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.7.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.7.3
        /// </summary>
        public static readonly AbstractSyntax Multiframe_Grayscale_Word_Secondary_Capture_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.7.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.7.4
        /// </summary>
        public static readonly AbstractSyntax Multiframe_True_Color_Secondary_Capture_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.7.4");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.8
        /// </summary>
        public static readonly AbstractSyntax Standalone_Overlay_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.8");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.9
        /// </summary>
        public static readonly AbstractSyntax Standalone_Curve_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.9");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.9.1.1
        /// </summary>
        public static readonly AbstractSyntax Twelve_lead_ECG_Waveform_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.9.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.9.1.2
        /// </summary>
        public static readonly AbstractSyntax General_ECG_Waveform_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.9.1.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.9.1.3
        /// </summary>
        public static readonly AbstractSyntax Ambulatory_ECG_Waveform_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.9.1.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.9.2.1
        /// </summary>
        public static readonly AbstractSyntax Hemodynamic_Waveform_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.9.2.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.9.3.1
        /// </summary>
        public static readonly AbstractSyntax Cardiac_Electrophysiology_Waveform_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.9.3.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.9.4.1
        /// </summary>
        public static readonly AbstractSyntax Basic_Voice_Audio_Waveform_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.9.4.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.10
        /// </summary>
        public static readonly AbstractSyntax Standalone_Modality_LUT_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.10");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.11
        /// </summary>
        public static readonly AbstractSyntax Standalone_VOI_LUT_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.11");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.11.1
        /// </summary>
        public static readonly AbstractSyntax Grayscale_Softcopy_Presentation_State_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.11.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.12.1
        /// </summary>
        public static readonly AbstractSyntax XRay_Angiographic_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.12.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.12.2
        /// </summary>
        public static readonly AbstractSyntax XRay_Radiofluoroscopic_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.12.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.20
        /// </summary>
        public static readonly AbstractSyntax Nuclear_Medicine_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.20");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.66
        /// </summary>
        public static readonly AbstractSyntax Raw_Data_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.66");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.77.1.1
        /// </summary>
        public static readonly AbstractSyntax VL_Endoscopic_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.77.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.77.1.2
        /// </summary>
        public static readonly AbstractSyntax VL_Microscopic_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.77.1.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.77.1.3
        /// </summary>
        public static readonly AbstractSyntax VL_Slide_Coordinates_Microscopic_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.77.1.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.77.1.4
        /// </summary>
        public static readonly AbstractSyntax VL_Photographic_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.77.1.4");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.88.11
        /// </summary>
        public static readonly AbstractSyntax Basic_Text_SR = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.88.11");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.88.22
        /// </summary>
        public static readonly AbstractSyntax Enhanced_SR = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.88.22");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.88.33
        /// </summary>
        public static readonly AbstractSyntax Comprehensive_SR = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.88.33");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.88.50
        /// </summary>
        public static readonly AbstractSyntax Mammography_CAD_SR = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.88.50");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.88.59
        /// </summary>
        public static readonly AbstractSyntax Key_Object_Selection_Document = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.88.59");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.88.65
        /// </summary>
        public static readonly AbstractSyntax Chest_CAD_SR = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.88.65");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.128
        /// </summary>
        public static readonly AbstractSyntax Positron_Emission_Tomography_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.128");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.129
        /// </summary>
        public static readonly AbstractSyntax Standalone_PET_Curve_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.129");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.481.1
        /// </summary>
        public static readonly AbstractSyntax RT_Image_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.481.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.481.2
        /// </summary>
        public static readonly AbstractSyntax RT_Dose_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.481.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.481.3
        /// </summary>
        public static readonly AbstractSyntax RT_Structure_Set_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.481.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.481.4
        /// </summary>
        public static readonly AbstractSyntax RT_Beams_Treatment_Record_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.481.4");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.481.5
        /// </summary>
        public static readonly AbstractSyntax RT_Plan_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.481.5");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.481.6
        /// </summary>
        public static readonly AbstractSyntax RT_Brachy_Treatment_Record_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.481.6");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.481.7
        /// </summary>
        public static readonly AbstractSyntax RT_Treatment_Summary_Record_Storage = new AbstractSyntax("1.2.840.10008.5.1.4.1.1.481.7");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.2.1.1
        /// </summary>
        public static readonly AbstractSyntax Patient_Root_Query_Retrieve_Information_Model_FIND = new AbstractSyntax("1.2.840.10008.5.1.4.1.2.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.2.1.2
        /// </summary>
        public static readonly AbstractSyntax Patient_Root_Query_Retrieve_Information_Model_MOVE = new AbstractSyntax("1.2.840.10008.5.1.4.1.2.1.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.2.1.3
        /// </summary>
        public static readonly AbstractSyntax Patient_Root_Query_Retrieve_Information_Model_GET = new AbstractSyntax("1.2.840.10008.5.1.4.1.2.1.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.2.2.1
        /// </summary>
        public static readonly AbstractSyntax Study_Root_Query_Retrieve_Information_Model_FIND = new AbstractSyntax("1.2.840.10008.5.1.4.1.2.2.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.2.2.2
        /// </summary>
        public static readonly AbstractSyntax Study_Root_Query_Retrieve_Information_Model_MOVE = new AbstractSyntax("1.2.840.10008.5.1.4.1.2.2.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.2.2.3
        /// </summary>
        public static readonly AbstractSyntax Study_Root_Query_Retrieve_Information_Model_GET = new AbstractSyntax("1.2.840.10008.5.1.4.1.2.2.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.2.3.1
        /// </summary>
        public static readonly AbstractSyntax Patient_Study_Only_Query_Retrieve_Information_Model_FIND = new AbstractSyntax("1.2.840.10008.5.1.4.1.2.3.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.2.3.2
        /// </summary>
        public static readonly AbstractSyntax Patient_Study_Only_Query_Retrieve_Information_Model_MOVE = new AbstractSyntax("1.2.840.10008.5.1.4.1.2.3.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.2.3.3
        /// </summary>
        public static readonly AbstractSyntax Patient_Study_Only_Query_Retrieve_Information_Model_GET = new AbstractSyntax("1.2.840.10008.5.1.4.1.2.3.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.31
        /// </summary>
        public static readonly AbstractSyntax Modality_Worklist_Information_Model_FIND = new AbstractSyntax("1.2.840.10008.5.1.4.31");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.32.1
        /// </summary>
        public static readonly AbstractSyntax General_Purpose_Worklist_Information_Model_FIND = new AbstractSyntax("1.2.840.10008.5.1.4.32.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.32.2
        /// </summary>
        public static readonly AbstractSyntax General_Purpose_Scheduled_Procedure_Step = new AbstractSyntax("1.2.840.10008.5.1.4.32.2");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.32.3
        /// </summary>
        public static readonly AbstractSyntax General_Purpose_Performed_Procedure_Step = new AbstractSyntax("1.2.840.10008.5.1.4.32.3");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.32
        /// </summary>
        public static readonly AbstractSyntax General_Purpose_Worklist_Management_Meta = new AbstractSyntax("1.2.840.10008.5.1.4.32");

        /*
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.1.20.2 - Retired
        /// </summary>
        public static readonly AbstractSyntax Storage_Commitment_Pull_Model                         = new AbstractSyntax( "1.2.840.10008.1.20.2" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.4.2 - Retired
        /// </summary>
        public static readonly AbstractSyntax Referenced_Image_Box_SOP                              = new AbstractSyntax( "1.2.840.10008.5.1.1.4.2" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.9.1 - Retired
        /// </summary>
        public static readonly AbstractSyntax Referenced_Grayscale_Print_Management_Meta            = new AbstractSyntax( "1.2.840.10008.5.1.1.9.1" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.18.1 - Retired
        /// </summary>
        public static readonly AbstractSyntax Referenced_Color_Print_Management_Meta                = new AbstractSyntax( "1.2.840.10008.5.1.1.18.1" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.24 - Retired
        /// </summary>
        public static readonly AbstractSyntax Image_Overlay_Box                                     = new AbstractSyntax( "1.2.840.10008.5.1.1.24" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.3 - Retired
        /// </summary>
        public static readonly AbstractSyntax Ultrasound_Multiframe_Image_Storage                   = new AbstractSyntax( "1.2.840.10008.5.1.4.1.1.3" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.5 - Retired
        /// </summary>
        public static readonly AbstractSyntax Nuclear_Medicine_Image_Storage                        = new AbstractSyntax( "1.2.840.10008.5.1.4.1.1.5" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.6 - Retired
        /// </summary>
        public static readonly AbstractSyntax Ultrasound_Image_Storage                              = new AbstractSyntax( "1.2.840.10008.5.1.4.1.1.6" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.12.3 - Retired
        /// </summary>
        public static readonly AbstractSyntax XRay_Angiographic_BiPlane_Image_Storage               = new AbstractSyntax( "1.2.840.10008.5.1.4.1.1.12.3" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.77.1 - Retired
        /// </summary>
        public static readonly AbstractSyntax VL_Image_Storage                                      = new AbstractSyntax( "1.2.840.10008.5.1.4.1.1.77.1" );
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.4.1.1.77.2 - Retired
        /// </summary>
        public static readonly AbstractSyntax VL_Multiframe_Image_Storage                           = new AbstractSyntax( "1.2.840.10008.5.1.4.1.1.77.2" );
        */

        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.1.20.1.1 - Sop Instance
        /// </summary>
        public static readonly AbstractSyntax Storage_Commitment_Push_Model_Sop_Instance = new AbstractSyntax("1.2.840.10008.1.20.1.1");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.17 - Sop Instance
        /// </summary>
        public static readonly AbstractSyntax Printer_Sop_Instance = new AbstractSyntax("1.2.840.10008.5.1.1.17");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.17.376 - Sop Instance
        /// </summary>
        public static readonly AbstractSyntax Printer_Configuration_Retrieval_Sop_Instance = new AbstractSyntax("1.2.840.10008.5.1.1.17.376");
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.5.1.1.25 - Sop Instance
        /// </summary>
        public static readonly AbstractSyntax Print_Queue_Sop_Instance = new AbstractSyntax("1.2.840.10008.5.1.1.25");

        /*
        /// <summary>
        /// Predefined AbstractSyntax: 1.2.840.10008.1.20.2.1 - Sop Instance - Retired
        /// </summary>
        public static readonly AbstractSyntax Storage_Commitment_Pull_Model                         = new AbstractSyntax( "1.2.840.10008.1.20.2.1" );
        */

        /// <summary>
        /// Hashtable collection of predefined <see cref="AbstractSyntax"/> items.
        /// </summary>
        public static readonly System.Collections.Hashtable AbstractSyntaxNames;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            return true;
        }
    }

    /// <summary>
    /// A Transfer Syntax is a set of encoding rules able to unambiguously represent 
    /// one or more Abstract Syntaxes. 
    /// </summary>
    /// <remarks>
    /// In particular, it allows communicating Application Entities to negotiate common encoding
    /// techniques they both support (e.g., byte ordering, compression, etc.).
    /// </remarks>
    /// <remarks>
    /// A Transfer Syntax is an attribute of a Presentation Context, 
    /// one or more of which are negotiated at the establishment of an Association
    /// between DICOM Application Entities. 
    /// </remarks>
    public class TransferSyntax : PduItem
    {
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this._UID.GetHashCode();
        }
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare with this instance, or a null reference.</param>
        /// <returns><see langword="true"/> if other is an instance of <see cref="TransferSyntax"/> and equals the value of this instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(System.Object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;
            TransferSyntax transferSyntax = (TransferSyntax)obj;
            return (this._UID == transferSyntax._UID);
        }

        static TransferSyntax()
        {
            TransferSyntaxNames = new Hashtable();
            TransferSyntaxNames.Add(Implicit_VR_Little_Endian, "Implicit VR Little Endian");
            TransferSyntaxNames.Add(Explicit_VR_Little_Endian, "Explicit VR Little Endian");
            TransferSyntaxNames.Add(Deflated_Explicit_VR_Little_Endian, "Deflated Explicit VR Little Endian");
            TransferSyntaxNames.Add(Explicit_VR_Big_Endian, "Explicit VR Big Endian");
            TransferSyntaxNames.Add(JPEG_Baseline_Process_1, "JPEG Baseline Process 1");
            TransferSyntaxNames.Add(JPEG_Extended_Process_2_And_4, "JPEG Extended Process 2 And 4");
            TransferSyntaxNames.Add(JPEG_Extended_Process_3_And_5, "JPEG Extended Process 3 And 5");
            TransferSyntaxNames.Add(JPEG_Spectral_Selection_Non_Hierarchical_6_And_8, "JPEG Spectral Selection Non Hierarchical 6 And 8");
            TransferSyntaxNames.Add(JPEG_Spectral_Selection_Non_Hierarchical_7_And_9, "JPEG Spectral Selection Non Hierarchical 7 And 9");
            TransferSyntaxNames.Add(JPEG_Full_Progression_Non_Hierarchical_10_And_12, "JPEG Full Progression Non Hierarchical 10 And 12");
            TransferSyntaxNames.Add(JPEG_Full_Progression_Non_Hierarchical_11_And_13, "JPEG Full Progression Non Hierarchical 11 And 13");
            TransferSyntaxNames.Add(JPEG_Lossless_Non_Hierarchical_14, "JPEG Lossless Non Hierarchical 14");
            TransferSyntaxNames.Add(JPEG_Lossless_Non_Hierarchical_15, "JPEG Lossless Non Hierarchical 15");
            TransferSyntaxNames.Add(JPEG_Extended_Hierarchical_16_And_18, "JPEG Extended Hierarchical 16 And 18");
            TransferSyntaxNames.Add(JPEG_Extended_Hierarchical_17_And_19, "JPEG Extended Hierarchical 17 And 19");
            TransferSyntaxNames.Add(JPEG_Spectral_Selection_Hierarchical_20_And_22, "JPEG Spectral Selection Hierarchical 20 And 22");
            TransferSyntaxNames.Add(JPEG_Spectral_Selection_Hierarchical_21_And_23, "JPEG Spectral Selection Hierarchical 21 And 23");
            TransferSyntaxNames.Add(JPEG_Full_Progression_Hierarchical_24_And_26, "JPEG Full Progression Hierarchical 24 And 26");
            TransferSyntaxNames.Add(JPEG_Full_Progression_Hierarchical_25_And_27, "JPEG Full Progression Hierarchical 25 And 27");
            TransferSyntaxNames.Add(JPEG_Lossless_Hierarchical_28, "JPEG Lossless Hierarchical 28");
            TransferSyntaxNames.Add(JPEG_Lossless_Hierarchical_29, "JPEG Lossless Hierarchical 29");
            TransferSyntaxNames.Add(JPEG_Lossless_Non_Hierarchical_1st_Order_Prediction, "JPEG Lossless Non Hierarchical 1st Order Prediction");
            TransferSyntaxNames.Add(JPEG_LS_Lossless_Image_Compression, "JPEG LS Lossless Image Compression");
            TransferSyntaxNames.Add(JPEG_LS_Lossy_Image_Compression, "JPEG LS Lossy Image Compression");
            TransferSyntaxNames.Add(JPEG_2000_IC_Lossless_Only, "JPEG 2000 Image Compression Lossless Only");
            TransferSyntaxNames.Add(JPEG_2000_IC, "JPEG 2000 Image Compression");
            TransferSyntaxNames.Add(JPEG_2000_Multicomponent_lossless2, "JPEG 2000Multi-component lossless");
            TransferSyntaxNames.Add(JPEG_2000_Multicomponent2, "JPEG 2000 Part 2 Multi-component");
            TransferSyntaxNames.Add(JPIP_Referenced, "JPIP Referenced");
            TransferSyntaxNames.Add(JPIP_Referenced_Deflate, "JPIP Referenced Deflate");
            TransferSyntaxNames.Add(MPEG2_Main_Profile_Level, "MPEG2 Main Profile");
            TransferSyntaxNames.Add(MPEG2_High_Profile_Level, "MPEG2 High Profile");
            TransferSyntaxNames.Add(RFC_2557_Mime_Encapsulation, "RFC 2557 MIME encapsulation");
            TransferSyntaxNames.Add(XML_Encoding, "XML Encoding");
            TransferSyntaxNames.Add(RLE_Lossless, "RLE Lossless");
            TransferSyntaxNames.Add(MPEG4_AVC_H_264_High_Profile_Level_4_1, "MPEG4 AVC/H.264 High Profile / Level 4.1");
            TransferSyntaxNames.Add(MPEG4_AVC_H_264_BD_compatible_High_Profile_Level_4_1, "MPEG4 AVC/H.264 BD-compatible High Profile / Level 4.1");
            TransferSyntaxNames.Add(MPEG4_AVC_H_264_High_Profile_Level_4_2_For_2D_Video, "MPEG4 AVC/H.264 High Profile / Level 4.2 For 2D Video");
            TransferSyntaxNames.Add(MPEG4_AVC_H_264_High_Profile_Level_4_2_For_3D_Video, "MPEG4 AVC/H.264 High Profile / Level 4.2 For 3D Video");
            TransferSyntaxNames.Add(MPEG4_AVC_H_264_Stereo_High_Profile_Level_4_2, "MPEG4 AVC/H.264 Stereo High Profile / Level 4.2");
            TransferSyntaxNames.Add(HEVC_H_265_Main_Profile_Level_5_1, "HEVC/H.265 Main Profile / Level 5.1");
            TransferSyntaxNames.Add(HEVC_H_265_Main_10_Profile_Level_5_1, "HEVC/H.265 Main 10 Profile / Level 5.1");
        }
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>Required by Xml serialization.</remarks>
        public TransferSyntax() { }

        /// <summary>
        /// Specific constructor.
        /// </summary>
        /// <param name="uid">Unique identifier</param>
        public TransferSyntax(System.String uid)
        {
            this._UID = uid;
        }

        /// <summary>
        /// Obtains the <see cref="System.String"/> representation of this instance.
        /// </summary>
        /// <returns>The friendly name of the <see cref="TransferSyntax"/>.</returns>
        public override string ToString()
        {
            string name = (string)TransferSyntaxNames[this];
            if (name != null)
            {
                // return literal string
                return name;
            }
            else
            {
                // return UID string
                return this._UID;
            }
        }

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.TRANSFER_SYNTAX"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.TRANSFER_SYNTAX;
            }
        }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public string UID
        {
            get
            {
                return _UID;
            }
        }
        private readonly string _UID = string.Empty; // Only set during construction.

        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2
        /// </summary>
        public static readonly TransferSyntax Implicit_VR_Little_Endian = new TransferSyntax("1.2.840.10008.1.2");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.1
        /// </summary>
        public static readonly TransferSyntax Explicit_VR_Little_Endian = new TransferSyntax("1.2.840.10008.1.2.1");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.1.99
        /// </summary>
        public static readonly TransferSyntax Deflated_Explicit_VR_Little_Endian = new TransferSyntax("1.2.840.10008.1.2.1.99");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.2
        /// </summary>
        public static readonly TransferSyntax Explicit_VR_Big_Endian = new TransferSyntax("1.2.840.10008.1.2.2");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.50
        /// </summary>
        public static readonly TransferSyntax JPEG_Baseline_Process_1 = new TransferSyntax("1.2.840.10008.1.2.4.50");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.51
        /// </summary>
        public static readonly TransferSyntax JPEG_Extended_Process_2_And_4 = new TransferSyntax("1.2.840.10008.1.2.4.51");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.52
        /// </summary>
        public static readonly TransferSyntax JPEG_Extended_Process_3_And_5 = new TransferSyntax("1.2.840.10008.1.2.4.52");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.53
        /// </summary>
        public static readonly TransferSyntax JPEG_Spectral_Selection_Non_Hierarchical_6_And_8 = new TransferSyntax("1.2.840.10008.1.2.4.53");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.54
        /// </summary>
        public static readonly TransferSyntax JPEG_Spectral_Selection_Non_Hierarchical_7_And_9 = new TransferSyntax("1.2.840.10008.1.2.4.54");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.55
        /// </summary>
        public static readonly TransferSyntax JPEG_Full_Progression_Non_Hierarchical_10_And_12 = new TransferSyntax("1.2.840.10008.1.2.4.55");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.56
        /// </summary>
        public static readonly TransferSyntax JPEG_Full_Progression_Non_Hierarchical_11_And_13 = new TransferSyntax("1.2.840.10008.1.2.4.56");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.57
        /// </summary>
        public static readonly TransferSyntax JPEG_Lossless_Non_Hierarchical_14 = new TransferSyntax("1.2.840.10008.1.2.4.57");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.58
        /// </summary>
        public static readonly TransferSyntax JPEG_Lossless_Non_Hierarchical_15 = new TransferSyntax("1.2.840.10008.1.2.4.58");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.59
        /// </summary>
        public static readonly TransferSyntax JPEG_Extended_Hierarchical_16_And_18 = new TransferSyntax("1.2.840.10008.1.2.4.59");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.60
        /// </summary>
        public static readonly TransferSyntax JPEG_Extended_Hierarchical_17_And_19 = new TransferSyntax("1.2.840.10008.1.2.4.60");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.61
        /// </summary>
        public static readonly TransferSyntax JPEG_Spectral_Selection_Hierarchical_20_And_22 = new TransferSyntax("1.2.840.10008.1.2.4.61");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.62
        /// </summary>
        public static readonly TransferSyntax JPEG_Spectral_Selection_Hierarchical_21_And_23 = new TransferSyntax("1.2.840.10008.1.2.4.62");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.63
        /// </summary>
        public static readonly TransferSyntax JPEG_Full_Progression_Hierarchical_24_And_26 = new TransferSyntax("1.2.840.10008.1.2.4.63");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.64
        /// </summary>
        public static readonly TransferSyntax JPEG_Full_Progression_Hierarchical_25_And_27 = new TransferSyntax("1.2.840.10008.1.2.4.64");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.65
        /// </summary>
        public static readonly TransferSyntax JPEG_Lossless_Hierarchical_28 = new TransferSyntax("1.2.840.10008.1.2.4.65");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.66
        /// </summary>
        public static readonly TransferSyntax JPEG_Lossless_Hierarchical_29 = new TransferSyntax("1.2.840.10008.1.2.4.66");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.70
        /// </summary>
        public static readonly TransferSyntax JPEG_Lossless_Non_Hierarchical_1st_Order_Prediction = new TransferSyntax("1.2.840.10008.1.2.4.70");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.80
        /// </summary>
        public static readonly TransferSyntax JPEG_LS_Lossless_Image_Compression = new TransferSyntax("1.2.840.10008.1.2.4.80");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.81
        /// </summary>
        public static readonly TransferSyntax JPEG_LS_Lossy_Image_Compression = new TransferSyntax("1.2.840.10008.1.2.4.81");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.90
        /// </summary>
        public static readonly TransferSyntax JPEG_2000_IC_Lossless_Only = new TransferSyntax("1.2.840.10008.1.2.4.90");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.91
        /// </summary>
        public static readonly TransferSyntax JPEG_2000_IC = new TransferSyntax("1.2.840.10008.1.2.4.91");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.92 
        /// </summary>
        public static readonly TransferSyntax JPEG_2000_Multicomponent_lossless2 = new TransferSyntax("1.2.840.10008.1.2.4.92");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.93 
        /// </summary>
        public static readonly TransferSyntax JPEG_2000_Multicomponent2 = new TransferSyntax("1.2.840.10008.1.2.4.93");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.94 
        /// </summary>
        public static readonly TransferSyntax JPIP_Referenced = new TransferSyntax("1.2.840.10008.1.2.4.94");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.95
        /// </summary>
        public static readonly TransferSyntax JPIP_Referenced_Deflate = new TransferSyntax("1.2.840.10008.1.2.4.95");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.100
        /// </summary>
        public static readonly TransferSyntax MPEG2_Main_Profile_Level = new TransferSyntax("1.2.840.10008.1.2.4.100");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.101
        /// </summary>
        public static readonly TransferSyntax MPEG2_High_Profile_Level = new TransferSyntax("1.2.840.10008.1.2.4.101");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.102
        /// </summary>
        public static readonly TransferSyntax MPEG4_AVC_H_264_High_Profile_Level_4_1 = new TransferSyntax("1.2.840.10008.1.2.4.102");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.103
        /// </summary>
        public static readonly TransferSyntax MPEG4_AVC_H_264_BD_compatible_High_Profile_Level_4_1 = new TransferSyntax("1.2.840.10008.1.2.4.103");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.104
        /// </summary>
        public static readonly TransferSyntax MPEG4_AVC_H_264_High_Profile_Level_4_2_For_2D_Video = new TransferSyntax("1.2.840.10008.1.2.4.104");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.105
        /// </summary>
        public static readonly TransferSyntax MPEG4_AVC_H_264_High_Profile_Level_4_2_For_3D_Video = new TransferSyntax("1.2.840.10008.1.2.4.105");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.106
        /// </summary>
        public static readonly TransferSyntax MPEG4_AVC_H_264_Stereo_High_Profile_Level_4_2 = new TransferSyntax("1.2.840.10008.1.2.4.106");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.107
        /// </summary>
        public static readonly TransferSyntax HEVC_H_265_Main_Profile_Level_5_1 = new TransferSyntax("1.2.840.10008.1.2.4.107");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.108
        /// </summary>
        public static readonly TransferSyntax HEVC_H_265_Main_10_Profile_Level_5_1 = new TransferSyntax("1.2.840.10008.1.2.4.108");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.6.1
        /// </summary>
        public static readonly TransferSyntax RFC_2557_Mime_Encapsulation = new TransferSyntax("1.2.840.10008.1.2.6.1");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.6.2
        /// </summary>
        public static readonly TransferSyntax XML_Encoding = new TransferSyntax("1.2.840.10008.1.2.6.2");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.5
        /// </summary>
        public static readonly TransferSyntax RLE_Lossless = new TransferSyntax("1.2.840.10008.1.2.5");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2
        /// </summary>
        public static readonly TransferSyntax Default_Transfer_Syntax = new TransferSyntax("1.2.840.10008.1.2");
        /// <summary>
        /// Predefined TransferSyntax: 1.2.840.10008.1.2.4.70
        /// </summary>
        public static readonly TransferSyntax Default_Transfer_Syntax_Lossless_JPEG_Compression = new TransferSyntax("1.2.840.10008.1.2.4.70");

        /// <summary>
        /// Hashtable collection of predefined <see cref="TransferSyntax"/> items.
        /// </summary>
        public static readonly System.Collections.Hashtable TransferSyntaxNames;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            return true;
        }
    }

    /// <summary>
    /// Type safe TransferSyntaxList
    /// </summary>
    public sealed class TransferSyntaxList :
        DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {
        private System.ComponentModel.ListChangedEventHandler onListChanged;

        private void OnListChanged(System.ComponentModel.ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }

        /// <summary>
        /// Performs additional custom processes after clearing the contents of the <see cref="System.Collections.CollectionBase"/> instance.
        /// </summary>
        protected override void OnClearComplete()
        {
            OnListChanged(
                new System.ComponentModel.ListChangedEventArgs(
                System.ComponentModel.ListChangedType.Reset, -1)
                );
        }
        /// <summary>
        /// Performs additional custom processes after inserting a new element into the <see cref="System.Collections.CollectionBase"/> instance.
        /// </summary>
        /// <param name="index">The zero-based index at which to insert <c>value</c>. </param>
        /// <param name="value">The new value of the element at <c>index</c>.</param>
        protected override void OnInsertComplete(int index, object value)
        {
            OnListChanged(
                new System.ComponentModel.ListChangedEventArgs(
                System.ComponentModel.ListChangedType.ItemAdded, index)
                );
        }
        /// <summary>
        /// Performs additional custom processes after removing an element from the <see cref="System.Collections.CollectionBase"/> instance.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> can be found. </param>
        /// <param name="value">The value of the element to remove from <c>index</c>. </param>
        protected override void OnRemoveComplete(int index, object value)
        {
            OnListChanged(
                new System.ComponentModel.ListChangedEventArgs(
                System.ComponentModel.ListChangedType.ItemDeleted, index)
                );
        }
        /// <summary>
        /// Performs additional custom processes after setting a value in the <see cref="System.Collections.CollectionBase"/> instance.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>oldValue</c> can be found. </param>
        /// <param name="oldValue">The value to replace with <c>newValue</c>. </param>
        /// <param name="newValue">The new value of the element at <c>index</c>. </param>
        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            OnListChanged(
                new System.ComponentModel.ListChangedEventArgs(
                System.ComponentModel.ListChangedType.ItemAdded, index)
                );
        }

        /// <summary>
        /// Occurs when the list managed by the <see cref="TransferSyntaxList"/> changes.
        /// </summary>
        public event System.ComponentModel.ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TransferSyntaxList() { }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new TransferSyntax this[int index]
        {
            get
            {
                return (TransferSyntax)base[index];
            }
            set
            {
                base.Insert(index, value);
            }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted.</param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, TransferSyntax value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(TransferSyntax value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(TransferSyntax value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(TransferSyntax value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(TransferSyntax value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<TransferSyntaxes>");
            foreach (TransferSyntax transferSyntax in this)
            {
                streamWriter.WriteLine("<TransferSyntax>{0}</TransferSyntax>", transferSyntax.UID);
            }
            streamWriter.WriteLine("</TransferSyntaxes>");

            return true;
        }
    }

    /// <summary>
    /// Specific RequestedPresentationContext PduItem.
    /// </summary>
    public class RequestedPresentationContext : PduItem
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RequestedPresentationContext()
        {
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="abstractSyntax">abstract syntax</param>
        /// <param name="transferSyntaxes">list of transfer syntaxes</param>
        public RequestedPresentationContext(
            AbstractSyntax abstractSyntax,
            params TransferSyntax[] transferSyntaxes)
        {
            this.AbstractSyntax = abstractSyntax;
            foreach (TransferSyntax ts in transferSyntaxes)
            {
                this.TransferSyntaxes.Add(ts);
            }
        }

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.RQ_PRESENTATION_CONTEXT"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.RQ_PRESENTATION_CONTEXT;
            }
        }

        /// <summary>
        /// Identifier.
        /// </summary>
        public System.Byte ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        private System.Byte _ID = 0;

        /// <summary>
        /// Abstract syntax.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public AbstractSyntax AbstractSyntax
        {
            get
            {
                return _AbstractSyntax;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _AbstractSyntax = value;
            }
        }
        private AbstractSyntax _AbstractSyntax = new AbstractSyntax();

        /// <summary>
        /// Collection of transfer syntaxes.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public TransferSyntaxList TransferSyntaxes
        {
            get
            {
                return _TransferSyntaxes;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _TransferSyntaxes = value;
            }
        }
        private TransferSyntaxList _TransferSyntaxes = new TransferSyntaxList();

        /// <summary>
        /// Add an array of <see cref="DvtkData.Dul.TransferSyntax"/> items to <see cref="DvtkData.Dul.RequestedPresentationContext"/>
        /// </summary>
        /// <param name="transferSyntaxes">Array of <see cref="DvtkData.Dul.TransferSyntax"/> items to add.</param>
        public void AddTransferSyntaxes(params TransferSyntax[] transferSyntaxes)
        {
            foreach (TransferSyntax transferSyntax in transferSyntaxes)
            {
                _TransferSyntaxes.Add(transferSyntax);
            }
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<PresentationContext Id=\"{0}\" AbstractSyntaxName=\"{1}\">", ID.ToString(), AbstractSyntax.UID);
            TransferSyntaxes.DvtDetailToXml(streamWriter, level);
            streamWriter.WriteLine("</PresentationContext>");

            return true;
        }
    }

    /// <summary>
    /// An application context explicitly defines the set of application service elements, related options and any
    /// other information necessary for the interworking of Application Entities on an association. The usage of
    /// the application context is defined in PS 3.7.
    /// </summary>
    /// <remarks>
    /// Two Application Entities when establishing an association agree on an application context. The requestor
    /// of an association proposes an Application Context Name and the acceptor returns either the same or a
    /// different Application Context Name. The returned name specifies the application context to be used for
    /// this association. The offer of an alternate application context by the acceptor provides a mechanism for
    /// limited negotiation. If the requestor cannot operate in the acceptor's application context, it will issue an
    /// A-Abort request primitive. Such a negotiation will facilitate the introduction of future versions of the
    /// DICOM Application Entity.
    /// </remarks>
    /// <remarks>
    /// DICOM APPLICATION CONTEXT NAME ENCODING AND REGISTRATION<br></br>
    /// The Application Context Name structure is based on the OSI Object Identification (numeric form) as
    /// defined by ISO 8824. Application Context Names are registered values as defined by ISO 9834-3 to
    /// ensure global uniqueness. They are encoded as defined in Annex F when the TCP/IP network
    /// communication support is used as defined in Section 9.
    /// </remarks>
    /// <remarks>
    /// A.2.1 DICOM registered application context names<br></br>
    /// The organization responsible for the definition and registration of DICOM Application Context Names is
    /// NEMA. NEMA guarantees uniqueness for all DICOM Application Context Names. A choice of DICOM
    /// registered Application Context Names related to the DICOM Application Entities, as well as the
    /// associated negotiation rules, are defined in PS 3.7.
    /// </remarks>
    public class ApplicationContext : PduItem
    {

        /// <summary>
        /// 1.2.840.10008.3.1.1.1 DICOM Application Context Name
        /// </summary>
        public const string DICOM_Application_Context_Name = "1.2.840.10008.3.1.1.1";

        /// <summary>
        /// Protocol Data Unit Item - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduItemType.APPLICATION_CONTEXT"/></remarks>
        public override PduItemType PduItemType
        {
            get
            {
                return PduItemType.APPLICATION_CONTEXT;
            }
        }

        /// <summary>
        /// Application-context-names are structured as UIDs as defined in PS 3.5
        /// (see Annex A for an overview of this concept). 
        /// DICOM Application-context-names are registered in PS 3.7.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _Name = value;
            }
        }
        private string _Name = DICOM_Application_Context_Name;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<ApplicationContextName>{0}</ApplicationContextName>", Name);

            return true;
        }
    }

    /// <summary>
    /// Type safe RequestedPresentationContextList
    /// </summary>
    public sealed class RequestedPresentationContextList :
        DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RequestedPresentationContextList() { }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new RequestedPresentationContext this[int index]
        {
            get
            {
                return (RequestedPresentationContext)base[index];
            }
            set
            {
                base.Insert(index, value);
            }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, RequestedPresentationContext value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(RequestedPresentationContext value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(RequestedPresentationContext value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(RequestedPresentationContext value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(RequestedPresentationContext value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<PresentationContexts>");
            foreach (RequestedPresentationContext presentationContext in this)
            {
                presentationContext.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</PresentationContexts>");

            return true;
        }
    }

    /// <summary>
    /// This Presentation P-DATA Service shall be used by either AE to cause the 
    /// exchange of application information (i.e. DICOM Messages). 
    /// </summary>
    /// <remarks>
    /// DICOM Messages shall be exchanged as defined in PS 3.7. 
    /// </remarks>
    /// <remarks>
    /// An association provides a simultaneous bi-directional exchange 
    /// of P-DATA request/indication primitives.
    /// </remarks>
    public class P_DATA_TF : DulMessage
    {
        /*
        /// <summary>
        /// The Presentation Data Value List parameter shall contain one or more 
        /// Presentation Data Values (PDV).
        /// </summary>
        /// <remarks>
        /// Each PDV shall consist of two parameters: <br></br>
        /// a Presentation Context ID <br></br>
        /// and User Data values. <br></br>
        /// </remarks>
        /// <remarks>
        /// The User Data values are taken from the Abstract Syntax and encoded in the 
        /// Transfer Syntax identified by the Presentation Context ID.
        /// </remarks>
        private PresentationDataValueList m_presentationDataValueList = 
            new PresentationDataValueList();
            */

        /// <summary>
        /// Protocol Data Unit - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduType.P_DATA_TF"/></remarks>
        public override PduType PduType
        {
            get
            {
                return PduType.P_DATA_TF;
            }
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<DataTf>");
            streamWriter.WriteLine("</DataTf>");

            return true;
        }
    }

    /// <summary>
    /// The graceful release of an association between two AEs shall be performed 
    /// through ACSE A-RELEASE request, indication, response, and confirmation primitives. 
    /// </summary>
    /// <remarks>
    /// The initiator of the service is hereafter called a requestor and the service-user 
    /// which receives the A-RELEASE indication is hereafter called the acceptor.
    /// </remarks>
    /// <remarks>
    /// It shall be a confirmed service.
    /// </remarks>
    public class A_RELEASE_RP : DulMessage
    {

        /// <summary>
        /// Protocol Data Unit - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduType.A_RELEASE_RP"/></remarks>
        public override PduType PduType
        {
            get
            {
                return PduType.A_RELEASE_RP;
            }
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<ReleaseRp>");
            streamWriter.WriteLine("</ReleaseRp>");

            return true;
        }
    }

    /// <summary>
    /// The graceful release of an association between two AEs shall be performed 
    /// through ACSE A-RELEASE request, indication, response, and confirmation primitives. 
    /// </summary>
    /// <remarks>
    /// The initiator of the service is hereafter called a requestor and the service-user 
    /// which receives the A-RELEASE indication is hereafter called the acceptor.
    /// </remarks>
    /// <remarks>
    /// It shall be a confirmed service.
    /// </remarks>
    public class A_RELEASE_RQ : DulMessage
    {

        /// <summary>
        /// Protocol Data Unit - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduType.A_RELEASE_RQ"/></remarks>
        public override PduType PduType
        {
            get
            {
                return PduType.A_RELEASE_RQ;
            }
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<ReleaseRq>");
            streamWriter.WriteLine("</ReleaseRq>");

            return true;
        }
    }

    /// <summary>
    /// The establishment of an association between two AEs shall be performed 
    /// through ACSE A-ASSOCIATE request, indication, response and confirmation primitives. 
    /// </summary>
    /// <remarks>
    /// The initiator of the service is hereafter called a requestor and the service-user 
    /// which receives the A-ASSOCIATE indication is hereafter called the acceptor. 
    /// </remarks>
    /// <remarks>
    /// It shall be a confirmed service.
    /// </remarks>
    public class A_ASSOCIATE_RJ : DulMessage
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>Required by Xml serialization.</remarks>
        public A_ASSOCIATE_RJ() { }

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="result">This parameter indicates the result of this association accept.</param>
        /// <param name="source">This parameter indicates the source of this association accept.</param>
        /// <param name="reason">This parameter indicates the reason of this association accept.</param>
        public A_ASSOCIATE_RJ(
            System.Byte result, System.Byte source, System.Byte reason)
        {
            this._Result = result;
            this._Reason = reason;
            this._Source = source;
        }

        /// <summary>
        /// Protocol Data Unit - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduType.A_ASSOCIATE_RJ"/></remarks>
        public override PduType PduType
        {
            get
            {
                return PduType.A_ASSOCIATE_RJ;
            }
        }

        /// <summary>
        /// This Result field shall contain an integer value encoded as an unsigned binary number.
        /// </summary>
        /// <remarks>
        /// One of the following values shall be used:<br></br>
        /// 1 - rejected-permanent<br></br>
        /// 2 - rejected-transient
        /// </remarks>
        public System.Byte Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value;
            }
        }
        private System.Byte _Result = Result_Rejected_Permanent;

        /// <summary>
        /// This Source field shall contain an integer value encoded as an unsigned binary number.
        /// </summary>
        /// <remarks>
        /// One of the following values shall be used:<br></br>
        /// 1 - DICOM UL service-user<br></br>
        /// 2 - DICOM UL service-provider (ACSE related function)<br></br>
        /// 3 - DICOM UL service-provider (Presentation related function)
        /// </remarks>
        public System.Byte Source
        {
            get
            {
                return _Source;
            }
            set
            {
                _Source = value;
            }
        }
        private System.Byte _Source = Source_Dul_Service_User;

        /// <summary>
        /// This field shall contain an integer value encoded as an unsigned binary number.
        /// </summary>
        /// <remarks>
        /// If the Source field has the value (1) “DICOM UL service-user”, 
        /// it shall take one of the following:<br></br>
        /// 1 - no-reason-given<br></br>
        /// 2 - application-context-name-not-supported<br></br>
        /// 3 - calling-AE-title-not-recognized<br></br>
        /// 4-6 - reserved<br></br>
        /// 7 - called-AE-title-not-recognized<br></br>
        /// 8-10 - reserved<br></br>
        /// </remarks>
        /// <remarks>
        /// If the Source field has the value (2) “DICOM UL service provided (ACSE related function),” 
        /// it shall take one of the following:<br></br>
        /// 1 - no-reason-given<br></br>
        /// 2 - protocol-version-not-supported<br></br>
        /// </remarks>
        /// <remarks>
        /// If the Source field has the value (3) “DICOM UL service provided (Presentation related function),” 
        /// it shall take one of the following:<br></br>
        /// 0 - reserved<br></br>
        /// 1 - temporary-congestion<br></br>
        /// 2 - local-limit-exceeded<br></br>
        /// 3-7 - reserved
        /// </remarks>
        public System.Byte Reason
        {
            get
            {
                return _Reason;
            }
            set
            {
                _Reason = value;
            }
        }
        private System.Byte _Reason = Reason_Dul_Service_User_No_Reason_Given;

        /// <summary>
        /// Predefined Result: Rejected Permanent = 1
        /// </summary>
        public const System.Byte Result_Rejected_Permanent = 1;
        /// <summary>
        /// Predefined Result: Rejected Transient = 2
        /// </summary>
        public const System.Byte Result_Rejected_Transient = 2;

        /// <summary>
        /// Predefined Source: Dul Service User = 1
        /// </summary>
        public const System.Byte Source_Dul_Service_User = 1;
        /// <summary>
        /// Predefined Source: Dul Service Provider A = 2
        /// </summary>
        public const System.Byte Source_Dul_Service_Provider_A = 2;
        /// <summary>
        /// Predefined Source: Dul Service Provider P = 3
        /// </summary>
        public const System.Byte Source_Dul_Service_Provider_P = 3;

        /// <summary>
        /// Predefined Reason: Dul Service User - No Reason Given = 1
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_No_Reason_Given = 1;
        /// <summary>
        /// Predefined Reason: Dul Service User - User Application Context Name Not Supported = 2
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Application_Context_Name_Not_Supported = 2;
        /// <summary>
        /// Predefined Reason: Dul Service User - User Calling AE Title Not Recognized = 3
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Calling_AE_Title_Not_Recognized = 3;
        /// <summary>
        /// Predefined Reason: Dul Service User - User Reserved 4 = 4
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Reserved_4 = 4;
        /// <summary>
        /// Predefined Reason: Dul Service User - User Reserved 5 = 5
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Reserved_5 = 5;
        /// <summary>
        /// Predefined Reason: Dul Service User - User Reserved 6 = 6
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Reserved_6 = 6;
        /// <summary>
        /// Predefined Reason: Dul Service User - User Called AE Title Not Recognized = 7
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Called_AE_Title_Not_Recognized = 7;
        /// <summary>
        /// Predefined Reason: Dul Service User - User Reserved 8 = 8
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Reserved_8 = 8;
        /// <summary>
        /// Predefined Reason: Dul Service User - User Reserved 9 = 9
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Reserved_9 = 9;
        /// <summary>
        /// Predefined Reason: Dul Service User - User Reserved 10 = 10
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Reserved_10 = 10;

        /// <summary>
        /// Predefined Reason: Dul Service Provider A - No Reason Given = 1
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_A_No_Reason_Given = 1;
        /// <summary>
        /// Predefined Reason: Dul Service Provider A - Protocol Version Not Supported = 2
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_A_Protocol_Version_Not_Supported = 2;

        /// <summary>
        /// Predefined Reason: Dul Service Provider P - Reserved 0 = 0
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_P_Reserved_0 = 0;
        /// <summary>
        /// Predefined Reason: Dul Service Provider P - Temporary Congestion = 1
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_P_Temporary_Congestion = 1;
        /// <summary>
        /// Predefined Reason: Dul Service Provider P - Local Limit Exceeded = 2
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_P_Local_Limit_Exceeded = 2;
        /// <summary>
        /// Predefined Reason: Dul Service Provider P - Reserved 3 = 3
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_P_Reserved_3 = 3;
        /// <summary>
        /// Predefined Reason: Dul Service Provider P - Reserved 4 = 4
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_P_Reserved_4 = 4;
        /// <summary>
        /// Predefined Reason: Dul Service Provider P - Reserved 5 = 5
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_P_Reserved_5 = 5;
        /// <summary>
        /// Predefined Reason: Dul Service Provider P - Reserved 6 = 6
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_P_Reserved_6 = 6;
        /// <summary>
        /// Predefined Reason: Dul Service Provider P - Reserved 7 = 7
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_P_Reserved_7 = 7;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<AssociateRj>");
            streamWriter.WriteLine("<Result>{0}</Result>", Result.ToString());
            streamWriter.WriteLine("<Source>{0}</Source>", Source.ToString());
            streamWriter.WriteLine("<Reason>{0}</Reason>", Reason.ToString());
            streamWriter.WriteLine("</AssociateRj>");

            return true;
        }
    }

    /// <summary>
    /// Type safe AcceptedPresentationContextList
    /// </summary>
    public sealed class AcceptedPresentationContextList :
        DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AcceptedPresentationContextList() { }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        public new AcceptedPresentationContext this[int index]
        {
            get
            {
                return (AcceptedPresentationContext)base[index];
            }
            set
            {
                base.Insert(index, value);
            }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        public void Insert(int index, AcceptedPresentationContext value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(AcceptedPresentationContext value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(AcceptedPresentationContext value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(AcceptedPresentationContext value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(AcceptedPresentationContext value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<PresentationContexts>");
            foreach (AcceptedPresentationContext presentationContext in this)
            {
                presentationContext.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</PresentationContexts>");

            return true;
        }
    }

    /// <summary>
    /// The establishment of an association between two AEs shall be performed 
    /// through ACSE A-ASSOCIATE request, indication, response and confirmation primitives. 
    /// </summary>
    /// <remarks>
    /// The initiator of the service is hereafter called a requestor and the service-user 
    /// which receives the A-ASSOCIATE indication is hereafter called the acceptor. 
    /// </remarks>
    /// <remarks>
    /// It shall be a confirmed service.
    /// </remarks>
    public class A_ASSOCIATE_AC : DulMessage
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public A_ASSOCIATE_AC() { }

        /// <summary>
        /// Specific constructor.
        /// </summary>
        /// <param name="protocolVersion">Protocol Version.</param>
        /// <param name="calledAETitle">Called Application Entity Title.</param>
        /// <param name="callingAETitle">Calling Application Entity Title.</param>
        /// <param name="applicationContextName">Application Context Name.</param>
        /// <param name="maximumLengthReceived">Maximum Length Received.</param>
        /// <param name="implementationClassUid">Implementation Class Unique Identifier.</param>
        public A_ASSOCIATE_AC(
            System.UInt16 protocolVersion,
            string calledAETitle,
            string callingAETitle,
            string applicationContextName,
            System.UInt16 maximumLengthReceived,
            string implementationClassUid)
        {
            this.ProtocolVersion = protocolVersion;
            this.CalledAETitle = calledAETitle;
            this.CallingAETitle = callingAETitle;
            this.ApplicationContext.Name = applicationContextName;
            this.UserInformation.MaximumLength.MaximumLengthReceived =
                maximumLengthReceived;
            this.UserInformation.ImplementationClassUid.UID =
                implementationClassUid;
        }

        /// <summary>
        /// Protocol Data Unit - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduType.A_ASSOCIATE_AC"/></remarks>
        public override PduType PduType
        {
            get
            {
                return PduType.A_ASSOCIATE_AC;
            }
        }

        /// <summary>
        /// This two byte field shall use one bit to identify each version 
        /// of the DICOM UL protocol supported by the calling end-system.
        /// </summary>
        /// <remarks>
        /// This is Version 1 and shall be identified with bit 0 set.
        /// A receiver of this PDU implementing only this version of the 
        /// DICOM UL protocol shall only test that bit 0 is set.
        /// </remarks>
        public System.UInt16 ProtocolVersion
        {
            get
            {
                return _ProtocolVersion;
            }
            set
            {
                _ProtocolVersion = value;
            }
        }
        private System.UInt16 _ProtocolVersion = 1;

        /// <summary>
        /// Destination DICOM Application Name.
        /// </summary>
        /// <remarks>
        /// Pdu Bytes 11-26 correspond with the Called AE Title of the A-ASSOCIATE-RQ.
        /// </remarks>
        /// <remarks>
        /// This reserved field shall be sent with a value identical to the value
        /// received in the same field of the A-ASSOCIATE-RQ PDU, but its value
        /// shall not be tested when received.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        //[XmlElementAttribute("CalledAETitle")]
        public string CalledAETitle
        {
            get
            {
                return _CalledAETitle;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _CalledAETitle = value;
            }
        }
        private string _CalledAETitle = "Called AE Title";

        /// <summary>
        /// Source DICOM Application Name.
        /// </summary>
        /// <remarks>
        /// Pdu Bytes 27-42 correspond with the Called AE Title of the A-ASSOCIATE-RQ.
        /// </remarks>
        /// <remarks>
        /// This reserved field shall be sent with a value identical to the value
        /// received in the same field of the A-ASSOCIATE-RQ PDU, but its value
        /// shall not be tested when received.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        //[XmlElementAttribute("CallingAETitle")]
        public string CallingAETitle
        {
            get
            {
                return _CallingAETitle;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _CallingAETitle = value;
            }
        }
        private string _CallingAETitle = "Calling AE Title";

        /// <summary>
        /// An Application Context explicitly defines the set of Application Service Elements, 
        /// related options and any other information necessary for the 
        /// inter-working of DICOM AEs on an Association.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public ApplicationContext ApplicationContext
        {
            get
            {
                return _ApplicationContext;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _ApplicationContext = value;
            }
        }
        private ApplicationContext _ApplicationContext = new ApplicationContext();

        /// <summary>
        /// A Presentation Context defines the presentation of the data on an Association. 
        /// It provides a lower level of negotiation and one or more Presentation Contexts 
        /// can be offered and accepted per Association.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public AcceptedPresentationContextList PresentationContexts
        {
            get
            {
                return _PresentationContexts;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _PresentationContexts = value;
            }
        }
        private AcceptedPresentationContextList _PresentationContexts
            = new AcceptedPresentationContextList();

        /// <summary>
        /// Add an array of <see cref="DvtkData.Dul.AcceptedPresentationContext"/> items to <see cref="DvtkData.Dul.A_ASSOCIATE_AC"/>
        /// </summary>
        /// <param name="presentationContexts">Array of <see cref="DvtkData.Dul.AcceptedPresentationContext"/> items to add.</param>
        public void AddPresentationContexts(params AcceptedPresentationContext[] presentationContexts)
        {
            foreach (AcceptedPresentationContext presentationContext in presentationContexts)
            {
                _PresentationContexts.Add(presentationContext);
            }
        }

        /// <summary>
        /// Peer DICOM AEs negotiate, at Association establishment, 
        /// a number of features related to the Dimse protocol by using 
        /// the ACSE User Information Item of the A-ASSOCIATE request. 
        /// </summary>
        /// <remarks>
        /// When the Association is established between peer Dimse-service-users 
        /// the Kernel Functional Unit shall be assumed; 
        /// therefore, the Kernel Functional Unit shall not be included in 
        /// the A-ASSOCIATE User Information item.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public UserInformation UserInformation
        {
            get
            {
                return _UserInformation;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _UserInformation = value;
            }
        }
        private UserInformation _UserInformation = new UserInformation();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<AssociateAc>");
            streamWriter.WriteLine("<ProtocolVersion>{0}</ProtocolVersion>", ProtocolVersion.ToString());
            streamWriter.WriteLine("<CalledAETitle>{0}</CalledAETitle>", CalledAETitle);
            streamWriter.WriteLine("<CallingAETitle>{0}</CallingAETitle>", CallingAETitle);
            ApplicationContext.DvtDetailToXml(streamWriter, level);
            PresentationContexts.DvtDetailToXml(streamWriter, level);
            UserInformation.DvtDetailToXml(streamWriter, level);
            streamWriter.WriteLine("</AssociateAc>");

            return true;
        }
    }

    /// <summary>
    /// The establishment of an association between two AEs shall be performed 
    /// through ACSE A-ASSOCIATE request, indication, response and confirmation primitives. 
    /// </summary>
    /// <remarks>
    /// The initiator of the service is hereafter called a requestor and the service-user 
    /// which receives the A-ASSOCIATE indication is hereafter called the acceptor. 
    /// </remarks>
    /// <remarks>
    /// It shall be a confirmed service.
    /// </remarks>
    public class A_ASSOCIATE_RQ : DulMessage
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public A_ASSOCIATE_RQ() { }

        /// <summary>
        /// Specific constructor.
        /// </summary>
        /// <param name="protocolVersion">Protocol Version.</param>
        /// <param name="calledAETitle">Called Application Entity Title.</param>
        /// <param name="callingAETitle">Calling Application Entity Title.</param>
        /// <param name="applicationContextName">Application Context Name.</param>
        /// <param name="maximumLengthReceived">Maximum Length Received.</param>
        /// <param name="implementationClassUid">Implementation Class Unique Identifier.</param>
        public A_ASSOCIATE_RQ(
            System.UInt16 protocolVersion,
            string calledAETitle,
            string callingAETitle,
            string applicationContextName,
            System.UInt16 maximumLengthReceived,
            string implementationClassUid)
        {
            this.ProtocolVersion = protocolVersion;
            this.CalledAETitle = calledAETitle;
            this.CallingAETitle = callingAETitle;
            this.ApplicationContext.Name = applicationContextName;
            this.UserInformation.MaximumLength.MaximumLengthReceived =
                maximumLengthReceived;
            this.UserInformation.ImplementationClassUid.UID =
                implementationClassUid;
        }

        /// <summary>
        /// Protocol Data Unit - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduType.A_ASSOCIATE_RQ"/></remarks>
        public override PduType PduType
        {
            get
            {
                return PduType.A_ASSOCIATE_RQ;
            }
        }

        /// <summary>
        /// This two byte field shall use one bit to identify each version of the
        /// DICOM UL protocol supported by the calling end-system.
        /// This is Version 1 and shall be identified with bit 0 set. 
        /// A receiver of this PDU implementing only this version of the DICOM UL 
        /// protocol shall only test that bit 0 is set.
        /// </summary>
        //[XmlElementAttribute("ProtocolVersion")]
        public System.UInt16 ProtocolVersion
        {
            get
            {
                return _ProtocolVersion;
            }
            set
            {
                _ProtocolVersion = value;
            }
        }
        private System.UInt16 _ProtocolVersion = 1;

        /// <summary>
        /// Destination DICOM Application Name.
        /// </summary>
        /// <remarks>
        /// It shall be encoded as 16 characters as defined by the 
        /// ISO 646:1990-Basic G0 Set with leading and trailing spaces (20H) being non-significant.
        /// The value made of 16 spaces (20H) meaning “no Application Name specified” shall not be used. 
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        //[XmlElementAttribute("CalledAETitle")]
        public string CalledAETitle
        {
            get
            {
                return _CalledAETitle;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _CalledAETitle = value;
            }
        }
        private string _CalledAETitle = "Called AE Title";

        /// <summary>
        /// Source DICOM Application Name.
        /// </summary>
        /// <remarks>
        /// It shall be encoded as 16 characters as defined by the 
        /// ISO 646:1990-Basic G0 Set with leading and trailing spaces (20H) being non-significant. 
        /// The value made of 16 spaces (20H) meaning “no Application Name specified” shall not be used.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        //[XmlElementAttribute("CallingAETitle")]
        public string CallingAETitle
        {
            get
            {
                return _CallingAETitle;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _CallingAETitle = value;
            }
        }
        private string _CallingAETitle = "Calling AE Title";

        /// <summary>
        /// An Application Context explicitly defines the set of Application Service Elements, 
        /// related options and any cother information necessary for the inter-working 
        /// of DICOM AEs on an Association.
        /// </summary>
        /// <remarks>
        /// The Application Context provides the highest level of negotiation, therefore, 
        /// a very high level definition.
        /// </remarks>
        /// <remarks>
        /// Only one Application Context shall be offered per Association. 
        /// </remarks>
        /// <remarks>
        /// DICOM specifies a single Application Context Name which 
        /// defines the DICOM Application Context 
        /// (applicable for this Standard and potentially later versions).
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public ApplicationContext ApplicationContext
        {
            get
            {
                return _ApplicationContext;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _ApplicationContext = value;
            }
        }
        private ApplicationContext _ApplicationContext = new ApplicationContext();

        /// <summary>
        /// A Presentation Context defines the presentation of the data on an Association. 
        /// </summary>
        /// <remarks>
        /// It provides a lower level of negotiation and 
        /// one or more Presentation Contexts can be offered and accepted per Association.
        /// </remarks>
        /// <remarks>
        /// A Presentation Context consists of three components, 
        /// a Presentation Context ID, an Abstract Syntax Name, 
        /// and a list of one or more Transfer Syntax Names.
        /// </remarks>
        /// <remarks>
        /// Only one Abstract Syntax shall be offered per Presentation Context. 
        /// However, multiple Transfer Syntaxes may be offered per Presentation Context, 
        /// but only one shall be accepted.
        /// </remarks>
        /// <remarks>
        /// For each SOP Class or Meta SOP Class a Presentation Context must be negotiated 
        /// such that this Presentation Context supports the associated Abstract Syntax 
        /// and a suitable Transfer Syntax.
        /// </remarks>
        /// <remarks>
        /// Presentation Contexts will be identified within the scope of a 
        /// specific Association by a Presentation Context ID.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public RequestedPresentationContextList PresentationContexts
        {
            get
            {
                return _PresentationContexts;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _PresentationContexts = value;
            }
        }
        private RequestedPresentationContextList _PresentationContexts =
            new RequestedPresentationContextList();

        /// <summary>
        /// Add an array of <see cref="DvtkData.Dul.RequestedPresentationContext"/> items to <see cref="DvtkData.Dul.A_ASSOCIATE_RQ"/>
        /// </summary>
        /// <param name="presentationContexts">Array of <see cref="DvtkData.Dul.RequestedPresentationContext"/> items to add.</param>
        public void AddPresentationContexts(params RequestedPresentationContext[] presentationContexts)
        {
            foreach (RequestedPresentationContext presentationContext in presentationContexts)
            {
                _PresentationContexts.Add(presentationContext);
            }
        }

        /// <summary>
        /// Add a new <see cref="DvtkData.Dul.RequestedPresentationContext"/> item to <see cref="DvtkData.Dul.A_ASSOCIATE_RQ"/>
        /// </summary>
        /// <returns>Newly created <see cref="DvtkData.Dul.RequestedPresentationContext"/></returns>
        public RequestedPresentationContext AddNewPresentationContext()
        {
            RequestedPresentationContext presentationContext = new RequestedPresentationContext();
            AddPresentationContexts(presentationContext);
            return presentationContext;
        }

        /// <summary>
        /// Peer DICOM AEs negotiate, at Association establishment, 
        /// a number of features related to the Dimse protocol by using the 
        /// ACSE User Information Item of the A-ASSOCIATE request. 
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public UserInformation UserInformation
        {
            get
            {
                return _UserInformation;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                _UserInformation = value;
            }
        }
        private UserInformation _UserInformation = new UserInformation();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<AssociateRq>");
            streamWriter.WriteLine("<ProtocolVersion>{0}</ProtocolVersion>", ProtocolVersion.ToString());
            streamWriter.WriteLine("<CalledAETitle>{0}</CalledAETitle>", CalledAETitle);
            streamWriter.WriteLine("<CallingAETitle>{0}</CallingAETitle>", CallingAETitle);
            ApplicationContext.DvtDetailToXml(streamWriter, level);
            PresentationContexts.DvtDetailToXml(streamWriter, level);
            UserInformation.DvtDetailToXml(streamWriter, level);
            streamWriter.WriteLine("</AssociateRq>");

            return true;
        }
    }

    /// <summary>
    /// The ACSE A-ABORT service shall be used by a requestor in either of the 
    /// AEs to cause the abnormal release of the association.
    /// </summary>
    /// <remarks>
    /// It shall be a non-confirmed service.
    /// </remarks>
    /// <remarks>
    /// However, because of the possibility of an A-ABORT service procedure collision, 
    /// the delivery of the indication primitive is not guaranteed. 
    /// </remarks>
    /// <remarks>
    /// Should such a collision occur, both AEs are aware that the association has been terminated. 
    /// The abort shall be performed through A-ABORT request and A-ABORT indication primitives.
    /// </remarks>
    /// <summary>
    /// The ACSE A-P-ABORT service shall be used by the UL service-provider to 
    /// signal the abnormal release of the association due to problems in services at 
    /// the Presentation Layer and below. 
    /// </summary>
    /// <remarks>
    /// This occurrence indicates the possible loss of information in transit. 
    /// </remarks>
    /// <remarks>
    /// A-P-ABORT is a provider-initiated service.
    /// </remarks>
    public class A_ABORT : DulMessage
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>Required by Xml serialization.</remarks>
        public A_ABORT() { }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        /// <param name="reason">This parameter indicates the reason of this abort.</param>
        /// <param name="source">This parameter indicates the initiating source of this abort.</param>
        public A_ABORT(System.Byte reason, System.Byte source)
        {
            this._Reason = reason;
            this._Source = source;
        }

        /// <summary>
        /// Protocol Data Unit - Type Identifier.
        /// </summary>
        /// <remarks>Returns <see cref="DvtkData.Dul.PduType.A_ABORT"/></remarks>
        public override PduType PduType
        {
            get
            {
                return PduType.A_ABORT;
            }
        }

        /// <summary>
        /// This field shall contain an integer value encoded as an unsigned binary number.
        /// </summary>
        /// <remarks>
        /// <p>
        /// If the Source field has the value (2) “DICOM UL service-provider,” 
        /// it shall take one of the following:<br></br>
        /// 0 - reason-not-specified<br></br>
        /// 1 - unrecognized-PDU<br></br>
        /// 2 - unexpected-PDU<br></br>
        /// 3 - reserved<br></br>
        /// 4 - unrecognized-PDU parameter<br></br>
        /// 5 - unexpected-PDU parameter<br></br>
        /// 6 - invalid-PDU-parameter value
        /// </p>
        /// <p>
        /// If the Source field has the value (0) “DICOM UL service-user,” 
        /// reason field shall not be significant.
        /// It shall be sent with a value 00H but not tested to this value when received.
        /// </p>
        /// </remarks>
        public System.Byte Reason
        {
            get
            {
                return _Reason;
            }
            set
            {
                _Reason = value;
            }
        }
        private System.Byte _Reason = Reason_Dul_Service_Provider_Reason_Not_Specified;

        /// <summary>
        /// This Source field shall contain an integer value encoded as an unsigned binary number.
        /// </summary>
        /// <remarks>
        /// One of the following values shall be used:<br></br>
        /// 0 - DICOM UL service-user (initiated abort)<br></br>
        /// 1 - reserved<br></br>
        /// 2 - DICOM UL service-provider (initiated abort)
        /// </remarks>
        public System.Byte Source
        {
            get
            {
                return _Source;
            }
            set
            {
                _Source = value;
            }
        }
        private System.Byte _Source = Source_Dul_Service_Provider;

        /// <summary>
        /// Predefined Reason: Dul Service Provider - Reason Not Specified = 0
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_Reason_Not_Specified = 0;
        /// <summary>
        /// Predefined Reason: Dul Service Provider - Unrecognized PDU = 1
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_Unrecognized_PDU = 1;
        /// <summary>
        /// Predefined Reason: Dul Service Provider - Unexpected PDU = 2
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_Unexpected_PDU = 2;
        /// <summary>
        /// Predefined Reason: Dul Service Provider - Reserved 3 = 3
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_Reserved_3 = 3;
        /// <summary>
        /// Predefined Reason: Dul Service Provider - Unrecognized PDU Parameter = 4
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_Unrecognized_PDU_Parameter = 4;
        /// <summary>
        /// Predefined Reason: Dul Service Provider - Unexpected PDU Parameter = 5
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_Unexpected_PDU_Parameter = 5;
        /// <summary>
        /// Predefined Reason: Dul Service Provider - Invalid PDU Parameter = 6
        /// </summary>
        public const System.Byte Reason_Dul_Service_Provider_Invalid_PDU_Parameter = 6;

        /// <summary>
        /// Predefined Reason: Dul Service User - User Reason Not Specified = 0
        /// </summary>
        public const System.Byte Reason_Dul_Service_User_Reason_Not_Specified = 0;

        /// <summary>
        /// Predefined Source: Dul Service User = 1
        /// </summary>
        public const System.Byte Source_Dul_Service_User = 1;
        /// <summary>
        /// Predefined Source: Reserved 2 = 2
        /// </summary>
        public const System.Byte Source_Reserved_2 = 2;
        /// <summary>
        /// Predefined Source: Dul Service Provider = 3
        /// </summary>
        public const System.Byte Source_Dul_Service_Provider = 3;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<AbortRq>");
            streamWriter.WriteLine("<Source>{0}</Source>", Source.ToString());
            streamWriter.WriteLine("<Reason>{0}</Reason>", Reason.ToString());
            streamWriter.WriteLine("</AbortRq>");

            return true;
        }
    }
}

