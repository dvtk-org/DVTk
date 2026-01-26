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

namespace Dvtk.Sessions
{
    using System.Collections;
    //
    // Aliases for types
    //
    using DefinitionFileRootDirectory = System.String;
    using DefinitionFileName = System.String;
    using DefinitionFileDirectory = System.String;

    /// <summary>
    /// Essential information for a definition file.
    /// </summary>
    /// <remarks>
    /// This information can be used to determine whether to load the file for the System Under Test (SUT).
    /// </remarks>
    public struct DefinitionFileDetails
    {
        /// <summary>
        /// The application entity name for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// This name is used to select the appropriate definitions from the 
        /// definition files for the corresponding SUT.
        /// </remarks>
        public System.String ApplicationEntityName;
        /// <summary>
        /// The application entity version for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// This version is used to select the appropriate definitions from the 
        /// definition files for the corresponding SUT.
        /// </remarks>
        public System.String ApplicationEntityVersion;
        /// <summary>
        /// Service Object Pair Class Name
        /// </summary>
        /// <remarks>Literal string corresponding to the SOPClassUID 
        /// as defined in part 6 of the DICOM standard.</remarks>
        public System.String SOPClassName;
        /// <summary>
        /// Service Object Pair Class Unique Identifier
        /// </summary>
        /// <remarks>
        /// Uniquely identifies the type of DIMSE message (Service Object Pair Class). 
        /// Each SOP Class is defined by its own definition.
        /// </remarks>
        public System.String SOPClassUID;
        /// <summary>
        /// This flag indicates whether the SOP Class is a Meta SOP Class.
        /// </summary>
        /// <remarks>
        /// For instance;
        /// <list type="bullet">
        /// <item>Basic Grayscale Print Management Meta SOP Class</item>
        /// <item>General Purpose Worklist Management Meta SOP Class</item>
        /// </list>
        /// </remarks>
        public System.Boolean IsMetaSOPClass;
    }
    /// <summary>
    /// Manage definitions used by the validation process.
    /// </summary>
    /// <remarks>
    /// <p>
    /// A Definition File (.DEF) describes a single DICOM (Meta) SOP Class in terms of the 
    /// combination of DIMSE Commands and IODs that make up the (Meta) SOP Class. 
    /// The combination of DIMSE Commands and IODs is taken from DICOM - parts 3 and 4.
    /// </p>
    /// <p>
    /// The Definition Files provide DVT with the DICOM specific knowledge to enable a validation.
    /// </p>
    /// <p>
    /// The Definition Files in this version of DVT support the MACRO syntax used in 
    /// DICOM and describe the conditions under which type 1C and 2C attributes should be present.
    /// </p>
    /// <p>
    /// Standard Definition Files for the most frequently used DICOM (Meta) SOP Classes 
    /// are provided as part of the DVT release package. Private Definition Files can be made by the User.
    /// DVT also provides and uses several Special Definition Files.
    /// </p>
    /// </remarks>
    public interface IDefinitionManagement
    {
        /// <summary>
        /// Load definition file.
        /// </summary>
        /// <param name="definitionFileName">Absolute or relative file name for the definition file.</param>
        /// <returns><see langword="false"/> if failed</returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>definitionFileName</c> is a <see langword="null"/> reference.</exception>
        System.Boolean LoadDefinitionFile(DefinitionFileName definitionFileName);
        /// <summary>
        /// Unload definition file.
        /// </summary>
        /// <param name="definitionFileName">Absolute or relative file name for the definition file.</param>
        /// <returns><see langword="false"/> if failed</returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>definitionFileName</c> is a <see langword="null"/> reference.</exception>
        System.Boolean UnLoadDefinitionFile(DefinitionFileName definitionFileName);
        /// <summary>
        /// Reload definition files.
        /// </summary>
        /// <remarks>
        /// May be used after changes to the definition root. In order to ensure the proper definition files
        /// are in memory.
        /// </remarks>
        /// <returns><see langword="false"/> if failed</returns>
        System.Boolean ReLoadDefinitionFiles();
        /// <summary>
        /// Unload definition files.
        /// </summary>
        /// <remarks>
        /// Unloads all currently loaded definition files.
        /// </remarks>
        void UnLoadDefinitionFiles();
        /// <summary>
        /// Definition File Root Directory
        /// </summary>
        /// <remarks>
        /// <p>
        /// The Definition File Root Driectory is used to prefix the definition file names 
        /// in order to define the full definition pathname.
        /// </p>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">Directory may not be an empty string. Use ".\" for current directory."</exception>
        DefinitionFileRootDirectory DefinitionFileRootDirectory
        {
            get;
            set;
        }
        /// <summary>
        /// The list of currently loaded definition file names.
        /// </summary>
        DefinitionFileName[] LoadedDefinitionFileNames
        {
            get;
        }
        /// <summary>
        /// The application entity name for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// This name is used to select the appropriate definitions from the 
        /// definition files for the corresponding SUT.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        System.String ApplicationEntityName
        {
            get;
            set;
        }
        /// <summary>
        /// The application entity version for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// This version is used to select the appropriate definitions from the 
        /// definition files for the corresponding SUT.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        System.String ApplicationEntityVersion
        {
            get;
            set;
        }
        /// <summary>
        /// Retrieves essential information from a definition file.
        /// </summary>
        /// <remarks>
        /// This information can be used to determine whether to load the file for the System Under Test (SUT).
        /// </remarks>
        /// <param name="definitionFileName">Absolute or relative file name for the definition file.</param>
        /// <returns>The details about the definition file.</returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>definitionFileName</c> is a <see langword="null"/> reference.</exception>
        DefinitionFileDetails GetDefinitionFileDetails(DefinitionFileName definitionFileName);

