Imports DXBASE
Imports NCCOM
Imports System.IO
Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Public Module RegIvaPdf
    Dim PathPrg, PathFil, Orig, Stampa, QualeLp, QualeReg, NomeFile, DataAl, Priga, PSerie As String
    Dim PathSto, Tdoc, IT(6) As String
    Dim PathTmp As String
    Dim StampaLaser As Boolean = False
    Dim Vendite, Corrisp, Intesta As Boolean
    Dim Rif, TesTest, MaxProt, Np As Int32
    Dim Pagine, IdProc, MaxCorp, Linea, AnnoIva As Int16
    Dim Bollo As Boolean
    Dim IvaCreditoP As Decimal

    Dim DsRep As DataSet
    Dim DaRep As SqlDataAdapter
    Dim RwRep As DataRow

    Dim DsIvp As DataSet
    Dim DaIvp As SqlDataAdapter
    Dim RwIvp As DataRow
    Dim doc As Document
    Dim myPhrase As Phrase
    Dim CiiPam As New ArrayList

    Public Function PrintRegIva(ByVal NRREG As Int16, ByVal BLOCK As Int32, ByVal TIPOREG As Int16, ByVal QSTAMPANTE As String, _
    ByVal INBOLLO As Boolean, ByVal REGDESC As String, ByVal FINOAL As String, ByVal CreditoP As Decimal, ByVal QANNO As Int16, _
    ByRef FILESTAMPA As String, ByRef PP As Int32, ByVal AZI As String, ByVal DESCREG As String, ByVal SERIEL As String) As Boolean
        Dim PrintUno As String = "Select * from CRREGIVA WHERE PRegNumReg = " & NRREG & " AND PREGID = " & BLOCK & " ORDER BY PRegNumReg, PRegNumProt, PRegProtBis, PRegPriId,PRegPriProg"
        Dim PrintDue As String = "Select * from VXRIEPIVA WHERE TIvaPRegIva = " & NRREG & " AND TIVAPID = " & BLOCK
        DsRep = New DataSet
        DaRep = New SqlDataAdapter(PrintUno, cnCo)
        DaRep.Fill(DsRep)
        DsIvp = New DataSet
        DaIvp = New SqlDataAdapter(PrintDue, cnCo)
        DaIvp.Fill(DsIvp)
        'Dim Righe, x As Int32
        TesTest = DsRep.Tables(0).Rows.Count
        If TesTest = 0 Then Exit Function
        If TIPOREG = 2 Or TIPOREG = 4 Then
            Vendite = False
        Else
            Vendite = True
        End If
        If TIPOREG = 5 Then
            Corrisp = True
        Else
            Corrisp = False
        End If
        Priga = ""
        Orig = ""
        PSerie = ""
        LeggiParametri(AZI)
        Bollo = INBOLLO
        QualeReg = REGDESC
        DataAl = FINOAL
        IvaCreditoP = CreditoP
        AnnoIva = QANNO - 1
        If TIPOREG = 2 And IvaCreditoP < 0 Then
            Priga = "".PadLeft(26) & "CREDITO IVA ANNO " & AnnoIva & " EURO " & Format(CreditoP, "###,###,##0.00") _
            & " ( art. 30 DPR n. 633/72) "
        End If
        VerifyDir()
        MaxCorp = 66
        Linea = 0
        Tdoc = "REGIVA" & NRREG '' tipo documento registri iva + NR REGISTRO
        Rif = BLOCK
        NomeFile = PathTmp & Tdoc & FINOAL & ".Pdf"
        FILESTAMPA = NomeFile
        settaggi(0)
        Pagine = 0
        If Val(AZI) > 0 Then Intesta = True Else Intesta = False
        Np = PP
        IT(6) = DESCREG
        PSerie = SERIEL
        SezTestata()
        SezCorpo()
        SezChiude()
        settaggi(1)
        PP = Np
    End Function
    Private Function SezChiude() As Boolean
        Dim Lpdec(12) As String
        Dim f, k, z As Int16
        Dim PRT(6), WT(6) As String
        Dim PRTRIAS As String = ""
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

        If Corrisp = True Then
            PRT(1) = WT(3)
            PRT(2) = WT(4)
        Else
            PRT(1) = WT(1)
            PRT(2) = WT(2)
        End If

        If Corrisp = False And Vendite = True And CiiPam.Count > 0 Then
            PRT(1) = WT(5)
            PRT(2) = WT(6)
        End If


        PRT(3) = PRT(3).PadLeft(17, " ") & "Riepilogo Registro Iva " & QualeReg & " " & DataAl _
        & " Fino Al  Nr. " & MaxProt
        PRT(0) = "".PadLeft(16, " ") & "".PadRight(92, "-")
        PRT(4) = " " & PRT(4).PadLeft(130, "*")
        'PRT(1) = PRT(1).PadLeft(16, " ")
        'PRT(2) = PRT(2).PadLeft(16, " ")
        RigheVuote(1)
        If (Linea + 7) > MaxCorp Then
            Linea = Linea + 7
            Contatore()
        End If
        Linea = Linea + 5
        Contatore() ' PER EVITARE DI STAMPARE SOLO RIGHE DI INTESTAZIONE ???? DA RIVEDERE  + 1 !!!!!

        myPhrase = New Phrase(PRT(3) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
        myPhrase.Add(New Phrase(PRT(0) & Microsoft.VisualBasic.Chr(10) & ""))
        myPhrase.Add(New Phrase(PRT(1) & Microsoft.VisualBasic.Chr(10) & ""))
        myPhrase.Add(New Phrase(PRT(2) & Microsoft.VisualBasic.Chr(10) & ""))
        myPhrase.Add(New Phrase(PRT(0) & Microsoft.VisualBasic.Chr(10) & ""))
        doc.Add(myPhrase)
        For k = 0 To DsIvp.Tables(0).Rows.Count - 1
            RwIvp = DsIvp.Tables(0).Rows(k)
            For z = 0 To 12
                Lpdec(z) = ""
            Next
            If Corrisp = True Then
                RwIvp("IvaDe") = RwIvp("Timpon") / (100 + RwIvp("CiiAli")) * 100
                RwIvp("IvaND") = Format(RwIvp("IvaDe") * RwIvp("CiiAli") / 100, "###,###,##0.00")
                RwIvp("Merce") = 0
                Totale = Totale + RwIvp("Timpon")
            Else
                Totale = Totale + RwIvp("Timpon") + RwIvp("IvaDe") + RwIvp("IvaND")
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
            myPhrase = New Phrase(Lpdec(11) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
            doc.Add(myPhrase)
        Next
        Contatore()
        myPhrase = New Phrase(PRT(0) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
        doc.Add(myPhrase)
        Contatore()
        For z = 0 To 12
            Lpdec(z) = ""
        Next
        Lpdec(0) = "   TOTALI"
        If Impon <> 0 Then Lpdec(1) = Format(Impon, "###,###,##0.00")
        If IvaDe <> 0 Then Lpdec(2) = Format(IvaDe, "###,###,##0.00")
        If IvaNd <> 0 Then Lpdec(3) = Format(IvaNd, "###,###,##0.00")
        If Merce <> 0 Then Lpdec(4) = Format(Merce, "###,###,##0.00")
        If Totale <> 0 Then Lpdec(5) = Format(Totale, "###,###,##0.00")
        Contatore()
        Lpch(Lpdec)
        For z = 0 To 5
            Lpdec(11) = Lpdec(11) & Lpdec(z)
        Next
        myPhrase = New Phrase(Lpdec(11) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
        myPhrase.Add(New Phrase(PRT(0) & Microsoft.VisualBasic.Chr(10) & ""))
        doc.Add(myPhrase)
        Linea = Linea + 1
        If (Linea + 5) >= MaxCorp Then GoTo ChiudeDefinitivamente
        Contatore()
        myPhrase = New Phrase(PRT(4) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
        f = MaxCorp - (Linea + 2)
        If f > 0 Then
            For z = Linea + 2 To MaxCorp
                PRTRIAS = "".PadLeft(z + 2, " ") & "*"
                myPhrase.Add(New Phrase(PRTRIAS & Microsoft.VisualBasic.Chr(10) & ""))
            Next
            myPhrase.Add(New Phrase(PRT(4) & Microsoft.VisualBasic.Chr(10) & ""))
        End If
        doc.Add(myPhrase)
ChiudeDefinitivamente:
        doc.NewPage()
    End Function
    Private Sub Contatore()
        Linea = Linea + 1
        If Linea > MaxCorp Then
            SezTestata()
            Linea = 1
        End If
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
        Dim LpDec(12) As String
        Dim z, k, s, q As Int32
        Dim TotaleFt, TotaleIm As Decimal
        Dim RwFt As DataRow = Nothing
        Linea = 0
        TotaleFt = 0
        TotaleIm = 0
        s = 0
        TesTest = TesTest - 1
        'Dettaglio
        For k = 0 To TesTest
            RwRep = DsRep.Tables(0).Rows(k)
            If k < TesTest Then
                RwFt = DsRep.Tables(0).Rows(k + 1)
            End If
            For z = 0 To 12
                LpDec(z) = ""
            Next
            If s = 0 Then
                LpDec(12) = RwRep("PRegDataG")
                LpDec(0) = Mid(LpDec(12), 1, 2) & Mid(LpDec(12), 4, 2) & Mid(LpDec(12), 7, 4)
                LpDec(12) = RwRep("PRegDataE")
                LpDec(1) = Mid(LpDec(12), 1, 2) & Mid(LpDec(12), 4, 2) & Mid(LpDec(12), 7, 4)
                LpDec(2) = RwRep("PRegNumProt")
                If Vendite = True Then
                    LpDec(3) = PSerie.PadRight(6, " ")
                Else
                    LpDec(3) = RwRep("PRegNumDoc")
                End If
                LpDec(4) = RwRep("PRegCliFor")
                LpDec(5) = Mid(RwRep("PRegAnaGraf"), 1, 28)
            Else
                For q = 0 To 5
                    LpDec(q) = ""
                Next q
                If s = 1 Then
                    If RwRep("PRegProtBis").ToString.Trim.Length > 0 Or Mid(RwRep("PRegAnaGraf"), 29, 28).Trim.Length > 0 Or (RwRep("PRegValuta") <> 0 And Vendite = True) Then
                        LpRageBis(LpDec)
                    End If
                End If
            End If
            If Corrisp = True Then LpDec(4) = RwRep("PRegCliFor")
            TotaleIm = TotaleIm + RwRep("PRegImpon")
            LpDec(6) = Format(TotaleIm, "##,###,##0.00")
            LpDec(7) = RwRep("PRegAliq")
            If RwRep("PRegImpIva") <> 0 Then
                LpDec(8) = Format(RwRep("PRegImpIva"), "###,###,##0.00")
            Else
                LpDec(8) = ""
            End If
            LpDec(10) = RwRep("PRegCpt")
            TotaleFt = TotaleFt + RwRep("PRegImpon") + RwRep("PRegImpIva")
            If k = TesTest Then
                LpDec(9) = Format(TotaleFt, "###,###,##0.00")
                TotaleFt = 0
                Stampa = True
                s = -1
            Else
                If RwRep("PRegNumProt").ToString & RwRep("PRegProtBis") <> RwFt("PRegNumProt").ToString & RwFt("PRegProtBis") Then
                    LpDec(9) = Format(TotaleFt, "###,###,##0.00")
                    TotaleFt = 0
                    Stampa = True
                    s = -1
                Else
                    LpDec(9) = ""
                    Stampa = False
                End If
            End If

            If Stampa = True Or RwRep("PRegCodIva") > 0 Then GoTo Riproponi Else GoTo Ritorno
Riproponi:
            Contatore()
            LpLp(LpDec)
            For z = 0 To 10
                LpDec(11) = LpDec(11) & LpDec(z)
            Next
            myPhrase = New Phrase(LpDec(11) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
            doc.Add(myPhrase)
            TotaleIm = 0
            s = s + 1
            If Stampa = True Then
                If Trim(Mid(LpDec(5), 1, 28)).Length = 0 Then RwRep("PRegAnaGraf") = ""
                If RwRep("PRegProtBis").ToString.Trim.Length > 0 Or Mid(RwRep("PRegAnaGraf"), 29, 28).Trim.Length > 0 Or (RwRep("PRegValuta") <> 0 And Vendite = True) Then
                    For q = 0 To 11
                        LpDec(q) = ""
                    Next
                    LpRageBis(LpDec)
                    s = s - 1
                    Stampa = False
                    GoTo riproponi
                End If
            End If
Ritorno:
            MaxProt = RwRep("PRegNumProt")
        Next
    End Function
    Private Sub LpRageBis(ByVal lpdec() As String)
        lpdec(5) = Mid(RwRep("PRegAnaGraf"), 29, 28)
        RwRep("PRegAnaGraf") = ""
        If RwRep("PRegProtBis") = "B" Then
            lpdec(2) = "BIS"
        Else
            If RwRep("PRegProtBis") = "R" Then
                lpdec(2) = "RETT"
            Else
                If RwRep("PRegProtBis") = "S" Then
                    lpdec(2) = "*SOSP*"
                Else
                    lpdec(2) = ""
                End If
            End If
        End If
        If RwRep("PRegValuta") <> 0 And Vendite = True Then
            lpdec(6) = Format(RwRep("PRegValuta"), "##,###,##0.00")
            lpdec(7) = "VALUTAESTERA"
        Else
            lpdec(6) = ""
            lpdec(7) = ""
        End If
    End Sub
    Private Sub LpLp(ByVal Lpdec() As String)
        Lpdec(0) = Lpdec(0).PadLeft(8, " ") & "|"
        Lpdec(1) = Lpdec(1).PadLeft(8, " ") & "|"
        Lpdec(2) = Lpdec(2).PadLeft(6, " ") & "|"
        Lpdec(3) = Lpdec(3).PadLeft(6, " ") & "|"
        Lpdec(4) = Lpdec(4).PadLeft(5, " ") & "|"
        Lpdec(5) = Lpdec(5).PadRight(28, " ") & "|"
        Lpdec(6) = Lpdec(6).PadLeft(14, " ") & "|"
        Lpdec(7) = Lpdec(7).PadRight(12, " ") & "|"
        Lpdec(8) = Lpdec(8).PadLeft(13, " ") & "|"
        Lpdec(9) = Lpdec(9).PadLeft(15, " ") & "|"
        Lpdec(10) = Lpdec(10).PadLeft(5, " ")
    End Sub
    Private Function SezTestata() As Boolean
        Dim PRT(10), WT(10) As String
        Dim x, WP(1) As Int16
        For x = 0 To 10
            PRT(x) = ""
            WT(x) = ""
        Next
        WT(3) = "".PadLeft(28, " ")
        If Vendite = False Then
            WT(0) = "Protoc"
            WT(1) = "N.Fatt"
            WT(2) = "     F O R N I T O R E      "
            WT(4) = "Fornit"
            WT(5) = " For "
            WT(6) = "Fattura "
            WT(7) = " Imponibile o "
            WT(8) = "   Importo    "
            WT(9) = "    I.v.a.   "
            WT(10) = "    Fattura    "
        Else
            WT(0) = " Fatt."
            WT(1) = "Serie "
            WT(2) = "       C L I E N T E        "
            WT(4) = "Lett. "
            WT(5) = " Cli "
            WT(6) = "Fattura "
            WT(7) = " Imponibile o "
            WT(8) = "   Importo    "
            WT(9) = "    I.v.a.   "
            WT(10) = "    Fattura    "
        End If
        If Corrisp = True Then
            WT(0) = "Protoc"
            WT(1) = "      "
            WT(2) = "   D E S C R I Z I O N E    "
            WT(4) = "      "
            WT(5) = " Cpt "
            WT(6) = "        "
            WT(7) = "   Importo    "
            WT(8) = "   Lordo      "
            WT(9) = "             "
            WT(10) = "  Giornaliero  "
        End If
        PRT(0) = " " & PRT(0).PadLeft(130, "-")
        PRT(1) = "  Data  " & "|" & "  Data  " & "|" & WT(0) & "|" & WT(1) & "|" & " Cod " & "|" & WT(2) & "|" _
        & WT(7) & "|" & "Aliquota Iva" & "|" & WT(9) & "|" & "    Totale     " & "|" & " Cod "

        PRT(2) = "Giornale" & "|" & WT(6) & "|" & "Numero" & "|" & WT(4) & "|" & WT(5) & "|" & WT(3) & "|" _
        & WT(8) & "|" & "  Causale   " & "|" & "             " & "|" & WT(10) & "|" & " Cpt "
        Pagine = Pagine + 1
        If Pagine > 1 Then
            doc.NewPage()
        End If
        '' INTESTAZIONE REGISTRI '''
        If Intesta = False Then
            Np = Np + 1
            RigheVuote(6)
        Else
            Np = Np + 1
            PRT(5) = " " & IT(1).PadRight(130, " ") & " "
            PRT(6) = " " & IT(2).PadRight(130, " ") & " "
            PRT(7) = " " & IT(3).PadRight(130, " ") & " "
            PRT(8) = " " & IT(0).PadRight(53, " ") & IT(6).PadRight(51, " ") & "Pagina N. " & (AnnoIva + 1) & " / " & Np.ToString.PadLeft(6, " ") & "  "
            myPhrase = New Phrase(PRT(0) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
            myPhrase.Add(New Phrase(PRT(5) & Microsoft.VisualBasic.Chr(10) & ""))
            myPhrase.Add(New Phrase(PRT(6) & Microsoft.VisualBasic.Chr(10) & ""))
            myPhrase.Add(New Phrase(PRT(7) & Microsoft.VisualBasic.Chr(10) & ""))
            myPhrase.Add(New Phrase(PRT(8) & Microsoft.VisualBasic.Chr(10) & ""))
            myPhrase.Add(New Phrase(PRT(0) & Microsoft.VisualBasic.Chr(10) & ""))
            doc.Add(myPhrase)
        End If
        '' INTESTAZIONE REGISTRI '''
        myPhrase = New Phrase(PRT(1) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
        myPhrase.Add(New Phrase(PRT(2) & Microsoft.VisualBasic.Chr(10) & ""))
        myPhrase.Add(New Phrase(PRT(0) & Microsoft.VisualBasic.Chr(10) & ""))
        If Priga > "" Then myPhrase.Add(New Phrase(Priga & Microsoft.VisualBasic.Chr(10) & "")) : Priga = ""
        doc.Add(myPhrase)
    End Function
    Private Function RigheVuote(ByVal n) As Boolean
        Dim x As Int16
        For x = 1 To n
            myPhrase = New Phrase("" & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
            doc.Add(myPhrase)
        Next
    End Function
    Private Sub VerifyDir()
        If Directory.Exists(PathSto) = False Then Directory.CreateDirectory(PathSto)
        If Directory.Exists(PathTmp) = False Then Directory.CreateDirectory(PathTmp)
    End Sub
    Private Function settaggi(ByVal ID) As Boolean
        If ID = 1 Then GoTo fine
Inizio:
        doc = New Document
        doc.SetPageSize(PageSize.A4)
        Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(NomeFile, FileMode.Create))
        doc.Open()

        Exit Function
fine:
        doc.Close()
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
