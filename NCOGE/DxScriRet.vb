Imports DXBASE
Imports NCCOM
Imports System.Data.SqlClient

Public Class DxScriRet
    Dim DsRet As DataTable
    Dim DaRet As SqlDataAdapter
    Dim RwRet As DataRow
    Dim RwX As DataRow
    Dim DataOperazione As Date
    Dim Irow As Int32

    Private Sub DxScriRet_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Inizia()
        Popolagrid()
    End Sub

    Sub Inizia()
        Dim str As String = "SELECT isNull(MAX(RetData),isnull((SELECT min(pridatagio) from tbpri where datepart(year,pridatagio)=datepart(year,getdate())),getdate())) from TRRET"
        Cmd = New SqlCommand(str, cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            DataOperazione = CDate(dataRd.Item(0))
        End While
        dataRd.Close()
        REM USO IL GIORNO 15 PER EVITARE DI SPOSTARE IL MESE NELLA SETTIMANA ULTIMA'''' MOLTO SOTTILE''''

        GroupControl2.Visible = False
        DateNavigator1.DateTime = CDate("15/" & DataOperazione.Month & "/" & DataOperazione.Year).ToShortDateString
        DisEnab(1)
    End Sub
    Sub DisEnab(ByVal n As Int16)
        If n = 0 Then
            GroupControl8.Enabled = False
            GroupControl3.Enabled = True
            GridControl1.Enabled = False
        Else
            GroupControl8.Enabled = True
            GroupControl3.Enabled = False
            GridControl1.Enabled = True
        End If
    End Sub
    Sub Popolagrid()
        DsRet = New DataTable("TRRET")
        DaRet = New SqlDataAdapter("Select * from TRRET where RetData ='" & DataOperazione.ToShortDateString & "'", cnCo)
        DaRet.SelectCommand.CommandTimeout = 300
        DaRet.Fill(DsRet)
        GridControl1.DataSource = DsRet
        GridControl1.Refresh()
        GridView1.ClearSelection()
    End Sub

    Private Sub ButtonF11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF11.Click
        DataOperazione = DataOut(DateNavigator1.DateTime)
        Popolagrid()
        DisEnab(1)
    End Sub

    Private Sub ButtonF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF5.Click
        DisEnab(0)
    End Sub

    Sub LanciaProgramma(ByVal II As Integer)
        Dim Gesterna As New DxScriRPn
        DxScriRPn.NRifArt = II
        DataOperazione = DataOut(DataOperazione)
        DxScriRPn.NDATAOPERA = DataOperazione.ToShortDateString
        Gesterna.WindowState = FormWindowState.Maximized
        Gesterna.ShowDialog()
        If II > 0 Then
            Dim UpUp As New SqlCommand("Update TrRet set RetData=(select distinct PRIDATAGIO FROM TRPRI WHERE RETID = PriId)", cnCo)
            UpUp.ExecuteNonQuery()
        End If
        Popolagrid() : Irow = -1
    End Sub

    Private Sub ButtonF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF1.Click
        LanciaProgramma(Irow)
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
            RwX = GridView1.GetDataRow(iset)
            Irow = RwX("RetArticolo")
        End If
    End Sub
    Private Sub ButtonF6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF6.Click
        REM reset date
        DateNavigator2.Enabled = True
        DateNavigator2.DateTime = CDate("15/" & DataOperazione.Month & "/" & DataOperazione.Year).ToShortDateString
        DateNavigator3.DateTime = DateNavigator2.DateTime.AddMonths(1)
        DateNavigator2.Enabled = False
        GroupControl2.Visible = True
        GroupControl8.Enabled = False
    End Sub

    Private Sub ButtonFF5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF5.Click
        GroupControl2.Visible = False
        GroupControl8.Enabled = True
    End Sub

    Private Sub ButtonFF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFF1.Click
        Cursor.Current = Cursors.WaitCursor
        AvviaDuplicazione()
        ButtonFF5.PerformClick()
        Inizia()
        Popolagrid()
    End Sub
    Sub AvviaDuplicazione()
        Dim DAL, AL As String
        DAL = DataOperazione.ToShortDateString
        AL = DataOut(DateNavigator3.DateTime).ToShortDateString
        EsegueSql("EXEC RDUPLICA @DAL ='" & DAL & "', @AL = '" & AL & "'", cnCo)
    End Sub
    Function DataOut(ByVal MM As Date) As Date
        Dim Mese, Giorno, Anno As String
        Mese = MM.Month.ToString.PadLeft(2, "0")
        Anno = MM.Year
        Giorno = Date.DaysInMonth(Anno, Mese)
        Return CDate(Giorno & "/" & Mese & "/" & Anno)
    End Function
    Private Sub ScriRet_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.F1 And GroupControl8.Enabled = True Then
            e.Handled = True
            ButtonF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F1 And GroupControl2.Enabled = True Then
            e.Handled = True
            ButtonFF1.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F6 Then
            e.Handled = True
            ButtonF6.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 And GroupControl8.Enabled = True Then
            e.Handled = True
            ButtonF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F5 And GroupControl2.Enabled = True Then
            e.Handled = True
            ButtonFF5.PerformClick()
            Exit Sub
        End If
        If e.KeyData = Keys.F11 Then
            e.Handled = True
            ButtonF11.PerformClick()
            Exit Sub
        End If
    End Sub
End Class