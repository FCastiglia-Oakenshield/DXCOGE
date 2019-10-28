Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.XtraReports
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native


Public Class BiEnneFat

    Dim TbCMG As DataTable
    Dim DaCMG As SqlDataAdapter
    Dim RwD As DataRowView

    Dim TbRighe As DataTable
    Dim DaRighe As SqlDataAdapter
    Dim RwX As DataRowView


    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem

    Dim Anno, CiRicavi, NReg, Nrighe, Segno, PagCod, TipoPag, Prog, TipoReg As Int16
    Dim CptRicavi, Cliente, TipoDoc As String
    Dim Rifer, RifCom, IsetR, IsetI As Int32
    Dim imp(73), iva(73), cor(73), CiiPerc(73) As Decimal
    Dim TImponibile, TIva, Imponibile, Valore, Totale As Decimal
    Dim PathPrg, PathSto, PathTmp As String

    Dim Documento, FL As Boolean

    Dim SCRI, StrPrint As String
    Dim DsFat As DataTable
    Dim DaFat As SqlDataAdapter

    Private Sub NFatNcr_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        LetturaTabelle()
        LeggiUltimeFatture()
        ButtonF5.PerformClick()
    End Sub
#Region "TESTATA"
    Sub LeggiUltimeFatture()
        Dim Str As String = "select * from vfatture where datepart(year,fatdata) = " & Anno & " order by fatdata desc,Fatnum desc"
        TbCMG = New DataTable()
        DaCMG = New SqlDataAdapter(Str, cnDb)
        DaCMG.Fill(TbCMG)
        GridControl1.DataSource = TbCMG
        GridView1.ClearSelection()
        GridView1.UnselectRow(0)
    End Sub
    Private Sub LetturaTabelle()
        Cmd = New SqlCommand("select top 1 * from TbTai order by TaiAnno desc", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Anno = dataRd.Item("TaiAnno")
            CiRicavi = dataRd.Item("Taiciv1")
            CptRicavi = dataRd.Item("Taicpt1")
        End If
        dataRd.Close()
        'Aree Lavoro
        Cmd = New SqlCommand(" SELECT * from TbSel where selId = 1", cnVd)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            PathPrg = dataRd.Item("sel12") & dataRd.Item("Sel1")
            PathSto = dataRd.Item("sel12") & dataRd.Item("sel11")
            PathTmp = dataRd.Item("sel8")
        End If
        dataRd.Close()

        'Lettura Codici Iva
        For x As Int16 = 0 To 73 : CiiPerc(x) = 0 : Next
        Dim Pi As Int16 = -1
        ImageComboBoxEdit3.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT * FROM TbCii where CiiDes > '' order by CiiCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("CiiCod") & " " & dataRd.Item("CiiDes").ToString, dataRd.Item("CiiCod"), dataRd.Item("CiiAli"))
            ImageComboBoxEdit3.Properties.Items.Add(nn)
            CiiPerc(dataRd.Item("CiiCod")) = dataRd.Item("CiiAli")
            If dataRd.Item("CiiCod") = CiRicavi Then Pi = ImageComboBoxEdit3.Properties.Items.Count - 1
        End While
        dataRd.Close()
        ImageComboBoxEdit3.SelectedIndex = Pi
        'Anni Lavoro
        ComboBoxEdit3.Properties.Items.Clear()
        Cmd = New SqlCommand("Select distinct RivaAnno from TbRegiva order by RivaAnno desc", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit3.Properties.Items.Add(dataRd.Item("RivaAnno"))
        End While
        dataRd.Close()
        ComboBoxEdit3.SelectedIndex = 0

        'Lettura banche azienda
        ComboBoxEdit1.Properties.Items.Clear()
        ComboBoxEdit1.Properties.Items.Add("")
        Cmd = New SqlCommand(" SELECT * from TbBan Order by BanCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("Bancod").ToString.PadLeft(3, " ") & " - " & dataRd.Item("BanDes"))
        End While
        dataRd.Close()
        ' lettura registri iva
        ImageComboBoxEdit12.Properties.Items.Clear()
        Cmd = New SqlCommand("Select * from TbRegIva where (RivaTipo = 1 or RivaTipo = 3) and RivaAnno =" & Anno, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("RivaNReg") = 1 Or dataRd.Item("RivaNReg") = 9 Then
                nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("RivaNReg").ToString("00") & " - " & dataRd.Item("RivaDesc").ToString, dataRd.Item("RivaNReg"), dataRd.Item("RivaTipo"))
                ImageComboBoxEdit12.Properties.Items.Add(nn)
            End If
        End While
        dataRd.Close()
        ImageComboBoxEdit12.SelectedIndex = 0
        'rem carico cpt da tbPia
        ImageComboBoxEdit13.Properties.Items.Clear()
        nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", "", -1)
        ImageComboBoxEdit13.Properties.Items.Add(nn)
        Cmd = New SqlCommand("select Distinct PiaCodCo,PiaAnaCo from TbPia where PiaFl01 = 7 Order by PiaCodCo", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("PiaCodCo") & " " & dataRd.Item("PiaAnaCo"), dataRd.Item("PiaCodCo"), -1)
            ImageComboBoxEdit13.Properties.Items.Add(nn)
        End While
        dataRd.Close()
        Dim UltCpt As String = "00.00"
        Cmd = New SqlCommand("select top 1 CorCnTrp from tbcor where CorCnTrp > '00.00' order by CorRif Desc", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UltCpt = dataRd.Item("CorCnTrp")
        End While
        dataRd.Close()
        ImageComboBoxEdit13.SelectedIndex = SettaComboImageString(ImageComboBoxEdit13, UltCpt)
    End Sub
    Sub PULIZIA(ByVal n As Int16)
        TextEdit1.Text = "" : TextEdit2.Text = "" : TextEdit3.Text = "" : TextEdit4.Text = "" : TextEdit5.Text = ""
        TextEdit6.Text = "" : TextEdit7.Text = "" : TextEdit8.Text = ""
        TextEdit11.Text = "" : TextEdit9.Text = "" : MemoEdit2.Text = ""
        DateEdit1.DateTime = Today
        TextEdit20.Text = "" : TextEdit19.Text = "" : TextEdit18.Text = "" : TextEdit22.Text = "" : TextEdit21.Text = ""
        TextEdit10.EditValue = CDec(0.0) : TextEdit16.EditValue = CDec(0.0) : TextEdit33.EditValue = CDec(0.0)
        TextEdit32.EditValue = CDec(0.0) : TextEdit24.Text = "" : TextEdit23.Text = "IT" : TextEdit15.EditValue = CDec(0.0)
        TipoReg = 1 : CheckButton2.Visible = False
        Rifer = -1 : Documento = False : RifCom = -1 : Cliente = "" : TipoDoc = "F" : TipoPag = -1 : IsetR = -1 : IsetI = -1 : iset = -1
        ComboBoxEdit1.SelectedIndex = -1 : Prog = -1 : FL = False
        REM ABILITA BOTTONI
        TextEdit1.Enabled = True : TbLeggiOrd.Enabled = True : DateEdit1.Enabled = True
        ImageComboBoxEdit12.Enabled = True : TextEdit2.Enabled = True
        ButtonF3.Enabled = False : ButtonF1.Enabled = True : ButtonF5.Enabled = True : ButtonF8.Enabled = True : ButtonPDF.Enabled = False
        REM AZZERA VARIABILI
        RIABILITAGROUPBOX(True)
    End Sub
    Sub RIABILITAGROUPBOX(ByVal ok As Boolean)
        GroupControl2.Enabled = ok : GroupControl4.Enabled = ok
        GroupControl6.Enabled = ok : GroupControl21.Enabled = ok
        GroupControl18.Enabled = ok : GroupControl11.Enabled = ok
        GroupControl25.Enabled = Not ok : GroupControl13.Enabled = Not ok
        GroupControl22.Enabled = Not ok : GroupControl15.Enabled = Not ok : GroupControl12.Enabled = Not ok
    End Sub
    Sub parzialeGROUPBOX(ByVal ok As Boolean)
        GroupControl25.Enabled = Not ok : GroupControl13.Enabled = Not ok
        GroupControl22.Enabled = ok
        GroupControl15.Enabled = ok
        If GroupControl22.Enabled = True Then
            GroupControl25.Enabled = True
            ButtonXF11.Enabled = False
            GroupControl14.Enabled = False
            GroupControl10.Enabled = False
            GroupControl40.Enabled = False
            GroupControl12.Enabled = False
            GroupControl9.Enabled = False
            GroupControl8.Enabled = False
            GroupControl36.Enabled = False
            GroupControl28.Enabled = False
            TextEdit14.Enabled = False
        Else
            ButtonXF11.Enabled = True
            GroupControl14.Enabled = True
            GroupControl10.Enabled = True
            GroupControl40.Enabled = True
            GroupControl9.Enabled = True
            GroupControl12.Enabled = True
            GroupControl8.Enabled = True
            GroupControl36.Enabled = True
            GroupControl28.Enabled = True
            TextEdit14.Enabled = True
        End If
    End Sub
    Sub TbLeggiOrd_ENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggiOrd.Enter
        If DateEdit1.EditorContainsFocus = True Then TextEdit1.Focus() : Exit Sub
        If Val(TextEdit1.Text) > 0 Then
            LeggiDocumento(Val(TextEdit1.Text))
            If Rifer > 0 Then
                CaricaDati()
                PopolaGridDett()
            Else
                NuovoDocumento()
            End If
        Else
            NuovoDocumento()
        End If
        DateEdit1.Focus()
    End Sub
    Private Sub CaricaDati()
        Dim CodBan As Int16 = 0
        Dim DescCom As String = ""
        Dim LetCom As Integer = -1
        Dim NReg As Int16 = 0
        Dim Abilita As Boolean = False
        Documento = False
        Cliente = "" : RifCom = -1
        Cmd = New SqlCommand("Select * from VTDocumenti where FatRif = " & Rifer, cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Cliente = dataRd.Item("FatCliFat")
            'Dati Documento
            TextEdit1.Text = dataRd.Item("FatNum")
            DateEdit1.EditValue = dataRd.Item("FatData")
            TextEdit11.Text = dataRd.Item("FatCliFat")
            'Dati anagrafica
            TextEdit2.Text = dataRd.Item("AnaRag1") & " " & dataRd.Item("AnaRag2")
            TextEdit4.Text = dataRd.Item("AnaIndirizzo")
            TextEdit5.Text = dataRd.Item("AnaCap")
            TextEdit6.Text = dataRd.Item("AnaCitta")
            TextEdit7.Text = dataRd.Item("AnaProv")
            TextEdit8.Text = "P.I. " & dataRd.Item("AnaPiva") & " - C.F. " & dataRd.Item("AnaCFis")
            REM
            TipoDoc = dataRd.Item("FatTipoDoc")
            CodBan = dataRd.Item("FatCodBan")
            NReg = dataRd.Item("FatNumReg")
            Abilita = dataRd.Item("FatTrasf")
            TextEdit3.Text = dataRd.Item("FatPagCod")
            TextEdit20.Text = dataRd.Item("ClAbi")
            TextEdit19.Text = dataRd.Item("ClCab")
            TextEdit18.Text = dataRd.Item("ClCC")
            TextEdit22.Text = dataRd.Item("ClCinEur")
            TextEdit21.Text = dataRd.Item("ClCin")
            TextEdit15.EditValue = CDec(dataRd.Item("FatBollo"))
            CheckButton2.Visible = Not dataRd.Item("ClAttivo")
            Documento = True
        End If
        dataRd.Close()
        MemoEdit2.Text = LeggiPagam(Val(TextEdit3.Text))
        TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))
        ComboBoxEdit1.SelectedIndex = SettaComboEdit(ComboBoxEdit1, CodBan.ToString.PadLeft(3, " "), 3)
        If CodBan > 0 Then LeggiBancaAzienda(CodBan)
        AssegnaContoRicavi(Cliente)
        ImageComboBoxEdit12.SelectedIndex = -1 ' rem per scatenare l'evento change selected index ho bisogno di 2 elementi
        ImageComboBoxEdit12.SelectedIndex = SettaComboImage(ImageComboBoxEdit12, NReg)
        ButtonPDF.Enabled = File.Exists(PathSto & "FC" & Rifer & ".pdf")
        ButtonF3.Enabled = Not Abilita : ButtonF1.Enabled = Not Abilita : ButtonF8.Enabled = Not Abilita

    End Sub
    Function AssegnaContoRicavi(ByVal CCli As String) As Boolean
        REM RICERCA RICAVO PRECEDENTE ( STESSO CLIENTE)
        Dim str As String = "select distinct TOP 1 FatCliFat,CorCnTrp,CorCiva from tbfat inner join tbcor on fatrif = corrif and fattipodoc = cortipodoc " _
        & "where CorCnTrp >'' AND FATCLIFAT = '" & CCli.PadLeft(5, "0") & "'"
        Dim CPT As String = ""
        Dim CII As Int16 = 0
        Cmd = New SqlCommand(str, cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CPT = dataRd.Item("CorCnTrp")
            CII = dataRd.Item("CorCiva")
        End While
        dataRd.Close()
        If CPT.Length > 3 Then ImageComboBoxEdit13.EditValue = CPT : TextEdit12.EditValue = Mid(ImageComboBoxEdit13.Text, 7, 35) Else ImageComboBoxEdit13.SelectedIndex = 0 : TextEdit12.EditValue = ""
        If CII > 0 Then
            ImageComboBoxEdit3.SelectedIndex = SettaComboImage(ImageComboBoxEdit3, CII)
        End If
    End Function
    Function LeggiBancaAzienda(ByVal nb As Int16) As Boolean
        If nb > 0 Then
            Cmd = New SqlCommand("SELECT  * FROM TbBan where BanCod=" & nb, cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                TextEdit20.Text = dataRd.Item("BanAbi")
                TextEdit19.Text = dataRd.Item("BanCab")
                TextEdit18.Text = dataRd.Item("BanCC")
                TextEdit22.Text = dataRd.Item("BanCinEur")
                TextEdit21.Text = dataRd.Item("BanCin")
            End While
            dataRd.Close()
            TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))
        Else
            TextEdit20.Text = ""
            TextEdit19.Text = ""
            TextEdit18.Text = ""
            TextEdit22.Text = ""
            TextEdit23.Text = "IT"
            TextEdit21.Text = ""
            TextEdit24.Text = ""
        End If
    End Function
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        If ComboBoxEdit1.SelectedIndex > -1 Then
            LeggiBancaAzienda(Val(Mid(ComboBoxEdit1.Text, 1, 3)))
        End If
    End Sub
    Function LeggiPagam(ByVal codice As Int16) As String
        LeggiPagam = ""
        Cmd = New SqlCommand("select * from TbPag where PagCod = " & codice, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            LeggiPagam = dataRd.Item("PagDesc")
            TipoPag = dataRd.Item("PagTipo")
        End If
        dataRd.Close()
    End Function
    Sub NuovoDocumento()
        TextEdit1.Text = ""
        PULIZIA(0)
        Documento = False
        TextEdit11.Focus()
    End Sub
    Private Sub PopolaGridDett()
        TbRighe = New DataTable()
        DaRighe = New SqlDataAdapter("Select * from TbCor where CorRif = " & Rifer & " And CorTipoDoc = '" & TipoDoc & "'", cnDb)
        DaRighe.Fill(TbRighe)
        GridControl2.DataSource = TbRighe
        GridView2.ClearSelection()
        NRighe = TbRighe.Rows.Count
        TotalizzaDocumento()
    End Sub
    Private Sub TotalizzaDocumento()
        Dim y As Int16 = 0
        For x As Int16 = 0 To 73
            imp(x) = 0 : iva(x) = 0 : cor(x) = 0
        Next
        If TipoReg = 3 Then Segno = -1 Else Segno = 1
        Totale = 0
        TImponibile = 0
        TIva = 0
        For x As Int16 = 0 To TbRighe.Rows.Count - 1
            y = TbRighe.Rows(x)("CorCiva")
            cor(y) = cor(y) + TbRighe.Rows(x)("CorImporto") * Segno
        Next
        For x As Int16 = 1 To 72
            TImponibile = TImponibile + cor(x)
            If cor(x) <> 0 And CiiPerc(x) > 0 Then
                iva(x) = (cor(x) * CiiPerc(x)) / 100
                TIva = TIva + iva(x)
            End If
            Totale = Totale + cor(x) + iva(x)
        Next
        TextEdit10.EditValue = TImponibile
        TextEdit16.EditValue = TIva
        TextEdit33.EditValue = Totale
    End Sub



    Private Sub LeggiDocumento(ByVal Numero As Int32)
        Dim strstr As String
        strstr = "Select top 1 isnull(FatRif,0) from VFatture where FatNum = @Numero and FatNumReg = " & NReg & " and year(FatData) = " & Anno
        Cmd = New SqlCommand(strstr, cnDb)
        Dim p2 As New SqlParameter("@Numero", SqlDbType.Int)
        p2.Value = Numero
        Cmd.Parameters.Add(p2)
        Rifer = Cmd.ExecuteScalar
    End Sub

    Private Sub ComboBoxEdit3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit3.SelectedIndexChanged
        If ComboBoxEdit3.SelectedIndex > -1 Then
            Anno = ComboBoxEdit3.SelectedItem
            LeggiUltimeFatture()
        End If
    End Sub

    Private Sub ImageComboBoxEdit12_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit12.SelectedIndexChanged
        If ImageComboBoxEdit12.SelectedIndex > -1 Then
            nn = ImageComboBoxEdit12.SelectedItem
            NReg = nn.Value
            TipoReg = nn.ImageIndex
        Else
            NReg = 0
            TipoReg = 1
        End If
    End Sub
    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        PULIZIA(0)
        PulisciDett()
        TextEdit1.Focus()
    End Sub
    Private Sub gridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        IsetI = hi.RowHandle
    End Sub
    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If IsetI > -1 Then
            RwD = GridView1.GetRow(IsetI)
            Rifer = RwD("FatRif")
            If Rifer > 0 Then
                RIABILITAGROUPBOX(True)
                CaricaDati()
                PopolaGridDett()
            Else
                Rifer = -1
            End If
        End If
    End Sub
    Sub TextEdit3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit3.LostFocus
        If Val(TextEdit3.Text) > 0 Then PagCod = Val(TextEdit3.Text) : RilevaPagamento()
    End Sub
    Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        RicCliFor("C")
    End Sub
    Sub RicCliFor(ByVal cf As String)
        Dim tipo As String
        If cf = "F" Then
            tipo = "FO"
        Else
            tipo = "CL"
        End If
        Dim frm As New RicercaClFo
        frm.StartPosition = FormStartPosition.CenterScreen
        frm.CliFor = tipo
        frm.ShowDialog()
        If Val(frm.Codice) > 1000 Then TextEdit11.Text = frm.Codice
        TbLeggiCli.Focus()
    End Sub
    Sub TbLeggiCli_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TbLeggiCli.Enter
        If Documento = True And Cliente = TextEdit11.Text.PadLeft(5, "0") Then Exit Sub
        If leggiCli(TextEdit11.Text.PadLeft(5, "0")) = False Then
            TextEdit11.Focus()
        End If
        If TextEdit3.EditorContainsFocus = True Then TextEdit11.Focus() Else TextEdit3.Focus()
    End Sub
    Function leggiCli(ByVal codice As String) As Boolean
        leggiCli = False
        If dataRd.IsClosed = False Then
            Exit Function
        End If
        Dim CodBan As Int16 = 0
        Cmd = New SqlCommand("select * from CRCLI where AnaCod ='" & codice & "'", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit11.Text = codice
            TextEdit2.Text = dataRd.Item("AnaRag1") & " " & dataRd.Item("AnaRag2")
            TextEdit4.Text = dataRd.Item("AnaIndirizzo")
            TextEdit5.Text = dataRd.Item("AnaCap")
            TextEdit6.Text = dataRd.Item("AnaCitta")
            TextEdit7.Text = dataRd.Item("AnaProv")
            TextEdit8.Text = "P.I. " & dataRd.Item("AnaPiva") & " - C.F. " & dataRd.Item("AnaCFis")
            CodBan = dataRd.Item("ClCodBan")
            TextEdit20.Text = dataRd.Item("ClAbi")
            TextEdit19.Text = dataRd.Item("ClCab")
            TextEdit18.Text = dataRd.Item("ClCC")
            TextEdit22.Text = dataRd.Item("ClCinEur")
            TextEdit21.Text = dataRd.Item("ClCin")
            TextEdit3.Text = dataRd.Item("ClPagam")
            CheckButton2.Visible = Not dataRd.Item("ClAttivo")
            leggiCli = True
        End If
        dataRd.Close()
        If leggiCli = True Then
            MemoEdit2.Text = LeggiPagam(Val(TextEdit3.Text))
            TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))
            ComboBoxEdit1.SelectedIndex = SettaComboEdit(ComboBoxEdit1, CodBan.ToString.PadLeft(3, " "), 3)
        End If
    End Function
    Sub HyperLinkEdit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkEdit2.Click
        PagCod = Ricerche.LnkCodPag()
        RilevaPagamento()
    End Sub
    Sub RilevaPagamento()
        If PagCod > 0 Then
            TextEdit3.Text = PagCod.ToString.PadLeft(3, "0")
            MemoEdit2.Text = LeggiPagam(PagCod)
        End If
        ComboBoxEdit1.Focus()
    End Sub
    Sub HyperLinkEdit1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkEdit1.Click
        Dim rabibanca As abibanca
        rabibanca = Ricerche.LnkAppoggio(Val(TextEdit20.Text), Val(TextEdit19.Text))
        If rabibanca.abi <> 0 And rabibanca.cab <> 0 Then
            TextEdit20.Text = rabibanca.abi.ToString.PadLeft(5, "0")
            TextEdit19.Text = rabibanca.cab.ToString.PadLeft(5, "0")
            TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))
        End If
    End Sub
    Function LeggiAbiCab(ByVal Abi As Int32, ByVal Cab As Int32) As String
        LeggiAbiCab = ""
        Cmd = New SqlCommand("select * from TbCab where CaAbi = " & Abi & " and CaCab =" & Cab, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            LeggiAbiCab = dataRd.Item("CaDescFt")
            TextEdit23.Text = dataRd.Item("CaPaese")
        End If
        dataRd.Close()
    End Function
    Private Sub TextEdit20_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit20.LostFocus, TextEdit19.LostFocus, TextEdit18.Enter
        If Val(TextEdit20.Text) > 0 And Val(TextEdit19.Text) > 0 And TextEdit24.Text.Length = 0 Then
            TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))
        End If
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If ControllaCampi() = False Then Exit Sub
        RIABILITAGROUPBOX(False)
        parzialeGROUPBOX(False)
        PulisciRiga()
        TextEdit9.Focus()
    End Sub
    Private Sub ButtonPDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPDF.Click
        StampaInPdf(PathSto & "FC" & Rifer & ".pdf")
    End Sub
    Function ControllaCampi() As Boolean
        Dim CC As New Control
        Dim Mail As String = ""

        If Val(TextEdit11.Text) < 1001 Then
            Mail = Mail & "<>MANCA IL CLIENTE" & Chr(13)
            CC = TextEdit11
        End If
        If Val(TextEdit3.Text) = 0 Then
            Mail = Mail & "<>MANCANO LE CONDIZIONI DI PAGAMENTO" & Chr(13)
            CC = TextEdit3
        End If

        If Mail > "" Then
            MoltoCritico(Mail)
            CC.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
    Sub MoltoCritico(ByVal Mail As String)
        Dim response As MsgBoxResult
        response = MsgBox(Mail, MsgBoxStyle.Critical, "CONTROLLO INSERIMENTO")
    End Sub

