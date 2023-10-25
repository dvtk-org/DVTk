'' Part of DvtkDicomWorklistMessageHandlers.dll - .NET class library providing basic data-classes for DVTK
'' Copyright © 2001-2006
'' Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

'Imports System
'Imports System.IO

'Imports VR = DvtkData.Dimse.VR
'Imports DvtkData.Dimse.DimseCommand
'Imports DvtkHighLevelInterface.Dicom.Messages
'Imports DvtkHighLevelInterface.Dicom.Other
'Imports DvtkHighLevelInterface.Dicom.Threads


'Namespace Dvtk.DvtkDicomEmulators.WorklistMessageHandlers

'    Public Class CFindRequiredHandler
'        Inherits MessageHandler

'        Public theSeqItemL1 As SequenceItem = New SequenceItem
'        Public theSeqItemL2 As SequenceItem = New SequenceItem
'        Public theSeqItemL3 As SequenceItem = New SequenceItem
'        Public theSeqItemL4 As SequenceItem = New SequenceItem
'        Public theSeqItemL5 As SequenceItem = New SequenceItem
'        Public theSeqItem0x00400100 As SequenceItem = New SequenceItem
'        Public theSeqItem0x00100101 As SequenceItem = New SequenceItem
'        Public theSeqItem0x00400008 As SequenceItem = New SequenceItem
'        Public theSeqItem0x00321064 As SequenceItem = New SequenceItem
'        Public theSeqItem0x00400440 As SequenceItem = New SequenceItem
'        Public theSeqItemCodeSeq As SequenceItem = New SequenceItem

'        ' Default is one match, so numberOfResponses is 2.
'        Public numberOfResponses As Integer = 2


'        Public Overrides Function HandleCFindRequest(ByVal receivedDicomMessage As DicomMessage) As Boolean

'            For index As Integer = 1 To numberOfResponses - 1
'                Dim cFindResponseMsg As DicomMessage = New DicomMessage(CFINDRSP)
'                SetRequired(receivedDicomMessage, cFindResponseMsg, index)
'                cFindResponseMsg.Set("0x00000900", VR.US, &HFF00)
'                Send(cFindResponseMsg)

'            Next

'            Dim cFindResponseSuccessMSG As DicomMessage = New DicomMessage(CFINDRSP)
'            cFindResponseSuccessMSG.Set("0x00000900", VR.US, 0)
'            Send(cFindResponseSuccessMSG)

'            Return True

'        End Function

'        Protected Sub SetRequired(ByVal cFindReq As DicomMessage, ByRef cFindResponse As DicomMessage, ByVal cFindResponseIndex As Integer)

'            Randomize() 'Initialize random-number generator
'            ' 0x0008
'            ' Dim ACCESSION_NR As Integer = CInt(Int((9999999999999998 * Rnd()) + 1)) ' Attribute 0x00080050 Accession Number
'            Const REF_PATIENT As String = "Referring Physician's Name" ' Attribute 0x00080090 Referring Physician's Name
'            ' 0x0010
'            Dim PATIENT As String = "Patient_" + cFindResponseIndex.ToString() + "-" '+ Context.Singleton.ScriptFileName ' Attribute 0x00100010 Patient's Name
'            Dim PATIENT_ID As String = "Patient_ID_" + cFindResponseIndex.ToString() + "-" '+ Context.Singleton.ScriptFileName               ' Attribute 0x00100020 Patient ID
'            Dim BIRTH As Date = Now.AddYears(-(CInt(Int((120 * Rnd()) + 1)))) 'Attribute 0x00100030 Patient's Birth Date and 0x00100032 Patient's Birth Time
'            Dim SEX() As String = {"M", "F", "O"}                                  ' Attribute 0x00100040 Patient's Sex
'            Dim PREGNANCY() As String = {"0001", "0002", "0003", "0004"}                     ' Attribute 0x001021C0 Pregnancy Status
'            ' 0x0020
'            Const STUDY_INSTANCE_UID As String = ""                      ' Attribute 0x0020000D Study Instance UID
'            ' 0x0040
'            Const REQUESTED_ID As String = ""                            ' Attribute 0x00401001 Requested Procedure ID 

