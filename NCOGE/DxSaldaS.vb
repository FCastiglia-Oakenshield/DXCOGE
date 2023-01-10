Imports DXBASE
Imports NCCOM
Imports DevExpress.XtraEditors
Imports System.Data.SqlClient
Public Class DxSaldaS
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

    Dim Ts As String = "TSCO"
    Dim DsSco As DataSet
    Dim DaSco As SqlDataAdapter
    Dim RwSco As DataRow

    Dim Pn As String = "NOTA"
    Dim DsPno As DataSet
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
    Dim StrUno, StrDue, StrTre, StrPrint, LIMITI(6), StrD(10), StrLim, LimD, LimA As String

    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim Em As New DevExpress.XtraEditors.Controls.CheckedListBoxItem
    Dim RwX As DataRow
    Dim RwY As DataRow


    Private Sub DxSaldaS_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If Sw = 0 Then
            PrimoMiglio()
            Sw = 1
        End If
        Pulizia()
        If RadioGroup1.SelectedIndex = -1 Then RadioGroup1.SelectedIndex = 1
    End Sub
    Sub PrimoMiglio()
        Dim Cmd As New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT TOP 1 EseArtGiroconto FROM TbEse ORDER BY EseAnno DESC", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ArtIrpef = dataRd.Item("EseArtGiroconto")
        End While
        dataRd.Close()
        LIMITI(1) = "000.00"
        LIMITI(2) = "101000"
        LIMITI(3) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(4) = "099.99"
        LIMITI(5) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(6) = "199999"
        StrD(0) = " SELECT * FROM CRG1 WHERE PRKPAPERTA = 0 AND SCARAPERTA <> 1 AND SCADSCA BETWEEN "
        StrD(1) = " AND SCATIPOCO+SCACONTO BETWEEN "
        StrD(2) = " AND SCATPAG = "
        StrD(5) = " ORDER BY SCADSCA,SCACONTO"
        StrD(9) = "SELECT *,GetDate() as DtValuta,GetDate() as DtDoc,'0' as FoAbi,'0' as Focab,'0' as FoCC,'' as AnaIndirizzo,'' as AnaCap,'' as AnaCitta,'' as AnaProv, '0' AS FoPaese, '0' as FoCinEur, '0' as FoCin ,0 as ScaNRata FROM VB8SC WHERE PRKCONTO = "
        StrD(10) = " ORDER BY PRKDOCANN,PRKDOCEST"
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
        DsSco = New DataSet
        GridControl3.DataSource = DsSco.Tables(Ts)
        GridControl3.Refresh()
    End Sub
    Sub PulisciPNota()
        REM inizializzo il dataset Vuoto
        Dim StrS = StrD(9) & "'zzzzzz'" & StrD(10)
        DsPno = New DataSet
        DaPno = New SqlDataAdapter(StrS, cnCo)
        DaPno.Fill(DsPno, Pn)
        GridControl4.DataSource = DsPno.Tables(Pn)
        GridControl4.Refresh()
        TextEdit1.EditValue = CDec(0.0)
        PPulisci()
    End Sub
    Sub PPulisci()
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = ""
        TextEdit6.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = CDec(0.0)
        TextEdit8.Properties.ReadOnly = True
    End Sub
    Sub Pulizia()
        PulisciGrid()
        PulisciPNota()
        DisBottoni()
        PPulisci()
        StrLim = ""
        CF = ""
        Scor = 0
        TextEdit8.ErrorText = ""
        CheckEdit1.Checked = False
        CheckEdit2.Checked = False
        TextEdit1.EditValue = CDec(0.0)
    End Sub
    Sub EseguoOperazione()
        PulisciGrid()
        Scor = 0
        Cursor.Current = Cursors.WaitCursor
        FormaStringaTesta()
        CaricaScoperti()
        Cursor.Current = Cursors.Default
    End Sub
    Sub CaricaScoperti()
        DsSco = New DataSet
        DaSco = New SqlDataAdapter(StrUno, cnCo)
        DaSco.SelectCommand.CommandTimeout = 300
        DaSco.Fill(DsSco, Ts)
        Scor = DsSco.Tables(Ts).Rows.Count
        GridControl3.DataSource = DsSco.Tables(Ts)
        GridControl3.Refresh()
    End Sub
    Sub FormaLimitiStandard()
        If RadioGroup1.Properties.Items(RadioGroup1.SelectedIndex).Value = "CL" Then
            LimD = LIMITI(2)
            LimA = LIMITI(5)
        Else
            If RadioGroup1.Properties.Items(RadioGroup1.SelectedIndex).Value = "FO" Then
                LimD = LIMITI(3)
                LimA = LIMITI(6)
            Else
                Exit Sub
            End If
        End If
    End Sub
    Sub FormaStringaTesta()
        FormaLimitiStandard()
        StrLim = "'" & LimD & "' AND '" & LimA & "'"
        StrUno = StrD(0) & "'" & DateEdit3.EditValue & "' AND '" & DateEdit4.EditValue & "' " & StrD(1) & StrLim
        Dim Ands As String = ""
        Dim StrPrint = ""
        Dim Ors As String = " OR SCATPAG = "
        Dim C As Int16 = 0
        For x = 1 To CheckedComboBoxEdit3.Properties.Items.Count
            Em = CheckedComboBoxEdit3.Properties.Items(x - 1)
            If Em.CheckState = CheckState.Checked Then
                Ands &= Ors & Em.Value
                If Em.Value = "5" Then TP = 5
                C += 1
            End If
        Next
        If Ands.Length = 0 Then TP = 0 : GoTo Glob
        If C <> 1 Then TP = 0
        Ors = " AND ( " & Mid(Ands, 5, Ands.Length - 4) & " ) "
        StrPrint &= Ors
