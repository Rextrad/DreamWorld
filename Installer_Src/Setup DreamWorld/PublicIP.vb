#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Net
Imports System.Net.Sockets

Module PublicIP

    Public Sub CheckDefaultPorts()

        Dim Check = New Dictionary(Of Integer, String)

        Try
            If Settings.ApacheEnable Then Check.Add(Settings.ApachePort, "Apache Port")

            If Settings.DTLEnable Then
                If Check.ContainsKey(Settings.DTLMoneyPort) Then
                    Bitch($"DTL Money Port: {Settings.DTLMoneyPort}")
                Else
                    Check.Add(Settings.DTLMoneyPort, "DTL Money Port")
                End If
            End If

            If Settings.SCEnable Then
                If Check.ContainsKey(Settings.SCPortBase) Then
                    Bitch($"Icecast Port 1: {Settings.SCPortBase}")
                Else
                    Check.Add(Settings.SCPortBase, "Icecast Port 1")
                End If

                If Check.ContainsKey(Settings.SCPortBase1) Then
                    Bitch($"Icecast Port 2 : {Settings.SCPortBase1}")
                Else
                    Check.Add(Settings.SCPortBase1, "Icecast Port 2")
                End If
            End If

            If Check.ContainsKey(Settings.DiagnosticPort) Then
                Bitch($"Diagnostic Port : {Settings.DiagnosticPort}")
            Else
                Check.Add(Settings.DiagnosticPort, "Diagnostic Port")
            End If

            If Check.ContainsKey(Settings.PrivatePort) Then
                Bitch($"Private Port : {Settings.PrivatePort}")
            Else
                Check.Add(Settings.PrivatePort, "Private Port")
            End If

            For Each Port In RegionUuids()
                If Check.ContainsKey(Region_Port(Port)) Then
                    Bitch($"Region Port {Region_Name(Port)}: {Region_Port(Port)}")
                Else
                    Check.Add(Region_Port(Port), "Region Port")
                End If
            Next
        Catch ex As Exception
            If Not RunningInServiceMode() Then
                MsgBox($"{My.Resources.Port_Error}", MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
            End If

        End Try

    End Sub

    Public Function Checkport(RegionUUID As String) As Boolean

        Dim PID = GetPIDFromFile(Group_Name(RegionUUID))
        If PID = 0 Then Return False
        Try
            Dim Pr = Process.GetProcessById(PID)
            If Pr.Id = 0 Then Return False
        Catch
            Return False
        End Try

        Return True

    End Function

    ''' <summary>
    ''' Checks port to see if region is up
    ''' </summary>
    ''' <param name="ServerAddress">IP</param>
    ''' <param name="Port">Port</param>
    ''' <returns>True is in memory</returns>
    Public Function CheckPortSocket(ServerAddress As String, Port As Integer) As Boolean

        Dim success As Boolean
        Dim result As IAsyncResult = Nothing
        Using ClientSocket As New TcpClient
            Try
                result = ClientSocket.BeginConnect(ServerAddress, Port, Nothing, Nothing)
                success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2))
                ClientSocket.EndConnect(result)
            Catch ex As Exception
                Return False
            End Try
            Return success
        End Using
        Return False

    End Function

    Public Function GetHostAddresses(hostName As String) As String

        Try
            Dim IPList As IPHostEntry = System.Net.Dns.GetHostEntry(hostName)

            For Each IPaddress In IPList.AddressList
                If (IPaddress.AddressFamily = Sockets.AddressFamily.InterNetwork) Then
                    Dim ip = IPaddress.ToString()
                    Return ip
                End If
                Application.DoEvents()
            Next
            Return String.Empty
        Catch ex As Exception
            BreakPoint.Dump(ex)
            ErrorLog("Warn:Unable to resolve name: " & ex.Message)
        End Try
        Return String.Empty

    End Function

    Public Function WANIP() As String

        Dim ipaddress As String = "127.0.0.1"
        Using client As New System.Net.WebClient ' download client for web page

            Try
                ipaddress = client.DownloadString("https://api.ipify.org")
            Catch ex1 As Exception
                Try
                    ipaddress = client.DownloadString("https://api.ip.sb/ip")
                Catch ex2 As Exception
                End Try
            End Try

        End Using
        Return ipaddress

    End Function

    Private Sub Bitch(msg As String)

        If Not RunningInServiceMode() Then
            MsgBox($"{Global.Outworldz.My.Resources.Error_word} Port conflict in {msg}", MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
        End If

    End Sub

End Module
