Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports DXBASE.Util
Imports System.Data.sqlclient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.ReportSource
Imports System.IO

Public Class FatNcr
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
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox40 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Numbox2 As DXBASE.numbox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBox12 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox5 As DXBASE.numbox
    Friend WithEvents Numbox4 As DXBASE.numbox
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox3 As DXBASE.numbox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonF8 As System.windows.forms.button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox1 As DXBASE.numbox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonF1 As System.windows.forms.button
    Friend WithEvents ButtonF5 As System.windows.forms.button
    Friend WithEvents ButtonF3 As System.windows.forms.button
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonF9 As System.windows.forms.button
    Friend WithEvents ButtonF2 As System.windows.forms.button
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonXX As System.windows.forms.button
    Friend WithEvents TextBox14 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonDown As System.windows.forms.button
    Friend WithEvents ButtonUp As System.windows.forms.button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox16 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Numbox6 As DXBASE.numbox
    Friend WithEvents Numbox11 As DXBASE.numbox
    Friend WithEvents Numbox12 As DXBASE.numbox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents ButtonF8A As System.windows.forms.button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboBox4 As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonF3A As System.windows.forms.button
    Friend WithEvents ButtonF5A As System.windows.forms.button
    Friend WithEvents ButtonF11 As System.windows.forms.button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn1 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn2 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn3 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn5 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn6 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn7 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn8 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn9 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn10 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn11 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn12 As System.Windows.Forms.DataGridTextBoxColumn
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FatNcr))
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.TextBox13 = New System.Windows.Forms.TextBox
        Me.TextBox10 = New System.Windows.Forms.TextBox
        Me.TextBox40 = New System.Windows.Forms.TextBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.DataGrid1 = New System.Windows.Forms.DataGrid
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle
        Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn2 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn12 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn3 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn5 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn6 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn7 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn8 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn9 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn10 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumn11 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.Numbox2 = New DXBASE.Numbox
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.TextBox12 = New System.Windows.Forms.TextBox
        Me.Numbox5 = New DXBASE.Numbox
        Me.Numbox4 = New DXBASE.Numbox
        Me.TextBox11 = New System.Windows.Forms.TextBox
        Me.Numbox3 = New DXBASE.Numbox
        Me.Label15 = New System.Windows.Forms.Label
        Me.ComboBox3 = New System.Windows.Forms.ComboBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.ButtonF8 = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextBox9 = New System.Windows.Forms.TextBox
        Me.TextBox8 = New System.Windows.Forms.TextBox
        Me.TextBox7 = New System.Windows.Forms.TextBox
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Numbox1 = New DXBASE.Numbox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.ButtonF1 = New System.Windows.Forms.Button
        Me.ButtonF5 = New System.Windows.Forms.Button
        Me.ButtonF3 = New System.Windows.Forms.Button
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.ButtonF9 = New System.Windows.Forms.Button
        Me.ButtonF2 = New System.Windows.Forms.Button
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.ButtonXX = New System.Windows.Forms.Button
        Me.TextBox14 = New System.Windows.Forms.TextBox
        Me.ButtonDown = New System.Windows.Forms.Button
        Me.ButtonUp = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.Numbox6 = New DXBASE.Numbox
        Me.Numbox11 = New DXBASE.Numbox
        Me.Numbox12 = New DXBASE.Numbox
        Me.Label19 = New System.Windows.Forms.Label
        Me.ButtonF8A = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboBox4 = New System.Windows.Forms.ComboBox
        Me.ButtonF3A = New System.Windows.Forms.Button
        Me.ButtonF5A = New System.Windows.Forms.Button
        Me.ButtonF11 = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox16 = New System.Windows.Forms.TextBox
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1_32
        '
        Me.ImageList1_32.ImageStream = CType(resources.GetObject("ImageList1_32.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1_32.Images.SetKeyName(0, "F1x.ICO")
        Me.ImageList1_32.Images.SetKeyName(1, "F2x.ICO")
        Me.ImageList1_32.Images.SetKeyName(2, "F3x.ICO")
        Me.ImageList1_32.Images.SetKeyName(3, "F4x.ICO")
        Me.ImageList1_32.Images.SetKeyName(4, "F5x.ICO")
        Me.ImageList1_32.Images.SetKeyName(5, "F6x.ICO")
        Me.ImageList1_32.Images.SetKeyName(6, "F7x.ICO")
        Me.ImageList1_32.Images.SetKeyName(7, "F8x.ICO")
        Me.ImageList1_32.Images.SetKeyName(8, "F9x.ICO")
        Me.ImageList1_32.Images.SetKeyName(9, "F10x.ICO")
        Me.ImageList1_32.Images.SetKeyName(10, "F11x.ICO")
        Me.ImageList1_32.Images.SetKeyName(11, "F12x.ICO")
        Me.ImageList1_32.Images.SetKeyName(12, "inserisci.ico")
        Me.ImageList1_32.Images.SetKeyName(13, "Fanteprimax.ICO")
        Me.ImageList1_32.Images.SetKeyName(14, "Controllox.ICO")
        Me.ImageList1_32.Images.SetKeyName(15, "PgDownx.ICO")
        Me.ImageList1_32.Images.SetKeyName(16, "PgUpx.ICO")
        Me.ImageList1_32.Images.SetKeyName(17, "Accedix.ico")
        Me.ImageList1_32.Images.SetKeyName(18, "aggiorna.ico")
        Me.ImageList1_32.Images.SetKeyName(19, "elimina.ico")
        Me.ImageList1_32.Images.SetKeyName(20, "destra.ico")
        Me.ImageList1_32.Images.SetKeyName(21, "Asinistra.ico")
        Me.ImageList1_32.Images.SetKeyName(22, "stampa.ico")
        Me.ImageList1_32.Images.SetKeyName(23, "spostagiu.ico")
        Me.ImageList1_32.Images.SetKeyName(24, "modifica.ico")
        Me.ImageList1_32.Images.SetKeyName(25, "voip.ico")
        Me.ImageList1_32.Images.SetKeyName(26, "voip2.ico")
        Me.ImageList1_32.Images.SetKeyName(27, "Sposta.ico")
        Me.ImageList1_32.Images.SetKeyName(28, "cambia.ico")
        Me.ImageList1_32.Images.SetKeyName(29, "reset.ico")
        Me.ImageList1_32.Images.SetKeyName(30, "spostasu.ico")
        '
        'ImageListXP_16
        '
        Me.ImageListXP_16.ImageStream = CType(resources.GetObject("ImageListXP_16.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListXP_16.Images.SetKeyName(0, "Documents.ico")
        Me.ImageListXP_16.Images.SetKeyName(1, "3.ico")
        Me.ImageListXP_16.Images.SetKeyName(2, "4.ico")
        Me.ImageListXP_16.Images.SetKeyName(3, "29.ico")
        Me.ImageListXP_16.Images.SetKeyName(4, "67.ico")
        Me.ImageListXP_16.Images.SetKeyName(5, "73.ico")
        Me.ImageListXP_16.Images.SetKeyName(6, "49.ico")
        Me.ImageListXP_16.Images.SetKeyName(7, "51.ico")
        Me.ImageListXP_16.Images.SetKeyName(8, "46.ico")
        Me.ImageListXP_16.Images.SetKeyName(9, "Run.ico")
        Me.ImageListXP_16.Images.SetKeyName(10, "Default Document.ico")
        Me.ImageListXP_16.Images.SetKeyName(11, "58.ico")
        Me.ImageListXP_16.Images.SetKeyName(12, "Workgroup.ico")
        Me.ImageListXP_16.Images.SetKeyName(13, "1.ico")
        Me.ImageListXP_16.Images.SetKeyName(14, "2.ico")
        Me.ImageListXP_16.Images.SetKeyName(15, "6.ico")
        Me.ImageListXP_16.Images.SetKeyName(16, "30.ico")
        Me.ImageListXP_16.Images.SetKeyName(17, "41.ico")
        Me.ImageListXP_16.Images.SetKeyName(18, "43.ico")
        Me.ImageListXP_16.Images.SetKeyName(19, "48.ico")
        Me.ImageListXP_16.Images.SetKeyName(20, "59.ico")
        Me.ImageListXP_16.Images.SetKeyName(21, "62.ico")
        Me.ImageListXP_16.Images.SetKeyName(22, "64.ico")
        Me.ImageListXP_16.Images.SetKeyName(23, "66.ico")
        Me.ImageListXP_16.Images.SetKeyName(24, "69.ico")
        Me.ImageListXP_16.Images.SetKeyName(25, "85.ico")
        Me.ImageListXP_16.Images.SetKeyName(26, "86.ico")
        Me.ImageListXP_16.Images.SetKeyName(27, "88.ico")
        Me.ImageListXP_16.Images.SetKeyName(28, "91.ico")
        Me.ImageListXP_16.Images.SetKeyName(29, "Administrative Tools.ico")
        Me.ImageListXP_16.Images.SetKeyName(30, "CD-ROM Drive.ico")
        Me.ImageListXP_16.Images.SetKeyName(31, "Closed Folder.ico")
        Me.ImageListXP_16.Images.SetKeyName(32, "Control Panel.ico")
        Me.ImageListXP_16.Images.SetKeyName(33, "Find.ico")
        Me.ImageListXP_16.Images.SetKeyName(34, "Hard Drive.ico")
        Me.ImageListXP_16.Images.SetKeyName(35, "Help.ico")
        Me.ImageListXP_16.Images.SetKeyName(36, "In Box.ico")
        Me.ImageListXP_16.Images.SetKeyName(37, "My Computer.ico")
        Me.ImageListXP_16.Images.SetKeyName(38, "Open Folder.ico")
        Me.ImageListXP_16.Images.SetKeyName(39, "Printers.ico")
        Me.ImageListXP_16.Images.SetKeyName(40, "Recycle Bin (empty).ico")
        Me.ImageListXP_16.Images.SetKeyName(41, "Recycle Bin (full).ico")
        Me.ImageListXP_16.Images.SetKeyName(42, "Subscriptions.ico")
        Me.ImageListXP_16.Images.SetKeyName(43, "Text Document.ico")
        Me.ImageListXP_16.Images.SetKeyName(44, "The Internet.ico")
        Me.ImageListXP_16.Images.SetKeyName(45, "Url History.ico")
        Me.ImageListXP_16.Images.SetKeyName(46, "Stampa.ico")
        Me.ImageListXP_16.Images.SetKeyName(47, "Pulisci.ico")
        Me.ImageListXP_16.Images.SetKeyName(48, "none.ico")
        Me.ImageListXP_16.Images.SetKeyName(49, "search4printer.ico")
        Me.ImageListXP_16.Images.SetKeyName(50, "search4files.ico")
        Me.ImageListXP_16.Images.SetKeyName(51, "search4people.ico")
        Me.ImageListXP_16.Images.SetKeyName(52, "users.ico")
        Me.ImageListXP_16.Images.SetKeyName(53, "search.ico")
        Me.ImageListXP_16.Images.SetKeyName(54, "propertiesORoptions.ico")
        Me.ImageListXP_16.Images.SetKeyName(55, "user.ico")
        Me.ImageListXP_16.Images.SetKeyName(56, "search4doc.ico")
        Me.ImageListXP_16.Images.SetKeyName(57, "searchweb.ico")
        Me.ImageListXP_16.Images.SetKeyName(58, "disconnect3.ico")
        Me.ImageListXP_16.Images.SetKeyName(59, "repair.ico")
        Me.ImageListXP_16.Images.SetKeyName(60, "delete_16x.ico")
        Me.ImageListXP_16.Images.SetKeyName(61, "disconnect2.ico")
        Me.ImageListXP_16.Images.SetKeyName(62, "fax.ico")
        Me.ImageListXP_16.Images.SetKeyName(63, "mobsync.20.ico")
        '
        'ImageListXP_32
        '
        Me.ImageListXP_32.ImageStream = CType(resources.GetObject("ImageListXP_32.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListXP_32.Images.SetKeyName(0, "Documents.ico")
        Me.ImageListXP_32.Images.SetKeyName(1, "3.ico")
        Me.ImageListXP_32.Images.SetKeyName(2, "4.ico")
        Me.ImageListXP_32.Images.SetKeyName(3, "29.ico")
        Me.ImageListXP_32.Images.SetKeyName(4, "67.ico")
        Me.ImageListXP_32.Images.SetKeyName(5, "73.ico")
        Me.ImageListXP_32.Images.SetKeyName(6, "49.ico")
        Me.ImageListXP_32.Images.SetKeyName(7, "51.ico")
        Me.ImageListXP_32.Images.SetKeyName(8, "46.ico")
        Me.ImageListXP_32.Images.SetKeyName(9, "Run.ico")
        Me.ImageListXP_32.Images.SetKeyName(10, "Default Document.ico")
        Me.ImageListXP_32.Images.SetKeyName(11, "58.ico")
        Me.ImageListXP_32.Images.SetKeyName(12, "Workgroup.ico")
        Me.ImageListXP_32.Images.SetKeyName(13, "1.ico")
        Me.ImageListXP_32.Images.SetKeyName(14, "2.ico")
        Me.ImageListXP_32.Images.SetKeyName(15, "6.ico")
        Me.ImageListXP_32.Images.SetKeyName(16, "30.ico")
        Me.ImageListXP_32.Images.SetKeyName(17, "41.ico")
        Me.ImageListXP_32.Images.SetKeyName(18, "43.ico")
        Me.ImageListXP_32.Images.SetKeyName(19, "48.ico")
        Me.ImageListXP_32.Images.SetKeyName(20, "59.ico")
        Me.ImageListXP_32.Images.SetKeyName(21, "62.ico")
        Me.ImageListXP_32.Images.SetKeyName(22, "64.ico")
        Me.ImageListXP_32.Images.SetKeyName(23, "66.ico")
        Me.ImageListXP_32.Images.SetKeyName(24, "69.ico")
        Me.ImageListXP_32.Images.SetKeyName(25, "85.ico")
        Me.ImageListXP_32.Images.SetKeyName(26, "86.ico")
        Me.ImageListXP_32.Images.SetKeyName(27, "88.ico")
        Me.ImageListXP_32.Images.SetKeyName(28, "91.ico")
        Me.ImageListXP_32.Images.SetKeyName(29, "Administrative Tools.ico")
        Me.ImageListXP_32.Images.SetKeyName(30, "CD-ROM Drive.ico")
        Me.ImageListXP_32.Images.SetKeyName(31, "Closed Folder.ico")
        Me.ImageListXP_32.Images.SetKeyName(32, "Control Panel.ico")
        Me.ImageListXP_32.Images.SetKeyName(33, "Find.ico")
        Me.ImageListXP_32.Images.SetKeyName(34, "Hard Drive.ico")
        Me.ImageListXP_32.Images.SetKeyName(35, "Help.ico")
        Me.ImageListXP_32.Images.SetKeyName(36, "In Box.ico")
        Me.ImageListXP_32.Images.SetKeyName(37, "My Computer.ico")
        Me.ImageListXP_32.Images.SetKeyName(38, "Open Folder.ico")
        Me.ImageListXP_32.Images.SetKeyName(39, "Printers.ico")
        Me.ImageListXP_32.Images.SetKeyName(40, "Recycle Bin (empty).ico")
        Me.ImageListXP_32.Images.SetKeyName(41, "Recycle Bin (full).ico")
        Me.ImageListXP_32.Images.SetKeyName(42, "Subscriptions.ico")
        Me.ImageListXP_32.Images.SetKeyName(43, "Text Document.ico")
        Me.ImageListXP_32.Images.SetKeyName(44, "The Internet.ico")
        Me.ImageListXP_32.Images.SetKeyName(45, "Url History.ico")
        Me.ImageListXP_32.Images.SetKeyName(46, "Stampa.ico")
        Me.ImageListXP_32.Images.SetKeyName(47, "Pulisci.ico")
        Me.ImageListXP_32.Images.SetKeyName(48, "none.ico")
        Me.ImageListXP_32.Images.SetKeyName(49, "search4printer.ico")
        Me.ImageListXP_32.Images.SetKeyName(50, "search4files.ico")
        Me.ImageListXP_32.Images.SetKeyName(51, "search4people.ico")
        Me.ImageListXP_32.Images.SetKeyName(52, "users.ico")
        Me.ImageListXP_32.Images.SetKeyName(53, "search.ico")
        Me.ImageListXP_32.Images.SetKeyName(54, "propertiesORoptions.ico")
        Me.ImageListXP_32.Images.SetKeyName(55, "user.ico")
        Me.ImageListXP_32.Images.SetKeyName(56, "search4doc.ico")
        Me.ImageListXP_32.Images.SetKeyName(57, "searchweb.ico")
        Me.ImageListXP_32.Images.SetKeyName(58, "disconnect3.ico")
        Me.ImageListXP_32.Images.SetKeyName(59, "repair.ico")
        Me.ImageListXP_32.Images.SetKeyName(60, "delete_16x.ico")
        Me.ImageListXP_32.Images.SetKeyName(61, "disconnect2.ico")
        Me.ImageListXP_32.Images.SetKeyName(62, "fax.ico")
        Me.ImageListXP_32.Images.SetKeyName(63, "mobsync.20.ico")
        '
        'GroupBox9
        '
        Me.GroupBox9.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox9.Controls.Add(Me.Label18)
        Me.GroupBox9.Controls.Add(Me.Label17)
        Me.GroupBox9.Controls.Add(Me.Label16)
        Me.GroupBox9.Controls.Add(Me.TextBox13)
        Me.GroupBox9.Controls.Add(Me.TextBox10)
        Me.GroupBox9.Controls.Add(Me.TextBox40)
        Me.GroupBox9.Location = New System.Drawing.Point(18, 584)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(400, 64)
        Me.GroupBox9.TabIndex = 222
        Me.GroupBox9.TabStop = False
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(256, 16)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(128, 16)
        Me.Label18.TabIndex = 39
        Me.Label18.Text = "TOTALE DOCUMENTO"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(128, 16)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(98, 16)
        Me.Label17.TabIndex = 38
        Me.Label17.Text = "TOTALE IVA"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(8, 16)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(128, 16)
        Me.Label16.TabIndex = 37
        Me.Label16.Text = "TOT. IMPONIBILE"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox13
        '
        Me.TextBox13.Location = New System.Drawing.Point(128, 32)
        Me.TextBox13.MaxLength = 11
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.ReadOnly = True
        Me.TextBox13.Size = New System.Drawing.Size(112, 22)
        Me.TextBox13.TabIndex = 36
        Me.TextBox13.TabStop = False
        Me.TextBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox10
        '
        Me.TextBox10.Location = New System.Drawing.Point(8, 32)
        Me.TextBox10.MaxLength = 11
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.ReadOnly = True
        Me.TextBox10.Size = New System.Drawing.Size(112, 22)
        Me.TextBox10.TabIndex = 35
        Me.TextBox10.TabStop = False
        Me.TextBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox40
        '
        Me.TextBox40.Location = New System.Drawing.Point(256, 32)
        Me.TextBox40.MaxLength = 11
        Me.TextBox40.Name = "TextBox40"
        Me.TextBox40.ReadOnly = True
        Me.TextBox40.Size = New System.Drawing.Size(128, 22)
        Me.TextBox40.TabIndex = 32
        Me.TextBox40.TabStop = False
        Me.TextBox40.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox6.Controls.Add(Me.DataGrid1)
        Me.GroupBox6.Location = New System.Drawing.Point(11, 192)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(919, 352)
        Me.GroupBox6.TabIndex = 220
        Me.GroupBox6.TabStop = False
        '
        'DataGrid1
        '
        Me.DataGrid1.AllowSorting = False
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
        Me.DataGrid1.ParentRowsVisible = False
        Me.DataGrid1.ReadOnly = True
        Me.DataGrid1.RowHeadersVisible = False
        Me.DataGrid1.SelectionBackColor = System.Drawing.Color.Teal
        Me.DataGrid1.SelectionForeColor = System.Drawing.Color.PaleGreen
        Me.DataGrid1.Size = New System.Drawing.Size(913, 331)
        Me.DataGrid1.TabIndex = 0
        Me.DataGrid1.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
        Me.DataGrid1.TabStop = False
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.DataGrid1
        Me.DataGridTableStyle1.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn1, Me.DataGridTextBoxColumn2, Me.DataGridTextBoxColumn12, Me.DataGridTextBoxColumn3, Me.DataGridTextBoxColumn5, Me.DataGridTextBoxColumn6, Me.DataGridTextBoxColumn7, Me.DataGridTextBoxColumn8, Me.DataGridTextBoxColumn9, Me.DataGridTextBoxColumn10, Me.DataGridTextBoxColumn11})
        Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridTableStyle1.MappingName = "RIGHE"
        Me.DataGridTableStyle1.ReadOnly = True
        Me.DataGridTableStyle1.RowHeadersVisible = False
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.MappingName = "CorRif"
        Me.DataGridTextBoxColumn1.ReadOnly = True
        Me.DataGridTextBoxColumn1.Width = 0
        '
        'DataGridTextBoxColumn2
        '
        Me.DataGridTextBoxColumn2.Format = ""
        Me.DataGridTextBoxColumn2.FormatInfo = Nothing
        Me.DataGridTextBoxColumn2.MappingName = "CorProg"
        Me.DataGridTextBoxColumn2.ReadOnly = True
        Me.DataGridTextBoxColumn2.Width = 0
        '
        'DataGridTextBoxColumn12
        '
        Me.DataGridTextBoxColumn12.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.DataGridTextBoxColumn12.Format = ""
        Me.DataGridTextBoxColumn12.FormatInfo = Nothing
        Me.DataGridTextBoxColumn12.HeaderText = "COD."
        Me.DataGridTextBoxColumn12.MappingName = "CorCodArt"
        Me.DataGridTextBoxColumn12.ReadOnly = True
        Me.DataGridTextBoxColumn12.Width = 40
        '
        'DataGridTextBoxColumn3
        '
        Me.DataGridTextBoxColumn3.Format = ""
        Me.DataGridTextBoxColumn3.FormatInfo = Nothing
        Me.DataGridTextBoxColumn3.HeaderText = "DESCRIZIONE"
        Me.DataGridTextBoxColumn3.MappingName = "CorDesc"
        Me.DataGridTextBoxColumn3.ReadOnly = True
        Me.DataGridTextBoxColumn3.Width = 400
        '
        'DataGridTextBoxColumn5
        '
        Me.DataGridTextBoxColumn5.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.DataGridTextBoxColumn5.Format = "###,##0.00"
        Me.DataGridTextBoxColumn5.FormatInfo = Nothing
        Me.DataGridTextBoxColumn5.HeaderText = "QTA"
        Me.DataGridTextBoxColumn5.MappingName = "CorQuaCon"
        Me.DataGridTextBoxColumn5.ReadOnly = True
        Me.DataGridTextBoxColumn5.Width = 72
        '
        'DataGridTextBoxColumn6
        '
        Me.DataGridTextBoxColumn6.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.DataGridTextBoxColumn6.Format = "###,##0.00"
        Me.DataGridTextBoxColumn6.FormatInfo = Nothing
        Me.DataGridTextBoxColumn6.HeaderText = "PREZZO"
        Me.DataGridTextBoxColumn6.MappingName = "CorPrezzo"
        Me.DataGridTextBoxColumn6.ReadOnly = True
        Me.DataGridTextBoxColumn6.Width = 96
        '
        'DataGridTextBoxColumn7
        '
        Me.DataGridTextBoxColumn7.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.DataGridTextBoxColumn7.Format = "###,##0.00"
        Me.DataGridTextBoxColumn7.FormatInfo = Nothing
        Me.DataGridTextBoxColumn7.HeaderText = "IMPORTO"
        Me.DataGridTextBoxColumn7.MappingName = "CorImporto"
        Me.DataGridTextBoxColumn7.ReadOnly = True
        Me.DataGridTextBoxColumn7.Width = 96
        '
        'DataGridTextBoxColumn8
        '
        Me.DataGridTextBoxColumn8.Format = ""
        Me.DataGridTextBoxColumn8.FormatInfo = Nothing
        Me.DataGridTextBoxColumn8.HeaderText = "Codice Iva"
        Me.DataGridTextBoxColumn8.MappingName = "CorDesCiva"
        Me.DataGridTextBoxColumn8.ReadOnly = True
        Me.DataGridTextBoxColumn8.Width = 126
        '
        'DataGridTextBoxColumn9
        '
        Me.DataGridTextBoxColumn9.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.DataGridTextBoxColumn9.Format = ""
        Me.DataGridTextBoxColumn9.FormatInfo = Nothing
        Me.DataGridTextBoxColumn9.HeaderText = "Conto"
        Me.DataGridTextBoxColumn9.MappingName = "CorCnTrp"
        Me.DataGridTextBoxColumn9.ReadOnly = True
        Me.DataGridTextBoxColumn9.Width = 50
        '
        'DataGridTextBoxColumn10
        '
        Me.DataGridTextBoxColumn10.Format = ""
        Me.DataGridTextBoxColumn10.FormatInfo = Nothing
        Me.DataGridTextBoxColumn10.MappingName = "CorDesCpt"
        Me.DataGridTextBoxColumn10.ReadOnly = True
        Me.DataGridTextBoxColumn10.Width = 0
        '
        'DataGridTextBoxColumn11
        '
        Me.DataGridTextBoxColumn11.Format = ""
        Me.DataGridTextBoxColumn11.FormatInfo = Nothing
        Me.DataGridTextBoxColumn11.MappingName = "CorCiva"
        Me.DataGridTextBoxColumn11.ReadOnly = True
        Me.DataGridTextBoxColumn11.Width = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.DateTimePicker1)
        Me.GroupBox1.Controls.Add(Me.Numbox2)
        Me.GroupBox1.Controls.Add(Me.ComboBox1)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.ComboBox2)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Location = New System.Drawing.Point(10, -8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(808, 64)
        Me.GroupBox1.TabIndex = 217
        Me.GroupBox1.TabStop = False
        '
        'CheckBox1
        '
        Me.CheckBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox1.Enabled = False
        Me.CheckBox1.Location = New System.Drawing.Point(704, 20)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(88, 24)
        Me.CheckBox1.TabIndex = 50
        Me.CheckBox1.Text = "IN COGE"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(480, 24)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(208, 22)
        Me.DateTimePicker1.TabIndex = 1
        '
        'Numbox2
        '
        Me.Numbox2.FormatInput = "######"
        Me.Numbox2.FormatOutput = "#####0"
        Me.Numbox2.Location = New System.Drawing.Point(392, 24)
        Me.Numbox2.MaxLength = 6
        Me.Numbox2.Name = "Numbox2"
        Me.Numbox2.Size = New System.Drawing.Size(80, 22)
        Me.Numbox2.TabIndex = 0
        Me.Numbox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.DropDownWidth = 184
        Me.ComboBox1.Location = New System.Drawing.Point(112, 24)
        Me.ComboBox1.MaxDropDownItems = 20
        Me.ComboBox1.MaxLength = 2
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(264, 24)
        Me.ComboBox1.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(504, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 19)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "DATA"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(400, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 19)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "NUMERO"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(112, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(224, 19)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "TIPO DOCUMENTO "
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.Location = New System.Drawing.Point(16, 24)
        Me.ComboBox2.MaxLength = 4
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(80, 24)
        Me.ComboBox2.TabIndex = 2
        Me.ComboBox2.TabStop = False
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(16, 8)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(84, 19)
        Me.Label13.TabIndex = 25
        Me.Label13.Text = "ANNO"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox2.Controls.Add(Me.GroupBox4)
        Me.GroupBox2.Controls.Add(Me.GroupBox3)
        Me.GroupBox2.Controls.Add(Me.GroupBox5)
        Me.GroupBox2.Location = New System.Drawing.Point(10, -8)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(992, 200)
        Me.GroupBox2.TabIndex = 218
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "GroupBox2"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.LinkLabel2)
        Me.GroupBox4.Controls.Add(Me.LinkLabel1)
        Me.GroupBox4.Controls.Add(Me.Label14)
        Me.GroupBox4.Controls.Add(Me.Label12)
        Me.GroupBox4.Controls.Add(Me.Label9)
        Me.GroupBox4.Controls.Add(Me.TextBox12)
        Me.GroupBox4.Controls.Add(Me.Numbox5)
        Me.GroupBox4.Controls.Add(Me.Numbox4)
        Me.GroupBox4.Controls.Add(Me.TextBox11)
        Me.GroupBox4.Controls.Add(Me.Numbox3)
        Me.GroupBox4.Controls.Add(Me.Label15)
        Me.GroupBox4.Controls.Add(Me.ComboBox3)
        Me.GroupBox4.Location = New System.Drawing.Point(496, 56)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(496, 144)
        Me.GroupBox4.TabIndex = 1
        Me.GroupBox4.TabStop = False
        '
        'LinkLabel2
        '
        Me.LinkLabel2.DisabledLinkColor = System.Drawing.Color.White
        Me.LinkLabel2.Location = New System.Drawing.Point(64, 8)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(232, 16)
        Me.LinkLabel2.TabIndex = 61
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "DESCRIZIONE PAGAMENTO"
        Me.LinkLabel2.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'LinkLabel1
        '
        Me.LinkLabel1.DisabledLinkColor = System.Drawing.Color.White
        Me.LinkLabel1.Location = New System.Drawing.Point(8, 64)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(57, 16)
        Me.LinkLabel1.TabIndex = 38
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "A.B.I."
        Me.LinkLabel1.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(120, 64)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(153, 16)
        Me.Label14.TabIndex = 28
        Me.Label14.Text = "BANCA APPOGGIO"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(64, 64)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 16)
        Me.Label12.TabIndex = 27
        Me.Label12.Text = "CAB"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(8, 8)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 16)
        Me.Label9.TabIndex = 24
        Me.Label9.Text = "CODICE"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'TextBox12
        '
        Me.TextBox12.Location = New System.Drawing.Point(120, 80)
        Me.TextBox12.MaxLength = 50
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.ReadOnly = True
        Me.TextBox12.Size = New System.Drawing.Size(368, 22)
        Me.TextBox12.TabIndex = 10
        Me.TextBox12.TabStop = False
        '
        'Numbox5
        '
        Me.Numbox5.FormatInput = "#####"
        Me.Numbox5.FormatOutput = "00000"
        Me.Numbox5.Location = New System.Drawing.Point(64, 80)
        Me.Numbox5.MaxLength = 5
        Me.Numbox5.Name = "Numbox5"
        Me.Numbox5.Size = New System.Drawing.Size(56, 22)
        Me.Numbox5.TabIndex = 2
        Me.Numbox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Numbox4
        '
        Me.Numbox4.FormatInput = "#####"
        Me.Numbox4.FormatOutput = "00000"
        Me.Numbox4.Location = New System.Drawing.Point(8, 80)
        Me.Numbox4.MaxLength = 5
        Me.Numbox4.Name = "Numbox4"
        Me.Numbox4.Size = New System.Drawing.Size(56, 22)
        Me.Numbox4.TabIndex = 1
        Me.Numbox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox11
        '
        Me.TextBox11.Location = New System.Drawing.Point(64, 32)
        Me.TextBox11.MaxLength = 60
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.ReadOnly = True
        Me.TextBox11.Size = New System.Drawing.Size(424, 22)
        Me.TextBox11.TabIndex = 7
        Me.TextBox11.TabStop = False
        '
        'Numbox3
        '
        Me.Numbox3.FormatInput = "##0"
        Me.Numbox3.FormatOutput = "###"
        Me.Numbox3.Location = New System.Drawing.Point(8, 32)
        Me.Numbox3.MaxLength = 3
        Me.Numbox3.Name = "Numbox3"
        Me.Numbox3.Size = New System.Drawing.Size(56, 22)
        Me.Numbox3.TabIndex = 0
        Me.Numbox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(8, 120)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(192, 16)
        Me.Label15.TabIndex = 245
        Me.Label15.Text = "COD. IVA NON IMPONIBILE"
        '
        'ComboBox3
        '
        Me.ComboBox3.Location = New System.Drawing.Point(200, 112)
        Me.ComboBox3.MaxDropDownItems = 20
        Me.ComboBox3.MaxLength = 2
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(240, 24)
        Me.ComboBox3.TabIndex = 244
        Me.ComboBox3.TabStop = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.ButtonF8)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.TextBox9)
        Me.GroupBox3.Controls.Add(Me.TextBox8)
        Me.GroupBox3.Controls.Add(Me.TextBox7)
        Me.GroupBox3.Controls.Add(Me.TextBox6)
        Me.GroupBox3.Controls.Add(Me.TextBox5)
        Me.GroupBox3.Controls.Add(Me.TextBox4)
        Me.GroupBox3.Controls.Add(Me.TextBox3)
        Me.GroupBox3.Controls.Add(Me.Numbox1)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Location = New System.Drawing.Point(0, 56)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(496, 144)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        '
        'ButtonF8
        '
        Me.ButtonF8.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF8.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF8.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF8.ImageIndex = 7
        Me.ButtonF8.ImageList = Me.ImageList1_32
        Me.ButtonF8.Location = New System.Drawing.Point(448, 24)
        Me.ButtonF8.Name = "ButtonF8"
        Me.ButtonF8.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF8.TabIndex = 212
        Me.ButtonF8.TabStop = False
        Me.ButtonF8.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 19)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "CODICE"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'TextBox9
        '
        Me.TextBox9.Location = New System.Drawing.Point(272, 32)
        Me.TextBox9.MaxLength = 11
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.ReadOnly = True
        Me.TextBox9.Size = New System.Drawing.Size(160, 22)
        Me.TextBox9.TabIndex = 12
        Me.TextBox9.TabStop = False
        '
        'TextBox8
        '
        Me.TextBox8.Location = New System.Drawing.Point(96, 32)
        Me.TextBox8.MaxLength = 16
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.ReadOnly = True
        Me.TextBox8.Size = New System.Drawing.Size(160, 22)
        Me.TextBox8.TabIndex = 11
        Me.TextBox8.TabStop = False
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(448, 112)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.Size = New System.Drawing.Size(40, 22)
        Me.TextBox7.TabIndex = 10
        Me.TextBox7.TabStop = False
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(56, 112)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ReadOnly = True
        Me.TextBox6.Size = New System.Drawing.Size(392, 22)
        Me.TextBox6.TabIndex = 9
        Me.TextBox6.TabStop = False
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(8, 112)
        Me.TextBox5.MaxLength = 5
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(48, 22)
        Me.TextBox5.TabIndex = 8
        Me.TextBox5.TabStop = False
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(8, 88)
        Me.TextBox4.MaxLength = 50
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(480, 22)
        Me.TextBox4.TabIndex = 7
        Me.TextBox4.TabStop = False
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(8, 64)
        Me.TextBox3.MaxLength = 60
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(480, 22)
        Me.TextBox3.TabIndex = 6
        Me.TextBox3.TabStop = False
        '
        'Numbox1
        '
        Me.Numbox1.FormatInput = "####0"
        Me.Numbox1.FormatOutput = "00000;#;#"
        Me.Numbox1.Location = New System.Drawing.Point(8, 32)
        Me.Numbox1.MaxLength = 5
        Me.Numbox1.Name = "Numbox1"
        Me.Numbox1.Size = New System.Drawing.Size(75, 22)
        Me.Numbox1.TabIndex = 0
        Me.Numbox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(272, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(176, 19)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "CODICE FISCALE"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(96, 8)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(168, 19)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "PARTITA IVA"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.ButtonF1)
        Me.GroupBox5.Controls.Add(Me.ButtonF5)
        Me.GroupBox5.Controls.Add(Me.ButtonF3)
        Me.GroupBox5.Location = New System.Drawing.Point(808, 0)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(184, 64)
        Me.GroupBox5.TabIndex = 0
        Me.GroupBox5.TabStop = False
        '
        'ButtonF1
        '
        Me.ButtonF1.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF1.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF1.ImageIndex = 0
        Me.ButtonF1.ImageList = Me.ImageList1_32
        Me.ButtonF1.Location = New System.Drawing.Point(128, 16)
        Me.ButtonF1.Name = "ButtonF1"
        Me.ButtonF1.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF1.TabIndex = 64
        Me.ButtonF1.UseVisualStyleBackColor = False
        '
        'ButtonF5
        '
        Me.ButtonF5.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF5.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF5.ImageIndex = 4
        Me.ButtonF5.ImageList = Me.ImageList1_32
        Me.ButtonF5.Location = New System.Drawing.Point(72, 16)
        Me.ButtonF5.Name = "ButtonF5"
        Me.ButtonF5.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF5.TabIndex = 63
        Me.ButtonF5.TabStop = False
        Me.ButtonF5.UseVisualStyleBackColor = False
        '
        'ButtonF3
        '
        Me.ButtonF3.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF3.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF3.ImageIndex = 2
        Me.ButtonF3.ImageList = Me.ImageList1_32
        Me.ButtonF3.Location = New System.Drawing.Point(16, 16)
        Me.ButtonF3.Name = "ButtonF3"
        Me.ButtonF3.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF3.TabIndex = 62
        Me.ButtonF3.TabStop = False
        '
        'GroupBox10
        '
        Me.GroupBox10.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox10.Controls.Add(Me.ButtonF9)
        Me.GroupBox10.Controls.Add(Me.ButtonF2)
        Me.GroupBox10.Location = New System.Drawing.Point(938, 544)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(56, 104)
        Me.GroupBox10.TabIndex = 221
        Me.GroupBox10.TabStop = False
        '
        'ButtonF9
        '
        Me.ButtonF9.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF9.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF9.ImageIndex = 8
        Me.ButtonF9.ImageList = Me.ImageList1_32
        Me.ButtonF9.Location = New System.Drawing.Point(8, 16)
        Me.ButtonF9.Name = "ButtonF9"
        Me.ButtonF9.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF9.TabIndex = 38
        '
        'ButtonF2
        '
        Me.ButtonF2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF2.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF2.ImageIndex = 1
        Me.ButtonF2.ImageList = Me.ImageList1_32
        Me.ButtonF2.Location = New System.Drawing.Point(8, 64)
        Me.ButtonF2.Name = "ButtonF2"
        Me.ButtonF2.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF2.TabIndex = 37
        '
        'GroupBox7
        '
        Me.GroupBox7.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox7.Controls.Add(Me.ButtonXX)
        Me.GroupBox7.Controls.Add(Me.TextBox14)
        Me.GroupBox7.Controls.Add(Me.ButtonDown)
        Me.GroupBox7.Controls.Add(Me.ButtonUp)
        Me.GroupBox7.Controls.Add(Me.Label4)
        Me.GroupBox7.Controls.Add(Me.Label11)
        Me.GroupBox7.Controls.Add(Me.Label22)
        Me.GroupBox7.Controls.Add(Me.Label23)
        Me.GroupBox7.Controls.Add(Me.Numbox6)
        Me.GroupBox7.Controls.Add(Me.Numbox11)
        Me.GroupBox7.Controls.Add(Me.Numbox12)
        Me.GroupBox7.Controls.Add(Me.Label19)
        Me.GroupBox7.Controls.Add(Me.ButtonF8A)
        Me.GroupBox7.Controls.Add(Me.Label10)
        Me.GroupBox7.Controls.Add(Me.TextBox1)
        Me.GroupBox7.Controls.Add(Me.Label8)
        Me.GroupBox7.Controls.Add(Me.ComboBox4)
        Me.GroupBox7.Controls.Add(Me.ButtonF3A)
        Me.GroupBox7.Controls.Add(Me.ButtonF5A)
        Me.GroupBox7.Controls.Add(Me.ButtonF11)
        Me.GroupBox7.Controls.Add(Me.TextBox2)
        Me.GroupBox7.Controls.Add(Me.TextBox16)
        Me.GroupBox7.Location = New System.Drawing.Point(10, 192)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(992, 472)
        Me.GroupBox7.TabIndex = 219
        Me.GroupBox7.TabStop = False
        '
        'ButtonXX
        '
        Me.ButtonXX.ImageIndex = 18
        Me.ButtonXX.ImageList = Me.ImageList1_32
        Me.ButtonXX.Location = New System.Drawing.Point(808, 424)
        Me.ButtonXX.Name = "ButtonXX"
        Me.ButtonXX.Size = New System.Drawing.Size(24, 23)
        Me.ButtonXX.TabIndex = 8
        Me.BaseTip.SetToolTip(Me.ButtonXX, "CONFERMA RIGA")
        '
        'TextBox14
        '
        Me.TextBox14.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBox14.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox14.Location = New System.Drawing.Point(48, 368)
        Me.TextBox14.MaxLength = 35
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.ReadOnly = True
        Me.TextBox14.Size = New System.Drawing.Size(400, 22)
        Me.TextBox14.TabIndex = 1
        Me.TextBox14.TabStop = False
        '
        'ButtonDown
        '
        Me.ButtonDown.ImageIndex = 23
        Me.ButtonDown.ImageList = Me.ImageList1_32
        Me.ButtonDown.Location = New System.Drawing.Point(904, 376)
        Me.ButtonDown.Name = "ButtonDown"
        Me.ButtonDown.Size = New System.Drawing.Size(17, 17)
        Me.ButtonDown.TabIndex = 243
        Me.ButtonDown.TabStop = False
        '
        'ButtonUp
        '
        Me.ButtonUp.ImageIndex = 24
        Me.ButtonUp.ImageList = Me.ImageList1_32
        Me.ButtonUp.Location = New System.Drawing.Point(904, 360)
        Me.ButtonUp.Name = "ButtonUp"
        Me.ButtonUp.Size = New System.Drawing.Size(17, 17)
        Me.ButtonUp.TabIndex = 242
        Me.ButtonUp.TabStop = False
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 352)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 16)
        Me.Label4.TabIndex = 241
        Me.Label4.Text = "COD."
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(616, 352)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(96, 16)
        Me.Label11.TabIndex = 239
        Me.Label11.Text = "Importo"
        '
        'Label22
        '
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(520, 352)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(88, 16)
        Me.Label22.TabIndex = 236
        Me.Label22.Text = "Prezzo Unit."
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(448, 352)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(56, 16)
        Me.Label23.TabIndex = 235
        Me.Label23.Text = "Qta"
        '
        'Numbox6
        '
        Me.Numbox6.FormatInput = "######0.00"
        Me.Numbox6.FormatOutput = "#,###,##0.00"
        Me.Numbox6.Location = New System.Drawing.Point(616, 368)
        Me.Numbox6.MaxLength = 10
        Me.Numbox6.Name = "Numbox6"
        Me.Numbox6.ReadOnly = True
        Me.Numbox6.Size = New System.Drawing.Size(112, 22)
        Me.Numbox6.TabIndex = 4
        Me.Numbox6.TabStop = False
        Me.Numbox6.Text = "Numbox6"
        Me.Numbox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Numbox11
        '
        Me.Numbox11.FormatInput = "######0.00"
        Me.Numbox11.FormatOutput = "#,###,##0.00"
        Me.Numbox11.Location = New System.Drawing.Point(520, 368)
        Me.Numbox11.MaxLength = 10
        Me.Numbox11.Name = "Numbox11"
        Me.Numbox11.Size = New System.Drawing.Size(96, 22)
        Me.Numbox11.TabIndex = 3
        Me.Numbox11.Text = "Numbox11"
        Me.Numbox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Numbox12
        '
        Me.Numbox12.FormatInput = "####0.000"
        Me.Numbox12.FormatOutput = "###,##0.000"
        Me.Numbox12.Location = New System.Drawing.Point(448, 368)
        Me.Numbox12.MaxLength = 9
        Me.Numbox12.Name = "Numbox12"
        Me.Numbox12.Size = New System.Drawing.Size(72, 22)
        Me.Numbox12.TabIndex = 2
        Me.Numbox12.TabStop = False
        Me.Numbox12.Text = "Numbox12"
        Me.Numbox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label19
        '
        Me.Label19.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label19.Location = New System.Drawing.Point(56, 352)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(296, 16)
        Me.Label19.TabIndex = 227
        Me.Label19.Text = "DESCRIZIONE"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'ButtonF8A
        '
        Me.ButtonF8A.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonF8A.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF8A.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF8A.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF8A.ImageIndex = 7
        Me.ButtonF8A.ImageList = Me.ImageList1_32
        Me.ButtonF8A.Location = New System.Drawing.Point(936, 208)
        Me.ButtonF8A.Name = "ButtonF8A"
        Me.ButtonF8A.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF8A.TabIndex = 226
        Me.ButtonF8A.TabStop = False
        Me.ButtonF8A.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(448, 400)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(48, 19)
        Me.Label10.TabIndex = 225
        Me.Label10.Text = "CONTO"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox1.Location = New System.Drawing.Point(448, 424)
        Me.TextBox1.MaxLength = 35
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(56, 22)
        Me.TextBox1.TabIndex = 6
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(736, 352)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(120, 16)
        Me.Label8.TabIndex = 222
        Me.Label8.Text = "CODICE IVA"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'ComboBox4
        '
        Me.ComboBox4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox4.Location = New System.Drawing.Point(728, 368)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(176, 24)
        Me.ComboBox4.TabIndex = 5
        '
        'ButtonF3A
        '
        Me.ButtonF3A.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonF3A.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF3A.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF3A.ImageIndex = 2
        Me.ButtonF3A.ImageList = Me.ImageList1_32
        Me.ButtonF3A.Location = New System.Drawing.Point(936, 160)
        Me.ButtonF3A.Name = "ButtonF3A"
        Me.ButtonF3A.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF3A.TabIndex = 220
        Me.ButtonF3A.TabStop = False
        '
        'ButtonF5A
        '
        Me.ButtonF5A.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonF5A.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF5A.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF5A.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF5A.ImageIndex = 4
        Me.ButtonF5A.ImageList = Me.ImageList1_32
        Me.ButtonF5A.Location = New System.Drawing.Point(936, 112)
        Me.ButtonF5A.Name = "ButtonF5A"
        Me.ButtonF5A.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF5A.TabIndex = 216
        Me.ButtonF5A.TabStop = False
        Me.ButtonF5A.UseVisualStyleBackColor = False
        '
        'ButtonF11
        '
        Me.ButtonF11.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF11.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF11.ImageIndex = 10
        Me.ButtonF11.ImageList = Me.ImageList1_32
        Me.ButtonF11.Location = New System.Drawing.Point(872, 418)
        Me.ButtonF11.Name = "ButtonF11"
        Me.ButtonF11.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF11.TabIndex = 34
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox2.Location = New System.Drawing.Point(504, 424)
        Me.TextBox2.MaxLength = 35
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(288, 22)
        Me.TextBox2.TabIndex = 7
        Me.TextBox2.TabStop = False
        '
        'TextBox16
        '
        Me.TextBox16.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBox16.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox16.Location = New System.Drawing.Point(8, 368)
        Me.TextBox16.MaxLength = 3
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.Size = New System.Drawing.Size(40, 22)
        Me.TextBox16.TabIndex = 0
        Me.TextBox16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'FatNcr
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.ClientSize = New System.Drawing.Size(1012, 656)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox10)
        Me.Controls.Add(Me.GroupBox7)
        Me.Name = "FatNcr"
        Me.Text = "FatNcr"
        Me.Controls.SetChildIndex(Me.GroupBox7, 0)
        Me.Controls.SetChildIndex(Me.GroupBox10, 0)
        Me.Controls.SetChildIndex(Me.GroupBox2, 0)
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.Controls.SetChildIndex(Me.GroupBox6, 0)
        Me.Controls.SetChildIndex(Me.GroupBox9, 0)
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public WriteOnly Property Servizio() As Int16
        Set(ByVal Value As Int16)
            Articolo = Value
        End Set
    End Property
    Dim WithEvents TbRighe As DataTable
    Dim AdRighe As SqlDataAdapter
    Dim RwRighe As DataRow
    Dim BlRighe As SqlCommandBuilder
    Dim RifRif, Numrif, riga, Prog, MaxProg, MaxRig As Int32
    Dim PathSto, PathTmp, PathPrg As String
    Dim ricerca, modifica, Assortito, Diretta, InCoge As Boolean
    Dim Cau, NReg, Ci, CiPerc(72), RegFat, RegRF, RegFF, TipoIva(48), RegIva(48), TipoReg, TipoPag, Anno, CiRicavi, segno, RicCpt, Nimp, NCopie, civa, Articolo As Int16
    Dim Valore, Netto, Importo, Imponibile As Decimal
    Dim ForArt, CF, TipoDoc, Doc, CiDesc(72), CliFat, CptRicavi As String
    Dim datadoc, datamax As Date
    Friend WithEvents prntDoc As System.Drawing.Printing.PrintDocument
    Dim prntDial As New PrintDialog

    Private Sub FatNcr_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RegistraTerminale()
        inizializza()
        Pulisci()
    End Sub
    Private Sub inizializza()
        LeggiTai()
        Nimp = 0 : NCopie = 1
        CheckBox1.Checked = False
    End Sub
    Private Sub LeggiTai()
        Dim x As Int16
        Cmd = New SqlCommand("select top 1 * from TbTai order by TaiAnno desc", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Anno = dataRd.Item("TaiAnno")
            RegFat = dataRd.Item("TaiReg10")
            RegRF = dataRd.Item("TaiReg11")
            RegFF = dataRd.Item("TaiReg12")
            CiRicavi = dataRd.Item("TaiPi1")
            CptRicavi = dataRd.Item("TaiCpt1")
        End If
        dataRd.Close()
        NReg = RegRF
        'AnniLavoro
        Cmd = New SqlCommand("select TaiAnno from TbTai order by TaiAnno desc", cnDb)
        dataRd = Cmd.ExecuteReader
        ComboBox2.Items.Clear()
        While dataRd.Read
            ComboBox2.Items.Add(Format(dataRd.Item("TaiAnno"), "0000"))
        End While
        dataRd.Close()
        ComboBox2.SelectedIndex = 0

        'Causali
        'Cmd = New SqlCommand("Select * from TbTCau where MgCauCli = 1 and MgCauGestione = 1 and MgCauOmaggi=1", cnDb)
        Cmd = New SqlCommand("Select * from TbTCau where MgCauId = 11 or MgCauId = 12", cnDb)
        dataRd = Cmd.ExecuteReader

        ComboBox1.Items.Clear()

        While dataRd.Read
            ComboBox1.Items.Add(Format(dataRd.Item("MgCauId"), "00") & " " & dataRd.Item("MgCauDesc"))
        End While
        dataRd.Close()
        'Lettura Codici Iva
        ComboBox4.Text = ""
        ComboBox4.Items.Clear()
        ComboBox3.Text = ""
        ComboBox3.Items.Clear()
        Cmd = New SqlCommand("SELECT * FROM TbCii where CiiDes > '' order by CiiCod", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            x = dataRd.Item("CiiCod")
            CiPerc(x) = dataRd.Item("CiiAli")
            ComboBox4.Items.Add(dataRd.Item("CiiCod").ToString.PadLeft(2, "0") & " " & dataRd.Item("CiiDes"))
            If dataRd.Item("CiiDes").trim > "" And CiPerc(x) = 0 Then
                ComboBox3.Items.Add(CInt(dataRd.Item("CiiCod")).ToString("00") & " " & dataRd.Item("CiiDes"))
            End If
        End While
        dataRd.Close()
        Cmd = New SqlCommand(" SELECT * from TbSel where selId = 1", cnVd)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            PathPrg = dataRd.Item("sel12") & dataRd.Item("Sel1")
            PathSto = dataRd.Item("sel12") & dataRd.Item("sel11")
            PathTmp = dataRd.Item("sel8")
        End If
        dataRd.Close()
        VerifyDir()
    End Sub
    Sub VerifyDir()
        If Directory.Exists(PathSto) = False Then Directory.CreateDirectory(PathSto)
        If Directory.Exists(PathTmp) = False Then Directory.CreateDirectory(PathTmp)
    End Sub
    Private Sub RoutCliFor()
        CF = "C"
        GroupBox3.Text = "CLIENTE"
        Cau = 12
        SettaCombo(ComboBox1, Cau.ToString.PadLeft(2, "0"), 0)
    End Sub
    Private Sub Pulisci()
        puliscicampi(Me)
        ButtonF1.Enabled = True
        ComboBox2.Enabled = True
        ButtonF5.Enabled = True
        ButtonF3.Enabled = False
        ComboBox1.Enabled = True
        Numbox2.Enabled = True
        GroupBox1.Enabled = True
        GroupBox3.Enabled = True
        GroupBox2.Enabled = False
        GroupBox4.Enabled = True
        GroupBox6.Enabled = False
        GroupBox7.Enabled = False
        GroupBox9.Enabled = False
        GroupBox10.Enabled = False
        Numbox1.Enabled = True
        DateTimePicker1.MinDate = DateTimePicker1.MinDateTime
        DateTimePicker1.MaxDate = DateTimePicker1.MaxDateTime
        Anno = ComboBox2.SelectedItem
        If ComboBox2.SelectedIndex = 0 Then
            DateTimePicker1.Value = DateSerial(Anno, Today.Month, Today.Day)
        Else
            DateTimePicker1.Value = DateSerial(Anno, 12, 31)
        End If
        If RifRif = 0 Then
            DateTimePicker1.MinDate = DateSerial(Anno, 1, 1)
            DateTimePicker1.MaxDate = DateSerial(Anno, 12, 31)
        End If
        Numrif = 0
        Nimp = 0 : NCopie = 1
        RoutCliFor()
        If Cau = 12 Then TipoDoc = "R" : NReg = 5 Else TipoDoc = "F" : NReg = 3
        LeggiRighe(Numrif)
        Numbox2.Focus()
    End Sub
    Private Sub leggiDoc(ByVal tipo As String, ByVal numero As Int32)
        Numrif = Query.RicFatbyNum(Anno, NReg, numero)
        leggiDocbyRif()
    End Sub
    Private Sub leggiDocbyRif()
        Dim leggi As String

        Doc = "Fat"
        leggi = "select * from TbFat where FatRif =" & Numrif

        Cmd = New SqlCommand(leggi, cnDb)

        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            CaricaTestata()
        Else
            Nuovatestata()
        End If
        dataRd.Close()
        SettaCombo(ComboBox1, Cau.ToString.PadLeft(2, "0"), 0)
        'SettaCombo(ComboBox1, NReg.ToString("00"), 0)
        If Cau = 12 Then NReg = 5 Else NReg = 3

        ' NReg = RegIva(ComboBox1.SelectedIndex)
        ' TipoReg = TipoIva(ComboBox1.SelectedIndex)

        leggiCliFor(CF, Numbox1.Text)
        If CheckBox1.Checked = True Then
            ButtonF1.Enabled = False
            GroupBox3.Enabled = False
            GroupBox4.Enabled = False
        End If
        LeggiRighe(Numrif)
        GroupBox2.Enabled = True
        ComboBox1.Enabled = False
        Numbox2.Enabled = False

        DateTimePicker1.Value = datadoc
    End Sub
    Private Sub CaricaTestata()
        Numbox1.Text = dataRd.Item(Doc & "CliCons")
        Numbox2.Text = dataRd.Item(Doc & "Num")
        Numbox3.Text = Format(dataRd.Item(Doc & "PagCod"), Numbox3.FormatOutput)
        Numbox4.Text = Format(dataRd.Item(Doc & "Abi"), Numbox4.FormatOutput)
        Numbox5.Text = Format(dataRd.Item(Doc & "Cab"), Numbox5.FormatOutput)
        SettaCombo(ComboBox3, Format(Val(dataRd.Item("FatNimp")), "00;#;#"), 0)
        datadoc = dataRd.Item(Doc & "Data").date
        Cau = dataRd.Item(Doc & "Cau")
        TipoDoc = dataRd.Item(Doc & "TipoDoc")
        Nimp = dataRd.Item("FatNimp")

        'CheckBox3.Checked = CBool(dataRd.Item(Doc & "Esespe"))
        CheckBox1.Checked = CBool(dataRd.Item(Doc & "Trasf"))
        ComboBox1.Enabled = False
        Numbox2.Enabled = False
        ComboBox2.Enabled = False

        CF = "C"
        NReg = dataRd.Item("FatNumreg")
    End Sub
    Private Sub Nuovatestata()
        Numbox2.Text = Format(0, Numbox2.FormatOutput)
        datadoc = DateTimePicker1.Value.Date
        Cau = Val(Mid(ComboBox1.Text, 1, 2))
        If Cau = 12 Then
            NReg = RegRF
            TipoDoc = "R"
        Else
            NReg = RegFF
            TipoDoc = "F"
        End If
    End Sub
    Private Sub LeggiRighe(ByVal rifer As Int32)
        Cursor.Current = Cursors.WaitCursor

        AdRighe = New SqlDataAdapter("select * from TbCor where CorRif = @Rifer and CorTipoDoc = @Tipo", cnDb)

        BlRighe = New SqlCommandBuilder(AdRighe)

        Dim p1 As New SqlParameter("@rifer", SqlDbType.Int)
        Dim p2 As New SqlParameter("@Tipo", SqlDbType.VarChar)

        p1.Value = rifer
        p2.Value = TipoDoc

        AdRighe.SelectCommand.Parameters.Add(p1)
        AdRighe.SelectCommand.Parameters.Add(p2)

        TbRighe = New DataTable("RIGHE")
        AdRighe.Fill(TbRighe)

        DataGrid1.DataSource = TbRighe
        Cursor.Current = Cursors.Default

        ButtonF3.Enabled = Not CheckBox1.Checked ''(TbRighe.Rows.Count > 0) And Val(Numbox2.Text) = 999999 'And controlladoc(Numrif)


        LeggiMaxProgCorpo()

        TotalizzaDocumento()
    End Sub
    Sub LeggiMaxProgCorpo()
        Cmd = New SqlCommand("select isnull(max(CorProg),0) from TbCor where CorRif = " & Numrif & " and CorTipoDoc ='" & TipoDoc & "'", cnDb)
        MaxProg = Cmd.ExecuteScalar
    End Sub
    Sub TotalizzaDocumento()
        Dim x, y As Int16
        Dim imp(73), iva(73), cor(73) As Decimal
        For x = 0 To 73
            imp(x) = 0
            iva(x) = 0
            cor(x) = 0
        Next
        '''' causale '''''
        segno = 1
        'If Cau = 12 Then segno = -1
        MaxRig = TbRighe.Rows.Count
        Valore = 0
        For x = 1 To MaxRig
            RwRighe = TbRighe.Rows(x - 1)
            If RwRighe.RowState <> DataRowState.Deleted Then
                If segno = -1 Then
                    RwRighe("CorImporto") = Math.Abs(RwRighe("CorImporto"))
                    RwRighe("CorQuaCon") = Math.Abs(RwRighe("CorQuaCon"))
                    RwRighe("CorPrezzo") = Math.Abs(RwRighe("CorPrezzo"))
                End If
                If Nimp = 0 Then
                    y = RwRighe("CorCiva")
                Else
                    y = Nimp
                End If
                'If TipoDoc = "R" Then
                cor(y) = cor(y) + Format(RwRighe("CorImporto"), "###,###,##0.00") * segno
                cor(0) = cor(0) + Format(RwRighe("CorImporto"), "###,###,##0.00") * segno
                'Else
                '    imp(y) = imp(y) + Format(RwRighe("CorImporto"), "###,###,##0.00") * segno
                '    imp(0) = imp(0) + Format(RwRighe("CorImporto"), "###,###,##0.00") * segno
                'End If
            End If
        Next
        For x = 1 To 72
            If cor(x) <> 0 And CiPerc(x) > 0 Then 'And TipoDoc = "R" Then
                'modificato per iva a scorporo 
                'Valore = imp(x) * CiPerc(x) / 100 ' da modificare dopo varie ricerche
                Imponibile = (cor(x) / (100 + CiPerc(x))) * 100
                imp(x) = Format(Imponibile, "###,##0.00")
                Valore = cor(x) - imp(x)
                iva(x) = Format(Valore, "###,##0.00")
                imp(0) = imp(0) + imp(x)
                iva(0) = iva(0) + iva(x)
            End If
            ' x fattura fiscale 
            'If imp(x) <> 0 And CiPerc(x) > 0 And TipoDoc = "F" Then
            '    Valore = imp(x) * CiPerc(x) / 100 ' da modificare dopo varie ricerche
            '    Imponibile = (imp(x) / (100 + CiPerc(x))) * 100
            '    imp(x) = Format(Imponibile, "###,##0.00")
            '    Valore = cor(x) - imp(x)
            '    iva(x) = Format(Valore, "###,##0.00")

            '    'imp(0) = imp(0) + imp(x)
            '    iva(0) = iva(0) + iva(x)
            'End If

        Next
        TextBox10.Text = Format(imp(0), "###,###,##0.00")
        TextBox13.Text = Format(iva(0), "###,###,##0.00")
        ' If TipoDoc = "R" Then
        TextBox40.Text = Format(cor(0), "###,###,##0.00")
        'Else
        '    TextBox40.Text = Format(imp(0) + iva(0), "###,###,##0.00")
        ' End If
    End Sub
    Private Function leggiCliFor(ByVal Cf As String, ByVal codice As String) As Int16
        Dim leggi As String

        leggi = "select * from VClienti where ClCod ='" & codice & "'"

        Cmd = New SqlCommand(leggi, cnDb)
        'puliscicampi(GroupBox3)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Numbox1.Text = dataRd.Item("ClCod")
            TextBox3.Text = dataRd.Item("AnaDesc")
            TextBox4.Text = dataRd.Item("Anaindirizzo")
            TextBox5.Text = dataRd.Item("AnaCap")
            TextBox6.Text = dataRd.Item("AnaCitta")
            TextBox7.Text = dataRd.Item("AnaProv")
            TextBox8.Text = dataRd.Item("AnaPiva")
            TextBox9.Text = dataRd.Item("AnaCfis")
            If Numrif = 0 Then
                Numbox3.Text = Format(dataRd.Item("ClPagam"), Numbox3.FormatOutput)
                Numbox4.Text = Format(dataRd.Item("ClAbi"), Numbox4.FormatOutput)
                Numbox5.Text = Format(dataRd.Item("ClCab"), Numbox5.FormatOutput)
                'CheckBox3.Checked = CBool(dataRd.Item("ClEsespe"))
                'SettaCombo(ComboBox3, Val(dataRd.Item("ClCodivani")).ToString.PadLeft(2, "0"), 0)
            End If
            CliFat = dataRd.Item("ClCod")
            NCopie = 1
        Else
            leggiCliFor = -1
        End If
        dataRd.Close()
        leggipagam(Val(Numbox3.Text))
        LeggiAbiCab(Val(Numbox4.Text), Val(Numbox5.Text))
    End Function
    Private Function leggipagam(ByVal codice As Int16) As Int16
        leggipagam = 0
        Cmd = New SqlCommand("select * from TbPag where pagcod =" & codice, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextBox11.Text = dataRd.Item("PagDesc")
            TipoPag = dataRd.Item("PagTipo")
        Else
            TextBox11.Text = ""
            leggipagam = -1
            TipoPag = 0
        End If
        dataRd.Close()
    End Function
    Private Function LeggiAbiCab(ByVal abi As Int32, ByVal cab As Int32) As Int16
        LeggiAbiCab = 0
        Cmd = New SqlCommand("select * from TbCab where CaAbi =" & abi & " and CaCab =" & cab, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextBox12.Text = dataRd.Item("CaDescFt")
        Else
            TextBox12.Text = ""
            LeggiAbiCab = -1
        End If
        dataRd.Close()
    End Function

    Private Sub ComboBox1_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.LostFocus
        SettaCombo(sender, sender.Text, 0)
        Cau = Mid(ComboBox1.Text, 1, 2)
        If Cau = 12 Then NReg = 5 : TipoDoc = "R" Else NReg = 3 : TipoDoc = "F"
    End Sub
    Private Sub ComboBox3_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.LostFocus
        SettaCombo(sender, sender.Text, 0)

    End Sub
    Private Sub Numbox2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Numbox2.GotFocus
        If ComboBox1.Text.Trim = "" Then
            SettaCombo(ComboBox1, "12", 0) '' causale ricevuta fiscale
            Exit Sub
        End If
    End Sub
    Private Sub Numbox2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Numbox2.LostFocus
        If ComboBox1.Focused Then
            Exit Sub
        End If
        leggiDoc(TipoDoc, Val(Numbox2.Text))
        If MaxDataFatt() > DateTimePicker1.Value.Date Then
            DateTimePicker1.Value = MaxDataFatt()
        End If
        DateTimePicker1.MinDate = MaxDataFatt()
    End Sub

    Private Sub Numbox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Numbox1.Leave 'Numbox3.Enter, ButtonF1.Enter
        If ButtonF8.Focused Then
            Exit Sub
        End If
        If leggiCliFor(CF, Val(Numbox1.Text).ToString("00000")) = -1 Then
            Numbox1.Focus()
        End If
    End Sub
    Private Sub Numbox3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Numbox3.LostFocus
        If leggipagam(Val(sender.text)) = -1 Then
            Numbox3.Focus()
        End If
    End Sub
    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Numbox3.Text = Ricerche.LnkCodPag().ToString(Numbox3.FormatOutput)
        If leggipagam(Val(Numbox3.Text)) = -1 Then
            Numbox3.Focus()
        Else
            SelectNextControl(Numbox3, True, True, True, True)
        End If
    End Sub
    Private Sub Numbox4_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Numbox4.LostFocus, Numbox5.LostFocus
        If Val(Numbox4.Text) = 0 Then
            Numbox5.Text = Format(0, Numbox5.FormatOutput)
        End If
        LeggiAbiCab(Val(Numbox4.Text), Val(Numbox5.Text))
    End Sub
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim rabibanca As abibanca
        rabibanca = Ricerche.LnkAppoggio(Val(Numbox4.Text), Val(Numbox5.Text))
        If rabibanca.abi <> 0 And rabibanca.cab <> 0 Then
            Numbox4.Text = rabibanca.abi.ToString(Numbox2.FormatOutput)
            Numbox5.Text = rabibanca.cab.ToString(Numbox3.FormatOutput)
            If LeggiAbiCab(Val(Numbox4.Text), Val(Numbox5.Text)) = -1 Then
                Numbox4.Focus()
            Else
                SelectNextControl(Numbox3, True, True, True, True)
            End If
        End If
    End Sub
    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Pulisci()
    End Sub

    Private Sub Numbox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Numbox1.KeyUp
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            ButtonF8.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            ButtonF8A.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub FatNcr_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 And ButtonF5.Enabled = True Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 And ButtonF5A.Enabled = True Then
            e.Handled = True
            ButtonF5A.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 And ButtonF3.Enabled = True Then
            e.Handled = True
            ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 And ButtonF3A.Enabled = True Then
            e.Handled = True
            ButtonF3A.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F1 Then
            e.Handled = True
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F2 Then
            e.Handled = True
            ButtonF2.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.Up Then
            e.Handled = True
            If modifica = True Then
                successiva(-1)
            End If
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
    End Sub
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        ButtonF8.Focus()
        RicCliFor(CF)
    End Sub
    Private Sub RicCliFor(ByVal cf As String)
        ricerca = True
        Dim tipo As String
        If cf = "F" Then
            tipo = "FO"
        Else
            tipo = "CL"
        End If

        Dim frm As New RicercaClFo
        frm.StartPosition = FormStartPosition.Manual
        frm.Location = New Point(GroupBox3.Location.X + 80, Me.Location.Y + GroupBox3.Location.Y + 80)
        frm.CliFor = tipo
        frm.ShowDialog()
        Numbox1.Text = frm.codice

        If leggiCliFor(cf, Numbox1.Text) = -1 Then
            Numbox1.Focus()
            Exit Sub
        End If
        Numbox3.Focus()
        ricerca = False
    End Sub
    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If ControllaCampi() = False Then
            Exit Sub
        End If

        ButtonF1.Enabled = False
        CaricaRighe()
    End Sub
    Function ControllaCampi() As Boolean
        Dim Mail As String = ""
        Dim TIPO As System.Object
        If TipoPag = 2 And (Val(Numbox4.Text) = 0 Or Val(Numbox5.Text) = 0) Then Mail = "<>MANCA CODICE ABI e/o CAB " & Chr(13) : TIPO = Numbox4
        If Val(Numbox3.Text) = 0 Then Mail = "<>MANCA CODICE PAGAMENTO " & Chr(13) : TIPO = Numbox3
        If Val(Numbox1.Text) < 1001 Then Mail = Mail & "<>MANCA CLIENTE " & Chr(13) : TIPO = Numbox1
        If Mail > "" Then
            MoltoCritico(Mail)
            TIPO.focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
    Sub MoltoCritico(ByVal Mail As String)
        Dim response As MsgBoxResult
        response = MsgBox(Mail, MsgBoxStyle.Critical, "CONTROLLO INSERIMENTO")
    End Sub
    Private Sub CaricaRighe()
        registratestata()
        ButtonF5.Enabled = False
        GroupBox1.Enabled = False
        GroupBox2.Enabled = False
        GroupBox6.Enabled = True
        GroupBox6.Enabled = True
        GroupBox7.Enabled = True
        GroupBox9.Enabled = True
        GroupBox10.Enabled = False

        LeggiRighe(Numrif)
        DataGrid1.CurrentRowIndex = quante() - 1

        modifica = False
        RicCpt = 0
        PulisciRiga()
    End Sub
    Private Sub successiva(ByVal offset As Int16)
        Dim i, confronto As Int32
        i = DataGrid1.CurrentRowIndex

        If offset > 0 Then
            confronto = quante() - 1
        Else
            confronto = 1
        End If


        If (i < confronto And offset > 0) Or (i > confronto And offset < 0) Then
            i = i + offset
            DataGrid1.Select(i)
            DataGrid1.CurrentRowIndex = i
            LeggiRiga(DataGrid1(i, 1))
            TextBox16.Focus()
        Else
            DataGrid1.UnSelect(i)
            PulisciRiga()
        End If
    End Sub
    Private Sub PulisciRiga()
        puliscicampi(GroupBox7)
        ButtonF3A.Enabled = False
        ButtonUp.Enabled = False
        ButtonDown.Enabled = False
        If modifica = True And quante() > 0 Then
            DataGrid1.UnSelect(DataGrid1.CurrentRowIndex)
        End If
        ButtonF11.Enabled = True
        modifica = False
        LeggiRiga(0)
        TextBox16.Focus()
    End Sub
    Private Sub registratestata()
        Dim scrivi As String

        Dim ultimo As New SqlCommand("Select @@identity", cnDb)

        If Numrif = 0 Then
            Numbox2.Text = Format(999999, Numbox2.FormatOutput)

            scrivi = "Insert into TbFat WITH (TABLOCKX) (FatTipoDoc,FatData,FatNum,FatAge,FatCliCons,FatPagCod,FatAbi,FatCab,FatEsespe,FatNimp,FatCau,FatCliFat,FatNumReg,FatLibera) VALUES ( @TipoDoc,@Data,999999,0,@Cliente,@PagCod,@Abi,@Cab,@Esespe,@Nimp,@Cau,@CliFat,@NumReg,'L')"
        Else
            scrivi = "Update TbFat set FatCliCons=@Cliente,FatCliFat = @CliFat,FatData=@Data, FatPagCod =@PagCod, FatAbi=@Abi, FatCab=@Cab, FatEsespe = @Esespe  where FatRif =" & Numrif
        End If

        Dim p1 As New SqlParameter("@TipoDoc", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@Data", SqlDbType.SmallDateTime)
        Dim p3 As New SqlParameter("@Cliente", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@PagCod", SqlDbType.SmallInt)
        Dim p6 As New SqlParameter("@Abi", SqlDbType.Int)
        Dim p7 As New SqlParameter("@Cab", SqlDbType.Int)
        Dim p8 As New SqlParameter("@Esespe", SqlDbType.SmallInt)
        Dim p9 As New SqlParameter("@Nimp", SqlDbType.SmallInt)
        Dim p10 As New SqlParameter("@Cau", SqlDbType.SmallInt)
        Dim p12 As New SqlParameter("@Numreg", SqlDbType.SmallInt)
        Dim p14 As New SqlParameter("@CliFat", SqlDbType.VarChar)

        Nimp = Val(Mid(ComboBox3.Text, 1, 2))

        p1.Value = TipoDoc
        p2.Value = DateTimePicker1.Value.Date
        p3.Value = Val(Numbox1.Text).ToString("00000")
        p4.Value = Val(Numbox3.Text)
        p6.Value = Val(Numbox4.Text)
        p7.Value = Val(Numbox5.Text)
        p8.Value = 0
        p9.Value = Nimp
        p10.Value = Cau
        p12.Value = NReg
        p14.Value = CliFat
        Cmd = New SqlCommand(scrivi, cnDb)
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)

        Cmd.Parameters.Add(p6)
        Cmd.Parameters.Add(p7)
        Cmd.Parameters.Add(p8)
        Cmd.Parameters.Add(p9)
        Cmd.Parameters.Add(p10)
        Cmd.Parameters.Add(p12)
        Cmd.Parameters.Add(p14)

        Cmd.ExecuteNonQuery()

        If Numrif = 0 Then
            Numrif = ultimo.ExecuteScalar
        End If
    End Sub
    Private Function quante() As Integer
        quante = 0
        Dim i As Integer
        For i = 0 To TbRighe.Rows.Count - 1
            If TbRighe.Rows(i).RowState <> DataRowState.Deleted Then
                quante += 1
            End If
        Next
    End Function
    Private Sub DataGrid1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid1.MouseUp
        Dim Hti As DataGrid.HitTestInfo = sender.HitTest(e.X, e.Y)
        If Hti.Row >= 0 Then
            LeggiRiga(sender.item(Hti.Row, 1))
            TextBox16.Focus()
        End If
    End Sub
    Private Sub ButtonF8A_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8A.Click
        TextBox16.Text = Query.Cercaservizi()
        leggiservizi(TextBox16.Text)
        TextBox16.Focus()
    End Sub
    Private Sub TextBox16_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox16.KeyUp
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            ButtonF8A.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub TextBox16_LOSTFOCUS(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox16.LostFocus
        leggiservizi(TextBox16.Text)
        If leggicpt(TextBox1.Text) = -1 Then TextBox16.Focus()
    End Sub
    Private Sub Combobox4_enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.Enter
        Calcoli()
    End Sub
    Private Function leggiservizi(ByVal codart As String) As Int16
        leggiservizi = 0
        Cmd = New SqlCommand("Select * from TbSer where SerCod ='" & codart & "'", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextBox14.Text = dataRd.Item("SerDesc")
            civa = dataRd.Item("SerCi")
            SettaCombo(ComboBox4, civa.ToString("00"), 0)
            TextBox1.Text = dataRd.Item("SerCpt")
        Else
            leggiservizi = -1
            TextBox14.Text = ""
            SettaCombo(ComboBox4, CiRicavi.ToString("00"), 0)
            TextBox1.Text = CptRicavi
        End If
        dataRd.Close()
    End Function
    Private Sub Calcoli()
        'Netto = IIf(IsNumeric(Numbox11.Text), Numbox11.Text, 0) * (1 - IIf(IsNumeric(Numbox10.Text), Numbox10.Text, 0) / 100) * (1 - IIf(IsNumeric(Numbox8.Text), Numbox8.Text, 0) / 100) * (1 - IIf(IsNumeric(Numbox9.Text), Numbox9.Text, 0) / 100)

        'Numbox7.Text = Format(Netto, Numbox7.FormatOutput)

        Importo = Numbox11.Text * IIf(IsNumeric(Numbox12.Text), Numbox12.Text, 0)
        Numbox6.Text = Format(Importo, Numbox6.FormatOutput)
    End Sub
    Private Sub Numbox12_enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Numbox12.Enter
        If TextBox16.Text.Trim = "" And TextBox14.Text.Trim = "" Then
            ComboBox4.Text = ""
            TextBox1.Text = ""
            Aggiorna()
            TotalizzaDocumento()
            successiva(1)
            Exit Sub
        End If
    End Sub
    Private Sub Numbox11_enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Numbox11.Enter
        If Val(Numbox12.Text) = 0 Then
            ComboBox4.Text = ""
            TextBox1.Text = ""
            Aggiorna()
            TotalizzaDocumento()
            successiva(1)
            Exit Sub
        End If
        If ComboBox4.Text.Trim = "" Then
            SettaCombo(ComboBox4, CiRicavi.ToString("00"), 0)
        End If
        If TextBox1.Text.Trim = "" Then
            TextBox1.Text = CptRicavi
            leggicpt(TextBox1.Text)
        End If
    End Sub
    Private Sub TextBox1_enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.Enter
        SettaCombo(ComboBox4, ComboBox4.Text.Trim, 0)
        RicCpt = 1
    End Sub
    Private Sub TextBox1_leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If ButtonF8A.Focused = True Then
            Exit Sub
        End If
        RicCpt = 0
    End Sub
    Private Sub ButtonXX_enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonXX.Enter
        If TextBox1.Text.Trim <> "" Then
            If leggicpt(TextBox1.Text.Trim) = -1 Then
                TextBox1.Focus()
            End If
        End If
    End Sub
    Private Sub ButtonXX_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonXX.Click
        If controllo() = True Then
            Aggiorna()
            TotalizzaDocumento()
            successiva(1)
        End If
    End Sub
    Private Sub LeggiRiga(ByVal Prog As Int32)
        Dim lrw() As DataRow
        lrw = TbRighe.Select("CorProg = '" & Prog & "'")
        If lrw.Length = 0 Then
            Nuovariga()
            ButtonF3A.Enabled = False
            ButtonUp.Enabled = False
            ButtonDown.Enabled = False
        Else
            modifica = True
            ButtonF3A.Enabled = True
            ButtonUp.Enabled = True
            ButtonDown.Enabled = True
            RwRighe = lrw(0)
        End If
        CaricaElementi()

        For riga = 0 To quante() - 1
            If RwRighe("CorProg") = DataGrid1.Item(riga, 1) Then
                DataGrid1.Select(riga)
            Else
                DataGrid1.UnSelect(riga)
            End If
        Next

        'ButtonF11.Enabled = False
    End Sub
    Private Sub CaricaElementi()
        TextBox16.Text = RwRighe("CorCodArt")
        TextBox14.Text = RwRighe("CorDesc")
        Numbox12.Text = Format(RwRighe("CorQuaCon"), Numbox12.FormatOutput)
        Numbox11.Text = Format(RwRighe("CorPrezzo"), Numbox11.FormatOutput)
        SettaCombo(ComboBox4, Val(RwRighe("CorCiva")).ToString("00"), 0)
        TextBox16.Text = RwRighe("CorCodArt")
        TextBox1.Text = RwRighe("CorCntrp")
        TextBox2.Text = RwRighe("CorDesCpt")
        Calcoli()
    End Sub
    Private Sub Nuovariga()
        ButtonF3A.Enabled = False
        ButtonUp.Enabled = False
        ButtonDown.Enabled = False
        modifica = False
        AzzeradataRow()
    End Sub
    Sub AzzeradataRow()
        RwRighe = TbRighe.NewRow()
        RwRighe("Cortipodoc") = TipoDoc
        RwRighe("CorRif") = Numrif
        RwRighe("Corprog") = MaxProg + 1
        RwRighe("CorCodArt") = 0
        RwRighe("CorDesc") = ""
       
        'Modificato il 17/05/2010 forza qta a 1 quando la riga è nuova - messo tab stop false su numbox12
        'RwRighe("CorQuaCon") = 0
        RwRighe("CorQuaCon") = 1
        RwRighe("CorPrezzo") = 0
        RwRighe("CorImporto") = 0
        RwRighe("CorCiva") = 0
        RwRighe("CorCntrp") = ""
        RwRighe("Corcau") = Cau
        RwRighe("CorDesciva") = ""
        RwRighe("CorDesCpt") = ""
    End Sub



    Private Function controllo() As Boolean
        controllo = True

        If Val(Numbox12.Text) <> 0 Then
            If ComboBox4.Text.Trim = "" Then
                ComboBox4.Focus()
                Return False
            End If
            If IIf(IsNumeric(Numbox11.Text), Numbox11.Text, 0) = 0 Then
                Numbox11.Focus()
                Return False
            End If
            If TextBox14.Text.Trim = "" Then
                TextBox14.Focus()
                Return False
            End If
            If TextBox1.Text.Trim = "" Then
                TextBox1.Focus()
                Return False
            End If
        End If
        If IIf(IsNumeric(Numbox11.Text), Numbox11.Text, 0) <> 0 Then
            If Val(Numbox12.Text) = 0 Then
                Numbox12.Focus()
                Return False
            End If
        End If
        If TextBox1.Text.Trim <> "" Then
            If leggicpt(TextBox1.Text.Trim) = -1 Then
                TextBox1.Focus()
                Return False
            End If
        End If
    End Function
    Private Sub Aggiorna()
        RwRighe("CorCodArt") = TextBox16.Text
        RwRighe("CorDesc") = TextBox14.Text
        RwRighe("CorQuaCon") = IIf(IsNumeric(Numbox12.Text), CDec(Numbox12.Text), 0)
        RwRighe("CorPrezzo") = IIf(IsNumeric(Numbox11.Text), Numbox11.Text, 0)
        RwRighe("CorImporto") = IIf(IsNumeric(Numbox6.Text), Numbox6.Text, 0)
        If RwRighe("CorQuaCon") <> 0 Then
            RwRighe("CorCiva") = Val(Mid(ComboBox4.Text, 1, 2))
            RwRighe("CorDesciva") = ComboBox4.Text
            RwRighe("CorCntrp") = TextBox1.Text.Trim
            RwRighe("CorDesCpt") = TextBox2.Text.Trim
        Else
            RwRighe("CorCiva") = 0
            RwRighe("CorDesciva") = ""
            RwRighe("CorCntrp") = ""
            RwRighe("CorDesCpt") = ""
        End If
        RwRighe("CorCodArt") = TextBox16.Text.Trim
        If modifica = False Then
            TbRighe.Rows.Add(RwRighe)
            If RwRighe("CorProg") > MaxProg Then
                MaxProg = RwRighe("Corprog")
            End If
            DataGrid1.CurrentRowIndex = quante() - 1
        Else
            DataGrid1.UnSelect(DataGrid1.CurrentRowIndex)
        End If

        RicCpt = 0
    End Sub
    Private Function leggicpt(ByVal codice As String) As Int16
        leggicpt = 0
        Cmd = New SqlCommand("Select * from TbPia where PiaCodCo ='" & codice & "'", cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextBox2.Text = dataRd.Item("PiaAnaCo")
        Else
            TextBox2.Text = ""
            leggicpt = -1
        End If
        dataRd.Close()
    End Function
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3A.Click
        If modifica = False Then
            Exit Sub
        End If
        ButtonF3A.Focus()
        AnnulloRiga()
    End Sub
    Private Sub AnnulloRiga()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "Annullo Completamente la riga evidenziata ?"
        style = MsgBoxStyle.YesNo
        title = "ELIMINAZIONE RIGA DOCUMENTO"
        response = MsgBox(msg, style, title)
        If response = MsgBoxResult.Yes Then
            RwRighe.Delete()
            TotalizzaDocumento()
            ButtonF5A.PerformClick()
        Else
            TextBox16.Focus()
        End If
    End Sub
    Private Sub ButtonF5A_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5A.Click
        ButtonF5A.Focus()
        PulisciRiga()
    End Sub
    Private Sub Annullotestata()
        Dim scrivi As String
        scrivi = "Delete from TbFat where FatRif = " & Numrif
        Cmd = New SqlCommand(scrivi, cnDb)
        Cmd.ExecuteNonQuery()
        scrivi = "Delete from TbDcg where DcgNumRif = " & Numrif
        Cmd = New SqlCommand(scrivi, cnDb)
        Cmd.ExecuteNonQuery()
        scrivi = "Delete from TbBlock where idriferim = " & Numrif & " and IdNumReg = " & NReg
        Cmd = New SqlCommand(scrivi, cnDb)
        Cmd.ExecuteNonQuery()
        EliminaVecchiEffetti()
    End Sub
    Sub EliminaVecchiEffetti()
        Cmd = New SqlCommand("Delete from TbEff where RicRifFat = " & Numrif, cnCo)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ButtonF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF2.Click
        GroupBox2.Enabled = True
        GroupBox5.Enabled = True
        ButtonF5.Enabled = True
        ButtonF5.PerformClick()
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        ButtonF9.Enabled = False
        ButtonF2.Enabled = False
        Diretta = False
        StampaDocumento()
        ButtonF2.Enabled = True
        ButtonF9.Enabled = True
        ButtonF2.PerformClick()
    End Sub
    Private Sub StampaDocumento()
        prntDial.Document = prntDoc
        prntDial.PrinterSettings.Copies = NCopie
        If prntDial.ShowDialog() = DialogResult.OK Then
            Diretta = True
            Cursor.Current = Cursors.WaitCursor
            StampaIldocumento()
            Cursor.Current = Cursors.Default
            ' Me.Close()
        End If
    End Sub
    Private Sub StampaIldocumento()
        Numeradocumento()
        'Me.Close()
        RSChiusura.ChiudeFattura(Numrif)
        EliminaVecchiEffetti()
        RSChiusura.AggiornaEffetti(Numrif)
        StampaFattura()
    End Sub
    Private Sub Numeradocumento()
        If Val(Numbox2.Text) <> 999999 Then Exit Sub
        Cmd = New SqlCommand("exec numeradoc @Tipo,@Numrif", cnDb)
        Dim p1 As New SqlParameter("@Tipo", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@Numrif", SqlDbType.Int)
        p1.Value = TipoDoc
        p2.Value = Numrif
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.ExecuteNonQuery()
    End Sub
    Sub StampaFattura()
        Cursor.Current = Cursors.WaitCursor
        Dim Rpt As New ReportClass
        Dim frm As New LpLp
        Dim TIPDOC As String
        Dim Selectformula As String

        If TipoDoc = "R" Then
            TIPDOC = "RF"
            Rpt = New RSRicevuta
        Else
            TIPDOC = "FA"
            Rpt = New RSFatFisc
        End If

        Selectformula = "{VPRINTFAT.FATRIF} = " & Numrif & " AND {VPRINTFAT2.FATRIF} = " & Numrif
        Rpt.RecordSelectionFormula = Selectformula
        Rpt.SetParameterValue("CAUSALE", Mid(ComboBox1.Text, 4, 30))

        frm.reportsource = Rpt
        frm.Text = "RICEVUTA/FATTURA FISCALE"
        '' frm.Show()
        Dim FileName As String = PathSto & TIPDOC & Numrif & ".pdf"
        RegistraPdf(Rpt, FileName)
        If Diretta = True Then
            Rpt.PrintOptions.PrinterName = prntDial.PrinterSettings.PrinterName
            Rpt.PrintToPrinter(prntDial.PrinterSettings.Copies, False, 0, 0)
        Else
            Process.Start("AcroRd32.exe", FileName)
        End If
    End Sub
    Function RegistraPdf(ByVal Report As ReportClass, ByVal Stampa As String) As Boolean
        SetCRLogOnInfo(Report, CryServer, CryUtente, CryPassword)
        Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Stampa)
    End Function

    Private Sub MgMov_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If GroupBox6.Enabled = True Then
            e.Cancel = True
        Else
            RifRif = 0
            RilasciaTerminale()
        End If
    End Sub

    Private Sub sposta(ByVal offset As Int16, ByVal rw As DataRow)
        Dim rw1, rw2 As DataRow
        Dim prog As Integer

        riga = DataGrid1.CurrentRowIndex

        If (riga + offset) < 0 Or (riga + offset) > (quante() - 1) Then
            Exit Sub
        End If

        DataGrid1.UnSelect(riga)

        prog = DataGrid1.Item(riga + offset, 1)
        rw1 = TbRighe.Select("CorProg = '" & prog & "'")(0)

        rw2 = TbRighe.NewRow
        rw2.ItemArray = rw1.ItemArray

        rw1("CorProg") = rw("Corprog")
        rw("Corprog") = rw2("Corprog")

        rw2.ItemArray = rw1.ItemArray
        rw1.ItemArray = rw.ItemArray
        rw.ItemArray = rw2.ItemArray

        DataGrid1.Select(riga + offset)
        DataGrid1.CurrentRowIndex = riga + offset
        LeggiRiga(DataGrid1.Item(riga + offset, 1))
    End Sub

    Private Sub ButtonUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUp.Click
        sposta(-1, RwRighe)
    End Sub
    Private Sub Buttondown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDown.Click
        sposta(1, RwRighe)
    End Sub
    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        AnnulloMovimenti()
    End Sub
    Private Sub AnnulloMovimenti()
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "Annullo Completamente il Documento " & Numbox2.Text & " DEL " & datadoc & "  ?"
        style = MsgBoxStyle.YesNo
        title = "ELIMINAZIONE COMPLETA DOCUMENTO"
        response = MsgBox(msg, style, title)
        If response = MsgBoxResult.No Then Exit Sub
        AnnulloTotaleDocumento()
        ButtonF5.PerformClick()
    End Sub
    Sub AnnulloTotaleDocumento()
        Dim i As Integer

        Cmd = New SqlCommand("Delete from TbCor where CorTipoDoc ='" & TipoDoc & "' and CorRif = " & Numrif, cnDb)
        Cmd.ExecuteNonQuery()

        Annullotestata()
    End Sub

    Private Sub RilasciaTerminale()
        Cmd = New SqlCommand("delete from Tmprim where TmpTerm ='" & cnDb.WorkstationId.Trim & "'", cnDb)
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub RegistraTerminale()
        Cmd = New SqlCommand("insert into TmpRim (TmpRif,TmpTerm,TmpTipo) SELECT isnull(max(tmprif),0) + 1 , '" & cnDb.WorkstationId & "', '' from TmpRim", cnDb)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ricercaconto()
        If RicCpt = 0 Then
            Exit Sub
        End If

        Dim conto As String = NCCOM.Query.CercaPia.Trim
        If conto.Trim <> "" Then
            TextBox1.Text = conto.Trim
        End If
        leggicpt(TextBox1.Text)
        TextBox1.Focus()
    End Sub

    Private Sub ComboBox3_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectionChangeCommitted
        Pulisci()
    End Sub
    Private Function MaxDataFatt() As Date
        Cmd = New SqlCommand("select isnull(MAX(fatdata),getdate()) as FatData,isnull(max(FatNum),0) as MaxFatNum from TbFat where FatNum > 0 and FatNum < @MaxNumero  and datepart(year,FatData) = @Anno And FatNumReg = " & NReg, cnDb)

        Dim p1 As New SqlParameter("@Anno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@MaxNumero", SqlDbType.Int)
        p1.Value = Anno
        If Numrif = 0 Then
            p2.Value = 999999
        Else
            p2.Value = Val(Numbox2.Text)
        End If
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)

        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            If dataRd.Item("MaxFatNum") = 0 Then
                MaxDataFatt = DateSerial(Anno, 1, 1)
            Else
                MaxDataFatt = dataRd.Item("FatData")
            End If
        End If
        dataRd.Close()
    End Function

    Private Sub ButtonF11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If quante() = 0 Then
            AdRighe.Update(TbRighe)
            TbRighe.AcceptChanges()
            Annullotestata()
            Pulisci()
            ButtonF5.PerformClick()
            Exit Sub
        End If
        ButtonF11.Enabled = False
        puliscicampi(GroupBox7)


        AdRighe.Update(TbRighe)
        TbRighe.AcceptChanges()


        GroupBox6.Enabled = False
        GroupBox7.Enabled = False
        GroupBox9.Enabled = False
        GroupBox10.Enabled = True
        GroupBox10.Focus()
    End Sub
End Class