        /// <summary>
        /// Definition-file directory list.
        /// </summary>
        DefinitionFileDirectoryList DefinitionFileDirectoryList { get; }

        /// <summary>
        /// Get the definition Iod Name from the loaded definition files.
        /// </summary>
        /// <param name="command">Dimse Command.</param>
        /// <param name="uid">SOP Class UID.</param>
        /// <returns>Iod Name of definition matching the Dimse Command and SOP Class UID combination.</returns>
        System.String GetIodNameFromDefinition(DvtkData.Dimse.DimseCommand command, System.String uid);

        /// <summary>
        /// Get the definition File Name from the loaded definition files.
        /// </summary>
        /// <param name="command">Dimse Command.</param>
        /// <param name="uid">SOP Class UID.</param>
        /// <returns>File Name of definition matching the Dimse Command and SOP Class UID combination.</returns>
        System.String GetFileNameFromSOPUID(DvtkData.Dimse.DimseCommand command, System.String uid);

        /// <summary>
        /// Get the definition Attribute Name from the loaded definition files.
        /// </summary>
        /// <param name="tag">Attribute Tag.</param>
        /// <returns>Attribute Name.</returns>
        System.String GetAttributeNameFromDefinition(DvtkData.Dimse.Tag tag);

        /// <summary>
        /// Get the definition Attribute VR from the loaded definition files.
        /// </summary>
        /// <param name="tag">Attribute Tag.</param>
        /// <returns>Attribute VR.</returns>
        DvtkData.Dimse.VR GetAttributeVrFromDefinition(DvtkData.Dimse.Tag tag);
    }

    internal class DefinitionManagement : IDefinitionManagement
    {
        internal DefinitionManagement(Wrappers.MBaseSession adaptee)
        {
            if (adaptee == null) throw new System.ArgumentNullException("adaptee");
            m_adaptee = adaptee;
            //
            // load list from the values in the session
            //
            System.UInt16 nrOfDefinitionFileDirectories = this.m_adaptee.NrOfDefinitionFileDirectories;
            for (System.UInt16 index = 0; index < nrOfDefinitionFileDirectories; index++)
            {
                this._DefinitionFileDirectoryList.Add(this.m_adaptee.get_DefinitionFileDirectory(index));
            }
            //
            // subscribe to contents changes to call underlying MBaseSession methods.
            //
            this._DefinitionFileDirectoryList.ListChanged +=
                new System.ComponentModel.ListChangedEventHandler(_DefinitionFileDirectoryList_ListChanged);
        }
        protected Wrappers.MBaseSession m_adaptee = null;

