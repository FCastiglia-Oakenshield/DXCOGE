Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml
Public Class DxSp2020
    Dim DsMin As DataTable
    Dim DaMin As SqlDataAdapter
    Dim RwMin As DataRow

    Dim DsMax As DataTable
    Dim DaMax As SqlDataAdapter
    Dim RwMax As DataRow

    Dim DsEle As DataTable
    Dim DaEle As SqlDataAdapter
    Dim RwEle As DataRow

    Dim DsDai, DsPoe, TbPErr, TbDErr As DataTable
    Dim DaDai, DaPoE, DaPErr, DaDErr As SqlDataAdapter
    Dim BlDai, BlPErr, BlDErr As SqlCommandBuilder
    Dim RwDai, RwPoE As DataRow

    Dim DsVar As DataTable
    Dim DaVar As SqlDataAdapter
    Dim RwVar As DataRow

    Dim DsWri As DataTable
    Dim DaWri As SqlDataAdapter
    Dim RwWri As DataRow

    Dim DsPrt As DataTable
    Dim DaPrt As SqlDataAdapter
    Dim RwPrt As DataRow

    Dim DaregV, DaRegA As SqlDataAdapter
    Dim TbRegV, TbRegA As DataTable

    Dim DaR As SqlDataAdapter
    Dim TbR As DataTable

    Dim TiRiep, StrPrint, StrR, Str As String
    Dim ANNO As Int16 = 2020
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
    Dim EsistePeriodo As Boolean
    Dim ProgInvio, ProgrFile As Int32
    Dim ClFo As String = "CL"
    Dim UltChiuso As Int16 = 0

    Dim TbDte, TbDtr As DataTable
    Dim DaDte, DaDtr As SqlDataAdapter
    Dim RwDte, RwDtr As DataRow
    Dim CliFor As String
    Dim NumProt, Registro As Int32
    Dim CambioPeriodo As Boolean = False
    Dim Inviato As Boolean

    Dim Xtw As XmlTextWriter
    Dim Paesi As Collection
    Dim CambioCliFor As Boolean = False
    Dim ProgrDTE, ProgrDTR As Int32
    Dim RegimeIva As String
    Dim Cloud As Boolean = False

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

    Private Sub DxSp2017_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        CaricaPaesi()
        ControlloRighe()
    End Sub
    Private Sub CaricaPaesi()
        Paesi = New Collection

        Cmd = New SqlCommand("Select distinct PaSigla from TbPaesi", cnCo)
        dataRd = Cmd.ExecuteReader

        While dataRd.Read
            Paesi.Add(dataRd.Item("PaSigla"), dataRd.Item("PaSigla"))
        End While
        dataRd.Close()
        Dim Contiene As String = ""
        Cmd = New SqlCommand("SELECT Sel8 from TbSel where SelId=1", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Contiene = dataRd.Item("Sel8")
        End While
        dataRd.Close()
        Cloud = Contiene.Contains("\\TSCLIENT")
    End Sub

    Private Sub DxSp2016_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        SetInizio()
    End Sub
    Private Sub SetInizio()
        ComboBoxEdit1.Properties.Items.Clear()
        ComboBoxEdit1.Properties.Items.Add(ANNO)
        ComboBoxEdit1.SelectedIndex = 0
        XtraTabControl1.SelectedTabPageIndex = 0

        Pulizia()
        Iva_Regime()
        RendiTrimestriDisponibili()
        UltimoChiuso()
        LeggiPeriodo()

        'VisRegistri()
    End Sub
    Private Sub Pulizia()
        RadioGroup1.EditValue = 0 : TextEdit20.EditValue = Nothing : TextEdit21.EditValue = Nothing : ProgInvio = 0 : RadioGroup2.EditValue = ClFo : RadioGroup3.EditValue = RadioGroup2.EditValue : RadioGroup4.EditValue = RadioGroup2.EditValue : RadioGroup5.EditValue = RadioGroup2.EditValue

        RadioGroup1.Properties.Items(0).Enabled = False : RadioGroup1.Properties.Items(1).Enabled = False : RadioGroup1.Properties.Items(2).Enabled = False
        RadioGroup1.Properties.Items(3).Enabled = False

        TextEdit22.ResetBackColor()

        XtraTabPage2.PageVisible = False : XtraTabPage3.PageVisible = False : XtraTabPage4.PageVisible = False : XtraTabPage5.PageVisible = False : XtraTabPage6.PageVisible = False : XtraTabPage7.PageVisible = False : XtraTabPage8.PageVisible = False

        ButtonF1.Enabled = False

        Inviato = False
        RadioGroup1.EditValue = 0
    End Sub
    Private Sub Iva_Regime()
        Cmd = New SqlCommand("Select top 1 * from tbAzi order by AziAnnoLavoro desc", cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            RegimeIva = dataRd.Item("AziRegimeIva")
        Else
            RegimeIva = 0
        End If
        dataRd.Close()
    End Sub
    Sub RendiTrimestriDisponibili()
        'If RegimeIva = 1 Then
        '    Exit Sub
        'End If

        Dim P As Int16
        Dim Cmd As New SqlCommand("Select * from TbVers where IvaVmese > 0 and IvaVmese < 13 and IvaVanno = " & ANNO & " order by IvaVmese", cnCo)
        ' Dim Cmd As New SqlCommand("Select * from TbVers where IvaVanno =" & ANNO & " and (ivavmese=3 or ivavmese=6 or ivavmese=9 or ivavmese=12)", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ' P = dataRd.Item("IvaVmese") - 1
            P = (dataRd.Item("IvaVmese") / 3) - 1
            If P > -1 Then RadioGroup1.Properties.Items(P).Enabled = True
        End While
        dataRd.Close()
    End Sub
    Private Sub UltimoChiuso()
        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)

        P1.Value = ANNO

        Dim Cmd As New SqlCommand("Select * from TbRegXML_EST where RxInviato = 1 AND RxAnno = @ANNO ORDER BY RxPeriodo desc ", cnCo)
        Cmd.Parameters.Add(P1)

        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            UltChiuso = dataRd.Item("RxPeriodo")
            CheckEdit2.Checked = dataRd.Item("RxNoControl")
        End If
        dataRd.Close()

        Cmd = New SqlCommand("Select isnull(Max(RxProgInvio),0) from TbRegXML_EST", cnCo)
        ProgInvio = Cmd.ExecuteScalar

        If RegimeIva = 0 Then
            Exit Sub
        End If

        For P = 0 To UltChiuso - 1
            RadioGroup1.Properties.Items(P).Enabled = True
        Next

    End Sub

    Sub Bottoni(ByVal N As Boolean)
        ButtonF1.Enabled = Not N
        XtraTabPage2.PageVisible = N And RadioGroup1.EditValue <> 0
        XtraTabPage8.PageVisible = N And RadioGroup1.EditValue <> 0
        XtraTabPage3.PageVisible = N And RadioGroup1.EditValue <> 0 And TbR.Rows.Count > 0
        XtraTabPage4.PageVisible = N And RadioGroup1.EditValue <> 0
        XtraTabPage5.PageVisible = N And RadioGroup1.EditValue <> 0
        XtraTabPage6.PageVisible = N And RadioGroup1.EditValue <> 0
        GridColumn5.OptionsColumn.ReadOnly = N : GridColumn10.OptionsColumn.ReadOnly = N
        GridColumn5.OptionsColumn.AllowEdit = Not N : GridColumn10.OptionsColumn.AllowEdit = Not N
        CheckButton2.Checked = N
        CheckButton2.Enabled = RadioGroup1.EditValue <> 0
    End Sub
    Private Sub LeggiPeriodo()
        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)

        P1.Value = ANNO
        P2.Value = RadioGroup1.EditValue

        Dim Cmd As New SqlCommand("Select * from TbRegXML_EST where RxAnno = @ANNO AND RxPeriodo = @PERIODO ", cnCo)
        Cmd.Parameters.Add(P1)
        Cmd.Parameters.Add(P2)

        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit20.EditValue = dataRd.Item("RxDal")
            TextEdit21.EditValue = dataRd.Item("RxAl")
            'ProgInvio = dataRd.Item("RxProgInvio")
            Inviato = dataRd.Item("RxInviato")
            CheckEdit2.Checked = dataRd.Item("RxNoControl")
            EsistePeriodo = True
        Else
            EsistePeriodo = False
            Inviato = False
        End If
        dataRd.Close()

        If RadioGroup1.EditValue = 0 Then
            EsistePeriodo = True
        End If

        Bottoni(EsistePeriodo)
        AbilitaDisabilita()
        Visregistri()
    End Sub
    Private Sub AbilitaDisabilita()
        GridControl1.Enabled = Not Inviato
        GridControl5.Enabled = Not Inviato
        ButtonF11.Enabled = Not Inviato
    End Sub
    Private Sub Visregistri()
        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)

        P1.Value = ANNO
        P2.Value = RadioGroup1.EditValue


        Dim RegV As String = "EXEC X_LEGGI_REGXML_EST @ANNO, 'V', @PERIODO"
        Dim RegA As String = "EXEC X_LEGGI_REGXML_EST @ANNO, 'A', @PERIODO"


        DaregV = New SqlDataAdapter(RegV, cnCo)
        DaregV.SelectCommand.Parameters.Add(P1)
        DaregV.SelectCommand.Parameters.Add(P2)
        TbRegV = New DataTable("REGV")
        DaregV.Fill(TbRegV)

        DaregV.SelectCommand.Parameters.Clear()

        DaRegA = New SqlDataAdapter(RegA, cnCo)
        DaRegA.SelectCommand.Parameters.Add(P1)
        DaRegA.SelectCommand.Parameters.Add(P2)
        TbRegA = New DataTable("REGA")
        DaRegA.Fill(TbRegA)

        DaRegA.SelectCommand.Parameters.Clear()

        GridControl2.DataSource = TbRegV
        GridControl4.DataSource = TbRegA

        GridView2.ClearSelection()
        GridView2.OptionsSelection.EnableAppearanceFocusedRow = False

        GridView3.ClearSelection()
        GridView3.OptionsSelection.EnableAppearanceFocusedRow = False

    End Sub
    Private Sub RadioGroup1_EditValueChanged(sender As Object, e As System.EventArgs) Handles RadioGroup1.EditValueChanged
        CambioPeriodo = True
        If RadioGroup1.EditValue = 0 Then
            GridColumn5.OptionsColumn.ReadOnly = True : GridColumn10.OptionsColumn.ReadOnly = True
            GridColumn5.OptionsColumn.AllowEdit = False : GridColumn10.OptionsColumn.AllowEdit = False
        Else
            GridColumn5.OptionsColumn.ReadOnly = False : GridColumn10.OptionsColumn.ReadOnly = False
            GridColumn5.OptionsColumn.AllowEdit = True : GridColumn10.OptionsColumn.AllowEdit = True
            LeggiPeriodo()
            AssegnaTrimestre()
            GridView2.Focus()
        End If
        CambioPeriodo = False
    End Sub
    Private Sub AssegnaTrimestre()
        Dim MMDA As Int16 = (RadioGroup1.EditValue - 1) * 3 + 1
        Dim MMA As Int16 = (RadioGroup1.EditValue - 1) * 3 + 3
        Dim GGDA As Int16 = 1
        Dim GGA As Int16 = Date.DaysInMonth(ANNO, MMA)
        TextEdit20.EditValue = DateSerial(ANNO, MMDA, GGDA)
        TextEdit21.EditValue = DateSerial(ANNO, MMA, GGA)
    End Sub

    Private Sub ButtonF1_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF1.Click
        If Controllo() = False Then
            Exit Sub
        End If
        Elabora()
        XtraTabPage2.PageVisible = True : XtraTabPage8.PageVisible = True : XtraTabPage4.PageVisible = True : XtraTabPage5.PageVisible = True : XtraTabPage6.PageVisible = True : XtraTabControl1.SelectedTabPageIndex = 1
        PaginaII()
    End Sub

    Private Function Controllo() As Boolean
        Controllo = False

        For i As Int16 = 0 To GridView2.RowCount
            If GridView2.GetRowCellValue(i, GridColumn5) = True Then
                Return True
            End If
        Next

        For i As Int16 = 0 To GridView3.RowCount
            If GridView3.GetRowCellValue(i, GridColumn10) = True Then
                Return True
            End If
        Next

        MessageBox.Show("SELEZIONARE ALMENO UN REGISTRO !!!", "CONTROLLO SELEZIONE REGISTRI", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Return Controllo
    End Function

    Private Sub Elabora()
        Cursor.Current = Cursors.WaitCursor

        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)

        P1.Value = ANNO
        P2.Value = RadioGroup1.EditValue

        Cmd = New SqlCommand("DELETE FROM TbRegXML_EST where RxAnno = @ANNO and RxPeriodo = @PERIODO", cnCo)
        Cmd.Parameters.Add(P1)
        Cmd.Parameters.Add(P2)
        Cmd.ExecuteNonQuery()

        Cmd.Parameters.Clear()

        For i As Int16 = 0 To GridView2.RowCount - 1
            Memorizza("V", GridView2.GetDataRow(i)("RIvaNreg"), GridView2.GetRowCellValue(i, GridColumn5))
        Next

        For i As Int16 = 0 To GridView3.RowCount - 1
            Memorizza("A", GridView3.GetDataRow(i)("RIvaNreg"), GridView3.GetRowCellValue(i, GridColumn10))
        Next

        Cmd = New SqlCommand("EXEC XI1XML_EST @ANNO, @PERIODO", cnCo)
        Cmd.Parameters.Add(P1)
        Cmd.Parameters.Add(P2)
        Cmd.ExecuteNonQuery()

        Cursor.Current = Cursors.Default

        ControlloPiva()

        Inviato = False
        AbilitaDisabilita()
    End Sub
    Private Sub ControlloPiva()
        Cursor.Current = Cursors.WaitCursor
        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)

        P1.Value = ANNO
        P2.Value = RadioGroup1.EditValue
        DaPErr = New SqlDataAdapter("Select * from TbErrP where PErrAnno=@ANNO AND PErrPeriodo=@PERIODO", cnCo)
        BlPErr = New SqlCommandBuilder(DaPErr)
        DaPErr.SelectCommand.Parameters.Add(P1)
        DaPErr.SelectCommand.Parameters.Add(P2)
        TbPErr = New DataTable("ERR")
        DaPErr.Fill(TbPErr)

        For i As Int32 = 0 To TbPErr.Rows.Count - 1
            Rw = TbPErr.Rows(i)
            If Codfisc(Rw("PErrPiva")) = False Then
                Rw("PErrEscludi") = True
                Rw("PErrErrore") = True
                Rw("PErrMsg") = "PARTITA IVA ERRATA !!"
            End If
            If Codfisc(Rw("PerrCFis")) = False Then
                Rw("PErrEscludi") = True
                Rw("PErrErrore") = True
                Rw("PErrMsg") = Rw("PErrMsg") & " CODICE FISCALE ERRATO !!"
            End If
        Next
        DaPErr.Update(TbPErr)
        TbPErr.AcceptChanges()

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub Memorizza(Tipo As String, Registro As Int16, Selezionato As Boolean)
        Dim P1 As New SqlParameter("@RxAnno", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@RxTipo", SqlDbType.VarChar)
        Dim P3 As New SqlParameter("@RxRegistro", SqlDbType.SmallInt)
        Dim P4 As New SqlParameter("@RxPeriodo", SqlDbType.SmallInt)
        Dim P5 As New SqlParameter("@RxDal", SqlDbType.SmallDateTime)
        Dim P6 As New SqlParameter("@RxAl", SqlDbType.SmallDateTime)
        Dim P7 As New SqlParameter("@RxProgInvio", SqlDbType.Int)
        Dim P8 As New SqlParameter("@RxSel", SqlDbType.Bit)

        P1.Value = ANNO
        P2.Value = Tipo
        P3.Value = Registro
        P4.Value = RadioGroup1.EditValue
        P5.Value = TextEdit20.EditValue
        P6.Value = TextEdit21.EditValue
        P7.Value = ProgInvio
        P8.Value = Selezionato

        Cmd = New SqlCommand("INSERT INTO TbRegXML_EST (RxAnno,RxTipo,RxRegistro,RxPeriodo,RxDal,RxAl,RxProgInvio,RxSel) VALUES (@RxAnno,@RxTipo,@RxRegistro,@RxPeriodo,@RxDal,@RxAl,@RxProgInvio,@RxSel)", cnCo)
        Cmd.Parameters.Add(P1)
        Cmd.Parameters.Add(P2)
        Cmd.Parameters.Add(P3)
        Cmd.Parameters.Add(P4)
        Cmd.Parameters.Add(P5)
        Cmd.Parameters.Add(P6)
        Cmd.Parameters.Add(P7)
        Cmd.Parameters.Add(P8)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub CheckButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckButton2.CheckedChanged
        If CheckButton2.Checked = False AndAlso AbilitaElenchi() = True Then
            CheckButton2.ImageIndex = 19 : CheckButton2.ToolTip = "ELENCO APERTO"
            If CambioPeriodo = False Then
                EsistePeriodo = Not EsistePeriodo : Bottoni(EsistePeriodo)
            End If

        Else
            CheckButton2.ImageIndex = 18 : CheckButton2.ToolTip = "ELENCO CHIUSO" 'LockButton(True)
        End If
    End Sub

    Function AbilitaElenchi() As Boolean
        If CambioPeriodo = True Then
            Return True
        End If

        Dim P As New DxPwdDialog
        DxPwdDialog.Password = "EST2020"
        P.ShowDialog()
        Return DxPwdDialog.Esatta
    End Function

#Region "DETTAGLIO FATTURE"
    Private Sub PaginaII()
        GroupControl3.Text = "DETTAGLIO FATTURE " & RadioGroup2.Properties.Items(RadioGroup2.SelectedIndex).Description

        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)
        Dim P3 As New SqlParameter("@TIPO", SqlDbType.VarChar)

        P1.Value = ANNO
        P2.Value = RadioGroup1.EditValue
        P3.Value = RadioGroup2.EditValue



        Cursor.Current = Cursors.WaitCursor

        DsMax = New DataTable
        DaMax = New SqlDataAdapter("Select * from VELEXMLFATTURE_EST where EleCfAnno = @ANNO AND  EleCfPeriodo = @PERIODO AND EleCfTipo = @TIPO order by EleCfTipo,EleCfPriRegIva,EleCfNumProt", cnCo)
        DaMax.SelectCommand.Parameters.Add(P1)
        DaMax.SelectCommand.Parameters.Add(P2)
        DaMax.SelectCommand.Parameters.Add(P3)
        DaMax.SelectCommand.CommandTimeout = 300
        DaMax.Fill(DsMax)
        GridControl51.DataSource = DsMax
        GridView51.ClearSelection()
        GridView51.ExpandAllGroups()

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadioGroup2_EditValueChanged(sender As Object, e As System.EventArgs) Handles RadioGroup2.EditValueChanged
        If IsNothing(DsMax) Then
            Exit Sub
        End If
        If CambioCliFor = True Then
            Exit Sub
        End If
        CambioCliFor = True
        RadioGroup4.EditValue = RadioGroup2.EditValue
        RadioGroup3.EditValue = RadioGroup2.EditValue
        RadioGroup5.EditValue = RadioGroup2.EditValue
        PaginaII()
        CambioCliFor = False
    End Sub
    Private Sub RadioGroupx_EditValueChanged(sender As Object, e As System.EventArgs) Handles RadioGroup3.EditValueChanged
        If IsNothing(DsMax) Then
            Exit Sub
        End If
        If CambioCliFor = True Then
            Exit Sub
        End If
        CambioCliFor = True
        RadioGroup4.EditValue = RadioGroup3.EditValue
        RadioGroup2.EditValue = RadioGroup3.EditValue
        RadioGroup5.EditValue = RadioGroup3.EditValue
        PaginaVIII()
        CambioCliFor = False
    End Sub
    Private Sub RadioGroup4_EditValueChanged(sender As Object, e As System.EventArgs) Handles RadioGroup4.EditValueChanged
        'If IsNothing(DsMax) Then
        '    Exit Sub
        'End If
        If CambioCliFor = True Then
            Exit Sub
        End If
        CambioCliFor = True
        RadioGroup2.EditValue = RadioGroup4.EditValue
        RadioGroup3.EditValue = RadioGroup4.EditValue
        RadioGroup5.EditValue = RadioGroup4.EditValue
        VisIntegrativi()
        CambioCliFor = False

    End Sub
    Private Sub RadioGroup5_EditValueChanged(sender As Object, e As System.EventArgs) Handles RadioGroup5.EditValueChanged
        'If IsNothing(DsMax) Then
        '    Exit Sub
        'End If
        If CambioCliFor = True Then
            Exit Sub
        End If
        CambioCliFor = True
        RadioGroup2.EditValue = RadioGroup5.EditValue
        RadioGroup3.EditValue = RadioGroup5.EditValue
        RadioGroup4.EditValue = RadioGroup5.EditValue
        If PAGINVIO = True Then AggiornaDatiEsteri()
        IVPagina()
        CambioCliFor = False
    End Sub

    Private Sub ButtonF9B_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9B.Click
        DXANTEPRIMA(GridControl51, True, Printing.PaperKind.A4, "DETTAGLIO FATTURE " & RadioGroup2.Properties.Items(RadioGroup2.SelectedIndex).Description)
    End Sub


    Private Sub RepositoryItemCheckEdit1_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemCheckEdit1.EditValueChanged
        GridView51.SetFocusedRowCellValue("EleCfOK", DirectCast(sender, DevExpress.XtraEditors.CheckEdit).EditValue)
    End Sub
    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView51.CellValueChanged

        If GridView51.UpdateCurrentRow() Then
            If e.Column.FieldName = "EleCfOK" Then
                AggiornaRiga(e.RowHandle, e.Value)
            End If
        End If
    End Sub

    Private Sub AggiornaRiga(Riga As Int32, Valore As Boolean)
        Dim P0 As New SqlParameter("@PRegAnno", SqlDbType.SmallInt)
        Dim P1 As New SqlParameter("@PRegPeriodo", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PRegPriId", SqlDbType.Int)
        Dim P3 As New SqlParameter("@PRegOK", SqlDbType.Bit)

        P0.Value = GridView51.GetDataRow(Riga)("EleCfAnno")
        P1.Value = GridView51.GetDataRow(Riga)("EleCfPeriodo")
        P2.Value = GridView51.GetDataRow(Riga)("EleId")
        P3.Value = Valore

        Cmd = New SqlCommand("UPDATE TMPXMLREGIVA_EST set PRegOK = @PRegOK where PRegAnno=@PRegAnno AND PRegPeriodo=@PRegPeriodo AND PRegPriId=@PRegPriId", cnCo)
        Cmd.Parameters.Add(P0)
        Cmd.Parameters.Add(P1)
        Cmd.Parameters.Add(P2)
        Cmd.Parameters.Add(P3)
        Cmd.ExecuteNonQuery()
    End Sub
#End Region
#Region "DETTAGLIO FATTURE GIA INVIATE"
    Private Sub PaginaVIII()
        GroupControl49.Text = "FATTURE ELETTRONICHE " & RadioGroup2.Properties.Items(RadioGroup2.SelectedIndex).Description

        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)
        Dim P3 As New SqlParameter("@TIPO", SqlDbType.VarChar)

        P1.Value = ANNO
        P2.Value = RadioGroup1.EditValue
        P3.Value = RadioGroup3.EditValue



        Cursor.Current = Cursors.WaitCursor

        DsMax = New DataTable
        DaMax = New SqlDataAdapter("Select * from VELEXMLFATTURE_ELT_EST where EleCfAnno = @ANNO AND  EleCfPeriodo = @PERIODO AND EleCfTipo = @TIPO order by EleCfTipo,EleCfPriRegIva,EleCfNumProt", cnCo)
        DaMax.SelectCommand.Parameters.Add(P1)
        DaMax.SelectCommand.Parameters.Add(P2)
        DaMax.SelectCommand.Parameters.Add(P3)
        DaMax.SelectCommand.CommandTimeout = 300
        DaMax.Fill(DsMax)
        GridControl7.DataSource = DsMax
        GridView5.ClearSelection()
        GridView5.ExpandAllGroups()

        Cursor.Current = Cursors.Default
    End Sub
