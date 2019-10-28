Imports DXBASE
Imports NPRINT
Imports System.Data.SqlClient
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI

Public Class DxLetInt
    Dim DsDic As DataTable
    Dim DaDic As SqlDataAdapter
    Dim RwDic As DataRow
    Dim Anno As Integer = -1
    Dim Rifer As Integer = -1
    Dim ProgressInterno As Integer = 0
    Dim IT(3) As String

    Private Sub DxLetInt_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        CaricaIniziale()
        PuliziaCampi()
    End Sub
    Sub PuliziaCampi()
        ComboBoxEdit8.SelectedIndex = -1 : Rifer = -1
        TextEdit1.EditValue = 0 : ProgressInterno = 0 : TextEdit20.EditValue = 0 : TextEdit21.EditValue = "" : DateEdit1.EditValue = Nothing : DateEdit2.EditValue = Nothing
        DateEdit3.EditValue = Nothing : DateEdit4.EditValue = Nothing : TextEdit2.EditValue = 0 : TextEdit3.EditValue = ""
    End Sub
    Sub CaricaIniziale()
        ' Lettura Intestazione
        For z = 0 To 3 : IT(z) = "" : Next
        Cmd = New SqlCommand("SELECT * from TbAna where anaGrp = 'AZ' and ANACOD = '00001'", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            IT(1) = dataRd.Item("AnaDesc")
            IT(2) = dataRd.Item("AnaIndirizzo")
            IT(3) = Trim(dataRd.Item("AnaCap")) & " " & Trim(dataRd.Item("AnaCitta")) & " " & Trim(dataRd.Item("AnaProv"))
            IT(0) = "P.I. " & Trim(dataRd.Item("AnaPiva")) & " C.F. " & Trim(dataRd.Item("AnaCFis"))
        End While
        dataRd.Close()
        'Lettura Codici Iva
        ComboBoxEdit8.Properties.Items.Clear()
        ComboBoxEdit8.Properties.Items.Add("")
        Cmd = New SqlCommand("SELECT * FROM TbCii where CiiDes > '' and CiiAli = 0 order by CiiCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit8.Properties.Items.Add(dataRd.Item("CiiCod").ToString.PadLeft(2, "0") & " " & dataRd.Item("CiiDes"))
        End While
        dataRd.Close()
        'Lettura Anni Aperti
        ComboBoxEdit1.SelectedIndex = -1
        ComboBoxEdit1.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT Distinct DDiCAnno from TbDDiCl order by DDiCAnno Desc", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("DDiCAnno"))
            If dataRd.Item("DDiCAnno") > Anno Then Anno = dataRd.Item("DDiCAnno")
        End While
        dataRd.Close()
        If Anno = -1 Then Anno = CDate(Today).Year
        If ComboBoxEdit1.Properties.Items.Count = 0 Then
            ComboBoxEdit1.Properties.Items.Add(Anno)
        End If
        ComboBoxEdit1.SelectedIndex = 0
    End Sub
    Sub CaricaGrid()
        Dim str As String = "Select * from VDichCli where DDiCAnno = " & ComboBoxEdit1.EditValue & " Order by DDiCAnno,DDiCNProgInterno"
        DsDic = New DataTable
        DaDic = New SqlDataAdapter(str, cnCo)
        DaDic.Fill(DsDic)
        GridControl1.DataSource = DsDic
        GridControl1.Refresh()
        GroupControl1.Text = "Registro Dichiarazioni Intento Ricevute Anno " & Anno
    End Sub
    Private Sub ComboBoxEdit1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        If ComboBoxEdit1.SelectedIndex > -1 Then
            Anno = ComboBoxEdit1.EditValue
            CaricaGrid()
        End If
    End Sub
    Private Sub ButtonPlus_Click(sender As System.Object, e As System.EventArgs) Handles ButtonPlus.Click
        AggiungiAnno()
    End Sub
    Sub AggiungiAnno()
        Dim msg As String
        Dim NewYear As Integer = Anno
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        For y As Int16 = 1 To ComboBoxEdit1.Properties.Items.Count
            If ComboBoxEdit1.Properties.Items(y - 1) > NewYear Then NewYear = ComboBoxEdit1.Properties.Items(y - 1)
        Next
        NewYear += 1
        msg = "Aggiungo Nuovo Anno " & NewYear.ToString & " ?" & Chr(13)
        msg &= Chr(13)
        msg &= "Attenzione il nuovo anno sarà operativo solo se inserisco almeno 1 nuova Lettera d'intento"
        style = MsgBoxStyle.YesNo
        response = MsgBox(msg, style, "APERTURA NUOVO ANNO")
        If response = MsgBoxResult.Yes Then
            ComboBoxEdit1.Properties.Items.Add(NewYear)
            Anno = NewYear
            ComboBoxEdit1.EditValue = Anno
            PuliziaCampi()
        End If
        ComboBoxEdit1.Focus()
    End Sub
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub
    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If iset > -1 Then
            RwDic = GridView1.GetDataRow(iset)
            LeggiDatiDaGrid()
        End If
    End Sub
    Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        RicCliFor("CL")
    End Sub
    Sub VerificaCli()
        If LeggiCli(TextEdit20.EditValue) = False Then
            TextEdit20.Focus()
        End If
    End Sub
    Sub RicCliFor(ByVal cf As String)
        Dim frm As New RicercaClFo
        frm.StartPosition = FormStartPosition.CenterScreen
        frm.CliFor = cf
        frm.ShowDialog()
        If frm.Codice > "01000" Then
            TextEdit20.EditValue = frm.Codice
            VerificaCli()
        End If
    End Sub
    Function LeggiCli(ByVal codice As String) As Boolean
        LeggiCli = False
        If dataRd.IsClosed = False Then
            Exit Function
        End If
        Cmd = New SqlCommand("select * from CRCli where AnaCod ='" & codice & "'", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit20.EditValue = dataRd.Item("AnaCod")
            TextEdit21.EditValue = dataRd.Item("AnaDesc")
            LeggiCli = True
        End If
        dataRd.Close()
        If LeggiCli = False Then Exit Function
        LeggiDatiIniziali(codice)
    End Function
    Sub LeggiDatiIniziali(ByVal codice As String)
        Rifer = -1
        Cmd = New SqlCommand("Select * from VDichCli where DDiCAnno = " & Anno & " and DDiCNProgDichiar = " & Val(TextEdit1.EditValue) & " and DDiCCli = '" & TextEdit20.EditValue.ToString & "'", cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            DateEdit1.EditValue = dataRd.Item("DDiCRicev")
            DateEdit2.EditValue = dataRd.Item("DDiCDataDoc")
            DateEdit3.EditValue = dataRd.Item("DDiCOpDa3")
            DateEdit4.EditValue = dataRd.Item("DDiCOpAa3")
            ComboBoxEdit8.EditValue = dataRd.Item("CiiCod").ToString.PadLeft(2, "0") & " " & dataRd.Item("CiiDes")
            ProgressInterno = dataRd.Item("DDiCNProgInterno")
            Rifer = dataRd.Item("DDiCId")
            TextEdit2.EditValue = dataRd.Item("DDiImporto")
            TextEdit3.EditValue = dataRd.Item("DDiProtocollo")
        End If
        dataRd.Close()
    End Sub
    Sub LeggiDatiDaGrid()
        Anno = RwDic("DDiCAnno")
        TextEdit1.EditValue = RwDic("DDiCNProgDichiar")
        TextEdit20.EditValue = RwDic("DDiCCli")
        TextEdit21.EditValue = RwDic("AnaDesc")
        DateEdit1.EditValue = RwDic("DDiCRicev")
        DateEdit2.EditValue = RwDic("DDiCDataDoc")
        DateEdit3.EditValue = RwDic("DDiCOpDa3")
        DateEdit4.EditValue = RwDic("DDiCOpAa3")
        ComboBoxEdit8.EditValue = RwDic("CiiCod").ToString.PadLeft(2, "0") & " " & RwDic("CiiDes")
        ProgressInterno = RwDic("DDiCNProgInterno")
        Rifer = RwDic("DDiCId")
        TextEdit2.EditValue = RwDic("DDiImporto")
        TextEdit3.EditValue = RwDic("DDiProtocollo")
    End Sub

    Private Sub TextEdit20_Leave(sender As Object, e As System.EventArgs) Handles TextEdit20.Leave
        TextEdit20.EditValue = TextEdit20.EditValue.ToString.PadLeft(5, "0")
        VerificaCli()
    End Sub
    ''  Private Sub ComboBoxEdit8_Leave(sender As Object, e As System.EventArgs) Handles ComboBoxEdit8.Leave
    ''      ButtonF11.Focus()
    ''  End Sub

    Private Sub ButtonF11_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF11.Click
        If Controllacampi() = False Then Exit Sub
        Registra()
        ButtonF5.PerformClick()
        TextEdit1.Focus()
    End Sub


    Function Controllacampi() As Boolean
        Dim Mail As String = ""
        If Val(TextEdit1.EditValue) < 1 Then
            Mail = Mail & "<>CONTROLLARE Nr.PROGRESSIVO ATTRIBUITO DAL DICHIARANTE" & Chr(13)
        End If
        If TextEdit20.EditValue < "01001" Or TextEdit21.EditValue.ToString.Length < 1 Then
            Mail = Mail & "<>SELEZIONARE ANAGRAFICA" & Chr(13)
        End If
        If ComboBoxEdit8.SelectedIndex < 1 Then
            Mail = Mail & "<>MANCA ASSOGGETTAMENTO I.V.A." & Chr(13)
        End If
        If Anno < Today.Year - 1 Or Anno > Today.Year + 1 Then
            Mail = Mail & "<>CONTROLLARE ANNO" & Chr(13)
        End If
        If TextEdit2.EditValue <= 0 Then
            Mail = Mail & "<>CONTROLLARE IMPORTO" & Chr(13)
        End If
        If TextEdit3.EditValue.ToString.Length < 17 Then
            Mail = Mail & "<>CONTROLLARE PROTOCOLLO ADE" & Chr(13)
        End If
        If DateEdit2.EditValue Is Nothing Then
            Mail = Mail & "<>CONTROLLARE DATA DOCUMENTO" & Chr(13)
        End If
        If DateEdit1.EditValue Is Nothing Then
            Mail = Mail & "<>CONTROLLARE DATA RICEZIONE" & Chr(13)
        End If
        If (DateEdit3.EditValue Is Nothing) Then
            Mail = Mail & "<>CONTROLLARE PERIODO" & Chr(13)
            GoTo USCITA
        End If
        If (DateEdit4.EditValue Is Nothing) Then
            Mail = Mail & "<>CONTROLLARE PERIODO" & Chr(13)
            GoTo USCITA
        End If
        If DateEdit3.EditValue > DateEdit4.EditValue Then
            Mail = Mail & "<>CONTROLLARE PERIODO" & Chr(13)
            GoTo USCITA
        End If
USCITA:
        If Mail > "" Then
            MoltoCritico(Mail)
            Return False
        End If
        Return True
    End Function

    Sub Registra()
        Dim SCRIVI(1) As String
        Dim N As Integer = 1
        Dim p0 As New SqlParameter("@DDiCId", SqlDbType.Int)
        Dim p1 As New SqlParameter("@DDiCAnno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@DDiCNProgInterno", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@DDiCNProgDichiar", SqlDbType.SmallInt)
        Dim p4 As New SqlParameter("@DDiCCli", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@DDiCCodEse", SqlDbType.TinyInt)
        Dim p6 As New SqlParameter("@DDiCOpDa3", SqlDbType.SmallDateTime)
        Dim p7 As New SqlParameter("@DDiCOpAa3", SqlDbType.SmallDateTime)
        Dim p8 As New SqlParameter("@DDiCRicev", SqlDbType.SmallDateTime)
        Dim p9 As New SqlParameter("@DDiCDataDoc", SqlDbType.SmallDateTime)
        Dim p10 As New SqlParameter("@DDiImporto", SqlDbType.Decimal)
        Dim p11 As New SqlParameter("@DDiProtocollo", SqlDbType.VarChar)
        Dim Xcmd As New SqlCommand("select ISNULL(max(DDiCNProgInterno),0) from TbDDiCl where DDiCAnno =" & Anno, cnCo)
        Dim MaxAttrib As Integer = Xcmd.ExecuteScalar
        Dim GG As String = ""
        Dim MM As String = ""

        SCRIVI(0) = "Insert into TbDDiCl (DDiCAnno,DDiCNProgInterno,DDiCNProgDichiar,DDiCCli,DDiCCodEse,DDiCOpDa3,DDiCOpAa3,DDiCRicev,DDiCDataDoc,DDiImporto,DDiProtocollo) VALUES " _
                               & "(@DDiCAnno,@DDiCNProgInterno,@DDiCNProgDichiar,@DDiCCli,@DDiCCodEse,@DDiCOpDa3,@DDiCOpAa3,@DDiCRicev,@DDiCDataDoc,@DDiImporto,@DDiProtocollo)"

        SCRIVI(1) = "Update TbDDiCl set DDiCAnno=@DDiCAnno,DDiCNProgInterno=@DDiCNProgInterno,DDiCNProgDichiar=@DDiCNProgDichiar,DDiCCli=@DDiCCli,DDiCCodEse=@DDiCCodEse,DDiCOpDa3=@DDiCOpDa3,DDiCOpAa3=@DDiCOpAa3,DDiCRicev=@DDiCRicev,DDiCDataDoc=@DDiCDataDoc,DDiImporto=@DDiImporto,DDiProtocollo=@DDiProtocollo where DDiCId=@DDiCId "
        If Rifer = -1 Then
            N = 0
            ProgressInterno = MaxAttrib + 1
        End If

        Cmd = New SqlCommand(SCRIVI(N), cnCo)
        Cmd.Parameters.Clear()
        If Rifer > 0 Then
            p0.Value = Rifer
            Cmd.Parameters.Add(p0)
        End If
        p1.Value = Anno
        p2.Value = ProgressInterno
        p3.Value = TextEdit1.EditValue
        p4.Value = TextEdit20.EditValue
        p5.Value = Mid(ComboBoxEdit8.EditValue.ToString, 1, 2)
        GG = CDate(DateEdit3.EditValue).Day.ToString.PadLeft(2, "0")
        MM = CDate(DateEdit3.EditValue).Month.ToString.PadLeft(2, "0")
        DateEdit3.EditValue = CDate(GG & "/" & MM & "/" & Anno.ToString)
        p6.Value = DateEdit3.EditValue
        GG = CDate(DateEdit4.EditValue).Day.ToString.PadLeft(2, "0")
        MM = CDate(DateEdit4.EditValue).Month.ToString.PadLeft(2, "0")
        DateEdit4.EditValue = CDate(GG & "/" & MM & "/" & Anno.ToString)
        p7.Value = DateEdit4.EditValue
        p8.Value = DateEdit1.EditValue
        p9.Value = DateEdit2.EditValue
        p10.Value = TextEdit2.EditValue
        p11.Value = TextEdit3.EditValue
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
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            ButtonF8.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ButtonF3_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF3.Click
        If Rifer > 0 Then
            EliminaDichiarazione()
            ButtonF5.PerformClick()
        End If
    End Sub
    Sub EliminaDichiarazione()
        If MessageBox.Show("VUOI ELIMINARE DICHIARAZIONE INTENTO di : " & TextEdit20.EditValue & " " & TextEdit21.EditValue & " .... ? ", "ELIMINA DICHIARAZIONE :" & Anno, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Dim StrDlt As String = "Delete TbDDiCL where DDiCId = @ID"
        Dim p0 As New SqlParameter("@ID", SqlDbType.Int)
        p0.Value = Rifer
        Cmd = New SqlCommand(StrDlt, cnCo)
        Cmd.Parameters.Add(p0)
        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()
    End Sub
    Private Sub ButtonF9_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF9.Click
        Stampa()

    End Sub
    Sub Stampa()
        Dim REPORT As New DxRegLetInt
        REPORT.DataSource = DsDic
        REPORT.DataMember = "DsDic"
        REPORT.Parameters.Item("RagioneSociale").Value = IT(1)
        REPORT.Parameters.Item("Indirizzo").Value = IT(2)
        REPORT.Parameters.Item("Citta").Value = IT(3)
        REPORT.Parameters.Item("PivaCfis").Value = IT(0)
        REPORT.Parameters.Item("Anno").Value = Anno
        REPORT.CreateDocument()
        REPORT.ShowPreviewDialog()
    End Sub
End Class