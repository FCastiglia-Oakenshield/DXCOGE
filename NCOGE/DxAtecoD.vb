Imports System.Data.SqlClient
Imports DXBASE
Public Class DxAtecoD
    Dim DaAteco As SqlDataAdapter
    Dim DsAteco As DataTable
    Dim RwX As DataRow
    Dim Insert As Boolean
    Dim OldValue As String = ""


    Private Sub DxAteco(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        PopolaGrid()
        TextEdit1.Focus()
    End Sub
    Sub Pulizia(ByVal T As Boolean)
        TextEdit2.EditValue = ""
        If T = True Then TextEdit1.EditValue = "" : Insert = True
    End Sub
    Sub PopolaGrid()
        Dim str As String = "SELECT * FROM TbAteco order by AtecoCod"
        DaAteco = New SqlDataAdapter(str, cnCo)
        DsAteco = New DataTable
        DaAteco.Fill(DsAteco)
        GridControl1.DataSource = DsAteco
        GridControl1.Refresh()
        GridView1.UnselectRow(0)
        GridView1.ClearSelection()
    End Sub
    Function ControlloCodice(ByVal Cod As String) As Boolean
        If Len(Cod) < 8 Then Return False
        For i As Int16 = 1 To 7 Step 3
            If Not IsNumeric(Mid(Cod, i, 2)) Then Return False
        Next
        For i As Int16 = 3 To 6 Step 3
            If Mid(Cod, i, 1) <> "." Then Return False
        Next
        Return True
    End Function
    Private Sub TbLeggi3_ENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi3.Enter
        If ControlloCodice(TextEdit1.EditValue) = False Then
            TextEdit1.Focus()
            Exit Sub
        End If
        CaricaDati()
        TextEdit2.Focus()
    End Sub
    Sub CaricaDati()
        Insert = True
        Dim str As String = "SELECT * from TbAteco where AtecoCod = '" & TextEdit1.EditValue & "'"
        Dim cmd As New SqlCommand(str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit1.EditValue = dataRd.Item("AtecoCod")
            TextEdit2.EditValue = dataRd.Item("AtecoDesc")
            Insert = False
            FocusedGrid(TextEdit1.EditValue)
        Else
            Pulizia(False)
        End If
        dataRd.Close()
    End Sub
    Function FocusedGrid(ByVal Cod As String) As Boolean
        Dim OpzControl As Array = GridView1.GetSelectedRows
        For i As Int16 = 1 To GridView1.SelectedRowsCount
            GridView1.UnselectRow(OpzControl(i - 1))
        Next
        For i As Int16 = 1 To GridView1.RowCount
            RwX = GridView1.GetDataRow(i - 1)
            If RwX("AtecoCod") = Cod Then
                GridView1.FocusedRowHandle = i - 1
                GridView1.SelectRow(i - 1)
                Exit Function
            End If
        Next
    End Function
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub

    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If iset > -1 Then
            RwX = GridView1.GetDataRow(iset)
            TextEdit1.EditValue = RwX("AtecoCod")
            SelectNextControl(TbLeggi3, True, True, True, False)
        End If
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Controllo() = False Then Exit Sub
        Registra()
        OldValue = TextEdit1.EditValue
        ButtonF5.PerformClick()
        FocusedGrid(OldValue)
    End Sub

    Private Function Controllo() As Boolean
        Controllo = True
        If ControlloCodice(TextEdit1.EditValue) = False Then TextEdit1.Focus() : Return False
        If TextEdit2.EditValue = "" Then TextEdit2.Focus() : Return False
    End Function
    Sub Registra()
        Dim Ins As String = "Insert into TbAteco (AtecoCod,AtecoDesc) values (@AtecoCod,@AtecoDesc)"
        Dim Upd As String = "Update TbAteco set AtecoDesc=@AtecoDesc where AtecoCod=@AtecoCod"
        Dim Str As String = ""
        Dim p1 As New SqlParameter("@AtecoCod", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@AtecoDesc", SqlDbType.VarChar)

        If Insert = True Then Str = Ins Else Str = Upd
        Cmd = New SqlCommand(Str, cnCo)
        p1.Value = TextEdit1.EditValue
        p2.Value = TextEdit2.EditValue

        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)

        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If ControlloCodice(TextEdit1.EditValue) = False Then Exit Sub
        DeleteCom()
        ButtonF5.PerformClick()
    End Sub
    Private Sub DeleteCom()
        Dim box As Object
        box = MessageBox.Show("ELIMINO IL CODICE Ateco " & TextEdit1.EditValue & " ?", "ELIMINA CODICI Ateco", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If box = DialogResult.No Then
            Exit Sub
        End If
        Cmd = New SqlCommand("Delete from TbAteco where AtecoCod='" & TextEdit1.EditValue & "'", cnCo)
        Cmd.ExecuteNonQuery()
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
    End Sub
End Class