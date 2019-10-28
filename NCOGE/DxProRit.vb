Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports DevExpress.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports NPRINT
Imports DevExpress.XtraEditors

Public Class DxProRit
    Private Shared AnaCod, AnaDesc, DatDoc As String
    Private Shared NumDoc As Int32
    Private Shared FTImport, FTIva As Decimal
    Public Shared Property PDatDoc() As String
        Get
            Return DatDoc
        End Get
        Set(ByVal Value As String)
            DatDoc = Value
        End Set
    End Property
    Public Shared Property PAnaCod() As String
        Get
            Return AnaCod
        End Get
        Set(ByVal Value As String)
            AnaCod = Value
        End Set
    End Property
    Public Shared Property PAnaDesc() As String
        Get
            Return AnaDesc
        End Get
        Set(ByVal Value As String)
            AnaDesc = Value
        End Set
    End Property
    Public Shared Property PImport() As Decimal
        Get
            Return FTImport
        End Get
        Set(ByVal Value As Decimal)
            FTImport = Value
        End Set
    End Property
    Public Shared Property PIva() As Decimal
        Get
            Return FTIva
        End Get
        Set(ByVal Value As Decimal)
            FTIva = Value
        End Set
    End Property
    Public Shared Property PNumDoc() As Int32
        Get
            Return NumDoc
        End Get
        Set(ByVal Value As Int32)
            NumDoc = Value
        End Set
    End Property

    Dim DsPer As DataTable
    Dim DaPer As SqlDataAdapter
    Dim RwPer As DataRow

    Dim DsFtt As DataTable
    Dim DaFtt As SqlDataAdapter
    Dim RwFtt As DataRow

    Dim H8 As DataRow

    Dim EsisteRit As Boolean
    Dim RifId, Iset2, Pagina As Int32

    REM Region Quietanze
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




    Private Sub DxProRit_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Pulizia(True)
        PopolaPerc()
        If NumDoc > 0 Then RicercaInDsRit()
    End Sub
