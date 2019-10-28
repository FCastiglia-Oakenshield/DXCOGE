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

Public Class ArcImm
    Dim OkImm As Boolean
    Dim RifImm As Int16
    Dim FL, SW As Boolean
    Dim Irow As Integer

    Dim TbCMG As DataTable
    Dim DaCMG As SqlDataAdapter
    Dim RwD As DataRowView
    Private Sub ArcImm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        FL = False
        PULIZIA()
        LeggiImmobili()
        TextEdit1.Focus()
    End Sub

    Sub PULIZIA()
        TextEdit1.Text = "" : TextEdit3.Text = "" : TextEdit4.Text = "" : TextEdit5.Text = "" : TextEdit6.Text = ""
        TextEdit10.Text = "" : TextEdit11.EditValue = CDec(0.0) : TextEdit12.EditValue = CDec(0.0) : TextEdit13.EditValue = CDec(0.0)
        TextEdit7.Text = "" : TextEdit8.Text = "" : TextEdit9.Text = ""
        TextEdit14.Text = "" : TextEdit15.Text = "" : TextEdit16.Text = ""
        TextEdit2.EditValue = CDec(0.0) : TextEdit17.EditValue = CDec(0.0) : TextEdit18.EditValue = CDec(0.0) : TextEdit19.EditValue = CDec(0.0)
        TextEdit22.EditValue = "" : TextEdit23.EditValue = "" : TextEdit24.EditValue = "" : TextEdit25.EditValue = "" : TextEdit26.EditValue = ""
        TextEdit20.EditValue = CDec(0.0) : TextEdit21.EditValue = CDec(0.0) : TextEdit27.EditValue = CDec(0.0) : TextEdit29.EditValue = CDec(0.0)
        TextEdit28.EditValue = ""
        MemoEdit1.Text = "" : MemoEdit2.EditValue = ""
        CaricaComboBox()
        ComboBoxEdit1.SelectedIndex = -1 : ComboBoxEdit2.SelectedIndex = -1
        Disabilita()
    End Sub
    Sub Disabilita()
        GroupControl2.Enabled = False
        GroupControl3.Enabled = False
        GroupControl4.Enabled = False
        GroupControl1.Enabled = False
        GroupControl9.Enabled = False
        Irow = -1
        GroupControl5.Enabled = True
        GroupControl7.Enabled = True
        GroupControl20.Enabled = True
        ButtonF11.Enabled = False
        ButtonF5.Enabled = True
        ButtonF3.Enabled = False
    End Sub

    Sub Abilita()
        GroupControl2.Enabled = True
        GroupControl3.Enabled = True
        GroupControl4.Enabled = True
        GroupControl1.Enabled = True
        GroupControl9.Enabled = True
        GroupControl5.Enabled = False
        GroupControl7.Enabled = True
        GroupControl20.Enabled = False
        ButtonF11.Enabled = True
        ButtonF5.Enabled = True
        If RifImm > 0 Then ButtonF3.Enabled = True
    End Sub
    Sub CaricaComboBox()
        'Lettura Piani
        ComboBoxEdit1.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT distinct ImmPiano from TbImmobili Order by ImmPiano", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("ImmPiano"))
        End While
        dataRd.Close()

        'Lettura Categorie 
        ComboBoxEdit2.Properties.Items.Clear()
        Cmd = New SqlCommand("SELECT distinct ImmCateg from TbImmobili Order by ImmCateg", cnDb)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            ComboBoxEdit2.Properties.Items.Add(dataRd.Item("ImmCateg"))
        End While
        dataRd.Close()
    End Sub
    Sub LeggiImmobili()
        Dim Str As String = "Select ImmCod,ImmCitta,ImmIndirizzo,ImmPiano,ImmSubnC,ImmCateg,ImmNrIdenCom from VArcImm order by ImmCitta,ImmIndirizzo, ImmPiano,ImmSubnC,ImmNrIdenCom desc"
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
    End Sub
    Sub TbLeggiImm_ENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggiImm.Enter
        'leggo Immobile
        SW = False
        If Val(TextEdit1.Text) > 0 Then
            LeggiImm(Val(TextEdit1.Text))
            If RifImm > 0 Then
                SW = True
                CaricaImmobile()
                Abilita()
                MemoEdit2.Focus()
                'ButtonF5.Focus()
            Else
                TextEdit1.Text = ""
                TextEdit1.Focus()
            End If
        Else
            NuovoImm()
        End If
    End Sub
    Sub NuovoImm()
        RifImm = 0
        Abilita()
        TextEdit7.Focus()
    End Sub
    Private Sub LeggiImm(ByVal Nume As Integer)
        Cmd = New SqlCommand("Select ImmCod from TbImmobili where ImmCod = @Nume", cnDb)
        Dim p2 As New SqlParameter("@Nume", SqlDbType.Int)
        p2.Value = Nume
        Cmd.Parameters.Add(p2)
        RifImm = Cmd.ExecuteScalar
    End Sub

    Private Sub CaricaImmobile()
        RifImm = 0
        OkImm = False
        Cmd = New SqlCommand("Select * from TbImmobili where ImmCod = " & Val(TextEdit1.Text), cnDb)
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

            TextEdit7.Text = dataRd.Item("ImmFgexC")
            TextEdit8.Text = dataRd.Item("ImmNrexC")
            TextEdit9.Text = dataRd.Item("ImmSubexC")
            TextEdit14.Text = dataRd.Item("ImmFgnC")
            TextEdit15.Text = dataRd.Item("ImmNrnC")
            TextEdit16.Text = dataRd.Item("ImmSubnC")
            MemoEdit1.Text = dataRd.Item("ImmNote")
            ComboBoxEdit2.SelectedIndex = SettaComboEdit(ComboBoxEdit2, dataRd.Item("ImmCateg"), 0)
            ComboBoxEdit1.SelectedIndex = SettaComboEdit(ComboBoxEdit1, dataRd.Item("ImmPiano"), 0)

            TextEdit2.EditValue = dataRd.Item("ImmMqNetti")
            TextEdit17.EditValue = dataRd.Item("ImmMillScala")
            TextEdit18.EditValue = dataRd.Item("ImmMillAsce")
            TextEdit19.EditValue = dataRd.Item("ImmRendCata")

            TextEdit22.EditValue = dataRd.Item("ImmClasse")
            TextEdit23.EditValue = dataRd.Item("ImmVani")
            TextEdit24.EditValue = dataRd.Item("ImmNrIdent")
            TextEdit25.EditValue = dataRd.Item("ImmApeNr")
            TextEdit26.EditValue = dataRd.Item("ImmApeClasse")
            TextEdit20.EditValue = dataRd.Item("ImmCatastoSuper")
            TextEdit21.EditValue = dataRd.Item("ImmAreeScop")
            TextEdit27.EditValue = dataRd.Item("ImmMcUnita")
            MemoEdit2.EditValue = dataRd.Item("ImmComposta")
            TextEdit28.EditValue = dataRd.Item("ImmResidenza")
            TextEdit29.EditValue = dataRd.Item("ImmSuperConv")

            OkImm = True
            RifImm = dataRd.Item("ImmCod")
        End If
        dataRd.Close()
        ButtonF5.Focus()
    End Sub
    Function ControllaCampiT() As Boolean
        Dim Mail As String = ""
        Dim cc As New Control
        If TextEdit4.Text = "" Then
            Mail = Mail & "<>Inserire Indirizzo" & Chr(13)
            cc = TextEdit4
        End If

        If TextEdit5.Text = "" Then
            Mail = Mail & "<>Inserire Città" & Chr(13)
            cc = TextEdit5
        End If

        ''If ComboBoxEdit1.SelectedIndex = -1 Then
        ''    Mail = Mail & "<>Inserire Piano" & Chr(13)
        ''    cc = ComboBoxEdit1
        ''End If

        ''If ComboBoxEdit2.SelectedIndex = -1 Then
        ''    Mail = Mail & "<>Inserire Categoria" & Chr(13)
        ''    cc = ComboBoxEdit2
        ''End If

        If Mail > "" Then
            MoltoCritico(Mail)
            cc.Focus()
            Return False
        End If
        Return True
    End Function
    Sub AggiornaImmobili()
        Dim ScriviImm As String
        Dim IDENT As New SqlCommand("SELECT @@IDENTITY ", cnDb)
        If RifImm = 0 Then
            ScriviImm = "Insert Into TbImmobili WITH (TABLOCKX)(ImmIndirizzo,ImmCap,ImmCitta,ImmPv,ImmNrIdenCom,ImmPiano,ImmCateg, ImmMq,ImmMillesimi, ImmMc, ImmFgexC, ImmNrexC, ImmSubexC,ImmFgnC,ImmNrnC,ImmSubnC, ImmNote, ImmMqNetti, ImmMillScala, ImmMillAsce,ImmRendCata,ImmComposta,ImmClasse,ImmVani,ImmCatastoSuper,ImmAreeScop,ImmNrIdent,ImmApeNr,ImmApeClasse,ImmMcUnita,ImmResidenza,ImmSuperConv) " _
                & " VALUES (@ImmIndirizzo,@ImmCap,@ImmCitta,@ImmPv,@ImmNrIdenCom,@ImmPiano,@ImmCateg,@ImmMq,@ImmMillesimi,@ImmMc,@ImmFgexC,@ImmNrexC,@ImmSubexC,@ImmFgnC,@ImmNrnC,@ImmSubnC,@ImmNote,@ImmMqNetti, @ImmMillScala,@ImmMillAsce,@ImmRendCata,@ImmComposta,@ImmClasse,@ImmVani,@ImmCatastoSuper,@ImmAreeScop,@ImmNrIdent,@ImmApeNr,@ImmApeClasse,@ImmMcUnita,@ImmResidenza,@ImmSuperConv) "
        Else
            ScriviImm = "UPDATE TbImmobili SET ImmIndirizzo= @ImmIndirizzo, ImmCap=@ImmCap, ImmCitta=@ImmCitta,ImmPv=@ImmPv,ImmNrIdenCom=@ImmNrIdenCom,ImmPiano=@ImmPiano,ImmCateg=@ImmCateg,ImmMq=@ImmMq,ImmMillesimi=@ImmMillesimi,ImmMc=@ImmMc,ImmFgexC=@ImmFgexC,ImmNrexC=@ImmNrexC,ImmSubexC=@ImmSubexC,ImmFgnC=@ImmFgnC,ImmNrnC=@ImmNrnC,ImmSubnC=@ImmSubnC,ImmNote=@ImmNote," _
                & "ImmMqNetti=@ImmMqNetti,ImmMillScala=@ImmMillScala,ImmMillAsce=@ImmMillAsce,ImmRendCata=@ImmRendCata,ImmComposta=@ImmComposta,ImmClasse=@ImmClasse,ImmVani=@ImmVani,ImmCatastoSuper=@ImmCatastoSuper,ImmAreeScop=@ImmAreeScop,ImmNrIdent=@ImmNrIdent,ImmApeNr=@ImmApeNr,ImmApeClasse=@ImmApeClasse,ImmMcUnita=@ImmMcUnita,ImmResidenza=@ImmResidenza,ImmSuperConv=@ImmSuperConv where ImmCod = " & RifImm
        End If
        Dim B0 As New SqlParameter("@RifImm", SqlDbType.Int)

        Dim p2 As New SqlParameter("@ImmIndirizzo", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@ImmCap", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@ImmCitta", SqlDbType.VarChar)
        Dim p5 As New SqlParameter("@ImmPv", SqlDbType.VarChar)
        Dim p6 As New SqlParameter("@ImmNrIdenCom", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@ImmPiano", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@ImmCateg", SqlDbType.VarChar)

        Dim p9 As New SqlParameter("@ImmMq", SqlDbType.Decimal)
        Dim p10 As New SqlParameter("@ImmMillesimi", SqlDbType.Decimal)
        Dim p11 As New SqlParameter("@ImmMc", SqlDbType.Decimal)

        Dim p12 As New SqlParameter("@ImmFgexC", SqlDbType.VarChar)
        Dim p13 As New SqlParameter("@ImmNrexC", SqlDbType.VarChar)
        Dim p14 As New SqlParameter("@ImmSubexC", SqlDbType.VarChar)
        Dim p15 As New SqlParameter("@ImmFgnC", SqlDbType.VarChar)
        Dim p16 As New SqlParameter("@ImmNrnC", SqlDbType.VarChar)
        Dim p17 As New SqlParameter("@ImmSubnC", SqlDbType.VarChar)
        Dim p18 As New SqlParameter("@ImmNote", SqlDbType.NText)

        Dim p19 As New SqlParameter("@ImmMqNetti", SqlDbType.Decimal)
        Dim p20 As New SqlParameter("@ImmMillScala", SqlDbType.Decimal)
        Dim p21 As New SqlParameter("@ImmMillAsce", SqlDbType.Decimal)
        Dim p22 As New SqlParameter("@ImmRendCata", SqlDbType.Decimal)

        Dim p23 As New SqlParameter("@ImmComposta", SqlDbType.VarChar)
        Dim p24 As New SqlParameter("@ImmClasse", SqlDbType.VarChar)
        Dim p25 As New SqlParameter("@ImmVani", SqlDbType.VarChar)
        Dim p26 As New SqlParameter("@ImmCatastoSuper", SqlDbType.Decimal)
        Dim p27 As New SqlParameter("@ImmAreeScop", SqlDbType.Decimal)
        Dim p28 As New SqlParameter("@ImmNrIdent", SqlDbType.VarChar)
        Dim p29 As New SqlParameter("@ImmApeNr", SqlDbType.VarChar)
        Dim p30 As New SqlParameter("@ImmApeClasse", SqlDbType.VarChar)
        Dim p31 As New SqlParameter("@ImmMcUnita", SqlDbType.Decimal)
        Dim p32 As New SqlParameter("@ImmResidenza", SqlDbType.VarChar)
        Dim p33 As New SqlParameter("@ImmSuperConv", SqlDbType.Decimal)

        p2.Value = TextEdit4.Text
        p3.Value = TextEdit3.Text
        p4.Value = TextEdit5.Text
        p5.Value = TextEdit6.Text
        p6.Value = TextEdit10.Text

        p7.Value = ComboBoxEdit1.Text
        p8.Value = ComboBoxEdit2.Text

        p9.Value = IIf(IsNumeric(TextEdit11.EditValue), TextEdit11.EditValue, 0)
        p10.Value = IIf(IsNumeric(TextEdit13.EditValue), TextEdit13.EditValue, 0)
        p11.Value = IIf(IsNumeric(TextEdit12.EditValue), TextEdit12.EditValue, 0)

        p12.Value = TextEdit7.Text
        p13.Value = TextEdit8.Text
        p14.Value = TextEdit9.Text
        p15.Value = TextEdit14.Text
        p16.Value = TextEdit15.Text
        p17.Value = TextEdit16.Text
        p18.Value = MemoEdit1.Text

        p19.Value = IIf(IsNumeric(TextEdit2.EditValue), TextEdit2.EditValue, 0)
        p20.Value = IIf(IsNumeric(TextEdit17.EditValue), TextEdit17.EditValue, 0)
        p21.Value = IIf(IsNumeric(TextEdit18.EditValue), TextEdit18.EditValue, 0)
        p22.Value = IIf(IsNumeric(TextEdit19.EditValue), TextEdit19.EditValue, 0)

        p23.Value = MemoEdit2.EditValue
        p24.Value = TextEdit22.EditValue
        p25.Value = TextEdit23.EditValue
        p26.Value = TextEdit20.EditValue
        p27.Value = TextEdit21.EditValue
        p28.Value = TextEdit24.EditValue
        p29.Value = TextEdit25.EditValue
        p30.Value = TextEdit26.EditValue
        p31.Value = TextEdit27.EditValue
        p32.Value = TextEdit28.EditValue
        p33.Value = TextEdit29.EditValue

        Cmd = New SqlCommand(ScriviImm, cnDb)

        If RifImm <> 0 Then
            B0.Value = RifImm
            Cmd.Parameters.Add(B0)
        End If
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
        Cmd.Parameters.Add(p20)
        Cmd.Parameters.Add(p21)
        Cmd.Parameters.Add(p22)
        Cmd.Parameters.Add(p23)
        Cmd.Parameters.Add(p24)
        Cmd.Parameters.Add(p25)
        Cmd.Parameters.Add(p26)
        Cmd.Parameters.Add(p27)
        Cmd.Parameters.Add(p28)
        Cmd.Parameters.Add(p29)
        Cmd.Parameters.Add(p30)
        Cmd.Parameters.Add(p31)
        Cmd.Parameters.Add(p32)
        Cmd.Parameters.Add(p33)

        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()

        If RifImm = 0 Then RifImm = IDENT.ExecuteScalar
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If ControllaCampiT() = False Then Exit Sub
        AggiornaImmobili()
        GroupControl20.Enabled = False
        GroupControl4.Enabled = True
        ButtonF5.PerformClick()
        TextEdit1.Focus()
    End Sub '
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        EliminaTutto()
        PULIZIA()
        LeggiImmobili()
    End Sub
    Private Sub EliminaTutto()
        If MessageBox.Show("Elimino l'Immobile selezionato ?", "ELIMINAZIONE IMMOBILE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If
        Cmd = New SqlCommand("Delete from TbImmobili where ImmCod = @RifImm", cnDb)
        Dim p1 As New SqlParameter("@rifImm", SqlDbType.Int)
        p1.Value = RifImm
        Cmd.Parameters.Add(p1)
        Cmd.ExecuteNonQuery()
        Cmd.Parameters.Clear()
        'LeggiImmobili()
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
            RifImm = RwD("ImmCod")
            TextEdit1.Text = RwD("ImmCod")
            SW = True
            CaricaImmobile()
            Abilita()
            MemoEdit2.Focus()
            'ButtonF5.Focus()
        Else
            RifImm = -1
        End If
    End Sub
End Class