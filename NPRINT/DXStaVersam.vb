Public Class DXStaVersam
    Private Sub XrLabel1_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel1.BeforePrint
        If XrLabel1.Text = "0" Then XrLabel1.Text = ""
    End Sub
    Private Sub XrLabel2_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel2.BeforePrint
        If XrLabel2.Text = "0" Then XrLabel2.Text = ""
    End Sub
End Class