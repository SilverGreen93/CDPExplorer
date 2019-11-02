<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.gridKUIDs = New System.Windows.Forms.DataGridView()
        Me.ctxMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyKUIDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyAssetInfoToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExtractSelectedAsCDPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.FIleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenCDPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportAsCSVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyKUIDListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyAssetListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.FindToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AssetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyKUIDToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyAssetInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExtractAsCDPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExtractAllAsCDPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.FolderBrowserDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.FindNextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.gridKUIDs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxMenu.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridKUIDs
        '
        Me.gridKUIDs.AllowUserToAddRows = False
        Me.gridKUIDs.AllowUserToDeleteRows = False
        Me.gridKUIDs.AllowUserToResizeRows = False
        Me.gridKUIDs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.gridKUIDs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridKUIDs.ContextMenuStrip = Me.ctxMenu
        Me.gridKUIDs.Location = New System.Drawing.Point(12, 39)
        Me.gridKUIDs.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.gridKUIDs.Name = "gridKUIDs"
        Me.gridKUIDs.RowHeadersVisible = False
        Me.gridKUIDs.RowHeadersWidth = 51
        Me.gridKUIDs.RowTemplate.Height = 24
        Me.gridKUIDs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridKUIDs.Size = New System.Drawing.Size(1025, 564)
        Me.gridKUIDs.TabIndex = 26
        '
        'ctxMenu
        '
        Me.ctxMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ctxMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyKUIDToolStripMenuItem, Me.CopyAssetInfoToolStripMenuItem1, Me.ToolStripMenuItem5, Me.ExtractSelectedAsCDPToolStripMenuItem})
        Me.ctxMenu.Name = "ContextMenuStrip"
        Me.ctxMenu.Size = New System.Drawing.Size(183, 82)
        '
        'CopyKUIDToolStripMenuItem
        '
        Me.CopyKUIDToolStripMenuItem.Name = "CopyKUIDToolStripMenuItem"
        Me.CopyKUIDToolStripMenuItem.Size = New System.Drawing.Size(182, 24)
        Me.CopyKUIDToolStripMenuItem.Text = "Copy KUID"
        '
        'CopyAssetInfoToolStripMenuItem1
        '
        Me.CopyAssetInfoToolStripMenuItem1.Name = "CopyAssetInfoToolStripMenuItem1"
        Me.CopyAssetInfoToolStripMenuItem1.Size = New System.Drawing.Size(182, 24)
        Me.CopyAssetInfoToolStripMenuItem1.Text = "Copy Asset info"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(179, 6)
        '
        'ExtractSelectedAsCDPToolStripMenuItem
        '
        Me.ExtractSelectedAsCDPToolStripMenuItem.Name = "ExtractSelectedAsCDPToolStripMenuItem"
        Me.ExtractSelectedAsCDPToolStripMenuItem.Size = New System.Drawing.Size(182, 24)
        Me.ExtractSelectedAsCDPToolStripMenuItem.Text = "Extract as CDP..."
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.CheckFileExists = False
        '
        'StatusStrip
        '
        Me.StatusStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus, Me.lblCount})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 616)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 13, 0)
        Me.StatusStrip.Size = New System.Drawing.Size(1049, 26)
        Me.StatusStrip.TabIndex = 31
        Me.StatusStrip.Text = "StatusStrip"
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(53, 20)
        Me.lblStatus.Text = "Ready."
        '
        'lblCount
        '
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(63, 20)
        Me.lblCount.Text = "0 assets."
        '
        'MenuStrip
        '
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FIleToolStripMenuItem, Me.EditToolStripMenuItem, Me.AssetToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(1049, 28)
        Me.MenuStrip.TabIndex = 32
        Me.MenuStrip.Text = "MenuStrip1"
        '
        'FIleToolStripMenuItem
        '
        Me.FIleToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenCDPToolStripMenuItem, Me.ExportAsCSVToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FIleToolStripMenuItem.Name = "FIleToolStripMenuItem"
        Me.FIleToolStripMenuItem.Size = New System.Drawing.Size(46, 24)
        Me.FIleToolStripMenuItem.Text = "FIle"
        '
        'OpenCDPToolStripMenuItem
        '
        Me.OpenCDPToolStripMenuItem.Name = "OpenCDPToolStripMenuItem"
        Me.OpenCDPToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenCDPToolStripMenuItem.Size = New System.Drawing.Size(242, 26)
        Me.OpenCDPToolStripMenuItem.Text = "Open CDP..."
        '
        'ExportAsCSVToolStripMenuItem
        '
        Me.ExportAsCSVToolStripMenuItem.Name = "ExportAsCSVToolStripMenuItem"
        Me.ExportAsCSVToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.ExportAsCSVToolStripMenuItem.Size = New System.Drawing.Size(242, 26)
        Me.ExportAsCSVToolStripMenuItem.Text = "Export as CSV..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(239, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(242, 26)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyKUIDListToolStripMenuItem, Me.CopyAssetListToolStripMenuItem, Me.ToolStripMenuItem2, Me.FindToolStripMenuItem, Me.FindNextToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(49, 24)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'CopyKUIDListToolStripMenuItem
        '
        Me.CopyKUIDListToolStripMenuItem.Name = "CopyKUIDListToolStripMenuItem"
        Me.CopyKUIDListToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyKUIDListToolStripMenuItem.Size = New System.Drawing.Size(281, 26)
        Me.CopyKUIDListToolStripMenuItem.Text = "Copy KUID list"
        '
        'CopyAssetListToolStripMenuItem
        '
        Me.CopyAssetListToolStripMenuItem.Name = "CopyAssetListToolStripMenuItem"
        Me.CopyAssetListToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.CopyAssetListToolStripMenuItem.Size = New System.Drawing.Size(281, 26)
        Me.CopyAssetListToolStripMenuItem.Text = "Copy Asset list"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(278, 6)
        '
        'FindToolStripMenuItem
        '
        Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
        Me.FindToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FindToolStripMenuItem.Size = New System.Drawing.Size(281, 26)
        Me.FindToolStripMenuItem.Text = "Find..."
        '
        'AssetToolStripMenuItem
        '
        Me.AssetToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyKUIDToolStripMenuItem1, Me.CopyAssetInfoToolStripMenuItem, Me.ToolStripMenuItem3, Me.ExtractAsCDPToolStripMenuItem, Me.ExtractAllAsCDPToolStripMenuItem})
        Me.AssetToolStripMenuItem.Name = "AssetToolStripMenuItem"
        Me.AssetToolStripMenuItem.Size = New System.Drawing.Size(58, 24)
        Me.AssetToolStripMenuItem.Text = "Asset"
        '
        'CopyKUIDToolStripMenuItem1
        '
        Me.CopyKUIDToolStripMenuItem1.Name = "CopyKUIDToolStripMenuItem1"
        Me.CopyKUIDToolStripMenuItem1.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyKUIDToolStripMenuItem1.Size = New System.Drawing.Size(307, 26)
        Me.CopyKUIDToolStripMenuItem1.Text = "Copy KUID"
        '
        'CopyAssetInfoToolStripMenuItem
        '
        Me.CopyAssetInfoToolStripMenuItem.Name = "CopyAssetInfoToolStripMenuItem"
        Me.CopyAssetInfoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.CopyAssetInfoToolStripMenuItem.Size = New System.Drawing.Size(307, 26)
        Me.CopyAssetInfoToolStripMenuItem.Text = "Copy Asset info"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(304, 6)
        '
        'ExtractAsCDPToolStripMenuItem
        '
        Me.ExtractAsCDPToolStripMenuItem.Name = "ExtractAsCDPToolStripMenuItem"
        Me.ExtractAsCDPToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.ExtractAsCDPToolStripMenuItem.Size = New System.Drawing.Size(307, 26)
        Me.ExtractAsCDPToolStripMenuItem.Text = "Extract as CDP..."
        '
        'ExtractAllAsCDPToolStripMenuItem
        '
        Me.ExtractAllAsCDPToolStripMenuItem.Name = "ExtractAllAsCDPToolStripMenuItem"
        Me.ExtractAllAsCDPToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.ExtractAllAsCDPToolStripMenuItem.Size = New System.Drawing.Size(307, 26)
        Me.ExtractAllAsCDPToolStripMenuItem.Text = "Extract all as CDP..."
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(55, 24)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(224, 26)
        Me.AboutToolStripMenuItem.Text = "About..."
        '
        'BackgroundWorker
        '
        Me.BackgroundWorker.WorkerReportsProgress = True
        Me.BackgroundWorker.WorkerSupportsCancellation = True
        '
        'FindNextToolStripMenuItem
        '
        Me.FindNextToolStripMenuItem.Name = "FindNextToolStripMenuItem"
        Me.FindNextToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.FindNextToolStripMenuItem.Size = New System.Drawing.Size(281, 26)
        Me.FindNextToolStripMenuItem.Text = "Find next"
        '
        'frmMain
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1049, 642)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.MenuStrip)
        Me.Controls.Add(Me.gridKUIDs)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmMain"
        Me.Text = "frmMain"
        CType(Me.gridKUIDs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxMenu.ResumeLayout(False)
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gridKUIDs As DataGridView
    Friend WithEvents OpenFileDialog As OpenFileDialog
    Friend WithEvents StatusStrip As StatusStrip
    Friend WithEvents lblStatus As ToolStripStatusLabel
    Friend WithEvents MenuStrip As MenuStrip
    Friend WithEvents FIleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenCDPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportAsCSVToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyKUIDListToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyAssetListToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents FindToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AssetToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyAssetInfoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents ExtractAsCDPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExtractAllAsCDPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblCount As ToolStripStatusLabel
    Friend WithEvents SaveFileDialog As SaveFileDialog
    Friend WithEvents FolderBrowserDialog As FolderBrowserDialog
    Friend WithEvents BackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents ctxMenu As ContextMenuStrip
    Friend WithEvents CopyAssetInfoToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents ExtractSelectedAsCDPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyKUIDToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyKUIDToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents FindNextToolStripMenuItem As ToolStripMenuItem
End Class
