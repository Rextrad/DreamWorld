#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Net
Imports System.Net.Sockets

Module PublicIP

    Public Sub CheckDefaultPorts()

        If Settings.DiagnosticPort = Settings.HttpPort _
        Or Settings.DiagnosticPort = Settings.PrivatePort _
        Or Settings.HttpPort = Settings.PrivatePort Then
            Settings.DiagnosticPort = 8001
            Settings.HttpPort = 8002
            Settings.PrivatePort = 8003

            MsgBox(My.Resources.Port_Error, MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
        End If

    End Sub

    ''' <summary>
    ''' Checks port or window handle to see if region is up
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    ''' <returns></returns>
    Public Function CheckPort(RegionUUID As String) As Boolean

        If RunningInServiceMode() Then
            If CheckPort2(Settings.PublicIP, GroupPort(RegionUUID)) Then
                Return True
            End If
        Else
            If CBool(GetHwnd(Group_Name(RegionUUID))) Then
                Return True
            End If
        End If
        Return False

    End Function

    ''' <summary>
    ''' Checks port to see if region is up
    ''' </summary>
    ''' <param name="ServerAddress">IP</param>
    ''' <param name="Port">Port</param>
    ''' <returns>True is in memory</returns>
    Public Function CheckPort2(ServerAddress As String, Port As Integer) As Boolean

        Dim success As Boolean
        Dim result As IAsyncResult = Nothing
        Using ClientSocket As New TcpClient
            Try
                result = ClientSocket.BeginConnect(ServerAddress, Port, Nothing, Nothing)
                success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1))
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

End Module
