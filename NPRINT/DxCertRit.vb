Public Class DxCertRit
    Dim totprev As Decimal = 0
    Dim totpag As Decimal = 0
    Dim t1 As Decimal = 0
    Dim t2 As Decimal = 0
    Dim t3 As Decimal = 0
    Dim t4 As Decimal = 0
    Dim t5 As Decimal = 0
    Dim t6 As Decimal = 0

    Dim t7 As Decimal = 0
    Dim t8 As Decimal = 0
    Dim t9 As Decimal = 0
    Private Sub XrtableCell30_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell30.BeforePrint
        If XrTableCell29.Text = "" Then XrTableCell29.Text = "0"
        If XrTableCell28.Text = "" Then XrTableCell28.Text = "0"
        totprev = CDec(XrTableCell28.Text) + CDec(XrTableCell29.Text)
        If totprev = 0 Then XrTableCell30.Text = "" Else XrTableCell30.Text = Format(totprev, "#,###,###,##0.00")
        t9 += totprev
    End Sub
    Private Sub XrtableCell29_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell29.BeforePrint
        If XrTableCell29.Text = "" Then XrTableCell29.Text = "0"
        t8 += CDec(XrTableCell29.Text)
        If CDec(XrTableCell29.Text) = 0 Then XrTableCell29.Text = ""
    End Sub
    Private Sub XrtableCell28_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell28.BeforePrint
        If XrTableCell28.Text = "" Then XrTableCell28.Text = "0"
        t7 += CDec(XrTableCell28.Text)
        If CDec(XrTableCell28.Text) = 0 Then XrTableCell28.Text = ""
    End Sub
    Private Sub XrtableCell6_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell6.BeforePrint
        If XrTableCell5.Text = " " Then
            XrTableCell6.Text = ""
        Else
            XrTableCell6.Text = XrLabel19.Text.ToString.PadLeft(2, "0") & "/" & XrLabel20.Text.ToString.PadLeft(2, "0") & "/" & XrLabel21.Text
        End If
    End Sub
    Private Sub XrtableCell5_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell5.BeforePrint
        XrTableCell5.Text = XrLabel28.Text & " " & XrLabel23.Text
    End Sub
    Private Sub XrtableCell22_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell22.BeforePrint
        If XrTableCell22.Text = "" Then XrTableCell22.Text = "0"
        t1 += CDec(XrTableCell22.Text)
        totpag = CDec(XrTableCell22.Text)
        If CDec(XrTableCell22.Text) = 0 Then XrTableCell22.Text = ""
    End Sub

    Private Sub XrtableCell24_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell24.BeforePrint
        If XrTableCell24.Text = "" Then XrTableCell22.Text = "0"
        t2 += CDec(XrTableCell24.Text)
        totpag = totpag - CDec(XrTableCell24.Text)
        If CDec(XrTableCell24.Text) = 0 Then XrTableCell24.Text = ""
    End Sub
    Private Sub XrtableCell25_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell25.BeforePrint
        If XrTableCell25.Text = "" Then XrTableCell25.Text = "0"
        t3 += CDec(XrTableCell25.Text)
        totpag = totpag + CDec(XrTableCell25.Text)
        If CDec(XrTableCell25.Text) = 0 Then XrTableCell25.Text = ""
    End Sub
    Private Sub XrtableCell41_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell41.BeforePrint
        If XrTableCell41.Text = "" Then XrTableCell41.Text = "0"
        t4 += CDec(XrTableCell41.Text)
        totpag = totpag + CDec(XrTableCell41.Text)
        If CDec(XrTableCell41.Text) = 0 Then XrTableCell41.Text = ""
    End Sub
    Private Sub XrtableCell26_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell26.BeforePrint
        If XrTableCell26.Text = "" Then XrTableCell26.Text = "0"
        t5 += CDec(XrTableCell26.Text)
        totpag = totpag + CDec(XrTableCell26.Text)
        If CDec(XrTableCell26.Text) = 0 Then XrTableCell26.Text = ""
    End Sub
    Private Sub XrtableCell27_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell27.BeforePrint
        If totpag = 0 Then XrTableCell27.Text = "" Else XrTableCell27.Text = Format(totpag, "#,###,###,##0.00")
        t6 += totpag
        If CDec(XrTableCell27.Text) = 0 Then XrTableCell27.Text = ""
    End Sub

    'T1
    Private Sub XrTableCell31_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell31.BeforePrint
        XrTableCell31.Text = Format(t1, "#,###,###,##0.00")
        If CDec(XrTableCell31.Text) = 0 Then XrTableCell31.Text = ""
    End Sub
    'T2
    Private Sub XrTableCell33_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell33.BeforePrint
        XrTableCell33.Text = Format(t2, "#,###,###,##0.00")
        If CDec(XrTableCell33.Text) = 0 Then XrTableCell33.Text = ""
    End Sub
    'T3
    Private Sub XrTableCell34_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell34.BeforePrint
        XrTableCell34.Text = Format(t3, "#,###,###,##0.00")
        If CDec(XrTableCell34.Text) = 0 Then XrTableCell34.Text = ""
    End Sub
    'T4
    Private Sub XrTableCell42_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell42.BeforePrint
        XrTableCell42.Text = Format(t4, "#,###,###,##0.00")
        If CDec(XrTableCell42.Text) = 0 Then XrTableCell42.Text = ""
    End Sub
    'T5
    Private Sub XrTableCell35_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell35.BeforePrint
        XrTableCell35.Text = Format(t5, "#,###,###,##0.00")
        If CDec(XrTableCell35.Text) = 0 Then XrTableCell35.Text = ""
    End Sub
    'T6
    Private Sub XrTableCell36_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell36.BeforePrint
        XrTableCell36.Text = Format(t6, "#,###,###,##0.00")
        If CDec(XrTableCell36.Text) = 0 Then XrTableCell36.Text = ""
    End Sub

    'T7
    Private Sub XrTableCell37_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell37.BeforePrint
        XrTableCell37.Text = Format(t7, "#,###,###,##0.00")
        If CDec(XrTableCell37.Text) = 0 Then XrTableCell37.Text = ""
    End Sub
    'T8
    Private Sub XrTableCell38_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell38.BeforePrint
        XrTableCell38.Text = Format(t8, "#,###,###,##0.00")
        If CDec(XrTableCell38.Text) = 0 Then XrTableCell38.Text = ""
    End Sub
    'T9
    Private Sub XrTableCell39_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell39.BeforePrint
        XrTableCell39.Text = Format(t9, "#,###,###,##0.00")
        If CDec(XrTableCell39.Text) = 0 Then XrTableCell39.Text = ""
    End Sub
    'AZZERO A CAMBIO PERCIPIENTE
    Private Sub XrLabel10_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles XrLabel10.TextChanged
        t1 = 0 : t2 = 0 : t3 = 0 : t4 = 0 : t5 = 0 : t6 = 0 : t7 = 0 : t8 = 0 : t9 = 0
    End Sub
End Class