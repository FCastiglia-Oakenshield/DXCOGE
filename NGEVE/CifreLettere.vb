Public Module CifreLettere
    Public Function spell_my_int(ByVal num As Long) As String
        Dim centOOttanta As Boolean
        centOOttanta = False
        Dim numstring As String
        Dim numlen As Integer
        Dim IDcent As Byte
        Dim subnumerostring As String
        Dim subnumero As Integer
        Dim cifra(2) As String
        Dim text(2) As String
        Dim prime2cifre As Integer
        Dim result As String
        Dim sezione As Integer
        Dim mono(9) As String
        Dim duplo(9) As String
        Dim deca(9) As String
        Dim cento(2) As String
        Dim mili(1, 9) As String
        Dim max
        max = ((10 ^ (6 * 3)) - 1) - 1
        '   Caricamento di mono() in formato esteso
        mono(0) = ""
        mono(1) = "uno"
        mono(2) = "due"
        mono(3) = "tre"
        mono(4) = "quattro"
        mono(5) = "cinque"
        mono(6) = "sei"
        mono(7) = "sette"
        mono(8) = "otto"
        mono(9) = "nove"
        '   Caricamento di duplo() in formato esteso
        duplo(0) = "dieci"
        duplo(1) = "undici"
        duplo(2) = "dodici"
        duplo(3) = "tredici"
        duplo(4) = "quattordici"
        duplo(5) = "quindici"
        duplo(6) = "sedici"
        duplo(7) = "dicias" & mono(7)
        duplo(8) = "dici" & mono(8)
        duplo(9) = "dician" & mono(9)
        '   Caricamento di deca() in formato esteso
        deca(0) = ""
        deca(1) = duplo(0)
        deca(2) = "venti"
        deca(3) = mono(3) & "nta"
        deca(4) = "quaranta"
        deca(5) = "cinquanta"
        deca(6) = "sessanta"
        deca(7) = "settanta"
        deca(8) = "ottanta"
        deca(9) = "novanta"
        '   Caricamento di cento() in formato esteso
        cento(0) = "cent"
        cento(1) = cento(0) & "o"
        '   Caricamento di mili(0 ,) in formato esteso
        mili(0, 0) = ""
        mili(0, 1) = "mille"
        mili(0, 2) = "milione"
        mili(0, 3) = "miliardo"
        mili(0, 4) = "bilione"
        mili(0, 5) = "biliardo"
        '   Caricamento di mili(1 ,) in formato esteso
        mili(1, 0) = ""
        mili(1, 1) = "mila"
        mili(1, 2) = "milioni"
        mili(1, 3) = "miliardi"
        mili(1, 4) = "bilioni"
        mili(1, 5) = "biliardi"
        result = ""
        sezione = 0
        If (num < 0) Then
            spell_my_int = "Minimo 0!"
        ElseIf (num > max) Then
            spell_my_int = "Massimo " & max & "!"
        ElseIf (num = 0) Then
            spell_my_int = "zero"
        Else
            numstring = CStr(num)
            Select Case (Len(numstring) Mod 3)
                Case 1
                    numstring = "00" & CStr(num)
                Case 2
                    numstring = "0" & CStr(num)
                Case Else
                    numstring = CStr(num)
            End Select
            numlen = Len(numstring)
            'MsgBox ("Numlen: " & numlen & "  subnumero: " & numstring)
            Do While ((sezione + 1) * 3 <= numlen)
                subnumerostring = Mid(numstring, (numlen - ((sezione + 1) * 3)) + 1, 3)
                subnumero = CInt(subnumerostring)
                cifra(0) = Mid(subnumerostring, 1, 1)
                cifra(1) = Mid(subnumerostring, 2, 1)
                cifra(2) = Mid(subnumerostring, 3, 1)
                'MsgBox ("Sezione numero: " & sezione & ", cifre della sezione: " & cifra(0) & cifra(1) & cifra(2))
                If (subnumero <> 0) Then
                    prime2cifre = CInt(cifra(1)) * 10 + CInt(cifra(2))
                    'MsgBox (prime2cifre)
                    If (prime2cifre < 10) Then
                        text(2) = mono(cifra(2))
                        text(1) = ""
                    ElseIf (prime2cifre < 20) Then
                        text(2) = ""
                        text(1) = duplo(prime2cifre - 10)
                    Else
                        'ventitre => ventitrè
                        If (sezione = 0 And cifra(2) = 3) Then
                            text(2) = "trè"
                        Else
                            text(2) = mono(cifra(2))
                        End If
                        '   novantaotto => novantotto
                        If (cifra(2) = 1 Or cifra(2) = 8) Then
                            text(1) = Mid(deca(cifra(1)), 1, Len(deca(cifra(1))) - 1)
                        Else
                            text(1) = deca(cifra(1))
                        End If
                    End If
                    If (cifra(0) = 0) Then
                        text(0) = ""
                    Else
                        ' centoottanta => centottanta
                        If (Not centOOttanta And cifra(1) = 8 Or (cifra(1) = 0 And cifra(2) = 8)) Then
                            IDcent = 0
                        Else
                            IDcent = 1
                        End If
                        If (cifra(0) <> 1) Then
                            text(0) = mono(cifra(0)) & cento(IDcent)
                        Else
                            text(0) = cento(IDcent)
                        End If
                    End If
                    '   unomille    => mille
                    '   miliardo    => unmiliardo
                    If (subnumero = 1 And sezione <> 0) Then
                        If (sezione >= 2) Then
                            result = "un" & mili(0, sezione) & result
                        Else
                            result = mili(0, sezione) & result
                        End If
                    Else
                        result = text(0) & text(1) & text(2) & mili(1, sezione) & result
                    End If
                End If
                sezione = sezione + 1
            Loop
            spell_my_int = result
        End If
    End Function
End Module
