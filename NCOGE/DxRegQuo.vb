Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class DxRegQuo
    Dim CspGru, CspSpe1, CspSpe2 As String
    Dim selectformula As String
    Dim TbCat As DataTable
    Dim DaCat As SqlDataAdapter
    Dim Anno As Int16 = 0
    Dim Esercizio As Int16 = 0
    Dim UltAnnoAmm As Int16 = 0
    Dim DataMin, DataMax, MaxDat, MinDat, MMMDat As Date

    Dim Dsh7 As DataTable
    Dim DaH7 As SqlDataAdapter
    Dim Tipo As Int16 = -1
    Dim RwX As DataRowView
    Dim nn As DevExpress.XtraEditors.Controls.ImageComboBoxItem

    Private Sub DxRegQuo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        IniziaTabella()
        If DataMax.Year < Anno Then
            MsgBox("NON ESISTONO QUOTE DA CALCOLARE PER L'ANNO " & Anno, MsgBoxStyle.Information, "CALCOLO AMMORTAMENTI")
            OscuroCalcoloQuote()
            RiassegnoDati()
            Exit Sub
        End If
        PopolaGrid1()
    End Sub
    Sub OscuroCalcoloQuote()
        GroupBox2.Enabled = False : GridControl1.Enabled = False : GroupControl7.Enabled = True : GroupControl5.Enabled = True
    End Sub
    Sub RiassegnoDati()
        TextEdit2.EditValue = UltAnnoAmm : CheckEdit1.Checked = True : Anno = UltAnnoAmm
        RiLeggiEsercizio()
        RileggoUltimi()
    End Sub
    Sub RiLeggiEsercizio()
        Cmd = New SqlCommand("SELECT  * from TbEse where EseAnno = " & Anno, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            DataMin = dataRd.Item("EseDal")
            DataMax = dataRd.Item("EseAl")
        End If
        dataRd.Close()
    End Sub
    Private Sub IniziaTabella()
        Esercizio = AnnoEsercizio()
        Dim Str As String = "select MAX(CespUltAnnoAmm) from TbCesp"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            UltAnnoAmm = dataRd.Item(0)
        End If
        dataRd.Close()
        Anno = UltAnnoAmm + 1
        If Anno < 2000 Then Anno = Esercizio
        Str = "Select * from TbAzi where aziannolavoro=" & Anno
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            CspGru = dataRd.Item("AziGruppoCesp")
            CspSpe1 = dataRd.Item("AziSpecieCesp")
            CspSpe2 = dataRd.Item("AziSottosCesp")
        End If
        dataRd.Close()

        RiLeggiEsercizio()

        TextEdit4.EditValue = Anno
        TextEdit2.EditValue = Anno
        Str = "SELECT * from TbCii order by CiiCod"
        Dim SS As String = ""
        ImageComboBoxEdit2.Properties.Items.Clear()
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("CiiCod") > 3 Then
                SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau")
                nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
                ImageComboBoxEdit2.Properties.Items.Add(nn)
            End If
        End While
        dataRd.Close()
        RileggoUltimi()
        AccendiSpegni(CheckEdit1.Checked)
        ImageComboBoxEdit2.SelectedIndex = -1 : TextEdit1.EditValue = "" : TextEdit5.EditValue = ""
    End Sub
    Sub RileggoUltimi()
        ''' ' LEGGO DA TBPRI MAX DATAGIO E MAX NUMART
        Dim Cmd As New SqlCommand("Select Top 1 EseCausaleChiusuraConti from Tbese order by Eseanno Desc", cnCo)
        If DateEdit1.EditValue Is Nothing Then DateEdit1.EditValue = Today
        MaxDat = DateEdit1.EditValue
        Dim PassData As String = MaxDat.ToShortDateString
        Dim Str As String = ""
        Str = "SELECT isnull(Max(PridataGio),'" & PassData & "') as PriDataGio from TbPri where PriregIva = 0"
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MaxDat = dataRd.Item("PridataGio")
        End While
        dataRd.Close()
        DateEdit1.EditValue = MaxDat
        Str = "SELECT isnull(MAX(PRIDATAGIO),(select esedal from tbese where eseanno = (select MIN(aziannolavoro)from tbazi ))) FROM TBPRI WHERE PRIGSTAMPA = 1 AND PRIARTFISC > 0 "
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MinDat = dataRd.Item(0)
        End While
        dataRd.Close()
    End Sub

    Private Sub PopolaGrid1()
        Dim cmd As New SqlCommand("Select CspNum,CspDesc,0 AS TipoAmm,CspPerc,0.00 as Perc,cast(0 as bit) as Flag from TbCsp where CspGru=" & CspGru & " and CspSpe1=" & CspSpe1 & " and CspSpe2=" & CspSpe2 & " and CspNum>0", cnCo)
        TbCat = New DataTable()
        DaCat = New SqlDataAdapter(cmd)
        DaCat.Fill(TbCat)
        GridControl1.DataSource = TbCat
        GridView1.ClearSelection()
    End Sub
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If GroupControl7.Enabled = False Then
            If e.KeyData = Keys.F11 Then
                ButtonF11.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F5 Then
                ButtonF5.PerformClick()
                Exit Sub
            End If
        Else
            If e.KeyData = Keys.F11 Then
                ButtonXF11.PerformClick()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub RepositoryItemTextEdit3_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RepositoryItemTextEdit3.Validating
        Tipo = Val(sender.editvalue)
        If Tipo > 4 Then e.Cancel = True : Exit Sub
    End Sub

    Private Sub RepositoryItemTextEdit1_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RepositoryItemTextEdit1.Validating
        If CDec(sender.editvalue) > 100 Then e.Cancel = True : Exit Sub
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If CheckEdit1.Checked = True Then
            If Controlli() = False Then Exit Sub
        End If
        If MessageBox.Show("Procedere con l'Ammortamento?", "Elaborazione Ammortamenti", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then Return
        Elabora()
        If CheckEdit1.Checked = True Then RegistraPrimaNota()
        Me.Close()
    End Sub
    Sub RegistraPrimaNota()
        Cursor.Current = Cursors.WaitCursor
        Dim REPORT As New XtraReport
        Dim Titolo As String = "Registrazioni AMMORTAMENTI ANNO " & Anno
        Dim str As String = "Exec XPRIQUO @ANNO=" & Anno & ",@DATAOP='" & DateEdit2.EditValue.toshortdatestring & "',@DATAGIO='" & DateEdit1.EditValue.toshortdatestring & "',@CAUS=" & ImageComboBoxEdit2.EditValue & ",@DESCR= '" & TextEdit1.EditValue.ToString & "', @NDOC='" & TextEdit5.EditValue.ToString & "'"
        Dsh7 = New DataTable
        DaH7 = New SqlDataAdapter(str, cnCo)
        DaH7.SelectCommand.CommandTimeout = 300
        DaH7.Fill(Dsh7)

        REPORT = New DxStH7PN
        REPORT.DataSource = Dsh7
        REPORT.DataMember = "DsH7"
        REPORT.Parameters.Item("Marchio").Value = Marchio()
        REPORT.Parameters.Item("Titolo").Value = Titolo
        REPORT.ShowPreview()
    End Sub
    Private Sub Elabora()
        Cursor.Current = Cursors.WaitCursor
        For I As Int32 = 1 To GridView1.RowCount
            RwX = GridView1.GetRow(I - 1)
            Dim Cesp As New ArrayList
            Cmd = New SqlCommand("Select CespNum from TbCesp where CespCat=" & RwX("CspNum"), cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                Cesp.Add(dataRd.Item("CespNum"))
            End While
            dataRd.Close()
            EseguiProc(Cesp)
        Next
    End Sub

    Private Sub EseguiProc(ByVal Cesp As ArrayList)
        For J As Int32 = 1 To Cesp.Count
            Cmd = New SqlCommand("EXEC XRegQuo @Data =@Data,@TipoAmm =@TipoAmm,@AmmLib =@AmmLib,@DaCat=@DaCat,@ACat=@ACat,@Num=@Num", cnCo)

            Dim p1 As New SqlParameter("@Data", SqlDbType.SmallDateTime)
            Dim p2 As New SqlParameter("@TipoAmm", SqlDbType.Int)
            Dim p3 As New SqlParameter("@AmmLib", SqlDbType.Decimal)
            Dim p4 As New SqlParameter("@DaCat", SqlDbType.Int)
            Dim p5 As New SqlParameter("@ACat", SqlDbType.Int)
            Dim p6 As New SqlParameter("@Num", SqlDbType.Int)

            p1.Value = CDate(DataMax)
            p2.Value = RwX("TipoAmm")
            p3.Value = RwX("Perc")
            p4.Value = 0 'DaCat
            p5.Value = 99 'ACat
            p6.Value = Val(Cesp(J - 1))
            Cmd.Parameters.Clear()
            Cmd.Parameters.Add(p1)
            Cmd.Parameters.Add(p2)
            Cmd.Parameters.Add(p3)
            Cmd.Parameters.Add(p4)
            Cmd.Parameters.Add(p5)
            Cmd.Parameters.Add(p6)

            Cmd.CommandTimeout = 600 ' 10 MINUTI ( 600 SECONDI )
            Cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Sub GridView1_ValidateRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs) Handles GridView1.ValidateRow
        RwX = e.Row
        If RwX("TipoAmm") <> 2 And RwX("Perc") <> 0 Then
            e.Valid = False
        End If
        If RwX("TipoAmm") = 2 And RwX("Perc") = 0 Then
            e.Valid = False
        End If
    End Sub
    Sub AccendiSpegni(n As Boolean)
        GroupControl6.Enabled = n : GroupControl9.Enabled = n : GroupControl87.Enabled = n : GroupControl12.Enabled = n : GroupControl13.Enabled = n
    End Sub
    Private Sub CheckEdit1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckEdit1.CheckedChanged
        AccendiSpegni(CheckEdit1.Checked)
    End Sub
    Private Sub DatBox1_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.Validated
        DateEdit1.EditValue = CDate(DateEdit1.EditValue)
        If DateEdit2.EditValue Is Nothing Then DateEdit2.EditValue = DateEdit1.EditValue
    End Sub
    Private Sub DatBox2_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit2.Validated
        If CDate(DateEdit2.EditValue) > CDate(DateEdit1.EditValue) Then
            DateEdit2.EditValue = DateEdit1.EditValue
        End If
    End Sub
    Function Controlli() As Boolean
        Dim Mail As String = ""
        If CDate(DateEdit1.EditValue) > DataMax Then
            Mail &= "<>DATA GIORNALE > MASSIMA DATA VALIDA(" & DataMax.ToShortDateString & ")"
        End If
        If CDate(DateEdit1.EditValue) < MinDat Then
            Mail &= "<>DATA GIORNALE < MINIMA DATA VALIDA(" & MinDat.ToShortDateString & ")"
        End If
        If ImageComboBoxEdit2.SelectedIndex = -1 Then
            Mail &= "<>MANCA CAUSALE"
        End If
        If Mail > "" Then
            MoltoCritico(Mail)
            Return False
        End If
        Return True
    End Function

    Private Sub ButtonXF11_Click(sender As System.Object, e As System.EventArgs) Handles ButtonXF11.Click
        If CheckEdit1.Checked = True Then RegistraPrimaNota()
        Me.Close()
    End Sub
End Class