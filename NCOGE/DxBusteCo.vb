Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports NPRINT
Imports CrystalDecisions.CrystalReports.Engine
Public Class DxBusteCo
    Friend WithEvents prntDoc As System.Drawing.Printing.PrintDocument
    Dim prntDial As New PrintDialog

    Dim Rpt As ReportClass
    Dim Rpt1 As LpBuste
    Dim frm As New LpDs

    Dim DsAgS As DataTable
    Dim DaAgS As SqlDataAdapter
    Dim RwAgS As DataRow

    Dim DsAgD As DataTable
    Dim DaAgD As SqlDataAdapter
    Dim RwAgD As DataRow

    Dim Iset2 As Integer

    Dim STnull As String = " select AnaCod,AnaDesc,AnaRag1,AnaRag2,AnaIndirizzo,AnaCap,AnaCitta,AnaProv from TBANA WHERE ANAGRP = '--' ORDER BY ANADESC"


    Private Sub DxBusteCo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        RadioGroup1.SelectedIndex = 0
    End Sub
    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex > -1 Then PopolaGrid()
    End Sub
    Sub PopolaGrid()
        Dim STR As String = "Select AnaCod,AnaDesc,AnaRag1,AnaRag2,AnaIndirizzo,AnaCap,AnaCitta,AnaProv  from TBANA WHERE ANAGRP = '" & RadioGroup1.EditValue & "' ORDER BY ANADESC"
        DsAgS = New DataTable
        DaAgS = New SqlDataAdapter(STR, cnVd)
        DaAgS.Fill(DsAgS)
        GridControl1.DataSource = DsAgS
        GridControl1.Refresh()
        GridView1.ClearSelection()
        DsAgD = New DataTable
        DaAgD = New SqlDataAdapter(STnull, cnVd)
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
        Dim STR As String = "Select AnaCod,AnaDesc,AnaRag1,AnaRag2,AnaIndirizzo,AnaCap,AnaCitta,AnaProv  from TBANA WHERE ANAGRP = '" & RadioGroup1.EditValue & "' ORDER BY ANADESC"
        DsAgS = New DataTable
        DaAgS = New SqlDataAdapter(STnull, cnVd)
        DaAgS.Fill(DsAgS)
        GridControl1.DataSource = DsAgS
        GridControl1.Refresh()
        GridView1.ClearSelection()
        DsAgD = New DataTable
        DaAgD = New SqlDataAdapter(STR, cnVd)
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
            For x = 1 To RwAgS.ItemArray.Length
                RwAgD(x - 1) = RwAgS(x - 1)
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
            For x = 1 To RwAgD.ItemArray.Length
                RwAgS(x - 1) = RwAgD(x - 1)
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
        frm = New LpDs
        Rpt = New ReportClass
        Rpt1 = New LpBuste
        Rpt = Rpt1
        Rpt.SetDataSource(DsAgD)
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub

End Class