Imports System.Data.SqlClient
Imports DXBASE
Public Class DxEliminaLock

    Private Sub DxEliminaLock_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        GroupControl1.Text = "ELIMINA LOCK"
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        Cmd = New SqlCommand("delete from TMPLOCK", cnCo)
        Cmd.ExecuteNonQuery()
        GroupControl1.Text = "OK LOCK ELIMINATO!!!"
    End Sub
End Class