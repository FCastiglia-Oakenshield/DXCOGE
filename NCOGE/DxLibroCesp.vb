Imports NCCOM
Imports NPRINT
Imports DXBASE
Imports System.Data.SqlClient

Public Class DxLibroCesp
    Dim PS As Int16
    Dim AZI, TIPOLP(), RESETLP() As String
    Dim DataMin, DataMax As Date
    Dim Periodo As String
    Dim Anno As Int16 = Today.Year

    Private Sub DxLibroCesp_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Pulizia()
        Inizializza()
        TextEdit1.Focus()
    End Sub
    Sub Pulizia()
        TextEdit2.EditValue = "00"
        TextEdit3.EditValue = ""
        TextEdit4.EditValue = "00"
        TextEdit5.EditValue = ""
        TextEdit6.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = ""
        CheckEdit1.Checked = False
        CheckEdit2.Checked = True
        CheckEdit3.Checked = True
    End Sub
    Private Sub Inizializza()
        Dim Str As String = "select MAX(CespUltAnnoAmm) from TbCesp"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Anno = dataRd.Item(0)
        End If
        dataRd.Close()
        TextEdit1.EditValue = Anno
        TextEdit6.EditValue = "LIBRO CESPITI AMMORTIZZABILI"
        Cmd = New SqlCommand("select min(CespCat) as CatMin,max(CespCat) as CatMax from TbCesp", cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit2.EditValue = dataRd.Item("CatMin")
            TextEdit4.EditValue = dataRd.Item("CatMax")
        End If
        dataRd.Close()
        LeggiCat(TextEdit2.EditValue, TextEdit3.EditValue)
        LeggiCat(TextEdit4.EditValue, TextEdit5.EditValue)
        LeggiAzi(TextEdit1.EditValue)
    End Sub
    Private Sub LeggiCat(ByRef Cat As String, ByRef Desc As String)
        Dim Str As String = "select * from VCspAzi where cspnum='" & Cat.Trim.PadLeft(2, "0") & "'"
        Dim cmd As New SqlCommand(Str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            Desc = dataRd.Item("CspDesc")
        Else
            Desc = ""
        End If
        dataRd.Close()
    End Sub
    Private Sub LeggiAzi(ByVal P As Integer)
        Cmd = New SqlCommand("SELECT * from TbAzi where AziAnnoLavoro = " & P, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            AZI = dataRd.Item("AziCod")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT  * from TbEse where EseAnno = " & P, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            DataMin = dataRd.Item("EseDal")
            DataMax = dataRd.Item("EseAl")
        End If
        dataRd.Close()
    End Sub
    Private Sub TextEdit2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit2.Leave
        LeggiCat(TextEdit2.EditValue, TextEdit3.EditValue)
    End Sub

    Private Sub TextEdit4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit4.Leave
        LeggiCat(TextEdit4.EditValue, TextEdit5.EditValue)
    End Sub

    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub TextEdit1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.TextChanged
        LeggiAzi(TextEdit1.EditValue)
        TextEdit7.EditValue = TextEdit1.EditValue
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If Not Val(TextEdit1.EditValue) > 0 Then
            MessageBox.Show("Inserire l'anno di Riferimento!", "Stampa Libro Cespiti", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextEdit1.Focus()
            Exit Sub
        End If
        If Not IsNumeric(TextEdit2.EditValue) Then TextEdit2.Focus() : Exit Sub
        If Not IsNumeric(TextEdit4.EditValue) Then TextEdit4.Focus() : Exit Sub
        If Not Val(TextEdit2.EditValue) > 0 Or Not Val(TextEdit4.EditValue) > 0 Then
            MessageBox.Show("Inserire la categoria!", "Stampa Libro Cespiti", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextEdit2.Focus()
            Exit Sub
        End If
        If CheckEdit3.Checked = True And TextEdit6.EditValue.ToString.Length = 0 Then
            MessageBox.Show("Inserire Intestazione", "Stampa Libro Cespiti", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextEdit6.Focus()
            Exit Sub
        End If
        If MessageBox.Show("Procedo con la Stampa?", "Libro Cespiti", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If
        Dim NOMESTAMPA As String = ""
        Periodo = DataMax.Year
        If DataMax.Year <> DataMin.Year Then Periodo = DataMin.Year & "-" & DataMax.Year
        Cursor.Current = Cursors.WaitCursor
        LibroCespPDF.StampaLibro(Val(TextEdit1.EditValue), Val(TextEdit2.EditValue), Val(TextEdit4.EditValue), CheckEdit1.Checked, CheckEdit2.Checked, CheckEdit3.Checked, TextEdit6.EditValue, Val(TextEdit7.EditValue), Val(TextEdit8.EditValue), AZI, NOMESTAMPA, DataMin, DataMax, Periodo)
        If NOMESTAMPA > "" Then StampaInPdf(NOMESTAMPA)
    End Sub
End Class