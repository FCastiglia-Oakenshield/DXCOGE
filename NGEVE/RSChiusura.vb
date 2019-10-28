Imports DXBASE
Imports DXBASE.Util
Imports System.IO
Imports System.Data.SqlClient

Public Module RSChiusura
    Dim swf, swd, CptCpt(11), SorCpt(11), CptSpe As String
    Dim TesTest, swR As Int32
    Dim CiPerc(72), Nrate, TipoPag, Pnt, CiiCpt(11), NonImpEse, CodCodIva, RegDiff, RegNcr, Segno As Int16
    Dim Imp(72), Iva(72), UltSco, TotCpt(11), IvaCpt(11), ImpCpt(11), TotMerce, TotImporto As Decimal
    Dim TotEse, TotSco, ImpSpese As Decimal
    Dim DaTai As SqlDataAdapter
    Dim DsTai As DataSet
    Dim DaRif As SqlDataAdapter
    Dim DsRif As DataSet
    Dim RwRif As DataRow
    Dim CbTai As SqlCommandBuilder
    Dim RwTai As DataRow
    Dim Spese As Decimal
    Dim CiSpe, CiImb As Int16
    Dim ScMax As Decimal
    Dim ScMin As Decimal
    Dim Di1, Di2 As Date
    Dim Qui As Int32 = 999999999
    Dim TaiD As String = "TbTai"
    Dim scrive As String = "Insert Into TbDcg (DcgTrasf, DcgData, DcgRegistro, DcgNumero, DcgCli,DcgCodAge,DcgImp1,DcgCodIva1,DcgIva1," _
    & "DcgCptCon1,DcgImp2,DcgCodIva2,DcgIva2,DcgCptCon2,DcgImp3,DcgCodIva3,DcgIva3,DcgCptCon3," _
    & "DcgImp4,DcgCodIva4,DcgIva4,DcgCptCon4,DcgImp5,DcgCodIva5,DcgIva5,DcgCptCon5,DcgImp6," _
    & "DcgCodIva6,DcgIva6,DcgCptCon6,DcgImp7,DcgCodIva7,DcgIva7,DcgCptCon7,DcgImp8,DcgCodIva8,DcgIva8," _
    & "DcgCptCon8,DcgImp9,DcgCodIva9,DcgIva9,DcgCptCon9,DcgImp10,DcgCodIva10,DcgIva10,DcgCptCon10," _
    & "DcgImp11, DcgCodIva11, DcgIva11,DcgCptCon11,DcgAcconti,DcgNBol,DcgTotMerce,DcgSpeBolli,DcgSpeBolliCi,DcgSpeRb," _
    & "DcgSpeRbCi,DcgRieImp1,DcgRieCi1,DcgRieIva1,DcgRieImp2,DcgRieCi2,DcgRieIva2,DcgRieImp3," _
    & "DcgRieCi3, DcgRieIva3, DcgRieImp4, DcgRieCi4, DcgRieIva4, DcgRieImp5, DcgRieCi5, DcgRieIva5," _
    & "DcgRieImp6, DcgRieCi6, DcgRieIva6, DcgRieImp7, DcgRieCi7, DcgRieIva7, DcgRieTotImp, DcgRieTotEse, " _
    & "DcgRieTotIva, DcgRieTotFat, DcgRieTotOma, DcgRieTotAcc, DcgRieTotale, DcgRieScCond, DcgRieNetto, " _
    & "DcgCodPag, DcgTipo,DcgNumRif) " & " Values (@DcgTrasf,@DcgData,@DcgRegistro,@DcgNumero,@DcgCli,@DcgCodAge,@DcgImp1,@DcgCodIva1,@DcgIva1," _
    & "@DcgCptCon1,@DcgImp2,@DcgCodIva2,@DcgIva2,@DcgCptCon2,@DcgImp3, @DcgCodIva3, @DcgIva3, @DcgCptCon3," _
    & "@DcgImp4, @DcgCodIva4, @DcgIva4, @DcgCptCon4, @DcgImp5, @DcgCodIva5, @DcgIva5, @DcgCptCon5, @DcgImp6," _
    & "@DcgCodIva6,@DcgIva6,@DcgCptCon6,@DcgImp7,@DcgCodIva7,@DcgIva7,@DcgCptCon7,@DcgImp8,@DcgCodIva8,@DcgIva8," _
    & "@DcgCptCon8,@DcgImp9,@DcgCodIva9,@DcgIva9,@DcgCptCon9,@DcgImp10,@DcgCodIva10,@DcgIva10,@DcgCptCon10," _
    & "@DcgImp11, @DcgCodIva11, @DcgIva11,@DcgCptCon11,@DcgAcconti,@DcgNBol,@DcgTotMerce,@DcgSpeBolli,@DcgSpeBolliCi,@DcgSpeRb," _
    & "@DcgSpeRbCi,@DcgRieImp1,@DcgRieCi1,@DcgRieIva1,@DcgRieImp2,@DcgRieCi2,@DcgRieIva2,@DcgRieImp3," _
    & "@DcgRieCi3,@DcgRieIva3,@DcgRieImp4,@DcgRieCi4,@DcgRieIva4,@DcgRieImp5,@DcgRieCi5,@DcgRieIva5," _
    & "@DcgRieImp6,@DcgRieCi6,@DcgRieIva6,@DcgRieImp7,@DcgRieCi7,@DcgRieIva7,@DcgRieTotImp,@DcgRieTotEse," _
    & "@DcgRieTotIva,@DcgRieTotFat,@DcgRieTotOma,@DcgRieTotAcc,@DcgRieTotale,@DcgRieScCond,@DcgRieNetto," _
    & "@DcgCodPag,@DcgTipo,@DcgNumRif)"




    Dim cmd As New SqlCommand(scrive, cnDb)
    Dim p1 As New SqlParameter("@DcgTrasf", SqlDbType.Bit)
    Dim p2 As New SqlParameter("@DcgData", SqlDbType.SmallDateTime)
    Dim p3 As New SqlParameter("@DcgRegistro", SqlDbType.SmallInt)
    Dim p4 As New SqlParameter("@DcgNumero", SqlDbType.Int)
    Dim p5 As New SqlParameter("@DcgCli", SqlDbType.VarChar)
    Dim p6 As New SqlParameter("@DcgCodAge", SqlDbType.SmallInt)
    Dim p7 As New SqlParameter("@DcgImp1", SqlDbType.Decimal)
    Dim p8 As New SqlParameter("@DcgCodiva1", SqlDbType.SmallInt)
    Dim p9 As New SqlParameter("@DcgIva1", SqlDbType.Decimal)
    Dim p10 As New SqlParameter("@DcgCptCon1", SqlDbType.VarChar)


    Dim p11 As New SqlParameter("@DcgImp2", SqlDbType.Decimal)
    Dim p12 As New SqlParameter("@DcgCodiva2", SqlDbType.SmallInt)
    Dim p13 As New SqlParameter("@DcgIva2", SqlDbType.Decimal)
    Dim p14 As New SqlParameter("@DcgCptCon2", SqlDbType.VarChar)


    Dim p15 As New SqlParameter("@DcgImp3", SqlDbType.Decimal)
    Dim p16 As New SqlParameter("@DcgCodiva3", SqlDbType.SmallInt)
    Dim p17 As New SqlParameter("@DcgIva3", SqlDbType.Decimal)
    Dim p18 As New SqlParameter("@DcgCptCon3", SqlDbType.VarChar)



    Dim p19 As New SqlParameter("@DcgImp4", SqlDbType.Decimal)
    Dim p20 As New SqlParameter("@DcgCodiva4", SqlDbType.SmallInt)
    Dim p21 As New SqlParameter("@DcgIva4", SqlDbType.Decimal)
    Dim p22 As New SqlParameter("@DcgCptCon4", SqlDbType.VarChar)

    Dim p23 As New SqlParameter("@DcgImp5", SqlDbType.Decimal)
    Dim p24 As New SqlParameter("@DcgCodiva5", SqlDbType.SmallInt)
    Dim p25 As New SqlParameter("@DcgIva5", SqlDbType.Decimal)
    Dim p26 As New SqlParameter("@DcgCptCon5", SqlDbType.VarChar)

    Dim p27 As New SqlParameter("@DcgImp6", SqlDbType.Decimal)
    Dim p28 As New SqlParameter("@DcgCodiva6", SqlDbType.SmallInt)
    Dim p29 As New SqlParameter("@DcgIva6", SqlDbType.Decimal)
    Dim p30 As New SqlParameter("@DcgCptCon6", SqlDbType.VarChar)

    Dim p31 As New SqlParameter("@DcgImp7", SqlDbType.Decimal)
    Dim p32 As New SqlParameter("@DcgCodiva7", SqlDbType.SmallInt)
    Dim p33 As New SqlParameter("@DcgIva7", SqlDbType.Decimal)
    Dim p34 As New SqlParameter("@DcgCptCon7", SqlDbType.VarChar)

    Dim p35 As New SqlParameter("@DcgImp8", SqlDbType.Decimal)
    Dim p36 As New SqlParameter("@DcgCodiva8", SqlDbType.SmallInt)
    Dim p37 As New SqlParameter("@DcgIva8", SqlDbType.Decimal)
    Dim p38 As New SqlParameter("@DcgCptCon8", SqlDbType.VarChar)

    Dim p39 As New SqlParameter("@DcgImp9", SqlDbType.Decimal)
    Dim p40 As New SqlParameter("@DcgCodiva9", SqlDbType.SmallInt)
    Dim p41 As New SqlParameter("@DcgIva9", SqlDbType.Decimal)
    Dim p42 As New SqlParameter("@DcgCptCon9", SqlDbType.VarChar)

    Dim p43 As New SqlParameter("@DcgImp10", SqlDbType.Decimal)
    Dim p44 As New SqlParameter("@DcgCodiva10", SqlDbType.SmallInt)
    Dim p45 As New SqlParameter("@DcgIva10", SqlDbType.Decimal)
    Dim p46 As New SqlParameter("@DcgCptCon10", SqlDbType.VarChar)


    Dim p47 As New SqlParameter("@DcgImp11", SqlDbType.Decimal)
    Dim p48 As New SqlParameter("@DcgCodiva11", SqlDbType.SmallInt)
    Dim p49 As New SqlParameter("@DcgIva11", SqlDbType.Decimal)
    Dim p50 As New SqlParameter("@DcgCptCon11", SqlDbType.VarChar)

    Dim p51 As New SqlParameter("@DcgAcconti", SqlDbType.Decimal)
    Dim p52 As New SqlParameter("@DcgNBol", SqlDbType.Int)
    Dim p53 As New SqlParameter("@DcgTotMerce", SqlDbType.Decimal)
    Dim p54 As New SqlParameter("@DcgSpeBolli", SqlDbType.Decimal)

    Dim p55 As New SqlParameter("@DcgSpeBolliCi", SqlDbType.SmallInt)
    Dim p56 As New SqlParameter("@DcgSpeRb", SqlDbType.Decimal)
    Dim p57 As New SqlParameter("@DcgSpeRbCi", SqlDbType.SmallInt)

    Dim p58 As New SqlParameter("@DcgRieImp1", SqlDbType.Decimal)
    Dim p59 As New SqlParameter("@DcgRieCi1", SqlDbType.SmallInt)
    Dim p60 As New SqlParameter("@DcgRieIva1", SqlDbType.Decimal)

    Dim p61 As New SqlParameter("@DcgRieImp2", SqlDbType.Decimal)
    Dim p62 As New SqlParameter("@DcgRieCi2", SqlDbType.SmallInt)
    Dim p63 As New SqlParameter("@DcgRieIva2", SqlDbType.Decimal)
    Dim p64 As New SqlParameter("@DcgRieImp3", SqlDbType.Decimal)
    Dim p65 As New SqlParameter("@DcgRieCi3", SqlDbType.SmallInt)
    Dim p66 As New SqlParameter("@DcgRieIva3", SqlDbType.Decimal)
    Dim p67 As New SqlParameter("@DcgRieImp4", SqlDbType.Decimal)
    Dim p68 As New SqlParameter("@DcgRieCi4", SqlDbType.SmallInt)
    Dim p69 As New SqlParameter("@DcgRieIva4", SqlDbType.Decimal)
    Dim p70 As New SqlParameter("@DcgRieImp5", SqlDbType.Decimal)
    Dim p71 As New SqlParameter("@DcgRieCi5", SqlDbType.SmallInt)
    Dim p72 As New SqlParameter("@DcgRieIva5", SqlDbType.Decimal)
    Dim p73 As New SqlParameter("@DcgRieImp6", SqlDbType.Decimal)
    Dim p74 As New SqlParameter("@DcgRieCi6", SqlDbType.SmallInt)
    Dim p75 As New SqlParameter("@DcgRieIva6", SqlDbType.Decimal)
    Dim p76 As New SqlParameter("@DcgRieImp7", SqlDbType.Decimal)
    Dim p77 As New SqlParameter("@DcgRieCi7", SqlDbType.SmallInt)
    Dim p78 As New SqlParameter("@DcgRieIva7", SqlDbType.Decimal)

    Dim p79 As New SqlParameter("@DcgRieTotImp", SqlDbType.Decimal)
    Dim p80 As New SqlParameter("@DcgRieTotEse", SqlDbType.Decimal)
    Dim p81 As New SqlParameter("@DcgRieTotIva", SqlDbType.Decimal)
    Dim p82 As New SqlParameter("@DcgRieTotFat", SqlDbType.Decimal)
    Dim p83 As New SqlParameter("@DcgRieTotOma", SqlDbType.Decimal)
    Dim p84 As New SqlParameter("@DcgRieTotAcc", SqlDbType.Decimal)

    Dim p85 As New SqlParameter("@DcgRieTotale", SqlDbType.Decimal)
    Dim p86 As New SqlParameter("@DcgRieScCond", SqlDbType.Decimal)
    Dim p87 As New SqlParameter("@DcgRieNetto", SqlDbType.Decimal)
    Dim p88 As New SqlParameter("@DcgCodPag", SqlDbType.SmallInt)
    Dim p89 As New SqlParameter("@DcgTipo", SqlDbType.VarChar)
    Dim p90 As New SqlParameter("@DcgNumrif", SqlDbType.Int)
    Dim uguale As Int32 = 0

    Public Function ChiudeFattura(ByVal RIFERFAT As Int32)
        LeggiCodiciIva()
        LeggiTai()
        If RIFERFAT = Qui Then
            Dim xx, ww As Int16
            DaRif = New SqlDataAdapter("SELECT * FROM TmpPre order by TmpRif", cnDb)
            DsRif = New DataSet
            DaRif.Fill(DsRif)
            xx = DsRif.Tables(0).Rows.Count
            For ww = 1 To xx
                RwRif = DsRif.Tables(0).Rows(ww - 1)
                RIFERFAT = Val(RwRif("TmpRif"))
                If RIFERFAT <> uguale Then
                    uguale = RIFERFAT
                    Inizia(RIFERFAT)
                End If
            Next ww
        Else
            Inizia(RIFERFAT)
        End If
    End Function

    Function Inizia(ByVal RiferFat As Int32)
        Dim j As Integer = 0
        Dim ar, DataF As String
        swR = 0
        ar = "VIEW1"
        DaTes = New SqlDataAdapter("SELECT * FROM VIEW1 where FatRif = " & RiferFat, cnDb)
        DaTes.SelectCommand.CommandTimeout = 180 ''
        DsTes = New DataSet(ar)
        DaTes.Fill(DsTes, ar)
        TesTest = DsTes.Tables(ar).Rows.Count
        If TesTest = 0 Then
            Dim cmline As String
            cmline = "Delete TbDcg where DcgNumRif = " & RiferFat
            Dim dcmd As New SqlCommand(cmline, cnDb)
            dcmd.ExecuteNonQuery()
            Exit Function
        End If

