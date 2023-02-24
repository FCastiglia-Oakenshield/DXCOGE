Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports NCDCO
Imports DevExpress.XtraEditors

Public Class DxInPrNo
    Private Shared ERifArt As Integer
    Public Shared Property NRifArt() As Integer
        Get
            Return ERifArt
        End Get

        Set(ByVal Value As Integer)
            ERifArt = Value
        End Set
    End Property
    ' OBBLIGATO DALLA FORZATURA ARTICOLO
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
    Dim SSPLIT As String
    ' OBBLIGATO DALLA FORZATURA ARTICOLO
    Dim Sw As Int16 = 0
    Dim Xreg As Int16 = -1
    Dim Righe, POSRIG, Irow, MaxEse, OLDCSP, Fl04, Fl05, Fl06, MaxConti As Int16
    Dim EseDal(5), EseAl(5), MaxDat, MinDat, MMMDat As Date
    Dim X0, X1, TotTransito, TotaleResiduo As Decimal
    Dim OkPn, OkFat, OkFcf, OkArt, OkQuadra, NOadd, PnCompensa As Boolean
    Dim ProgId, MaxArt, AP, MiglioFo, ArtIrpef As Int32
    Dim ArtTerminale As Int32 = 0
    Dim Rispondi As MsgBoxResult
    Dim CauDes(72), Grp(18) As String
    Dim COBA As String = "00.10"
    Dim Fa As String = "PRNO"
    Dim DsFat As DataSet
    Dim DaFat As SqlDataAdapter
    Dim RwFat As DataRow
    Dim RwFRI As DataRow

    Dim Ft As String = "PSCF"
    Dim DsFcf As DataSet
    Dim DaFcf As SqlDataAdapter
    Dim RwFcf As DataRow

    Dim Pn As String = "NOTA"
    Dim DsPno As DataSet
    Dim RwPno As DataRow
    Dim TCoDare, TCoAvere As Int16
    Dim ContoD, ContoA As String
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim RwX As DataRow

    Dim AnnoOp As Int16
    Dim OkMondo As Boolean = False
    Dim OkCSA As Boolean = False
    Dim OkLDP As Boolean = False
    Dim UserId As String = ""
    Dim Em As New DevExpress.XtraEditors.Controls.CheckedListBoxItem

    Dim LastTbUFa As New DataTable
    Dim LastDaUFa As New SqlDataAdapter
    Dim errorT As String = ""
    Dim CausaleAP As Int16 = 45
    Dim CausaleCh As Int16 = -1

    Private Sub DxInPrNo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Sw = 0 Then
            ButtonF5.PerformClick()
            GRUPPI()
            GestioneUser()
            Sw = 1
        End If
        If ERifArt > 0 Then
            TextEdit1.EditValue = ERifArt
            errorT = TextEdit1.ErrorText
            ControllaArticolo(ERifArt)
            TextEdit1.ErrorText = errorT
            ArtTerminale = ERifArt
            ERifArt = 0

        End If
    End Sub
    ''obbligato dall'inserimento contemporaneo da piu terminali
    Private Sub ButtonF5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        RileggoUltimi()
        TextEdit1.EditValue = ""
        Pulizia(0)
    End Sub
    Sub Pulizia(ByVal p As Int16)
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = ""
        DateEdit1.ErrorText = ""
        TextEdit1.ErrorText = ""
        TextEdit20.ErrorText = ""
        OkPn = False
        OkArt = False
        AbilitaGroup(0)
        OLDCSP = 0 : Fl04 = 0 : Fl05 = 0 : Fl06 = 0
        Righe = 0
        POSRIG = 1
        ProgId = 0
        MaxConti = -1
        NOadd = False
        ButtonEXP.Visible = False
        If OkMondo = True Then ButtonEXP.Visible = OkMondo
        If OkLDP = True Then ButtonEXP.Visible = OkLDP : ButtonEXP.ImageIndex = 5
        If p = 0 Then
            PulisciGrid()
            ResetNumBox()
            CaricaIniziale()
            TextEdit1.Focus()
        End If
    End Sub
    Sub ResetNumBox()
        TextEdit2.EditValue = ""
        TextEdit3.EditValue = ""
        TextEdit15.EditValue = 0
        TextEdit20.EditValue = CDec(0.0)
        TextEdit24.EditValue = CDec(0.0)
        TextEdit16.EditValue = ""
        TextEdit17.EditValue = ""
        TextEdit18.EditValue = ""
        TextEdit19.EditValue = ""
        TextEdit4.EditValue = ""
        ImageComboBoxEdit1.SelectedIndex = -1
        ImageComboBoxEdit2.SelectedIndex = -1
        X1 = 0
        X0 = 0
        TotaleResiduo = 0
        TotTransito = 0
    End Sub
    Sub AbilitaGroup(ByVal n As Int16)
        If n = 0 Then
            TextEdit1.Enabled = True
            DateEdit1.Enabled = True
            DateEdit2.Enabled = True
            ButtonF1.Enabled = True
            'GroupBox4.Enabled = False
            GroupControl3.Enabled = False
            GroupControl5.Enabled = False
            ImageComboBoxEdit1.Enabled = True
            ButtonF11.Enabled = False
        ElseIf n = 1 Then
            TextEdit1.Enabled = False
            DateEdit1.Enabled = False
            DateEdit2.Enabled = False
            ButtonF1.Enabled = False
            ImageComboBoxEdit1.Enabled = False
            'GroupBox4.Enabled = True
            GroupControl3.Enabled = True
            GroupControl5.Enabled = True
            ButtonF11.Enabled = True
        End If
    End Sub
    Sub GRUPPI()
        Dim x, k As Int16
        k = 0
        Cmd = New SqlCommand("SELECT GrpMigl,GrpCpt1, GrpCpt2,GrpCpt3,GrpCpt4,GrpCpt5,GrpCpt6,GrpCpt7,GrpCpt8,GrpCpt9 FROM TbGrp Where GrpCod = 'FO' ", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
            For x = 1 To 9
                If dataRd.Item(x) > "00.00" Then
                    k = k + 1
                    Grp(k) = dataRd.Item(x)
                End If
            Next
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT GrpMigl,GrpCpt1, GrpCpt2,GrpCpt3,GrpCpt4,GrpCpt5,GrpCpt6,GrpCpt7,GrpCpt8,GrpCpt9 FROM TbGrp Where GrpCod = 'CL' ", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            For x = 1 To 9
                If dataRd.Item(x) > "00.00" Then
                    k = k + 1
                    Grp(k) = dataRd.Item(x)
                End If
            Next
        End While
        dataRd.Close()
        Array.Sort(Grp)
        k = 1
        For x = 2 To 18
            If Grp(x) = Grp(k) Then
                Grp(x) = ""
            Else
                k = k + 1
                Grp(k) = Grp(x)
            End If
        Next
        Grp(0) = k
        For x = k + 1 To 18
            Grp(x) = ""
        Next
    End Sub
    Sub PulisciGrid()
        DsFat = New DataSet(Fa)
        GridControl1.DataSource = DsFat.Tables(Fa)
        GridControl1.Refresh()
        GridView1.ClearSelection()
    End Sub
    Sub CaricaIniziale()
        DateEdit1.EditValue = Today
        DateEdit2.EditValue = Today
        'TextEdit1.EditValue = MaxArt + 1
        TextEdit1.Focus()
    End Sub
    Sub RileggoUltimi()
        ''' ' LEGGO DA TBPRI MAX DATAGIO E MAX NUMART
        Dim Cmd As New SqlCommand("Select Top 1 EseCausaleChiusuraConti from Tbese order by Eseanno Desc", cnCo)
        CausaleCh = Cmd.ExecuteScalar
        MaxArt = 0
        If DateEdit1.EditValue Is Nothing Then DateEdit1.EditValue = Today
        MaxDat = DateEdit1.EditValue
        Dim PassData As String = MaxDat.ToShortDateString
        Dim Str As String = ""
        If ArtTerminale = 0 Then
            Str = "SELECT isnull(Max(PridataGio),'" & PassData & "') as PriDataGio,IsNull(Max(PriNumProt),0) as PriNumProt from TbPri where PriregIva = 0"
        Else
            Str = "SELECT distinct isnull(PridataGio,'" & PassData & "') as PriDataGio ,IsNull(PriNumProt,0) as PriNumProt from TbPri where PriregIva = 0 and PriNumProt = " & ArtTerminale
        End If
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            'MaxArt = dataRd.Item("PriNumProt")
            If ArtTerminale = 0 Then GroupLabel9.Text = dataRd.Item("PriNumProt")
            If OkCSA = False Then MaxDat = dataRd.Item("PridataGio")
        End While
        dataRd.Close()
        If ArtTerminale > 0 Then
            Str = "SELECT Max(PriNumProt) as PriNumProt from TbPri where PriregIva = 0"
            Cmd = New SqlCommand(Str, cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                'MaxArt = dataRd.Item("PriNumProt")
            End While
            dataRd.Close()
        End If
        DateEdit1.EditValue = MaxDat
        Str = "SELECT isnull(MAX(PRIDATAGIO),(select esedal from tbese where eseanno = (select MIN(aziannolavoro)from tbazi ))) FROM TBPRI WHERE PRIGSTAMPA = 1 AND PRIARTFISC > 0 "
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MinDat = dataRd.Item(0)
        End While
        dataRd.Close()
        Str = "SELECT EseAl from tbese where eseanno = (select max(aziannolavoro)from tbazi ) "
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MMMDat = dataRd.Item("EseAl")
        End While
        dataRd.Close()
        ''leggo anno dell'ultima apertura per controllare data operazione
        Str = "SELECT top 1 PriDataGio from TbPri where PriCausale = 45 order by PriDataGio desc"
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            AnnoOp = CDate(dataRd.Item("PriDataGio")).Year
        End While
        dataRd.Close()
        If Sw = 1 Then Exit Sub


        Str = "SELECT * from TbCii order by CiiCod"
        Dim SS As String = ""
        ImageComboBoxEdit2.Properties.Items.Clear()
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("CiiCod") > 3 Then
                SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau")
                nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
                ImageComboBoxEdit2.Properties.Items.Add(nn)
            End If
        End While
        dataRd.Close()
        Str = "SELECT PIAANACO from TbPIA WHERE PIACODCO = '99.99'"
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit31.EditValue = dataRd.Item("PiaAnaCo")
        End While
        dataRd.Close()
        ImageComboBoxEdit1.Properties.Items.Clear()
        Str = "SELECT DISTINCT  ARTPID,ARTPSIGLA from TBARTP ORDER BY ARTPSIGLA"
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SS = dataRd.Item("ArtPid").ToString.PadRight(6, " ") & " " & dataRd.Item("ARTPSIGLA")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("ArtPid"), -1)
            ImageComboBoxEdit1.Properties.Items.Add(nn)
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT EseArtGiroconto FROM TbEse Where EseAnno = " & CDate(DateEdit1.EditValue).Year, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ArtIrpef = dataRd.Item("EseArtGiroconto")
        End While
        dataRd.Close()
        ImageComboBoxEdit1.SelectedIndex = -1
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
        OkLDP = False
        If UserId = "CSABOX" Then OkCSA = True Else OkCSA = False
        If UserId.ToUpper = "PASTAECO" Or UserId.ToUpper = "PASTANEW" Or UserId.ToUpper = "PASTAGROUP" Then
            OkLDP = True
            ButtonEXP.ImageIndex = 5
            CheckedComboBoxEdit1.Properties.Items.Clear()
            Cmd = New SqlCommand("SELECT LdpRif,LdpSigla FROM TbLdp order by LdpSigla", CnDc)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(dataRd.Item("LdpRif"), dataRd.Item("LdpSigla").ToString, CheckState.Unchecked)
                CheckedComboBoxEdit1.Properties.Items.Add(Em)
            End While
            dataRd.Close()
            GoTo IIPARTE
        End If
        If UserId.ToUpper = "MONDOMARINE" Then
            OkMondo = True
            CheckedComboBoxEdit1.Properties.Items.Clear()
            Cmd = New SqlCommand("SELECT TcmRif,TcmSigla FROM TbTcm Where TcmCdc = 1 order by TcmSigla", cnDb)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(dataRd.Item("TcmRif"), dataRd.Item("TcmSigla").ToString, CheckState.Unchecked)
                CheckedComboBoxEdit1.Properties.Items.Add(Em)
            End While
            dataRd.Close()
        End If
IIPARTE:
        If OkMondo = True Or OkLDP = True Then
            CheckedComboBoxEdit1.Visible = True : ButtonEXP.Visible = True
        Else
            CheckedComboBoxEdit1.Visible = False : ButtonEXP.Visible = False
        End If
        REM PRIMA NOTA COMPENSATA
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 500", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("Sel9") Is DBNull.Value Then PnCompensa = False Else PnCompensa = dataRd.Item("Sel9")
        End While
        dataRd.Close()
    End Sub
    Private Sub Numbox2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit1.Enter
        ButtonF1.Enabled = False
    End Sub
    Private Sub NumBox2_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextEdit1.Validating
        RileggoUltimi()
        ' If Val(TextEdit1.EditValue) > MaxArt + 1 Then
        'TextEdit1.EditValue = MaxArt + 1
        ' TextEdit1.EditValue = 
        ' e.Cancel = True
        'End If
    End Sub
    Private Sub Numbox2_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit1.LostFocus
        If Sw = 0 Then Exit Sub
        ButtonF1.Enabled = True
        ControllaArticolo(Val(TextEdit1.EditValue))
    End Sub
    Private Sub TextEdit4_Enter(sender As Object, e As System.EventArgs) Handles TextEdit4.Enter
        TextEdit4.SelectAll()
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If Controlli() = False Then Exit Sub
        Parallelo()
        IniziaCorpo()
    End Sub
    Sub IniziaCorpo()
        AbilitaGroup(1)
        TotaleResiduo = 0
        X1 = 0
        ' CARICA PRIMA RIGA
        CaricaDettagli(1)
    End Sub
    Sub CaricaDettagli(ByVal I As Int16)
        TextEdit15.EditValue = RwFcf("PriProg")
        TextEdit20.EditValue = RwFcf("Importo")
        TextEdit18.EditValue = RwFcf("PriCoAvere")
        LeggiConto(TextEdit18.EditValue, TextEdit19)
        TotaleIn()
        ImageComboBoxEdit2.EditValue = RwFcf("PriCausale")
        TextEdit17.EditValue = RwFcf("PriCoDare")
        TextEdit4.EditValue = RwFcf("PriDesc") & RTrim(RwFcf("PriDescB"))
        LeggiConto(TextEdit17.EditValue, TextEdit16)
        DateEdit2.EditValue = RwFcf("PriDataEst")
        TextEdit2.EditValue = RwFcf("PriDocEst")
        TextEdit3.EditValue = RwFcf("PriDocAnn")
        If DsFat.Tables(Fa).Rows.Count >= I Then
            GridView1.ClearSelection()
            GridView1.FocusedRowHandle = I - 1
            GridView1.SelectRow(I - 1)
            If I > 1 Then
                GridView1.UnselectRow(I - 2)
            End If
        Else
            If I > 1 Then
                GridView1.FocusedRowHandle = I - 2
                GridView1.SelectRow(I - 2)
            End If
        End If
        If I > 1 Then
            ImageComboBoxEdit2.Focus()
        Else
            TextEdit17.Focus()
        End If
    End Sub

    Sub TotaleIn()
        If Trim(TextEdit20.EditValue.ToString) = "" Then TextEdit20.EditValue = CDec(0.0)
        If RwFat("PriCoDare") <> COBA And RwFat("PriCoAvere") <> COBA Then
            TotTransito = TotaleResiduo + (CDec(TextEdit20.EditValue)) - (CDec(TextEdit20.EditValue))
            GoTo Display
        End If
        If RwFat("PriCoDare") <> COBA Then
            TotTransito = TotaleResiduo + (CDec(TextEdit20.EditValue))
        End If
        If RwFat("PriCoAvere") <> COBA Then
            TotTransito = TotaleResiduo - (CDec(TextEdit20.EditValue))
        End If
Display:
        TextEdit24.EditValue = TotTransito
    End Sub
    Function LeggiConto(ByRef CodCo As String, ByRef Anagraf As TextEdit) As Boolean
        Anagraf.Text = ""
        LeggiConto = False
        Fl04 = 0
        Fl05 = 0
        Fl06 = 0
        AggiustaConto(CodCo) ''''' verifica il punto se e' un sottoconto
        REM CONTROLLO CONTI RAGGRUPPAMENTO CLIENTI E FORNITORI DA NON UTILIZZARE
        For K As Int16 = 1 To Grp(0)
            If Grp(K) = CodCo Then
                LeggiConto = False
                Exit Function
            End If
        Next

        Dim Str As String = "SELECT * from TbPia where PiaCodCo = '" & CodCo & "'"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Anagraf.Text = dataRd("PiaAnaCo")
            Fl04 = dataRd("PiaFl04")
            Fl05 = dataRd("PiaFl05")
            Fl06 = dataRd("PiaFl06")
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
            RwFat("IMPORTO") = 0
            RwFat("PriCoDare") = ""
            RwFat("PriCoAvere") = ""
            RwFat("PriIvaPrint") = 0
            RwFat("PriGstampa") = 0
            RwFat("PriCodPag") = 0
            OkFat = False
        End If
        RwFat("PriCausale") = ImageComboBoxEdit2.EditValue
        RwFat("PriDescB") = RTrim(Mid(TextEdit4.EditValue, 25, 32))
        RwFat("PriDesc") = Mid(TextEdit4.EditValue, 1, 24)
        RwFat("PriDocEst") = Val(TextEdit2.EditValue)
        RwFat("PriDocAnn") = Val(TextEdit3.EditValue)
        RwFat("PriNsRif") = TextEdit5.EditValue
        RwFat("PridataGio") = DateEdit1.EditValue
        RwFat("PridataEst") = DateEdit2.EditValue
        IniziaRwFat()
        ProgId = RwFat("PriId")
        DsFcf = New DataSet(Ft)
        DaFcf = New SqlDataAdapter("SELECT *,0.0 as IMPORTO from TbPri where  PriId = " & ProgId, cnCo)
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
        RwFcf("PriDataEst") = CDate(DateEdit2.EditValue)
        RwFcf("PriCausale") = RwFat("PriCausale") 'Val(Numbox3.Text)
        RwFcf("PriCoDare") = RwFat("PriCoDare") '  TextEdit17.EditValue
        RwFcf("PriCoAvere") = RwFat("PriCoAvere")
        RwFcf("PriNumProt") = Val(TextEdit1.EditValue)
        RwFcf("PriBisRet") = ""
        RwFcf("PriRegIva") = 0
        RwFcf("PriDesc") = RwFat("PriDesc") 'TextBox2.Text
        If OkArt = True And NOadd = False Then    ' ARTICOLO AUTOMATICO and f6 = false 
            RwFcf("PriDocEst") = Val(TextEdit2.EditValue)
            RwFcf("PriDocAnn") = Val(TextEdit3.EditValue)
        Else
            RwFcf("PriDocEst") = RwFat("PriDocEst")
            RwFcf("PriDocAnn") = RwFat("PriDocAnn")
        End If
        RwFcf("PriMeseSk") = ""
        'RwFcf("PriDataEst") = RwFat("PriDataEst")
        RwFcf("PriDescB") = RwFat("PriDescB")
        RwFcf("PriFl04") = Fl04
        RwFcf("PriFl05") = Fl05
        RwFcf("PriFl06") = Fl06
        RwFcf("PriNsRif") = RwFat("PriNsRif")
        RwFcf("PriSos") = RwFat("PriSos")
        RwFcf("PriLinea") = ""
        RwFcf("PriCodPag") = 0
        RwFcf("PriValuta") = 0
        RwFcf("PriArtFisc") = 0
        RwFcf("PriId") = ProgId
        RwFcf("PriProg") = POSRIG
        RwFcf("PriIvaPrint") = RwFat("PriIvaPrint")
        RwFcf("PriGStampa") = RwFat("PriGStampa")
        ''' se esiste lo scorporo cambiare
        RwFcf("PriCodIva") = 0
        RwFcf("PriImpDare") = RwFat("PriImpDare")
        RwFcf("PriImpAvere") = RwFat("PriImpAvere")
        RwFcf("IMPORTO") = RwFat("IMPORTO")
    End Sub
    Function Controlli() As Boolean
        Controlli = True
        If CDate(DateEdit1.EditValue) > MMMDat Then
            Messaggio(1, "DATA GIORNALE > MASSIMA DATA VALIDA(" & MMMDat.ToShortDateString & ")")
            Controlli = False
        End If
        If CDate(DateEdit1.EditValue) < MinDat Then
            Messaggio(1, "DATA GIORNALE < MINIMA DATA VALIDA(" & MinDat.ToShortDateString & ")")
            Controlli = False
        End If

        If CDate(DateEdit2.EditValue).Year < AnnoOp Then
            Messaggio(1, "DATA OPERAZIONE NON VALIDA < ANNO ULTIMA APERTURA (" & AnnoOp & ")")
            Controlli = False
        End If
        ''If CDate(DateEdit2.EditValue).Year < (CDate(DateEdit1.EditValue).Year - 1) Then
        ''    Messaggio(1, "DATA OPERAZIONE NON VALIDA < ANNO MINIMO (" & (CDate(DateEdit1.EditValue).Year - 1) & ")")
        ''    Controlli = False
        ''End If
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
        If ImageComboBoxEdit2.SelectedIndex = -1 Then
            Controlli = False
            ImageComboBoxEdit2.Focus()
            Exit Function
        End If
    End Function
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "INSERIMENTO PRIMA NOTA"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub
    Function ControllaArticolo(ByVal n As Int32) As Boolean
        ControllaArticolo = False
        OkPn = False
        If n = 0 Then
            'n = MaxArt + 1
            'TextEdit1.EditValue = n
            TextEdit1.EditValue = ""
        End If
        InizializzaTableFat("SELECT * FROM vh4 where PriNumProt = " & n & " Order by Priprog")
        'Dim OldTrue As Boolean = GroupBox4.Enabled
        ' GroupBox4.Enabled = True
        GridControl1.DataSource = DsFat.Tables(Fa)
        GridControl1.Refresh()
        GridView1.ClearSelection()
        'GroupBox4.Enabled = OldTrue
        If DsFat.Tables(Fa).Rows.Count > 0 Then
            ControllaArticolo = True
            OkPn = True
            CaricaDati()
            If OkMondo = True Or OkLDP = True Then ButtonEXP.Visible = True Else ButtonEXP.Visible = False
        Else
            Pulizia(1) : AzzeraCheck()
            n = 0
            TextEdit1.EditValue = ""
        End If
    End Function
    Sub InizializzaTableFat(ByVal StrReg As String)
        DsFat = New DataSet(Fa)
        DaFat = New SqlDataAdapter(StrReg, cnCo)
        DaFat.SelectCommand.CommandTimeout = 300
        DaFat.Fill(DsFat, Fa)
    End Sub
    Sub CaricaDati()
        Dim k As Int16
        For k = DsFat.Tables(Fa).Rows.Count() To 1 Step -1
            RwFat = DsFat.Tables(Fa).Rows(k - 1)
            If k = 1 Then
                ProgId = RwFat("PriId")
                TextEdit15.EditValue = k
                DateEdit1.EditValue = RwFat("PriDataGio")
                DateEdit2.EditValue = RwFat("PriDataEst")
                ImageComboBoxEdit2.EditValue = RwFat("PriCausale")
                TextEdit2.EditValue = RwFat("PriDocEst")
                TextEdit3.EditValue = RwFat("PriDocAnn")
                TextEdit5.EditValue = RwFat("PriNsRif")
                TextEdit4.EditValue = RwFat("PriDesc") & RTrim(RwFat("PriDescB"))
                TextEdit17.EditValue = RwFat("PriCoDare")
                TextEdit16.EditValue = RwFat("DAREDESC")
                TextEdit18.EditValue = RwFat("PriCoAvere")
                TextEdit19.EditValue = RwFat("AVEREDESC")
                TextEdit20.EditValue = RwFat("IMPORTO")
                If RwFat("PriGstampa") = True Then
                    DateEdit1.ErrorText = "Articolo Stampato sul Libro Giornale In Bollo!!!"
                    ButtonF3.Enabled = False
                Else
                    DateEdit1.ErrorText = ""
                    ButtonF3.Enabled = True
                End If
            End If
            LeggoFlagCpt()
        Next
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
    Sub LeggoFlagCpt()
        Dim Cmd As New SqlCommand("SELECT PiaFl04 from TbPia where PiaCodCo = '" & RwFat("PriCoDare") & "'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("PiaFl04") > 0 And dataRd.Item("PiaFl04") < 25 Then OLDCSP = 1
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT PiaFl04 from TbPia where PiaCodCo = '" & RwFat("PriCoAvere") & "'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("PiaFl04") > 0 And dataRd.Item("PiaFl04") < 25 Then OLDCSP = 1
        End While
        dataRd.Close()
    End Sub
    Private Sub Textbox6_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit18.Enter
        If LeggiConto(TextEdit17.EditValue, TextEdit16) = False Then
            TextEdit17.Focus()
        Else
            ButtonF6Click()
        End If
        CalcolaRiga(0)
    End Sub
    Private Sub Textbox20_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit20.Enter
        If LeggiConto(TextEdit18.EditValue, TextEdit19) = False Then
            TextEdit18.Focus()
        Else
            ButtonF6Click()
        End If
        CalcolaRiga(1)
    End Sub
    Private Sub Numbox16_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonXX.Enter
        If TextEdit20.EditValue.ToString = "" Then TextEdit20.EditValue = CDec(0.0)
        CalcolaRiga(2)
    End Sub
    Sub CalcolaRiga(ByVal i As Int16)
        If i = 0 Then
            RwFat("PriCoDare") = TextEdit17.EditValue
            RwFat("DAREDESC") = TextEdit16.EditValue
        End If
        If i = 1 Then
            RwFat("PriCoAvere") = TextEdit18.EditValue
            RwFat("AVEREDESC") = TextEdit19.EditValue
        End If
        If i = 2 Then
            RwFat("IMPORTO") = CDec(TextEdit20.EditValue)
            If RwFat("PriCoDare") <> COBA Then RwFat("PriImpDare") = RwFat("IMPORTO")
            If RwFat("PriCoAvere") <> COBA Then RwFat("PriImpAvere") = RwFat("IMPORTO")
            ButtonXX.Focus()
        End If
        TotaleIn()
    End Sub
    Private Sub ButtonXX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXX.Click
        If ControllaConto(TextEdit17.EditValue, TextEdit16) = False Then
            TextEdit17.Focus()
            Exit Sub
        End If
        If ImageComboBoxEdit2.SelectedIndex = -1 Then
            ImageComboBoxEdit2.Focus()
            Exit Sub
        End If
        If ControllaConto(TextEdit18.EditValue, TextEdit19) = False Then
            TextEdit18.Focus()
            Exit Sub
        End If
        RicalcolaDocumento()
    End Sub
    Sub RiassegnaValori()
        RwFat("PriCausale") = ImageComboBoxEdit2.EditValue
        RwFat("PriDescB") = RTrim(Mid(TextEdit4.EditValue, 25, 32))
        RwFat("PriDesc") = Mid(TextEdit4.EditValue, 1, 24)
        RwFat("PriDataEst") = DateEdit2.EditValue
        RwFat("PriDocEst") = Val(TextEdit2.EditValue)
        RwFat("PriDocAnn") = Val(TextEdit3.EditValue)
        RwFat("PriNsRif") = TextEdit5.EditValue
        RwFat("PriCoDare") = TextEdit17.EditValue
        RwFat("DAREDESC") = TextEdit16.EditValue
        RwFat("PriCoAvere") = TextEdit18.EditValue
        RwFat("AVEREDESC") = TextEdit19.EditValue
        RwFat("PriProg") = TextEdit15.EditValue
    End Sub
    Sub RicalcolaDocumento()
        RiassegnaValori() ' da sotto a sopra e sul datarow
        'If RwFat("PriCoDare") = COBA And RwFat("PriCoAvere") = COBA Then Exit Sub OCCHIO NON DEVE REGISTRARE LA RIGA 
        If OkFat = False Then DsFat.Tables(Fa).Rows.Add(RwFat)
        POSRIG = RwFat("PriProg")
        IniziaRwFcf()
        If OkFcf = False Then DsFcf.Tables(Ft).Rows.Add(RwFcf)
        RicalcoloFinale(POSRIG + 1)
        If TotaleResiduo = 0 And OkQuadra = True Then ''''new OKQUADRA '''' sara' vero ?????
            RegistraFattura()
            Exit Sub
        End If
        POSRIG = POSRIG + 1
        RigeneraFat()
        RigeneraFcF()
        GridView1.ClearSelection()
        CaricaDettagli(POSRIG)
        ImageComboBoxEdit2.Focus()
    End Sub
    Sub RicalcoloFinale(ByVal i As Int16)
        Dim x, OkD, OkA As Int16
        X1 = 0 : OkD = 0 : OkA = 0
        For x = 1 To i - 1
            RwFRI = DsFcf.Tables(Ft).Rows(x - 1)
            If RwFRI("PriCoDare") <> COBA Then X1 = X1 + RwFRI("PriImpDare") Else OkD = 1
            If RwFRI("PriCoAvere") <> COBA Then X1 = X1 - RwFRI("PriImpAvere") Else OkA = 1
        Next
        TotaleResiduo = X1
        If i = 2 And TotaleResiduo = 0 And (OkD = 1 Or OkA = 1) Then OkQuadra = False Else OkQuadra = True
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
        RiassegnaValori()
        RwFat("PriCodIva") = 0
        RwFat("PriImpDare") = 0
        RwFat("PriImpavere") = 0
        RwFat("IMPORTO") = 0
        RwFat("PriIvaPrint") = 0
        RwFat("PriGstampa") = 0
        RwFat("PriCodPag") = 0
        RwFat("PriDocEst") = 0
        RwFat("PriCoDare") = ""
        RwFat("DAREDESC") = ""
        RwFat("PriCoAvere") = ""
        RwFat("AVEREDESC") = ""
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
        ScriviPri()
        ButtonF5.PerformClick()
    End Sub
    Function ControllaConto(ByRef CodCo As String, ByRef Anagraf As TextEdit) As Boolean
        ControllaConto = False
        If Mid(CodCo, 3, 3) = ".00" Then Exit Function '' mastri e transitorio
        ControllaConto = LeggiConto(CodCo, Anagraf)
    End Function
    Private Sub ButtonF6Click()
        If Val(TextEdit17.EditValue) > 1000 And Val(TextEdit2.EditValue) > 0 And Val(TextEdit3.EditValue) > 0 And Val(TextEdit20.EditValue) = 0 Then
            RicercaPARTITADiretta(TextEdit17.EditValue)
        ElseIf Val(TextEdit18.EditValue) > 1000 And Val(TextEdit2.EditValue) > 0 And Val(TextEdit3.EditValue) > 0 And Val(TextEdit20.EditValue) = 0 Then
            RicercaPARTITADiretta(TextEdit18.EditValue)
        End If
    End Sub
    Private Sub ButtonF6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF6.Click
        If Val(TextEdit1.EditValue) > 0 Then Exit Sub
        If Val(TextEdit17.EditValue) > 1000 Then
            LancioPARTITA(TextEdit17.EditValue, 0)
        ElseIf Val(TextEdit18.EditValue) > 1000 Then
            LancioPARTITA(TextEdit18.EditValue, 1)
        End If
    End Sub
    Function RicercaPARTITADiretta(ByVal CF As String) As Boolean
        Dim Str = "SELECT sum(dare) as Dare,sum(avere) as Avere FROM VB8 WHERE PRKCONTO = '" & CF.PadLeft(5, "0") & "' AND PRKDOCEST = " & Val(TextEdit2.EditValue) & " AND PRKDOCANN = " & Val(TextEdit3.EditValue) & " AND PRIID <> " & ProgId
        Dim NETTO As Decimal = 0
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("DARE") Is DBNull.Value Then Exit While
            If dataRd.Item("AVERE") Is DBNull.Value Then Exit While
            NETTO = Math.Abs(dataRd.Item("DARE") - dataRd.Item("AVERE"))
        End While
        dataRd.Close()
        TextEdit20.EditValue = NETTO '''' sempre in assoluto
        If Val(CF) > MiglioFo Then ControllaImportoFo(CF)
    End Function
    Sub ControllaImportoFo(ByVal CF As String)
        Dim Str = "SELECT * from VRITNETTO WHERE RITCODFOR = '" & CF.PadLeft(5, "0") & "' AND RITPROTFAT = " & Val(TextEdit2.EditValue) & " AND DATEPART(YEAR,RITDATAFAT) = " & Val(TextEdit3.EditValue)
        Dim NETTO As Decimal = 0
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            NETTO = dataRd.Item("NETTO")
        End While
        dataRd.Close()
        If NETTO > 0 And CDec(TextEdit20.EditValue) > NETTO Then
            TextEdit20.EditValue = NETTO
            TextEdit20.ErrorText = "IMPORTO AL NETTO DELLA RITENUTA! "
        Else
            TextEdit20.ErrorText = ""
        End If
    End Sub
    Function LancioPARTITA(ByVal CF As String, ByVal DA As Int16) As Boolean
        Dim Gesterna As New DxSaldaP
        Dim J, S, K As Int16
        MaxConti = 0 : NOadd = False
        Gesterna.PAnaCod = CF
        Gesterna.ShowDialog()
        If Val(CF) > MiglioFo Then S = -1 Else S = 1
        DsPno = Gesterna.PDsPno
        MaxConti = DsPno.Tables(Pn).Rows.Count
        If MaxConti > 0 Then
            If OkArt = True Then
                For Q = 1 To DsFat.Tables(Fa).Rows.Count
                    RwFat = DsFat.Tables(Fa).Rows(Q - 1)
                    If RwFat("PriProg") = POSRIG Then NOadd = True : Exit For
                Next
            End If
            K = POSRIG
            For J = 1 To MaxConti
                RwPno = DsPno.Tables(Pn).Rows(J - 1)
                If NOadd = False Then RwFat = DsFat.Tables(Fa).NewRow
                POSRIG = (J - 1) + K
                RwFat("PriProg") = POSRIG
                RwFat("PriCodIva") = 0
                RwFat("PriRegIva") = 0
                RwFat("PriNumProt") = 0 'MaxArt + 1
                TextEdit1.EditValue = "" 'MaxArt + 1
                If PnCompensa = True Then RwFat("IMPORTO") = Val(RwPno("SCOPERTO")) * S Else RwFat("IMPORTO") = Math.Abs(Val(RwPno("SCOPERTO")))
                If DA = 0 Then
                    RwFat("PriCoDare") = TextEdit17.EditValue
                    If PnCompensa = True Then RwFat("PriImpDare") = Val(RwPno("SCOPERTO")) * S Else RwFat("PriImpDare") = Math.Abs(Val(RwPno("SCOPERTO")))
                    RwFat("PriImpavere") = 0
                    If OkArt = False AndAlso MaxConti > 1 Then RwFat("PriCoAvere") = "00.10" Else RwFat("PriCoAvere") = TextEdit18.EditValue
                Else
                    RwFat("PriImpDare") = 0
                    RwFat("PriCoAvere") = TextEdit18.EditValue
                    If PnCompensa = True Then RwFat("PriImpavere") = Val(RwPno("SCOPERTO")) * S Else RwFat("PriImpavere") = Math.Abs(Val(RwPno("SCOPERTO")))
                    If OkArt = False AndAlso MaxConti > 1 Then RwFat("PriCoDare") = "00.10" Else RwFat("PriCoDare") = TextEdit17.EditValue
                End If
                RwFat("PriDataEst") = DateEdit2.EditValue
                RwFat("PriDocEst") = Val(RwPno("PrkDocEst"))
                RwFat("PriDocAnn") = Val(RwPno("PrkDocAnn"))
                RwFat("PriIvaPrint") = 0
                RwFat("PriGstampa") = 0
                RwFat("PriCodPag") = 0
                RwFat("PriNsRif") = ""
                RwFat("PriCausale") = ImageComboBoxEdit2.EditValue
                RwFat("PriDescB") = RTrim(Mid(TextEdit4.EditValue, 25, 32))
                RwFat("PriDesc") = Mid(TextEdit4.EditValue, 1, 24)
                RwFat("PridataGio") = DateEdit1.EditValue
                RwFat("PridataEst") = DateEdit2.EditValue
                IniziaRwFat()
                RwFat("PriId") = 0
                If NOadd = False Then DsFat.Tables(Fa).Rows.Add(RwFat)
                RigeneraFcF()
                If OkFcf = False Then DsFcf.Tables(Ft).Rows.Add(RwFcf) : OkFcf = True
                OkFat = True
            Next
            For X = 1 To DsFat.Tables(Fa).Rows.Count
                RwFat = DsFat.Tables(Fa).Rows(X - 1)
                LeggiConto(RwFat("PriCoDare"), TextEdit16)
                LeggiConto(RwFat("PriCoAvere"), TextEdit19)
                RwFat("DAREDESC") = TextEdit16.EditValue
                RwFat("AVEREDESC") = TextEdit19.EditValue
                If X = DsFat.Tables(Fa).Rows.Count Then
                    GridControl1.DataSource = DsFat.Tables(Fa)
                    GridControl1.Refresh()
                    GridView1.ClearSelection()
                    GridView1.FocusedRowHandle = POSRIG - 1
                    RwX = GridView1.GetFocusedDataRow
                    DaMouse()
                    If DA = 0 Then TextEdit18.Focus() Else ButtonXX.Focus()
                End If
            Next
        End If
    End Function
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        If TextEdit17.ContainsFocus = True Then
            Dim Nc As String = ""
            Nc = EstraiRicerca(TextEdit17.EditValue.ToUpper)
            If Nc > "00.00" Then
                TextEdit17.EditValue = Nc
                LeggiConto(TextEdit17.EditValue, TextEdit16)
                SelectNextControl(TextEdit18, True, True, True, True)
            End If
            Exit Sub
        End If
        If TextEdit18.ContainsFocus = True Then
            Dim Nc As String = ""
            Nc = EstraiRicerca(TextEdit18.EditValue.ToUpper)
            If Nc > "00.00" Then
                TextEdit18.EditValue = Nc
                LeggiConto(TextEdit18.EditValue, TextEdit19)
                SelectNextControl(TextEdit19, True, True, True, True)
            End If
            Exit Sub
        End If
    End Sub
    Function EstraiRicerca(ByVal Tipo As String) As String
        Dim frm As New RicercaClFo
        Dim CF As String = ""
        If Tipo <> "F" And Tipo <> "C" Then
            EstraiRicerca = Query.CercaPia()
            Exit Function
        End If
        If Tipo = "F" Then CF = "FO"
        If Tipo = "C" Then CF = "CL"
        frm.StartPosition = FormStartPosition.Manual
        frm.Location = New Point(GroupControl2.Location.X, GroupControl2.Location.Y + 80)
        frm.CliFor = CF
        frm.ShowDialog()
        EstraiRicerca = frm.Codice
    End Function
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If OkPn = False Then Exit Sub
        If Lettura = True Then Exit Sub
        Messaggio(2, "ELIMINO L'ARTICOLO N. " & Val(TextEdit1.EditValue) & " DEL " & DateEdit2.EditValue & " ? ")
        If Rispondi = MsgBoxResult.Yes Then
            EliminaProt()
            If OKCDC = True Then EliminaMcc()
            RielaboroPartita()
            ArtTerminale = 0
            'RileggoUltimi()
            ButtonF5.PerformClick()
            TextEdit1.EditValue = ""
        End If
    End Sub
    Sub EliminaMcc()
        Dim Cancella As String = "Delete from TbMcc where MCCPRKID = " & ProgId
        Dim Dmd As New SqlCommand(Cancella, CnDc)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub RielaboroPartita()
        EsegueSql(" EXEC DaEliminaPartita ", cnCo)
    End Sub
    Private Sub DatBox1_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.Validated
        DateEdit1.EditValue = CDate(DateEdit1.EditValue)
        If OkPn = False Then DateEdit2.EditValue = DateEdit1.EditValue
    End Sub
    Private Sub DatBox2_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit2.Validated
        If CDate(DateEdit2.EditValue) > CDate(DateEdit1.EditValue) Then
            DateEdit2.EditValue = DateEdit1.EditValue
        End If
        If Val(TextEdit3.EditValue) = 0 And OkPn = False Then TextEdit3.EditValue = CDate(DateEdit2.EditValue).Year
    End Sub
    Sub EliminaProt()
        Partita(0, ProgId)
        Dim Cancella As String = "BEGIN Delete from TbPri where PriId = " & ProgId & " Delete from TbPrk where PrKId = " & ProgId & " END"
        Dim Dmd As New SqlCommand(Cancella, cnCo)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub ScriviPri()
        Dim Scheggia As Int16 = 0
        Dim Articolo As Int32 = 0
        Dim RISERVATA As Boolean = False
        If OkPn = False Or ProgId = 0 Then
            LeggiUltimo(DateEdit2.EditValue)
            Scheggia = 1
        Else
            EliminaProt()
        End If
        ComboBoxEdit1.Properties.Items.Clear()
        Dim x, M, Sr, Sc As Int16
        Dim IvaPScadenze As Decimal
        M = 0 : TotaleResiduo = 0 : Sc = 0 : Sr = 0 : IvaPScadenze = 0
        REM CONTADARE + AVERE
        TCoDare = 0
        TCoAvere = 0
        ContoD = ""
        ContoA = ""
        For x = 1 To DsFcf.Tables(Ft).Rows.Count
            If x > POSRIG Then Exit For
            RwFRI = DsFcf.Tables(Ft).Rows(x - 1)
            If RwFRI("PriCoDare") <> "00.10" Then TCoDare = TCoDare + 1 : ContoD = RwFRI("PriCoDare")
            If RwFRI("PriCoAvere") <> "00.10" Then TCoAvere = TCoAvere + 1 : ContoA = RwFRI("PriCoAvere")
        Next
        For x = 1 To DsFcf.Tables(Ft).Rows.Count
            If x > POSRIG Then Exit For
            RwFRI = DsFcf.Tables(Ft).Rows(x - 1)
            If RwFRI("PriImpDare") = 0 And RwFRI("PriImpAvere") = 0 Then
                GoTo VaiOltre
            End If
            If RwFRI("PriCoDare") = COBA And RwFRI("PriCoAvere") = COBA Then
                GoTo VaiOltre
            End If
            Wmd.Parameters.Clear()
            If RwFRI("PriCoDare") <> COBA Then
                TotaleResiduo = TotaleResiduo + RwFRI("PriImpDare")
            Else
                RwFRI("PriImpDare") = 0
            End If
            If RwFRI("PriCoAvere") <> COBA Then
                TotaleResiduo = TotaleResiduo - RwFRI("PriImpAvere")
            Else
                RwFRI("PriImpAvere") = 0
            End If
            If RwFRI("PriFl04") > 0 And RwFRI("PriFl04") < 25 Then Sc = 1
            If Scheggia = 1 Then Articolo = RileggoLocked()
            M = M + 1
            RwFRI("PriDataGio") = CDate(DateEdit1.EditValue) ''''' DEVONO ESSERE UGUALI SULLO STESSO ARTICOLO
            RwFRI("PriDataEst") = CDate(DateEdit2.EditValue) ''''' DEVONO ESSERE UGUALI SULLO STESSO ARTICOLO
            p1.Value = RwFRI("PriDataGio")
            '' If RwFRI("PriCausale") = 72 Or RwFRI("PriCausale") = CausaleAP Or RwFRI("PriCausale") = CausaleCh Then RISERVATA = True
            If RwFRI("PriCausale") = CausaleAP Or RwFRI("PriCausale") = CausaleCh Then RISERVATA = True
            '' nuova modifica ma non so perchè succede dal 16/05/2017
            RwFRI("PriCoDare") = RwFRI("PriCoDare").PadLeft(5, "0")
            RwFRI("PriCoAvere") = RwFRI("PriCoAvere").PadLeft(5, "0")
            '' nuova modifica ma non so perchè succede dal 16/05/2017
            p2.Value = RwFRI("PriCausale")
            p3.Value = RwFRI("PriCoDare")
            p4.Value = RwFRI("PriCoAvere")
            If Articolo > 0 Then RwFRI("PriNumProt") = Articolo
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
            'gestione conti dare/avere
            DefinisciPriSos()
            'p20.Value = RwFRI("PriSos")
            p21.Value = RwFRI("PriLinea")
            p22.Value = RwFRI("PriDocAnn")
            p23.Value = 0
            p24.Value = RwFRI("PriValuta")
            p25.Value = RwFRI("PriArtFisc")
            p26.Value = ProgId
            p27.Value = M
            p28.Value = RwFRI("PriIvaPrint")
            p29.Value = RwFRI("PriGStampa")
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
            If Scheggia = 1 Then Scheggia = SbloccoLocked()
            If Val(RwFRI("PriCoDare")) > MiglioFo Then ControllaRitenute()
VaiOltre:
        Next
        If TotaleResiduo <> 0 Then PareggiaArticolo(M)
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        Partita(0, ProgId)
        Partita(1, 0)
        If Sc = 1 Then LancioCespiti()
        If OLDCSP = 1 Then
            Messaggio(1, "CONTROLLARE CESPITI")
        End If
        Dim ProgCdc As Int32
        ProgCdc = ProgId
        '''' SE ARTICOLO IRPEF AUTOMATICO MI SPORCA LA VARIABILE PROGID CON UN ALTRO ARTICOLO
        ' If ComboBoxEdit1.Properties.Items.Count > 0 And ArtIrpef > 0 Then PagaEGiroconta()
        If ComboBoxEdit1.Properties.Items.Count > 0 Then
            PagaEGiroconta()
            ComboBoxEdit1.Properties.Items.Clear()
        End If
        GroupLabel9.Text = p5.Value
        GroupLabel9.Refresh()
        TextEdit1.EditValue = ""
        If OkMondo = True And RISERVATA = False Then LancioCDCMONDO()
        If OkLDP = True And RISERVATA = False Then LancioLDP()
    End Sub
    Sub DefinisciPriSos()
        If TCoAvere > TCoDare And TCoDare = 1 And RwFRI("PriCoAvere") <> "00.10" Then
            p20.Value = ContoD
        ElseIf TCoDare > TCoAvere And TCoAvere = 1 And RwFRI("PriCoDare") <> "00.10" Then
            p20.Value = ContoA
        ElseIf TCoDare = 1 And TCoAvere = 1 And RwFRI("PriCoAvere") = "00.10" Then
            p20.Value = ContoA
        ElseIf TCoDare = 1 And TCoAvere = 1 And RwFRI("PriCoDare") = "00.10" Then
            p20.Value = ContoD
        Else
            p20.Value = ""
        End If
    End Sub

    Sub LancioCDCMONDO()
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
        MONDOSiglaC.PLocat = New Point(TextEdit18.Location.X, Me.Size.Height)
        MONDOSiglaC.PRet = False
        Gesterna.ShowDialog()
    End Sub
    Sub LancioLDP()
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
        LDPSigla.PLocat = New Point(TextEdit18.Location.X, Me.Size.Height)
        LDPSigla.PRet = False
        Gesterna.ShowDialog()
    End Sub
    Sub ControllaRitenute()
        Dim Str = "SELECT * from VRITNETTO WHERE RITCODFOR = '" & RwFRI("PriCoDare") & "' AND RITPROTFAT = " & RwFRI("PriDocEst") & " AND DATEPART(YEAR,RITDATAFAT) = " & RwFRI("PriDocAnn")
        Dim NETTO As Decimal = 0
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            NETTO = dataRd.Item("NETTO")
        End While
        dataRd.Close()
        ''' doppio controllo non fa male!!!!
        If NETTO = 0 Or RwFRI("PriImpDare") = 0 Then Exit Sub
        If NETTO <> RwFRI("PriImpDare") Then Exit Sub
        Str = "SELECT * from TBRIT WHERE RITCODFOR = '" & RwFRI("PriCoDare") & "' AND RITPROTFAT = " & RwFRI("PriDocEst") & " AND DATEPART(YEAR,RITDATAFAT) = " & RwFRI("PriDocAnn") & " AND RITDATAPAG is null"
        Dim RTN As New SqlCommand(Str, cnCo)
        dataRd = RTN.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("RitNum") & "@" & dataRd.Item("RitCodFor") & "@" & dataRd.Item("RitRitenuta") & "@" & dataRd.Item("RitProtFat") & "@" & RwFRI("PriDocAnn") & "@" & RwFRI("PriDataEst"))
        End While
        dataRd.Close()
    End Sub
    Sub PagaEGiroconta()
        Dim K As Int16
        Dim PAG As String = ""
        Dim Upd As New SqlCommand(PAG, cnCo)
        For K = 1 To ComboBoxEdit1.Properties.Items.Count
            SSPLIT = ComboBoxEdit1.Properties.Items(K - 1)
            PAG = "Update TbRit Set RITDATAPAG = '" & SSPLIT.Split("@")(5) & "' WHERE RITNUM = " & Val(SSPLIT.Split("@")(0))
            Upd = New SqlCommand(PAG, cnCo)
            Upd.ExecuteNonQuery()
            If ArtIrpef > 0 Then GiroIrpef(Val(SSPLIT.Split("@")(0)))
        Next
    End Sub
    Function GiroIrpef(ByVal NrRit As Int32) As Boolean
        Dim RitRitenuta As Decimal = 0
        Dim RitCodFor As String = ""
        Dim Ok As Boolean = False
        RitCodFor = SSPLIT.Split("@")(1)
        RitRitenuta = CDec(SSPLIT.Split("@")(2))
        If RitRitenuta = 0 Then Exit Function
        Dim StrReg As String = "SELECT top 1 * from TbArtP where ArtPId = " & ArtIrpef & " Order By ArtPprog"
        Dim Acau As Int16
        Dim AAvere As String = ""
        Dim ADesc1 As String = ""
        Dim ADesc2 As String = ""
        Dim RR As New SqlCommand(StrReg, cnCo)
        dataRd = RR.ExecuteReader
        While dataRd.Read
            Ok = True
            Acau = dataRd.Item("ArtPCausale")
            AAvere = dataRd.Item("ArtPAvere")
            ADesc1 = dataRd.Item("ArtPDesc1")
            ADesc2 = dataRd.Item("ArtPDesc2")
        End While
        dataRd.Close()
        If Ok = False Then Exit Function
        LeggiUltimo(DateEdit2.EditValue)
        Wmd.Parameters.Clear()
        Dim Articolo As Int32
        Articolo = RileggoLocked()
        p1.Value = RwFRI("PriDataGio")
        p2.Value = Acau
        p3.Value = RitCodFor
        p4.Value = AAvere
        p5.Value = Articolo
        p6.Value = ""
        p7.Value = 0
        p8.Value = 0
        p9.Value = RitRitenuta
        p10.Value = RitRitenuta
        p11.Value = ADesc1
        p12.Value = Val(SSPLIT.Split("@")(3))
        p13.Value = ""
        p14.Value = CDate(SSPLIT.Split("@")(5))
        p15.Value = ADesc2
        p16.Value = 0
        p17.Value = 0
        p18.Value = 0
        p19.Value = ""
        p20.Value = ""
        p21.Value = ""
        p22.Value = Val(SSPLIT.Split("@")(4))
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
        Partita(1, 0)
    End Function

    Function SbloccoLocked() As Int16
        Dim Del As New SqlCommand("Delete from TMPlock WITH (TABLOCKX) where IdPrNota = 1 ", cnCo)
        Del.ExecuteNonQuery()
        Return 0
    End Function
    Function RileggoLocked() As Int32
        RileggoLocked = -1
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
        ArtTerminale = RileggoLocked
    End Function
    Sub LancioCespiti()
        Dim Gesterna As New DxInsCesp
        OLDCSP = 0
        Gesterna.ShowDialog()
    End Sub

    Sub PareggiaArticolo(ByVal Riga As Int16)
        Wmd.Parameters.Remove(p3)
        Wmd.Parameters.Remove(p4)
        Wmd.Parameters.Remove(p9)
        Wmd.Parameters.Remove(p10)
        Wmd.Parameters.Remove(p27)
        Riga = Riga + 1
        p27.Value = Riga
        If TotaleResiduo < 0 Then
            p4.Value = "00.10"
            p3.Value = "99.99"
            p10.Value = 0
            p9.Value = TotaleResiduo * -1
        Else
            p3.Value = "00.10"
            p4.Value = "99.99"
            p9.Value = 0
            p10.Value = TotaleResiduo
        End If
        Wmd.Parameters.Add(p3)
        Wmd.Parameters.Add(p4)
        Wmd.Parameters.Add(p9)
        Wmd.Parameters.Add(p10)
        Wmd.Parameters.Add(p27)
        Wmd.ExecuteNonQuery()
    End Sub
    Sub Partita(ByVal Tipo As Int16, ByVal AZ As Int32)
        If Tipo = 0 Then
            EsegueSql(" EXEC RiAprePartita  @Id = " & ProgId & ",@Az=" & AZ & ",@Miglio=" & MiglioFo, cnCo)
        Else
            EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
        End If
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
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub

    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If GroupControl3.Enabled = False Then Exit Sub
        If iset > -1 Then
            RwX = GridView1.GetDataRow(iset)
            POSRIG = RwX("PriProg")
            DaMouse()
        End If
    End Sub
    Sub DaMouse()
        Dim x As Int16
        X0 = 0
        For x = 1 To DsFcf.Tables(Ft).Rows.Count
            RwFcf = DsFcf.Tables(Ft).Rows(x - 1)
            RwFat = DsFat.Tables(Fa).Rows(x - 1)
            If RwX("PriProg") = RwFcf("PriProg") Then
                OkFat = True
                OkFcf = True
                RwFcf("Importo") = RwFat("Importo")
                RicalcoloFinale(RwFcf("PriProg"))
                CaricaDettagli(RwFcf("PriProg"))
                Exit Sub
            End If
        Next
    End Sub
    Private Sub TextEdit17_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextEdit17.KeyUp
        If Len(TextEdit17.Text) = 1 And TextEdit17.Text = "." Then
            TextEdit17.EditValue = "00.10"
            SelectNextControl(TextEdit18, True, True, True, True)
        End If
    End Sub
    Private Sub TextEdit18_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextEdit18.KeyUp
        If Len(TextEdit18.Text) = 1 And TextEdit18.Text = "." Then
            TextEdit18.EditValue = "00.10"
            SelectNextControl(TextEdit20, True, True, True, True)
        End If
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Lettura = True Then Exit Sub
        POSRIG = DsFat.Tables(Fa).Rows.Count
        RicalcoloFinale(POSRIG + 1)
        RegistraFattura()
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
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
        If e.KeyData = Keys.F6 Then
            e.Handled = True
            ButtonF6.PerformClick()
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

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit1.SelectedIndexChanged
        AP = ImageComboBoxEdit1.EditValue
        If AP > 0 Then
            ArticoloIniziale()
        End If
        ImageComboBoxEdit2.Focus()
    End Sub
    Sub ArticoloIniziale()
        Dim StrReg As String = "SELECT * from TbArtP where ArtPId = " & AP & " Order By ArtPprog"
        Dim Cmd As New SqlCommand(StrReg, cnCo)
        Dim x As Int16 = 0
        OLDCSP = 0
        OkArt = False
        InizializzaTableFat("SELECT * FROM vh4 where PriNumProt = 989898989898")
        'RileggoUltimi() modificato in data 060405
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            x = x + 1
            RwFat = DsFat.Tables(Fa).NewRow
            RwFat("PriProg") = x
            RwFat("PriCodIva") = 0
            RwFat("PriRegIva") = 0
            RwFat("PriNumProt") = 0 'MaxArt + 1
            TextEdit1.EditValue = "" 'MaxArt + 1
            RwFat("PriImpDare") = 0
            RwFat("PriImpavere") = 0
            RwFat("IMPORTO") = 0
            RwFat("PriCoDare") = dataRd.Item("ArtPDare")
            RwFat("PriCoAvere") = dataRd.Item("ArtPAvere")
            RwFat("PriIvaPrint") = 0
            RwFat("PriGstampa") = 0
            RwFat("PriCodPag") = 0
            RwFat("PriCausale") = dataRd.Item("ArtPCausale")
            RwFat("PriDesc") = Trim(dataRd.Item("ArtPDesc1"))
            RwFat("PriDescB") = Trim(dataRd.Item("ArtPDesc2"))
            RwFat("PriDocEst") = 0
            RwFat("PriDocAnn") = CDate(DateEdit2.EditValue).Year
            RwFat("PriNsRif") = ""
            RwFat("PridataGio") = DateEdit1.EditValue
            RwFat("PridataEst") = DateEdit2.EditValue
            IniziaRwFat()
            RwFat("PriId") = 0
            DsFat.Tables(Fa).Rows.Add(RwFat)
            OkArt = True
        End While
        dataRd.Close()
        For x = 1 To DsFat.Tables(Fa).Rows.Count
            RwFat = DsFat.Tables(Fa).Rows(x - 1)
            LeggiConto(RwFat("PriCoDare"), TextEdit16)
            LeggiConto(RwFat("PriCoAvere"), TextEdit19)
            RwFat("DAREDESC") = TextEdit16.EditValue
            RwFat("AVEREDESC") = TextEdit19.EditValue
            If x = DsFat.Tables(Fa).Rows.Count Then
                'Dim OldTrue As Boolean = GroupBox4.Enabled
                'GroupBox4.Enabled = True
                GridControl1.DataSource = DsFat.Tables(Fa)
                GridControl1.Refresh()
                GridView1.ClearSelection()
                'GroupBox4.Enabled = OldTrue
                CaricaDati()
            End If
        Next
    End Sub
    Private Sub ButtonF7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEXP.Click
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
    Private Sub TextEdit3_Leave(sender As Object, e As System.EventArgs) Handles TextEdit3.Leave
        If Val(TextEdit3.EditValue) < 0 Or Val(TextEdit3.EditValue) > 2030 Then
            TextEdit3.Focus()
        End If
    End Sub
End Class