Imports DXBASE
Imports System.Data.SqlClient
Public Class TbPaesi

    Private Sub TbPaesi_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        CaricaTabella()
    End Sub
    Private Sub CaricaTabella()
        Dim TbP As DataTable
        Dim DaP As SqlDataAdapter

        DaP = New SqlDataAdapter("SELECT * FROM TbPaesi ORDER BY PaDesc", cnCo)
        TbP = New DataTable("PA")
        DaP.Fill(TbP)

        GridControl1.DataSource = TbP
        GridView1.ClearSelection()
        GridView1.OptionsSelection.EnableAppearanceFocusedRow = False
    End Sub

    Private Sub ButtonF9B_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF9B.Click
        DXANTEPRIMA(GridControl1, False, Printing.PaperKind.A4, "TABELLA PAESI (ISO 3166)")
    End Sub
End Class