'            ' Sequence 0x00400100 Scheduled Procedure Step Sequence
'            Const STATION_AE_TITLE As String = "DVT"                     ' Attribute 0x00400100/0x00400001 Scheduled Station AE Title
'            ' Const START_DATE As String = ""                             ' Attribute 0x00400100/0x00400002 Scheduled Procedure Step Start Date
'            Const Modality As String = "XA"                              ' Attribute 0x00400100/0x00080060 Modality
'            Const SCHED_PROC_ID As String = ""                           ' Attribute 0x00400100/0x00400009 Scheduled Procedure Step ID
'            ' Long Strings
'            Const LONG_STRING As String = "This is a long string of 64 characters needed for the long texts"
'            Const LONG_STRING1 As String = "Long string1 !#$%&'()*+,-./0123456789:;<=>? needed for long text"
'            Const LONG_STRING2 As String = "Long string2 @ABCDEFGHIJKLMNOPQRSTUVWXYZ[ ]^_need for long text "
'            Const LONG_STRING3 As String = " Long string3 `abcdefghijklmnopqrstuvwxyz{|}~ need for long text"

'            Dim MyValue As Integer = CInt(Int((1000000000 * Rnd()) + 1)) ' Generate random
'            Dim UID As Integer

'            Dim DateTimeNow As Date = Now
'            Dim DateNow As String = DateTimeNow.ToString("yyyyMMdd")
'            Dim TimeNow As String = DateTimeNow.ToString("hhmmss")

