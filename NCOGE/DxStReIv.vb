Imports DXBASE
Imports NPRINT
Imports NCCOM
Imports System.Data.SqlClient

Public Class DxStReIv
    Dim Tr As String = "TReg"
    Dim DsReg As DataSet
    Dim DaReg As SqlDataAdapter
    Dim RwReg As DataRow

    Dim Ti As String = "TIva"
    Dim DsIva As DataSet
    Dim DaIva As SqlDataAdapter
    Dim RwIva As DataRow

    Dim Tp As String = "TPva"
    Dim DsPva As DataSet
    Dim DaPva As SqlDataAdapter
    Dim RwPva As DataRow
    Dim CbPva As SqlCommandBuilder

    Dim Tc As String = "TCrs" ''' CORRISPETTIVI LORDI PER CPT
    Dim DsCrs As DataSet
    Dim DaCrs As SqlDataAdapter
    Dim RwCrs As DataRow
    Dim CbCrs As SqlCommandBuilder

    Dim Sw As Int16 = 0
    Dim IdBlk, PP As Int32
    Dim OkFlash, OKINTESTA As Boolean

    Dim StrReg, StrRie, QUALEREG, MT(1), TRI(4), REGIME, TIPOLP(), RESETLP(), FILEST, AZI, COAZI, DESCREG, SERIEL As String
    Dim DataAl As Date
    Dim CreditoP As Decimal

    Dim NRREG, TIPOREG, MIN, MAX, MESE, UCHI, DM, AM, PS As Int16

    Dim RwX As DataRow
    Dim CiiPam As New ArrayList
    Dim Elettronica As String = "XI12019"

    Private Sub DxStReIv_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Sw = 0 Then
            Apertura()
            Popolaprinter()
            PopolaIvaPubblica()
            Sw = 1
        End If
    End Sub
    Sub PopolaIvaPubblica()
        REM lettura codici iva pubblica amministrazione
        CiiPam.Clear()
        Cmd = New SqlCommand("SELECT * from TbPaCii where PaTipo = 'IVA' order by PaCodIva", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CiiPam.Add(dataRd.Item("PaCodIva"))
        End While
        dataRd.Close()
    End Sub
    Private Sub Popolaprinter()
        ComboBoxEdit3.Properties.Items.Clear()
        ComboBoxEdit3.SelectedIndex = -1
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
        MT(0) = "MENSILE"
        MT(1) = "TRIMESTRALE"
        TRI(1) = " I^ TRIMESTRE "
        TRI(2) = " II^ TRIMESTRE "
        TRI(3) = " III^ TRIMESTRE "
        TRI(4) = " IV^ TRIMESTRE "
        CreditoP = 0
        CheckEdit1.Checked = False
        CheckEdit2.Checked = False
        If Lettura = True Then CheckEdit2.Visible = False
        ButtonF11.Enabled = False
        Dim Cmd As New SqlCommand("SELECT distinct top 8 RivaAnno from TbRegIva Order by RivaAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        Dim x As Int16
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("RIvaAnno"))
        End While
        dataRd.Close()
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Exit Sub
        For x = 0 To ComboBoxEdit1.Properties.Items.Count - 1
            If ComboBoxEdit1.Properties.Items(x) = CDate(DateEdit1.EditValue).Year Then
                PopolaGrid(x)
                Exit Sub
            End If
        Next
        If ComboBoxEdit1.SelectedIndex = -1 Then PopolaGrid(0)
    End Sub
    Sub PopolaGrid(ByVal i As Int16)
        ComboBoxEdit1.SelectedIndex = i
        Dim StrUno As String
        StrUno = "Select * from FnFotoRIva(" & Val(ComboBoxEdit1.EditValue) & ") Where RivaTipo <> 9 order by RivaNreg"
        DsReg = New DataSet
        DaReg = New SqlDataAdapter(StrUno, cnCo)
        DaReg.SelectCommand.CommandTimeout = 300
        DaReg.Fill(DsReg, Tr)
        GridControl1.DataSource = DsReg.Tables(Tr)
        GridControl1.Refresh()
        GridView1.ClearSelection()
        If CDate(DateEdit1.EditValue).Year > Val(ComboBoxEdit1.EditValue) Then
            DateEdit1.EditValue = CDate("31/12/" & ComboBoxEdit1.EditValue.ToString)
        Else
            DateEdit1.EditValue = Today
        End If
        REGIME = "0"
        Dim Cmd As New SqlCommand("SELECT * from TbAzi Where AziAnnoLavoro = " & Val(ComboBoxEdit1.EditValue), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            REGIME = dataRd.Item("AziRegimeIva")
            COAZI = dataRd.Item("AziCod")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT * from TbVers Where IvaVMese = 0 and IvaVChiusura = 0 and IvaVAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CreditoP = dataRd.Item("IvaVVersam")
        End While
        dataRd.Close()
        Dim Str As String
        Str = "SELECT * from TbVers Where IvaVChiusura = 1 and IvaVAnno = " & Val(ComboBoxEdit1.EditValue)
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
        End While
        dataRd.Close()
        If UCHI = 12 Then
            GroupControl8.Text = "REGISTRI IVA GIA' STAMPATI"
            Exit Sub
        End If
        If REGIME = 0 Then
            MESE = UCHI + 1
            Formaday()
            GroupControl8.Text = "del MESE di " & Format(DataAl, "MMMM yyyy").ToUpper
            DM = MESE
            AM = MESE
        Else
            MESE = UCHI + 3
            Formaday()
            GroupControl8.Text = "del " & TRI(Val(MESE / 3)) & ComboBoxEdit1.EditValue
            DM = UCHI + 1
            AM = MESE
        End If
        GroupControl11.Text = MT(Val(REGIME))
        GroupControl11.Refresh()
        GroupControl8.Refresh()
        DateEdit1.EditValue = DataAl
        DateEdit1.ErrorText = ""
    End Sub
    Sub Formaday()
        DataAl = CDate("1/" & MESE & " /" & ComboBoxEdit1.EditValue)
        DataAl = CDate(Date.DaysInMonth(DataAl.Year, DataAl.Month) & "/" & DataAl.Month & "/" & DataAl.Year)
    End Sub
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        Dim x As Int16
        x = ComboBoxEdit1.SelectedIndex
        If x = -1 Or Sw = 0 Then Exit Sub
        PopolaGrid(x)
        DateEdit1.Focus()
    End Sub
    Private Sub RepositoryItemCheckEdit3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemCheckEdit1.CheckedChanged
        RwX = GridView1.GetFocusedDataRow
        RwX("RivaPrintIniziale") = sender.checked
        DsReg.Tables(Tr).AcceptChanges()
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim x, Y As Int16
        If PS = -1 Then Exit Sub
        If ControllaDataBollo() = False Then Exit Sub
        Cursor.Current = Cursors.WaitCursor
        If OkFlash = False Then
            OkFlash = True
            IdBlk = semaforo("Registri Iva AL " & Today.Date)
        Else
            PuliziaFlash()
        End If
        Y = DsReg.Tables(Tr).Rows.Count
        ProgressBarControl1.Visible = True
        ProgressBarControl1.Properties.Minimum = 1
        ProgressBarControl1.Properties.Maximum = Y + 2
        ProgressBarControl1.Position = 0
        ProgressBarControl1.Properties.Step = 1
        For x = Y To 1 Step -1
            ProgressBarControl1.PerformStep()
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            RwX = GridView1.GetDataRow(x - 1)
            If RwX("RivaPrintIniziale") = True Then
                CaricaDati(x - 1)
            End If
        Next
        ProgressBarControl1.PerformStep()
        Application.DoEvents()
        If CheckEdit2.Checked = True Then
            MIN = 0
            MAX = Y
            Preparadati()
            Exit Sub
        End If
        For x = 1 To Y
            RwX = GridView1.GetDataRow(x - 1)
            If RwX("RivaPrintIniziale") = True Then
                NRREG = RwX("RivaNreg")
                TIPOREG = RwX("RivaTipo")
                QUALEREG = RwX("TipoDesc")
                DESCREG = RwX("RivaDesc")
                PP = RwX("RivaNumFog")
                OKINTESTA = RwX("RivaInt")
                SERIEL = RwX("RivaSL")
                SparaStampa()
            End If
        Next
        Me.Close()
    End Sub
    Sub Preparadati()
Ritorno:
        ButtonF9.Enabled = False
        MIN = MIN + 1
        If MIN > MAX Then
            Me.Close()
            Exit Sub
        End If
        RwX = GridView1.GetDataRow(MIN - 1)
        If RwX("RivaPrintIniziale") = True Then
            NRREG = RwX("RivaNreg")
            TIPOREG = RwX("RivaTipo")
            QUALEREG = RwX("TipoDesc")
            DESCREG = RwX("RivaDesc")
            PP = RwX("RivaNumFog")
            OKINTESTA = RwX("RivaInt")
            SERIEL = RwX("RivaSL")
            ButtonF11.Enabled = True
            GroupControl3.Refresh()
            If OKINTESTA = False Then
                MemoEdit1.Text = "INSERIRE REGISTRO IVA N. " & NRREG & " " & QUALEREG.ToUpper
                ButtonF11.Focus()
            Else
                SparaStampa()
                GoTo Ritorno
            End If
        Else
            GoTo Ritorno
        End If
    End Sub
    Function SparaStampa() As Boolean
        If OKINTESTA = True Then AZI = COAZI Else AZI = ""
        Dim NOMESTAMPA As String = ""
        Dim TIPOSTAMPA2018 As Boolean = False
        Try
            Cmd = New SqlCommand("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TbIVA2018]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) BEGIN Select 1 END", cnCo)
            If Cmd.ExecuteScalar > 0 Then TIPOSTAMPA2018 = True
        Catch ex As Exception

        End Try
        If ComboBoxEdit1.EditValue > 2018 And TIPOSTAMPA2018 = False Then
            If CheckEdit3.Checked = True Then
                RegIvaPdf2019.PrintRegIva(NRREG, IdBlk, TIPOREG, ComboBoxEdit3.EditValue, CheckEdit2.Checked, QUALEREG, GroupControl8.Text, CreditoP, Val(ComboBoxEdit1.EditValue), NOMESTAMPA, PP, AZI, DESCREG, SERIEL)
            Else
                RegIvaAghi2019.PrintRegIva(NRREG, IdBlk, TIPOREG, ComboBoxEdit3.EditValue, CheckEdit2.Checked, QUALEREG, GroupControl8.Text, CreditoP, Val(ComboBoxEdit1.EditValue), TIPOLP(PS), RESETLP(PS), FILEST, PP, AZI, DESCREG, SERIEL)
            End If
        Else
            If CheckEdit3.Checked = True Then
                RegIvaPdf.PrintRegIva(NRREG, IdBlk, TIPOREG, ComboBoxEdit3.EditValue, CheckEdit2.Checked, QUALEREG, GroupControl8.Text, CreditoP, Val(ComboBoxEdit1.EditValue), NOMESTAMPA, PP, AZI, DESCREG, SERIEL)
            Else
                RegIvaAghi.PrintRegIva(NRREG, IdBlk, TIPOREG, ComboBoxEdit3.EditValue, CheckEdit2.Checked, QUALEREG, GroupControl8.Text, CreditoP, Val(ComboBoxEdit1.EditValue), TIPOLP(PS), RESETLP(PS), FILEST, PP, AZI, DESCREG, SERIEL)
            End If
        End If

        If CheckEdit2.Checked = True Then
            AddIva(NRREG, "SUMIVAP") ''' AGGIORNA IVA PERIODICA 
            If CreditoP < 0 And TIPOREG = 2 Then AggCreditoAnnoP()
            If TIPOREG = 5 Then AddIva(NRREG, "SUMCRSP")
            AggPagine()
        End If
        ButtonF11.Enabled = False
        If NOMESTAMPA > "" And CheckEdit3.Checked = True Then
            StampaInPdf(NOMESTAMPA)
            Exit Function
        End If
        If TIPOLP(PS) = "F" Then
            EsegueAnteprima(FILEST)
        End If
    End Function
    Private Sub ButtonF11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        SparaStampa()
        Preparadati()
    End Sub
    Sub AggPagine()
        Cmd = New SqlCommand("Update TbRegIva set RivaNumFog = " & PP & " where RivaNReg =  " & NRREG & " and RivaAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        Cmd.ExecuteNonQuery()
    End Sub
    Sub AggCreditoAnnoP()
        Cmd = New SqlCommand("Update TbVers set IvaVChiusura = 1 Where IvaVMese = 0 and IvaVAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        Cmd.ExecuteNonQuery()
        CreditoP = 0
    End Sub
    Sub AddIva(ByVal NRREG As Int16, ByVal XP As String)
        'xp = addiva Aggiunge Iva Precedente, xp = sumiva Sostituisce iva Totale
        EsegueSql(" EXEC " & XP & " @ANNO = " & Val(ComboBoxEdit1.EditValue) & ", @REG = " & NRREG & ", @DM = " & DM & ", @AM = " & AM & ", @BLOCK = " & IdBlk, cnCo)
    End Sub
    Sub CaricaDati(ByVal Riga As Int16)
        Dim d1 As String
        NRREG = RwX("RivaNreg")
        TIPOREG = RwX("RivaTipo")
        QUALEREG = RwX("TipoDesc")
        DESCREG = RwX("RivaDesc")
        SERIEL = RwX("RivaSL")
        d1 = CDate(DateEdit1.EditValue).ToShortDateString
        MESE = DateEdit1.EditValue.Month
        If ComboBoxEdit1.EditValue < 2019 Then Elettronica = "XI1" Else Elettronica = "XI12019"
        Try
            Cmd = New SqlCommand("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TbIVA2018]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) BEGIN Select 1 END", cnCo)
            If Cmd.ExecuteScalar > 0 Then Elettronica = "XI1"
        Catch ex As Exception

        End Try
        EsegueSql(" EXEC " & Elettronica & "  @FAL ='" & d1 & "' , @REG = " & NRREG & ", @DBVDOX = '" & DbVdox & "', @BLOCK = " & IdBlk, cnCo)
        'carica iva periodo
        Dim StrDue As String
        Dim y As Int32
        StrReg = "Select * from CRREGIVA WHERE PRegNumReg = " & NRREG & " AND PREGID = " & IdBlk & " ORDER BY PRegNumReg, PRegNumProt, PRegProtBis, PRegPriId,PRegPriProg"
        DsIva = New DataSet
        DaIva = New SqlDataAdapter(StrReg, cnCo)
        DaIva.SelectCommand.CommandTimeout = 300
        DaIva.Fill(DsIva, Ti)
        StrDue = "Select * from TMPIVAP WHERE TivaPanno = 2995 " ' solo per inizializzare il dataset
        DsPva = New DataSet
        DaPva = New SqlDataAdapter(StrDue, cnCo)
        DaPva.Fill(DsPva, Tp)
        CbPva = New SqlCommandBuilder(DaPva)
        If TIPOREG = 5 Then
            Dim StrTre As String
            StrTre = "Select * from TMPCORR WHERE TCorrAnno = 2995 " ' solo per inizializzare il dataset
            DsCrs = New DataSet
            DaCrs = New SqlDataAdapter(StrTre, cnCo)
            DaCrs.Fill(DsCrs, Tc)
            CbCrs = New SqlCommandBuilder(DaCrs)
        End If
        Dim Resto, NoDet, Merce As Decimal
        Resto = 0
        NoDet = 0
        Merce = 0
        If DsIva.Tables(Ti).Rows.Count = 0 Then
            RwX("RivaPrintIniziale") = Not RwX("RivaPrintIniziale")
            DsIva.Tables(Ti).AcceptChanges()
            Exit Sub
        End If
        For y = 1 To DsIva.Tables(Ti).Rows.Count
            RwIva = DsIva.Tables(Ti).Rows(y - 1)
            If RwIva("PRegCodIva") = 0 Then
                Resto = Resto + RwIva("PRegImpon")
                If RwIva("PRegMerce") = 2 Then
                    Merce = Merce + RwIva("PRegImpon")
                End If
                GoTo Oltre
            End If
            RwPva = DsPva.Tables(Tp).NewRow
            RwPva("TIvaPId") = RwIva("PRegId")
            RwPva("TIvaPAnno") = CDate(RwIva("PRegFinoAl")).Year
            RwPva("TIvaPMese") = CDate(RwIva("PRegFinoAl")).Month
            RwPva("TIvaPRegIva") = RwIva("PRegNumReg")
            RwPva("TIvaPCodIva") = RwIva("PRegCodIva")
            RwPva("TIvaPImpon") = RwIva("PRegImpon") + Resto
            If RwIva("PRegPND") > 0 Then
                NoDet = RwIva("PRegImpIva") * RwIva("PRegPND") / 100
                RwPva("TIvaPIvaND") = Format(NoDet, "0000000000.00")
                RwPva("TIvaPIvaDE") = RwIva("PRegImpIva") - RwPva("TIvaPIvaND")
            Else
                RwPva("TIvaPIvaDE") = RwIva("PRegImpIva")
                RwPva("TIvaPIvaND") = 0
            End If
            REM CONTROLLO SE PUBBLICA AMMINISTRAZIONE
            For J As Int16 = 1 To CiiPam.Count
                If RwIva("PRegCodIva") = CiiPam(J - 1) Then
                    RwPva("TIvaPIvaDE") = 0
                    RwPva("TIvaPIvaND") = RwIva("PRegImpIva")
                    Exit For
                End If
            Next
            ''
            If RwIva("PRegMerce") = 2 Then
                RwPva("TIvaPImpMerce") = RwIva("PRegImpon")
            Else
                RwPva("TIvaPImpMerce") = 0
            End If
            RwPva("TIvaPImpMerce") = RwPva("TIvaPImpMerce") + Merce
            Resto = 0
            Merce = 0
            DsPva.Tables(Tp).Rows.Add(RwPva)
            If TIPOREG = 5 Then
                RwCrs = DsCrs.Tables(Tc).NewRow
                RwCrs("TCorrId") = RwIva("PRegId")
                RwCrs("TCorrAnno") = CDate(RwIva("PRegFinoAl")).Year
                RwCrs("TCorrMese") = CDate(RwIva("PRegFinoAl")).Month
                RwCrs("TCorrRegIva") = RwIva("PRegNumReg")
                RwCrs("TCorrCodIva") = RwIva("PRegCodIva")
                RwCrs("TCorrLordo") = RwIva("PRegImpon")
                RwCrs("TCorrConto") = RwIva("PRegCpt")
                DsCrs.Tables(Tc).Rows.Add(RwCrs)
            End If
Oltre:
        Next
        DaPva.Update(DsPva, Tp)
        DsPva.AcceptChanges()
        If TIPOREG = 5 Then
            DaCrs.Update(DsCrs, Tc)
            DsCrs.AcceptChanges()
            AddIva(NRREG, "ADDCRSP")
        End If
        AddIva(NRREG, "ADDIVAP")
    End Sub

    Private Sub StReIv_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If OkFlash = True Then PuliziaFlash()
    End Sub
    Sub PuliziaFlash()
        If OkFlash = False Then Exit Sub
        Dim Ultimo As New SqlCommand("BEGIN DELETE FROM TMPREGIVA WHERE PREGID = " & IdBlk & " DELETE FROM TMPIVAP WHERE TIVAPID = " & IdBlk & " DELETE FROM TMPCORR WHERE TCORRID = " & IdBlk & " END ", cnCo)
        Ultimo.ExecuteNonQuery()
    End Sub
    Private Sub CheckEdit2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        GroupControl3.Enabled = CheckEdit2.Checked
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit3.SelectedIndexChanged
        PS = ComboBoxEdit3.SelectedIndex
        If PS = -1 Then ComboBoxEdit3.EditValue = ""
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.EditValueChanged
        DateEdit1.ErrorText = ""
        If CheckEdit2.Checked = True And CDate(DateEdit1.EditValue) > DataAl Then
            DateEdit1.EditValue = CDate(DataAl.ToShortDateString)
        End If
    End Sub
    Function ControllaDataBollo() As Boolean
        ControllaDataBollo = True
        If CheckEdit2.Checked = True And CDate(DateEdit1.EditValue) > DataAl Then
            DateEdit1.ErrorText = "DataErrata per stampa in bollo!!!"
            ControllaDataBollo = False
            Exit Function
        Else
            DateEdit1.ErrorText = ""
        End If
    End Function

    Private Sub CheckEdit3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit3.Click
        ComboBoxEdit3.Enabled = CheckEdit3.Checked
    End Sub
End Class