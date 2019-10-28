Imports DXBASE
Imports System.IO
Imports System.Data.SqlClient
Imports NCCOM
Public Class NAffitti

    Dim DsEdp As DataTable
    Dim DaEdp As SqlDataAdapter
    Dim CbEdp As SqlCommandBuilder
    Dim RwEdp As DataRow

    Dim RiW As DataRow
    Dim DsPc As DataTable

    Dim DsStl As DataTable
    Dim DaStl As SqlDataAdapter
    Dim CbStl As SqlCommandBuilder
    Dim RwStl As DataRow

    Dim Articolo As Int32 = 0
    Dim CAUSALE, M, NDIS, ANNODIS, segno As Int16
    Dim ProgId, MiglioFo As Int32
    Dim DDIS, EpCpt As String
    Dim Totale As Decimal = 0
    Dim Cifra As Int32
    Dim CONTATORE As Int16
    Dim TotaleAddebito, SegnoAdd As String
    Dim flagnuovo, FLAGESITO As Boolean
    Dim uc As Integer
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
    Dim NomeB, NomeT, ExtraStr, StrStr, PathBackup As String
    Private Sub Affitti_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Azzeracampi()
    End Sub
    Sub Azzeracampi()
        TextBox1.Text = "c:\ROSINE\"
        PathBackup = Trim(TextBox1.Text) & "BACKUP"
        TextBox3.Text = ""
        Cmd = New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Trim(TextBox1.Text) > "" Then
            CaricaDati()
            TrasfTabelle()
        End If
        Me.Close()
        Exit Sub
    End Sub
    Sub CaricaDati()
        If Mid(TextBox1.Text, Len(TextBox1.Text), 1) <> "\" Then TextBox1.Text = TextBox1.Text & "\"
        PathBackup = Trim(TextBox1.Text) & "BACKUP\"

        CaricaIncassiRid()
        ' gli incassi mav sono sospesi fino a quando non verrà inserito il codice fiscale nella risposta 
        CaricaIncassiMav()
        CaricaLocazioniMav()
        CaricaLocazioniRid()
    End Sub
    Sub TrasfTabelle()
        DsEdp = New DataTable
        DaEdp = New SqlDataAdapter("Select * from TbEDP WHERE EPCoge = 0 AND EPCODCLI > '00000' order by EPDATA,EPID", cnDb)
        DaEdp.Fill(DsEdp)
        If DsEdp.Rows.Count <= 0 Then GoTo IINext
        ProgressBar1.Value = 0
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = DsEdp.Rows.Count - 1

        For i As Int32 = 0 To DsEdp.Rows.Count - 1
            RiW = DsEdp.Rows(i)
            TextBox3.Text = RiW("EPANAGRAFICA")
            TextBox3.Refresh()
            ProgressBar1.Value = i
            ScriviLocazioni()
        Next
