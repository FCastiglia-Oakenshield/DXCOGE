Imports System.Data.SqlClient
Imports System.IO
Imports DXBASE
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.ReportSource
Public Module Stampe
    Public ComandoInizializza As String
    Public ComandoLaser = Chr(27) & "&a0c&a0R" & Chr(27) & "&l0S" & Chr(27) & "(s0p17h4148T" & Chr(27) & Chr(13)
    Function IniziaComando(ByVal LPTIPO As String, ByVal LPRESET As String) As Boolean
        ComandoInizializza = ""
        If LPTIPO = "L" Then ComandoInizializza = ComandoLaser
        If LPTIPO <> "A" Then Exit Function
        Dim x As Int16
        Dim Ok As Boolean = False
        For x = 0 To 4
            If Val(LPRESET.Split(",")(x)) > 0 Then
                ComandoInizializza = ComandoInizializza & Chr(Val(LPRESET.Split(",")(x)))
                Ok = True
            Else
                Exit For
            End If
        Next
        If Ok = True Then ComandoInizializza = ComandoInizializza & Chr(27) & Chr(13) Else ComandoInizializza = ""
    End Function
    Sub EsegueAnteprima(ByVal NOMEFILE As String)
        If File.Exists(NOMEFILE) = True Then Process.Start("wordpad.exe", NOMEFILE)
    End Sub
    Sub PdfStart(ByVal Report As ReportClass, ByVal Stampa As String)
        Dim Str As String = "Select Sel8 from TbSel where selId = 1"
        Cmd = New SqlCommand(Str, cnVd)
        Dim PathTmp As String = Cmd.ExecuteScalar
        'Dim Logoninfo As New TableLogOnInfo
        'Dim X As Int16
        'For X = 1 To Report.Database.Tables.Count
        '    Logoninfo = Report.Database.Tables.Item(X - 1).LogOnInfo
        '    Logoninfo.ConnectionInfo.ServerName = CryServer
        '    Report.Database.Tables.Item(X - 1).ApplyLogOnInfo(Logoninfo)
        'Next
        'Report.SetDatabaseLogon(CryUtente, CryPassword)
        SetCRLogOnInfo(Report, CryServer, CryUtente, CryPassword)
        Dim FileName As String = PathTmp & Stampa & ".pdf"
        Report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, FileName)
        Process.Start("AcroRd32.exe", FileName)
    End Sub
    Function Marchio() As String
        Marchio = ""
        Dim cmd As New SqlCommand(" SELECT * from TbSel where selId = 400", cnVd)
        dataRd = cmd.ExecuteReader
        While dataRd.Read
            Marchio = dataRd.GetString(1)
        End While
        dataRd.Close()
    End Function
    Function Sole24Ore(ByVal NomeFile As String, ByVal Block As Int32) As Boolean
        Dim Riga, Str, Segno As String
        If File.Exists(NomeFile) Then File.Delete(NomeFile)
        Dim output As TextWriter = File.AppendText(NomeFile)
        Cmd = New SqlCommand("SELECT * FROM VSOLE24ORE WHERE TMSBLOCK = " & Block & " ORDER BY PIACODCO", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            If dataRd.Item("CEEST") = 1 And dataRd.Item("SALDO") < 0 Then Segno = "-" Else Segno = "+"
            Str = (Segno & Math.Abs(dataRd.Item("SALDO")).ToString).PadLeft(13, " ")
            Riga = dataRd.Item("Rigo").ToString.PadLeft(5, "0") & ";" & Str
            output.WriteLine(Riga)
        End While
        dataRd.Close()
        output.Close()
    End Function
End Module