'            If (cFindReq.Exists("0x00400100")) Then
'                If (cFindReq.Exists("0x00400100[1]/0x00080060")) Then
'                    theSeqItem0x00400100.Set("0x00080060", VR.CS, "XA") ' Modality, M=R, R=1
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00321070")) Then
'                    theSeqItem0x00400100.Set("0x00321070", VR.LO) ' Requested Contrast Agent M=O, R=2C
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400001")) Then
'                    theSeqItem0x00400100.Set("0x00400001", VR.AE, STATION_AE_TITLE) ' Scheduled Station AE Title, M=R, R=1
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400002")) Then
'                    theSeqItem0x00400100.Set("0x00400002", VR.DA, DateNow) ' Scheduled Procedure Step Start Date, M=R, R=1
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400003")) Then
'                    theSeqItem0x00400100.Set("0x00400003", VR.TM, (CInt(TimeNow) + 10000).ToString()) ' Scheduled Procedure Step Start Time, M=R, R=1
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400006")) Then
'                    theSeqItem0x00400100.Set("0x00400006", VR.PN, "Scheduled^Performing^Physician^NR^1") ' Scheduled Performing Physician's Name, M=R, R=1
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400009")) Then
'                    theSeqItem0x00400100.Set("0x00400009", VR.SH, "SchedProcStepID1") ' Scheduled Procedure Step ID, M=O, R=1
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400010")) Then
'                    theSeqItem0x00400100.Set("0x00400010", VR.SH) ' Scheduled Station Name, M=O, R=2
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400011")) Then
'                    theSeqItem0x00400100.Set("0x00400011", VR.SH) ' Scheduled Procedure Step Location, M=0, R=2
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400012")) Then
'                    theSeqItem0x00400100.Set("0x00400012", VR.LO) ' Pre-Medication M=O, R=2C
'                End If
'                '
'                If theSeqItem0x00400100.Count.Equals(0) Then
'                    cFindResponse.Set("0x00400100", VR.SQ)
'                    WriteWarning("Empty Required Attribute 0x00400100")
'                Else
'                    cFindResponse.Set("0x00400100", VR.SQ, theSeqItem0x00400100)
'                End If
'            End If ' Scheduled Procedure Step Sequence, M=R, R=1
'            '
'            ' Requested Procedure Module
'            ' -----------------------------------------------
'            If (cFindReq.Exists("0x00401001")) Then
'                cFindResponse.Set("0x00401001", VR.SH, MyValue) ' Requested Procedure ID, M=O, R=1
'            End If
'            If (cFindReq.Exists("0x0020000D")) Then
'                Randomize()
'                UID = CInt(Int((9999998 * Rnd()) + 1))
'                cFindResponse.Set("0x0020000D", VR.UI, "1.3.46.670589.0.0.0.0." + UID.ToString) ' Study Instance UID, M=O, R=1
'            End If
'            ' Sequence has referenced SOP Instance UID and SOP Class UIS
'            If (cFindReq.Exists("0x00081110")) Then
'                theSeqItemL1 = New SequenceItem
'                If (cFindReq.Exists("0x00081110[1]/0x00081150")) Then
'                    theSeqItemL1.Set("0x00081150", VR.UI, "1.2.840.10008.5.1.4.1.1.9.1.2.1.1.1") ' Referenced SOP Instance UID, M=O, R=1C
'                End If
'                If (cFindReq.Exists("0x00081110[1]/0x00081155")) Then
'                    theSeqItemL1.Set("0x00081155", VR.UI, "1.2.840.10008.3.1.2.3.1") ' Referenced SOP Class UID (list of all values in def:"1.2.840.10008.3.1.2.3.1"), M=O, R=1C
'                End If ' Referenced Study Sequence, M=O, R=2
'                If theSeqItemL1.Count.Equals(0) Then
'                    cFindResponse.Set("0x00081110", VR.SQ)
'                Else
'                    cFindResponse.Set("0x00081110", VR.SQ, theSeqItemL1)
'                End If
'            End If ' Referenced Study Sequence, M=O, R=2
'            If (cFindReq.Exists("0x00401003")) Then
'                cFindResponse.Set("0x00401003", VR.SH) ' Requested Procedure ID, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00401004")) Then
'                cFindResponse.Set("0x00401004", VR.LO) ' Requested Procedure Description, M=O, R=2
'            End If
'            '
'            ' Imaging Service Request Module
'            ' -----------------------------------------------
'            If (cFindReq.Exists("0x00080050")) Then
'                cFindResponse.Set("0x00080050", VR.SH) ' Accession Number, M=O,R=2
'            End If
'            If (cFindReq.Exists("0x00080090")) Then
'                cFindResponse.Set("0x00080090", VR.PN) ' Referring Physician's Name, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00321032")) Then
'                cFindResponse.Set("0x00321032", VR.PN) ' Requested Physician, M=O, R=2
'            End If
'            '
'            ' Visit Identification Module
'            ' ------------------------------------------------
'            If (cFindReq.Exists("0x00380010")) Then
'                cFindResponse.Set("0x00380010", VR.LO) ' Admission ID, UID M=O, R=2
'            End If
'            '
'            ' Visit Status Module
'            ' ------------------------------------------------
'            If (cFindReq.Exists("0x00380300")) Then
'                cFindResponse.Set("0x00380300", VR.LO) ' Current Patient Location M=O, R=2
'            End If
'            '
'            ' Visit Relationship Module
'            ' ------------------------------------------------
'            If (cFindReq.Exists("0x00081120")) Then
'                theSeqItemL1 = New SequenceItem
'                If (cFindReq.Exists("0x00081120[1]/0x00081150")) Then
'                    theSeqItemL1.Set("0x00081150", VR.UI, "1.2.840.10008.3.1.2.1.1") ' Referenced SOP Instance UID, M=O, R=1C
'                End If
'                If (cFindReq.Exists("0x00081120[1]/0x00081155")) Then
'                    theSeqItemL1.Set("0x00081155", VR.LO, "Pre-Medication1") ' Referenced SOP Class UID (list of all values in def:"1.2.840.10008.3.1.2.1.1"), M=O, R=1C
'                End If
'                If theSeqItemL1.Count.Equals(0) Then
'                    cFindResponse.Set("0x00081120", VR.SQ)
'                Else
'                    cFindResponse.Set("0x00081120", VR.SQ, theSeqItemL1)
'                End If
'            End If ' Referenced Patient Sequence, M=O, R=2

