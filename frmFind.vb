Public Class frmFind
    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        frmMain.FindText(txtFind.Text)
        Close()
    End Sub

End Class