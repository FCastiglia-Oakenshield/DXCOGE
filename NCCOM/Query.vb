Imports DXBASE
Imports System.Data.SqlClient
Imports System.Collections

Public Class Query
    Shared ad As SqlDataAdapter
    Shared DT As DataTable

    



    ''Shared Function CercaCom(ByVal ArgRicerca As String) As String
    ''    Dim StrSelect, Jolly As String
    ''    Dim Contatore As Integer
    ''    Dim i As Int16
    ''    CercaCom = ""
    ''    Jolly = "%"
    ''    For i = 0 To ArgRicerca.Length - 1
    ''        If ArgRicerca.Substring(i, 1) = "%" Or ArgRicerca.Substring(i, 1) = "_" Or ArgRicerca.Substring(i, 1) = "[" Then
    ''            Jolly = ""
    ''            Exit For
    ''        End If
    ''    Next

    ''    StrSelect = " SELECT * FROM TbCom Where ComCod Like @Chiave Order By ComCod"
    ''    Dim cmd As New SqlCommand(StrSelect, cnCo)
    ''    Dim p1 As New SqlParameter("@Chiave", SqlDbType.Char)
    ''    p1.Value = ArgRicerca & Jolly
    ''    cmd.Parameters.Add(p1)

    ''    ad = New SqlDataAdapter(cmd)
    ''    ds = New DataSet
    ''    ad.Fill(ds, "TbCom")
    ''    Contatore = ds.Tables("TbCom").Rows.Count
    ''    If Contatore = 0 Then
    ''        Return ""
    ''    ElseIf Contatore = 1 Then
    ''        Return ds.Tables("TbCom").Rows(0).Item(0)
    ''    ElseIf Contatore > 1 Then
    ''        Dim FCercaCom As New RicercheFrm
    ''        FCercaCom.dataset = ds
    ''        FCercaCom.Text = "Ricerca Comune"
    ''        FCercaCom.Tabella = "TbCom"
    ''        FCercaCom.ShowDialog()
    ''        Return FCercaCom.codice
    ''    End If
    ''    ad.Dispose()
    ''    ds.Dispose()
    ''End Function

    ''Shared Function CercaCau() As Int16
    ''    Dim StrSelect As String
    ''    Dim Contatore As Integer
    ''    Dim i As Int16

    ''    StrSelect = " SELECT * FROM TbCii Order By CiiCod"
    ''    Dim cmd As New SqlCommand(StrSelect, cnCo)
    ''    ad = New SqlDataAdapter(cmd)
    ''    ds = New DataSet
    ''    ad.Fill(ds, "TbCii")
    ''    Contatore = ds.Tables("TbCii").Rows.Count
    ''    If Contatore = 0 Then
    ''        Return 0
    ''    ElseIf Contatore = 1 Then
    ''        Return ds.Tables("TbCii").Rows(0).Item(0)
    ''    ElseIf Contatore > 1 Then
    ''        Dim FCercaCom As New RicercheFrm
    ''        FCercaCom.dataset = ds
    ''        FCercaCom.Text = "Cerca Causale"
    ''        FCercaCom.Tabella = "TbCii"
    ''        FCercaCom.ShowDialog()
    ''        Return Val(FCercaCom.codice)
    ''    End If
    ''    ad.Dispose()
    ''    ds.Dispose()
    ''End Function
   



    ''Shared Function CercaUtente(ByVal ArgRicerca As String) As String 'su CCOM
    ''    Dim StrSelect, Jolly As String
    ''    Dim Contatore As Integer
    ''    Dim i As Int16

    ''    Jolly = "%"
    ''    For i = 0 To ArgRicerca.Length - 1
    ''        If ArgRicerca.Substring(i, 1) = "%" Or ArgRicerca.Substring(i, 1) = "_" Or ArgRicerca.Substring(i, 1) = "[" Then
    ''            Jolly = ""
    ''            Exit For
    ''        End If
    ''    Next

    ''    StrSelect = " SELECT * FROM TbAna Where Anagrp='AZ'"
    ''    Dim cmd As New SqlCommand(StrSelect, cnVd)
    ''    Dim p1 As New SqlParameter("@Chiave", SqlDbType.VarChar)
    ''    p1.Value = ArgRicerca & Jolly
    ''    cmd.Parameters.Add(p1)

    ''    ad = New SqlDataAdapter(cmd)
    ''    ds = New DataSet
    ''    ad.Fill(ds, "TbUte")
    ''    Contatore = ds.Tables("TbUte").Rows.Count
    ''    If Contatore = 0 Then
    ''        Return 0
    ''    ElseIf Contatore = 1 Then
    ''        Return ds.Tables("TbUte").Rows(0).Item("AnaCod")
    ''    ElseIf Contatore > 1 Then
    ''        Dim FCercaUte As New frmRicUte
    ''        FCercaUte.dataset = ds
    ''        FCercaUte.ShowDialog()
    ''        Return FCercaUte.codice
    ''    End If
    ''    ad.Dispose()
    ''    ds.Dispose()
    ''End Function
  

    REM ELEMENTI CONTROLLATI --------
    ''''Shared Function CercaCii() As Int16
    ''''    Dim StrSelect As String
    ''''    Dim Contatore As Integer
    ''''    Dim i As Int16
    ''''    StrSelect = " SELECT * FROM TbCii Order By CiiCod"
    ''''    Dim cmd As New SqlCommand(StrSelect, cnCo)
    ''''    Dim Field As New ArrayList
    ''''    Field.Add("CiiCod")
    ''''    Field.Add("CiiDes")
    ''''    ad = New SqlDataAdapter(cmd)
    ''''    DT = New DataTable
    ''''    ad.Fill(DT)
    ''''    Contatore = DT.Rows.Count
    ''''    If Contatore = 0 Then
    ''''        Return 0
    ''''    ElseIf Contatore = 1 Then
    ''''        Return DT.Rows(0).Item(0)
    ''''    ElseIf Contatore > 1 Then
    ''''        Dim FCercaCii As New DxRicercheFrm
    ''''        FCercaCii.Dta = DT
    ''''        FCercaCii.Ar = Field
    ''''        FCercaCii.TT = 0
    ''''        FCercaCii.Text = "Ricerca Codici Iva"
    ''''        FCercaCii.ShowDialog()
    ''''        Return Val(FCercaCii.codice)
    ''''    End If
    ''''    ad.Dispose()
    ''''    DT.Dispose()


    ''''End Function


    Shared Function CercaPia() As String
        Dim StrSelect As String
        Dim Contatore As Integer
        Dim Field As New ArrayList
        Field.Add("PiaCodCo")
        Field.Add("PiaAnaCo")
        CercaPia = ""
        StrSelect = " SELECT * FROM TbPia"
        Dim cmd As New SqlCommand(StrSelect, cnCo)
        ad = New SqlDataAdapter(cmd)
        DT = New DataTable
        ad.Fill(DT)
        Contatore = DT.Rows.Count
        If Contatore = 0 Then
            Return 0
        ElseIf Contatore = 1 Then
            Return DT.Rows(0).Item("PiaCodCo")
        ElseIf Contatore > 1 Then
            Dim FCercaPia As New DxRicPia
            FCercaPia.Dta = DT
            FCercaPia.Ar = Field
            FCercaPia.TT = 1
            FCercaPia.Text = "Ricerca Conto"
            FCercaPia.ShowDialog()
            Return FCercaPia.codice
        End If
        ad.Dispose()
        DT.Dispose()
    End Function
    Shared Function CercaAteco(ByVal ArgRicerca As String) As String
        Dim StrSelect, Jolly As String
        Dim Contatore As Integer
        Dim i As Int16
        Dim Field As New ArrayList
        Field.Add("AtecoCod")
        Field.Add("AtecoDesc")
        CercaAteco = ""
        Jolly = "%"
        For i = 0 To ArgRicerca.Length - 1
            If ArgRicerca.Substring(i, 1) = "%" Or ArgRicerca.Substring(i, 1) = "_" Or ArgRicerca.Substring(i, 1) = "[" Then
                Jolly = ""
                Exit For
            End If
        Next
        StrSelect = " SELECT * FROM TbAteco order By AtecoCod"
        Dim cmd As New SqlCommand(StrSelect, cnCo)
        Dim p1 As New SqlParameter("@Chiave", SqlDbType.Char)
        p1.Value = ArgRicerca & Jolly
        cmd.Parameters.Add(p1)
        ad = New SqlDataAdapter(cmd)
        DT = New DataTable
        ad.Fill(DT)
        Contatore = DT.Rows.Count
        If Contatore = 0 Then
            Return 0
        ElseIf Contatore = 1 Then
            Return DT.Rows(0).Item(0)
        ElseIf Contatore > 1 Then
            Dim Cerca As New DxRicercheFrm
            Cerca.Dta = DT
            Cerca.Ar = Field
            Cerca.TT = 0
            Cerca.Text = "Ricerca Codice Ateco"
            Cerca.ShowDialog()
            Return Cerca.codice
        End If
        ad.Dispose()
        DT.Dispose()
    End Function
    Shared Function CercaIstat(ByVal ArgRicerca As String) As String
        Dim StrSelect, Jolly As String
        Dim Contatore As Integer
        Dim i As Int16
        Dim Field As New ArrayList
        Field.Add("IstatCod")
        Field.Add("IstatDesc")
        CercaIstat = ""
        Jolly = "%"
        For i = 0 To ArgRicerca.Length - 1
            If ArgRicerca.Substring(i, 1) = "%" Or ArgRicerca.Substring(i, 1) = "_" Or ArgRicerca.Substring(i, 1) = "[" Then
                Jolly = ""
                Exit For
            End If
        Next
        StrSelect = " SELECT * FROM TbIstat order By IstatCod"
        Dim cmd As New SqlCommand(StrSelect, cnCo)
        Dim p1 As New SqlParameter("@Chiave", SqlDbType.Char)
        p1.Value = ArgRicerca & Jolly
        cmd.Parameters.Add(p1)

        ad = New SqlDataAdapter(cmd)
        DT = New DataTable
        ad.Fill(DT)
        Contatore = DT.Rows.Count
        If Contatore = 0 Then
            Return 0
        ElseIf Contatore = 1 Then
            Return DT.Rows(0).Item(0)
        ElseIf Contatore > 1 Then
            Dim Cerca As New DxRicercheFrm
            Cerca.Dta = DT
            Cerca.Ar = Field
            Cerca.TT = 0
            Cerca.Text = "Ricerca Codice Istat"
            Cerca.ShowDialog()
            Return Cerca.codice
        End If
        ad.Dispose()
        DT.Dispose()
    End Function
    Shared Function CercaCespite(ByVal cod As String, ByVal cod2 As String, ByVal cod3 As String) As String
        Dim StrSelect As String
        Dim Contatore As Integer
        CercaCespite = ""
        StrSelect = " SELECT * FROM TbCsp where CspNum='00' and CspSpe1>0"
        Dim cmd As New SqlCommand(StrSelect, cnCo)
        ad = New SqlDataAdapter(cmd)
        DT = New DataTable
        ad.Fill(DT)
        Contatore = DT.Rows.Count
        If Contatore = 0 Then
            Return 0
        ElseIf Contatore = 1 Then
            Return 1
        ElseIf Contatore > 1 Then
            Dim FCercaCespiti As New DxRicCespiti
            DxRicCespiti.DataTable = DT
            FCercaCespiti.ShowDialog()
            cod = DxRicCespiti.codice
            cod2 = DxRicCespiti.codice2
            cod3 = DxRicCespiti.codice3
            Return cod & cod2 & cod3
        End If
        ad.Dispose()
        DT.Dispose()
    End Function
    Shared Function CercaCee(ByVal ArgRicerca As String) As Integer
        Dim StrSelect, Jolly As String
        Dim Contatore As Integer
        Dim i As Int16
        Dim Field As New ArrayList
        Field.Add("CeeCod")
        Field.Add("CeeDesc")
        Jolly = "%"
        For i = 0 To ArgRicerca.Length - 1
            If ArgRicerca.Substring(i, 1) = "%" Or ArgRicerca.Substring(i, 1) = "_" Or ArgRicerca.Substring(i, 1) = "[" Then
                Jolly = ""
                Exit For
            End If
        Next
        StrSelect = " SELECT * FROM TbCee Where CeeDesc Like @Chiave Order By CeeCod"
        Dim cmd As New SqlCommand(StrSelect, cnCo)
        Dim p1 As New SqlParameter("@Chiave", SqlDbType.Char)
        p1.Value = ArgRicerca & Jolly
        cmd.Parameters.Add(p1)

        ad = New SqlDataAdapter(cmd)
        DT = New DataTable
        ad.Fill(DT)
        Contatore = DT.Rows.Count
        If Contatore = 0 Then
            Return 0
        ElseIf Contatore = 1 Then
            Return DT.Rows(0).Item(0)
        ElseIf Contatore > 1 Then
            Dim FCercaCee As New DxRicercheFrm
            FCercaCee.Dta = DT
            FCercaCee.Ar = Field
            FCercaCee.TT = 0
            FCercaCee.Text = "Ricerca Codice Cee"
            FCercaCee.ShowDialog()
            Return Val(FCercaCee.codice)
        End If
        ad.Dispose()
        DT.Dispose()
    End Function
    Shared Function CercaTrib(ByVal ArgRicerca As String) As String
        CercaTrib = ""
        Dim StrSelect, Jolly As String
        Dim Contatore As Integer
        Dim i As Int16
        Dim Field As New ArrayList
        Field.Add("TribCod")
        Field.Add("TribDesc")
        Jolly = "%"
        For i = 0 To ArgRicerca.Length - 1
            If ArgRicerca.Substring(i, 1) = "%" Or ArgRicerca.Substring(i, 1) = "_" Or ArgRicerca.Substring(i, 1) = "[" Then
                Jolly = ""
                Exit For
            End If
        Next
        StrSelect = "SELECT * FROM TbTrib where TribDesc like @chiave"
        Dim cmd As New SqlCommand(StrSelect, cnCo)
        Dim p1 As New SqlParameter("@Chiave", SqlDbType.VarChar)
        p1.Value = ArgRicerca & Jolly
        cmd.Parameters.Add(p1)

        ad = New SqlDataAdapter(cmd)
        DT = New DataTable
        ad.Fill(DT)
        Contatore = DT.Rows.Count
        If Contatore = 0 Then
            Return ""
        ElseIf Contatore = 1 Then
            Return DT.Rows(0).Item(0)
        ElseIf Contatore > 1 Then
            Dim FCercaTrib As New DxRicercheFrm
            FCercaTrib.Dta = DT
            FCercaTrib.Ar = Field
            FCercaTrib.TT = 0
            FCercaTrib.Text = "Cerca Tributo"
            FCercaTrib.ShowDialog()
            Return FCercaTrib.codice
        End If
        ad.Dispose()
        DT.Dispose()
    End Function
    Shared Function CercaCespiti(ByVal cspgru, ByVal cspspe1, ByVal cspspe2) As String 'Su CCOM
        CercaCespiti = ""
        Dim StrSelect As String
        Dim Contatore As Integer
        StrSelect = "select * from vcespcat where cspgru='" & cspgru & "' and cspspe1='" & cspspe1 & "' and cspspe2='" & cspspe2 & "'"
        Dim cmd As New SqlCommand(StrSelect, cnCo)

        ad = New SqlDataAdapter(cmd)
        DT = New DataTable
        ad.Fill(DT)
        Contatore = DT.Rows.Count
        If Contatore = 0 Then
            Return ""
        ElseIf Contatore = 1 Then
            Return DT.Rows(0).Item("CespNum") & " " & DT.Rows(0).Item("CespAnnoA") & " " & DT.Rows(0).Item("CespCat")
        ElseIf Contatore > 1 Then
            Dim FCercaCespiti As New DxRicCsp
            FCercaCespiti.Dta = DT
            FCercaCespiti.XCspGru = cspgru
            FCercaCespiti.XCspSpe1 = cspspe1
            FCercaCespiti.XCspSpe2 = cspspe2
            FCercaCespiti.ShowDialog()
            Return FCercaCespiti.codice & " " & FCercaCespiti.codice2 & " " & FCercaCespiti.codice3
        End If
        ad.Dispose()
        DT.Dispose()
    End Function
    Shared Function CercaCat(ByVal cspgru, ByVal cspspe1, ByVal cspspe2) As String 'OK su RicercheFrm
        Dim Contatore As Integer
        Dim Field As New ArrayList
        Field.Add("CspNum")
        Field.Add("CspDesc")
        Field.Add("CspPerc")
        CercaCat = ""
        StrSelect = " select * from TbCsp where cspgru='" & cspgru & "' and cspspe1='" & cspspe1 & "' and cspspe2='" & cspspe2 & "' and CspNum<>'00'"
        Dim cmd As New SqlCommand(StrSelect, cnCo)
        ad = New SqlDataAdapter(cmd)
        DT = New DataTable
        ad.Fill(DT)
        Contatore = DT.Rows.Count
        If Contatore = 0 Then
            Return ""
        ElseIf Contatore = 1 Then
            Return DT.Rows(0).Item(0)
        ElseIf Contatore > 1 Then
            Dim FCercaCom As New DxRicercheFrm
            FCercaCom.Dta = DT
            FCercaCom.Ar = Field
            FCercaCom.TT = 0
            FCercaCom.Text = "Ricerca Categoria"
            FCercaCom.ShowDialog()
            Return FCercaCom.codice
        End If
        ad.Dispose()
        DT.Dispose()
    End Function
End Class
