Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports DevExpress.XtraEditors

Public Class DxInsCesp
    Dim TbCesp As DataTable
    Dim DaCesp As SqlDataAdapter
    Dim RwCesp As DataRow

    Dim TbQuo As DataTable
    Dim DaQuo As SqlDataAdapter
    Dim RwQuo As DataRow


    Dim CspGru, CspSpe1, CspSpe2 As String
    Dim Cespite, Storico, Variazione As Boolean
    Dim Iset As Integer = -1
    Dim Iset2 As Integer = -1
    Dim VProg As Integer = -1
    Dim TipoRic As Integer = -1
    Dim TC As Integer = -1
    Dim EseDal(), EseAl() As Date
    Dim EseAnno() As Int16
    Dim MaxEse As Integer = -1
    Dim Periodo As String = ""

    Private Sub DxInsCesp_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        CercaUtente()
        TextEdit1.Focus()
    End Sub

    Private Sub CercaUtente()
        Dim Str As String = "Select * from TbAzi where aziannolavoro=" & AnnoEsercizio()
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            CspGru = dataRd.Item("AziGruppoCesp")
            CspSpe1 = dataRd.Item("AziSpecieCesp")
            CspSpe2 = dataRd.Item("AziSottosCesp")
        End If
        dataRd.Close()
        Cmd = New SqlCommand("SELECT * from TbEse Order by EseAnno desc", cnCo)
        MaxEse = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MaxEse += 1
            ReDim Preserve EseAnno(MaxEse)
            ReDim Preserve EseDal(MaxEse)
            ReDim Preserve EseAl(MaxEse)
            EseAnno(MaxEse) = dataRd.Item("EseAnno")
            EseDal(MaxEse) = dataRd.Item("EseDal")
            EseAl(MaxEse) = dataRd.Item("EseAl")
        End While
        dataRd.Close()
    End Sub

    Sub Pulizia(ByVal n As Boolean)
        TextEdit2.EditValue = 0
        TextEdit3.EditValue = "00"
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = CDec(0.0)
        TextEdit6.EditValue = CDec(0.0)
        CheckEdit1.Checked = False
        TextEdit7.EditValue = ""
        DateEdit1.EditValue = Nothing
        TextEdit8.EditValue = 0
        TextEdit9.EditValue = CDec(0.0)
        TextEdit10.EditValue = 0
        DateEdit2.EditValue = Nothing
        TextEdit11.EditValue = ""
        TextEdit12.EditValue = "00.00"
        TextEdit13.EditValue = ""
        TextEdit14.EditValue = "00.00"
        TextEdit15.EditValue = ""
        TextEdit16.EditValue = "00.00"
        TextEdit17.EditValue = ""
        TextEdit18.EditValue = "00.00"
        Periodo = ""
        DateEdit1.ErrorText = ""
        PuliziaStorico(n)
        PuliziaVariazioni()
       
        GroupControl2.Enabled = False
        GroupControl3.Enabled = False
        GroupControl21.Enabled = True
        If n = True Then
            TextEdit1.EditValue = 0
            Cespite = False : Storico = False : Variazione = False
            TbQuo = New DataTable
            TbCesp = New DataTable
            GridControl1.DataSource = TbQuo
            GridControl2.DataSource = TbCesp
        End If
    End Sub
    Sub EnableGroup()
        GroupControl2.Enabled = True
        GroupControl3.Enabled = True
        GroupControl21.Enabled = False
    End Sub

    Private Sub TextEdit1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.Enter
        ButtonF8.Enabled = True : TipoRic = 1 : TC = -1
    End Sub

    Private Sub TextEdit1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.Leave
        ButtonF8.Enabled = False : TipoRic = -1
        LeggiCesp()
    End Sub
    Sub LeggiCesp()
        Cespite = False
        Dim x As Int32 = -1
        Dim Str As String = "select * from vcespcat where cspgru='" & CspGru & "' and cspspe1='" & CspSpe1 & "' and cspspe2='" & CspSpe2 & "' and CespNum=" & TextEdit1.EditValue
        Dim CMD As New SqlCommand(Str, cnCo)
        dataRd = CMD.ExecuteReader
        If dataRd.Read Then
            Cespite = True
            CaricaElementi()
        End If
        dataRd.Close()
        If Cespite = True Then
            PopolaGrid1()
            TextEdit11.EditValue = LeggiCpt(TextEdit12.EditValue)
            TextEdit13.EditValue = LeggiCpt(TextEdit14.EditValue)
            TextEdit15.EditValue = LeggiCpt(TextEdit16.EditValue)
            TextEdit17.EditValue = LeggiCpt(TextEdit18.EditValue)
        Else
            CMD = New SqlCommand("select isnull(max(CespNum),0) from TbCesp", cnCo)
            x = CMD.ExecuteScalar
            If TextEdit1.EditValue > x Or TextEdit1.EditValue = 0 Then TextEdit1.EditValue = x + 1
            Pulizia(False)
            TextEdit2.EditValue = Today.Year
        End If
    End Sub
    Sub CaricaElementi()
        TextEdit2.EditValue = dataRd.Item("CespAnnoa")
        TextEdit3.EditValue = dataRd.Item("CespCat")
        TextEdit4.EditValue = dataRd.Item("CspDesc")
        TextEdit6.EditValue = CDec(dataRd.Item("CespAliFis"))
        TextEdit5.EditValue = CDec(dataRd.Item("CespAliTab"))
        CheckEdit1.Checked = dataRd.Item("CspCp")

        TextEdit7.EditValue = dataRd.Item("CespDescr")
        TextEdit8.EditValue = dataRd.Item("CespProtFat")
        TextEdit12.EditValue = dataRd.Item("CespContoStorico")
        TextEdit14.EditValue = dataRd.Item("CespContoFondo")
        TextEdit16.EditValue = dataRd.Item("CespContoQuota")
        TextEdit18.EditValue = dataRd.Item("CespContoQuotaAnt")

        TextEdit9.EditValue = CDec(dataRd.Item("CespCostoStorico"))
        TextEdit10.EditValue = dataRd.Item("CespUltAnnoAmm")
        DateEdit1.EditValue = dataRd.Item("CespDataFat")
        DateEdit2.EditValue = dataRd.Item("CespDataCessione")
    End Sub
    Sub PopolaGrid1()
        Dim Str As String = "Select * from TbQuo where QuoNum=" & TextEdit1.EditValue & " order by QuoAnno"
        TbQuo = New DataTable()
        DaQuo = New SqlDataAdapter(Str, cnCo)
        DaQuo.Fill(TbQuo)
        GridControl1.DataSource = TbQuo
        AdvBandedGridView1.ClearSelection()
        Iset = TbQuo.Rows.Count - 1
    End Sub
    Sub DaMouseAdv()
        If Iset > -1 Then
            AdvBandedGridView1.SelectRow(Iset)
            AdvBandedGridView1.FocusedRowHandle = Iset
            RwQuo = AdvBandedGridView1.GetDataRow(Iset)
            TextEdit19.EditValue = RwQuo("QuoAnno")
            LeggiStorico(RwQuo("QuoNum"), TextEdit19.EditValue)
            LeggiVariazioni(RwQuo("QuoNum"), TextEdit19.EditValue)
            TextEdit20.Focus()
        Else
            TextEdit19.Focus()
        End If
    End Sub
    Private Sub LeggiStorico(ByVal Num As Integer, ByVal Anno As Integer)
        Dim Cmd As New SqlCommand("Select * from TbQuo where QuoNum=" & Num & " and QuoAnno=" & Anno, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            CaricaStorico()
            Storico = True
        Else
            PuliziaStorico(False)
            Storico = False
        End If
        dataRd.Close()
    End Sub
    Sub CaricaStorico()
        TextEdit20.EditValue = CDec(dataRd.Item("QuoCoStorIni"))
        TextEdit21.EditValue = CDec(dataRd.Item("QuoCoAmmIni"))
        TextEdit22.EditValue = CDec(dataRd.Item("QuoFondoIni"))
        TextEdit23.EditValue = CDec(dataRd.Item("QuoResiduoIni"))
        TextEdit24.EditValue = CDec(dataRd.Item("QuoNoDetraIni"))
        TextEdit25.EditValue = dataRd.Item("QuoTipoAmm")
        TextEdit26.EditValue = CDec(dataRd.Item("QuoAli"))
        TextEdit27.EditValue = CDec(dataRd.Item("QuoQuota"))
        TextEdit28.EditValue = CDec(dataRd.Item("QuoCoAmm"))
        TextEdit29.EditValue = CDec(dataRd.Item("QuoFondo"))
        TextEdit30.EditValue = CDec(dataRd.Item("QuoResiduo"))
        TextEdit31.EditValue = CDec(dataRd.Item("QuoNoDetra"))
    End Sub
    Sub PuliziaStorico(ByVal n As Boolean)
        TextEdit20.EditValue = CDec(0.0)
        TextEdit21.EditValue = CDec(0.0)
        TextEdit22.EditValue = CDec(0.0)
        TextEdit23.EditValue = CDec(0.0)
        TextEdit24.EditValue = CDec(0.0)
        TextEdit25.EditValue = 0
        TextEdit26.EditValue = CDec(0.0)
        TextEdit27.EditValue = CDec(0.0)
        TextEdit28.EditValue = CDec(0.0)
        TextEdit29.EditValue = CDec(0.0)
        TextEdit30.EditValue = CDec(0.0)
        TextEdit31.EditValue = CDec(0.0)
        If n = True Then TextEdit19.EditValue = 0
    End Sub
    Private Sub LeggiVariazioni(ByVal Num As Int16, ByVal Anno As Int16)
        Dim Str As String = "select * from VCesp where VCespNum=" & Num & " and VCespAnno=" & Anno & " order by cast(GgMm + '/' + cast(VCespAnno as varchar(4)) as smalldatetime), VCespProt"
        TbCesp = New DataTable
        DaCesp = New SqlDataAdapter(Str, cnCo)
        DaCesp.Fill(TbCesp)
        GridControl2.DataSource = TbCesp
        GridView2.ClearSelection()
        ImageComboBoxEdit1.SelectedIndex = 0
        TextEdit32.EditValue = TextEdit19.EditValue
        DateEdit3.EditValue = CDate("31/12/" & TextEdit32.EditValue)
        For I As Int16 = 0 To MaxEse
            If Anno = EseAnno(I) Then
                DateEdit3.EditValue = EseAl(I)
                Exit Sub
            End If
        Next
    End Sub

#Region "INPUT CAMPI A VIDEO"

    Private Sub DateEdit1_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DateEdit1.Validating
        DateEdit1.ErrorText = ""
        If DateEdit1.EditValue Is Nothing Then
            e.Cancel = True
            Exit Sub
        End If
        If DateEdit1.EditValue Is DBNull.Value Then
            e.Cancel = True
            Exit Sub
        End If
        If ControllaDataAcq(TextEdit2.EditValue) = False Then
            DateEdit1.ErrorText = Periodo
        End If
    End Sub
    Function ControllaDataAcq(anno As Int16) As Boolean
        ControllaDataAcq = True
        Periodo = ""
        If anno < EseAnno(MaxEse) Then
            If CDate(DateEdit1.EditValue).Year <> anno Then
                Periodo = "ANNO CESPITE " & anno & " <> DATA ACQUISTO " & CDate(DateEdit1.EditValue).ToShortDateString
                Return False : Exit Function
            Else
                Return True : Exit Function
            End If
        End If
        For I As Int16 = 0 To MaxEse
            If anno = EseAnno(I) Then
                Periodo = "ESERCIZIO " & EseDal(I).ToShortDateString & " - " & EseAl(I).ToShortDateString
                If CDate(DateEdit1.EditValue) < EseDal(I) Or CDate(DateEdit1.EditValue) > EseAl(I) Then
                    Return False : Exit Function
                Else
                    Return True : Exit Function
                End If
            End If
        Next
    End Function

    Private Sub TextEdit12_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit12.Enter, TextEdit14.Enter, TextEdit16.Enter, TextEdit18.Enter
        ButtonF8.Enabled = True : TipoRic = 3 : TC = CType(sender, TextEdit).Tag
    End Sub

    Private Sub TextEdit12_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit12.Leave, TextEdit14.Leave, TextEdit16.Leave, TextEdit18.Leave
        ButtonF8.Enabled = False : TipoRic = -1
        TextEdit11.EditValue = LeggiCpt(TextEdit12.EditValue)
        TextEdit13.EditValue = LeggiCpt(TextEdit14.EditValue)
        TextEdit15.EditValue = LeggiCpt(TextEdit16.EditValue)
        TextEdit17.EditValue = LeggiCpt(TextEdit18.EditValue)
    End Sub

    Private Sub TextEdit3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit3.Enter
        ButtonF8.Enabled = True : TipoRic = 2 : TC = -1
    End Sub

    Private Sub TextEdit3_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit3.Leave
        ButtonF8.Enabled = False : LeggiCat(TextEdit3.EditValue) : TipoRic = -1
    End Sub
    Private Sub LeggiCat(ByRef Cat As String)
        Dim Str As String = "select * from VCspCpt where cspgru='" & CspGru & "' and cspspe1='" & CspSpe1 & "' and cspspe2='" & CspSpe2 & "' and cspnum='" & Cat.Trim.PadLeft(2, "0") & "' and cspnum<>'00'"
        Dim cmd As New SqlCommand(Str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit3.EditValue = dataRd.Item("CspNum")
            TextEdit4.EditValue = dataRd.Item("CspDesc")
            TextEdit6.EditValue = CDec(dataRd.Item("CspPerc"))
            TextEdit5.EditValue = CDec(dataRd.Item("CspPerc"))
            CheckEdit1.Checked = CBool(dataRd.Item("CspCp"))
            If Cespite = False Then
                TextEdit12.EditValue = dataRd.Item("CspCespiti")
                TextEdit11.EditValue = dataRd.Item("CptCespiti")
                TextEdit14.EditValue = dataRd.Item("CspFondoAmm")
                TextEdit13.EditValue = dataRd.Item("CptFondoAmm")
                TextEdit16.EditValue = dataRd.Item("CspQuotaNormale")
                TextEdit15.EditValue = dataRd.Item("CptQuotaNormale")
                TextEdit18.EditValue = dataRd.Item("CspQuotaAnticipata")
                TextEdit17.EditValue = dataRd.Item("CptQuotaAnticipata")
            End If
        End If
        dataRd.Close()
    End Sub
    Private Sub TextEdit19_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextEdit19.Validating
        If Val(TextEdit19.EditValue) > 0 AndAlso Val(TextEdit19.EditValue) < Val(TextEdit2.EditValue) Then
            TextEdit19.ErrorText = "Anno < Anno di Esercizio/Acquisto del Cespite"
            e.Cancel = True
        End If

    End Sub
    Private Sub TextEdit19_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit19.Leave
        If AdvBandedGridView1.RowCount > 0 Then
            RwQuo = AdvBandedGridView1.GetDataRow(AdvBandedGridView1.RowCount - 1)
            If TextEdit19.EditValue > (RwQuo("QuoAnno") + 1) Then TextEdit19.Focus() : Exit Sub
        End If
        LeggiStorico(TextEdit1.EditValue, TextEdit19.EditValue)
        AdvBandedGridView1.ClearSelection()
        For x As Int16 = 1 To AdvBandedGridView1.RowCount
            RwQuo = AdvBandedGridView1.GetDataRow(x - 1)
            If RwQuo("QuoAnno") = TextEdit19.EditValue Then
                AdvBandedGridView1.SelectRow(x - 1)
                AdvBandedGridView1.FocusedRowHandle = (x - 1)
                Exit For
            End If
        Next
    End Sub
#End Region
#Region "XGRID"
    Private Sub AdvBandedGridView1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AdvBandedGridView1.MouseMove
        ShowHitInfo(AdvBandedGridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub

    Private Sub ShowHitInfo(ByVal hi As DevExpress.XtraGrid.Views.BandedGrid.ViewInfo.BandedGridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl2.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset = hi.RowHandle
    End Sub
    Private Sub AdvBandedGridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdvBandedGridView1.Click
        If Iset > -1 Then DaMouseAdv()
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
            RwCesp = GridView2.GetDataRow(Iset2)
            LeggiVar()
        End If
    End Sub
    Sub LeggiVar()
        Variazione = False : VProg = 0
        Dim cmd As New SqlCommand("Select *,substring(vcespggmm,1,2) + '/' + substring(vcespggmm,3,2) as GgMm from TbVCesp where VCespNum=" & RwCesp("VCespNum") & " and VCespAnno=" & RwCesp("VCespAnno") & " and VCespProg=" & RwCesp("VCespProg"), cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit32.EditValue = dataRd.Item("VCespAnno")
            DateEdit3.EditValue = CDate(dataRd.Item("GgMm") & "/" & TextEdit32.EditValue)
            TextEdit33.EditValue = dataRd.Item("VCespProt")
            ImageComboBoxEdit1.EditValue = dataRd.Item("VCespCaus").ToString
            TextEdit34.EditValue = CDec(dataRd.Item("VCespVariazioni"))
            TextEdit35.EditValue = CDec(dataRd.Item("VCespCessioni"))
            TextEdit36.EditValue = CDec(dataRd.Item("VCespPlusMinus"))
            TextEdit37.EditValue = dataRd.Item("vCespAnnota")
            Variazione = True
            VProg = dataRd.Item("VCespProg")
            DateEdit3.Focus()
        End If
        dataRd.Close()
    End Sub
    Private Sub ImageComboBoxEdit1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit1.SelectedValueChanged
        If ImageComboBoxEdit1.EditValue = 2 Then
            TextEdit34.EditValue = (CDec(TextEdit28.EditValue)) * -1
            TextEdit35.Enabled = True
            TextEdit36.Enabled = True
        Else
            TextEdit35.Enabled = False
            TextEdit36.Enabled = False
        End If
    End Sub
    Private Sub TextEdit35_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit35.Leave

        If ImageComboBoxEdit1.EditValue = 2 Then TextEdit36.EditValue = CDec(TextEdit35.EditValue) + CDec(TextEdit29.EditValue) + CDec(TextEdit34.EditValue)
    End Sub
#End Region

#Region "REGISTRA TABELLE"
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Lettura = True Then Exit Sub
        If Controllo() = False Then Exit Sub
        ScriviCesp()
        EnableGroup()
    End Sub
    Sub ScriviCesp()
        Dim str As String
        If Cespite = False Then
            str = "SET IDENTITY_INSERT TbCesp ON Insert into TbCesp (CespNum,CespAnnoA,CespCat,CespContoStorico,CespAliFis,CespDescr,CespDataFat,CespProtFat,CespCostoStorico,CespContoQuota,CespContoFondo,CespAliTab,CespDataCessione,CespUltAnnoAmm,CespCp,CespContoQuotaAnt) values  (@CespNum,@CespAnnoA,@CespCat,@CespContoStorico,@CespAliFis,@CespDescr,@CespDataFat,@CespProtFat,@CespCostoStorico,@CespContoQuota,@CespContoFondo,@CespAliTab,@CespDataCessione,@CespUltAnnoAmm,@CespCp,@CespContoQuotaAnt)"
        Else
            str = "Update TbCesp set CespAnnoA=@CespAnnoA,CespCat=@CespCat,CespContoStorico=@CespContoStorico,CespAliFis=@CespAliFis,CespDescr=@CespDescr,CespDataFat=@CespDataFat,CespProtFat=@CespProtFat,CespCostoStorico=@CespCostoStorico,CespContoQuota=@CespContoQuota,CespContoFondo=@CespContoFondo,CespAliTab=@CespAliTab,CespDataCessione=@CespDataCessione,CespUltAnnoAmm=@CespUltAnnoAmm,CespCp=@CespCp,CespContoQuotaAnt=@CespContoQuotaAnt where CespNum=" & TextEdit1.EditValue
        End If

        Dim cmd As New SqlCommand(str, cnCo)
        Dim p1 As New SqlParameter("@CespNum", SqlDbType.Int)
        Dim p2 As New SqlParameter("@CespAnnoA", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@CespCat", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@CespContoStorico", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@CespAliFis", SqlDbType.Decimal)
        Dim p6 As New SqlParameter("@CespDescr", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@CespDataFat", SqlDbType.SmallDateTime)
        Dim p9 As New SqlParameter("@CespProtFat", SqlDbType.Int)
        Dim p10 As New SqlParameter("@CespCostoStorico", SqlDbType.Decimal)
        Dim p11 As New SqlParameter("@CespContoQuota", SqlDbType.VarChar)
        Dim p12 As New SqlParameter("@CespContoFondo", SqlDbType.VarChar)
        Dim p12b As New SqlParameter("@CespContoQuotaAnt", SqlDbType.VarChar)
        Dim p13 As New SqlParameter("@CespAliTab", SqlDbType.Decimal)
        Dim p14 As New SqlParameter("@CespDataCessione", SqlDbType.SmallDateTime)
        Dim p15 As New SqlParameter("@CespUltAnnoAmm", SqlDbType.SmallInt)
        Dim p16 As New SqlParameter("@CespCp", SqlDbType.Bit)

        'DateEdit1.EditValue = CDate(CDate(DateEdit1.EditValue).Day & "/" & CDate(DateEdit1.EditValue).Month & "/" & Val(TextEdit2.EditValue))
        p1.Value = Val(TextEdit1.EditValue)
        p2.Value = Val(TextEdit2.EditValue)
        p3.Value = TextEdit3.EditValue.ToString.PadLeft(2, "0")
        p4.Value = IIf(Not TextEdit12.EditValue.ToString.Length > 0, "00.00", TextEdit12.EditValue)
        p5.Value = CDec(TextEdit6.EditValue)
        p6.Value = TextEdit7.EditValue
        p8.Value = DateEdit1.EditValue
        p9.Value = Val(TextEdit8.EditValue)
        p10.Value = TextEdit9.EditValue
        ' conto quote
        p11.Value = IIf(Not TextEdit16.EditValue.ToString.Length > 0, "00.00", TextEdit16.EditValue)
        ' conto fondo
        p12.Value = IIf(Not TextEdit14.EditValue.ToString.Length > 0, "00.00", TextEdit14.EditValue)
        p12b.Value = IIf(Not TextEdit18.EditValue.ToString.Length > 0, "00.00", TextEdit18.EditValue)
        p13.Value = CDec(TextEdit5.EditValue)
        If DateEdit2.EditValue Is Nothing Then DateEdit2.EditValue = DBNull.Value
        p14.Value = DateEdit2.EditValue
        p15.Value = Val(TextEdit10.EditValue)
        p16.Value = CheckEdit1.Checked
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmd.Parameters.Add(p3)
        cmd.Parameters.Add(p4)
        cmd.Parameters.Add(p5)
        cmd.Parameters.Add(p6)
        cmd.Parameters.Add(p8)
        cmd.Parameters.Add(p9)
        cmd.Parameters.Add(p10)
        cmd.Parameters.Add(p11)
        cmd.Parameters.Add(p12)
        cmd.Parameters.Add(p12b)
        cmd.Parameters.Add(p13)
        cmd.Parameters.Add(p14)
        cmd.Parameters.Add(p15)
        cmd.Parameters.Add(p16)
        cmd.ExecuteNonQuery()
        If Cespite = False Then
            Dim Nc As Integer = TextEdit1.EditValue
            ScriviQuo()
            Pulizia(True)
            TextEdit1.EditValue = Nc
            LeggiCesp()
            ButtonF1.PerformClick()
        End If
    End Sub
    Private Sub ScriviQuo()
        Dim str As String = "Insert into TbQuo (QuoNum,QuoAnno,QuoCoStorIni,QuoCoAmmIni,QuoFondoIni,QuoResiduoIni,QuoNoDetraIni,QuoCoStor,QuoCoAmm,QuoTipoAmm,QuoAli,QuoQuota,QuoFondo,QuoResiduo,QuoNoDetra) values  (@QuoNum,@QuoAnno,@QuoCoStorIni,@QuoCoAmmIni,@QuoFondoIni,@QuoResiduoIni,@QuoNoDetraIni,@QuoCoStor,@QuoCoAmm,@QuoTipoAmm,@QuoAli,@QuoQuota,@QuoFondo,@QuoResiduo,@QuoNoDetra)"
        Dim cmd As New SqlCommand(str, cnCo)
        Dim p1 As New SqlParameter("@QuoNum", SqlDbType.Int)
        Dim p2 As New SqlParameter("@QuoAnno", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@QuoCoStorIni", SqlDbType.Decimal)
        Dim p4 As New SqlParameter("@QuoCoAmmIni", SqlDbType.Decimal)
        Dim p5 As New SqlParameter("@QuoFondoIni", SqlDbType.Decimal)
        Dim p6 As New SqlParameter("@QuoResiduoIni", SqlDbType.Decimal)
        Dim p7 As New SqlParameter("@QuoNoDetraIni", SqlDbType.Decimal)
        Dim p8 As New SqlParameter("@QuoCoStor", SqlDbType.Decimal)
        Dim p9 As New SqlParameter("@QuoCoAmm", SqlDbType.Decimal)
        Dim p10 As New SqlParameter("@QuoTipoAmm", SqlDbType.SmallInt)
        Dim p11 As New SqlParameter("@QuoAli", SqlDbType.Decimal)
        Dim p12 As New SqlParameter("@QuoQuota", SqlDbType.Decimal)
        Dim p13 As New SqlParameter("@QuoFondo", SqlDbType.Decimal)
        Dim p14 As New SqlParameter("@QuoResiduo", SqlDbType.Decimal)
        Dim p15 As New SqlParameter("@QuoNoDetra", SqlDbType.Decimal)

        Dim Id As New SqlCommand("Select @@Identity", cnCo)
        p1.Value = Id.ExecuteScalar
        p2.Value = Val(TextEdit2.EditValue)
        p3.Value = TextEdit9.EditValue
        p4.Value = TextEdit9.EditValue
        p5.Value = 0
        p6.Value = TextEdit9.EditValue
        p7.Value = 0
        p8.Value = TextEdit9.EditValue
        p9.Value = TextEdit9.EditValue
        p10.Value = 0
        p11.Value = 0
        p12.Value = 0
        p13.Value = 0
        p14.Value = TextEdit9.EditValue
        p15.Value = 0

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
        cmd.Parameters.Add(p14)
        cmd.Parameters.Add(p15)

        cmd.ExecuteNonQuery()
    End Sub

    Private Function Controllo() As Boolean
        Dim Text As String = ""
        If Not Val(TextEdit2.EditValue) > 1950 Then
            Text &= Chr(13) & "<> Manca Anno Acquisto"
        End If
        If DateEdit1.EditValue Is Nothing Then
            Text &= Chr(13) & "<> Manca Data Acquisto"
        ElseIf DateEdit1.EditValue Is DBNull.Value Then
            Text &= Chr(13) & "<> Manca Data Acquisto"
        ElseIf ControllaDataAcq(TextEdit2.EditValue) = False Then
            Text &= Chr(13) & "<>" & Periodo
        End If
        If Not Val(TextEdit3.EditValue) > 0 Then
            Text &= Chr(13) & "<> Manca Categoria"
        End If
        If Not TextEdit7.EditValue.ToString.Length > 0 Then
            Text &= Chr(13) & "<> Manca la Descrizione del Cespite"
        End If
        If Not TextEdit11.EditValue.ToString.Length > 0 Then
            Text &= Chr(13) & "<> Manca Sottoconto Immobilizzazioni o Sottoconto non valido"
        End If
        If Not TextEdit15.EditValue.ToString.Length > 0 Then
            Text &= Chr(13) & "<> Manca Sottoconto Quote Ammortamento o Sottoconto non valido"
        End If
        If Not TextEdit13.EditValue.ToString.Length > 0 Then
            Text &= Chr(13) & "<> Manca Sottoconto Fondi o Sottoconto non valido"
        End If
        If Not TextEdit17.EditValue.ToString.Length > 0 Then
            Text &= Chr(13) & "<> Manca Sottoconto Quote Anticipate o Sottoconto non valido"
        End If
        If Not CDec(TextEdit9.EditValue) > 0 Then
            Text &= Chr(13) & "<> Inserire il Costo Storico"
        End If
        If Not Val(TextEdit8.EditValue) > 0 Then
            Text &= Chr(13) & "<> Inserire il Numero di Protocollo"
        End If
        If Text.Length > 0 Then
            MessageBox.Show("Si sono verificati i seguenti Errori:" & Chr(13) & Text, "Errore nel Salvataggio Cespite", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Controllo = False
            Exit Function
        End If
        Return True
    End Function
    Private Sub ButtonXF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF11.Click
        If TextEdit19.EditValue >= TextEdit2.EditValue And TextEdit19.EditValue <= Today.Year Then ScriviStorico()
        TextEdit19.Focus()
    End Sub
    Sub ScriviStorico()
        Dim Str As String
        If Storico = False Then
            Str = "Insert into TbQuo (QuoNum,QuoAnno,QuoCoStorIni,QuoCoAmmIni,QuoFondoIni,QuoResiduoIni,QuoNoDetraIni,QuoCoStor,QuoCoAmm,QuoTipoAmm,QuoAli,QuoQuota,QuoFondo,QuoResiduo,QuoNoDetra) values (@QuoNum,@QuoAnno,@QuoCoStorIni,@QuoCoAmmIni,@QuoFondoIni,@QuoResiduoIni,@QuoNoDetraIni,@QuoCoStor,@QuoCoAmm,@QuoTipoAmm,@QuoAli,@QuoQuota,@QuoFondo,@QuoResiduo,@QuoNoDetra)"
        Else
            Str = "Update TbQuo set QuoCoStorIni=@QuoCoStorIni,QuoCoAmmIni=@QuoCoAmmIni,QuoFondoIni=@QuoFondoIni,QuoResiduoIni=@QuoResiduoIni,QuoNoDetraIni=@QuoNoDetraIni,QuoCoStor=@QuoCoStor,QuoCoAmm=@QuoCoAmm,QuoTipoAmm=@QuoTipoAmm,QuoAli=@QuoAli,QuoQuota=@QuoQuota,QuoFondo=@QuoFondo,QuoResiduo=@QuoResiduo,QuoNoDetra=@QuoNoDetra where QuoNum=@QuoNum and QuoAnno=@QuoAnno"
        End If

        Dim cmd As New SqlCommand(Str, cnCo)

        Dim p1 As New SqlParameter("@QuoNum", SqlDbType.Int)
        Dim p2 As New SqlParameter("@QuoAnno", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@QuoCoStorIni", SqlDbType.Decimal)
        Dim p4 As New SqlParameter("@QuoCoAmmIni", SqlDbType.Decimal)
        Dim p5 As New SqlParameter("@QuoFondoIni", SqlDbType.Decimal)
        Dim p6 As New SqlParameter("@QuoResiduoIni", SqlDbType.Decimal)
        Dim p7 As New SqlParameter("@QuoNoDetraIni", SqlDbType.Decimal)
        Dim p8 As New SqlParameter("@QuoCoStor", SqlDbType.Decimal)
        Dim p9 As New SqlParameter("@QuoCoAmm", SqlDbType.Decimal)
        Dim p10 As New SqlParameter("@QuoTipoAmm", SqlDbType.SmallInt)
        Dim p11 As New SqlParameter("@QuoAli", SqlDbType.Decimal)
        Dim p12 As New SqlParameter("@QuoQuota", SqlDbType.Decimal)
        Dim p13 As New SqlParameter("@QuoFondo", SqlDbType.Decimal)
        Dim p14 As New SqlParameter("@QuoResiduo", SqlDbType.Decimal)
        Dim p15 As New SqlParameter("@QuoNoDetra", SqlDbType.Decimal)


        p1.Value = Val(TextEdit1.EditValue)
        p2.Value = TextEdit19.EditValue
        p3.Value = CDec(TextEdit20.EditValue)
        p4.Value = CDec(TextEdit21.EditValue)
        p5.Value = CDec(TextEdit22.EditValue)
        p6.Value = CDec(TextEdit23.EditValue)
        p7.Value = CDec(TextEdit24.EditValue)
        p8.Value = CDec(TextEdit9.EditValue)
        p9.Value = CDec(TextEdit28.EditValue)
        p10.Value = Val(TextEdit25.EditValue)
        p11.Value = CDec(TextEdit26.EditValue)
        p12.Value = CDec(TextEdit27.EditValue)
        p13.Value = CDec(TextEdit29.EditValue)
        p14.Value = CDec(TextEdit30.EditValue)
        p15.Value = CDec(TextEdit31.EditValue)

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
        cmd.Parameters.Add(p14)
        cmd.Parameters.Add(p15)
        cmd.ExecuteNonQuery()
        PopolaGrid1()
        PuliziaStorico(True)
    End Sub
    Private Sub ButtonXF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF3.Click
        If Lettura = True Then Exit Sub
        If Cespite = False Or Storico = False Then Exit Sub
        If GridView2.RowCount > 0 Then
            MessageBox.Show("Eliminare Prima Le Variazioni!!!!" & Chr(13), "Elimina Storico", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        REM INSERIRE CONTROLLI VARI
        If MessageBox.Show("Vuoi eliminare la riga selezionata?", "Elimina Storico", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            EliminaStorico(False)
        End If
    End Sub

    Private Sub EliminaStorico(ByVal c As Boolean)
        REM c = TRUE ELIMINA TUTTI GLI ANNI altrimenti solo l'anno interessato
        Dim Str As String = "Delete from TbQuo where QuoNum=" & TextEdit1.EditValue
        If c = True Then Str &= " and QuoAnno= " & TextEdit19.EditValue
        Dim Cmd As New SqlCommand("Delete from TbQuo where QuoNum=" & TextEdit1.EditValue & " and QuoAnno=" & CInt(TextEdit19.EditValue), cnCo)
        Cmd.ExecuteNonQuery()
        PopolaGrid1()
        PuliziaStorico(True)
    End Sub

    Private Sub ButtonF3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If Lettura = True Then Exit Sub
        If Cespite = False Then Return
        If MessageBox.Show("Vuoi  eliminare COMPLETAMENTE il Cespite " & TextEdit1.EditValue & " ?", "Elimina Cespite", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            EliminaCespite()
        End If
    End Sub

    Private Sub EliminaCespite()
        EliminaStorico(True)
        Dim Cmd As New SqlCommand("Delete from TbCesp where CespNum=" & TextEdit1.EditValue, cnCo)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("Delete from TbQuo where QuoNum=" & TextEdit1.EditValue, cnCo)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("Delete from TbVCesp where VCespNum=" & TextEdit1.EditValue, cnCo)
        Cmd.ExecuteNonQuery()
        Pulizia(True)
        TextEdit1.Focus()
    End Sub

    Private Sub ButtonYF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonYF11.Click
        If TextEdit32.EditValue <= 0 Then Exit Sub
        If ControllaVariazioni() = False Then Exit Sub
        ScriviVariazioni()
    End Sub
    Sub ScriviVariazioni()
        Dim Str As String
        If Variazione = False Then
            Str = "Insert into TbVCesp (VCespNum,VCespAnno,VCespProg,VCespGgMm,VCespProt,VCespCaus,VCespVariazioni,VCespCessioni,VCespPlusMinus,VCespAnnota) values (@VCespNum,@VCespAnno,@VCespProg,@VCespGgMm,@VCespProt,@VCespCaus,@VCespVariazioni,@VCespCessioni,@VCespPlusMinus,@VCespAnnota)"
        Else
            Str = "Update TbVCesp set VCespGgMm=@VCespGgMm,VCespProt=@VCespProt,VCespCaus=@VCespCaus,VCespVariazioni=@VCespVariazioni,VCespCessioni=@VCespCessioni,VCespPlusMinus=@VCespPlusMinus,VCespAnnota=@VCespAnnota where VCespNum=@VCespNum and VCespAnno=@VCespAnno and VCespProg=@VCespProg"
        End If

        Dim cmd As New SqlCommand(Str, cnCo)

        Dim p1 As New SqlParameter("@VCespNum", SqlDbType.Int)
        Dim p2 As New SqlParameter("@VCespAnno", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@VCespProg", SqlDbType.Int)
        Dim p4 As New SqlParameter("@VCespGgMm", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@VCespProt", SqlDbType.Int)
        Dim p6 As New SqlParameter("@VCespCaus", SqlDbType.SmallInt)
        Dim p7 As New SqlParameter("@VCespVariazioni", SqlDbType.Decimal)
        Dim p8 As New SqlParameter("@VCespCessioni", SqlDbType.Decimal)
        Dim p9 As New SqlParameter("@VCespPlusMinus", SqlDbType.Decimal)
        Dim p10 As New SqlParameter("@VCespAnnota", SqlDbType.VarChar)

        p1.Value = TextEdit1.EditValue
        p2.Value = TextEdit32.EditValue
        p3.Value = IIf(Variazione = False, LeggiMaxProgCorpo() + 1, VProg)
        p4.Value = CDate(DateEdit3.EditValue).Day.ToString.PadLeft(2, "0") & CDate(DateEdit3.EditValue).Month.ToString.PadLeft(2, "0")
        p5.Value = TextEdit33.EditValue
        p6.Value = ImageComboBoxEdit1.EditValue
        p7.Value = CDec(TextEdit34.EditValue)
        p8.Value = CDec(TextEdit35.EditValue)
        p9.Value = CDec(TextEdit36.EditValue)
        p10.Value = TextEdit37.EditValue

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

        cmd.ExecuteNonQuery()
        AggiornaCespite()
        AggiornaCespite(False, True)
        PuliziaVariazioni()
        LeggiVariazioni(TextEdit1.EditValue, TextEdit19.EditValue)
        DateEdit3.Focus()
    End Sub
    Sub PuliziaVariazioni()
        TextEdit32.EditValue = 0
        DateEdit3.EditValue = Nothing
        TextEdit33.EditValue = 0
        ImageComboBoxEdit1.SelectedIndex = -1
        TextEdit34.EditValue = CDec(0.0)
        TextEdit35.EditValue = CDec(0.0)
        TextEdit36.EditValue = CDec(0.0)
        TextEdit37.EditValue = ""
        Variazione = False
    End Sub
    Private Sub AggiornaCespite(Optional ByVal Canc As Boolean = False, Optional ByVal Ricalcola As Boolean = False)
        CALCOLARESIDUO()
        Dim Str As String
        If ImageComboBoxEdit1.SelectedIndex = 2 Then
            Str = "Update TbQuo with (tablock) set QuoFondo=isnull((SELECT SUM(VCESPVARIAZIONI) AS TOT FROM TBVCESP WHERE VCESPNUM=" & TextEdit1.EditValue & " AND VCESPANNO=" & TextEdit19.EditValue & " and VCespCaus=3 GROUP BY VCESPNUM,VCESPANNO),'0')+QuoQuota+QuoFondoIni,QuoResiduo=@Residuo where QuoNum=" & TextEdit1.EditValue & " and QuoAnno=" & TextEdit19.EditValue
        ElseIf ImageComboBoxEdit1.SelectedIndex = 0 Then
            Str = "Update TbQuo with (tablock) set QuoCoAmm=isnull((SELECT SUM(VCESPVARIAZIONI) AS TOT FROM TBVCESP WHERE VCESPNUM=" & TextEdit1.EditValue & " AND VCESPANNO=" & TextEdit19.EditValue & " and VCespCaus=1 GROUP BY VCESPNUM,VCESPANNO),'0')+QuoCoAmmIni, QuoResiduo=isnull((SELECT SUM(VCESPVARIAZIONI) AS TOT FROM TBVCESP WHERE VCESPNUM=" & TextEdit1.EditValue & " AND VCESPANNO=" & TextEdit19.EditValue & " and VCespCaus=1 GROUP BY VCESPNUM,VCESPANNO),'0')+QuoCoAmmIni-QuoFondo where QuoNum=" & TextEdit1.EditValue & " and QuoAnno=" & TextEdit19.EditValue
        Else
            Str = "Update TbQuo with (tablock) set QuoFondo=0,QuoQuota=0,QuoResiduo=0,QuoCoAmm=0 where QuoNum=" & TextEdit1.EditValue & " and QuoAnno=" & TextEdit19.EditValue
        End If

        If Ricalcola = True And Canc = True Then
            If ImageComboBoxEdit1.SelectedIndex = 1 Then
                Str = "Update TbQuo with (tablock) set QuoCoAmm=isnull((SELECT SUM(VCESPVARIAZIONI) AS TOT FROM TBVCESP WHERE VCESPNUM=" & TextEdit1.EditValue & " AND VCESPANNO=" & TextEdit19.EditValue & " and VCespCaus=1 GROUP BY VCESPNUM,VCESPANNO),'0')+QuoCoAmmIni, Quofondo=isnull((SELECT SUM(VCESPVARIAZIONI) AS TOT FROM TBVCESP WHERE VCESPNUM=" & TextEdit1.EditValue & " AND VCESPANNO=" & TextEdit19.EditValue & " and VCespCaus=1 GROUP BY VCESPNUM,VCESPANNO),'0')+QuoCoAmmIni-QuoFondo where QuoNum=" & TextEdit1.EditValue & " and QuoAnno=" & TextEdit19.EditValue
            Else
                Str = "Update TbQuo with (tablock) set QuoCoAmm=isnull((SELECT SUM(VCESPVARIAZIONI) AS TOT FROM TBVCESP WHERE VCESPNUM=" & TextEdit1.EditValue & " AND VCESPANNO=" & TextEdit19.EditValue & " and VCespCaus=1 GROUP BY VCESPNUM,VCESPANNO),'0')+QuoCoAmmIni, QuoResiduo=isnull((SELECT SUM(VCESPVARIAZIONI) AS TOT FROM TBVCESP WHERE VCESPNUM=" & TextEdit1.EditValue & " AND VCESPANNO=" & TextEdit19.EditValue & " and VCespCaus=1 GROUP BY VCESPNUM,VCESPANNO),'0')+QuoCoAmmIni-QuoFondo where QuoNum=" & TextEdit1.EditValue & " and QuoAnno=" & TextEdit19.EditValue

            End If
        End If

        Dim cmd As New SqlCommand(Str, cnCo)
        Dim p1b As New SqlParameter("@Residuo", CDec(TextEdit30.EditValue))
        cmd.Parameters.Add(p1b)
        cmd.ExecuteNonQuery()

        If ImageComboBoxEdit1.SelectedIndex = 1 Then
            Str = "Update TbCesp with (tablock) set CespDataCessione=@CespDataCessione where CespNum=" & TextEdit1.EditValue
            cmd = New SqlCommand(Str, cnCo)
            Dim p1 As New SqlParameter("@CespDataCessione", SqlDbType.SmallDateTime)
            p1.Value = CDate(DateEdit3.EditValue) '''.ToShortDateString
            If Canc = True Then p1.Value = DBNull.Value
            cmd.Parameters.Add(p1)

            cmd.ExecuteNonQuery()
            DateEdit2.EditValue = p1.Value
        End If
        PopolaGrid1()
        LeggiStorico(TextEdit1.EditValue, TextEdit19.EditValue)
    End Sub
    Private Sub CALCOLARESIDUO()
        TextEdit30.EditValue = TextEdit28.EditValue - TextEdit29.EditValue
    End Sub
    Private Function LeggiMaxProgCorpo() As Int16
        Cmd = New SqlCommand("Select max(VCespProg) from TbVCesp where vCespAnno =" & TextEdit32.EditValue & " and VCespNum = " & TextEdit1.EditValue, cnCo)
        Return IIf(Cmd.ExecuteScalar Is DBNull.Value, 0, Cmd.ExecuteScalar)
    End Function
    Function ControllaVariazioni() As Boolean
        If Variazione = True Then
            If Not DateEdit2.EditValue Is Nothing AndAlso Not DateEdit2.EditValue Is DBNull.Value AndAlso ImageComboBoxEdit1.EditValue <> 2 Then
                MessageBox.Show("Il Cespite risulta Ceduto! Annullare la Cessione prima di Inserire una nuova variazione!", "Nuova Variazione", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False : Exit Function
            End If
        End If
        If UltAnnoAmm() < TextEdit32.EditValue Then
            MessageBox.Show("Impossibile procedere con l'inserimento della Variazione!" & Chr(13) & "Non è stato effettuato il calcolo della quota di Ammortamento", "Inserimento Variazione", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False : Exit Function
        End If
        Dim Text As String = ""
        If Not TextEdit33.EditValue > 0 Then
            Text &= Chr(13) & "<> Inserire il Numero di Protocollo"
        End If
        If CDec(TextEdit34.EditValue) = 0 Then
            Text &= Chr(13) & "<> Manca l'importo della Variazione"
        End If
        If Text.Length > 0 Then
            MessageBox.Show("Si sono verificati i seguenti Errori:" & Chr(13) & Text, "Errore nel Salvataggio Variazione", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextEdit33.Focus()
            Return False : Exit Function
        End If
        Return True
    End Function
    Private Function UltAnnoAmm() As Int16
        Cmd = New SqlCommand("Select max(QuoAnno) from TbQuo where QuoNum=@Num", cnCo)
        Dim p1 As New SqlParameter("@Num", SqlDbType.Int)
        p1.Value = TextEdit1.EditValue
        Cmd.Parameters.Add(p1)
        UltAnnoAmm = Cmd.ExecuteScalar
    End Function
    Private Sub ButtonYF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonYF3.Click
        If Lettura = True Then Exit Sub
        If Cespite = False Or CDec(TextEdit34.EditValue) = 0 Then Exit Sub
        If MessageBox.Show("Vuoi eliminare la Variazione selezionata?", "Elimina Variazione", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            EliminaVariazione()
        End If
    End Sub

    Private Sub EliminaVariazione()
        REM c = TRUE ELIMINA TUTTI GLI ANNI altrimenti solo l'anno interessato
        Dim Str As String = "Delete from TbVCesp where VCespNum=" & TextEdit1.EditValue & " and VCespAnno=" & TextEdit19.EditValue & " and VCespProg=" & VProg
        Dim Cmd As New SqlCommand(Str, cnCo)
        Cmd.ExecuteNonQuery()
        AggiornaCespite(True)
        AggiornaCespite(IIf(ImageComboBoxEdit1.SelectedIndex = 1, True, False), True)
        PuliziaVariazioni()
        LeggiVariazioni(TextEdit1.EditValue, TextEdit19.EditValue)
        DateEdit3.Focus()
    End Sub

#End Region

    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 Then
            ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F1 Then
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            F8DATASTIERA()
            Exit Sub
        End If
    End Sub


    Sub F8DATASTIERA()
        If TipoRic = 1 Then
            CercaCespiti()
            SelectNextControl(TextEdit2, True, True, True, True)
        ElseIf TipoRic = 2 Then
            CercaCat()
            SelectNextControl(TextEdit4, True, True, True, True)
        ElseIf TipoRic = 3 Then
            Dim TextEditCpt As New TextEdit
            Dim TextEditOut As New TextEdit
            Select Case TC
                Case -1
                    Exit Sub
                Case 1
                    TextEditCpt = TextEdit12
                    TextEditOut = TextEdit11
                Case 2
                    TextEditCpt = TextEdit14
                    TextEditOut = TextEdit13
                Case 3
                    TextEditCpt = TextEdit16
                    TextEditOut = TextEdit15
                Case 4
                    TextEditCpt = TextEdit18
                    TextEditOut = TextEdit17
            End Select
            CercaCpt(TextEditCpt, TextEditOut)
            SelectNextControl(TextEditOut, True, True, True, True)
        End If
    End Sub
    Private Sub CercaCespiti()
        Dim Cod As String = Query.CercaCespiti(CspGru, CspSpe1, CspSpe2)
        If Cod.Trim = "" Then TextEdit1.Focus() : Exit Sub
        Dim Cesp As Object
        Cesp = Split(Cod)
        TextEdit1.EditValue = Cesp(0)
        TextEdit2.EditValue = Cesp(1)
        TextEdit3.EditValue = Cesp(2)
        LeggiCesp()
    End Sub
    Private Sub CercaCat()
        Dim Cod As String = Query.CercaCat(CspGru, CspSpe1, CspSpe2)
        If Cod.Trim = "" Then
            TextEdit3.Focus()
            Return
        End If
        TextEdit3.EditValue = Cod
        LeggiCat(Cod)
    End Sub

    Private Sub CercaCpt(ByVal TextEditCpt As TextEdit, ByVal TextEditOut As TextEdit)
        Dim Cod1 As String = Query.CercaPia()
        TextEditCpt.EditValue = IIf(Cod1 > "", Cod1, TextEditCpt.EditValue)
        TextEditOut.EditValue = LeggiCpt(TextEditCpt.EditValue)
    End Sub

    Private Sub ButtonF8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        MessageBox.Show("PREMERE IL TASTO F8 da TASTIERA", "F8 CLICK", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3)
    End Sub


    Private Sub TextEdit5_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextEdit5.Validating, TextEdit6.Validating
        If CType(sender, TextEdit).EditValue > 100 Then
            e.Cancel = True
        End If
    End Sub
End Class