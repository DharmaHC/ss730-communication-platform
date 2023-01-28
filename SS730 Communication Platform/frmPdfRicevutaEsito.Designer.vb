<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPdfRicevutaEsito
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.RadPdfViewer1 = New Telerik.WinControls.UI.RadPdfViewer()
        Me.RadPdfViewerNavigator1 = New Telerik.WinControls.UI.RadPdfViewerNavigator()
        CType(Me.RadPdfViewer1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPdfViewerNavigator1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPdfViewer1
        '
        Me.RadPdfViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPdfViewer1.Location = New System.Drawing.Point(0, 0)
        Me.RadPdfViewer1.Name = "RadPdfViewer1"
        Me.RadPdfViewer1.Size = New System.Drawing.Size(1044, 524)
        Me.RadPdfViewer1.TabIndex = 1
        Me.RadPdfViewer1.Text = "RadPdfViewer1"
        Me.RadPdfViewer1.ThumbnailsScaleFactor = 0.15!
        '
        'RadPdfViewerNavigator1
        '
        Me.RadPdfViewerNavigator1.AssociatedViewer = Me.RadPdfViewer1
        Me.RadPdfViewerNavigator1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPdfViewerNavigator1.Location = New System.Drawing.Point(0, 0)
        Me.RadPdfViewerNavigator1.Name = "RadPdfViewerNavigator1"
        Me.RadPdfViewerNavigator1.Size = New System.Drawing.Size(1044, 38)
        Me.RadPdfViewerNavigator1.TabIndex = 2
        Me.RadPdfViewerNavigator1.Text = "RadPdfViewerNavigator1"
        '
        'frmPdfRicevutaEsito
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1044, 524)
        Me.Controls.Add(Me.RadPdfViewerNavigator1)
        Me.Controls.Add(Me.RadPdfViewer1)
        Me.Name = "frmPdfRicevutaEsito"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "Ricevuta Esito"
        CType(Me.RadPdfViewer1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPdfViewerNavigator1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RadPdfViewer1 As Telerik.WinControls.UI.RadPdfViewer
    Friend WithEvents RadPdfViewerNavigator1 As Telerik.WinControls.UI.RadPdfViewerNavigator
End Class
