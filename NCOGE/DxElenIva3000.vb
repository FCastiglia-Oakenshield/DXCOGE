Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO



Public Class DxElenIva3000
    Dim DsMin As DataTable
    Dim DaMin As SqlDataAdapter
    Dim RwMin As DataRow

    Dim DsMax As DataTable
    Dim DaMax As SqlDataAdapter
    Dim RwMax As DataRow

    Dim DsEle As DataTable
    Dim DaEle As SqlDataAdapter
    Dim RwEle As DataRow

    Dim DsDai As DataTable
    Dim DaDai As SqlDataAdapter
    Dim RwDai As DataRow

    Dim DsVar As DataTable
    Dim DaVar As SqlDataAdapter
    Dim RwVar As DataRow

    Dim DsWri As DataTable
    Dim DaWri As SqlDataAdapter
    Dim RwWri As DataRow

    Dim DsPrt As DataTable
    Dim DaPrt As SqlDataAdapter
    Dim RwPrt As DataRow

    Dim TiRiep, StrPrint, StrR, Str As String
    Dim ANNO As Int16 = 2011
    Dim OkEsiste As Boolean = False
    Dim OpzControl As Array
    Dim Rispondi As MsgBoxResult
    Dim Bloccato As Boolean = False
    Dim PAGINVIO As Boolean = False
    Dim PAGVAR As Boolean = False
    Dim PAGFILES As Boolean = False
    Dim PathEle As String = ""
    Dim FileEle As String = ""
    Dim Title As String = "CONTROLLO ELENCHI"
    Dim output As TextWriter

    Private Sub DxElenIva3000_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        SetInizio()
    End Sub
    Sub SetInizio()
        XtraTabControl1.SelectedTabPageIndex = 0
        Bottoni(False)
        Pulizia()
        RadioGroup1.EditValue = PrimaLettura()
        If RadioGroup1.EditValue > "" Then Bottoni(True) : LockButton(Bloccato)
    End Sub
    Sub Bottoni(ByVal N As Boolean)
        ButtonF1.Enabled = Not N
        ButtonF3.Enabled = N
        ButtonP1A.Enabled = N
        XtraTabPage2.PageVisible = N
        XtraTabPage3.PageVisible = N
        XtraTabPage4.PageVisible = N
    End Sub
    Function PrimaLettura() As String
        Dim Ti As String = ""
        TextEdit12.EditValue = "" : TextEdit13.EditValue = "" : DateEdit1.EditValue = Nothing
        Dim Cmd As New SqlCommand("Select * from TbInEle where IeAnno =" & ANNO, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Ti = dataRd.Item("IeTipo")
            Bloccato = dataRd.Item("IeLock")
            ButtonF1.Enabled = False
            TextEdit12.EditValue = dataRd.Item("IeDrCodFisc")
            TextEdit13.EditValue = dataRd.Item("IeDrCaf")
            DateEdit1.EditValue = dataRd.Item("IeDrImpegno")
        End While
        dataRd.Close()
        Return Ti
    End Function
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        Messaggio(2, "RICARICO COMPLETAMENTE I DATI ? ")
        If Rispondi = MsgBoxResult.Yes Then
            Cmd = New SqlCommand("delete from TbEleCf where EleCfAnno =" & ANNO, cnCo)
            Cmd.ExecuteNonQuery()
            Cmd = New SqlCommand("delete from TbInEle where IeAnno =" & ANNO, cnCo)
            Cmd.ExecuteNonQuery()
            Cmd = New SqlCommand("delete from TbElVar where VaCfAnno =" & ANNO, cnCo)
            Cmd.ExecuteNonQuery()
            SetInizio()
        End If
    End Sub
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        Rispondi = MsgBox(Mexage, style(Tipo), Title)

    End Sub
#Region "LETTURA FATTURE e 1a GRID < 3000 euro  "
    Sub Pulizia()
        DsMin = New DataTable : DsMax = New DataTable : DsEle = New DataTable
        GridControl50.DataSource = DsMin : GridControl51.DataSource = DsMax : GridControl50.DataSource = DsEle
        GridControl1.DataSource = DsDai
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Controllo() = False Then Exit Sub
        Pulizia()
        Cursor.Current = Cursors.WaitCursor
        StrPrint = "Exec XELEDA2010 @ANNO= " & ANNO
        EsegueSql(StrPrint, cnCo)
        IPagina()
        Bottoni(True)
    End Sub
    Sub RegistraIndici()
        Cmd = New SqlCommand("delete from TbInEle where IeAnno =" & ANNO, cnCo) ''''' molto pericoloso'''' da rivedere
        Cmd.ExecuteNonQuery()
        If RadioGroup1.EditValue <= "" Then Exit Sub
        Cmd = New SqlCommand("INSERT INTO TbInEle (IeAnno,IeTipo,IeLock,IeDrCodFisc,IeDrCaf,IeDrImpegno) VALUES (@A1,@A2,@A3,@A4,@A5,@A6)", cnCo)
        Dim A1 As New SqlParameter("@A1", SqlDbType.SmallInt)
        Dim A2 As New SqlParameter("@A2", SqlDbType.NVarChar)
        Dim A3 As New SqlParameter("@A3", SqlDbType.Bit)
        Dim A4 As New SqlParameter("@A4", SqlDbType.NVarChar)
        Dim A5 As New SqlParameter("@A5", SqlDbType.NVarChar)
        Dim A6 As New SqlParameter("@A6", SqlDbType.SmallDateTime)
        A1.Value = ANNO
        A2.Value = RadioGroup1.EditValue
        A3.Value = Bloccato
        A4.Value = TextEdit12.EditValue
        A5.Value = TextEdit13.EditValue
        If DateEdit1.EditValue Is Nothing Then A6.Value = DBNull.Value Else A6.Value = DateEdit1.EditValue
        Cmd.Parameters.Add(A1)
        Cmd.Parameters.Add(A2)
        Cmd.Parameters.Add(A3)
        Cmd.Parameters.Add(A4)
        Cmd.Parameters.Add(A5)
        Cmd.Parameters.Add(A6)
        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()
    End Sub
    Sub IPagina()
        If RadioGroup1.EditValue = "CL" Then GroupControl1.Text = "CLIENTI " Else GroupControl1.Text = "FORNITORI"
        GroupControl3.Text = GroupControl1.Text
        GroupControl7.Text = GroupControl1.Text
        StrR = "Select * from VEleDa2010 where EleCfTipo = '" & RadioGroup1.EditValue & "' and EleCfAnno = " & ANNO & " and EleCfP = 0 order by EleCfAnaDesc,EleCfDataDoc,EleCfNumDoc"
        DsMin = New DataTable
        DaMin = New SqlDataAdapter(StrR, cnCo)
        DaMin.SelectCommand.CommandTimeout = 300
        DaMin.Fill(DsMin)
        GridControl50.DataSource = DsMin
        GridView50.ClearSelection()
        GridView50.ExpandAllGroups()
        LockButton(Bloccato)
    End Sub
    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex > -1 Then
            If ButtonF1.Enabled = False Then IPagina() Else ButtonF1.PerformClick()
        End If
    End Sub
    Function Controllo() As Boolean
        Dim Msg As Boolean = True
        If RadioGroup1.SelectedIndex = -1 Then
            RadioGroup1.ErrorText = "Selezionare CLienti o Fornitori"
            Msg = False
        End If
        Return Msg
    End Function
    Private Sub ButtonXF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonP1A.Click
        AssociaDaPaginaI()
    End Sub
    Sub AssociaDaPaginaI()
        OpzControl = GridView50.GetSelectedRows
        If OpzControl.Length <= 0 Then Exit Sub
        ProgressBarControl1.Position = 0
        Dim UWRI As New SqlCommand("Update TbEleCf set EleCfP = ElecfP + 2 where EleCfPriId=@F1", cnCo)
        Dim p1 As New SqlParameter("@F1", SqlDbType.Int)
        ProgressBarControl1.Properties.Maximum = GridView50.SelectedRowsCount
        ProgressBarControl1.Properties.PercentView = True
        For i As Int16 = 1 To GridView50.SelectedRowsCount
            RwMin = GridView50.GetDataRow(OpzControl(i - 1))
            p1.Value = RwMin("EleCfPriId")
            UWRI.Parameters.Clear()
            UWRI.Parameters.Add(p1)
            UWRI.ExecuteNonQuery()
            ProgressBarControl1.EditValue = i
            ProgressBarControl1.Update()
        Next
        IPagina()
        ProgressBarControl1.Position = 0
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim Land As Boolean = False
        If DsMin.Rows.Count = 0 Then Exit Sub
        DXANTEPRIMA(GridControl50, Land, Printing.PaperKind.A4, "IMPORTI < 3.000 - " & GroupControl1.Text)
    End Sub

#End Region
#Region " 2a GRID >= 3000 euro"
    Sub IIPagina()
        StrR = "Select * from VEleda2010 where EleCfTipo = '" & RadioGroup1.EditValue & "' and EleCfAnno = " & ANNO & " and EleCfP = 1 order by EleCfAnaDesc,EleCfDataDoc,EleCfNumDoc"
        DsMax = New DataTable
        DaMax = New SqlDataAdapter(StrR, cnCo)
        DaMax.SelectCommand.CommandTimeout = 300
        DaMax.Fill(DsMax)
        GridControl51.DataSource = DsMax
        GridView51.ClearSelection()
        GridView51.ExpandAllGroups()
    End Sub
    Private Sub ButtonP2A_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonP2A.Click
        AssociaDaPaginaII()
    End Sub
    Sub AssociaDaPaginaII()
        OpzControl = GridView51.GetSelectedRows
        If OpzControl.Length <= 0 Then Exit Sub
        ProgressBarControl2.Position = 0
        Dim UWRI As New SqlCommand("Update TbEleCf set EleCfP = ElecfP + 2 where EleCfPriId=@F1 ", cnCo)
        Dim p1 As New SqlParameter("@F1", SqlDbType.Int)
        ProgressBarControl2.Properties.Maximum = GridView51.SelectedRowsCount
        ProgressBarControl2.Properties.PercentView = True
        For i As Int16 = 1 To GridView51.SelectedRowsCount
            RwMin = GridView51.GetDataRow(OpzControl(i - 1))
            p1.Value = RwMin("EleCfPriId")
            UWRI.Parameters.Clear()
            UWRI.Parameters.Add(p1)
            UWRI.ExecuteNonQuery()
            ProgressBarControl2.EditValue = i
            ProgressBarControl2.Update()
        Next
        IIPagina()
        ProgressBarControl2.Position = 0
    End Sub
    Private Sub ButtonF9B_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9B.Click
        Dim Land As Boolean = False
        If DsMin.Rows.Count = 0 Then Exit Sub
        DXANTEPRIMA(GridControl51, Land, Printing.PaperKind.A4, "IMPORTI >= 3.000 - " & GroupControl3.Text)
    End Sub
#End Region
#Region " 3a GRID elenchi"
    Sub IIIPagina()
        StrR = "Select * from VEleda2010 where EleCfTipo = '" & RadioGroup1.EditValue & "' and EleCfAnno = " & ANNO & " and EleCfP > 1 order by EleCfAnaDesc,EleCfUnion,EleCfDataDoc,EleCfNumDoc"
        DsEle = New DataTable
        DaEle = New SqlDataAdapter(StrR, cnCo)
        DaEle.SelectCommand.CommandTimeout = 300
        DaEle.Fill(DsEle)
        GridControl52.DataSource = DsEle
        GridView52.ClearSelection()
        GridView52.ExpandAllGroups()
    End Sub
    Private Sub ButtonP3I_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonP3I.Click
        DissociaDaPaginaIII()
    End Sub
    Sub DissociaDaPaginaIII()
        OpzControl = GridView52.GetSelectedRows
        If OpzControl.Length <= 0 Then Exit Sub
        ProgressBarControl3.Position = 0
        Dim UWRI As New SqlCommand("Update TbEleCf set EleCfP = ElecfP - 2,EleCfUnion = 0 where EleCfPriId=@F1 ", cnCo)
        Dim p1 As New SqlParameter("@F1", SqlDbType.Int)
        ProgressBarControl3.Properties.Maximum = GridView52.SelectedRowsCount
        ProgressBarControl3.Properties.PercentView = True
        For i As Int16 = 1 To GridView52.SelectedRowsCount
            RwMin = GridView52.GetDataRow(OpzControl(i - 1))
            p1.Value = RwMin("EleCfPriId")
            UWRI.Parameters.Clear()
            UWRI.Parameters.Add(p1)
            UWRI.ExecuteNonQuery()
            ProgressBarControl3.EditValue = i
            ProgressBarControl3.Update()
        Next
        IIIPagina()
        ProgressBarControl3.Position = 0
    End Sub
    Private Sub ButtonF9C_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9C.Click
        Dim Land As Boolean = True
        If DsMin.Rows.Count = 0 Then Exit Sub
        DXANTEPRIMA(GridControl52, Land, Printing.PaperKind.A4, "FATTURE IN ELENCO - " & GroupControl7.Text)
    End Sub
#End Region
#Region " 4a GRID Dati Integrativi Esteri"
    Sub IVPagina()
        StrR = "Exec XEleDie @ANNO=" & ANNO
        DsDai = New DataTable
        DaDai = New SqlDataAdapter(StrR, cnCo)
        DaDai.SelectCommand.CommandTimeout = 300
        DaDai.Fill(DsDai)
        GridControl1.DataSource = DsDai
        AdvBandedGridView1.ClearSelection()
        GroupControl6.Text = ""
    End Sub
    Sub AggiornaDatiEsteri()
        Dim str As String = "Update TbEPoE set  DiCognome=@F4, DiNome=@F5, DiDataNasc=@F6, DiComune=@F7, DiProv=@F8, DiStato=@F9, DiDenomina=@F10,DiECitta=@F11,DiEIndiri=@F12,DiEStato=@F13 where DiCfAnno=@F1 AND DiCfTipo=@F2 AND DiCfCodice=@F3 "
        Dim p1 As New SqlParameter("@F1", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@F2", SqlDbType.NVarChar)
        Dim p3 As New SqlParameter("@F3", SqlDbType.NVarChar)
        Dim p4 As New SqlParameter("@F4", SqlDbType.NVarChar)
        Dim p5 As New SqlParameter("@F5", SqlDbType.NVarChar)
        Dim p6 As New SqlParameter("@F6", SqlDbType.SmallDateTime)
        Dim p7 As New SqlParameter("@F7", SqlDbType.NVarChar)
        Dim p8 As New SqlParameter("@F8", SqlDbType.NVarChar)
        Dim p9 As New SqlParameter("@F9", SqlDbType.NVarChar)
        Dim p10 As New SqlParameter("@F10", SqlDbType.NVarChar)
        Dim p11 As New SqlParameter("@F11", SqlDbType.NVarChar)
        Dim p12 As New SqlParameter("@F12", SqlDbType.NVarChar)
        Dim p13 As New SqlParameter("@F13", SqlDbType.NVarChar)
        Dim ChangeTable As DataTable = DsDai.GetChanges(DataRowState.Modified)
        If ChangeTable Is Nothing Then Exit Sub
        For x As Int16 = 1 To ChangeTable.Rows.Count
            RwDai = ChangeTable.Rows(x - 1)
            p1.Value = RwDai("EleCfAnno")
            p2.Value = RwDai("EleCfTipo")
            p3.Value = RwDai("EleCfCodice")
            p4.Value = RwDai("DiCognome")
            p5.Value = RwDai("DiNome")
            p6.Value = RwDai("DiDataNasc")
            p7.Value = RwDai("DiComune")
            p8.Value = RwDai("DiProv")
            p9.Value = RwDai("DiStato")
            p10.Value = RwDai("DiDenomina")
            p11.Value = RwDai("DiECitta")
            p12.Value = RwDai("DiEIndiri")
            p13.Value = RwDai("DiEStato")
            Cmd = New SqlCommand(str, cnCo)
            Cmd.Parameters.Add(p1)
            Cmd.Parameters.Add(p2)
            Cmd.Parameters.Add(p3)
            Cmd.Parameters.Add(p4)
            Cmd.Parameters.Add(p5)
            Cmd.Parameters.Add(p6)
            Cmd.Parameters.Add(p7)
            Cmd.Parameters.Add(p8)
            Cmd.Parameters.Add(p9)
            Cmd.Parameters.Add(p10)
            Cmd.Parameters.Add(p11)
            Cmd.Parameters.Add(p12)
            Cmd.Parameters.Add(p13)
            Cmd.ExecuteNonQuery()
            Cmd.Parameters.Clear()
        Next
        DsDai.AcceptChanges()
        PAGINVIO = False
    End Sub
    Private Sub AdvBandedGridView1_ShownEditor(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdvBandedGridView1.ShownEditor
        GroupControl6.Text = AdvBandedGridView1.FocusedColumn.ToolTip
    End Sub
#End Region
#Region " 5a GRID note variazione"
    Sub VPagina()
        StrR = "Exec XEleVar @ANNO=" & ANNO
        DsVar = New DataTable
        DaVar = New SqlDataAdapter(StrR, cnCo)
        DaVar.SelectCommand.CommandTimeout = 300
        DaVar.Fill(DsVar)
        GridControl2.DataSource = DsVar
        AdvBandedGridView2.ClearSelection()
        GroupControl9.Text = ""
    End Sub
    Sub AggiornaVariazioni()
        Dim str As String = "Update TbElVar set  VaCfDataDoc=@F4, VaCfNumDoc=@F5, VaCfImponibile=@F6, VaCfIva=@F7 where VaCfPriId=@F1"
        Dim p1 As New SqlParameter("@F1", SqlDbType.Int)
        Dim p4 As New SqlParameter("@F4", SqlDbType.SmallDateTime)
        Dim p5 As New SqlParameter("@F5", SqlDbType.Int)
        Dim p6 As New SqlParameter("@F6", SqlDbType.NVarChar)
        Dim p7 As New SqlParameter("@F7", SqlDbType.NVarChar)
        Dim ChangeTable As DataTable = DsVar.GetChanges(DataRowState.Modified)
        If ChangeTable Is Nothing Then Exit Sub
        For x As Int16 = 1 To ChangeTable.Rows.Count
            RwVar = ChangeTable.Rows(x - 1)
            p1.Value = RwVar("EleCfPriId")
            p4.Value = RwVar("VaCfDataDoc")
            p5.Value = RwVar("VaCfNumDoc")
            p6.Value = RwVar("VaCfImponibile")
            p7.Value = RwVar("VaCfIva")
            Cmd = New SqlCommand(str, cnCo)
            Cmd.Parameters.Add(p1)
            Cmd.Parameters.Add(p4)
            Cmd.Parameters.Add(p5)
            Cmd.Parameters.Add(p6)
            Cmd.Parameters.Add(p7)
            Cmd.ExecuteNonQuery()
            Cmd.Parameters.Clear()
        Next
        DsVar.AcceptChanges()
        PAGVAR = False
    End Sub
    Private Sub AdvBandedGridView2_ShownEditor(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdvBandedGridView2.ShownEditor
        GroupControl9.Text = AdvBandedGridView2.FocusedColumn.ToolTip
    End Sub
#End Region
#Region " 6a CREAZIONE FILE INVIO"
    Sub VIPagina()
        LeggiAnagraficaAzienda()
        NomeFiles()
    End Sub
    Sub LeggiAnagraficaAzienda()
        Dim Str As String = "Select  * from tbazi inner join vdox.dbo.tbana on anacod = azicod  where AziAnnoLavoro = " & ANNO
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read = True Then
            TextEdit2.EditValue = dataRd.Item("AnaCfis")
            TextEdit3.EditValue = dataRd.Item("AnaPiva")
            TextEdit4.EditValue = dataRd.Item("AziCognome")
            TextEdit5.EditValue = dataRd.Item("AziNome")
            TextEdit6.EditValue = dataRd.Item("AziSesso")
            DateEdit14.EditValue = dataRd.Item("AziDataNascita")
            TextEdit7.EditValue = dataRd.Item("AziComuneNascita")
            TextEdit8.EditValue = dataRd.Item("AziProvNascita")
            TextEdit9.EditValue = dataRd.Item("AnaDesc")
            TextEdit10.EditValue = dataRd.Item("AnaCitta")
            TextEdit11.EditValue = dataRd.Item("AnaProv")
        End If
        dataRd.Close()
        If TextEdit4.EditValue > "" Then
            TextEdit9.EditValue = ""
            TextEdit10.EditValue = ""
            TextEdit11.EditValue = ""
        Else
            TextEdit4.EditValue = ""
            TextEdit5.EditValue = ""
            TextEdit6.EditValue = ""
            TextEdit7.EditValue = ""
            TextEdit8.EditValue = ""
            DateEdit14.EditValue = DBNull.Value
        End If
    End Sub
    Private Sub NomeFiles()
        PathEle = "C:\ELECF" & ComboBoxEdit1.EditValue & "\"
        If Not Directory.Exists(PathEle) Then
            Directory.CreateDirectory(PathEle)
        End If
        FileEle = PathEle & TextEdit2.EditValue & "_SPES2011.txt"

        If File.Exists(Trim(FileEle)) Then
            File.Delete(Trim(FileEle))
        End If
        TextEdit1.EditValue = FileEle
    End Sub
    Sub AggiornaCreazioneFiles()
        PAGFILES = False
    End Sub
#End Region
#Region "GESTIONE FONDO PAGINA"
    Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        If PAGINVIO = True Then AggiornaDatiEsteri()
        If PAGVAR = True Then AggiornaVariazioni()
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            IPagina()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            IIPagina()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 2 Then
            IIIPagina()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 3 Then
            IVPagina() : PAGINVIO = True
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 4 Then
            VPagina() : PAGVAR = True
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 5 Then
            VIPagina() : PAGFILES = True
            Exit Sub
        End If
    End Sub
#End Region
#Region "CHIUDO ed Esco"
    Private Sub DxElenIva_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        RegistraIndici()
        If PAGINVIO = True Then AggiornaDatiEsteri()
        If PAGVAR = True Then AggiornaVariazioni()
    End Sub
#End Region
#Region "UNISCI o DISSOCIA FATTURE"
    Private Sub ButtonUN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUN.Click
        UnionFatture()
    End Sub
    Sub UnionFatture()
        OpzControl = GridView52.GetSelectedRows
        If OpzControl.Length <= 0 Then Exit Sub
        Dim Dissocia As Boolean = False
        ProgressBarControl3.Position = 0
        Dim UWRI As New SqlCommand("Update TbEleCf set EleCfUnion = 0 where EleCfPriId=@F1 ", cnCo)
        Dim p1 As New SqlParameter("@F1", SqlDbType.Int)
        ProgressBarControl3.Properties.Maximum = GridView52.SelectedRowsCount * 2
        ProgressBarControl3.Properties.PercentView = True
        For i As Int16 = 1 To GridView52.SelectedRowsCount
            RwMin = GridView52.GetDataRow(OpzControl(i - 1))
            If RwMin("EleCfUnion") > 0 Then Dissocia = True
            p1.Value = RwMin("EleCfPriId")
            UWRI.Parameters.Clear()
            UWRI.Parameters.Add(p1)
            UWRI.ExecuteNonQuery()
            ProgressBarControl3.EditValue = i
            ProgressBarControl3.Update()
        Next
        If Dissocia = True Then GoTo IIUNION
        Cmd = New SqlCommand("Select Max(EleCfUnion) from TbEleCf", cnCo)
        Dim UUMax As Integer = Cmd.ExecuteScalar + 1
        Dim u1 As New SqlParameter("@F1", SqlDbType.Int)
        Dim UURI As New SqlCommand("Update TbEleCf set EleCfUnion = " & UUMax & " where EleCfPriId=@F1 ", cnCo)
        For i As Int16 = 1 To GridView52.SelectedRowsCount
            RwMin = GridView52.GetDataRow(OpzControl(i - 1))
            u1.Value = RwMin("EleCfPriId")
            UURI.Parameters.Clear()
            UURI.Parameters.Add(u1)
            UURI.ExecuteNonQuery()
            ProgressBarControl3.EditValue += 1
            ProgressBarControl3.Update()
        Next
IIUNION:
        IIIPagina()
        ProgressBarControl3.Position = 0
    End Sub
#End Region
#Region "AGGIORNA VARIAZIONE e MODALITA' PAGAMENTO"
    Private Sub GridView52_RowUpdated(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowObjectEventArgs) Handles GridView52.RowUpdated
        RwMin = GridView52.GetDataRow(e.RowHandle)
        Dim UWRI As New SqlCommand("Update TbEleCf set EleCfModPag = @F2,EleCfVariazione =@F3 where EleCfPriId=@F1 ", cnCo)
        Dim p1 As New SqlParameter("@F1", SqlDbType.Int)
        Dim p2 As New SqlParameter("@F2", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@F3", SqlDbType.Bit)
        p1.Value = RwMin("EleCfPriId")
        p2.Value = RwMin("EleCfModPag")
        p3.Value = RwMin("EleCfVariazione")
        UWRI.Parameters.Clear()
        UWRI.Parameters.Add(p1)
        UWRI.Parameters.Add(p2)
        UWRI.Parameters.Add(p3)
        UWRI.ExecuteNonQuery()
        RwMin.AcceptChanges()
        DsMin.AcceptChanges()
        GridView52.ClearSelection()
    End Sub
    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        If ComboBoxEdit2.SelectedIndex < 0 Then Exit Sub
        AssociaPagamento()
    End Sub

    Sub AssociaPagamento()
        OpzControl = GridView52.GetSelectedRows
        If OpzControl.Length <= 0 Then Exit Sub

        Dim UWRI As New SqlCommand("Update TbEleCf set EleCfModPag = @F2 where EleCfPriId=@F1", cnCo)
        Dim p1 As New SqlParameter("@F1", SqlDbType.Int)
        Dim p2 As New SqlParameter("@F2", SqlDbType.VarChar)
        ProgressBarControl3.Properties.Maximum = GridView52.SelectedRowsCount
        ProgressBarControl3.Properties.PercentView = True
        For i As Int16 = 1 To GridView52.SelectedRowsCount
            RwMin = GridView52.GetDataRow(OpzControl(i - 1))

            p1.Value = RwMin("EleCfPriId")
            p2.Value = Mid(ComboBoxEdit2.EditValue, 1, 1)

            UWRI.Parameters.Clear()
            UWRI.Parameters.Add(p1)
            UWRI.Parameters.Add(p2)
            UWRI.ExecuteNonQuery()
            RwMin.AcceptChanges()
            DsMin.AcceptChanges()
            GridView52.ClearSelection()
        Next
        IIIPagina()
        ProgressBarControl3.Position = 0
    End Sub
#End Region
#Region "BLOCCO ELENCO --- DA COMPLETARE"
    Private Sub CheckButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckButton2.CheckedChanged
        If CheckButton2.Checked = False AndAlso AbilitaElenchi() = True Then
            CheckButton2.ImageIndex = 19 : CheckButton2.ToolTip = "ELENCO APERTO" : LockButton(False)
        Else
            CheckButton2.ImageIndex = 18 : CheckButton2.ToolTip = "ELENCO CHIUSO" : LockButton(True)
        End If
    End Sub
    Function AbilitaElenchi() As Boolean
        Dim P As New DxPwdDialog
        DxPwdDialog.Password = "SPE2011"
        P.ShowDialog()
        Return DxPwdDialog.Esatta
    End Function
    Sub LockButton(ByVal n As Boolean)
        ButtonF11.Enabled = Not n
        ButtonP1A.Enabled = Not n
        ButtonP2A.Enabled = Not n
        ButtonP3I.Enabled = Not n
        ButtonF3.Enabled = Not n
        SimpleButton2.Enabled = Not n
        ComboBoxEdit2.Enabled = Not n
        ButtonUN.Enabled = Not n
        CheckButton2.Checked = n
        Bloccato = n
        GridColumn47.OptionsColumn.AllowEdit = Not n
        GridColumn47.OptionsColumn.AllowFocus = Not n
        GridColumn47.OptionsColumn.ReadOnly = n
        GridColumn46.OptionsColumn.AllowEdit = Not n
        GridColumn46.OptionsColumn.AllowFocus = Not n
        GridColumn46.OptionsColumn.ReadOnly = n

        BandedGridColumn14.OptionsColumn.ReadOnly = Not n
        BandedGridColumn14.OptionsColumn.AllowFocus = Not n
        BandedGridColumn14.OptionsColumn.ReadOnly = n
        BandedGridColumn15.OptionsColumn.ReadOnly = Not n
        BandedGridColumn15.OptionsColumn.AllowFocus = Not n
        BandedGridColumn15.OptionsColumn.ReadOnly = n
        BandedGridColumn16.OptionsColumn.ReadOnly = Not n
        BandedGridColumn16.OptionsColumn.AllowFocus = Not n
        BandedGridColumn16.OptionsColumn.ReadOnly = n
        BandedGridColumn17.OptionsColumn.ReadOnly = Not n
        BandedGridColumn17.OptionsColumn.AllowFocus = Not n
        BandedGridColumn17.OptionsColumn.ReadOnly = n
        BandedGridColumn18.OptionsColumn.ReadOnly = Not n
        BandedGridColumn18.OptionsColumn.AllowFocus = Not n
        BandedGridColumn18.OptionsColumn.ReadOnly = n
        BandedGridColumn19.OptionsColumn.ReadOnly = Not n
        BandedGridColumn19.OptionsColumn.AllowFocus = Not n
        BandedGridColumn19.OptionsColumn.ReadOnly = n
        BandedGridColumn20.OptionsColumn.ReadOnly = Not n
        BandedGridColumn20.OptionsColumn.AllowFocus = Not n
        BandedGridColumn20.OptionsColumn.ReadOnly = n
        BandedGridColumn21.OptionsColumn.ReadOnly = Not n
        BandedGridColumn21.OptionsColumn.AllowFocus = Not n
        BandedGridColumn21.OptionsColumn.ReadOnly = n
        BandedGridColumn22.OptionsColumn.ReadOnly = Not n
        BandedGridColumn22.OptionsColumn.AllowFocus = Not n
        BandedGridColumn22.OptionsColumn.ReadOnly = n
        BandedGridColumn23.OptionsColumn.ReadOnly = Not n
        BandedGridColumn23.OptionsColumn.AllowFocus = Not n
        BandedGridColumn23.OptionsColumn.ReadOnly = n
        BandedGridColumn24.OptionsColumn.ReadOnly = Not n
        BandedGridColumn24.OptionsColumn.AllowFocus = Not n
        BandedGridColumn24.OptionsColumn.ReadOnly = n
        BandedGridColumn25.OptionsColumn.ReadOnly = Not n
        BandedGridColumn25.OptionsColumn.AllowFocus = Not n
        BandedGridColumn25.OptionsColumn.ReadOnly = n
        BandedGridColumn26.OptionsColumn.ReadOnly = Not n
        BandedGridColumn26.OptionsColumn.AllowFocus = Not n
        BandedGridColumn26.OptionsColumn.ReadOnly = n

        BandedGridColumn27.OptionsColumn.ReadOnly = Not n
        BandedGridColumn27.OptionsColumn.AllowFocus = Not n
        BandedGridColumn27.OptionsColumn.ReadOnly = n
        BandedGridColumn28.OptionsColumn.ReadOnly = Not n
        BandedGridColumn28.OptionsColumn.AllowFocus = Not n
        BandedGridColumn28.OptionsColumn.ReadOnly = n
        BandedGridColumn30.OptionsColumn.ReadOnly = Not n
        BandedGridColumn30.OptionsColumn.AllowFocus = Not n
        BandedGridColumn30.OptionsColumn.ReadOnly = n
        BandedGridColumn31.OptionsColumn.ReadOnly = Not n
        BandedGridColumn31.OptionsColumn.AllowFocus = Not n
        BandedGridColumn31.OptionsColumn.ReadOnly = n
        BandedGridColumn32.OptionsColumn.ReadOnly = Not n
        BandedGridColumn32.OptionsColumn.AllowFocus = Not n
        BandedGridColumn32.OptionsColumn.ReadOnly = n
        BandedGridColumn33.OptionsColumn.ReadOnly = Not n
        BandedGridColumn33.OptionsColumn.AllowFocus = Not n
        BandedGridColumn33.OptionsColumn.ReadOnly = n

        GridColumn50.OptionsColumn.AllowEdit = Not n
        GridColumn50.OptionsColumn.AllowFocus = Not n
        GridColumn50.OptionsColumn.ReadOnly = n
        GridColumn51.OptionsColumn.AllowEdit = Not n
        GridColumn51.OptionsColumn.AllowFocus = Not n
        GridColumn51.OptionsColumn.ReadOnly = n
        GridColumn54.OptionsColumn.AllowEdit = Not n
        GridColumn54.OptionsColumn.AllowFocus = Not n
        GridColumn54.OptionsColumn.ReadOnly = n
        GridColumn55.OptionsColumn.AllowEdit = Not n
        GridColumn55.OptionsColumn.AllowFocus = Not n
        GridColumn55.OptionsColumn.ReadOnly = n
        GridColumn56.OptionsColumn.AllowEdit = Not n
        GridColumn56.OptionsColumn.AllowFocus = Not n
        GridColumn56.OptionsColumn.ReadOnly = n
        GridColumn57.OptionsColumn.AllowEdit = Not n
        GridColumn57.OptionsColumn.AllowFocus = Not n
        GridColumn57.OptionsColumn.ReadOnly = n
        GridColumn60.OptionsColumn.AllowEdit = Not n
        GridColumn60.OptionsColumn.AllowFocus = Not n
        GridColumn60.OptionsColumn.ReadOnly = n


    End Sub
#End Region
#Region "CREAZIONE FILE ELENCHI"
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        'If ANNO = 2011 And ControlloLimite() = False Then Exit Sub
        If Controlli() = False Then Exit Sub
        Cursor.Current = Cursors.WaitCursor
        NomeFiles()
        output = File.AppendText(Trim(FileEle))
        Dim riga As String = ele_t()
        Dim wrt As String = "0" & riga
        Dim wrc As String = "9" & riga
        output.WriteLine(wrt)
        WriteRecord(1)
        WriteRecord(2)
        WriteRecord(3)
        WriteRecord(4)
        WriteRecord(5)

        output.WriteLine(wrc)
        output.Close()
        Cursor.Current = Cursors.Default
        ButtonF8.Enabled = True
        ButtonF11.Enabled = False
        CheckButton2.Checked = True
        Bloccato = True
        ButtonF8.PerformClick()
    End Sub
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        StrR = "Select *,Riepilogo = cast(Cast(EleCfCodice as varchar(5)) + ' ' + EleCfAnaDesc + ' ' + cast(EleCfUnion as varchar(6)) as varchar(100))  from VEleda2010 where EleCfP > 1 AND EleCfAnno = " & ANNO & " order by EleCfAnaDesc,EleCfUnion,EleCfDataDoc,EleCfNumDoc"
        DsPrt = New DataTable
        DaPrt = New SqlDataAdapter(StrR, cnCo)
        DaPrt.SelectCommand.CommandTimeout = 300
        DaPrt.Fill(DsPrt)
        GridControl3.DataSource = DsPrt
        '''StrR = "Exec XEleDie @ANNO=" & ANNO
        '''DsDai = New DataTable
        '''DaDai = New SqlDataAdapter(StrR, cnCo)
        '''DaDai.SelectCommand.CommandTimeout = 300
        '''DaDai.Fill(DsDai)
        '''GridControl1.DataSource = DsDai
        '''AdvBandedGridView1.ClearSelection()
        '''GroupControl6.Text = ""
        IVPagina()
        VPagina()
        DXANTEPRIMA(GridControl3, True, Printing.PaperKind.A4, "STAMPA di CONTROLLO FILE " & FileEle, , , , , GridControl1, , GridControl2)
    End Sub
    Function ControlloLimite() As Boolean
        ControlloLimite = True
        Dim Cmd As New SqlCommand("Select count(*) from VVERIFICASPE2011 where ERRORE > ''", cnCo)
        Dim Errori As Integer = Cmd.ExecuteScalar
        If Errori > 0 Then
            Messaggio(1, "** ATTENZIONE ERRORE BLOCCANTE PER TRASMISSIONE ** - FATTURE IN ELENCO CON IMPORTO INFERIORE A € 3.000 E PAGAMENTO NON FRAZIONATO!!!")
            ControlloLimite = False
        End If
    End Function
    Function Controlli() As Boolean
        Controlli = True
        If TextEdit2.EditValue = "" Then
            Messaggio(1, "MANCA CODICE FISCALE DEL CONTRIBUENTE")
            Controlli = False
        End If
        If TextEdit3.EditValue = "" Then
            Messaggio(1, "MANCA PARTITA IVA DEL CONTRIBUENTE")
            Controlli = False
        End If
        If TextEdit4.EditValue = "" And TextEdit9.EditValue = "" Then
            Messaggio(1, "DATI PERSONA FISICA O NON FISICA ASSENTI")
            Controlli = False
        End If
        If TextEdit12.EditValue > "" And (DateEdit1.EditValue Is Nothing Or DateEdit1.EditValue Is DBNull.Value) Then
            Messaggio(1, "INSERIRE DATA IMPEGNO TRASMISSIONE")
            Controlli = False
        End If
    End Function
    Private Function ele_t() As String
        'RECORD TESTATA CONTRIBUENTE
        ele_t = "ART2147"
        'INVIO ORDINARIO
        ele_t &= "0"
        ele_t &= "".PadLeft(23, "0")
        ' ele_t &= " ".PadLeft(23, "")
        ele_t &= CStr(TextEdit2.EditValue).PadRight(16)
        ele_t &= CStr(TextEdit3.EditValue).PadRight(11)
        'PERSONA NON FISICA
        ele_t &= CStr(TextEdit9.EditValue).PadRight(60)
        ele_t &= CStr(TextEdit10.EditValue).PadRight(40)
        ele_t &= CStr(TextEdit11.EditValue).PadRight(2)

        'PERSONA FISICA
        ele_t &= CStr(TextEdit4.EditValue).PadRight(24)
        ele_t &= CStr(TextEdit5.EditValue).PadRight(20)
        ele_t &= CStr(TextEdit6.EditValue).PadRight(1)
        If (DateEdit14.EditValue Is Nothing Or DateEdit14.EditValue Is DBNull.Value) Then
            ele_t &= "00000000"
        Else
            ele_t &= CDate(DateEdit14.EditValue).Day.ToString.PadLeft(2, "0")
            ele_t &= CDate(DateEdit14.EditValue).Month.ToString.PadLeft(2, "0")
            ele_t &= CDate(DateEdit14.EditValue).Year.ToString.PadLeft(4, "0")
        End If
        ele_t &= CStr(TextEdit7.EditValue).PadRight(40)
        ele_t &= CStr(TextEdit8.EditValue).PadRight(2)
        ele_t &= ANNO
        ele_t &= "0"
        ele_t &= "0001"
        ele_t &= "0001"
        ele_t &= CStr(TextEdit12.EditValue).PadRight(16)
        ele_t &= CStr(TextEdit13.EditValue).PadLeft(5, "0")
        If TextEdit12.EditValue > "" Then
            ele_t &= "1"
        Else
            ele_t &= "0"
        End If
        If (DateEdit1.EditValue Is Nothing Or DateEdit1.EditValue Is DBNull.Value) Then
            ele_t &= "00000000"
        Else
            ele_t &= CDate(DateEdit1.EditValue).Day.ToString.PadLeft(2, "0")
            ele_t &= CDate(DateEdit1.EditValue).Month.ToString.PadLeft(2, "0")
            ele_t &= CDate(DateEdit1.EditValue).Year.ToString.PadLeft(4, "0")
        End If
        ele_t &= "".PadRight(1498)
        ele_t &= "A"
        ' ele_t &= "0D0A"
    End Function
    Sub WriteRecord(ByVal n)
        StrR = "Exec XELEFILE @T=" & n & ",@ANNO=" & ANNO
        DsWri = New DataTable
        DaWri = New SqlDataAdapter(StrR, cnCo)
        DaWri.SelectCommand.CommandTimeout = 300
        DaWri.Fill(DsWri)
        Dim wr As String = ""
        Select Case n
            Case 1
                For x As Int16 = 1 To DsWri.Rows.Count
                    RwWri = DsWri.Rows(x - 1)
                    wr = n
                    wr &= RwWri("AnaCfis").ToString.PadRight(16, " ")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Day.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Month.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Year.ToString.PadLeft(4, "0")
                    wr &= RwWri("EleCfModPag")
                    wr &= RwWri("Totale").ToString.PadLeft(9, "0")
                    wr &= "".PadRight(1762)
                    wr &= "A"
                    output.WriteLine(wr)
                Next
            Case 2
                For x As Int16 = 1 To DsWri.Rows.Count
                    RwWri = DsWri.Rows(x - 1)
                    wr = n
                    wr &= RwWri("AnaPiva").ToString.PadRight(11, " ")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Day.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Month.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Year.ToString.PadLeft(4, "0")
                    wr &= RwWri("EleCfNumDoc").ToString.PadRight(15, " ")
                    wr &= RwWri("EleCfModPag")
                    wr &= RwWri("EleCfImponibile").ToString.PadLeft(9, "0")
                    wr &= RwWri("EleCfIva").ToString.PadLeft(9, "0")
                    wr &= RwWri("Op")
                    wr &= "".PadRight(1742)
                    wr &= "A"
                    output.WriteLine(wr)
                Next
            Case 3
                For x As Int16 = 1 To DsWri.Rows.Count
                    RwWri = DsWri.Rows(x - 1)
                    wr = n
                    wr &= RwWri("DiCognome").ToString.PadRight(24, " ")
                    wr &= RwWri("DiNome").ToString.PadRight(20, " ")
                    If RwWri("DiDataNasc") Is DBNull.Value Then
                        wr &= "00000000"
                    Else
                        wr &= CDate(RwWri("DiDataNasc")).Day.ToString.PadLeft(2, "0")
                        wr &= CDate(RwWri("DiDataNasc")).Month.ToString.PadLeft(2, "0")
                        wr &= CDate(RwWri("DiDataNasc")).Year.ToString.PadLeft(4, "0")
                    End If
                    wr &= RwWri("DiComune").ToString.PadRight(40, " ")
                    wr &= RwWri("DiProv").ToString.PadRight(2, " ")
                    wr &= RwWri("DiStato").ToString.PadRight(3, " ")
                    wr &= RwWri("DiDenomina").ToString.PadRight(60, " ")
                    wr &= RwWri("DiECitta").ToString.PadRight(40, " ")
                    wr &= RwWri("DiEStato").ToString.PadRight(3, " ")
                    wr &= RwWri("DiEIndiri").ToString.PadRight(40, " ")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Day.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Month.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Year.ToString.PadLeft(4, "0")
                    wr &= RwWri("EleCfNumDoc").ToString.PadRight(15, " ")
                    wr &= RwWri("EleCfModPag")
                    wr &= RwWri("EleCfImponibile").ToString.PadLeft(9, "0")
                    wr &= RwWri("EleCfIva").ToString.PadLeft(9, "0")
                    wr &= RwWri("Op")
                    wr &= "".PadRight(1513)
                    wr &= "A"
                    output.WriteLine(wr)
                Next
            Case 4
                For x As Int16 = 1 To DsWri.Rows.Count
                    RwWri = DsWri.Rows(x - 1)
                    wr = n
                    wr &= RwWri("AnaPiva").ToString.PadRight(11, " ")
                    wr &= RwWri("AnaCfis").ToString.PadRight(16, " ")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Day.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Month.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Year.ToString.PadLeft(4, "0")
                    wr &= RwWri("EleCfNumDoc").ToString.PadRight(15, " ")
                    wr &= RwWri("EleCfImponibile").ToString.PadLeft(9, "0")
                    wr &= RwWri("EleCfIva").ToString.PadLeft(9, "0")
                    wr &= CDate(RwWri("VaCfDataDoc")).Day.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("VaCfDataDoc")).Month.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("VaCfDataDoc")).Year.ToString.PadLeft(4, "0")
                    wr &= RwWri("VaCfNumDoc").ToString.PadRight(15, " ")
                    wr &= RwWri("VaCfImponibile")
                    wr &= RwWri("VaCfIva")
                    wr &= "".PadRight(1703)
                    wr &= "A"
                    output.WriteLine(wr)
                Next
            Case 5
                For x As Int16 = 1 To DsWri.Rows.Count
                    RwWri = DsWri.Rows(x - 1)
                    wr = n
                    wr &= RwWri("DiCognome").ToString.PadRight(24, " ")
                    wr &= RwWri("DiNome").ToString.PadRight(20, " ")
                    If RwWri("DiDataNasc") Is DBNull.Value Then
                        wr &= "00000000"
                    Else
                        wr &= CDate(RwWri("DiDataNasc")).Day.ToString.PadLeft(2, "0")
                        wr &= CDate(RwWri("DiDataNasc")).Month.ToString.PadLeft(2, "0")
                        wr &= CDate(RwWri("DiDataNasc")).Year.ToString.PadLeft(4, "0")
                    End If
                    wr &= RwWri("DiComune").ToString.PadRight(40, " ")
                    wr &= RwWri("DiProv").ToString.PadRight(2, " ")
                    wr &= RwWri("DiStato").ToString.PadRight(3, " ")
                    wr &= RwWri("DiDenomina").ToString.PadRight(60, " ")
                    wr &= RwWri("DiECitta").ToString.PadRight(40, " ")
                    wr &= RwWri("DiEStato").ToString.PadRight(3, " ")
                    wr &= RwWri("DiEIndiri").ToString.PadRight(40, " ")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Day.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Month.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("EleCfRegistrazione")).Year.ToString.PadLeft(4, "0")
                    wr &= RwWri("EleCfNumDoc").ToString.PadRight(15, " ")
                    wr &= RwWri("EleCfImponibile").ToString.PadLeft(9, "0")
                    wr &= RwWri("EleCfIva").ToString.PadLeft(9, "0")
                    wr &= CDate(RwWri("VaCfDataDoc")).Day.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("VaCfDataDoc")).Month.ToString.PadLeft(2, "0")
                    wr &= CDate(RwWri("VaCfDataDoc")).Year.ToString.PadLeft(4, "0")
                    wr &= RwWri("VaCfNumDoc").ToString.PadRight(15, " ")
                    wr &= RwWri("VaCfImponibile")
                    wr &= RwWri("VaCfIva")
                    wr &= "".PadRight(1490)
                    wr &= "A"
                    output.WriteLine(wr)
                Next
        End Select
    End Sub
#End Region
End Class