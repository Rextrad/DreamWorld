Public Class FormBots
    Private BotPID As Integer
    Private botStart As String = ""

    ' See http://opensimulator.org/wiki/PCampBot

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

    Private Sub BotCommand(msg As String)

        If BotPID > 0 Then
            Try
                AppActivate(BotPID)
                SendKeys.Send("{ENTER}" & msg & "{ENTER}")  ' DO NOT make a interpolated string, will break!!
                SendKeys.Flush()
            Catch
            End Try
        Else
            ProgressPrint("Not running")
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ConnectButton.Click

        BotCommand("connect")

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles StartButton.Click
        RunBots()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxPhysics.CheckedChanged
        BotCommand("add behaviour p")
        ProgressPrint("Bots constantly move and jump around")
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGrab.CheckedChanged
        BotCommand("add behaviour g")
        ProgressPrint("Bots randomly click prims whether set clickable or not")
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxTeleport.CheckedChanged
        BotCommand("add behaviour t")
        ProgressPrint("Bots regularly teleport between regions on the grid")
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxNone.CheckedChanged

        BotCommand("remove behaviour n")
        ProgressPrint("Bots do nothing")

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim text = ComboBox1.SelectedItem.ToString()

        Select Case text
            Case "Home"
                botStart = "home"
            Case "Last"
                botStart = "last"
            Case "(Region Name)"
                Dim RegionName = ChooseRegion(True) ' Just running regions
                If RegionName.Length = 0 Then Return
                botStart = RegionName
        End Select

    End Sub

    Private Sub Createbots(N As Integer)

        While N > 0
            Dim C = GetAviCountByName("Ima", $"Bot{N - 1}")
            If C = 0 Then
                ProgressPrint($"Creating Ima Bot{N - 1}")
                ConsoleCommand(RobustName, $"create user Ima Bot{N - 1} {Settings.Password & "xyz"} not@set.yet ~~")
            End If
            N -= 1
        End While

    End Sub

    Private Sub DisconnectButton_Click(sender As Object, e As EventArgs) Handles DisconnectButton.Click

        BotCommand("disconnect")

    End Sub

    Private Sub Form1_Close(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Closing

        StopBot()

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        SetScreen()
        ComboBox1.SelectedIndex = 2
        HelpOnce("Bots")
        PrintBotHelp()

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        HelpManual("Bots")

    End Sub

    Private Sub PrintBotHelp()

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
                "show status - Shows status.",
                "shutdown - Shutdown bots And exit.",
                "sit - Sit all bots on the ground.",
                "stand - Stand all bots."
            }

        For Each line In help
            ProgressPrint(line)
        Next

    End Sub

    Private Sub ProgressPrint(Value As String)
        Log(My.Resources.Info_word, Value)
        TextBox1.Text += Value & vbCrLf
        If TextBox1.Text.Length > TextBox1.MaxLength - 1000 Then
            TextBox1.Text = Mid(TextBox1.Text, 1000)
        End If
    End Sub

    Private Sub RegionButton_Click(sender As Object, e As EventArgs) Handles RegionButton.Click

        BotCommand("show regions")

    End Sub

    Private Sub RunBot(RegionName As String, N As Integer)

        Createbots(N)

        ProgressPrint($"Starting up {CStr(N)} bots in {RegionName}")

        Dim arguments As String = $"-botcount {CStr(N)} -c  -loginuri http://127.0.0.1:{Settings.HttpPort} {RegionName} -firstname Ima  -lastname Bot -password {Settings.Password & "xyz"}"

        'ProgressPrint("pCambot.exe " & arguments)
        'pCambot.exe -botcount 1 -loginuri http://127.0.0.1:6002 -start Normal -firstname Ima  -lastname Bot -password 123456xyz

        Dim BotProcess = New Process

        BotProcess.StartInfo.UseShellExecute = True
        BotProcess.StartInfo.WorkingDirectory = Settings.OpensimBinPath()
        BotProcess.StartInfo.FileName = """" & Settings.OpensimBinPath() & "pCampBot.exe" & """"
        BotProcess.StartInfo.CreateNoWindow = False

        Select Case Settings.ConsoleShow
            Case "True"
                BotProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Case "False"
                BotProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Case "None"
                BotProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
        End Select

        BotProcess.StartInfo.Arguments = arguments
        Dim ok As Boolean

        Try
            ok = BotProcess.Start
        Catch ex As Exception
            ErrorLog("Failed to boot pCampbot.exe")
            TextPrint("Failed to boot pCampbot.exe")
            Return
        End Try

        BotPID = WaitForPID(BotProcess)      ' check if it gave us a PID, if not, it failed.

        SetWindowTextCall(BotProcess, "pCampBot")
        ProgressPrint("Bot running")

    End Sub

    Private Sub RunBots()

        Dim out As Integer
        Dim how As String = "home"

        Dim N = InputBox("How many bots?  0-100 ", "pCampBots", "1")
        If Integer.TryParse(N, out) Then
            If out > 100 Then out = 0

            If botStart = "home" Then
                RunBot($"-s {Settings.WelcomeRegion}", out)
            ElseIf botStart = "last" Then
                RunBot("-s last", out)
            ElseIf botStart.Length > 0 Then
                RunBot($"-s {botStart}", out)
            Else
                Dim RegionName = ChooseRegion(True) ' Just running regions
                If RegionName.Length = 0 Then Return
                RunBot($"-s {RegionName}", out)
            End If

        End If

    End Sub

    Private Sub SitButton_Click(sender As Object, e As EventArgs) Handles SitButton.Click
        BotCommand("sit")
    End Sub

    Private Sub StandButton_Click(sender As Object, e As EventArgs) Handles StandButton.Click
        BotCommand("stand")
    End Sub

    Private Sub StatusButton_Click(sender As Object, e As EventArgs) Handles StatusButton.Click
        BotCommand("show status")
    End Sub

    Private Sub StopBot()

        ProgressPrint("Shutting down all bots")
        BotCommand("disconnect{ENTER}quit")
        ProgressPrint("Shutdown Sent")

    End Sub

    Private Sub StopButton_Click(sender As Object, e As EventArgs) Handles StopButton.Click
        StopBot()
    End Sub

    Private Sub TextBox1_Changed(sender As System.Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim ln As Integer = TextBox1.Text.Length
        TextBox1.SelectionStart = ln
        TextBox1.ScrollToCaret()
    End Sub

End Class