        #region IDefinitionManagement
        /// <summary>
        /// <see cref="IDefinitionManagement.LoadDefinitionFile"/>
        /// </summary>
        public System.Boolean LoadDefinitionFile(DefinitionFileName definitionFileName)
        {
            if (definitionFileName == null) throw new System.ArgumentNullException("definitionFileName");
            return this.m_adaptee.LoadDefinitionFile(definitionFileName);
        }
        /// <summary>
        /// <see cref="IDefinitionManagement.UnLoadDefinitionFile"/>
        /// </summary>
        public System.Boolean UnLoadDefinitionFile(DefinitionFileName definitionFileName)
        {
            if (definitionFileName == null) throw new System.ArgumentNullException("definitionFileName");
            return this.m_adaptee.UnLoadDefinitionFile(definitionFileName);
        }
        /// <summary>
        /// <see cref="IDefinitionManagement.ReLoadDefinitionFiles"/>
        /// </summary>
        public System.Boolean ReLoadDefinitionFiles()
        {
            return this.m_adaptee.ReloadDefinitions();
        }
        /// <summary>
        /// <see cref="IDefinitionManagement.UnLoadDefinitionFiles"/>
        /// </summary>
        public void UnLoadDefinitionFiles()
        {
            this.m_adaptee.RemoveDefinitions();
            return;
        }
        /// <summary>
        /// <see cref="IDefinitionManagement.DefinitionFileRootDirectory"/>
        /// </summary>
        public DefinitionFileRootDirectory DefinitionFileRootDirectory
        {
            get
            {
                return this.m_adaptee.DefinitionFileRootDirectory;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                if (value == string.Empty)
                    throw new System.ArgumentException(
                        "DefinitionFileRootDirectory may not be an empty string.\n" +
                        "Use \".\" for current directory.");
                this.m_adaptee.DefinitionFileRootDirectory = value;
            }
        }
        /// <summary>
        /// <see cref="IDefinitionManagement.LoadedDefinitionFileNames"/>
        /// </summary>
        public DefinitionFileName[] LoadedDefinitionFileNames
        {
            get
            {
                System.UInt16 size = this.m_adaptee.NrOfDefinitionFiles;
                ArrayList m_data = new ArrayList(size);
                for (System.UInt16 idx = 0; idx < size; idx++)
                {
                    DefinitionFileName definitionFileName =
                        this.m_adaptee.get_DefinitionFileName(idx);
                    m_data.Add(definitionFileName);
                }
                return (DefinitionFileName[])m_data.ToArray(typeof(DefinitionFileName));
            }
        }
        /// <summary>
        /// <see cref="IDefinitionManagement.ApplicationEntityName"/>
        /// </summary>
        public System.String ApplicationEntityName
        {
            get
            {
                return this.m_adaptee.ApplicationEntityName;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.ApplicationEntityName = value;
            }
        }
        /// <summary>
        /// <see cref="IDefinitionManagement.ApplicationEntityVersion"/>
        /// </summary>
        public System.String ApplicationEntityVersion
        {
            get
            {
                return this.m_adaptee.ApplicationEntityVersion;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.ApplicationEntityVersion = value;
            }
        }
        /// <summary>
        /// <see cref="IDefinitionManagement.GetDefinitionFileDetails"/>
        /// </summary>
        public DefinitionFileDetails GetDefinitionFileDetails(DefinitionFileName definitionFileName)
        {
            if (definitionFileName == null) throw new System.ArgumentNullException("definitionFileName");
            Wrappers.MDefinitionFileDetails mDefinitionFileDetails =
                Wrappers.MDefinition.get_FileDetails(definitionFileName);
            DefinitionFileDetails definitionFileDetails = new DefinitionFileDetails();
            definitionFileDetails.ApplicationEntityName = mDefinitionFileDetails.AEName;
            definitionFileDetails.ApplicationEntityVersion = mDefinitionFileDetails.AEVersion;
            definitionFileDetails.SOPClassName = mDefinitionFileDetails.SOPClassName;
            definitionFileDetails.SOPClassUID = mDefinitionFileDetails.SOPClassUID;
            definitionFileDetails.IsMetaSOPClass = mDefinitionFileDetails.IsMetaSOPClass;
            return definitionFileDetails;
        }

