﻿Imports System.Net

Public Class FormDebug

    Private _backup As Boolean
    Private _command As String
    Private _value As Boolean

#Region "FormPos"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ScreenPos

    Public Property ScreenPosition As ScreenPos
        Get
            Return _screenPosition
        End Get
        Set(value As ScreenPos)
            _screenPosition = value
        End Set
    End Property

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()
        Me.Show()
        ScreenPosition = New ScreenPos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Properties"

    Public Property Backup As Boolean
        Get
            Return _backup
        End Get
        Set(value As Boolean)
            _backup = value
        End Set
    End Property

    Public Property Command As String
        Get
            Return _command
        End Get
        Set(value As String)
            _command = value
        End Set
    End Property

    Public Property Value As Boolean
        Get
            Return _value
        End Get
        Set(value As Boolean)
            _value = value
        End Set
    End Property

#End Region

#Region "Scrolling text box"

    Public Sub ProgressPrint(Value As String)
        Log(My.Resources.Info_word, Value)
        TextBox1.Text += Value & vbCrLf
        If TextBox1.Text.Length > TextBox1.MaxLength - 1000 Then
            TextBox1.Text = Mid(TextBox1.Text, 1000)
        End If
    End Sub

    Private Sub TextBox1_Changed(sender As System.Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim ln As Integer = TextBox1.Text.Length
        TextBox1.SelectionStart = ln
        TextBox1.ScrollToCaret()
    End Sub

#End Region

#Region "Set"

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ApplyButton.Click

        If Command = My.Resources.Speak Then
            Speechtest()

        ElseIf Command = My.Resources.SmartStartEnable Then
            EnableSS()

        ElseIf Command = My.Resources.TeleportAPI Then

            TPAPITest()

        ElseIf Command = My.Resources.Send_alert Then
            If Value Then
                Dim UserId = InputBox("Agent UUID?")
                RPC_admin_dialog(UserId, "Pop up Alert Test")
            Else
                MsgBox("Needs an avatar UUID", vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
            End If

        ElseIf Command = $"{My.Resources.Debug_word} {My.Resources.Off}" Then

            If Value Then
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} {My.Resources.Off}")
            Else
                ProgressPrint(My.Resources.Unchanged)
            End If
            Settings.SaveSettings()

        ElseIf Command = $"{My.Resources.Debug_word} 1 {My.Resources.Minute}" Then

            If Value Then
                Settings.StatusInterval = 60
                ProgressPrint($"{My.Resources.Debug_word} 1 {My.Resources.Minute}")
            Else
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} {My.Resources.Off}")
            End If
            Settings.SaveSettings()

        ElseIf Command = $"{My.Resources.Debug_word} 10 {My.Resources.Minutes}" Then

            If Value Then
                Settings.StatusInterval = 600
                ProgressPrint($"{My.Resources.Debug_word} 10 {My.Resources.Minutes}")
            Else
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} Off")
            End If
            Settings.SaveSettings()

        ElseIf Command = $"{My.Resources.Debug_word} 60 {My.Resources.Minutes}" Then

            If Value Then
                Settings.StatusInterval = 3600
                ProgressPrint($"{My.Resources.Debug_word} 60 {My.Resources.Minutes}")
            Else
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} {My.Resources.Off}")
            End If
            Settings.SaveSettings()

        ElseIf Command = $"{My.Resources.Debug_word} 24 {My.Resources.Hours}" Then

            If Value Then
                Settings.StatusInterval = 60 * 60 * 24
                ProgressPrint($"{My.Resources.Debug_word} 24 {My.Resources.Hours}")
            Else
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} {My.Resources.Off}")
            End If
            Settings.SaveSettings()

        End If

    End Sub

    Private Sub Speechtest()

        ProgressPrint(Speach(My.Resources.HelloToSpeech, Value))

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        Command = CStr(ComboBox1.SelectedItem)
        Value = RadioTrue.Checked

    End Sub

    Private Sub EnableSS()
        If Value Then
            ProgressPrint(My.Resources.SSisEnabled)
            Settings.SSVisible = True
            Settings.SmartStart = True
            HelpManual("SmartStart")
        Else
            ProgressPrint(My.Resources.SSisDisabled)
            Settings.SSVisible = False
            Settings.SmartStart = False
        End If
        Settings.SaveSettings()
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        ComboBox1.Items.Add(My.Resources.SmartStartEnable)

        RadioTrue.Checked = False
        RadioFalse.Checked = True

        RadioTrue.Text = My.Resources.True_word
        RadioFalse.Text = My.Resources.False_word
        ComboBox1.Items.Add(My.Resources.Speak)
        ComboBox1.Items.Add(My.Resources.Send_alert)
        ComboBox1.Items.Add(My.Resources.TeleportAPI)
        ComboBox1.Items.Add($"{My.Resources.Debug_word} {My.Resources.Off}")

        ComboBox1.Items.Add($"{My.Resources.Debug_word} 1 {My.Resources.Minute}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 10 {My.Resources.Minutes}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 60 {My.Resources.Minutes}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 24 {My.Resources.Hours}")

        SetScreen()

    End Sub

    Private Sub NewMethod()
        If Value Then
            Dim region = ChooseRegion(False)
            Dim UUID = Guid.NewGuid().ToString
            Dim url = $"http://{Settings.PublicIP}:{Settings.DiagnosticPort}/alt={region}&agent=Wifi%20Admin&AgentID={UUID}&password={Settings.MachineID}"
            ProgressPrint(url)
            Using client As New WebClient ' download client for web pages
                Dim r As String = ""
                Try
                    r = client.DownloadString(url)
                Catch ex As Exception
                    ProgressPrint(ex.Message)
                End Try
                ProgressPrint(r)
            End Using
        End If
    End Sub

    Private Sub TPAPITest()

        If Value Then
            Dim region = ChooseRegion(False)
            Dim UUID = Guid.NewGuid().ToString
            Dim AviName = InputBox("Avatar Name?")
            Dim AviUUID As String = ""
            If AviName.Length > 0 Then
                AviUUID = Uri.EscapeDataString(MysqlInterface.GetAviUUUD(AviName))
                If AviUUID.Length > 0 Then
                    Dim url = $"http://{Settings.PublicIP}:{Settings.DiagnosticPort}/alt={region}&agent=AviName&AgentID={AviUUID}&password={Settings.MachineID}"
                    ProgressPrint(url)
                    Using client As New WebClient ' download client for web pages
                        Dim r As String = ""
                        Try
                            r = client.DownloadString(url)
                        Catch ex As Exception
                            ProgressPrint(ex.Message)
                        End Try
                        ProgressPrint(r)
                    End Using
                Else
                    ProgressPrint("Avatar Not located")
                End If
            Else
                ProgressPrint($"{My.Resources.Aborted_word} ")
            End If
        End If

    End Sub

#End Region

#Region "Radio"

    Private Sub RadioTrue_CheckedChanged(sender As Object, e As EventArgs) Handles RadioTrue.CheckedChanged

        Value = RadioTrue.Checked

    End Sub

#End Region

End Class