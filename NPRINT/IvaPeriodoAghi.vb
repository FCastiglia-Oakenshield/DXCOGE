Imports DXBASE
Imports NCCOM
Imports System.IO
Imports System.Data.SqlClient
Imports System.Drawing.Printing

Public Module IvaPeriodoAghi
    Dim Orig, Dest, FtFm, Ftmp, Concat, Stampa, QualeLp, QualeReg, NomeFile, IntestaT, Datadel As String
    Dim PathPrg, PathFil, PathSto, PathTmp, IT(6) As String
    Dim StampaLaser As Boolean = False
    Dim Trimestrale, Intesta As Boolean
    Dim TesTest, MaxProt, Np As Int32
    Dim Pagine, IdProc, MaxCorp, Linea, TipoReg, ProRata, RAQ, NumReg, AnnoIva As Int16
    Dim Bollo, OKCHIUDO As Boolean
    Dim IVALIQ(17), Esente, Credito, Acconto, MaxVerEu As Decimal
    REM VENTILAZIONE
    Dim LORDOC, TM(72), TY(72), PS(72), AA(72), WSMERANN(72), WSPSM(72), TP, TPP, TPOR, TTPOR, MERCE, INE, NNET, TX, TCN, IVAPUBB, CIIPERC(72) As Decimal
    Dim PE(72), PV, PT, CIICAMP(72), CIICMP(72), REGVENTILA As Int16
    Dim DsMer As DataSet
    Dim DaMer As SqlDataAdapter
    Dim RwMer As DataRow

    Dim DsIve As DataSet
    Dim DaIve As SqlDataAdapter
    Dim RwIve As DataRow
    REM VENTILAZIONE

    Dim DsRep As DataSet
    Dim DaRep As SqlDataAdapter
    Dim RwRep As DataRow

    Dim DsIvp As DataSet
    Dim DaIvp As SqlDataAdapter
    Dim RwIvp As DataRow

    Dim CiiPam As New ArrayList

    Public Function PrintChiusuraIva(ByVal QSTAMPANTE As String, ByVal INBOLLO As Boolean, ByVal RMIN As Int16, _
    ByVal RMAX As Int16, ByVal QANNO As Int16, ByVal TDESC As String, ByVal DEL As String, ByVal PESENTE As Decimal, _
    ByVal CREDITOP As Decimal, ByVal ACCONTOP As Decimal, ByVal LIMITE As Decimal, ByVal TRIM As Boolean, ByRef VERSAMENTO As Decimal, _
    ByRef OKCHIUDO As Boolean, ByVal LPTIPO As String, ByVal LPRESET As String, ByVal PB As Int16, ByRef PP As Int32, ByVal AZI As String, _
    ByVal DESCREG As String, ByVal SIMULATA As Boolean, ByRef INTERESSI As Decimal) As Boolean
        Dim DOVE, SIMULA As String
        If SIMULATA = False Then
            DOVE = "VRIEPIVA"
            SIMULA = ""
        Else
            DOVE = "VTMPRIEPIVA"
            SIMULA = " (SIMULATA)"
        End If
        Dim PrintUno As String = "Select DISTINCT IvaPRegIva from " & DOVE & " WHERE IvaPAnno = " & QANNO & " AND IvaPMese between " & RMIN & " And " & RMAX
        Dim PrintDue, PrintTre As String
        OKCHIUDO = False
        DsRep = New DataSet
        DaRep = New SqlDataAdapter(PrintUno, cnCo)
        DaRep.Fill(DsRep)
        TesTest = DsRep.Tables(0).Rows.Count
        If TesTest = 0 Then Exit Function
        IniziaComando(LPTIPO, LPRESET)
        PathFil = ""
        Orig = ""
        LeggiParametri(AZI)
        IntestaT = TDESC & SIMULA
        QualeLp = QSTAMPANTE
        Bollo = INBOLLO
        Datadel = DEL
        Esente = PESENTE
        Credito = CREDITOP
        Acconto = ACCONTOP
        MaxVerEu = LIMITE
        Trimestrale = TRIM
        Dest = PathTmp & "Print."
        VerifyDir()
        MaxCorp = 52
        settaggi(0)
        NomeFile = Ftmp
        Pagine = 0
        Linea = 0
        Np = PP
        AnnoIva = QANNO
        If Val(AZI) > 0 Then Intesta = True Else Intesta = False
        IT(6) = DESCREG
        SezTestata()
        Dim x As Int32
        For x = 0 To 17
            IVALIQ(x) = 0
        Next
        IVAPUBB = 0 ''' iva pubblica amministrazione
        REM AZZERAMENTI VARIABILI VENTILAZIONE
        LORDOC = 0 : TP = 0 : TPP = 0 : TPOR = 0 : TTPOR = 0 : PV = 0 : MERCE = 0 : PT = 0
        INE = 0 : NNET = 0 : TX = 0 : TCN = 0 : REGVENTILA = 0
        For x = 0 To 72
            TM(x) = 0 : TY(x) = 0 : PS(x) = 0 : AA(x) = 0 : WSMERANN(x) = 0 : WSPSM(x) = 0
            PE(x) = CIIPERC(CIICMP(x))
        Next
        REM LETTURA MERCE ANNO
        If SIMULATA = False Then
            PrintDue = "exec XMERCEV @anno=" & QANNO & ",@MESE = " & RMAX
        Else
            PrintDue = "exec XMERCEVSIM @anno=" & QANNO & ",@MESE = " & RMIN
        End If
        DsMer = New DataSet
        DaMer = New SqlDataAdapter(PrintDue, cnCo)
        DaMer.Fill(DsMer)
        For x = 1 To DsMer.Tables(0).Rows.Count
            RwMer = DsMer.Tables(0).Rows(x - 1)
            If CIICAMP(RwMer("IvaPCodIva")) <> 1 Then RwMer.Delete() Else TY(RwMer("IvaPCodIva")) = TY(RwMer("IvaPCodIva")) + RwMer("Merce")
        Next
        DsMer.AcceptChanges()
        REM INIZIALIZZO A ZERO
        PrintDue = "SELECT * FROM TbIvaV WHERE IvaVAnno =  2925"
        DsIve = New DataSet
        DaIve = New SqlDataAdapter(PrintDue, cnCo)
        DaIve.Fill(DsIve)

        REM FINE AZZERAMENTI VARIABILI VENTILAZIONE

        For x = 1 To TesTest
            RwRep = DsRep.Tables(0).Rows(x - 1)
            If SIMULATA = True Then
                PrintDue = "Select * from " & DOVE & " WHERE IvaPAnno = " & QANNO & " AND IvaPRegIva = " & RwRep("IvaPRegIva") & " AND IvaPMese between " & RMIN & " And " & RMAX
            Else
                PrintDue = "EXEC XSUMP @ANNO=" & QANNO & ",@DMESE= " & RMIN & ",@AMESE=" & RMAX & " ,@REG = " & RwRep("IvaPRegIva")
            End If
            DsIvp = New DataSet
            DaIvp = New SqlDataAdapter(PrintDue, cnCo)
            DaIvp.Fill(DsIvp)
            PrintTre = "select * from TBREGIVA where RivaAnno =" & QANNO & " And RivaNReg = " & RwRep("IvaPRegIva")
            Dim Cmd As New SqlCommand(PrintTre, cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                TipoReg = dataRd.Item("RivaTipo")
                QualeReg = dataRd.Item("RivaDesc")
                ProRata = dataRd.Item("RivaPRata")
            End While
            dataRd.Close()
            If TipoReg = 2 Or TipoReg = 4 Or TipoReg = 7 Or TipoReg = 8 Then
                RAQ = 2
            ElseIf TipoReg = 6 Then '''' IVA IN SOSPENSIONE
                RAQ = 3
            Else
                RAQ = 1
            End If
            SezCorpo()
            If TipoReg = 5 Then
                REM VENTILAZIONE
                REGVENTILA = RwRep("IvaPRegIva")
                If LORDOC > 0 Then
                    SEZMERCE()
                    TOTSCO()
                    VENTILA(QANNO, RMAX)
                End If
                If Bollo = True Then
                    If LORDOC > 0 Then
                        AggiornaVentila()
                    End If
                    PrintDue = "Select * from VRiepiva WHERE IvaPAnno = " & QANNO & " AND IvaPRegIva = " & RwRep("IvaPRegIva") & " AND IvaPMese between " & RMIN & " And " & RMAX
                    DsIvp = New DataSet
                    DaIvp = New SqlDataAdapter(PrintDue, cnCo)
                    DaIvp.Fill(DsIvp)
                    AggiornaCorrispettivi()
                End If
                REM VENTILAZIONE
                LORDOC = 0
            End If
        Next
        SezChiude()
        VERSAMENTO = IVALIQ(10) : INTERESSI = IVALIQ(16)
        settaggi(1)
        OKCHIUDO = True
        PP = Np
    End Function
    Private Sub AggiornaVentila()
        Dim k As Int16
        Dim Str As String = "Insert into TbIvaV (IvaVAnno,IvaVRegIva,IvaVMese,IvaVCodIva,IvaVAcLordi,IvaVPerComp,IvaVLordi,IvaVCMPIva,IvaVNetti,IvaVIva) VALUES (@IvaVAnno,@IvaVRegIva,@IvaVMese,@IvaVCodIva,@IvaVAcLordi,@IvaVPerComp,@IvaVLordi,@IvaVCMPIva,@IvaVNetti,@IvaVIva)"
        Cmd = New SqlCommand(Str, cnCo)
        Dim p1 As New SqlParameter("@IvaVAnno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@IvaVMese", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@IvaVRegIva", SqlDbType.SmallInt)
        Dim p4 As New SqlParameter("@IvaVCodIva", SqlDbType.SmallInt)
        Dim p5 As New SqlParameter("@IvaVAcLordi", SqlDbType.Decimal)
        Dim p6 As New SqlParameter("@IvaVPerComp", SqlDbType.Decimal)
        Dim p7 As New SqlParameter("@IvaVLordi", SqlDbType.Decimal)
        Dim p8 As New SqlParameter("@IvaVCMPIva", SqlDbType.SmallInt)
        Dim p9 As New SqlParameter("@IvaVNetti", SqlDbType.Decimal)
        Dim p10 As New SqlParameter("@IvaVIva", SqlDbType.Decimal)
        REM ATTENZIONE obbligatorio 1 SOLO REGISTRO SOGGETTO A VENTILAZIONE
        For k = 1 To DsIve.Tables(0).Rows.Count
            RwIve = DsIve.Tables(0).Rows(k - 1)
            If k = 1 Then
                Dim ELIMINA As New SqlCommand("Delete from TbIvaV where IvaVanno = " & RwIve("IvaVAnno") & " and IvaVMese = " & RwIve("IvaVMese"), cnCo)
                ELIMINA.ExecuteNonQuery()
            End If
            p1.Value = RwIve("IvaVAnno")
            p2.Value = RwIve("IvaVMese")
            p3.Value = RwIve("IvaVRegIva")
            p4.Value = RwIve("IvaVCodIva")
            p5.Value = RwIve("IvaVAcLordi")
            p6.Value = RwIve("IvaVPerComp")
            p7.Value = RwIve("IvaVLordi")
            p8.Value = RwIve("IvaVCMPIva")
            p9.Value = RwIve("IvaVNetti")
            p10.Value = RwIve("IvaVIva")
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
            Cmd.Parameters.Add(p10)
            Cmd.ExecuteNonQuery()
        Next
    End Sub
    Function VENTILA(ByVal ANNO, ByVal MESE) As Boolean
        TP = TP - 100
        If TP = 0 Then GoTo AVENTILA
        If TP > 0 Then PS(PT) = PS(PT) - TP Else TPP = TP : PS(PT) = PS(PT) + TPP
AVENTILA:
        TPOR = TPOR - LORDOC
        If TPOR = 0 Then GoTo SVENTILA
        If TPOR > 0 Then AA(PT) = AA(PT) - TPOR Else TTPOR = TPOR : AA(PT) = AA(PT) + TTPOR
SVENTILA:
        Dim PRT(11), LpVe(11) As String
        For x As Int16 = 0 To 11
            PRT(x) = "" : LpVe(x) = ""
        Next

        PRT(0) = "".PadLeft(16, " ") & " VENTILAZIONE CORRISPETTIVI " & Format(LORDOC, "###,###,##0.00")

        PRT(1) = "".PadLeft(16, " ") & "".PadRight(92, "-")
        PRT(2) = "".PadLeft(16, " ") & "|" & "Aliq. " & "|" & "Acq. Lordi  " & "|" & " % Composizione" & "|" & " Corr.Lordi  " & "|" & "Aliq. " & "|" & "Corrispett.Netti" & "|" & "Iva Corrispett. " & "|"
        PRT(3) = "".PadLeft(16, " ") & "".PadRight(92, "=")

        ''' contatore
        If (Linea + 3) > MaxCorp Then
            Linea = Linea + 3
            Contatore()
            Linea = Linea - 1
        End If
        Linea = Linea + 3
        PrintLine(1, PRT(0))
        PrintLine(1, PRT(1))
        PrintLine(1, PRT(2))
        PrintLine(1, PRT(1))
WVENTILA:
        For i As Int16 = 1 To 72
            If TM(i) = 0 Then WSPSM(i) = 0 : GoTo ZVENTILA
            Contatore()
            NNET = AA(i) * 100 / (100 + PE(i))
            INE = NNET * PE(i) / 100
            LpVe(1) = Format(CIIPERC(i), "###.00")
            LpVe(2) = Format(TM(i), "#,###,##0.00")
            LpVe(3) = Format(PS(i), "###.0000000000")
            LpVe(4) = Format(AA(i), "#,###,##0.00")
            LpVe(5) = Format(PE(i), "###.00")
            WSPSM(i) = Format(PE(i), "###.00")
            LpVe(6) = Format(NNET, "###,###,##0.00")
            LpVe(7) = Format(INE, "###,###,##0.00")
            TCN = TCN + Format(NNET, "###,###,##0.00")
            TX = TX + Format(INE, "###,###,##0.00")
            RwIve = DsIve.Tables(0).NewRow()
            RwIve("IvaVAnno") = ANNO
            RwIve("IvaVRegIva") = REGVENTILA
            RwIve("IvaVMese") = MESE
            RwIve("IvaVCodIva") = i
            RwIve("IvaVAcLordi") = Format(TM(i), "#,###,##0.00")
            RwIve("IvaVPerComp") = Format(PS(i), "###.0000000000")
            RwIve("IvaVLordi") = Format(AA(i), "#,###,##0.00")
            RwIve("IvaVCMPIva") = CIICMP(i)
            RwIve("IvaVNetti") = Format(NNET, "###,###,##0.00")
            RwIve("IvaVIva") = Format(INE, "###,###,##0.00")
            DsIve.Tables(0).Rows.Add(RwIve)
            LpVch(LpVe)
            For z As Int16 = 1 To 7
                LpVe(11) = LpVe(11) & LpVe(z)
            Next
            PrintLine(1, TAB(1), LpVe(11))
ZVENTILA:
            For z As Int16 = 0 To 11
                LpVe(Z) = ""
            Next
        Next
        If (Linea + 3) > MaxCorp Then
            Linea = Linea + 3
            Contatore()
            Linea = Linea - 1
        End If
        Linea = Linea + 3
        PrintLine(1, PRT(3))

        LpVe(1) = "Totale"
        LpVe(2) = Format(MERCE, "#,###,##0.00")
        LpVe(3) = "100.0000000000"
        LpVe(4) = Format(LORDOC, "#,###,##0.00")
        LpVe(5) = ""
        LpVe(6) = Format(TCN, "###,###,##0.00")
        LpVe(7) = Format(TX, "###,###,##0.00")
        IVALIQ(1) = IVALIQ(1) + TX
        LpVch(LpVe)
        For z As Int16 = 1 To 7
            LpVe(11) = LpVe(11) & LpVe(z)
        Next
        PrintLine(1, TAB(1), LpVe(11))
        PrintLine(1, PRT(3))
        PrintLine(1)
        DsIve.AcceptChanges()
    End Function
    Private Sub LpVch(ByVal Lpve() As String)
        Lpve(11) = Lpve(11).PadLeft(16, " ") & "|"
        Lpve(1) = Lpve(1).PadLeft(6, " ") & "|"
        Lpve(2) = Lpve(2).PadLeft(12, " ") & "|"
        Lpve(3) = Lpve(3).PadLeft(15, " ") & "|"
        Lpve(4) = Lpve(4).PadLeft(13, " ") & "|"
        Lpve(5) = Lpve(5).PadLeft(6, " ") & "|"
        Lpve(6) = Lpve(6).PadLeft(16, " ") & "|"
        Lpve(7) = Lpve(7).PadLeft(16, " ") & "|"
    End Sub
    Sub TOTSCO()
        For I As Int16 = 1 To 72
            If TM(I) > 0 Then
                PS(I) = (TM(I) / MERCE) * 100
                AA(I) = LORDOC * PS(I) / 100
                If AA(I) > 0 Then PT = I
                TP = TP + PS(I)
                TPOR = TPOR + AA(I)
            End If
        Next
    End Sub
    Sub SEZMERCE()
        Dim P As Int16 = 0
        MERCE = 0
        For I As Int16 = 1 To DsMer.Tables(0).Rows.Count
            RwMer = DsMer.Tables(0).Rows(I - 1)
            P = RwMer("IvaPcodIva")
            WSMERANN(P) = RwMer("Merce") * (CIIPERC(P) + 100) / 100
            TM(P) = WSMERANN(P)
            MERCE = MERCE + TM(P)
        Next
    End Sub
    Private Sub AggiornaCorrispettivi()
        Dim k As Int16
        Dim Str As String
        For k = 0 To DsIvp.Tables(0).Rows.Count - 1
            RwIvp = DsIvp.Tables(0).Rows(k)
            Str = "Update TbIvaP set IvaPImpon = @Impon ,IvaPIvaDe = @IvaDe  " _
            & "Where IvaPAnno = @Anno and IvaPMese = @Mese and IvaPRegIva = @Reg and IvaPCodIva = @CodIva"
            Cmd = New SqlCommand(Str, cnCo)
            Dim p1 As New SqlParameter("@Anno", SqlDbType.SmallInt)
            Dim p2 As New SqlParameter("@Mese", SqlDbType.SmallInt)
            Dim p3 As New SqlParameter("@Impon", SqlDbType.Decimal)
            Dim p4 As New SqlParameter("@IvaDe", SqlDbType.Decimal)
            Dim p5 As New SqlParameter("@Reg", SqlDbType.SmallInt)
            Dim p6 As New SqlParameter("@CodIva", SqlDbType.SmallInt)


            RwIvp("Timpon") = RwIvp("Timpon") + RwIvp("IvaDe")
            RwIvp("IvaDe") = RwIvp("Timpon") / (100 + RwIvp("CiiAli")) * 100
            RwIvp("IvaND") = Format(RwIvp("IvaDe") * RwIvp("CiiAli") / 100, "###,###,##0.00")
            p1.Value = RwIvp("IvaPAnno")
            p2.Value = RwIvp("IvaPMese")
            p3.Value = RwIvp("IvaDe")
            p4.Value = RwIvp("IvaNd")
            p5.Value = RwIvp("IvaPRegIva")
            p6.Value = RwIvp("IvaPCodIva")
            Cmd.Parameters.Clear()
            Cmd.Parameters.Add(p1)
            Cmd.Parameters.Add(p2)
            Cmd.Parameters.Add(p3)
            Cmd.Parameters.Add(p4)
            Cmd.Parameters.Add(p5)
            Cmd.Parameters.Add(p6)
            Cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Function SezChiude() As Boolean
        Dim f, z, x As Int16
        Dim Lpdec(16) As String
        Dim PRT(16), WT(16), PIPA(1), Str As String
        Dim Esente98 As Decimal = 0
        For x = 0 To 16
            PRT(x) = ""
            WT(x) = ""
            Lpdec(x) = ""
        Next
        PIPA(0) = "".PadLeft(17, " ") & "   7]-IVA PUBBLICA AMMINISTRAZIONE"
        PIPA(1) = "".PadLeft(17, " ") & "      ( E. " & Format(IVAPUBB, "###,###,##0.00") & " )"
        PRT(1) = "".PadLeft(17, " ") & "   1]-IVA A DEBITO :"
        PRT(2) = "".PadLeft(17, " ") & "      a)-Iva su cessioni e prestazioni         E."
        Str = "".PadLeft(66, " ")
        PRT(0) = Str.PadRight(82, "-")
        Str = "".PadLeft(66, " ")
        PRT(13) = Str.PadRight(82, "=")
        PRT(3) = "".PadLeft(17, " ") & "   2]-IVA A CREDITO:"
        PRT(4) = "".PadLeft(17, " ") & "      a)-Iva sugli Acquisti                    E."
        Str = "".PadLeft(17, " ") & "   3]-SALDO ATTUALE"
        PRT(5) = Str.PadRight(59, " ") & "1-2] E. "
        PRT(6) = "".PadLeft(17, " ") & "      d)-I.v.a. detraibile  per operazioni"
        WT(0) = "".PadLeft(17, " ") & "         esenti "
        WT(1) = " %  su E."
        WT(2) = " E.  "
        PRT(7) = "".PadRight(61, " ") & "1] E. "
        PRT(8) = "".PadRight(61, " ") & "2] E. "
        WT(3) = "".PadLeft(17, " ") & "   4]-"
        WT(5) = Str.PadRight(59, " ") & "   ] E. "
        WT(6) = "".PadLeft(17, " ") & "   5]-"
        PRT(11) = "".PadLeft(17, " ") & "                        Maggiorazione 1 %      E."
        Str = "".PadLeft(17, " ") & "   6]-IVA DA VERSARE"
        PRT(12) = Str.PadRight(59, " ") & "     E. "
        'ESENZIONE PRORATA
        If Esente > 0 Then
            Esente98 = 100 - Esente
            IVALIQ(5) = Format(IVALIQ(17) * Esente98 / 100, "###,###,##0.00")
            Lpdec(3) = Format(Esente98, "##0.00")
            Lpdec(3) = Lpdec(3).PadLeft(6, " ")
            Lpdec(4) = Format(IVALIQ(17), "###,###,##0.00")
            Lpdec(4) = Lpdec(4).PadLeft(15, " ")
            Lpdec(5) = Format(IVALIQ(5), "###,###,##0.00")
            Lpdec(5) = Lpdec(5).PadLeft(15, " ")
        End If
        If Esente = 100 Or Esente98 > 0 Then
            IVALIQ(6) = IVALIQ(2) + IVALIQ(5)
        Else
            IVALIQ(6) = IVALIQ(2) + IVALIQ(13) + IVALIQ(4) + IVALIQ(5)
        End If

        IVALIQ(7) = IVALIQ(1) - IVALIQ(6)
        IVALIQ(8) = IVALIQ(7) + Credito - Acconto
        If IVALIQ(8) <= MaxVerEu Then
            IVALIQ(10) = IVALIQ(8)
            GoTo TLIQUIDA
        End If
        If Trimestrale = True Then
            IVALIQ(16) = Format(IVALIQ(8) * 1 / 100, "###,###,##0.00")
        End If
        'IVALIQ(8) = IVALIQ(8) + IVALIQ(16)
        'IVALIQ(10) = IVALIQ(8)

        IVALIQ(10) = IVALIQ(8) + IVALIQ(16)
TLIQUIDA:
        ''' EVENTUALI ESTREMI DI VERSAMENTO

        Lpdec(1) = Format(IVALIQ(1), "###,###,##0.00")
        Lpdec(2) = Format(IVALIQ(2), "###,###,##0.00")
        Lpdec(6) = Format(IVALIQ(6), "###,###,##0.00")
        Lpdec(7) = Format(IVALIQ(7), "###,###,##0.00")
        If Credito < 0 Then WT(4) = "CREDITO PRECEDENTE" Else WT(4) = "DEBITO PRECEDENTE"
        PRT(9) = WT(3) & WT(4)
        PRT(9) = PRT(9).PadRight(59, " ") & "     E. "
        Lpdec(8) = Format(Credito, "###,###,##0.00")
        If Acconto > 0 Then
            PRT(16) = "".PadLeft(23, " ") & "DEDOTTO ACCONTO I.V.A"
            PRT(16) = PRT(16).PadRight(59, " ") & "     E. "
            Lpdec(11) = Format((Acconto * -1), "###,###,##0.00")
        End If

        If IVALIQ(8) > 0 Then WT(7) = "DEBITO ATTUALE" Else WT(7) = "CREDITO ATTUALE"
        PRT(10) = WT(6) & WT(7)
        PRT(10) = PRT(10).PadRight(59, " ") & "     E. "
        Lpdec(9) = Format(IVALIQ(8), "###,###,##0.00")
        Lpdec(10) = Format(IVALIQ(16), "###,###,##0.00")
        Lpdec(12) = Format(IVALIQ(10), "###,###,##0.00")
        Lplp(Lpdec)
        ' iva a debito
        If (Linea + 3) > MaxCorp Then
            Linea = Linea + 3
            Contatore()
            Linea = Linea - 1
        End If
        Linea = Linea + 3
        PrintLine(1, PRT(1))
        PrintLine(1, PRT(2), TAB(68), Lpdec(1))
        PrintLine(1, PRT(0))
        ' iva a credito 
        If (Linea + 3) > MaxCorp Then
            Linea = Linea + 3
            Contatore()
            Linea = Linea - 1
        End If
        Linea = Linea + 3
        PrintLine(1, PRT(7), TAB(68), Lpdec(1))
        PrintLine(1, PRT(3))
        PrintLine(1, PRT(4), TAB(68), Lpdec(2))

        ''' prorata
        If Esente > 0 Then
            If (Linea + 2) > MaxCorp Then
                Linea = Linea + 2
                Contatore()
                Linea = Linea - 1
            End If
            Linea = Linea + 2
            PrintLine(1, PRT(6))
            PrintLine(1, WT(0) & Lpdec(3) & WT(1) & Lpdec(4) & WT(2) & Lpdec(5))
        End If
        ' 1-2 e acconto
        If (Linea + 4) > MaxCorp Then
            Linea = Linea + 3
            Contatore()
            Linea = Linea - 1
        End If
        Linea = Linea + 3
        PrintLine(1, PRT(0))
        PrintLine(1, PRT(8), TAB(68), Lpdec(6))
        PrintLine(1, PRT(5), TAB(68), Lpdec(7))

        If Acconto > 0 Then
            Contatore()
            PrintLine(1, PRT(16), TAB(68), Lpdec(11))
        End If
        'credito
        If Credito <> 0 Then
            Contatore()
            PrintLine(1, PRT(9), TAB(68), Lpdec(8))
        End If
        ' debito credito attuale
        If (Linea + 2) > MaxCorp Then
            Linea = Linea + 2
            Contatore()
            Linea = Linea - 1
        End If
        Linea = Linea + 2
        PrintLine(1, PRT(0))
        PrintLine(1, PRT(10), TAB(68), Lpdec(9))
        'trimestrale
        If Trimestrale = True And IVALIQ(16) > 0 Then
            Contatore()
            PrintLine(1, PRT(11), TAB(68), Lpdec(10))
        End If
        Linea = Linea + 1
        PrintLine(1, PRT(0))
        ' iva da versare
        If IVALIQ(10) > MaxVerEu Then
            If (Linea + 2) > MaxCorp Then
                Linea = Linea + 2
                Contatore()
                Linea = Linea - 1
            End If
            Linea = Linea + 2
            PrintLine(1, PRT(12), TAB(68), Lpdec(12))
            PrintLine(1, PRT(13))
        End If
        'IVA PUBBLICA AMMINISTRAZIONE
        If IVAPUBB > 0 Then
            If (Linea + 2) > MaxCorp Then
                Linea = Linea + 2
                Contatore()
                Linea = Linea - 1
            End If
            Linea = Linea + 2
            PrintLine(1, PIPA(0))
            PrintLine(1, PIPA(1))
        End If
        Linea = Linea + 1
        PRT(15) = " " & PRT(15).PadLeft(130, "*")
        PrintLine(1, PRT(15))
        If (Linea + 5) >= MaxCorp Then GoTo ChiudeDefinitivamente
        Contatore()
        f = MaxCorp - (Linea + 2)
        If f > 0 Then
            For z = Linea + 2 To MaxCorp
                PrintLine(1, TAB(z + 2), "*")
            Next
            PrintLine(1, PRT(15))
        End If
ChiudeDefinitivamente:
        PrintLine(1, Chr(12))
        FileClose(1)   ' Close file.
    End Function
    Private Sub Contatore()
        Linea = Linea + 1
        If Linea > MaxCorp Then
            SezTestata()
            Linea = Linea + 1
        End If
    End Sub
    Private Sub Lplp(ByVal Lpdec() As String)
        Lpdec(12) = Lpdec(12).PadLeft(15, " ")
        Lpdec(1) = Lpdec(1).PadLeft(15, " ")
        Lpdec(2) = Lpdec(2).PadLeft(15, " ")
        Lpdec(3) = Lpdec(3).PadLeft(6, " ")
        Lpdec(4) = Lpdec(4).PadLeft(14, " ")
        Lpdec(5) = Lpdec(5).PadLeft(15, " ")
        Lpdec(6) = Lpdec(6).PadLeft(15, " ")
        Lpdec(7) = Lpdec(7).PadLeft(15, " ")
        Lpdec(8) = Lpdec(8).PadLeft(15, " ")
        Lpdec(9) = Lpdec(9).PadLeft(15, " ")
        Lpdec(10) = Lpdec(10).PadLeft(15, " ")
        Lpdec(11) = Lpdec(11).PadLeft(15, " ")
    End Sub
    Private Sub Lpch(ByVal Lpdec() As String)
        Lpdec(11) = Lpdec(11).PadLeft(16, " ") & "|"
        Lpdec(0) = Lpdec(0).PadRight(12, " ") & "|"
        Lpdec(1) = Lpdec(1).PadLeft(15, " ") & "|"
        Lpdec(2) = Lpdec(2).PadLeft(14, " ") & "|"
        Lpdec(3) = Lpdec(3).PadLeft(14, " ") & "|"
        Lpdec(4) = Lpdec(4).PadLeft(15, " ") & "|"
        Lpdec(5) = Lpdec(5).PadLeft(15, " ") & "|"
    End Sub
    Private Function SezCorpo() As Boolean
        Dim Lpdec(12) As String
        Dim k, z As Int16
        Dim PRT(6), WT(6) As String
        Dim x, WP(1) As Int16
        Dim Impon, IvaDe, IvaNd, Merce, Totale As Decimal
        Impon = 0
        IvaDe = 0
        IvaNd = 0
        Merce = 0
        Totale = 0
        For x = 0 To 6
            PRT(x) = ""
            WT(x) = ""
        Next
        PRT(3) = PRT(3).PadLeft(17, " ") & " N' " & RwRep("IvaPRegIva") & "".PadLeft(15) & QualeReg
        PRT(0) = "".PadLeft(16, " ") & "".PadRight(92, "-")
        PRT(4) = " " & PRT(4).PadLeft(130, "*")
        WT(1) = "".PadLeft(16, " ") & "|" & "Aliquota Iva" & "|" & "  Imponibile o " & "|" & "    I.v.a.    " & "|" & "    I.v.a.    " & "|" _
        & "  Imponibile o " & "|" & "  T O T A L E  " & "|"
        WT(2) = "".PadLeft(16, " ") & "|" & "  Causale   " & "|" & "    Importo    " & "|" & "  Detraibile  " & "|" & " Indetraibile " & "|" _
        & " Importo Merci " & "|" & " F A T T U R E " & "|"

        WT(3) = "".PadLeft(16, " ") & "|" & "Aliquota Iva" & "|" & "    Importo    " & "|" & "    Importo   " & "|" & "    I.v.a.    " & "|" _
            & "               " & "|" & "  T O T A L E  " & "|"
        WT(4) = "".PadLeft(16, " ") & "|" & "  Causale   " & "|" & "    Lordo      " & "|" & "    Netto     " & "|" & "              " & "|" _
        & "               " & "|" & " CORRISPETTIVO " & "|"
        REM pubblica amministrazione
        WT(5) = "".PadLeft(16, " ") & "|" & "Aliquota Iva" & "|" & "  Imponibile o " & "|" & "    I.v.a.    " & "|" & "I.v.a. Indetr." & "|" _
             & "  Imponibile o " & "|" & "  T O T A L E  " & "|"
        WT(6) = "".PadLeft(16, " ") & "|" & "  Causale   " & "|" & "    Importo    " & "|" & "  Detraibile  " & "|" & " o Pubb.Ammin." & "|" _
        & " Importo Merci " & "|" & " F A T T U R E " & "|"


        If TipoReg = 5 Then
            PRT(1) = WT(3)
            PRT(2) = WT(4)
        Else
            PRT(1) = WT(1)
            PRT(2) = WT(2)
        End If
        If TipoReg = 1 And CiiPam.Count > 0 Then
            PRT(1) = WT(5)
            PRT(2) = WT(6)
        End If

        If (Linea + 6) > MaxCorp Then
            Linea = Linea + 6
            Contatore()
            Linea = Linea - 1
        End If
        Linea = Linea + 5 ' PER EVITARE DI STAMPARE SOLO RIGHE DI INTESTAZIONE ???? DA RIVEDERE  + 1 !!!!!
        PrintLine(1, PRT(3))
        PrintLine(1, PRT(0))
        PrintLine(1, PRT(1))
        PrintLine(1, PRT(2))
        PrintLine(1, PRT(0))
        For k = 0 To DsIvp.Tables(0).Rows.Count - 1
            RwIvp = DsIvp.Tables(0).Rows(k)
            For z = 0 To 12
                Lpdec(z) = ""
            Next
            If TipoReg = 5 Then
                RwIvp("Timpon") = RwIvp("Timpon") + RwIvp("IvaDe")
                RwIvp("IvaDe") = RwIvp("Timpon") / (100 + RwIvp("CiiAli")) * 100
                RwIvp("IvaND") = Format(RwIvp("IvaDe") * RwIvp("CiiAli") / 100, "###,###,##0.00")
                RwIvp("Merce") = 0
                Totale = Totale + RwIvp("Timpon")
                If RwIvp("IvaPCodIva") = 32 Then LORDOC = LORDOC + RwIvp("Timpon")
            Else
                Totale = Totale + RwIvp("Timpon") + RwIvp("IvaDe") + RwIvp("IvaND")
                REM If TipoReg = 2 And RwIvp("Merce") > 0 Then SIMERC()
            End If
            REM IVA PUBBLICA AMMINISTRAZIONE
            If TipoReg = 1 And CiiPam.Count > 0 Then
                For J As Int16 = 1 To CiiPam.Count
                    If CiiPam(J - 1) = RwIvp("IvaPCodIva") Then
                        IVAPUBB += RwIvp("IvaND")
                    End If
                Next
            End If

            Impon = Impon + RwIvp("Timpon")
            IvaDe = IvaDe + RwIvp("IvaDe")
            IvaNd = IvaNd + RwIvp("IvaND")
            Merce = Merce + RwIvp("Merce")
            Lpdec(0) = RwIvp("CiiDes")
            If RwIvp("Timpon") <> 0 Then Lpdec(1) = Format(RwIvp("Timpon"), "###,###,##0.00")
            If RwIvp("IvaDe") <> 0 Then Lpdec(2) = Format(RwIvp("IvaDe"), "###,###,##0.00")
            If RwIvp("IvaND") <> 0 Then Lpdec(3) = Format(RwIvp("IvaND"), "###,###,##0.00")
            If RwIvp("Merce") <> 0 Then Lpdec(4) = Format(RwIvp("Merce"), "###,###,##0.00")
            Contatore()
            Lpch(Lpdec)
            For z = 0 To 5
                Lpdec(11) = Lpdec(11) & Lpdec(z)
            Next
            PrintLine(1, TAB(1), Lpdec(11))
        Next
        Contatore()
        PrintLine(1, PRT(0))
        For z = 0 To 12
            Lpdec(z) = ""
        Next
        Lpdec(0) = "   TOTALI"
        If Impon <> 0 Then Lpdec(1) = Format(Impon, "###,###,##0.00")
        If IvaDe <> 0 Then Lpdec(2) = Format(IvaDe, "###,###,##0.00")
        If IvaNd <> 0 Then Lpdec(3) = Format(IvaNd, "###,###,##0.00")
        If Merce <> 0 Then Lpdec(4) = Format(Merce, "###,###,##0.00")
        If Totale <> 0 Then Lpdec(5) = Format(Totale, "###,###,##0.00")
        Lpch(Lpdec)
        For z = 0 To 5
            Lpdec(11) = Lpdec(11) & Lpdec(z)
        Next
        Contatore()
        PrintLine(1, TAB(1), Lpdec(11))
        If TipoReg = 6 Then
            IVALIQ(RAQ) = IVALIQ(RAQ) + IvaDe '?'?'?''' IVA IN SOSPENSIONE cosa faccio
        Else
            If TipoReg = 5 Then
                IVALIQ(RAQ) = IVALIQ(RAQ) + IvaNd
            Else
                IVALIQ(RAQ) = IVALIQ(RAQ) + IvaDe
            End If
        End If
        If TipoReg = 7 Then
            IVALIQ(RAQ) = IVALIQ(RAQ) - IvaDe
        End If
        If Math.Abs(ProRata) = 1 Then
            IVALIQ(17) = IVALIQ(17) + IvaDe
            IVALIQ(RAQ) = IVALIQ(RAQ) - IvaDe
        End If
        Linea = Linea + 1
        PrintLine(1, PRT(0))
        RigheVuote(1)
        Contatore()
    End Function
    'REM VENTILAZIONE
    'Sub SIMERC()
    ' PV = CIICAMP(RwIvp("IvaPCodIva"))
    '    If PV = 1 Then TY(RwIvp("IvaPCodIva")) = TY(RwIvp("IvaPCodIva")) + RwIvp("Merce")
    'End Sub
    Private Function SezTestata() As Boolean
        Dim PRT, IRT(4) As String
        Dim x As Int16
        For x = 0 To 4
            IRT(x) = ""
        Next
        PRT = "".PadLeft(17, " ") & "LIQUIDAZIONE I.V.A. DEL " & Datadel & " RELATIVA " & IntestaT
        Pagine = Pagine + 1
        If Pagine > 1 Then
            PrintLine(1, Chr(12))
        End If
        If Pagine = 1 Then FileOpen(1, NomeFile, OpenMode.Append) ' Open file for output.
        If Trim(ComandoInizializza) > "" Then PrintLine(1, ComandoInizializza)
        '' INTESTAZIONE Chiusura '''
        If Intesta = False Then
            Np = Np + 1
            RigheVuote(6)
        Else
            Np = Np + 1
            IRT(0) = " ".PadRight(130, "-") & " "
            IRT(1) = " " & IT(1).PadRight(130, " ") & " "
            IRT(2) = " " & IT(2).PadRight(130, " ") & " "
            IRT(3) = " " & IT(3).PadRight(130, " ") & " "
            IRT(4) = " " & IT(0).PadRight(55, " ") & IT(6).PadRight(51, " ") & "Pagina N. " & AnnoIva & " / " & Np.ToString.PadLeft(6, " ") & "  "
            PrintLine(1, IRT(0))
            PrintLine(1, IRT(1))
            PrintLine(1, IRT(2))
            PrintLine(1, IRT(3))
            PrintLine(1, IRT(4))
            PrintLine(1, IRT(0))
        End If
        '' INTESTAZIONE Chiusura '''
        PrintLine(1, PRT)
        RigheVuote(1)
        Linea = 2
    End Function

    Private Function RigheVuote(ByVal n) As Boolean
        Dim x As Int16
        For x = 1 To n
            PrintLine(1)
        Next
    End Function
    Private Sub VerifyDir()
        If Directory.Exists(PathSto) = False Then Directory.CreateDirectory(PathSto)
        If Directory.Exists(PathTmp) = False Then Directory.CreateDirectory(PathTmp)
    End Sub

    Private Function settaggi(ByVal ID) As Boolean
        If ID = 1 Then GoTo fine
Inizio:
        Ftmp = Dest & "01"
        Concat = Ftmp
        FileOpen(1, Ftmp, OpenMode.Output)
        FileClose(1)
        Exit Function
fine:
        FtFm = Dest & "Prn"
        Dim NomeCmd As String = PathTmp & "Print.bat"
        File.Delete(NomeCmd)
        Dim FileCmd As New System.IO.FileStream(NomeCmd, IO.FileMode.Create, FileAccess.Write)
        Dim BufCmd As New System.IO.BufferedStream(FileCmd, 1024)
        Dim WrCmd As New System.IO.StreamWriter(BufCmd)
        Stampa = "Copy /b " & Concat & " " & FtFm
        WrCmd.WriteLine(Stampa)
        WrCmd.Close()
        IdProc = Shell(NomeCmd, AppWinStyle.Hide, True)
        If Pagine > 0 Then
            System.IO.File.Copy(FtFm, QualeLp, True)
        End If
    End Function
    Private Function CopiaLaser() As Boolean
        System.IO.File.Copy(Orig, Ftmp, True)
    End Function
    Private Function LeggiParametri(ByVal AZI As String) As Boolean
        Dim cmd As New SqlCommand(" SELECT * from TbSel where selId = 1", cnVd)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            PathPrg = dataRd.GetString(12) & dataRd.GetString(1)
            PathSto = dataRd.GetString(12) & dataRd.GetString(11)
            PathTmp = dataRd.GetString(8)
        End While
        dataRd.Close()
        REM VENTILAZIONE
        cmd = New SqlCommand("SELECT * from TbCii", cnCo)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            CIICAMP(dataRd.Item("CiiCod")) = dataRd.Item("CiiTp")
            CIIPERC(dataRd.Item("CiiCod")) = dataRd.Item("CiiAli")
            CIICMP(dataRd.Item("CiiCod")) = dataRd.Item("CiiCmp")
        End While
        dataRd.Close()
        Dim z As Int16
        For z = 0 To 6
            IT(z) = ""
        Next
        If Val(AZI) = 0 Then Exit Function
        cmd = New SqlCommand("SELECT * from TbAna where anaGrp = 'AZ' and ANACOD = '" & AZI & "'", cnVd)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            IT(1) = dataRd.Item("AnaDesc")
            IT(2) = dataRd.Item("AnaIndirizzo")
            IT(3) = Trim(dataRd.Item("AnaCap")) & " " & Trim(dataRd.Item("AnaCitta")) & " " & Trim(dataRd.Item("AnaProv"))
            IT(0) = Trim(dataRd.Item("AnaPiva")) & " - " & Trim(dataRd.Item("AnaCFis"))
        End While
        dataRd.Close()
        REM lettura codici iva pubblica amministrazione
        CiiPam.Clear()
        cmd = New SqlCommand("SELECT * from TbPaCii where PaTipo = 'IVA' order by PaCodIva", cnCo)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            CiiPam.Add(dataRd.Item("PaCodIva"))
        End While
        dataRd.Close()
    End Function
End Module
