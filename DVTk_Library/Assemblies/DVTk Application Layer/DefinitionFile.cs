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

namespace DvtkApplicationLayer
{
    /// <summary>
    /// Summary description for DefinitionFile.
    /// </summary>
    public class DefinitionFile
    {
        /// <summary>
        /// Class represents Definition file details
        /// </summary>
        /// <param name="loaded"></param>
        /// <param name="filename"></param>
        /// <param name="sop_class_name"></param>
        /// <param name="sop_class_uid"></param>
        /// <param name="ae_title"></param>
        /// <param name="ae_version"></param>
        /// <param name="definition_root"></param>
		public DefinitionFile(bool loaded,
                              string filename,
                              string sop_class_name,
                              string sop_class_uid,
                              string ae_title,
                              string ae_version,
                              string definition_root)
        {
            this.df_loaded = loaded;
            this.df_filename = filename;
            this.df_sop_class_name = sop_class_name;
            this.df_sop_class_uid = sop_class_uid;
            this.df_ae_title = ae_title;
            this.df_ae_version = ae_version;
            this.df_definition_root = definition_root;
        }

        /// <summary>
        /// Definition file load property
        /// </summary>
        public bool Loaded
        {
            get { return this.df_loaded; }
            set { this.df_loaded = value; }
        }

        /// <summary>
        /// Definition file name property
        /// </summary>
        public string Filename
        {
            get { return this.df_filename; }
            set { this.df_filename = value; }
        }

        /// <summary>
        /// SOP Class name property
        /// </summary>
        public string SOPClassName
        {
            get { return this.df_sop_class_name; }
            set { this.df_sop_class_name = value; }
        }

        /// <summary>
        /// SOP Class UID property
        /// </summary>
        public string SOPClassUID
        {
            get { return this.df_sop_class_uid; }
            set { this.df_sop_class_uid = value; }
        }

        /// <summary>
        /// AE Title property
        /// </summary>
        public string AETitle
        {
            get { return this.df_ae_title; }
            set { this.df_ae_title = value; }
        }

        /// <summary>
        /// AE Version property
        /// </summary>
        public string AEVersion
        {
            get { return this.df_ae_version; }
            set { this.df_ae_version = value; }
        }

        /// <summary>
        /// Def directory property
        /// </summary>
        public string DefinitionRoot
        {
            get { return this.df_definition_root; }
            set { this.df_definition_root = value; }
        }

        private bool df_loaded;
        private string df_filename;
        private string df_sop_class_name;
        private string df_sop_class_uid;
        private string df_ae_title;
        private string df_ae_version;
        private string df_definition_root;
    }
}
