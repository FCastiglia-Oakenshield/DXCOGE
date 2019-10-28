Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Partial Public Class DxArtPers

    Private Shared ArtPers As Int16

    Public Shared Property PArtPers() As Int16
        Get
            Return ArtPers
        End Get
        Set(ByVal Value As Int16)
            ArtPers = Value
        End Set
    End Property
    Dim REPORT As New XtraReport
    Dim selectformula, SCRI As String
    Dim DsArt As DataTable
    Dim DaArt As SqlDataAdapter
    Dim TbArt As DataTable
    Dim RwX As DataRow
    Dim Insert As Boolean
    Dim OldValue As String = ""
    Dim P As Boolean = False
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim Irow As Integer
    Private Sub DxArtPers_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        PopolaCb1()
        PopolaCb2()
        ImageComboBoxEdit2.Focus()
    End Sub
    Private Sub Pulizia(ByVal T As Boolean)
        Textedit3.EditValue = ""
        Textedit4.EditValue = ""
        Textedit5.EditValue = ""
        Textedit6.EditValue = ""
        Textedit7.EditValue = ""
        ImageComboBoxEdit1.SelectedIndex = -1
        Irow = -1
        If T = True Then
            P = False
            EnableDisable(T)
            TbArt = New DataTable
            GridControl1.DataSource = TbArt
            ImageComboBoxEdit2.SelectedIndex = -1
            TextEdit1.EditValue = ""
            MarqueeProgressBarControl1.Visible = False
        End If
    End Sub
    Sub EnableDisable(ByVal T As Boolean)
        GroupControl6.Enabled = Not T
        ButtonXF3.Enabled = Not T
        GroupControl7.Enabled = Not T
        Insert = T
        ButtonP.Enabled = T
        ButtonF3.Enabled = T
        ImageComboBoxEdit2.Enabled = T
        ButtonF1.Enabled = T
    End Sub
    Private Sub PopolaCb1()
        ImageComboBoxEdit1.Properties.Items.Clear()
        Dim Str As String = "SELECT CiiCod,CiiCau from TbCii where CiiCod > 3 order by CiiCod"
        Dim SS As String = ""
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
            ImageComboBoxEdit1.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Sub PopolaCb2()
        ImageComboBoxEdit2.Properties.Items.Clear()
        Dim str As String = "Select distinct ArtPId,ArtPSigla from TbArtP ORDER BY ArtPId"
        Dim SS As String = ""
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SS = dataRd.Item("ArtPId").ToString.PadRight(3, " ") & " " & dataRd.Item("ArtPSigla")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("ArtPId"), -1)
            ImageComboBoxEdit2.Properties.Items.Add(nn)
        End While
        dataRd.Close()
    End Sub
    Private Sub ImageComboBoxEdit1_CloseUp(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles ImageComboBoxEdit2.CloseUp, ImageComboBoxEdit1.CloseUp
        If P = False Then System.Windows.Forms.SendKeys.Send("{TAB}")
    End Sub
    Private Sub ImageComboBoxEdit2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit2.SelectedIndexChanged
        If ImageComboBoxEdit2.SelectedIndex > -1 Then
            PopolaGrid(ImageComboBoxEdit2.EditValue)
        End If
    End Sub
    Private Sub PopolaGrid(ByVal ArtPId As Int32)
        Dim Str As String = "Select * from VArtP where ArtPId=@ArtPId order by ArtPId,ArtPProg"
        Dim p1 As New SqlParameter("@ArtPId", SqlDbType.Int)
        p1.Value = ArtPId
        TbArt = New DataTable()
        DaArt = New SqlDataAdapter(Str, cnCo)
        DaArt.SelectCommand.Parameters.Add(p1)
        DaArt.Fill(TbArt)
        GridControl1.DataSource = TbArt
        GridView1.UnselectRow(0)
        GridView1.ClearSelection()
    End Sub

    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If ImageComboBoxEdit2.SelectedIndex = -1 Then
            ImageComboBoxEdit2.Focus()
            Exit Sub
        End If
        EnableDisable(False)
        TextEdit1.EditValue = Trim(Mid(ImageComboBoxEdit2.Text, Len(ImageComboBoxEdit2.EditValue) + 1, 30))
    End Sub
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub

    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If iset > -1 And GroupControl6.Enabled = True Then
            RwX = GridView1.GetDataRow(iset)
            ImageComboBoxEdit1.SelectedIndex = SettaComboImage(ImageComboBoxEdit1, RwX("ArtPCausale"))
            Textedit3.EditValue = RwX("ArtPDare")
            Textedit4.EditValue = RwX("DescDare")
            Textedit5.EditValue = RwX("ArtPAvere")
            Textedit6.EditValue = RwX("DescAvere")
            Textedit7.EditValue = RwX("ArtPDesc1")
            Irow = iset
            ImageComboBoxEdit1.Focus()
        End If
    End Sub
    Private Sub Textedit3_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Textedit3.Validated, Textedit5.Validated
        Dim t As TextEdit = CType(sender, TextEdit)
        If t.EditValue.Trim = "." Or Len(t.EditValue.trim) = 0 Then t.EditValue = "00.10"
        If sender Is Textedit3 Then
            If Textedit3.EditValue.Trim.ToLower = "f" Then
                Textedit4.EditValue = "Fornitori"
            ElseIf Textedit3.EditValue.Trim.ToLower = "c" Then
                Textedit4.EditValue = "Clienti"
            ElseIf Textedit3.EditValue.Trim.ToLower = "s" Then
                Textedit4.EditValue = "Sottoconto"
            ElseIf IsNumeric(Textedit3.EditValue.Trim) And Not Mid(Textedit3.EditValue, 3, 1) = "." Then
                LeggiClFo(Textedit3.EditValue, Textedit4.EditValue)
            Else
                Textedit4.EditValue = LeggiCpt(Textedit3.EditValue)
                If Textedit4.EditValue = "" Then Textedit3.EditValue = "00.10"
                Textedit4.EditValue = LeggiCpt(Textedit3.EditValue)
            End If
        End If
        If sender Is Textedit5 Then
            If Textedit5.EditValue.Trim.ToLower = "f" Then
                Textedit6.EditValue = "Fornitori"
            ElseIf Textedit5.EditValue.Trim.ToLower = "c" Then
                Textedit6.EditValue = "Clienti"
            ElseIf Textedit5.EditValue.Trim.ToLower = "s" Then
                Textedit6.EditValue = "Sottoconto"
            ElseIf IsNumeric(Textedit5.EditValue.Trim) And Not Mid(Textedit5.EditValue, 3, 1) = "." Then
                LeggiClFo(Textedit5.EditValue, Textedit6.EditValue)
            Else
                Textedit6.EditValue = LeggiCpt(Textedit5.EditValue)
                If Textedit6.EditValue = "" Then Textedit5.EditValue = "00.10"
                Textedit6.EditValue = LeggiCpt(Textedit5.EditValue)
            End If
        End If
    End Sub

    Private Sub LeggiClFo(ByRef Codice As String, ByRef Desc As String)
        Codice = Codice.Trim.PadLeft(5, "0")
        Cmd = New SqlCommand("select * from TbAna where anaCod=@Cod", cnVd)
        Dim p1 As New SqlParameter("@Cod", SqlDbType.VarChar)
        p1.Value = Codice
        Cmd.Parameters.Add(p1)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Desc = dataRd.Item("AnaDesc")
        Else
            Desc = "" : Codice = ""
        End If
        dataRd.Close()
        If Codice = "" Then Codice = "00.10" : Desc = LeggiCpt(Codice)
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF11.Click
        If checkDati() = False Then Exit Sub
        AggiornaTabella()
        Pulizia(False)
        ImageComboBoxEdit1.Focus()
    End Sub
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF3.Click
        If Irow > -1 Then RwX.Delete()
        Pulizia(False)
    End Sub
    Sub AggiornaTabella()
        If Irow > -1 Then
            RwX("ArtPCausale") = ImageComboBoxEdit1.EditValue
            RwX("ArtPDare") = Textedit3.EditValue
            RwX("DescDare") = Textedit4.EditValue
            RwX("ArtPAvere") = Textedit5.EditValue
            RwX("DescAvere") = Textedit6.EditValue
            RwX("ArtPDesc1") = Textedit7.EditValue
            GridView1.UnselectRow(Irow)
            GridView1.ClearSelection()
        Else
            RwX = TbArt.NewRow
            RwX("ArtPCausale") = ImageComboBoxEdit1.EditValue
            RwX("CiiCau") = Trim(Mid(ImageComboBoxEdit1.Text, Len(ImageComboBoxEdit1.EditValue) + 1, 15))
            RwX("ArtPDare") = Textedit3.EditValue
            RwX("DescDare") = Textedit4.EditValue
            RwX("ArtPAvere") = Textedit5.EditValue
            RwX("DescAvere") = Textedit6.EditValue
            RwX("ArtPDesc1") = Textedit7.EditValue
            TbArt.Rows.Add(RwX)
        End If

    End Sub
    Private Function checkDati() As Boolean
        Dim Text As String = ""
        If ImageComboBoxEdit1.SelectedIndex = -1 Then
            Text &= "<> Manca la Causale" & Chr(13)
        End If
        If Not Textedit3.EditValue.Trim > "" Or Not Textedit4.EditValue.Trim > "" Then
            Text &= "<> Manca il Conto Dare" & Chr(13)
        End If
        If Not Textedit5.EditValue.Trim > "" Or Not Textedit6.EditValue.Trim > "" Then
            Text &= "<> Manca il Conto Avere" & Chr(13)
        End If
        If Mid(Textedit5.EditValue.Trim, 4, 2) = "00" Then
            Text &= "<> E' Stato selezionato un Mastro nel Conto Avere!!!" & Chr(13)
        End If
        If Mid(Textedit3.EditValue.Trim, 4, 2) = "00" Then
            Text &= "<> E' Stato selezionato un Mastro nel Conto Dare!!!" & Chr(13)
        End If
        If Textedit3.EditValue.Trim = Textedit5.EditValue.Trim Then
            Text &= "<> Il Conto Dare non può essere uguale al conto Avere" & Chr(13)
        End If
        If Text > "" Then
            MessageBox.Show("Impossibile continuare, si sono verificati i seguenti errori:" & Chr(13) & Chr(13) & Text, "Operazione Interrotta", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
            Exit Function
        End If
        Return True
    End Function

    Private Sub ButtonR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Len(TextEdit1.EditValue.trim) = 0 Then
            TextEdit1.ErrorText = "INDICARE SIGLA ARTICOLO"
            TextEdit1.Focus()
            Exit Sub
        End If
        RegistraDatabase()
        ButtonF5.PerformClick()
    End Sub
    Sub DeleteDatabase()
        Dim DD As String = "Delete from TbArtP where ArtPId=@ArtPId"
        Dim DDelete As New SqlCommand(DD, cnCo)
        Dim d1 As New SqlParameter("@ArtPId", SqlDbType.Int)
        d1.Value = ImageComboBoxEdit2.EditValue
        DDelete.Parameters.Clear()
        DDelete.Parameters.Add(d1)
        DDelete.ExecuteNonQuery()
    End Sub
    Function RitornaDataBase() As Integer
        Dim Str As String = "Select ISNULL(Max(ArtPId),0) from TbArtP"
        Dim RT As New SqlCommand(Str, cnCo)
        Return RT.ExecuteScalar
    End Function
    Private Sub RegistraDatabase()
        Dim Str As String = "Insert into TbArtP (ArtPId,ArtPProg,ArtPSigla,ArtPCausale,ArtPDare,ArtPAvere,ArtPDesc1,ArtPDesc2) values (@ArtPId,@ArtPProg,@ArtPSigla,@ArtPCausale,@ArtPDare,@ArtPAvere,@ArtPDesc1,@ArtPDesc2)"
        Cmd = New SqlCommand(Str, cnCo)

        Dim p1 As New SqlParameter("@ArtPId", SqlDbType.Int)
        Dim p2 As New SqlParameter("@ArtPProg", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@ArtPSigla", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@ArtPCausale", SqlDbType.SmallInt)
        Dim p5 As New SqlParameter("@ArtPDare", SqlDbType.VarChar)
        Dim p6 As New SqlParameter("@ArtPAvere", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@ArtPDesc1", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@ArtPDesc2", SqlDbType.VarChar)
        Dim Rif As Integer = ImageComboBoxEdit2.EditValue
        If Insert = False Then
            DeleteDatabase()
        Else
            Rif = RitornaDataBase() + 1
        End If
        For i As Int16 = 1 To GridView1.RowCount
            Rw = GridView1.GetDataRow(i - 1)
            p1.Value = Rif
            p2.Value = i
            p3.Value = TextEdit1.EditValue
            p4.Value = Rw("ArtPCausale")
            p5.Value = Rw("ArtPDare")
            p6.Value = Rw("ArtPAvere")
            p7.Value = Rw("ArtPDesc1")
            p8.Value = ""

            Cmd.Parameters.Clear()

            Cmd.Parameters.Add(p1)
            Cmd.Parameters.Add(p2)
            Cmd.Parameters.Add(p3)
            Cmd.Parameters.Add(p4)
            Cmd.Parameters.Add(p5)
            Cmd.Parameters.Add(p6)
            Cmd.Parameters.Add(p7)
            Cmd.Parameters.Add(p8)

            Cmd.ExecuteNonQuery()
        Next

    End Sub

    Private Sub ButtonP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonP.Click
        Pulizia(True)
        EnableDisable(False)
        Insert = True : MarqueeProgressBarControl1.Visible = True
        PopolaGrid(88888888)
    End Sub

    Private Sub ButtonD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If ImageComboBoxEdit2.SelectedIndex = -1 Then
            ImageComboBoxEdit2.Focus()
            Exit Sub
        End If
        ChiediConferma()
        ButtonF5.PerformClick()
    End Sub
    Sub ChiediConferma()
        If MessageBox.Show("SEI SICURO DI ELIMINARE L'ARTICOLO SELEZIONATO?", "ELIMINA ARTICOLO PERSONALIZZATO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            DeleteDatabase()
        End If
    End Sub
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F1 Then
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 Then
            ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 Then
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            F8CLICK()
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
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim StrPrint As String = "SELECT * FROM VArtP"
        DsArt = New DataTable
        DaArt = New SqlDataAdapter(StrPrint, cnCo)
        DaArt.SelectCommand.CommandTimeout = 300
        DaArt.Fill(DsArt)
        selectformula = ""
        REPORT = New DxStaArt
        REPORT.DataSource = DsArt
        REPORT.DataMember = "DsArt"
        REPORT.FilterString = selectformula
        REPORT.Parameters.Item("Marchio").Value = Marchio()
        REPORT.ShowPreview()
    End Sub

    Private Sub ButtonC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF8.Click
        BottoneF8(Textedit3.EditValue, Textedit3.EditValue)
    End Sub
    Private Sub ButtonC2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonYF8.Click
        BottoneF8(Textedit5.EditValue, Textedit6.EditValue) : Exit Sub
    End Sub
    Sub F8CLICK()
        If Textedit3.ContainsFocus = True Then ButtonXF8.PerformClick() : Exit Sub
        If Textedit5.ContainsFocus = True Then ButtonYF8.PerformClick() : Exit Sub
    End Sub

    Private Sub BottoneF8(ByRef Cod As String, ByRef Dest As String)
        If Cod.ToLower = "f" Then
            Dim frm As New RicercaClFo
            frm.StartPosition = FormStartPosition.Manual
            frm.Location = New Point(GroupControl1.Location.X, GroupControl1.Location.Y + 80)
            frm.CliFor = "FO"
            frm.ShowDialog()
            Cod = frm.Codice
            If Cod > "" Then
                LeggiClFo(Cod, Dest)
            End If
            Return
        End If
        If Cod.ToLower = "c" Then
            Dim frm As New RicercaClFo
            frm.StartPosition = FormStartPosition.Manual
            frm.Location = New Point(GroupControl1.Location.X, GroupControl1.Location.Y + 80)
            frm.CliFor = "CL"
            frm.ShowDialog()
            Cod = frm.Codice
            If Cod > "" Then
                LeggiClFo(Cod, Dest)
            End If
            Return
        End If
        Dim Cod1 As String = Query.CercaPia()
        Cod = IIf(Cod1 > "", Cod1, Cod)
        Dest = LeggiCpt(Cod)
    End Sub

End Class