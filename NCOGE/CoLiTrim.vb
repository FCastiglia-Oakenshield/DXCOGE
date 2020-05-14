Imports DevExpress.XtraEditors
Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Imports System.Linq
Imports System.Xml


Public Class CoLiTrim
    Dim DsMin, TbDte, TbDtr As DataTable
    Dim DaMin, DaDte, DaDtr As SqlDataAdapter
    Dim RwMin, RwDte, RwDtr As DataRow
    Dim ANNO As Int16 = 2017
    Dim Bloccato As Boolean = False
    Dim PathEle As String = ""
    Dim FileEle As String = ""
    Dim FileFatture As String = ""
    Dim Rispondi As MsgBoxResult
    Dim Title As String = "COMUNICAZIONE LIQUIDAZIONI PERIODICHE IVA"
    Dim VpEventi As String = ""
    Dim VpSuforn As Boolean = False
    Dim VpAuto As Decimal = 0
    Dim VPMese As Int16 = -1
    Dim VPAnno As Int16 = -1
    Dim VpOperazioni As Boolean = False
    Dim VPMetodo As Int16 = 0
    Dim Vp13 As Decimal = 0
    Dim Ko As Boolean = False
    Dim Trimestrale As Boolean = False
    Dim ProgrFile As Int16 = 0
    Dim Inizio, Massimo, Fine As Int16

    Dim PFisica As Boolean
    Dim Cloud As Boolean = False

    '' XML
    Dim Xtw As XmlTextWriter

    Private Sub CoLiTrim_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        SetInizio()
        LeggiAnagraficaAzienda()
    End Sub
    Sub SetInizio()
        Dim Contiene As String = ""
        XtraTabControl1.SelectedTabPageIndex = 0
        BottoniIniziali()
        Dim Cmd As New SqlCommand("SELECT distinct RivaAnno from TbRegIva where RivaAnno > 2016 Order by RivaAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()
        Dim x As Int16 = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("RIvaAnno"))
        End While
        dataRd.Close()
        If ComboBoxEdit1.Properties.Items.Count = 0 Then Exit Sub
        ComboBoxEdit1.SelectedIndex = 0
        Cmd = New SqlCommand("SELECT Sel8 from TbSel where SelId=1", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Contiene = dataRd.Item("Sel8").ToString.ToUpper
        End While
        dataRd.Close()
        Cloud = Contiene.Contains("\\TSCLIENT")
    End Sub
    Sub LeggiAnagraficaAzienda()
        Dim Str As String = "Select  * from tbazi inner join vdox.dbo.tbana on anacod = azicod  where AziAnnoLavoro = " & ComboBoxEdit1.EditValue
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read = True Then
            TextEdit2.EditValue = dataRd.Item("AnaCfis")
            TextEdit3.EditValue = dataRd.Item("AnaPiva")
            TextEdit9.EditValue = dataRd.Item("AnaDesc")
            TextEdit10.EditValue = dataRd.Item("AnaCitta")
            TextEdit11.EditValue = dataRd.Item("AnaProv")
            TextEdit16.EditValue = dataRd.Item("AziDrrCarica770")
            TextEdit19.EditValue = dataRd.Item("AziDrrCodiceFisc")
            If dataRd.Item("AziRegimeIva") = 0 Then Trimestrale = False Else Trimestrale = True
        End If
        dataRd.Close()
        If Trimestrale = True Then
            TextEdit4.Visible = True : TextEdit5.Visible = False : TextEdit6.Visible = False : CheckButton1.Visible = True : CheckButton3.Visible = False : CheckButton4.Visible = False
        Else
            TextEdit4.Visible = True : TextEdit5.Visible = True : TextEdit6.Visible = True : CheckButton1.Visible = True : CheckButton3.Visible = True : CheckButton4.Visible = True
        End If
    End Sub
    Private Sub NomeFiles()
        If Cloud = True Then
            PathEle = "\\TSCLIENT\C\LIQPER" & ComboBoxEdit1.EditValue & "\"
        Else
            PathEle = "C:\LIQPER" & ComboBoxEdit1.EditValue & "\"
        End If
        If Not Directory.Exists(PathEle) Then
            Directory.CreateDirectory(PathEle)
        End If
        FileEle = PathEle & "IT" & TextEdit3.EditValue & "_LI_" & Format(ProgrFile, "00000") & ".XML"
        If File.Exists(Trim(FileEle)) Then
            File.Delete(Trim(FileEle))
        End If

        TextEdit1.EditValue = FileEle
    End Sub
    Sub BottoniIniziali()
        RadioGroup1.Enabled = False
        CheckButton2.Checked = False
        CheckButton2.Enabled = False
        ButtonF11.Enabled = False
        RepositoryItemCheckEdit1.ReadOnly = True : RepositoryItemCheckEdit2.ReadOnly = True : RepositoryItemTextEdit3.ReadOnly = True : RepositoryItemTextEdit4.ReadOnly = True : RepositoryItemTextEdit6.ReadOnly = True
    End Sub
    Sub Pulizia()
        DsMin = New DataTable
        VGridControl1.DataSource = DsMin
        TextEdit1.EditValue = "" : TextEdit12.EditValue = "" : DateEdit1.EditValue = Nothing
        TextEdit4.Visible = False : TextEdit5.Visible = False : TextEdit6.Visible = False
        TextEdit4.EditValue = 0 : TextEdit5.EditValue = 0 : TextEdit6.EditValue = 0
        CheckButton1.Visible = False : CheckButton3.Visible = False : CheckButton4.Visible = False
        CheckButton1.Checked = False : CheckButton3.Checked = False : CheckButton4.Checked = False
        RadioGroup1.Properties.Items(0).Enabled = False : RadioGroup1.Properties.Items(1).Enabled = False
        RadioGroup1.Properties.Items(2).Enabled = False : RadioGroup1.Properties.Items(3).Enabled = False
        RadioGroup2.SelectedIndex = 1
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        RadioGroup1.SelectedIndex = -1
        RadioGroup1.Enabled = False
        Dim x As Int16
        x = ComboBoxEdit1.SelectedIndex
        If x = -1 Then Exit Sub
        ANNO = ComboBoxEdit1.EditValue
        Pulizia()
        LeggiAnagraficaAzienda()
        RendiTrimestriDisponibili()
        RadioGroup1.Enabled = True
        RadioGroup1.Focus()
    End Sub
    Sub RendiTrimestriDisponibili()
        Dim P As Int16
        Dim Cmd As New SqlCommand("Select * from TbVers where IvaVanno =" & ANNO & " and (ivavmese = 3 or ivavmese=6 or ivaVmese=9 or ivavmese=12)", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            P = (dataRd.Item("IvaVmese") / 3) - 1
            RadioGroup1.Properties.Items(P).Enabled = True
        End While
        dataRd.Close()
    End Sub
    Sub PrimaLettura()
        TextEdit1.EditValue = "" : TextEdit12.EditValue = "" : DateEdit1.EditValue = Nothing : Bloccato = False
        Dim Cmd = New SqlCommand("select isnull(Max(IcmProg),0) from TbInCmi", cnCo)
        ProgrFile = Cmd.ExecuteScalar + 1
        Cmd = New SqlCommand("Select * from TbInCmi where IcmAnno =" & ANNO & " and IcmTrim = " & RadioGroup1.EditValue, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Bloccato = dataRd.Item("IcmLock")
            TextEdit12.EditValue = dataRd.Item("IcmDrCodFisc")
            RadioGroup2.EditValue = dataRd.Item("IcmDrCaf")
            DateEdit1.EditValue = dataRd.Item("IcmDrImpegno")
        End While
        dataRd.Close()
        CheckButton2.Enabled = Bloccato
        CheckButton2.Checked = Bloccato
        CambiaVista()
        If Bloccato = True Then NomeFiles()
    End Sub
    Sub Messaggio(ByVal Tipo As Int16, ByVal Mexage As String)
        Dim style(2) As MsgBoxStyle
        style(1) = MsgBoxStyle.Exclamation
        style(0) = MsgBoxStyle.Critical
        style(2) = MsgBoxStyle.YesNo
        Rispondi = MsgBox(Mexage, style(Tipo), Title)
    End Sub
    Sub CambiaVista()
        If CheckButton2.Checked = False Then
            CheckButton2.ImageIndex = 19 : CheckButton2.ToolTip = "FILE XML APERTO" : LockButton(False)
        Else
            CheckButton2.ImageIndex = 18 : CheckButton2.ToolTip = "FILE XML CHIUSO" : LockButton(True)
        End If
    End Sub
    Private Sub CheckButton2_Click(sender As Object, e As System.EventArgs) Handles CheckButton2.Click
        If CheckButton2.Checked = True AndAlso CheckButton2.Enabled = True AndAlso AbilitaElenchi() = True Then
            CheckButton2.Checked = False : CambiaVista()
        End If
    End Sub
    Function AbilitaElenchi() As Boolean
        Dim P As New DxPwdDialog
        DxPwdDialog.Password = "LIQUIDA"
        P.ShowDialog()
        Return DxPwdDialog.Esatta
    End Function
    Sub LockButton(ByVal n As Boolean)
        ButtonF11.Enabled = Not n
        CheckButton2.Checked = n
        CheckButton2.Enabled = n
        Bloccato = n
        TextEdit12.Enabled = Not n
        DateEdit1.Enabled = Not n
        RepositoryItemCheckEdit1.ReadOnly = n : RepositoryItemCheckEdit2.ReadOnly = n : RepositoryItemTextEdit3.ReadOnly = n : RepositoryItemTextEdit4.ReadOnly = n : RepositoryItemTextEdit6.ReadOnly = n
        RadioGroup2.Enabled = Not n
    End Sub
    Sub LeggiTabella()
        PrimaLettura()
        EsegueSql("exec XCOMIVA @ANNO=" & ANNO & ",@T=" & RadioGroup1.EditValue, cnCo)
        Dim whe As String = " and VpMese between 1 and 3"
        Dim ch As Decimal = 0
        Dim Xpe As Int16 = 0
        Select Case RadioGroup1.EditValue
            Case 1
                whe = " and VpMese between 1 and 3"
                Xpe = 0
            Case 2
                whe = " and VpMese between 4 and 6"
                Xpe = 3
            Case 3
                whe = " and VpMese between 7 and 9"
                Xpe = 6
            Case 4
                whe = " and VpMese between 10 and 12"
                Xpe = 9
        End Select
        DsMin = New DataTable
        DaMin = New SqlDataAdapter("Select * from TbCmVp where VpAnno =" & ANNO & whe, cnCo)
        DaMin.SelectCommand.CommandTimeout = 300
        DaMin.Fill(DsMin)
        VGridControl1.DataSource = DsMin
        If Trimestrale = True Then Xpe = Xpe + 3 Else Xpe = Xpe + 1
        Cmd = New SqlCommand("select IvaVVersam from TbVers where IvaVAnno=" & ANNO & " and IvaVmese =" & Xpe, cnCo)
        TextEdit4.EditValue = Cmd.ExecuteScalar
        ch = DsMin.Rows(0).Item("Vp14")
        VPMetodo = DsMin.Rows(0).Item("VpMetodo")
        If ch = TextEdit4.EditValue Then CheckButton1.Checked = False : CheckButton1.ImageIndex = 31 Else CheckButton1.Checked = True : CheckButton1.ImageIndex = 27
        REM CONTROLLO METODO ACCONTO
        If ANNO > 2017 Then
            Vp13 = DsMin.Rows(0).Item("Vp13")
            If Vp13 > 0 And (VPMetodo < 1 Or VPMetodo > 4) Then CheckButton1.Checked = True : CheckButton1.ImageIndex = 27
        End If
        If Trimestrale = True Then GoTo okTrim
        Xpe = Xpe + 1
        Cmd = New SqlCommand("select IvaVVersam from TbVers where IvaVAnno=" & ANNO & " and IvaVmese =" & Xpe, cnCo)
        TextEdit5.EditValue = Cmd.ExecuteScalar
        ch = DsMin.Rows(1).Item("Vp14")
        If ch = TextEdit5.EditValue Then CheckButton3.Checked = False : CheckButton3.ImageIndex = 31 Else CheckButton3.Checked = True : CheckButton3.ImageIndex = 27
        Xpe = Xpe + 1
        Cmd = New SqlCommand("select IvaVVersam from TbVers where IvaVAnno=" & ANNO & " and IvaVmese =" & Xpe, cnCo)
        TextEdit6.EditValue = Cmd.ExecuteScalar
        ch = DsMin.Rows(2).Item("Vp14")
        If ch = TextEdit6.EditValue Then CheckButton4.Checked = False : CheckButton4.ImageIndex = 31 Else CheckButton4.Checked = True : CheckButton4.ImageIndex = 27
        REM CONTROLLO METODO ACCONTO
        If ANNO > 2017 Then
            Vp13 = DsMin.Rows(0).Item("Vp13")
            If Vp13 > 0 And (VPMetodo < 1 Or VPMetodo > 4) Then CheckButton4.Checked = True : CheckButton4.ImageIndex = 27
        End If
OkTrim:
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex = -1 Then Exit Sub
        LeggiTabella()
    End Sub
    Private Sub RadioGroup2_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioGroup2.SelectedIndexChanged
        If RadioGroup2.SelectedIndex = -1 Then Exit Sub
        GroupControl29.Enabled = RadioGroup2.EditValue : GroupControl31.Enabled = RadioGroup2.EditValue
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Controlli() = False Then Exit Sub
        Cursor.Current = Cursors.WaitCursor
        NomeFiles()
        '' CREAZIONE FILE XML
        CreazioneFileXml()
        RegistraIndici()
        Cursor.Current = Cursors.Default
        ButtonF11.Enabled = False
        CheckButton2.Checked = True
        Bloccato = True
        CambiaVista()

        'creazioneXMLFatture()

        TextEdit1.Focus()
    End Sub
    Function Controlli() As Boolean
        Controlli = True
        If RadioGroup1.SelectedIndex < 0 Then
            Messaggio(1, "MANCA SELEZIONE TRIMESTRE")
            Controlli = False
            Exit Function
        End If
        If CheckButton1.Checked = True Or CheckButton3.Checked = True Or CheckButton4.Checked = True Then
            Messaggio(1, "MANCA QUADRATURA o METODO CALCOLO VP13 ACCONTO DOVUTO")
            Controlli = False
            Exit Function
        End If


OOKK:
        If RadioGroup2.EditValue = False Then GoTo OOPS
        If TextEdit12.EditValue = "" Then
            Messaggio(1, "MANCA CODICE FISCALE INCARICATO INVIO")
            Controlli = False
        End If
        If TextEdit12.EditValue > "" And (DateEdit1.EditValue Is Nothing Or DateEdit1.EditValue Is DBNull.Value) Then
            Messaggio(1, "INSERIRE DATA IMPEGNO TRASMISSIONE")
            Controlli = False
        End If
OOPS:
        If Val(TextEdit16.EditValue) = 0 Then
            Messaggio(1, "MANCA CODICE CARICA")
            Controlli = False
        End If
        If Trim(TextEdit19.EditValue).Length <> 16 And TextEdit16.EditValue <> 9 Then
            Messaggio(1, "CONTROLLARE CODICE FISCALE LEGALE RAPPRESENTANTE")
            Controlli = False
        End If
        If Trim(TextEdit2.EditValue).Length <> 11 And Trim(TextEdit2.EditValue).Length <> 16 Then
            Messaggio(1, "MANCA CODICE FISCALE DEL CONTRIBUENTE")
            Controlli = False
        End If
        If Trim(TextEdit3.EditValue).Length <> 11 Then
            Messaggio(1, "MANCA PARTITA IVA DEL CONTRIBUENTE")
            Controlli = False
        End If

    End Function
    Sub RegistraIndici()
        If RadioGroup1.EditValue <= 0 Then Exit Sub
        Cmd = New SqlCommand("delete from TbInCmi where IcmAnno =" & ANNO & " and IcmTrim = " & RadioGroup1.EditValue, cnCo) ''''' molto pericoloso'''' da rivedere
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("INSERT INTO TbInCmi (IcmAnno,IcmTrim,IcmLock,IcmDrCodFisc,IcmDrCaf,IcmDrImpegno,IcmProg) VALUES (@A1,@A2,@A3,@A4,@A5,@A6,@A7)", cnCo)
        Dim A1 As New SqlParameter("@A1", SqlDbType.SmallInt)
        Dim A2 As New SqlParameter("@A2", SqlDbType.NVarChar)
        Dim A3 As New SqlParameter("@A3", SqlDbType.Bit)
        Dim A4 As New SqlParameter("@A4", SqlDbType.NVarChar)
        Dim A5 As New SqlParameter("@A5", SqlDbType.NVarChar)
        Dim A6 As New SqlParameter("@A6", SqlDbType.SmallDateTime)
        Dim A7 As New SqlParameter("@A7", SqlDbType.SmallInt)
        A1.Value = ANNO
        A2.Value = RadioGroup1.EditValue
        A3.Value = 1
        A4.Value = TextEdit12.EditValue
        A5.Value = RadioGroup2.EditValue
        If DateEdit1.EditValue Is Nothing Then A6.Value = DBNull.Value Else A6.Value = DateEdit1.EditValue
        A7.Value = ProgrFile
        Cmd.Parameters.Add(A1)
        Cmd.Parameters.Add(A2)
        Cmd.Parameters.Add(A3)
        Cmd.Parameters.Add(A4)
        Cmd.Parameters.Add(A5)
        Cmd.Parameters.Add(A6)
        Cmd.Parameters.Add(A7)
        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()

        RegistraProgressivo()
    End Sub
    Private Sub RegistraProgressivo()
        Cmd = New SqlCommand("Update TbInCmi set IcmProg=" & ProgrFile, cnCo)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub RepositoryItemCheckEdit1_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemCheckEdit1.EditValueChanged
        VpSuforn = CType(sender, DevExpress.XtraEditors.CheckEdit).Checked
    End Sub
    Private Sub RepositoryItemCheckEdit2_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemCheckEdit2.EditValueChanged
        VpOperazioni = CType(sender, DevExpress.XtraEditors.CheckEdit).Checked
    End Sub

    Private Sub RepositoryItemTextEdit3_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit3.EditValueChanged
        VpEventi = ""
        If Not VGridControl1.ActiveEditor Is Nothing Then
            VpEventi = VGridControl1.EditingValue.ToString()
            If VpEventi <> "" And VpEventi <> "1" And VpEventi <> "9" Then VGridControl1.EditingValue = ""
        End If
    End Sub

    Private Sub RepositoryItemTextEdit4_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit4.EditValueChanged
        VpAuto = 0.0
        If Not VGridControl1.ActiveEditor Is Nothing Then
            VpAuto = VGridControl1.EditingValue()
            If VpAuto > 0 Then VGridControl1.EditingValue = 0.0
        End If
    End Sub
    Private Sub RepositoryItemTextEdit6_EditValueChanged(sender As Object, e As System.EventArgs) Handles RepositoryItemTextEdit6.EditValueChanged
        VPMetodo = 0
        If Not VGridControl1.ActiveEditor Is Nothing Then
            VPMetodo = VGridControl1.EditingValue()
            If VPMetodo > 4 Then VGridControl1.EditingValue = 0
        End If
    End Sub

    Sub AggiornaElementi()
        Dim Str As String = "Update TbCmVp set Vp10 =@Vp10,VpSub =@VpSub,VpEventi=@VpEventi,VpOperazioni =@VpOperazioni,VpMetodo=@VpMetodo where VpAnno = @VpAnno and VpMese =@VpMese"
        Dim p0 As New SqlParameter("@VpAnno", SqlDbType.SmallInt)
        Dim p1 As New SqlParameter("@VpMese", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@VpSub", SqlDbType.Bit)
        Dim p3 As New SqlParameter("@VpEventi", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@Vp10", SqlDbType.Decimal)
        Dim p5 As New SqlParameter("@VpOperazioni", SqlDbType.Bit)
        Dim p6 As New SqlParameter("@VpMetodo", SqlDbType.SmallInt)
        Cmd = New SqlCommand(Str, cnCo)
        p0.Value = VPAnno
        p1.Value = VPMese
        p2.Value = VpSuforn ''= True Then p2.Value = 1 Else p2.Value = 0
        p3.Value = VpEventi
        p4.Value = VpAuto
        p5.Value = VpOperazioni
        p6.Value = VPMetodo
        Cmd.Parameters.Add(p0)
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.Parameters.Add(p6)
        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()
    End Sub

    Private Sub VGridControl1_RecordUpdated(sender As Object, e As DevExpress.XtraVerticalGrid.Events.RecordObjectEventArgs) Handles VGridControl1.RecordUpdated
        VpSuforn = DirectCast(e.Record, System.Data.DataRowView).Item("VpSub")
        If DirectCast(e.Record, System.Data.DataRowView).Item("Vp10") Is DBNull.Value Then VpAuto = 0.0 Else VpAuto = DirectCast(e.Record, System.Data.DataRowView).Item("Vp10")
        VpEventi = DirectCast(e.Record, System.Data.DataRowView).Item("VpEventi")
        VPMese = DirectCast(e.Record, System.Data.DataRowView).Item("VpMese")
        VPAnno = DirectCast(e.Record, System.Data.DataRowView).Item("VpAnno")
        VpOperazioni = DirectCast(e.Record, System.Data.DataRowView).Item("VpOperazioni")
        VPMetodo = DirectCast(e.Record, System.Data.DataRowView).Item("VPMetodo")
        AggiornaElementi()
        LeggiTabella()
    End Sub
    Private Sub RepositoryItemTextEdit5_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles RepositoryItemTextEdit5.KeyDown
        SendKeys.Send("{Left}")
    End Sub
    Sub CreazioneFileXml()
        Xtw = New XmlTextWriter(TextEdit1.EditValue.ToString, System.Text.Encoding.UTF8)
        Xtw.Formatting = Formatting.Indented
        Xtw.Indentation = 2
        Xtw.WriteStartDocument()
        Xtw.WriteStartElement("Fornitura")
        Xtw.WriteAttributeString("xmlns", "urn:www.agenziaentrate.gov.it:specificheTecniche:sco:ivp")
        Xtw.WriteAttributeString("xmlns", "ds", Nothing, "http://www.w3.org/2000/09/xmldsig#")

        'Xtw.WriteAttributeString("xmlns", "xsi", Nothing, "http://www.w3.org/2001/XMLSchema-instance")
        'Xtw.WriteAttributeString("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "fornituraIvp_2017_v1.xsd")
        '''''''Xtw.WriteAttributeString("xmlns", "xs", Nothing, "http://www.w3.org/2001/XMLSchema")
        '''''''Xtw.WriteAttributeString("xmlns", "cm", Nothing, "urn:www.agenziaentrate.gov.it:specificheTecniche:common")
        '''''''Xtw.WriteAttributeString("xmlns", "sc", Nothing, "urn:www.agenziaentrate.gov.it:specificheTecniche:sco:common")
        '''''''Xtw.WriteAttributeString("xmlns", "iv", Nothing, "urn:www.agenziaentrate.gov.it:specificheTecniche:sco:ivp")
        '''''''Xtw.WriteAttributeString("xmlns", "ds", Nothing, "http://www.w3.org/2000/09/xmldsig#")
        '''''''Xtw.WriteAttributeString("targetNamespace", "urn:www.agenziaentrate.gov.it:specificheTecniche:sco:ivp")
        '''''''Xtw.WriteAttributeString("elementFormDefault", "qualified")
        '''''''Xtw.WriteAttributeString("attributeFormDefault", "unqualified")
        '''''''Xtw.WriteAttributeString("version", "1.0")
        'Xtw.WriteAttributeString("xs", "schema")
        ScriviIntestazione()
        ScriviComunicazione()
        For x As Int16 = 1 To DsMin.Rows.Count
            Rw = DsMin.Rows(x - 1)
            ScriviModulo(x)
        Next
        Xtw.WriteEndElement()
        Xtw.WriteEndElement()
        Xtw.WriteEndElement()
        Xtw.WriteEndDocument()
        Xtw.Close()
    End Sub
    Sub ScriviIntestazione()
        Xtw.WriteStartElement("Intestazione")
        ''Xtw.WriteAttributeString("xmlns", "urn:www.agenziaentrate.gov.it:specificheTecniche:sco:ivp")
        ''Xtw.WriteAttributeString("xmlns", "xsi", Nothing, "http://www.w3.org/2001/XMLSchema-instance")
        ''Xtw.WriteAttributeString("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "intestazioneIvp_2017_v1.xsd")
        Xtw.WriteElementString("CodiceFornitura", "IVP18")
        Xtw.WriteElementString("CodiceFiscaleDichiarante", TextEdit19.EditValue.ToString)
        Xtw.WriteElementString("CodiceCarica", TextEdit16.EditValue.ToString)
        '''''''' Xtw.WriteElementString("IdSistema", "")
        Xtw.WriteEndElement()
    End Sub
    Sub ScriviComunicazione()
        Xtw.WriteStartElement("Comunicazione")
        ''Xtw.WriteAttributeString("xmlns", "urn:www.agenziaentrate.gov.it:specificheTecniche:sco:ivp")
        ''Xtw.WriteAttributeString("xmlns", "xsi", Nothing, "http://www.w3.org/2001/XMLSchema-instance")
        ''Xtw.WriteAttributeString("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "comunicazioneIvp_2017_v1.xsd")
        Xtw.WriteAttributeString("identificativo", Format(RadioGroup1.EditValue, "00000"))
        Xtw.WriteStartElement("Frontespizio")
        Xtw.WriteElementString("CodiceFiscale", TextEdit2.EditValue.ToString)
        Xtw.WriteElementString("AnnoImposta", ANNO)
        Xtw.WriteElementString("PartitaIVA", TextEdit3.EditValue.ToString)
        Xtw.WriteElementString("CFDichiarante", TextEdit19.EditValue.ToString)
        Xtw.WriteElementString("CodiceCaricaDichiarante", TextEdit16.EditValue.ToString)
        If TextEdit16.EditValue.ToString = "9" Then Xtw.WriteElementString("CodiceFiscaleSocieta", TextEdit19.EditValue.ToString)
        Xtw.WriteElementString("FirmaDichiarazione", 1) '' da definire
        If RadioGroup2.EditValue = True Then
            Xtw.WriteElementString("CFIntermediario", TextEdit12.EditValue.ToString)
            Xtw.WriteElementString("ImpegnoPresentazione", 1) '' da definire
            Xtw.WriteElementString("DataImpegno", Format(DateEdit1.EditValue, "ddMMyyyy"))
            Xtw.WriteElementString("FirmaIntermediario", 1)
        End If
        Xtw.WriteElementString("IdentificativoProdSoftware", "04394270013")
        Xtw.WriteEndElement()
        Xtw.WriteStartElement("DatiContabili")
    End Sub
    Sub ScriviModulo(N As Int16)
        Xtw.WriteStartElement("Modulo")
        Xtw.WriteElementString("NumeroModulo", N)
        If Trimestrale = False Then
            Xtw.WriteElementString("Mese", Rw("VpMese"))
        Else
            Xtw.WriteElementString("Trimestre", Rw("Vp1")) ''' occhio al 4 trimestre
        End If
        If Rw("VpSub") = True Then Xtw.WriteElementString("Subfornitura", "1")
        If Rw("VpEventi") = "1" Or Rw("VpEventi") = "9" Then
            Xtw.WriteElementString("EventiEccezionali", Rw("VpEventi"))
        End If
        If Rw("VpOperazioni") = True Then Xtw.WriteElementString("OperazioniStraordinarie", "1")
        If Rw("Vp2") <> 0 Then Xtw.WriteElementString("TotaleOperazioniAttive", Rw("Vp2").ToString.Replace(".", ","))
        If Rw("Vp3") <> 0 Then Xtw.WriteElementString("TotaleOperazioniPassive", Rw("Vp3").ToString.Replace(".", ","))
        If Rw("Vp4") <> 0 Then Xtw.WriteElementString("IvaEsigibile", Rw("Vp4").ToString.Replace(".", ","))
        If Rw("Vp5") <> 0 Then Xtw.WriteElementString("IvaDetratta", Rw("Vp5").ToString.Replace(".", ","))
        If Rw("Vp6") > 0 Then Xtw.WriteElementString("IvaDovuta", Rw("Vp6").ToString.Replace(".", ","))
        If Rw("Vp6") < 0 Then Xtw.WriteElementString("IvaCredito", (Rw("Vp6") * -1).ToString.Replace(".", ","))
        If Rw("Vp7") > 0 Then Xtw.WriteElementString("DebitoPrecedente", Rw("Vp7").ToString.Replace(".", ","))
        If Rw("Vp8") < 0 Then Xtw.WriteElementString("CreditoPeriodoPrecedente", (Rw("Vp8") * -1).ToString.Replace(".", ","))
        If Rw("Vp9") > 0 Then Xtw.WriteElementString("CreditoAnnoPrecedente", (Rw("Vp9") * -1).ToString.Replace(".", ",")) '' PER LA GESTIONE DEL RECUPERO IMPORTO UTILIZZATO PER ALTRE IMPOSTE
        If Rw("Vp9") < 0 Then Xtw.WriteElementString("CreditoAnnoPrecedente", (Rw("Vp9") * -1).ToString.Replace(".", ","))
        If Rw("Vp10") < 0 Then Xtw.WriteElementString("VersamentiAutoUE", (Rw("Vp10") * -1).ToString.Replace(".", ","))
        If Trimestrale = True And Rw("Vp1") = 5 Then GoTo NoVp11e12
        If Rw("Vp11") < 0 Then Xtw.WriteElementString("CreditiImposta", (Rw("Vp11") * -1).ToString.Replace(".", ","))
        If Rw("Vp12") > 0 Then Xtw.WriteElementString("InteressiDovuti", Rw("Vp12").ToString.Replace(".", ","))
NoVP11e12:
        If Rw("VpMetodo") > 0 And Rw("Vp13") > 0 Then Xtw.WriteElementString("Metodo", Rw("VpMetodo"))
        If Rw("Vp13") > 0 Then Xtw.WriteElementString("Acconto", Rw("Vp13").ToString.Replace(".", ","))
        If Trimestrale = True And Rw("Vp1") = 5 Then GoTo NoVp14
        If Rw("Vp14") > 0 Then Xtw.WriteElementString("ImportoDaVersare", Rw("Vp14").ToString.Replace(".", ","))
        If Rw("Vp14") < 0 Then Xtw.WriteElementString("ImportoACredito", (Rw("Vp14") * -1).ToString.Replace(".", ","))
NoVp14:
        Xtw.WriteEndElement()
    End Sub
#Region "XML FATTURE"
    Private Sub creazioneXMLFatture()

        Inizio = 1

        While Inizio < TbDte.Rows.Count
            CreaFatture(1)
            Inizio = Inizio + 1000
        End While


        Inizio = 1

        While Inizio < TbDtr.Rows.Count
            CreaFatture(2)
            Inizio = Inizio + 1000
        End While


    End Sub

    Private Sub CreaFatture(Tipo As Int16)
        ProgrFile = ProgrFile + 1
        FileFatture = PathEle & "IT" & TextEdit3.EditValue & "_DF_" & Format(ProgrFile, "00000") & ".XML"
        If File.Exists(Trim(FileEle)) Then
            File.Delete(Trim(FileEle))
        End If
        TextEdit1.EditValue = FileFatture

        Xtw = New XmlTextWriter(TextEdit1.EditValue.ToString, System.Text.Encoding.UTF8)
        Xtw.Formatting = Formatting.Indented
        Xtw.Indentation = 2
        Xtw.WriteStartDocument()
        Xtw.WriteStartElement("DatiFattura")
        Xtw.WriteAttributeString("xmlns", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v2.0")
        Xtw.WriteAttributeString("xmlns", "ds", Nothing, "http://www.w3.org/2000/09/xmldsig#")
        Xtw.WriteAttributeString("NamespaceSchemaLocation", "http://www.w3.org/2000/09/xmldsig#", "xmldsig-core-schema.xsd")
        Xtw.WriteAttributeString("targetNamespace", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v2.0")
        Xtw.WriteAttributeString("version", "1.0")

        If RadioGroup2.EditValue = True Then
            CaricaHeader()
        End If

        If Tipo = 1 Then
            caricaDTE()
        Else
            CaricaDTR()
        End If

        Xtw.WriteEndElement()
        Xtw.WriteEndDocument()
        Xtw.Close()

        RegistraProgressivo()
    End Sub
    Private Sub CaricaHeader()
        Xtw.WriteStartElement("DatiFatturaHeader")
        Xtw.WriteStartElement("Dichiarante")

        Xtw.WriteElementString("CodiceFiscale", TextEdit12.EditValue.ToString)
        Xtw.WriteElementString("Carica", TextEdit16.EditValue.ToString)

        Xtw.WriteEndElement()
        Xtw.WriteEndElement()

    End Sub
    Private Sub caricaDTE()
        Xtw.WriteStartElement("DTE")

        CaricaCedentePrestatoreDTE()

        For i As Int16 = Inizio To TbDte.Rows.Count
            If i > (Inizio + 999) Then
                GoTo esci
            End If
            RwDte = TbDte.Rows(i - 1)
            CaricaCessionarioCommittenteDTE(RwDte)
        Next

esci:
        Xtw.WriteEndElement()
    End Sub
    Private Sub CaricaCedentePrestatoreDTE()
        Xtw.WriteStartElement("CedentePrestatoreDTE")

        'Id Fiscali
        Xtw.WriteStartElement("IdentificativiFiscali")

        Xtw.WriteStartElement("IdFiscaleIVA")
        Xtw.WriteElementString("IdPaese", "IT")
        Xtw.WriteElementString("IdCodice", TextEdit3.Text.Trim)
        Xtw.WriteEndElement()

        Xtw.WriteElementString("CodiceFiscale", TextEdit2.Text.Trim)

        Xtw.WriteEndElement()


        'Altri identificativi.
        Xtw.WriteStartElement("AltriDatiIdentificativi")

        If PFisica = False Then
            Xtw.WriteElementString("Denominazione", TextEdit9.Text.Trim)
        Else
            Dim NC() As String = TextEdit9.Text.Trim.Split(" ")
            Dim COGNOME As String = NC(0)
            Dim NOME As String = TextEdit9.Text.Trim.Remove(0, COGNOME.Length)
            Xtw.WriteElementString("Nome", NOME)
            Xtw.WriteElementString("Cognome", COGNOME)
        End If

        Xtw.WriteEndElement()

        Xtw.WriteEndElement()
    End Sub
    Private Sub CaricaCessionarioCommittenteDTE(rw As DataRow)
        Xtw.WriteStartElement("CessionarioCommittenteDTE")

        'Id Fiscali
        Xtw.WriteStartElement("IdentificativiFiscali")

        Xtw.WriteStartElement("IdFiscaleIVA")
        Xtw.WriteElementString("IdPaese", "IT")
        Xtw.WriteElementString("IdCodice", rw("CliPartIva"))
        Xtw.WriteEndElement()

        Xtw.WriteElementString("CodiceFiscale", rw("CliCFis"))

        Xtw.WriteEndElement()

        'Altri identificativi.
        Xtw.WriteStartElement("AltriDatiIdentificativi")

        If rw("PFisica") = False Then
            Xtw.WriteElementString("Denominazione", rw("AnaDesc"))
        Else
            Xtw.WriteElementString("Nome", rw("AnaRag2"))
            Xtw.WriteElementString("Cognome", rw("AnaRag1"))
        End If
        Xtw.WriteStartElement("Sede")
        Xtw.WriteElementString("Indirizzo", rw("AnaIndirizzo"))
        Xtw.WriteElementString("Comune", rw("AnaCitta"))
        Xtw.WriteElementString("Nazione", rw("Nazione"))
        Xtw.WriteEndElement()

        Xtw.WriteEndElement()


        'DATI FATTURA
        Xtw.WriteStartElement("DatiFatturaBodyDTE")

        'Dati Generali
        Xtw.WriteStartElement("DatiGenerali")
        Xtw.WriteElementString("TipoDocumento", rw("TipoDocumento"))
        Xtw.WriteElementString("Data", rw("Data"))
        Xtw.WriteElementString("Numero", rw("Numero"))
        Xtw.WriteEndElement()

        'Riepilogo
        For i As Int16 = 1 To 12
            RiepilogoDTE(i)
        Next
        Xtw.WriteEndElement()



        Xtw.WriteEndElement()
    End Sub

    Private Sub RiepilogoDTE(i As Int16)
        Xtw.WriteStartElement("DatiRiepilogo")

        Xtw.WriteElementString("ImponibileImporto", Rw("ImponibileImporto" & i.ToString))

        Xtw.WriteStartElement("DatiIVA")
        Xtw.WriteElementString("Imposta", Rw("Imposta" & i.ToString))
        Xtw.WriteElementString("Aliquota", Rw("Aliquota" & i.ToString))
        Xtw.WriteEndElement()

        If Rw("Imposta" & i.ToString) = 0 Then
            Xtw.WriteElementString("Natura", Rw("Natura" & i.ToString))
        End If
        If Rw("Detraibile" & i.ToString) > 0 Then
            Xtw.WriteElementString("Detraibile", Rw("Detraibile" & i.ToString))
        End If
        If Rw("Deducibile" & i.ToString) = True Then
            Xtw.WriteElementString("Deducibile", "SI")
        End If
        Xtw.WriteElementString("EsigibilitaIVA", Rw("EsigibilitaIVA" & i.ToString))

        Xtw.WriteEndElement()
    End Sub
    Private Sub CaricaDTR()
        Xtw.WriteStartElement("DTE")

        CaricaCessionarioCommittenteDTR()


        For i As Int16 = Inizio To TbDtr.Rows.Count
            If i > (Inizio + 999) Then
                GoTo esci
            End If
            RwDtr = TbDtr.Rows(i - 1)
            CaricaCessionarioCommittenteDTE(RwDtr)
        Next

esci:
        Xtw.WriteEndElement()
    End Sub

    Private Sub CaricaCessionarioCommittenteDTR()
        Xtw.WriteStartElement("CessionarioCommittenteDTR")

        'Id Fiscali
        Xtw.WriteStartElement("IdentificativiFiscali")

        Xtw.WriteStartElement("IdFiscaleIVA")
        Xtw.WriteElementString("IdPaese", "IT")
        Xtw.WriteElementString("IdCodice", TextEdit3.Text.Trim)
        Xtw.WriteEndElement()

        Xtw.WriteElementString("CodiceFiscale", TextEdit2.Text.Trim)

        Xtw.WriteEndElement()


        'Altri identificativi.
        Xtw.WriteStartElement("AltriDatiIdentificativi")

        If PFisica = False Then
            Xtw.WriteElementString("Denominazione", TextEdit9.Text.Trim)
        Else
            Dim NC() As String = TextEdit9.Text.Trim.Split(" ")
            Dim COGNOME As String = NC(0)
            Dim NOME As String = TextEdit9.Text.Trim.Remove(0, COGNOME.Length)
            Xtw.WriteElementString("Nome", NOME)
            Xtw.WriteElementString("Cognome", COGNOME)
        End If

        Xtw.WriteEndElement()

        Xtw.WriteEndElement()
    End Sub

    Private Sub CaricaCedentePrestatoreDTR(rw As DataRow)
        Xtw.WriteStartElement("CedentePrestatoreDTR")

        'Id Fiscali
        Xtw.WriteStartElement("IdentificativiFiscali")

        Xtw.WriteStartElement("IdFiscaleIVA")
        Xtw.WriteElementString("IdPaese", "IT")
        Xtw.WriteElementString("IdCodice", rw("ForPartIva"))
        Xtw.WriteEndElement()

        Xtw.WriteElementString("CodiceFiscale", rw("ForCFis"))

        Xtw.WriteEndElement()

        'Altri identificativi.
        Xtw.WriteStartElement("AltriDatiIdentificativi")

        If rw("PFisica") = False Then
            Xtw.WriteElementString("Denominazione", rw("AnaDesc"))
        Else
            Xtw.WriteElementString("Nome", rw("AnaRag2"))
            Xtw.WriteElementString("Cognome", rw("AnaRag1"))
        End If
        Xtw.WriteStartElement("Sede")
        Xtw.WriteElementString("Indirizzo", rw("AnaIndirizzo"))
        Xtw.WriteElementString("Comune", rw("AnaCitta"))
        Xtw.WriteElementString("Nazione", rw("Nazione"))
        Xtw.WriteEndElement()

        Xtw.WriteEndElement()


        'DATI FATTURA
        Xtw.WriteStartElement("DatiFatturaBodyDTR")

        'Dati Generali
        Xtw.WriteStartElement("DatiGenerali")
        Xtw.WriteElementString("TipoDocumento", rw("TipoDocumento"))
        Xtw.WriteElementString("Data", rw("Data"))
        Xtw.WriteElementString("Numero", rw("Numero"))
        Xtw.WriteElementString("DataRegistrazione", rw("DataRegistrazione"))
        Xtw.WriteEndElement()

        'Riepilogo
        For i As Int16 = 1 To 12
            RiepilogoDTR(i)
        Next
        Xtw.WriteEndElement()



        Xtw.WriteEndElement()
    End Sub
    Private Sub RiepilogoDTR(i As Int16)
        Xtw.WriteStartElement("DatiRiepilogo")

        Xtw.WriteElementString("ImponibileImporto", Rw("ImponibileImporto" & i.ToString))

        Xtw.WriteStartElement("DatiIVA")
        Xtw.WriteElementString("Imposta", Rw("Imposta" & i.ToString))
        Xtw.WriteElementString("Aliquota", Rw("Aliquota" & i.ToString))
        Xtw.WriteEndElement()

        If Rw("Imposta" & i.ToString) = 0 Then
            Xtw.WriteElementString("Natura", Rw("Natura" & i.ToString))
        End If
        If Rw("Detraibile" & i.ToString) > 0 Then
            Xtw.WriteElementString("Detraibile", Rw("Detraibile" & i.ToString))
        End If
        If Rw("Deducibile" & i.ToString) = True Then
            Xtw.WriteElementString("Deducibile", "SI")
        End If
        Xtw.WriteElementString("EsigibilitaIVA", Rw("EsigibilitaIVA" & i.ToString))

        Xtw.WriteEndElement()
    End Sub
#End Region


End Class