Imports System.Threading

Public Class ClassTeleport

    Public Sub New(o As Object)

        Dim start As ParameterizedThreadStart = AddressOf TPLoop
        Dim L = New Thread(start)
        L.SetApartmentState(ApartmentState.STA)
        L.Priority = ThreadPriority.Lowest ' UI gets priority
        L.Start(o)

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId:="Outworldz.ClassTeleport")>
    Private Sub TPLoop(o As Object)

        Dim AgentID = o.AvatarUUID.ToString
        Dim RegionToUUID = o.DestinationUUID.ToString

        Dim DestinationName = Region_Name(RegionToUUID)
        Dim FromRegionUUID As String = GetRegionFromAgentId(AgentID)
        Dim fromName = Region_Name(FromRegionUUID)
        If fromName Is Nothing Then Return
        If fromName.Length = 0 Then Return

        While True
            Dim status = RegionStatus(RegionToUUID)

            If status = SIMSTATUSENUM.Suspended Then
                ResumeRegion(RegionToUUID)
                Application.DoEvents()
                Sleep(1000)
                Continue While
            End If

            status = RegionStatus(RegionToUUID)

            If status = SIMSTATUSENUM.Booted And
                CheckPID(RegionToUUID) And
                RegionIsRegisteredOnline(RegionToUUID) Then

                If TeleportTo(FromRegionUUID, DestinationName, AgentID) Then
                    Logger("Teleport", $"{DestinationName} teleport command sent", "Teleport")
                    Return
                Else
                    Logger("Teleport", $"{DestinationName} failed to receive teleport", "Teleport")
                    BreakPoint.Print($"{DestinationName} failed to receive teleport")
                    RPC_admin_dialog(AgentID, $"Unable to locate region {Region_Name(RegionToUUID)}.")
                    Return
                End If
            Else ' not booted

                If Settings.TeleportSleepTime > 0 And Smart_Boot_Enabled(RegionToUUID) Then

                    If TeleportTo(FromRegionUUID, Settings.ParkingLot, AgentID) Then
                        Logger("Teleport", $"{DestinationName} teleport command sent", "Teleport")
                    End If

                    Sleep(1000)
                    RPC_admin_dialog(AgentID, $"{ Region_Name(RegionToUUID)} will be ready in {CStr(Settings.TeleportSleepTime)} seconds.")
                    Sleep(Settings.TeleportSleepTime * 1000)
                    Dim I = New ClassTeleport(o)

                    Return
                End If

            End If
            Sleep(1000)
        End While

    End Sub

End Class
