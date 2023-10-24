' Part of DvtkDicomWorklistMessageHandlers.dll - .NET class library providing basic data-classes for DVTK
' Copyright © 2001-2006
' Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

'Imports System
'Imports System.IO

'Imports VR = DvtkData.Dimse.VR
'Imports DvtkData.Dimse.DimseCommand
'Imports DvtkHighLevelInterface.Dicom.Messages
'Imports DvtkHighLevelInterface.Dicom.Threads

'Namespace Dvtk.DvtkDicomEmulators.WorklistMessageHandlers

'    Public Class CFindExtendedHandler
'        Inherits CFindRequiredHandler

'        Public Overrides Function HandleCFindRequest(ByVal receivedDicomMessage As DicomMessage) As Boolean

'            For index As Integer = 1 To numberOfResponses - 1
'                Dim cFindResponseMsg As DicomMessage = New DicomMessage(CFINDRSP)
'                SetRequired(receivedDicomMessage, cFindResponseMsg, index)
'                SetExtended(receivedDicomMessage, cFindResponseMsg)
'                cFindResponseMsg.Set("0x00000900", VR.US, &HFF00)
'                Send(cFindResponseMsg)

'            Next

'            Dim cFindResponseSuccessMSG As DicomMessage = New DicomMessage(CFINDRSP)
'            cFindResponseSuccessMSG.Set("0x00000900", VR.US, 0)
'            Send(cFindResponseSuccessMSG)

'            Return True

'        End Function

'        Protected Sub SetExtended(ByVal cFindReq As DicomMessage, ByRef cFindResponse As DicomMessage)
'            Randomize() 'Initialize random-number generator
'            ' 0x0008
'            ' Dim ACCESSION_NR As Integer = CInt(Int((9999999999999998 * Rnd()) + 1)) ' Attribute 0x00080050 Accession Number
'            Const REF_PATIENT As String = "Referring Physician's Name" ' Attribute 0x00080090 Referring Physician's Name
'            ' 0x0010
'            Dim BIRTH As Date = Now.AddYears(-(CInt(Int((120 * Rnd()) + 1)))) 'Attribute 0x00100030 Patient's Birth Date and 0x00100032 Patient's Birth Time
'            Dim SEX() As String = {"M", "F", "O"}                                  ' Attribute 0x00100040 Patient's Sex
'            Dim PREGNANCY() As String = {"0001", "0002", "0003", "0004"}                     ' Attribute 0x001021C0 Pregnancy Status
'            ' Sequence 0x00400100 Scheduled Procedure Step Sequence
'            Const STATION_AE_TITLE As String = "DVT"                     ' Attribute 0x00400100/0x00400001 Scheduled Station AE Title
'            ' Const START_DATE As String = ""                             ' Attribute 0x00400100/0x00400002 Scheduled Procedure Step Start Date
'            ' Long Strings
'            Const LONG_STRING As String = "This is a long string of 64 characters needed for the long texts"
'            Const LONG_STRING1 As String = "Long string1 !#$%&'()*+,-./0123456789:;<=>? needed for long text"
'            Const LONG_STRING2 As String = "Long string2 @ABCDEFGHIJKLMNOPQRSTUVWXYZ[ ]^_need for long text "
'            Const LONG_STRING3 As String = " Long string3 `abcdefghijklmnopqrstuvwxyz{|}~ need for long text"

'            Dim DateTimeNow As Date = Now
'            Dim DateNow As String = DateTimeNow.ToString("yyyyMMdd")
'            Dim TimeNow As String = DateTimeNow.ToString("hhmmss")

