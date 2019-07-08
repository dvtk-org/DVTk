using System;
using System.Collections.Generic;
using System.Text;

using DvtkHighLevelInterface.Dicom.Files;

namespace DvtkHighLevelInterface.InformationModel
{
    abstract public class QueryRetrieveInformationModel : BaseInformationModel
    {
        public QueryRetrieveInformationModel(Dvtk.Dicom.InformationEntity.QueryRetrieve.QueryRetrieveInformationModel root)
            : base(root)
        {
            // Do nothing.
        }

        /// <summary>
        /// Add data to the Information Model from the given dicomFile.
        /// </summary>
        /// <param name="dicomFile">Dicom File containing the dataset used to populate the Information Model.</param>
        public void AddToInformationModel(DicomFile dicomFile)
        {
            // Add dataset to Information Model and store dataset in DCM file for possible retrieval.
            Root.AddToInformationModel(dicomFile.DvtkDataDicomFile, true);
        }

        /// <summary>
        /// Add data to the Information Model from the given dicom File with Storage option.
        /// </summary>
        /// <param name="dicomFile">Dicom File containing the dataset used to populate the Information Model.</param>
        /// <param name="storeDataset">Boolean indicating whether or not the dataset should also be stored to file for possible retrieval.</param>
        public void AddToInformationModel(DicomFile dicomFile, bool storeDataset)
        {
            // Add dataset to Information Model and store dataset in DCM file for possible retrieval.
            Root.AddToInformationModel(dicomFile.DvtkDataDicomFile, storeDataset);
        }

        protected Dvtk.Dicom.InformationEntity.QueryRetrieve.QueryRetrieveInformationModel Root
        {
            get
            {
                return (_root as Dvtk.Dicom.InformationEntity.QueryRetrieve.QueryRetrieveInformationModel);
            }
        }
    }
}
