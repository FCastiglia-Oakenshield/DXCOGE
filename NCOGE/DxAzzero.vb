Imports DXBASE
Imports System.Data.SqlClient
Public Class DxAzzero
    Dim str As String
    Dim NREG As Int16
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem

    Private Sub DxAzzero_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        ComboBoxEdit1.Focus()
    End Sub
    Sub Pulizia(ByVal n As Boolean)
        RadioGroup1.SelectedIndex = -1
        GroupControl2.Enabled = False
        GroupControl3.Enabled = False
        DateEdit1.EditValue = Today
        ComboBoxEdit2.SelectedIndex = -1
        ComboBoxEdit3.SelectedIndex = -1
        If n = True Then
            str = "select AziAnnoLavoro from tbazi order by aziannolavoro desc"
            Dim cmd As New SqlCommand(str, cnCo)
            dataRd = cmd.ExecuteReader
            ComboBoxEdit1.Properties.Items.Clear()
            While dataRd.Read
                ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
            End While
            dataRd.Close()
            ComboBoxEdit1.SelectedIndex = 0
        End If
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex = 0 Then
            GroupControl2.Enabled = True
            GroupControl3.Enabled = False
            AssegnaDataMinimaGiornale()
        End If
        If RadioGroup1.SelectedIndex = 1 Then
            GroupControl2.Enabled = False
            GroupControl3.Enabled = True
            popolacombo2()
            ComboBoxEdit2.Focus()
        End If
    End Sub
    Sub AssegnaDataMinimaGiornale()
        Dim sstr As String = "select EseDal from tbese where EseAnno=" & ComboBoxEdit1.EditValue
        Dim cmd As New SqlCommand(sstr, cnCo)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            DateEdit1.EditValue = dataRd.Item("EseDal")
        End While
        dataRd.Close()
    End Sub
    Private Sub ComboBoxEdit1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.Leave, ComboBoxEdit1.SelectedIndexChanged
        If ComboBoxEdit1.SelectedIndex > -1 Then AssegnaDataMinimaGiornale()
    End Sub
    Private Sub popolacombo2()
        str = "select RIvaNreg, RIvaDesc from tbRegIva where RIvaAnno = " & ComboBoxEdit1.EditValue & " and RIvaTipo <> 9 order by RIvaNreg"
        Dim cmd As New SqlCommand(str, cnCo)
        ImageComboBoxEdit2.Properties.Items.Clear()
        nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("00 Tutti i Registri", "00", -1)
        ImageComboBoxEdit2.Properties.Items.Add(nn)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("RivaNreg").ToString.PadLeft(2, "0") & " " & dataRd.Item("RIvaDesc"), dataRd.Item("RivaNreg").ToString.PadLeft(2, "0"), -1)
            ImageComboBoxEdit2.Properties.Items.Add(nn)
        End While
        dataRd.Close()
        ImageComboBoxEdit2.SelectedIndex = 0
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If RadioGroup1.SelectedIndex = 0 Then
            EsegueSql(" EXEC XAZZGIO  @UDATA = '" & CDate(DateEdit1.EditValue).ToShortDateString & "'", cnCo)
            Pulizia(False)
            ComboBoxEdit1.Focus()
        ElseIf RadioGroup1.SelectedIndex = 1 Then
            ControlloIva()
        End If
    End Sub
    Private Sub ControlloIva()
        Dim dal, al As Date
        Dim StrX As String = "XAZZIVA"
        If ComboBoxEdit2.SelectedIndex < 0 Then ComboBoxEdit2.Focus() : Exit Sub
        If ComboBoxEdit3.SelectedIndex < 0 Then ComboBoxEdit3.Focus() : Exit Sub
        If ComboBoxEdit2.SelectedIndex > ComboBoxEdit3.SelectedIndex Then ComboBoxEdit2.Focus() : Exit Sub
        NREG = ImageComboBoxEdit2.EditValue
        dal = CDate("01/" & ComboBoxEdit2.SelectedIndex + 1 & "/" & ComboBoxEdit1.EditValue)
        al = CDate(Date.DaysInMonth(ComboBoxEdit1.EditValue, ComboBoxEdit3.SelectedIndex + 1) & "/" & ComboBoxEdit3.SelectedIndex + 1 & " / " & ComboBoxEdit1.EditValue)
        If CDate(al).Year < 2019 Then StrX = "XAZZIVA2018"
        EsegueSql(" EXEC " & StrX & "  @DAL = '" & dal.ToShortDateString & "', @AL = '" & al.ToShortDateString & "', @NREG = " & NREG, cnCo)
        Pulizia(False)
        ComboBoxEdit1.Focus()
    End Sub

    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            ButtonF11.PerformClick()
            Exit Sub
        End If
    End Sub

End Class