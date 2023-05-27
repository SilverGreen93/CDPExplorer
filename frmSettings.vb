Public Class frmSettings

    Private Sub chkUnderscore_CheckedChanged(sender As Object, e As EventArgs) Handles chkUnderscore.CheckedChanged
        ReformatExample()
    End Sub

    Sub ReformatExample()
        If chkUnderscore.Checked Then
            lblExample.Text = lblExample.Text.Replace(" ", "_")
        Else
            lblExample.Text = lblExample.Text.Replace("_", " ")
        End If
    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbPart1.SelectedItem = My.Settings.fileNamePart1
        cmbPart2.SelectedItem = My.Settings.fileNamePart2
        cmbPart3.SelectedItem = My.Settings.fileNamePart3

        chkUnderscore.Checked = My.Settings.fileUseUnderscores

        Select Case My.Settings.fileSavePolicy
            Case frmMain.SavePolicy.SAVE_OVERWRITE
                optSaveOverwrite.Checked = True
            Case frmMain.SavePolicy.SAVE_SKIP
                optSaveSkip.Checked = True
        End Select
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        My.Settings.fileNamePart1 = cmbPart1.SelectedItem
        My.Settings.fileNamePart2 = cmbPart2.SelectedItem
        My.Settings.fileNamePart3 = cmbPart3.SelectedItem

        My.Settings.fileUseUnderscores = chkUnderscore.Checked

        If optSaveOverwrite.Checked Then
            My.Settings.fileSavePolicy = frmMain.SavePolicy.SAVE_OVERWRITE
        ElseIf optSaveSkip.Checked Then
            My.Settings.fileSavePolicy = frmMain.SavePolicy.SAVE_SKIP
        End If
    End Sub

    Private Sub cmbSelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPart1.SelectedIndexChanged, cmbPart2.SelectedIndexChanged, cmbPart3.SelectedIndexChanged
        Dim ex As String = ""
        Dim lc As New List(Of ComboBox)({cmbPart1, cmbPart2, cmbPart3})

        If cmbPart1.SelectedItem = "(none)" AndAlso cmbPart2.SelectedItem = "(none)" AndAlso cmbPart3.SelectedItem = "(none)" Then
            cmbPart1.SelectedItem = "KUID"
        End If

        For Each c In lc
            Select Case c.SelectedItem
                Case "KUID"
                    ex &= "kuid 1234 5678 "
                Case "build"
                    ex &= "4.5 "
                Case "username"
                    ex &= "My Asset "
            End Select
        Next c

        ex = ex.Trim()
        ex &= ".cdp"
        lblExample.Text = ex
        ReformatExample()
    End Sub
End Class