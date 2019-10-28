Imports DXBASE
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports System.IO

Public Class LDPRiparto

    Dim ERifAnno As Integer = 0
    Dim ERifRiva As Integer = 0
    Dim sw As Int16 = 0
    Dim Rispondi As MsgBoxResult

    Dim Ri As String = "REGI"
    Dim DsReg As DataSet
    Dim DaReg As SqlDataAdapter
    Dim RwReg As DataRow
    Dim RwAGG As DataRow

    Dim DsMcc As DataTable
    Dim DaMcc As SqlDataAdapter
    Dim RwMcc As DataRow
    Dim RwW As DataRow
    Dim RwV As DataRowView
    Dim DsFpn As DataTable
    Dim DaFpn As SqlDataAdapter
    Dim RwFpn As DataRow


    Dim DsCpt As DataTable
    Dim DaCpt As SqlDataAdapter
    Dim RwCpt As DataRow

    Dim Totale, Parziale, Ripartire(1) As Decimal
    Dim Residuo As Boolean = False
    Dim Filter As String = ""

    Dim ds As DataSet
    Dim TbPcm As DataTable
    Dim DaPcm As SqlDataAdapter
    Dim TbCcm As DataTable
    Dim DaCcm As SqlDataAdapter

    Dim Xreg As Int16 = -1
    Dim ZxZ As Integer = -1
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim nx As New DevExpress.XtraEditors.Controls.ComboBoxItem

    Dim Liv2(), Liv3(), Commesse(), RADIO As Int16
    Dim Liv1() As String

    Private Sub LDPRiparto_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(0)
        If sw = 0 Then
            'Cursor.Current = Cursors.WaitCursor
            'EsegueSql("XCRONRIP", cnDb)
            'Cursor.Current = Cursors.Default
            Apertura() : LDPeRepertorio() : RadioGroup1.SelectedIndex = 0
            sw = 1
        End If
        RADIO = RadioGroup1.SelectedIndex
        If RadioGroup1.SelectedIndex > -1 Then RadioGroup1.SelectedIndex = -1 : RadioGroup1.SelectedIndex = RADIO Else RadioGroup1.SelectedIndex = 0
        'If ERifProt > 0 Then
        '    TextEdit1.EditValue = ERifProt
        '    TextEdit2.EditValue = ERifBis
        '    If ControllaProtocollo(ERifProt) = True Then
        '        errorT(0) = TextEdit1.ErrorText
        '        errorT(1) = DateEdit1.ErrorText
        '        SelectNextControl(TextEdit4, True, True, True, True)
        '        TextEdit1.ErrorText = errorT(0)
        '        DateEdit1.ErrorText = errorT(1)
        '    End If
        '    ERifProt = 0 : ERifBis = "" : ERifRiva = 0 : ERifAnno = 0
        'End If
    End Sub

    Sub Pulizia(ByVal p As Int16)
        Totale = 0 : Parziale = 0 : TextEdit1.EditValue = 0 : TextEdit2.EditValue = 0 : TextEdit3.EditValue = 0 : RadioGroup2.SelectedIndex = 2
        DsMcc = New DataTable
        DsFpn = New DataTable
        DsCpt = New DataTable
        GridControl4.DataSource = DsMcc
        GridControl3.DataSource = DsFpn
        GridControl2.DataSource = DsCpt
        GroupControl5.Enabled = True
        RadioGroup1.Enabled = True
        ButtonF11.Enabled = False : ButtonDEL.Enabled = False
        ButtonPLUS.Enabled = False : ButtonHelp.Enabled = False
        CheckButton1.Visible = False
        GridControl2.Text = ""
    End Sub
    Sub Apertura()
        Dim Cmd As New SqlCommand("SELECT distinct RivaAnno from TbRegIva Order by RivaAnno desc", cnCo)
        ComboBoxEdit2.Properties.Items.Clear()
        Dim x As Int16 = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit2.Properties.Items.Add(dataRd.Item("RIvaAnno"))
        End While
        dataRd.Close()
        If ComboBoxEdit2.Properties.Items.Count = 0 Then Exit Sub
        ComboBoxEdit2.SelectedIndex = -1
        For x = 0 To ComboBoxEdit2.Properties.Items.Count - 1
            If ERifAnno > 0 And ComboBoxEdit2.Properties.Items(x) = ERifAnno Then
                PopolaRegime(x)
                Exit Sub
            End If
            If ComboBoxEdit2.Properties.Items(x) = CDate(Today.Date).Year And ERifAnno = 0 Then
                PopolaRegime(x)
                Exit Sub
            End If
        Next
        If ComboBoxEdit2.SelectedIndex = -1 Then PopolaRegime(0)
    End Sub
    Sub LDPeRepertorio()
        Dim ii(0) As Integer
        Dim i As Int16 = 0
        Dim n As Int16 = 0
        Cmd = New SqlCommand("Select * from TbLdp  order by LdpSigla", CnDc)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            nx = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("LdpSigla"))
            RepositoryItemComboBox3.Items.Add(nx)
            ReDim Preserve Commesse(i)
            Commesse(i) = dataRd.Item("LdpRif")
            i += 1
        End While
        dataRd.Close()
        'Repertorio al momento non gestiito ( solo gruppo Pasta )
        RepositoryItemComboBox1.Items.Clear()
        Exit Sub
        'FINE Repertorio al momento non gestiito ( solo gruppo Pasta )
        Cmd = New SqlCommand("SELECT * FROM VRepertorio where Tipo = 3", cnDb)
        dataRd = Cmd.ExecuteReader

        While dataRd.Read
            nx = New DevExpress.XtraEditors.Controls.ComboBoxItem(dataRd.Item("VOCE"))
            RepositoryItemComboBox1.Items.Add(nx)
        End While
        dataRd.Close()
    End Sub
    Sub PopolaRegime(ByVal i As Int16)
        ComboBoxEdit2.SelectedIndex = i
        Dim x As Int16
        RileggoUltimi()
        If DsReg.Tables(Ri).Rows.Count = 0 Then
            Messaggio(1, "REGISTRI IVA INESISTENTI !!!")
            Me.Close()
            Exit Sub
        End If
        ImageComboBoxEdit1.Properties.Items.Clear()
        ImageComboBoxEdit1.SelectedIndex = -1
        Xreg = -1 '' DsReg.Tables(Ri).Rows.Count - 1 '' SELEZIONO IVA ACQUISTI
        For x = 1 To DsReg.Tables(Ri).Rows.Count
            RwReg = DsReg.Tables(Ri).Rows(x - 1)
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(RwReg("RivaNreg").ToString.PadLeft(2, "0") & " " & RwReg("RivaDesc"), RwReg("RivaNreg").ToString.PadLeft(2, "0"), -1)
            ImageComboBoxEdit1.Properties.Items.Add(nn)
            If ERifRiva > 0 And RwReg("RivaNreg") = ERifRiva Then
                Xreg = x - 1
            ElseIf Xreg = -1 And RwReg("RivaTipo") = 2 Then
                Xreg = x - 1
            End If
        Next
        ImageComboBoxEdit1.SelectedIndex = Xreg : ERifRiva = ImageComboBoxEdit1.EditValue
    End Sub
    Sub RileggoUltimi()
        ' LEGGO SOLO I RGISTRI IVA DI TIPO 1, 2, 3, 4 (Vendite,Acquisti,Rett.Vendite,Rett.Acquisti)
        Dim Str As String = "Select * from FnFotoRIva(" & Val(ComboBoxEdit2.EditValue) & ") Where RivaTipo < 5  ORDER BY RivaNReg"
        DsReg = New DataSet(Ri)
        DaReg = New SqlDataAdapter(Str, cnCo)
        DaReg.Fill(DsReg, Ri)
        '' AGGIUNGO I REG.IVA NON MOVIMENTATI
        Dim P As Int16
        Dim StrReg As String = "SELECT * from TbRegIva where RivaTipo < 5 and RivaAnno = " & Val(ComboBoxEdit2.EditValue) & " ORDER BY RivaNReg"
        Dim Cmd As New SqlCommand(StrReg, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            For P = 1 To DsReg.Tables(Ri).Rows.Count
                RwReg = DsReg.Tables(Ri).Rows(P - 1)
                If RwReg("RivaNReg") = dataRd.Item("RivaNReg") Then
                    GoTo DopoLet
                End If
            Next
            RwReg = DsReg.Tables(Ri).NewRow
            RwReg("RivaNReg") = dataRd.Item("RivaNReg")
            RwReg("RivaTipo") = dataRd.Item("RivaTipo")
            If dataRd.Item("RivaTipo") = 1 Then
                RwReg("TipoDesc") = "Vendite"
            ElseIf dataRd.Item("RivaTipo") = 2 Then
                RwReg("TipoDesc") = "Acquisti"
            ElseIf dataRd.Item("RivaTipo") = 3 Then
                RwReg("TipoDesc") = "Rett.Vendite"
            ElseIf dataRd.Item("RivaTipo") = 4 Then
                RwReg("TipoDesc") = "Rett.Acquisti"
            Else : RwReg("TipoDesc") = ""
            End If
            RwReg("RivaDesc") = dataRd.Item("RivaDesc")
            RwReg("ProtCar") = 0
            RwReg("DataCar") = Today.ToShortDateString
            RwReg("ProtSta") = 0
            RwReg("DataSta") = Today.ToShortDateString
            RwReg("RivaPrintIniziale") = dataRd.Item("RivaPrintIniziale")
            RwReg("RivaCpt") = dataRd.Item("RivaCpt")
            RwReg("RivaAutoFCee") = dataRd.Item("RivaAutoFCee")
            RwReg("RivaCptCee") = dataRd.Item("RivaCptCee")
            RwReg("RivaCliCee") = dataRd.Item("RivaCliCee")
            If dataRd.Item("RIvaRCharge") Is DBNull.Value Then RwReg("RivaRcharge") = False Else RwReg("RivaRcharge") = CBool(dataRd.Item("RIvaRCharge"))
            DsReg.Tables(Ri).Rows.Add(RwReg)
            DsReg.Tables(Ri).AcceptChanges()
DopoLet:
        End While
        dataRd.Close()
    End Sub
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "INSERIMENTO CENTRI DI COSTO"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Private Sub ComboBoxEdit2_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ComboBoxEdit2.SelectedIndexChanged
        If sw > 0 And ComboBoxEdit2.SelectedIndex > -1 And RadioGroup1.SelectedIndex > -1 Then GridFatture(RadioGroup1.SelectedIndex)
    End Sub
    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex > -1 Then GridFatture(RadioGroup1.SelectedIndex)
    End Sub
    Private Sub ImageComboBoxEdit1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ImageComboBoxEdit1.SelectedIndexChanged
        If ImageComboBoxEdit1.SelectedIndex > -1 Then
            ERifRiva = ImageComboBoxEdit1.EditValue
            RadioGroup1.SelectedIndex = -1 : RadioGroup1.SelectedIndex = 0
        End If
    End Sub
    Private Sub RadioGroup2_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioGroup2.SelectedIndexChanged
        If RadioGroup2.SelectedIndex > -1 Then
            Select Case RadioGroup2.SelectedIndex
                Case 0
                    GridView3.ActiveFilterString = "[NC] = 0"
                Case 1
                    GridView3.ActiveFilterString = "[NC] = 1"
                Case 2
                    GridView3.ActiveFilterString = ""
            End Select
        End If
    End Sub
    Sub GridFatture(n As Int16)
        Cursor = Cursors.WaitCursor
        Dim Str As String = "exec XGRIDFAT @ANNO=" & ComboBoxEdit2.EditValue & ",@REG = " & ERifRiva
        GridColumn15.VisibleIndex = 8
        GridColumn24.Caption = "Nr. Prot."
        GridColumn28.Visible = True
        GridColumn15.Visible = True
        GridColumn13.Visible = False
        If n = 1 Then
            Str = "exec XGRIDPNOTA @ANNO=" & ComboBoxEdit2.EditValue
            ImageComboBoxEdit1.SelectedIndex = -1 : ERifRiva = 0
            GridColumn24.Caption = "Nr. PNota"
            GridColumn28.Visible = False
            GridColumn15.Visible = False
            GridColumn13.Visible = True
        End If
        DsFpn = New DataTable
        DaFpn = New SqlDataAdapter(Str, CnDc)
        DaFpn.Fill(DsFpn)
        GridControl3.DataSource = DsFpn
        GridView3.ClearSelection()
        Cursor = Cursors.Default
    End Sub

    Private Sub GridView3_Click(sender As Object, e As System.EventArgs) Handles GridView3.Click
        Dim x As Int16 = 0
        If GridView3.FocusedColumn.Name = "GridColumn15" Then
            DisplayDoc(GridView3.FocusedColumn.FieldName, GridView3.FocusedRowHandle)
        End If

    End Sub
    Function DisplayDoc(ByVal TipoS As String, ByVal RifG As Integer) As Boolean
        Dim DTipo As String = "FF"
        Dim FD As FileInfo
        Dim NomeN As String = ""
        Rw = GridView3.GetDataRow(RifG)
        Select Case TipoS
            Case Is = "S1"
                FD = New FileInfo(PathDoc & CDate(Rw("PriDataEst")).Year & "\" & DTipo & Rw("FRifInterno") & ".pdf")
                NomeN = Rw("CConto") & " " & Rw("AnaDesc") & " FATTURA NR. " & Rw("PriDocEst") & " DEL " & Rw("PriDataEst")
                Dim Gesterna As New DxDisplay
                DxDisplay.Percorso = FD
                DxDisplay.NOMET = NomeN
                Gesterna.ShowDialog()
        End Select
    End Function
    Private Sub GridView3_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView3.RowClick
        If e.RowHandle > -1 Then
            RwFpn = GridView3.GetDataRow(e.RowHandle)
            DaRipartire()
            GridView4.Focus()
        End If
    End Sub
    Sub DaRipartire()
        Dim Str = "exec XRIPARTO @PRIID=" & RwFpn("PriId")
        DsMcc = New DataTable
        DaMcc = New SqlDataAdapter(Str, CnDc)
        DaMcc.Fill(DsMcc)
        GridControl4.DataSource = DsMcc : ZxZ = -1
        GridView4.ClearSelection()

        GroupControl5.Enabled = False
        RadioGroup1.Enabled = False

        EsegueSql("EXEC XMONDOCPT @ID=" & RwFpn("PriId"), CnDc)
        Str = "SELECT * FROM ##TACPT ORDER BY COGCONTO "
        DsCpt = New DataTable()
        DaCpt = New SqlDataAdapter(Str, CnDc)
        DaCpt.Fill(DsCpt)
        GridControl2.DataSource = DsCpt
        GridView2.ClearSelection()
        RwMcc = GridView4.GetDataRow(0)
        TextEdit1.EditValue = RwMcc("PriId") : TextEdit2.EditValue = RwMcc("MCCID") : TextEdit3.EditValue = RwFpn("FdcgNumRif")
        ''If TextEdit3.EditValue > 0 Then ButtonHelp.Enabled = True Else ButtonHelp.Enabled = False -- per ora SOLO PASTAGROUP
        GroupControl2.Text = RwMcc("PriDescrizione")
        If RwMcc("PriArtFisc") > 0 Then
            CheckButton1.Visible = True
            GridColumn19.OptionsColumn.ReadOnly = True : GridColumn19.OptionsColumn.AllowEdit = False
        Else
            CheckButton1.Visible = False
            GridColumn19.OptionsColumn.ReadOnly = False : GridColumn19.OptionsColumn.AllowEdit = True
        End If
        CaricaTotali()
    End Sub
    Sub CaricaTotali()
        Dim y, q As Int16
        Parziale = 0 : Residuo = False : Ripartire(0) = 0 : Ripartire(1) = 0
        For q = 1 To DsCpt.Rows.Count
            RwCpt = DsCpt.Rows(q - 1)
            RwCpt("RIPARTITO") = 0
            RwCpt("RESIDUO") = 0
        Next
        DsCpt.AcceptChanges()
        For y = 1 To DsMcc.Rows.Count
            RwMcc = DsMcc.Rows(y - 1)
            Parziale = Parziale + RwMcc("MCCImporto")
            For q = 1 To DsCpt.Rows.Count
                RwCpt = DsCpt.Rows(q - 1)
                If RwMcc("MCCCOGCONTO") = RwCpt("COGCONTO") And RwMcc("DAREAVERE") = RwCpt("DAREAVERE") And RwMcc("MCCCogLdp") > -1 Then ''-- a + livelli deve essere  RwMcc("MCCCogDet") >0
                    RwCpt("RIPARTITO") = RwCpt("RIPARTITO") + RwMcc("MCCIMPORTO")
                    RwCpt("RESIDUO") = RwCpt("IMPORTO") - RwCpt("RIPARTITO")
                ElseIf RwMcc("MCCCOGCONTO") = RwCpt("COGCONTO") And RwMcc("DAREAVERE") = RwCpt("DAREAVERE") And RwMcc("MCCCogLdp") < 0 Then ''-- a + livelli deve essere  RwMcc("MCCCogDet") =0
                    RwCpt("RIPARTITO") = 0
                    RwCpt("RESIDUO") = RwCpt("IMPORTO")
                End If
            Next
        Next
        For q = 1 To DsCpt.Rows.Count
            RwCpt = DsCpt.Rows(q - 1)
            RwCpt("RESIDUO") = RwCpt("IMPORTO") - RwCpt("RIPARTITO")
            If RwCpt("RESIDUO") <> 0 Then Residuo = True
            If RwCpt("DAREAVERE") = "D" Then Ripartire(0) += RwCpt("RIPARTITO") Else Ripartire(1) += RwCpt("RIPARTITO")
        Next
        DsCpt.AcceptChanges()
        If Residuo = False Then
            ButtonF11.Enabled = True
            ButtonDEL.Enabled = True
            ButtonPLUS.Enabled = False
        Else
            ButtonF11.Enabled = False
            ButtonDEL.Enabled = False
            If (Ripartire(0) - Ripartire(1)) <> 0 Then ButtonPLUS.Enabled = True Else ButtonPLUS.Enabled = False
        End If
    End Sub
    Sub AggiungiRigaMcc()
        For q = 1 To DsCpt.Rows.Count
            RwCpt = DsCpt.Rows(q - 1)
            If RwCpt("RESIDUO") <> 0 Then
                For i = 1 To DsMcc.Rows.Count
                    RwMcc = DsMcc.Rows(i - 1)
                    If DsMcc.Rows(i - 1).Item("MCCCOGCONTO") = RwCpt("COGCONTO") Then
                        RwMcc = DsMcc.NewRow()
                        For J As Int16 = 1 To DsMcc.Rows(i - 1).ItemArray.Length
                            RwMcc(J - 1) = DsMcc.Rows(i - 1).Item(J - 1)
                        Next
                        RwMcc("MCCPROG") = 0 : RwMcc("MCCIMPORTO") = RwCpt("RESIDUO") : RwMcc("CONTODES") = RwCpt("CONTODESC")
                        DsMcc.Rows.Add(RwMcc)
                        DsMcc.AcceptChanges()
                        Exit Sub
                    End If
                Next
                RwMcc = DsMcc.NewRow()
                For J As Int16 = 1 To DsMcc.Rows(DsMcc.Rows.Count - 1).ItemArray.Length
                    RwMcc(J - 1) = DsMcc.Rows(DsMcc.Rows.Count - 1).Item(J - 1)
                Next
                RwMcc("MCCPROG") = 0 : RwMcc("MCCIMPORTO") = RwCpt("RESIDUO") : RwMcc("MCCCOGCONTO") = RwCpt("COGCONTO") : RwMcc("CONTODES") = RwCpt("CONTODESC")
                DsMcc.Rows.Add(RwMcc)
                DsMcc.AcceptChanges()
                CheckButton1.Visible = False
                GridColumn19.OptionsColumn.ReadOnly = False : GridColumn19.OptionsColumn.AllowEdit = True
            End If
        Next
    End Sub


#Region "GESTIONE GRID RIPARTIZIONE"

    Private Sub RepositoryItemTextEdit1_Leave(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit1.Leave
        If GridView4.IsLastRow = True Then
            GridView4.DataController.BeginCurrentRowEdit()
            GridView4.UpdateCurrentRow()
            System.Windows.Forms.SendKeys.Send("{TAB}")
            GridControl2.Focus()
        End If
    End Sub
    Private Sub GridView4_RowUpdated(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowObjectEventArgs) Handles GridView4.RowUpdated
        If e.Row Is Nothing Then Exit Sub
        RwV = e.Row
        If RwV("VOCE") Is DBNull.Value Then RwV("VOCE") = ""
        If RwV("LDPSIGLA") Is DBNull.Value Then RwV("LDPSIGLA") = ""
        If RwV("MCCIMPORTO") Is DBNull.Value Then RwV("MCCIMPORTO") = 0
        RwMcc = GridView4.GetFocusedDataRow()
        AssegnaRepertorio()
        CaricaTotali()

    End Sub
    Sub AssegnaRepertorio()
        RwMcc("MccCogCdc") = Mid(RwMcc("VOCE").ToString, 1, 1)
        RwMcc("MccCogRep") = Val(Mid(RwMcc("VOCE").ToString, 3, 2))
        RwMcc("MccCogDet") = Val(Mid(RwMcc("VOCE").ToString, 6, 2))
        For I As Int16 = 1 To RepositoryItemComboBox3.Items.Count
            If RwMcc("LDPSIGLA") = RepositoryItemComboBox3.Items(I - 1) Then RwMcc("MccCogLdp") = Commesse(I - 1) : Exit For
        Next
    End Sub
#End Region
    Private Sub ButtonPLUS_Click(sender As System.Object, e As System.EventArgs) Handles ButtonPLUS.Click
        AggiungiRigaMcc()
        CaricaTotali()
    End Sub
    Private Sub ButtonDEL_Click(sender As System.Object, e As System.EventArgs) Handles ButtonDEL.Click
        If Controlla(1) = False Then Exit Sub
        RegistraRiparto()
        Pulizia(0)
        GridFatture(RadioGroup1.SelectedIndex)
    End Sub
    Private Sub ButtonF11_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF11.Click
        If Controlla(0) = False Then Exit Sub
        RegistraRiparto()
        Pulizia(0)
        GridFatture(RadioGroup1.SelectedIndex)
    End Sub
    Function Controlla(n As Int16) As Boolean
        Dim Mail As String = ""
        For I = 1 To DsMcc.Rows.Count
            RwMcc = DsMcc.Rows(I - 1)
            If n = 1 Then RwMcc("VOCE") = "0.00.00"
            '' If RwMcc("VOCE").ToString.Length = 0 And RwMcc("MCCIMPORTO") <> 0 And n = 0 Then Mail &= "Riga " & I & " MANCA REPERTORIO" & Chr(10) --- PER ORA GRUPPO PASTA
            If RwMcc("LDPSIGLA").ToString.Length = 0 And RwMcc("MCCIMPORTO") <> 0 Then Mail &= "Riga " & I & " MANCA LINEA DI PRODOTTO" & Chr(10)
        Next
        If Mail > "" Then
            MoltoCritico(Mail)
            Return False
            Exit Function
        End If
        Return True
    End Function

    Sub RegistraRiparto()
        Dim ChangeTable As DataTable = DsMcc ''.GetChanges(DataRowState.Modified)
        If ChangeTable Is Nothing Then Exit Sub
        Dim RwCha As DataRow
        Dim MMCwrite As String = "INSERT INTO TbMCC (MCCID,MCCPROG,MCCPrkAammgg,MCCPrkId,MCCPrkProg,MCCPrkDa,MCCCogLdp,MCCCogCdc,MCCCogRep,MCCCogDet,MCCCogConto,MCCIMPORTO) " _
& " values(@MCCID,@MCCPROG,@MCCPrkAammgg,@MCCPrkId,@MCCPrkProg,@MCCPrkDa,@MCCCogLdp,@MCCCogCdc,@MCCCogRep,@MCCCogDet,@MCCCogConto,@MCCIMPORTO)"
        Dim Mmd As New SqlCommand(MMCwrite, CnDc)
        Dim mp1 As New SqlParameter("@MCCID", SqlDbType.Int)
        Dim mp2 As New SqlParameter("@MCCPROG", SqlDbType.SmallInt)
        Dim mp3 As New SqlParameter("@MCCPrkAammgg", SqlDbType.SmallDateTime)
        Dim mp4 As New SqlParameter("@MCCPrkId", SqlDbType.Int)
        Dim mp5 As New SqlParameter("@MCCPrkProg", SqlDbType.SmallInt)
        Dim mp6 As New SqlParameter("@MCCPrkDa", SqlDbType.VarChar)
        Dim mp7 As New SqlParameter("@MCCCogLdp", SqlDbType.SmallInt)
        Dim mp8 As New SqlParameter("@MCCCogCdc", SqlDbType.VarChar)
        Dim mp9 As New SqlParameter("@MCCCogRep", SqlDbType.SmallInt)
        Dim mp9a As New SqlParameter("@MCCCogDet", SqlDbType.SmallInt)
        Dim mp10 As New SqlParameter("@MCCCogConto", SqlDbType.VarChar)
        Dim mp11 As New SqlParameter("@MCCIMPORTO", SqlDbType.Decimal)
        Dim MMCdelete As New SqlCommand("DELETE FROM TBMCC WHERE MCCID = " & TextEdit2.EditValue, CnDc)

        MMCdelete.ExecuteNonQuery()
        For I As Int16 = 1 To DsMcc.Rows.Count
            RwCha = DsMcc.Rows(I - 1)
            If RwCha("MCCIMPORTO") <> 0 Then
                mp1.Value = RwCha("MCCID")
                mp2.Value = I
                mp3.Value = RwCha("MCCPrkAammgg")
                mp4.Value = RwCha("MCCPrkId")
                mp5.Value = RwCha("MCCPrkProg")
                mp6.Value = RwCha("MCCPrkDa")
                mp7.Value = RwCha("MCCCogLdp")
                '' LA VOCE = 0 PERCHE NON VIENE GESTITA --- SOLO PASTAGROUP PER ORA
                mp8.Value = "" ''Mid(RwCha("VOCE").ToString, 1, 1)
                mp9.Value = 0 ''CInt(Mid(RwCha("VOCE").ToString, 3, 2))
                mp9a.Value = 0 ''CInt(Mid(RwCha("VOCE").ToString, 6, 2))


                mp10.Value = RwCha("MCCCogConto")
                mp11.Value = RwCha("MCCIMPORTO")

                Mmd.Parameters.Clear()
                Mmd.Parameters.Add(mp1)
                Mmd.Parameters.Add(mp2)
                Mmd.Parameters.Add(mp3)
                Mmd.Parameters.Add(mp4)
                Mmd.Parameters.Add(mp5)
                Mmd.Parameters.Add(mp6)
                Mmd.Parameters.Add(mp7)
                Mmd.Parameters.Add(mp8)
                Mmd.Parameters.Add(mp9)
                Mmd.Parameters.Add(mp9a)
                Mmd.Parameters.Add(mp10)
                Mmd.Parameters.Add(mp11)
                Mmd.ExecuteNonQuery()
            End If
        Next
    End Sub
    Sub MoltoCritico(ByVal Mail As String)
        Dim response As MsgBoxResult
        response = MsgBox(Mail, MsgBoxStyle.Critical, "CONTROLLO INSERIMENTO")
    End Sub


    Private Sub ButtonHelp_Click(sender As System.Object, e As System.EventArgs) Handles ButtonHelp.Click
        If TextEdit3.EditValue > 0 Then
            Dim Gesterna As New DxCronArgo
            DxCronArgo.NRif = Val(TextEdit3.EditValue)
            Gesterna.ShowDialog()
        End If
    End Sub


    Private Sub SimpleButton1_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton1.Click
        LetturaDataset(RadioGroup1.SelectedIndex)
        'GridColumn15.VisibleIndex = 8
        'GridColumn13.VisibleIndex = 9 : GridColumn13.Visible = True : GridColumn14.VisibleIndex = 10 : GridColumn14.Visible = True
        'DXANTEPRIMA(GridControl3, True, Printing.PaperKind.A4, "")
        'GridColumn15.VisibleIndex = 8
        'GridColumn13.Visible = False
    End Sub
    Sub LetturaDataset(n)
        Cursor = Cursors.WaitCursor
        Dim Str As String = "exec XGRIDFAT @ANNO=" & ComboBoxEdit2.EditValue & ",@REG = " & ERifRiva
        Dim SStr As String = "exec XGRIDFATPRINT @ANNO=" & ComboBoxEdit2.EditValue & ",@REG = " & ERifRiva
        Try
            GridView1.ActiveFilterString = GridView3.ActiveFilterString
        Catch ex As Exception
            GridView1.ActiveFilterString = ""
        End Try

        If n = 1 Then
            Str = "exec XGRIDPNOTAONE @ANNO=" & ComboBoxEdit2.EditValue
            SStr = "exec XGRIDPNOTAPRINT @ANNO=" & ComboBoxEdit2.EditValue
            ImageComboBoxEdit1.SelectedIndex = -1 : ERifRiva = 0
            Try
                GridView7.ActiveFilterString = GridView3.ActiveFilterString
            Catch ex As Exception
                GridView7.ActiveFilterString = ""
            End Try
        End If
        ds = New DataSet
        TbPcm = New DataTable()
        DaPcm = New SqlDataAdapter(Str, CnDc)
        DaPcm.Fill(ds, "TbPcm")
        TbCcm = New DataTable()
        DaCcm = New SqlDataAdapter(SStr, CnDc)
        DaCcm.Fill(ds, "TbCcm")
        ds.Relations.Add("Dettaglio", ds.Tables("TbPcm").Columns("PriId"), ds.Tables("TbCcm").Columns("PRIID"))
        If n = 1 Then
            GridControl5.DataSource = ds
            GridControl5.DataMember = "TbPcm"
            DXANTEPRIMA(GridControl5, True, Printing.PaperKind.A4, "")
        Else
            GridControl1.DataSource = ds
            GridControl1.DataMember = "TbPcm"
            DXANTEPRIMA(GridControl1, True, Printing.PaperKind.A4, "")
        End If
        Cursor = Cursors.Default
    End Sub

    Private Sub SimpleButton2_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton2.Click
        Dim INTESTA As String = "PRIMA NOTA"
        If ImageComboBoxEdit1.SelectedIndex > 0 Then
            INTESTA = DirectCast(ImageComboBoxEdit1.SelectedItem, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Description
        End If
        DXANTEPRIMA(GridControl3, True, Printing.PaperKind.A4, INTESTA)

    End Sub
End Class