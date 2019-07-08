Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Xsl

Imports VR = DvtkData.Dimse.VR
Imports DimseCommand = DvtkData.Dimse.DimseCommand
Imports Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute
Imports DvtkHighLevelInterface
Imports DvtkHighLevelInterface.Dicom.Files
Imports DvtkHighLevelInterface.Dicom.Messages
Imports DvtkHighLevelInterface.Dicom.Other
Imports DvtkHighLevelInterface.Dicom.Threads
Imports DvtkHighLevelInterface.Common.Other
Imports DvtkHighLevelInterface.Common.Threads
Imports DvtkHighLevelInterface.Common.UserInterfaces
Imports DvtkHighLevelInterface.InformationModel



' Indicates if this Visual Basic Script will be executed under DVT.
#Const DVT_INTERPRETS_SCRIPT = False


'====================
Class MainDicomThread
    Inherits DicomThread

    Private theResultsDirectory As String

    Private theTable As Table = Nothing

    Private Const SESSION_INDEX As Integer = 1

    Private Const TEST_CASE_INDEX As Integer = 2

    Private Const PASSED_FAILED_INDEX As Integer = 3

    Private Const COMMENT_INDEX As Integer = 4


    Protected Overrides Sub Execute()

        Options.LogChildThreadsOverview = False

        Me.theTable = New Table(4)
        Me.theTable.CellItemSeperator = "<br>"
        Me.theTable.AddHeader("Session", "Test case", "P/F", "Comment")

        theResultsDirectory = Path.Combine(System.Windows.Forms.Application.StartupPath, "..\Results\DvtkValidation")

        Dim theInputFileFullName As String = Path.Combine(System.Windows.Forms.Application.StartupPath, "DvtkValidation.txt")

        If CheckInputFile(theInputFileFullName) Then
            Dim theFileInfo As FileInfo = New FileInfo(theInputFileFullName)

            Dim theStreamReader As StreamReader = theFileInfo.OpenText()

            Dim theInputLine As String

            Do
                theInputLine = theStreamReader.ReadLine()

                If Not (theInputLine = Nothing) Then
                    ExecuteLine(theInputLine)

                End If

            Loop While Not (theInputLine = Nothing)

            theStreamReader.Close()

        End If

        WriteHtmlInformation(Me.theTable.ConvertToHtml())

    End Sub

    Private Function CheckInputFile(ByVal theInputFileFullName As String) As Boolean
        Dim isSuccessfull As Boolean = True

        Dim theFileInfo As FileInfo = New FileInfo(theInputFileFullName)

        If (theFileInfo.Exists) Then
            Dim theStreamReader As StreamReader = theFileInfo.OpenText()

            Dim theInputLine As String

            Do
                theInputLine = theStreamReader.ReadLine()

                If Not (theInputLine = Nothing) Then
                    If Not (theInputLine.StartsWith("'")) Then
                        Dim theSplitInputLine As ArrayList = New ArrayList(theInputLine.Split("|"))

                        If (theSplitInputLine.Count = 8) Then

                        ElseIf (theSplitInputLine.Count = 16) Then

                        Else
                            MessageBox.Show(String.Format("Incorrect line: {0}", theInputLine))
                            isSuccessfull = False
                            Exit Do

                        End If

                    End If

                End If

            Loop While Not (theInputLine = Nothing)

            theStreamReader.Close()

        Else
            MessageBox.Show("Input file " + theInputFileFullName + " does not exist!")

            isSuccessfull = False

        End If

        Return (isSuccessfull)

    End Function


    Private Function CreateDicomScriptDicomThread(ByVal theSessionFullFileName As String, ByVal theScriptFileName As String) As DicomScriptDicomThread
        Dim theDicomScriptDicomThread As New DicomScriptDicomThread(theScriptFileName)

        theDicomScriptDicomThread.Initialize(Me)

        theDicomScriptDicomThread.Options.LoadFromFile(theSessionFullFileName)
        theDicomScriptDicomThread.Options.LogWaitingForCompletionChildThreads = False
        theDicomScriptDicomThread.Options.LogThreadStartingAndStoppingInParent = False
        theDicomScriptDicomThread.Options.ResultsDirectory = Me.theResultsDirectory
        theDicomScriptDicomThread.Options.Identifier = (Path.GetFileName(theSessionFullFileName) + "_" + theScriptFileName).Replace(".", "_")

        Return (theDicomScriptDicomThread)

    End Function

    Sub ValidateDicomScript(ByVal theSessionFullFileName As String, _
                            ByVal theScriptFileName As String, _
                            ByVal expectedNrOfGeneralErrors As Int32, _
                            ByVal expectedNrOfGeneralWarnings As Int32, _
                            ByVal expectedNrOfUserErrors As Int32, _
                            ByVal expectedNrOfUserWarnings As Int32, _
                            ByVal expectedNrOfValidationErrors As Int32, _
                            ByVal expectedNrOfValidationWarnings As Int32)

        Dim theDicomScriptDicomThread As DicomScriptDicomThread = CreateDicomScriptDicomThread(theSessionFullFileName, theScriptFileName)

        theDicomScriptDicomThread.Start()
        theDicomScriptDicomThread.WaitForCompletion()

        CheckErrorsAndWarnings(theDicomScriptDicomThread, theScriptFileName, expectedNrOfGeneralErrors, expectedNrOfGeneralWarnings, expectedNrOfUserErrors, expectedNrOfUserWarnings, expectedNrOfValidationErrors, expectedNrOfValidationWarnings)

        theDicomScriptDicomThread.Options.DvtkScriptSession.DefinitionManagement.UnLoadDefinitionFiles()

        Me.theTable.NewRow()
        Me.theTable.AddBlackItem(SESSION_INDEX, "-")
        Me.theTable.AddBlackItem(TEST_CASE_INDEX, "-")
        Me.theTable.AddBlackItem(PASSED_FAILED_INDEX, "-")
        Me.theTable.AddBlackItem(COMMENT_INDEX, "-")

    End Sub

    Sub ValidateDicomScriptPair(ByVal theSessionFullFileName1 As String, _
                                ByVal theScriptFileName1 As String, _
                                ByVal expectedNrOfGeneralErrors1 As Int32, _
                                ByVal expectedNrOfGeneralWarnings1 As Int32, _
                                ByVal expectedNrOfUserErrors1 As Int32, _
                                ByVal expectedNrOfUserWarnings1 As Int32, _
                                ByVal expectedNrOfValidationErrors1 As Int32, _
                                ByVal expectedNrOfValidationWarnings1 As Int32, _
                                ByVal theSessionFullFileName2 As String, _
                                ByVal theScriptFileName2 As String, _
                                ByVal expectedNrOfGeneralErrors2 As Int32, _
                                ByVal expectedNrOfGeneralWarnings2 As Int32, _
                                ByVal expectedNrOfUserErrors2 As Int32, _
                                ByVal expectedNrOfUserWarnings2 As Int32, _
                                ByVal expectedNrOfValidationErrors2 As Int32, _
                                ByVal expectedNrOfValidationWarnings2 As Int32)

        Dim theDicomScriptDicomThread1 As DicomScriptDicomThread = CreateDicomScriptDicomThread(theSessionFullFileName1, theScriptFileName1)
        Dim theDicomScriptDicomThread2 As DicomScriptDicomThread = CreateDicomScriptDicomThread(theSessionFullFileName2, theScriptFileName2)

        theDicomScriptDicomThread1.Start()
        theDicomScriptDicomThread2.Start(500)

        theDicomScriptDicomThread1.WaitForCompletion()
        theDicomScriptDicomThread2.WaitForCompletion()

        CheckErrorsAndWarnings(theDicomScriptDicomThread1, theScriptFileName1, expectedNrOfGeneralErrors1, expectedNrOfGeneralWarnings1, expectedNrOfUserErrors1, expectedNrOfUserWarnings1, expectedNrOfValidationErrors1, expectedNrOfValidationWarnings1)
        CheckErrorsAndWarnings(theDicomScriptDicomThread2, theScriptFileName2, expectedNrOfGeneralErrors2, expectedNrOfGeneralWarnings2, expectedNrOfUserErrors2, expectedNrOfUserWarnings2, expectedNrOfValidationErrors2, expectedNrOfValidationWarnings2)

        theDicomScriptDicomThread1.Options.DvtkScriptSession.DefinitionManagement.UnLoadDefinitionFiles()
        theDicomScriptDicomThread2.Options.DvtkScriptSession.DefinitionManagement.UnLoadDefinitionFiles()

        Me.theTable.NewRow()
        Me.theTable.AddBlackItem(SESSION_INDEX, "-")
        Me.theTable.AddBlackItem(TEST_CASE_INDEX, "-")
        Me.theTable.AddBlackItem(PASSED_FAILED_INDEX, "-")
        Me.theTable.AddBlackItem(COMMENT_INDEX, "-")

    End Sub

    Public Sub ExecuteLine(ByVal theInputLine As String)
        If Not (theInputLine.StartsWith("'")) Then
            Dim theSplitInputLine As ArrayList = New ArrayList(theInputLine.Split("|"))

            Dim theSessionFullFileName1 As String = CType(theSplitInputLine.Item(0), String)
            Dim theScriptFileName1 As String = CType(theSplitInputLine.Item(1), String)
            Dim expectedNrOfGeneralErrors1 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(2), String))
            Dim expectedNrOfGeneralWarnings1 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(3), String))
            Dim expectedNrOfUserErrors1 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(4), String))
            Dim expectedNrOfUserWarnings1 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(5), String))
            Dim expectedNrOfValidationErrors1 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(6), String))
            Dim expectedNrOfValidationWarnings1 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(7), String))

            If (theSplitInputLine.Count = 8) Then
                ValidateDicomScript(theSessionFullFileName1, _
                                    theScriptFileName1, _
                                    expectedNrOfGeneralErrors1, _
                                    expectedNrOfGeneralWarnings1, _
                                    expectedNrOfUserErrors1, _
                                    expectedNrOfUserWarnings1, _
                                    expectedNrOfValidationErrors1, _
                                    expectedNrOfValidationWarnings1)

                WriteInformation("Execute line: " + theInputLine)

            ElseIf (theSplitInputLine.Count = 16) Then
                Dim theSessionFullFileName2 As String = CType(theSplitInputLine.Item(8), String)
                Dim theScriptFileName2 As String = CType(theSplitInputLine.Item(9), String)
                Dim expectedNrOfGeneralErrors2 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(10), String))
                Dim expectedNrOfGeneralWarnings2 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(11), String))
                Dim expectedNrOfUserErrors2 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(12), String))
                Dim expectedNrOfUserWarnings2 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(13), String))
                Dim expectedNrOfValidationErrors2 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(14), String))
                Dim expectedNrOfValidationWarnings2 As Int32 = Convert.ToInt32(CType(theSplitInputLine.Item(15), String))

                ValidateDicomScriptPair(theSessionFullFileName1, _
                                        theScriptFileName1, _
                                        expectedNrOfGeneralErrors1, _
                                        expectedNrOfGeneralWarnings1, _
                                        expectedNrOfUserErrors1, _
                                        expectedNrOfUserWarnings1, _
                                        expectedNrOfValidationErrors1, _
                                        expectedNrOfValidationWarnings1, _
                                        theSessionFullFileName2, _
                                        theScriptFileName2, _
                                        expectedNrOfGeneralErrors2, _
                                        expectedNrOfGeneralWarnings2, _
                                        expectedNrOfUserErrors2, _
                                        expectedNrOfUserWarnings2, _
                                        expectedNrOfValidationErrors2, _
                                        expectedNrOfValidationWarnings2)

                WriteInformation("Execute line: " + theInputLine)

            End If

        End If

    End Sub

    Private Sub CheckErrorsAndWarnings(ByVal theDicomThread As DicomThread, ByVal theScriptFileName As String, ByVal expectedNrOfGeneralErrors As Int32, ByVal expectedNrOfGeneralWarnings As Int32, ByVal expectedNrOfUserErrors As Int32, ByVal expectedNrOfUserWarnings As Int32, ByVal expectedNrOfValidationErrors As Int32, ByVal expectedNrOfValidationWarnings As Int32)
        Dim different As Boolean = False
        Dim theRemarks As String = ""

        Dim NrOfErrGen As Int32 = Convert.ToInt32(theDicomThread.NrOfGeneralErrors)
        Dim NrOfWrnGen As Int32 = Convert.ToInt32(theDicomThread.NrOfGeneralWarnings)
        Dim NrOfErrUsr As Int32 = Convert.ToInt32(theDicomThread.NrOfUserErrors)
        Dim NrOfWrnUsr As Int32 = Convert.ToInt32(theDicomThread.NrOfUserWarnings)
        Dim NrOfErrVal As Int32 = Convert.ToInt32(theDicomThread.NrOfValidationErrors)
        Dim NrOfWrnVal As Int32 = Convert.ToInt32(theDicomThread.NrOfValidationWarnings)


        '
        ' Start with new row.
        '
        Me.theTable.NewRow()


        '
        ' Comment column.
        '
        If Not (NrOfErrGen = expectedNrOfGeneralErrors) Then
            Me.theTable.AddRedItem(COMMENT_INDEX, String.Format("Number of general errors differs (expected {0}, actual {1}).", expectedNrOfGeneralErrors, NrOfErrGen))
            different = True
        End If

        If Not (NrOfWrnGen = expectedNrOfGeneralWarnings) Then
            Me.theTable.AddRedItem(COMMENT_INDEX, String.Format("Number of general warnings differs (expected {0}, actual {1}).", expectedNrOfGeneralWarnings, NrOfWrnGen))
            different = True
        End If

        If Not (NrOfErrUsr = expectedNrOfUserErrors) Then
            Me.theTable.AddRedItem(COMMENT_INDEX, String.Format("Number of user errors differs (expected {0}, actual {1}).", expectedNrOfUserErrors, NrOfErrUsr))
            different = True
        End If

        If Not (NrOfWrnUsr = expectedNrOfUserWarnings) Then
            Me.theTable.AddRedItem(COMMENT_INDEX, String.Format("Number of user warnings differs (expected {0}, actual {1}).", expectedNrOfUserWarnings, NrOfWrnUsr))
            different = True
        End If

        If Not (NrOfErrVal = expectedNrOfValidationErrors) Then
            Me.theTable.AddRedItem(COMMENT_INDEX, String.Format("Number of validation errors differs (expected {0}, actual {1}).", expectedNrOfValidationErrors, NrOfErrVal))
            different = True
        End If

        If Not (NrOfWrnVal = expectedNrOfValidationWarnings) Then
            Me.theTable.AddRedItem(COMMENT_INDEX, String.Format("Number of validation warnings differs (expected {0}, actual {1}).", expectedNrOfValidationWarnings, NrOfWrnVal))
            different = True
        End If

        Me.theTable.AddBlackItem(COMMENT_INDEX, "<A HREF='" + theDicomThread.Options.DetailResultsFileNameOnly + "'>See detail" + "</A>" + " " + "<A HREF='" + theDicomThread.Options.SummaryResultsFileNameOnly + "'>See summary" + "</A>")


        '
        ' Session column.
        '
        Me.theTable.AddBlackItem(SESSION_INDEX, theDicomThread.Options.DvtkScriptSession.SessionFileName)


        '
        ' Test Case column.
        '
        Me.theTable.AddBlackItem(TEST_CASE_INDEX, theScriptFileName)


        '
        ' PASSED or FAILED column.
        '
        If (different) Then
            Me.theTable.AddRedItem(PASSED_FAILED_INDEX, "F")

        Else
            Me.theTable.AddBlackItem(PASSED_FAILED_INDEX, "P")

        End If

    End Sub

