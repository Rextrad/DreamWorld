#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Module Icecast

    Public WithEvents IcecastProcess As New Process()
    Private _IcecastProcID As Integer
    Private _IcecastCrashCounter As Integer
    Private _IceCastExited As Boolean

#Region "Properties"

    Public Property IcecastCrashCounter As Integer
        Get
            Return _IcecastCrashCounter
        End Get
        Set(value As Integer)
            _IcecastCrashCounter = value
        End Set
    End Property

    Public Property PropIceCastExited() As Boolean
        Get
            Return _IceCastExited
        End Get
        Set(ByVal Value As Boolean)
            _IceCastExited = Value
        End Set
    End Property

    Public Property PropIcecastProcID As Integer
        Get
            Return _IcecastProcID
        End Get
        Set(value As Integer)
            _IcecastProcID = value
        End Set
    End Property

#End Region

#Region "Icecast"

    ''' <summary>Event handler for Icecast</summary>

    Public Sub IceCastExited(ByVal sender As Object, ByVal e As EventArgs)

        If PropAborting Then Return

        If Settings.RestartOnCrash AndAlso IcecastCrashCounter < 10 Then
            IcecastCrashCounter += 1
            PropIceCastExited = True
            Return
        End If
        IcecastCrashCounter = 0

        If Not RunningInServiceMode() Then
            Dim yesno = MsgBox(My.Resources.Icecast_Exited, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
            If yesno = MsgBoxResult.Yes Then
                Baretail("""" & IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Icecast\log\error.log") & """")
            End If
            ErrorLog(My.Resources.Icecast_Exited)
        End If

    End Sub

    Public Function StartIcecast() As Boolean

        If Not Settings.SCEnable Then
            TextPrint(Global.Outworldz.My.Resources.IceCast_disabled)
            IceCastIcon(False)
            Return True
        End If


        Try
            ' Check if DOS box exists, first, if so, its running.
            For Each p In Process.GetProcessesByName("icecast")
                If p.ProcessName = "icecast" Then
                    PropIcecastProcID = p.Id

                    p.EnableRaisingEvents = True
                    AddHandler p.Exited, AddressOf IceCastExited

                    IceCastIcon(True)
                    Return True
                End If
            Next
        Catch
        End Try

        DoIceCast()

        DeleteFile(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Icecast\log\access.log"))
        'Launch .\bin\icecast.exe -c .\icecast_run.xml

        PropIcecastProcID = 0
        TextPrint(My.Resources.Icecast_starting)
        IcecastProcess.EnableRaisingEvents = True
        IcecastProcess.StartInfo.UseShellExecute = True
        IcecastProcess.StartInfo.FileName = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\icecast\bin\icecast.exe")
        IcecastProcess.StartInfo.CreateNoWindow = True
        IcecastProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        IcecastProcess.StartInfo.Arguments = "-c .\icecast_run.xml"
        IcecastProcess.StartInfo.WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\icecast")
        AddHandler IcecastProcess.Exited, AddressOf IceCastExited

        Try
            IcecastProcess.Start()
        Catch ex As Exception
            BreakPoint.Dump(ex)
            TextPrint(My.Resources.Icecast_failed & ":" & ex.Message)
            IceCastIcon(False)
            Return False
        End Try

        PropIcecastProcID = WaitForPID(IcecastProcess)
        If PropIcecastProcID = 0 Then
            BreakPoint.Print(My.Resources.Icecast_failed)
            TextPrint(My.Resources.Icecast_failed)
            IceCastIcon(False)
            Return False
        End If

        IceCastIcon(True)

        PropIceCastExited = False


        Return True

    End Function

#End Region

    Public Sub IceCastIcon(Running As Boolean)

        If Not Running Then
            FormSetup.RestartIcecastIcon.Image = Global.Outworldz.My.Resources.nav_plain_red
        Else
            FormSetup.RestartIcecastIcon.Image = Global.Outworldz.My.Resources.check2
        End If
        Application.DoEvents()

    End Sub


    Public Sub StopIcecast()

        Zap("icecast")
        IceCastIcon(False)

    End Sub

End Module
