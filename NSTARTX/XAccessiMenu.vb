Imports DXBASE
Public Class XAccessiMenu
    Public ReadOnly Property AccessiX() As ArrayList
        Get
            Return Accessi
        End Get
    End Property

    Public WriteOnly Property VociAbil() As ArrayList
        Set(ByVal Value As ArrayList)
            VociAbilitate = Value
        End Set
    End Property

    Dim Accessi As New ArrayList
    Dim VociAbilitate As New ArrayList
    Dim MenuMergeV As New MenuStrip
    Dim ContaNulli As Int16 = -1

    Private Sub AccessiMenu_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MenuMergeV = MenuMenu
        Me.MenuStrip1.Parent = Me
        Me.MainMenuStrip = Me.MenuStrip1
INIZIOLOOP:
        ContaNulli += 1
        If ContaNulli = MenuMergeV.Items.Count Then GoTo FINELOOP
        If MenuMergeV.Items(ContaNulli).Name = "" Then MenuMergeV.Items.RemoveAt(ContaNulli) : ContaNulli -= 1
        GoTo INIZIOLOOP
FINELOOP:
        For I As Int16 = 0 To MenuMergeV.Items.Count - 1
            '
            'MenuStrip1
            '
            Me.MenuStrip1.Items.Add(MenuMergeV.Items(I).Text)
            Dim XX As System.Windows.Forms.ToolStripMenuItem = MenuStrip1.Items(I)
            Dim yy As System.Windows.Forms.ToolStripMenuItem = MenuMergeV.Items(I)
            XX.Name = MenuMergeV.Items(I).Name
            XX.Text = MenuMergeV.Items(I).Text
            XX.Tag = MenuMergeV.Items(I).Tag
            For HH As Int16 = 1 To VociAbilitate.Count
                If XX.Tag = VociAbilitate.Item(HH - 1) Then XX.Text = "(" & XX.Text.ToLower & ")" : Exit For
            Next
            AddHandler MenuStrip1.Items(I).Click, AddressOf ClicTitolo

            ''''REM SONO QUA
            Dim tt As Int16 = yy.DropDownItems.Count - 1
            For j As Int16 = 0 To tt
                XX.DropDownItems.Add(yy.DropDownItems(j).Text)
                XX.DropDownItems(j).Name = yy.DropDownItems(j).Name
                XX.DropDownItems(j).Text = yy.DropDownItems(j).Text
                XX.DropDownItems(j).Tag = yy.DropDownItems(j).Tag
                AddHandler XX.DropDownItems(j).Click, AddressOf Clic
            Next
            For HH As Int16 = 1 To VociAbilitate.Count
                For j As Int16 = 0 To XX.DropDownItems.Count - 1
                    If XX.DropDownItems(j).Tag = VociAbilitate.Item(HH - 1) Then
                        yy = XX.DropDownItems(j)
                        yy.Checked = True
                        Exit For
                    End If
                Next
            Next
            REM menu non gestibili dall'utente
            If MenuStrip1.Items(I).Text.ToUpper = "RISERVATO" Or MenuMergeV.Items(I).Text.ToUpper = "GEVE" Or MenuMergeV.Items(I).Text.ToUpper = "FINESTRE" Then
                MenuStrip1.Items(I).Visible = False
            End If
        Next
        Me.MenuStrip1.MdiWindowListItem = Nothing
    End Sub
    Private Sub Clic(ByVal sender As Object, ByVal e As System.EventArgs)
        sender.checked = Not sender.checked
        sender.OwnerItem.select()
    End Sub

    Private Sub ClicTitolo(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not sender.DropDownItems.count > 0 Then
            If sender.Text.Chars(0) = "(" Then
                sender.Text = Mid(sender.text.ToUpper, 2, 1) & Mid(sender.text, 3, sender.text.length - 3)
            Else
                sender.Text = "(" & sender.text.tolower & ")"
            End If
        End If
        Dim Okset As Boolean = True
        If Control.ModifierKeys = Keys.Control Then
            For J As Int16 = 0 To sender.DropDownItems.Count - 1
                sender.DropDownItems(J).Checked = Not sender.DropDownItems(J).Checked
                If sender.DropDownItems(J).Checked = False Then Okset = False
            Next
            If Okset = False And sender.Text.Chars(0) = "(" Then sender.Text = Mid(sender.text.ToUpper, 2, 1) & Mid(sender.text, 3, sender.text.length - 3)
        End If
    End Sub

    Private Sub AccessiMenu_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        REM INSERIRE LETTURE TAG -------------------
        For I As Int16 = 0 To MenuStrip1.Items.Count - 1
            Dim XX As System.Windows.Forms.ToolStripMenuItem = MenuStrip1.Items(I)
            If MenuStrip1.Items(I).Text.ToUpper = "FINESTRE" Then GoTo INEXT
            If XX.DropDownItems.Count = 0 Then
                If Mid(MenuStrip1.Items(I).Text, 1, 1) = "(" Then Accessi.Add(MenuStrip1.Items(I).Tag)
                GoTo Inext
            End If
            Dim Cf As Int16 = XX.DropDownItems.Count - 1
            For j As Int16 = 0 To XX.DropDownItems.Count - 1
                Dim YY As System.Windows.Forms.ToolStripMenuItem = XX.DropDownItems(j)
                If YY.Checked = True Then Accessi.Add(XX.DropDownItems(j).Tag) : Cf = Cf - 1
            Next
            If Cf = -1 Then Accessi.Add(MenuStrip1.Items(I).Tag) : GoTo INEXT
INEXT:
        Next
    End Sub

End Class