Imports DXBASE
Imports DXBASE.Util
Imports NCCOM
Imports System.Data.SqlClient
Public Class RSIndici
    Inherits DXBASE.WinBase
    Dim ArDoc, ArDes, ArReg, ArPi, ArDescPi, ArCiv1, ArCiv2, ArCiv3, ArCpt1, ArCpt2 As New ArrayList
    Dim TaiDs As DataSet
    Dim TaiBl As SqlCommandBuilder
    Dim TaiAd As SqlDataAdapter
    Dim TaiRw As DataRow
    Dim NTai As Int16
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
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonF4 As System.Windows.Forms.Button
    Friend WithEvents ButtonF11 As System.Windows.Forms.Button
    Friend WithEvents ButtonF5 As System.Windows.Forms.Button
    Friend WithEvents GroupBox14 As System.Windows.Forms.GroupBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TextBox72 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox73 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox74 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox69 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox70 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox71 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox66 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox67 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox68 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox63 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox64 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox65 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox60 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox61 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox62 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox57 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox58 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox59 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox54 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox55 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox56 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox51 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox52 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox53 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox48 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox49 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox50 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox45 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox46 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox47 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox42 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox43 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox44 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox41 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox40 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox39 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox36 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox37 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox38 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox33 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox34 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox35 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox30 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox31 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox32 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox27 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox28 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox29 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox24 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox25 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox26 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox21 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox22 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox23 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox18 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox19 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox20 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox15 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox16 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox17 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox12 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox14 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox106 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox105 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox104 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox103 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox102 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox101 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox99 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox98 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox97 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox96 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox95 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox93 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox92 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox89 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox88 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents Numbox2 As DXBASE.numbox
    Friend WithEvents Numbox1 As DXBASE.numbox
    Friend WithEvents TextBox211 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox210 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox209 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox203 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox100 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox5 As DXBASE.numbox
    Friend WithEvents GroupBox17 As System.Windows.Forms.GroupBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextBox176 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox16 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox170 As System.Windows.Forms.TextBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents Numbox3 As DXBASE.numbox
    Friend WithEvents TextBox138 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox139 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox216 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox215 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox214 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox10 As DXBASE.numbox
    Friend WithEvents Numbox9 As DXBASE.numbox
    Friend WithEvents Numbox8 As DXBASE.numbox
    Friend WithEvents TextBox213 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox7 As DXBASE.numbox
    Friend WithEvents TextBox212 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox6 As DXBASE.numbox
    Friend WithEvents TextBox208 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox207 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox206 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox205 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox204 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox4 As DXBASE.numbox
    Friend WithEvents TextBox107 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox94 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox109 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox108 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox87 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox86 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox85 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox91 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox90 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBox80 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox81 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox82 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox83 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox84 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox79 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox78 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox77 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox76 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox75 As System.Windows.Forms.TextBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox06 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox05 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox117 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox118 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents Numbox12 As DXBASE.numbox
    Friend WithEvents TextBox119 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox11 As DXBASE.numbox
    Friend WithEvents TextBox116 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox110 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox111 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox04 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox03 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox112 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox115 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents Numbox01 As DXBASE.numbox
    Friend WithEvents TextBox113 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox114 As System.Windows.Forms.TextBox
    Friend WithEvents Numbox02 As DXBASE.numbox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RSIndici))
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.ButtonF4 = New System.Windows.Forms.Button
        Me.ButtonF11 = New System.Windows.Forms.Button
        Me.ButtonF5 = New System.Windows.Forms.Button
        Me.GroupBox14 = New System.Windows.Forms.GroupBox
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.TextBox72 = New System.Windows.Forms.TextBox
        Me.TextBox73 = New System.Windows.Forms.TextBox
        Me.TextBox74 = New System.Windows.Forms.TextBox
        Me.TextBox69 = New System.Windows.Forms.TextBox
        Me.TextBox70 = New System.Windows.Forms.TextBox
        Me.TextBox71 = New System.Windows.Forms.TextBox
        Me.TextBox66 = New System.Windows.Forms.TextBox
        Me.TextBox67 = New System.Windows.Forms.TextBox
        Me.TextBox68 = New System.Windows.Forms.TextBox
        Me.TextBox63 = New System.Windows.Forms.TextBox
        Me.TextBox64 = New System.Windows.Forms.TextBox
        Me.TextBox65 = New System.Windows.Forms.TextBox
        Me.TextBox60 = New System.Windows.Forms.TextBox
        Me.TextBox61 = New System.Windows.Forms.TextBox
        Me.TextBox62 = New System.Windows.Forms.TextBox
        Me.TextBox57 = New System.Windows.Forms.TextBox
        Me.TextBox58 = New System.Windows.Forms.TextBox
        Me.TextBox59 = New System.Windows.Forms.TextBox
        Me.TextBox54 = New System.Windows.Forms.TextBox
        Me.TextBox55 = New System.Windows.Forms.TextBox
        Me.TextBox56 = New System.Windows.Forms.TextBox
        Me.TextBox51 = New System.Windows.Forms.TextBox
        Me.TextBox52 = New System.Windows.Forms.TextBox
        Me.TextBox53 = New System.Windows.Forms.TextBox
        Me.TextBox48 = New System.Windows.Forms.TextBox
        Me.TextBox49 = New System.Windows.Forms.TextBox
        Me.TextBox50 = New System.Windows.Forms.TextBox
        Me.TextBox45 = New System.Windows.Forms.TextBox
        Me.TextBox46 = New System.Windows.Forms.TextBox
        Me.TextBox47 = New System.Windows.Forms.TextBox
        Me.TextBox42 = New System.Windows.Forms.TextBox
        Me.TextBox43 = New System.Windows.Forms.TextBox
        Me.TextBox44 = New System.Windows.Forms.TextBox
        Me.TextBox41 = New System.Windows.Forms.TextBox
        Me.TextBox40 = New System.Windows.Forms.TextBox
        Me.TextBox39 = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.TextBox36 = New System.Windows.Forms.TextBox
        Me.TextBox37 = New System.Windows.Forms.TextBox
        Me.TextBox38 = New System.Windows.Forms.TextBox
        Me.TextBox33 = New System.Windows.Forms.TextBox
        Me.TextBox34 = New System.Windows.Forms.TextBox
        Me.TextBox35 = New System.Windows.Forms.TextBox
        Me.TextBox30 = New System.Windows.Forms.TextBox
        Me.TextBox31 = New System.Windows.Forms.TextBox
        Me.TextBox32 = New System.Windows.Forms.TextBox
        Me.TextBox27 = New System.Windows.Forms.TextBox
        Me.TextBox28 = New System.Windows.Forms.TextBox
        Me.TextBox29 = New System.Windows.Forms.TextBox
        Me.TextBox24 = New System.Windows.Forms.TextBox
        Me.TextBox25 = New System.Windows.Forms.TextBox
        Me.TextBox26 = New System.Windows.Forms.TextBox
        Me.TextBox21 = New System.Windows.Forms.TextBox
        Me.TextBox22 = New System.Windows.Forms.TextBox
        Me.TextBox23 = New System.Windows.Forms.TextBox
        Me.TextBox18 = New System.Windows.Forms.TextBox
        Me.TextBox19 = New System.Windows.Forms.TextBox
        Me.TextBox20 = New System.Windows.Forms.TextBox
        Me.TextBox15 = New System.Windows.Forms.TextBox
        Me.TextBox16 = New System.Windows.Forms.TextBox
        Me.TextBox17 = New System.Windows.Forms.TextBox
        Me.TextBox12 = New System.Windows.Forms.TextBox
        Me.TextBox13 = New System.Windows.Forms.TextBox
        Me.TextBox14 = New System.Windows.Forms.TextBox
        Me.TextBox9 = New System.Windows.Forms.TextBox
        Me.TextBox10 = New System.Windows.Forms.TextBox
        Me.TextBox11 = New System.Windows.Forms.TextBox
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.TextBox7 = New System.Windows.Forms.TextBox
        Me.TextBox8 = New System.Windows.Forms.TextBox
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.TextBox106 = New System.Windows.Forms.TextBox
        Me.TextBox105 = New System.Windows.Forms.TextBox
        Me.TextBox104 = New System.Windows.Forms.TextBox
        Me.TextBox103 = New System.Windows.Forms.TextBox
        Me.TextBox102 = New System.Windows.Forms.TextBox
        Me.TextBox101 = New System.Windows.Forms.TextBox
        Me.TextBox99 = New System.Windows.Forms.TextBox
        Me.TextBox98 = New System.Windows.Forms.TextBox
        Me.TextBox97 = New System.Windows.Forms.TextBox
        Me.TextBox96 = New System.Windows.Forms.TextBox
        Me.TextBox95 = New System.Windows.Forms.TextBox
        Me.TextBox93 = New System.Windows.Forms.TextBox
        Me.TextBox92 = New System.Windows.Forms.TextBox
        Me.TextBox89 = New System.Windows.Forms.TextBox
        Me.TextBox88 = New System.Windows.Forms.TextBox
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.Numbox2 = New DXBASE.numbox
        Me.Numbox1 = New DXBASE.numbox
        Me.TextBox211 = New System.Windows.Forms.TextBox
        Me.TextBox210 = New System.Windows.Forms.TextBox
        Me.TextBox209 = New System.Windows.Forms.TextBox
        Me.TextBox203 = New System.Windows.Forms.TextBox
        Me.TextBox100 = New System.Windows.Forms.TextBox
        Me.Numbox5 = New DXBASE.numbox
        Me.GroupBox17 = New System.Windows.Forms.GroupBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextBox176 = New System.Windows.Forms.TextBox
        Me.GroupBox16 = New System.Windows.Forms.GroupBox
        Me.TextBox170 = New System.Windows.Forms.TextBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.GroupBox11 = New System.Windows.Forms.GroupBox
        Me.Numbox3 = New DXBASE.numbox
        Me.TextBox138 = New System.Windows.Forms.TextBox
        Me.TextBox139 = New System.Windows.Forms.TextBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.TextBox216 = New System.Windows.Forms.TextBox
        Me.TextBox215 = New System.Windows.Forms.TextBox
        Me.TextBox214 = New System.Windows.Forms.TextBox
        Me.Numbox10 = New DXBASE.numbox
        Me.Numbox9 = New DXBASE.numbox
        Me.Numbox8 = New DXBASE.numbox
        Me.TextBox213 = New System.Windows.Forms.TextBox
        Me.Numbox7 = New DXBASE.numbox
        Me.TextBox212 = New System.Windows.Forms.TextBox
        Me.Numbox6 = New DXBASE.numbox
        Me.TextBox208 = New System.Windows.Forms.TextBox
        Me.TextBox207 = New System.Windows.Forms.TextBox
        Me.TextBox206 = New System.Windows.Forms.TextBox
        Me.TextBox205 = New System.Windows.Forms.TextBox
        Me.TextBox204 = New System.Windows.Forms.TextBox
        Me.Numbox4 = New DXBASE.numbox
        Me.TextBox107 = New System.Windows.Forms.TextBox
        Me.TextBox94 = New System.Windows.Forms.TextBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.TextBox109 = New System.Windows.Forms.TextBox
        Me.TextBox108 = New System.Windows.Forms.TextBox
        Me.TextBox87 = New System.Windows.Forms.TextBox
        Me.TextBox86 = New System.Windows.Forms.TextBox
        Me.TextBox85 = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox91 = New System.Windows.Forms.TextBox
        Me.TextBox90 = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextBox80 = New System.Windows.Forms.TextBox
        Me.TextBox81 = New System.Windows.Forms.TextBox
        Me.TextBox82 = New System.Windows.Forms.TextBox
        Me.TextBox83 = New System.Windows.Forms.TextBox
        Me.TextBox84 = New System.Windows.Forms.TextBox
        Me.TextBox79 = New System.Windows.Forms.TextBox
        Me.TextBox78 = New System.Windows.Forms.TextBox
        Me.TextBox77 = New System.Windows.Forms.TextBox
        Me.TextBox76 = New System.Windows.Forms.TextBox
        Me.TextBox75 = New System.Windows.Forms.TextBox
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.TextBox06 = New System.Windows.Forms.TextBox
        Me.TextBox05 = New System.Windows.Forms.TextBox
        Me.TextBox117 = New System.Windows.Forms.TextBox
        Me.TextBox118 = New System.Windows.Forms.TextBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Numbox12 = New DXBASE.numbox
        Me.TextBox119 = New System.Windows.Forms.TextBox
        Me.Numbox11 = New DXBASE.numbox
        Me.TextBox116 = New System.Windows.Forms.TextBox
        Me.TextBox110 = New System.Windows.Forms.TextBox
        Me.TextBox111 = New System.Windows.Forms.TextBox
        Me.TextBox04 = New System.Windows.Forms.TextBox
        Me.TextBox03 = New System.Windows.Forms.TextBox
        Me.TextBox112 = New System.Windows.Forms.TextBox
        Me.TextBox115 = New System.Windows.Forms.TextBox
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.Numbox01 = New DXBASE.numbox
        Me.TextBox113 = New System.Windows.Forms.TextBox
        Me.TextBox114 = New System.Windows.Forms.TextBox
        Me.Numbox02 = New DXBASE.numbox
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox14.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox17.SuspendLayout()
        Me.GroupBox16.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox2.Controls.Add(Me.TextBox2)
        Me.GroupBox2.Location = New System.Drawing.Point(472, 32)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(125, 65)
        Me.GroupBox2.TabIndex = 26
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "ANNO LAVORO"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(32, 28)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(56, 22)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.TabStop = False
        Me.TextBox2.Text = "TextBox2"
        Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ButtonF4
        '
        Me.ButtonF4.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonF4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF4.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF4.Image = CType(resources.GetObject("ButtonF4.Image"), System.Drawing.Image)
        Me.ButtonF4.Location = New System.Drawing.Point(152, 19)
        Me.ButtonF4.Name = "ButtonF4"
        Me.ButtonF4.Size = New System.Drawing.Size(35, 35)
        Me.ButtonF4.TabIndex = 208
        Me.ButtonF4.TabStop = False
        Me.BaseTip.SetToolTip(Me.ButtonF4, "F4 CAMBIO ANNO GESTIONE")
        '
        'ButtonF11
        '
        Me.ButtonF11.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF11.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonF11.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF11.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF11.Image = CType(resources.GetObject("ButtonF11.Image"), System.Drawing.Image)
        Me.ButtonF11.Location = New System.Drawing.Point(88, 16)
        Me.ButtonF11.Name = "ButtonF11"
        Me.ButtonF11.Size = New System.Drawing.Size(40, 40)
        Me.ButtonF11.TabIndex = 58
        '
        'ButtonF5
        '
        Me.ButtonF5.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonF5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF5.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF5.Image = CType(resources.GetObject("ButtonF5.Image"), System.Drawing.Image)
        Me.ButtonF5.Location = New System.Drawing.Point(24, 16)
        Me.ButtonF5.Name = "ButtonF5"
        Me.ButtonF5.Size = New System.Drawing.Size(40, 40)
        Me.ButtonF5.TabIndex = 57
        '
        'GroupBox14
        '
        Me.GroupBox14.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox14.Controls.Add(Me.ButtonF4)
        Me.GroupBox14.Controls.Add(Me.ButtonF11)
        Me.GroupBox14.Controls.Add(Me.ButtonF5)
        Me.GroupBox14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox14.Location = New System.Drawing.Point(656, 32)
        Me.GroupBox14.Name = "GroupBox14"
        Me.GroupBox14.Size = New System.Drawing.Size(216, 65)
        Me.GroupBox14.TabIndex = 27
        Me.GroupBox14.TabStop = False
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(125, 112)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(763, 528)
        Me.TabControl1.TabIndex = 28
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox4)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(755, 499)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Numeratori e Codici Iva"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label9)
        Me.GroupBox4.Controls.Add(Me.Label8)
        Me.GroupBox4.Controls.Add(Me.TextBox72)
        Me.GroupBox4.Controls.Add(Me.TextBox73)
        Me.GroupBox4.Controls.Add(Me.TextBox74)
        Me.GroupBox4.Controls.Add(Me.TextBox69)
        Me.GroupBox4.Controls.Add(Me.TextBox70)
        Me.GroupBox4.Controls.Add(Me.TextBox71)
        Me.GroupBox4.Controls.Add(Me.TextBox66)
        Me.GroupBox4.Controls.Add(Me.TextBox67)
        Me.GroupBox4.Controls.Add(Me.TextBox68)
        Me.GroupBox4.Controls.Add(Me.TextBox63)
        Me.GroupBox4.Controls.Add(Me.TextBox64)
        Me.GroupBox4.Controls.Add(Me.TextBox65)
        Me.GroupBox4.Controls.Add(Me.TextBox60)
        Me.GroupBox4.Controls.Add(Me.TextBox61)
        Me.GroupBox4.Controls.Add(Me.TextBox62)
        Me.GroupBox4.Controls.Add(Me.TextBox57)
        Me.GroupBox4.Controls.Add(Me.TextBox58)
        Me.GroupBox4.Controls.Add(Me.TextBox59)
        Me.GroupBox4.Controls.Add(Me.TextBox54)
        Me.GroupBox4.Controls.Add(Me.TextBox55)
        Me.GroupBox4.Controls.Add(Me.TextBox56)
        Me.GroupBox4.Controls.Add(Me.TextBox51)
        Me.GroupBox4.Controls.Add(Me.TextBox52)
        Me.GroupBox4.Controls.Add(Me.TextBox53)
        Me.GroupBox4.Controls.Add(Me.TextBox48)
        Me.GroupBox4.Controls.Add(Me.TextBox49)
        Me.GroupBox4.Controls.Add(Me.TextBox50)
        Me.GroupBox4.Controls.Add(Me.TextBox45)
        Me.GroupBox4.Controls.Add(Me.TextBox46)
        Me.GroupBox4.Controls.Add(Me.TextBox47)
        Me.GroupBox4.Controls.Add(Me.TextBox42)
        Me.GroupBox4.Controls.Add(Me.TextBox43)
        Me.GroupBox4.Controls.Add(Me.TextBox44)
        Me.GroupBox4.Controls.Add(Me.TextBox41)
        Me.GroupBox4.Controls.Add(Me.TextBox40)
        Me.GroupBox4.Controls.Add(Me.TextBox39)
        Me.GroupBox4.Location = New System.Drawing.Point(451, 48)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(298, 388)
        Me.GroupBox4.TabIndex = 1
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "CODICI IVA"
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(86, 28)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(192, 18)
        Me.Label9.TabIndex = 41
        Me.Label9.Text = "DESCRIZIONE"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(48, 28)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(38, 18)
        Me.Label8.TabIndex = 40
        Me.Label8.Text = "COD."
        '
        'TextBox72
        '
        Me.TextBox72.BackColor = System.Drawing.Color.White
        Me.TextBox72.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox72.Location = New System.Drawing.Point(86, 351)
        Me.TextBox72.Name = "TextBox72"
        Me.TextBox72.ReadOnly = True
        Me.TextBox72.Size = New System.Drawing.Size(202, 22)
        Me.TextBox72.TabIndex = 35
        Me.TextBox72.TabStop = False
        Me.TextBox72.Text = "TextBox72"
        '
        'TextBox73
        '
        Me.TextBox73.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox73.Location = New System.Drawing.Point(48, 351)
        Me.TextBox73.MaxLength = 2
        Me.TextBox73.Name = "TextBox73"
        Me.TextBox73.Size = New System.Drawing.Size(38, 22)
        Me.TextBox73.TabIndex = 34
        Me.TextBox73.Text = "TextBox73"
        '
        'TextBox74
        '
        Me.TextBox74.BackColor = System.Drawing.Color.Aqua
        Me.TextBox74.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox74.Location = New System.Drawing.Point(10, 351)
        Me.TextBox74.Name = "TextBox74"
        Me.TextBox74.ReadOnly = True
        Me.TextBox74.Size = New System.Drawing.Size(38, 22)
        Me.TextBox74.TabIndex = 33
        Me.TextBox74.TabStop = False
        Me.TextBox74.Tag = "1"
        Me.TextBox74.Text = "TextBox74"
        '
        'TextBox69
        '
        Me.TextBox69.BackColor = System.Drawing.Color.White
        Me.TextBox69.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox69.Location = New System.Drawing.Point(86, 323)
        Me.TextBox69.Name = "TextBox69"
        Me.TextBox69.ReadOnly = True
        Me.TextBox69.Size = New System.Drawing.Size(202, 22)
        Me.TextBox69.TabIndex = 32
        Me.TextBox69.TabStop = False
        Me.TextBox69.Text = "TextBox69"
        '
        'TextBox70
        '
        Me.TextBox70.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox70.Location = New System.Drawing.Point(48, 323)
        Me.TextBox70.MaxLength = 2
        Me.TextBox70.Name = "TextBox70"
        Me.TextBox70.Size = New System.Drawing.Size(38, 22)
        Me.TextBox70.TabIndex = 31
        Me.TextBox70.Text = "TextBox70"
        '
        'TextBox71
        '
        Me.TextBox71.BackColor = System.Drawing.Color.Aqua
        Me.TextBox71.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox71.Location = New System.Drawing.Point(10, 323)
        Me.TextBox71.Name = "TextBox71"
        Me.TextBox71.ReadOnly = True
        Me.TextBox71.Size = New System.Drawing.Size(38, 22)
        Me.TextBox71.TabIndex = 30
        Me.TextBox71.TabStop = False
        Me.TextBox71.Tag = "1"
        Me.TextBox71.Text = "TextBox71"
        '
        'TextBox66
        '
        Me.TextBox66.BackColor = System.Drawing.Color.White
        Me.TextBox66.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox66.Location = New System.Drawing.Point(86, 295)
        Me.TextBox66.Name = "TextBox66"
        Me.TextBox66.ReadOnly = True
        Me.TextBox66.Size = New System.Drawing.Size(202, 22)
        Me.TextBox66.TabIndex = 29
        Me.TextBox66.TabStop = False
        Me.TextBox66.Text = "TextBox66"
        '
        'TextBox67
        '
        Me.TextBox67.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox67.Location = New System.Drawing.Point(48, 295)
        Me.TextBox67.MaxLength = 2
        Me.TextBox67.Name = "TextBox67"
        Me.TextBox67.Size = New System.Drawing.Size(38, 22)
        Me.TextBox67.TabIndex = 28
        Me.TextBox67.Text = "TextBox67"
        '
        'TextBox68
        '
        Me.TextBox68.BackColor = System.Drawing.Color.Aqua
        Me.TextBox68.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox68.Location = New System.Drawing.Point(10, 295)
        Me.TextBox68.Name = "TextBox68"
        Me.TextBox68.ReadOnly = True
        Me.TextBox68.Size = New System.Drawing.Size(38, 22)
        Me.TextBox68.TabIndex = 27
        Me.TextBox68.TabStop = False
        Me.TextBox68.Tag = "1"
        Me.TextBox68.Text = "TextBox68"
        '
        'TextBox63
        '
        Me.TextBox63.BackColor = System.Drawing.Color.White
        Me.TextBox63.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox63.Location = New System.Drawing.Point(86, 268)
        Me.TextBox63.Name = "TextBox63"
        Me.TextBox63.ReadOnly = True
        Me.TextBox63.Size = New System.Drawing.Size(202, 22)
        Me.TextBox63.TabIndex = 26
        Me.TextBox63.TabStop = False
        Me.TextBox63.Text = "TextBox63"
        '
        'TextBox64
        '
        Me.TextBox64.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox64.Location = New System.Drawing.Point(48, 268)
        Me.TextBox64.MaxLength = 2
        Me.TextBox64.Name = "TextBox64"
        Me.TextBox64.Size = New System.Drawing.Size(38, 22)
        Me.TextBox64.TabIndex = 25
        Me.TextBox64.Text = "TextBox64"
        '
        'TextBox65
        '
        Me.TextBox65.BackColor = System.Drawing.Color.Aqua
        Me.TextBox65.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox65.Location = New System.Drawing.Point(10, 268)
        Me.TextBox65.Name = "TextBox65"
        Me.TextBox65.ReadOnly = True
        Me.TextBox65.Size = New System.Drawing.Size(38, 22)
        Me.TextBox65.TabIndex = 24
        Me.TextBox65.TabStop = False
        Me.TextBox65.Tag = "1"
        Me.TextBox65.Text = "TextBox65"
        '
        'TextBox60
        '
        Me.TextBox60.BackColor = System.Drawing.Color.White
        Me.TextBox60.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox60.Location = New System.Drawing.Point(86, 240)
        Me.TextBox60.Name = "TextBox60"
        Me.TextBox60.ReadOnly = True
        Me.TextBox60.Size = New System.Drawing.Size(202, 22)
        Me.TextBox60.TabIndex = 23
        Me.TextBox60.TabStop = False
        Me.TextBox60.Text = "TextBox60"
        '
        'TextBox61
        '
        Me.TextBox61.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox61.Location = New System.Drawing.Point(48, 240)
        Me.TextBox61.MaxLength = 2
        Me.TextBox61.Name = "TextBox61"
        Me.TextBox61.Size = New System.Drawing.Size(38, 22)
        Me.TextBox61.TabIndex = 22
        Me.TextBox61.Text = "TextBox61"
        '
        'TextBox62
        '
        Me.TextBox62.BackColor = System.Drawing.Color.Aqua
        Me.TextBox62.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox62.Location = New System.Drawing.Point(10, 240)
        Me.TextBox62.Name = "TextBox62"
        Me.TextBox62.ReadOnly = True
        Me.TextBox62.Size = New System.Drawing.Size(38, 22)
        Me.TextBox62.TabIndex = 21
        Me.TextBox62.TabStop = False
        Me.TextBox62.Tag = "1"
        Me.TextBox62.Text = "TextBox62"
        '
        'TextBox57
        '
        Me.TextBox57.BackColor = System.Drawing.Color.White
        Me.TextBox57.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox57.Location = New System.Drawing.Point(86, 212)
        Me.TextBox57.Name = "TextBox57"
        Me.TextBox57.ReadOnly = True
        Me.TextBox57.Size = New System.Drawing.Size(202, 22)
        Me.TextBox57.TabIndex = 20
        Me.TextBox57.TabStop = False
        Me.TextBox57.Text = "TextBox57"
        '
        'TextBox58
        '
        Me.TextBox58.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox58.Location = New System.Drawing.Point(48, 212)
        Me.TextBox58.MaxLength = 2
        Me.TextBox58.Name = "TextBox58"
        Me.TextBox58.Size = New System.Drawing.Size(38, 22)
        Me.TextBox58.TabIndex = 19
        Me.TextBox58.Text = "TextBox58"
        '
        'TextBox59
        '
        Me.TextBox59.BackColor = System.Drawing.Color.Aqua
        Me.TextBox59.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox59.Location = New System.Drawing.Point(10, 212)
        Me.TextBox59.Name = "TextBox59"
        Me.TextBox59.ReadOnly = True
        Me.TextBox59.Size = New System.Drawing.Size(38, 22)
        Me.TextBox59.TabIndex = 18
        Me.TextBox59.TabStop = False
        Me.TextBox59.Tag = "1"
        Me.TextBox59.Text = "TextBox59"
        '
        'TextBox54
        '
        Me.TextBox54.BackColor = System.Drawing.Color.White
        Me.TextBox54.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox54.Location = New System.Drawing.Point(86, 185)
        Me.TextBox54.Name = "TextBox54"
        Me.TextBox54.ReadOnly = True
        Me.TextBox54.Size = New System.Drawing.Size(202, 22)
        Me.TextBox54.TabIndex = 17
        Me.TextBox54.TabStop = False
        Me.TextBox54.Text = "TextBox54"
        '
        'TextBox55
        '
        Me.TextBox55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox55.Location = New System.Drawing.Point(48, 185)
        Me.TextBox55.MaxLength = 2
        Me.TextBox55.Name = "TextBox55"
        Me.TextBox55.Size = New System.Drawing.Size(38, 22)
        Me.TextBox55.TabIndex = 16
        Me.TextBox55.Text = "TextBox55"
        '
        'TextBox56
        '
        Me.TextBox56.BackColor = System.Drawing.Color.Aqua
        Me.TextBox56.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox56.Location = New System.Drawing.Point(10, 185)
        Me.TextBox56.Name = "TextBox56"
        Me.TextBox56.ReadOnly = True
        Me.TextBox56.Size = New System.Drawing.Size(38, 22)
        Me.TextBox56.TabIndex = 15
        Me.TextBox56.TabStop = False
        Me.TextBox56.Tag = "1"
        Me.TextBox56.Text = "TextBox56"
        '
        'TextBox51
        '
        Me.TextBox51.BackColor = System.Drawing.Color.White
        Me.TextBox51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox51.Location = New System.Drawing.Point(86, 157)
        Me.TextBox51.Name = "TextBox51"
        Me.TextBox51.ReadOnly = True
        Me.TextBox51.Size = New System.Drawing.Size(202, 22)
        Me.TextBox51.TabIndex = 14
        Me.TextBox51.TabStop = False
        Me.TextBox51.Text = "TextBox51"
        '
        'TextBox52
        '
        Me.TextBox52.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox52.Location = New System.Drawing.Point(48, 157)
        Me.TextBox52.MaxLength = 2
        Me.TextBox52.Name = "TextBox52"
        Me.TextBox52.Size = New System.Drawing.Size(38, 22)
        Me.TextBox52.TabIndex = 13
        Me.TextBox52.Text = "TextBox52"
        '
        'TextBox53
        '
        Me.TextBox53.BackColor = System.Drawing.Color.Aqua
        Me.TextBox53.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox53.Location = New System.Drawing.Point(10, 157)
        Me.TextBox53.Name = "TextBox53"
        Me.TextBox53.ReadOnly = True
        Me.TextBox53.Size = New System.Drawing.Size(38, 22)
        Me.TextBox53.TabIndex = 12
        Me.TextBox53.TabStop = False
        Me.TextBox53.Tag = "1"
        Me.TextBox53.Text = "TextBox53"
        '
        'TextBox48
        '
        Me.TextBox48.BackColor = System.Drawing.Color.White
        Me.TextBox48.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox48.Location = New System.Drawing.Point(86, 129)
        Me.TextBox48.Name = "TextBox48"
        Me.TextBox48.ReadOnly = True
        Me.TextBox48.Size = New System.Drawing.Size(202, 22)
        Me.TextBox48.TabIndex = 11
        Me.TextBox48.TabStop = False
        Me.TextBox48.Text = "TextBox48"
        '
        'TextBox49
        '
        Me.TextBox49.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox49.Location = New System.Drawing.Point(48, 129)
        Me.TextBox49.MaxLength = 2
        Me.TextBox49.Name = "TextBox49"
        Me.TextBox49.Size = New System.Drawing.Size(38, 22)
        Me.TextBox49.TabIndex = 10
        Me.TextBox49.Text = "TextBox49"
        '
        'TextBox50
        '
        Me.TextBox50.BackColor = System.Drawing.Color.Aqua
        Me.TextBox50.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox50.Location = New System.Drawing.Point(10, 129)
        Me.TextBox50.Name = "TextBox50"
        Me.TextBox50.ReadOnly = True
        Me.TextBox50.Size = New System.Drawing.Size(38, 22)
        Me.TextBox50.TabIndex = 9
        Me.TextBox50.TabStop = False
        Me.TextBox50.Tag = "1"
        Me.TextBox50.Text = "TextBox50"
        '
        'TextBox45
        '
        Me.TextBox45.BackColor = System.Drawing.Color.White
        Me.TextBox45.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox45.Location = New System.Drawing.Point(86, 102)
        Me.TextBox45.Name = "TextBox45"
        Me.TextBox45.ReadOnly = True
        Me.TextBox45.Size = New System.Drawing.Size(202, 22)
        Me.TextBox45.TabIndex = 8
        Me.TextBox45.TabStop = False
        Me.TextBox45.Text = "TextBox45"
        '
        'TextBox46
        '
        Me.TextBox46.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox46.Location = New System.Drawing.Point(48, 102)
        Me.TextBox46.MaxLength = 2
        Me.TextBox46.Name = "TextBox46"
        Me.TextBox46.Size = New System.Drawing.Size(38, 22)
        Me.TextBox46.TabIndex = 7
        Me.TextBox46.Text = "TextBox46"
        '
        'TextBox47
        '
        Me.TextBox47.BackColor = System.Drawing.Color.Aqua
        Me.TextBox47.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox47.Location = New System.Drawing.Point(10, 102)
        Me.TextBox47.Name = "TextBox47"
        Me.TextBox47.ReadOnly = True
        Me.TextBox47.Size = New System.Drawing.Size(38, 22)
        Me.TextBox47.TabIndex = 6
        Me.TextBox47.TabStop = False
        Me.TextBox47.Tag = "1"
        Me.TextBox47.Text = "TextBox47"
        '
        'TextBox42
        '
        Me.TextBox42.BackColor = System.Drawing.Color.White
        Me.TextBox42.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox42.Location = New System.Drawing.Point(86, 74)
        Me.TextBox42.Name = "TextBox42"
        Me.TextBox42.ReadOnly = True
        Me.TextBox42.Size = New System.Drawing.Size(202, 22)
        Me.TextBox42.TabIndex = 5
        Me.TextBox42.TabStop = False
        Me.TextBox42.Text = "TextBox42"
        '
        'TextBox43
        '
        Me.TextBox43.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox43.Location = New System.Drawing.Point(48, 74)
        Me.TextBox43.MaxLength = 2
        Me.TextBox43.Name = "TextBox43"
        Me.TextBox43.Size = New System.Drawing.Size(38, 22)
        Me.TextBox43.TabIndex = 4
        Me.TextBox43.Text = "TextBox43"
        '
        'TextBox44
        '
        Me.TextBox44.BackColor = System.Drawing.Color.Aqua
        Me.TextBox44.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox44.Location = New System.Drawing.Point(10, 74)
        Me.TextBox44.Name = "TextBox44"
        Me.TextBox44.ReadOnly = True
        Me.TextBox44.Size = New System.Drawing.Size(38, 22)
        Me.TextBox44.TabIndex = 3
        Me.TextBox44.TabStop = False
        Me.TextBox44.Tag = "1"
        Me.TextBox44.Text = "TextBox44"
        '
        'TextBox41
        '
        Me.TextBox41.BackColor = System.Drawing.Color.White
        Me.TextBox41.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox41.Location = New System.Drawing.Point(86, 46)
        Me.TextBox41.Name = "TextBox41"
        Me.TextBox41.ReadOnly = True
        Me.TextBox41.Size = New System.Drawing.Size(202, 22)
        Me.TextBox41.TabIndex = 2
        Me.TextBox41.TabStop = False
        Me.TextBox41.Text = "TextBox41"
        '
        'TextBox40
        '
        Me.TextBox40.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox40.Location = New System.Drawing.Point(48, 46)
        Me.TextBox40.MaxLength = 2
        Me.TextBox40.Name = "TextBox40"
        Me.TextBox40.Size = New System.Drawing.Size(38, 22)
        Me.TextBox40.TabIndex = 1
        Me.TextBox40.Text = "TextBox40"
        '
        'TextBox39
        '
        Me.TextBox39.BackColor = System.Drawing.Color.Aqua
        Me.TextBox39.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox39.Location = New System.Drawing.Point(10, 46)
        Me.TextBox39.Name = "TextBox39"
        Me.TextBox39.ReadOnly = True
        Me.TextBox39.Size = New System.Drawing.Size(38, 22)
        Me.TextBox39.TabIndex = 0
        Me.TextBox39.TabStop = False
        Me.TextBox39.Tag = "1"
        Me.TextBox39.Text = "TextBox39"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TextBox36)
        Me.GroupBox3.Controls.Add(Me.TextBox37)
        Me.GroupBox3.Controls.Add(Me.TextBox38)
        Me.GroupBox3.Controls.Add(Me.TextBox33)
        Me.GroupBox3.Controls.Add(Me.TextBox34)
        Me.GroupBox3.Controls.Add(Me.TextBox35)
        Me.GroupBox3.Controls.Add(Me.TextBox30)
        Me.GroupBox3.Controls.Add(Me.TextBox31)
        Me.GroupBox3.Controls.Add(Me.TextBox32)
        Me.GroupBox3.Controls.Add(Me.TextBox27)
        Me.GroupBox3.Controls.Add(Me.TextBox28)
        Me.GroupBox3.Controls.Add(Me.TextBox29)
        Me.GroupBox3.Controls.Add(Me.TextBox24)
        Me.GroupBox3.Controls.Add(Me.TextBox25)
        Me.GroupBox3.Controls.Add(Me.TextBox26)
        Me.GroupBox3.Controls.Add(Me.TextBox21)
        Me.GroupBox3.Controls.Add(Me.TextBox22)
        Me.GroupBox3.Controls.Add(Me.TextBox23)
        Me.GroupBox3.Controls.Add(Me.TextBox18)
        Me.GroupBox3.Controls.Add(Me.TextBox19)
        Me.GroupBox3.Controls.Add(Me.TextBox20)
        Me.GroupBox3.Controls.Add(Me.TextBox15)
        Me.GroupBox3.Controls.Add(Me.TextBox16)
        Me.GroupBox3.Controls.Add(Me.TextBox17)
        Me.GroupBox3.Controls.Add(Me.TextBox12)
        Me.GroupBox3.Controls.Add(Me.TextBox13)
        Me.GroupBox3.Controls.Add(Me.TextBox14)
        Me.GroupBox3.Controls.Add(Me.TextBox9)
        Me.GroupBox3.Controls.Add(Me.TextBox10)
        Me.GroupBox3.Controls.Add(Me.TextBox11)
        Me.GroupBox3.Controls.Add(Me.TextBox6)
        Me.GroupBox3.Controls.Add(Me.TextBox7)
        Me.GroupBox3.Controls.Add(Me.TextBox8)
        Me.GroupBox3.Controls.Add(Me.TextBox5)
        Me.GroupBox3.Controls.Add(Me.TextBox4)
        Me.GroupBox3.Controls.Add(Me.TextBox3)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Location = New System.Drawing.Point(10, 48)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(432, 388)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "DOCUMENTI"
        '
        'TextBox36
        '
        Me.TextBox36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox36.Location = New System.Drawing.Point(365, 351)
        Me.TextBox36.MaxLength = 2
        Me.TextBox36.Name = "TextBox36"
        Me.TextBox36.Size = New System.Drawing.Size(57, 22)
        Me.TextBox36.TabIndex = 35
        Me.TextBox36.Text = "TextBox36"
        Me.TextBox36.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox37
        '
        Me.TextBox37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox37.Location = New System.Drawing.Point(259, 351)
        Me.TextBox37.MaxLength = 6
        Me.TextBox37.Name = "TextBox37"
        Me.TextBox37.Size = New System.Drawing.Size(106, 22)
        Me.TextBox37.TabIndex = 34
        Me.TextBox37.Text = "TextBox37"
        Me.TextBox37.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox38
        '
        Me.TextBox38.BackColor = System.Drawing.Color.Aqua
        Me.TextBox38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox38.ForeColor = System.Drawing.Color.Black
        Me.TextBox38.Location = New System.Drawing.Point(10, 351)
        Me.TextBox38.Name = "TextBox38"
        Me.TextBox38.ReadOnly = True
        Me.TextBox38.Size = New System.Drawing.Size(249, 22)
        Me.TextBox38.TabIndex = 33
        Me.TextBox38.TabStop = False
        Me.TextBox38.Tag = "1"
        Me.TextBox38.Text = "TextBox38"
        '
        'TextBox33
        '
        Me.TextBox33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox33.Location = New System.Drawing.Point(365, 323)
        Me.TextBox33.MaxLength = 2
        Me.TextBox33.Name = "TextBox33"
        Me.TextBox33.Size = New System.Drawing.Size(57, 22)
        Me.TextBox33.TabIndex = 32
        Me.TextBox33.Text = "TextBox33"
        Me.TextBox33.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox34
        '
        Me.TextBox34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox34.Location = New System.Drawing.Point(259, 323)
        Me.TextBox34.MaxLength = 6
        Me.TextBox34.Name = "TextBox34"
        Me.TextBox34.Size = New System.Drawing.Size(106, 22)
        Me.TextBox34.TabIndex = 31
        Me.TextBox34.Text = "TextBox34"
        Me.TextBox34.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox35
        '
        Me.TextBox35.BackColor = System.Drawing.Color.Aqua
        Me.TextBox35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox35.ForeColor = System.Drawing.Color.Black
        Me.TextBox35.Location = New System.Drawing.Point(10, 323)
        Me.TextBox35.Name = "TextBox35"
        Me.TextBox35.ReadOnly = True
        Me.TextBox35.Size = New System.Drawing.Size(249, 22)
        Me.TextBox35.TabIndex = 30
        Me.TextBox35.TabStop = False
        Me.TextBox35.Tag = "1"
        Me.TextBox35.Text = "TextBox35"
        '
        'TextBox30
        '
        Me.TextBox30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox30.Location = New System.Drawing.Point(365, 295)
        Me.TextBox30.MaxLength = 2
        Me.TextBox30.Name = "TextBox30"
        Me.TextBox30.Size = New System.Drawing.Size(57, 22)
        Me.TextBox30.TabIndex = 29
        Me.TextBox30.Text = "TextBox30"
        Me.TextBox30.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox31
        '
        Me.TextBox31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox31.Location = New System.Drawing.Point(259, 295)
        Me.TextBox31.MaxLength = 6
        Me.TextBox31.Name = "TextBox31"
        Me.TextBox31.Size = New System.Drawing.Size(106, 22)
        Me.TextBox31.TabIndex = 28
        Me.TextBox31.Text = "TextBox31"
        Me.TextBox31.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox32
        '
        Me.TextBox32.BackColor = System.Drawing.Color.Aqua
        Me.TextBox32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox32.ForeColor = System.Drawing.Color.Black
        Me.TextBox32.Location = New System.Drawing.Point(10, 295)
        Me.TextBox32.Name = "TextBox32"
        Me.TextBox32.ReadOnly = True
        Me.TextBox32.Size = New System.Drawing.Size(249, 22)
        Me.TextBox32.TabIndex = 27
        Me.TextBox32.TabStop = False
        Me.TextBox32.Tag = "1"
        Me.TextBox32.Text = "TextBox32"
        '
        'TextBox27
        '
        Me.TextBox27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox27.Location = New System.Drawing.Point(365, 268)
        Me.TextBox27.MaxLength = 2
        Me.TextBox27.Name = "TextBox27"
        Me.TextBox27.Size = New System.Drawing.Size(57, 22)
        Me.TextBox27.TabIndex = 26
        Me.TextBox27.Text = "TextBox27"
        Me.TextBox27.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox28
        '
        Me.TextBox28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox28.Location = New System.Drawing.Point(259, 268)
        Me.TextBox28.MaxLength = 6
        Me.TextBox28.Name = "TextBox28"
        Me.TextBox28.Size = New System.Drawing.Size(106, 22)
        Me.TextBox28.TabIndex = 25
        Me.TextBox28.Text = "TextBox28"
        Me.TextBox28.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox29
        '
        Me.TextBox29.BackColor = System.Drawing.Color.Aqua
        Me.TextBox29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox29.ForeColor = System.Drawing.Color.Black
        Me.TextBox29.Location = New System.Drawing.Point(10, 268)
        Me.TextBox29.Name = "TextBox29"
        Me.TextBox29.ReadOnly = True
        Me.TextBox29.Size = New System.Drawing.Size(249, 22)
        Me.TextBox29.TabIndex = 24
        Me.TextBox29.TabStop = False
        Me.TextBox29.Tag = "1"
        Me.TextBox29.Text = "TextBox29"
        '
        'TextBox24
        '
        Me.TextBox24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox24.Location = New System.Drawing.Point(365, 240)
        Me.TextBox24.MaxLength = 2
        Me.TextBox24.Name = "TextBox24"
        Me.TextBox24.Size = New System.Drawing.Size(57, 22)
        Me.TextBox24.TabIndex = 23
        Me.TextBox24.Text = "TextBox24"
        Me.TextBox24.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox25
        '
        Me.TextBox25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox25.Location = New System.Drawing.Point(259, 240)
        Me.TextBox25.MaxLength = 6
        Me.TextBox25.Name = "TextBox25"
        Me.TextBox25.Size = New System.Drawing.Size(106, 22)
        Me.TextBox25.TabIndex = 22
        Me.TextBox25.Text = "TextBox25"
        Me.TextBox25.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox26
        '
        Me.TextBox26.BackColor = System.Drawing.Color.Aqua
        Me.TextBox26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox26.ForeColor = System.Drawing.Color.Black
        Me.TextBox26.Location = New System.Drawing.Point(10, 240)
        Me.TextBox26.Name = "TextBox26"
        Me.TextBox26.ReadOnly = True
        Me.TextBox26.Size = New System.Drawing.Size(249, 22)
        Me.TextBox26.TabIndex = 21
        Me.TextBox26.TabStop = False
        Me.TextBox26.Tag = "1"
        Me.TextBox26.Text = "TextBox26"
        '
        'TextBox21
        '
        Me.TextBox21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox21.Location = New System.Drawing.Point(365, 212)
        Me.TextBox21.MaxLength = 2
        Me.TextBox21.Name = "TextBox21"
        Me.TextBox21.Size = New System.Drawing.Size(57, 22)
        Me.TextBox21.TabIndex = 20
        Me.TextBox21.Text = "TextBox21"
        Me.TextBox21.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox22
        '
        Me.TextBox22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox22.Location = New System.Drawing.Point(259, 212)
        Me.TextBox22.MaxLength = 6
        Me.TextBox22.Name = "TextBox22"
        Me.TextBox22.Size = New System.Drawing.Size(106, 22)
        Me.TextBox22.TabIndex = 19
        Me.TextBox22.Text = "TextBox22"
        Me.TextBox22.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox23
        '
        Me.TextBox23.BackColor = System.Drawing.Color.Aqua
        Me.TextBox23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox23.ForeColor = System.Drawing.Color.Black
        Me.TextBox23.Location = New System.Drawing.Point(10, 212)
        Me.TextBox23.Name = "TextBox23"
        Me.TextBox23.ReadOnly = True
        Me.TextBox23.Size = New System.Drawing.Size(249, 22)
        Me.TextBox23.TabIndex = 18
        Me.TextBox23.TabStop = False
        Me.TextBox23.Tag = "1"
        Me.TextBox23.Text = "TextBox23"
        '
        'TextBox18
        '
        Me.TextBox18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox18.Location = New System.Drawing.Point(365, 185)
        Me.TextBox18.MaxLength = 2
        Me.TextBox18.Name = "TextBox18"
        Me.TextBox18.Size = New System.Drawing.Size(57, 22)
        Me.TextBox18.TabIndex = 17
        Me.TextBox18.Text = "TextBox18"
        Me.TextBox18.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox19
        '
        Me.TextBox19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox19.Location = New System.Drawing.Point(259, 185)
        Me.TextBox19.MaxLength = 6
        Me.TextBox19.Name = "TextBox19"
        Me.TextBox19.Size = New System.Drawing.Size(106, 22)
        Me.TextBox19.TabIndex = 16
        Me.TextBox19.Text = "TextBox19"
        Me.TextBox19.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox20
        '
        Me.TextBox20.BackColor = System.Drawing.Color.Aqua
        Me.TextBox20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox20.ForeColor = System.Drawing.Color.Black
        Me.TextBox20.Location = New System.Drawing.Point(10, 185)
        Me.TextBox20.Name = "TextBox20"
        Me.TextBox20.ReadOnly = True
        Me.TextBox20.Size = New System.Drawing.Size(249, 22)
        Me.TextBox20.TabIndex = 15
        Me.TextBox20.TabStop = False
        Me.TextBox20.Tag = "1"
        Me.TextBox20.Text = "TextBox20"
        '
        'TextBox15
        '
        Me.TextBox15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox15.Location = New System.Drawing.Point(365, 157)
        Me.TextBox15.MaxLength = 2
        Me.TextBox15.Name = "TextBox15"
        Me.TextBox15.Size = New System.Drawing.Size(57, 22)
        Me.TextBox15.TabIndex = 14
        Me.TextBox15.Text = "TextBox15"
        Me.TextBox15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox16
        '
        Me.TextBox16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox16.Location = New System.Drawing.Point(259, 157)
        Me.TextBox16.MaxLength = 6
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.Size = New System.Drawing.Size(106, 22)
        Me.TextBox16.TabIndex = 13
        Me.TextBox16.Text = "TextBox16"
        Me.TextBox16.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox17
        '
        Me.TextBox17.BackColor = System.Drawing.Color.Aqua
        Me.TextBox17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox17.ForeColor = System.Drawing.Color.Black
        Me.TextBox17.Location = New System.Drawing.Point(10, 157)
        Me.TextBox17.Name = "TextBox17"
        Me.TextBox17.ReadOnly = True
        Me.TextBox17.Size = New System.Drawing.Size(249, 22)
        Me.TextBox17.TabIndex = 12
        Me.TextBox17.TabStop = False
        Me.TextBox17.Tag = "1"
        Me.TextBox17.Text = "TextBox17"
        '
        'TextBox12
        '
        Me.TextBox12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox12.Location = New System.Drawing.Point(365, 129)
        Me.TextBox12.MaxLength = 2
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.Size = New System.Drawing.Size(57, 22)
        Me.TextBox12.TabIndex = 11
        Me.TextBox12.Text = "TextBox12"
        Me.TextBox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox13
        '
        Me.TextBox13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox13.Location = New System.Drawing.Point(259, 129)
        Me.TextBox13.MaxLength = 6
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.Size = New System.Drawing.Size(106, 22)
        Me.TextBox13.TabIndex = 10
        Me.TextBox13.Text = "TextBox13"
        Me.TextBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox14
        '
        Me.TextBox14.BackColor = System.Drawing.Color.Aqua
        Me.TextBox14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox14.ForeColor = System.Drawing.Color.Black
        Me.TextBox14.Location = New System.Drawing.Point(10, 129)
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.ReadOnly = True
        Me.TextBox14.Size = New System.Drawing.Size(249, 22)
        Me.TextBox14.TabIndex = 9
        Me.TextBox14.TabStop = False
        Me.TextBox14.Tag = "1"
        Me.TextBox14.Text = "TextBox14"
        '
        'TextBox9
        '
        Me.TextBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox9.Location = New System.Drawing.Point(365, 102)
        Me.TextBox9.MaxLength = 2
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.Size = New System.Drawing.Size(57, 22)
        Me.TextBox9.TabIndex = 8
        Me.TextBox9.Text = "TextBox9"
        Me.TextBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox10
        '
        Me.TextBox10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox10.Location = New System.Drawing.Point(259, 102)
        Me.TextBox10.MaxLength = 6
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New System.Drawing.Size(106, 22)
        Me.TextBox10.TabIndex = 7
        Me.TextBox10.Text = "TextBox10"
        Me.TextBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox11
        '
        Me.TextBox11.BackColor = System.Drawing.Color.Aqua
        Me.TextBox11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox11.ForeColor = System.Drawing.Color.Black
        Me.TextBox11.Location = New System.Drawing.Point(10, 102)
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.ReadOnly = True
        Me.TextBox11.Size = New System.Drawing.Size(249, 22)
        Me.TextBox11.TabIndex = 6
        Me.TextBox11.TabStop = False
        Me.TextBox11.Tag = "1"
        Me.TextBox11.Text = "TextBox11"
        '
        'TextBox6
        '
        Me.TextBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox6.Location = New System.Drawing.Point(365, 74)
        Me.TextBox6.MaxLength = 2
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(57, 22)
        Me.TextBox6.TabIndex = 5
        Me.TextBox6.Text = "TextBox6"
        Me.TextBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox7
        '
        Me.TextBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox7.Location = New System.Drawing.Point(259, 74)
        Me.TextBox7.MaxLength = 6
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(106, 22)
        Me.TextBox7.TabIndex = 4
        Me.TextBox7.Text = "TextBox7"
        Me.TextBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox8
        '
        Me.TextBox8.BackColor = System.Drawing.Color.Aqua
        Me.TextBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox8.ForeColor = System.Drawing.Color.Black
        Me.TextBox8.Location = New System.Drawing.Point(10, 74)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.ReadOnly = True
        Me.TextBox8.Size = New System.Drawing.Size(249, 22)
        Me.TextBox8.TabIndex = 3
        Me.TextBox8.TabStop = False
        Me.TextBox8.Tag = "1"
        Me.TextBox8.Text = "TextBox8"
        '
        'TextBox5
        '
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox5.Location = New System.Drawing.Point(365, 46)
        Me.TextBox5.MaxLength = 2
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(57, 22)
        Me.TextBox5.TabIndex = 2
        Me.TextBox5.Text = "TextBox5"
        Me.TextBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox4
        '
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox4.Location = New System.Drawing.Point(259, 46)
        Me.TextBox4.MaxLength = 6
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(106, 22)
        Me.TextBox4.TabIndex = 1
        Me.TextBox4.Text = "TextBox4"
        Me.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox3
        '
        Me.TextBox3.BackColor = System.Drawing.Color.Aqua
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox3.ForeColor = System.Drawing.Color.Black
        Me.TextBox3.Location = New System.Drawing.Point(10, 46)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(249, 22)
        Me.TextBox3.TabIndex = 0
        Me.TextBox3.TabStop = False
        Me.TextBox3.Tag = "1"
        Me.TextBox3.Text = "TextBox3"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(365, 28)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(62, 18)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "REG."
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(259, 28)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(106, 18)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "PROGRESSIVO"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(10, 28)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(220, 18)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "TIPO"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Controls.Add(Me.GroupBox7)
        Me.TabPage2.Controls.Add(Me.GroupBox17)
        Me.TabPage2.Controls.Add(Me.GroupBox16)
        Me.TabPage2.Controls.Add(Me.GroupBox11)
        Me.TabPage2.Controls.Add(Me.GroupBox6)
        Me.TabPage2.Controls.Add(Me.GroupBox5)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(755, 499)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Personalizzazione Gestione"
        Me.TabPage2.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TextBox106)
        Me.GroupBox1.Controls.Add(Me.TextBox105)
        Me.GroupBox1.Controls.Add(Me.TextBox104)
        Me.GroupBox1.Controls.Add(Me.TextBox103)
        Me.GroupBox1.Controls.Add(Me.TextBox102)
        Me.GroupBox1.Controls.Add(Me.TextBox101)
        Me.GroupBox1.Controls.Add(Me.TextBox99)
        Me.GroupBox1.Controls.Add(Me.TextBox98)
        Me.GroupBox1.Controls.Add(Me.TextBox97)
        Me.GroupBox1.Controls.Add(Me.TextBox96)
        Me.GroupBox1.Controls.Add(Me.TextBox95)
        Me.GroupBox1.Controls.Add(Me.TextBox93)
        Me.GroupBox1.Controls.Add(Me.TextBox92)
        Me.GroupBox1.Controls.Add(Me.TextBox89)
        Me.GroupBox1.Controls.Add(Me.TextBox88)
        Me.GroupBox1.Location = New System.Drawing.Point(352, 168)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(392, 168)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "CONTROPARTITE VARIE"
        '
        'TextBox106
        '
        Me.TextBox106.BackColor = System.Drawing.Color.White
        Me.TextBox106.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox106.Location = New System.Drawing.Point(144, 112)
        Me.TextBox106.Name = "TextBox106"
        Me.TextBox106.ReadOnly = True
        Me.TextBox106.Size = New System.Drawing.Size(232, 22)
        Me.TextBox106.TabIndex = 9
        Me.TextBox106.TabStop = False
        Me.TextBox106.Text = ""
        '
        'TextBox105
        '
        Me.TextBox105.BackColor = System.Drawing.Color.White
        Me.TextBox105.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox105.Location = New System.Drawing.Point(144, 88)
        Me.TextBox105.Name = "TextBox105"
        Me.TextBox105.ReadOnly = True
        Me.TextBox105.Size = New System.Drawing.Size(232, 22)
        Me.TextBox105.TabIndex = 7
        Me.TextBox105.TabStop = False
        Me.TextBox105.Text = ""
        '
        'TextBox104
        '
        Me.TextBox104.BackColor = System.Drawing.Color.White
        Me.TextBox104.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox104.Location = New System.Drawing.Point(144, 64)
        Me.TextBox104.Name = "TextBox104"
        Me.TextBox104.ReadOnly = True
        Me.TextBox104.Size = New System.Drawing.Size(232, 22)
        Me.TextBox104.TabIndex = 5
        Me.TextBox104.TabStop = False
        Me.TextBox104.Text = ""
        '
        'TextBox103
        '
        Me.TextBox103.BackColor = System.Drawing.Color.White
        Me.TextBox103.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox103.Location = New System.Drawing.Point(144, 40)
        Me.TextBox103.Name = "TextBox103"
        Me.TextBox103.ReadOnly = True
        Me.TextBox103.Size = New System.Drawing.Size(232, 22)
        Me.TextBox103.TabIndex = 3
        Me.TextBox103.TabStop = False
        Me.TextBox103.Text = ""
        '
        'TextBox102
        '
        Me.TextBox102.BackColor = System.Drawing.Color.White
        Me.TextBox102.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox102.Location = New System.Drawing.Point(144, 16)
        Me.TextBox102.Name = "TextBox102"
        Me.TextBox102.ReadOnly = True
        Me.TextBox102.Size = New System.Drawing.Size(232, 22)
        Me.TextBox102.TabIndex = 1
        Me.TextBox102.TabStop = False
        Me.TextBox102.Text = ""
        '
        'TextBox101
        '
        Me.TextBox101.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox101.Location = New System.Drawing.Point(96, 112)
        Me.TextBox101.MaxLength = 5
        Me.TextBox101.Name = "TextBox101"
        Me.TextBox101.Size = New System.Drawing.Size(48, 22)
        Me.TextBox101.TabIndex = 8
        Me.TextBox101.Text = ""
        Me.TextBox101.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox99
        '
        Me.TextBox99.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox99.Location = New System.Drawing.Point(96, 88)
        Me.TextBox99.MaxLength = 5
        Me.TextBox99.Name = "TextBox99"
        Me.TextBox99.Size = New System.Drawing.Size(48, 22)
        Me.TextBox99.TabIndex = 6
        Me.TextBox99.Text = ""
        Me.TextBox99.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox98
        '
        Me.TextBox98.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox98.Location = New System.Drawing.Point(96, 64)
        Me.TextBox98.MaxLength = 5
        Me.TextBox98.Name = "TextBox98"
        Me.TextBox98.Size = New System.Drawing.Size(48, 22)
        Me.TextBox98.TabIndex = 4
        Me.TextBox98.Text = ""
        Me.TextBox98.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox97
        '
        Me.TextBox97.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox97.Location = New System.Drawing.Point(96, 40)
        Me.TextBox97.MaxLength = 5
        Me.TextBox97.Name = "TextBox97"
        Me.TextBox97.Size = New System.Drawing.Size(48, 22)
        Me.TextBox97.TabIndex = 2
        Me.TextBox97.Text = ""
        Me.TextBox97.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox96
        '
        Me.TextBox96.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox96.Location = New System.Drawing.Point(96, 16)
        Me.TextBox96.MaxLength = 5
        Me.TextBox96.Name = "TextBox96"
        Me.TextBox96.Size = New System.Drawing.Size(48, 22)
        Me.TextBox96.TabIndex = 0
        Me.TextBox96.Text = ""
        Me.TextBox96.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox95
        '
        Me.TextBox95.BackColor = System.Drawing.Color.Aqua
        Me.TextBox95.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox95.Location = New System.Drawing.Point(8, 112)
        Me.TextBox95.Name = "TextBox95"
        Me.TextBox95.ReadOnly = True
        Me.TextBox95.Size = New System.Drawing.Size(88, 22)
        Me.TextBox95.TabIndex = 38
        Me.TextBox95.TabStop = False
        Me.TextBox95.Tag = "1"
        Me.TextBox95.Text = "BOLLE"
        '
        'TextBox93
        '
        Me.TextBox93.BackColor = System.Drawing.Color.Aqua
        Me.TextBox93.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox93.Location = New System.Drawing.Point(8, 88)
        Me.TextBox93.Name = "TextBox93"
        Me.TextBox93.ReadOnly = True
        Me.TextBox93.Size = New System.Drawing.Size(88, 22)
        Me.TextBox93.TabIndex = 37
        Me.TextBox93.TabStop = False
        Me.TextBox93.Tag = "1"
        Me.TextBox93.Text = "ABBUONI"
        '
        'TextBox92
        '
        Me.TextBox92.BackColor = System.Drawing.Color.Aqua
        Me.TextBox92.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox92.Location = New System.Drawing.Point(8, 64)
        Me.TextBox92.Name = "TextBox92"
        Me.TextBox92.ReadOnly = True
        Me.TextBox92.Size = New System.Drawing.Size(88, 22)
        Me.TextBox92.TabIndex = 36
        Me.TextBox92.TabStop = False
        Me.TextBox92.Tag = "1"
        Me.TextBox92.Text = "SC. CONDIZ."
        '
        'TextBox89
        '
        Me.TextBox89.BackColor = System.Drawing.Color.Aqua
        Me.TextBox89.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox89.Location = New System.Drawing.Point(8, 40)
        Me.TextBox89.Name = "TextBox89"
        Me.TextBox89.ReadOnly = True
        Me.TextBox89.Size = New System.Drawing.Size(88, 22)
        Me.TextBox89.TabIndex = 35
        Me.TextBox89.TabStop = False
        Me.TextBox89.Tag = "1"
        Me.TextBox89.Text = "SC. OMAGGI"
        '
        'TextBox88
        '
        Me.TextBox88.BackColor = System.Drawing.Color.Aqua
        Me.TextBox88.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox88.Location = New System.Drawing.Point(8, 16)
        Me.TextBox88.Name = "TextBox88"
        Me.TextBox88.ReadOnly = True
        Me.TextBox88.Size = New System.Drawing.Size(88, 22)
        Me.TextBox88.TabIndex = 34
        Me.TextBox88.TabStop = False
        Me.TextBox88.Tag = "1"
        Me.TextBox88.Text = "CASSA"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.Numbox2)
        Me.GroupBox7.Controls.Add(Me.Numbox1)
        Me.GroupBox7.Controls.Add(Me.TextBox211)
        Me.GroupBox7.Controls.Add(Me.TextBox210)
        Me.GroupBox7.Controls.Add(Me.TextBox209)
        Me.GroupBox7.Controls.Add(Me.TextBox203)
        Me.GroupBox7.Controls.Add(Me.TextBox100)
        Me.GroupBox7.Controls.Add(Me.Numbox5)
        Me.GroupBox7.Location = New System.Drawing.Point(10, 408)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(734, 80)
        Me.GroupBox7.TabIndex = 6
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "VARIE"
        '
        'Numbox2
        '
        Me.Numbox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox2.FormatInput = "#0.00"
        Me.Numbox2.FormatOutput = "#0.00"
        Me.Numbox2.Location = New System.Drawing.Point(600, 48)
        Me.Numbox2.MaxLength = 5
        Me.Numbox2.Name = "Numbox2"
        Me.Numbox2.Size = New System.Drawing.Size(120, 22)
        Me.Numbox2.TabIndex = 3
        Me.Numbox2.Text = ""
        Me.Numbox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Numbox1
        '
        Me.Numbox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox1.FormatInput = "#0.00"
        Me.Numbox1.FormatOutput = "#0.00"
        Me.Numbox1.Location = New System.Drawing.Point(600, 24)
        Me.Numbox1.MaxLength = 5
        Me.Numbox1.Name = "Numbox1"
        Me.Numbox1.Size = New System.Drawing.Size(120, 22)
        Me.Numbox1.TabIndex = 2
        Me.Numbox1.Text = ""
        Me.Numbox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox211
        '
        Me.TextBox211.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox211.Location = New System.Drawing.Point(224, 24)
        Me.TextBox211.MaxLength = 8
        Me.TextBox211.Name = "TextBox211"
        Me.TextBox211.Size = New System.Drawing.Size(115, 22)
        Me.TextBox211.TabIndex = 0
        Me.TextBox211.Text = ""
        '
        'TextBox210
        '
        Me.TextBox210.BackColor = System.Drawing.Color.Aqua
        Me.TextBox210.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox210.Location = New System.Drawing.Point(408, 48)
        Me.TextBox210.Name = "TextBox210"
        Me.TextBox210.ReadOnly = True
        Me.TextBox210.Size = New System.Drawing.Size(192, 22)
        Me.TextBox210.TabIndex = 29
        Me.TextBox210.TabStop = False
        Me.TextBox210.Tag = "1"
        Me.TextBox210.Text = "SCARTO MAX  SCONTO"
        '
        'TextBox209
        '
        Me.TextBox209.BackColor = System.Drawing.Color.Aqua
        Me.TextBox209.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox209.Location = New System.Drawing.Point(408, 24)
        Me.TextBox209.Name = "TextBox209"
        Me.TextBox209.ReadOnly = True
        Me.TextBox209.Size = New System.Drawing.Size(192, 22)
        Me.TextBox209.TabIndex = 28
        Me.TextBox209.TabStop = False
        Me.TextBox209.Tag = "1"
        Me.TextBox209.Text = "SCARTO MIN   SCONTO"
        '
        'TextBox203
        '
        Me.TextBox203.BackColor = System.Drawing.Color.Aqua
        Me.TextBox203.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox203.Location = New System.Drawing.Point(16, 24)
        Me.TextBox203.Name = "TextBox203"
        Me.TextBox203.ReadOnly = True
        Me.TextBox203.Size = New System.Drawing.Size(208, 22)
        Me.TextBox203.TabIndex = 3
        Me.TextBox203.TabStop = False
        Me.TextBox203.Tag = "1"
        Me.TextBox203.Text = " CONCESSIONE NESTLE'"
        '
        'TextBox100
        '
        Me.TextBox100.BackColor = System.Drawing.Color.Aqua
        Me.TextBox100.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox100.Location = New System.Drawing.Point(16, 48)
        Me.TextBox100.Name = "TextBox100"
        Me.TextBox100.ReadOnly = True
        Me.TextBox100.Size = New System.Drawing.Size(208, 22)
        Me.TextBox100.TabIndex = 26
        Me.TextBox100.TabStop = False
        Me.TextBox100.Tag = "1"
        Me.TextBox100.Text = "IMPONIBILE SPESE R.B. "
        '
        'Numbox5
        '
        Me.Numbox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox5.FormatInput = "##0.00"
        Me.Numbox5.FormatOutput = "##0.00"
        Me.Numbox5.Location = New System.Drawing.Point(224, 48)
        Me.Numbox5.MaxLength = 6
        Me.Numbox5.Name = "Numbox5"
        Me.Numbox5.Size = New System.Drawing.Size(115, 22)
        Me.Numbox5.TabIndex = 1
        Me.Numbox5.Text = ""
        Me.Numbox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox17
        '
        Me.GroupBox17.Controls.Add(Me.Label12)
        Me.GroupBox17.Controls.Add(Me.TextBox176)
        Me.GroupBox17.Location = New System.Drawing.Point(424, 336)
        Me.GroupBox17.Name = "GroupBox17"
        Me.GroupBox17.Size = New System.Drawing.Size(320, 72)
        Me.GroupBox17.TabIndex = 5
        Me.GroupBox17.TabStop = False
        Me.GroupBox17.Text = "CONTABILITA'"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(8, 16)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(192, 16)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "PATH PROGRAMMI"
        '
        'TextBox176
        '
        Me.TextBox176.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox176.Location = New System.Drawing.Point(8, 32)
        Me.TextBox176.MaxLength = 100
        Me.TextBox176.Name = "TextBox176"
        Me.TextBox176.Size = New System.Drawing.Size(296, 22)
        Me.TextBox176.TabIndex = 0
        Me.TextBox176.Text = ""
        '
        'GroupBox16
        '
        Me.GroupBox16.Controls.Add(Me.TextBox170)
        Me.GroupBox16.Controls.Add(Me.CheckBox1)
        Me.GroupBox16.Location = New System.Drawing.Point(10, 336)
        Me.GroupBox16.Name = "GroupBox16"
        Me.GroupBox16.Size = New System.Drawing.Size(406, 72)
        Me.GroupBox16.TabIndex = 4
        Me.GroupBox16.TabStop = False
        Me.GroupBox16.Text = "GESTIONE EFFETTI"
        '
        'TextBox170
        '
        Me.TextBox170.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox170.Location = New System.Drawing.Point(152, 32)
        Me.TextBox170.MaxLength = 100
        Me.TextBox170.Name = "TextBox170"
        Me.TextBox170.Size = New System.Drawing.Size(240, 22)
        Me.TextBox170.TabIndex = 1
        Me.TextBox170.Text = ""
        '
        'CheckBox1
        '
        Me.CheckBox1.Location = New System.Drawing.Point(8, 16)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(141, 37)
        Me.CheckBox1.TabIndex = 0
        Me.CheckBox1.Text = "Vengono gestite somme e riepiloghi"
        '
        'GroupBox11
        '
        Me.GroupBox11.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox11.Controls.Add(Me.Numbox3)
        Me.GroupBox11.Controls.Add(Me.TextBox138)
        Me.GroupBox11.Controls.Add(Me.TextBox139)
        Me.GroupBox11.Location = New System.Drawing.Point(10, 120)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(734, 48)
        Me.GroupBox11.TabIndex = 1
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "CODICI PAGAMENTO"
        '
        'Numbox3
        '
        Me.Numbox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox3.FormatInput = "##0"
        Me.Numbox3.FormatOutput = "000;0;0"
        Me.Numbox3.Location = New System.Drawing.Point(115, 16)
        Me.Numbox3.MaxLength = 3
        Me.Numbox3.Name = "Numbox3"
        Me.Numbox3.Size = New System.Drawing.Size(58, 22)
        Me.Numbox3.TabIndex = 0
        Me.Numbox3.Text = ""
        Me.Numbox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox138
        '
        Me.TextBox138.BackColor = System.Drawing.Color.Aqua
        Me.TextBox138.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox138.Location = New System.Drawing.Point(10, 16)
        Me.TextBox138.Name = "TextBox138"
        Me.TextBox138.Size = New System.Drawing.Size(105, 22)
        Me.TextBox138.TabIndex = 30
        Me.TextBox138.TabStop = False
        Me.TextBox138.Tag = "1"
        Me.TextBox138.Text = "PAGATO"
        '
        'TextBox139
        '
        Me.TextBox139.BackColor = System.Drawing.Color.White
        Me.TextBox139.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox139.Location = New System.Drawing.Point(173, 16)
        Me.TextBox139.MaxLength = 40
        Me.TextBox139.Name = "TextBox139"
        Me.TextBox139.ReadOnly = True
        Me.TextBox139.Size = New System.Drawing.Size(288, 22)
        Me.TextBox139.TabIndex = 29
        Me.TextBox139.TabStop = False
        Me.TextBox139.Text = ""
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.TextBox216)
        Me.GroupBox6.Controls.Add(Me.TextBox215)
        Me.GroupBox6.Controls.Add(Me.TextBox214)
        Me.GroupBox6.Controls.Add(Me.Numbox10)
        Me.GroupBox6.Controls.Add(Me.Numbox9)
        Me.GroupBox6.Controls.Add(Me.Numbox8)
        Me.GroupBox6.Controls.Add(Me.TextBox213)
        Me.GroupBox6.Controls.Add(Me.Numbox7)
        Me.GroupBox6.Controls.Add(Me.TextBox212)
        Me.GroupBox6.Controls.Add(Me.Numbox6)
        Me.GroupBox6.Controls.Add(Me.TextBox208)
        Me.GroupBox6.Controls.Add(Me.TextBox207)
        Me.GroupBox6.Controls.Add(Me.TextBox206)
        Me.GroupBox6.Controls.Add(Me.TextBox205)
        Me.GroupBox6.Controls.Add(Me.TextBox204)
        Me.GroupBox6.Controls.Add(Me.Numbox4)
        Me.GroupBox6.Controls.Add(Me.TextBox107)
        Me.GroupBox6.Controls.Add(Me.TextBox94)
        Me.GroupBox6.Location = New System.Drawing.Point(10, 168)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(334, 168)
        Me.GroupBox6.TabIndex = 2
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "CAUSALI  VARIE"
        '
        'TextBox216
        '
        Me.TextBox216.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox216.Location = New System.Drawing.Point(192, 136)
        Me.TextBox216.Name = "TextBox216"
        Me.TextBox216.Size = New System.Drawing.Size(128, 22)
        Me.TextBox216.TabIndex = 11
        Me.TextBox216.TabStop = False
        Me.TextBox216.Text = ""
        '
        'TextBox215
        '
        Me.TextBox215.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox215.Location = New System.Drawing.Point(192, 112)
        Me.TextBox215.Name = "TextBox215"
        Me.TextBox215.Size = New System.Drawing.Size(128, 22)
        Me.TextBox215.TabIndex = 9
        Me.TextBox215.TabStop = False
        Me.TextBox215.Text = ""
        '
        'TextBox214
        '
        Me.TextBox214.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox214.Location = New System.Drawing.Point(192, 88)
        Me.TextBox214.Name = "TextBox214"
        Me.TextBox214.Size = New System.Drawing.Size(128, 22)
        Me.TextBox214.TabIndex = 7
        Me.TextBox214.TabStop = False
        Me.TextBox214.Text = ""
        '
        'Numbox10
        '
        Me.Numbox10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox10.FormatInput = "##"
        Me.Numbox10.FormatOutput = "00;0;0"
        Me.Numbox10.Location = New System.Drawing.Point(160, 136)
        Me.Numbox10.MaxLength = 2
        Me.Numbox10.Name = "Numbox10"
        Me.Numbox10.Size = New System.Drawing.Size(32, 22)
        Me.Numbox10.TabIndex = 10
        Me.Numbox10.Text = ""
        Me.Numbox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Numbox9
        '
        Me.Numbox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox9.FormatInput = "##"
        Me.Numbox9.FormatOutput = "00;0;0"
        Me.Numbox9.Location = New System.Drawing.Point(160, 112)
        Me.Numbox9.MaxLength = 2
        Me.Numbox9.Name = "Numbox9"
        Me.Numbox9.Size = New System.Drawing.Size(32, 22)
        Me.Numbox9.TabIndex = 8
        Me.Numbox9.Text = ""
        Me.Numbox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Numbox8
        '
        Me.Numbox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox8.FormatInput = "##"
        Me.Numbox8.FormatOutput = "00;0;0"
        Me.Numbox8.Location = New System.Drawing.Point(160, 88)
        Me.Numbox8.MaxLength = 2
        Me.Numbox8.Name = "Numbox8"
        Me.Numbox8.Size = New System.Drawing.Size(32, 22)
        Me.Numbox8.TabIndex = 6
        Me.Numbox8.Text = ""
        Me.Numbox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox213
        '
        Me.TextBox213.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox213.Location = New System.Drawing.Point(192, 64)
        Me.TextBox213.Name = "TextBox213"
        Me.TextBox213.Size = New System.Drawing.Size(128, 22)
        Me.TextBox213.TabIndex = 5
        Me.TextBox213.TabStop = False
        Me.TextBox213.Text = ""
        '
        'Numbox7
        '
        Me.Numbox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox7.FormatInput = "##"
        Me.Numbox7.FormatOutput = "00;0;0"
        Me.Numbox7.Location = New System.Drawing.Point(160, 64)
        Me.Numbox7.MaxLength = 2
        Me.Numbox7.Name = "Numbox7"
        Me.Numbox7.Size = New System.Drawing.Size(32, 22)
        Me.Numbox7.TabIndex = 4
        Me.Numbox7.Text = ""
        Me.Numbox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox212
        '
        Me.TextBox212.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox212.Location = New System.Drawing.Point(192, 40)
        Me.TextBox212.Name = "TextBox212"
        Me.TextBox212.Size = New System.Drawing.Size(128, 22)
        Me.TextBox212.TabIndex = 3
        Me.TextBox212.TabStop = False
        Me.TextBox212.Text = ""
        '
        'Numbox6
        '
        Me.Numbox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox6.FormatInput = "##"
        Me.Numbox6.FormatOutput = "00;0;0"
        Me.Numbox6.Location = New System.Drawing.Point(160, 40)
        Me.Numbox6.MaxLength = 2
        Me.Numbox6.Name = "Numbox6"
        Me.Numbox6.Size = New System.Drawing.Size(32, 22)
        Me.Numbox6.TabIndex = 2
        Me.Numbox6.Text = ""
        Me.Numbox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox208
        '
        Me.TextBox208.BackColor = System.Drawing.Color.Aqua
        Me.TextBox208.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox208.Location = New System.Drawing.Point(10, 136)
        Me.TextBox208.Name = "TextBox208"
        Me.TextBox208.ReadOnly = True
        Me.TextBox208.Size = New System.Drawing.Size(150, 22)
        Me.TextBox208.TabIndex = 45
        Me.TextBox208.TabStop = False
        Me.TextBox208.Tag = "1"
        Me.TextBox208.Text = "GIROCONTO BOLL.-CLI"
        '
        'TextBox207
        '
        Me.TextBox207.BackColor = System.Drawing.Color.Aqua
        Me.TextBox207.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox207.Location = New System.Drawing.Point(10, 112)
        Me.TextBox207.Name = "TextBox207"
        Me.TextBox207.ReadOnly = True
        Me.TextBox207.Size = New System.Drawing.Size(150, 22)
        Me.TextBox207.TabIndex = 44
        Me.TextBox207.TabStop = False
        Me.TextBox207.Tag = "1"
        Me.TextBox207.Text = "INCASSO FATTURE"
        '
        'TextBox206
        '
        Me.TextBox206.BackColor = System.Drawing.Color.Aqua
        Me.TextBox206.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox206.Location = New System.Drawing.Point(10, 88)
        Me.TextBox206.Name = "TextBox206"
        Me.TextBox206.ReadOnly = True
        Me.TextBox206.Size = New System.Drawing.Size(150, 22)
        Me.TextBox206.TabIndex = 43
        Me.TextBox206.TabStop = False
        Me.TextBox206.Tag = "1"
        Me.TextBox206.Text = "TRASFERIMENTO R.B."
        '
        'TextBox205
        '
        Me.TextBox205.BackColor = System.Drawing.Color.Aqua
        Me.TextBox205.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox205.Location = New System.Drawing.Point(10, 64)
        Me.TextBox205.Name = "TextBox205"
        Me.TextBox205.ReadOnly = True
        Me.TextBox205.Size = New System.Drawing.Size(150, 22)
        Me.TextBox205.TabIndex = 42
        Me.TextBox205.TabStop = False
        Me.TextBox205.Tag = "1"
        Me.TextBox205.Text = "ABBUONI"
        '
        'TextBox204
        '
        Me.TextBox204.BackColor = System.Drawing.Color.Aqua
        Me.TextBox204.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox204.Location = New System.Drawing.Point(10, 40)
        Me.TextBox204.Name = "TextBox204"
        Me.TextBox204.ReadOnly = True
        Me.TextBox204.Size = New System.Drawing.Size(150, 22)
        Me.TextBox204.TabIndex = 41
        Me.TextBox204.TabStop = False
        Me.TextBox204.Tag = "1"
        Me.TextBox204.Text = "SCONTO"
        '
        'Numbox4
        '
        Me.Numbox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox4.FormatInput = "## "
        Me.Numbox4.FormatOutput = "00;0;0"
        Me.Numbox4.Location = New System.Drawing.Point(160, 16)
        Me.Numbox4.MaxLength = 2
        Me.Numbox4.Name = "Numbox4"
        Me.Numbox4.Size = New System.Drawing.Size(32, 22)
        Me.Numbox4.TabIndex = 0
        Me.Numbox4.Text = ""
        Me.Numbox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox107
        '
        Me.TextBox107.BackColor = System.Drawing.Color.Aqua
        Me.TextBox107.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox107.Location = New System.Drawing.Point(10, 16)
        Me.TextBox107.Name = "TextBox107"
        Me.TextBox107.ReadOnly = True
        Me.TextBox107.Size = New System.Drawing.Size(150, 22)
        Me.TextBox107.TabIndex = 33
        Me.TextBox107.TabStop = False
        Me.TextBox107.Tag = "1"
        Me.TextBox107.Text = "INCASSO BOLLE"
        '
        'TextBox94
        '
        Me.TextBox94.BackColor = System.Drawing.Color.White
        Me.TextBox94.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox94.Location = New System.Drawing.Point(192, 16)
        Me.TextBox94.Name = "TextBox94"
        Me.TextBox94.ReadOnly = True
        Me.TextBox94.Size = New System.Drawing.Size(128, 22)
        Me.TextBox94.TabIndex = 1
        Me.TextBox94.TabStop = False
        Me.TextBox94.Text = ""
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.TextBox109)
        Me.GroupBox5.Controls.Add(Me.TextBox108)
        Me.GroupBox5.Controls.Add(Me.TextBox87)
        Me.GroupBox5.Controls.Add(Me.TextBox86)
        Me.GroupBox5.Controls.Add(Me.TextBox85)
        Me.GroupBox5.Controls.Add(Me.TextBox1)
        Me.GroupBox5.Controls.Add(Me.TextBox91)
        Me.GroupBox5.Controls.Add(Me.TextBox90)
        Me.GroupBox5.Controls.Add(Me.Label11)
        Me.GroupBox5.Controls.Add(Me.Label10)
        Me.GroupBox5.Controls.Add(Me.TextBox80)
        Me.GroupBox5.Controls.Add(Me.TextBox81)
        Me.GroupBox5.Controls.Add(Me.TextBox82)
        Me.GroupBox5.Controls.Add(Me.TextBox83)
        Me.GroupBox5.Controls.Add(Me.TextBox84)
        Me.GroupBox5.Controls.Add(Me.TextBox79)
        Me.GroupBox5.Controls.Add(Me.TextBox78)
        Me.GroupBox5.Controls.Add(Me.TextBox77)
        Me.GroupBox5.Controls.Add(Me.TextBox76)
        Me.GroupBox5.Controls.Add(Me.TextBox75)
        Me.GroupBox5.Location = New System.Drawing.Point(10, 0)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(734, 120)
        Me.GroupBox5.TabIndex = 0
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "DEFAULT CODICI"
        '
        'TextBox109
        '
        Me.TextBox109.BackColor = System.Drawing.Color.White
        Me.TextBox109.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox109.Location = New System.Drawing.Point(488, 92)
        Me.TextBox109.Name = "TextBox109"
        Me.TextBox109.ReadOnly = True
        Me.TextBox109.Size = New System.Drawing.Size(232, 22)
        Me.TextBox109.TabIndex = 14
        Me.TextBox109.TabStop = False
        Me.TextBox109.Text = ""
        '
        'TextBox108
        '
        Me.TextBox108.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox108.Location = New System.Drawing.Point(440, 92)
        Me.TextBox108.MaxLength = 5
        Me.TextBox108.Name = "TextBox108"
        Me.TextBox108.Size = New System.Drawing.Size(48, 22)
        Me.TextBox108.TabIndex = 13
        Me.TextBox108.Text = ""
        Me.TextBox108.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox87
        '
        Me.TextBox87.BackColor = System.Drawing.Color.White
        Me.TextBox87.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox87.Location = New System.Drawing.Point(240, 92)
        Me.TextBox87.Name = "TextBox87"
        Me.TextBox87.ReadOnly = True
        Me.TextBox87.Size = New System.Drawing.Size(179, 22)
        Me.TextBox87.TabIndex = 12
        Me.TextBox87.TabStop = False
        Me.TextBox87.Text = ""
        '
        'TextBox86
        '
        Me.TextBox86.BackColor = System.Drawing.Color.White
        Me.TextBox86.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox86.Location = New System.Drawing.Point(192, 92)
        Me.TextBox86.Name = "TextBox86"
        Me.TextBox86.ReadOnly = True
        Me.TextBox86.Size = New System.Drawing.Size(48, 22)
        Me.TextBox86.TabIndex = 11
        Me.TextBox86.TabStop = False
        Me.TextBox86.Text = ""
        Me.TextBox86.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox85
        '
        Me.TextBox85.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox85.Location = New System.Drawing.Point(144, 92)
        Me.TextBox85.Name = "TextBox85"
        Me.TextBox85.Size = New System.Drawing.Size(48, 22)
        Me.TextBox85.TabIndex = 10
        Me.TextBox85.Text = ""
        Me.TextBox85.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.Aqua
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Location = New System.Drawing.Point(10, 92)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(134, 22)
        Me.TextBox1.TabIndex = 19
        Me.TextBox1.TabStop = False
        Me.TextBox1.Tag = "1"
        Me.TextBox1.Text = "OMAGGIO ESENTE"
        '
        'TextBox91
        '
        Me.TextBox91.BackColor = System.Drawing.Color.Aqua
        Me.TextBox91.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox91.Location = New System.Drawing.Point(10, 65)
        Me.TextBox91.Name = "TextBox91"
        Me.TextBox91.ReadOnly = True
        Me.TextBox91.Size = New System.Drawing.Size(134, 22)
        Me.TextBox91.TabIndex = 18
        Me.TextBox91.TabStop = False
        Me.TextBox91.Tag = "1"
        Me.TextBox91.Text = "SPESE R.B."
        '
        'TextBox90
        '
        Me.TextBox90.BackColor = System.Drawing.Color.Aqua
        Me.TextBox90.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox90.Location = New System.Drawing.Point(10, 37)
        Me.TextBox90.Name = "TextBox90"
        Me.TextBox90.ReadOnly = True
        Me.TextBox90.Size = New System.Drawing.Size(134, 22)
        Me.TextBox90.TabIndex = 17
        Me.TextBox90.TabStop = False
        Me.TextBox90.Tag = "1"
        Me.TextBox90.Text = "RICAVI"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(432, 18)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(134, 19)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "CONTROPARTITA"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(125, 18)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(115, 19)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "CODICE IVA"
        '
        'TextBox80
        '
        Me.TextBox80.BackColor = System.Drawing.Color.White
        Me.TextBox80.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox80.Location = New System.Drawing.Point(488, 65)
        Me.TextBox80.Name = "TextBox80"
        Me.TextBox80.ReadOnly = True
        Me.TextBox80.Size = New System.Drawing.Size(232, 22)
        Me.TextBox80.TabIndex = 9
        Me.TextBox80.TabStop = False
        Me.TextBox80.Text = ""
        '
        'TextBox81
        '
        Me.TextBox81.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox81.Location = New System.Drawing.Point(440, 65)
        Me.TextBox81.MaxLength = 5
        Me.TextBox81.Name = "TextBox81"
        Me.TextBox81.Size = New System.Drawing.Size(48, 22)
        Me.TextBox81.TabIndex = 8
        Me.TextBox81.Text = ""
        Me.TextBox81.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox82
        '
        Me.TextBox82.BackColor = System.Drawing.Color.White
        Me.TextBox82.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox82.Location = New System.Drawing.Point(240, 65)
        Me.TextBox82.Name = "TextBox82"
        Me.TextBox82.ReadOnly = True
        Me.TextBox82.Size = New System.Drawing.Size(179, 22)
        Me.TextBox82.TabIndex = 7
        Me.TextBox82.TabStop = False
        Me.TextBox82.Text = ""
        '
        'TextBox83
        '
        Me.TextBox83.BackColor = System.Drawing.Color.White
        Me.TextBox83.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox83.Location = New System.Drawing.Point(192, 65)
        Me.TextBox83.Name = "TextBox83"
        Me.TextBox83.ReadOnly = True
        Me.TextBox83.Size = New System.Drawing.Size(48, 22)
        Me.TextBox83.TabIndex = 6
        Me.TextBox83.TabStop = False
        Me.TextBox83.Text = ""
        Me.TextBox83.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox84
        '
        Me.TextBox84.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox84.Location = New System.Drawing.Point(144, 65)
        Me.TextBox84.Name = "TextBox84"
        Me.TextBox84.Size = New System.Drawing.Size(48, 22)
        Me.TextBox84.TabIndex = 5
        Me.TextBox84.Text = ""
        Me.TextBox84.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox79
        '
        Me.TextBox79.BackColor = System.Drawing.Color.White
        Me.TextBox79.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox79.Location = New System.Drawing.Point(488, 37)
        Me.TextBox79.Name = "TextBox79"
        Me.TextBox79.ReadOnly = True
        Me.TextBox79.Size = New System.Drawing.Size(232, 22)
        Me.TextBox79.TabIndex = 4
        Me.TextBox79.TabStop = False
        Me.TextBox79.Text = ""
        '
        'TextBox78
        '
        Me.TextBox78.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox78.Location = New System.Drawing.Point(440, 37)
        Me.TextBox78.MaxLength = 5
        Me.TextBox78.Name = "TextBox78"
        Me.TextBox78.Size = New System.Drawing.Size(48, 22)
        Me.TextBox78.TabIndex = 3
        Me.TextBox78.Text = ""
        Me.TextBox78.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox77
        '
        Me.TextBox77.BackColor = System.Drawing.Color.White
        Me.TextBox77.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox77.Location = New System.Drawing.Point(240, 37)
        Me.TextBox77.Name = "TextBox77"
        Me.TextBox77.ReadOnly = True
        Me.TextBox77.Size = New System.Drawing.Size(179, 22)
        Me.TextBox77.TabIndex = 2
        Me.TextBox77.TabStop = False
        Me.TextBox77.Text = ""
        '
        'TextBox76
        '
        Me.TextBox76.BackColor = System.Drawing.Color.White
        Me.TextBox76.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox76.Location = New System.Drawing.Point(192, 37)
        Me.TextBox76.Name = "TextBox76"
        Me.TextBox76.ReadOnly = True
        Me.TextBox76.Size = New System.Drawing.Size(48, 22)
        Me.TextBox76.TabIndex = 1
        Me.TextBox76.TabStop = False
        Me.TextBox76.Text = ""
        Me.TextBox76.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox75
        '
        Me.TextBox75.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox75.Location = New System.Drawing.Point(144, 37)
        Me.TextBox75.Name = "TextBox75"
        Me.TextBox75.Size = New System.Drawing.Size(48, 22)
        Me.TextBox75.TabIndex = 0
        Me.TextBox75.Text = ""
        Me.TextBox75.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.GroupBox10)
        Me.TabPage3.Controls.Add(Me.GroupBox9)
        Me.TabPage3.Controls.Add(Me.GroupBox8)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(755, 499)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Personalizza Gestione 2"
        Me.TabPage3.Visible = False
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.TextBox06)
        Me.GroupBox10.Controls.Add(Me.TextBox05)
        Me.GroupBox10.Controls.Add(Me.TextBox117)
        Me.GroupBox10.Controls.Add(Me.TextBox118)
        Me.GroupBox10.Location = New System.Drawing.Point(8, 96)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(304, 80)
        Me.GroupBox10.TabIndex = 9
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "LIVE UPDATE"
        '
        'TextBox06
        '
        Me.TextBox06.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox06.Location = New System.Drawing.Point(128, 48)
        Me.TextBox06.MaxLength = 50
        Me.TextBox06.Name = "TextBox06"
        Me.TextBox06.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.TextBox06.Size = New System.Drawing.Size(168, 22)
        Me.TextBox06.TabIndex = 27
        Me.TextBox06.Text = ""
        '
        'TextBox05
        '
        Me.TextBox05.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox05.Location = New System.Drawing.Point(128, 24)
        Me.TextBox05.MaxLength = 50
        Me.TextBox05.Name = "TextBox05"
        Me.TextBox05.Size = New System.Drawing.Size(168, 22)
        Me.TextBox05.TabIndex = 0
        Me.TextBox05.Text = ""
        '
        'TextBox117
        '
        Me.TextBox117.BackColor = System.Drawing.Color.Aqua
        Me.TextBox117.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox117.Location = New System.Drawing.Point(16, 24)
        Me.TextBox117.Name = "TextBox117"
        Me.TextBox117.ReadOnly = True
        Me.TextBox117.Size = New System.Drawing.Size(112, 22)
        Me.TextBox117.TabIndex = 3
        Me.TextBox117.TabStop = False
        Me.TextBox117.Text = "UTENTE"
        '
        'TextBox118
        '
        Me.TextBox118.BackColor = System.Drawing.Color.Aqua
        Me.TextBox118.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox118.Location = New System.Drawing.Point(16, 48)
        Me.TextBox118.Name = "TextBox118"
        Me.TextBox118.ReadOnly = True
        Me.TextBox118.Size = New System.Drawing.Size(112, 22)
        Me.TextBox118.TabIndex = 26
        Me.TextBox118.TabStop = False
        Me.TextBox118.Text = "PASSWORD"
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.Numbox12)
        Me.GroupBox9.Controls.Add(Me.TextBox119)
        Me.GroupBox9.Controls.Add(Me.Numbox11)
        Me.GroupBox9.Controls.Add(Me.TextBox116)
        Me.GroupBox9.Controls.Add(Me.TextBox110)
        Me.GroupBox9.Controls.Add(Me.TextBox111)
        Me.GroupBox9.Controls.Add(Me.TextBox04)
        Me.GroupBox9.Controls.Add(Me.TextBox03)
        Me.GroupBox9.Controls.Add(Me.TextBox112)
        Me.GroupBox9.Controls.Add(Me.TextBox115)
        Me.GroupBox9.Location = New System.Drawing.Point(320, 8)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(424, 168)
        Me.GroupBox9.TabIndex = 8
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "TERMINALINI E TRASFERIMENTI NESTLE'"
        '
        'Numbox12
        '
        Me.Numbox12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox12.FormatInput = "##"
        Me.Numbox12.FormatOutput = "#0"
        Me.Numbox12.Location = New System.Drawing.Point(376, 136)
        Me.Numbox12.MaxLength = 2
        Me.Numbox12.Name = "Numbox12"
        Me.Numbox12.Size = New System.Drawing.Size(40, 22)
        Me.Numbox12.TabIndex = 4
        Me.Numbox12.Text = ""
        Me.Numbox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox119
        '
        Me.TextBox119.BackColor = System.Drawing.Color.Aqua
        Me.TextBox119.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox119.Location = New System.Drawing.Point(224, 136)
        Me.TextBox119.Name = "TextBox119"
        Me.TextBox119.ReadOnly = True
        Me.TextBox119.Size = New System.Drawing.Size(152, 22)
        Me.TextBox119.TabIndex = 32
        Me.TextBox119.TabStop = False
        Me.TextBox119.Tag = "1"
        Me.TextBox119.Text = "CP. D.D.T. RIEPILOGO"
        '
        'Numbox11
        '
        Me.Numbox11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox11.FormatInput = "##"
        Me.Numbox11.FormatOutput = "#0"
        Me.Numbox11.Location = New System.Drawing.Point(160, 136)
        Me.Numbox11.MaxLength = 2
        Me.Numbox11.Name = "Numbox11"
        Me.Numbox11.Size = New System.Drawing.Size(40, 22)
        Me.Numbox11.TabIndex = 3
        Me.Numbox11.Text = ""
        Me.Numbox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox116
        '
        Me.TextBox116.BackColor = System.Drawing.Color.Aqua
        Me.TextBox116.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox116.Location = New System.Drawing.Point(8, 136)
        Me.TextBox116.Name = "TextBox116"
        Me.TextBox116.ReadOnly = True
        Me.TextBox116.Size = New System.Drawing.Size(152, 22)
        Me.TextBox116.TabIndex = 30
        Me.TextBox116.TabStop = False
        Me.TextBox116.Tag = "1"
        Me.TextBox116.Text = "CP. D.D.T. CONSEGNA"
        '
        'TextBox110
        '
        Me.TextBox110.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox110.Location = New System.Drawing.Point(120, 96)
        Me.TextBox110.MaxLength = 50
        Me.TextBox110.Name = "TextBox110"
        Me.TextBox110.Size = New System.Drawing.Size(296, 22)
        Me.TextBox110.TabIndex = 2
        Me.TextBox110.Text = ""
        '
        'TextBox111
        '
        Me.TextBox111.BackColor = System.Drawing.Color.Aqua
        Me.TextBox111.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox111.Location = New System.Drawing.Point(8, 96)
        Me.TextBox111.Name = "TextBox111"
        Me.TextBox111.ReadOnly = True
        Me.TextBox111.Size = New System.Drawing.Size(112, 22)
        Me.TextBox111.TabIndex = 28
        Me.TextBox111.TabStop = False
        Me.TextBox111.Tag = "1"
        Me.TextBox111.Text = "PATH NESTLE'"
        '
        'TextBox04
        '
        Me.TextBox04.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox04.Location = New System.Drawing.Point(120, 48)
        Me.TextBox04.MaxLength = 50
        Me.TextBox04.Name = "TextBox04"
        Me.TextBox04.Size = New System.Drawing.Size(296, 22)
        Me.TextBox04.TabIndex = 1
        Me.TextBox04.Text = ""
        '
        'TextBox03
        '
        Me.TextBox03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox03.Location = New System.Drawing.Point(120, 24)
        Me.TextBox03.MaxLength = 50
        Me.TextBox03.Name = "TextBox03"
        Me.TextBox03.Size = New System.Drawing.Size(296, 22)
        Me.TextBox03.TabIndex = 0
        Me.TextBox03.Text = ""
        '
        'TextBox112
        '
        Me.TextBox112.BackColor = System.Drawing.Color.Aqua
        Me.TextBox112.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox112.Location = New System.Drawing.Point(8, 24)
        Me.TextBox112.Name = "TextBox112"
        Me.TextBox112.ReadOnly = True
        Me.TextBox112.Size = New System.Drawing.Size(112, 22)
        Me.TextBox112.TabIndex = 3
        Me.TextBox112.TabStop = False
        Me.TextBox112.Tag = "1"
        Me.TextBox112.Text = "PATH CARICO"
        '
        'TextBox115
        '
        Me.TextBox115.BackColor = System.Drawing.Color.Aqua
        Me.TextBox115.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox115.Location = New System.Drawing.Point(8, 48)
        Me.TextBox115.Name = "TextBox115"
        Me.TextBox115.ReadOnly = True
        Me.TextBox115.Size = New System.Drawing.Size(112, 22)
        Me.TextBox115.TabIndex = 26
        Me.TextBox115.TabStop = False
        Me.TextBox115.Tag = "1"
        Me.TextBox115.Text = "PATH SCARICO"
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.Numbox01)
        Me.GroupBox8.Controls.Add(Me.TextBox113)
        Me.GroupBox8.Controls.Add(Me.TextBox114)
        Me.GroupBox8.Controls.Add(Me.Numbox02)
        Me.GroupBox8.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(304, 80)
        Me.GroupBox8.TabIndex = 7
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "INVENTARIO"
        '
        'Numbox01
        '
        Me.Numbox01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox01.FormatInput = "####"
        Me.Numbox01.FormatOutput = "###0"
        Me.Numbox01.Location = New System.Drawing.Point(224, 24)
        Me.Numbox01.MaxLength = 4
        Me.Numbox01.Name = "Numbox01"
        Me.Numbox01.Size = New System.Drawing.Size(64, 22)
        Me.Numbox01.TabIndex = 27
        Me.Numbox01.Text = ""
        Me.Numbox01.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox113
        '
        Me.TextBox113.BackColor = System.Drawing.Color.Aqua
        Me.TextBox113.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox113.Location = New System.Drawing.Point(16, 24)
        Me.TextBox113.Name = "TextBox113"
        Me.TextBox113.ReadOnly = True
        Me.TextBox113.Size = New System.Drawing.Size(208, 22)
        Me.TextBox113.TabIndex = 3
        Me.TextBox113.TabStop = False
        Me.TextBox113.Tag = "1"
        Me.TextBox113.Text = "ULTIMO ANNO INVENTARIO"
        '
        'TextBox114
        '
        Me.TextBox114.BackColor = System.Drawing.Color.Aqua
        Me.TextBox114.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox114.Location = New System.Drawing.Point(16, 48)
        Me.TextBox114.Name = "TextBox114"
        Me.TextBox114.ReadOnly = True
        Me.TextBox114.Size = New System.Drawing.Size(208, 22)
        Me.TextBox114.TabIndex = 26
        Me.TextBox114.TabStop = False
        Me.TextBox114.Tag = "1"
        Me.TextBox114.Text = "RIFERIMENTO INVENTARIO"
        '
        'Numbox02
        '
        Me.Numbox02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Numbox02.FormatInput = "#####"
        Me.Numbox02.FormatOutput = "####0"
        Me.Numbox02.Location = New System.Drawing.Point(224, 48)
        Me.Numbox02.MaxLength = 5
        Me.Numbox02.Name = "Numbox02"
        Me.Numbox02.Size = New System.Drawing.Size(64, 22)
        Me.Numbox02.TabIndex = 1
        Me.Numbox02.Text = ""
        Me.Numbox02.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'RSIndici
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(1012, 656)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.GroupBox14)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "RSIndici"
        Me.Text = "RSIndici"
        Me.Controls.SetChildIndex(Me.GroupBox2, 0)
        Me.Controls.SetChildIndex(Me.GroupBox14, 0)
        Me.Controls.SetChildIndex(Me.TabControl1, 0)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox14.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox17.ResumeLayout(False)
        Me.GroupBox16.ResumeLayout(False)
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private Sub inizializza()
        TabControl1.SelectedTab = TabPage1
        TabPage1.Visible = True
    End Sub
    Private Sub RsIndici_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, ButtonF5.Click
        inizializza()
        numera3(GroupBox3)
        numera4(GroupBox4)
        numera5(GroupBox5)
        NTai = leggiTai(1)
        output_Campi()
        TextBox4.Focus()
    End Sub
    Private Sub output_Campi()
        TextBox2.Text = TaiRw("TaiAnno")

        Numbox5.Text = CDec(TaiRw("TaiSpeseRb")).ToString(Numbox5.FormatOutput)
        TextBox176.Text = TaiRw("TaiPathXeur")
        CheckBox1.Checked = CBool(TaiRw("TaiEffRiep"))
        TextBox170.Text = TaiRw("TaiPathEffetti")
        Numbox8.Text = CInt(TaiRw("TaiCauEff")).ToString(Numbox8.FormatOutput)

        Numbox4.Text = CInt(TaiRw("TaiCauInc")).ToString(Numbox4.FormatOutput)
        TextBox96.Text = TaiRw("TaiCptCassa")

        'TextBox97.Text = TaiRw("TaiCptSconOm")
        'TextBox98.Text = TaiRw("TaiCptSconCond")
        'TextBox99.Text = TaiRw("TaiCptAbb")
        'TextBox101.Text = TaiRw("TaiCptBolle")
        'TextBox2.Text = TaiRw("TaiAnno")
        'TextBox211.Text = TaiRw("TaiConcNestle")

        TextBox97.Text = ""
        TextBox98.Text = ""
        TextBox99.Text = ""
        TextBox101.Text = ""
        TextBox211.Text = ""



        'Numbox1.Text = CDec(TaiRw("TaiScartoMin")).ToString(Numbox1.FormatOutput)
        'Numbox2.Text = CDec(TaiRw("TaiScartoMax")).ToString(Numbox2.FormatOutput)
        'Numbox3.Text = CInt(TaiRw("TaiPagato")).ToString(Numbox3.FormatOutput)
        'Numbox6.Text = CInt(TaiRw("TaiCauSconti")).ToString(Numbox6.FormatOutput)
        'Numbox7.Text = CInt(TaiRw("TaiCauAbb")).ToString(Numbox7.FormatOutput)

        'Numbox9.Text = CInt(TaiRw("TaiCauFatt")).ToString(Numbox9.FormatOutput)
        'Numbox10.Text = CInt(TaiRw("TaiCauGiroc")).ToString(Numbox10.FormatOutput)
        'Numbox11.Text = TaiRw("TaiCopieBol")
        'Numbox12.Text = TaiRw("TaiCopieBolR")


        Numbox1.Text = 0
        Numbox2.Text = 0
        Numbox3.Text = 0
        Numbox6.Text = 0
        Numbox7.Text = 0

        Numbox9.Text = 0
        Numbox10.Text = 0
        Numbox11.Text = 0
        Numbox12.Text = 0


        output_documenti()
        output_pi()
        output_civ()
        output_cpt()
        output_cptvarie()
        output_causali()
        output_Pagato()
        output_inv()
        output_liveupdate()
        output_terminalini()
    End Sub

    Private Sub output_documenti()
        Dim i, y, k, z, w As Integer
        For i = 0 To 11
            y = i + 66
            CType(ArDes(i), TextBox).Text = TaiRw(y)
            k = i + 2
            CType(ArDoc(i), TextBox).Text = TaiRw(k)
            z = 34 + i
            CType(ArReg(i), TextBox).Text = CInt(TaiRw(z)).ToString("##")
        Next
    End Sub
    Private Sub output_pi()
        Dim i, y, k As Integer
        For i = 0 To 11
            y = i + 54
            CType(ArPi(i), TextBox).Text = CInt(TaiRw(y)).ToString("##")
            CType(ArDescPi(i), TextBox).Text = leggi_cii(TaiRw(y))
        Next
    End Sub

    Private Sub output_civ()
        Dim i, y, z As Integer
        For i = 0 To 2
            y = i + 22
            z = CInt(TaiRw(y))
            CType(ArCiv1(i), TextBox).Text = z.ToString("##")
            If z > 0 Then
                CType(ArCiv2(i), TextBox).Text = CType(ArPi(z - 1), TextBox).Text
                CType(ArCiv3(i), TextBox).Text = CType(ArDescPi(z - 1), TextBox).Text
            End If
        Next
    End Sub
    Private Sub output_cpt()
        Dim i, y, z As Integer
        For i = 1 To 3
            CType(ArCpt1(i - 1), TextBox).Text = (TaiRw("TaiCpt" & i))
            CType(ArCpt2(i - 1), TextBox).Text = leggi_Pia(CType(ArCpt1(i - 1), TextBox).Text)
        Next
    End Sub
    Private Sub output_civdef()
        Dim i As Integer
        i = Val(TextBox75.Text)
        If i > 0 Then
            TextBox76.Text = (TaiRw("TaiPi" & i))
            TextBox77.Text = leggi_cii(TaiRw("TaiPi" & i))
        End If
    End Sub
    Private Sub output_cptvarie()
        TextBox102.Text = leggi_Pia(TextBox96.Text)
        TextBox103.Text = leggi_Pia(TextBox97.Text)
        TextBox104.Text = leggi_Pia(TextBox98.Text)
        TextBox105.Text = leggi_Pia(TextBox99.Text)
        TextBox106.Text = leggi_Pia(TextBox101.Text)
    End Sub
    Private Sub output_causali()
        TextBox94.Text = leggi_cau(CInt(Numbox4.Text))
        TextBox212.Text = leggi_cau(CInt(Numbox6.Text))
        TextBox213.Text = leggi_cau(CInt(Numbox7.Text))
        TextBox214.Text = leggi_cau(CInt(Numbox8.Text))
        TextBox215.Text = leggi_cau(CInt(Numbox9.Text))
        TextBox216.Text = leggi_cau(CInt(Numbox10.Text))
    End Sub
    Private Sub output_Pagato()
        TextBox139.Text = leggi_pagam(CInt(Val(Numbox3.Text)))
    End Sub

    Private Sub output_inv()
        'Numbox01.Text = Format(TaiRw("TaiUltInv"), Numbox01.FormatOutput)
        'Numbox02.Text = Format(TaiRw("TaiInvRifer"), Numbox02.FormatOutput)
    End Sub

    Private Sub output_liveupdate()
        TextBox05.Text = TaiRw("TaiAggUtente").ToString
        TextBox06.Text = TaiRw("TaiAggPassword").ToString
    End Sub

    Private Sub output_terminalini()
        'TextBox03.Text = TaiRw("TaiPathCarico").ToString
        'TextBox04.Text = TaiRw("TaiPathScarico").ToString
        'TextBox110.Text = TaiRw("TaiPathNestle").ToString
    End Sub

    Private Sub aggiorna_campi()
        TaiRw("TaiAnno") = CInt(TextBox2.Text)
        TaiRw("TaiSpeseRb") = CDec(Numbox5.Text)
        TaiRw("TaiPathXeur") = TextBox176.Text
        TaiRw("TaiEffRiep") = CheckBox1.Checked
        TaiRw("TaiPathEffetti") = TextBox170.Text
        TaiRw("TaiCauEff") = CInt(Numbox8.Text)

        TaiRw("TaiCauInc") = CInt(Numbox4.Text)
        TaiRw("TaiCptCassa") = TextBox96.Text
        TaiRw("TaiCiv1") = 0
        TaiRw("TaiCiv2") = 0
        TaiRw("TaiCpt1") = "80.01"
        TaiRw("TaiCpt2") = "00.00"

        'TaiRw("TaiCiv1") = CInt(TextBox75.Text)
        'TaiRw("TaiCiv2") = CInt(TextBox84.Text)
        'TaiRw("TaiCpt1") = TextBox78.Text
        'TaiRw("TaiCpt2") = TextBox81.Text
        'TaiRw("TaiCauSconti") = CInt(Numbox6.Text)
        'TaiRw("TaiCauAbb") = CInt(Numbox7.Text)
        'TaiRw("TaiCauGiroc") = CInt(Numbox10.Text)
        'TaiRw("TaiConcNestle") = TextBox211.Text
        'TaiRw("TaiCauFatt") = CInt(Numbox9.Text)
        'TaiRw("TaiScartoMin") = CDec(Numbox1.Text)
        'TaiRw("TaiScartoMax") = CDec(Numbox2.Text)
        'TaiRw("TaiPagato") = CDec(Val(Numbox3.Text))
        'TaiRw("TaiCivOme") = CInt(TextBox85.Text)
        'TaiRw("TaiCptSconOm") = TextBox97.Text
        'TaiRw("TaiCptSconCond") = TextBox98.Text
        'TaiRw("TaiCptAbb") = TextBox99.Text
        'TaiRw("TaiCptBolle") = TextBox101.Text
        'TaiRw("TaiCopieBol") = Val(Numbox11.Text)
        'TaiRw("TaiCopieBolR") = Val(Numbox12.Text)

        aggiorna_documenti()
        aggiorna_pi()
        aggiorna_civ()
        aggiorna_cpt()
        aggiorna_inv()
        aggiorna_liveupdate()
        aggiorna_terminalini()
    End Sub
    Private Sub aggiorna_documenti()
        Dim i, k, z As Int16
        For i = 0 To 11
            k = i + 2
            TaiRw(k) = CInt(CType(ArDoc(i), TextBox).Text)
            z = 34 + i
            If CType(ArReg(i), TextBox).Text = "" Then
                CType(ArReg(i), TextBox).Text = "0"
            End If
            TaiRw(z) = CInt(CType(ArReg(i), TextBox).Text)
        Next
    End Sub
    Private Sub aggiorna_pi()
        Dim i, y As Int16
        For i = 0 To 11
            y = i + 54
            If CType(ArPi(i), TextBox).Text = "" Then
                CType(ArPi(i), TextBox).Text = "0"
            End If
            TaiRw(y) = CInt(CType(ArPi(i), TextBox).Text)
        Next
    End Sub
    Private Sub aggiorna_civ()
        Dim i, y As Integer
        For i = 0 To 2
            y = i + 22
            If CType(ArCiv1(i), TextBox).Text = "" Then
                CType(ArCiv1(i), TextBox).Text = "0"
            End If
            TaiRw(y) = CInt(CType(ArCiv1(i), TextBox).Text)
        Next
    End Sub

    Private Sub aggiorna_cpt()
        Dim i, y As Integer
        For i = 1 To 3
            TaiRw("TaiCpt" & i) = CType(ArCpt1(i - 1), TextBox).Text
        Next
    End Sub

    Private Sub aggiorna_inv()
        'TaiRw("TaiUltInv") = Numbox01.Text
        'TaiRw("TaiInvRifer") = Numbox02.Text
    End Sub

    Private Sub aggiorna_liveupdate()
        TaiRw("TaiAggUtente") = TextBox05.Text
        TaiRw("TaiAggPassword") = TextBox06.Text
    End Sub

    Private Sub aggiorna_terminalini()
        'TaiRw("TaiPathCarico") = TextBox03.Text
        'TaiRw("TaiPathScarico") = TextBox04.Text
        'TaiRw("TaiPathNestle") = TextBox110.Text
    End Sub

    Private Function leggiTai(ByVal codice As Int16) As Integer
        Dim archivio As String = "TbTai"
        Dim strselect As String = "select  top 1 * from TbTai order by TaiAnno Desc "
        TaiDs = New DataSet
        TaiAd = New SqlDataAdapter(strselect, cnDb)
        TaiAd.Fill(TaiDs, archivio)
        TaiBl = New SqlCommandBuilder(TaiAd)
        If TaiDs.Tables(archivio).Rows.Count > 0 Then
            TaiRw = TaiDs.Tables(archivio).Rows(0)
        Else
            initTai(archivio)
        End If
        Return TaiDs.Tables(archivio).Rows.Count
    End Function
    Private Sub initTai(ByVal archivio As String)
        TaiRw = TaiDs.Tables(archivio).NewRow
        Dim i As Int16
        TaiRw("TaiAnno") = Today.Year
        For i = 2 To 21
            TaiRw(i) = CInt(0)
        Next
        For i = 22 To 27
            TaiRw(i) = CInt(0)
        Next
        For i = 28 To 33
            TaiRw(i) = " "
        Next
        For i = 34 To 53
            TaiRw(i) = CInt(0)
        Next
        For i = 54 To 65
            TaiRw(i) = CInt(0)
        Next

        TaiRw("TaiDes1") = "Disponibile"
        TaiRw("TaiDes2") = "Disponibile"
        TaiRw("TaiDes3") = "Disponibile"
        TaiRw("TaiDes4") = "Disponibile"
        TaiRw("TaiDes5") = "Disponibile"
        TaiRw("TaiDes6") = "Disponibile"
        TaiRw("TaiDes7") = "Disponibile"
        TaiRw("TaiDes8") = "Disponibile"
        TaiRw("TaiDes9") = "Disponibile"
        TaiRw("TaiDes10") = "Fattura  "
        TaiRw("TaiDes11") = "Ricevute "
        TaiRw("TaiDes12") = "Fattura Libera"
        TaiRw("TaiDes13") = "Disponibile"
        TaiRw("TaiDes14") = "Disponibile"
        TaiRw("TaiDes15") = "Disponibile"
        TaiRw("TaiDes16") = "Disponibile"
        TaiRw("TaiDes17") = "Disponibile"
        TaiRw("TaiDes18") = "Disponibile"
        TaiRw("TaiDes19") = "Disponibile"
        TaiRw("TaiDes20") = "Disponibile"

        'TaiRw("TaiConcNestle") = 0

        TaiRw("TaiSpeseRb") = 0
        TaiRw("TaiPathXeur") = ""
        TaiRw("TaiPathEffetti") = ""
        TaiRw("TaiEffRiep") = CBool(0)
        TaiRw("TaiCauInc") = 0
        TaiRw("TaiCptCassa") = ""
        'TaiRw("TaiCptSconOm") = ""
        'TaiRw("TaiCauSconti") = 0
        'TaiRw("TaiCptSconCond") = ""
        'TaiRw("TaiScartoMin") = 0
        'TaiRw("TaiScartoMax") = 0
        'TaiRw("TaiCauAbb") = 0
        'TaiRw("TaiCptAbb") = ""
        TaiRw("TaiCauEff") = 0
        'TaiRw("TaiPagato") = 0
        'TaiRw("TaiCptBolle") = ""
        'TaiRw("TaiCauFatt") = 0
        'TaiRw("TaiCauGiroc") = 0
    End Sub
    Private Function leggi_cii(ByVal codice As Int16) As String
        leggi_cii = ""
        Dim strselect As String = "select Ciides from TbCii where CiiCod = " & codice
        Cmd = New SqlCommand(strselect, cnCo)
        dataRd = Cmd.ExecuteReader

        If dataRd.Read Then
            leggi_cii = dataRd.GetString(0)
        End If
        dataRd.Close()
    End Function

    Private Function leggi_cau(ByVal codice As Int16) As String
        leggi_cau = ""
        Dim strselect As String = "select CiiCau from TbCii where CiiCod = " & codice
        Cmd = New SqlCommand(strselect, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            leggi_cau = dataRd.GetString(0)
        End If
        dataRd.Close()
    End Function
    Private Function leggi_pagam(ByVal codice As Int16) As String
        leggi_pagam = ""
        Dim strselect As String = "select PagDesc from TbPag where PagCod = " & codice
        Cmd = New SqlCommand(strselect, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            leggi_pagam = dataRd.GetString(0)
        End If
        dataRd.Close()
    End Function
    Private Function leggi_Pia(ByVal codice As String) As String
        leggi_Pia = ""
        Dim strselect As String = "select PiaAnaCo from TbPia where PiaCodCo = '" & codice & "'"
        Cmd = New SqlCommand(strselect, cnCo)
        dataRd = Cmd.ExecuteReader

        If dataRd.Read Then
            leggi_Pia = dataRd.GetString(0)
        End If
        dataRd.Close()
    End Function
    Private Function aggiorna() As Integer
        aggiorna = 0
        aggiorna_campi()
        scriviTai("TbTai")
    End Function
    Private Sub scriviTai(ByVal archivio As String)
        If NTai = 0 Then
            TaiDs.Tables(archivio).Rows.Add(TaiRw)
        End If
        If TaiAd.Update(TaiDs, archivio) = -1 Then
            Exit Sub
        End If
        TaiDs.AcceptChanges()
    End Sub
    Private Sub numera3(ByVal gb As GroupBox)
        Dim tb As Object
        Dim i, k, l, m As Integer
        For i = 0 To 11
            k = i * 3
            l = k + 1
            m = k + 2
            For Each tb In gb.Controls
                If TypeOf (tb) Is TextBox Then
                    If tb.TabIndex = k Then
                        ArDes.Add(tb)
                    ElseIf tb.tabindex = l Then
                        ArDoc.Add(tb)
                        AddHandler CType(tb, TextBox).Validating, AddressOf doc_Validating
                    ElseIf tb.tabindex = m Then
                        ArReg.Add(tb)
                        AddHandler CType(tb, TextBox).Validating, AddressOf reg_Validating
                    End If
                End If
            Next
        Next
    End Sub
    Private Sub numera4(ByVal gb As GroupBox)
        Dim tb As Object
        Dim i, k, l, m As Integer
        For i = 0 To 11
            k = i * 3
            l = k + 1
            m = k + 2
            For Each tb In gb.Controls
                If TypeOf (tb) Is TextBox Then
                    If tb.TabIndex = k Then
                        tb.text = i + 1
                    ElseIf tb.tabindex = l Then
                        ArPi.Add(tb)
                        AddHandler CType(tb, TextBox).Validating, AddressOf Pi_Validating
                        AddHandler CType(tb, TextBox).Validated, AddressOf Pi_Validated
                    ElseIf tb.tabindex = m Then
                        ArDescPi.Add(tb)
                    End If
                End If
            Next
        Next
    End Sub
    Private Sub numera5(ByVal gb As GroupBox)
        Dim tb As Object
        Dim i, k, l, m, n, o As Integer
        For i = 0 To 2
            k = i * 5
            l = k + 1
            m = k + 2
            n = k + 3
            o = k + 4
            For Each tb In gb.Controls
                If TypeOf (tb) Is TextBox Then
                    If tb.TabIndex = k Then
                        ArCiv1.Add(tb)
                        AddHandler CType(tb, TextBox).Validating, AddressOf Civ1_Validating
                        AddHandler CType(tb, TextBox).Validated, AddressOf Civ1_Validated
                    ElseIf tb.tabindex = l Then
                        ArCiv2.Add(tb)
                    ElseIf tb.tabindex = m Then
                        ArCiv3.Add(tb)
                    ElseIf tb.tabindex = n Then
                        ArCpt1.Add(tb)
                        AddHandler CType(tb, TextBox).Validating, AddressOf Cpt1_Validating
                        AddHandler CType(tb, TextBox).Validated, AddressOf Cpt1_Validated
                    ElseIf tb.tabindex = o Then
                        ArCpt2.Add(tb)
                    End If
                End If
            Next
        Next
    End Sub
    Private Sub doc_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If Not IsNumeric(CType(sender, TextBox).Text) Or Val(CType(sender, TextBox).Text) > 999999 Then
            CType(sender, TextBox).Undo()
            e.Cancel = True
        End If
    End Sub
    Private Sub reg_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If (CType(sender, TextBox).Text = Space(CType(sender, TextBox).Text.Length)) Then
            CType(sender, TextBox).Text = "0"
        End If
        If Not IsNumeric(CType(sender, TextBox).Text) Or Val(CType(sender, TextBox).Text) > 90 Then
            CType(sender, TextBox).Undo()
            e.Cancel = True
        Else
            CType(sender, TextBox).Text = Val(CType(sender, TextBox).Text).ToString("##")
        End If
    End Sub
    Private Sub Pi_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If (CType(sender, TextBox).Text = Space(CType(sender, TextBox).Text.Length)) Then
            CType(sender, TextBox).Text = "0"
        End If
        If Not IsNumeric(CType(sender, TextBox).Text) Or Val(CType(sender, TextBox).Text) > 72 Then
            CType(sender, TextBox).Undo()
            e.Cancel = True
        End If
    End Sub
    Private Sub Civ1_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If (CType(sender, TextBox).Text = Space(CType(sender, TextBox).Text.Length)) Then
            CType(sender, TextBox).Text = "0"
        End If
        If Not IsNumeric(CType(sender, TextBox).Text) Or Val(CType(sender, TextBox).Text) > 12 Then
            CType(sender, TextBox).Undo()
            e.Cancel = True
        End If
    End Sub
    Private Sub TextBox2_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox2.Validating
        If Not IsNumeric(CType(sender, TextBox).Text) Or Val(CType(sender, TextBox).Text) < (Today.Year - 1) Or Val(CType(sender, TextBox).Text) > (Today.Year + 1) Then
            CType(sender, TextBox).Undo()
            e.Cancel = True
        End If
    End Sub
    Private Sub TextBox93_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Numbox11.Validating, Numbox12.Validating
        If Val(CType(sender, TextBox).Text) > 72 Then
            CType(sender, TextBox).Undo()
            e.Cancel = True
        End If
    End Sub
    Private Sub TextBox96_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
        output_cptvarie()
    End Sub
    Private Sub TextBox93_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
        output_causali()
    End Sub
    Private Sub Pi_Validated(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer
        For i = 0 To 11
            If CType(ArPi(i), TextBox) Is CType(sender, TextBox) Then
                CType(ArDescPi(i), TextBox).Text = leggi_cii(Val(CType(sender, TextBox).Text))
            End If
        Next
        CType(sender, TextBox).Text = Val(CType(sender, TextBox).Text).ToString("##")
    End Sub
    Private Sub Civ1_Validated(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i, z As Integer
        For i = 0 To 2
            If CType(ArCiv1(i), TextBox) Is CType(sender, TextBox) Then
                z = Val(CType(sender, TextBox).Text)
                If z > 0 Then
                    CType(ArCiv2(i), TextBox).Text = CType(ArPi(z - 1), TextBox).Text
                    CType(ArCiv3(i), TextBox).Text = CType(ArDescPi(z - 1), TextBox).Text
                End If
            End If
        Next
        CType(sender, TextBox).Text = Val(CType(sender, TextBox).Text).ToString("##")
    End Sub
    Private Sub Cpt1_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If CptCon(sender.text) = False Then
            e.Cancel = True
        End If
    End Sub
    Function CptCon(ByVal codice As String) As Boolean
        CptCon = True

        If codice.Trim = "" Then
            Exit Function
        End If
        If codice.Length <> 5 Then
            Return False
        End If
        If codice.Substring(2, 1) <> "." Or Not IsNumeric(codice.Substring(0, 2)) Or Not IsNumeric(codice.Substring(3, 2)) Then
            Return False
        End If
    End Function
    Private Sub Cpt1_Validated(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Int16
        For i = 0 To 2
            If CType(ArCpt1(i), TextBox) Is CType(sender, TextBox) Then
                CType(ArCpt2(i), TextBox).Text = leggi_Pia(CType(ArCpt1(i), TextBox).Text)
            End If
        Next
    End Sub
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        aggiorna()
        inizializza()
        TextBox4.Focus()
    End Sub
    Private Sub Numbox3_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
        output_Pagato()
    End Sub
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub ButtonF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF4.Click
        CambioAnnoGestione("TbTai")
    End Sub

    Private Sub CambioAnnoGestione(ByVal archivio As String)
        Dim AnnoNuovo As Int16 = TaiRw("TaiAnno") + 1
        Dim AnnoSis As Int16 = Today.Date.Year

        If AnnoNuovo > AnnoSis + 1 Then
            Exit Sub
        End If

        If MessageBox.Show("DEVO APRIRE L'ANNO " & AnnoNuovo & " ?", "APERTURA NUOVO ANNO DI GESTIONE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        Dim RwNuovo As DataRow

        RwNuovo = TaiDs.Tables(archivio).NewRow
        RwNuovo.ItemArray = TaiRw.ItemArray
        RwNuovo("TaiAnno") = AnnoNuovo
        For i As Int16 = 1 To 20
            TaiRw("TaiDoc" & i) = 0
        Next

        TaiDs.Tables(archivio).Rows.Add(RwNuovo)
        If TaiAd.Update(TaiDs, archivio) = -1 Then
            Exit Sub
        End If
        TaiDs.AcceptChanges()

        inizializza()
        leggiTai(1)
        output_Campi()
        TextBox4.Focus()
    End Sub
End Class
