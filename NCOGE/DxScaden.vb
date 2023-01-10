Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Public Class DxScaden

    Dim DsCbo As DataTable
    Dim DaCbo As SqlDataAdapter
    Dim RwCbo As DataRow

    Dim DsSke As DataTable
    Dim DaSke As SqlDataAdapter
    Dim Ar As String = "Stampe"


    Dim DsTbo As DataTable
    Dim DaTbo As SqlDataAdapter
    Dim RwTbo As DataRow

    Dim DsSca As DataTable
    Dim DaSca As SqlDataAdapter
    Dim RwSca As DataRow

    Dim DsRip As DataSet
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow

    Dim Rpt As ReportClass
    Dim Rpt1 As New SkeScadenze
    Dim CheckConto As Integer = -1
    Dim Sw As Int16 = 0
    Dim TipoStampa As Int16
    Dim MiglioFo, Test, Corp, Rrat, Iset2, Irow, Srow, Rrow As Int32
    Dim StrUno, StrDue, StrTre, StrQua, StrCin, StrSei, StrPrint, LIMITI(6), StrD(12), StrLim, LimD, LimA As String

    Private Sub DxScaden_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If Sw = 0 Then
            PrimoMiglio()
            Sw = 1
        End If
        CheckConto = RadioGroup1.SelectedIndex
        Pulizia()
        RadioGroup1.SelectedIndex = -1 : RadioGroup1.SelectedIndex = CheckConto
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
            TipoStampa = dataRd.Item("EseFormato")
        End While
        dataRd.Close()
        LIMITI(1) = "000.00"
        LIMITI(2) = "101000"
        LIMITI(3) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(4) = "099.99"
        LIMITI(5) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(6) = "199999"
        StrD(0) = "SELECT DISTINCT SCATIPOCO,SCACONTO,SCADESC FROM CRG1"
        StrD(1) = " AND PRKPAPERTA = 0 "
        StrD(3) = " WHERE SCATIPOCO+SCACONTO BETWEEN "
        StrD(5) = " GROUP BY SCATIPOCO,SCACONTO,SCADESC ORDER BY SCATIPOCO,SCACONTO"
        StrD(4) = " SELECT DISTINCT ScaNdoc,ScaImpDoc,ScaIvaSpe,ScaTPag,ScaCodPag,PagDesc,ScaDdoc,ScaRifId,ScaRifProg,CLFOPI FROM CRG1 WHERE SCACONTO = "
        StrD(6) = "GROUP BY ScaNdoc,ScaImpDoc,ScaIvaSpe,ScaTPag,ScaCodPag,PagDesc,ScaDdoc,ScaRifId,ScaRifProg,CLFOPI"
        StrD(8) = " ORDER BY ScaDdoc desc ,ScaNdoc desc"
        StrD(9) = "select *,Aperta=case when ScaRAperta = '' then cast('0' AS VARCHAR(1)) else CAST(ScaRaperta as varchar(1))end from Tbsca where scaconto = "
        StrD(10) = " And scaNdoc = "
        StrD(11) = " And scaDDoc = "
        StrD(7) = "SELECT * FROM CRG1 WHERE SCATIPOCO+SCACONTO BETWEEN  "
        StrD(12) = "SELECT * FROM CRG1 WHERE SCACONTO = "
        CheckEdit2.Checked = True
    End Sub
    Sub DisBottoni()
        ButtonF5.Enabled = True
        ButtonF1.Enabled = False
        ButtonF11.Enabled = True
    End Sub
    Sub PulisciScad()
        DsSca = New DataTable
        GridControl3.DataSource = DsSca
        GridControl3.Refresh()
    End Sub
    Sub PulisciGrid()
        DsTbo = New DataTable
        GridControl1.DataSource = DsTbo
        GridControl1.Refresh()
        DsCbo = New DataTable
        GridControl2.DataSource = DsCbo
        GridControl2.Refresh()
        PulisciScad()
    End Sub
    Sub Pulizia()
        PulisciGrid()
        DisBottoni()
        ButtonF9.Enabled = False
        StrLim = ""
        Test = 0
        Corp = 0
        Rrat = 0
    End Sub
    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If Sw = 1 Then ButtonF11.PerformClick()
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        EseguoOperazione()
    End Sub
    Sub EseguoOperazione()
        PulisciGrid()
        Test = 0
        Corp = 0
        Cursor.Current = Cursors.WaitCursor
        StrUno = StrD(0) & StrD(3) & "'" & LIMITI(0) & "' AND '" & LIMITI(0) & "'"
        FormaStringaTesta()
        PopolaTESTATA()
        Cursor.Current = Cursors.Default
    End Sub
    Sub FormaStringaTesta()
        FormaLimitiStandard()
        StrLim = "'" & LimD & "' AND '" & LimA & "'"
        StrUno = StrD(0) & StrD(3) & StrLim
        If CheckEdit2.Checked = True Then
            StrUno = StrUno & StrD(1) & StrD(5)
        Else
            StrUno = StrUno & StrD(5)
        End If
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
    Sub PopolaTESTATA()
        DsTbo = New DataTable
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
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub
    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If iset > -1 Then
            RwTbo = GridView1.GetDataRow(iset)
            Irow = iset
            FormaStringaCorpo()
            PopolaCORPO()
        End If
    End Sub
    Sub FormaStringaCorpo()
        StrDue = StrD(4) & "'" & RwTbo("ScaConto") & "'"
        StrSei = StrD(12) & "'" & RwTbo("ScaConto") & "'"
        StrQua = StrD(9) & "'" & RwTbo("ScaConto") & "'"
        If CheckEdit2.Checked = True Then
            StrDue = StrDue & StrD(1)
            StrSei = StrSei & StrD(1)
        End If
        StrDue = StrDue & StrD(6) & StrD(8)
        If RwTbo("ScaTipoCo") = 0 Then
            LimD = "0" & RwTbo("ScaConto")
        Else
            LimD = "1" & RwTbo("ScaConto")
        End If
        LimA = LimD
    End Sub
    Sub PopolaCORPO()
        PulisciScad()
        DsCbo = New DataTable
        DaCbo = New SqlDataAdapter(StrDue, cnCo)
        DaCbo.SelectCommand.CommandTimeout = 300
        DaCbo.Fill(DsCbo)
        Corp = DsCbo.Rows.Count
        GridControl2.DataSource = DsCbo
        GridControl2.Refresh()
        GridView2.ClearSelection()
        If Corp > 0 Then
            ButtonF9.Enabled = True
        Else
            ButtonF9.Enabled = False
        End If
    End Sub

    Private Sub GridControl2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl2.MouseMove
        ShowHitInfo2(GridView2.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo2(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl2.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset2 = hi.RowHandle
    End Sub
    Private Sub GridView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.Click
        If iset2 > -1 Then
            RwCbo = GridView2.GetDataRow(Iset2)
            Srow = Iset2
          PopolaScadenze()
        End If
    End Sub
    Sub PopolaScadenze()
        ButtonF1.Enabled = True
        StrCin = StrQua & StrD(10) & RwCbo("ScaNDoc") & StrD(11) & "'" & RwCbo("ScaDDoc") & "'"
        DsSca = New DataTable
        DaSca = New SqlDataAdapter(StrCin, cnCo)
        DaSca.SelectCommand.CommandTimeout = 300
        DaSca.Fill(DsSca)
        Rrat = DsSca.Rows.Count
        GridControl3.DataSource = DsSca
        GridControl3.Refresh()
    End Sub
    Private Sub CheckEdit2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        If Sw = 1 Then LancioAutomatico()
    End Sub
    Private Sub GridView1_RowCountChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.RowCountChanged
        If GridView1.RowCount <> 1 Then Exit Sub
        LancioAutomatico()
    End Sub
    Sub LancioAutomatico()
        RwTbo = GridView1.GetFocusedDataRow
        If RwTbo IsNot Nothing Then
            FormaStringaCorpo()
            PopolaCORPO()
        End If
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim frm As New LpDs
        Rpt = New ReportClass
        Rpt1 = New SkeScadenze
        If Test = 0 Then Exit Sub
        If Corp = 0 Then
            StrPrint = StrD(7) & StrLim
            If CheckEdit2.Checked = True Then
                StrPrint = StrPrint & StrD(1)
            Else
                StrPrint = StrPrint
            End If
        Else
            StrPrint = StrSei
        End If
        Rpt = Rpt1
        Cursor.Current = Cursors.WaitCursor
        DsRip = New DataSet("CRG1")
        DaRip = New SqlDataAdapter(StrPrint, cnCo)
        DaRip.SelectCommand.CommandTimeout = 300
        DaRip.Fill(DsRip, "CRG1")
        Rpt.SetDataSource(DsRip.Tables("CRG1"))
        Rpt.SetParameterValue("Marchio", Marchio)
        If TipoStampa = 1 Then
            PdfStart(Rpt, Me.Text)
            Exit Sub
        End If
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F1 Then
            e.Handled = True
            ButtonF1.PerformClick()
            Exit Sub
        End If
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
    End Sub

    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Lettura = True Then Exit Sub
        If RwTbo Is Nothing Or RwCbo Is Nothing Then Exit Sub
        Dim Gsca As New DxScaRat
        Gsca.NAnaCod = RwTbo("SCACONTO")
        Gsca.NAnaDesc = RwTbo("SCADESC")
        Gsca.NCodPag = RwCbo("ScaCodPag")
        Gsca.NAnaPag = RwCbo("PagDesc")
        Gsca.NNumDoc = RwCbo("ScaNdoc")
        Gsca.NDatDoc = CDate(RwCbo("ScaDdoc")).ToShortDateString
        Gsca.NImport = RwCbo("ScaImpDoc")
        Gsca.NIvaSpe = RwCbo("ScaIvaSpe")
        Gsca.StartPosition = FormStartPosition.CenterScreen
        Gsca.ShowDialog()
        PopolaCORPO()
        PopolaScadenze()
        GridView1.FocusedRowHandle = Irow
        GridView2.FocusedRowHandle = Srow
        GridView1.SelectRow(Irow)
        GridView2.SelectRow(Srow)
        RwTbo = GridView1.GetDataRow(Irow)
        RwCbo = GridView2.GetDataRow(Srow)
    End Sub
End Class