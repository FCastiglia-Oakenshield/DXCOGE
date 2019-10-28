Imports DXBASE
Imports System.Data.SqlClient
Public Class DxChiApe
    Dim UltimaApertura, Newapertura, sw, MaxEse, EseBilChi(5), CauChiusura, CauApertura, Causale, Zero, FLAGC As Int16
    Dim IdBlk, ProgId, MiglioFo As Int32
    Dim str, descau, EseCE(2), EseUT(2), EsePE(2), EseCH(2), EseAp(2), RilevaUtile, RilevaPeriodo, RilevaDoc, Grp(18), CONTO As String
    Dim OkFlash As Boolean
    Dim EseDal(2), EseAl(2) As Date
    Dim EseProg(2), EseSppp(2), EseClFo(2), EseSdo(2) As Boolean
    Dim DsCha As DataTable
    Dim DaCha As SqlDataAdapter
    Dim RwPno As DataRow
    Dim UTILE As Decimal
    Dim Rispondi As MsgBoxResult
    Dim UltimoSaldo As Decimal
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem

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


    Private Sub DxChiApe_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulisci()
        GRUPPI()
        Chiusura()
        Apertura()
        If Controlli() = False Then
            Me.Close()
            Exit Sub
        End If
    End Sub

    Sub Chiusura()
        FLAGC = 0
        'cerco anno più alto con apertura
        Dim Cmd As New SqlCommand("SELECT ISNULL(max(YEAR(pridatagio)),0) FROM Tbpri Where PriCausale = 45", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UltimaApertura = dataRd.Item(0)
            FLAGC = 1
        End While
        dataRd.Close()
        'LEGGO PARAMETRI SU AZIENDA PER EFFETTUARE LA CHIUSURA
        Cmd = New SqlCommand("SELECT * from TbEse WHERE EseAnno = " & UltimaApertura, cnCo)
        Dim x As Int16
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            x = 1
            TextEdit101.EditValue = dataRd.Item("EseAnno")
            EseDal(x) = dataRd.Item("EseDal")
            EseAl(x) = dataRd.Item("EseAl")
            CauChiusura = dataRd.Item("EseCausaleChiusuraConti")
            DateEdit1.EditValue = EseAl(x)
            DateEdit2.EditValue = EseAl(x)
            DateEdit1.Properties.MaxValue = EseAl(x).AddMinutes(1)
            DateEdit1.Properties.MinValue = EseDal(x)
            DateEdit2.Properties.MaxValue = EseAl(x).AddYears(1).AddMinutes(1)
            DateEdit2.Properties.MinValue = EseDal(x)
            DateEdit101.Properties.MaxValue = EseDal(x).AddMinutes(1)
            DateEdit101.Properties.MinValue = EseDal(x)
            DateEdit101.EditValue = EseDal(x)
            DateEdit102.Properties.MaxValue = EseAl(x).AddMinutes(1)
            DateEdit102.Properties.MinValue = EseAl(x)
            DateEdit102.EditValue = EseAl(x)
            EseCE(x) = dataRd.Item("EsePP")
            EseUT(x) = dataRd.Item("EseUti")
            EsePE(x) = dataRd.Item("EsePer")
            EseCH(x) = dataRd.Item("EseBilChi")
            EseAp(x) = dataRd.Item("EseBilAp")
        End While
        dataRd.Close()
        REM lettura causali
        Dim SS As String = ""
        ImageComboBoxEdit1.Properties.Items.Clear()
        ImageComboBoxEdit2.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT * from TbCii order by CiiCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("CiiCod") > 3 Then
                SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau")
                nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
                ImageComboBoxEdit1.Properties.Items.Add(nn)
                ImageComboBoxEdit2.Properties.Items.Add(nn)
            End If
        End While
        dataRd.Close()
        'aggiungere verifica che non esista chiusura
        Cmd = New SqlCommand("SELECT * FROM Tbpri Where PriCausale = " & CauChiusura, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            FLAGC = 10
        End While
        dataRd.Close()
        ImageComboBoxEdit1.EditValue = CauChiusura
        DateEdit2.Focus()
    End Sub
    Sub Apertura()
        Dim Cmd As New SqlCommand("SELECT ISNULL(max(YEAR(pridatagio)),0) FROM Tbpri Where PriCausale = 45", cnCo)
        FLAGC = 15
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UltimaApertura = dataRd.Item(0)
        End While
        dataRd.Close()
        Newapertura = UltimaApertura + 1
        Cmd = New SqlCommand("SELECT * from TbEse WHERE EseAnno = " & Newapertura, cnCo)
        Dim x As Int16
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            FLAGC = 3
            x = 1
            TextEdit102.EditValue = dataRd.Item("EseAnno")
            EseDal(x) = dataRd.Item("EseDal")
            EseAl(x) = dataRd.Item("EseAl")
            CauApertura = 45
            DateEdit103.Properties.MaxValue = EseDal(x).AddMinutes(1)
            DateEdit103.Properties.MinValue = EseDal(x)
            DateEdit103.Text = EseDal(x)
            DateEdit104.Properties.MaxValue = EseAl(x).AddMinutes(1)
            DateEdit104.Properties.MinValue = EseAl(x)
            DateEdit104.Text = EseAl(x)
            DateEdit3.EditValue = EseDal(x)
            DateEdit4.EditValue = EseDal(x)
            DateEdit3.Properties.MaxValue = EseAl(x).AddMinutes(1)
            DateEdit3.Properties.MinValue = EseDal(x)
            DateEdit4.Properties.MaxValue = EseAl(x).AddMinutes(1)
            DateEdit4.Properties.MinValue = EseDal(x)
            EseCE(x) = dataRd.Item("EsePP")
            EseUT(x) = dataRd.Item("EseUti")
            EsePE(x) = dataRd.Item("EsePer")
            EseCH(x) = dataRd.Item("EseBilChi")
            EseAp(x) = dataRd.Item("EseBilAp")
        End While
        dataRd.Close()
        ImageComboBoxEdit2.EditValue = CauApertura
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
    Private Sub Pulisci()
        TextEdit3.EditValue = ""
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = ""
        TextEdit6.EditValue = ""
        TextEdit9.EditValue = ""
        TextEdit10.EditValue = ""
        DateEdit1.EditValue = Today
        DateEdit2.EditValue = Today
        DateEdit3.EditValue = Today
        DateEdit4.EditValue = Today
    End Sub
    Sub Vedo()
        ButtonF11.Enabled = Not LabelControl14.Visible
        ButtonF5.Enabled = Not LabelControl14.Visible
        GroupControl2.Enabled = Not LabelControl14.Visible
        GroupControl5.Enabled = Not LabelControl14.Visible
    End Sub

    Function EsegueProcedura(ByVal QualeP As String, ByVal Dal As String, ByVal Al As String, ByVal IdBlk As Int32, ByVal Caus As Int16, ByVal Switch As Int16, ByVal CauChi As Int16) As Boolean
        EsegueSql(" EXEC " & QualeP & "  @DAL ='" & Dal & "', @AL = '" & Al & "' , @BLOCK = " & IdBlk & " , @CAUS= " & Caus & " , @SW= " & Switch & ", @CAUCH= " & CauChi, cnCo)
    End Function

    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
    End Sub
    Sub Esercizi(ByVal x As Int16)
        DateEdit101.Properties.MaxValue = CDate("31/12/2050") ' reset campi per ricalcolare limiti
        DateEdit101.Properties.MinValue = CDate("01/01/1900")

        DateEdit102.Properties.MaxValue = CDate("31/12/2050")
        DateEdit102.Properties.MinValue = CDate("01/01/1900")

        DateEdit101.Properties.MaxValue = EseDal(x).AddMinutes(1)
        DateEdit101.Properties.MinValue = EseDal(x)
        DateEdit101.Text = EseDal(x)

        DateEdit102.Properties.MaxValue = EseAl(x).AddMinutes(1)
        DateEdit102.Properties.MinValue = EseAl(x)
        DateEdit102.Text = EseAl(x)

        CauChiusura = EseBilChi(x)
        ImageComboBoxEdit1.EditValue = CauChiusura
        DateEdit1.EditValue = EseAl(x)
        DateEdit2.EditValue = EseAl(x)
    End Sub
  
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Controlli() = False Then Exit Sub
        TextEdit11.Visible = True
        CREACHIUSURA()
        CREAAPERTURA()
        TextEdit11.Visible = False
        LabelControl14.Visible = True
        Vedo()
        TextEdit103.Focus()
    End Sub

    Sub CREAAPERTURA()
        Causale = 45 : Zero = 1
        Dim da1, da2 As String
        da1 = DateEdit1.EditValue
        da2 = DateEdit2.EditValue
        DateEdit1.EditValue = DateEdit3.EditValue
        DateEdit2.EditValue = DateEdit4.EditValue
        str = "select * from VChiusura where TmsBlock = " & IdBlk
        DaCha = New SqlDataAdapter(str, cnCo)
        DsCha = New DataTable("Chi")
        DaCha.Fill(DsCha)
        Dim K, P As Int16
        ''' registro APERTURA
        EseCE(2) = EseAp(1)
        RilevaUtile = Trim(TextEdit9.EditValue)
        RilevaPeriodo = ""
        RilevaDoc = Trim(TextEdit10.EditValue)
        For K = 1 To DsCha.Rows.Count
            Wmd.Parameters.Clear()
            RwPno = DsCha.Rows(K - 1)
            If RwPno("TmsFlag") = 6 Or RwPno("TmsFlag") = 7 Then GoTo IINext
            For P = 1 To Val(Grp(0))
                If RwPno("TMSCONTO") = Grp(P) Then GoTo IINext
            Next
            CONTO = RwPno("TMSCONTO")
            RwPno("SALDO") = RwPno("SALDO") * -1
            RegistraMovimenti()
IINext:
        Next
        str = "select * from VCHIUCLFO where TmCBlock = " & IdBlk
        DaCha = New SqlDataAdapter(str, cnCo)
        DsCha = New DataTable("Chi")
        DaCha.Fill(DsCha)
        For K = 1 To DsCha.Rows.Count
            Wmd.Parameters.Clear()
            RwPno = DsCha.Rows(K - 1)
            CONTO = RwPno("TMCCONTO")
            RwPno("SALDO") = RwPno("SALDO") * -1
            RegistraMovimenti()
        Next
        If UTILE < 0 Then
            EseCE(2) = EseAp(1)
            CONTO = EseUT(1)
            RwPno("SALDO") = UTILE * -1
        Else
            EseCE(2) = EsePE(1)
            CONTO = EseAp(1)
            RwPno("SALDO") = UTILE
        End If
        '''RwPno("SALDO") = UTILE ''* -1
        RegistraMovimenti()
        DateEdit1.EditValue = da1
        DateEdit2.EditValue = da2
    End Sub
    Sub CREACHIUSURA()
        CaricaSaldi()
        Causale = Val(ImageComboBoxEdit1.EditValue)
        UTILE = 0 : Zero = 0
        Dim str As String = "select SUM(SALDO) as UTILE from VCHIUSURA WHERE TMSFLAG > 5 AND TMSFLAG < 8 AND TmsBlock = " & IdBlk
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UTILE = dataRd.Item(0)
        End While
        dataRd.Close()
        TextEdit103.EditValue = UTILE
        str = "select * from VChiusura where TmsBlock = " & IdBlk
        DaCha = New SqlDataAdapter(str, cnCo)
        DsCha = New DataTable("Chi")
        DaCha.Fill(DsCha)
        Dim K, P As Int16
        ''' registro conto economico
        EseCE(2) = EseCE(1)
        RilevaUtile = Trim(TextEdit5.EditValue)
        RilevaPeriodo = ""
        RilevaDoc = Trim(TextEdit6.EditValue)
        For K = 1 To DsCha.Rows.Count
            Wmd.Parameters.Clear()
            RwPno = DsCha.Rows(K - 1)
            If RwPno("TmsFlag") <> 6 And RwPno("TmsFlag") <> 7 Then GoTo INext
            CONTO = RwPno("TMSCONTO")
            RegistraMovimenti()
Inext:
        Next
        RilevaPeriodo = "izio " & CDate(DateEdit101.EditValue).ToShortDateString & " - " & CDate(DateEdit102.EditValue).ToShortDateString
        If UTILE < 0 Then
            RilevaUtile = "Rilevato UTILE   d'eserc"
            CONTO = EseUT(1)
        Else
            RilevaUtile = "Rilevato PERDITA d'eserc"
            CONTO = EsePE(1)
        End If
        UltimoSaldo = RwPno("SALDO")
        RwPno("SALDO") = UTILE * -1
        RegistraMovimenti()
        RwPno("SALDO") = UltimoSaldo
        EseCE(2) = EseCH(1)
        RilevaUtile = Trim(TextEdit3.EditValue)
        RilevaPeriodo = ""
        RilevaDoc = Trim(TextEdit4.EditValue)
        For K = 1 To DsCha.Rows.Count
            Wmd.Parameters.Clear()
            RwPno = DsCha.Rows(K - 1)
            If RwPno("TmsFlag") = 6 Or RwPno("TmsFlag") = 7 Then GoTo IINext
            For P = 1 To Val(Grp(0))
                If RwPno("TMSCONTO") = Grp(P) Then GoTo IINext
            Next
            CONTO = RwPno("TMSCONTO")
            RegistraMovimenti()
IINext:
        Next
        str = "select * from VCHIUCLFO where TmCBlock = " & IdBlk
        DaCha = New SqlDataAdapter(str, cnCo)
        DsCha = New DataTable("Chi")
        DaCha.Fill(DsCha)
        EseCE(2) = EseCH(1)
        For K = 1 To DsCha.Rows.Count
            Wmd.Parameters.Clear()
            RwPno = DsCha.Rows(K - 1)
            CONTO = RwPno("TMCCONTO")
            RegistraMovimenti()
        Next
        If UTILE < 0 Then
            EseCE(2) = EseUT(1)
            CONTO = EseCH(1)
            RwPno("SALDO") = UTILE * -1
        Else
            EseCE(2) = EseCH(1)
            CONTO = EsePE(1)
            RwPno("SALDO") = UTILE
        End If
        ''RwPno("SALDO") = UTILE ''* -1
        RegistraMovimenti()
    End Sub
    Sub RegistraMovimenti()
        Dim Articolo As Int32 = RileggoLocked()
        TextEdit11.EditValue = Articolo
        TextEdit11.Refresh()
        Application.DoEvents()
        LeggiUltimo(DateEdit2.EditValue)
        Wmd.Parameters.Clear()
        p1.Value = CDate(DateEdit2.EditValue)
        p2.Value = Causale
        If RwPno("SALDO") > 0 Then
            p3.Value = EseCE(2)
            p4.Value = CONTO
        Else
            p3.Value = CONTO
            p4.Value = EseCE(2)
        End If
        p5.Value = Articolo
        p6.Value = ""
        p7.Value = 0
        p8.Value = 0
        p9.Value = Math.Abs(RwPno("SALDO"))
        p10.Value = Math.Abs(RwPno("SALDO"))
        p11.Value = RilevaUtile
        p12.Value = 99 * 10000 + (CDate(DateEdit1.EditValue).Year - Zero)
        p13.Value = ""
        p14.Value = CDate(DateEdit1.EditValue)
        p15.Value = RilevaPeriodo
        p16.Value = 0
        p17.Value = 0
        p18.Value = 0
        p19.Value = RilevaDoc
        p20.Value = ""
        p21.Value = ""
        p22.Value = CDate(DateEdit1.EditValue).Year - Zero
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
        If Causale = 45 Then EsegueSql(" EXEC RiChiudePartita  @Id = " & ProgId & ",@Miglio=" & MiglioFo, cnCo)
    End Sub
    Sub CaricaSaldi()
        If OkFlash = False Then
            OkFlash = True
            IdBlk = semaforo("Bilancio AL " & Today.Date)
        Else
            PuliziaFlash()
        End If
        Dim d1, d2 As String
        d1 = CDate(DateEdit101.EditValue).ToShortDateString
        d2 = CDate(DateEdit102.EditValue).ToShortDateString
        EsegueProcedura("XF1", d1, d2, IdBlk, 0, 0, 0)
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
    Sub PuliziaFlash()
        Dim Ultimo As New SqlCommand("DELETE FROM TMPBILC WHERE TMCBLOCK = " & IdBlk, cnCo)
        Ultimo.ExecuteNonQuery()
        Ultimo = New SqlCommand("DELETE FROM TMPBILS WHERE TMSBLOCK = " & IdBlk, cnCo)
        Ultimo.ExecuteNonQuery()
    End Sub

    Private Sub ChiApe_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        PuliziaFlash()
    End Sub
    Function Controlli() As Boolean
        Controlli = True
        If FLAGC = 0 Then
            Messaggio(1, "NON ESISTE UN ANNO VALIDO PER GENERAZIONE CHIUSURA  !!!")
            Controlli = False
        End If
        If FLAGC = 10 Then
            Messaggio(1, "CHIUSURA GIA' EFFETTUATA !!!")
            Controlli = False
        End If
        If FLAGC = 15 Then
            Messaggio(1, "MANCA ANNO PER APERTURA!!!")
            Controlli = False
        End If
        If CDate(DateEdit1.EditValue) > CDate(DateEdit2.EditValue) Then
            Messaggio(1, "CHIUSURA - DATA OPERAZIONE SUPERIORE A DATA GIORNALE !!!")
            Controlli = False
        End If
        If CDate(DateEdit3.EditValue) > CDate(DateEdit4.EditValue) Then
            Messaggio(1, "APERTURA - DATA OPERAZIONE SUPERIORE A DATA GIORNALE !!!")
            Controlli = False
        End If
    End Function
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim msg(1) As String
        Dim title As String
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        title = "CHIUSURA - APERTURA"
        Rispondi = MsgBox(Mexage, style(Tipo), title)
    End Sub

    Private Sub ButtonF5_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF5.LostFocus
        If LabelControl14.Visible = True Then
            Me.Close()
        End If
    End Sub

    Private Sub TextEdit103_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextEdit103.KeyPress
        If LabelControl14.Visible = True Then
            Me.Close()
        End If
    End Sub
End Class