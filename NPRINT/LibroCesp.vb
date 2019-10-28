Imports DXBASE
Imports NCCOM
Imports System.IO
Imports System.Data.SqlClient
Imports System.Drawing.Printing

Public Module LibroCesp
    Dim PathPrg, Dest, FtFm, Ftmp, Concat, Stampa, QualeLp, NomeFile, QualeTipo As String
    Dim PathSto, FtSt, Tdoc As String
    Dim PathTmp As String
    Dim Rif, TesTest As Int32
    Dim Pagine, IdProc As Int16
    Dim Comprimi As Boolean
    Dim IT(6) As String
    Dim TbCat, TbCesp, TbQuo, TbVar As DataTable
    Dim DaCat, DaCesp, DaQuo, DaVar As SqlDataAdapter
    Dim RwCesp, RwCat, RwQuo, RwVar As DataRow
    Dim MaxRighe As Int16 = 55
    Dim NumRighe As Int16
    Dim TotCol As Int16 = 132
    Dim Tot1, Tot2 As Decimal
    Dim AnnoRif As Int16
    Dim AnniPrec, FineCat, SuBollato As Boolean
    Dim TotQuota, TotFondo, TotResiduo, TotND, TotCVal, TotCCos As Decimal
    Dim TotGQuota, TotGFondo, TotGResiduo, TotGND As Decimal
    Dim TotVal, TotCos As Decimal
    Dim Inizio, Fine, AnnoLp, barra As String

    Public Sub StampaLibro(ByVal QSTAMPANTE As String, ByVal COMPRESSA As Boolean, ByVal LPTIPO As String, ByVal LPRESET As String, ByVal Anno As Int16, ByVal DaCateg As Int16, ByVal ACateg As Int16, ByVal AnniPrecedenti As Boolean, ByVal Tutti As Boolean, ByVal SuBol As Boolean, ByVal Intesta As String, ByVal n1 As Int16, ByVal n2 As Int32, ByVal AZI As String)
        TotVal = 0
        TotCos = 0
        TotGQuota = 0
        TotGFondo = 0
        TotGResiduo = 0
        TotGND = 0
        If n1 = 0 Then
            AnnoLp = ""
            barra = ""
        Else
            AnnoLp = n1
            barra = "/"
        End If

        If SuBol = True Then LeggiAzienda(AZI)
        IT(6) = Intesta
        Inizio = TimeString
        'If SuBol = True Then SaltoPagina = True
        Dim PrintUno As String = "Select distinct CespCat,CspDesc,CspGru,CspSpe1,CspSpe2,CespCp,CspCp  from VLibroCesp where cast(CespCat as smallint)>=" & DaCateg & " and cast(CespCat as smallint)<=" & ACateg & _
        IIf(AnniPrecedenti = True, " and QuoAnno <= " & Anno, " and QuoAnno = " & Anno) & _
        IIf(Tutti = False, " and ceduto = 'false' and (quoquota>0 or QuoResiduo>0 or VCespVariazioni>0)", "")

        TbCat = New DataTable
        DaCat = New SqlDataAdapter(PrintUno, cnCo)
        DaCat.Fill(TbCat)

        Dim PrintDue As String = "Select distinct CespNum,CespAnnoA,CespCat,CespContoStorico,CespAliFis,CespDescr,CespDataFat,CespProtFat,CespCostoStorico,CespContoQuota,CespContoQuotaAnt,CespContoFondo,CespAliTab,CespDataCessione,CespUltAnnoAmm,CspDesc,CspGru,CspSpe1,CspSpe2,CespCp,CspCp  from VLibroCesp where cast(CespCat as smallint)>=" & DaCateg & " and cast(CespCat as smallint)<=" & ACateg & _
        IIf(AnniPrecedenti = True, " and QuoAnno <= " & Anno, " and QuoAnno = " & Anno) & _
        IIf(Tutti = False, " and ceduto = 'false' and (quoquota>0 or QuoResiduo>0 or VCespVariazioni>0)", "")

        TbCesp = New DataTable
        DaCesp = New SqlDataAdapter(PrintDue, cnCo)
        DaCesp.Fill(TbCesp)

        Dim PrintTre As String = "Select distinct QuoNum,QuoAnno,QuoCoStorIni,QuoCoAmmIni,QuoFondoIni,QuoResiduoIni,QuoNoDetraIni,QuoCoStor,QuoCoAmm,QuoTipoAmm,QuoAli,QuoQuota,QuoFondo,QuoResiduo,QuoNoDetra from VLibroCesp where cast(CespCat as smallint)>=" & DaCateg & " and cast(CespCat as smallint)<=" & ACateg & " and QuoAnno<=" & Anno
        TbQuo = New DataTable
        DaQuo = New SqlDataAdapter(PrintTre, cnCo)
        DaQuo.Fill(TbQuo)

        Dim PrintQuattro As String = "select VCespNum,VCespAnno,VCespProg,VCespGgMm,VCespProt,VCespCaus,VCespVariazioni,VCespCessioni,VCespPlusMinus,VCespAnnota,Caus,Segno from VLibroCesp where not VCespNum is null"
        TbVar = New DataTable
        DaVar = New SqlDataAdapter(PrintQuattro, cnCo)
        DaVar.Fill(TbVar)

        TesTest = TbCat.Rows.Count
        If TesTest = 0 Then
            MessageBox.Show("Nessun Cespite corrisponde ai criteri selezionati!", "Stampa Libro Cespiti", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        IniziaComando(LPTIPO, LPRESET) ' MODULO PUBBLICO STAMPE 
        LeggiParametri()
        QualeLp = QSTAMPANTE
        QualeTipo = LPTIPO
        Comprimi = COMPRESSA
        Dest = PathTmp & "Print."
        VerifyDir()
        settaggi(0)
        NomeFile = Ftmp
        Pagine = n2
        NumRighe = 0
        AnnoRif = Anno
        AnniPrec = AnniPrecedenti
        SuBollato = SuBol
        FileOpen(1, NomeFile, OpenMode.Append)
        Dim jj As Int32 = 0

        If Trim(ComandoInizializza) > "" Then PrintLine(1, ComandoInizializza)
        For I As Int16 = 0 To TbCat.Rows.Count - 1
            FineCat = False
            RwCat = TbCat.Rows(I)
            SezTitolo(RwCat)
            'PopolaTbCesp(RwCat("CespCat"))
            Dim RWRW() As DataRow
            RWRW = TbCesp.Select("CespCat >=" & RwCat("CespCat") & " and CespCat<=" & RwCat("CespCat"))
            Dim Cont As Int16 = RWRW.Length - 1
            For j As Int16 = 0 To Cont 'TbCesp.Rows.Count - 1
                RwCesp = RWRW(j) 'TbCesp.Rows(j)
                jj += 1
                SezTestata(RwCesp, jj)
                'PopolaTbQuo(RwCesp("CespNum"))
                Dim RWRW2() As DataRow
                RWRW2 = TbQuo.Select("QuoNum=" & RwCesp("CespNum") & IIf(AnniPrec = True, "", " and QuoAnno=" & AnnoRif))
                Dim Cont2 As Int16 = RWRW2.Length - 1
                For z As Int16 = 0 To Cont2 'TbQuo.Rows.Count - 1
                    RwQuo = RWRW2(z) 'TbQuo.Rows(z)
                    SezQuo(RwQuo)
                    'PopolaTbVar(RwQuo("QuoNum"), RwQuo("QuoAnno"))
                    Dim RWRW3() As DataRow
                    RWRW3 = TbVar.Select("VCespNum=" & RwQuo("QuoNum") & " and VCespAnno=" & RwQuo("QuoAnno"))
                    Dim Cont3 As Int16 = RWRW3.Length - 1
                    For y As Int16 = 0 To Cont3 'TbVar.Rows.Count - 1
                        RwVar = RWRW3(y) 'TbVar.Rows(y)
                        SezVar(RwVar, y + 1)
                    Next
                    'SezQuoTot(RwQuo, IIf(z = TbQuo.Rows.Count - 1, True, False))
                    SezQuoTot(RwQuo, IIf(z = Cont2, True, False))
                    Tot1 = 0
                    Tot2 = 0
                Next
            Next
            SezTotCat()
            TotGQuota += TotQuota
            TotGFondo += TotFondo
            TotGResiduo += TotResiduo
            TotGND += TotND

            TotQuota = 0
            TotFondo = 0
            TotResiduo = 0
            TotND = 0
            TotCCos = 0
            TotCVal = 0

            'If SaltoPagina = False And Not I = TbCat.Rows.Count - 1 Then
            '    Dim Str As String
            '    Str = "".PadLeft(TotCol, "-")
            '    PrintLineX(Str)
            'End If

            If Not I = TbCat.Rows.Count - 1 Then
                Dim Str As String
                Str = "".PadLeft(TotCol, "-")
                PrintLineX(Str)
                PrintLine(1, Chr(12))
                NumRighe = 1
                If Trim(ComandoInizializza) > "" Then PrintLine(1, ComandoInizializza)
            End If
        Next

        If TbCat.Rows.Count > 0 Then
            TotaliGenerali()
        End If

        PrintLine(1, Chr(12))
        FileClose(1)
        Fine = TimeString
        'MessageBox.Show(Inizio & "     " & Fine, "", MessageBoxButtons.OK)
        '
        settaggi(1)
        '
    End Sub

    Private Sub SezTitolo(ByVal Rw As DataRow)
        Dim Str As String
        Dim PRT(10) As String
        Dim x As Int16
        For x = 0 To 10
            PRT(x) = ""
        Next
        Pagine = Pagine + 1
        Str = " Anno " & AnnoRif & "                   " & Rw("CespCat") & " " & Rw("CspDesc").ToString.PadRight(43) ' & " Pag. N. " & Pagine.ToString.PadLeft(3)

        'modificato in data 27/07/2006
        If SuBollato = False Then
            RigheVuote(6)
            'Else
            '    Str = "Stampa Libro Cespiti Ammortizzabili  al  " & Today.ToShortDateString & " - Anno " & AnnoRif & "      " & Rw("CespCat") & " " & Rw("CspDesc").ToString.PadRight(43) & " Pag. N. " & Pagine.ToString.PadLeft(3)


        Else
            PRT(0) = "".PadLeft(TotCol, "-")
            PRT(5) = IT(1).PadRight(130, " ")
            PRT(6) = IT(2).PadRight(130, " ")
            PRT(7) = IT(3).PadRight(130, " ")
            PRT(8) = IT(0).PadRight(55, " ") & IT(6).PadRight(51, " ") & "Pagina N. " & AnnoLp & barra & Pagine.ToString.PadLeft(6, " ")
            PrintLine(1, PRT(0))
            PrintLine(1, PRT(5))
            PrintLine(1, PRT(6))
            PrintLine(1, PRT(7))
            PrintLine(1, PRT(8))
            PrintLine(1, PRT(0))
        End If
        PrintLineX(Str)

        Str = "".PadLeft(TotCol, "=")
        PrintLineX(Str)

        Str = "Nr. |Csp |Anno|ggmm|Prot. |C|              D e s c r i z i o n e              |  %  |Quota Amm. |Fondo Amm. |  Residuo  |Quota N.D. |"
        PrintLineX(Str)

        Str = "".PadLeft(TotCol, "-")
        PrintLineX(Str)
    End Sub

    Private Sub SezTestata(ByVal Rw As DataRow, ByVal Num As Int32)
        Dim Str As String
        Str = Num.ToString.PadLeft(4) & "|" & Rw("CespNum").ToString.PadLeft(4) & "|" & Rw("CespAnnoA") & "|" & Format(CDate(Rw("CespDataFat")), "ddMM") & "|" & Rw("CespProtFat").ToString.PadLeft(6, "0") & "| |" & Mid(Rw("CespDescr"), 1, 49).ToString.PadRight(49) & "|     |           |           |           |           |"
        PrintLineX(Str)
        Str = "    |    |    |    |      | |" & Mid(Rw("CespDescr"), 50, 49).ToString.PadRight(49) & "|     |           |           |           |           |"
        PrintLineX(Str)
        If Rw("CespDescr").ToString.Length > 98 Then
            Str = "    |    |    |    |      | |" & Mid(Rw("CespDescr"), 99, 49).ToString.PadRight(49) & "|     |           |           |           |           |"
            PrintLineX(Str)
        End If
        Str = "    |    |    |    |      | |-------------------------------------------------|     |           |           |           |           |"
        PrintLineX(Str)
    End Sub

    Private Sub SezQuo(ByVal Rw As Object)
        Dim Anno As String = IIf(AnniPrec = True, Rw("QuoAnno"), "".PadLeft(4))
        Dim CoAmmIni As String = IIf(Rw("QuoCoAmmIni") > 0, Format(CDec(Rw("QuoCoAmmIni")), "######0.00").PadLeft(9), "".PadLeft(9))
        Dim FondoIni As String = IIf(Rw("QuoFondoIni") > 0, Format(CDec(Rw("QuoFondoIni")), "######0.00").PadLeft(9), "".PadLeft(9))

        Dim Str As String
        Str = "    |    |" & Anno & "|    |      | |V.Bene - 01/01|" & CoAmmIni & "|" & FondoIni & "|Fondo -  01/01|     |           |           |           |           |"
        PrintLineX(Str)
        Tot1 = Tot1 + CDec(Rw("QuoCoAmmIni"))
        Tot2 = Tot2 + CDec(Rw("QuoFondoIni"))
    End Sub

    Private Sub SezQuoTot(ByVal Rw As Object, ByVal Fine As Boolean)
        Dim Totale1 As String = IIf(Tot1 > 0, Format(Tot1, "######0.00").PadLeft(9), "".PadLeft(9))
        Dim Totale2 As String = IIf(Tot2 > 0, Format(Tot2, "######0.00").PadLeft(9), "".PadLeft(9))
        Dim CoAmm As String = IIf(Rw("QuoCoAmm") > 0, Format(CDec(Rw("QuoCoAmm")), "######0.00").PadLeft(10), "".PadLeft(10))
        Dim Ali As String = IIf(Rw("QuoAli") > 0, IIf(Rw("QuoAli") < 100, Format(Rw("QuoAli"), "#0.00").PadLeft(5), Format(Rw("QuoAli"), "0")), "".PadLeft(5))
        Dim Quota As String = IIf(Rw("QuoQuota") > 0, Format(Rw("QuoQuota"), "######0.00").PadLeft(11), "".PadLeft(11))
        Dim Fondo As String = IIf(Rw("QuoFondo") > 0, Format(Rw("QuoFondo"), "######0.00").PadLeft(11), "".PadLeft(11))
        Dim Residuo As String = IIf(Rw("QuoResiduo") > 0, Format(Rw("QuoResiduo"), "######0.00").PadLeft(11), "".PadLeft(11))
        Dim NoDetra As String = IIf(Rw("QuoNoDetra") > 0, Format(Rw("QuoNoDetra"), "######0.00").PadLeft(11), "".PadLeft(11))
        Dim Str As String


        If Rw("QuoAli") = 100 Then
            Ali = " 100 "
        End If

        Str = "    |    |    |    |      | |-------------------------------------------------|     |           |           |           |           |"
        PrintLineX(Str)

        Str = "    |    |    |    |      | |TOTALI - 31/12|" & Totale1 & "|" & Totale2 & "|CxA|" & CoAmm & "|" & Ali & "|" & Quota & "|" & Fondo & "|" & Residuo & "|" & NoDetra & "|"
        PrintLineX(Str)

        If Fine = True Then
            Str = "----------------------------|-------------------------------------------------|-----|-----------|-----------|-----------|-----------|"
        Else
            Str = "    |    |    |    |      | |-------------------------------------------------|-----|-----------|-----------|-----------|-----------|"
        End If
        PrintLineX(Str)
        TotVal = TotVal + Tot1
        TotCos = TotCos + Rw("QuoCoAmm")
        TotQuota = TotQuota + Rw("QuoQuota")
        TotFondo = TotFondo + Rw("QuoFondo")
        TotResiduo = TotResiduo + Rw("QuoResiduo")
        TotND = TotND + Rw("QuoNoDetra")
        TotCVal = TotCVal + Tot1
        TotCCos = TotCCos + Rw("QuoCoAmm")
    End Sub


    Private Sub SezVar(ByVal Rw As Object, ByVal Num As Int16)
        Dim GgMm As String = Rw("VCespGgMm")
        Dim Prot As String = Rw("VCespProt").ToString.PadLeft(6, "0")
        Dim Cau As String = Rw("VCespCaus")
        Dim Caus As String = Rw("Caus").ToString.PadRight(12)
        Dim Segno As String = Rw("Segno")
        Dim Variazioni As String = Format(Math.Abs(CDec(Rw("VCespVariazioni"))), "#####0.00").PadLeft(9)
        If Cau = 2 Then Variazioni = Format(Math.Abs(CDec(Rw("VCespCessioni"))), "#####0.00").PadLeft(9) : Segno = "+" : Tot2 = 0
        Dim PlusMinus As String = IIf(Rw("VCespPlusMinus") <> 0, Format(CDec(Rw("VCespPlusMinus")), "#####0.00").PadLeft(10), "".PadLeft(10))
        Dim PLMI As String
        Dim Str As String

        If Rw("VCespPlusMinus") < 0 Then PLMI = "MIN" Else PLMI = "PLU"

        If Num = 1 Then
            Str = "    |    |    |    |      | |-------------------------------------------------|     |           |           |           |           |"
            PrintLineX(Str)
        End If

        'Str = "    |    |    |" & GgMm & "|" & Prot & "|" & Cau & "|" & Caus & "|" & Segno & "|" & Variazioni & "|         |PLU|" & PlusMinus & "|     |           |           |           |           |"
        Str = "    |    |    |" & GgMm & "|" & Prot & "|" & Cau & "|" & Caus & "|" & Segno & "|" & Variazioni & "|         |" & PLMI & "|" & PlusMinus & "|     |           |           |           |           |"





        PrintLineX(Str)
        'Tot1 = Tot1 + Rw("VCespVariazioni") --- versione precedente
        If Cau = 3 Then
            Tot2 = Tot2 + Rw("VCespVariazioni")
        Else
            Tot1 = Tot1 + Rw("VCespVariazioni")
        End If
    End Sub

    Private Sub SezTotCat()
        ' per evitare che intesti la pagina successiva per una riga
        NumRighe = NumRighe + 4
        If NumRighe = MaxRighe Then
            Dim Strcat As String
            Strcat = "".PadLeft(TotCol, "-")
            PrintLineX(Strcat)
            PrintLine(1, Chr(12))
            NumRighe = 1
            If Trim(ComandoInizializza) > "" Then PrintLine(1, ComandoInizializza)
            SezTitolo(RwCat)
        Else
            NumRighe = NumRighe - 4
        End If

        Dim TCVal As String = IIf(TotCVal > 0, Format(TotCVal, "######0.00").PadLeft(16), "".PadLeft(16))
        Dim TCCos As String = IIf(TotCCos > 0, Format(TotCCos, "######0.00").PadLeft(16), "".PadLeft(16))
        Dim TQuo As String = IIf(TotQuota > 0, Format(TotQuota, "######0.00").PadLeft(11), "".PadLeft(11))
        Dim TFon As String = IIf(TotFondo > 0, Format(TotFondo, "######0.00").PadLeft(11), "".PadLeft(11))
        Dim TRes As String = IIf(TotResiduo > 0, Format(TotResiduo, "######0.00").PadLeft(11), "".PadLeft(11))
        Dim TND As String = IIf(TotND > 0, Format(TotND, "######0.00").PadLeft(11), "".PadLeft(11))
        Dim Str As String
        Str = "                                            |  Valore Beni   |Costo da Ammort.|     |           |           |           |           |"
        PrintLineX(Str)
        Str = "T O T A L I   C A T E G O R I A- - - - - >  |" & TCVal & "|" & TCCos & "|     |" & TQuo & "|" & TFon & "|" & TRes & "|" & TND & "|"
        PrintLineX(Str)
        'Str = "                                             ---------------------------------------------------------------------------------------"
        'PrintLineX(Str)
    End Sub

    Private Sub TotaliGenerali()
        Dim TVal As String = IIf(TotVal > 0, Format(TotVal, "######0.00").PadLeft(16), "".PadLeft(16))
        Dim TCos As String = IIf(TotCos > 0, Format(TotCos, "######0.00").PadLeft(16), "".PadLeft(16))
        Dim TQuo As String = IIf(TotGQuota > 0, Format(TotGQuota, "######0.00").PadLeft(11), "".PadLeft(11))
        Dim TFon As String = IIf(TotGFondo > 0, Format(TotGFondo, "######0.00").PadLeft(11), "".PadLeft(11))
        Dim TRes As String = IIf(TotGResiduo > 0, Format(TotGResiduo, "######0.00").PadLeft(11), "".PadLeft(11))
        Dim TND As String = IIf(TotGND > 0, Format(TotGND, "######0.00").PadLeft(11), "".PadLeft(11))
        Dim Str As String
        Str = "".PadLeft(TotCol, "-")
        PrintLineX(Str)
        Str = "".PadLeft(TotCol, "")
        PrintLineX(Str)
        Str = "".PadLeft(TotCol, "-")
        PrintLineX(Str)
        Str = "T O T A L I   G E N E R A L I               |  Valore Beni   |Costo da Ammort.|     |           |           |           |           |"
        PrintLineX(Str)
        Str = "                                             ---------------------------------|     |           |           |           |           |"
        PrintLineX(Str)

        Str = "A L   3 1 / 1 2                             |" & TVal & "|" & TCos & "|     |" & TQuo & "|" & TFon & "|" & TRes & "|" & TND & "|"
        PrintLineX(Str)
        Str = "                                             ---------------------------------------------------------------------------------------"
        PrintLineX(Str)
        If SuBollato = True Then
            StampaRigaChiusura()
        End If
    End Sub

    Private Sub StampaRigaChiusura()
        Dim Str As String
        Str = "".PadLeft(TotCol, "*")
        PrintLineX(Str)
        Str = "".PadLeft(TotCol, " ")
        PrintLineX(Str)
        Str = "".PadLeft(TotCol, "*")
        PrintLineX(Str)
        Dim k As Int16 = 17
        For I As Int16 = 0 To MaxRighe - NumRighe - 3
            Str = "".PadLeft(k, " ") & "*"
            k = k + 1
            PrintLineX(Str)
        Next
        Str = "".PadLeft(TotCol, "*")
        PrintLineX(Str)
    End Sub

    Private Sub PrintLineX(ByVal Line As String)
        NumRighe = NumRighe + 1
        If NumRighe = MaxRighe Then
            Dim Str As String
            Str = "".PadLeft(TotCol, "-")
            PrintLineX(Str)
            PrintLine(1, Chr(12))
            NumRighe = 1
            If Trim(ComandoInizializza) > "" Then PrintLine(1, ComandoInizializza)
            SezTitolo(RwCat)
        End If
        PrintLine(1, Line)
    End Sub


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
        FtSt = PathSto & Tdoc & Rif
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

    Private Function LeggiParametri() As Boolean
        Dim cmd As New SqlCommand(" SELECT * from TbSel where selId = 1", cnVd)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            PathPrg = dataRd.GetString(12) & dataRd.GetString(1)
            PathSto = dataRd.GetString(12) & dataRd.GetString(11)
            PathTmp = dataRd.GetString(8)
        End While
        dataRd.Close()
    End Function

    Private Function LeggiAzienda(ByVal AZI As String) As Boolean
        Dim z As Int16
        For z = 0 To 6
            IT(z) = ""
        Next
        If AZI = "" Then Exit Function
        Cmd = New SqlCommand("SELECT * from TbAna where anaGrp = 'AZ' and ANACOD = '" & AZI & "'", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            IT(1) = " " & dataRd.Item("AnaDesc")
            IT(2) = " " & dataRd.Item("AnaIndirizzo")
            IT(3) = " " & Trim(dataRd.Item("AnaCap")) & " " & Trim(dataRd.Item("AnaCitta")) & " " & Trim(dataRd.Item("AnaProv"))
            IT(0) = " " & Trim(dataRd.Item("AnaPiva")) & " - " & Trim(dataRd.Item("AnaCFis"))
        End While
        dataRd.Close()
    End Function
End Module

