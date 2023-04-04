Imports System.IO
Imports System.Net
Imports System.Threading.Tasks

Module DNS

    Public Async Function GetNewDnsNameAsync() As Task(Of String)

        Dim Checkname() As Byte

        Try
            Checkname = Await GetURLContentsAsync("http://ns1.outworldz.com/getnewname.plx/?r=" & RandomNumber.Random)
            Dim msg = System.Text.Encoding.ASCII.GetString(Checkname)
            Return msg
        Catch ex As Exception
            ErrorLog("Error:Cannot get new name fron NS1:" & ex.Message)
        End Try

        Try
            Checkname = Await GetURLContentsAsync("http://ns2.outworldz.com/getnewname.plx/?r=" & RandomNumber.Random)
            Dim msg = System.Text.Encoding.ASCII.GetString(Checkname)
            Return msg
        Catch ex As Exception
            ErrorLog("Error:Cannot get new name from NS2:" & ex.Message)
        End Try

        Try
            Checkname = Await GetURLContentsAsync("http://ns3.outworldz.com/getnewname.plx/?r=" & RandomNumber.Random)
            Dim msg = System.Text.Encoding.ASCII.GetString(Checkname)
            Return msg
        Catch ex As Exception
            ErrorLog("Error:Cannot get new name from NS3:" & ex.Message)
        End Try

        Return ""

    End Function

    Public Async Function GetURLContentsAsync(url As String) As Task(Of Byte())

        Dim content = New MemoryStream()
        ' Initialize an HttpWebRequest for the current URL.
        Dim webReq = CType(WebRequest.Create(url), HttpWebRequest)
        webReq.AllowAutoRedirect = True

        ' Send the request to the Internet resource and wait for
        ' the response.
        Using response As WebResponse = Await webReq.GetResponseAsync()
            ' Get the data stream that is associated with the specified URL.
            Using responseStream As Stream = response.GetResponseStream()
                ' Read the bytes in responseStream and copy them to content.
                Await responseStream.CopyToAsync(content)
            End Using
        End Using
        Return content.ToArray()

    End Function

    Public Sub NewDNSName()

        If Settings.DnsName.Length = 0 And Settings.EnableHypergrid Then
            Dim newname = GetNewDnsNameAsync()
            If newname.ToString.Length >= 0 And Not RunningInServiceMode() Then
                Dim r = RegisterNameAsync(newname.ToString).ToString
                If Not CBool(r) Then
                    Settings.DnsName = newname.ToString
                    Settings.PublicIP = newname.ToString
                    Settings.SaveSettings()

                    MsgBox(My.Resources.NameAlreadySet, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Information_word)
                End If
            End If

        End If

    End Sub

    Public Async Function RegisterNameAsync(DNSName As String) As Task(Of Boolean)

        DNSName = DNSName.Trim
        If DNSName Is Nothing Then Return False
        If DNSName.Length = 0 Then Return False
        Dim Checkname As Byte() = Nothing

        If IPCheck.IsPrivateIP(DNSName) Then
            Settings.DnsTestPassed() = True
            Return True
        End If

        Dim DNS = New List(Of String) From {
             "http://ns1.outworldz.com/dns.plx" & GetPostData(DNSName),
             "http://ns2.outworldz.com/dns.plx" & GetPostData(DNSName),
             "http://ns1.outworldz.net/dns.plx" & GetPostData(DNSName),
             "http://ns2.outworldz.net/dns.plx" & GetPostData(DNSName),
             "http://ns3.outworldz.net/dns.plx" & GetPostData(DNSName),
             "http://ns3.outworldz.com/dns.plx" & GetPostData(DNSName)
            }

        For Each url In DNS
            Try
                Checkname = Await GetURLContentsAsync(url)
                Logger("DNS", url, "Outworldz")
            Catch ex As Exception
                ErrorLog("Warn: Cannot register this DNS Name " & ex.Message)
            End Try
            Dim name = System.Text.Encoding.ASCII.GetString(Checkname)
            If name = "UPDATE" Then
                Settings.DnsTestPassed() = True
                Return True
            Else
                If Not RunningInServiceMode() Then
                    MsgBox(DNSName & ":" & My.Resources.DDNS_In_Use, vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
                    Exit For
                End If

            End If
            Application.DoEvents()
        Next

        Settings.DnsTestPassed() = False
        Return False

    End Function

    Public Async Function SetPublicIPAsync() As Task(Of Boolean)

        TextPrint(My.Resources.Public_IP_Setup_Word)

        If PropMyUPnpMap Is Nothing Then Return False

        Settings.LANIP = PropMyUPnpMap.LocalIP
        Settings.MacAddress = GetMacByIP(Settings.LANIP)

        ' Region Name override
        If Settings.OverrideName.Length > 0 Then
            Await RegisterNameAsync(Settings.OverrideName)
        End If

        ' set up the alternate DNS names
        Dim a As String() = Settings.AltDnsName.Split(",".ToCharArray())
        For Each part As String In a
            If part.Length > 0 Then
                Await RegisterNameAsync(part)
            End If
        Next

        ' WAN USE
        If Settings.DnsName.Length = 0 Then
            Settings.PublicIP = Settings.LANIP
        ElseIf Settings.DnsName.Length > 0 Then
            Settings.PublicIP = Settings.DnsName()
        End If

        Settings.BaseHostName = Settings.PublicIP

        Await RegisterNameAsync(Settings.PublicIP)
        Settings.SaveSettings()

        Return True

    End Function

End Module
