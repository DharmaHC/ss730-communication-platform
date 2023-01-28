Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Public Class WaitingDialog
    Inherits Telerik.WinControls.UI.ShapedForm
    Private Shared _waitingThread As Thread
    Private Shared _waitingDialog As WaitingDialog

    Public Sub New(message As String)
        InitializeComponent()
        RadWaitingBarMain.StartWaiting()
        labelMessage.Text = message
    End Sub

    Public Overloads Shared Sub ShowDialog(message As String)
        'Show the form in a new thread  
        Dim threadStart As New ParameterizedThreadStart(AddressOf LaunchDialog)
        _waitingThread = New Thread(threadStart)
        _waitingThread.IsBackground = True
        _waitingThread.Start(message)
    End Sub

    Private Shared Sub LaunchDialog(message As Object)
        _waitingDialog = New WaitingDialog(message.ToString())

        'Create new message pump  
        Application.Run(_waitingDialog)
    End Sub

    Private Shared Sub CloseDialogDown()
        Application.ExitThread()
    End Sub

    Public Shared Sub CloseDialog()
        Try
            'Need to get the thread that launched the form, so  
            'we need to use invoke.  
            If _waitingDialog IsNot Nothing Then
                Try
                    Dim mi As New MethodInvoker(AddressOf CloseDialogDown)
                    _waitingDialog.Invoke(mi)
                Catch
                End Try
            End If

        Catch ex As Exception

        End Try
    End Sub

End Class
