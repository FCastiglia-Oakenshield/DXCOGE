Imports CrystalDecisions.CrystalReports.Engine
Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI

Public Class DxBilAnn

    Dim UltimaApertura, sw, MaxEse, EseFormato(10), TipoStampa, EseBilChi(10), EseAnno(10), CauChiusura, s As Int16
    Dim IdBlk, IdBlk2 As Int32
    Dim OkFlash As Boolean
    Dim EseDal(10), EseAl(10) As Date
    Dim EseOk(10) As Boolean
    Dim frm As New LpLp
    Dim PathTmp, PathSto, PathPrg As String
    Dim Rpt As New ReportClass
    Dim Rpt2 As New BilSppp
    Dim Rpt3 As New BilClFo
    Dim REPORT As New XtraReport
    Dim Cfis, Piva, Indir, Citta, Prov As String
    Dim d1, d2, D3, D4 As String
    Dim DateEdit4 As Date = Today
    Dim DateEdit5 As Date = Today
    Dim q As Int16 = 0
    Private Sub DxBilAnn_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If sw = 0 Then
            TextEdit1.Enabled = False
            Apertura()
            LeggiParametri()
            sw = 1
        End If
    End Sub
    Sub Apertura()
        s = 0
        Cmd = New SqlCommand("SELECT top 10 * from TbEse Order by EseAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        Dim x, y As Int16
        MaxEse = -1
        y = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            y = y + 1
            EseAnno(y) = dataRd.Item("EseAnno")
            EseDal(y) = dataRd.Item("EseDal")
            EseAl(y) = dataRd.Item("EseAl")
            EseFormato(y) = dataRd.Item("EseFormato")
            EseBilChi(y) = dataRd.Item("EseCausaleChiusuraConti")
            EseOk(y) = False
        End While
        dataRd.Close()
        For x = 0 To y
            Dim Cmd As New SqlCommand("SELECT TOP 1 PRIDATAEST FROM Tbpri Where PriCausale = " & EseBilChi(x) & " and DATEPART(YEAR,PRIDATAEST)= " & EseAnno(x), cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                MaxEse = MaxEse + 1
                EseOk(MaxEse) = True
                ComboBoxEdit1.Properties.Items.Add(EseAnno(x))
                If MaxEse = 0 Then Esercizi(x)
            End While
            dataRd.Close()
        Next
        s = y - MaxEse
    End Sub
    Private Sub Sbilve_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If OkFlash = True Then PuliziaFlash()
    End Sub
    Sub PuliziaFlash()
        If OkFlash = False Then Exit Sub
        Dim Ultimo As New SqlCommand("DELETE FROM TMPBILC WHERE TMCBLOCK = " & IdBlk, cnCo)
        Ultimo.ExecuteNonQuery()
        Ultimo = New SqlCommand("DELETE FROM TMPBILS WHERE TMSBLOCK = " & IdBlk, cnCo)
        Ultimo.ExecuteNonQuery()
        Ultimo = New SqlCommand("DELETE FROM TMPANNS WHERE TMSBLOCK = " & IdBlk2, cnCo)
        Ultimo.ExecuteNonQuery()
    End Sub
    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        'Dim x As Int16
        Cursor.Current = Cursors.WaitCursor
        DateEdit4 = CDate(DateEdit2.EditValue).AddYears(-1)
        DateEdit5 = CDate(DateEdit3.EditValue).AddYears(-1)
        d1 = CDate(DateEdit2.EditValue)
        d2 = CDate(DateEdit3.EditValue)
        q = 0
        CaricaSaldi()
        SparaStampa(Rpt2, "Patrimoniale e Conto Economico", "{CrBilProg.TMSBLOCK} = ", False, True, "SitPpp", True)
        SparaStampa(Rpt3, "Progressivo Clienti e Fornitori", "{CrBilClFo.TMCBLOCK} = ", True, False, "ProgCf", False)
        If CheckEdit6.Checked = True Then EsegueSole24Ore()
        If CheckEdit12.Checked = True Then
            For p As Int16 = 1 To 2
                q = p
                If p = 2 Then
                    d1 = CDate(DateEdit4)
                    d2 = CDate(DateEdit5)
                End If
                RicaricaSaldi()
            Next
            BilancioCee()
        End If
        Me.Close()
    End Sub
    Sub RicaricaSaldi()
        If q = 1 Then IdBlk2 = semaforo("Bilancio AL " & Today.Date)
        EsegueProcedura("XF1", CDate(d1).ToShortDateString, CDate(d2).ToShortDateString, IdBlk2, 0, q, CauChiusura)
    End Sub
    Private Sub EsegueSole24Ore()
        If Trim(TextEdit1.EditValue) = "" Then Exit Sub
        Sole24Ore(Trim(TextEdit1.EditValue), IdBlk)
    End Sub
    Sub SparaStampa(ByVal RRpt As ReportClass, ByVal Txt As String, ByVal Formula As String, ByVal Dettaglio As Boolean, ByVal QAmm As Boolean, ByVal TextPdf As String, ByVal TipoS As Boolean)
        Cursor.Current = Cursors.WaitCursor
        Rpt = New ReportClass
        frm = New LpLp
        Dim Selectformula As String
        Rpt = RRpt
        Selectformula = Formula & IdBlk
        Rpt.RecordSelectionFormula = Selectformula
        Rpt.SetParameterValue("periodo", CDate(DateEdit3.EditValue).ToShortDateString)
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("Dettaglio", Dettaglio)
        Rpt.SetParameterValue("Intesta", Txt.ToUpper)
        If TipoS = True Then
            Rpt.SetParameterValue("Ammortamenti", "")
            Rpt.SetParameterValue("Quote", 0)
        End If
        If TipoStampa = 1 Then
            PdfStart(Rpt, TextPdf & IdBlk)
            Exit Sub
        End If
        frm.reportsource = Rpt
        frm.Text = Txt
        frm.Show()
        Cursor.Current = Cursors.Default
    End Sub
    Sub CaricaSaldi()
        If OkFlash = False Then
            OkFlash = True
            IdBlk = semaforo("Bilancio AL " & Today.Date)
        Else
            PuliziaFlash()
        End If
        EsegueProcedura("XF1", CDate(d1).ToShortDateString, CDate(d2).ToShortDateString, IdBlk, 0, 0, CauChiusura)
    End Sub
    Function EsegueProcedura(ByVal QualeP As String, ByVal Dal As String, ByVal Al As String, ByVal IdBlk As Int32, ByVal Caus As Int16, ByVal Switch As Int16, ByVal CauChi As Int16) As Boolean
        EsegueSql(" EXEC " & QualeP & "  @DAL ='" & Dal & "', @AL = '" & Al & "' , @BLOCK = " & IdBlk & " , @CAUS= " & Caus & " , @SW= " & Switch & ", @CAUCH= " & CauChi, cnCo)
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
        x = x + s
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
        TipoStampa = EseFormato(x)
        CauChiusura = EseBilChi(x)
        ComboBoxEdit1.EditValue = EseAnno(x)
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
        ''LeggiAzienda()
        Dim Str As String = "Select  * from tbazi inner join vdox.dbo.tbana on anacod = azicod  where AziAnnoLavoro = " & ComboBoxEdit1.EditValue
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
        Periodo = Mid(CDate(DateEdit3.EditValue).ToShortDateString, 7, 4) & Mid(CDate(DateEdit3.EditValue).ToShortDateString, 4, 2) & Mid(CDate(DateEdit3.EditValue).ToShortDateString, 1, 2)
        If CheckEdit6.Checked = True Then
            TextEdit1.Enabled = True
            VerifyDir()
            TextEdit1.EditValue = PathTmp & "SOLE" & Periodo
        Else
            TextEdit1.Enabled = False
        End If
    End Sub
#Region "BILANCIO CEE"
    Private Sub CheckEdit12_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckEdit12.CheckedChanged
        '  RadioGroup1.Enabled = CheckEdit12.Checked
    End Sub
    Sub BilancioCee()
        Cursor.Current = Cursors.WaitCursor
        Dim ContiNc As Int16 = 0
        Dim Anno As Int16 = CDate(DateEdit3.EditValue).Year
        Dim TbNcl = New DataTable
        Dim DaNcl = New SqlDataAdapter("select * from crsole24ore2ANNI where rigo = 0 and TMSBLOCK =" & IdBlk2 & " order by PiaCodCo", cnCo)
        DaNcl.SelectCommand.CommandTimeout = 300
        DaNcl.Fill(TbNcl)
        ContiNc = TbNcl.Rows.Count
        Dim Azienda As String = Marchio()
        EsegueSql("EXEC [XBILCEE2ANNI] @BLOCK=" & IdBlk2 & ",@ANNO=" & Anno, cnCo)
        Dim StrPrint As String = "select  BILCEECOD,BILCEEDES,BILCEEST,BILCODICE,BILDESCR,BILCEEDESCEE,BILSALDO=SUM(BILSALDO),BILSALDOA=SUM(BILSALDOA) from TbBilCee GROUP BY BILCEECOD,BILCEEDES,BILCEEST,BILCODICE,BILDESCR,BILCEEDESCEE"
        'If RadioGroup1.EditValue = True Then
        '    StrPrint = "select  BILCEECOD,BILCEEDES,BILCEEST,BILCEEDESCEE,BILSALDO=SUM(BILSALDO),BILSALDOA=SUM(BILSALDOA) from TbBilCee GROUP BY BILCEECOD,BILCEEDES,BILCEEST,BILCEEDESCEE"
        'End If

        Dim DsSez = New DataTable
        Dim DaSez = New SqlDataAdapter(StrPrint, cnCo)
        DaSez.SelectCommand.CommandTimeout = 300
        DaSez.Fill(DsSez)
        REPORT = New DxBilCee
        REPORT.DataSource = DsSez
        REPORT.DataMember = "DsSez"
        REPORT.Parameters("Periodo1").Value = "Esercizio Al " & CDate(DateEdit3.EditValue).ToShortDateString
        REPORT.Parameters("Periodo2").Value = "Esercizio Al " & CDate(DateEdit5).ToShortDateString
        REPORT.Parameters("Marchio").Value = Azienda
        REPORT.Parameters("Indirizzo").Value = Indir & " " & Citta & " " & Prov
        REPORT.Parameters("CFPI").Value = "Codice Fiscale e Partita Iva " & Cfis
        REPORT.Parameters("DettaglioCee").Value = False
        REPORT.Parameters("OneYear").Value = False
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