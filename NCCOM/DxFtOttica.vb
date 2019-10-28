Imports DXBASE
Imports System.Data.SqlClient
Public Class DxFtOttica

    Public WriteOnly Property _RifDcg As Integer
        Set(ByVal value As Integer)
            RifDcg = value
        End Set
    End Property

    Public WriteOnly Property _TotaleFattura As Decimal
        Set(ByVal value As Decimal)
            TotaleMerce = value
        End Set
    End Property

    Public ReadOnly Property _TotaleResiduo As Decimal
        Get
            Return TotaleResiduo
        End Get
    End Property

    Public WriteOnly Property _MaxRow As Int16
        Set(ByVal value As Int16)
            MaxRow = value
        End Set
    End Property

    Public ReadOnly Property _DT As DataTable
        Get
            Return DsFat
        End Get
    End Property
    Public ReadOnly Property _CodPag As Int16
        Get
            Return CodPag
        End Get
    End Property
    Public WriteOnly Property _RegIva As Int16
        Set(ByVal value As Int16)
            RegIva = value
        End Set
    End Property
    Public WriteOnly Property _CodFor As String
        Set(ByVal value As String)
            CodFor = value
        End Set
    End Property

    Dim DsFat As DataTable
    Dim DaFat As SqlDataAdapter

    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim Merce, Sw, MaxRow As Int16
    Dim RifDcg As Integer = -1
    Dim CodPag As Int16 = 0
    Dim Iset As Integer = -1
    Dim RegIva As Int16 = -1
    Dim CodFor As String = ""
    Dim Causale As Int16 = 2 '' PER ORA SOLO FATTURE FORNITORI
    Dim TC(2) As String
    Dim TotaleMerce, X0, X1, X2, TotaleResiduo As Decimal
    Dim Okfat As Boolean
    Dim OkCpt As Boolean = False
    Dim RwFat As DataRow


    '''Private Sub DxFtOttica_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    '''    'PopolaCii()
    '''    'AssegnaGrid()
    '''    'Pulizia()
    '''End Sub
    Sub PopolaCii()
        ImageComboBoxEdit4.Properties.Items.Clear()
        Dim Str As String = "SELECT CiiCod,CiiDes,CiiAli from TbCii WHERE CiiDes >'' order by CiiCod"
        Dim SS As String = "0 "
        nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, 0, -1)
        ImageComboBoxEdit4.Properties.Items.Add(nn)
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiDes")
            nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), dataRd.Item("CiiAli"))
            ImageComboBoxEdit4.Properties.Items.Add(nn)
        End While
        dataRd.Close()
        Sw = 1
    End Sub
    Sub AssegnaGrid()
        DsFat = New DataTable
        DaFat = New SqlDataAdapter("exec XFDCG @RIF=" & RifDcg, cnDb)
        DaFat.SelectCommand.CommandTimeout = 300
        DaFat.Fill(DsFat)
        If RifDcg < 1 And RegIva > 0 And CodFor > "01000" Then
            UltimeCpt()
            OkCpt = True
        Else
            GridControl1.DataSource = DsFat
            GridControl1.Refresh()
            OkCpt = False
        End If
        GridView1.UnselectRow(0)
        RicalcoloFinale()
        AddRwFat()
    End Sub
    Sub UltimeCpt()
        TC(1) = " and PriCoDare = "
        TC(2) = " and PriCoAvere = "
        Dim Id As Int32 = 0
        Dim Cmd As New SqlCommand("SELECT ISNULL(MAX(PRIID),0) FROM TBPRI WHERE PRIREGIVA = " & RegIva & TC(Causale) & " '" & CodFor & "' And PRICAUSALE = " & Causale & " And PriMeseSk = '*'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Id = dataRd.Item(0)
        End While
        dataRd.Close()
        If Id > 0 Then GoTo OkTrovato
        Cmd = New SqlCommand("SELECT ISNULL(MAX(PRIID),0) FROM TBPRI WHERE PRIREGIVA = " & RegIva & TC(Causale) & " '" & CodFor & "' And PRICAUSALE = " & Causale, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Id = dataRd.Item(0)
        End While
        dataRd.Close()
OkTrovato:
        DsFat = New DataTable
        DaFat = New SqlDataAdapter("SELECT *,PriFl06=(Select PiaFl06 from TbPia where PiaCodCo=CPT) from VH1H2 where PriId = " & Id, cnCo)
        DaFat.SelectCommand.CommandTimeout = 300
        DaFat.Fill(DsFat)
        GridControl1.DataSource = DsFat
        GridControl1.Refresh()
        If DsFat.Rows.Count > 0 Then
            ScorporaDati()
        End If
    End Sub
    Sub ScorporaDati()
        Dim q As Int16
        If DsFat.Rows.Count = 1 Then
            RwFat = DsFat.Rows(0)
            RwFat("PriProg") = 1
            RwFat("PriValuta") = 0
            RwFat("PriIvaPrint") = 0
            RwFat("PriArtFisc") = 0
            RwFat("PriGStampa") = 0
            UltimaRiga(CDec(TextEdit13.EditValue), True)
        Else
            For q = 1 To DsFat.Rows.Count
                RwFat = DsFat.Rows(q - 1)
                RwFat("PriProg") = q
                If RwFat("PriFl06") = 1 Then
                    UltimaRiga(CDec(TextEdit13.EditValue), True)
                Else
                    RwFat("PriImpDare") = 0
                    RwFat("PriImpAvere") = 0
                End If
                RwFat("PriValuta") = 0
                RwFat("PriArtFisc") = 0
                RwFat("PriGStampa") = 0
                RwFat("PriIvaPrint") = 0
            Next
        End If
    End Sub
    Sub UltimaRiga(ByVal Residuo As Decimal, ByVal Tipo As Boolean)
        If Tipo = True Then
            RwFat("PriImpDare") = Residuo
        End If

        RwFat("PriImpAvere") = (RwFat("PriImpDare") + X0) * RwFat("CiiAli") / 100
Inext:
        RwFat("PriImpAvere") = Format(RwFat("PriImpAvere"), "#########0.00")
    End Sub
    Sub Pulizia()
        TextEdit10.EditValue = 0
        TextEdit13.EditValue = TotaleMerce
        TextEdit14.EditValue = CDec(0.0)
        TextEdit15.EditValue = 0
        TextEdit16.EditValue = CDec(0.0)
        TextEdit3.EditValue = CDec(0.0)
        TextEdit1.EditValue = CDec(0.0)
        ImageComboBoxEdit4.SelectedIndex = 0
        TextEdit19.EditValue = CDec(0.0)
        TextEdit20.EditValue = "00.00"
        TextEdit21.EditValue = ""
        MemoEdit1.EditValue = ""
        X2 = CDec(0.0)
        X1 = CDec(0.0)
        X0 = CDec(0.0)
        TotaleResiduo = CDec(0.0)
        Merce = 1
        ButtonF3.Enabled = False
    End Sub
    Sub AbilitaIns(Ok As Boolean)
        ButtonF11.Enabled = Ok : TextEdit16.Enabled = Ok : ImageComboBoxEdit4.Enabled = Ok
        TextEdit19.Enabled = Ok : TextEdit20.Enabled = Ok : TextEdit21.Enabled = Ok : ButtonF8.Enabled = Ok
    End Sub
    Function LeggiPagam(ByVal codice As Int16) As String
        LeggiPagam = ""
        Cmd = New SqlCommand("select * from TbPag where PagCod = " & codice, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            LeggiPagam = dataRd.Item("PagDesc")
        End If
        dataRd.Close()
        MemoEdit1.EditValue = LeggiPagam
        TextEdit10.EditValue = codice
        CodPag = codice
    End Function
    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If ControllaConto() = False Then TextEdit20.Focus() : Exit Sub
        RicalcolaDocumento()
        If iset > -1 Then
            GridView1.UnselectRow(iset)
        End If
        ButtonF3.Enabled = False
    End Sub
    Sub RicalcolaDocumento()
        Dim Zez As Boolean = False
        If RwFat("PriImpDare") = 0 And RwFat("PriImpAvere") = 0 Then Zez = True
        If Okfat = False And Zez = False Then DsFat.Rows.Add(RwFat)
        RicalcoloFinale()
        AddRwFat()
        CaricaDettagli()
    End Sub
    Sub RegistraFattura()

    End Sub

    Sub CaricaDettagli()
        TextEdit15.EditValue = RwFat("PriProg")
        TextEdit16.EditValue = RwFat("PriImpDare")
        ImageComboBoxEdit4.EditValue = RwFat("PriCodIva")
        TextEdit19.EditValue = RwFat("PriImpAvere")
        TextEdit20.EditValue = RwFat("Cpt")
        If ImageComboBoxEdit4.EditValue > 0 Then
            RwFat("CiiDes") = Mid(DirectCast(ImageComboBoxEdit4.SelectedItem, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Description, 4, 20)
            RwFat("PriCodIva") = DirectCast(ImageComboBoxEdit4.SelectedItem, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Value
            RwFat("CiiAli") = DirectCast(ImageComboBoxEdit4.SelectedItem, DevExpress.XtraEditors.Controls.ImageComboBoxItem).ImageIndex
        End If
        LeggiConto()
        RwFat("PriFl06") = Merce
        '  TotaleIn()
        TextEdit16.Focus()
    End Sub
    Sub RicalcoloFinale()
        Dim x As Int16
        X2 = 0 '' TOTALE IMPONIBILE ( ALTRI ADDEBITI )
        X1 = 0 '' TOTALE FATTURA
        X0 = 0 '' TOTALE IMPONIBILE ( MERCE )

        For x = 1 To DsFat.Rows.Count
            RwFat = DsFat.Rows(x - 1)
            RwFat("PriProg") = x
            X1 = X1 + RwFat("PriImpDare") + RwFat("PriImpAvere")
            If RwFat("PriFl06") > 0 Then X0 = X0 + RwFat("PriImpDare") Else X2 = X2 + RwFat("PriImpDare")
        Next
        Differenze(TotaleMerce)
    End Sub
    Sub Differenze(TM As Decimal)
        TextEdit3.EditValue = X1
        TextEdit2.EditValue = X2
        TextEdit13.EditValue = TM
        TotaleResiduo = TM - X0
        TextEdit14.EditValue = TotaleResiduo
        TextEdit1.EditValue = TextEdit13.EditValue - TextEdit14.EditValue
        If TextEdit14.EditValue <> 0 Then
            TextEdit14.ErrorIcon = DevExpress.XtraEditors.BaseEdit.DefaultErrorIcon
            TextEdit14.ErrorIconAlignment = ErrorIconAlignment.BottomLeft
            TextEdit14.ErrorText = "NON QUADRA!!!"
        Else
            TextEdit14.ErrorText = ""
        End If
    End Sub
    Function ControllaConto() As Boolean
        ControllaConto = False
        AggiustaConto()
        If Mid(TextEdit20.Text, 3, 3) = ".00" Or Mid(TextEdit20.Text, 1, 3) = "00." Then Exit Function '' mastri e transitorio
        ControllaConto = LeggiConto()
    End Function
    Sub AggiustaConto()
        Dim x As Int16
        For x = 1 To Len(TextEdit20.Text)
            If Mid(TextEdit20.Text, x, 1) = "." Then
                TextEdit20.Text = Format(Val(Mid(TextEdit20.Text, 1, x - 1)), "00") & "." & Format(Val(Mid(TextEdit20.Text, x + 1, Len(TextEdit20.Text) - (x - 1))), "00")
                Exit Sub
            End If
        Next
    End Sub
    Function LeggiConto() As Boolean
        Merce = 1
        TextEdit21.EditValue = "*** ERRATO ***"
        LeggiConto = False
        AggiustaConto() ''''' verifica il punto se e' un sottoconto
        Dim Str As String = "SELECT * from TbPia where PiaCodCo = '" & TextEdit20.EditValue & "'"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit21.EditValue = dataRd("PiaAnaCo")
            Merce = dataRd("PiaFl06")
            LeggiConto = True
        End While
        dataRd.Close()
    End Function
    REM INIZIO RIGHE
    Private Sub TextEdit16_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit16.Leave
        If TextEdit16.EditValue.ToString = "" Then TextEdit16.EditValue = "00.00"
        CalcolaRiga(0)
    End Sub
    Private Sub ImageComboBoxEdit4_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageComboBoxEdit4.Validated
        CalcolaRiga(0)
    End Sub
    Private Sub ImageComboBoxEdit4_CloseUp(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles ImageComboBoxEdit4.CloseUp
        If Sw > 0 Then System.Windows.Forms.SendKeys.Send("{TAB}")
    End Sub
    Private Sub TextEdit19_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit19.Leave
        If TextEdit16.EditValue.ToString = "" Then TextEdit19.EditValue = "00.00"
        CalcolaRiga(1)
    End Sub
    Private Sub TextEdit20_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit20.Leave
        TextEdit20.EditValue = TextEdit20.Text.PadLeft(5, "0")
        LeggiConto()
        CalcolaRiga(2)
    End Sub
    Sub CalcolaRiga(ByVal i As Int16)
        If i = 0 Then
            RwFat("PriImpDare") = CDec(TextEdit16.EditValue)
            RwFat("PriCodIva") = 0
            RwFat("CiiAli") = 0
            RwFat("CiiDes") = ""

            If ImageComboBoxEdit4.EditValue > 0 Then
                RwFat("CiiDes") = Mid(DirectCast(ImageComboBoxEdit4.SelectedItem, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Description, 4, 20)
                RwFat("PriCodIva") = DirectCast(ImageComboBoxEdit4.SelectedItem, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Value
                RwFat("CiiAli") = DirectCast(ImageComboBoxEdit4.SelectedItem, DevExpress.XtraEditors.Controls.ImageComboBoxItem).ImageIndex
            End If
            CalcolaIva()
            TextEdit19.EditValue = RwFat("PriImpAvere")
        End If
        If i = 1 Then
            RwFat("PriImpAvere") = CDec(TextEdit19.EditValue)
        End If
        If i = 2 Then
            RwFat("Cpt") = TextEdit20.EditValue
            RwFat("AVEREDESC") = TextEdit21.EditValue
            RwFat("PriFl06") = Merce
        End If
        '' TotaleIn()
    End Sub
    Sub CalcolaIva()
        Dim X3 As Decimal = 0
        For i As Int16 = (RwFat("PriProg") - 1) To 1 Step -1
            If DsFat.Rows(i - 1).Item("PriCodIva") = 0 Then X3 += DsFat.Rows(i - 1).Item("PriImpDare") Else Exit For
        Next
        RwFat("PriImpAvere") = Format((RwFat("PriImpDare") + X3) * RwFat("CiiAli") / 100, "#########0.00")

    End Sub
    Sub AddRwFat()
        If (DsFat.Rows.Count + 1) > MaxRow Then
            MessageBox.Show("Raggiunto Nr. Max Righe consentite (" & MaxRow & ")", "CONTROLLO REGISTRAZIONI", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextEdit16.EditValue = CDec(0.0)
            ImageComboBoxEdit4.SelectedIndex = 0
            TextEdit19.EditValue = CDec(0.0)
            TextEdit20.EditValue = "00.00"
            TextEdit21.EditValue = ""
            AbilitaIns(False)
            Exit Sub
        Else
            ButtonF11.Enabled = True
        End If
        RwFat = DsFat.NewRow
        RwFat("PriProg") = DsFat.Rows.Count + 1
        RwFat("PriCodIva") = 0
        RwFat("PriImpDare") = 0
        RwFat("PriImpavere") = 0
        RwFat("Cpt") = "00.00"
        RwFat("CiiAli") = 0
        RwFat("CiiDes") = ""
        RwFat("AVEREDESC") = ""
        RwFat("PriFl06") = 1
        Okfat = False
        ButtonF3.Enabled = False
    End Sub
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        TextEdit20.EditValue = Query.CercaPia()
        SelectNextControl(TextEdit20, True, True, True, True)
    End Sub
    Private Sub GridView1_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView1.RowClick
        If e.RowHandle > -1 Then
            Iset = e.RowHandle
            RwFat = GridView1.GetDataRow(Iset)
            CaricaDettagli()
            Okfat = True
            ButtonF3.Enabled = True
            AbilitaIns(True)
        Else
            Iset = -1
            Okfat = False
            ButtonF3.Enabled = False
        End If
    End Sub
    Private Sub TextEdit10_Leave(sender As Object, e As System.EventArgs) Handles TextEdit10.Leave
        LeggiPagam(Val(TextEdit10.EditValue))
    End Sub

    Private Sub HyperLinkEdit1_Click(sender As Object, e As System.EventArgs) Handles HyperLinkEdit1.Click
        Dim CodiceRt As Int16 = 0
        CodiceRt = Ricerche.LnkCodPag()
        If CodiceRt > 0 Then LeggiPagam(CodiceRt)
    End Sub
    Private Sub ButtonF3_Click(sender As System.Object, e As System.EventArgs) Handles ButtonF3.Click
        If Okfat = True Then
            RwFat.Delete()
            DsFat.AcceptChanges()
            RicalcoloFinale()
            AddRwFat()
            CaricaDettagli()
        End If
    End Sub
End Class
