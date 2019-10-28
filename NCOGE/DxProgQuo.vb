Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Public Class DxProgQuo
    Dim REPORT As New XtraReport
    Dim selectformula, StrPrint As String
    Dim DsProQuo As DataTable
    Dim DaProQuo As SqlDataAdapter
    Dim CspGru, CspSpe1, CspSpe2 As String
    Dim TbCat As DataTable
    Dim DaCat As SqlDataAdapter
    Dim Anno As Int16 = 0
    Dim Esercizio As Int16 = 0
    Dim DataMin As Date
    Dim DataMax As Date

    Private Sub DxProQuo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        IniziaTabella()
        PopolaGrid1()
        CheckEdit1.Focus()
    End Sub
    Private Sub IniziaTabella()
        Esercizio = AnnoEsercizio()
        Dim Str As String = "select MAX(CespUltAnnoAmm) from TbCesp"
        Dim Cmd As New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            Anno = dataRd.Item(0) + 1
        End If
        dataRd.Close()

        If Anno < 2000 Then Anno = Esercizio

        Str = "Select * from TbAzi where aziannolavoro=" & Anno
        Cmd = New SqlCommand(Str, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            CspGru = dataRd.Item("AziGruppoCesp")
            CspSpe1 = dataRd.Item("AziSpecieCesp")
            CspSpe2 = dataRd.Item("AziSottosCesp")
        End If
        dataRd.Close()

        Cmd = New SqlCommand("SELECT  * from TbEse where EseAnno = " & Anno, cnCo)
        dataRd = Cmd.ExecuteReader
        If dataRd.Read Then
            DataMin = dataRd.Item("EseDal")
            DataMax = dataRd.Item("EseAl")
        End If
        dataRd.Close()
        DateEdit1.EditValue = DataMax
        CheckEdit1.Checked = False
        TextEdit4.EditValue = Anno
    End Sub
    Private Sub PopolaGrid1()
        Dim cmd As New SqlCommand("Select CspNum,CspDesc,CspPerc,0.00 as Perc,cast(0 as bit) as Flag from TbCsp where CspGru=" & CspGru & " and CspSpe1=" & CspSpe1 & " and CspSpe2=" & CspSpe2 & " and CspNum>0", cnCo)
        TbCat = New DataTable()
        DaCat = New SqlDataAdapter(cmd)
        DaCat.Fill(TbCat)
        GridControl1.DataSource = TbCat
        GridView1.ClearSelection()
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click
        Stampa()
    End Sub
    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F9 Then
            ButtonF9.PerformClick()
            Exit Sub
        End If
    End Sub

    Private Sub InserisciRiga(ByVal idblk, ByVal Cat, ByVal Perc)
        Cmd = New SqlCommand("Insert into TMPAMMLIB (AmmLibBlock,AmmLibCat,AmmLibPerc) values (@AmmLibBlock,@AmmLibCat,@AmmLibPerc)", cnCo)
        Dim p1 As New SqlParameter("@AmmLibBlock", idblk)
        Dim p2 As New SqlParameter("@AmmLibCat", Cat)
        Dim p3 As New SqlParameter("@AmmLibPerc", Perc)
        Cmd.Parameters.Add(p1)
        Cmd.Parameters.Add(p2)
        Cmd.Parameters.Add(p3)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub Stampa()
        Cursor.Current = Cursors.WaitCursor
        Dim idblk As Int32

        Cmd = New SqlCommand("select isnull(max(ammlibblock),0)+1 as Block from tmpammlib", cnCo)
        idblk = Cmd.ExecuteScalar
        Dim Cat As New ArrayList
        For I As Int32 = 0 To TbCat.Rows.Count - 1
            If TbCat.Rows(I).Item("Flag") = True Then
                Cat.Add(TbCat.Rows(I).Item("CspNum"))
                InserisciRiga(idblk, TbCat.Rows(I).Item("CspNum"), TbCat.Rows(I).Item("Perc"))
            End If
        Next
        Dim Str As String = "EXEC XCesp @Data ='" & CDate(DateEdit1.EditValue).ToShortDateString & "', @idblk = " & idblk
        EsegueSql(Str, cnCo)

        StrPrint = ""
        If Cat.Count > 0 Then
            StrPrint = "Select * from CRPROGQUO where CRPROGQUO.CspGru = '" & CspGru & "' AND  CRPROGQUO.CspSpe1 = '" & CspSpe1 & "' AND CRPROGQUO.CspSpe2 = '" & CspSpe2 & "' AND CRPROGQUO.QuoAnno = " & Val(TextEdit4.EditValue) & " and (CRPROGQUO.ProQuoCat = '" & Cat(0) & "'"
            For I As Int32 = 1 To Cat.Count - 1
                StrPrint &= " or CRPROGQUO.ProQuoCat = '" & Cat(I) & "'"
            Next
            StrPrint = StrPrint & ") order by proquocat,proquonum "
            stampaProg()
            Return
        End If
    End Sub
    Private Sub stampaProg()
        DsProQuo = New DataTable
        DaProQuo = New SqlDataAdapter(StrPrint, cnCo)
        DaProQuo.SelectCommand.CommandTimeout = 300
        DaProQuo.Fill(DsProQuo)
        selectformula = ""
        REPORT = New DxStProQuo
        REPORT.DataSource = DsProQuo
        REPORT.DataMember = "DsProQuo"
        REPORT.FilterString = selectformula
        REPORT.Parameters.Item("Marchio").Value = Marchio()
        REPORT.Parameters.Item("Titolo").Value = "PROGETTO QUOTE AMMORTAMENTO ANNO " & Anno & " Dal " & DataMin & " al " & DateEdit1.EditValue
        REPORT.ShowPreview()
    End Sub
    Private Sub CheckEdit1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit1.EditValueChanged
        For I As Int32 = 0 To TbCat.Rows.Count - 1
            TbCat.Rows(I).Item("Flag") = CheckEdit1.Checked
        Next
    End Sub
    Private Sub RepositoryItemTextEdit1_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RepositoryItemTextEdit1.Validating
        If CDec(sender.editvalue) > 100 Then e.Cancel = True
    End Sub

    Private Sub DateEdit1_Leave(sender As Object, e As System.EventArgs) Handles DateEdit1.Leave
        If DateEdit1.EditValue > DataMax Or DateEdit1.EditValue < DataMin Then DateEdit1.EditValue = DataMax
        DateEdit1.EditValue = CDate(Date.DaysInMonth(CDate(DateEdit1.EditValue).Year, CDate(DateEdit1.EditValue).Month) & "/" & CDate(DateEdit1.EditValue).Month & "/" & CDate(DateEdit1.EditValue).Year)
    End Sub
End Class