#Region "Dati Compensi"
    Sub RicercaInDsRit()
        TextEdit1.EditValue = AnaCod
        TextEdit2.EditValue = AnaDesc
        TextEdit3.EditValue = NumDoc
        DateEdit1.EditValue = CDate(DatDoc)
        TextEdit7.EditValue = FTImport
        TextEdit15.EditValue = FTIva
        CALCOLANETTO()
        CALCOLAPAGATO()
        TextEdit4.Focus()
        REM FASE 1 Cerco il Percipiente
        For X As Int16 = 1 To DsPer.Rows.Count
            H8 = DsPer.Rows(X - 1)
            If H8("RitcodFor") = TextEdit1.EditValue Then iset = X - 1 : Exit For
        Next
        If iset = -1 Then Exit Sub
        RwPer = GridView1.GetDataRow(iset)
        PopolaFatt()
        GridView1.FocusedRowHandle = iset
        GridView1.SelectRow(iset)
        TextEdit7.EditValue = FTImport
        TextEdit15.EditValue = FTIva
        CALCOLANETTO()
        CALCOLAPAGATO()
        TextEdit4.Focus()
        REM FASE 2 Cerco Eventuale Fattura
        TextEdit3.EditValue = NumDoc
        DateEdit1.EditValue = CDate(DatDoc)
        For X As Int16 = 1 To DsFtt.Rows.Count
            H8 = DsFtt.Rows(X - 1)
            If H8("RitCodFor") = TextEdit1.EditValue And H8("RitProtFat") = TextEdit3.EditValue And H8("RitDataFat") = DateEdit1.EditValue Then Iset2 = X - 1 : Exit For
        Next
        If Iset2 = -1 Then Exit Sub
        RwFtt = GridView2.GetDataRow(Iset2)
        GridView2.FocusedRowHandle = Iset2
        GridView2.SelectRow(Iset2)
        RifId = RwFtt("RitNum")
        CaricaRit(RifId)
        TextEdit4.Focus()
    End Sub
    Sub Pulizia(ByVal n As Boolean)
        TextEdit1.EditValue = "00000"
        TextEdit2.EditValue = ""
        TextEdit3.EditValue = 0
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = ""
        TextEdit6.EditValue = CDec(0.0)
        TextEdit7.EditValue = CDec(0.0)
        TextEdit8.EditValue = CDec(0.0)
        TextEdit9.EditValue = CDec(0.0)
        TextEdit10.EditValue = CDec(0.0)
        TextEdit11.EditValue = CDec(0.0)
        TextEdit12.EditValue = CDec(0.0)
        TextEdit13.EditValue = CDec(0.0)
        TextEdit14.EditValue = CDec(0.0)
        TextEdit15.EditValue = CDec(0.0)
        TextEdit16.EditValue = CDec(0.0)
        TextEdit17.EditValue = CDec(0.0)
        TextEdit18.EditValue = ""
        TextEdit19.EditValue = CDec(0.0)
        DateEdit1.EditValue = Nothing
        DateEdit2.EditValue = Nothing
        DateEdit3.EditValue = Nothing
        DateEdit4.EditValue = Nothing
        DateEdit5.EditValue = Nothing
        ImageComboBoxEdit1.SelectedIndex = -1
        RifId = -1
        If n = True Then
            Iset2 = -1 : iset = -1
            DsFtt = New DataTable
            GridControl2.DataSource = DsFtt
        End If
    End Sub
    Private Sub PopolaPerc()
        Dim Str As String = "select distinct RitcodFor,AnaDesc from VRitTrib order by AnaDesc"
        DsPer = New DataTable()
        DaPer = New SqlDataAdapter(Str, cnCo)
        DaPer.Fill(DsPer)
        GridControl1.DataSource = DsPer
        GridControl1.Refresh()
        GridView1.ClearSelection()
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
            RwPer = GridView1.GetDataRow(iset)
            PopolaFatt()
        End If
    End Sub
    Private Sub PopolaFatt()
        Dim Str As String = "select * from VRitTrib where RitCodFor='" & RwPer("RitcodFor") & "' order by RitDataFat desc"
        DsFtt = New DataTable()
        DaFtt = New SqlDataAdapter(Str, cnCo)
        DaFtt.Fill(DsFtt)
        GridControl2.DataSource = DsFtt
        GridControl2.Refresh()
        GridView2.ClearSelection()
        Pulizia(False)
        TextEdit1.EditValue = RwPer("RitCodFor")
        TextEdit2.EditValue = RwPer("AnaDesc")
        DateEdit1.Focus()
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
            RwFtt = GridView2.GetDataRow(Iset2)
            RifId = RwFtt("RitNum")
            CaricaRit(RifId)
        End If
    End Sub
    Private Function CaricaRit(ByVal RitId As Int32) As Boolean
        EsisteRit = False
        Dim cmd As New SqlCommand("select * from VRitTrib where RitNum =" & RitId, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            CaricaElementi()
            EsisteRit = True
        End If
        dataRd.Close()
    End Function

    Private Sub CaricaElementi()
        TextEdit1.EditValue = dataRd.Item("RitCodFor")
        TextEdit2.EditValue = IIf(dataRd.Item("AnaDesc") Is DBNull.Value, "", dataRd.Item("AnaDesc"))
        DateEdit1.EditValue = dataRd.Item("RitDataFat")
        TextEdit3.EditValue = dataRd.Item("RitProtFat")
        DateEdit2.EditValue = dataRd.Item("RitDataPag")
        TextEdit4.EditValue = dataRd.Item("RitTributo")
        TextEdit5.EditValue = dataRd.Item("TribDesc")
        TextEdit6.EditValue = dataRd.Item("RitPerc")
        If CDec(TextEdit6.EditValue) = 0 Then TextEdit6.EditValue = dataRd.Item("TribRit")

        TextEdit7.EditValue = dataRd.Item("RitCompenso")
        TextEdit8.EditValue = dataRd.Item("RitRitenuta")
        TextEdit9.EditValue = dataRd.Item("RitPrevPerc")
        TextEdit10.EditValue = dataRd.Item("RitContrInps")

        TextEdit12.EditValue = dataRd.Item("RitRimborsi")
        TextEdit13.EditValue = dataRd.Item("RitRivalsa")
        TextEdit14.EditValue = dataRd.Item("RitRimbConv")
        TextEdit15.EditValue = dataRd.Item("RitIva")

        TextEdit17.EditValue = dataRd.Item("RitSospesa")
        TextEdit18.EditValue = dataRd.Item("RitNumQB")
        TextEdit19.EditValue = dataRd.Item("RitImpVersam")

        DateEdit3.EditValue = dataRd.Item("RitPerAtDa")
        DateEdit4.EditValue = dataRd.Item("RitPerAtAa")
        DateEdit5.EditValue = dataRd.Item("RitDataVer")
        ImageComboBoxEdit1.EditValue = dataRd.Item("RitEsCc")
        CALCOLANETTO()
        CALCOLAPAGATO()
    End Sub
    Sub CALCOLANETTO()
        TextEdit11.EditValue = TextEdit7.EditValue - TextEdit8.EditValue - TextEdit9.EditValue
    End Sub
    Sub CALCOLAPAGATO()
        TextEdit16.EditValue = TextEdit11.EditValue + TextEdit12.EditValue + TextEdit13.EditValue + TextEdit14.EditValue + TextEdit15.EditValue
    End Sub
    Private Sub TextEdit_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit7.Leave, TextEdit9.Leave, TextEdit10.Leave, TextEdit12.Leave, TextEdit13.Leave, TextEdit14.Leave, TextEdit15.Leave
        If DirectCast(sender, TextEdit).Name = "TextEdit7" Then
            TextEdit8.EditValue = CDec(TextEdit7.EditValue) * CDec(TextEdit6.EditValue / 100)
        End If
        CALCOLANETTO()
        CALCOLAPAGATO()
    End Sub

    Private Sub TextEdit1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.Leave
        TextEdit2.EditValue = LeggiFornitore(TextEdit1.EditValue)
    End Sub
    Function LeggiFornitore(ByVal cod As String) As String
        Dim AnaDesc As String = ""
        Dim cmd As New SqlCommand("select * from TbAna where AnaGrp ='FO' and AnaCod = '" & cod & "'", cnVd)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            AnaDesc = dataRd.Item("AnaDesc")
        End If
        dataRd.Close()
        Return AnaDesc
    End Function

    Private Sub TextEdit4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit4.Leave
        LeggiTributo()
    End Sub
    Sub LeggiTributo()
        Dim cmd As New SqlCommand("select * from TbTrib where TribCod ='" & TextEdit4.EditValue & "'", cnCo)
        Dim Trib As String = ""
        Dim Perc As Decimal = 0.0
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            Trib = dataRd.Item("TribDesc")
            Perc = dataRd.Item("TribRit")
        End If
        dataRd.Close()
        If CDec(TextEdit6.EditValue) = 0 Then TextEdit6.EditValue = CDec(Perc)
        TextEdit5.EditValue = Trib
    End Sub

    Private Sub GroupControl6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GroupControl6.Click
        Dim x As String = Query.CercaTrib("")
        If x = Nothing Then Exit Sub
        If x.Length > 0 Then TextEdit4.EditValue = x : LeggiTributo()
    End Sub

    Private Sub GroupControl4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GroupControl4.Click
        Dim frm As New RicercaClFo
        frm.StartPosition = FormStartPosition.CenterScreen
        frm.CliFor = "FO"
        frm.ShowDialog()
        Dim x As String = frm.Codice
        If x = Nothing Then Exit Sub
        If x.Length = 5 Then TextEdit1.EditValue = x : TextEdit2.EditValue = LeggiFornitore(TextEdit1.EditValue)
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If ControlloCampi() = False Then Return
        Registra()
    End Sub
    Sub Registra()
        Dim Str As String
        If EsisteRit = False Then
            Str = "Insert into TbRit (RitCodFor,RitCompenso,RitContrInps,RitDataFat,RitDataPag,RitDataVer,RitEsCc,RitImpVersam,RitIva,RitLetCont10,RitNumQB,RitPerAtAa,RitPerAtDa,RitPerc,RitPrevPerc,RitProtFat,RitRimbConv,RitRimborsi,RitRitenuta,RitRivalsa,RitSospesa,RitTributo) values (@RitCodFor,@RitCompenso,@RitContrInps,@RitDataFat,@RitDataPag,@RitDataVer,@RitEsCc,@RitImpVersam,@RitIva,@RitLetCont10,@RitNumQB,@RitPerAtAa,@RitPerAtDa,@RitPerc,@RitPrevPerc,@RitProtFat,@RitRimbConv,@RitRimborsi,@RitRitenuta,@RitRivalsa,@RitSospesa,@RitTributo)"
        Else
            Str = "Update TbRit set RitCodFor=@RitCodFor,RitCompenso=@RitCompenso,RitContrInps=@RitContrInps,RitDataFat=@RitDataFat,RitDataPag=@RitDataPag,RitDataVer=@RitDataVer,RitEsCc=@RitEsCc,RitImpVersam=@RitImpVersam,RitIva=@RitIva,RitLetCont10=@RitLetCont10,RitNumQB=@RitNumQB,RitPerAtAa=@RitPerAtAa,RitPerAtDa=@RitPerAtDa,RitPerc=@RitPerc,RitPrevPerc=@RitPrevPerc,RitProtFat=@RitProtFat,RitRimbConv=@RitRimbConv,RitRimborsi=@RitRimborsi,RitRitenuta=@RitRitenuta,RitRivalsa=@RitRivalsa,RitSospesa=@RitSospesa,RitTributo=@RitTributo where ritNum=" & RifId
        End If
        Dim cmd As New SqlCommand(Str, cnCo)
        Dim p2 As New SqlParameter("@RitCodFor", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@RitDataFat", SqlDbType.SmallDateTime)
        Dim p4 As New SqlParameter("@RitProtFat", SqlDbType.Int)
        Dim p5 As New SqlParameter("@RitDataPag", SqlDbType.SmallDateTime)
        Dim p6 As New SqlParameter("@RitTributo", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@RitCompenso", SqlDbType.Decimal)
        Dim p8 As New SqlParameter("@RitPerc", SqlDbType.Decimal)
        Dim p9 As New SqlParameter("@RitRitenuta", SqlDbType.Decimal)
        Dim p10 As New SqlParameter("@RitPrevPerc", SqlDbType.Decimal)
        Dim p11 As New SqlParameter("@RitRimborsi", SqlDbType.Decimal)
        Dim p12 As New SqlParameter("@RitRivalsa", SqlDbType.Decimal)
        Dim p13 As New SqlParameter("@RitRimbConv", SqlDbType.Decimal)
        Dim p14 As New SqlParameter("@RitIva", SqlDbType.Decimal)
        Dim p15 As New SqlParameter("@RitDataVer", SqlDbType.SmallDateTime)
        Dim p16 As New SqlParameter("@RitEsCc", SqlDbType.VarChar)
        Dim p17 As New SqlParameter("@RitNumQB", SqlDbType.VarChar)
        Dim p18 As New SqlParameter("@RitImpVersam", SqlDbType.Decimal)
        Dim p19 As New SqlParameter("@RitContrInps", SqlDbType.Decimal)
        Dim p20 As New SqlParameter("@RitLetCont10", SqlDbType.VarChar)
        Dim p21 As New SqlParameter("@RitSospesa", SqlDbType.Decimal)
        Dim p22 As New SqlParameter("@RitPerAtDa", SqlDbType.SmallDateTime)
        Dim p23 As New SqlParameter("@RitPerAtAa", SqlDbType.SmallDateTime)

        p2.Value = TextEdit1.EditValue
        p3.Value = DateEdit1.EditValue
        p4.Value = TextEdit3.EditValue
        If DateEdit2.EditValue Is Nothing Then p5.Value = DBNull.Value Else p5.Value = DateEdit2.EditValue
        p6.Value = TextEdit4.EditValue
        p7.Value = TextEdit7.EditValue
        p8.Value = TextEdit6.EditValue
        p9.Value = TextEdit8.EditValue
        p10.Value = TextEdit9.EditValue
        p11.Value = TextEdit12.EditValue
        p12.Value = TextEdit13.EditValue
        p13.Value = TextEdit14.EditValue
        p14.Value = TextEdit15.EditValue
        If DateEdit5.EditValue Is Nothing Then p15.Value = DBNull.Value Else p15.Value = DateEdit5.EditValue
        If ImageComboBoxEdit1.SelectedIndex = -1 Then p16.Value = "" Else p16.Value = ImageComboBoxEdit1.EditValue
        p17.Value = TextEdit18.EditValue
        p18.Value = TextEdit19.EditValue
        p19.Value = TextEdit10.EditValue
        p20.Value = ""
        p21.Value = TextEdit17.EditValue
        If DateEdit3.EditValue Is Nothing Then p22.Value = DBNull.Value Else p22.Value = DateEdit3.EditValue
        If DateEdit4.EditValue Is Nothing Then p23.Value = DBNull.Value Else p23.Value = DateEdit4.EditValue
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
        cmd.Parameters.Add(p14)
        cmd.Parameters.Add(p15)
        cmd.Parameters.Add(p16)
        cmd.Parameters.Add(p17)
        cmd.Parameters.Add(p18)
        cmd.Parameters.Add(p19)
        cmd.Parameters.Add(p20)
        cmd.Parameters.Add(p21)
        cmd.Parameters.Add(p22)
        cmd.Parameters.Add(p23)
        cmd.ExecuteNonQuery()
        ButtonF5.PerformClick()
    End Sub
    Function ControlloCampi() As Boolean
        Dim Errori As New ArrayList
        If Not TextEdit1.EditValue.ToString > "" Or Not TextEdit2.EditValue.ToString > "" Then
            Errori.Add("<> Inserire il Percipiente" & Chr(13))
        End If
        If Val(TextEdit3.EditValue) <= 0 Then
            Errori.Add("<> Inserire il NUMERO Fattura" & Chr(13))
        End If
        If DateEdit1.EditValue Is Nothing Then
            Errori.Add("<> Inserire la DATA Fattura" & Chr(13))
        End If
        If Not TextEdit4.EditValue.ToString > "" Or Not TextEdit5.EditValue.ToString > "" Then
            Errori.Add("<> Inserire il Tributo" & Chr(13))
        End If
        If CDec(TextEdit7.EditValue) = 0 Then
            Errori.Add("<> Inserire il compenso lordo" & Chr(13))
        End If
        If CDec(TextEdit8.EditValue) = 0 Then
            Errori.Add("<> Inserire la ritenuta" & Chr(13))
        End If
        If Errori.Count > 0 Then
            Dim Messaggio As String = "Si sono verificati i seguenti errori" & Chr(13) & Chr(13)
            For I As Int16 = 0 To Errori.Count - 1
                Messaggio &= Errori.Item(I)
            Next
            MessageBox.Show(Messaggio, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Errori.Clear()
            Return False
        End If
        Return True
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
    End Sub

    Private Sub ButtonF3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If RifId <= 0 Then Return
        DelRit()
    End Sub

    Private Sub DelRit()
        Dim box As Object
        box = MessageBox.Show("Vuoi eliminare la Ritenuta selezionata?", "ELIMINA RITENUTA", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If box = DialogResult.No Then
            Exit Sub
        End If
        Cmd = New SqlCommand("delete from TbRit where ritNum =" & RifId, cnCo)
        Cmd.ExecuteNonQuery()
        Pulizia(False)
        PopolaFatt()
    End Sub

    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Pulizia(True)
        PopolaPerc()
    End Sub
#End Region

#Region "Quietanze"
    Sub PaginaQuietanze()
        POlizia()
        PopolaGrid()
        DateEdit5x.Focus()
    End Sub
    Sub POlizia()
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

    Private Sub ButtonF11x_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11x.Click
        If Controlla() = False Then Exit Sub
        Aggiorna()
        ButtonF5x.PerformClick()
    End Sub
    Sub Aggiorna()
        For i As Int16 = 1 To GridView12.GetDetailView(UEX, 0).RowCount
            RwFat = GridView12.GetDetailView(UEX, 0).GetRow(i - 1)
            If RwFat("bool") = True Then RegistraQui()
        Next
    End Sub
    Sub RegistraQui()
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

    Private Sub ButtonF5x_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5x.Click
        PaginaQuietanze()
    End Sub
#End Region
#Region "GESTIONE FONDO PAGINA"
    Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            Pagina = XtraTabControl1.SelectedTabPageIndex
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            Pagina = XtraTabControl1.SelectedTabPageIndex
            PaginaQuietanze()
            Exit Sub
        End If
    End Sub
#End Region
End Class