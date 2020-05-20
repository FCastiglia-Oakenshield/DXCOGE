Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Public Class DxStaPiv
    Dim Mail As String = ""
    Dim Ti As String = "PiCf"
    Dim DsPc As DataSet
    Dim DaPc As SqlDataAdapter
    Dim RwPc As DataRow
    Dim CbPc As SqlCommandBuilder
    Dim Paesi As Collection
    Dim Rpt As ReportClass
    Dim Rpt1 As New StPiCfPe
    Private Sub DxStaPiv_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        CaricaPaesi()
        TextEdit1.EditValue = Year(Today)
        TextEdit1.Focus()
    End Sub
    Private Sub CaricaPaesi()
        Paesi = New Collection

        Cmd = New SqlCommand("Select distinct PaSigla from TbPaesi", cnCo)
        dataRd = Cmd.ExecuteReader

        While dataRd.Read
            Paesi.Add(dataRd.Item("PaSigla"), dataRd.Item("PaSigla"))
        End While
        dataRd.Close()
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If ControllaCampi() = False Then
            TextEdit1.EditValue = ""
            TextEdit1.Focus()
            Exit Sub
        End If
        Cursor.Current = Cursors.WaitCursor
        Dim frm As New LpDs
        Rpt = New ReportClass
        Rpt1 = New StPiCfPe
        Rpt = Rpt1
        Cursor.Current = Cursors.WaitCursor
        leggiclifor()
        Rpt.SetDataSource(DsPc.Tables(Ti))
        Rpt.SetParameterValue("Anno", TextEdit1.EditValue)
        Rpt.SetParameterValue("Marchio", Marchio)

        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
    Private Sub leggiclifor()
        Dim StrUno As String
        Dim y As Int16
        Mail = ""
        StrUno = "Select * from FnControllo(" & Val(TextEdit1.EditValue) & ")"
        DsPc = New DataSet
        DaPc = New SqlDataAdapter(StrUno, cnCo)
        DaPc.Fill(DsPc, Ti)
        If DsPc.Tables(Ti).Rows.Count = 0 Then Exit Sub
        For y = 1 To DsPc.Tables(Ti).Rows.Count
            RwPc = DsPc.Tables(Ti).Rows(y - 1)
            If Codfisc(RwPc("AnaPiva")) = False Or RwPc("AnaPiva") = "" Then RwPc("ErrP") = "*"
            If Codfisc(RwPc("AnaCfis")) = False Or RwPc("AnaCfis") = "" Then RwPc("ErrC") = "*"
            If (RwPc("AnaPivaEst").ToString.Trim <> "" And RwPc("AnaPivaEst").ToString.Trim <> "OO99999999999" And Paesi.Contains(Mid(RwPc("AnaPivaEst").ToString.Trim, 1, 2)) = False) Or (Len(RwPc("AnaPivaEst").ToString.Trim) < 5 And RwPc("AnaPivaEst").ToString.Trim <> "") Then
                RwPc("ErrE") = "*"
            End If
        Next
        DsPc.AcceptChanges()
    End Sub
    Private Function ControllaCampi() As Boolean
        Dim Mail As String = ""
        If Val(TextEdit1.EditValue) < 2005 Then Mail = "<> ANNO NON VALIDO PER CONTROLLO " & Chr(13)
        If Mail > "" Then
            MoltoCritico(Mail, "CONTROLLO PARTITA IVA/CODICE FISCALE")
            Return False
            Exit Function
        End If
        Return True
    End Function
    Sub MoltoCritico(ByVal Mail As String, ByVal contesto As String)
        MessageBox.Show(Mail, contesto, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
End Class