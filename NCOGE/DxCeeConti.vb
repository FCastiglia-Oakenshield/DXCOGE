Imports System.Data.SqlClient
Imports DXBASE
Imports NPRINT
Imports NCCOM
Imports DevExpress.XtraReports.UI

Public Class DxCeeConti
    Dim DaCee As SqlDataAdapter
    Dim DsCee As DataTable
    Dim RwX As DataRow
    Dim Insert As Boolean
    Dim OldValue As String = ""
    Dim REPORT As New XtraReport
    Dim selectformula As String

    Private Sub DxCeeConti_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        PopolaGrid()
        TextEdit1.Focus()
    End Sub
    Sub Pulizia(ByVal T As Boolean)
        TextEdit3.EditValue = "00000" : TextEdit2.EditValue = "" : TextEdit4.EditValue = "0"
        If T = True Then TextEdit1.EditValue = "00000" : Insert = True
    End Sub
    Sub PopolaGrid()
        Dim str As String = "SELECT * FROM TbCee order by CeeCod"
        DaCee = New SqlDataAdapter(str, cnCo)
        DsCee = New DataTable
        DaCee.Fill(DsCee)
        GridControl1.DataSource = DsCee
        GridControl1.Refresh()
        GridView1.UnselectRow(0)
        GridView1.ClearSelection()
    End Sub
    Private Sub TbLeggi3_ENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi3.Enter
        CaricaDati()
        TextEdit2.Focus()
    End Sub
    Sub CaricaDati()
        Insert = True
        Dim str As String = "SELECT * from TbCee where CeeCod = '" & TextEdit1.EditValue & "'"
        Dim cmd As New SqlCommand(str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit1.EditValue = dataRd.Item("CeeCod")
            TextEdit2.EditValue = dataRd.Item("CeeDesc")
            TextEdit3.EditValue = dataRd.Item("CeeAlt")
            TextEdit4.EditValue = dataRd.Item("CeeSt")
            Insert = False
            FocusedGrid(TextEdit1.EditValue)
        Else
            Pulizia(False)
        End If
        dataRd.Close()
        If Val(TextEdit1.EditValue) = 3664 Or Val(TextEdit1.EditValue) = 4308 Or Val(TextEdit1.EditValue) = 5050 Then ButtonF3.Enabled = False Else ButtonF3.Enabled = True
    End Sub
    Function FocusedGrid(ByVal Cod As String) As Boolean
        Dim OpzControl As Array = GridView1.GetSelectedRows
        For i As Int16 = 1 To GridView1.SelectedRowsCount
            GridView1.UnselectRow(OpzControl(i - 1))
        Next
        For i As Int16 = 1 To GridView1.RowCount
            RwX = GridView1.GetDataRow(i - 1)
            If RwX("CeeCod") = Cod Then
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
            TextEdit1.EditValue = RwX("CeeCod")
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
        If TextEdit2.EditValue = "" Then TextEdit2.Focus() : Return False
    End Function
    Sub Registra()
        Dim Ins As String = "Insert into TbCee (CeeCod,CeeDesc,CeeAlt,CeeSt) values (@CeeCod,@CeeDesc,@CeeAlt,@CeeSt)"
        Dim Upd As String = "Update TbCee set CeeDesc=@CeeDesc,CeeAlt=@CeeAlt,CeeSt=@CeeSt where CeeCod=@CeeCod"
        Dim Str As String = ""
        Dim p1 As New SqlParameter("@CeeCod", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@CeeDesc", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@CeeAlt", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@CeeSt", SqlDbType.VarChar)
        If Insert = True Then Str = Ins Else Str = Upd
        Cmd = New SqlCommand(Str, cnCo)
        p1.Value = TextEdit1.EditValue
        p2.Value = TextEdit2.EditValue
        p3.Value = TextEdit3.EditValue
        p4.Value = TextEdit4.EditValue
        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        DeleteCom()
        ButtonF5.PerformClick()
    End Sub
    Private Sub DeleteCom()
        Dim box As Object
        Dim str As String = "Select * from TbPia where Piafl11=" & TextEdit1.EditValue & " and piafl11>0"
        Dim cmd As New SqlCommand(str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            MessageBox.Show("Impossibile eliminare il codice CEE " & TextEdit1.EditValue & " ." & Chr(13) & "Il codice è in uso da uno o più conti.", "ELIMINAZIONE IMPOSSIBILE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            dataRd.Close()
            Return
        End If
        dataRd.Close()
        box = MessageBox.Show("ELIMINO IL CODICE CEE " & TextEdit1.EditValue & " ?", "ELIMINA CODICE CEE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If box = DialogResult.No Then
            Exit Sub
        End If
        cmd = New SqlCommand("Delete from TbCEE where CeeCod='" & TextEdit1.EditValue & "'", cnCo)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim StrPrint As String = "SELECT * FROM TbCee order by CeeCod"
        DsCee = (New DataTable)
        DaCee = (New SqlDataAdapter(StrPrint, cnCo))
        DaCee.SelectCommand.CommandTimeout = 300
        DaCee.Fill(DsCee)
        selectformula = ""
        REPORT = New DxStCee
        REPORT.DataSource = DsCee
        REPORT.DataMember = "DsCee"
        REPORT.FilterString = selectformula
        REPORT.Parameters.Item("Marchio").Value = Marchio()
        REPORT.ShowPreview()
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
End Class