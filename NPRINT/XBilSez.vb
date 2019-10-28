Public Class XBilSez
    Dim T1 As Decimal = 0
    Dim T2 As Decimal = 0
    Dim T3 As Decimal = 0
    Dim T4 As Decimal = 0
    Dim T5 As Decimal = 0
    Dim T6 As Decimal = 0

    Private Sub XrLabel3_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel3.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If rw("TMPSPPP") = 0 Then XrLabel3.Text = "SITUAZIONE PATRIMONIALE" Else XrLabel3.Text = "CONTO ECONOMICO"
    End Sub

    Private Sub XrTableCell1_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell1.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If rw("TMPSPPP") = 0 Then XrTableCell1.Text = "ATTIVITA'" Else XrTableCell1.Text = "COMPONENTI NEGATIVE DEL REDDITO"
    End Sub

    Private Sub XrTableCell4_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell4.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If rw("TMPSPPP") = 0 Then XrTableCell4.Text = "PASSIVITA'" Else XrTableCell4.Text = "COMPONENTI POSITIVE DEL REDDITO"
    End Sub


    Private Sub XrTableCell5_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell5.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If rw("TMPS1SALDO") <> 0 Then XrTableCell5.Text = Format(rw("TMPS1SALDO"), "#,##0.00") Else XrTableCell5.Text = ""
        If rw("TMPSPPP") = 0 And Mid(rw("TMPS1CODICE"), 4, 2) = "00" Then T1 += rw("TMPS1SALDO")
        If rw("TMPSPPP") = 1 And Mid(rw("TMPS1CODICE"), 4, 2) = "00" Then T3 += rw("TMPS1SALDO")
    End Sub
    Private Sub XrTableCell14_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell14.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        Dim S As Int16 = -1
        If rw("TMPS2SALDO") <> 0 Then XrTableCell14.Text = Format(rw("TMPS2SALDO") * S, "#,##0.00") Else XrTableCell14.Text = ""
        If rw("TMPSPPP") = 0 And Mid(rw("TMPS2CODICE"), 4, 2) = "00" Then T2 += rw("TMPS2SALDO") * S
        If rw("TMPSPPP") = 1 And Mid(rw("TMPS2CODICE"), 4, 2) = "00" Then T4 += rw("TMPS2SALDO") * S
    End Sub

    Private Sub XrTableCell2_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell2.BeforePrint, XrTableCell3.BeforePrint, XrTableCell5.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If Mid(rw("TMPS1CODICE"), 4, 2) = "00" Then
            Me.XrTableCell2.Font = New System.Drawing.Font("Calibri", 8.0!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle))
            Me.XrTableCell3.Font = New System.Drawing.Font("Calibri", 8.0!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle))
            Me.XrTableCell5.Font = New System.Drawing.Font("Calibri", 8.0!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle))
        Else
            Me.XrTableCell2.Font = New System.Drawing.Font("Calibri", 8.0!, FontStyle.Regular)
            Me.XrTableCell3.Font = New System.Drawing.Font("Calibri", 8.0!, FontStyle.Regular)
            Me.XrTableCell5.Font = New System.Drawing.Font("Calibri", 8.0!, FontStyle.Regular)
        End If
    End Sub
    Private Sub XrTableCell6_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell6.BeforePrint, XrTableCell13.BeforePrint, XrTableCell14.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If Mid(rw("TMPS2CODICE"), 4, 2) = "00" Then
            Me.XrTableCell6.Font = New System.Drawing.Font("Calibri", 8.0!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle))
            Me.XrTableCell13.Font = New System.Drawing.Font("Calibri", 8.0!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle))
            Me.XrTableCell14.Font = New System.Drawing.Font("Calibri", 8.0!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle))
        Else
            Me.XrTableCell6.Font = New System.Drawing.Font("Calibri", 8.0!, FontStyle.Regular)
            Me.XrTableCell13.Font = New System.Drawing.Font("Calibri", 8.0!, FontStyle.Regular)
            Me.XrTableCell14.Font = New System.Drawing.Font("Calibri", 8.0!, FontStyle.Regular)
        End If
    End Sub

    Private Sub XrTableCell17_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell17.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If rw("TMPSPPP") = 0 Then XrTableCell17.Text = Format(T1, "#,##0.00") Else XrTableCell17.Text = Format(T3, "#,##0.00")

    End Sub
    Private Sub XrTableCell20_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell20.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If rw("TMPSPPP") = 0 Then XrTableCell20.Text = Format(T2, "#,##0.00") Else XrTableCell20.Text = Format(T4, "#,##0.00")
    End Sub

    Private Sub XrTableCell22_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell22.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        Select Case rw("TMPSPPP")
            Case 0
                T5 = T1 - T2
                If T5 > 0 Then XrTableCell22.Text = "" Else XrTableCell22.Text = "ECCEDENZA PASSIVA"
            Case 1
                T6 = T3 - T4
                If T6 > 0 Then XrTableCell22.Text = "" Else XrTableCell22.Text = "UTILE"
        End Select
        If XrTableCell22.Text = "ECCEDENZA PASSIVA" Then XrTableCell22.ForeColor = System.Drawing.Color.Red Else XrTableCell22.ForeColor = System.Drawing.Color.Black
    End Sub
    Private Sub XrTableCell25_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell25.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        Select rw("TMPSPPP")
            Case 0
                T5 = T1 - T2
                If T5 <= 0 Then XrTableCell25.Text = "" Else XrTableCell25.Text = "ECCEDENZA ATTIVA"
            Case 1
                T6 = T3 - T4
                If T6 > 0 Then XrTableCell25.Text = "PERDITA" Else XrTableCell25.Text = ""
        End Select
        If XrTableCell25.Text = "PERDITA" Then XrTableCell25.ForeColor = System.Drawing.Color.Red Else XrTableCell25.ForeColor = System.Drawing.Color.Black
    End Sub

    Private Sub XrTableCell23_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell23.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        Select Case rw("TMPSPPP")
            Case 0
                If T5 > 0 Then XrTableCell23.Text = "" Else XrTableCell23.Text = Format(T5 * -1, "#,##0.00")
            Case 1
                If T6 > 0 Then XrTableCell23.Text = "" Else XrTableCell23.Text = Format(T6 * -1, "#,##0.00")
        End Select
        If XrTableCell22.Text = "ECCEDENZA PASSIVA" Then XrTableCell23.ForeColor = System.Drawing.Color.Red Else XrTableCell23.ForeColor = System.Drawing.Color.Black
    End Sub

    Private Sub XrTableCell26_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell26.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        Select Case rw("TMPSPPP")
            Case 0
                If T5 <= 0 Then XrTableCell26.Text = "" Else XrTableCell26.Text = Format(T5, "#,##0.00")
            Case 1
                If T6 > 0 Then XrTableCell26.Text = Format(T6, "#,##0.00") Else XrTableCell26.Text = ""
        End Select
        If XrTableCell25.Text = "PERDITA" Then XrTableCell26.ForeColor = System.Drawing.Color.Red Else XrTableCell25.ForeColor = System.Drawing.Color.Black
    End Sub

    Private Sub XrTableCell29_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell29.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        Select Case rw("TMPSPPP")
            Case 0
                If T1 > T2 Then XrTableCell29.Text = Format(T1, "#,##0.00") Else XrTableCell29.Text = Format(T2, "#,##0.00")
            Case 1
                If T3 > T4 Then XrTableCell29.Text = Format(T3, "#,##0.00") Else XrTableCell29.Text = Format(T4, "#,##0.00")
        End Select
    End Sub
    Private Sub XrTableCell32_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell32.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        Select Case rw("TMPSPPP")
            Case 0
                If T1 > T2 Then XrTableCell32.Text = Format(T1, "#,##0.00") Else XrTableCell32.Text = Format(T2, "#,##0.00")
            Case 1
                If T3 > T4 Then XrTableCell32.Text = Format(T3, "#,##0.00") Else XrTableCell32.Text = Format(T4, "#,##0.00")
        End Select
    End Sub
End Class