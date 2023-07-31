Imports System.IO
Imports System.Threading

Module App
    Public Sub Main()
        AddHandler Application.ThreadException, AddressOf Application_ThreadException
        Application.EnableVisualStyles()
        Application.Run(New mainForm())

    End Sub

    Private Sub Application_ThreadException(sender As Object, e As ThreadExceptionEventArgs)
        ' Cattura l'errore e lo registra in un file di log o lo visualizza nella finestra di output del debug.
        ' Per esempio:
        Debug.WriteLine("Errore non gestito: " & e.Exception.Message)
        ' Oppure, se si desidera salvare il log in un file:
        Using writer As New StreamWriter("log.txt", True)
            writer.WriteLine("Errore non gestito: " & e.Exception.ToString())
        End Using
    End Sub
End Module
