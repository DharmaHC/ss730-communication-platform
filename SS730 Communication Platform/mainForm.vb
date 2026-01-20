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
Imports Telerik.WinControls.UI
Imports System.Data.OleDb
Imports SS730_Communication_Platform
Imports SS730_Communication_Platform.SS730_CommunicationPlatform
Imports System.Xml.Schema
Imports System.Xml


Public Class mainForm


    Private File As String = Nothing
    Private DB As String = Nothing
    Private FP As FileProcesser = Nothing

    Private WebServiceUser As String
    Private WebServicePassword As String
    Private WebServicePincode As String
    Private SessioniList As New List(Of RigaSessione)
    Private currentSessione As New RigaSessione
    Private isXMLvalid As Boolean


    Private Sub mainForm_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        Try
            Me.StartPosition = FormStartPosition.CenterScreen

        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadForm1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            RadButtonSfoglia_FileASCII.Enabled = True
            RadButtonCaricaFileASCII.Enabled = False
            RadButtonGeneraXML.Enabled = False
            DB = Application.StartupPath & "\TS730_Data.mdb"
            Me.RadPageViewMainForm.SelectedPage = Me.RadPageViewSettingsPage

            RadDropDownListReadSettingsFrom.SelectedIndex = 0
            Me.RadPageViewComunicazioniPage.Enabled = False
            Me.RadPageViewPreparazioniPage.Enabled = False

            RadGridViewElencoSessioni.AutoGenerateColumns = False

            generateGridViewSessioni_Columns()

            ' Clear zTableInvio_SS
            deleteFromZtable()

            loadVecchieSessioni()

            RadLabelStruttura.Text = My.Settings.Struttura
            RadLabelStruttura1.Text = My.Settings.Struttura
            RadLabelStruttura2.Text = My.Settings.Struttura
            Dim custColor = Color.FromArgb(My.Settings.Color2.Substring(0, 3), My.Settings.Color2.Substring(5, 3), My.Settings.Color2.Substring(10, 3))

            RadPageViewSettingsPage.BackColor = custColor
        Catch ex As Exception
            ' Cattura l'errore e lo registra in un file di log o lo visualizza nella finestra di output del debug.
            ' Per esempio:
            Debug.WriteLine("Errore durante il caricamento del form: " & ex.Message)
            ' Oppure, se si desidera salvare il log in un file:
            Using writer As New StreamWriter("log.txt", True)
                writer.WriteLine("Errore durante il caricamento del form: " & ex.ToString())
            End Using
        End Try

    End Sub

    Private Sub generateGridViewSessioni_Columns()

        Dim idEstrazioneColumn As New GridViewTextBoxColumn With {.Name = "idEstrazione", .Width = 60, .FieldName = "idEstrazione",
                                                    .HeaderText = "Sessione",
                                                    .TextAlignment = ContentAlignment.MiddleLeft}

        Dim idDataEstrazioneColumn As New GridViewDateTimeColumn With {.Name = "Data", .Width = 130, .FieldName = "DataEstrazione",
                                                    .HeaderText = "Data Creazione",
                                                    .TextAlignment = ContentAlignment.MiddleLeft}

        Dim idDataInvioColumn As New GridViewDateTimeColumn With {.Name = "DataInvio", .Width = 130, .FieldName = "DataInvio",
                                                    .HeaderText = "Data Invio",
                                                    .TextAlignment = ContentAlignment.MiddleLeft}

        Dim xmlButtonColumn As New GridViewCommandColumn With {.Name = "XMLButton", .Width = 60,
                                                            .HeaderText = "File XML", .DefaultText = "xml", .Image = My.Resources.XMLFileHS, .ImageAlignment = ContentAlignment.MiddleCenter}

        Dim esitoButtonColumn As New GridViewCommandColumn With {.Name = "esitoButton", .Width = 60,
                                                            .HeaderText = "Esito", .DefaultText = "esito", .Image = My.Resources.Information_16x16, .ImageAlignment = ContentAlignment.MiddleCenter}

        Dim ricevutaButtonColumn As New GridViewCommandColumn With {.Name = "RicevutaButton", .Width = 60,
                                                            .HeaderText = "Ricevuta", .DefaultText = "pdf", .Image = My.Resources.file_pdf, .ImageAlignment = ContentAlignment.MiddleCenter}

        Dim idStatoColumn As New GridViewTextBoxColumn With {.Name = "Stato", .Width = 170, .FieldName = "Stato",
                                                    .HeaderText = "Stato Invio",
                                                    .TextAlignment = ContentAlignment.MiddleLeft}

        Dim csvButtonColumn As New GridViewCommandColumn With {.Name = "csvButton", .Width = 60,
                                                            .HeaderText = "File CSV", .DefaultText = "csv", .Image = My.Resources.NewReportHS, .ImageAlignment = ContentAlignment.MiddleCenter}

        Dim idProtocolloColumn As New GridViewTextBoxColumn With {.Name = "Protocollo", .Width = 140, .FieldName = "Protocollo",
                                                    .HeaderText = "Protocollo n.",
                                                    .TextAlignment = ContentAlignment.MiddleLeft}

        Dim idEstratteDalColumn As New GridViewDateTimeColumn With {.Name = "EstratteDal", .Width = 90, .FieldName = "DataDal",
                                                    .HeaderText = "Data Inizio",
                                                    .TextAlignment = ContentAlignment.MiddleLeft, .IsVisible = False}

        Dim esitoElaborazioneColumn As New GridViewTextBoxColumn With {.Name = "esitoElaborazione", .Width = 200, .FieldName = "EsitoElaborazione",
                                                    .HeaderText = "Esito Elaborazione",
                                                    .TextAlignment = ContentAlignment.MiddleLeft}

        Dim idEstratteAlColumn As New GridViewDateTimeColumn With {.Name = "EstratteAl", .Width = 90, .FieldName = "DataAl",
                                                    .HeaderText = "Data Fine",
                                                    .TextAlignment = ContentAlignment.MiddleLeft, .IsVisible = False}

        Dim idNumeroDalColumn As New GridViewTextBoxColumn With {.Name = "NumeroDal", .Width = 70, .FieldName = "NumeroDal",
                                                    .HeaderText = "Dal Numero",
                                                    .TextAlignment = ContentAlignment.MiddleLeft, .IsVisible = False}

        Dim idNumeroAlColumn As New GridViewTextBoxColumn With {.Name = "NumeroAl", .Width = 70, .FieldName = "NumeroAl",
                                                    .HeaderText = "Al Numero",
                                                    .TextAlignment = ContentAlignment.MiddleLeft, .IsVisible = False}

        Dim filesButtonColumn As New GridViewCommandColumn With {.Name = "filesButton", .Width = 70,
                                                            .HeaderText = "Cartella File", .DefaultText = "files", .Image = My.Resources.Folder_24x24, .ImageAlignment = ContentAlignment.MiddleCenter}

        RadGridViewElencoSessioni.Columns.Add(esitoButtonColumn)
        RadGridViewElencoSessioni.Columns.Add(idEstrazioneColumn)
        RadGridViewElencoSessioni.Columns.Add(idDataEstrazioneColumn)
        RadGridViewElencoSessioni.Columns.Add(idDataInvioColumn)
        RadGridViewElencoSessioni.Columns.Add(idStatoColumn)
        RadGridViewElencoSessioni.Columns.Add(idProtocolloColumn)
        RadGridViewElencoSessioni.Columns.Add(xmlButtonColumn)
        RadGridViewElencoSessioni.Columns.Add(ricevutaButtonColumn)
        RadGridViewElencoSessioni.Columns.Add(csvButtonColumn)
        RadGridViewElencoSessioni.Columns.Add(idEstratteDalColumn)
        RadGridViewElencoSessioni.Columns.Add(idEstratteAlColumn)
        RadGridViewElencoSessioni.Columns.Add(idNumeroDalColumn)
        RadGridViewElencoSessioni.Columns.Add(idNumeroAlColumn)
        RadGridViewElencoSessioni.Columns.Add(esitoElaborazioneColumn)
        RadGridViewElencoSessioni.Columns.Add(filesButtonColumn)

        AddHandler RadGridViewElencoSessioni.CommandCellClick, AddressOf RadGridViewElencoSessioni_CommandCellClick

    End Sub

    Private Sub RadGridViewElencoSessioni_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadGridViewElencoSessioni.ToolTipTextNeeded
        If Not sender.Parent.GetType.GetProperty("ColumnIndex") Is Nothing Then

            Select Case sender.Parent.ColumnIndex
                Case 0
                    e.ToolTipText = "Richiesta esito Invio"

                Case 6
                    e.ToolTipText = "Apri il file XML"

                Case 7
                    e.ToolTipText = "Scarica la Ricevuta di informazioni sull'invio"

                Case 8
                    e.ToolTipText = "Scarica il file di errori "

                Case 14
                    e.ToolTipText = "Apri la cartella dei file"

            End Select
        Else
            e.ToolTipText = sender.Text
        End If
    End Sub

    Private Sub ReadSettings()
        If RadDropDownListReadSettingsFrom.SelectedItem Is Nothing Then Exit Sub

        Dim itm As RadListDataItem = RadDropDownListReadSettingsFrom.SelectedItem

        Select Case itm.ToString
            Case "Configuratore"
                RadTextBoxCfProprietario.Text = My.Settings.cfProprietario
                RadTextBoxCodASL.Text = My.Settings.codASL
                RadTextBoxCodRegione.Text = My.Settings.codRegione
                RadTextBoxCodSSA.Text = My.Settings.codSSA

                RadTextBoxNomeUtente.Text = My.Settings.NomeUtente
                RadTextBoxPassword.Text = My.Settings.Password
                RadTextBoxPincode.Text = My.Settings.Pincode
                RadTextBoxPercorsoCreazioneXML.Text = My.Settings.DefaultXmlPath

            Case "Tabella"
                Try
                    Dim settingsRow As DataRow = DataBase.GetSettings(DB)
                    If settingsRow Is Nothing Then
                        RadMessageBox.Show(Me, "Impostazioni mancanti nel database" & vbCrLf & vbCrLf &
                                           "Inserire i dati", "Attenzione!", MessageBoxButtons.OK, RadMessageIcon.Exclamation)

                        RadButtonModificaSettings_Click(Nothing, Nothing)
                        Exit Sub
                    End If
                    RadTextBoxCfProprietario.Text = settingsRow("cfProprietario")
                    RadTextBoxCodASL.Text = settingsRow("codASL")
                    RadTextBoxCodRegione.Text = settingsRow("codRegione")
                    RadTextBoxCodSSA.Text = settingsRow("codSSA")

                    RadTextBoxNomeUtente.Text = settingsRow("NomeUtente")
                    RadTextBoxPassword.Text = settingsRow("Passw")
                    RadTextBoxPincode.Text = settingsRow("Pincode")
                    RadTextBoxPercorsoCreazioneXML.Text = settingsRow("DefaultXmlPath")

                Catch ex As Exception
                    RadMessageBox.Show(Me, "Errore nella tabella Settings" & vbCrLf & vbCrLf & "Database: " & DB & vbCrLf &
                                       "Controllare le impostazioni e riprovare", "Errore!", MessageBoxButtons.OK, RadMessageIcon.Error)

                    End
                End Try
        End Select

        WebServiceUser = RadTextBoxNomeUtente.Text.Trim
        WebServicePassword = RadTextBoxPassword.Text.Trim
        WebServicePincode = RadTextBoxPincode.Text.Trim

    End Sub


    Private Sub provaConnessioneWeb()

        ' Chiedo una ricevuta per un protocollo per testare la connessione al servizio

        Dim RicevutaClient As RicevutaPdf730Service.RicevutaPdf730Client = New RicevutaPdf730Service.RicevutaPdf730Client
        ServicePointManager.ServerCertificateValidationCallback = Function(
                            se As Object, certif As System.Security.Cryptography.X509Certificates.X509Certificate,
                            chain As System.Security.Cryptography.X509Certificates.X509Chain,
                            sslerror As System.Net.Security.SslPolicyErrors) True

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12

        'Dim myBinding As New BasicHttpBinding()
        'With myBinding
        '    .MessageEncoding = WSMessageEncoding.Mtom
        '    .TransferMode = TransferMode.Streamed
        '    .Security.Mode = BasicHttpSecurityMode.Transport
        '    .Security.Transport.ClientCredentialType = HttpClientCredentialType.None
        '    .Security.Transport.ProxyCredentialType = HttpClientCredentialType.None
        '    .Security.Transport.Realm = ""
        '    .SendTimeout = New TimeSpan(0, 5, 0)
        'End With
        'RicevutaClient.Endpoint.Binding = myBinding

        RicevutaClient.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior(RadTextBoxNomeUtente.Text.Trim, RadTextBoxPassword.Text.Trim))
        'RicevutaClient.ClientCredentials.UserName.UserName = RadTextBoxNomeUtente.Text.Trim
        'RicevutaClient.ClientCredentials.UserName.Password = RadTextBoxPassword.Text.Trim

        Dim Certificate As String = Application.StartupPath & "\SanitelCF.cer"

        'RadMessageBox.Show(Me, "UserName = " & RicevutaClient.ClientCredentials.UserName.UserName & vbCrLf &
        '"Password = " & RicevutaClient.ClientCredentials.UserName.Password & vbCrLf &
        '"EndPoint = " & RicevutaClient.Endpoint.Address.ToString & vbCrLf &
        '"pincode = " & RadTextBoxPincode.Text.Trim & vbCrLf &
        '"Percorso Cert. = " & Certificate)

        'WriteActivityLog("UserName = " & RicevutaClient.ClientCredentials.UserName.UserName)
        'WriteActivityLog("Password = " & RicevutaClient.ClientCredentials.UserName.Password)
        'WriteActivityLog("EndPoint = " & RicevutaClient.Endpoint.Address.ToString)
        'WriteActivityLog("pincode = " & RadTextBoxPincode.Text.Trim)
        'WriteActivityLog("Percorso Cert. = " & Certificate)

        Dim pincode As String = CriptaStringa(RadTextBoxPincode.Text.Trim)



        ' Dati Test Lazio
        'RicevutaClient.ClientCredentials.UserName.UserName = "UYBP8F4G"
        'RicevutaClient.ClientCredentials.UserName.Password = "PXXCLMBE"
        'RicevutaClient.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior("UYBP8F4G", "PXXCLMBE"))
        'Dim pincode As String = CriptaStringa("4143408326")

        'Dati DM
        'RicevutaClient.ClientCredentials.UserName.UserName = "UI4DZXX1"
        'RicevutaClient.ClientCredentials.UserName.Password = "PAM72H67"
        'RicevutaClient.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior("UI4DZXX1", "PAM72H67"))
        'Dim pincode As String = CriptaStringa("1598469793")

        'Dati Aster
        'RicevutaClient.ClientCredentials.UserName.UserName = "UKZE5BNZ"
        'RicevutaClient.ClientCredentials.UserName.Password = "PWMTXUUA"
        'RicevutaClient.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior("UKZE5BNZ", "PWMTXUUA"))
        'Dim pincode As String = CriptaStringa("7131420964")

        Dim protocollo As String = "16020217264038299"
        Dim datiInput As New RicevutaPdf730Service.datiInput
        datiInput.pinCode = pincode
        datiInput.protocollo = protocollo
        Dim Ricevuta As RicevutaPdf730Service.datiOutput = Nothing

        WriteActivityLog(RicevutaClient.ChannelFactory.Endpoint.ToString)

        Try
            Ricevuta = RicevutaClient.RicevutaPdf(datiInput)
        Catch ex As Exception
            RadMessageBox.Show(Me, "Errore di connessione al servizio TS: " & vbCrLf & vbCrLf & ex.ToString, "Errore di Connessione!", MessageBoxButtons.OK, RadMessageIcon.Error)
            Exit Sub
        End Try

        If Ricevuta IsNot Nothing Then
            If Ricevuta.esitiNegativi IsNot Nothing AndAlso Ricevuta.esitiNegativi.Count > 0 Then
                RadMessageBox.Show(Me, "Connessione al servizio TS con errori: " & vbCrLf & vbCrLf & "Cod.: " & Ricevuta.esitiNegativi.First.codice &
                                   vbCrLf & " Descr.: " & Ricevuta.esitiNegativi.First.descrizione, "Errore di Connessione!", MessageBoxButtons.OK, RadMessageIcon.Error)
                Exit Sub
            End If
            If Ricevuta.esitiPositivi IsNot Nothing AndAlso Ricevuta.esitiPositivi.dettagliEsito IsNot Nothing Then
                RadMessageBox.Show(Me, "Connessione al servizio TS correttamente effettuata", "Connessione OK", MessageBoxButtons.OK, RadMessageIcon.Info)
            End If
        End If

    End Sub



    Private Sub InvioFile()

        If RadGridViewElencoSessioni.SelectedRows.Count = 0 Then
            RadMessageBox.Show(Me, "Seleziona una sessione per cui effettuare l'operazione", "Nessuna sessione selezionata", MessageBoxButtons.OK, RadMessageIcon.Exclamation)
            Exit Sub
        End If

        currentSessione = RadGridViewElencoSessioni.SelectedRows.First.DataBoundItem

        Dim myBinding As New BasicHttpBinding()
        With myBinding
            .TransferMode = TransferMode.Streamed
            .MessageEncoding = WSMessageEncoding.Mtom
            .Security.Mode = BasicHttpSecurityMode.Transport
            .SendTimeout = New TimeSpan(0, 5, 0)
        End With

        ServicePointManager.ServerCertificateValidationCallback = Function(
                            se As Object, certif As System.Security.Cryptography.X509Certificates.X509Certificate,
                            chain As System.Security.Cryptography.X509Certificates.X509Chain,
                            sslerror As System.Net.Security.SslPolicyErrors) True

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12


        Dim InvioClient As InvioTelematicoSpeseSanitarie730p.InvioTelematicoSS730pMtomClient = New InvioTelematicoSpeseSanitarie730p.InvioTelematicoSS730pMtomClient
        Dim Proprietario As InvioTelematicoSpeseSanitarie730p.proprietario = New InvioTelematicoSpeseSanitarie730p.proprietario
        Dim Ricevuta As InvioTelematicoSpeseSanitarie730p.ricevutaInvio = New InvioTelematicoSpeseSanitarie730p.ricevutaInvio

        ' Dati Test Lazio
        InvioClient.ClientCredentials.UserName.UserName = WebServiceUser
        InvioClient.ClientCredentials.UserName.Password = WebServicePassword
        InvioClient.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior(WebServiceUser, WebServicePassword))
        'Proprietario.cfProprietario = CriptaStringa("CCSRMO77A09H501E")
        Proprietario.cfProprietario = RadTextBoxCfProprietario.Text.Trim
        Proprietario.codiceRegione = RadTextBoxCodRegione.Text.Trim
        Proprietario.codiceAsl = RadTextBoxCodASL.Text.Trim
        Proprietario.codiceSSA = RadTextBoxCodSSA.Text.Trim
        Dim pincode As String = CriptaStringa(WebServicePincode)


        Dim zipFile As String = Strings.Right(currentSessione.PercorsoXML, currentSessione.PercorsoXML.Length - InStrRev(currentSessione.PercorsoXML, "\")) & ".zip"

        If currentSessione.Stato <> "XML creato e da Inviare" Then
            RadMessageBox.Show(Me, "Lo stato corrente della sessione non consente di inviare il file." & vbCrLf & vbCrLf & "File non trovato o già inviato", "Errore", MessageBoxButtons.OK, RadMessageIcon.Error)
            Exit Sub
        End If

        ' Check XML existance
        If Not IO.File.Exists("TS\" & zipFile) Then
            RadMessageBox.Show(Me, "File XML non trovato." & vbCrLf & vbCrLf & "Creare il file o verificare che esista nel percorso : " & vbCrLf & zipFile, "Errore", MessageBoxButtons.OK, RadMessageIcon.Error)
            Exit Sub
        End If


        Dim contenuto_allegato As Byte()
        contenuto_allegato = IO.File.ReadAllBytes("TS\" & zipFile)

        Try

            Ricevuta = InvioClient.inviaFileMtom("TS\" & zipFile, pincode, Proprietario, Nothing, Nothing, Nothing, contenuto_allegato)
        Catch ex As Exception
            RadMessageBox.Show(Me, "Errore di comunicazione:" & vbCrLf & vbCrLf & ex.ToString, "Errore", MessageBoxButtons.OK, RadMessageIcon.Error)
            Exit Sub
        End Try
        Dim protocollo As String = Ricevuta.protocollo

        If protocollo Is Nothing Then
            MsgBox("Errore: " & Ricevuta.descrizioneEsito & vbCrLf &
                   "Invio non effettuato")
        Else
            updateProtocollo(protocollo, currentSessione, Ricevuta)

            For Each s As RigaSessione In SessioniList.Where(Function(ses) ses.idEstrazione = currentSessione.idEstrazione)
                s.DataInvio = Now
                s.Protocollo = protocollo
                s.Stato = "XML Inviato"
                s.EsitoElaborazione = Ricevuta.codiceEsito & " - " & Ricevuta.descrizioneEsito
            Next
            RadGridViewElencoSessioni.DataSource = Nothing
            RadGridViewElencoSessioni.DataSource = SessioniList.OrderByDescending(Function(s) s.idEstrazione)
        End If


    End Sub
    Public Function CriptaStringa(ByVal s As String) As String
        Try

            Dim Certificate As String = Application.StartupPath & "\SanitelCF.cer"
            Dim cert As New X509Certificate2(Certificate)
            Dim rsaEncryptor As RSACryptoServiceProvider = CType(cert.PublicKey.Key, RSACryptoServiceProvider)

            Dim stringa As Byte() = System.Text.Encoding.ASCII.GetBytes(s)
            Dim cipherData() As Byte = rsaEncryptor.Encrypt(stringa, False)

            CriptaStringa = Convert.ToBase64String(cipherData)

        Catch ex As Exception
            CriptaStringa = ex.Message
        End Try

    End Function

    Private Sub scaricaRicevuta()
        ServicePointManager.ServerCertificateValidationCallback = Function(
                            se As Object, certif As System.Security.Cryptography.X509Certificates.X509Certificate,
                            chain As System.Security.Cryptography.X509Certificates.X509Chain,
                            sslerror As System.Net.Security.SslPolicyErrors) True

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12

        Dim RicevutaClient As RicevutaPdf730Service.RicevutaPdf730Client = New RicevutaPdf730Service.RicevutaPdf730Client

        RicevutaClient.ClientCredentials.UserName.UserName = WebServiceUser
        RicevutaClient.ClientCredentials.UserName.Password = WebServicePassword
        RicevutaClient.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior(WebServiceUser, WebServicePassword))
        Dim pincode As String = CriptaStringa(WebServicePincode)

        currentSessione = RadGridViewElencoSessioni.SelectedRows.First.DataBoundItem

        'Dati DM
        'RicevutaClient.ClientCredentials.UserName.UserName = "UI4DZXX1"
        'RicevutaClient.ClientCredentials.UserName.Password = "PAM72H67"
        'RicevutaClient.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior("UI4DZXX1", "PAM72H67"))
        'Dim pincode As String = CriptaStringa("1598469793")

        'Dati Aster
        'RicevutaClient.ClientCredentials.UserName.UserName = "UKZE5BNZ"
        'RicevutaClient.ClientCredentials.UserName.Password = "PWMTXUUA"
        'RicevutaClient.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior("UKZE5BNZ", "PWMTXUUA"))
        'Dim pincode As String = CriptaStringa("7131420964")

        Dim protocollo As String = currentSessione.Protocollo



        Dim datiInput As New RicevutaPdf730Service.datiInput
        datiInput.pinCode = pincode
        datiInput.protocollo = protocollo
        Dim Ricevuta As RicevutaPdf730Service.datiOutput
        Ricevuta = RicevutaClient.RicevutaPdf(datiInput)

        If Ricevuta.esitiNegativi IsNot Nothing AndAlso Ricevuta.esitiNegativi.Count > 0 Then
            RadMessageBox.Show(Me, Ricevuta.esitiNegativi.First.descrizione)
            Exit Sub
        End If

        Dim frmViewer As New frmPdfRicevutaEsito
        Dim stream As New MemoryStream(Ricevuta.esitiPositivi.dettagliEsito.pdf)
        frmViewer.stream = stream
        frmViewer.ShowDialog()

    End Sub

    Private Sub richiestaCSV()
        ServicePointManager.ServerCertificateValidationCallback = Function(
                            se As Object, certif As System.Security.Cryptography.X509Certificates.X509Certificate,
                            chain As System.Security.Cryptography.X509Certificates.X509Chain,
                            sslerror As System.Net.Security.SslPolicyErrors) True

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12

        Dim RicevutaCSV As DettaglioErrori730Service.DettaglioErrori730Client = New DettaglioErrori730Service.DettaglioErrori730Client

        ' Dati Test Lazio
        RicevutaCSV.ClientCredentials.UserName.UserName = WebServiceUser
        RicevutaCSV.ClientCredentials.UserName.Password = WebServicePassword
        RicevutaCSV.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior(WebServiceUser, WebServicePassword))
        Dim pincode As String = CriptaStringa(WebServicePincode)

        currentSessione = RadGridViewElencoSessioni.SelectedRows.First.DataBoundItem

        Dim protocollo As String = currentSessione.Protocollo
        Dim datiInput As New DettaglioErrori730Service.datiInput
        datiInput.pinCode = pincode
        datiInput.protocollo = protocollo


        Dim Ricevuta As DettaglioErrori730Service.datiOutput
        Ricevuta = RicevutaCSV.DettaglioErrori(datiInput)

        If Ricevuta.esitiNegativi IsNot Nothing AndAlso Ricevuta.esitiNegativi.Count > 0 Then
            RadMessageBox.Show(Me, Ricevuta.esitiNegativi.First.descrizione)
            Exit Sub
        End If

        Dim fileCSV = Ricevuta.esitiPositivi.dettagliEsito.csv
        Dim CartellaXML_Root As String = currentSessione.PercorsoXML.Substring(0, InStrRev(currentSessione.PercorsoXML, "\"))
        Dim SalvaIn As String = CartellaXML_Root & "Errori-730MEF-prot." & protocollo & ".zip"

        If IO.File.Exists(SalvaIn) Then
            Dim answ As DialogResult = RadMessageBox.Show(Me, "File già esistente in " & SalvaIn & vbCrLf & vbCrLf & "Sovrascrivere il file?", "Azione richiesta", MessageBoxButtons.YesNo, RadMessageIcon.Question)
            If answ = Windows.Forms.DialogResult.No Then Exit Sub
            IO.File.Delete(SalvaIn)
        End If

        Dim fileStream As New System.IO.FileStream(SalvaIn, FileMode.Create, FileAccess.Write)
        fileStream.Write(fileCSV, 0, fileCSV.Length)
        fileStream.Close()

        Dim answ2 As DialogResult = RadMessageBox.Show(Me, "File correttamente salvato in " & SalvaIn & vbCrLf & vbCrLf & "Aprire il percorso del file?", "Azione richiesta", MessageBoxButtons.YesNo, RadMessageIcon.Question)
        If answ2 = Windows.Forms.DialogResult.Yes Then
            If IO.Directory.Exists(CartellaXML_Root) Then
                Dim Proc As String = "explorer.exe"

                Process.Start(CartellaXML_Root)
            End If

        End If

    End Sub

    Private Function richiestaEsitoInvio() As String
        ServicePointManager.ServerCertificateValidationCallback = Function(
                            se As Object, certif As System.Security.Cryptography.X509Certificates.X509Certificate,
                            chain As System.Security.Cryptography.X509Certificates.X509Chain,
                            sslerror As System.Net.Security.SslPolicyErrors) True

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12

        Dim EsitoInvioDati As EsitoInvioDatiSpesa730Service.EsitoInvioDatiSpesa730Client = New EsitoInvioDatiSpesa730Service.EsitoInvioDatiSpesa730Client
        ' Dati Test Lazio
        EsitoInvioDati.ClientCredentials.UserName.UserName = WebServiceUser
        EsitoInvioDati.ClientCredentials.UserName.Password = WebServicePassword
        EsitoInvioDati.Endpoint.Behaviors.Add(New BasicAuthenticationBehavior(WebServiceUser, WebServicePassword))
        Dim pincode As String = CriptaStringa(WebServicePincode)

        currentSessione = RadGridViewElencoSessioni.SelectedRows.First.DataBoundItem

        Dim protocollo As String = currentSessione.Protocollo

        Dim datiInput As New EsitoInvioDatiSpesa730Service.datiInput
        datiInput.pinCode = pincode
        datiInput.protocollo = protocollo
        Dim Ricevuta As EsitoInvioDatiSpesa730Service.datiOutput
        Ricevuta = EsitoInvioDati.EsitoInvii(datiInput)

        If Ricevuta.esitiNegativi IsNot Nothing AndAlso Ricevuta.esitiNegativi.Count > 0 Then
            RadMessageBox.Show(Me, Ricevuta.esitiNegativi.First.codice & " - " & Ricevuta.esitiNegativi.First.descrizione)
            updateEsito(currentSessione, Ricevuta.esitiNegativi.First.codice & " - " & Ricevuta.esitiNegativi.First.descrizione)
            For Each s As RigaSessione In SessioniList.Where(Function(ses) ses.idEstrazione = currentSessione.idEstrazione)
                s.EsitoElaborazione = Ricevuta.esitiNegativi.First.codice & " - " & Ricevuta.esitiNegativi.First.descrizione
            Next
            Return Ricevuta.esitiNegativi.First.codice & " - " & Ricevuta.esitiNegativi.First.descrizione
        End If

        If Ricevuta.esitiPositivi IsNot Nothing AndAlso Ricevuta.esitiPositivi.Count > 0 Then
            RadMessageBox.Show(Me, Ricevuta.esitiPositivi.First.descrizione)
            updateEsito(currentSessione, Ricevuta.esitiPositivi.First.descrizione)
            For Each s As RigaSessione In SessioniList.Where(Function(ses) ses.idEstrazione = currentSessione.idEstrazione)
                s.EsitoElaborazione = Ricevuta.esitiPositivi.First.descrizione
            Next
            Return Ricevuta.esitiPositivi.First.descrizione
        End If

        Dim stream As New MemoryStream(Ricevuta.descrizioneEsito)

    End Function

    Private Sub RadButtonRichiediRicevutaEsitoPDF_Click(sender As Object, e As EventArgs)
        scaricaRicevuta()
    End Sub

    Private Sub RadButtonInviaXML_Click(sender As Object, e As EventArgs) Handles RadButtonInviaXML.Click
        Cursor = Cursors.WaitCursor
        InvioFile()
        Cursor = Cursors.Default
    End Sub

    Private Sub RadButtonRichiediRicevutaEsitoCSV_Click(sender As Object, e As EventArgs)
        richiestaCSV()
    End Sub

    Private Sub RadButtonRichiediRicevutaEsitoInvio_Click(sender As Object, e As EventArgs)
        richiestaEsitoInvio()
    End Sub

    Private Sub RadButtonSfogliaPercorsoCreazione_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RadDropDownListReadSettingsFrom_KeyPress(sender As Object, e As KeyPressEventArgs) Handles RadDropDownListReadSettingsFrom.KeyPress
        e.Handled = True
    End Sub

    Private Sub RadDropDownListReadSettingsFrom_SelectedIndexChanged(sender As Object, e As UI.Data.PositionChangedEventArgs) Handles RadDropDownListReadSettingsFrom.SelectedIndexChanged

        ReadSettings()

    End Sub

    Private Sub RadButtonSfoglia_FileASCII_Click(sender As Object, e As EventArgs) Handles RadButtonSfoglia_FileASCII.Click
        Cursor = Cursors.WaitCursor
        Dim openFile As FileDialog = New OpenFileDialog With {.FileName = "*.txt",
                                                              .Filter = "All Files|*.txt",
                                                              .InitialDirectory = "..\\",
                                                              .Title = "Seleziona il file dati testo da importare"}
        openFile.ShowDialog()
        File = openFile.FileName
        RadTextBoxPercorsoFileDiTesto.Text = openFile.FileName
        RadButtonCaricaFileASCII.Enabled = True
        RadButtonGeneraXML.Enabled = False
        RadLabelAzioni.Text = "Carica i dati in Tabella"
        Cursor = Cursors.Default
    End Sub

    Private Sub RadButtonCaricaFileASCII_Click(sender As Object, e As EventArgs) Handles RadButtonCaricaFileASCII.Click
        WaitingDialog.ShowDialog("Acquisizione File")
        Try
            FP = New FileProcesser(File, DB)
            Cursor = Cursors.WaitCursor
            RadLabelElementDescrizioneOperazione.Text = "Conversione e caricamento dati in tabella con il file selezionato....."
            RadLabelElementDescrizioneOperazione.ForeColor = Color.Red
            FP.RadTextBoxFattureDal = Me.RadTextBoxFattureDal
            FP.RadTextBoxFattureAl = Me.RadTextBoxFattureAl
            FP.RadTextBoxNumFattureDal = Me.RadTextBoxNumFattureDal
            FP.RadTextBoxNumFattureAl = Me.RadTextBoxNumFattureAl
            FP.RadTextBoxRecordImportati = Me.RadTextBoxRecordImportati
            FP.RadTextBoxIdSessione = Me.RadTextBoxIdSessione

            FP.StartProcessing()
            Me.DataGridView1.DataSource = FP.DTable
            Me.DataGridView1.Refresh()
            DataGridView1.Columns(0).Visible = False
            RadTextBoxRecordImportati.Text = DataGridView1.Rows.Count()
            RadLabelElementDescrizioneOperazione.ForeColor = Color.Green
            RadLabelElementDescrizioneOperazione.Text = "Acquisizione file completata"
            Me.RadTextBoxFattureDal.Text = DataGridView1.Rows.Item(0).Cells(6).Value.Date
            Me.RadTextBoxNumFattureDal.Text = DataGridView1.Rows.Item(0).Cells(5).Value
            Me.RadTextBoxFattureAl.Text = DataGridView1.Rows.Item(DataGridView1.Rows.Count - 2).Cells(6).Value.Date
            Me.RadTextBoxNumFattureAl.Text = DataGridView1.Rows.Item(DataGridView1.Rows.Count - 2).Cells(5).Value

            FP.UpDataDB()

            RadButtonCaricaFileASCII.Enabled = False
            RadButtonGeneraXML.Enabled = True
            RadLabelAzioni.Text = "Verifica dati e crea XML"
            Cursor = Cursors.Default
        Catch oex As OleDbException
            Cursor = Cursors.Default
            RadMessageBox.Show(Me, "Errore:" & vbCrLf & vbCrLf & oex.ToString)
        Catch ex As Exception
            Cursor = Cursors.Default
            RadMessageBox.Show(Me, "Errore:" & vbCrLf & vbCrLf & ex.ToString)
        End Try
        WaitingDialog.CloseDialog()
    End Sub

    Private Sub RadButtonGeneraXML_Click(sender As Object, e As EventArgs) Handles RadButtonGeneraXML.Click
        CreateXML_SS730()
    End Sub


    Private Sub CreateXML_SS730()

        WaitingDialog.ShowDialog("Creazione XML")
        Cursor = Cursors.WaitCursor
        isXMLvalid = False
        Dim TotaleFatture As String
        Dim TotaleRecord As String
        Dim XML_Folder As String = RadTextBoxPercorsoCreazioneXML.Text & "\"
        Dim XML_Name As String = String.Empty
        Dim ZIP_Folder As String = RadTextBoxPercorsoCreazioneXML.Text & "\"
        Dim nowString As String = String.Empty
        Dim nowDateTime As DateTime

        Try
            RadButtonGeneraXML.Enabled = False
            ' Encrypt cfProprietario
            Dim cfProprietarioString As String = RadTextBoxCfProprietario.Text.Trim
            Dim cfProprietarioDb As String = FP.DTable.Rows(0).Item("cfProprietario")
            Dim cfProprietario As String = CriptaStringa(cfProprietarioString)

            Dim recordList As New List(Of SchemaXML)
            Dim distinctRecordList As New List(Of SchemaXML)

            For Each r As DataRow In FP.DTable.Rows
                Dim newInput As New SchemaXML
                newInput.numDocumento = Trim(r.Item("numDocumento"))
                newInput.dataEmissione = r.Item("dataEmissione")
                newInput.codiceRegione = r.Item("codiceRegione")
                newInput.codiceAsl = r.Item("codiceASL")
                newInput.codiceSSA = r.Item("codiceSSA")

                If newInput.dataEmissione Is Nothing Then
                    Dim debug = 1
                End If

                ' Qui spaghettata
                ' C'è un problema con la struttura Radiologia Mostacciano quando emette fatture con sezionale C Via Cina
                ' Il codice SSA che leggiamo dal file è quello di Radiologia Mostacciano sesionale R ovver 011400 mentre qiello C è 020210
                ' Quindi nel caso in cui rilevo la presenza di codice SSA 011400 ma nella configurazione trovo 020210, allora forzo quest'ultimo

                If RadTextBoxCodSSA.Text.Trim = "020210" And r.Item("codiceSSA").ToString.Trim = "011400" Then
                    newInput.codiceSSA = "020210"
                End If

                newInput.cfProprietario = cfProprietario
                newInput.pIva = r.Item("pIva")
                newInput.dataPagamento = r.Item("dataPagamento")
                If Not distinctRecordList.Where(Function(dr) dr.numDocumento = newInput.numDocumento).Any Then distinctRecordList.Add(newInput)
                newInput.madreNotaCreditoData = IIf(r.Item("madreNotaCreditoData") Is DBNull.Value, Nothing, r.Item("madreNotaCreditoData"))
                newInput.madreNotaCreditoNumero = Trim(r.Item("madreNotaCreditoNumero"))
                If r.Item("madreNotaCreditoNumero") Is Nothing OrElse
                Strings.Left(r.Item("madreNotaCreditoNumero"), 1) = "/" OrElse
                r.Item("madreNotaCreditoNumero").ToString.Trim.Length = 0 Then
                    newInput.flgRimborso = Nothing
                Else
                    newInput.flgRimborso = True
                End If
                newInput.flagPagamentoTracciato = r.Item("flagPagamentoTracciato")
                newInput.tipoDocumento = r.Item("tipoDocumento")
                newInput.flagOpposizione = r.Item("flagOpposizione")
                If r.Item("flagOpposizione") Is Nothing OrElse r.Item("flagOpposizione") = False Then
                    ' se c'è opposizione il cf cittadino non va valorizzato
                    newInput.cfCittadino = r.Item("cfCittadino")
                End If
                newInput.dispositivo = Trim(r.Item("dispositivo"))
                newInput.flagOperazione = Trim(r.Item("flagOperazione"))
                newInput.tipoSpesa = Trim(r.Item("flagTipoSpesa"))
                newInput.tipoSpesa = Trim(r.Item("TipoSpesa"))
                newInput.importo = Trim(r.Item("importo"))
                recordList.Add(newInput)
            Next

            TotaleFatture = distinctRecordList.Count
            TotaleRecord = FP.DTable.Rows.Count
            ' Start XML 
            Dim ns As XNamespace = "http://www.w3.org/2001/XMLSchema-instance"

            Dim xEle As XElement = New XElement("precompilata",
                                                New XAttribute(ns + "noNamespaceSchemaLocation", "730_precompilata.xsd"),
                                                New XAttribute(XNamespace.Xmlns + "xsi", ns.NamespaceName),
                                                New XElement("opzionale1"),
                                                New XElement("opzionale2"),
                                                New XElement("opzionale3"),
                                                New XElement("proprietario", New XElement("codiceRegione", recordList.First.codiceRegione),
                                                                             New XElement("codiceAsl", recordList.First.codiceAsl),
                                                                             New XElement("codiceSSA", recordList.First.codiceSSA),
                                                                             New XElement("cfProprietario", cfProprietario)))
            ' Iterate through distinct
            Dim cycle As Integer = 0

            For Each zt In distinctRecordList
                ' Populate documentoSpesa TAG
                xEle.Add(New XElement("documentoSpesa"))
                xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Add(New XElement("idSpesa", New XElement("pIva", zt.pIva),
                                                                           New XElement("dataEmissione", Format(zt.dataEmissione, "yyyy-MM-dd")),
                                                                           New XElement("numDocumentoFiscale")))
                xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Element("idSpesa").Element("numDocumentoFiscale").Add(New XElement("dispositivo", zt.dispositivo),
                                                                                                         New XElement("numDocumento", zt.numDocumento))
                If zt.flgRimborso Then

                    xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Add(New XElement("idRimborso", New XElement("pIva", zt.pIva),
                                                                                   New XElement("dataEmissione", Format(zt.madreNotaCreditoData, "yyyy-MM-dd")),
                                                                                   New XElement("numDocumentoFiscale")))
                    xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Element("idRimborso").Element("numDocumentoFiscale").Add(New XElement("dispositivo", zt.dispositivo),
                                                                                                     New XElement("numDocumento", zt.madreNotaCreditoNumero))
                End If
                xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Add(New XElement("dataPagamento", Format(zt.dataPagamento, "yyyy-MM-dd")))
                xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Add(New XElement("flagOperazione", zt.flagOperazione))
                ' Encrypf cfCittadino
                Dim cfCittadino As String = ""
                If zt.cfCittadino IsNot Nothing Then
                    cfCittadino = CriptaStringa(zt.cfCittadino)
                End If
                xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Add(New XElement("cfCittadino", cfCittadino))
                xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Add(New XElement("pagamentoTracciato", IIf(zt.flagPagamentoTracciato = True, "SI", "NO")))
                xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Add(New XElement("tipoDocumento", zt.tipoDocumento))
                xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Add(New XElement("flagOpposizione", IIf(zt.flagOpposizione = True, "1", "0")))


                ' Iterate through distinct subrecords
                Dim recordCollection As List(Of SchemaXML) = recordList.Where(
                    Function(z) z.numDocumento = zt.numDocumento And z.dataEmissione = zt.dataEmissione).ToList
                For Each r As SchemaXML In recordCollection
                    xEle.Elements.Where(Function(xe) xe.Name = "documentoSpesa").Last.Add(New XElement("voceSpesa",
                                                                                          New XElement("tipoSpesa", r.tipoSpesa),
                                                                                          New XElement("importo", r.importo),
                                                                                          New XElement("aliquotaIVA", "0.00")))
                Next
                cycle += 1
            Next

            nowDateTime = Now
            nowString = nowDateTime.Date.Year & nowDateTime.Date.Month & nowDateTime.Date.Day & "_" & nowDateTime.Hour & nowDateTime.Second
            XML_Name = "XML_TS_" & nowString

            XML_Folder = XML_Folder & nowString & "\"

            If Not Directory.Exists(XML_Folder) Then
                Directory.CreateDirectory(XML_Folder)
            Else
                For Each f In Directory.GetFiles(XML_Folder, "*.*", SearchOption.AllDirectories)
                    My.Computer.FileSystem.DeleteFile(f)
                Next
            End If
            xEle.Save(XML_Folder & XML_Name & ".xml")
            RadTextBoxRecordImportati.Text = TotaleRecord

            WaitingDialog.CloseDialog()

            validaXML(XML_Folder & XML_Name & ".xml")

            If isXMLvalid = False Then
                RadLabelAzioni.Text = "XML creato con errori"
                Me.RadLabelStatoXML.Text = "Errori di validazione. Verificare e riprovare"
                Me.RadLabelStatoXML.ForeColor = Color.Red


                Try

                    IO.File.Delete(XML_Folder & XML_Name & ".xml")
                    IO.Directory.Delete(XML_Folder)
                Catch ex As Exception

                End Try
                isXMLvalid = True
                Cursor = Cursors.Default
                Exit Sub
            End If

            Cursor = Cursors.Default

            RadMessageBox.Show(Me, "File XML creato correttamente e validato in: " & vbCrLf & XML_Folder & XML_Name & ".xml" & vbCrLf & vbCrLf &
                               "Fatture processate: " & TotaleFatture & vbCrLf & "Record Totali: " & TotaleRecord, "Creazione XML terminata", MessageBoxButtons.OK, RadMessageIcon.Info)

            Me.RadPageViewMainForm.SelectedPage = Me.RadPageViewComunicazioniPage

        Catch ex As Exception

            RadMessageBox.Show(Me, "Errore: " & vbCrLf & vbCrLf & ex.Message)

        End Try

        Cursor = Cursors.WaitCursor
        Dim ZIP_Name As String = XML_Name & ".zip"
        System.IO.Compression.ZipFile.CreateFromDirectory(XML_Folder, "TS\" & XML_Name & ".zip", Compression.CompressionLevel.Optimal, True)

        CreaSessioneSuDB(nowDateTime, nowString, XML_Folder & XML_Name)



        inserisciInStorico()

        deleteFromZtable()

        RadLabelAzioni.Text = "XML creato. Procedere all'invio"
        Me.RadLabelStatoXML.Text = "XML creato correttamente"
        Me.RadLabelStatoXML.ForeColor = Color.Green

        RadGridViewElencoSessioni.DataSource = Nothing
        RadGridViewElencoSessioni.DataSource = SessioniList.OrderByDescending(Function(s) s.idEstrazione)
        WaitingDialog.CloseDialog()
        Cursor = Cursors.Default

    End Sub



    Private Function FormatImporto(nullable As String) As String
        Dim Lenght As Integer = 7
        Dim firstPart As String = Strings.Left(nullable, InStr(nullable, ",") - 1)
        nullable = Strings.Right(nullable, nullable.ToString.Length - (firstPart.Length + 1))
        Dim decimalPart As String = Strings.Left(nullable, 2)
        decimalPart = decimalPart.PadRight(2, "0")
        Dim Importo As String = (firstPart & "." & decimalPart).ToString
        If Importo = "0.00" Then Importo = "0.01"
        Return Strings.Right(Importo, 7)
    End Function


    Private Sub RadButtonSfogliaPercorsoCreazione_Click_1(sender As Object, e As EventArgs) Handles RadButtonSfogliaPercorsoCreazione.Click
        Dim openFile As FolderBrowserDialog = New FolderBrowserDialog With {.RootFolder = Environment.SpecialFolder.Desktop}
        openFile.ShowDialog()

        RadTextBoxPercorsoCreazioneXML.Text = openFile.SelectedPath

    End Sub


    Private Sub RadButtonConfermaImpostazioni_Click(sender As Object, e As EventArgs) Handles RadButtonInizioProcedura.Click
        Cursor = Cursors.WaitCursor
        RadDropDownListReadSettingsFrom.Enabled = False
        RadTextBoxPercorsoCreazioneXML.Enabled = False
        Me.RadPageViewComunicazioniPage.Enabled = True
        Me.RadPageViewPreparazioniPage.Enabled = True
        System.Threading.Thread.Sleep(1000)
        Me.RadPageViewMainForm.SelectedPage = RadPageViewPreparazioniPage
        Me.RadPageViewSettingsPage.Enabled = False

        Cursor = Cursors.Default
    End Sub

    Private Sub RadButtonModificaSettings_Click(sender As Object, e As EventArgs) Handles RadButtonModificaSettings.Click

        RadButtonSalvaSettings.Enabled = True
        RadButtonInizioProcedura.Enabled = False
        RadButtonVerificaConnessioneWEB.Enabled = False
        If ValidSettings Then
            ValidSettings = False
            RadTextBoxCfProprietario.Text = String.Empty
            RadTextBoxCodASL.Text = String.Empty
            RadTextBoxCodRegione.Text = String.Empty
            RadTextBoxCodSSA.Text = String.Empty
            RadTextBoxNomeUtente.Text = String.Empty
            RadTextBoxPassword.Text = String.Empty
            RadTextBoxPincode.Text = String.Empty
            RadTextBoxPercorsoCreazioneXML.Text = String.Empty
            ReadSettings()
        End If

        RadTextBoxCfProprietario.Enabled = True
        RadTextBoxCodASL.Enabled = True
        RadTextBoxCodRegione.Enabled = True
        RadTextBoxCodSSA.Enabled = True
        RadTextBoxNomeUtente.Enabled = True
        RadTextBoxPassword.Enabled = True
        RadTextBoxPincode.Enabled = True
        RadTextBoxPercorsoCreazioneXML.Enabled = True

        RadButtonSfogliaPercorsoCreazione.Enabled = True

        RadTextBoxNomeUtente.Focus()
    End Sub

    Private Sub RadButtonSalvaSettings_Click(sender As Object, e As EventArgs) Handles RadButtonSalvaSettings.Click
        If ValidateSettingsAndWarn() = "Invalid" Then
            RadButtonInizioProcedura.Enabled = False
            Exit Sub
        End If

        Select Case RadDropDownListReadSettingsFrom.SelectedItem.Text
            Case "Tabella"
                InsertOrUpdateTableSettings()

            Case "Configuratore"
                My.MySettings.Default.Item("cfProprietario") = RadTextBoxCfProprietario.Text.Trim
                My.MySettings.Default.Item("codRegione") = RadTextBoxCodRegione.Text.Trim
                My.MySettings.Default.Item("codASL") = RadTextBoxCodASL.Text.Trim
                My.MySettings.Default.Item("codSSA") = RadTextBoxCodSSA.Text.Trim
                My.MySettings.Default.Item("NomeUtente") = RadTextBoxNomeUtente.Text.Trim
                My.MySettings.Default.Item("Password") = RadTextBoxPassword.Text.Trim
                My.MySettings.Default.Item("Pincode") = RadTextBoxPincode.Text.Trim
                My.MySettings.Default.Item("DefaultXmlPath") = RadTextBoxPercorsoCreazioneXML.Text.Trim
                My.Settings.Save()
                RadButtonSalvaSettings.Enabled = False

        End Select

        RadTextBoxCfProprietario.Enabled = False
        RadTextBoxCodASL.Enabled = False
        RadTextBoxCodRegione.Enabled = False
        RadTextBoxCodSSA.Enabled = False
        RadTextBoxNomeUtente.Enabled = False
        RadTextBoxPassword.Enabled = False
        RadTextBoxPincode.Enabled = False
        RadTextBoxPercorsoCreazioneXML.Enabled = False
        RadButtonVerificaConnessioneWEB.Enabled = True
        RadButtonInizioProcedura.Enabled = True
        RadButtonSalvaSettings.Enabled = False
        RadButtonSfogliaPercorsoCreazione.Enabled = False
    End Sub

    Private ValidSettings As Boolean
    Private Function ValidateSettingsAndWarn() As String
        If RadTextBoxCfProprietario.Text = String.Empty Or
            RadTextBoxCodASL.Text = String.Empty Or
            RadTextBoxCodRegione.Text = String.Empty Or
            RadTextBoxCodSSA.Text = String.Empty Or
            RadTextBoxNomeUtente.Text = String.Empty Or
            RadTextBoxPassword.Text = String.Empty Or
            RadTextBoxPincode.Text = String.Empty Or
            RadTextBoxPercorsoCreazioneXML.Text = String.Empty Then

            RadMessageBox.Show(Me, "Configurazioni incomplete o mancanti" & vbCrLf & vbCrLf & "Compila i campi e Salva le impostazioni", "Azione Richiesta!", MessageBoxButtons.OK, RadMessageIcon.Exclamation)

            RadButtonModificaSettings_Click(Nothing, Nothing)
            ValidSettings = False
            Return "Invalid"
        Else
            ValidSettings = True
            Return "Valid"
        End If
    End Function

    Private DT_Settings As DataTable = Nothing

    Private Sub InsertOrUpdateTableSettings()

        DT_Settings = DataBase.LoadDT_Settings(DB)
        Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
        If DT_Settings.Rows.Count = 0 Then

            Dim sqlInsertQuery As String = "INSERT INTO Settings(NomeUtente, Passw, PinCode, CodRegione, CodASL, CodSSA, cfProprietario, DefaultXmlPath) " &
                                   "VALUES ('" & RadTextBoxNomeUtente.Text.Trim & "', '" _
                                              & RadTextBoxPassword.Text.Trim & "', '" _
                                              & RadTextBoxPincode.Text.Trim & "', '" _
                                              & RadTextBoxCodRegione.Text.Trim & "', '" _
                                              & RadTextBoxCodASL.Text.Trim & "', '" _
                                              & RadTextBoxCodSSA.Text.Trim & "', '" _
                                              & RadTextBoxCfProprietario.Text.Trim & "', '" _
                                              & RadTextBoxPercorsoCreazioneXML.Text.Trim & "')"
            Dim cmdSQL = New OleDbCommand
            cmdSQL.CommandText = sqlInsertQuery

            cmdSQL.Connection = New OleDbConnection(ConnString)
            cmdSQL.Connection.Open()
            Try
                Dim righe As Integer = cmdSQL.ExecuteNonQuery
                cmdSQL.Connection.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Errore nel salvataggio ", "Errore")
                cmdSQL.Connection.Close()
                Exit Sub
            End Try

            RadButtonModificaSettings_Click(Nothing, Nothing)
            Exit Sub

        Else
            Dim cmdSQLUpdate = New OleDbCommand
            Dim sqlUpdateQuery As String = "UPDATE Settings Set " _
                                           & "NomeUtente = '" & RadTextBoxNomeUtente.Text.Trim & "', " _
                                           & "Passw = '" & RadTextBoxPassword.Text.Trim & "', " _
                                           & "PinCode = '" & RadTextBoxPincode.Text.Trim & "', " _
                                           & "CodRegione = '" & RadTextBoxCodRegione.Text.Trim & "', " _
                                           & "CodASL = '" & RadTextBoxCodASL.Text.Trim & "', " _
                                           & "CodSSA = '" & RadTextBoxCodSSA.Text.Trim & "', " _
                                           & "cfProprietario = '" & RadTextBoxCfProprietario.Text.Trim & "', " _
                                           & "DefaultXmlPath = '" & RadTextBoxPercorsoCreazioneXML.Text.Trim & "'"

            cmdSQLUpdate = New OleDbCommand
            cmdSQLUpdate.CommandText = sqlUpdateQuery
            cmdSQLUpdate.Connection = New OleDbConnection(ConnString)
            cmdSQLUpdate.Connection.Open()
            Try
                Dim righe As Integer = cmdSQLUpdate.ExecuteNonQuery
                cmdSQLUpdate.Connection.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Errore nel salvataggio ", "Errore")
                cmdSQLUpdate.Connection.Close()
                Exit Sub
            End Try

        End If
    End Sub

    Private Sub RadButtonVerificaConnessioneWEB_Click(sender As Object, e As EventArgs) Handles RadButtonVerificaConnessioneWEB.Click
        provaConnessioneWeb()
    End Sub

    Private Sub CreaSessioneSuDB(nowDate As DateTime, nowString As String, FileXml As String)


        DT_Settings = DataBase.LoadDT_SessioniById(DB, CInt(Me.RadTextBoxIdSessione.Text.Trim))
        Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
        Dim FattureDal As Date = FormatDateTime(RadTextBoxFattureDal.Text.Trim, DateFormat.ShortDate)
        Dim FattureAl As Date = FormatDateTime(RadTextBoxFattureAl.Text.Trim, DateFormat.ShortDate)

        If DT_Settings.Rows.Count = 0 Then

            Dim sqlInsertQuery As String = "INSERT INTO Sessioni(idEstrazione, Data, EstratteDal, EstratteAl, NumeroDal, NumeroAl, Stato, Protocollo, PercorsoXML) " &
                                   "VALUES (" & CInt(RadTextBoxIdSessione.Text.Trim) & ", '" _
                                              & nowDate & "', '" _
                                              & FattureDal & "', '" _
                                              & FattureAl & "', '" _
                                              & RadTextBoxNumFattureDal.Text.Trim & "', '" _
                                              & RadTextBoxNumFattureAl.Text.Trim & "', '" _
                                              & "XML creato e da Inviare" & "', NULL, '" & FileXml & "')"
            Dim cmdSQL = New OleDbCommand
            cmdSQL.CommandText = sqlInsertQuery

            cmdSQL.Connection = New OleDbConnection(ConnString)
            cmdSQL.Connection.Open()
            Try
                Dim righe As Integer = cmdSQL.ExecuteNonQuery
                cmdSQL.Connection.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Errore nel salvataggio ", "Errore")
                cmdSQL.Connection.Close()
                Exit Sub
            End Try

            Dim newSessione As New RigaSessione
            newSessione.idEstrazione = CInt(RadTextBoxIdSessione.Text.Trim)
            newSessione.DataEstrazione = nowDate.Date
            newSessione.DataDal = FattureDal
            newSessione.DataAl = FattureAl
            newSessione.NumeroDal = RadTextBoxNumFattureDal.Text.Trim
            newSessione.Numeroal = RadTextBoxNumFattureAl.Text.Trim
            newSessione.Stato = "XML creato e da Inviare"
            newSessione.PercorsoXML = FileXml
            SessioniList.Add(newSessione)

            RadButtonModificaSettings_Click(Nothing, Nothing)
            Exit Sub

        Else
            Dim cmdSQLUpdate = New OleDbCommand
            Exit Sub
            Dim sqlUpdateQuery As String = "UPDATE Sessioni Set " _
                                           & "EstratteDal = " & FattureDal & ", " _
                                           & "EstratteAl = " & FattureDal & ", " _
                                           & "NumeroDal = '" & RadTextBoxNumFattureDal.Text.Trim & "', " _
                                           & "NumeroAl = '" & RadTextBoxNumFattureAl.Text.Trim & "', " _
                                           & "Stato = 'XML creato e da Inviare', " _
                                           & "WHERE = EstratteDal = " & FattureDal & ", "



            cmdSQLUpdate = New OleDbCommand
            cmdSQLUpdate.CommandText = sqlUpdateQuery
            cmdSQLUpdate.Connection = New OleDbConnection(ConnString)
            cmdSQLUpdate.Connection.Open()
            Try
                Dim righe As Integer = cmdSQLUpdate.ExecuteNonQuery
                cmdSQLUpdate.Connection.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Errore nel salvataggio ", "Errore")
                cmdSQLUpdate.Connection.Close()
                Exit Sub
            End Try

            Dim newSessione As New RigaSessione
            newSessione.DataEstrazione = nowDate.Date
            newSessione.DataDal = FattureDal
            newSessione.DataAl = FattureAl
            newSessione.NumeroDal = RadTextBoxNumFattureDal.Text.Trim
            newSessione.Numeroal = RadTextBoxNumFattureAl.Text.Trim
            newSessione.Stato = "XML creato e da Inviare"

            SessioniList.Add(newSessione)



        End If

    End Sub

    Private Sub AggiornaSessioneSuDB(nowDate As DateTime, nowString As String)

        DT_Settings = DataBase.LoadDT_Sessioni(DB)
        Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
        Dim FattureDal As Date = FormatDateTime(RadTextBoxFattureDal.Text.Trim, DateFormat.ShortDate)
        Dim FattureAl As Date = FormatDateTime(RadTextBoxFattureAl.Text.Trim, DateFormat.ShortDate)

        If DT_Settings.Rows.Count = 0 Then

            Dim sqlInsertQuery As String = "INSERT INTO Sessioni(idEstrazione, Data, EstratteDal, EstratteAl, NumeroDal, NumeroAl, Stato, Protocollo) " &
                                   "VALUES (" & CInt(RadTextBoxIdSessione.Text.Trim) & ", " _
                                              & nowDate.Date & ", " _
                                              & FattureDal & ", " _
                                              & FattureAl & ", '" _
                                              & RadTextBoxNumFattureDal.Text.Trim & "', '" _
                                              & RadTextBoxNumFattureAl.Text.Trim & "', '" _
                                              & "XML creato e da Inviare" & "', NULL)"
            Dim cmdSQL = New OleDbCommand
            cmdSQL.CommandText = sqlInsertQuery

            cmdSQL.Connection = New OleDbConnection(ConnString)
            cmdSQL.Connection.Open()
            Try
                Dim righe As Integer = cmdSQL.ExecuteNonQuery
                cmdSQL.Connection.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Errore nel salvataggio ", "Errore")
                cmdSQL.Connection.Close()
                Exit Sub
            End Try

            Dim newSessione As New RigaSessione
            newSessione.DataEstrazione = nowDate.Date
            newSessione.DataDal = FattureDal
            newSessione.DataAl = FattureAl
            newSessione.NumeroDal = RadTextBoxNumFattureDal.Text.Trim
            newSessione.Numeroal = RadTextBoxNumFattureAl.Text.Trim
            newSessione.Stato = "XML creato e da Inviare"

            RadGridViewElencoSessioni.DataSource = SessioniList.OrderByDescending(Function(s) s.idEstrazione)


            RadButtonModificaSettings_Click(Nothing, Nothing)
            Exit Sub

        Else
            Dim cmdSQLUpdate = New OleDbCommand
            Exit Sub
            Dim sqlUpdateQuery As String = "UPDATE Sessioni Set " _
                                           & "EstratteDal = " & FattureDal & ", " _
                                           & "EstratteAl = " & FattureDal & ", " _
                                           & "NumeroDal = '" & RadTextBoxNumFattureDal.Text.Trim & "', " _
                                           & "NumeroAl = '" & RadTextBoxNumFattureAl.Text.Trim & "', " _
                                           & "Stato = 'XML creato e da Inviare', " _
                                           & "WHERE = EstratteDal = " & FattureDal & ", "



            cmdSQLUpdate = New OleDbCommand
            cmdSQLUpdate.CommandText = sqlUpdateQuery
            cmdSQLUpdate.Connection = New OleDbConnection(ConnString)
            cmdSQLUpdate.Connection.Open()
            Try
                Dim righe As Integer = cmdSQLUpdate.ExecuteNonQuery
                cmdSQLUpdate.Connection.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Errore nel salvataggio ", "Errore")
                cmdSQLUpdate.Connection.Close()
                Exit Sub
            End Try

            Dim newSessione As New RigaSessione
            newSessione.DataDal = FattureDal
            newSessione.DataAl = FattureAl
            newSessione.NumeroDal = RadTextBoxNumFattureDal.Text.Trim
            newSessione.Numeroal = RadTextBoxNumFattureAl.Text.Trim
            newSessione.Stato = "XML creato e da Inviare"

            SessioniList.Add(newSessione)



        End If

    End Sub

    Private Class RigaSessione
        Public Property idEstrazione As Integer
        Public Property DataEstrazione As Date
        Public Property DataInvio As Date
        Public Property Protocollo As String
        Public Property Stato As String
        Public Property DataDal As Date
        Public Property DataAl As Date
        Public Property NumeroDal As String
        Public Property Numeroal As String
        Public Property EsitoElaborazione As String
        Public Property PercorsoXML As String
    End Class

    Private Sub deleteFromZtable()
        WaitingDialog.ShowDialog("Pulizia dati in linea")

        Dim cmdSQLUpdate = New OleDbCommand
        Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
        Dim sqlUpdateQuery As String = "DELETE FROM zTableInvio_SS"
        cmdSQLUpdate = New OleDbCommand
        cmdSQLUpdate.CommandText = sqlUpdateQuery
        cmdSQLUpdate.Connection = New OleDbConnection(ConnString)
        cmdSQLUpdate.Connection.Open()
        Try
            Dim righe As Integer = cmdSQLUpdate.ExecuteNonQuery
            cmdSQLUpdate.Connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Errore nella cancellazione di vecchie sessioni in linea", "Errore")
            cmdSQLUpdate.Connection.Close()
            Exit Sub
        End Try
        WaitingDialog.CloseDialog()
    End Sub

    Private Sub loadVecchieSessioni()
        Try

            DT_Settings = DataBase.LoadDT_Sessioni(DB)
            Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"

            If DT_Settings.Rows.Count > 0 Then

                For Each r As DataRow In DT_Settings.Rows
                    Dim newSessione As New RigaSessione
                    newSessione.idEstrazione = r("idEstrazione")
                    newSessione.DataEstrazione = r("Data")
                    newSessione.DataInvio = IIf(r("DataInvio") IsNot DBNull.Value, r("DataInvio"), Nothing)
                    newSessione.DataDal = r("EstratteDal")
                    newSessione.DataAl = r("EstratteAl")
                    newSessione.NumeroDal = r("NumeroDal")
                    newSessione.Numeroal = r("NumeroAl")
                    newSessione.Stato = r("Stato")
                    newSessione.Protocollo = IIf(r("Protocollo") IsNot DBNull.Value, r("Protocollo"), Nothing)
                    newSessione.EsitoElaborazione = IIf(r("EsitoElaborazione") IsNot DBNull.Value, r("EsitoElaborazione"), Nothing)
                    newSessione.PercorsoXML = IIf(r("PercorsoXML") IsNot DBNull.Value, r("PercorsoXML"), Nothing)
                    SessioniList.Add(newSessione)
                Next
            End If
            RadGridViewElencoSessioni.DataSource = Nothing
            RadGridViewElencoSessioni.DataSource = SessioniList.OrderByDescending(Function(s) s.idEstrazione)
        Catch ex As Exception
            RadMessageBox.Show(Me, "Errore nel database " & DB & vbCrLf & vbCrLf &
                               ex.ToString, "Attenzione!", MessageBoxButtons.OK, RadMessageIcon.Exclamation)

        End Try

    End Sub

    Private Sub RadGridViewElencoSessioni_CommandCellClick(sender As Object, e As GridViewCellEventArgs)
        Cursor = Cursors.WaitCursor
        Select Case e.Column.Name
            Case "XMLButton"
                If e.Row.DataBoundItem.Stato = "XML creato e da Inviare" Or e.Row.DataBoundItem.Stato = "XML Inviato" Then
                    Dim XML_Path = e.Row.DataBoundItem.PercorsoXML
                    Shell("C:\Program Files\Internet Explorer\IEXPLORE.EXE " & XML_Path & ".xml", vbNormalNoFocus)
                    Exit Select
                Else
                    'RadMessageBox.Show(Me, "File XML non ancora inviato." & vbCrLf & vbCrLf & " il file prima di visualizzarlo", "Errore", MessageBoxButtons.OK, RadMessageIcon.Error)
                    Exit Select
                End If

            Case "RicevutaButton"
                If e.Row.DataBoundItem.Protocollo Is Nothing Then
                    RadMessageBox.Show(Me, "File XML non ancora inviato." & vbCrLf & vbCrLf & "Inviare il file prima di richiedere la ricevuta", "Errore", MessageBoxButtons.OK, RadMessageIcon.Error)
                    Exit Select
                End If
                scaricaRicevuta()

            Case "csvButton"
                If e.Row.DataBoundItem.Protocollo Is Nothing Then
                    RadMessageBox.Show(Me, "File XML non ancora inviato." & vbCrLf & vbCrLf & "Inviare il file prima di richiedere l'elenco errori", "Errore", MessageBoxButtons.OK, RadMessageIcon.Error)
                    Exit Select
                End If
                richiestaCSV()

            Case "filesButton"
                Dim XML_Path = e.Row.DataBoundItem.PercorsoXML
                XML_Path = Strings.Left(XML_Path, InStrRev(XML_Path, "\"))
                If IO.Directory.Exists(XML_Path) Then
                    Dim Proc As String = "explorer.exe"

                    Process.Start(XML_Path)
                End If

            Case "esitoButton"
                If e.Row.DataBoundItem.Protocollo Is Nothing Then
                    RadMessageBox.Show(Me, "File XML non ancora inviato." & vbCrLf & vbCrLf & "Inviare il file prima di richiedere l'esito dell'invio", "Errore", MessageBoxButtons.OK, RadMessageIcon.Error)
                    Exit Select
                End If
                richiestaEsitoInvio()
        End Select

        Cursor = Cursors.Default
    End Sub

    Private Sub RadGridViewElencoSessioni_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewElencoSessioni.CellFormatting
        Select Case e.Column.Name
            Case "Data"
                Dim dateColumn As GridViewDateTimeColumn = Me.RadGridViewElencoSessioni.Columns(e.Column.Name)
                Dim pippo = e.CellElement.Value
                dateColumn.FormatString = "{0:dd-MMM-yyyy HH:mm:ss}"
            Case "EstratteDal", "EstratteAl"
                Dim dateColumn As GridViewDateTimeColumn = Me.RadGridViewElencoSessioni.Columns(e.Column.Name)
                dateColumn.FormatString = "{0:dd-MMM-yyyy}"

            Case "DataInvio"
                Dim dateColumn As GridViewDateTimeColumn = Me.RadGridViewElencoSessioni.Columns(e.Column.Name)
                dateColumn.FormatString = "{0:dd-MMM-yyyy HH:mm:ss}"
                If e.CellElement.Value = "01/01/0001" Then
                    e.CellElement.Text = ""
                    'e.CellElement.TableElement.CurrentCell.Visibility = ElementVisibility.Hidden
                Else
                    e.CellElement.Visibility = ElementVisibility.Visible
                End If

            Case "Stato"
                If e.Row.DataBoundItem.Stato = "XML Inviato" Then
                    e.CellElement.DrawFill = True
                    e.CellElement.ForeColor = Color.White
                    e.CellElement.NumberOfColors = 1
                    e.CellElement.BackColor = Color.OrangeRed
                Else
                    e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local)
                    e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local)
                    e.CellElement.ResetValue(LightVisualElement.NumberOfColorsProperty, ValueResetFlags.Local)
                    e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local)
                End If

        End Select

    End Sub

    Private Sub inserisciInStorico()
        WaitingDialog.ShowDialog("Inserimento in Storico")
        FP = New FileProcesser(File, DB)
        'Throw New NotImplementedException
        FP.StartProcessingStoricizza()
        FP.UpDataDBStorico()
        WaitingDialog.CloseDialog()
    End Sub

    Private Sub updateProtocollo(protocollo As String, currentSessione As RigaSessione, Ricevuta As InvioTelematicoSpeseSanitarie730p.ricevutaInvio)
        Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
        Dim cmdSQLUpdate = New OleDbCommand
        Dim Esito As String = CorreggiStringa(Ricevuta.codiceEsito & " - " & Ricevuta.descrizioneEsito)
        'Esito = CorreggiStringa(Esito)
        Dim sqlUpdateQuery As String = "UPDATE Sessioni Set Protocollo = " & protocollo & ", Stato = 'XML Inviato', EsitoElaborazione = '" & Esito &
            "', DataInvio = #" & Now & "# WHERE idEstrazione = " & currentSessione.idEstrazione
        cmdSQLUpdate = New OleDbCommand
        cmdSQLUpdate.CommandText = sqlUpdateQuery
        cmdSQLUpdate.Connection = New OleDbConnection(ConnString)
        cmdSQLUpdate.Connection.Open()
        Try
            Dim righe As Integer = cmdSQLUpdate.ExecuteNonQuery
            cmdSQLUpdate.Connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Problema sull' aggiornamento della sessione", "Errore")
            cmdSQLUpdate.Connection.Close()
            Exit Sub
        End Try
    End Sub

    Private Sub updateEsito(currentSessione As RigaSessione, Esito As String)
        Esito = CorreggiStringa(Esito)
        Dim ConnString As String = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DB) + ";"
        Dim cmdSQLUpdate = New OleDbCommand
        Dim sqlUpdateQuery As String = "UPDATE Sessioni Set EsitoElaborazione = '" & Esito & "' WHERE idEstrazione = " & currentSessione.idEstrazione
        cmdSQLUpdate = New OleDbCommand
        cmdSQLUpdate.CommandText = sqlUpdateQuery
        cmdSQLUpdate.Connection = New OleDbConnection(ConnString)
        cmdSQLUpdate.Connection.Open()
        Try
            Dim righe As Integer = cmdSQLUpdate.ExecuteNonQuery
            cmdSQLUpdate.Connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Problema sull' aggiornamento della sessione", "Errore")
            cmdSQLUpdate.Connection.Close()
            Exit Sub
        End Try
    End Sub

    Public Shared Sub WriteActivityLog(ByVal Text As String)
        If Not IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory & "Log\") Then
            IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory & "Log\")
        End If
        Dim ActivityLog_FileName As String = AppDomain.CurrentDomain.BaseDirectory & "Log\" & Format(Now, "dd.MM.yyyy") & "_ActivityLog.txt"
        My.Computer.FileSystem.WriteAllText(ActivityLog_FileName, Now & " - " & Text & vbCrLf, True)
    End Sub

    Private Function CorreggiStringa(ByVal st As String) As String
        Dim P As Integer = 1
        Dim Q As Integer
        Do While InStr(P, st, "'") > 0
            Q = InStr(P, st, "'")
            st = Mid(st, 1, Q) & Mid(st, Q)
            P = Q + 2
        Loop
        Return st
    End Function


    Private Sub xmlSettingsValidationEventHandler(ByVal sender As Object, ByVal e As ValidationEventArgs)

        If e.Severity = XmlSeverityType.Warning Then
            RadMessageBox.Show("Attenzione! Presenti segnalazioni di validazione del file XML" & vbCrLf & vbCrLf & e.Message)
            isXMLvalid = False
            Exit Sub
        ElseIf e.Severity = XmlSeverityType.Error Then
            RadMessageBox.Show("Attenzione! Presenti errori di validazione del file XML" & vbCrLf & vbCrLf & e.Message)
            isXMLvalid = False
            Exit Sub
        End If
        isXMLvalid = True

    End Sub

    Private Sub validaXML(Xml_File As String)

        isXMLvalid = True

        Dim schemaPath As String = Application.StartupPath & "\TS\"
        Dim documentoSettings As XmlReaderSettings = New XmlReaderSettings()
        documentoSettings.Schemas.Add(Nothing, "TS\730_precompilata.xsd")
        documentoSettings.ValidationType = ValidationType.Schema
        AddHandler documentoSettings.ValidationEventHandler, New ValidationEventHandler(AddressOf xmlSettingsValidationEventHandler)



        Dim fatture As XmlReader = XmlReader.Create(Xml_File, documentoSettings)

        While fatture.Read()

        End While




    End Sub

End Class



Public Class BasicAuthenticationBehavior
    Implements IEndpointBehavior
    Private ReadOnly _username As String
    Private ReadOnly _password As String
    Public Sub New(username As String, password As String)
        'raccolgo user e password che passerò poi al costruttore di IClientMessageInspector
        _username = username
        _password = password
    End Sub

    Public Sub AddBindingParameters(endpoint As ServiceEndpoint, bindingParameters As System.ServiceModel.Channels.BindingParameterCollection) Implements IEndpointBehavior.AddBindingParameters
        Return
    End Sub

    Public Sub ApplyClientBehavior(endpoint As ServiceEndpoint, clientRuntime As ClientRuntime) Implements IEndpointBehavior.ApplyClientBehavior
        'aggiungo un'istanza dell'inspector
        clientRuntime.MessageInspectors.Add(New BasicAuthenticationInspector(_username, _password))
    End Sub

    Public Sub ApplyDispatchBehavior(endpoint As ServiceEndpoint, endpointDispatcher As EndpointDispatcher) Implements IEndpointBehavior.ApplyDispatchBehavior
        Return
    End Sub

    Public Sub Validate(endpoint As ServiceEndpoint) Implements IEndpointBehavior.Validate
        Return
    End Sub
End Class


Public Class BasicAuthenticationInspector
    Implements IClientMessageInspector

    Private ReadOnly _authorization As String
    Public Sub New(username As String, password As String)
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            Throw New ArgumentException("Devi fornire username e password")
        End If
        _authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", username, password)))
    End Sub

    Public Function BeforeSendRequest(ByRef messaggio As System.ServiceModel.Channels.Message, channel As System.ServiceModel.IClientChannel) As Object Implements IClientMessageInspector.BeforeSendRequest
        'ottengo un riferimento alla richiesta http sottostante
        Dim richiestaHttp = messaggio.Properties.Values.OfType(Of HttpRequestMessageProperty)().FirstOrDefault()
        If richiestaHttp IsNot Nothing Then
            richiestaHttp.Headers.Add("Authorization", _authorization)
        Else
            richiestaHttp = New HttpRequestMessageProperty()
            richiestaHttp.Headers.Add("Authorization", _authorization)
            messaggio.Properties.Add(HttpRequestMessageProperty.Name, richiestaHttp)
        End If

        Return messaggio
    End Function

    Public Sub AfterReceiveReply(ByRef reply As System.ServiceModel.Channels.Message, correlationState As Object) Implements IClientMessageInspector.AfterReceiveReply
        'non ci interessa esaminare la risposta
    End Sub

End Class

