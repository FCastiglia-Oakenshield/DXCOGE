Imports DXBASE
Imports System.Data.SqlClient

Public Class Query
    Shared ds As DataSet
    Shared ad As SqlDataAdapter
    Shared Function Cercaservizi() As String 'Su geve
        Dim StrSelect As String
        Dim Contatore As Integer
        Dim i As Int16

        StrSelect = " SELECT * FROM TbSer"
        Dim cmd As New SqlCommand(StrSelect, cnDb)

        ad = New SqlDataAdapter(cmd)
        ds = New DataSet
        ad.Fill(ds, "TbSer")
        Contatore = ds.Tables("TbSer").Rows.Count
        If Contatore = 0 Then
            Return 0
        ElseIf Contatore = 1 Then
            Return ds.Tables("TbSer").Rows(0).Item("SerCod")
        ElseIf Contatore > 1 Then
            Dim FCercaSer As New frmRicSer
            FCercaSer.dataset = ds
            FCercaSer.Text = "Ricerca Servizio"
            FCercaSer.ShowDialog()
            Return FCercaSer.codice
        End If
        ad.Dispose()
        ds.Dispose()
    End Function
    Shared Function RicFatbyNum(ByVal Anno As Int16, ByVal Reg As Int16, ByVal numero As Int32) As Int32
        Dim leggi As String
        Dim ad As SqlDataAdapter
        Dim tb As DataTable


        leggi = "Select * from VRicFat where datepart(year,Data) = @Anno and numero = @numero and Registro = @Reg order by registro, data desc"

        Dim p1 As New SqlParameter("@Anno", SqlDbType.SmallInt)
        Dim p2 As New SqlParameter("@Reg", SqlDbType.SmallInt)
        Dim p3 As New SqlParameter("@Numero", SqlDbType.Int)

        p1.Value = Anno
        p2.Value = Reg
        p3.Value = numero

        ad = New SqlDataAdapter(leggi, cnDb)

        ad.SelectCommand.Parameters.Add(p1)
        ad.SelectCommand.Parameters.Add(p2)
        ad.SelectCommand.Parameters.Add(p3)


        Cursor.Current = Cursors.WaitCursor
        tb = New DataTable("DOCUM")
        ad.Fill(tb)
        Cursor.Current = Cursors.Default

        If tb.Rows.Count = 0 Then
            Return 0
        End If

        If tb.Rows.Count = 1 Then
            Return tb.Rows(0)("Numrif")
        End If

        Dim frm As New RicDoc
        frm.Dati = tb
        frm.ShowDialog()

        Return frm.Riferimento
    End Function
    Shared Function RicDocbyNum(ByVal tipo As String, ByVal numero As Int32, ByVal anno As Int16) As Int32
        Dim leggi As String
        Dim ad As SqlDataAdapter
        Dim tb As DataTable

        leggi = "Select * from VRicDoc where numero = @numero and Tipo = @Tipo  and datepart(year,data) = @Anno order by data desc"

        ad = New SqlDataAdapter(leggi, cnDb)

        Dim p1 As New SqlParameter("@Tipo", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@Numero", SqlDbType.Int)
        Dim p3 As New SqlParameter("@Anno", SqlDbType.SmallInt)
        p1.Value = tipo
        p2.Value = numero
        p3.Value = anno
        ad.SelectCommand.Parameters.Add(p1)
        ad.SelectCommand.Parameters.Add(p2)
        ad.SelectCommand.Parameters.Add(p3)

        Cursor.Current = Cursors.WaitCursor
        tb = New DataTable("DOCUM")
        ad.Fill(tb)
        Cursor.Current = Cursors.Default

        If tb.Rows.Count = 0 Then
            Return 0
        End If

        If tb.Rows.Count = 1 Then
            Return tb.Rows(0)("Numrif")
        End If

        Dim frm As New RicDoc
        frm.Dati = tb
        frm.ShowDialog()

        Return frm.Riferimento
    End Function
    Shared Function CercaCii() As Int16
        Dim DT As DataTable
        Dim StrSelect As String
        Dim Contatore As Integer
        Dim i As Int16
        StrSelect = " SELECT * FROM TbCii Order By CiiCod"
        Dim cmd As New SqlCommand(StrSelect, cnCo)
        Dim Field As New ArrayList
        Field.Add("CiiCod")
        Field.Add("CiiDes")
        ad = New SqlDataAdapter(cmd)
        DT = New DataTable
        ad.Fill(DT)
        Contatore = DT.Rows.Count
        If Contatore = 0 Then
            Return 0
        ElseIf Contatore = 1 Then
            Return DT.Rows(0).Item(0)
        ElseIf Contatore > 1 Then
            Dim FCercaCii As New NCCOM.DxRicercheFrm
            FCercaCii.Dta = DT
            FCercaCii.Ar = Field
            FCercaCii.TT = 0
            FCercaCii.Text = "Ricerca Codici Iva"
            FCercaCii.ShowDialog()
            Return Val(FCercaCii.codice)
        End If
        ad.Dispose()
        DT.Dispose()


    End Function
End Class


