Imports DXBASE
Imports System.Data.SqlClient
Public Module GiroCRPn
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
    Dim ProId As Int32 = -1
    Dim MiglioFo As Int32
    Dim DaFce As SqlDataAdapter
    Dim DsFce As DataTable
    Dim RwFce As DataRow
  
    Function GiroRivalsaECee(ByVal Id As Int32, ByVal CFor As String, ByVal CDes As String, ByVal ArtGcRCee As Int32, ByVal Migliaia As Int32) As Boolean
        MiglioFo = Migliaia
        Dim Oggi As Date = CDate(Today).ToShortDateString
        Dim Articolo As Int32 = -1
        Dim Scheggia As Int16 = 1
        Dim P As Int16 = -1
        Dim X As Int16 = 0
        ProId = -1
        Dim StrReg As String = "SELECT * from TbPri where PriId = " & Id & " Order By PriProg"
        DsFce = New DataTable
        DaFce = New SqlDataAdapter(StrReg, cnCo)
        DaFce.Fill(DsFce)
        For P = 1 To DsFce.Rows.Count
            If ProId = -1 Then LeggiUltimo(CDate(Today).ToShortDateString)
            If Scheggia = 1 Then Articolo = RileggoLocked()
            RwFce = DsFce.Rows(P - 1)
            If RwFce("PriCausale") = 1 Then
                p3.Value = RwFce("PriCoAvere")
                p4.Value = "00.10"
                p9.Value = RwFce("PriImpDare")
                p10.Value = 0
                p11.Value = ""
                p20.Value = RwFce("PriCoDare")
                Aggiungi(X, ArtGcRCee, Articolo, Scheggia)
            End If
            If RwFce("PriCausale") = 3 Then
                p3.Value = CFor
                p4.Value = "00.10"
                p9.Value = RwFce("PriImpAvere")
                p10.Value = 0
                p11.Value = ""
                p20.Value = RwFce("PriCoDare")
                Aggiungi(X, ArtGcRCee, Articolo, Scheggia)
            End If
        Next
        p3.Value = "00.10"
        p4.Value = RwFce("PriCoDare")
        p9.Value = 0
        p10.Value = RwFce("PriImpDare")
        p11.Value = Mid(CDes, 1, 24)
        p20.Value = ""
        Aggiungi(X, ArtGcRCee, Articolo, Scheggia)
        REM
        EsegueSql(" EXEC InitPrk  @ID = " & ProId, cnCo)
        ResetIdP()
        Partita(1, 0)
    End Function
    Sub Aggiungi(ByRef X As Int16, ByVal ArtGcRCee As Int16, ByVal Articolo As Int32, ByRef Scheggia As Int16)
        p1.Value = RwFce("PriDataGio")
        p2.Value = ArtGcRCee
        p5.Value = Articolo
        p6.Value = ""
        p7.Value = 0
        p8.Value = 0
        p12.Value = RwFce("PriDocEst")
        p13.Value = ""
        p14.Value = RwFce("PriDataGio")
        p15.Value = ""
        p16.Value = 0
        p17.Value = 0
        p18.Value = 0
        p19.Value = ""
        p21.Value = ""
        p22.Value = CDate(RwFce("PriDataEst")).Year
        p23.Value = 0
        p24.Value = 0
        p25.Value = 0
        p26.Value = ProId
        X += 1
        p27.Value = X
        p28.Value = 0
        p29.Value = 0
        Wmd.Parameters.Clear()
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
    End Sub
    Sub Partita(ByVal Tipo As Int16, ByVal AZ As Int32)
        If Tipo = 0 Then
            EsegueSql(" EXEC RiAprePartita  @Id = " & ProId & ",@Az=" & AZ & ",@Miglio=" & MiglioFo, cnCo)
        Else
            EsegueSql(" EXEC RiChiudePartita  @Id = " & ProId & ",@Miglio=" & MiglioFo, cnCo)
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
        ProId = UltimaRiga.ExecuteScalar
    End Function
    Sub ResetIdP()
        Dim Elimina As String = "DELETE FROM TbIDP WHERE IdPRI = " & ProId
        Dim Dmd As New SqlCommand(Elimina, cnCo)
        Dmd.ExecuteNonQuery()
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
End Module
