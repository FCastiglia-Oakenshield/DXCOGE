Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Imports DevExpress.Data
Imports System.Drawing.Printing
Public Class DxInvest
    Dim REPORT As New XtraReport
    Dim Titolo, Scelta As String
    Dim TbPCF As DataTable
    Dim DaPCF As SqlDataAdapter
    Dim Str As String


    Dim DsRie As DataTable
    Dim DaRie As SqlDataAdapter
    Dim RwRie As DataRow
    Private Sub DxStaSic_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Pulizia(True)
        CaricaDati()
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
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyData = Keys.F9 Then
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        CaricaDati()
    End Sub
    Sub PuliziaRIEP()
        DsRie = New DataTable
        GridControl1.DataSource = DsRie
    End Sub
    Sub LANCIO(strm As String)
        PuliziaRIEP()
        GroupControl5.Text = " BENI STRUMENTALI ACQUISTATI NELL'ANNO " & ComboBoxEdit1.EditValue
        Cursor.Current = Cursors.WaitCursor

        DsRie = New DataTable
        DaRie = New SqlDataAdapter(strm, cnCo)
        DaRie.SelectCommand.CommandTimeout = 300
        DaRie.Fill(DsRie)
        GridControl1.DataSource = DsRie
    End Sub

    Sub CaricaDati()
        Cmd = New SqlCommand("SELECT * from TbEse where EseAnno = " & ComboBoxEdit1.EditValue, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            DateEdit2.EditValue = dataRd.Item("EseDal")
            DateEdit3.EditValue = dataRd.Item("EseAl")
        End If
        dataRd.Close()
        Dim STRM As String = "EXEC XCESPITIANNO @ANNO=" & ComboBoxEdit1.EditValue
        LANCIO(STRM)
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Titolo = Marchio() & " - Beni Strumentali Acquistati nell' Esercizio " & ComboBoxEdit1.EditValue & " ( " & CDate(DateEdit2.EditValue).ToShortDateString & " - " & CDate(DateEdit3.EditValue).ToShortDateString & " )"
        DXANTEPRIMA(GridControl1, True, PaperKind.A4, Titolo)
    End Sub

End Class