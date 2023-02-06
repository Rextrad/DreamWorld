Module FreezeThaw

#Region "FreezeThaw"

    ''' <summary>
    ''' Freeze a region
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>

    Public Sub Freeze(RegionUUID As String)

        Dim PID = ProcessID(RegionUUID)
        ShowDOSWindow(RegionUUID, MaybeHideWindow())

        If Smart_Suspend_Enabled(RegionUUID) Then
            NtSuspendProcess(CachedProcess(PID).Handle)
            RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended
        ElseIf Smart_Boot_Enabled(RegionUUID) Then
            ShutDown(RegionUUID, SIMSTATUSENUM.Suspended)
        End If

    End Sub

    ''' <summary>
    ''' Suspends region
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    Public Sub PauseRegion(RegionUUID As String)

        If Settings.Smart_Start_Enabled And Smart_Suspend_Enabled(RegionUUID) And Settings.Smart_Start_Enabled And Settings.BootOrSuspend = False Then
            Dim Groupname = Group_Name(RegionUUID)
            For Each UUID As String In RegionUuidListByName(Groupname)
                BreakPoint.Print($"Pausing {Region_Name(UUID)}")
                Freeze(UUID)
            Next

        End If

    End Sub

    ''' <summary>
    ''' Thaw a region
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>
    Public Sub Thaw(RegionUUID As String)

        Dim PID = ProcessID(RegionUUID)

        Try
            NtResumeProcess(CachedProcess(PID).Handle)
        Catch
        End Try

    End Sub

#End Region

End Module
