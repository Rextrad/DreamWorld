Imports System.Net

Public Class FormDebug

    Private _backup As Boolean
    Private _command As String
    Private _value As Boolean
    Private BotPID As Integer

#Region "FormPos"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(Me.Name)
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

    Private Sub ProgressPrint(Value As String)
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

    Private Shared Sub Createbots(N As Integer)

        While N > 1
            Dim C = GetAviCountByName("Ima", $"Bot{N - 1}")
            If C = 0 Then
                TextPrint($"Creating Ima Bot{N - 1}")
                ConsoleCommand(RobustName, $"create user Ima Bot{N - 1} {Settings.Password & "xyz"} not@set.yet ~~")
            End If
            N -= 1
        End While

    End Sub

    Private Shared Sub MakeMap()

        Try

            Dim NewMap = New FormGlobalMap

            NewMap.Show()
            NewMap.Activate()
            NewMap.Select()
            NewMap.BringToFront()
        Catch
        End Try

    End Sub

    Private Shared Sub Poketest()
        For Each RegionUUID In RegionUuids()
            Thaw(RegionUUID)
            ConsoleCommand(RegionUUID, "show stats")
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ApplyButton.Click

        If Command = My.Resources.MakeNewMap Then
            MakeMap()

        ElseIf Command = My.Resources.TeleportAPI Then

            TPAPITest()

        ElseIf Command = "All region stats" Then

            Poketest()

        ElseIf Command = "Load Bots" Then

            If Value Then
                LoadBots()
            Else
                StopBot()
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

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        Command = CStr(ComboBox1.SelectedItem)
        Value = RadioTrue.Checked

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        My.Application.ChangeUICulture(Settings.Language)
        My.Application.ChangeCulture(Settings.Language)

        RadioTrue.Checked = False
        RadioFalse.Checked = True

        RadioTrue.Text = My.Resources.True_word
        RadioFalse.Text = My.Resources.False_word

        ComboBox1.Items.Add("All region stats")
        ComboBox1.Items.Add("Load Bots")
        ComboBox1.Items.Add(My.Resources.TeleportAPI)
        ComboBox1.Items.Add($"{My.Resources.Debug_word} {My.Resources.Off}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 1 {My.Resources.Minute}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 10 {My.Resources.Minutes}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 60 {My.Resources.Minutes}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 24 {My.Resources.Hours}")

        ComboBox1.Items.Add(My.Resources.MakeNewMap)

        SetScreen()

        HelpOnce("Debug")

    End Sub

    Private Sub LoadBots()

        PrintBotHelp()

        Dim out As Integer
        Dim N = InputBox("How many bots?  0-100 ", "pCampBots", "1")
        If Integer.TryParse(N, out) Then
            If out > 100 Then out = 0

            Dim RegionName = ChooseRegion(True) ' Just running regions
            If RegionName.Length = 0 Then Return
            Dim RegionUUID = FindRegionByName(RegionName)

            If out > 0 Then RunBot(RegionUUID, out)
            If out = 0 Then StopBot()

        End If

    End Sub

    Private Sub PrintBotHelp()

        'pCambot.exe -botcount 1 -loginuri http://127.0.0.1:6002 -start Normal -firstname Ima  -lastname Bot -password 123xyz

        Dim help As New List(Of String) From {
                "For more information, type 'help all' to get a list of all commands,or type help <item>' where <item> is one of the following:",
                "add behaviour <abbreviated-name> [<bot-number>] - Add a behaviour to a bot.",
                "connect [<n>] - Connect bots.",
                "disconnect [<n>] - Disconnect bots.",
                "quit - Shutdown bots and exit.",
                "remove behaviour <abbreviated-name> [<bot-number>] - Remove a behaviour from a bot.",
                "set bots <key> <value> - Set a setting for all bots.",
                "show bot <bot-number> - Shows the detailed status And settings of a particular bot.",
                "show bots - Shows the status of all bots.",
                "show regions - Show regions known to bots.",
                "show status - Shows pCampbot status.",
                "shutdown - Shutdown bots And exit.",
                "sit - Sit all bots on the ground.",
                "stand - Stand all bots."
            }

        For Each line In help
            ProgressPrint(line)
        Next

    End Sub

    Private Sub RunBot(RegionUUID As String, N As Integer)

        Createbots(N)

        ProgressPrint($"Starting up {CStr(N)} bots in {Region_Name(RegionUUID)}")

        Dim arguments As String = $"-botcount {CStr(N)} -b p,t -loginuri http://127.0.0.1:{Settings.HttpPort} -start {Region_Name(RegionUUID)} -firstname Ima  -lastname Bot -password {Settings.Password & "xyz"}"
        ProgressPrint("pCambot.exe " & arguments)

        Dim BootProcess = New Process

        BootProcess.StartInfo.UseShellExecute = True
        BootProcess.StartInfo.WorkingDirectory = Settings.OpensimBinPath()
        BootProcess.StartInfo.FileName = """" & Settings.OpensimBinPath() & "pCampBot.exe" & """"
        BootProcess.StartInfo.CreateNoWindow = False

        Select Case Settings.ConsoleShow
            Case "True"
                BootProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Case "False"
                BootProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Case "None"
                BootProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
        End Select

        BootProcess.StartInfo.Arguments = arguments
        Dim ok As Boolean

        Try
            ok = BootProcess.Start
        Catch ex As Exception
            ErrorLog("Failed to boot pCampbot.exe")
            ProgressPrint("Failed to boot pCampbot.exe")
            Return
        End Try

        BotPID = BootProcess.Id
        ProgressPrint("Bot running")

    End Sub

    Private Sub StopBot()

        ProgressPrint("Shutting down all bots")
        If BotPID > 0 Then
            Try
                AppActivate(BotPID)
                SendKeys.Send("{ENTER}" & "disconnect" & "{ENTER}")  ' DO NOT make a interpolated string, will break!!
                SendKeys.Send("{ENTER}" & "quit" & "{ENTER}")  ' DO NOT make a interpolated string, will break!!
                SendKeys.Flush()
                ProgressPrint("Shutdown Sent")
            Catch
            End Try
        Else
            ProgressPrint("Not running")
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
                    Dim url = $"http://{Settings.PublicIP}:{Settings.DiagnosticPort}?alt={region}&agent=UUID&AgentID={AviUUID}&password={Settings.MachineId}"
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

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("Debug")

    End Sub

    Private Sub RadioTrue_CheckedChanged(sender As Object, e As EventArgs) Handles RadioTrue.CheckedChanged

        Value = RadioTrue.Checked

    End Sub

#End Region

End Class
