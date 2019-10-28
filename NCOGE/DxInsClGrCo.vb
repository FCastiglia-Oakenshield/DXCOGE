Imports DXBASE
Imports System.Data.SqlClient
Public Class DxInsClGrCo
    Dim TbRe1 As DataTable
    Dim TbRe2 As DataTable
    Dim TbRe3 As DataTable
    Dim TbRe4 As DataTable
    Dim TbRe5 As DataTable

    Dim Rx As DataRowView
    Dim Rxw As DataRowView

    Dim DaRe1 As SqlDataAdapter
    Dim DaRe2 As SqlDataAdapter
    Dim DaRe3 As SqlDataAdapter
    Dim DaRe4 As SqlDataAdapter
    Dim DaRe5 As SqlDataAdapter

    Dim CbRe4 As SqlCommandBuilder

    Dim TbRep As DataTable
    Dim DaReP As SqlDataAdapter
    Dim CbReP As SqlCommandBuilder

    Dim Irow1, Irow2, Irow3 As Integer


    Private Sub DxInsClGrCo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia()
    End Sub
    Sub Pulizia()
        PopolaGrid1()
        TextEdit1.EditValue = "" : TextEdit2.EditValue = 0 : TextEdit3.EditValue = 0 : TextEdit4.EditValue = ""
        TextEdit2.Enabled = False : TextEdit3.Enabled = False
        Irow1 = -1 : Irow2 = -2 : Irow3 = -3 : TextEdit1.Focus()
    End Sub
    Sub PopolaGrid1()
        Cmd = New SqlCommand("SELECT *,CgcCod1=CgcCod FROM TbCgc where CgcLivello=1", cnCo)
        TbRe1 = New DataTable()
        DaRe1 = New SqlDataAdapter(Cmd)
        DaRe1.Fill(TbRe1)
        GridControl1.DataSource = TbRe1
        GridView1.UnselectRow(0)
        GridView1.ClearSelection()
        TbRe2 = New DataTable()
        TbRe3 = New DataTable()
        GridControl2.DataSource = TbRe2
        GridControl3.DataSource = TbRe3
    End Sub

    Sub PopolaGrid2()
        Cmd = New SqlCommand("SELECT *,substring(CgcCod,2,1) as CgcCod2 FROM TbCgc where CgcLivello=2 and CgcCod like '" & TextEdit1.Text & "%'", cnCo)
        TbRe2 = New DataTable()
        DaRe2 = New SqlDataAdapter(Cmd)
        DaRe2.Fill(TbRe2)
        GridControl2.DataSource = TbRe2
        GridView2.UnselectRow(0)
        TbRe3 = New DataTable()
        GridControl3.DataSource = TbRe3
    End Sub
    Sub PopolaGrid3()
        Cmd = New SqlCommand("SELECT *,substring(CgcCod,3,1) as CgcCod3 FROM TbCgc where CgcLivello=3 and CgcCod like '" & TextEdit1.Text & TextEdit2.Text & "%'", cnCo)
        TbRe3 = New DataTable()
        DaRe3 = New SqlDataAdapter(Cmd)
        DaRe3.Fill(TbRe3)
        GridControl3.DataSource = TbRe3
        GridView3.UnselectRow(0)
    End Sub
    Private Sub TextEdit1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles AccLiv1.Enter, TextEdit2.Enter
        Dim esiste As Boolean = False
        TextEdit4.EditValue = "" : TextEdit2.EditValue = "" : TextEdit3.EditValue = ""
        If IsNumeric(TextEdit1.EditValue) = False Then TextEdit1.Focus() : Exit Sub
        If GridControl1.DataSource Is Nothing Then Return
        GridView1.ClearSelection()
        For I As Int32 = 1 To GridView1.RowCount
            Rx = GridView1.GetRow(I - 1)
            If TextEdit1.EditValue = Rx("CgcCod1") Then
                GridView1.SelectRow(I - 1)
                TextEdit4.EditValue = Rx("CgcDesc")
                esiste = True
            End If
        Next
        If esiste = True Then TextEdit2.Enabled = True : TextEdit2.Focus() Else TextEdit2.Enabled = False : TextEdit4.Focus()
        TextEdit3.Enabled = TextEdit2.Enabled
        PopolaGrid2()
    End Sub

    Private Sub TextEdit2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles AccLiv2.Enter, TextEdit3.Enter
        Dim esiste As Boolean = False
        TextEdit3.EditValue = ""
        If IsNumeric(TextEdit2.EditValue) = False Then TextEdit4.Focus() : Exit Sub
        If GridControl2.DataSource Is Nothing Then Return
        GridView2.ClearSelection()
        For I As Int32 = 1 To GridView2.RowCount
            Rx = GridView2.GetRow(I - 1)
            If TextEdit2.EditValue = Rx("CgcCod2") Then
                GridView2.SelectRow(I - 1)
                TextEdit4.EditValue = Rx("CgcDesc")
                esiste = True
            End If
        Next
        If esiste = True Then TextEdit3.Enabled = True : TextEdit3.Focus() Else TextEdit3.Enabled = False : TextEdit4.Focus()
        PopolaGrid3()
    End Sub
    Private Sub TextEdit3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles AccLiv3.Enter
        Dim esiste As Boolean = False
        If IsNumeric(TextEdit2.EditValue) = False Then TextEdit4.Focus() : Exit Sub
        If GridControl3.DataSource Is Nothing Then Return
        GridView3.ClearSelection()
        For I As Int32 = 1 To GridView3.RowCount
            Rx = GridView3.GetRow(I - 1)
            If TextEdit3.EditValue = Rx("CgcCod3") Then
                GridView3.SelectRow(I - 1)
                TextEdit4.EditValue = Rx("CgcDesc")
                esiste = True
            End If

        Next
        TextEdit4.Focus()
    End Sub
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Irow1 = hi.RowHandle
    End Sub
    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If Irow1 > -1 Then
            Rx = GridView1.GetRow(Irow1)
            TextEdit1.EditValue = Rx("CgcCod1")
            AccLiv1.Focus()
        End If
    End Sub
    Private Sub GridControl2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl2.MouseMove
        ShowHitInfo2(GridView2.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo2(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl2.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Irow2 = hi.RowHandle
    End Sub
    Private Sub GridView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.Click
        If Irow2 > -1 Then
            Rx = GridView2.GetRow(Irow2)
            TextEdit2.EditValue = Rx("CgcCod2")
            AccLiv2.Focus()
        End If
    End Sub
    Private Sub GridControl3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl3.MouseMove
        ShowHitInfo3(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo3(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl3.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Irow3 = hi.RowHandle
    End Sub
    Private Sub GridView3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView3.Click
        If Irow3 > -1 Then
            Rx = GridView3.GetRow(Irow3)
            TextEdit3.EditValue = Rx("CgcCod3")
            AccLiv3.Focus()
        End If
    End Sub
    Private Function Controllo() As Boolean
        '''If TextEdit1.EditValue < "0" Or TextEdit1.EditValue > "9" Then
        '''    Return False
        '''End If
        '''If Val(TextEdit3.EditValue) > 0 Then
        '''    If Val(TextEdit2.EditValue) = 0 Then
        '''        Return False
        '''    End If
        '''End If
        Return True
    End Function
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Controllo() = False Then Return
        Dim Codice As String = Trim(TextEdit1.EditValue & TextEdit2.EditValue & TextEdit3.EditValue)
        Cmd = New SqlCommand("Select * from TbCgc WHERE CgcCod = '" & Codice & "'", cnCo)
        TbRep = New DataTable()
        DaReP = New SqlDataAdapter(Cmd)
        DaReP.Fill(TbRep)
        CbReP = New SqlCommandBuilder(DaReP)
        If TbRep.Rows.Count = 0 Then
            Rw = TbRep.NewRow
            Rw("CgcCod") = Codice
            Rw("CgcDesc") = TextEdit4.EditValue
            Rw("CgcLivello") = Len(Codice)
            TbRep.Rows.Add(Rw)
        Else
            Rw = TbRep.Rows(0)
            Rw("CgcCod") = Codice
            Rw("CgcDesc") = TextEdit4.EditValue
        End If
        DaReP.Update(TbRep)
        TbRep.AcceptChanges()
        ButtonF5.PerformClick()
    End Sub
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If TextEdit1.EditValue < "0" Or TextEdit1.EditValue > "9" Then TextEdit1.Focus() : Return
        Dim Tipo As String = "CLASSE"
        Dim Str As String = "Delete TbCgc WHERE CgcCod = '" & Trim(TextEdit1.EditValue & TextEdit2.EditValue & TextEdit3.EditValue) & "'"
        If IsNumeric(TextEdit2.EditValue) = True Then Tipo = "GRUPPO"
        If IsNumeric(TextEdit3.EditValue) = True Then Tipo = "CONTO"
        If MessageBox.Show("Sei sicuro di voler eliminare il " & Tipo & " selezionato?", "Eliminazione", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Return
        End If
        Cmd = New SqlCommand(Str, cnCo)
        Cmd.ExecuteNonQuery()
        ButtonF5.PerformClick()
    End Sub
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 Then
            ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 Then
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
 
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        StampaRepertorio()
        DXANTEPRIMA(GridControl4, False, Printing.PaperKind.A4, "PIANO GENERALE DEI CONTI")
    End Sub
    Sub StampaRepertorio()
        Cmd = New SqlCommand("SELECT SUBSTRING(CgcCod,1,1) as CgcCod1,SUBSTRING(CgcCod,2,1) as CgcCod2,SUBSTRING(CgcCod,3,1) as CgcCod3,* FROM TbCgc ORDER BY CgcCod", cnCo)
        TbRe5 = New DataTable()
        DaRe5 = New SqlDataAdapter(Cmd)
        DaRe5.Fill(TbRe5)
        GridControl4.DataSource = TbRe5
    End Sub
End Class