Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports DevExpress.XtraReports.UI

Public Class DxPianoForm
    Dim RwX As DataRow
    Dim Insert As Boolean
    Dim OldValue As String = ""
    Dim P As Boolean = False
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim REPORT As New XtraReport
    Dim selectformula, SCRI As String

    Dim DsPia As DataTable
    Dim DaPia As SqlDataAdapter

    Dim DsCns As DataTable
    Dim DaCns As SqlDataAdapter
    Dim RwCns As DataRow

    Dim Sw As Int16 = 0
    Dim OkLDP As Boolean = False
    Dim OkGruppoMondo As Boolean = False
    Dim UserId As String = ""
    Dim GlGroupAccount As String = ""
    Dim CCeeSt As String = ""
    Dim Pagina As Int16 = 1
    Dim CnAVVERSA As SqlClient.SqlConnection
    Dim WhoAmI As String = ""
    Dim Rosine As Boolean = False
    Private Sub DxPiaForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If Sw = 0 Then
            LeggiAzienda() : GestioneUser()
            Sw = 1
        End If
        Pagina = 1
        Pulizia(True)
        PopolaCb1()
        PopolaGrid()
        TextEdit1.Focus()
    End Sub
    Sub GestioneUser()
        REM MONDOMARINE
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UserId = dataRd.Item("Sel14")
        End While
        dataRd.Close()
        OkLDP = False
        If (UserId.ToUpper = "PASTAECO" Or UserId.ToUpper = "PASTANEW" Or UserId.ToUpper = "PASTAGROUP" Or UserId.ToUpper = "MONDOMARINE") Then
            OkLDP = True
        End If
        If UserId.ToUpper = "MONDOMARINE" Then
            Cmd = New SqlCommand("SELECT count(*) FROM TbGroup", cnCo)
            If Cmd.ExecuteScalar > 0 Then
                OkGruppoMondo = True
                LeggiGruppoMondo()
            End If

        End If
        If (UserId.ToUpper = "COMUNITA" Or UserId.ToUpper = "PENSIONATO") Then
            XtraTabControl1.ShowTabHeader = True
            Rosine = True
            If ConnettiIDB() = False Then Me.Close()
            CaricaSimboli
            GroupControl20.Enabled = False
        Else
            XtraTabControl1.ShowTabHeader = False
            Rosine = False
        End If
    End Sub
    Sub CaricaSimboli()
        ImageComboBoxEdit6.Properties.Items.Clear()
        For i As Int16 = 1 To RepositoryItemImageComboBox7.Items.Count
            ImageComboBoxEdit6.Properties.Items.Add(RepositoryItemImageComboBox7.Items(i - 1))
        Next
        ImageComboBoxEdit6.SelectedIndex = -1
    End Sub
    Function ConnettiIDB() As Boolean
        Dim BaseConn, CCOGE As String
        Dim ISTSQL As String = ""

        Cmd = New SqlCommand("Select * from TbIstanzaAvversa", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            ISTSQL = dataRd.Item("IstanzaSQL")
            WhoAmI = dataRd.Item("NomeDesc")
        End If
        dataRd.Close()

        BaseConn = "Server=" & ISTSQL.Trim & ";Uid=selco;Pwd=1234Clienti;database="
        CCOGE = BaseConn & "COGE"
        CnAVVERSA = New SqlConnection(CCOGE)

        Try
            CnAVVERSA.Open()
        Catch ex As Exception
            MessageBox.Show("Impossibile connettersi con il server : " & WhoAmI & " !!!", "CONNESSIONE DATI CONSOLIDATO", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End Try

        Return True
    End Function
    Sub LeggiGruppoMondo()
        Dim str As String = "select distinct GLGroupAccount,GLGroupAccountDesc from TbGroup  order by GLGroupAccount"
        ImageComboBoxEdit5.Properties.Items.Clear()
        nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 0, -1)
        ImageComboBoxEdit5.Properties.Items.Add(nn)
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("GLGroupAccount") & " " & dataRd.Item("GLGroupAccountDesc"), dataRd.Item("GLGroupAccount"), -1)
            ImageComboBoxEdit5.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Private Sub LeggiAzienda()
        Dim Cmd As New SqlCommand("SELECT distinct top 1 Aziannolavoro, AziGruppoCesp, AziSpecieCesp, AziSottosCesp  from TbAzi order by aziannolavoro desc", cnCo)
        Dim a1 As String = ""
        Dim a2 As String = ""
        Dim a3 As String = "0"
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            a1 = dataRd.Item("AziGruppoCesp")
            a2 = dataRd.Item("AziSpecieCesp")
            a3 = dataRd.Item("AziSottosCesp")
        End While
        dataRd.Close()
        Dim str As String = "SELECT * from TbCsp where CspGru = '" & a1 & "' and CspSpe1 = '" & a2 & "' and CspSpe2 = '" & a3 & "' and CspNum <> '00'"
        ImageComboBoxEdit4.Properties.Items.Clear()
        nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 0, -1)
        ImageComboBoxEdit4.Properties.Items.Add(nn)
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("CspNum").Padleft(2, "0") & " " & dataRd.Item("CspDesc"), dataRd.Item("CspNum"), -1)
            ImageComboBoxEdit4.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Sub PopolaCb1()
        ImageComboBoxEdit1.Properties.Items.Clear()
        Dim str As String = "SELECT CgcCod, CgcDesc from TbCgc where CgcLivello = 1 order by CgcCod"
        Dim SS As String = ""
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SS = dataRd.Item("CgcCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CgcDesc")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CgcCod"), -1)
            ImageComboBoxEdit1.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Sub PopolaCb2()
        Dim Cod As String = Mid(ImageComboBoxEdit1.EditValue, 1, 1)
        Dim Str As String = "SELECT CgcCod, CgcDesc from TbCgc where CgcLivello = 2 and CgcCod like'" & Cod & "%' order by CgcCod"
        Dim CodCod As String = ""
        ImageComboBoxEdit2.Properties.Items.Clear()
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CodCod = Mid(dataRd.Item("CgcCod").ToString, 2, 1)
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(CodCod.PadRight(2, " ") & " " & dataRd.Item("CgcDesc"), CodCod, -1)
            ImageComboBoxEdit2.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Sub PopolaCb3()
        Dim Cod As String = ImageComboBoxEdit1.EditValue & ImageComboBoxEdit2.EditValue
        Dim Str As String = "SELECT CgcCod, CgcDesc from TbCgc where CgcLivello = 3 and CgcCod like'" & Cod & "%' order by CgcCod"
        Dim CodCod As String = ""
        ImageComboBoxEdit3.Properties.Items.Clear()
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CodCod = Mid(dataRd.Item("CgcCod").ToString, 3, 1)
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(CodCod.PadRight(2, " ") & " " & dataRd.Item("CgcDesc"), CodCod, -1)
            ImageComboBoxEdit3.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Sub Pulizia(ByVal T As Boolean)
        TextEdit2.EditValue = "" : TextEdit3.EditValue = "" : TextEdit4.EditValue = ""
        TextEdit5.EditValue = "" : TextEdit6.EditValue = ""
        ImageComboBoxEdit1.SelectedIndex = -1 : ImageComboBoxEdit4.SelectedIndex = -1
        ImageComboBoxEdit2.Properties.Items.Clear() : ImageComboBoxEdit3.Properties.Items.Clear()
        ImageComboBoxEdit2.SelectedIndex = -1 : ImageComboBoxEdit3.SelectedIndex = -1
        ImageComboBoxEdit5.SelectedIndex = -1
        ComboBoxEdit5.SelectedIndex = -1 : ComboBoxEdit6.SelectedIndex = -1
        If T = True Then TextEdit1.EditValue = "" : Insert = True : P = False : ButtonF3.Enabled = False
        CheckEdit1.Checked = False : CheckEdit2.Checked = False : CheckEdit3.Checked = False
        GroupControl18.Visible = OkGruppoMondo
    End Sub
    Sub PopolaGrid()
        Dim str As String = "SELECT * FROM TbPia order by PiaCodCo"
        DaPia = New SqlDataAdapter(str, cnCo)
        DsPia = New DataTable
        DaPia.Fill(DsPia)
        GridControl1.DataSource = DsPia
        GridControl1.Refresh()
        GridView1.UnselectRow(0)
        GridView1.ClearSelection()
    End Sub
    Function ControlloCodice(ByVal CodiceCo As String) As Boolean
        If Len(CodiceCo) <> 5 Then Return False : Exit Function
        If Not IsNumeric(Mid(CodiceCo, 1, 2)) Then Return False : Exit Function
        If Not IsNumeric(Mid(CodiceCo, 4, 2)) Then Return False : Exit Function
        If Mid(CodiceCo, 3, 1) <> "." Then Return False : Exit Function
        Return True
    End Function
    Private Sub TbLeggi1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi1.Enter
        If ControlloCodice(TextEdit1.EditValue) = False Then
            TextEdit1.Focus()
            Exit Sub
        End If
        CaricaDati()
        TextEdit2.Focus()
    End Sub
    Sub CaricaDati()
        Insert = True
        CheckEdit3.Checked = False
        Dim str As String = "SELECT * from TbPia where PiaCodCo = '" & TextEdit1.EditValue & "'"
        Dim cmd As New SqlCommand(str, cnCo)
        Dim fl01 As String = ""
        Dim fl02 As String = ""
        Dim fl03 As String = ""
        Dim fl04 As String = ""
        Dim fl05 As String = ""
        Dim fl06 As String = ""
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit1.EditValue = dataRd.Item("PiaCodCo")
            TextEdit2.EditValue = dataRd.Item("PiaAnaCo")
            fl01 = dataRd.Item("PiaFl01")
            fl02 = dataRd.Item("PiaFl02")
            fl03 = dataRd.Item("PiaFl03")
            fl04 = dataRd.Item("PiaFl04")
            fl05 = dataRd.Item("PiaFl05")
            fl06 = dataRd.Item("PiaFl06")
            TextEdit3.EditValue = dataRd.Item("PiaFl07")
            CheckEdit1.Checked = dataRd.Item("PiaFl08")
            CheckEdit2.Checked = dataRd.Item("PiaFl09")
            TextEdit4.EditValue = dataRd.Item("PiaFl11")
            TextEdit6.EditValue = dataRd.Item("PiaFl12")
            Insert = False
            FocusedGrid(TextEdit1.EditValue)
        End If
        dataRd.Close()
        If Insert = False Then
            P = True
            ImageComboBoxEdit1.SelectedIndex = SettaComboImage(ImageComboBoxEdit1, fl01)
            ImageComboBoxEdit2.SelectedIndex = SettaComboImage(ImageComboBoxEdit2, fl02)
            ImageComboBoxEdit3.SelectedIndex = SettaComboImage(ImageComboBoxEdit3, fl03)
            ImageComboBoxEdit4.SelectedIndex = SettaComboImage(ImageComboBoxEdit4, fl04)
            ComboBoxEdit5.SelectedIndex = fl05
            ComboBoxEdit6.SelectedIndex = fl06
            LetturaCee()
            P = False
        Else
            Pulizia(False)
        End If
        ButtonF3.Enabled = Not Insert
        If OkLDP = True Then LetturaLDP()
        If OkGruppoMondo = True Then LetturaGruppoMondo()
    End Sub
    Sub LetturaGruppoMondo()
        Dim str As String = "Select GlGroupAccount from TbGroup where GLAccount = '" & TextEdit1.EditValue & "'"
        Dim cmd As New SqlCommand(str, cnCo)
        GlGroupAccount = ""
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            GlGroupAccount = dataRd.Item("GlGroupAccount")
        End If
        dataRd.Close()
        ImageComboBoxEdit5.EditValue = GlGroupAccount
    End Sub
    Sub LetturaLDP()
        Dim str As String = "Select top 1 CogConto from TbCog where CogConto = '" & TextEdit1.EditValue & "'"
        Dim cmd As New SqlCommand(str, CnDc)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            CheckEdit3.Checked = True
        End If
        dataRd.Close()
    End Sub
    Sub LetturaCee()
        Dim str As String = "SELECT * from TbCee where CeeCod = '" & TextEdit4.EditValue.ToString.PadLeft(5, "0") & "'"
        TextEdit5.EditValue = ""
        CCeeSt = "0"
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit5.EditValue = dataRd.Item("CeeDesc")
            CCeeSt = dataRd.Item("CeeSt")
        End While
        dataRd.Close()
        If CCeeSt > "6" Then TextEdit5.EditValue = "" : TextEdit4.EditValue = "00000"
    End Sub
    Function FocusedGrid(ByVal Cod As String) As Boolean
        Dim OpzControl As Array = GridView1.GetSelectedRows
        For i As Int16 = 1 To GridView1.SelectedRowsCount
            GridView1.UnselectRow(OpzControl(i - 1))
        Next
        For i As Int16 = 1 To GridView1.RowCount
            RwX = GridView1.GetDataRow(i - 1)
            If RwX("PiaCodCo") = Cod Then
                GridView1.FocusedRowHandle = i - 1
                GridView1.SelectRow(i - 1)
                Exit Function
            End If
        Next
    End Function
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub

    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If iset > -1 Then
            RwX = GridView1.GetDataRow(iset)
            TextEdit1.EditValue = RwX("PiaCodCo")
            SelectNextControl(TbLeggi1, True, True, True, False)
        End If
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If ControlloCodice(TextEdit1.EditValue) = False Then Exit Sub
        If OkLDP = True Then
            If Flagmcc() = False Then CheckEdit3.Checked = True : CheckEdit3.Focus() : Exit Sub
            Registra()
            If Mid(TextEdit1.EditValue, 3, 3) <> ".00" Then RegistraLDP()
        Else
            Registra()
        End If
        If OkGruppoMondo = True Then RegistraGruppoMondo()
        OldValue = TextEdit1.EditValue
        ButtonF5.PerformClick()
        FocusedGrid(OldValue)
        If Rosine = True And UserId.ToUpper = "PENSIONATO" Then
            XtraTabControl1.SelectedTabPageIndex = 1
            FocusedBanded(OldValue)
        End If
    End Sub
    Sub RegistraGruppoMondo()
        If Mid(TextEdit1.EditValue, 3, 3) = ".00" Then Exit Sub

        Dim GDel As String = "Delete from TbGroup where GLAccount = @P1"
        Dim GIns As String = "Insert into TbGroup(GLAccount,GLGroupAccount,GlGroupAccountDesc) Values (@P2,@P3,@P4)"
        Dim p1 As New SqlParameter("@P1", SqlDbType.NVarChar)
        Dim p2 As New SqlParameter("@P2", SqlDbType.NVarChar)
        Dim p3 As New SqlParameter("@P3", SqlDbType.NVarChar)
        Dim p4 As New SqlParameter("@P4", SqlDbType.NVarChar)

        Cmd = New SqlCommand(GDel, cnCo)
        p1.Value = TextEdit1.EditValue
        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p1)
        Cmd.ExecuteNonQuery()

        If ImageComboBoxEdit5.SelectedIndex < 1 Then Exit Sub
        p2.Value = TextEdit1.EditValue
        p3.Value = ImageComboBoxEdit5.EditValue.ToString
        p4.Value = Mid(ImageComboBoxEdit5.SelectedItem.ToString, 8, 60)
        Cmd = New SqlCommand(GIns, cnCo)
        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.ExecuteNonQuery()
    End Sub
    Sub Registra()
        Dim Ins As String = "Insert Into TbPia (PiaCodCo,PiaAnaco,PiaFl01,PiaFl02,PiaFl03,PiaFl04,PiaFl05,PiaFl06,PiaFl07,PiaFl08,PiaFl09,PiaFl10,PiaFl11,PiaFl12) values (@PiaCodCo,@PiaAnaco,@PiaFl01,@PiaFl02,@PiaFl03,@PiaFl04,@PiaFl05,@PiaFl06,@PiaFl07,@PiaFl08,@PiaFl09,@PiaFl10,@PiaFl11,@PiaFl12)"
        Dim Upd As String = "Update TbPia set PiaAnaco=@PiaAnaco,PiaFl01=@PiaFl01,PiaFl02=@PiaFl02,PiaFl03=@PiaFl03,PiaFl04=@PiaFl04,PiaFl05=@PiaFl05,PiaFl06=@PiaFl06,PiaFl07=@PiaFl07,PiaFl08=@PiaFl08,PiaFl09=@PiaFl09,PiaFl10=@PiaFl10,PiaFl11=@PiaFl11,PiaFl12=@PiaFl12 where PiaCodCo=@PiaCodCo"
        Dim Str As String = ""
        Dim p1 As New SqlParameter("@PiaCodCo", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@PiaAnaCo", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@PiaFl01", SqlDbType.SmallInt)
        Dim p4 As New SqlParameter("@PiaFl02", SqlDbType.SmallInt)
        Dim p5 As New SqlParameter("@PiaFl03", SqlDbType.SmallInt)
        Dim p6 As New SqlParameter("@PiaFl04", SqlDbType.SmallInt)
        Dim p7 As New SqlParameter("@PiaFl05", SqlDbType.SmallInt)
        Dim p8 As New SqlParameter("@PiaFl06", SqlDbType.SmallInt)
        Dim p9 As New SqlParameter("@PiaFl07", SqlDbType.SmallInt)
        Dim p10 As New SqlParameter("@PiaFl08", SqlDbType.Bit)
        Dim p11 As New SqlParameter("@PiaFl09", SqlDbType.Bit)
        Dim p12 As New SqlParameter("@PiaFl10", SqlDbType.SmallInt)
        Dim p13 As New SqlParameter("@PiaFl11", SqlDbType.SmallInt)
        Dim p14 As New SqlParameter("@PiaFl12", SqlDbType.SmallInt)

        If Insert = True Then Str = Ins Else Str = Upd
        Cmd = New SqlCommand(Str, cnCo)

        p1.Value = TextEdit1.EditValue
        p2.Value = TextEdit2.EditValue
        If Mid(TextEdit1.EditValue, 3, 3) = ".00" Then
            p3.Value = 0
            p4.Value = 0
            p5.Value = 0
            p6.Value = 0
            p7.Value = 0
            p8.Value = 0
            p9.Value = 0
            p10.Value = 0
            p11.Value = 0
            p12.Value = 0
            p13.Value = 0
            p14.Value = 0
        Else
            p3.Value = Val(ImageComboBoxEdit1.EditValue)
            p4.Value = Val(ImageComboBoxEdit2.EditValue)
            p5.Value = Val(ImageComboBoxEdit3.EditValue)
            p6.Value = Val(ImageComboBoxEdit4.EditValue)
            If ComboBoxEdit5.SelectedIndex > 0 Then p7.Value = ComboBoxEdit5.SelectedIndex Else p7.Value = 0
            If ComboBoxEdit6.SelectedIndex > 0 Then p8.Value = ComboBoxEdit6.SelectedIndex Else p8.Value = 0
            p9.Value = Val(TextEdit3.EditValue)
            If CheckEdit1.Checked = True Then p10.Value = 1 Else p10.Value = 0
            If CheckEdit2.Checked = True Then p11.Value = 1 Else p11.Value = 0
            p12.Value = 0 '??????????????????????????????????????????????
            p13.Value = Val(TextEdit4.EditValue)
            p14.Value = Val(TextEdit6.EditValue)
        End If

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

        Cmd.ExecuteNonQuery()
        If Rosine = False Then Exit Sub
        If UserId.ToUpper = "COMUNITA" Then Exit Sub '' MANCA CONTROLLO SE DOPPIO IN ROSINE
        Dim InsX As String = "Insert Into TbDaAa (DACONTO,DADESCRIZIONE,AACONTO,AADESCRIZIONE) values (@DAC,@DAD,@AAC,@AAD)"
        Dim UpdX As String = "Update TbDaAa set DADESCRIZIONE=@DAD where DACONTO=@DAC"
        Str = ""
        Dim Q1 As New SqlParameter("@DAC", SqlDbType.VarChar)
        Dim Q2 As New SqlParameter("@DAD", SqlDbType.VarChar)
        Dim Q3 As New SqlParameter("@AAC", SqlDbType.VarChar)
        Dim Q4 As New SqlParameter("@AAD", SqlDbType.VarChar)
        If Insert = True Then Str = InsX Else Str = UpdX
        Cmd = New SqlCommand(Str, CnAVVERSA)
        Q1.Value = TextEdit1.EditValue
        Q2.Value = TextEdit2.EditValue
        Q3.Value = TextEdit1.EditValue
        Q4.Value = TextEdit2.EditValue

        Cmd.Parameters.Add(Q1)
        Cmd.Parameters.Add(Q2)
        If Insert = True Then
            Cmd.Parameters.Add(Q3)
            Cmd.Parameters.Add(Q4)
        End If
        Try
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            '' REM NON REGISTRA PERCHè DOPPIO ELIMINATO E RICREATO
        End Try
    End Sub
    Sub RegistraLDP()
        'Se non c'è flag deletta sul TbCog
        'Se c'è il flag prima deletta e poi inserisce
        ' SE UN CONTO è DI TIPO 6 O 7 VIENE SEMPRE IMPOSTATO IL FLAG A 1 - non e' vero
        ''If Val(ImageComboBoxEdit1.EditValue) > 5 And Val(ImageComboBoxEdit1.EditValue) < 8 Then CheckEdit3.Checked = True

        Cmd = New SqlCommand("Delete from TbCog where CogConto ='" & TextEdit1.EditValue & "'", CnDc)
        Cmd.ExecuteNonQuery()

        If CheckEdit3.Checked = False Then Exit Sub
        Dim Str1 As String = "Insert Into TbCog (CogLdp,CogCdc,CogRep,CogConto,CogPerc) values (@CogLdp,@CogCdc,@CogRep,@CogConto,@CogPerc)"

        Dim p1 As New SqlParameter("@CogLdp", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@CogCdc", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@CogRep", SqlDbType.SmallInt)
        Dim p4 As New SqlParameter("@CogConto", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@CogPerc", SqlDbType.Decimal)


        Cmd = New SqlCommand(Str1, CnDc)

        p1.Value = 1
        p2.Value = 0
        p3.Value = 0
        p4.Value = TextEdit1.EditValue
        p5.Value = CDec(0.0)
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If ControlloCodice(TextEdit1.EditValue) = False Then Exit Sub
        If Controllod() = False Then Exit Sub
        EliminaConto()
        ButtonF5.PerformClick()
    End Sub

    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If Pagina = 1 Then
            If e.KeyData = Keys.F5 Then
                ButtonF5.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F3 Then
                ButtonF3.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F9 Then
                ButtonF9.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F11 Then
                ButtonF11.PerformClick()
                Exit Sub
            End If
        End If
        If Pagina = 2 Then
            If e.KeyData = Keys.F5 Then
                ButtonXF5.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F9 Then
                ButtonXF9.PerformClick()
                Exit Sub
            End If
            If e.KeyData = Keys.F11 Then
                ButtonXF11.PerformClick()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub ImageComboBoxEdit1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit1.Enter, ImageComboBoxEdit2.Enter, ImageComboBoxEdit3.Enter, ImageComboBoxEdit4.Enter, ComboBoxEdit5.Enter, ComboBoxEdit6.Enter
        If P = False And Mid(TextEdit1.EditValue, 3, 3) = ".00" Then
            sender.SelectedIndex = -1
            ButtonF11.Focus()
        End If
    End Sub
    Private Sub ImageComboBoxEdit1_CloseUp(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles ImageComboBoxEdit1.CloseUp, ImageComboBoxEdit2.CloseUp, ImageComboBoxEdit3.CloseUp, ImageComboBoxEdit4.CloseUp, ComboBoxEdit5.CloseUp, ComboBoxEdit6.CloseUp
        If P = False Then System.Windows.Forms.SendKeys.Send("{TAB}")
    End Sub
    Private Sub ImageComboBoxEdit1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit1.SelectedIndexChanged
        If ImageComboBoxEdit1.SelectedIndex > -1 Then
            PopolaCb2() : PopolaCb3()
        End If
    End Sub
    Private Sub ImageComboBoxEdit2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit2.SelectedIndexChanged
        If ImageComboBoxEdit2.SelectedIndex > -1 Then
            PopolaCb3()
        End If
    End Sub

    Private Sub ButtonC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonC1.Click
        Dim CodiceConto As String = Query.CercaPia()
        If CodiceConto Is Nothing Then Exit Sub
        If CodiceConto.Length <> 5 Then Exit Sub
        TextEdit1.EditValue = CodiceConto
        SelectNextControl(TbLeggi1, True, True, True, False)
    End Sub

    Private Sub ButtonC2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonC2.Click
        Dim CodiceConto As Integer = Query.CercaCee("")
        If CodiceConto > 0 Then TextEdit4.EditValue = CodiceConto
        LetturaCee()
        SelectNextControl(sender, True, True, True, False)
    End Sub

    Private Sub TextEdit4_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit4.Validated
        LetturaCee()
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim StrPrint As String = "SELECT * FROM CRPIA"
        DsPia = New DataTable
        If OkLDP = True Then
            StrPrint = "SELECT * FROM CRPIACDC"
            DaPia = New SqlDataAdapter(StrPrint, CnDc)
            DaPia.SelectCommand.CommandTimeout = 300
            DaPia.Fill(DsPia)
            REPORT = New DxStPiaLDP
        Else
            DaPia = New SqlDataAdapter(StrPrint, cnCo)
            DaPia.SelectCommand.CommandTimeout = 300
            DaPia.Fill(DsPia)
            REPORT = New DxStPia
        End If
        REPORT.DataSource = DsPia
        REPORT.DataMember = "DsPia"
        REPORT.Parameters.Item("Marchio").Value = Marchio()
        REPORT.ShowPreview()
    End Sub

    Private Function Controllod() As Boolean
        Controllod = True
        Dim Str As String
        Dim CONTO As String = TextEdit1.EditValue
        If CONTO.Substring(3, 2) = "00" Then
            Str = "Select top 1 PRKCONTO from TbPrk where PrkConto LIKE '" & CONTO.Substring(0, 3) & "%'"
        Else
            Str = "Select top 1 PRKCONTO from TbPrk where PrkConto = '" & CONTO & "'"
        End If
        Dim cmd As New SqlCommand(Str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            Controllod = False
        End If
        dataRd.Close()
        If Controllod = False Then
            MessageBox.Show("Conto Movimentato!!!" & Chr(13), "Impossibile Annullare", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Function


    Sub EliminaConto()
        Dim cod As String = TextEdit1.EditValue
        Dim box As Object
        If cod.Substring(3, 2) = "00" Then
            box = MessageBox.Show("Vuoi eliminare il mastro?", "ELIMINA MASTRO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
        Else
            box = MessageBox.Show("Vuoi eliminare il conto?", "ELIMINA CONTO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
        End If
        If box <> DialogResult.Yes Then GoTo FineOp
        Cmd = New SqlCommand("delete from TbPia where PiaCodCo ='" & cod & "'", cnCo)
        Cmd.ExecuteNonQuery()
        If Rosine = True And UserId.ToUpper = "PENSIONATO" Then
            Cmd = New SqlCommand("delete from TbDaAa where DACONTO ='" & cod & "'", CnAVVERSA)
            Cmd.ExecuteNonQuery()
        End If
        If OkLDP = True Then
            Cmd = New SqlCommand("delete from TbCog where CogConto ='" & cod & "'", CnDc)
            Cmd.ExecuteNonQuery()
        End If
FineOp:
        ButtonF5.PerformClick()
    End Sub

    Private Function Flagmcc() As Boolean
        'deve controllare se esistono movimenti sul TBMCC SE ESISTONO NON DEVO PERMETTERE IL CAMBIO 
        Flagmcc = True
        If CheckEdit3.Checked = True Then Exit Function

        Dim Strm As String
        Dim CONTO As String = TextEdit1.EditValue
        If CONTO.Substring(3, 2) = "00" Then
            Strm = "Select top 1 MCCCogConto from TbMcc where MccCogConto LIKE '" & CONTO.Substring(0, 3) & "%'"
        Else
            Strm = "Select top 1 MCCCogConto from TbMcc where MccCogConto = '" & CONTO & "'"
        End If
        Dim cmd As New SqlCommand(Strm, CnDc)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            Flagmcc = False
        End If
        dataRd.Close()
        If Flagmcc = False Then
            MessageBox.Show("Conto Movimentato!!! Impossibile Variare il Flag" & Chr(13), "Commesse/Centri di costo", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Function
#Region "PAGINA 2 CONSOLIDATO SOLO PER ROSINE"
    Sub IIPagina()
        '' Dim str As String = "SELECT * FROM TbDaAa order by DACONTO"
        Dim str As String = "exec XLEGGIDAAA"
        If UserId.ToUpper = "COMUNITA" Then
            DaCns = New SqlDataAdapter(str, cnCo)
        Else
            DaCns = New SqlDataAdapter(str, CnAVVERSA)
        End If
        DsCns = New DataTable
        DaCns.Fill(DsCns)
        GridControl2.DataSource = DsCns
        GridControl2.Refresh()
        BandedGridView1.UnselectRow(0)
        BandedGridView1.ClearSelection()
    End Sub
    Private Sub BandedGridView1_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles BandedGridView1.RowClick
        If e.RowHandle > -1 Then
            iset = e.RowHandle
            RwCns = BandedGridView1.GetDataRow(iset)
            CaricaDettagli()
            GroupControl20.Enabled = True
        Else
            iset = -1
            GroupControl20.Enabled = False
        End If
    End Sub
    Function FocusedBanded(ByVal Cod As String) As Boolean
        Dim OpzControl As Array = BandedGridView1.GetSelectedRows
        For i As Int16 = 1 To BandedGridView1.SelectedRowsCount
            BandedGridView1.UnselectRow(OpzControl(i - 1))
        Next
        For i As Int16 = 1 To BandedGridView1.RowCount
            RwCns = BandedGridView1.GetDataRow(i - 1)
            If RwCns("DACONTO") = Cod Then
                BandedGridView1.FocusedRowHandle = i - 1
                BandedGridView1.SelectRow(i - 1)
                CaricaDettagli()
                GroupControl20.Enabled = True
                TextEdit11.Focus()
                Exit Function
            End If
        Next
    End Function
    Sub CaricaDettagli()
        TextEdit8.EditValue = RwCns("DACONTO")
        TextEdit7.EditValue = RwCns("DADESCRIZIONE")
        TextEdit11.EditValue = RwCns("AACONTO")
        TextEdit10.EditValue = RwCns("AADESCRIZIONE")
        ImageComboBoxEdit6.EditValue = RwCns("AASO")
    End Sub
    Private Sub TbLeggi2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi2.Enter
        If ControlloCodice(TextEdit8.EditValue) = False Then
            TextEdit8.Focus()
            Exit Sub
        End If
        '' CaricaDati()
        TextEdit11.Focus()
    End Sub
    Private Sub TbLeggi3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi3.Enter
        If ControlloCodice(TextEdit11.EditValue) = False Then
            TextEdit11.Focus()
            Exit Sub
        End If
        If LeggiConto() = True Then
            If TextEdit11.EditValue = TextEdit8.EditValue Then
                ImageComboBoxEdit6.SelectedIndex = 0
            Else
                ImageComboBoxEdit6.SelectedIndex = 2
            End If
            ButtonXF11.Focus()
        Else
            ImageComboBoxEdit6.SelectedIndex = 1
            TextEdit10.Focus()
        End If
    End Sub
    Function LeggiConto() As Boolean
        LeggiConto = False
        Dim Str As String = "SELECT * from TbPia where PiaCodCo = '" & TextEdit11.EditValue & "'"
        Dim Cmd As New SqlCommand(Str, cnCo)
        If UserId.ToUpper = "COMUNITA" Then
            Cmd = New SqlCommand(Str, cnCo)
        Else
            Cmd = New SqlCommand(Str, CnAVVERSA)
        End If

        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit10.EditValue = dataRd("PiaAnaCo")
            LeggiConto = True
        End While
        dataRd.Close()
    End Function

    Private Sub ButtonXF5_Click(sender As Object, e As EventArgs) Handles ButtonXF5.Click
        TextEdit8.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit11.EditValue = ""
        TextEdit10.EditValue = ""
        ImageComboBoxEdit6.SelectedIndex = -1
        IIPagina()
    End Sub

    Private Sub ButtonXF9_Click(sender As Object, e As EventArgs) Handles ButtonXF9.Click
        DXANTEPRIMA(GridControl2, True, Printing.PaperKind.A4, XtraTabPage2.Text)
    End Sub
    Private Sub ButtonXF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF11.Click
        If TextEdit10.EditValue = "*** ERRATO ***" Then Exit Sub
        If Mid(TextEdit8.EditValue, 4, 2) = "00" And Mid(TextEdit11.EditValue, 4, 2) <> "00" Then TextEdit10.EditValue = "*** ERRATO ***" : Exit Sub
        RegistraContoCons()
        ButtonXF5.PerformClick()
    End Sub
    Sub RegistraContoCons()
        Dim Str As String = "Update TbDaAa set AACONTO=@AAC,AADESCRIZIONE=@AAD where DACONTO=@DAC"
        Dim Q1 As New SqlParameter("@DAC", SqlDbType.VarChar)
        Dim Q3 As New SqlParameter("@AAC", SqlDbType.VarChar)
        Dim Q4 As New SqlParameter("@AAD", SqlDbType.VarChar)
        If UserId.ToUpper = "COMUNITA" Then
            Cmd = New SqlCommand(Str, cnCo)
        Else
            Cmd = New SqlCommand(Str, CnAVVERSA)
        End If
        Q1.Value = TextEdit8.EditValue
        Q3.Value = TextEdit11.EditValue
        Q4.Value = TextEdit10.EditValue

        Cmd.Parameters.Add(Q1)
        Cmd.Parameters.Add(Q3)
        Cmd.Parameters.Add(Q4)
        Cmd.ExecuteNonQuery()
    End Sub


#End Region

#Region "GESTIONE FONDO PAGINA"
    Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            Pagina = 1
            Exit Sub
        End If
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            Pagina = 2
            IIPagina()
            Exit Sub
        End If
    End Sub
#End Region
End Class