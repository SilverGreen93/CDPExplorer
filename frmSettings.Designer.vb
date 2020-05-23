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
        Me.lblExample = New System.Windows.Forms.Label()
        Me.optBuildUsernameKuid = New System.Windows.Forms.RadioButton()
        Me.optBuildUsername = New System.Windows.Forms.RadioButton()
        Me.optUsernameKuid = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.optUsername = New System.Windows.Forms.RadioButton()
        Me.optBuildKuid = New System.Windows.Forms.RadioButton()
        Me.optKuid = New System.Windows.Forms.RadioButton()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(53, 285)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(128, 36)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(218, 285)
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
        Me.GroupBox1.Location = New System.Drawing.Point(12, 198)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(372, 78)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "File overwrite policy"
        '
        'optSaveSkip
        '
        Me.optSaveSkip.AutoSize = True
        Me.optSaveSkip.Location = New System.Drawing.Point(22, 48)
        Me.optSaveSkip.Name = "optSaveSkip"
        Me.optSaveSkip.Size = New System.Drawing.Size(192, 21)
        Me.optSaveSkip.TabIndex = 1
        Me.optSaveSkip.TabStop = True
        Me.optSaveSkip.Text = "Skip file if already present"
        Me.optSaveSkip.UseVisualStyleBackColor = True
        '
        'optSaveOverwrite
        '
        Me.optSaveOverwrite.AutoSize = True
        Me.optSaveOverwrite.Location = New System.Drawing.Point(22, 21)
        Me.optSaveOverwrite.Name = "optSaveOverwrite"
        Me.optSaveOverwrite.Size = New System.Drawing.Size(262, 21)
        Me.optSaveOverwrite.TabIndex = 0
        Me.optSaveOverwrite.TabStop = True
        Me.optSaveOverwrite.Text = "Overwrite existing files without asking"
        Me.optSaveOverwrite.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblExample)
        Me.GroupBox2.Controls.Add(Me.optBuildUsernameKuid)
        Me.GroupBox2.Controls.Add(Me.optBuildUsername)
        Me.GroupBox2.Controls.Add(Me.optUsernameKuid)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.optUsername)
        Me.GroupBox2.Controls.Add(Me.optBuildKuid)
        Me.GroupBox2.Controls.Add(Me.optKuid)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(372, 180)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "CDP file export name"
        '
        'lblExample
        '
        Me.lblExample.AutoSize = True
        Me.lblExample.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblExample.Location = New System.Drawing.Point(6, 138)
        Me.lblExample.Name = "lblExample"
        Me.lblExample.Size = New System.Drawing.Size(210, 20)
        Me.lblExample.TabIndex = 9
        Me.lblExample.Text = "kuid 474195 100102.cdp"
        '
        'optBuildUsernameKuid
        '
        Me.optBuildUsernameKuid.AutoSize = True
        Me.optBuildUsernameKuid.Location = New System.Drawing.Point(170, 75)
        Me.optBuildUsernameKuid.Name = "optBuildUsernameKuid"
        Me.optBuildUsernameKuid.Size = New System.Drawing.Size(188, 21)
        Me.optBuildUsernameKuid.TabIndex = 9
        Me.optBuildUsernameKuid.TabStop = True
        Me.optBuildUsernameKuid.Text = "build + Username + KUID"
        Me.optBuildUsernameKuid.UseVisualStyleBackColor = True
        '
        'optBuildUsername
        '
        Me.optBuildUsername.AutoSize = True
        Me.optBuildUsername.Location = New System.Drawing.Point(170, 48)
        Me.optBuildUsername.Name = "optBuildUsername"
        Me.optBuildUsername.Size = New System.Drawing.Size(140, 21)
        Me.optBuildUsername.TabIndex = 8
        Me.optBuildUsername.TabStop = True
        Me.optBuildUsername.Text = "build + Username"
        Me.optBuildUsername.UseVisualStyleBackColor = True
        '
        'optUsernameKuid
        '
        Me.optUsernameKuid.AutoSize = True
        Me.optUsernameKuid.Location = New System.Drawing.Point(22, 75)
        Me.optUsernameKuid.Name = "optUsernameKuid"
        Me.optUsernameKuid.Size = New System.Drawing.Size(142, 21)
        Me.optUsernameKuid.TabIndex = 7
        Me.optUsernameKuid.TabStop = True
        Me.optUsernameKuid.Text = "Username + KUID"
        Me.optUsernameKuid.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 109)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(163, 20)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Example filename:"
        '
        'optUsername
        '
        Me.optUsername.AutoSize = True
        Me.optUsername.Location = New System.Drawing.Point(170, 21)
        Me.optUsername.Name = "optUsername"
        Me.optUsername.Size = New System.Drawing.Size(94, 21)
        Me.optUsername.TabIndex = 4
        Me.optUsername.TabStop = True
        Me.optUsername.Text = "Username"
        Me.optUsername.UseVisualStyleBackColor = True
        '
        'optBuildKuid
        '
        Me.optBuildKuid.AutoSize = True
        Me.optBuildKuid.Location = New System.Drawing.Point(22, 48)
        Me.optBuildKuid.Name = "optBuildKuid"
        Me.optBuildKuid.Size = New System.Drawing.Size(107, 21)
        Me.optBuildKuid.TabIndex = 3
        Me.optBuildKuid.TabStop = True
        Me.optBuildKuid.Text = "build + KUID"
        Me.optBuildKuid.UseVisualStyleBackColor = True
        '
        'optKuid
        '
        Me.optKuid.AutoSize = True
        Me.optKuid.Location = New System.Drawing.Point(22, 21)
        Me.optKuid.Name = "optKuid"
        Me.optKuid.Size = New System.Drawing.Size(61, 21)
        Me.optKuid.TabIndex = 2
        Me.optKuid.TabStop = True
        Me.optKuid.Text = "KUID"
        Me.optKuid.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(397, 333)
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
    Friend WithEvents optUsername As RadioButton
    Friend WithEvents optBuildKuid As RadioButton
    Friend WithEvents optKuid As RadioButton
    Friend WithEvents optBuildUsername As RadioButton
    Friend WithEvents optUsernameKuid As RadioButton
    Friend WithEvents lblExample As Label
    Friend WithEvents optBuildUsernameKuid As RadioButton
End Class
