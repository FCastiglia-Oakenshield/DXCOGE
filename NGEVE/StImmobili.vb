Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Public Class StImmobili
    Dim TbPrint As DataTable
    Dim DaPrint As SqlDataAdapter
    Dim StrPrint As String
    Dim Em As New DevExpress.XtraEditors.Controls.CheckedListBoxItem
    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim StrWhere As String = ""
    Dim StampaDefault As Integer = -1
    Dim StampaLock As Boolean = False
    Dim PathSto As String = ""
    Dim SalvaFile As String = ""
    Dim P As Boolean = True
    Dim A4A3 As Boolean = False
    Dim OrizVert As Boolean = False
    Private Sub StImmobili_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Pulizia()
        StampaLock = False
        TextE26.ErrorText = ""
        StrWhere = " Order by Indirizzo,Cap,Citta,Pr"
        LeggiParametri()
        PopolaLayoutStampa()
    End Sub
    Sub Pulizia()
        TbPrint = New DataTable
        GridStampeLp.DataSource = TbPrint
    End Sub
    Private Sub StImmobili_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ''ButtonF5.PerformClick()
            Exit Sub
        End If
    End Sub
#Region "MODULO STAMPA"
    Private Sub ButtonP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim Foglio As System.Drawing.Printing.PaperKind
        Dim options As New DevExpress.XtraPrinting.PdfExportOptions
        options.DocumentOptions.Title = GroupTop.Text
        If CheckB4.Checked = True Then Foglio = Printing.PaperKind.A3 Else Foglio = Printing.PaperKind.A4
        Cursor.Current = Cursors.WaitCursor
        DXANTEPRIMA(GridStampeLp, CheckB3.Checked, Foglio, GroupTop.Text)
        Cursor.Current = Cursors.Default
        RegistraUltima()
        If TextE26.Text.Length > 1 Then
            If CONTROLLAFABBRICA() = False Then
                SalvaFile = PathSto & TextE26.EditValue & ".xml"
                AdvBandedGridViewLP.SaveLayoutToXml(SalvaFile)
                RegistraGrid()
                PopolaCb2()
            Else
                TextE26.ErrorText = "INSERIRE NOME MODULO DIVERSO"
            End If
        End If
    End Sub
    Sub RegistraUltima()
        Cmd = New SqlCommand("update TbXml set IdUltima=0 where IdProgramma = '" & Me.Name.ToString & "'", cnVd)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("update TbXml set IdUltima=1 where IdProgramma = '" & Me.Name.ToString & "' and IdNomeStampa = '" & ImageComboB2.EditValue & "'", cnVd)
        Cmd.ExecuteNonQuery()
    End Sub
    Function CONTROLLAFABBRICA() As Boolean
        Dim Ok As Int16 = 0
        Cmd = New SqlCommand("Select IdFabbrica from TbXml where IdProgramma = '" & Me.Name.ToString & "' and IdNomeStampa = '" & TextE26.EditValue & "'", cnVd)
        Ok = Cmd.ExecuteScalar()
        If Ok > 0 Then Return True : Exit Function
        Cmd = New SqlCommand("Select count(*) from TbXml where IdProgramma <> '" & Me.Name.ToString & "' and IdNomeStampa = '" & TextE26.EditValue & "'", cnVd)
        Ok = Cmd.ExecuteScalar()
        If Ok > 0 Then Return True Else Return False
    End Function
    Sub RegistraGrid()
        Cmd = New SqlCommand("delete from TbXml where IdProgramma = '" & Me.Name.ToString & "' and IdNomeStampa = '" & TextE26.EditValue & "'", cnVd)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("update TbXml set IdUltima=0 where IdProgramma = '" & Me.Name.ToString & "'", cnVd)
        Cmd.ExecuteNonQuery()
        Cmd = New SqlCommand("Insert into TbXml(IdProgramma,IdNomeStampa,IdA4A3,IdOrizVert,IdUltima) values (@Programma,@NomeStampa,@A4A3,@OrizVert,@Ultima)", cnVd)
        Dim p1 As New SqlParameter("@Programma", SqlDbType.NVarChar)
        Dim p2 As New SqlParameter("@NomeStampa", SqlDbType.NVarChar)
        Dim p3 As New SqlParameter("@A4A3", SqlDbType.Bit)
        Dim p4 As New SqlParameter("@OrizVert", SqlDbType.Bit)
        Dim p5 As New SqlParameter("@Ultima", SqlDbType.Bit)

        p1.Value = Me.Name.ToString
        p2.Value = TextE26.EditValue
        p3.Value = CheckB4.Checked
        p4.Value = CheckB3.Checked
        p5.Value = True

        Cmd.Parameters.Clear()
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.ExecuteNonQuery()

    End Sub
    Sub PopolaLayoutStampa()
        Cursor.Current = Cursors.WaitCursor
        Dim Str As String = "Select * from DX" & Me.Name.ToString & StrWhere
        TbPrint = New DataTable()
        DaPrint = New SqlDataAdapter(Str, cnDb)
        DaPrint.SelectCommand.CommandTimeout = 1200
        DaPrint.Fill(TbPrint)
        AdvBandedGridViewLP.ClearColumnsFilter()
        GridStampeLp.DataSource = TbPrint
        PopolaCb2()
        P = True
        If ImageComboB2.EditValue > "" Then
            SalvaFile = PathSto & ImageComboB2.EditValue & ".xml"
            RestoreLay()
        End If
        AdvBandedGridViewLP.ClearSelection()
        Cursor.Current = Cursors.Default
    End Sub
    Sub RestoreLay()
        AdvBandedGridViewLP.RestoreLayoutFromXml(SalvaFile)
    End Sub
    Sub PopolaCb2()
        ImageComboB2.Properties.Items.Clear()
        Dim i As Int16 = -1
        Dim SS As Int16 = 27
        StampaDefault = 0
        Cmd = New SqlCommand("SELECT * from TbXml where IdProgramma = '" & Me.Name.ToString & "' order by IdNomeStampa", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            i += 1
            If dataRd.Item("IdFabbrica") = True Then SS = 63 Else SS = 64
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(dataRd.Item("IdNomeStampa"), dataRd.Item("IdNomeStampa"), SS)
            ImageComboB2.Properties.Items.Add(nn)
            If dataRd.Item("IdUltima") = True Then
                StampaDefault = i
                OrizVert = dataRd.Item("IdOrizVert")
                A4A3 = dataRd.Item("IdA4A3")
                StampaLock = dataRd.Item("IdFabbrica")
            End If
        End While
        dataRd.Close()
        ImageComboB2.SelectedIndex = StampaDefault
        CheckB3.Checked = OrizVert
        CheckB4.Checked = A4A3
        ButtonDE.Enabled = Not StampaLock
    End Sub
    Private Sub CheckButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckB3.CheckedChanged
        If CheckB3.Checked = True Then
            CheckB3.ImageIndex = 21
            CheckB3.Text = "ORIZZONTALE"
        Else
            CheckB3.ImageIndex = 22
            CheckB3.Text = "VERTICALE"
        End If
    End Sub

    Private Sub CheckButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckB4.CheckedChanged
        If CheckB4.Checked = True Then
            CheckB4.Image = Nothing
            CheckB4.ImageIndex = 24
            CheckB4.Text = "FOGLIO A3"
        Else
            CheckB4.Image = My.Resources.Resources.A4
            CheckB4.Text = "FOGLIO A4"
        End If
    End Sub

    Private Sub ImageComboBoxEdit1_CloseUp(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles ImageComboB2.CloseUp
        If P = False Then System.Windows.Forms.SendKeys.Send("{TAB}")
    End Sub


    Private Sub ImageComboBoxEdit2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboB2.SelectedIndexChanged
        If ImageComboB2.SelectedIndex > -1 Then
            PopolaGrid(ImageComboB2.EditValue)
            TextE26.EditValue = ImageComboB2.EditValue
            SalvaFile = PathSto & ImageComboB2.EditValue & ".xml"
            RestoreLay()
        End If
        If StampaLock = True Then TextE26.EditValue = "" : TextE26.ErrorText = "INSERIRE NOME MODULO DIVERSO" Else TextE26.ErrorText = ""
    End Sub
    Private Sub PopolaGrid(ByVal NomeStampa As String)
        Cmd = New SqlCommand("SELECT * from TbXml where IdProgramma = '" & Me.Name.ToString & "' and IdNomeStampa = '" & NomeStampa & "'", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            OrizVert = dataRd.Item("IdOrizVert")
            A4A3 = dataRd.Item("IdA4A3")
            StampaLock = dataRd.Item("IdFabbrica")
        End While
        dataRd.Close()
        CheckB3.Checked = OrizVert
        CheckB4.Checked = A4A3
        ButtonDE.Enabled = Not StampaLock
    End Sub
    Private Sub ButtonDX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDE.Click
        If CONTROLLAFABBRICA() = False Then ELIMINAMODULO() : PopolaCb2()
    End Sub
    Sub ELIMINAMODULO()
        Dim box As Object
        box = MessageBox.Show("ELIMINO IL MODULO DI STAMPA? " & TextE26.EditValue, "ELIMINA STAMPE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
        If box = DialogResult.Yes Then
            Cmd = New SqlCommand("delete from TbXml where IdProgramma = '" & Me.Name.ToString & "' and IdNomeStampa = '" & TextE26.EditValue & "'", cnVd)
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub LeggiParametri()
        Dim cmd As New SqlCommand(" SELECT * from TbSel where selId = 1", cnVd)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            PathSto = dataRd.GetString(12) & dataRd.GetString(11)
        End While
        dataRd.Close()
        If Directory.Exists(PathSto) = False Then Directory.CreateDirectory(PathSto)
    End Sub
#End Region

End Class