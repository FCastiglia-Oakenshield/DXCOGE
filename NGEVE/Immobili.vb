Imports DXBASE
Imports System.IO
Imports System.Data.SqlClient
Imports NCCOM
Imports System.Drawing
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native

Public Class Immobili
    Dim PrivId As Int32
    Dim IrowI As Integer
    Public ReadOnly Property Codice() As String
        Get
            Return PrivId
        End Get
    End Property
    Dim TbRIC As DataTable
    Dim DaRIC As SqlDataAdapter
    Dim RwD As DataRowView

    Private Sub Immobili_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown ', BUTTONXF5.Click
        PULIZIA()
        LeggiImmobili()
    End Sub
    Sub PULIZIA()
        GroupControl20.Enabled = True
    End Sub
    Sub LeggiImmobili()
        Dim Str As String = "Select ImmCod,ImmCitta,ImmIndirizzo,ImmSubnC,ImmCateg,ImmPiano,ImmNrIdenCom FROM VArcImm order by ImmCitta,ImmIndirizzo,ImmSubnC,ImmCateg,ImmNrIdenCom"
        TbRIC = New DataTable()
        DaRIC = New SqlDataAdapter(Str, cnDb)
        DaRIC.Fill(TbRIC)
        GridControl3.DataSource = TbRIC
        GridView4.ClearSelection()
        GridView4.UnselectRow(0)
    End Sub
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Me.Close()
    End Sub
    Private Sub GridControl3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl3.MouseMove
        ShowHitInfo3(GridView4.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo3(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl3.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        IrowI = hi.RowHandle
    End Sub
    Private Sub GridView4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.Click
        If IrowI > -1 Then
            RwD = GridView4.GetRow(IrowI)
            PrivId = RwD("ImmCod")
            Me.Close()
        Else
            PrivId = 0
        End If
    End Sub
End Class