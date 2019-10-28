Imports DXBASE
Imports NCCOM
Imports System.Data.SqlClient
Imports DevExpress.XtraPrinting

Public Class DxScaClf
    Dim DsRip As DataTable
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow

    Dim DsAge As DataTable
    Dim DaAge As SqlDataAdapter
    Dim RwAge As DataRow
    Dim DsOne As DataTable
    Dim DaOne As SqlDataAdapter

    Dim DsRie As DataTable
    Dim DaRie As SqlDataAdapter
    Dim RwRie As DataRow

    Dim MiglioFo As Int32
    Dim StrPrint, TiPosta, TiRiep, Analitico As String
    Dim PAGCOD As ArrayList
    Dim Em As New DevExpress.XtraEditors.Controls.CheckedListBoxItem
    Dim OkPasta As Boolean = False
    Dim UserId As String = ""


    Private Sub DxScaClf_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        XtraTabControl1.SelectedTabPageIndex = 0
        PrimoMiglio()
        Pulizia()
    End Sub
#Region "DETTAGLIO SCADENZE"
    Sub PrimoMiglio()
        Dim Cmd As New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        RadioGroup1.SelectedIndex = -1 : TiPosta = ""
        DateEdit1.EditValue = CDate("01/01/" & Today.Year) : DateEdit2.EditValue = CDate("31/12/" & Today.Year)
        DateEdit3.EditValue = CDate(Today)
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UserId = dataRd.Item("Sel14")
        End While
        dataRd.Close()
    End Sub
    Sub Pulizia()
        PAGCOD = New ArrayList
        PAGCOD.Add(1) : PAGCOD.Add(2) : PAGCOD.Add(3) : PAGCOD.Add(4) : PAGCOD.Add(5)
        PAGCOD.Add(6) : PAGCOD.Add(7) : PAGCOD.Add(8)
        DsRip = New DataTable : DsAge = New DataTable
        GridControl1.DataSource = DsRip
        GridColumn1.Visible = True : GridColumn2.Visible = True : GridColumn3.Visible = True : GridColumn4.Visible = True : GridColumn5.Visible = True
        GridColumn1.VisibleIndex = 2 : GridColumn2.VisibleIndex = 3 : GridColumn3.VisibleIndex = 4 : GridColumn4.VisibleIndex = 5
        GridColumn5.VisibleIndex = 6 : GridColumn6.VisibleIndex = 7 : GridColumn7.VisibleIndex = 8 : GridColumn8.VisibleIndex = 9
        GridColumn6.Visible = True : GridColumn7.Visible = True : GridColumn8.Visible = True
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Controllo() = False Then Exit Sub
        Pulizia()
        GroupControl2.Text = Format(DateEdit1.EditValue, "dd/MM/yyyy") & " - " & Format(DateEdit2.EditValue, "dd/MM/yyyy")
        If TiPosta = "CL" Then
            GroupControl2.Text = marchio() & " - Crediti Periodo " & GroupControl2.Text
        Else
            GroupControl2.Text = Marchio() & " - Debiti Periodo " & GroupControl2.Text
        End If
        Cursor.Current = Cursors.WaitCursor
        StrPrint = "select DISTINCT SCATPAG from DXSCACLF where PrkPAperta = '0' and CLFOPI = '" & TiPosta & "' AND  scadsca between '" & Format(DateEdit1.EditValue, "dd/MM/yyyy") & "' and '" & Format(DateEdit2.EditValue, "dd/MM/yyyy") & "'AND ScaScopRata <> 0 order by SCATPAG"
        Dim Cmd As New SqlCommand(StrPrint, cnCo)
        Dim p As Int16 = 0
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            p = dataRd.Item("SCATPAG") - 1
            Em = CheckedComboBoxEdit1.Properties.Items(p)
            For x As Int16 = 1 To PAGCOD.Count
                If PAGCOD(x - 1) = dataRd.Item("SCATPAG") And Em.CheckState = CheckState.Checked Then
                    PAGCOD.RemoveAt(x - 1)
                    Exit For
                End If
            Next
        End While
        dataRd.Close()
        For x As Int16 = 1 To PAGCOD.Count
            Select Case PAGCOD(x - 1)
                Case 1
                    GridColumn1.Visible = False
                Case 2
                    GridColumn2.Visible = False
                Case 3
                    GridColumn3.Visible = False
                Case 4
                    GridColumn4.Visible = False
                Case 5
                    GridColumn5.Visible = False
                Case 6
                    GridColumn6.Visible = False
                Case 7
                    GridColumn7.Visible = False
                Case 8
                    GridColumn8.Visible = False
            End Select
        Next
        StrPrint = "select * from DXSCACLF where PrkPAperta = '0' and CLFOPI = '" & TiPosta & "' AND  scadsca between '" & Format(DateEdit1.EditValue, "dd/MM/yyyy") & "' and '" & Format(DateEdit2.EditValue, "dd/MM/yyyy") & "'AND ScaScopRata <> 0 "
        Dim Ands As String = ""
        Dim Ors As String = " OR SCATPAG = "
        For x = 1 To CheckedComboBoxEdit1.Properties.Items.Count
            Em = CheckedComboBoxEdit1.Properties.Items(x - 1)
            If Em.CheckState = CheckState.Checked Then
                Ands &= Ors & Em.Value
            End If
        Next
        If Ands.Length = 0 Then GoTo Glob
        Ors = " AND ( " & Mid(Ands, 5, Ands.Length - 4) & " ) "
        StrPrint &= Ors
