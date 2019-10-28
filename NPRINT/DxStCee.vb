Public Class DxStCee
    Dim Descri As String
    Private Sub Xrlabel3_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel3.BeforePrint
        Descri = ""
        Descri = XrLabel3.Text
        If XrLabel4.Text = "0" Then
            XrLabel3.Text = "             " & Descri
        ElseIf XrLabel4.Text = "9" Then
            XrLabel3.Text = "                    " & Descri
        ElseIf XrLabel4.Text = "8" Then
            XrLabel3.Text = Descri
        ElseIf XrLabel4.Text = "7" Then
            XrLabel3.Text = "   " & Descri
        ElseIf XrLabel4.Text = "6" Then
            XrLabel3.Text = "     " & Descri
        ElseIf XrLabel4.Text = "5" Then
            XrLabel3.Text = "       " & Descri
        ElseIf XrLabel4.Text = "4" Then
            XrLabel3.Text = "         " & Descri
        Else
            XrLabel3.Text = Descri
        End If
    End Sub
End Class