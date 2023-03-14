Imports System.IO

Public Class ClassFilewatcher

    Private ReadOnly INI As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles")

    Public Sub New()

        ' Create a FileSystemWatcher object passing it the folder to watch.
        '
        Dim fsw As New FileSystemWatcher(INI)

        '
        ' Assign event procedures to the events to watch.
        '
        AddHandler fsw.Changed, AddressOf OnChanged

        With fsw
            .EnableRaisingEvents = True
            .IncludeSubdirectories = False
            '
            ' Specify the event to watch for.
            '
            '.WaitForChanged(WatcherChangeTypes.Changed)

            ' Watch certain file types.
            .Filter = "Settings.ini"
            '
            ' Specify file change notifications.
            '
            .NotifyFilter = (NotifyFilters.LastWrite)

        End With

    End Sub

    Private Sub OnChanged(ByVal source As Object, ByVal e As FileSystemEventArgs)

        Debug.Print("File changed: " & e.FullPath & " change type: " & e.ChangeType)
        Sleep(10)
        Dim I = New LoadIni(IO.Path.Combine(INI, "Settings.ini"), ";", System.Text.Encoding.UTF8)

    End Sub

End Class
