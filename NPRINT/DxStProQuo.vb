
Public Class DxStProQuo
    Dim t1 As Decimal = 0
    Dim t2 As Decimal = 0
    Dim t3 As Decimal = 0
    Dim t4 As Decimal = 0
    Dim t5 As Decimal = 0
    Dim t6 As Decimal = 0

    Dim tt1 As Decimal = 0
    Dim tt2 As Decimal = 0
    Dim tt3 As Decimal = 0
    Dim tt4 As Decimal = 0
    Dim tt5 As Decimal = 0
    Dim tt6 As Decimal = 0
    Private Sub XrTableCell16_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell16.BeforePrint
        If XrTableCell16.Text = "" Then Exit Sub
        If CDec(XrTableCell16.Text) = 0 Then XrTableCell16.Text = ""
    End Sub
    Private Sub XrTableCell18_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell18.BeforePrint
        If XrTableCell18.Text = "" Then Exit Sub
        If CDec(XrTableCell18.Text) = 0 Then XrTableCell18.Text = ""
    End Sub
    Private Sub XrTableCell19_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell19.BeforePrint
        If XrTableCell19.Text = "" Then Exit Sub
        t1 += CDec(XrTableCell19.Text)
        tt1 += CDec(XrTableCell19.Text)
    End Sub
    Private Sub XrTableCell20_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell20.BeforePrint
        If XrTableCell20.Text = "" Then Exit Sub
        t2 += CDec(XrTableCell20.Text)
        tt2 += CDec(XrTableCell20.Text)
    End Sub
    Private Sub XrTableCell22_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell22.BeforePrint
        If XrTableCell22.Text = "" Then Exit Sub
        t3 += CDec(XrTableCell22.Text)
        tt3 += CDec(XrTableCell22.Text)
        If CDec(XrTableCell22.Text) = 0 Then XrTableCell22.Text = ""
    End Sub
    Private Sub XrTableCell23_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell23.BeforePrint
        If XrTableCell23.Text = "" Then Exit Sub
        t4 += CDec(XrTableCell23.Text)
        tt4 += CDec(XrTableCell23.Text)
        If CDec(XrTableCell23.Text) = 0 Then XrTableCell23.Text = ""
    End Sub
    Private Sub XrTableCell24_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell24.BeforePrint
        If XrTableCell24.Text = "" Then Exit Sub
        t5 += CDec(XrTableCell24.Text)
        tt5 += CDec(XrTableCell24.Text)
        If CDec(XrTableCell24.Text) = 0 Then XrTableCell24.Text = ""
    End Sub
    Private Sub XrTableCell25_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell25.BeforePrint
        If XrTableCell25.Text = "" Then Exit Sub
        t6 += CDec(XrTableCell25.Text)
        tt6 += CDec(XrTableCell25.Text)
        If CDec(XrTableCell25.Text) = 0 Then XrTableCell25.Text = ""
    End Sub
    Private Sub XrTableCell29_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell29.BeforePrint
        XrTableCell29.Text = Format(t1, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell30_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell30.BeforePrint
        XrTableCell30.Text = Format(t2, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell32_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell32.BeforePrint
        XrTableCell32.Text = Format(t3, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell33_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell33.BeforePrint
        XrTableCell33.Text = Format(t4, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell34_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell34.BeforePrint
        XrTableCell34.Text = Format(t5, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell35_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell35.BeforePrint
        XrTableCell35.Text = Format(t6, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell36_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell36.BeforePrint
        XrTableCell36.Text = Format(tt1, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell37_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell37.BeforePrint
        XrTableCell37.Text = Format(tt2, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell39_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell39.BeforePrint
        XrTableCell39.Text = Format(tt3, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell40_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell40.BeforePrint
        XrTableCell40.Text = Format(tt4, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell41_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell41.BeforePrint
        XrTableCell41.Text = Format(tt5, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell42_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell42.BeforePrint
        XrTableCell42.Text = Format(tt6, "#,###,###,##0.00")
    End Sub
    Private Sub XrLabel3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles XrLabel3.TextChanged, XrLabel1.TextChanged
        t1 = 0 : t2 = 0 : t3 = 0 : t4 = 0 : t5 = 0 : t6 = 0
    End Sub
End Class