leggirighe:
        j = j + 1
        If j > TesTest Then
            ChiudeFattura()
            Exit Function
        End If
        RwTes = DsTes.Tables(ar).Rows(j - 1)
        If swR = 0 Then
            CambiaFattura()
        ElseIf swR = Val(RwTes("FatRif")) Then
            SommaIva()
        Else
            ChiudeFattura()
            CambiaFattura()
        End If
        GoTo leggirighe
    End Function
    Function CambiaFattura()
        AzzeraImpo()
        swR = Val(RwTes("FatRif"))
        swf = RwTes("FatNum")
        swd = RwTes("FatData")
        p1.Value = False
        p2.Value = swd
        p3.Value = Val(RwTes("FatNumReg"))
        p4.Value = Val(swf)
        p5.Value = RwTes("FatCliFat")
        NonImpEse = Val(RwTes("FatNimp"))
        p89.Value = RwTes("FatTipoDoc")
        p88.Value = Val(RwTes("FatPagCod"))
        p90.Value = Val(RwTes("FatRif"))
        'If RwTes("FatCau") = 12 Then
        '    Segno = -1
        'Else
        '    Segno = +1
        'End If
        Segno = +1
        SommaIva()
    End Function
    Sub AzzeraImpo()
        Dim x As Int16
        For x = 0 To 72
            Imp(x) = 0
            Iva(x) = 0
        Next
        For x = 0 To 11
            CptCpt(x) = ""
            TotCpt(x) = 0
            CiiCpt(x) = 0
            IvaCpt(x) = 0
            SorCpt(x) = ""
        Next
        Pnt = 0
        TotEse = 0
        ImpSpese = 0
        TotMerce = 0
        TotImporto = 0
        p53.Value = 0
        p83.Value = 0
        p84.Value = 0
        p86.Value = 0
        'DA VEDERE BOLLI + SPESE RB
        p52.Value = 0
        p54.Value = 0
        p55.Value = 0
        p56.Value = 0
        p57.Value = 0
        ' DA VEDERE TRASF COGE
        p6.Value = 0
        p7.Value = 0
        p8.Value = 0
        p9.Value = 0
        p11.Value = 0
        p12.Value = 0
        p13.Value = 0
        p15.Value = 0
        p16.Value = 0
        p17.Value = 0
        p19.Value = 0
        p20.Value = 0
        p21.Value = 0
        p23.Value = 0
        p24.Value = 0
        p25.Value = 0
        p27.Value = 0
        p28.Value = 0
        p29.Value = 0
        p31.Value = 0
        p32.Value = 0
        p33.Value = 0
        p35.Value = 0
        p36.Value = 0
        p37.Value = 0
        p39.Value = 0
        p40.Value = 0
        p41.Value = 0
        p43.Value = 0
        p44.Value = 0
        p45.Value = 0
        p47.Value = 0
        p48.Value = 0
        p49.Value = 0


        p10.Value = "00.00"
        p14.Value = "00.00"
        p18.Value = "00.00"
        p22.Value = "00.00"
        p26.Value = "00.00"
        p30.Value = "00.00"
        p34.Value = "00.00"
        p38.Value = "00.00"
        p42.Value = "00.00"
        p46.Value = "00.00"
        p50.Value = "00.00"

        p51.Value = 0
    End Sub
    Sub SommaIva()
        'CodCodIva = Tci(Val(RwTes("CorCiva")))
        CodCodIva = RwTes("CorCiva")
        If NonImpEse > 0 Then CodCodIva = NonImpEse

        Dim Impmerce As Decimal

        Impmerce = Format(CDec(RwTes("CorPrezzo")) * RwTes("CorQuaCon"))

        TotMerce = TotMerce + Format(Impmerce, "###,###,##0.00") * Segno
        TotImporto = TotImporto + Format(RwTes("CorImporto"), "###,###,##0.00") * Segno

        Imp(CodCodIva) = Imp(CodCodIva) + Format(RwTes("CorImporto"), "###,###,##0.00") * Segno
        If CDec(RwTes("CorImporto")) <> 0 Then
            SommaCpt(RwTes("CorCntrp"), Format(RwTes("CorImporto"), "###,###,##0.00") * Segno, CodCodIva)
        End If
    End Sub
    Sub SommaCpt(ByVal CnTrp As String, ByVal Importo As Decimal, ByVal CCodiva As Int16)
        Dim x As Int16
        Dim impo As Decimal
        If CnTrp = "" And CCodiva = 0 Then Exit Sub
        impo = Importo
        x = 0
