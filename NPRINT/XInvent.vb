Imports System.Drawing.Printing
Imports DevExpress.XtraReports.UI
Public Class XInvent

    Private Sub SDOCONTO_1_BeforePrint(sender As Object, e As PrintEventArgs) Handles SDOCONTO_1.BeforePrint    'Visualizza il campo SDOCONTO_1 sotto Parziali
        Dim rw As DataRowView = Me.GetCurrentRow()
        If rw("TMSDARE") <> 0 Then
            SDOCONTO_1.Text = rw("TMSDARE")
        Else
            SDOCONTO_1.Text = rw("TMSAVERE")
        End If

    End Sub

    '--------------------------------
    ' Per SDOCONTO
    'IF {TMPBINV.TMSDARE} <> 0 THEN {TMPBINV.TMSDARE} ELSE {TMPBINV.TMSAVERE}


    '------------------------------------------------------------------------------


    'Sistemare PERDITA_1

    'Private Sub PERDITA_1_BeforePrint(sender As Object, e As PrintEventArgs) Handles PERDITA_1.BeforePrint    'Visualizza il campo PERDITA_1 sotto Parziali
    '    Dim rw As DataRowView = Me.GetCurrentRow()                                                            '48.02 PERDITE campo a sinistra.
    '    If {@DIFFE}) >= 0 And {@BILANCIO} = 0 Then
    '        PERDITA_1.Text = "0" Else
    '        If {@DIFFE} < 0 And {@BILANCIO} = 0  Then
    '            PERDITA_1.Text = "{@DIFFE} * -1"Else
    '        If {@DIFFE} >= 0 & {@BILANCIO} = 1  Then
    '                PERDITA_1.Text = "0"
    '                If {@DIFFE} < 0 And {@BILANCIO} = 1  Then
    '                    PERDITA_1.Text = "{@DIFFE} * -1"


    '                End If
    '            End If
    '        End If
    '    End If


    'End Sub
    '--------------------------------
    ' Per PERDITA_1
    'If {@DIFFE} >= 0 And {@BILANCIO} = 0  Then 0 Else 
    'If {@DIFFE} < 0 And {@BILANCIO} = 0  Then {@DIFFE} * -1 Else 
    'If {@DIFFE} >= 0 And {@BILANCIO} = 1  Then 0  Else 
    'If {@DIFFE} < 0 And {@BILANCIO} = 1  Then {@DIFFE} * -1
    '--------------------------------






    'Sistemare TOTPAG_1
    'Private Sub TOTPAG_1_BeforePrint(sender As Object, e As PrintEventArgs) Handles TOTPAG_1.BeforePrint    'Visualizza il campo TOTPAG_1 che indica il numero di
    '    Dim rw As DataRowView = Me.GetCurrentRow()                                                          'totale di pagine
    '    Dim Totpg As Int16
    '    Totpg = "NPAG" + "PageNumber"         'Sistemare sintassi.
    '    TOTPAG_1.Text = Totpg
    'End Sub


    'TOTPAG {?NPAG} + PageNumber







    ''Esempio di stampa di un campo
    'Private Sub Sconti_1_BeforePrint(sender As Object, e As PrintEventArgs) Handles Sconti_1.BeforePrint 'Stampa uno sconto se uno dei due è zero
    '    Dim rw As DataRowView = Me.GetCurrentRow()                                                       'Stampa "sc1+sc2" se ci sono due sconti
    '    Dim Sconti As String                                                                             'E' relativo al campo "SCONTO%",se non ci sono sconti,non stampa nulla
    '    Sconti = ""
    '    If rw("SC1") > 0 Then Sconti = Sconti & rw("SC1")
    '    If rw("SC2") > 0 Then Sconti = Sconti & "+" & rw("SC2")
    '    Sconti_1.Text = Sconti
    'End Sub










End Class