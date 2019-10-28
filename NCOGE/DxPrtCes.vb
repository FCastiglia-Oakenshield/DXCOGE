Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Public Class DxPrtCes
    Dim frm As New LpLp
    Dim Rpt As ReportClass
    Dim Rpt1 As New StArcCesp
    Dim Rpt2 As New SinCespi
    Dim Rpt3 As New EleCes
    Dim ANNO As Int16
    Dim str As String
    Private Sub DxPrtCes_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        RadioGroup1.SelectedIndex = 0
        ComboBoxEdit1.Focus()
    End Sub
    Private Sub Pulizia(ByVal Tutto As Boolean)
        str = "select AziAnnoLavoro from tbazi order by aziannolavoro desc"
        Dim cmd As New SqlCommand(str, cnCo)
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
        Rpt1 = New StArcCesp
        Rpt2 = New SinCespi
        Rpt3 = New EleCes
        If RadioGroup1.SelectedIndex = 0 Then
            Rpt = Rpt1
            frm.Text = "Analitico  Cespiti"
        ElseIf RadioGroup1.SelectedIndex = 1 Then
            Rpt = Rpt2
            frm.Text = "Sintetico  Cespiti"
        Else
            Rpt = Rpt3
            frm.Text = "Elenco  Cespiti"
        End If
        ANNO = ComboBoxEdit1.EditValue
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("Anno", ANNO)
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