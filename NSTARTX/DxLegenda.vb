Imports DXBASE
Imports System.Data.SqlClient


Public Class DxLegenda
    Dim DsTas As DataTable
    Dim DaTas As SqlDataAdapter

    Private Sub DxLegenda_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        LeggiTasti()
        ButtonF9.Focus()
    End Sub

    Private Sub LeggiTasti()
        DsTas = New DataTable
        DaTas = New SqlDataAdapter("select * from TbTasti", cnVd)
        DaTas.Fill(DsTas)
        GridControl1.DataSource = DsTas
    End Sub

    Private Sub DxLegenda_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        DXANTEPRIMA(GridControl1, False, Printing.PaperKind.A4, Me.Text)
    End Sub
End Class