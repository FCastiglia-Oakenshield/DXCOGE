Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports NPRINT
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class TabVend
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
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonF9 As System.windows.forms.button
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonF8 As System.windows.forms.button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonF5 As System.windows.forms.button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(TabVend))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.ButtonF9 = New System.windows.forms.button
        Me.ButtonF5 = New System.windows.forms.button
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.ButtonF8 = New System.windows.forms.button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.TextBox9 = New System.Windows.Forms.TextBox
        Me.TextBox8 = New System.Windows.Forms.TextBox
        Me.TextBox7 = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.GroupBox4)
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.DateTimePicker2)
        Me.GroupBox1.Controls.Add(Me.DateTimePicker1)
        Me.GroupBox1.Location = New System.Drawing.Point(134, 228)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(745, 320)
        Me.GroupBox1.TabIndex = 26
        Me.GroupBox1.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ButtonF9)
        Me.GroupBox2.Controls.Add(Me.ButtonF5)
        Me.GroupBox2.Location = New System.Drawing.Point(600, 192)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(136, 80)
        Me.GroupBox2.TabIndex = 215
        Me.GroupBox2.TabStop = False
        '
        'ButtonF9
        '
        Me.ButtonF9.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF9.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF9.ImageIndex = 8
        Me.ButtonF9.ImageList = Me.ImageList1_32
        Me.ButtonF9.Location = New System.Drawing.Point(24, 24)
        Me.ButtonF9.Name = "ButtonF9"
        Me.ButtonF9.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF9.TabIndex = 0
        Me.BaseTip.SetToolTip(Me.ButtonF9, "F9 - Stampa")
        '
        'ButtonF5
        '
        Me.ButtonF5.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF5.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF5.ImageIndex = 4
        Me.ButtonF5.ImageList = Me.ImageList1_32
        Me.ButtonF5.Location = New System.Drawing.Point(80, 24)
        Me.ButtonF5.Name = "ButtonF5"
        Me.ButtonF5.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF5.TabIndex = 1
        Me.ButtonF5.TabStop = False
        Me.BaseTip.SetToolTip(Me.ButtonF5, "F5 - RESET")
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.TextBox6)
        Me.GroupBox4.Controls.Add(Me.TextBox5)
        Me.GroupBox4.Controls.Add(Me.TextBox4)
        Me.GroupBox4.Controls.Add(Me.TextBox3)
        Me.GroupBox4.Controls.Add(Me.ButtonF8)
        Me.GroupBox4.Controls.Add(Me.TextBox2)
        Me.GroupBox4.Controls.Add(Me.TextBox1)
        Me.GroupBox4.Location = New System.Drawing.Point(16, 24)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(720, 112)
        Me.GroupBox4.TabIndex = 0
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Selezione Cliente"
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(664, 72)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ReadOnly = True
        Me.TextBox6.Size = New System.Drawing.Size(40, 22)
        Me.TextBox6.TabIndex = 15
        Me.TextBox6.TabStop = False
        Me.TextBox6.Text = ""
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(136, 72)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(528, 22)
        Me.TextBox5.TabIndex = 14
        Me.TextBox5.TabStop = False
        Me.TextBox5.Text = ""
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(72, 72)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(64, 22)
        Me.TextBox4.TabIndex = 13
        Me.TextBox4.TabStop = False
        Me.TextBox4.Text = ""
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(72, 48)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(632, 22)
        Me.TextBox3.TabIndex = 12
        Me.TextBox3.TabStop = False
        Me.TextBox3.Text = ""
        '
        'ButtonF8
        '
        Me.ButtonF8.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF8.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF8.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF8.ImageIndex = 7
        Me.ButtonF8.ImageList = Me.ImageList1_32
        Me.ButtonF8.Location = New System.Drawing.Point(24, 56)
        Me.ButtonF8.Name = "ButtonF8"
        Me.ButtonF8.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF8.TabIndex = 11
        Me.ButtonF8.TabStop = False
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(72, 24)
        Me.TextBox2.MaxLength = 60
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(632, 22)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.TabStop = False
        Me.TextBox2.Text = ""
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(16, 24)
        Me.TextBox1.MaxLength = 5
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(56, 22)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = ""
        '
        'RadioButton2
        '
        Me.RadioButton2.Location = New System.Drawing.Point(280, 248)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(120, 24)
        Me.RadioButton2.TabIndex = 4
        Me.RadioButton2.Text = "Fatture Fiscali"
        '
        'RadioButton1
        '
        Me.RadioButton1.Location = New System.Drawing.Point(32, 248)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(128, 24)
        Me.RadioButton1.TabIndex = 3
        Me.RadioButton1.Text = "Ricevute Fiscali"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(32, 208)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Alla  Data"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(32, 168)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Dalla Data"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.DateTimePicker2.Location = New System.Drawing.Point(128, 200)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(96, 22)
        Me.DateTimePicker2.TabIndex = 2
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.DateTimePicker1.Location = New System.Drawing.Point(128, 160)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(96, 22)
        Me.DateTimePicker1.TabIndex = 1
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox3.Controls.Add(Me.TextBox9)
        Me.GroupBox3.Controls.Add(Me.TextBox8)
        Me.GroupBox3.Controls.Add(Me.TextBox7)
        Me.GroupBox3.Location = New System.Drawing.Point(133, 108)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(745, 112)
        Me.GroupBox3.TabIndex = 27
        Me.GroupBox3.TabStop = False
        '
        'TextBox9
        '
        Me.TextBox9.Location = New System.Drawing.Point(8, 72)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.ReadOnly = True
        Me.TextBox9.Size = New System.Drawing.Size(728, 22)
        Me.TextBox9.TabIndex = 2
        Me.TextBox9.Text = "movimentati  con dettaglio dei documenti emessi."
        '
        'TextBox8
        '
        Me.TextBox8.Location = New System.Drawing.Point(8, 48)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.ReadOnly = True
        Me.TextBox8.Size = New System.Drawing.Size(728, 22)
        Me.TextBox8.TabIndex = 1
        Me.TextBox8.Text = "In alternativa  avremo,  in base al  periodo scelto e al tipo di  documento selez" & _
        "ionato, un' esposizione  alfabetica dei clienti"
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(8, 24)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.Size = New System.Drawing.Size(728, 22)
        Me.TextBox7.TabIndex = 0
        Me.TextBox7.TabStop = False
        Me.TextBox7.Tag = ""
        Me.TextBox7.Text = "Richiedendo un singolo cliente si ottiene l'elenco di tutti i documenti (Ricevute" & _
        " e/o Fatture) emessi nel periodo selezionato."
        '
        'TabVend
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(1012, 656)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "TabVend"
        Me.Text = "TabVend"
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.Controls.SetChildIndex(Me.GroupBox3, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim Rpt As ReportClass
    Dim Rpt1 As TabRic
    Dim Rpt2 As CliRic
    Dim DsSer As DataSet
    Dim DaSer As SqlDataAdapter
    Dim CbSer As SqlCommandBuilder
    Dim Ser As String = "TbSer"
    Dim EsisteRiga As Boolean
    Dim MaxRig, How As Int16
    Dim TesTest, R As Int16
    Dim MiglioFo As Int32
    Private Sub SerConti_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cercamiglio()
        RadioButton1.Checked = True
        DateTimePicker1.Value = "01/01/" & Today.Year
        DateTimePicker2.Value = Date.DaysInMonth(Today.Year, Today.Month) & "/" & Today.Month & "/" & Today.Year
    End Sub
    Private Sub textbox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If Val(TextBox1.Text) > 1000 And Val(TextBox1.Text) < MiglioFo Then
            TextBox1.Text = Format(Val(TextBox1.Text), "00000")
            leggiAna(TextBox1.Text)
        Else
            Pulizia()
        End If
    End Sub
    Private Function controllo() As Boolean
        controllo = True
    End Function
    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Pulizia()
    End Sub
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            ButtonF5.PerformClick()
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

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click

        Cursor.Current = Cursors.WaitCursor
        Dim frm As New LpLp
        Dim tipod As String
        Rpt = New ReportClass
        Rpt1 = New TabRic
        Rpt2 = New CliRic
        If TextBox1.Text <> "" And Val(TextBox1.Text) > 1000 Then
            Rpt = Rpt2
        Else
            Rpt = Rpt1
        End If
        If RadioButton1.Checked = True Then tipod = "R" Else tipod = "F"

        Rpt.SetParameterValue("periodo", "Dal " & DateTimePicker1.Text & " al " & DateTimePicker2.Text)
        Rpt.SetParameterValue("DaData", DateTimePicker1.Text)
        Rpt.SetParameterValue("AData", DateTimePicker2.Text)
        Rpt.SetParameterValue("tipod", tipod)
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("CLIENTE", TextBox1.Text)
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()

    End Sub



    Private Sub Pulizia()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
    End Sub
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        Dim frm As New RicercaClFo
        frm.StartPosition = FormStartPosition.Manual
        frm.Location = New Point(GroupBox1.Location.X, GroupBox1.Location.Y + 80)
        frm.CliFor = "CL"
        frm.ShowDialog()
        TextBox1.Text = frm.codice
        If TextBox1.Text > "" Then
            leggiAna(TextBox1.Text)
            TextBox1.Focus()
        Else
            Pulizia()
        End If
    End Sub
    Private Sub leggiAna(ByVal codice As String)
        Cmd = New SqlCommand("select * from TbAna where AnaGrp = 'CL' and AnaCod ='" & codice & "'", cnVd)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            CaricaAna()
        Else
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = ""
        End If
        dataRd.Close()
    End Sub
    Private Sub CaricaAna()
        TextBox2.Text = dataRd.Item("AnaDesc")
        TextBox3.Text = dataRd.Item("AnaIndirizzo")
        TextBox4.Text = dataRd.Item("AnaCap")
        TextBox5.Text = dataRd.Item("AnaCitta")
        TextBox6.Text = dataRd.Item("AnaProv")
    End Sub
    Private Sub cercamiglio()
        Cmd = New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'CL'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
    End Sub
End Class
