Imports DXBASE
Imports System.Data.SqlClient
Imports NCCOM
Imports DevExpress.XtraEditors

Public Class DxRegIva
    Dim TbRegIva As DataTable
    Dim DaRegIva As SqlDataAdapter
    Dim Str As String
    Dim EsisteRegistro As Boolean

    Dim nn As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
    Dim RxW As DataRow

    Private Sub DxRegIva_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        Pulizia(True)
        ComboBoxEdit1.Focus()
    End Sub
    Private Sub Pulizia(ByVal Tutto As Boolean)
        TextEdit1.EditValue = ""
        TextEdit2.EditValue = ""
        TextEdit3.EditValue = ""
        TextEdit4.EditValue = ""
        TextEdit5.EditValue = ""
        TextEdit6.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = ""
        TextEdit9.EditValue = ""
        TextEdit10.EditValue = ""

        TextEdit102.EditValue = ""
        iset = -1
        ComboBoxEdit2.SelectedIndex = 0
        ComboBoxEdit3.SelectedIndex = -1
        ComboBoxEdit4.SelectedIndex = -1
        CheckEdit1.Checked = False
        CheckEdit2.Checked = False
        CheckEdit3.Checked = False
        CheckEdit4.Checked = False
        CheckEdit5.Checked = False
        GroupControl2.Enabled = Not Tutto

        AttivaProrata()
        AttivaCorrisp()
        If Tutto = True Then
            TextEdit101.EditValue = ""
            Str = "select AziAnnoLavoro from tbazi order by aziannolavoro desc"
            Dim cmd As New SqlCommand(Str, cnCo)
            dataRd = cmd.ExecuteReader
            ComboBoxEdit1.Properties.Items.Clear()
            While dataRd.Read
                ComboBoxEdit1.Properties.Items.Add(dataRd.Item("AziAnnoLavoro"))
            End While
            dataRd.Close()
            GroupControl4.Enabled = False
            ComboBoxEdit1.SelectedIndex = 0
            Str = "SELECT * from TbCii order by CiiCod"
            Dim SS As String = ""
            ImageComboBoxEdit2.Properties.Items.Clear()
            cmd = New SqlCommand(Str, cnCo)
            dataRd = cmd.ExecuteReader
            While dataRd.Read
                If dataRd.Item("CiiCod") > 3 And dataRd.Item("CiiCod") <> 45 Then
                    SS = dataRd.Item("CiiCod").ToString.PadRight(2, " ") & " " & dataRd.Item("CiiCau")
                    nn = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(SS, dataRd.Item("CiiCod"), -1)
                    ImageComboBoxEdit2.Properties.Items.Add(nn)
                End If
            End While
            dataRd.Close()
        End If
    End Sub
    Private Sub TbLeggi0_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi0.Enter
        GridView1.ClearSelection()
        LeggiRegistro(Val(TextEdit101.EditValue))
    End Sub
  
    Private Sub LeggiRegistro(ByVal NReg As Int16)
        If Not Val(NReg) > 0 Then Return
        GroupControl2.Enabled = True
        GroupControl4.Enabled = True
        Dim cmd As New SqlCommand("Select * from VRegIva where RIvaAnno=" & ComboBoxEdit1.EditValue & " and RIvaNReg=" & NReg, cnCo)
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            EsisteRegistro = True
            CaricaDati()
            AttivaProrata()
            AttivaCorrisp()
            For I As Int16 = 0 To TbRegIva.Rows.Count - 1
                RxW = GridView1.GetDataRow(I)
                If NReg = RxW("RivaNReg") Then GridView1.SelectRow(I) : Exit For
            Next
        Else
            EsisteRegistro = False
            Pulizia(False)
        End If
        dataRd.Close()
        TextEdit2.Focus()
    End Sub
    Private Sub CaricaDati()
        Dim CC As Int16
        Dim DD As String = ""
        CC = IIf(dataRd.Item("RIvaAutoFCee") Is DBNull.Value, 0, dataRd.Item("RIvaAutoFCee")).ToString
        ComboBoxEdit3.SelectedIndex = SELEZIONAINDEX(CC)
        DD = IIf(dataRd.Item("RIvaTipoDoc") Is DBNull.Value, "", dataRd.Item("RIvaTipoDoc")).ToString
        ComboBoxEdit4.SelectedIndex = SELEZIONATD(DD)
        TextEdit9.EditValue = dataRd.Item("RIvaCliCee")
        TextEdit10.EditValue = dataRd.Item("RIvaCliDesc")
        TextEdit3.EditValue = dataRd.Item("RIvaCpt")
        TextEdit4.EditValue = dataRd.Item("RIvaCptDesc")
        TextEdit6.EditValue = dataRd.Item("RIvaCptCee")
        TextEdit5.EditValue = dataRd.Item("RIvaCptCeeDesc")
        TextEdit8.EditValue = dataRd.Item("RIvaCptSosp")
        TextEdit7.EditValue = dataRd.Item("RIvaCptSospDesc")
        TextEdit2.EditValue = dataRd.Item("RIvaDesc")
        TextEdit102.EditValue = dataRd.Item("RIvaNumFog")
        ImageComboBoxEdit2.EditValue = dataRd.Item("RIvaArt")
        CheckEdit1.Checked = dataRd.Item("RIvaPRata")
        TextEdit1.EditValue = dataRd.Item("RIvaSL")
        ComboBoxEdit2.EditValue = dataRd.Item("RIvaTipoDesc")
        '''' ArticoloCorr(dataRd.Item("RIvaArt").ToString, 0)
        CheckEdit2.Checked = dataRd.Item("RIvaInt")
        CheckEdit3.Checked = dataRd.Item("RIvaCh")
        If dataRd.Item("RIvaRCharge") Is DBNull.Value Then CheckEdit4.Checked = False Else CheckEdit4.Checked = CBool(dataRd.Item("RIvaRCharge"))
        CheckEdit5.Checked = dataRd.Item("RIvaFteP")
    End Sub
    Function SELEZIONAINDEX(ByVal CODICE As String) As Int16
        Dim x As Int16 = -1
        For x = 1 To ComboBoxEdit3.Properties.Items.Count
            If Val(Mid(ComboBoxEdit3.Properties.Items(x - 1), 1, 2)) = CODICE Then
                Return (x - 1) : Exit Function
            End If
        Next
        Return 0
    End Function
    Function SELEZIONATD(ByVal CODICE As String) As Int16
        Dim x As Int16 = -1
        For x = 1 To ComboBoxEdit4.Properties.Items.Count
            If Mid(ComboBoxEdit4.Properties.Items(x - 1), 1, 4) = CODICE Then
                Return (x - 1) : Exit Function
            End If
        Next
        Return 0
    End Function
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        If ComboBoxEdit1.SelectedIndex = -1 Then Exit Sub
        Str = "select RIvaNReg,RIvaDesc from TbRegIva where RIvaAnno=" & ComboBoxEdit1.EditValue
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        ComboBoxEdit3.Properties.Items.Clear()
        ComboBoxEdit3.Properties.Items.Add("")
        While dataRd.Read
            ComboBoxEdit3.Properties.Items.Add(dataRd.Item("RIvaNReg") & " - " & dataRd.Item("RIvaDesc"))
        End While
        dataRd.Close()
        PopolaGrid()
        LeggiRegistro(Val(TextEdit101.EditValue))
    End Sub

    Private Sub PopolaGrid()
        Str = "Select * from VRegIva where RIvaAnno=" & ComboBoxEdit1.EditValue & " order by rivanreg"
        TbRegIva = New DataTable("TbRegIva")
        DaRegIva = New SqlDataAdapter(Str, cnCo)
        DaRegIva.Fill(TbRegIva)
        GridControl1.DataSource = TbRegIva
        GridView1.ClearSelection()
    End Sub

    Private Sub ComboBoxEdit2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEdit2.SelectedIndexChanged
        AttivaChiusura()
        AttivaProrata()
        AttivaCorrisp()
    End Sub
    Private Sub AttivaChiusura()
        If ComboBoxEdit2.SelectedIndex = 8 Then
            CheckEdit3.Checked = True
        End If
    End Sub
    Private Sub AttivaProrata()
        If ComboBoxEdit2.SelectedIndex = 1 Or ComboBoxEdit2.SelectedIndex = 3 Then
            CheckEdit1.Enabled = True
            GroupControl5.Enabled = True
            CheckEdit4.Enabled = True
            CheckEdit5.Enabled = True
        Else
            CheckEdit1.Enabled = False
            GroupControl5.Enabled = False
            CheckEdit4.Enabled = False
            CheckEdit5.Enabled = False
        End If
    End Sub
    Private Sub AttivaCorrisp()
        If ComboBoxEdit2.SelectedIndex = 4 Then
            GroupControl87.Enabled = True
        Else
            GroupControl87.Enabled = False
        End If
    End Sub
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo1(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo1(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        iset = hi.RowHandle
    End Sub

    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If iset > -1 Then
            RxW = GridView1.GetDataRow(iset)
            TextEdit101.EditValue = RxW("RivaNReg")
            LeggiRegistro(TextEdit101.EditValue)
        End If
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        If Controllo() = False Then Return
        If EsisteRegistro = False Then
            Str = "Insert into TbRegIva (RIvaAnno,RIvaNReg,RIvaSL,RIvaNumFog,RIvaTipo,RIvaPRata,RIvaDesc,RIvaCpt,RIvaAutoFCee,RIvaCptCee,RIvaCptSosp,RIvaCliCee,RIvaArt,RIvaInt,RIvaCh,RIvaRCharge,RIvaFteP,RIvaTipoDoc) values (@RIvaAnno,@RIvaNReg,@RIvaSL,@RIvaNumFog,@RIvaTipo,@RIvaPRata,@RIvaDesc,@RIvaCpt,@RIvaAutoFCee,@RIvaCptCee,@RIvaCptSosp,@RIvaCliCee,@RIvaArt,@RIvaInt,@RIvaCh,@RIvaRCharge,@RIvaFteP,@RIvaTipoDoc)"
        Else
            Str = "Update TbRegIva set RIvaSL=@RIvaSL,RIvaNumFog=@RIvaNumFog,RIvaTipo=@RIvaTipo,RIvaPRata=@RIvaPRata,RIvaDesc=@RIvaDesc,RIvaCpt=@RIvaCpt,RIvaAutoFCee=@RIvaAutoFCee,RIvaCptCee=@RIvaCptCee,RIvaCptSosp=@RIvaCptSosp,RIvaCliCee=@RIvaCliCee,RIvaArt=@RIvaArt,RIvaInt=@RIvaInt,RIvaCh=@RIvaCh,RIvaRCharge=@RIvaRCharge,RIvaFteP=@RIvaFteP,RIvaTipoDoc=@RIvaTipoDoc where RIvaAnno=@RIvaAnno and RIvaNReg=@RIvaNReg"
        End If
        Dim Cmd As New SqlCommand(Str, cnCo)

        Dim p1 As New SqlParameter("@RIvaAnno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@RIvaNReg", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@RIvaSL", SqlDbType.VarChar)
        Dim p4 As New SqlParameter("@RIvaNumFog", SqlDbType.Int)
        Dim p5 As New SqlParameter("@RIvaTipo", SqlDbType.SmallInt)
        Dim p6 As New SqlParameter("@RIvaPRata", SqlDbType.SmallInt)
        Dim p7 As New SqlParameter("@RIvaDesc", SqlDbType.VarChar)
        Dim p8 As New SqlParameter("@RIvaCpt", SqlDbType.VarChar)
        Dim p9 As New SqlParameter("@RIvaAutoFCee", SqlDbType.SmallInt)
        Dim p10 As New SqlParameter("@RIvaCptCee", SqlDbType.VarChar)
        Dim p11 As New SqlParameter("@RIvaCptSosp", SqlDbType.VarChar)
        Dim p12 As New SqlParameter("@RIvaCliCee", SqlDbType.VarChar)
        Dim p13 As New SqlParameter("@RIvaArt", SqlDbType.SmallInt)
        Dim p14 As New SqlParameter("@RIvaInt", SqlDbType.Bit)
        Dim p15 As New SqlParameter("@RIvaCh", SqlDbType.Bit)
        Dim p16 As New SqlParameter("@RIvaRCharge", SqlDbType.Bit)
        Dim p17 As New SqlParameter("@RIvaFteP", SqlDbType.Bit)
        Dim p18 As New SqlParameter("@RIvaTipoDoc", SqlDbType.VarChar)

        p1.Value = ComboBoxEdit1.EditValue
        p2.Value = TextEdit101.EditValue
        p3.Value = TextEdit1.EditValue
        p4.Value = Val(TextEdit102.EditValue)
        p5.Value = ComboBoxEdit2.SelectedIndex + 1
        p6.Value = CheckEdit1.Checked
        p7.Value = TextEdit2.EditValue
        p8.Value = TextEdit3.EditValue
        p9.Value = Val(Mid(ComboBoxEdit3.EditValue, 1, 2)) 'ComboBox3.SelectedIndex
        p10.Value = TextEdit6.EditValue
        p11.Value = TextEdit8.EditValue
        p12.Value = TextEdit9.EditValue
        If ImageComboBoxEdit2.EditValue IsNot Nothing Then p13.Value = ImageComboBoxEdit2.EditValue Else p13.Value = 0
        p14.Value = CheckEdit2.Checked
        p15.Value = CheckEdit3.Checked
        p16.Value = CheckEdit4.Checked
        p17.Value = CheckEdit5.Checked
        p18.Value = Mid(ComboBoxEdit4.EditValue, 1, 4) 'ComboBox4.SelectedIndex
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.Parameters.Add(p4)
        Cmd.Parameters.Add(p5)
        Cmd.Parameters.Add(p6)
        Cmd.Parameters.Add(p7)
        Cmd.Parameters.Add(p8)
        Cmd.Parameters.Add(p9)
        Cmd.Parameters.Add(p10)
        Cmd.Parameters.Add(p11)
        Cmd.Parameters.Add(p12)
        Cmd.Parameters.Add(p13)
        Cmd.Parameters.Add(p14)
        Cmd.Parameters.Add(p15)
        Cmd.Parameters.Add(p16)
        Cmd.Parameters.Add(p17)
        Cmd.Parameters.Add(p18)
        Cmd.ExecuteNonQuery()

        Dim Anno As Int16 = ComboBoxEdit1.SelectedIndex
        Pulizia(True)
        ComboBoxEdit1.SelectedIndex = -1 : ComboBoxEdit1.SelectedIndex = Anno
        TextEdit101.Focus()
    End Sub

    Private Function Controllo() As Boolean
        If TextEdit2.EditValue.ToString.Length <= 0 Then
            TextEdit2.Focus()
            Return False
        End If
        If TextEdit4.EditValue.ToString.Length = 0 Then
            TextEdit3.Focus()
            Return False
        End If
        If TextEdit3.EditValue.ToString.Length <> 5 Then
            TextEdit3.Focus()
            Return False
        End If
        If Not Val(TextEdit101.EditValue) > 0 Then Return False
        If ComboBoxEdit2.SelectedIndex = 4 And ImageComboBoxEdit2.SelectedIndex = -1 Then
            ImageComboBoxEdit2.Focus()
            Return False
        End If
        Return True
    End Function
    Private Sub TbLeggi1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi1.Enter
        If TextEdit3.EditValue = "00.10" Then TextEdit3.EditValue = "00.00"
        If Val(Mid(TextEdit3.EditValue, 1, 2)) > 0 AndAlso Val(Mid(TextEdit3.EditValue, 4, 2)) = 0 Then TextEdit4.EditValue = "" : TextEdit3.EditValue = "00.00"
        If Not TextEdit3.EditValue = "00.00" Then
            TextEdit4.EditValue = LeggiCpt(TextEdit3.EditValue.ToString)
        Else
            TextEdit4.EditValue = ""
            TextEdit3.Focus()
        End If
    End Sub

    Private Sub TbLeggi2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi2.Enter
        If TextEdit6.EditValue = "00.10" Then TextEdit6.EditValue = "00.00"
        If Val(Mid(TextEdit6.EditValue, 1, 2)) > 0 AndAlso Val(Mid(TextEdit6.EditValue, 4, 2)) = 0 Then TextEdit5.EditValue = "" : TextEdit6.EditValue = "00.00"
        If Not TextEdit6.EditValue = "00.00" Then
            TextEdit5.EditValue = LeggiCpt(TextEdit6.EditValue.ToString)
        Else
            TextEdit5.EditValue = ""
            TextEdit6.Focus()
        End If
    End Sub

    Private Sub TbLeggi3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi1.Enter
        If TextEdit8.EditValue = "00.10" Then TextEdit8.EditValue = "00.00"
        If Val(Mid(TextEdit8.EditValue, 1, 2)) > 0 AndAlso Val(Mid(TextEdit8.EditValue, 4, 2)) = 0 Then TextEdit7.EditValue = "" : TextEdit8.EditValue = "00.00"
        If Not TextEdit8.EditValue = "00.00" Then
            TextEdit7.EditValue = LeggiCpt(TextEdit8.EditValue.ToString)
        Else
            TextEdit7.EditValue = ""
        End If
    End Sub

    Private Sub TbLeggi4_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbLeggi4.Enter
        LeggiCliente(TextEdit9.EditValue, TextEdit10.EditValue)
    End Sub
    Private Sub LeggiCliente(ByRef cod, ByRef desc)
        cod = cod.ToString.PadLeft(5, "0")
        Str = "SELECT AnaDesc FROM TbAna WHERE AnaGrp = 'CL' and AnaCod='" & cod & "'"
        Dim cmd As New SqlCommand(Str, cnVd)
        desc = cmd.ExecuteScalar()
    End Sub

    Private Sub ButtonF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF3.Click
        Try
            If TextEdit101.EditValue <= 0 Then Return
        Catch ex As Exception
            Return
        End Try
        DelReg()
    End Sub

    Private Sub DelReg()
        Dim box As Object
        box = MessageBox.Show("Vuoi Eliminare il Registro Selezionato?", "ELIMINA REGISTRO IVA", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If box = DialogResult.No Then
            Exit Sub
        End If

        Cmd = New SqlCommand("delete from TbRegIva where RIvaNReg =" & TextEdit101.EditValue & " and RivaAnno =" & ComboBoxEdit1.EditValue, cnCo)
        Cmd.ExecuteNonQuery()
        Pulizia(True)
        PopolaGrid()
        TextEdit101.Focus()
    End Sub

    Private Sub RegIva_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F5 Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F3 Then
            e.Handled = True
            ButtonF3.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F8 Then
            e.Handled = True
            ButtonF8.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub TextEdit1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextEdit3.KeyPress, TextEdit6.KeyPress, TextEdit8.KeyPress
        If Not IsNumeric(e.KeyChar) And Not e.KeyChar = Chr(8) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And sender.text.length <> 2 Then
            e.Handled = True
        End If
    End Sub

    Dim where As Object
    Private Sub TextEdit_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit3.Enter, TextEdit6.Enter, TextEdit8.Enter
        where = sender
        ButtonF8.Enabled = True
    End Sub
    Private Sub TextEdit_LostFocus1(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEdit3.LostFocus, TextEdit6.LostFocus, TextEdit8.LostFocus
        If ButtonF8.Focused Then Return
        where = Nothing
        ButtonF8.Enabled = False
    End Sub

    Private Sub ButtonF8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF8.Click
        Dim Dest As TextEdit = Nothing
        If where Is TextEdit3 Then Dest = TextEdit4
        If where Is TextEdit6 Then Dest = TextEdit5
        If where Is TextEdit8 Then Dest = TextEdit7
        If Not Dest Is Nothing Then BottoneF8b(where, Dest)
    End Sub

    Private Sub BottoneF8b(ByRef Cod As TextEdit, ByRef Dest As TextEdit)
        Dim Cod1 As String = Query.CercaPia()
        Cod.EditValue = IIf(Cod1 > "", Cod1, Cod.EditValue)
        LeggiCpt(Dest.EditValue.ToString)
        Cod.Focus()
    End Sub

    Private Sub CheckEdit2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        TextEdit102.Enabled = CheckEdit2.Checked
    End Sub

    Private Sub CheckEdit3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit3.CheckedChanged
        AttivaChiusura()
    End Sub


End Class