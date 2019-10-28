Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports CrystalDecisions.CrystalReports.Engine
Imports NPRINT
Public Class DxCorIva
    Dim Str As String
    Dim TbTot, TbTotA As DataTable
    Dim Annuale As Boolean = True
    Dim Rpt As New ReportClass
    Dim Rpt1 As New StIvaAnn
    Dim Rpt2 As New StaTotIva

    Dim DsIvaP As DataTable
    Dim DaIvaP As SqlDataAdapter
    Dim RwReg As DataRow
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim Reg As Int16 = -1

    Private Sub DxCorIva_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Pulizia()
    End Sub

    Private Sub Pulizia()
        GridControl1.DataSource = Nothing
        Dim Cmd As New SqlCommand("SELECT distinct RivaAnno from TbRegIva Order by RivaAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        Dim x As Int16 = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("RIvaAnno"))
        End While
        dataRd.Close()
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Exit Sub
        Str = "select IvaPAnno,IvaPRegIva,IvaPMese,sum(ivapimpon) as TotImpon, sum(ivapivade) as TotIvaDe,sum(ivapivand) as TotIvaND,sum(ivapimpmerce) as TotImpMerce from VCiiIvaP group by IvaPAnno,IvaPRegIva,IvaPMese"
        TbTot = New DataTable("TbTot")
        Dim DaTot As New SqlDataAdapter(Str, cnCo)
        DaTot.Fill(TbTot)
        TbTot.PrimaryKey = New DataColumn() {TbTot.Columns(0), TbTot.Columns(1), TbTot.Columns(2)}

        Str = "select IvaPAnno,IvaPRegIva,'',sum(ivapimpon) as TotImpon, sum(ivapivade) as TotIvaDe,sum(ivapivand) as TotIvaND,sum(ivapimpmerce) as TotImpMerce from VCiiIvaP group by IvaPAnno,IvaPRegIva"
        TbTotA = New DataTable("TbTotA")
        DaTot = New SqlDataAdapter(Str, cnCo)
        DaTot.Fill(TbTotA)
        TbTotA.PrimaryKey = New DataColumn() {TbTotA.Columns(0), TbTotA.Columns(1)}

        ComboBoxEdit1.SelectedIndex = 0
        ComboBoxEdit2.SelectedIndex = ComboBoxEdit2.Properties.Items.Count - 1
        PopolaRegime(ComboBoxEdit1.SelectedIndex)
    End Sub
    Sub PopolaRegime(ByVal i As Int16)
        ComboBoxEdit1.SelectedIndex = i
        Str = "Select distinct * from TbRegIva where RIvaTipo <> 9 and RIvaAnno=" & Val(ComboBoxEdit1.EditValue)
        Dim DsIvaP = New DataTable
        Dim DaIvaP = New SqlDataAdapter(Str, cnCo)
        Dim j As Int16 = -1
        DaIvaP.Fill(DsIvaP)
        ImageComboBoxEdit3.Properties.Items.Clear()
        For x = 1 To DsIvaP.Rows.Count
            RwReg = DsIvaP.Rows(x - 1)
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(RwReg("RivaNreg").ToString.PadLeft(2, "0") & " " & RwReg("RivaDesc"), RwReg("RivaNreg").ToString.PadLeft(2, "0"), -1)
            ImageComboBoxEdit3.Properties.Items.Add(nn)
            If Reg = RwReg("RivaNreg") Then j = x - 1
        Next
        REM x SCATENARE L'EVENTO SELECTEDINDEXCHANGED
        ImageComboBoxEdit3.SelectedIndex = -1 : ImageComboBoxEdit3.SelectedIndex = j
    End Sub

    Private Sub PopolaGrid2(ByVal RegIva As Int16)
        Dim XStr, XFilter As String
        Dim xx() As DataRow
        If Annuale = True Then
            XStr = "Select ivapregiva,ivapcodiva,ciides,sum(ivapimpon) as ivapimpon, sum(ivapivade) as ivapivade,sum(ivapivand) as ivapivand,sum(ivapimpmerce) as ivapimpmerce,'' as Totale from VCiiIvaP where IvaPRegIva=" & RegIva & " and IvaPAnno=" & Val(ComboBoxEdit1.EditValue) & " group by ivapregiva,ivapcodiva,ciides order by ivapregiva,ivapcodiva,ciides"
            XFilter = "IvaPRegIva = " & RegIva & " and IvaPAnno=" & Val(ComboBoxEdit1.EditValue)
            xx = TbTotA.Select(XFilter)
        Else
            XStr = "Select ivapregiva,ivapcodiva,ciides,ivapimpon,ivapivade,ivapivand,ivapimpmerce,'' as Totale from VCiiIvaP where IvaPRegIva=" & RegIva & " and IvaPAnno=" & Val(ComboBoxEdit1.EditValue) & " and IvaPMese=" & CDate("01" & ComboBoxEdit2.EditValue & Val(ComboBoxEdit1.EditValue)).Month & " order by ivapregiva,ivapcodiva,ciides"
            XFilter = "IvaPRegIva = " & RegIva & " and IvaPAnno=" & Val(ComboBoxEdit1.EditValue) & " and IvaPMese=" & CDate("01" & ComboBoxEdit2.EditValue & Val(ComboBoxEdit1.EditValue)).Month
            xx = TbTot.Select(XFilter)
        End If

        Dim str As String = XStr

        Dim DsIvaP As New DataSet
        Dim DaIvaP As New SqlDataAdapter(str, cnCo)
        DaIvaP.Fill(DsIvaP, "TbIvaP")

        RwTes = DsIvaP.Tables("TbIvaP").NewRow
        RwTes("IvaPRegIva") = DBNull.Value
        RwTes("IvaPCodIva") = DBNull.Value
        RwTes("CiiDes") = "Totali"
        If xx.Length = 0 Then
            RwTes("IvaPImpon") = 0
            RwTes("IvaPIvaDE") = 0
            RwTes("IvaPIvaND") = 0
            RwTes("IvaPImpMerce") = 0
            RwTes("Totale") = 0
        Else
            RwTes("IvaPImpon") = xx(0).ItemArray(3)
            RwTes("IvaPIvaDE") = xx(0).ItemArray(4)
            RwTes("IvaPIvaND") = xx(0).ItemArray(5)
            RwTes("IvaPImpMerce") = xx(0).ItemArray(6)
            RwTes("Totale") = CDec(xx(0).ItemArray(3) + xx(0).ItemArray(4) + xx(0).ItemArray(6)).ToString("#,###,###,##0.00")
        End If

        DsIvaP.Tables("TbIvaP").Rows.Add(RwTes)

        GridControl1.DataSource = DsIvaP.Tables("TbIvaP")
        GridControl1.Refresh()
        GridView1.ClearSelection()

        GridView1.SelectRow(DsIvaP.Tables("TbIvaP").Rows.Count - 1)
        GridView1.FocusedRowHandle = DsIvaP.Tables("TbIvaP").Rows.Count - 1
        If Annuale = False Then VerificaChiusura() Else CheckEdit1.Checked = False
    End Sub

    Private Sub VerificaChiusura()
        Str = "Select IvaVChiusura from TbVers where IvaVAnno=" & Val(ComboBoxEdit1.EditValue) & " and IvaVMese=" & CDate("01" & ComboBoxEdit2.EditValue & Val(ComboBoxEdit1.EditValue)).Month
        Dim cmd As New SqlCommand(Str, cnCo)
        Dim Valore As Boolean = cmd.ExecuteScalar

        If Valore = True Then CheckEdit1.Checked = True Else CheckEdit1.Checked = False
    End Sub

    Private Sub ImageComboBoxEdit3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit3.SelectedIndexChanged, ComboBoxEdit2.SelectedIndexChanged
        If ComboBoxEdit2.SelectedIndex = 12 Then Annuale = True Else Annuale = False REM '' DA 0 A 11 MESE 12 ANNUALE
        Reg = ImageComboBoxEdit3.EditValue
        PopolaGrid2(ImageComboBoxEdit3.EditValue)
    End Sub

    Private Sub CorIva_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        PopolaRegime(ComboBoxEdit1.SelectedIndex)
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Cursor.Current = Cursors.WaitCursor
        Dim frm As New LpLp
        Rpt = New ReportClass
        Rpt1 = New StIvaAnn
        Rpt2 = New StaTotIva
        If Annuale = True Then
            Rpt = Rpt1
        Else
            Rpt = Rpt2
        End If
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("Anno", Val(ComboBoxEdit1.EditValue))
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
End Class