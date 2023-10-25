using System;
using System.Collections.Generic;
using System.Text;
using DvtkData.Dimse;


namespace Dvtk.Dicom.InformationEntity
{
   public class BaseMethods
    {
        public virtual void AddToInformationModel( DataSet dataset, bool storeDataset)
        {
        }

        public virtual void AddToInformationModel(DvtkData.Media.DicomFile dicomFile, bool storeFile)
        {
        }
    }
}
