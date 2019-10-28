Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data.SqlClient
Imports System.IO
Public Class DxConAge
    Dim frm As New LpLp
    Dim Str As String
    Dim Rpt As ReportClass
    Dim Rpt1 As New StConAge
    Dim Rpt2 As New SkeEnasarco
    Dim Anno As Int16
    Private Sub DxConAge_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        RadioGroup1.SelectedIndex = 0
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
        Rpt1 = New StConAge
        Rpt2 = New SkeEnasarco
        If RadioGroup1.SelectedIndex = 0 Then
            Rpt = Rpt1
        Else
            Rpt = Rpt2
        End If
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