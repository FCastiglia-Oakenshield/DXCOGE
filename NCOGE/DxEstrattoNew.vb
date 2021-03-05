Imports NPRINT
Imports System.Data.SqlClient
Imports DXBASE
Imports CrystalDecisions.CrystalReports.Engine
Imports DevExpress.XtraReports.UI

Public Class DxEstrattoNew
    Dim cli As Boolean
    Dim REPORT As New XtraReport
    Dim dscade As String
    Dim MiglioFo As Int32
    Dim Azienda As String = ""
    Dim Rispondi As MsgBoxResult
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim StampaDefault As Integer = -1
    Dim TbEsc As DataTable
    Dim DsEsc As SqlDataAdapter

    Dim TbTxt As DataTable
    Dim DsTxt As SqlDataAdapter


    Private Sub DxEstratto_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        PrimoMigliaio()
        Pulizia(True)
        LeggiTesto()
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
    Private Sub LeggiTesto()
        ImageComboB2.Properties.Items.Clear()
        Dim i As Int16 = -1
        Dim SS As Int16 = 27
        StampaDefault = 0
        Dim Str As String = "SELECT * from TbPos order by PosId"
        TbTxt = New DataTable
        DsTxt = New SqlDataAdapter(Str, cnCo)
        DsTxt.Fill(TbTxt)
        For J As Int16 = 1 To TbTxt.Rows.Count
            Rw = TbTxt.Rows(J - 1)
            If Rw("PosId") = 0 Then SS = 63 Else SS = 64
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Rw("PosNomeModulo"), Rw("PosNomeModulo"), SS)
            ImageComboB2.Properties.Items.Add(nn)
            If Rw("PosUltima") = True Then StampaDefault = J - 1
        Next
        ImageComboB2.SelectedIndex = StampaDefault
    End Sub
    Private Sub ImageComboB2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ImageComboB2.SelectedIndexChanged
        If ImageComboB2.SelectedIndex > -1 Then
            Rw = TbTxt.Rows(ImageComboB2.SelectedIndex)
            MemoEdit1.EditValue = Rw("PosTesto")
            MemoEdit3.EditValue = Rw("PosPiede")
            TextE26.EditValue = Rw("PosNomeModulo")
            If ImageComboB2.SelectedIndex > 0 Then ButtonF3.Enabled = True Else ButtonF3.Enabled = False
        End If
    End Sub

    Private Sub ButtonPlus_Click(sender As Object, e As EventArgs) Handles ButtonPlus.Click
        GroupControl11.Enabled = True
        GroupControl9.Enabled = True
        GroupControl10.Enabled = True
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
            MemoEdit1.EditValue = ""
            MemoEdit3.EditValue = ""
            MemoEdit1.EnterMoveNextControl = False
            MemoEdit3.EnterMoveNextControl = False
            RadioGroup1.SelectedIndex = 0
            CheckEdit1.Checked = False
            CheckEdit2.Checked = False
            CheckEdit3.Checked = False
            GroupControl9.Enabled = False
            GroupControl10.Enabled = False
            GroupControl11.Enabled = False
            leggiazienda()
            DateEdit1.EditValue = CDate(Today)
            ImageComboB2.SelectedIndex = -1
        End If
        If Puliscitutto = False Then
            TextEdit1.EditValue = "00000"
            TextEdit2.EditValue = ""
            TextEdit3.EditValue = ""
            TextEdit4.EditValue = ""
            TextEdit5.EditValue = ""
            TextEdit6.EditValue = ""
            GroupControl11.Enabled = False
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
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        Rispondi = MsgBox("ELIMINO COMPLETAMENTE IL MODULO '" & ImageComboB2.EditValue & "' DALL'ARCHIVIO ?", MsgBoxStyle.YesNo, "ELIMINA MODULO")
        If Rispondi = MsgBoxResult.Yes Then
            Cmd = New SqlCommand("delete from TbPos where PosNomeModulo ='" & ImageComboB2.EditValue & "'", cnCo)
            Cmd.ExecuteNonQuery()
            LeggiTesto()
            GroupControl9.Enabled = False
            GroupControl10.Enabled = False
            GroupControl11.Enabled = False
        End If

    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If CheckEdit3.Checked = True Then
            If MemoEdit1.EditValue.ToString.Length <= 0 Or MemoEdit3.EditValue.ToString.Length <= 0 Then
                MemoEdit1.Focus()
                Exit Sub
            End If
        End If
        If TextE26.EditValue.ToString.Length < 2 Then
            TextE26.ErrorText = "INSERIRE NOME MODULO"
            Exit Sub
        End If

        dscade = CDate(DateEdit1.EditValue).ToShortDateString

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
        REPORT.Parameters("Libero").Value = True
        REPORT.Parameters("TB15").Value = MemoEdit1.EditValue.ToString
        REPORT.Parameters("TB17").Value = MemoEdit3.EditValue.ToString

        REPORT.ShowPreview()

        Rw = TbTxt.Rows(0)
        If Rw("PosNomeModulo") <> Trim(TextE26.EditValue.ToString) Then
            AggiornaTesti()
            LeggiTesto()
            GroupControl9.Enabled = False
            GroupControl10.Enabled = False
            GroupControl11.Enabled = False
        Else
            Cmd = New SqlCommand("update TbPos set PosUltima=0", cnCo)
            Cmd.ExecuteNonQuery()
            Cmd = New SqlCommand("update TbPos set PosUltima=1 where PosId = 0", cnCo)
        End If

    End Sub
    Sub RegistraUltima(N)
        Cmd = New SqlCommand("update TbPos set PosUltima=0", cnCo)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("update TbPos set PosUltima=1,PosTesto=@A,PosPiede=@B where PosId = " & N, cnCo)
        Dim p1 As New SqlParameter("@A", SqlDbType.NVarChar)
        Dim p2 As New SqlParameter("@B", SqlDbType.NVarChar)
        p1.Value = Trim(MemoEdit1.EditValue)
        p2.Value = Trim(MemoEdit3.EditValue)
        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.ExecuteNonQuery()
    End Sub
    Sub AggiornaTesti()
        Dim N As Int16 = 0
        For J As Int16 = 2 To TbTxt.Rows.Count
            Rw = TbTxt.Rows(J - 1)
            If Rw("PosNomeModulo") = Trim(TextE26.EditValue.ToString) Then
                RegistraUltima(Rw("PosId"))
                Exit Sub
            End If
        Next
        Cmd = New SqlCommand("Insert into TbPos(PosId,PosCauInsoluti,PosNomeModulo,PosTesto,PosPiede,PosUltima) values (@Id,@Ins,@NomeModulo,@Testo,@Piede,@Ultima)", cnCo)
        Dim p0 As New SqlParameter("@Id", SqlDbType.TinyInt)
        Dim p1 As New SqlParameter("@Ins", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@NomeModulo", SqlDbType.NVarChar)
        Dim p3 As New SqlParameter("@Testo", SqlDbType.NVarChar)
        Dim p4 As New SqlParameter("@Piede", SqlDbType.NVarChar)
        Dim p5 As New SqlParameter("@Ultima", SqlDbType.Bit)

        p0.Value = TbTxt.Rows.Count
        p1.Value = -1
        p2.Value = TextE26.EditValue
        p3.Value = MemoEdit1.EditValue
        p4.Value = MemoEdit3.EditValue
        p5.Value = True

        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p0)
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.ExecuteNonQuery()

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