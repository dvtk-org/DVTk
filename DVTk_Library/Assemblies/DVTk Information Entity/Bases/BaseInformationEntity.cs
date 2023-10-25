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
using System.Text;
using System.Text.RegularExpressions;

using DvtkData.Dimse;
using Dvtk.Dicom.InformationEntity.AttributeMatching;
using Dvtk.Dicom.InformationEntity.CompositeInfoModel;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using DvtkData.Collections;
using Dvtk.Dicom.InformationEntity.Worklist;

namespace Dvtk.Dicom.InformationEntity
{
	/// <summary>
	/// Summary description for BaseInformationEntity.
	/// </summary>
	public abstract class BaseInformationEntity
	{
		private System.String _level;
		private DataSet _dataset = null;
		private DataSet _additionalDatasetOverWrite = null;
		private DataSet _additionalDatasetNoOverWrite = null;
		private BaseInformationEntity _parent = null;
		private BaseInformationEntityList _children = new BaseInformationEntityList();
		private TagTypeList _tagTypeList = new TagTypeList();

		/// <summary>
		/// Class Constructor.
		/// </summary>
		/// <param name="level">Entity level in the Information Model.</param>
		public BaseInformationEntity(System.String level)
		{
			_level = level;
			_dataset = new DataSet(_level);
			_additionalDatasetOverWrite = new DataSet(_level + " Additional OverWrite");
			_additionalDatasetNoOverWrite = new DataSet(_level + " Additional NoOverWrite");

			// set up the default Tag Type List
			SetDefaultTagTypeList();
		}

		/// <summary>
		/// Get the Entity Level in the Information Model.
		/// </summary>
		public System.String Level
		{
			get
			{
				return _level;
			}
		}

		/// <summary>
		/// Get the local DataSet used to store the Entity attribute values.
		/// </summary>
		public DataSet DataSet
		{
			get
			{
				return _dataset;
			}
			set
			{
				_dataset = value;
			}
		}

		/// <summary>
		/// Get the Entity child list in the Information Model.
		/// </summary>
		public BaseInformationEntityList Children
		{
			get
			{
				return _children;
			}
		}

        public BaseInformationEntityList ChildrenWithUniqueTagFoundIn(AttributeSet matchDataset)
        {
            BaseInformationEntityList baseInformationEntityList = new BaseInformationEntityList();

            foreach(BaseInformationEntity baseInformationEntity in Children)
            {
                if (baseInformationEntity.IsUniqueTagFoundIn(matchDataset))
                {
                    baseInformationEntityList.Add(baseInformationEntity);
                }
            }

            return(baseInformationEntityList);
        }

		/// <summary>
		/// Get the Entity parent in the Information Model.
		/// </summary>
		public BaseInformationEntity Parent
		{
			get
			{
				return _parent;
			}
		}

		/// <summary>
		/// Set/Get the Tag Type list used to interogate the Entity.
		/// </summary>
		public TagTypeList TagTypeList
		{
			set
			{
				_tagTypeList = value;
			}
			get
			{
				return _tagTypeList;
			}
		}

		/// <summary>
		/// Add a child Entity to this.
		/// </summary>
		/// <param name="informationEntity">Child Entity being added.</param>
		public void AddChild(BaseInformationEntity informationEntity)
		{
			informationEntity._parent = this;
			_children.Add(informationEntity);
		}

		/// <summary>
		/// Copy from the given source Dataset into the local Dataset as defined by the
		/// default Tag Type list.
		/// </summary>
		/// <param name="sourceDataset">Source Dataset used to populate the local Dataset.</param>
		public virtual void CopyFrom(AttributeSet sourceDataset)
		{
			CopyFrom(_tagTypeList, sourceDataset);
		}

		/// <summary>
		/// Copy from the given source Dataset into the local Dataset as defined by the
		/// given Tag Type list.
		/// </summary>
		/// <param name="tagTypeList">Tag Type list identifying attributes to copy.</param>
		/// <param name="sourceDataset">Source Dataset used to populate the local Dataset.</param>
		public void CopyFrom(TagTypeList tagTypeList, AttributeSet sourceDataset)
		{
			if (tagTypeList != null)
			{
				foreach (TagType tagType in tagTypeList)
				{
					DvtkData.Dimse.Attribute sourceAttribute = sourceDataset.GetAttribute(tagType.Tag);
					if (sourceAttribute != null)
					{
						// if an entry already exists - remove it
						DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
                        if (destinationAttribute != null)
						{
							_dataset.Remove(destinationAttribute);
						}
                        //if (destinationAttribute != null && destinationAttribute.Tag == Tag.MODALITIES_IN_STUDY&&destinationAttribute.ValueRepresentation==VR.CS)
                        //{
                        //    StringCollection values = ((CodeString)destinationAttribute.DicomValue).Values;
                        //    string actualModality = ((CodeString)sourceDataset.GetAttribute(Tag.MODALITY).DicomValue).Values[0];

                        //    if (actualModality!=null&& !values.Contains(actualModality.Trim()))
                        //    {
                        //        values.Add(actualModality.Trim());
                        //    }
                        //    CodeString cs=new CodeString();
                        //    cs.Values=values;
                        //    sourceAttribute.DicomValue = cs;
                        //    System.UInt32 length = 0;
                        //    foreach (String data in cs.Values)
                        //    {
                        //        length += (System.UInt32)data.Length;
                        //    }
                        //    sourceAttribute.Length = length + (System.UInt32)cs.Values.Count - 1;
                        //}
						_dataset.Add(sourceAttribute);
					}
                    //else if (tagType.Tag == Tag.MODALITIES_IN_STUDY)
                    //{
                    //    sourceAttribute = new DvtkData.Dimse.Attribute();
                    //    sourceAttribute.Tag = Tag.MODALITIES_IN_STUDY;
                    //    sourceAttribute.Name = "Modalities in study";
                    //    DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
                    //    string actualModality = ((CodeString)sourceDataset.GetAttribute(Tag.MODALITY).DicomValue).Values[0];
                    //    StringCollection values=null;
                    //    if (destinationAttribute != null&&destinationAttribute.ValueRepresentation==VR.CS)
                    //    {
                    //        _dataset.Remove(destinationAttribute);
                    //        values = ((CodeString)destinationAttribute.DicomValue).Values;
                    //    }
                    //    if (values == null)
                    //        values = new StringCollection();
                    //    if (actualModality != null && !values.Contains(actualModality.Trim()))
                    //    {
                    //        values.Add(actualModality.Trim());
                    //    }
                    //    CodeString cs = new CodeString();
                    //    cs.Values = values;
                    //    sourceAttribute.DicomValue = cs;
                    //    System.UInt32 length = 0;
                    //    foreach (String data in cs.Values)
                    //    {
                    //        length += (System.UInt32)data.Length;
                    //    }
                    //    sourceAttribute.Length = length + (System.UInt32)cs.Values.Count - 1;
                    //    _dataset.Add(sourceAttribute);
                    //}
				}

			}
		}
        /// <summary>
        /// Includes the non-dataset attributes in the information model.
        /// Handled attributes are,
        /// MODALITIES_IN_STUDY
        /// NUMBER_OF_PATIENT_RELATED_STUDIES
        /// NUMBER_OF_PATIENT_RELATED_SERIES
        /// NUMBER_OF_PATIENT_RELATED_INSTANCES
        /// NUMBER_OF_STUDY_RELATED_SERIES
        /// NUMBER_OF_STUDY_RELATED_INSTANCES.
        /// NUMBER_OF_SERIES_RELATED_INSTANCES
        /// </summary>
        /// <param name="sourceDataset"></param>
        public void CheckForSpecialTags(AttributeSet sourceDataset)
        {
            if (_tagTypeList != null&&sourceDataset!=null)
            {
                foreach (TagType tagType in _tagTypeList)
                {
                    if (tagType.Tag == Tag.MODALITIES_IN_STUDY)
                    {
                        DvtkData.Dimse.Attribute sourceAttribute = sourceDataset.GetAttribute(tagType.Tag);
                        if (sourceAttribute != null)
                        {
                            // if an entry already exists - remove it
                            DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
                            if (destinationAttribute != null)
                            {
                                _dataset.Remove(destinationAttribute);
                            }
                            if (destinationAttribute != null && destinationAttribute.Tag == Tag.MODALITIES_IN_STUDY && destinationAttribute.ValueRepresentation == VR.CS)
                            {
                                StringCollection values = ((CodeString)destinationAttribute.DicomValue).Values;
                                string actualModality = ((CodeString)sourceDataset.GetAttribute(Tag.MODALITY).DicomValue).Values[0];

                                if (actualModality != null && !values.Contains(actualModality.Trim()))
                                {
                                    values.Add(actualModality.Trim());
                                }
                                CodeString cs = new CodeString();
                                cs.Values = values;
                                sourceAttribute.DicomValue = cs;
                                System.UInt32 length = 0;
                                foreach (String data in cs.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                sourceAttribute.Length = length + (System.UInt32)cs.Values.Count - 1;
                            }
                            _dataset.Add(sourceAttribute);
                        }
                        else if (tagType.Tag == Tag.MODALITIES_IN_STUDY)
                        {
                            sourceAttribute = new DvtkData.Dimse.Attribute();
                            sourceAttribute.Tag = Tag.MODALITIES_IN_STUDY;
                            sourceAttribute.Name = "Modalities in study";
                            DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
                            string actualModality = ((CodeString)sourceDataset.GetAttribute(Tag.MODALITY).DicomValue).Values[0];
                            StringCollection values = null;
                            if (destinationAttribute != null && destinationAttribute.ValueRepresentation == VR.CS)
                            {
                                _dataset.Remove(destinationAttribute);
                                values = ((CodeString)destinationAttribute.DicomValue).Values;
                            }
                            if (values == null)
                                values = new StringCollection();
                            if (actualModality != null && !values.Contains(actualModality.Trim()))
                            {
                                values.Add(actualModality.Trim());
                            }
                            CodeString cs = new CodeString();
                            cs.Values = values;
                            sourceAttribute.DicomValue = cs;
                            System.UInt32 length = 0;
                            foreach (String data in cs.Values)
                            {
                                length += (System.UInt32)data.Length;
                            }
                            sourceAttribute.Length = length + (System.UInt32)cs.Values.Count - 1;
                            _dataset.Add(sourceAttribute);
                        }
                    }
                    else if (tagType.Tag == Tag.NUMBER_OF_PATIENT_RELATED_STUDIES&&_level=="PATIENT")
                    {
                        HandlePatientRelatedStudies(tagType, sourceDataset);
                    }
                    else if (tagType.Tag == Tag.NUMBER_OF_PATIENT_RELATED_SERIES&& _level=="PATIENT")
                    {
                        HandlePatientRelatedSeries(tagType, sourceDataset);
                    }
                    else if (tagType.Tag == Tag.NUMBER_OF_PATIENT_RELATED_INSTANCES&&_level=="PATIENT")
                    {
                        HandlePatientRelatedInstances(tagType, sourceDataset);
                    }
                    else if (tagType.Tag == Tag.NUMBER_OF_STUDY_RELATED_SERIES&&_level=="STUDY")
                    {
                        HandleStudyRelatedSeries(tagType, sourceDataset);
                    }
                    else if (tagType.Tag == Tag.NUMBER_OF_STUDY_RELATED_INSTANCES&&_level=="STUDY")
                    {
                        HandleStudyRelatedInstances(tagType, sourceDataset);
                    }
                    else if (tagType.Tag == Tag.NUMBER_OF_SERIES_RELATED_INSTANCES&&_level=="SERIES")
                    {
                        HandleSeriesRelatedInstances(tagType, sourceDataset);
                    }
                }

            }
        }

        void HandlePatientRelatedStudies(TagType tagType, AttributeSet sourceDataset)
        {
            DvtkData.Dimse.Attribute sourceAttribute = sourceDataset.GetAttribute(tagType.Tag);
            if (sourceAttribute == null)
            {
                sourceAttribute = new DvtkData.Dimse.Attribute();
                sourceAttribute.Tag = Tag.NUMBER_OF_PATIENT_RELATED_STUDIES;
                sourceAttribute.Name = "Number of Patient related studies";
            }
            DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
            if (destinationAttribute != null)
            {
                _dataset.Remove(destinationAttribute);
            }
            IntegerString str = new IntegerString();
            StringCollection colle = new StringCollection();
            colle.Add(Children.Count.ToString());
            str.Values = colle;
            System.UInt32 length = 0;
            foreach (String data in str.Values)
            {
                length += (System.UInt32)data.Length;
            }
            sourceAttribute.DicomValue = str;
            sourceAttribute.Length = length + (System.UInt32)str.Values.Count - 1;
            _dataset.Add(sourceAttribute);
        }

        void HandlePatientRelatedSeries(TagType tagType, AttributeSet sourceDataset)
        {
            DvtkData.Dimse.Attribute sourceAttribute = sourceDataset.GetAttribute(tagType.Tag);
            if (sourceAttribute == null)
            {
                sourceAttribute = new DvtkData.Dimse.Attribute();
                sourceAttribute.Tag = Tag.NUMBER_OF_PATIENT_RELATED_SERIES;
                sourceAttribute.Name = "Number of Patient related series";
            }
            DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
            if (destinationAttribute != null)
            {
                _dataset.Remove(destinationAttribute);
            }
            IntegerString str = new IntegerString();
            StringCollection colle = new StringCollection();
            int noOfSeries = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                noOfSeries = noOfSeries + Children[i].Children.Count;
            }
            colle.Add(noOfSeries.ToString());
            str.Values = colle;
            System.UInt32 length = 0;
            foreach (String data in str.Values)
            {
                length += (System.UInt32)data.Length;
            }
            sourceAttribute.DicomValue = str;
            sourceAttribute.Length = length + (System.UInt32)str.Values.Count - 1;
            _dataset.Add(sourceAttribute);


            //DvtkData.Dimse.Attribute sourceAttribute = sourceDataset.GetAttribute(tagType.Tag);
            //if (sourceAttribute != null)
            //{
            //    // if an entry already exists - remove it
            //    DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
            //    if (destinationAttribute != null)
            //    {
            //        _dataset.Remove(destinationAttribute);
            //    }
            //    if (destinationAttribute != null && destinationAttribute.Tag == Tag.NUMBER_OF_PATIENT_RELATED_STUDIES)
            //    {
            //        IntegerString str = new IntegerString();
            //        StringCollection colle = new StringCollection();
            //        int noOfSeries = 0;
            //        for (int i = 0; i < Children.Count; i++)
            //        {
            //            noOfSeries = noOfSeries + Children[i].Children.Count;
            //        }

            //        colle.Add(noOfSeries.ToString());
            //        str.Values = colle;
            //        System.UInt32 length = 0;
            //        foreach (String data in str.Values)
            //        {
            //            length += (System.UInt32)data.Length;
            //        }
            //        sourceAttribute.Length = length + (System.UInt32)str.Values.Count - 1;
            //    }
            //    _dataset.Add(sourceAttribute);
            //}
            //else
            //{
            //    sourceAttribute = new DvtkData.Dimse.Attribute();
            //    sourceAttribute.Tag = Tag.NUMBER_OF_PATIENT_RELATED_STUDIES;
            //    sourceAttribute.Name = "Number of Patient related series";
            //    DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
            //    StringCollection values = null;
            //    if (destinationAttribute != null)
            //    {
            //        _dataset.Remove(destinationAttribute);
            //    }
            //    values = new StringCollection();
            //    int noOfSeries = 0;
            //    for (int i = 0; i < Children.Count; i++)
            //    {
            //        noOfSeries = noOfSeries + Children[i].Children.Count;
            //    }
            //    values.Add(noOfSeries.ToString());

            //    IntegerString IS = new IntegerString();
            //    IS.Values = values;
            //    sourceAttribute.DicomValue = IS;
            //    System.UInt32 length = 0;
            //    foreach (String data in IS.Values)
            //    {
            //        length += (System.UInt32)data.Length;
            //    }
            //    sourceAttribute.Length = length + (System.UInt32)IS.Values.Count - 1;
            //    _dataset.Add(sourceAttribute);
            //}
        }

