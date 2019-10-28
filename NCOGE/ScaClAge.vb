Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.Data.SqlClient
Imports DevExpress.XtraPrinting
Imports System.Drawing
Imports DevExpress.Data
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Public Class ScaClAge
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


    Private Sub DXSCACLAGE_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
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
        TiPosta = "CL"
        DateEdit1.EditValue = CDate("01/01/" & Today.Year) : DateEdit2.EditValue = CDate("31/12/" & Today.Year)
        DateEdit3.EditValue = CDate(Today)
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UserId = dataRd.Item("Sel14")
        End While
        dataRd.Close()
        REM carico Agenti
        CheckedComboBoxEdit2.Properties.Items.Clear()
        Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(0, "0 -SENZA AGENTE-", CheckState.Unchecked)
        CheckedComboBoxEdit2.Properties.Items.Add(Em)
        Cmd = New SqlCommand("select AgeId,AgeDesc from tbagenti Order by AgeId", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(dataRd.Item("AgeId"), (dataRd.Item("AgeId") & " " & dataRd.Item("AgeDesc")).ToString, CheckState.Checked)
            CheckedComboBoxEdit2.Properties.Items.Add(Em)
        End While
        dataRd.Close()
        REM carico Nazioni
        CheckedComboBoxEdit3.Properties.Items.Clear()
        Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(0, "0 -SENZA NAZIONE-", CheckState.Unchecked)
        CheckedComboBoxEdit3.Properties.Items.Add(Em)
        Cmd = New SqlCommand("select NazId,NazDesc from tbNazioni Order by NazId", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(dataRd.Item("NazId"), (dataRd.Item("NazId") & " " & dataRd.Item("NazDesc")).ToString, CheckState.Checked)
            CheckedComboBoxEdit3.Properties.Items.Add(Em)
        End While
        dataRd.Close()
    End Sub
    Sub Pulizia()
        PAGCOD = New ArrayList
        PAGCOD.Add(1) : PAGCOD.Add(2) : PAGCOD.Add(3) : PAGCOD.Add(4) : PAGCOD.Add(5)
        PAGCOD.Add(6) : PAGCOD.Add(7) : PAGCOD.Add(8)
        DsRip = New DataTable : DsAge = New DataTable
        GridControl1.DataSource = DsRip
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Controllo() = False Then Exit Sub
        Pulizia()
        GroupControl2.Text = Marchio() & "Crediti Periodo Dal " & Format(DateEdit1.EditValue, "dd/MM/yyyy") & " al " & Format(DateEdit2.EditValue, "dd/MM/yyyy")
        Cursor.Current = Cursors.WaitCursor
        StrPrint = "select * from DXSCACLAGENAZ where PrkPAperta = '0' and CLFOPI = '" & TiPosta & "' AND  scadsca between '" & Format(DateEdit1.EditValue, "dd/MM/yyyy") & "' and '" & Format(DateEdit2.EditValue, "dd/MM/yyyy") & "' AND ScaScopRata <> 0 "
        Dim Ands As String = ""
        Dim Ors As String = " OR SCATPAG = "
        For x = 1 To CheckedComboBoxEdit1.Properties.Items.Count
            Em = CheckedComboBoxEdit1.Properties.Items(x - 1)
            If Em.CheckState = CheckState.Checked Then
                Ands &= Ors & Em.Value
            End If
        Next
        If Ands.Length = 0 Then GoTo Glob1
        Ors = " AND ( " & Mid(Ands, 5, Ands.Length - 4) & " ) "
        StrPrint &= Ors
Glob1:
        Ors = " OR AgeCod = " : Ands = ""
        For x = 1 To CheckedComboBoxEdit2.Properties.Items.Count
            Em = CheckedComboBoxEdit2.Properties.Items(x - 1)
            If Em.CheckState = CheckState.Checked Then
                Ands &= Ors & Em.Value
            End If
        Next
        If Ands.Length = 0 Then GoTo Glob2
        Ors = " AND ( " & Mid(Ands, 5, Ands.Length - 4) & " ) "
        StrPrint &= Ors
Glob2:
        Ors = " OR NazCod = " : Ands = ""
        For x = 1 To CheckedComboBoxEdit3.Properties.Items.Count
            Em = CheckedComboBoxEdit3.Properties.Items(x - 1)
            If Em.CheckState = CheckState.Checked Then
                Ands &= Ors & Em.Value
            End If
        Next
        If Ands.Length = 0 Then GoTo Glob
        Ors = " AND ( " & Mid(Ands, 5, Ands.Length - 4) & " ) "
        StrPrint &= Ors
Glob:
        StrPrint &= " order by Agente,Nazione,scadesc,ScaDsca"
        DsRip = New DataTable
        DaRip = New SqlDataAdapter(StrPrint, cnDb)
        DaRip.SelectCommand.CommandTimeout = 300
        DaRip.Fill(DsRip)
        GridControl1.DataSource = DsRip
        GridView1.ClearSelection()
        GridView1.ExpandAllGroups()
        Dim gridView As DevExpress.XtraGrid.Views.Grid.GridView = GridControl1.FocusedView
        gridView.BeginSort()
        Try
            gridView.ClearSorting()
            gridView.Columns("SCADESC").SortOrder = ColumnSortOrder.Ascending
        Finally
            gridView.EndSort()
        End Try

        ''  AddHandler GridView1.BeforePrintRow, AddressOf GridView1_BeforePrintRow
    End Sub
    'Private Sub GridView1_BeforePrintRow(sender As Object, e As DevExpress.XtraGrid.Views.Printing.CancelPrintRowEventArgs)
    '    Dim rowHeight As Integer = 20
    '    Dim currentView As DevExpress.XtraGrid.Views.Grid.GridView = CType(sender, DevExpress.XtraGrid.Views.Grid.GridView)

    '    If currentView.IsGroupRow(e.RowHandle) AndAlso e.Level = 0 AndAlso e.RowHandle < -1 Then
    '        Dim viewInfo As GridViewInfo = currentView.GetViewInfo()
    '        For Each rowInfo As GridRowInfo In viewInfo.RowsInfo
    '            If rowInfo.RowHandle = e.RowHandle Then
    '                rowHeight = rowInfo.Bounds.Height
    '            End If
    '        Next
    '        'it is a group row, the highest level group, and it is not the very first group
    '        e.PS.InsertPageBreak(e.Y + rowHeight)
    '        e.Y += 1
    '    End If
    '    currentView = Nothing
    'End Sub
    Private Sub DateEdit2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateEdit2.TextChanged
        If DateEdit2.EditValue Is Nothing Then DateEdit2.ErrorText = "Manca Data Fine" Else DateEdit2.ErrorText = ""
    End Sub
    Private Sub DateEdit2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateEdit2.Leave
        ButtonF1.Focus()
    End Sub
    Function Controllo() As Boolean
        Dim Msg As Boolean = True
        If DateEdit2.EditValue Is Nothing Then
            DateEdit2.ErrorText = "Manca Data Fine"
            Msg = False
        End If
        Return Msg
    End Function
    Private Sub DXSCACLAGE_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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
        If DsRip.Rows.Count = 0 Then Exit Sub
        Cursor.Current = Cursors.WaitCursor
        Dim REPORT = New XScaClAge
        REPORT.DataSource = DsRip
        REPORT.DataMember = "DS"
        REPORT.Parameters.Item("DAL").Value = CDate(DateEdit1.EditValue)
        REPORT.Parameters.Item("AAL").Value = CDate(DateEdit2.EditValue)
        REPORT.Parameters.Item("AZIENDA").Value = Marchio()
        REPORT.CreateDocument()
        REPORT.ShowPreviewDialog()
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub ButtonF5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Pulizia()
        DateEdit1.Focus()
    End Sub
    'Private Sub ButtonF9Y_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
    '    If DsRip.Rows.Count = 0 Then Exit Sub
    '    Cursor.Current = Cursors.WaitCursor
    '    Dim Land As Boolean = True
    '    Dim ResZ As Integer = GridView1.Columns.Item("SCADESC").Width
    '    DXANTEPRIMA(GridControl1, Land, System.Drawing.Printing.PaperKind.A4, GroupControl2.Text)
    '    Cursor.Current = Cursors.Default
    'End Sub
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
            DXANTEPRIMA(GridControl2, True, System.Drawing.Printing.PaperKind.A4, GroupControl8.Text)
        Else
            DXANTEPRIMA(GridControl3, True, System.Drawing.Printing.PaperKind.A4, GroupControl8.Text)
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
            DateEdit2.Focus()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            DateEdit3.Focus()
            Exit Sub
        End If
    End Sub
#End Region


End Class