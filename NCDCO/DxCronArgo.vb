Imports DXBASE
Imports System.Data.SqlClient
Public Class DxCronArgo
    Private Shared ERif As Integer

    Public Shared Property NRif() As Integer
        Get
            Return ERif
        End Get
        Set(ByVal Value As Integer)
            ERif = Value
        End Set
    End Property

    Dim TbCMG As DataTable
    Dim DaCMG As SqlDataAdapter
    Dim RwCMG As DataRow

    Dim TbGan As DataTable
    Dim DaGan As SqlDataAdapter
    Dim RwGan As DataRow

    Dim r As New ArrayList
    Dim c As New ArrayList

    Private Sub DxCronArgo_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Leggi()
    End Sub
    Sub Leggi()
        Dim SStr As String = "Select * from TbNDcg Where FDcgGancio=" & ERif & " Or FdcgNumRif = " & ERif & " Order By FdcgGancio"
        Dim str As String = "select * from TMPCRONCM inner join tbndcg on Tesfatrif = fdcgnumrif WHERE IDBLOCK = 1 and (Totale <> 0 OR ( Origine<>0 AND Tipo = 'H') ) order by TcmSigla,Repertorio,TesData,TesNum"
        Dim RIPARTI As String = "Exec XCRONRIP @RIF=" & ERif & ",@ID=1,@F=0"
        Dim RIPARTIplus As String = ""
        TbGan = New DataTable()
        DaGan = New SqlDataAdapter(SStr, cnDb)
        DaGan.Fill(TbGan)
        If TbGan.Rows.Count > 1 Then
            For ZZ As Int16 = 1 To TbGan.Rows.Count
                RwGan = TbGan.Rows(ZZ - 1)
                RIPARTIplus = "exec XCRONRIP @RIF=" & RwGan("FDcgNumRif") & ",@ID=1,@F=" & (ZZ - 1)
                EsegueSql(RIPARTIplus, cnDb)
            Next
        Else
            EsegueSql(RIPARTI, cnDb)
        End If
        TbCMG = New DataTable()
        DaCMG = New SqlDataAdapter(Str, cnDb)
        DaCMG.Fill(TbCMG)
        GridControl1.DataSource = TbCMG
        If TbCMG.Rows.Count > 0 Then
            RwCMG = TbCMG.Rows(0)
            GroupControl1.Text = RwCMG("AnaDesc") & " Fattura " & RwCMG("FdcgNumero") & " del " & RwCMG("FdcgData")
            GridView1.ClearSelection()
            GridView1.SelectRow(-1)
        End If
    End Sub
    Private Sub GridView1_CustomSummaryCalculate(sender As Object, e As DevExpress.Data.CustomSummaryEventArgs) Handles GridView1.CustomSummaryCalculate
        Dim P As Integer = 0
        If e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Start Then e.TotalValue = 0 : Exit Sub
        If TbCMG.Rows.Count < 1 Then Exit Sub
        Rw = TbCMG.Rows(e.RowHandle)
        For i As Int16 = 1 To r.Count
            If Rw("FdcgNumrif") = r(i - 1) And DirectCast(e.Item, DevExpress.XtraGrid.GridColumnSummaryItem).FieldName = c(i - 1) Then
                P = 0
                GoTo II
            End If
        Next
        r.Add(Rw("FdcgNumrif")) : c.Add(DirectCast(e.Item, DevExpress.XtraGrid.GridColumnSummaryItem).FieldName)
        P = 1
II:
        If e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Calculate Then

            e.TotalValue += Rw(DirectCast(e.Item, DevExpress.XtraGrid.GridColumnSummaryItem).FieldName) * P
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton1.Click
        DXANTEPRIMA(GridControl1, True, Printing.PaperKind.A4, GroupControl1.Text)
    End Sub
End Class