torna:
        x = x + 1
        If x > 11 Then Exit Sub
        If RwTes("FatTipoDoc") <> "R" Then Importo = (impo / (100 + CiPerc(CCodiva))) * 100
        If x > Pnt Then
            Pnt = x
            CptCpt(x) = CnTrp
            CiiCpt(x) = CCodiva
            TotCpt(x) = Format(Importo, "###,###,##0.00")
            SorCpt(x) = CiiCpt(x).ToString.PadLeft(2, "0") & "." & x.ToString.PadLeft(2, "0") & "." & CptCpt(x)
            Exit Sub
        End If
        If CptCpt(x) <> CnTrp Or CiiCpt(x) <> CCodiva Then
            GoTo torna
        Else
            TotCpt(x) = TotCpt(x) + Format(Importo, "###,###,##0.00")
        End If
    End Sub

    Sub ChiudeFattura()
        Dim x, y, Rcii(7), z As Int16
        Dim Rimp(7), Riva(7), imponibile As Decimal

        CalSpeseRb()

        TotEse = 0
        y = 0
        For x = 1 To 72
            imponibile = 0
            If Imp(x) <> 0 And RwTes("FatTipoDoc") <> "R" Then
                imponibile = Imp(x)
                Imp(x) = (imponibile / (100 + CiPerc(x))) * 100
                Iva(x) = imponibile - Imp(x)
                'Iva(x) = Format(Imp(x) / (100 + CiPerc(x)), "###,##0.00")
                If CiPerc(x) = 0 Then
                    TotEse = TotEse + Imp(x)
                Else
                    Imp(0) = Imp(0) + Imp(x)
                    Iva(0) = Iva(0) + Iva(x)
                End If
            ElseIf Imp(x) <> 0 And RwTes("FatTipoDoc") = "R" Then
                Iva(x) = 0
                If CiPerc(x) = 0 Then
                    TotEse = TotEse + Imp(x)
                Else
                    Imp(0) = Imp(0) + Imp(x)
                    Iva(0) = Iva(0) + Iva(x)
                End If
            End If
        Next
        For x = 1 To 72
            If Imp(x) <> 0 Then
                y = y + 1
                Rimp(y) = Imp(x)
                Riva(y) = Iva(x)
                Rcii(y) = x
            End If
        Next
        ' CARICA VETTORE IMPONIBILE IVA E CPT PER COGE
        Array.Sort(SorCpt)
        Dim p, k As Int16
        k = 0
        For x = 0 To 11
            CptCpt(x) = ""
            CiiCpt(x) = 0
            IvaCpt(x) = 0
            ImpCpt(x) = 0
        Next
        For x = 11 To 1 Step -1
            If SorCpt(x) <> "" Then
                p = Val(Mid(SorCpt(x), 4, 2))
                CiiCpt(x) = Val(Mid(SorCpt(x), 1, 2))
                CptCpt(x) = Mid(SorCpt(x), 7, 5)
                ImpCpt(x) = TotCpt(p)
                If k <> CiiCpt(x) Then
                    k = CiiCpt(x)
                    IvaCpt(x) = Iva(k)
                End If
            End If
        Next
        For x = 0 To 10
            If IvaCpt(x) = 0 And CiiCpt(x) = CiiCpt(x + 1) Then CiiCpt(x) = 0
            If CptCpt(x) = "" Then p = x
        Next
        If p = 0 Then GoTo Oltre
        For x = 1 To Pnt
            p = p + 1
            CptCpt(x) = CptCpt(p)
            CiiCpt(x) = CiiCpt(p)
            IvaCpt(x) = IvaCpt(p)
            ImpCpt(x) = ImpCpt(p)
        Next
        '''per le ricevute fiscali registro dei corrispettivi 5
        If p89.Value = "R" Then
            Dim ucodiva As Int16 = 0
            For x = Pnt To 1 Step -1
                If CiiCpt(x) > 0 Then ucodiva = CiiCpt(x)
                If CiiCpt(x) = 0 Then CiiCpt(x) = ucodiva
                If ImpCpt(x) <> 0 Then
                    IvaCpt(x) = 0
                    'IvaCpt(x) = Format(ImpCpt(x) * CiPerc(CiiCpt(x)) / 100, "###,##0.00")
                End If
            Next
        End If
        '''per le ricevute fiscali registro dei corrispettivi 5
        p = Pnt + 1
        For x = p To 11
            CptCpt(x) = ""
            CiiCpt(x) = 0
            IvaCpt(x) = 0
            ImpCpt(x) = 0
        Next
