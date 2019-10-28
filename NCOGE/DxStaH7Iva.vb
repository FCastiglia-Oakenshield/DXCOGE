Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient

Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Public Class DxStaH7Iva
    Dim Rispondi As MsgBoxResult
    Dim swreg As Int16 = 0

    Dim REPORT As New XtraReport
    Dim selectformula, SCRI, StrPrint, Formula, Titolo As String
    Dim DsAcVE As DataTable
    Dim DaAcVE As SqlDataAdapter
    Private Sub DxStaH7Iva_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        TextEdit1.EditValue = Year(Today)
        Pulizia()
        TextEdit1.Focus()
    End Sub
    Private Sub Pulizia()
        TextEdit2.EditValue = "" : TextEdit3.EditValue = ""
        TextEdit4.EditValue = "" : TextEdit5.EditValue = "" : TextEdit6.EditValue = "" : TextEdit7.EditValue = ""
    End Sub
    Private Sub TextEdit1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit1.EditValueChanged
        If Val(TextEdit1.EditValue) < 2005 Then TextEdit1.EditValue = Year(Today)
        Pulizia()
    End Sub
    Private Sub TextEdit2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit2.EditValueChanged
        swreg = 0
        If Val(TextEdit2.EditValue) > 0 Then leggitiporeg()
        If swreg = 1 Then
            leggipriult()
            TextEdit6.Focus()
        End If
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim str As String
        If Controlli() = False Then
            TextEdit2.Focus()
            Exit Sub
        End If

        str = "SELECT * FROM CRH7IVA where  ANNOIVA = " & TextEdit1.Text & " and PriRegIva = " & TextEdit2.Text & " AND PRINUMPROT between " & TextEdit6.Text & " AND " & TextEdit7.Text & " order by PriNumProt"
        Titolo = "Anno " & TextEdit1.Text & " - Registro N. " & TextEdit2.Text & " " & TextEdit3.Text & " - Protocolli dal N. " & TextEdit6.Text & " al N. " & TextEdit7.Text

        DsAcVE = New DataTable()
        DaAcVE = New SqlDataAdapter(str, cnCo)
        DaAcVE.Fill(DsAcVE)

        selectformula = Formula

        REPORT = New DxStH7Iv
        REPORT.DataSource = DsAcVE
        REPORT.DataMember = "DsAcVE"
        REPORT.FilterString = selectformula
        REPORT.Parameters("Titolo").Value = Titolo
        REPORT.Parameters("Marchio").Value = Marchio()
        REPORT.ShowPreview()
    End Sub
    Function Controlli() As Boolean
        If Val(TextEdit2.EditValue) = 0 Or TextEdit2.EditValue = "" Or swreg = 0 Then
            Messaggio(1, "SELEZIONARE UN NUMERO DI REGISTRO ESISTENTE!!! ")
            Controlli = False
        ElseIf Val(TextEdit6.EditValue) = 0 And Val(TextEdit7.EditValue) = 0 Then
            Messaggio(1, "SELEZIONARE I PROTOCOLLI DA STAMPARE!!! ")
            Controlli = False
        ElseIf Val(TextEdit6.EditValue) > Val(TextEdit7.EditValue) Then
            Messaggio(1, "VERIFICARE I PROTOCOLLI DA STAMPARE!!! ")
            Controlli = False
        ElseIf (Val(TextEdit6.EditValue) < Val(TextEdit4.EditValue)) Or (Val(TextEdit6.EditValue) > Val(TextEdit5.EditValue)) Then
            Messaggio(1, "VERIFICARE I PROTOCOLLI DA STAMPARE!!! ")
            Controlli = False
        ElseIf (TextEdit7.EditValue < TextEdit4.EditValue) Or (Val(TextEdit7.EditValue) > Val(TextEdit5.EditValue)) Then
            Messaggio(1, "VERIFICARE I PROTOCOLLI DA STAMPARE!!! ")
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
        title = "STAMPA PROTOCOLLI IVA"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Private Sub leggipriult()
        Dim cmd As New SqlCommand("select isnull(min(prinumprot),0), isnull(max(prinumprot),0) from tbpri where pricausale = 3 and priregiva = " & Val(TextEdit2.EditValue) & " and YEAR(pridataGio)= " & Val(TextEdit1.EditValue), cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit4.EditValue = dataRd.GetInt32(0)
            TextEdit6.EditValue = dataRd.GetInt32(0)
            TextEdit5.EditValue = dataRd.GetInt32(1)
            TextEdit7.EditValue = dataRd.GetInt32(1)
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
    Private Sub leggitiporeg()
        Dim STRW = "select RIvaDesc from TbRegIva where RIvaNReg = " & Val(TextEdit2.EditValue) & " and RIvaAnno = " & Val(TextEdit1.EditValue)
        Dim cmd As New SqlCommand(STRW, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit3.EditValue = dataRd.GetString(0)
            swreg = 1
        End If
        dataRd.Close()
    End Sub
End Class