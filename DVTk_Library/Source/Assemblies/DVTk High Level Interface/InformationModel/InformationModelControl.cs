using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dvtk.Dicom.InformationEntity.CompositeInfoModel;

namespace DvtkHighLevelInterface.InformationModel
{
    /// <summary>
    /// Information Model control class is to display the Patient, Study, Patient/Study information models. It is designed to handle Composite Data modek objects.
    /// 
    /// </summary>
    public partial class InformationModelControl : UserControl
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public InformationModelControl()
        {
            InitializeComponent();
        }

        List<BaseCompositeInformationEntity> patientList,studyList,patientStudyList;
        /// <summary>
        /// Sets the selected infromation models
        /// </summary>
        /// <param name="isPatientRoot">True-if patient root is enabled</param>
        /// <param name="isStudyRoot">True-if study root is enabled</param>
        /// <param name="isPatientStudyRoot">True- if patient/study root is enabled</param>
        public void SetInfoModel(bool isPatientRoot, bool isStudyRoot, bool isPatientStudyRoot)
        {
            patientSelection.Enabled = isPatientRoot;
            studySelection.Enabled = isStudyRoot;
            patientStudyselection.Enabled = isPatientStudyRoot;

        }
        /// <summary>
        /// Lods the composite datas to the view
        /// </summary>
        /// <param name="_patient">List of patients. This can be null if PatientRoot not selected</param>
        /// <param name="_study">List of studies. This can be null if StudyRoot not selected</param>
        /// <param name="_patientStudy">List of pateint/studies. This can be null if Patient/Study Root not selected</param>
        public void LoadData(List<BaseCompositeInformationEntity> _patient,List<BaseCompositeInformationEntity> _study,List<BaseCompositeInformationEntity> _patientStudy)
        {
            patientList = _patient;
            studyList = _study;
            patientStudyList = _patientStudy;
            if(patientSelection.Enabled)
                patientSelection.Checked=true;
            else if(studySelection.Enabled )
                studySelection.Checked = true;
            else if(patientStudyselection.Enabled)
                patientStudyselection.Checked = true;
        }
        void imageTable_GotFocus(object sender, System.EventArgs e)
        {
            imageTable_SelectedIndexChanged(null, null);
        }
        void seriesTable_GotFocus(object sender, System.EventArgs e)
        {
            seriesTable_SelectedIndexChanged(null, null);
        }
        void studyTable_GotFocus(object sender, System.EventArgs e)
        {
            studyTable_SelectedIndexChanged(null, null);
        }
        void patientTable_GotFocus(object sender, System.EventArgs e)
        {
            patientTable_SelectedIndexChanged(null, null);
        }
        
        private void patientSelection_CheckedChanged(object sender, EventArgs e)
        {
            if (patientSelection.Checked)
            {
                ClearAllTable();
                foreach(Patient p in patientList)
                {
                  patientTable.Items.Add(new ListViewItem(new string[]{p.PatientName,p.PatientId}));
                }
            }
        }
        private void studySelection_CheckedChanged(object sender, EventArgs e)
        {
            if (studySelection.Checked)
            {
                ClearAllTable();
                patientTable.Enabled = false;
                foreach (Study s in studyList)
                {
                    studyTable.Items.Add(new ListViewItem(new string[] { s.StudyID, s.StudyInstanceUID}));
                }
            }
        }

