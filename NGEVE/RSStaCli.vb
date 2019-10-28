Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports DXBASE.Util
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.ReportSource
Public Class RSStaCli
    Inherits DXBASE.WinBase
    Dim frm As New LpLp
    Dim Rpt As ReportClass
    Dim Rpt1 As New RSRubrica
    Dim Rpt2 As New RSRubInd
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
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonF5 As System.windows.forms.button
    Friend WithEvents ButtonF9 As System.windows.forms.button
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RSStaCli))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ButtonF5 = New System.windows.forms.button
        Me.ButtonF9 = New System.windows.forms.button
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.ButtonF5)
        Me.GroupBox1.Controls.Add(Me.ButtonF9)
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Location = New System.Drawing.Point(350, 266)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(313, 166)
        Me.GroupBox1.TabIndex = 26
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Selezione Stampa Clienti"
        '
        'ButtonF5
        '
        Me.ButtonF5.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF5.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF5.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF5.ImageIndex = 4
        Me.ButtonF5.ImageList = Me.ImageList1_32
        Me.ButtonF5.Location = New System.Drawing.Point(256, 104)
        Me.ButtonF5.Name = "ButtonF5"
        Me.ButtonF5.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF5.TabIndex = 4
        Me.ButtonF5.TabStop = False
        '
        'ButtonF9
        '
        Me.ButtonF9.BackColor = System.Drawing.Color.Transparent
        Me.ButtonF9.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF9.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF9.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF9.ImageIndex = 8
        Me.ButtonF9.ImageList = Me.ImageList1_32
        Me.ButtonF9.Location = New System.Drawing.Point(256, 56)
        Me.ButtonF9.Name = "ButtonF9"
        Me.ButtonF9.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF9.TabIndex = 3
        '
        'RadioButton2
        '
        Me.RadioButton2.Location = New System.Drawing.Point(24, 80)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(192, 24)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.Text = "INDIRIZZO/ALFABETICA"
        '
        'RadioButton1
        '
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(24, 40)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(192, 24)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "ALFABETICA "
        '
        'CheckBox1
        '
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(24, 128)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(184, 24)
        Me.CheckBox1.TabIndex = 2
        Me.CheckBox1.Text = "Solo Clienti Attivi"
        '
        'RSStaCli
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(1012, 656)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "RSStaCli"
        Me.Text = "RSStaCli"
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private Sub RSStaCli_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load, ButtonF5.Click
        RadioButton1.Checked = True
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim selectFormula As String
        frm = New LpLp
        Rpt = New ReportClass
        Rpt1 = New RSRubrica
        Rpt2 = New RSRubInd
        If RadioButton1.Checked = True Then
            Rpt = Rpt1         '' Clienti Alfa
            frm.Text = "Alfabetica Clienti"
        ElseIf RadioButton2.Checked = True Then
            Rpt = Rpt2         '' Clienti Codice
            frm.Text = "Indirizzo/Alfabetica"
        End If
        selectFormula = ""
        If CheckBox1.Checked = True Then selectFormula = "{CRRUBCOMUNITA.ClAttivo} = True"
        Rpt.RecordSelectionFormula = selectFormula
        Rpt.SetParameterValue("Marchio", Marchio)
        frm.reportsource = Rpt
        frm.Show()
    End Sub

End Class
