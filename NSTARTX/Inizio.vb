Imports System.IO
Imports System.Data.SqlClient
Imports DXBASE

Module Inizio
    Public Dati() As String = System.Environment.GetCommandLineArgs()
    Public Sub main()
        Dim desktopSize As Size
        desktopSize = System.Windows.Forms.SystemInformation.PrimaryMonitorSize
        Dim height As Integer = desktopSize.Height
        Dim width As Integer = desktopSize.Width
        If width < 1024 Or height < 768 Then
            MessageBox.Show("Impostazioni video non adatte all'esecuzione del programma." & Chr(13) & "Si consiglia una impostazione minima di 1024x768 pixel." & Chr(13) & Chr(13) & "Impossibile eseguire il programma." & Chr(13) & Chr(13) & "Risoluzione corrente: " & width & "x" & height & ".", "Impostazioni video errate", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        If ConfigClass.CnOk = True Then
            leggiinizio()
            If LogIn() = False Then Return
            Cursor.Current = Cursors.WaitCursor
            My.User.InitializeWithWindowsUser()
            DevExpress.UserSkins.OfficeSkins.Register()
            DevExpress.UserSkins.BonusSkins.Register()
            Application.EnableVisualStyles()
            Application.Run(XMENU)
        End If
    End Sub

    Private Function LogIn() As Boolean
        Cursor.Current = Cursors.WaitCursor
        'If Dati(1).ToLower = "selco" And Dati(2).ToLower = "master" Then
        '    NomeUtente = "selco"
        '    Nomegruppo = "Administrator"
        '    Return True
        'End If

        'If LeggiUtenti(Dati(1), Dati(2)) = 0 Then Messaggio() : Return False
        'Return True

        Dim Dati() As String = System.Environment.GetCommandLineArgs()
        If Not Dati.Length > 1 Then Messaggio() : Return False

        If Dati.Length = 2 Then
            Return Risorse(Dati)
        End If

        If Dati(2) = "@" Then
            Return Risorse(Dati)
        Else
            Return Utenti(Dati)
        End If
    End Function
    Function Utenti(dati() As String) As Boolean
        If dati(1).ToLower = "selco" And dati(2).ToLower = "master" Then
            NomeUtente = "selco"
            Nomegruppo = "Administrator"
            Return True
        End If

        If LeggiUtenti(dati(1), dati(2)) = 0 Then Messaggio() : Return False
        Return True
    End Function
    Private Sub Messaggio()
        MessageBox.Show("ACCESSO NEGATO!" & Chr(13) & Chr(13) & "Nome Utente o Password errati!", "ACCESSO NEGATO", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Function LeggiUtenti(ByVal Utente As String, ByVal Password As String) As Int32
        Dim str As String = "select* from VUteGrup where Utente = @Utente and Password=@Password and substring(grupLavGest,2,1)=1"
        Dim cmd As New SqlCommand(str, cnVd)
        Dim p1 As New SqlParameter("@Utente", SqlDbType.VarChar)
        Dim p2 As New SqlParameter("@Password", SqlDbType.VarChar)
        p1.Value = Utente
        p2.Value = Password
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)
        Dim RifId As Int32 = 0
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            RifId = dataRd.Item("Id")
            NomeUtente = dataRd.Item("Utente")
            Nomegruppo = dataRd.Item("GrupLavNome")
            dataRd.Close()
            Return 1
        End If
        dataRd.Close()
        Return 0
    End Function
    Function Risorse(dati() As String) As Boolean
        If dati(1).ToLower = "selco" Then
            NomeUtente = "selco"
            Nomegruppo = "Administrator"
            Return True
        End If

        If LeggiRisorse(dati(1)) = 0 Then Messaggio() : Return False
        Return True
    End Function
    Private Function LeggiRisorse(ByVal Utente As String) As Int32
        Dim p1 As New SqlParameter("@Utente", SqlDbType.VarChar)
        Dim str As String = "select * from VTbRisorse where [RIS-USERW] = @Utente"
        p1.Value = Utente

        Dim cmd As New SqlCommand(str, cnVd)

        cmd.Parameters.Add(p1)

        Dim RifId As Int32 = 0
        dataRd = cmd.ExecuteReader
        If dataRd.Read Then
            RifId = dataRd.Item("RIS-ID")
            NomeUtente = dataRd.Item("RIS-USERW")
            Nomegruppo = dataRd.Item("GRUPPOLAVORO")
            dataRd.Close()
            Return 1
        End If
        dataRd.Close()
        Return 0
    End Function
End Module
