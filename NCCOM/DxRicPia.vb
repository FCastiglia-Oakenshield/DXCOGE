Imports System.Collections
Imports System.Data.SqlClient
Imports DXBASE
Public Class DxRicPia
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
        GridControl1.DataSource = DT
        For i As Int16 = 1 To Field.Count
            GridView1.Columns(i - 1).FieldName = Field(i - 1)
        Next
        Privcodice = "" : TempCodice = ""
        If DT.Rows.Count > 0 And T = 0 Then
            GridView1.SelectRow(0)
            Rwx = GridView1.GetRow(GridView1.GetSelectedRows(0))
            TempCodice = Rwx.Item(0)
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
            Privcodice = Rwx.Item(0)
            Me.Close()
        End If
    End Sub

    Private Sub GridView1_SelectionChanged(ByVal sender As Object, ByVal e As DevExpress.Data.SelectionChangedEventArgs) Handles GridView1.SelectionChanged
        Dim OPZIONI As Array = GridView1.GetSelectedRows
        If OPZIONI.Length > 0 Then
            Rwx = GridView1.GetRow(OPZIONI(0))
            TempCodice = Rwx.Item(0)
        End If
    End Sub

    Private Sub DxRicPia_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If TempCodice.Length > 4 Then
            If e.KeyCode = Keys.Enter Then
                ASSEGNAedESCI()
            End If
        End If
    End Sub
    Sub ASSEGNAedESCI()
        Privcodice = TempCodice
        Me.Close()
    End Sub

    Private Sub TextEdit1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.TextChanged, TextEdit2.TextChanged
        If T = 1 Then PopolaPianoConti()
    End Sub
    Private Sub PopolaPianoConti()
        Dim leggi As String = "select * from TbPia where PiaCodCo like @Num and PiaAnaCo like @Ana"
        Dim TbPia As DataTable
        Dim DaPia As SqlDataAdapter

        Dim p1 As New SqlParameter("@Num", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@Ana", SqlDbType.VarChar)

        p1.Value = TextEdit1.EditValue & "%"
        p2.Value = TextEdit2.EditValue & "%"

        Dim cmd As New SqlCommand(leggi, cnCo)
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)

        TbPia = New DataTable
        DaPia = New SqlDataAdapter(cmd)
        DaPia.Fill(TbPia)
        GridControl1.DataSource = TbPia
        GridControl1.Refresh()
    End Sub
End Class