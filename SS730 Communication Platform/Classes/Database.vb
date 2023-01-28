
Imports System.Collections.Generic
Imports System.Text
Imports System.Data
Imports System.Data.OleDb

Namespace SS730_CommunicationPlatform
    Class DataBase
        Public Shared Function LoadDT(DB As String) As DataTable
            'CheckTables(DB)
            Dim DT As New DataTable()
            'Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";User Id=admin;Password=;"
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
            Dim SQL As String = "SELECT * FROM zTableInvio_SS WHERE ID=0"

            Dim OleConn As New OleDbConnection(ConnString)
            Dim OleAdp As New OleDbDataAdapter(SQL, OleConn)
            OleConn.Open()
            OleAdp.Fill(DT)
            OleConn.Close()
            Return DT
        End Function

        Public Shared Function LoadDT_Storico(DB As String) As DataTable
            'CheckTables(DB)
            Dim DT As New DataTable()
            'Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";User Id=admin;Password=;"
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
            Dim SQL As String = "SELECT * FROM zTableInvio_SS_Storico WHERE ID=0"

            Dim OleConn As New OleDbConnection(ConnString)
            Dim OleAdp As New OleDbDataAdapter(SQL, OleConn)
            OleConn.Open()
            OleAdp.Fill(DT)
            OleConn.Close()
            Return DT
        End Function

        Public Shared Function LoadDT_Sessioni(DB As String) As DataTable
            'CheckTables(DB)
            Dim DT As New DataTable()
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
            Dim SQL As String = "SELECT Sessioni.* FROM Sessioni"

            Dim OleConn As New OleDbConnection(ConnString)
            Dim OleAdp As New OleDbDataAdapter(SQL, OleConn)
            OleConn.Open()
            OleAdp.Fill(DT)
            OleConn.Close()
            Return DT
        End Function

        Public Shared Function LoadDT_SessioniById(DB As String, IdSessione As Integer) As DataTable
            'CheckTables(DB)
            Dim DT As New DataTable()
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
            Dim SQL As String = "SELECT Sessioni.* FROM Sessioni WHERE idEstrazione = " & IdSessione

            Dim OleConn As New OleDbConnection(ConnString)
            Dim OleAdp As New OleDbDataAdapter(SQL, OleConn)
            OleConn.Open()
            OleAdp.Fill(DT)
            OleConn.Close()
            Return DT
        End Function

        Public Shared Function LoadDT_Settings(DB As String) As DataTable
            'CheckTables(DB)
            Dim DT As New DataTable()
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
            Dim SQL As String = "SELECT Top 1 Settings.* FROM Settings"

            Dim OleConn As New OleDbConnection(ConnString)
            Dim OleAdp As New OleDbDataAdapter(SQL, OleConn)
            OleConn.Open()
            OleAdp.Fill(DT)
            OleConn.Close()
            Return DT
        End Function

        Public Shared Function GetLastID_Storico(DB As String) As Integer
            Dim DT As New DataTable()
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
            Dim SQL As String = "SELECT Top 1 zTableInvio_SS_Storico.idEstrazione FROM zTableInvio_SS_Storico ORDER BY zTableInvio_SS_Storico.ID DESC"

            Dim OleConn As New OleDbConnection(ConnString)
            Dim OleAdp As New OleDbDataAdapter(SQL, OleConn)
            OleConn.Open()
            OleAdp.Fill(DT)
            OleConn.Close()
            If DT.Rows.Count = 0 Then
                Return 0
            Else
                Return DT.Rows(0).Item(0)
            End If

        End Function

        Public Shared Sub UpDataDB(DB As String, DT As DataTable)
            Try
                Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
                Dim SQL As String = "SELECT * FROM zTableInvio_SS WHERE ID=0"
                Dim INSERT As String = "INSERT INTO zTableInvio_SS(idEstrazione, pIva, dataEmissione, dispositivo, numDocumento, dataPagamento, flagPagamentoAnticipato, flagPagamentoTracciato, tipoDocumento, flagOpposizione, flagOperazione, cfCittadino, tipoSpesa, flagTipoSpesa, importo, codiceRegione, codiceAsl, codiceSSA, cfProprietario, madreNotaCreditoNumero, madreNotaCreditoData) " &
                                   "VALUES (@idEstrazione, @pIva, @dataEmissione, @dispositivo, @numDocumento, @dataPagamento, @flagPagamentoAnticipato, @flagPagamentoTracciato, @tipoDocumento, @flagOpposizione, @flagOperazione, @cfCittadino, @tipoSpesa, @flagTipoSpesa, @importo, @codiceRegione, @codiceAsl, @codiceSSA, @cfProprietario, @madreNotaCreditoNumero, @madreNotaCreditoData)"

                Dim OleConn As New OleDbConnection(ConnString)
                Dim OleAdp As New OleDbDataAdapter(SQL, OleConn)
                OleAdp.InsertCommand = New OleDbCommand(INSERT)
                OleAdp.InsertCommand.Parameters.Add("@idEstrazione", OleDbType.Integer, 4, "idEstrazione")
                OleAdp.InsertCommand.Parameters.Add("@pIva", OleDbType.VarChar, 11, "pIva")
                OleAdp.InsertCommand.Parameters.Add("@dataEmissione", OleDbType.DBDate, 10, "dataEmissione")
                OleAdp.InsertCommand.Parameters.Add("@dispositivo", OleDbType.VarChar, 3, "dispositivo")
                OleAdp.InsertCommand.Parameters.Add("@numDocumento", OleDbType.VarChar, 20, "numDocumento")
                OleAdp.InsertCommand.Parameters.Add("@dataPagamento", OleDbType.DBDate, 10, "dataPagamento")
                OleAdp.InsertCommand.Parameters.Add("@flagPagamentoAnticipato", OleDbType.Boolean, 1, "flagPagamentoAnticipato")
                OleAdp.InsertCommand.Parameters.Add("@flagPagamentoTracciato", OleDbType.Boolean, 1, "flagPagamentoTracciato")
                OleAdp.InsertCommand.Parameters.Add("@tipoDocumento", OleDbType.VarChar, 1, "tipoDocumento")
                OleAdp.InsertCommand.Parameters.Add("@flagOpposizione", OleDbType.Boolean, 1, "flagOpposizione")
                OleAdp.InsertCommand.Parameters.Add("@flagOperazione", OleDbType.VarChar, 1, "flagOperazione")
                OleAdp.InsertCommand.Parameters.Add("@cfCittadino", OleDbType.VarChar, 16, "cfCittadino")
                OleAdp.InsertCommand.Parameters.Add("@tipoSpesa", OleDbType.VarChar, 2, "tipoSpesa")
                OleAdp.InsertCommand.Parameters.Add("@flagTipoSpesa", OleDbType.VarChar, 1, "flagTipoSpesa")
                OleAdp.InsertCommand.Parameters.Add("@importo", OleDbType.VarChar, 7, "importo")
                OleAdp.InsertCommand.Parameters.Add("@codiceRegione", OleDbType.VarChar, 3, "codiceRegione")
                OleAdp.InsertCommand.Parameters.Add("@codiceAsl", OleDbType.VarChar, 3, "codiceAsl")
                OleAdp.InsertCommand.Parameters.Add("@codiceSSA", OleDbType.VarChar, 3, "codiceSSA")
                OleAdp.InsertCommand.Parameters.Add("@cfProprietario", OleDbType.VarChar, 16, "cfProprietario")
                OleAdp.InsertCommand.Parameters.Add("@madreNotaCreditoNumero", OleDbType.VarChar, 20, "madreNotaCreditoNumero")
                OleAdp.InsertCommand.Parameters.Add("@madreNotaCreditoData", OleDbType.DBDate, 10, "madreNotaCreditoData")

                OleAdp.InsertCommand.Connection = OleConn
                OleAdp.InsertCommand.Connection.Open()
                OleAdp.Update(DT)
                OleAdp.InsertCommand.Connection.Close()
            Catch oex As OleDbException
                MessageBox.Show("Errore:" & vbCrLf & vbCrLf & oex.ToString)
            Catch ex As Exception
                MessageBox.Show("Errore:" & vbCrLf & vbCrLf & ex.ToString)
            End Try

        End Sub


        Public Shared Sub UpDataDBStorico(DB As String, DT As DataTable)
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
            Dim SQL As String = "SELECT * FROM zTableInvio_SS_Storico WHERE ID=0"
            Dim INSERT As String = "INSERT INTO zTableInvio_SS_Storico(idEstrazione, pIva, dataEmissione, dispositivo, numDocumento, dataPagamento, flagPagamentoAnticipato, flagPagamentoTracciato, tipoDocumento, flagOpposizione, flagOperazione, cfCittadino, tipoSpesa, flagTipoSpesa, importo, codiceRegione, codiceAsl, codiceSSA, cfProprietario, madreNotaCreditoNumero, madreNotaCreditoData) " &
                                   "VALUES (@idEstrazione, @pIva, @dataEmissione, @dispositivo, @numDocumento, @dataPagamento, @flagPagamentoAnticipato, @flagPagamentoTracciato, @tipoDocumento, @flagOpposizione, @flagOperazione, @cfCittadino, @tipoSpesa, @flagTipoSpesa, @importo, @codiceRegione, @codiceAsl, @codiceSSA, @cfProprietario, @madreNotaCreditoNumero, @madreNotaCreditoData)"

            Dim OleConn As New OleDbConnection(ConnString)
            Dim OleAdp As New OleDbDataAdapter(SQL, OleConn)
            Try
                OleAdp.InsertCommand = New OleDbCommand(INSERT)
                OleAdp.InsertCommand.Parameters.Add("@idEstrazione", OleDbType.Integer, 4, "idEstrazione")
                OleAdp.InsertCommand.Parameters.Add("@pIva", OleDbType.VarChar, 11, "pIva")
                OleAdp.InsertCommand.Parameters.Add("@dataEmissione", OleDbType.DBDate, 10, "dataEmissione")
                OleAdp.InsertCommand.Parameters.Add("@dispositivo", OleDbType.VarChar, 3, "dispositivo")
                OleAdp.InsertCommand.Parameters.Add("@numDocumento", OleDbType.VarChar, 20, "numDocumento")
                OleAdp.InsertCommand.Parameters.Add("@dataPagamento", OleDbType.DBDate, 10, "dataPagamento")
                OleAdp.InsertCommand.Parameters.Add("@flagPagamentoAnticipato", OleDbType.Boolean, 1, "flagPagamentoAnticipato")
                OleAdp.InsertCommand.Parameters.Add("@flagPagamentoTracciato", OleDbType.Boolean, 1, "flagPagamentoTracciato")
                OleAdp.InsertCommand.Parameters.Add("@tipoDocumento", OleDbType.VarChar, 1, "tipoDocumento")
                OleAdp.InsertCommand.Parameters.Add("@flagOpposizione", OleDbType.Boolean, 1, "flagOpposizione")
                OleAdp.InsertCommand.Parameters.Add("@flagOperazione", OleDbType.VarChar, 1, "flagOperazione")
                OleAdp.InsertCommand.Parameters.Add("@cfCittadino", OleDbType.VarChar, 16, "cfCittadino")
                OleAdp.InsertCommand.Parameters.Add("@tipoSpesa", OleDbType.VarChar, 2, "tipoSpesa")
                OleAdp.InsertCommand.Parameters.Add("@flagTipoSpesa", OleDbType.VarChar, 1, "flagTipoSpesa")
                OleAdp.InsertCommand.Parameters.Add("@importo", OleDbType.VarChar, 7, "importo")
                OleAdp.InsertCommand.Parameters.Add("@codiceRegione", OleDbType.VarChar, 3, "codiceRegione")
                OleAdp.InsertCommand.Parameters.Add("@codiceAsl", OleDbType.VarChar, 3, "codiceAsl")
                OleAdp.InsertCommand.Parameters.Add("@codiceSSA", OleDbType.VarChar, 3, "codiceSSA")
                OleAdp.InsertCommand.Parameters.Add("@cfProprietario", OleDbType.VarChar, 16, "cfProprietario")
                OleAdp.InsertCommand.Parameters.Add("@madreNotaCreditoNumero", OleDbType.VarChar, 20, "madreNotaCreditoNumero")
                OleAdp.InsertCommand.Parameters.Add("@madreNotaCreditoData", OleDbType.DBDate, 10, "madreNotaCreditoData")

                OleAdp.InsertCommand.Connection = OleConn
                OleAdp.InsertCommand.Connection.Open()
                OleAdp.Update(DT)
            Catch ex As Exception
                MessageBox.Show("Errore:" & vbCrLf & vbCrLf & ex.ToString)
            End Try
            OleAdp.InsertCommand.Connection.Close()
        End Sub

        Private Shared Sub CheckTables(DB As String)
            Dim DT As DataTable = Nothing
            Dim T_F As Boolean = False
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"

            Dim OleConn As New OleDbConnection(ConnString)
            OleConn.Open()
            DT = OleConn.GetSchema("Tables")
            OleConn.Close()

            For i As Integer = 0 To DT.Rows.Count - 1
                If DT.Rows(i)(2).ToString() = "zTableInvioSS" Then
                    T_F = True
                End If
            Next

            If Not T_F Then
                MessageBox.Show("Tabella dati non trovata in db Access")
                Exit Sub
                CreateTable(DB)
            End If
        End Sub

        Private Shared Sub CreateTable(DB As String)
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";User Id=admin;Password=;"
            Dim SQL As String = "CREATE TABLE MeterReadings " + "(ReadingID AUTOINCREMENT CONSTRAINT PK_MeterReadings PRIMARY KEY," + " Meter1 INTEGER," + " Meter2 INTEGER," + " Meter3 INTEGER," + " Meter4 INTEGER)"

            Dim OleConn As New OleDbConnection(ConnString)
            Dim OleComm As New OleDbCommand(SQL, OleConn)
            OleComm.Connection.Open()
            OleComm.ExecuteNonQuery()
            OleComm.Connection.Close()
        End Sub


        Public Shared Function GetSettings(DB As String) As DataRow
            Dim DT As New DataTable()
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
            Dim SQL As String = "SELECT Top 1 Settings.* FROM Settings"

            Dim OleConn As New OleDbConnection(ConnString)
            Dim OleAdp As New OleDbDataAdapter(SQL, OleConn)
            OleConn.Open()
            OleAdp.Fill(DT)
            OleConn.Close()
            If DT.Rows.Count = 0 Then
                Return Nothing
            Else
                Return DT.Rows(0)
            End If

        End Function
    End Class
End Namespace

