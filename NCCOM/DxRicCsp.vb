Imports System.Data.SqlClient
Imports DXBASE
Public Class DxRicCsp
    Public ReadOnly Property codice()
        Get
            Return Privcodice
        End Get
    End Property

    Public ReadOnly Property codice2()
        Get
            Return Privcodice2
        End Get
    End Property

    Public ReadOnly Property codice3()
        Get
            Return Privcodice3
        End Get
    End Property
    Public WriteOnly Property Dta() As DataTable
        Set(ByVal Value As DataTable)
            DT = Value
        End Set
    End Property

    Public WriteOnly Property XCspGru() As String
        Set(ByVal Value As String)
            CspGru = Value
        End Set
    End Property

    Public WriteOnly Property XCspSpe1() As String
        Set(ByVal Value As String)
            CspSpe1 = Value
        End Set
    End Property

    Public WriteOnly Property XCspSpe2() As String
        Set(ByVal Value As String)
            CspSpe2 = Value
        End Set
    End Property
    Dim Privcodice, Privcodice2, Privcodice3 As String
    Dim DT As DataTable
    Dim CspGru As String
    Dim CspSpe1 As String
    Dim CspSpe2 As String
    Dim Rwx As DataRowView
    Dim DsCesp As DataTable
    Dim DaCesp As SqlDataAdapter

    Private Sub DxRicCsp_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        GridControl1.DataSource = DT
        Privcodice = "" : Privcodice2 = "" : Privcodice3 = ""
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
            Rwx = GridView1.GetRow(iset)
            Privcodice = Rwx.Item("CespNum")
            Privcodice2 = Rwx.Item("CespAnnoA")
            Privcodice3 = Rwx.Item("CespCat")
            Me.Close()
            Exit Sub
        End If
    End Sub

    Private Sub TextEdit1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.TextChanged, TextEdit2.TextChanged, TextEdit3.TextChanged, TextEdit4.TextChanged, TextEdit5.TextChanged
        PopolaGrid(TextEdit1.Text, TextEdit2.Text, TextEdit3.Text, TextEdit4.Text, TextEdit5.Text)
        sender.focus()
        SendKeys.Send("{END}")
    End Sub

    Private Function PopolaGrid(ByVal Num As String, ByVal Anno As String, ByVal Cat As String, ByVal DescCat As String, ByVal DescCesp As String) As Int32
        Dim leggi As String = "select * from vcespcat where CespNum like @Num and CespAnnoA like @Anno and CespCat like @Cat and CespDescr like @DescCesp and CspDesc like @DescCat and cspgru='" & CspGru & "' and cspspe1='" & CspSpe1 & "' and cspspe2='" & CspSpe2 & "'"

        Dim p1 As New SqlParameter("@Num", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@Anno", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@Cat", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@DescCesp", SqlDbType.VarChar)
        Dim p6 As New SqlParameter("@DescCat", SqlDbType.VarChar)


        p1.Value = Num & "%"
        p2.Value = Anno & "%"
        p3.Value = Cat & "%"
        p4.Value = DescCesp & "%"
        p6.Value = DescCat & "%"

        Dim cmd As New SqlCommand(leggi, cnCo)
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmd.Parameters.Add(p3)
        cmd.Parameters.Add(p4)
        cmd.Parameters.Add(p6)

        DsCesp = New DataTable
        DaCesp = New SqlDataAdapter(cmd)
        DaCesp.Fill(DsCesp)

        GridControl1.DataSource = DsCesp
        GridControl1.Refresh()
        GridView1.ClearSelection()
    End Function

End Class