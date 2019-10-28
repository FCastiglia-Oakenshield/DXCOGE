Imports DXBASE
Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.XtraEditors

Public Class LDPSigla
    Private Shared LastTbUFa As DataTable
    Private Shared TbUFa As DataTable
    Private Shared Sigla As CheckedComboBoxEdit
    Private Shared Locat As Point
    Private Shared Ret As Boolean
    Public Shared Property PRet() As Boolean
        Get
            Return Ret
        End Get
        Set(ByVal Value As Boolean)
            Ret = Value
        End Set
    End Property

    Public Shared Property PLastTbUFa() As DataTable
        Get
            Return LastTbUFa
        End Get
        Set(ByVal Value As DataTable)
            LastTbUFa = Value
        End Set
    End Property

    Public Shared Property PTbUFa() As DataTable
        Get
            Return TbUFa
        End Get
        Set(ByVal Value As DataTable)
            TbUFa = Value
        End Set
    End Property

    Public Shared Property PSigla() As CheckedComboBoxEdit
        Get
            Return Sigla
        End Get
        Set(ByVal Value As CheckedComboBoxEdit)
            Sigla = Value
        End Set
    End Property

    Public Shared Property PLocat() As Point
        Get
            Return Locat
        End Get
        Set(ByVal Value As Point)
            Locat = Value
        End Set
    End Property


    Dim Em As New DevExpress.XtraEditors.Controls.CheckedListBoxItem
    Dim DSCPT As DataTable
    Dim DACPT As SqlDataAdapter
    Dim RWCPT As DataRow

    Dim RwEdit As DataRow
    Dim NrCom As ArrayList
    Dim DeCom As ArrayList
    Dim ProgId As Integer = -1
    Dim TabM As String = "TbMcc"
    Dim TabI As String = "TbIMc"
    Dim Exc As String = "XLDPRIP"
    Dim Exu As String = "XLDPCREA"
    Dim TOTALEIMP As Decimal = 0

    Private Sub LDPSigla_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        GroupControl2.Visible = False
        GroupControl18.Visible = True
        XtraTabControl1.SelectedTabPageIndex = 0
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = New Point(3, 72)
        CaricaRett()
        CaricaCheck()
    End Sub
    Sub CaricaRett()
        If Ret = True Then
            TabM = "TrMcc"
            TabI = "TrIMc"
            Exc = "RLDPRIP"
            Exu = "RLDPCREA"
        End If
    End Sub
    Sub CaricaCheck()
        CheckedComboBoxEdit1.Properties.Items.Clear()
        For x As Int16 = 1 To Sigla.Properties.Items.Count
            Em = New DevExpress.XtraEditors.Controls.CheckedListBoxItem(Sigla.Properties.Items(x - 1).Value, Sigla.Properties.Items(x - 1).Description, Sigla.Properties.Items(x - 1).CheckState)
            CheckedComboBoxEdit1.Properties.Items.Add(Em)
        Next
        CheckedComboBoxEdit1.EditValue = Sigla.EditValue
        ButtonEdit2.Focus()
    End Sub

    Private Sub ButtonEdit2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ButtonEdit2.MouseMove
        ButtonEdit2.Focus()
        If CheckedComboBoxEdit1.EditValue IsNot Nothing AndAlso CheckedComboBoxEdit1.EditValue > "" Then
            ButtonEdit2.Properties.Buttons(1).ToolTip = ""
            ButtonEdit2.Properties.Buttons.Item(1).Enabled = True
        Else
            ButtonEdit2.Properties.Buttons.Item(1).Enabled = False
            ButtonEdit2.Properties.Buttons(1).ToolTip = "MANCA LDP"
        End If
    End Sub
    Private Sub ButtonEdit2_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ButtonEdit2.ButtonClick
        Cursor.Current = Cursors.WaitCursor
        Select Case e.Button.Index
            Case 0
                CaricaCheck()
            Case 1
                If CheckedComboBoxEdit1.EditValue IsNot Nothing AndAlso CheckedComboBoxEdit1.EditValue > "" Then
                    RICaricaCheck()
                End If
        End Select
        Cursor.Current = Cursors.Default
        ButtonEdit1.Focus()
    End Sub
    Private Sub ButtonEdit1_MouseMove(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEdit1.MouseMove
        ButtonEdit1.Focus()
        ButtonEdit1.Properties.Buttons.Item(1).Enabled = ControllaTotali()
    End Sub
    Sub RegistraFatturaDcg()
        If ControllaTotali() = False Then ButtonEdit1.Focus() : Exit Sub
        ScriviMCC()
        Me.Close()
    End Sub
    Function ControllaTotali() As Boolean
        Dim T1, T2 As Decimal
        Dim PP As Integer = -1
        Dim Conto As String = ""
        Dim Mail As String = ""
        For x As Int16 = 1 To DSCPT.Rows.Count
            RWCPT = DSCPT.Rows(x - 1)
            If x = 1 Then Conto = RWCPT("PRKCONTO") : T2 = RWCPT("IMPORTO") : PP = RWCPT("PRKPROG")
            If Conto = RWCPT("PRKCONTO") And PP = RWCPT("PRKPROG") Then
                T1 += RWCPT("MCCIMPORTO")
            Else
                If T1 <> T2 Then
                    Mail &= Conto & " DIFFERENZA  " & Format(T1 - T2, "c") & Chr(10)
                End If
                Conto = RWCPT("PRKCONTO") : T2 = RWCPT("IMPORTO") : T1 = RWCPT("MCCIMPORTO") : PP = RWCPT("PRKPROG")
            End If
        Next
        If T1 <> T2 Then
            Mail &= Conto & " DIFFERENZA  " & Format(T1 - T2, "c") & Chr(10)
        End If
        ButtonEdit1.Properties.Buttons(1).ToolTip = Mail
        If Mail.Length > 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Sub EliminaMcc()
        Dim Cancella As String = "Delete from " & TabM & " where MCCID = " & ProgId
        Dim Dmd As New SqlCommand(Cancella, CnDc)
        Dmd.ExecuteNonQuery()
    End Sub
    Private Function LeggiUltimo(ByVal DataOdierna As Date) As Boolean
        Dim ultimo As String = "INSERT INTO " & TabI & "  (IDmcdt) values(@Oggi)"
        Dim UltimaRiga As New SqlCommand("SELECT  @@IDENTITY ", CnDc)
        Dim Qmd As New SqlCommand(ultimo, CnDc)
        Dim px As New SqlParameter("@Oggi", SqlDbType.SmallDateTime)
        px.Value = CDate(DataOdierna)
        Qmd.Parameters.Add(px)
        Qmd.ExecuteNonQuery()
        ProgId = UltimaRiga.ExecuteScalar
    End Function
    Sub ScriviMCC()
        Dim Scrivi As String = "INSERT INTO " & TabM & "  (MCCID,MCCPROG,MCCPrkAammgg,MCCPrkId,MCCPrkProg,MCCPrkDa,MCCCogLdp,MCCCogCdc,MCCCogRep,MCCCogDet,MCCCogConto,MCCIMPORTO) " _
& " values(@MCCID,@MCCPROG,@MCCPrkAammgg,@MCCPrkId,@MCCPrkProg,@MCCPrkDa,@MCCCogLdp,@MCCCogCdc,@MCCCogRep,@MCCCogDet,@MCCCogConto,@MCCIMPORTO)"
        Dim Mmd As New SqlCommand(Scrivi, CnDc)
        Dim p1 As New SqlParameter("@MCCID", SqlDbType.Int)
        Dim p2 As New SqlParameter("@MCCPROG", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@MCCPrkAammgg", SqlDbType.SmallDateTime)
        Dim p4 As New SqlParameter("@MCCPrkId", SqlDbType.Int)
        Dim p5 As New SqlParameter("@MCCPrkProg", SqlDbType.SmallInt)
        Dim p6 As New SqlParameter("@MCCPrkDa", SqlDbType.VarChar)
        Dim p7 As New SqlParameter("@MCCCogLdp", SqlDbType.SmallInt)
        Dim p8 As New SqlParameter("@MCCCogCdc", SqlDbType.VarChar)
        Dim p9 As New SqlParameter("@MCCCogRep", SqlDbType.SmallInt)
        Dim p9a As New SqlParameter("@MCCCogDet", SqlDbType.SmallInt)
        Dim p10 As New SqlParameter("@MCCCogConto", SqlDbType.VarChar)
        Dim p11 As New SqlParameter("@MCCIMPORTO", SqlDbType.Decimal)
        Dim Oggi As String = Today.ToShortDateString

        RWCPT = DSCPT.Rows(0)
        ProgId = RWCPT("MCCID")
        Dim Cancella As String = "Delete from " & TabM & " where MCCPRKID = " & RWCPT("PRKID")
        Dim Dmd As New SqlCommand(Cancella, CnDc)
        Dmd.ExecuteNonQuery()

        If ProgId = 0 Then
            LeggiUltimo(Oggi)
        Else
            EliminaMcc()
        End If
        Dim x, M As Int16
        M = 0
        For x = 1 To DSCPT.Rows.Count
            RWCPT = DSCPT.Rows(x - 1)
            Mmd.Parameters.Clear()
            If RWCPT("MCCIMPORTO") <> 0 Then ''And RWCPT("LDPRIF") > 0 Then
                M = M + 1
                p1.Value = ProgId
                p2.Value = M
                p3.Value = RWCPT("PrkAammgg")
                p4.Value = RWCPT("PrkId")
                p5.Value = RWCPT("PrkProg")
                p6.Value = RWCPT("PrkDa")
                p7.Value = RWCPT("LdpRif")
                p8.Value = ""
                p9.Value = 0
                p9a.Value = 0
                p10.Value = RWCPT("PRKCONTO")
                p11.Value = RWCPT("MCCIMPORTO")
                Mmd.Parameters.Add(p1)
                Mmd.Parameters.Add(p2)
                Mmd.Parameters.Add(p3)
                Mmd.Parameters.Add(p4)
                Mmd.Parameters.Add(p5)
                Mmd.Parameters.Add(p6)
                Mmd.Parameters.Add(p7)
                Mmd.Parameters.Add(p8)
                Mmd.Parameters.Add(p9)
                Mmd.Parameters.Add(p9a)
                Mmd.Parameters.Add(p10)
                Mmd.Parameters.Add(p11)
                Mmd.ExecuteNonQuery()
            End If
        Next
        ResetIdP()
        EliminaCommesse()
    End Sub
    Sub ResetIdP()
        Dim Elimina As String = "DELETE FROM " & TabI & "  WHERE IdMCC = " & ProgId
        Dim Dmd As New SqlCommand(Elimina, CnDc)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub RICaricaCheck()
        Rw = TbUFa.Rows(0)
        Dim Id As Integer = Rw("PrkId")
        EsegueSql("EXEC " & Exc & "  @ID=" & Id, CnDc)
        Dim Str As String = "SELECT *,TOT=(SELECT SUM(IMPORTO) FROM ##TABTO) FROM ##TABCM ORDER BY PRKCONTO"
        DSCPT = New DataTable()
        DACPT = New SqlDataAdapter(Str, CnDc)
        DACPT.Fill(DSCPT)
        If VerificaNrCommesse() = True Then ScriviMCC() : Me.Close() : Exit Sub
        RiassegnaCommesse()
        Rw = DSCPT.Rows(0)
        TOTALEIMP = Rw("TOT")
        GridControl4.DataSource = DSCPT

        'Me.Width = 500 : Me.Height = 400
        'Me.CenterToParent()
        'Me.Width = 400
        'Dim H As Integer = (GridView5.RowCount + 4) * 32 + 36
        'If H < 300 Then H = 300
        'Me.Height = H
        'If Me.Width > 900 Then Me.Width = 900
        'Me.CenterToScreen()
        GroupControl18.Visible = False
        GroupControl2.Visible = True
    End Sub
    Sub EliminaCommesse()
        Dim Elimina As String = "DELETE FROM TMPTCM where TMPID = " & Rw("PrkId")
        Dim Dmd As New SqlCommand(Elimina, CnDc)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub RiassegnaCommesse()
        If Sigla.EditValue = CheckedComboBoxEdit1.EditValue And PLastTbUFa.Rows.Count = PTbUFa.Rows.Count Then
            EsegueSql("EXEC  " & Exc & " @ID=" & Rw("PrkId"), CnDc)
            DSCPT = New DataTable()
            DACPT = New SqlDataAdapter("SELECT *,TOT=(SELECT SUM(IMPORTO) FROM ##TABTO) FROM ##TABCM ORDER BY PRKCONTO", CnDc)
            DACPT.Fill(DSCPT)
            Exit Sub
        End If
        EliminaCommesse()
        Dim Inser As String = "INSERT INTO TMPTCM (TMPID,LDPRIF,LDPSIGLA) values(@ID,@RIF,@SIGLA)"
        Dim iCmd As New SqlCommand(Inser, CnDc)
        Dim T1 As New SqlParameter("@ID", SqlDbType.Int)
        Dim T2 As New SqlParameter("@RIF", SqlDbType.SmallInt)
        Dim T3 As New SqlParameter("@SIGLA", SqlDbType.VarChar)
        For X As Int16 = 1 To NrCom.Count
            T1.Value = Rw("PrkId")
            T2.Value = NrCom(X - 1)
            T3.Value = DeCom(X - 1)
            iCmd.Parameters.Clear()
            iCmd.Parameters.Add(T1)
            iCmd.Parameters.Add(T2)
            iCmd.Parameters.Add(T3)
            iCmd.ExecuteNonQuery()
        Next
        ProgId = 0
        For i As Int16 = 1 To DSCPT.Rows.Count
            RWCPT = DSCPT.Rows(i - 1)
            If RWCPT("MCCID") > 0 Then ProgId = RWCPT("MCCID") : EliminaMcc()
        Next

        Dim Id As Integer = Rw("PrkId")
        EsegueSql("EXEC " & Exu & " @ID=" & Id & ",@MCC= " & ProgId, CnDc)
        Dim Str As String = "SELECT *,TOT=(SELECT SUM(IMPORTO) FROM ##TABTO) FROM ##TABCM ORDER BY PRKCONTO"
        DSCPT = New DataTable()
        DACPT = New SqlDataAdapter(Str, CnDc)
        DACPT.Fill(DSCPT)
    End Sub

    Function VerificaNrCommesse() As Boolean
        NrCom = New ArrayList
        DeCom = New ArrayList
        For i As Int16 = 1 To CheckedComboBoxEdit1.Properties.Items.Count
            Em = CheckedComboBoxEdit1.Properties.Items(i - 1)
            If Em.CheckState = CheckState.Checked Then
                NrCom.Add(Em.Value)
                DeCom.Add(Em.Description)
            End If
        Next
        If NrCom.Count > 1 Then Return False : Exit Function
        Cmd = New SqlCommand("SELECT MCCID,MCCIMPORTO,PRKAAMMGG,PRKID,PRKDA,PRKCONTO,LDPRIF,LDPSIGLA,CONTODES,IMPORTO,TOT=(SELECT SUM(IMPORTO) FROM ##TABTO) FROM ##TABCM ORDER BY PRKCONTO", CnDc)
        Dim QV As Integer = 0
        Dim EV As Integer = 0
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            QV += 1
        End While
        dataRd.Close()
        For I As Int16 = 1 To DSCPT.Rows.Count
            EV += 1
            RWCPT = DSCPT.Rows(I - 1)
            If EV <= QV Then
                RWCPT("LDPRIF") = NrCom(0)
                RWCPT("MCCIMPORTO") = RWCPT("IMPORTO")
            Else
                RWCPT.Delete()
            End If
        Next
        DSCPT.AcceptChanges()
        Return True
    End Function
    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ButtonEdit1.ButtonClick
        Cursor.Current = Cursors.WaitCursor
        Select Case e.Button.Index
            Case 0
                RICaricaCheck()
            Case 1
                RegistraFatturaDcg()
        End Select
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub GridView5_ShowingEditor(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles GridView5.ShowingEditor
        RwEdit = GridView5.GetFocusedDataRow
    End Sub

    Private Sub GridView5_ValidatingEditor(sender As Object, e As DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs) Handles GridView5.ValidatingEditor
        'If e.Value > RwEdit("IMPORTO") Then e.Valid = False
    End Sub
    Private Sub GridView5_CustomSummaryCalculate(sender As Object, e As DevExpress.Data.CustomSummaryEventArgs) Handles GridView5.CustomSummaryCalculate
        e.TotalValue = TOTALEIMP
    End Sub

End Class