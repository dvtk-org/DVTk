'' Part of DvtkDicomWorklistMessageHandlers.dll - .NET class library providing basic data-classes for DVTK
'' Copyright © 2001-2006
'' Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

'Imports System
'Imports System.IO

'Imports VR = DvtkData.Dimse.VR
'Imports DvtkData.Dimse.DimseCommand
'Imports DvtkHighLevelInterface.Dicom.Messages
'Imports DvtkHighLevelInterface.Dicom.Other


'Namespace Dvtk.DvtkDicomEmulators.WorklistMessageHandlers

'    Public Class CFindOptionalHandler
'        Inherits CFindExtendedHandler

'        Public Overrides Function HandleCFindRequest(ByVal receivedDicomMessage As DicomMessage) As Boolean

'            For index As Integer = 1 To numberOfResponses - 1
'                Dim cFindResponseMsg As DicomMessage = New DicomMessage(CFINDRSP)
'                SetRequired(receivedDicomMessage, cFindResponseMsg, index)
'                SetExtended(receivedDicomMessage, cFindResponseMsg)
'                SetOptional(receivedDicomMessage, cFindResponseMsg, index)
'                cFindResponseMsg.Set("0x00000900", VR.US, &HFF00)
'                Send(cFindResponseMsg)

'            Next

'            Dim cFindResponseSuccessMSG As DicomMessage = New DicomMessage(CFINDRSP)
'            cFindResponseSuccessMSG.Set("0x00000900", VR.US, 0)
'            Send(cFindResponseSuccessMSG)

'            Return True

'        End Function

'        Protected Sub SetOptional(ByVal cFindReq As DicomMessage, ByRef cFindResponse As DicomMessage, ByVal cFindResponseIndex As Integer)
'            Randomize() 'Initialize random-number generator
'            ' 0x0010
'            Dim PATIENT As String = "Patient" + cFindResponseIndex.ToString() + "-" ' + Context.Singleton.ScriptFileName                      ' Attribute 0x00100010 Patient's Name
'            Dim STATUS() As String = {"SCHEDULED", "ARRIVED"} ' Attribute 0x00400020 Scheduled Procedure Step Status
'            Dim VISITSTATUS() As String = {"CREATED", "SCHEDULED", "ADMITTED", "DISCHARGED"}
'            Dim SMOKINGSTATUS() As String = {"YES", "NO", "UNKNOWN"}
'            ' Long Strings
'            Const LONG_STRING As String = "This is a long string of 64 characters needed for the long texts"
'            Const LONG_STRING1 As String = "Long string1 !#$%&'()*+,-./0123456789:;<=>? needed for long text"
'            Const LONG_STRING2 As String = "Long string2 @ABCDEFGHIJKLMNOPQRSTUVWXYZ[ ]^_need for long text "
'            Const LONG_STRING3 As String = " Long string3 `abcdefghijklmnopqrstuvwxyz{|}~ need for long text"

'            Dim DateTimeNow As Date = Now
'            Dim DateNow As String = DateTimeNow.ToString("yyyyMMdd")
'            Dim TimeNow As String = DateTimeNow.ToString("hhmmss")

