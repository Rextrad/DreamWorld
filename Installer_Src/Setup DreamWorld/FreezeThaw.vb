﻿Module FreezeThaw

#Region "FreezeThaw"

    ''' <summary>
    ''' Freeze a region
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>

    Public Sub Freeze(RegionUUID As String)

        ShowDOSWindow(RegionUUID, MaybeHideWindow())

        If Smart_Suspend_Enabled(RegionUUID) Then
            Dim PID = GetPIDFromFile(RegionUUID)
            NtSuspendProcess(Process.GetProcessById(PID).Handle)
            RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended
        ElseIf Smart_Boot_Enabled(RegionUUID) Then
            ShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
        End If

    End Sub

    ''' <summary>
    ''' Suspends region
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    Public Sub PauseRegion(RegionUUID As String)

        If Smart_Suspend_Enabled(RegionUUID) AndAlso Settings.Smart_Start_Enabled Then
            Dim Groupname = Group_Name(RegionUUID)
            For Each UUID As String In RegionUuidListByName(Groupname)
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

        Dim PID = GetPIDFromFile(RegionUUID)

        Try
            NtResumeProcess(Process.GetProcessById(PID).Handle)
        Catch
        End Try

    End Sub

#End Region

End Module
