Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Public Class DxAccIva
    Dim ANNO, ANNOP, perc As Int16
    Dim dicembre, acconto, totale, newacc As Decimal
    Dim Titolo As String
    Dim OkFl As Boolean = False

    Dim REPORT As New XtraReport
    Dim selectformula, SCRI, StrPrint As String
    Private Sub DxAccIva_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Apertura()
        PulisciDati()
        ComboBoxEdit1.Focus()
    End Sub
    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Stampa()
    End Sub
    Sub Messaggio()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "L'UTENTE NON E' MENSILE!!! "
        style = MsgBoxStyle.Exclamation
        title = "???? CALCOLO ACCONTO IVA ????"
        response = MsgBox(msg, style, title)
    End Sub
    Sub Messaggio1()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "NON SONO PRESENTI DATI PER IL CALCOLO DELL'ACCONTO!!! "
        style = MsgBoxStyle.Exclamation
        title = "???? CALCOLO ACCONTO IVA ????"
        response = MsgBox(msg, style, title)
    End Sub
    Sub Messaggio2()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = " NON E' PRESENTE LA CHIUSURA DI DICEMBRE NELL'ANNO SELEZIONATO!!! "
        style = MsgBoxStyle.Exclamation
        title = "???? CALCOLO ACCONTO IVA ????"
        response = MsgBox(msg, style, title)
    End Sub
    Sub Stampa()
        REPORT = New DxAccontoIva
        REPORT.Parameters("Titolo").Value = "Anno " & ANNO & " - Sviluppo Calcolo Acconto Iva con Metodo Storico"
        REPORT.Parameters("ANNOP").Value = ANNOP
        REPORT.Parameters("acconto").Value = acconto
        REPORT.Parameters("dicembre").Value = dicembre
        REPORT.Parameters("totale").Value = totale
        REPORT.Parameters("Marchio").Value = Marchio()
        REPORT.Parameters("perc").Value = perc
        REPORT.Parameters("newacc").Value = newacc
        REPORT.ShowPreview()
    End Sub
    Sub Apertura()
        Dim Cmd As New SqlCommand("SELECT AZIANNOLAVORO FROM TbAzi where AziRegimeIva = '0' ORDER BY AziAnnoLavoro desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        ComboBoxEdit1.SelectedIndex = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
        End While
        dataRd.Close()
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Exit Sub
        ComboBoxEdit1.SelectedIndex = 0
    End Sub
    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Apertura()
        PulisciDati()
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
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub PulisciDati()
        TextEdit1.EditValue = "88"
        TextEdit2.EditValue = "" : TextEdit3.EditValue = CDec(0.0)
        TextEdit4.EditValue = CDec(0.0) : TextEdit5.EditValue = CDec(0.0)
        TextEdit20.EditValue = CDec(0.0)
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        '''' la procedura è attiva dall'anno 2006 
        If Val(ComboBoxEdit1.EditValue) < 2006 Then
            Messaggio1()
            ComboBoxEdit1.Focus()
            Exit Sub
        End If
        OkFl = False
        ANNO = Val(ComboBoxEdit1.EditValue)
        ANNOP = ANNO - 1
        acconto = 0
        dicembre = 0
        totale = 0
        newacc = 0
        perc = Val(TextEdit1.EditValue)
        PulisciDati()
        verificachiusure()
        If OkFl = False Then
            Messaggio2()
            ComboBoxEdit1.Focus()
            Exit Sub
        End If
        totale = dicembre + acconto
        If totale < 0 Then
            newacc = 0
        Else
            newacc = (totale * perc) / 100
        End If
        If newacc < 103.91 Then newacc = 0
        TextEdit2.EditValue = ANNOP
        TextEdit20.EditValue = acconto
        TextEdit3.EditValue = dicembre
        TextEdit4.EditValue = totale
        TextEdit5.EditValue = newacc
    End Sub
    Private Sub verificachiusure()
        Dim Cmd As New SqlCommand("SELECT IvaVVersam FROM TbVers WHERE (IvaVMese = 12) and IvaVAnno = " & ANNOP, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            dicembre = dataRd.Item("IvaVVersam")
            OkFl = True
        End While
        dataRd.Close()
        Dim Cmd1 As New SqlCommand("SELECT IvaVVersam FROM TbVers WHERE (IvaVMese = 14) and IvaVAnno = " & ANNOP, cnCo)
        dataRd = Cmd1.ExecuteReader
        While dataRd.Read
            acconto = dataRd.Item("IvaVVersam")
        End While
        dataRd.Close()
    End Sub
End Class