'            theSeqItemCodeSeq = New SequenceItem
'            theSeqItemCodeSeq.Set( _
'                "0x00080100", VR.SH, "SAICS CV1", _
'                "0x00080102", VR.SH, "SAICS CSD1", _
'                "0x00080103", VR.SH, "SAICS CSV1", _
'                "0x00080104", VR.LO, "SAICS CM1" _
'            )
'            '
'            ' Scheduled Procedure Step Module
'            ' ---------------------------------------------------
'            If (cFindResponse.Exists("0x00400100")) Then
'                If (cFindResponse.Exists("0x00400100[1]/0x00400008")) Then
'                    If (cFindReq.Exists("0x00400100[1]/0x00400008[1]/0x00080103")) Then
'                        theSeqItem0x00400008.Set("0x00080103", VR.SH, "SAICS CSV1") ' Coding Scheme Version, M=O, R=3
'                    End If
'                    If (cFindReq.Exists("0x00400100[1]/0x00400008[1]/0x00080104")) Then
'                        theSeqItem0x00400008.Set("0x00080104", VR.LO, "SAICS CM1") ' Code Meaning, M=O, R=3
'                    End If
'                    If (cFindResponse.Exists("0x00400100[1]/0x00400008[1]/0x00400440")) Then
'                        If (cFindResponse("0x00400100[1]/0x00400008[1]/0x00400440").Values.Count.Equals(0)) Then
'                            theSeqItem0x00400440.Set( _
'                            "0x0040A040", VR.CS, "TEXT", _
'                            "0x0040A043", VR.SQ, theSeqItemCodeSeq _
'                            )
'                            Select Case theSeqItem0x00400440("0x0040A040").Values(0)
'                                Case "DATETIME"
'                                    theSeqItem0x00400440.Set("0x0040A120", VR.DT, DateTimeNow.ToString("yyyyMMddhhmmss"))
'                                Case "PNAME"
'                                    theSeqItem0x00400440.Set("0x0040A123", VR.PN, PATIENT)
'                                Case "TEXT"
'                                    theSeqItem0x00400440.Set("0x0040160", VR.UT, LONG_STRING + LONG_STRING1 + LONG_STRING2 + LONG_STRING3)
'                                Case "CODE"
'                                    theSeqItem0x00400440.Set("0x0040A168", VR.SQ, theSeqItemCodeSeq)
'                                Case "NUMERIC"
'                                    theSeqItem0x00400440.Set("0x0040A30A", VR.DS, cFindResponseIndex.ToString())
'                                    theSeqItem0x00400440.Set("0x004008EA", VR.SQ, theSeqItemCodeSeq)
'                                Case "DATE"
'                                    theSeqItem0x00400440.Set("0x0040A121", VR.DA, DateNow)
'                                Case "TIME"
'                                    theSeqItem0x00400440.Set("0x0040A122", VR.TM, TimeNow)
'                                Case "UIDREF"
'                                    theSeqItem0x00400440.Set("0x0040124", VR.UI, cFindResponse("0x0020000D").Values)
'                            End Select
'                            theSeqItemL4 = New SequenceItem
'                            theSeqItemL4.Set( _
'                                "0x0040A040", VR.CS, "PATIENT", _
'                                "0x0040A043", VR.SQ, theSeqItemCodeSeq _
'                            )
'                            Select Case theSeqItemL4("0x0040A040").Values(0)
'                                Case "DATETIME"
'                                    theSeqItemL4.Set("0x0040A120", VR.DT, DateTimeNow.ToString("yyyyMMddhhmmss"))
'                                Case "PNAME"
'                                    theSeqItemL4.Set("0x0040A123", VR.PN, PATIENT)
'                                Case "TEXT"
'                                    theSeqItemL4.Set("0x0040160", VR.UT, LONG_STRING + LONG_STRING1 + LONG_STRING2 + LONG_STRING3)
'                                Case "CODE"
'                                    theSeqItemL4.Set("0x0040A168", VR.SQ, theSeqItemCodeSeq)
'                                Case "NUMERIC"
'                                    theSeqItemL4.Set("0x0040A30A", VR.DS, cFindResponseIndex.ToString())
'                                    theSeqItemL4.Set("0x004008EA", VR.SQ, theSeqItemCodeSeq)
'                                Case "DATE"
'                                    theSeqItemL4.Set("0x0040A121", VR.DA, DateNow)
'                                Case "TIME"
'                                    theSeqItemL4.Set("0x0040A122", VR.TM, TimeNow)
'                                Case "UIDREF"
'                                    theSeqItemL4.Set("0x0040124", VR.UI, cFindResponse("0x0020000D").Values)
'                            End Select
'                            theSeqItem0x00400440.Set("0x00400441", VR.SQ, theSeqItemL4)
'                            theSeqItem0x00400008.Set("0x00400440", VR.SQ, theSeqItem0x00400440)
'                            theSeqItem0x00400100.Set("0x00400008", VR.SQ, theSeqItem0x00400008)
'                        End If
'                    End If
'                    If (cFindReq.Exists("0x00400100[1]/0x00400020")) Then
'                        theSeqItem0x00400100.Set("0x00400020", VR.CS, STATUS(CInt(Int(2 * Rnd())))) ' Scheduled Procedure Step Status, M=O, R=3
'                    End If
'                    If (cFindReq.Exists("0x00400100[1]/0x00400004")) Then
'                        theSeqItem0x00400100.Set("0x00400004", VR.TM, (CInt(DateNow) + 1).ToString()) ' Scheduled Procedure Step End Date, M=O, R=3
'                    End If
'                    If (cFindReq.Exists("0x00400100[1]/0x00400005")) Then
'                        theSeqItem0x00400100.Set("0x00400005", VR.TM, (CInt(TimeNow) + 5).ToString()) ' Scheduled Procedure Step End Time, M=O, R=3
'                    End If
'                    If (cFindReq.Exists("0x00400100[1]/0x00400400")) Then
'                        theSeqItem0x00400100.Set("0x00400400", VR.LT, LONG_STRING + LONG_STRING1 + LONG_STRING2 + LONG_STRING3) ' Scheduled Procedure Step Status, M=O, R=3
'                    End If
'                    If (cFindReq.Exists("0x00400100[1]/0x0040000B")) Then
'                        theSeqItemL2 = New SequenceItem
'                        If (cFindReq.Exists("0x00400100[1]/0x0040000B[1]/0x00401101")) Then
'                            If (cFindResponse("0x00400100[1]/0x00400008[1]/0x00401101").Values.Count.Equals(0)) Then
'                                theSeqItemL2.Set("0x00401101", VR.SQ, theSeqItemCodeSeq)
'                            End If
'                        Else
'                            WriteWarning("Sequence 0x00400100[1]/0x00400008[1]/0x00401101[1] Personal Identification Code Sequence not tested with this testcase. The sequence was not empty in the request")
'                        End If
'                        If (cFindReq.Exists("0x00400100[1]/0x0040000B[1]/0x00401102")) Then
'                            theSeqItemL2.Set("0x00401102", VR.ST, PATIENT)
'                        End If
'                        If (cFindReq.Exists("0x00400100[1]/0x0040000B[1]/0x00401102")) Then
'                            theSeqItemL2.Set("0x00401103", VR.LO, "0123456789")
'                        End If
'                        If (cFindReq.Exists("0x00400100[1]/0x0040000B[1]/0x00401102")) Then
'                            theSeqItemL2.Set("0x00080081", VR.ST, PATIENT)
'                        End If
'                        Dim AttributeChoose As Integer = CInt(Int((2 * Rnd()) + 1)) ' Generate random
'                        Select Case AttributeChoose
'                            Case 1
'                                theSeqItemL2.Set("0x00080080", VR.LO, PATIENT)
'                            Case 2
'                                theSeqItemL3 = New SequenceItem
'                                If (cFindReq.Exists("0x00400100[1]/0x0040000B[1]/0x00080082")) Then
'                                    If (cFindResponse("0x00400100[1]/0x00400008[1]/0x00080082").Values.Count.Equals(0)) Then
'                                        theSeqItemL2.Set("0x0080082", VR.SQ, theSeqItemCodeSeq)
'                                    End If
'                                End If
'                        End Select
'                        If Not theSeqItemL2.Count.Equals(0) Then
'                            theSeqItem0x00400100.Set("0x0040000B", VR.SQ, theSeqItemL2) ' Scheduled Performing Physician Identification Sequence, M=O, R=3
'                        End If
'                    End If
'                    If Not theSeqItem0x00400100.Count.Equals(0) Then
'                        cFindResponse.Set("0x00400100", VR.SQ, theSeqItem0x00400100)
'                    End If
'                End If
'            Else
'                WriteInformation("Sequence 0x00400100[1]/0x00400008[1]/0x00400440[1] Protocol Context Sequence not tested with this testcase. The sequence was not empty in the request")
'            End If

