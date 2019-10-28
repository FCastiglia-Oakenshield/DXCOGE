Public Class DxBilCee
    Dim Descri As String
    Dim St As Int16
    Private Sub XrTableCell6_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell6.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        Descri = rw("BILCEEDES")
        St = rw("BILCEEST")
        If St = 9 Then
            XrTableCell6.Text = "                    " & Descri
        ElseIf St = 8 Then
            XrTableCell6.Text = Descri
        ElseIf St = 7 Then
            XrTableCell6.Text = "   " & Descri
        End If

        If St < 7 Then
            If Parameters.Item("DettaglioCee").Value = False Then
                Descri = rw("BILCODICE") & "  " & rw("BILDESCR")
            Else
                Descri = rw("BILCEEDES")
            End If
            XrTableCell6.Text = "     " & Descri
        End If

        'If St = 0 Then
        '    XrTableCell6.Text = "             " & Descri
        'ElseIf St = 9 Then
        '    XrTableCell6.Text = "                    " & Descri
        'ElseIf St = 8 Then
        '    XrTableCell6.Text = Descri
        'ElseIf St = 7 Then
        '    XrTableCell6.Text = "   " & Descri
        'ElseIf St = 6 Then
        '    XrTableCell6.Text = "     " & Descri
        'ElseIf St = 5 Then
        '    XrTableCell6.Text = "       " & Descri
        'ElseIf St = 4 Then
        '    XrTableCell6.Text = "         " & Descri
        'Else
        '    XrTableCell6.Text = Descri
        'End If
    End Sub

    Private Sub XrTableCell7_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell7.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If St > 6 Then
            XrTableCell7.Text = ""
        ElseIf CDec(rw("BILSALDO")) <> 0 Then
            XrTableCell7.Text = Format(Math.Abs(rw("BILSALDO")), "n2")
        Else
            XrTableCell7.Text = ""
        End If
    End Sub
    Private Sub XrTableCell1_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell1.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If St > 6 Then
            XrTableCell1.Text = Format(Math.Abs(rw("BILSALDO")), "n2")
        Else
            XrTableCell1.Text = ""
        End If
    End Sub

    Private Sub XrTableCell5_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell5.BeforePrint
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        XrTableCell5.Text = ""
        '' If St < 7 Then
        If CDec(rw("BILSALDO")) > 0 Then
            XrTableCell5.Text = "D"
        ElseIf CDec(rw("BILSALDO")) < 0 Then
            XrTableCell5.Text = "A"
        Else
            XrTableCell5.Text = ""
        End If
        '' End If
    End Sub
    Private Sub XrTableCell9_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell9.BeforePrint
        If Parameters.Item("OneYear").Value = True Then XrTableCell9.Text = "" : Exit Sub
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If St > 6 Then
            XrTableCell9.Text = ""
        ElseIf CDec(rw("BILSALDOA")) <> 0 Then
            XrTableCell9.Text = Format(Math.Abs(rw("BILSALDOA")), "n2")
        Else
            XrTableCell9.Text = ""
        End If
    End Sub
    Private Sub XrTableCell11_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell11.BeforePrint
        If Parameters.Item("OneYear").Value = True Then XrTableCell11.Text = "" : Exit Sub
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        If St > 6 Then
            XrTableCell11.Text = Format(Math.Abs(rw("BILSALDOA")), "n2")
        Else
            XrTableCell11.Text = ""
        End If
    End Sub

    Private Sub XrTableCell10_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell10.BeforePrint
        If Parameters.Item("OneYear").Value = True Then XrTableCell10.Text = "" : Exit Sub
        Dim rw As DataRow = Me.DataSource.rows(Me.CurrentRowIndex)
        XrTableCell10.Text = ""
        ''  If St < 7 Then
        If CDec(rw("BILSALDOA")) > 0 Then
            XrTableCell10.Text = "D"
        ElseIf CDec(rw("BILSALDOA")) < 0 Then
            XrTableCell10.Text = "A"
        Else
            XrTableCell10.Text = ""
        End If
        '' End If
    End Sub

    Private Sub XrTableCell12_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell12.BeforePrint, XrTableCell14.BeforePrint
        If Parameters.Item("OneYear").Value = True Then XrTableCell12.Text = "" : XrTableCell14.Text = ""
    End Sub

End Class