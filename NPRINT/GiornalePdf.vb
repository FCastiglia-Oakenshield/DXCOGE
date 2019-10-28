Imports DXBASE
Imports NCCOM
Imports System.IO
Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Public Module GiornalePdf
    Dim PathPrg, PathFil, Orig, Concat, Stampa, QualeLp, QualeReg, NomeFile, DataAl, Priga, QualeTipo As String
    Dim PathSto, Tdoc, Causale(72), IT(6), DalAl As String
    Dim PathTmp, AD As String
    Dim StampaLaser As Boolean = False
    Dim Rif, TesTest, MaxProt, Articolo, Np As Int32
    Dim Pagine, IdProc, MaxCorp, Linea, AnnoLP, LL As Int16
    Dim Bollo, Comprimi, Intesta, Multirigo As Boolean
    Dim TotaleGenerale As Decimal = 0
    Dim DsGio As DataSet
    Dim DaGio As SqlDataAdapter
    Dim RwGio As DataRow
    Dim doc As Document
    Dim myPhrase As Phrase

    Public Function PrintGiornale(ByVal INBOLLO As Boolean, ByVal COMPRESSA As Boolean, ByVal UART As Int32, ByVal RIPO As Decimal, ByVal ANNOG As Int16, ByVal PP As Int32, ByVal LIBRO As String, ByVal AZI As String, ByRef NOMESTAMPA As String, MULTIDESC As Boolean, ByVal Periodo As String) As Boolean
        Dim PrintUno As String = "Select * from TMPGIO ORDER BY PRIDATAGIO,PRIID,PRIPROG"
        DsGio = New DataSet
        DaGio = New SqlDataAdapter(PrintUno, cnCo)
        DaGio.Fill(DsGio)
        'Dim Righe, x As Int32
        TesTest = DsGio.Tables(0).Rows.Count
        If TesTest = 0 Then Exit Function
        PathFil = ""
        Priga = ""
        Orig = ""
        LeggiParametri(AZI)
        Bollo = INBOLLO
        Comprimi = COMPRESSA
        NomeFile = PathTmp & "GG.Pdf"
        NOMESTAMPA = NomeFile
        Multirigo = MULTIDESC
        VerifyDir()
        MaxCorp = 66 '( + 5 ) 4 sopra 1 riporto sotto
        settaggi(0)
        Pagine = 0
        Linea = 0
        TotaleGenerale = RIPO
        Articolo = UART
        AnnoLP = ANNOG
        DalAl = Periodo
        LL = 55 - Len(DalAl)
        Np = PP
        IT(6) = LIBRO
        If Val(AZI) > 0 Then Intesta = True Else Intesta = False
        SezTestata()
        SezCorpo()
        SezChiude()
        settaggi(1)
        If Bollo = True Then AggiornaBollato()
    End Function
    Private Sub AggiornaBollato()
        Dim x As Int32
        For x = 0 To TesTest
            RwGio = DsGio.Tables(0).Rows(x)
            If RwGio("PriArtFisc") > 0 Then UpGiornale(RwGio)
        Next
        Dim Str As String
        Str = "Update TbESE set EseGioProg = @EseGioProg,EseGioNFog = @EseGioNFog Where EseAnno = " & AnnoLP
        Cmd = New SqlCommand(Str, cnCo)
        Dim p4 As New SqlParameter("@EseGioProg", SqlDbType.Decimal)
        Dim p5 As New SqlParameter("@EseGioNFog", SqlDbType.Int)
        p4.Value = TotaleGenerale
        p5.Value = Np
        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub UpGiornale(ByVal rw As DataRow)
        Dim Str As String
        Str = "Update TbPri set PriGStampa = @PriGStampa ,PriArtFisc =@PriArtFisc Where PriId = @PriId"
        Cmd = New SqlCommand(Str, cnCo)
        Dim p1 As New SqlParameter("@PriGStampa", SqlDbType.Bit)
        Dim p2 As New SqlParameter("@PriArtFisc", SqlDbType.Int)
        Dim p3 As New SqlParameter("@PriId", SqlDbType.Int)
        p1.Value = 1
        p2.Value = RwGio("PriArtFisc")
        p3.Value = RwGio("PriId")
        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Function SezChiude() As Boolean
        Dim PRTAST As String = ""
        Dim PRTRIP As String = ""
        Dim PRTRIAS As String = ""
        Dim f, z As Int16
        PRTRIP = "      " & "|" & "        " & "|" & "".PadLeft(80, " ") & "|" & "A  R I P O R T O" & "|" & Format(TotaleGenerale, "#,###,###,##0.00").PadLeft(16, " ")
        PRTAST = PRTAST.PadLeft(130, "*")
        myPhrase = New Phrase(PRTRIP & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
        doc.Add(myPhrase)
        ''' RIGA A RIPORTO
        If (Linea + 3) >= MaxCorp Then GoTo ChiudeDefinitivamente
        f = MaxCorp - (Linea + 2)
        If f > 0 Then
            myPhrase = New Phrase(PRTAST & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
            For z = Linea + 2 To MaxCorp
                PRTRIAS = "".PadLeft(z + 2, " ") & "*"
                myPhrase.Add(New Phrase(PRTRIAS & Microsoft.VisualBasic.Chr(10) & ""))
            Next
            myPhrase.Add(New Phrase(PRTAST & Microsoft.VisualBasic.Chr(10) & ""))
            doc.Add(myPhrase)
        End If
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
    Private Function SezCorpo() As Boolean
        Dim LpDec(14), ChA, DiversiDare, DiversiAvere As String
        Dim k, s, q, X As Int32
        Dim AA, BB, CC As Int16
        Dim Totale As Decimal
        Dim Fattura As Boolean = False
        Dim Corrisp As Boolean = False
        Dim TotaleFt As Decimal = 0
        LpDec(12) = "      " & "|" & "        " & "|".PadRight(81, " ") & "|".PadRight(17, "-") & "|".PadRight(17, " ")
        LpDec(13) = "      " & "|" & "        " & "|".PadRight(81, "-") & "|".PadRight(17, "-") & "|".PadRight(17, " ")
        LpDec(14) = "      " & "|" & "        " & "|".PadRight(81, "-") & "|".PadRight(17, " ") & "|".PadRight(17, " ")
        Totale = 0
        s = 0
        X = 1
        k = -1
        ChA = " a "
        DiversiDare = "Diversi".PadRight(39, " ")
        DiversiAvere = "Diversi".PadLeft(38, " ")
        TesTest = TesTest - 1
        'Dettaglio
INIZIO:
        k = k + X
        If k > TesTest Then Exit Function
        For q = 0 To 11
            LpDec(q) = ""
        Next
        RwGio = DsGio.Tables(0).Rows(k)
        Dim Rs0 As DataRow()
        Dim S0 As String
        S0 = "PriId = " & RwGio("PriId")
        Rs0 = DsGio.Tables(0).Select(S0)
LETTURAARTICOLO:
        X = Rs0.Length
        CC = 13
        TotaleFt = 0
        Fattura = False
        Corrisp = False
OLTRE:
        Articolo = Articolo + 1
        RwGio("PriArtFisc") = Articolo
        RwGio("PriGStampa") = 1
        LpDec(0) = Articolo
        LpDec(1) = Format(RwGio("PridataGio"), "ddMMyyyy")
        If RwGio("RigheD") = 1 And RwGio("RigheA") = 1 Then GoTo ART01
        If RwGio("RigheD") > 1 And RwGio("RigheA") = 1 Then GoTo ART02
        If RwGio("RigheD") = 1 And RwGio("RigheA") > 1 Then GoTo ART03
        GoTo ART04
        GoTo INIZIO
ART01:  ''' conto  A conto
        For BB = 0 To 1 ' Avere Dare
            Totale = 0
            For AA = 0 To Rs0.Length - 1
                If BB = 0 Then
                    If Rs0(AA).Item("PriCoDare") <> "00.10" Then
                        LpDec(2) = Rs0(AA).Item("PriCoDare") & " " & Mid(Rs0(AA).Item("DAREDESC"), 1, 32)
                        Totale = Rs0(AA).Item("PriImpDare")
                        AD = ChA
                        LpDec(5) = Format(Totale, "#,###,###,##0.00")
                        TotaleRiporto(Totale)
                        Exit For
                    End If
                Else
                    If Rs0(AA).Item("PriCoAvere") <> "00.10" Then
                        LpDec(3) = Rs0(AA).Item("PriCoAvere") & " " & Mid(Rs0(AA).Item("AVEREDESC"), 1, 32)
                        LpDec(4) = ""
                        STAMPARIGA(LpDec)
                        RETTIFICA(LpDec, Rs0(AA))
                        DesRiga(LpDec, Fattura, Rs0(AA))
                        STAMPARIGA(LpDec)
                        Exit For
                    End If
                End If
            Next
        Next
        CC = 14
        GoTo RITORNO
ART02:  ''' DIVERSI A conto
        If Rs0(0).Item("PriCausale") = 2 Then
            Fattura = True
            For AA = 0 To Rs0.Length - 1
                If Rs0(AA).Item("PriCausale") = 3 Then TotaleFt = Rs0(AA).Item("PriImpAvere")
            Next
        End If

        For BB = 0 To 1 ' Avere Dare
            Totale = 0
            For AA = 0 To Rs0.Length - 1
                If BB = 0 Then
                    If Rs0(AA).Item("PriCoAvere") <> "00.10" Then
                        LpDec(3) = Rs0(AA).Item("PriCoAvere") & " " & Mid(Rs0(AA).Item("AVEREDESC"), 1, 32)
                        If Fattura = False Then Totale = Rs0(AA).Item("PriImpAvere") Else Totale = TotaleFt
                        AD = ChA
                        LpDec(2) = DiversiDare
                        LpDec(4) = ""
                        LpDec(5) = Format(Totale, "#,###,###,##0.00")
                        TotaleRiporto(Totale)
                        STAMPARIGA(LpDec)
                        RETTIFICA(LpDec, Rs0(AA))
                        DesRiga(LpDec, Fattura, Rs0(AA))
                        STAMPARIGA(LpDec)
                        Exit For
                    End If
                Else
                    If Rs0(AA).Item("PriCoDare") <> "00.10" Then
                        LpDec(3) = ""
                        LpDec(2) = Rs0(AA).Item("PriCoDare") & " " & Mid(Rs0(AA).Item("DAREDESC"), 1, 32)
                        Totale = Rs0(AA).Item("PriImpDare")
                        AD = "   "
                        LpDec(4) = Format(Totale, "#,###,###,##0.00")
                        LpDec(5) = ""
                        STAMPARIGA(LpDec)
                        ''' NUOVA INSERIMENTO DESCRIZIONE AGGIUNTIVA
                        If Multirigo = True And Fattura = False Then
                            DesRiga(LpDec, Fattura, Rs0(AA)) ''' TEST
                            STAMPARIGA(LpDec) '' TEST
                        End If
                    End If
                    End If
            Next
        Next
        GoTo RITORNO
ART03:  ''' CONTO A DIVERSI

        If Rs0(0).Item("PriCausale") = 1 Then
            Fattura = True
            For AA = 0 To Rs0.Length - 1
                If Rs0(AA).Item("PriCausale") = 3 Then TotaleFt = Rs0(AA).Item("PriImpDare")
            Next
        End If
ART03CORRISP:
        For BB = 0 To 1 ' dare - avere
            Totale = 0
            For AA = 0 To Rs0.Length - 1
                If BB = 0 Then
                    If Rs0(AA).Item("PriCoDare") <> "00.10" Then
                        LpDec(2) = Rs0(AA).Item("PriCoDare") & " " & Mid(Rs0(AA).Item("DAREDESC"), 1, 32)
                        If Fattura = False And Corrisp = False Then Totale = Rs0(AA).Item("PriImpDare") Else Totale = TotaleFt
                        AD = ChA
                        LpDec(3) = DiversiAvere
                        LpDec(4) = ""
                        LpDec(5) = Format(Totale, "#,###,###,##0.00")
                        TotaleRiporto(Totale)
                        STAMPARIGA(LpDec)
                        If Corrisp = True Then
                            LpDec(2) = Trim("Corrispettivi " & Format(Rs0(AA).Item("PriDataEst"), "dd/MM/yyyy") & " " & Trim(Rs0(AA).Item("Pridesc")))
                            LpDec(3) = Mid(LpDec(2), 43, 38)
                            AD = Mid(LpDec(2), 40, 3).PadRight(3, " ")
                            LpDec(2) = Mid(LpDec(2), 1, 39)
                        Else
                            RETTIFICA(LpDec, Rs0(AA))
                            DesRiga(LpDec, Fattura, Rs0(AA))
                        End If
                        STAMPARIGA(LpDec)
                        Exit For
                    End If
                Else
                    If Rs0(AA).Item("PriCoAvere") <> "00.10" Then
                        LpDec(2) = ""
                        LpDec(3) = Rs0(AA).Item("PriCoAvere") & " " & Mid(Rs0(AA).Item("AVEREDESC"), 1, 32)
                        If Fattura = False Or Rs0(AA).Item("PriCausale") > 2 Then
                            Totale = Rs0(AA).Item("PriImpAvere")
                        Else
                            Totale = Rs0(AA).Item("PriImpDare")
                        End If
                        AD = ChA
                        LpDec(4) = Format(Totale, "#,###,###,##0.00")
                        LpDec(5) = ""
                        STAMPARIGA(LpDec)
                        ''' NUOVA INSERIMENTO DESCRIZIONE AGGIUNTIVA
                        If Multirigo = True And Fattura = False Then
                            DesRiga(LpDec, Fattura, Rs0(AA)) ''' TEST
                            STAMPARIGA(LpDec) '' TEST
                        End If
                    End If
                    End If
            Next
        Next
        GoTo RITORNO
ART04:  ''' DIVERSI A DIVERSI
        Totale = 0
        BB = -1
        For AA = 0 To Rs0.Length - 1
            If Rs0(AA).Item("PriCoDare") <> "00.10" Then
                Totale = Totale + Rs0(AA).Item("PriImpDare")
                If BB = -1 Then BB = AA
            End If
        Next
        If BB = -1 Then
            For AA = 0 To Rs0.Length - 1
                If Rs0(AA).Item("PriCoAvere") <> "00.10" Then
                    Totale = Totale + Rs0(AA).Item("PriImpAvere")
                    If BB = -1 Then BB = AA
                End If
            Next
        End If
        If Rs0(BB).Item("TIPOREG") <> 5 Then GoTo ART04B
        For AA = 0 To Rs0.Length - 1
            If Rs0(AA).Item("PriCoDare") <> "00.10" Then
                If Rs0(AA).Item("PriCoDare") <> Rs0(BB).Item("PriCoDare") Then GoTo ART04B
            End If
        Next
        Corrisp = True
        TotaleFt = Totale
        GoTo ART03CORRISP
ART04B:
        LpDec(2) = DiversiDare
        AD = ChA
        LpDec(3) = DiversiAvere
        LpDec(4) = ""
        LpDec(5) = Format(Totale, "#,###,###,##0.00")
        TotaleRiporto(Totale)
        STAMPARIGA(LpDec)
        RETTIFICA(LpDec, Rs0(BB))
        DesRiga(LpDec, Fattura, Rs0(BB))
        STAMPARIGA(LpDec)
        For BB = 0 To 1 ' dare - avere
            For AA = 0 To Rs0.Length - 1
                Totale = 0

                If BB = 0 Then
                    If Rs0(AA).Item("PriCoDare") <> "00.10" Then
                        LpDec(2) = Rs0(AA).Item("PriCoDare") & " " & Mid(Rs0(AA).Item("DAREDESC"), 1, 32)
                        Totale = Rs0(AA).Item("PriImpDare")
                        AD = "   "
                        LpDec(3) = ""
                        LpDec(4) = Format(Totale, "#,###,###,##0.00")
                        LpDec(5) = ""
                        STAMPARIGA(LpDec)
                        ''' NUOVA INSERIMENTO DESCRIZIONE AGGIUNTIVA
                        If Multirigo = True And Fattura = False Then
                            DesRiga(LpDec, Fattura, Rs0(AA)) ''' TEST
                            STAMPARIGA(LpDec) '' TEST
                        End If
                    End If
                Else
                    If Rs0(AA).Item("PriCoAvere") <> "00.10" Then
                        LpDec(2) = ""
                        LpDec(3) = Rs0(AA).Item("PriCoAvere") & " " & Mid(Rs0(AA).Item("AVEREDESC"), 1, 32)
                        Totale = Rs0(AA).Item("PriImpAvere")
                        AD = ChA
                        LpDec(4) = Format(Totale, "#,###,###,##0.00")
                        LpDec(5) = ""
                        STAMPARIGA(LpDec)
                        ''' NUOVA INSERIMENTO DESCRIZIONE AGGIUNTIVA
                        If Multirigo = True And Fattura = False Then
                            DesRiga(LpDec, Fattura, Rs0(AA)) ''' TEST
                            STAMPARIGA(LpDec) '' TEST
                        End If
                    End If
                    End If
            Next
            If BB = 0 Then
                Contatore()
                myPhrase = New Phrase(LpDec(12) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
                doc.Add(myPhrase)
            End If
        Next
        GoTo RITORNO
RITORNO:
        If Comprimi = False Then
            Contatore()
            myPhrase = New Phrase(LpDec(CC) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
            doc.Add(myPhrase)
        End If
        GoTo INIZIO
    End Function
    Private Function RETTIFICA(ByVal Lpdec() As String, ByVal Rx0 As DataRow) As Boolean
        If Rx0.Item("RETTIF") = True Then
            Lpdec(2) = "Scritture di Rettifica e di Chiusura al " & Format(Rx0.Item("PriDataEst"), "dd/MM/yyyy")
            Lpdec(3) = Mid(Lpdec(2), 43, 38)
            AD = Mid(Lpdec(2), 40, 3).PadRight(3, " ")
            Lpdec(2) = Mid(Lpdec(2), 1, 39)
            STAMPARIGA(Lpdec)
        End If
    End Function
    Private Sub DesRiga(ByVal lpdec() As String, ByVal Fattura As Boolean, ByVal rs As DataRow)
        If Fattura = False Then
            lpdec(2) = Trim(Trim(Causale(rs.Item("PriCausale")) & " " & Format(rs.Item("PriDocEst"), "######")) _
              & " " & Trim(rs.Item("Pridesc")) & Trim(rs.Item("PridescB")))
            If Trim(rs.Item("PriNsRif")) > "" Then
                lpdec(2) = lpdec(2) & "[Doc. n. " & rs.Item("PriNsRif") & "]"
            End If
            lpdec(3) = Mid(lpdec(2), 43, 38)
            AD = Mid(lpdec(2), 40, 3).PadRight(3, " ")
            lpdec(2) = Mid(lpdec(2), 1, 39)
        Else
            lpdec(2) = Causale(rs.Item("PriCausale")) & " n. " & Format(rs.Item("PriDocEst"), "000000") _
              & " del " & Format(rs.Item("PriDataEst"), "dd/MM/yyyy")
            AD = "[Pr"
            lpdec(3) = "ot. n." & Format(rs.Item("PriNumProt"), "######") & "/" & Format(rs.Item("PriRegIva"), "00") & "]"
        End If
    End Sub
    Private Sub TotaleRiporto(ByVal Totale As Decimal)
        TotaleGenerale = TotaleGenerale + Totale
    End Sub
    Private Sub STAMPARIGA(ByVal Lpdec() As String)
        Dim z As Int16
        Contatore()
        LpLp(Lpdec)
        Lpdec(11) = ""
        For z = 0 To 5
            Lpdec(11) = Lpdec(11) & Lpdec(z)
            Lpdec(z) = ""
        Next
        myPhrase = New Phrase(Lpdec(11) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
        doc.Add(myPhrase)
    End Sub
    Private Sub LpLp(ByVal Lpdec() As String)
        Lpdec(0) = Lpdec(0).PadLeft(6, " ") & "|"
        Lpdec(1) = Lpdec(1).PadLeft(8, " ") & "|"
        Lpdec(2) = Lpdec(2).PadRight(39, " ") & AD
        Lpdec(3) = Lpdec(3).PadRight(38, " ") & "|"
        Lpdec(4) = Lpdec(4).PadLeft(16, " ") & "|"
        Lpdec(5) = Lpdec(5).PadLeft(16, " ")
    End Sub
    Private Function SezTestata() As Boolean
        Dim PRT(10), WT(10) As String
        Dim x, WP(1) As Int16
        Dim BlobPagine As Int16 = 30 ''' ogni 30 pagine DI stampa va in stampa
        For x = 0 To 10
            PRT(x) = ""
            WT(x) = ""
        Next
        WT(0) = "".PadLeft(23, " ") & "O  P  E  R  A  Z  I  O  N  E".PadRight(57, " ")
        WT(1) = "    Parziali    "
        WT(2) = "     Totali     "
        WT(3) = "".PadLeft(80, " ")
        WT(4) = "  R I P O R T O "
        WT(6) = "A  R I P O R T O"
        WT(5) = Format(TotaleGenerale, "#,###,###,##0.00").PadLeft(16, " ")
        PRT(0) = PRT(0).PadLeft(130, "-")
        PRT(1) = "Art.lo" & "|" & "  Data  " & "|" & WT(0) & "|" & WT(1) & "|" & WT(2)
        PRT(2) = "".PadRight(130, "-")
        PRT(3) = "      " & "|" & "        " & "|" & WT(3) & "|" & WT(4) & "|" & WT(5)
        PRT(4) = "      " & "|" & "        " & "|" & WT(3) & "|" & WT(6) & "|" & WT(5)
        Pagine = Pagine + 1
        If Pagine > 1 Then
            myPhrase = New Phrase(PRT(4) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
            doc.Add(myPhrase)
            doc.NewPage()
        End If

        If Pagine = 1 Then RigheVuote(1)

        If Intesta = False Then
            RigheVuote(6)
        Else
            Np = Np + 1
            PRT(5) = IT(1).PadRight(130, " ")
            PRT(6) = IT(2).PadRight(130, " ")
            PRT(7) = IT(3).PadRight(130, " ")
            PRT(8) = IT(0).PadRight(55, " ") & IT(6).PadRight(LL, " ") & "Pagina N. " & DalAl & " / " & Np.ToString.PadLeft(6, " ")
            myPhrase = New Phrase(PRT(0) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
            myPhrase.Add(New Phrase(PRT(5) & Microsoft.VisualBasic.Chr(10) & ""))
            myPhrase.Add(New Phrase(PRT(6) & Microsoft.VisualBasic.Chr(10) & ""))
            myPhrase.Add(New Phrase(PRT(7) & Microsoft.VisualBasic.Chr(10) & ""))
            myPhrase.Add(New Phrase(PRT(8) & Microsoft.VisualBasic.Chr(10) & ""))
            myPhrase.Add(New Phrase(PRT(0) & Microsoft.VisualBasic.Chr(10) & ""))
            doc.Add(myPhrase)
        End If
        myPhrase = New Phrase(PRT(1) & Microsoft.VisualBasic.Chr(10) & "", New Font(Font.COURIER, 6.5))
        myPhrase.Add(New Phrase(PRT(2) & Microsoft.VisualBasic.Chr(10) & ""))
        myPhrase.Add(New Phrase(PRT(3) & Microsoft.VisualBasic.Chr(10) & ""))
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
        cmd = New SqlCommand("SELECT * from TbCii", cnCo)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            Causale(dataRd.Item("CiiCod")) = dataRd.Item("CiiCau")
        End While
        dataRd.Close()
        Dim z As Int16
        For z = 0 To 6
            IT(z) = ""
        Next
        If AZI = "" Then Exit Function
        cmd = New SqlCommand("SELECT * from TbAna where anaGrp = 'AZ' and ANACOD = '" & AZI & "'", cnVd)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            IT(1) = dataRd.Item("AnaDesc")
            IT(2) = dataRd.Item("AnaIndirizzo")
            IT(3) = Trim(dataRd.Item("AnaCap")) & " " & Trim(dataRd.Item("AnaCitta")) & " " & Trim(dataRd.Item("AnaProv"))
            IT(0) = Trim(dataRd.Item("AnaPiva")) & " - " & Trim(dataRd.Item("AnaCFis"))
        End While
        dataRd.Close()
    End Function
End Module
