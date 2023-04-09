Module Services

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

    Public Function isDreamGridServiceRunning() As Boolean

        If ServiceExists("DreamGridService") AndAlso
                CheckPortSocket(Settings.LANIP, Settings.DiagnosticPort) Then
            ServiceIcon(True)
            Return True
        End If
        ServiceIcon(False)
        Return False

    End Function

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

    Public Function ServiceExists(name As String) As Boolean

        Using ServiceProcess As New Process()
            ServiceProcess.StartInfo.RedirectStandardOutput = True
            ServiceProcess.StartInfo.RedirectStandardError = True
            ServiceProcess.StartInfo.RedirectStandardInput = True
            ServiceProcess.StartInfo.UseShellExecute = False
            ServiceProcess.StartInfo.FileName = "sc.exe"
            ServiceProcess.StartInfo.Arguments = "query " & name
            ServiceProcess.StartInfo.CreateNoWindow = True
            ServiceProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden

            Dim console As String = ""
            Try
                ServiceProcess.Start()
                console = ServiceProcess.StandardOutput.ReadToEnd()
                ServiceProcess.WaitForExit()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            If console.Contains("does not exist") Then Return False
            Return True

        End Using

    End Function

    Public Sub ServiceIcon(Running As Boolean)

        If Not Running Then
            FormSetup.ServiceToolStripMenuItemDG.Image = Global.Outworldz.My.Resources.nav_plain_red
        Else
            FormSetup.ServiceToolStripMenuItemDG.Image = Global.Outworldz.My.Resources.check2
        End If
        Application.DoEvents()

    End Sub

    ''' <summary>
    ''' Send a message to the service
    ''' </summary>
    ''' <param name="Command">A string command</param>
    Public Function SignalService(Command As String) As String

        If Not Foreground() And Not Settings.RunAsService Then Return "NO"

        Using client As New TimedWebClient With {
                .Timeout = 3000
                } ' download client for web pages
            Try
                Dim Url = $"http://{Settings.LANIP}:{Settings.DiagnosticPort}?Command={Command}&Password={Settings.MachineId}"
                Diagnostics.Debug.Print(Url)
                Dim result = client.DownloadString(Url)

                Return result
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
                Return "0"
            End Try
        End Using
        Return "0"

    End Function

End Module
