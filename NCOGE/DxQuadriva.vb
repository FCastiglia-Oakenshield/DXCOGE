Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native

Public Class DxQuadriva
    Dim REPORT As New XtraReport
    Dim selectformula, SCRI, StrPrint, Formula, Titolo, str As String

    Dim DsAcVE As DataTable
    Dim DaAcVE As SqlDataAdapter
    Private Sub DxQuadriva_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        RadioGroup1.SelectedIndex = 0
        ComboBoxEdit1.Focus()
    End Sub
    Private Sub Pulizia(ByVal Tutto As Boolean)
        str = "select AziAnnoLavoro from tbazi order by aziannolavoro desc"
        Dim cmd As New SqlCommand(str, cnCo)
        dataRd = cmd.ExecuteReader
        ComboBoxEdit1.Properties.Items.Clear()
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
        End While
        dataRd.Close()
        ComboBoxEdit1.SelectedIndex = 0
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Stampa()
    End Sub
    
    Sub Stampa()
        Dim str As String
        str = ""
        If RadioGroup1.SelectedIndex = 0 Then
            str = "SELECT * FROM CRQUADIVAVEND WHERE anno = " & ComboBoxEdit1.Text & " and (imponibile<>0 or iva<>0) order by PriCoavere"
            REPORT = New DxDetVendite
            Titolo = "Quadratura I.v.a Vendite Anno " & ComboBoxEdit1.Text
        ElseIf RadioGroup1.SelectedIndex = 1 Then
            str = "SELECT * FROM CRQUADIVACQ WHERE anno = " & ComboBoxEdit1.Text & " and (imponibile<>0 or iva<>0) order by PriCoDare"
            REPORT = New DxDetAcquisti
            Titolo = "Quadratura I.v.a Acquisti Anno " & ComboBoxEdit1.Text
        Else
            str = "SELECT * FROM CRQUADCORRISP WHERE anno = " & ComboBoxEdit1.Text & " and imponibile<>0 order by PriCoAvere"
            REPORT = New DxDetCorr
            Titolo = "Quadratura I.v.a Corrispettivi Anno " & ComboBoxEdit1.Text
        End If

        DsAcVE = New DataTable()
        DaAcVE = New SqlDataAdapter(Str, cnCo)
        DaAcVE.Fill(DsAcVE)


        selectformula = Formula

        REPORT.DataSource = DsAcVE
        REPORT.DataMember = "DsAcVE"
        REPORT.FilterString = selectformula
        REPORT.Parameters("Titolo").Value = Titolo
        REPORT.Parameters("Marchio").Value = Marchio()
        REPORT.ShowPreview()
    End Sub
    Private Sub RegIva_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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
End Class