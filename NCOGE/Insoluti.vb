Imports DXBASE
Imports NCCOM
Imports DevExpress.XtraEditors
Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Grid

Public Class Insoluti

    Dim Scrivi As String = "INSERT INTO TbPri (PriId,PriProg,PriDataGio, PriCausale, PriCoDare, PriCoAvere, PriNumProt, PriBisRet, PriCodIva, PriRegIva, PriImpDare, PriImpavere, PriDesc, PriDocEst, PriMeseSk, PriDataEst, PriDescB, PriFl04, PriFl05, PriFl06, PriNsRif, PriSos, PriLinea, PriDocAnn, PriCodPag, PriValuta, PriArtFisc,PriIvaPrint,PriGStampa) " _
& " values(@PriId,@PriProg,@PriDataGio, @PriCausale, @PriCoDare, @PriCoAvere, @PriNumProt, @PriBisRet, @PriCodIva, @PriRegIva, @PriImpDare, @PriImpavere, @PriDesc, @PriDocEst, @PriMeseSk, @PriDataEst, @PriDescB, @PriFl04, @PriFl05, @PriFl06, @PriNsRif, @PriSos, @PriLinea, @PriDocAnn, @PriCodPag, @PriValuta, @PriArtFisc,@PriIvaPrint,@PriGStampa)"
    Dim Wmd As New SqlCommand(Scrivi, cnCo)
    Dim p1 As New SqlParameter("@PriDataGio", SqlDbType.SmallDateTime)
    Dim p2 As New SqlParameter("@PriCausale", SqlDbType.SmallInt)
    Dim p3 As New SqlParameter("@PriCoDare", SqlDbType.VarChar)
    Dim p4 As New SqlParameter("@PriCoAvere", SqlDbType.VarChar)
    Dim p5 As New SqlParameter("@PriNumProt", SqlDbType.Int)
    Dim p6 As New SqlParameter("@PriBisRet", SqlDbType.VarChar)
    Dim p7 As New SqlParameter("@PriCodIva", SqlDbType.SmallInt)
    Dim p8 As New SqlParameter("@PriRegIva", SqlDbType.SmallInt)
    Dim p9 As New SqlParameter("@PriImpDare", SqlDbType.Decimal)
    Dim p10 As New SqlParameter("@PriImpavere", SqlDbType.Decimal)
    Dim p11 As New SqlParameter("@PriDesc", SqlDbType.VarChar)
    Dim p12 As New SqlParameter("@PriDocEst", SqlDbType.Int)
    Dim p13 As New SqlParameter("@PriMeseSk", SqlDbType.VarChar)
    Dim p14 As New SqlParameter("@PriDataEst", SqlDbType.SmallDateTime)
    Dim p15 As New SqlParameter("@PriDescB", SqlDbType.VarChar)
    Dim p16 As New SqlParameter("@PriFl04", SqlDbType.SmallInt)
    Dim p17 As New SqlParameter("@PriFl05", SqlDbType.SmallInt)
    Dim p18 As New SqlParameter("@PriFl06", SqlDbType.SmallInt)
    Dim p19 As New SqlParameter("@PriNsRif", SqlDbType.VarChar)
    Dim p20 As New SqlParameter("@PriSos", SqlDbType.VarChar)
    Dim p21 As New SqlParameter("@PriLinea", SqlDbType.VarChar)
    Dim p22 As New SqlParameter("@PriDocAnn", SqlDbType.SmallInt)
    Dim p23 As New SqlParameter("@PriCodPag", SqlDbType.SmallInt)
    Dim p24 As New SqlParameter("@PriValuta", SqlDbType.Decimal)
    Dim p25 As New SqlParameter("@PriArtFisc", SqlDbType.Int)
    Dim p26 As New SqlParameter("@PriID", SqlDbType.Int)
    Dim p27 As New SqlParameter("@PriProg", SqlDbType.SmallInt)
    Dim p28 As New SqlParameter("@PriIvaPrint", SqlDbType.Bit)
    Dim p29 As New SqlParameter("@PriGStampa", SqlDbType.Bit)

    Dim TbSco As DataTable
    Dim DaSco As SqlDataAdapter
    Dim RwSco As DataRow

    Dim TbPno As DataTable
    Dim DaPno As SqlDataAdapter
    Dim RwPno As DataRow

    Dim StrTot, Dap, Alp, CauDes(72), CF As String
    Dim Esponi As Boolean
    Dim MaxDat, MinDat, MMMDat As Date
    Dim Rispondi As MsgBoxResult
    Dim Sw As Int16 = 0
    Dim MiglioFo, Test, Corp, Irow, Scor, ProgId, ArtIrpef, iset4 As Int32
    Dim SSPLIT As String
    Dim TP, QRIGA As Int16
    Dim StrUno, StrDue, StrTre, StrPrint As String

    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim Em As New DevExpress.XtraEditors.Controls.CheckedListBoxItem
    Dim RwX As DataRow
    Dim RwY As DataRow


    Private Sub DxSaldaS_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If Sw = 0 Then
            PrimoMiglio()
            Sw = 1
        End If
        DateEdit3.EditValue = CDate(Today).AddDays(-20)
        DateEdit4.EditValue = CDate(Today)
        Pulizia()
    End Sub
    Sub PrimoMiglio()
        Dim Cmd As New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        Dim Str As String = "SELECT * from TbCii order by CiiCod"
        Dim SS As String = ""
        ComboBoxEdit1.Properties.Items.Clear()
        ImageComboBoxEdit2.Properties.Items.Clear()
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("CiiCod") > 3 Then
                SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau")
                nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
                ImageComboBoxEdit2.Properties.Items.Add(nn)
                If Trim(dataRd.Item("CiiCau")) = "INSOLUTO" Then
                    ImageComboBoxEdit2.EditValue = dataRd.Item("CiiCod")
                End If
            End If
        End While
        dataRd.Close()
        ''' ' LEGGO DA TBPRI MAX DATAGIO E MAX NUMART
        MaxDat = Today.Date
        Str = "SELECT distinct PridataGio,PriNumProt from TbPri where PriregIva = 0 and PriNumProt = ( SELECT isnull(max(PriNumProt),0) from TbPri where PriregIva = 0 )"
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MaxDat = dataRd.Item("PridataGio")
        End While
        dataRd.Close()
        Str = "SELECT isnull(MAX(PRIDATAGIO),(select esedal from tbese where eseanno = (select MIN(aziannolavoro)from tbazi ))) FROM TBPRI WHERE PRIGSTAMPA = 1 AND PRIARTFISC > 0 "
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MinDat = dataRd.Item(0)
        End While
        dataRd.Close()
        Str = "SELECT EseAl from tbese where eseanno = (select max(aziannolavoro)from tbazi ) "
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MMMDat = dataRd.Item("EseAl")
        End While
        dataRd.Close()
        DateEdit1.EditValue = Today.Date
        DateEdit2.EditValue = Today.Date
        DateEdit3.EditValue = CDate("01/01/" & Today.Year)
        DateEdit4.EditValue = CDate(Date.DaysInMonth(Today.Year, Today.Month) & "/" & Today.Month & "/" & Today.Year)
        TP = 0

        ImageComboBoxEdit1.Properties.Items.Clear()
        Cmd = New SqlCommand(" SELECT * from TbBan where BanAttivo = 1 Order by BanCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("BanDes"), dataRd.Item("BanCod"), -1)
            ImageComboBoxEdit1.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub

    Private Sub DateEdit1_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.Validated
        DateEdit2.EditValue = DateEdit1.EditValue
    End Sub
    Private Sub DatBox2_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit2.Validated
        If CDate(DateEdit2.EditValue) > CDate(DateEdit1.EditValue) Then
            DateEdit2.EditValue = DateEdit1.EditValue
        End If
    End Sub
    Sub DisBottoni()
        GroupControl5.Enabled = True
        ButtonF5.Enabled = True
        ButtonF1.Enabled = True
        GroupControl8.Enabled = False
    End Sub
    Sub PulisciGrid()
        TbSco = New DataTable
        GridControl3.DataSource = TbSco
        GridControl3.Refresh()
    End Sub
    Sub PulisciPNota()
        REM inizializzo il dataset Vuoto
        Dim StrS = "select * from VDISTRBINS WHERE CLIENTE ='zzzzzz'"
        TbPno = New DataTable
        DaPno = New SqlDataAdapter(StrS, cnCo)
        DaPno.Fill(TbPno)
        GridControl4.DataSource = TbPno
        GridControl4.Refresh()
        TextEdit1.EditValue = CDec(0.0)

    End Sub

    Sub Pulizia()
        PulisciGrid()
        PulisciPNota()
        DisBottoni()

        CF = ""
        Scor = 0
        TextEdit1.EditValue = CDec(0.0)
    End Sub
    Sub EseguoOperazione()
        PulisciGrid()
        Scor = 0
        Cursor.Current = Cursors.WaitCursor
        CaricaScadenze()
        Cursor.Current = Cursors.Default
    End Sub
    Sub CaricaScadenze()
        Dim struno As String = "select * from VDISTRBINS where RicBan = " & ImageComboBoxEdit1.EditValue & " and SCADENZA between '" & DateEdit3.EditValue & "' and '" & DateEdit4.EditValue & "'"
        TbSco = New DataTable
        DaSco = New SqlDataAdapter(struno, cnCo)
        DaSco.SelectCommand.CommandTimeout = 300
        DaSco.Fill(TbSco)
        Scor = TbSco.Rows.Count
        GridControl3.DataSource = TbSco
        GridControl3.Refresh()
    End Sub



    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If ImageComboBoxEdit1.SelectedIndex > -1 Then
            EseguoOperazione()
        End If

    End Sub

    Private Sub RepositoryItemCheckEdit3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemCheckEdit3.CheckedChanged
        RwY = GridView3.GetFocusedDataRow
        RwY("PrkPAperta") = sender.checked
        Totalizza()
    End Sub
    'Private Sub GridView3_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView3.RowClick
    '    RwY = GridView3.GetFocusedDataRow
    '    Totalizza()
    'End Sub
    Sub Totalizza()
        Dim x, z As Int16
        z = -1
        For x = 1 To TbPno.Rows.Count
            RwPno = TbPno.Rows(x - 1)
            If RwPno("CLIENTE") = RwY("CLIENTE") And RwPno("ANNO") = RwY("ANNO") And RwPno("RicNfat") = RwY("RicNfat") And RwPno("RicNfat") = RwY("RicNfat") Then
                z = x - 1
                Exit For
            End If
        Next
Dopo:
        If z = -1 Then
            RwPno = TbPno.NewRow()

            RwPno("CLIENTE") = RwY("CLIENTE")
            RwPno("ANNO") = RwY("ANNO")
            RwPno("RicNfat") = RwY("RicNfat")
            RwPno("EMITTENTE") = RwY("EMITTENTE")
            RwPno("IMPRATA") = RwY("IMPRATA")
            RwPno("RicNRata") = RwY("RicNRata")
            RwPno("PrkPAperta") = RwY("PrkPAperta")
            TbPno.Rows.Add(RwPno)
            TbPno.AcceptChanges()
            If CF = "" Then CF = "CL"
            GoTo Oltre
        End If
        If RwY("PrkPAperta") = False Then
            RwPno.Delete()
            TbPno.AcceptChanges()
        End If
Oltre:
        TotaleIn()
    End Sub
    Sub TotaleIn()
        Dim x As Int16
        Dim Progress As Decimal = 0
        For x = 1 To TbPno.Rows.Count
            Rw = GridView4.GetDataRow(x - 1)
            Progress = Progress + CDec(Rw("IMPRATA"))
        Next
        GridView4.FocusedRowHandle = TbPno.Rows.Count - 1
        TextEdit1.EditValue = CDec(Progress)

        If CDec(TextEdit1.EditValue) <> 0 Then
            GroupControl8.Enabled = True
            GroupControl5.Enabled = False
        Else
            GroupControl8.Enabled = False
            GroupControl5.Enabled = True
        End If
    End Sub
    Private Sub GridControl4_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl4.MouseMove
        ShowHitInfo4(GridView4.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub

    Private Sub ImageComboBoxEdit1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ImageComboBoxEdit1.SelectedIndexChanged
        If ImageComboBoxEdit1.SelectedIndex > 0 Then
            Cmd = New SqlCommand("Select BanRb from TbBan where BanCod=" & ImageComboBoxEdit1.EditValue, cnCo)
            TextEdit20.EditValue = Cmd.ExecuteScalar
            LeggiConto(TextEdit20.EditValue, TextEdit21)
        Else
            TextEdit20.EditValue = "" : TextEdit21.EditValue = ""
        End If
    End Sub

    Private Sub ShowHitInfo4(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl4.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset4 = hi.RowHandle
    End Sub
    Private Sub GridView4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.Click
        QRIGA = -1
        If iset4 >= 0 And iset4 < TbPno.Rows.Count Then
            RwPno = TbPno.Rows(iset4)
            EliminaCheck()
            RwPno.Delete()
            TbPno.AcceptChanges()
            TotaleIn()
        End If
    End Sub
    Sub EliminaCheck()
        GridView3.ActiveFilterString = ""
        Dim X As Int16
        For X = 1 To TbSco.Rows.Count
            RwY = GridView3.GetDataRow(X - 1)
            If RwPno("CLIENTE") = RwY("CLIENTE") And RwPno("ANNO") = RwY("ANNO") And RwPno("RicNfat") = RwY("RicNfat") And RwPno("RicNRata") = RwY("RicNRata") Then
                RwY("PrkpAperta") = False
                Exit Sub
            End If
        Next
    End Sub
    Function LeggiConto(ByRef CodCo As String, ByRef Anagraf As TextEdit) As Boolean
        Anagraf.Text = ""
        LeggiConto = False
        AggiustaConto(CodCo) ''''' verifica il punto se e' un sottoconto
        Dim Str As String = "SELECT * from TbPia where PiaCodCo = '" & CodCo & "'"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Anagraf.Text = dataRd("PiaAnaCo")
            LeggiConto = True
        End While
        dataRd.Close()
        If LeggiConto = True Then Exit Function
        If Val(CodCo) < 1000 Then Exit Function
        CodCo = CodCo.PadLeft(5, "0")
        Str = "SELECT * from TbAna where AnaCoD = '" & CodCo & "'"
        Cmd = New SqlCommand(Str, cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Anagraf.Text = dataRd("AnaDesc")
            LeggiConto = True
        End While
        dataRd.Close()
    End Function
    Function AggiustaConto(ByRef CodCo As String) As Boolean
        Dim x As Int16
        For x = 1 To Len(CodCo)
            If Mid(CodCo, x, 1) = "." Then
                CodCo = Format(Val(Mid(CodCo, 1, x - 1)), "00") & "." & Format(Val(Mid(CodCo, x + 1, Len(CodCo) - (x - 1))), "00")
                Exit Function
            End If
        Next
    End Function
    Private Sub DxSaldaS_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 And GroupControl5.Enabled = True Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F1 And GroupControl5.Enabled = True Then
            e.Handled = True
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 And GroupControl8.Enabled = True Then
            e.Handled = True
            ButtonFF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 And GroupControl8.Enabled = True Then
            e.Handled = True
            ButtonFF11.PerformClick()
            Exit Sub
        End If

    End Sub
    Private Sub ButtonFF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF11.Click
        If TbPno.Rows.Count = 0 Then
            Messaggio(1, "MANCANO INCASSI o PAGAMENTI")
            Exit Sub
        End If
        If CDate(DateEdit1.EditValue) > MMMDat Then
            Messaggio(1, "DATA GIORNALE > MASSIMA DATA VALIDA(" & MMMDat.ToShortDateString & ")")
            DateEdit1.Focus()
            Exit Sub
        End If
        If CDate(DateEdit1.EditValue) < MinDat Then
            Messaggio(1, "DATA GIORNALE < MINIMA DATA VALIDA(" & MinDat.ToShortDateString & ")")
            DateEdit1.Focus()
            Exit Sub
        End If
        If CDate(DateEdit2.EditValue).Year < (CDate(DateEdit1.EditValue).Year - 1) Then
            Messaggio(1, "DATA OPERAZIONE NON VALIDA < ANNO MINIMO (" & (CDate(DateEdit1.EditValue).Year - 1) & ")")
            DateEdit2.Focus()
            Exit Sub
        End If

        REM controllo causale
        If ImageComboBoxEdit2.EditValue < 4 Then
            ImageComboBoxEdit2.Focus()
            Exit Sub
        End If
        If ControllaConto(TextEdit20.EditValue, TextEdit21) = False Then
            TextEdit20.Focus()
            Exit Sub
        End If
        Dim articolo As Int32 = 0
        articolo = RegistraMovimenti()
        LanciaProgramma(articolo)
        Pulizia()
    End Sub
    Sub LanciaProgramma(n)
        Dim Gesterna As New DxInPrNo
        DxInPrNo.NRifArt = n
        Gesterna.WindowState = FormWindowState.Maximized
        Gesterna.ShowDialog()
    End Sub
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "INSERIMENTO PRIMA NOTA"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Function ControllaConto(ByRef CodCo As String, ByRef Anagraf As TextEdit) As Boolean
        ControllaConto = False
        If Mid(CodCo, 3, 3) = ".00" Then Exit Function '' mastri e transitorio
        ControllaConto = LeggiConto(CodCo, Anagraf)
    End Function
    Function RegistraMovimenti()
        Dim K, M As Int32
        Dim Scheggia As Int16 = 0
        Dim Articolo As Int32 = RileggoLocked()
        Dim Total As Decimal = 0
        Dim P As Int16 = 0
        LeggiUltimo(DateEdit2.EditValue)
        TextEdit2.EditValue = Articolo
        If TbPno.Rows.Count > 1 Then P = 1 Else P = 0
        M = TbPno.Rows.Count + P
        For K = 1 To TbPno.Rows.Count + P
            Wmd.Parameters.Clear()
            If K < M Or P = 0 Then
                RwPno = TbPno.Rows(K - 1)
                p12.Value = RwPno("RicNfat")
                p22.Value = RwPno("ANNO")
                p3.Value = RwPno("CLIENTE")
                p9.Value = RwPno("IMPRATA")
                p4.Value = "00.10"
                p10.Value = 0
                Total = Total + RwPno("IMPRATA")
                If P = 0 Then
                    p4.Value = TextEdit20.EditValue
                    p10.Value = RwPno("IMPRATA")
                End If
            Else
                p12.Value = 0
                p22.Value = 0
                p3.Value = "00.10"
                p9.Value = 0
                p4.Value = TextEdit20.EditValue
                p10.Value = Total
            End If
            p1.Value = CDate(DateEdit1.EditValue)
            p2.Value = ImageComboBoxEdit2.EditValue
            p5.Value = Articolo
            p6.Value = ""
            p7.Value = 0
            p8.Value = 0
            p11.Value = ""
            p13.Value = ""
            p14.Value = CDate(DateEdit2.EditValue)
            p15.Value = ""
            p16.Value = 0
            p17.Value = 0
            p18.Value = 0
            p19.Value = ""
            p20.Value = ""
            p21.Value = ""
            p23.Value = 0
            p24.Value = 0
            p25.Value = 0
            p26.Value = ProgId
            p27.Value = K
            p28.Value = 0
            p29.Value = 0
            Wmd.Parameters.Add(p1)
            Wmd.Parameters.Add(p2)
            Wmd.Parameters.Add(p3)
            Wmd.Parameters.Add(p4)
            Wmd.Parameters.Add(p5)
            Wmd.Parameters.Add(p6)
            Wmd.Parameters.Add(p7)
            Wmd.Parameters.Add(p8)
            Wmd.Parameters.Add(p9)
            Wmd.Parameters.Add(p10)
            Wmd.Parameters.Add(p11)
            Wmd.Parameters.Add(p12)
            Wmd.Parameters.Add(p13)
            Wmd.Parameters.Add(p14)
            Wmd.Parameters.Add(p15)
            Wmd.Parameters.Add(p16)
            Wmd.Parameters.Add(p17)
            Wmd.Parameters.Add(p18)
            Wmd.Parameters.Add(p19)
            Wmd.Parameters.Add(p20)
            Wmd.Parameters.Add(p21)
            Wmd.Parameters.Add(p22)
            Wmd.Parameters.Add(p23)
            Wmd.Parameters.Add(p24)
            Wmd.Parameters.Add(p25)
            Wmd.Parameters.Add(p26)
            Wmd.Parameters.Add(p27)
            Wmd.Parameters.Add(p28)
            Wmd.Parameters.Add(p29)
            Wmd.ExecuteNonQuery()
        Next
        SbloccoLocked()
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
        Return Articolo
    End Function
    Private Function LeggiUltimo(ByVal dataGio As Date) As Boolean
        Dim ultimo As String = "INSERT INTO TbIDP (IDdata) values(@PriDataGio)"
        Dim UltimaRiga As New SqlCommand("SELECT  @@IDENTITY ", cnCo)
        Dim Qmd As New SqlCommand(ultimo, cnCo)
        Dim px As New SqlParameter("@PriDataGio", SqlDbType.SmallDateTime)
        px.Value = CDate(dataGio)
        Qmd.Parameters.Add(px)
        Qmd.ExecuteNonQuery()
        ProgId = UltimaRiga.ExecuteScalar
    End Function
    Sub ResetIdP()
        Dim Elimina As String = "DELETE FROM TbIDP WHERE IdPRI = " & ProgId
        Dim Dmd As New SqlCommand(Elimina, cnCo)
        Dmd.ExecuteNonQuery()
    End Sub
    Function SbloccoLocked() As Int16
        Dim Del As New SqlCommand("Delete from TMPlock WITH (TABLOCKX) where IdPrNota = 1 ", cnCo)
        Del.ExecuteNonQuery()
        Return 0
    End Function
    Function RileggoLocked() As Int32
        ''' pausa per scrittura articolo nuovo
        Dim Loc As New SqlCommand("Select IdPrNota  from Tmplock WITH (TABLOCKX) where IdPrNota > 0", cnCo)
        Dim Ins As New SqlCommand("Insert into TMPlock WITH (TABLOCKX) (IdPrNota) values (1) ", cnCo)
        Dim pausa As Boolean
Attesa:
        pausa = False
        dataRd = Loc.ExecuteReader
        While dataRd.Read
            pausa = True
        End While
        dataRd.Close()
        If pausa = True Then GoTo Attesa
        Ins.ExecuteNonQuery()
        ''' ' LEGGO DA TBPRI MAX DATAGIO E MAX NUMART
        Dim Str As String = "SELECT isnull(max(PriNumProt),0) from TbPri WITH (TABLOCKX) where PriregIva = 0"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            RileggoLocked = dataRd.Item(0) + 1
        End While
        dataRd.Close()
    End Function

    Private Sub ButtonFF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF5.Click
        PulisciPNota()
        DisBottoni()
    End Sub

End Class