Oltre:

        If ImpSpese > 0 Then
            p56.Value = ImpSpese
            p57.Value = CiSpe
        End If
        p7.Value = ImpCpt(1)
        p8.Value = CiiCpt(1)
        p9.Value = IvaCpt(1)
        p10.Value = CptCpt(1)

        p11.Value = ImpCpt(2)
        p12.Value = CiiCpt(2)
        p13.Value = IvaCpt(2)
        p14.Value = CptCpt(2)

        p15.Value = ImpCpt(3)
        p16.Value = CiiCpt(3)
        p17.Value = IvaCpt(3)
        p18.Value = CptCpt(3)

        p19.Value = ImpCpt(4)
        p20.Value = CiiCpt(4)
        p21.Value = IvaCpt(4)
        p22.Value = CptCpt(4)


        p23.Value = ImpCpt(5)
        p24.Value = CiiCpt(5)
        p25.Value = IvaCpt(5)
        p26.Value = CptCpt(5)


        p27.Value = ImpCpt(6)
        p28.Value = CiiCpt(6)
        p29.Value = IvaCpt(6)
        p30.Value = CptCpt(6)


        p31.Value = ImpCpt(7)
        p32.Value = CiiCpt(7)
        p33.Value = IvaCpt(7)
        p34.Value = CptCpt(7)


        p35.Value = ImpCpt(8)
        p36.Value = CiiCpt(8)
        p37.Value = IvaCpt(8)
        p38.Value = CptCpt(8)


        p39.Value = ImpCpt(9)
        p40.Value = CiiCpt(9)
        p41.Value = IvaCpt(9)
        p42.Value = CptCpt(9)


        p43.Value = ImpCpt(10)
        p44.Value = CiiCpt(10)
        p45.Value = IvaCpt(10)
        p46.Value = CptCpt(10)


        p47.Value = ImpCpt(11)
        p48.Value = CiiCpt(11)
        p49.Value = IvaCpt(11)
        p50.Value = CptCpt(11)




        p58.Value = Rimp(1)
        p59.Value = Rcii(1)
        p60.Value = Riva(1)
        p61.Value = Rimp(2)
        p62.Value = Rcii(2)
        p63.Value = Riva(2)
        p64.Value = Rimp(3)
        p65.Value = Rcii(3)
        p66.Value = Riva(3)
        p67.Value = Rimp(4)
        p68.Value = Rcii(4)
        p69.Value = Riva(4)
        p70.Value = Rimp(5)
        p71.Value = Rcii(5)
        p72.Value = Riva(5)
        p73.Value = Rimp(6)
        p74.Value = Rcii(6)
        p75.Value = Riva(6)
        p76.Value = Rimp(7)
        p77.Value = Rcii(7)
        p78.Value = Riva(7)

        p79.Value = Imp(0)
        p80.Value = TotEse
        p81.Value = Iva(0)

        p82.Value = Imp(0) + TotEse + Iva(0)
        p85.Value = p82.Value - p84.Value
        p87.Value = p85.Value

        'Controllo Ulteriore sconto

        p53.Value = TotMerce
        ScriveDcg()
    End Sub
    Function ScriveDcg()
        Dim cmline As String
        cmline = "Delete TbDcg where DcgNumRif = " & p90.Value
        Dim dcmd As New SqlCommand(cmline, cnDb)
        dcmd.ExecuteNonQuery()

        cmd.Parameters.Clear()
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmd.Parameters.Add(p3)
        cmd.Parameters.Add(p4)
        cmd.Parameters.Add(p5)
        cmd.Parameters.Add(p6)
        cmd.Parameters.Add(p7)
        cmd.Parameters.Add(p8)
        cmd.Parameters.Add(p9)
        cmd.Parameters.Add(p10)
        cmd.Parameters.Add(p11)
        cmd.Parameters.Add(p12)
        cmd.Parameters.Add(p13)
        cmd.Parameters.Add(p14)
        cmd.Parameters.Add(p15)
        cmd.Parameters.Add(p16)
        cmd.Parameters.Add(p17)
        cmd.Parameters.Add(p18)
        cmd.Parameters.Add(p19)
        cmd.Parameters.Add(p20)
        cmd.Parameters.Add(p21)
        cmd.Parameters.Add(p22)
        cmd.Parameters.Add(p23)
        cmd.Parameters.Add(p24)
        cmd.Parameters.Add(p25)
        cmd.Parameters.Add(p26)
        cmd.Parameters.Add(p27)
        cmd.Parameters.Add(p28)
        cmd.Parameters.Add(p29)
        cmd.Parameters.Add(p30)
        cmd.Parameters.Add(p31)

        cmd.Parameters.Add(p32)
        cmd.Parameters.Add(p33)
        cmd.Parameters.Add(p34)
        cmd.Parameters.Add(p35)
        cmd.Parameters.Add(p36)
        cmd.Parameters.Add(p37)
        cmd.Parameters.Add(p38)
        cmd.Parameters.Add(p39)
        cmd.Parameters.Add(p40)
        cmd.Parameters.Add(p41)
        cmd.Parameters.Add(p42)
        cmd.Parameters.Add(p43)
        cmd.Parameters.Add(p44)
        cmd.Parameters.Add(p45)
        cmd.Parameters.Add(p46)
        cmd.Parameters.Add(p47)
        cmd.Parameters.Add(p48)
        cmd.Parameters.Add(p49)
        cmd.Parameters.Add(p50)
        cmd.Parameters.Add(p51)
        cmd.Parameters.Add(p52)
        cmd.Parameters.Add(p53)
        cmd.Parameters.Add(p54)
        cmd.Parameters.Add(p55)
        cmd.Parameters.Add(p56)
        cmd.Parameters.Add(p57)
        cmd.Parameters.Add(p58)
        cmd.Parameters.Add(p59)
        cmd.Parameters.Add(p60)

        cmd.Parameters.Add(p61)
        cmd.Parameters.Add(p62)
        cmd.Parameters.Add(p63)
        cmd.Parameters.Add(p64)
        cmd.Parameters.Add(p65)
        cmd.Parameters.Add(p66)
        cmd.Parameters.Add(p67)
        cmd.Parameters.Add(p68)
        cmd.Parameters.Add(p69)
        cmd.Parameters.Add(p70)

        cmd.Parameters.Add(p71)
        cmd.Parameters.Add(p72)
        cmd.Parameters.Add(p73)
        cmd.Parameters.Add(p74)
        cmd.Parameters.Add(p75)
        cmd.Parameters.Add(p76)
        cmd.Parameters.Add(p77)
        cmd.Parameters.Add(p78)
        cmd.Parameters.Add(p79)
        cmd.Parameters.Add(p80)
        cmd.Parameters.Add(p81)
        cmd.Parameters.Add(p82)
        cmd.Parameters.Add(p83)
        cmd.Parameters.Add(p84)
        cmd.Parameters.Add(p85)
        cmd.Parameters.Add(p86)
        cmd.Parameters.Add(p87)
        cmd.Parameters.Add(p88)
        cmd.Parameters.Add(p89)
        cmd.Parameters.Add(p90)
        cmd.ExecuteNonQuery()
        ''solo per il vecchio
        'If p3.Value = 4 Then
        '    NotaCredito()
        'End If
    End Function
    Sub NotaCredito()
        Dim CmdNc As String
        CmdNc = "Update TbFat set FatNumReg = " & p3.Value & " where FatRif = " & p90.Value
        Dim Acmd As New SqlCommand(CmdNc, cnDb)
        Acmd.ExecuteNonQuery()
    End Sub
    Function LeggiCodiciIva()
        Dim cmd As New SqlCommand("SELECT * FROM TbCii order by CiiCod ", cnCo)
        dataRd = cmd.ExecuteReader
        Dim x As Int16 = 0
        While dataRd.Read
            x = x + 1
            CiPerc(x) = dataRd.GetInt16(1)
        End While
        dataRd.Close()
    End Function

    Function LeggiPagamenti(ByVal CodPag) As String
        LeggiPagamenti = ""
        Dim cmd As New SqlCommand("SELECT * FROM TbPag Where PagCod = " & CodPag, cnCo)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            LeggiPagamenti = dataRd.GetString(1)
            Nrate = dataRd.GetInt16(3)
            TipoPag = dataRd.GetInt16(2)
        End While
        dataRd.Close()
        Return Nrate
        Return TipoPag
    End Function
    Function LeggiTai() As String
        Dim x As Int16
        LeggiTai = ""
        DaTai = New SqlDataAdapter("SELECT top 1 * FROM TbTai order by TaiAnno desc", cnDb)
        DsTai = New DataSet(TaiD)
        DaTai.Fill(DsTai, TaiD)
        RwTai = DsTai.Tables(TaiD).Rows(0)
        Spese = RwTai("TaiSpeseRb")
        CiSpe = RwTai("TaiPi2")
        CptSpe = RwTai("TaiCpt2")
    End Function

    Private Sub CalSpeseRb()
        LeggiPagamenti(p88.Value)
        ImpSpese = 0
        '''' PER ORA NON CALCOLO SPESE RB ''''  HO AGGIUNTO NELLA IF Val(RwTes("FatEseSpe")) = 0 

        If (TipoPag <> 2 And TipoPag <> 7) Or Val(RwTes("FatEseSpe")) = 1 Or Val(RwTes("FatEseSpe")) = 0 Then
            Exit Sub
        End If

        ImpSpese = Spese * Nrate
        CodCodIva = CiSpe
        Imp(CodCodIva) = Imp(CodCodIva) + ImpSpese
        SommaCpt(CptSpe, ImpSpese, CodCodIva)
    End Sub

    Function AggiornaEffetti(ByVal FatRif As Int32)
        'If RISTAMPA = True Then Exit Function
        StrSelect = "Select * from TbDcg where DcgNumRif=" & FatRif
        Dim DsDcg As DataSet
        Dim RiW As DataRow
        Dim DaDcg = New SqlDataAdapter(StrSelect, cnDb)
        DsDcg = New DataSet("DCG")
        DaDcg.Fill(DsDcg, "DCG")
        If DsDcg.Tables("DCG").Rows.Count = 0 Then Exit Function
        RiW = DsDcg.Tables("DCG").Rows(0)

        StrSelect = "Select * from TbFat where FatRif=" & FatRif
        Dim DsFat As DataSet
        Dim RwFat As DataRow
        Dim DaFat = New SqlDataAdapter(StrSelect, cnDb)
        DsFat = New DataSet("FAT")
        DaFat.Fill(DsFat, "FAT")
        If DsFat.Tables("FAT").Rows.Count = 0 Then Exit Function
        RwFat = DsFat.Tables("FAT").Rows(0)

        StrSelect = "SELECT * FROM TbPag Where PagCod = " & RwFat("FatPagCod")
        Dim DsPag As DataSet
        Dim RwPag As DataRow
        Dim DaPag = New SqlDataAdapter(StrSelect, cnCo)
        DsPag = New DataSet("PAG")
        DaPag.Fill(DsPag, "PAG")
        If DsPag.Tables("PAG").Rows.Count = 0 Then Exit Function
        RwPag = DsPag.Tables("PAG").Rows(0)

        If Not RwPag("PagTipo") = 2 And Not RwPag("PagTipo") = 7 Then Exit Function

        Dim VetDt(24) As Date
        Dim VetImp(24) As Decimal
        Dim kk As Int16
        EsplImporti(RwPag, RiW("DcgRieNetto"), RiW("DcgSpeRb") + RiW("DcgRieTotiva"), RiW("DcgData"), VetDt, VetImp)
        Dim scrive As String
        scrive = "Insert Into TbEff (RicClie,RicImpFatt,RicNfat,RicAbi,RicCab,RicImpRata,RicTpag,RicSeff,RicNRata,RicBan,RicFlagCoge,RicDFat,RicDsca,RicDDis,RicSomma,RicRifFat) " _
        & " Values (@RicClie,@RicImpFatt,@RicNfat,@RicAbi,@RicCab,@RicImpRata,@RicTpag,@RicSeff,@RicNRata,@RicBan,@RicFlagCoge,@RicDfat,@RicDsca,@RicDDis,@RicSomma,@RicRifFat)"
        Dim cmd As New SqlCommand(scrive, cnCo)
        Dim c1 As New SqlParameter("@RicClie", SqlDbType.VarChar)
        Dim c2 As New SqlParameter("@RicImpFatt", SqlDbType.Decimal)
        Dim c3 As New SqlParameter("@RicNfat", SqlDbType.Int)
        Dim c4 As New SqlParameter("@RicAbi", SqlDbType.Int)
        Dim c5 As New SqlParameter("@RicCab", SqlDbType.Int)
        Dim c6 As New SqlParameter("@RicImpRata", SqlDbType.Decimal)
        Dim c7 As New SqlParameter("@RicTPag", SqlDbType.SmallInt)
        Dim c8 As New SqlParameter("@RicSeff", SqlDbType.SmallInt)
        Dim c9 As New SqlParameter("@RicNRata", SqlDbType.SmallInt)
        Dim c10 As New SqlParameter("@RicBan", SqlDbType.SmallInt)
        Dim c11 As New SqlParameter("@RicFlagCoge", SqlDbType.Bit)
        Dim c12 As New SqlParameter("@RicDFat", SqlDbType.SmallDateTime)
        Dim c13 As New SqlParameter("@RicDsca", SqlDbType.SmallDateTime)
        Dim c14 As New SqlParameter("@RicDDis", SqlDbType.SmallDateTime)
        Dim c15 As New SqlParameter("@RicSomma", SqlDbType.VarChar)
        Dim c16 As New SqlParameter("@RicRifFat", SqlDbType.Int)
        c1.Value = RiW("DcgCli")
        c2.Value = RiW("DcgRieNetto")
        c3.Value = RiW("DcgNumero")
        c4.Value = RwFat("FatAbi")
        c5.Value = RwFat("FatCab")
        c6.Value = "" ' importo rata
        c7.Value = RwPag("PagTipo")
        If c7.Value = 3 Then
            c8.Value = 1 ' Da Emettere
        Else
            c8.Value = 2 ' In Portafoglio
        End If
        c10.Value = 1 ' BANCA 1 DI DEFAULT
        c11.Value = 0 ' non trasferito in coge
        c12.Value = RiW("DcgData")
        c14.Value = RiW("DcgData") ' data distinta = data fattura
        c15.Value = "E" ' EFFETTO
        c16.Value = RiW("DcgNumrif")
        cmd.Parameters.Add(c1)
        cmd.Parameters.Add(c2)
        cmd.Parameters.Add(c3)
        cmd.Parameters.Add(c4)
        cmd.Parameters.Add(c5)
        cmd.Parameters.Add(c6)
        cmd.Parameters.Add(c7)
        cmd.Parameters.Add(c8)
        cmd.Parameters.Add(c9)
        cmd.Parameters.Add(c10)
        cmd.Parameters.Add(c11)
        cmd.Parameters.Add(c12)
        cmd.Parameters.Add(c13)
        cmd.Parameters.Add(c14)
        cmd.Parameters.Add(c15)
        cmd.Parameters.Add(c16)
        For kk = 1 To Val(RwPag("PagNRate"))
            c6.Value = VetImp(kk) ' importo rata
            c9.Value = kk ' N.RATA
            c13.Value = VetDt(kk) ' DATA SCADENZA
            cmd.ExecuteNonQuery()
        Next
    End Function

End Module

