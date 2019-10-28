Imports DXBASE
Imports NPRINT
Imports NCCOM
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Public Class DxVersam
    Dim TbVers As DataTable
    Dim DaVers As SqlDataAdapter
    Dim Str As String
    Dim Ultimo As Int16
    Dim Importo As Decimal
    Dim Esiste As Boolean
    Dim cmdCorr As SqlCommand
    Dim REPORT As New XtraReport
    Dim selectformula, SCRI, StrPrint As String

    Dim DsVersam As DataTable
    Dim DaVersam As SqlDataAdapter

    Dim RxW As DataRow

    Private Sub DxVersam_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        PopolaGrid()
        LeggiVers(Val(ComboBoxEdit1.EditValue), ComboBoxEdit2.SelectedIndex)
        ComboBoxEdit1.Focus()
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub PopolaGrid()
        Str = "select  'Periodo' =	case WHEN IvaVMese = 0 THEN 'ANNO PREC.'  WHEN IvaVMese = 13 THEN 'ANNUALE' WHEN IvaVMese = 14 THEN 'ACCONTO' else UPPER(datename(m,'01/'+cast(IvaVMese as varchar(2))+'/1900'))	END,* from tbvers where IvaVAnno=" & Val(ComboBoxEdit1.EditValue) & " order by IvaVMese"
        TbVers = New DataTable("TbVers")
        DaVers = New SqlDataAdapter(Str, cnCo)
        DaVers.Fill(TbVers)
        GridControl1.DataSource = TbVers
        GridView1.ClearSelection()
        Ultimo = -1
        If TbVers.Rows.Count > 0 Then
            Ultimo = TbVers.Rows(TbVers.Rows.Count - 1).Item("IvaVMese")
        End If
    End Sub

    Private Sub Pulizia(ByVal Tutto As Boolean)
        TextEdit7.EditValue = CDec(0.0)
        TextEdit8.EditValue = CDec(0.0)
        TextEdit20.EditValue = ""
        TextEdit1.EditValue = CDec(0.0)
        TextEdit2.EditValue = 0
        TextEdit3.EditValue = 0
        TextEdit4.EditValue = CDec(0.0)
        TextEdit5.EditValue = CDec(0.0)
        TextEdit6.EditValue = CDec(0.0)
        DateEdit1.EditValue = Today
        If Tutto = True Then
            Str = "select AziAnnoLavoro from tbazi order by aziannolavoro desc"
            Dim cmd As New SqlCommand(Str, cnCo)
            dataRd = cmd.ExecuteReader
            ComboBoxEdit1.Properties.Items.Clear()
            While dataRd.Read
                ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
            End While
            dataRd.Close()
            ComboBoxEdit1.SelectedIndex = 0
            ComboBoxEdit2.SelectedIndex = 0
        End If
    End Sub

    Private Sub Numbox2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit2.LostFocus, TextEdit3.LostFocus
        If Val(TextEdit2.EditValue) = 0 Then
            TextEdit3.EditValue = 0
        End If
        TextEdit20.EditValue = LeggiAbiCab(Val(TextEdit2.EditValue), Val(TextEdit3.EditValue))
        If Val(TextEdit2.EditValue) > 0 And Val(TextEdit3.EditValue) > 0 And Not TextEdit20.Text > "" Then
            TextEdit2.EditValue = 0
            TextEdit3.EditValue = 0
            TextEdit20.EditValue = ""
        End If
    End Sub

    Private Function LeggiAbiCab(ByVal Abi As Int32, ByVal Cab As Int32) As String
        LeggiAbiCab = ""
        Cmd = New SqlCommand("select * from TbCab where CaAbi = " & Abi & " and CaCab =" & Cab, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            LeggiAbiCab = dataRd.Item("CaDescFt")
        End If
        dataRd.Close()
    End Function
    Sub HyperLinkEdit1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkEdit1.Click
        Dim rabibanca As abibanca
        rabibanca = Ricerche.LnkAppoggio(Val(TextEdit2.EditValue), Val(TextEdit3.EditValue))
        If rabibanca.abi <> 0 And rabibanca.cab <> 0 Then
            TextEdit2.EditValue = rabibanca.abi
            TextEdit3.EditValue = rabibanca.cab
            TextEdit20.EditValue = LeggiAbiCab(Val(TextEdit2.EditValue), Val(TextEdit3.EditValue))
        End If
    End Sub
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        PopolaGrid()
        LeggiVers(Val(ComboBoxEdit1.EditValue), ComboBoxEdit2.SelectedIndex)
    End Sub

    Private Sub ComboBoxEdit2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEdit2.SelectedIndexChanged
        If Not Ultimo = -1 Then GridView1.ClearSelection()
        LeggiVers(Val(ComboBoxEdit1.EditValue), ComboBoxEdit2.SelectedIndex)
    End Sub

    Private Sub LeggiVers(ByVal Anno As Int16, ByVal Mese As Int16)
        Dim cmd As New SqlCommand("Select * from TbVers where IvaVAnno=" & Anno & " and IvaVMese=" & Mese, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            For I As Int16 = 0 To TbVers.Rows.Count - 1
                RxW = TbVers.Rows(I)
                Try
                    If RxW("IvaVMese") = Mese Then iset = I : GridView1.SelectRow(iset)
                Catch ex As Exception

                End Try
            Next
            If Not Mese = Ultimo Then ButtonF3.Enabled = False Else ButtonF3.Enabled = True
            Importo = dataRd.Item("IvaVVersam")
            TextEdit7.EditValue = CDec(dataRd.Item("IvaVVersam"))
            TextEdit8.EditValue = CDec(dataRd.Item("IvaVLiquida"))
            TextEdit1.EditValue = CDec(dataRd.Item("IvaVCrImUt"))
            TextEdit2.EditValue = dataRd.Item("IvaVAbi")
            TextEdit3.EditValue = dataRd.Item("IvaVCab")
            TextEdit4.EditValue = CDec(dataRd.Item("IvaVInteressi"))
            TextEdit5.EditValue = CDec(dataRd.Item("IvaVImpDaVers"))
            TextEdit6.EditValue = CDec(dataRd.Item("IvaVVersato"))
            If dataRd.Item("IvaVData") Is DBNull.Value Then
                DateEdit1.EditValue = Nothing
            Else
                DateEdit1.EditValue = CDate(dataRd.Item("IvaVData"))
            End If
            CheckEdit2.Checked = dataRd.Item("IvaVDelega")
            CheckEdit1.Checked = dataRd.Item("IvaVChiusura")
            dataRd.Close()
            TextEdit20.EditValue = LeggiAbiCab(Val(TextEdit2.EditValue), Val(TextEdit3.EditValue))
            Esiste = True
            Return
        Else
            Esiste = False
            Pulizia(False)
        End If
        dataRd.Close()
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
            RxW = GridView1.GetDataRow(iset)
            ComboBoxEdit1.EditValue = RxW("IvaVAnno")
            ComboBoxEdit2.SelectedIndex = RxW("IvaVMese")
            LeggiVers(Val(ComboBoxEdit1.EditValue), ComboBoxEdit2.SelectedIndex)
            ComboBoxEdit2.Focus()
        End If
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If ComboBoxEdit2.SelectedIndex = 14 Then CheckEdit1.Checked = False
        ScriviVers()
    End Sub

    Private Sub ScriviVers()
        If Esiste = True Then
            Str = "Update TbVers set IvaVVersam=@IvaVVersam,IvaVLiquida=@IvaVLiquida,IvaVData=@IvaVData,IvaVAbi=@IvaVAbi,IvaVCab=@IvaVCab,IvaVDelega=@IvaVDelega,IvaVChiusura=@IvaVChiusura,IvaVCrImUt=@IvaVCrImUt,IvaVInteressi=@IvaVInteressi,IvaVImpDaVers=@IvaVImpDaVers,IvaVVersato=@IvaVVersato where IvaVanno=@IvaVAnno and IvaVMese=@IvaVMese"
        Else
            Str = "Insert into TbVers (IvaVanno,IvaVMese,IvaVVersam,IvaVLiquida,IvaVData,IvaVAbi,IvaVCab,IvaVDelega,IvaVChiusura,IvaVCrImUt,IvaVInteressi,IvaVImpDaVers,IvaVVersato) values (@IvaVAnno,@IvaVMese,@IvaVVersam,@IvaVLiquida,@IvaVData,@IvaVAbi,@IvaVCab,@IvaVDelega,@IvaVChiusura,@IvaVCrImUt,@IvaVInteressi,@IvaVImpDaVers,@IvaVVersato)"
        End If

        Dim cmd As New SqlCommand(Str, cnCo)

        Dim p1 As New SqlParameter("@IvaVAnno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@IvaVMese", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@IvaVVersam", SqlDbType.Decimal)
        Dim p4 As New SqlParameter("@IvaVLiquida", SqlDbType.Decimal)
        Dim p5 As New SqlParameter("@IvaVData", SqlDbType.SmallDateTime)
        Dim p6 As New SqlParameter("@IvaVAbi", SqlDbType.Int)
        Dim p7 As New SqlParameter("@IvaVCab", SqlDbType.Int)
        Dim p8 As New SqlParameter("@IvaVDelega", SqlDbType.Bit)
        Dim p9 As New SqlParameter("@IvaVChiusura", SqlDbType.Bit)
        Dim p10 As New SqlParameter("@IvaVCrImUt", SqlDbType.Decimal)
        Dim p11 As New SqlParameter("@IvaVInteressi", SqlDbType.Decimal)
        Dim p12 As New SqlParameter("@IvaVImpDaVers", SqlDbType.Decimal)
        Dim p13 As New SqlParameter("@IvaVVersato", SqlDbType.Decimal)

        p1.Value = Val(ComboBoxEdit1.EditValue)
        p2.Value = ComboBoxEdit2.SelectedIndex
        p3.Value = TextEdit7.EditValue
        p4.Value = TextEdit8.EditValue
        p5.Value = IIf(DateEdit1.EditValue = Nothing, DBNull.Value, DateEdit1.EditValue)
        p6.Value = Val(TextEdit2.EditValue)
        p7.Value = Val(TextEdit3.EditValue)
        p8.Value = CheckEdit2.Checked
        p9.Value = CheckEdit1.Checked
        p10.Value = TextEdit1.EditValue
        p11.Value = TextEdit4.EditValue
        p12.Value = TextEdit5.EditValue
        p13.Value = TextEdit6.EditValue

        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmd.Parameters.Add(p3)
        cmd.Parameters.Add(p4)
        cmd.Parameters.Add(p5)
        cmd.Parameters.Add(p6)
        cmd.Parameters.Add(p7)
        cmd.Parameters.Add(p8)
        cmd.Parameters.Add(p9)
        cmd.Parameters.Add(p10)
        cmd.Parameters.Add(p11)
        cmd.Parameters.Add(p12)
        cmd.Parameters.Add(p13)

        cmd.ExecuteNonQuery()

        Dim AnnoX As Int16 = ComboBoxEdit1.SelectedIndex
        Dim PeriodoX As Int16 = ComboBoxEdit2.SelectedIndex
        Pulizia(True)
        ComboBoxEdit1.SelectedIndex = AnnoX
        ComboBoxEdit2.SelectedIndex = PeriodoX
        PopolaGrid()
        LeggiVers(Val(ComboBoxEdit1.EditValue), ComboBoxEdit2.SelectedIndex)
        ComboBoxEdit2.Focus()
    End Sub

    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click

        'Bisogna inserire ulteriori controlli!
        If Esiste = False Then Return
        If MessageBox.Show("Eliminare il Versamento di " & ComboBoxEdit2.Text & " del  " & Val(ComboBoxEdit1.EditValue) & "?" & ControllaCorrispettivi(Val(ComboBoxEdit1.EditValue), ComboBoxEdit2.SelectedIndex), "Elimina Versamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Return
        End If
        EliminaVers(Val(ComboBoxEdit1.EditValue), ComboBoxEdit2.SelectedIndex)
        ComboBoxEdit2.Focus()
    End Sub

    Private Sub EliminaVers(ByVal Anno As Int16, ByVal Mese As Int16)
        Dim cmd As New SqlCommand("Delete from TbVers where IvaVAnno=@IvaVAnno and IvaVMese=@IvaVMese", cnCo)
        Dim p1 As New SqlParameter("@IvaVAnno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@IvaVMese", SqlDbType.SmallInt)
        p1.Value = Anno
        p2.Value = Mese
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmd.ExecuteNonQuery()
        If Not cmdCorr Is Nothing Then cmdCorr.ExecuteNonQuery()
        Dim AnnoX As Int16 = ComboBoxEdit1.SelectedIndex
        Pulizia(True)
        ComboBoxEdit1.SelectedIndex = AnnoX
        ComboBoxEdit2.SelectedIndex = Mese - 1
        PopolaGrid()
        LeggiVers(Val(ComboBoxEdit1.EditValue), ComboBoxEdit2.SelectedIndex)
    End Sub

    Private Function ControllaCorrispettivi(ByVal Anno As Int16, ByVal Mese As Int16) As String
        Dim cmd As New SqlCommand("select count(*) from VIvaP where IvaPAnno=@IvaPAnno and IvaPMese=@IvaPMese", cnCo)
        Dim p1 As New SqlParameter("@IvaPAnno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@IvaPMese", SqlDbType.SmallInt)
        p1.Value = Anno
        p2.Value = Mese
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmdCorr = Nothing
        If cmd.ExecuteScalar() > 0 Then
            EsegueSql("EXEC XIvaP @IvaPAnno =" & Anno & ", @IvaPMese = " & Mese, cnCo)
            Return Chr(13) & Chr(13) & "ATTENZIONE: sono stati trovati Corrispettivi." & Chr(13) & " Procedere all'annullo delle Registrazioni di Scorporo IVA!"
            Exit Function
        End If
        Return ""
    End Function

    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 Then
            ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 Then
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        StrPrint = "Select * from DXVersam where IvaVAnno = " & ComboBoxEdit1.EditValue
        DsVersam = New DataTable
        DaVersam = New SqlDataAdapter(StrPrint, cnCo)
        DaVersam.SelectCommand.CommandTimeout = 300
        DaVersam.Fill(DsVersam)
        selectformula = ""
        REPORT = New DXStaVersam
        REPORT.DataSource = DsVersam
        REPORT.DataMember = "DsVersam"
        REPORT.FilterString = selectformula
        REPORT.Parameters("Titolo").Value = "Anno " & ComboBoxEdit1.EditValue & " - Dati e Importi delle Liquidazioni Periodiche"
        REPORT.Parameters("Marchio").Value = Marchio()
        REPORT.ShowPreview()
    End Sub

End Class