Imports DXBASE
Imports System.Data.SqlClient

Public Class DatiIntPF
    Dim DsDai As DataTable
    Dim DaDai As SqlDataAdapter
    Dim BlDai As SqlCommandBuilder
    Dim RwDai As DataRow

    Private Sub DatiIntPF_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        GeneraIntegrativi()
    End Sub
    Private Sub GeneraIntegrativi()
        Dim genera As String = "INSERT INTO TbIntPF(PfClifor,PfAnadesc,PfCognome,PfNome,PfTipo,PFPiva,PFCFis) SELECT DISTINCT AnaCod,AnaDesc, '','',AnaGrp,CASE WHEN AnaPivaEst <> '' THEN AnaPivaEst ELSE AnaPiva END, AnaCfis FROM vdox.dbo.TbAna where  (Len(Anacfis) = 16) AND (anagrp = 'CL' OR ANAGRP = 'FO') AND AnaCod not in (SELECT PfClifor from TbIntPF )"

        Cmd = New SqlCommand(genera, cnCo)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub DatiIntPF_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        RadioGroup2.EditValue = "CL"
    End Sub

    Private Sub RadioGroup2_EditValueChanged(sender As Object, e As System.EventArgs) Handles RadioGroup2.EditValueChanged
        VisIntegrativi()
    End Sub
    Private Sub VisIntegrativi()
        GroupControl6.Text = "DATI INTEGRATIVI PERSONE FISICHE " & RadioGroup2.Properties.Items(RadioGroup2.SelectedIndex).Description


        'Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        'Dim P2 As New SqlParameter("@PERIODO", SqlDbType.TinyInt)
        Dim P3 As New SqlParameter("@TIPO", SqlDbType.VarChar)

        'P1.Value = ANNO
        'P2.Value = RadioGroup1.EditValue
        P3.Value = RadioGroup2.EditValue

        DaDai = New SqlDataAdapter("SELECT * FROM TbIntPF WHERE PFTIPO=@TIPO", cnCo)
        BlDai = New SqlCommandBuilder(DaDai)
        'DaDai.SelectCommand.Parameters.Add(P1)
        'DaDai.SelectCommand.Parameters.Add(P2)
        DaDai.SelectCommand.Parameters.Add(P3)
        DaDai.SelectCommand.CommandTimeout = 300

        DsDai = New DataTable("DAI")
        DaDai.Fill(DsDai)

        GridControl1.DataSource = DsDai
        AdvBandedGridView1.ClearSelection()
    End Sub
    Private Sub RepositoryItemTextedit4_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit4.EditValueChanged
        AdvBandedGridView1.SetFocusedRowCellValue("PfCognome", DirectCast(sender, DevExpress.XtraEditors.TextEdit).EditValue)
    End Sub
    Private Sub RepositoryItemTextedit5_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit5.EditValueChanged
        AdvBandedGridView1.SetFocusedRowCellValue("PfNome", DirectCast(sender, DevExpress.XtraEditors.TextEdit).EditValue)
    End Sub
    Private Sub GridView3_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles AdvBandedGridView1.CellValueChanged

        If AdvBandedGridView1.UpdateCurrentRow() Then
            DaDai.Update(DsDai)
        End If
    End Sub
End Class