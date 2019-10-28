Imports DXBASE
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors

Public Class DxPaindex
    Dim Em As New DevExpress.XtraEditors.Controls.CheckedListBoxItem
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim DaArt As SqlDataAdapter
    Dim TbArt As DataTable
    Dim RwX As DataRow
    Dim ArtPNota As Int16 = 0
    Dim ArtSigla As String = ""
    Dim OkSet As Boolean = False

    Private Sub DxPaindex_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        CaricaTabelle()
        If OkSet = True Then GroupControl3.Focus() Else CheckedComboBoxEdit5.Focus()
    End Sub
    Sub CaricaTabelle()
        REM Codice Iva X pubblica mministrazione
        CheckedComboBoxEdit5.Properties.Items.Clear()
        Cmd = New SqlCommand("select * from TbCii where CiiTp < 2 and CiiDes > '' Order by CiiCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(dataRd.Item("CiiCod"), dataRd.Item("CiiDes").ToString, CheckState.Unchecked)
            CheckedComboBoxEdit5.Properties.Items.Add(Em)
        End While
        dataRd.Close()
        REM ASSEGNA CHECK
        Cmd = New SqlCommand("select * from TbPaCii where PaTipo = 'IVA' Order by PaCodIva", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SettaCheckedCombo(CheckedComboBoxEdit5, dataRd.Item("PaCodIva"))
            OkSet = True
        End While
        dataRd.Close()
        ImageComboBoxEdit2.Properties.Items.Clear()
        Dim Str As String = "SELECT CiiCod,CiiCau from TbCii where CiiCod > 3 order by CiiCod"
        Dim SS As String = "-1 NO GIROCONTO"
        nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, 0, -1)
        ImageComboBoxEdit2.Properties.Items.Add(nn)
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
            ImageComboBoxEdit2.Properties.Items.Add(nn)
        End While
        dataRd.Close()
        REM LEGGICAUSALE
        Cmd = New SqlCommand("select * from TbPaCii where PaTipo = 'PNO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ArtPNota = dataRd.Item("PaCodIva")
        End While
        dataRd.Close()
        ImageComboBoxEdit2.SelectedIndex = SettaComboImage(ImageComboBoxEdit2, ArtPNota)
    End Sub
    Function SettaCheckedCombo(ByVal CheckC As CheckedComboBoxEdit, ByVal Id As Integer) As Boolean
        If Id = -1 Then GoTo II
        For x As Int16 = 1 To CheckC.Properties.Items.Count
            If CheckC.Properties.Items(x - 1).Value = Id Then
                CheckC.Properties.Items(x - 1).CheckState = CheckState.Checked
                SettaCheckedCombo = True : Exit Function
            End If
        Next
II:
        SettaCheckedCombo = False
    End Function

    Private Sub ButtonF11_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF11.Click
        RegistraModifiche()
        Me.Close()
    End Sub
    Sub RegistraModifiche()
        Cursor.Current = Cursors.WaitCursor
        REM RIASSEGNA CHECK
        Dim JJ As Object
        Dim JC As Int16 = 0
        Dim JD As String = ""
        Dim DCmd As New SqlCommand("Delete from TbPaCii", cnCo)
        DCmd.ExecuteNonQuery()
        For i As Int16 = 1 To CheckedComboBoxEdit5.Properties.Items.Count
            Em = CheckedComboBoxEdit5.Properties.Items(i - 1)
            If Em.CheckState = CheckState.Checked Then
                JJ = Em.Value
                JD = Em.Description
                Cmd = New SqlCommand("INSERT INTO TbPaCii(PaTipo,PaCodIva,PaDesciva) Values ('IVA'," & JJ & ",'" & JD & "')", cnCo)
                Cmd.ExecuteNonQuery()
            End If
        Next
        ArtPNota = ImageComboBoxEdit2.EditValue
        ArtSigla = Mid(ImageComboBoxEdit2.SelectedItem.ToString, 4, 12)
        Cmd = New SqlCommand("INSERT INTO TbPaCii(PaTipo,PaCodIva,PaDesciva) Values ('PNO'," & ArtPNota & ",'" & ArtSigla & "')", cnCo)
        Cmd.ExecuteNonQuery()
        Cursor.Current = Cursors.Default
    End Sub
    Sub DxPaindex_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ImageComboBoxEdit2_CloseUp(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles ImageComboBoxEdit2.CloseUp
        System.Windows.Forms.SendKeys.Send("{TAB}")
    End Sub
End Class