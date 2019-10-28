Imports DXBASE
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports NCCOM
Imports NPRINT

Public Class DxVentAnn
    Dim REPORT As New XtraReport
    Dim selectformula, SCRI As String
    Dim DsVent As DataTable
    Dim DaVent As SqlDataAdapter

    Private Sub DxVentAnn_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        AnniIva()
        Pulizia()
    End Sub
    Private Sub AnniIva()
        ComboBoxEdit1.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT distinct top 5 RivaAnno from TbRegIva Order by RivaAnno desc", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("RIvaAnno"))
        End While
        dataRd.Close()
    End Sub
    Sub Pulizia()
        ComboBoxEdit1.SelectedIndex = -1
    End Sub
    Function Controllo() As Boolean
        Dim Msg As Boolean = True
        If ComboBoxEdit1.SelectedIndex = -1 Then
            ComboBoxEdit1.ErrorText = "Selezionare Anno Iva"
            Msg = False
        End If
        Return Msg
    End Function

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If ComboBoxEdit1.SelectedIndex < 0 Then Exit Sub
        EsegueSql("EXEC WWVENTILA @ANNO = " & ComboBoxEdit1.EditValue, cnCo)
        Dim StrPrint As String = "SELECT * FROM TMPVENTILA"
        DsVent = New DataTable
        DaVent = New SqlDataAdapter(StrPrint, cnCo)
        DaVent.SelectCommand.CommandTimeout = 300
        DaVent.Fill(DsVent)
        selectformula = ""
        REPORT = New StVentila
        REPORT.DataSource = DsVent
        REPORT.DataMember = "DsVent"
        REPORT.FilterString = selectformula
        REPORT.Parameters.Item("Marchio").Value = Marchio()
        REPORT.ShowPreview()
    End Sub
    Private Sub ButtonF5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Pulizia()
        ComboBoxEdit1.Focus()
    End Sub
    Private Sub DxVentAnn_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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