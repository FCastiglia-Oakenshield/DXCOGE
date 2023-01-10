Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils.Menu

Public Class DxSchedC
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
    Dim Rwx As DataRow
    Dim Rwx2 As DataRow

    Dim Tr As String = "TRip"
    Dim DsRip As DataSet
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow

    Dim RwSca As DataRowView

    Dim StrTot, Dap, Alp As String
    Dim Esponi As Boolean

    Dim Rpt As ReportClass
    Dim Rpt1 As New SkePAperte
    Dim Rpt2 As New SkeAlfaApe
    Dim Tipostampa As Int16
    Dim Singolo As Boolean
    Dim Sw As Int16 = 0
    Dim MiglioFo, Test, Corp, Irow, IIrow, Iset2 As Int32
    Dim pik As Int16 = 0
    Dim StrUno, StrDue, StrTre, StrPrint, LIMITI(6), StrD(9), StrLim, LimD, LimA As String


    Private Sub DxSchedC_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
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
        RadioGroup1.Properties.Items(3).Enabled = False
        Cmd = New SqlCommand("SELECT DISTINCT PIAFL09 FROM TbPIA Where PIAFL09 = 1", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            RadioGroup1.Properties.Items(3).Enabled = True
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT top 1 * from TbEse Order by EseAnno desc", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Tipostampa = dataRd.Item("EseFormato")
        End While
        dataRd.Close()
        ComboBoxEdit1.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT distinct RivaNreg from TbRegIva where RivaNReg = 5", cnCo)
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
        StrD(0) = "SELECT DISTINCT PRKTIPOCO,PRKCONTO,PRKDESC"
        StrD(2) = " FROM VB8 WHERE PARTITARIO = 1 "
        StrD(3) = " AND PRKTIPOCO+PRKCONTO BETWEEN "
        StrD(5) = " GROUP BY PRKTIPOCO,PRKCONTO,PRKDESC ORDER BY PRKTIPOCO,PRKCONTO"
        StrD(1) = " AND PRKPAPERTA = 0 "
        StrD(4) = " SELECT *,00.0 as TSCOPERTO FROM VB8 WHERE PRKCONTO = "
        StrD(8) = " ORDER BY PRKTIPOCO,PRKCONTO,PRKPAPERTA,PRKDOCANN,PRKDOCEST,PRIDATAEST"
        StrD(7) = "SELECT * FROM VB8 WHERE PARTITARIO = 1 AND PRKTIPOCO+PRKCONTO BETWEEN "
        CheckEdit2.Checked = True
        GroupControl6.Enabled = False
        GroupControl5.Enabled = True
        CheckEdit4.Checked = False
        If Lettura = True Then CheckEdit4.Visible = False
        RadioGroup2.SelectedIndex = 0
        TextErr2.Visible = True : TextErr2.ErrorText = "REGISTRO IVA MANCANTE!"
    End Sub
    Sub DisBottoni()
        ButtonF5.Enabled = True
        ButtonF9.Enabled = False
        ButtonF11.Enabled = True
    End Sub
    Sub PulisciGrid()
        DsTbo = New DataTable()
        GridControl1.DataSource = DsTbo
        GridControl1.Refresh()
        DsCbo = New DataSet
        GridControl2.DataSource = DsCbo.Tables(Cb)
        GridControl2.Refresh()
    End Sub
    Sub Pulizia()
        PulisciGrid()
        DisBottoni()
        StrLim = "" : LimD = "" : LimA = ""
        Singolo = False
        Test = 0
        Corp = 0
        pik = 0
        TextEdit20.EditValue = "" : TextEdit21.EditValue = ""
        RadioGroup1.SelectedIndex = -1
        TextErr2.Visible = False
        TextErr2.ErrorText = ""
    End Sub
    Sub PopolaTESTATA()
        DsTbo = New DataTable()
        DaTbo = New SqlDataAdapter(StrUno, cnCo)
        DaTbo.SelectCommand.CommandTimeout = 300
        DaTbo.Fill(DsTbo)
        Test = DsTbo.Rows.Count
        GridControl1.DataSource = DsTbo
        GridControl1.Refresh()
        GridView1.ClearSelection()
        If Test > 0 Then
            ButtonF9.Enabled = True
        Else
            ButtonF9.Enabled = False
        End If
    End Sub
    Sub PopolaCORPO()
        Dim x, Nd, An As Int32
        Dim SD, SA, TS, TD, TA As Decimal
        Dim RwTsa As DataRow
        Nd = 0
        An = 0
        TD = 0
        TA = 0
        DsCbo = New DataSet
        DaCbo = New SqlDataAdapter(StrDue, cnCo)
        DaCbo.SelectCommand.CommandTimeout = 300
        DaCbo.Fill(DsCbo, Cb)
        Corp = DsCbo.Tables(Cb).Rows.Count
        If Corp = 0 Then
            GridControl2.DataSource = DsCbo.Tables(Cb)
            GridControl2.Refresh()
            Exit Sub
        End If
        For x = 1 To Corp - 1
            RwCbo = DsCbo.Tables(Cb).Rows(x - 1)
            SD = SD + RwCbo("DARE")
            SA = SA + RwCbo("AVERE")
            TD = TD + RwCbo("DARE")
            TA = TA + RwCbo("AVERE")
            RwTsa = DsCbo.Tables(Cb).Rows(x)
            If RwCbo("PrkDocEst") <> RwTsa("PrkDocEst") Or RwCbo("PrkDocAnn") <> RwTsa("PrkDocAnn") Then
                TS = SD - SA
                RwCbo("TSCOPERTO") = TS
                SD = 0
                SA = 0
                TS = 0
            End If
        Next
        RwCbo = DsCbo.Tables(Cb).Rows(x - 1)
        SD = SD + RwCbo("DARE")
        SA = SA + RwCbo("AVERE")
        TD = TD + RwCbo("DARE")
        TA = TA + RwCbo("AVERE")
        TS = SD - SA
        RwCbo("TSCOPERTO") = TS
        DsCbo.AcceptChanges()
        Dim Rrow As Int32
        Rrow = DsCbo.Tables(Cb).Rows.Count
        GridControl2.DataSource = DsCbo.Tables(Cb)
        GridControl2.Refresh()
        GridView2.ClearSelection()
        pik = 0
        RadioGroup2.Properties.Items(1).Enabled = True
    End Sub

    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub
    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        TextErr2.Visible = False : TextErr2.ErrorText = ""
        If iset > -1 Then
            Irow = iset
            Rwx = GridView1.GetDataRow(Irow)
            FormaStringaCorpo()
            PopolaCORPO()
        End If
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click, ButtonFF11.Click
        If pik = 0 Then EseguoOperazione() : Exit Sub
        If Lettura = True Then Exit Sub Else ChiudoPartitaManuale()
    End Sub
    Sub ChiusuraPartiteAutomatica()
        If Test = 0 Then EseguoOperazione()
        Cursor.Current = Cursors.WaitCursor
        FormaLimitiStandard()
        EsegueSql(" EXEC ChiudePiuContiZero  @CONTOD ='" & LimD & "' , @CONTOA = '" & LimA & "'", cnCo)
    End Sub
    Sub ChiudoPartitaManuale()
        Dim x As Int32
        For x = 1 To DsCbo.Tables(Cb).Rows.Count
            Rwx2 = DsCbo.Tables(Cb).Rows(x - 1)
            AggPartita(Rwx2("PrkPAperta"), Rwx2("PriId"), Rwx2("PriProg"), Rwx2("PrkDa"))
        Next
        GridControl1.Enabled = True
        PopolaCORPO()
        Singolo = False
    End Sub
    Sub AggPartita(ByVal AC As Boolean, ByVal IdK As Int32, ByVal IdProg As Int16, ByVal IdDa As String)
        Dim Str As String
        Str = "Update TbPrk set PrkPAperta = @PrkpAperta Where PrkId = @PrkId and PrkProg =@PrkProg and PrkDa = @PrkDa"
        Cmd = New SqlCommand(Str, cnCo)
        Dim p1 As New SqlParameter("@PrkPAperta", SqlDbType.Bit)
        Dim p2 As New SqlParameter("@PrkId", SqlDbType.Int)
        Dim p3 As New SqlParameter("@PrkProg", SqlDbType.SmallInt)
        Dim p4 As New SqlParameter("@PrkDa", SqlDbType.VarChar)
        If AC = True Then p1.Value = 1 Else p1.Value = 0
        p2.Value = IdK
        p3.Value = IdProg
        p4.Value = IdDa
        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.ExecuteNonQuery()
    End Sub
    Sub EseguoOperazione()
        PulisciGrid()
        Test = 0
        Corp = 0
        Cursor.Current = Cursors.WaitCursor
        If Singolo = True Then
            If CheckEdit2.Checked = True Then
                StrUno = StrD(0) & StrD(2) & StrD(1)
            Else
                StrUno = StrD(0) & StrD(2)
            End If
            StrUno = StrUno & StrD(3) & "'" & LIMITI(0) & "' AND '" & LIMITI(0) & "'" & StrD(5)
            PopolaTESTATA()
            If Test > 0 Then
                Irow = 0
                GridView1.SelectRow(Irow)
                Rwx = GridView1.GetFocusedDataRow
                FormaStringaCorpo()
                PopolaCORPO()
            End If
            ''   Singolo = False
            Cursor.Current = Cursors.Default
            Exit Sub
        End If
        FormaStringaTesta()
        PopolaTESTATA()
        Cursor.Current = Cursors.Default
    End Sub
    Sub FormaStringaCorpo()
        StrDue = StrD(4) & "'" & Rwx("PrkConto") & "'"
        If CheckEdit2.Checked = True Then
            StrDue = StrDue & StrD(1)
        End If
        StrDue = StrDue & StrD(8)
        If Rwx("PrkTipoCo") = 0 Then
            LimD = "0" & Rwx("PrkConto")
        Else
            LimD = "1" & Rwx("PrkConto")
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
                    If RadioGroup1.SelectedIndex = 0 And RadioGroup1.Properties.Items(3).Enabled = True Then
                        LimD = LIMITI(1)
                        LimA = LIMITI(6)
                    Else
                        If RadioGroup1.SelectedIndex = 0 And RadioGroup1.Properties.Items(3).Enabled = False Then
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
    Sub FormaStringaTesta()
        FormaLimitiStandard()
        StrLim = "'" & LimD & "' AND '" & LimA & "'"
        If CheckEdit2.Checked = True Then
            StrUno = StrD(0) & StrD(2) & StrD(1)
        Else
            StrUno = StrD(0) & StrD(2)
        End If
        StrUno = StrUno & StrD(3) & StrLim & StrD(5)
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim frm As New LpDs
        Rpt = New ReportClass
        Rpt1 = New SkePAperte
        Rpt2 = New SkeAlfaApe

        If Test = 0 Then Exit Sub

        If CheckEdit1.Checked = True Then
            Rpt = Rpt2
        Else
            Rpt = Rpt1
        End If

        If Corp = 0 Then
            StrPrint = StrD(7) & StrLim
            If CheckEdit2.Checked = True Then
                StrPrint = StrPrint & StrD(1)
            Else
                StrPrint = StrPrint
            End If
        Else
            StrPrint = StrDue
        End If
        Cursor.Current = Cursors.WaitCursor
        DsRip = New DataSet("VB8")
        DaRip = New SqlDataAdapter(StrPrint, cnCo)
        DaRip.SelectCommand.CommandTimeout = 300
        DaRip.Fill(DsRip, "VB8")
        Rpt.SetDataSource(DsRip.Tables("VB8"))
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("Miglio", MiglioFo.ToString.PadLeft(5, "0"))

        If Tipostampa = 1 Then
            PdfStart(Rpt, Me.Text)
            Exit Sub
        End If
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If Sw = 1 Then ButtonF11.PerformClick()
    End Sub

    Private Sub CheckEdit4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit4.CheckedChanged
        GroupControl6.Enabled = CheckEdit4.Checked
        GroupControl5.Enabled = Not CheckEdit4.Checked
    End Sub

    Private Sub GridView2_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView2.RowClick
        Iset2 = e.RowHandle
        TextErr2.Visible = False : TextErr2.ErrorText = ""
        If Iset2 > -1 And Corp > 0 And Iset2 < Corp Then
            If CheckEdit4.Checked = True Then
                Rwx2 = GridView2.GetDataRow(Iset2)
                Rwx2("PrkPAperta") = Not Rwx2("PrkPAperta")
                pik = 1
            Else
                Rwx2 = GridView2.GetDataRow(Iset2)
                IIrow = Iset2
                LanciaProgramma()
            End If
            If pik = 1 Then
                GridControl1.Enabled = False
                RadioGroup2.Properties.Items(1).Enabled = False
            End If
        End If
    End Sub
#Region "CONTEXT MENU"
    'Private Sub GridView2_PopupMenuShowing(sender As Object, e As DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs) Handles GridView2.PopupMenuShowing
    '    Dim view As GridView = CType(sender, GridView)
    '    If e.MenuType = DevExpress.XtraGrid.Views.Grid.GridMenuType.Row Then
    '        Dim rowHandle As Integer = e.HitInfo.RowHandle
    '        e.Menu.Items.Clear()
    '        Dim item As DXMenuItem = CreaEliminaRiga(view, rowHandle)
    '        item.BeginGroup = True EVENTUALE RIGA RAGGRUPPAMENTO
    '        e.Menu.Items.Add(item)
    '    End If
    'End Sub
    'Function CreaEliminaRiga(ByVal view As GridView, ByVal rowHandle As Integer) As DXMenuItem
    '    Dim NewItem As New DXMenuItem("Genera Scadenze", AddressOf INSSCADClick, imageList1.Images(98))
    '    NewItem.Tag = New RowInfo(view, rowHandle)
    '    Return NewItem
    'End Function
    'REM PER GESTIRE EVENTUALMENTE LA RIGA E LA GRIDVIEW
    'Class RowInfo
    '    Public View As GridView
    '    Public RowHandle As Integer
    '    Public Sub New(ByVal view As GridView, ByVal rowHandle As Integer)
    '        Me.RowHandle = rowHandle
    '        Me.View = view
    '    End Sub
    'End Class
    'Private Sub INSSCADClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim mail As String = "Scadenza già in archivio!!!"
    '    Dim response As MsgBoxResult
    '    Dim OPZIONI As Array = GridView2.GetSelectedRows
    '    If OPZIONI.Length <= 0 Then Exit Sub
    '    RwSca = GridView2.GetRow(OPZIONI(0))
    '    Cmd = New SqlCommand("Select count(*) from TbSca where ScaRifId=" & RwSca("PriId") & " AND ScaRifProg=" & RwSca("PriProg") & " AND ScaRifDA =" & RwSca("PrkDa"), cnCo)
    '    If Cmd.ExecuteScalar > 0 Then
    '        response = MsgBox(mail, MsgBoxStyle.Critical, "CREAZIONE SCADENZE")
    '        Exit Sub
    '    End If
    '    If RwSca("Pricausale") < 4 Then Exit Sub
    '    If RwSca("PrkTipoCo") <> 1 Then Exit Sub
    '    If RwSca("PrkConto") > MiglioFo And RwSca("Prkda") = 0 Then Exit Sub
    '    If RwSca("PrkConto") < MiglioFo And RwSca("Prkda") = 1 Then Exit Sub
    '    response = MsgBox("OK INSERISCO SCADENZA", MsgBoxStyle.Critical, "CREAZIONE SCADENZE")
    '    Dim Gsca As New DxScaRat
    '    Gsca.NAnaCod = RwSca("PrkConto")
    '    Gsca.NAnaDesc = RwSca("PRKDESC")
    '    Gsca.NCodPag = 0
    '    Gsca.NAnaPag = ""
    '    Gsca.NNumDoc = RwSca("PrkDocEst")
    '    Gsca.NDatDoc = CDate(RwSca("pridataest")).ToShortDateString
    '    Gsca.NImport = IIf(RwSca("Prkda") = 0, RwSca("DARE"), RwSca("AVERE"))
    '    Gsca.NIvaSpe = 0
    '    Gsca.Nrwsca = RwSca
    '    Gsca.StartPosition = FormStartPosition.CenterScreen
    '    Gsca.ShowDialog()
    'End Sub
#End Region

    'Private Sub GridControl2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl2.MouseMove
    '    ShowHitInfo2(GridView2.CalcHitInfo(New Point(e.X, e.Y)))
    'End Sub
    'Private Sub ShowHitInfo2(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
    '    Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl2.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
    '    Iset2 = hi.RowHandle
    'End Sub
    'Private Sub GridView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.Click
    '    TextErr2.Visible = False : TextErr2.ErrorText = ""
    '    If Iset2 > -1 And Corp > 0 And Iset2 < Corp Then
    '        If CheckEdit4.Checked = True Then
    '            Rwx2 = GridView2.GetDataRow(Iset2)
    '            Rwx2("PrkPAperta") = Not Rwx2("PrkPAperta")
    '            pik = 1
    '        Else
    '            Rwx2 = GridView2.GetDataRow(Iset2)
    '            IIrow = Iset2
    '            LanciaProgramma()
    '        End If
    '        If pik = 1 Then
    '            GridControl1.Enabled = False
    '            RadioGroup2.Properties.Items(1).Enabled = False
    '        End If
    '    End If
    'End Sub

    Sub LanciaProgramma()
        Dim jj, Corr As Int16
        Dim Gesterna As New DxInPrNo
        Dim GesternF As New DxInFtCF
        Dim GesternC As New DxInFtCo
        Corr = 0
        If Rwx2("PriRegIva") > 0 Then
            For jj = 1 To ComboBoxEdit1.Properties.Items.Count
                If Rwx2("PriRegIva") = ComboBoxEdit1.Properties.Items(jj - 1) Then Corr = 1
            Next
        End If
        If Rwx2("PriRegIva") = 0 And Rwx2("PriNumProt") > 0 Then
            DxInPrNo.NRifArt = Rwx2("PriNumProt")
            Gesterna.WindowState = FormWindowState.Maximized
            Gesterna.ShowDialog()
        ElseIf Rwx2("PriRegIva") > 0 And Rwx2("PriNumProt") > 0 And Corr = 0 Then
            DxInFtCF.NRifAnno = RitornaAnno(Rwx2("PriId"), Rwx2("PriProg"))
            If ControlloIva(DxInFtCF.NRifAnno, Rwx2("PriRegIva")) = False Then
                TextErr2.Visible = True : TextErr2.ErrorText = "REGISTRO IVA MANCANTE!"
                GoTo IINext
            End If
            DxInFtCF.NRifProt = Rwx2("PriNumProt")
            DxInFtCF.NRifRiva = Rwx2("PriRegIva")
            DxInFtCF.NRifBis = ""
            GesternF.WindowState = FormWindowState.Maximized
            GesternF.ShowDialog()
        ElseIf Rwx2("PriRegIva") > 0 And Rwx2("PriNumProt") > 0 And Corr = 1 Then
            DxInFtCo.NRifAnno = RitornaAnno(Rwx2("PriId"), Rwx2("PriProg"))
            If ControlloIva(DxInFtCo.NRifAnno, Rwx2("PriRegIva")) = False Then
                TextErr2.Visible = True : TextErr2.ErrorText = "REGISTRO IVA MANCANTE!"
                GoTo IINext
            End If
            DxInFtCo.NRifProt = Rwx2("PriNumProt")
            DxInFtCo.NRifRiva = Rwx2("PriRegIva")
            GesternC.WindowState = FormWindowState.Maximized
            GesternC.ShowDialog()
        End If
IINext:
        PopolaTESTATA()
        GridView1.FocusedRowHandle = Irow
        GridView1.SelectRow(Irow)
        PopolaCORPO()
        GridView2.FocusedRowHandle = IIrow
        GridView2.SelectRow(IIrow)
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
    Private Function RitornaAnno(ByVal RIF As Int32, ByVal PROG As Int16) As Int16
        Cmd = New SqlCommand("SELECT PriDataGio from TbPri where Priid = " & RIF & " and PriProg = " & PROG, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            RitornaAnno = CDate(dataRd.Item(0)).Year
        End While
        dataRd.Close()
    End Function
    Private Sub ButtonFF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF5.Click
        If Trim(StrDue) > "" Then
            GridControl1.Enabled = True
            PopolaCORPO()
            RadioGroup2.Properties.Items(1).Enabled = True
            Singolo = False
        Else
            ButtonFF11.PerformClick()
        End If
    End Sub

    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 And GroupControl5.Enabled = True Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 And GroupControl5.Enabled = True Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 And GroupControl5.Enabled = True Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 And GroupControl6.Enabled = True Then
            e.Handled = True
            ButtonFF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 And GroupControl6.Enabled = True Then
            e.Handled = True
            ButtonFF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            ButtonF8_Click()
            Exit Sub
        End If
    End Sub

    Private Sub RadioButton6_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup2.SelectedIndexChanged
        If RadioGroup2.SelectedIndex = 1 Then
            GroupControl7.Visible = True
        Else
            GroupControl7.Visible = False
        End If
        RadioGroup3.SelectedIndex = -1
    End Sub

    Private Sub RadioButton7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioGroup3.SelectedIndexChanged
        If RadioGroup3.SelectedIndex = 1 Then
            ChiusuraPartiteAutomatica()
            pik = 0
            RadioGroup2.SelectedIndex = 0
            GroupControl7.Visible = False
            ButtonFF11.PerformClick()
        ElseIf RadioGroup3.SelectedIndex = 0 Then
            RadioGroup2.SelectedIndex = 0
            GroupControl7.Visible = False
        End If
    End Sub
#Region "RICERCA CONTO DIRETTO"
    Private Sub TbLeggiConto_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit20.Leave
        If LeggiConto(TextEdit20.EditValue, TextEdit21) = False Then TextEdit20.Focus() Else ButtonF11.Focus()
    End Sub
    Private Sub ButtonF8_Click()
        If TextEdit20.ContainsFocus = True Then
            Dim Nc As String = ""
            Nc = EstraiRicerca(TextEdit20.EditValue.ToUpper)
            If Nc > "00.00" Then
                TextEdit20.EditValue = Nc
                LeggiConto(TextEdit20.EditValue, TextEdit21)
                SelectNextControl(ButtonF11, True, True, True, True)
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