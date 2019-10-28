Imports NCCOM
Imports DXBASE
Imports System.Data.SqlClient
Public Module IvaSim
    Public Function IvaSimulata(ByVal DataAl As String, ByVal DM As Int16, ByVal AM As Int16) As Boolean
        Dim IdBlk, Y, x As Int32
        Dim Tr As String = "TReg"
        Dim StrUno As String
        Dim DsReg As DataSet
        Dim DaReg As SqlDataAdapter
        Dim RwReg As DataRow
        Dim NEWDATE As Date = CDate(DataAl).AddDays(15)
        If CDate(DataAl).Month = 12 Then NEWDATE = DataAl
        If CDate(DataAl).Year < 2019 Then NEWDATE = DataAl
        StrUno = "Select * from FnFotoChIva('" & NEWDATE & "','" & DataAl & "')"
        DsReg = New DataSet
        DaReg = New SqlDataAdapter(StrUno, cnCo)
        DaReg.SelectCommand.CommandTimeout = 300
        DaReg.Fill(DsReg, Tr)
        IdBlk = semaforo("Registri Iva AL " & Today.Date)
        Y = DsReg.Tables(Tr).Rows.Count
        Dim Ultimo As New SqlCommand("BEGIN DELETE FROM TMPIVAS DELETE FROM TMPCORS END", cnCo)
        If Y > 0 Then Ultimo.ExecuteNonQuery()
        For x = 1 To Y
            RwReg = DsReg.Tables(Tr).Rows(x - 1)
            ScaricaDati(RwReg, IdBlk, DataAl, CDate(DataAl).Year, DM, AM)
        Next
        Ultimo = New SqlCommand("BEGIN DELETE FROM TMPREGIVA WHERE PREGID = " & IdBlk & " DELETE FROM TMPIVAP WHERE TIVAPID = " & IdBlk & " DELETE FROM TMPCORR WHERE TCORRID = " & IdBlk & " END ", cnCo)
        Ultimo.ExecuteNonQuery()
    End Function
    Sub ScaricaDati(ByVal RwReg As DataRow, ByVal Idblk As Int32, ByVal d1 As String, ByVal ANNO As Int16, ByVal DM As Int16, ByVal AM As Int16)
        Dim StrReg, Elettronica As String
        Dim NRREG, MESE, TIPOREG As Int16
        NRREG = RwReg("RivaNReg")
        TIPOREG = RwReg("RivaTipo")
        ' QUALEREG = DataGrid1.Item(Riga, 2)
        ' DESCREG = DataGrid1.Item(Riga, 3)
        MESE = CDate(d1).Month
        If ANNO < 2019 Then Elettronica = "XI1" Else Elettronica = "XI12019"
        Try
            Cmd = New SqlCommand("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TbIVA2018]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) BEGIN Select 1 END", cnCo)
            If Cmd.ExecuteScalar > 0 Then Elettronica = "XI1"
        Catch ex As Exception

        End Try
        EsegueSql(" EXEC " & Elettronica & "  @FAL ='" & d1 & "' , @REG = " & NRREG & ", @DBVDOX = '" & DbVdox & "', @BLOCK = " & Idblk, cnCo)
        'carica iva periodo
        Dim StrDue As String
        Dim y As Int32
        Dim Ti As String = "TIva"
        Dim DsIva As DataSet
        Dim DaIva As SqlDataAdapter
        Dim RwIva As DataRow

        Dim Tp As String = "TPva"
        Dim DsPva As DataSet
        Dim DaPva As SqlDataAdapter
        Dim RwPva As DataRow
        Dim CbPva As SqlCommandBuilder

        Dim Tc As String = "TCrs" ''' CORRISPETTIVI LORDI PER CPT
        Dim DsCrs As DataSet = Nothing
        Dim DaCrs As SqlDataAdapter = Nothing
        Dim RwCrs As DataRow
        Dim CbCrs As SqlCommandBuilder
        Dim CiiPam As New ArrayList
        REM lettura codici iva pubblica amministrazione
        CiiPam.Clear()
        Cmd = New SqlCommand("SELECT * from TbPaCii where PaTipo = 'IVA' order by PaCodIva", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            CiiPam.Add(dataRd.Item("PaCodIva"))
        End While
        dataRd.Close()
        StrReg = "Select * from CRREGIVA WHERE PRegNumReg = " & NRREG & " AND PREGID = " & Idblk & " ORDER BY PRegNumReg, PRegNumProt, PRegProtBis, PRegPriId,PRegPriProg"
        DsIva = New DataSet
        DaIva = New SqlDataAdapter(StrReg, cnCo)
        DaIva.SelectCommand.CommandTimeout = 300
        DaIva.Fill(DsIva, Ti)
        StrDue = "Select * from TMPIVAP WHERE TivaPanno = 2995 " ' solo per inizializzare il dataset
        DsPva = New DataSet
        DaPva = New SqlDataAdapter(StrDue, cnCo)
        DaPva.Fill(DsPva, Tp)
        CbPva = New SqlCommandBuilder(DaPva)
        If TIPOREG = 5 Then
            Dim StrTre As String
            StrTre = "Select * from TMPCORR WHERE TCorrAnno = 2995 " ' solo per inizializzare il dataset
            DsCrs = New DataSet
            DaCrs = New SqlDataAdapter(StrTre, cnCo)
            DaCrs.Fill(DsCrs, Tc)
            CbCrs = New SqlCommandBuilder(DaCrs)
        End If
        Dim Resto, NoDet, Merce As Decimal
        Resto = 0
        NoDet = 0
        Merce = 0
        If DsIva.Tables(Ti).Rows.Count = 0 Then GoTo InFine
        For y = 1 To DsIva.Tables(Ti).Rows.Count
            RwIva = DsIva.Tables(Ti).Rows(y - 1)
            If RwIva("PRegCodIva") = 0 Then
                Resto = Resto + RwIva("PRegImpon")
                If RwIva("PRegMerce") = 2 Then
                    Merce = Merce + RwIva("PRegImpon")
                End If
                GoTo Oltre
            End If
            RwPva = DsPva.Tables(Tp).NewRow
            RwPva("TIvaPId") = RwIva("PRegId")
            RwPva("TIvaPAnno") = CDate(RwIva("PRegFinoAl")).Year
            RwPva("TIvaPMese") = CDate(RwIva("PRegFinoAl")).Month
            RwPva("TIvaPRegIva") = RwIva("PRegNumReg")
            RwPva("TIvaPCodIva") = RwIva("PRegCodIva")
            RwPva("TIvaPImpon") = RwIva("PRegImpon") + Resto
            If RwIva("PRegPND") > 0 Then
                NoDet = RwIva("PRegImpIva") * RwIva("PRegPND") / 100
                RwPva("TIvaPIvaND") = Format(NoDet, "0000000000.00")
                RwPva("TIvaPIvaDE") = RwIva("PRegImpIva") - RwPva("TIvaPIvaND")
            Else
                RwPva("TIvaPIvaDE") = RwIva("PRegImpIva")
                RwPva("TIvaPIvaND") = 0
            End If
            REM CONTROLLO SE PUBBLICA AMMINISTRAZIONE
            For J As Int16 = 1 To CiiPam.Count
                If RwIva("PRegCodIva") = CiiPam(J - 1) Then
                    RwPva("TIvaPIvaDE") = 0
                    RwPva("TIvaPIvaND") = RwIva("PRegImpIva")
                    Exit For
                End If
            Next
            ''
            If RwIva("PRegMerce") = 2 Then
                RwPva("TIvaPImpMerce") = RwIva("PRegImpon")
            Else
                RwPva("TIvaPImpMerce") = 0
            End If
            RwPva("TIvaPImpMerce") = RwPva("TIvaPImpMerce") + Merce
            Resto = 0
            Merce = 0
            DsPva.Tables(Tp).Rows.Add(RwPva)
            If TIPOREG = 5 Then
                RwCrs = DsCrs.Tables(Tc).NewRow
                RwCrs("TCorrId") = RwIva("PRegId")
                RwCrs("TCorrAnno") = CDate(RwIva("PRegFinoAl")).Year
                RwCrs("TCorrMese") = CDate(RwIva("PRegFinoAl")).Month
                RwCrs("TCorrRegIva") = RwIva("PRegNumReg")
                RwCrs("TCorrCodIva") = RwIva("PRegCodIva")
                RwCrs("TCorrLordo") = RwIva("PRegImpon")
                RwCrs("TCorrConto") = RwIva("PRegCpt")
                DsCrs.Tables(Tc).Rows.Add(RwCrs)
            End If
Oltre:
        Next
        DaPva.Update(DsPva, Tp)
        DsPva.AcceptChanges()
        If TIPOREG = 5 Then
            DaCrs.Update(DsCrs, Tc)
            DsCrs.AcceptChanges()
        End If
InFine:
        If TIPOREG = 5 Then
            AddIva(NRREG, "ADDCRSP", ANNO, DM, AM, Idblk)
            AddIva(NRREG, "SUMCRSS", ANNO, DM, AM, Idblk)
        End If
        AddIva(NRREG, "ADDIVAP", ANNO, DM, AM, Idblk)
        AddIva(NRREG, "SUMIVAS", ANNO, DM, AM, Idblk)
    End Sub
    Sub AddIva(ByVal NRREG As Int16, ByVal XP As String, ByVal ANNO As Int16, ByVal DM As Int16, ByVal AM As Int16, ByVal Idblk As Int32)
        'xp = addiva Aggiunge Iva Precedente, xp = sumiva Sostituisce iva Totale
        EsegueSql(" EXEC " & XP & " @ANNO = " & ANNO & ", @REG = " & NRREG & ", @DM = " & DM & ", @AM = " & AM & ", @BLOCK = " & Idblk, cnCo)
    End Sub
End Module
