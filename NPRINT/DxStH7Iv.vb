Public Class DxStH7Iv
    Private Sub XrTableCell4_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell4.BeforePrint
        If CDec(XrTableCell4.Text) = 0 Then XrTableCell4.Text = ""
    End Sub
    Private Sub XrTableCell19_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell19.BeforePrint
        If CDec(XrTableCell19.Text) = 0 Then XrTableCell19.Text = ""
    End Sub
End Class