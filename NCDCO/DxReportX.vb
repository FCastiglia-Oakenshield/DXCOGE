Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Threading
Public Class DxReportX
    Dim TbPrint As DataTable
    Dim DaPrint As SqlDataAdapter

    Dim TbRp4 As DataTable
    Dim DaRp4 As SqlDataAdapter

    Dim TbRp3 As DataTable
    Dim DaRp3 As SqlDataAdapter

    Dim TbRp2 As DataTable
    Dim DaRp2 As SqlDataAdapter

    Dim TbRep As DataTable
    Dim DaRep As SqlDataAdapter

    Dim TbRp2Bis As DataTable
    Dim DaRp2Bis As SqlDataAdapter

    Dim TbAvl As DataTable
    Dim DaAvl As SqlDataAdapter

    Dim EseAnno(5), MaxEse As Int16
    Dim EseDal(5), EseAl(5) As Date
    Dim Sw As Int16 = 0
    Dim ii(0), Rif As Integer
    Dim Intesta As String = ""
    Dim CM As Integer = -1
    Dim FO As String = ""


    Private Sub DxReportX_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        If Sw = 0 Then
            DateEdit1.EditValue = Today : Apertura() : ComboBoxEdit2.Enabled = False : ComboBoxEdit3.Enabled = False : Sw = 1 : ButtonRESET8.PerformClick() : CheckButton1.Checked = False : CheckButton1.Checked = True : CheckButton2.Checked = False
        End If
        XtraTabControl1.SelectedTabPageIndex = 1
    End Sub
    Sub Apertura()
        Cmd = New SqlCommand("SELECT top 5 * from TbEse Order by EseAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        ComboBoxEdit5.Properties.Items.Clear()
        Dim x As Int16
        Dim Str As String = ""
        MaxEse = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MaxEse = MaxEse + 1
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("EseAnno"))
            ComboBoxEdit5.Properties.Items.Add(dataRd.Item("EseAnno"))
            EseAnno(MaxEse) = dataRd.Item("EseAnno")
            EseDal(MaxEse) = dataRd.Item("EseDal")
            EseAl(MaxEse) = dataRd.Item("EseAl")
        End While
        dataRd.Close()
        For x = 0 To MaxEse
            If ComboBoxEdit1.Properties.Items(x) = DateEdit1.EditValue.Year Then
                ComboBoxEdit1.SelectedIndex = x
                ComboBoxEdit5.SelectedIndex = x
                Esercizi(x) : EserciziDF(x)
                Exit For
            End If
        Next
        If Sw = 0 Then
            For x = 1 To ImageComboBoxEdit2.Properties.Items.Count
                ReDim Preserve ii(x - 1)
                ii(x - 1) = ImageComboBoxEdit2.Properties.Items(x - 1).ImageIndex
            Next
        End If
        ImageComboBoxEdit2.Text = ""
        ImageComboBoxEdit2.Properties.Items.Clear()
        Cmd = New SqlCommand("Select * from VTCommessa order by TcmSigla", cnDb)
        dataRd = Cmd.ExecuteReader
        Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
        While dataRd.Read
            Str = dataRd.Item("TcmSigla").ToString.PadRight(9, " ") & dataRd.Item("TcmOggetto")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Str, dataRd.Item("TcmRif"), ii(dataRd.Item("TIPO")))
            ImageComboBoxEdit2.Properties.Items.Add(nn)
        End While
        dataRd.Close()
        Rif = -1
        REM carico commesse avanzamento lavori
        Dim Str2 As String = "Select Distinct TcmRif,TcmSigla,TcmTipo,TcmOggetto from VLISTAVAN2  Order by TcmSigla"
        ImageComboBoxEdit1.Properties.Items.Clear()
        Dim SStr As String = ""
        Cmd = New SqlCommand(Str2, cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SStr = dataRd.Item("TcmSigla").ToString.PadRight(9, " ") & dataRd.Item("TcmOggetto")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SStr, dataRd.Item("TcmRif"), ii(dataRd.Item("TcmTipo")))
            ImageComboBoxEdit1.Properties.Items.Add(nn)
        End While
        dataRd.Close()
        REM carico FORNITORI avanzamento lavori
        Dim Strr As String = "Select Distinct TorCliFor,AnaDesc from VLISTAVAN2  Order by AnaDesc"
        ImageComboBoxEdit3.Properties.Items.Clear()
        Cmd = New SqlCommand(Strr, cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("AnaDesc"), dataRd.Item("TorCliFor"), -1)
            ImageComboBoxEdit3.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        Dim x As Int16
        x = ComboBoxEdit1.SelectedIndex
        If x = -1 Or sw = 0 Then Exit Sub
        Esercizi(x)
    End Sub
    Private Sub ComboBox5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit5.SelectedIndexChanged
        Dim x As Int16
        x = ComboBoxEdit5.SelectedIndex
        If x = -1 Or Sw = 0 Then Exit Sub
        EserciziDF(x)
    End Sub
    Sub EserciziDF(ByVal x As Int16)
        DateEdit6.Properties.MaxValue = "31/12/2050" ' reset campi per ricalcolare limiti
        DateEdit6.Properties.MinValue = "01/01/1900"
        DateEdit5.Properties.MaxValue = "31/12/2050"
        DateEdit5.Properties.MinValue = "01/01/1900"
        DateEdit6.Properties.MaxValue = EseDal(x).AddMinutes(1)
        DateEdit6.Properties.MinValue = EseDal(x)
        DateEdit6.EditValue = EseDal(x)
        DateEdit5.Properties.MaxValue = EseAl(x).AddMinutes(1)
        DateEdit5.Properties.MinValue = EseAl(x)
        DateEdit5.EditValue = EseAl(x)
        DateEdit4.Properties.MaxValue = "31/12/2050"
        DateEdit4.Properties.MinValue = "01/01/1900"
        DateEdit4.EditValue = DateEdit5.EditValue
        DateEdit4.Properties.MaxValue = DateEdit5.Properties.MaxValue
        DateEdit4.Properties.MinValue = DateEdit6.Properties.MinValue
        If DateEdit4.EditValue > Today Then DateEdit4.EditValue = Today
        DateEdit4.Focus()
    End Sub

    Sub Esercizi(ByVal x As Int16)
        DateEdit2.Properties.MaxValue = "31/12/2050" ' reset campi per ricalcolare limiti
        DateEdit2.Properties.MinValue = "01/01/1900"
        DateEdit3.Properties.MaxValue = "31/12/2050"
        DateEdit3.Properties.MinValue = "01/01/1900"
        DateEdit2.Properties.MaxValue = EseDal(x).AddMinutes(1)
        DateEdit2.Properties.MinValue = EseDal(x)
        DateEdit2.EditValue = EseDal(x)
        DateEdit3.Properties.MaxValue = EseAl(x).AddMinutes(1)
        DateEdit3.Properties.MinValue = EseAl(x)
        DateEdit3.EditValue = EseAl(x)
        DateEdit1.Properties.MaxValue = "31/12/2050"
        DateEdit1.Properties.MinValue = "01/01/1900"
        DateEdit1.EditValue = DateEdit3.EditValue
        DateEdit1.Properties.MaxValue = DateEdit3.Properties.MaxValue
        DateEdit1.Properties.MinValue = DateEdit2.Properties.MinValue
        If DateEdit1.EditValue > Today Then DateEdit1.EditValue = Today
        DateEdit1.Focus()
    End Sub
    Private Sub SimpleButton7_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton7.Click
        PopolaDf()
    End Sub
    Sub PopolaDf()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "XFATTFO @DAL='" & CDate(DateEdit6.EditValue).ToShortDateString & "',@AL='" & CDate(DateEdit4.EditValue).ToShortDateString & "'"
        TbPrint = New DataTable()
        DaPrint = New SqlDataAdapter(Str, CnDc)
        DaPrint.Fill(TbPrint)
        GridControl11.DataSource = TbPrint
        GridView11.ClearSelection()
        Cursor = Cursors.Default
    End Sub
    Private Sub SimpleButton6_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton6.Click
        DXANTEPRIMA(GridControl11, True, Printing.PaperKind.A3, "")
    End Sub

    Private Sub ButtonST_Click(sender As System.Object, e As System.EventArgs) Handles ButtonST.Click
        PopolaStampa()
    End Sub
    Sub PopolaStampa()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "XREPORTONE @DAL='" & CDate(DateEdit2.EditValue).ToShortDateString & "',@AL='" & CDate(DateEdit1.EditValue).ToShortDateString & "'"
        TbPrint = New DataTable()
        DaPrint = New SqlDataAdapter(Str, CnDc)
        DaPrint.Fill(TbPrint)
        GridControlLP.DataSource = TbPrint
        GridViewLP.ClearSelection()
        Cursor = Cursors.Default
        ComboBoxEdit2.Enabled = True
    End Sub

    Private Sub ButtonP_Click(sender As System.Object, e As System.EventArgs) Handles ButtonP.Click
        TravasaDati()
        Select Case ComboBoxEdit2.SelectedIndex
            Case Is <= 0
                DXANTEPRIMA(GridControlLP, True, Printing.PaperKind.A3, "")
            Case 1
                Report1()
            Case 2
                Report2()
            Case 3
                Report3()
            Case 4
                Report4()
            Case 5
                Report5()
            Case 6
                Report6()
        End Select
    End Sub
    Sub TravasaDati()
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en")
        Dim str As String = "delete from TMPREPORTX INSERT INTO TMPREPORTX SELECT * FROM TMPREPORT "
        If GridViewLP.ActiveFilterString > "" Then
            str &= " WHERE " & GridViewLP.ActiveFilterString
        End If
        Cmd = New SqlCommand(str, CnDc)
        Cmd.ExecuteNonQuery()
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("it")
    End Sub
    Sub Report1()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "Select * from DxReportOne1 order by [Tipologia],[Commessa],Avere,Dare"
        TbRep = New DataTable()
        DaRep = New SqlDataAdapter(Str, CnDc)
        DaRep.Fill(TbRep)
        GridControl1.DataSource = TbRep
        GridView1.ClearSelection()
        Cursor = Cursors.Default
        DXANTEPRIMA(GridControl1, False, Printing.PaperKind.A4, ComboBoxEdit2.EditValue.ToString, "", True, "", "", , GridViewLP.ActiveFilterString, )
    End Sub
    Sub Report2()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "Select * from DxReportOne2 order by [Tipologia],[Commessa],[Livello 1],Avere,Dare"
        TbRep = New DataTable()
        DaRep = New SqlDataAdapter(Str, CnDc)
        DaRep.Fill(TbRep)
        GridControl2.DataSource = TbRep
        GridView2.ClearSelection()
        Cursor = Cursors.Default
        DXANTEPRIMA(GridControl2, False, Printing.PaperKind.A4, ComboBoxEdit2.EditValue.ToString, "", True, "", "", , GridViewLP.ActiveFilterString, )
    End Sub
    Sub Report3()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "Select * from DxReportOne3 order by [Tipologia],[Commessa],[Livello 1],[Livello 2],Avere,Dare"
        TbRep = New DataTable()
        DaRep = New SqlDataAdapter(Str, CnDc)
        DaRep.Fill(TbRep)
        GridControl3.DataSource = TbRep
        GridView3.ClearSelection()
        Cursor = Cursors.Default
        DXANTEPRIMA(GridControl3, False, Printing.PaperKind.A4, ComboBoxEdit2.EditValue.ToString, "", True, "", "", , GridViewLP.ActiveFilterString, )
    End Sub
    Sub Report4()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "Select * from DxReportOne4 order by [Tipologia],[Commessa],[Livello 1],[Livello 2],[Repertorio],Avere,Dare"
        TbRep = New DataTable()
        DaRep = New SqlDataAdapter(Str, CnDc)
        DaRep.Fill(TbRep)
        GridControl4.DataSource = TbRep
        GridView4.ClearSelection()
        Cursor = Cursors.Default
        DXANTEPRIMA(GridControl4, False, Printing.PaperKind.A4, ComboBoxEdit2.EditValue.ToString, "", True, "", "", , GridViewLP.ActiveFilterString, )
    End Sub
    Sub Report5()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "Select * from DxReportOne5 order by [Tipologia],[Commessa],[Mastro],Avere,Dare"
        TbRep = New DataTable()
        DaRep = New SqlDataAdapter(Str, CnDc)
        DaRep.Fill(TbRep)
        GridControl5.DataSource = TbRep
        GridView5.ClearSelection()
        Cursor = Cursors.Default
        DXANTEPRIMA(GridControl5, False, Printing.PaperKind.A4, ComboBoxEdit2.EditValue.ToString, "", True, "", "", , GridViewLP.ActiveFilterString, )
    End Sub
    Sub Report6()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "Select * from DxReportOne6 order by [Tipologia],[Commessa],[Mastro],[Conto],Avere,Dare"
        TbRep = New DataTable()
        DaRep = New SqlDataAdapter(Str, CnDc)
        DaRep.Fill(TbRep)
        GridControl6.DataSource = TbRep
        GridView6.ClearSelection()
        Cursor = Cursors.Default
        DXANTEPRIMA(GridControl6, False, Printing.PaperKind.A4, ComboBoxEdit2.EditValue.ToString, "", True, "", "", , GridViewLP.ActiveFilterString, )
    End Sub
