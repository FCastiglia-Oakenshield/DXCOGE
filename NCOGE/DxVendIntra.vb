Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports DevExpress.XtraReports.UI    'Importa l'interfaccia grafica di DevExpress.
Public Class DxVendIntra
    Dim Rpt As ReportClass
    Dim Rpt1 As New Intra1bis
    Dim periodo, desiva As String
    Dim CIVA, SS As Int16
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem

    Dim controllo As Boolean
    Dim Rispondi As MsgBoxResult
    Private Sub DxVendIntra_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia()
    End Sub
    Private Sub Pulizia()
        DateEdit1.EditValue = CDate("01/01/" & Today.Year)
        DateEdit2.EditValue = Today
        PopolaCii()
    End Sub
    Sub PopolaCii()
        ImageComboBoxEdit4.Properties.Items.Clear()
        Dim Str As String = "SELECT CiiCod,CiiDes from TbCii order by CiiCod"
        Dim SS As String = "0 "
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiDes")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiDes"), -1)
            ImageComboBoxEdit4.Properties.Items.Add(nn)
        End While
        dataRd.Close()
        ImageComboBoxEdit4.SelectedIndex = -1
    End Sub
    Private Sub preparaparametri()
        EsegueSql(" EXEC XINTRACEE @codiva = '" & CIVA & "',@DAL = '" & CDate(DateEdit1.EditValue) & "',@AL = '" & CDate(DateEdit2.EditValue) & "'", cnCo)
    End Sub
    Function Controlli() As Boolean
        Dim MSG As String = ""
        Controlli = True
        If CDate(DateEdit2.EditValue) < CDate(DateEdit1.EditValue) Then
            MSG &= "VERIFICARE PERIODO DAL - AL !!!" & Chr(10)
        End If
        If ImageComboBoxEdit4.SelectedIndex = -1 Then
            MSG &= "SELEZIONARE UN CODICE IVA !!!" & Chr(10)
        End If
        If MSG.Length > 0 Then
            Messaggio(1, MSG)
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
        title = "Tabulato Vendite Intra"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click

        'StampaCr()         'Lancia  CRISTALREPORT
        StampaDevexp()    'Lancia DevExpress

    End Sub

    Private Sub StampaCr()

        If Controlli() = False Then DateEdit1.Focus() : Exit Sub
        periodo = " Cessioni Intracomunitarie di Beni periodo " & DateEdit1.EditValue & " - " & DateEdit2.EditValue
        CIVA = ImageComboBoxEdit4.SelectedIndex + 1
        preparaparametri()
        Dim frm As New LpLp
        Rpt = New ReportClass
        Rpt1 = New Intra1bis
        Rpt = Rpt1
        Cursor.Current = Cursors.WaitCursor


        Rpt.SetParameterValue("desiva", ImageComboBoxEdit4.EditValue.ToString)
        Rpt.SetParameterValue("Periodo", periodo)
        Rpt.SetParameterValue("CIVA", CIVA)
        Rpt.SetParameterValue("Marchio", Marchio)

        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()

    End Sub

    Private Sub StampaDevexp()
        Dim TipoDo As String = "IN"
        Dim FileName As String = PathSto & TipoDo & ".pdf"

        Dim REPORT As New XtraReport
        REPORT = New XIntra1bis

        If Controlli() = False Then DateEdit1.Focus() : Exit Sub
        periodo = " Cessioni Intracomunitarie di Beni periodo " & DateEdit1.EditValue & " - " & DateEdit2.EditValue
        CIVA = ImageComboBoxEdit4.SelectedIndex + 1
        preparaparametri()
        Dim TbOrd As DataTable
        Dim DaOrd As SqlDataAdapter
        DaOrd = New SqlDataAdapter("SELECT * FROM TMPINTRA", cnCo)
        TbOrd = New DataTable("Ordini")     'Crea una nuova tabella
        DaOrd.Fill(TbOrd)

        Cursor.Current = Cursors.WaitCursor

        Dim opt As New DevExpress.XtraPrinting.PdfExportOptions  'Da qui fino a End Sub prepara e crea il pdf
        opt.Compressed = True


        REPORT.Parameters.Item("desiva").Value = ImageComboBoxEdit4.EditValue.ToString
        REPORT.Parameters.Item("Periodo").Value = periodo
        REPORT.Parameters.Item("CIVA").Value = CIVA
        REPORT.Parameters.Item("Marchio").Value = Marchio()

        REPORT.DataSource = TbOrd
        REPORT.DataMember = "Ordini"
        REPORT.ShowPrintMarginsWarning = False
        REPORT.CreateDocument()
        REPORT.ShowPreviewDialog

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
End Class