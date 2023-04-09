Public Class frmFind
    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        frmMain.FindText(txtFind.Text)
        Close()
    End Sub

    Private Sub frmFind_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFind.Focus()
    End Sub
End Class