Public Class DxStH7PN
    Dim Descri As String
    Private Sub XrTableCell25_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell25.BeforePrint
        Descri = ""
        Descri = XrLabel4.Text & XrLabel15.Text
        XrTableCell25.Text = Descri
    End Sub
    Private Sub XrLabel8_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel8.BeforePrint
        If CDec(XrLabel8.Text) = 0 Then XrLabel8.Text = ""
    End Sub
    Private Sub XrLabel9_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel9.BeforePrint
        If CDec(XrLabel9.Text) = 0 Then XrLabel9.Text = ""
    End Sub
End Class