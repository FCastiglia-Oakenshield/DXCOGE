Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports DevExpress.XtraEditors

Public Class DxSparti
    Dim Cb As String = "CMov"
    Dim DsCbo As DataSet
    Dim DaCbo As SqlDataAdapter
    Dim RwCbo As DataRow

    Dim DsSke As DataSet
    Dim DaSke As SqlDataAdapter
    Dim Ar As String = "Stampe"

    Dim Tb As String = "TMov"
    Dim DsTbo As DataTable
    Dim DaTbo As SqlDataAdapter
    Dim RwTbo As DataRow
    Dim DvRit As New DataView

    Dim RwX As DataRow
    Dim RwX2 As DataRow


    Dim Tr As String = "TRip"
    Dim DsRip As DataSet
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow

    Dim StrTot, Dap, Alp As String
    Dim Esponi As Boolean

    Dim Rpt As ReportClass
    Dim Rpt1 As New SkeConti
    Dim Rpt2 As New SkePagina
    Dim Rpt3 As New SkePSaldo

    Dim Singolo As Boolean
    Dim Sw As Int16 = 0
    Dim TipoStampa As Int16
    Dim MiglioFo, Test, Corp, Irow, Iset, Iset2, IIrow As Int32
    Dim UltimaApertura, EseDal As Date
    Dim StrUno, StrDue, StrTre, StrPrint, LIMITI(6), StrD(11), StrLim, LimD, LimA As String

    Private Sub DxSparti_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If Sw = 0 Then
            PrimoMiglio()
            Sw = 1
        End If
        Pulizia()
    End Sub
    Sub PrimoMiglio()
        Dim Cmd As New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT ISNULL(max(pridataest),'01/01/2000') FROM Tbpri Where PriCausale = 45", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UltimaApertura = dataRd.Item(0)
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT top 1 * from TbEse Order by EseAnno desc", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TipoStampa = dataRd.Item("EseFormato")
            EseDal = dataRd.Item("EseDal")
        End While
        dataRd.Close()
        ComboBoxEdit1.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT distinct RivaNreg from TbRegIva where RivaTipo = 5", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item(0))
        End While
        dataRd.Close()
        LIMITI(1) = "000.00"
        LIMITI(2) = "101000"
        LIMITI(3) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(4) = "099.99"
        LIMITI(5) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(6) = "199999"
        StrD(0) = "SELECT DISTINCT PRKTIPOCO,PRKCONTO,PRKDESC,SUM(DARE) as TDARE,sum(AVERE) as TAVERE ,(SUM(DARE)-SUM(AVERE)) AS TSALDO ,max(PRKAAMMGG) as FINOAL,min(PRKAAMMGG) as DAL "
        StrD(1) = "SELECT * ,0.00 as Tsaldo "
        StrD(2) = " FROM VH8 WHERE PRKTIPOCO+PRKCONTO BETWEEN "
        StrD(3) = " AND PRKAAMMGG BETWEEN "
        StrD(4) = " FROM VH8 WHERE PRKCONTO = "
        StrD(5) = " GROUP BY PRKTIPOCO,PRKCONTO,PRKDESC ORDER BY PRKTIPOCO,PRKCONTO"
        StrD(6) = "SELECT DISTINCT PRKTIPOCO,PRKCONTO,PRKDESC,(SUM(DARE)-SUM(AVERE)) AS TSALDO FROM VH8 WHERE PRKTIPOCO+PRKCONTO BETWEEN "
        StrD(7) = "SELECT DISTINCT PRKTIPOCO,PRKCONTO,PRKDESC,PIAFL,0.00 AS TSALDO FROM VH8 WHERE PRKTIPOCO+PRKCONTO BETWEEN "
        StrD(8) = " GROUP BY PRKTIPOCO,PRKCONTO,PRKDESC,PIAFL"
        'StrD(9) = " AND PIAFL <> 6 AND PIAFL <> 7 " 
        StrD(9) = "  "
        'StrD(10) = " ORDER BY PRKAAMMGG "
        StrD(10) = " order by prkTipoCo,PrkConto,PRKAAMMGG,PriregIva desc,PriNumProt"
        StrD(11) = " order by prkTipoCo,PrkConto,PRIDATAEST,PriregIva desc,PriNumProt"
        DateEdit1.EditValue = CDate(EseDal)
        DateEdit2.EditValue = CDate(Date.DaysInMonth(Today.Year, Today.Month) & "/" & Today.Month & "/" & Today.Year)
        '''''''''  ErrorProvider1.SetError(Label13, "REGISTRO IVA MANCANTE!")
        RadioGroup1.SelectedIndex = -1
    End Sub
    Sub DisBottoni()
        ButtonF5.Enabled = True
        ButtonF9.Enabled = False
        ButtonF11.Enabled = True
    End Sub
    Sub PulisciGrid()
        DsTbo = New DataTable(Tb)
        GridControl1.DataSource = DsTbo
        GridView1.ActiveFilterString = ""
        GridControl1.Refresh()
        DsCbo = New DataSet
        GridControl2.DataSource = DsCbo.Tables(Cb)
        GridControl2.Refresh()
    End Sub
    Sub Pulizia()
        PulisciGrid()
        DisBottoni()
        CheckEdit1.Enabled = False
        CheckEdit2.Enabled = False
        CheckEdit3.Enabled = False
        StrLim = ""
        TextEdit20.EditValue = "" : TextEdit21.EditValue = ""
        Singolo = False
        Test = 0
        Corp = 0
        REM x scatenare l'evento change dopo la selezione
        Dim OG As Integer = RadioGroup1.SelectedIndex
        RadioGroup1.SelectedIndex = -1 : RadioGroup1.SelectedIndex = OG
    End Sub
    Sub PopolaTESTATA()
        DsTbo = New DataTable(Tb)
        DaTbo = New SqlDataAdapter(StrUno, cnCo)
        DaTbo.SelectCommand.CommandTimeout = 300
        DaTbo.Fill(DsTbo)
        Test = DsTbo.Rows.Count
        GridControl1.DataSource = DsTbo
        GridControl1.Refresh()
        GridView1.ClearSelection()
        If Test > 0 Then
            ButtonF9.Enabled = True
            CheckEdit1.Enabled = True
            CheckEdit2.Enabled = True
            CheckEdit3.Enabled = True
        Else
            ButtonF9.Enabled = False
            CheckEdit1.Enabled = False
            CheckEdit2.Enabled = True
            CheckEdit3.Enabled = False
        End If
    End Sub
    Sub PopolaCORPO()
        'ATTENZIONE CARICO RIGHE TOTALI CON PRKTIPOCO = 9 PER NON STAMPARLE IN CRYSTAL REPORT
        DsCbo = New DataSet
        DaCbo = New SqlDataAdapter(StrDue, cnCo)
        DaCbo.SelectCommand.CommandTimeout = 300
        DaCbo.Fill(DsCbo, Cb)
        Corp = DsCbo.Tables(Cb).Rows.Count
        RwCbo = DsCbo.Tables(Cb).NewRow()
        RwCbo("PrkTipoCo") = "9"
        DsCbo.Tables(Cb).Rows.Add(RwCbo)
        RwCbo = DsCbo.Tables(Cb).NewRow()
        RwCbo("PrkTipoCo") = "9"
        If Corp = 0 Then
            RwCbo("DARE") = 0 : RwCbo("AVERE") = 0
        Else
            RwCbo("DARE") = RwX("TDARE")
            RwCbo("AVERE") = RwX("TAVERE")
        End If


        RwCbo("DESCRIZ") = "TOTALI DAL " & RwX("DAL") & " " & RwX("FINOAL")
        DsCbo.Tables(Cb).Rows.Add(RwCbo)
        RwCbo = DsCbo.Tables(Cb).NewRow()
        RwCbo("PrkTipoCo") = "9"
        If RwX("TSALDO") < 0 Then
            If Corp = 0 Then
                RwCbo("DARE") = 0
            Else
                RwCbo("DARE") = Math.Abs(RwX("TSALDO"))
            End If
            RwCbo("AVERE") = 0
            RwCbo("DESCRIZ") = "SALDO A V E R E DEL PERIODO"
        Else
            If RwX("TSALDO") > 0 Then
                RwCbo("DARE") = 0
                If Corp = 0 Then
                    RwCbo("AVERE") = 0
                Else
                    RwCbo("AVERE") = RwX("TSALDO")
                End If
                RwCbo("DESCRIZ") = "SALDO  D A R E  DEL PERIODO"
            Else
                RwCbo("DARE") = 0
                RwCbo("AVERE") = 0
                RwCbo("DESCRIZ") = "SALDO DEL PERIODO"
            End If
        End If
        DsCbo.Tables(Cb).Rows.Add(RwCbo)
        DsCbo.AcceptChanges()
        Dim Rrow As Int32
        Rrow = DsCbo.Tables(Cb).Rows.Count
        GridControl2.DataSource = DsCbo.Tables(Cb)
        GridControl2.Refresh()
        GridView2.ClearSelection()
        GridView2.FocusedRowHandle = Rrow - 1
        GridView2.SelectRange(Rrow - 1, Rrow - 2)
    End Sub
    Private Sub GridControl2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl2.MouseMove
        ShowHitInfo2(GridView2.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo2(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl2.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset2 = hi.RowHandle
    End Sub
    Private Sub GridView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.Click
        If Iset2 > -1 And Corp > 0 And Iset2 < Corp Then
            RwX2 = GridView2.GetDataRow(Iset2)
            IIrow = Iset2
            LanciaProgramma()
        End If
    End Sub
    Sub LanciaProgramma()
        Dim jj, Corr As Int16
        Dim Gesterna As New DxInPrNo
        Dim GesternF As New DxInFtCF
        Dim GesternC As New DxInFtCo
        Corr = 0
        If RwX2("PriRegIva") > 0 Then
            For jj = 1 To ComboBoxEdit1.Properties.Items.Count
                If RwX2("PriRegIva") = ComboBoxEdit1.Properties.Items(jj - 1) Then Corr = 1
            Next
        End If
        If RwX2("PriRegIva") = 0 And RwX2("PriNumProt") > 0 Then
            DxInPrNo.NRifArt = RwX2("PriNumProt")
            Gesterna.WindowState = FormWindowState.Maximized
            Gesterna.ShowDialog()
        ElseIf RwX2("PriRegIva") > 0 And RwX2("PriNumProt") > 0 And Corr = 0 Then
            DxInFtCF.NRifAnno = CDate(RwX2("PriDataGio")).Year
            If ControlloIva(DxInFtCF.NRifAnno, RwX2("PriRegIva")) = False Then
                '''' TextErr2.Visible = True : TextErr2.ErrorText = "REGISTRO IVA MANCANTE!"
                GoTo IINext
            End If
            DxInFtCF.NRifProt = RwX2("PriNumProt")
            DxInFtCF.NRifRiva = RwX2("PriRegIva")
            DxInFtCF.NRifBis = ""
            GesternF.WindowState = FormWindowState.Maximized
            GesternF.ShowDialog()
        ElseIf RwX2("PriRegIva") > 0 And RwX2("PriNumProt") > 0 And Corr = 1 Then
            DxInFtCo.NRifAnno = CDate(RwX2("PriDataGio")).Year
            If ControlloIva(DxInFtCo.NRifAnno, RwX2("PriRegIva")) = False Then
                ''''TextErr2.Visible = True : TextErr2.ErrorText = "REGISTRO IVA MANCANTE!"
                GoTo IINext
            End If
            DxInFtCo.NRifProt = RwX2("PriNumProt")
            DxInFtCo.NRifRiva = RwX2("PriRegIva")
            GesternC.WindowState = FormWindowState.Maximized
            GesternC.ShowDialog()
        End If
IINext:
        PopolaTESTATA()
        REM CONTROLLO x NUOVI TOTALI E SELEZIONA CONTO !!! POTREBBE NON ESSERCI PIU' 

        Dim CONTO As String = RwX("PrkConto")
        If GridView1.RowCount > 1 Then
            RwX = GridView1.GetDataRow(Irow)
            If RwX("PrkConto") <> CONTO Then
                Irow = Irow - 1
                RwX = GridView1.GetDataRow(Irow)
                If RwX("PrkConto") <> CONTO Then
                    Irow = Irow + 2
                    RwX = GridView1.GetDataRow(Irow)
                End If
            End If
        ElseIf GridView1.RowCount = 1 Then
            Irow = 0 : RwX = GridView1.GetDataRow(Irow)
        Else
            Irow = -1
        End If
        GridView1.FocusedRowHandle = Irow : GridView1.SelectRow(Irow)
        If Test > 0 Then
            PopolaCORPO()
            GridView2.FocusedRowHandle = IIrow
            GridView2.SelectRow(IIrow)
        Else
            ButtonF11.PerformClick()
        End If
    End Sub
    Private Function ControlloIva(ByVal Anno As Int16, ByVal Reg As Int16) As Boolean
        ControlloIva = False
        Cmd = New SqlCommand("SELECT * from TbRegIva where RivaAnno = " & Anno & " and RIvaNReg = " & Reg, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ControlloIva = True
        End While
        dataRd.Close()
    End Function
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset = hi.RowHandle
    End Sub
    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        '''TextErr2.Visible = False : TextErr2.ErrorText = ""
        If Iset > -1 Then
            Irow = Iset
            RwX = GridView1.GetDataRow(Irow)
            Singolo = True
            LIMITI(0) = RwX("PrkTipoCo") & RwX("PrkConto")
            FormaStringaCorpo()
            PopolaCORPO()
        End If
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If VerificaData() = False Then Exit Sub
        Cursor.Current = Cursors.WaitCursor
        If Singolo = True Then
            StrUno = StrD(0) & StrD(2) & "'" & LIMITI(0) & "' AND '" & LIMITI(0) & "'" & StrD(3) & "'" & CDate(DateEdit1.EditValue).ToShortDateString & "' AND '" & CDate(DateEdit2.EditValue).ToShortDateString & "'"
            StrUno = StrUno & StrD(5)
            PopolaTESTATA()
            If GridView1.RowCount <> 1 Then Exit Sub
            RwX = GridView1.GetFocusedDataRow
            FormaStringaCorpo()
            PopolaCORPO()
            Cursor.Current = Cursors.Default
            Exit Sub
        End If
        PulisciGrid()
        Test = 0
        Corp = 0
        If FormaStringaTesta() = False Then Exit Sub
        PopolaTESTATA()
        Cursor.Current = Cursors.Default
    End Sub
    Function VerificaData() As Boolean
        VerificaData = True
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        style = MsgBoxStyle.Critical
        If DateEdit1.EditValue > DateEdit2.EditValue Then
            response = MsgBox("DATA INIZIO > DATA FINE", style, "PERIODO DAL ... AL ...")
            VerificaData = False
            Exit Function
        End If
    End Function
    Sub FormaStringaCorpo()
        Dim hey As Int16 = 10
        If CheckEdit4.Checked = True Then hey = 11
        StrDue = StrD(1) & StrD(4) & "'" & RwX("PrkConto") & "'" & StrD(3) & "'" & CDate(DateEdit1.EditValue).ToShortDateString & "' AND '" & CDate(DateEdit2.EditValue).ToShortDateString & "'" & StrD(hey)
        If Mid(RwX("PrkConto"), 3, 1) = "." Then
            LimD = "0" & RwX("PrkConto")
        Else
            LimD = "1" & RwX("PrkConto")
        End If
        LimA = LimD
    End Sub
    Sub FormaLimitiStandard()
        If RadioGroup1.SelectedIndex = 3 Then
            LimD = LIMITI(1)
            LimA = LIMITI(4)
        Else
            If RadioGroup1.SelectedIndex = 1 Then
                LimD = LIMITI(2)
                LimA = LIMITI(5)
            Else
                If RadioGroup1.SelectedIndex = 2 Then
                    LimD = LIMITI(3)
                    LimA = LIMITI(6)
                Else
                    If RadioGroup1.SelectedIndex = 0 Then
                        LimD = LIMITI(1)
                        LimA = LIMITI(6)
                    Else
                        If RadioGroup1.SelectedIndex = 0 Then
                            LimD = LIMITI(2)
                            LimA = LIMITI(6)
                        Else
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Function FormaStringaTesta() As Boolean
        FormaLimitiStandard()
        StrLim = "'" & LimD & "' AND '" & LimA & "'"
        StrUno = StrD(0) & StrD(2) & StrLim & StrD(3) & "'" & CDate(DateEdit1.EditValue).ToShortDateString & "' AND '" & CDate(DateEdit2.EditValue).ToShortDateString & "'"
        StrUno = StrUno & StrD(5)
        FormaStringaTesta = True
    End Function

    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            ButtonF8_Click()
            Exit Sub
        End If
    End Sub

    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim frm As New LpDs
        Rpt = New ReportClass
        Rpt1 = New SkeConti
        Rpt2 = New SkePagina
        Rpt3 = New SkePSaldo
        If Test = 0 Then Exit Sub
        Dim hey As Int16 = 10
        If CheckEdit4.Checked = True Then hey = 11
        If Corp = 0 Then
            StrPrint = StrD(1) & StrD(2) & StrLim & StrD(3) & "'" & CDate(DateEdit1.EditValue).ToShortDateString & "' AND '" & CDate(DateEdit2.EditValue).ToShortDateString & "' " & StrD(hey)
        Else
            StrPrint = StrDue
        End If
        If CheckEdit3.Checked = True Then
            Rpt = Rpt3
        ElseIf CheckEdit1.Checked = True Then
            Rpt = Rpt2
        Else
            Rpt = Rpt1
        End If
        Cursor.Current = Cursors.WaitCursor
        DsRip = New DataSet("H8")
        DaRip = New SqlDataAdapter(StrPrint, cnCo)
        DaRip.SelectCommand.CommandTimeout = 300
        DaRip.Fill(DsRip, "H8")
        If CheckEdit2.Checked = True Then SommaRiporti()
        Rpt.SetDataSource(DsRip.Tables("H8"))
        Rpt.SetParameterValue("periodo", "DAL " & DateEdit1.EditValue & " AL " & DateEdit2.EditValue)
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("CONTOPAGINA", CheckEdit1.Checked)
        Rpt.SetParameterValue("DATERIPORTO", Dap & " - " & Alp)

        If TipoStampa = 1 Then
            PdfStart(Rpt, Me.Text())
            Exit Sub
        End If
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
    Sub SommaRiporti()
        Dim Tt As String = "Tot"
        Dim DsTot As DataSet
        Dim DaTot As SqlDataAdapter
        Dim RwTot As DataRow
        If UltimaApertura = CDate(DateEdit1.EditValue) Then
            If CDate(DateEdit1.EditValue).Day = CDate(UltimaApertura).Day And CDate(DateEdit1.EditValue).Month = CDate(UltimaApertura).Month Then
                Esponi = False
            Else
                Dap = CDate(UltimaApertura).ToShortDateString
                Alp = CDate(DateEdit1.EditValue).AddDays(-1).ToShortDateString
                Esponi = True
            End If
        Else
            Dap = CDate(UltimaApertura).ToShortDateString
            Alp = CDate(DateEdit1.EditValue).AddDays(-1).ToShortDateString
            Esponi = True
        End If
        If Esponi = False Then Exit Sub
        StrTot = StrD(6) & "'" & LimD & "' AND '" & LimA & "'" & StrD(3) & "'" & Dap & "' AND '" & Alp & "'"
        StrTot = StrTot & StrD(9) & StrD(8)
        Cursor.Current = Cursors.WaitCursor
        DsTot = New DataSet
        DaTot = New SqlDataAdapter(StrTot, cnCo)
        DaTot.SelectCommand.CommandTimeout = 300
        DaTot.Fill(DsTot, Tt)
        Dim S0 As String
        Dim Rs0 As DataRow()
        Dim y, x As Int32
        For y = 1 To DsTot.Tables(Tt).Rows.Count
            RwTot = DsTot.Tables(Tt).Rows(y - 1)
            S0 = "PrkConto = '" & RwTot("PrkConto").ToString & "'"
            Rs0 = DsRip.Tables("H8").Select(S0)
            For x = 0 To Rs0.Length - 1
                Rs0(x).Item("Tsaldo") = RwTot("Tsaldo")
            Next
        Next
        DsRip.Tables("H8").AcceptChanges()
    End Sub
    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If Sw = 1 And RadioGroup1.SelectedIndex > -1 Then ButtonF11.PerformClick()
    End Sub
    Private Sub CheckEdit3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit3.CheckedChanged, CheckEdit1.CheckedChanged
        If CheckEdit3.Checked = True Then CheckEdit1.Checked = True
    End Sub

    Private Sub GridView1_RowCountChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.RowCountChanged
        If GridView1.RowCount <> 1 Then Exit Sub
        RwX = GridView1.GetFocusedDataRow
        If RwX Is Nothing Then Exit Sub
        Singolo = True
        LIMITI(0) = RwX("PrkTipoCo") & RwX("PrkConto")
        FormaStringaCorpo()
        PopolaCORPO()
    End Sub
#Region "RICERCA CONTO DIRETTO"
    Private Sub TbLeggiConto_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit20.Leave
        If TextEdit20.EditValue = "" Then DateEdit1.Focus() : Exit Sub
        If LeggiConto(TextEdit20.EditValue, TextEdit21) = False Then TextEdit20.Focus() Else DateEdit1.Focus()
    End Sub
    Private Sub ButtonF8_Click()
        If TextEdit20.ContainsFocus = True Then
            Dim Nc As String = ""
            Nc = EstraiRicerca(TextEdit20.EditValue.ToUpper)
            If Nc > "00.00" Then
                TextEdit20.EditValue = Nc
                LeggiConto(TextEdit20.EditValue, TextEdit21)
                SelectNextControl(DateEdit1, True, True, True, True)
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
        frm.StartPosition = FormStartPosition.CenterParent
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
        If LeggiConto = True Then LIMITI(0) = "0" & CodCo : Singolo = True : Exit Function
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
        If LeggiConto = True Then LIMITI(0) = "1" & CodCo : Singolo = True
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
#End Region


End Class
