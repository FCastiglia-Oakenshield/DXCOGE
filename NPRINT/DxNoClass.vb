Public Class DxNoClass
    Dim Descri As String
    Dim St As Int16
    Private Sub XrTableCell3_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell3.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        Descri = rw("PIACODCO") & " " & rw("PIAANACO")
        XrTableCell3.Text = Descri
    End Sub
End Class