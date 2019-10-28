Public Class DxDetVendite
    Dim totale As Decimal = 0
    Private Sub XrTableCell3_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell3.BeforePrint
        If XrLabel5.Text = "" Then XrLabel5.Text = "0"
        If XrLabel6.Text = "" Then XrLabel6.Text = "0"
        totale += CDec(XrLabel5.Text)
        totale += CDec(XrLabel6.Text)
    End Sub
    Private Sub XrTableCell9_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell9.BeforePrint
        XrTableCell9.Text = Format(totale, "#,###,###,###,##0.00")
    End Sub
End Class