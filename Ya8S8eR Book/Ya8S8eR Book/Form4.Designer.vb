<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTools
    Inherits System.Windows.Forms.Form

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
        Me.ButtonRUN = New System.Windows.Forms.Button()
        Me.ListBoxTools = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'ButtonRUN
        '
        Me.ButtonRUN.Location = New System.Drawing.Point(12, 193)
        Me.ButtonRUN.Name = "ButtonRUN"
        Me.ButtonRUN.Size = New System.Drawing.Size(212, 44)
        Me.ButtonRUN.TabIndex = 2
        Me.ButtonRUN.Text = "RUN"
        Me.ButtonRUN.UseVisualStyleBackColor = True
        '
        'ListBoxTools
        '
        Me.ListBoxTools.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxTools.FormattingEnabled = True
        Me.ListBoxTools.ItemHeight = 19
        Me.ListBoxTools.Location = New System.Drawing.Point(12, 12)
        Me.ListBoxTools.Name = "ListBoxTools"
        Me.ListBoxTools.Size = New System.Drawing.Size(212, 175)
        Me.ListBoxTools.TabIndex = 3
        '
        'FormTools
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(236, 238)
        Me.Controls.Add(Me.ListBoxTools)
        Me.Controls.Add(Me.ButtonRUN)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormTools"
        Me.ShowIcon = False
        Me.Text = "Tools"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonRUN As System.Windows.Forms.Button
    Friend WithEvents ListBoxTools As System.Windows.Forms.ListBox
End Class
