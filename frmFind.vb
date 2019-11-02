Public Class frmFind
    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        frmMain.FindText(txtFind.Text)
        Close()
    End Sub

    Private Sub txtFind_TextChanged(sender As Object, e As EventArgs) Handles txtFind.TextChanged

    End Sub
End Class