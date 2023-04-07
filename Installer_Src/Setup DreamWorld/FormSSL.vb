Imports System.Text.RegularExpressions

Public Class FormSSL

    Private changed As Boolean
    Private initted As Boolean
    Private LogFile As String = ""

#Region "ScreenSize"

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

        ScreenPosition.SaveXY(Me.Left, Me.Top)
        ScreenPosition.SaveHW(Me.Height, Me.Width)
    End Sub

    Private Sub SetScreen()

        Try
            ScreenPosition = New ClassScreenpos(Me.Name)
            AddHandler ResizeEnd, Handler
            Dim xy As List(Of Integer) = ScreenPosition.GetXY()
            Me.Left = xy.Item(0)
            Me.Top = xy.Item(1)
            Dim hw As List(Of Integer) = ScreenPosition.GetHW()

            If hw.Item(0) = 0 Then
                Me.Height = 500
            Else
                Me.Height = hw.Item(0)
            End If
            If hw.Item(1) = 0 Then
                Me.Width = 600
            Else
                Me.Width = hw.Item(1)
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Me.Height = 500
            Me.Width = 600
            Me.Left = 100
            Me.Top = 100
        End Try
    End Sub

#End Region

    Private Shared Function MakeSSLbatch(stuff As String) As String

        Dim filename = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\SSL\InstallSSL.bat")
        Try
            Using file As New System.IO.StreamWriter(filename, False)
                file.WriteLine("@REM program to renew SSL certificate")
                file.WriteLine("@echo off")
                file.WriteLine($"cd {IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\SSL")}")
                file.WriteLine(stuff)
                file.WriteLine("Exit /B %ERRORLEVEL%")
            End Using
        Catch
        End Try

        Return filename

    End Function

    Private Shared Sub WriteLog(filename As String, contents As String)
        Try
            Using file As New System.IO.StreamWriter(filename, False)
                file.WriteLine(contents)
            End Using
        Catch
        End Try
    End Sub

    'https://www.win-acme.com/manual/advanced-use/examples/apache
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles CreateButton.Click

        SetLoading(True)

        If Settings.SSLIsInstalled = True Then
            Dim result = MsgBox(My.Resources.AreYouSureSSL, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, "SSL")
            If result <> vbYes Then
                Return
            End If
        End If

        If Settings.DnsName.Length = 0 Then
            MsgBox(My.Resources.MustUseDNS, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, "SSL Setup")
            Return
        End If

        If Not IsApacheRunning() Then
            MsgBox(My.Resources.ApacheMustBeRunning, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, "SSL Setup")
            Return
        End If

        If Settings.ApachePort <> 80 Then
            MsgBox(My.Resources.ApacheMustBeRunning, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, "SSL Setup")
            Return
        End If

        If changed Then Settings.SaveSettings()
        InstallSSL()
        SetLoading(False)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles StopButton.Click

        SetLoading(True)
        StartButton.Enabled = False
        StopButton.Enabled = False
        RestartButton.Enabled = False
        PictureBox2.Image = My.Resources.gear_time
        StopApache()
        If IsApacheRunning() Then
            PictureBox2.Image = My.Resources.gear_run
            StartButton.Enabled = False
            StopButton.Enabled = True
            RestartButton.Enabled = True
        Else
            PictureBox2.Image = My.Resources.gear_stop
            StartButton.Enabled = True
        End If
        SetLoading(False)

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles ViewLogButton.Click

        If IO.File.Exists(LogFile) Then
            Baretail(LogFile)
        End If

    End Sub

    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Button2.Click

        Dim pattern = New Regex("\.?(.*)$", RegexOptions.IgnoreCase)
        Dim match As Match = pattern.Match(Settings.PublicIP)
        Dim Str As String = ""
        If match.Success Then
            Str = $"?q={match.Groups(1).Value}"
        End If

        Dim webAddress As String = $"https://crt.sh/{Str}"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles RestartButton.Click

        SetLoading(True)
        StopButton.Enabled = False
        StartButton.Enabled = False
        RestartButton.Enabled = False
        PictureBox2.Image = My.Resources.gear_time
        StopApache()
        PictureBox2.Image = My.Resources.gear_stop
        StartApache()
        If IsApacheRunning() Then
            PictureBox2.Image = My.Resources.gear_run
        Else
            PictureBox2.Image = My.Resources.gear_stop
        End If
        StartButton.Enabled = False
        StopButton.Enabled = True
        RestartButton.Enabled = True
        SetLoading(False)

    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Revokebutton.Click

        If changed Then
            Dim result = MsgBox("Revoke the certificate?", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, My.Resources.Save_changes_word)
            If result <> vbYes Then
                Return
            End If
        End If

        Settings.SSLIsInstalled = False

        ' TODO actually revoke the certificate

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles StartButton.Click

        SetLoading(True)
        StartButton.Enabled = False
        RestartButton.Enabled = False
        StopButton.Enabled = False
        PictureBox2.Image = My.Resources.gear_time
        StartApache()
        If IsApacheRunning() Then
            PictureBox2.Image = My.Resources.gear_run
            StopButton.Enabled = True
            RestartButton.Enabled = True
        Else
            PictureBox2.Image = My.Resources.gear_stop
            StartButton.Enabled = True
        End If
        SetLoading(False)

    End Sub

    Private Sub EnableSSLCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles EnableSSLCheckbox.CheckedChanged

        If Not initted Then Return
        Settings.SSLEnabled = EnableSSLCheckbox.Checked
        changed = True

    End Sub

    Private Sub Form_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        If changed Then
            Dim result = MsgBox(My.Resources.Changes_Made, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, My.Resources.Save_changes_word)
            If result <> vbYes Then
                Return
            End If
        End If

        Settings.SaveSettings()

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        HelpManual("SSL")

    End Sub

    Private Sub InstallSSL()

        PictureBox1.Image = My.Resources.lock_time
        Using SSLProcess As New Process

            Dim shortname = Settings.DnsName

            Dim pattern = New Regex("\.?(.*)$", RegexOptions.IgnoreCase)
            Dim match As Match = pattern.Match(shortname)
            Dim Str As String = ""
            If match.Success Then
                shortname = match.Groups(1).Value
            End If

            SSLProcess.StartInfo.CreateNoWindow = True
            SSLProcess.StartInfo.UseShellExecute = False
            SSLProcess.StartInfo.RedirectStandardOutput = True
            SSLProcess.StartInfo.Arguments = $"--accepttos --source manual --host {shortname} --validation filesystem --webroot ""{IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\htdocs")}"" --store pemfiles --pemfilespath ""{IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\Certs")}""  "
            If Settings.SSLEmail.Length > 0 Then
                SSLProcess.StartInfo.Arguments &= $" --emailaddress {Settings.SSLEmail} "
            End If
            If Debugger.IsAttached Then
                '  SSLProcess.StartInfo.Arguments &= " --closeonfinish --test " ' so we do not hit a rate limit
            End If
            SSLProcess.StartInfo.FileName = MakeSSLbatch($".\wacs.exe {SSLProcess.StartInfo.Arguments}")
            If Debugger.IsAttached Then

                Using P As New Process
                    SSLProcess.StartInfo.WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "SSL")
                    Dim Address As String = "explorer.exe"
                    Try
                        '            Process.Start(Address, $"/open, {SSLProcess.StartInfo.FileName}")
                    Catch ex As Exception
                        BreakPoint.Print(ex.Message)
                    End Try
                End Using
                '   Return
            End If

            SSLProcess.StartInfo.WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\SSL")
            Dim log As String = ""
            Try
                SSLProcess.Start()
                log = SSLProcess.StandardOutput.ReadToEnd()
                SSLProcess.WaitForExit()
                WriteLog(LogFile, log)
            Catch ex As Exception
                ErrorLog(ex.Message)
                Logger("Error", ex.Message, "SSL")
                Settings.SSLIsInstalled = False
                PictureBox1.Image = My.Resources.lock_error
                Return
            End Try

            Dim Status = SSLProcess.ExitCode
            If Status = 0 Then
                Logger("OK", log, "SSL")
                Logger("Info", "Certificate installed", "SSL")
                ' It was not installed, so we need to restart Apache
                Settings.SSLIsInstalled = True
                PictureBox1.Image = My.Resources.lock_time
                StopApache()
                StartApache()
            Else
                If Status = -1 Then Logger("Error", "Failed to make the Certificate", "SSL")
                If Status = 1 Then Logger("Error", "Non-recognized command", "SSL")
                If Status = 2 Then Logger("Error", "The system cannot find the file specified. ", "SSL")
                If Status = 3 Then Logger("Error", "Non-recognized command", "SSL")
                If Status = 4 Then Logger("Error", "The system cannot find the path specified", "SSL")
                If Status = 5 Then Logger("Error", "Access denied", "SSL")
                Logger("Error", log, "SSL")

                If IO.File.Exists(LogFile) Then
                    Baretail(LogFile)
                End If
                PictureBox1.Image = My.Resources.lock_error
            End If

            If IO.File.Exists(LogFile) Then
                ViewLogButton.Enabled = True
            End If

        End Using
        Return
    End Sub

    Private Sub SetLoading(displayLoader As Boolean)

        If displayLoader Then
            PictureBox3.Visible = True
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Else
            PictureBox3.Visible = False
            Me.Cursor = System.Windows.Forms.Cursors.[Default]
        End If

    End Sub

    Private Sub SSL_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SetScreen()
        SetLoading(False)

        My.Application.ChangeUICulture(Settings.Language)
        My.Application.ChangeCulture(Settings.Language)

        'If Debugger.IsAttached Then Settings.SSLIsInstalled = False

        LogFile = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Logs\SSL.log")

        If Not Settings.ApacheEnable Then
            Settings.SSLEnabled = False
        End If

        EnableSSLCheckbox.Checked = Settings.SSLEnabled
        If Settings.SSLIsInstalled Then
            Revokebutton.Enabled = True
            PictureBox1.Image = My.Resources.lock_ok
        Else
            Revokebutton.Enabled = False
            PictureBox1.Image = My.Resources.lock_open
        End If

        If IsApacheRunning() Then
            StopButton.Enabled = True
            RestartButton.Enabled = True
            StartButton.Enabled = False
        Else
            StopButton.Enabled = False
            RestartButton.Enabled = False
            StartButton.Enabled = True
        End If

        If IO.File.Exists(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Logs\SSL.log")) Then
            ViewLogButton.Enabled = True
        End If

        EmailBox.Text = Settings.SSLEmail

        If IsApacheRunning() Then
            PictureBox2.Image = My.Resources.gear_run
        Else
            PictureBox2.Image = My.Resources.gear_stop
        End If

        HelpOnce("SSL")
        initted = True

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles EmailBox.TextChanged

        If Not initted Then Return
        Settings.SSLEmail = EmailBox.Text
        changed = True

    End Sub

End Class
