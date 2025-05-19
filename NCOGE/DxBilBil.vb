Imports CrystalDecisions.CrystalReports.Engine
Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI

Public Class DxBilBil

    Dim sw, MaxEse, EseFormato(10), TipoStampa, EseBilChi(10), CauChiusura As Int16
    Dim IdBlk As Int32
    Dim OkFlash As Boolean
    Dim Quote As Decimal = 0
    Dim Ammortamenti As String = ""
    Dim EseDal(10), EseAl(10), UltimaApertura As Date
    Dim EseProg(10), EseSppp(10), EseClFo(10), EseSdo(10), EseQuote(10) As Boolean
    Dim Rpt As New ReportClass
    Dim Rpt2 As New ConfSppp
    Dim frm As New LpLp
    Dim REPORT As New XtraReport
    Dim Cfis, Piva, Indir, Citta, Prov As String

    Private Sub DxBilBil_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If sw = 0 Then
            DateEdit1.EditValue = Today
            Apertura()
            LeggiAzienda()
            sw = 1
        End If
    End Sub
    Sub Apertura()
        Dim Cmd As New SqlCommand("SELECT isnull(max(pridataest),'01/01/2000') FROM Tbpri Where PriCausale = 45", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UltimaApertura = dataRd.Item(0)
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT top 10 * from TbEse Order by EseAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        Dim x As Int16
        MaxEse = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MaxEse = MaxEse + 1
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("EseAnno"))
            EseDal(MaxEse) = dataRd.Item("EseDal")
            EseAl(MaxEse) = dataRd.Item("EseAl")
            EseProg(MaxEse) = dataRd.Item("EseProg")
            EseSppp(MaxEse) = dataRd.Item("EseSppp")
            EseClFo(MaxEse) = dataRd.Item("EseClFo")
            EseSdo(MaxEse) = dataRd.Item("EseSdo")
            EseQuote(MaxEse) = dataRd.Item("EseQuote")
            EseFormato(MaxEse) = dataRd.Item("EseFormato")
            EseBilChi(MaxEse) = dataRd.Item("EseCausaleChiusuraConti")
        End While
        dataRd.Close()
        For x = 0 To MaxEse
            If ComboBoxEdit1.Properties.Items(x) = CDate(DateEdit1.EditValue).Year Then
                ComboBoxEdit1.SelectedIndex = x
                Esercizi(x)
                Exit For
            End If
        Next
    End Sub
    Private Sub LeggiAzienda()
        Dim Str As String = "Select  * from tbazi inner join vdox.dbo.tbana on anacod = azicod  where AziAnnoLavoro = " & ComboBoxEdit1.EditValue
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read = True Then
            Cfis = dataRd.Item("AnaCfis")
            Piva = dataRd.Item("AnaPiva")
            Indir = dataRd.Item("AnaIndirizzo")
            Citta = dataRd.Item("AnaCitta")
            Prov = dataRd.Item("AnaProv")
        End If
        dataRd.Close()
        Try
            Cmd = New SqlCommand("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TbBILCEE]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) BEGIN Select 1 END", cnCo)
            If Cmd.ExecuteScalar > 0 Then CheckEdit12.Visible = True Else CheckEdit12.Visible = False
        Catch ex As Exception
            CheckEdit12.Visible = False
        End Try
    End Sub
    Private Sub Sbilve_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If OkFlash = True Then PuliziaFlash()
    End Sub
    Sub PuliziaFlash()
        If OkFlash = False Then Exit Sub
        Dim Ultimo As New SqlCommand("DELETE FROM TMPANNS WHERE TMSBLOCK = " & IdBlk, cnCo)
        Ultimo.ExecuteNonQuery()
    End Sub
    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Cursor.Current = Cursors.WaitCursor
        CaricaSaldi()
        SparaStampa(Rpt2, "Confronto Patrimoniale e Conto Economico", "{CrBilConf.TMSBLOCK} = ", False, True, "SitConf")
        If CheckEdit12.Checked = True Then BilancioCee()
        Me.Close()
    End Sub
    Sub SparaStampa(ByVal RRpt As ReportClass, ByVal Txt As String, ByVal Formula As String, ByVal Dettaglio As Boolean, ByVal QAmm As Boolean, ByVal TextPdf As String)
        Cursor.Current = Cursors.WaitCursor
        Rpt = New ReportClass
        frm = New LpLp
        Dim Selectformula As String
        Rpt = RRpt
        Selectformula = Formula & IdBlk
        Rpt.RecordSelectionFormula = Selectformula
        Rpt.SetParameterValue("periodo", DateEdit1.EditValue)
        Rpt.SetParameterValue("Pperiodo", CDate(DateEdit5.EditValue).ToShortDateString)
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("Dettaglio", Dettaglio)
        Rpt.SetParameterValue("Intesta", Txt.ToUpper)
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
        Dim d1, d2, D3, D4 As String
        Dim Caus, x, q As Int16
        Dim P1 As Date
        Dim P2 As Date
        d1 = CDate(DateEdit2.EditValue).ToShortDateString
        d2 = CDate(DateEdit1.EditValue).ToShortDateString
        DateEdit4.EditValue = CDate(DateEdit2.EditValue).AddYears(-1)
        DateEdit5.EditValue = CDate(DateEdit1.EditValue).AddYears(-1)
        D3 = CDate(DateEdit4.EditValue).ToShortDateString
        D4 = CDate(DateEdit5.EditValue).ToShortDateString
        For q = 1 To 2
            Caus = 0
            If q = 1 Then
                P1 = d1
                P2 = d2
            ElseIf q = 2 Then
                P1 = D3
                P2 = D4
            End If
            If UltimaApertura < P1 And UltimaApertura > "01/01/2000" Then
                For x = 0 To MaxEse
                    If EseDal(x) = UltimaApertura Then
                        EsegueProcedura("XF1P", EseDal(x), EseAl(x), IdBlk, Caus, 0, EseBilChi(x))
                        Exit For
                    End If
                Next
            Else
                Caus = 45
            End If
            For x = 0 To MaxEse
                If ComboBoxEdit1.Properties.Items(x) = Mid(P2, 7, 4) Then
                    CauChiusura = EseBilChi(x)
                    Exit For
                End If
            Next
            EsegueProcedura("XF1", P1, P2, IdBlk, Caus, q, CauChiusura)
        Next
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
        If CDate(DateEdit1.EditValue) > Today Then DateEdit1.EditValue = Today
        TipoStampa = EseFormato(x)
        CauChiusura = EseBilChi(x)
        DateEdit1.Focus()
        ASSEGNAALTREDATE()
    End Sub
    Sub ASSEGNAALTREDATE()
        ''' PER EVITARE DI MODIFICARE I VALORI A VIDEO
        DateEdit4.Properties.MaxValue = "31/12/2050" ' reset campi per ricalcolare limiti
        DateEdit4.Properties.MinValue = "01/01/1900"
        DateEdit5.Properties.MaxValue = "31/12/2050"
        DateEdit5.Properties.MinValue = "01/01/1900"
        DateEdit6.Properties.MaxValue = "31/12/2050" ' reset campi per ricalcolare limiti
        DateEdit6.Properties.MinValue = "01/01/1900"
        DateEdit7.Properties.MaxValue = "31/12/2050"
        DateEdit7.Properties.MinValue = "01/01/1900"
        DateEdit6.EditValue = DateEdit2.EditValue
        DateEdit7.EditValue = DateEdit1.EditValue
        DateEdit4.EditValue = CDate(DateEdit2.EditValue).AddYears(-1)
        DateEdit5.EditValue = CDate(DateEdit1.EditValue).AddYears(-1)
        DateEdit4.Properties.MaxValue = CDate(DateEdit2.EditValue).AddYears(-1).AddMinutes(1)
        DateEdit4.Properties.MinValue = CDate(DateEdit2.EditValue).AddYears(-1)
        DateEdit5.Properties.MaxValue = CDate(DateEdit1.EditValue).AddYears(-1).AddMinutes(1)
        DateEdit5.Properties.MinValue = CDate(DateEdit1.EditValue).AddYears(-1)
        DateEdit6.Properties.MaxValue = CDate(DateEdit2.EditValue).AddMinutes(1)
        DateEdit6.Properties.MinValue = DateEdit2.EditValue
        DateEdit7.Properties.MaxValue = CDate(DateEdit1.EditValue).AddMinutes(1)
        DateEdit7.Properties.MinValue = DateEdit1.EditValue
    End Sub

    Private Sub DateEdit1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateEdit1.EditValueChanged
        If sw = 0 Then Exit Sub
        If DateEdit1.EditValue Is Nothing Then Exit Sub
        ASSEGNAALTREDATE()
    End Sub
#Region "BILANCIO CEE"
    Private Sub CheckEdit12_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckEdit12.CheckedChanged
        '  RadioGroup1.Enabled = CheckEdit12.Checked
    End Sub
    Sub BilancioCee()
        Cursor.Current = Cursors.WaitCursor
        Dim ContiNc As Int16 = 0
        Dim Anno As Int16 = CDate(DateEdit1.EditValue).Year
        Dim TbNcl = New DataTable
        Dim DaNcl = New SqlDataAdapter("select * from crsole24ore2ANNI where rigo = 0 and TMSBLOCK =" & IdBlk & " order by PiaCodCo", cnCo)
        DaNcl.SelectCommand.CommandTimeout = 300
        DaNcl.Fill(TbNcl)
        ContiNc = TbNcl.Rows.Count
        Dim Azienda As String = Marchio()
        EsegueSql("EXEC [XBILCEE2ANNI] @BLOCK=" & IdBlk & ",@ANNO=" & Anno, cnCo)
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
        REPORT.Parameters("Periodo1").Value = "Esercizio Al " & CDate(DateEdit1.EditValue).ToShortDateString
        REPORT.Parameters("Periodo2").Value = "Esercizio Al " & CDate(DateEdit5.EditValue).ToShortDateString
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