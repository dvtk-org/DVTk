using System;
using System.Collections.Generic;
using System.Text;

using Dvtk.Dicom;
using Dvtk.Dicom.AttributeLayer;

namespace Dvtk.Dicom.Conversion
{
    /// <summary>
    /// Class containing conversion functionality.
    /// </summary>
    public static class Convert
    {
        #region - Public methods -
        // -------------------------------
        // - Begin public methods region -
        // -------------------------------

        /// <summary>
        /// Convert a HLI Attribute Set to a AttributeLayer Attribute Set.
        /// </summary>
        /// <param name="attributeSetIn">The HLI Attribute Set.</param>
        /// <param name="attributeSetOut">The AttributeLayer Attribute Set.</param>
        public static void ToAttributeSet(DvtkHighLevelInterface.Dicom.Other.AttributeSet attributeSetIn, Dvtk.Dicom.AttributeLayer.AttributeSet attributeSetOut)
        {
            for (int index = 0; index < attributeSetIn.Count; index++)
            {
                DvtkHighLevelInterface.Dicom.Other.Attribute hliAttribute = attributeSetIn[index];

                Tag tag = new Tag(hliAttribute.GroupNumber, hliAttribute.ElementNumber);

                if (hliAttribute.VR != DvtkData.Dimse.VR.SQ)
                {
                    SingleAttribute singleAttribute = new SingleAttribute(tag, (VR)Enum.Parse(typeof(VR), hliAttribute.VR.ToString(), true), attributeSetOut);
                }
                else
                {
                    SequenceAttribute sequenceAttribute = new SequenceAttribute(tag, attributeSetOut);

                    for (int sequenceItemIndex = 1; sequenceItemIndex <= hliAttribute.ItemCount; sequenceItemIndex++)
                    {
                        DvtkHighLevelInterface.Dicom.Other.SequenceItem hliSequenceItem = hliAttribute.GetItem(sequenceItemIndex);

                        SequenceItem sequenceItem = new SequenceItem(sequenceAttribute);

                        ToAttributeSet(hliSequenceItem, sequenceItem);
                    }
                }
            }
        }

        // -----------------------------
        // - End public methods region -
        // -----------------------------
        #endregion
    }
}