IINext:
        DsStl = New DataTable
        DaStl = New SqlDataAdapter("Select * from TbSTL WHERE SLCoge = 0 AND SLCODCLI > '00000' order by SLDATA,SLID", cnDb)

        'DaStl = New SqlDataAdapter("Select * from TbSTL WHERE slCoge = 0 AND slCODCLI = '01009' order by slDATA,slID", cnDb)
        DaStl.Fill(DsStl)
        If DsStl.Rows.Count <= 0 Then Exit Sub
        ProgressBar2.Value = 0
        ProgressBar2.Minimum = 0
        ProgressBar2.Maximum = DsStl.Rows.Count - 1
        Dim Rb As String = "01/01/2000"
        Totale = 0

        For i As Int32 = 0 To DsStl.Rows.Count - 1
            RiW = DsStl.Rows(i)
            TextBox3.Text = RiW("SLCODCLI") & " " & RiW("SLCODFIS")
            TextBox3.Refresh()
            ProgressBar2.Value = i
            If Rb <> CDate(RiW("SLDATA")).ToShortDateString Then
                If Totale <> 0 Then ChiudiIncassi()
                Rb = CDate(RiW("SLDATA")).ToShortDateString
            End If
            If RiW("SLIMPORTO") <> 0 Then ScriviIncassi()
        Next
        If Totale <> 0 Then ChiudiIncassi()
    End Sub
    Sub ChiudiIncassi()
        Wrd.Parameters.Remove(p3)
        Wrd.Parameters.Remove(p4)
        Wrd.Parameters.Remove(p9)
        Wrd.Parameters.Remove(p10)
        Wrd.Parameters.Remove(p11)
        Wrd.Parameters.Remove(p12)
        Wrd.Parameters.Remove(p14)
        Wrd.Parameters.Remove(p22)
        Wrd.Parameters.Remove(p27)
        'p3.Value = "05.03"
        p3.Value = "05.15"
        p4.Value = "00.10"
        p9.Value = Totale
        p10.Value = 0
        p11.Value = ""
        p12.Value = 0
        p14.Value = DDIS
        p27.Value = M + 1
        Wrd.Parameters.Add(p3)
        Wrd.Parameters.Add(p4)
        Wrd.Parameters.Add(p9)
        Wrd.Parameters.Add(p10)
        Wrd.Parameters.Add(p11)
        Wrd.Parameters.Add(p12)
        Wrd.Parameters.Add(p14)
        Wrd.Parameters.Add(p22)
        Wrd.Parameters.Add(p27)
        Wrd.ExecuteNonQuery()
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        Partita(0, ProgId)
        Partita(1, 0)
        Totale = 0
        M = 0
    End Sub

    Sub ScriviIncassi()
        Dim UpWr, Desc As String
        Dim Importo As Decimal
        Dim CauIncasso As Int16
        CAUSALE = 50

        Dim Str As String = "Select * from TrovaAddebito (@CODCLI,@DATA,@IMPORTO,@CAUSALE)"
        Dim k1 As New SqlParameter("@CODCLI", SqlDbType.VarChar)
        Dim k2 As New SqlParameter("@DATA", SqlDbType.SmallDateTime)
        Dim k3 As New SqlParameter("@IMPORTO", SqlDbType.Decimal)
        Dim k4 As New SqlParameter("@CAUSALE", SqlDbType.SmallInt)
        Dim Cmd As New SqlCommand(Str, cnDb)
        k1.Value = RiW("SLCODCLI")
        k2.Value = CDate(RiW("SLDATAADD")).ToShortDateString
        k3.Value = RiW("SLIMPORTO")
        k4.Value = CAUSALE
        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(k1)
        Cmd.Parameters.Add(k2)
        Cmd.Parameters.Add(k3)
        Cmd.Parameters.Add(k4)
        NDIS = 0 : ANNODIS = CDate(RiW("SLDATA")).Year

        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            NDIS = dataRd.Item(0)
            ANNODIS = CDate(RiW("SLDATAADD")).Year
        End While
        dataRd.Close()
        'CauIncasso = 51  ' causale incasso
        CauIncasso = 18  ' causale incasso
        EpCpt = "00.10" ' conto transito
        If Totale = 0 Then
            LeggiUltimo(RiW("SLDATA"))
            Articolo = RileggoLocked()
            M = 1
            DDIS = CDate(RiW("SLDATA")).ToShortDateString
            SbloccoLocked()
        Else
            M = M + 1
        End If
        Wrd.Parameters.Clear()
        Desc = ""
        Importo = RiW("SLIMPORTO")
        SbloccoLocked()
        Wrd.Parameters.Clear()
        p1.Value = RiW("SLDATA")
        p2.Value = CauIncasso
        p3.Value = EpCpt
        p4.Value = RiW("SLCODCLI")
        p9.Value = 0
        p10.Value = Importo
        p5.Value = Articolo
        p6.Value = ""
        p7.Value = 0
        p8.Value = 0
        p11.Value = Mid(Desc, 1, 24)
        ''NDIS = CDate(RiW("SLDATA")).Month
        p12.Value = NDIS
        p13.Value = "" 'RiW("PriMeseSk")
        p14.Value = DDIS
        p15.Value = Mid(Desc, 25, 32)
        p16.Value = 0 'RiW("PriFl04")
        p17.Value = 0 'RiW("PriFl05")
        p18.Value = 0 'RiW("PriFl06")
        p19.Value = "" 'RiW("PriNsRif")
        'p20.Value = "" 'RiW("PriSos")
        'variazione apportata il 30/07/2008
        ''If M > 1 Then p20.Value = RiW("EPCODCLI") Else p20.Value = ""
        If M > 1 Then p20.Value = RiW("SLCODCLI") Else p20.Value = ""
        p21.Value = "" 'RiW("PriLinea")
        p22.Value = ANNODIS
        p23.Value = 0
        p24.Value = 0 'RiW("PriValuta")
        p25.Value = 0 'RiW("PriArtFisc")
        p26.Value = ProgId
        p27.Value = M
        p28.Value = 0 'RiW("PriIvaPrint")
        p29.Value = 0 'RiW("PriGStampa")
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
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        Partita(0, ProgId)
        Partita(1, 0)
        Totale = Totale + Importo
        UpWr = "Update TbSTL set SLCoge = 1 where SLID = " & RiW("SLID")
        Dim Ulb As New SqlCommand(UpWr, cnDb)
        Ulb.ExecuteNonQuery()
    End Sub
    Sub ScriviLocazioni()
        Dim UpWr, Desc As String
        Dim Importo As Decimal = 0
        Dim P, Q As Int16
        CAUSALE = 50
        If RiW("EPDIRETTO") = 0 Then Q = 120 Else Q = 100
        For P = 100 To Q
            If P = 100 Then
                LeggiUltimo(RiW("EPDATA"))
                Articolo = RileggoLocked()
                M = 1
                NDIS = CDate(RiW("EPDATA")).Month
                DDIS = CDate(RiW("EPDATA")).ToShortDateString
                Desc = RiW("EPDESC")
                'Desc = RiW("EPDESC").ToString.Remove(1, 10)
                Importo = RiW("EPTOTALE")
                SbloccoLocked()
                EpCpt = "80.01"
            Else
                If P = 101 Then
                    If RiW("EPCANONE") = 0 Then GoTo LOOPING
                    Importo = RiW("EPCANONE")
                    EpCpt = "80.01"
                Else
                    If RiW("EPSPESE" & P) = 0 Then Exit For
                    Desc = RiW("EPDESC" & P)
                    Importo = RiW("EPSPESE" & P)
                    EpCpt = VerificaCpt(Desc)
                End If
                M = M + 1
            End If
            Wrd.Parameters.Clear()
            p1.Value = RiW("EPDATA")
            p2.Value = CAUSALE
            If RiW("EPDIRETTO") = 0 Then
                If M = 1 Then
                    p3.Value = RiW("EPCODCLI")
                    p4.Value = "00.10"
                    p9.Value = Importo
                    p10.Value = 0
                Else
                    p3.Value = "00.10"
                    p4.Value = EpCpt
                    p9.Value = 0
                    p10.Value = Importo
                End If
            Else
                p3.Value = RiW("EPCODCLI")
                p4.Value = EpCpt
                p9.Value = Importo
                p10.Value = Importo
            End If
            p5.Value = Articolo
            p6.Value = ""
            p7.Value = 0
            p8.Value = 0
            p11.Value = Mid(Desc, 1, 24)
            p12.Value = NDIS
            p13.Value = "" 'RiW("PriMeseSk")
            p14.Value = DDIS
            p15.Value = Mid(Desc, 25, 32)
            p16.Value = 0 'RiW("PriFl04")
            p17.Value = 0 'RiW("PriFl05")
            p18.Value = 0 'RiW("PriFl06")
            p19.Value = "" 'RiW("PriNsRif")
            'p20.Value = "" 'RiW("PriSos")
            'variazione apportata il 30/07/2008
            If M > 1 Then p20.Value = RiW("EPCODCLI") Else p20.Value = ""
            p21.Value = "" 'RiW("PriLinea")
            p22.Value = CDate(RiW("EPDATA")).Year
            p23.Value = 0
            p24.Value = 0 'RiW("PriValuta")
            p25.Value = 0 'RiW("PriArtFisc")
            p26.Value = ProgId
            p27.Value = M
            p28.Value = 0 'RiW("PriIvaPrint")
            p29.Value = 0 'RiW("PriGStampa")
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
LOOPING:
        Next P
        EsegueSql(" EXEC InitPrk  @ID = " & ProgId, cnCo)
        ResetIdP()
        Partita(0, ProgId)
        Partita(1, 0)
        UpWr = "Update TbEDP set EPCoge = 1 where EPID = " & RiW("EPID")
        Dim Ulb As New SqlCommand(UpWr, cnDb)
        Ulb.ExecuteNonQuery()
    End Sub
    Private Function VerificaCpt(ByVal Desc As String) As String
        'per cose diverse da affitto in data 20/06/2007 Istat su 80.01 - 50% contratto su 82.05 - il resto 82.04
        If Mid(Desc, 1, 16) = "Conguaglio Istat" Then
            VerificaCpt = "80.01"
        ElseIf Mid(Desc, 1, 24) = "Regolamento contratto 50" Then
            VerificaCpt = "82.05"
        ElseIf Mid(Desc, 1, 7) = "Canone " Then
            VerificaCpt = "80.01"
        ElseIf Mid(Desc, 1, 8) = "Esazione" Then
            VerificaCpt = "82.12"
        ElseIf Mid(Desc, 1, 5) = "Bollo" Then
            VerificaCpt = "82.11"
        ElseIf Mid(Desc, 1, 13) = "Riscaldamento" Then
            VerificaCpt = "82.06"
        ElseIf Desc.IndexOf("Interessi") > -1 Then
            VerificaCpt = "66.02"
        ElseIf Desc.IndexOf("Cauzione") > -1 Then
            VerificaCpt = "44.12"
        ElseIf Desc.IndexOf("50%") > -1 Then
            VerificaCpt = "82.05"
        Else
            VerificaCpt = "82.04"
        End If
    End Function
    Private Function LeggiUltimo(ByVal dataGio As Date)
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
    Sub Partita(ByVal Tipo As Int16, ByVal AZ As Int32)
        If Tipo = 0 Then
            EsegueSql(" EXEC RiAprePartita  @Id = " & ProgId & ",@Az=" & AZ & ",@Miglio=" & MiglioFo, cnCo)
        Else
            EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
        End If
    End Sub
    Sub CaricaLocazioni()
        Dim FF As FileInfo
        Dim dir As New DirectoryInfo(Trim(TextBox1.Text))
        For Each FF In dir.GetFiles("*.txt")
            NomeB = Trim(TextBox1.Text) & FF.Name
            If File.Exists(NomeB) = False Then Exit Sub
            DsEdp = New DataTable
            DaEdp = New SqlDataAdapter("SELECT * FROM TBEDP WHERE EPID = 89898989898989", cnDb)
            DaEdp.Fill(DsEdp)
            CbEdp = New SqlCommandBuilder(DaEdp)
            REM RECORD DI TESTA 'EP'
            Dim Ten As New System.IO.FileStream(NomeB, IO.FileMode.Open, FileAccess.Read)
            Dim Buf As New System.IO.BufferedStream(Ten, 120)
            Dim Rea As New System.IO.StreamReader(Buf)
            'Dim Cifra As Int32
            Dim SW As Int16 = 0
            Dim TIP As String
            ExtraStr = Rea.ReadLine
            If Mid(ExtraStr, 2, 2) <> "EP" Then
                Rea.Close()
                GoTo FineLoop
            End If
            '' REM FILE NON VALIDO
            Do
                ExtraStr = Rea.ReadLine
                TextBox3.Text = ExtraStr
                TextBox3.Refresh()
                TIP = Mid(ExtraStr, 2, 2)
                Select Case TIP
                    Case Is = "14"
                        If SW = 1 Then DsEdp.Rows.Add(RwEdp) Else SW = 1 ''SE non ci sono spese?
                        RwEdp = DsEdp.NewRow()

                        RwEdp("EPDATA") = CDate(Mid(ExtraStr, 23, 2) & "/" & Mid(ExtraStr, 25, 2) & "/20" & Mid(ExtraStr, 27, 2)).ToShortDateString
                        RwEdp("EPCODFIS") = Mid(ExtraStr, 98, 16)
                        Exit Select
                    Case Is = "30"
                        RwEdp("EPANAGRAFICA") = Mid(ExtraStr, 11, 110)
                        Exit Select
                    Case Is = "40"
                        RwEdp("EPINDIRIZZO") = Mid(ExtraStr, 11, 30)
                        RwEdp("EPCAP") = Mid(ExtraStr, 41, 5)
                        RwEdp("EPCITTA") = Mid(ExtraStr, 46, 23)
                        RwEdp("EPPROV") = Mid(ExtraStr, 69, 2)
                        Exit Select
                    Case Is = "50"
                        Exit Select
                    Case Is = "70"
                        If Mid(ExtraStr, 11, 3) = "001" Then
                            If Mid(ExtraStr, 13, 1) = "-" Then segno = -1 Else segno = 1
                            Cifra = Val(Mid(ExtraStr, 14, 10)) * segno
                            RwEdp("EPTOTALE") = CDec(Cifra / 100)
                            RwEdp("EPCANONE") = 0.0
                            RwEdp("EPDESC") = ""
                            RwEdp("EPSPESE102") = 0.0
                            RwEdp("EPDESC102") = ""
                            RwEdp("EPSPESE103") = 0.0
                            RwEdp("EPDESC103") = ""
                            RwEdp("EPSPESE104") = 0.0
                            RwEdp("EPDESC104") = ""
                            RwEdp("EPSPESE105") = 0.0
                            RwEdp("EPDESC105") = ""
                            RwEdp("EPSPESE106") = 0.0
                            RwEdp("EPDESC106") = ""
                            RwEdp("EPSPESE107") = 0.0
                            RwEdp("EPDESC107") = ""
                            RwEdp("EPSPESE108") = 0.0
                            RwEdp("EPDESC108") = ""
                            RwEdp("EPSPESE109") = 0.0
                            RwEdp("EPDESC109") = ""
                            RwEdp("EPCOGE") = 0
                            RwEdp("EPDIRETTO") = 1
                            RwEdp("EPCODCLI") = RecuperoCliente(RwEdp("EPCODFIS").ToString)
                            If RwEdp("EPCODCLI") = "00000" Then
                                RwEdp("EPCODCLI") = InserisciCliente(RwEdp)
                            End If
                            Exit Select
                        End If
                        If Mid(ExtraStr, 11, 3) = "101" Then
                            RwEdp("EPDESC") = Mid(ExtraStr, 25, 48)
                            If Mid(ExtraStr, 73, 1) = "-" Then segno = -1 Else segno = 1
                            Cifra = Val(Mid(ExtraStr, 74, 10)) * segno
                            RwEdp("EPCANONE") = CDec(Cifra / 100)
                            Exit Select
                        End If
                        If Val(Mid(ExtraStr, 11, 3)) > 101 And Val(Mid(ExtraStr, 11, 3)) < 121 Then
                            RwEdp("EPDESC" & Mid(ExtraStr, 11, 3)) = Mid(ExtraStr, 25, 48)
                            If Mid(ExtraStr, 73, 1) = "-" Then segno = -1 Else segno = 1
                            Cifra = Val(Mid(ExtraStr, 74, 10)) * segno
                            RwEdp("EPSPESE" & Mid(ExtraStr, 11, 3)) = CDec(Cifra / 100)
                            RwEdp("EPDIRETTO") = 0
                            Exit Select
                        End If
                End Select
            Loop While ExtraStr <> Nothing
            If SW = 1 Then DsEdp.Rows.Add(RwEdp)
            Rea.Close()
            DaEdp.Update(DsEdp)
            DsEdp.AcceptChanges()
            File.Move(NomeB, PathBackup & FF.Name)
