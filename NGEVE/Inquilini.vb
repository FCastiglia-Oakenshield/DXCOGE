Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit

Public Class Inquilini
    Dim OkImm, okInq As Boolean
    Dim rifer As Int32
    Dim RifAff As Int16 ' RIFERIMENTO DELL'ARCHIVIO AFFITTI
    Dim FL, SW As Boolean
    Dim Irow As Integer

    Dim TbCMG As DataTable
    Dim DaCMG As SqlDataAdapter
    Dim RwD As DataRowView
    Dim NomeFile As String = "C:\ROSINE\TestContratti\"
    Dim Nb As Int16 = 2 '' BANCA PROSSIMA
    Dim Paragrafo(50) As String
    Dim TbDoc As DataTable
    Dim DaDoc As SqlDataAdapter
    Dim Rxw As DataRow
    Dim COMUNE As String = ""
    Dim PROV As String = ""
    Dim SESSO As String = ""
    Dim Cap As String = ""
    Dim DataN As Date
    Dim CodSia As String
    Dim TipoModulo As Int16 = -1
    Dim Canone As Decimal = 0
    Dim Intero As Int32 = 0
    Dim Resto As Int16 = 0
    Dim Valori As String = ""
    Dim MeseRisc(13) As String


    Private Sub Inquilini_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonReset.Click
        FL = False
        PULIZIA()
        PulisciImmobile()
        TextEdit1.Focus()
    End Sub
    Sub PULIZIA()
        TextEdit1.Text = "" : TextEdit2.Text = "" : TextEdit3.Text = "" : TextEdit4.Text = "" : TextEdit5.Text = "" : TextEdit6.Text = ""
        TextEdit10.Text = "" : TextEdit11.Text = "" : TextEdit12.Text = "" : TextEdit13.Text = ""
        TextEdit7.Text = "" : TextEdit8.Text = "" : TextEdit9.Text = ""
        TextEdit14.Text = "" : TextEdit15.Text = "" : TextEdit16.Text = "" : TextEdit35.Text = ""
        MemoEdit1.Text = ""
        TextEdit36.EditValue = "" : TextEdit37.EditValue = CDec(0.0) : TextEdit39.EditValue = 0 : TextEdit38.EditValue = CDec(0.0)
        TextEdit18.EditValue = CDec(0.0) : TextEdit17.EditValue = CDec(0.0)
        TextEdit19.EditValue = CDec(0.0) : TextEdit20.EditValue = CDec(0.0)
        TextEdit21.EditValue = CDec(0.0)
        DateEdit1.EditValue = Today
        DateEdit2.EditValue = Today
        DateEdit3.EditValue = Today
        DateEdit4.EditValue = Today
        CaricaComboBox()
        ComboBoxEdit3.SelectedIndex = -1 : ComboBoxEdit4.SelectedIndex = -1 : ComboBoxEdit2.SelectedIndex = -1 : ComboBoxEdit5.SelectedIndex = -1 : ComboBoxEdit1.SelectedIndex = -1 : ComboBoxEdit7.SelectedIndex = -1
        Disabilita()
        TbCMG = New DataTable
        GridControl3.DataSource = TbCMG
        TbLeggiCli.Enabled = True
    End Sub
    Sub Disabilita()
        GroupControl2.Enabled = False
        GroupControl3.Enabled = False
        GroupControl4.Enabled = False
        GroupControl5.Enabled = False
        GroupControl6.Enabled = False
        GroupControl7.Enabled = False
        GroupControl8.Enabled = False
        GroupControl20.Enabled = False
        Irow = -1
        ButtonF11.Enabled = False
        ButtonF5.Enabled = True
        ButtonF3.Enabled = False
        ButtonF8.Enabled = True
    End Sub

    Sub Abilita()
        GroupControl1.Enabled = True
        GroupControl20.Enabled = True

        GroupControl2.Enabled = True
        GroupControl3.Enabled = True
        GroupControl4.Enabled = True

        GroupControl5.Enabled = True
        GroupControl6.Enabled = True
        GroupControl8.Enabled = True

        GroupControl7.Enabled = True

        ButtonF8.Enabled = False
        ButtonF3.Enabled = True
        ButtonF11.Enabled = True
        ButtonF5.Enabled = True
        TextEdit32.Focus()
        ' If RifAff > 0 Then ButtonF3.Enabled = True
    End Sub
    Sub CaricaComboBox()
        'Lettura Tipo
        ComboBoxEdit3.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT distinct AffTipo from TbAffitti Order by AffTipo", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit3.Properties.Items.Add(dataRd.Item("AffTipo"))
        End While
        dataRd.Close()
        'Lettura Durata
        ComboBoxEdit4.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT distinct AffDurata from TbAffitti Order by AffDurata", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit4.Properties.Items.Add(dataRd.Item("AffDurata"))
        End While
        dataRd.Close()
        'Lettura Pagare a
        ComboBoxEdit2.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT distinct AffPagareA from TbAffitti Order by AffPagareA", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit2.Properties.Items.Add(dataRd.Item("AffPagareA"))
        End While
        dataRd.Close()
        'Lettura Inviate da
        ComboBoxEdit5.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT distinct AffInviateDa from TbAffitti Order by AffInviateDa", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit5.Properties.Items.Add(dataRd.Item("AffInviateDa"))
        End While
        dataRd.Close()
        'TipoContrattiAbilitati
        ComboBoxEdit7.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT * from TbIndexModelli Order by ID", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("ID") = 9999 Then
                NomeFile = dataRd.Item("IdPath_Nome")
            Else
                ComboBoxEdit7.Properties.Items.Add(dataRd.Item("IdPath_Nome"))
            End If
        End While
        dataRd.Close()
        'ComboBoxEdit7.Properties.Items.Clear()
        'ComboBoxEdit7.Properties.Items.Add("Modello3+2Bon.docx")
        'ComboBoxEdit7.Properties.Items.Add("Modello3+2SddPlus.docx")
        'ComboBoxEdit7.Properties.Items.Add("ModelloBoxBon.docx")
        'Banca Prossima
        Cmd = New SqlCommand("SELECT * from TbBan where BanCod = " & Nb, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CodSia = "Cod. SIA " & dataRd.Item("BanSia") & ", Cod.Creditore " & dataRd.Item("BanCreditorId")
        End While
        dataRd.Close()
        'Campi mese riscaldamento
        MeseRisc(0) = "annuale" : MeseRisc(1) = "ottobre" : MeseRisc(2) = "novembre" : MeseRisc(3) = "dicembre" : MeseRisc(4) = "gennaio" : MeseRisc(5) = "febbraio" : MeseRisc(6) = "marzo"
        MeseRisc(7) = "aprile" : MeseRisc(8) = "maggio" : MeseRisc(9) = "giugno" : MeseRisc(10) = "luglio" : MeseRisc(11) = "agosto" : MeseRisc(12) = "settembre"
    End Sub
    Sub LeggiImmobiliInquilino()
        Dim Str As String = "Select AffRif,ImmCod,ImmCitta,ImmIndirizzo,ImmPiano,ImmCateg,AffTipo,AffDurata,AffIniLoc,AffRegAnn,AffScaProroga,AffFineContr from VAffitti where AffCliente = " & Val(TextEdit1.Text) & " order by ImmCitta,ImmIndirizzo, ImmPiano desc"
        TbCMG = New DataTable()
        DaCMG = New SqlDataAdapter(Str, cnDb)
        DaCMG.Fill(TbCMG)
        GridControl3.DataSource = TbCMG
        GridView4.ClearSelection()
        GridView4.UnselectRow(0)
    End Sub

    Sub ArcImm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyData = Keys.F3 Then
            e.Handled = True
            ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
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
    Sub TbLeggiImm_ENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggiImm.Enter
        'leggo Immobile
        SW = False
        If Val(TextEdit32.Text) > 0 Then LeggiImm(Val(TextEdit32.Text))
        ControllaArcImm()
        If SW = False Then TextEdit32.Focus() : Exit Sub

        CaricaImmobile()
        CaricaAffitti()
        'LeggiAffitti()
        Abilita()
        ComboBoxEdit3.Focus()
        'If RifAff > 0 Then ' record presente nel TbAff
        '    ButtonF5.Focus()
        'Else
        '    ComboBoxEdit3.Focus()
        'End If
    End Sub
    Private Sub LeggiImm(ByVal Nume As Integer)
        Cmd = New SqlCommand("Select * from VArcImm where ImmCod = " & Val(TextEdit32.Text), cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            SW = True
        End If
        dataRd.Close()
    End Sub
    Function ControllaArcImm() As Boolean
        Dim MailI As String = ""
        Dim cci As New Control
        If SW = False Then
            MailI = MailI & "<>Immobile Inesistente !!!" & Chr(13)
            cci = TextEdit32
        End If
        If MailI > "" Then
            MoltoCritico(MailI)
            cci.Focus()
            Return False
        End If
        Return True
    End Function
    Private Sub CaricaImmobile()
        Cmd = New SqlCommand("Select * from VArcImm where ImmCod = " & Val(TextEdit32.Text), cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit4.Text = dataRd.Item("ImmIndirizzo")
            TextEdit3.Text = dataRd.Item("ImmCap")
            TextEdit5.Text = dataRd.Item("ImmCitta")
            TextEdit6.Text = dataRd.Item("ImmPv")
            TextEdit10.Text = dataRd.Item("ImmNrIdenCom")
            TextEdit11.EditValue = dataRd.Item("ImmMq")
            TextEdit12.EditValue = dataRd.Item("ImmMc")
            TextEdit13.EditValue = dataRd.Item("ImmMillesimi")
            TextEdit33.EditValue = dataRd.Item("ImmPiano")
            TextEdit34.EditValue = dataRd.Item("ImmCateg")
            TextEdit7.Text = dataRd.Item("ImmFgexC")
            TextEdit8.Text = dataRd.Item("ImmNrexC")
            TextEdit9.Text = dataRd.Item("ImmSubexC")
            TextEdit14.Text = dataRd.Item("ImmFgnC")
            TextEdit15.Text = dataRd.Item("ImmNrnC")
            TextEdit16.Text = dataRd.Item("ImmSubnC")
            TextEdit35.EditValue = dataRd.Item("ImmRendCata")
            'ComboBoxEdit3.SelectedIndex = SettaComboEdit(ComboBoxEdit3, dataRd.Item("AffTipo"), 0)
            'ComboBoxEdit4.SelectedIndex = SettaComboEdit(ComboBoxEdit4, dataRd.Item("AffDurata"), 0)
        End If
        dataRd.Close()
    End Sub

    Private Sub CaricaAffitti()
        RifAff = 0
        Cmd = New SqlCommand("Select * from VAffitti where ImmCod = " & Val(TextEdit32.Text) & " and AffCLiente = " & Val(TextEdit1.Text), cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            ComboBoxEdit3.SelectedIndex = SettaComboEdit(ComboBoxEdit3, dataRd.Item("AffTipo"), 0)
            ComboBoxEdit4.SelectedIndex = SettaComboEdit(ComboBoxEdit4, dataRd.Item("AffDurata"), 0)

            ComboBoxEdit1.SelectedIndex = SettaComboEdit(ComboBoxEdit1, dataRd.Item("AffBaseCalcolo"), 0)
            ComboBoxEdit2.SelectedIndex = SettaComboEdit(ComboBoxEdit2, dataRd.Item("AffPagareA"), 0)
            ComboBoxEdit5.SelectedIndex = SettaComboEdit(ComboBoxEdit5, dataRd.Item("AffInviateDa"), 0)
            ComboBoxEdit7.SelectedIndex = dataRd.Item("AffTipoModulo")

            DateEdit1.EditValue = dataRd.Item("AffIniLoc")
            DateEdit2.EditValue = dataRd.Item("AffRegAnn")
            DateEdit3.EditValue = dataRd.Item("AffFineContr")
            DateEdit4.EditValue = dataRd.Item("AffScaProroga")
            ComboBoxEdit6.SelectedIndex = dataRd.Item("AffTipoPag")
            TextEdit17.EditValue = dataRd.Item("AffCauzione")
            TextEdit18.EditValue = dataRd.Item("AffCanone")
            TextEdit19.EditValue = dataRd.Item("AffImpReg")
            TextEdit20.EditValue = dataRd.Item("AffImpIst")
            TextEdit21.EditValue = dataRd.Item("AffImpInq")

            TextEdit27.EditValue = dataRd.Item("AffDes1")
            TextEdit28.EditValue = dataRd.Item("AffDes2")
            TextEdit29.EditValue = dataRd.Item("AffDes3")
            TextEdit30.EditValue = dataRd.Item("AffDes4")
            TextEdit31.EditValue = dataRd.Item("AffDes5")

            TextEdit22.EditValue = dataRd.Item("AffImp1")
            TextEdit23.EditValue = dataRd.Item("AffImp2")
            TextEdit24.EditValue = dataRd.Item("AffImp3")
            TextEdit25.EditValue = dataRd.Item("AffImp4")
            TextEdit26.EditValue = dataRd.Item("AffImp5")
            MemoEdit1.Text = dataRd.Item("AffNote")
            TextEdit36.EditValue = dataRd.Item("AffNrPers")
            TextEdit37.EditValue = dataRd.Item("AffOneriAccess")
            TextEdit38.EditValue = dataRd.Item("AffAccontoRisc")
            TextEdit39.EditValue = dataRd.Item("AffRateRisc")
            RifAff = dataRd.Item("AffRif")
        End If
        dataRd.Close()
        If ComboBoxEdit7.SelectedIndex < 1 Then ButtonCN.Enabled = False
        ComboBoxEdit3.Focus()
    End Sub
    Function ControllaCampiT() As Boolean
        Dim Mail As String = ""
        Dim cc As New Control
        If ComboBoxEdit3.Text = "" Then
            Mail = Mail & "<>Inserire Tipo" & Chr(13)
            cc = ComboBoxEdit3
        End If
        If ComboBoxEdit4.Text = "" Then
            Mail = Mail & "<>Inserire Durata" & Chr(13)
            cc = ComboBoxEdit4
        End If
        If ComboBoxEdit1.Text = "" Then
            Mail = Mail & "<>Inserire Base Calcolo" & Chr(13)
            cc = ComboBoxEdit1
        End If
        'If ComboBoxEdit2.Text = "" And ComboBoxEdit3.Text <> "BOX" Then
        '    Mail = Mail & "<>Inserire Pagare a" & Chr(13)
        '    cc = ComboBoxEdit2
        'End If
        'If ComboBoxEdit5.Text = "" And ComboBoxEdit3.Text <> "BOX" Then
        '    Mail = Mail & "<>Inserire Comunicazioni Inviate da" & Chr(13)
        '    cc = ComboBoxEdit5
        'End If
        If TextEdit18.EditValue = 0 Then
            Mail = Mail & "<>Inserire Importo Canone" & Chr(13)
            cc = TextEdit18
        End If
        'If TextEdit17.EditValue = 0 Then
        '    Mail = Mail & "<>Inserire Importo Cauzione" & Chr(13)
        '    cc = TextEdit17
        'End If
        'If TextEdit19.EditValue = 0 Then
        '    Mail = Mail & "<>Inserire Importo Imposta di Registro" & Chr(13)
        '    cc = TextEdit19
        'End If
        'If TextEdit20.EditValue = 0 Then
        '    Mail = Mail & "<>Inserire Imposta di Registro c/o Istituto" & Chr(13)
        '    cc = TextEdit20
        'End If
        'If TextEdit21.EditValue = 0 Then
        '    Mail = Mail & "<>Inserire Imposta di Registro c/o Inquilino" & Chr(13)
        '    cc = TextEdit21
        'End If

        If Mail > "" Then
            MoltoCritico(Mail)
            cc.Focus()
            Return False
        End If
        Return True
    End Function

    'Sub LeggiAffitti()
    '    RifAff = 0
    '    Cmd = New SqlCommand("Select AffRif from VAffitti where AffImm = " & Val(TextEdit32.Text) & " and AffCliente = " & Val(TextEdit1.Text), cnDb)
    '    dataRd = Cmd.ExecuteReader
    '    If dataRd.Read Then
    '        RifAff = dataRd.Item("AffRif")
    '    End If
    '    dataRd.Close()
    'End Sub

    Sub AggiornaAffitti()
        Dim ScriviAff As String
        Dim XCMD As SqlCommand
        Dim IDENT As New SqlCommand("SELECT @@IDENTITY ", cnDb)
        If RifAff = 0 Then
            ScriviAff = "Insert Into TbAffitti WITH (TABLOCKX)(AffCliente,AffImm,AffTipo,AffDurata,AffIniLoc,AffRegAnn,AffScaProroga,AffFineContr,AffCauzione,AffCanone,AffImpReg,AffImpIst,AffImpInq,AffDes1,AffDes2,AffDes3,AffDes4,AffDes5,AffImp1,AffImp2,AffImp3,AffImp4,AffImp5,AffRid,AffNote,AffBaseCalcolo,AffNrPers,AffOneriAccess,AffRateRisc,AffAccontoRisc,AffPagareA,AffInviateDa,AffTipoPag,AffTipoModulo) " _
                & "VALUES (@AffCliente,@AffImm,@AffTipo,@AffDurata,@AffIniLoc,@AffRegAnn,@AffScaProroga,@AffFineContr,@AffCauzione,@AffCanone,@AffImpReg,@AffImpIst,@AffImpInq,@AffDes1,@AffDes2,@AffDes3,@AffDes4,@AffDes5,@AffImp1,@AffImp2,@AffImp3,@AffImp4,@AffImp5,@AffRid,@AffNote,@AffBaseCalcolo,@AffNrPers,@AffOneriAccess,@AffRateRisc,@AffAccontoRisc,@AffPagareA,@AffInviateDa,@AffTipoPag,@AffTipoModulo)"
        Else
            ScriviAff = "UPDATE TbAffitti SET AffCliente=@AffCliente,AffImm=@AffImm,AffTipo=@AffTipo,AffDurata=@AffDurata,AffIniLoc=@AffIniLoc,AffRegAnn=@AffRegAnn,AffScaProroga=@AffScaProroga,AffFineContr=@AffFineContr,AffCauzione=@AffCauzione,AffCanone=@AffCanone,AffImpReg=@AffImpReg,AffImpIst=@AffImpIst,AffImpInq=@AffImpInq,AffDes1=@AffDes1,AffDes2=@AffDes2,AffDes3=@AffDes3," _
                & "AffDes4=@AffDes4,AffDes5=@AffDes5,AffImp1=@AffImp1,AffImp2=@AffImp2,AffImp3=@AffImp3,AffImp4=@AffImp4,AffImp5=@AffImp5,AffRid=@AffRid,AffNote=@AffNote,AffBaseCalcolo=@AffBaseCalcolo,AffNrPers=@AffNrPers,AffOneriAccess=@AffOneriAccess,AffRateRisc=@AffRateRisc,AffAccontoRisc=@AffAccontoRisc,AffPagareA=@AffPagareA,AffInviateDa=@AffInviateDa,AffTipoPag=@AffTipoPag,AffTipoModulo=@AffTipoModulo where AffRif  =" & RifAff
        End If
        Dim B0 As New SqlParameter("@RifImm", SqlDbType.Int)

        Dim p2 As New SqlParameter("@AffCliente", SqlDbType.Int)
        Dim p3 As New SqlParameter("@AffImm", SqlDbType.Int)

        Dim p4 As New SqlParameter("@AffTipo", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@AffDurata", SqlDbType.VarChar)

        Dim p6 As New SqlParameter("@AffIniLoc", SqlDbType.SmallDateTime)
        Dim p7 As New SqlParameter("@AffRegAnn", SqlDbType.SmallDateTime)
        Dim p8 As New SqlParameter("@AffScaProroga", SqlDbType.SmallDateTime)
        Dim p9 As New SqlParameter("@AffFineContr", SqlDbType.SmallDateTime)

        Dim p10 As New SqlParameter("@AffCauzione", SqlDbType.Decimal)
        Dim p11 As New SqlParameter("@AffCanone", SqlDbType.Decimal)
        Dim p12 As New SqlParameter("@AffImpReg", SqlDbType.Decimal)
        Dim p13 As New SqlParameter("@AffImpIst", SqlDbType.Decimal)
        Dim p14 As New SqlParameter("@AffImpInq", SqlDbType.Decimal)

        Dim p15 As New SqlParameter("@AffDes1", SqlDbType.VarChar)
        Dim p16 As New SqlParameter("@AffDes2", SqlDbType.VarChar)
        Dim p17 As New SqlParameter("@AffDes3", SqlDbType.VarChar)
        Dim p18 As New SqlParameter("@AffDes4", SqlDbType.VarChar)
        Dim p19 As New SqlParameter("@AffDes5", SqlDbType.VarChar)

        Dim p20 As New SqlParameter("@AffImp1", SqlDbType.Decimal)
        Dim p21 As New SqlParameter("@AffImp2", SqlDbType.Decimal)
        Dim p22 As New SqlParameter("@AffImp3", SqlDbType.Decimal)
        Dim p23 As New SqlParameter("@AffImp4", SqlDbType.Decimal)
        Dim p24 As New SqlParameter("@AffImp5", SqlDbType.Decimal)

        Dim p25 As New SqlParameter("@AffRid", SqlDbType.Bit)

        Dim p26 As New SqlParameter("@AffNote", SqlDbType.NText)

        Dim p27 As New SqlParameter("@AffBaseCalcolo", SqlDbType.VarChar)
        Dim p28 As New SqlParameter("@AffNrPers", SqlDbType.VarChar)
        Dim p29 As New SqlParameter("@AffOneriAccess", SqlDbType.Decimal)
        Dim p30 As New SqlParameter("@AffRateRisc", SqlDbType.TinyInt)
        Dim p31 As New SqlParameter("@AffAccontoRisc", SqlDbType.Decimal)
        Dim p32 As New SqlParameter("@AffPagareA", SqlDbType.VarChar)
        Dim p33 As New SqlParameter("@AffInviateDa", SqlDbType.VarChar)
        Dim p34 As New SqlParameter("@AffTipoPag", SqlDbType.TinyInt)
        Dim p35 As New SqlParameter("@AffTipoModulo", SqlDbType.TinyInt)

        p2.Value = Val(TextEdit1.Text)
        p3.Value = Val(TextEdit32.Text)

        p4.Value = ComboBoxEdit3.Text
        p5.Value = ComboBoxEdit4.Text

        p6.Value = DateEdit1.EditValue
        p7.Value = DateEdit2.EditValue
        p8.Value = DateEdit4.EditValue
        p9.Value = DateEdit3.EditValue


        p10.Value = IIf(IsNumeric(TextEdit17.EditValue), TextEdit17.EditValue, 0)
        p11.Value = IIf(IsNumeric(TextEdit18.EditValue), TextEdit18.EditValue, 0)
        p12.Value = IIf(IsNumeric(TextEdit19.EditValue), TextEdit19.EditValue, 0)
        p13.Value = IIf(IsNumeric(TextEdit20.EditValue), TextEdit20.EditValue, 0)
        p14.Value = IIf(IsNumeric(TextEdit21.EditValue), TextEdit21.EditValue, 0)

        p15.Value = TextEdit27.Text
        p16.Value = TextEdit28.Text
        p17.Value = TextEdit29.Text
        p18.Value = TextEdit30.Text
        p19.Value = TextEdit31.Text

        p20.Value = IIf(IsNumeric(TextEdit22.EditValue), TextEdit22.EditValue, 0)
        p21.Value = IIf(IsNumeric(TextEdit23.EditValue), TextEdit23.EditValue, 0)
        p22.Value = IIf(IsNumeric(TextEdit24.EditValue), TextEdit24.EditValue, 0)
        p23.Value = IIf(IsNumeric(TextEdit25.EditValue), TextEdit25.EditValue, 0)
        p24.Value = IIf(IsNumeric(TextEdit26.EditValue), TextEdit26.EditValue, 0)

        p25.Value = 0
        p26.Value = MemoEdit1.Text

        p27.Value = ComboBoxEdit1.EditValue
        p28.Value = TextEdit36.EditValue
        p29.Value = IIf(IsNumeric(TextEdit37.EditValue), TextEdit37.EditValue, 0)
        p30.Value = TextEdit39.EditValue
        p31.Value = IIf(IsNumeric(TextEdit38.EditValue), TextEdit38.EditValue, 0)
        If ComboBoxEdit2.EditValue Is Nothing Then p32.Value = "" Else p32.Value = ComboBoxEdit2.EditValue
        If ComboBoxEdit5.EditValue Is Nothing Then p33.Value = "" Else p33.Value = ComboBoxEdit5.EditValue
        p34.Value = ComboBoxEdit6.SelectedIndex
        p35.Value = ComboBoxEdit7.SelectedIndex

        XCMD = New SqlCommand(ScriviAff, cnDb)

        If RifAff <> 0 Then
            B0.Value = RifAff
            XCMD.Parameters.Add(B0)
        End If
        XCMD.Parameters.Add(p2)
        XCMD.Parameters.Add(p3)
        XCMD.Parameters.Add(p4)
        XCMD.Parameters.Add(p5)
        XCMD.Parameters.Add(p6)
        XCMD.Parameters.Add(p7)
        XCMD.Parameters.Add(p8)
        XCMD.Parameters.Add(p9)
        XCMD.Parameters.Add(p10)
        XCMD.Parameters.Add(p11)
        XCMD.Parameters.Add(p12)
        XCMD.Parameters.Add(p13)
        XCMD.Parameters.Add(p14)
        XCMD.Parameters.Add(p15)
        XCMD.Parameters.Add(p16)
        XCMD.Parameters.Add(p17)
        XCMD.Parameters.Add(p18)
        XCMD.Parameters.Add(p19)
        XCMD.Parameters.Add(p20)
        XCMD.Parameters.Add(p21)
        XCMD.Parameters.Add(p22)
        XCMD.Parameters.Add(p23)
        XCMD.Parameters.Add(p24)
        XCMD.Parameters.Add(p25)
        XCMD.Parameters.Add(p26)
        XCMD.Parameters.Add(p27)
        XCMD.Parameters.Add(p28)
        XCMD.Parameters.Add(p29)
        XCMD.Parameters.Add(p30)
        XCMD.Parameters.Add(p31)
        XCMD.Parameters.Add(p32)
        XCMD.Parameters.Add(p33)
        XCMD.Parameters.Add(p34)
        XCMD.Parameters.Add(p35)
        XCMD.ExecuteNonQuery()
        XCMD.Parameters.Clear()

        If RifAff = 0 Then RifAff = IDENT.ExecuteScalar

    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If ControllaCampiT() = False Then TextEdit32.Focus() : Exit Sub
        AggiornaAffitti()
        LeggiImmobiliInquilino()
        'GroupControl20.Enabled = False
        GroupControl4.Enabled = True
        ButtonF5.PerformClick()
        TextEdit32.Focus()
    End Sub '
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If RifAff < 1 Then Exit Sub
        EliminaTutto()
        PULIZIA()
        LeggiImmobiliInquilino()
    End Sub
    Private Sub EliminaTutto()
        If MessageBox.Show("Elimino la locazione selezionata?", "ELIMINAZIONE LOCAZIONE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If
        Cmd = New SqlCommand("Delete from TbAffitti where AffRif = @Rifaff", cnDb)
        Dim p1 As New SqlParameter("@rifaff", SqlDbType.Int)
        p1.Value = RifAff
        Cmd.Parameters.Add(p1)
        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()
    End Sub
    Private Sub GridControl3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl3.MouseMove
        ShowHitInfo3(GridView4.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo3(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl3.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Irow = hi.RowHandle
    End Sub
    Private Sub GridView4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.Click
        If Irow > -1 Then
            RwD = GridView4.GetRow(Irow)
            RifAff = RwD("AffRif")
            SW = True
            TextEdit32.Text = RwD("ImmCod")
            CaricaImmobile()
            CaricaAffitti()
            ComboBoxEdit3.Focus()
        Else
            RifAff = -1
        End If
    End Sub

    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        RicCliFor()
    End Sub
    Sub RicCliFor()
        Dim frm As New RicercaClFo
        frm.StartPosition = FormStartPosition.CenterScreen
        frm.CliFor = "CL"
        frm.ShowDialog()
        If Val(frm.Codice) > 0 Then
            TextEdit1.Text = frm.Codice
            TbLeggiCli.Focus()
        Else
            TextEdit1.Focus()
        End If
    End Sub

    Sub TbLeggiCli_ENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggiCli.Enter
        okInq = False
        TextEdit1.Text = Format(Val(TextEdit1.Text), "00000")
        If TextEdit2.EditorContainsFocus = True Then TextEdit1.Focus() : Exit Sub
        If Val(TextEdit1.Text) > 0 Then CaricaDati()

        If okInq = True Then LeggiImmobiliInquilino() : Abilita()
    End Sub
    Private Sub CaricaDati()
        Cmd = New SqlCommand("Select AnaDesc from TbAna where AnaCod = '" & TextEdit1.Text & "'", cnVd)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextEdit2.Text = dataRd.Item("AnaDesc")
            okInq = True
        End If
        dataRd.Close()
    End Sub
    Sub PulisciImmobile()
        'groupcontrol2
        TextEdit32.Text = ""
        TextEdit3.Text = "" : TextEdit4.Text = "" : TextEdit5.Text = "" : TextEdit6.Text = ""
        TextEdit33.Text = "" : TextEdit34.Text = ""
        TextEdit10.Text = "" : TextEdit11.Text = "" : TextEdit12.Text = "" : TextEdit13.Text = "" : TextEdit35.Text = ""
        'groupcontrol3
        TextEdit7.Text = "" : TextEdit8.Text = "" : TextEdit9.Text = ""
        'groupcontrol4
        TextEdit14.Text = "" : TextEdit15.Text = "" : TextEdit16.Text = ""
        'groupcontrol5
        ComboBoxEdit3.SelectedIndex = -1 : ComboBoxEdit4.SelectedIndex = -1
        ComboBoxEdit6.SelectedIndex = -1
        TextEdit18.EditValue = CDec(0.0) : TextEdit17.EditValue = CDec(0.0)
        TextEdit19.EditValue = CDec(0.0) : TextEdit20.EditValue = CDec(0.0)
        TextEdit21.EditValue = CDec(0.0)
        DateEdit1.EditValue = Today
        DateEdit2.EditValue = Today
        DateEdit3.EditValue = Today
        DateEdit4.EditValue = Today
        'groupcontrol8
        MemoEdit1.Text = ""
        'groupcontrol6
        TextEdit27.Text = "" : TextEdit28.Text = "" : TextEdit29.Text = "" : TextEdit30.Text = "" : TextEdit31.Text = ""
        TextEdit22.EditValue = CDec(0.0) : TextEdit23.EditValue = CDec(0.0) : TextEdit24.EditValue = CDec(0.0) : TextEdit25.EditValue = CDec(0.0) : TextEdit26.EditValue = CDec(0.0)
        TextEdit36.EditValue = "" : TextEdit37.EditValue = CDec(0.0) : TextEdit39.EditValue = 0 : TextEdit38.EditValue = CDec(0.0)
        ComboBoxEdit1.SelectedIndex = -1 : ComboBoxEdit2.SelectedIndex = -1 : ComboBoxEdit5.SelectedIndex = -1 : ComboBoxEdit7.SelectedIndex = -1
    End Sub
    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        PulisciImmobile()
        GridView4.UnselectRow(0)
        TextEdit32.Focus()
    End Sub

    Private Sub ButtonRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRicerca.Click
        RicArcImm()
    End Sub
    Sub RicArcImm()
        Dim frm As New Immobili
        frm.StartPosition = FormStartPosition.CenterScreen
        'frm.CliFor = "CL"
        frm.ShowDialog()
        If frm.Codice > 0 Then
            TextEdit32.Text = frm.Codice
            TbLeggiImm.Focus()
        Else
            TextEdit32.Focus()
        End If
    End Sub
    Private Sub ComboBoxEdit7_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxEdit7.SelectedIndexChanged
        If ComboBoxEdit7.SelectedIndex < 1 Then ButtonCN.Enabled = False Else ButtonCN.Enabled = True
    End Sub
    Private Sub ButtonCN_Click(sender As System.Object, e As System.EventArgs) Handles ButtonCN.Click
        If ControllaCampiT() = False Then TextEdit32.Focus() : Exit Sub
        AggiornaAffitti()
        LeggiImmobiliInquilino()
        If ComboBoxEdit7.SelectedIndex = -1 Then ComboBoxEdit7.Focus() : Exit Sub
        VaiAlContratto()
    End Sub
    Sub VaiAlContratto()
        TipoModulo = ComboBoxEdit7.SelectedIndex
        If TipoModulo = 3 Then
            If VaialBox() = False Then Exit Sub
            GoTo Apertura
        End If
        If TipoModulo = 4 Then
            If VaiAutonomo() = False Then Exit Sub
            GoTo Apertura
        End If
        If TipoModulo = 5 Then
            If Abitativo_Fidej_Bon() = False Then Exit Sub
            GoTo Apertura
        End If
        If PreparaDati() = False Then Exit Sub
Apertura:
        Dim fine As Int16 = 0
        If ComboBoxEdit7.SelectedIndex = 1 Then fine = 41
        If ComboBoxEdit7.SelectedIndex = 2 Then fine = 42
        If ComboBoxEdit7.SelectedIndex = 3 Then fine = 12
        If ComboBoxEdit7.SelectedIndex = 4 Then fine = 32
        If ComboBoxEdit7.SelectedIndex = 5 Then fine = 33
        Apridocumento(fine)
    End Sub
    Sub LeggiDatiAffitto()
        Dim Str As String = "select * from Vaffitti where AffRif = " & RifAff & " and AffCliente = " & Val(TextEdit1.Text)
        TbDoc = New DataTable()
        DaDoc = New SqlDataAdapter(Str, cnDb)
        DaDoc.Fill(TbDoc)
    End Sub
    Function VaialBox() As Boolean
        LeggiDatiAffitto()
        If TbDoc.Rows.Count <> 1 Then Return False : Exit Function
        Rxw = TbDoc.Rows(0)
        COMUNE = Rxw("ClDtnCitta")
        SESSO = Rxw("ClDtnSex")
        PROV = Rxw("ClDtnProv")
        DataN = Rxw("ClDtnData")
        If TipoModulo = 3 Then  ''' 
            Paragrafo(1) = Rxw("AnaDesc")
            If SESSO = "F" Then
                Paragrafo(1) &= ", nata a "
            Else
                Paragrafo(1) &= ", nato a "
            End If
            Paragrafo(1) &= COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", Codice Fiscale " & Rxw("AnaCfis") & ", residente a " & Rxw("AnaCitta") & " " & Rxw("AnaIndirizzo")
        End If

        Paragrafo(2) = Trim(Rxw("ImmIndirizzo")) & ","
        Paragrafo(2) &= " censito al N.C.E.U. al foglio " & Rxw("ImmFgnC")
        Paragrafo(2) &= ", particella " & Rxw("ImmNrnC") & ", sub " & Rxw("ImmSubNc") & ", categoria " & Trim(Rxw("ImmCateg")) & ", classe " & Trim(Rxw("ImmClasse"))
        Paragrafo(2) &= " rendita catastale " & Format(Rxw("ImmRendCata"), "c2")

        Paragrafo(3) = Rxw("ImmCitta") & " (" & Rxw("ImmPv") & ") " & Trim(Rxw("ImmIndirizzo"))
        Paragrafo(4) = Format(Rxw("AffIniLoc"), "dd.MM.yyyy")
        Paragrafo(5) = Format(Rxw("AffFineContr"), "dd.MM.yyyy")
        Dim Mx As Int16 = 12
        If Trim(Rxw("AffBaseCalcolo")) = "trimestrale" Then Mx = 4
        Canone = Rxw("AffCanone") * Mx
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(6) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        Paragrafo(7) = Mid(Trim(Rxw("AffBaseCalcolo")), 1, Len(Trim(Rxw("AffBaseCalcolo"))) - 1) & "i"
        Paragrafo(8) = Rxw("AffCanone")
        If Rxw("AffTipoPag") = 0 Then
            Paragrafo(9) = "Sepa Direct Debit (SDD)"
        ElseIf Rxw("AffTipoPag") = 1 Then
            Paragrafo(9) = "Bonifico Bancario"
        Else
            Paragrafo(9) = "Contanti"
        End If
        Dim PrimoDelMese As String = "1° " & Format(CDate(Rxw("AffIniLoc")), "MMMM")
        Paragrafo(10) = PrimoDelMese
        Paragrafo(11) = Format(CDate(Rxw("AffIniLoc")), "dd MMMM yyyy")
        Paragrafo(12) = Format(CDate(Rxw("AffIniLoc")), "dd MMMM yyyy")
        Return True
    End Function
    Function VaiAutonomo() As Boolean
        LeggiDatiAffitto()
        If TbDoc.Rows.Count <> 1 Then Return False : Exit Function
        Rxw = TbDoc.Rows(0)
        COMUNE = Rxw("ClDtnCitta")
        SESSO = Rxw("ClDtnSex")
        PROV = Rxw("ClDtnProv")
        DataN = Rxw("ClDtnData")
        Paragrafo(1) = Rxw("AnaDesc") & ", lavoratore autonomo"
        If SESSO = "F" Then
            Paragrafo(1) &= ", nata a "
            Paragrafo(2) = "a"
        Else
            Paragrafo(1) &= ", nato a "
            Paragrafo(2) = "o"
        End If
        Paragrafo(1) &= COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", Codice Fiscale " & Rxw("AnaCfis") & ", Partita Iva " & Rxw("AnaPiva") & ", residente in "
        If (Rxw("ClAnteCitta") & " " & Rxw("ClAnteIndirizzo")).ToString.Length > 5 Then
            Paragrafo(1) &= Trim(Rxw("ClAnteCitta") & " " & Rxw("ClAnteIndirizzo"))
        Else
            Paragrafo(1) &= Trim(Rxw("AnaCitta") & " " & Rxw("AnaIndirizzo"))
        End If
        Paragrafo(3) = Trim(Rxw("ImmIndirizzo")) & ","
        Paragrafo(3) &= " al piano "
        If IsNumeric(Rxw("ImmPiano")) = True Then
            Select Case Val(Rxw("ImmPiano"))
                Case 1
                    Paragrafo(3) &= "primo"
                Case 2
                    Paragrafo(3) &= "secondo"
                Case 3
                    Paragrafo(3) &= "terzo"
                Case 4
                    Paragrafo(3) &= "quarto"
                Case 5
                    Paragrafo(3) &= "quinto"
            End Select
        Else
            Paragrafo(3) &= Rxw("ImmPiano")
        End If
        REM definira n. cantina
        Dim Senza As String = ""
        For C = 0 To Len(Rxw("ImmNrIdenCom"))
            If IsNumeric(Mid(Rxw("ImmNrIdenCom"), C + 1, 1)) Then
                Senza = Senza & Mid(Rxw("ImmNrIdenCom"), C + 1, 1)
            End If
        Next
        Paragrafo(3) &= ", composta di: " & Trim(Rxw("ImmComposta")) & " quest'ultima individuata dal n." & Val(Trim(Senza)) & ", censita al N.C.E.U. al foglio " & Rxw("ImmFgnC")
        Paragrafo(3) &= ", particella " & Rxw("ImmNrnC") & ", sub " & Rxw("ImmSubNc") & ", categoria " & Trim(Rxw("ImmCateg")) & ", classe " & Trim(Rxw("ImmClasse")) & " vani " & Trim(Rxw("ImmVani"))
        Paragrafo(3) &= ", superficie catastale " & CInt(Rxw("ImmCatastoSuper")) & " mq,"
        If Val(Rxw("ImmAreeScop")) > 0 Then
            Paragrafo(3) &= " totale escluse aree scoperte " & CInt(Rxw("ImmAreeScop")) & " mq,"
        End If
        Paragrafo(3) &= " rendita catastale " & Format(Rxw("ImmRendCata"), "c2")
        Paragrafo(4) = Rxw("ImmNrIdent")
        Paragrafo(5) = Rxw("ImmResidenza")
        Paragrafo(6) = Rxw("ImmApeNr")
        Paragrafo(7) = Rxw("ImmApeClasse")
        Paragrafo(8) = Rxw("AffTipo").ToString.ToLower
        Paragrafo(9) = Trim(Rxw("ImmIndirizzo"))
        Paragrafo(10) = Rxw("AffTipo").ToString.ToLower
        Dim Anni As Int16 = CDate(Rxw("AffFineContr")).Year - CDate(Rxw("AffIniLoc")).Year
        Paragrafo(11) = spell_my_int(Anni).ToLower
        Paragrafo(12) = Format(Rxw("AffIniLoc"), "dd.MM.yyyy")
        Paragrafo(13) = Format(Rxw("AffFineContr"), "dd.MM.yyyy")
        Paragrafo(14) = spell_my_int(Anni).ToLower
        Paragrafo(15) = spell_my_int(Anni).ToLower
        Dim Mx As Int16 = 12
        If Trim(Rxw("AffBaseCalcolo")) = "trimestrale" Then Mx = 4
        Canone = Rxw("AffCanone") * Mx
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(16) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        Paragrafo(17) = Rxw("AffOneriAccess")
        Canone = Rxw("AffCanone")
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(18) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        Paragrafo(19) = Rxw("AffOneriAccess") / Mx
        Paragrafo(20) = Rxw("ClCntRid")
        Dim PrimoDelMese As String = "1° " & Format(CDate(Rxw("AffIniLoc")), "MMMM")
        Paragrafo(21) = PrimoDelMese
        Paragrafo(22) = Rxw("ImmMillesimi")
        Paragrafo(23) = spell_my_int(Rxw("ImmMillAsce")).ToLower
        Paragrafo(24) = Format((Rxw("AffAccontoRisc") / Rxw("AffRateRisc")), "n2")
        Paragrafo(25) = Rxw("AffAccontoRisc")
        Paragrafo(26) = Rxw("ImmMcUnita")
        Paragrafo(27) = MeseRisc(Rxw("AffRateRisc"))
        Canone = Rxw("AffCauzione")
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(28) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        COMUNE = Rxw("ClCoCitta")
        SESSO = Rxw("ClCoSex")
        PROV = Rxw("ClCoProv")
        DataN = Rxw("ClCoDtnData")
        If SESSO = "M" Then
            Paragrafo(29) &= Rxw("ClCoCognomeNome") & ", nato a " & COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", C.F.: " & Rxw("ClCoCodFisc")
        Else
            Paragrafo(29) &= Rxw("ClCoCognomeNome") & ", nata a " & COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", C.F.: " & Rxw("ClCoCodFisc")
        End If
        Paragrafo(29) &= " residente a " & Rxw("ClCoAnteCitta") & " (" & Rxw("ClCoAnteProv") & ") " & Rxw("ClCoAnteIndirizzo")
        Paragrafo(30) = "01" & Format(CDate(Rxw("AffIniLoc")), " MMMM yyyy")
        Paragrafo(31) = "01" & Format(CDate(Rxw("AffIniLoc")), " MMMM yyyy")
        Paragrafo(32) = "01" & Format(CDate(Rxw("AffIniLoc")), " MMMM yyyy")
        Return True
    End Function
    Function Abitativo_Fidej_Bon() As Boolean
        LeggiDatiAffitto()
        If TbDoc.Rows.Count <> 1 Then Return False : Exit Function
        Paragrafo(0) = 0 '''' solo per inizializzare il paragrafo ( 0) non utilizzato
        Rxw = TbDoc.Rows(0)
        COMUNE = Rxw("ClDtnCitta")
        SESSO = Rxw("ClDtnSex")
        PROV = Rxw("ClDtnProv")
        DataN = Rxw("ClDtnData")
        If SESSO = "F" Then
            Paragrafo(1) = "La signora " & Rxw("AnaDesc") & ", nata a "
            Paragrafo(2) = "a"
        Else
            Paragrafo(1) = "Il signor " & Rxw("AnaDesc") & ", nato a "
            Paragrafo(2) = "o"
        End If
        Paragrafo(1) &= COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", Codice Fiscale " & Rxw("AnaCfis") & ", residente in "
        If (Rxw("ClAnteCitta") & " " & Rxw("ClAnteIndirizzo")).ToString.Length > 5 Then
            Paragrafo(1) &= Trim(Rxw("ClAnteCitta") & " " & Rxw("ClAnteIndirizzo"))
        Else
            Paragrafo(1) &= Trim(Rxw("AnaCitta") & " " & Rxw("AnaIndirizzo"))
        End If
        Paragrafo(3) = Trim(Rxw("ImmIndirizzo")) & ","
        Paragrafo(3) &= " al piano "
        If IsNumeric(Rxw("ImmPiano")) = True Then
            Select Case Val(Rxw("ImmPiano"))
                Case 1
                    Paragrafo(3) &= "primo"
                Case 2
                    Paragrafo(3) &= "secondo"
                Case 3
                    Paragrafo(3) &= "terzo"
                Case 4
                    Paragrafo(3) &= "quarto"
                Case 5
                    Paragrafo(3) &= "quinto"
            End Select
        Else
            Paragrafo(3) &= Rxw("ImmPiano")
        End If
        Paragrafo(3) &= ", composta di: " & Trim(Rxw("ImmComposta")) & " censita al N.C.E.U. al foglio " & Rxw("ImmFgnC")
        Paragrafo(3) &= ", particella " & Rxw("ImmNrnC") & ", sub " & Rxw("ImmSubNc") & ", categoria " & Trim(Rxw("ImmCateg")) & ", classe " & Trim(Rxw("ImmClasse")) & " vani " & Trim(Rxw("ImmVani"))
        Paragrafo(3) &= ", superficie catastale totale " & CInt(Rxw("ImmCatastoSuper")) & " mq; "
        If IsNumeric(Rxw("ImmAreeScop")) = True Then
            Paragrafo(3) &= "totale escluse aree scoperte " & CInt(Rxw("ImmAreeScop")) & " mq,"
        End If
        Paragrafo(3) &= " rendita catastale " & Format(Rxw("ImmRendCata"), "c2")
        Paragrafo(4) = Rxw("ImmNrIdent")
        Paragrafo(5) = Rxw("ImmResidenza")
        Paragrafo(6) = Rxw("ImmApeNr")
        Paragrafo(7) = Rxw("ImmApeClasse")
        Paragrafo(8) = Trim(Rxw("ImmIndirizzo"))
        Dim Anni As Int16 = CDate(Rxw("AffFineContr")).Year - CDate(Rxw("AffIniLoc")).Year
        Paragrafo(9) = spell_my_int(Anni).ToLower
        Paragrafo(10) = Format(Rxw("AffIniLoc"), "dd.MM.yyyy")
        Paragrafo(11) = Format(Rxw("AffFineContr"), "dd.MM.yyyy")
        Paragrafo(12) = spell_my_int(Anni).ToLower
        Paragrafo(13) = spell_my_int(Anni).ToLower
        Paragrafo(14) = Rxw("AffNrPers")

        Dim Mx As Int16 = 12
        If Trim(Rxw("AffBaseCalcolo")) = "trimestrale" Then Mx = 4
        Canone = Rxw("AffCanone") * Mx
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(15) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        Paragrafo(16) = Rxw("AffOneriAccess")
        Paragrafo(17) = Rxw("ImmMillesimi")
        Paragrafo(18) = Rxw("ImmMillAsce")
        Dim Base As String = Trim(Rxw("AffBaseCalcolo"))
        Paragrafo(19) = Mid(Base, 1, Len(Base) - 1) & "i"
        Canone = Rxw("AffCanone")
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(20) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        Paragrafo(21) = Rxw("AffOneriAccess")
        Dim PrimoDelMese As String = "1° " & Format(CDate(Rxw("AffIniLoc")), "MMMM")
        Paragrafo(22) = PrimoDelMese
        Paragrafo(23) = Rxw("ImmMcUnita")
        Paragrafo(24) = Rxw("AffRateRisc")
        Dim d1 As Decimal = Rxw("AffAccontoRisc")
        Dim d2 As Int16 = Rxw("AffRateRisc")
        Dim Spese As Decimal = Format(d1 / d2, "n2")
        Paragrafo(25) = Format(Spese, "n2")
        Dim Periodo As String = (CDate(Rxw("AffIniLoc")).Year - 1).ToString & "/" & (CDate(Rxw("AffIniLoc")).Year).ToString
        Paragrafo(26) = Periodo
        Paragrafo(27) = Format(Rxw("AffAccontoRisc"), "n2")
        Paragrafo(28) = MeseRisc(Rxw("AffRateRisc"))
        Canone = Rxw("AffCauzione")
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(29) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        COMUNE = Rxw("ClCoCitta")
        SESSO = Rxw("ClCoSex")
        PROV = Rxw("ClCoProv")
        DataN = Rxw("ClCoDtnData")
        If SESSO = "M" Then
            Paragrafo(30) &= "il signor " & Rxw("ClCoCognomeNome") & ", nato a " & COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", C.F.: " & Rxw("ClCoCodFisc")
        Else
            Paragrafo(30) &= "la signora " & Rxw("ClCoCognomeNome") & ", nata a " & COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", C.F.: " & Rxw("ClCoCodFisc")
        End If
        Paragrafo(30) &= " residente a " & Rxw("ClCoAnteCitta") & " (" & Rxw("ClCoAnteProv") & ") " & Rxw("ClCoAnteIndirizzo")
        Paragrafo(31) = "01" & Format(CDate(Rxw("AffIniLoc")), " MMMM yyyy")
        Paragrafo(32) = "01" & Format(CDate(Rxw("AffIniLoc")), " MMMM yyyy")
        Paragrafo(33) = "01" & Format(CDate(Rxw("AffIniLoc")), " MMMM yyyy")
        Return True
    End Function

    Function PreparaDati() As Boolean
        LeggiDatiAffitto()
        If TbDoc.Rows.Count <> 1 Then Return False : Exit Function
        Paragrafo(0) = 0 '''' solo per inizializzare il paragrafo ( 0) non utilizzato
        Rxw = TbDoc.Rows(0)
        COMUNE = Rxw("ClDtnCitta")
        SESSO = Rxw("ClDtnSex")
        PROV = Rxw("ClDtnProv")
        DataN = Rxw("ClDtnData")
        If TipoModulo = 1 Then  ''' 3+2 singolo inquilino
            If SESSO = "F" Then
                Paragrafo(1) = "La signora " & Rxw("AnaDesc") & ", nata a "
                Paragrafo(2) = "a"
            Else
                Paragrafo(1) = "Il signor " & Rxw("AnaDesc") & ", nato a "
                Paragrafo(2) = "o"
            End If
            Paragrafo(1) &= COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", Codice Fiscale " & Rxw("AnaCfis") & ", residente a " & Rxw("AnaCitta") & " " & Rxw("AnaIndirizzo")
        End If
        If TipoModulo = 2 Then  ''' 3+2 + inquilini
            If SESSO = "M" Then
                Paragrafo(1) = "I signori " & Rxw("AnaDesc") & ", nato a " & COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", C.F.: " & Rxw("AnaCfis") & "; "
            Else
                Paragrafo(1) = "I signori " & Rxw("AnaDesc") & ", nata a " & COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", C.F.: " & Rxw("AnaCfis") & "; "
            End If
            COMUNE = Rxw("ClCoCitta")
            SESSO = Rxw("ClCoSex")
            PROV = Rxw("ClCoProv")
            DataN = Rxw("ClCoDtnData")

            If SESSO = "M" Then
                Paragrafo(1) &= Rxw("ClCoCognomeNome") & ", nato a " & COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", C.F.: " & Rxw("ClCoCodFisc")
            Else
                Paragrafo(1) &= Rxw("ClCoCognomeNome") & ", nata a " & COMUNE & " " & "(" & PROV & ")" & " il " & Format(DataN, "dd.MM.yyyy") & ", C.F.: " & Rxw("ClCoCodFisc")
            End If
            Paragrafo(1) &= " entrambi residenti in " & Rxw("ClAnteCitta") & " " & Rxw("ClAnteIndirizzo")
        End If

        Paragrafo(3) = Trim(Rxw("ImmIndirizzo")) & ","
        Paragrafo(3) &= " al piano "
        If IsNumeric(Rxw("ImmPiano")) = True Then
            Select Case Val(Rxw("ImmPiano"))
                Case 1
                    Paragrafo(3) &= "primo"
                Case 2
                    Paragrafo(3) &= "secondo"
                Case 3
                    Paragrafo(3) &= "terzo"
                Case 4
                    Paragrafo(3) &= "quarto"
                Case 5
                    Paragrafo(3) &= "quinto"
            End Select
        Else
            Paragrafo(3) &= Rxw("ImmPiano")
        End If
        Paragrafo(3) &= ", composta di: " & Trim(Rxw("ImmComposta")) & " Tale unità immobiliare è censita al N.C.E.U. al foglio " & Rxw("ImmFgnC")
        Paragrafo(3) &= ", particella " & Rxw("ImmNrnC") & ", sub " & Rxw("ImmSubNc") & ", categoria " & Trim(Rxw("ImmCateg")) & ", classe " & Trim(Rxw("ImmClasse")) & " vani " & Trim(Rxw("ImmVani"))
        Paragrafo(3) &= ", superficie catastale totale " & CInt(Rxw("ImmCatastoSuper")) & " mq; "
        If IsNumeric(Rxw("ImmAreeScop")) = True Then
            Paragrafo(3) &= "totale escluse aree scoperte " & CInt(Rxw("ImmAreeScop")) & " mq,"
        End If
        Paragrafo(3) &= " rendita catastale " & Format(Rxw("ImmRendCata"), "c2")
        Paragrafo(4) = Rxw("ImmNrIdent")
        Paragrafo(5) = Rxw("ImmResidenza")
        Paragrafo(6) = Rxw("ImmApeNr")
        Paragrafo(7) = Rxw("ImmApeClasse")
        Paragrafo(8) = Rxw("ImmMcUnita")
        Paragrafo(9) = Rxw("ImmMillesimi")
        Paragrafo(10) = Rxw("ImmMillAsce")
        Paragrafo(11) = Trim(Rxw("ImmIndirizzo"))

        Paragrafo(12) = Rxw("AffNrPers")
        Dim Anni As Int16 = CDate(Rxw("AffFineContr")).Year - CDate(Rxw("AffIniLoc")).Year
        Paragrafo(13) = spell_my_int(Anni).ToUpper
        Paragrafo(14) = Format(Rxw("AffIniLoc"), "dd.MM.yyyy")
        Paragrafo(15) = Format(Rxw("AffFineContr"), "dd.MM.yyyy")
        Dim Mx As Int16 = 12
        If Trim(Rxw("AffBaseCalcolo")) = "trimestrale" Then Mx = 4
        Canone = Rxw("AffCanone") * Mx
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(16) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        Paragrafo(17) = Rxw("AffOneriAccess")
        Paragrafo(18) = Trim(Rxw("AffBaseCalcolo"))
        Paragrafo(19) = Mid(Paragrafo(18), 1, Len(Paragrafo(18)) - 1) & "i"
        If Trim(Rxw("AffBaseCalcolo")) = "mensile" Then
            Paragrafo(20) = "mese"
        ElseIf Trim(Rxw("AffBaseCalcolo")) = "trimestrale" Then
            Paragrafo(20) = "primo mese del trimestre in corso"
        End If
        Canone = Rxw("AffCanone")
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(21) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        Paragrafo(22) = Rxw("AffOneriAccess")

        If Rxw("AffTipoPag") = 0 Then
            Paragrafo(23) = "Sepa Direct Debit (SDD)"
        ElseIf Rxw("AffTipoPag") = 1 Then
            Paragrafo(23) = "Bonifico Bancario"
        Else
            Paragrafo(23) = "Contanti"
        End If
        Dim PrimoDelMese As String = "1° " & Format(CDate(Rxw("AffIniLoc")), "MMMM")
        Paragrafo(24) = PrimoDelMese
        Paragrafo(25) = Rxw("AffRateRisc")
        Dim d1 As Decimal = Rxw("AffAccontoRisc")
        Dim d2 As Int16 = Rxw("AffRateRisc")
        Dim Spese As Decimal = Format(d1 / d2, "n2")
        Paragrafo(26) = Format(Spese, "n2")
        Dim Periodo As String = (CDate(Rxw("AffIniLoc")).Year - 1).ToString & "/" & (CDate(Rxw("AffIniLoc")).Year).ToString
        Paragrafo(27) = Periodo
        Paragrafo(28) = Format(Rxw("AffAccontoRisc"), "n2")
        Paragrafo(29) = Trim(Rxw("AffPagareA"))
        Paragrafo(30) = MeseRisc(Rxw("AffRateRisc"))
        Paragrafo(31) = Trim(Rxw("AffInviateDa"))
        Canone = Rxw("AffCauzione")
        Intero = CInt(Canone)
        Resto = (Canone - Intero)
        Valori = spell_my_int(Intero)
        Paragrafo(32) = Format(Canone, "n2") & " (" & Valori & "/" & Format(Resto, "00") & ")"
        If Trim(Rxw("AffBaseCalcolo")) = "trimestrale" Then
            Intero = 3
        Else
            Intero = Rxw("AffCauzione") / Rxw("AffCanone")
        End If
        Valori = spell_my_int(Intero)
        Paragrafo(33) = Valori
        Paragrafo(34) = Format(CDate(Rxw("AffIniLoc")), "dd MMMM yyyy")
        Paragrafo(35) = Format(CDate(Rxw("AffIniLoc")), "dd MMMM yyyy")
        Paragrafo(36) = Trim(Rxw("ImmIndirizzo"))
        Paragrafo(37) = Rxw("ImmSuperConv")
        If Trim(Rxw("AffBaseCalcolo")) = "trimestrale" Then
            Canone = Rxw("AffCanone") / 3 / Rxw("ImmSuperConv")
        Else
            Canone = Rxw("AffCanone") / Rxw("ImmSuperConv")
        End If
        Paragrafo(38) = Format(Canone, "n2")
        Paragrafo(39) = Paragrafo(16)
        Paragrafo(40) = CDate(Rxw("AffIniLoc")).Day & "." & Format(CDate(Rxw("AffIniLoc")).Month, "00") & "." & CDate(Rxw("AffIniLoc")).Year
        Paragrafo(41) = Format(CDate(Rxw("AffIniLoc")), "dd MMMM yyyy")
        If Rxw("AffTipoPag") = 0 Then Paragrafo(42) = CodSia & ", Cod.Mandato " & Rxw("ClCntRid")
        Return True
    End Function
    Sub Apridocumento(fine As Int16)
        ''Dim docum As String = "c:\rosine\testcontratti\AGEVOLATO 3+2  BOTTERO EVIDENZIATO.doc"

        Dim docum As String = NomeFile & ComboBoxEdit7.Text
        Dim newdoc As String = RifAff.ToString & ".docx"
        File.Delete(newdoc)
        RichEditControl1.LoadDocument(docum, DocumentFormat.OpenXml)
        RichEditControl1.Overtype = True
        Dim doc As Document = RichEditControl1.Document
        Dim ranges() As DocumentRange
        For n = 1 To fine
            If ComboBoxEdit7.SelectedIndex = 2 And n = 2 Then GoTo ISalto
            ranges = doc.FindAll("#" & n, SearchOptions.None)
            doc.Replace(ranges(0), Paragrafo(n))
ISalto:
        Next
        doc.SaveDocument(newdoc, DocumentFormat.OpenXml)
        Process.Start(newdoc)
    End Sub

End Class