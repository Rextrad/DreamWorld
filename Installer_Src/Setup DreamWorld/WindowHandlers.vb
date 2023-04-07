#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Collections.Concurrent
Imports System.Threading

Module WindowHandlers

    Private ReadOnly _exitList As New ConcurrentDictionary(Of String, String)

    Public ReadOnly Property ExitList As ConcurrentDictionary(Of String, String)
        Get
            Return _exitList
        End Get
    End Property

#Region "Enum"

    Public Enum SHOWWINDOWENUM As Integer
        SWHIDE = 0
        SWNORMAL = 1
        SWSHOWMINIMIZED = 2
        SWMAXIMIZE = 3
        SWSHOWNOACTIVATE = 4
        SWSHOW = 5
        SWMINIMIZE = 6
        SWSHOWMINNOACTIVE = 7
        SWSHOWNA = 8
        SWRESTORE = 9
        SWSHOWDEFAULT = 10
        SWFORCEMINIMIZE = 11

    End Enum

#End Region

    Public Function ConsoleCommand(RegionUUID As String, command As String) As Boolean

        ''' <summary>Sends keystrokes to Opensim. Always sends an enter button before to clear and use keys</summary>
        ''' <param name="ProcessID">PID of the DOS box</param>
        ''' <param name="command">String</param>
        ''' <returns></returns>
        If command Is Nothing Then Return True

        If command.Length > 0 Then
            command = ToLowercaseKeys(command)
            If RegionUUID <> RobustName() Then

                Thaw(RegionUUID)
                If RunningInServiceMode() Then
                    Return RPC_Region_Command(RegionUUID, command)
                End If

                ShowDOSWindow(RegionUUID, MaybeShowWindow())
                DoType(RegionUUID, command)
                ShowDOSWindow(RegionUUID, MaybeHideWindow())
            Else ' Robust
                ShowDOSWindow(RobustName, MaybeShowWindow())
                DoType("Robust", command)
                ShowDOSWindow(RobustName, MaybeHideWindow())
            End If
        End If

        Return False

    End Function

    Public Sub DoType(RegionUUID As String, command As String)

        'plus sign(+), caret(^), percent sign (%), tilde (~), And parentheses ()
        command = command.Replace("+", "{+}")
        command = command.Replace("^", "{^}")
        command = command.Replace("%", "{%}")
        command = command.Replace("{", "{{}")
        command = command.Replace("}", "{}}")
        command = command.Replace("(", "{(}")
        command = command.Replace(")", "{)}")

        Dim PID As Integer
        If RegionUUID = "Robust" Then

            PID = GetPIDofRobust()
            If PID = 0 AndAlso command = "q" Then
                Zap("Robust")
                Return
            End If
            If PID = 0 Then
                Return
            End If
            Try
                AppActivate(PID)
                SendKeys.Send("{ENTER}" & command & "{ENTER}")  ' DO NOT make a interpolated string, will break!!
                SendKeys.Flush()
            Catch ex As ArithmeticException
                BreakPoint.Print(ex.Message)
            End Try
            Return
        End If

        ' Regions
        PID = GetPIDFromFile(Group_Name(RegionUUID))
        If PID = 0 Then
            ' try a direct way
            RPC_Region_Command(RegionUUID, command)
        Else
            Try
                AppActivate(PID)
                If (FocusWindow(PID)) Then
                    SendKeys.Send("{ENTER}" & command & "{ENTER}")  ' DO NOT make a interpolated string, will break due to {}
                    SendKeys.Flush()
                End If
            Catch ex As Exception
                RPC_Region_Command(RegionUUID, command)
            End Try
        End If

    End Sub

    Public Function FocusWindow(ByVal PID As Integer) As Boolean

        Dim p As System.Diagnostics.Process = Process.GetProcessById(PID)
        If p IsNot Nothing Then
            Return SetForegroundWindow(p.MainWindowHandle)
        End If
        Return False

    End Function

    ''' <summary>
    ''' Returns if Service is running and we are foreground App
    ''' </summary>
    ''' <returns></returns>
    Public Function Foreground() As Boolean

        Dim Param = Command()
        'Log("Service", $"Startup param = {Param}")
        'Log("Service", $"Environment path = {Environment.CommandLine}")
        'Log("Service", $"RunAsService = {RunAsService}")

        If ServiceExists("DreamGridService") And
                Settings.RunAsService And
                CBool(Param.ToLower <> "service") And
                CheckPortSocket(Settings.LANIP, Settings.DiagnosticPort) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetHwnd(Groupname As String) As IntPtr

        If Groupname <> RobustName() Then
            Try

                Dim PID = GetPIDFromFile(Groupname)
                If PID = 0 Then Return IntPtr.Zero

                Dim Pr = Process.GetProcessById(PID)
                If Pr IsNot Nothing Then
                    Return Pr.MainWindowHandle
                End If
            Catch ex As Exception
            End Try
        Else
            Try
                Dim Pr = Process.GetProcessById(PropRobustProcID)
                Return Pr.MainWindowHandle
            Catch
            End Try

        End If

        Return IntPtr.Zero

    End Function

    Public Sub GetOpensimPIDsFromFiles()

        Dim P = IO.Path.Combine(Settings.CurrentDirectory, "Outworldzfiles\Opensim\bin\Regions")
        Dim directory As New System.IO.DirectoryInfo(P)

        For Each Folder In directory.GetDirectories()
            Dim GroupName = Folder.Name
            Dim PID = GetPIDFromFile(GroupName)

            For Each RegionUUID In RegionUuidListFromGroup(GroupName)
                ProcessID(RegionUUID) = PID
            Next

        Next

    End Sub

    ''' <summary>
    ''' Returns a PID from PID.pid file in each region
    ''' </summary>
    ''' <param name="GroupName">GroupName</param>
    ''' <returns>PID</returns>
    Public Function GetPIDFromFile(GroupName As String) As Integer

        Dim PID As Integer ' return this 0 or a positive in
        Dim PIDFIle = IO.Path.Combine(Settings.CurrentDirectory, $"Outworldzfiles\Opensim\bin\Regions\{GroupName}\PID.pid")
        Try
            If System.IO.File.Exists(PIDFIle) Then
                Using Reader As New IO.StreamReader(PIDFIle, System.Text.Encoding.ASCII)
                    While Not Reader.EndOfStream
                        Dim line As String = Reader.ReadLine

                        If Int32.TryParse(line, PID) Then
                            Return PID
                        Else
                            Debug.Print("No PID on disk")
                        End If
                    End While
                End Using
            End If
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

        Return PID

    End Function

    ''' <summary>
    ''' Returns a handle to the window, by process list, or by reading the PID file.
    ''' </summary>
    ''' <param name="Groupname">Name of the DOS box</param>
    ''' <returns>Handle to a window to Intptr.zero</returns>
    Public Function GetPIDofRobust() As Integer

        For Each pList As Process In Process.GetProcessesByName("Robust")
            Try
                If pList.MainWindowTitle = RobustName() Then
                    Return pList.Id
                End If
            Catch
            End Try
        Next
        Return 0

    End Function

    Public Function MaybeHideWindow() As SHOWWINDOWENUM

        Dim w As SHOWWINDOWENUM
        Select Case Settings.ConsoleShow
            Case "True"
                w = SHOWWINDOWENUM.SWRESTORE
            Case "False"
                w = SHOWWINDOWENUM.SWMINIMIZE
            Case "None"
                w = SHOWWINDOWENUM.SWMINIMIZE
        End Select

        Return w

    End Function

    Public Function MaybeShowWindow() As SHOWWINDOWENUM

        Dim w As SHOWWINDOWENUM
        Select Case Settings.ConsoleShow
            Case "True"
                w = SHOWWINDOWENUM.SWRESTORE
            Case "False"
                w = SHOWWINDOWENUM.SWRESTORE
            Case "None"
                w = SHOWWINDOWENUM.SWMINIMIZE
        End Select

        Return w

    End Function

    Public Sub OpensimExited(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim S As System.Diagnostics.Process = CType(sender, Process)
        Dim RegionUUID = FindRegionUUIDByPID(S.Id)
        If RegionUUID.Length = 0 Then
            ErrorLog("Null Region UUID")
            Return
        End If

        Debug.Print($"{Region_Name(RegionUUID)} Exited")
        If RegionUUID.Length = 0 Then
            Return
        End If

        ExitList.TryAdd(Region_Name(RegionUUID), RegionUUID)

    End Sub

    ''' <summary>
    ''' Returns if we started with the Server param and service is installed, which means run as a Service.
    ''' </summary>
    ''' <returns></returns>
    Public Function RunningInServiceMode() As Boolean

        Dim Param = Command()
        'Log("Service", $"Startup param = {Param}")
        'Log("Service", $"Environment path = {Environment.CommandLine}")
        'Log("Service", $"RunAsService = {RunAsService}")

        If ServiceExists("DreamGridService") And
            CBool(Param.ToLower = "service") And
            Settings.RunAsService Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Sub SendMsg(msg As String)

        Dim l As New List(Of String)
        If PropOpensimIsRunning() Then
            For Each RegionUUID In RegionUuids()

                If Not l.Contains(Group_Name(RegionUUID)) Then
                    l.Add(Group_Name(RegionUUID))
                    If IsBooted(RegionUUID) Then
                        RPC_Region_Command(RegionUUID, "set log level " & msg)
                    End If
                End If

            Next
            ConsoleCommand(RobustName, "set log level " & msg)
        End If

    End Sub

    Public Sub SendScriptCmd(command As String)
        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If
        Dim rname = ChooseRegion(False)
        Dim RegionUUID As String = FindRegionByName(rname)
        If RegionUUID.Length > 0 Then
            ConsoleCommand(RegionUUID, "change region ""{Region_Name(RegionUUID)}""{vbCrLf}{command}")
        End If

    End Sub

    ''' <summary>
    ''' Sets the window title text
    ''' </summary>
    ''' <param name="myProcess">PID</param>
    ''' <param name="windowName">WindowName</param>
    ''' <returns>True if window is set</returns>
    Public Sub SetWindowTextCall(myProcess As Process, windowName As String)
        ''' <summary>
        ''' SetWindowTextCall is here to wrap the SetWindowtext API call. This call fails when there is no hwnd as Windows takes its sweet time to get that. Also, may fail to write the title. It has a
        ''' timer to make sure we do not get stuck
        ''' </summary>
        ''' <param name="hwnd">Handle to the window to change the text on</param>
        ''' <param name="windowName">the name of the Window</param>

        If myProcess Is Nothing Then
            Return
        End If

        If RunningInServiceMode() Then Return

        Dim WindowCounter As Integer = 0
        Dim myhandle As IntPtr
        Try
            myProcess.Refresh()
            myhandle = myProcess.MainWindowHandle
            While myhandle = IntPtr.Zero
                WindowCounter += 1
                If WindowCounter > 600 Then '  60 seconds for process to start
                    ErrorLog($"{My.Resources.CannotgetWindowHandle} : {windowName}")
                    Return
                End If
                Thread.Sleep(100)
                myProcess.Refresh()
                myhandle = myProcess.MainWindowHandle
            End While
        Catch ex As Exception
            BreakPoint.Print($"{windowName} {ex.Message}")
            Return
        End Try

        Dim status As Boolean
        WindowCounter = 0
        Dim isthere As Integer = 0
        While True
            Try

                myProcess.Refresh()
                Thread.Sleep(10)
                If myProcess.MainWindowTitle = windowName Then
                    isthere += 1
                    If isthere > 3 Then
                        Return
                    End If
                Else
                    isthere = 0
                    status = SetWindowText(myhandle, windowName)
                End If
            Catch ex As Exception ' can fail to be a valid window handle
                BreakPoint.Print(ex.Message)
                Return
            End Try

            WindowCounter += 1
            If WindowCounter > 6000 Then '  1 minute
                Return
            End If

        End While

        Return

    End Sub

    Public Function ShowDOSWindow(RegionUUID As String, command As SHOWWINDOWENUM) As Boolean

        If RunningInServiceMode() Then Return True

        If Settings.ConsoleShow = "None" AndAlso command <> SHOWWINDOWENUM.SWMINIMIZE Then
            Return True
        End If

        Dim handle As IntPtr
        If RegionUUID = RobustName() Then
            handle = GetHwnd(RegionUUID)
        Else
            handle = GetHwnd(Group_Name(RegionUUID))
        End If

        Dim ctr = 20    ' 2 seconds
        If handle <> IntPtr.Zero Then
            Dim HandleValid As Boolean = False

            While Not HandleValid AndAlso ctr > 0
                Try
                    If SetForegroundWindow(handle) Then
                        HandleValid = ShowWindow(handle, command)
                        If HandleValid Then Return True
                    End If
                Catch ex As Exception
                    BreakPoint.Print(ex.Message)
                End Try
                ctr -= 1
                Sleep(100)
                Application.DoEvents()
            End While
        End If

        Return False

    End Function

    ''' <summary>
    ''' Send a message to the service
    ''' </summary>
    ''' <param name="Command">A string command</param>
    Public Function SignalService(Command As String) As Boolean

        If Not Foreground() And Not Settings.RunAsService Then Return False

        Using client As New TimedWebClient With {
                .Timeout = 3000
                } ' download client for web pages
            Try
                Dim Url = $"http://{Settings.LANIP}:{Settings.DiagnosticPort}?Command={Command}&password={Settings.MachineId}"
                Diagnostics.Debug.Print(Url)
                Dim result = client.DownloadString(Url)

                If result = "ACK" Then Return True
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
                Return False
            End Try
        End Using
        Return False

    End Function

    Public Function WaitForPID(myProcess As Process) As Integer

        If myProcess Is Nothing Then
            Return 0
        End If

        Dim TooMany As Integer = 0

        ' 2 minutes for old hardware and it to build DB
        Do While TooMany < 600
            Try
                If myProcess.Id > 0 Then
                    Return myProcess.Id
                End If
            Catch ex As Exception
            End Try

            Sleep(100)
            TooMany += 1
        Loop

        Return 0

    End Function

    Public Sub Zap(processName As String)

        ''' <summary>Kill processes by name</summary>
        ''' <param name="processName"></param>
        ''' <returns></returns>

        If SignalService($"Stop{processName}") Then Return

        PropAborting = True

        ' Kill process by name
        For Each P As Process In System.Diagnostics.Process.GetProcessesByName(processName)
            Log(My.Resources.Info_word, $"{My.Resources.Stoppingprocess} {processName}")
            Try
                P.Kill()
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try
        Next

    End Sub

    Private Function ToLowercaseKeys(Str As String) As String

        If My.Computer.Keyboard.CapsLock Then
            For Pos = 1 To Len(Str)
                Dim C As String = Mid(Str, Pos, 1)
                Mid(Str, Pos) = CStr(IIf(UCase(C) = C, LCase(C), UCase(C)))
            Next
        End If
        Return Str

    End Function

End Module
