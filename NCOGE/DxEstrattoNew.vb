Imports NPRINT
Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports CrystalDecisions.CrystalReports.Engine
Imports DevExpress.XtraReports.UI

Public Class DxEstrattoNew
    Dim cli, ana As Boolean
    Dim frm As New LpLp
    Dim Rpt As ReportClass
    Dim Rpt1 As New StEstratto
    Dim Rpt2 As New StTabSCli
    Dim Rpt3 As New StEstLib
    Dim REPORT As New XtraReport
    Dim dluogo, dscade, rag1, rag2, rag3, rag4, rag5, firma, Stringstring, string0, Scadenza, SELESTR As String
    Dim corr, inte As Int16
    Dim MiglioFo As Int32
    Dim Azienda As String = ""

    Dim TbEsc As DataTable
    Dim DsEsc As SqlDataAdapter

    Private Sub DxEstratto_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        PrimoMigliaio()
        Pulizia(True)
        CheckEdit3.Enabled = False
    End Sub

    Private Sub PrimoMigliaio()
        Cmd = New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'CL'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
    End Sub
    Private Sub Pulizia(ByVal Puliscitutto As Boolean)
        If Puliscitutto = True Then
            TextEdit1.EditValue = "00000"
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
            TextEdit12.EditValue = ""
            TextEdit14.EditValue = ""
            'MemoEdit1.EditValue = ""
            MemoEdit3.EditValue = ""
            MemoEdit1.EnterMoveNextControl = False
            MemoEdit3.EnterMoveNextControl = False
            RadioGroup1.SelectedIndex = 0
            CheckEdit1.Checked = False
            CheckEdit2.Checked = False
            CheckEdit3.Checked = False
            GroupControl9.Enabled = True
            GroupControl10.Enabled = False
            leggiazienda()
            DateEdit1.EditValue = CDate(Today)
        End If
        If Puliscitutto = False Then
            TextEdit1.EditValue = "00000"
            TextEdit2.EditValue = ""
            TextEdit3.EditValue = ""
            TextEdit4.EditValue = ""
            TextEdit5.EditValue = ""
            TextEdit6.EditValue = ""
        End If
    End Sub
    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Pulizia(False)
    End Sub
    Private Sub leggiazienda()
        Azienda = ""
        Dim Cmd As New SqlCommand("SELECT max(AZIANNOLAVORO),azicod FROM TbAzi group by azicod ", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Azienda = dataRd.Item("azicod")
        End While
        dataRd.Close()
        If Azienda = "" Then Exit Sub

        ' leggo da tbana
        Dim Cmd1 As New SqlCommand("SELECT AnaDesc, AnaIndirizzo, AnaCap, AnaCitta, AnaProv FROM TbAna WHERE AnaGrp = 'AZ' AND AnaCod = '" & Azienda & "'", cnVd)
        dataRd = Cmd1.ExecuteReader
        While dataRd.Read
            TextEdit7.EditValue = (dataRd.Item("AnaDesc"))
            TextEdit8.EditValue = (dataRd.Item("AnaIndirizzo"))
            TextEdit9.EditValue = (dataRd.Item("AnaCap")) & " " & (dataRd.Item("AnaCitta")) & " " & (dataRd.Item("AnaProv"))
        End While
        dataRd.Close()
    End Sub


    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If CheckEdit3.Checked = True Then
            If MemoEdit1.EditValue.ToString.Length <= 0 Or MemoEdit3.EditValue.ToString.Length <= 0 Then
                MemoEdit1.Focus()
                Exit Sub
            End If
        End If
        dscade = CDate(DateEdit1.EditValue).ToShortDateString
        '' Dim StrPrint As String = "Select * from VEstratto where PrkConto ='" & TextEdit1.EditValue & "' order by PrkDocAnn,PrkDocEst,Prkaammgg"
        Dim StrPrint As String = "EXEC XESTRATTO @CLIE='" & TextEdit1.EditValue & "',@AL='" & dscade & "'"
        Cursor.Current = Cursors.WaitCursor
        TbEsc = New DataTable
        DsEsc = New SqlDataAdapter(StrPrint, cnCo)
        DsEsc.SelectCommand.CommandTimeout = 300
        DsEsc.Fill(TbEsc)

        REPORT = New DxStaEstratto
        REPORT.DataSource = TbEsc
        REPORT.DataMember = "TbEst"
        If CheckEdit2.Checked = False Then
            REPORT.Parameters("Rag1").Value = TextEdit7.EditValue
            REPORT.Parameters("Rag2").Value = TextEdit8.EditValue
            REPORT.Parameters("Rag3").Value = TextEdit9.EditValue
            REPORT.Parameters("Rag4").Value = TextEdit10.EditValue
            REPORT.Parameters("Rag5").Value = TextEdit11.EditValue
        End If
        REPORT.Parameters("firma").Value = TextEdit12.EditValue
        REPORT.Parameters("dluogo").Value = TextEdit14.EditValue
        REPORT.Parameters("Corr").Value = CheckEdit1.Checked
        REPORT.Parameters("dsca").Value = dscade
        REPORT.Parameters("Libero").Value = CheckEdit3.Checked
        REPORT.Parameters("TB15").Value = MemoEdit1.EditValue.ToString
        REPORT.Parameters("TB17").Value = MemoEdit3.EditValue

        REPORT.ShowPreview()




        ''   frm = New LpLp
        Stringstring = "" : string0 = "" : rag1 = "" : rag2 = "" : rag3 = "" : rag4 = ""
        rag5 = "" : firma = "" : dluogo = "" : SELESTR = ""
        ''   Rpt = New ReportClass
        ''   Rpt1 = New StEstratto
        ''  Rpt2 = New StTabSCli
        ''    Rpt3 = New StEstLib
        Dim selectformula As String
        If RadioGroup1.SelectedIndex = 0 And CheckEdit3.Checked = False Then
            ''      Rpt = Rpt1
            ''    frm.Text = "Estratto Conto"
            SELESTR = ""
        ElseIf RadioGroup1.SelectedIndex = 0 And CheckEdit3.Checked = True Then
            ''   Rpt = Rpt3
            ''   frm.Text = "Estratto Conto"
            SELESTR = ""
        Else
            ''   Rpt = Rpt2
            ''   frm.Text = "Riepilogo Crediti"
            SELESTR = " and {CRESTRATTO.ScaraPERTA}<> '1'"
        End If
        If TextEdit1.EditValue > "00000" Then Stringstring = " {CRESTRATTO.ScaConto} = '" & TextEdit1.EditValue & "' and "
        ' dscade = DateEdit1.EditValue
        'per utilizzo carta intestata - si seleziona carta intestata e io pulisco i valori rag1 rag2, ecc.
        'If CheckEdit2.Checked = False Then
        '    rag1 = TextEdit7.EditValue
        '    rag2 = TextEdit8.EditValue
        '    rag3 = TextEdit9.EditValue
        '    rag4 = TextEdit10.EditValue
        '    rag5 = TextEdit11.EditValue
        'End If
        'firma = TextEdit12.EditValue
        'If CheckEdit1.Checked = True Then corr = 1 Else corr = 0
        'dluogo = TextEdit14.EditValue
        'Scadenza = "date(" & DateEdit1.EditValue.Year & "," & DateEdit1.EditValue.Month & "," & DateEdit1.EditValue.Day & ")"
        'string0 = "{CRESTRATTO.ScaDSca} <= " & Scadenza & " and {CRESTRATTO.CLFOPI}= 'CL'"
        'Stringstring = Stringstring & string0 & SELESTR
        'selectformula = Stringstring
        'Rpt.RecordSelectionFormula = selectformula
        'Rpt.SetParameterValue("dluogo", dluogo)
        'Rpt.SetParameterValue("dscade", dscade)
        'Rpt.SetParameterValue("rag1", rag1)
        'Rpt.SetParameterValue("rag2", rag2)
        'Rpt.SetParameterValue("rag3", rag3)
        'Rpt.SetParameterValue("rag4", rag4)
        'Rpt.SetParameterValue("rag5", rag5)
        'Rpt.SetParameterValue("firma", firma)
        'Rpt.SetParameterValue("corr", corr)
        'Rpt.SetParameterValue("Marchio", Marchio())
        'If CheckEdit3.Checked = True Then
        '    Rpt.SetParameterValue("TB15", MemoEdit1.EditValue)
        '    Rpt.SetParameterValue("TB16", MemoEdit2.EditValue)
        '    Rpt.SetParameterValue("TB17", MemoEdit3.EditValue)
        'End If
        'frm.reportsource = Rpt
        'frm.Show()
    End Sub

    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click

        Dim frm As New RicercaClFo
        frm.StartPosition = FormStartPosition.Manual
        frm.Location = New Point(GroupControl1.Location.X, GroupControl1.Location.Y + 80)
        frm.CliFor = "CL"
        frm.ShowDialog()
        TextEdit1.EditValue = frm.Codice
        If TextEdit1.EditValue > "00000" Then
            leggiAna(TextEdit1.EditValue.ToString)
            TextEdit1.Focus()
        Else
            Pulizia(False)
        End If
    End Sub
    Private Sub leggiAna(ByVal codice As String)
        Cmd = New SqlCommand("select * from TbAna where AnaGrp = 'CL' and AnaCod ='" & codice & "'", cnVd)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            CaricaAna()
        Else
            TextEdit2.EditValue = ""
            TextEdit3.EditValue = ""
            TextEdit4.EditValue = ""
            TextEdit5.EditValue = ""
            TextEdit6.EditValue = ""
        End If
        dataRd.Close()
    End Sub
    Private Sub CaricaAna()
        TextEdit2.EditValue = dataRd.Item("AnaDesc")
        TextEdit3.EditValue = dataRd.Item("AnaIndirizzo")
        TextEdit4.EditValue = dataRd.Item("AnaCap")
        TextEdit5.EditValue = dataRd.Item("AnaCitta")
        TextEdit6.EditValue = dataRd.Item("AnaProv")
    End Sub
    Private Sub leggiClienti(ByVal codice As String)
        Cmd = New SqlCommand("select * from TbCli where ClCod ='" & codice & "'", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            cli = True
        Else
            cli = False
        End If
        dataRd.Close()
        If cli = False Then TextEdit1.Focus()
    End Sub
    Private Sub TextEdit1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.Leave
        If Val(TextEdit1.EditValue) > 1000 And Val(TextEdit1.EditValue) < MiglioFo Then
            leggiClienti(TextEdit1.EditValue)
            leggiAna(TextEdit1.EditValue)
        Else
            Pulizia(False)
        End If
    End Sub

    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            ButtonF8.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 Then
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub


    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex = 1 Then
            CheckEdit3.Checked = False : CheckEdit3.Enabled = False
        Else
            CheckEdit3.Enabled = True
        End If
    End Sub

    Private Sub CheckEdit3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckEdit3.CheckedChanged
        GroupControl9.Enabled = CheckEdit3.Checked
        GroupControl10.Enabled = CheckEdit3.Checked
    End Sub

    Private Sub MemoEdit3_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemoEdit3.Leave
        ButtonF9.Focus()
    End Sub
End Class