#Region "GESTIONE FONDO PAGINA"
    Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            Report1()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            ButtonST.Focus()
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 2 Then
            ImageComboBoxEdit2.Focus()
            Exit Sub
        End If
    End Sub
#End Region
#Region "REPORT 3"
    Private Sub ImageComboBoxEdit2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit2.SelectedIndexChanged
        If ImageComboBoxEdit2.SelectedIndex > -1 Then
            Rif = ImageComboBoxEdit2.Properties.Items(ImageComboBoxEdit2.SelectedIndex).Value
        Else
            Rif = -1
        End If
    End Sub
    Private Sub ButtonST3_Click(sender As System.Object, e As System.EventArgs) Handles ButtonST3.Click
        PopolaReport3()
    End Sub
    Sub PopolaReport3()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "XDETTAGLIMOV @RCOM = " & Rif
        TbRp3 = New DataTable()
        DaRp3 = New SqlDataAdapter(Str, cnDb)
        DaRp3.SelectCommand.CommandTimeout = 300
        DaRp3.Fill(TbRp3)
        GridControl7.DataSource = TbRp3
        GridView7.ClearSelection()
        Cursor = Cursors.Default
        ComboBoxEdit3.Enabled = True
        ''If Rif > 0 Then GridColumn45.Visible = False : GridColumn35.Visible = False
    End Sub
    Private Sub ButtonP3_Click(sender As System.Object, e As System.EventArgs) Handles ButtonP3.Click
        Select Case ComboBoxEdit3.SelectedIndex
            Case Is <= 0
                ''  If Rif > 0 Then Intesta = ImageComboBoxEdit2.Text Else Intesta = ""
                DXANTEPRIMA(GridControl7, True, Printing.PaperKind.A3, "")
        End Select
    End Sub
    Private Sub ButtonRESET_Click(sender As System.Object, e As System.EventArgs) Handles ButtonRESET.Click
        ImageComboBoxEdit2.SelectedIndex = -1 : Rif = -1 : TbRp3 = New DataTable() : GridControl7.DataSource = TbRp3
    End Sub
