Imports DXBASE
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports System.Drawing

Public Class LDPPop
    Private Shared TbUFa As DataTable
    Private Shared Ret As Boolean
    Public Shared Property PTbUFa() As DataTable
        Get
            Return TbUFa
        End Get
        Set(ByVal Value As DataTable)
            TbUFa = Value
        End Set
    End Property
    Public Shared Property PRet() As Boolean
        Get
            Return Ret
        End Get
        Set(ByVal Value As Boolean)
            Ret = Value
        End Set
    End Property

    Dim DSCPT As DataTable
    Dim DACPT As SqlDataAdapter
    Dim RWCPT As DataRow

    Dim RwEdit As DataRow
    Dim TOTALEIMP As Decimal

    Private Sub LDPPop_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        CaricaVIDEO()
    End Sub

    Sub CaricaVIDEO()
        Rw = TbUFa.Rows(0)
        Dim Id As Integer = Rw("PrkId")
        Dim Exc As String = "EXEC XLDPRIP @ID=" & Id
        If Ret = True Then Exc = "EXEC RLDPRIP @ID=" & Id
        EsegueSql(Exc, CnDc)
        Dim Str As String = "SELECT *,TOT=(SELECT SUM(IMPORTO) FROM ##TABTO) FROM ##TABCM ORDER BY PRKCONTO"
        DSCPT = New DataTable()
        DACPT = New SqlDataAdapter(Str, CnDc)
        DACPT.Fill(DSCPT)
        Rw = DSCPT.Rows(0)
        TOTALEIMP = Rw("TOT")
        GridControl4.DataSource = DSCPT
        Me.Width = 400
        Me.Height = 400
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = New Point(3, 72)
    End Sub

    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ButtonEdit1.ButtonClick
        Me.Close()
    End Sub

    Private Sub GridView5_CustomSummaryCalculate(sender As Object, e As DevExpress.Data.CustomSummaryEventArgs) Handles GridView5.CustomSummaryCalculate
        e.TotalValue = TOTALEIMP
    End Sub

End Class