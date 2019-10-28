Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports NPRINT
Imports DevExpress.XtraReports.UI
Public Class DxCer770

    Dim selectFormula, StrinGString, StrinG1, String2, StrPrint As String
    Dim Sc As String = "TbScoperti"
    Dim MaxR, ANNO As Int16
    Dim frm As New LpLp

    Dim REPORT As New XtraReport
    Dim SCRI As String
    Dim DsCert As DataTable
    Dim DaCert As SqlDataAdapter
    Dim P1, P2, P3, P4 As String

    Dim DsAgS As DataTable
    Dim DaAgS As SqlDataAdapter
    Dim RwAgS As DataRow

    Dim DsAgD As DataTable
    Dim DaAgD As SqlDataAdapter
    Dim RwAgD As DataRow

    Dim Iset2 As Integer

    Dim STnull As String = "SELECT DISTINCT RitCodFor, AnaDesc FROM CRCERTIFIC Where ANNOC = 9999" ' INIZIALIZZO VUOTO

    Private Sub DxCer770_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        DateEdit2.EditValue = CDate(Today)
        Apertura()
    End Sub
    Sub Apertura()
        Dim Cmd As New SqlCommand("SELECT AZIANNOLAVORO FROM tBaZI ORDER BY AziAnnoLavoro desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
        End While
        dataRd.Close()
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Me.Close() : Exit Sub
        If ComboBoxEdit1.Properties.Items.Count > 1 Then ComboBoxEdit1.SelectedIndex = 1 Else ComboBoxEdit1.SelectedIndex = 0
    End Sub
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        If ComboBoxEdit1.SelectedIndex > -1 Then PopolaGrid()
    End Sub

    Sub PopolaGrid()
        Dim STR As String = "SELECT DISTINCT RitCodFor, AnaDesc FROM CRCERTIFIC Where ANNOC = " & ComboBoxEdit1.EditValue & " OR ENASARCO = " & ComboBoxEdit1.EditValue
        DsAgS = New DataTable
        DaAgS = New SqlDataAdapter(STR, cnCo)
        DaAgS.Fill(DsAgS)
        GridControl1.DataSource = DsAgS
        GridControl1.Refresh()
        GridView1.ClearSelection()
        DsAgD = New DataTable
        DaAgD = New SqlDataAdapter(STnull, cnCo)
        DaAgD.Fill(DsAgD)
        DsAgD.AcceptChanges()
        GridControl2.DataSource = DsAgD
        GridControl2.Refresh()
        GridView2.ClearSelection()
    End Sub

    Private Sub CheckButton1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckButton1.CheckedChanged
        If CheckButton1.Checked = True Then
            CheckButton1.ImageIndex = 27
            CheckButton1.ToolTip = "TOGLI da STAMPA"
            PassaTutti()
        Else
            CheckButton1.ImageIndex = 26
            CheckButton1.ToolTip = "INVIA in STAMPA"
            PopolaGrid()
        End If
    End Sub
    Sub PassaTutti()
        Dim STR As String = "SELECT DISTINCT RitCodFor, AnaDesc FROM CRCERTIFIC Where ANNOC = " & ComboBoxEdit1.EditValue & " OR ENASARCO = " & ComboBoxEdit1.EditValue
        DsAgS = New DataTable
        DaAgS = New SqlDataAdapter(STnull, cnCo)
        DaAgS.Fill(DsAgS)
        GridControl1.DataSource = DsAgS
        GridControl1.Refresh()
        GridView1.ClearSelection()
        DsAgD = New DataTable
        DaAgD = New SqlDataAdapter(STR, cnCo)
        DaAgD.Fill(DsAgD)
        DsAgD.AcceptChanges()
        GridControl2.DataSource = DsAgD
        GridControl2.Refresh()
        GridView2.ClearSelection()
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
            RwAgS = GridView1.GetDataRow(iset)
            exButton79()
        End If
    End Sub
    Sub exButton79()
        Dim x As Int16
        If iset > -1 Then
            RwAgD = DsAgD.NewRow()
            For x = 0 To 1
                RwAgD(x) = RwAgS(x)
            Next
            DsAgD.Rows.Add(RwAgD)
            DsAgD.AcceptChanges()
            x = DsAgD.Rows.Count - 1
            GridView1.ClearSelection()
            GridView2.ClearSelection()
            GridControl2.DataSource = DsAgD
            GridControl2.Refresh()
            If x > -1 Then
                GridView2.SelectRow(x)
                GridView2.FocusedRowHandle = x
            End If
            DsAgS.Rows(iset).Delete()
            DsAgS.AcceptChanges()
            GridControl1.DataSource = DsAgS
            GridControl1.Refresh()
        End If
        iset = -1
    End Sub
    Private Sub GridControl2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl2.MouseMove
        ShowHitInfo2(GridView2.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo2(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl2.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset2 = hi.RowHandle
    End Sub
    Private Sub GridView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.Click
        If Iset2 > -1 Then
            RwAgD = GridView2.GetDataRow(Iset2)
            ExButton77()
        End If
    End Sub
    Sub ExButton77()
        Dim x As Int16
        If Iset2 > -1 Then
            RwAgS = DsAgS.NewRow()
            For x = 0 To 1
                RwAgS(x) = RwAgD(x)
            Next
            DsAgS.Rows.Add(RwAgS)
            DsAgS.AcceptChanges()
            x = DsAgS.Rows.Count - 1
            GridView2.ClearSelection()
            GridView1.ClearSelection()
            GridControl1.DataSource = DsAgS
            GridControl1.Refresh()
            If x > -1 Then
                GridView1.SelectRow(x)
                GridView1.FocusedRowHandle = x
            End If
            DsAgD.Rows(Iset2).Delete()
            DsAgD.AcceptChanges()
            GridControl2.DataSource = DsAgD
            GridControl2.Refresh()
        End If
        Iset2 = -1
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If DsAgD.Rows.Count = 0 Then Exit Sub
        Stampa()
    End Sub
    Sub Stampa()
        StrinGString = ""
        StrPrint = "select * from crcertific where "
        LeggiParametri()
        PreparoPercipienti()
        DsCert = New DataTable
        DaCert = New SqlDataAdapter(StrinGString, cnCo)
        DaCert.SelectCommand.CommandTimeout = 300
        DaCert.Fill(DsCert)
        selectFormula = ""
        REPORT = New DxCertRit
        REPORT.FilterString = selectFormula
        REPORT.DataSource = DsCert
        REPORT.DataMember = "DsCert"
        REPORT.Parameters.Item("P1").Value = P1
        REPORT.Parameters.Item("P2").Value = P2
        REPORT.Parameters.Item("P3").Value = P3
        REPORT.Parameters.Item("P4").Value = P4
        REPORT.Parameters.Item("AnnoCert").Value = ComboBoxEdit1.EditValue
        REPORT.Parameters.Item("dstampa").Value = CDate(DateEdit2.EditValue)
        REPORT.ShowPreview()
    End Sub
    Sub PreparoPercipienti()
        For jj As Int16 = 1 To DsAgD.Rows.Count
            RwAgD = DsAgD.Rows(jj - 1)
            If jj = 1 Then StrinGString = StrPrint & " ( CRCERTIFIC.ENASARCO = " & Val(ComboBoxEdit1.EditValue) & " OR CRCERTIFIC.ANNOC= " & Val(ComboBoxEdit1.EditValue) & ") AND ("
            StrinGString = StrinGString & "CRCERTIFIC.RitCodFor = '" & RwAgD("RitCodFor")
            If jj < DsAgD.Rows.Count Then StrinGString = StrinGString & "' or "
        Next
        StrinGString = StrinGString & "')"
    End Sub
    Sub LeggiParametri()
        Dim Cmd As New SqlCommand("SELECT * FROM CRUTE WHERE AziAnnoLavoro =" & ComboBoxEdit1.EditValue, cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            P1 = dataRd.Item("AnaCfis")
            P2 = dataRd.Item("UTEDESC")
            P3 = dataRd.Item("UTEIND")
            P4 = dataRd.Item("UTECAP") & " " & dataRd.Item("UTECITTA") & " " & dataRd.Item("UTEPROV")
        End While
        dataRd.Close()
    End Sub
End Class