FineLoop:
        Next

    End Sub
    Function RecuperoCliente(ByVal CodFis As String) As String
        Dim Cmd As New SqlCommand("select Anacod from Tbana where anagrp = 'CL' and (anacfis = '" & CodFis & "' OR anapiva = '" & CodFis & "')", cnVd)
        RecuperoCliente = "00000"
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            RecuperoCliente = dataRd.Item("AnaCod")
        End While
        dataRd.Close()
    End Function
    ''    Sub CaricaIncassi()
    ''        Dim FF As FileInfo
    ''        Dim dir As New DirectoryInfo(Trim(TextBox1.Text))
    ''        For Each FF In dir.GetFiles("*.txt")
    ''            NomeT = Trim(TextBox1.Text) & FF.Name
    ''            If File.Exists(NomeT) = False Then Exit Sub
    ''            DsStl = New DataTable
    ''            DaStl = New SqlDataAdapter("SELECT * FROM TBStl WHERE SLID = 89898989898989", cnDb)
    ''            DaStl.Fill(DsStl)
    ''            CbStl = New SqlCommandBuilder(DaStl)
    ''            REM RECORD DI TESTA 'SL'
    ''            Dim Ten As New System.IO.FileStream(NomeT, IO.FileMode.Open, FileAccess.Read)
    ''            Dim Buf As New System.IO.BufferedStream(Ten, 120)
    ''            Dim Rea As New System.IO.StreamReader(Buf)
    ''            'Dim Cifra As Int32
    ''            Dim SW As Int16 = 0
    ''            Dim DATAOP As String = ""
    ''            Dim TIP As String
    ''            ExtraStr = Rea.ReadLine
    ''            If Mid(ExtraStr, 2, 2) <> "SL" Then
    ''                Rea.Close()
    ''                GoTo FINELOOPP
    ''            End If
    ''            ''' REM FILE NON VALIDO
    ''            Do
    ''                If SW = 1 Then ExtraStr = Rea.ReadLine
    ''                TextBox3.Text = ExtraStr
    ''                TextBox3.Refresh()
    ''                TIP = Mid(ExtraStr, 2, 2)
    ''                If TIP = "SL" Then
    ''                    DATAOP = CDate(Mid(ExtraStr, 14, 2) & "/" & Mid(ExtraStr, 16, 2) & "/20" & Mid(ExtraStr, 18, 2)).ToShortDateString
    ''                    SW = 1
    ''                    GoTo FINETEST
    ''                End If
    ''                TIP = Mid(ExtraStr, 1, 6)
    ''                If TIP = "P372Q9" Then
    ''                    RwStl = DsStl.NewRow()
    ''                    RwStl("SLDATA") = DATAOP
    ''                    RwStl("SLCODFIS") = Mid(ExtraStr, 7, 16)
    ''                    RwStl("SLCODCLI") = RecuperoCliente(Trim(Mid(ExtraStr, 7, 16)))
    ''                    If Mid(ExtraStr, 32, 1) = "-" Then segno = -1 Else segno = 1
    ''                    Cifra = Val(Mid(ExtraStr, 33, 10)) * segno
    ''                    RwStl("SLIMPORTO") = CDec(Cifra / 100)
    ''                    RwStl("SLDATAADD") = CDate(Mid(ExtraStr, 53, 2) & "/" & Mid(ExtraStr, 55, 2) & "/20" & Mid(ExtraStr, 57, 2)).ToShortDateString
    ''                    RwStl("SLCOGE") = 0
    ''                    DsStl.Rows.Add(RwStl)
    ''                End If
    ''FINETEST:
    ''            Loop While ExtraStr <> Nothing
    ''            Rea.Close()
    ''            DaStl.Update(DsStl)
    ''            DsStl.AcceptChanges()
    ''            File.Move(NomeT, PathBackup & FF.Name)
    ''        Next
    ''FINELOOPP:
    ''    End Sub
    Function SbloccoLocked() As Int16
        Dim Del As New SqlCommand("Delete from TMPlock WITH (TABLOCKX) where IdPrNota = 1 ", cnCo)
        Del.ExecuteNonQuery()
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
    Function CercaCliente(ByVal Riw As DataRow) As String
        'Dim Uc As Integer
        Dim Cmd As New SqlCommand("select GrpUl1 from TbGrp where GrpCod = 'CL'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            uc = dataRd.Item(0)
        End While
        dataRd.Close()
        uc = uc + 1
    End Function
    Function InserisciCliente(ByVal Riw As DataRow) As String
        Dim Registra As String = "INSERT INTO TbAna (AnaDesc,AnaPiva,AnaCfis,AnaIndirizzo,AnaCap,AnaCitta,AnaProv,Anatel1,Anatel2,AnaTel3,AnaWWW,AnaEmail,AnaGrp,AnaCod,AnaResp,AnaNote,AnaFax,AnaRag1,AnaRag2,AnaPivaEst) " _
    & " values(@AnaDesc,@AnaPiva,@AnaCfis,@AnaIndirizzo,@AnaCap,@AnaCitta,@AnaProv,'','','','','','CL',@AnaCod,'','','',@AnaRag1,@AnaRag2,@AnaPivaEst)"
        Dim Parall As String = "INSERT into TbCli (ClCod,ClPagam,ClAbi,ClCab,ClCntRid,ClCodBan,ClCC) VALUES (@Clcod,10,0,0,'',1,'')"
        Dim x As Int16
        'Dim Uc As Integer
        'Dim Cmd As New SqlCommand("select GrpUl1 from TbGrp where GrpCod = 'CL'", cnCo)
        'dataRd = Cmd.ExecuteReader
        'While dataRd.Read
        '    Uc = dataRd.Item(0)
        'End While
        'dataRd.Close()
        Cmd = New SqlCommand(Registra, cnVd)
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

        If Not IsNumeric(Trim(Riw("EPCODFIS").ToString)) Then
            p3.Value = Riw("EPCODFIS")
            p2.Value = ""
        Else
            p3.Value = Mid(Trim(Riw("EPCODFIS").ToString), 1, 11)
            p2.Value = Mid(Trim(Riw("EPCODFIS").ToString), 1, 11)
        End If
        ' Uc = Uc + 1
        p11.Value = ""
        p4.Value = Riw("EPINDIRIZZO").ToString.ToUpper
        p5.Value = Riw("EPCAP")
        p6.Value = Riw("EPCITTA").ToString.ToUpper
        p7.Value = Riw("EPPROV").ToString.ToUpper
        p8.Value = (Uc).ToString.PadLeft(5, "0")
        p1.Value = Mid(Trim(Riw("EPANAGRAFICA").ToString), 1, 60).ToString.ToUpper
        p9.Value = Mid(p1.Value, 1, 28)
        p10.Value = Mid(p1.Value, 29, 28)
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
        Cmd.ExecuteNonQuery()
        'inserimento dati su GEVE\TbCli
        Cmd = New SqlCommand(Parall, cnDb)
        Dim p12 As New SqlParameter("@ClCod", SqlDbType.VarChar)
        p12.Value = (Uc).ToString.PadLeft(5, "0")
        Cmd.Parameters.Add(p12)
        Cmd.ExecuteNonQuery()
        InserisciCliente = p12.Value
        Cmd = New SqlCommand("Update TbGrp set GrpUl1=" & Uc & " where GrpCod = 'CL'", cnCo)
        Cmd.ExecuteNonQuery()
        flagnuovo = False
    End Function
    'nuovo trasferimento x cambio banca 
    Sub CaricaIncassiRid()
        Dim FF As FileInfo
        Dim dir As New DirectoryInfo(Trim(TextBox1.Text))
        For Each FF In dir.GetFiles("*.txt")
            If Mid(FF.Name, 1, 5) <> "ESRID" Then Exit Sub
            NomeT = Trim(TextBox1.Text) & FF.Name
            If File.Exists(NomeT) = False Then Exit Sub
            DsStl = New DataTable
            DaStl = New SqlDataAdapter("SELECT * FROM TBStl WHERE SLID = 89898989898989", cnDb)
            DaStl.Fill(DsStl)
            CbStl = New SqlCommandBuilder(DaStl)
            REM RECORD DI TESTA 'IR'
            Dim Ten As New System.IO.FileStream(NomeT, IO.FileMode.Open, FileAccess.Read)
            Dim Buf As New System.IO.BufferedStream(Ten, 120)
            Dim Rea As New System.IO.StreamReader(Buf)
            'Dim Cifra As Int32
            Dim SW As Int16 = 0
            Dim DATAOP As String = ""
            Dim TIP As String
            Dim ESITO As String
            ExtraStr = Rea.ReadLine
            If Mid(ExtraStr, 2, 2) <> "IR" Then
                Rea.Close()
                GoTo FINELOOPP
            End If
            DATAOP = CDate(Mid(ExtraStr, 14, 2) & "/" & Mid(ExtraStr, 16, 2) & "/20" & Mid(ExtraStr, 18, 2)).ToShortDateString
            SW = 1
            Do
                If SW = 1 Then ExtraStr = Rea.ReadLine
                TextBox3.Text = ExtraStr
                TextBox3.Refresh()

                TIP = Mid(ExtraStr, 2, 2)
                ESITO = Mid(ExtraStr, 29, 5)
                'SE 10 E CAUSALE 50000 SI TRATTA DI FILE RID DI ADDEBITO E NON DI FLUSSO DI RITORNO
                If TIP = "10" And ESITO = "50000" Then
                    Rea.Close()
                    GoTo FINELOOPP
                End If

                'SE FLUSSO DI RITORNO RID L'ESITO PUO' ESSERE 50010, 50006, 50008, 50001, 50003, 50004, 50007, 50009 - CONSIDERIAMO SOLO 50010 = OK PAGATA
                If TIP = "10" And ESITO = "50010" Then
                    'DATAOP = CDate(Mid(ExtraStr, 23, 2) & "/" & Mid(ExtraStr, 25, 2) & "/20" & Mid(ExtraStr, 27, 2)).ToShortDateString
                    RwStl = DsStl.NewRow()
                    RwStl("SLDATA") = DATAOP
                    RwStl("SLCODFIS") = Mid(ExtraStr, 98, 16)
                    RwStl("SLCODCLI") = RecuperoCliente(Trim(Mid(ExtraStr, 98, 16)))
                    If Mid(ExtraStr, 47, 1) = "-" Then segno = -1 Else segno = 1
                    Cifra = Val(Mid(ExtraStr, 34, 13)) * segno
                    RwStl("SLIMPORTO") = CDec(Cifra / 100)
                    RwStl("SLDATAADD") = CDate(Mid(ExtraStr, 23, 2) & "/" & Mid(ExtraStr, 25, 2) & "/20" & Mid(ExtraStr, 27, 2)).ToShortDateString
                    RwStl("SLCOGE") = 0
                    DsStl.Rows.Add(RwStl)
                End If
FINETEST:
            Loop While ExtraStr <> Nothing
            Rea.Close()
            DaStl.Update(DsStl)
            DsStl.AcceptChanges()
            File.Move(NomeT, PathBackup & FF.Name)
        Next
FINELOOPP:
    End Sub
    Sub CaricaLocazioniRid()
        Dim FF As FileInfo
        Dim TIPORID As String
        'Dim TotaleAddebito, SegnoAdd As String
        Dim dir As New DirectoryInfo(Trim(TextBox1.Text))
        TIPORID = "" : TotaleAddebito = "" : SegnoAdd = ""
        For Each FF In dir.GetFiles("*.txt")
            If Mid(FF.Name, 1, 9) <> "FlussoRID" Then Exit Sub
            NomeB = Trim(TextBox1.Text) & FF.Name
            If File.Exists(NomeB) = False Then Exit Sub
            DsEdp = New DataTable
            DaEdp = New SqlDataAdapter("SELECT * FROM TBEDP WHERE EPID = 89898989898989", cnDb)
            DaEdp.Fill(DsEdp)
            CbEdp = New SqlCommandBuilder(DaEdp)
            REM RECORD DI TESTA 'EP'
            Dim Ten As New System.IO.FileStream(NomeB, IO.FileMode.Open, FileAccess.Read)
            Dim Buf As New System.IO.BufferedStream(Ten, 120)
            Dim Rea As New System.IO.StreamReader(Buf)
            'Dim Cifra As Int32
            Dim SW As Int16 = 0
            Dim TIP As String
            Dim PIU As Int16 = 0
            ExtraStr = Rea.ReadLine
            If Mid(ExtraStr, 2, 2) <> "IR" Then
                Rea.Close()
                GoTo FineLoop
            End If
            '' REM FILE NON VALIDO
            CONTATORE = 0
            Do
                ExtraStr = Rea.ReadLine
                TextBox3.Text = ExtraStr
                TextBox3.Refresh()
                TIP = Mid(ExtraStr, 2, 2)
                TIPORID = Mid(ExtraStr, 29, 5)

                ' VUOL DIRE CHE IL FILE E' UN FLUSSO DI RITORNO
                If TIP = "10" And TIPORID <> "50000" Then
                    Rea.Close()
                    GoTo FineLoop
                End If

                Select Case TIP
                    Case Is = "10"
                        If SW = 1 Then DsEdp.Rows.Add(RwEdp) Else SW = 1 ''SE non ci sono spese?
                        flagnuovo = False
                        RwEdp = DsEdp.NewRow()
                        RwEdp("EPDATA") = CDate(Mid(ExtraStr, 23, 2) & "/" & Mid(ExtraStr, 25, 2) & "/20" & Mid(ExtraStr, 27, 2)).ToShortDateString
                        RwEdp("EPCODFIS") = Mid(ExtraStr, 98, 16)
                        TotaleAddebito = Mid(ExtraStr, 34, 13)
                        SegnoAdd = "+"
                        CONTATORE = 100
                        RigaTotale()
                        Exit Select

                    Case Is = "30"
                        RwEdp("EPANAGRAFICA") = Mid(ExtraStr, 11, 110)
                        Exit Select

                    Case Is = "40"
                        RwEdp("EPINDIRIZZO") = Mid(ExtraStr, 11, 30)
                        RwEdp("EPCAP") = Mid(ExtraStr, 41, 5)
                        RwEdp("EPCITTA") = Mid(ExtraStr, 46, 23)
                        RwEdp("EPPROV") = Mid(ExtraStr, 71, 2)
                        If flagnuovo = True Then RwEdp("EPCODCLI") = InserisciCliente(RwEdp)
                        Exit Select

                    Case Is = "50"
                        Exit Select

                    Case Is = "60"
                        If Mid(ExtraStr, 11, 4) = "----" Then Exit Select
                        CONTATORE = CONTATORE + 1
                        If CONTATORE = 101 Then
                            If Mid(ExtraStr, 11, 6) = "Canone" Then
                                RwEdp("EPDESC") = Mid(ExtraStr, 11, 45)
                                'VARIATO IL 28072014
                                'If Mid(ExtraStr, 120, 1) = "-" Then segno = -1 Else segno = 1
                                'Cifra = Val(Mid(ExtraStr, 112, 8)) * segno

                                If Mid(ExtraStr, 78, 1) = "-" Then segno = -1 Else segno = 1
                                Cifra = Val(Mid(ExtraStr, 70, 8)) * segno
                                RwEdp("EPCANONE") = CDec(Cifra / 100)
                                Exit Select
                            End If
                            CONTATORE = CONTATORE + 1
                        End If
                        If CONTATORE > 101 Then
                            RwEdp("EPDESC" & CONTATORE.ToString("000")) = Mid(ExtraStr, 11, 46)
                            'VARIATO IL 28072014
                            'If Mid(ExtraStr, 120, 1) = "-" Then segno = -1 Else segno = 1
                            'Cifra = Val(Mid(ExtraStr, 112, 8)) * segno
                            If Mid(ExtraStr, 78, 1) = "-" Then segno = -1 Else segno = 1
                            Cifra = Val(Mid(ExtraStr, 70, 8)) * segno
                            RwEdp("EPSPESE" & CONTATORE.ToString("000")) = CDec(Cifra / 100)
                            RwEdp("EPDIRETTO") = 0
                            Exit Select
                        End If
                End Select
            Loop While ExtraStr <> Nothing
            If SW = 1 Then DsEdp.Rows.Add(RwEdp)
            Rea.Close()
            DaEdp.Update(DsEdp)
            DsEdp.AcceptChanges()
            File.Move(NomeB, PathBackup & FF.Name)
FineLoop:
        Next
    End Sub
    Sub RigaTotale()
        If SegnoAdd = "-" Then segno = -1 Else segno = 1
        Cifra = Val(TotaleAddebito) * segno
        RwEdp("EPTOTALE") = CDec(Cifra / 100)
        RwEdp("EPCANONE") = 0.0
        RwEdp("EPDESC") = ""
        RwEdp("EPSPESE102") = 0.0
        RwEdp("EPDESC102") = ""
        RwEdp("EPSPESE103") = 0.0
        RwEdp("EPDESC103") = ""
        RwEdp("EPSPESE104") = 0.0
        RwEdp("EPDESC104") = ""
        RwEdp("EPSPESE105") = 0.0
        RwEdp("EPDESC105") = ""
        RwEdp("EPSPESE106") = 0.0
        RwEdp("EPDESC106") = ""
        RwEdp("EPSPESE107") = 0.0
        RwEdp("EPDESC107") = ""
        RwEdp("EPSPESE108") = 0.0
        RwEdp("EPDESC108") = ""
        RwEdp("EPSPESE109") = 0.0
        RwEdp("EPDESC109") = ""
        RwEdp("EPCOGE") = 0
        RwEdp("EPDIRETTO") = 1
        RwEdp("EPCODCLI") = RecuperoCliente(RwEdp("EPCODFIS").ToString)
        RwEdp("EPSPESE110") = 0.0
        RwEdp("EPDESC110") = ""
        RwEdp("EPSPESE111") = 0.0
        RwEdp("EPDESC111") = ""
        RwEdp("EPSPESE112") = 0.0
        RwEdp("EPDESC112") = ""
        RwEdp("EPSPESE113") = 0.0
        RwEdp("EPDESC113") = ""
        RwEdp("EPSPESE114") = 0.0
        RwEdp("EPDESC114") = ""
        RwEdp("EPSPESE115") = 0.0
        RwEdp("EPDESC115") = ""
        RwEdp("EPSPESE116") = 0.0
        RwEdp("EPDESC116") = ""
        RwEdp("EPSPESE117") = 0.0
        RwEdp("EPDESC117") = ""
        RwEdp("EPSPESE118") = 0.0
        RwEdp("EPDESC118") = ""
        RwEdp("EPSPESE119") = 0.0
        RwEdp("EPDESC119") = ""
        RwEdp("EPSPESE120") = 0.0
        RwEdp("EPDESC120") = ""

        If RwEdp("EPCODCLI") = "00000" Then
            flagnuovo = True
            'deve cercare il codice da inserire ma l'inserimento avviene quando legge il record 40
            CercaCliente(RwEdp)
            RwEdp("EPCODCLI") = (uc).ToString.PadLeft(5, "0")
        End If

    End Sub
    Sub CaricaLocazioniMav()
        Dim FF As FileInfo
        Dim TIPORID As String
        'Dim TotaleAddebito, SegnoAdd As String
        Dim dir As New DirectoryInfo(Trim(TextBox1.Text))
        TIPORID = "" : TotaleAddebito = "" : SegnoAdd = ""
        For Each FF In dir.GetFiles("*.txt")
            If Mid(FF.Name, 1, 9) <> "FlussoMAV" Then Exit Sub
            NomeB = Trim(TextBox1.Text) & FF.Name
            If File.Exists(NomeB) = False Then Exit Sub
            DsEdp = New DataTable
            DaEdp = New SqlDataAdapter("SELECT * FROM TBEDP WHERE EPID = 89898989898989", cnDb)
            DaEdp.Fill(DsEdp)
            CbEdp = New SqlCommandBuilder(DaEdp)
            REM RECORD DI TESTA 'EP'
            Dim Ten As New System.IO.FileStream(NomeB, IO.FileMode.Open, FileAccess.Read)
            Dim Buf As New System.IO.BufferedStream(Ten, 120)
            Dim Rea As New System.IO.StreamReader(Buf)
            'Dim Cifra As Int32
            Dim SW As Int16 = 0
            Dim TIP As String
            ExtraStr = Rea.ReadLine
            If Mid(ExtraStr, 2, 2) <> "IM" Then
                Rea.Close()
                GoTo FineLoop
            End If
            '' REM FILE NON VALIDO
            CONTATORE = 0
            Do
                ExtraStr = Rea.ReadLine
                TextBox3.Text = ExtraStr
                TextBox3.Refresh()
                TIP = Mid(ExtraStr, 2, 2)
                TIPORID = Mid(ExtraStr, 29, 5)


                If TIP = "14" And TIPORID <> "07000" Then
                    Rea.Close()
                    GoTo FineLoop
                End If

                Select Case TIP
                    Case Is = "14"
                        If SW = 1 Then DsEdp.Rows.Add(RwEdp) Else SW = 1 ''SE non ci sono spese?
                        flagnuovo = False
                        RwEdp = DsEdp.NewRow()
                        RwEdp("EPDATA") = CDate(Mid(ExtraStr, 23, 2) & "/" & Mid(ExtraStr, 25, 2) & "/20" & Mid(ExtraStr, 27, 2)).ToShortDateString
                        TotaleAddebito = Mid(ExtraStr, 34, 13)
                        SegnoAdd = "+"
                        CONTATORE = 100
                        'RigaTotale()
                        Exit Select

                    Case Is = "16"
                        Exit Select

                    Case Is = "20"
                        Exit Select

                    Case Is = "30"
                        RwEdp("EPANAGRAFICA") = Mid(ExtraStr, 11, 60)
                        RwEdp("EPCODFIS") = Mid(ExtraStr, 71, 16)
                        RigaTotale()
                        Exit Select

                    Case Is = "40"
                        RwEdp("EPINDIRIZZO") = Mid(ExtraStr, 11, 30)
                        RwEdp("EPCAP") = Mid(ExtraStr, 41, 5)
                        RwEdp("EPCITTA") = Mid(ExtraStr, 46, 21)
                        RwEdp("EPPROV") = Mid(ExtraStr, 69, 2)
                        If flagnuovo = True Then RwEdp("EPCODCLI") = InserisciCliente(RwEdp)
                        Exit Select

                    Case Is = "51"
                        Exit Select

                    Case Is = "59"
                        'If Mid(ExtraStr, 11, 4) = "----" Then Exit Select
                        CONTATORE = CONTATORE + 1
                        If Mid(ExtraStr, 11, 6) = "Canone" And CONTATORE = 101 Then
                            RwEdp("EPDESC") = Mid(ExtraStr, 11, 45)
                            If Mid(ExtraStr, 120, 1) = "-" Then segno = -1 Else segno = 1
                            Cifra = Val(Mid(ExtraStr, 112, 8)) * segno
                            RwEdp("EPCANONE") = CDec(Cifra / 100)
                            Exit Select
                        End If
                        'AGGIUNTO PER FLUSSI MAV SENZA CANONE MA CON SPESE
                        If Mid(ExtraStr, 11, 6) <> "Canone" And CONTATORE = 101 Then
                            RwEdp("EPDESC") = ""
                            RwEdp("EPCANONE") = 0
                            CONTATORE = CONTATORE + 1
                        End If

                        If CONTATORE > 101 Then
                            RwEdp("EPDESC" & CONTATORE.ToString("000")) = Mid(ExtraStr, 11, 46)
                            If Mid(ExtraStr, 120, 1) = "-" Then segno = -1 Else segno = 1
                            Cifra = Val(Mid(ExtraStr, 112, 8)) * segno
                            RwEdp("EPSPESE" & CONTATORE.ToString("000")) = CDec(Cifra / 100)
                            RwEdp("EPDIRETTO") = 0
                            Exit Select
                        End If
                End Select
            Loop While ExtraStr <> Nothing
            If SW = 1 Then DsEdp.Rows.Add(RwEdp)
            Rea.Close()
            DaEdp.Update(DsEdp)
            DsEdp.AcceptChanges()
            File.Move(NomeB, PathBackup & FF.Name)
FineLoop:
        Next
    End Sub
    Sub CaricaIncassiMav()
        'files ESITOMAV
        Dim FF As FileInfo
        Dim dir As New DirectoryInfo(Trim(TextBox1.Text))
        For Each FF In dir.GetFiles("*.txt")
            If Mid(FF.Name, 1, 5) <> "ESMAV" Then Exit Sub
            NomeT = Trim(TextBox1.Text) & FF.Name
            If File.Exists(NomeT) = False Then Exit Sub
            DsStl = New DataTable
            DaStl = New SqlDataAdapter("SELECT * FROM TBStl WHERE SLID = 89898989898989", cnDb)
            DaStl.Fill(DsStl)
            CbStl = New SqlCommandBuilder(DaStl)
            REM RECORD DI TESTA 'IM'
            Dim Ten As New System.IO.FileStream(NomeT, IO.FileMode.Open, FileAccess.Read)
            Dim Buf As New System.IO.BufferedStream(Ten, 120)
            Dim Rea As New System.IO.StreamReader(Buf)
            'Dim Cifra As Int32
            Dim SW As Int16 = 0
            Dim DATAOP As String = ""
            Dim TIP As String
            Dim ESITO As String
            ExtraStr = Rea.ReadLine
            If Mid(ExtraStr, 2, 2) <> "IM" Then
                Rea.Close()
                GoTo FINELOOPP
            End If
            SW = 1
            DATAOP = CDate(Mid(ExtraStr, 14, 2) & "/" & Mid(ExtraStr, 16, 2) & "/20" & Mid(ExtraStr, 18, 2)).ToShortDateString
            Do
                If SW = 1 Then ExtraStr = Rea.ReadLine
                TextBox3.Text = ExtraStr
                TextBox3.Refresh()

                TIP = Mid(ExtraStr, 2, 2)

                'SE 14 E CAUSALE 07000 07011 PAGATA
                'SE 14 E MANCA DATA E' UN FILE DI ADDEBITO
                If TIP = "14" And Mid(ExtraStr, 23, 6) = "" Then
                    Rea.Close()
                    GoTo FINELOOPP
                End If

                Select Case TIP
                    Case Is = "14"
                        ESITO = Mid(ExtraStr, 29, 5)
                        If ESITO = "07000" Or ESITO = "07011" Then FLAGESITO = True Else FLAGESITO = False
                        If FLAGESITO = False Then Exit Select
                        'If SW = 1 Then DsStl.Rows.Add(RwStl) Else SW = 1
                        'DATAOP = CDate(Mid(ExtraStr, 23, 2) & "/" & Mid(ExtraStr, 25, 2) & "/20" & Mid(ExtraStr, 27, 2)).ToShortDateString
                        RwStl = DsStl.NewRow()
                        RwStl("SLDATA") = DATAOP
                        RwStl("SLCODFIS") = ""
                        RwStl("SLCODCLI") = "00000"
                        If Mid(ExtraStr, 47, 1) = "-" Then segno = -1 Else segno = 1
                        Cifra = Val(Mid(ExtraStr, 34, 13)) * segno
                        RwStl("SLIMPORTO") = CDec(Cifra / 100)
                        RwStl("SLDATAADD") = CDate(Mid(ExtraStr, 23, 2) & "/" & Mid(ExtraStr, 25, 2) & "/20" & Mid(ExtraStr, 27, 2)).ToShortDateString
                        RwStl("SLCOGE") = 0
                        Exit Select

                    Case Is = "30"
                        If FLAGESITO = False Then Exit Select
                        RwStl("SLCODFIS") = Mid(ExtraStr, 71, 16)
                        RwStl("SLCODCLI") = RecuperoCliente(Trim(Mid(ExtraStr, 71, 16)))
                        If FLAGESITO = True Then DsStl.Rows.Add(RwStl)
                        Exit Select

                    Case Is = "20"
                        Exit Select

                    Case Is = "40"
                        Exit Select

                    Case Is = "50"
                        Exit Select

                    Case Is = "51"
                        Exit Select

                    Case Is = "70"
                        Exit Select


                End Select

FINETEST:
            Loop While ExtraStr <> Nothing
            'If SW = 1 Then DsStl.Rows.Add(RwStl)
            Rea.Close()
            DaStl.Update(DsStl)
            DsStl.AcceptChanges()
            File.Move(NomeT, PathBackup & FF.Name)
        Next
FINELOOPP:
    End Sub

End Class