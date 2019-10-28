Imports DXBASE
Public Class TestComunita


    Private Sub SimpleButton1_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton1.Click
        TextEdit2.EditValue = spell_my_int(Int(TextEdit1.EditValue))
    End Sub

    Private Sub SimpleButton2_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton2.Click
        Dim COMUNE As String = ""
        Dim PROV As String = ""
        Dim SESSO As String = ""
        Dim DataN As Date
        Dim Cap As String = ""

        'ScorporaCodFis(ByVal Codfis As String, ByRef Comune As String, ByRef Prov As String, ByRef Sesso As String, ByRef DataN As Date) As Boolean
        If ScorporaCodFis(TextEdit4.EditValue, COMUNE, PROV, SESSO, DataN, Cap) = True Then
            TextEdit3.EditValue = COMUNE & " " & "(" & PROV & ")"
            TextEdit5.EditValue = DataN
            TextEdit6.EditValue = SESSO
        End If

    End Sub
End Class