'            '
'            ' Requested Procedure Module
'            ' -----------------------------------------------
'            If (cFindResponse.Exists("0x00321064")) Then
'                If (cFindReq.Exists("0x00321064[1]/0x00080103")) Then
'                    theSeqItem0x00321064.Set("0x00080103", VR.SH, "RPCS CSV1") 'Code Value, M=O, R=3
'                End If
'                If (cFindReq.Exists("0x00321064[1]/0x00080104")) Then
'                    theSeqItem0x00321064.Set("0x00080104", VR.SH, "RPCS CM1") 'Coding Scheme Designator, M=O, R=3
'                End If
'                If theSeqItem0x00321064.Count.Equals(0) Then
'                    cFindResponse.Set("0x00321064", VR.SQ, theSeqItem0x00321064)
'                End If
'            End If
'            If (cFindReq.Exists("0x00401002")) Then
'                cFindResponse.Set("0x00401002", VR.LO, LONG_STRING) ' Reason for the Requested Procedure, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00401400")) Then
'                cFindResponse.Set("0x00401400", VR.LT, LONG_STRING + LONG_STRING1 + LONG_STRING2 + LONG_STRING3) ' Reason for the Requested Procedure, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00401005")) Then
'                cFindResponse.Set("0x00401005", VR.LO, LONG_STRING) ' Requested Procedure Location, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00401008")) Then
'                cFindResponse.Set("0x00401008", VR.LO, LONG_STRING) ' Confidentiality Code, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00401009")) Then
'                cFindResponse.Set("0x00401009", VR.SH, LONG_STRING) ' Reporting Priority, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00401010")) Then
'                cFindResponse.Set("0x00401010", VR.PN, PATIENT) ' Names of Inteded Recipients of Results, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00401011")) Then
'                Patient_ID_Macro(cFindReq, cFindResponse, "0x00401011", cFindResponseIndex.ToString())
'            End If
'            '
'            ' Imaging Service Request Module
'            ' -----------------------------------------------
'            If (cFindReq.Exists("0x00402400")) Then
'                cFindResponse.Set("0x00402400", VR.SH, "1234567890123456") ' Imaging Service Request Comments, M=O,R=3
'            End If
'            If (cFindReq.Exists("0x00321031")) Then
'                Patient_ID_Macro(cFindReq, cFindResponse, "0x00321031", cFindResponseIndex.ToString())
'            End If
'            If (cFindReq.Exists("0x00080096")) Then
'                Patient_ID_Macro(cFindReq, cFindResponse, "0x00080096", cFindResponseIndex.ToString())
'            End If
'            If (cFindReq.Exists("0x00321033")) Then
'                cFindResponse.Set("0x00321033", VR.LO, "1234567890123456") ' Requesting Service, M=O,R=3
'            End If
'            If (cFindReq.Exists("0x00402004")) Then
'                cFindResponse.Set("0x00402004", VR.DA, DateNow) ' Issue Date of Imaging Service Request, M=O,R=3
'            End If
'            If (cFindReq.Exists("0x00402005")) Then
'                cFindResponse.Set("0x00402005", VR.TM, TimeNow) ' Issue Time of Imaging Service Request, M=O,R=3
'            End If
'            If (cFindReq.Exists("0x00402016")) Then
'                cFindResponse.Set("0x00402016", VR.LO, LONG_STRING) ' Placer Order Number/ Imaging Service Request, M=O,R=3
'            End If
'            If (cFindReq.Exists("0x00402017")) Then
'                cFindResponse.Set("0x00402017", VR.LO, LONG_STRING) ' Filler Order Number/ Imaging Service Request, M=O,R=3
'            End If
'            If (cFindReq.Exists("0x00402008")) Then
'                cFindResponse.Set("0x00402008", VR.PN, PATIENT) ' Order entered by, M=O,R=3
'            End If
'            If (cFindReq.Exists("0x00402009")) Then
'                cFindResponse.Set("0x00402009", VR.SH, "1234567890123456") ' Order Enterer's Location, M=O,R=3
'            End If
'            If (cFindReq.Exists("0x00402010")) Then
'                cFindResponse.Set("0x00402010", VR.SH, "1234567890123456") ' Order Callback Phone Number, M=O,R=3
'            End If
'            '
'            ' Visit Identification Module
'            ' ------------------------------------------------
'            If (cFindReq.Exists("0x00380011")) Then
'                cFindResponse.Set("0x00380011", VR.LO, LONG_STRING) ' Issuer of Admission ID M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00080081")) Then
'                cFindResponse.Set("0x00080081", VR.ST, "hjahlghlhkl") ' Institution Address M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00080080")) Then
'                cFindResponse.Set("0x00080080", VR.LO, PATIENT) ' Institution Name, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00080082")) Then
'                If (cFindResponse("0x00080082").Values.Count.Equals(0)) Then
'                    cFindResponse.Set("0x00080082", VR.SQ, theSeqItemCodeSeq) ' Patient's Primary Laguage Code Modifier Sequence
'                Else
'                    WriteWarning("Sequence 0x00080082 not tested with this testcase. The sequence was not empty in the request")
'                End If
'            End If        '
'            '
'            ' Visit Status Module
'            ' ------------------------------------------------
'            If (cFindReq.Exists("0x00380008")) Then
'                cFindResponse.Set("0x00380008", VR.CS, VISITSTATUS(CInt(Int(4 * Rnd())))) ' Visit Status ID, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00380400")) Then
'                cFindResponse.Set("0x00380400", VR.LO, "1.2.840.10008.3.1.2.1.1") ' Patient's Institution Residence, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00384000")) Then
'                cFindResponse.Set("0x00384000", VR.LT, "1.2.840.10008.3.1.2.1.1") ' Visit Comments, M=O, R=3
'            End If
'            '
'            ' Visit Admission Module
'            ' ------------------------------------------------
'            If (cFindReq.Exists("0x00080092")) Then
'                cFindResponse.Set("0x00080092", VR.ST, "adjlkgdaj") ' Referring Physician's Address, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00080094")) Then
'                cFindResponse.Set("0x00080094", VR.SH, "adjlgadj") ' Referring Physician's Telephone Numbers, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00081084")) Then
'                If (cFindResponse("0x00081084").Values.Count.Equals(0)) Then
'                    cFindResponse.Set("0x00081084", VR.SQ, theSeqItemCodeSeq) ' Patient's Primary Laguage Code Modifier Sequence
'                Else
'                    WriteWarning("Sequence 0x00081084 not tested with this testcase. The sequence was not empty in the request")
'                End If
'            End If        '
'            If (cFindReq.Exists("0x00081080")) Then
'                cFindResponse.Set("0x00081080", VR.LO, LONG_STRING) ' Admitting Diagnoses Description, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00380016")) Then
'                cFindResponse.Set("0x00380016", VR.LO, LONG_STRING) ' Route of Admissions, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00380020")) Then
'                cFindResponse.Set("0x00380020", VR.DA, DateNow) ' Admitting Date, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00380021")) Then
'                cFindResponse.Set("0x00380021", VR.TM, TimeNow) ' Admitting Time, M=O, R=3
'            End If

