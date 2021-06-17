Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Imports DevExpress.Data
Imports System.Drawing.Printing

Public Class DxStTotIcli
    Dim REPORT As New XtraReport
    Dim DsIvaCF As DataTable
    Dim DaIvaCF As SqlDataAdapter
    Dim RwIvaCF As DataRow
    Dim controllo As Boolean
    Dim Rispondi As MsgBoxResult
    Private Sub DxStTotIcf_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Apertura()
    End Sub
    Sub Apertura()
        DateEdit1.EditValue = "01/01/" & Year(Today)
        DateEdit2.EditValue = Today
        TextEdit10.EditValue = ""
        TextEdit10.Focus()
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
        If Controlli() = False Then
            DateEdit1.Focus()
            Exit Sub
        End If
        CaricaDatiIva()
        GridControl1.DataSource = DsIvaCF
        DXANTEPRIMA(GridControl1, True, PaperKind.A4, TextEdit10.EditValue.ToString)
    End Sub
    Function Controlli() As Boolean
        Controlli = True
        If CDate(DateEdit2.EditValue) < CDate(DateEdit1.EditValue) Then
            Messaggio(1, "VERIFICARE PERIODO DAL - AL !!!")
            Controlli = False
        End If
    End Function
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "Report Totali Iva Clienti"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Sub CaricaDatiIva()
        DsIvaCF = New DataTable
        DaIvaCF = New SqlDataAdapter(" EXEC XXDettIva @DAL = '" & DateEdit1.EditValue.ToString & "',@AL = '" & DateEdit2.EditValue.ToString & "'", cnCo)
        DaIvaCF.Fill(DsIvaCF)
    End Sub
End Class