Imports DXBASE
Imports System.IO
Public Class DxDisplay
    Private Shared EPERCORSO As FileInfo
    Private Shared NOMEF As String

    Public Shared Property Percorso As FileInfo
        Get
            Return EPERCORSO
        End Get
        Set(ByVal Value As FileInfo)
            EPERCORSO = Value
        End Set
    End Property

    Public Shared Property NOMET As String
        Get
            Return NOMEf
        End Get
        Set(ByVal Value As String)
            NOMEF = Value
        End Set
    End Property

    Private Sub DxDisplay_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        GroupControl1.Text = NOMEF
        WebBrowser2.Stop()
        WebBrowser2.Navigate(EPERCORSO.FullName)
    End Sub
End Class