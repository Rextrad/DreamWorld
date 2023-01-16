﻿#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Module GridNames

    Public Sub SetServerType()

        If Settings.ServerType = RobustServerName Then
            TextPrint("--> " & My.Resources.Server_Type_is & " Robust")
        ElseIf Settings.ServerType = OsgridServer Then
            Settings.DnsName = "hg.osgrid.org"
            Settings.BaseHostName = "hg.osgrid.org"
            TextPrint("--> " & My.Resources.Server_Type_is & " OSGrid")
        ElseIf Settings.ServerType = RegionServerName Then
            TextPrint("--> " & My.Resources.Server_Type_is & " Region")
        ElseIf Settings.ServerType = MetroServer Then
            Settings.DnsName = "hg.metro.land"
            Settings.BaseHostName = "hg.metro.land"
            TextPrint("--> " & My.Resources.Server_Type_is & " Metro")
        End If

        If Settings.OverrideName.Length > 0 Then
            Settings.ExternalHostName = Settings.OverrideName
            TextPrint("--> Region IP=" & Settings.ExternalHostName)
        ElseIf Settings.ServerType = RobustName() Then
            Settings.ExternalHostName = Settings.PublicIP
            TextPrint("--> Region IP=" & Settings.PublicIP)
        End If

    End Sub

End Module
