Imports System.Data.SqlClient
Imports DXBASE
Imports DXBASE.Util
Imports NCCOM
Public Class frmRicSer
    Inherits DXBASE.WinBase

    Private Shared Privcodice As String
    Private Shared ds As DataSet

    Public Shared ReadOnly Property codice()
        Get
            Return Privcodice
        End Get
    End Property

    Public Shared WriteOnly Property dataset() As DataSet
        Set(ByVal Value As DataSet)
            ds = Value
        End Set
    End Property

    Dim iset, contatore As Integer
    Dim Pcl, Hcl, MaxPro, RighePagina, UnoPiu, USelect As Int32
    Dim DGRID As DataGrid = Datagrid1

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
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Datagrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn1 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn2 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonF1 As System.windows.forms.button
    Friend WithEvents ButtonF2 As System.windows.forms.button
    Friend WithEvents ButtonF4 As System.windows.forms.button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmRicSer))
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Datagrid1 = New System.Windows.Forms.DataGrid
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle
        Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn2 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ButtonF1 = New System.windows.forms.button
        Me.ButtonF2 = New System.windows.forms.button
        Me.ButtonF4 = New System.windows.forms.button
        Me.TextBox3 = New System.Windows.Forms.TextBox
        CType(Me.Datagrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(0, 56)
        Me.TextBox1.MaxLength = 5
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(48, 22)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.Text = ""
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(48, 56)
        Me.TextBox2.MaxLength = 60
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(544, 22)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.Text = ""
        '
        'Datagrid1
        '
        Me.Datagrid1.AlternatingBackColor = System.Drawing.Color.GhostWhite
        Me.Datagrid1.BackColor = System.Drawing.Color.GhostWhite
        Me.Datagrid1.BackgroundColor = System.Drawing.Color.White
        Me.Datagrid1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Datagrid1.CaptionBackColor = System.Drawing.Color.RoyalBlue
        Me.Datagrid1.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.Datagrid1.CaptionForeColor = System.Drawing.Color.White
        Me.Datagrid1.CaptionVisible = False
        Me.Datagrid1.DataMember = ""
        Me.Datagrid1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Datagrid1.FlatMode = True
        Me.Datagrid1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.Datagrid1.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Datagrid1.GridLineColor = System.Drawing.Color.RoyalBlue
        Me.Datagrid1.HeaderBackColor = System.Drawing.Color.MidnightBlue
        Me.Datagrid1.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.Datagrid1.HeaderForeColor = System.Drawing.Color.Lavender
        Me.Datagrid1.LinkColor = System.Drawing.Color.Teal
        Me.Datagrid1.Location = New System.Drawing.Point(0, 73)
        Me.Datagrid1.Name = "Datagrid1"
        Me.Datagrid1.ParentRowsBackColor = System.Drawing.Color.Lavender
        Me.Datagrid1.ParentRowsForeColor = System.Drawing.Color.MidnightBlue
        Me.Datagrid1.ParentRowsVisible = False
        Me.Datagrid1.ReadOnly = True
        Me.Datagrid1.RowHeadersVisible = False
        Me.Datagrid1.SelectionBackColor = System.Drawing.Color.Teal
        Me.Datagrid1.SelectionForeColor = System.Drawing.Color.PaleGreen
        Me.Datagrid1.Size = New System.Drawing.Size(616, 469)
        Me.Datagrid1.TabIndex = 26
        Me.Datagrid1.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
        Me.Datagrid1.TabStop = False
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.Datagrid1
        Me.DataGridTableStyle1.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn1, Me.DataGridTextBoxColumn2})
        Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridTableStyle1.MappingName = "TbSer"
        Me.DataGridTableStyle1.ReadOnly = True
        Me.DataGridTableStyle1.RowHeadersVisible = False
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.HeaderText = "Cod."
        Me.DataGridTextBoxColumn1.MappingName = "SerCod"
        Me.DataGridTextBoxColumn1.ReadOnly = True
        Me.DataGridTextBoxColumn1.Width = 48
        '
        'DataGridTextBoxColumn2
        '
        Me.DataGridTextBoxColumn2.Format = ""
        Me.DataGridTextBoxColumn2.FormatInfo = Nothing
        Me.DataGridTextBoxColumn2.HeaderText = "Servizio o Prestazione"
        Me.DataGridTextBoxColumn2.MappingName = "SerDesc"
        Me.DataGridTextBoxColumn2.Width = 544
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(0, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 46
        Me.Label1.Text = "Codice"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(96, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 16)
        Me.Label2.TabIndex = 47
        Me.Label2.Text = "Descrizione"
        '
        'ButtonF1
        '
        Me.ButtonF1.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF1.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF1.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF1.ImageIndex = 0
        Me.ButtonF1.ImageList = Me.ImageList1_32
        Me.ButtonF1.Location = New System.Drawing.Point(208, 8)
        Me.ButtonF1.Name = "ButtonF1"
        Me.ButtonF1.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF1.TabIndex = 48
        Me.ButtonF1.TabStop = False
        Me.ButtonF1.Visible = False
        '
        'ButtonF2
        '
        Me.ButtonF2.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF2.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF2.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF2.ImageIndex = 1
        Me.ButtonF2.ImageList = Me.ImageList1_32
        Me.ButtonF2.Location = New System.Drawing.Point(248, 8)
        Me.ButtonF2.Name = "ButtonF2"
        Me.ButtonF2.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF2.TabIndex = 49
        Me.ButtonF2.TabStop = False
        Me.ButtonF2.Visible = False
        '
        'ButtonF4
        '
        Me.ButtonF4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonF4.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF4.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF4.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF4.ImageIndex = 3
        Me.ButtonF4.ImageList = Me.ImageList1_32
        Me.ButtonF4.Location = New System.Drawing.Point(288, 8)
        Me.ButtonF4.Name = "ButtonF4"
        Me.ButtonF4.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF4.TabIndex = 50
        Me.ButtonF4.TabStop = False
        Me.ButtonF4.Visible = False
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(336, 16)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.TabIndex = 51
        Me.TextBox3.TabStop = False
        Me.TextBox3.Text = ""
        Me.TextBox3.Visible = False
        '
        'frmRicSer
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(616, 542)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.ButtonF4)
        Me.Controls.Add(Me.ButtonF2)
        Me.Controls.Add(Me.ButtonF1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Datagrid1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "frmRicSer"
        Me.Text = "frmRicSer"
        Me.Controls.SetChildIndex(Me.TextBox1, 0)
        Me.Controls.SetChildIndex(Me.TextBox2, 0)
        Me.Controls.SetChildIndex(Me.Datagrid1, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.Controls.SetChildIndex(Me.ButtonF1, 0)
        Me.Controls.SetChildIndex(Me.ButtonF2, 0)
        Me.Controls.SetChildIndex(Me.ButtonF4, 0)
        Me.Controls.SetChildIndex(Me.TextBox3, 0)
        CType(Me.Datagrid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Dim DsPia As dataset
    Dim DaPia As SqlDataAdapter

    Private Sub frmRicSer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DGRID = Datagrid1
        Dim Larghezza As Int16 = 0
        Dim x As DataGridTableStyle
        Dim y As Object
        For Each x In DGRID.TableStyles
            For Each y In x.GridColumnStyles
                If x.MappingName = "TbSer" Then Larghezza += y.Width
            Next
        Next
        Dim Largh2 As Int16
        If Me.FormBorderStyle = FormBorderStyle.FixedToolWindow Then Largh2 = (SystemInformation.FixedFrameBorderSize.Width() * 2)
        If Me.FormBorderStyle = FormBorderStyle.Sizable Then Largh2 = (SystemInformation.FrameBorderSize.Width() * 2)
        Me.Width = Larghezza + SystemInformation.VerticalScrollBarWidth() + Largh2 '6 '25

        DisabTextBox(DGRID)
        puliscicampi(Me)
        Privcodice = ""
        DGRID = Datagrid1
        DGRID.DataSource = ds.Tables("TbSer")
        DGRID.Select(0)
        Hcl = 0
        RighePagina = DGRID.VisibleRowCount - 1 '''' INTESTAZIONE DI COLONNA
        UnoPiu = RighePagina
        USelect = 0
        MaxPro = ds.Tables("TbSer").Rows.Count
        TextBox3.Text = DGRID.Item(0, 0)
        DGRID.Refresh()
    End Sub

    Private Sub Grid_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Datagrid1.MouseUp
        Dim myGrid As DataGrid = CType(sender, DataGrid)
        Dim myHitInfo As DataGrid.HitTestInfo = myGrid.HitTest(e.X, e.Y)
        iset = -1
        If myHitInfo.Row >= 0 Then
            myGrid.Select(myHitInfo.Row)
            iset = myHitInfo.Row
            Privcodice = Datagrid1.Item(iset, 0)
            Me.Close()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged
        PopolaGrid(TextBox1.Text, TextBox2.Text)
    End Sub

    Private Sub PopolaGrid(ByVal Cod As String, ByVal Ana As String)
        Dim leggi As String
        Dim i As Int16
        leggi = "select * from TbSer where SerCod like @Num and SerDesc like @Ana"

        Dim p1 As New SqlParameter("@Num", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@Ana", SqlDbType.VarChar)

        p1.Value = Cod & "%"
        p2.Value = Ana & "%"

        Dim cmd As New SqlCommand(leggi, cnDb)
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)

        DsPia = New DataSet
        DaPia = New SqlDataAdapter(cmd)
        DaPia.Fill(DsPia, "TbSer")

        Datagrid1.DataSource = DsPia.Tables("TbSer")
        Datagrid1.Refresh()

        MaxPro = DsPia.Tables("TbSer").Rows.Count
        If MaxPro > 0 Then
            Datagrid1.Select(0)
            Hcl = 0
            RighePagina = Datagrid1.VisibleRowCount - 1 '''' INTESTAZIONE DI COLONNA
            UnoPiu = RighePagina
            USelect = 0
            TextBox3.Text = Datagrid1.Item(0, 0)
        End If
    End Sub

    'Private Sub ButtonF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF4.Click
    '    Privcodice = TextBox3.Text
    '    Me.Close()
    'End Sub

    'Private Sub Button75_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF2.Click
    '    Pcl = Hcl - UnoPiu
    '    If Pcl > UnoPiu * -1 Then
    '        Hcl = Pcl
    '    Else
    '        Hcl = 0
    '    End If
    '    IniziaRoutine(Hcl)
    'End Sub

    'Private Sub Button81_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
    '    Pcl = Hcl + UnoPiu
    '    If Pcl < MaxPro Then
    '        Hcl = Pcl
    '    Else
    '        Hcl = MaxPro - 1
    '    End If
    '    IniziaRoutine(Hcl)
    'End Sub

    Function IniziaRoutine(ByVal Hcl As Int32)   ''' occhio al name DataGrid
        If MaxPro < 1 Then Exit Function
        If Hcl < 0 Then Exit Function
        If USelect > -1 Then DGRID.UnSelect(USelect)
        If Hcl < 0 Then Hcl = 0
        If Hcl >= MaxPro Then Hcl = MaxPro - 1
        DGRID.Select(Hcl)
        USelect = Hcl
        If UnoPiu = RighePagina Then DGRID.CurrentRowIndex = MaxPro - 1
        DGRID.CurrentRowIndex = Hcl
        TextBox3.Text = DGRID.Item(Hcl, 0)
        'TextBox2.Text = DGRID.Item(Hcl, 1)
        TextBox1.Focus()
        'TextBox7.Text = Hcl + 1
        'If MDesc = True Then VisualizzaDati()
    End Function

    Private Sub frmRicClf_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.Down Or e.KeyData = Keys.Right Then
            e.Handled = True
            UnoPiu = 1
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.Up Or e.KeyData = Keys.Left Then
            e.Handled = True
            UnoPiu = 1
            ButtonF2.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F4 Then
            e.Handled = True
            ButtonF4.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F1 Or e.KeyData = Keys.PageDown Then
            e.Handled = True
            UnoPiu = RighePagina
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F2 Or e.KeyData = Keys.PageUp Then
            e.Handled = True
            UnoPiu = RighePagina
            ButtonF2.PerformClick()
            Exit Sub
        End If
    End Sub
End Class