#End Region
#Region "DATI INTEGRATIVI PERSONE FISICHE"
    Private Sub VisIntegrativi()
        GroupControl6.Text = "DATI INTEGRATIVI PERSONE FISICHE " & RadioGroup2.Properties.Items(RadioGroup2.SelectedIndex).Description

        Dim P3 As New SqlParameter("@TIPO", SqlDbType.VarChar)

        P3.Value = RadioGroup2.EditValue

        DaDai = New SqlDataAdapter("SELECT * FROM TbIntPF WHERE PfClifor in (SELECT PRegClifor from TMPXMLREGIVA_EST) AND PFTIPO=@TIPO", cnCo)
        BlDai = New SqlCommandBuilder(DaDai)

        DaDai.SelectCommand.Parameters.Add(P3)
        DaDai.SelectCommand.CommandTimeout = 300

        DsDai = New DataTable("DAI")
        DaDai.Fill(DsDai)

        GridControl1.DataSource = DsDai
        AdvBandedGridView1.ClearSelection()
    End Sub
    Private Sub RepositoryItemTextedit4_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit4.EditValueChanged
        AdvBandedGridView1.SetFocusedRowCellValue("PfCognome", DirectCast(sender, DevExpress.XtraEditors.TextEdit).EditValue)
    End Sub
    Private Sub RepositoryItemTextedit5_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit5.EditValueChanged
        AdvBandedGridView1.SetFocusedRowCellValue("PfNome", DirectCast(sender, DevExpress.XtraEditors.TextEdit).EditValue)
    End Sub
    Private Sub GridView3_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles AdvBandedGridView1.CellValueChanged

        If AdvBandedGridView1.UpdateCurrentRow() Then
            DaDai.Update(DsDai)
        End If
    End Sub