#End Region
#Region "REPORT 2"
    Private Sub SimpleButton2_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton2.Click
        PopolaReport2()
    End Sub
    Sub PopolaReport2()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "XREPORTTWO"
        TbRp2 = New DataTable()
        DaRp2 = New SqlDataAdapter(Str, cnDb)
        DaRp2.SelectCommand.CommandTimeout = 300
        DaRp2.Fill(TbRp2)
        GridControl8.DataSource = TbRp2
        GridView8.ClearSelection()
        Cursor = Cursors.Default
    End Sub
    Private Sub uttonP2_Click(sender As System.Object, e As System.EventArgs) Handles ButtonP2.Click
        DXANTEPRIMA(GridControl8, True, Printing.PaperKind.A3, "")
    End Sub
    Private Sub ButtonRESET2_Click(sender As System.Object, e As System.EventArgs) Handles ButtonReset2.Click
        TbRp2 = New DataTable() : GridControl8.DataSource = TbRp2
    End Sub
#End Region
#Region "REPORT 4"
    Sub PopolaReport4()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "XREPORTFOUR"
        TbRp4 = New DataTable()
        DaRp4 = New SqlDataAdapter(Str, cnDb)
        DaRp4.SelectCommand.CommandTimeout = 300
        DaRp4.Fill(TbRp4)
        GridControl9.DataSource = TbRp4
        GridView9.ClearSelection()
        Cursor = Cursors.Default
    End Sub
    Private Sub SimpleButton4_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton4.Click
        PopolaReport4()
    End Sub

    Private Sub ResetButton3_Click_1(sender As System.Object, e As System.EventArgs) Handles ResetButton3.Click
        TbRp4 = New DataTable() : GridControl9.DataSource = TbRp4
    End Sub

    Private Sub PrintButton1_Click(sender As System.Object, e As System.EventArgs) Handles PrintButton1.Click
        DXANTEPRIMA(GridControl9, True, Printing.PaperKind.A3, "")
    End Sub
