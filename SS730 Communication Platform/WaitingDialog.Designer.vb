<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WaitingDialog
    Inherits Telerik.WinControls.UI.ShapedForm

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
        Me.components = New System.ComponentModel.Container()
        Me.RoundRectShapeForm = New Telerik.WinControls.RoundRectShape(Me.components)
        Me.RadWaitingBarMain = New Telerik.WinControls.UI.RadWaitingBar()
        Me.labelMessage = New Telerik.WinControls.UI.RadLabel()
        CType(Me.RadWaitingBarMain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.labelMessage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadWaitingBarMain
        '
        Me.RadWaitingBarMain.Location = New System.Drawing.Point(60, 172)
        Me.RadWaitingBarMain.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadWaitingBarMain.Name = "RadWaitingBarMain"
        Me.RadWaitingBarMain.Size = New System.Drawing.Size(512, 37)
        Me.RadWaitingBarMain.TabIndex = 0
        Me.RadWaitingBarMain.Text = "RadWaitingBar1"
        '
        'labelMessage
        '
        Me.labelMessage.Location = New System.Drawing.Point(60, 63)
        Me.labelMessage.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.labelMessage.Name = "labelMessage"
        Me.labelMessage.Size = New System.Drawing.Size(146, 34)
        Me.labelMessage.TabIndex = 1
        Me.labelMessage.Text = "labelMessage"
        '
        'WaitingDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 283)
        Me.Controls.Add(Me.labelMessage)
        Me.Controls.Add(Me.RadWaitingBarMain)
        Me.Margin = New System.Windows.Forms.Padding(12, 12, 12, 12)
        Me.Name = "WaitingDialog"
        Me.Opacity = 0.8R
        Me.Shape = Me.RoundRectShapeForm
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WaitingDialog"
        CType(Me.RadWaitingBarMain, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.labelMessage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RoundRectShapeForm As Telerik.WinControls.RoundRectShape
    Friend WithEvents RadWaitingBarMain As Telerik.WinControls.UI.RadWaitingBar
    Friend WithEvents labelMessage As Telerik.WinControls.UI.RadLabel
End Class

