Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO



Public Class DxSp2016
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
    Dim ANNO As Int16 = 2016
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

    Private Structure RecC
        Dim c1 As String
        Dim c2 As String
        Dim c3 As String
        Dim c4 As String
        Dim c5 As String
        Dim c6 As String
    End Structure
    Private RecordC As RecC

    Dim AC(5) As String


    Private Sub DxSp2016_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        SetInizio()
    End Sub
    Sub SetInizio()
        ComboBoxEdit1.Properties.Items.Clear()
        ComboBoxEdit1.Properties.Items.Add(ANNO)
        ComboBoxEdit1.SelectedIndex = 0
        XtraTabControl1.SelectedTabPageIndex = 0
        Bottoni(False)
        Pulizia()
        RadioGroup1.EditValue = PrimaLettura()
        If RadioGroup1.EditValue > "" Then Bottoni(True) : LockButton(Bloccato)
    End Sub
    Sub Bottoni(ByVal N As Boolean)
        ButtonF1.Enabled = Not N
        ButtonF3.Enabled = N
        XtraTabPage2.PageVisible = N
        XtraTabPage3.PageVisible = N
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
            Cmd = New SqlCommand("delete from TbRieCf where RieCfAnno =" & ANNO, cnCo)
            Cmd.ExecuteNonQuery()
            Cmd = New SqlCommand("delete from TbInEle where IeAnno =" & ANNO, cnCo)
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

#Region "LETTURA FATTURE e 1a+ 2a GRID GLOBALI  "
    Sub Pulizia()
        DsMin = New DataTable : DsMax = New DataTable : DsEle = New DataTable
        GridControl50.DataSource = DsMin : GridControl51.DataSource = DsMax
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Controllo() = False Then Exit Sub
        Pulizia()
        Cursor.Current = Cursors.WaitCursor
        EsegueSql("Exec XSPESEM2012 @ANNO= " & ANNO, cnCo)
        IPaginaeII()
        Bottoni(True)
    End Sub

    Sub IPaginaeII()
        If RadioGroup1.EditValue = "CL" Then GroupControl1.Text = "CLIENTI " Else GroupControl1.Text = "FORNITORI"
        GroupControl3.Text = GroupControl1.Text
        GroupControl7.Text = GroupControl1.Text
        DsMin = New DataTable
        DaMin = New SqlDataAdapter("Select * from TbRieCf where RieCfTipo = '" & RadioGroup1.EditValue & "' and RieCfAnno = " & ANNO & " order by RieCfAnadesc", cnCo)
        DaMin.SelectCommand.CommandTimeout = 300
        DaMin.Fill(DsMin)
        GridControl50.DataSource = DsMin
        GridView50.ClearSelection()
        DsMax = New DataTable
        DaMax = New SqlDataAdapter("Select * from VEleDa2010 where EleCfTipo = '" & RadioGroup1.EditValue & "' and EleCfAnno = " & ANNO & " and EleCfP = 0 order by EleCfAnaDesc,EleCfDataDoc,EleCfNumDoc", cnCo)
        DaMax.SelectCommand.CommandTimeout = 300
        DaMax.Fill(DsMax)
        GridControl51.DataSource = DsMax
        GridView51.ClearSelection()
        GridView51.ExpandAllGroups()
        LockButton(Bloccato)
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex > -1 Then
            If ButtonF1.Enabled = False Then IPaginaeII() Else ButtonF1.PerformClick()
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
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If DsMin.Rows.Count = 0 Then Exit Sub
        DXANTEPRIMA(GridControl50, True, Printing.PaperKind.A4, "RIEPILOGO FATTURE " & GroupControl1.Text)
    End Sub
    Private Sub RepositoryItemCheckEdit2_CheckedChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemCheckEdit2.CheckedChanged
        Dim T As Boolean = DirectCast(sender, DevExpress.XtraEditors.CheckEdit).Checked
        InsCheckRiepilogo(T)
    End Sub
    Sub InsCheckRiepilogo(ByVal T As Boolean)
        OpzControl = GridView50.GetSelectedRows
        If OpzControl.Length <= 0 Then Exit Sub
        RwMax = GridView50.GetDataRow(OpzControl(0))
        Dim Dissocia As Int16 = 0
        If T = True Then Dissocia = 2
        Dim UWEL As New SqlCommand("Update TbEleCf set EleCfP = @F1 where EleCfCodice=@F0 and EleCfAnno =" & ANNO, cnCo)
        Dim UWRI As New SqlCommand("Update TbRieCf set RieCfEscludi = @Q1 where RieCfCodice=@Q0 and RieCfAnno =" & ANNO, cnCo)
        Dim p0 As New SqlParameter("@F0", SqlDbType.NVarChar)
        Dim p1 As New SqlParameter("@F1", SqlDbType.SmallInt)

        Dim q0 As New SqlParameter("@Q0", SqlDbType.NVarChar)
        Dim q1 As New SqlParameter("@Q1", SqlDbType.Bit)

        p0.Value = RwMax("RieCfCodice")
        p1.Value = Dissocia

        q0.Value = RwMax("RieCfCodice")
        q1.Value = T

        UWRI.Parameters.Clear()
        UWRI.Parameters.Add(q0)
        UWRI.Parameters.Add(q1)
        UWRI.ExecuteNonQuery()

        UWEL.Parameters.Clear()
        UWEL.Parameters.Add(p0)
        UWEL.Parameters.Add(p1)
        UWEL.ExecuteNonQuery()
        IPaginaeII()
    End Sub
