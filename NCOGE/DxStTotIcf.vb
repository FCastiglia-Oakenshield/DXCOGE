Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Imports DevExpress.Data
Imports System.Drawing.Printing

Public Class DxStTotIcf
    Dim REPORT As New XtraReport
    Dim DsIvaCF As DataTable
    Dim DaIvaCF As SqlDataAdapter
    Dim RwIvaCF As DataRow
    Dim Titolo As String
    Private Sub DxStTotIcf_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Apertura()
        ComboBoxEdit1.Focus()
    End Sub
    Sub Messaggio1()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "Selezionare Anno Iva!!! "
        style = MsgBoxStyle.Exclamation
        title = "???? STAMPA TOTALI IVA CLIENTI/FORNITORI ????"
        response = MsgBox(msg, style, title)
    End Sub
    Sub Apertura()
        Dim Cmd As New SqlCommand("SELECT AZIANNOLAVORO FROM TbAzi ORDER BY AziAnnoLavoro desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        ComboBoxEdit1.SelectedIndex = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
        End While
        dataRd.Close()
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Exit Sub
        ComboBoxEdit1.SelectedIndex = 0
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If Val(ComboBoxEdit1.EditValue) < 2000 Then
            Messaggio1()
            ComboBoxEdit1.Focus()
            Exit Sub
        End If
        CaricaDatiIva()
        GridControl1.DataSource = DsIvaCF
        Titolo = Marchio() & " - Dati Iva Clienti/Fornitori Anno " & ComboBoxEdit1.EditValue
        For X As Int16 = 2 To 1 Step -1
            If X = 1 Then GridView1.ActiveFilterString = "[CLoFO] = 'CL'" Else GridView1.ActiveFilterString = "[CLoFO] =  'FO'"
            DXANTEPRIMA(GridControl1, True, PaperKind.A4, Titolo)
        Next
    End Sub
    Sub CaricaDatiIva()
        DsIvaCF = New DataTable
        DaIvaCF = New SqlDataAdapter(" EXEC XDettIva @ANNO = " & ComboBoxEdit1.EditValue, cnCo)
        DaIvaCF.Fill(DsIvaCF)
    End Sub
End Class