Imports System.Data.sqlclient
Imports DXBASE
Public Class RicDoc
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
    Friend WithEvents ButtonF4 As System.windows.forms.button
    Friend WithEvents ButtonF2 As System.windows.forms.button
    Friend WithEvents ButtonF1 As System.windows.forms.button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn1 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn2 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn3 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn4 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn5 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn6 As System.Windows.Forms.DataGridTextBoxColumn
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RicDoc))
        Me.ButtonF4 = New System.windows.forms.button
        Me.ButtonF2 = New System.windows.forms.button
        Me.ButtonF1 = New System.windows.forms.button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.DataGrid1 = New System.Windows.Forms.DataGrid
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle
        Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn2 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn3 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn4 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn5 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn6 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonF4
        '
        Me.ButtonF4.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF4.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF4.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF4.ImageIndex = 3
        Me.ButtonF4.ImageList = Me.ImageList1_32
        Me.ButtonF4.Location = New System.Drawing.Point(350, 480)
        Me.ButtonF4.Name = "ButtonF4"
        Me.ButtonF4.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF4.TabIndex = 50
        Me.ButtonF4.TabStop = False
        '
        'ButtonF2
        '
        Me.ButtonF2.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF2.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF2.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF2.ImageIndex = 1
        Me.ButtonF2.ImageList = Me.ImageList1_32
        Me.ButtonF2.Location = New System.Drawing.Point(262, 480)
        Me.ButtonF2.Name = "ButtonF2"
        Me.ButtonF2.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF2.TabIndex = 49
        Me.ButtonF2.TabStop = False
        '
        'ButtonF1
        '
        Me.ButtonF1.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF1.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF1.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF1.ImageIndex = 0
        Me.ButtonF1.ImageList = Me.ImageList1_32
        Me.ButtonF1.Location = New System.Drawing.Point(214, 480)
        Me.ButtonF1.Name = "ButtonF1"
        Me.ButtonF1.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF1.TabIndex = 48
        Me.ButtonF1.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.DataGrid1)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Location = New System.Drawing.Point(0, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(599, 464)
        Me.GroupBox1.TabIndex = 47
        Me.GroupBox1.TabStop = False
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
        Me.DataGrid1.Dock = System.Windows.Forms.DockStyle.Fill
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
        Me.DataGrid1.Size = New System.Drawing.Size(593, 443)
        Me.DataGrid1.TabIndex = 0
        Me.DataGrid1.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(32, 376)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(96, 22)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.Text = "TextBox1"
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.DataGrid1
        Me.DataGridTableStyle1.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn1, Me.DataGridTextBoxColumn2, Me.DataGridTextBoxColumn3, Me.DataGridTextBoxColumn4, Me.DataGridTextBoxColumn5, Me.DataGridTextBoxColumn6})
        Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridTableStyle1.MappingName = "DOCUM"
        Me.DataGridTableStyle1.ReadOnly = True
        Me.DataGridTableStyle1.RowHeadersVisible = False
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.MappingName = "NumRif"
        Me.DataGridTextBoxColumn1.ReadOnly = True
        Me.DataGridTextBoxColumn1.Width = 0
        '
        'DataGridTextBoxColumn2
        '
        Me.DataGridTextBoxColumn2.Format = ""
        Me.DataGridTextBoxColumn2.FormatInfo = Nothing
        Me.DataGridTextBoxColumn2.HeaderText = "Tipo"
        Me.DataGridTextBoxColumn2.MappingName = "DesTipo"
        Me.DataGridTextBoxColumn2.ReadOnly = True
        Me.DataGridTextBoxColumn2.Width = 90
        '
        'DataGridTextBoxColumn3
        '
        Me.DataGridTextBoxColumn3.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.DataGridTextBoxColumn3.Format = ""
        Me.DataGridTextBoxColumn3.FormatInfo = Nothing
        Me.DataGridTextBoxColumn3.HeaderText = "Numero"
        Me.DataGridTextBoxColumn3.MappingName = "Numero"
        Me.DataGridTextBoxColumn3.ReadOnly = True
        Me.DataGridTextBoxColumn3.Width = 75
        '
        'DataGridTextBoxColumn4
        '
        Me.DataGridTextBoxColumn4.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.DataGridTextBoxColumn4.Format = ""
        Me.DataGridTextBoxColumn4.FormatInfo = Nothing
        Me.DataGridTextBoxColumn4.HeaderText = "Data"
        Me.DataGridTextBoxColumn4.MappingName = "Data"
        Me.DataGridTextBoxColumn4.ReadOnly = True
        Me.DataGridTextBoxColumn4.Width = 90
        '
        'DataGridTextBoxColumn5
        '
        Me.DataGridTextBoxColumn5.Format = ""
        Me.DataGridTextBoxColumn5.FormatInfo = Nothing
        Me.DataGridTextBoxColumn5.HeaderText = "Clie"
        Me.DataGridTextBoxColumn5.MappingName = "Cliente"
        Me.DataGridTextBoxColumn5.ReadOnly = True
        Me.DataGridTextBoxColumn5.Width = 55
        '
        'DataGridTextBoxColumn6
        '
        Me.DataGridTextBoxColumn6.Format = ""
        Me.DataGridTextBoxColumn6.FormatInfo = Nothing
        Me.DataGridTextBoxColumn6.HeaderText = "Ragione sociale"
        Me.DataGridTextBoxColumn6.MappingName = "RagSoc"
        Me.DataGridTextBoxColumn6.ReadOnly = True
        Me.DataGridTextBoxColumn6.Width = 260
        '
        'RicDoc
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(600, 542)
        Me.Controls.Add(Me.ButtonF4)
        Me.Controls.Add(Me.ButtonF2)
        Me.Controls.Add(Me.ButtonF1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "RicDoc"
        Me.Text = "RicDoc"
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.Controls.SetChildIndex(Me.ButtonF1, 0)
        Me.Controls.SetChildIndex(Me.ButtonF2, 0)
        Me.Controls.SetChildIndex(Me.ButtonF4, 0)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public WriteOnly Property Dati() As DataTable
        Set(ByVal Value As DataTable)
            Tb = Value
        End Set
    End Property
    Public ReadOnly Property Riferimento()
        Get
            Return Numrif
        End Get
    End Property

    Dim Numrif As Int32
    Dim Tb As DataTable
    Dim iset As Integer
    Dim Pcl, Hcl, MaxPro, RighePagina, UnoPiu, USelect As Int32
    Private Sub RicDoc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Disabilito le TextBox delle celle per ogni TableStyle
        Dim x As DataGridTableStyle
        Dim y As Object
        For Each x In DataGrid1.TableStyles
            For Each y In x.GridColumnStyles
                If TypeOf y Is DataGridTextBoxColumn Then
                    CType(y, DataGridTextBoxColumn).TextBox.Enabled = False
                End If
            Next
        Next

        Numrif = 0
        DataGrid1.DataSource = Tb

        DataGrid1.Select(0)
        Hcl = 0
        RighePagina = DataGrid1.VisibleRowCount - 1 '''' INTESTAZIONE DI COLONNA
        UnoPiu = RighePagina
        USelect = 0
        MaxPro = Tb.Rows.Count
        TextBox1.Text = DataGrid1.Item(0, 0)
        DataGrid1.Refresh()
    End Sub

    Private Sub Grid_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid1.MouseUp
        Dim myGrid As DataGrid = CType(sender, DataGrid)
        Dim myHitInfo As DataGrid.HitTestInfo = myGrid.HitTest(e.X, e.Y)
        iset = -1
        If myHitInfo.Row >= 0 Then
            myGrid.Select(myHitInfo.Row)
            iset = myHitInfo.Row
            Numrif = DataGrid1.Item(iset, 0)
            Me.Close()
        End If
    End Sub

    Private Sub ButtonF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF4.Click
        Numrif = TextBox1.Text
        Me.Close()
    End Sub

    Private Sub Button75_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF2.Click
        Pcl = Hcl - UnoPiu
        If Pcl > UnoPiu * -1 Then
            Hcl = Pcl
        Else
            Hcl = 0
        End If
        IniziaRoutine(Hcl)
    End Sub

    Private Sub Button81_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        Pcl = Hcl + UnoPiu
        If Pcl < MaxPro Then
            Hcl = Pcl
        Else
            Hcl = MaxPro - 1
        End If
        IniziaRoutine(Hcl)
    End Sub

    Function IniziaRoutine(ByVal Hcl As Int32)   ''' occhio al name DataGrid
        If Hcl < 0 Then Exit Function
        If USelect > -1 Then DataGrid1.UnSelect(USelect)
        If Hcl < 0 Then Hcl = 0
        If Hcl >= MaxPro Then Hcl = MaxPro - 1
        DataGrid1.Select(Hcl)
        USelect = Hcl
        If UnoPiu = RighePagina Then DataGrid1.CurrentRowIndex = MaxPro - 1
        DataGrid1.CurrentRowIndex = Hcl
        TextBox1.Text = DataGrid1.Item(Hcl, 0)
        TextBox1.Focus()
    End Function

    Private Sub RicercheFrm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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