Glob:
        StrPrint &= " order by scadsca,clfopi,scadesc "

        DsRip = New DataTable
        DaRip = New SqlDataAdapter(StrPrint, cnCo)
        DaRip.SelectCommand.CommandTimeout = 300
        DaRip.Fill(DsRip)
        GridControl1.DataSource = DsRip
        GridView1.ClearSelection()
        GridView1.ExpandAllGroups()
    End Sub
    Private Sub DateEdit1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateEdit1.TextChanged
        If DateEdit1.EditValue Is Nothing Then DateEdit1.ErrorText = "Manca Data Inizio" Else DateEdit1.ErrorText = ""
    End Sub
    Private Sub DateEdit2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateEdit2.TextChanged
        If DateEdit2.EditValue Is Nothing Then DateEdit2.ErrorText = "Manca Data Fine" Else DateEdit2.ErrorText = ""
    End Sub
    Function Controllo() As Boolean
        Dim Msg As Boolean = True
        If RadioGroup1.SelectedIndex = -1 Then
            RadioGroup1.ErrorText = "Selezionare CLienti o Fornitori"
            Msg = False
        End If
        If DateEdit1.EditValue Is Nothing Then
            DateEdit1.ErrorText = "Manca Data Inizio"
            Msg = False
        End If
        If DateEdit2.EditValue Is Nothing Then
            DateEdit2.ErrorText = "Manca Data Fine"
            Msg = False
        End If
        If DateEdit1.EditValue > DateEdit2.EditValue Then
            DateEdit1.ErrorText = "Data Inizio > Data Fine"
            Msg = False
        End If
        Return Msg
    End Function
    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex > -1 Then
            TiPosta = RadioGroup1.Properties.Items(RadioGroup1.SelectedIndex).Value
            ButtonF1.PerformClick()
        End If
    End Sub
    Private Sub DxScaClf_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            If e.KeyData = Keys.F5 Then
                e.Handled = True
                ButtonF5.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F9 Then
                e.Handled = True
                ButtonF9.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F1 Then
                e.Handled = True
                ButtonF1.PerformClick()
                Exit Sub
            End If
        Else
            If e.KeyData = Keys.F5 Then
                e.Handled = True
                ButtonXF5.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F9 Then
                e.Handled = True
                ButtonXF9.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F1 Then
                e.Handled = True
                ButtonXF1.PerformClick()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim Land As Boolean = True
        Dim ResZ As Integer = GridView1.Columns.Item("SCADESC").Width
        If DsRip.Rows.Count = 0 Then Exit Sub
        If GridView1.VisibleColumns.Count < 9 Then
            Land = False
            GridView1.Columns.Item("SCADESC").Resize(250)
        End If
        DXANTEPRIMA(GridControl1, Land, Printing.PaperKind.A4, GroupControl2.Text)
        GridView1.Columns.Item("SCADESC").Resize(ResZ)
    End Sub
    Private Sub ButtonF5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Pulizia()
        DateEdit1.Focus()
    End Sub
#End Region
#Region "RIEPILOGO SCADENZE"
    Sub PuliziaRIEP()
        DsRie = New DataTable
        GridControl2.DataSource = DsRie
        GridControl3.DataSource = DsRie
    End Sub
    Private Sub ButtonXF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF1.Click
        Dim STRM As String = "EXEC DXRIESCA @AL='" & Format(DateEdit3.EditValue, "dd/MM/yyyy") & "',@CLFO = '" & TiRiep & "',@TIPO = '" & Analitico & "'"
        LANCIO(STRM)
    End Sub
    Sub LANCIO(str As String)
        If ControlloRIEP() = False Then Exit Sub
        PuliziaRIEP()
        GroupControl8.Text = Marchio() & " " & TiRiep & " PARTITE SCOPERTE Al " & Today & " Ore " & TimeOfDay & " Raggruppate al " & Format(DateEdit3.EditValue, "dd/MM/yyyy")
        Cursor.Current = Cursors.WaitCursor
        '''  Dim STR As String = "EXEC DXRIESCA @AL='" & Format(DateEdit3.EditValue, "dd/MM/yyyy") & "',@CLFO = '" & TiRiep & "',@TIPO = '" & Analitico & "'"
        DsRie = New DataTable
        DaRie = New SqlDataAdapter(str, cnCo)
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
    Private Sub SimpleButton1_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton1.Click
        Dim STRM As String = "EXEC DXRIESCAEXTRA @AL='" & Format(DateEdit3.EditValue, "dd/MM/yyyy") & "',@CLFO = '" & TiRiep & "',@TIPO = '" & Analitico & "'"
        LANCIO(STRM)
    End Sub
#End Region
#Region "GESTIONE FONDO PAGINA"
    Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            DateEdit1.Focus()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            DateEdit3.Focus()
            Exit Sub
        End If
    End Sub
#End Region

End Class