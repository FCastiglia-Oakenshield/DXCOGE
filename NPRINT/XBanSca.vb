Imports System.Drawing.Printing
Imports DevExpress.XtraReports.UI
Public Class XBanSca

    Private Sub DESCRIBAN1_BeforePrint(sender As Object, e As PrintEventArgs) Handles DESCRIBAN_1.BeforePrint 'Comanda come stampare DESCRIBAN sopra,è sopra Codice e Ragione Sociale
        Dim rw As DataRowView = Me.GetCurrentRow()
        If rw("ScaBan") = "0" Then
            DESCRIBAN_1.Text = "DA ASSEGNARE"
        Else
            DESCRIBAN_1.Text = rw("BANDES")
        End If
    End Sub

    Private Sub DESCRIBAN2_BeforePrint(sender As Object, e As PrintEventArgs) Handles DESCRIBAN_2.BeforePrint 'Comanda come stampare DESCRIBAN sotto,è sotto riga blu,alla fine della tabella.
        Dim rw As DataRowView = Me.GetCurrentRow()
        Dim banca As String
        If rw("ScaBan") = "0" Then
            DESCRIBAN_2.Text = "DA ASSEGNARE"
        Else
            banca = rw("BANDES")
            DESCRIBAN_2.Text = banca.ToString
        End If
    End Sub


    'DESCRIBAN: If {CRG1.ScaBan} = 0 Then "DA ASSEGNARE" Else {CRG1.BANDES}

End Class