Public Class XScaClAge
    Dim TOTALEAG As Decimal = 0
    Dim PROGCLI As Decimal = 0

    Private Sub GroupHeader2_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles GroupHeader2.BeforePrint
        PROGCLI = 0
    End Sub
    Private Sub XrTableCell34_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell34.BeforePrint
        Dim rw As DataRowView = GetCurrentRow()
        If rw IsNot Nothing Then
            PROGCLI = PROGCLI + CDec(rw("ScaScopRata"))
        End If
        XrTableCell34.Text = Format(PROGCLI, "c2")
    End Sub
End Class