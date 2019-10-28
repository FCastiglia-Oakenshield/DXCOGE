
Public Class DxCommesse
    Dim tp As Decimal = 0
    Dim te As Decimal = 0
    Dim tt As Decimal = 0
    Private Sub XrLabel6_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel6.BeforePrint
        If XrLabel9.Text = 0 Then
            XrLabel6.Text = " Situazione Patrimoniale"
        Else
            XrLabel6.Text = " Conto Economico"
        End If
    End Sub
End Class