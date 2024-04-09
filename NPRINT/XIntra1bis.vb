Imports System.Drawing.Printing
Imports DevExpress.XtraReports.UI

Public Class XIntra1bis


    Private Sub Section11_BeforePrint(sender As Object, e As PrintEventArgs) Handles Section11.BeforePrint 'Comanda apparizione campo dopo 10 righe.
        Dim rw As DataRowView = Me.GetCurrentRow()
        Dim C1 As Int16 = rw("PROG") / 10
        Dim C2 As Int16 = C1 * 10
        If rw("PROG") <> C2 Then
            e.Cancel = True
        End If

        'L'istruzione e.Cancel = True 'Non lo fa stampare
        'e.Cancel = False


    End Sub


    'Mettere un before print per section11.
    'Eccezioni CRISTAL REPORT:
    ' Per PAGINA:   IF {TMPINTRA.PROG} < 10 THEN 1 ELSE {TMPINTRA.PROG} / 10 
    ' Per C1    :   INT({TMPINTRA.PROG} / 10) Per fare avere un numero intero. 11/10=1,1.quindi prendo 1,12/10=1,2.prendo 1
    ' Per C2    :   {@C1} * 10                Per fare avere un numero intero.
    '{TMPINTRA.PROG} <> {@C2}


End Class