'            '
'            ' Patient Identification Module
'            ' ----------------------------------------------
'            If (cFindReq.Exists("0x00100010")) Then
'                cFindResponse.Set("0x00100010", VR.PN, PATIENT) ' Patient's Name, M=R, R=1
'            End If
'            If (cFindReq.Exists("0x00100020")) Then
'                cFindResponse.Set("0x00100020", VR.LO, PATIENT_ID) ' Patient ID, M=R, R=1
'            End If
'            '
'            ' Patient Demographic Module
'            ' ----------------------------------------------
'            If (cFindReq.Exists("0x00100030")) Then
'                cFindResponse.Set("0x00100030", VR.DA) ' Patient's Birth Date, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00100040")) Then
'                cFindResponse.Set("0x00100040", VR.CS) ' Patient's Sex, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00100101")) Then
'                If (cFindReq.Exists("0x00100101[1]/0x00080100")) Then
'                    theSeqItem0x00100101.Set("0x00080100", VR.SH, "bjkjkkjlkj") ' Code Value, M=O, R=1
'                End If
'                If (cFindReq.Exists("0x00100101[1]/0x00080102")) Then
'                    theSeqItem0x00100101.Set("0x00080102", VR.SH, "llkjlk") ' Code Scheme Designator, M=O, R=1
'                End If
'                If (cFindReq.Exists("0x00100101[1]/0x00080104")) Then
'                    theSeqItem0x00100101.Set("0x00080104", VR.LO, "jhlkhlkjljk") ' Code Meaning, M=O, R=1
'                End If
'            End If
'            If (cFindReq.Exists("0x00100101")) Then
'                cFindResponse.Set("0x00100101", VR.SQ, theSeqItem0x00100101)
'            End If 'Patient’s Primary Language Code Sequence, M=O, R=3
'            If (cFindReq.Exists("0x000101030")) Then
'                cFindResponse.Set("0x00101030", VR.DS) ' Patient's Weight, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00403001")) Then
'                cFindResponse.Set("0x00403001", VR.LO) ' Confidentiality constraint on patient data, M=O, R=2
'            End If
'            '
'            ' Patient Medical Module
'            '-----------------------------------------------
'            If (cFindReq.Exists("0x00102000")) Then
'                cFindResponse.Set("0x00102000", VR.LO) ' Medical Alerts, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00102110")) Then
'                cFindResponse.Set("0x00102110", VR.LO) ' Contrast Allergies, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x001021C0")) Then
'                cFindResponse.Set("0x001021C0", VR.US) ' Pregnancy Status, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00380500")) Then
'                cFindResponse.Set("0x00380500", VR.LO) ' Patient State, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00380050")) Then
'                cFindResponse.Set("0x00380050", VR.LO) ' Special Needs, M=O, R=2
'            End If

