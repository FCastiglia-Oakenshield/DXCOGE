Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Public Class Fornitori
    Structure gruppiclfo
        Dim CliMax As Integer
        Dim al() As Integer
        Dim ult() As Integer
    End Structure
    Dim gruppi As New gruppiclfo
    Dim Forn, Ana As Boolean
    Dim enasarco, PagCod As Int16
    Dim FileNote As String
    Dim Admin As Boolean
    Dim TbAna As DataTable
    Dim DaAna As SqlDataAdapter
    Dim RwAna As DataRowView
    Dim Ultimo As String = ""
    Dim MIGLIO As Integer = -1
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim WithEvents UserPrint As DevExpress.XtraEditors.XtraUserControl


    Private Sub AnagFor_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
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
        ComboBoxEdit1.Properties.Items.Clear()
        ComboBoxEdit1.Properties.Items.Add("")
        Cmd = New SqlCommand(" SELECT * from TbEnasarco Order by EnaCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("EnaCod") & " - " & Format(dataRd.Item("EnaFinoA"), "#,###,###,##0.00"))
        End While
        dataRd.Close()
    End Sub
    Private Sub pulizia()
        Dim h1 As Object = HyperLinkEdit1.EditValue
        PulisciDxCampi(Me)
        HyperLinkEdit1.EditValue = h1
        iset = -1 : PagCod = -1
        GroupControl17.Enabled = True
        ErrorProvider1.SetError(ButtonF6, "")
        TextEdit3.ErrorText = "" : TextEdit5.ErrorText = ""
        cargo_ultimi()
        TextEdit00.Enabled = True
        PictureBox1.BringToFront()
        PictureBox3.BringToFront()
        PictureBox5.BringToFront()
        TextEdit00.Focus()
        CheckButton2.Checked = True : CheckButton2.Checked = False
    End Sub
    Private Function LeggiEnasarco(ByRef cod As String) As String
        Cmd = New SqlCommand("Select * from TbEnasarco where EnaCod=" & Val(cod), cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Dim a As String = (dataRd.Item("EnaFinoA"))
            dataRd.Close()
            Return a
        End If
        dataRd.Close()
        cod = ""
        Return ""
    End Function

    Private Sub cargo_ultimi()
        gruppi = leggi_ultimi("FO")
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
        Dim Str As String = "Select * from TbAna where AnaGrp='FO' order by AnaDesc"
        TbAna = New DataTable("TbAna")
        DaAna = New SqlDataAdapter(Str, cnVd)
        DaAna.Fill(TbAna)
        GridControl3.DataSource = TbAna
        GridView4.ActiveFilterString = ""
    End Sub

    Private Sub leggiNote()
        FileNote = vedinote("FO", TextEdit00.Text)
        If File.Exists(FileNote) = True Then ErrorProvider1.SetError(ButtonF6, "Sono presenti Note!") Else ErrorProvider1.SetError(ButtonF6, "")
    End Sub

    Private Sub leggiAna(ByVal codice As String)
        Cmd = New SqlCommand("select * from TbAna where AnaGrp = 'FO' and AnaCod ='" & codice & "'", cnVd)
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
        If Codfisc(TextEdit5.Text) = False Then TextEdit5.ErrorText = " ! Errata" Else TextEdit5.ErrorText = ""
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
        If Val(TextEdit00.Text) > gruppi.CliMax And Val(TextEdit00.Text) < 99999 Then
            leggiNote()
            If iset > -1 Then GridView4.UnselectRow(iset)
            leggiFor(TextEdit00.Text)
            leggiAna(TextEdit00.Text)
            SelectNextControl(TextEdit1, True, True, True, True)
            Ultimo = TextEdit00.EditValue
        End If
    End Sub

    Private Sub leggiFor(ByVal codice As String)
        Cmd = New SqlCommand("select * from TbFor where FoCod ='" & codice & "'", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Forn = True
            CaricaFor()
        Else
            Forn = False
            AzzeraFor()
        End If
        dataRd.Close()

        MemoEdit2.Text = LeggiPagam(Val(TextEdit17.Text))
        TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))

        GroupControl22.Enabled = True
        GroupControl17.Enabled = True

        FileNote = vedinote("FO", TextEdit00.Text)
        If File.Exists(FileNote) = True Then ErrorProvider1.SetError(ButtonF6, "Sono presenti Note!") Else ErrorProvider1.SetError(ButtonF6, "")
        ButtonF6.Enabled = Forn
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

    Private Sub CaricaFor()
        TextEdit17.Text = dataRd.Item("FoPagam")
        TextEdit20.Text = dataRd.Item("FoAbi")
        TextEdit19.Text = dataRd.Item("FoCab")
        ComboBoxEdit1.SelectedIndex = dataRd.Item("FoEnasarco")
        TextEdit18.Text = dataRd.Item("FoCC")
        If dataRd.Item("FoCinEur") Is DBNull.Value Then TextEdit22.Text = "" Else TextEdit22.Text = dataRd.Item("FoCinEur")
        If dataRd.Item("FoCin") Is DBNull.Value Then TextEdit21.Text = "" Else TextEdit21.Text = dataRd.Item("FoCin")
        ComboBoxEdit4.SelectedIndex = SettaComboEdit(ComboBoxEdit4, dataRd.Item("FoCodBan").ToString.PadLeft(3, " "), 3)
        CheckEdit1.Checked = dataRd.Item("FoSoggRit")
        If dataRd.Item("FoAttivo") Is DBNull.Value Then CheckButton2.Checked = True Else CheckButton2.Checked = dataRd.Item("FoAttivo")
        CheckEdit3.Checked = IIf(IsDBNull(dataRd.Item("FoBlackList")), False, dataRd.Item("FoBlackList"))
    End Sub
    Private Sub AzzeraFor()
        TextEdit17.Text = "000"
        TextEdit18.Text = ""
        TextEdit19.Text = ""
        TextEdit20.Text = ""
        TextEdit21.Text = ""
        TextEdit22.Text = ""
        TextEdit23.Text = "IT"
        TextEdit24.Text = ""
        MemoEdit2.Text = ""
        CheckButton2.Checked = False
        CheckEdit3.Checked = False
        enasarco = 0
        ComboBoxEdit4.SelectedIndex = 0
        ComboBoxEdit1.SelectedIndex = 0
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
            CheckButton2.ToolTip = "FORNITORE ATTIVO"
        Else
            CheckButton2.ImageIndex = 18
            CheckButton2.ToolTip = "FORNITORE NON ATTIVO"
        End If
    End Sub
    Private Sub TextEdit3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit3.Validated
        output_piva()
        Dim testPiva As Int32 = EsistePiva(TextEdit00.Text, TextEdit3.Text, "FO", Me.Location.X + 456, Me.Location.Y + 84)
        If testPiva > 0 Then
            pulizia()
            TextEdit00.Text = testPiva.ToString("00000")
            leggiNote()
            leggiFor(TextEdit00.Text)
            leggiAna(TextEdit00.Text)
            SelectNextControl(TextEdit1, True, True, True, True)
        End If
    End Sub

    Private Sub TextEdit5_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit5.LostFocus
        output_codfisc()
    End Sub

    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        pulizia() : PopolaGrid()
        If iset > -1 Then GridView4.UnselectRow(iset)
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Val(TextEdit00.Text) <= MIGLIO Then
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
            MoltoCritico(Mail, "INSERIMENTO FORNITORI")
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
        ScriviFor()
        scrivi_ultimi("FO")
    End Sub
    Function RILEGGIANA() As Integer
        Cmd = New SqlCommand("SELECT isnull(ANACOD,0) from TBANA WHERE anagrp = 'FO' AND ANACOD = '" & TextEdit00.Text & "'", cnVd)
        RILEGGIANA = Cmd.ExecuteScalar
    End Function
    Function RILEGGIFOR() As Integer
        Cmd = New SqlCommand("SELECT isnull(FOCOD,0) from TBFOR WHERE FOCOD = '" & TextEdit00.Text & "'", cnDb)
        RILEGGIFOR = Cmd.ExecuteScalar
    End Function
    Private Sub ScriviAna()
        Dim scrivi As String = ""

        If Ana = False Then
            If RILEGGIANA() = 0 Then
                scrivi = "INSERT INTO TbAna (AnaDesc,AnaPiva,AnaCfis,AnaIndirizzo,AnaCap,AnaCitta,AnaProv,Anatel1,Anatel2,AnaTel3,AnaWWW,AnaEmail,AnaGrp,AnaCod,AnaResp,AnaNote,AnaFax,AnaRag1,AnaRag2,AnaPivaEst,AnaNoRubrica)  values(@AnaDesc,@AnaPiva,@AnaCfis,@AnaIndirizzo,@AnaCap,@AnaCitta,@AnaProv,@Anatel1,@Anatel2,@AnaTel3,@AnaWWW,@AnaEmail,'FO',@AnaCod,@AnaResp,'',@AnaFax,@AnaRag1,@AnaRag2,@AnaPivaEst,@AnaNoRubrica)"
            Else
                scrivi = "Update TbAna set AnaDesc=@AnaDesc,AnaPiva=@AnaPiva,AnaCfis=@AnaCFis,AnaIndirizzo=@AnaIndirizzo,AnaCap=@AnaCap,AnaCitta=@AnaCitta,AnaProv=@AnaProv,Anatel1=@AnaTel1,Anatel2=@AnaTel2,AnaTel3=@AnaTel3,AnaWWW=@AnaWWW,AnaEmail=@AnaEmail,AnaResp=@AnaResp,AnaFax=@AnaFax,AnaRag1=@AnaRag1,AnaRag2=@AnaRag2,AnaPivaEst=@AnaPivaEst,AnaNoRubrica=@AnaNoRubrica where AnaCod = @AnaCod and AnaGrp = 'FO'"
            End If
        Else
            scrivi = "Update TbAna set AnaDesc=@AnaDesc,AnaPiva=@AnaPiva,AnaCfis=@AnaCFis,AnaIndirizzo=@AnaIndirizzo,AnaCap=@AnaCap,AnaCitta=@AnaCitta,AnaProv=@AnaProv,Anatel1=@AnaTel1,Anatel2=@AnaTel2,AnaTel3=@AnaTel3,AnaWWW=@AnaWWW,AnaEmail=@AnaEmail,AnaResp=@AnaResp,AnaFax=@AnaFax,AnaRag1=@AnaRag1,AnaRag2=@AnaRag2,AnaPivaEst=@AnaPivaEst,AnaNoRubrica=@AnaNoRubrica where AnaCod = @AnaCod and AnaGrp = 'FO'"
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

    Private Sub ScriviFor()
        Dim scrivi As String = ""

        If Forn = False Then
            If RILEGGIFOR() = 0 Then
                scrivi = "INSERT into Tbfor (FoCod,FoPagam,FoAbi,FoCab,FoEnasarco,FoCC,FoCinEur,FoCin,FocodBan,FoSoggRit,FoAttivo,FoBlackList) VALUES (@Focod,@FoPagam,@FoAbi,@FoCab,@FoEnasarco,@FoCC,@FoCinEur,@FoCin,@FoCodBan,@FoSoggRit,@FoAttivo,@FoBlackList)"
            Else
                scrivi = "Update TbFor set FoPagam=@FoPagam,FoAbi=@FoAbi,FoCab=@FoCab,FoEnasarco=@FoEnasarco,FoCC=@FoCC,FoCinEur=@FoCinEur,FoCin=@FoCin,Focodban=@FocodBan,FoSoggRit=@FoSoggRit,FoAttivo=@FoAttivo,FoBlackList=@FoBlackList where FoCod = @FoCod "
            End If
        Else
            scrivi = "Update TbFor set FoPagam=@FoPagam,FoAbi=@FoAbi,FoCab=@FoCab,FoEnasarco=@FoEnasarco,FoCC=@FoCC,FoCinEur=@FoCinEur,FoCin=@FoCin,Focodban=@FocodBan,FoSoggRit=@FoSoggRit,FoAttivo=@FoAttivo,FoBlackList=@FoBlackList where FoCod = @FoCod "
        End If

        Cmd = New SqlCommand(scrivi, cnDb)

        Dim p1 As New SqlParameter("@FoCod", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@FoPagam", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@FoAbi", SqlDbType.Int)
        Dim p4 As New SqlParameter("@FoCab", SqlDbType.Int)
        Dim p5 As New SqlParameter("@FoEnasarco", SqlDbType.SmallInt)
        Dim p6 As New SqlParameter("@FoCC", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@FoCinEur", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@FoCin", SqlDbType.VarChar)
        Dim p9 As New SqlParameter("@FoCodBan", SqlDbType.SmallInt)
        Dim p10 As New SqlParameter("@FoSoggRit", SqlDbType.Bit)
        Dim p16 As New SqlParameter("@FoAttivo", SqlDbType.Bit)
        Dim p17 As New SqlParameter("@FoBlackList", SqlDbType.Bit)


        p1.Value = TextEdit00.Text
        p2.Value = Val(TextEdit17.Text)
        p3.Value = Val(TextEdit20.Text)
        p4.Value = Val(TextEdit19.Text)
        If ComboBoxEdit1.SelectedIndex = -1 Then p5.Value = 0 Else p5.Value = ComboBoxEdit1.SelectedIndex
        p6.Value = TextEdit18.Text.PadLeft(12, "0")
        p7.Value = Val(TextEdit22.Text)
        p8.Value = TextEdit21.Text.Trim
        If ComboBoxEdit4.SelectedIndex = -1 Then
            p9.Value = 0
        Else
            p9.Value = Val(Mid(ComboBoxEdit4.Properties.Items(ComboBoxEdit4.SelectedIndex), 1, 3))
        End If
        p10.Value = CheckEdit1.Checked
        p16.Value = CheckButton2.Checked
        p17.Value = CheckEdit3.Checked
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
        Cmd.Parameters.Add(p16)
        Cmd.Parameters.Add(p17)

        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub scrivi_ultimi(ByVal CLFO As String)
        Dim i As Int16
        Dim scrivi As Boolean


        gruppi = leggi_ultimi("FO")

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

    Private Sub Fornitori_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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
    Sub HyperLinkEdit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GroupControl20.Click
        If Val(TextEdit17.Text) > 0 Then PagCod = Val(TextEdit17.Text)
        PagCod = Ricerche.LnkCodPag()
        If PagCod = 0 Then PagCod = Val(TextEdit17.Text)
        RilevaPagamento()
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
        If Val(TextEdit00.Text) <= MIGLIO Then
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
        box = MessageBox.Show("Vuoi eliminare il Codice?", "ELIMINA FORNITORI", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
        If box <> DialogResult.Yes Then GoTo FineOp
        Cmd = New SqlCommand("delete from TbFor where FoCod ='" & cod & "'", cnDb)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("delete from TbAna where AnaGrp = 'FO' and AnaCod ='" & cod & "'", cnVd)
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

End Class