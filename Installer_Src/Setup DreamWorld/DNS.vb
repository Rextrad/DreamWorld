Imports System.IO
Imports System.Net
Imports System.Threading.Tasks
Imports System.Windows.Interop

Module DNS

    Public Function GetNewDnsName() As String

        Dim Checkname As String

        Try
            Checkname = GetURLContents("http://ns1.outworldz.com/getnewname.plx/?r=" & RandomNumber.Random)
            Return Checkname
        Catch ex As Exception
            ErrorLog("Error:Cannot get new name fron NS1:" & ex.Message)
        End Try

        Try
            Checkname = GetURLContents("http://ns2.outworldz.com/getnewname.plx/?r=" & RandomNumber.Random)
            Return Checkname
        Catch ex As Exception
            ErrorLog("Error:Cannot get new name from NS2:" & ex.Message)
        End Try

        Return ""

    End Function

    Public Function GetURLContents(url As String) As String

        Dim wc = New System.Net.WebClient()
        ' Send the request to the Internet resource and wait for
        Return wc.DownloadString(url)

    End Function

    Public Sub NewDNSName()

        If Settings.DnsName.Length = 0 And Settings.EnableHypergrid Then
            Dim newname = GetNewDnsName()
            If newname.Length >= 0 And Not RunningInServiceMode() Then
                Dim r = RegisterName(newname)
                If r Then
                    Settings.DnsName = newname.ToString
                    Settings.PublicIP = newname.ToString
                    Settings.SaveSettings()
                Else
                    MsgBox(My.Resources.NameAlreadySet, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Information_word)
                End If
            End If

        End If

    End Sub

    Public Function RegisterName(DNSName As String) As Boolean

        DNSName = DNSName.Trim
        If DNSName Is Nothing Then Return False
        If DNSName.Length = 0 Then Return False
        Dim Checkname As String = ""

        If IPCheck.IsPrivateIP(DNSName) Then
            Settings.DnsTestPassed() = True
            Return True
        End If

        Dim DNS = New List(Of String) From {
             "http://ns1.outworldz.com/dns.plx" & GetPostData(DNSName),
             "http://ns2.outworldz.com/dns.plx" & GetPostData(DNSName),
             "http://ns1.outworldz.net/dns.plx" & GetPostData(DNSName),
             "http://ns2.outworldz.net/dns.plx" & GetPostData(DNSName)
            }

        For Each url In DNS
            Try
                Checkname = GetURLContents(url)
                Logger("DNS", url, "Outworldz")
            Catch ex As Exception
                ErrorLog("Warn: Cannot register this DNS Name " & ex.Message)
                If Not Debugger.IsAttached Then
                    Continue For
                End If
            End Try

            If Checkname = "UPDATE" Then
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

    Public Function SetPublicIP() As Boolean

        TextPrint(My.Resources.Public_IP_Setup_Word)

        If PropMyUPnpMap Is Nothing Then Return False

        Settings.LANIP = PropMyUPnpMap.LocalIP
        Settings.MacAddress = GetMacByIP(Settings.LANIP)

        Settings.BaseHostName = Settings.PublicIP
        RegisterName(Settings.PublicIP)
        TextPrint($"WAN->{Settings.PublicIP}")

        ' Region Name override
        If Settings.OverrideName.Length > 0 Then
            RegisterName(Settings.OverrideName)
            TextPrint($"REGION->{Settings.OverrideName}")
        End If

        ' set up the alternate DNS names
        Dim a As String() = Settings.AltDnsName.Split(",".ToCharArray())
        For Each part As String In a
            If part.Length > 0 Then
                RegisterName(part)
                TextPrint($"ALT->{part}")
            End If
        Next

        ' WAN USE
        If Settings.DnsName.Length = 0 Then
            Settings.PublicIP = Settings.LANIP
        ElseIf Settings.DnsName.Length > 0 Then
            Settings.PublicIP = Settings.DnsName()
        End If

        Settings.SaveSettings()

        Return True

    End Function

End Module
