Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports DevExpress.XtraEditors

Public Class DxAziForm
    Dim EsisteAzi As Boolean
    Dim EsisteAna As Boolean
    Dim R As Int16 = -1
    Dim AnnoDa As Int16 = Today.Year
    Dim UltAnno As Int16 = Today.Year
    Dim NoJob As Boolean = False

    Private Sub DxAziForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        PuliziaAna(True)
        PuliziaAzi(True)
        ControllaContratto()
        TextEdit1.Focus()
    End Sub
    Sub ControllaContratto()
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 600", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            NoJob = dataRd.Item("Sel15")
        End While
        dataRd.Close()
    End Sub
    Private Sub PuliziaAna(ByVal puliscitutto As Boolean)
        TextEdit2.EditValue = ""
        TextEdit3.EditValue = ""
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = ""
        TextEdit6.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = ""
        TextEdit9.EditValue = ""
        TextEdit10.EditValue = ""
        TextEdit11.EditValue = ""
        TextEdit12.EditValue = ""
        TextEdit13.EditValue = ""
        TextEdit14.EditValue = ""
        TextEdit3.ErrorText = ""
        TextEdit5.ErrorText = ""
    End Sub
    Private Sub PuliziaAzi(ByVal puliscitutto As Boolean)
        'I pagina
        TextEdit15.EditValue = ""
        TextEdit16.EditValue = ""
        TextEdit17.EditValue = ""
        TextEdit18.EditValue = ""
        TextEdit19.EditValue = ""
        TextEdit20.EditValue = ""
        TextEdit21.EditValue = ""
        TextEdit22.EditValue = ""
        TextEdit23.EditValue = ""
        TextEdit24.EditValue = ""
        TextEdit25.EditValue = ""
        TextEdit26.EditValue = ""
        TextEdit27.EditValue = ""
        TextEdit28.EditValue = ""
        TextEdit29.EditValue = ""
        TextEdit30.EditValue = ""
        TextEdit31.EditValue = ""
        TextEdit32.EditValue = ""
        TextEdit33.EditValue = 0
        TextEdit36.EditValue = ""
        TextEdit37.EditValue = ""
        DateEdit1.EditValue = Nothing
        DateEdit2.EditValue = Nothing
        ComboBoxEdit2.SelectedIndex = -1
        ComboBoxEdit4.SelectedIndex = -1
        ComboBoxEdit7.SelectedIndex = -1
        RadioGroup1.SelectedIndex = 0
        ' II pagina
        ComboBoxEdit5.SelectedIndex = 0
        ComboBoxEdit6.SelectedIndex = 0
        DateEdit5.EditValue = Nothing
        TextEdit50.EditValue = ""
        TextEdit51.EditValue = CDec(0.0)
        TextEdit52.EditValue = ""
        TextEdit53.EditValue = ""
        TextEdit54.EditValue = ""
        TextEdit55.EditValue = ""
        TextEdit56.EditValue = ""
        TextEdit57.EditValue = ""
        TextEdit58.EditValue = ""
        TextEdit59.EditValue = ""
        TextEdit60.EditValue = ""
        TextEdit61.EditValue = ""
        TextEdit62.EditValue = ""
        TextEdit63.EditValue = ""
        TextEdit64.EditValue = ""
        TextEdit65.EditValue = ""
        TextEdit66.EditValue = ""
        TextEdit67.EditValue = ""
        ' III pagina
        CheckEdit5.Checked = False
        CheckEdit6.Checked = False
        DateEdit6.EditValue = Nothing
        TextEdit70.EditValue = ""
        TextEdit71.EditValue = ""
        TextEdit72.EditValue = ""
        TextEdit73.EditValue = ""
        TextEdit74.EditValue = ""
        TextEdit75.EditValue = ""
        TextEdit76.EditValue = ""
        TextEdit77.EditValue = ""
        TextEdit78.EditValue = ""
        TextEdit79.EditValue = ""
        TextEdit80.EditValue = ""
        TextEdit81.EditValue = ""
        TextEdit82.EditValue = ""
        TextEdit83.EditValue = ""
        TextEdit84.EditValue = ""
        TextEdit85.EditValue = ""
        TextEdit86.EditValue = ""
        TextEdit87.EditValue = ""
        TextEdit88.EditValue = ""
        TextEdit89.EditValue = ""
        TextEdit90.EditValue = ""
        TextEdit91.EditValue = ""
        TextEdit72.ErrorText = ""
        TextEdit83.ErrorText = ""
        ComboBoxEdit3.SelectedIndex = 0
        If puliscitutto = True Then
            PopolaCb4()
            PopolaCb2()
            PopolaCb1()
            If ComboBoxEdit1.Properties.Items.Count > 0 Then
                ComboBoxEdit1.SelectedIndex = 0
            Else
                CaricaPrimaVolta()
            End If
        End If
    End Sub
    Sub CaricaPrimaVolta()
        ComboBoxEdit1.Properties.Items.Add(Today.Year)
        TextEdit1.EditValue = "00001"
        ComboBoxEdit1.SelectedIndex = 0
    End Sub
    Private Sub PopolaCb4()
        ComboBoxEdit4.Properties.Items.Clear()
        ComboBoxEdit7.Properties.Items.Clear()
        Dim Str As String = "SELECT CiiCod,CiiCau from TbCii where CiiCod > 3 and CiiCod <> 45 and CiiCau >'' order by CiiCod"
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit4.Properties.Items.Add(dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau"))
            ComboBoxEdit7.Properties.Items.Add(dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau"))
        End While
        dataRd.Close()
    End Sub
    Private Sub PopolaCb2()
        ComboBoxEdit2.Properties.Items.Clear()
        Dim Str As String = "SELECT DISTINCT  ARTPID,ARTPSIGLA from TBARTP"
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit2.Properties.Items.Add(dataRd.Item("ArtPid").ToString.PadRight(6, " ") & " " & dataRd.Item("ARTPSIGLA"))
        End While
        dataRd.Close()
    End Sub

    Private Sub PopolaCb1()
        ComboBoxEdit1.Properties.Items.Clear()
        Dim str As String = "SELECT AziAnnoLavoro from TbAzi order by AziAnnoLavoro desc"
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
        End While
        dataRd.Close()
    End Sub
    Private Sub LetturaAzi()
        EsisteAzi = False
        Dim str As String = "SELECT * from VAziCsp where AziAnnoLavoro=" & ComboBoxEdit1.EditValue
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            CaricaElementiAzi()
            EsisteAzi = True
        End If
        dataRd.Close()

        'LetturaEse

        str = "SELECT * from TbEse where EseAnno=" & ComboBoxEdit1.EditValue
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            DateEdit1.EditValue = CDate(dataRd.Item("EseDal"))
            DateEdit2.EditValue = CDate(dataRd.Item("EseAl"))
            TextEdit15.EditValue = dataRd.Item("EseBilAp")
            TextEdit17.EditValue = dataRd.Item("EseBilChi")
            TextEdit19.EditValue = dataRd.Item("EsePP")
            TextEdit21.EditValue = dataRd.Item("EseUti")
            TextEdit23.EditValue = dataRd.Item("EsePer")
            TextEdit25.EditValue = dataRd.Item("EseSaDa")
            TextEdit26.EditValue = dataRd.Item("EseSaA")
            TextEdit27.EditValue = dataRd.Item("EseSpDa")
            TextEdit28.EditValue = dataRd.Item("EseSpA")
            TextEdit29.EditValue = dataRd.Item("EseCeDa")
            TextEdit30.EditValue = dataRd.Item("EseCeA")
            CheckEdit1.Checked = IIf(dataRd.Item("EseGioInt") Is DBNull.Value, False, dataRd.Item("EseGioInt"))
            TextEdit31.EditValue = IIf(CheckEdit1.Checked = True, IIf(dataRd.Item("EseGioDesc") Is DBNull.Value, "", dataRd.Item("EseGioDesc")), "")
            TextEdit32.EditValue = IIf(dataRd.Item("EseGioNFog") Is DBNull.Value, 0, dataRd.Item("EseGioNFog"))
            TextEdit33.EditValue = IIf(dataRd.Item("EseGioProg") Is DBNull.Value, 0, dataRd.Item("EseGioProg"))
            TextEdit36.EditValue = IIf(dataRd.Item("EseInvDesc") Is DBNull.Value, "", dataRd.Item("EseInvDesc"))
            TextEdit37.EditValue = IIf(dataRd.Item("EseInvNFog") Is DBNull.Value, 0, dataRd.Item("EseInvNFog"))
            RadioGroup1.SelectedIndex = dataRd.Item("EseFormato")
            If dataRd.Item("EseCausaleChiusuraConti") Is DBNull.Value Then
                ComboBoxEdit4.SelectedIndex = -1
            Else
                ComboBoxEdit4.SelectedIndex = SettaComboEdit(ComboBoxEdit4, dataRd.Item("EseCausaleChiusuraConti"), 0)
            End If
            ComboBoxEdit2.SelectedIndex = SettaComboEdit(ComboBoxEdit2, dataRd.Item("EseArtGiroconto"), 0)
            ComboBoxEdit7.SelectedIndex = SettaComboEdit(ComboBoxEdit7, dataRd.Item("EseGContoRCee"), 0)
            dataRd.Close()
            TextEdit16.EditValue = LeggiCpt(TextEdit15.EditValue)
            TextEdit18.EditValue = LeggiCpt(TextEdit17.EditValue)
            TextEdit20.EditValue = LeggiCpt(TextEdit19.EditValue)
            TextEdit22.EditValue = LeggiCpt(TextEdit21.EditValue)
            TextEdit24.EditValue = LeggiCpt(TextEdit23.EditValue)
        Else
            DateEdit1.EditValue = CDate("01/01/" & ComboBoxEdit1.EditValue)
            DateEdit2.EditValue = CDate("31/12/" & ComboBoxEdit1.EditValue)
        End If
        dataRd.Close()
    End Sub
#Region "Carica Elementi TbAzi"
    Private Sub CaricaElementiAzi() 'TbAzi (COGE)
        'Dati Azienda
        TextEdit1.EditValue = dataRd.Item("AziCod")
        ComboBoxEdit5.SelectedIndex = dataRd.Item("AziRegimeIva")
        TextEdit50.EditValue = dataRd.Item("AziUfficioIva")
        TextEdit51.EditValue = CDec(dataRd.Item("AziEsente"))
        TextEdit52.EditValue = dataRd.Item("AziNaturaIva")
        TextEdit53.EditValue = dataRd.Item("AziNaturaRedditi")
        TextEdit54.EditValue = dataRd.Item("AziCodAttivita")
        TextEdit55.EditValue = dataRd.Item("AziDescAttivita")
        TextEdit56.EditValue = dataRd.Item("AziCodIstat")
        TextEdit57.EditValue = dataRd.Item("IstatDesc")
        TextEdit58.EditValue = dataRd.Item("AziGruppoCesp")
        TextEdit59.EditValue = dataRd.Item("AziSpecieCesp")
        TextEdit60.EditValue = dataRd.Item("AziSottosCesp")
        TextEdit61.EditValue = dataRd.Item("CspDesc")
        TextEdit62.EditValue = IIf(dataRd.Item("AziCodAteco") Is DBNull.Value, "", dataRd.Item("AziCodAteco").ToString.Trim)
        TextEdit63.EditValue = dataRd.Item("AtecoDesc")
        DateEdit5.EditValue = dataRd.Item("AziDataNascita")
        TextEdit64.EditValue = dataRd.Item("AziCognome")
        TextEdit65.EditValue = dataRd.Item("AziNome")
        ComboBoxEdit6.EditValue = dataRd.Item("AziSesso")
        TextEdit66.EditValue = dataRd.Item("AziComuneNascita")
        TextEdit67.EditValue = dataRd.Item("AziProvNascita")
        'Drr
        TextEdit70.EditValue = dataRd.Item("AziDrrCognome")
        TextEdit71.EditValue = dataRd.Item("AziDrrNome")
        TextEdit72.EditValue = dataRd.Item("AziDrrCodiceFisc")
        ComboBoxEdit3.EditValue = dataRd.Item("AziDrrSesso")
        TextEdit73.EditValue = dataRd.Item("AziDrrCarica770")
        DateEdit6.EditValue = dataRd.Item("AziDrrDataNascita")
        TextEdit74.EditValue = dataRd.Item("AziDrrCapNascita")
        TextEdit75.EditValue = dataRd.Item("AziDrrComuneNascita")
        TextEdit76.EditValue = dataRd.Item("AziDrrProvNascita")
        TextEdit77.EditValue = dataRd.Item("AziDrrCapRes")
        TextEdit78.EditValue = dataRd.Item("AziDrrComuneRes")
        TextEdit79.EditValue = dataRd.Item("AziDrrProvRes")
        TextEdit80.EditValue = dataRd.Item("AziDrrIndirizzo")

        'Dsc
        TextEdit81.EditValue = dataRd.Item("AziDscCognome")
        TextEdit82.EditValue = dataRd.Item("AziDscNome")
        TextEdit83.EditValue = dataRd.Item("AziDscCodiceFisc")
        TextEdit84.EditValue = dataRd.Item("AziDscCap")
        TextEdit85.EditValue = dataRd.Item("AziDscComune")
        TextEdit86.EditValue = dataRd.Item("AziDscProv")
        TextEdit87.EditValue = dataRd.Item("AziDscIndirizzo")
        CheckEdit5.Checked = dataRd.Item("AziScritture")

        'Lea
        TextEdit88.EditValue = dataRd.Item("AziLeaCap")
        TextEdit89.EditValue = dataRd.Item("AziLeaComune")
        TextEdit90.EditValue = dataRd.Item("AziLeaProv")
        TextEdit91.EditValue = dataRd.Item("AziLeaIndirizzo")
        CheckEdit6.Checked = dataRd.Item("AziLuoghiAttivita")
        '''
        If Codfisc(TextEdit72.EditValue) = False Then TextEdit72.ErrorText = " ! Errato" Else TextEdit72.ErrorText = ""
        If Codfisc(TextEdit83.EditValue) = False Then TextEdit83.ErrorText = " ! Errato" Else TextEdit83.ErrorText = ""

    End Sub
#End Region
#Region "Carica Elementi TbAna"
    Private Sub LetturaAna()
        EsisteAna = False
        Dim str As String = "SELECT * from TbAna where AnaCod = '" & TextEdit1.EditValue & "' AND AnaGrp='AZ'"
        Dim cmd As New SqlCommand(str, cnVd)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            CaricaElementiAna()
            EsisteAna = True
        Else
            PuliziaAna(True)
        End If
        dataRd.Close()
    End Sub

    Private Sub CaricaElementiAna() 'TbAna (VDOX)
        TextEdit2.EditValue = dataRd.Item("AnaDesc")
        TextEdit3.EditValue = dataRd.Item("AnaPiva")
        TextEdit4.EditValue = dataRd.Item("AnaIndirizzo")
        TextEdit5.EditValue = dataRd.Item("AnaCfis")
        TextEdit6.EditValue = dataRd.Item("AnaCap")
        TextEdit7.EditValue = dataRd.Item("AnaCitta")
        TextEdit8.EditValue = dataRd.Item("AnaProv")
        TextEdit9.EditValue = dataRd.Item("AnaTel1")
        TextEdit10.EditValue = dataRd.Item("AnaTel2")
        TextEdit11.EditValue = dataRd.Item("AnaTel3")
        TextEdit12.EditValue = dataRd.Item("AnaFax")
        TextEdit13.EditValue = dataRd.Item("AnaWww")
        TextEdit14.EditValue = dataRd.Item("AnaEmail")
        If Codfisc(TextEdit3.EditValue) = False Then TextEdit3.ErrorText = " ! Errato" Else TextEdit3.ErrorText = ""
        If Codfisc(TextEdit5.EditValue) = False Then TextEdit5.ErrorText = " ! Errato" Else TextEdit5.ErrorText = ""
    End Sub
#End Region
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Val(TextEdit1.EditValue) = 0 Then TextEdit1.Focus() : Exit Sub
        If CheckDati() = False Then Return
        ScriviAna()
        ScriviAzi()
        ScriviEse()
        ButtonF5.PerformClick()
    End Sub
    Private Function CheckDati() As Boolean
        Dim txt As String = ""
        If Val(Mid(ComboBoxEdit4.EditValue, 1, 2)) = 45 Then txt &= Chr(13) & "<> Causale Chiusura Conti Errata"
        If ComboBoxEdit2.SelectedIndex > -1 Then ControllaArtGiroconto(txt)
        If txt > "" Then
            MessageBox.Show("Si sono verificati i seguenti errori:" & Chr(13) & txt, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function
    Function ControllaArtGiroconto(ByRef txt As String) As Boolean
        Dim str As String = "Select * from TbArtP where ArtPId=@ArtPId"
        Cmd = New SqlCommand(str, cnCo)
        Dim p1 As New SqlParameter("@ArtPId", SqlDbType.Int)
        p1.Value = Val(Mid(ComboBoxEdit2.EditValue, 1, 6))
        Cmd.Parameters.Add(p1)
        dataRd = Cmd.ExecuteReader
        Dim Numart As Int16
        Dim F As String = ""
        While dataRd.Read
            F = dataRd.Item("ArtPDare")
            Numart += 1
        End While
        dataRd.Close()
        If Numart > 1 Then txt &= Chr(13) & "<> l'Articolo Giroconto deve avere una sola riga" : Return False
        If F.ToLower <> "f" Then txt &= Chr(13) & "<> l'Articolo Giroconto deve avere un Fornitore in Dare" : Return False
    End Function

    Private Sub ScriviAna()
        Dim Com As String
        If EsisteAna = False Then
            Com = "Insert Into TbAna(AnaCod,AnaDesc,AnaPiva,AnaIndirizzo,AnaCfis,AnaCap,AnaCitta,AnaProv,AnaGrp,AnaTel1,AnaTel2,AnaTel3,AnaFax,AnaWww,AnaEmail,AnaResp,AnaRag1,AnaRag2,AnaPivaEst,AnaNote) values (@AnaCod,@AnaDesc,@AnaPiva,@AnaIndirizzo,@AnaCfis,@AnaCap,@AnaCitta,@AnaProv,@AnaGrp,@AnaTel1,@AnaTel2,@AnaTel3,@AnaFax,@AnaWww,@AnaEmail,@AnaResp,@AnaRag1,@AnaRag2,@AnaPivaEst,@AnaNote)"
            EsisteAna = True
        Else
            Com = "Update TbAna set AnaDesc=@AnaDesc,AnaPiva=@AnaPiva,AnaIndirizzo=@AnaIndirizzo,AnaCfis=@AnaCfis,AnaCap=@AnaCap,AnaCitta=@AnaCitta,AnaProv=@AnaProv,AnaGrp=@AnaGrp,AnaTel1=@AnaTel1,AnaTel2=@AnaTel2,AnaTel3=@AnaTel3,AnaFax=@AnaFax,AnaWww=@AnaWww,AnaEmail=@AnaEmail,AnaResp=@AnaResp,AnaRag1=@AnaRag1,AnaRag2=@AnaRag2,AnaPivaEst=@AnaPivaEst,AnaNote=@AnaNote where AnaCod='" & Val(TextEdit1.EditValue).ToString("00000") & "'"
        End If

        Cmd = New SqlCommand(Com, cnVd)

        Dim p1 As New SqlParameter("@AnaCod", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@AnaDesc", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@AnaPiva", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@AnaCfis", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@AnaIndirizzo", SqlDbType.VarChar)
        Dim p6 As New SqlParameter("@AnaCap", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@AnaCitta", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@AnaProv", SqlDbType.VarChar)
        Dim p9 As New SqlParameter("@AnaGrp", SqlDbType.VarChar)
        Dim p10 As New SqlParameter("@AnaTel1", SqlDbType.VarChar)
        Dim p11 As New SqlParameter("@AnaTel2", SqlDbType.VarChar)
        Dim p12 As New SqlParameter("@AnaTel3", SqlDbType.VarChar)
        Dim p13 As New SqlParameter("@AnaFax", SqlDbType.VarChar)
        Dim p14 As New SqlParameter("@AnaWWW", SqlDbType.VarChar)
        Dim p15 As New SqlParameter("@AnaEmail", SqlDbType.VarChar)
        Dim p16 As New SqlParameter("@AnaResp", SqlDbType.VarChar)
        Dim p17 As New SqlParameter("@AnaRag1", SqlDbType.VarChar)
        Dim p18 As New SqlParameter("@AnaRag2", SqlDbType.VarChar)
        Dim p19 As New SqlParameter("@AnaPivaEst", SqlDbType.VarChar)
        Dim p20 As New SqlParameter("@AnaNote", SqlDbType.VarChar)

        p1.Value = Val(TextEdit1.EditValue).ToString("00000")
        p2.Value = TextEdit2.EditValue
        p3.Value = TextEdit3.EditValue
        p4.Value = TextEdit5.EditValue
        p5.Value = TextEdit4.EditValue
        p6.Value = TextEdit6.EditValue
        p7.Value = TextEdit7.EditValue
        p8.Value = TextEdit8.EditValue
        p9.Value = "AZ"
        p10.Value = TextEdit9.EditValue
        p11.Value = TextEdit10.EditValue
        p12.Value = TextEdit11.EditValue
        p13.Value = TextEdit12.EditValue
        p14.Value = TextEdit13.EditValue
        p15.Value = TextEdit14.EditValue
        p16.Value = ""
        p17.Value = ""
        p18.Value = ""
        p19.Value = ""
        p20.Value = ""

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

        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ScriviAzi()
        Dim Com As String
        If EsisteAzi = False Then
            Com = "Insert Into TbAzi(AziCod,AziAnnoLavoro,AziRegimeIva,AziEsente,AziGruppoCesp,AziSpecieCesp,AziSottosCesp,AziUfficioIva,AziCodIstat,AziCodAttivita,AziDescAttivita,AziCognome,AziNome,AziDataNascita,AziSesso,AziComuneNascita,AziProvNascita,AziNaturaIva,AziNaturaRedditi,AziScritture,AziLuoghiAttivita,AziLeaCap,AziLeaComune,AziLeaProv,AziLeaIndirizzo,AziDrrCodiceFisc,AziDrrCognome,AziDrrNome,AziDrrSesso,AziDrrDataNascita,AziDrrCarica770,AziDrrCapNascita,AziDrrComuneNascita,AziDrrProvNascita,AziDrrCapRes,AziDrrComuneRes, AziDrrProvRes, AziDrrIndirizzo, AziDscCodiceFisc, AziDscCognome, AziDscNome, AziDscCap, AziDscComune, AziDscProv, AziDscIndirizzo,AziCodAteco )" & _
" values (@AziCod, @AziAnnoLavoro, @AziRegimeIva,@AziEsente,@AziGruppoCesp,@AziSpecieCesp,@AziSottosCesp,@AziUfficioIva,@AziCodIstat,@AziCodAttivita,@AziDescAttivita,@AziCognome,@AziNome,@AziDataNascita,@AziSesso,@AziComuneNascita,@AziProvNascita,@AziNaturaIva,@AziNaturaRedditi,@AziScritture,@AziLuoghiAttivita,@AziLeaCap,@AziLeaComune,@AziLeaProv,@AziLeaIndirizzo,@AziDrrCodiceFisc,@AziDrrCognome,@AziDrrNome,@AziDrrSesso,@AziDrrDataNascita,@AziDrrCarica770,@AziDrrCapNascita,@AziDrrComuneNascita,@AziDrrProvNascita,@AziDrrCapRes,@AziDrrComuneRes, @AziDrrProvRes, @AziDrrIndirizzo, @AziDscCodiceFisc, @AziDscCognome, @AziDscNome, @AziDscCap, @AziDscComune, @AziDscProv, @AziDscIndirizzo, @AziCodAteco)"
        Else
            Com = "Update TbAzi set Azicod=@aziCod,AziRegimeIva=@AziRegimeIva,AziEsente=@AziEsente,AziGruppoCesp=@AziGruppoCesp,AziSpecieCesp=@AziSpecieCesp,AziSottosCesp=@AziSottosCesp,AziUfficioIva=@AziUfficioIva,AziCodIstat=@AziCodIstat,AziCodAttivita=@AziCodAttivita,AziDescAttivita=@AziDescAttivita,AziCognome=@AziCognome,AziNome=@AziNome,AziDataNascita=@AziDataNascita,AziSesso=@AziSesso,AziComuneNascita=@AziComuneNascita,AziProvNascita=@AziProvNascita,AziNaturaIva=@AziNaturaIva,AziNaturaRedditi=@AziNaturaRedditi,AziScritture=@AziScritture,AziLuoghiAttivita=@AziLuoghiAttivita,AziLeaCap=@AziLeaCap,AziLeaComune=@AziLeaComune,AziLeaProv=@AziLeaProv,AziLeaIndirizzo=@AziLeaIndirizzo,AziDrrCodiceFisc=@AziDrrCodiceFisc,AziDrrCognome=@AziDrrCognome,AziDrrNome=@AziDrrNome,AziDrrSesso=@AziDrrSesso,AziDrrDataNascita=@AziDrrDataNascita,AziDrrCarica770=@AziDrrCarica770,AziDrrCapNascita=@AziDrrCapNascita,AziDrrComuneNascita=@AziDrrComuneNascita,AziDrrProvNascita=@AziDrrProvNascita,AziDrrCapRes=@AziDrrCapRes,AziDrrComuneRes=@AziDrrComuneRes, AziDrrProvRes=@AziDrrProvRes, AziDrrIndirizzo=@AziDrrIndirizzo, AziDscCodiceFisc=@AziDscCodiceFisc, AziDscCognome=@AziDscCognome, AziDscNome=@AziDscNome, AziDscCap=@AziDscCap, AziDscComune=@AziDscComune, AziDscProv=@AziDscProv, AziDscIndirizzo=@AziDscIndirizzo, AziCodAteco=@AziCodAteco where AziAnnoLavoro='" & ComboBoxEdit1.EditValue & "'"
        End If

        Cmd = New SqlCommand(Com, cnCo)

        Dim p1 As New SqlParameter("@AziAnnoLavoro", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@AziCod", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@AziCodAttivita", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@AziCodIstat", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@AziCognome", SqlDbType.VarChar)
        Dim p6 As New SqlParameter("@AziComuneNascita", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@AziDataNascita", SqlDbType.SmallDateTime)
        Dim p8 As New SqlParameter("@AziDescAttivita", SqlDbType.VarChar)
        Dim p9 As New SqlParameter("@AziDrrCapNascita", SqlDbType.VarChar)
        Dim p10 As New SqlParameter("@AziDrrCapRes", SqlDbType.VarChar)
        Dim p11 As New SqlParameter("@AziDrrCarica770", SqlDbType.VarChar)
        Dim p12 As New SqlParameter("@AziDrrCodiceFisc", SqlDbType.VarChar)
        Dim p13 As New SqlParameter("@AziDrrCognome", SqlDbType.VarChar)
        Dim p14 As New SqlParameter("@AziDrrComuneNascita", SqlDbType.VarChar)
        Dim p15 As New SqlParameter("@AziDrrComuneRes", SqlDbType.VarChar)
        Dim p16 As New SqlParameter("@AziDrrDataNascita", SqlDbType.SmallDateTime)
        Dim p17 As New SqlParameter("@AziDrrIndirizzo", SqlDbType.VarChar)
        Dim p18 As New SqlParameter("@AziDrrNome", SqlDbType.VarChar)
        Dim p19 As New SqlParameter("@AziDrrProvNascita", SqlDbType.VarChar)
        Dim p20 As New SqlParameter("@AziDrrProvRes", SqlDbType.VarChar)
        Dim p21 As New SqlParameter("@AziDrrSesso", SqlDbType.VarChar)
        Dim p22 As New SqlParameter("@AziDscCap", SqlDbType.VarChar)
        Dim p23 As New SqlParameter("@AziDscCodiceFisc", SqlDbType.VarChar)
        Dim p24 As New SqlParameter("@AziDscCognome", SqlDbType.VarChar)
        Dim p25 As New SqlParameter("@AziDscComune", SqlDbType.VarChar)
        Dim p26 As New SqlParameter("@AziDscIndirizzo", SqlDbType.VarChar)
        Dim p27 As New SqlParameter("@AziDscNome", SqlDbType.VarChar)
        Dim p28 As New SqlParameter("@AziDscProv", SqlDbType.VarChar)
        Dim p29 As New SqlParameter("@AziEsente", SqlDbType.Decimal)
        Dim p30 As New SqlParameter("@AziGruppoCesp", SqlDbType.VarChar)
        Dim p31 As New SqlParameter("@AziLeaCap", SqlDbType.VarChar)
        Dim p32 As New SqlParameter("@AziLeaComune", SqlDbType.VarChar)
        Dim p33 As New SqlParameter("@AziLeaIndirizzo", SqlDbType.VarChar)
        Dim p34 As New SqlParameter("@AziLeaProv", SqlDbType.VarChar)
        Dim p35 As New SqlParameter("@AziLuoghiAttivita", SqlDbType.Bit)
        Dim p36 As New SqlParameter("@AziNaturaIva", SqlDbType.VarChar)
        Dim p37 As New SqlParameter("@AziNaturaRedditi", SqlDbType.VarChar)
        Dim p38 As New SqlParameter("@AziNome", SqlDbType.VarChar)
        Dim p39 As New SqlParameter("@AziProvNascita", SqlDbType.VarChar)
        Dim p40 As New SqlParameter("@AziRegimeIva", SqlDbType.VarChar)
        Dim p41 As New SqlParameter("@AziScritture", SqlDbType.Bit)
        Dim p42 As New SqlParameter("@AziSesso", SqlDbType.VarChar)
        Dim p43 As New SqlParameter("@AziSottosCesp", SqlDbType.VarChar)
        Dim p44 As New SqlParameter("@AziSpecieCesp", SqlDbType.VarChar)
        Dim p45 As New SqlParameter("@AziUfficioIva", SqlDbType.VarChar)
        Dim p46 As New SqlParameter("@AziCodAteco", SqlDbType.VarChar)
        p1.Value = CInt(ComboBoxEdit1.EditValue)
        p2.Value = Val(TextEdit1.EditValue).ToString("00000")
        p3.Value = TextEdit54.EditValue
        p4.Value = TextEdit56.EditValue
        p5.Value = TextEdit64.EditValue
        p6.Value = TextEdit66.EditValue
        If DateEdit5.EditValue Is Nothing Then p7.Value = System.DBNull.Value Else p7.Value = DateEdit5.EditValue
        p8.Value = TextEdit55.EditValue
        p9.Value = TextEdit74.EditValue
        p10.Value = TextEdit77.EditValue
        p11.Value = TextEdit73.EditValue
        p12.Value = TextEdit72.EditValue
        p13.Value = TextEdit70.EditValue
        p14.Value = TextEdit75.EditValue
        p15.Value = TextEdit78.EditValue
        If DateEdit6.EditValue Is Nothing Then p16.Value = System.DBNull.Value Else p16.Value = DateEdit6.EditValue
        p17.Value = TextEdit80.EditValue
        p18.Value = TextEdit71.EditValue
        p19.Value = TextEdit76.EditValue
        p20.Value = TextEdit79.EditValue
        p21.Value = ComboBoxEdit3.EditValue
        p22.Value = TextEdit84.EditValue
        p23.Value = TextEdit83.EditValue
        p24.Value = TextEdit81.EditValue
        p25.Value = TextEdit85.EditValue
        p26.Value = TextEdit87.EditValue
        p27.Value = TextEdit82.EditValue
        p28.Value = TextEdit86.EditValue
        p29.Value = CDec(TextEdit51.EditValue)
        p30.Value = TextEdit58.EditValue
        p31.Value = TextEdit88.EditValue
        p32.Value = TextEdit89.EditValue
        p33.Value = TextEdit91.EditValue
        p34.Value = TextEdit90.EditValue
        If CheckEdit6.Checked = False Then p35.Value = 0 Else p35.Value = 1
        p36.Value = TextEdit52.EditValue
        p37.Value = TextEdit53.EditValue
        p38.Value = TextEdit65.EditValue
        p39.Value = TextEdit67.EditValue
        p40.Value = ComboBoxEdit5.SelectedIndex
        If CheckEdit5.Checked = False Then p41.Value = 0 Else p41.Value = 1
        p42.Value = ComboBoxEdit6.EditValue
        p43.Value = TextEdit60.EditValue
        p44.Value = TextEdit59.EditValue
        p45.Value = TextEdit50.EditValue
        p46.Value = TextEdit62.EditValue

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
        Cmd.Parameters.Add(p33)
        Cmd.Parameters.Add(p34)
        Cmd.Parameters.Add(p35)
        Cmd.Parameters.Add(p36)
        Cmd.Parameters.Add(p37)
        Cmd.Parameters.Add(p38)
        Cmd.Parameters.Add(p39)
        Cmd.Parameters.Add(p40)
        Cmd.Parameters.Add(p41)
        Cmd.Parameters.Add(p42)
        Cmd.Parameters.Add(p43)
        Cmd.Parameters.Add(p44)
        Cmd.Parameters.Add(p45)
        Cmd.Parameters.Add(p46)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ScriviEse()
        Dim Com As String
        If EsisteAzi = False Then
            Com = "Insert Into TbEse (EseAnno, EseDal,EseAl,EseBilAp,EseBilChi,EsePP,EseUti,EsePer,EseGioInt,EseGioDesc,EseGioNFog,EseGioProg,EseFormato,EseCausaleChiusuraConti,EseArtGiroconto,EseSaDa,EseSaA,EseSpDa,EseSpA,EseCeDa,EseCeA,EseInvNFog,EseInvDesc,EseGContoRCee)values (@EseAnno, @EseDal, @EseAl,@EseBilAp,@EseBilChi,@EsePP,@EseUti,@EsePer,@EseGioInt,@EseGioDesc,@EseGioNFog,@EseGioProg,@EseFormato,@EseCausaleChiusuraConti,@EseArtGiroconto,@EseSaDa,@EseSaA,@EseSpDa,@EseSpA,@EseCeDa,@EseCeA,@EseInvNFog,@EseInvDesc,@EseGContoRCee)"
        Else
            Com = "Update TbEse set EseDal=@EseDal,EseAl=@EseAl,EseBilAp=@EseBilAp,EseBilChi=@EseBilChi,EsePP=@EsePP,EseUti=@EseUti,EsePer=@EsePer,EseGioInt=@EseGioInt,EseGioDesc=@EseGioDesc,EseGioNFog=@EseGioNFog,EseGioProg=@EseGioProg,EseFormato=@EseFormato,EseCausaleChiusuraConti=@EseCausaleChiusuraConti,EseArtGiroconto=@EseArtGiroconto,EseSaDa=@EseSaDa,EseSaA=@EseSaA,EseSpDa=@EseSpDa,EseSpA=@EseSpA,EseCeDa=@EseCeDa,EseCeA=@EseCeA,EseInvNFog=@EseInvNFog,EseInvDesc=@EseInvDesc,EseGContoRCee=@EseGContoRCee where eseAnno='" & ComboBoxEdit1.EditValue & "'"
        End If

        Cmd = New SqlCommand(Com, cnCo)

        Dim p1 As New SqlParameter("@EseAnno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@EseDal", SqlDbType.SmallDateTime)
        Dim p3 As New SqlParameter("@EseAl", SqlDbType.SmallDateTime)
        Dim p4 As New SqlParameter("@EseBilAp", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@EseBilChi", SqlDbType.VarChar)
        Dim p6 As New SqlParameter("@EsePP", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@EseUti", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@EsePer", SqlDbType.VarChar)
        Dim p9 As New SqlParameter("@EseGioInt", SqlDbType.Bit)
        Dim p10 As New SqlParameter("@EseGioDesc", SqlDbType.VarChar)
        Dim p11 As New SqlParameter("@EseGioNFog", SqlDbType.Int)
        Dim p13 As New SqlParameter("@EseGioProg", SqlDbType.Decimal)
        Dim p15 As New SqlParameter("@EseFormato", SqlDbType.SmallInt)
        Dim p16 As New SqlParameter("@EseCausaleChiusuraConti", SqlDbType.SmallInt)
        Dim p17 As New SqlParameter("@EseArtGiroconto", SqlDbType.Int)
        Dim p18 As New SqlParameter("@EseSaDa", SqlDbType.Int)
        Dim p19 As New SqlParameter("@EseSaA", SqlDbType.Int)
        Dim p20 As New SqlParameter("@EseSpDa", SqlDbType.Int)
        Dim p21 As New SqlParameter("@EseSpA", SqlDbType.Int)
        Dim p22 As New SqlParameter("@EseCeDa", SqlDbType.Int)
        Dim p23 As New SqlParameter("@EseCeA", SqlDbType.Int)
        Dim p24 As New SqlParameter("@EseInvNFog", SqlDbType.Int)
        Dim p25 As New SqlParameter("@EseInvDesc", SqlDbType.VarChar)
        Dim p26 As New SqlParameter("@EseGContoRCee", SqlDbType.Int)

        p1.Value = ComboBoxEdit1.EditValue
        p2.Value = DateEdit1.EditValue
        p3.Value = DateEdit2.EditValue
        p4.Value = TextEdit15.EditValue
        p5.Value = TextEdit17.EditValue
        p6.Value = TextEdit19.EditValue
        p7.Value = TextEdit21.EditValue
        p8.Value = TextEdit23.EditValue
        p9.Value = CheckEdit1.Checked
        p10.Value = IIf(CheckEdit1.Checked = True, TextEdit31.EditValue, "")
        p11.Value = Val(TextEdit32.EditValue)
        p13.Value = CDec(TextEdit33.EditValue)
        p15.Value = RadioGroup1.SelectedIndex
        p16.Value = Val(Mid(ComboBoxEdit4.EditValue, 1, 2))
        p17.Value = Val(Mid(ComboBoxEdit2.EditValue, 1, 6))
        p18.Value = Val(TextEdit25.EditValue)
        p19.Value = Val(TextEdit26.EditValue)
        p20.Value = Val(TextEdit27.EditValue)
        p21.Value = Val(TextEdit28.EditValue)
        p22.Value = Val(TextEdit29.EditValue)
        p23.Value = Val(TextEdit30.EditValue)
        p24.Value = Val(TextEdit37.EditValue)
        p25.Value = TextEdit36.EditValue
        p26.Value = Val(Mid(ComboBoxEdit7.EditValue, 1, 2))

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
        Cmd.Parameters.Add(p13)
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

        Cmd.ExecuteNonQuery()
    End Sub

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

    Function ControllaConto(ByVal CodiceCo As String) As Boolean
        If Len(CodiceCo) <> 5 Then Return False : Exit Function
        If Not IsNumeric(Mid(CodiceCo, 1, 2)) Then Return False : Exit Function
        If Not IsNumeric(Mid(CodiceCo, 4, 2)) Then Return False : Exit Function
        If Mid(CodiceCo, 3, 1) <> "." Then Return False : Exit Function
        Return True
    End Function
    Private Sub LeggiCespiti()
        If TextEdit60.EditValue = "" Then TextEdit60.EditValue = "0"
        Dim str As String = "SELECT * from TbCsp where CspGru = '" & TextEdit58.EditValue & "' and CspSpe1='" & TextEdit59.EditValue & "' and CspSpe2 = '" & TextEdit60.EditValue & "' and CspNum=0"
        TextEdit61.EditValue = ""
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit61.EditValue = dataRd.Item("CspDesc")
        Else
            TextEdit61.EditValue = ""
        End If
        dataRd.Close()
    End Sub
    Private Sub LeggiAteco()
        Dim str As String = "SELECT * from TbAteco where AtecoCod = '" & TextEdit62.EditValue & "'"
        TextEdit63.EditValue = ""
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit63.EditValue = dataRd.Item("AtecoDesc")
        End While
        dataRd.Close()
    End Sub
    Private Sub LeggiIstat()
        Dim str As String = "SELECT * from TbIstat where IstatCod = '" & TextEdit56.EditValue & "'"
        TextEdit57.EditValue = ""
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit57.EditValue = dataRd.Item("IstatDesc")
        End While
        dataRd.Close()
    End Sub

#Region "Letture da uscita campo"
    Private Sub TextEdit60_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit60.Validated
        LeggiCespiti()
    End Sub
    Private Sub TextEdit56_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit56.Validated
        LeggiIstat()
    End Sub
    Private Sub TextEdit62_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit62.Validated
        LeggiAteco()
    End Sub
    Private Sub TextEdit3_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit3.Validated, TextEdit5.Validated, TextEdit72.Validated, TextEdit83.Validated
        ControllaCFPI(sender)
    End Sub

    Sub ControllaCFPI(ByVal Codice As TextEdit)
        If Codfisc(Codice.Text) = False Then Codice.ErrorText = " ! Errato" Else Codice.ErrorText = ""
    End Sub

    Private Sub TbLeggi1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi1.Enter
        If TextEdit17.EditorContainsFocus = True Or ButtonC1.Focused Then TextEdit15.Focus() : Exit Sub
        If ControllaConto(TextEdit15.EditValue) = False Then TextEdit15.EditValue = "00.00"
        TextEdit16.EditValue = LeggiCpt(TextEdit15.EditValue)
        SelectNextControl(TextEdit16, True, True, True, False)
    End Sub
    Private Sub TbLeggi2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi2.Enter
        If TextEdit19.EditorContainsFocus = True Or ButtonC2.Focused Then TextEdit17.Focus() : Exit Sub
        If ControllaConto(TextEdit17.EditValue) = False Then TextEdit17.EditValue = "00.00"
        TextEdit18.EditValue = LeggiCpt(TextEdit17.EditValue)
        SelectNextControl(TextEdit18, True, True, True, False)
    End Sub
    Private Sub TbLeggi3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi3.Enter
        If TextEdit21.EditorContainsFocus = True Or ButtonC3.Focused Then TextEdit19.Focus() : Exit Sub
        If ControllaConto(TextEdit19.EditValue) = False Then TextEdit19.EditValue = "00.00"
        TextEdit20.EditValue = LeggiCpt(TextEdit19.EditValue)
        SelectNextControl(TextEdit20, True, True, True, False)
    End Sub
    Private Sub TbLeggi4_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi4.Enter
        If TextEdit23.EditorContainsFocus = True Or ButtonC4.Focused Then TextEdit21.Focus() : Exit Sub
        If ControllaConto(TextEdit21.EditValue) = False Then TextEdit21.EditValue = "00.00"
        TextEdit22.EditValue = LeggiCpt(TextEdit21.EditValue)
        SelectNextControl(TextEdit22, True, True, True, False)
    End Sub
    Private Sub TbLeggi5_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi5.Enter
        If TextEdit25.EditorContainsFocus = True Or ButtonC5.Focused Then TextEdit23.Focus() : Exit Sub
        If ControllaConto(TextEdit23.EditValue) = False Then TextEdit23.EditValue = "00.00"
        TextEdit24.EditValue = LeggiCpt(TextEdit23.EditValue)
        SelectNextControl(TextEdit24, True, True, True, False)
    End Sub
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        If ComboBoxEdit1.SelectedIndex > -1 Then
            If ComboBoxEdit1.SelectedIndex > 0 Then ButtonPlus.Enabled = False : ButtonF3.Enabled = False Else ButtonPlus.Enabled = True : ButtonF3.Enabled = True
            LetturaAzi()
            LetturaAna()
            TextEdit1.Focus()
        End If
    End Sub
    Private Sub TextEdit1_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.Validated
        If Val(TextEdit1.EditValue) > 0 Then
            LetturaAzi()
            LetturaAna()
            ButtonF5.Focus()
        End If
    End Sub
#End Region
    Private Sub ButtonRistat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRistat.Click
        Dim Str As String = Query.CercaIstat(TextEdit57.EditValue)
        If Str.Length = 5 Then TextEdit56.EditValue = Str
        LeggiIstat()
        SelectNextControl(sender, True, True, True, False)
    End Sub

    Private Sub ButtonRateco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRateco.Click
        Dim Str As String = Query.CercaAteco(TextEdit63.EditValue)
        If Str.Length = 8 Then TextEdit62.EditValue = Str
        LeggiAteco()
        SelectNextControl(sender, True, True, True, False)
    End Sub
    Private Sub ButtonRCes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRCes.Click
        Dim WsProg As String = Query.CercaCespite(TextEdit58.EditValue, TextEdit59.EditValue, TextEdit60.EditValue)
        If WsProg.Length = 6 Then
            TextEdit58.EditValue = Mid(WsProg, 1, 3)
            TextEdit59.EditValue = Mid(WsProg, 4, 2)
            TextEdit60.EditValue = Mid(WsProg, 6, 1)
        End If
        LeggiCespiti()
        SelectNextControl(TextEdit61, True, True, True, False)
    End Sub

    Private Sub ButtonPlus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlus.Click
        If NoJob = True Then Exit Sub
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Exit Sub
        If (ComboBoxEdit1.EditValue + 1) > (Today.Year + 1) Then Exit Sub
        If MessageBox.Show("APRO nuovo Anno di Lavoro?", "NUOVO ANNO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            NuovoAnno()
        End If
    End Sub
    Sub NuovoAnno()
        ComboBoxEdit1.SelectedIndex = 0
        AnnoDa = ComboBoxEdit1.EditValue
        UltAnno = ComboBoxEdit1.EditValue + 1
        ComboBoxEdit1.EditValue = UltAnno
        Button1.Text = UltAnno
        Disablenable(False)
    End Sub

    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        eliminaAzi()
    End Sub

    Private Sub eliminaAzi()
        Cmd = New SqlCommand("select COUNT(*) from tbpri where DATEPART(year,pridatagio) =" & ComboBoxEdit1.EditValue, cnCo)
        If Cmd.ExecuteScalar > 0 Then
            MessageBox.Show("Esistono Movimenti!!!! Impossibile Eliminare il " & ComboBoxEdit1.EditValue, "ELIMINA ANNO DI LAVORO", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        Dim box As Object
        box = MessageBox.Show("Vuoi eliminare l'Anno di Lavoro?", "ELIMINA ANNO DI LAVORO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If box = DialogResult.No Then Exit Sub
        Dim Delete As String
        Delete = "delete from TbAzi where AziCod ='" & TextEdit1.EditValue & "' and AziannoLavoro='" & ComboBoxEdit1.EditValue & "'"
        Cmd = New SqlCommand(Delete, cnCo)
        Cmd.ExecuteNonQuery()
        Delete = "delete from TbRegiva where RivaAnno =" & ComboBoxEdit1.EditValue
        Cmd = New SqlCommand(Delete, cnCo)
        Cmd.ExecuteNonQuery()
        Delete = "delete from Tbese where EseAnno =" & ComboBoxEdit1.EditValue
        Cmd = New SqlCommand(Delete, cnCo)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("Select count(*) from Tbazi where aziCod='" & TextEdit1.EditValue & "'", cnCo)
        If Not Cmd.ExecuteScalar > 0 Then
            Delete = "delete from TbAna where AnaCod ='" & TextEdit1.EditValue & "'"
            Cmd = New SqlCommand(Delete, cnVd)
            Cmd.ExecuteNonQuery()
        End If
        ButtonF5.PerformClick()
    End Sub
    Sub Disablenable(ByVal ok As Boolean)
        GroupControl91.Visible = Not ok
        GroupControl1.Enabled = ok
        GroupControl16.Enabled = ok
        GroupControl83.Enabled = ok
        GroupControl17.Enabled = ok
        XtraTabPage2.PageEnabled = ok
        XtraTabPage3.PageEnabled = ok
    End Sub

    Private Sub ButtonF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF2.Click
        Disablenable(True)
        ComboBoxEdit1.SelectedIndex = 0
    End Sub

    Private Sub ButtonF11X_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11X.Click
        If NoJob = True Then Exit Sub
        Dim cmd As New SqlCommand("select * from tbAzi where AziAnnoLavoro=" & ComboBoxEdit1.EditValue, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            MessageBox.Show("Impossibile continuare. Esiste già in Archivio l'Anno di Lavoro " & ComboBoxEdit1.EditValue, "ERRORE", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
            dataRd.Close()
            Return
        End If
        dataRd.Close()
        Dim Com As String = "EXEC CopiaAnnoLavoro  @Da =" & AnnoDa & ", @A =" & ComboBoxEdit1.EditValue
        Dim Agg As New SqlCommand(Com, cnCo)
        Agg.CommandTimeout = 300 '5 MINUTI (300 SECONDI)
        Agg.ExecuteNonQuery()
        ButtonF2.PerformClick()
        ButtonF5.PerformClick()
    End Sub
    Private Sub ComboBoxEdit7_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ComboBoxEdit7.ButtonClick
        If e.Button.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Delete Then DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit).SelectedIndex = -1
    End Sub
    Private Sub ButtonC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonC1.Click, ButtonC2.Click, ButtonC3.Click, ButtonC4.Click, ButtonC5.Click
        Dim CodiceConto As String = Query.CercaPia()
        If CodiceConto Is Nothing Then Exit Sub
        If CodiceConto.Length <> 5 Then Exit Sub
        If sender Is ButtonC1 Then
            TextEdit15.EditValue = CodiceConto
            TextEdit16.EditValue = LeggiCpt(TextEdit15.EditValue)
            SelectNextControl(TextEdit16, True, True, True, False)
        ElseIf sender Is ButtonC2 Then
            TextEdit17.EditValue = CodiceConto
            TextEdit18.EditValue = LeggiCpt(TextEdit17.EditValue)
            SelectNextControl(TextEdit18, True, True, True, False)
        ElseIf sender Is ButtonC3 Then
            TextEdit19.EditValue = CodiceConto
            TextEdit20.EditValue = LeggiCpt(TextEdit19.EditValue)
            SelectNextControl(TextEdit20, True, True, True, False)
        ElseIf sender Is ButtonC4 Then
            TextEdit21.EditValue = CodiceConto
            TextEdit22.EditValue = LeggiCpt(TextEdit21.EditValue)
            SelectNextControl(TextEdit22, True, True, True, False)
        ElseIf sender Is ButtonC5 Then
            TextEdit23.EditValue = CodiceConto
            TextEdit24.EditValue = LeggiCpt(TextEdit23.EditValue)
            SelectNextControl(TextEdit24, True, True, True, False)
        End If
    End Sub

End Class