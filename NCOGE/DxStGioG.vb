Imports DXBASE
Imports NPRINT
Imports NCCOM
Imports System.Data.SqlClient
Imports System.IO
Public Class DxStGioG
    Dim Sw As Int16 = 0

    Dim TIPOLP(), RESETLP() As String
    Dim DataAl, DateG(4) As Date
    Dim UART As Int32
    Dim PS As Int16
    Dim RIPO As Decimal = 0
    Dim PP As Int32 = 0
    Dim AZI As String = ""
    Dim LIBRO As String = ""
    Dim AnnoE As Int16 = 0

    Dim Tr As String = "TGio"
    Dim DsReg As DataSet
    Dim DaReg As SqlDataAdapter
    Dim RwReg As DataRow

    Private Sub DxStGioG_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Sw = 0 Then
            Popolaprinter()
            Sw = 1
        End If
    End Sub
    Private Sub Popolaprinter()
        ComboBoxEdit3.Properties.Items.Clear()
        ComboBoxEdit3.SelectedIndex = -1
        Dim x As Int16 = -1
        PS = -1
        Dim Cmd = New SqlCommand("SELECT * from TbLaser where LasTipo = 'A' or LasTipo = 'F' or LasTipo = 'L'", cnVd)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            x = x + 1
            ComboBoxEdit3.Properties.Items.Add(dataRd.Item("LasFile"))
            ReDim Preserve TIPOLP(x), RESETLP(x)
            TIPOLP(x) = dataRd.Item("LasTipo")
            RESETLP(x) = dataRd.Item("LasReset")
            If dataRd.Item("LasDEFAULT") = "D" And ComboBoxEdit3.SelectedIndex = -1 Then ComboBoxEdit3.SelectedIndex = x
        End While
        dataRd.Close()
        REM Date Limite giornale
        Dim str(2) As String
        str(0) = "Select ISNULL(MAX(PRIDATAGIO),'01/01/2000') from TBPRI WHERE PRIgSTAMPA = 1 "
        str(1) = "Select ISNULL(MIN(PRIDATAGIO),'01/01/2000') from TBPRI WHERE PRIgSTAMPA = 0 "

        For x = 0 To 1
            Cmd = New SqlCommand(str(x), cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                DateG(x) = dataRd.Item(0)
            End While
            dataRd.Close()
        Next
        DateEdit2.EditValue = DateG(0)
        str(2) = "Select * from TBESE WHERE '" & DateG(1).ToShortDateString & "' between EseDal and EseAl"
        Cmd = New SqlCommand(str(2), cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            DateG(2) = dataRd.Item("EseAl")
        End While
        dataRd.Close()
        DateEdit1.EditValue = DateG(2)
        CheckButton3.Checked = False
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        If Verificagiornale() = False Then
            Exit Sub
        End If
        Cursor.Current = Cursors.WaitCursor
        If ComboBoxEdit3.SelectedIndex = -1 And CheckButton3.Checked = False Then
            ComboBoxEdit3.Focus()
            Exit Sub
        End If
        PreparaDati()
        SparaStampa()
        Me.Close()
    End Sub
    Function PreparaDati() As Boolean
        Dim d1 As String
        d1 = DateEdit1.EditValue.ToShortDateString
        EsegueSql(" EXEC XH11  @FAL ='" & d1 & "' ", cnCo)
    End Function
    Function Verificagiornale() As Boolean
        Verificagiornale = True
        DataAl = DateEdit1.EditValue.ToShortDateString
        Dim Messaggio As String = ""
        If DataAl < DateG(0) Then
            Messaggio = "DATA INFERIORE ULTIMA DATA STAMPATA  " & Chr(13) _
                      & DataAl & " < " & DateG(0) & Chr(13)
            Verificagiornale = False
        End If
        'If DataAl.Year <> DateG(1).Year Then
        '    Messaggio = "ANNO ESERCIZIO DIVERSO DATE DI STAMPA  " & Chr(13) _
        '              & "( Dal " & DateG(1) & " al " & DataAl & ") " & DataAl.Year & " <> " & DateG(1).Year & Chr(13)
        '    Verificagiornale = False
        'End If
        If DataAl < DateG(1) Then
            Messaggio = Messaggio & "DATA INFERIORE PRIMA DATA DI STAMPA  " & Chr(13) _
                      & DataAl & " < " & DateG(1)
            Verificagiornale = False
        End If
        If Messaggio > "" Then
            MsgBox(Messaggio, MsgBoxStyle.Critical, "STAMPA GIORNALE")
        Else
            '''' articoloMax

            Dim StrStr As String = "Select * from TBESE WHERE '" & DataAl.ToShortDateString & "' between EseDal and EseAl"
            Cmd = New SqlCommand(StrStr, cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                RIPO = dataRd.Item("EseGioProg")
                PP = dataRd.Item("EseGioNFog")
                LIBRO = dataRd.Item("EseGioDesc")
                If CBool(dataRd.Item("EseGioInt")) = False Then AZI = ""
                DateG(3) = dataRd.Item("EseDal")
                DateG(4) = dataRd.Item("Eseal")
                AnnoE = dataRd.Item("EseAnno")
            End While
            dataRd.Close()
            StrStr = "Select * from TBAZI WHERE AZIANNOLAVORO =" & DataAl.Year
            Cmd = New SqlCommand(StrStr, cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                AZI = dataRd.Item("AziCod")
            End While
            dataRd.Close()
            StrStr = "Select ISNULL(MAX(PriArtFisc),0) from TBPRI WHERE PRIGSTAMPA = 1 AND PriDataGio between '" & DateG(3) & "' and ' " & DateG(4) & "'"
            Cmd = New SqlCommand(StrStr, cnCo)
            dataRd = Cmd.ExecuteReader
            While dataRd.Read
                UART = dataRd.Item(0)
            End While
            dataRd.Close()
        End If
    End Function

    Function SparaStampa() As Boolean
        Dim NOMESTAMPA As String = ""
        Dim Periodo As String = DateG(3).Year
        If DateG(3).Year <> DateG(4).Year Then Periodo = DateG(3).Year & "-" & DateG(4).Year
        If CheckButton3.Checked = True Then
            GiornalePdf.PrintGiornale(CheckEdit2.Checked, CheckEdit1.Checked, UART, RIPO, AnnoE, PP, LIBRO, AZI, NOMESTAMPA, CheckEdit3.Checked, Periodo)
            If NOMESTAMPA > "" Then StampaInPdf(NOMESTAMPA)
        Else
            GiornaleAghi.PrintGiornale(ComboBoxEdit3.Text, CheckEdit2.Checked, CheckEdit1.Checked, TIPOLP(PS), RESETLP(PS), UART, RIPO, AnnoE, PP, LIBRO, AZI, Periodo)
        End If
        If TIPOLP(PS) = "F" And CheckButton3.Checked = False Then
            EsegueAnteprima(ComboBoxEdit3.Text)
        End If
    End Function
    Private Sub Base_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            e.Handled = True
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub
    Private Sub ComboBoxEdit3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit3.SelectedIndexChanged
        PS = ComboBoxEdit3.SelectedIndex
    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateEdit2.EditValueChanged
        DateEdit2.EditValue = DateG(0)
    End Sub

    Private Sub CheckButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckButton3.CheckedChanged
        If CheckButton3.Checked = True Then
            ComboBoxEdit3.Enabled = False
            CheckButton3.Text = "STAMPA GIORNALE IN FORMATO PDF"
            CheckButton3.ImageIndex = 62
            CheckEdit3.Enabled = True
        Else
            CheckButton3.Text = "STAMPA GIORNALE IN FORMATO NORMALE"
            CheckButton3.ImageIndex = 107
            ComboBoxEdit3.Enabled = True
            CheckEdit3.Enabled = False
        End If
    End Sub

    Private Sub DateEdit1_Leave(sender As Object, e As System.EventArgs) Handles DateEdit1.Leave
        If DateEdit1.EditValue > DateG(2) Then DateEdit1.EditValue = DateG(2)
    End Sub
End Class