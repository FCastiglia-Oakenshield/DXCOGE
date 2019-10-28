Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Public Class DxStReg
    Dim DaH7 As SqlDataAdapter
    Dim DsH7 As DataTable
    Dim REPORT As New XtraReport
    Dim selectformula As String
    Dim Titolo As String
    Dim danr, anr As Int32
    Dim Rispondi As MsgBoxResult
    Dim sw As Int16 = 0
    Private Sub DxStReg_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If sw = 0 Then
            inizio()
            leggipriult()
            sw = 1
        End If
        TextEdit3.EditValue = 0
        TextEdit4.EditValue = 0
        TextEdit3.Focus()
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If Controlli() = False Then
            TextEdit3.Focus()
            Exit Sub
        End If
        TextEdit3.Focus()


        danr = TextEdit3.EditValue
        anr = TextEdit4.EditValue

        Titolo = "Registrazioni Prima Nota Periodo " & DateEdit1.EditValue & " - " & DateEdit2.EditValue & "  Articoli dal N. " & danr & " al N. " & anr

        Dim StrPrint As String = "SELECT * FROM CRH7PN WHERE (CRH7PN.PriNumProt >= " & danr & " and CRH7PN.PriNumProt<= " & anr & ") and (CRH7PN.PriDataEst >='" & DateEdit1.EditValue & "' and CRH7PN.PriDataEst <= '" & DateEdit2.EditValue & "')"
        DsH7 = (New DataTable)
        DaH7 = (New SqlDataAdapter(StrPrint, cnCo))
        DaH7.SelectCommand.CommandTimeout = 300
        DaH7.Fill(DsH7)
        selectformula = ""
        REPORT = New DxStH7PN
        REPORT.DataSource = DsH7
        REPORT.DataMember = "DsH7"
        REPORT.FilterString = selectformula
        REPORT.Parameters.Item("Marchio").Value = Marchio()
        REPORT.Parameters.Item("Titolo").Value = Titolo
        REPORT.ShowPreview()
    End Sub
    Function Controlli() As Boolean
        If Val(TextEdit3.EditValue) = 0 And Val(TextEdit4.EditValue) = 0 Then
            Messaggio(1, "SELEZIONARE GLI ARTICOLI DA STAMPARE!!! ")
            Controlli = False
        ElseIf (Val(TextEdit3.EditValue) > Val(TextEdit4.EditValue)) Then
            Messaggio(1, "VERIFICARE GLI ARTICOLI DA STAMPARE!!! ")
            Controlli = False
        ElseIf (Val(TextEdit3.EditValue) < Val(TextEdit1.EditValue)) Or (Val(TextEdit3.EditValue) > Val(TextEdit2.EditValue)) Then
            Messaggio(1, "VERIFICARE GLI ARTICOLI DA STAMPARE!!! ")
            Controlli = False
        ElseIf Val(TextEdit4.EditValue < Val(TextEdit1.EditValue)) Or Val(TextEdit4.EditValue > Val(TextEdit2.EditValue)) Then
            Messaggio(1, "VERIFICARE GLI ARTICOLI DA STAMPARE!!! ")
            Controlli = False
        Else
            Controlli = True
        End If
    End Function
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "STAMPA ARTICOLI PRIMA NOTA"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Private Sub inizio()
        DateEdit1.EditValue = CDate("01/01/" & Year(Today)).ToShortDateString
        DateEdit2.EditValue = CDate(Today).ToShortDateString
    End Sub
    Private Sub leggipriult()
        Dim cmd As New SqlCommand("select isnull(min(prinumprot),0), isnull(max(prinumprot),0) from tbpri where pricausale > 3 and priregiva = 0 and (pridataest between '" & DateEdit1.EditValue & "' AND '" & DateEdit2.EditValue & "') ", cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit1.EditValue = dataRd.GetInt32(0)
            TextEdit2.EditValue = dataRd.GetInt32(1)
        End If
        dataRd.Close()
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub DateEdit1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.EditValueChanged, DateEdit2.EditValueChanged
        If sw = 1 Then leggipriult()
    End Sub
End Class