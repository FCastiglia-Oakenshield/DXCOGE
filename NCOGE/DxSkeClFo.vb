Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Public Class DxSkeClFo
    Dim DsRip As DataSet
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow
    Dim StrTot, Dap, Alp As String
    Dim Esponi As Boolean
    Dim Rpt As ReportClass
    Dim Rpt1 As New SkeAlfaConti
    Dim Rpt2 As New SkeAlfaPagina
    Dim Singolo As Boolean
    Dim Sw As Int16 = 0
    Dim TipoStampa As Int16
    Dim MiglioFo, Irow As Int32
    Dim UltimaApertura As Int16
    Dim StrUno, StrDue, StrTre, StrPrint, LIMITI(6), StrD(10), StrLim, LimD, LimA As String
    Private Sub DxSkeClFo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If Sw = 0 Then
            PrimoMiglio()
            Sw = 1
        End If
        Pulizia()
    End Sub
    Sub PrimoMiglio()
        Dim Cmd As New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT ISNULL(max(YEAR(pridatagio)),0) FROM Tbpri Where PriCausale = 45", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            UltimaApertura = dataRd.Item(0)
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT top 1 * from TbEse Order by EseAnno desc", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TipoStampa = dataRd.Item("EseFormato")
        End While
        dataRd.Close()
        LIMITI(1) = "000.00"
        LIMITI(2) = "101000"
        LIMITI(3) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(4) = "099.99"
        LIMITI(5) = "1" & MiglioFo.ToString.PadLeft(5, "0")
        LIMITI(6) = "199999"
        StrD(0) = "SELECT DISTINCT PRKTIPOCO,PRKCONTO,PRKDESC,SUM(DARE) as TDARE,sum(AVERE) as TAVERE ,(SUM(DARE)-SUM(AVERE)) AS TSALDO ,max(PRKAAMMGG) as FINOAL,min(PRKAAMMGG) as DAL "
        StrD(1) = "SELECT * ,0.00 as Tsaldo "
        StrD(2) = " FROM VH8 WHERE PRKTIPOCO+PRKCONTO BETWEEN "
        StrD(3) = " AND PRKAAMMGG BETWEEN "
        StrD(4) = " FROM VH8 WHERE PRKCONTO = "
        StrD(5) = " GROUP BY PRKTIPOCO,PRKCONTO,PRKDESC ORDER BY PRKTIPOCO,PRKCONTO"
        StrD(6) = "SELECT DISTINCT PRKTIPOCO,PRKCONTO,PRKDESC,(SUM(DARE)-SUM(AVERE)) AS TSALDO FROM VH8 WHERE PRKTIPOCO+PRKCONTO BETWEEN "
        StrD(7) = "SELECT DISTINCT PRKTIPOCO,PRKCONTO,PRKDESC,PIAFL,0.00 AS TSALDO FROM VH8 WHERE PRKTIPOCO+PRKCONTO BETWEEN "
        StrD(8) = " GROUP BY PRKTIPOCO,PRKCONTO,PRKDESC,PIAFL"
        StrD(9) = "  "
        StrD(10) = " order by prkTipoCo,PrkConto,PRKAAMMGG,PriregIva desc,PriNumProt"
        DateEdit1.EditValue = CDate("01/01/" & Today.Year).ToShortDateString
        DateEdit2.EditValue = CDate(Date.DaysInMonth(Today.Year, Today.Month) & "/" & Today.Month & "/" & Today.Year).ToShortDateString
    End Sub
    Sub DisBottoni()
        ButtonF5.Enabled = True
        ButtonF9.Enabled = True
    End Sub
    Sub Pulizia()
        DisBottoni()
        CheckEdit1.Checked = False
        CheckEdit2.Checked = False
        RadioGroup1.SelectedIndex = 0
        StrLim = ""
        Singolo = False
        FormaStringaTesta()
    End Sub
    Function VerificaData() As Boolean
        VerificaData = True
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        style = MsgBoxStyle.Critical
        If DateEdit1.EditValue > DateEdit2.EditValue Then
            response = MsgBox("DATA INIZIO > DATA FINE", style, "PERIODO DAL ... AL ...")
            VerificaData = False
            Exit Function
        End If
    End Function
    Function FormaStringaTesta() As Boolean
        FormaLimitiStandard()
        StrLim = "'" & LimD & "' AND '" & LimA & "'"
        StrUno = StrD(0) & StrD(2) & StrLim & StrD(3) & "'" & DateEdit1.EditValue & "' AND '" & DateEdit2.EditValue & "'"
        StrUno = StrUno & StrD(5)
        FormaStringaTesta = True
    End Function
    Sub FormaLimitiStandard()
        If RadioGroup1.SelectedIndex = 0 Then
            LimD = LIMITI(2)
            LimA = LIMITI(5)
        Else
            LimD = LIMITI(3)
            LimA = LIMITI(6)
        End If
    End Sub
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If VerificaData() = False Then Exit Sub
        Dim frm As New LpDs
        Rpt = New ReportClass
        Rpt1 = New SkeAlfaConti
        Rpt2 = New SkeAlfaPagina
        StrPrint = StrD(1) & StrD(2) & StrLim & StrD(3) & "'" & CDate(DateEdit1.EditValue).ToShortDateString & "' AND '" & CDate(DateEdit2.EditValue).ToShortDateString & "' " & StrD(10)
        If CheckEdit1.Checked = True Then
            Rpt = Rpt2
        Else
            Rpt = Rpt1
        End If
        Cursor.Current = Cursors.WaitCursor
        DsRip = New DataSet("H8")
        DaRip = New SqlDataAdapter(StrPrint, cnCo)
        DaRip.SelectCommand.CommandTimeout = 300
        DaRip.Fill(DsRip, "H8")
        If CheckEdit2.Checked = True Then SommaRiporti()
        Rpt.SetDataSource(DsRip.Tables("H8"))
        Rpt.SetParameterValue("periodo", "DAL " & DateEdit1.EditValue & " AL " & DateEdit2.EditValue)
        Rpt.SetParameterValue("Marchio", Marchio)
        Rpt.SetParameterValue("CONTOPAGINA", CheckEdit1.Checked)
        Rpt.SetParameterValue("DATERIPORTO", Dap & " - " & Alp)

        If TipoStampa = 1 Then
            PdfStart(Rpt, Me.Text())
            Exit Sub
        End If
        frm.reportsource = Rpt
        frm.Text = Me.Text
        frm.Show()
    End Sub
    Sub SommaRiporti()
        Dim Tt As String = "Tot"
        Dim DsTot As DataSet
        Dim DaTot As SqlDataAdapter
        Dim RwTot As DataRow

        If UltimaApertura = CDate(DateEdit1.EditValue).Year Then
            If CDate(DateEdit1.EditValue).Day = 1 And CDate(DateEdit1.EditValue).Month = 1 Then
                Esponi = False
            Else
                Dap = CDate("01/01/" & CDate(DateEdit1.EditValue).Year).ToShortDateString
                Alp = CDate(DateEdit1.EditValue).AddDays(-1).ToShortDateString
                Esponi = True
            End If
        Else
            Dap = CDate("01/01/" & UltimaApertura).ToShortDateString
            Alp = CDate(DateEdit1.EditValue).AddDays(-1).ToShortDateString
            Esponi = True
        End If

        If Esponi = False Then Exit Sub
        StrTot = StrD(6) & "'" & LimD & "' AND '" & LimA & "'" & StrD(3) & "'" & Dap & "' AND '" & Alp & "'"
        StrTot = StrTot & StrD(9) & StrD(8)
        Cursor.Current = Cursors.WaitCursor
        DsTot = New DataSet
        DaTot = New SqlDataAdapter(StrTot, cnCo)
        DaTot.SelectCommand.CommandTimeout = 300
        DaTot.Fill(DsTot, Tt)
        Dim S0 As String
        Dim Rs0 As DataRow()
        Dim y, x As Int32
        For y = 1 To DsTot.Tables(Tt).Rows.Count
            RwTot = DsTot.Tables(Tt).Rows(y - 1)
            S0 = "PrkConto = '" & RwTot("PrkConto").ToString & "'"
            Rs0 = DsRip.Tables("H8").Select(S0)
            For x = 0 To Rs0.Length - 1
                Rs0(x).Item("Tsaldo") = RwTot("Tsaldo")
            Next
        Next
        DsRip.Tables("H8").AcceptChanges()
    End Sub
    Private Sub DateEdit_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit1.Enter, DateEdit2.Enter
        DisBottoni()
    End Sub
    Private Sub RadioGroup1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        FormaStringaTesta()
    End Sub
End Class