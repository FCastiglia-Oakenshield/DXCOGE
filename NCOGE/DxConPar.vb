Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data.SqlClient
Imports System.IO
Public Class DxConPar
    Dim Rpt As ReportClass
    Dim Rpt1 As New StConPar
    Private Sub DxConPar_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        EsegueSql(" EXEC XDIFFERENZE", cnCo)
        ButtonF9.Focus()
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Cursor.Current = Cursors.WaitCursor
        Dim frm As New LpLp
        Rpt = New ReportClass
        Rpt1 = New StConPar
        Rpt = Rpt1
        Rpt.SetParameterValue("Marchio", Marchio)
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
End Class