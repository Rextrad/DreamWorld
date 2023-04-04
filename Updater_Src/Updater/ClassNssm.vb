Imports System.Runtime
Imports System.Threading

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


    Public Sub StopService()

        NssmCommand("stop DreamGridService")

    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            ' free unmanaged resources (unmanaged objects) and override finalizer
            ' set large fields to null
            disposedValue = True
        End If
    End Sub

    Private Sub NssmCommand(command As String)

        Dim BootProcess = New Process
        BootProcess.StartInfo.UseShellExecute = True
        BootProcess.StartInfo.WorkingDirectory = CurDir()
        BootProcess.StartInfo.FileName = "nssm.exe"
        BootProcess.StartInfo.CreateNoWindow = True
        BootProcess.StartInfo.Arguments = command

        Try
            BootProcess.Start()
            Thread.Sleep(1000)
            BootProcess.WaitForExit()
        Catch ex As Exception
        End Try

    End Sub

End Class
