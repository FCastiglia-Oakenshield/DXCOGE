Imports NPRINT
Imports System.Data.SqlClient
Imports DXBASE
Imports NCCOM
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraReports.UI

Public Class DxBsConti
    Dim REPORT As New XtraReport
    Dim selectformula, SCRI As String
    Dim DsCespM As DataTable
    Dim DaCespM As SqlDataAdapter

    Dim DsCes As DataSet
    Dim DaCes As SqlDataAdapter
    Dim CbCes As SqlCommandBuilder
    Dim RwCes As DataRow
    Dim Rx As DataRowView
    Dim Ces As String = "Ces"
    Dim TesTest, TesTestGr, TesTestSp, MaxRig, How As Int16
    Dim Iset As Integer
    Dim i As Int16 = -1
    Dim specie2 As String
    Dim NewGruppo, NewSpecie As Boolean

   
    Dim where As Object

    Private Sub DxBsConti_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Pulizia(True)
        pulisciGrid()
        TextBox1.Focus()
    End Sub

    Private Sub PopolaGrid()
        Dim str As String
        str = "SELECT * FROM TbCsp where CspGru='" & TextBox1.EditValue.Trim & "' And CspSpe1 = '" & TextBox2.EditValue.Trim & "' And CspSpe2 = '" & specie2 & "' and CspNum <> '00'"
        DaCes = New SqlDataAdapter(str, cnCo)
        DsCes = New DataSet(Ces)
        DaCes.Fill(DsCes, Ces)
        CbCes = New SqlCommandBuilder(DaCes)
        If Not TextBox20.EditValue = "00.00" Then TextBox21.EditValue = LeggiCpt(TextBox20.EditValue)
        If Not TextBox22.EditValue = "00.00" Then TextBox23.EditValue = LeggiCpt(TextBox22.EditValue)
        If Not TextBox24.EditValue = "00.00" Then TextBox25.EditValue = LeggiCpt(TextBox24.EditValue)
        If Not TextBox26.EditValue = "00.00" Then TextBox27.EditValue = LeggiCpt(TextBox26.EditValue)
        GridControl1.DataSource = DsCes.Tables(Ces)
        GridControl1.Refresh()
        GridView1.UnselectRow(0)
        GridView1.ClearSelection()
    End Sub

    Private Sub Pulizia(ByVal Puliscitutto As Boolean)
        TextBox5.EditValue = ""
        TextBox6.EditValue = CDec(0.0)
        TextBox4.EditValue = ""
        CheckBox1.Checked = False
        TextBox20.EditValue = "00.00"
        TextBox21.EditValue = ""
        TextBox22.EditValue = "00.00"
        TextBox23.EditValue = ""
        TextBox24.EditValue = "00.00"
        TextBox25.EditValue = ""
        TextBox26.EditValue = "00.00"
        TextBox27.EditValue = ""
        NewGruppo = False
        NewSpecie = False
        Iset = -1
        If Puliscitutto = True Then
            GroupBox3.Enabled = False
            GroupBox4.Enabled = False
            GroupBox2.Enabled = True
            TextBox8.EditValue = ""
            TextBox7.EditValue = ""
            TextBox2.EditValue = ""
            TextBox3.EditValue = ""
            TextBox1.EditValue = ""
        End If
    End Sub


    Private Function ControlloCodice(ByVal cod As String) As Boolean
        ControlloCodice = True
        If cod.Trim = "" Then
            Return False
        End If
        If Not IsNumeric(cod) Then
            Return False
        End If
    End Function

    Private Sub PuliziaGr(ByVal Puliscitutto As Boolean)
        TextBox3.EditValue = ""
        TextBox2.EditValue = ""
        TextBox7.EditValue = ""
        TextBox8.EditValue = ""
        If Puliscitutto = True Then
            TextBox1.EditValue = ""
        End If
    End Sub

    Private Sub PuliziaSp(ByVal Puliscitutto As Boolean)
        TextBox8.EditValue = ""
        If Puliscitutto = True Then
            TextBox2.EditValue = ""
            TextBox3.EditValue = ""
        End If
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If controllo() = False Then
            Exit Sub
        End If
        AggiornoTes()
        If Esiste() = False Then
            DsCes.Tables(Ces).Rows.Add(RwCes)
            Iset = DsCes.Tables(Ces).Rows.Count - 1
        End If
        DaCes.Update(DsCes, Ces)
        DsCes.AcceptChanges()
        Iset = -1
        Pulizia(False)
        TextBox4.Focus()
    End Sub

    Private Function controllo() As Boolean
        controllo = True
        If TextBox5.EditValue = "" Then
            TextBox5.Focus()
            Return False
        End If
        ''If Not IsNumeric(TextBox6.EditValue) Then
        ''    TextBox6.Focus()
        ''    Return False
        ''End If
        If Val(TextBox6.EditValue) > 100 Then
            TextBox6.Focus()
            Return False
        End If
    End Function

    Private Sub AggiornoTes()
        RwCes("CspGru") = TextBox1.EditValue
        RwCes("CspSpe1") = TextBox2.EditValue
        If Not IsNumeric(TextBox3.EditValue) Then
            RwCes("CspSpe2") = 0
        Else
            RwCes("CspSpe2") = TextBox3.EditValue
        End If
        RwCes("CspNum") = TextBox4.EditValue
        RwCes("CspDesc") = TextBox5.EditValue
        RwCes("CspPerc") = CDec(TextBox6.EditValue)
        RwCes("CspCp") = CheckBox1.Checked
        RwCes("CspCespiti") = TextBox20.EditValue
        RwCes("CspFondoAmm") = TextBox24.EditValue
        RwCes("CspQuotaNormale") = TextBox22.EditValue
        RwCes("CspQuotaAnticipata") = TextBox26.EditValue
    End Sub

    Private Function Esiste() As Boolean
        Iset = -1
        For i As Int16 = 1 To GridView1.RowCount
            Rx = GridView1.GetRow(i - 1)
            If Rx("CspNum") = TextBox4.EditValue Then
                GridView1.SelectRow(i - 1)
                Iset = i - 1
                Exit For
            End If
        Next
        If Iset > -1 Then
            RwCes = DsCes.Tables(Ces).Rows(Iset)
            Return True
        End If
        Return False
    End Function

    Private Sub AzzeradataRow()
        RwCes = DsCes.Tables(Ces).NewRow()
        RwCes("CspGru") = TextBox1.EditValue
        RwCes("CspSpe1") = TextBox2.EditValue
        RwCes("CspSpe2") = Format(TextBox3.EditValue, "0")
        RwCes("CspNum") = "00"
        RwCes("CspDesc") = ""
        RwCes("CspPerc") = 0
        RwCes("CspCp") = 0
    End Sub

    Private Sub TextBox3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox8.GotFocus
        If ControlloCodice2(TextBox2.EditValue) = False Then
            TextBox2.Focus()
            Exit Sub
        End If
        If TextBox3.EditValue = "" Then
            specie2 = 0
        Else
            specie2 = TextBox3.EditValue
        End If
        LetturaSpe1(TextBox1.EditValue, TextBox2.EditValue, specie2)
        PopolaGrid()
    End Sub

    Private Sub LetturaGr(ByVal Gru As String)
        Dim str As String
        str = "SELECT * from TbCsp where CspGru = '" & Gru & "' and CspSpe1='00' and CspSpe2='0' and CspNum='00'"
        Dim cmd As New SqlCommand(str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextBox7.EditValue = dataRd.Item("CspDesc")
            NewGruppo = False
            dataRd.Close()
            Exit Sub
        End If
        NewGruppo = True
        dataRd.Close()
    End Sub

    Private Sub LetturaSpe1(ByVal Gru As String, ByVal Spe1 As String, ByVal Spe2 As String)
        Dim str As String
        str = "SELECT * from TbCsp where CspGru = '" & Gru & "' and CspSpe1='" & Spe1 & "' and CspSpe2='" & Spe2 & "' and CspNum='00'"
        Dim cmd As New SqlCommand(str, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            TextBox8.EditValue = dataRd.Item("CspDesc")
            NewSpecie = False
            dataRd.Close()
            Exit Sub
        End If
        NewSpecie = True
        TextBox8.EditValue = ""
        dataRd.Close()
    End Sub

    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        If controllo1() = False Then
            Exit Sub
        End If
        scriviGr()
        scriviSp()
        AccettaCespiti()
        PopolaGrid()
    End Sub

    Private Function controllo1() As Boolean
        controllo1 = True
        If Not IsNumeric(TextBox1.EditValue) Then
            Return False
        End If
        If TextBox1.EditValue.Trim = "" Then
            TextBox1.Focus()
            Return False
        End If
        If TextBox2.EditValue.Trim = "" Then
            TextBox2.Focus()
            Return False
        End If
        If TextBox7.EditValue.Trim = "" Then
            TextBox7.Focus()
            Return False
        End If
        If TextBox8.EditValue.Trim = "" Then
            TextBox8.Focus()
            Return False
        End If
    End Function

    Private Sub AccettaCespiti()
        GroupBox3.Enabled = True
        GroupBox4.Enabled = True
        GroupBox2.Enabled = False
    End Sub

    Private Sub scriviGr()
        Dim Str As String
        If NewGruppo = True Then
            Str = "Insert Into TbCsp (CspGru,CspSpe1,CspSpe2,CspNum,CspDesc,CspPerc,CspCp) values (@CspGru,'00','0','00',@CspDesc,0,1)"
            NewGruppo = False
        Else
            Str = "update TbCsp set CspGru=@CspGru,CspDesc=@CspDesc where cspgru=@cspgru and cspspe1='00' and CspSpe2='0' and CspNum='00'"
        End If
        Dim cmd As New SqlCommand(Str, cnCo)
        Dim p1 As New SqlParameter("@CspGru", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@CspDesc", SqlDbType.VarChar)
        p1.Value = TextBox1.EditValue
        p2.Value = TextBox7.EditValue
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub AggiornoTesGr()
        RwTes("CspGru") = TextBox1.EditValue
        RwTes("CspSpe1") = "00"
        RwTes("CspSpe2") = 0
        RwTes("CspNum") = "00"
        RwTes("CspDesc") = TextBox7.EditValue
        RwTes("CspPerc") = 0
        RwTes("CspCp") = True
    End Sub

    Private Sub scriviSp()
        Dim Str As String
        If NewSpecie = True Then
            Str = "Insert Into TbCsp (CspGru,CspSpe1,CspSpe2,CspNum,CspDesc,CspPerc,CspCp) values (@CspGru,@CspSpe1,@CspSpe2,'00',@CspDesc,0,1)"
            NewSpecie = False
        Else
            Str = "update TbCsp set CspGru=@CspGru,CspDesc=@CspDesc where cspgru=@cspgru and cspspe1=@CspSpe1 and CspSpe2=@CspSpe2 and CspNum='00'"
        End If
        Dim cmd As New SqlCommand(Str, cnCo)
        Dim p1 As New SqlParameter("@CspGru", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@CspDesc", SqlDbType.VarChar)
        Dim p3 As New SqlParameter("@CspSpe1", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@CspSpe2", SqlDbType.VarChar)
        p1.Value = TextBox1.EditValue
        p2.Value = TextBox8.EditValue
        p3.Value = TextBox2.EditValue
        p4.Value = TextBox3.EditValue.Trim.ToString.PadLeft(1, "0")
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        cmd.Parameters.Add(p3)
        cmd.Parameters.Add(p4)
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub AggiornoTesSp()
        RwTes("CspGru") = TextBox1.EditValue
        RwTes("CspSpe1") = TextBox2.EditValue
        If TextBox3.EditValue = "" Then
            RwTes("CspSpe2") = 0
        Else
            RwTes("CspSpe2") = TextBox3.EditValue
        End If
        RwTes("CspNum") = "00"
        RwTes("CspDesc") = TextBox8.EditValue
        RwTes("CspPerc") = 0
        RwTes("CspCp") = True
        specie2 = TextBox3.EditValue
    End Sub

    Private Sub ButtonF5b_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5b.Click
        PuliziaGr(True)
        pulisciGrid()
        TextBox1.Focus()
    End Sub

    Private Sub ButtonF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF2.Click
        GroupBox3.Enabled = False
        GroupBox4.Enabled = False
        GroupBox2.Enabled = True
        pulisciGrid()
        Pulizia(False)
        TextBox8.Focus()
    End Sub

    Private Function ControlloCodice2(ByVal cod As String) As Boolean
        ControlloCodice2 = True
        If TextBox1.EditValue = "" Then Return True
        If cod.Trim = "" Then
            Return False
        End If
        If Not IsNumeric(cod) Or Val(cod.Trim) = 0 Then
            Return False
        End If
    End Function

    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        PopolaGrid()
        Pulizia(False)
        TextBox4.Focus()
    End Sub

    Private Sub pulisciGrid()
        DsCes = New DataSet(Ces)
        GridControl1.DataSource = DsCes.Tables(Ces)
        GridControl1.Refresh()
    End Sub
    Private Sub Lettura(ByVal CspNum As String)
        Cmd = New SqlCommand("select * from VCspCpt where CspGru ='" & TextBox1.EditValue & "' and CspSpe1 ='" & TextBox2.EditValue & "' and cspspe2='" & TextBox3.EditValue.ToString.PadLeft(1, "0") & "' and cspnum = '" & CspNum & "'", cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            TextBox4.EditValue = dataRd.Item("CspNum")
            TextBox5.EditValue = dataRd.Item("CspDesc")
            TextBox6.EditValue = CDec(dataRd.Item("CspPerc"))
            CheckBox1.Checked = CBool(dataRd.Item("CspCp"))
            TextBox20.EditValue = dataRd.Item("CspCespiti")
            TextBox21.EditValue = dataRd.Item("CptCespiti")
            TextBox24.EditValue = dataRd.Item("CspFondoAmm")
            TextBox25.EditValue = dataRd.Item("CptFondoAmm")
            TextBox22.EditValue = dataRd.Item("CspQuotaNormale")
            TextBox23.EditValue = dataRd.Item("CptQuotaNormale")
            TextBox26.EditValue = dataRd.Item("CspQuotaAnticipata")
            TextBox27.EditValue = dataRd.Item("CptQuotaAnticipata")
        End If
        dataRd.Close()
    End Sub

    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        If TextBox4.EditValue > "" AndAlso TextBox5.EditValue > "" Then
            DelRiga()
        End If
    End Sub

    Private Sub DelRiga()
        Dim box As Object
        box = MessageBox.Show("Vuoi eliminare la riga selezionata?", "ELIMINA RIGA", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
        If box = DialogResult.No Then
            Exit Sub
        End If
        If TextBox3.EditValue.ToString.Trim = "" Then TextBox3.EditValue = 0
        Cmd = New SqlCommand("delete from TbCsp where CspGru ='" & TextBox1.EditValue & "' and CspSpe1 ='" & TextBox2.EditValue & "' and Cspspe2 ='" & TextBox3.EditValue & "' and cspnum='" & TextBox4.EditValue & "'", cnCo)
        Cmd.ExecuteNonQuery()
        PopolaGrid()
        Pulizia(False)
        TextBox4.Focus()
    End Sub

    '''Private Sub TextBox6_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox6.LostFocus
    '''    If CDec(IIf(TextBox6.EditValue = "", CDec(0.0), TextBox6.EditValue)) > 100 Then
    '''        TextBox6.Focus()
    '''        Exit Sub
    '''    End If
    '''    TextBox6.EditValue = Format(CDec(IIf(TextBox6.EditValue = "", CDec(0.0), TextBox6.EditValue)), "##0.00")
    '''End Sub

    '''Private Sub txtCod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '''    If e.KeyChar = "." Then
    '''        For I As Int16 = 0 To TextBox6.EditValue.Length - 1
    '''            If TextBox6.EditValue.Chars(I) = "," Then
    '''                e.Handled = True
    '''                Return
    '''            End If
    '''        Next
    '''        If TextBox6.EditValue.Length > 0 Then TextBox6.SelectedText = ","
    '''        e.Handled = True
    '''    End If
    '''End Sub

    Private Sub TextBox8_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox8.LostFocus
        If Not IsNumeric(TextBox8.EditValue) Or TextBox8.EditValue <> "" Then
            If TesTestSp > 0 Then
                TextBox8.Visible = False
            End If
        End If
    End Sub
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset = hi.RowHandle
    End Sub

    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If Iset > -1 Then
            Rx = GridView1.GetRow(Iset)
            RwCes = DsCes.Tables(Ces).Rows(Iset)
            TextBox4.EditValue = Rx("CspNum")
            Lettura(TextBox4.EditValue)
            TextBox4.Focus()
            TextBox5.Focus()
        End If
    End Sub

    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            If GroupBox2.Enabled = True Then ButtonF5b.PerformClick() Else ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 Then
            If GroupBox2.Enabled = True Then ButtonF3b.PerformClick() Else ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F1 Then
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F2 Then
            ButtonF2.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            ButtonF8.PerformClick()
            ButtonF8b.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F9 Then
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub TextBox1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.Enter
        Pulizia(True)
    End Sub

    Private Sub ButtonF3b_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3b.Click
        If TextBox8.EditValue > "" Then
            DelSpecie()
            TextBox2.EditValue = ""
            TextBox3.EditValue = ""
            TextBox8.EditValue = ""
            GridControl1.DataSource = Nothing
            TextBox2.Focus()
        ElseIf TextBox1.EditValue > "" Then
            DelGruppo()
            Pulizia(True)
            GridControl1.DataSource = Nothing
            TextBox1.Focus()
        End If
    End Sub

    Private Sub DelSpecie()
        Dim box As Object
        box = MessageBox.Show("Vuoi eliminare la specie selezionata?", "ELIMINA SPECIE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)

        If box = DialogResult.No Then
            Exit Sub
        End If
        If TextBox3.EditValue.Trim = "" Then TextBox3.EditValue = 0
        Cmd = New SqlCommand("delete from TbCsp where CspGru ='" & TextBox1.EditValue & "' and CspSpe1 ='" & TextBox2.EditValue & "' and Cspspe2 ='" & TextBox3.EditValue & "'", cnCo)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub DelGruppo()
        Dim box As Object
        box = MessageBox.Show("Vuoi eliminare la Gruppo selezionato e le Specie associate?", "ELIMINA GRUPPO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)

        If box = DialogResult.No Then
            Exit Sub
        End If
        If TextBox3.EditValue.Trim = "" Then TextBox3.EditValue = 0
        Cmd = New SqlCommand("delete from TbCsp where CspGru ='" & TextBox1.EditValue & "'", cnCo)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub TextBox7_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox7.Leave
        If Not TextBox7.EditValue > "" Then TextBox1.Focus()
    End Sub

    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        ButtonF8.Focus()
        Dim WsProg2 As String
        WsProg2 = Query.CercaCespite(TextBox1.EditValue, TextBox2.EditValue, TextBox3.EditValue)
        If WsProg2 > "" And WsProg2.Length = 6 Then
            TextBox1.EditValue = WsProg2.Substring(0, 3).ToString
            TextBox2.EditValue = WsProg2.Substring(3, 2).ToString
            TextBox3.EditValue = WsProg2.Substring(5, 1).ToString
            LetturaGr(TextBox1.EditValue)
            LetturaSpe1(TextBox1.EditValue, TextBox2.EditValue, specie2)
            PopolaGrid()
            TextBox8.Focus()
        Else
            WsProg2 = ""
            TextBox1.Focus()
        End If
    End Sub

    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Dim Gruppo As String
        Dim Specie As String
        If TextBox3.EditValue <> "" Then
            Specie = TextBox2.EditValue & " " & TextBox3.EditValue
        Else
            Specie = TextBox2.EditValue
        End If

        Gruppo = TextBox1.EditValue & " " & TextBox7.EditValue
        Specie = Specie & " " & TextBox8.EditValue

        Dim StrPrint As String = "select * from TbCsp where CspGru ='" & TextBox1.EditValue & "' and CspSpe1 ='" & TextBox2.EditValue & "' and cspspe2='" & TextBox3.EditValue.ToString.PadLeft(1, "0") & "' and CspNum > 0 "

        DsCespM = New DataTable
        DaCespM = New SqlDataAdapter(StrPrint, cnCo)
        DaCespM.SelectCommand.CommandTimeout = 300
        DaCespM.Fill(DsCespM)
        selectformula = ""
        REPORT = New DxStaCespMin
        REPORT.DataSource = DsCespM
        REPORT.DataMember = "DsCespM"
        REPORT.FilterString = selectformula
        REPORT.Parameters.Item("Marchio").Value = Marchio()
        REPORT.Parameters.Item("Gruppo").Value = Gruppo
        REPORT.Parameters.Item("Specie").Value = Specie
        REPORT.ShowPreview()
    End Sub

    Private Sub TextBox20_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox20.LostFocus
        If Val(Mid(TextBox20.EditValue, 1, 2)) > 0 AndAlso Val(Mid(TextBox20.EditValue, 4, 2)) = 0 Then TextBox21.EditValue = "" : TextBox20.EditValue = "00.00"
        If Not TextBox20.EditValue = "00.00" Then TextBox21.EditValue = LeggiCpt(TextBox20.EditValue)
    End Sub
    Private Sub TextBox22_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox22.LostFocus
        If Val(Mid(TextBox22.EditValue, 1, 2)) > 0 AndAlso Val(Mid(TextBox22.EditValue, 4, 2)) = 0 Then TextBox23.EditValue = "" : TextBox22.EditValue = "00.00"
        If Not TextBox22.EditValue = "00.00" Then TextBox23.EditValue = LeggiCpt(TextBox22.EditValue)
    End Sub
    Private Sub TextBox24_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox24.LostFocus
        If Val(Mid(TextBox24.EditValue, 1, 2)) > 0 AndAlso Val(Mid(TextBox24.EditValue, 4, 2)) = 0 Then TextBox25.EditValue = "" : TextBox24.EditValue = "00.00"
        If Not TextBox24.EditValue = "00.00" Then TextBox25.EditValue = LeggiCpt(TextBox24.EditValue)
    End Sub
    Private Sub TextBox26_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox26.LostFocus
        If Val(Mid(TextBox26.EditValue, 1, 2)) > 0 AndAlso Val(Mid(TextBox26.EditValue, 4, 2)) = 0 Then TextBox27.EditValue = "" : TextBox26.EditValue = "00.00"
        If Not TextBox26.EditValue = "00.00" Then TextBox27.EditValue = LeggiCpt(TextBox26.EditValue)
    End Sub
    Private Sub ButtonF8b_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8b.Click
        Dim Dest As String = Nothing
        If where Is TextBox20 Then Dest = TextBox21.EditValue
        If where Is TextBox22 Then Dest = TextBox23.EditValue
        If where Is TextBox24 Then Dest = TextBox25.EditValue
        If where Is TextBox26 Then Dest = TextBox27.EditValue
        If Not Dest Is Nothing Then BottoneF8b(where.TEXT, Dest)
    End Sub

    Private Sub BottoneF8b(ByRef Cod As String, ByRef Dest As String)
        Dim Cod1 As String = Query.CercaPia()
        Cod = IIf(Cod1 > "", Cod1, Cod)
        Dest = LeggiCpt(Cod)
    End Sub

    Private Sub TextBox1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox20.GotFocus, TextBox22.GotFocus, TextBox24.GotFocus, TextBox26.GotFocus
        where = sender
    End Sub
    Private Sub TextBox1_LostFocus1(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox20.LostFocus, TextBox22.LostFocus, TextBox24.LostFocus, TextBox26.LostFocus
        where = Nothing
        If sender.text = "" Then sender.text = "00.00"
    End Sub
#Region "TASTI CONFERMA handles enter"
    Sub TbLeggi1_ENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi1.Enter
        If TextBox7.EditorContainsFocus = True Then TextBox1.Focus() : Exit Sub
        If ControlloCodice(TextBox1.EditValue) = False Then
            If Not ButtonF8.Focused Then TextBox1.Focus()
            Exit Sub
        End If
        TextBox1.EditValue = Format(Val(TextBox1.EditValue), "000")
        LetturaGr(TextBox1.EditValue)
        TextBox7.Focus()
    End Sub
    Private Sub TbLeggi2_ENTER(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TbLeggi2.Enter
        If TextBox3.EditorContainsFocus = True Then TextBox2.Focus() : Exit Sub
        If ControlloCodice2(TextBox2.EditValue) = False And TextBox7.EditValue > "" Then
            TextBox2.Focus()
            Exit Sub
        End If
        TextBox2.EditValue = Format(Val(TextBox2.EditValue), "00")
        TextBox3.Focus()
    End Sub
    Private Sub TbLeggi3_ENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi3.Enter
        If ControlloCodice2(TextBox4.EditValue) = False Then
            TextBox4.Focus()
            Exit Sub
        End If
        TextBox4.EditValue = Val(TextBox4.EditValue).ToString("00")
        MaxRig = DsCes.Tables(Ces).Rows.Count
        If Iset > -1 Then
            RwCes = DsCes.Tables(Ces).Rows(Iset)
            Exit Sub
        End If
        If Esiste() = False Then
            AzzeradataRow()
            TextBox5.EditValue = ""
            TextBox6.EditValue = CDec(0.0)
            CheckBox1.Checked = False
        Else
            Lettura(TextBox4.EditValue)
        End If
        TextBox5.Focus()
    End Sub
#End Region
End Class