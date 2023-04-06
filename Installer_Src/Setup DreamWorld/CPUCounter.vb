Module CPUCounter

    Private ReadOnly _counterList As New Dictionary(Of String, PerformanceCounter)

    Private ReadOnly _CPUValues As New Dictionary(Of String, Double)

    Private _PCList As Dictionary(Of Integer, PerformanceCounter)
    Private CalcCPUIsBusy As Boolean

    Public ReadOnly Property CPUValues As Dictionary(Of String, Double)
        Get
            Return _CPUValues
        End Get
    End Property

    Private ReadOnly Property CounterList As Dictionary(Of String, PerformanceCounter)
        Get
            Return _counterList
        End Get
    End Property

    Private ReadOnly Property PCList As Dictionary(Of Integer, PerformanceCounter)
        Get
            If _PCList Is Nothing Then _PCList = New Dictionary(Of Integer, PerformanceCounter)
            Return _PCList
        End Get
    End Property

    Public Sub AddCPU(PID As Integer, GName As String)

        If Not CounterList.ContainsKey(GName) Then
            Try
                Using counter As PerformanceCounter = GetPerfCounterForProcessId(PID)
                    If counter IsNot Nothing Then
                        CounterList.Add(GName, counter)
                        counter.NextValue() ' start the counter
                    End If
                End Using
            Catch ex As Exception
            End Try
        End If

    End Sub

    Public Sub CalcCPU()

        If RunningInServiceMode() Then Return

        If CalcCPUIsBusy Then
            Return
        End If
        CalcCPUIsBusy = True

        If PropOpensimIsRunning Then

            For Each RegionUUID In RegionUuids()

                Dim PID = GetPIDFromFile(Group_Name(RegionUUID))
                If PID = 0 Then
                    Continue For
                End If

                'Dim c As PerformanceCounter = Nothing
                Dim RegionName = Region_Name(RegionUUID)

                Dim Status = RegionStatus(RegionUUID)
                If Status = SIMSTATUSENUM.Stopped Or Status = SIMSTATUSENUM.Suspended Then
                    CPUValues.Item(Group_Name(RegionUUID)) = 0
                    Continue For
                End If

                If Not CounterList.ContainsKey(Group_Name(RegionUUID)) Then
                    Try
                        Using counter As PerformanceCounter = GetPerfCounterForProcessId(PID)
                            If counter IsNot Nothing Then
                                Debug.Print($"> Creating new CPU counter for {RegionName}")
                                CounterList.Add(Group_Name(RegionUUID), counter)
                                counter.NextValue() ' start the counter
                            End If
                        End Using
                    Catch ex As Exception
                        CounterList.Item(Group_Name(RegionUUID)).Close()
                        CounterList.Remove(Group_Name(RegionUUID))
                        CPUValues.Remove(Group_Name(RegionUUID))
                        Continue For
                    End Try
                Else
                    'Debug.Print("ctr exists")
                End If

                If Not CPUValues.ContainsKey(Group_Name(RegionUUID)) Then
                    CPUValues.Add(Group_Name(RegionUUID), 0)
                Else
                    Dim a As Double
                    Try
                        a = CDbl(CounterList.Item(Group_Name(RegionUUID)).NextValue())
                    Catch ex As Exception
                        CounterList.Remove(Group_Name(RegionUUID))
                    End Try

                    Dim b = (a / Environment.ProcessorCount)
                    CPUValues.Item(Group_Name(RegionUUID)) = Math.Round(b, 3)
                    ' Debug.Print($"> CPU {RegionName} = {CStr(Math.Round(b, 3))}")
                End If
            Next

        End If

        CalcCPUIsBusy = False

    End Sub

    Private Function GetInstanceNameForProcessId(ByVal processId As Integer) As String

        Try
            Dim cat As New PerformanceCounterCategory("Process")
            Dim instances As String() = cat.GetInstanceNames().Where(Function(inst) inst.StartsWith("opensim", System.StringComparison.OrdinalIgnoreCase)).ToArray()

            For Each instance As String In instances
                Using cnt = New PerformanceCounter("Process", "ID Process", instance, True)
                    Dim val As Integer = CInt(cnt.RawValue)
                    If val = processId Then
                        Return instance
                    End If
                End Using
                Application.DoEvents()
            Next
        Catch ex As Exception
        End Try

        Return Nothing
    End Function

    Private Function GetPerfCounterForProcessId(ByVal processId As Integer) As PerformanceCounter

        Dim processCounterName As String = "% Processor Time"
        If PCList.ContainsKey(processId) Then
            Return PCList.Item(processId)
        End If

        Dim instance As String = GetInstanceNameForProcessId(processId)
        If String.IsNullOrEmpty(instance) Then Return Nothing

        Dim PC = New PerformanceCounter("Process", processCounterName, instance)
        PCList.Add(processId, PC)
        Return PC

    End Function

End Module