'            Dim AttributeChoose As Integer = CInt(Int((7 * Rnd()) + 1)) ' Generate random
'            Select Case AttributeChoose
'                Case 1, 2, 3, 4, 7
'                    '
'                    ' Scheduled Procedure Step Module
'                    ' ---------------------------------------------------
'                    If (cFindReq.Exists("0x00400100")) Then
'                        If (cFindReq.Exists("0x00400100[1]/0x00400007")) Then
'                            theSeqItem0x00400100.Set("0x00400007", VR.LO) ' Scheduled Procedure Step Description, M=O, R=2
'                            cFindResponse.Set("0x00400100", VR.SQ, theSeqItem0x00400100) ' Scheduled Procedure Step Description, M=O, R=2
'                        Else
'                            If (cFindReq.Exists("0x00400100[1]/0x00400008")) Then
'                                If (cFindReq.Exists("0x00400100[1]/0x00400008[1]/0x00080100")) Then
'                                    theSeqItem0x00400008.Set("0x00080100", VR.SH, "SAICS CV1") ' Code Value, M=O, R=1C
'                                End If
'                                If (cFindReq.Exists("0x00400100[1]/0x00400008[1]/0x00080102")) Then
'                                    theSeqItem0x00400008.Set("0x00080102", VR.SH, "SAICS CSD1") ' Coding Scheme Designator, M=O, R=1C
'                                End If
'                                If theSeqItem0x00400008.Count.Equals(0) Then
'                                    theSeqItem0x00400100.Set("0x00400008", VR.SQ)
'                                    cFindResponse.Set("0x00400100", VR.SQ, theSeqItem0x00400100)
'                                Else
'                                    theSeqItem0x00400100.Set("0x00400008", VR.SQ, theSeqItem0x00400008)
'                                    cFindResponse.Set("0x00400100", VR.SQ, theSeqItem0x00400100)
'                                End If
'                            Else
'                                WriteWarning("Missing attribute following the rules Either 0x00400007 or 0x00400008 or Both shall be supported by SCP")
'                            End If ' Scheduled Protocol Code Sequence, M=O, R=1C
'                        End If
'                    End If
'                Case 3, 4, 5, 6, 8
'                    If (cFindReq.Exists("0x00400100")) Then
'                        If (cFindReq.Exists("0x00400100[1]/0x00400008")) Then
'                            If (cFindReq.Exists("0x00400100[1]/0x00400008[1]/0x00080100")) Then
'                                theSeqItem0x00400008.Set("0x00080100", VR.SH, "SAICS CV1") ' Code Value, M=O, R=1C
'                            End If
'                            If (cFindReq.Exists("0x00400100[1]/0x00400008[1]/0x00080102")) Then
'                                theSeqItem0x00400008.Set("0x00080102", VR.SH, "SAICS CSD1") ' Coding Scheme Designator, M=O, R=1C
'                            End If
'                            If theSeqItem0x00400008.Count.Equals(0) Then
'                                theSeqItem0x00400100.Set("0x00400008", VR.SQ)
'                                cFindResponse.Set("0x00400100", VR.SQ, theSeqItem0x00400100)
'                            Else
'                                theSeqItem0x00400100.Set("0x00400008", VR.SQ, theSeqItem0x00400008)
'                                cFindResponse.Set("0x00400100", VR.SQ, theSeqItem0x00400100)
'                            End If
'                        Else
'                            If (cFindReq.Exists("0x00400100[1]/0x00400007")) Then
'                                theSeqItem0x00400100.Set("0x00400007", VR.LO) ' Scheduled Procedure Step Description, M=O, R=2
'                                cFindResponse.Set("0x00400100", VR.SQ, theSeqItem0x00400100) ' Scheduled Procedure Step Description, M=O, R=2
'                            Else
'                                WriteWarning("Missing attribute following the rules Either 0x00400007 or 0x00400008 or Both shall be supported by SCP")
'                            End If
'                        End If ' Scheduled Protocol Code Sequence, M=O, R=1C
'                    End If
'                Case 1, 3, 5, 7, 8
'                    '
'                    ' Requested Procedure Module
'                    ' -----------------------------------------------
'                    If (cFindReq.Exists("0x00321060")) Then
'                        cFindResponse.Set("0x00321060", VR.LO, "LONG_STRING1") ' Requested Procedure Description, M=O, R=1C
'                    Else
'                        If (cFindReq.Exists("0x00321064")) Then
'                            If (cFindReq.Exists("0x00321064[1]/0x00080100")) Then
'                                theSeqItem0x00321064.Set("0x00080100", VR.SH, "RPCS CV1") 'Code Value, M=O, R=1C
'                            End If
'                            If (cFindReq.Exists("0x00321064[1]/0x00080102")) Then
'                                theSeqItem0x00321064.Set("0x00080102", VR.SH, "RPCS CSD1") 'Coding Scheme Designator, M=O, R=1C
'                            End If
'                            If theSeqItem0x00321064.Count.Equals(0) Then
'                                cFindResponse.Set("0x00321064", VR.SQ)
'                            Else
'                                cFindResponse.Set("0x00321064", VR.SQ, theSeqItem0x00321064)
'                            End If
'                        End If 'Requested Procedure Code Sequence M=O, R=1C
'                    End If
'                Case 2, 4, 6, 7, 8
'                    If (cFindReq.Exists("0x00321064")) Then
'                        If (cFindReq.Exists("0x00321064[1]/0x00080100")) Then
'                            theSeqItem0x00321064.Set("0x00080100", VR.SH, "RPCS CV1") 'Code Value, M=O, R=1C
'                        End If
'                        If (cFindReq.Exists("0x00321064[1]/0x00080102")) Then
'                            theSeqItem0x00321064.Set("0x00080102", VR.SH, "RPCS CSD1") 'Coding Scheme Designator, M=O, R=1C
'                        End If
'                        If theSeqItem0x00321064.Count.Equals(0) Then
'                            cFindResponse.Set("0x00321064", VR.SQ)
'                        Else
'                            cFindResponse.Set("0x00321064", VR.SQ, theSeqItem0x00321064)
'                        End If
'                    Else
'                        If (cFindReq.Exists("0x00321060")) Then
'                            cFindResponse.Set("0x00321060", VR.LO, "LONG_STRING1") ' Requested Procedure Description, M=O, R=1C
'                        Else
'                            WriteWarning("Missing attribute following the rules Either 0x00321060 or 0x00321064 or Both shall be supported by SCP")
'                        End If
'                    End If 'Requested Procedure Code Sequence M=O, R=1C
'            End Select

