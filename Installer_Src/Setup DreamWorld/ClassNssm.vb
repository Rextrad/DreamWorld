Public Class ClassNssm
    Implements IDisposable
    Private disposedValue As Boolean

    Public Sub New()
    End Sub

    Public Sub DeleteService()

        NssmCommand("stop DreamGridService")

        If NssmCommand("remove DreamGridService confirm") Then
            Settings.RunAsService = False
            TextPrint(My.Resources.ServiceRemoved)
            FormSetup.ServiceToolStripMenuItem.Image = My.Resources.gear_stop
        Else
            TextPrint(My.Resources.ServiceFailedtoDelete)
        End If

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

    Public Function InstallService() As Boolean

        If NssmCommand($"install DreamGridService {Settings.CurrentDirectory}\Start.exe service") AndAlso
            NssmCommand("set DreamGridService Description DreamGridService DreamGridInstallService.bat=Install, DreamGridDeleteService.bat=Delete the service.") AndAlso
            NssmCommand("nssm set DreamGridService Start SERVICE_DELAYED_AUTO_START") Then

            Settings.RunAsService = True
            TextPrint(My.Resources.ServiceInstalled)
            FormSetup.ServiceToolStripMenuItem.Image = My.Resources.gear_stop
            Return True
        Else
            TextPrint(My.Resources.ServiceFailedtoInstall)
            FormSetup.ServiceToolStripMenuItem.Image = My.Resources.gear_error
            Return False
        End If

    End Function

    Public Function StartService() As Boolean

        If NssmCommand("start DreamGridService") Then
            TextPrint(My.Resources.Running_word)
            FormSetup.ServiceToolStripMenuItem.Image = My.Resources.gear_run
            Return True
        Else
            TextPrint(My.Resources.ServiceFailedtoStart)
            FormSetup.ServiceToolStripMenuItem.Image = My.Resources.gear_error
            Return False
        End If

    End Function

    Public Function StopService() As Boolean

        If NssmCommand("stop DreamGridService") Then
            FormSetup.ServiceToolStripMenuItem.Image = My.Resources.gear_stop
            TextPrint(My.Resources.Stopped_word)
            Return True
        Else
            TextPrint(My.Resources.ServiceFailedtoStop)
            Return False
        End If

    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then

            End If
            ' free unmanaged resources (unmanaged objects) and override finalizer
            ' set large fields to null
            disposedValue = True
        End If
    End Sub

    Private Function NssmCommand(command As String) As Boolean

        Dim BootProcess = New Process
        BootProcess.StartInfo.UseShellExecute = True
        BootProcess.StartInfo.FileName = IO.Path.Combine(Settings.CurrentDirectory(), "nssm.exe")
        BootProcess.StartInfo.CreateNoWindow = True
        BootProcess.StartInfo.Arguments = command

        Dim ok As Boolean = False
        Try
            BootProcess.Start()
            BootProcess.WaitForExit()
            Dim code = BootProcess.ExitCode
            If code = 0 Then
                TextPrint($"{My.Resources.Failedto} {command}")
                Return False
            End If
        Catch ex As Exception
            Logger("Failed to Run ", command, "Outworldz")
            Return False
        End Try

        Return True

    End Function

End Class
