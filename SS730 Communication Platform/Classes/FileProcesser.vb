Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Data
Imports SS730_Communication_Platform.SS730_CommunicationPlatform
Imports Telerik.WinControls.UI

Class FileProcesser
    Private Str As StreamReader = Nothing
    Private Line As String = Nothing
    Private DT As DataTable = Nothing
    Private DT_Storico As DataTable = Nothing
    Private db As String = Nothing
    Private m_file As String = Nothing

    ' Definizioni File di Import
    Dim lung_CodiceRegione As Integer = 3
    Dim lung_CodiceASL As Integer = 3
    Dim lung_CodiceSSA As Integer = 6
    Dim lung_cfProprietario As Integer = 16
    Dim lung_pIva As Integer = 11
    Dim lung_Dispositivo As Integer = 3
    Dim lung_numDocumento As Integer = 20
    Dim lung_dataEmissione As Integer = 10
    Dim lung_dataPagamento As Integer = 10
    Dim lung_flagPagamentoAnticipato As Integer = 1
    Dim lung_flagPagamentoTracciato As Integer = 1
    Dim lung_tipoDocumento As Integer = 1
    Dim lung_flagOpposizione As Integer = 1
    Dim lung_madreNotaCreditoNumero As Integer = 20
    Dim lung_madreNotaCreditoData As Integer = 10
    Dim lung_flagOperazione As Integer = 1
    Dim lung_cfCittadino As Integer = 16
    Dim lung_tipoSpesa As Integer = 2
    Dim lung_flagTipoSpesa As Integer = 1
    Dim lung_importo As Integer = 8

    Public RadTextBoxFattureDal As RadTextBox
    Public RadTextBoxFattureAl As RadTextBox
    Public RadTextBoxNumFattureDal As RadTextBox
    Public RadTextBoxNumFattureAl As RadTextBox
    Public RadTextBoxRecordImportati As RadTextBox
    Public RadTextBoxIdSessione As RadTextBox
    Public counter As Integer = 0
    Public numMinDocumento As String
    Public numMaxDocumento As String
    Public dataMinDocumento As DateTime
    Public dataMaxDocumento As DateTime

    Public Sub New()
        Str = New StreamReader(m_file)
        DT = DataBase.LoadDT(db)
        DT_Storico = DataBase.LoadDT_Storico(db)
    End Sub

    Public Sub New(File As String, DB__1 As String)
        Str = New StreamReader(File)
        db = DB__1
        DT = DataBase.LoadDT(db)
        DT_Storico = DataBase.LoadDT_Storico(db)
    End Sub

    Private uldimoIdStorico_Storico As Integer
    Private FirstDR As DataRow
    Private GlobalDR As DataRow
    Public Sub StartProcessing()
        uldimoIdStorico_Storico = DataBase.GetLastID_Storico(db)
        counter = 0
        While Not Str.EndOfStream
            counter += 1
            RadTextBoxRecordImportati.Text = counter
            Line = Str.ReadLine
            ProcessLine()
            If counter = 1 Then
                numMaxDocumento = GlobalDR("numDocumento")
                dataMaxDocumento = GlobalDR("dataEmissione")
                numMinDocumento = FirstDR("numDocumento")
                dataMinDocumento = FirstDR("dataEmissione")
            End If
        End While
        Me.RadTextBoxFattureDal.Text = dataMinDocumento.Date
        Me.RadTextBoxFattureAl.Text = dataMaxDocumento
        Me.RadTextBoxNumFattureDal.Text = numMinDocumento.Trim
        Me.RadTextBoxNumFattureAl.Text = numMaxDocumento
        Me.RadTextBoxIdSessione.Text = uldimoIdStorico_Storico + 1
    End Sub

    Public Sub StartProcessingStoricizza()
        counter = 0
        uldimoIdStorico_Storico = DataBase.GetLastID_Storico(db)

        While Not Str.EndOfStream
            counter += 1
            Line = Str.ReadLine()
            ProcessLine()
        End While
    End Sub

    Private Sub ProcessLine()
        Dim Position As Integer = 0
        'Position in the StreamReader Line
        Dim nuovoIdEstrazione As Integer = Convert.ToInt32(uldimoIdStorico_Storico + 1)
        Dim CycleCounter As Integer = 0
        Try
            While Position < Line.Length
                CycleCounter += 1
                Dim DR As DataRow = DT.NewRow()
                DR("idEstrazione") = nuovoIdEstrazione
                DR("codiceRegione") = Line.Substring(Position, lung_CodiceRegione)
                Position += lung_CodiceRegione
                DR("codiceASL") = Line.Substring(Position, lung_CodiceASL)
                Position += lung_CodiceASL
                DR("codiceSSA") = Line.Substring(Position, lung_CodiceSSA)
                Position += lung_CodiceSSA
                DR("cfProprietario") = Line.Substring(Position, lung_cfProprietario)
                Position += lung_cfProprietario
                DR("pIva") = Line.Substring(Position, lung_pIva)
                Position += lung_pIva
                DR("dispositivo") = Line.Substring(Position, lung_Dispositivo)
                Position += lung_Dispositivo
                DR("numDocumento") = Line.Substring(Position, lung_numDocumento)
                Position += lung_numDocumento
                Dim dataEmissioneString As String = Line.Substring(Position, lung_dataEmissione).Trim
                Dim dataEmissioneDate As DateTime = New DateTime(dataEmissioneString.Substring(0, 4), dataEmissioneString.Substring(4, 2), dataEmissioneString.Substring(6, 2))
                DR("dataEmissione") = Convert.ToDateTime(FormatDateTime(dataEmissioneDate, DateFormat.ShortDate))
                Position += lung_dataEmissione
                Dim dataPagamentoString As String = Line.Substring(Position, lung_dataPagamento).Trim
                If Not dataPagamentoString.Trim = String.Empty Then
                    Dim dataPagamentoDate As DateTime = New DateTime(dataPagamentoString.Substring(0, 4), dataPagamentoString.Substring(4, 2), dataPagamentoString.Substring(6, 2))
                    DR("dataPagamento") = Convert.ToDateTime(FormatDateTime(dataPagamentoDate, DateFormat.ShortDate))
                Else
                    MessageBox.Show("Errore:" & vbCrLf & vbCrLf & "dataPagamento mancante sul documento n. " & DR("numDocumento"))
                End If
                Position += lung_dataPagamento
                DR("flagPagamentoAnticipato") = IIf(Line.Substring(Position, lung_flagPagamentoAnticipato).Trim = String.Empty, DBNull.Value, True)
                Position += lung_flagPagamentoAnticipato
                DR("flagPagamentoTracciato") = IIf(Line.Substring(Position, lung_flagPagamentoTracciato) = "0", False, True)
                Position += lung_flagPagamentoTracciato
                DR("tipoDocumento") = Line.Substring(Position, lung_tipoDocumento)
                Position += lung_tipoDocumento
                DR("flagOpposizione") = IIf(Line.Substring(Position, lung_flagOpposizione) = "0", False, True)
                Position += lung_flagOpposizione
                DR("madreNotaCreditoNumero") = Line.Substring(Position, lung_madreNotaCreditoNumero)
                Position += lung_madreNotaCreditoNumero
                Dim madreNotaCreditoDataString As String = Line.Substring(Position, lung_madreNotaCreditoData).Trim
                If Not madreNotaCreditoDataString.Trim = String.Empty Then
                    Dim madreNotaCreditoDataDate As DateTime = New DateTime(madreNotaCreditoDataString.Substring(0, 4), madreNotaCreditoDataString.Substring(4, 2), madreNotaCreditoDataString.Substring(6, 2))
                    DR("madreNotaCreditoData") = Convert.ToDateTime(FormatDateTime(madreNotaCreditoDataDate, DateFormat.ShortDate))
                End If
                Position += lung_madreNotaCreditoData
                DR("flagOperazione") = Line.Substring(Position, lung_flagOperazione)
                Position += lung_flagOperazione
                DR("cfCittadino") = Line.Substring(Position, lung_cfCittadino)
                Position += lung_cfCittadino
                DR("tipoSpesa") = Line.Substring(Position, lung_tipoSpesa)
                Position += lung_tipoSpesa
                DR("flagTipoSpesa") = Line.Substring(Position, lung_flagTipoSpesa)
                Position += lung_flagTipoSpesa
                Dim importoString As String = FormatImportoFileImport(Line.Substring(Position, lung_importo))

                DR("importo") = importoString
                Position += lung_importo


                DT.Rows.Add(DR)
                If CycleCounter = 1 Then
                    FirstDR = DR
                End If
                GlobalDR = DR
            End While
        Catch ex As Exception
            MessageBox.Show("Errore:" & vbCrLf & vbCrLf & ex.ToString)
        End Try

    End Sub

    Private Function FormatImportoFileImport(nullable As String) As String
        Dim Lenght As Integer = 8
        Dim firstPart As String = nullable.Substring(0, 6)
        Dim decimalPart As String = nullable.Substring(6, 2)
        Dim Importo As String = Right((firstPart & "." & decimalPart).ToString, 7)
        Return Right(Importo, 7)
    End Function

    Public Sub UpDataDB()
        DataBase.UpDataDB(db, DT)
    End Sub

    Public Sub UpDataDBStorico()
        DataBase.UpDataDBStorico(db, DT)
    End Sub

    Public ReadOnly Property DTable() As DataTable
        Get
            Return DT
        End Get
    End Property

    Public Property File() As String
        Get
            Return m_file
        End Get
        Set(value As String)
            m_file = value
        End Set
    End Property

    Public Property DBase() As String
        Get
            Return db
        End Get
        Set(value As String)
            db = value
        End Set
    End Property
End Class

