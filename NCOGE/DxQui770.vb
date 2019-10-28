Imports DXBASE
Imports System.Data.SqlClient
Imports DevExpress.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports NPRINT
Imports NCCOM
Public Class DxQui770
    Dim Ds As DataSet
    Dim DsTri As DataTable
    Dim DaTri As SqlDataAdapter
    Dim RwTri As DataRow


    Dim DsFat As DataTable
    Dim DaFat As SqlDataAdapter
    Dim RwFat As DataRowView

    Dim UEX As Integer = -1
    Dim TotComp As Decimal
    Dim TotRite As Decimal
    Dim TotBool As Integer
    Dim Rpt As ReportClass
    Dim Rpt1 As New StE14
    Dim Anno, Mese As Int16

    Private Sub DxQui770_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5x.Click
        Pulizia()
        PopolaGrid()
        DateEdit5x.Focus()
    End Sub
    Sub Pulizia()
        TextEdit7x.EditValue = CDec(0.0)
        TextEdit8x.EditValue = CDec(0.0)
        TextEdit17x.EditValue = CDec(0.0)
        TextEdit19x.EditValue = CDec(0.0)
        TextEdit18x.EditValue = ""
        DateEdit5x.EditValue = Nothing
        ImageComboBoxEdit1x.SelectedIndex = 0
        TotComp = 0 : TotRite = 0 : TotBool = 0
    End Sub
    Sub PopolaGrid()
        Ds = New DataSet
        Dim Str As String = "Select Distinct RitCompenso=sum(ritcompenso),RitRitenuta=sum(ritritenuta),Sospeso=sum(RitSospesa)," _
