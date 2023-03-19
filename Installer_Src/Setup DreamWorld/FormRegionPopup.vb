#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Public Class FormRegionPopup

#Region "Private Fields"

    Private _RegionName As String = ""
    Dim gPick As String = ""

#End Region

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
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(MyBase.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Public Methods"

    Public Function Choice() As String
        Return gPick
    End Function

    Public Sub Init(RegionName As String)

        If Settings.ServerType = RobustServerName Then
            ViewMapButton.Enabled = True
        Else
            ViewMapButton.Enabled = False
        End If

        _RegionName = RegionName

        Dim RegionUUID As String = FindRegionByName(RegionName)
        Dim status = RegionStatus(RegionUUID)

        Me.Text = $"{RegionName} {GetStateString(status)}"
        GroupBox1.Text = Group_Name(RegionUUID)

        If Not RegionEnabled(RegionUUID) Then
            ShowConsoleButton.Enabled = False
            StatsButton1.Enabled = False
            StartButton.Enabled = False
            StopButton.Enabled = True
            SaveOAR.Enabled = False
            Teleport.Enabled = False
            LoadOAR.Enabled = False
            MsgButton.Enabled = False
            EditButton1.Enabled = True
        Else

            If RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended Then
                ShowConsoleButton.Enabled = True
                StatsButton1.Enabled = False
                StartButton.Enabled = True
                StopButton.Enabled = True
                SaveOAR.Enabled = True
                LoadOAR.Enabled = True
                Teleport.Enabled = True
                EditButton1.Enabled = True
                MsgButton.Enabled = False

            ElseIf RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
                ShowConsoleButton.Enabled = True
                StatsButton1.Enabled = True
                StartButton.Enabled = False
                StopButton.Enabled = True
                SaveOAR.Enabled = True
                LoadOAR.Enabled = True
                Teleport.Enabled = True
                EditButton1.Enabled = True
                MsgButton.Enabled = True
            ElseIf RegionStatus(RegionUUID) = SIMSTATUSENUM.RecyclingDown Then
                ShowConsoleButton.Enabled = True
                StatsButton1.Enabled = False
                StartButton.Enabled = False
                StopButton.Enabled = True
                LoadOAR.Enabled = False
                SaveOAR.Enabled = False
                Teleport.Enabled = False
                MsgButton.Enabled = False
                EditButton1.Enabled = True
            ElseIf RegionStatus(RegionUUID) = SIMSTATUSENUM.Booting Or
             RegionStatus(RegionUUID) = SIMSTATUSENUM.RecyclingUp Then
                ShowConsoleButton.Enabled = True
                StatsButton1.Enabled = False
                StartButton.Enabled = False
                StopButton.Enabled = True
                SaveOAR.Enabled = False
                LoadOAR.Enabled = False
                Teleport.Enabled = False
                MsgButton.Enabled = False
                EditButton1.Enabled = False
            ElseIf RegionStatus(RegionUUID) = SIMSTATUSENUM.Stopped Then
                ShowConsoleButton.Enabled = False
                StatsButton1.Enabled = False
                StartButton.Enabled = True
                StopButton.Enabled = True
                SaveOAR.Enabled = True
                LoadOAR.Enabled = True
                Teleport.Enabled = True
                EditButton1.Enabled = True
                MsgButton.Enabled = False
            End If
        End If

        BringToFront()

    End Sub

    Private Sub Form_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Diagnostics.Debug.Print("closing")

    End Sub

#End Region

#Region "Private Methods"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Teleport.Click
        gPick = "Teleport"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles ViewLog.Click

        Dim UUID = FindRegionByName(_RegionName)
        Dim GroupName As String = Group_Name(UUID)
        Baretail("""" & IO.Path.Combine(Settings.CurrentDirectory, $"OutworldzFiles\Opensim\bin\Regions\{GroupName}\OpenSim.log") & """")

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles StatsButton1.Click

        Dim RegionNum = FindRegionByName(_RegionName)
        Dim RegionPort = GroupPort(RegionNum)
        Dim webAddress As String = $"http://{Settings.PublicIP}:{CType(RegionPort, String)}/SStats/"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles ShowConsoleButton.Click

        If Not ServiceExists("DreamGridService") Then
            gPick = "Console"
            DialogResult = DialogResult.OK
        Else

            ' enable console for Service mode
            Dim args As String = ""

            Dim INI = New LoadIni(Settings.OpensimBinPath & "Opensim.ConsoleClient.ini", ";", System.Text.Encoding.UTF8)
            Dim RegionUUID = FindRegionByName(_RegionName)

            ResumeRegion(RegionUUID)

            INI.SetIni("Startup", "pass", CStr(Settings.Password))
            INI.SetIni("Startup", "user", $"{Settings.AdminFirst} {Settings.AdminLast}")
            INI.SetIni("Startup", "port", CStr(GroupPort(RegionUUID)))
            INI.SetIni("Startup", "host", CStr(Settings.PublicIP))
            INI.SaveIni()

            Dim ConsoleProcess As New Process
            ConsoleProcess.StartInfo.UseShellExecute = False
            ConsoleProcess.StartInfo.FileName = Settings.OpensimBinPath & "Opensim.ConsoleClient.exe"
            ConsoleProcess.StartInfo.CreateNoWindow = False
            ConsoleProcess.StartInfo.WorkingDirectory = Settings.OpensimBinPath
            ConsoleProcess.StartInfo.RedirectStandardOutput = False
            ConsoleProcess.StartInfo.Arguments &= args

            Try
                ConsoleProcess.Start()
                Timer(RegionUUID) = DateAdd("n", 5, Date.Now) ' Add  5 minutes for console to do things
                'TODO lkeep jabbing until consle closes
            Catch ex As Exception
                BreakPoint.Dump(ex)
                TextPrint($"Console {Global.Outworldz.My.Resources.did_not_start_word} {ex.Message}")
            End Try

        End If
    End Sub

    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Restart.Click
        gPick = "Restart"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub EditButton1_Click(sender As Object, e As EventArgs) Handles EditButton1.Click
        gPick = "Edit"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Load_Click(sender As Object, e As EventArgs) Handles LoadOAR.Click
        gPick = "Load"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub MsgButton_Click(sender As Object, e As EventArgs) Handles MsgButton.Click

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If

        Dim Message = InputBox(My.Resources.What_to_say_2_region)
        Dim RegionUUID As String = FindRegionByName(_RegionName)
        If RegionUUID.Length > 0 Then
            SendMessage(RegionUUID, Message)
        End If

    End Sub

    Private Sub Popup_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ViewLog.Image = Global.Outworldz.My.Resources.document_view
        ViewLog.Text = Global.Outworldz.My.Resources.View_Log_word
        EditButton1.Image = Global.Outworldz.My.Resources.document_dirty
        EditButton1.Text = Global.Outworldz.My.Resources.Edit_word
        GroupBox1.Text = Global.Outworldz.My.Resources.Region_Controls
        Restart.Text = Global.Outworldz.My.Resources.Restart_word
        ShowConsoleButton.Text = Global.Outworldz.My.Resources.View_Console_word
        StartButton.Text = Global.Outworldz.My.Resources.Start_word
        Teleport.Text = Global.Outworldz.My.Resources.Teleport_word
        StatsButton1.Text = Global.Outworldz.My.Resources.View_Statistics_Word
        StopButton.Text = Global.Outworldz.My.Resources.Stop_word
        ViewMapButton.Text = Global.Outworldz.My.Resources.View_Map_word
        LoadOAR.Text = Global.Outworldz.My.Resources.Load_Region_OAR
        SaveOAR.Text = Global.Outworldz.My.Resources.Save_Region_OAR_word
        MsgButton.Text = Global.Outworldz.My.Resources.Send_Alert_Message_word
        SetScreen()

    End Sub

    Private Sub RecycleButton2_Click(sender As Object, e As EventArgs) Handles SaveOAR.Click
        gPick = "Save"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub StartButton3_Click(sender As Object, e As EventArgs) Handles StartButton.Click
        gPick = "Start"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub StopButton1_Click(sender As Object, e As EventArgs) Handles StopButton.Click
        gPick = "Stop"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub ViewMapButton_Click(sender As Object, e As EventArgs) Handles ViewMapButton.Click

        VarChooser(_RegionName, False, False)

    End Sub

#End Region

End Class