Glob:
        StrUno = StrUno & StrPrint
        StrUno = StrUno & StrD(5)
    End Sub
    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex > -1 Then
            CF = RadioGroup1.Properties.Items(RadioGroup1.SelectedIndex).Value
            EseguoOperazione()
        End If
    End Sub

    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        EseguoOperazione()
    End Sub
    Private Sub RepositoryItemCheckEdit3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemCheckEdit3.CheckedChanged
        RwY = GridView3.GetFocusedDataRow
        RwY("PrkPAperta") = sender.checked
        Totalizza()
    End Sub
    Sub Totalizza()
        Dim x, z As Int16
        z = -1
        TextEdit8.ErrorText = ""
        For x = 1 To DsPno.Tables(Pn).Rows.Count
            RwPno = DsPno.Tables(Pn).Rows(x - 1)
            If RwPno("PrkConto") = RwY("ScaConto") And RwPno("PrkDocAnn") = CDate(RwY("ScaDDoc")).Year And RwPno("PrkDocEst") = RwY("ScaNDoc") And RwPno("ScaNRata") = RwY("ScaNRata") Then
                z = x - 1
                Exit For
            End If
        Next
Dopo:
        If z = -1 Then
            RwPno = DsPno.Tables(Pn).NewRow()

            RwPno("PrkConto") = RwY("ScaConto")
            RwPno("PrkDocAnn") = CDate(RwY("ScaDDoc")).Year
            RwPno("PrkDocEst") = RwY("ScaNDoc")
            RwPno("PrkPAperta") = RwY("PrkPAperta")
            RwPno("AnaDesc") = RwY("ScaDesc")
            RwPno("SCOPERTO") = RwY("ScaScopRata")
            RwPno("DtValuta") = CDate(RwY("ScaDDoc"))
            RwPno("DtDoc") = CDate(RwY("ScaDDoc"))
            RwPno("FoAbi") = "00000"
            RwPno("FoCab") = "00000"
            RwPno("FoCC") = "000000000000"
            RwPno("ScaNRata") = RwY("ScaNRata")
            DsPno.Tables(Pn).Rows.Add(RwPno)
            DsPno.AcceptChanges()
            TextEdit4.EditValue = RwPno("PrkConto")
            TextEdit5.EditValue = RwPno("AnaDesc")
            TextEdit6.EditValue = RwPno("PrkDocAnn")
            TextEdit7.EditValue = RwPno("PrkDocEst")
            TextEdit8.EditValue = RwPno("SCOPERTO")
            TextEdit8.Properties.ReadOnly = False
            If CF = "" Then
                If RwPno("PrkConto") > MiglioFo Then CF = "FO" Else CF = "CL"
            End If
            '''' eventuale ricerca se fornitore soggetto ritenuta dell'importo al netto della ritenuta'''
            If CF = "FO" Then ControllaImportoFo()
            GoTo Oltre
        End If
        If RwY("PrkPAperta") = False Then
            RwPno.Delete()
            DsPno.AcceptChanges()
            PPulisci()
        End If
