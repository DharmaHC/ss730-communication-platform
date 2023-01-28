Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports Telerik.WinControls
Imports System.Text
Imports System.Net
Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher
Imports System.ServiceModel.Channels



Public Class frmPdfRicevutaEsito

    Public stream As Stream
    Private Sub RadForm1_Load(sender As Object, e As EventArgs) Handles Me.Load

        RadPdfViewer1.ReadingMode = Telerik.WinControls.UI.ReadingMode.OnDemand
        Me.RadPdfViewer1.LoadDocument(stream)


    End Sub

End Class
