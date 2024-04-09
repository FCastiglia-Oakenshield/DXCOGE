Imports System.Drawing.Printing
Imports DevExpress.XtraReports.UI
Public Class XLpBuste

    Private Sub CITTA_1_BeforePrint(sender As Object, e As PrintEventArgs) Handles CITTA_1.BeforePrint 'E' il campo CITTA' scritto sotto 
        Dim rw As DataRowView = Me.GetCurrentRow()                                                           'con CAP,città e provincia.
        CITTA_1.Text = rw("AnaCap") & " " & rw("AnaCitta") & " " & rw("AnaProv")  'AnaProv viene stampato lontano dal resto
        'CITTA_1.Text = rw("AnaProv") & " " & rw("AnaCap") & " " & rw("AnaCitta") 'Prova mia,vien scritto senza spazi
    End Sub

End Class