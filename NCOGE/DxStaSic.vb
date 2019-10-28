Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Imports DevExpress.Data
Imports System.Drawing.Printing

Public Class DxStaSic
    Dim REPORT As New XtraReport
    Dim Titolo, TipoAna, Scelta As String
    Dim TbPCF As DataTable
    Dim DaPCF As SqlDataAdapter
    Dim Str As String
    Private Sub DxStaSic_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Pulizia(True)
        RadioGroup1.SelectedIndex = 0
        CaricaDatiIva()
    End Sub
    Private Sub Pulizia(ByVal Tutto As Boolean)
        If Tutto = True Then
            Str = "select AziAnnoLavoro from tbazi order by aziannolavoro desc"
            Dim cmd As New SqlCommand(Str, cnCo)
            dataRd = cmd.ExecuteReader
            ComboBoxEdit1.Properties.Items.Clear()
            ComboBoxEdit1.SelectedIndex = -1
            While dataRd.Read
                ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
            End While
            dataRd.Close()
            ComboBoxEdit1.SelectedIndex = 0
        End If
    End Sub
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Titolo = Marchio() & Scelta & " - Progressivi Iva Anno " & ComboBoxEdit1.EditValue
        DXANTEPRIMA(GridControl1, True, PaperKind.A4, Titolo)
    End Sub
    Sub CaricaDatiIva()
        TipoAna = ""
        Scelta = ""
        If RadioGroup1.SelectedIndex = 0 Then
            TipoAna = "CL"
            Scelta = " - Clienti "
        Else
            TipoAna = "FO"
            Scelta = " - Fornitori"
        End If
        Dim Strcf As String = "Select Anno,AnaGrp, Codice, AnaDesc,PIva,AnaCfis,AnaIndirizzo,AnaCap, AnaCitta, AnaProv, Imponibile, Iva, TFAT from DXProgCF where Anno = " & ComboBoxEdit1.EditValue & " and AnaGrp = '" & TipoAna & "'"
        TbPCF = New DataTable()
        DaPCF = New SqlDataAdapter(Strcf, cnCo)
        DaPCF.Fill(TbPCF)
        GridControl1.DataSource = TbPCF
        GridView1.ClearSelection()
        GridView1.UnselectRow(0)
    End Sub
    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged, ComboBoxEdit1.SelectedIndexChanged
        TipoAna = ""
        Scelta = ""
        If RadioGroup1.SelectedIndex = 0 Then
            TipoAna = "CL"
            Scelta = " - Clienti "
        Else
            TipoAna = "FO"
            Scelta = " - Fornitori"
        End If
        CaricaDatiIva()
    End Sub
End Class