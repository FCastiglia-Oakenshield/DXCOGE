Imports System.Collections
Imports System.Data.SqlClient
Imports DXBASE
Public Class DxRicercheFrm
    Public ReadOnly Property codice()
        Get
            Return Privcodice
        End Get
    End Property

    Public WriteOnly Property Dta() As DataTable
        Set(ByVal Value As DataTable)
            DT = Value
        End Set
    End Property

    Public WriteOnly Property Ar() As ArrayList
        Set(ByVal Value As ArrayList)
            Field = Value
        End Set
    End Property

    Public WriteOnly Property TT() As Integer
        Set(ByVal Value As Integer)
            T = Value
        End Set
    End Property

    Dim Privcodice As String
    Dim DT As DataTable
    Dim Field As ArrayList
    Dim T As Integer

    Dim Rwx As DataRowView
    Dim Iset As Integer = -1
    Dim TempCodice As String

    Private Sub DxRicPia_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        For i As Int16 = 1 To GridView1.Columns.Count
            GridView1.Columns(i - 1).Visible = False
            GridView1.Columns(i - 1).VisibleIndex = -1
        Next
        Privcodice = "" : TempCodice = ""
        For i As Int16 = 1 To Field.Count
            GridView1.Columns(i - 1).Visible = True
            GridView1.Columns(i - 1).FieldName = Field(i - 1)
            GridView1.Columns(i - 1).VisibleIndex = i - 1
        Next
        GridControl1.DataSource = DT
        If DT.Rows.Count > 0 Then
            GridView1.SelectRow(0)
            Rwx = GridView1.GetRow(GridView1.GetSelectedRows(0))
            TempCodice = Rwx.Item(Field(0))
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
            Privcodice = Rwx.Item(Field(0))
            Me.Close()
        End If
    End Sub

    Private Sub GridView1_SelectionChanged(ByVal sender As Object, ByVal e As DevExpress.Data.SelectionChangedEventArgs) Handles GridView1.SelectionChanged
        Dim OPZIONI As Array = GridView1.GetSelectedRows
        If OPZIONI.Length > 0 Then
            Rwx = GridView1.GetRow(OPZIONI(0))
            TempCodice = Rwx.Item(Field(0))
        End If
    End Sub

    Private Sub DxRicPia_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If TempCodice.Length > 0 Then
            If e.KeyCode = Keys.Enter Then
                ASSEGNAedESCI()
            End If
        End If
    End Sub
    Sub ASSEGNAedESCI()
        Privcodice = TempCodice
        Me.Close()
    End Sub
End Class