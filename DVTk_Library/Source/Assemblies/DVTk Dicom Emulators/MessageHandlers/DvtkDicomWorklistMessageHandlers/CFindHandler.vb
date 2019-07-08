' Part of DvtkDicomWorklistMessageHandlers.dll - .NET class library providing basic data-classes for DVTK
' Copyright © 2001-2006
' Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

Imports System.IO

Imports DvtkData.Dimse.DimseCommand
Imports DvtkHighLevelInterface.Dicom.Messages
Imports DvtkHighLevelInterface.Dicom.Threads
Imports DvtkHighLevelInterface.InformationModel

Namespace Dvtk.DvtkDicomEmulators.WorklistMessageHandlers

    Public Class CFindHandler
        Inherits MessageHandler

        Private _modalityWorklistInformationModel As ModalityWorklistInformationModel = New ModalityWorklistInformationModel

        Public Sub New(ByVal modalityWorklistInformationModel As ModalityWorklistInformationModel)
            _modalityWorklistInformationModel = modalityWorklistInformationModel
        End Sub

        Public Overrides Function HandleCFindRequest(ByVal queryMessage As DicomMessage) As Boolean

            Dim responseMessages As DicomMessageCollection
            responseMessages = _modalityWorklistInformationModel.QueryInformationModel(queryMessage)

            Dim responseMessage As DicomMessage
            For Each responseMessage In responseMessages
                Send(responseMessage)
            Next

            Return True

        End Function

    End Class

End Namespace
