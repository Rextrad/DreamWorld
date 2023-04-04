Imports System.Security.Cryptography
Imports System.Text

Public Class CampBots
    Private ReadOnly behaviour As New List(Of String)
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

    Shared Function GetHash(theInput As String) As String

        Using hasher As MD5 = MD5.Create()    ' create hash object

            ' Convert to byte array and get hash
            Dim dbytes As Byte() =
                 hasher.ComputeHash(Encoding.UTF8.GetBytes(theInput))

            ' sb to create string from bytes
            Dim sBuilder As New StringBuilder()

            ' convert byte data to hex string
            For n As Integer = 0 To dbytes.Length - 1
#Disable Warning CA1305 ' Specify IFormatProvider
                sBuilder.Append(dbytes(n).ToString("X2"))
#Enable Warning CA1305 ' Specify IFormatProvider
            Next n

            Return sBuilder.ToString()
        End Using

    End Function

    Public Sub AddValue(key As String)

        If behaviour.Contains(key) Then
            Return
        End If
        behaviour.Add(key)

    End Sub

    Private Sub BotCommand(msg As String)

        If BotPID > 0 Then
            Try
                AppActivate(BotPID)
                SendKeys.Send("{ENTER}" & msg & "{ENTER}")  ' DO NOT make a interpolated string, will break!!
                SendKeys.Flush()
            Catch
            End Try
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ConnectButton.Click

        BotCommand("connect")

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles StartButton.Click
        RunBots()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxPhysics.CheckedChanged

        If CheckBoxPhysics.Checked Then
            AddValue("p")
            BotCommand("add behaviour p")
            CheckBoxNone.Checked = False
            ProgressPrint("Bots constantly move and jump around")
        Else
            ProgressPrint("Bots stop moving")
            DeleteValue("p")
            BotCommand("remove behaviour p")
        End If

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGrab.CheckedChanged

        If CheckBoxGrab.Checked Then
            BotCommand("add behaviour g")
            AddValue("g")
            CheckBoxNone.Checked = False
            ProgressPrint("Bots randomly click prims whether set clickable or not")
        Else
            BotCommand("remove behaviour g")
            ProgressPrint("Bots Stop grabbing prims")
            DeleteValue("g")
        End If

    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxTeleport.CheckedChanged

        If CheckBoxTeleport.Checked Then
            BotCommand("add behaviour t")
            AddValue("t")
            CheckBoxNone.Checked = False
            ProgressPrint("Bots regularly teleport between regions on the grid")
        Else
            BotCommand("remove behaviour t")
            ProgressPrint("Bots Stop Telporting")
            DeleteValue("t")
        End If

    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxNone.CheckedChanged

        If CheckBoxNone.Checked Then

            CheckBoxTeleport.Checked = False
            CheckBoxGrab.Checked = False
            CheckBoxPhysics.Checked = False

            BotCommand("remove behaviour n")
            behaviour.Clear()
            ProgressPrint("Bots do nothing")

        End If

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
            Dim C = GetAviCountByName("Iama", $"Bot{N - 1}")
            If C = 0 Then
                ProgressPrint($"Creating Iama Bot{N - 1}")
                ConsoleCommand(RobustName, $"create user Iama Bot{N - 1} {GetHash(Settings.Password) } not@set.yet ~~")
            End If
            N -= 1
        End While

    End Sub

    Private Sub DeleteValue(key As String)

        If behaviour.Contains(key) Then
            behaviour.Remove(key)
        End If

    End Sub

    Private Sub DisconnectButton_Click(sender As Object, e As EventArgs) Handles DisconnectButton.Click

        BotCommand("disconnect")

    End Sub

    Private Sub Form1_Close(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Closing

        StopBot()

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        My.Application.ChangeUICulture(Settings.Language)
        My.Application.ChangeCulture(Settings.Language)

        SetScreen()
        ComboBox1.SelectedIndex = 2

        StartButton.Text = My.Resources.Start_word
        StopButton.Text = My.Resources.Stop_word
        StatusButton.Text = My.Resources.Status_word
        RegionButton.Text = My.Resources.Regions_word
        ConnectButton.Text = My.Resources.Connectbots
        DisconnectButton.Text = My.Resources.Disconnectbots

        SitButton.Text = My.Resources.sit_word
        StandButton.Text = My.Resources.Stand_word
        CheckBoxPhysics.Text = My.Resources.Physics_word
        CheckBoxGrab.Text = My.Resources.Grab_word
        CheckBoxTeleport.Text = My.Resources.Teleport_word
        CheckBoxNone.Text = My.Resources.None
        SendAgentUpdatesCheckBox.Text = My.Resources.SendAgentUpdate
        RequestObjectTexturesCheckBox.Text = My.Resources.RequestObjectTextures
        Me.Text = My.Resources.Campbot

        HelpOnce("DreamBots")
        PrintBotHelp()

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        HelpManual("DreamBots")

    End Sub

    Private Sub PrintBotHelp()

        Dim help As New List(Of String) From {
                $"{My.Resources.Formoreinformation} 'help all' {My.Resources.Toget} 'help <item>' {My.Resources.whereitem}:",
                $"add behaviour <abbreviated-name> [<bot-number>] - {My.Resources.AddBehaviour}.",
                $"connect [<n>] - {My.Resources.Connectbots}.",
                $"disconnect [<n>] - {My.Resources.Disconnectbots}.",
                $"quit - {My.Resources.Shutdownbots}.",
                $"remove behaviour <abbreviated-name> [<bot-number>] - {My.Resources.Removebehaviour}.",
                $"set bots <key> <value> - {My.Resources.Setasetting}.",
                $"show bot <bot-number> - {My.Resources.Showsdetailedstatus}.",
                $"show bots - {My.Resources.Showthesstatus}.",
                $"show regions - {My.Resources.Showregionsknown}.",
                $"show status - {My.Resources.Showsstatus}.",
                $"shutdown - {My.Resources.Shutdownbotsandexit}.",
                $"sit - {My.Resources.SitAllBots}",
                $"stand - {My.Resources.Standallbots}."
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

    Private Sub RequestObjectTexturesCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles RequestObjectTexturesCheckBox.CheckedChanged

        Dim ini = IO.Path.Combine(Settings.OpensimBinPath, "pCampBot.ini")
        Dim pCampBot = New LoadIni(ini, ";", System.Text.Encoding.UTF8)
        pCampBot.SetIni("Bot", "RequestObjectTextures", CStr(RequestObjectTexturesCheckBox.Checked))
        pCampBot.SaveIni()

    End Sub

    Private Sub RunBot(RegionName As String, N As Integer)

        Createbots(N)

        ProgressPrint($"Starting up {CStr(N)} bots in {RegionName}")

        Dim behaviorlist As String
        Dim a = behaviour.ToArray
        If behaviour.Count > 0 Then
            behaviorlist = "-b " & behaviorlist.Join(",", a)
        Else
            behaviorlist = "-b none"
        End If

        Dim arguments As String = $"-botcount {CStr(N)} -c {behaviorlist} -loginuri http://127.0.0.1:{Settings.HttpPort}  -s ""{RegionName}"" -firstname Iama  -lastname Bot -password {GetHash(Settings.Password)}"

        'ProgressPrint("pCambot.exe " & arguments)
        'pCambot.exe -botcount 1 -loginuri http://127.0.0.1:6002 -start Normal -firstname Iama  -lastname Bot -password 123456xyz

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

        Dim N = InputBox("How many bots?  0-100 ", "pCampBots", "1")
        If Integer.TryParse(N, out) Then
            If out > 100 Then out = 0

            If botStart = "home" Then
                RunBot($"{Settings.WelcomeRegion}", out)
            ElseIf botStart = "last" Then
                RunBot("last", out)
            ElseIf botStart.Length > 0 Then
                RunBot($"{botStart}", out)
            Else
                Dim RegionName = ChooseRegion(True) ' Just running regions
                If RegionName.Length = 0 Then Return
                RunBot($"{RegionName}", out)
            End If

        End If

    End Sub

    Private Sub SendAgentUpdatesCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SendAgentUpdatesCheckBox.CheckedChanged

        Dim ini = IO.Path.Combine(Settings.OpensimBinPath, "pCampBot.ini")
        Dim pCampBot = New LoadIni(ini, ";", System.Text.Encoding.UTF8)
        pCampBot.SetIni("Bot", "SendAgentUpdates", CStr(SendAgentUpdatesCheckBox.Checked))
        pCampBot.SaveIni()

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