        void HandlePatientRelatedInstances(TagType tagType, AttributeSet sourceDataset)
        {
            DvtkData.Dimse.Attribute sourceAttribute = sourceDataset.GetAttribute(tagType.Tag);
            if (sourceAttribute == null)
            {
                sourceAttribute = new DvtkData.Dimse.Attribute();
                sourceAttribute.Tag = Tag.NUMBER_OF_PATIENT_RELATED_INSTANCES;
                sourceAttribute.Name = "Number of Patient related instances";
            }
            DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
            if (destinationAttribute != null)
            {
                _dataset.Remove(destinationAttribute);
            }
            IntegerString str = new IntegerString();
            StringCollection colle = new StringCollection();
            int noOfInstances = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                for (int j = 0; j < Children[i].Children.Count; j++)
                {
                    noOfInstances = noOfInstances + Children[i].Children[j].Children.Count;
                }
            }
            colle.Add(noOfInstances.ToString());
            str.Values = colle;
            System.UInt32 length = 0;
            foreach (String data in str.Values)
            {
                length += (System.UInt32)data.Length;
            }
            sourceAttribute.DicomValue = str;
            sourceAttribute.Length = length + (System.UInt32)str.Values.Count - 1;
            _dataset.Add(sourceAttribute);
        }

        void HandleStudyRelatedSeries(TagType tagType, AttributeSet sourceDataset)
        {
            DvtkData.Dimse.Attribute sourceAttribute = sourceDataset.GetAttribute(tagType.Tag);
            if (sourceAttribute == null)
            {
                sourceAttribute = new DvtkData.Dimse.Attribute();
                sourceAttribute.Tag = Tag.NUMBER_OF_STUDY_RELATED_SERIES;
                sourceAttribute.Name = "Number of Study related Series";
            }
            DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
            if (destinationAttribute != null)
            {
                _dataset.Remove(destinationAttribute);
            }
            IntegerString str = new IntegerString();
            StringCollection colle = new StringCollection();
            colle.Add(Children.Count.ToString());
            str.Values = colle;
            System.UInt32 length = 0;
            foreach (String data in str.Values)
            {
                length += (System.UInt32)data.Length;
            }
            sourceAttribute.DicomValue = str;
            sourceAttribute.Length = length + (System.UInt32)str.Values.Count - 1;
            _dataset.Add(sourceAttribute);
        }

