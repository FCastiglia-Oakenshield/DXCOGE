Imports DXBASE
Imports NPRINT
Imports NCCOM
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.XtraEditors

Public Class DxChPeIv
    Dim Sw As Int16 = 0

    Dim StrReg, StrRie, QUALEREG, MT(1), TRI(4), REGIME, TIPOLP(), RESETLP(), AZI, COAZI, DESCREG, CauDes(72) As String
    Dim DataAl, DataDal As Date

    Dim NRREG, TIPOREG, MIN, MAX, MESE, UCHI, PS, PB, REGCHIU, NRCORR() As Int16
    Dim PP, ProgId As Int32
    Dim ESENTE, CreditoAnnoP, CreditoP, LIMITE, AccontoP, Versamento, Interessi As Decimal
    Dim TRIM, EsisteVers, OKCHIUDO, OKINTESTA, OKGIRO As Boolean

    Dim Tr As String = "TReg"
    Dim DsReg As DataSet
    Dim DaReg As SqlDataAdapter
    Dim RwReg As DataRow

    Dim Pn As String = "NOTA"
    Dim DsPno As DataSet
    Dim DaPno As SqlDataAdapter
    Dim RwPno As DataRow
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim OkPlafond As Boolean = False
    Dim Userid As String = ""
    Dim OkLDP As Boolean = False

    Private Sub DxChPeIv_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Sw = 0 Then
            LeggiPlafond()
            Apertura()
            Popolaprinter()
            PopolaBanche()
            PopolaCorrispettivi()
            Sw = 1
        End If
    End Sub
    Sub GestioneUser()
        REM CDC PER ORA SOLO SU GRUPPO PASTA ATAVOLA
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UserId = dataRd.Item("Sel14")
        End While
        dataRd.Close()
        OkLDP = False
        If UserId.ToUpper = "PASTAECO" Or UserId.ToUpper = "PASTANEW" Or UserId.ToUpper = "PASTAGROUP" Then
            OkLDP = True
        End If
    End Sub
    Sub LeggiPlafond()
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 2", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            OkPlafond = IIf(dataRd.Item("Sel1") = "PLAFOND", True, False)
        End While
        dataRd.Close()
    End Sub
    Sub PopolaCorrispettivi()
        OKGIRO = False
        GroupControl8.Visible = OKGIRO
        Dim Str As String = "SELECT * from TbCii order by CiiCod"
        Dim SS As String = ""
        ImageComboBoxEdit2.Properties.Items.Clear()
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("CiiCod") > 3 And dataRd.Item("CiiCod") <> 45 Then
                SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau")
                nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
                ImageComboBoxEdit2.Properties.Items.Add(nn)
            End If
        End While
        dataRd.Close()
        Dim Y As Int16 = 0
        ReDim NRCORR(0)
        NRCORR(0) = 0
        Cmd = New SqlCommand("SELECT * from TbRegIva Where RivaTipo = 5 and RivaAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            OKGIRO = True
            ImageComboBoxEdit2.EditValue = dataRd.Item("RivaArt")
            TextEdit20.EditValue = dataRd.Item("RivaCpt")
            Y = Y + 1
            ReDim Preserve NRCORR(Y)
            NRCORR(Y) = dataRd.Item("RivaNreg")
        End While
        dataRd.Close()
        If OKGIRO = False Then Exit Sub
        NRCORR(0) = Y
        DateEdit3.EditValue = CDate(DataAl.ToShortDateString)
        DateEdit4.EditValue = DateEdit3.EditValue
        If ImageComboBoxEdit2.SelectedIndex = -1 Then ImageComboBoxEdit2.Focus()
        If LeggiConto(TextEdit20.EditValue, TextEdit21) = False Then
            TextEdit20.Focus()
        End If
        TextEdit22.EditValue = "IVA AL " & DateEdit3.EditValue
        GroupControl8.Visible = OKGIRO
    End Sub
    Sub PopolaBanche()
        ComboBoxEdit2.Properties.Items.Clear()
        ComboBoxEdit4.Properties.Items.Clear()
        Dim cmd As New SqlCommand(" SELECT * from TbBan Order by BanDes", cnCo)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit2.Properties.Items.Add(dataRd.Item("BanDes"))
            ComboBoxEdit4.Properties.Items.Add(dataRd.Item("BanCod"))
        End While
        dataRd.Close()
        If ComboBoxEdit2.Properties.Items.Count = 1 Then ComboBoxEdit2.SelectedIndex = 0
    End Sub
    Private Sub Popolaprinter()
        ComboBoxEdit3.Properties.Items.Clear()
        Dim x As Int16 = -1
        PS = -1
        Dim Cmd = New SqlCommand("SELECT * from TbLaser where LasTipo = 'A' or LasTipo = 'F' or LasTipo = 'L'", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            x = x + 1
            ComboBoxEdit3.Properties.Items.Add(dataRd.Item("LasFile"))
            ReDim Preserve TIPOLP(x), RESETLP(x)
            TIPOLP(x) = dataRd.Item("LasTipo")
            RESETLP(x) = dataRd.Item("LasReset")
            If dataRd.Item("LasDEFAULT") = "D" And ComboBoxEdit3.SelectedIndex = -1 Then ComboBoxEdit3.EditValue = dataRd.Item("LasFile") ''' STAMPANTE DI DEFAULT
        End While
        dataRd.Close()
    End Sub
    Sub Apertura()
        GroupControl8.Visible = False
        '' LIMITE = 25.82 
        LIMITE = 100.0 '' IMPORTO MINIMO DI VERSAMENTO dal 16022024 -- 100.00
        MT(0) = "MENSILE"
        MT(1) = "TRIMESTRALE"
        TRI(1) = " I^ TRIMESTRE "
        TRI(2) = " II^ TRIMESTRE "
        TRI(3) = " III^ TRIMESTRE "
        TRI(4) = " IV^ TRIMESTRE "
        CheckEdit1.Checked = False
        CheckEdit2.Checked = False
        If Lettura = True Then CheckEdit2.Visible = False
        Dim x As Int16
        Dim Cmd As New SqlCommand("SELECT distinct top 8 RivaAnno from TbRegIva Order by RivaAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        ComboBoxEdit1.EditValue = ""
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("RIvaAnno"))
        End While
        dataRd.Close()
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Exit Sub
        For x = 0 To ComboBoxEdit1.Properties.Items.Count - 1
            If ComboBoxEdit1.Properties.Items(x) = CDate(DateEdit1.EditValue).Year Then
                PopolaElementi(x)
                Exit Sub
            End If
        Next
        If ComboBoxEdit1.SelectedIndex = -1 Then PopolaElementi(0)
        OKCHIUDO = False
    End Sub
    Sub PopolaElementi(ByVal i As Int16)
        ComboBoxEdit1.SelectedIndex = i
        If CDate(DateEdit1.EditValue).Year > Val(ComboBoxEdit1.EditValue) Then
            DateEdit1.EditValue = "31/12/" & ComboBoxEdit1.EditValue
        Else
            DateEdit1.EditValue = Today
        End If
        REGIME = 0
        ESENTE = 0
        CreditoP = 0
        CreditoAnnoP = 0
        AccontoP = 0
        TRIM = False
        Dim Cmd As New SqlCommand("SELECT * from TbAzi Where AziAnnoLavoro = " & Val(ComboBoxEdit1.EditValue), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            REGIME = dataRd.Item("AziRegimeIva")
            ESENTE = dataRd.Item("AziESENTE")
            COAZI = dataRd.Item("AziCod")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT * from TbVers Where IvaVMese = 0 and IvaVDelega = 0 and IvaVAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CreditoAnnoP = dataRd.Item("IvaVVersam")
        End While
        dataRd.Close()

        Dim Str As String
        Str = "SELECT  * from TbVers Where IvaVChiusura = 1 and IvaVAnno = " & Val(ComboBoxEdit1.EditValue)
        If REGIME = 0 Then
            Str = Str & " and IvaVMese > 0 "
        Else
            Str = Str & " and ( IvaVMese = 3 or IvaVMese = 6 Or IvaVMese = 9 )"
        End If
        UCHI = 0
        DataAl = Today
        Cmd = New SqlCommand(Str & " order by IvaVAnno,IvaVMese ", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UCHI = dataRd.Item("IvaVmese")
            CreditoP = dataRd.Item("IvaVVersam")
        End While
        dataRd.Close()
        If UCHI = 0 Then CreditoP = CreditoAnnoP
        If CreditoP > LIMITE Then CreditoP = 0
        If REGIME = 0 Then
            MESE = UCHI + 1
            Formaday()
            If MESE < 13 Then
                GroupControl12.Text = "al MESE di " & Format(DataAl, "MMMM yyyy").ToUpper
            Else
                GroupControl12.Text = "ANNUALE " & Format(DataAl, "yyyy").ToUpper
            End If
            ''''''' da gestire iva annuale '''''' MIN = 1 MAX = 12
            MIN = MESE
            MAX = MESE
            TRIM = False
        Else
            MESE = UCHI + 3
            Formaday()
            GroupControl12.Text = "al " & TRI(Val(MESE / 3)) & ComboBoxEdit1.EditValue
            MIN = UCHI + 1
            MAX = MESE
            TRIM = True
        End If
        GroupControl14.Text = MT(Val(REGIME))
        GroupControl14.Refresh()
        GroupControl12.Refresh()
        DateEdit1.EditValue = DataAl
        DateEdit2.EditValue = DataAl.AddDays(16)
        If REGIME = 1 Then DateEdit2.EditValue = DataAl.AddMonths(1).AddDays(16)
        EsisteVers = False
        Cmd = New SqlCommand("SELECT * from TbVers Where IvaVMese = " & MAX & " And IvaVAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            EsisteVers = True
        End While
        dataRd.Close()
        If MAX <> 12 Then Exit Sub
        Cmd = New SqlCommand("SELECT * from TbVers Where IvaVMese = 14 and IvaVChiusura = 0 and IvaVAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            AccontoP = dataRd.Item("IvaVVersam")
        End While
        dataRd.Close()
    End Sub
    Sub Formaday()
        If MESE = 13 Then
            DataAl = CDate("1/" & 12 & " /" & ComboBoxEdit1.EditValue)
        Else
            DataAl = CDate("1/" & MESE & " /" & ComboBoxEdit1.EditValue)
        End If
        DataAl = CDate(Date.DaysInMonth(DataAl.Year, DataAl.Month) & "/" & DataAl.Month & "/" & DataAl.Year)
    End Sub
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        Dim x As Int16
        x = ComboBoxEdit1.SelectedIndex
        If x = -1 Or Sw = 0 Then Exit Sub
        PopolaElementi(x)
        ' inserito popolacorrispettivi per cambiare data al movimento di scorporo corrispettivi relativo al mese di dicembre
        PopolaCorrispettivi()
        DateEdit1.Focus()
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        ButtonF9.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        If VerificaRegistri() = False And CheckEdit3.Checked = False Then GoTo ConTantiSaluti
        If CheckEdit3.Checked = True Then
            IvaSim.IvaSimulata(DataAl, MIN, MAX)
            GoTo Prosegui
        End If
        If OKGIRO = False Or CheckEdit2.Checked = False Then GoTo Prosegui
        If ImageComboBoxEdit2.SelectedIndex = -1 Then
            ImageComboBoxEdit2.Focus()
            Exit Sub
        End If
        If LeggiConto(TextEdit20.EditValue, TextEdit21) = False Then
            TextEdit20.Focus()
            Exit Sub
        End If
Prosegui:
        Cursor.Current = Cursors.WaitCursor
        If ComboBoxEdit3.SelectedIndex = -1 Then
            ComboBoxEdit3.Focus()
            Exit Sub
        End If
        ControllaRegChiusura()
        AssegnaBanca()
        SparaStampa()
ConTantiSaluti:
        Me.Close()
    End Sub
    Sub ControllaRegChiusura()
        PP = 0
        OKINTESTA = False
        If REGCHIU > 0 Then GoTo Intesta
        Dim x As Int16
        For x = 9 To 1 Step -8
            Cmd = New SqlCommand("SELECT * from TbRegIva Where RivaTipo = " & x & " and RIvaAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                REGCHIU = dataRd.Item("RIvaNReg")
            End While
            dataRd.Close()
            If REGCHIU > 0 Then GoTo Intesta
        Next
Intesta:
        Cmd = New SqlCommand("SELECT * from TbRegIva Where RivaNReg = " & REGCHIU & " and RIvaAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            PP = dataRd.Item("RIvaNumFog")
            OKINTESTA = dataRd.Item("RIvaInt")
            DESCREG = dataRd.Item("RIvaDesc")
        End While
        dataRd.Close()
    End Sub
    Function VerificaRegistri() As Boolean
        VerificaRegistri = True
        Dim StrUno, Messaggio As String
        Dim y As Int16
        Dim NewDate As Date = DataAl.AddDays(15)
        If CDate(DataAl).Month = 12 Then NewDate = DataAl
        If CDate(DataAl).Year < 2019 Then NewDate = DataAl
        StrUno = "Select * from FnFotoChIva('" & NewDate & "','" & DataAl & "')"
        DsReg = New DataSet
        DaReg = New SqlDataAdapter(StrUno, cnCo)
        DaReg.SelectCommand.CommandTimeout = 300
        DaReg.Fill(DsReg, Tr)
        For y = 1 To DsReg.Tables(Tr).Rows.Count
            RwReg = DsReg.Tables(Tr).Rows(y - 1)
            If RwReg("RivaCh") = True Then REGCHIU = RwReg("RivaNReg")
            If RwReg("ProtCar") <> RwReg("ProtSta") Then
                VerificaRegistri = False
                If CheckEdit3.Checked = False Then
                    Messaggio = "PROTOCOLLI CARICATI  " & RwReg("ProtCar") & Chr(13) _
                              & "PROTOCOLLI STAMPATI  " & RwReg("ProtSta")
                    MsgBox(Messaggio, MsgBoxStyle.Critical, RwReg("RivaDesc"))
                End If
            End If
        Next
    End Function

    Function SparaStampa() As Boolean
        If OKINTESTA = True Then AZI = COAZI Else AZI = ""
        Dim NOMESTAMPA As String = ""
        If CheckEdit4.Checked = True Then
            IvaPeriodoPdf.PrintChiusuraIva(ComboBoxEdit3.EditValue, CheckEdit2.Checked, MIN, MAX, Val(ComboBoxEdit1.EditValue), GroupControl12.Text, _
     DateEdit1.EditValue, ESENTE, CreditoP, AccontoP, LIMITE, TRIM, Versamento, OKCHIUDO, PB, PP, AZI, DESCREG, CheckEdit3.Checked, NOMESTAMPA, Interessi)
        Else
            IvaPeriodoAghi.PrintChiusuraIva(ComboBoxEdit3.EditValue, CheckEdit2.Checked, MIN, MAX, Val(ComboBoxEdit1.EditValue), GroupControl12.Text, _
    DateEdit1.EditValue, ESENTE, CreditoP, AccontoP, LIMITE, TRIM, Versamento, OKCHIUDO, TIPOLP(PS), RESETLP(PS), PB, PP, AZI, DESCREG, CheckEdit3.Checked, Interessi)
        End If
        If OKCHIUDO = False Then Exit Function
        If CheckEdit2.Checked = True Then
            AggiornaVers() ''' AGGIORNA Versamenti
            If CreditoAnnoP < 0 Then AggCreditoAnnoP() ''' aggiorna creditoannoP
            AggPagine()
            If OKGIRO = True Then GirocontoCorrisp()
            If OkPlafond = True Then
                EsegueSql("EXEC XCREAPLAF @DATI='" & CDate("01/" & DataAl.Month & "/" & DataAl.Year).ToShortDateString & "',@DATF='" & DataAl.ToShortDateString & "'", cnCo)
            End If
        End If
        If NOMESTAMPA > "" And CheckEdit4.Checked = True Then
            StampaInPdf(NOMESTAMPA)
            Exit Function
        End If
        If TIPOLP(PS) = "F" Then
            EsegueAnteprima(ComboBoxEdit3.EditValue)
        End If
    End Function
    Sub AggPagine()
        Cmd = New SqlCommand("Update TbRegIva set RivaNumFog = " & PP & " where RivaNReg =  " & REGCHIU & " and RivaAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        Cmd.ExecuteNonQuery()
    End Sub
    Sub AggiornaVers()
        Dim Str As String
        If EsisteVers = True Then
            Str = "Update TbVers set IvaVVersam = @Versam ,IvaVLiquida = @Liquida,IvaVdata = @DataV,IvaVChiusura = @Chiusura,IvaVAbi = @Abi,IvaVCab = @Cab,IvaVInteressi=@Interessi Where IvaVMese = @Mese and IvaVAnno = @Anno"
        Else
            Str = "INSERT INTO TbVers (IvaVAnno,IvaVmese,IvaVVersam,IvaVLiquida,IvaVData,IvaVChiusura,IvaVAbi,IvaVCab,IvaVInteressi) Values (@Anno,@Mese,@Versam,@Liquida,@DataV,@Chiusura,@Abi,@Cab,@Interessi)"
        End If
        Dim QABI As New SqlCommand(" SELECT * from TbBan Where BanCod = " & PB, cnCo)
        Dim Abbi, Caab As Int32
        Abbi = 0
        Caab = 0
        dataRd = QABI.ExecuteReader
        While dataRd.Read
            Abbi = dataRd.Item("BanAbi")
            Caab = dataRd.Item("BanCab")
        End While
        dataRd.Close()
        Cmd = New SqlCommand(Str, cnCo)
        Dim p1 As New SqlParameter("@Anno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@Mese", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@Versam", SqlDbType.Decimal)
        Dim p4 As New SqlParameter("@DataV", SqlDbType.SmallDateTime)
        Dim p5 As New SqlParameter("@Chiusura", SqlDbType.Bit)
        Dim p6 As New SqlParameter("@Liquida", SqlDbType.Decimal)
        Dim p7 As New SqlParameter("@Abi", SqlDbType.Int)
        Dim p8 As New SqlParameter("@Cab", SqlDbType.Int)
        Dim p9 As New SqlParameter("@Interessi", SqlDbType.Decimal)
        p1.Value = Val(ComboBoxEdit1.EditValue)
        p2.Value = MAX
        p3.Value = Versamento
        p4.Value = DateEdit2.EditValue
        p5.Value = 1
        p6.Value = Versamento
        p7.Value = Abbi
        p8.Value = Caab
        p9.Value = Interessi
        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.Parameters.Add(p6)
        Cmd.Parameters.Add(p7)
        Cmd.Parameters.Add(p8)
        Cmd.Parameters.Add(p9)
        Cmd.ExecuteNonQuery()
    End Sub
    Sub AssegnaBanca()
        If ComboBoxEdit2.SelectedIndex = -1 Then
            PB = -1
        Else
            ComboBoxEdit4.SelectedIndex = ComboBoxEdit2.SelectedIndex
            PB = ComboBoxEdit4.EditValue
        End If
    End Sub
    Sub AggCreditoAnnoP()
        Cmd = New SqlCommand("Update TbVers set IvaVDelega = 1 Where IvaVMese = 0 and IvaVAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub DateEdit1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.EditValueChanged
        If Sw = 0 Then Exit Sub
        If DateEdit1.EditValue <> DataAl Then DateEdit1.EditValue = DataAl
    End Sub

    Private Sub ComboBoxEdit3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit3.SelectedIndexChanged
        PS = ComboBoxEdit3.SelectedIndex
        If PS = -1 Then ComboBoxEdit3.EditValue = ""
    End Sub
    Private Sub TbLeggi1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi1.Enter
        If LeggiConto(TextEdit20.EditValue, TextEdit21) = False Then TextEdit20.Focus() Else TextEdit22.Focus()
    End Sub
    Private Sub DatBox3_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit3.Validated
        DateEdit4.EditValue = DateEdit3.EditValue
    End Sub
    Private Sub DatBox4_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit4.Validated
        If DateEdit4.EditValue > DateEdit3.EditValue Then DateEdit4.EditValue = DateEdit3.EditValue
    End Sub
    Function LeggiConto(ByRef CodCo As String, ByRef Anagraf As TextEdit) As Boolean
        Anagraf.Text = ""
        LeggiConto = False
        AggiustaConto(CodCo) ''''' verifica il punto se e' un sottoconto
        Dim Str As String = "SELECT * from TbPia where PiaCodCo = '" & CodCo & "'"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Anagraf.Text = dataRd("PiaAnaCo")
            LeggiConto = True
        End While
        dataRd.Close()
        If LeggiConto = True Then Exit Function
        If Val(CodCo) < 1000 Then Exit Function
        CodCo = CodCo.PadLeft(5, "0")
        Str = "SELECT * from TbAna where AnaCoD = '" & CodCo & "'"
        Cmd = New SqlCommand(Str, cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Anagraf.Text = dataRd("AnaDesc")
            LeggiConto = True
        End While
        dataRd.Close()
    End Function
    Function AggiustaConto(ByRef CodCo As String) As Boolean
        Dim x As Int16
        For x = 1 To Len(CodCo)
            If Mid(CodCo, x, 1) = "." Then
                CodCo = Format(Val(Mid(CodCo, 1, x - 1)), "00") & "." & Format(Val(Mid(CodCo, x + 1, Len(CodCo) - (x - 1))), "00")
                Exit Function
            End If
        Next
    End Function
    Sub GirocontoCorrisp()
        Dim StrUno, StrDue, CONTO As String
        Dim IVA, TOTALEIVA, PARZIALE As Decimal
        StrDue = "select * from ##XSCO"
        DataDal = CDate("1/" & MIN & "/" & DataAl.Year)
        Dim y, k As Int16
        For y = 1 To NRCORR(0)
            StrUno = "EXEC XSCORRIS @DAL = '" & DataDal & "',@AL = '" & DataAl & "',@NREG = " & NRCORR(y)
            EsegueSql(StrUno, cnCo)
            DsPno = New DataSet(Pn)
            DaPno = New SqlDataAdapter(StrDue, cnCo)
            DaPno.Fill(DsPno, Pn)
            PARZIALE = 0
            For k = 1 To DsPno.Tables(Pn).Rows.Count
                RwPno = DsPno.Tables(Pn).Rows(k - 1)
                CONTO = RwPno("CONTO")
                IVA = RwPno("IVA")
                TOTALEIVA = RwPno("TOTALEIVA")
                PARZIALE = PARZIALE + IVA
                If k = DsPno.Tables(Pn).Rows.Count Then IVA = IVA + (TOTALEIVA - PARZIALE)
                If IVA <> 0 Then RegistraMovimenti(CONTO, IVA, NRCORR(y))
            Next
        Next
    End Sub
    Sub RegistraMovimenti(ByVal DARE As String, ByVal IMPORTO As Decimal, ByVal NREG As Int16)
        Dim Scrivi As String = "INSERT INTO TbPri (PriId,PriProg,PriDataGio, PriCausale, PriCoDare, PriCoAvere, PriNumProt, PriBisRet, PriCodIva, PriRegIva, PriImpDare, PriImpavere, PriDesc, PriDocEst, PriMeseSk, PriDataEst, PriDescB, PriFl04, PriFl05, PriFl06, PriNsRif, PriSos, PriLinea, PriDocAnn, PriCodPag, PriValuta, PriArtFisc,PriIvaPrint,PriGStampa) " _
& " values(@PriId,@PriProg,@PriDataGio, @PriCausale, @PriCoDare, @PriCoAvere, @PriNumProt, @PriBisRet, @PriCodIva, @PriRegIva, @PriImpDare, @PriImpavere, @PriDesc, @PriDocEst, @PriMeseSk, @PriDataEst, @PriDescB, @PriFl04, @PriFl05, @PriFl06, @PriNsRif, @PriSos, @PriLinea, @PriDocAnn, @PriCodPag, @PriValuta, @PriArtFisc,@PriIvaPrint,@PriGStampa)"
        Dim Wmd As New SqlCommand(Scrivi, cnCo)
        Dim p1 As New SqlParameter("@PriDataGio", SqlDbType.SmallDateTime)
        Dim p2 As New SqlParameter("@PriCausale", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@PriCoDare", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@PriCoAvere", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@PriNumProt", SqlDbType.Int)
        Dim p6 As New SqlParameter("@PriBisRet", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@PriCodIva", SqlDbType.SmallInt)
        Dim p8 As New SqlParameter("@PriRegIva", SqlDbType.SmallInt)
        Dim p9 As New SqlParameter("@PriImpDare", SqlDbType.Decimal)
        Dim p10 As New SqlParameter("@PriImpavere", SqlDbType.Decimal)
        Dim p11 As New SqlParameter("@PriDesc", SqlDbType.VarChar)
        Dim p12 As New SqlParameter("@PriDocEst", SqlDbType.Int)
        Dim p13 As New SqlParameter("@PriMeseSk", SqlDbType.VarChar)
        Dim p14 As New SqlParameter("@PriDataEst", SqlDbType.SmallDateTime)
        Dim p15 As New SqlParameter("@PriDescB", SqlDbType.VarChar)
        Dim p16 As New SqlParameter("@PriFl04", SqlDbType.SmallInt)
        Dim p17 As New SqlParameter("@PriFl05", SqlDbType.SmallInt)
        Dim p18 As New SqlParameter("@PriFl06", SqlDbType.SmallInt)
        Dim p19 As New SqlParameter("@PriNsRif", SqlDbType.VarChar)
        Dim p20 As New SqlParameter("@PriSos", SqlDbType.VarChar)
        Dim p21 As New SqlParameter("@PriLinea", SqlDbType.VarChar)
        Dim p22 As New SqlParameter("@PriDocAnn", SqlDbType.SmallInt)
        Dim p23 As New SqlParameter("@PriCodPag", SqlDbType.SmallInt)
        Dim p24 As New SqlParameter("@PriValuta", SqlDbType.Decimal)
        Dim p25 As New SqlParameter("@PriArtFisc", SqlDbType.Int)
        Dim p26 As New SqlParameter("@PriID", SqlDbType.Int)
        Dim p27 As New SqlParameter("@PriProg", SqlDbType.SmallInt)
        Dim p28 As New SqlParameter("@PriIvaPrint", SqlDbType.Bit)
        Dim p29 As New SqlParameter("@PriGStampa", SqlDbType.Bit)
        Dim Scheggia As Int16 = 0
        Dim Articolo As Int32 = RileggoLocked()
        Dim Total As Decimal = 0
        LeggiUltimo(DateEdit4.EditValue)
        TextEdit2.EditValue = Articolo
        Wmd.Parameters.Clear()
        p12.Value = 0
        p22.Value = CDate(DateEdit4.EditValue).Year
        p3.Value = DARE
        p4.Value = TextEdit20.EditValue
        p9.Value = IMPORTO
        p10.Value = IMPORTO
        p12.Value = 0
        p1.Value = CDate(DateEdit3.EditValue)
        p2.Value = ImageComboBoxEdit2.EditValue
        p5.Value = Articolo
        p6.Value = ""
        p7.Value = 0
        p8.Value = 0
        p11.Value = TextEdit22.EditValue
        p13.Value = ""
        p14.Value = CDate(DateEdit4.EditValue)
        p15.Value = "REGISTRO IVA N. " & NREG
        p16.Value = 0
        p17.Value = 0
        p18.Value = 0
        p19.Value = ""
        p20.Value = ""
        p21.Value = ""
        p23.Value = 0
        p24.Value = 0
        p25.Value = 0
        p26.Value = ProgId
        p27.Value = 1
        p28.Value = 0
        p29.Value = 0
        Wmd.Parameters.Add(p1)
        Wmd.Parameters.Add(p2)
        Wmd.Parameters.Add(p3)
        Wmd.Parameters.Add(p4)
        Wmd.Parameters.Add(p5)
        Wmd.Parameters.Add(p6)
        Wmd.Parameters.Add(p7)
        Wmd.Parameters.Add(p8)
        Wmd.Parameters.Add(p9)
        Wmd.Parameters.Add(p10)
        Wmd.Parameters.Add(p11)
        Wmd.Parameters.Add(p12)
        Wmd.Parameters.Add(p13)
        Wmd.Parameters.Add(p14)
        Wmd.Parameters.Add(p15)
        Wmd.Parameters.Add(p16)
        Wmd.Parameters.Add(p17)
        Wmd.Parameters.Add(p18)
        Wmd.Parameters.Add(p19)
        Wmd.Parameters.Add(p20)
        Wmd.Parameters.Add(p21)
        Wmd.Parameters.Add(p22)
        Wmd.Parameters.Add(p23)
        Wmd.Parameters.Add(p24)
        Wmd.Parameters.Add(p25)
        Wmd.Parameters.Add(p26)
        Wmd.Parameters.Add(p27)
        Wmd.Parameters.Add(p28)
        Wmd.Parameters.Add(p29)
        Wmd.ExecuteNonQuery()
        SbloccoLocked()
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        If OkLDP = True Then LancioCDC()
    End Sub

    Sub LancioCDC()
        EsegueSql("exec XRECUPEROCDCCORRISPETTIVI @ID=" & ProgId & ", @LDP = 0", CnDc) '' PER ORA FISSO ATAVOLA NON CLASSIFICATO
        '''RipCdc.PIIDD = ProgId
        '''RipCdc.PNdoc = RwFRI("PriDocEst").ToString
        '''RipCdc.PNreg = RwFRI("PriRegIva").ToString
        '''RipCdc.PProt = RwFRI("PriNumProt").ToString
        '''RipCdc.PDatDoc = CDate(RwFRI("PriDataEst")).ToShortDateString
        '''Gesterna.ShowDialog()
    End Sub
    Private Function LeggiUltimo(ByVal dataGio As Date) As Boolean
        Dim ultimo As String = "INSERT INTO TbIDP (IDdata) values(@PriDataGio)"
        Dim UltimaRiga As New SqlCommand("SELECT  @@IDENTITY ", cnCo)
        Dim Qmd As New SqlCommand(ultimo, cnCo)
        Dim px As New SqlParameter("@PriDataGio", SqlDbType.SmallDateTime)
        px.Value = CDate(dataGio)
        Qmd.Parameters.Add(px)
        Qmd.ExecuteNonQuery()
        ProgId = UltimaRiga.ExecuteScalar
    End Function
    Sub ResetIdP()
        Dim Elimina As String = "DELETE FROM TbIDP WHERE IdPRI = " & ProgId
        Dim Dmd As New SqlCommand(Elimina, cnCo)
        Dmd.ExecuteNonQuery()
    End Sub
    Function SbloccoLocked() As Int16
        Dim Del As New SqlCommand("Delete from TMPlock WITH (TABLOCKX) where IdPrNota = 1 ", cnCo)
        Del.ExecuteNonQuery()
        Return 0
    End Function
    Function RileggoLocked() As Int32
        ''' pausa per scrittura articolo nuovo
        Dim Loc As New SqlCommand("Select IdPrNota  from Tmplock WITH (TABLOCKX) where IdPrNota > 0", cnCo)
        Dim Ins As New SqlCommand("Insert into TMPlock WITH (TABLOCKX) (IdPrNota) values (1) ", cnCo)
        Dim pausa As Boolean
Attesa:
        pausa = False
        dataRd = Loc.ExecuteReader
        While dataRd.Read
            pausa = True
        End While
        dataRd.Close()
        If pausa = True Then GoTo Attesa
        Ins.ExecuteNonQuery()
        ''' ' LEGGO DA TBPRI MAX DATAGIO E MAX NUMART
        Dim Str As String = "SELECT isnull(max(PriNumProt),0) from TbPri WITH (TABLOCKX) where PriregIva = 0"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            RileggoLocked = dataRd.Item(0) + 1
        End While
        dataRd.Close()
    End Function

    Private Sub CheckEdit2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        If CheckEdit2.Checked = True Then CheckEdit3.Checked = False
    End Sub
    Private Sub CheckEdit3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit3.CheckedChanged
        If CheckEdit3.Checked = True Then CheckEdit2.Checked = False
    End Sub

    Private Sub CheckEdit4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit4.Click
        ComboBoxEdit3.Enabled = CheckEdit4.Checked
    End Sub
End Class