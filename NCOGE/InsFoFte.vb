Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO

Public Class InsFoFte
    Private Shared FO_PARTITAIVA, FO_CODICEFISCALE, FO_DENOMINAZIONE, FO_COGNOME, FO_NOME, FO_INDIRIZZO, FO_CAP, FO_COMUNE, FO_PROVINCIA, FO_TELEFONO, FO_FAX, FO_EMAIL, FO_IBAN, FO_PAESE As String

    Public Shared Property NPARTITAIVA As String
        Get
            Return FO_PARTITAIVA
        End Get
        Set(ByVal Value As String)
            FO_PARTITAIVA = Value
        End Set
    End Property

    Public Shared Property NCODICEFISCALE As String
        Get
            Return FO_CODICEFISCALE
        End Get
        Set(ByVal Value As String)
            FO_CODICEFISCALE = Value
        End Set
    End Property
    Public Shared Property NDENOMINAZIONE As String
        Get
            Return FO_DENOMINAZIONE
        End Get
        Set(ByVal Value As String)
            FO_DENOMINAZIONE = Value
        End Set
    End Property
    Public Shared Property NCOGNOME As String
        Get
            Return FO_COGNOME
        End Get
        Set(ByVal Value As String)
            FO_COGNOME = Value
        End Set
    End Property
    Public Shared Property NNOME As String
        Get
            Return FO_NOME
        End Get
        Set(ByVal Value As String)
            FO_NOME = Value
        End Set
    End Property
    Public Shared Property NINDIRIZZO As String
        Get
            Return FO_INDIRIZZO
        End Get
        Set(ByVal Value As String)
            FO_INDIRIZZO = Value
        End Set
    End Property
    Public Shared Property NCAP As String
        Get
            Return FO_CAP
        End Get
        Set(ByVal Value As String)
            FO_CAP = Value
        End Set
    End Property
    Public Shared Property NCOMUNE As String
        Get
            Return FO_COMUNE
        End Get
        Set(ByVal Value As String)
            FO_COMUNE = Value
        End Set
    End Property
    Public Shared Property NPROVINCIA As String
        Get
            Return FO_PROVINCIA
        End Get
        Set(ByVal Value As String)
            FO_PROVINCIA = Value
        End Set
    End Property
    Public Shared Property NTELEFONO As String
        Get
            Return FO_TELEFONO
        End Get
        Set(ByVal Value As String)
            FO_TELEFONO = Value
        End Set
    End Property
    Public Shared Property NFAX As String
        Get
            Return FO_FAX
        End Get
        Set(ByVal Value As String)
            FO_FAX = Value
        End Set
    End Property
    Public Shared Property NEMAIL As String
        Get
            Return FO_EMAIL
        End Get
        Set(ByVal Value As String)
            FO_EMAIL = Value
        End Set
    End Property
    Public Shared Property NIBAN As String
        Get
            Return FO_IBAN
        End Get
        Set(ByVal Value As String)
            FO_IBAN = Value
        End Set
    End Property
    Public Shared Property NPAESE As String
        Get
            Return FO_PAESE
        End Get
        Set(ByVal Value As String)
            FO_PAESE = Value
        End Set
    End Property
    Structure gruppiclfo
        Dim CliMax As Integer
        Dim al() As Integer
        Dim ult() As Integer
    End Structure
    Dim gruppi As New gruppiclfo
    Dim MIGLIO As Integer = -1
    Dim PagCod As Int16 = 0
    Dim Ultimo As String = ""
    Private Sub AnagFor_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        LeggiTabelle()
        cargo_ultimi()
        CaricaDati()
        TextEdit00.Focus()
    End Sub
    Sub LeggiTabelle()
        ComboBoxEdit4.Properties.Items.Clear()
        ComboBoxEdit4.Properties.Items.Add("")
        Cmd = New SqlCommand(" SELECT * from TbBan Order by BanCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit4.Properties.Items.Add(dataRd.Item("Bancod").ToString.PadLeft(3, " ") & " - " & dataRd.Item("BanDes"))
        End While
        dataRd.Close()
    End Sub
    Private Sub cargo_ultimi()
        gruppi = leggi_ultimi("FO")
        BarStaticItem1.Caption = gruppi.ult(0).ToString("00000;#;#")
        BarStaticItem2.Caption = gruppi.ult(1).ToString("00000;#;#")
        BarStaticItem3.Caption = gruppi.ult(2).ToString("00000;#;#")
        BarStaticItem4.Caption = gruppi.ult(3).ToString("00000;#;#")
        BarStaticItem5.Caption = gruppi.ult(4).ToString("00000;#;#")
        BarStaticItem6.Caption = gruppi.ult(5).ToString("00000;#;#")
        BarStaticItem7.Caption = gruppi.ult(6).ToString("00000;#;#")
        BarStaticItem8.Caption = gruppi.ult(7).ToString("00000;#;#")
        BarStaticItem9.Caption = gruppi.ult(8).ToString("00000;#;#")
    End Sub
    Private Function leggi_ultimi(ByVal CLFO As String) As gruppiclfo
        Dim strselect As String = "select * from TbGrp where GrpCod = '" & CLFO & "'"
        Cmd = New SqlCommand(strselect, cnCo)
        dataRd = Cmd.ExecuteReader
        ReDim leggi_ultimi.ult(8)
        ReDim leggi_ultimi.al(8)
        If dataRd.Read Then
            leggi_ultimi.CliMax = dataRd.GetInt32(2)
            leggi_ultimi.al(0) = dataRd.GetInt32(3)
            leggi_ultimi.al(1) = dataRd.GetInt32(4)
            leggi_ultimi.al(2) = dataRd.GetInt32(5)
            leggi_ultimi.al(3) = dataRd.GetInt32(6)
            leggi_ultimi.al(4) = dataRd.GetInt32(7)
            leggi_ultimi.al(5) = dataRd.GetInt32(8)
            leggi_ultimi.al(6) = dataRd.GetInt32(9)
            leggi_ultimi.al(7) = dataRd.GetInt32(10)
            leggi_ultimi.al(8) = dataRd.GetInt32(11)
            leggi_ultimi.ult(0) = dataRd.GetInt32(12)
            leggi_ultimi.ult(1) = dataRd.GetInt32(13)
            leggi_ultimi.ult(2) = dataRd.GetInt32(14)
            leggi_ultimi.ult(3) = dataRd.GetInt32(15)
            leggi_ultimi.ult(4) = dataRd.GetInt32(16)
            leggi_ultimi.ult(5) = dataRd.GetInt32(17)
            leggi_ultimi.ult(6) = dataRd.GetInt32(18)
            leggi_ultimi.ult(7) = dataRd.GetInt32(19)
            leggi_ultimi.ult(8) = dataRd.GetInt32(20)
        End If
        dataRd.Close()
        MIGLIO = leggi_ultimi.CliMax
    End Function
    Sub CaricaDati()
        Dim CGNO As String = NCOGNOME & " " & NNOME
        If NCOGNOME = "" Then
            TextEdit1.Text = Mid(NDENOMINAZIONE, 1, 28)
            TextEdit2.Text = Mid(NDENOMINAZIONE, 29, 28)
        Else
            TextEdit1.Text = Mid(CGNO, 1, 28)
            TextEdit2.Text = Mid(CGNO, 29, 28)
        End If
        If FO_PAESE <> "IT" Then
            TextEdit3.Text = ""
            TextEdit9.Text = FO_PAESE & NPARTITAIVA
        Else
            TextEdit3.Text = NPARTITAIVA
            TextEdit9.Text = ""
        End If

        TextEdit4.Text = Mid(NINDIRIZZO, 1, 50)
        TextEdit5.Text = NCODICEFISCALE
        TextEdit6.Text = NCAP
        TextEdit7.Text = Mid(NCOMUNE, 1, 50)
        TextEdit8.Text = NPROVINCIA
        TextEdit10.Text = Mid(NTELEFONO, 1, 20)
        TextEdit13.Text = Mid(NFAX, 1, 20)
        TextEdit15.Text = Mid(NEMAIL, 1, 50)
        TextEdit23.Text = Mid(NIBAN, 1, 2)
        TextEdit22.Text = Mid(NIBAN, 3, 2)
        TextEdit21.Text = Mid(NIBAN, 5, 1)
        TextEdit20.Text = Mid(NIBAN, 6, 5)
        TextEdit19.Text = Mid(NIBAN, 11, 5)
        TextEdit18.Text = Mid(NIBAN, 16, 12)
        TextEdit24.Text = ""
        If Val(TextEdit20.Text) > 0 And Val(TextEdit19.Text) > 0 Then
            TextEdit24.Text = LeggiAbiCab(Val(TextEdit20.Text), Val(TextEdit19.Text))
        End If
        If ComboBoxEdit4.Properties.Items.Count = 2 Then ComboBoxEdit4.SelectedIndex = 1 Else ComboBoxEdit4.SelectedIndex = 0
        CheckEdit1.Checked = False
    End Sub
    Private Function LeggiAbiCab(ByVal Abi As Int32, ByVal Cab As Int32) As String
        LeggiAbiCab = ""
        Cmd = New SqlCommand("select * from TbCab where CaAbi = " & Abi & " and CaCab =" & Cab, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            LeggiAbiCab = dataRd.Item("CaDescFt")
        End If
        dataRd.Close()
    End Function
    Sub HyperLinkEdit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GroupControl20.Click
        If Val(TextEdit17.Text) > 0 Then PagCod = Val(TextEdit17.Text)
        PagCod = Ricerche.LnkCodPag()
        If PagCod = 0 Then PagCod = Val(TextEdit17.Text)
        RilevaPagamento()
    End Sub
    Sub RilevaPagamento()
        MemoEdit2.Text = LeggiPagam(PagCod)
        If PagCod > 0 Then
            TextEdit17.Text = PagCod
            If MemoEdit2.Text.Length = 0 Then
                SelectNextControl(TextEdit17, True, True, True, True)
            Else
                SelectNextControl(TextEdit23, True, True, True, True)
            End If
        End If
    End Sub
    Private Function LeggiPagam(ByVal codice As Int16) As String
        LeggiPagam = ""
        Cmd = New SqlCommand("select * from TbPag where PagCod = " & codice, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            LeggiPagam = dataRd.Item("PagDesc")
        End If
        dataRd.Close()
    End Function
    Private Sub TbLeggiCod_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggiCod.Enter
        Dim DupA As Boolean = False
        Dim DupF As Boolean = False
        If Val(TextEdit00.Text) > gruppi.CliMax And Val(TextEdit00.Text) < 99999 Then
            DupF = leggiFor(TextEdit00.Text)
            DupA = leggiAna(TextEdit00.Text)
            If DupF = True Or DupA = True Then
                MsgBox("CODICE :" & TextEdit00.Text, MsgBoxStyle.Critical, "FORNITORE ESISTENTE")
                TextEdit00.Focus()
                Exit Sub
            End If
            ButtonF11.Focus()
            Ultimo = TextEdit00.EditValue
        End If
    End Sub
    Private Function leggiFor(ByVal codice As String) As Boolean
        Cmd = New SqlCommand("select FoCod from TbFor where FoCod ='" & codice & "'", cnDb)
        If Cmd.ExecuteScalar() > "00000" Then Return True Else Return False
    End Function
    Private Function leggiAna(ByVal codice As String) As Boolean
        Cmd = New SqlCommand("select AnaCod from TbAna where AnaGrp = 'FO' and AnaCod ='" & codice & "'", cnVd)
        If Cmd.ExecuteScalar() > "00000" Then Return True Else Return False
    End Function
    Private Sub ButtonF11_Click(sender As Object, e As System.EventArgs) Handles ButtonF11.Click
        If Val(TextEdit00.Text) <= MIGLIO Then
            TextEdit00.Focus()
            Return
        End If
        If ControllaCampi() = False Then Exit Sub
        registra()
        Me.Close()
    End Sub

    Private Function ControllaCampi() As Boolean
        Dim Mail As String = ""
        Dim cc As New Control
        If TextEdit1.Text = "" Then
            Mail = "<>MANCA RAGIONE SOCIALE " & Chr(13)
            cc = TextEdit1
        End If

        If Mail > "" Then
            MoltoCritico(Mail, "INSERIMENTO FORNITORI")
            cc.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
    Sub MoltoCritico(ByVal Mail As String, ByVal contesto As String)
        MessageBox.Show(Mail, contesto, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub
    Private Sub registra()
        ScriviAna()
        ScriviFor()
        scrivi_ultimi("FO")
    End Sub
    Private Sub ScriviAna()
        Dim scrivi As String = ""
        scrivi = "INSERT INTO TbAna (AnaDesc,AnaPiva,AnaCfis,AnaIndirizzo,AnaCap,AnaCitta,AnaProv,Anatel1,Anatel2,AnaTel3,AnaWWW,AnaEmail,AnaGrp,AnaCod,AnaResp,AnaNote,AnaFax,AnaRag1,AnaRag2,AnaPivaEst,AnaNoRubrica)  values(@AnaDesc,@AnaPiva,@AnaCfis,@AnaIndirizzo,@AnaCap,@AnaCitta,@AnaProv,@Anatel1,@Anatel2,@AnaTel3,@AnaWWW,@AnaEmail,'FO',@AnaCod,@AnaResp,'',@AnaFax,@AnaRag1,@AnaRag2,@AnaPivaEst,@AnaNoRubrica)"

        Cmd = New SqlCommand(scrivi, cnVd)

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
        Dim p12 As New SqlParameter("@AnaTel1", SqlDbType.VarChar)
        Dim p13 As New SqlParameter("@AnaTel2", SqlDbType.VarChar)
        Dim p14 As New SqlParameter("@AnaTel3", SqlDbType.VarChar)
        Dim p15 As New SqlParameter("@AnaFax", SqlDbType.VarChar)
        Dim p16 As New SqlParameter("@AnaWWW", SqlDbType.VarChar)
        Dim p17 As New SqlParameter("@AnaEmail", SqlDbType.VarChar)
        Dim p18 As New SqlParameter("@AnaResp", SqlDbType.VarChar)
        Dim p19 As New SqlParameter("@AnaNoRubrica", SqlDbType.Bit)


        p1.Value = TextEdit1.Text & " " & TextEdit2.Text
        p9.Value = TextEdit1.Text
        p10.Value = TextEdit2.Text
        p2.Value = TextEdit3.Text
        p3.Value = TextEdit5.Text
        p4.Value = TextEdit4.Text
        p5.Value = TextEdit6.Text
        p6.Value = TextEdit7.Text
        p7.Value = TextEdit8.Text
        p8.Value = TextEdit00.Text
        p11.Value = TextEdit9.Text
        p12.Value = TextEdit10.Text
        p13.Value = ""
        p14.Value = ""
        p15.Value = TextEdit13.Text
        p16.Value = ""
        p17.Value = TextEdit15.Text
        p18.Value = ""
        p19.Value = 0

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

        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ScriviFor()
        Dim scrivi As String = ""
        scrivi = "INSERT into Tbfor (FoCod,FoPagam,FoAbi,FoCab,FoEnasarco,FoCC,FoCinEur,FoCin,FocodBan,FoSoggRit,FoAttivo,FoBlackList) VALUES (@Focod,@FoPagam,@FoAbi,@FoCab,@FoEnasarco,@FoCC,@FoCinEur,@FoCin,@FoCodBan,@FoSoggRit,@FoAttivo,@FoBlackList)"
        Cmd = New SqlCommand(scrivi, cnDb)

        Dim p1 As New SqlParameter("@FoCod", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@FoPagam", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@FoAbi", SqlDbType.Int)
        Dim p4 As New SqlParameter("@FoCab", SqlDbType.Int)
        Dim p5 As New SqlParameter("@FoEnasarco", SqlDbType.SmallInt)
        Dim p6 As New SqlParameter("@FoCC", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@FoCinEur", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@FoCin", SqlDbType.VarChar)
        Dim p9 As New SqlParameter("@FoCodBan", SqlDbType.SmallInt)
        Dim p10 As New SqlParameter("@FoSoggRit", SqlDbType.Bit)
        Dim p16 As New SqlParameter("@FoAttivo", SqlDbType.Bit)
        Dim p17 As New SqlParameter("@FoBlackList", SqlDbType.Bit)
        p1.Value = TextEdit00.Text
        p2.Value = Val(TextEdit17.Text)
        p3.Value = Val(TextEdit20.Text)
        p4.Value = Val(TextEdit19.Text)
        p5.Value = 0
        p6.Value = TextEdit18.Text.PadLeft(12, "0")
        p7.Value = Val(TextEdit22.Text)
        p8.Value = TextEdit21.Text.Trim
        If ComboBoxEdit4.SelectedIndex = -1 Then
            p9.Value = 0
        Else
            p9.Value = Val(Mid(ComboBoxEdit4.Properties.Items(ComboBoxEdit4.SelectedIndex), 1, 3))
        End If
        p10.Value = CheckEdit1.Checked
        p16.Value = 1
        p17.Value = 0
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
        Cmd.Parameters.Add(p16)
        Cmd.Parameters.Add(p17)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub scrivi_ultimi(ByVal CLFO As String)
        Dim i As Int16
        Dim scrivi As Boolean


        gruppi = leggi_ultimi("FO")

        scrivi = False
        For i = 0 To 8
            If Val(TextEdit00.Text) <= gruppi.al(i) Then
                If Val(TextEdit00.Text) > gruppi.ult(i) Then
                    gruppi.ult(i) = Val(TextEdit00.Text)
                    scrivi = True
                End If
                Exit For
            End If
        Next

        If scrivi = False Then
            Exit Sub
        End If

        Dim strscrivi As String
        strscrivi = "update TbGrp set GrpUl1=@GrpUl1, GrpUl2=@GrpUl2, GrpUl3=@GrpUl3, GrpUl4=@GrpUl4, GrpUl5=@GrpUl5, GrpUl6=@GrpUl6,GrpUl7=@GrpUl7, GrpUl8=@GrpUl8, GrpUl9=@GrpUl9 where GrpCod=@GrpCod"
        Cmd = New SqlCommand(strscrivi, cnCo)

        Dim p1 As New SqlParameter("@GrpUl1", SqlDbType.Int)
        Dim p2 As New SqlParameter("@GrpUl2", SqlDbType.Int)
        Dim p3 As New SqlParameter("@GrpUl3", SqlDbType.Int)
        Dim p4 As New SqlParameter("@GrpUl4", SqlDbType.Int)
        Dim p5 As New SqlParameter("@GrpUl5", SqlDbType.Int)
        Dim p6 As New SqlParameter("@GrpUl6", SqlDbType.Int)
        Dim p7 As New SqlParameter("@GrpUl7", SqlDbType.Int)
        Dim p8 As New SqlParameter("@GrpUl8", SqlDbType.Int)
        Dim p9 As New SqlParameter("@GrpUl9", SqlDbType.Int)
        Dim p10 As New SqlParameter("@GrpCod", SqlDbType.Char)

        p1.Value = gruppi.ult(0)
        p2.Value = gruppi.ult(1)
        p3.Value = gruppi.ult(2)
        p4.Value = gruppi.ult(3)
        p5.Value = gruppi.ult(4)
        p6.Value = gruppi.ult(5)
        p7.Value = gruppi.ult(6)
        p8.Value = gruppi.ult(7)
        p9.Value = gruppi.ult(8)
        p10.Value = CLFO
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
        Try
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
        End Try
    End Sub

End Class