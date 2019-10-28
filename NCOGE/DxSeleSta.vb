Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.IO
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Public Class DxSeleSta
    Dim DsRip As DataSet
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow
    Dim StrTot, Dap, Alp As String
    Dim Esponi As Boolean
    Dim Rpt As ReportClass
    Dim Rpt1 As New SkeConti
    Dim Rpt2 As New SkePagina
    Dim Rpt3 As New SkePSaldo
    Dim descri As String
    Dim Singolo As Boolean
    Dim Sw As Int16 = 0
    Dim TipoStampa As Int16
    Dim MiglioFo, Irow As Int32
    Dim UltimaApertura As Int16
    Dim StrUno, StrDue, StrTre, StrPrint, LIMITI(6), StrD(10), StrLim, LimD, LimA As String
    Private Sub DxSeleSta_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
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
        CheckEdit3.Checked = False
        CheckEdit1.Checked = False
        CheckEdit2.Checked = False

        TextEdit1.EditValue = ""
        TextEdit2.EditValue = ""
        TextEdit3.EditValue = ""
        TextEdit4.EditValue = ""
        StrLim = ""
        Singolo = False
        TextEdit1.Focus()
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
        StrLim = "'" & LimD & "' AND '" & LimA & "'"
        StrUno = StrD(0) & StrD(2) & StrLim & StrD(3) & "'" & DateEdit1.EditValue & "' AND '" & DateEdit2.EditValue & "'"
        StrUno = StrUno & StrD(5)
        FormaStringaTesta = True
    End Function

    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            ButtonF8.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub TextEdit1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.LostFocus
        If Mid(TextEdit1.EditValue, 3, 1) = "." And Val(Mid(TextEdit1.EditValue, 1, 2)) > 0 And Val(Mid(TextEdit1.EditValue, 4, 2)) Then
            StrTre = "SELECT PIAANACO FROM TBPIA WHERE PIACODCO = '" & TextEdit1.EditValue & "'"
            LIMITI(0) = "0" & TextEdit1.EditValue
            LEGGI()
            TextEdit3.EditValue = descri
        Else
            If Val(TextEdit1.EditValue) > 1000 And Val(TextEdit1.EditValue) < MiglioFo Then
                TextEdit1.EditValue = TextEdit1.EditValue.PadLeft(5, "0")
                StrTre = "SELECT ANADESC FROM  " & DbVdox.Trim & ".DBO.TBANA WHERE ANAGRP = 'CL' AND ANACOD = '" & TextEdit1.EditValue & "'"
                LIMITI(0) = "1" & TextEdit1.EditValue
                LEGGI()
                TextEdit3.EditValue = descri
            Else
                If Val(TextEdit1.EditValue) > MiglioFo Then
                    TextEdit1.EditValue = TextEdit1.EditValue.PadLeft(5, "0")
                    StrTre = "SELECT ANADESC FROM  " & DbVdox.Trim & ".DBO.TBANA WHERE ANAGRP = 'FO' AND ANACOD = '" & TextEdit1.EditValue & "'"
                    LIMITI(0) = "1" & TextEdit1.EditValue
                    LEGGI()
                    TextEdit3.EditValue = descri
                End If

            End If
        End If
        DisBottoni()
    End Sub
    Private Sub TextEdit2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit2.LostFocus
        Singolo = False
        If Mid(TextEdit2.EditValue, 3, 1) = "." And Val(Mid(TextEdit2.EditValue, 1, 2)) > 0 And Val(Mid(TextEdit2.EditValue, 4, 2)) Then
            StrTre = "SELECT PIAANACO FROM TBPIA WHERE PIACODCO = '" & TextEdit2.EditValue & "'"
            LIMITI(1) = "0" & TextEdit2.EditValue
            LEGGI()
            TextEdit4.EditValue = descri
        Else
            If Val(TextEdit2.EditValue) > 1000 And Val(TextEdit2.EditValue) < MiglioFo Then
                TextEdit2.EditValue = TextEdit2.EditValue.PadLeft(5, "0")
                StrTre = "SELECT ANADESC FROM  " & DbVdox.Trim & ".DBO.TBANA WHERE ANAGRP = 'CL' AND ANACOD = '" & TextEdit2.EditValue & "'"
                LIMITI(1) = "1" & TextEdit2.EditValue
                LEGGI()
                TextEdit4.EditValue = descri
            Else
                If Val(TextEdit2.EditValue) > MiglioFo Then
                    TextEdit2.EditValue = TextEdit2.EditValue.PadLeft(5, "0")
                    StrTre = "SELECT ANADESC FROM  " & DbVdox.Trim & ".DBO.TBANA WHERE ANAGRP = 'FO' AND ANACOD = '" & TextEdit2.EditValue & "'"
                    LIMITI(1) = "1" & TextEdit2.EditValue
                    LEGGI()
                    TextEdit4.EditValue = descri
                End If

            End If
        End If
        DisBottoni()
    End Sub
    Sub LEGGI()
        descri = ""
        Dim Cmd As New SqlCommand(StrTre, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            descri = dataRd.GetString(0)
        End While
        dataRd.Close()
    End Sub
    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If VerificaData() = False Then Exit Sub
        Dim frm As New LpDs
        Rpt = New ReportClass
        Rpt1 = New SkeConti
        Rpt2 = New SkePagina
        Rpt3 = New SkePSaldo
        StrLim = "'" & LIMITI(0) & "' AND '" & LIMITI(1) & "'"
        StrPrint = StrD(1) & StrD(2) & StrLim & StrD(3) & "'" & DateEdit1.EditValue & "' AND '" & DateEdit2.EditValue & "' " & StrD(10)
        If CheckEdit3.Checked = True Then
            Rpt = Rpt3
        ElseIf CheckEdit1.Checked = True Then
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
        StrTot = StrD(6) & "'" & LIMITI(0) & "' AND '" & LIMITI(1) & "'" & StrD(3) & "'" & Dap & "' AND '" & Alp & "'"
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

    Private Sub TEXTEDIT1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit1.Enter, DateEdit1.Enter, DateEdit2.Enter
        DisBottoni()
    End Sub
    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        FormaStringaTesta()
    End Sub
    '''''''''''''''''RICERCA CONTO CLI FOR SOTTOCONTO'''''''''''''''''''''
    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        If TextEdit1.ContainsFocus = True Then
            Dim Nc As String = ""
            Nc = EstraiRicerca(TextEdit1.EditValue.ToUpper)
            If Nc > "00.00" Then
                TextEdit1.EditValue = Nc
                LeggiConto(TextEdit1.EditValue, TextEdit3.EditValue)
                SelectNextControl(TextEdit1, True, True, True, True)
            End If
            Exit Sub
        End If
        If TextEdit2.ContainsFocus = True Then
            Dim Nc As String = ""
            Nc = EstraiRicerca(TextEdit2.EditValue.ToUpper)
            If Nc > "00.00" Then
                TextEdit2.EditValue = Nc
                LeggiConto(TextEdit2.EditValue, TextEdit4.EditValue)
                SelectNextControl(TextEdit2, True, True, True, True)
            End If
            Exit Sub
        End If
    End Sub
    Function LeggiConto(ByRef CodCo As String, ByRef Anagraf As String) As Boolean
        Anagraf = ""
        LeggiConto = False
        AggiustaConto(CodCo) ''''' verifica il punto se e' un sottoconto
        Dim Str As String = "SELECT * from TbPia where PiaCodCo = '" & CodCo & "'"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Anagraf = dataRd("PiaAnaCo")
            LeggiConto = True
        End While
        dataRd.Close()
        If LeggiConto = True Then Exit Function
        If Val(CodCo) < 1000 Then Exit Function
        CodCo = CodCo.PadLeft(5, "0")
        Str = "SELECT * from TbAna where AnaCoD = '" & CodCo & "'"
        Cmd = New SqlCommand(Str, cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            Anagraf = dataRd("AnaDesc")
            LeggiConto = True
        End While
        dataRd.Close()
    End Function
    Function AggiustaConto(ByRef CodCo As String) As Boolean
        Dim x As Int16
        For x = 1 To Len(CodCo)
            If Mid(CodCo, x, 1) = "." Then
                CodCo = Format(Val(Mid(CodCo, 1, x - 1)), "00") & "." & Format(Val(Mid(CodCo, x + 1, Len(CodCo) - (x - 1))), "00")
                Exit Function
            End If
        Next
    End Function
    Function EstraiRicerca(ByVal Tipo As String) As String
        Dim frm As New RicercaClFo
        Dim CF As String = ""
        If Tipo <> "F" And Tipo <> "C" Then
            EstraiRicerca = Query.CercaPia()
            Exit Function
        End If
        If Tipo = "F" Then CF = "FO"
        If Tipo = "C" Then CF = "CL"
        frm.StartPosition = FormStartPosition.Manual
        frm.Location = New Point(GroupControl2.Location.X, GroupControl2.Location.Y + 80)
        frm.CliFor = CF
        frm.ShowDialog()
        EstraiRicerca = frm.Codice
    End Function
    Private Sub CheckEdit3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit3.CheckedChanged, CheckEdit1.CheckedChanged
        If CheckEdit3.Checked = True Then CheckEdit1.Checked = True
    End Sub
End Class