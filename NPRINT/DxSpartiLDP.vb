Public Class DxSpartiLDP
    Dim t1 As Decimal = 0
    Dim t2 As Decimal = 0
    Dim tt As Decimal = 0
    Private Sub XrtableCell18_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell18.BeforePrint
        If XrTableCell18.Text = "" Then XrTableCell18.Text = "0"
        t1 += CDec(XrTableCell18.Text)
        If CDec(XrTableCell18.Text) = 0 Then XrTableCell18.Text = ""
    End Sub
    Private Sub XrtableCell19_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell19.BeforePrint
        If XrTableCell19.Text = "" Then XrTableCell19.Text = "0"
        t2 += CDec(XrTableCell19.Text)
        If CDec(XrTableCell19.Text) = 0 Then XrTableCell19.Text = ""
    End Sub
    Private Sub XrTableCell24_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell24.BeforePrint
        XrTableCell24.Text = Format(t1, "#,###,###,##0.00")
        If CDec(XrTableCell24.Text) = 0 Then XrTableCell24.Text = ""
    End Sub
    Private Sub XrTableCell25_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell25.BeforePrint
        XrTableCell25.Text = Format(t2, "#,###,###,##0.00")
        If CDec(XrTableCell25.Text) = 0 Then XrTableCell25.Text = ""
    End Sub
    Private Sub XrTableCell26_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell24.BeforePrint, XrTableCell25.BeforePrint, XrTableCell26.BeforePrint, XrTableCell27.BeforePrint
        tt = t1 - t2
        XrTableCell26.Text = Format(tt, "#,###,###,##0.00")
        If tt < 0 Then
            XrTableCell27.Text = "Saldo Avere "
        ElseIf tt > 0 Then
            XrTableCell27.Text = "Saldo Dare "
        Else
            XrTableCell27.Text = "           "
        End If
    End Sub
    Private Sub XrLabel3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles XrLabel3.TextChanged, XrLabel1.TextChanged
        t1 = 0 : t2 = 0 : tt = 0
    End Sub
End Class