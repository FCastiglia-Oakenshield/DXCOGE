Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native

Public Class DxCntIva
    Dim Iset, sw, Irow As Int16
    Dim selectFormula, StrinGString, StrinG1, String2 As String
    Dim ANNO As Int16
    Dim dicembre As Decimal
    Dim OkFl As Boolean = False
    Dim REPORT As New XtraReport
    Dim DsCoIva As DataTable
    Dim DaCoIva As SqlDataAdapter


    Private Sub CntIva_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Apertura()
        ComboBoxEdit1.Focus()
    End Sub
    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        verificadati()
        If OkFl = False Then
            ButtonF5.PerformClick()
            ComboBoxEdit1.Focus()
            Exit Sub
        End If

        ANNO = Val(ComboBoxEdit1.EditValue)
        EsegueSql(" EXEC cmodIva @anno = " & ANNO, cnCo)
        Stampa()
    End Sub
    Sub Messaggio()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "L'UTENTE NON E' MENSILE!!! "
        style = MsgBoxStyle.Exclamation
        title = "???? Controlli per Dichiarazione Iva????"
        response = MsgBox(msg, style, title)
    End Sub
    Sub Messaggio1()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "Selezionare Anno x Controllo Dati Iva!!! "
        style = MsgBoxStyle.Exclamation
        title = "???? Controlli Dichiarazione Iva ????"
        response = MsgBox(msg, style, title)
    End Sub
    Sub Messaggio2()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = " NON E' PRESENTE LA CHIUSURA DI DICEMBRE NELL'ANNO SELEZIONATO!!! "
        style = MsgBoxStyle.Exclamation
        title = "???? Controlli Dichiarazione Iva ????"
        response = MsgBox(msg, style, title)
    End Sub
    Sub Stampa()
        DsCoIva = New DataTable()
        DaCoIva = New SqlDataAdapter("select * from TMPDICIVA", cnCo)
        DaCoIva.Fill(DsCoIva)
        REPORT = New DxControlliva
        REPORT.DataSource = DsCoIva
        REPORT.DataMember = "DsCoIva"
        REPORT.Parameters("Titolo").Value = "Anno " & ANNO & " - Controlli per Dichiarazione Iva"
        REPORT.Parameters("Marchio").Value = Marchio()
        REPORT.ShowPreview()
    End Sub
    Sub Apertura()
        'Dim Cmd As New SqlCommand("SELECT AZIANNOLAVORO FROM TbAzi where AziRegimeIva = '0' ORDER BY AziAnnoLavoro desc", cnCo)
        Dim Cmd As New SqlCommand("SELECT AZIANNOLAVORO FROM TbAzi ORDER BY AziAnnoLavoro desc", cnCo)
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
    Private Sub verificadati()
        OkFl = False
        If Val(ComboBoxEdit1.EditValue) < 2000 Then
            Messaggio1()
            ComboBoxEdit1.Focus()
            Exit Sub
        End If
        ANNO = Val(ComboBoxEdit1.EditValue)
        verificachiusure()
        If OkFl = False Then
            Messaggio2()
            ComboBoxEdit1.Focus()
            Exit Sub
        End If
    End Sub
    Private Sub verificachiusure()
        Dim Cmd As New SqlCommand("SELECT IvaVVersam FROM TbVers WHERE (IvaVMese = 12) and IvaVAnno = " & ANNO, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            dicembre = dataRd.Item("IvaVVersam")
            OkFl = True
        End While
        dataRd.Close()
    End Sub
End Class