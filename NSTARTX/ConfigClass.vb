Imports System.IO
Imports DXBASE

Public Class ConfigClass
    Private Shared privConfigFile As String
    Shared Pwd As String = CryPassword

    Public Shared ReadOnly Property ConfigFile() As String
        Get
            Return privConfigFile
        End Get
    End Property

    Public Shared Property CnOk() As Boolean
        Get
            Return StrCnSql
        End Get
        Set(ByVal Value As Boolean)
            StrCnSql = Value
        End Set
    End Property

    Shared Sub New()
        Cursor.Current = Cursors.WaitCursor
        Dim NConn As String = ""
        If Dati.Length = 4 Then NConn = Dati(3) REM MultiUser
        privConfigFile = System.Environment.CurrentDirectory + "\Config.xml"
        Dim I As Int16
        Dim ConnectStr As New ArrayList
        Try
            Dim ds As DataSet = New DataSet
            Dim fs As FileStream = New FileStream(ConfigFile, _
                                        FileMode.Open, FileAccess.Read)
            Dim reader As StreamReader = New StreamReader(fs)
            ds.ReadXml(reader)
            fs.Close()
            Dim configs As DataTable = ds.Tables("Configuration")
            Try
                Dim strs() As DataRow = configs.Select("ConfigSetting='ConnectionString'")
                For I = 0 To ds.Tables("Configuration").Rows.Count - 1
                    If Not strs Is Nothing Then
                        ConnectStr.Add(strs(I).Item("Value").ToString.Replace("*", Pwd))
                    End If
                Next
            Catch ex As Exception
            End Try
        Catch ex As Exception
            MsgBox("Caricamento del file di configurazione")
        End Try
        REM MultiUser
        If NConn.Length > 3 Then
            Dim ns As String = ""
            For z As Int16 = 0 To ConnectStr.Count - 1
                If z = 0 Then NomeServer(ConnectStr(z), CryServer)
                ns = ConnectStr(z).ToString.Replace(CryServer, NConn) : ConnectStr(z) = ns
            Next
        End If
        REM Fine MultiUser
        Try
            cnDb = New System.Data.SqlClient.SqlConnection(ConnectStr(0))
            cnDb.Open()
            NomeDb(cnDb.ConnectionString, DbCliente)

            cnVd = New System.Data.SqlClient.SqlConnection(ConnectStr(1))
            cnVd.Open()
            NomeDb(cnVd.ConnectionString, DbVdox)

            cnCo = New System.Data.SqlClient.SqlConnection(ConnectStr(2))
            cnCo.Open()
            NomeDb(cnCo.ConnectionString, DbCoge)

            NomeServer(cnDb.ConnectionString, CryServer)
            CryPassword = Pwd
            StrCnSql = VerificaLicenze(Ist2server(cnDb.DataSource))
            'CnOk = True
        Catch ex As Exception
            'MsgBox("La stringa di connessione non è valida.")
            MsgBox(ex.Message.ToString)
            Return
        Finally
            If cnDb Is Nothing Or cnVd Is Nothing Or cnCo Is Nothing Then
                CnOk = False
            End If
        End Try
        If ConnectStr.Count = 4 Then
            Try
                CnDc = New System.Data.SqlClient.SqlConnection(ConnectStr(3))
                CnDc.Open()
            Catch ex As Exception
            End Try
        End If
    End Sub

    Shared Sub NomeDb(ByVal CnStr As String, ByRef DbName As String)
        Cursor.Current = Cursors.WaitCursor
        Dim x As Int16
        For x = CnStr.Length To 1 Step -1
            If Mid(CnStr, x, 1) = "=" Then
                Exit For
            End If
        Next
        DbName = Mid(CnStr, x + 1, CnStr.Length - x)
    End Sub

    Shared Sub NomeServer(ByVal CnStr As String, ByRef ServerName As String)
        Cursor.Current = Cursors.WaitCursor
        Dim x, J, K As Int16
        For x = 1 To CnStr.Length
            If Mid(CnStr, x, 1) = "=" Then J = x
            If Mid(CnStr, x, 1) = ";" Then
                K = x - J
                Exit For
            End If
        Next
        ServerName = Mid(CnStr, J + 1, K - 1)
    End Sub
End Class
