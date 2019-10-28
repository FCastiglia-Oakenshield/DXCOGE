Imports DXBASE
Imports System.Data.SqlClient


Public Class DxAgeing
    Dim DsRie As DataTable
    Dim DaRie As SqlDataAdapter
    Dim RwRie As DataRow

    Dim MiglioFo As Int32
    Dim StrPrint, TiRiep, Analitico As String
    Dim Ordini As Integer = 0

    Private Sub DxAgeing_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        XtraTabControl1.SelectedTabPageIndex = 0
        PrimoMiglio()
        PuliziaRIEP()
    End Sub
    Sub PrimoMiglio()
        Dim Cmd As New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        DateEdit3.EditValue = CDate(Today)
        REM COMMESSE
        Dim NN As DevExpress.XtraEditors.Controls.ImageComboBoxItem
        RepositoryItemImageComboBox1.Items.Clear()
        RepositoryItemImageComboBox2.Items.Clear()
        NN = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("*NESSUNA*", "0", 23)
        RepositoryItemImageComboBox1.Items.Add(NN)
        RepositoryItemImageComboBox2.Items.Add(NN)
        Cmd = New SqlCommand("SELECT  * FROM TbTcm order by TcmAnno desc,TcmNum desc", cnDb)
        dataRd = Cmd.ExecuteReader
        Dim ii As Int16 = 29
        While dataRd.Read
            If dataRd.Item("TcmGaranzia") = True Then
                ii = 46
            ElseIf dataRd.Item("TcmRefit") = True Then
                ii = 47
            Else
                ii = 29
            End If
            If dataRd.Item("TcmChiusa") = True Then ii = 103
            NN = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("TcmSigla"), dataRd.Item("TcmRif").ToString, ii)
            RepositoryItemImageComboBox1.Items.Add(NN) : RepositoryItemImageComboBox2.Items.Add(NN)
        End While
        dataRd.Close()
    End Sub
#Region "RIEPILOGO SCADENZE"
    Sub PuliziaRIEP()
        DsRie = New DataTable
        GridControl2.DataSource = DsRie
        GridControl3.DataSource = DsRie
    End Sub
    Private Sub ButtonXF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF1.Click
        If ControlloRIEP() = False Then Exit Sub
        PuliziaRIEP()
        GroupControl8.Text = TiRiep & " PARTITE SCOPERTE Al " & Today & " Ore " & TimeOfDay & " Raggruppate al " & Format(DateEdit3.EditValue, "dd/MM/yyyy")
        Cursor.Current = Cursors.WaitCursor
        Dim Qo As Integer = Ordini
        If TiRiep <> "FO" Then Qo = 0
        Dim STR As String = "EXEC DXRIESCACOM @AL='" & Format(DateEdit3.EditValue, "dd/MM/yyyy") & "',@CLFO = '" & TiRiep & "',@TIPO = '" & Analitico & "',@OR = " & Qo
        DsRie = New DataTable
        DaRie = New SqlDataAdapter(STR, CnDc)
        DaRie.SelectCommand.CommandTimeout = 300
        DaRie.Fill(DsRie)
        If Analitico = "A" Then
            GridControl2.BringToFront()
            GridControl2.DataSource = DsRie
            AdvBandedGridView1.ClearSelection()
            AdvBandedGridView1.ExpandAllGroups()
        Else
            GridControl3.BringToFront()
            GridControl3.DataSource = DsRie
            AdvBandedGridView3.ClearSelection()
            AdvBandedGridView3.ExpandAllGroups()
        End If
    End Sub
    Function ControlloRIEP() As Boolean
        Dim Msg As Boolean = True
        If RadioGroup2.SelectedIndex = -1 Then
            RadioGroup2.ErrorText = "Selezionare Clienti o Fornitori"
            Msg = False
        End If
        If RadioGroup3.SelectedIndex = -1 Then
            RadioGroup3.ErrorText = "Selezionare Analitico o Sintetico"
            Msg = False
        End If
        If RadioGroup1.SelectedIndex = -1 Then
            RadioGroup2.ErrorText = "Selezionare Ordini"
            Msg = False
        End If
        If DateEdit3.EditValue Is Nothing Then
            DateEdit3.ErrorText = "Manca Data Riferimento"
            Msg = False
        End If
        Return Msg
    End Function
    Private Sub RadioGroup2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup2.SelectedIndexChanged
        If RadioGroup2.SelectedIndex > -1 Then
            TiRiep = RadioGroup2.Properties.Items(RadioGroup2.SelectedIndex).Value
            ButtonXF1.PerformClick()
        End If
    End Sub
    Private Sub RadioGroup3_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles RadioGroup3.SelectedIndexChanged
        If RadioGroup3.SelectedIndex > -1 Then
            Analitico = RadioGroup3.Properties.Items(RadioGroup3.SelectedIndex).Value
            ButtonXF1.PerformClick()
        End If
    End Sub
    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex > -1 Then
            Ordini = RadioGroup1.Properties.Items(RadioGroup1.SelectedIndex).Value
            ButtonXF1.PerformClick()
        End If
    End Sub
    Private Sub ButtonXF5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonXF5.Click
        PuliziaRIEP()
        DateEdit3.Focus()
    End Sub
    Private Sub ButtonXF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF9.Click
        If Analitico = "A" Then
            DXANTEPRIMA(GridControl2, True, Printing.PaperKind.A4, GroupControl8.Text)
        Else
            DXANTEPRIMA(GridControl3, True, Printing.PaperKind.A4, GroupControl8.Text)
        End If
    End Sub
#End Region


End Class