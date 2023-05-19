Imports DXBASE
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Threading

Public Class LPDContoEconomico

    Dim TbBil As DataTable
    Dim DaBil As SqlDataAdapter

    Dim EseAnno(5), MaxEse As Int16
    Dim EseDal(5), EseAl(5) As Date
    Dim Sw As Int16 = 0
    Dim ii(0), Rif As Integer
    Dim Intesta As String = ""
    Dim CM As Integer = -1
    Dim FO As String = ""
    Private Sub LPDContoEconomico_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        If Sw = 0 Then
            DateEdit1.EditValue = Today : Apertura() : Sw = 1
        End If
    End Sub
    Sub Apertura()
        Cmd = New SqlCommand("SELECT top 5 * from TbEse Order by EseAnno desc", cnCo)
        ComboBoxEdit1.Properties.Items.Clear()

        Dim x As Int16
        Dim Str As String = ""
        MaxEse = -1
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MaxEse = MaxEse + 1
            ComboBoxEdit1.Properties.Items.Add(dataRd.Item("EseAnno"))
            EseAnno(MaxEse) = dataRd.Item("EseAnno")
            EseDal(MaxEse) = dataRd.Item("EseDal")
            EseAl(MaxEse) = dataRd.Item("EseAl")
        End While
        dataRd.Close()
        For x = 0 To MaxEse
            If ComboBoxEdit1.Properties.Items(x) = DateEdit1.EditValue.Year Then
                ComboBoxEdit1.SelectedIndex = x
                Esercizi(x)
                Exit For
            End If
        Next
    End Sub
    Private Sub ButtonST_Click(sender As Object, e As EventArgs) Handles ButtonST.Click
        PopolaStampa()
    End Sub

    Private Sub ButtonP_Click(sender As Object, e As EventArgs) Handles ButtonP.Click
        DXANTEPRIMA(GridControl11, False, Printing.PaperKind.A4, "Anno " & ComboBoxEdit1.EditValue.ToString & " Conto Economico " & DateEdit2.EditValue & "-" & DateEdit1.EditValue)
    End Sub

    Sub PopolaStampa()
        Cursor = Cursors.WaitCursor
        Dim Str As String = "XLDPCONTOECONOMICO  @DAL='" & CDate(DateEdit2.EditValue).ToShortDateString & "',@AL='" & CDate(DateEdit1.EditValue).ToShortDateString & "'"
        TbBil = New DataTable()
        DaBil = New SqlDataAdapter(Str, CnDc)
        DaBil.Fill(TbBil)
        GridControl11.DataSource = TbBil
        GridView11.ClearSelection()
        Cursor = Cursors.Default
    End Sub
    Sub Esercizi(ByVal x As Int16)
        DateEdit2.Properties.MaxValue = "31/12/2050" ' reset campi per ricalcolare limiti
        DateEdit2.Properties.MinValue = "01/01/1900"
        DateEdit3.Properties.MaxValue = "31/12/2050"
        DateEdit3.Properties.MinValue = "01/01/1900"
        DateEdit2.Properties.MaxValue = EseDal(x).AddMinutes(1)
        DateEdit2.Properties.MinValue = EseDal(x)
        DateEdit2.EditValue = EseDal(x)
        DateEdit3.Properties.MaxValue = EseAl(x).AddMinutes(1)
        DateEdit3.Properties.MinValue = EseAl(x)
        DateEdit3.EditValue = EseAl(x)
        DateEdit1.Properties.MaxValue = "31/12/2050"
        DateEdit1.Properties.MinValue = "01/01/1900"
        DateEdit1.EditValue = DateEdit3.EditValue
        DateEdit1.Properties.MaxValue = DateEdit3.Properties.MaxValue
        DateEdit1.Properties.MinValue = DateEdit2.Properties.MinValue
        If DateEdit1.EditValue > Today Then DateEdit1.EditValue = Today
        DateEdit1.Focus()
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        Dim x As Int16
        x = ComboBoxEdit1.SelectedIndex
        If x = -1 Or Sw = 0 Then Exit Sub
        Esercizi(x)
    End Sub
End Class