Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.XtraEditors
Imports System.Collections

Public Class DxTrFtXc
    Dim IDBLK, ProgId, MiglioFo, ProgMc As Int32
    Dim RegFat, TIPOREG, TaiCauInc, TaiCauSconti, TaiCauAbb, TaiCauGiroc, TaiCauFatt, TaiPagato, TaiPagatoDet As Int16
    Dim TaiCptCassa, TaiCptScOmaggi, TaiCptScCond, TaiCptAbb, TaiCptBolle As String
    Dim TaiScartoMin, TaiScartoMax As Decimal

    Dim PNotaDat As Date
    Dim PNotaCau As Int16
    Dim PNotaAnno As Int16 = 0
    Dim PNotaImp As Decimal
    Dim PNotaCpt, PNotaDes, PNotaCon As String
    Dim PNotaMaxDat As Date

    Dim FinoAl, IvaCpt As String


    Dim Cc As String = "FATT"
    Dim TbDcg As DataTable
    Dim DaDcg As SqlDataAdapter
    Dim RiW As DataRow

    Dim Ac As String = "ACCO"
    Dim DsDac As DataSet
    Dim DaDac As SqlDataAdapter
    Dim RiC As DataRow

    Dim Ab As String = "ABBU"
    Dim DsDab As DataSet
    Dim DaDab As SqlDataAdapter
    Dim Rib As DataRow

    Dim Ci As String = "CIVA"
    Dim DsCii As DataSet
    Dim DaCii As SqlDataAdapter
    Dim RwCii As DataRow

    Dim Ri As String = "REGI"
    Dim DsReg As DataSet
    Dim DaReg As SqlDataAdapter
    Dim RwReg As DataRow

    Dim Mc As String = "VMCC"
    Dim DsMcc As DataSet
    Dim DaMcc As SqlDataAdapter
    Dim RwMcc As DataRow

    REM ARCHIVIAZIONE OTTICA
    Dim TbDoc As DataTable
    Dim DaDoc As SqlDataAdapter
    Dim RwDoc As DataRow

    Dim TbFat As DataTable
    Dim DaFat As SqlDataAdapter
    Dim RwFat As DataRow
    REM ARCHIVIAZIONE OTTICA

    Dim Rc(10), Cau(10) As Int16
    Dim Cassa(10), RegDesc(10), CAUSALE, CONTO As String
    Dim OkUpdate, OkCorris, OkFrascheri, OkRivGel, OkDefend, OkRicambi, OkRosine, OkGs, OkPn, OkMondo, OkSelco, OkOttica, OkZeroFt, OkGruppoPasta As Boolean
    Dim UserId As String
    Dim BOXCAUSALE As New TextEdit
    Dim BOXCONTO As New TextEdit
    Dim Rispondi As MsgBoxResult

    Dim RegNcr As Int16 = 0

    Dim PathPrg, PathSto, PathTmp As String

    REM IVA PUBBLICA AMMINISTRAZIONE
    Dim ArtPNota As Int16 = 0
    Dim CiiPam As New ArrayList
    Dim oKPubAmm As Boolean = False

    ''DEFENDINI CASSETTE GTT
    Dim DirGTT, DirBackup, FilGTT, FilBackup As String
    Dim ifiles() As String
    Dim DaTrasf, DaErr As SqlDataAdapter
    Dim TbTrasf, TbErr As DataTable
    Dim ErrBl, CbTrasf As SqlCommandBuilder
    ''DEFENDINI CASSETTE GTT

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

    Dim MMCwrite As String = "INSERT INTO TbMCC (MCCID,MCCPROG,MCCPrkAammgg,MCCPrkId,MCCPrkProg,MCCPrkDa,MCCCogLdp,MCCCogCdc,MCCCogRep,MCCCogDet,MCCCogConto,MCCIMPORTO) " _
