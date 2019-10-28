Imports DXBASE
Imports System.Data.SqlClient

Public Class DxSaldaP

    Dim AnaCod As String
    Dim DsPno As DataSet

    Public Property PDsPno() As DataSet
        Get
            Return DsPno
        End Get
        Set(ByVal Value As DataSet)
            DsPno = Value
        End Set
    End Property
    Public Property PAnaCod() As String
        Get
            Return AnaCod
        End Get
        Set(ByVal Value As String)
            AnaCod = Value
        End Set
    End Property

    Dim Cb As String = "CMov"
    Dim DsCbo As DataSet
    Dim DaCbo As SqlDataAdapter
    Dim RwCbo As DataRow


    Dim Ts As String = "TSCO"
    Dim DsSco As DataSet
    Dim DaSco As SqlDataAdapter
    Dim RwSco As DataRow

    Dim Pn As String = "NOTA"
    Dim DaPno As SqlDataAdapter
    Dim RwPno As DataRow

    Dim StrTot, Dap, Alp, CF As String
    Dim Esponi, OkM As Boolean
    Dim MaxDat, MinDat, MMMDat As Date
    Dim Rispondi As MsgBoxResult

    Dim SSPLIT As String

    Dim Sw As Int16 = 0
    Dim MiglioFo, Corp, Scor, QRIGA, Iset4 As Int32
    Dim StrDue, StrTre, LIMITI(6), StrD(10), LimD, LimA As String

    Dim RwY As DataRow


    Private Sub DxSaldaP_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonFF5.Click
        If Sw = 0 Then
            PrimoMiglio()
            Sw = 1
        End If
        Pulizia()
        TextEdit20.EditValue = PAnaCod
        Anagrafica()
        EseguoOperazione()
    End Sub
    Sub PrimoMiglio()
        Dim Cmd As New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        LIMITI(1) = "000.00"
        LIMITI(2) = "101000"
        LIMITI(3) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(4) = "099.99"
        LIMITI(5) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(6) = "199999"
        StrD(0) = "SELECT DISTINCT PRKTIPOCO,PRKCONTO,PRKDESC"
        StrD(2) = " FROM VB8 WHERE PARTITARIO = 1 "
        StrD(3) = " AND PRKTIPOCO+PRKCONTO BETWEEN "
        StrD(5) = " GROUP BY PRKTIPOCO,PRKCONTO,PRKDESC ORDER BY PRKTIPOCO,PRKCONTO"
        StrD(1) = " AND PRKPAPERTA = 0 "
        StrD(4) = " SELECT *,00.0 as TSCOPERTO FROM VB8 WHERE PRKCONTO = "
        StrD(8) = " ORDER BY PRKTIPOCO,PRKCONTO,PRKPAPERTA,PRKDOCANN,PRKDOCEST,PRIDATAEST"
        StrD(7) = "SELECT * FROM VB8 WHERE PARTITARIO = 1 AND PRKTIPOCO+PRKCONTO BETWEEN "
        StrD(9) = "SELECT * FROM VB8SC WHERE PRKCONTO = "
        StrD(10) = " ORDER BY PRKDOCANN,PRKDOCEST"
    End Sub
    Sub PPulisci()
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = ""
        TextEdit6.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = CDec(0.0)
        ButtonXF11.Enabled = False
        TextEdit8.Properties.ReadOnly = True
    End Sub
    Sub PulisciGrid()
        DsCbo = New DataSet
        GridControl2.DataSource = DsCbo.Tables(Cb)
        GridControl2.Refresh()
        DsSco = New DataSet
        GridControl3.DataSource = DsSco.Tables(Ts)
        GridControl3.Refresh()
    End Sub
    Sub PulisciPNota()
        REM inizializzo il dataset Vuoto
        Dim StrS = StrD(9) & "'999999'" & StrD(10)
        DsPno = New DataSet
        DaPno = New SqlDataAdapter(StrS, cnCo)
        DaPno.Fill(DsPno, Pn)
        GridControl4.DataSource = DsPno.Tables(Pn)
        GridControl4.Refresh()
        TextEdit1.EditValue = CDec(0.0)
    End Sub
    Sub Pulizia()
        PulisciGrid()
        PulisciPNota()
        PPulisci()
        ButtonFF11.Enabled = False
        CF = ""
        Corp = 0
        Scor = 0
        OkM = False
        TextEdit8.ErrorText = ""
        TextEdit20.Focus()
    End Sub
    Sub PopolaCORPO()
        Dim x, Nd, An As Int32
        Dim SD, SA, TS, TD, TA As Decimal
        Dim RwTsa As DataRow
        Nd = 0
        An = 0
        TD = 0
        TA = 0
        DsCbo = New DataSet
        DaCbo = New SqlDataAdapter(StrDue, cnCo)
        DaCbo.SelectCommand.CommandTimeout = 300
        DaCbo.Fill(DsCbo, Cb)
        Corp = DsCbo.Tables(Cb).Rows.Count
        If Corp = 0 Then
            GridControl2.DataSource = DsCbo.Tables(Cb)
            GridView2.ClearSelection()
            GridControl2.Refresh()
            Exit Sub
        End If
        For x = 1 To Corp - 1
            RwCbo = DsCbo.Tables(Cb).Rows(x - 1)
            SD = SD + RwCbo("DARE")
            SA = SA + RwCbo("AVERE")
            TD = TD + RwCbo("DARE")
            TA = TA + RwCbo("AVERE")
            RwTsa = DsCbo.Tables(Cb).Rows(x)
            If RwCbo("PrkDocEst") <> RwTsa("PrkDocEst") Or RwCbo("PrkDocAnn") <> RwTsa("PrkDocAnn") Then
                TS = SD - SA
                RwCbo("TSCOPERTO") = TS
                SD = 0
                SA = 0
                TS = 0
            End If
        Next
        RwCbo = DsCbo.Tables(Cb).Rows(x - 1)
        SD = SD + RwCbo("DARE")
        SA = SA + RwCbo("AVERE")
        TS = SD - SA
        RwCbo("TSCOPERTO") = TS
        TD = TD + RwCbo("DARE")
        TA = TA + RwCbo("AVERE")
        RwCbo = DsCbo.Tables(Cb).NewRow()
        RwCbo("PrkTipoCo") = "9"
        RwCbo("PrkDocAnn") = "0"
        RwCbo("PrkAst") = ""
        RwCbo("PrkPAperta") = 0
        RwCbo("DARE") = TD
        RwCbo("AVERE") = TA
        RwCbo("TSCOPERTO") = TD - TA
        DsCbo.Tables(Cb).Rows.Add(RwCbo)
        DsCbo.AcceptChanges()
        Dim Rrow As Int32
        Rrow = DsCbo.Tables(Cb).Rows.Count
        GridControl2.DataSource = DsCbo.Tables(Cb)
        GridView2.ClearSelection()
        GridControl2.Refresh()
        GridView2.FocusedRowHandle = Rrow - 1
        GridView2.SelectRow(Rrow - 1)
    End Sub

    Sub EseguoOperazione()
        PulisciGrid()
        Corp = 0
        Cursor.Current = Cursors.WaitCursor
        CaricaScoperti()
        FormaStringaCorpo()
        PopolaCORPO()
        Cursor.Current = Cursors.Default
    End Sub
    Sub CaricaScoperti()
        Dim StrS = StrD(9) & "'" & TextEdit20.EditValue & "'" & StrD(10)
        DsSco = New DataSet
        DaSco = New SqlDataAdapter(StrS, cnCo)
        DaSco.SelectCommand.CommandTimeout = 300
        DaSco.Fill(DsSco, Ts)
        Scor = DsSco.Tables(Ts).Rows.Count
        GridControl3.DataSource = DsSco.Tables(Ts)
        GridControl3.Refresh()
        GridView3.ClearSelection()
    End Sub

    Sub FormaStringaCorpo()
        StrDue = StrD(4) & "'" & TextEdit20.EditValue & "'" & StrD(1) & StrD(8)
        LimD = "1" & TextEdit20.EditValue
        LimA = LimD
    End Sub
    Sub Anagrafica()
        TextEdit20.EditValue = TextEdit20.EditValue.ToString.PadLeft(5, "0")
        If Val(TextEdit20.EditValue) > 1000 And Val(TextEdit20.EditValue) < MiglioFo Then
            StrTre = "SELECT ANADESC FROM  " & DbVdox.Trim & ".DBO.TBANA WHERE ANAGRP = 'CL' AND ANACOD = '" & TextEdit20.EditValue & "'"
            LIMITI(0) = "1" & TextEdit20.EditValue
            LEGGI()
        ElseIf Val(TextEdit20.EditValue) > MiglioFo Then
            StrTre = "SELECT ANADESC FROM  " & DbVdox.Trim & ".DBO.TBANA WHERE ANAGRP = 'FO' AND ANACOD = '" & TextEdit20.EditValue & "'"
            LIMITI(0) = "1" & TextEdit20.EditValue
            LEGGI()
        Else
            Me.Close()
            Exit Sub
        End If
    End Sub
    Sub LEGGI()
        Dim Cmd As New SqlCommand(StrTre, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TextEdit21.EditValue = dataRd.GetString(0)
        End While
        dataRd.Close()
    End Sub

    Private Sub RepositoryItemCheckEdit3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemCheckEdit3.CheckedChanged
        RwY = GridView3.GetFocusedDataRow
        RwY("PrkPAperta") = sender.checked
        Totalizza()
    End Sub
    Sub Totalizza()
        Dim x, z As Int16
        z = -1
        TextEdit8.ErrorText = ""
        For x = 1 To DsPno.Tables(Pn).Rows.Count
            RwPno = DsPno.Tables(Pn).Rows(x - 1)
            If RwPno("PrkConto") = RwY("PrkConto") And RwPno("PrkDocAnn") = RwY("PrkDocAnn") And RwPno("PrkDocEst") = RwY("PrkDocEst") Then
                z = x - 1
                Exit For
            End If
        Next
Dopo:
        If z = -1 Then
            RwPno = DsPno.Tables(Pn).NewRow()
            RwPno("PrkConto") = RwY("PrkConto")
            RwPno("PrkDocAnn") = RwY("PrkDocAnn")
            RwPno("PrkDocEst") = RwY("PrkDocEst")
            RwPno("PrkPAperta") = RwY("PrkPAperta")
            RwPno("AnaDesc") = RwY("AnaDesc")
            RwPno("SCOPERTO") = RwY("SCOPERTO")
            DsPno.Tables(Pn).Rows.Add(RwPno)
            DsPno.AcceptChanges()
            TextEdit4.EditValue = RwPno("PrkConto")
            TextEdit5.EditValue = RwPno("AnaDesc")
            TextEdit6.EditValue = RwPno("PrkDocAnn")
            TextEdit7.EditValue = RwPno("PrkDocEst")
            TextEdit8.EditValue = RwPno("SCOPERTO")
            TextEdit8.Properties.ReadOnly = False
            ButtonXF11.Enabled = True
            If CF = "" Then
                If RwPno("PrkConto") > MiglioFo Then CF = "F" Else CF = "C"
            End If
            If CF = "F" Then ControllaImportoFo()
            GoTo Oltre
        End If
        If RwY("PrkPAperta") = False Then
            RwPno.Delete()
            DsPno.AcceptChanges()
            PPulisci()
        End If
Oltre:  TotaleIn()
    End Sub
    Sub ControllaImportoFo()
        Dim Str = "SELECT * from VRITNETTO WHERE RITCODFOR = '" & RwPno("PrkConto") & "' AND RITPROTFAT = " & RwPno("PrkDocEst") & " AND DATEPART(YEAR,RITDATAFAT) = " & RwPno("PrkDocAnn")
        Dim NETTO As Decimal = 0
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            NETTO = dataRd.Item("NETTO") * -1
        End While
        dataRd.Close()
        If NETTO < 0 And CDec(TextEdit8.EditValue) < NETTO Then
            TextEdit8.EditValue = NETTO
            RwPno("SCOPERTO") = NETTO
            TextEdit8.ErrorText = "IMPORTO AL NETTO DELLA RITENUTA! "
        Else
            TextEdit8.ErrorText = ""
        End If
    End Sub
    Sub TotaleIn()
        Dim x As Int16
        Dim Progress As Decimal = 0
        For x = 1 To DsPno.Tables(Pn).Rows.Count
            Rw = GridView4.GetDataRow(x - 1)
            Progress = Progress + CDec(Rw("SCOPERTO"))
        Next
        GridView4.FocusedRowHandle = DsPno.Tables(Pn).Rows.Count - 1
        TextEdit1.EditValue = CDec(Progress)
        If TextEdit8.Properties.ReadOnly = False Then TextEdit8.Focus()
        If CDec(TextEdit1.EditValue) <> 0 Then
            ButtonFF11.Enabled = True
        Else
            ButtonFF11.Enabled = False
        End If
    End Sub

    Private Sub GridControl4_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl4.MouseMove
        ShowHitInfo4(GridView4.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo4(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl4.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset4 = hi.RowHandle
    End Sub
    Private Sub GridView4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.Click
        QRIGA = -1
        If Iset4 >= 0 And Iset4 < DsPno.Tables(Pn).Rows.Count Then
            RwPno = DsPno.Tables(Pn).Rows(Iset4)
            EliminaCheck()
            RwPno.Delete()
            DsPno.AcceptChanges()
            PPulisci()
            TotaleIn()
        End If
    End Sub
    Sub EliminaCheck()
        Dim X As Int16
        For X = 1 To DsSco.Tables(Ts).Rows.Count
            RwY = GridView3.GetDataRow(X - 1)
            If RwPno("PrkConto") = RwY("PrkConto") And RwPno("PrkDocAnn") = RwY("PrkDocAnn") And RwPno("PrkDocEst") = RwY("PrkDocEst") Then
                RwY("PrkpAperta") = False
                Exit Sub
            End If
        Next
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonFF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonFF11.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub ButtonFF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF11.Click
        OkM = True
        Me.Close()
    End Sub
    Private Sub ButtonXF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonXF11.Click
        If ModificaImporto() = False Then TextEdit8.Focus()
    End Sub
    Function ModificaImporto() As Boolean
        ModificaImporto = True
        If CDec(TextEdit8.EditValue) = 0 Then
            Exit Function
        End If
        If (RwPno("Scoperto") > 0 And CDec(TextEdit8.EditValue) < 0) Or (RwPno("Scoperto") < 0 And CDec(TextEdit8.EditValue) > 0) Then
            ModificaImporto = False
            Exit Function
        End If
        If Math.Abs(CDec(TextEdit8.EditValue)) > Math.Abs(RwPno("Scoperto")) Then
            ModificaImporto = False
            Exit Function
        End If
        RwPno("Scoperto") = CDec(TextEdit8.EditValue)
        DsPno.Tables(Pn).AcceptChanges()
        PPulisci()
        TotaleIn()
    End Function
    Private Sub DxSaldaP_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If OkM = False Then PulisciPNota()
    End Sub
End Class