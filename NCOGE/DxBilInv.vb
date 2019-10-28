Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Public Class DxBilInv
    Dim x, y, UltimaApertura, sw, MaxEse, EseFormato(5), TipoStampa, EseBilChi(5), EseAnno(5), CauChiusura, ANNOSTA As Int16
    Dim IdBlk, NPAG As Int32
    Dim frm As New LpLp
    Dim Rpt As New ReportClass
    Dim Rpt1 As New Invent
    Dim Rpt2 As New InvNoCf
    Dim Rpt3 As New InvPart
    Dim TIPOLP(), RESETLP(), CUTI, CPER, periodo, uti, per, AZI, INTINV, pg As String
    Dim Rispondi As MsgBoxResult

    Private Sub DxBilInv_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        pulizia()
        TextEdit11.Focus()
    End Sub
    Private Sub pulizia()
        RadioGroup1.SelectedIndex = 0
        CheckEdit1.Checked = False
        TextEdit1.EditValue = ""
        TextEdit2.EditValue = ""
        TextEdit3.EditValue = ""
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = ""
        TextEdit6.EditValue = "Il presente bilancio è vero e reale."
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = ""
        TextEdit9.EditValue = ""
        TextEdit10.EditValue = ""
        TextEdit11.EditValue = ""
    End Sub

    Private Sub LEGGIAZI()
        Dim Cmd As New SqlCommand("SELECT * FROM TBAZI where AziAnnoLavoro = " & Val(TextEdit11.Text), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            AZI = dataRd("AziCod")
        End While
        dataRd.Close()
        leggiana()
        CaricaSaldi()
    End Sub
    Private Sub leggiana()
        Dim Cmd As New SqlCommand("SELECT * FROM tbana where AnaCod = '" & AZI & "' and AnaGrp = 'AZ'", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit1.EditValue = dataRd("AnaDesc")
            TextEdit2.EditValue = dataRd("AnaIndirizzo")
            TextEdit3.EditValue = dataRd("AnaCap") & " " & dataRd("AnaCitta") & " " & dataRd("AnaProv")
            TextEdit4.EditValue = dataRd("AnaPiva") & " - " & dataRd("AnaCfis")
            TextEdit5.EditValue = ""
        End While
        dataRd.Close()
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub SeChiuso()
        ''controllo se fatta la chiusura
        MaxEse = -1
        Dim Cmd As New SqlCommand("SELECT * FROM TBpri where year(pridataest) = " & ANNOSTA & " and pricausale = " & CauChiusura, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MaxEse = MaxEse + 1
        End While
        dataRd.Close()
    End Sub

    Sub CaricaSaldi()
        CUTI = ""
        CPER = ""
        Dim Cmd As New SqlCommand("SELECT * FROM TBESE where eseanno = " & Val(TextEdit11.Text), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            periodo = "BILANCIO ESERCIZIO " & dataRd("EseDal") & " - " & dataRd("EseAl")
            uti = dataRd("EseUti")
            per = dataRd("EsePer")
            CauChiusura = dataRd("EseCausaleChiusuraConti")
            INTINV = dataRd("EseInvDesc")
            NPAG = dataRd("EseInvNFog")
        End While
        dataRd.Close()
        utiper()
    End Sub
    Private Sub utiper()
        CUTI = uti
        CPER = per
        Dim Cmd As New SqlCommand("SELECT * FROM TBPia where PiaCodCo = '" & uti & "'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CUTI = uti & " " & dataRd("PiaAnaCo")
        End While
        dataRd.Close()

        Dim Cmd1 As New SqlCommand("SELECT * FROM TBPia where PiaCodCo = '" & per & "'", cnCo)
        dataRd = Cmd1.ExecuteReader
        While dataRd.Read
            CPER = per & " " & dataRd("PiaAnaCo")
        End While
        dataRd.Close()
    End Sub
    Sub EsegueProcedura(ByVal QualeP As String, ByVal AnnoSta As Int16, ByVal CauChiusura As Int16)
        EsegueSql(" EXEC " & QualeP & "  @Anno =" & AnnoSta & ", @CAUCH= " & CauChiusura, cnCo)
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        ''If VERIFICA() = False Then Exit Sub
        If Val(TextEdit11.Text) = 0 Then Exit Sub

        EsegueProcedura("XF5", ANNOSTA, CauChiusura)
        Cursor.Current = Cursors.WaitCursor
        Rpt = New ReportClass
        frm = New LpLp
        If RadioGroup1.SelectedIndex = 0 Then
            Rpt = Rpt1
        ElseIf RadioGroup1.SelectedIndex = 1 Then
            Rpt = Rpt2
        ElseIf RadioGroup1.SelectedIndex = 2 Then
            Rpt = Rpt3
        End If
        If CheckEdit1.Checked = False Then NPAG = 0
        'Rpt.RecordSelectionFormula = Selectformula
        Rpt.SetParameterValue("periodo", periodo)
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("TB1", TextEdit1.EditValue)
        Rpt.SetParameterValue("TB2", TextEdit2.EditValue)
        Rpt.SetParameterValue("TB3", TextEdit3.EditValue)
        Rpt.SetParameterValue("TB4", TextEdit4.EditValue)
        Rpt.SetParameterValue("TB5", TextEdit5.EditValue)
        Rpt.SetParameterValue("TB6", TextEdit6.EditValue)
        Rpt.SetParameterValue("TB7", TextEdit7.EditValue)
        Rpt.SetParameterValue("TB8", TextEdit8.EditValue)
        Rpt.SetParameterValue("TB9", TextEdit9.EditValue)
        Rpt.SetParameterValue("TB10", TextEdit10.EditValue)
        Rpt.SetParameterValue("CUTI", CUTI)
        Rpt.SetParameterValue("CPER", CPER)
        Rpt.SetParameterValue("ANNOSTA", ANNOSTA)
        Rpt.SetParameterValue("NPAG", NPAG)
        Rpt.SetParameterValue("INTINV", INTINV)
        frm.reportsource = Rpt
        frm.Text = "Stampa Inventario"
        frm.ShowDialog()
        Dim FILE As String = LEGGIPATH() & "InvTmp.RTF"
        Rpt.ExportToDisk(ExportFormatType.RichText, FILE)
        RichTextBox1.LoadFile(FILE)
        pg = ""
        Dim x As Int32
        For x = Len(RichTextBox1.Text) To 1 Step -1
            If IsNumeric(Mid(RichTextBox1.Text, x, 1)) Then
                pg = Mid(RichTextBox1.Text, x, 1) & pg
            Else
                Exit For
            End If
        Next
        Me.Close()
    End Sub
    Private Function LEGGIPATH() As String
        Dim Str As String = "Select Sel8 from TbSel where selId = 1"
        Cmd = New SqlCommand(Str, cnVd)
        LEGGIPATH = Cmd.ExecuteScalar
        If Directory.Exists(LEGGIPATH) = False Then Directory.CreateDirectory(LEGGIPATH)
    End Function
    Private Sub MemoNPag()
        Dim Com As String
        Com = "Update TbEse set EseInvNFog=@EseInvNFog where eseAnno=" & ANNOSTA
        Cmd = New SqlCommand(Com, cnCo)
        Dim p24 As New SqlParameter("@EseInvNFog", SqlDbType.Int)
        p24.Value = Val(pg)
        Cmd.Parameters.Add(p24)
        Cmd.ExecuteNonQuery()
        NPAG = Val(pg)
    End Sub
    Private Sub TextEdit11_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles TextEdit11.Leave
        If VERIFICA() = True Then ButtonF9.Focus()
    End Sub
    Function VERIFICA() As Boolean
        ANNOSTA = Val(TextEdit11.Text)
        LEGGIAZI()
        SeChiuso()
        If MaxEse = -1 Then
            Messaggio(1, "Manca Chiusura Esercizio")
            TextEdit11.Focus()
            Return False : Exit Function
        End If
        Return True
    End Function
    Private Function controllo() As Boolean
        controllo = True
        If Not IsNumeric(TextEdit11.Text) Then
            Return False
        End If
        If Val(TextEdit11.Text) < 2000 Then
            Return False
        End If
    End Function
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "SELEZIONE ANNO INVENTARIO"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Private Sub DxBilInv_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If CheckEdit1.Checked = True Then MemoNPag()
    End Sub
End Class