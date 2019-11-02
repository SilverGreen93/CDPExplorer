Public Class frmProgress
    Private Sub frmProgress_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ProgressBar.Value = 0
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        frmMain.BackgroundWorker.CancelAsync()
    End Sub

    Private Sub frmProgress_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If frmMain.BackgroundWorker.IsBusy Then e.Cancel = True
        frmMain.BackgroundWorker.CancelAsync()
    End Sub
End Class