        private void patientStudyselection_CheckedChanged(object sender, EventArgs e)
        {
            if (patientStudyselection.Checked)
            {
                ClearAllTable();
                seriesTable.Enabled = false;
                imageTable.Enabled = false;
                foreach (Patient p in patientStudyList)
                {
                    patientTable.Items.Add(new ListViewItem(new string[] { p.PatientName, p.PatientId }));
                }
            }
        }
        private void patientTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            imageTable.Items.Clear();
            seriesTable.Items.Clear();
            if (patientTable.SelectedIndices.Count > 0)
            {
                ClearLabels();
                HighlightLabel(patientLabel);
                studyTable.Items.Clear();
                attribTable.Items.Clear();
                if (patientSelection.Checked)
                {
                    foreach (Study s in patientList[patientTable.SelectedIndices[0]].Children)
                    {
                        studyTable.Items.Add(new ListViewItem(new string[] { s.StudyID, s.StudyInstanceUID }));
                    }
                    LoadAttributeTable(patientList[patientTable.SelectedIndices[0]].Attributes);
                }
                else if (patientStudyselection.Checked)
                {
                    foreach (Study s in patientStudyList[patientTable.SelectedIndices[0]].Children)
                    {
                        studyTable.Items.Add(new ListViewItem(new string[] { s.StudyID, s.StudyInstanceUID }));
                    }
                    LoadAttributeTable(patientStudyList[patientTable.SelectedIndices[0]].Attributes);
                }
            }
        }

        private void studyTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            imageTable.Items.Clear();
            if (patientTable.SelectedIndices.Count > 0&&studyTable.SelectedIndices.Count>0)
            {
                ClearLabels();
                HighlightLabel(studyLabel);
                seriesTable.Items.Clear();
                attribTable.Items.Clear();
                
                if (patientSelection.Checked)
                {
                    foreach (Series s in patientList[patientTable.SelectedIndices[0]].Children[studyTable.SelectedIndices[0]].Children)
                    {
                        seriesTable.Items.Add(new ListViewItem(new string[] { s.InstanceNumber, s.SeriesInstanceUID}));
                    }
                    LoadAttributeTable(patientList[patientTable.SelectedIndices[0]].Children[studyTable.SelectedIndices[0]].Attributes);
                }
                else if (patientStudyselection.Checked)
                {
                    LoadAttributeTable(patientStudyList[patientTable.SelectedIndices[0]].Children[studyTable.SelectedIndices[0]].Attributes);
                }
            }
            else if (studySelection.Checked && studyTable.SelectedIndices.Count > 0)
            {
                ClearLabels();
                HighlightLabel(studyLabel);
                seriesTable.Items.Clear();
                attribTable.Items.Clear();
                foreach (Series s in studyList[studyTable.SelectedIndices[0]].Children)
                {
                    seriesTable.Items.Add(new ListViewItem(new string[] { s.InstanceNumber, s.SeriesInstanceUID }));
                }
                LoadAttributeTable(studyList[studyTable.SelectedIndices[0]].Attributes);
            }

        }

        private void seriesTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (patientTable.SelectedIndices.Count > 0 && studyTable.SelectedIndices.Count > 0&&seriesTable.SelectedIndices.Count>0)
            {
                ClearLabels();
                HighlightLabel(seriesLabel);
                imageTable.Items.Clear();
                attribTable.Items.Clear();
                if (patientSelection.Checked)
                {
                    foreach (Dvtk.Dicom.InformationEntity.CompositeInfoModel.Image i in patientList[patientTable.SelectedIndices[0]].
                        Children[studyTable.SelectedIndices[0]].
                        Children[seriesTable.SelectedIndices[0]].Children)
                    {
                        imageTable.Items.Add(new ListViewItem(new string[] { i.InstanceNumber, i.SOPInstanceUID}));
                    }
                    LoadAttributeTable(patientList[patientTable.SelectedIndices[0]].Children[studyTable.SelectedIndices[0]].Children[seriesTable.SelectedIndices[0]].Attributes);
                }
            }
            else if (studySelection.Checked && studyTable.SelectedIndices.Count > 0&&seriesTable.SelectedIndices.Count>0)
            {
                ClearLabels();
                HighlightLabel(seriesLabel);
                imageTable.Items.Clear();
                attribTable.Items.Clear();
                foreach (Dvtk.Dicom.InformationEntity.CompositeInfoModel.Image i in studyList[studyTable.SelectedIndices[0]].Children[seriesTable.SelectedIndices[0]].Children)
                {
                    imageTable.Items.Add(new ListViewItem(new string[] { i.InstanceNumber, i.SOPInstanceUID }));
                }
                LoadAttributeTable(studyList[studyTable.SelectedIndices[0]].Children[seriesTable.SelectedIndices[0]].Attributes);
            }
        }

        private void imageTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (patientTable.SelectedIndices.Count > 0 && 
                studyTable.SelectedIndices.Count > 0 && 
                seriesTable.SelectedIndices.Count > 0&&
                imageTable.SelectedIndices.Count>0)
            {
                ClearLabels();
                HighlightLabel(imageLabel);
                attribTable.Items.Clear();
                if (patientSelection.Checked)
                {
                    LoadAttributeTable(patientList[patientTable.SelectedIndices[0]].
                        Children[studyTable.SelectedIndices[0]].
                        Children[seriesTable.SelectedIndices[0]].
                        Children[imageTable.SelectedIndices[0]].Attributes);
                }
            }
            else if (studySelection.Checked && 
                studyTable.SelectedIndices.Count > 0&&
                seriesTable.SelectedIndices.Count>0&&
                imageTable.SelectedIndices.Count>0)
            {
                ClearLabels();
                HighlightLabel(imageLabel);
                attribTable.Items.Clear();
                LoadAttributeTable(studyList[studyTable.SelectedIndices[0]].Children[seriesTable.SelectedIndices[0]].Children[imageTable.SelectedIndices[0]].Attributes);
            }
        }
        private void ClearAllTable()
        {
            patientTable.Items.Clear();
            studyTable.Items.Clear();
            seriesTable.Items.Clear();
            attribTable.Items.Clear();
            imageTable.Items.Clear();
            patientTable.Enabled = true;
            studyTable.Enabled = true;
            seriesTable.Enabled = true;
            imageTable.Enabled = true;
            ClearLabels();
        }
        private void ClearLabels()
        {
            patientLabel.Font = new Font(patientLabel.Font, FontStyle.Regular);
            studyLabel.Font = new Font(studyLabel.Font, FontStyle.Regular);
            seriesLabel.Font = new Font(seriesLabel.Font, FontStyle.Regular);
            imageLabel.Font = new Font(imageLabel.Font, FontStyle.Regular);
        }
        private void HighlightLabel(Label label)
        {
            label.Font=new Font(label.Font,FontStyle.Bold);
        }
        void LoadAttributeTable(List<DvtkData.Dimse.Attribute> list)
        {
            foreach (DvtkData.Dimse.Attribute a in list)
            {
                if (a.Tag == DvtkData.Dimse.Tag.QUERY_RETRIEVE_LEVEL)
                {
                    //DvtkData.Dimse.Tag t = new DvtkData.Dimse.Tag(a.Tag.GroupNumber, a.Tag.ElementNumber);

                    attribTable.Items.Add(new ListViewItem(new string[] { a.Tag.GroupNumber.ToString("X4") + "," + a.Tag.ElementNumber.ToString("X4"), "Query/Retrieve Level", a.ValueRepresentation.ToString(), BaseCompositeInformationEntity.GetDicomValue(a) }));
                }
                else
                    attribTable.Items.Add(new ListViewItem(new string[] { a.Tag.GroupNumber.ToString("X4") + "," + a.Tag.ElementNumber.ToString("X4"), a.Name, a.ValueRepresentation.ToString(), BaseCompositeInformationEntity.GetDicomValue(a) }));
               
            }
        }
        /// <summary>
        /// Size of the control can be set from outside. So the cotrols re-sized accordingly
        /// </summary>
        /// <param name="height">new height of the control</param>
        /// <param name="width">new width of the control</param>
        public void Resize(int height, int width)
        {
            this.Width = width;
            this.Height = height;
            double controlWidth = (240.0 / 992.0) * width;
            patientGroup.Width = studyGroup.Width = seriesGroup.Width = imageGroup.Width =(int) controlWidth;
        }
    }
}