#End Region
#Region "DETTAGLIO"
    Private Sub PulisciDett()
        TbRighe = New DataTable
        GridControl2.DataSource = TbRighe
    End Sub
    Sub PulisciRiga()
        PopolaGridDett()
        Prog = 0
        TextEdit9.Text = ""
        TextEdit26.Text = ""
        TextEdit32.EditValue = CDec(0.0)
        GridView2.MoveLastVisible()
        GridView2.UnselectRow(GridView2.RowCount - 1)
    End Sub


    Private Sub gridControl2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl2.MouseMove
        ShowHitInfo2(GridView2.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo2(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl2.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        IsetR = hi.RowHandle
    End Sub

    Private Sub GridView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.Click
        If IsetR > -1 Then
            iset = IsetR
            RwX = GridView2.GetRow(iset)
            CaricaDatiD()
        End If
    End Sub

    Sub CaricaDatiD()
        TextEdit9.Text = RwX("CorDesc")
        TextEdit26.Text = RwX("CorProg")
        Prog = RwX("CorProg")
        TextEdit32.EditValue = RwX("CorImporto")
        ImageComboBoxEdit3.SelectedIndex = SettaComboImage(ImageComboBoxEdit3, RwX("CorCiva"))
        ImageComboBoxEdit13.EditValue = RwX("CorCntrp") : TextEdit12.EditValue = Mid(ImageComboBoxEdit13.Text, 7, 35)
        TextEdit9.Focus()
    End Sub
    Private Sub TextEdit14_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit14.Enter
        If controllo() = True Then
            If FL = False Then AggiornaTestata() : FL = True : Documento = True
            AggiornaRiga() ' SONO QUI
            TotalizzaDocumento()
            TextEdit9.Focus()
        End If
    End Sub
    Sub AggiornaRiga()
        Dim ScriviCorpo As String

        If Prog = 0 Then
            ScriviCorpo = "Insert Into TbCor (CorTipoDoc,CorRif,CorProg,CorDesc,CorImporto,CorCiva,CorCntrp,CorCau,CorDescIva,CorDesCpt)" _
                        & " VALUES (@CorTipoDoc,@CorRif,@CorProg,@CorDesc,@CorImporto,@CorCiva,@CorCntrp,@CorCau,@CorDescIva,@CorDesCpt)"
        Else
            ScriviCorpo = "UPDATE TbCor SET CorTipoDoc=@CorTipoDoc,CorDesc=@CorDesc,CorImporto=@CorImporto,CorCiva=@CorCiva,CorCntrp=@CorCntrp, " _
             & " CorCau=@CorCau,CorDescIva=@CorDescIva,CorDesCpt=@CorDesCpt where CorRif = @Corrif and CorProg =@CorProg and CortipoDoc =@CorTipoDoc"
        End If

        Dim p0 As New SqlParameter("@CorTipoDoc", SqlDbType.VarChar)
        Dim p1 As New SqlParameter("@CorRif", SqlDbType.Int)
        Dim p2 As New SqlParameter("@CorProg", SqlDbType.SmallInt)

        Dim p3 As New SqlParameter("@CorDesc", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@CorImporto", SqlDbType.Decimal)
        Dim p5 As New SqlParameter("@CorCiva", SqlDbType.SmallInt)

        Dim p6 As New SqlParameter("@CorCntrp", SqlDbType.VarChar)

        Dim p8 As New SqlParameter("@CorDescIva", SqlDbType.VarChar)
        Dim p9 As New SqlParameter("@CorDesCpt", SqlDbType.VarChar)
        Dim p10 As New SqlParameter("@CorCau", SqlDbType.SmallInt)



        REM i documenti sono tutti di tipo F
        p0.Value = "F"
        p1.Value = Rifer
        p2.Value = IIf(Prog = 0, UltProg(Rifer) + 1, Prog)
        p3.Value = TextEdit9.Text
        p4.Value = CDec(TextEdit32.EditValue)

        If CDec(TextEdit32.EditValue) = 0 Then
            p5.Value = 0 : p6.Value = "" : p8.Value = "" : p9.Value = ""
        Else
            p5.Value = Val(Mid(ImageComboBoxEdit3.Text, 1, 2))
            p6.Value = ImageComboBoxEdit13.EditValue

            p8.Value = ImageComboBoxEdit3.Text
            p9.Value = TextEdit12.Text.Trim
        End If
        p10.Value = 5 REM causale vendita

        Cmd = New SqlCommand(ScriviCorpo, cnDb)
        Cmd.Parameters.Add(p0)
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.Parameters.Add(p6)
        Cmd.Parameters.Add(p8)
        Cmd.Parameters.Add(p9)
        Cmd.Parameters.Add(p10)
        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()
        If Prog > 0 Then
            If TbRighe.Rows.Count - 1 > iset Then
                PopolaGridDett()
                iset = iset + 1
                RwX = GridView2.GetRow(iset)
                GridView2.MoveBy(iset)
                CaricaDatiD()
                Exit Sub
            End If
        End If
        PulisciRiga()
    End Sub
    Private Function UltProg(ByVal rifer As Integer) As Int32
        Cmd = New SqlCommand("select isnull(max(CorProg),0) from TbCor where CorTipoDoc = 'F' and CorRif = " & rifer, cnDb)
        Return Cmd.ExecuteScalar
    End Function
    Private Function controllo() As Boolean
        controllo = True
        If Val(TextEdit32.EditValue) <> 0 Then
            If ImageComboBoxEdit3.SelectedIndex = -1 Then
                ImageComboBoxEdit3.Focus()
                Return False
            End If
            If ImageComboBoxEdit13.EditValue = "" Then
                ImageComboBoxEdit13.Focus()
                Return False
            End If
        End If

    End Function
    Private Sub ButtonXF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF11.Click
        REM CONTROLLO SE NON ESISTONO RIGHE ELIMINO IL DOCUMENTO
        If TextEdit10.EditValue = 0 And TextEdit16.EditValue = 0 Then
            AnnulloTotaleDocumento()
            RitornaAllinizio()
            Exit Sub
        End If
        parzialeGROUPBOX(True)
        TextEdit15.Focus()
    End Sub
  
    Private Sub ImageComboBoxEdit3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit3.Enter
        If CDec(TextEdit32.Text) = 0.0 Then
            TextEdit14.Focus()
        End If
    End Sub
    Private Sub AggiornaTestata()
        Dim scrivi As String
        Dim ultimo As New SqlCommand("Select @@identity", cnDb)

        If Rifer < 1 Then
            scrivi = "Insert into TbFat WITH (TABLOCKX) (FatTipoDoc,FatData,FatNum,FatCliCons,FatPagCod,FatAbi,FatCab,FatEsespe,FatNimp,FatCau,FatCliFat,FatNumReg,FatLibera,FatCodBan,FatBollo) VALUES ( @TipoDoc,@Data,999999,@Cliente,@PagCod,@Abi,@Cab,0,0,@Cau,@CliFat,@NumReg,'L',@CodBan,@Bollo)"
        Else
            scrivi = "Update TbFat set FatCliCons=@Cliente,FatCliFat = @CliFat,FatData=@Data, FatPagCod =@PagCod, FatAbi=@Abi, FatCab=@Cab,FatCodBan=@CodBan,FatBollo=@Bollo where FatRif =" & Rifer
        End If

        Dim p1 As New SqlParameter("@TipoDoc", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@Data", SqlDbType.SmallDateTime)
        Dim p3 As New SqlParameter("@Cliente", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@PagCod", SqlDbType.SmallInt)
        Dim p6 As New SqlParameter("@Abi", SqlDbType.Int)
        Dim p7 As New SqlParameter("@Cab", SqlDbType.Int)
        Dim p10 As New SqlParameter("@Cau", SqlDbType.SmallInt)
        Dim p11 As New SqlParameter("@CodBan", SqlDbType.SmallInt)
        Dim p12 As New SqlParameter("@Numreg", SqlDbType.SmallInt)
        Dim p13 As New SqlParameter("@CliFat", SqlDbType.VarChar)
        Dim p14 As New SqlParameter("@Bollo", SqlDbType.Decimal)

        p1.Value = TipoDoc
        p2.Value = DateEdit1.EditValue
        p3.Value = Val(TextEdit11.Text).ToString("00000")
        p4.Value = Val(TextEdit3.Text)
        p6.Value = Val(TextEdit20.Text)
        p7.Value = Val(TextEdit19.Text)
        p10.Value = 5 ' rem causale vendita
        p11.Value = Val(Mid(ComboBoxEdit1.Text, 1, 3))
        p12.Value = NReg
        p13.Value = Val(TextEdit11.Text).ToString("00000")
        p14.Value = CDec(TextEdit15.EditValue)

        Cmd = New SqlCommand(scrivi, cnDb)

        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p6)
        Cmd.Parameters.Add(p7)
        Cmd.Parameters.Add(p10)
        Cmd.Parameters.Add(p11)
        Cmd.Parameters.Add(p12)
        Cmd.Parameters.Add(p13)
        Cmd.Parameters.Add(p14)
        Cmd.ExecuteNonQuery()
        If Rifer < 1 Then Rifer = ultimo.ExecuteScalar
    End Sub
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        AnnulloMovimenti()
    End Sub
    Private Sub AnnulloMovimenti()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "Annullo Completamente il Documento " & TextEdit1.Text & " DEL " & CDate(DateEdit1.EditValue).ToShortDateString & "  ?"
        style = MsgBoxStyle.YesNo
        title = "ELIMINAZIONE COMPLETA DOCUMENTO"
        response = MsgBox(msg, style, title)
        If response = MsgBoxResult.No Then Exit Sub
        AnnulloTotaleDocumento()
        RitornaAllinizio()
    End Sub
    Sub AnnulloTotaleDocumento()
        Cmd = New SqlCommand("Delete from TbCor where CorTipoDoc ='F' and CorRif = " & Rifer, cnDb)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("Delete from TbFat where FatRif = " & Rifer, cnDb)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("Delete from TbDcg where DcgNumrif = " & Rifer, cnDb)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ButtonXF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF3.Click
        If Prog > 0 Then EliminaRiga()
        TextEdit9.Focus()
    End Sub

    Private Sub EliminaRiga()
        If MessageBox.Show("Elimino la riga Evidenziata ?", "ELIMINAZIONE RIGHE DOCUMENTO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        Cmd = New SqlCommand("Delete from TbCor where CorTipoDoc = 'F' AND CorRif = @Rifer and CorProg = @Prog", cnDb)

        Dim p1 As New SqlParameter("@rifer", SqlDbType.Int)
        Dim p2 As New SqlParameter("@Prog", SqlDbType.Int)

        p1.Value = Rifer
        p2.Value = Prog

        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)

        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()
        PulisciRiga()
    End Sub
    Private Sub ButtonXF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF5.Click
        PulisciRiga()
        TextEdit9.Focus()
    End Sub
#End Region
#Region "CHIUDI e/o STAMPA"
    Private Sub ButtonF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF2.Click
        AggiornaTestata()
        RitornaAllinizio()
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        AggiornaTestata()
        If ASSEGNANUMERO() = True Then
            BienneChiusura.ChiudeFattura(Rifer)
            StampaFattura()
        End If
        RitornaAllinizio()
    End Sub
    Sub RitornaAllinizio()
        LeggiUltimeFatture()
        PulisciDett()
        PULIZIA(0)
        TextEdit1.Focus()
    End Sub
    Private Sub StampaFattura()
        Dim NomeFATTURA As String = PathSto & "FC" & Rifer & ".pdf"
        Dim REPORT As New XBienneFat
        StrPrint = "Select * from VPRINTFAT  WHERE FatRif = " & Rifer
        DsFat = New DataTable
        DaFat = New SqlDataAdapter(StrPrint, cnDb)
        DaFat.SelectCommand.CommandTimeout = 300
        DaFat.Fill(DsFat)
        REPORT.DataSource = DsFat
        REPORT.DataMember = "DsFat"
        REPORT.CreateDocument()
        REPORT.ExportToPdf(NomeFATTURA)
        REPORT.ShowPreviewDialog()
    End Sub
    Function ASSEGNANUMERO() As Boolean
        Dim Numero As Integer = 0
        Dim Registro As Integer = 0
        Dim MData As Date = Today
        Dim LData As Date = Today
        Dim UltNumero As Integer = 0
        Dim Mail As String = ""
        Cmd = New SqlCommand("select * from TbFat where FatRif = " & Rifer, cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Numero = dataRd.Item("FatNum")
            Registro = dataRd.Item("FatNumReg")
            LData = CDate(dataRd.Item("FatData"))
        End If
        dataRd.Close()
        If Numero <> 999999 Then Return True : Exit Function
        REM Controllo Data Max
        Cmd = New SqlCommand("select MAX(FatData) from tbfat where FatNum <> 999999 and datepart(year,FatData)=" & LData.Year & " and FatNumReg = " & Registro, cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            If dataRd.Item(0) Is DBNull.Value Then MData = LData Else MData = CDate(dataRd.Item(0))
        End If
        dataRd.Close()
        If CDate(MData) > CDate(LData) Then
            Mail = " DATA FATTURA " & LData & " < ULTIMA FATTURA EMESSA " & MData & Chr(13)
            MoltoCritico(Mail)
            Return False
            Exit Function
        End If

        Cmd = New SqlCommand("Select dbo.FnUltNumero(@tipo,@Rifer)", cnDb)
        Dim p1 As New SqlParameter("@Tipo", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@Rifer", SqlDbType.Int)
        p1.Value = "F"
        p2.Value = Rifer
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        UltNumero = Cmd.ExecuteScalar + 1
        Cmd = New SqlCommand("Update TbFat set FatNum = " & UltNumero & " where FatRif = " & Rifer, cnDb)
        Cmd.ExecuteNonQuery()
        TextEdit1.EditValue = UltNumero
        Return True
    End Function
#End Region
#Region "TASTI FUNZIONE"
    Sub FatNcr_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If GroupControl22.Enabled = True Then
            If e.KeyData = Keys.F9 Then
                e.Handled = True
                ButtonF9.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F2 Then
                e.Handled = True
                ButtonF2.PerformClick()
                Exit Sub
            End If
        End If
        If GroupControl25.Enabled = False Then

            'Testata
            If e.KeyData = Keys.F1 Then
                e.Handled = True
                ButtonF1.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F5 Then
                e.Handled = True
                ButtonF5.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F3 Then
                e.Handled = True
                ButtonF3.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F8 Then
                e.Handled = True
                ButtonF8.PerformClick()
                Exit Sub
            End If
        Else
            'Dettaglio
            If e.KeyData = Keys.F5 Then
                e.Handled = True
                ButtonXF5.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F3 Then
                e.Handled = True
                ButtonXF3.PerformClick()
                Exit Sub
            End If

            If e.KeyData = Keys.F11 Then
                e.Handled = True
                ButtonXF11.PerformClick()
                Exit Sub
            End If
        End If
    End Sub
#End Region
End Class