Imports System.Data.SqlClient
Imports DevExpress.XtraGrid
Public Class DxRicCespiti
    Private Shared Privcodice, Privcodice2, Privcodice3 As String
    Private Shared DT As DataTable

    Public Shared ReadOnly Property codice()
        Get
            Return Privcodice
        End Get
    End Property

    Public Shared ReadOnly Property codice2()
        Get
            Return Privcodice2
        End Get
    End Property

    Public Shared ReadOnly Property codice3()
        Get
            Return Privcodice3
        End Get
    End Property

    Public Shared WriteOnly Property DataTable() As DataTable
        Set(ByVal Value As DataTable)
            DT = Value
        End Set
    End Property

    Dim Rwx As DataRowView
    Dim Iset As Integer = -1
    Dim TempCodice, TempCodice2, TempCodice3 As String

    Private Sub DxRicCespiti_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        TempCodice = ""
        GridControl1.DataSource = DT
        If DT.Rows.Count > 0 Then
            GridView1.SelectRow(0)
            Rwx = GridView1.GetRow(GridView1.GetSelectedRows(0))
            TempCodice = Rwx("CspGru")
            TempCodice2 = Rwx("CspSpe1")
            TempCodice3 = Rwx("CspSpe2")
        End If
    End Sub
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset = hi.RowHandle
    End Sub

    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If Iset > -1 Then
            Rwx = GridView1.GetRow(Iset)
            Privcodice = Rwx("CspGru")
            Privcodice2 = Rwx("CspSpe1")
            Privcodice3 = Rwx("CspSpe2")
            Me.Close()
        End If
    End Sub

    Private Sub GridView1_SelectionChanged(ByVal sender As Object, ByVal e As DevExpress.Data.SelectionChangedEventArgs) Handles GridView1.SelectionChanged
        Dim OPZIONI As Array = GridView1.GetSelectedRows
        If OPZIONI.Length > 0 Then
            Rwx = GridView1.GetRow(OPZIONI(0))
            TempCodice = Rwx("CspGru")
            TempCodice2 = Rwx("CspSpe1")
            TempCodice3 = Rwx("CspSpe2")
        End If
    End Sub

    Private Sub DxRicCespiti_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If TempCodice.Length > 0 Then
            If e.KeyCode = Keys.Enter Then
                ASSEGNAedESCI()
            End If
        End If
    End Sub
    Sub ASSEGNAedESCI()
        Privcodice = TempCodice
        Privcodice2 = TempCodice2
        Privcodice3 = TempCodice3
        Me.Close()
    End Sub
End Class