& " values(@MCCID,@MCCPROG,@MCCPrkAammgg,@MCCPrkId,@MCCPrkProg,@MCCPrkDa,@MCCCogLdp,@MCCCogCdc,@MCCCogRep,@MCCCogDet,@MCCCogConto,@MCCIMPORTO)"
    Dim Mmd As New SqlCommand(MMCwrite, CnDc)
    Dim mp1 As New SqlParameter("@MCCID", SqlDbType.Int)
    Dim mp2 As New SqlParameter("@MCCPROG", SqlDbType.SmallInt)
    Dim mp3 As New SqlParameter("@MCCPrkAammgg", SqlDbType.SmallDateTime)
    Dim mp4 As New SqlParameter("@MCCPrkId", SqlDbType.Int)
    Dim mp5 As New SqlParameter("@MCCPrkProg", SqlDbType.SmallInt)
    Dim mp6 As New SqlParameter("@MCCPrkDa", SqlDbType.VarChar)
    Dim mp7 As New SqlParameter("@MCCCogLdp", SqlDbType.SmallInt)
    Dim mp8 As New SqlParameter("@MCCCogCdc", SqlDbType.VarChar)
    Dim mp9 As New SqlParameter("@MCCCogRep", SqlDbType.SmallInt)
    Dim mp9a As New SqlParameter("@MCCCogDet", SqlDbType.SmallInt)
    Dim mp10 As New SqlParameter("@MCCCogConto", SqlDbType.VarChar)
    Dim mp11 As New SqlParameter("@MCCIMPORTO", SqlDbType.Decimal)

    Private Sub DxTrFtXc_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ' DateEdit1.EditValue = Today.ToShortDateString
        DateEdit1.EditValue = Today
        TabelleIniziali()
    End Sub
    Sub TabelleIniziali()
        Cmd = New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            OkCorris = CBool(dataRd.Item("Sel15"))
            UserId = dataRd.Item("Sel14")
        End While
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
        oKPubAmm = False
        ' IVA PUBBLICA AMMINISTRAZIONE
        ArtPNota = 0
        Cmd = New SqlCommand("select * from TbPaCii where PaTipo = 'PNO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ArtPNota = dataRd.Item("PaCodIva")
        End While
        dataRd.Close()
        CiiPam.Clear()
        Cmd = New SqlCommand("SELECT * from TbPaCii where PaTipo = 'IVA' order by PaCodIva", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CiiPam.Add(dataRd.Item("PaCodIva"))
        End While
        dataRd.Close()
        If ArtPNota > 0 And CiiPam.Count > 0 Then oKPubAmm = True
        IvaCpt = "99.99"
        DsCii = New DataSet(Ci)
        DaCii = New SqlDataAdapter("SELECT * from TbCii order by CiiCod", cnCo)
        DaCii.Fill(DsCii, Ci)
        OkFrascheri = False
        OkRivGel = False
        OkDefend = False
        OkRicambi = False
        OkGs = False
        OkMondo = False
        OkSelco = False
        OkOttica = False
        OkGruppoPasta = False
        OkZeroFt = True
        LeggitaiGenerico()
        If UserId = "SELCO" Then OkSelco = True
        If UserId = "MONDOMARINE" Then OkMondo = True
        If UserId = "PASTAECO" Then OkOttica = True : OkZeroFt = True : OkGruppoPasta = True
        If UserId = "PASTANEW" Then OkOttica = True : OkZeroFt = True : OkGruppoPasta = True
        If UserId = "PASTAGROUP" Then OkOttica = True : OkZeroFt = True : OkGruppoPasta = True
        If UserId = "GSSPA" Then OkGs = True
        If UserId = "PENSIONATO" Then OkRosine = True
        If UserId = "FRASCHERI" Then LeggiTaiFrascheri()
        If UserId = "RIVIERAGEL" Then LeggiTaiRivGel()
        If UserId = "DEFENDINI" Then LeggiTaiDefend()
        If UserId = "RICAMBI" Then LeggiTaiRicambi()
        If OkCorris = True Then VerificaCorrispettivi()
        If UserId = "CSABOX" Then OkOttica = True : OkZeroFt = True
        PNotaMaxDat = CDate("01/01/2000")

    End Sub
    Sub LeggitaiGenerico()
        Dim cmd As New SqlCommand("SELECT top 1 * FROM TbTai Order by TaiAnno Desc", cnDb)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            RegNcr = dataRd.Item("TaiReg11")
        End While
        dataRd.Close()
    End Sub
    Sub LeggiTaiRicambi()
        Dim cmd As New SqlCommand("SELECT top 1 * FROM TbTai Order by TaiAnno Desc", cnDb)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            TaiCauInc = dataRd.Item("TaiCauInc")
            TaiCptCassa = dataRd.Item("TaiCptCassa")
        End While
        dataRd.Close()
        OkRicambi = True
    End Sub
    Sub LeggiTaiDefend()
        Dim cmd As New SqlCommand("SELECT top 1 * FROM TbTai Order by TaiAnno Desc", cnDb)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            TaiCauInc = dataRd.Item("TaiCauInc")
            TaiCptCassa = dataRd.Item("TaiCptCassa")
        End While
        dataRd.Close()
        OkDefend = True
    End Sub
    Sub LeggiTaiRivGel()
        Dim cmd As New SqlCommand("SELECT top 1 * FROM TbTai Order by TaiAnno Desc", cnDb)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            TaiCauInc = dataRd.Item("TaiCauInc")
            TaiCauFatt = dataRd.Item("TaiCauFatt")
            TaiCauGiroc = dataRd.Item("TaiCauGiroc")
            TaiCauSconti = dataRd.Item("TaiCauSconti")
            TaiCptAbb = dataRd.Item("TaiCptSconOm")
            TaiCptCassa = dataRd.Item("TaiCptCassa")
            TaiCptBolle = dataRd.Item("TaiCptBolle")
            TaiPagato = dataRd.Item("TaiPagato")
            TaiPagatoDet = dataRd.Item("TaiPagatoDet")
        End While
        dataRd.Close()
        OkRivGel = True
    End Sub
    Sub LeggiTaiFrascheri()
        Dim cmd As New SqlCommand("SELECT * FROM TbTai Where TaiCod =  1", cnDb)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            RegFat = dataRd.Item("TaiReg2")
            TaiCauInc = dataRd.Item("TaiCauInc")
            TaiCauSconti = dataRd.Item("TaiCauSconti")
            TaiCauAbb = dataRd.Item("TaiCauAbb")
            TaiCptCassa = dataRd.Item("TaiCptCassa")
            TaiCptScOmaggi = dataRd.Item("TaiCptScOmaggi")
            TaiCptScCond = dataRd.Item("TaiCptScCond")
            TaiCptAbb = dataRd.Item("TaiCptAbb")
            TaiScartoMin = Format(dataRd.Item("TaiScartoMin"), "00.00")
            TaiScartoMax = Format(dataRd.Item("TaiScartoMax"), "00.00")
        End While
        dataRd.Close()
        OkFrascheri = True
    End Sub
    Sub FattureDsDcg()
        OkUpdate = False
        Dim StrUno As String
        Dim K, Ri As Int32
        FinoAl = CDate(DateEdit1.EditValue).ToShortDateString
        StrUno = "Select * from TbDcg where DcgRegistro > 0 and DcgTipo = 'F' and DcgTrasf = 0 and DcgNumero > 0 and DcgData <= '" & FinoAl & "' Order By  DcgRegistro,DcgData,DcgNumero"
        IvaCpt = "99.99"
        TbDcg = New DataTable
        DaDcg = New SqlDataAdapter(StrUno, cnDb)
        ' If OkGs = True Then DaDcg = New SqlDataAdapter(StrUno, CnDbSede)
        DaDcg.Fill(TbDcg)
        If TbDcg.Rows.Count <= 0 Then Exit Sub
        Ri = 0
        Cursor.Current = Cursors.WaitCursor
        For K = 1 To TbDcg.Rows.Count
            RiW = TbDcg.Rows(K - 1)
            If Ri <> RiW("DcgRegistro") Then
                Ri = RiW("DcgRegistro")
                ContoIva()
            End If
            If TIPOREG <> 1 And TIPOREG <> 3 Then GoTo IInext
            If DoppiaFtFo() = False Then
                TextEdit1.Text = "FATTURA N. " & Format(RiW("DcgNumero"), "000000") & " DEL " & CDate(RiW("DcgData")).ToShortDateString
                TextEdit1.Refresh()
                ' Application.DoEvents()
                If OkZeroFt = False Then
                    If RiW("DcgRieTotFat") <> 0 Then
                        ScriviVendite()
                    End If
                Else
                    ScriviVendite()
                End If
            Else
                BloccoDoppiaFattura()
            End If
IInext:
        Next
    End Sub
    Sub BloccoDoppiaFattura()
        Dim UpWr As String
        If OkUpdate = False Then
            OkUpdate = True
            IDBLK = semaforo("TRASF.COGE AL " & FinoAl)
        End If
        UpWr = "INSERT INTO TBBLOCK (IDBLOCCO,IDRIFERIM,IDNUMREG) VALUES(" & IDBLK & "," & RiW("DcgNumRif") & ", 0)"
        Dim Ulb As New SqlCommand(UpWr, cnDb)
        ''  If OkGs = True Then Ulb = New SqlCommand(UpWr, CnDbSede)
        Ulb.ExecuteNonQuery()
    End Sub
    Sub AggiornaUpg()
        If OkFrascheri = True Then AbbuoniLimite()
        If OkRivGel = True Then PerRivGel()
        If OkDefend = True Then
            IncassiContoPoste()
            'IncassiGTT()
        End If
        If IDBLK = 0 Then Exit Sub
        Dim par As String
        par = "EXEC UPCOGE @BLOK = " & IDBLK
        Dim DSP As New SqlCommand(par, cnDb)
        ''   If OkGs = True Then DSP = New SqlCommand(par, CnDbSede)
        DSP.ExecuteNonQuery()
    End Sub
    Sub IncassiGTT()
        Dim DrGTT As New SqlCommand("Select * from TbSel where selid = 1", cnVd)
        Dim StrTre(3) As String
        Dim i As Int16 = 0
        dataRd = DrGTT.ExecuteReader
        If dataRd.Read Then
            DirGTT = dataRd.Item("sel12") & "GTT\"
            DirBackup = dataRd.Item("sel12") & "GTT\BACK\"
        End If
        dataRd.Close()
        If Directory.Exists(DirGTT) = False Then Exit Sub
        DaTrasf = New SqlDataAdapter("select * from TMPGTT where TMPCASSETTE = 32000", cnDb) ''' INIZIALIZZO VUOTO
        DaErr = New SqlDataAdapter("select * from ERRGTT", cnDb)
        ErrBl = New SqlCommandBuilder(DaErr)
        TbTrasf = New DataTable("TRASF")
        DaTrasf.Fill(TbTrasf)
        TbErr = New DataTable("ERR")
        DaErr.Fill(TbErr)
        ifiles = Directory.GetFiles(DirGTT, "*.txt")
        If ifiles.Length = 0 Then GoTo INext
        For i = 0 To ifiles.Length - 1
            FilGTT = ifiles(i)
            LeggiFile(FilGTT)
        Next
        If TbErr.Rows.Count > 0 Then
            MessageBox.Show("ERRORE DI LETTURA TELEFONARE ALLA SELCO", "ESITO ELABORAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        TbTrasf.AcceptChanges()

        For i = 0 To ifiles.Length - 1
            FilGTT = ifiles(i)
            FilBackup = DirBackup & Mid(ifiles(i), Len(DirGTT) + 1, 25)
            File.Copy(FilGTT, FilBackup, True)
            File.Delete(FilGTT)
        Next
        Dim RwTrasf As DataRow
        StrTre(0) = "INSERT INTO TMPGTT (TmpCassette,TmpData,TmpGiro,TmpImporto,TmpTrasf) VALUES (@TmpCassette,@TmpData,@TmpGiro,@TmpImporto,@TmpTrasf)"
        Dim Mmd As New SqlCommand(StrTre(0), cnDb)
        Dim p1 As New SqlParameter("@TmpCassette", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@TmpData", SqlDbType.SmallDateTime)
        Dim p3 As New SqlParameter("@TmpGiro", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@TmpImporto", SqlDbType.Decimal)
        Dim p5 As New SqlParameter("@TmpTrasf", SqlDbType.SmallInt)
        For i = 1 To TbTrasf.Rows.Count
            RwTrasf = TbTrasf.Rows(i - 1)
            Mmd.Parameters.Clear()
            p1.Value = RwTrasf("TmpCassette")
            p2.Value = RwTrasf("TmpData")
            p3.Value = RwTrasf("TmpGiro")
            p4.Value = RwTrasf("TmpImporto")
            p5.Value = RwTrasf("TmpTrasf")
            Mmd.Parameters.Add(p1)
            Mmd.Parameters.Add(p2)
            Mmd.Parameters.Add(p3)
            Mmd.Parameters.Add(p4)
            Mmd.Parameters.Add(p5)
            Mmd.ExecuteNonQuery()
        Next
INext:
        Dim NumDoc As Int32
        Dim TaiCauGtt As Int16
        Dim TaiCptGTTD, TaiCptGTTA, TaiCptGTTV As String
        TaiCptGTTD = "27.60" : TaiCauGtt = 16 : TaiCptGTTA = "52.60" : TaiCptGTTV = "52.61"
        StrTre(0) = "SELECT * FROM TMPGTT WHERE TmpTrasf = 0 and  TmpData <= '" & FinoAl & "'"
        StrTre(1) = "UPDATE TMPGTT SET TmpTrasf = 1 WHERE TmpTrasf = 0 and TmpData <= '" & FinoAl & "'"
        Dim K As Int16
        Dim Dado As String
        DsDac = New DataSet
        DaDac = New SqlDataAdapter(StrTre(0), cnDb)
        DaDac.Fill(DsDac, Ac)
        If DsDac.Tables(Ac).Rows.Count <= 0 Then Exit Sub
        For K = 1 To DsDac.Tables(Ac).Rows.Count
            RiC = DsDac.Tables(Ac).Rows(K - 1)
            Dado = CDate(RiC("TmpData")).ToShortDateString
            PNotaAnno = CDate(RiC("TmpData")).Year
            PNotaDes = Trim(RiC("TmpGiro").ToString.ToUpper)
            PNotaCau = TaiCauGtt
            PNotaCpt = TaiCptGTTD
            If PNotaDes = "VENARIA" Then PNotaCon = TaiCptGTTV Else PNotaCon = TaiCptGTTA
            PNotaImp = RiC("TmpImporto")
            PNotaDat = RiC("TmpData")
            LeggiUltimo(PNotaDat.ToShortDateString)
            NumDoc = RiC("TmpCassette")
            ScriviPrimaNota(PNotaCon, NumDoc)
        Next K
        Dim Dmd As New SqlCommand(StrTre(1), cnDb)
        Dmd.ExecuteNonQuery()
    End Sub
    Private Sub LeggiFile(ByVal nomefile As String)
        Dim input As TextReader = File.OpenText(nomefile)
        Dim line As String
        line = input.ReadLine()
        If line Is Nothing Then GoTo USCITA
        If line.Trim.Length = 0 Then GoTo USCITA
        AddRiga(line.Trim)
        While Not (line Is Nothing)
            line = input.ReadLine()
            If Not line Is Nothing Then
                AddRiga(line.Trim)
            End If
        End While
USCITA:
        input.Close()
    End Sub

    Private Sub AddRiga(ByVal line As String)
        'Dim Cassetta, lung As Int16
        Dim Cassetta As Int16
        Dim data As Date
        'Dim Giro, Errore As String
        Dim Giro As String
        Dim Importo As Decimal
        Dim rw As DataRow
        'Controllo dati
        If Len(line) <> 40 Then
            AddErrore(line, "Lunghezza riga incongruente (" & Len(line) & " caratteri)")
            Exit Sub
        End If
        If Not IsNumeric(line.Substring(0, 2)) Or Not IsNumeric(line.Substring(25, 5)) Or Not IsNumeric(line.Substring(2, 8)) Then
            AddErrore(line, "Dati di tipo incongruente !!")
            Exit Sub
        End If
        If Val(line.Substring(6, 4)) > Today.Year Or Val(line.Substring(6, 4)) < (Today.Year - 1) Then
            AddErrore(line, "Anno incongruente !!")
            Exit Sub
        End If
        If Val(line.Substring(4, 2)) > 12 Or Val(line.Substring(4, 2)) < 1 Then
            AddErrore(line, "Mese incongruente !!")
            Exit Sub
        End If
        Cassetta = line.Substring(0, 2)
        data = DateSerial(line.Substring(6, 4), line.Substring(4, 2), line.Substring(2, 2))
        Giro = line.Substring(10, 15)
        Importo = line.Substring(25, 15) / 100
        'Trasferimento
        rw = TbTrasf.NewRow
        rw("TmpCassette") = Cassetta
        rw("TmpData") = data
        rw("TmpGiro") = Giro
        rw("TmpImporto") = Importo
        rw("TmpTrasf") = 0
        TbTrasf.Rows.Add(rw)
    End Sub
    Private Sub AddErrore(ByVal line As String, ByVal Errore As String)
        Dim rw As DataRow
        rw = TbErr.NewRow
        rw("ErrFile") = Mid(FilGTT, Len(DirGTT) + 1, 25).Trim
        rw("ErrRiga") = line
        rw("ErrErrore") = Errore
        TbErr.Rows.Add(rw)
    End Sub
    Sub IncassiContoPoste()
        Dim StrTre(3), TaiCptPoste, TaiCptRend As String
        Dim NumDoc As Int32
        Dim TaiCauRend As Int16
        TaiCptPoste = "27.90" : TaiCauRend = 37 : TaiCptRend = "27.95"
        StrTre(0) = "SELECT SUM(DCGRIETOTFAT) as Incasso,DcgData FROM TBDCG WHERE (dcgtipo = 'P' OR (dcgtipo = 'R' and DCGREGISTRO = 102)) AND DCGTRASF = 0 AND DcgData <= '" & FinoAl & "' GROUP BY DCGDATA"
        StrTre(1) = "UPDATE TBDCG SET DCGTRASF = 1 WHERE (dcgtipo = 'P' OR (dcgtipo = 'R' and DCGREGISTRO = 102)) AND DcgData <= '" & FinoAl & "'"
        Dim K As Int16
        Dim Dado As String
        DsDac = New DataSet
        DaDac = New SqlDataAdapter(StrTre(0), cnDb)
        DaDac.Fill(DsDac, Ac)
        If DsDac.Tables(Ac).Rows.Count <= 0 Then GoTo NNEXT
        For K = 1 To DsDac.Tables(Ac).Rows.Count
            RiC = DsDac.Tables(Ac).Rows(K - 1)
            If RiC("Incasso") = 0 Then GoTo IINext
            Dado = CDate(RiC("DcgData")).ToShortDateString
            PNotaAnno = CDate(RiC("DcgData")).Year
            PNotaDes = ""
            PNotaCau = TaiCauInc
            PNotaCpt = TaiCptCassa
            PNotaCon = TaiCptPoste
            PNotaImp = RiC("Incasso")
            PNotaDat = RiC("DcgData")
            LeggiUltimo(PNotaDat.ToShortDateString)
            NumDoc = PNotaDat.Day * 100 + PNotaDat.Month
            ScriviPrimaNota(PNotaCon, NumDoc)
IINext:
        Next K
        Dim Dmd As New SqlCommand(StrTre(1), cnDb)
        Dmd.ExecuteNonQuery()
        'inizio rendiconti
NNEXT:
        StrTre(2) = "SELECT * FROM TBDCG WHERE dcgtipo = 'R' and DCGREGISTRO = 105 AND DCGTRASF = 0 AND DcgData <= '" & FinoAl & "'"
        StrTre(3) = "UPDATE TBDCG SET DCGTRASF = 1 WHERE dcgtipo = 'R' and DCGREGISTRO = 105 AND DcgData <= '" & FinoAl & "'"
        DsDac = New DataSet
        DaDac = New SqlDataAdapter(StrTre(2), cnDb)
        DaDac.Fill(DsDac, Ac)
        If DsDac.Tables(Ac).Rows.Count <= 0 Then Exit Sub
        For K = 1 To DsDac.Tables(Ac).Rows.Count
            RiC = DsDac.Tables(Ac).Rows(K - 1)
            If RiC("DcgRieTotale") = 0 Then GoTo IIINext
            Dado = CDate(RiC("DcgData")).ToShortDateString
            PNotaAnno = CDate(RiC("DcgData")).Year
            PNotaDes = ""
            PNotaCau = TaiCauRend
            PNotaCpt = RiC("DcgCli")
            PNotaCon = TaiCptRend
            PNotaImp = RiC("DcgRieTotale")
            PNotaDat = RiC("DcgData")
            LeggiUltimo(PNotaDat.ToShortDateString)
            NumDoc = RiC("DcgNumero")
            ScriviPrimaNota(PNotaCon, NumDoc)
IIINext:
        Next K
        Dmd = New SqlCommand(StrTre(3), cnDb)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub PerRivGel()
        Dim StrTre As String = "Select * From TbInc Order by IncTipoDoc,IncClCod"
        Dim K As Int16
        Dim Dado As String
        DsDac = New DataSet
        DaDac = New SqlDataAdapter(StrTre, cnDb)
        DaDac.Fill(DsDac, Ac)
        If DsDac.Tables(Ac).Rows.Count <= 0 Then Exit Sub
        For K = 1 To DsDac.Tables(Ac).Rows.Count
            RiC = DsDac.Tables(Ac).Rows(K - 1)
            If RiC("IncIncasso") = 0 Then GoTo IINext
            If RiC("IncDataInc") Is DBNull.Value Then GoTo IINext
            If RiC("IncDatDoc") Is DBNull.Value Then
                Dado = ""
                PNotaAnno = 0
            Else
                Dado = CDate(RiC("IncDatDoc")).ToShortDateString
                PNotaAnno = CDate(RiC("IncDatDoc")).Year
            End If
            PNotaDes = ""
            If RiC("IncTipoDoc") = "B" Then
                PNotaCau = TaiCauInc
                PNotaCpt = TaiCptCassa
                PNotaCon = TaiCptBolle
                PNotaDes = "BOLLA " & RiC("IncNumDoc") & "-" & Dado
            ElseIf RiC("IncTipoDoc") = "F" And ((RiC("IncPagam") = TaiPagato And TaiPagato > 0) Or (RiC("IncPagam") = TaiPagatoDet And TaiPagatoDet > 0)) Then
                PNotaCau = TaiCauGiroc
                PNotaCpt = TaiCptBolle
                PNotaCon = RiC("IncClCod")
            Else
                PNotaCau = TaiCauFatt
                PNotaCpt = TaiCptCassa
                PNotaCon = RiC("IncClCod")
                PNotaDes = "FATT. " & RiC("IncNumDoc") & "-" & Dado
            End If
            PNotaImp = RiC("IncIncasso")
            PNotaDat = RiC("IncDataInc")
            LeggiUltimo(PNotaDat.ToShortDateString)
            ScriviPrimaNota(PNotaCon, RiC("IncNumDoc"))
IINext:
        Next K
        Dim Elimina As String = "DELETE FROM TbInc"
        Dim Dmd As New SqlCommand(Elimina, cnDb)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub RileggoUltimi()
        ' LEGGO SOLO I RGISTRI IVA DI TIPO 5 (CORRISPETTIVI)
        Dim Str As String = "Select * from FnFotoRIva(" & CDate(FinoAl).Year & ") Where RivaTipo = 5 "
        DsReg = New DataSet(Ri)
        DaReg = New SqlDataAdapter(Str, cnCo)
        DaReg.Fill(DsReg, Ri)
        Dim P As Int16
        '' AGGIUNGO I REG.IVA NON MOVIMENTATI
        Dim StrReg As String = "SELECT * from TbRegIva where RivaTipo = 5 and RivaAnno = " & CDate(FinoAl).Year
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
    Sub CorrispDcg()
        Dim SqlS As String
        If OkRosine = False Then SqlS = "ORIZZDCG" Else SqlS = "XORIZZDCG"
        Dim Command As New SqlClient.SqlCommand(SqlS)
        Command.CommandType = CommandType.StoredProcedure
        Command.Connection = cnDb
        Dim q1 As New SqlParameter("@FINOAL", SqlDbType.SmallDateTime)
        Dim q2 As New SqlParameter("@CDC", SqlDbType.VarChar)
        q1.Value = FinoAl
        If OKCDC = True Then q2.Value = "S" Else q2.Value = "N"
        Command.Parameters.Clear()
        Command.Parameters.Add(q1)
        Command.Parameters.Add(q2)
        Command.CommandTimeout = 300
        Command.ExecuteNonQuery()
        Dim Ri As Int32
        Ri = 0
        Dim StrUno As String = "SELECT REG,DATA,CI,SUM(IMPON)AS IMPON,SUM(IVA)AS IVA,CPT FROM ##CORRISP GROUP BY REG,DATA,CI,CPT ORDER BY REG,DATA,CI,CPT"
        If OkRosine = True Then StrUno = "SELECT REG,DATA,CI,SUM(IMPON)AS IMPON,SUM(IVA)AS IVA,CPT,PAG FROM ##CORRISP GROUP BY REG,DATA,PAG,CI,CPT ORDER BY REG,DATA,PAG,CI,CPT"
        TbDcg = New DataTable
        DaDcg = New SqlDataAdapter(StrUno, cnDb)
        DaDcg.Fill(TbDcg)
        If TbDcg.Rows.Count <= 0 Then Exit Sub
        RileggoUltimi()
        Cursor.Current = Cursors.WaitCursor
        ScriviCorrisp()
    End Sub
    Sub AssegnaProtCorr()
        Dim P As Int16
        For P = 1 To DsReg.Tables(Ri).Rows.Count
            RwReg = DsReg.Tables(Ri).Rows(P - 1)
            If RwReg("RivaNReg") = RiW("REG") Then Exit For
        Next
        For P = 1 To Rc(0)
            If Rc(P) = RiW("REG") Then
                CAUSALE = Cau(P)
                CONTO = Cassa(P)
                Exit Sub
            End If
        Next
    End Sub
    Sub ScriviCorrisp()
        Dim Scrivi As String = "INSERT INTO TbPri (PriId,PriProg,PriDataGio, PriCausale, PriCoDare, PriCoAvere, PriNumProt, PriBisRet, PriCodIva, PriRegIva, PriImpDare, PriImpavere, PriDesc, PriDocEst, PriMeseSk, PriDataEst, PriDescB, PriFl04, PriFl05, PriFl06, PriNsRif, PriSos, PriLinea, PriDocAnn, PriCodPag, PriValuta, PriArtFisc,PriIvaPrint,PriGStampa) " _
& " values(@PriId,@PriProg,@PriDataGio, @PriCausale, @PriCoDare, @PriCoAvere, @PriNumProt, @PriBisRet, @PriCodIva, @PriRegIva, @PriImpDare, @PriImpavere, @PriDesc, @PriDocEst, @PriMeseSk, @PriDataEst, @PriDescB, @PriFl04, @PriFl05, @PriFl06, @PriNsRif, @PriSos, @PriLinea, @PriDocAnn, @PriCodPag, @PriValuta, @PriArtFisc,@PriIvaPrint,@PriGStampa)"
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
        'inserire controllo doppia registrazione data = data reg iva corrispetti
        Dim M, Ri, k, PAG As Int16
        Dim Prot As Int32
        Dim LaData, UData As String
        Dim OkReg As Boolean = False
        Dim OkLeggi As Boolean = False
        M = 0 : Ri = 0
        LaData = "01/01/2000"
        For k = 1 To TbDcg.Rows.Count
            RiW = TbDcg.Rows(k - 1)
            TextEdit1.Text = "INCASSI DEL  " & CDate(RiW("DATA")).ToShortDateString
            Application.DoEvents()
            Cmd.Parameters.Clear()
            If RiW("IMPON") + RiW("IVA") = 0 Then
                GoTo VaiOltre
            End If
            If Ri <> RiW("REG") Then
                AssegnaProtCorr()
                Prot = RwReg("ProtCar")
                LaData = "01/01/2000"
                If OkRosine = True Then
                    PAG = RiW("PAG")
                    CONTO = "05.51"
                    Select Case PAG
                        Case 17
                            ' in data 10/06/2009 segnalano nuova banca 
                            ' CONTO = "05.01"
                            CONTO = "05.03"
                        Case 18
                            CONTO = "05.51"
                        Case 19
                            CONTO = "05.02"
                        Case 23
                            CONTO = "05.02"
                    End Select
                End If
            End If
            If OkRosine = False Then GoTo NoRosine
Rosine:

            If CDate(RiW("DATA")) <> CDate(LaData) Or RiW("PAG") <> PAG Then
                If OkReg = True Then
                    EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
                    ResetIdP()
                End If
                LeggiUltimo(RiW("DATA"))
                OkLeggi = True
                M = 0
                Prot = Prot + 1
                LaData = CDate(RiW("DATA")).ToShortDateString
                UData = CDate(RiW("DATA")).ToShortDateString
                Ri = RiW("REG")
                PAG = RiW("PAG")
            End If

            GoTo OltreRosine
NoRosine:
            If CDate(RiW("DATA")) <> CDate(LaData) Then
                If OkReg = True Then
                    EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
                    ResetIdP()
                End If
                LeggiUltimo(RiW("DATA"))
                OkLeggi = True
                M = 0
                Prot = Prot + 1
                LaData = CDate(RiW("DATA")).ToShortDateString
                UData = CDate(RiW("DATA")).ToShortDateString
                Ri = RiW("REG")
            End If
OltreRosine:
            M = M + 1
            p1.Value = RiW("DATA")
            p2.Value = CAUSALE
            If OkRosine = True Then
                PAG = RiW("PAG")
                CONTO = "05.51"
                Select Case PAG
                    Case 17
                        CONTO = "05.03"
                    Case 18
                        CONTO = "05.51"
                    Case 19
                        CONTO = "05.02"
                    Case 23
                        CONTO = "05.02"
                End Select
            End If
            p3.Value = CONTO
            p4.Value = RiW("CPT")
            p5.Value = Prot
            p6.Value = ""
            p7.Value = RiW("CI")
            p8.Value = RiW("REG")
            p9.Value = RiW("IMPON") + RiW("IVA")
            p10.Value = RiW("IMPON") + RiW("IVA")
            p11.Value = ""
            p12.Value = Prot
            p13.Value = ""
            p14.Value = RiW("DATA")
            p15.Value = ""
            p16.Value = 0
            p17.Value = 0
            p18.Value = 0
            p19.Value = ""
            p20.Value = ""
            p21.Value = ""
            p22.Value = CDate(RiW("DATA")).Year
            p23.Value = 0
            p24.Value = 0
            p25.Value = 0
            p26.Value = ProgId
            p27.Value = M
            p28.Value = 0
            p29.Value = 0
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
            If OKCDC = True Then
                CorrispMCC(CDate(RiW("DATA")).ToShortDateString, RiW("REG"), RiW("CI"), RiW("CPT"), M, OkLeggi)
                OkLeggi = False
            End If
            OkReg = True
VaiOltre:
        Next
        If OkUpdate = False Then
            OkUpdate = True
            IDBLK = semaforo("TRASF.COGE AL " & FinoAl)
        End If
        If OkReg = True Then
            EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
            ResetIdP()
            Dim UpWr As String = "INSERT INTO TBBLOCK (IDBLOCCO,IDRIFERIM,IDNUMREG) SELECT distinct " & IDBLK & " ,RIF,0 FROM ##CORRISP"
            Dim Ulb As New SqlCommand(UpWr, cnDb)
            Ulb.ExecuteNonQuery()
        End If
    End Sub
    Function LeggiMaxMcc() As Int16
        Dim UltimaRiga As New SqlCommand("Select isnull(max(mccprog),0) from TBMCC WHERE MCCID = " & ProgMc, CnDc)
        LeggiMaxMcc = UltimaRiga.ExecuteScalar
    End Function
    Function CorrispMCC(ByVal DDATA As String, ByVal REG As Int16, ByVal CIVA As Int16, ByVal CCPT As String, ByVal PROG As Int16, ByVal OkUltimo As Boolean) As Boolean
        Dim StrS As String = "SELECT DCGREGISTRO,DCGDATA,DCGMCCogLdp,DCGMCCogCdc,DCGMCCogRep,SUM(DCGMCIMPORTO) AS DCGMCIMPORTO,DCGMCCogConto " _
             & "  from ##corrisp left outer join ##corrcdc on dcgmcid = rif and cpt = DCGMCCogConto WHERE DCGDATA = '" _
             & DDATA & "' And DCGREGISTRO = " & REG & " And Ci = " & CIVA & " AND CPT = '" & CCPT & "'"
        Dim StrG As String = " GROUP BY DCGREGISTRO,DCGDATA, CI,CPT,DCGMCCogLdp,DCGMCCogCdc,DCGMCCogRep,DCGMCCogConto"
        Dim StrO As String = " ORDER BY DCGREGISTRO,DCGDATA,CI,CPT"
        Dim Oggi As String = Today.ToShortDateString
        Dim x, M As Int16
        DsMcc = New DataSet(Mc)
        DaMcc = New SqlDataAdapter(StrS & StrG & StrO, CnDc)
        DaMcc.Fill(DsMcc, Mc)
        If DsMcc.Tables(Mc).Rows.Count = 0 Then Exit Function
        If OkUltimo = True Then LeggiUltMCC(Oggi)
        M = LeggiMaxMcc()
        For x = 1 To DsMcc.Tables(Mc).Rows.Count
            RwMcc = DsMcc.Tables(Mc).Rows(x - 1)
            Mmd.Parameters.Clear()
            M = M + 1
            mp1.Value = ProgMc
            mp2.Value = M
            mp3.Value = DDATA
            mp4.Value = ProgId
            mp5.Value = PROG
            mp6.Value = 1
            mp7.Value = RwMcc("DCGMCCogLdp")
            mp8.Value = RwMcc("DCGMCCogcdc")
            mp9.Value = RwMcc("DCGMCCogRep")
            mp10.Value = RwMcc("DCGMCCogConto")
            mp11.Value = RwMcc("DCGMCIMPORTO")
            Mmd.Parameters.Add(mp1)
            Mmd.Parameters.Add(mp2)
            Mmd.Parameters.Add(mp3)
            Mmd.Parameters.Add(mp4)
            Mmd.Parameters.Add(mp5)
            Mmd.Parameters.Add(mp6)
            Mmd.Parameters.Add(mp7)
            Mmd.Parameters.Add(mp8)
            Mmd.Parameters.Add(mp9)
            Mmd.Parameters.Add(mp10)
            Mmd.Parameters.Add(mp11)
            Mmd.ExecuteNonQuery()
        Next
        ResetIdM()
    End Function
    Sub ContoIva()
        Dim StrReg As String = "SELECT RIvaCpt,RivaTipo from TbRegIva where RivaNreg = " & RiW("DcgRegistro") & " and RIvaAnno = " & CDate(RiW("DcgData")).Year
        Dim Cmd As New SqlCommand(StrReg, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            IvaCpt = dataRd.Item("RivaCpt")
            TIPOREG = dataRd.Item("RivaTipo")
        End While
        dataRd.Close()
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
        If dataGio > PNotaMaxDat Then PNotaMaxDat = dataGio
    End Function
    Sub ResetIdP()
        Dim Elimina As String = "DELETE FROM TbIDP WHERE IdPRI = " & ProgId
        Dim Dmd As New SqlCommand(Elimina, cnCo)
        Dmd.ExecuteNonQuery()
    End Sub
    Function LeggiCodiciIva(ByVal i As Int16) As String
        RwCii = DsCii.Tables(Ci).Rows(i - 1)
        Return RwCii("CiiDes")
    End Function
    Function DoppiaFtFo() As Boolean
        DoppiaFtFo = False
        Dim Str As String = "SELECT * from TBPRI WHERE PRICAUSALE = 3 AND PRINUMPROT = " & RiW("DcgNumero") _
  & " AND PRIREGIVA = " & RiW("DcgRegistro") & " AND PRIDOCANN = " & CDate(RiW("DcgData")).Year
        Dim FTFO As New SqlCommand(Str, cnCo)
        dataRd = FTFO.ExecuteReader
        While dataRd.Read
            DoppiaFtFo = True
        End While
        dataRd.Close()
    End Function
    Function TestDoppio() As Integer
        TestDoppio = RiW("DcgNumero")
        Dim Str As String = "SELECT * from TBPRI WHERE PRICAUSALE = 3 AND PRIDOCEST = " & RiW("DcgNumero") & "  AND PRIDOCANN = " & CDate(RiW("DcgData")).Year & " AND PRICODARE='" & RiW("DcgCli") & "'"
        Dim FTFO As New SqlCommand(Str, cnCo)
        dataRd = FTFO.ExecuteReader
        While dataRd.Read
            TestDoppio = RiW("DcgRegistro") * 100000 + RiW("DcgNumero")
        End While
        dataRd.Close()
    End Function
    Sub ScriviVendite()
        Dim x, M, Y As Int16
        Dim UpWr As String
        Dim StrMCC = "SELECT * FROM TBDMC WHERE DCGMCID = "
        Dim QuestaSi As Boolean = False
        Dim NN As Integer = 0

        Y = 0
        M = 12
        LeggiUltimo(RiW("DcgData"))
        For x = 1 To M
            If x < M Then
                If RiW("DcgIva" & x) = 0 And RiW("DcgImp" & x) = 0 Then GoTo Oltre
            End If
            Wrd.Parameters.Clear()
            Y = Y + 1
            p1.Value = RiW("DcgData")
            p3.Value = RiW("DcgCli")
            p5.Value = RiW("DcgNumero")
            p6.Value = ""
            p8.Value = RiW("DcgRegistro")
            p11.Value = "" 'RiW("PriDesc")
            NN = TestDoppio()
            p12.Value = NN
            p13.Value = "" 'RiW("PriMeseSk")
            p14.Value = RiW("DcgData")
            p15.Value = "" 'RiW("PriDescB")
            p16.Value = 0 'RiW("PriFl04")
            p17.Value = 0 'RiW("PriFl05")
            p18.Value = 0 'RiW("PriFl06")
            p19.Value = "" 'RiW("PriNsRif")
            p20.Value = "" 'RiW("PriSos")
            p21.Value = "" 'RiW("PriLinea")
            p22.Value = CDate(RiW("DcgData")).Year
            p23.Value = 0
            p24.Value = 0 'RiW("PriValuta")
            p25.Value = 0 'RiW("PriArtFisc")
            p26.Value = ProgId
            p27.Value = Y
            p28.Value = 0 'RiW("PriIvaPrint")
            p29.Value = 0 'RiW("PriGStampa")
            If x = M Then
                p2.Value = 3
                p4.Value = IvaCpt ''conto iva da registro
                p7.Value = RiW("DcgCodIva" & x - 1)
                p9.Value = RiW("DcgRieTotFat") 'TotaleFattura
                p10.Value = RiW("DcgRieTotIva") 'TotaleIva
                p23.Value = RiW("DcgCodPag")
                If Y > 2 Then p20.Value = "00.10" Else p20.Value = RiW("DcgCptCon1")
            Else
                p2.Value = 1
                p4.Value = RiW("DcgCptCon" & x)
                p7.Value = RiW("DcgCodIva" & x)
                p9.Value = RiW("DcgImp" & x)
                p10.Value = RiW("DcgIva" & x)
                If oKPubAmm = True Then
                    For w As Int16 = 1 To CiiPam.Count
                        If CiiPam(w - 1) = CInt(p7.Value) Then
                            QuestaSi = True
                            Exit For
                        End If
                    Next
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
Oltre:
        Next
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        AssegnaNPartita(NN)
        If RiW("DcgCodPag") > 0 Then EsegueSql(" EXEC XCREASCADENZE  @IDP = " & ProgId, cnCo)
        EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
        If OkUpdate = False Then
            OkUpdate = True
            IDBLK = semaforo("TRASF.COGE AL " & FinoAl)
        End If
        UpWr = "INSERT INTO TBBLOCK (IDBLOCCO,IDRIFERIM,IDNUMREG) VALUES(" & IDBLK & "," & RiW("DcgNumRif") & ", 0)"
        Dim Ulb As New SqlCommand(UpWr, cnDb)
        ''   If OkGs = True Then Ulb = New SqlCommand(UpWr, CnDbSede)
        Ulb.ExecuteNonQuery()
        If OKCDC = True And OkDefend = True Then VenditeMCC(StrMCC & RiW("DcgNumRif"), CDate(RiW("DcgData")).ToShortDateString)
        If QuestaSi = True Then PerPubblicaAmministrazione()
        If OkFrascheri = True Then PerFrascheri()
        If OkRivGel = True Then PerRivieraGel()
        If OkDefend = True Then PerDefendini()
        If OkRicambi = True Then PerRicambi()
        If OkMondo = True Then ScriviCDC() : AggiornoVisualDoc()
        If OkGruppoPasta = True Then ScriviCDCaZERO()
        If OkSelco = True Then AggiornoVisualDoc()
        If OkOttica = True Then AggiornoVisualDoc()
    End Sub
    Sub ScriviCDCaZERO()
        Dim M As Int16 = 0
        Dim P As Integer = 0
        Dim S As Int16 = 0
        Dim TbUFa As DataTable
        Dim DaUfa As SqlDataAdapter
        Dim RwUFa As DataRow
        TbUFa = New DataTable
        DaUfa = New SqlDataAdapter("exec XDADCG @ID = " & ProgId, CnDc)
        DaUfa.Fill(TbUFa)
        If TbUFa.Rows.Count = 0 Then Exit Sub
        '  Dim PCom As New SqlCommand("select isnull(FatComRif,0) from TbFat where FatRif = " & RiW("DcgNumRif"), cnDb)
        Dim QC As Integer = 0 ''PCom.ExecuteScalar

        ''  If QC = 0 Then Exit Sub

        LeggiUltMCC(RiW("DcgData"))


        For j As Int16 = 1 To TbUFa.Rows.Count
            RwUFa = TbUFa.Rows(j - 1)
            Mmd.Parameters.Clear()
            M = M + 1
            mp1.Value = ProgMc
            mp2.Value = M
            mp3.Value = RwUFa("PrkAAmmgg")
            mp4.Value = RwUFa("PrkId")
            mp5.Value = RwUFa("PrkProg")
            mp6.Value = RwUFa("PrkDa")
            mp7.Value = QC
            mp8.Value = ""
            mp9.Value = 0
            mp9a.Value = 0
            mp10.Value = RwUFa("PrkConto")
            mp11.Value = RwUFa("PriImpDare")
            Mmd.Parameters.Add(mp1)
            Mmd.Parameters.Add(mp2)
            Mmd.Parameters.Add(mp3)
            Mmd.Parameters.Add(mp4)
            Mmd.Parameters.Add(mp5)
            Mmd.Parameters.Add(mp6)
            Mmd.Parameters.Add(mp7)
            Mmd.Parameters.Add(mp8)
            Mmd.Parameters.Add(mp9)
            Mmd.Parameters.Add(mp9a)
            Mmd.Parameters.Add(mp10)
            Mmd.Parameters.Add(mp11)
            Mmd.ExecuteNonQuery()
        Next
        ResetIdM()
    End Sub
    Sub ScriviCDC()
        Dim M As Int16 = 0
        Dim P As Integer = 0
        Dim S As Int16 = 0
        Dim TbUFa As DataTable
        Dim DaUfa As SqlDataAdapter
        Dim RwUFa As DataRow
        TbUFa = New DataTable
        DaUfa = New SqlDataAdapter("exec XDADCG @ID = " & ProgId, CnDc)
        DaUfa.Fill(TbUFa)
        If TbUFa.Rows.Count = 0 Then Exit Sub
        Dim PCom As New SqlCommand("select isnull(FatComRif,0) from TbFat where FatRif = " & RiW("DcgNumRif"), cnDb)
        Dim QC As Integer = PCom.ExecuteScalar

        If QC = 0 Then Exit Sub

        LeggiUltMCC(RiW("DcgData"))


        For j As Int16 = 1 To TbUFa.Rows.Count
            RwUFa = TbUFa.Rows(j - 1)
            Mmd.Parameters.Clear()
            M = M + 1
            mp1.Value = ProgMc
            mp2.Value = M
            mp3.Value = RwUFa("PrkAAmmgg")
            mp4.Value = RwUFa("PrkId")
            mp5.Value = RwUFa("PrkProg")
            mp6.Value = RwUFa("PrkDa")
            mp7.Value = QC
            mp8.Value = ""
            mp9.Value = 0
            mp9a.Value = 0
            mp10.Value = RwUFa("PrkConto")
            mp11.Value = RwUFa("PriImpDare")
            Mmd.Parameters.Add(mp1)
            Mmd.Parameters.Add(mp2)
            Mmd.Parameters.Add(mp3)
            Mmd.Parameters.Add(mp4)
            Mmd.Parameters.Add(mp5)
            Mmd.Parameters.Add(mp6)
            Mmd.Parameters.Add(mp7)
            Mmd.Parameters.Add(mp8)
            Mmd.Parameters.Add(mp9)
            Mmd.Parameters.Add(mp9a)
            Mmd.Parameters.Add(mp10)
            Mmd.Parameters.Add(mp11)
            Mmd.ExecuteNonQuery()
        Next
        ResetIdM()
    End Sub
    Sub PerPubblicaAmministrazione()
        PNotaCau = ArtPNota
        PNotaCpt = IvaCpt
        PNotaDes = ""
        PNotaImp = RiW("DcgRieTotIva")
        PNotaDat = RiW("DcgData")
        LeggiUltimo(PNotaDat.ToShortDateString)
        ScriviPrimaNota(RiW("DcgCli"), RiW("DcgNumero"))
    End Sub
    Sub PerRicambi()
        If RiW("DcgRieTotAcc") <= 0 Then Exit Sub
        PNotaCau = TaiCauInc
        PNotaCpt = TaiCptCassa
        PNotaDes = ""
        PNotaImp = RiW("DcgRieTotAcc")
        PNotaDat = RiW("DcgData")
        LeggiUltimo(PNotaDat.ToShortDateString)
        ScriviPrimaNota(RiW("DcgCli"), RiW("DcgNumero"))
    End Sub
    Sub PerDefendini()
        If RiW("DcgRieTotAcc") <= 0 Then Exit Sub
        PNotaCau = TaiCauInc
        PNotaCpt = TaiCptCassa
        PNotaDes = ""
        PNotaImp = RiW("DcgRieTotAcc")
        PNotaDat = RiW("DcgData")
        LeggiUltimo(PNotaDat.ToShortDateString)
        ScriviPrimaNota(RiW("DcgCli"), RiW("DcgNumero"))
    End Sub
    Sub PerRivieraGel()
Omaggi:
        If RiW("DcgRieTotOma") = 0 Then Exit Sub
        PNotaImp = RiW("DcgRieTotOma")
        PNotaDat = CDate(RiW("DcgData"))
        PNotaAnno = 0
        AssegnoRigheSconti()
    End Sub
    Sub PerFrascheri()
Omaggi:
        If RiW("DcgRieTotOma") = 0 Then GoTo Sconti
        PNotaImp = RiW("DcgRieTotOma")
        PNotaDat = CDate(RiW("DcgData"))
        AssegnoRigheSconti()
Sconti:
        If RiW("DcgRieTotAcc") = 0 Then GoTo FineSconti
        If RiW("DcgRieScCond") = 0 Then GoTo Acconti
        PNotaImp = RiW("DcgRieScCond")
        PNotaDat = CDate(RiW("DcgData"))
        AssegnoRigheSconti()
Acconti:
        If RiW("DcgRegistro") = RegFat Then
            AccontiFattura()
        Else
            AccontiBolla()
        End If
FineSconti:
    End Sub
    Sub AbbuoniLimite()
        '''' IN ATTESA DI RISPORTA SE OK SEMPRE O SOLO PER LE FATTURA TRASFERITE ''''''
        Dim p1 As New SqlParameter("@MIN", SqlDbType.Decimal)
        Dim p2 As New SqlParameter("@MAX", SqlDbType.Decimal)
        Dim p3 As New SqlParameter("@MIGLIO", SqlDbType.Int)
        p1.Value = TaiScartoMin
        p2.Value = TaiScartoMax
        p3.Value = MiglioFo
        Dim StrQua As String = "Select * From fnSCONTIABBUONI(@MIN,@MAX,@MIGLIO)"
        Dim K As Int16
        DsDab = New DataSet
        DaDab = New SqlDataAdapter(StrQua, cnCo)
        DaDab.SelectCommand.Parameters.Clear()
        DaDab.SelectCommand.Parameters.Add(p1)
        DaDab.SelectCommand.Parameters.Add(p2)
        DaDab.SelectCommand.Parameters.Add(p3)
        DaDab.Fill(DsDab, Ab)
        If DsDab.Tables(Ab).Rows.Count <= 0 Then Exit Sub
        For K = 1 To DsDab.Tables(Ab).Rows.Count
            Rib = DsDab.Tables(Ab).Rows(K - 1)
            If Rib("DIFF") = 0 Then GoTo IINext
            PNotaAnno = Rib("PrkDocAnn")
            PNotaCau = TaiCauAbb
            PNotaCpt = TaiCptAbb
            PNotaImp = Rib("DIFF")
            PNotaDat = PNotaMaxDat '''' VIENE CAMBIATA DALL'ESTRAIDATAFATTURA
            If EstraiDatafattura() = False Then GoTo IINext
            LeggiUltimo(PNotaDat.ToShortDateString)
            ScriviPrimaNota(Rib("PrkConto"), Rib("PrkDocEst"))
IINext:
        Next K
    End Sub
    Function EstraiDatafattura() As Boolean
        EstraiDatafattura = False
        Dim cmd As New SqlCommand("select PRIDATAEST from TBPRI WHERE PRIDOCEST = " & Rib("PrkDocEst") & " and datepart(year,pridataest) = " & Rib("PrkDocAnn") & "  and pricausale = 3 and priCoDare ='" & Rib("PrkConto") & "'", cnCo)
        PNotaDes = "FATT. " & Rib("PrkDocEst") & "-" & Rib("PrkDocAnn")
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("PRIDATAEST") Is DBNull.Value Then Exit While
            PNotaDat = dataRd.Item("PRIDATAEST")
            EstraiDatafattura = True
            PNotaDes = "FATT. " & Rib("PrkDocEst") & "-" & CDate(dataRd.Item("PRIDATAEST")).ToShortDateString
        End While
        dataRd.Close()
    End Function
    Sub AccontiBolla()
        Dim StrTre As String = "Select * From TbBol where BolRifFat = " & RiW("DcgNumRif")
        Dim K As Int16
        DsDac = New DataSet
        DaDac = New SqlDataAdapter(StrTre, cnDb)
        DaDac.Fill(DsDac, Ac)
        If DsDac.Tables(Ac).Rows.Count <= 0 Then Exit Sub
        For K = 1 To DsDac.Tables(Ac).Rows.Count
            RiC = DsDac.Tables(Ac).Rows(K - 1)
            If RiC("BolAcconto") = 0 Then GoTo IINext
            If RiC("BolDataAcc") Is DBNull.Value Then GoTo IINext
            PNotaCau = TaiCauInc
            PNotaCpt = TaiCptCassa
            PNotaDes = "BOLLA " & RiC("BolNum") & "-" & CDate(RiC("BolData")).ToShortDateString
            PNotaImp = RiC("BolAcconto")
            PNotaDat = RiC("BolDataAcc")
            LeggiUltimo(PNotaDat.ToShortDateString)
            ScriviPrimaNota(RiW("DcgCli"), RiW("DcgNumero"))
IINext:
        Next K
    End Sub
    Sub AccontiFattura()
        Dim StrTre As String = "Select FatdataAcc,FatAcconto From Tbfat where FatAcconto <> 0 and FatRif = " & RiW("DcgNumRif")
        Dim K As Int16
        DsDac = New DataSet
        DaDac = New SqlDataAdapter(StrTre, cnDb)
        DaDac.Fill(DsDac, Ac)
        If DsDac.Tables(Ac).Rows.Count <= 0 Then Exit Sub
        For K = 1 To DsDac.Tables(Ac).Rows.Count
            RiC = DsDac.Tables(Ac).Rows(K - 1)
            If RiC("FatDataAcc") Is DBNull.Value Then GoTo IINext
            If RiC("FatAcconto") = 0 Then GoTo IINext
            PNotaCau = TaiCauInc
            PNotaCpt = TaiCptCassa
            PNotaDes = ""
            PNotaImp = RiC("FatAcconto")
            PNotaDat = RiC("FatDataAcc")
            LeggiUltimo(PNotaDat.ToShortDateString)
            ScriviPrimaNota(RiW("DcgCli"), RiW("DcgNumero"))
IINext:
        Next K
    End Sub
    Sub AssegnoRigheSconti()
        PNotaCau = TaiCauSconti
        PNotaCpt = TaiCptAbb
        PNotaDes = "FATT. " & RiW("DcgNumero") & "-" & PNotaDat.ToShortDateString
        LeggiUltimo(PNotaDat.ToShortDateString)
        ScriviPrimaNota(RiW("DcgCli"), RiW("DcgNumero"))
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
    Sub ScriviPrimaNota(ByVal Conto As String, ByVal Numero As Integer)
        Dim Scheggia As Int16 = 0
        Dim Articolo As Int32 = 0
        Dim M As Int16
        M = 0
        TextEdit1.Text = "PRIMA NOTA " & PNotaDes
        Wrd.Parameters.Clear()
        Articolo = RileggoLocked()
        M = M + 1
        p1.Value = PNotaDat
        p2.Value = PNotaCau
        p3.Value = PNotaCpt
        p4.Value = Conto
        p5.Value = Articolo
        p6.Value = ""
        p7.Value = 0
        p8.Value = 0
        p9.Value = PNotaImp
        p10.Value = PNotaImp
        p11.Value = PNotaDes
        p12.Value = Numero
        p13.Value = ""
        p14.Value = PNotaDat
        p15.Value = ""
        p16.Value = 0
        p17.Value = 0
        p18.Value = 0
        p19.Value = ""
        p20.Value = ""
        p21.Value = ""
        If PNotaAnno = 0 Then p22.Value = PNotaDat.Year Else p22.Value = PNotaAnno
        p23.Value = 0
        p24.Value = 0
        p25.Value = 0
        p26.Value = ProgId
        p27.Value = M
        p28.Value = 0
        p29.Value = 0
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
        Scheggia = SbloccoLocked()
VaiOltre:
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        Partita(0, ProgId)
        Partita(1, 0)
    End Sub
    Sub Partita(ByVal Tipo As Int16, ByVal AZ As Int32)
        If Tipo = 0 Then
            EsegueSql(" EXEC RiAprePartita  @Id = " & ProgId & ",@Az=" & AZ & ",@Miglio=" & MiglioFo, cnCo)
        Else
            EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
        End If
    End Sub
    Function VenditeMCC(ByVal Str As String, ByVal Ddata As String) As Boolean
        Dim Oggi As String = Today.ToShortDateString
        Dim x, M As Int16
        DsMcc = New DataSet(Mc)
        DaMcc = New SqlDataAdapter(Str, CnDc)
        DaMcc.Fill(DsMcc, Mc)
        If DsMcc.Tables(Mc).Rows.Count = 0 Then Exit Function
        LeggiUltMCC(Oggi)
        M = 0
        For x = 1 To DsMcc.Tables(Mc).Rows.Count
            RwMcc = DsMcc.Tables(Mc).Rows(x - 1)
            Mmd.Parameters.Clear()
            M = M + 1
            mp1.Value = ProgMc
            mp2.Value = M
            mp3.Value = Ddata
            mp4.Value = ProgId
            mp5.Value = RwMcc("DCGMCProg")
            mp6.Value = 1
            mp7.Value = RwMcc("DCGMCCogLdp")
            mp8.Value = RwMcc("DCGMCCogcdc")
            mp9.Value = RwMcc("DCGMCCogRep")
            mp10.Value = RwMcc("DCGMCCogConto")
            mp11.Value = RwMcc("DCGMCIMPORTO")
            Mmd.Parameters.Add(mp1)
            Mmd.Parameters.Add(mp2)
            Mmd.Parameters.Add(mp3)
            Mmd.Parameters.Add(mp4)
            Mmd.Parameters.Add(mp5)
            Mmd.Parameters.Add(mp6)
            Mmd.Parameters.Add(mp7)
            Mmd.Parameters.Add(mp8)
            Mmd.Parameters.Add(mp9)
            Mmd.Parameters.Add(mp10)
            Mmd.Parameters.Add(mp11)
            Mmd.ExecuteNonQuery()
        Next
        ResetIdM()
    End Function
    Private Function LeggiUltMCC(ByVal DataOdierna As Date) As Boolean
        Dim ultimo As String = "INSERT INTO TbImc (IDmcdt) values(@Oggi)"
        Dim UltimaRiga As New SqlCommand("SELECT  @@IDENTITY ", CnDc)
        Dim Qmd As New SqlCommand(ultimo, CnDc)
        Dim px As New SqlParameter("@Oggi", SqlDbType.SmallDateTime)
        px.Value = CDate(DataOdierna)
        Qmd.Parameters.Add(px)
        Qmd.ExecuteNonQuery()
        ProgMc = UltimaRiga.ExecuteScalar
    End Function
    Sub ResetIdM()
        Dim Elimina As String = "DELETE FROM TbIMc WHERE IdMCC = " & ProgMc
        Dim Dmd As New SqlCommand(Elimina, CnDc)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub AssegnaNPartita(NN As Integer)
        Dim Command As New SqlClient.SqlCommand("AssegnaPartita")
        Command.CommandType = CommandType.StoredProcedure
        Command.Connection = cnCo
        Dim q1 As New SqlParameter("@CONTO", SqlDbType.VarChar)
        Dim q2 As New SqlParameter("@ANNO", SqlDbType.SmallInt)
        Dim q3 As New SqlParameter("@NDOC", SqlDbType.Int)
        Dim q4 As New SqlParameter("@IMPORTO", SqlDbType.Decimal)
        q1.Value = RiW("DcgCli")
        q2.Value = CDate(RiW("DcgData")).Year
        q3.Value = NN
        q4.Value = RiW("DcgRieTotFat")
        Command.Parameters.Clear()
        Command.Parameters.Add(q1)
        Command.Parameters.Add(q2)
        Command.Parameters.Add(q3)
        Command.Parameters.Add(q4)
        Command.CommandTimeout = 300
        Command.ExecuteNonQuery()
    End Sub

    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        FattureDsDcg()
        If OkCorris = True Then CorrispDcg()
        AggiornaUpg()
        'per defendini incassi gtt  spostati in coda per evitare errori nei corrispettivi
        If OkDefend = True Then IncassiGTT()

        Me.Close()
    End Sub
    Sub VerificaCorrispettivi()
        Dim StrUno As String
        Dim h, k, j As Int16
        OkCorris = False
        FinoAl = CDate(DateEdit1.EditValue).ToShortDateString
        StrUno = "Select DISTINCT DCGREGISTRO from TbDcg where (DcgRegistro between 1 and 99) and  (DcgTipo = 'S' or DcgTipo = 'R') and DcgTrasf = 0 and DcgNumero > 0 and DcgData <= '" & FinoAl & "'"
        For h = 0 To 10
            Rc(h) = 0
            Cau(h) = 0
            Cassa(h) = ""
            RegDesc(h) = ""
        Next h
        h = 0
        Cmd = New SqlCommand(StrUno, cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            h = h + 1
            Rc(h) = dataRd.Item("DCGREGISTRO")
        End While
        dataRd.Close()
        If h > 5 Then h = 5
        Rc(0) = h
        If h = 0 Then Exit Sub
        OkCorris = True
        For k = 1 To h
            Cmd = New SqlCommand("SELECT top 1 PriCausale,PriCoDare from tbpri where Priid = (SELECT ISNULL(MAX(PRIID),0) FROM TBPRI WHERE PRIREGIVA = " & Rc(k) & ")", cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                Cau(k) = dataRd.Item("PriCausale")
                Cassa(k) = dataRd.Item("PriCoDare")
            End While
            dataRd.Close()
        Next
        For k = 1 To h
            Cmd = New SqlCommand("SELECT RivaDesc from TbRegIva WHERE RivaTipo = 5 and RivaNReg = " & Rc(k) & " and RivaAnno = datepart(year,'" & FinoAl & "')", cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                RegDesc(k) = dataRd.Item("RivaDesc")
            End While
            dataRd.Close()
            If RegDesc(k) = "" Then
                OkCorris = False
                Exit Sub
            End If
            Select Case k
                Case 1
                    TextEdit101.Text = Cau(k)
                    TextEdit2.Text = Rc(k) & " " & RegDesc(k)
                    TextEdit4.Text = Cassa(k)
                    If Val(TextEdit101.Text) > 0 Then
                        LeggiCodiciIva(Val(TextEdit101.Text))
                        TextEdit3.Text = RwCii("CiiCau")
                    End If
                    LeggiConto(Cassa(k), TextEdit5)
                Case 2
                    TextEdit102.Text = Cau(k)
                    TextEdit6.Text = Rc(k) & " " & RegDesc(k)
                    TextEdit8.Text = Cassa(k)
                    If Val(TextEdit102.Text) > 0 Then
                        LeggiCodiciIva(Val(TextEdit102.Text))
                        TextEdit7.Text = RwCii("CiiCau")
                    End If
                    LeggiConto(Cassa(k), TextEdit9)
                Case 3
                    TextEdit103.Text = Cau(k)
                    TextEdit10.Text = Rc(k) & " " & RegDesc(k)
                    TextEdit12.Text = Cassa(k)
                    If Val(TextEdit103.Text) > 0 Then
                        LeggiCodiciIva(Val(TextEdit103.Text))
                        TextEdit11.Text = RwCii("CiiCau")
                    End If
                    LeggiConto(Cassa(k), TextEdit13)
                Case 4
                    TextEdit104.Text = Cau(k)
                    TextEdit14.Text = Rc(k) & " " & RegDesc(k)
                    TextEdit16.Text = Cassa(k)
                    If Val(TextEdit104.Text) > 0 Then
                        LeggiCodiciIva(Val(TextEdit104.Text))
                        TextEdit15.Text = RwCii("CiiCau")
                    End If
                    LeggiConto(Cassa(k), TextEdit17)
                Case 5
                    TextEdit105.Text = Cau(k)
                    TextEdit18.Text = Rc(k) & " " & RegDesc(k)
                    TextEdit20.Text = Cassa(k)
                    If Val(TextEdit105.Text) > 0 Then
                        LeggiCodiciIva(Val(TextEdit105.Text))
                        TextEdit19.Text = RwCii("CiiCau")
                    End If
                    LeggiConto(Cassa(k), TextEdit21)
            End Select
            If Cau(k) = 0 Then
                GroupControl2.Visible = True
                GroupControl1.Enabled = False
            End If
        Next
        If GroupControl2.Visible = False Then Exit Sub
        For j = k To 5
            Select Case j
                Case 1
                    TextEdit101.Properties.ReadOnly = True
                    TextEdit4.Properties.ReadOnly = True
                    TextEdit101.TabStop = False
                    TextEdit4.TabStop = False
                Case 2
                    TextEdit102.Properties.ReadOnly = True
                    TextEdit8.Properties.ReadOnly = True
                    TextEdit102.TabStop = False
                    TextEdit8.TabStop = False
                Case 3
                    TextEdit103.Properties.ReadOnly = True
                    TextEdit12.Properties.ReadOnly = True
                    TextEdit103.TabStop = False
                    TextEdit12.TabStop = False
                Case 4
                    TextEdit104.Properties.ReadOnly = True
                    TextEdit16.Properties.ReadOnly = True
                    TextEdit104.TabStop = False
                    TextEdit16.TabStop = False
                Case 5
                    TextEdit105.Properties.ReadOnly = True
                    TextEdit20.Properties.ReadOnly = True
                    TextEdit105.TabStop = False
                    TextEdit20.TabStop = False
            End Select
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
    Function AggiustaConto(ByRef CodCo As String) As Boolean
        Dim x As Int16
        For x = 1 To Len(CodCo)
            If Mid(CodCo, x, 1) = "." Then
                CodCo = Format(Val(Mid(CodCo, 1, x - 1)), "00") & "." & Format(Val(Mid(CodCo, x + 1, Len(CodCo) - (x - 1))), "00")
                Exit Function
            End If
        Next
    End Function
    Private Sub Textedit101_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit101.Enter, TextEdit102.Enter, TextEdit103.Enter, TextEdit104.Enter, TextEdit105.Enter, ButtonFF11.Enter '''Textedit103.LostFocus
        If TypeOf sender Is TextEdit Then
            BOXCAUSALE = sender
        End If
        SelezionaConti()
    End Sub
    Private Sub Textedit103_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit4.Enter, TextEdit8.Enter, TextEdit12.Enter, TextEdit16.Enter, TextEdit20.Enter '''Textedit103.LostFocus
        Dim CauDesc As String = ""
        If Val(BOXCAUSALE.Text) > 3 And Val(BOXCAUSALE.Text) < 73 And Val(BOXCAUSALE.Text) <> 45 Then
            LeggiCodiciIva(Val(BOXCAUSALE.Text))
            CauDesc = RwCii("CiiCau")
        Else
            CauDesc = ""
        End If
        Select Case Val(Mid(BOXCAUSALE.Name, 11, 1))
            Case 0
                Exit Select
            Case 1
                TextEdit3.Text = CauDesc
                Cau(1) = Val(TextEdit101.Text)
            Case 2
                TextEdit7.Text = CauDesc
                Cau(2) = Val(TextEdit102.Text)
            Case 3
                TextEdit11.Text = CauDesc
                Cau(3) = Val(TextEdit103.Text)
            Case 4
                TextEdit15.Text = CauDesc
                Cau(4) = Val(TextEdit104.Text)
            Case 5
                TextEdit19.Text = CauDesc
                Cau(5) = Val(TextEdit105.Text)
        End Select
        BOXCONTO = sender
    End Sub
    Sub SelezionaConti()
        Select Case Val(Mid(BOXCONTO.Name, 9, 2))
            Case 0
                Exit Select
            Case 4
                LeggiConto(BOXCONTO.Text, TextEdit5)
                Cassa(1) = BOXCONTO.Text
            Case 8
                LeggiConto(BOXCONTO.Text, TextEdit9)
                Cassa(2) = BOXCONTO.Text
            Case 12
                LeggiConto(BOXCONTO.Text, TextEdit13)
                Cassa(3) = BOXCONTO.Text
            Case 16
                LeggiConto(BOXCONTO.Text, TextEdit17)
                Cassa(4) = BOXCONTO.Text
            Case 20
                LeggiConto(BOXCONTO.Text, TextEdit21)
                Cassa(5) = BOXCONTO.Text
        End Select
    End Sub
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F11 And GroupControl2.Visible = False Then
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 And GroupControl2.Visible = True Then
            ButtonFF11.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ButtonFF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF11.Click
        Dim K As Int16
        Dim Mail As String = ""
        For K = 1 To Rc(0)
            If Cau(K) <= 4 Or Cau(K) > 72 Or Cau(K) = 45 Or LeggiConto(Cassa(K), TextEdit22) = False Then
                Mail = Mail & "CONTO o CAUSALE ERRATE SU " & RegDesc(K) & " N. " & Rc(K) & Chr(13)
            End If
        Next
        If Mail > "" Then
            Mail = Mail & "NESSUN REGISTRO DEI CORRISPETTIVI  VERRA' TRASFERITO!!!!! "
            Messaggio(0, Mail)
            OkCorris = False
        End If
        GroupControl1.Enabled = True
        GroupControl2.Visible = False
    End Sub

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
#Region "AGGIORNA TBDOC"
    Sub InitCBTbdoc()
        TbDoc = New DataTable
        DaDoc = New SqlDataAdapter("Select TOP 1 * from Tbdoc", cnVd)
        DaDoc.Fill(TbDoc)
        If TbDoc.Rows.Count > 0 Then
            RwDoc = TbDoc.Rows(0)
        End If
        Dim Str As String = "Select * from VPRINTFAT where FatRif = " & RiW("DcgNumRif")
        If OkSelco = True Or OkOttica = True Then Str = "Select * from VFATVAOG where  FatRif = " & RiW("DcgNumRif")
        TbFat = New DataTable
        DaFat = New SqlDataAdapter(Str, cnDb)
        DaFat.Fill(TbFat)
    End Sub
    Function AggiornoVisualDoc() As Boolean
        InitCBTbdoc()
        If TbFat.Rows.Count <> 1 Then Exit Function
        RwFat = TbFat.Rows(0)
        RwDoc = TbDoc.NewRow()
        Dim Nomefile As String = PathSto & "FC" & RiW("DcgNumRif") & ".pdf"
        Dim NCommesse As String = ""
        RwDoc("DocRifInterno") = "000000"
        RwDoc("DocAnno") = CDate(RwFat("FatData")).Year
        RwDoc("DocReg") = RwFat("FatNumReg")
        If RwFat("FatNumReg") = RegNcr Then
            RwDoc("DocTipo") = "NC"
            RwDoc("DocDesc") = "NOTA CREDITO NR. " & RwFat("FatNum") & " DEL " & CDate(RwFat("FatData")).ToShortDateString
        Else
            RwDoc("DocTipo") = "FC"
            RwDoc("DocDesc") = "FATTURA NR. " & RwFat("FatNum") & " DEL " & CDate(RwFat("FatData")).ToShortDateString
        End If

        RwDoc("DocData") = CDate(RwFat("FatData"))
        RwDoc("DocAnaProg") = RwFat("AnaProg")
        RwDoc("DocNumRif") = RwFat("FatRif")

        RwDoc("DocNote") = ""
        RwDoc("DocEst") = "pdf"
        RwDoc("DocProt") = RwFat("FatNum")
        RwDoc("DocAnnoCoge") = CDate(RwFat("FatData")).Year
        If OkMondo = True Then NCommesse = RwFat("FatComRif").ToString

        DXANNOPROG(RwDoc, Me.Text & " " & Today.ToShortDateString, NCommesse, False)
        If Directory.Exists(PathDoc & RwDoc("DocAnno") & "\") = False Then Directory.CreateDirectory(PathDoc & RwDoc("DocAnno") & "\")
        Dim FILEOUT As String = PathDoc & RwDoc("DocAnno") & "\" & RwDoc("DocTipo") & RwDoc("DocRifInterno") & "." & RwDoc("DocEst")
        Try
            File.Copy(Nomefile, FILEOUT, True)
        Catch ex As Exception
        End Try
    End Function
#End Region

End Class