        void HandleStudyRelatedInstances(TagType tagType, AttributeSet sourceDataset)
        {
            DvtkData.Dimse.Attribute sourceAttribute = sourceDataset.GetAttribute(tagType.Tag);
            if (sourceAttribute == null)
            {
                sourceAttribute = new DvtkData.Dimse.Attribute();
                sourceAttribute.Tag = Tag.NUMBER_OF_STUDY_RELATED_INSTANCES;
                sourceAttribute.Name = "Number of Study related instances";
                
            }
            DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
            if (destinationAttribute != null)
            {
                _dataset.Remove(destinationAttribute);
            }
            IntegerString str = new IntegerString();
            StringCollection colle = new StringCollection();
            int noOfInstances = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                noOfInstances = noOfInstances + Children[i].Children.Count;
            }
            colle.Add(noOfInstances.ToString());
            str.Values = colle;
            System.UInt32 length = 0;
            foreach (String data in str.Values)
            {
                length += (System.UInt32)data.Length;
            }
            sourceAttribute.DicomValue = str;
            sourceAttribute.Length = length + (System.UInt32)str.Values.Count - 1;
            _dataset.Add(sourceAttribute);
        }

        void HandleSeriesRelatedInstances(TagType tagType, AttributeSet sourceDataset)
        {
            DvtkData.Dimse.Attribute sourceAttribute = sourceDataset.GetAttribute(tagType.Tag);
            if (sourceAttribute == null)
            {
                sourceAttribute = new DvtkData.Dimse.Attribute();
                sourceAttribute.Tag = Tag.NUMBER_OF_SERIES_RELATED_INSTANCES;
                sourceAttribute.Name = "Number of series related instances";
                
            }
            DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
            if (destinationAttribute != null)
            {
                _dataset.Remove(destinationAttribute);
            }
            IntegerString str = new IntegerString();
            StringCollection colle = new StringCollection();
            colle.Add(Children.Count.ToString());
            str.Values = colle;
            System.UInt32 length = 0;
            foreach (String data in str.Values)
            {
                length += (System.UInt32)data.Length;
            }
            sourceAttribute.DicomValue = str;
            sourceAttribute.Length = length + (System.UInt32)str.Values.Count - 1;
            _dataset.Add(sourceAttribute);
        }

		/// <summary>
		/// Copy the attribute with a Unique tag from the local Dataset into the given destination Dataset.
		/// </summary>
		/// <param name="destinationDataset">Dataset being populated with the Unique tag attribute.</param>
		public void CopyUniqueTagTo(AttributeSet destinationDataset)
		{
			CopyTo(_tagTypeList, destinationDataset, true);
		}

		/// <summary>
		/// Copy from the local Dataset into the given destination Dataset as defined by the
		/// default Tag Type list.
		/// </summary>
		/// <param name="destinationDataset">Dataset being populated by the default Tag Type list.</param>
		public void CopyTo(AttributeSet destinationDataset)
		{
			CopyTo(_tagTypeList, destinationDataset, false);
		}

		/// <summary>
		/// Copy from the local Dataset into the given destination Dataset as defined by the
		/// given Tag Type list.
		/// </summary>
		/// <param name="tagTypeList">Tag Type list used to define copy.</param>
		/// <param name="destinationDataset">Dataset being populated by the given Tag Type list.</param>
		public void CopyTo(TagTypeList tagTypeList, AttributeSet destinationDataset)
		{
			CopyTo(tagTypeList, destinationDataset, false);
		}

		/// <summary>
		/// Copy from the local Dataset into the given destination Dataset as defined by the
		/// given Tag Type list. If the copyUniqueTagOnly parameter is true - only copy the Unique Tag attribute.
		/// </summary>
		/// <param name="tagTypeList">Tag Type list used to define copy.</param>
		/// <param name="destinationDataset">Dataset being populated by the given Tag Type list.</param>
		/// <param name="copyUniqueTagOnly">Boolean indicator to define use of Unique Tag.</param>
		private void CopyTo(TagTypeList tagTypeList, AttributeSet destinationDataset, bool copyUniqueTagOnly)
		{
			if (tagTypeList != null)
			{
				foreach (TagType tagType in tagTypeList)
				{
					// check if we should only copy the unique TagType
					if (copyUniqueTagOnly)
					{
						if (tagType.Type != TagTypeEnum.TagUnique) continue;
					}

					DvtkData.Dimse.Attribute destinationAttribute = _dataset.GetAttribute(tagType.Tag);
					if (destinationAttribute != null)
					{
						destinationDataset.Add(destinationAttribute);
					}
				}
			}
		}

		/// <summary>
		/// Copy the defined Additional Attributes from the local additional attributes to the given
		/// dataset.
		/// </summary>
		/// <param name="destinationDataset">Destinaion dataset for loacl additional attributes.</param>
		public void CopyAdditionalAttributes(AttributeSet destinationDataset)
		{
			// try adding all additional attributes
			foreach (DvtkData.Dimse.Attribute additionalAttribute in _additionalDatasetOverWrite)
			{
				// check if the attribute should first be removed from the dataset
				DvtkData.Dimse.Attribute lAdditionalAttribute = destinationDataset.GetAttribute(additionalAttribute.Tag);
				if (lAdditionalAttribute != null)
				{
					destinationDataset.Remove(lAdditionalAttribute);
				}

				// add to the dataset
				destinationDataset.Add(additionalAttribute);
			}

			// try adding all additional attributes
			foreach (DvtkData.Dimse.Attribute additionalAttribute in _additionalDatasetNoOverWrite)
			{
				// only add if not already in the dataset
				if (destinationDataset.GetAttribute(additionalAttribute.Tag) == null)
				{
					destinationDataset.Add(additionalAttribute);
				}
			}
		}

		/// <summary>
		/// Check if a Universal Match is possible on the local dataset using the Tag Type list given.
		/// </summary>
		/// <param name="tagTypeList">Tag type list used for Universal Match.</param>
		/// <returns>Boolean indicating if a Universal Match is true or false.</returns>
		public bool UniversalMatch(TagTypeList tagTypeList)
		{
			bool UniversalMatch = true;
			if (tagTypeList.Count != 0)
			{
				foreach(TagType tagType in tagTypeList)
				{
					DvtkData.Dimse.Attribute thisAttribute = _dataset.GetAttribute(tagType.Tag);
					if (thisAttribute != null)
					{
						if (thisAttribute.Length != 0)
						{
							if (thisAttribute.ValueRepresentation == VR.SQ)
							{
								SequenceOfItems sequenceOfItems = (SequenceOfItems)thisAttribute.DicomValue;
								if (sequenceOfItems.Sequence.Count != 0)
								{
									UniversalMatch = false;
								}
							}
							else
							{
								UniversalMatch = false;
							}
							break;
						}
						else if (IsTagTypeIn(tagType) == false)
						{
							UniversalMatch = false;
							break;
						}
					}					
				}
			}

			return UniversalMatch;
		}

		/// <summary>
		/// Check if the given Tag Type is in the local Tag Type list.
		/// </summary>
		/// <param name="tagType">Tag Type.</param>
		/// <returns>Boolean indicating if the Tag Type is in the local Tag Type list - true or false.</returns>
		private bool IsTagTypeIn(TagType tagType)
		{
			bool isTagTypeIn = false;
			foreach(TagType lTagType in _tagTypeList)
			{
				if (lTagType.Tag == tagType.Tag)
				{
					isTagTypeIn = true;
					break;
				}
			}

			return isTagTypeIn;
		}

		/// <summary>
		/// Check if the Unique Tag as defined in the local Tag Type list is present in the given match dataset.
		/// </summary>
		/// <param name="matchDataset">Dataset to check for match.</param>
		/// <returns>Boolean indicating if the match dataset contains the default Unique Tag.</returns>
		public bool IsUniqueTagFoundIn(AttributeSet matchDataset)
		{
			return IsFoundIn(_tagTypeList, matchDataset, true);
		}

		/// <summary>
		/// Check if the given match dataset is found in the local dataset using the default Tag Type list. 
		/// A check is made to see if all the attributes in the given match dataset are present in the local
		/// dataset.
		/// </summary>
		/// <param name="matchDataset">Match dataset to check.</param>
		/// <returns>Boolean indicating if the match attributes are present in the local dataset.</returns>
		public virtual bool IsFoundIn(AttributeSet matchDataset)
		{
			return IsFoundIn(_tagTypeList, matchDataset, false);
		}

		/// <summary>
		/// Check if the given match dataset is found in the local dataset using the given Tag Type list. 
		/// A check is made to see if all the attributes in the given match dataset are present in the local
		/// dataset.
		/// </summary>
		/// <param name="tagTypeList">Match Tag Type list.</param>
		/// <param name="matchDataset">Match dataset to check.</param>
		/// <returns>Boolean indicating if the match attributes are present in the local dataset.</returns>
		public bool IsFoundIn(TagTypeList tagTypeList, AttributeSet matchDataset)
		{
			return IsFoundIn(tagTypeList, matchDataset, false);
		}

        private bool IsTagFoundInParent(BaseInformationEntity infoEntity,TagType tag)
        {
            if (infoEntity.Parent == null)
                return false;
            if(IsTagFoundInParent(infoEntity.Parent,tag))
            {
                return true;
            }
            if(TagTypeList==null)
                return false;

            foreach (TagType actTag in infoEntity.Parent.TagTypeList)
            {
                if (actTag.Tag.ElementNumber == tag.Tag.ElementNumber &&
                    actTag.Tag.GroupNumber == tag.Tag.GroupNumber
                    )
                {
                    return true;
                }

            }
            return false;
        }
        private bool IsTagFoundInChildren(BaseInformationEntity infoEntity, TagType tag)
        {
            if(infoEntity.Children ==null||infoEntity.Children.Count==0)
            {
                return false;
            }
            for (int i = 0; i < infoEntity.Children.Count; i++)
            {
                if (IsTagFoundInChildren(infoEntity.Children[i], tag))
                    return true;
                foreach (TagType actTag in infoEntity.Children[i].TagTypeList)
                {
                    if (actTag.Tag.ElementNumber == tag.Tag.ElementNumber &&
                        actTag.Tag.GroupNumber == tag.Tag.GroupNumber
                        )
                    {
                        return true;
                    }

                }
            }
                return false;
        }
		/// <summary>
		/// Check if the given match dataset is found in the local dataset using the given Tag Type list. 
		/// A check is made to see if all the attributes in the given match dataset are present in the local
		/// dataset. if the given matchOnUniqueTagOnly parameter is true only the Unique Tags will be checked.
		/// </summary>
		/// <param name="tagTypeList">Match Tag Type list.</param>
		/// <param name="matchDataset">Match dataset to check.</param>
		/// <param name="matchOnUniqueTagOnly">Boolean indicating if only the Unique Tag should be checked.</param>
		/// <returns>Boolean indicating if the match attributes are present in the local dataset.</returns>
		private bool IsFoundIn(TagTypeList tagTypeList, AttributeSet matchDataset, bool matchOnUniqueTagOnly)
		{
			bool isFound = true;
			bool uniqueTagFound = false;

			if (tagTypeList != null)
			{
				foreach (TagType tagType in tagTypeList)
				{
                    if (this is ImagingServiceRequestInformationEntity ||
                        this is PatientInformationEntity ||
                        this is RequestedProcedureInformationEntity ||
                        this is ScheduledProcedureStepInformationEntity ||
                        this is ScheduledProcedureStepInformationEntity ||
                        this is VisitInformationEntity)
                    {
                        if (IsTagFoundInParent(this, tagType) || IsTagFoundInChildren(this,tagType))
                            continue;
                    }


					// check if we should try to match on the unique TagType only
					if (matchOnUniqueTagOnly)
					{
						if (tagType.Type != TagTypeEnum.TagUnique) continue;
					}
					DvtkData.Dimse.Attribute thisAttribute = _dataset.GetAttribute(tagType.Tag);
					DvtkData.Dimse.Attribute matchAttribute = matchDataset.GetAttribute(tagType.Tag);

					if ((thisAttribute != null) &&
						(matchAttribute == null))
					{
						isFound = false;
					}
					else if ((thisAttribute == null) &&
						(matchAttribute != null))
					{
						if (matchAttribute.ValueRepresentation == VR.SQ)
						{
							SequenceOfItems sequenceOfItems = (SequenceOfItems)matchAttribute.DicomValue;
							if (sequenceOfItems.Sequence.Count != 0)
							{
								isFound = false;
							}
						}
						else
						{
                            if (tagType.Tag.ElementNumber == 0x0005 && tagType.Tag.GroupNumber == 0x0008)
                            {
                                isFound = true;
                            }

                            else
                                isFound = false;
						}
					}
					else if ((thisAttribute != null) &&
						(matchAttribute != null))
					{
						// set the unique tag used flag - if we get this far in the code when matching the
						// unique tag only then we must have found it
						if (matchOnUniqueTagOnly)
						{
							uniqueTagFound = true;
						}

						if (thisAttribute.ValueRepresentation != matchAttribute.ValueRepresentation)
						{
							isFound = false;
						}
						else
						{
							if (thisAttribute.ValueRepresentation == VR.SQ)
							{
                                isFound= MatchSequence(thisAttribute, matchAttribute);
							}
							else if ((thisAttribute.Length == 0) &&
								(matchAttribute.Length == 0))
							{
								// found
							}
							else if (thisAttribute.Length == 0)
							{
								// not found
								isFound = false;
							}
							else if (matchAttribute.Length == 0)
							{
								// not found
								isFound = false;
							}
							else
							{
								switch(thisAttribute.ValueRepresentation)
								{
									case VR.AE:
									{
										ApplicationEntity thisApplicationEntity = (ApplicationEntity)thisAttribute.DicomValue;
										ApplicationEntity matchApplicationEntity = (ApplicationEntity)matchAttribute.DicomValue;
										if (thisApplicationEntity.Values.Count != matchApplicationEntity.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisApplicationEntity.Values.Count; i++)
											{
                                                if (!WildCardMatchString(matchApplicationEntity.Values[i], thisApplicationEntity.Values[i], IsCaseSensitiveAE))
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.AS:
									{
										AgeString thisAgeString = (AgeString)thisAttribute.DicomValue;
										AgeString matchAgeString = (AgeString)matchAttribute.DicomValue;
										if (thisAgeString.Values.Count != matchAgeString.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisAgeString.Values.Count; i++)
											{
												if (!MatchString(matchAgeString.Values[i], thisAgeString.Values[i]))
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.AT:
									{
										AttributeTag thisAttributeTag = (AttributeTag)thisAttribute.DicomValue;
										AttributeTag matchAttributeTag = (AttributeTag)matchAttribute.DicomValue;
										if (thisAttributeTag.Values.Count != matchAttributeTag.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisAttributeTag.Values.Count; i++)
											{
												if (matchAttributeTag.Values[i] != thisAttributeTag.Values[i])
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.CS:
									{
										CodeString thisCodeString = (CodeString)thisAttribute.DicomValue;
										CodeString matchCodeString = (CodeString)matchAttribute.DicomValue;
                                        //if (thisCodeString.Values.Count != matchCodeString.Values.Count)
                                        //{
                                        //    isFound = false;
                                        //}
                                        //else
                                        //{
                                        //    for (int i = 0; i < thisCodeString.Values.Count; i++)
                                        //    {
                                        //        if (!WildCardMatchString(matchCodeString.Values[i], thisCodeString.Values[i], IsCaseSensitiveCS))
                                        //        {
                                        //            isFound = false;
                                        //            break;
                                        //        }
                                        //    }
                                        //}
                                        isFound = false;
                                        for (int i = 0; i < thisCodeString.Values.Count; i++)
                                        {
                                            for (int j = 0; j < matchCodeString.Values.Count; j++)
                                            {
                                                if (WildCardMatchString(matchCodeString.Values[j], thisCodeString.Values[i], IsCaseSensitiveCS))
                                                {
                                                    isFound = true;
                                                    break;
                                                }
                                            }
                                        }

										break;
									}
									case VR.DA:
									{
										Date thisDate = (Date)thisAttribute.DicomValue;
										Date matchDate = (Date)matchAttribute.DicomValue;
										if (thisDate.Values.Count != matchDate.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisDate.Values.Count; i++)
											{
												System.String thisDateString = thisDate.Values[i].Trim();
												System.String matchDateString = matchDate.Values[i].Trim();
												switch (matchDateString.Length)
												{
												case 9:
													// ToDate = -YYYYMMDD
													// FromDate = YYYYMMDD-
													if (matchDateString.StartsWith("-"))
													{
														System.String date = matchDateString.Substring(1,8);
														int comparison = thisDateString.CompareTo(date);
														if (comparison > 0)
														{
															isFound = false;
														}
													}
													else if (matchDateString.EndsWith("-"))
													{
														System.String date = matchDateString.Substring(0,8);
														int comparison = thisDateString.CompareTo(date);
														if (comparison < 0)
														{
															isFound = false;
														}
													}
													break;
												case 17:
													// DateRange = YYYYMMDD-YYYYMMDD
													System.String[] dates = matchDateString.Split('-');
													int comparison1 = thisDateString.CompareTo(dates[0]);
													int comparison2 = thisDateString.CompareTo(dates[1]);
													if ((comparison1 < 0) ||
													    (comparison2 > 0))
													{
														isFound = false;
													}
													break;
												case 8:
													// Date = YYYYMMDD
												default:
													if (!MatchString(matchDateString, thisDateString))
													{
														isFound = false;
													}
													break;
												}

												if (isFound == false)
												{
													break;
												}
											}
										}
										break;
									}
									case VR.DS:
									{
										DecimalString thisDecimalString = (DecimalString)thisAttribute.DicomValue;
										DecimalString matchDecimalString = (DecimalString)matchAttribute.DicomValue;
										if (thisDecimalString.Values.Count != matchDecimalString.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisDecimalString.Values.Count; i++)
											{
												if (!MatchString(matchDecimalString.Values[i], thisDecimalString.Values[i]))
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.DT:
									{
										DvtkData.Dimse.DateTime thisDateTime = (DvtkData.Dimse.DateTime)thisAttribute.DicomValue;
										DvtkData.Dimse.DateTime matchDateTime = (DvtkData.Dimse.DateTime)matchAttribute.DicomValue;
										if (thisDateTime.Values.Count != matchDateTime.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisDateTime.Values.Count; i++)
											{
												if (!MatchString(matchDateTime.Values[i], thisDateTime.Values[i]))
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.FD:
									{
										FloatingPointDouble thisFloatingPointDouble = (FloatingPointDouble)thisAttribute.DicomValue;
										FloatingPointDouble matchFloatingPointDouble = (FloatingPointDouble)matchAttribute.DicomValue;
										if (thisFloatingPointDouble.Values.Count != matchFloatingPointDouble.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisFloatingPointDouble.Values.Count; i++)
											{
												if (matchFloatingPointDouble.Values[i] != thisFloatingPointDouble.Values[i])
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.FL:
									{
										FloatingPointSingle thisFloatingPointSingle = (FloatingPointSingle)thisAttribute.DicomValue;
										FloatingPointSingle matchFloatingPointSingle = (FloatingPointSingle)matchAttribute.DicomValue;
										if (thisFloatingPointSingle.Values.Count != matchFloatingPointSingle.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisFloatingPointSingle.Values.Count; i++)
											{
												if (matchFloatingPointSingle.Values[i] != thisFloatingPointSingle.Values[i])
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.IS:
									{
										IntegerString thisIntegerString = (IntegerString)thisAttribute.DicomValue;
										IntegerString matchIntegerString = (IntegerString)matchAttribute.DicomValue;
										if (thisIntegerString.Values.Count != matchIntegerString.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisIntegerString.Values.Count; i++)
											{
												if (!MatchString(matchIntegerString.Values[i], thisIntegerString.Values[i]))
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.LO:
									{
										LongString thisLongString = (LongString)thisAttribute.DicomValue;
										LongString matchLongString = (LongString)matchAttribute.DicomValue;
										if (thisLongString.Values.Count != matchLongString.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisLongString.Values.Count; i++)
											{
                                                if (!WildCardMatchString(matchLongString.Values[i], thisLongString.Values[i], IsCaseSensitiveLO))
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.LT:
									{
										break;
									}
									case VR.OB:
									{
										break;
									}
									case VR.OF:
									{
										break;
									}
									case VR.OW:
									{
										break;
                                    }
                                    case VR.OV:
                                    {
                                        break;
                                    }
									case VR.PN:
									{
										PersonName thisPersonName = (PersonName)thisAttribute.DicomValue;
										PersonName matchPersonName = (PersonName)matchAttribute.DicomValue;
										if (thisPersonName.Values.Count != matchPersonName.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisPersonName.Values.Count; i++)
											{
                                                if (!WildCardMatchString(matchPersonName.Values[i], thisPersonName.Values[i], IsCaseSensitivePN))
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.SH:
									{
										ShortString thisShortString = (ShortString)thisAttribute.DicomValue;
										ShortString matchShortString = (ShortString)matchAttribute.DicomValue;
										if (thisShortString.Values.Count != matchShortString.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisShortString.Values.Count; i++)
											{
                                                if (!WildCardMatchString(matchShortString.Values[i], thisShortString.Values[i], IsCaseSensitiveSH))
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.SL:
									{
										SignedLong thisSignedLong = (SignedLong)thisAttribute.DicomValue;
										SignedLong matchSignedLong = (SignedLong)matchAttribute.DicomValue;
										if (thisSignedLong.Values.Count != matchSignedLong.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisSignedLong.Values.Count; i++)
											{
												if (matchSignedLong.Values[i] != thisSignedLong.Values[i])
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.SQ:
									{
//										SequenceOfItems sequenceOfItems = (SequenceOfItems)attribute.DicomValue;
//										foreach (SequenceItem item in sequenceOfItems.Sequence)
//										{
//										}
										break;
									}
									case VR.SS:
									{
										SignedShort thisSignedShort = (SignedShort)thisAttribute.DicomValue;
										SignedShort matchSignedShort = (SignedShort)matchAttribute.DicomValue;
										if (thisSignedShort.Values.Count != matchSignedShort.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisSignedShort.Values.Count; i++)
											{
												if (matchSignedShort.Values[i] != thisSignedShort.Values[i])
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.ST:
									{
										break;
                                    }
                                    case VR.SV:
                                    {
                                        SignedVeryLongString thisSignedVeryLongString = (SignedVeryLongString)thisAttribute.DicomValue;
                                        SignedVeryLongString matchSignedVeryLongString = (SignedVeryLongString)matchAttribute.DicomValue;
                                        if (thisSignedVeryLongString.Values.Count != matchSignedVeryLongString.Values.Count)
                                        {
                                            isFound = false;
                                        }
                                        else
                                        {
                                            for (int i = 0; i < thisSignedVeryLongString.Values.Count; i++)
                                            {
                                                if (matchSignedVeryLongString.Values[i] != thisSignedVeryLongString.Values[i])
                                                {
                                                    isFound = false;
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    case VR.TM:
                                    {
                                        isFound = false;

            							Time thisTime = (Time)thisAttribute.DicomValue;
        								Time matchTime = (Time)matchAttribute.DicomValue;

                                        for (int i = 0; i < thisTime.Values.Count; i++)
                                        {
                                            if (TMMatching.Matches(thisTime.Values[i], matchTime.Values[0]))
                                            {
                                                isFound = true;
                                                break;
                                            }
                                        }

                                        break;
                                    }
									case VR.UI:
									{
										UniqueIdentifier thisUniqueIdentifier = (UniqueIdentifier)thisAttribute.DicomValue;
										UniqueIdentifier matchUniqueIdentifier = (UniqueIdentifier)matchAttribute.DicomValue;

										// check for list of UID matching
										if ((thisUniqueIdentifier.Values.Count == 1) && 
											(matchUniqueIdentifier.Values.Count > 1))
										{
											isFound = false;

											// iterate over all the possible matches
											for (int i = 0; i < matchUniqueIdentifier.Values.Count; i++)
											{
												if (MatchString(matchUniqueIdentifier.Values[i], thisUniqueIdentifier.Values[0]))
												{
													isFound = true;
													break;
												}
											}
										}
										else if (thisUniqueIdentifier.Values.Count == matchUniqueIdentifier.Values.Count)
										{
											for (int i = 0; i < thisUniqueIdentifier.Values.Count; i++)
											{
												if (!MatchString(matchUniqueIdentifier.Values[i], thisUniqueIdentifier.Values[i]))
												{
													isFound = false;
													break;
												}
											}
										}
										else
										{
											isFound = false;
										}
										break;
									}
									case VR.UL:
									{
										UnsignedLong thisUnsignedLong = (UnsignedLong)thisAttribute.DicomValue;
										UnsignedLong matchUnsignedLong = (UnsignedLong)matchAttribute.DicomValue;
										if (thisUnsignedLong.Values.Count != matchUnsignedLong.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisUnsignedLong.Values.Count; i++)
											{
												if (matchUnsignedLong.Values[i] != thisUnsignedLong.Values[i])
												{
													isFound = false;
													break;
												}
											}
										}
										break;
									}
									case VR.UN:
									{
										break;
									}
									case VR.US:
									{
										UnsignedShort thisUnsignedShort = (UnsignedShort)thisAttribute.DicomValue;
										UnsignedShort matchUnsignedShort = (UnsignedShort)matchAttribute.DicomValue;
										if (thisUnsignedShort.Values.Count != matchUnsignedShort.Values.Count)
										{
											isFound = false;
										}
										else
										{
											for (int i = 0; i < thisUnsignedShort.Values.Count; i++)
											{
												if (matchUnsignedShort.Values[i] != thisUnsignedShort.Values[i])
												{
													isFound = false;
													break;
												}
											}
										}
										break;
                                    }
                                    case VR.UV:
                                    {
                                        UnsignedVeryLongString thisUnsignedVeryLongString = (UnsignedVeryLongString)thisAttribute.DicomValue;
                                        UnsignedVeryLongString matchUnsignedVeryLongString = (UnsignedVeryLongString)matchAttribute.DicomValue;
                                        if (thisUnsignedVeryLongString.Values.Count != matchUnsignedVeryLongString.Values.Count)
                                        {
                                            isFound = false;
                                        }
                                        else
                                        {
                                            for (int i = 0; i < thisUnsignedVeryLongString.Values.Count; i++)
                                            {
                                                if (matchUnsignedVeryLongString.Values[i] != thisUnsignedVeryLongString.Values[i])
                                                {
                                                    isFound = false;
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
									case VR.UT:
									{
										break;
									}
                                    case VR.UR:
                                    {
                                        break;
                                    }
                                    case VR.UC:
                                    {
                                        break;
                                    }

									default:
										isFound = false;
										break;
								}
							}
						}
						if (isFound == false)
						{
							break;
						}
					}
				}
			}

			// check for special case where we should match on the Unique Tag only - but it was not found
			if ((matchOnUniqueTagOnly == true) &&
				(uniqueTagFound == false))
			{
				// no match
				isFound = false;
			}

			return isFound;
		}

        bool MatchSequence(DvtkData.Dimse.Attribute thisAttribute,DvtkData.Dimse.Attribute matchAttribute)
        {
            SequenceOfItems thisAttrib = ((SequenceOfItems)thisAttribute.DicomValue);
            SequenceOfItems matchAttrib = ((SequenceOfItems)matchAttribute.DicomValue);
            bool isMatchFound=true;
            foreach (SequenceItem items in matchAttrib.Sequence)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ValueRepresentation == VR.SQ)
                    {
 
                    }
                    else if ((items[i].Length != 0) &&
                                (items[i].Tag.ElementNumber != 0x0000))
                    {

                        foreach (SequenceItem item in thisAttrib.Sequence)
                        {
                            for (int j = 0; j < item.Count; j++)
                            {
                                if (item[j].Tag == items[i].Tag)
                                {
                                  isMatchFound=  CompareSeqence(item[j].ValueRepresentation, item[j].DicomValue, items[i].DicomValue);
                                }
                            }
                        }

                    }
                }
            }
            //for (int i = 0; i < items.Count; i++)
            //{
            //    DvtkData.Dimse.Attribute attrib = items[i];
            //    if (attrib.ValueRepresentation == VR.SQ)
            //    {
            //        foreach (SequenceItem s in ((SequenceOfItems)attrib.DicomValue).Sequence)
            //        {
            //            if (IsSequenceHavingValue(s))
            //                return true;
            //        }
            //    }
            //    else if ((attrib.Length != 0) && (attrib.Tag.ElementNumber != 0x0000))
            //    {
            //        return true;
            //    }
            //}
            return isMatchFound;
        }


        bool CompareSeqence(VR vr,DicomValueType thisAttribValue,DicomValueType matchAttribValue )
        {
            bool isFound = true;
            switch (vr)
            {
                case VR.AE:
                    {
                        ApplicationEntity thisApplicationEntity = (ApplicationEntity)thisAttribValue;
                        ApplicationEntity matchApplicationEntity = (ApplicationEntity)matchAttribValue;
                        if (thisApplicationEntity.Values.Count != matchApplicationEntity.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisApplicationEntity.Values.Count; i++)
                            {
                                if (!WildCardMatchString(matchApplicationEntity.Values[i], thisApplicationEntity.Values[i], IsCaseSensitiveAE))
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.AS:
                    {
                        AgeString thisAgeString = (AgeString)thisAttribValue;
                        AgeString matchAgeString = (AgeString)matchAttribValue;
                        if (thisAgeString.Values.Count != matchAgeString.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisAgeString.Values.Count; i++)
                            {
                                if (!MatchString(matchAgeString.Values[i], thisAgeString.Values[i]))
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.AT:
                    {
                        AttributeTag thisAttributeTag = (AttributeTag)thisAttribValue;
                        AttributeTag matchAttributeTag = (AttributeTag)matchAttribValue;
                        if (thisAttributeTag.Values.Count != matchAttributeTag.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisAttributeTag.Values.Count; i++)
                            {
                                if (matchAttributeTag.Values[i] != thisAttributeTag.Values[i])
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.CS:
                    {
                        CodeString thisCodeString = (CodeString)thisAttribValue;
                        CodeString matchCodeString = (CodeString)matchAttribValue;
                        //if (thisCodeString.Values.Count != matchCodeString.Values.Count)
                        //{
                        //    isFound = false;
                        //}
                        //else
                        //{
                        //    for (int i = 0; i < thisCodeString.Values.Count; i++)
                        //    {
                        //        if (!WildCardMatchString(matchCodeString.Values[i], thisCodeString.Values[i], IsCaseSensitiveCS))
                        //        {
                        //            isFound = false;
                        //            break;
                        //        }
                        //    }
                        //}
                        isFound = false;
                        for (int i = 0; i < thisCodeString.Values.Count; i++)
                        {
                            for (int j = 0; j < matchCodeString.Values.Count; j++)
                            {
                                if (WildCardMatchString(matchCodeString.Values[j], thisCodeString.Values[i], IsCaseSensitiveCS))
                                {
                                    isFound = true;
                                    break;
                                }
                            }
                        }

                        break;
                    }
                case VR.DA:
                    {
                        Date thisDate = (Date)thisAttribValue;
                        Date matchDate = (Date)matchAttribValue;
                        if (thisDate.Values.Count != matchDate.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisDate.Values.Count; i++)
                            {
                                System.String thisDateString = thisDate.Values[i].Trim();
                                System.String matchDateString = matchDate.Values[i].Trim();
                                switch (matchDateString.Length)
                                {
                                    case 9:
                                        // ToDate = -YYYYMMDD
                                        // FromDate = YYYYMMDD-
                                        if (matchDateString.StartsWith("-"))
                                        {
                                            System.String date = matchDateString.Substring(1, 8);
                                            int comparison = thisDateString.CompareTo(date);
                                            if (comparison > 0)
                                            {
                                                isFound = false;
                                            }
                                        }
                                        else if (matchDateString.EndsWith("-"))
                                        {
                                            System.String date = matchDateString.Substring(0, 8);
                                            int comparison = thisDateString.CompareTo(date);
                                            if (comparison < 0)
                                            {
                                                isFound = false;
                                            }
                                        }
                                        break;
                                    case 17:
                                        // DateRange = YYYYMMDD-YYYYMMDD
                                        System.String[] dates = matchDateString.Split('-');
                                        int comparison1 = thisDateString.CompareTo(dates[0]);
                                        int comparison2 = thisDateString.CompareTo(dates[1]);
                                        if ((comparison1 < 0) ||
                                            (comparison2 > 0))
                                        {
                                            isFound = false;
                                        }
                                        break;
                                    case 8:
                                    // Date = YYYYMMDD
                                    default:
                                        if (!MatchString(matchDateString, thisDateString))
                                        {
                                            isFound = false;
                                        }
                                        break;
                                }

                                if (isFound == false)
                                {
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.DS:
                    {
                        DecimalString thisDecimalString = (DecimalString)thisAttribValue;
                        DecimalString matchDecimalString = (DecimalString)matchAttribValue;
                        if (thisDecimalString.Values.Count != matchDecimalString.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisDecimalString.Values.Count; i++)
                            {
                                if (!MatchString(matchDecimalString.Values[i], thisDecimalString.Values[i]))
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.DT:
                    {
                        DvtkData.Dimse.DateTime thisDateTime = (DvtkData.Dimse.DateTime)thisAttribValue;
                        DvtkData.Dimse.DateTime matchDateTime = (DvtkData.Dimse.DateTime)matchAttribValue;
                        if (thisDateTime.Values.Count != matchDateTime.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisDateTime.Values.Count; i++)
                            {
                                if (!MatchString(matchDateTime.Values[i], thisDateTime.Values[i]))
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.FD:
                    {
                        FloatingPointDouble thisFloatingPointDouble = (FloatingPointDouble)thisAttribValue;
                        FloatingPointDouble matchFloatingPointDouble = (FloatingPointDouble)matchAttribValue;
                        if (thisFloatingPointDouble.Values.Count != matchFloatingPointDouble.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisFloatingPointDouble.Values.Count; i++)
                            {
                                if (matchFloatingPointDouble.Values[i] != thisFloatingPointDouble.Values[i])
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.FL:
                    {
                        FloatingPointSingle thisFloatingPointSingle = (FloatingPointSingle)thisAttribValue;
                        FloatingPointSingle matchFloatingPointSingle = (FloatingPointSingle)matchAttribValue;
                        if (thisFloatingPointSingle.Values.Count != matchFloatingPointSingle.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisFloatingPointSingle.Values.Count; i++)
                            {
                                if (matchFloatingPointSingle.Values[i] != thisFloatingPointSingle.Values[i])
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.IS:
                    {
                        IntegerString thisIntegerString = (IntegerString)thisAttribValue;
                        IntegerString matchIntegerString = (IntegerString)matchAttribValue;
                        if (thisIntegerString.Values.Count != matchIntegerString.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisIntegerString.Values.Count; i++)
                            {
                                if (!MatchString(matchIntegerString.Values[i], thisIntegerString.Values[i]))
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.LO:
                    {
                        LongString thisLongString = (LongString)thisAttribValue;
                        LongString matchLongString = (LongString)matchAttribValue;
                        if (thisLongString.Values.Count != matchLongString.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisLongString.Values.Count; i++)
                            {
                                if (!WildCardMatchString(matchLongString.Values[i], thisLongString.Values[i], IsCaseSensitiveLO))
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.LT:
                    {
                        break;
                    }
                case VR.OB:
                    {
                        break;
                    }
                case VR.OF:
                    {
                        break;
                    }
                case VR.OW:
                    {
                        break;
                    }
                case VR.OV:
                    {
                        break;
                    }
                case VR.PN:
                    {
                        PersonName thisPersonName = (PersonName)thisAttribValue;
                        PersonName matchPersonName = (PersonName)matchAttribValue;
                        if (thisPersonName.Values.Count != matchPersonName.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisPersonName.Values.Count; i++)
                            {
                                if (!WildCardMatchString(matchPersonName.Values[i], thisPersonName.Values[i], IsCaseSensitivePN))
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.SH:
                    {
                        ShortString thisShortString = (ShortString)thisAttribValue;
                        ShortString matchShortString = (ShortString)matchAttribValue;
                        if (thisShortString.Values.Count != matchShortString.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisShortString.Values.Count; i++)
                            {
                                if (!WildCardMatchString(matchShortString.Values[i], thisShortString.Values[i], IsCaseSensitiveSH))
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.SL:
                    {
                        SignedLong thisSignedLong = (SignedLong)thisAttribValue;
                        SignedLong matchSignedLong = (SignedLong)matchAttribValue;
                        if (thisSignedLong.Values.Count != matchSignedLong.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisSignedLong.Values.Count; i++)
                            {
                                if (matchSignedLong.Values[i] != thisSignedLong.Values[i])
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.SQ:
                    {
                        //										SequenceOfItems sequenceOfItems = (SequenceOfItems)attribute.DicomValue;
                        //										foreach (SequenceItem item in sequenceOfItems.Sequence)
                        //										{
                        //										}
                        break;
                    }
                case VR.SS:
                    {
                        SignedShort thisSignedShort = (SignedShort)thisAttribValue;
                        SignedShort matchSignedShort = (SignedShort)matchAttribValue;
                        if (thisSignedShort.Values.Count != matchSignedShort.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisSignedShort.Values.Count; i++)
                            {
                                if (matchSignedShort.Values[i] != thisSignedShort.Values[i])
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.ST:
                    {
                        break;
                    }

                case VR.SV:
                    {
                        SignedVeryLongString thisSignedVeryLongString = (SignedVeryLongString)thisAttribValue;
                        SignedVeryLongString matchSignedVeryLongString = (SignedVeryLongString)matchAttribValue;
                        if (thisSignedVeryLongString.Values.Count != matchSignedVeryLongString.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisSignedVeryLongString.Values.Count; i++)
                            {
                                if (matchSignedVeryLongString.Values[i] != thisSignedVeryLongString.Values[i])
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.TM:
                    {
                        isFound = false;

                        Time thisTime = (Time)thisAttribValue;
                        Time matchTime = (Time)matchAttribValue;

                        for (int i = 0; i < thisTime.Values.Count; i++)
                        {
                            if (TMMatching.Matches(thisTime.Values[i], matchTime.Values[0]))
                            {
                                isFound = true;
                                break;
                            }
                        }

                        break;
                    }
                case VR.UI:
                    {
                        UniqueIdentifier thisUniqueIdentifier = (UniqueIdentifier)thisAttribValue;
                        UniqueIdentifier matchUniqueIdentifier = (UniqueIdentifier)matchAttribValue;

                        // check for list of UID matching
                        if ((thisUniqueIdentifier.Values.Count == 1) &&
                            (matchUniqueIdentifier.Values.Count > 1))
                        {
                            isFound = false;

                            // iterate over all the possible matches
                            for (int i = 0; i < matchUniqueIdentifier.Values.Count; i++)
                            {
                                if (MatchString(matchUniqueIdentifier.Values[i], thisUniqueIdentifier.Values[0]))
                                {
                                    isFound = true;
                                    break;
                                }
                            }
                        }
                        else if (thisUniqueIdentifier.Values.Count == matchUniqueIdentifier.Values.Count)
                        {
                            for (int i = 0; i < thisUniqueIdentifier.Values.Count; i++)
                            {
                                if (!MatchString(matchUniqueIdentifier.Values[i], thisUniqueIdentifier.Values[i]))
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            isFound = false;
                        }
                        break;
                    }
                case VR.UL:
                    {
                        UnsignedLong thisUnsignedLong = (UnsignedLong)thisAttribValue;
                        UnsignedLong matchUnsignedLong = (UnsignedLong)matchAttribValue;
                        if (thisUnsignedLong.Values.Count != matchUnsignedLong.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisUnsignedLong.Values.Count; i++)
                            {
                                if (matchUnsignedLong.Values[i] != thisUnsignedLong.Values[i])
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.UN:
                    {
                        break;
                    }
                case VR.US:
                    {
                        UnsignedShort thisUnsignedShort = (UnsignedShort)thisAttribValue;
                        UnsignedShort matchUnsignedShort = (UnsignedShort)matchAttribValue;
                        if (thisUnsignedShort.Values.Count != matchUnsignedShort.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisUnsignedShort.Values.Count; i++)
                            {
                                if (matchUnsignedShort.Values[i] != thisUnsignedShort.Values[i])
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.UT:
                    {
                        break;
                    }

                case VR.UV:
                    {
                        UnsignedVeryLongString thisUnsignedVeryLongString = (UnsignedVeryLongString)thisAttribValue;
                        UnsignedVeryLongString matchUnsignedVeryLongString = (UnsignedVeryLongString)matchAttribValue;
                        if (thisUnsignedVeryLongString.Values.Count != matchUnsignedVeryLongString.Values.Count)
                        {
                            isFound = false;
                        }
                        else
                        {
                            for (int i = 0; i < thisUnsignedVeryLongString.Values.Count; i++)
                            {
                                if (matchUnsignedVeryLongString.Values[i] != thisUnsignedVeryLongString.Values[i])
                                {
                                    isFound = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case VR.UR:
                    {
                        break;
                    }
                case VR.UC:
                    {
                        break;
                    }
                default:
                    isFound = false;
                    break;
            }
            return isFound;		
        }

		/// <summary>
		/// Implement is any sub-classes - define the default Tag Type list.
		/// </summary>
		protected abstract void SetDefaultTagTypeList();

		/// <summary>
		/// Display this Entity to the Console - useful when debugging.
		/// </summary>
		public void ConsoleDisplay()
		{
			Console.Write(Dump(""));
		}

		/// <summary>
		/// Dump this Entity to a String - useful when debugging.
		/// </summary>
		/// <returns>The dump.</returns>
		public String Dump(String prefix)
		{
			String dumpString = "";

			dumpString+= prefix + String.Format("Level: {0}\r\n", _level);
			if (_dataset != null)
			{
				dumpString+= _dataset.Dump(prefix);
			}

			dumpString+= prefix + String.Format("Children: {0}\r\n", _children.Count);
			int child = 1;
			foreach (BaseInformationEntity informationEntity in _children)
			{
				dumpString+= prefix + "   " + String.Format("Child: {0}\r\n", child++);
				dumpString+= informationEntity.Dump(prefix + "   ");
			}

			return(dumpString);
		}
        /// <summary>
        /// Gets the composite children of the object
        /// </summary>
        /// <returns></returns>
        public BaseCompositeInformationEntity GetCompositeDataModel()
        {
            BaseCompositeInformationEntity entity;
            if (_level == "PATIENT")
                entity = new Patient(_dataset);
            else if (_level == "STUDY")
                entity = new Study(_dataset);
            else if (_level == "SERIES")
                entity = new Series(_dataset);
            else if (_level == "IMAGE")
                entity = new Image(_dataset);
            else
                return null;
           
            foreach (BaseInformationEntity informationEntity in _children)
            {
                entity.Children.Add(informationEntity.GetCompositeDataModel());
            }
            return entity;
        }

		/// <summary>
		/// Use the information entity tagTypeList to add default values from the defaultDataset
		/// into this _dataset.
		/// A default value will be added if:
		/// overWriteExistingValue is true and a default value exists.
		/// or:
		/// overWriteExistingValue is false and the default value exists and this _dataset either does
		/// not contain the default tag or the default tag has a zero-length value.
		/// All child Entities are also processed.
		/// </summary>
        /// <param name="overWriteExistingValue"></param>
		/// <param name="defaultDataset"></param>
		public void AddDefaultAttributes(bool overWriteExistingValue, DataSet defaultDataset)
		{
			// use tag list to determine which attributes should take default values
        
			foreach (TagType tagType in _tagTypeList)
			{
				// only add a default value it the attribute is not already in the dataset.
				DvtkData.Dimse.Attribute attribute = _dataset.GetAttribute(tagType.Tag);

				// check if a default value is available for this tag
				DvtkData.Dimse.Attribute defaultAttribute = defaultDataset.GetAttribute(tagType.Tag);

				// Only make changes if a default attribute is found
				if (defaultAttribute != null)
				{
					// overwrite any existing value
					if (overWriteExistingValue == true)
					{
                        if (attribute != null && attribute.ValueRepresentation == VR.SQ&&defaultAttribute.ValueRepresentation==VR.SQ)
                        {
                            DvtkData.Dimse.Attribute actualAttrib = attribute;
                            foreach( SequenceItem sourceItem in ((SequenceOfItems)defaultAttribute.DicomValue).Sequence)
                            {
                                for (int i = 0; i < sourceItem.Count; i++)
                                {
                                    DvtkData.Dimse.Attribute sourceAttrib = sourceItem[i];
                                    foreach (SequenceItem destItem in ((SequenceOfItems)attribute.DicomValue).Sequence)
                                    {
                                        DvtkData.Dimse.Attribute desattrib = destItem.GetAttribute(sourceAttrib.Tag);
                                        if (desattrib != null)
                                        {
                                            destItem.Remove(desattrib);
                                        }
                                        destItem.Add(sourceAttrib);
                                    }
                                }
                            }
                            _dataset.Remove(actualAttrib);
                            _dataset.Add(attribute);
                        }
                        else
                        {
                            if (attribute != null)
                            {
                                // remove the old attribute value
                                _dataset.Remove(attribute);
                            }

                            // add default value to the dataset
                            _dataset.Add(defaultAttribute);
                        }
					}
					else
					{
						// need to remove any zero-length attribute
						if ((attribute != null) &&
							(attribute.Length == 0))
						{
							_dataset.Remove(attribute);

							// add default value to the dataset
							_dataset.Add(defaultAttribute);
						}
						else if (attribute == null)
						{
							// add default value to the dataset
							_dataset.Add(defaultAttribute);
						}
					}
				}
			}

			// include all children
			foreach (BaseInformationEntity informationEntity in _children)
			{
				informationEntity.AddDefaultAttributes(overWriteExistingValue, defaultDataset);
			}
		}

		/// <summary>
		/// Add the additional attributes to this Entity and any children.
		/// </summary>
        /// <param name="overWriteExistingValue"></param>        
		/// <param name="additionalDataset"></param>
		public void AddAdditionalAttributes(bool overWriteExistingValue, DataSet additionalDataset)
		{
			// add the additional attribute
			foreach (DvtkData.Dimse.Attribute additionalAttribute in additionalDataset)
			{
				if (overWriteExistingValue == true)
				{
					_additionalDatasetOverWrite.Add(additionalAttribute);
				}
				else
				{
					_additionalDatasetNoOverWrite.Add(additionalAttribute);
				}
			}

			// include all children
			foreach (BaseInformationEntity informationEntity in _children)
			{
				informationEntity.AddAdditionalAttributes(overWriteExistingValue, additionalDataset);
			}
		}

		/// <summary>
		/// Check if the searchKey matches the candidateValue.
		/// </summary>
		/// <param name="searchKey">Key to match against.</param>
		/// <param name="candidateValue">Value to try to match against the searchKey.</param>
		/// <returns>Bool indicating the result of the match.</returns>
		private bool MatchString(String searchKey, String candidateValue)
		{
			bool matchesSearchKey = false;
			String lSearchKey = searchKey.Trim();
			String lCandidateValue = candidateValue.Trim();

			// Check for simple string equivalence
			if (lSearchKey == lCandidateValue)
			{
				// Strings the same
				matchesSearchKey = true;
			}

			// Return matching result
			return matchesSearchKey;
		}

		/// <summary>
		/// Check if the searchKey matches the candidateValue. A "*" can be present as the last
		/// character of the searchKey to indciate a wildcard match from that point in the string
		/// onwards.
		/// </summary>
		/// <param name="searchKey">Key to match against - last character maybe a "*" wildcard.</param>
		/// <param name="candidateValue">Value to try to match against the searchKey.</param>
        /// <param name="caseSensitive"></param>
		/// <returns>Bool indicating the result of the match.</returns>
		private bool WildCardMatchString(String searchKey, String candidateValue, bool caseSensitive)
		{
			bool matchesSearchKey = false;
			String lSearchKey = searchKey.Trim();
			String lCandidateValue = candidateValue.Trim();

            // Make sure that all regular expression operators (besides '*' and '?'), that may be part of the searchKey, are escaped.
            // '*' and '?' have special meanings in DICOM and are processed differently.

            string regExSearchKey = lSearchKey;

            // First escape the '\' before escaping the other regular expression operators (which will be escaped with a '\').
            regExSearchKey = regExSearchKey.Replace(@"\", @"\\");

            regExSearchKey = regExSearchKey.Replace(@".", @"\.");
            regExSearchKey = regExSearchKey.Replace(@"$", @"\$");
            regExSearchKey = regExSearchKey.Replace(@"^", @"\^");
            regExSearchKey = regExSearchKey.Replace(@"{", @"\{");
            regExSearchKey = regExSearchKey.Replace(@"[", @"\[");
            regExSearchKey = regExSearchKey.Replace(@"(", @"\(");
            regExSearchKey = regExSearchKey.Replace(@"|", @"\|");
            regExSearchKey = regExSearchKey.Replace(@")", @"\)");
            regExSearchKey = regExSearchKey.Replace(@"+", @"\+");
            
			// The Dicom '*' is equivalent to '.*' (any number of characters) in regular expressions.
            regExSearchKey = regExSearchKey.Replace("*", ".*");

			// The Dicom '?' is equivalent to '.' (any character) in regular expressions.
			regExSearchKey = regExSearchKey.Replace("?", ".");

			// Check if the candidate matches the regular expression
			Regex regEx = null;
			
			if (caseSensitive)
			{
				regEx = new Regex(regExSearchKey);
			}
			else
			{
				regEx = new Regex(regExSearchKey, RegexOptions.IgnoreCase);
			}

			matchesSearchKey = regEx.IsMatch(lCandidateValue);
			
			// Return matching result
			return matchesSearchKey;
		}

		/// <summary>
		/// Try to get the Specific Character Set attribute stored in the dataset of the IE.
		/// </summary>
		/// <returns>Specific Character Set attribute if found in IE else null</returns>
		public DvtkData.Dimse.Attribute GetSpecificCharacterSet()
		{
			DvtkData.Dimse.Attribute specificCharacterSetAttribute = null;

			// Check if a value has been stored in the scheduled procedure step item for the Specific Character Set
			DataSet dataset = this.DataSet;
			if (dataset != null)
			{
				specificCharacterSetAttribute = dataset.GetAttribute(Tag.SPECIFIC_CHARACTER_SET);
			}

			return specificCharacterSetAttribute;
		}
        /// <summary>
        /// Enabling the case sensitive query for the information entity. by default the case sensitivity is false for all.
        /// </summary>
        /// <param name="AE"></param>
        /// <param name="CS"></param>
        /// <param name="PN"></param>
        /// <param name="SH"></param>
        /// <param name="LO"></param>
        public static void SetCaseSentive(bool AE, bool CS, bool PN, bool SH, bool LO)
        {
            IsCaseSensitiveAE = AE;
            IsCaseSensitiveCS = CS;
            IsCaseSensitivePN = PN;
            IsCaseSensitiveSH = SH;
            IsCaseSensitiveLO = LO;
        }
        static bool IsCaseSensitiveAE = false;
        static bool IsCaseSensitiveCS=false;
        static bool IsCaseSensitivePN = false;
        static bool IsCaseSensitiveSH = false;
        static bool IsCaseSensitiveLO = false;
        internal void GetCustomQueryAttributes()
        {
            
        }
	}
    class CustomQueryAttributes
    {
        static CustomQueryAttributes _instance;
        static bool isFileModified = false;
        public static CustomQueryAttributes Instance
        {
            get
            {
                if (_instance == null )
                {
                    _instance = new CustomQueryAttributes();
                    _instance.AddFileWatcher();
                    _instance.ReadQueryAttributes();
                }
                else if (isFileModified)
                {
                    _instance.AddFileWatcher();
                    _instance.ReadQueryAttributes();
                }
                return _instance;
            }

        }
        public struct CustomTag
        {
            public string group;
            public string element;
        }
        FileSystemWatcher watch;
        CustomQueryAttributes()
        {
            
           
        }


        List<CustomTag> patientList = new List<CustomTag>();
        List<CustomTag> studyList = new List<CustomTag>();
        List<CustomTag> seriesList = new List<CustomTag>();
        List<CustomTag> instanceList = new List<CustomTag>();
        List<CustomTag> patientStudyList = new List<CustomTag>();
        public List<CustomTag> PatientList
        {
            get { return patientList; }
        }
        public List<CustomTag> StudyList
        {
            get { return studyList; }
        }
        public List<CustomTag> SeriesList
        {
            get { return seriesList; }
        }
        public List<CustomTag> InstanceList
        {
            get { return instanceList; }
        }
        public List<CustomTag> PatientStudyList
        {
            get { return patientStudyList; }
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            if (e.Name == @"QueryAttributes.txt")
            {
                isFileModified = true;
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            if (e.FullPath == @"QueryAttributes.txt")
                Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }
        void AddFileWatcher()
        {
            if (watch == null)
            {

                System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
                if (a == null)
                    return;
                string dir = System.IO.Path.GetDirectoryName(a.Location);
                if (!Directory.Exists(dir))
                    return;
                watch = new FileSystemWatcher();
                watch.Path = dir;
                watch.NotifyFilter = NotifyFilters.LastWrite;
                watch.Filter = "*.txt";
                watch.Changed += new FileSystemEventHandler(OnChanged);
                watch.EnableRaisingEvents = true;
                //watch.Created += new FileSystemEventHandler(OnChanged);
                //watch.Deleted += new FileSystemEventHandler(OnChanged);
                //watch.Renamed += new RenamedEventHandler(OnRenamed);

                //watch.EnableRaisingEvents = true;
            }
        }
        void ReadQueryAttributes()
        {
            patientList.Clear();
            studyList.Clear();
            seriesList.Clear();
            instanceList.Clear();
            patientStudyList.Clear();
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            if (a == null)
                return;
            string baseDir = System.IO.Path.GetDirectoryName(a.Location);
            if (!File.Exists(baseDir + @"\QueryAttributes.txt"))
                return;
            TextReader r = new StreamReader(baseDir+@"\QueryAttributes.txt");
            try
            {
                while (true)
                {
                    string s = r.ReadLine();
                    if (s.Contains("End of Document"))
                        break;
                    if (s.Contains("Patient Information Entity"))
                        ReadPatientAttributes(r);
                    if (s.Contains("Study Information Entity"))
                        ReadStudyAttributes(r);
                    if (s.Contains("Series Information Entity"))
                        ReadSeriesAttributes(r);
                    if (s.Contains("Instance information Entity"))
                        ReadInstanceAttributes(r);
                    if (s.Contains("Patient-study Information Entity"))
                        ReadPatientStudyAttributes(r);
                }
            }
            finally
            {
                r.Close();
                r.Dispose();
            }
            isFileModified = false;
        }
        void ReadPatientAttributes(TextReader r)
        {
            while (true)
            {
                string s = r.ReadLine().Trim();
                if (s.Contains("End of Patient"))
                    break;
                if (s != null && s != "")
                {
                    if (s[0] == '#')
                        continue;
                    s = s.Split('#')[0];
                    s = s.Replace("(", "");
                    s = s.Replace(")", "");
                    if (s == null || Array.IndexOf(existingPatientAttrib, s)>=0)
                        continue;
                    string[] spl = s.Split(',');
                    if (spl.Length == 2)
                    {
                        CustomTag t;
                        t.group = spl[0].Trim();
                        t.element = spl[1].Trim();
                        patientList.Add(t);
                    }
                }
            }
        }
        void ReadStudyAttributes(TextReader r)
        {
            while (true)
            {
                string s = r.ReadLine().Trim();
                if (s .Contains("End of Study"))
                    break;
                if (s != null && s != "")
                {
                    if (s[0] == '#')
                        continue;
                    s = s.Split('#')[0];
                    s = s.Replace("(", "");
                    s = s.Replace(")", "");
                    if (s == null || Array.IndexOf(existingStudyAttrib, s)>=0)
                        continue;
                    string[] spl = s.Split(',');
                    if (spl.Length == 2)
                    {
                        CustomTag t;
                        t.group = spl[0].Trim();
                        t.element = spl[1].Trim();
                        studyList.Add(t);
                    }
                }
            }
        }
        void ReadSeriesAttributes(TextReader r)
        {
            while (true)
            {
                string s = r.ReadLine().Trim();
                if (s.Contains("End of Series"))
                    break;
                if (s != null && s != "")
                {
                    if (s[0] == '#')
                        continue;
                    s = s.Split('#')[0];
                    s = s.Replace("(", "");
                    s = s.Replace(")", "");
                    if (s == null || Array.IndexOf(existingSeriesAttrib, s)>=0)
                        continue;
                    string[] spl = s.Split(',');
                    if (spl.Length == 2)
                    {
                        CustomTag t;
                        t.group = spl[0].Trim();
                        t.element = spl[1].Trim();
                        seriesList.Add(t);
                    }
                }
            }
        }
        void ReadInstanceAttributes(TextReader r)
        {
            while (true)
            {
                string s = r.ReadLine().Trim();
                if (s.Contains("End of Instance"))
                    break;
                if (s != null && s != "")
                {
                    if (s[0] == '#')
                        continue;
                    s = s.Split('#')[0];
                    s = s.Replace("(", "");
                    s = s.Replace(")", "");
                    string[] spl = s.Split(',');
                    if (s == null || Array.IndexOf(existingInstanceAttrib, s)>=0)
                        continue;
                    if (spl.Length == 2)
                    {
                        CustomTag t;
                        t.group = spl[0].Trim();
                        t.element = spl[1].Trim();
                        instanceList.Add(t);
                    }
                }
            }
        }
        void ReadPatientStudyAttributes(TextReader r)
        {
            while (true)
            {
                string s = r.ReadLine().Trim();
                if (s.Contains("End of Patient-study"))
                    break;
                if (s != null && s != "")
                {
                    if (s[0] == '#')
                        continue;
                    s = s.Split('#')[0];
                    s = s.Replace("(", "");
                    s = s.Replace(")", "");
                    string[] spl = s.Split(',');
                    if (s == null || Array.IndexOf(existingPatientStudyAttrib, s) >= 0)
                        continue;
                    if (spl.Length == 2)
                    {
                        CustomTag t;
                        t.group = spl[0].Trim();
                        t.element = spl[1].Trim();
                        instanceList.Add(t);
                    }
                }
            }
        }


        string[] existingPatientAttrib = new string[] { "0008,0005", "0010,0010", "0010,0020", "0010,0021",
                                                        "0008,1120","0010,0030","0010,0032","0010,0040",
                                                        "0010,1000","0010,1001","0010,2160","0010,4000",
                                                        "0020,1200","0020,1202","0020,1204"};
        string[] existingStudyAttrib = new string[] {
                                                        "0008,0005","0008,0020","0008,0030","0008,0050","0020,0010","0020,000D",
                                                        "0008,0061","0008,0090","0008,1030","0008,1032","0008,1060","0008,1080",
                                                        "0008,1110","0010,1010","0010,1020","0010,1030","0010,2180","0010,21B0",
                                                        "0020,1070","0020,1206","0020,1208","4008,010C"};
        string[] existingSeriesAttrib = new string[] {  "0008,0005", "0008,0060", "0020,0011", "0020,000E", "0020,1209" };
        string[] existingInstanceAttrib = new string[] {"0008,0005","0020,0013","0020,0022","0020,0024","0020,0026","0008,0018",
                                                        "0008,0016","0008,001A","0028,0010","0028,0011"};
        string[] existingPatientStudyAttrib = new string[] {    "0008,0005", "0008,0020", "0008,0030", "0008,0050", "0010,0010", "0010,0020", 
                                                                "0020,0010", "0020,000D", "0008,0061", "0008,0090", "0008,1030", "0008,1032", 
                                                                "0008,1060", "0008,1080", "0008,1110", "0008,1120", "0010,0030", "0010,0032",
                                                                "0010,0040", "0010,1000", "0010,1001", "0010,1010", "0010,1020", "0010,1030",
                                                                "0010,2160", "0010,2180", "0010,21B0", "0010,4000", "0020,1070", "0020,1200",
                                                                "0020,1202", "0020,1204", "0020,1206", "0020,1208", "4008,010C"};

    }


}
