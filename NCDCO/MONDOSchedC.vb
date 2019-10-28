Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports NPRINT

Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Public Class MONDOSchedC
    Friend WithEvents prntDoc As System.Drawing.Printing.PrintDocument
    Dim prntDial As New PrintDialog


    Dim DsCom As DataTable
    Dim DaCom As SqlDataAdapter
    Dim RwCom As DataRow

    Dim sw As Int16 = 0

    Dim REPORT As New XtraReport
    Dim selectformula, SCRI, StrPrint, Formula, Periodo, ForExp As String

    Dim DsBilCdc As DataTable
    Dim DaBilCdc As SqlDataAdapter

    Dim TbExp As DataTable
    Dim DaExp As SqlDataAdapter

    Dim L1 As String = "PrkAAmmgg"   'Procedura NO COMPETENZA
    Dim L1x As String = "[Data Giornale]"
    Dim T2 As String = ""
    Dim DataMin As Date
    Dim DataMax As Date

    Private Sub MONDOSk_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If sw = 0 Then
            CheckEdit1.Checked = False
            DateEdit1.EditValue = Nothing
            DateEdit4.EditValue = Nothing
            sw = 1
        End If
        POPOLACOMMESSE()
    End Sub
    Sub POPOLACOMMESSE()
        Dim sstr As String = "Select *,STAMPA = cast(" & RadioGroup2.EditValue & " as bit) from VCommdcdc WHERE TcmAu = " & RadioGroup1.SelectedIndex & " order by TcmAnno desc, TcmNum desc"
        DsCom = New DataTable
        DaCom = New SqlDataAdapter(sstr, cnDb)
        DaCom.Fill(DsCom)
        GridControl3.DataSource = DsCom
        GridView4.ClearSelection()
    End Sub
  
  
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If ControllaCampi() = False Then Exit Sub
        Stampa()
    End Sub


    Function ControllaCampi() As Boolean
        Dim Mail As String = ""
        Dim XX As Int16 = 0
        For pp As Int16 = 1 To GridView4.DataRowCount
            Rw = GridView4.GetDataRow(pp - 1)
            If Rw("STAMPA") = True Then
                XX = XX + 1
            End If
        Next
        If XX = 0 Then
            Mail = Mail & "<> SELEZIONARE ALMENO UNA COMMESSA " & Chr(13)
        End If
        If RadioGroup1.SelectedIndex = 0 Then
            If DateEdit1.EditValue Is Nothing Or DateEdit4.EditValue Is Nothing Then
                Mail = Mail & "<> STAMPA ANNUALI OBBLIGATORIO SELEZIONARE PERIODO ANNUALE " & Chr(13)
            ElseIf CDate(DateEdit1.EditValue).Year <> CDate(DateEdit4.EditValue).Year Then
                Mail = Mail & "<> STAMPA ANNUALI SELEZIONATO PERIODO ULTRANNUALE " & Chr(13)
            End If
        Else
            If DateEdit1.EditValue Is Nothing And DateEdit4.EditValue Is Nothing Then
                Mail = Mail & ""
            ElseIf DateEdit1.EditValue Is Nothing Then
                Mail = Mail & "<> STAMPA INFRANNUALI SELEZIONARE DATA O PULISCI DATE " & Chr(13)
            ElseIf DateEdit4.EditValue Is Nothing Then
                Mail = Mail & "<> STAMPA INFRANNUALI SELEZIONARE DATA O PULISCI DATE " & Chr(13)
            ElseIf DateEdit4.EditValue < DateEdit1.EditValue Then
                Mail = Mail & "<> STAMPA INFRANNUALI SELEZIONARE DATE CORRETTE " & Chr(13)
            End If
        End If
        If Mail > "" Then
            MoltoCritico(Mail)
            Return False
        End If
            Return True
    End Function
    Private Sub ButtonExp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExp.Click
        If ControllaCampi() = False Then Exit Sub
        Exporto()
    End Sub
    Sub Exporto()
        Cursor = Cursors.WaitCursor
        Dim LINDO As Boolean = False
        PreparoFormula()
        If DateEdit1.EditValue Is Nothing And DateEdit4.EditValue Is Nothing Then
            DateEdit1.EditValue = CDate(DataMin)
            DateEdit4.EditValue = CDate(DataMax)
            LINDO = True
        End If
        Dim RR As String = ""
        Dim Lancio As String = "select * from VCOMMH8DETT where ("
        Dim seledata As String = ") and ( " & L1x & " between '" & DateEdit1.EditValue.ToShortDateString & "' and '" & DateEdit4.EditValue.ToShortDateString & "')"
        Dim LancioR As String = "select * from RCOMMH8DETT where ("
        Dim seledataR As String = ") and " & L1x & " = '" & DateEdit4.EditValue.ToShortDateString & "'"
        Dim Order As String = " order by [Commessa], [Conto] ," & L1x & ""
        Dim Str As String = ""
        If CheckEdit8.Checked = True Then
            RR = "-R-"
            Str = Lancio & ForExp & seledata & " UNION " & LancioR & ForExp & seledataR & Order
        Else
            RR = ""
            Str = Lancio & ForExp & seledata & Order
        End If
        TbExp = New DataTable()
        DaExp = New SqlDataAdapter(Str, CnDc)
        DaExp.Fill(TbExp)

        GridControl2.DataSource = TbExp
        Cursor = Cursors.Default
        DXANTEPRIMA(GridControl2, True, Printing.PaperKind.A3, "")
        If LINDO = True Then
            DateEdit1.EditValue = Nothing : DateEdit4.EditValue = Nothing
        End If
    End Sub

    Sub Stampa()
        Dim LINDO As Boolean = False
        PreparoFormula()
        If DateEdit1.EditValue Is Nothing And DateEdit4.EditValue Is Nothing Then
            DateEdit1.EditValue = CDate(DataMin)
            DateEdit4.EditValue = CDate(DataMax)
            LINDO = True
        End If
        Dim RR As String = ""
        Dim Lancio As String = "select * from VCOMMH8 where ("
        Dim seledata As String = ") and ( " & L1 & " between '" & DateEdit1.EditValue.ToShortDateString & "' and '" & DateEdit4.EditValue.ToShortDateString & "')"
        Dim LancioR As String = "select * from RCOMMH8 where ("
        Dim seledataR As String = ") and " & L1 & " = '" & DateEdit4.EditValue.ToShortDateString & "'"
        Dim Order As String = " order by TcmNum, MCCCogConto, " & L1 & ""
        Dim Str As String = ""
        If CheckEdit8.Checked = True Then
            RR = "-R-"
            Str = Lancio & Formula & seledata & " UNION " & LancioR & Formula & seledataR & Order
        Else
            RR = ""
            Str = Lancio & Formula & seledata & Order
        End If
        DsBilCdc = New DataTable()
        DaBilCdc = New SqlDataAdapter(Str, CnDc)
        DaBilCdc.Fill(DsBilCdc)
        If DsBilCdc.Rows.Count > 0 Then
            Periodo = "Periodo " & DateEdit1.EditValue.ToShortDateString & " - " & DateEdit4.EditValue.ToShortDateString & T2 & RR
            selectformula = Formula
            REPORT = New DxSpartiComm
            REPORT.DataSource = DsBilCdc
            REPORT.DataMember = "DsBilCdc"
            REPORT.FilterString = selectformula
            REPORT.Parameters("Periodo").Value = Periodo
            REPORT.Parameters("Marchio").Value = Marchio()
            REPORT.ShowPreview()
        End If
        If LINDO = True Then
            DateEdit1.EditValue = Nothing : DateEdit4.EditValue = Nothing
        End If
    End Sub
    Sub PreparoFormula()
        Cursor.Current = Cursors.WaitCursor
        Formula = ""
        ForExp = ""
        Dim dd1, dd2 As String
        DataMin = CDate("01/01/2030")
        DataMax = CDate("01/01/2000")
        If CheckEdit1.Checked = True Then
            dd1 = "DALC" : dd2 = "ALC"
        Else
            dd1 = "DAL" : dd2 = "AL"
        End If
        For pp As Int16 = 1 To GridView4.DataRowCount
            Rw = GridView4.GetDataRow(pp - 1)
            If Rw("STAMPA") = True Then
                Formula = Formula & "MCCCOGLDP = " & Rw("TcmRif") & " OR "
                ForExp = ForExp & "[Commessa]= '" & Rw("TcmSigla") & "' OR "
                If CDate(Rw(dd1)) < CDate(DataMin) Then DataMin = Rw(dd1)
                If CDate(Rw(dd2)) > CDate(DataMax) Then DataMax = Rw(dd2)
            End If
        Next
        Formula = Mid(Formula, 1, Len(Formula) - 4)
        ForExp = Mid(ForExp, 1, Len(ForExp) - 4)
    End Sub
    Private Sub DateEdit4_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit4.EditValueChanged
        Dim Mese, Giorno, Anno As String
        Mese = CDate(DateEdit4.EditValue).Month
        Anno = CDate(DateEdit4.EditValue).Year
        Giorno = Date.DaysInMonth(Anno, Mese)
        If CDate(DateEdit4.EditValue).ToShortDateString = CDate(Giorno & "/" & Mese & "/" & Anno).ToShortDateString Then
            CheckEdit8.Enabled = True
        Else
            CheckEdit8.Checked = False : CheckEdit8.Enabled = False
        End If
    End Sub
    Private Sub CheckEdit1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit1.CheckedChanged
        If CheckEdit1.Checked = True Then
            GroupControl1.Text = "MOVIMENTO dei CONTI per COMPETENZA"
            L1 = "PriDataEst"   'Procedura COMPETENZA
            L1x = "[Data Giornale]"
            T2 = "-C"
        Else
            GroupControl1.Text = "MOVIMENTO dei CONTI"
            L1 = "PrkAAmmgg" 'Procedura NO COMPETENZA
            L1x = "[Data Operazione]"
            T2 = ""
        End If
        PictureEdit1.Visible = CheckEdit1.Checked
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged, RadioGroup2.SelectedIndexChanged
        POPOLACOMMESSE()
    End Sub
End Class