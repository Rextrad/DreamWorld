﻿Module FreezeThaw

#Region "FreezeThaw"

    ''' <summary>
    ''' Freeze a region
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>

    Public Sub Freeze(RegionUUID As String)

        Try
            If Smart_Suspend_Enabled(RegionUUID) Then
                Dim PID = GetPIDFromFile(Group_Name(RegionUUID))
                Dim retval = NtSuspendProcess(Process.GetProcessById(PID).Handle)
                If CBool(retval) Then
                    ErrorLog($"Cannot freeze {Region_Name(RegionUUID)}")
                End If
                ShowDOSWindow(RegionUUID, MaybeHideWindow())
                RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended
            ElseIf Smart_Boot_Enabled(RegionUUID) Then
                ShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
            End If
        Catch ex As Exception
            ErrorLog(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Suspends region
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    Public Sub PauseRegion(RegionUUID As String)

        If Smart_Suspend_Enabled(RegionUUID) AndAlso Settings.Smart_Start_Enabled Then
            Dim Groupname = Group_Name(RegionUUID)
            For Each UUID As String In RegionUuidListFromGroup(Groupname)
                Logger("State", $"Pausing {Region_Name(UUID)}", "Outworldz")
                Freeze(UUID)
            Next

        End If

    End Sub

    ''' <summary>
    ''' Thaw a region
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>
    Public Sub Thaw(RegionUUID As String)

        Dim PID = GetPIDFromFile(Group_Name(RegionUUID))
        If PID > 0 Then
            Try
                NtResumeProcess(Process.GetProcessById(PID).Handle)

                If Settings.Smart_Start_Enabled And Smart_Suspend_Enabled(RegionUUID) Then
                    RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted
                End If
            Catch
            End Try
        End If

    End Sub

#End Region

End Module
