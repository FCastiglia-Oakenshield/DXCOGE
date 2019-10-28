Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Public Class DxCon770
    Dim frm As New LpLp
    Dim Str As String
    Dim Rpt As ReportClass
    Dim Rpt1 As New StCon770
    Dim Anno As Int16
    Private Sub DxCon770_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        ComboBoxEdit1.Focus()
    End Sub
    Private Sub Pulizia(ByVal Tutto As Boolean)
        Str = "select AziAnnoLavoro from tbazi order by aziannolavoro desc"
        Dim cmd As New SqlCommand(Str, cnCo)
        dataRd = cmd.ExecuteReader
        ComboBoxEdit1.Properties.Items.Clear()
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
        End While
        dataRd.Close()
        ComboBoxEdit1.SelectedIndex = 0
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Cursor.Current = Cursors.WaitCursor
        frm = New LpLp
        Rpt = New ReportClass
        Rpt1 = New StCon770
        Rpt = Rpt1

        Anno = ComboBoxEdit1.EditValue
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("Anno", Anno)
        frm.reportsource = Rpt
        frm.Show()
    End Sub
    Private Sub RegIva_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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
    End Sub
End Class