#End Region
#Region "DATI ANAGRAFICI ERRATI"
    Sub IVPagina()
        GroupControl42.Text = "DATI ERRATI " & RadioGroup2.Properties.Items(RadioGroup2.SelectedIndex).Description
        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)
        Dim P3 As New SqlParameter("@TIPO", SqlDbType.VarChar)

        P1.Value = ANNO
        P2.Value = RadioGroup1.EditValue
        P3.Value = RadioGroup2.EditValue

        StrR = "SELECT * FROM TBERRP WHERE PERRANNO = @ANNO AND PERRPERIODO = @PERIODO AND PERRTIPO = @TIPO AND PERRERRORE = 1"
        TbDErr = New DataTable
        DaDErr = New SqlDataAdapter(StrR, cnCo)
        BlDErr = New SqlCommandBuilder(DaDErr)
        DaDErr.SelectCommand.Parameters.Add(P1)
        DaDErr.SelectCommand.Parameters.Add(P2)
        DaDErr.SelectCommand.Parameters.Add(P3)
        DaDErr.SelectCommand.CommandTimeout = 300
        DaDErr.Fill(TbDErr)
        GridControl5.DataSource = TbDErr
        AdvBandedGridView2.ClearSelection()
        GroupControl6.Text = ""
    End Sub
    Private Sub RepositoryItemTextedit7_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit7.EditValueChanged
        AdvBandedGridView2.SetFocusedRowCellValue("PErrPiva", DirectCast(sender, DevExpress.XtraEditors.TextEdit).EditValue)

        ControllaTutto()
    End Sub
    Private Sub RepositoryItemTextedit8_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit8.EditValueChanged
        AdvBandedGridView2.SetFocusedRowCellValue("PerrCFis", DirectCast(sender, DevExpress.XtraEditors.TextEdit).EditValue)

        ControllaTutto()
    End Sub
    Private Sub RepositoryItemTextedit9_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit9.EditValueChanged
        AdvBandedGridView2.SetFocusedRowCellValue("PErrPivaEst", DirectCast(sender, DevExpress.XtraEditors.TextEdit).EditValue)
        ControllaTutto()
    End Sub

    Private Sub RepositoryItemCheckEdit5_Enter(sender As Object, e As System.EventArgs) Handles RepositoryItemCheckEdit5.Enter
        RepositoryItemCheckEdit5.ReadOnly = (AdvBandedGridView2.GetFocusedRowCellValue("PErrMsg").ToString.Trim <> "")
    End Sub

    Private Sub ControllaTutto()
        AdvBandedGridView2.SetFocusedRowCellValue("PErrMsg", "")
        AdvBandedGridView2.SetFocusedRowCellValue("PErrEscludi", False)

        If Codfisc(AdvBandedGridView2.GetFocusedRowCellValue("PErrPiva").ToString.Trim) = False Then
            AdvBandedGridView2.SetFocusedRowCellValue("PErrMsg", "PARTITA IVA ERRATA !!")
            AdvBandedGridView2.SetFocusedRowCellValue("PErrEscludi", True)
            Exit Sub
        End If

        If Codfisc(AdvBandedGridView2.GetFocusedRowCellValue("PerrCFis").ToString.Trim) = False Then
            AdvBandedGridView2.SetFocusedRowCellValue("PErrMsg", " CODICE FISCALE ERRATO !!")
            AdvBandedGridView2.SetFocusedRowCellValue("PErrEscludi", True)
            Exit Sub
        End If

        If AdvBandedGridView2.GetFocusedRowCellValue("PErrPiva").ToString.Trim = "" And AdvBandedGridView2.GetFocusedRowCellValue("PerrCFis").ToString.Trim = "" And AdvBandedGridView2.GetFocusedRowCellValue("PErrPivaEst").ToString.Trim = "" Then
            AdvBandedGridView2.SetFocusedRowCellValue("PErrMsg", "MANCA PARTITA IVA !!")
            AdvBandedGridView2.SetFocusedRowCellValue("PErrEscludi", True)
            Exit Sub
        End If

        If (AdvBandedGridView2.GetFocusedRowCellValue("PErrPivaEst").ToString.Trim <> "" And AdvBandedGridView2.GetFocusedRowCellValue("PErrPivaEst").ToString.Trim <> "OO99999999999" And Paesi.Contains(Mid(AdvBandedGridView2.GetFocusedRowCellValue("PErrPivaEst").ToString.Trim, 1, 2)) = False) Or (Len(AdvBandedGridView2.GetFocusedRowCellValue("PErrPivaEst").ToString.Trim) < 5 And AdvBandedGridView2.GetFocusedRowCellValue("PErrPivaEst").ToString.Trim <> "") Then
            AdvBandedGridView2.SetFocusedRowCellValue("PErrMsg", "PARTITA IVA ESTERA ERRATA !!")
            AdvBandedGridView2.SetFocusedRowCellValue("PErrEscludi", True)
            Exit Sub
        End If

    End Sub

    Private Sub GridView5_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles AdvBandedGridView2.CellValueChanged

        If AdvBandedGridView2.UpdateCurrentRow() Then
            PAGINVIO = True
            DaDErr.Update(TbDErr)
        End If
    End Sub
    Sub AggiornaDatiEsteri()
        Dim str As String = "Update TbAna set  AnaPiva=@AnaPiva, AnaCFis=@AnaCFis, AnaPivaEst=@AnaPivaEst WHERE AnaCod=@AnaCod"
        Dim p1 As New SqlParameter("@AnaPiva", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@AnaCFis", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@AnaPivaEst", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@AnaCod", SqlDbType.VarChar)

        'Dim ChangeTable As DataTable = TbDErr.GetChanges(DataRowState.Modified)
        'If ChangeTable Is Nothing Then Exit Sub
        If TbDErr Is Nothing Then Exit Sub

        For x As Int16 = 1 To TbDErr.Rows.Count
            Rw = TbDErr.Rows(x - 1)

            If Rw("PerrEscludi") = False Then

                p1.Value = Rw("PErrPiva")
                p2.Value = Rw("PerrCFis")
                p3.Value = Rw("PErrPivaEst")
                p4.Value = Rw("PErrClifor")

                Cmd = New SqlCommand(str, cnVd)
                Cmd.Parameters.Add(p1)
                Cmd.Parameters.Add(p2)
                Cmd.Parameters.Add(p3)
                Cmd.Parameters.Add(p4)

                Cmd.ExecuteNonQuery()
                Cmd.Parameters.Clear()
            End If
        Next
        'DsPoe.AcceptChanges()
        PAGINVIO = False
    End Sub
    'Private Sub AdvBandedGridView2_ShownEditor(ByVal sender As Object, ByVal e As System.EventArgs)
    '    GroupControl42.Text = AdvBandedGridView2.FocusedColumn.ToolTip
    'End Sub
#End Region
#Region " 6a CREAZIONE FILE INVIO"
    Sub VIPagina()
        LeggiAnagraficaAzienda()
        GiaFatti()
        NomeFiles()
    End Sub
    Sub LeggiAnagraficaAzienda()
        For j As Int16 = 0 To 5 : AC(j) = "" : Next


        CheckEdit1.Checked = False : GroupControl30.Enabled = False : GroupControl31.Enabled = False : TextEdit12.EditValue = Nothing

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
        TextEdit13.EditValue = Nothing
    End Sub
    Private Sub GiaFatti()
        If Cloud = True Then
            PathEle = "\\TSCLIENT\C\ELECF" & ComboBoxEdit1.EditValue & "\"
        Else
            PathEle = "C:\ELECF" & ComboBoxEdit1.EditValue & "\"
        End If
        If Not Directory.Exists(PathEle) Then
            Directory.CreateDirectory(PathEle)
        End If

        Dim df As New DirectoryInfo(PathEle)

        Dim TbF As DataTable = New DataTable("f")
        TbF.Columns.Add("Percorso", Type.GetType("System.String"))
        TbF.Columns.Add("DataOra", Type.GetType("System.String"))

        For Each ff As FileInfo In df.GetFiles
            Rw = TbF.NewRow
            Rw("Percorso") = ff.FullName
            Rw("DataOra") = ff.LastWriteTime.ToString
            TbF.Rows.Add(Rw)
        Next

        GridControl6.DataSource = TbF
        GridView4.ClearSelection()
        GridView4.OptionsSelection.EnableAppearanceFocusedRow = False
    End Sub
    Private Sub NomeFiles()
        If Cloud = True Then
            PathEle = "\\TSCLIENT\C\ELECF" & ComboBoxEdit1.EditValue & "\"
        Else
            PathEle = "C:\ELECF" & ComboBoxEdit1.EditValue & "\"
        End If

        If Not Directory.Exists(PathEle) Then
            Directory.CreateDirectory(PathEle)
        End If

        ProgrFile = ProgInvio
        ProgrFile = ProgrFile + 1
        ProgrDTE = ProgrFile
        FileEle = PathEle & "IT" & TextEdit3.EditValue & "_DF_" & Format(ProgrFile, "00000") & ".XML"
        TextEdit1.EditValue = FileEle
        ProgrFile = ProgrFile + 1
        ProgrDTR = ProgrFile
        FileEle = PathEle & "IT" & TextEdit3.EditValue & "_DF_" & Format(ProgrFile, "00000") & ".XML"
        TextEdit22.EditValue = FileEle
    End Sub
    Sub AggiornaCreazioneFiles()
        PAGFILES = False
    End Sub
    Private Sub RadioGroup3_EditValueChanged(sender As Object, e As System.EventArgs) Handles CheckEdit1.CheckedChanged
        GroupControl30.Enabled = CheckEdit1.Checked
        GroupControl31.Enabled = CheckEdit1.Checked
        If CheckEdit1.Checked = True Then
            TextEdit12.Focus()
        End If
    End Sub
#End Region
#Region "GESTIONE FONDO PAGINA"
    Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        If PAGINVIO = True Then AggiornaDatiEsteri()
        If IsNothing(RadioGroup1.EditValue) Then
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            LeggiPeriodo()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            PaginaII()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 2 Then
            PaginaVIII()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 4 Then
            VisIntegrativi()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 5 Then
            IVPagina() : PAGINVIO = True
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 7 Then
            TextEdit22.ResetBackColor()
            VIPagina()
            Exit Sub
        End If
    End Sub
#End Region
#Region "XML FATTURE"
    Private Sub ButtonF11_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF11.Click
        If CheckEdit1.Checked = True Then
            If TextEdit12.Text.Trim = "" Then
                TextEdit12.Focus()
                Exit Sub
            End If
            If TextEdit13.Text.Trim = "" Then
                TextEdit13.Focus()
                Exit Sub
            End If
        End If
        creazioneXMLFatture()
        TextEdit1.Focus()
        TextEdit22.BackColor = Color.Yellow
        If XtraTabPage3.PageVisible = True Then
            XtraTabControl1.SelectedTabPageIndex = 2
        End If
    End Sub
    Private Sub creazioneXMLFatture()
        Cursor.Current = Cursors.WaitCursor
        CreaFatture()
        ControlloRighe()

        'se ci sono segnalazioni esco dalla procedura
        If TbR.Rows.Count > 0 Then
            Exit Sub
        End If

        FattureDTE()
        FattureDTR()

        registraprogressivo()
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub CreaFatture()
        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)
        Dim P5 As New SqlParameter("@NO_CONTROL", SqlDbType.Bit)

        P1.Value = ANNO
        P2.Value = RadioGroup1.EditValue
        P5.Value = CheckEdit2.Checked


        DaDte = New SqlDataAdapter("EXEC X_FATTXML_DTE_EST @ANNO, @PERIODO,@NO_CONTROL", cnCo)
        DaDte.SelectCommand.Parameters.Add(P1)
        DaDte.SelectCommand.Parameters.Add(P2)
        DaDte.SelectCommand.Parameters.Add(P5)

        TbDte = New DataTable("DTE")
        DaDte.Fill(TbDte)

        Dim P3 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P4 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)
        Dim P6 As New SqlParameter("@NO_CONTROL", SqlDbType.Bit)

        P3.Value = ANNO
        P4.Value = RadioGroup1.EditValue
        P6.Value = CheckEdit2.Checked

        DaDtr = New SqlDataAdapter("EXEC X_FATTXML_DTR_EST @ANNO, @PERIODO,@NO_CONTROL", cnCo)
        DaDtr.SelectCommand.Parameters.Add(P3)
        DaDtr.SelectCommand.Parameters.Add(P4)
        DaDtr.SelectCommand.Parameters.Add(P6)

        TbDtr = New DataTable("DTR")
        DaDtr.Fill(TbDtr)
    End Sub
    Private Sub FattureDTE()
        'Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        'Dim P2 As New SqlParameter("@PERIODO", SqlDbType.TinyInt)

        'P1.Value = ANNO
        'P2.Value = RadioGroup1.EditValue

        'DaDte = New SqlDataAdapter("EXEC X_FATTXML_DTE @ANNO, @PERIODO", cnCo)
        'DaDte.SelectCommand.Parameters.Add(P1)
        'DaDte.SelectCommand.Parameters.Add(P2)

        'TbDte = New DataTable("DTE")
        'DaDte.Fill(TbDte)

        If TbDte.Rows.Count = 0 Then
            Exit Sub
        End If

        TestataDTE()
    End Sub
    Private Sub TestataDTE()
        FileEle = TextEdit1.EditValue


        If File.Exists(Trim(FileEle)) Then
            File.Delete(Trim(FileEle))
        End If

        Xtw = New XmlTextWriter(FileEle, System.Text.Encoding.UTF8)
        Xtw.Formatting = Formatting.Indented
        Xtw.Indentation = 2
        Xtw.WriteStartDocument()
        Xtw.WriteStartElement("ns2:DatiFattura")
        Xtw.WriteAttributeString("xmlns:ns2", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v2.0")
        'Xtw.WriteAttributeString("xmlns", "ds", Nothing, "http://www.w3.org/2000/09/xmldsig#")
        'Xtw.WriteAttributeString("NamespaceSchemaLocation", "http://www.w3.org/2000/09/xmldsig#", "xmldsig-core-schema.xsd")
        'Xtw.WriteAttributeString("targetNamespace", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v2.0")
        Xtw.WriteAttributeString("versione", "DAT20")

        CaricaHeader(ProgrDTE)

        caricaDTE()

        Xtw.WriteEndElement()
        Xtw.WriteEndDocument()
        Xtw.Close()
    End Sub
    Private Sub caricaDTE()
        Xtw.WriteStartElement("DTE")

        Xtw.WriteStartElement("CedentePrestatoreDTE")

        'Id Fiscali
        Xtw.WriteStartElement("IdentificativiFiscali")

        Xtw.WriteStartElement("IdFiscaleIVA")
        Xtw.WriteElementString("IdPaese", "IT")
        Xtw.WriteElementString("IdCodice", TextEdit3.Text.Trim)
        Xtw.WriteEndElement()

        Xtw.WriteElementString("CodiceFiscale", TextEdit2.Text.Trim)

        Xtw.WriteEndElement()


        'Altri identificativi.
        Xtw.WriteStartElement("AltriDatiIdentificativi")

        If TextEdit4.Text.Trim = "" Then
            Xtw.WriteElementString("Denominazione", TextEdit9.Text.Trim)
        Else

            Xtw.WriteElementString("Nome", TextEdit5.Text.Trim)
            Xtw.WriteElementString("Cognome", TextEdit4.Text.Trim)
        End If

        Xtw.WriteEndElement()

        'Cedente prestatore
        Xtw.WriteEndElement()


        ' COMMITTENTI
        CliFor = ""
        Registro = 0
        NumProt = 0

        For i As Int16 = 0 To TbDte.Rows.Count - 1
            Rw = TbDte.Rows(i)
            If Rw("CliFor") <> CliFor Then
                If CliFor <> "" Then
                    Xtw.WriteEndElement()
                    Xtw.WriteEndElement()
                End If

                CaricaCessionarioCommittenteDTE()
            End If
            If Rw("Registro") <> Registro Or Rw("NumProt") <> NumProt Then
                If Registro <> 0 And NumProt <> 0 Then
                    If CliFor = Rw("cLIfOR") Then
                        Xtw.WriteEndElement()
                    End If

                End If
                CliFor = Rw("CliFor")
                Registro = Rw("Registro")
                NumProt = Rw("NumProt")
                CaricaDatiFatturaDTE()
            End If
            RiepilogoDTE()
        Next


        'Dati Fattura
        Xtw.WriteEndElement()

        'Cessionario Committente
        Xtw.WriteEndElement()



        Xtw.WriteEndElement()
    End Sub

    Private Sub CaricaCessionarioCommittenteDTE()
        Xtw.WriteStartElement("CessionarioCommittenteDTE")

        'Id Fiscali
        Xtw.WriteStartElement("IdentificativiFiscali")



        If Rw("IdCodice").ToString.Trim <> "" Then
            Xtw.WriteStartElement("IdFiscaleIVA")
            Xtw.WriteElementString("IdPaese", Rw("IdPaese"))
            Xtw.WriteElementString("IdCodice", Rw("IdCodice"))
            Xtw.WriteEndElement()
        End If

        If Rw("CodiceFiscale").ToString.Trim <> "" Then
            Xtw.WriteElementString("CodiceFiscale", Rw("CodiceFiscale"))
        End If


        Xtw.WriteEndElement()

        If CheckEdit2.Checked = True Then
            Exit Sub
        End If

        'Altri identificativi.
        Xtw.WriteStartElement("AltriDatiIdentificativi")

        If Rw("Cognome").ToString.Trim = "" Then
            Xtw.WriteElementString("Denominazione", Rw("Denominazione"))
        Else
            Xtw.WriteElementString("Nome", Rw("Nome"))
            Xtw.WriteElementString("Cognome", Rw("Cognome"))
        End If

        Xtw.WriteStartElement("Sede")
        Xtw.WriteElementString("Indirizzo", Rw("Indirizzo"))
        If Rw("CAP").trim <> "" Then
            Xtw.WriteElementString("CAP", Rw("CAP"))
        End If
        Xtw.WriteElementString("Comune", Rw("Comune"))
        If Rw("Provincia").trim <> "" Then
            Xtw.WriteElementString("Provincia", Rw("Provincia"))
        End If
        Xtw.WriteElementString("Nazione", Rw("Nazione"))
        Xtw.WriteEndElement()

        Xtw.WriteEndElement()
    End Sub
    Private Sub CaricaDatiFatturaDTE()
        'DATI FATTURA
        Xtw.WriteStartElement("DatiFatturaBodyDTE")

        'Dati Generali
        Xtw.WriteStartElement("DatiGenerali")
        Xtw.WriteElementString("TipoDocumento", Rw("TipoDocumento"))
        Xtw.WriteElementString("Data", Rw("Data"))
        Xtw.WriteElementString("Numero", Rw("Numero"))
        Xtw.WriteEndElement()

    End Sub
    Private Sub RiepilogoDTE()
        Xtw.WriteStartElement("DatiRiepilogo")

        Xtw.WriteElementString("ImponibileImporto", Rw("ImponibileImporto").ToString.Replace(",", "."))

        Xtw.WriteStartElement("DatiIVA")
        Xtw.WriteElementString("Imposta", Rw("Imposta").ToString.Replace(",", "."))
        Xtw.WriteElementString("Aliquota", Rw("Aliquota").ToString.Replace(",", "."))
        Xtw.WriteEndElement()

        If Rw("Imposta") = 0 Then
            Xtw.WriteElementString("Natura", Rw("Natura"))
        End If
        'If Rw("Detraibile" & i.ToString) > 0 Then
        '    Xtw.WriteElementString("Detraibile", Rw("Detraibile" & i.ToString))
        'End If
        'If Rw("Deducibile" & i.ToString) = True Then
        '    Xtw.WriteElementString("Deducibile", "SI")
        'End If
        If Rw("EsigibilitaIVA").trim <> "" Then
            Xtw.WriteElementString("EsigibilitaIVA", Rw("EsigibilitaIVA"))
        End If

        Xtw.WriteEndElement()
    End Sub

    '''''' FORNITORI
    Private Sub FattureDTR()
        'Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        'Dim P2 As New SqlParameter("@PERIODO", SqlDbType.TinyInt)

        'P1.Value = ANNO
        'P2.Value = RadioGroup1.EditValue

        'DaDtr = New SqlDataAdapter("EXEC X_FATTXML_DTR @ANNO, @PERIODO", cnCo)
        'DaDtr.SelectCommand.Parameters.Add(P1)
        'DaDtr.SelectCommand.Parameters.Add(P2)

        'TbDtr = New DataTable("DTR")
        'DaDtr.Fill(TbDtr)

        If TbDtr.Rows.Count = 0 Then
            Exit Sub
        End If

        TestataDTR()
    End Sub
    Private Sub TestataDTR()
        FileEle = TextEdit22.EditValue

        If File.Exists(Trim(FileEle)) Then
            File.Delete(Trim(FileEle))
        End If

        Xtw = New XmlTextWriter(FileEle, System.Text.Encoding.UTF8)
        Xtw.Formatting = Formatting.Indented
        Xtw.Indentation = 2
        Xtw.WriteStartDocument()
        Xtw.WriteStartElement("ns2:DatiFattura")
        Xtw.WriteAttributeString("xmlns:ns2", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v2.0")
        'Xtw.WriteAttributeString("xmlns", "ds", Nothing, "http://www.w3.org/2000/09/xmldsig#")
        'Xtw.WriteAttributeString("NamespaceSchemaLocation", "http://www.w3.org/2000/09/xmldsig#", "xmldsig-core-schema.xsd")
        'Xtw.WriteAttributeString("targetNamespace", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v2.0")
        Xtw.WriteAttributeString("versione", "DAT20")


        CaricaHeader(ProgrDTR)

        caricaDTR()


        Xtw.WriteEndElement()
        Xtw.WriteEndDocument()
        Xtw.Close()
    End Sub
    Private Sub caricaDTR()
        Xtw.WriteStartElement("DTR")

        Xtw.WriteStartElement("CessionarioCommittenteDTR")

        'Id Fiscali
        Xtw.WriteStartElement("IdentificativiFiscali")

        Xtw.WriteStartElement("IdFiscaleIVA")
        Xtw.WriteElementString("IdPaese", "IT")
        Xtw.WriteElementString("IdCodice", TextEdit3.Text.Trim)
        Xtw.WriteEndElement()

        Xtw.WriteElementString("CodiceFiscale", TextEdit2.Text.Trim)

        Xtw.WriteEndElement()


        'Altri identificativi.
        Xtw.WriteStartElement("AltriDatiIdentificativi")

        If TextEdit4.Text.Trim = "" Then
            Xtw.WriteElementString("Denominazione", TextEdit9.Text.Trim)
        Else

            Xtw.WriteElementString("Nome", TextEdit5.Text.Trim)
            Xtw.WriteElementString("Cognome", TextEdit4.Text.Trim)
        End If


        'Xtw.WriteStartElement("Sede")
        'Xtw.WriteElementString("Indirizzo", Rw("Indirizzo"))
        'Xtw.WriteElementString("Comune", Rw("Comune"))
        'Xtw.WriteElementString("Nazione", Rw("Nazione"))
        'Xtw.WriteEndElement()

        Xtw.WriteEndElement()

        'Cessionario Committente
        Xtw.WriteEndElement()



        ' CEDENTI/PRESTATORI
        CliFor = ""
        Registro = 0
        NumProt = 0

        For i As Int16 = 0 To TbDtr.Rows.Count - 1
            Rw = TbDtr.Rows(i)
            If Rw("CliFor") <> CliFor Then
                If CliFor <> "" Then
                    Xtw.WriteEndElement()
                    Xtw.WriteEndElement()
                End If

                CaricaCedentePrestatoreDTR()
            End If
            If Rw("Registro") <> Registro Or Rw("NumProt") <> NumProt Then
                If Registro <> 0 And NumProt <> 0 Then
                    If CliFor = Rw("cLIfOR") Then
                        Xtw.WriteEndElement()
                    End If

                End If
                CliFor = Rw("CliFor")
                Registro = Rw("Registro")
                NumProt = Rw("NumProt")
                CaricaDatiFatturaDTR()
            End If
            RiepilogoDTR()
        Next


        'Dati Fattura
        Xtw.WriteEndElement()


        'Cedente prestatore
        Xtw.WriteEndElement()

        Xtw.WriteEndElement()
    End Sub
    Private Sub CaricaCedentePrestatoreDTR()
        Xtw.WriteStartElement("CedentePrestatoreDTR")

        'Id Fiscali
        Xtw.WriteStartElement("IdentificativiFiscali")


        If Rw("IdCodice").ToString.Trim <> "" Then
            Xtw.WriteStartElement("IdFiscaleIVA")
            Xtw.WriteElementString("IdPaese", Rw("IdPaese"))
            Xtw.WriteElementString("IdCodice", Rw("IdCodice"))
            Xtw.WriteEndElement()
        End If

        If Rw("CodiceFiscale").ToString.Trim <> "" Then
            Xtw.WriteElementString("CodiceFiscale", Rw("CodiceFiscale"))
        End If

        Xtw.WriteEndElement()

        If CheckEdit2.Checked = True Then
            Exit Sub
        End If

        'Altri identificativi.
        Xtw.WriteStartElement("AltriDatiIdentificativi")

        If Rw("Cognome").ToString.Trim = "" Then
            Xtw.WriteElementString("Denominazione", Rw("Denominazione"))
        Else
            Xtw.WriteElementString("Nome", Rw("Nome"))
            Xtw.WriteElementString("Cognome", Rw("Cognome"))
        End If
        Xtw.WriteStartElement("Sede")
        Xtw.WriteElementString("Indirizzo", Rw("Indirizzo"))

        If Rw("CAP").trim <> "" Then
            Xtw.WriteElementString("CAP", Rw("CAP"))
        End If
        Xtw.WriteElementString("Comune", Rw("Comune"))
        If Rw("Provincia").trim <> "" Then
            Xtw.WriteElementString("Provincia", Rw("Provincia"))
        End If
        Xtw.WriteElementString("Nazione", Rw("Nazione"))
        Xtw.WriteEndElement()

        Xtw.WriteEndElement()

        'Xtw.WriteEndElement()
    End Sub
    Private Sub CaricaDatiFatturaDTR()
        'DATI FATTURA
        Xtw.WriteStartElement("DatiFatturaBodyDTR")

        'Dati Generali
        Xtw.WriteStartElement("DatiGenerali")
        Xtw.WriteElementString("TipoDocumento", Rw("TipoDocumento"))
        Xtw.WriteElementString("Data", Rw("Data"))
        Xtw.WriteElementString("Numero", Rw("Numero"))
        Xtw.WriteElementString("DataRegistrazione", Rw("DataRegistrazione"))
        Xtw.WriteEndElement()

    End Sub
    Private Sub RiepilogoDTR()
        Xtw.WriteStartElement("DatiRiepilogo")

        Xtw.WriteElementString("ImponibileImporto", Rw("ImponibileImporto").ToString.Replace(",", "."))

        Xtw.WriteStartElement("DatiIVA")
        Xtw.WriteElementString("Imposta", Rw("Imposta").ToString.Replace(",", "."))
        Xtw.WriteElementString("Aliquota", Rw("Aliquota").ToString.Replace(",", "."))
        Xtw.WriteEndElement()

        If Rw("Natura") <> "" Then
            Xtw.WriteElementString("Natura", Rw("Natura"))
        End If
        'If Rw("Detraibile" & i.ToString) > 0 Then
        '    Xtw.WriteElementString("Detraibile", Rw("Detraibile" & i.ToString))
        'End If
        'If Rw("Deducibile" & i.ToString) = True Then
        '    Xtw.WriteElementString("Deducibile", "SI")
        'End If
        If Rw("EsigibilitaIVA").trim <> "" Then
            Xtw.WriteElementString("EsigibilitaIVA", Rw("EsigibilitaIVA"))
        End If


        Xtw.WriteEndElement()
    End Sub

    Private Sub CaricaHeader(progr As Int32)
        Xtw.WriteStartElement("DatiFatturaHeader")

        Xtw.WriteElementString("ProgressivoInvio", progr)

        If CheckEdit1.Checked = True Then
            Xtw.WriteStartElement("Dichiarante")

            Xtw.WriteElementString("CodiceFiscale", TextEdit12.EditValue.ToString)
            Xtw.WriteElementString("Carica", TextEdit13.EditValue.ToString)

            Xtw.WriteEndElement()
        End If



        Xtw.WriteEndElement()
    End Sub
    Private Sub registraprogressivo()
        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@PERIODO", SqlDbType.SmallInt)
        Dim P3 As New SqlParameter("@PROGINVIO", SqlDbType.Int)
        Dim P4 As New SqlParameter("@NO_CONTROL", SqlDbType.Bit)

        P1.Value = ANNO
        P2.Value = RadioGroup1.EditValue
        P3.Value = ProgrFile
        P4.Value = CheckEdit2.Checked


        Cmd = New SqlCommand("UPDATE TbRegXML_EST set RxInviato = 1, RxNoControl = @NO_CONTROL where RxAnno = @ANNO and RxPeriodo = @PERIODO", cnCo)
        Cmd.Parameters.Add(P1)
        Cmd.Parameters.Add(P2)
        Cmd.Parameters.Add(P4)
        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()

        Cmd = New SqlCommand("UPDATE TbRegXML_EST set RxProgInvio = @PROGINVIO", cnCo)
        Cmd.Parameters.Add(P3)
        Cmd.ExecuteNonQuery()



        Inviato = True
        ProgInvio = ProgrFile
        GiaFatti()
        NomeFiles()
        AbilitaDisabilita()
    End Sub
#End Region
#Region "CONTROLLO NATURE INCONGRUENTI"
    Private Sub ControlloRighe()

        DaR = New SqlDataAdapter("SELECT * FROM VTMPFTERRORE", cnCo)
        TbR = New DataTable("ERR")
        DaR.Fill(TbR)

        GridControl52.DataSource = TbR
        GridView52.ClearSelection()
        GridView52.OptionsSelection.EnableAppearanceFocusedRow = False

        If TbR.Rows.Count = 0 Then
            XtraTabPage3.PageVisible = False
        Else
            XtraTabPage3.PageVisible = True
        End If
    End Sub
#End Region



    Private Sub RepositoryItemCheckEdit2_Enter(sender As Object, e As System.EventArgs) Handles RepositoryItemCheckEdit2.Enter
        RepositoryItemCheckEdit2.ReadOnly = (GridView2.GetFocusedDataRow("ABILITATO") = False)
    End Sub


End Class