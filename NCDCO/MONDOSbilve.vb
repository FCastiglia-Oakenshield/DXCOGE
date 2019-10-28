Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Imports NPRINT
Imports CrystalDecisions.CrystalReports.Engine
Imports NCCOM

Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native


Public Class MONDOSbilve
    Friend WithEvents prntDoc As System.Drawing.Printing.PrintDocument
    Dim prntDial As New PrintDialog

    Dim TbCMG As DataTable
    Dim DaCMG As SqlDataAdapter
    Dim RwCMG As DataRowView
    Dim BLOCK As Integer = -1
    Dim RIF As Integer = -1

    Dim Rpt As New ReportClass
    Dim Rpt2 As New BilSpppCDC
    

    Dim Iset2 As Integer
    Dim frm As New LpLp

    Dim STnull As String = "select TcmRif,TcmSigla,TcmNum,TcmOggetto from VCOMMESSE where TcmRif = 32000"

    Dim sw, TipoStampa As Int16
    Dim IdBlk, P1, P2, P3, P4, P5, P6 As Int32
    Dim OkFlash, OkQuote As Boolean
    Dim Quote As Decimal = 0
    Dim Ammortamenti As String = ""
    
    Dim d1, d2, d5 As String

    REM VARIABILI DIFFERENZA LANCIO BILANCIO NORMALE o DI COMPETENZA
    Dim L1 As String = "RF1CDC" 'Procedura NO COMPETENZA
    Dim T1 As String = "" ' INTESTAZIONE 1 NO COMPETENZA
    Dim T2 As String = "" ' INTESTAZIONE 2 NO COMPETENZA

    REM VARIABILI DA MOVIMENTO CONTI 
    Dim DsCom As DataTable
    Dim DaCom As SqlDataAdapter
    Dim RwCom As DataRow


    Dim REPORT As New XtraReport
    Dim selectformula, SCRI, StrPrint, Formula, Periodo As String

    Dim DsBilCdc As DataTable
    Dim DaBilCdc As SqlDataAdapter

    Dim DataMin As Date
    Dim DataMax As Date


    Private Sub MONDOSbilve_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
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
            GroupControl1.Text = "BILANCIO di VERIFICA per COMPETENZA"
            L1 = "RF1CCDC" 'Procedura COMPETENZA
            T1 = "(Competenza)"
            T2 = "-C"
        Else
            GroupControl1.Text = "BILANCIO di VERIFICA"
            L1 = "RF1CDC" 'Procedura NO COMPETENZA
            T1 = ""
            T2 = ""
        End If
        PictureEdit1.Visible = CheckEdit1.Checked
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged, RadioGroup2.SelectedIndexChanged
        POPOLACOMMESSE()
    End Sub

    Sub Stampa()
        Dim LINDO As Boolean = False
        Dim RR As String

        Cursor.Current = Cursors.WaitCursor


        DataMin = CDate("01/01/2030")
        DataMax = CDate("01/01/2000")
        If CheckEdit1.Checked = True Then
            d1 = "DALC" : d2 = "ALC"
        Else
            d1 = "DAL" : d2 = "AL"
        End If


        For pp As Int16 = 1 To GridView4.DataRowCount
            Rw = GridView4.GetDataRow(pp - 1)
            If Rw("STAMPA") = True Then
                If CDate(Rw(d1)) < CDate(DataMin) Then DataMin = Rw(d1)
                If CDate(Rw(d2)) > CDate(DataMax) Then DataMax = Rw(d2)
            End If
        Next



        If CheckEdit8.Checked = True Then d5 = DateEdit4.EditValue Else d5 = "31/12/2050"



        If DateEdit1.EditValue Is Nothing And DateEdit4.EditValue Is Nothing Then
            DateEdit1.EditValue = CDate(DataMin)
            DateEdit4.EditValue = CDate(DataMax)
            LINDO = True
        End If
       
        
        For pp As Int16 = 1 To GridView4.DataRowCount
            Rw = GridView4.GetDataRow(pp - 1)
            If Rw("STAMPA") = True Then
                RIF = Rw("TcmRif")
                If CDate(Rw(d1)) < CDate(DataMin) Then DataMin = Rw(d1)
                If CDate(Rw(d2)) > CDate(DataMax) Then DataMax = Rw(d2)
                If LetturaMovimenti() > 0 Then
                    Rpt = Nothing
                    Rpt = New ReportClass
                    Rpt2 = New BilSpppCDC
                    Rpt = Rpt2
                    If CheckEdit8.Checked = True Then RR = T2 & "-R" Else RR = T2 & ""
                    Rpt.RecordSelectionFormula = "{CrBilProg.TMSBLOCK} = " & BLOCK
                    Rpt.SetParameterValue("periodo", "(" & CDate(DateEdit1.EditValue).ToShortDateString & " - " & CDate(DateEdit4.EditValue).ToShortDateString & ")" & RR)
                    Rpt.SetParameterValue("Marchio", Marchio)
                    Rpt.SetParameterValue("Dettaglio", False)
                    Rpt.SetParameterValue("Intesta", "Patrimoniale e Conto Economico" & T1.ToUpper)
                    Rpt.SetParameterValue("Ammortamenti", Ammortamenti)
                    Rpt.SetParameterValue("Quote", Quote)
                    Rpt.SetParameterValue("Commessa", Rw("TcmSigla") & " " & Rw("TcmOggetto"))
                    If TipoStampa = 1 Then
                        PdfStart(Rpt, "SPPCDC" & RIF & IdBlk)
                    Else
                        frm = New LpLp
                        frm.reportsource = Rpt
                        frm.Text = "Patrimoniale e Conto Economico" & T1.ToUpper
                        frm.Show()
                    End If

                End If

            End If
            PuliziaFlash()
        Next

        If LINDO = True Then
            DateEdit1.EditValue = Nothing : DateEdit4.EditValue = Nothing
        End If
    End Sub
    Function LetturaMovimenti() As Integer
        Cursor.Current = Cursors.WaitCursor
        If BLOCK < 1 Then BLOCK = semaforo("BILANCIO COMMESSE")
        Dim Lancio As String = "Exec " & L1 : XtraTabControl1.SelectedTabPageIndex = 1

        Dim Str As String = Lancio & " @DAL='" & DateEdit1.EditValue.ToShortDateString & "',@AL='" & DateEdit4.EditValue.ToShortDateString & "',@BLOCK= " & BLOCK & ",@CAUS=45,@SW=0,@NCOMM= " & RIF & ", @FAL ='" & d5 & "'"
        EsegueSql(Str, CnDc)
        Cmd = New SqlCommand("select COUNT(*) from VDCDBILPROG where TMSBLOCK = " & BLOCK, CnDc)
        LetturaMovimenti = Cmd.ExecuteScalar()
    End Function

    Private Sub MONDOSbilve_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        PuliziaFlash()
    End Sub

    Sub PuliziaFlash()
        Dim Ultimo As New SqlCommand("DELETE FROM TMPBILC WHERE TMcBLOCK = " & BLOCK, CnDc)
        Ultimo.ExecuteNonQuery()
        Ultimo = New SqlCommand("DELETE FROM TMPBILS WHERE TMSBLOCK = " & BLOCK, CnDc)
        Ultimo.ExecuteNonQuery()
    End Sub



End Class