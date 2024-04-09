Imports System.Drawing.Printing
Imports DevExpress.XtraReports.UI

Imports DevExpress.XtraReports                  'Richiesti da controllo errori per parte break
Imports DevExpress.XtraReports.Localization     'Richiesti da controllo errori per parte break

Public Class XClFoSca
    Dim Control As String
    Private Sub Field35_BeforePrint(sender As Object, e As PrintEventArgs) Handles Field35.BeforePrint 'Comanda come stampare DESCRIBAN sopra,è sopra Codice e Ragione Sociale
        Dim rw As DataRowView = Me.GetCurrentRow()
        If rw("ScaBan") = "0" Then
            Field35.Text = "DA ASSEGNARE"
        Else
            Field35.Text = rw("BANDES")
        End If
    End Sub

    Private Sub Field6_BeforePrint(sender As Object, e As PrintEventArgs) Handles Field6.BeforePrint 'Comanda come stampare DESCRIBAN sotto,è sotto riga blu,alla fine della tabella.
        Dim rw As DataRowView = Me.GetCurrentRow()
        Dim banca As String
        If rw("ScaBan") = "0" Then
            Field6.Text = "DA ASSEGNARE"
        Else
            banca = rw("BANDES")
            Field6.Text = banca.ToString
        End If
    End Sub 'Fino a qui ok.Da qui in poi sistemare.
    ' Questa parte potrebbe controllare l'interruzione,vedere come usarla:detailBand.PageBreak = PageBreak.AfterBand


    ''Parte qui sotto può servire per fare la somma di ratarb e ratard.
    'Private Sub Field16_BeforePrint(sender As Object, e As PrintOnPageEventArgs) Handles Field16.BeforePrint 'Comanda se stampare Field21
    '    Dim rw As DataRowView = Me.GetCurrentRow()
    '    Dim f35 As Decimal = Field35.Summary.GetResult   ' Ho creato una variabile relativa al Field 35.
    '    Dim f6 As Decimal = Field6.Summary.GetResult   ' Ho creato una variabile relativa al Field 6



    'End Sub









    'Per field 16,17 somma delle colonne sopra e 21,22 fare somme dei totali clienti e fornitori.

    'Per ratarb:if {CRG1.CLFOPI} = "CL"  then {CRG1.ScaScopRata} else 0
    'Per ratard:If {CRG1.CLFOPI} = 'FO' THEN {CRG1.ScaScopRata} ELSE 0 













End Class