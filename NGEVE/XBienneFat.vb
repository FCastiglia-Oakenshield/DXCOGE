Public Class XBienneFat

    Private Sub XrLabel_PrintOnPage(ByVal sender As Object, ByVal e As DevExpress.XtraReports.UI.PrintOnPageEventArgs) Handles XrLabel5.PrintOnPage, XrLabel6.PrintOnPage, XrLabel7.PrintOnPage, XrLabel8.PrintOnPage, XrLabel9.PrintOnPage, XrLabel10.PrintOnPage, XrLabel11.PrintOnPage, XrLabel12.PrintOnPage, XrLabel13.PrintOnPage, XrLabel14.PrintOnPage, XrLabel15.PrintOnPage, XrLabel16.PrintOnPage, XrLabel35.PrintOnPage, XrLabel38.PrintOnPage, XrLabel39.PrintOnPage, XrLabel40.PrintOnPage, XrLabel41.PrintOnPage, XrLabel42.PrintOnPage, XrLabel43.PrintOnPage, XrLabel44.PrintOnPage
        If e.PageCount <> (e.PageIndex + 1) Then e.Cancel = True
    End Sub
End Class