'        End Sub

'        Protected Sub Code_Sequence_Macro(ByVal cFindReq As DicomMessage, ByRef theSeqL2 As SequenceItem, ByVal AttributeSeq As String)

'            theSeqL2 = New SequenceItem
'            If (cFindReq.Exists(AttributeSeq)) Then
'                If (cFindReq.Exists(AttributeSeq + "/0x00080100")) Then
'                    theSeqL2.Set("0x00080100", VR.SH, "SAICS CV1")
'                End If
'                If (cFindReq.Exists(AttributeSeq + "/0x00080102")) Then
'                    theSeqL2.Set("0x00080102", VR.SH, "SAICS CSD1")
'                End If
'                If (cFindReq.Exists(AttributeSeq + "/0x00080103")) Then
'                    theSeqL2.Set("0x00080103", VR.SH, "SAICS CSV1")
'                End If
'                If (cFindReq.Exists(AttributeSeq + "/0x00080104")) Then
'                    theSeqL2.Set("0x00080104", VR.LO, "SAICS CM1")
'                End If
'            End If
'        End Sub

'        Protected Sub Patient_ID_Macro(ByVal cFindReq As DicomMessage, ByRef cFindResponse As DicomMessage, ByVal AttributeSeq As String, ByVal index As String)

'            Dim PATIENT As String = "Patient" + index + "-" '+ Context.Singleton.ScriptFileName                     ' Attribute 0x00100010 Patient's Name
'            If (cFindResponse(AttributeSeq).Values.Count.Equals(0)) Then
'                Dim theSeqL1 As SequenceItem = New SequenceItem
'                theSeqL1.Set( _
'                    "0x00401101", VR.SQ, theSeqItemCodeSeq, _
'                    "0x00401102", VR.ST, PATIENT, _
'                    "0x00401103", VR.LO, "0123456789", _
'                    "0x00080081", VR.ST, PATIENT _
'                    )
'                Dim AttributeChoose As Integer = CInt(Int((2 * Rnd()) + 1)) ' Generate random
'                Select Case AttributeChoose
'                    Case 1
'                        theSeqL1.Set("0x00080080", VR.LO, PATIENT)
'                    Case 2
'                        theSeqL1.Set("0x00080082", VR.SQ, theSeqItemCodeSeq)
'                End Select
'                cFindResponse.Set(AttributeSeq, VR.SQ, theSeqL1) ' Intended Recipients of Results Identification Sequence, M=O, R=3
'            Else
'                WriteWarning("Sequence " + AttributeSeq + "not tested with this testcase. The sequence was not empty in the request")
'            End If

'        End Sub

'    End Class

'End Namespace