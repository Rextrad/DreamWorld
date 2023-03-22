Imports System.IO

Public Class ClassFilewatcher

    Private ReadOnly INI As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles")

    Public Sub Init()

    End Sub
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
        Debug.Print(Settings.MachineId)

        Dim _myFolder = Settings.CurrentDirectory
        Settings = New MySettings(_myFolder) With {
            .CurrentDirectory = _myFolder,
            .CurrentSlashDir = _myFolder.Replace("\", "/")    ' because MySQL uses Unix like slashes, that's why
            }

        Debug.Print("IP:" & Settings.MachineId)

    End Sub

End Class
