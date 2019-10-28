Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class DxRicArc
    Dim Rpt As ReportClass
    Dim Rpt1 As New StL8
    Dim str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str11, periodo, strstr As String
    Dim CDARE, CAVERE, BR, DADATA, ADATA, PAG As String
    Dim NPROT As Int32
    Dim CAUS, CIVA, BS, MERCE, RIVA As Int16
    Dim controllo As Boolean
    Dim Rispondi As MsgBoxResult
    Private Sub DxRicArc_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia()
    End Sub
    Private Sub Pulizia()
        DateEdit1.EditValue = "01/01/" & Year(Today)
        DateEdit2.EditValue = Today
        TextEdit1.EditValue = ""
        TextEdit2.EditValue = ""
        TextEdit3.EditValue = ""
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = ""
        TextEdit6.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = ""
        TextEdit9.EditValue = ""
        TextEdit10.EditValue = ""
        TextEdit11.EditValue = ""
        TextEdit10.Focus()
    End Sub
    Private Sub preparaparametri()
        Cursor.Current = Cursors.WaitCursor

        DADATA = "date(" & CDate(DateEdit1.EditValue).Year & "," & CDate(DateEdit1.EditValue).Month & "," & CDate(DateEdit1.EditValue).Day & ")"
        ADATA = "date(" & CDate(DateEdit2.EditValue).Year & "," & CDate(DateEdit2.EditValue).Month & "," & CDate(DateEdit2.EditValue).Day & ")"
        str0 = "{CRL8.Datagio}>= " & DADATA & " AND {CRL8.Datagio}<= " & ADATA & " "
        strstr = ""


        'Conto Dare
TB1:
        str1 = ""
        If Val(TextEdit1.EditValue) > 1000 Then
            CDARE = Format(Val(TextEdit1.EditValue), "00000")
        Else
            CDARE = TextEdit1.EditValue
        End If

        If Trim(TextEdit1.EditValue) = "" Then
            GoTo TB2
        End If

        str1 = " AND {CRL8.CODARE} = '" & CDARE & "' "
        'Conto Avere
TB2:
        str2 = ""
        If Val(TextEdit2.EditValue) > 1000 Then
            CAVERE = Format(Val(TextEdit2.EditValue), "00000")
        Else
            CAVERE = TextEdit2.EditValue
        End If

        If Trim(TextEdit2.EditValue) = "" Then
            GoTo TB3
        End If

        str2 = " AND {CRL8.COAVERE} = '" & CAVERE & "' "
        'Causale
TB3:
        str3 = ""
        CAUS = Val(TextEdit3.EditValue)
        If Trim(TextEdit3.EditValue) = "" Then
            GoTo TB4
        End If

        str3 = " AND {CRL8.PriCausale} = " & CAUS & " "
        'Codice iva
TB4:
        str4 = ""
        CIVA = Val(TextEdit4.EditValue)
        If Trim(TextEdit4.EditValue) = "" Then
            GoTo TB5
        End If
        str4 = " AND {CRL8.CodiceIva} = " & CIVA & " "
        'Registro Iva
TB5:
        str5 = ""
        RIVA = Val(TextEdit5.EditValue)
        If Trim(TextEdit5.EditValue) = "" Then
            GoTo TB6
        End If

        str5 = " AND {CRL8.RegIva} = " & RIVA & " "
        'numero di protocollo
TB6:
        str6 = ""
        NPROT = Val(TextEdit6.EditValue)
        If Trim(TextEdit6.EditValue) = "" Then
            GoTo TB7
        End If

        str5 = " AND {CRL8.numpro} = " & NPROT & " "
        'Bis - ret
TB7:
        str7 = ""
        BR = TextEdit7.EditValue
        If Trim(TextEdit7.EditValue) = "" Then
            GoTo TB8
        End If

        str7 = " AND {CRL8.bis} = '" & BR & "' "
        'BENI STRUMENTALI 