#End Region
#Region " 2a GRID DETTAGLIO FATTURE"
    Private Sub ButtonF9B_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9B.Click
        If DsMin.Rows.Count = 0 Then Exit Sub
        DXANTEPRIMA(GridControl51, True, Printing.PaperKind.A4, "DETTAGLIO FATTURE " & GroupControl3.Text)
    End Sub
#End Region
#Region " 3a GRID elenchi"
    Sub IIIPagina()
        DsEle = New DataTable
        DaEle = New SqlDataAdapter("EXEC XELE2012 @ANNO=" & ANNO & ",@T=0", cnCo)
        DaEle.SelectCommand.CommandTimeout = 300
        DaEle.Fill(DsEle)
        GridControl52.DataSource = DsEle
        GridView52.ClearSelection()
        GridView52.ExpandAllGroups()
    End Sub
    Private Sub ButtonF9C_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9C.Click
        If DsMin.Rows.Count = 0 Then Exit Sub
        DXANTEPRIMA(GridControl52, True, Printing.PaperKind.A4, "CLIENTI/FORNITORI IN ELENCO")
    End Sub
#End Region
#Region " 4a GRID Dati Integrativi Esteri"
    Sub IVPagina()
        StrR = "Exec XEleDie2012 @ANNO=" & ANNO
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
            p1.Value = RwDai("RieCfAnno")
            p2.Value = RwDai("RieCfTipo")
            p3.Value = RwDai("RieCfCodice")
            p4.Value = IIf(RwDai("DiCognome") Is DBNull.Value, "", RwDai("DiCognome"))
            p5.Value = IIf(RwDai("DiNome") Is DBNull.Value, "", RwDai("DiNome"))
            p6.Value = RwDai("DiDataNasc")
            p7.Value = IIf(RwDai("DiComune") Is DBNull.Value, "", RwDai("DiComune"))
            p8.Value = IIf(RwDai("DiProv") Is DBNull.Value, "", RwDai("DiProv"))
            p9.Value = IIf(RwDai("DiStato") Is DBNull.Value, "", RwDai("DiStato"))
            p10.Value = IIf(RwDai("DiDenomina") Is DBNull.Value, "", RwDai("DiDenomina"))
            p11.Value = IIf(RwDai("DiECitta") Is DBNull.Value, "", RwDai("DiECitta"))
            p12.Value = IIf(RwDai("DiEIndiri") Is DBNull.Value, "", RwDai("DiEIndiri"))
            p13.Value = IIf(RwDai("DiEStato") Is DBNull.Value, "", RwDai("DiEStato"))
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
#Region " 6a CREAZIONE FILE INVIO"
    Sub VIPagina()
        LeggiAnagraficaAzienda()
        NomeFiles()
    End Sub
    Sub LeggiAnagraficaAzienda()
        For j As Int16 = 0 To 5 : AC(j) = "" : Next
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
            TextEdit14.EditValue = dataRd.Item("AnaTel1")
            TextEdit17.EditValue = dataRd.Item("AnaFax")
            TextEdit18.EditValue = dataRd.Item("AnaEmail")
            TextEdit16.EditValue = dataRd.Item("AziDrrCarica770")
            TextEdit15.EditValue = dataRd.Item("AziCodAteco")
            TextEdit15.EditValue = dataRd.Item("AziCodAteco")
            TextEdit19.EditValue = dataRd.Item("AziDrrCodiceFisc")
            AC(0) = dataRd.Item("AziDrrCognome")
            AC(1) = dataRd.Item("AziDrrNome")
            AC(2) = dataRd.Item("AziDrrSesso")
            AC(3) = IIf(dataRd.Item("AziDrrDataNascita") Is DBNull.Value, "", CDate(dataRd.Item("AziDrrDataNascita")).ToShortDateString)
            AC(4) = dataRd.Item("AziDrrComuneNascita")
            AC(5) = dataRd.Item("AziDrrProvNascita")
        End If
        dataRd.Close()
        Dim ss As String = TextEdit15.EditValue
        TextEdit15.EditValue = ""
        If ss > "" Then
            For j As Int16 = 1 To Len(ss)
                If Mid(ss, j, 1) <> "." Then TextEdit15.EditValue &= Mid(ss, j, 1)
            Next
        End If
        If TextEdit4.EditValue > "" Then
            TextEdit9.EditValue = ""
            TextEdit10.EditValue = ""
            TextEdit11.EditValue = ""
            TextEdit16.EditValue = ""
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
        FileEle = PathEle & TextEdit2.EditValue & "_SPES2016.txt"

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
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            IPaginaeII()
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
        DxPwdDialog.Password = "SPE2016"
        P.ShowDialog()
        Return DxPwdDialog.Esatta
    End Function
    Sub LockButton(ByVal n As Boolean)
        ButtonF11.Enabled = Not n
        ButtonF3.Enabled = Not n

        CheckButton2.Checked = n
        Bloccato = n
        GridColumn42.OptionsColumn.AllowEdit = Not n
        GridColumn42.OptionsColumn.AllowFocus = Not n
        GridColumn42.OptionsColumn.ReadOnly = n
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
    End Sub