& "Versam=sum(ritImpVersam),Competenza = cast('01/'+cast(DATEPART(MONTH,RitDataPag) as varchar(2))+'/'+cast(datepart(year,RitDataPag) as varchar(4)) as smalldatetime) from vrittrib " _
  & " WHERE(RitDataPag Is Not NULL) group by cast('01/'+cast(DATEPART(MONTH,RitDataPag) as varchar(2))+'/'+cast(datepart(year,RitDataPag) as varchar(4)) as smalldatetime)" '' having sum(ritritenuta)>sum(ritImpVersam)"
        DsTri = New DataTable("Trib")
        DaTri = New SqlDataAdapter(Str, cnCo)
        DaTri.Fill(Ds, "Trib")

        Str = "select bool = case when ritimpversam > 0 then cast(0 as bit)  else cast(1 as bit) end,*,Competenza = cast('01/'+cast(DATEPART(MONTH,RitDataPag) as varchar(2))+'/'+cast(datepart(year,RitDataPag) as varchar(4)) as smalldatetime) from vrittrib where (RitDataPag Is Not NULL) " '' and (ritdataver is null)" ''  or not ritimpversam>0 or abs(ritsospesa) >0)"
        DsFat = New DataTable("Fatt")
        DaFat = New SqlDataAdapter(Str, cnCo)
        DaFat.Fill(Ds, "Fatt")

        Ds.Relations.Clear()
        Ds.Relations.Add("Dettaglio", Ds.Tables("Trib").Columns("Competenza"), Ds.Tables("Fatt").Columns("Competenza"))
        GridControl12.DataSource = Ds
        GridControl12.DataMember = "Trib"
        GridView12.ClearSelection()
        GridView12.SetMasterRowExpanded(0, True)
    End Sub
    Private Sub GridView1_MasterRowExpanded(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.CustomMasterRowEventArgs) Handles GridView12.MasterRowExpanded
        If UEX <> e.RowHandle Then
            GridView12.SetMasterRowExpanded(UEX, False)
            GridView12.ClearSelection()
        End If
        UEX = e.RowHandle
        GridView12.SelectRow(UEX)
        RwTri = GridView12.GetDataRow(UEX)
        DateEdit5x.Focus()
    End Sub

    Private Sub GridView2_CustomSummaryCalculate(ByVal sender As Object, ByVal e As DevExpress.Data.CustomSummaryEventArgs) Handles GridView13.CustomSummaryCalculate
        Dim summaryID As Integer = Convert.ToInt32(CType(e.Item, DevExpress.XtraGrid.GridSummaryItem).Tag)
        Dim View As DevExpress.XtraGrid.Views.Grid.GridView = CType(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        If e.SummaryProcess = CustomSummaryProcess.Start Then
            TotComp = 0
            TotRite = 0
            TotBool = 0
        End If
        If e.SummaryProcess = CustomSummaryProcess.Calculate Then
            Dim check As Boolean = CBool(View.GetRowCellValue(e.RowHandle, "bool"))
            If check = False Then GoTo III
            Select Case summaryID
                Case 1
                    TotBool += 1
                Case 2
                    TotComp += Convert.ToDecimal(e.FieldValue)
                Case 3
                    TotRite += Convert.ToDecimal(e.FieldValue)
            End Select
        End If
III:
        If e.SummaryProcess = CustomSummaryProcess.Finalize Then
            Select Case summaryID
                Case 1
                    e.TotalValue = TotBool
                    If TotBool <> 1 Then TextEdit17x.Properties.ReadOnly = True Else TextEdit17x.Properties.ReadOnly = False
                Case 2
                    e.TotalValue = TotComp
                    TextEdit7x.EditValue = CDec(TotComp)
                Case 3
                    e.TotalValue = TotRite
                    TextEdit8x.EditValue = CDec(TotRite)
                    TextEdit19x.EditValue = CDec(TotRite)
            End Select
        End If
    End Sub
    Private Sub RepositoryItemCheckEdit3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemCheckEdit1x.CheckedChanged
        DateEdit5x.Focus()
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11x.Click
        If Controlla() = False Then Exit Sub
        Aggiorna()
        ButtonF5x.PerformClick()
    End Sub
    Sub Aggiorna()
        For i As Int16 = 1 To GridView12.GetDetailView(UEX, 0).RowCount
            RwFat = GridView12.GetDetailView(UEX, 0).GetRow(i - 1)
            If RwFat("bool") = True Then Registra()
        Next
    End Sub
    Sub Registra()
        Dim Str As String = "Update TbRit set RitDataVer=@RitDataVer,RitEsCc=@RitEsCc,RitImpVersam=@RitImpVersam,RitNumQB=@RitNumQB,RitRitenuta=@RitRitenuta,RitSospesa=@RitSospesa where ritNum=" & RwFat("RitNum")

        Dim cmd As New SqlCommand(Str, cnCo)

        Dim p1 As New SqlParameter("@RitRitenuta", SqlDbType.Decimal)
        Dim p2 As New SqlParameter("@RitDataVer", SqlDbType.SmallDateTime)
        Dim p3 As New SqlParameter("@RitEsCc", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@RitNumQB", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@RitImpVersam", SqlDbType.Decimal)
        Dim p6 As New SqlParameter("@RitSospesa", SqlDbType.Decimal)

        p1.Value = RwFat("RitRitenuta")
        p2.Value = DateEdit5x.EditValue

        p3.Value = ImageComboBoxEdit1x.EditValue
        p4.Value = TextEdit18x.EditValue
        p5.Value = RwFat("RitRitenuta")
        p6.Value = 0

        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmd.Parameters.Add(p3)
        cmd.Parameters.Add(p4)
        cmd.Parameters.Add(p5)
        cmd.Parameters.Add(p6)

        cmd.ExecuteNonQuery()
    End Sub
    Function Controlla() As Boolean
        Dim Email As String = ""

        If CDec(TextEdit19x.EditValue) <= 0 Then
            Email &= "<> MANCA Importo Versato " & Chr(13)
            TextEdit19x.Focus()
        End If
        If ImageComboBoxEdit1x.SelectedIndex < 1 Then
            Email &= "<> MANCA Versamento a Mezzo " & Chr(13)
            ImageComboBoxEdit1x.Focus()
        End If
        If DateEdit5x.EditValue Is Nothing Then
            Email &= "<> MANCA Data Versamento " & Chr(13)
            DateEdit5x.Focus()
        End If
        If (TextEdit8x.EditValue - (TextEdit17x.EditValue + TextEdit19x.EditValue)) <> 0 Then
            Email &= "<> IMPORTO Versamento Errato!!! " & Chr(13)
        End If

        If Email.Length > 0 Then
            MessageBox.Show("Impossibile effettuare la Registrazione:" & Chr(13) & Email, "Errore Registrazione", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False : Exit Function
        End If
        Return True
    End Function

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9x.Click
        Cursor.Current = Cursors.WaitCursor
        Dim frm As New LpLp
        Rpt = New ReportClass
        Rpt1 = New StE14
        Rpt = Rpt1
        Rpt.SetParameterValue("Mese", CDate(RwTri("Competenza")).Month)
        Rpt.SetParameterValue("Anno", CDate(RwTri("Competenza")).Year)
        Rpt.SetParameterValue("Marchio", Marchio)
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
End Class