End Class ' MainDicomThread
'==========================


Class DicomScriptDicomThread
    Inherits DicomThread

    Private sessionFullFileName As String = ""

    Protected Overrides Sub Execute()
        Options.DvtkScriptSession.ExecuteScript(Me.sessionFullFileName, False)

    End Sub

    ' Hide default constructor.
    Private Sub New()
        ' Do nothing.

    End Sub

    Public Sub New(ByVal sessionFullFileName As String)
        Me.sessionFullFileName = sessionFullFileName

    End Sub


End Class


'
' Contains the entry point.
'
'================
Module DvtkScript

    '
    ' Entry point of this Visual Basic Script.
    '
    '-------
    Sub Main(ByVal CmdArgs() As String)
        Dvtk.Setup.Initialize()

        Dim theThreadManager As ThreadManager = New ThreadManager

        Dim theMainDicomThread As MainDicomThread = New MainDicomThread
        theMainDicomThread.Initialize(theThreadManager)

        ' Make sure a Results directory exists and make sure this path is set.
        Dim resultsFullDirectoryName As String = Path.GetFullPath(Path.Combine(System.Windows.Forms.Application.StartupPath, "..\Results\DvtkValidation"))

        If (Not Directory.Exists(resultsFullDirectoryName)) Then
            Directory.CreateDirectory(resultsFullDirectoryName)

        End If

        theMainDicomThread.Options.ResultsDirectory = resultsFullDirectoryName

        ' Set the identifier.
        theMainDicomThread.Options.Identifier = "Overview"

        ' This will take care that any activity logging will be displayed to the HliForm.
        Dim theHliForm As HliForm = New HliForm
        theHliForm.Attach(theMainDicomThread)

        ' Show the results when execution of the threads has finished.
        theMainDicomThread.Options.ShowResults = True

        ' Start the actual execution of theMainDicomThread.
        theMainDicomThread.Start()

        ' Wait until all threads have finished executing.
        theThreadManager.WaitForCompletionThreads()

        Dvtk.Setup.Terminate()

    End Sub 'Main

End Module ' DvtkScript
'======================