Oltre:
        TotaleIn()
    End Sub
    Sub ControllaImportoFo()
        Dim Str = "SELECT * from VRITNETTO WHERE RITCODFOR = '" & RwPno("PrkConto") & "' AND RITPROTFAT = " & RwPno("PrkDocEst") & " AND DATEPART(YEAR,RITDATAFAT) = " & RwPno("PrkDocAnn")
        Dim NETTO As Decimal = 0
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            NETTO = dataRd.Item("NETTO")
        End While
        dataRd.Close()
        If NETTO > 0 And CDec(TextEdit8.EditValue) > NETTO Then
            TextEdit8.EditValue = NETTO
            RwPno("SCOPERTO") = NETTO
            TextEdit8.ErrorText = "IMPORTO AL NETTO DELLA RITENUTA! "
        Else
            TextEdit8.ErrorText = ""
        End If
    End Sub
    Sub TotaleIn()
        Dim x As Int16
        Dim Progress As Decimal = 0
        For x = 1 To DsPno.Tables(Pn).Rows.Count
            Rw = GridView4.GetDataRow(x - 1)
            Progress = Progress + CDec(Rw("SCOPERTO"))
        Next
        GridView4.FocusedRowHandle = DsPno.Tables(Pn).Rows.Count - 1
        TextEdit1.EditValue = CDec(Progress)
        If TextEdit8.Properties.ReadOnly = False Then TextEdit8.Focus()
        If CDec(TextEdit1.EditValue) <> 0 Or DsPno.Tables(Pn).Rows.Count > 0 Then
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
    Private Sub ShowHitInfo4(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl4.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset4 = hi.RowHandle
    End Sub
    Private Sub GridView4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.Click
        QRIGA = -1
        If iset4 >= 0 And iset4 < DsPno.Tables(Pn).Rows.Count Then
            RwPno = DsPno.Tables(Pn).Rows(iset4)
            EliminaCheck()
            RwPno.Delete()
            DsPno.AcceptChanges()
            PPulisci()
            TotaleIn()
        End If
    End Sub
    Sub EliminaCheck()
        Dim X As Int16
        For X = 1 To DsSco.Tables(Ts).Rows.Count
            RwY = GridView3.GetDataRow(X - 1)
            If RwPno("PrkConto") = RwY("ScaConto") And RwPno("PrkDocAnn") = CDate(RwY("ScaDDoc")).Year And RwPno("PrkDocEst") = RwY("ScaNDoc") And RwPno("ScaNRata") = RwY("ScaNRata") Then
                RwY("PrkpAperta") = False
                Exit Sub
            End If
        Next
    End Sub
    Private Sub TbLeggi1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi1.Enter
        If LeggiConto(TextEdit20.EditValue, TextEdit21) = False Then TextEdit20.Focus() Else ButtonFF11.Focus()
    End Sub
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        If TextEdit20.ContainsFocus = True Then
            Dim Nc As String = ""
            Nc = EstraiRicerca(TextEdit20.EditValue.ToUpper)
            If Nc > "00.00" Then
                TextEdit20.EditValue = Nc
                LeggiConto(TextEdit20.EditValue, TextEdit21)
                SelectNextControl(ButtonFF11, True, True, True, True)
            End If
            Exit Sub
        End If
    End Sub
    Function EstraiRicerca(ByVal Tipo As String) As String
        Dim frm As New RicercaClFo
        Dim CF As String = ""
        If Tipo <> "F" And Tipo <> "C" Then
            EstraiRicerca = Query.CercaPia()
            Exit Function
        End If
        If Tipo = "F" Then CF = "FO"
        If Tipo = "C" Then CF = "CL"
        frm.StartPosition = FormStartPosition.Manual
        frm.Location = New Point(GridControl3.Location.X, GridControl3.Location.Y + 80)
        frm.CliFor = CF
        frm.ShowDialog()
        EstraiRicerca = frm.Codice
    End Function
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
        If e.KeyData = Keys.F8 And GroupControl8.Enabled = True Then
            e.Handled = True
            ButtonF8.PerformClick()
            Exit Sub
        End If

    End Sub
    Private Sub ButtonFF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF11.Click
        If Lettura = True Then Exit Sub
        If DsPno.Tables(Pn).Rows.Count = 0 Then
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
        RegistraMovimenti()
        Dim cke As Integer
        cke = RadioGroup1.SelectedIndex
        Pulizia()
        RadioGroup1.SelectedIndex = -1
        RadioGroup1.SelectedIndex = cke
        CheckEdit1.Checked = False
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
    Sub RegistraMovimenti()
        Dim K, M As Int32
        Dim Scheggia As Int16 = 0
        Dim Articolo As Int32 = RileggoLocked()
        Dim Total As Decimal = 0
        Dim P As Int16 = 0
        LeggiUltimo(DateEdit2.EditValue)
        TextEdit2.EditValue = Articolo
        If DsPno.Tables(Pn).Rows.Count > 1 Then P = 1 Else P = 0
        M = DsPno.Tables(Pn).Rows.Count + P
        For K = 1 To DsPno.Tables(Pn).Rows.Count + P
            Wmd.Parameters.Clear()
            If K < M Or P = 0 Then
                RwPno = DsPno.Tables(Pn).Rows(K - 1)
                p12.Value = RwPno("PrkDocEst")
                p22.Value = RwPno("PrkDocAnn")
                If Val(RwPno("PrkConto")) > MiglioFo Then
                    p3.Value = RwPno("PrkConto")
                    p9.Value = RwPno("SCOPERTO")
                    p4.Value = "00.10"
                    p10.Value = 0
                    Total = Total + RwPno("SCOPERTO")
                    If P = 0 Then
                        p4.Value = TextEdit20.EditValue
                        p10.Value = RwPno("SCOPERTO")
                    End If
                Else
                    p3.Value = "00.10"
                    p9.Value = 0
                    p4.Value = RwPno("PrkConto")
                    p10.Value = RwPno("Scoperto")
                    Total = Total + RwPno("SCOPERTO")
                    If P = 0 Then
                        p3.Value = TextEdit20.EditValue
                        p9.Value = RwPno("SCOPERTO")
                    End If
                End If
            Else
                p12.Value = 0
                p22.Value = 0
                If Val(RwPno("PrkConto")) > MiglioFo Then
                    p3.Value = "00.10"
                    p9.Value = 0
                    p4.Value = TextEdit20.EditValue
                    p10.Value = Total
                Else
                    p3.Value = TextEdit20.EditValue
                    p9.Value = Total
                    p4.Value = "00.10"
                    p10.Value = 0
                End If
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
            If Val(p3.Value) > MiglioFo Then ControllaRitenute()
        Next
        SbloccoLocked()
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
        If TP = 5 And CheckEdit2.Checked = True Then
            EseguiBonifico(Total, Articolo)
        End If
        Dim ProgCdc As Int32
        ProgCdc = ProgId
        '''' SE ARTICOLO IRPEF AUTOMATICO MI SPORCA LA VARIABILE PROGID CON UN ALTRO ARTICOLO
        If ComboBoxEdit1.Properties.Items.Count > 0 Then
            PagaEGiroconta()
            ComboBoxEdit1.Properties.Items.Clear()
        End If
    End Sub
    Sub EseguiBonifico(ByVal TotaleD As Decimal, ByVal NArt As Int32)
        Dim Gesterna As New CfBoni
        Gesterna.Total = TotaleD
        Gesterna.PNota = NArt
        Gesterna.DataOp = DateEdit1.EditValue
        Gesterna.DataSet = DsPno
        Gesterna.ShowDialog()
    End Sub
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
        If CheckEdit1.Checked = True Then
            CheckEdit1.Checked = False
        ElseIf Scor > 0 Then
            SaldaTutto()
        Else
            PulisciPNota()
            DisBottoni()
        End If
    End Sub

    Private Sub CheckedComboBoxEdit3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckedComboBoxEdit3.TextChanged
        If Sw = 1 Then
            EseguoOperazione()
            If Scor > 0 And CheckEdit1.Checked = True Then SaldaTutto()
        End If
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit1.CheckedChanged
        If Scor > 0 Then SaldaTutto()
    End Sub
    Sub SaldaTutto()
        PulisciPNota()
        Dim Q As Int16
        For Q = 1 To DsSco.Tables(Ts).Rows.Count
            RwSco = DsSco.Tables(Ts).Rows(Q - 1)
            If CheckEdit1.Checked = True Then
                RwPno = DsPno.Tables(Pn).NewRow()
                RwPno("PrkConto") = RwSco("ScaConto")
                RwPno("PrkDocAnn") = CDate(RwSco("ScaDDoc")).Year
                RwPno("PrkDocEst") = RwSco("ScaNDoc")
                RwSco("PrkPAperta") = 1
                RwPno("PrkPAperta") = RwSco("PrkPAperta")
                RwPno("AnaDesc") = RwSco("ScaDesc")
                RwPno("SCOPERTO") = RwSco("ScaScopRata")
                RwPno("DtValuta") = CDate(RwSco("ScaDDoc"))
                RwPno("DtDoc") = CDate(RwSco("ScaDDoc"))
                RwPno("FoAbi") = "00000"
                RwPno("FoCab") = "00000"
                RwPno("FoCC") = "000000000000"
                RwPno("FoPaese") = "00"
                RwPno("FoCinEur") = "00"
                RwPno("FoCin") = "0"
                RwPno("ScaNRata") = RwSco("ScaNRata")

                If CF = "" Then
                    If RwPno("PrkConto") > MiglioFo Then CF = "FO" Else CF = "CL"
                End If
                TextEdit8.EditValue = RwPno("SCOPERTO")
                '''' eventuale ricerca se fornitore soggetto ritenuta dell'importo al netto della ritenuta'''
                If CF = "FO" Then ControllaImportoFo()
                DsPno.Tables(Pn).Rows.Add(RwPno)
            Else
                RwSco("PrkPAperta") = 0
            End If
        Next
        DsPno.AcceptChanges()
        TotaleIn()

    End Sub
    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        If CheckEdit2.Checked = True And TP = 5 Then Exit Sub
        CheckEdit2.Checked = False
    End Sub

    Private Sub ButtonXF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF11.Click
        If ModificaImporto() = False Then TextEdit8.Focus() Else ImageComboBoxEdit2.Focus()
    End Sub

    Function ModificaImporto() As Boolean
        ModificaImporto = True
        If CDec(TextEdit8.EditValue) = 0 Then
            Exit Function
        End If
        If (RwPno("Scoperto") > 0 And CDec(TextEdit8.EditValue) < 0) Or (RwPno("Scoperto") < 0 And CDec(TextEdit8.EditValue) > 0) Then
            ModificaImporto = False
            Exit Function
        End If
        If Math.Abs(CDec(TextEdit8.EditValue)) > Math.Abs(RwPno("Scoperto")) Then
            ModificaImporto = False
            Exit Function
        End If
        RwPno("Scoperto") = CDec(TextEdit8.EditValue)
        DsPno.Tables(Pn).AcceptChanges()
        PPulisci()
        TotaleIn()
    End Function
    Sub ControllaRitenute()
        Dim Str As String = "SELECT * from TBRIT WHERE RITCODFOR = '" & p3.Value & "' AND RITPROTFAT = " & p12.Value & " AND DATEPART(YEAR,RITDATAFAT) = " & p22.Value & " AND RITDATAPAG is null"
        Dim RTN As New SqlCommand(Str, cnCo)
        dataRd = RTN.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("RitNum") & "@" & dataRd.Item("RitCodFor") & "@" & dataRd.Item("RitRitenuta") & "@" & dataRd.Item("RitProtFat") & "@" & p22.Value & "@" & p14.Value)
        End While
        dataRd.Close()
    End Sub
  
    Sub PagaEGiroconta()
        Dim K As Int16
        Dim PAG As String = ""
        Dim Upd As New SqlCommand(PAG, cnCo)
        For K = 1 To ComboBoxEdit1.Properties.Items.Count
            SSPLIT = ComboBoxEdit1.Properties.Items(K - 1)
            PAG = "Update TbRit Set RITDATAPAG = '" & SSPLIT.Split("@")(5) & "' WHERE RITNUM = " & Val(SSPLIT.Split("@")(0))
            Upd = New SqlCommand(PAG, cnCo)
            Upd.ExecuteNonQuery()
            If ArtIrpef > 0 Then GiroIrpef(Val(SSPLIT.Split("@")(0)))
        Next
    End Sub
    Function GiroIrpef(ByVal NrRit As Int32) As Boolean
        Dim RitRitenuta As Decimal = 0
        Dim RitCodFor As String = ""
        Dim Ok As Boolean = False
        RitCodFor = SSPLIT.Split("@")(1)
        RitRitenuta = CDec(SSPLIT.Split("@")(2))
        If RitRitenuta = 0 Then Exit Function
        Dim StrReg As String = "SELECT top 1 * from TbArtP where ArtPId = " & ArtIrpef & " Order By ArtPprog"
        Dim Acau As Int16
        Dim AAvere As String = ""
        Dim ADesc1 As String = ""
        Dim ADesc2 As String = ""
        Dim RR As New SqlCommand(StrReg, cnCo)
        dataRd = RR.ExecuteReader
        While dataRd.Read
            Ok = True
            Acau = dataRd.Item("ArtPCausale")
            AAvere = dataRd.Item("ArtPAvere")
            ADesc1 = dataRd.Item("ArtPDesc1")
            ADesc2 = dataRd.Item("ArtPDesc2")
        End While
        dataRd.Close()
        If Ok = False Then Exit Function
        LeggiUltimo(DateEdit2.EditValue)
        Wmd.Parameters.Clear()
        Dim Articolo As Int32
        Articolo = RileggoLocked()
        p1.Value = CDate(DateEdit1.EditValue)
        p2.Value = Acau
        p3.Value = RitCodFor
        p4.Value = AAvere
        p5.Value = Articolo
        p6.Value = ""
        p7.Value = 0
        p8.Value = 0
        p9.Value = RitRitenuta
        p10.Value = RitRitenuta
        p11.Value = ADesc1
        p12.Value = Val(SSPLIT.Split("@")(3))
        p13.Value = ""
        p14.Value = CDate(SSPLIT.Split("@")(5))
        p15.Value = ADesc2
        p16.Value = 0
        p17.Value = 0
        p18.Value = 0
        p19.Value = ""
        p20.Value = ""
        p21.Value = ""
        p22.Value = Val(SSPLIT.Split("@")(4))
        p23.Value = 0
        p24.Value = 0
        p25.Value = 0
        p26.Value = ProgId
        p27.Value = 1
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
        SbloccoLocked()
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        Partita(1, 0)
    End Function
    Sub Partita(ByVal Tipo As Int16, ByVal AZ As Int32)
        If Tipo = 0 Then
            EsegueSql(" EXEC RiAprePartita  @Id = " & ProgId & ",@Az=" & AZ & ",@Miglio=" & MiglioFo, cnCo)
        Else
            EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
        End If
    End Sub

End Class