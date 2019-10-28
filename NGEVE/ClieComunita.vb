Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO

Public Class ClieComunita
    Structure gruppiclfo
        Dim CliMax As Integer
        Dim al() As Integer
        Dim ult() As Integer
    End Structure
    Dim gruppi As New gruppiclfo
    Dim Clie, Ana As Boolean
    Dim PagCod As Int16
    Dim FileNote As String
    Dim Admin As Boolean

    Dim TbAna As DataTable
    Dim DaAna As SqlDataAdapter
    Dim RwAna As DataRowView
    Dim Ultimo As String = ""
    Dim MIGLIO As Integer = -1

    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim WithEvents UserPrint As DevExpress.XtraEditors.XtraUserControl


    Private Sub AnagCli_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        XtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False
        XtraTabControl1.SelectedTabPageIndex = 0
        LeggiTabelle()
        pulizia()
        PopolaGrid()
    End Sub

#Region "ANAGRAFICA"
    Sub LeggiTabelle()
        'Lettura risorse
        Admin = False
        Dim Cmd As New SqlCommand("Select * from TbRisorse where [RIS-USERW] ='" & Userwin & "'", cnVd)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Admin = dataRd.Item("RIS-AMMIN")
        End If
        dataRd.Close()
        REM IN ALCUNI CASI DI GESTIONE VENDITE SOLO PER L'AMMINISTRATORE DI SISTEMA 
        ''ButtonF3.Enabled = Admin
        ComboBoxEdit4.Properties.Items.Clear()
        ComboBoxEdit4.Properties.Items.Add("")
        Cmd = New SqlCommand(" SELECT * from TbBan Order by BanCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit4.Properties.Items.Add(dataRd.Item("Bancod").ToString.PadLeft(3, " ") & " - " & dataRd.Item("BanDes"))
        End While
        dataRd.Close()
    End Sub
    Private Sub pulizia()
        Dim h1 As Object = HyperLinkEdit1.EditValue
        Dim h2 As Object = HyperLinkEdit2.EditValue
        PulisciDxCampi(Me)
        HyperLinkEdit1.EditValue = h1
        HyperLinkEdit2.EditValue = h2
        iset = -1 : PagCod = -1
        GroupControl17.Enabled = True
        ErrorProvider1.SetError(ButtonF6, "")
        TextEdit3.ErrorText = "" : TextEdit5.ErrorText = "" : TextEdit35.ErrorText = ""
        cargo_ultimi()
        TextEdit00.Enabled = True
        PictureBox1.BringToFront()
        PictureBox3.BringToFront()
        PictureBox5.BringToFront()
        TextEdit00.Focus()
        CheckButton2.Checked = True : CheckButton2.Checked = False : XtraTabControl2.SelectedTabPageIndex = 2
    End Sub

    Private Sub cargo_ultimi()
        gruppi = leggi_ultimi("CL")
        BarStaticItem1.Caption = gruppi.ult(0).ToString("00000;#;#")
        BarStaticItem2.Caption = gruppi.ult(1).ToString("00000;#;#")
        BarStaticItem3.Caption = gruppi.ult(2).ToString("00000;#;#")
        BarStaticItem4.Caption = gruppi.ult(3).ToString("00000;#;#")
        BarStaticItem5.Caption = gruppi.ult(4).ToString("00000;#;#")
        BarStaticItem6.Caption = gruppi.ult(5).ToString("00000;#;#")
        BarStaticItem7.Caption = gruppi.ult(6).ToString("00000;#;#")
        BarStaticItem8.Caption = gruppi.ult(7).ToString("00000;#;#")
        BarStaticItem9.Caption = gruppi.ult(8).ToString("00000;#;#")
    End Sub
    Private Function leggi_ultimi(ByVal CLFO As String) As gruppiclfo
        Dim strselect As String = "select * from TbGrp where GrpCod = '" & CLFO & "'"
        Cmd = New SqlCommand(strselect, cnCo)
        dataRd = Cmd.ExecuteReader
        ReDim leggi_ultimi.ult(8)
        ReDim leggi_ultimi.al(8)
        If dataRd.Read Then
            leggi_ultimi.CliMax = dataRd.GetInt32(2)
            leggi_ultimi.al(0) = dataRd.GetInt32(3)
            leggi_ultimi.al(1) = dataRd.GetInt32(4)
            leggi_ultimi.al(2) = dataRd.GetInt32(5)
            leggi_ultimi.al(3) = dataRd.GetInt32(6)
            leggi_ultimi.al(4) = dataRd.GetInt32(7)
            leggi_ultimi.al(5) = dataRd.GetInt32(8)
            leggi_ultimi.al(6) = dataRd.GetInt32(9)
            leggi_ultimi.al(7) = dataRd.GetInt32(10)
            leggi_ultimi.al(8) = dataRd.GetInt32(11)
            leggi_ultimi.ult(0) = dataRd.GetInt32(12)
            leggi_ultimi.ult(1) = dataRd.GetInt32(13)
            leggi_ultimi.ult(2) = dataRd.GetInt32(14)
            leggi_ultimi.ult(3) = dataRd.GetInt32(15)
            leggi_ultimi.ult(4) = dataRd.GetInt32(16)
            leggi_ultimi.ult(5) = dataRd.GetInt32(17)
            leggi_ultimi.ult(6) = dataRd.GetInt32(18)
            leggi_ultimi.ult(7) = dataRd.GetInt32(19)
            leggi_ultimi.ult(8) = dataRd.GetInt32(20)
        End If
        dataRd.Close()
        MIGLIO = leggi_ultimi.CliMax
    End Function
    Private Sub PopolaGrid()
        Dim Str As String = "Select * from TbAna where AnaGrp='CL' order by AnaDesc"
        TbAna = New DataTable("TbAna")
        DaAna = New SqlDataAdapter(Str, cnVd)
        DaAna.Fill(TbAna)
        GridControl3.DataSource = TbAna
        GridView4.ActiveFilterString = ""
    End Sub

    Private Sub leggiNote()
        FileNote = vedinote("CL", TextEdit00.Text)
        If File.Exists(FileNote) = True Then ErrorProvider1.SetError(ButtonF6, "Sono presenti Note!") Else ErrorProvider1.SetError(ButtonF6, "")
    End Sub

    Private Sub leggiAna(ByVal codice As String)
        Cmd = New SqlCommand("select * from TbAna where AnaGrp = 'CL' and AnaCod ='" & codice & "'", cnVd)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            CaricaAna()
            Ana = True
        Else
            AzzeraAna()
            Ana = False
        End If
        dataRd.Close()

        output_piva()
        output_codfisc()
    End Sub
    Private Sub CaricaAna()
        TextEdit1.Text = dataRd.Item("AnaRag1")
        TextEdit2.Text = dataRd.Item("AnaRag2")
        TextEdit3.Text = dataRd.Item("AnaPiva")
        TextEdit4.Text = dataRd.Item("AnaIndirizzo")
        TextEdit5.Text = dataRd.Item("AnaCFis")
        TextEdit6.Text = dataRd.Item("AnaCap")
        TextEdit7.Text = dataRd.Item("AnaCitta")
        TextEdit8.Text = dataRd.Item("AnaProv")
        TextEdit9.Text = dataRd.Item("AnaPivaEst")
        TextEdit10.Text = dataRd.Item("AnaTel1")
        TextEdit11.Text = dataRd.Item("AnaTel2")
        TextEdit12.Text = dataRd.Item("AnaTel3")
        TextEdit13.Text = dataRd.Item("AnaFax")
        TextEdit14.Text = dataRd.Item("AnaResp")
        TextEdit15.Text = dataRd.Item("AnaEmail")
        TextEdit16.Text = dataRd.Item("Anawww")
        CheckButton1.Checked = dataRd.Item("AnaNoRubrica")
    End Sub
    Private Sub output_piva()
        If Codfisc(TextEdit3.Text) = False Then TextEdit3.ErrorText = " ! Errato" Else TextEdit3.ErrorText = ""
    End Sub
    Private Sub output_codfisc()
        If Codfisc(TextEdit5.Text) = False Then
            TextEdit5.ErrorText = " ! Errata"
        Else
            TextEdit5.ErrorText = ""
            If Len(TextEdit5.Text) = 16 Then CaricaDatiNascita(0, TextEdit5.Text)
        End If
        If Codfisc(TextEdit35.Text) = False Then
            TextEdit35.ErrorText = " ! Errata"
        Else
            TextEdit35.ErrorText = ""
            If Len(TextEdit35.Text) = 16 Then CaricaDatiNascita(1, TextEdit35.Text)
        End If
    End Sub
    Sub CaricaDatiNascita(x As Int16, cf As String)
        Dim COMUNE As String = ""
        Dim PROV As String = ""
        Dim SESSO As String = ""
        Dim CAP As String = ""
        Dim DataN As Date = Nothing
        If ScorporaCodFis(cf, COMUNE, PROV, SESSO, DataN, CAP) = True Then
            COMUNE = COMUNE.ToLower : COMUNE = Trim(Mid(COMUNE, 1, 1).ToUpper & Mid(COMUNE, 2, 100))
        Else
            Exit Sub
        End If
        If x = 0 Then
            TextEdit26.EditValue = CAP
            TextEdit27.EditValue = COMUNE
            TextEdit28.EditValue = PROV
            TextEdit33.EditValue = SESSO
            DateEdit2.EditValue = CDate(DataN)
        Else
            TextEdit36.EditValue = CAP
            TextEdit37.EditValue = COMUNE
            TextEdit38.EditValue = PROV
            TextEdit39.EditValue = SESSO
            DateEdit3.EditValue = CDate(DataN)
        End If
    End Sub
    Private Sub AzzeraAna()
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        TextEdit3.Text = ""
        TextEdit4.Text = ""
        TextEdit5.Text = ""
        TextEdit6.Text = ""
        TextEdit7.Text = ""
        TextEdit8.Text = ""
        TextEdit9.Text = ""
        TextEdit10.Text = ""
        TextEdit11.Text = ""
        TextEdit12.Text = ""
        TextEdit13.Text = ""
        TextEdit14.Text = ""
        TextEdit15.Text = ""
        TextEdit16.Text = ""
        CheckButton1.Checked = False
    End Sub
    Private Function LeggiPagam(ByVal codice As Int16) As String
        LeggiPagam = ""
        Cmd = New SqlCommand("select * from TbPag where PagCod = " & codice, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            LeggiPagam = dataRd.Item("PagDesc")
        End If
        dataRd.Close()
    End Function

    Private Sub TbLeggiCod_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggiCod.Enter
        If Val(TextEdit00.Text) > 1000 And Val(TextEdit00.Text) < gruppi.CliMax Then
            leggiNote()
            If iset > -1 Then GridView4.UnselectRow(iset)
            XtraTabControl2.SelectedTabPageIndex = 0
            leggiCli(TextEdit00.Text)
            leggiAna(TextEdit00.Text)
            SelectNextControl(TextEdit1, True, True, True, True)
            Ultimo = TextEdit00.EditValue
        End If
    End Sub

    Private Sub leggiCli(ByVal codice As String)
        Cmd = New SqlCommand("select * from TbCli where ClCod ='" & codice & "'", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Clie = True
            CaricaCli()
        Else
            Clie = False
            AzzeraCli()
        End If
        dataRd.Close()

        MemoEdit2.Text = LeggiPagam(Val(TextEdit17.Text))
        TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))

        GroupControl22.Enabled = True
        GroupControl17.Enabled = True

        FileNote = vedinote("CL", TextEdit00.Text)
        If File.Exists(FileNote) = True Then ErrorProvider1.SetError(ButtonF6, "Sono presenti Note!") Else ErrorProvider1.SetError(ButtonF6, "")
        ButtonF6.Enabled = Clie
    End Sub

    Private Function LeggiAbiCab(ByVal Abi As Int32, ByVal Cab As Int32) As String
        LeggiAbiCab = ""
        TextEdit19.Text = Cab
        TextEdit20.Text = Abi
        Cmd = New SqlCommand("select * from TbCab where CaAbi = " & Abi & " and CaCab =" & Cab, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            LeggiAbiCab = dataRd.Item("CaDescFt")
            TextEdit23.Text = dataRd.Item("CaPaese")
            TextEdit20.Text = dataRd.Item("CaAbi")
            TextEdit19.Text = dataRd.Item("CaCab")
        End If
        dataRd.Close()
    End Function

    Private Sub CaricaCli()
        TextEdit17.Text = dataRd.Item("ClPagam")
        TextEdit20.Text = dataRd.Item("ClAbi")
        TextEdit19.Text = dataRd.Item("ClCab")
        TextEdit25.Text = dataRd.Item("ClCntRid")
        TextEdit18.Text = dataRd.Item("ClCC")
        If dataRd.Item("ClCinEur") Is DBNull.Value Then TextEdit22.Text = "" Else TextEdit22.Text = dataRd.Item("ClCinEur")
        If dataRd.Item("ClCin") Is DBNull.Value Then TextEdit21.Text = "" Else TextEdit21.Text = dataRd.Item("ClCin")
        If dataRd.Item("ClAttivo") Is DBNull.Value Then CheckButton2.Checked = True Else CheckButton2.Checked = dataRd.Item("ClAttivo")
        ComboBoxEdit4.SelectedIndex = SettaComboEdit(ComboBoxEdit4, dataRd.Item("ClCodBan").ToString.PadLeft(3, " "), 3)
        CheckEdit3.Checked = IIf(IsDBNull(dataRd.Item("ClBlackList")), False, dataRd.Item("ClBlackList"))
        DateEdit1.EditValue = dataRd.Item("Cl1RidData") : DateEdit2.EditValue = dataRd.Item("ClDtnData")
        TextEdit26.EditValue = dataRd.Item("ClDtnCap") : TextEdit27.EditValue = dataRd.Item("ClDtnCitta")
        TextEdit28.EditValue = dataRd.Item("ClDtnProv") : TextEdit29.EditValue = dataRd.Item("ClAnteIndirizzo")
        TextEdit30.EditValue = dataRd.Item("ClAnteCap") : TextEdit31.EditValue = dataRd.Item("ClAnteCitta")
        TextEdit32.EditValue = dataRd.Item("ClAnteProv") : TextEdit33.EditValue = dataRd.Item("ClDtnSex")
        TextEdit34.EditValue = dataRd.Item("ClCoCognomeNome") : TextEdit35.EditValue = dataRd.Item("ClCoCodFisc") : DateEdit2.EditValue = dataRd.Item("ClCoDtnData")
        TextEdit36.EditValue = dataRd.Item("ClCoCap") : TextEdit37.EditValue = dataRd.Item("ClCoCitta")
        TextEdit38.EditValue = dataRd.Item("ClCoProv") : TextEdit40.EditValue = dataRd.Item("ClCoAnteIndirizzo")
        TextEdit41.EditValue = dataRd.Item("ClCoAnteCap") : TextEdit42.EditValue = dataRd.Item("ClCoAnteCitta")
        TextEdit43.EditValue = dataRd.Item("ClCoAnteProv") : TextEdit39.EditValue = dataRd.Item("ClCoSex")

    End Sub
    Private Sub AzzeraCli()
        TextEdit17.Text = "000"
        TextEdit18.Text = ""
        TextEdit19.Text = ""
        TextEdit20.Text = ""
        TextEdit21.Text = ""
        TextEdit22.Text = ""
        TextEdit23.Text = "IT"
        TextEdit24.Text = ""
        TextEdit25.Text = ""
        MemoEdit2.Text = ""
        CheckButton2.Checked = True
        CheckEdit3.Checked = False
        ComboBoxEdit4.SelectedIndex = 0
        DateEdit1.EditValue = Nothing : DateEdit2.EditValue = Nothing : DateEdit3.EditValue = Nothing
        TextEdit26.EditValue = "" : TextEdit27.EditValue = "" : TextEdit28.EditValue = "" : TextEdit29.EditValue = "" : TextEdit30.EditValue = "" : TextEdit31.EditValue = "" : TextEdit32.EditValue = "" : TextEdit33.EditValue = ""
        TextEdit34.EditValue = "" : TextEdit35.EditValue = "" : TextEdit36.EditValue = "" : TextEdit37.EditValue = "" : TextEdit38.EditValue = "" : TextEdit39.EditValue = "" : TextEdit40.EditValue = "" : TextEdit41.EditValue = ""
        TextEdit42.EditValue = "" : TextEdit43.EditValue = ""
    End Sub
    Private Sub CheckButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckButton1.CheckedChanged
        If CheckButton1.Checked = True Then
            CheckButton1.ImageIndex = 16
            CheckButton1.ToolTip = "NON VISIBILE IN RUBRICA TELEFONICA"
        Else
            CheckButton1.ImageIndex = 17
            CheckButton1.ToolTip = "VISIBILE IN RUBRICA TELEFONICA"
        End If
    End Sub
    Private Sub CheckButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckButton2.CheckedChanged
        If CheckButton2.Checked = True Then
            CheckButton2.ImageIndex = 19
            CheckButton2.ToolTip = "CLIENTE ATTIVO"
        Else
            CheckButton2.ImageIndex = 18
            CheckButton2.ToolTip = "CLIENTE NON ATTIVO"
        End If
    End Sub
    Private Sub TextEdit3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit3.Validated
        output_piva()
        Dim testPiva As Int32 = EsistePiva(TextEdit00.Text, TextEdit3.Text, "CL", Me.Location.X + 456, Me.Location.Y + 84)
        If testPiva > 0 Then
            pulizia()
            TextEdit00.Text = testPiva.ToString("00000")
            leggiNote()
            leggiCli(TextEdit00.Text)
            leggiAna(TextEdit00.Text)
            SelectNextControl(TextEdit1, True, True, True, True)
        End If
    End Sub

    Private Sub TextEdit5_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit5.LostFocus, TextEdit35.LostFocus
        output_codfisc()
    End Sub

    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        pulizia() : PopolaGrid()
        If iset > -1 Then GridView4.UnselectRow(iset)
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Val(TextEdit00.Text) >= MIGLIO Or Val(TextEdit00.Text) < 1001 Then
            TextEdit00.Focus()
            Return
        End If
        If ControllaCampi() = False Then Exit Sub
        registra()
        pulizia()
        PopolaGrid()
        TextEdit00.Focus()
    End Sub

    Private Function ControllaCampi() As Boolean
        Dim Mail As String = ""
        Dim cc As New Control
        If TextEdit1.Text = "" Then
            Mail = "<>MANCA RAGIONE SOCIALE " & Chr(13)
            cc = TextEdit1
        End If

        If Mail > "" Then
            MoltoCritico(Mail, "INSERIMENTO CLIENTI")
            cc.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
    Sub MoltoCritico(ByVal Mail As String, ByVal contesto As String)
        MessageBox.Show(Mail, contesto, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub
    Private Sub registra()
        ScriviAna()
        ScriviCli()
        scrivi_ultimi("CL")
    End Sub
    Function RILEGGIANA() As Integer
        Cmd = New SqlCommand("SELECT isnull(ANACOD,0) from TBANA WHERE anagrp = 'CL' AND ANACOD = '" & TextEdit00.Text & "'", cnVd)
        RILEGGIANA = Cmd.ExecuteScalar
    End Function
    Function RILEGGICLI() As Integer
        Cmd = New SqlCommand("SELECT isnull(CLCOD,0) from TBCLI WHERE CLCOD = '" & TextEdit00.Text & "'", cnDb)
        RILEGGICLI = Cmd.ExecuteScalar
    End Function
    Private Sub ScriviAna()
        Dim scrivi As String = ""

        If Ana = False Then
            If RILEGGIANA() = 0 Then
                scrivi = "INSERT INTO TbAna (AnaDesc,AnaPiva,AnaCfis,AnaIndirizzo,AnaCap,AnaCitta,AnaProv,Anatel1,Anatel2,AnaTel3,AnaWWW,AnaEmail,AnaGrp,AnaCod,AnaResp,AnaNote,AnaFax,AnaRag1,AnaRag2,AnaPivaEst,AnaNoRubrica)  values(@AnaDesc,@AnaPiva,@AnaCfis,@AnaIndirizzo,@AnaCap,@AnaCitta,@AnaProv,@Anatel1,@Anatel2,@AnaTel3,@AnaWWW,@AnaEmail,'CL',@AnaCod,@AnaResp,'',@AnaFax,@AnaRag1,@AnaRag2,@AnaPivaEst,@AnaNoRubrica)"
            Else
                scrivi = "Update TbAna set AnaDesc=@AnaDesc,AnaPiva=@AnaPiva,AnaCfis=@AnaCFis,AnaIndirizzo=@AnaIndirizzo,AnaCap=@AnaCap,AnaCitta=@AnaCitta,AnaProv=@AnaProv,Anatel1=@AnaTel1,Anatel2=@AnaTel2,AnaTel3=@AnaTel3,AnaWWW=@AnaWWW,AnaEmail=@AnaEmail,AnaResp=@AnaResp,AnaFax=@AnaFax,AnaRag1=@AnaRag1,AnaRag2=@AnaRag2,AnaPivaEst=@AnaPivaEst,AnaNoRubrica=@AnaNoRubrica where AnaCod = @AnaCod and AnaGrp = 'CL'"
            End If
        Else
            scrivi = "Update TbAna set AnaDesc=@AnaDesc,AnaPiva=@AnaPiva,AnaCfis=@AnaCFis,AnaIndirizzo=@AnaIndirizzo,AnaCap=@AnaCap,AnaCitta=@AnaCitta,AnaProv=@AnaProv,Anatel1=@AnaTel1,Anatel2=@AnaTel2,AnaTel3=@AnaTel3,AnaWWW=@AnaWWW,AnaEmail=@AnaEmail,AnaResp=@AnaResp,AnaFax=@AnaFax,AnaRag1=@AnaRag1,AnaRag2=@AnaRag2,AnaPivaEst=@AnaPivaEst,AnaNoRubrica=@AnaNoRubrica where AnaCod = @AnaCod and AnaGrp = 'CL'"
        End If


        Cmd = New SqlCommand(scrivi, cnVd)

        Dim p1 As New SqlParameter("@AnaDesc", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@AnaPiva", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@AnaCfis", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@AnaIndirizzo", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@AnaCap", SqlDbType.VarChar)
        Dim p6 As New SqlParameter("@AnaCitta", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@AnaProv", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@AnaCod", SqlDbType.VarChar)
        Dim p9 As New SqlParameter("@AnaRag1", SqlDbType.VarChar)
        Dim p10 As New SqlParameter("@AnaRag2", SqlDbType.VarChar)
        Dim p11 As New SqlParameter("@AnaPivaEst", SqlDbType.VarChar)
        Dim p12 As New SqlParameter("@AnaTel1", SqlDbType.VarChar)
        Dim p13 As New SqlParameter("@AnaTel2", SqlDbType.VarChar)
        Dim p14 As New SqlParameter("@AnaTel3", SqlDbType.VarChar)
        Dim p15 As New SqlParameter("@AnaFax", SqlDbType.VarChar)
        Dim p16 As New SqlParameter("@AnaWWW", SqlDbType.VarChar)
        Dim p17 As New SqlParameter("@AnaEmail", SqlDbType.VarChar)
        Dim p18 As New SqlParameter("@AnaResp", SqlDbType.VarChar)
        Dim p19 As New SqlParameter("@AnaNoRubrica", SqlDbType.Bit)

        p1.Value = TextEdit1.Text & " " & TextEdit2.Text
        p9.Value = TextEdit1.Text
        p10.Value = TextEdit2.Text
        p2.Value = TextEdit3.Text
        p3.Value = TextEdit5.Text
        p4.Value = TextEdit4.Text
        p5.Value = TextEdit6.Text
        p6.Value = TextEdit7.Text
        p7.Value = TextEdit8.Text
        p8.Value = TextEdit00.Text
        p11.Value = TextEdit9.Text
        p12.Value = TextEdit10.Text
        p13.Value = TextEdit11.Text
        p14.Value = TextEdit12.Text
        p15.Value = TextEdit13.Text
        p16.Value = TextEdit16.Text
        p17.Value = TextEdit15.Text
        p18.Value = TextEdit14.Text
        p19.Value = CheckButton1.Checked

        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.Parameters.Add(p6)
        Cmd.Parameters.Add(p7)
        Cmd.Parameters.Add(p8)
        Cmd.Parameters.Add(p9)
        Cmd.Parameters.Add(p10)
        Cmd.Parameters.Add(p11)
        Cmd.Parameters.Add(p12)
        Cmd.Parameters.Add(p13)
        Cmd.Parameters.Add(p14)
        Cmd.Parameters.Add(p15)
        Cmd.Parameters.Add(p16)
        Cmd.Parameters.Add(p17)
        Cmd.Parameters.Add(p18)
        Cmd.Parameters.Add(p19)

        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ScriviCli()
        Dim scrivi As String = ""
        If Clie = False Then
            If RILEGGICLI() = 0 Then
                scrivi = "INSERT into TbCli (ClCod,ClPagam,ClAbi,ClCab,ClCC,ClCinEur,ClCin,ClCodBan,ClCntRid,ClAttivo,ClBlackList,Cl1RidData,ClDtnCap,ClDtnCitta,ClDtnProv,ClDtnData,ClAnteIndirizzo,ClAnteCap,ClAnteCitta,ClAnteProv,ClDtnSex," _
                    & " ClCoCognomeNome,ClCoCodFisc,ClCoCap,ClCoCitta,ClCoProv,ClCoSex,ClCoDtnData,ClCoAnteIndirizzo,ClCoAnteCap,ClCoAnteCitta,ClCoAnteProv)" _
                    & " VALUES (@ClCod,@ClPagam,@ClAbi,@ClCab,@ClCC,@ClCinEur,@ClCin,@ClCodBan,@ClCntRid,@ClAttivo,@ClBlackList,@Cl1RidData,@ClDtnCap,@ClDtnCitta,@ClDtnProv,@ClDtnData,@ClAnteIndirizzo,@ClAnteCap,@ClAnteCitta,@ClAnteProv,@ClDtnSex," _
                    & "@ClCoCognomeNome,@ClCoCodFisc,@ClCoCap,@ClCoCitta,@ClCoProv,@ClCoSex,@ClCoDtnData,@ClCoAnteIndirizzo,@ClCoAnteCap,@ClCoAnteCitta,@ClCoAnteProv)"

            Else
                scrivi = "Update TbCli set ClPagam=@ClPagam,ClAbi=@ClAbi,ClCab=@ClCab,ClCC=@ClCC,ClCinEur=@ClCinEur,ClCin=@ClCin,ClCodBan=@ClCodBan,ClCntRid=@ClCntRid,ClAttivo=@ClAttivo,ClBlackList=@ClBlackList, " _
                    & "Cl1RidData=@Cl1RidData,ClDtnCap=@ClDtnCap,ClDtnCitta=@ClDtnCitta,ClDtnProv=@ClDtnProv,ClDtnData=@ClDtnData,ClAnteIndirizzo=@ClAnteIndirizzo,ClAnteCap=@ClAnteCap,ClAnteCitta=@ClAnteCitta,ClAnteProv=@ClAnteProv,ClDtnSex=@ClDtnSex," _
                    & "ClCoCognomeNome=@ClCoCognomeNome,ClCoCodFisc=@ClCoCodFisc,ClCoCap=@ClCoCap,ClCoCitta=@ClCoCitta,ClCoProv=@ClCoProv,ClCoSex=@ClCoSex,ClCoDtnData=@ClCoDtnData,ClCoAnteIndirizzo=@ClCoAnteIndirizzo,ClCoAnteCap=@ClCoAnteCap,ClCoAnteCitta=@ClCoAnteCitta,ClCoAnteProv=@ClCoAnteProv where ClCod = @ClCod "
            End If
        Else
            scrivi = "Update TbCli set ClPagam=@ClPagam,ClAbi=@ClAbi,ClCab=@ClCab,ClCC=@ClCC,ClCinEur=@ClCinEur,ClCin=@ClCin,ClCodBan=@ClCodBan,ClCntRid=@ClCntRid,ClAttivo=@ClAttivo,ClBlackList=@ClBlackList, " _
                   & "Cl1RidData=@Cl1RidData,ClDtnCap=@ClDtnCap,ClDtnCitta=@ClDtnCitta,ClDtnProv=@ClDtnProv,ClDtnData=@ClDtnData,ClAnteIndirizzo=@ClAnteIndirizzo,ClAnteCap=@ClAnteCap,ClAnteCitta=@ClAnteCitta,ClAnteProv=@ClAnteProv,ClDtnSex=@ClDtnSex," _
                   & "ClCoCognomeNome=@ClCoCognomeNome,ClCoCodFisc=@ClCoCodFisc,ClCoCap=@ClCoCap,ClCoCitta=@ClCoCitta,ClCoProv=@ClCoProv,ClCoSex=@ClCoSex,ClCoDtnData=@ClCoDtnData,ClCoAnteIndirizzo=@ClCoAnteIndirizzo,ClCoAnteCap=@ClCoAnteCap,ClCoAnteCitta=@ClCoAnteCitta,ClCoAnteProv=@ClCoAnteProv where ClCod = @ClCod "
        End If

        Cmd = New SqlCommand(scrivi, cnDb)

        Dim p1 As New SqlParameter("@ClCod", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@ClPagam", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@ClAbi", SqlDbType.Int)
        Dim p4 As New SqlParameter("@ClCab", SqlDbType.Int)
        Dim p5 As New SqlParameter("@ClCC", SqlDbType.VarChar)
        Dim p6 As New SqlParameter("@ClCinEur", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@ClCin", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@ClCodBan", SqlDbType.SmallInt)
        Dim p9 As New SqlParameter("@ClCntRid", SqlDbType.VarChar)
        Dim p10 As New SqlParameter("@ClAttivo", SqlDbType.Bit)
        Dim p11 As New SqlParameter("@ClBlackList", SqlDbType.Bit)
        Dim p12 As New SqlParameter("@Cl1RidData", SqlDbType.SmallDateTime)
        Dim p13 As New SqlParameter("@ClDtnCap", SqlDbType.VarChar)
        Dim p14 As New SqlParameter("@ClDtnCitta", SqlDbType.VarChar)
        Dim p15 As New SqlParameter("@ClDtnProv", SqlDbType.VarChar)
        Dim p16 As New SqlParameter("@ClDtnData", SqlDbType.SmallDateTime)
        Dim p17 As New SqlParameter("@ClAnteIndirizzo", SqlDbType.VarChar)
        Dim p18 As New SqlParameter("@ClAnteCap", SqlDbType.VarChar)
        Dim p19 As New SqlParameter("@ClAnteCitta", SqlDbType.VarChar)
        Dim p20 As New SqlParameter("@ClAnteProv", SqlDbType.VarChar)
        Dim p21 As New SqlParameter("@ClDtnSex", SqlDbType.VarChar)

        Dim p22 As New SqlParameter("@ClCoCognomeNome", SqlDbType.VarChar)
        Dim p23 As New SqlParameter("@ClCoCodFisc", SqlDbType.VarChar)
        Dim p24 As New SqlParameter("@ClCoCap", SqlDbType.VarChar)
        Dim p25 As New SqlParameter("@ClCoCitta", SqlDbType.VarChar)
        Dim p26 As New SqlParameter("@ClCoProv", SqlDbType.VarChar)
        Dim p27 As New SqlParameter("@ClCoSex", SqlDbType.VarChar)
        Dim p28 As New SqlParameter("@ClCoDtnData", SqlDbType.SmallDateTime)
        Dim p29 As New SqlParameter("@ClCoAnteIndirizzo", SqlDbType.VarChar)
        Dim p30 As New SqlParameter("@ClCoAnteCap", SqlDbType.VarChar)
        Dim p31 As New SqlParameter("@ClCoAnteCitta", SqlDbType.VarChar)
        Dim p32 As New SqlParameter("@ClCoAnteProv", SqlDbType.VarChar)


        p1.Value = TextEdit00.Text
        p2.Value = Val(TextEdit17.Text)
        p3.Value = Val(TextEdit20.Text)
        p4.Value = Val(TextEdit19.Text)
        p5.Value = TextEdit18.Text.PadLeft(12, "0")
        p6.Value = Val(TextEdit22.Text)
        p7.Value = TextEdit21.Text.Trim
        If ComboBoxEdit4.SelectedIndex = -1 Then
            p8.Value = 0
        Else
            p8.Value = Val(Mid(ComboBoxEdit4.Properties.Items(ComboBoxEdit4.SelectedIndex), 1, 3))
        End If
        p9.Value = TextEdit25.Text
        p10.Value = CheckButton2.Checked
        p11.Value = CheckEdit3.Checked
        If DateEdit1.EditValue Is Nothing Then p12.Value = System.DBNull.Value Else p12.Value = DateEdit1.EditValue
        p13.Value = TextEdit26.EditValue
        p14.Value = TextEdit27.EditValue
        p15.Value = TextEdit28.EditValue
        If DateEdit2.EditValue Is Nothing Then p16.Value = System.DBNull.Value Else p16.Value = DateEdit2.EditValue
        p17.Value = TextEdit29.EditValue
        p18.Value = TextEdit30.EditValue
        p19.Value = TextEdit31.EditValue
        p20.Value = TextEdit32.EditValue
        p21.Value = TextEdit33.EditValue

        p22.Value = TextEdit34.EditValue
        p23.Value = TextEdit35.EditValue
        p24.Value = TextEdit36.EditValue
        p25.Value = TextEdit37.EditValue
        p26.Value = TextEdit38.EditValue
        p27.Value = TextEdit39.EditValue
        If DateEdit3.EditValue Is Nothing Then p28.Value = System.DBNull.Value Else p28.Value = DateEdit3.EditValue
        p29.Value = TextEdit40.EditValue
        p30.Value = TextEdit41.EditValue
        p31.Value = TextEdit42.EditValue
        p32.Value = TextEdit43.EditValue

        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.Parameters.Add(p6)
        Cmd.Parameters.Add(p7)
        Cmd.Parameters.Add(p8)
        Cmd.Parameters.Add(p9)
        Cmd.Parameters.Add(p10)
        Cmd.Parameters.Add(p11)
        Cmd.Parameters.Add(p12)
        Cmd.Parameters.Add(p13)
        Cmd.Parameters.Add(p14)
        Cmd.Parameters.Add(p15)
        Cmd.Parameters.Add(p16)
        Cmd.Parameters.Add(p17)
        Cmd.Parameters.Add(p18)
        Cmd.Parameters.Add(p19)
        Cmd.Parameters.Add(p20)
        Cmd.Parameters.Add(p21)
        Cmd.Parameters.Add(p22)
        Cmd.Parameters.Add(p23)
        Cmd.Parameters.Add(p24)
        Cmd.Parameters.Add(p25)
        Cmd.Parameters.Add(p26)
        Cmd.Parameters.Add(p27)
        Cmd.Parameters.Add(p28)
        Cmd.Parameters.Add(p29)
        Cmd.Parameters.Add(p30)
        Cmd.Parameters.Add(p31)
        Cmd.Parameters.Add(p32)

        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub scrivi_ultimi(ByVal CLFO As String)
        Dim i As Int16
        Dim scrivi As Boolean


        gruppi = leggi_ultimi("CL")

        scrivi = False
        For i = 0 To 8
            If Val(TextEdit00.Text) <= gruppi.al(i) Then
                If Val(TextEdit00.Text) > gruppi.ult(i) Then
                    gruppi.ult(i) = Val(TextEdit00.Text)
                    scrivi = True
                End If
                Exit For
            End If
        Next

        If scrivi = False Then
            Exit Sub
        End If

        Dim strscrivi As String
        strscrivi = "update TbGrp set GrpUl1=@GrpUl1, GrpUl2=@GrpUl2, GrpUl3=@GrpUl3, GrpUl4=@GrpUl4, GrpUl5=@GrpUl5, GrpUl6=@GrpUl6,GrpUl7=@GrpUl7, GrpUl8=@GrpUl8, GrpUl9=@GrpUl9 where GrpCod=@GrpCod"
        Cmd = New SqlCommand(strscrivi, cnCo)

        Dim p1 As New SqlParameter("@GrpUl1", SqlDbType.Int)
        Dim p2 As New SqlParameter("@GrpUl2", SqlDbType.Int)
        Dim p3 As New SqlParameter("@GrpUl3", SqlDbType.Int)
        Dim p4 As New SqlParameter("@GrpUl4", SqlDbType.Int)
        Dim p5 As New SqlParameter("@GrpUl5", SqlDbType.Int)
        Dim p6 As New SqlParameter("@GrpUl6", SqlDbType.Int)
        Dim p7 As New SqlParameter("@GrpUl7", SqlDbType.Int)
        Dim p8 As New SqlParameter("@GrpUl8", SqlDbType.Int)
        Dim p9 As New SqlParameter("@GrpUl9", SqlDbType.Int)
        Dim p10 As New SqlParameter("@GrpCod", SqlDbType.Char)

        p1.Value = gruppi.ult(0)
        p2.Value = gruppi.ult(1)
        p3.Value = gruppi.ult(2)
        p4.Value = gruppi.ult(3)
        p5.Value = gruppi.ult(4)
        p6.Value = gruppi.ult(5)
        p7.Value = gruppi.ult(6)
        p8.Value = gruppi.ult(7)
        p9.Value = gruppi.ult(8)
        p10.Value = CLFO
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.Parameters.Add(p6)
        Cmd.Parameters.Add(p7)
        Cmd.Parameters.Add(p8)
        Cmd.Parameters.Add(p9)
        Cmd.Parameters.Add(p10)
        Try
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ButtonF6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF6.Click
        If Val(TextEdit00.Text) < 1000 Then TextEdit00.Focus() : Exit Sub
        Dim annota As New Note
        annota.PathFile = FileNote
        annota.Location = New Point(GroupControl23.Location.X + 32, GroupControl23.Location.Y)
        annota.Size = New Size(958, 408)
        annota.ShowDialog()
        If File.Exists(FileNote) = True Then ErrorProvider1.SetError(ButtonF6, "Sono presenti Note!") Else ErrorProvider1.SetError(ButtonF6, "")
        TextEdit1.Focus()
    End Sub

    Private Sub Clienti_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            If e.KeyData = Keys.F3 Then
                ButtonF3.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F5 Then
                ButtonF5.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F6 Then
                ButtonF6.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F9 Then
                ButtonF9.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F11 Then
                ButtonF11.PerformClick()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub ComboBox1_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit4.LostFocus
        SettaComboEdit(sender, sender.Text, 2)
    End Sub

    Private Sub ComboBox1_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit4.SelectedValueChanged
        SelectNextControl(sender, True, True, True, True)
    End Sub
    Private Sub GridView4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.Click
        If iset > -1 Then
            RwAna = GridView4.GetRow(iset)
            TextEdit00.EditValue = RwAna("AnaCod")
            SelectNextControl(TbLeggiCod, True, True, True, True)
        End If
    End Sub
    Private Sub gridControl3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl3.MouseMove
        ShowHitInfo(GridView4.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl3.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub
    Private Sub Memoedit2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit17.LostFocus
        PagCod = Val(TextEdit17.Text)
        RilevaPagamento()
    End Sub
    Private Sub TextEdit20_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit20.LostFocus, TextEdit19.LostFocus
        If Val(TextEdit20.Text) > 0 And Val(TextEdit19.Text) > 0 Then
            TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))
        Else
            TextEdit24.Text = ""
        End If
    End Sub

    Sub HyperLinkEdit1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkEdit1.Click
        Dim rabibanca As abibanca
        rabibanca = Ricerche.LnkAppoggio(Val(TextEdit20.Text), Val(TextEdit19.Text))
        If rabibanca.abi <> 0 And rabibanca.cab <> 0 Then
            TextEdit20.Text = rabibanca.abi
            TextEdit19.Text = rabibanca.cab
            TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))
            SelectNextControl(TextEdit18, True, True, True, True)
        End If
    End Sub
    Sub HyperLinkEdit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkEdit2.Click
        If Val(TextEdit17.Text) > 0 Then PagCod = Val(TextEdit17.Text)
        PagCod = Ricerche.LnkCodPag()
        If PagCod = 0 Then PagCod = Val(TextEdit17.Text)
        RilevaPagamento() : HyperLinkEdit2.Reset()
    End Sub
    Sub RilevaPagamento()
        MemoEdit2.Text = LeggiPagam(PagCod)
        If PagCod > 0 Then
            TextEdit17.Text = PagCod
            If MemoEdit2.Text.Length = 0 Then
                SelectNextControl(TextEdit17, True, True, True, True)
            Else
                SelectNextControl(TextEdit23, True, True, True, True)
            End If
        End If
    End Sub
    'VOISpeed
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        ChiamaNumero(TextEdit10.Text, CType(sender, PictureBox))
    End Sub

    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox3.Click
        ChiamaNumero(TextEdit11.Text, CType(sender, PictureBox))
    End Sub

    Private Sub PictureBox5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox5.Click
        ChiamaNumero(TextEdit12.Text, CType(sender, PictureBox))
    End Sub

    Private Sub ChiamaNumero(ByVal Numero As String, ByRef Controllo As Object)
        If ControlloVOISpeed() = False Then InfoVoiSpeed() : Return
        If Not Numero.Trim > "" Then Return
        EffettuaChiamata(Numero.Trim)
        Controllo.SendToBack()
    End Sub
    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click, PictureBox4.Click, PictureBox6.Click
        If ControlloVOISpeed() = False Then InfoVoiSpeed() : Return
        ChiudiChiamata()
        CType(sender, PictureBox).SendToBack()
    End Sub
    Private Sub InfoVoiSpeed()
        Dim frm As New InfoVoispeed
        frm.ShowDialog()
    End Sub

    Private Sub PictureBox9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox9.Click
        If Not TextEdit15.Text.Trim > "" Then Return
        Process.Start("mailto:" & TextEdit15.Text.Trim)
    End Sub

    Private Sub PictureBox8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox8.Click
        If Not TextEdit16.Text.Trim > "" Then Return
        Process.Start("iexplore", TextEdit16.Text.Trim)
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        If Val(Ultimo) > 1000 Then
            TextEdit00.EditValue = Ultimo
            SelectNextControl(TbLeggiCod, True, True, True, True)
            Exit Sub
        End If
    End Sub


    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If Val(TextEdit00.Text) >= MIGLIO Or Val(TextEdit00.Text) < 1001 Then
            TextEdit00.Focus()
            Return
        End If
        If Controllod() = False Then Exit Sub
        EliminaConto()
        ButtonF5.PerformClick()
    End Sub
    Private Function Controllod() As Boolean
        Controllod = True
        Dim Str As String
        Dim CONTO As String = TextEdit00.Text
        Str = "Select top 1 PRKCONTO from TbPrk where PrkConto = '" & CONTO & "'"
        Dim cmd As New SqlCommand(Str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            Controllod = False
        End If
        dataRd.Close()
        If Controllod = False Then
            MessageBox.Show("Conto Movimentato!!!" & Chr(13), "Impossibile Annullare", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Function


    Sub EliminaConto()
        Dim cod As String = TextEdit00.Text
        Dim box As Object
        box = MessageBox.Show("Vuoi eliminare il Codice?", "ELIMINA CLIENTI", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
        If box <> DialogResult.Yes Then GoTo FineOp
        Cmd = New SqlCommand("delete from TbCli where ClCod ='" & cod & "'", cnDb)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("delete from TbAna where AnaGrp = 'CL' and AnaCod ='" & cod & "'", cnVd)
        Cmd.ExecuteNonQuery()
FineOp:
    End Sub
#End Region

#Region "MODULO DI STAMPA"

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        UserPrint = New DxUserSelf
        UserPrint.Name = Me.Name
        UserPrint.Parent = XtraTabPage2
        XtraTabControl1.SelectedTabPageIndex = 1
    End Sub

    Private Sub UserPrint_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserPrint.Disposed
        If XtraTabControl1.SelectedTabPageIndex = -1 Then Exit Sub
        XtraTabControl1.SelectedTabPageIndex = 0
    End Sub

#End Region
#Region "ALTRI DATI PER LA COMUNITA'"
    Private Sub TextEdit13_Leave(sender As Object, e As System.EventArgs) Handles TextEdit13.Leave
        TextEdit14.Focus()
    End Sub
    Private Sub TextEdit32_Leave(sender As Object, e As System.EventArgs) Handles TextEdit32.Leave
        ButtonF11.Focus()
    End Sub
#End Region

End Class