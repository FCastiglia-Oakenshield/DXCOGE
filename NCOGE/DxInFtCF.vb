Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports NCDCO
Imports DevExpress.XtraEditors
Imports DXFTELE
Imports System.IO
Imports System.Xml

Public Class DxInFtCF
    Private Shared ERifProt, ERifAnno, ERifRiva As Integer
    Private Shared ERifBis As String
    Public Shared Property NRifBis() As String
        Get
            Return ERifBis
        End Get
        Set(ByVal Value As String)
            ERifBis = Value
        End Set
    End Property
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
    Dim Scrivi As String = "INSERT INTO TbPri (PriId,PriProg,PriDataGio, PriCausale, PriCoDare, PriCoAvere, PriNumProt, PriBisRet, PriCodIva, PriRegIva, PriImpDare, PriImpavere, PriDesc, PriDocEst, PriMeseSk, PriDataEst, PriDescB, PriFl04, PriFl05, PriFl06, PriNsRif, PriSos, PriLinea, PriDocAnn, PriCodPag, PriValuta, PriArtFisc,PriIvaPrint,PriGStampa) " _
  & " values(@PriId,@PriProg,@PriDataGio, @PriCausale, @PriCoDare, @PriCoAvere, @PriNumProt, @PriBisRet, @PriCodIva, @PriRegIva, @PriImpDare, @PriImpavere, @PriDesc, @PriDocEst, @PriMeseSk, @PriDataEst, @PriDescB, @PriFl04, @PriFl05, @PriFl06, @PriNsRif, @PriSos, @PriLinea, @PriDocAnn, @PriCodPag, @PriValuta, @PriArtFisc,@PriIvaPrint,@PriGStampa)"
    Dim Wrd As New SqlCommand(Scrivi, cnCo)
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

    Dim Sw As Int16 = 0
    Dim Xreg As Int16 = -1
    Dim NumReg, Causale, Righe, Fl04, Fl05, Fl06, POSRIG, R, Irow, DS As Int16
    Dim GR(2), TC(2), Pcod, IvaCpt As String
    Dim TotaleFattura, X0, X1, TotaleReg, TotTransito, TotaleResiduo As Decimal
    Dim OkProt, OkFat, OkFcf, Fl08, Fl088, SOLOCPT As Boolean
    Dim ProgId, DCGNUMRIF As Int32
    Dim Rispondi As MsgBoxResult
    Dim RivalsaoCee As Boolean = False

    Dim OLDCONTO As String
    Dim OLDDOCUM, MiglioFo, ArtGcRCee As Int32
    Dim OLDPANNO, OLDPAGAM As Int16
    Dim OLDTOTAL As Decimal
    Dim OLDRIT, OLDCSP As Int16

    Dim Fa As String = "FATT"
    Dim DsFat As DataSet
    Dim DaFat As SqlDataAdapter
    Dim RwFat As DataRow
    Dim RwFRI As DataRow
    Dim RwX As DataRow

    Dim Ci As String = "CIVA"
    Dim DsCii As DataSet
    Dim DaCii As SqlDataAdapter
    Dim RwCii As DataRow

    Dim Ft As String = "FACF"
    Dim DsFcf As DataSet
    Dim DaFcf As SqlDataAdapter
    Dim RwFcf As DataRow
    ' Dim CbFcf As SqlCommandBuilder

    Dim Ri As String = "REGI"
    Dim DsReg As DataSet
    Dim DaReg As SqlDataAdapter
    Dim RwReg As DataRow
    Dim RwAGG As DataRow

    Dim OkMondo As Boolean = False
    Dim OkOttica As Boolean = False
    Dim OkLDP As Boolean = False
    Dim UserId As String = ""
    Dim DataInizio As String = "31/08/2008"
    Dim DataMondoCli As String = "31/12/2010"

    Dim NumOldReg As String = ""
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim Em As New DevExpress.XtraEditors.Controls.CheckedListBoxItem

    Dim LastTbUFa As New DataTable
    Dim LastDaUFa As New SqlDataAdapter
    Dim errorT(1) As String
    Dim RegVend As New ArrayList

    Dim WithEvents PA As XVisPassive
    Dim MonitorOk As Boolean = False
    Dim RIFERFTEP As Integer = 0
    Dim DaFte As SqlDataAdapter
    Dim TbFte As New DataTable
    Dim FRMFO As New InsFoFte

    Private Sub DxInFtCF_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If Sw = 0 Then
            Apertura() : PopolaCii() : CheckEdit2.Checked = False : MonitorOk = MonitorIdoneo()
            Sw = 1
        End If
        Pulizia(0)
        If ERifProt > 0 Then
            TextEdit1.EditValue = ERifProt
            TextEdit2.EditValue = ERifBis
            If ControllaProtocollo(ERifProt) = True Then
                errorT(0) = TextEdit1.ErrorText
                errorT(1) = DateEdit1.ErrorText
                SelectNextControl(TextEdit4, True, True, True, True)
                TextEdit1.ErrorText = errorT(0)
                DateEdit1.ErrorText = errorT(1)
                CheckEdit2.Enabled = False
            End If
            ERifProt = 0 : ERifBis = "" : ERifRiva = 0 : ERifAnno = 0
        End If
        Visualizzazione_dettagli(CheckEdit2.Checked)
    End Sub
    Sub Pulizia(ByVal p As Int16)
        TextEdit3.EditValue = 0
        TextEdit13.EditValue = CDec(0.0)
        TextEdit12.EditValue = 0
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = 0
        TextEdit6.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = ""
        TextEdit9.EditValue = ""
        TextEdit10.EditValue = ""
        TextEdit11.EditValue = ""
        TextEdit25.EditValue = ""
        DateEdit1.ErrorText = ""
        TextEdit1.ErrorText = ""
        TextEdit7.ErrorText = ""
        TextEdit9.ErrorText = ""
        TextEdit12.ErrorText = ""
        Pcod = ""
        OkProt = False
        AbilitaGroup(0)
        OLDPAGAM = 0 : OLDCONTO = "" : OLDTOTAL = 0 : OLDPANNO = 0 : OLDDOCUM = 0 : OLDCSP = 0 : OLDRIT = 0
        Fl04 = 0
        Fl05 = 0
        Fl06 = 0
        Fl08 = False
        Fl088 = False
        Righe = 0
        POSRIG = 1
        ProgId = 0
        CheckButton2.Visible = False
        CheckEdit1.Checked = False
        ButtonEXP.Visible = False
        ImageComboBoxEdit3.SelectedIndex = -1
        ImageComboBoxEdit4.SelectedIndex = 0
        If p = 0 Then
            TextEdit2.EditValue = ""
            PulisciGrid()
            ResetNumBox()
            If Xreg > -1 Then
                CaricaIniziale(Xreg)
            End If
            TextEdit1.Focus()
        End If
    End Sub
    Sub PulisciGrid()
        DsFat = New DataSet(Fa)
        GridControl1.DataSource = DsFat.Tables(Fa)
        GridControl1.Refresh()
    End Sub
    Sub GestioneUser()
        REM MONDOMARINE
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UserId = dataRd.Item("Sel14")
        End While
        dataRd.Close()
        OkMondo = False
        OkOttica = False
        OkLDP = False
        If UserId.ToUpper = "MONDOMARINE" Or UserId.ToUpper = "PASTAECO" Or UserId.ToUpper = "PASTANEW" Or UserId.ToUpper = "PASTAGROUP" Then
            Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 600", cnVd)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                If dataRd.Item("Sel14") Is DBNull.Value Then DataInizio = "31/08/2008" Else DataInizio = CDate(dataRd.Item("Sel14")).ToShortDateString
            End While
            dataRd.Close()
            If UserId.ToUpper = "PASTAECO" Or UserId.ToUpper = "PASTANEW" Or UserId.ToUpper = "PASTAGROUP" Then
                OkOttica = True : OkLDP = True
                ButtonEXP.ImageIndex = 5
                CheckedComboBoxEdit1.Properties.Items.Clear()
                Cmd = New SqlCommand("SELECT LdpRif,LdpSigla FROM TbLdp order by LdpSigla", CnDc)
                dataRd = Cmd.ExecuteReader
                While dataRd.Read
                    Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(dataRd.Item("LdpRif"), dataRd.Item("LdpSigla").ToString, CheckState.Unchecked)
                    CheckedComboBoxEdit1.Properties.Items.Add(Em)
                End While
                dataRd.Close()
                GroupControl18.Text = "SIGLA LDP"
                GroupControl18.Visible = True
                Exit Sub
            End If
            OkMondo = True
            REM SOLO CANTIERI NAVALI
            CheckedComboBoxEdit1.Properties.Items.Clear()
            Cmd = New SqlCommand("SELECT TcmRif,TcmSigla FROM TbTcm Where TcmCdc = 1 order by TcmSigla", cnDb)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(dataRd.Item("TcmRif"), dataRd.Item("TcmSigla").ToString, CheckState.Unchecked)
                CheckedComboBoxEdit1.Properties.Items.Add(Em)
            End While
            dataRd.Close()
            RegVend.Clear()
            Dim SS As String = ""
            Cmd = New SqlCommand("SELECT Sel1 from Tbsel where selid = 3", cnVd)
            SS = Cmd.ExecuteScalar
            If SS IsNot Nothing Then
                For x As Int16 = 1 To Len(SS) Step 2
                    RegVend.Add(CInt(Mid(SS, x, 2)))
                Next
            End If
        End If
        GroupControl18.Visible = OkMondo
    End Sub
    Sub PopolaBanche()
        ImageComboBoxEdit3.Properties.Items.Clear()
        nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 0, -1)
        ImageComboBoxEdit3.Properties.Items.Add(nn)
        Dim cmd As New SqlCommand(" SELECT * from TbBan Order by BanCod", cnCo)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("BanDes"), dataRd.Item("Bancod"), -1)
            ImageComboBoxEdit3.Properties.Items.Add(nn)
        End While
        dataRd.Close()
        ImageComboBoxEdit3.SelectedIndex = -1
    End Sub
    Sub PopolaCii()
        ImageComboBoxEdit4.Properties.Items.Clear()
        Dim Str As String = "SELECT CiiCod,CiiDes from TbCii order by CiiCod"
        Dim SS As String = "0 "
        nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, 0, -1)
        ImageComboBoxEdit4.Properties.Items.Add(nn)
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiDes")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
            ImageComboBoxEdit4.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Sub Apertura()
        GR(1) = "CL"
        GR(2) = "FO"
        TC(1) = " and PriCoDare = "
        TC(2) = " and PriCoAvere = "
        Causale = 0
        SOLOCPT = False
        Dim Cmd As New SqlCommand("SELECT distinct RivaAnno from TbRegIva Order by RivaAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        Dim x As Int16 = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("RIvaAnno"))
        End While
        dataRd.Close()
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Exit Sub
        Cmd = New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        ComboBoxEdit1.SelectedIndex = -1
        REM MONDOMARINE
        GestioneUser()
        PopolaBanche()
        DsCii = New DataSet(Ci)
        DaCii = New SqlDataAdapter("SELECT * from TbCii order by CiiCod", cnCo)
        DaCii.Fill(DsCii, Ci)
        DateEdit1.EditValue = Today.Date
        DateEdit2.EditValue = DateEdit1.EditValue
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
            Messaggio(1, "REGISTRI IVA INESISTENTI !!!")
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
    Function ControllaProtocollo(ByVal n As Int32) As Boolean
        ControllaProtocollo = False
        OkProt = False
        If n = 0 Then
            n = RwReg("ProtCar") + 1
            TextEdit1.EditValue = n
        End If
        If Verifica(n) = False Then
            Messaggio(1, "IMPOSSIBILE CARICARE IL PROT N. " & n & TextEdit2.EditValue & Chr(13) & "INSERIRE PRIMA IL PROT. N. " & n & Chr(13) & "CORREZIONE AUTOMATICA ")
            TextEdit2.EditValue = ""
        End If
        Dim StrReg As String = "SELECT * FROM vh1h2 where PriRegIva = " & NumReg & " and PriNumProt = " & n & " and PriBisRet = '" & Trim(TextEdit2.EditValue) & "' and datepart(year,PriDataGio) = " & Val(ComboBoxEdit1.EditValue)
        DsFat = New DataSet(Fa)
        DaFat = New SqlDataAdapter(StrReg, cnCo)
        DaFat.SelectCommand.CommandTimeout = 300
        DaFat.Fill(DsFat, Fa)
        GridControl1.DataSource = DsFat.Tables(Fa)
        GridControl1.Refresh()
        GridView1.ClearSelection()
        If DsFat.Tables(Fa).Rows.Count > 0 Then
            ControllaProtocollo = True
            CaricaDati() : RIFERFTEP = 0
            If OkMondo = True Or OkLDP = True Then ButtonEXP.Visible = True Else ButtonEXP.Visible = False
        Else
            Pulizia(1) : AzzeraCheck()
        End If
        If Causale = 1 And Val(TextEdit3.EditValue) = 0 Then TextEdit3.EditValue = n
    End Function
    Sub AbilitaGroup(ByVal n As Int16)
        If n = 0 Then
            TextEdit1.Properties.ReadOnly = False
            TextEdit3.Properties.ReadOnly = False
            TextEdit13.Properties.ReadOnly = False
            TextEdit12.Properties.ReadOnly = False
            TextEdit2.Properties.ReadOnly = False
            TextEdit4.Properties.ReadOnly = False
            TextEdit5.Properties.ReadOnly = False
            '''   textedit27.ReadOnly = False
            '''     LinkLabel3.Enabled = True
            DateEdit2.Enabled = True
            ButtonF1.Enabled = True
            ButtonF3.Enabled = True
            ButtonF5.Enabled = True
            ButtonF8.Enabled = True
            GroupControl1.Enabled = True
            GroupControl7.Enabled = False
            '''    GroupBox8.Enabled = False
            GroupControl17.Visible = False
            '''Numbox22.Visible = False
            ImageComboBoxEdit3.Enabled = True
        Else
            TextEdit1.Properties.ReadOnly = True
            TextEdit3.Properties.ReadOnly = True
            TextEdit13.Properties.ReadOnly = True
            TextEdit12.Properties.ReadOnly = True
            TextEdit2.Properties.ReadOnly = True
            TextEdit4.Properties.ReadOnly = True
            TextEdit5.Properties.ReadOnly = True
            ''' textedit27.ReadOnly = True
            '''   LinkLabel3.Enabled = False
            DateEdit2.Enabled = False
            ButtonF1.Enabled = False
            ButtonF3.Enabled = False
            ButtonF5.Enabled = False
            ButtonF8.Enabled = False
            GroupControl1.Enabled = False
            GroupControl7.Enabled = True
            '''     GroupBox8.Enabled = True
            ImageComboBoxEdit3.Enabled = False
        End If

        If RwReg("RivaAutoFcee") > 0 And TextEdit11.EditValue.ToString.Length > 0 Then
            GroupControl17.Visible = True
            '''Numbox22.Visible = True
        End If
    End Sub
    Sub ResetNumBox()
        TextEdit14.EditValue = CDec(0.0)
        TextEdit15.EditValue = 0
        TextEdit16.EditValue = CDec(0.0)
        ImageComboBoxEdit4.SelectedIndex = 0
        ''''''TextEdit17.EditValue = 0
        ''''''TextEdit18.EditValue = ""
        TextEdit19.EditValue = CDec(0.0)
        TextEdit22.EditValue = CDec(0.0)
        TextEdit20.EditValue = "00.00"
        TextEdit21.EditValue = ""
        X1 = CDec(0.0)
        X0 = CDec(0.0)
        TotaleFattura = CDec(0.0)
        TotaleReg = CDec(0.0)
        TotaleResiduo = CDec(0.0)
        TotTransito = CDec(0.0)
    End Sub
    Sub CaricaIniziale(ByVal i As Int16)
        CheckEdit2.Enabled = MonitorOk
        RwReg = DsReg.Tables(Ri).Rows(i)
        NumOldReg = RwReg("RivaNreg")
        IvaCpt = RwReg("RivaCpt")
        If ControllaRegistroIva(Val(NumOldReg)) = False Then
            Messaggio(1, "REGISTRO IVA INESISTENTE !!!")
        Else
            DateEdit1.EditValue = RwReg("DataCar")
            TextEdit1.EditValue = RwReg("ProtCar") + 1
            TextEdit1.Focus()
            If RwReg("RivaTipo") = 1 Or RwReg("RivaTipo") = 3 Then
                Causale = 1
                GroupControl12.Text = "CLIENTE"
                TextEdit2.ToolTip = "B = BIS, R= RETTIFICA, S = IN SOSPESO"
                CheckButton2.ToolTip = "CLIENTE NON ATTIVO NON  UTILIZZARE !!!!!!"
                CheckEdit2.Checked = False
                CheckEdit2.Enabled = False
            Else
                Causale = 2
                GroupControl12.Text = "FORNITORE"
                TextEdit2.ToolTip = "B = BIS, R= RETTIFICA"
                CheckButton2.ToolTip = "FORNITORE NON ATTIVO NON  UTILIZZARE !!!!!!"
                If ComboBoxEdit1.EditValue > 2018 And MonitorOk = True And RwReg("RivaFteP") = True Then CheckEdit2.Enabled = True : CheckEdit2.Checked = True Else CheckEdit2.Enabled = False
            End If
        End If
    End Sub
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "INSERIMENTO FATTURE"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Private Sub ControlloCfPi(ByVal Cf As String, ByVal Pi As String)
        If Codfisc(Cf) = False Then
            TextEdit7.ErrorText = " ! "
        Else
            TextEdit7.ErrorText = ""
        End If
        If Codfisc(Pi) = False Then
            TextEdit9.ErrorText = " ! "
        Else
            TextEdit9.ErrorText = ""
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
        Cmd = New SqlCommand("SELECT EseGContoRCee FROM TbEse Where EseAnno = " & Val(ComboBoxEdit1.EditValue), cnCo)
        Try
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                ArtGcRCee = dataRd.Item("EseGContoRCee")
            End While
        Catch ex As Exception
            ArtGcRCee = 0
        End Try
        dataRd.Close()
    End Function
    Sub RileggoUltimi()
        ' LEGGO SOLO I RGISTRI IVA DI TIPO 1, 2, 3, 4 (Vendite,Acquisti,Rett.Vendite,Rett.Acquisti)
        Dim Str As String = "Select * from FnFotoRIva(" & Val(ComboBoxEdit1.EditValue) & ") Where RivaTipo < 5  ORDER BY RivaNReg"
        DsReg = New DataSet(Ri)
        DaReg = New SqlDataAdapter(Str, cnCo)
        DaReg.Fill(DsReg, Ri)
        '' AGGIUNGO I REG.IVA NON MOVIMENTATI
        Dim P As Int16
        Dim StrReg As String = "SELECT * from TbRegIva where RivaTipo < 5 and RivaAnno = " & Val(ComboBoxEdit1.EditValue) & " ORDER BY RivaNReg"
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
            If dataRd.Item("RivaTipo") = 1 Then
                RwReg("TipoDesc") = "Vendite"
            ElseIf dataRd.Item("RivaTipo") = 2 Then
                RwReg("TipoDesc") = "Acquisti"
            ElseIf dataRd.Item("RivaTipo") = 3 Then
                RwReg("TipoDesc") = "Rett.Vendite"
            ElseIf dataRd.Item("RivaTipo") = 4 Then
                RwReg("TipoDesc") = "Rett.Acquisti"
            Else : RwReg("TipoDesc") = ""
            End If
            RwReg("RivaDesc") = dataRd.Item("RivaDesc")
            RwReg("ProtCar") = 0
            RwReg("DataCar") = Today.ToShortDateString
            RwReg("ProtSta") = 0
            RwReg("DataSta") = Today.ToShortDateString
            RwReg("RivaPrintIniziale") = dataRd.Item("RivaPrintIniziale")
            RwReg("RivaCpt") = dataRd.Item("RivaCpt")
            RwReg("RivaAutoFCee") = dataRd.Item("RivaAutoFCee")
            RwReg("RivaCptCee") = dataRd.Item("RivaCptCee")
            RwReg("RivaCliCee") = dataRd.Item("RivaCliCee")
            If dataRd.Item("RIvaRCharge") Is DBNull.Value Then RwReg("RivaRcharge") = False Else RwReg("RivaRcharge") = CBool(dataRd.Item("RIvaRCharge"))
            RwReg("RivaFteP") = dataRd.Item("RivaFteP")
            DsReg.Tables(Ri).Rows.Add(RwReg)
            DsReg.Tables(Ri).AcceptChanges()
DopoLet:
        End While
        dataRd.Close()
    End Sub
    Function Verifica(ByVal n As Int16) As Boolean
        Verifica = False
        If TextEdit2.EditValue <> "B" Then
            Verifica = True
            Exit Function
        End If
        Dim Cmd As New SqlCommand("SELECT top 1 * FROM TbPri where PriRegIva = " & NumReg & " and PriNumProt = " & n & " and datepart(year,PriDataGio) = " & Val(ComboBoxEdit1.EditValue), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Verifica = True
        End While
        dataRd.Close()
    End Function
    Sub CaricaDati()
        Dim k As Int16
        For k = 1 To DsFat.Tables(Fa).Rows.Count()
            RwFat = DsFat.Tables(Fa).Rows(k - 1)
            If k = 1 Then
                TotaleFattura = 0
                ProgId = RwFat("PriId")
                DateEdit1.EditValue = RwFat("PridataGio")
                DateEdit2.EditValue = RwFat("PridataEst")
                TextEdit3.EditValue = RwFat("PriDocEst")
                TextEdit4.EditValue = Trim(RwFat("PriDesc"))
                LeggoPagamento()
                RwFat("PriCodPag") = OLDPAGAM
                TextEdit12.EditValue = RwFat("PriCodPag") '''' PAGAMENTO ERRATO DEVE LEGGERLO DALLA CAUSALE 3 
                ImageComboBoxEdit3.SelectedIndex = SettaComboImage(ImageComboBoxEdit3, Val(RwFat("PriLinea")))
                TextEdit5.EditValue = RwFat("CONTO")
                Pcod = TextEdit5.Text
                TextEdit6.EditValue = RwFat("DAREDESC")
                OkProt = True
                OLDCONTO = RwFat("CONTO")
                OLDDOCUM = RwFat("PriDocEst")
                OLDPANNO = RwFat("PriDocAnn")
                If RwFat("PriGstampa") = True Then
                    DateEdit1.ErrorText = "Fattura Stampata sul Libro Giornale In Bollo!!!"
                    ButtonF3.Enabled = False
                Else
                    DateEdit1.ErrorText = ""
                    ButtonF3.Enabled = True
                End If
                If RwFat("PriIvaPrint") = True Then
                    TextEdit1.ErrorText = "Fattura Stampata sul Registro I.v.a. In Bollo!!!"
                    ButtonF3.Enabled = False
                    BolloIva()
                Else
                    TextEdit1.ErrorText = ""
                    ButtonF3.Enabled = True
                    BolloIva()
                End If
                If RwFat("PriMeseSk") = "*" Then CheckEdit1.Checked = True
            End If
            TotaleFattura = TotaleFattura + RwFat("PriImpDare") + RwFat("PriImpAvere")
            LeggoFlagCpt()
        Next
        TextEdit13.EditValue = TotaleFattura
        OLDTOTAL = TotaleFattura
        LeggoAnagrafica(TextEdit5.EditValue)
        If OkMondo = True Or OkLDP = True Then CaricaCheck()
    End Sub
    Sub AzzeraCheck()
        For I As Int16 = 1 To CheckedComboBoxEdit1.Properties.Items.Count
            Em = CheckedComboBoxEdit1.Properties.Items(I - 1)
            Em.CheckState = CheckState.Unchecked
        Next
    End Sub
    Sub CaricaCheck()
        AzzeraCheck()
        Dim Str As String = "select Distinct MccCogLdp from TBMCC where MccPrkid = " & ProgId
        Cmd = New SqlCommand(Str, CnDc)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SettaCheckedCombo(CheckedComboBoxEdit1, dataRd.Item("MccCogLdp"))
        End While
        dataRd.Close()
        LastTbUFa = New DataTable
        LastDaUFa = New SqlDataAdapter("exec XDADCG @ID = " & ProgId, CnDc)
        LastDaUFa.Fill(LastTbUFa)
    End Sub
    Function SettaCheckedCombo(ByVal CheckC As CheckedComboBoxEdit, ByVal Id As Integer) As Boolean
        If Id = -1 Then GoTo II
        For x As Int16 = 1 To CheckC.Properties.Items.Count
            If CheckC.Properties.Items(x - 1).Value = Id Then
                CheckC.Properties.Items(x - 1).CheckState = CheckState.Checked
                SettaCheckedCombo = True : Exit Function
            End If
        Next
II:
        SettaCheckedCombo = False
    End Function

    Sub BolloIva()
        DateEdit1.Properties.ReadOnly = Not (ButtonF3.Enabled)
        TextEdit13.Properties.ReadOnly = Not (ButtonF3.Enabled)
        TextEdit16.Properties.ReadOnly = Not (ButtonF3.Enabled)
        ImageComboBoxEdit4.Properties.ReadOnly = Not (ButtonF3.Enabled)
        TextEdit19.Properties.ReadOnly = Not (ButtonF3.Enabled)
        If Causale = 2 Then Exit Sub
        DateEdit2.Properties.ReadOnly = Not (ButtonF3.Enabled)
        TextEdit3.Properties.ReadOnly = Not (ButtonF3.Enabled)
    End Sub
    Sub LeggoFlagCpt()
        Dim Cmd As New SqlCommand("SELECT PiaFl04,PiaFl08 from TbPia where PiaCodCo = '" & RwFat("Cpt") & "'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("PiaFl04") > 0 And dataRd.Item("PiaFl04") < 25 Then OLDCSP = 1
            If dataRd.Item("PiaFl08") = True Then OLDRIT = 1
        End While
        dataRd.Close()
    End Sub
    Sub LeggoPagamento()
        Dim Cmd As New SqlCommand("SELECT distinct ISNULL(ScaCodPag,0) as PriCodPag FROM TbSca where ScaRifId = " & RwFat("PriId"), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            OLDPAGAM = dataRd.Item("PriCodPag")
        End While
        dataRd.Close()
    End Sub
    Function LeggoAnagrafica(ByVal Cod As String) As Boolean
        LeggoAnagrafica = False
        Fl088 = False
        If Causale = 0 Or Val(Cod) < 1000 Then Exit Function
        Dim StrReg As String = "SELECT * from TbAna where AnaCod = '" & Cod & "' and AnaGrp = '" & GR(Causale) & "'"
        Dim Cmd As New SqlCommand(StrReg, cnVd)

        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            LeggoAnagrafica = True
            TextEdit6.EditValue = dataRd("AnaDesc")
            TextEdit7.EditValue = dataRd("AnaPiva")
            TextEdit9.EditValue = dataRd("AnaCfis")
            TextEdit11.EditValue = dataRd("AnaPivaEst")
            TextEdit8.EditValue = dataRd("AnaIndirizzo")
            TextEdit10.EditValue = dataRd("AnaCap") & " " & Trim(dataRd("AnaCitta")) & " " & dataRd("AnaProv")
        End While
        dataRd.Close()
        If Pcod <> Cod Then TextEdit12.EditValue = "" ''' per cambiare condizioni di pagamento 
        If Val(TextEdit12.EditValue) = 0 And LeggoAnagrafica = True Then
            If Causale = 1 Then
                StrReg = "SELECT * from TbCli where ClCod = '" & Cod & "' "
            Else
                StrReg = "SELECT * from TbFor where FoCod = '" & Cod & "' "
            End If
            Cmd = New SqlCommand(StrReg, cnDb)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                If Causale = 1 Then
                    If Val(TextEdit12.EditValue) = 0 Then TextEdit12.EditValue = dataRd("ClPagam")
                    CheckButton2.Visible = Not dataRd("ClAttivo")
                ElseIf Causale = 2 Then
                    If Val(TextEdit12.EditValue) = 0 Then TextEdit12.EditValue = dataRd("FoPagam")
                    CheckButton2.Visible = Not dataRd("FoAttivo")
                End If
            End While
            dataRd.Close()
        End If
        If Val(TextEdit12.EditValue) = 0 Then
            TextEdit25.EditValue = ""
        Else
            LeggiPagamenti()
        End If
        ControlloCfPi(TextEdit7.EditValue, TextEdit9.EditValue)
        LeggiBanca(Causale, Cod)
    End Function
    Sub LeggiBanca(ByVal cau As Int16, ByVal Codice As String)
        Dim cb As Int16 = 0
        Dim StrReg As String
        If cau = 1 Then
            StrReg = "SELECT * from TbCli where ClCod = '" & Codice & "' "
        Else
            StrReg = "SELECT * from TbFor where FoCod = '" & Codice & "' "
        End If
        Cmd = New SqlCommand(StrReg, cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If Causale = 2 Then
                cb = Val(dataRd("FoCodBan"))
            Else
                cb = Val(dataRd("ClCodBan"))
            End If
            If Causale = 2 AndAlso dataRd("FoSoggRit") = True Then Fl088 = True
        End While
        dataRd.Close()
        If ImageComboBoxEdit3.SelectedIndex < 0 Then ImageComboBoxEdit3.SelectedIndex = SettaComboImage(ImageComboBoxEdit3, cb)
    End Sub
    Sub LeggiPagamenti()
        TextEdit25.EditValue = ""
        Dim StrReg As String = "SELECT * from TbPag where PagCod = " & Val(TextEdit12.EditValue)
        Dim Cmd As New SqlCommand(StrReg, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit25.EditValue = dataRd("PagDesc")
        End While
        dataRd.Close()
    End Sub

    Private Sub TextEdit2_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit2.Validated
        If Sw = 0 Then Exit Sub
        If DateEdit2.EditValue > DateEdit1.EditValue Then DateEdit2.EditValue = DateEdit1.EditValue

        If ControllaProtocollo(Val(TextEdit1.EditValue)) = False Then
            DateEdit2.Focus()
        Else
            TextEdit5.Focus()
        End If
    End Sub

    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        If ComboBoxEdit1.SelectedIndex = -1 Or Sw = 0 Then Exit Sub
        PopolaRegime(ComboBoxEdit1.SelectedIndex)
        REM X SCATENARE L'EVENTO
        Dim P As Int16 = ImageComboBoxEdit2.SelectedIndex
        ImageComboBoxEdit2.SelectedIndex = -1
        ImageComboBoxEdit2.SelectedIndex = P
    End Sub

    Private Sub ImageComboBoxEdit2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit2.SelectedIndexChanged
        Xreg = ImageComboBoxEdit2.SelectedIndex
        If Sw = 0 Or Xreg = -1 Then Exit Sub
        CaricaIniziale(Xreg)
    End Sub
    Private Sub TextEdit1_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextEdit1.Validating
        If Val(TextEdit1.EditValue) > RwReg("ProtCar") + 1 Then
            TextEdit1.EditValue = RwReg("ProtCar") + 1
            e.Cancel = True
        End If
    End Sub
    Private Sub TextEdit2_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextEdit2.Validating
        If ComboBoxEdit1.EditValue > 2018 Then TextEdit2.EditValue = "" : Exit Sub
        If Causale = 1 And Trim(TextEdit2.EditValue) <> "" And Trim(TextEdit2.EditValue) <> "B" And Trim(TextEdit2.EditValue) <> "R" And Trim(TextEdit2.EditValue) <> "S" Then
            e.Cancel = True
        ElseIf Causale = 2 And Trim(TextEdit2.EditValue) <> "" And Trim(TextEdit2.EditValue) <> "B" And Trim(TextEdit2.EditValue) <> "R" Then
            e.Cancel = True
        End If
    End Sub
    Private Sub TextEdit5_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit5.Enter
        Pcod = TextEdit5.Text
    End Sub
    Private Sub TextEdit5_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit5.Validated
        If Val(TextEdit5.Text) > 1000 Then
            Anagrafica()
        Else
            If Val(Pcod) > 1000 Then
                TextEdit5.EditValue = Pcod
            Else
                TextEdit5.EditValue = ""
            End If
        End If
    End Sub
    Sub Anagrafica()
        '  TextEdit5.Text = Trim(TextEdit5.Text).PadLeft(5, "0")
        If LeggoAnagrafica(TextEdit5.Text) = False Then
            If Val(TextEdit5.Text) > 0 Then Messaggio(1, "CODICE " & GroupControl12.Text & " INESISTENTE !!! ")
            TextEdit5.Text = Pcod
            TextEdit5.Focus()
        End If
    End Sub
    Private Sub TextEdit12_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit12.Validated
        Pagamenti()
    End Sub

    Sub Pagamenti()
        LeggiPagamenti()
        TextEdit13.Focus()
    End Sub

    Private Sub DateEdit1_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.Validated
        If OkProt = True Then TextEdit5.Focus()
    End Sub
    Private Sub GroupControl13_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GroupControl13.Click
        Dim PagCod As Int16 = 0
        PagCod = Ricerche.LnkCodPag()
        If PagCod > 0 Then TextEdit12.EditValue = PagCod
        Pagamenti()
    End Sub

    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        If Causale = 0 Then Exit Sub
        Dim frm As New RicercaClFo
        frm.StartPosition = FormStartPosition.Manual
        frm.Location = New Point(GroupControl5.Location.X + 20, GroupControl5.Location.Y + 80)
        frm.CliFor = GR(Causale)
        frm.ShowDialog()
        TextEdit5.EditValue = frm.Codice
        Anagrafica()
        TextEdit5.Focus()
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Controlli() = False Then Exit Sub
        If DoppiaFtFo() = True Then
            ButtonF5.PerformClick()
            Exit Sub
        End If
        REM MONDOMARINE
        If OkMondo = True Then
            If ControlloMondo() = False Then Exit Sub
        End If
        REM PASTA SRL
        If OkOttica = True Then
            If ControlloOttica() = False Then Exit Sub
        End If
        If OkProt = False Then
            LeggiUltimo(DateEdit1.EditValue)
            UltimeCpt()
        End If
        Parallelo()
        IniziaCorpo()
        ButtonEXP.Visible = False
    End Sub
    Function Controlli() As Boolean
        Controlli = True
        SOLOCPT = False
        If CDate(DateEdit2.EditValue) > CDate(DateEdit1.EditValue) Then
            Messaggio(1, "DATA FATTURA MAGGIORE di DATA GIORNALE  ")
            Controlli = False
        End If
        If CDate(DateEdit1.EditValue).Year <> Val(ComboBoxEdit1.EditValue) Then
            Messaggio(1, "ESERCIZIO IVA DIVERSO DALLA DATA GIORNALE  ")
            Controlli = False
        End If
        If Val(TextEdit3.EditValue) = 0 Then
            Messaggio(1, "MANCA IL NUMERO DELLA FATTURA !!!")
            Controlli = False
        End If
        If Val(TextEdit5.EditValue) < 1001 Then
            Messaggio(1, "CODICE " & GroupControl12.Text & " INESISTENTE !!! ")
            Controlli = False
        End If
        If DateEdit1.ErrorText > "" Then
            Messaggio(0, DateEdit1.ErrorText.ToUpper)
            Controlli = False
        End If
        If TextEdit1.ErrorText > "" Then
            Messaggio(0, TextEdit1.ErrorText.ToUpper)
            SOLOCPT = True
        End If
    End Function
    Function DoppiaFtFo() As Boolean
        DoppiaFtFo = False
        If RwReg("RIvaTipo") = 1 Or RwReg("RIvaTipo") = 3 Then Exit Function
        Dim ProtDup As Int32
        Dim ProtReg As Int16
        Dim ProtDub As String = ""
        Dim Str As String = "SELECT * from TBPRI WHERE PRICAUSALE = 3 AND PRIDOCEST = " & Val(TextEdit3.EditValue) & " AND PRICOAVERE = '" & TextEdit5.Text _
        & "' AND PRIDOCANN = DATEPART(YEAR,'" & DateEdit2.EditValue & "')"
        Dim FTFO As New SqlCommand(Str, cnCo)
        dataRd = FTFO.ExecuteReader
        While dataRd.Read
            ProtDup = dataRd.Item("PriNumProt")
            ProtDub = dataRd.Item("PriBisRet")
            ProtReg = dataRd.Item("PriRegIva")
            DoppiaFtFo = True
        End While
        dataRd.Close()
        If DoppiaFtFo = False Then Exit Function
        If ProtDup = Val(TextEdit1.EditValue) And Trim(ProtDub) = Trim(TextEdit2.EditValue) Then
            DoppiaFtFo = False
            Exit Function
        End If
        Messaggio(2, "FATTURA DOPPIA - PROT N. " & ProtDup & ProtDub & " - REG.IVA N. " & ProtReg & Chr(13) & " PROSEGUO LA REGISTRAZIONE ? ")
        If Rispondi = MsgBoxResult.Yes Then
            DoppiaFtFo = False
        Else
            DoppiaFtFo = True
        End If
    End Function
    Function ControlloMondo() As Boolean
        REM MONDOMARINE
        Dim Sblocco As Boolean = False
        Dim DcgCodPag, DcgQuadratura As Integer
        Dim DcgRieTotFat, DcgDifferenza As Decimal
        Select Case Causale
            Case 1
                For j As Int16 = 1 To RegVend.Count
                    If NumReg = RegVend(j - 1) Then GoTo prosegui
                Next
                Return True : Exit Function
prosegui:
                '' If NumReg <> 1 And NumReg <> 9 Then
                If CDate(DateEdit2.EditValue) <= CDate(DataMondoCli) Then Return False : Exit Function
                Cmd = New SqlCommand("SELECT * FROM TbDcg Where DcgCli=@DcgCli AND DCGData=@DcgData and DCGNumero=@DcgNumero and DCGRegistro=@DcgRegistro", cnDb)
                Dim c1 As New SqlParameter("@DcgCli", SqlDbType.VarChar)
                Dim c2 As New SqlParameter("@DcgData", SqlDbType.SmallDateTime)
                Dim c3 As New SqlParameter("@DcgNumero", SqlDbType.Int)
                Dim c4 As New SqlParameter("@DcgRegistro", SqlDbType.SmallInt)
                DCGNUMRIF = -1
                c1.Value = TextEdit5.Text
                c2.Value = DateEdit2.EditValue
                c3.Value = Val(TextEdit3.EditValue)
                c4.Value = NumReg
                Cmd.Parameters.Clear()
                Cmd.Parameters.Add(c1)
                Cmd.Parameters.Add(c2)
                Cmd.Parameters.Add(c3)
                Cmd.Parameters.Add(c4)
                dataRd = Cmd.ExecuteReader
                While dataRd.Read
                    Sblocco = True
                    DCGNUMRIF = dataRd.Item("DcgNumRif")
                    DcgRieTotFat = dataRd.Item("DcgRieTotFat")
                End While
                dataRd.Close()
                If DCGNUMRIF = -1 Then
                    Messaggio(1, "MANCA FATTURA IN ARGO")
                    Return False : Exit Function
                End If
                If CDec(DcgRieTotFat) <> CDec(TextEdit13.EditValue) Then
                    Messaggio(1, "IMPORTO FATTURA <> VERSIONE OTTICA " & Format(DcgRieTotFat, "##,###,##0.00"))
                    Return False : Exit Function
                End If
                Return Sblocco
            Case 2
                If CDate(DateEdit2.EditValue) <= CDate(DataInizio) Then Return True : Exit Function

                Cmd = New SqlCommand("SELECT ISNULL(FoSbloccoFat,0) FROM TbFor Where FoCod ='" & TextEdit5.Text & "'", cnDb)
                dataRd = Cmd.ExecuteReader
                While dataRd.Read
                    Sblocco = CBool(dataRd.Item(0))
                End While
                dataRd.Close()
                If Sblocco = True Then Return True : Exit Function

                DCGNUMRIF = -1
                Dim q1 As New SqlParameter("@DcgCli", SqlDbType.VarChar)
                Dim q2 As New SqlParameter("@DcgData", SqlDbType.SmallDateTime)
                Dim q3 As New SqlParameter("@DcgNumero", SqlDbType.Int)
                Dim q4 As New SqlParameter("@DcgDifferenza", SqlDbType.Decimal)

                Dim Str As String = "SELECT * FROM TbNDcg Where FDCGCLI =@DcgCli AND FDCGData =@DcgData and FDCGNumero = @DcgNumero order by FDcgNumRif desc "

                q1.Value = TextEdit5.Text
                q2.Value = DateEdit2.EditValue
                q3.Value = Val(TextEdit3.EditValue)
                q4.Value = CDec(TextEdit13.EditValue) * -1

                If CDec(TextEdit13.EditValue) < 0 Then
                    Str = "SELECT * FROM VDIFFCOGE Where FDCGCLI =@DcgCli AND FDCGDifferenza=@DcgDifferenza"
                End If
                Cmd = New SqlCommand(Str, cnDb)

                Cmd.Parameters.Clear()
                Cmd.Parameters.Add(q1)
                Cmd.Parameters.Add(q2)
                Cmd.Parameters.Add(q3)

                If CDec(TextEdit13.EditValue) < 0 Then Cmd.Parameters.Add(q4)

                dataRd = Cmd.ExecuteReader
                While dataRd.Read
                    DCGNUMRIF = dataRd.Item("FDcgNumRif")
                    DcgRieTotFat += dataRd.Item("FDcgRieTotFat")
                    DcgDifferenza += dataRd.Item("FDcgDifferenza")
                    DcgCodPag = dataRd.Item("FDcgCodPag")
                    DcgQuadratura += dataRd.Item("FDcgQuadratura")
                End While
                dataRd.Close()

                If DCGNUMRIF = -1 Then
                    Messaggio(1, "FORNITORE BLOCCATO - MANCA FATTURA ")
                    ControlloMondo = False : Exit Function
                End If

                If DcgQuadratura <> 1 Then '''' note credito
                    If CDec(DcgRieTotFat) <> CDec(TextEdit13.EditValue) Then
                        Messaggio(1, "IMPORTO FATTURA <> VERSIONE OTTICA " & Format(DcgRieTotFat, "##,###,##0.00"))
                        ControlloMondo = False : Exit Function
                    End If
                End If
                If DcgCodPag <> Val(TextEdit12.EditValue) Then
                    TextEdit12.ErrorText = "         " & DcgCodPag
                Else
                    TextEdit12.ErrorText = ""
                End If
                Return True
        End Select
    End Function
    Function ControlloOttica() As Boolean
        REM x ORA PASTA SRL  solo Fornitori Blocco Fat
        Dim Sblocco As Boolean = False
        Dim DcgCodPag, DcgQuadratura As Integer
        Dim DcgRieTotFat, DcgDifferenza As Decimal
        Select Case Causale
            Case 1
                Return True : Exit Function
                '                For j As Int16 = 1 To RegVend.Count
                '                    If NumReg = RegVend(j - 1) Then GoTo prosegui
                '                Next
                '                Return True : Exit Function
                'prosegui:
                '                '' If NumReg <> 1 And NumReg <> 9 Then
                '                If CDate(DateEdit2.EditValue) <= CDate(DataMondoCli) Then Return False : Exit Function
                '                Cmd = New SqlCommand("SELECT * FROM TbDcg Where DcgCli=@DcgCli AND DCGData=@DcgData and DCGNumero=@DcgNumero and DCGRegistro=@DcgRegistro", cnDb)
                '                Dim c1 As New SqlParameter("@DcgCli", SqlDbType.VarChar)
                '                Dim c2 As New SqlParameter("@DcgData", SqlDbType.SmallDateTime)
                '                Dim c3 As New SqlParameter("@DcgNumero", SqlDbType.Int)
                '                Dim c4 As New SqlParameter("@DcgRegistro", SqlDbType.SmallInt)
                '                DCGNUMRIF = -1
                '                c1.Value = TextEdit5.Text
                '                c2.Value = DateEdit2.EditValue
                '                c3.Value = Val(TextEdit3.EditValue)
                '                c4.Value = NumReg
                '                Cmd.Parameters.Clear()
                '                Cmd.Parameters.Add(c1)
                '                Cmd.Parameters.Add(c2)
                '                Cmd.Parameters.Add(c3)
                '                Cmd.Parameters.Add(c4)
                '                dataRd = Cmd.ExecuteReader
                '                While dataRd.Read
                '                    Sblocco = True
                '                    DCGNUMRIF = dataRd.Item("DcgNumRif")
                '                    DcgRieTotFat = dataRd.Item("DcgRieTotFat")
                '                End While
                '                dataRd.Close()
                '                If DCGNUMRIF = -1 Then
                '                    Messaggio(1, "MANCA FATTURA IN ARGO")
                '                    Return False : Exit Function
                '                End If
                '                If CDec(DcgRieTotFat) <> CDec(TextEdit13.EditValue) Then
                '                    Messaggio(1, "IMPORTO FATTURA <> VERSIONE OTTICA " & Format(DcgRieTotFat, "##,###,##0.00"))
                '                    Return False : Exit Function
                '                End If
                '                Return Sblocco
            Case 2
                If CDate(DateEdit2.EditValue) <= CDate(DataInizio) Then Return True : Exit Function

                Cmd = New SqlCommand("SELECT ISNULL(FoSbloccoFat,0) FROM TbFor Where FoCod ='" & TextEdit5.Text & "'", cnDb)
                dataRd = Cmd.ExecuteReader
                While dataRd.Read
                    Sblocco = CBool(dataRd.Item(0))
                End While
                dataRd.Close()
                If Sblocco = True Then Return True : Exit Function

                DCGNUMRIF = -1
                Dim q1 As New SqlParameter("@DcgCli", SqlDbType.VarChar)
                Dim q2 As New SqlParameter("@DcgData", SqlDbType.SmallDateTime)
                Dim q3 As New SqlParameter("@DcgNumero", SqlDbType.Int)
                Dim q4 As New SqlParameter("@DcgDifferenza", SqlDbType.Decimal)

                Dim Str As String = "SELECT * FROM TbNDcg Where FDCGCLI =@DcgCli AND FDCGData =@DcgData and FDCGNumero = @DcgNumero order by FDcgNumRif desc "

                q1.Value = TextEdit5.Text
                q2.Value = DateEdit2.EditValue
                q3.Value = Val(TextEdit3.EditValue)
                q4.Value = CDec(TextEdit13.EditValue) * -1

                If CDec(TextEdit13.EditValue) < 0 Then
                    Str = "SELECT * FROM VDIFFCOGE Where FDCGCLI =@DcgCli AND FDCGDifferenza=@DcgDifferenza"
                End If
                Cmd = New SqlCommand(Str, cnDb)

                Cmd.Parameters.Clear()
                Cmd.Parameters.Add(q1)
                Cmd.Parameters.Add(q2)
                Cmd.Parameters.Add(q3)

                If CDec(TextEdit13.EditValue) < 0 Then Cmd.Parameters.Add(q4)

                dataRd = Cmd.ExecuteReader
                While dataRd.Read
                    DCGNUMRIF = dataRd.Item("FDcgNumRif")
                    DcgRieTotFat += dataRd.Item("FDcgRieTotFat")
                    DcgDifferenza += dataRd.Item("FDcgDifferenza")
                    DcgCodPag = dataRd.Item("FDcgCodPag")
                    DcgQuadratura += dataRd.Item("FDcgQuadratura")
                End While
                dataRd.Close()

                If DCGNUMRIF = -1 Then
                    Messaggio(1, "FORNITORE BLOCCATO - MANCA FATTURA ")
                    ControlloOttica = False : Exit Function
                End If

                If DcgQuadratura <> 1 Then '''' note credito
                    If CDec(DcgRieTotFat) <> CDec(TextEdit13.EditValue) Then
                        Messaggio(1, "IMPORTO FATTURA <> VERSIONE OTTICA " & Format(DcgRieTotFat, "##,###,##0.00"))
                        ControlloOttica = False : Exit Function
                    End If
                End If
                If DcgCodPag <> Val(TextEdit12.EditValue) Then
                    TextEdit12.ErrorText = "         " & DcgCodPag
                Else
                    TextEdit12.ErrorText = ""
                End If
                Return True
        End Select
    End Function
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
    Sub UltimeCpt()
        Dim Id As Int32 = 0
        Dim Cmd As New SqlCommand("SELECT ISNULL(MAX(PRIID),0) FROM TBPRI WHERE PRIREGIVA = " & Val(NumOldReg) & TC(Causale) & " '" & TextEdit5.Text & "' And PRICAUSALE = " & Causale & " And PriMeseSk = '*'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Id = dataRd.Item(0)
        End While
        dataRd.Close()
        If Id > 0 Then GoTo OkTrovato
        Cmd = New SqlCommand("SELECT ISNULL(MAX(PRIID),0) FROM TBPRI WHERE PRIREGIVA = " & Val(NumOldReg) & TC(Causale) & " '" & TextEdit5.Text & "' And PRICAUSALE = " & Causale, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Id = dataRd.Item(0)
        End While
        dataRd.Close()
OkTrovato:
        DsFat = New DataSet(Fa)
        DaFat = New SqlDataAdapter("SELECT * from VH1H2 where PriId = " & Id, cnCo)
        DaFat.SelectCommand.CommandTimeout = 300
        DaFat.Fill(DsFat, Fa)
        GridControl1.DataSource = DsFat.Tables(Fa)
        GridControl1.Refresh()
        If DsFat.Tables(Fa).Rows.Count > 0 Then
            ScorporaDati()
        End If
    End Sub
    Sub Parallelo()
        If DsFat.Tables(Fa).Rows.Count > 0 Then
            RwFat = DsFat.Tables(Fa).Rows(0)
            ProgId = RwFat("PriId")
            OkFat = True
        Else
            RwFat = DsFat.Tables(Fa).NewRow
            RwFat("PriProg") = 1
            RwFat("PriCodIva") = 0
            RwFat("PriImpDare") = 0
            RwFat("PriImpavere") = 0
            RwFat("Cpt") = "00.00"
            RwFat("PriValuta") = 0
            RwFat("PriIvaPrint") = 0
            RwFat("PriGstampa") = 0
            OkFat = False
        End If
        IniziaRwFat()
        ProgId = RwFat("PriId")
        DsFcf = New DataSet(Ft)
        DaFcf = New SqlDataAdapter("SELECT *,0 as PriFl08 from TbPri where PriCausale < 3 and PriId = " & ProgId, cnCo)
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
        RwFat("PridataEst") = DateEdit2.EditValue
        RwFat("PriDocEst") = Val(TextEdit3.EditValue)
        RwFat("PriDesc") = Trim(TextEdit4.EditValue)
        RwFat("PriCodPag") = Val(TextEdit12.EditValue)
        RwFat("PriLinea") = Val(ImageComboBoxEdit3.EditValue)
        RwFat("CONTO") = TextEdit5.Text
        RwFat("DAREDESC") = TextEdit6.EditValue
    End Sub
    Sub IniziaRwFcf()
        If OkFcf = True Then Ricontrolla() '''' PURTROPPO IL MOVIMENTO DEL MOUSWE GENERA CONFUSIONE
        RwFcf("PriDataGio") = CDate(DateEdit1.EditValue)
        RwFcf("PriCausale") = Causale
        If Causale = 1 Then
            RwFcf("PriCoDare") = TextEdit5.Text
            RwFcf("PriCoAvere") = RwFat("Cpt")
        Else
            RwFcf("PriCoDare") = RwFat("Cpt")
            RwFcf("PriCoAvere") = TextEdit5.Text
        End If
        RwFcf("PriNumProt") = Val(TextEdit1.EditValue)
        RwFcf("PriBisRet") = TextEdit2.EditValue
        RwFcf("PriRegIva") = NumReg
        RwFcf("PriDesc") = TextEdit4.EditValue
        RwFcf("PriDocEst") = Val(TextEdit3.EditValue)
        RwFcf("PriMeseSk") = ""
        RwFcf("PriDataEst") = CDate(DateEdit2.EditValue)
        RwFcf("PriDescB") = ""
        RwFcf("PriFl04") = Fl04
        RwFcf("PriFl05") = Fl05
        RwFcf("PriFl06") = Fl06
        If Fl08 = True Or Fl088 = True Then RwFcf("PriFl08") = True Else RwFcf("PriFl08") = False
        RwFcf("PriNsRif") = ""
        RwFcf("PriSos") = ""
        RwFcf("PriLinea") = Val(ImageComboBoxEdit3.EditValue)
        RwFcf("PriDocAnn") = CDate(DateEdit2.EditValue).Year
        RwFcf("PriCodPag") = Val(TextEdit12.EditValue)
        RwFcf("PriValuta") = RwFat("PriValuta")
        RwFcf("PriArtFisc") = 0
        RwFcf("PriId") = ProgId
        RwFcf("PriProg") = POSRIG
        RwFcf("PriIvaPrint") = RwFat("PriIvaPrint")
        RwFcf("PriGStampa") = RwFat("PriGStampa")
        ''' se esiste lo scorporo cambiare
        RwFcf("PriCodIva") = RwFat("PriCodIva")
        RwFcf("PriImpDare") = RwFat("PriImpDare")
        RwFcf("PriImpavere") = RwFat("PriImpavere")
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
    Sub ScorporaDati()
        Dim q As Int16
        If DsFat.Tables(Fa).Rows.Count = 1 Then
            RwFat = DsFat.Tables(Fa).Rows(0)
            RwFat("Priid") = ProgId
            RwFat("PriValuta") = 0
            RwFat("PriIvaPrint") = 0
            RwFat("PriArtFisc") = 0
            RwFat("PriGStampa") = 0
            UltimaRiga(CDec(TextEdit13.EditValue), True)
        Else
            For q = 1 To DsFat.Tables(Fa).Rows.Count
                RwFat = DsFat.Tables(Fa).Rows(q - 1)
                RwFat("Priid") = ProgId
                RwFat("PriProg") = q
                RwFat("PriImpDare") = 0
                RwFat("PriImpAvere") = 0
                RwFat("PriValuta") = 0
                RwFat("PriArtFisc") = 0
                RwFat("PriGStampa") = 0
                RwFat("PriIvaPrint") = 0
            Next
        End If
    End Sub
    Sub UltimaRiga(ByVal Residuo As Decimal, ByVal Tipo As Boolean)
        If SOLOCPT = True Then GoTo Inext
        If Tipo = True Then
            RwFat("PriImpDare") = Format((CDec(Residuo) - (X0 * RwFat("CiiAli") / 100)) / (100 + RwFat("CiiAli")) * 100, "##########0.00")
        End If

        RwFat("PriImpAvere") = (RwFat("PriImpDare") + X0) * RwFat("CiiAli") / 100
Inext:
        RwFat("PriImpAvere") = Format(RwFat("PriImpAvere"), "#########0.00")
    End Sub
    Sub IniziaCorpo()
        AbilitaGroup(1)
        TotaleFattura = CDec(TextEdit13.EditValue)
        TotaleResiduo = TotaleFattura
        TotaleReg = 0
        X1 = 0
        ' CARICA PRIMA RIGA
        CaricaDettagli(1)
    End Sub
    Sub CaricaDettagli(ByVal I As Int16)
        TextEdit15.EditValue = RwFcf("PriProg")
        TextEdit16.EditValue = RwFcf("PriImpDare")
        ImageComboBoxEdit4.SelectedIndex = RwFcf("PriCodIva")
        TextEdit19.EditValue = RwFcf("PriImpAvere")
        TextEdit22.EditValue = RwFcf("PriValuta")
        If Causale = 1 Then
            TextEdit20.EditValue = RwFcf("PriCoAvere")
        Else
            TextEdit20.EditValue = RwFcf("PriCoDare")
        End If
        If ImageComboBoxEdit4.SelectedIndex > 0 Then LeggiCodiciIva(ImageComboBoxEdit4.SelectedIndex)
        ''''''Else
        ''''''    TextEdit18.EditValue = ""
        ''''''End If
        LeggiConto()
        TotaleIn()
        TextEdit16.Focus()
    End Sub

    Function LeggiConto() As Boolean
        TextEdit21.EditValue = "*** ERRATO ***"
        Fl04 = 0
        Fl05 = 0
        Fl06 = 0
        Fl08 = False
        LeggiConto = False
        AggiustaConto() ''''' verifica il punto se e' un sottoconto
        Dim Str As String = "SELECT * from TbPia where PiaCodCo = '" & TextEdit20.EditValue & "'"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit21.EditValue = dataRd("PiaAnaCo")
            Fl04 = dataRd("PiaFl04")
            Fl05 = dataRd("PiaFl05")
            Fl06 = dataRd("PiaFl06")
            Fl08 = dataRd("PiaFl08")
            LeggiConto = True
        End While
        dataRd.Close()
        If LeggiConto = True Then Exit Function
        Str = "SELECT * from TbAna where AnaCoD = '" & TextEdit20.EditValue & "' AND (AnaGrp='CL' OR AnaGrp='FO')"
        Cmd = New SqlCommand(Str, cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit21.EditValue = dataRd("AnaRag1")
            LeggiConto = True
        End While
        dataRd.Close()
    End Function
    Function LeggiCodiciIva(ByVal i As Int16) As String
        RwCii = DsCii.Tables(Ci).Rows(i - 1)
        Return RwCii("CiiDes")
    End Function
    Sub TotaleIn()
        'If Trim(TextEdit16.EditValue) = "" Then TextEdit16.EditValue = CDec(0.0)
        'If Trim(TextEdit19.EditValue) = "" Then TextEdit19.EditValue = CDec(0.0)
        'If Trim(TextEdit22.EditValue) = "" Then TextEdit22.EditValue = CDec(0.0)
        TotTransito = TotaleReg + ((CDec(TextEdit16.EditValue) + CDec(TextEdit19.EditValue)))
        TextEdit14.EditValue = TotaleFattura - TotTransito
    End Sub
    Sub AggiustaConto()
        Dim x As Int16
        For x = 1 To Len(TextEdit20.Text)
            If Mid(TextEdit20.Text, x, 1) = "." Then
                TextEdit20.Text = Format(Val(Mid(TextEdit20.Text, 1, x - 1)), "00") & "." & Format(Val(Mid(TextEdit20.Text, x + 1, Len(TextEdit20.Text) - (x - 1))), "00")
                Exit Sub
            End If
        Next
    End Sub
    'inizio input righe
    Private Sub TextEdit16_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit16.Validated
        If TextEdit16.EditValue.ToString = "" Then TextEdit16.EditValue = "00.00"
        CalcolaRiga(0)
    End Sub
    Private Sub ImageComboBoxEdit4_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit4.Validated
        CalcolaRiga(0)
        R = 0
    End Sub
    Private Sub TextEdit19_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit19.Validated
        If TextEdit16.EditValue.ToString = "" Then TextEdit19.EditValue = "00.00"
        CalcolaRiga(1)
    End Sub
    Private Sub TextEdit16_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit16.Enter
        DS = 0
    End Sub

    Private Sub TextEdit17_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        DS = 1 : R = 17
    End Sub
    Private Sub TextEdit19_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit19.Enter
        DS = 2
    End Sub
    Private Sub TextEdit20_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit20.Enter
        DS = 3 : R = 20
    End Sub
    Private Sub ButtonF11_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF11.Enter
        DS = 4
    End Sub
    Private Sub TextEdit20_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit20.Validated
        TextEdit20.EditValue = TextEdit20.Text.PadLeft(5, "0")
        LeggiConto()
        CalcolaRiga(2)
    End Sub
    Private Sub TextEdit22_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit22.Validated
        If TextEdit22.EditValue.ToString = "" Then TextEdit22.EditValue = CDec(0.0)
        CalcolaRiga(3)
    End Sub
    Private Sub ImageComboBoxEdit4_CloseUp(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles ImageComboBoxEdit4.CloseUp
        If Sw > 0 Then System.Windows.Forms.SendKeys.Send("{TAB}")
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If ControllaConto() = False Then
            TextEdit20.Focus()
            Exit Sub
        End If
        RicalcolaDocumento()
    End Sub
    Sub CalcolaRiga(ByVal i As Int16)
        If i = 0 Then
            RwFat("PriImpDare") = CDec(TextEdit16.EditValue)
            RwFat("PriCodIva") = 0
            RwFat("CiiAli") = 0
            RwFat("CiiDes") = ""
            ''TextEdit18.EditValue = ""
            If ImageComboBoxEdit4.SelectedIndex > 0 Then
                LeggiCodiciIva(ImageComboBoxEdit4.SelectedIndex)
                RwFat("CiiDes") = RwCii("CiiDes")
                RwFat("PriCodIva") = ImageComboBoxEdit4.SelectedIndex
                RwFat("CiiAli") = RwCii("CiiAli")
            End If
            UltimaRiga(0, False)
            TextEdit19.EditValue = RwFat("PriImpAvere")
            TextEdit22.EditValue = RwFat("PriValuta")
        End If
        If i = 1 Then
            RwFat("PriImpAvere") = CDec(TextEdit19.EditValue)
        End If
        If i = 2 Then
            RwFat("Cpt") = TextEdit20.EditValue
            RwFat("AVEREDESC") = TextEdit21.EditValue
        End If
        If i = 3 Then
            RwFat("PriValuta") = CDec(TextEdit22.EditValue)
        End If
        TotaleIn()
    End Sub
    Function ControllaConto() As Boolean
        ControllaConto = False
        AggiustaConto()
        If Mid(TextEdit20.Text, 3, 3) = ".00" Or Mid(TextEdit20.Text, 1, 3) = "00." Then Exit Function '' mastri e transitorio
        ControllaConto = LeggiConto()
    End Function
    Sub RicalcolaDocumento()
        If OkFat = False Then DsFat.Tables(Fa).Rows.Add(RwFat)
        POSRIG = RwFat("PriProg")
        IniziaRwFcf()
        If OkFcf = False Then DsFcf.Tables(Ft).Rows.Add(RwFcf)
        RicalcoloFinale(POSRIG + 1)
        If TotaleResiduo = 0 Then
            RegistraFattura()
            Exit Sub
        End If
        POSRIG = POSRIG + 1
        RigeneraFat()
        RigeneraFcF()
        CaricaDettagli(POSRIG)
    End Sub
    Sub RicalcoloFinale(ByVal i As Int16)
        Dim x As Int16
        X1 = 0
        X0 = 0
        For x = 1 To i - 1
            RwFRI = DsFcf.Tables(Ft).Rows(x - 1)
            X1 = X1 + RwFRI("PriImpDare") + RwFRI("PriImpAvere")
            If RwFRI("PriCodIva") = 0 Then
                X0 = X0 + RwFRI("PriImpDare")
            Else
                X0 = 0
            End If
        Next
        TotaleReg = X1
        TotaleResiduo = TotaleFattura - TotaleReg
        TextEdit14.EditValue = TotaleResiduo
    End Sub
    Sub RigeneraFat()
        Dim Scorp As Boolean = True
        If OkFat = False Then GoTo Aggiungi
        Dim x As Int16
        For x = 1 To DsFat.Tables(Fa).Rows.Count
            RwFat = DsFat.Tables(Fa).Rows(x - 1)
            If RwFat("PriProg") = POSRIG Then
                IniziaRwFat()
                If x = DsFat.Tables(Fa).Rows.Count Then
                    Scorp = True
                ElseIf OkFat = False Then
                    Scorp = True
                Else
                    Scorp = False
                End If
                GoTo scorporo
            End If
        Next
        OkFat = False
Aggiungi:
        RwFat = DsFat.Tables(Fa).NewRow
        IniziaRwFat()
        RwFat("PriProg") = POSRIG
        RwFat("PriCodIva") = 0
        RwFat("PriImpDare") = 0
        RwFat("PriImpavere") = 0
        RwFat("Cpt") = "00.00"
        RwFat("PriValuta") = 0
        RwFat("PriIvaPrint") = 0
        RwFat("PriGStampa") = 0
        Exit Sub
Scorporo:
        UltimaRiga(TotaleResiduo, Scorp)
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
    'fine   input righe
    Sub RegistraFattura()
        Dim x As Int16
        For x = POSRIG + 1 To DsFcf.Tables(Ft).Rows.Count
            RwFRI = DsFcf.Tables(Ft).Rows(x - 1)
            RwFRI.Delete()
        Next
        DsFcf.AcceptChanges()
        ScriviPri()
        ButtonFF5.PerformClick()
        If RwReg("RivaFteP") = True Then IngressoFTE()
    End Sub
    Sub ScriviPri()
        EliminaProt()
        EliminaScad()
        Dim x, M, Sr, Sc As Int16
        Dim Ival, IvaDetra, IvaPScadenze As Decimal
        Dim NEWCONTO As String = ""
        Dim charge As Boolean = RwReg("RIvaRCharge")
        RivalsaoCee = False

RipetiCee:
        Ival = 0 : IvaDetra = 0 : IvaPScadenze = 0 : Sr = 0 : Sc = 0
        M = DsFcf.Tables(Ft).Rows.Count + 1 ''' PER LA CAUSALE 3
        For x = 1 To M
            Wrd.Parameters.Clear()
            If x < M Then
                RwFRI = DsFcf.Tables(Ft).Rows(x - 1)
                If RwFRI("PriCodIva") > 0 Then
                    LeggiCodiciIva(RwFRI("PriCodIva"))
                    Ival = RwFRI("PriImpAvere") - (RwFRI("PriImpAvere") * (100 - RwCii("CiiInd")) / 100)
                    Ival = Format(Ival, "#########0.00")
                    IvaDetra = IvaDetra + RwFRI("PriImpAvere") - Ival
                Else
                    IvaDetra = IvaDetra + RwFRI("PriImpAvere")
                End If
                IvaPScadenze = IvaPScadenze + RwFRI("PriImpAvere")

                If RwFRI("PriFl08") = 1 Then Sr = 1
                If RwFRI("PriFl04") > 0 And RwFRI("PriFl04") < 25 Then Sc = 1
            End If
            If CheckEdit1.Checked = True Then RwFRI("PriMeseSk") = "*" Else RwFRI("PriMeseSk") = ""
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
            p20.Value = ""
            p21.Value = RwFRI("PriLinea")
            p22.Value = RwFRI("PriDocAnn")
            p23.Value = 0
            p24.Value = RwFRI("PriValuta")
            p25.Value = RwFRI("PriArtFisc")
            p26.Value = RwFRI("PriId")
            p27.Value = RwFRI("PriProg")
            p28.Value = RwFRI("PriIvaPrint")
            p29.Value = RwFRI("PriGStampa")
            If x = M Then
                p2.Value = 3
                p16.Value = 0
                p17.Value = 0
                p18.Value = 0
                p24.Value = 0
                p27.Value = RwFRI("PriProg") + 1
                p23.Value = RwFRI("PriCodPag")
                If RwFRI("PriCausale") = 2 Then
                    p3.Value = IvaCpt
                    p10.Value = TotaleFattura
                    p9.Value = IvaDetra
                    NEWCONTO = RwFRI("PriCoAvere")
                    If M > 2 Then p20.Value = "00.10" Else p20.Value = RwFRI("PriCoDare")
                Else
                    p4.Value = IvaCpt
                    p9.Value = TotaleFattura
                    p10.Value = IvaDetra
                    NEWCONTO = RwFRI("PriCoDare")
                    If M > 2 Then p20.Value = "00.10" Else p20.Value = RwFRI("PriCoAvere")
                End If
            End If
            Wrd.Parameters.Add(p1)
            Wrd.Parameters.Add(p2)
            Wrd.Parameters.Add(p3)
            Wrd.Parameters.Add(p4)
            Wrd.Parameters.Add(p5)
            Wrd.Parameters.Add(p6)
            Wrd.Parameters.Add(p7)
            Wrd.Parameters.Add(p8)
            Wrd.Parameters.Add(p9)
            Wrd.Parameters.Add(p10)
            Wrd.Parameters.Add(p11)
            Wrd.Parameters.Add(p12)
            Wrd.Parameters.Add(p13)
            Wrd.Parameters.Add(p14)
            Wrd.Parameters.Add(p15)
            Wrd.Parameters.Add(p16)
            Wrd.Parameters.Add(p17)
            Wrd.Parameters.Add(p18)
            Wrd.Parameters.Add(p19)
            Wrd.Parameters.Add(p20)
            Wrd.Parameters.Add(p21)
            Wrd.Parameters.Add(p22)
            Wrd.Parameters.Add(p23)
            Wrd.Parameters.Add(p24)
            Wrd.Parameters.Add(p25)
            Wrd.Parameters.Add(p26)
            Wrd.Parameters.Add(p27)
            Wrd.Parameters.Add(p28)
            Wrd.Parameters.Add(p29)
            Wrd.ExecuteNonQuery()
        Next
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        If RwReg("RIvaFteP") = True And RIFERFTEP > 0 Then RegistraFteP()
        ResetIdP()
        '
        AssegnaNPartita()

        If RwFRI("PriNumProt") > RwReg("ProtCar") Then RwReg("ProtCar") = RwFRI("PriNumProt")
        If RwFRI("PridataGio") <> RwReg("DataCar") Then RwReg("DataCar") = RwFRI("PriDataGio")
        If RwFRI("PriCodPag") > 0 Then EsegueSql(" EXEC XCREASCADENZE  @IDP = " & ProgId, cnCo)
        Partita(1)
        If TotaleFattura = 0 Or RwFRI("PriCodPag") = 0 Then
            EliminaScad()
            GoTo DopoScad
        End If
DopoScad:
        If Sc = 1 Then LancioCespiti()
        If Sr = 1 Then LancioRitenute(IvaPScadenze)
        If OLDCSP = 1 Then
            Messaggio(1, "CONTROLLARE CESPITI")
        End If
        If OLDRIT = 1 Then
            Messaggio(1, "CONTROLLARE RITENUTE")
        End If
        If OkProt = True And GroupControl17.Visible = True Then
            Messaggio(1, "CONTROLLARE REGISTRO IVA INTRACEE VENDITE")
        End If
        REM MONDOMARINE
        If OkMondo = True Then LancioCDCMONDO()
        If OkLDP = True Then LancioLDP()
        If OkOttica = True And Causale = 2 Then AggiornaDcg()
        '' If OKCDC = True Then LancioCDC()
        If OkProt = False And GroupControl17.Visible = True Then
            charge = False
            IvaCpt = RicaricaCee()
            GoTo RipetiCee
        End If
        REM MODIFICA PER REVERSE CHARGE
        If OkProt = False And charge = True Then
            IvaCpt = ReverseCharge()
            charge = False
            GoTo RipetiCee
        End If
        If RivalsaoCee = True And ArtGcRCee > 0 And OkProt = False Then
            GiroRivalsaECee(ProgId, TextEdit5.Text, TextEdit6.Text, ArtGcRCee, MiglioFo)
            RivalsaoCee = False
        End If
        IvaCpt = RwReg("RivaCpt")
    End Sub
    Sub RegistraFteP()
        Dim UPFTEP As String = "Update TbFte_Passiva Set FteRifPri=" & ProgId & " where FteRif=" & RIFERFTEP
        Dim Umd As New SqlCommand(UPFTEP, cnDb)
        Umd.ExecuteNonQuery()
        RIFERFTEP = 0
    End Sub
    Sub EliminaRegistraFteP()
        Dim UPFTEP As String = "Update TbFte_Passiva Set FteRifPri=0 where FteRifPri=" & ProgId
        Dim Umd As New SqlCommand(UPFTEP, cnDb)
        Umd.ExecuteNonQuery()
        RIFERFTEP = 0
    End Sub
    Sub EliminaProt()
        Partita(0)
        Dim Cancella As String = "BEGIN Delete from TbPri where PriId = " & ProgId & " Delete from TbPrk where PrKId = " & ProgId & " END"
        Dim Dmd As New SqlCommand(Cancella, cnCo)
        Dmd.ExecuteNonQuery()
        Dim UPDMONDO As String = "BEGIN Update TbFat Set FatTrasf=0 where FatRif =" & DCGNUMRIF & " Update TbDcg Set DcgTrasf=0 where DcgNumRif =" & DCGNUMRIF & " END"
        Dim Umd As New SqlCommand(UPDMONDO, cnDb)
        If OkMondo = True Then
            If NumReg <> 1 And NumReg <> 9 Then Exit Sub
            If DCGNUMRIF > 0 Then Umd.ExecuteNonQuery()
        End If
    End Sub
    Sub EliminaScad()
        Dim Cancella As String = "Delete from TbSca where ScaRifId = " & ProgId
        Dim Dmd As New SqlCommand(Cancella, cnCo)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub AssegnaNPartita()
        Dim Command As New SqlClient.SqlCommand("AssegnaPartita")
        Command.CommandType = CommandType.StoredProcedure
        Command.Connection = cnCo
        Dim q1 As New SqlParameter("@CONTO", SqlDbType.VarChar)
        Dim q2 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim q3 As New SqlParameter("@NDOC", SqlDbType.Int)
        Dim q4 As New SqlParameter("@IMPORTO", SqlDbType.Decimal)
        q1.Value = TextEdit5.Text
        q2.Value = RwFRI("PriDocAnn")
        q3.Value = RwFRI("PriDocEst")
        q4.Value = TotaleFattura
        Command.Parameters.Clear()
        Command.Parameters.Add(q1)
        Command.Parameters.Add(q2)
        Command.Parameters.Add(q3)
        Command.Parameters.Add(q4)
        Command.CommandTimeout = 300
        Command.ExecuteNonQuery()
    End Sub
    Sub Partita(ByVal Tipo As Int16)
        If Tipo = 0 Then
            EsegueSql(" EXEC RiAprePartita  @Id = " & ProgId & ",@Az= 0,@Miglio=" & MiglioFo, cnCo)
        Else
            EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
        End If
    End Sub
    Sub LancioCespiti()
        Dim Gesterna As New DxInsCesp
        OLDCSP = 0
        Gesterna.ShowDialog()
    End Sub
    Sub LancioRitenute(ByVal IvaSpe As Decimal)
        Dim Gesterna As New DxProRit
        OLDRIT = 0
        DxProRit.PAnaCod = TextEdit5.EditValue
        DxProRit.PAnaDesc = TextEdit6.EditValue
        DxProRit.PNumDoc = RwFRI("PriDocEst")
        DxProRit.PDatDoc = CDate(RwFRI("PriDataEst")).ToShortDateString
        DxProRit.PImport = TotaleFattura - IvaSpe
        DxProRit.PIva = IvaSpe
        Gesterna.ShowDialog()
    End Sub
    Sub LancioScarat(ByVal IvaSpe As Decimal)
        Dim Gesterna As New DxScaRat
        Gesterna.NAnaCod = TextEdit5.EditValue
        Gesterna.NAnaDesc = TextEdit6.EditValue
        Gesterna.NCodPag = OLDPAGAM
        Gesterna.NAnaPag = TextEdit25.EditValue
        Gesterna.NNumDoc = RwFRI("PriDocEst")
        Gesterna.NDatDoc = CDate(RwFRI("PriDataEst")).ToShortDateString
        Gesterna.NImport = TotaleFattura
        Gesterna.NIvaSpe = IvaSpe
        Gesterna.StartPosition = FormStartPosition.CenterScreen
        Gesterna.ShowDialog()
    End Sub
    Sub AggiornaDcg()
        REM MONDOMARINE
        Dim Dmd As New SqlCommand("Update TbNDCG set FDcgTrasf = 1 where FdcgNumRif =" & DCGNUMRIF, cnDb)
        REM NOTA CREDITO
        If CDec(TextEdit13.EditValue) < 0 Then
            Dmd = New SqlCommand("Update TbNDCG set FDcgTrasf = 1,FDcgQuadratura = 0 where FdcgNumRif =" & DCGNUMRIF, cnDb)
        End If
        Dmd.ExecuteNonQuery()
        DCGNUMRIF = TrovoDoc()
        If DCGNUMRIF <= 0 Then Exit Sub
        Dim AggTbDoc As String = "Update TbDoc set DocProt = @Prot,DocReg=@Reg,DocAnnoCoge=@AnnoCoge where DocKeyCm =@NumRif"
        Dim Command As New SqlCommand(AggTbDoc, cnVd)
        Dim q1 As New SqlParameter("@Prot", SqlDbType.VarChar)
        Dim q2 As New SqlParameter("@Reg", SqlDbType.SmallInt)
        Dim q3 As New SqlParameter("@NumRif", SqlDbType.Int)
        Dim q4 As New SqlParameter("@AnnoCoge", SqlDbType.SmallInt)
        q1.Value = RwFRI("PriNumProt")
        q2.Value = RwFRI("PriRegIva")
        q3.Value = DCGNUMRIF
        q4.Value = Val(ComboBoxEdit1.EditValue)
        Command.Parameters.Clear()
        Command.Parameters.Add(q1)
        Command.Parameters.Add(q2)
        Command.Parameters.Add(q3)
        Command.Parameters.Add(q4)
        Command.ExecuteNonQuery()
    End Sub
    Function TrovoDoc() As Integer
        Dim TrovaKey As String = "Select DocKeyCm from TbDoc where DocNumRif =@NumRif and DocTipo = @Tipo"
        Dim TrovaXF As String = "Select TesRif from TbTes where TesFatRif = @Numrif and TesTipoDoc = 'X'"
        Dim CmKey As Integer = -1
        Dim Command As New SqlCommand(TrovaKey, cnVd)
        Dim q3 As New SqlParameter("@NumRif", SqlDbType.Int)
        Dim q4 As New SqlParameter("@Tipo", SqlDbType.VarChar)
        Dim q5 As New SqlParameter("@NumRif", SqlDbType.Int)
        Dim q6 As New SqlParameter("@Tipo", SqlDbType.VarChar)
        Dim q7 As New SqlParameter("@NumRif", SqlDbType.Int)

        q3.Value = DCGNUMRIF
        q4.Value = "FF"
        Command.Parameters.Clear()
        Command.Parameters.Add(q3)
        Command.Parameters.Add(q4)
        CmKey = Command.ExecuteScalar()
        If CmKey > 0 Then GoTo IIFine
        Command = New SqlCommand(TrovaXF, cnDb)
        q5.Value = DCGNUMRIF
        Command.Parameters.Clear()
        Command.Parameters.Add(q5)
        DCGNUMRIF = Command.ExecuteScalar()
        If DCGNUMRIF <= 0 Then GoTo IIFine
        Command = New SqlCommand(TrovaKey, cnVd)
        q7.Value = DCGNUMRIF
        q6.Value = "XF"
        Command.Parameters.Clear()
        Command.Parameters.Add(q7)
        Command.Parameters.Add(q6)
        CmKey = Command.ExecuteScalar()
IIFine:
        Return CmKey
    End Function
    Sub LancioCDCMONDO()
        If Causale = 2 Then AggiornaDcg()
        Dim TbUFa As New DataTable
        Dim DaUFa As New SqlDataAdapter("exec XDADCG @ID = " & ProgId, CnDc)
        DaUFa.Fill(TbUFa)
        If TbUFa.Rows.Count = 0 Then
            EliminaMcc()
            Exit Sub
        End If
        Dim Gesterna As New MONDOSiglaC
        MONDOSiglaC.PTbUFa = TbUFa
        MONDOSiglaC.PLastTbUFa = LastTbUFa
        MONDOSiglaC.PSigla = CheckedComboBoxEdit1
        MONDOSiglaC.PLocat = New Point(GroupControl19.Location.X, Me.Size.Height)
        MONDOSiglaC.PRet = False
        Gesterna.ShowDialog()
    End Sub
    Sub LancioLDP()
        ''If Causale = 2 Then AggiornaDcg()
        Dim TbUFa As New DataTable
        Dim DaUFa As New SqlDataAdapter("exec XDADCG @ID = " & ProgId, CnDc)
        DaUFa.Fill(TbUFa)
        If TbUFa.Rows.Count = 0 Then
            EliminaMcc()
            Exit Sub
        End If
        Dim Gesterna As New LDPSigla
        LDPSigla.PTbUFa = TbUFa
        LDPSigla.PLastTbUFa = LastTbUFa
        LDPSigla.PSigla = CheckedComboBoxEdit1
        LDPSigla.PLocat = New Point(GroupControl19.Location.X, Me.Size.Height)
        LDPSigla.PRet = False
        Gesterna.ShowDialog()
    End Sub

    Function ReverseCharge() As String
        Dim P, Up As Int16
        Dim RwKey As DataRow = Nothing
        LeggiUltimo(DateEdit1.EditValue)
        Up = 0
        ReverseCharge = IvaCpt
        For P = 1 To DsReg.Tables(Ri).Rows.Count
            RwKey = DsReg.Tables(Ri).Rows(P - 1)
            If RwReg("RIvaAutoFcee") = RwKey("RivaNReg") Then
                Up = RwKey("ProtCar")
                ReverseCharge = RwKey("RivaCpt")
                Exit For
            End If
        Next
        Up = Up + 1
        RwKey("ProtCar") = Up
        For P = 1 To DsFcf.Tables(Ft).Rows.Count
            RwFRI = DsFcf.Tables(Ft).Rows(P - 1)
            RwFRI("PriCausale") = 1
            RwFRI("PriId") = ProgId
            RwFRI("PriCoDare") = RwReg("RIvaCliCee")
            RwFRI("PriCoAvere") = RwReg("RIvaCptCee")
            RwFRI("PriRegIva") = RwReg("RIvaAutoFcee")
            RwFRI("PriDesc") = Mid(Trim(TextEdit6.Text), 1, 24)
            RwFRI("PriNumProt") = Up
            RwFRI("PriFl08") = 0
            RwFRI("PriFl04") = 0
        Next
        RivalsaoCee = True
    End Function
    Function RicaricaCee() As String
        Dim P, Up As Int16
        Dim RwKey As DataRow = Nothing
        LeggiUltimo(DateEdit1.EditValue)
        Up = 0
        RicaricaCee = IvaCpt
        For P = 1 To DsReg.Tables(Ri).Rows.Count
            RwKey = DsReg.Tables(Ri).Rows(P - 1)
            If RwReg("RIvaAutoFcee") = RwKey("RivaNReg") Then
                Up = RwKey("ProtCar")
                RicaricaCee = RwKey("RivaCpt")
                Exit For
            End If
        Next
        Up = Up + 1
        RwKey("ProtCar") = Up
        For P = 1 To DsFcf.Tables(Ft).Rows.Count
            RwFRI = DsFcf.Tables(Ft).Rows(P - 1)
            RwFRI("PriCausale") = 1
            RwFRI("PriId") = ProgId
            RwFRI("PriCoDare") = RwReg("RIvaCliCee")
            RwFRI("PriCoAvere") = RwReg("RIvaCptCee")
            RwFRI("PriRegIva") = RwReg("RIvaAutoFcee")
            RwFRI("PriDesc") = Mid(Trim(TextEdit6.Text), 1, 24)
            RwFRI("PriNumProt") = Up
            RwFRI("PriFl08") = 0
            RwFRI("PriFl04") = 0
        Next
        RivalsaoCee = True
        GroupControl17.Visible = False
    End Function
    Private Sub ButtonFF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF5.Click
        Pulizia(0)
        ResetNumBox()
        BolloIva()
    End Sub
    Private Sub ButtonFF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF8.Click
        If R = 20 Then
            TextEdit20.EditValue = Query.CercaPia()
            SelectNextControl(TextEdit20, True, True, True, True)
            Exit Sub
        End If
    End Sub

    Private Sub InFtCF_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        e.Cancel = GroupControl7.Enabled
    End Sub

    Private Sub CheckEdit1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckEdit1.CheckedChanged
        If Sw = 0 Then Exit Sub
        If DS = 2 Then TextEdit19.Focus() : Exit Sub
        If DS = 3 Then TextEdit20.Focus() : Exit Sub
        If DS = 4 Then ButtonF11.Focus() : Exit Sub
        TextEdit16.Focus()
    End Sub
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub

    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If iset > -1 And ButtonF11.Enabled = True Then
            RwX = GridView1.GetDataRow(iset)
            DaMouse()
        End If
    End Sub
    Sub DaMouse()
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
            If GroupControl7.Enabled = False Then
                ButtonF5.PerformClick()
            Else
                ButtonFF5.PerformClick()
            End If
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            If GroupControl7.Enabled = False Then
                ButtonF8.PerformClick()
            Else
                ButtonFF8.PerformClick()
            End If
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If OkProt = False Then Exit Sub
        If OkMondo = True Then
            If ControlloMondo() = False Then Exit Sub
        End If
        If OkOttica = True Then
            If ControlloOttica() = False Then Exit Sub
        End If
        Messaggio(2, "ELIMINO IL PROT N. " & Val(TextEdit1.EditValue) & TextEdit2.EditValue & " ? ")
        If Rispondi = MsgBoxResult.Yes Then
            EliminaProt()
            If RwReg("RIvaFteP") = True Then EliminaRegistraFteP()
            If OKCDC = True Then EliminaMcc()
            EliminaScad()
            ControlloCespERit()
            RileggoUltimi()
            ButtonF5.PerformClick()
        End If
    End Sub
    Sub ControlloCespERit()
        Dim x, Sr, Sc As Int16
        Dim IvaPScadenze As Decimal
        IvaPScadenze = 0 : Sr = 0 : Sc = 0
        For x = 1 To DsFat.Tables(Fa).Rows.Count
            RwFRI = DsFat.Tables(Fa).Rows(x - 1)
            IvaPScadenze = IvaPScadenze + RwFRI("PriImpAvere")
        Next
        If OLDCSP = 1 Then LancioCespiti()
        If OLDRIT = 1 Or Fl088 = True Then LancioRitenute(IvaPScadenze)
    End Sub
    Sub EliminaMcc()
        Dim Cancella As String = "Delete from TbMcc where MCCPRKID = " & ProgId
        Dim Dmd As New SqlCommand(Cancella, CnDc)
        Dmd.ExecuteNonQuery()
    End Sub

    Private Sub ButtonEXP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEXP.Click
        VISUALCDC()
    End Sub
    Sub VISUALCDC()
        Dim TbUFa As New DataTable
        Dim DaUFa As New SqlDataAdapter("exec XDADCG @ID = " & ProgId, CnDc)
        DaUFa.Fill(TbUFa)
        If TbUFa.Rows.Count = 0 Then Exit Sub
        If CheckedComboBoxEdit1.EditValue IsNot Nothing AndAlso CheckedComboBoxEdit1.EditValue.ToString.Length > 0 Then
            If OkLDP = True Then
                Dim Gesterna As New LDPPop
                LDPPop.PTbUFa = TbUFa
                LDPPop.PRet = False
                Gesterna.ShowDialog()
            Else
                Dim Gesterna As New MONDOPOP
                MONDOPOP.PTbUFa = TbUFa
                MONDOPOP.PRet = False
                Gesterna.ShowDialog()
            End If
        End If
    End Sub
    Private Sub DateEdit2_Enter(sender As Object, e As System.EventArgs) Handles DateEdit2.Enter
        DateEdit2.SelectAll()
    End Sub
#Region "VISUALIZZAZIONE PASSIVE"
    Public Function MonitorIdoneo() As Boolean
        Dim desktopSize As Size
        desktopSize = System.Windows.Forms.SystemInformation.PrimaryMonitorSize
        Return desktopSize.Width >= 1600
    End Function
    Private Sub CheckEdit2_DockChanged(sender As Object, e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        Visualizzazione_dettagli(CheckEdit2.Checked)
    End Sub
    Private Sub Visualizzazione_dettagli(tipo As Boolean)
        If tipo = False Or MonitorOk = False Then
            'NORMALE
            Dim X As Integer
            Dim Y As Integer
            X = (Me.Width - 1008) / 2
            Y = (Me.Height - 656) / 2
            XtraTabControl1.Location = New Point(X, Y)
            XtraTabControl1.Anchor = AnchorStyles.None
            XtraTabControl1.Dock = DockStyle.None
            SPCC.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1
        Else
            ' ESTESO
            XtraTabControl1.Dock = DockStyle.Fill
            SPCC.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both
            SPCC.SplitterPosition = 1000
            IngressoFTE()
        End If
        If MonitorOk = False Then CheckEdit2.Enabled = False
    End Sub
    Private Enum Exec
        OLECMDID_OPTICAL_ZOOM = 63
    End Enum

    Private Enum execOpt
        OLECMDEXECOPT_DODEFAULT = 0
        OLECMDEXECOPT_PROMPTUSER = 1
        OLECMDEXECOPT_DONTPROMPTUSER = 2
        OLECMDEXECOPT_SHOWHELP = 3
    End Enum
    Dim ZOOM As Integer = 70
    Dim RAPP As Decimal
    Dim Url2, Url3 As String
    Dim DirFE, DirLocal, DirPDF, Pathserver, PASSIVE As String
    Dim Inizio As Boolean = True
    Dim esisteAziPa As Boolean
    Dim PortaleTest As Boolean
    Dim TbAllega As DataTable
    Dim ANNO, ANNOP, AnnoInCorso, GGIndietro As Int16
    Dim DaData, AData, DaDataP, ADataP As Date
    Dim LeggiOk As Boolean
    Dim TIPODOCUMENTO As String = ""
    Dim DA(13) As String
    Dim AA(13) As String
    Dim CH(13) As Int16
    Dim Finoal As String = ""
    Dim AZ_PARTITAIVA As String = ""
    Dim FO_PARTITAIVA As String = ""
    Dim FO_CODICEFISCALE As String = ""
    Dim FO_DENOMINAZIONE As String = ""
    Dim FO_COGNOME As String = ""
    Dim FO_NOME As String = ""
    Dim FO_INDIRIZZO As String = ""
    Dim FO_CAP As String = ""
    Dim FO_COMUNE As String = ""
    Dim FO_PROVINCIA As String = ""
    Dim FO_TELEFONO As String = ""
    Dim FO_FAX As String = ""
    Dim FO_EMAIL As String = ""
    Dim FO_IBAN As String = ""

    Sub IngressoFTE()
        If LeggiOk = False Then
            AnnoInCorso = ComboBoxEdit1.EditValue
            LeggiTabelle()
            Inizializza()
            LeggiOk = True
        End If
        PulisciBrowser()
        VisualizzaPassive()
    End Sub
    Private Sub LeggiTabelle()
        'Tabella TbSel
        Cmd = New SqlCommand("Select * from TbSel where SelId = 1", cnVd)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            DirLocal = dataRd.Item("Sel8")
            If DirLocal.Contains("tsclient") Then
                DirLocal = "C:\" & dataRd.Item("Sel11")
            End If
            DirPDF = dataRd.Item("Sel12") & dataRd.Item("Sel11")
            Pathserver = dataRd.Item("Sel12")
        End If
        dataRd.Close()
        'Tabella AziPa
        Cmd = New SqlCommand("Select top 1 * from TbAziPa", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            DirFE = dataRd.Item("APCartellaINVIO")
            PortaleTest = dataRd.Item("APPortaleTest")
            If GGIndietro = 0 Then
                GGIndietro = dataRd.Item("APGGIndietro")
            End If
            esisteAziPa = True
        Else
            esisteAziPa = False
            PortaleTest = True
            GGIndietro = 5
            DirFE = ""
        End If
        dataRd.Close()
        Cmd = New SqlCommand("Select Top 1 AnaPiva from TbAna where anagrp='AZ'", cnVd)
        AZ_PARTITAIVA = Cmd.ExecuteScalar
        ButtonREC.Enabled = False
        RepositoryItemImageComboBox4.Items.Clear()

        Cmd = New SqlCommand(" SELECT * from TbCod_fe where TIPO='P' Order by COD", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("COD") & " " & dataRd.Item("DESCRIZIONE"), dataRd.Item("COD"), -1)
            RepositoryItemImageComboBox4.Items.Add(nn)
        End While
        dataRd.Close()


        PASSIVE = DirFE & "PASSIVE\"

        'TABELLA ANNI PASSIVE
        ComboBoxEdit3.Properties.Items.Clear()
        Cmd = New SqlCommand("Select DISTINCT FteAnno from TbFte_Passiva order by FteAnno DESC", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit3.Properties.Items.Add(dataRd.Item("FteAnno"))
        End While
        dataRd.Close()

        Inizio = True
        If ComboBoxEdit3.Properties.Items.Count > 0 Then
            ComboBoxEdit3.SelectedIndex = 0
        End If
        Inizio = False
        RadioGroup8.SelectedIndex = 0
        CambioTipo()
    End Sub
    Sub CambioTipo()
        ANNOP = ComboBoxEdit3.EditValue
        Dim STR As String = "EXEC XLEGGIFTEREG @ANNO=" & ANNOP
        Dim TBDATE As DataTable
        Dim DADATE As SqlDataAdapter
        Dim RwDate As DataRow
        TBDATE = New DataTable
        DADATE = New SqlDataAdapter(STR, cnCo)
        DADATE.Fill(TBDATE)
        ImageComboBoxEdit5.Properties.Items.Clear()
        Dim X As Int16 = -1
        For I As Int16 = 1 To TBDATE.Rows.Count
            RwDate = TBDATE.Rows(I - 1)
            If RwDate("FePTipo") = RadioGroup7.EditValue Then
                nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(RwDate("FePPeriodo"), I - 1, RwDate("FePImage"))
                ImageComboBoxEdit5.Properties.Items.Add(nn)
                X += 1
                DA(X) = RwDate("FePDal")
                AA(X) = RwDate("FePAl")
                CH(X) = RwDate("FePCh")
            End If
        Next
        ImageComboBoxEdit5.SelectedIndex = 0
        DaDataP = DA(0) : ADataP = AA(0)
    End Sub
    Private Sub ImageComboBoxEdit5_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ImageComboBoxEdit5.SelectedIndexChanged
        DaDataP = DA(ImageComboBoxEdit5.SelectedIndex)
        ADataP = AA(ImageComboBoxEdit5.SelectedIndex)

        VisualizzaPassive()
    End Sub
    Private Sub Inizializza()
        If esisteAziPa = False Then
            Exit Sub
        End If
        If Directory.Exists(PASSIVE) = False Then
            Directory.CreateDirectory(PASSIVE)
        End If
    End Sub
    Private Sub PulisciBrowser()
        '' RAPP = WebBrowser2.Height / 860
        '' ZOOM = 100 * RAPP

        ZoomT.EditValue = ZOOM
        Url2 = "about:blank"
        WebBrowser2.Navigate(Url2)

        GroupControl37.Text = "ALLEGATI"
        ErrorProvider1.SetError(GroupControl37, "")

        Dim FileOutput As String = DirLocal & "TMPXMLP.xml"
        File.Delete(FileOutput)

        GridControl2.DataSource = Nothing
        GridControl3.DataSource = Nothing
        GridControl4.DataSource = Nothing
        GridControl5.DataSource = Nothing
        GridControl6.DataSource = Nothing
        PanelControl6.Visible = False : PanelControl5.Visible = False : RIFERFTEP = 0
    End Sub
    Private Sub VisualizzaPassive()
        'ESCO SE NON ESISTE AZIPA
        If esisteAziPa = False Then
            Exit Sub
        End If

        'ESCO SE NON ESISTONO DATI
        If ComboBoxEdit3.SelectedIndex = -1 Then
            Exit Sub
        End If

        ImageComboBoxEdit1.Properties.Items.Clear()

        Dim DADATA As Date = Today
        Dim AADATA As Date = Today

        Dim leggi As String = "SELECT *,TipoStampa= case when FtePrintM=0 and FtePrintA=0 then '' when  FtePrintM=1 and FtePrintA=0 then 'M' when FtePrintM=0 and FtePrintA=1 then 'A' else 'AM' end, " _
                  & "Registrata= case when FteRifPri > 0 then CAST(1 as bit) else cast(0 as bit) end FROM  TbFte_passiva WHERE FteAnno = @ANNO AND FtePartiva <> @AZ_PIVA "
        Dim Selec As String = "AND FteDataRicezione BETWEEN @DA_DATA AND @A_DATA and FteRifPri <=0 "


        If RadioGroup7.SelectedIndex = 0 Then
            'If ImageComboBoxEdit5.SelectedIndex = 0 Then
            '    GoTo POWER
            'End If
            'If CH(ImageComboBoxEdit5.SelectedIndex) > 0 Then
            '    DADATA = "01/" & CH(ImageComboBoxEdit5.SelectedIndex).ToString.PadLeft(2, "0") & "/" & ANNOP
            '    AADATA = DADATA.AddMonths(1)
            '    AADATA = AADATA.AddDays(-1)
            '    Selec &= " AND FteData between '" & DADATA & "' AND '" & AADATA & "'"
            '    GoTo POWER
            'End If
POWER:
            Selec &= "ORDER BY FteDataConsegnaSdi desc,FteDataRicezione desc"
            GoTo LABELINIT
        End If

        If RadioGroup7.SelectedIndex = 1 Then
            Selec = "AND FteRifPri >0 "
            Selec &= " AND FteDataRicezione BETWEEN @DA_DATA AND @A_DATA "
            '    GoTo ORDER
            'End If

            'If CH(ImageComboBoxEdit5.SelectedIndex) = 1 Then
            '    Selec &= " AND FteFinoAl = @A_DATA "
            '    GoTo ORDER
            'End If

            'If CH(ImageComboBoxEdit5.SelectedIndex) = 0 Then
            '    Selec &= " AND FteDataRicezione BETWEEN @DA_DATA AND @A_DATA AND FteFinoAl IS NOT NULL "
            '    GoTo ORDER
            'End If
ORDER:
            Selec &= "ORDER BY FteDataConsegnaSdi desc,FteDataRicezione desc"
        End If

LABELINIT:
        leggi &= Selec

        Dim P1 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim P2 As New SqlParameter("@DA_DATA", SqlDbType.SmallDateTime)
        Dim P3 As New SqlParameter("@A_DATA", SqlDbType.SmallDateTime)
        Dim P4 As New SqlParameter("@AZ_PIVA", SqlDbType.VarChar)

        P1.Value = ANNOP
        P2.Value = DaDataP
        P3.Value = ADataP
        P4.Value = AZ_PARTITAIVA

        DaFte = New SqlDataAdapter(leggi, cnDb)
        DaFte.SelectCommand.Parameters.Add(P1)
        DaFte.SelectCommand.Parameters.Add(P2)
        DaFte.SelectCommand.Parameters.Add(P3)
        DaFte.SelectCommand.Parameters.Add(P4)
        TbFte = New DataTable("FTEP")

        DaFte.Fill(TbFte)
        If RadioGroup7.SelectedIndex = 0 Then
            GridColumn34.Visible = False
        Else
            GridColumn34.VisibleIndex = 7
            GridColumn34.Visible = True
        End If
        GridControl2.DataSource = TbFte
        GridView2.ClearSelection()
        If RadioGroup7.SelectedIndex = 0 AndAlso TbFte IsNot Nothing AndAlso TbFte.Rows.Count > 0 Then ButtonREC.Enabled = True Else ButtonREC.Enabled = False
    End Sub

    Private Sub GridView2_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView2.RowClick
        RIFERFTEP = 0
        VerificaAllegati(GridView2.GetFocusedDataRow)
        VisualizzaFattura(GridView2.GetFocusedDataRow)
        LeggiDatiXml()
        If RadioGroup7.SelectedIndex = 0 Then
            ControllaProtocollo(0)
            CaricaDatiDaSia(GridView2.GetFocusedDataRow)
        Else
            CaricaDatiDaTbPri(GridView2.GetFocusedDataRow)
        End If
    End Sub
    Sub CaricaDatiDaTbPri(rw)
        RIFERFTEP = 0
        Dim t1 As String = ""
        Dim t2 As String = ""

        Cmd = New SqlCommand("select PriNumProt,PriBisRet,PriRegIva from TbPri where PriId=" & rw("FteRifPri") & " and Pricausale=3", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            t1 = dataRd.Item("PriNumProt")
            t2 = dataRd.Item("PriBisRet")
            NumReg = dataRd.Item("PriRegIva")
        End While
        dataRd.Close()
        ImageComboBoxEdit2.EditValue = NumReg.ToString.PadLeft(2, "0")
        TextEdit1.EditValue = t1
        TextEdit2.EditValue = t2
        If ControllaProtocollo(TextEdit1.EditValue) = True Then
            errorT(0) = TextEdit1.ErrorText
            errorT(1) = DateEdit1.ErrorText
            SelectNextControl(TextEdit4, True, True, True, True)
            TextEdit1.ErrorText = errorT(0)
            DateEdit1.ErrorText = errorT(1)
        End If
    End Sub
    Sub CaricaDatiDaSia(rw)
OP:
        Dim QP As Int16 = 0
        REM da fornitore completare se esiste + volte
        Cmd = New SqlCommand("select count(*) from TbAna where Anagrp='FO' and AnaPiva='" & rw("FtePartiva") & "'", cnVd)
        QP = Cmd.ExecuteScalar
        If QP = 0 Then
            Messaggio(2, "PARTITA IVA NON IN ARCHIVIO '" & rw("FtePartiva") & "' INSERISCO NUOVO FORNITORE ?")
            If Rispondi = MsgBoxResult.No Then
                IngressoFTE()
                Exit Sub
            Else
                CaricaNuovoFornitore()
                GoTo OP
            End If
        End If
        If QP > 1 Then
            TextEdit5.EditValue = RitornoPivaPlus(rw("FtePartiva"))
            If Val(TextEdit5.EditValue) > MiglioFo Then GoTo II
            IngressoFTE()
            Exit Sub
        End If
        Cmd = New SqlCommand("select Anacod from TbAna where Anagrp='FO' and AnaPiva='" & rw("FtePartiva") & "'", cnVd)
        TextEdit5.EditValue = Cmd.ExecuteScalar
II:
        Dim NumeroAlfa As String = rw("FteNumero")
        DateEdit1.EditValue = IIf(rw("FteDataConsegnaSdi") Is DBNull.Value, rw("FteDataRicezione"), rw("FteDataConsegnaSdi"))
        DateEdit2.EditValue = rw("FteData")
        NumeroAlfa = EstraiNumeri(rw("FteNumero"))
        If rw("FtePartiva") = "04394270013" Then NumeroAlfa = Mid(NumeroAlfa, 1, Len(NumeroAlfa) - 1)
        If Len(NumeroAlfa) > 6 Then NumeroAlfa = Mid(NumeroAlfa, Len(NumeroAlfa) - 5, 6)
        TextEdit3.EditValue = NumeroAlfa
        TextEdit13.EditValue = CDec(rw("FteTotFat"))
        Pcod = TextEdit5.EditValue
        Anagrafica()
        RIFERFTEP = rw("FteRif")
    End Sub
    Sub CaricaNuovoFornitore()
        FRMFO = New InsFoFte
        InsFoFte.NPARTITAIVA = FO_PARTITAIVA
        InsFoFte.NCODICEFISCALE = FO_CODICEFISCALE
        InsFoFte.NDENOMINAZIONE = FO_DENOMINAZIONE
        InsFoFte.NCOGNOME = FO_COGNOME
        InsFoFte.NNOME = FO_NOME
        InsFoFte.NINDIRIZZO = FO_INDIRIZZO
        InsFoFte.NCAP = FO_CAP
        InsFoFte.NCOMUNE = FO_COMUNE
        InsFoFte.NPROVINCIA = FO_PROVINCIA
        InsFoFte.NTELEFONO = FO_TELEFONO
        InsFoFte.NFAX = FO_FAX
        InsFoFte.NEMAIL = FO_EMAIL
        InsFoFte.NIBAN = FO_IBAN
        FRMFO.ShowDialog()
    End Sub
    Function RitornoPivaPlus(AnaPiva) As String
        Dim Tb As DataTable
        Dim ad As SqlDataAdapter

        Cmd = New SqlCommand("select * from TbAna where Anagrp='FO' and AnaPiva='" & AnaPiva & "'", cnVd)

        Tb = New DataTable("TbAna")
        ad = New SqlDataAdapter(Cmd)
        ad.Fill(Tb)

        Dim fricclf As New DupPiva
        fricclf.Location = New Point(Me.Location.X, Me.Location.Y)
        fricclf._DataTable = Tb
        fricclf.ShowDialog()
        RitornoPivaPlus = Val(fricclf.codice)
        ad.Dispose()
    End Function
    Public Function EstraiNumeri(str As String) As String
        Dim temp As String = ""
        For k As Int16 = 1 To str.Length
            If IsNumeric(Mid(str, k, 1)) Then temp &= Mid(str, k, 1)
        Next k
        Return temp
    End Function
    Private Sub LeggiDatiXml()
        Dim ds As New DataSet
        Dim dt As New DataTable()
        Dim RwTipo As DataRow
        GridControl3.DataSource = Nothing
        GridControl4.DataSource = Nothing
        GridControl5.DataSource = Nothing
        GridControl6.DataSource = Nothing
        ds.ReadXml(DirLocal & "TMPXMLP.xml")
        PanelControl5.Visible = False : PanelControl6.Visible = False

        FO_PARTITAIVA = "" : FO_CODICEFISCALE = "" : FO_DENOMINAZIONE = "" : FO_COGNOME = "" : FO_NOME = "" : FO_INDIRIZZO = "" : FO_CAP = "" : FO_COMUNE = ""
        FO_PROVINCIA = "" : FO_TELEFONO = "" : FO_FAX = "" : FO_EMAIL = "" : FO_IBAN = ""


        If ds.Tables("DatiGeneraliDocumento") IsNot Nothing Then
            RwTipo = ds.Tables("DatiGeneraliDocumento").Rows(0)
            TIPODOCUMENTO = RwTipo("TipoDocumento")
        End If
        If ds.Tables("DatiCassaPrevidenziale") IsNot Nothing Then
            GridControl6.DataSource = ds.Tables("DatiCassaPrevidenziale") : PanelControl6.Visible = True
        End If
        If ds.Tables("DatiRitenuta") IsNot Nothing Then
            GridControl5.DataSource = ds.Tables("DatiRitenuta") : PanelControl5.Visible = True
        End If
        If ds.Tables("DettaglioPagamento") IsNot Nothing Then
            GridControl4.DataSource = ds.Tables("DettaglioPagamento")
            Rw = ds.Tables("DettaglioPagamento").Rows(0)
            Try
                FO_IBAN = Rw("IBAN")
            Catch ex As Exception
                FO_IBAN = ""
            End Try
        Else
            GridControl4.DataSource = Nothing
        End If
        If Len(FO_IBAN) <> 27 Then FO_IBAN = ""
        If ds.Tables("DatiRiepilogo") IsNot Nothing Then GridControl3.DataSource = ds.Tables("DatiRiepilogo") Else GridControl3.DataSource = Nothing

        Dim Obj As XmlDocument
        Obj = New XmlDocument
        Obj.Load(DirLocal & "TMPXMLP.xml")

        Dim RIE As Xml.XmlNodeList
        Dim ECC As Xml.XmlNode
        Dim nodes As XmlNodeList
        RIE = Obj.DocumentElement.SelectNodes("FatturaElettronicaHeader/DatiTrasmissione")
        Dim DAT As System.Xml.XmlElement = RIE.Item(0)
        DAT = DirectCast(RIE.Item(0), System.Xml.XmlElement)
        For Each DAT In RIE
            ECC = DAT.NextSibling
            Exit For
        Next
        If DAT IsNot Nothing Then
            DAT = ECC.ChildNodes(0)
            nodes = DAT.GetElementsByTagName("IdCodice")
            If nodes.Count > 0 Then FO_PARTITAIVA = nodes(0).InnerText Else FO_PARTITAIVA = ""
            nodes = DAT.GetElementsByTagName("CodiceFiscale") '' CodiceFiscale controllare se codice fiscale dell'azienda
            If nodes.Count > 0 Then FO_CODICEFISCALE = nodes(0).InnerText Else FO_CODICEFISCALE = ""
            nodes = DAT.GetElementsByTagName("Denominazione") '' Denominazione
            If nodes.Count > 0 Then FO_DENOMINAZIONE = nodes(0).InnerText Else FO_DENOMINAZIONE = ""
            nodes = DAT.GetElementsByTagName("Cognome") '' Cognome
            If nodes.Count > 0 Then FO_COGNOME = nodes(0).InnerText Else FO_COGNOME = ""
            nodes = DAT.GetElementsByTagName("Nome") '' Nome
            If nodes.Count > 0 Then FO_NOME = nodes(0).InnerText Else FO_NOME = ""
        End If
        DAT = ECC.ChildNodes(1)
        If DAT IsNot Nothing Then
            nodes = DAT.GetElementsByTagName("Indirizzo") '' Indirizzo
            If nodes.Count > 0 Then FO_INDIRIZZO = nodes(0).InnerText Else FO_INDIRIZZO = ""
            nodes = DAT.GetElementsByTagName("NumeroCivico") '' Numero Civico
            If nodes.Count > 0 And FO_INDIRIZZO > "" Then FO_INDIRIZZO = FO_INDIRIZZO & " " & nodes(0).InnerText
            nodes = DAT.GetElementsByTagName("CAP") '' CAP
            If nodes.Count > 0 Then FO_CAP = nodes(0).InnerText Else FO_CAP = ""
            nodes = DAT.GetElementsByTagName("Comune") '' COMUNE
            If nodes.Count > 0 Then FO_COMUNE = nodes(0).InnerText Else FO_COMUNE = ""
            nodes = DAT.GetElementsByTagName("Provincia") '' Provincia
            If nodes.Count > 0 Then FO_PROVINCIA = nodes(0).InnerText Else FO_PROVINCIA = ""
        End If
        DAT = ECC.ChildNodes(3)
        If DAT IsNot Nothing Then
            nodes = DAT.GetElementsByTagName("Telefono") '' TELEFONO
            If nodes.Count > 0 Then FO_TELEFONO = nodes(0).InnerText Else FO_TELEFONO = ""
            nodes = DAT.GetElementsByTagName("Fax") '' FAX
            If nodes.Count > 0 Then FO_FAX = nodes(0).InnerText Else FO_FAX = ""
            nodes = DAT.GetElementsByTagName("Email") '' EMAIL
            If nodes.Count > 0 Then FO_EMAIL = nodes(0).InnerText Else FO_EMAIL = ""
        End If
    End Sub

    Private Sub VisualizzaFattura(rw As DataRow)
        Dim FileInput As String = PASSIVE & rw("FteNomeFile")
        Dim FileOutput As String = DirLocal & "TMPXMLP.xml"

        Dim FoglioStile As String = RadioGroup8.EditValue
        Url2 = DirLocal & "TMP.htm"

        If File.Exists(Url2) Then
            File.Delete(Url2)
        End If
        XmlTrasforma(FileOutput, FoglioStile, Url2)

        WebBrowser2.Navigate(Url2)
    End Sub
    Private Sub VerificaAllegati(rw As DataRow)
        Dim FileInput As String = PASSIVE & rw("FteNomeFile")
        Dim FileOutput As String = DirLocal & "TMPXMLP.xml"


        File.Delete(FileOutput)

        Dim F As New FileInfo(FileInput)


        If F.Extension.ToLower = ".p7m" Then
            Dim pippo As String = convertP7M(FileInput, FileOutput)
        Else
            File.Copy(FileInput, FileOutput)
        End If

        RimuoviFirma(FileOutput)

        Dim DatAll As New DataSet
        DatAll.ReadXml(FileOutput)

        If IsNothing(TbAllega) = False Then
            TbAllega.Dispose()
        End If

        GroupControl37.Text = "ALLEGATI"
        ErrorProvider1.SetError(GroupControl37, "")

        ImageComboBoxEdit1.Properties.Items.Clear()
        If IsNothing(DatAll.Tables("Allegati")) Then
            Exit Sub
        End If

        TbAllega = New DataTable
        TbAllega = DatAll.Tables("Allegati")

        For I As Int16 = 0 To TbAllega.Rows.Count - 1
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(TbAllega.Rows(0)("NomeAttachment"), I, -1)
            ImageComboBoxEdit1.Properties.Items.Add(nn)
        Next

        GroupControl37.Text = "ALLEGATI ( " & TbAllega.Rows.Count & " )"
        ErrorProvider1.SetError(GroupControl37, "SONO PRESENTI ALLEGATI !!!")
    End Sub
    Private Sub WebBrowser2_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles WebBrowser2.Navigated
        Try
            Dim Res As Object = Nothing
            Dim MyWeb As Object
            MyWeb = Me.WebBrowser2.ActiveXInstance
            MyWeb.ExecWB(Exec.OLECMDID_OPTICAL_ZOOM, execOpt.OLECMDEXECOPT_PROMPTUSER, CObj(ZoomT.EditValue), CObj(IntPtr.Zero))
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ZoomT_EditValueChanged(sender As Object, e As System.EventArgs) Handles ZoomT.EditValueChanged
        WebBrowser2.Navigate(Url2)
    End Sub

    Private Sub Radiogroup8_EditValueChanged(sender As Object, e As System.EventArgs) Handles RadioGroup8.EditValueChanged
        Try
            VisualizzaFattura(GridView2.GetFocusedDataRow)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonF9_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF9.Click
        StampaXml()
    End Sub

    Private Sub StampaXml()
        WebBrowser2.ShowPrintDialog()
        AggiornaDatiStampa()
    End Sub

    Private Sub ImageComboBoxEdit1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ImageComboBoxEdit1.SelectedIndexChanged
        If ImageComboBoxEdit1.SelectedIndex = -1 Then
            Exit Sub
        End If

        Try
            VisualizzaAllegato(ImageComboBoxEdit1.EditValue)
        Catch ex As Exception
            Try
                VisualizzaAllegato1(ImageComboBoxEdit1.EditValue)
            Catch ex1 As Exception

            End Try
        End Try
    End Sub

    Private Sub VisualizzaAllegato(i As Integer)
        Dim FileZip As String = DirLocal & "TMP_COMPR." & TbAllega.Rows(i)("AlgoritmoCompressione")

        Dim TEMP As String = DirLocal & "TMPZIP\"

        SvuotaDir(TEMP)

        If File.Exists(FileZip) Then
            File.Delete(FileZip)
        End If

        Dim data() As Byte = System.Convert.FromBase64String(TbAllega.Rows(i)("Attachment"))

        File.WriteAllBytes(FileZip, data)

        Dim at As New FileInfo(TbAllega.Rows(i)("NomeAttachment"))

        Dim NomeAtt As String = at.Name.Replace(at.Extension, "." + TbAllega.Rows(i)("FormatoAttachment"))

        EstraiDaZip(FileZip, TEMP, NomeAtt)

        Process.Start(TEMP & NomeAtt)

        ImageComboBoxEdit1.SelectedIndex = -1
    End Sub

    Private Sub VisualizzaAllegato1(i As Integer)
        'CASO ALLEGATO NON COMPRESSO
        Dim TEMP As String = DirLocal & "TMPZIP\"
        Dim NomeAtt As String = TbAllega.Rows(i)("NomeAttachment")

        SvuotaDir(TEMP)

        Dim data() As Byte = System.Convert.FromBase64String(TbAllega.Rows(i)("Attachment"))

        File.WriteAllBytes(TEMP & NomeAtt, data)

        Process.Start(TEMP & NomeAtt)

        ImageComboBoxEdit1.SelectedIndex = -1
    End Sub

    Sub AggiornaDatiStampa()
        Try
            Rw = GridView2.GetFocusedDataRow
            Dim Rif As Integer = Rw("FteRif")
            Dim TS(1) As String
            TS(0) = "Update TbFte_Passiva set FtePrintA=1 where FteRif=" & Rif
            TS(1) = "Update TbFte_Passiva set FtePrintM=1 where FteRif=" & Rif
            Cmd = New SqlCommand(TS(RadioGroup8.SelectedIndex), cnDb)
            Cmd.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        VisualizzaPassive()
    End Sub
    Private Sub ComboBoxEdit3_EditValueChanged(sender As Object, e As System.EventArgs) Handles ComboBoxEdit3.EditValueChanged, RadioGroup7.SelectedIndexChanged
        If Inizio = True Then
            Exit Sub
        End If
        ButtonF5.PerformClick()
        CambioTipo()
        VisualizzaPassive()
    End Sub
    Private Sub ButtonView_Click(sender As System.Object, e As System.EventArgs) Handles ButtonView.Click
        SplitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel2
    End Sub
    Private Sub ButtonClose_Click(sender As System.Object, e As System.EventArgs) Handles ButtonClose.Click
        SplitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1
    End Sub
    Private Sub ButtonREC_Click(sender As System.Object, e As System.EventArgs) Handles ButtonREC.Click
        Recupera()
        PulisciBrowser()
        VisualizzaPassive()
    End Sub
    Sub Recupera()
        Cursor.Current = Cursors.WaitCursor
        EsegueSql("Exec RecuperoRegistrazioni", cnDb)
        Cursor.Current = Cursors.Default
    End Sub
#End Region

End Class