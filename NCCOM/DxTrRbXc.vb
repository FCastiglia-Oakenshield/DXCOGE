Imports DXBASE
Imports System.Data.SqlClient

Public Class DxTrRbXc
    Dim IDBLK, ProgId, MiglioFo As Int32
    Dim CAUSALE, M, NDIS As Int16
    Dim FinoAl, RbCpt, DDIS As String
    Dim Totale As Decimal
    Dim Cc As String = "EFF"
    Dim DsEff As DataSet
    Dim DaEff As SqlDataAdapter
    Dim RiW As DataRow
    Dim OkUpdate As Boolean
    Dim Articolo As Int32 = 0

    Dim OkFrascheri As Boolean
    Dim UserId As String

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
    Private Sub DxTrRbXc_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        'DateEdit1.EditValue = Today.ToShortDateString
        DateEdit1.EditValue = Today
    End Sub
    Sub Effetti()
        ''' FRASCHERI NON GESTISCE GLI EFFETTI IN XCOG 
        ''' LA LETTURA DEL TBTAI NON SAREBBE CORRETTA
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UserId = dataRd.Item("Sel14")
        End While
        dataRd.Close()
        If UserId = "FRASCHERI" Then Exit Sub
        OkUpdate = False
        Dim StrUno As String
        Dim K As Int32
        Dim Rb As Int16 = -1
        Dim Nd As Int16 = -1
        FinoAl = CDate(DateEdit1.EditValue).ToShortDateString
        StrUno = "Select * from TbEff where RicFlagCoge = 0 and RicSeff = 3 and Ricban > 0 and RicNumDis > 0 and RicDDis <= '" & FinoAl & "' and (RicTPag = 2 or RicTPag = 7) Order By  RicBan,RicNumDis,RicDFat,RicNFat"
        DsEff = New DataSet
        DaEff = New SqlDataAdapter(StrUno, cnCo)
        '    If UserId = "GSSPA" Then DaEff = New SqlDataAdapter(StrUno, CnCoSede)
        DaEff.Fill(DsEff, Cc)
        If DsEff.Tables(Cc).Rows.Count <= 0 Then Exit Sub
        Cmd = New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT top 1 TaiCauEFF FROM TbTai order by TaiAnno DESC ", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CAUSALE = dataRd.Item("TaiCauEFF")
        End While
        dataRd.Close()
        Cursor.Current = Cursors.WaitCursor
        RbCpt = "99.99"
        Totale = 0
        M = 0
        For K = 1 To DsEff.Tables(Cc).Rows.Count
            RiW = DsEff.Tables(Cc).Rows(K - 1)
            TextEdit1.Text = "FATTURA N. " & Format(RiW("RicNFat"), "000000") & " DEL " & CDate(RiW("RicDFat")).ToShortDateString
            '  Application.DoEvents()
            If Rb <> RiW("RicBan") Or Nd <> RiW("RicNumDis") Then
                If Totale <> 0 And Rb > 0 Then ChiudiEffetti()
                Rb = RiW("RicBan")
                Nd = RiW("RicNumDis")
                ContoCpt()
            End If
            If RiW("RicImpRata") <> 0 Then ScriviEffetti()
        Next
        If Totale <> 0 And Rb > 0 Then ChiudiEffetti()
    End Sub
    Sub ContoCpt()
        Dim StrReg As String = "SELECT * from TbBan where BanCod = " & RiW("RicBan")
        Dim Cmd As New SqlCommand(StrReg, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            RbCpt = dataRd.Item("BanRb")
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
    End Function
    Sub ResetIdP()
        Dim Elimina As String = "DELETE FROM TbIDP WHERE IdPRI = " & ProgId
        Dim Dmd As New SqlCommand(Elimina, cnCo)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub ChiudiEffetti()
        Wrd.Parameters.Remove(p3)
        Wrd.Parameters.Remove(p4)
        Wrd.Parameters.Remove(p9)
        Wrd.Parameters.Remove(p10)
        Wrd.Parameters.Remove(p11)
        Wrd.Parameters.Remove(p12)
        Wrd.Parameters.Remove(p14)
        Wrd.Parameters.Remove(p22)
        Wrd.Parameters.Remove(p27)
        p3.Value = RbCpt
        p4.Value = "00.10"
        p9.Value = Totale
        p10.Value = 0
        p11.Value = "DISTINTA EFFETTI"
        p12.Value = NDIS
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
    Sub ScriviEffetti()
        Dim UpWr As String
        If Totale = 0 Then
            LeggiUltimo(RiW("RicDDis"))
            Articolo = RileggoLocked()
            M = 1
            NDIS = RiW("RicNumDis")
            DDIS = CDate(RiW("RicDDis")).ToShortDateString
            SbloccoLocked()
        Else
            M = M + 1
        End If
        Wrd.Parameters.Clear()
        p1.Value = RiW("RicDDis")
        p2.Value = CAUSALE
        p3.Value = "00.10"
        p4.Value = RiW("RicClie")
        p5.Value = Articolo
        p6.Value = ""
        p7.Value = 0
        p8.Value = 0
        p9.Value = 0
        p10.Value = RiW("RicImpRata")
        p11.Value = "EFF.SCAD.IL " & CDate(RiW("RicDsca")).ToShortDateString
        p12.Value = RiW("RicNFat")
        p13.Value = "" 'RiW("PriMeseSk")
        p14.Value = RiW("RicDDis")
        p15.Value = "" 'RiW("PriDescB")
        p16.Value = 0 'RiW("PriFl04")
        p17.Value = 0 'RiW("PriFl05")
        p18.Value = 0 'RiW("PriFl06")
        p19.Value = "" 'RiW("PriNsRif")
        p20.Value = "" 'RiW("PriSos")
        p21.Value = "" 'RiW("PriLinea")
        p22.Value = CDate(RiW("RicDFat")).Year
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
        Totale = Totale + RiW("RicImpRata")
        UpWr = "Update TbEFF set RicFlagCoge = 1 where RicProg = " & RiW("RicProg")
        Dim Ulb As New SqlCommand(UpWr, cnCo)
        '     If UserId = "GSSPA" Then Ulb = New SqlCommand(UpWr, CnCoSede)
        Ulb.ExecuteNonQuery()
    End Sub
    Sub Partita(ByVal Tipo As Int16, ByVal AZ As Int32)
        If Tipo = 0 Then
            EsegueSql(" EXEC RiAprePartita  @Id = " & ProgId & ",@Az=" & AZ & ",@Miglio=" & MiglioFo, cnCo)
        Else
            EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
        End If
    End Sub
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
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        Effetti()
        Me.Close()
    End Sub
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F11 Then
            ButtonF1.PerformClick()
            Exit Sub
        End If
    End Sub
End Class