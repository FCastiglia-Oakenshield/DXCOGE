Public Class DxGrMondo
    Dim saldoconto As Decimal = 0
    Dim totdare As Decimal = 0
    Dim totavere As Decimal = 0
    Dim t1 As Decimal = 0 ' totale gruppo
    Dim t2 As Decimal = 0 ' totale non classificato
    Dim t3 As Decimal = 0 ' totale patrimoniale 
    Dim t4 As Decimal = 0 ' totale conto economico

    ''Private Sub XrLabel2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles XrLabel2.TextChanged
    ''    t1 = 0
    ''End Sub
    Private Sub XrLabel1_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel1.BeforePrint
        If XrLabel1.Text = "" Then XrLabel1.Text = "*** NON CLASSIFICATO ***"
    End Sub
    Private Sub XrLabel7_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel7.BeforePrint
        If XrLabel2.Text = "" Then t2 = t2 + CDec(XrLabel7.Text) : GoTo totale

        If XrLabel15.Text = "6" Or XrLabel15.Text = "7" Then
            t4 = t4 + CDec(XrLabel7.Text)
        Else
            t3 = t3 + CDec(XrLabel7.Text)
        End If
totale:
        t1 = t1 + CDec(XrLabel7.Text)
    End Sub
    Private Sub XrLabel8_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel8.BeforePrint
        XrLabel8.Text = Format(t1, "#,###,###,##0.00") : t1 = 0
    End Sub
    Private Sub XrLabel12_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel12.BeforePrint
        XrLabel12.Text = Format(t2, "#,###,###,##0.00")
        XrLabel13.Text = Format(t3, "#,###,###,##0.00")
        XrLabel14.Text = Format(t4, "#,###,###,##0.00")
    End Sub
End Class