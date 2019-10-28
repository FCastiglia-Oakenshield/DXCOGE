Imports DXBASE
Imports System.Data.SqlClient

Public Class DxBlack
    Dim DsRip As DataTable
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow
    Dim OkMondo As Boolean
    Dim UserId As String = ""
    Private Sub DxBlack_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        'If GestioneUser() = False Then Me.Close() : Exit Sub
        Pulizia()
        DateEdit1.Focus()
    End Sub
    Function GestioneUser() As Boolean
        REM MONDOMARINE
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UserId = dataRd.Item("Sel14")
        End While
        dataRd.Close()
        If UserId Is DBNull.Value Then Return False
        If UserId.ToUpper = "MONDOMARINE" Then Return True Else Return False
    End Function
    Sub Pulizia()
        DsRip = New DataTable
        GridControl1.DataSource = DsRip
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Controllo() = False Then Exit Sub
        Pulizia()
        GroupControl2.Text = Format(DateEdit1.EditValue, "dd/MM/yyyy") & " - " & Format(DateEdit2.EditValue, "dd/MM/yyyy")
        Cursor.Current = Cursors.WaitCursor
        Dim StrPrint As String = "select * from VBlackList where DataReg between '" & Format(DateEdit1.EditValue, "dd/MM/yyyy") & "' and '" & Format(DateEdit2.EditValue, "dd/MM/yyyy") & "' order by DataReg"
        DsRip = New DataTable
        DaRip = New SqlDataAdapter(StrPrint, cnCo)
        DaRip.SelectCommand.CommandTimeout = 300
        Try
            DaRip.Fill(DsRip)
        Catch ex As Exception
            MsgBox("BLACK LIST NON ABILITATA!!! chiamare la Selco")
        End Try
        GridControl1.DataSource = DsRip
        GridView1.ClearSelection()
        GridView1.ExpandAllGroups()
    End Sub
    Private Sub DateEdit1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If DateEdit1.EditValue Is Nothing Then DateEdit1.ErrorText = "Manca Data Inizio" Else DateEdit1.ErrorText = ""
    End Sub
    Private Sub DateEdit2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If DateEdit2.EditValue Is Nothing Then DateEdit2.ErrorText = "Manca Data Fine" Else DateEdit2.ErrorText = ""
    End Sub
    Function Controllo() As Boolean
        Dim Msg As Boolean = True
        If DateEdit1.EditValue Is Nothing Then
            DateEdit1.ErrorText = "Manca Data Inizio"
            Msg = False
        End If
        If DateEdit2.EditValue Is Nothing Then
            DateEdit2.ErrorText = "Manca Data Fine"
            Msg = False
        End If
        If DateEdit1.EditValue > DateEdit2.EditValue Then
            DateEdit1.ErrorText = "Data Inizio > Data Fine"
            Msg = False
        End If
        Return Msg
    End Function
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If DsRip.Rows.Count = 0 Then Exit Sub
        DXANTEPRIMA(GridControl1, True, Printing.PaperKind.A4, GroupControl2.Text)
    End Sub
    Private Sub DDxBlack_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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
        If e.KeyData = Keys.F1 Then
            e.Handled = True
            ButtonF1.PerformClick()
            Exit Sub
        End If
    End Sub
End Class