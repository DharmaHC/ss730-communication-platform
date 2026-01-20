# SS730 Communication Platform

Applicazione Windows Forms per la comunicazione telematica delle spese sanitarie al Sistema Tessera Sanitaria (TS) del Ministero dell'Economia e delle Finanze (MEF), ai fini della dichiarazione dei redditi precompilata (730).

## Scopo

L'applicazione consente alle strutture sanitarie di:

1. **Importare dati** da file di testo ASCII contenenti le informazioni sulle prestazioni sanitarie erogate
2. **Generare file XML** conformi allo schema XSD del Sistema TS (730_precompilata.xsd)
3. **Inviare telematicamente** i file XML ai servizi web del Sistema Tessera Sanitaria
4. **Gestire le sessioni di invio** con tracciamento dello stato (creato, inviato, elaborato)
5. **Scaricare ricevute ed esiti** degli invii effettuati (PDF, CSV errori)

## Architettura

### Tecnologie utilizzate

- **Linguaggio**: VB.NET
- **Framework**: .NET Framework 4.8
- **UI**: Windows Forms con componenti Telerik UI for WinForms
- **Database locale**: Microsoft Access (MDB) per persistenza sessioni e impostazioni
- **Comunicazione**: WCF (Windows Communication Foundation) con servizi SOAP

### Servizi Web integrati

L'applicazione si interfaccia con i seguenti servizi del Sistema TS:

| Servizio | Descrizione |
|----------|-------------|
| `InvioTelematicoSpeseSanitarie730p` | Invio file XML delle spese sanitarie (MTOM) |
| `EsitoInvioDatiSpesa730Service` | Richiesta esito elaborazione invio |
| `DettaglioErrori730Service` | Download dettaglio errori in formato CSV |
| `RicevutaPdf730Service` | Download ricevuta invio in formato PDF |

### Struttura del progetto

```
SS730 Communication Platform/
├── Classes/
│   ├── Database.vb          # Accesso dati Access
│   ├── FileProcesser.vb     # Elaborazione file ASCII
│   ├── SchemaXML.vb         # Modello dati per XML
│   └── zTableInvioSS.vb     # Entita tabella invio
├── Service References/      # Proxy WCF generati
│   ├── DettaglioErrori730Service/
│   ├── EsitoInvioDatiSpesa730Service/
│   ├── InvioTelematicoSpeseSanitarie730p/
│   └── RicevutaPdf730Service/
├── mainForm.vb              # Form principale
├── frmPdfRicevutaEsito.vb   # Visualizzatore PDF ricevute
├── WaitingDialog.vb         # Dialog di attesa
└── App.config               # Configurazione (non tracciato)
```

## Configurazione

### Prerequisiti

- Windows con .NET Framework 4.8
- Certificato `SanitelCF.cer` per la cifratura dei dati sensibili (CF, pincode)
- Credenziali di accesso al Sistema TS (username, password, pincode)

### Setup iniziale

1. **Configurazione applicazione**:
   - Copiare `SS730 Communication Platform/App.config.example` in `SS730 Communication Platform/App.config`
   - Configurare in `App.config`:
     - Credenziali utente (NomeUtente, Password, Pincode)
     - Codice fiscale proprietario
     - Codici identificativi struttura (Regione, ASL, SSA)
     - Percorso predefinito per creazione XML

2. **Database locale**:
   - Copiare `SS730 Communication Platform/Resources/TS730_Data_Example.mdb` nella cartella di output della build (es. `bin/Debug/` o `bin/Release/`)
   - Rinominare il file in `TS730_Data.mdb`

   Oppure, in alternativa, da riga di comando:
   ```batch
   copy "SS730 Communication Platform\Resources\TS730_Data_Example.mdb" "SS730 Communication Platform\bin\Debug\TS730_Data.mdb"
   ```

### Ambienti

L'applicazione supporta due ambienti (configurabili in App.config):

- **Test**: `https://invioSS730pTest.sanita.finanze.it/...`
- **Produzione**: `https://invioSS730p.sanita.finanze.it/...`

## Utilizzo

### Workflow tipico

1. **Impostazioni**: Verificare/configurare le credenziali e i parametri della struttura
2. **Preparazione**:
   - Selezionare il file ASCII con i dati delle prestazioni
   - Caricare i dati in tabella temporanea
   - Verificare i dati importati
3. **Generazione XML**: Creare il file XML validato secondo lo schema XSD
4. **Invio**: Trasmettere il file XML al Sistema TS
5. **Verifica**: Controllare l'esito dell'elaborazione e scaricare eventuali errori

### Gestione sessioni

La griglia delle sessioni permette di:
- Visualizzare lo storico degli invii
- Controllare lo stato di ogni sessione
- Scaricare ricevute PDF
- Scaricare elenco errori CSV
- Aprire i file XML generati

## Sicurezza

- I dati sensibili (codice fiscale cittadino, pincode) vengono cifrati con RSA prima dell'invio
- Le credenziali di accesso sono gestite tramite Basic Authentication su canale HTTPS (TLS 1.2)
- Il file `App.config` contenente le credenziali e escluso dal version control

## Note tecniche

- La validazione XML avviene contro lo schema `730_precompilata.xsd` prima dell'invio
- I file XML vengono compressi in formato ZIP prima della trasmissione (MTOM)
- Il database Access locale (`TS730_Data.mdb`) mantiene lo storico delle sessioni

## Licenza

Software proprietario - Tutti i diritti riservati.
