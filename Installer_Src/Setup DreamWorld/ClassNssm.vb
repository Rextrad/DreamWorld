Public Class ClassNssm
    Implements IDisposable
    Private disposedValue As Boolean

    Public Sub New()
    End Sub

    Public Sub DeleteService()

        If Not ServiceExists("DreamGridService") Then
            TextPrint(My.Resources.ServiceRemoved)
            FormSetup.ServiceToolStripMenuItemDG.Image = My.Resources.gear
            Return
        End If

        If NssmCommand("stop DreamGridService") Then
            TextPrint(My.Resources.ServiceFailedtoStop)
        End If
        Sleep(1000)
        If Not NssmCommand("remove DreamGridService confirm") Then
            Settings.RunAsService = False
            TextPrint(My.Resources.ServiceRemoved)
            FormSetup.ServiceToolStripMenuItemDG.Image = My.Resources.gear_stop
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

        If ServiceExists("DreamGridService") Then
            Return True
        End If

        If Not NssmCommand($"install DreamGridService ""{Settings.CurrentDirectory}\Start.exe"" service") Then

            NssmCommand("set DreamGridService Description DreamGridService DreamGridInstallService.bat=Install, DreamGridDeleteService.bat=Delete the service.")

            Settings.RunAsService = True
            TextPrint(My.Resources.ServiceInstalled)
            FormSetup.ServiceToolStripMenuItemDG.Image = My.Resources.gear
            Return True
        Else
            TextPrint(My.Resources.ServiceFailedtoInstall)
            FormSetup.ServiceToolStripMenuItemDG.Image = My.Resources.gear_error
            Return False
        End If

    End Function

    Public Function StartService() As Boolean

        If Not ServiceExists("DreamGridService") Then
            If Not InstallService() Then
                TextPrint(My.Resources.ServiceFailedtoInstall)
                FormSetup.ServiceToolStripMenuItemDG.Image = My.Resources.gear_error
                Return False
            End If
        End If

        If CheckPort2(Settings.LANIP, Settings.DiagnosticPort) Then
            Logger("Services", "DreamGrid Is Running As a service", "Outworldz")
            Return True
        End If

        If Not NssmCommand("start DreamGridService") Then
            TextPrint(My.Resources.Running_word)
            FormSetup.ServiceToolStripMenuItemDG.Image = My.Resources.gear_run
            Return True
        Else
            TextPrint(My.Resources.ServiceFailedtoStart)
            FormSetup.ServiceToolStripMenuItemDG.Image = My.Resources.gear_error
            Return False
        End If

    End Function

    Public Function StopService() As Boolean

        If Not ServiceExists("DreamGridService") Then
            FormSetup.ServiceToolStripMenuItemDG.Image = My.Resources.gear
            Return True
        End If

        If NssmCommand("stop DreamGridService") Then
            Sleep(5000)
            Return True
        End If
        Return False

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
