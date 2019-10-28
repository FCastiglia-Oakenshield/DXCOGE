Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports NPRINT

Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Public Class DxBilCdc
    Friend WithEvents prntDoc As System.Drawing.Printing.PrintDocument
    Dim prntDial As New PrintDialog


    Dim DsAgS As DataTable
    Dim DaAgS As SqlDataAdapter
    Dim RwAgS As DataRow

    Dim DsAgD As DataTable
    Dim DaAgD As SqlDataAdapter
    Dim RwAgD As DataRow

    Dim Iset2 As Integer

    Dim STnull As String = " select LDPRIF,LDPSIGLA,LDPDESC from TbLdp where LdpRif = 32000"

    Dim UltimaApertura, sw, MaxEse, EseFormato(5), TipoStampa, EseBilChi(5), CauChiusura, EseAnno(5) As Int16
    Dim EseSaDa(5), EseSaA(5), EseSpDa(5), EseSpA(5), EseCeDa(5), EseCeA(5) As Int32
    Dim IdBlk, P1, P2, P3, P4, P5, P6 As Int32
    Dim OkFlash, OkQuote As Boolean
    Dim Quote As Decimal = 0
    Dim Ammortamenti As String = ""
    Dim EseDal(5), EseAl(5) As Date
    Dim EseProg(5), EseSppp(5), EseClFo(5), EseSdo(5), EseQuote(5) As Boolean

    Dim REPORT As New XtraReport
    Dim selectformula, SCRI, StrPrint, Formula As String

    Dim DsBilCdc As DataTable
    Dim DaBilCdc As SqlDataAdapter


    Private Sub DxBilCdc_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If sw = 0 Then
            DateEdit1.EditValue = Today
            Apertura()
            sw = 1
        End If

        PopolaGrid()
    End Sub
    Sub Apertura()
        Dim Cmd As New SqlCommand("SELECT top 5 * from TbEse Order by EseAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        MaxEse = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MaxEse = MaxEse + 1
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("EseAnno"))
            EseAnno(MaxEse) = dataRd.Item("EseAnno")
            EseDal(MaxEse) = dataRd.Item("EseDal")
            EseAl(MaxEse) = dataRd.Item("EseAl")
            EseFormato(MaxEse) = dataRd.Item("EseFormato")
        End While
        dataRd.Close()
        If MaxEse > -1 Then
            ComboBoxEdit1.SelectedIndex = 0
            Esercizi(0)
        Else
            Me.Close()
            Exit Sub
        End If
        CheckEdit8.Checked = False
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
        
        TipoStampa = EseFormato(x)
        CauChiusura = EseBilChi(x)
        DateEdit1.Focus()
        P1 = EseSaDa(x)
        P2 = EseSaA(x)
        P3 = EseSpDa(x)
        P4 = EseSpA(x)
        P5 = EseCeDa(x)
        P6 = EseCeA(x)

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        Dim x As Int16
        x = ComboBoxEdit1.SelectedIndex
        If x = -1 Or sw = 0 Then Exit Sub
        Esercizi(x)
    End Sub
    Sub PopolaGrid()
        Dim STR As String = "Select LDPRIF,LDPSIGLA,LDPDESC from TbLdp"
        DsAgS = New DataTable
        DaAgS = New SqlDataAdapter(STR, CnDc)
        DaAgS.Fill(DsAgS)
        GridControl1.DataSource = DsAgS
        GridControl1.Refresh()
        GridView1.ClearSelection()
        DsAgD = New DataTable
        DaAgD = New SqlDataAdapter(STnull, CnDc)
        DaAgD.Fill(DsAgD)
        DsAgD.AcceptChanges()
        GridControl2.DataSource = DsAgD
        GridControl2.Refresh()
        GridView2.ClearSelection()
    End Sub
    Private Sub CheckButton1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckButton1.CheckedChanged
        If CheckButton1.Checked = True Then
            CheckButton1.ImageIndex = 27
            CheckButton1.ToolTip = "TOGLI da STAMPA"
            PassaTutti()
        Else
            CheckButton1.ImageIndex = 26
            CheckButton1.ToolTip = "INVIA in STAMPA"
            PopolaGrid()
        End If
    End Sub
    Sub PassaTutti()
        Dim STR As String = "Select LDPRIF,LDPSIGLA,LDPDESC from TbLdp"
        DsAgS = New DataTable
        DaAgS = New SqlDataAdapter(STnull, CnDc)
        DaAgS.Fill(DsAgS)
        GridControl1.DataSource = DsAgS
        GridControl1.Refresh()
        GridView1.ClearSelection()
        DsAgD = New DataTable
        DaAgD = New SqlDataAdapter(STR, CnDc)
        DaAgD.Fill(DsAgD)
        DsAgD.AcceptChanges()
        GridControl2.DataSource = DsAgD
        GridControl2.Refresh()
        GridView2.ClearSelection()
    End Sub
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub
    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If iset > -1 Then
            RwAgS = GridView1.GetDataRow(iset)
            exButton79()
        End If
    End Sub
    Sub exButton79()
        Dim x As Int16
        If iset > -1 Then
            RwAgD = DsAgD.NewRow()
            For x = 1 To RwAgS.ItemArray.Length
                RwAgD(x - 1) = RwAgS(x - 1)
            Next
            DsAgD.Rows.Add(RwAgD)
            DsAgD.AcceptChanges()
            x = DsAgD.Rows.Count - 1
            GridView1.ClearSelection()
            GridView2.ClearSelection()
            GridControl2.DataSource = DsAgD
            GridControl2.Refresh()
            If x > -1 Then
                GridView2.SelectRow(x)
                GridView2.FocusedRowHandle = x
            End If
            DsAgS.Rows(iset).Delete()
            DsAgS.AcceptChanges()
            GridControl1.DataSource = DsAgS
            GridControl1.Refresh()
        End If
        iset = -1
    End Sub
    Private Sub GridControl2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl2.MouseMove
        ShowHitInfo2(GridView2.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo2(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl2.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset2 = hi.RowHandle
    End Sub
    Private Sub GridView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.Click
        If Iset2 > -1 Then
            RwAgD = GridView2.GetDataRow(Iset2)
            ExButton77()
        End If
    End Sub
    Sub ExButton77()
        Dim x As Int16
        If Iset2 > -1 Then
            RwAgS = DsAgS.NewRow()
            For x = 1 To RwAgD.ItemArray.Length
                RwAgS(x - 1) = RwAgD(x - 1)
            Next
            DsAgS.Rows.Add(RwAgS)
            DsAgS.AcceptChanges()
            x = DsAgS.Rows.Count - 1
            GridView2.ClearSelection()
            GridView1.ClearSelection()
            GridControl1.DataSource = DsAgS
            GridControl1.Refresh()
            If x > -1 Then
                GridView1.SelectRow(x)
                GridView1.FocusedRowHandle = x
            End If
            DsAgD.Rows(Iset2).Delete()
            DsAgD.AcceptChanges()
            GridControl2.DataSource = DsAgD
            GridControl2.Refresh()
        End If
        Iset2 = -1
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If DsAgD.Rows.Count = 0 Then
            messaggio()
            Exit Sub
        End If

        Stampa()
    End Sub
    Sub messaggio()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "SELEZIONARE ALMENO UN CODICE TRA QUELLI DISPONIBILI!!! "
        style = MsgBoxStyle.Exclamation
        title = "???? SELEZIONE ????"
        response = MsgBox(msg, style, title)
    End Sub
    Sub Stampa()
        Dim RR As String
        If CheckEdit8.Checked = True Then RR = " -R-" Else RR = ""
        PreparoFormula()
        CaricaSaldi()
        DsBilCdc = New DataTable
        DaBilCdc = New SqlDataAdapter(StrPrint, CnDc)
        DaBilCdc.SelectCommand.CommandTimeout = 300
        DaBilCdc.Fill(DsBilCdc)
        selectformula = ""
        REPORT = New DxStBilCdc
        REPORT.DataSource = DsBilCdc
        REPORT.DataMember = "DsBilCdc"
        REPORT.FilterString = selectformula
        REPORT.Parameters("Titolo").Value = "ANNO " & ComboBoxEdit1.EditValue & " - CONTO ECONOMICO " & DateEdit2.EditValue & " - " & DateEdit1.EditValue & RR
        REPORT.Parameters("Marchio").Value = Marchio()
        REPORT.ShowPreview()
    End Sub
    Sub PreparoFormula()
        Cursor.Current = Cursors.WaitCursor

        Formula = ""
        Dim jj As Int16
        For jj = 1 To DsAgD.Rows.Count
            RwAgD = DsAgD.Rows(jj - 1)
            'If jj = 1 Then Formula = "WHERE "
            'Formula = Formula & "{FnCDCF1.MCCCOGLDP} = " & RwAgD("LdpCod")
            Formula = Formula & "MCCCOGLDP = " & RwAgD("LdpRif")
            If jj < DsAgD.Rows.Count Then Formula = Formula & " or "
        Next
    End Sub
    Sub CaricaSaldi()
        Dim d1, d2, d5 As String
        d1 = CDate(DateEdit2.EditValue).ToShortDateString
        d2 = DateEdit1.EditValue
        If CheckEdit8.Checked = True Then d5 = d2 Else d5 = "31/12/2050"
        StrPrint = "select distinct * from FnCDCf1 ('" & d1 & "','" & d2 & "') where (" & Formula & ") Order by LDPSIGLA,MCCCOGCDC,MCCCOGREP,PIAFL01,MCCCOGCONTO,CONTODES"
    End Sub


    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.EditValueChanged
        Dim Mese, Giorno, Anno As String
        Mese = CDate(DateEdit1.EditValue).Month
        Anno = CDate(DateEdit1.EditValue).Year
        Giorno = Date.DaysInMonth(Anno, Mese)
        If CDate(DateEdit1.EditValue).ToShortDateString = CDate(Giorno & "/" & Mese & "/" & Anno).ToShortDateString Then
            CheckEdit8.Enabled = True
        Else
            CheckEdit8.Checked = False : CheckEdit8.Enabled = False
        End If
    End Sub
End Class