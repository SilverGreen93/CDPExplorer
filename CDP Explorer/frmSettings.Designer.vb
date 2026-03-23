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
        Me.optSaveRename = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmbPart3 = New System.Windows.Forms.ComboBox()
        Me.cmbPart2 = New System.Windows.Forms.ComboBox()
        Me.cmbPart1 = New System.Windows.Forms.ComboBox()
        Me.chkUnderscore = New System.Windows.Forms.CheckBox()
        Me.lblExample = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkKuid2 = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(38, 269)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(96, 29)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(162, 269)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(96, 29)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.optSaveSkip)
        Me.GroupBox1.Controls.Add(Me.optSaveOverwrite)
        Me.GroupBox1.Controls.Add(Me.optSaveRename)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 171)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox1.Size = New System.Drawing.Size(279, 89)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "File conflict policy"
        '
        'optSaveSkip
        '
        Me.optSaveSkip.AutoSize = True
        Me.optSaveSkip.Location = New System.Drawing.Point(8, 59)
        Me.optSaveSkip.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.optSaveSkip.Name = "optSaveSkip"
        Me.optSaveSkip.Size = New System.Drawing.Size(136, 17)
        Me.optSaveSkip.TabIndex = 1
        Me.optSaveSkip.TabStop = True
        Me.optSaveSkip.Text = "Skip file if already exists"
        Me.optSaveSkip.UseVisualStyleBackColor = True
        '
        'optSaveOverwrite
        '
        Me.optSaveOverwrite.AutoSize = True
        Me.optSaveOverwrite.Location = New System.Drawing.Point(8, 38)
        Me.optSaveOverwrite.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.optSaveOverwrite.Name = "optSaveOverwrite"
        Me.optSaveOverwrite.Size = New System.Drawing.Size(160, 17)
        Me.optSaveOverwrite.TabIndex = 0
        Me.optSaveOverwrite.TabStop = True
        Me.optSaveOverwrite.Text = "Overwrite file if already exists"
        Me.optSaveOverwrite.UseVisualStyleBackColor = True
        '
        'optSaveRename
        '
        Me.optSaveRename.AutoSize = True
        Me.optSaveRename.Location = New System.Drawing.Point(8, 17)
        Me.optSaveRename.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.optSaveRename.Name = "optSaveRename"
        Me.optSaveRename.Size = New System.Drawing.Size(175, 17)
        Me.optSaveRename.TabIndex = 2
        Me.optSaveRename.TabStop = True
        Me.optSaveRename.Text = "Auto-rename file if already exists"
        Me.optSaveRename.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkKuid2)
        Me.GroupBox2.Controls.Add(Me.cmbPart3)
        Me.GroupBox2.Controls.Add(Me.cmbPart2)
        Me.GroupBox2.Controls.Add(Me.cmbPart1)
        Me.GroupBox2.Controls.Add(Me.chkUnderscore)
        Me.GroupBox2.Controls.Add(Me.lblExample)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(9, 10)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox2.Size = New System.Drawing.Size(279, 157)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "CDP file export name"
        '
        'cmbPart3
        '
        Me.cmbPart3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPart3.FormattingEnabled = True
        Me.cmbPart3.Items.AddRange(New Object() {"KUID", "username", "build", "(none)"})
        Me.cmbPart3.Location = New System.Drawing.Point(178, 26)
        Me.cmbPart3.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmbPart3.Name = "cmbPart3"
        Me.cmbPart3.Size = New System.Drawing.Size(82, 21)
        Me.cmbPart3.TabIndex = 13
        '
        'cmbPart2
        '
        Me.cmbPart2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPart2.FormattingEnabled = True
        Me.cmbPart2.Items.AddRange(New Object() {"KUID", "username", "build", "(none)"})
        Me.cmbPart2.Location = New System.Drawing.Point(93, 26)
        Me.cmbPart2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmbPart2.Name = "cmbPart2"
        Me.cmbPart2.Size = New System.Drawing.Size(82, 21)
        Me.cmbPart2.TabIndex = 12
        '
        'cmbPart1
        '
        Me.cmbPart1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPart1.FormattingEnabled = True
        Me.cmbPart1.Items.AddRange(New Object() {"KUID", "username", "build", "(none)"})
        Me.cmbPart1.Location = New System.Drawing.Point(8, 26)
        Me.cmbPart1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmbPart1.Name = "cmbPart1"
        Me.cmbPart1.Size = New System.Drawing.Size(82, 21)
        Me.cmbPart1.TabIndex = 11
        '
        'chkUnderscore
        '
        Me.chkUnderscore.AutoSize = True
        Me.chkUnderscore.Location = New System.Drawing.Point(8, 57)
        Me.chkUnderscore.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkUnderscore.Name = "chkUnderscore"
        Me.chkUnderscore.Size = New System.Drawing.Size(192, 17)
        Me.chkUnderscore.TabIndex = 10
        Me.chkUnderscore.Text = "Use underscores instead of spaces"
        Me.chkUnderscore.UseVisualStyleBackColor = True
        '
        'lblExample
        '
        Me.lblExample.AutoSize = True
        Me.lblExample.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblExample.Location = New System.Drawing.Point(5, 126)
        Me.lblExample.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblExample.Name = "lblExample"
        Me.lblExample.Size = New System.Drawing.Size(138, 16)
        Me.lblExample.TabIndex = 9
        Me.lblExample.Text = "kuid 1234 5678.cdp"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 103)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(134, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Example filename:"
        '
        'chkKuid2
        '
        Me.chkKuid2.AutoSize = True
        Me.chkKuid2.Location = New System.Drawing.Point(8, 78)
        Me.chkKuid2.Margin = New System.Windows.Forms.Padding(2)
        Me.chkKuid2.Name = "chkKuid2"
        Me.chkKuid2.Size = New System.Drawing.Size(137, 17)
        Me.chkKuid2.TabIndex = 14
        Me.chkKuid2.Text = "Force always use kuid2"
        Me.chkKuid2.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(298, 304)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
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
    Friend WithEvents chkKuid2 As CheckBox
End Class
