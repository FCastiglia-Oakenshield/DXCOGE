Imports DXBASE
Imports System.Data.SqlClient

Public Class DxScaRat
    Public Property NDatDoc() As String
        Get
            Return DatDoc
        End Get
        Set(ByVal Value As String)
            DatDoc = Value
        End Set
    End Property
    Public Property NAnaCod() As String
        Get
            Return AnaCod
        End Get
        Set(ByVal Value As String)
            AnaCod = Value
        End Set
    End Property
    Public Property NAnaDesc() As String
        Get
            Return AnaDesc
        End Get
        Set(ByVal Value As String)
            AnaDesc = Value
        End Set
    End Property
    Public Property NAnaPag() As String
        Get
            Return AnaPag
        End Get
        Set(ByVal Value As String)
            AnaPag = Value
        End Set
    End Property
    Public Property NCodPag() As Integer
        Get
            Return CodPag
        End Get
        Set(ByVal Value As Integer)
            CodPag = Value
        End Set
    End Property
    Public Property NImport() As Decimal
        Get
            Return Import
        End Get
        Set(ByVal Value As Decimal)
            Import = Value
        End Set
    End Property
    Public Property NIvaSpe() As Decimal
        Get
            Return IvaSpe
        End Get
        Set(ByVal Value As Decimal)
            IvaSpe = Value
        End Set
    End Property
    Public Property NNumDoc() As Int32
        Get
            Return NumDoc
        End Get
        Set(ByVal Value As Int32)
            NumDoc = Value
        End Set
    End Property
    'Public Property Nrwsca() As DataRowView
    '    Get
    '        Return rwsca
    '    End Get
    '    Set(ByVal Value As DataRowView)
    '        rwsca = Value
    '    End Set
    'End Property

    Dim AnaCod, AnaDesc, AnaPag, DatDoc As String
    Private NumDoc As Int32
    Private Import, IvaSpe As Decimal
    Private CodPag As Integer

    Dim DsScT As DataTable
    Dim DaScT As SqlDataAdapter
    Dim CbSct As SqlCommandBuilder
    Dim RwScT As DataRow

    Dim RwX As DataRow
    '  Dim rwsca As DataRowView

    Dim OLDPAG, TIPOPAG, POSRAT, NUMRAT, UN As Int16
    Dim OLDDES As String

    Private Sub DxScaRat_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        TextEdit1.EditValue = AnaCod
        TextEdit2.EditValue = AnaDesc
        TextEdit4.EditValue = AnaPag
        TextEdit6.EditValue = DatDoc
        TextEdit3.EditValue = CodPag.ToString
        TextEdit5.EditValue = Format(NumDoc, "######").ToString
        TextEdit7.EditValue = Format(Import, "#######0.00;-#######0.00;#").ToString
        OLDPAG = CodPag
        OLDDES = TextEdit4.EditValue
        Iniziale()
    End Sub
    Sub Iniziale()
        Popola()
        TextEdit3.Enabled = True
        GroupControl3.Enabled = True
        ButtonF11.Enabled = False
        TextEdit3.EditValue = OLDPAG
        TextEdit4.EditValue = OLDDES
        TextEdit8.EditValue = 0
        TextEdit9.EditValue = "0"
        TextEdit10.EditValue = CDec(0.0)
        TextEdit11.EditValue = CDec(0.0)
        TextEdit12.EditValue = 0
        TextEdit13.EditValue = 0
        TextEdit14.EditValue = 0
        TextEdit3.Focus()
    End Sub
    Sub Popola()
        Dim Str As String = "Select * from Tbsca where scaconto = '" & TextEdit1.EditValue & "' And scaNdoc = " & NumDoc & " And scaDDoc = '" & TextEdit6.EditValue & "'"
        DsScT = New DataTable
        DaScT = New SqlDataAdapter(Str, cnCo)
        DaScT.SelectCommand.CommandTimeout = 300
        DaScT.Fill(DsScT)
        CbSct = New SqlCommandBuilder(DaScT)
        GridControl8.DataSource = DsScT
        GridControl8.Refresh()
        GridView8.ClearSelection()
        NUMRAT = DsScT.Rows.Count
    End Sub

    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F11 Then
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            ButtonF8.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 Then
            ButtonF5.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        Iniziale()
    End Sub
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        NCodPag = Ricerche.LnkCodPag()
        If NCodPag > 0 Then
            TextEdit3.EditValue = NCodPag.ToString
            DesPag()
        End If
    End Sub
    Sub DesPag()
        Dim ok As Boolean = False
        Cmd = New SqlCommand("SELECT * from TbPag Where PagCod =" & NCodPag, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit4.EditValue = dataRd.Item("PagDesc")
            TIPOPAG = dataRd.Item("PagTipo")
            ok = True
        End While
        dataRd.Close()
        If ok = True Then NuoveScadenze()
    End Sub
    Sub NuoveScadenze()
        If Val(TextEdit3.EditValue) = OLDPAG Then Exit Sub
        Dim TMPF As String
        TMPF = "##88" & cnDb.WorkstationId
        ''' da finire sostituzione TMPF E PASSAGGIO FILE A RATEESCADENZE + LETTURA IN SEQUENZA '''
        Dim execp As String = "exec RATEESCADENZE @Fattura=@fatt,@Spese=@IvaSp,@DataFattura=@DataFatt,@CodPag=@Cpag"
        Dim p1 As New SqlParameter("@Fatt", SqlDbType.Decimal)
        Dim p2 As New SqlParameter("@IvaSp", SqlDbType.Decimal)
        Dim p3 As New SqlParameter("@DataFatt", SqlDbType.SmallDateTime)
        Dim p4 As New SqlParameter("@Cpag", SqlDbType.SmallInt)
        If Trim(TextEdit7.EditValue) = "" Then TextEdit7.EditValue = "0"
        p1.Value = CDec(TextEdit7.EditValue)
        p2.Value = CDec(NIvaSpe)
        p3.Value = CDate(TextEdit6.EditValue)
        p4.Value = Val(TextEdit3.EditValue)
        Dim cmd As New SqlCommand(execp, cnCo)
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmd.Parameters.Add(p3)
        cmd.Parameters.Add(p4)
        cmd.ExecuteNonQuery()
        cmd = New SqlCommand("SELECT * from ##TMP order by NR", cnCo)
        Dim x As Int16 = DsScT.Rows.Count
        Dim y As Int16 = 0
        Dim k As Int16
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            y = y + 1
            If y > x Then GoTo Aggiungi
            RwScT = DsScT.Rows(y - 1)
            RwScT("ScaDSca") = dataRd.Item("scadenza")
            RwScT("ScaImpRata") = dataRd.Item("rata")
            RwScT("ScaTpag") = TIPOPAG
            RwScT("ScaCodPag") = Val(TextEdit3.EditValue)
            GoTo Dopo
Aggiungi:
            RwScT = DsScT.NewRow
            RwScT(0) = 0
            '     If x > 0 Then
            For k = 1 To 19
                RwScT(k) = DsScT.Rows(x - 1).Item(k)
            Next
            'Else
            '    RwScT("ScaTipoCo") = rwsca("PrkTipoCo")
            '    RwScT("ScaConto") = rwsca("PrkConto")
            '    RwScT("ScaNdoc") = rwsca("PrkDocEst")
            '    RwScT("ScaImpDoc") = CDec(TextEdit7.EditValue)
            '    RwScT("ScaIvaSpe") = 0
            '    RwScT("ScaAbi") = 0
            '    RwScT("ScaCab") = 0
            '    RwScT("ScaImpRata") = dataRd.Item("rata")
            '    RwScT("ScaTPag") = TIPOPAG
            '    RwScT("ScaCodPag") = Val(TextEdit3.EditValue)
            '    RwScT("ScaNRata") = 0
            '    RwScT("ScaBan") = 0
            '    RwScT("ScaDdoc") = rwsca("pridataest")
            '    RwScT("ScaDsca") = dataRd.Item("scadenza")
            '    RwScT("ScaRifId") = rwsca("Priid")
            '    RwScT("ScaRifProg") = rwsca("priprog")
            '    RwScT("ScaRifDA") = rwsca("PrkDA")
            '    RwScT("ScaRAperta") = 0
            '    RwScT("ScaImpPagato") = 0
            '      End If
            RwScT("ScaDSca") = dataRd.Item("scadenza")
            RwScT("ScaImpRata") = dataRd.Item("rata")
            RwScT("ScaTpag") = TIPOPAG
            RwScT("ScaCodPag") = Val(TextEdit3.EditValue)
            RwScT("ScaNRata") = dataRd.Item("Nr")
            DsScT.Rows.Add(RwScT)
Dopo:
        End While
        dataRd.Close()

        If y < x Then
            For k = y + 1 To x
                RwScT = DsScT.Rows(k - 1)
                RwScT.Delete()
            Next
        End If
        TextEdit3.Enabled = False
        GroupControl3.Enabled = False
        ButtonF5.Enabled = True
        ButtonF11.Enabled = True
        ButtonF11.Focus()
        NUMRAT = DsScT.Rows.Count
    End Sub

    Private Sub TextEdit3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit3.LostFocus
        If Val(TextEdit3.EditValue) = 0 Then Exit Sub
        NCodPag = Val(TextEdit3.EditValue)
        DesPag()
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        DaScT.Update(DsScT)
        DsScT.AcceptChanges()
        EsegueSql("EXEC POINTSCADENZE", cnCo)
        Me.Close()
    End Sub
    Private Sub GridControl8_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl8.MouseMove
        ShowHitInfo8(GridView8.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo8(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl8.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub
    Private Sub GridView8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView8.Click
        If iset > -1 Then
            RwX = GridView8.GetDataRow(iset)
            PopolaDettagli()
        End If
    End Sub
    Sub PopolaDettagli()
        ButtonF11.Enabled = True
        TextEdit8.EditValue = RwX("ScaNRata")
        TextEdit9.EditValue = RwX("ScaRAperta")
        DateEdit1.EditValue = CDate(RwX("ScaDSca"))
        TextEdit10.EditValue = CDec(RwX("ScaImpRata"))
        TextEdit11.EditValue = CDec(RwX("ScaImpPagato"))
        TextEdit12.EditValue = RwX("ScaAbi")
        TextEdit13.EditValue = RwX("ScaCab")
        TextEdit14.EditValue = RwX("ScaBan")
        TextEdit9.Focus()
    End Sub
    'Private Sub Numbox8_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Numbox8.Enter
    '    If Val(TextEdit8.EditValue) < 1 Or Val(TextEdit8.EditValue) > NUMRAT Then UN = -1 Else UN = Val(TextEdit8.EditValue) - 1
    'End Sub
    Private Sub Textedit11_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit11.Enter
        If TextEdit9.EditValue = "1" And CDec(TextEdit11.EditValue) = 0 Then
            TextEdit11.EditValue = CDec(TextEdit10.EditValue)
        End If
    End Sub
    Private Sub TextEdit8_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit8.LostFocus
        If Val(TextEdit8.EditValue) < 1 Or Val(TextEdit8.EditValue) > NUMRAT Then
            TextEdit8.EditValue = 1
        End If
        POSRAT = Val(TextEdit8.EditValue) - 1
        RwX = GridView8.GetDataRow(POSRAT)
        PopolaDettagli()
    End Sub
    Private Sub TextEdit9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit9.EditValueChanged
        If TextEdit9.EditValue <> "1" And TextEdit9.EditValue <> "0" Then
            TextEdit9.EditValue = "0"
        End If
    End Sub

    Private Sub TextEdit14_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit14.LostFocus
        'If Val(Numbox15.EditValue) = 0 Then TextEdit14.EditValue= 1
        If POSRAT > -1 Then AggiornaTabella()
    End Sub
    Sub AggiornaTabella()
        RwScT = DsScT.Rows(POSRAT)
        RwScT("ScaDSca") = DateEdit1.EditValue
        RwScT("ScaImpRata") = CDec(TextEdit10.EditValue)
        RwScT("ScaImpPagato") = CDec(TextEdit11.EditValue)
        RwScT("ScaAbi") = Val(TextEdit12.EditValue)
        RwScT("ScaCab") = Val(TextEdit13.EditValue)
        RwScT("ScaBan") = Val(TextEdit14.EditValue)
        RwScT("ScaRAperta") = TextEdit9.EditValue
        UN = POSRAT
        If (POSRAT + 2) <= NUMRAT Then
            TextEdit8.EditValue = (POSRAT + 2)
            POSRAT = Val(TextEdit8.EditValue) - 1
            RwX = GridView8.GetDataRow(POSRAT)
            PopolaDettagli()
        Else
            ButtonF11.Focus()
        End If
    End Sub

   
End Class