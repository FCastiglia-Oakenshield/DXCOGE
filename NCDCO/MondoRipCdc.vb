Imports DXBASE
Imports System.Data.SqlClient
Public Class MondoRipCdc
    Private Shared Prot, Nreg, DatDoc, Ndoc As String
    Private Shared IIDD As Int32
    Public Shared Property PNdoc() As String
        Get
            Return Ndoc
        End Get
        Set(ByVal Value As String)
            Ndoc = Value
        End Set
    End Property
    Public Shared Property PDatDoc() As String
        Get
            Return DatDoc
        End Get
        Set(ByVal Value As String)
            DatDoc = Value
        End Set
    End Property
    Public Shared Property PProt() As String
        Get
            Return Prot
        End Get
        Set(ByVal Value As String)
            Prot = Value
        End Set
    End Property
    Public Shared Property PNreg() As String
        Get
            Return Nreg
        End Get
        Set(ByVal Value As String)
            Nreg = Value
        End Set
    End Property
    Public Shared Property PIIDD() As Int32
        Get
            Return IIDD
        End Get
        Set(ByVal Value As Int32)
            IIDD = Value
        End Set
    End Property


    Dim TbCms As DataTable
    Dim DaCms As SqlDataAdapter
    Dim RwD As DataRow

    Dim DsMcc As DataTable
    Dim DaMcc As SqlDataAdapter
    Dim RwMcc As DataRow

    Dim DsRip As DataTable
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow

    Dim DsCpt As DataTable
    Dim DaCpt As SqlDataAdapter
    Dim RwCpt As DataRow


    Dim Max, Ps, Pr As Int16
    Dim Iset1, Iset4 As Integer

    Dim Totale, Parziale As Decimal
    Dim OkRip, OkChiudi As Boolean
    Dim ProgId As Int32

    Private Sub DxRipCdc_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Pulizia()
        PopolaCommesse()
        LeggiRiparto(IIDD)
        CaricaRipartizione(IIDD)
        '''If Max = 0 Then
        '''    EliminaDaPrk()
        '''    OkChiudi = True
        '''    Me.Close()
        '''    Exit Sub
        '''End If
    End Sub
    Sub EliminaDaPrk()
        Dim Cancella As String = "Delete from TbMcc where MCCPRKID = " & IIDD
        Dim Dmd As New SqlCommand(Cancella, CnDc)
        Dmd.ExecuteNonQuery()
    End Sub
    Sub Pulizia()
        TextEdit100.EditValue = IIDD
        TextEdit10.EditValue = Prot
        TextEdit11.EditValue = Nreg
        TextEdit12.EditValue = Ndoc
        DateEdit13.EditValue = CDate(DatDoc)
        Totale = CDec(0.0) : Parziale = CDec(0.0) : ProgId = 0 : Max = 0
        ButtonXF11.Enabled = False
        OkChiudi = False
    End Sub
    Sub PopolaCommesse()
        Dim Str As String = "  Select * from VTCommessa order by TcmAnno desc, TcmNum desc"
        TbCms = New DataTable()
        DaCms = New SqlDataAdapter(Str, cnDb)
        DaCms.Fill(TbCms)
        GridControl1.DataSource = TbCms
        GridView1.ClearSelection()
        GridView1.SelectRow(-1)
    End Sub
    Function LeggiRiparto(ByVal Id As Int32) As Boolean
        Dim Str = "SELECT * FROM FnMONDORip (" & Id & ")"
        DsMcc = New DataTable
        DaMcc = New SqlDataAdapter(Str, CnDc)
        DaMcc.Fill(DsMcc)
        GridControl4.DataSource = DsMcc

        If DsMcc.Rows.Count = 0 Then
            OkRip = False
            DistruggiMccIdOld()
        Else
            OkRip = True
            ProgId = DsMcc.Rows(0).Item("MCCID")
        End If
    End Function

    Sub DistruggiMccIdOld()
        Dim Cancella As String = "Delete from TbMcc where MCCPRKID = " & IIDD
        Dim Dmd As New SqlCommand(Cancella, CnDc)
        Dmd.ExecuteNonQuery()
    End Sub
    Function CaricaRipartizione(ByVal Id As Int32) As Boolean
        Dim x As Int16
        EsegueSql("EXEC XMONDOCPT @ID=" & Id, CnDc)
        Dim Str As String = "SELECT * FROM ##TACPT ORDER BY COGCONTO "
        DsCpt = New DataTable()
        DaCpt = New SqlDataAdapter(Str, CnDc)
        DaCpt.Fill(DsCpt)
        Totale = 0
        For x = 1 To DsCpt.Rows.Count
            RwCpt = DsCpt.Rows(x - 1)
            Totale = Totale + RwCpt("Importo")
        Next
        TextEdit3.EditValue = Totale
        GridControl2.DataSource = DsCpt
        CaricaTotali()
    End Function
    Sub CaricaTotali()
        Dim y, q As Int16
        Parziale = 0
        For q = 1 To DsCpt.Rows.Count
            RwCpt = DsCpt.Rows(q - 1)
            RwCpt("RIPARTITO") = 0
            RwCpt("RESIDUO") = 0
        Next
        DsCpt.AcceptChanges()
        For y = 1 To DsMcc.Rows.Count
            RwMcc = DsMcc.Rows(y - 1)
            Parziale = Parziale + RwMcc("MCCImporto")
            For q = 1 To DsCpt.Rows.Count
                RwCpt = DsCpt.Rows(q - 1)
                If RwMcc("COGCONTO") = RwCpt("COGCONTO") Then
                    RwCpt("RIPARTITO") = RwCpt("RIPARTITO") + RwMcc("MCCIMPORTO")
                    RwCpt("RESIDUO") = RwCpt("IMPORTO") - RwCpt("RIPARTITO")
                End If
            Next
        Next
        For q = 1 To DsCpt.Rows.Count
            RwCpt = DsCpt.Rows(q - 1)
            If (RwCpt("RESIDUO") < 0 And RwCpt("IMPORTO") > 0) Or (RwCpt("RESIDUO") > 0 And RwCpt("IMPORTO") < 0) Then
                RwMcc = DsMcc.Rows(DsMcc.Rows.Count - 1)
                RwMcc("MCCIMPORTO") = RwMcc("MCCIMPORTO") + RwCpt("RESIDUO")
                Parziale = Parziale + RwCpt("RESIDUO")
                RwCpt("RIPARTITO") = RwCpt("IMPORTO")
                TextEdit1.EditValue = CDec(RwMcc("MCCIMPORTO"))
                RwCpt("RESIDUO") = 0
                DsCpt.AcceptChanges()
                DsMcc.AcceptChanges()
                If TextEdit1.EditValue = 0 Then ModificaImporto()
                Exit For
            End If
        Next
        TextEdit2.EditValue = CDec(Parziale)
        If Math.Abs(CDec(TextEdit2.EditValue)) > Math.Abs(CDec(TextEdit3.EditValue)) Then
            TextEdit2.ErrorText = "PARZIALE SUPERIORE AL TOTALE !!!"
        Else
            TextEdit2.ErrorText = ""
        End If
        If DsMcc.Rows.Count > 0 Then
            GridView4.FocusedRowHandle = DsMcc.Rows.Count - 1
        End If
    End Sub
    Function ModificaImporto() As Boolean
        ModificaImporto = True
        If CDec(TextEdit1.EditValue) = 0 Then
            RwMcc = DsMcc.Rows((DsMcc.Rows.Count - 1))
            RiAssegnaDati()
            Exit Function
        End If
        If Math.Abs(CDec(TextEdit1.EditValue)) > Math.Abs(RwMcc("Importo")) Then
            ModificaImporto = False
            Exit Function
        End If
        RwMcc("MccImporto") = CDec(TextEdit1.EditValue)
        DsMcc.AcceptChanges()
        CaricaTotali()
        TextEdit1.EditValue = 0
        TextEdit2.EditValue = 0
        TextEdit3.EditValue = 0
        TextEdit6.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = ""
        TextEdit1.Properties.ReadOnly = True
        ButtonXF11.Enabled = False
        TextEdit1.Focus()
    End Function
    Sub RiAssegnaDati()
        TextEdit1.EditValue = 0
        TextEdit2.EditValue = 0
        TextEdit3.EditValue = 0
        TextEdit6.EditValue = ""
        TextEdit7.EditValue = ""
        TextEdit8.EditValue = ""
        TextEdit1.Properties.ReadOnly = True
        ButtonXF11.Enabled = False
        TextEdit1.Focus()
        ''Pr = Irow
        ''For y = 1 To DsMcc.Rows.Count
        ''    RwMcc = DsMcc.Rows(y - 1)
        ''    If RwMcc("Riga") = k Then Exit For
        ''Next
        If RwMcc IsNot Nothing Then RwMcc.Delete()
        DsMcc.AcceptChanges()
        CaricaTotali()
    End Sub
    Sub AssegnaDati()
        RwMcc = DsMcc.NewRow()
        RwMcc("MCCIMPORTO") = 0

        DsMcc.Rows.Add(RwMcc)
        DsMcc.AcceptChanges()
        CaricaTotali()
    End Sub
#Region "XGRID"
    Private Sub GridControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseMove
        ShowHitInfo(GridView1.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub

    Private Sub ShowHitInfo(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl1.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset1 = hi.RowHandle
    End Sub
    Private Sub GridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Click
        If Iset1 > -1 Then
            RwD = GridView1.GetDataRow(Iset1)
            AssegnaDati()
            Exit Sub
        End If
    End Sub

    Private Sub GridControl4_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl4.MouseMove
        ShowHitInfo4(GridView4.CalcHitInfo(New Point(e.X, e.Y)))
    End Sub
    Private Sub ShowHitInfo4(ByVal hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo)
        Dim cgv As DevExpress.XtraGrid.Views.Base.ColumnView = CType(GridControl4.MainView, DevExpress.XtraGrid.Views.Base.ColumnView)
        Iset4 = hi.RowHandle
    End Sub

    Private Sub GridView4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.Click
        If Iset4 > -1 Then
            RwMcc = GridView4.GetDataRow(Iset4)
            RiAssegnaDati()
        End If
    End Sub
#End Region

End Class