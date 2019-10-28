Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports NPRINT
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class Servizi
    Inherits DXBASE.WinBase
#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()

        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

    End Sub

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form.
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    Friend WithEvents ButtonF9 As System.windows.forms.button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonF11 As System.windows.forms.button
    Friend WithEvents ButtonF5 As System.windows.forms.button
    Friend WithEvents ButtonF3 As System.windows.forms.button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn1 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn2 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn3 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents ButtonF8 As System.windows.forms.button
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents DataGridTextBoxColumn4 As System.Windows.Forms.DataGridTextBoxColumn
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Servizi))
        Me.ButtonF9 = New System.windows.forms.button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.ButtonF8 = New System.windows.forms.button
        Me.ButtonF11 = New System.windows.forms.button
        Me.ButtonF5 = New System.windows.forms.button
        Me.ButtonF3 = New System.windows.forms.button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.DataGrid1 = New System.Windows.Forms.DataGrid
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle
        Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn2 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn3 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn4 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonF9
        '
        Me.ButtonF9.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF9.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF9.ImageIndex = 8
        Me.ButtonF9.ImageList = Me.ImageList1_32
        Me.ButtonF9.Location = New System.Drawing.Point(10, 160)
        Me.ButtonF9.Name = "ButtonF9"
        Me.ButtonF9.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF9.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.ButtonF9, "F9 - Stampa")
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox3.Controls.Add(Me.ButtonF8)
        Me.GroupBox3.Controls.Add(Me.ButtonF11)
        Me.GroupBox3.Controls.Add(Me.ButtonF5)
        Me.GroupBox3.Controls.Add(Me.ButtonF3)
        Me.GroupBox3.Controls.Add(Me.ButtonF9)
        Me.GroupBox3.Location = New System.Drawing.Point(816, 92)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(56, 448)
        Me.GroupBox3.TabIndex = 27
        Me.GroupBox3.TabStop = False
        '
        'ButtonF8
        '
        Me.ButtonF8.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF8.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF8.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF8.ImageIndex = 7
        Me.ButtonF8.ImageList = Me.ImageList1_32
        Me.ButtonF8.Location = New System.Drawing.Point(10, 216)
        Me.ButtonF8.Name = "ButtonF8"
        Me.ButtonF8.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF8.TabIndex = 11
        Me.ButtonF8.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ButtonF8, "F8 - Ricerca Codici Iva")
        '
        'ButtonF11
        '
        Me.ButtonF11.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF11.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF11.ImageIndex = 10
        Me.ButtonF11.ImageList = Me.ImageList1_32
        Me.ButtonF11.Location = New System.Drawing.Point(10, 384)
        Me.ButtonF11.Name = "ButtonF11"
        Me.ButtonF11.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF11.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.ButtonF11, "F11 - Aggiorna")
        '
        'ButtonF5
        '
        Me.ButtonF5.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF5.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF5.ImageIndex = 4
        Me.ButtonF5.ImageList = Me.ImageList1_32
        Me.ButtonF5.Location = New System.Drawing.Point(10, 328)
        Me.ButtonF5.Name = "ButtonF5"
        Me.ButtonF5.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF5.TabIndex = 1
        Me.ButtonF5.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ButtonF5, "F5 - Reset")
        '
        'ButtonF3
        '
        Me.ButtonF3.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF3.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF3.ImageIndex = 2
        Me.ButtonF3.ImageList = Me.ImageList1_32
        Me.ButtonF3.Location = New System.Drawing.Point(10, 272)
        Me.ButtonF3.Name = "ButtonF3"
        Me.ButtonF3.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF3.TabIndex = 2
        Me.ButtonF3.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ButtonF3, "F3 - Annulla")
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox1.Controls.Add(Me.TextBox6)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.TextBox5)
        Me.GroupBox1.Controls.Add(Me.DataGrid1)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.TextBox3)
        Me.GroupBox1.Controls.Add(Me.TextBox4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Location = New System.Drawing.Point(141, 68)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(635, 520)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(272, 480)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ReadOnly = True
        Me.TextBox6.Size = New System.Drawing.Size(304, 22)
        Me.TextBox6.TabIndex = 22
        Me.TextBox6.TabStop = False
        Me.TextBox6.Text = ""
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(584, 400)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 16)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "Cpt"
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(576, 416)
        Me.TextBox5.MaxLength = 5
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(48, 22)
        Me.TextBox5.TabIndex = 4
        Me.TextBox5.Text = ""
        Me.TextBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DataGrid1
        '
        Me.DataGrid1.AlternatingBackColor = System.Drawing.Color.GhostWhite
        Me.DataGrid1.BackColor = System.Drawing.Color.GhostWhite
        Me.DataGrid1.BackgroundColor = System.Drawing.Color.White
        Me.DataGrid1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGrid1.CaptionBackColor = System.Drawing.Color.RoyalBlue
        Me.DataGrid1.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.DataGrid1.CaptionForeColor = System.Drawing.Color.White
        Me.DataGrid1.CaptionVisible = False
        Me.DataGrid1.DataMember = ""
        Me.DataGrid1.Dock = System.Windows.Forms.DockStyle.Top
        Me.DataGrid1.FlatMode = True
        Me.DataGrid1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.DataGrid1.ForeColor = System.Drawing.Color.MidnightBlue
        Me.DataGrid1.GridLineColor = System.Drawing.Color.RoyalBlue
        Me.DataGrid1.HeaderBackColor = System.Drawing.Color.MidnightBlue
        Me.DataGrid1.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.DataGrid1.HeaderForeColor = System.Drawing.Color.Lavender
        Me.DataGrid1.LinkColor = System.Drawing.Color.Teal
        Me.DataGrid1.Location = New System.Drawing.Point(3, 18)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.ParentRowsBackColor = System.Drawing.Color.Lavender
        Me.DataGrid1.ParentRowsForeColor = System.Drawing.Color.MidnightBlue
        Me.DataGrid1.ReadOnly = True
        Me.DataGrid1.SelectionBackColor = System.Drawing.Color.Teal
        Me.DataGrid1.SelectionForeColor = System.Drawing.Color.PaleGreen
        Me.DataGrid1.Size = New System.Drawing.Size(629, 382)
        Me.DataGrid1.TabIndex = 0
        Me.DataGrid1.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
        Me.DataGrid1.TabStop = False
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.DataGrid1
        Me.DataGridTableStyle1.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn1, Me.DataGridTextBoxColumn2, Me.DataGridTextBoxColumn3, Me.DataGridTextBoxColumn4})
        Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridTableStyle1.MappingName = "TbSer"
        Me.DataGridTableStyle1.ReadOnly = True
        Me.DataGridTableStyle1.RowHeadersVisible = False
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.HeaderText = "Codice"
        Me.DataGridTextBoxColumn1.MappingName = "SerCod"
        Me.DataGridTextBoxColumn1.NullText = ""
        Me.DataGridTextBoxColumn1.ReadOnly = True
        Me.DataGridTextBoxColumn1.Width = 64
        '
        'DataGridTextBoxColumn2
        '
        Me.DataGridTextBoxColumn2.Format = ""
        Me.DataGridTextBoxColumn2.FormatInfo = Nothing
        Me.DataGridTextBoxColumn2.HeaderText = "Descrizione Prestazione - Servizio"
        Me.DataGridTextBoxColumn2.MappingName = "SerDesc"
        Me.DataGridTextBoxColumn2.NullText = ""
        Me.DataGridTextBoxColumn2.ReadOnly = True
        Me.DataGridTextBoxColumn2.Width = 472
        '
        'DataGridTextBoxColumn3
        '
        Me.DataGridTextBoxColumn3.Format = ""
        Me.DataGridTextBoxColumn3.FormatInfo = Nothing
        Me.DataGridTextBoxColumn3.HeaderText = "C.Iva"
        Me.DataGridTextBoxColumn3.MappingName = "SerCi"
        Me.DataGridTextBoxColumn3.NullText = ""
        Me.DataGridTextBoxColumn3.Width = 35
        '
        'DataGridTextBoxColumn4
        '
        Me.DataGridTextBoxColumn4.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.DataGridTextBoxColumn4.Format = ""
        Me.DataGridTextBoxColumn4.FormatInfo = Nothing
        Me.DataGridTextBoxColumn4.HeaderText = "Cpt"
        Me.DataGridTextBoxColumn4.MappingName = "SerCpt"
        Me.DataGridTextBoxColumn4.ReadOnly = True
        Me.DataGridTextBoxColumn4.Width = 40
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(67, 416)
        Me.TextBox2.MaxLength = 35
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(472, 22)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.Text = ""
        '
        'TextBox3
        '
        Me.TextBox3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox3.Location = New System.Drawing.Point(539, 416)
        Me.TextBox3.MaxLength = 2
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(35, 22)
        Me.TextBox3.TabIndex = 3
        Me.TextBox3.Text = ""
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox4
        '
        Me.TextBox4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox4.Location = New System.Drawing.Point(400, 448)
        Me.TextBox4.MaxLength = 12
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(175, 22)
        Me.TextBox4.TabIndex = 7
        Me.TextBox4.TabStop = False
        Me.TextBox4.Text = ""
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(536, 400)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 15)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "C. Iva"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(72, 400)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(440, 15)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Descrizione Prestazione - Servizio "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 400)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 15)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Codice"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox1
        '
        Me.TextBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox1.Location = New System.Drawing.Point(3, 416)
        Me.TextBox1.MaxLength = 3
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(64, 22)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.Text = ""
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Servizi
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(1012, 656)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Servizi"
        Me.Text = "Servizi"
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.Controls.SetChildIndex(Me.GroupBox3, 0)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Dim Rpt As ReportClass
    Dim Rpt1 As StServizi
    Dim DsSer As DataSet
    Dim DaSer As SqlDataAdapter
    Dim CbSer As SqlCommandBuilder
    Dim Ser As String = "TbSer"
    Dim EsisteRiga As Boolean
    Dim MaxRig, How As Int16
    Dim TesTest, R As Int16
    Private Sub SerConti_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Pulizia(True)
        PopolaGrid()
    End Sub

    Private Sub PopolaGrid()
        Dim str As String
        str = "SELECT * FROM TbSer"
        DaSer = New SqlDataAdapter(str, cnDb)
        DsSer = New DataSet(Ser)
        DaSer.Fill(DsSer, Ser)
        CbSer = New SqlCommandBuilder(DaSer)
        DataGrid1.DataSource = DsSer.Tables(Ser)
        DataGrid1.Refresh()
    End Sub

    Private Sub Pulizia(ByVal Puliscitutto As Boolean)
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""

        iset = -1
        EsisteRiga = False
        If Puliscitutto = True Then
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If controllo() = False Then
            Exit Sub
        End If
        AggiornoTes()
        If EsisteRiga = False Then
            DsSer.Tables(Ser).Rows.Add(RwTes)
            iset = DsSer.Tables(Ser).Rows.Count - 1
            EsisteRiga = True
        End If
        DaSer.Update(DsSer, Ser)
        DsSer.AcceptChanges()
        Selezione(1, How, DataGrid1)
        iset = -1
        Pulizia(True)
        TextBox1.Focus()
    End Sub

    Private Function controllo() As Boolean
        controllo = True
        If Not IsNumeric(TextBox1.Text) Then
            Return False
        End If
        If TextBox2.Text.Trim = "" Then
            TextBox2.Focus()
            Return False
        End If
        If Val(TextBox3.Text) = 0 Then
            MessageBox.Show("Selezionare un codice iva valido", "ATTENZIONE!!!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox3.Focus()
            Return False
        End If
        If TextBox6.Text = "" Then
            MessageBox.Show("Selezionare una contropartita valida", "ATTENZIONE!!!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox5.Focus()
            Return False
        End If
    End Function

    Private Sub AggiornoTes()
        RwTes("SerCod") = Format(Val(TextBox1.Text), "000")
        RwTes("SerDesc") = TextBox2.Text
        RwTes("SerCi") = Format(Val(TextBox3.Text), "00")
        RwTes("SerCpt") = TextBox5.Text
    End Sub

    Private Sub TextBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If TextBox1.Text <> "" Then
            TextBox1.Text = Val(TextBox1.Text).ToString("000")
        End If
        If ControlloCodice(TextBox1.Text) = False Then
            Exit Sub
        End If
        MaxRig = DsSer.Tables(Ser).Rows.Count
        If MaxRig > 0 And How > -1 Then Selezione(2, How, DataGrid1)
        If iset > -1 Then
            RwTes = DsSer.Tables(Ser).Rows(iset)
            EsisteRiga = True
            How = iset
            Selezione(0, How, DataGrid1)
            Exit Sub
        End If
        If Esiste() = False Then
            AzzeradataRow()
            EsisteRiga = False
            How = MaxRig
            TextBox2.Text = ""
        Else
            EsisteRiga = True
            Selezione(0, How, DataGrid1)
            Lettura(TextBox1.Text)
        End If
        TextBox2.Focus()
    End Sub

    Private Sub AzzeradataRow()
        RwTes = DsSer.Tables(Ser).NewRow()
        RwTes("SerCod") = TextBox1.Text
        RwTes("SerDesc") = ""
        RwTes("SerCi") = 10
    End Sub

    Private Function Esiste() As Boolean
        Cmd = New SqlCommand("select * from TbSer where SerCod ='" & TextBox1.Text & "'", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            For I As Int16 = 0 To DsSer.Tables(Ser).Rows.Count - 1
                If DataGrid1.Item(I, 0) = TextBox1.Text Then
                    RwTes = DsSer.Tables(Ser).Rows(I)
                    How = I
                    dataRd.Close()
                    Return True
                End If
            Next
        End If
        dataRd.Close()
        Return False
    End Function

    Private Function ControlloCodice(ByVal cod As String) As Boolean
        ControlloCodice = True
        If Val(cod) < 1 Then
            Return False
        End If
    End Function

    Private Sub Lettura(ByVal cod As String)
        Dim str As String = "SELECT * from TbSer where SerCod = '" & cod & "'"
        Dim cmd As New SqlCommand(str, cnDb)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            CaricaElementi()
            EsisteRiga = True
            dataRd.Close()
            Return
        End If
        EsisteRiga = False
        dataRd.Close()
    End Sub

    Private Sub CaricaElementi()
        TextBox1.Text = dataRd.Item("SerCod").ToString.Trim
        TextBox2.Text = dataRd.Item("SerDesc").ToString.Trim
        TextBox3.Text = dataRd.Item("SerCi").ToString.Trim
        TextBox5.Text = dataRd.Item("SerCpt").ToString.Trim
        'LeggiDesiva(TextBox3.Text)
    End Sub

    Private Sub DataGrid1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid1.MouseUp
        Dim myGrid As DataGrid = CType(sender, DataGrid)
        Dim myHitInfo As DataGrid.HitTestInfo = myGrid.HitTest(e.X, e.Y)
        If myHitInfo.Row >= 0 Then
            myGrid.Select(myHitInfo.Row)
            TextBox1.Text = DataGrid1.Item(myHitInfo.Row, 0)
            Lettura(TextBox1.Text)
            LeggiDesiva(TextBox3.Text)
            LeggiConto(TextBox5.Text, TextBox6)
            TextBox1.Focus()
            TextBox2.Focus()
        End If
    End Sub

    Sub Selezione(ByVal Rw As Int16, ByVal Ps As Int16, ByVal Grid As DataGrid)
        If Rw = 2 And Ps > -1 And Ps < MaxRig Then
            Grid.UnSelect(Ps)
            Exit Sub
        End If
        If Rw = 1 Then
            Grid.TableStyles(0).SelectionBackColor = System.Drawing.Color.Teal
        Else
            Grid.TableStyles(0).SelectionBackColor = Grid.SelectionBackColor
        End If
        Try
            If Ps > -1 And Ps <= MaxRig Then
                Grid.CurrentRowIndex = Ps
                Grid.Select(Ps)
            End If
        Catch ex As Exception
            Beep()   ' Beep after error processing.
        End Try
    End Sub

    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 Then
            ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            ButtonF8.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 Then
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Pulizia(True)
        DataGrid1.DataSource = Nothing
        PopolaGrid()
        TextBox1.Focus()
    End Sub


    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        eliminaSer(TextBox1.Text)
        TextBox1.Focus()
    End Sub
    Private Sub LeggiDesiva(ByVal ci As String)
        Dim str As String = "Select * from CRIVA where CiiCod = " & ci
        Dim cmd As New SqlCommand(str, cnDb)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextBox4.Text = dataRd.Item("CiiDes").ToString.Trim
        Else
            TextBox4.Text = ""
        End If
        dataRd.Close()
    End Sub
    Private Sub eliminaSer(ByVal cod As String)
        If cod = "" Or Val(cod) = 0 Then
            Exit Sub
        End If
        ' verificare x annullo codice
        Dim str As String = "Select * from TbCor where CorCodArt = " & cod
        Dim cmd As New SqlCommand(str, cnDb)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            MessageBox.Show("Impossibile eliminare il codice " & Format(Val(cod), "000") & " ." & Chr(13) & " Il codice è movimentato.", "ELIMINAZIONE IMPOSSIBILE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            dataRd.Close()
            Return
        End If
        dataRd.Close()

        Dim box As Object
        box = MessageBox.Show("Vuoi eliminare il Codice Servizio selezionato?", "ELIMINA CODICE SERVIZIO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)

        If box = DialogResult.No Then
            Exit Sub
        End If

        RwTes("SerCod") = TextBox1.Text
        RwTes("SerDesc") = TextBox2.Text
        RwTes("SerCi") = TextBox3.Text
        RwTes("SerCpt") = TextBox5.Text


        RwTes.Delete()
        DaSer.Update(DsSer, Ser)
        DsSer.AcceptChanges()
        iset = -1
        Pulizia(True)
    End Sub

    Private Sub TextBox3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.LostFocus
        TextBox3.Text = Format(Val(TextBox3.Text), "00")
        LeggiDesiva(TextBox3.Text)
    End Sub
    Private Sub Textbox3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.Enter
        R = 1
    End Sub
    Private Sub Textbox5_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox5.Enter
        R = 2
    End Sub
    Private Sub TextBox5_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox5.LostFocus
        LeggiConto(TextBox5.Text, TextBox6)
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Cursor.Current = Cursors.WaitCursor
        Dim frm As New LpLp
        Rpt = New ReportClass
        Rpt1 = New StServizi
        Rpt = Rpt1

        Rpt.SetParameterValue("Marchio", Marchio)
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        If R = 2 Then
            TextBox5.Text = NCCOM.Query.CercaPia()
            If LeggiConto(TextBox5.Text, TextBox6) = False Then
                TextBox6.Text = ""
                Exit Sub
            End If
            Exit Sub
        End If
        If R = 1 Then
            TextBox3.Text = Query.CercaCii()
            If Val(TextBox3.Text) > 0 And Val(TextBox3.Text) < 73 Then LeggiDesiva(TextBox3.Text)
            Exit Sub
        End If
    End Sub
    Function LeggiConto(ByRef CodCo As String, ByRef Anagraf As TextBox) As Boolean
        Anagraf.Text = ""
        LeggiConto = False
        AggiustaConto(CodCo) ''''' verifica il punto se e' un sottoconto
        Dim Str As String = "SELECT * from TbPia where PiaCodCo = '" & CodCo & "'"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Anagraf.Text = dataRd("PiaAnaCo")
            LeggiConto = True
        End While
        dataRd.Close()
    End Function
    Function AggiustaConto(ByRef CodCo As String)
        Dim x As Int16
        For x = 1 To Len(CodCo)
            If Mid(CodCo, x, 1) = "." Then
                CodCo = Format(Val(Mid(CodCo, 1, x - 1)), "00") & "." & Format(Val(Mid(CodCo, x + 1, Len(CodCo) - (x - 1))), "00")
                Exit Function
            End If
        Next
    End Function
End Class
