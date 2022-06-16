Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports NCDCO
Imports DevExpress.XtraEditors

Public Class DxInFtCo
    Private Shared ERifProt, ERifAnno, ERifRiva As Integer
    '''Friend WithEvents TextEdit20 As System.Windows.Forms.TextBox
    Public Shared Property NRifProt() As Integer
        Get
            Return ERifProt
        End Get
        Set(ByVal Value As Integer)
            ERifProt = Value
        End Set
    End Property
    Public Shared Property NRifAnno() As Integer
        Get
            Return ERifAnno
        End Get
        Set(ByVal Value As Integer)
            ERifAnno = Value
        End Set
    End Property
    Public Shared Property NRifRiva() As Integer
        Get
            Return ERifRiva
        End Get
        Set(ByVal Value As Integer)
            ERifRiva = Value
        End Set
    End Property
    Dim Sw As Int16 = 0
    Dim Xreg As Int16 = -1
    Dim NumReg, Righe, POSRIG, Irow As Int16
    Dim Pcod, IvaCpt As String
    Dim TotaleFattura, X0, X1, TotaleReg, TotTransito, TotaleResiduo As Decimal
    Dim OkProt, OkFat, OkFcf As Boolean
    Dim ProgId As Int32
    Dim Rispondi As MsgBoxResult


    Dim Fa As String = "FATT"
    Dim DsFat As DataSet
    Dim DaFat As SqlDataAdapter
    Dim RwFat As DataRow
    Dim RwFRI As DataRow

    Dim Ic As String = "VINC"
    Dim DsInc As DataSet
    Dim DaInc As SqlDataAdapter
    Dim RwInc As DataRow


    Dim Ci As String = "CIVA"
    Dim DsCii As DataSet
    Dim DaCii As SqlDataAdapter
    Dim RwCii As DataRow

    Dim Ft As String = "FACF"
    Dim DsFcf As DataSet
    Dim DaFcf As SqlDataAdapter
    Dim RwFcf As DataRow

    Dim Ri As String = "REGI"
    Dim DsReg As DataSet
    Dim DaReg As SqlDataAdapter
    Dim RwReg As DataRow
    Dim RwAGG As DataRow

    Dim OldRegis As String = ""
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim RwX As DataRow
    Dim errorT(1) As String

    Private Sub DxInFtCo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If Sw = 0 Then
            Apertura() : PopolaCii()
            Sw = 1
        End If
        Pulizia(0)
        If ERifProt > 0 Then
            TextEdit1.EditValue = ERifProt
            If ControllaProtocollo(ERifProt) = True Then
                errorT(0) = TextEdit1.ErrorText
                errorT(1) = DateEdit1.ErrorText
                SelectNextControl(TextEdit4, True, True, True, True)
                TextEdit1.ErrorText = errorT(0)
                DateEdit1.ErrorText = errorT(1)
            End If
            ERifProt = 0 : ERifRiva = 0 : ERifAnno = 0
        End If
    End Sub
    Sub Pulizia(ByVal p As Int16)
        TextEdit4.EditValue = ""
        TextEdit3.EditValue = ""
        TextEdit2.EditValue = ""
        DateEdit1.ErrorText = ""
        TextEdit1.ErrorText = ""
        Pcod = ""
        OkProt = False
        AbilitaGroup(0)
        Righe = 0
        POSRIG = 1
        ProgId = 0
        If p = 0 Then
            PulisciGrid()
            ResetNumBox()
            If Xreg > -1 Then
                CaricaIniziale(Xreg)
            End If
            TextEdit1.Focus()
        End If
    End Sub
    Sub PopolaCii()
        ImageComboBoxEdit1.Properties.Items.Clear()
        ImageComboBoxEdit3.Properties.Items.Clear()
        Dim Str As String = "SELECT CiiCod,CiiDes,CiiCau from TbCii order by CiiCod"
        Dim SS As String = ""
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiDes")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
            ImageComboBoxEdit3.Properties.Items.Add(nn)
            If dataRd.Item("CiiCod") > 3 And dataRd.Item("CiiCod") <> 45 Then
                SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau")
                nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
                ImageComboBoxEdit1.Properties.Items.Add(nn)
            End If
        End While
        dataRd.Close()
    End Sub
    Sub ResetNumBox()
        TextEdit15.EditValue = 0
        TextEdit16.EditValue = CDec(0.0)
        ImageComboBoxEdit3.SelectedIndex = -1
        TextEdit20.EditValue = "00.00"
        TextEdit21.EditValue = ""
        TextEdit24.EditValue = "00.00"
        X1 = 0
        X0 = 0
        TotaleFattura = 0
        TotaleReg = 0
        TotaleResiduo = 0
        TotTransito = 0
    End Sub
    Sub AbilitaGroup(ByVal n As Int16)
        If n = 0 Then
            TextEdit1.Enabled = True
            DateEdit1.Enabled = True
            ImageComboBoxEdit2.Enabled = True
            GroupControl1.Enabled = True
            GroupControl3.Enabled = True
            ButtonF11.Enabled = False
            ButtonF1.Enabled = True
            GridControl2.Enabled = True
            GroupControl4.Enabled = False
            GroupControl5.Enabled = False
        ElseIf n = 1 Then
            TextEdit1.Enabled = False
            DateEdit1.Enabled = False
            ImageComboBoxEdit2.Enabled = False
            GroupControl1.Enabled = True
            GroupControl4.Enabled = True
            GroupControl5.Enabled = True
            ButtonF11.Enabled = True
            ButtonF1.Enabled = False
            GridControl2.Enabled = False
        End If
    End Sub
    Sub PulisciGrid()
        DsFat = New DataSet(Fa)
        GridControl1.DataSource = DsFat.Tables(Fa)
        GridControl1.Refresh()
        GridView1.ClearSelection()
    End Sub
    Sub Apertura()
        Dim Cmd As New SqlCommand("SELECT distinct top 5 RivaAnno from TbRegIva Order by RivaAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        Dim x As Int16
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("RIvaAnno"))
        End While
        dataRd.Close()
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Exit Sub
        DsCii = New DataSet(Ci)
        DaCii = New SqlDataAdapter("SELECT * from TbCii order by CiiCod", cnCo)
        DaCii.Fill(DsCii, Ci)
        ComboBoxEdit1.SelectedIndex = -1
        DateEdit1.EditValue = Today
        For x = 0 To ComboBoxEdit1.Properties.Items.Count - 1
            If ERifAnno > 0 And ComboBoxEdit1.Properties.Items(x) = ERifAnno Then
                PopolaRegime(x)
                Exit Sub
            End If
            If ComboBoxEdit1.Properties.Items(x) = CDate(DateEdit1.EditValue).Year And ERifAnno = 0 Then
                PopolaRegime(x)
                Exit Sub
            End If
        Next
        If ComboBoxEdit1.SelectedIndex = -1 Then PopolaRegime(0)
    End Sub
    Sub PopolaRegime(ByVal i As Int16)
        ComboBoxEdit1.SelectedIndex = i
        Dim x As Int16
        RileggoUltimi()
        If DsReg.Tables(Ri).Rows.Count = 0 Then
            Messaggio(1, "REGISTRO CORRISPETTIVI INESISTENTE !!!")
            AbilitaGroup(0)
            Me.Close()
            Exit Sub
        End If
        ImageComboBoxEdit2.Properties.Items.Clear()
        ImageComboBoxEdit2.SelectedIndex = -1
        Xreg = 0
        For x = 1 To DsReg.Tables(Ri).Rows.Count
            RwReg = DsReg.Tables(Ri).Rows(x - 1)
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(RwReg("RivaNreg").ToString.PadLeft(2, "0") & " " & RwReg("RivaDesc"), RwReg("RivaNreg").ToString.PadLeft(2, "0"), -1)
            ImageComboBoxEdit2.Properties.Items.Add(nn)
            If ERifRiva > 0 And RwReg("RivaNreg") = ERifRiva Then
                Xreg = x - 1
            End If
        Next
        ImageComboBoxEdit2.SelectedIndex = Xreg
        If ERifRiva = 0 Then ImageComboBoxEdit2.Focus()
    End Sub
    Sub RileggoUltimi()
        ' LEGGO SOLO I RGISTRI IVA DI TIPO 5 (CORRISPETTIVI)
        Dim Str As String = "Select * from FnFotoRIva(" & Val(ComboBoxEdit1.EditValue) & ") Where RivaTipo = 5  ORDER BY RivaNReg"
        DsReg = New DataSet(Ri)
        DaReg = New SqlDataAdapter(Str, cnCo)
        DaReg.Fill(DsReg, Ri)
        Dim P As Int16
        '' AGGIUNGO I REG.IVA NON MOVIMENTATI
        Dim StrReg As String = "SELECT * from TbRegIva where RivaTipo = 5 and RivaAnno = " & Val(ComboBoxEdit1.EditValue)
        Dim Cmd As New SqlCommand(StrReg, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            For P = 1 To DsReg.Tables(Ri).Rows.Count
                RwReg = DsReg.Tables(Ri).Rows(P - 1)
                If RwReg("RivaNReg") = dataRd.Item("RivaNReg") Then
                    GoTo DopoLet
                End If
            Next
            RwReg = DsReg.Tables(Ri).NewRow
            RwReg("RivaNReg") = dataRd.Item("RivaNReg")
            RwReg("RivaTipo") = dataRd.Item("RivaTipo")
            RwReg("TipoDesc") = "Corrispettivi"
            RwReg("RivaDesc") = dataRd.Item("RivaDesc")
            RwReg("ProtCar") = 0
            RwReg("DataCar") = Today.ToShortDateString
            RwReg("ProtSta") = 0
            RwReg("DataSta") = Today.ToShortDateString
            RwReg("RivaPrintIniziale") = dataRd.Item("RivaPrintIniziale")
            RwReg("RivaCpt") = dataRd.Item("RivaCpt")
            RwReg("RivaArt") = dataRd.Item("RivaArt")
            DsReg.Tables(Ri).Rows.Add(RwReg)
            DsReg.Tables(Ri).AcceptChanges()
DopoLet:
        End While
        dataRd.Close()
    End Sub
    Private Sub ImageComboBoxEdit2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit2.SelectedIndexChanged
        Xreg = ImageComboBoxEdit2.SelectedIndex()
        If Sw = 0 Or Xreg = -1 Then Exit Sub
        CaricaIniziale(Xreg)
    End Sub
    Sub CaricaIniziale(ByVal i As Int16)
        RwReg = DsReg.Tables(Ri).Rows(i)
        OldRegis = RwReg("RivaNreg")
        IvaCpt = RwReg("RivaCpt")
        If ControllaRegistroIva(Val(OldRegis)) = False Then
            Messaggio(1, "REGISTRO IVA INESISTENTE !!!")
        Else
            DateEdit1.EditValue = RwReg("DataCar")
            If CDate(DateEdit1.EditValue).Day = 31 And CDate(DateEdit1.EditValue).Month = 12 Then
                DateEdit1.EditValue = CDate(DateEdit1.EditValue)
            Else
                DateEdit1.EditValue = CDate(DateEdit1.EditValue).AddDays(1)
            End If
            TextEdit1.EditValue = RwReg("ProtCar") + 1
            TextEdit1.Focus()
            VedoIncassi()
        End If
    End Sub
    Sub VedoIncassi()
        Dim Str As String = "SELECT * FROM VINCCOR where priregiva = " & Val(OldRegis) & " And datepart(year,PriDataGio) = " & CDate(DateEdit1.EditValue).Year & " order by PridataGio DESC,PriNumProt DESC"
        DsInc = New DataSet(Ic)
        DaInc = New SqlDataAdapter(Str, cnCo)
        DaInc.Fill(DsInc, Ic)
        GridControl2.DataSource = DsInc.Tables(Ic)
        GridControl2.Refresh()
        GridView2.ClearSelection()
    End Sub
    'Private Sub TextEdit1_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextEdit1.Validating
    '    If RwReg IsNot Nothing Then
    '        If Val(TextEdit1.EditValue) > RwReg("ProtCar") + 1 Then
    '            TextEdit1.EditValue = RwReg("ProtCar") + 1
    '            e.Cancel = True
    '        End If
    '    End If
    'End Sub
    Private Sub DateEdit1_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles DateEdit1.Validating
        If RwReg IsNot Nothing Then
            If DateEdit1.EditValue < RwReg("DataCar") And TextEdit1.EditValue = RwReg("ProtCar") + 1 Then
                e.Cancel = True
            End If
        End If
    End Sub
    Function ControllaRegistroIva(ByVal n As Int16) As Boolean
        ControllaRegistroIva = False
        NumReg = 0
        ProgId = 0
        TotaleFattura = 0
        Dim StrReg As String = "SELECT * from TbRegIva where RivaAnno = " & Val(ComboBoxEdit1.EditValue) & " and RIvaNreg = " & n
        Dim Cmd As New SqlCommand(StrReg, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ControllaRegistroIva = True
            NumReg = n
        End While
        dataRd.Close()
    End Function
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "INSERIMENTO CORRISPETTIVI"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        Dim x As Int16
        x = ComboBoxEdit1.SelectedIndex
        If x = -1 Or Sw = 0 Then Exit Sub
        PopolaRegime(x)
    End Sub
    Private Sub TextEdit1_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit1.Leave
        If Sw = 0 Then Exit Sub
        If ControllaProtocollo(Val(TextEdit1.EditValue)) = False Then
            DateEdit1.Focus()
        ElseIf TextEdit1.ErrorText > "" Then
            TextEdit1.Focus()
        Else
            TextEdit4.Focus()
        End If
    End Sub

    Function ControllaProtocollo(ByVal n As Int32) As Boolean
        ControllaProtocollo = False
        OkProt = False
        If n = 0 Then
            n = RwReg("ProtCar") + 1
            TextEdit1.EditValue = n
        End If
        Dim StrReg As String = "SELECT * FROM vh3 where PriRegIva = " & NumReg & " and PriNumProt = " & n & " and datepart(year,PriDataGio) = " & Val(ComboBoxEdit1.EditValue)
        DsFat = New DataSet(Fa)
        DaFat = New SqlDataAdapter(StrReg, cnCo)
        DaFat.SelectCommand.CommandTimeout = 300
        DaFat.Fill(DsFat, Fa)
        GridControl1.DataSource = DsFat.Tables(Fa)
        GridControl1.Refresh()
        GridView1.ClearSelection()
        If DsFat.Tables(Fa).Rows.Count > 0 Then
            ControllaProtocollo = True
            OkProt = True
            CaricaDati()
        Else
            Pulizia(1)
            UltimeCpt()
        End If
    End Function
    Sub CaricaDati()
        Dim k As Int16
        For k = 1 To DsFat.Tables(Fa).Rows.Count()
            RwFat = DsFat.Tables(Fa).Rows(k - 1)
            If k = 1 Then
                TotaleFattura = 0
                ProgId = RwFat("PriId")
                DateEdit1.EditValue = CDate(RwFat("PridataGio"))
                ImageComboBoxEdit1.SelectedIndex = SettaComboImage(ImageComboBoxEdit1, RwFat("PriCausale"))
                If Val(ImageComboBoxEdit1.EditValue) > 3 And Val(ImageComboBoxEdit1.EditValue) < 73 And Val(ImageComboBoxEdit1.EditValue) <> 45 Then
                    LeggiCodiciIva(Val(ImageComboBoxEdit1.EditValue))
                End If
                TextEdit4.EditValue = Trim(RwFat("PriDesc"))
                TextEdit2.EditValue = RwFat("CONTO")
                Pcod = TextEdit2.EditValue
                TextEdit3.EditValue = RwFat("DAREDESC")
                If RwFat("PriGstampa") = True Then
                    DateEdit1.ErrorText = "Corrispettivo Stampato sul Libro Giornale In Bollo!!!"
                    ButtonF3.Enabled = False
                Else
                    DateEdit1.ErrorText = ""
                    ButtonF3.Enabled = True
                End If
                If RwFat("PriIvaPrint") = True Then
                    TextEdit1.ErrorText = "Corrispettivo Stampato sul Registro I.v.a. In Bollo!!!"
                    ButtonF3.Enabled = False
                Else
                    TextEdit1.ErrorText = ""
                    ButtonF3.Enabled = True
                End If
            End If
            TotaleFattura = TotaleFattura + RwFat("PriImpDare")
        Next
    End Sub
    Function LeggiConto(ByRef CodCo As String, ByRef Anagraf As TextEdit) As Boolean
        Anagraf.Text = "*** ERRATO ***"
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
        CodCo = CodCo.PadLeft(5, "0")
        If Val(CodCo) < 1000 Then Exit Function
        Str = "SELECT * from TbAna where AnaCoD = '" & CodCo & "'"
        Cmd = New SqlCommand(Str, cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Anagraf.Text = dataRd("AnaRag1")
            LeggiConto = True
        End While
        dataRd.Close()
    End Function
    Function LeggiCodiciIva(ByVal i As Int16) As String
        RwCii = DsCii.Tables(Ci).Rows(i - 1)
        Return RwCii("CiiDes")
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

    Private Sub Textedit2_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit2.Validated
        If LeggiConto(TextEdit2.EditValue, TextEdit3) = False Then
            TextEdit2.EditValue = Pcod
            TextEdit2.Focus()
        End If
    End Sub
    Private Sub Textedit2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit2.Enter '''Numbox3.LostFocus
        Pcod = TextEdit2.EditValue
        LeggiCodiciIva(Val(ImageComboBoxEdit1.EditValue))
    End Sub
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        Dim Nc As String = ""
        If GroupControl5.Enabled = False Then
            Nc = Query.CercaPia()
            If Nc > "01.00" Then
                TextEdit2.EditValue = Nc
                LeggiConto(TextEdit2.EditValue, TextEdit3)
                SelectNextControl(TextEdit3, True, True, True, True)
            End If
            Exit Sub
        End If
        If GroupControl5.Enabled = True Then
            Nc = Query.CercaPia()
            If Nc > "01.00" Then
                TextEdit20.EditValue = Nc
                LeggiConto(TextEdit20.EditValue, TextEdit21)
                CalcolaRiga(2)
            End If
            Exit Sub
        End If
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Controlli() = False Then Exit Sub
        Parallelo()
        IniziaCorpo()
    End Sub
    Sub IniziaCorpo()
        AbilitaGroup(1)
        TotaleFattura = 0
        TotaleResiduo = 0
        TotaleReg = 0
        X1 = 0
        ' CARICA PRIMA RIGA
        CaricaDettagli(1)
    End Sub
    Sub CaricaDettagli(ByVal I As Int16)
        TextEdit15.EditValue = RwFcf("PriProg")
        TextEdit16.EditValue = RwFcf("PriImpDare")
        ImageComboBoxEdit3.EditValue = RwFcf("PriCodIva")
        TextEdit20.EditValue = RwFcf("PriCoAvere")
        If Val(ImageComboBoxEdit3.EditValue) > 0 Then LeggiCodiciIva(Val(ImageComboBoxEdit3.EditValue))
        LeggiConto(TextEdit20.EditValue, TextEdit21)
        TotaleIn()
        ImageComboBoxEdit1.EditValue = RwFcf("PriCausale")
        TextEdit2.EditValue = RwFcf("PriCoDare")
        TextEdit4.EditValue = RwFcf("PriDesc")
        If Val(ImageComboBoxEdit3.EditValue) > 3 And Val(ImageComboBoxEdit3.EditValue) < 73 And Val(ImageComboBoxEdit3.EditValue) <> 45 Then
            LeggiCodiciIva(Val(ImageComboBoxEdit3.EditValue))
        End If
        LeggiConto(TextEdit2.EditValue, TextEdit3)
        TextEdit16.Focus()
    End Sub
    Sub TotaleIn()
        If Trim(TextEdit16.EditValue.ToString) = "" Then TextEdit16.EditValue = CDec(0.0)
        TotTransito = TotaleReg + (CDec(TextEdit16.EditValue))
        TextEdit24.EditValue = Format(TotTransito, "###,###,###,#0.00")
    End Sub
    Private Function LeggiUltimo(ByVal dataGio As Date) As Boolean
        Dim ultimo As String = "INSERT INTO TbIDP (IDdata) values(@PriDataGio)"
        Dim Qmd As New SqlCommand(ultimo, cnCo)
        Dim px As New SqlParameter("@PriDataGio", SqlDbType.SmallDateTime)
        px.Value = CDate(dataGio)
        Qmd.Parameters.Add(px)
        Qmd.ExecuteNonQuery()
        Dim UltimaRiga As New SqlCommand("SELECT  @@IDENTITY ", cnCo)
        ProgId = UltimaRiga.ExecuteScalar
    End Function
    Sub ResetIdP()
        Dim Elimina As String = "DELETE FROM TbIDP WHERE IdPRI = " & ProgId
        Dim Dmd As New SqlCommand(Elimina, cnCo)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub Parallelo()
        If DsFat.Tables(Fa).Rows.Count > 0 Then
            RwFat = DsFat.Tables(Fa).Rows(0)
            ProgId = RwFat("PriId")
            OkFat = True
        Else
            RwFat = DsFat.Tables(Fa).NewRow
            RwFat("PriProg") = 1
            If ImageComboBoxEdit3.EditValue IsNot Nothing Then RwFat("PriCodIva") = ImageComboBoxEdit3.EditValue Else RwFat("PriCodIva") = 0
            RwFat("PriImpDare") = 0
            RwFat("PriImpavere") = 0
            RwFat("Cpt") = "00.00"
            RwFat("PriIvaPrint") = 0
            RwFat("PriGstampa") = 0
            RwFat("PriCodPag") = 0
            OkFat = False
        End If
        RwFat("PriCausale") = Val(ImageComboBoxEdit1.EditValue)
        RwFat("PriDesc") = Trim(TextEdit4.EditValue)
        RwFat("CONTO") = TextEdit2.EditValue
        RwFat("DAREDESC") = TextEdit3.EditValue
        IniziaRwFat()
        ProgId = RwFat("PriId")
        DsFcf = New DataSet(Ft)
        DaFcf = New SqlDataAdapter("SELECT * from TbPri where  PriId = " & ProgId, cnCo)
        DaFcf.SelectCommand.CommandTimeout = 300
        DaFcf.Fill(DsFcf, Ft)
        Righe = DsFcf.Tables(Ft).Rows.Count
        If Righe > 0 Then
            RwFcf = DsFcf.Tables(Ft).Rows(0)
            OkFcf = True
        Else
            RwFcf = DsFcf.Tables(Ft).NewRow
            OkFcf = False
        End If
        IniziaRwFcf()
    End Sub
    Sub IniziaRwFat()
        RwFat("PriId") = ProgId
        RwFat("PridataGio") = DateEdit1.EditValue
        RwFat("PridataEst") = DateEdit1.EditValue
        RwFat("PriDocEst") = Val(TextEdit1.EditValue)
        RwFat("PriCodPag") = 0
    End Sub
    Sub Ricontrolla()
        Dim x As Int16
        OkFcf = False
        For x = 1 To DsFcf.Tables(Ft).Rows.Count
            RwFRI = DsFcf.Tables(Ft).Rows(x - 1)
            If RwFRI("PriProg") = POSRIG Then
                OkFcf = True
                Exit Sub
            End If
        Next
    End Sub
    Sub IniziaRwFcf()
        If OkFcf = True Then Ricontrolla() '''' PURTROPPO IL MOVIMENTO DEL MOUSE GENERA CONFUSIONE
        RwFcf("PriDataGio") = CDate(DateEdit1.EditValue)
        RwFcf("PriCausale") = RwFat("PriCausale") 'Val(ImageComboBoxEdit1.editvalue)
        RwFcf("PriCoDare") = RwFat("CONTO") 'Textedit2.editvalue
        RwFcf("PriCoAvere") = RwFat("Cpt")
        RwFcf("PriNumProt") = Val(TextEdit1.EditValue)
        RwFcf("PriBisRet") = ""
        RwFcf("PriRegIva") = NumReg
        RwFcf("PriDesc") = RwFat("PriDesc") 'TextEdit4.Text
        RwFcf("PriDocEst") = Val(TextEdit1.EditValue)
        RwFcf("PriMeseSk") = ""
        RwFcf("PriDataEst") = CDate(DateEdit1.EditValue)
        RwFcf("PriDescB") = ""
        RwFcf("PriFl04") = 0
        RwFcf("PriFl05") = 0
        RwFcf("PriFl06") = 0
        RwFcf("PriNsRif") = ""
        RwFcf("PriSos") = ""
        RwFcf("PriLinea") = ""
        RwFcf("PriDocAnn") = CDate(DateEdit1.EditValue).Year
        RwFcf("PriCodPag") = 0
        RwFcf("PriValuta") = 0
        RwFcf("PriArtFisc") = 0
        RwFcf("PriId") = ProgId
        RwFcf("PriProg") = POSRIG
        RwFcf("PriIvaPrint") = RwFat("PriIvaPrint")
        RwFcf("PriGStampa") = RwFat("PriGStampa")
        ''' se esiste lo scorporo cambiare
        RwFcf("PriCodIva") = RwFat("PriCodIva")
        RwFcf("PriImpDare") = RwFat("PriImpDare")
        RwFcf("PriImpavere") = RwFat("PriImpDare")
    End Sub
    Function Controlli() As Boolean
        Controlli = True
        If CDate(DateEdit1.EditValue).Year <> Val(ComboBoxEdit1.EditValue) Then
            Messaggio(1, "ESERCIZIO IVA DIVERSO DALLA DATA INCASSO e GIORNALE  ")
            Controlli = False
            DateEdit1.Focus()
        End If
        If DateEdit1.ErrorText > "" Then
            Messaggio(0, DateEdit1.ErrorText.ToUpper)
            Controlli = False
            DateEdit1.Focus()
        End If
        If TextEdit1.ErrorText > "" Then
            Messaggio(0, TextEdit1.ErrorText.ToUpper)
            Controlli = False
            TextEdit1.Focus()
        End If
        If ControllaConto(TextEdit2.EditValue, TextEdit3) = False Then
            Controlli = False
            TextEdit2.Focus()
            Exit Function
        End If
        If Val(ImageComboBoxEdit1.EditValue) < 4 Or Val(ImageComboBoxEdit1.EditValue) > 72 Or Val(ImageComboBoxEdit1.EditValue) = 45 Then
            Controlli = False
            ImageComboBoxEdit1.Focus()
            Exit Function
        End If


    End Function
    Private Sub TextEdit16_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit16.Validated
        If TextEdit16.EditValue.ToString = "" Then TextEdit16.EditValue = CDec(0.0)
        CalcolaRiga(0)
    End Sub
    Private Sub TextEdit20_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit20.Enter  '''Numbox17.LostFocus
        If Val(ImageComboBoxEdit3.EditValue) > 0 And Val(ImageComboBoxEdit3.EditValue) < 73 Then
            CalcolaRiga(0)
        Else
            ImageComboBoxEdit3.Focus()
            Exit Sub
        End If
    End Sub
    Private Sub TextEdit40_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonXX.Enter '''TextEdit40.LostFocus
        If LeggiConto(TextEdit20.EditValue, TextEdit21) = False Then
            TextEdit20.Focus()
        End If
        CalcolaRiga(2)
    End Sub
    Sub CalcolaRiga(ByVal i As Int16)
        If i = 0 Then
            RwFat("PriImpDare") = CDec(TextEdit16.EditValue)
            RwFat("PriCodIva") = 0
            RwFat("CiiAli") = 0
            RwFat("CiiDes") = ""
            If Val(ImageComboBoxEdit3.EditValue) > 0 And Val(ImageComboBoxEdit3.EditValue) < 73 Then
                LeggiCodiciIva(Val(ImageComboBoxEdit3.EditValue))
                RwFat("CiiDes") = RwCii("CiiDes")
                RwFat("PriCodIva") = RwCii("CiiCod")
                RwFat("CiiAli") = RwCii("CiiAli")
            End If
        End If
        If i = 2 Then
            RwFat("Cpt") = TextEdit20.EditValue
            RwFat("AVEREDESC") = TextEdit21.EditValue
            ButtonXX.Focus()
        End If
        TotaleIn()
    End Sub
    Sub UltimeCpt()
        Dim Id As Int32 = 0
        Dim Cmd As New SqlCommand("SELECT ISNULL(MAX(PRIID),0) FROM TBPRI WHERE PRIREGIVA = " & Val(OldRegis), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Id = dataRd.Item(0)
        End While
        dataRd.Close()
        DsFat = New DataSet(Fa)
        DaFat = New SqlDataAdapter("SELECT * from VH3 where PriId = " & Id, cnCo)
        DaFat.SelectCommand.CommandTimeout = 300
        DaFat.Fill(DsFat, Fa)
        GridControl1.DataSource = DsFat.Tables(Fa)
        GridControl1.Refresh()
        GridView1.ClearSelection()
        If DsFat.Tables(Fa).Rows.Count > 0 Then
            PulisciDatiDati()
            CaricaDati()
        End If
    End Sub
    Sub PulisciDatiDati()
        Dim q As Int16
        For q = 1 To DsFat.Tables(Fa).Rows.Count
            RwFat = DsFat.Tables(Fa).Rows(q - 1)
            RwFat("Priid") = ProgId
            RwFat("PriProg") = q
            RwFat("PridataGio") = CDate(DateEdit1.EditValue)
            RwFat("PridataEst") = CDate(DateEdit1.EditValue)
            RwFat("PriImpDare") = 0
            RwFat("PriImpAvere") = 0
            RwFat("PriGstampa") = 0
            RwFat("PriIvaPrint") = 0
            RwFat("PriDesc") = ""
        Next
    End Sub
    Private Sub ButtonXX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXX.Click
        If ControllaConto(TextEdit2.EditValue, TextEdit3) = False Then
            TextEdit2.Focus()
            Exit Sub
        End If
        If Val(ImageComboBoxEdit1.EditValue) < 4 Or Val(ImageComboBoxEdit1.EditValue) > 72 Or Val(ImageComboBoxEdit1.EditValue) = 45 Then
            ImageComboBoxEdit1.Focus()
            Exit Sub
        End If
        If ControllaConto(TextEdit20.EditValue, TextEdit21) = False Then
            TextEdit20.Focus()
            Exit Sub
        End If
        If Val(ImageComboBoxEdit3.EditValue) < 1 Or Val(ImageComboBoxEdit3.EditValue) > 72 Then
            ImageComboBoxEdit3.Focus()
            Exit Sub
        End If
        RicalcolaDocumento()
    End Sub
    Function ControllaConto(ByRef CodCo As String, ByRef Anagraf As TextEdit) As Boolean
        ControllaConto = False
        If Mid(CodCo, 3, 3) = ".00" Or Mid(CodCo, 1, 3) = "00." Then Exit Function '' mastri e transitorio
        ControllaConto = LeggiConto(CodCo, Anagraf)
    End Function
    Sub RiassegnaValori()
        RwFat("PriCausale") = Val(ImageComboBoxEdit1.EditValue)
        RwFat("CONTO") = TextEdit2.EditValue
        RwFat("DAREDESC") = TextEdit3.EditValue
        RwFat("PriDesc") = Trim(TextEdit4.EditValue)
        RwFat("AVEREDESC") = TextEdit21.EditValue
        RwFat("CPT") = TextEdit20.EditValue
    End Sub
    Sub RicalcolaDocumento()
        RiassegnaValori() ' da sotto a sopra e sul datarow
        If OkFat = False Then DsFat.Tables(Fa).Rows.Add(RwFat)
        POSRIG = RwFat("PriProg")
        IniziaRwFcf()
        If OkFcf = False Then DsFcf.Tables(Ft).Rows.Add(RwFcf)
        RicalcoloFinale(POSRIG + 1)
        POSRIG = POSRIG + 1
        RigeneraFat()
        RigeneraFcF()
        CaricaDettagli(POSRIG)
    End Sub
    Sub RicalcoloFinale(ByVal i As Int16)
        Dim x As Int16
        X1 = 0
        For x = 1 To i - 1
            RwFRI = DsFcf.Tables(Ft).Rows(x - 1)
            X1 = X1 + RwFRI("PriImpDare")
        Next
        TotaleReg = X1
    End Sub
    Sub RigeneraFat()
        Dim Scorp As Boolean = True
        If OkFat = False Then GoTo Aggiungi
        Dim x As Int16
        For x = 1 To DsFat.Tables(Fa).Rows.Count
            RwFat = DsFat.Tables(Fa).Rows(x - 1)
            If RwFat("PriProg") = POSRIG Then
                IniziaRwFat()
                Exit Sub
            End If
        Next
        OkFat = False
Aggiungi:
        RwFat = DsFat.Tables(Fa).NewRow
        IniziaRwFat()
        RwFat("PriProg") = POSRIG
        RwFat("PriCodIva") = ImageComboBoxEdit3.EditValue
        RwFat("PriImpDare") = 0
        RwFat("PriImpavere") = 0
        RwFat("PriIvaPrint") = 0
        RwFat("PriGStampa") = 0
        RiassegnaValori()
        Exit Sub
    End Sub
    Sub RigeneraFcF()
        If OkFcf = False Then GoTo Aggiungi
        Dim x As Int16
        For x = 1 To DsFcf.Tables(Ft).Rows.Count
            RwFcf = DsFcf.Tables(Ft).Rows(x - 1)
            If RwFcf("PriProg") = POSRIG Then
                IniziaRwFcf()
                Exit Sub
            End If
        Next
Aggiungi:
        RwFcf = DsFcf.Tables(Ft).NewRow
        IniziaRwFcf()
    End Sub

    Sub RegistraFattura()
        If TotTransito <> 0 Then
            ScriviPri()
            ButtonF5.PerformClick()
        End If
    End Sub
    Sub EliminaProt()
        Dim Cancella As String = "BEGIN Delete from TbPri where PriId = " & ProgId & " Delete from TbPrk where PrKId = " & ProgId & " END"
        Dim Dmd As New SqlCommand(Cancella, cnCo)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub ScriviPri()
        Dim Scrivi As String = "INSERT INTO TbPri (PriId,PriProg,PriDataGio, PriCausale, PriCoDare, PriCoAvere, PriNumProt, PriBisRet, PriCodIva, PriRegIva, PriImpDare, PriImpavere, PriDesc, PriDocEst, PriMeseSk, PriDataEst, PriDescB, PriFl04, PriFl05, PriFl06, PriNsRif, PriSos, PriLinea, PriDocAnn, PriCodPag, PriValuta, PriArtFisc,PriIvaPrint,PriGStampa) " _
& " values(@PriId,@PriProg,@PriDataGio, @PriCausale, @PriCoDare, @PriCoAvere, @PriNumProt, @PriBisRet, @PriCodIva, @PriRegIva, @PriImpDare, @PriImpavere, @PriDesc, @PriDocEst, @PriMeseSk, @PriDataEst, @PriDescB, @PriFl04, @PriFl05, @PriFl06, @PriNsRif, @PriSos, @PriLinea, @PriDocAnn, @PriCodPag, @PriValuta, @PriArtFisc,@PriIvaPrint,@PriGStampa)"
        If OkProt = False Then LeggiUltimo(DateEdit1.EditValue)
        EliminaProt()
        Dim Cmd As New SqlCommand(Scrivi, cnCo)
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

        Dim x, M As Int16
        M = 0
        For x = 1 To DsFcf.Tables(Ft).Rows.Count
            Cmd.Parameters.Clear()
            RwFRI = DsFcf.Tables(Ft).Rows(x - 1)
            If RwFRI("PriImpDare") = 0 Then
                GoTo VaiOltre
            End If
            M = M + 1
            p1.Value = RwFRI("PriDataGio")
            p2.Value = RwFRI("PriCausale")
            p3.Value = RwFRI("PriCoDare")
            p4.Value = RwFRI("PriCoAvere")
            p5.Value = RwFRI("PriNumProt")
            p6.Value = RwFRI("PriBisRet")
            p7.Value = RwFRI("PriCodIva")
            p8.Value = RwFRI("PriRegIva")
            p9.Value = RwFRI("PriImpDare")
            p10.Value = RwFRI("PriImpavere")
            p11.Value = RwFRI("PriDesc")
            p12.Value = RwFRI("PriDocEst")
            p13.Value = RwFRI("PriMeseSk")
            p14.Value = RwFRI("PriDataEst")
            p15.Value = RwFRI("PriDescB")
            p16.Value = RwFRI("PriFl04")
            p17.Value = RwFRI("PriFl05")
            p18.Value = RwFRI("PriFl06")
            p19.Value = RwFRI("PriNsRif")
            p20.Value = RwFRI("PriSos")
            p21.Value = RwFRI("PriLinea")
            p22.Value = RwFRI("PriDocAnn")
            p23.Value = 0
            p24.Value = RwFRI("PriValuta")
            p25.Value = RwFRI("PriArtFisc")
            p26.Value = ProgId
            p27.Value = M
            p28.Value = RwFRI("PriIvaPrint")
            p29.Value = RwFRI("PriGStampa")
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
            Cmd.ExecuteNonQuery()
VaiOltre:
        Next
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        RileggoUltimi()
        If OKCDC = True Then LancioCDC()
    End Sub
    Sub LancioCDC()
        '''Dim Gesterna As New RipCdc
        '''RipCdc.PIIDD = ProgId
        '''RipCdc.PNdoc = RwFRI("PriDocEst").ToString
        '''RipCdc.PNreg = RwFRI("PriRegIva").ToString
        '''RipCdc.PProt = RwFRI("PriNumProt").ToString
        '''RipCdc.PDatDoc = CDate(RwFRI("PriDataEst")).ToShortDateString
        '''Gesterna.ShowDialog()
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
            RwX = GridView1.GetDataRow(iset)
            DaMouse()
        End If
    End Sub
    Sub DaMouse()

        If DsFcf Is Nothing Then Exit Sub
        Dim x As Int16
        X0 = 0
        For x = 1 To DsFcf.Tables(Ft).Rows.Count
            RwFcf = DsFcf.Tables(Ft).Rows(x - 1)
            RwFat = DsFat.Tables(Fa).Rows(x - 1)
            If RwFcf("PriCodIva") = 0 Then
                X0 = X0 + RwFcf("PriImpDare")
            Else
                X0 = 0
            End If
            If RwX("PriProg") = RwFcf("PriProg") Then
                OkFat = True
                OkFcf = True
                RicalcoloFinale(RwFat("PriProg"))
                CaricaDettagli(RwFat("PriProg"))
                GridView1.UnselectRow(iset)
                Exit Sub
            End If
        Next
    End Sub
    Private Sub GridControl2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl2.MouseMove
        ShowHitInfo2(GridView2.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo2(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub
    Private Sub GridView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.Click
        If iset > -1 Then
            RwX = GridView2.GetDataRow(iset)
            IcMouse()
        End If
    End Sub
    Sub IcMouse()
        TextEdit1.EditValue = RwX("PriNumProt")
        If ControllaProtocollo(Val(TextEdit1.EditValue)) = False Then
            ImageComboBoxEdit1.Focus()
        Else
            TextEdit4.Focus()
        End If
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F1 Then
            e.Handled = True
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 Then
            e.Handled = True
            ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            ButtonF8.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub InFtCo_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        e.Cancel = GroupControl5.Enabled
    End Sub
    Sub EliminaMcc()
        '''Dim Cancella As String = "Delete from TbMcc where MCCPRKID = " & ProgId
        '''Dim Dmd As New SqlCommand(Cancella, CnDc)
        '''Dmd.ExecuteNonQuery()
    End Sub
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If OkProt = False Then Exit Sub
        Messaggio(2, "ELIMINO IL PROT N. " & Val(TextEdit1.EditValue) & " DEL " & DateEdit1.EditValue & " ? ")
        If Rispondi = MsgBoxResult.Yes Then
            EliminaProt()
            If OKCDC = True Then EliminaMcc()
            RileggoUltimi()
            ButtonF5.PerformClick()
        End If
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        RegistraFattura()
    End Sub

    Private Sub TextEdit4_Leave(sender As Object, e As System.EventArgs) Handles TextEdit4.Leave
        If ButtonF1.Enabled = False Then TextEdit16.Focus() Else ButtonF1.Focus()
    End Sub
End Class