'            '
'            ' Scheduled Procedure Step Module
'            ' ---------------------------------------------------
'            '
'            If (cFindReq.Exists("0x00400100")) Then
'                If (cFindReq.Exists("0x00400100[1]/0x00321070")) Then
'                    theSeqItem0x00400100.Set("0x00321070", VR.LO, LONG_STRING) ' Requested Contrast Agent M=O, R=2C
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400010")) Then
'                    theSeqItem0x00400100.Set("0x00400010", VR.SH, STATION_AE_TITLE) ' Scheduled Station Name, M=O, R=2
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400011")) Then
'                    theSeqItem0x00400100.Set("0x00400011", VR.SH, "jdakljg") ' Scheduled Procedure Step Location, M=0, R=2
'                End If
'                If (cFindReq.Exists("0x00400100[1]/0x00400012")) Then
'                    theSeqItem0x00400100.Set("0x00400012", VR.LO, LONG_STRING) ' Pre-Medication M=O, R=2C
'                End If
'                '
'                If Not theSeqItem0x00400100.Count.Equals(0) Then
'                    cFindResponse.Set("0x00400100", VR.SQ, theSeqItem0x00400100)
'                End If
'            End If ' Scheduled Procedure Step Sequence, M=R, R=1
'            '
'            ' Requested Procedure Module
'            ' -----------------------------------------------
'            If (cFindReq.Exists("0x00401003")) Then
'                cFindResponse.Set("0x00401003", VR.SH, "jadjjdga") ' Requested Procedure ID, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00401004")) Then
'                cFindResponse.Set("0x00401004", VR.LO, LONG_STRING) ' Requested Procedure Description, M=O, R=2
'            End If
'            '
'            ' Imaging Service Request Module
'            ' -----------------------------------------------
'            If (cFindReq.Exists("0x00080050")) Then
'                cFindResponse.Set("0x00080050", VR.SH, "jlgdsalj") ' Accession Number, M=O,R=2
'            End If
'            If (cFindReq.Exists("0x00080090")) Then
'                cFindResponse.Set("0x00080090", VR.PN, REF_PATIENT) ' Referring Physician's Name, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00321032")) Then
'                cFindResponse.Set("0x00321032", VR.PN, REF_PATIENT) ' Requested Physician, M=O, R=2
'            End If
'            '
'            ' Visit Identification Module
'            ' ------------------------------------------------
'            If (cFindReq.Exists("0x00380010")) Then
'                cFindResponse.Set("0x00380010", VR.LO, LONG_STRING) ' Admission ID, UID M=O, R=2
'            End If
'            '
'            ' Visit Status Module
'            ' ------------------------------------------------
'            If (cFindReq.Exists("0x00380300")) Then
'                cFindResponse.Set("0x00380300", VR.LO, LONG_STRING) ' Current Patient Location M=O, R=2
'            End If
'            '
'            ' Patient Demographic Module
'            ' ----------------------------------------------
'            If (cFindReq.Exists("0x00100030")) Then
'                cFindResponse.Set("0x00100030", VR.DA, BIRTH) ' Patient's Birth Date, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00100040")) Then
'                cFindResponse.Set("0x00100040", VR.CS, SEX(CInt(Int(3 * Rnd())))) ' Patient's Sex, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x000101030")) Then
'                cFindResponse.Set("0x00101030", VR.DS, "100") ' Patient's Weight, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00403001")) Then
'                cFindResponse.Set("0x00403001", VR.LO, LONG_STRING) ' Confidentiality constraint on patient data, M=O, R=2
'            End If
'            '
'            ' Patient Medical Module
'            '-----------------------------------------------
'            If (cFindReq.Exists("0x00102000")) Then
'                cFindResponse.Set("0x00102000", VR.LO, LONG_STRING) ' Medical Alerts, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00102110")) Then
'                cFindResponse.Set("0x00102110", VR.LO, LONG_STRING) ' Contrast Allergies, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x001021C0")) Then
'                cFindResponse.Set("0x001021C0", VR.US, PREGNANCY(CInt(Int(3 * Rnd())))) ' Pregnancy Status, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00380500")) Then
'                cFindResponse.Set("0x00380500", VR.LO, LONG_STRING) ' Patient State, M=O, R=2
'            End If
'            If (cFindReq.Exists("0x00380050")) Then
'                cFindResponse.Set("0x00380050", VR.LO, LONG_STRING) ' Special Needs, M=O, R=2
'            End If

'        End Sub

'    End Class

'End Namespace