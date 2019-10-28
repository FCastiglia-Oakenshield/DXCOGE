Imports System.Data.SqlClient
Imports DXBASE
Imports DevExpress.XtraBars
Public Class XGestioneMenu
    Inherits DXBASE.XWINBASE

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
    Friend WithEvents Button1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents DataGridTextBoxColumn3 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn2 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn1 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents GridControl3 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView4 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridColumn27 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn32 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemImageComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents RepositoryItemImageComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents GridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TextEdit2 As DevExpress.XtraEditors.TextEdit
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(XGestioneMenu))
        Me.TextEdit2 = New DevExpress.XtraEditors.TextEdit()
        Me.Button1 = New DevExpress.XtraEditors.SimpleButton()
        Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
        Me.GridControl3 = New DevExpress.XtraGrid.GridControl()
        Me.GridView4 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.GridColumn27 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn32 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepositoryItemImageComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.RepositoryItemImageComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.DataGridTextBoxColumn3 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn2 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn()
        CType(Me.ImageDx24, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl1.SuspendLayout()
        CType(Me.GridControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemImageComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageGly
        '
        Me.ImageGly.ImageStream = CType(resources.GetObject("ImageGly.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageGly.Images.SetKeyName(0, "")
        Me.ImageGly.Images.SetKeyName(1, "")
        Me.ImageGly.Images.SetKeyName(2, "")
        Me.ImageGly.Images.SetKeyName(3, "")
        Me.ImageGly.Images.SetKeyName(4, "")
        Me.ImageGly.Images.SetKeyName(5, "")
        Me.ImageGly.Images.SetKeyName(6, "")
        Me.ImageGly.Images.SetKeyName(7, "")
        Me.ImageGly.Images.SetKeyName(8, "")
        Me.ImageGly.Images.SetKeyName(9, "")
        Me.ImageGly.Images.SetKeyName(10, "")
        Me.ImageGly.Images.SetKeyName(11, "Red Checkmark 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(12, "Blue Checkmark 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(13, "Green Checkmark 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(14, "Add Document 4 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(15, "Help - Lifesaver 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(16, "Delete Selection 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(17, "Options 3 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(18, "Trash Plastic Full 2 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(19, "Paintbrush and Document 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(20, "View Documents 32 h i32.ico")
        Me.ImageGly.Images.SetKeyName(21, "Print Preview 32 h i32.ico")
        '
        'imageList1
        '
        Me.imageList1.ImageStream = CType(resources.GetObject("imageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageList1.Images.SetKeyName(0, "")
        Me.imageList1.Images.SetKeyName(1, "")
        Me.imageList1.Images.SetKeyName(2, "")
        Me.imageList1.Images.SetKeyName(3, "")
        Me.imageList1.Images.SetKeyName(4, "")
        Me.imageList1.Images.SetKeyName(5, "")
        Me.imageList1.Images.SetKeyName(6, "")
        Me.imageList1.Images.SetKeyName(7, "")
        Me.imageList1.Images.SetKeyName(8, "")
        Me.imageList1.Images.SetKeyName(9, "Red Delete 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(10, "")
        Me.imageList1.Images.SetKeyName(11, "")
        Me.imageList1.Images.SetKeyName(12, "")
        Me.imageList1.Images.SetKeyName(13, "")
        Me.imageList1.Images.SetKeyName(14, "")
        Me.imageList1.Images.SetKeyName(15, "")
        Me.imageList1.Images.SetKeyName(16, "")
        Me.imageList1.Images.SetKeyName(17, "")
        Me.imageList1.Images.SetKeyName(18, "")
        Me.imageList1.Images.SetKeyName(19, "")
        Me.imageList1.Images.SetKeyName(20, "")
        Me.imageList1.Images.SetKeyName(21, "")
        Me.imageList1.Images.SetKeyName(22, "")
        Me.imageList1.Images.SetKeyName(23, "")
        Me.imageList1.Images.SetKeyName(24, "Green Checkmark 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(25, "Blue Checkmark 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(26, "Green Plus 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(27, "Stop Sign 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(28, "Red Checkmark 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(29, "Cargo Ship 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(30, "Red Minus 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(31, "OK 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(32, "Blue Arrow Down 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(33, "Blue Arrow Up 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(34, "Blue Arrow NE 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(35, "Red Arrow Down 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(36, "Red Arrow Up 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(37, "Red Arrow NE 16 h i32.ico")
        Me.imageList1.Images.SetKeyName(38, "Blue Delete 16 h i32.ico")
        '
        'ImageList32
        '
        Me.ImageList32.ImageStream = CType(resources.GetObject("ImageList32.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList32.Images.SetKeyName(0, "")
        Me.ImageList32.Images.SetKeyName(1, "")
        Me.ImageList32.Images.SetKeyName(2, "")
        Me.ImageList32.Images.SetKeyName(3, "")
        Me.ImageList32.Images.SetKeyName(4, "")
        Me.ImageList32.Images.SetKeyName(5, "")
        Me.ImageList32.Images.SetKeyName(6, "")
        Me.ImageList32.Images.SetKeyName(7, "")
        Me.ImageList32.Images.SetKeyName(8, "")
        Me.ImageList32.Images.SetKeyName(9, "")
        Me.ImageList32.Images.SetKeyName(10, "")
        Me.ImageList32.Images.SetKeyName(11, "")
        Me.ImageList32.Images.SetKeyName(12, "")
        Me.ImageList32.Images.SetKeyName(13, "")
        Me.ImageList32.Images.SetKeyName(14, "")
        Me.ImageList32.Images.SetKeyName(15, "")
        Me.ImageList32.Images.SetKeyName(16, "")
        Me.ImageList32.Images.SetKeyName(17, "")
        Me.ImageList32.Images.SetKeyName(18, "")
        Me.ImageList32.Images.SetKeyName(19, "")
        Me.ImageList32.Images.SetKeyName(20, "")
        Me.ImageList32.Images.SetKeyName(21, "")
        Me.ImageList32.Images.SetKeyName(22, "")
        Me.ImageList32.Images.SetKeyName(23, "")
        Me.ImageList32.Images.SetKeyName(24, "")
        Me.ImageList32.Images.SetKeyName(25, "")
        Me.ImageList32.Images.SetKeyName(26, "")
        Me.ImageList32.Images.SetKeyName(27, "")
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
        'ImageDx24
        '
        Me.ImageDx24.ImageStream = CType(resources.GetObject("ImageDx24.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ImageDx24.Images.SetKeyName(0, "F1P.png")
        Me.ImageDx24.Images.SetKeyName(1, "F1.png")
        Me.ImageDx24.Images.SetKeyName(2, "F2.png")
        Me.ImageDx24.Images.SetKeyName(3, "F3.png")
        Me.ImageDx24.Images.SetKeyName(4, "selection.png")
        Me.ImageDx24.Images.SetKeyName(5, "F5.png")
        Me.ImageDx24.Images.SetKeyName(6, "F6.png")
        Me.ImageDx24.Images.SetKeyName(7, "selection.png")
        Me.ImageDx24.Images.SetKeyName(8, "F8.png")
        Me.ImageDx24.Images.SetKeyName(9, "F9.png")
        Me.ImageDx24.Images.SetKeyName(10, "DataAdd.png")
        Me.ImageDx24.Images.SetKeyName(11, "F11.png")
        Me.ImageDx24.Images.SetKeyName(12, "F12.png")
        Me.ImageDx24.Images.SetKeyName(13, "dataOk.png")
        Me.ImageDx24.Images.SetKeyName(14, "F9P.png")
        Me.ImageDx24.Images.SetKeyName(15, "Refresh.png")
        Me.ImageDx24.Images.SetKeyName(16, "Organizer - Delete 24 h p.png")
        Me.ImageDx24.Images.SetKeyName(17, "Organizer - Edit 24 h p.png")
        Me.ImageDx24.Images.SetKeyName(18, "lock.png")
        Me.ImageDx24.Images.SetKeyName(19, "lock_open.png")
        Me.ImageDx24.Images.SetKeyName(20, "data_yellow.png")
        Me.ImageDx24.Images.SetKeyName(21, "layout_horizontal.png")
        Me.ImageDx24.Images.SetKeyName(22, "layout_vertical.png")
        Me.ImageDx24.Images.SetKeyName(23, "Ritorno.png")
        Me.ImageDx24.Images.SetKeyName(24, "A3.png")
        Me.ImageDx24.Images.SetKeyName(25, "A4.png")
        Me.ImageDx24.Images.SetKeyName(26, "redo.png")
        Me.ImageDx24.Images.SetKeyName(27, "undo.png")
        Me.ImageDx24.Images.SetKeyName(28, "brush.png")
        Me.ImageDx24.Images.SetKeyName(29, "checkbox.png")
        Me.ImageDx24.Images.SetKeyName(30, "delete.png")
        Me.ImageDx24.Images.SetKeyName(31, "document_view.png")
        Me.ImageDx24.Images.SetKeyName(32, "media_play.png")
        Me.ImageDx24.Images.SetKeyName(33, "media_play_green.png")
        Me.ImageDx24.Images.SetKeyName(34, "message_information.png")
        Me.ImageDx24.Images.SetKeyName(35, "refresh.png")
        Me.ImageDx24.Images.SetKeyName(36, "sign_warning.png")
        Me.ImageDx24.Images.SetKeyName(37, "view.png")
        Me.ImageDx24.Images.SetKeyName(38, "arrow_down_red.png")
        Me.ImageDx24.Images.SetKeyName(39, "arrow_up_green.png")
        Me.ImageDx24.Images.SetKeyName(40, "tasto prova3.png")
        Me.ImageDx24.Images.SetKeyName(41, "money2_delete.png")
        Me.ImageDx24.Images.SetKeyName(42, "money2_edit.png")
        '
        'TextEdit2
        '
        Me.TextEdit2.EditValue = "TextEdit2"
        Me.TextEdit2.EnterMoveNextControl = True
        Me.TextEdit2.Location = New System.Drawing.Point(7, 245)
        Me.TextEdit2.Name = "TextEdit2"
        Me.TextEdit2.Properties.Appearance.BackColor = System.Drawing.SystemColors.Info
        Me.TextEdit2.Properties.Appearance.Options.UseBackColor = True
        Me.TextEdit2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Yellow
        Me.TextEdit2.Properties.AppearanceFocused.Options.UseBackColor = True
        Me.TextEdit2.Properties.ReadOnly = True
        Me.TextEdit2.Size = New System.Drawing.Size(258, 20)
        Me.TextEdit2.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button1.Location = New System.Drawing.Point(281, 240)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(132, 28)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Gestisci Menu"
        '
        'GroupControl1
        '
        Me.GroupControl1.Controls.Add(Me.GridControl3)
        Me.GroupControl1.Controls.Add(Me.TextEdit2)
        Me.GroupControl1.Controls.Add(Me.Button1)
        Me.GroupControl1.Location = New System.Drawing.Point(293, 190)
        Me.GroupControl1.Name = "GroupControl1"
        Me.GroupControl1.Size = New System.Drawing.Size(426, 277)
        Me.GroupControl1.TabIndex = 1
        Me.GroupControl1.Text = "GRUPPI UTENTI"
        '
        'GridControl3
        '
        Me.GridControl3.Dock = System.Windows.Forms.DockStyle.Top
        Me.GridControl3.FormsUseDefaultLookAndFeel = True
        Me.GridControl3.Location = New System.Drawing.Point(2, 22)
        Me.GridControl3.LookAndFeel.SkinName = "Office 2007 Black"
        Me.GridControl3.MainView = Me.GridView4
        Me.GridControl3.Name = "GridControl3"
        Me.GridControl3.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemImageComboBox1, Me.RepositoryItemImageComboBox2})
        Me.GridControl3.Size = New System.Drawing.Size(422, 222)
        Me.GridControl3.TabIndex = 5
        Me.GridControl3.TabStop = False
        Me.GridControl3.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView4})
        '
        'GridView4
        '
        Me.GridView4.ActiveFilterEnabled = False
        Me.GridView4.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn27, Me.GridColumn32, Me.GridColumn1})
        Me.GridView4.CustomizationFormBounds = New System.Drawing.Rectangle(798, 464, 216, 178)
        Me.GridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
        Me.GridView4.GridControl = Me.GridControl3
        Me.GridView4.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden
        Me.GridView4.Name = "GridView4"
        Me.GridView4.OptionsPrint.ExpandAllGroups = False
        Me.GridView4.OptionsPrint.PrintGroupFooter = False
        Me.GridView4.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridView4.OptionsSelection.MultiSelect = True
        Me.GridView4.OptionsSelection.UseIndicatorForSelection = False
        Me.GridView4.OptionsView.EnableAppearanceEvenRow = True
        Me.GridView4.OptionsView.EnableAppearanceOddRow = True
        Me.GridView4.OptionsView.ShowGroupPanel = False
        Me.GridView4.OptionsView.ShowIndicator = False
        Me.GridView4.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.[Default]
        '
        'GridColumn27
        '
        Me.GridColumn27.Caption = "GrupLavId"
        Me.GridColumn27.FieldName = "GrupLavId"
        Me.GridColumn27.Name = "GridColumn27"
        Me.GridColumn27.OptionsColumn.AllowEdit = False
        Me.GridColumn27.OptionsColumn.AllowFocus = False
        Me.GridColumn27.OptionsColumn.FixedWidth = True
        Me.GridColumn27.OptionsColumn.ReadOnly = True
        Me.GridColumn27.Width = 199
        '
        'GridColumn32
        '
        Me.GridColumn32.Caption = "NOME"
        Me.GridColumn32.FieldName = "GrupLavNome"
        Me.GridColumn32.Name = "GridColumn32"
        Me.GridColumn32.OptionsColumn.AllowEdit = False
        Me.GridColumn32.OptionsColumn.AllowFocus = False
        Me.GridColumn32.OptionsColumn.FixedWidth = True
        Me.GridColumn32.OptionsColumn.ReadOnly = True
        Me.GridColumn32.Visible = True
        Me.GridColumn32.VisibleIndex = 0
        Me.GridColumn32.Width = 200
        '
        'GridColumn1
        '
        Me.GridColumn1.Caption = "DESCRIZIONE"
        Me.GridColumn1.FieldName = "GrupLavDesc"
        Me.GridColumn1.Name = "GridColumn1"
        Me.GridColumn1.OptionsColumn.AllowEdit = False
        Me.GridColumn1.OptionsColumn.ReadOnly = True
        Me.GridColumn1.Visible = True
        Me.GridColumn1.VisibleIndex = 1
        Me.GridColumn1.Width = 300
        '
        'RepositoryItemImageComboBox1
        '
        Me.RepositoryItemImageComboBox1.AutoHeight = False
        Me.RepositoryItemImageComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemImageComboBox1.Name = "RepositoryItemImageComboBox1"
        '
        'RepositoryItemImageComboBox2
        '
        Me.RepositoryItemImageComboBox2.AutoHeight = False
        Me.RepositoryItemImageComboBox2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemImageComboBox2.DropDownRows = 1
        Me.RepositoryItemImageComboBox2.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Etichette Incomplete", 0, 2), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Stampa Completa ( Avvisato CapoBarca)", 1, 3), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Documento Controllato e Archiviato", 2, 1)})
        Me.RepositoryItemImageComboBox2.Name = "RepositoryItemImageComboBox2"
        Me.RepositoryItemImageComboBox2.ReadOnly = True
        '
        'DataGridTextBoxColumn3
        '
        Me.DataGridTextBoxColumn3.Format = ""
        Me.DataGridTextBoxColumn3.FormatInfo = Nothing
        Me.DataGridTextBoxColumn3.HeaderText = "DESCRIZIONE"
        Me.DataGridTextBoxColumn3.MappingName = "GrupLavDesc"
        Me.DataGridTextBoxColumn3.NullText = ""
        Me.DataGridTextBoxColumn3.ReadOnly = True
        Me.DataGridTextBoxColumn3.Width = 300
        '
        'DataGridTextBoxColumn2
        '
        Me.DataGridTextBoxColumn2.Format = ""
        Me.DataGridTextBoxColumn2.FormatInfo = Nothing
        Me.DataGridTextBoxColumn2.HeaderText = "NOME"
        Me.DataGridTextBoxColumn2.MappingName = "GrupLavNome"
        Me.DataGridTextBoxColumn2.NullText = ""
        Me.DataGridTextBoxColumn2.ReadOnly = True
        Me.DataGridTextBoxColumn2.Width = 200
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.MappingName = "GrupLavId"
        Me.DataGridTextBoxColumn1.NullText = ""
        Me.DataGridTextBoxColumn1.ReadOnly = True
        Me.DataGridTextBoxColumn1.Width = 0
        '
        'XGestioneMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(1008, 656)
        Me.Controls.Add(Me.GroupControl1)
        Me.LookAndFeel.SkinName = "Office 2007 Black"
        Me.Name = "XGestioneMenu"
        Me.Text = "GestioneMenu"
        CType(Me.ImageDx24, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl1.ResumeLayout(False)
        CType(Me.GridControl3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemImageComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Shared VociAbil As New ArrayList
    Dim Accessi As New ArrayList
    Dim TbGrup As DataTable
    Dim DaGrup As SqlDataAdapter
    Dim Id As Int32
    Dim Tgest As Int16 = 0
    Dim TipoMenu As String
    Dim RwD As DataRowView

    Private Sub GestioneMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        puliscicampi(Me)
        PopolaGrid()
    End Sub

    Private Sub PopolaGrid()
        If CnMenu.Database = cnDb.Database Then
            Tgest = 1 ''geve
            TipoMenu = "Gestione Vendite"
        ElseIf CnMenu.Database = cnCo.Database Then
            Tgest = 2 '' coge
            TipoMenu = "Contabilita'"
        ElseIf CnMenu.Database = cnVd.Database Then
            Tgest = 4 '' visualdox
            TipoMenu = "Archiviazione Ottica"
        End If
        TbGrup = New DataTable("TbGrup")
        DaGrup = New SqlDataAdapter("Select * from TbGrupLav where substring(GrupLavGest," & Tgest & ",1)=1", cnVd)
        DaGrup.Fill(TbGrup)
        GridControl3.DataSource = TbGrup
        GridView4.SelectRow(0)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextEdit2.Text.ToUpper = "ADMINISTRATOR" Then 'Or ComboBox1.Text = Nomegruppo Then
            MessageBox.Show("Impossibile modificare gli Accessi per Il Gruppo di Lavoro " & TextEdit2.Text & "!", "MODIFICA NON CONSENTITA", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Not TextEdit2.Text > "" Then Return
        ModAccessi(TextEdit2.Text)
        ScriviAcc(Id)
    End Sub

    Private Sub ScriviAcc(ByVal RifId As Int32)
        Dim del As New SqlCommand("delete from TbDXMENU where BARGROUP=" & RifId, CnMenu)
        del.ExecuteNonQuery()
        For JJ As Int16 = 0 To Accessi.Count - 1
            Dim cmd As New SqlCommand("Insert Into TbDXMENU (BARGROUP,BARID) values (@BARGROUP,@BARID)", CnMenu)
            Dim p1 As New SqlParameter("@BARGROUP", SqlDbType.SmallInt)
            Dim p2 As New SqlParameter("@BARID", SqlDbType.SmallInt)
            p1.Value = RifId
            p2.Value = Accessi.Item(JJ)
            cmd.Parameters.Add(p1)
            cmd.Parameters.Add(p2)
            cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Sub ModAccessi(ByVal nome As String)
        VociAbil.Clear()
        Dim cmd As New SqlCommand("Select * from VDXmenu where GrupLavNome='" & nome & "'", CnMenu)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            If Not dataRd.Item("indice") Is DBNull.Value Then VociAbil.Add(dataRd.Item("indice"))
        End While
        dataRd.Close()
        Dim frm As New XAccessiMenu
        frm.VociAbil = VociAbil
        Me.AddOwnedForm(frm)
        frm.WindowState = FormWindowState.Normal
        frm.Text = "Gestione Menu - " & TipoMenu
        frm.ShowDialog()
        Accessi = frm.AccessiX()
    End Sub

    Private Sub GridView4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.Click
        Dim OPZIONI As Array = GridView4.GetSelectedRows
        If OPZIONI.Length > 0 Then
            RwD = GridView4.GetRow(OPZIONI(0))
            Id = RwD("GrupLavId")
            TextEdit2.Text = RwD("GrupLavNome")
        End If
    End Sub

    Private Sub GridView4_SelectionChanged(ByVal sender As Object, ByVal e As DevExpress.Data.SelectionChangedEventArgs) Handles GridView4.SelectionChanged
        Dim OPZIONI As Array = GridView4.GetSelectedRows
        If OPZIONI.Length > 0 Then
            RwD = GridView4.GetRow(OPZIONI(0))
            Id = RwD("GrupLavId")
            TextEdit2.Text = RwD("GrupLavNome")
        End If
    End Sub
End Class
