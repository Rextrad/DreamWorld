#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Collections.Concurrent
Imports System.Windows.Documents

Module Teleport

    Public Fin As New List(Of Guid)

    Public Sub TeleportAgents()

        '' get the first desination for any avatar. If two or more, skip the rest

        Dim used As New List(Of String)
        Try
            For Each O In TeleportQ.Data
                Dim AgentID = O.AvatarUUID
                Dim RegionToUUID = O.DestinationUUID
                Dim WhenSent = O.DateAdded

                'Only handle the 1st one
                If used.Contains(AgentID) Then Continue For
                used.Add(AgentID)

                Dim diff = DateAndTime.DateDiff(DateInterval.Minute, WhenSent, Date.Now)
                If diff < 0 Then
                    diff = 0
                End If

                ' if aded 5 minutes, clear it our
                If diff > 5 Then
                    Fin.Add(O.ID)
                    Continue For
                End If

                Dim status = RegionStatus(RegionToUUID)
                Dim Port As Integer = GroupPort(RegionToUUID)

                If status = SIMSTATUSENUM.Suspended Then
                    ResumeRegion(RegionToUUID)
                    Application.DoEvents()
                End If

                status = RegionStatus(RegionToUUID)
                If status = SIMSTATUSENUM.Stopped Then
                    Fin.Add(O.ID) ' cancel this, the region went away

                ElseIf status = SIMSTATUSENUM.Booted Then

                    ShowDOSWindow(RegionToUUID, MaybeShowWindow())

                    If CheckPID(RegionToUUID) And RegionIsRegisteredOnline(RegionToUUID) Then

                        Dim DestinationName = Region_Name(RegionToUUID)
                        Dim FromRegionUUID As String = GetRegionFromAgentId(AgentID)
                        Dim fromName = Region_Name(FromRegionUUID)
                        If fromName Is Nothing Then Fin.Add(O.ID)

                        If fromName.Length > 0 Then
                            If Settings.TeleportSleepTime > 0 And Smart_Boot_Enabled(RegionToUUID) Then
                                RPC_admin_dialog(AgentID, $"{ Region_Name(RegionToUUID)} will be ready in {CStr(Settings.TeleportSleepTime)} seconds.")
                                Sleep(Settings.TeleportSleepTime * 1000)
                                'TODO make this sleep a future promise
                            End If

                            If TeleportTo(FromRegionUUID, DestinationName, AgentID) Then
                                Logger("Teleport", $"{DestinationName} teleport command sent", "Teleport")
                                Fin.Add(O.ID)
                            Else
                                Logger("Teleport", $"{DestinationName} failed to receive teleport", "Teleport")
                                BreakPoint.Print($"{DestinationName} failed to receive teleport")
                                RPC_admin_dialog(AgentID, $"Unable to locate region { Region_Name(RegionToUUID)}.")
                                Fin.Add(O.ID) ' cancel this, the agent is not anywhere  we can get to
                            End If
                        Else
                            Fin.Add(O.ID) ' cancel this, the agent is not anywhere  we can get to
                        End If
                    End If
                    ShowDOSWindow(RegionToUUID, MaybeHideWindow())
                End If
            Next

            ' rem from the list as they have moved on
            For Each UUID In Fin
                For i = TeleportQ.Count - 1 To 0 Step -1
                    If TeleportQ.ID(i) = UUID Then
                        TeleportQ.RemoveAt(i)
                    End If
                Next
            Next
        Catch
        End Try

        Fin.Clear()

    End Sub

End Module
