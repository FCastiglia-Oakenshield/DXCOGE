Imports NCCOM

Public Class XtraForm1
    Dim WithEvents UserFat As New DxFtOttica
    Private Sub XtraForm1_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        TextEdit1.EditValue = 0
    End Sub

    Sub PaginaUserFtOttica()
        UserFat = New DxFtOttica
        UserFat._TotaleFattura = TextEdit1.EditValue
        UserFat.Parent = PanelControl8
        UserFat.Dock = DockStyle.Fill
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As System.EventArgs) Handles SimpleButton1.Click
        PaginaUserFtOttica()
    End Sub

End Class