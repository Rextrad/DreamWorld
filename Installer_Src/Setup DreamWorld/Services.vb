Imports Renci.SshNet.Messages
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Module Services

    Public RegionStats As New Dictionary(Of String, Integer)

    Private _fore As Boolean

    ''' <summary>
    ''' Cached copy of ForeGND for speed
    ''' </summary>
    ''' <returns>true is in foreground</returns>
    Public Function Fore() As Boolean
        Return _fore
    End Function

    ''' <summary>
    ''' Returns if Service is running and we are foreground App
    ''' </summary>
    ''' <returns></returns>
    Public Function ForeGND() As Boolean

        Dim Param = Command()
        'Log("Service", $"Startup param = {Param}")
        'Log("Service", $"Environment path = {Environment.CommandLine}")
        'Log("Service", $"RunAsService = {RunAsService}")

        If ServiceExists("DreamGridService") And
                Settings.RunAsService And
                CBool(Param.ToLower <> "service") Then
            _fore = True
            Return True
        Else
            _fore = False
            Return False
        End If

    End Function

    Public Sub GetServiceList()

        If ForeGND() Then
            Try
                ' UUID,int|UUID,int|
                Dim regions = SignalService("RegionList").Split(New Char() {"|"c}) ' split at the |
                For Each Region As String In regions
                    Dim R = Region.Split(New Char() {","c}) ' split at the comma
                    If R.Length > 1 Then
                        Dim uuid = R(0).Trim
                        If uuid.Length > 0 Then
                            If RegionStats.ContainsKey(uuid) Then
                                RegionStats(uuid) = CInt("0" & R(1).Trim)
                            Else
                                RegionStats.Add(uuid, CInt("0" & R(1).Trim))
                            End If
                        End If
                    End If
                Next
            Catch ex As Exception
            End Try
        End If
    End Sub

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

        Dim N = New ClassNssm
        Return N.NssmCommand($"status {name}")

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

        If RunningInServiceMode() Or Not Fore() Then
            Return "OK"
        End If

        Using client As New TimedWebClient With {
                .Timeout = 10000
                } ' download client for web pages
            Try
                Dim Url = $"http://{Settings.LANIP}:{Settings.DiagnosticPort}?Command={Command}&Password={Settings.MachineId}"
                Dim result = client.DownloadString(Url)
                Application.DoEvents()
                Return result
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
                Return "0"
            End Try
        End Using
        Return "0"

    End Function

End Module
