Public Class DxBilSppp
    Dim saldoconto As Decimal = 0
    Dim totdare As Decimal = 0
    Dim totavere As Decimal = 0
    Dim t1 As Decimal = 0
    Dim t2 As Decimal = 0
    Dim t3 As Decimal = 0
    Dim t4 As Decimal = 0
    Dim t5 As Decimal = 0
    Dim t6 As Decimal = 0
    Dim t7 As Decimal = 0

    'AZZERO A CAMBIO FLAG PATRIMONIALE/CONTO ECONOMICO
    Private Sub XrLabel1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles XrLabel1.TextChanged
        t1 = 0 : t2 = 0 : t3 = 0 : t4 = 0 : t5 = 0 : t6 = 0
    End Sub
    Private Sub XrLabel3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles XrLabel3.TextChanged
        t1 = 0
    End Sub
    Private Sub XrTableCell10_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell10.BeforePrint
        saldoconto = +CDec(Xrlabel4.Text) - CDec(Xrlabel5.Text) + CDec(Xrlabel6.Text)
        If saldoconto = 0 Then Exit Sub
        If saldoconto > 0 Then
            XrTableCell10.Text = Format(saldoconto, "#,###,###,##0.00")
        Else
            saldoconto = saldoconto * -1
            XrTableCell11.Text = Format(saldoconto, "#,###,###,##0.00")
        End If

        'If XrTableCell29.Text = "" Then XrTableCell29.Text = "0"
        'If XrTableCell28.Text = "" Then XrTableCell28.Text = "0"
        'totprev = CDec(XrTableCell28.Text) + CDec(XrTableCell29.Text)
        'If totprev = 0 Then XrTableCell30.Text = "" Else XrTableCell30.Text = Format(totprev, "#,###,###,##0.00")
        't9 += totprev
    End Sub
    Private Sub XrtableCell3_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell3.BeforePrint
        If XrLabel1.Text = 0 Then
            XrTableCell24.Text = "SITUAZIONE PATRIMONIALE AL "
            XrTableCell3.Text = "ATTIVO"
        Else
            XrTableCell24.Text = "CONTO ECONOMICO AL "
            XrTableCell3.Text = "COSTI"
        End If
    End Sub

End Class