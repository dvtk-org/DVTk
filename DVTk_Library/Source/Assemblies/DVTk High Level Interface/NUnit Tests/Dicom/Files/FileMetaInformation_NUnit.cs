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
using DvtkData.Dimse;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Dicom.Files
{
    /// <summary>
    /// Contains NUnit Test Cases.
    /// </summary>
    [TestFixture]
    public class FileMetaInformation_NUnit
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Test methods can use this field to reduce the method length.
        /// </summary>
        FileMetaInformation fileMetaInformation = null;



        //
        // - Methods containing common functionality for all test methods -
        //

        /// <summary>
        /// This method is performed just before each test method is called.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.fileMetaInformation = new FileMetaInformation();
        }

        /// <summary>
        /// This method is performed after each test method is run.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.fileMetaInformation = null;
        }

        /// <summary>
        /// This method is performed once prior to executing any of the tests
        /// in this class.
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Dvtk.Setup.Initialize();
        }

        /// <summary>
        /// This method is performed once after all tests are completed in this
        /// class.
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Dvtk.Setup.Terminate();
        }



        //
        // - Test methods -
        //

        [Test]
        public void Test1()
        {
            Assert.That(fileMetaInformation, Is.InstanceOfType(typeof(FileMetaInformation)));
            Assert.That(fileMetaInformation.Count, Is.EqualTo(3));

            Assert.That(fileMetaInformation["0x00020001"].Exists, Is.True);
            Assert.That(fileMetaInformation["0x00020001"].VR, Is.EqualTo(VR.OB));
            Assert.That(fileMetaInformation["0x00020001"].VM, Is.EqualTo(1));

            Assert.That(fileMetaInformation["0x00020010"].Exists, Is.True);
            Assert.That(fileMetaInformation["0x00020010"].VR, Is.EqualTo(VR.UI));
            Assert.That(fileMetaInformation["0x00020010"].VM, Is.EqualTo(1));
            Assert.That(fileMetaInformation["0x00020010"].Values[0], Is.EqualTo("1.2.840.10008.1.2.1"));

            Assert.That(fileMetaInformation["0x00020012"].Exists, Is.True);
            Assert.That(fileMetaInformation["0x00020012"].VR, Is.EqualTo(VR.UI));
            Assert.That(fileMetaInformation["0x00020012"].VM, Is.EqualTo(1));
            Assert.That(fileMetaInformation["0x00020012"].Values[0], Is.GreaterThan("1.2.826.0.1.3680043.2.1545.1."));
        }

        /// <summary>
        ///     Get an attribute given the TagSequence.
        ///     The TagSequence supplied must be single attribute matching.
        /// 
        ///     Test that invalid parameter values are handled properly.
        ///     Test that non-existing tags are handled properly.
        ///     Test that all existing tags are handled properly.
        /// </summary>
        // Removed until ticket 973 has been solved [Test].
        public void this_tagSequence()
        {
            String tagSequence;
            string attributeName;
            string values;

            // empty value exception
            tagSequence = "";
            try
            {
                DvtkHighLevelInterface.Dicom.Other.Attribute attribute = fileMetaInformation[tagSequence];
            }
            catch(DvtkHighLevelInterface.Common.Other.HliException)
            {
            }

            // invalid value exception
            tagSequence = "TransferSyntax";
            try
            {
                DvtkHighLevelInterface.Dicom.Other.Attribute attribute = fileMetaInformation[tagSequence];
            }
            catch (DvtkHighLevelInterface.Common.Other.HliException)
            {
            }

            // non-existing tag
            Assert.That(fileMetaInformation["0x00020000"].Exists, Is.False);
            Assert.That(fileMetaInformation["0x00020000"].GroupNumber, Is.EqualTo(2));
            Assert.That(fileMetaInformation["0x00020000"].ElementNumber, Is.EqualTo(0));
            Assert.That(fileMetaInformation["0x00020000"].TagSequenceString, Is.EqualTo("0x00020000"));
            Assert.That(fileMetaInformation["0x00020000"].VR, Is.EqualTo(VR.UN));
            Assert.That(fileMetaInformation["0x00020000"].VM, Is.EqualTo(0));
            Assert.That(fileMetaInformation["0x00020000"].Values[0], Is.Empty);
            Assert.That(fileMetaInformation["0x00020000"].Name, Is.Empty);
            Assert.That(fileMetaInformation["0x00020000"].ItemCount, Is.EqualTo(0));

            // existing tags
            Assert.That(fileMetaInformation["0x00020001"].Exists, Is.True);
            Assert.That(fileMetaInformation["0x00020001"].GroupNumber, Is.EqualTo(2));
            Assert.That(fileMetaInformation["0x00020001"].ElementNumber, Is.EqualTo(1));
            Assert.That(fileMetaInformation["0x00020001"].TagSequenceString, Is.EqualTo("0x00020001"));
            Assert.That(fileMetaInformation["0x00020001"].Name, Is.Empty);
            Assert.That(fileMetaInformation["0x00020001"].ItemCount, Is.EqualTo(0));

            Assert.That(fileMetaInformation["0x00020010"].Exists, Is.True);
            Assert.That(fileMetaInformation["0x00020010"].GroupNumber, Is.EqualTo(2));
            Assert.That(fileMetaInformation["0x00020010"].ElementNumber, Is.EqualTo(16));
            Assert.That(fileMetaInformation["0x00020010"].TagSequenceString, Is.EqualTo("0x00020010"));
            Assert.That(fileMetaInformation["0x00020010"].Name, Is.Empty);
            Assert.That(fileMetaInformation["0x00020010"].ItemCount, Is.EqualTo(0));

            Assert.That(fileMetaInformation["0x00020012"].Exists, Is.True);
            Assert.That(fileMetaInformation["0x00020012"].GroupNumber, Is.EqualTo(2));
            Assert.That(fileMetaInformation["0x00020012"].ElementNumber, Is.EqualTo(18));
            Assert.That(fileMetaInformation["0x00020012"].TagSequenceString, Is.EqualTo("0x00020012"));
            Assert.That(fileMetaInformation["0x00020012"].Name, Is.Empty);
            Assert.That(fileMetaInformation["0x00020012"].ItemCount, Is.EqualTo(0));

            // set non-existing tag properties
            values = "80";
            fileMetaInformation["0x00020000"].Values[0] = values;
            Assert.That(fileMetaInformation["0x00020000"].Values[0], Is.EqualTo(values));

            attributeName = "Group Length";
            fileMetaInformation["0x00020000"].Name = attributeName;
            Assert.That(fileMetaInformation["0x00020000"].Name, Is.EqualTo(attributeName));

            // set existing tag properties
            values = "ILE";
            fileMetaInformation["0x00020010"].Values[0] = values;
            Assert.That(fileMetaInformation["0x00020010"].Values[0], Is.EqualTo(values));

            attributeName = "Transfer Syntax";
            fileMetaInformation["0x00020010"].Name = attributeName;
            Assert.That(fileMetaInformation["0x00020010"].Name, Is.EqualTo(attributeName));
        }

        /// <summary>
        ///     Get an attribute given the zero based index for this AttributeSet.
        /// 
        ///     Test that invalid index values are handled properly.
        ///     Test that valid index values are handled properly.
        /// </summary>
        [Test]
        public void this_zeroBasedIndex()
        {
            int zeroBasedIndex;
            string attributeName;
            string values;

            // lower bound index exception
            try
            {
                zeroBasedIndex = -1;
                DvtkHighLevelInterface.Dicom.Other.Attribute attribute = fileMetaInformation[zeroBasedIndex];
            }
            catch (System.Exception)
            {
            }

            // valid index values {0|1|2}
            Assert.That(fileMetaInformation[0].GroupNumber, Is.EqualTo(2));
            Assert.That(fileMetaInformation[0].ElementNumber, Is.EqualTo(1));
            Assert.That(fileMetaInformation[0].TagSequenceString, Is.EqualTo("0x00020001"));
            Assert.That(fileMetaInformation[0].Name, Is.Empty);
            Assert.That(fileMetaInformation[0].ItemCount, Is.EqualTo(0));

            Assert.That(fileMetaInformation[1].GroupNumber, Is.EqualTo(2));
            Assert.That(fileMetaInformation[1].ElementNumber, Is.EqualTo(16));
            Assert.That(fileMetaInformation[1].TagSequenceString, Is.EqualTo("0x00020010"));
            Assert.That(fileMetaInformation[1].Name, Is.Empty);
            Assert.That(fileMetaInformation[1].ItemCount, Is.EqualTo(0));

            Assert.That(fileMetaInformation[2].GroupNumber, Is.EqualTo(2));
            Assert.That(fileMetaInformation[2].ElementNumber, Is.EqualTo(18));
            Assert.That(fileMetaInformation[2].TagSequenceString, Is.EqualTo("0x00020012"));
            Assert.That(fileMetaInformation[2].Name, Is.Empty);
            Assert.That(fileMetaInformation[2].ItemCount, Is.EqualTo(0));

            // upper bound index exception
            try
            {
                zeroBasedIndex = 3;
                DvtkHighLevelInterface.Dicom.Other.Attribute attribute = fileMetaInformation[zeroBasedIndex];
            }
            catch (System.Exception)
            {
            }

            // set existing tag properties
            values = "ILE";
            fileMetaInformation[1].Values[0] = values;
            Assert.That(fileMetaInformation[1].Values[0], Is.EqualTo(values));

            attributeName = "Transfer Syntax";
            fileMetaInformation[1].Name = attributeName;
            Assert.That(fileMetaInformation[1].Name, Is.EqualTo(attributeName));
        }

        /// <summary>
        ///     Gets the number of attributes.
        /// 
        ///     Default construction:
        /// 	  File Meta Information Version (0x00020001, OB, )
        /// 	  TransferSyntax                (0x00020010, UI, "1.2.840.10008.1.2.1")
        /// 	  Implementation Class UID      (0x00020012, UI, "1.2.826.0.1.3680043.2.1545.1.Major.Minor.Build.Revision")
        /// 
        ///     Test that the actual (default) value is retrieved.
        /// </summary>
        [Test]
        public void Count()
        {
            // actual (default) value
            Assert.That(fileMetaInformation.Count, Is.EqualTo(3));
        }

        /// <summary>
        ///     Get and set the DICOM prefix.
        /// 
        ///     Test that the actual (default) value is retrieved.
        ///     Test that invalid parameter values are handled properly.
        ///     Test that valid parameter values are handled properly.
        /// </summary>
        [Test]
        public void DicomPrefix()
        {
            byte[] actualPrefix = fileMetaInformation.DicomPrefix;
            byte[] dicomPrefix = new System.Byte[4] { 68, 73, 67, 77 }; // DICM
            byte[] hl7Prefix = new System.Byte[3] { 72, 76, 37 }; // HL7
            byte[] noPrefix = new System.Byte[4] { 00, 00, 00, 00 };

            // actual (default) value
            Assert.That(actualPrefix, Is.EqualTo(dicomPrefix));

            // null value exception
            try
            {
                fileMetaInformation.DicomPrefix = null;
            }
            catch (System.ArgumentNullException)
            {
            }

            // invalid value exception
            try
            {
                fileMetaInformation.DicomPrefix = hl7Prefix;
            }
            catch (System.ArgumentException)
            {
            }

            // actual value after exceptions
            Assert.That(fileMetaInformation.DicomPrefix, Is.EqualTo(actualPrefix));

            // empty value
            fileMetaInformation.DicomPrefix = noPrefix;
            Assert.That(fileMetaInformation.DicomPrefix, Is.EqualTo(noPrefix));

            // normal value
            fileMetaInformation.DicomPrefix = dicomPrefix;
            Assert.That(fileMetaInformation.DicomPrefix, Is.EqualTo(dicomPrefix));
        }

        /// <summary>
        ///     Get and set the file preamble.
        /// 
        ///     Test that the actual (default) value is retrieved.
        ///     Test that invalid parameter values are handled properly.
        ///     Test that valid parameter values are handled properly.
        /// </summary>
        [Test]
        public void FilePreamble()
        {
            byte[] filePreamble = fileMetaInformation.FilePreamble;

            // actual (default) value
            Assert.That(filePreamble, Is.All.EqualTo(0));

            // null value exception
            try
            {
                fileMetaInformation.FilePreamble = null;
            }
            catch (System.ArgumentNullException)
            {
            }

            // actual value after exceptions
            Assert.That(fileMetaInformation.FilePreamble, Is.EqualTo(filePreamble));

            // new value
            filePreamble[1] = 1;
            fileMetaInformation.FilePreamble = filePreamble;
            Assert.That(fileMetaInformation.FilePreamble, Is.EqualTo(filePreamble));
        }

        ///// <summary>
        /////     Unknown implementation...
        ///// </summary>
        //[Test, Ignore]
        //public void Item()
        //{
        //}

        /// <summary>
        ///     Get and set the Media Storage SOP Class UID ((0002,0002)).
        /// 
        ///     Test that the actual (default) value is retrieved.
        ///     Test that invalid values are handled properly.
        ///     Test that valid values are handled properly.
        /// </summary>
        [Test]
        public void MediaStorageSOPClassUID()
        {
            string mediaStorageSOPClassUID;

            // actual (default) value
            Assert.That(fileMetaInformation.MediaStorageSOPClassUID, Is.Empty);

            // test invalid UID
            mediaStorageSOPClassUID = "1,034.06 09A";
            fileMetaInformation.MediaStorageSOPClassUID = mediaStorageSOPClassUID;
            Assert.That(fileMetaInformation.MediaStorageSOPClassUID, Is.EqualTo(mediaStorageSOPClassUID));

            // test valid UID
            mediaStorageSOPClassUID = "1.2.840.10008.1.3.10";
            fileMetaInformation.MediaStorageSOPClassUID = mediaStorageSOPClassUID;
            Assert.That(fileMetaInformation.MediaStorageSOPClassUID, Is.EqualTo(mediaStorageSOPClassUID));

            // test empty UID
            fileMetaInformation.MediaStorageSOPClassUID = "";
            Assert.That(fileMetaInformation.MediaStorageSOPClassUID, Is.Empty);
        }

        /// <summary>
        ///     Get and set the Media Storage SOP Instance UID ((0002,0003)).
        /// 
        ///     Test that the actual (default) value is retrieved.
        ///     Test that invalid values are handled properly.
        ///     Test that valid values are handled properly.
        /// </summary>
        [Test]
        public void MediaStorageSOPInstanceUID()
        {
            string mediaStorageSOPInstanceUID;

            // initial value
            Assert.That(fileMetaInformation.MediaStorageSOPInstanceUID, Is.Empty);

            // test invalid UID
            mediaStorageSOPInstanceUID = "1,034.06 09A";
            fileMetaInformation.MediaStorageSOPInstanceUID = mediaStorageSOPInstanceUID;
            Assert.That(fileMetaInformation.MediaStorageSOPInstanceUID, Is.EqualTo(mediaStorageSOPInstanceUID));

            // test valid UID
            mediaStorageSOPInstanceUID = "1.2.826.0.1.3680043.2.1545.1.2.3.1.1.1";
            fileMetaInformation.MediaStorageSOPInstanceUID = mediaStorageSOPInstanceUID;
            Assert.That(fileMetaInformation.MediaStorageSOPInstanceUID, Is.EqualTo(mediaStorageSOPInstanceUID));

            // test empty UID
            fileMetaInformation.MediaStorageSOPInstanceUID = "";
            Assert.That(fileMetaInformation.MediaStorageSOPInstanceUID, Is.Empty);
        }

        /// <summary>
        ///     Get and set the transfer syntax.
        /// 
        ///     Default is 1.2.840.10008.1.2.1 (Explicit Little Endian).
        /// 
        ///     Test that the actual (default) value is retrieved.
        ///     Test that invalid values are handled properly.
        ///     Test that valid values are handled properly.
        /// </summary>
        [Test]
        public void TransferSyntax()
        {
            string actualTransferSyntax = fileMetaInformation.TransferSyntax;
            string textualTransferSyntax = "Explicit Little Endian";

            // test actual value
            Assert.That(actualTransferSyntax, Is.EqualTo("1.2.840.10008.1.2.1"));

            // test empty value
            fileMetaInformation.TransferSyntax = "";
            Assert.That(fileMetaInformation.TransferSyntax, Is.Empty);

            // test text value
            fileMetaInformation.TransferSyntax = textualTransferSyntax;
            Assert.That(fileMetaInformation.TransferSyntax, Is.EqualTo(textualTransferSyntax));

            // test initial (UID) value
            fileMetaInformation.TransferSyntax = actualTransferSyntax;
            Assert.That(fileMetaInformation.TransferSyntax, Is.EqualTo(actualTransferSyntax));
        }

        // =======================
        // Public Instance Methods
        // =======================

        /// <summary>
        /// Test that invalid parameter values are handled properly.
        /// </summary>
        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void Add_attribute()
        {
            DvtkHighLevelInterface.Dicom.Other.Attribute attribute = null;
            fileMetaInformation.Add(attribute);
        }

        /// <summary>
        ///     Adds (using deep copy) the sequenceItem to all Sequence Attributes
        ///     indicated by tagSequence.
        /// </summary>
        [Test]
        public void AddItem_tagSequence_sequenceItem()
        {
            String tagSequence = "";
            DvtkHighLevelInterface.Dicom.Other.SequenceItem sequenceItem = null;

            // null value exception
            try
            {
                fileMetaInformation.AddItem(tagSequence, sequenceItem);
            }
            catch (DvtkHighLevelInterface.Common.Other.HliException)
            {
            }
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void AddValues_tagSequence_values()
        {
            String tagSequence = "";
            Values values = null;

            // null value exception
            try
            {
                fileMetaInformation.AddValues(tagSequence, values);
            }
            catch (DvtkHighLevelInterface.Common.Other.HliException)
            {
            }
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void AddValues_tagSequence_parameters()
        {
            String tagSequence = "";
            Object[] parameters = null;

            // null value exception
            try
            {
                fileMetaInformation.AddValues(tagSequence, parameters);
            }
            catch (DvtkHighLevelInterface.Common.Other.HliException)
            {
            }
        }

        /// <summary>
        ///     When the tag sequence is single attribute specifying, this method 
        ///     does the following:
        ///     - When the attribute already exists and also has VR SQ, the values 
        ///       or sequence items are appended to the already existing values or 
        ///       sequence items.
        ///     - When the attribute already exists but has a different VR, an 
        ///       error is displayed and nothing is appended.
        ///     - When the attribute does not already exists, it is created and the 
        ///       values or sequence items are added.
        /// 
        ///     When the tag sequence contains wildcards, the rules above are 
        ///     applied to each existing attribute specified by the tag sequence.
        /// 
        ///     Whenever the VR specified is SQ and the parameters supplied contain 
        ///     non sequence item(s), and exception is thrown.
        ///     Whenever the VR specified is unequal SQ and the parameters contain 
        ///     sequence item(s), and exception is thrown.
        /// </summary>
        [Test]
        public void Append_tagSequence_vR_parameters()
        {
            String tagSequence = "";
            VR vR = VR.UN;
            Object[] parameters = null;


            // null value exception
            try
            {
                fileMetaInformation.Append("0x00020000", VR.UL, parameters);
            }
            catch (System.NullReferenceException)
            {
            }

            // empty string exception
            try
            {
                fileMetaInformation.Append(tagSequence, vR, parameters);
            }
            catch (System.Exception)
            {
            }
        }

        /// <summary>
        ///     Remove all attributes from the AttributeSet.
        /// </summary>
        [Test]
        public void Clear_()
        {
            fileMetaInformation.Clear();
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void ClearValues_tagSequence()
        {
            String tagSequence = "";

            fileMetaInformation.ClearValues("0x00020000");

            fileMetaInformation.ClearValues("0x00020010");

            // empty string exception
            try
            {
                fileMetaInformation.ClearValues(tagSequence);
            }
            catch (DvtkHighLevelInterface.Common.Other.HliException)
            {
            }
        }

        /// <summary>
        ///     Delete Attributes.
        /// 
        ///     The tag sequence supplied can be both single attribute matching and
        ///     wildcard attribute matching.
        /// </summary>
        [Test]
        public void Delete_tagSequence()
        {
            String tagSequence = "";

            fileMetaInformation.Delete("0x00020000");

            fileMetaInformation.Delete("0x00020010");

            // empty string exception
            try
            {
                fileMetaInformation.Delete(tagSequence);
            }
            catch (DvtkHighLevelInterface.Common.Other.HliException)
            {
            }
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Equals_object()
        {
            Assert.That(fileMetaInformation.Equals(fileMetaInformation), Is.True);
        }

        /// <summary>
        ///     Indicates if the attribute with the supplied tag sequence exists.
        /// 
        ///     Returns a boolean that indicates if the attribute exists.
        /// </summary>
        [Test]
        public void Exists_tagSequence()
        {
            String tagSequence = "";

            Assert.That(fileMetaInformation.Exists("0x00020000"), Is.False);
            Assert.That(fileMetaInformation.Exists("0x00020001"), Is.True);
            Assert.That(fileMetaInformation.Exists("0x00020010"), Is.True);
            Assert.That(fileMetaInformation.Exists("0x00020012"), Is.True);

            // empty string exception
            try
            {
                Assert.That(fileMetaInformation.Exists(tagSequence), Is.False);
            }
            catch (DvtkHighLevelInterface.Common.Other.HliException)
            {
            }
        }

        /// <summary>
        ///		Get a SequenceItem by specifying a tagSequence.
        ///     oneBasedIndex is used to specify which SequenceItem is 
        ///     requested. (1 for the first SequenceItem in the sequence)
        /// 
        ///     Returns a Values Object containing a list of all values the 
        ///     attribute contains.
        /// 
        ///     Throws exception "HliException" when Tag sequence supplied invalid 
        ///     for this operation.
        /// 
        ///	    If the attribute requested by the tagSequence is non existent or 
        ///     invalid an empty SequenceItem will be returned.
        /// </summary>
        [Test]
        public void Getitem_tagSequence_oneBasedIndex_1()
        {
            Assert.That(fileMetaInformation.Getitem("0x00020000", 1), Is.TypeOf(typeof(DvtkHighLevelInterface.Dicom.Other.SequenceItem)));
            Assert.That(fileMetaInformation.Getitem("0x00020001", 1), Is.TypeOf(typeof(DvtkHighLevelInterface.Dicom.Other.SequenceItem)));
            Assert.That(fileMetaInformation.Getitem("0x00020010", 1), Is.TypeOf(typeof(DvtkHighLevelInterface.Dicom.Other.SequenceItem)));
            Assert.That(fileMetaInformation.Getitem("0x00020012", 1), Is.TypeOf(typeof(DvtkHighLevelInterface.Dicom.Other.SequenceItem)));
        }

        [Test]
        [ExpectedException("System.ArgumentException")]
        public void Getitem_tagSequence_oneBasedIndex_2()
        {
            fileMetaInformation.Getitem("", 0);
        }

        /// <summary>
        ///		Get the number of SequenceItems by specifying a tagSequence.
        /// 
        ///     Returns a Values Object containing a list of all values the 
        ///     attribute contains.
        /// 
        ///     Throws exception "HliException" when Tag sequence supplied invalid 
        ///     for this operation.
        /// 
        ///	    If the attribute requested by the tagSequence is non existent or 
        ///     invalid 0 will be returned.
        /// </summary>
        [Test]
        public void GetitemCount_tagSequence_1()
        {
            Assert.That(fileMetaInformation.GetitemCount("0x00020000"), Is.EqualTo(0));
            Assert.That(fileMetaInformation.GetitemCount("0x00020001"), Is.EqualTo(0));
            Assert.That(fileMetaInformation.GetitemCount("0x00020010"), Is.EqualTo(0));
            Assert.That(fileMetaInformation.GetitemCount("0x00020012"), Is.EqualTo(0));
        }

        [Test]
        [ExpectedException("System.ArgumentException")] 
        public void GetitemCount_tagSequence_2()
        {
            fileMetaInformation.GetitemCount("");
        }

        /// <summary>
        ///		Get the Values of the attribute	specified in the tagSequence.
        /// 
        ///     Returns a Values Object containing a list of all values the 
        ///     attribute contains.
        /// 
        ///     Throws exception "HliException" when Tag sequence supplied invalid 
        ///     for this operation.
        /// </summary>
        [Test]
        public void GetValues_tagSequence()
        {
            bool isExceptionThrown = false;

            try
            {
                String tagSequence = "";

                Assert.That(fileMetaInformation.GetValues(tagSequence), Is.TypeOf(typeof(Values)) & Is.Null);
            }
            catch
            {
                isExceptionThrown = true;
            }

            Assert.That(isExceptionThrown, Is.EqualTo(true));
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void InsertValues_tagSequence_zeroBasedIndex_parameters()
        {
            String tagSequence = "0x00100010";
            int zeroBasedIndex = 0;

            fileMetaInformation.InsertValues(tagSequence, zeroBasedIndex, "test");
        }

        /// <summary>
        ///     Make attributes in this AttributeSet ascending (Dicom compliant).
        /// 
        ///     When recursive is true, all attributes are made ascending 
        ///     recursively, i.e. all contained sequence items are also sorted.
        /// </summary>
        [Test]
        public void MakeAscending_recursive()
        {
            bool recursive;

            recursive = true;
            fileMetaInformation.MakeAscending(recursive);

            recursive = false;
            fileMetaInformation.MakeAscending(recursive);
        }

        ///// <summary>
        /////     Testcase description...
        ///// </summary>
        //[Test, Ignore]
        //public void Randomize_stringToReplace()
        //{
        //    String stringToReplace = "";

        //    fileMetaInformation.Randomize(stringToReplace);
        //}

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Randomize_stringToReplace_random()
        {
            String stringToReplace = "@";
            Random random = new Random();

            fileMetaInformation.Randomize(stringToReplace, random);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void RemoveValueAt_tagSequence_zeroBasedIndex()
        {
            String tagSequence = "0x00100010";
            int zeroBasedIndex = 0;

            fileMetaInformation.RemoveValueAt(tagSequence, zeroBasedIndex);
        }

        /// <summary>
        ///     Testcase description...
        /// </summary>
        [Test]
        public void Set_dvtkDataTag_vR_parameters()
        {
            DvtkData.Dimse.Tag dvtkDataTag = new DvtkData.Dimse.Tag(0x02, 0x10);
            VR vR = VR.UN;

            fileMetaInformation.Set(dvtkDataTag, vR);
        }

        /// <summary>
        ///     Set a single Attribute.
        ///
        ///     Functionality: 
        ///     - If attributes exist that are in contradiction with the Attribute 
        ///       to set, remove them.
        ///     - If sequence items are missing, they are automatically added.
        ///     - The Attribute to set is always removed.
        ///     - The Attribute to set is added and the values are also added to the 
        ///       Attribute.
        /// </summary>
        [Test]
        public void Set_tagSequenceString_vR_parameters()
        {
            String tagSequenceString = "0x00020010";
            VR vR = VR.UN;

            fileMetaInformation.Set(tagSequenceString, vR);
        }
    }
}
