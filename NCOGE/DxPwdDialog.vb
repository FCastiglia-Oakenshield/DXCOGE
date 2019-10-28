Imports DXBASE
Public Class DxPwdDialog
    Private Shared PEsatta As Boolean
    Private Shared Pwd As String
    Public Shared ReadOnly Property Esatta() As Boolean
        Get
            Return PEsatta
        End Get
    End Property

    Public Shared WriteOnly Property Password() As String
        Set(ByVal Value As String)
            Pwd = Value
        End Set
    End Property

    Private Sub DxPwdDialog_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        PEsatta = False
    End Sub

    Private Sub DxPwdDialog_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Password = ""
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If TextEdit1.Text.ToLower = Pwd.ToLower Then
            PEsatta = True
        Else
            PEsatta = False
        End If
        Me.Close()
    End Sub
End Class