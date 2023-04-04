#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Public Class TosForm

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Private Methods"

    Private Sub ApplyButton_Click(sender As Object, e As EventArgs) Handles ApplyButton.Click

        Save()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles PreviewButton.Click

        If PropOpensimIsRunning() Then
            Dim webAddress As String = "http://" & CStr(Settings.PublicIP) & ":" & CStr(Settings.HttpPort) & "/wifi/termsofservice.html"
            Try
                Process.Start(webAddress)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        Else
            MsgBox(My.Resources.Not_Running)
        End If

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

        Settings.TOSEnabled = CheckBox1.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        My.Application.ChangeUICulture(Settings.Language)
        My.Application.ChangeCulture(Settings.Language)

        SaveButton.Text = My.Resources.Save_changes_word
        ApplyButton.Text = My.Resources.Apply_word
        PreviewButton.Text = My.Resources.Preview_in_Browser

        CheckBox1.Checked = Settings.TOSEnabled
        CheckBox1.Text = My.Resources.RequireTOS

        Dim reader As System.IO.StreamReader
        reader = System.IO.File.OpenText(IO.Path.Combine(Settings.CurrentDirectory, "tos.html"))

        'now loop through each line
        Dim HTML As String = ""
        While reader.Peek <> -1
            Dim Str = reader.ReadLine()
            Str = Str.Replace("'<!-- #get var=GridName -->'", "[GRIDNAME]")
            HTML = HTML + Str + vbCrLf
        End While
        reader.Close()
        Try
            Editor1.BodyHtml = HTML
        Catch ex As Exception
            ErrorLog(ex.Message)
        End Try

        SetScreen()

        HelpOnce("TOS")

    End Sub

    Private Sub Save()
        Try
            Dim arrList = New ArrayList(Editor1.BodyHtml.Split(New String() {Convert.ToChar(13), Convert.ToChar(10)}, StringSplitOptions.RemoveEmptyEntries))
            Dim HTML As String = ""
            Dim fname = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\WifiPages\tos.html")
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter(fname, False)

            For Each Str As String In arrList
                Str = Str.Replace("[GRIDNAME]", "'<!-- #get var=GridName -->'")
                file.WriteLine(Str & vbCrLf)
            Next
            file.Close()
            CopyFileFast(IO.Path.Combine(Settings.CurrentDirectory, "tos.html"), IO.Path.Combine(Settings.CurrentDirectory, "tos.html"))
        Catch
        End Try

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Save()
        Me.Close()

    End Sub

#End Region

End Class
