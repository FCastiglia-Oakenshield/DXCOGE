Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports NPRINT
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class RSProspetto
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
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonF9 As System.windows.forms.button
    Friend WithEvents ButtonF5 As System.windows.forms.button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonF8 As System.windows.forms.button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RSProspetto))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.ButtonF8 = New System.windows.forms.button
        Me.ButtonF9 = New System.windows.forms.button
        Me.ButtonF5 = New System.windows.forms.button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.DateTimePicker2)
        Me.GroupBox1.Controls.Add(Me.DateTimePicker1)
        Me.GroupBox1.Location = New System.Drawing.Point(197, 227)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(619, 202)
        Me.GroupBox1.TabIndex = 28
        Me.GroupBox1.TabStop = False
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(56, 32)
        Me.TextBox2.MaxLength = 60
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(472, 22)
        Me.TextBox2.TabIndex = 1
        Me.TextBox2.Text = ""
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 16)
        Me.Label3.TabIndex = 217
        Me.Label3.Text = "Codice Servizio"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(8, 32)
        Me.TextBox1.MaxLength = 3
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(48, 22)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = ""
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ButtonF8)
        Me.GroupBox2.Controls.Add(Me.ButtonF9)
        Me.GroupBox2.Controls.Add(Me.ButtonF5)
        Me.GroupBox2.Location = New System.Drawing.Point(544, 16)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(64, 176)
        Me.GroupBox2.TabIndex = 215
        Me.GroupBox2.TabStop = False
        '
        'ButtonF8
        '
        Me.ButtonF8.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF8.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonF8.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF8.ImageIndex = 7
        Me.ButtonF8.ImageList = Me.ImageList1_32
        Me.ButtonF8.Location = New System.Drawing.Point(16, 72)
        Me.ButtonF8.Name = "ButtonF8"
        Me.ButtonF8.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF8.TabIndex = 12
        Me.ButtonF8.TabStop = False
        Me.BaseTip.SetToolTip(Me.ButtonF8, "F8 -  RICERCA")
        '
        'ButtonF9
        '
        Me.ButtonF9.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.ButtonF9.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonF9.ImageIndex = 8
        Me.ButtonF9.ImageList = Me.ImageList1_32
        Me.ButtonF9.Location = New System.Drawing.Point(16, 120)
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
        Me.ButtonF5.Location = New System.Drawing.Point(16, 24)
        Me.ButtonF5.Name = "ButtonF5"
        Me.ButtonF5.Size = New System.Drawing.Size(36, 36)
        Me.ButtonF5.TabIndex = 1
        Me.ButtonF5.TabStop = False
        Me.BaseTip.SetToolTip(Me.ButtonF5, "F5 - RESET")
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(64, 144)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Alla  Data"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(64, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Dalla Data"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.DateTimePicker2.Location = New System.Drawing.Point(184, 136)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(96, 22)
        Me.DateTimePicker2.TabIndex = 3
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.DateTimePicker1.Location = New System.Drawing.Point(184, 96)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(96, 22)
        Me.DateTimePicker1.TabIndex = 2
        '
        'RSProspetto
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(1012, 656)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "RSProspetto"
        Me.Text = "RSProspetto"
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Dim Rpt As ReportClass
    Dim Rpt1 As EleRic
    Dim DsSer As DataSet
    Dim DaSer As SqlDataAdapter
    Dim CbSer As SqlCommandBuilder
    Dim Ser As String = "TbSer"
    Dim EsisteRiga As Boolean
    Dim MaxRig, How, cod As Int16
    Dim TesTest, R As Int16
    Dim MiglioFo As Int32
    Private Sub SerConti_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cercamiglio()
        DateTimePicker1.Value = "01/01/" & Today.Year
        DateTimePicker2.Value = Date.DaysInMonth(Today.Year, Today.Month) & "/" & Today.Month & "/" & Today.Year
    End Sub
    Private Sub textbox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If Val(TextBox1.Text) > 0 Then leggiservizi(TextBox1.Text)
    End Sub
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
        If controllo() = False Then
            Exit Sub
        End If
        Cursor.Current = Cursors.WaitCursor
        Dim frm As New LpLp
        Rpt = New ReportClass
        Rpt1 = New EleRic
        Rpt = Rpt1

        Rpt.SetParameterValue("periodo", "Dal " & DateTimePicker1.Text & " al " & DateTimePicker2.Text)
        Rpt.SetParameterValue("DaData", DateTimePicker1.Text)
        Rpt.SetParameterValue("AData", DateTimePicker2.Text)
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("CodSer", TextBox1.Text)
        Rpt.SetParameterValue("Servizio", TextBox2.Text)
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
    Private Function controllo() As Boolean
        controllo = True
        If Not IsNumeric(TextBox1.Text) Then
            Return False
        End If
        If Val(TextBox1.Text) < 1 Or Val(TextBox1.Text) > 999 Then
            TextBox1.Focus()
        End If
        If TextBox2.Text.Trim = "" Then
            TextBox1.Focus()
            Return False
        End If

    End Function
    Private Sub Pulizia()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox1.Focus()
    End Sub
    Private Sub cercamiglio()
        Cmd = New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'CL'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
    End Sub
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        TextBox1.Text = Query.Cercaservizi()
        leggiservizi(TextBox1.Text)
        TextBox1.Focus()
    End Sub
    Private Function leggiservizi(ByVal codart As String) As Int16
        leggiservizi = 0
        Cmd = New SqlCommand("Select * from TbSer where SerCod ='" & codart & "'", cnDb)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextBox2.Text = dataRd.Item("SerDesc")
        Else
            leggiservizi = -1
            TextBox2.Text = ""
        End If
        dataRd.Close()
    End Function
End Class
