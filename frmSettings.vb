Public Class frmSettings

    Private Sub optKuid_CheckedChanged(sender As Object, e As EventArgs) Handles optKuid.CheckedChanged
        lblExample.Text = "kuid 1234 5678.cdp"
        ReformatExample()
    End Sub

    Private Sub optBuildKuid_CheckedChanged(sender As Object, e As EventArgs) Handles optBuildKuid.CheckedChanged
        lblExample.Text = "3.7 kuid 1234 5678.cdp"
        ReformatExample()
    End Sub

    Private Sub optUsernameKuid_CheckedChanged(sender As Object, e As EventArgs) Handles optUsernameKuid.CheckedChanged
        lblExample.Text = "My Asset kuid 1234 5678.cdp"
        ReformatExample()
    End Sub

    Private Sub optUsername_CheckedChanged(sender As Object, e As EventArgs) Handles optUsername.CheckedChanged
        lblExample.Text = "My Asset.cdp"
        ReformatExample()
    End Sub

    Private Sub optBuildUsername_CheckedChanged(sender As Object, e As EventArgs) Handles optBuildUsername.CheckedChanged
        lblExample.Text = "3.7 My Asset.cdp"
        ReformatExample()
    End Sub

    Private Sub optBuildUsernameKuid_CheckedChanged(sender As Object, e As EventArgs) Handles optBuildUsernameKuid.CheckedChanged
        lblExample.Text = "3.7 My Asset kuid 1234 5678.cdp"
        ReformatExample()
    End Sub

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
        Select Case My.Settings.fileNameFormat
            Case frmMain.NameFormat.NAME_KUID
                optKuid.Checked = True
            Case frmMain.NameFormat.NAME_BUILD_KUID
                optBuildKuid.Checked = True
            Case frmMain.NameFormat.NAME_USERNAME_KUID
                optUsernameKuid.Checked = True
            Case frmMain.NameFormat.NAME_USERNAME
                optUsername.Checked = True
            Case frmMain.NameFormat.NAME_BUILD_USERNAME
                optBuildUsername.Checked = True
            Case frmMain.NameFormat.NAME_BUILD_USERNAME_KUID
                optBuildUsernameKuid.Checked = True
        End Select

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
        If optKuid.Checked Then
            My.Settings.fileNameFormat = frmMain.NameFormat.NAME_KUID
        ElseIf optBuildKuid.Checked Then
            My.Settings.fileNameFormat = frmMain.NameFormat.NAME_BUILD_KUID
        ElseIf optUsernameKuid.Checked Then
            My.Settings.fileNameFormat = frmMain.NameFormat.NAME_USERNAME_KUID
        ElseIf optUsername.Checked Then
            My.Settings.fileNameFormat = frmMain.NameFormat.NAME_USERNAME
        ElseIf optBuildUsername.Checked Then
            My.Settings.fileNameFormat = frmMain.NameFormat.NAME_BUILD_USERNAME
        ElseIf optBuildUsernameKuid.Checked Then
            My.Settings.fileNameFormat = frmMain.NameFormat.NAME_BUILD_USERNAME_KUID
        End If

        My.Settings.fileUseUnderscores = chkUnderscore.Checked

        If optSaveOverwrite.Checked Then
            My.Settings.fileSavePolicy = frmMain.SavePolicy.SAVE_OVERWRITE
        ElseIf optSaveSkip.Checked Then
            My.Settings.fileSavePolicy = frmMain.SavePolicy.SAVE_SKIP
        End If
    End Sub
End Class