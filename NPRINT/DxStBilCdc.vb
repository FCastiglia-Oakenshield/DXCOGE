Public Class DxStBilCdc
    Dim t1 As Decimal = 0
    Dim t2 As Decimal = 0
    Dim tt As Decimal = 0
    Private Sub XrTableCell7_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell7.BeforePrint
        Dim rw As DataRowView = GetCurrentRow()
        If rw("PiaFl01") = 6 Then
            XrTableCell7.Text = Format(rw("MCCIMPORTO"), "#,###,###,##0.00")
            t1 += CDec(XrTableCell7.Text)
        Else
            XrTableCell7.Text = ""
        End If
    End Sub
    Private Sub XrTableCell8_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell8.BeforePrint
        Dim rw As DataRowView = GetCurrentRow()
        If rw("PiaFl01") = 7 Then
            XrTableCell8.Text = Format(rw("MCCIMPORTO"), "#,###,###,##0.00")
            t2 += CDec(XrTableCell8.Text)
        Else
            XrTableCell8.Text = ""
        End If
    End Sub
    'Private Sub XrLabel15_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel15.BeforePrint
    '    If XrLabel19.Text = 6 Then
    '        XrLabel15.Text = XrLabel13.Text
    '        t1 += CDec(XrLabel15.Text)
    '    Else
    '        XrLabel15.Text = ""
    '    End If
    'End Sub
    'Private Sub XrLabel1_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel1.BeforePrint
    '    If XrLabel19.Text = 7 Then
    '        XrLabel1.Text = XrLabel13.Text
    '        t2 += CDec(XrLabel1.Text)
    '    Else
    '        XrLabel1.Text = ""
    '    End If

    'End Sub
    Private Sub XrTableCell16_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell16.BeforePrint
        XrTableCell16.Text = Format(t1, "#,###,###,##0.00")
    End Sub
    Private Sub XrTableCell18_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell18.BeforePrint
        XrTableCell18.Text = Format(t2, "#,###,###,##0.00")
    End Sub
    Private Sub XrLabel16_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell19.BeforePrint, XrTableCell20.BeforePrint, XrLabel18.BeforePrint
        tt = t1 - t2
        If tt < 0 Then
            XrTableCell19.Text = Format(tt, "#,###,###,##0.00")
            XrTableCell20.Text = Format(0, "#,###,###,###.##")
            XrLabel18.Text = "ECCEDENZA RICAVI "
        Else
            XrTableCell20.Text = Format(tt, "#,###,###,##0.00")
            XrTableCell19.Text = Format(0, "#,###,###,###.##")
            XrLabel18.Text = "ECCEDENZA COSTI "
        End If
    End Sub
    Private Sub XrTableCell14_TextChanged(sender As Object, e As System.EventArgs) Handles XrTableCell14.TextChanged
        t1 = 0 : t2 = 0 : tt = 0
    End Sub

   
End Class