Imports System.Data.SqlClient
Imports DXBASE
Public Class DxAccessi

    Dim TbAcc As DataTable
    Dim DaAcc As SqlDataAdapter
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem

    Private Sub DxAccessi_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        TextEdit1.EditValue = ""
        PopolaAnni()
    End Sub
    Sub PopolaAnni()
        ImageComboBoxEdit1.Properties.Items.Clear()
        Dim Str As String = "select distinct Datepart(year,entrata) from tbaccessi order by Datepart(year,entrata) desc"
        Dim SS As String = ""
        Cmd = New SqlCommand(Str, cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item(0), dataRd.Item(0), -1)
            ImageComboBoxEdit1.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Private Sub PopolaGrid()
        Cursor.Current = Cursors.WaitCursor
        TextEdit1.EditValue = ""
        Dim str As String = "Select * from DxVAccessi where not OraUscita is null and Datepart(year,Data) = " & ImageComboBoxEdit1.EditValue & " order by data desc,OraEntrata,Workstation,Programma"
        TbAcc = New DataTable()
        DaAcc = New SqlDataAdapter(str, cnVd)
        DaAcc.Fill(TbAcc)
        GridControl1.DataSource = TbAcc
        TextEdit1.EditValue = Format(TbAcc.Rows.Count, "######")
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If ImageComboBoxEdit1.SelectedIndex > -1 Then PopolaGrid()
    End Sub
End Class