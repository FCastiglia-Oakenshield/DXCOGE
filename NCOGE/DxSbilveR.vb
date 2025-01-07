Imports CrystalDecisions.CrystalReports.Engine
Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient

Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native


Public Class DxSbilveR
    Dim sw, MaxEse, EseFormato(5), TipoStampa, EseBilChi(5), CauChiusura, EseAnno(5) As Int16
    Dim EseSaDa(5), EseSaA(5), EseSpDa(5), EseSpA(5), EseCeDa(5), EseCeA(5) As Int32
    Dim IdBlk, P1, P2, P3, P4, P5, P6 As Int32
    Dim OkFlash, OkQuote As Boolean
    Dim Quote As Decimal = 0
    Dim Ammortamenti As String = ""
    Dim EseDal(5), EseAl(5), UltimaApertura As Date
    Dim EseProg(5), EseSppp(5), EseClFo(5), EseSdo(5), EseQuote(5) As Boolean
    Dim Rpt As New ReportClass
    Dim Rpt1 As New BilProg
    Dim Rpt2 As New BilSppp
    Dim Rpt3 As New BilClFo
    Dim Rpt4 As New BilCee
    Dim Rpt5 As New BilGis
    Dim Rpt6 As New BilSem

    Dim frm As New LpLp
    Dim PathTmp, PathSto, PathPrg, UserId As String

    REM GRUPPO MONDO
    Dim OkMondo As Boolean = False
    Dim OkGruppoMondo As Boolean = False
    Dim GlGroupAccount As String = ""
    Dim DaGro As SqlDataAdapter
    Dim DsGro As DataTable
    Dim RwGro As DataRow




    REM VARIABILI DIFFERENZA LANCIO BILANCIO NORMALE o DI COMPETENZA
    Dim L1 As String = "RF1" 'Procedura NO COMPETENZA
    Dim T1 As String = "" ' INTESTAZIONE 1 NO COMPETENZA
    Dim T2 As String = "" ' INTESTAZIONE 2 NO COMPETENZA
    REM NUOVO REPORT PER GRUPPO MONDO
    Dim REPORT As New XtraReport
    Dim selectformula As String
    Dim Cfis, Piva, Indir, Citta, Prov As String


    Private Sub DxSbilveR_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Me.Tag = "@C" Then
            GroupControl1.Text = "BILANCIO di VERIFICA per COMPETENZA"
            L1 = "RF1C"
            T1 = "(Competenza)"
            T2 = "-C"
            PictureEdit1.Visible = True
        End If

        If sw = 0 Then
            LeggiParametri()
            DateEdit1.EditValue = Today
            Apertura()
            sw = 1
        End If
        ControlloConsolidato()
    End Sub
    Sub ControlloConsolidato()
        Dim Source As String = cnDb.DataSource
        Dim WHY As String = ""
        WHY = Source.Split("\")(0)
        If WHY = Source Then GoTo Oltre
        WHY = Source.Split("\")(1)
Oltre:
        If WHY = "COMUNITA" Or WHY = "RSCOMUNITA" Then CheckEdit11.Visible = True : CheckEdit11.Checked = False Else CheckEdit11.Visible = False : CheckEdit11.Checked = False
    End Sub
    Sub Apertura()
        Dim Cmd As New SqlCommand("SELECT isnull(max(pridataest),'01/01/2000') FROM Tbpri Where PriCausale = 45", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UltimaApertura = dataRd.Item(0)
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT top 5 * from TbEse Order by EseAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        Dim x, y As Int16
        MaxEse = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MaxEse = MaxEse + 1
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("EseAnno"))
            EseAnno(MaxEse) = dataRd.Item("EseAnno")
            EseDal(MaxEse) = dataRd.Item("EseDal")
            EseAl(MaxEse) = dataRd.Item("EseAl")
            EseProg(MaxEse) = dataRd.Item("EseProg")
            EseSppp(MaxEse) = dataRd.Item("EseSppp")
            EseClFo(MaxEse) = dataRd.Item("EseClFo")
            EseSdo(MaxEse) = dataRd.Item("EseSdo")
            EseQuote(MaxEse) = dataRd.Item("EseQuote")
            EseFormato(MaxEse) = dataRd.Item("EseFormato")
            EseBilChi(MaxEse) = dataRd.Item("EseCausaleChiusuraConti")
            EseSaDa(MaxEse) = dataRd.Item("EseSaDa")
            EseSaA(MaxEse) = dataRd.Item("EseSaA")
            EseSpDa(MaxEse) = dataRd.Item("EseSpDa")
            EseSpA(MaxEse) = dataRd.Item("EseSpA")
            EseCeDa(MaxEse) = dataRd.Item("EseCeDa")
            EseCeA(MaxEse) = dataRd.Item("EseCeA")
        End While
        dataRd.Close()
        y = MaxEse
        For x = 0 To y
            Cmd = New SqlCommand("SELECT TOP 1 PRIDATAEST FROM Tbpri Where PriCausale = " & EseBilChi(x) & " and DATEPART(YEAR,PRIDATAEST)= " & EseAnno(x), cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                MaxEse = MaxEse - 1
                ComboBoxEdit1.Properties.Items.Remove(EseAnno(x))
            End While
            dataRd.Close()
        Next
        For x = 0 To MaxEse
            If ComboBoxEdit1.Properties.Items(x) = DateEdit1.EditValue.Year Then
                ComboBoxEdit1.SelectedIndex = x
                Esercizi(x)
                Exit For
            End If
        Next
        CheckEdit4.Enabled = CheckEdit3.Checked
        GroupControl8.Visible = OkGruppoMondo : CheckEdit9.Checked = False : GroupControl9.Enabled = False
    End Sub
    Private Sub SbilveR_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        PuliziaFlash()
    End Sub
    Sub PuliziaFlash()
        'Dim Ultimo As New SqlCommand("DELETE FROM TMPBILC WHERE TMCBLOCK = " & IdBlk, cnCo)
        'Ultimo.ExecuteNonQuery()
        'Ultimo = New SqlCommand("DELETE FROM TMPBILS WHERE TMSBLOCK = " & IdBlk, cnCo)
        'Ultimo.ExecuteNonQuery()

        'If CheckEdit11.Checked = True AndAlso CnCoSede IsNot Nothing AndAlso CnCoSede.State = ConnectionState.Open Then
        '    Ultimo = New SqlCommand("DELETE FROM TMPBILC WHERE TMCBLOCK = " & IdBlk, CnCoSede)
        '    Ultimo.ExecuteNonQuery()
        '    Ultimo = New SqlCommand("DELETE FROM TMPBILS WHERE TMSBLOCK = " & IdBlk, CnCoSede)
        '    Ultimo.ExecuteNonQuery()
        '    CnCoSede.Close()
        'End If

        Dim Ultimo As New SqlCommand("DELETE FROM TMPBILC", cnCo)
        Ultimo.ExecuteNonQuery()
        Ultimo = New SqlCommand("DELETE FROM TMPBILS", cnCo)
        Ultimo.ExecuteNonQuery()

        If CheckEdit11.Checked = True AndAlso CnCoSede IsNot Nothing AndAlso CnCoSede.State = ConnectionState.Open Then
            Ultimo = New SqlCommand("DELETE FROM TMPBILC", CnCoSede)
            Ultimo.ExecuteNonQuery()
            Ultimo = New SqlCommand("DELETE FROM TMPBILS", CnCoSede)
            Ultimo.ExecuteNonQuery()
            ''CnCoSede.Close()
        End If
    End Sub
    Sub AggiornaCheck(ByVal Qa As Int16)
        Dim Str As String
        Str = "Update TbEse set EseProg = " & Math.Abs(CInt(CheckEdit1.Checked)) _
        & " , EseSppp = " & Math.Abs(CInt(CheckEdit2.Checked)) & " , EseClFo = " & Math.Abs(CInt(CheckEdit3.Checked)) _
        & " , EseSdo = " & Math.Abs(CInt(CheckEdit4.Checked)) & " , EseQuote = " & Math.Abs(CInt(CheckEdit5.Checked)) _
        & " where EseAnno = " & Qa
        Dim Check As New SqlCommand(Str, cnCo)
        Check.ExecuteNonQuery()
    End Sub
    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim x As Int16
        Dim Titolo As String = "Patrimoniale e Conto Economico"
        Cursor.Current = Cursors.WaitCursor
        CaricaSaldi()
        If CheckEdit11.Checked = True Then Titolo &= " CONSOLIDATO "
        For x = 3 To 1 Step -1
            If x = 1 And CheckEdit1.Checked = True Then
                SparaStampa(Rpt1, "Progressivo Sottoconti" & T1, "{CrBilProg.TMSBLOCK} = ", False, False, "BilVer")
            End If
            If x = 2 And CheckEdit2.Checked = True Then
                SparaStampa(Rpt2, Titolo & T1, "{CrBilProg.TMSBLOCK} = ", False, True, "SitPpp")
            End If
            If x = 3 And CheckEdit3.Checked = True Then
                SparaStampa(Rpt3, "Progressivo Clienti e Fornitori" & T1, "{CrBilClFo.TMCBLOCK} = ", CheckEdit4.Checked, False, "ProgCf")
            End If
        Next
        If CheckEdit10.Checked = True Then SezioniContrapposte(Titolo & T1, False, True, "Sezioni")
        If CheckEdit6.Checked = True Then EsegueSole24Ore()
        If CheckEdit7.Checked = True And UserId = "SELCO" Then
            SparaStampa(Rpt6, "Conversione Saldi Piano dei Conti SEMPLIFICATA", "{CRSEMPLIFICATA.RIGO} > 0 AND {CRSEMPLIFICATA.TMSBLOCK} = ", False, False, "Saldi")
            'ElseIf CheckEdit7.Checked = True And UserId = "SELCO" And Val(ComboBoxEdit1.EditValue) <> 2014 Then
            '    SparaStampa(Rpt5, "Conversione Saldi Piano dei Conti", "{CRSOLE24OREG.TMSBLOCK} = ", False, False, "Saldi")
        ElseIf CheckEdit7.Checked = True Then
            SparaStampa(Rpt4, "Piano dei Conti C.E.E.", "{CRSOLE24ORE.TMSBLOCK} = ", False, False, "Cee")
        End If
        If CheckEdit9.Checked = True Then ReportingGroup()
        If CheckEdit12.Checked = True Then BilancioCee()
        Me.Close()
    End Sub
    Sub SezioniContrapposte(ByVal Txt As String, ByVal Dettaglio As Boolean, ByVal QAmm As Boolean, ByVal TextPdf As String)
        Cursor.Current = Cursors.WaitCursor
        Dim RR As String
        Dim Azienda As String = Marchio()
        If CheckEdit11.Checked = True Then Azienda &= " CONSOLIDATO"
        If CheckEdit8.Checked = True Then RR = T2 & "-R" Else RR = T2 & ""
        EsegueSql("EXEC XBILSEZ @BLOCK=" & IdBlk, cnCo)
        Dim StrPrint As String = "select * from TMPSEZ ORDER BY TMPSID"
        Dim DsSez = New DataTable
        Dim DaSez = New SqlDataAdapter(StrPrint, cnCo)
        DaSez.SelectCommand.CommandTimeout = 300
        DaSez.Fill(DsSez)
        REPORT = New XBilSez
        REPORT.DataSource = DsSez
        REPORT.DataMember = "DsSez"
        REPORT.Parameters("Periodo").Value = CDate(DateEdit1.EditValue).ToShortDateString & RR
        REPORT.Parameters("Azienda").Value = Azienda
        REPORT.ShowPreview()
    End Sub
    Sub ReportingGroup()
        Dim Sstr As String = "SELECT * FROM VREPGROUP WHERE TMSBLOCK = " & IdBlk & " order by GLGroupAccount,TMSFLAG"
        Dim DaGro = New SqlDataAdapter(Sstr, cnCo)
        Dim DsGro = New DataTable
        DaGro.Fill(DsGro)

        Dim Files(1) As String
        Dim Eserc As String = CDate(DateEdit1.EditValue).Year.ToString
        Dim Perio As String = CDate(DateEdit1.EditValue).Month.ToString.PadLeft(2, "0")
        Dim str As String = ""
        Files(0) = Trim(TextEdit2.EditValue.ToString) & "SP" & Eserc & Perio & ".txt"
        Files(1) = Trim(TextEdit2.EditValue.ToString) & "CE" & Eserc & Perio & ".txt"
        If DsGro.Rows.Count = 0 Then Exit Sub

        If File.Exists(Files(0)) Then File.Delete(Files(0))
        Dim OutSPA As TextWriter = File.AppendText(Files(0))
        OutSPA.WriteLine("SCENARIO;ESERCIZIO;PERIODO;ID_SOCIETA;DIV;COD_CONTO;VALORE;VALORE_LC")
        If File.Exists(Files(1)) Then File.Delete(Files(1))
        Dim OutCEC As TextWriter = File.AppendText(Files(1))
        OutCEC.WriteLine("SCENARIO;ESERCIZIO;PERIODO;ID_SOCIETA;DIV;COD_CONTO;VALORE")
        For i As Int16 = 1 To DsGro.Rows.Count
            RwGro = DsGro.Rows(i - 1)
            If RwGro("GLGroupAccount") Is DBNull.Value Then GoTo II
            str = "ACT;" & Eserc & ";" & Perio & ";" & "08;3;" & RwGro("GLGroupAccount").ToString & ";" & Format(RwGro("SALDO"), "#########0.00").ToString
            If IsNumeric(Mid(RwGro("GLGroupAccount").ToString, 1, 1)) Then GoTo SPA
            OutCEC.WriteLine(str)
            GoTo II
SPA:
            str &= ";" & Format(RwGro("SALDO"), "#########0.00").ToString
            OutSPA.WriteLine(str)
II:
        Next
        OutCEC.Close()
        OutSPA.Close()


        REPORT = New DxGrMondo
        REPORT.DataSource = DsGro
        REPORT.DataMember = "DsGro"
        REPORT.Parameters.Item("Marchio").Value = Marchio()
        REPORT.Parameters.Item("Titolo").Value = "REPORTING di GRUPPO al " & DateEdit1.EditValue & " ( SP" & Eserc & Perio & ".txt - " & "CE" & Eserc & Perio & ".txt)"

        REPORT.ShowPreview()
    End Sub
    Private Sub EsegueSole24Ore()
        If Trim(TextEdit1.EditValue) = "" Then Exit Sub
        Sole24Ore(Trim(TextEdit1.EditValue), IdBlk)
    End Sub
    Sub SparaStampa(ByVal RRpt As ReportClass, ByVal Txt As String, ByVal Formula As String, ByVal Dettaglio As Boolean, ByVal QAmm As Boolean, ByVal TextPdf As String)

        ' StampaCr()         'Lancia  CRISTALREPORT    Chiedere come metterli,con parantesi vuote danno errori
        'StampaDevexp()    'Lancia DevExpress

        Cursor.Current = Cursors.WaitCursor
        Rpt = New ReportClass
        frm = New LpLp
        Dim Selectformula, RR As String
        Dim Azienda As String = Marchio()
        If CheckEdit11.Checked = True Then Azienda &= " CONSOLIDATO"
        Rpt = RRpt
        Selectformula = Formula & IdBlk
        If CheckEdit8.Checked = True Then RR = T2 & "-R" Else RR = T2 & ""
        Rpt.RecordSelectionFormula = Selectformula
        Rpt.SetParameterValue("periodo", CDate(DateEdit1.EditValue).ToShortDateString & RR)
        Rpt.SetParameterValue("Marchio", Azienda)
        Rpt.SetParameterValue("Dettaglio", Dettaglio)
        Rpt.SetParameterValue("Intesta", Txt.ToUpper)

        If QAmm = True Then
            Rpt.SetParameterValue("Ammortamenti", Ammortamenti)
            Rpt.SetParameterValue("Quote", Quote)
        End If
        If CheckEdit7.Checked = True And Rpt Is Rpt4 Then
            Rpt.SetParameterValue("P1", P1)
            Rpt.SetParameterValue("P2", P2)
            Rpt.SetParameterValue("P3", P3)
            Rpt.SetParameterValue("P4", P4)
            Rpt.SetParameterValue("P5", P5)
            Rpt.SetParameterValue("P6", P6)
        End If

        If TipoStampa = 1 Then
            PdfStart(Rpt, TextPdf & IdBlk)
            Exit Sub
        End If


        frm.reportsource = Rpt
        frm.Text = Txt
        frm.Show()
    End Sub


    Sub StampaCr(ByVal RRpt As ReportClass, ByVal Txt As String, ByVal Formula As String, ByVal Dettaglio As Boolean, ByVal QAmm As Boolean, ByVal TextPdf As String)

        Cursor.Current = Cursors.WaitCursor
        Rpt = New ReportClass
        frm = New LpLp
        Dim Selectformula, RR As String
        Dim Azienda As String = Marchio()
        If CheckEdit11.Checked = True Then Azienda &= " CONSOLIDATO"
        Rpt = RRpt
        Selectformula = Formula & IdBlk
        If CheckEdit8.Checked = True Then RR = T2 & "-R" Else RR = T2 & ""
        Rpt.RecordSelectionFormula = Selectformula
        Rpt.SetParameterValue("periodo", CDate(DateEdit1.EditValue).ToShortDateString & RR)
        Rpt.SetParameterValue("Marchio", Azienda)
        Rpt.SetParameterValue("Dettaglio", Dettaglio)
        Rpt.SetParameterValue("Intesta", Txt.ToUpper)

        If QAmm = True Then
            Rpt.SetParameterValue("Ammortamenti", Ammortamenti)
            Rpt.SetParameterValue("Quote", Quote)
        End If
        If CheckEdit7.Checked = True And Rpt Is Rpt4 Then
            Rpt.SetParameterValue("P1", P1)
            Rpt.SetParameterValue("P2", P2)
            Rpt.SetParameterValue("P3", P3)
            Rpt.SetParameterValue("P4", P4)
            Rpt.SetParameterValue("P5", P5)
            Rpt.SetParameterValue("P6", P6)
        End If

        If TipoStampa = 1 Then
            PdfStart(Rpt, TextPdf & IdBlk)
            Exit Sub
        End If


        frm.reportsource = Rpt
        frm.Text = Txt
        frm.Show()




    End Sub


    Sub StampaDevexp(ByVal RRpt As ReportClass, ByVal Txt As String, ByVal Formula As String, ByVal Dettaglio As Boolean, ByVal QAmm As Boolean, ByVal TextPdf As String)
        'Modificare.
        Cursor.Current = Cursors.WaitCursor
        Rpt = New ReportClass
        frm = New LpLp
        Dim Selectformula, RR As String
        Dim Azienda As String = Marchio()
        If CheckEdit11.Checked = True Then Azienda &= " CONSOLIDATO"
        Rpt = RRpt
        Selectformula = Formula & IdBlk
        If CheckEdit8.Checked = True Then RR = T2 & "-R" Else RR = T2 & ""
        Rpt.RecordSelectionFormula = Selectformula
        Rpt.SetParameterValue("periodo", CDate(DateEdit1.EditValue).ToShortDateString & RR)
        Rpt.SetParameterValue("Marchio", Azienda)
        Rpt.SetParameterValue("Dettaglio", Dettaglio)
        Rpt.SetParameterValue("Intesta", Txt.ToUpper)

        If QAmm = True Then
            Rpt.SetParameterValue("Ammortamenti", Ammortamenti)
            Rpt.SetParameterValue("Quote", Quote)
        End If
        If CheckEdit7.Checked = True And Rpt Is Rpt4 Then
            Rpt.SetParameterValue("P1", P1)
            Rpt.SetParameterValue("P2", P2)
            Rpt.SetParameterValue("P3", P3)
            Rpt.SetParameterValue("P4", P4)
            Rpt.SetParameterValue("P5", P5)
            Rpt.SetParameterValue("P6", P6)
        End If

        If TipoStampa = 1 Then
            PdfStart(Rpt, TextPdf & IdBlk)
            Exit Sub
        End If


        frm.reportsource = Rpt
        frm.Text = Txt
        frm.Show()
    End Sub














    Sub CaricaSaldi()
        PuliziaFlash()
        If OkFlash = False Then
            OkFlash = True
            If CheckEdit11.Checked = True Then IdBlk = 1 Else IdBlk = semaforo("Bilancio AL " & Today.Date)
        End If

        Dim d1, d2, D3, D4, D5 As String
        Dim Caus, x As Int16
        Caus = 0
        Quote = 0
        Ammortamenti = ""
        d1 = CDate(DateEdit2.EditValue).ToShortDateString
        d2 = DateEdit1.EditValue
        If CheckEdit8.Checked = True Then D5 = d2 Else D5 = "31/12/2050"
        If UltimaApertura < DateEdit2.EditValue And UltimaApertura > "01/01/2000" Then
            For x = 0 To MaxEse
                If EseDal(x) = UltimaApertura Then
                    D3 = EseDal(x)
                    D4 = EseAl(x)
                    EsegueProcedura("XF1P", D3, D4, IdBlk, Caus, 0, 0)
                    Exit For
                End If
            Next
        Else
            Caus = 45
        End If
        EsegueProceduraR(L1, d1, d2, IdBlk, Caus, 0, 0, D5)
        OkQuote = True
        Dim Cmd As New SqlCommand("SELECT TOP 1 PRIDATAEST FROM Tbpri Where PriCausale = " & CauChiusura & " and DATEPART(YEAR,PRIDATAEST)= " & CDate(d2).Year, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            OkQuote = False
        End While
        dataRd.Close()
        If CheckEdit5.Checked = True And CheckEdit5.Enabled = True And OkQuote = True Then
            ConteggioQuote(d1, d2)
        End If
        AggiornaCheck(Val(ComboBoxEdit1.EditValue))
    End Sub
    Sub ConteggioQuote(ByVal d1 As String, ByVal d2 As String)
        'Dim hh As String = "31/12/" & CDate(d2).Year
        '''' ATTENZIONE I MESI NON SERVONO PIU' VIENE PASSATA LA DATA EFFETTIVA PER IL CALCOLO
        Dim NMesi As Integer = 0

        EsegueSql("EXEC XCESP @DATA = '" & d2 & "',@idblk = 0", cnCo)
        Cmd = New SqlCommand("SELECT isnull(sum(PROQUOAMMORTAM),0) FROM TMPPROQUO where PROCONTOQUO = '00.00'", cnCo)
        Quote = 0
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            'Quote = Format(dataRd.Item(0) / 12 * Val(Mid(d2, 4, 2)), "#########0.00")
            Quote = Format(dataRd.Item(0), "#########0.00")
        End While
        dataRd.Close()
        NMesi = DateDiff(DateInterval.Month, CDate(d1), CDate(d2)) + 1
        Ammortamenti = "QUOTE AMMORTAMENTO NON CLASSIFICATE N. " & NMesi & " MESI"
        EsegueSql(" EXEC XDETTQUO @BLOCK = " & IdBlk & ",@MESI = ' N. " & NMesi & " MESI'", cnCo)
    End Sub

    Function EsegueProcedura(ByVal QualeP As String, ByVal Dal As String, ByVal Al As String, ByVal IdBlk As Int32, ByVal Caus As Int16, ByVal Switch As Int16, ByVal CauChi As Int16) As Boolean
        EsegueSql(" EXEC " & QualeP & "  @DAL ='" & Dal & "', @AL = '" & Al & "' , @BLOCK = " & IdBlk & " , @CAUS= " & Caus & " , @SW= " & Switch & ", @CAUCH= " & CauChi, cnCo)
        If CheckEdit11.Checked = True Then
            EsegueSql(" EXEC " & QualeP & "  @DAL ='" & Dal & "', @AL = '" & Al & "' , @BLOCK = " & IdBlk & " , @CAUS= " & Caus & " , @SW= " & Switch & ", @CAUCH= " & CauChi, CnCoSede)
            ''  EsegueSql(" EXEC XCONSOLIDA", cnCo)
        End If
    End Function
    Function EsegueProceduraR(ByVal QualeP As String, ByVal Dal As String, ByVal Al As String, ByVal IdBlk As Int32, ByVal Caus As Int16, ByVal Switch As Int16, ByVal CauChi As Int16, ByVal Fal As String) As Boolean
        EsegueSql(" EXEC " & QualeP & "  @DAL ='" & Dal & "', @AL = '" & Al & "' , @BLOCK = " & IdBlk & " , @CAUS= " & Caus & " , @SW= " & Switch & ", @CAUCH= " & CauChi & ", @FAL ='" & Fal & "'", cnCo)
        If CheckEdit11.Checked = True Then
            EsegueSql(" EXEC " & QualeP & "  @DAL ='" & Dal & "', @AL = '" & Al & "' , @BLOCK = " & IdBlk & " , @CAUS= " & Caus & " , @SW= " & Switch & ", @CAUCH= " & CauChi & ", @FAL ='" & Fal & "'", CnCoSede)
            EsegueSql(" EXEC XCONSOLIDA", cnCo)
        End If
    End Function

    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        Dim x As Int16
        x = ComboBoxEdit1.SelectedIndex
        If x = -1 Or sw = 0 Then Exit Sub
        Esercizi(x)
    End Sub
    Sub Esercizi(ByVal x As Int16)
        DateEdit2.Properties.MaxValue = "31/12/2050" ' reset campi per ricalcolare limiti
        DateEdit2.Properties.MinValue = "01/01/1900"
        DateEdit3.Properties.MaxValue = "31/12/2050"
        DateEdit3.Properties.MinValue = "01/01/1900"
        DateEdit2.Properties.MaxValue = EseDal(x).AddMinutes(1)
        DateEdit2.Properties.MinValue = EseDal(x)
        DateEdit2.EditValue = EseDal(x)
        DateEdit3.Properties.MaxValue = EseAl(x).AddMinutes(1)
        DateEdit3.Properties.MinValue = EseAl(x)
        DateEdit3.EditValue = EseAl(x)
        DateEdit1.Properties.MaxValue = "31/12/2050"
        DateEdit1.Properties.MinValue = "01/01/1900"
        DateEdit1.EditValue = DateEdit3.EditValue
        DateEdit1.Properties.MaxValue = DateEdit3.Properties.MaxValue
        DateEdit1.Properties.MinValue = DateEdit2.Properties.MinValue
        If DateEdit1.EditValue > Today Then DateEdit1.EditValue = Today
        CheckEdit1.Checked = EseProg(x)
        CheckEdit2.Checked = EseSppp(x)
        CheckEdit3.Checked = EseClFo(x)
        CheckEdit4.Checked = EseSdo(x)
        CheckEdit5.Checked = EseQuote(x)
        TipoStampa = EseFormato(x)
        CauChiusura = EseBilChi(x)
        CheckEdit10.Checked = Not CheckEdit2.Checked
        DateEdit1.Focus()
        P1 = EseSaDa(x)
        P2 = EseSaA(x)
        P3 = EseSpDa(x)
        P4 = EseSpA(x)
        P5 = EseCeDa(x)
        P6 = EseCeA(x)
    End Sub
    Private Sub CheckEdit11_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit11.CheckedChanged
        CheckEdit1.Checked = False : CheckEdit3.Checked = False : CheckEdit5.Checked = False : CheckEdit4.Checked = False : CheckEdit5.Checked = False
        CheckEdit1.Enabled = Not CheckEdit11.Checked : CheckEdit3.Enabled = Not CheckEdit11.Checked : CheckEdit4.Enabled = Not CheckEdit11.Checked : CheckEdit5.Enabled = Not CheckEdit11.Checked
        If CheckEdit11.Checked = True Then ConnettiRosine()
    End Sub
    Sub ConnettiRosine()
        Dim NConn As String = ""
        Cmd = New SqlCommand("Select Sel1 from Tbsel where SelId = 2", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            NConn = dataRd.Item("Sel1")
        End While
        dataRd.Close()
        CnCoSede = New SqlConnection(NConn)
        Try
            CnCoSede.Open()
        Catch ex As Exception
            MessageBox.Show("Impossibile connettersi con il server", "CONNESSIONE AL PENSIONATO", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CheckEdit11.Checked = False
        End Try
    End Sub
    Private Sub CheckEdit3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit3.CheckedChanged
        CheckEdit4.Enabled = CheckEdit3.Checked
    End Sub

    Private Sub CheckEdit210_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged, CheckEdit10.CheckedChanged
        If (CheckEdit2.Checked = True Or CheckEdit10.Checked = True) And CheckEdit11.Checked = False Then CheckEdit5.Enabled = True Else CheckEdit5.Enabled = False
    End Sub
    Private Sub CheckEdit2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        If CheckEdit2.Checked = True Then CheckEdit10.Checked = False
    End Sub
    Private Sub CheckEdit10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit10.CheckedChanged
        If CheckEdit10.Checked = True Then CheckEdit2.Checked = False
    End Sub
    Private Function LeggiParametri() As Boolean
        Dim cmd As New SqlCommand(" SELECT * from TbSel where selId = 1", cnVd)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            PathPrg = dataRd.GetString(12) & dataRd.GetString(1)
            PathSto = dataRd.GetString(12) & dataRd.GetString(11)
            PathTmp = dataRd.GetString(8)
        End While
        dataRd.Close()
        cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            UserId = dataRd.Item("Sel14")
        End While
        dataRd.Close()
        Dim Str As String = "Select top 1 * from tbazi inner join vdox.dbo.tbana on anacod = azicod order by AziAnnoLavoro desc"
        cmd = New SqlCommand(Str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read = True Then
            Cfis = dataRd.Item("AnaCfis")
            Piva = dataRd.Item("AnaPiva")
            Indir = dataRd.Item("AnaIndirizzo")
            Citta = dataRd.Item("AnaCitta")
            Prov = dataRd.Item("AnaProv")
        End If
        dataRd.Close()
        OkMondo = False
        If UserId.ToUpper = "MONDOMARINE" Then
            OkMondo = True
            cmd = New SqlCommand("SELECT count(*) FROM TbGroup", cnCo)
            If cmd.ExecuteScalar > 0 Then OkGruppoMondo = True : TextEdit2.EditValue = PathTmp : VerifyDir()
        End If
        Try
            cmd = New SqlCommand("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TbBILCEE]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) BEGIN Select 1 END", cnCo)
            If cmd.ExecuteScalar > 0 Then CheckEdit12.Visible = True Else CheckEdit12.Visible = False
        Catch ex As Exception
            CheckEdit12.Visible = False
        End Try
    End Function
    Private Sub VerifyDir()
        If Directory.Exists(PathTmp) = False Then Directory.CreateDirectory(PathTmp)
    End Sub

    Private Sub CheckEdit6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit6.CheckedChanged
        Dim Periodo As String
        Periodo = Mid(CDate(DateEdit1.EditValue).ToShortDateString, 7, 4) & Mid(CDate(DateEdit1.EditValue).ToShortDateString, 4, 2) & Mid(CDate(DateEdit1.EditValue).ToShortDateString, 1, 2)
        If CheckEdit6.Checked = True Then
            TextEdit1.Enabled = True
            VerifyDir()
            TextEdit1.EditValue = PathTmp & "SOLE" & Periodo
        Else
            TextEdit1.Enabled = False
            CheckEdit7.Checked = False
        End If
    End Sub
    Private Sub CheckEdit7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit7.CheckedChanged
        If CheckEdit6.Checked = False Then
            CheckEdit7.Checked = False
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.EditValueChanged
        Dim Mese, Giorno, Anno As String
        Mese = CDate(DateEdit1.EditValue).Month
        Anno = CDate(DateEdit1.EditValue).Year
        Giorno = Date.DaysInMonth(Anno, Mese)
        If CDate(DateEdit1.EditValue).ToShortDateString = CDate(Giorno & "/" & Mese & "/" & Anno).ToShortDateString Then
            CheckEdit8.Enabled = True
        Else
            CheckEdit8.Checked = False : CheckEdit8.Enabled = False
        End If
    End Sub
#Region "GRUPPO MONDO"
    Private Sub CheckEdit9_CheckedChanged(sender As Object, e As System.EventArgs) Handles CheckEdit9.CheckedChanged
        GroupControl9.Enabled = CheckEdit9.Checked
    End Sub

#End Region
#Region "BILANCIO CEE"
    Sub BilancioCee()
        Cursor.Current = Cursors.WaitCursor
        Dim ContiNc As Int16 = 0
        Dim TbNcl = New DataTable
        Dim DaNcl = New SqlDataAdapter("select * from crsole24ore where rigo = 0 and TMSBLOCK =" & IdBlk & " order by PiaCodCo", cnCo)
        DaNcl.SelectCommand.CommandTimeout = 300
        DaNcl.Fill(TbNcl)
        ContiNc = TbNcl.Rows.Count
        Dim Azienda As String = Marchio()
        EsegueSql("EXEC XBILCEE @BLOCK=" & IdBlk, cnCo)
        Dim StrPrint As String = "select * from TbBilCee ORDER BY BILCEECOD"
        Dim DsSez = New DataTable
        Dim DaSez = New SqlDataAdapter(StrPrint, cnCo)
        DaSez.SelectCommand.CommandTimeout = 300
        DaSez.Fill(DsSez)
        REPORT = New DxBilCee
        REPORT.DataSource = DsSez
        REPORT.DataMember = "DsSez"
        REPORT.Parameters("Periodo1").Value = "Esercizio Al " & CDate(DateEdit1.EditValue).ToShortDateString
        REPORT.Parameters("Periodo2").Value = ""
        REPORT.Parameters("Marchio").Value = Azienda
        REPORT.Parameters("Indirizzo").Value = Indir & " " & Citta & " " & Prov
        REPORT.Parameters("CFPI").Value = "Codice Fiscale e Partita Iva " & Cfis
        REPORT.Parameters("DettaglioCee").Value = False
        REPORT.Parameters("OneYear").Value = True
        If ContiNc > 0 Then
            REPORT.Watermark.Text = "NON VALIDO ci sono CONTI da CLASSIFICARE"
        Else
            REPORT.Watermark.Text = ""
        End If
        REPORT.Parameters("Nc").Value = ContiNc
        REPORT.CreateDocument()

        Dim NoteNcl As XtraReport
        NoteNcl = New DxNoClass
        If ContiNc > 0 Then
            NoteNcl = New DxNoClass
            NoteNcl.DataSource = TbNcl
            NoteNcl.DataMember = "TbNcl"
            NoteNcl.Parameters("Marchio").Value = Azienda
            NoteNcl.CreateDocument()
            For i As Integer = 0 To NoteNcl.Pages.Count - 1
                REPORT.Pages.Add(NoteNcl.Pages(i))
            Next i
            REPORT.PrintingSystem.ContinuousPageNumbering = True
        End If
        REPORT.ShowPreview()
    End Sub
#End Region

End Class