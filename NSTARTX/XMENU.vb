Imports DXBASE
Imports NGEVE
Imports NCCOM
Imports NCDCO
Imports NCOGE
Imports System.IO
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraBars
Imports DevExpress.Skins
Imports System.Xml

Public Class XMENU
    Dim fileName As String = "c:\XtraBars_SaveToXML.xml"
    Dim skinMask As String = "ActiveSkin: "
    Dim styleMask As String = "ActiveStyle: "
    Dim styleMenu As String = "DockStyle: "
    Dim Line As String = ""
    Dim UserId As String
    Dim EComunita As Boolean = False

    Private Sub XMENU_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        CaricaSkinCombo()
        Settaggi()
        Me.Bar2.OptionsBar.MultiLine = False
    End Sub
    Private Sub CaricaSkinCombo()
        For Each cnt As SkinContainer In SkinManager.Default.Skins
            SkinCombo.Items.Add(cnt.SkinName)
        Next cnt
    End Sub
    Private Sub Settaggi()
        Dim Locale As String

        Cmd = New SqlCommand("select sel8 from TbSel where SelId = 1", cnVd)

        Locale = Cmd.ExecuteScalar
        Locale = "C:\WKCSA\"
        If Directory.Exists(Locale) = False Then
            Directory.CreateDirectory(Locale)
        End If

        NomeFile = Locale & "Xsettings.Cfg"

        If Not File.Exists(NomeFile) Then
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetOffice2003Style()
            Exit Sub
        End If

        Dim output As StreamReader = New StreamReader(NomeFile)

        'ActiveSkin
        LookSkins.EditValue = ""
        Line = Trim(output.ReadLine())

        'ActiveStyle
        Dim en As Int16 = Trim(output.ReadLine()).Remove(0, 13)

        If en = 0 Then
            AbilitaStile(LookA1, "")
        ElseIf en = 1 Then
            AbilitaStile(LookA2, "")
        ElseIf en = 3 Then
            AbilitaStile(LookA3, "")
        ElseIf en = 2 Then
            AbilitaStile(LookA4, "")
        ElseIf en = 5 Then
            AbilitaStile(LookA4, "")
        Else
            LookSkins.EditValue = Line.Remove(0, 12)
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(Line.Remove(0, 12))
        End If
        'DockStyle
        Line = output.ReadLine()
        If Line Is Nothing Then en = 4 Else en = Line.Remove(0, 11)
        If en > 0 And en < 5 Then Bar2.DockStyle = en Else Bar2.DockStyle = BarDockStyle.Bottom
        output.Close()
    End Sub
    Private Sub XMENU_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        OKCDC = False
        If aggiorna_sql() = False Then
            Me.Close()
            Return
        End If
        Me.Text = Marchio() & " - CONTABILITA'"

        AccessoUscitaError("I")
        AccessoUscita(Me)
        VociMenuAbilitate()
        AbilitaDisabilita()
        TextEdit1.Text = ""
        If Nomegruppo = "Administrator" Then MenuUtenti.Visibility = BarItemVisibility.Always Else MenuUtenti.Visibility = BarItemVisibility.Never
        MenuRiservato.Visibility = BarItemVisibility.Never
        MenuCDCO.Visibility = BarItemVisibility.Never
        MenuCOAN.Visibility = BarItemVisibility.Never
        LeggiUserId()
        If CnDc Is Nothing Then
            MenuCDCO.Visibility = BarItemVisibility.Never
            MenuCOAN.Visibility = BarItemVisibility.Never
            OKCDC = False
        ElseIf CnDc.State = ConnectionState.Open Then
            If (UserId = "PASTAECO" Or UserId = "PASTANEW" Or UserId = "PASTAGROUP") Then
                MenuCOAN.Visibility = BarItemVisibility.Always
                OKCDC = True
            ElseIf UserId = "MONDOMARINE" Then
                MenuCDCO.Visibility = BarItemVisibility.Always
                OKCDC = True
            End If
        End If

        Userwin = GetUserName()
        AbilitaGeve()
        'CONNESSIONIGS()
    End Sub
    Private Sub XMENU_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim TaskForm As Form
        For Each TaskForm In Me.MdiChildren
            e.Cancel = True
        Next

        If File.Exists(NomeFile) Then File.Delete(NomeFile)
        Dim output As StreamWriter = New StreamWriter(NomeFile)
        Line = skinMask & BarManager1.GetController().LookAndFeel.ActiveSkinName
        output.WriteLine(Line)

        Line = styleMask & BarManager1.GetController().LookAndFeel.ActiveStyle
        output.WriteLine(Line)
        Line = styleMenu & Bar2.DockStyle
        output.WriteLine(Line)
        output.Close()
    End Sub
    Sub LeggiUserId()
        UserId = ""
        Cmd = New SqlCommand("SELECT * FROM TbSel Where SelId = 400", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UserId = dataRd.Item("Sel14")
        End While
        dataRd.Close()
        If (UserId = "PASTAECO" Or UserId = "PASTANEW" Or UserId = "PASTAGROUP") Then BarButtonItem100.Visibility = BarItemVisibility.Always Else BarButtonItem100.Visibility = BarItemVisibility.Never
        If UserId = "CSABOX" Then BarButtonItem113.Visibility = BarItemVisibility.Always Else BarButtonItem113.Visibility = BarItemVisibility.Never
    End Sub
    Sub AbilitaGeve()
        Dim Source As String = cnDb.DataSource
        Dim WHY As String = ""
        WHY = Source.Split("\")(0)
        If WHY = Source Then GoTo Oltre
        WHY = Source.Split("\")(1)
Oltre:
        ''    If WHY = "COMUNITA" Or WHY = "RSCOMUNITA" Or WHY = "RSPENSIONATO" Or WHY = "SVRRSN" Or WHY = "BIENNE" Then MenuGeve.Visibility = BarItemVisibility.Always Else MenuGeve.Visibility = BarItemVisibility.Never : GoTo CHIUDI
        If WHY = "BIENNE" Then MenuGeve.Visibility = BarItemVisibility.Always Else MenuGeve.Visibility = BarItemVisibility.Never : GoTo CHIUDI
        If WHY = "BIENNE" Then
            BarButtonItem84.Visibility = BarItemVisibility.Never
            BarButtonItem85.Visibility = BarItemVisibility.Never
            BarButtonItem86.Visibility = BarItemVisibility.Never
            BarButtonItem87.Visibility = BarItemVisibility.Never
            BarButtonItem88.Visibility = BarItemVisibility.Never
            BarButtonItem61.Visibility = BarItemVisibility.Never
            BarButtonItem82.Visibility = BarItemVisibility.Never
            BarButtonItem83.Visibility = BarItemVisibility.Never
            BarButtonItem90.Visibility = BarItemVisibility.Never
            BarButtonItem91.Visibility = BarItemVisibility.Never
            BarButtonItem92.Visibility = BarItemVisibility.Never
            BarButtonItem105.Visibility = BarItemVisibility.Never
            BarButtonItem102.Visibility = BarItemVisibility.Never

        End If
        'If WHY = "COMUNITA" Or WHY = "RSCOMUNITA" Then
        '    EComunita = True
        ' End If
        'If WHY = "COMUNITA" Then
        '    BarButtonItem84.Visibility = BarItemVisibility.Never
        '    BarButtonItem85.Visibility = BarItemVisibility.Never
        '    BarButtonItem86.Visibility = BarItemVisibility.Never
        '    BarButtonItem87.Visibility = BarItemVisibility.Never
        '    BarButtonItem88.Visibility = BarItemVisibility.Never
        '    BarButtonItem77.Visibility = BarItemVisibility.Never
        'End If
        'If WHY = "SVRRSN" Then
        '    BarButtonItem61.Visibility = BarItemVisibility.Never
        '    BarButtonItem82.Visibility = BarItemVisibility.Never
        '    BarButtonItem83.Visibility = BarItemVisibility.Never
        '    BarButtonItem90.Visibility = BarItemVisibility.Never
        '    BarButtonItem91.Visibility = BarItemVisibility.Never
        '    BarButtonItem92.Visibility = BarItemVisibility.Never
        '    BarButtonItem77.Visibility = BarItemVisibility.Never
        '    BarButtonItem105.Visibility = BarItemVisibility.Never
        'End If
CHIUDI:
        REM disabilitazione provvisoria cli  for
        If UserId.ToUpper = "GSSPA" Then
            BarButtonItem10.Visibility = BarItemVisibility.Never
            BarButtonItem11.Visibility = BarItemVisibility.Never
        End If
    End Sub
    'Private Sub ConnessioniGs()
    '    If UserId.ToUpper <> "GSSPA" Then
    '        Exit Sub
    '    End If

    '    LeggiDeposito()
    '    ConnettiSede()
    '    ConnettiASTI()
    '    ConnettiTORINO()
    '    ConnettiCUNEO()
    '    ConnettiAMMINISTRAZIONE()
    'End Sub
    Private Sub VociMenuAbilitate()
        REM OBBLIGATORIO PER LA NUOVA GESTIONE MENU GEVE = CNDB COGE = CNCO
        CnMenu = cnCo
        Dim MenuStrip1 As New MenuStrip


        REM inizia  
        Dim VociAbil As New ArrayList
        Dim MENU As New DevExpress.XtraBars.LinkPersistInfo
        Dim cmd As New SqlCommand("Select * from VDXmenu where GrupLavNome='" & Nomegruppo & "'", CnMenu)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            If Not dataRd.Item("indice") Is DBNull.Value Then VociAbil.Add(dataRd.Item("indice"))
        End While
        dataRd.Close()
        Dim MMENU As New DevExpress.XtraBars.BarSubItem
        Dim LMENU As DevExpress.XtraBars.LinkPersistInfo
        REM rende tutto VISIBILE
        Dim FF, JJ As Int16
        For I As Int16 = 1 To Bar2.ItemLinks.Count
            MENU = Bar2.LinksPersistInfo(I - 1)
            MMENU = MENU.Item
            MenuStrip1.Items.Add(MENU.Item.Caption).Name = MENU.Item.Caption
            FF = MenuStrip1.Items.Count
            MenuStrip1.Items(FF - 1).Tag = MENU.Item.Id
            Dim XX As System.Windows.Forms.ToolStripMenuItem = MenuStrip1.Items(I - 1)
            For Y As Int16 = 1 To MMENU.LinksPersistInfo.Count
                LMENU = MMENU.LinksPersistInfo.Item(Y - 1)
                LMENU.Link.Visible = True
                XX.DropDownItems.Add(LMENU.Item.Caption)
                JJ = XX.DropDownItems.Count
                XX.DropDownItems(JJ - 1).Tag = LMENU.Item.Id
            Next
            MENU.Link.Visible = True
        Next
        MenuMenu = MenuStrip1
        If Nomegruppo = "Administrator" Then Exit Sub
        REM inizia INvisibile orizzontale
        For Y As Int16 = 0 To VociAbil.Count - 1
            For I As Int16 = 1 To Bar2.ItemLinks.Count
                MENU = Bar2.LinksPersistInfo(I - 1)
                If MENU.Item.Id = VociAbil.Item(Y) Then MENU.Link.Visible = False : Exit For
            Next
        Next

        REM inizia INvisibile verticale
        For I As Int16 = 1 To Bar2.ItemLinks.Count
            MENU = Bar2.LinksPersistInfo(I - 1)
            If MENU.Link.Visible = True Then
                MMENU = MENU.Item
                For K As Int16 = 1 To MMENU.LinksPersistInfo.Count
                    LMENU = MMENU.LinksPersistInfo.Item(K - 1)
                    For kk As Int16 = 0 To VociAbil.Count - 1
                        If LMENU.Item.Id = VociAbil.Item(kk) Then LMENU.Link.Visible = False : Exit For
                    Next
                Next
            End If
        Next
    End Sub
    Public Sub DXMenu(ByRef Lancia As XtraForm, ByVal INTESTA As String, Optional ByVal Quadri As String = "", Optional ByVal TipoLancio As String = "")
        AddHandler Lancia.Disposed, AddressOf FMenuDisposed
        Me.Cursor = Cursors.WaitCursor
        If ControllaMDI(Lancia) = 0 Then
            GoTo esci
        End If
        Try
            If Lancia Is Nothing Then
                GoTo esci
            End If
            Lancia.MdiParent = Me
            Lancia.WindowState = FormWindowState.Maximized
            Lancia.Text = INTESTA
            AccessoUscita(Lancia) 'AccessoUtenti
            Lancia.Tag = TipoLancio & "@" & Quadri
            PictureEdit1.SendToBack()
            GroupControl1.Visible = False
            Lancia.Show()
            Lancia = Nothing
            GC.Collect()
        Catch Ex As Exception
            AccessoUscitaError("E", Ex.Message, Ex.StackTrace, Lancia)
            MsgBox(Ex.Message)
        End Try
esci:
        Me.Cursor = Cursors.Default
    End Sub
    Public Sub FMenu(ByRef Lancia As Form, ByVal INTESTA As String)
        AddHandler Lancia.Disposed, AddressOf FMenuDisposed
        Me.Cursor = Cursors.WaitCursor
        If ControllaMDI(Lancia) = 0 Then
            GoTo esci
        End If
        Try
            If Lancia Is Nothing Then
                GoTo esci
            End If
            Lancia.MdiParent = Me
            Lancia.WindowState = FormWindowState.Maximized
            Lancia.Text = INTESTA
            AccessoUscita(Lancia) 'AccessoUtenti
            PictureEdit1.SendToBack()
            GroupControl1.Visible = False
            Lancia.Show()
            Lancia = Nothing
            GC.Collect()
        Catch Ex As Exception
            AccessoUscitaError("E", Ex.Message, Ex.StackTrace, Lancia)
            MsgBox(Ex.Message)
        End Try
esci:
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub FMenuDisposed(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.MdiChildren.Length = 0 Then
            PictureEdit1.BringToFront()
        End If
    End Sub
    Private Function ControllaMDI(ByVal lancia As Form) As Int16
        For x As Int16 = 0 To Me.MdiChildren.Length - 1
            If CType(Me.MdiChildren(x), Form).Name = lancia.Name Then
                ' PictureBox1.Visible = False
                CType(Me.MdiChildren(x), Form).Activate()
                CType(Me.MdiChildren(x), Form).WindowState = FormWindowState.Maximized
                Return 0
            End If
        Next
        Return -1
    End Function
    Private Sub LookA1_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles LookA1.ItemClick, LookA2.ItemClick, LookA3.ItemClick, LookA4.ItemClick, LookA5.ItemClick
        LookSkins.EditValue = ""
        AbilitaStile(e.Item, LookSkins.EditValue)
    End Sub
    Private Sub LookSkins_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookSkins.EditValueChanged
        Dim pippo As New BarCheckItem
        AbilitaStile(pippo, LookSkins.EditValue)
    End Sub
    Public Sub AbilitaStile(ByVal ctrl As BarCheckItem, ByVal NomeSkin As String)
        LookA1.Checked = False
        LookA2.Checked = False
        LookA3.Checked = False
        LookA4.Checked = False
        LookA5.Checked = False

        ctrl.Checked = True

        Dim Stile As DevExpress.LookAndFeel.LookAndFeelStyle
        Dim XP As Boolean = False

        If NomeSkin = "" Then
            If LookA1.Checked = True Then
                Stile = DevExpress.LookAndFeel.LookAndFeelStyle.Flat
            ElseIf LookA2.Checked = True Then
                Stile = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
            ElseIf LookA3.Checked = True Then
                Stile = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D
            ElseIf LookA4.Checked = True Then
                Stile = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003
            ElseIf LookA5.Checked = True Then
                XP = True
            End If
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetStyle(Stile, XP, False)
        Else
            Stile = DevExpress.LookAndFeel.LookAndFeelStyle.Skin
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(NomeSkin)
        End If
    End Sub
    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBase3.ItemClick
        DXMenu(New AbiCab, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBase4.ItemClick
        DXMenu(New CodPag, e.Item.Caption)
    End Sub
    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBase1.ItemClick
        DXMenu(New CodIva, e.Item.Caption)
    End Sub
    Private Sub BarButtonItem48_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBase2.ItemClick
        DXMenu(New Gruppi, e.Item.Caption)
    End Sub
    Private Sub BarButtonItem60_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarUtenti1.ItemClick
        DXMenu(New XGestioneMenu, e.Item.Caption)
    End Sub
    Private Sub PictureEdit1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureEdit1.Click
        If GroupControl1.Visible = True Then
            GroupControl1.Visible = False
            MenuRiservato.Visibility = BarItemVisibility.Never
            TextEdit1.Text = ""
        Else
            GroupControl1.Visible = True
            TextEdit1.Focus()
        End If
    End Sub
    Private Sub TextEdit2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit2.Enter
        AbilitaDisabilita()
    End Sub
    Private Sub AbilitaDisabilita()
        Try
            If CDate(TextEdit1.EditValue).ToShortDateString = CDate(Today).ToShortDateString Then
                MenuRiservato.Visibility = BarItemVisibility.Always
            Else
                MenuRiservato.Visibility = BarItemVisibility.Never
            End If
        Catch ex As Exception
            MenuRiservato.Visibility = BarItemVisibility.Never
        End Try
        TextEdit1.Text = ""
        GroupControl1.Visible = False
    End Sub
    Private Sub BarButtonItem1_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        DXMenu(New DxInsClGrCo, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem2_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        DXMenu(New DxBsConti, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem3_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        DXMenu(New DxComForm, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        DXMenu(New DxCeeConti, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        DXMenu(New DxTribForm, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        DXMenu(New DxIstatForm, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem7_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        DXMenu(New DxAtecoD, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem8_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem8.ItemClick
        DXMenu(New DxAziForm, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem9_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem9.ItemClick
        DXMenu(New DxPianoForm, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem10_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem10.ItemClick
        DXMenu(New Clienti, e.Item.Caption, , "A")
    End Sub

    Private Sub BarButtonItem11_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem11.ItemClick
        DXMenu(New Fornitori, e.Item.Caption, , "A")
    End Sub

    Private Sub BarButtonItem12_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem12.ItemClick
        DXMenu(New DxArtPers, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem14_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem14.ItemClick
        DXMenu(New DxInFtCF, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem15_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem15.ItemClick
        DXMenu(New DxInFtCo, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem16_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem16.ItemClick
        DXMenu(New DxInPrNo, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem17_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem17.ItemClick
        DXMenu(New DxSaldaC, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem18_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem18.ItemClick
        DXMenu(New DxSaldaS, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem19_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem19.ItemClick
        DXMenu(New DxSchedC, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem20_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem20.ItemClick
        DXMenu(New DxSparti, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem21_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem21.ItemClick
        DXMenu(New DxStGioG, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem22_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem22.ItemClick
        DXMenu(New DxTrFtXc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem23_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem23.ItemClick
        DXMenu(New DxTrRbXc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem24_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem24.ItemClick
        DXMenu(New DxStReg, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem25_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem25.ItemClick
        DXMenu(New DxRicArc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem26_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem26.ItemClick
        DXMenu(New DxChiApe, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem27_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem27.ItemClick
        DXMenu(New DxSeleSta, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem28_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem28.ItemClick
        DXMenu(New DxStaH7Iva, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem29_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem29.ItemClick
        DXMenu(New DxSkeClFo, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem30_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem30.ItemClick
        DXMenu(New DxSbilveR, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem31_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem31.ItemClick
        DXMenu(New DxBilBil, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem32_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem32.ItemClick
        DXMenu(New DxBilAnn, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem33_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem33.ItemClick
        DXMenu(New DxScriRet, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem34_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem34.ItemClick
        DXMenu(New DxSbilveR, e.Item.Caption, "C")
    End Sub

    Private Sub BarButtonItem35_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem35.ItemClick
        DXMenu(New DxStReIv, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem36_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem36.ItemClick
        DXMenu(New DxChPeIv, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem37_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem37.ItemClick
        DXMenu(New DxVersam, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem38_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem38.ItemClick
        DXMenu(New DxCorIva, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem39_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem39.ItemClick
        DXMenu(New DxRegIva, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem40_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem40.ItemClick
        DXMenu(New DxQuadriva, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem41_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem41.ItemClick
        DXMenu(New DxStaSic, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem42_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem42.ItemClick
        DXMenu(New DxAccIva, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem43_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem43.ItemClick
        DXMenu(New DxCntIva, e.Item.Caption)
    End Sub

    ''Private Sub BarButtonItem44_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem44.ItemClick
    ''    '       FMenu(New EleClFo, e.Item.Caption)
    ''End Sub

    Private Sub BarButtonItem45_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem45.ItemClick
        DXMenu(New DxStTotIcf, e.Item.Caption)
    End Sub

    ''Private Sub BarButtonItem46_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
    ''    '     FMenu(New StaDiffEle, e.Item.Caption)
    ''End Sub

    Private Sub BarButtonItem47_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem47.ItemClick
        DXMenu(New DxInsCesp, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem48_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem48.ItemClick
        DXMenu(New DxProgQuo, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem49_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem49.ItemClick
        DXMenu(New DxRegQuo, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem50_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem50.ItemClick
        DXMenu(New DxPrtCes, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem51_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem51.ItemClick
        DXMenu(New DxLibroCesp, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem52_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem52.ItemClick
        DXMenu(New DxProRit, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem53_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem53.ItemClick
        DXMenu(New DxQui770, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem54_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem54.ItemClick
        DXMenu(New DxCer770, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem55_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem55.ItemClick
        DXMenu(New DxEnasarco, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem56_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem56.ItemClick
        DXMenu(New DxConAge, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem57_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem57.ItemClick
        DXMenu(New DxCon770, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem58_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem58.ItemClick
        DXMenu(New DxScaden, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem59_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem59.ItemClick
        DXMenu(New DxEstrattoNew, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem60_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem60.ItemClick
        DXMenu(New DxStaScaden, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem64_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem64.ItemClick
        DXMenu(New DxBilInv, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem65_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem65.ItemClick
        DXMenu(New Stampanti, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem66_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem66.ItemClick
        DXMenu(New DxAzzero, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem67_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem67.ItemClick
        DXMenu(New DxBusteCo, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem68_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem68.ItemClick
        DXMenu(New DxStaPiv, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem69_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem69.ItemClick
        DXMenu(New DxConPar, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem70_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem70.ItemClick
        DXMenu(New DxVendIntra, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem71_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        '   FMenu(New Pdcdc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem72_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        '  FMenu(New CoCoge, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem73_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        '  FMenu(New BilCdc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem74_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        '  FMenu(New SpaCdC, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem75_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        '    FMenu(New StatCdc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem76_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        '  FMenu(New FilCdc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem77_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        ' FMenu(New BilCdcC, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem78_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        '   FMenu(New FilCdcC, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem79_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        ' FMenu(New QuadCdc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem80_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem80.ItemClick
        DXMenu(New DxAccessi, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem81_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem81.ItemClick
        DXMenu(New DxEliminaLock, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem82_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem82.ItemClick
        FMenu(New NAffitti, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem83_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem83.ItemClick
        FMenu(New RSStaCli, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem84_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem84.ItemClick
        FMenu(New Servizi, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem85_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem85.ItemClick
        FMenu(New FatNcr, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem86_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem86.ItemClick
        FMenu(New TabVend, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem87_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem87.ItemClick
        FMenu(New RSProspetto, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem88_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem88.ItemClick
        FMenu(New RSIndici, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem89_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem89.ItemClick
        DXMenu(New DxScaClf, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem61_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem61.ItemClick
        DXMenu(New ArcImm, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem90_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem90.ItemClick
        DXMenu(New Inquilini, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem91_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem91.ItemClick
        DXMenu(New StImmobili, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem92_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem92.ItemClick
        DXMenu(New StInquilini, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem93_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        REM   DXMenu(New NewCECdc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem94_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        ' FMenu(New SpaRep, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem95_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        REM   DXMenu(New MolCdc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem96_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        REM    DXMenu(New NReport, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem97_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem97.ItemClick
        DXMenu(New DxVentAnn, e.Item.Caption)
    End Sub
    Private Sub BarButtonItem98_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem98.ItemClick
        DXMenu(New NewBan, e.Item.Caption)
    End Sub
    Private Sub BarButtonItem44_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem44.ItemClick
        DXMenu(New MONDOSbilve, e.Item.Caption)
    End Sub
    ''DEFENDINI
    ''Private Sub BarButtonItem46_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem46.ItemClick
    ''    DXMenu(New DxBilCdc, e.Item.Caption)
    ''End Sub
    Private Sub BarButtonItem46_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem46.ItemClick
        DXMenu(New MONDOSchedC, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem73_ItemClick_1(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem73.ItemClick
        DXMenu(New DxBlack, e.Item.Caption)
    End Sub

    '''Private Sub BarButtonItem75_ItemClick_1(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem75.ItemClick
    '''    DXMenu(New DxElenIva, e.Item.Caption)
    '''End Sub

    Private Sub BarButtonItem77_ItemClick_1(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem77.ItemClick
        DXMenu(New BiEnneFat, e.Item.Caption)
    End Sub

    Private Sub BarEditItem2_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BarEditItem2.EditValueChanged
        Select Case BarEditItem2.EditValue
            Case "2010"
                DXMenu(New DxElenIva, "Elenchi Iva Anno " & BarEditItem2.EditValue)
            Case "2011"
                DXMenu(New DxElenIva3000, "Elenchi Iva Anno " & BarEditItem2.EditValue)
            Case "2012"
                DXMenu(New DxSp2012, "Elenchi Iva Anno " & BarEditItem2.EditValue)
            Case "2013"
                DXMenu(New DxSp2013, "Elenchi Iva Anno " & BarEditItem2.EditValue)
            Case "2014"
                DXMenu(New DxSp2014, "Elenchi Iva Anno " & BarEditItem2.EditValue)
            Case "2015"
                DXMenu(New DxSp2015, "Elenchi Iva Anno " & BarEditItem2.EditValue)
            Case "2016"
                DXMenu(New DxSp2016, "Elenchi Iva Anno " & BarEditItem2.EditValue)
            Case "2017"
                DXMenu(New DxSp2017, "Elenchi Iva Anno " & BarEditItem2.EditValue)
            Case "2018"
                DXMenu(New DxSp2018, "Elenchi Iva Anno " & BarEditItem2.EditValue)
            Case "2019"
                DXMenu(New DxSp2019, "Esterometro Anno " & BarEditItem2.EditValue)
            Case "2020"
                DXMenu(New DxSp2020, "Esterometro Anno " & BarEditItem2.EditValue)
            Case "2021"
                DXMenu(New DxSp2021, "Esterometro Anno " & BarEditItem2.EditValue)
            Case "2022"
                DXMenu(New DxSp2022, "Esterometro Anno " & BarEditItem2.EditValue)
        End Select
        BarEditItem2.EditValue = ""
        System.Windows.Forms.SendKeys.Send("{ENTER}")
    End Sub

    Private Sub BarButtonItem75_ItemClick_1(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem75.ItemClick
        DXMenu(New DxAgeing, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem79_ItemClick_1(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem79.ItemClick
        DXMenu(New DxInvest, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem94_ItemClick_1(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem94.ItemClick
        DXMenu(New DxLetInt, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem95_ItemClick_1(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem95.ItemClick
        DXMenu(New DxPaindex, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem96_ItemClick_1(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem96.ItemClick
        DXMenu(New DxRiparto, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem99_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem99.ItemClick
        DXMenu(New DxReportX, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem100_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem100.ItemClick
        DXMenu(New ScaClAge, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem101_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem101.ItemClick
        DXMenu(New CoLiTrim, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem102_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem102.ItemClick
        DXMenu(New RicevPag, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem104_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem104.ItemClick
        DXMenu(New TbPaesi, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem105_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem105.ItemClick
        DXMenu(New TestComunita, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem106_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem106.ItemClick
        DXMenu(New LDPArc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem107_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem107.ItemClick
        DXMenu(New LDPRiparto, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem111_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem111.ItemClick
        DXMenu(New DxBilCdc, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem108_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem108.ItemClick
        DXMenu(New LDPSchedLDP, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem109_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem109.ItemClick
        DXMenu(New DxAgeingLDP, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem110_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem110.ItemClick
        DXMenu(New LDPReportX, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem112_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem112.ItemClick
        DXMenu(New Insoluti, e.Item.Caption)
    End Sub

    Private Sub BarButtonItem113_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem113.ItemClick
        DXMenu(New DxStTotIcli, e.Item.Caption)
    End Sub

End Class