        /// <summary>
        /// <see cref="IDefinitionManagement.GetIodNameFromDefinition"/>
        /// </summary>
        public System.String GetIodNameFromDefinition(DvtkData.Dimse.DimseCommand command, System.String uid)
        {
            System.String iodName = this.m_adaptee.get_IodNameFromDefinition(command, uid);
            return iodName;
        }

        /// <summary>
        /// <see cref="IDefinitionManagement.GetFileNameFromSOPUID"/>
        /// </summary>
        public System.String GetFileNameFromSOPUID(DvtkData.Dimse.DimseCommand command, System.String uid)
        {
            System.String fileName = this.m_adaptee.get_FileNameFromSOPUID(command, uid);
            return fileName;
        }

        /// <summary>
        /// <see cref="IDefinitionManagement.GetAttributeNameFromDefinition"/>
        /// </summary>
        public System.String GetAttributeNameFromDefinition(DvtkData.Dimse.Tag tag)
        {
            System.String attributeName = this.m_adaptee.get_AttributeNameFromDefinition(tag);
            return attributeName;
        }

        /// <summary>
        /// <see cref="IDefinitionManagement.GetAttributeVrFromDefinition"/>
        /// </summary>
        public DvtkData.Dimse.VR GetAttributeVrFromDefinition(DvtkData.Dimse.Tag tag)
        {
            DvtkData.Dimse.VR vr = this.m_adaptee.get_AttributeVrFromDefinition(tag);
            return vr;
        }

        #endregion

        #region DefinitionFileDirectoryList
        public DefinitionFileDirectoryList DefinitionFileDirectoryList
        {
            get { return _DefinitionFileDirectoryList; }
        }

        private DefinitionFileDirectoryList _DefinitionFileDirectoryList =
            new DefinitionFileDirectoryList();

        private void _DefinitionFileDirectoryList_ListChanged(
            object sender,
            System.ComponentModel.ListChangedEventArgs e)
        {
            // Refresh underlying list
            this.m_adaptee.RemoveAllDefinitionFileDirectories();
            foreach (System.String definitionFileDirectory in this._DefinitionFileDirectoryList)
            {
                this.m_adaptee.AddDefinitionFileDirectory(definitionFileDirectory);
            }
        }
        #endregion DefinitionFileDirectoryList
    }

    /// <summary>
    /// Type safe DefinitionFileDirectoryList
    /// </summary>
    public sealed class DefinitionFileDirectoryList :
        DvtkData.Collections.NullSafeCollectionBase
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
        /// Occurs when the list managed by the <see cref="DefinitionFileDirectoryList"/> changes.
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
        public DefinitionFileDirectoryList() { }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified <c>index</c>.</value>
        /// <exception cref="System.ArgumentException">Directory does not exist.</exception>
        public new DefinitionFileDirectory this[int index]
        {
            get
            {
                return (DefinitionFileDirectory)base[index];
            }
            set
            {
                System.IO.DirectoryInfo directoryInfo =
                    new System.IO.DirectoryInfo(value);
                if (!directoryInfo.Exists)
                    throw new System.ArgumentException(
                        $"Definition file directory '{value}' does not exist.");
                base.Insert(index, value);
            }
        }

        /// <summary>
        /// Inserts an item to the IList at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted.</param>
        /// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
        /// <exception cref="System.ArgumentException">Directory does not exist.</exception>
        public void Insert(int index, DefinitionFileDirectory value)
        {
            System.IO.DirectoryInfo directoryInfo =
                new System.IO.DirectoryInfo(value);
            if (!directoryInfo.Exists)
                throw new System.ArgumentException(
                    $"Definition file directory '{value}' does not exist.");
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the IList.
        /// </summary>
        /// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
        public void Remove(DefinitionFileDirectory value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(DefinitionFileDirectory value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
        /// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
        public int IndexOf(DefinitionFileDirectory value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
        /// <returns>The position into which the new element was inserted.</returns>
        /// <exception cref="System.ArgumentException">Directory does not exist.</exception>
        public int Add(DefinitionFileDirectory value)
        {
            System.IO.DirectoryInfo directoryInfo =
                new System.IO.DirectoryInfo(value);
            if (!directoryInfo.Exists)
                throw new System.ArgumentException(
                    $"Definition file directory '{value}' does not exist.");
            return base.Add(value);
        }
    }
}
