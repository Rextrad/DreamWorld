Public Class ClassNssm
    Implements IDisposable
    Private disposedValue As Boolean

    Public Sub New()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

    Public Sub InstallService()

        If NssmCommand($"install DreamGridService {Settings.CurrentDirectory}\Start.exe service") Then
            NssmCommand("set DreamGridService Description DreamGridService DreamGridInstallService.bat=Install, DreamGridDeleteService.bat=Delete the service.")

            Settings.RunAsService = True
            TextPrint(My.Resources.ServiceInstalled)

        End If

    End Sub

    Public Sub StartService()
        If NssmCommand("start DreamGridService") Then
            TextPrint(My.Resources.ServiceInstalled)
        Else
            TextPrint(My.Resources.ServiceFailedtoStart)
        End If
    End Sub

    Public Sub StopAndDeleteService()

        If NssmCommand("stop DreamGridService") Then
            If NssmCommand("remove DreamGridService confirm") Then
                Settings.RunAsService = False
                TextPrint(My.Resources.ServiceRemoved)
            Else
                TextPrint(My.Resources.ServiceFailedtoDelete)
            End If
        End If

    End Sub

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

#Disable Warning CA2000 ' Dispose objects before losing scope
        Dim BootProcess = New Process
#Enable Warning CA2000 ' Dispose objects before losing scope

        BootProcess.StartInfo.UseShellExecute = True
        BootProcess.StartInfo.FileName = IO.Path.Combine(Settings.CurrentDirectory(), "nssm.exe")
        BootProcess.StartInfo.CreateNoWindow = True
        BootProcess.StartInfo.Arguments = command

        Dim ok As Boolean = False
        Try
            BootProcess.Start()
            BootProcess.WaitForExit()
            Dim code = BootProcess.ExitCode
            If code = 1 Then
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
