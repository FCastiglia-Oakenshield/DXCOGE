Imports DXBASE
Imports System.Data.SqlClient

Public Class StInquilini
    Dim DsRip As DataTable
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow
    Dim StrPrint As String
    Dim Em As New DevExpress.XtraEditors.Controls.CheckedListBoxItem
    Private Sub StInquilini_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Pulizia()
        CaricoLocazioni()
    End Sub
    Sub Pulizia()
        DsRip = New DataTable
        GridControl1.DataSource = DsRip
        DsRip = New DataTable
        GridControl1.DataSource = DsRip
    End Sub
    Sub CaricoLocazioni()
        Cursor.Current = Cursors.WaitCursor
        StrPrint = "select AffCliente,AffImm,AffTipo,AffDurata,AffIniLoc,AffRegAnn,AffScaProroga,AffFineContr,AffCauzione,AffCanone,AffImpReg,AffImpIst,AffImpInq,AffRid,AffNote,ImmIndirizzo,ImmCap,ImmCitta,ImmPv,ImmNrIdenCom,ImmPiano,ImmCateg,ImmMq,ImmMillesimi,ImmMc,ImmFgexC,ImmNrexC,ImmSubexC,ImmFgnC,ImmNrnC,ImmSubnC,ImmNote, AnaDesc from VLocazioni  order by AnaDesc,ImmCitta,ImmIndirizzo,ImmPiano,ImmCateg"
        DsRip = New DataTable
        DaRip = New SqlDataAdapter(StrPrint, cnDb)
        DaRip.SelectCommand.CommandTimeout = 300
        DaRip.Fill(DsRip)
        GridControl1.DataSource = DsRip
        AdvBandedGridView1.ClearSelection()
        AdvBandedGridView1.ExpandAllGroups()
    End Sub
    Private Sub StInquilini_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim Land As Boolean = True
        Dim ResZ As Integer = AdvBandedGridView1.Columns.Item("ImmIndirizzo").Width
        If DsRip.Rows.Count = 0 Then Exit Sub
        If AdvBandedGridView1.VisibleColumns.Count < 9 Then
            Land = False
            AdvBandedGridView1.Columns.Item("ImmIndirizzo").Resize(250)
        End If
        DXANTEPRIMA(GridControl1, Land, Printing.PaperKind.A4, GroupControl2.Text)
        AdvBandedGridView1.Columns.Item("ImmIndirizzo").Resize(ResZ)
    End Sub
End Class