TB8:
        str8 = ""
        BS = Val(TextEdit8.EditValue)
        If Trim(TextEdit8.EditValue) = "" Then
            GoTo TB9
        End If

        str8 = " AND {CRL8.PriFl04} = " & BS & " "
        'Merce
TB9:
        str9 = ""
        MERCE = Val(TextEdit9.EditValue)
        If Trim(TextEdit9.EditValue) = "" Then
            GoTo TB11
        End If

        str9 = " AND {CRL8.PriFl06} = " & MERCE
TB11:
        'Codice Pagamento
        str11 = ""
        PAG = Val(TextEdit11.EditValue)
        If Trim(TextEdit11.EditValue) = "" Then
            PAG = ""
            GoTo finale
        End If

        str11 = " AND {CRL8.PriCodPag} = " & PAG
finale:
        strstr = str0 & str1 & str2 & str3 & str4 & str5 & str6 & str7 & str8 & str9 & str11
    End Sub
    Function Controlli() As Boolean
        Controlli = True
        If TextEdit1.EditValue = "" And TextEdit2.EditValue = "" And TextEdit3.EditValue = "" And TextEdit4.EditValue = "" And TextEdit5.EditValue = "" And TextEdit6.EditValue = "" _
            And TextEdit7.EditValue = "" And TextEdit8.EditValue = "" And TextEdit9.EditValue = "" And TextEdit11.EditValue = "" Then
            Messaggio(1, "NESSUN PARAMETRO SELEZIONATO !!!")
            Controlli = False
        End If
        If CDate(DateEdit2.EditValue) < CDate(DateEdit1.EditValue) Then
            Messaggio(1, "VERIFICARE PERIODO DAL - AL !!!")
            Controlli = False
        End If
        If Val(TextEdit3.EditValue) > 72 Then
            Messaggio(1, "CAUSALE INESISTENTE !!! ")
            Controlli = False
        End If
        If Val(TextEdit4.EditValue) > 72 Then
            Messaggio(1, "CODICE IVA INESISTENTE !!! ")
            Controlli = False
        End If
        If TextEdit7.EditValue <> "" And TextEdit7.EditValue <> "B" And TextEdit7.EditValue <> "R" Then
            Messaggio(1, "VALORI AMMESSI B o R!!! ")
            Controlli = False
        End If
        If Val(TextEdit9.EditValue) > 2 Then
            Messaggio(1, "VALORI AMMESSI 0 1 2 !!! ")
            Controlli = False
        End If
    End Function
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "Ricerche Su Movimenti"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim selectformula As String
        If Controlli() = False Then
            TextEdit10.Focus()
            Exit Sub
        End If
        preparaparametri()
        Cursor.Current = Cursors.WaitCursor
        Dim frm As New LpLp
        Rpt = New ReportClass
        Rpt1 = New StL8
        Rpt = Rpt1
        periodo = "Periodo " & DateEdit1.EditValue & " - " & DateEdit2.EditValue
        Cursor.Current = Cursors.WaitCursor
        selectformula = strstr
        Rpt.RecordSelectionFormula = selectformula
        Rpt.SetParameterValue("TMERCE", MERCE)
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("TITOLOREPORT", TextEdit10.EditValue)
        Rpt.SetParameterValue("periodo", periodo)
        Rpt.SetParameterValue("CDARE", CDARE)
        Rpt.SetParameterValue("CAVERE", CAVERE)
        Rpt.SetParameterValue("CAUS", CAUS)
        Rpt.SetParameterValue("CIVA", CIVA)
        Rpt.SetParameterValue("NPROT", NPROT)
        Rpt.SetParameterValue("BR", BR)
        Rpt.SetParameterValue("BS", BS)
        Rpt.SetParameterValue("RIVA", RIVA)
        Rpt.SetParameterValue("PAG", PAG)
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 And GroupControl14.Enabled = True Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 And GroupControl14.Enabled = True Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

End Class