Public Class DxStaEstratto
    Dim t1 As Decimal = 0 ' TOTALE DARE CLIENTE
    Dim t2 As Decimal = 0 ' TOTALE AVERE CLIENTE
    Dim t3 As Decimal = 0 ' SCOPERTO PER PARTITA
    Dim ok As Boolean = True
    Dim r As Int16 = -1
    Private Sub XrTableCell20_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell20.BeforePrint
        Dim rw As DataRowView = GetCurrentRow()
        XrTableCell20.Text = ""
        If rw IsNot Nothing Then
            If rw("TotaleFattura") <> 0 Then
                XrTableCell20.Text = Format(rw("TotaleFattura"), "#,###,###,##0.00")
            End If
        End If
    End Sub
    Private Sub XrTableCell25_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell25.BeforePrint
        Dim rw As DataRowView = GetCurrentRow()

        If rw IsNot Nothing Then t1 += CDec(rw("DARE"))
        If XrTableCell25.Text <> "" AndAlso CDec(XrTableCell25.Text) = 0 Then XrTableCell25.Text = ""
    End Sub
    Private Sub XrTableCell26_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell26.BeforePrint
        Dim rw As DataRowView = GetCurrentRow()
        If rw IsNot Nothing Then t2 += CDec(rw("AVERE"))
        If XrTableCell26.Text <> "" AndAlso CDec(XrTableCell26.Text) = 0 Then XrTableCell26.Text = ""
    End Sub
    Private Sub XrTableCell27_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell27.BeforePrint
        Dim rw As DataRowView = GetCurrentRow()
        XrTableCell27.Text = ""
        If rw IsNot Nothing Then
            If rw("PriCausale") = 3 Then
                XrTableCell27.Text = Format(rw("ScaDsca"), "dd/MM/yyyy")
            End If
        End If
    End Sub
    Private Sub XrTableCell2_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell2.BeforePrint
        Dim rw As DataRowView = GetCurrentRow()
        XrTableCell2.Text = ""
        If rw IsNot Nothing Then
            If rw("PriCausale") = 3 Then XrTableCell2.Text = rw("ScaNrata")
        End If
    End Sub
    Private Sub XrTableCell35_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell35.BeforePrint
        t3 = t1 - t2
        XrTableCell35.Text = Format(t3, "#,###,###,##0.00")
    End Sub
End Class