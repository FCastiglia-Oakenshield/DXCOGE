Imports DXBASE
Imports System.Data.SqlClient
Public Module Generale
    Public OKCDC As Boolean
    Public Function AnnoEsercizio()
        Dim Str As String = "Select EseAnno from TbEse order by EseAnno desc"
        Cmd = New SqlCommand(Str, cnCo)
        Dim Anno As Int16 = Val(Cmd.ExecuteScalar)
        If Not Val(Anno) > 0 Then
            MessageBox.Show("Attenzione: non esiste l'Anno di Lavoro!" & Chr(13) & Chr(13) & "Controllare l'Anagrafica Azienda.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End If
        Return Anno
    End Function

    'Cod = codice Cpt, ritorna la descrizione
    Public Function LeggiCpt(ByRef Cod As String) As String
        If Not Cod > "" Then Return ""
        If Cod = "00.00" Then Return ""
        If Not IsNumeric(Mid(Cod, 1, 2)) And Not Mid(Cod, 3, 1) = "." And Not IsNumeric(Mid(Cod, 4, 2)) Then Return ""
        Dim cmd As New SqlCommand("Select PiaAnaCo from TbPia where PiaCodCo='" & Cod & "'", cnCo)
        Dim Ris As String = cmd.ExecuteScalar
        If Ris Is Nothing Then Cod = "00.00"
        Return Ris
    End Function

    Public Sub DisabTextBox(ByVal myGrid As DataGrid)
        'Disabilito le TextBox delle celle per ogni TableStyle
        Dim x As DataGridTableStyle
        Dim y As Object
        For Each x In myGrid.TableStyles
            For Each y In x.GridColumnStyles
                If TypeOf y Is DataGridTextBoxColumn Then
                    CType(y, DataGridTextBoxColumn).TextBox.Enabled = False
                End If
            Next
        Next
    End Sub
End Module