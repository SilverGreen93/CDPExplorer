<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.optSaveSkip = New System.Windows.Forms.RadioButton()
        Me.optSaveOverwrite = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmbPart3 = New System.Windows.Forms.ComboBox()
        Me.cmbPart2 = New System.Windows.Forms.ComboBox()
        Me.cmbPart1 = New System.Windows.Forms.ComboBox()
        Me.chkUnderscore = New System.Windows.Forms.CheckBox()
        Me.lblExample = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.optSaveRename = New System.Windows.Forms.RadioButton()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(51, 299)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(128, 36)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(216, 299)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(128, 36)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.optSaveSkip)
        Me.GroupBox1.Controls.Add(Me.optSaveOverwrite)
        Me.GroupBox1.Controls.Add(Me.optSaveRename)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 179)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(372, 109)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "File conflict policy"
        '
        'optSaveSkip
        '
        Me.optSaveSkip.AutoSize = True
        Me.optSaveSkip.Location = New System.Drawing.Point(22, 73)
        Me.optSaveSkip.Name = "optSaveSkip"
        Me.optSaveSkip.Size = New System.Drawing.Size(170, 20)
        Me.optSaveSkip.TabIndex = 1
        Me.optSaveSkip.TabStop = True
        Me.optSaveSkip.Text = "Skip file if already exists"
        Me.optSaveSkip.UseVisualStyleBackColor = True
        '
        'optSaveOverwrite
        '
        Me.optSaveOverwrite.AutoSize = True
        Me.optSaveOverwrite.Location = New System.Drawing.Point(22, 47)
        Me.optSaveOverwrite.Name = "optSaveOverwrite"
        Me.optSaveOverwrite.Size = New System.Drawing.Size(199, 20)
        Me.optSaveOverwrite.TabIndex = 0
        Me.optSaveOverwrite.TabStop = True
        Me.optSaveOverwrite.Text = "Overwrite file if already exists"
        Me.optSaveOverwrite.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cmbPart3)
        Me.GroupBox2.Controls.Add(Me.cmbPart2)
        Me.GroupBox2.Controls.Add(Me.cmbPart1)
        Me.GroupBox2.Controls.Add(Me.chkUnderscore)
        Me.GroupBox2.Controls.Add(Me.lblExample)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(372, 161)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "CDP file export name"
        '
        'cmbPart3
        '
        Me.cmbPart3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPart3.FormattingEnabled = True
        Me.cmbPart3.Items.AddRange(New Object() {"KUID", "username", "build", "(none)"})
        Me.cmbPart3.Location = New System.Drawing.Point(238, 32)
        Me.cmbPart3.Name = "cmbPart3"
        Me.cmbPart3.Size = New System.Drawing.Size(108, 24)
        Me.cmbPart3.TabIndex = 13
        '
        'cmbPart2
        '
        Me.cmbPart2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPart2.FormattingEnabled = True
        Me.cmbPart2.Items.AddRange(New Object() {"KUID", "username", "build", "(none)"})
        Me.cmbPart2.Location = New System.Drawing.Point(124, 32)
        Me.cmbPart2.Name = "cmbPart2"
        Me.cmbPart2.Size = New System.Drawing.Size(108, 24)
        Me.cmbPart2.TabIndex = 12
        '
        'cmbPart1
        '
        Me.cmbPart1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPart1.FormattingEnabled = True
        Me.cmbPart1.Items.AddRange(New Object() {"KUID", "username", "build", "(none)"})
        Me.cmbPart1.Location = New System.Drawing.Point(10, 32)
        Me.cmbPart1.Name = "cmbPart1"
        Me.cmbPart1.Size = New System.Drawing.Size(108, 24)
        Me.cmbPart1.TabIndex = 11
        '
        'chkUnderscore
        '
        Me.chkUnderscore.AutoSize = True
        Me.chkUnderscore.Location = New System.Drawing.Point(10, 70)
        Me.chkUnderscore.Name = "chkUnderscore"
        Me.chkUnderscore.Size = New System.Drawing.Size(241, 20)
        Me.chkUnderscore.TabIndex = 10
        Me.chkUnderscore.Text = "Use underscores instead of spaces"
        Me.chkUnderscore.UseVisualStyleBackColor = True
        '
        'lblExample
        '
        Me.lblExample.AutoSize = True
        Me.lblExample.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblExample.Location = New System.Drawing.Point(6, 130)
        Me.lblExample.Name = "lblExample"
        Me.lblExample.Size = New System.Drawing.Size(170, 20)
        Me.lblExample.TabIndex = 9
        Me.lblExample.Text = "kuid 1234 5678.cdp"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 100)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(163, 20)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Example filename:"
        '
        'optSaveRename
        '
        Me.optSaveRename.AutoSize = True
        Me.optSaveRename.Location = New System.Drawing.Point(22, 21)
        Me.optSaveRename.Name = "optSaveRename"
        Me.optSaveRename.Size = New System.Drawing.Size(220, 20)
        Me.optSaveRename.TabIndex = 2
        Me.optSaveRename.TabStop = True
        Me.optSaveRename.Text = "Auto-rename file if already exists"
        Me.optSaveRename.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(397, 347)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Settings"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents optSaveSkip As RadioButton
    Friend WithEvents optSaveOverwrite As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents lblExample As Label
    Friend WithEvents cmbPart3 As ComboBox
    Friend WithEvents cmbPart2 As ComboBox
    Friend WithEvents cmbPart1 As ComboBox
    Friend WithEvents chkUnderscore As CheckBox
    Friend WithEvents optSaveRename As RadioButton
End Class