'            '
'            ' Patient Identification Module
'            ' ----------------------------------------------
'            If (cFindReq.Exists("0x00100021")) Then
'                cFindResponse.Set("0x00100021", VR.LO, LONG_STRING) ' Issuer of Patient ID, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00101000")) Then
'                cFindResponse.Set("0x00101000", VR.LO, LONG_STRING) ' Other Patient IDs, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00101001")) Then
'                cFindResponse.Set("0x00101001", VR.PN, PATIENT) ' Other Patient Names, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00101005")) Then
'                cFindResponse.Set("0x00101005", VR.PN, PATIENT) ' Patient's Birth Name, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00101060")) Then
'                cFindResponse.Set("0x00101060", VR.PN, PATIENT) ' Patient's Mother's Birth Name, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00101090")) Then
'                cFindResponse.Set("0x00101090", VR.LO, LONG_STRING) ' Medical Record Locator, M=O, R=3
'            End If
'            '
'            ' Patient Demographic Module
'            ' ----------------------------------------------
'            If (cFindReq.Exists("0x00101010")) Then
'                cFindResponse.Set("0x00101010", VR.AS, CInt(Int(120 * Rnd()))) ' Patient's Age, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00102180")) Then
'                cFindResponse.Set("0x00102180", VR.SH, "jagdjlgdaj") ' Occupation, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00100032")) Then
'                cFindResponse.Set("0x00100032", VR.TM, TimeNow) ' Patient's Birth Time, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x00100050")) Then
'                If (cFindResponse("0x00100050").Values.Count.Equals(0)) Then
'                    cFindResponse.Set("0x00100050", VR.SQ, theSeqItemCodeSeq) 'Patient's Isurance Plan Code, M=O, R=3
'                Else
'                    WriteWarning("Sequence 0x00100050 not tested with this testcase. The sequence was not empty in the request")
'                End If
'            End If
'            If (cFindReq.Exists("0x00100101")) Then
'                If (cFindResponse("0x00100101[1]/0x00100102").Values.Count.Equals(0)) Then
'                    theSeqItem0x00100101.Set("0x00100102", VR.SQ, theSeqItemCodeSeq) ' Patient's Primary Laguage Code Modifier Sequence
'                    cFindResponse.Set("0x00100101", VR.SQ, theSeqItem0x00100101) ' Patient's Primary Language Code Sequence, M=O, R=3
'                Else
'                    WriteWarning("Sequence 0x00100101[1]/0x00100102 not tested with this testcase. The sequence was not empty in the request")
'                End If
'            End If
'            If (cFindReq.Exists("0x00101020")) Then
'                cFindResponse.Set("0x00101020", VR.DS, "2.54") ' Patient's Size
'            End If
'            If (cFindReq.Exists("0x00101040")) Then
'                cFindResponse.Set("0x00101040", VR.LO, LONG_STRING)  ' Patient's Address
'            End If
'            If (cFindReq.Exists("0x00101080")) Then
'                cFindResponse.Set("0x00101080", VR.LO, LONG_STRING)  ' Military Rank
'            End If
'            If (cFindReq.Exists("0x00101081")) Then
'                cFindResponse.Set("0x00101081", VR.LO, LONG_STRING)  ' Branch of Service
'            End If
'            If (cFindReq.Exists("0x00102150")) Then
'                cFindResponse.Set("0x00102150", VR.LO, LONG_STRING)  ' Country of Residence
'            End If
'            If (cFindReq.Exists("0x00102152")) Then
'                cFindResponse.Set("0x00102152", VR.LO, LONG_STRING)  ' Region of Residence
'            End If
'            If (cFindReq.Exists("0x00102154")) Then
'                cFindResponse.Set("0x00102154", VR.SH, "hdsagjkhgj")  ' Patient's Telephone Numbers
'            End If
'            If (cFindReq.Exists("0x00102160")) Then
'                cFindResponse.Set("0x00102160", VR.SH, "jhdgslkjg")  ' Ethnic Group
'            End If
'            If (cFindReq.Exists("0x001021F0")) Then
'                cFindResponse.Set("0x001021F0", VR.LO, LONG_STRING)  ' Patient's Religious Preference
'            End If
'            If (cFindReq.Exists("0x00104000")) Then
'                cFindResponse.Set("0x00104000", VR.LT, LONG_STRING + LONG_STRING1 + LONG_STRING2 + LONG_STRING3)  ' Patient Comments
'            End If
'            '
'            ' Patient Medical Module
'            '-----------------------------------------------
'            If (cFindReq.Exists("0x001021A0")) Then
'                cFindResponse.Set("0x001021A0", VR.CS, SMOKINGSTATUS(CInt(Int(3 * Rnd())))) ' Smoking Status, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x001021B0")) Then
'                cFindResponse.Set("0x0021B0", VR.LT, LONG_STRING + LONG_STRING1 + LONG_STRING2 + LONG_STRING3) ' Additional Patient History, M=O, R=3
'            End If
'            If (cFindReq.Exists("0x001021D0")) Then
'                cFindResponse.Set("0x001021D0", VR.DA, DateNow) ' Last Menstrual Date, M=O, R=3
'            End If

'        End Sub

'    End Class

'End Namespace