#End Region
#Region "REPORT 2 BIS"
    Private Sub SimpleButton5_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton5.Click
        PopolaReport2BIS()
    End Sub
    Sub PopolaReport2BIS()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "XREPORTTWOBIS"
        TbRp2Bis = New DataTable()
        DaRp2Bis = New SqlDataAdapter(Str, cnDb)
        DaRp2Bis.SelectCommand.CommandTimeout = 300
        DaRp2Bis.Fill(TbRp2Bis)
        GridControl10.DataSource = TbRp2Bis
        GridView10.ClearSelection()
        Cursor = Cursors.Default
    End Sub
    Private Sub SimpleButton1_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton1.Click
        DXANTEPRIMA(GridControl10, True, Printing.PaperKind.A3, "")
    End Sub
    Private Sub SimpleButton3_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton3.Click
        TbRp2Bis = New DataTable() : GridControl10.DataSource = TbRp2Bis
    End Sub

#End Region
#Region "REPORT SITUAZIONE AVANZAMENTI"
    Private Sub CheckButton1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckButton1.CheckedChanged
        If CheckButton1.Checked = True Then
            CheckButton1.ImageIndex = 73
            CheckButton1.ToolTip = "FORNITORI"
            GroupControl31.Visible = False : GroupControl30.Visible = True
            ButtonRESET8.PerformClick()
            ImageComboBoxEdit1.Focus()
        Else
            CheckButton1.ImageIndex = 81
            CheckButton1.ToolTip = "COMMESSE"
            GroupControl31.Visible = True : GroupControl30.Visible = False
            ButtonRESET8.PerformClick()
            ImageComboBoxEdit3.Focus()
        End If
    End Sub

    Private Sub ButtonRESET8_Click(sender As System.Object, e As System.EventArgs) Handles ButtonRESET8.Click
        ImageComboBoxEdit1.SelectedIndex = -1 : ImageComboBoxEdit3.SelectedIndex = -1 : TbAvl = New DataTable() : GridControl12.DataSource = TbAvl : CheckButton2.Checked = False
        If CheckButton1.Checked = True Then ImageComboBoxEdit3.Focus() Else ImageComboBoxEdit1.Focus()
    End Sub


    Private Sub SimpleButton9_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton9.Click
        If ImageComboBoxEdit1.SelectedIndex = -1 And ImageComboBoxEdit3.SelectedIndex = -1 Then Exit Sub
        If ImageComboBoxEdit1.SelectedIndex = -1 Then
            CM = -1
            FO = ImageComboBoxEdit3.EditValue
        Else
            CM = ImageComboBoxEdit1.EditValue
            FO = ""
        End If
        ElaboraAVL()
    End Sub
    Sub ElaboraAVL()
        Cursor = Cursors.WaitCursor
        Dim s As String = "XREPORTAVL"
        If CheckButton2.Checked = True Then s = "XREPORTAVLDETT"
        Dim Str As String = "exec " & s & " @CM = " & CM & ",@FO = '" & FO & "'"
        TbAvl = New DataTable()
        DaAvl = New SqlDataAdapter(Str, cnDb)
        DaAvl.SelectCommand.CommandTimeout = 300
        DaAvl.Fill(TbAvl)
        GridControl12.DataSource = TbAvl
        GridView12.ClearSelection()
        Cursor = Cursors.Default
    End Sub
    Private Sub ImageComboBoxEdit1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ImageComboBoxEdit1.SelectedIndexChanged, ImageComboBoxEdit3.SelectedIndexChanged
        If sender.SelectedIndex > -1 Then SimpleButton9.Focus()
    End Sub
    Private Sub SimpleButton10_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton10.Click
        DXANTEPRIMA(GridControl12, True, Printing.PaperKind.A3, "")
    End Sub

    Private Sub CheckButton2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckButton2.CheckedChanged
        If CheckButton2.Checked = True Then
            CheckButton2.ImageIndex = 0
            CheckButton2.ToolTip = "DETTAGLI ON"
            GridColumn139.Visible = True
            GridColumn141.Visible = True
            GridColumn142.Visible = True
            GridColumn143.Visible = True
            GridColumn144.Visible = True
            GridColumn145.Visible = True
            GridColumn146.Visible = True
            GridColumn147.Visible = True
            GridColumn139.VisibleIndex = 15
            GridColumn141.VisibleIndex = 16
            GridColumn142.VisibleIndex = 17
            GridColumn143.VisibleIndex = 18
            GridColumn144.VisibleIndex = 19
            GridColumn145.VisibleIndex = 20
            GridColumn146.VisibleIndex = 21
            GridColumn147.VisibleIndex = 22
        Else
            CheckButton2.ImageIndex = 1
            CheckButton2.ToolTip = "DETTAGLI OFF"
            GridColumn139.Visible = False
            GridColumn141.Visible = False
            GridColumn142.Visible = False
            GridColumn143.Visible = False
            GridColumn144.Visible = False
            GridColumn145.Visible = False
            GridColumn146.Visible = False
            GridColumn147.Visible = False
        End If
        SimpleButton9.PerformClick()
    End Sub
#End Region

End Class