#End Region
#Region "CREAZIONE FILE ELENCHI"
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Controlli() = False Then Exit Sub
        ControlloQuadri()
        Cursor.Current = Cursors.WaitCursor
        NomeFiles()
        output = File.CreateText(Trim(FileEle))
        Dim riga As String = ele_t("A")
        output.WriteLine(riga)
        riga = ele_t("B")
        output.WriteLine(riga)
        WriteRecord(1)
        riga = ele_t("E")
        output.WriteLine(riga)
        riga = ele_t("Z")
        output.WriteLine(riga)
        output.Close()
        Cursor.Current = Cursors.Default
        ButtonF11.Enabled = False
        CheckButton2.Checked = True
        Bloccato = True
        TextEdit1.Focus()
    End Sub
    Sub ControlloQuadri()
        REM lettura FA
        DsWri = New DataTable
        DaWri = New SqlDataAdapter("EXEC XELE2012 @ANNO=" & ANNO & ",@T=1", cnCo)
        DaWri.SelectCommand.CommandTimeout = 300
        DaWri.Fill(DsWri)
        REM lettura BL
        DsVar = New DataTable
        DaVar = New SqlDataAdapter("SELECT * FROM TBESTCF inner join TbEPoE ON EstCfCodice = DiCfCodice AND DiCfAnno = EstCFANNO  WHERE EstCfAnno =" & ANNO & " order by EstCfCodice", cnCo)
        DaVar.SelectCommand.CommandTimeout = 300
        DaVar.Fill(DsVar)
    End Sub
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
    Private Function ele_t(ByVal x As String) As String
        Dim rs As Integer = 0
        Dim Mo As String = ""
        ele_t = ""
        Select Case x
            Case "A"
                ' RECORD A
                ele_t = "A"
                ele_t &= "".PadRight(14)
                ele_t &= "NSP00"
                ' TIPO DI INVIO 01 O 10 SE C'E' L'INTERMEDIARIO
                If TextEdit12.EditValue > "" Then
                    ele_t &= "10"
                Else
                    ele_t &= "01"
                End If
                If TextEdit12.EditValue > "" Then
                    ele_t &= CStr(TextEdit12.EditValue).PadRight(16)
                Else
                    ele_t &= CStr(TextEdit19.EditValue).PadRight(16)
                End If
                ele_t &= "".PadRight(483)
                'n. invio telematico e totale invii
                ele_t &= "00000000"
                ele_t &= "".PadRight(1368)
                ele_t &= "A"
            Case "B"
                ' RECORD B
                ele_t = "B"
                ele_t &= CStr(TextEdit2.EditValue).PadRight(16)
                ele_t &= "1".PadLeft(8, "0")
                ele_t &= "".PadRight(3)
                ele_t &= "".PadRight(25)
                ele_t &= "".PadRight(20)
                ele_t &= "04394270013".PadRight(16)
                'COMUNICAZIONE ORDINARIA
                ele_t &= "100"
                ele_t &= "".PadLeft(17, "0")
                ele_t &= "".PadLeft(6, "0")
                ' DATI AGGREGATI
                ele_t &= "1"
                ele_t &= "0"
                'QUADRI COMPILATI - BISOGNA SAPERLO PRIMA - DA MODIFICARE
                If DsWri.Rows.Count > 0 Then ele_t &= "1" Else ele_t &= "0"
                ele_t &= "0"
                If DsVar.Rows.Count > 0 Then ele_t &= "1" Else ele_t &= "0"
                ele_t &= "000000001"
                ele_t &= CStr(TextEdit3.EditValue).PadRight(11)
                'codice ateco C28
                ele_t &= CStr(TextEdit15.EditValue).PadRight(6)
                'telefono C29
                ele_t &= CStr(TextEdit14.EditValue).PadRight(12)
                'fax C30
                ele_t &= CStr(TextEdit17.EditValue).PadRight(12)
                'posta elettronica C31
                ele_t &= CStr(TextEdit18.EditValue).PadRight(50)

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

                'PERSONA NON FISICA
                ele_t &= CStr(TextEdit9.EditValue).PadRight(60)


                ele_t &= ANNO
                ele_t &= "  "
                'soggetto che effettua la comunicazione se diverso dal soggetto cui si riferisce la comunicazione
                If TextEdit9.EditValue.ToString > "" Then
                    ele_t &= CStr(TextEdit19.EditValue).PadRight(16)
                    ele_t &= CStr(TextEdit16.EditValue).PadLeft(2, "0")
                    ele_t &= "00000000"
                    ele_t &= "00000000"
                    ele_t &= Mid(AC(0), 1, 24).PadRight(24)
                    ele_t &= Mid(AC(1), 1, 20).PadRight(20)
                    ele_t &= AC(2).PadRight(1)
                    ele_t &= CDate(AC(3)).Day.ToString.PadLeft(2, "0")
                    ele_t &= CDate(AC(3)).Month.ToString.PadLeft(2, "0")
                    ele_t &= CDate(AC(3)).Year.ToString.PadLeft(4, "0")
                    ele_t &= Mid(AC(4), 1, 40).PadRight(40)
                    ele_t &= AC(5).PadRight(2)
                Else
                    ele_t &= "".PadRight(16)
                    ele_t &= "00"
                    ele_t &= "00000000"
                    ele_t &= "00000000"
                    ele_t &= "".PadRight(24)
                    ele_t &= "".PadRight(20)
                    ele_t &= "".PadRight(1)
                    ele_t &= "00000000"
                    ele_t &= "".PadRight(40) ''CStr(TextEdit7.EditValue).PadRight(40)
                    ele_t &= "".PadRight(2) ''CStr(TextEdit8.EditValue).PadRight(2)
                End If

                'PERSONA NON FISICA
                ele_t &= "".PadRight(60) ''CStr(TextEdit9.EditValue).PadRight(60)

                'INTERMEDIARIO
                ele_t &= CStr(TextEdit12.EditValue).PadRight(16)
                ele_t &= CStr(TextEdit13.EditValue).PadLeft(5, "0")
                ele_t &= "1"
                ele_t &= " "
                If (DateEdit1.EditValue Is Nothing Or DateEdit1.EditValue Is DBNull.Value) Then
                    ele_t &= "00000000"
                Else
                    ele_t &= CDate(DateEdit1.EditValue).Day.ToString.PadLeft(2, "0")
                    ele_t &= CDate(DateEdit1.EditValue).Month.ToString.PadLeft(2, "0")
                    ele_t &= CDate(DateEdit1.EditValue).Year.ToString.PadLeft(4, "0")
                End If
                ele_t &= "".PadRight(1296)
                ele_t &= "A"
            Case "E"
                ' RECORD E
                ele_t = "E"
                ele_t &= CStr(TextEdit2.EditValue).PadRight(16)
                ele_t &= "1".PadLeft(8, "0")
                ele_t &= "".PadRight(3)
                ele_t &= "".PadRight(25)
                ele_t &= "".PadRight(20)
                ele_t &= "04394270013".PadRight(16)
                ele_t &= "TA001001" & Trim(RecordC.c6).PadLeft(16)
                If RecordC.c4 > "" Then ele_t &= "TA003002" & Trim(RecordC.c4).PadLeft(16)
                If RecordC.c5 > "" Then ele_t &= "TA003003" & Trim(RecordC.c5).PadLeft(16)
                rs = Len(ele_t)
                ele_t &= "".PadRight(1897 - rs) & "A"
            Case "Z"
                ' RECORD Z
                ele_t = "Z"
                ele_t &= "".PadRight(14)
                ele_t &= "1".PadLeft(9, "0")
                ele_t &= Trim(RecordC.c2).PadLeft(9, "0")
                ele_t &= "0".PadLeft(9, "0")
                ele_t &= "1".PadLeft(9, "0")
                rs = Len(ele_t)
                ele_t &= "".PadRight(1897 - rs) & "A"
        End Select
        ' ele_t &= "0D0A"
    End Function
    Sub WriteRecord(ByVal n)
        REM lettura FA
        DsWri = New DataTable
        DaWri = New SqlDataAdapter("EXEC XELE2012 @ANNO=" & ANNO & ",@T=1", cnCo)
        DaWri.SelectCommand.CommandTimeout = 300
        DaWri.Fill(DsWri)
        REM lettura BL
        DsVar = New DataTable
        DaVar = New SqlDataAdapter("SELECT * FROM TBESTCF inner join TbEPoE ON EstCfCodice = DiCfCodice AND DiCfAnno = EstCFANNO  WHERE EstCfAnno =" & ANNO & " order by EstCfCodice", cnCo)
        DaVar.SelectCommand.CommandTimeout = 300
        DaVar.Fill(DsVar)
        Dim Cbl As Integer = 0
        Dim wr As String = ""
        Dim Mo As String = ""
        Dim NC As Integer = 1
        Dim FA(16) As String
        Dim NP As Integer = 0
        Dim Rs As Integer = 0
        Dim Npc As Integer = 0
        Dim Npf As Integer = 0
        Dim M As Int16 = 3
        Dim CnF As Integer = 0
        Dim CnB As Integer = 0
        Dim CnX As Integer = 0
        For j As Int16 = 0 To 16 : FA(j) = "" : Next
        RecordC.c1 = "C" & CStr(TextEdit2.EditValue).PadRight(16)
        RecordC.c2 = NC.ToString.PadLeft(8, "0")
        RecordC.c3 = "".PadLeft(48) & "04394270013".PadRight(16)
        For x As Int16 = 1 To DsWri.Rows.Count
            RwWri = DsWri.Rows(x - 1)
            If RwWri("SpeCfPiva") > "" Then FA(1) = RwWri("SpeCfPiva").ToString.PadRight(16) Else FA(1) = ""
            If RwWri("SpeCfCfis") > "" And RwWri("SpeCfRie") = 0 Then FA(2) = RwWri("SpeCfCfis").ToString.PadRight(16) Else FA(2) = ""
            If RwWri("SpeCfRie") = 1 Then FA(3) = "1".ToString.PadLeft(16) Else FA(3) = ""
            If RwWri("SpeCfNoa") > 0 Then FA(4) = RwWri("SpeCfNoa").ToString.PadLeft(16) Else FA(4) = ""
            If RwWri("SpeCfNop") > 0 Then FA(5) = RwWri("SpeCfNop").ToString.PadLeft(16) Else FA(5) = ""
            FA(6) = ""
            If RwWri("SpeCfT7") > 0 Then FA(7) = RwWri("SpeCfT7").ToString.PadLeft(16) Else FA(7) = ""
            If RwWri("SpeCfT8") > 0 Then FA(8) = RwWri("SpeCfT8").ToString.PadLeft(16) Else FA(8) = ""
            If RwWri("SpeCfT9") > 0 Then FA(9) = RwWri("SpeCfT9").ToString.PadLeft(16) Else FA(9) = ""
            If RwWri("SpeCfT10") > 0 Then FA(10) = RwWri("SpeCfT10").ToString.PadLeft(16) Else FA(10) = ""
            If RwWri("SpeCfT11") > 0 Then FA(11) = RwWri("SpeCfT11").ToString.PadLeft(16) Else FA(11) = ""
            If RwWri("SpeCfT12") > 0 Then FA(12) = RwWri("SpeCfT12").ToString.PadLeft(16) Else FA(12) = ""
            If RwWri("SpeCfT13") > 0 Then FA(13) = RwWri("SpeCfT13").ToString.PadLeft(16) Else FA(13) = ""
            If RwWri("SpeCfT14") > 0 Then FA(14) = RwWri("SpeCfT14").ToString.PadLeft(16) Else FA(14) = ""
            If RwWri("SpeCfT15") > 0 Then FA(15) = RwWri("SpeCfT15").ToString.PadLeft(16) Else FA(15) = ""
            If RwWri("SpeCfT16") > 0 Then FA(16) = RwWri("SpeCfT16").ToString.PadLeft(16) Else FA(16) = ""
            NP = NP + 1
            CnF = CnF + 1
            RecordC.c6 = CnF.ToString.PadLeft(16)
            If NP > 3 Then
                NC = NC + 1
                RecordC.c2 = NC.ToString.PadLeft(8, "0")
                NP = 1
            End If
            For k = 1 To 3
                If FA(k) > "" Then
                    For u = k + 1 To 3
                        FA(u) = ""
                    Next
                End If
            Next
            For K = 1 To 16
                If FA(K) <> "" Then
                    wr &= "FA" & NP.ToString.PadLeft(3, "0") & K.ToString.PadLeft(3, "0") & FA(K)

                End If
            Next
            If NP = 3 Then
                Cbl = Cbl + 1
                CercaBl(Cbl, wr, Npc, Npf)
                Mo = RecordC.c1 & RecordC.c2 & RecordC.c3 & wr
                Rs = Len(Mo)
                Mo &= "".PadRight(1897 - Rs) & "A"
                output.WriteLine(Mo)
                wr = ""
            End If
        Next
        If NP <> 3 Then
            Mo = RecordC.c1 & RecordC.c2 & RecordC.c3 & wr
            Rs = Len(Mo)
            Mo &= "".PadRight(1897 - Rs) & "A"
            output.WriteLine(Mo)
            wr = ""
        End If
    End Sub
    Sub CercaBl(ByRef Cbl As Integer, ByRef wr As String, ByRef Npc As Integer, ByRef Npf As Integer)
        If Cbl > DsVar.Rows.Count Then Exit Sub
        Dim BL(25) As String
        For j As Int16 = 0 To 25 : BL(j) = "" : Next
        For x As Int16 = Cbl To Cbl
            RwVar = DsVar.Rows(x - 1)
            If RwVar("DiCognome") > "" Then BL(1) = RwVar("DiCognome") Else BL(1) = ""
            If RwVar("DiNome") > "" Then BL(2) = RwVar("DiNome") Else BL(2) = ""
            If (RwVar("DiDataNasc") Is Nothing Or RwVar("DiDataNasc") Is DBNull.Value) Then
                BL(3) = ""
            ElseIf CDate(RwVar("DiDataNasc")).ToShortDateString > "" Then
                BL(3) = CDate(RwVar("DiDataNasc")).Day.ToString.PadLeft(2, "0")
                BL(3) &= CDate(RwVar("DiDataNasc")).Month.ToString.PadLeft(2, "0")
                BL(3) &= CDate(RwVar("DiDataNasc")).Year.ToString.PadLeft(4, "0")
                BL(3) = BL(3).PadLeft(16)
            Else
                BL(3) = ""
            End If
            If RwVar("DiComune") > "" Then BL(4) = RwVar("DiComune") Else BL(4) = ""
            If RwVar("DiProv") > "" Then BL(5) = RwVar("DiProv").ToString.PadRight(16) Else BL(5) = ""
            If RwVar("DiStato") > "" Then BL(6) = RwVar("DiStato").ToString.PadLeft(16) Else BL(6) = ""
            If RwVar("DiDenomina") > "" Then BL(7) = RwVar("DiDenomina") Else BL(7) = ""
            If RwVar("DiECitta") > "" Then BL(8) = RwVar("DiECitta") Else BL(8) = ""
            If RwVar("DiEStato") > "" Then BL(9) = RwVar("DiEStato").ToString.PadLeft(16) Else BL(9) = ""
            If RwVar("DiEIndiri") > "" Then BL(10) = RwVar("DiEIndiri") Else BL(10) = ""
            If RwVar("DiCfTipo") = "CL" Then
                BL(11) = "BL002003" & "1".ToString.PadLeft(16)
                If RwVar("EstCfT3") > 0 Then BL(13) = "BL003001" & RwVar("EstCfT3").ToString.PadLeft(16) Else BL(13) = ""
                If RwVar("EstCfT4") > 0 Then BL(14) = "BL003002" & RwVar("EstCfT4").ToString.PadLeft(16) Else BL(14) = ""
                If RwVar("EstCfT6") > 0 Then BL(15) = "BL006001" & RwVar("EstCfT6").ToString.PadLeft(16) Else BL(15) = ""
                If RwVar("EstCfT7") > 0 Then BL(16) = "BL006002" & RwVar("EstCfT7").ToString.PadLeft(16) Else BL(16) = ""
                Npc = Npc + 1
                RecordC.c4 = Npc.ToString.PadLeft(16)
            Else
                BL(11) = ""
            End If
            If RwVar("DiCfTipo") = "FO" Then
                BL(12) = "BL002004" & "1".ToString.PadLeft(16)
                If RwVar("EstCfT3") > 0 Then BL(13) = "BL006001" & RwVar("EstCfT3").ToString.PadLeft(16) Else BL(13) = ""
                If RwVar("EstCfT4") > 0 Then BL(14) = "BL006002" & RwVar("EstCfT4").ToString.PadLeft(16) Else BL(14) = ""
                If RwVar("EstCfT6") > 0 Then BL(15) = "BL003001" & RwVar("EstCfT6").ToString.PadLeft(16) Else BL(15) = ""
                If RwVar("EstCfT7") > 0 Then BL(16) = "BL003002" & RwVar("EstCfT7").ToString.PadLeft(16) Else BL(16) = ""
                Npf = Npf + 1
                RecordC.c5 = Npf.ToString.PadLeft(16)
            Else
                BL(12) = ""
            End If
            BL(0) = ""
            For k = 1 To 10
                If BL(k) <> "" Then
                    If k <> 3 Or k <> 5 Or k <> 6 Or k <> 9 Then Spacchetta(BL(k), k)
                    BL(0) &= "BL001" & k.ToString.PadLeft(3, "0") & BL(k)
                End If
            Next
            For k = 11 To 16
                If BL(k) <> "" Then
                    BL(0) &= BL(k)
                End If
            Next
        Next
        wr &= BL(0)
    End Sub
    Sub Spacchetta(ByRef str As String, ByVal N As Int16)
        If str.Length < 17 Then
            str = str.PadRight(16)
            Exit Sub
        End If
        Dim Iniziale As String = Mid(str, 1, 16)

        For y = 17 To 47 Step 15
            If str.Length >= y Then
                Iniziale &= "BL001" & N.ToString.PadLeft(3, "0") & "+" & (Mid(str, y, 15)).PadRight(15)
            End If
        Next
        str = Iniziale
    End Sub
#End Region

End Class