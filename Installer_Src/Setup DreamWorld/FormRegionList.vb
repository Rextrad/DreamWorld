﻿Imports System.IO

Public Class RegionList

#Region "Declarations"

    Dim ViewNotBusy As Boolean
    Dim TheView As Integer = ViewType.Details
    Private Shared FormExists As Boolean = False
    Dim pixels As Integer = 70
    Dim imageListSmall As New ImageList
    Dim imageListLarge As ImageList
    Dim ItemsAreChecked As Boolean = False
    Dim RegionClass As RegionMaker = RegionMaker.Instance(Form1.MysqlConn)
    Dim Timertick As Integer = 0

    Private Enum ViewType As Integer
        Maps = 0
        Icons = 1
        Details = 2
        Avatars = 3
    End Enum

#End Region

#Region "Properties"

    Shared Property UpdateView() As Boolean
        Get
            Return Form1.UpdateView
        End Get
        Set(ByVal Value As Boolean)
            Form1.UpdateView = Value
        End Set
    End Property

    ' property exposing FormExists
    Public Shared ReadOnly Property InstanceExists() As Boolean
        Get
            ' Access shared members through the Class name, not an instance.
            Return RegionList.FormExists
        End Get
    End Property

    Public Property ScreenPosition As ScreenPos
        Get
            Return _screenPosition
        End Get
        Set(value As ScreenPos)
            _screenPosition = value
        End Set
    End Property

#End Region

#Region "ScreenSize"

    Private _screenPosition As ScreenPos
    Private Handler As New EventHandler(AddressOf Resize_page)

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)

        ScreenPosition.SaveXY(Me.Left, Me.Top)
        ScreenPosition.SaveHW(Me.Height, Me.Width)
    End Sub

    Private Sub SetScreen(View As Integer)
        Me.Show()
        ScreenPosition = New ScreenPos(MyBase.Name & View.ToString(Form1.usa))
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
        Dim hw As List(Of Integer) = ScreenPosition.GetHW()

        If hw.Item(0) = 0 Then
            Me.Height = 400
        Else
            Me.Height = hw.Item(0)
        End If
        If hw.Item(1) = 0 Then
            Me.Width = 560
        Else
            Me.Width = hw.Item(1)
        End If

    End Sub

#End Region

#Region "Layout"

    Private Sub Panel1_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseWheel

        If TheView = ViewType.Maps And ViewNotBusy Then
            ' Update the drawing based upon the mouse wheel scrolling.
            Dim numberOfTextLinesToMove As Integer = CInt(e.Delta * SystemInformation.MouseWheelScrollLines / 120)

            pixels += numberOfTextLinesToMove
            'Debug.Print(pixels.ToString)
            If pixels > 256 Then pixels = 256
            If pixels < 10 Then pixels = 10

            LoadMyListView()
        End If

    End Sub

    Private Sub RegionList_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout

        Dim X = Me.Width - 45
        Dim Y = Me.Height - 125
        ListView1.Size = New System.Drawing.Size(X, Y)
        AvatarView.Size = New System.Drawing.Size(X, Y)

    End Sub

#End Region

    Enum DGICON

        ' index of 0-4 to display icons
        bootingup = 0

        shuttingdown = 1
        up = 2
        disabled = 3
        stopped = 4
        recyclingdown = 5
        recyclingup = 6
        warning = 7
        user1 = 8
        user2 = 9
    End Enum

#Region "Loader"

    Private Sub LoadForm(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pixels = 70
        RegionList.FormExists = True

        Form1.MySetting.RegionListVisible = True
        Form1.MySetting.SaveSettings()

        AvatarView.Hide()
        AvatarView.CheckBoxes = False

        ' ListView Setup
        ListView1.AllowDrop = True

        ' Set the view to show details.
        TheView = Form1.MySetting.RegionListView()
        Dim W As View

        If TheView = ViewType.Details Then
            W = View.Details
            ListView1.CheckBoxes = True
        ElseIf TheView = ViewType.Icons Then
            W = View.SmallIcon
            ListView1.CheckBoxes = False
        ElseIf TheView = ViewType.Maps Then
            ListView1.CheckBoxes = False
            W = View.LargeIcon
        End If

        ListView1.View = W
        AvatarView.View = View.Details

        ' Allow the user to edit item text.
        ListView1.LabelEdit = False
        AvatarView.LabelEdit = False

        ' Allow the user to rearrange columns.
        ListView1.AllowColumnReorder = True
        AvatarView.AllowColumnReorder = False

        Me.ListView1.ListViewItemSorter = New ListViewItemComparer(2)
        Me.AvatarView.ListViewItemSorter = New ListViewItemComparer(1)

        ' Select the item and subitems when selection is made.
        ListView1.FullRowSelect = True
        AvatarView.FullRowSelect = True

        ' Display grid lines.
        ListView1.GridLines = True
        AvatarView.GridLines = True

        ' Sort the items in the list in ascending order.
        ListView1.Sorting = SortOrder.Ascending
        AvatarView.Sorting = SortOrder.Ascending

        ' Create columns for the items and subitems.
        ' Width of -2 indicates auto-size.
        ListView1.Columns.Add("Enabled", 120, HorizontalAlignment.Center)
        ListView1.Columns.Add("DOS Box", 100, HorizontalAlignment.Center)
        ListView1.Columns.Add("Agents", 50, HorizontalAlignment.Center)
        ListView1.Columns.Add("Status", 120, HorizontalAlignment.Center)
        ListView1.Columns.Add("RAM", 80, HorizontalAlignment.Center)
        ListView1.Columns.Add("X", 50, HorizontalAlignment.Center)
        ListView1.Columns.Add("Y", 50, HorizontalAlignment.Center)
        ListView1.Columns.Add("Size", 40, HorizontalAlignment.Center)
        ' optional
        ListView1.Columns.Add("Map", 80, HorizontalAlignment.Center)
        ListView1.Columns.Add("Physics", 120, HorizontalAlignment.Center)
        ListView1.Columns.Add("Birds", 60, HorizontalAlignment.Center)
        ListView1.Columns.Add("Tides", 60, HorizontalAlignment.Center)
        ListView1.Columns.Add("Teleport", 60, HorizontalAlignment.Center)
        ListView1.Columns.Add("SmartStart", 80, HorizontalAlignment.Center)

        'Add the items to the ListView.
        ' Connect the ListView.ColumnClick event to the ColumnClick event handler.
        AddHandler Me.ListView1.ColumnClick, AddressOf ColumnClick
        Me.Name = "Region List"
        Me.Text = "Region List"

        AvatarView.Columns.Add("Agent", 150, HorizontalAlignment.Left)
        AvatarView.Columns.Add("Region", 150, HorizontalAlignment.Center)
        AvatarView.Columns.Add("Type", 80, HorizontalAlignment.Center)

        ' index  to display icons
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_up2", Form1.usa))   ' 0 booting up
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_down2", Form1.usa)) ' 1 shutting down
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("check2", Form1.usa)) ' 2 okay, up
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("media_stop_red", Form1.usa)) ' 3 disabled
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("media_stop", Form1.usa))  ' 4 enabled, stopped
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_down", Form1.usa))  ' 5 Recycling down
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_up", Form1.usa))  ' 6 Recycling Up
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("warning", Form1.usa))  ' 7 Unknown
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("user2", Form1.usa))  ' 8 - 1 User
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("users1", Form1.usa))  ' 9 - 2 user

        Form1.UpdateView = True ' make form refresh

        LoadMyListView()
        Timer1.Interval = 1000 ' check for Form1.UpdateView every second
        Timer1.Start() 'Timer starts functioning

        SetScreen(TheView)

        Form1.HelpOnce("RegionList")

    End Sub

    Private Sub SingletonForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        RegionList.FormExists = False
        Form1.MySetting.RegionListVisible = False
        Form1.MySetting.SaveSettings()

    End Sub

#End Region

#Region "Timer"

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If UpdateView() Or Timertick Mod 120 = 0 Then ' force a refresh
            LoadMyListView()
        End If
        Timertick += 1

    End Sub

#End Region

#Region "Load List View"

    Public Sub LoadMyListView()

        If TheView = ViewType.Avatars Then
            ShowAvatars()
        Else
            ShowRegions()
        End If

    End Sub

    Private Sub ShowRegions()

        ListView1.Show()
        AvatarView.Hide()

        Try
            ViewNotBusy = False
            ListView1.BeginUpdate()

            imageListLarge = New ImageList()
            If pixels = 0 Then pixels = 20
            imageListLarge.ImageSize = New Size(pixels, pixels)
            ListView1.Items.Clear()

            Dim Num As Integer = 0

            ' have to get maps by http port + region UUID, not region port + uuid
            ' RegionClass.DebugGroup() ' show the list of groups and http ports.

            For Each X In RegionClass.RegionNumbers

                Dim Letter As String = ""
                If RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.RecyclingDown Then
                    Letter = "Recycling Down"
                    Num = DGICON.recyclingdown
                ElseIf RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.RecyclingUp Then
                    Letter = "Recycling Up"
                    Num = DGICON.recyclingup
                ElseIf RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.RestartPending Then
                    Letter = "Restart Pending"
                    Num = DGICON.recyclingup
                ElseIf RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.RetartingNow Then
                    Letter = "Restarting Now"
                    Num = DGICON.recyclingup
                ElseIf RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.Booting Then
                    Letter = "Booting"
                    Num = DGICON.bootingup
                ElseIf RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.ShuttingDown Then
                    Letter = "Stopping"
                    Num = DGICON.shuttingdown
                ElseIf RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.Booted And RegionClass.AvatarCount(X) = 1 Then
                    Letter = "Running"
                    Num = DGICON.user1
                ElseIf RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.Booted And RegionClass.AvatarCount(X) > 1 Then
                    Letter = "Running"
                    Num = DGICON.user2
                ElseIf RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.Booted Then
                    Letter = "Running"
                    Num = DGICON.up
                ElseIf Not RegionClass.RegionEnabled(X) Then
                    Letter = "Disabled"
                    Num = DGICON.disabled
                ElseIf RegionClass.RegionEnabled(X) Then
                    Letter = "Stopped"
                    Num = DGICON.stopped
                Else
                    Num = DGICON.warning ' warning
                End If

                ' maps
                If TheView = ViewType.Maps Then

                    If RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.Booted Then
                        Dim img As String = "http://127.0.0.1:" + RegionClass.GroupPort(X).ToString(Form1.usa) + "/" + "index.php?method=regionImage" + RegionClass.UUID(X).Replace("-", "")
                        Debug.Print(img)

                        Dim bmp As Image = LoadImage(img)
                        If bmp Is Nothing Then
                            imageListLarge.Images.Add(My.Resources.ResourceManager.GetObject("OfflineMap", Form1.usa))
                        Else
                            imageListLarge.Images.Add(bmp)
                        End If
                    Else
                        imageListLarge.Images.Add(My.Resources.ResourceManager.GetObject("OfflineMap", Form1.usa))
                    End If
                    Num = X

                End If

                ' Create items and subitems for each item.
                ' Place a check mark next to the item.
                Dim item1 As New ListViewItem(RegionClass.RegionName(X), Num) With {
                    .Checked = RegionClass.RegionEnabled(X)
                }
                item1.SubItems.Add(RegionClass.GroupName(X).ToString(Form1.usa))
                item1.SubItems.Add(RegionClass.AvatarCount(X).ToString(Form1.usa))
                item1.SubItems.Add(Letter)
                Dim fmtXY = "00000" ' 65536
                Dim fmtRam = "0000." ' 9999 MB
                ' RAM
                Try
                    Dim PID = RegionClass.ProcessID(X)
                    Dim component1 As Process = Process.GetProcessById(PID)
                    If Form1.RegionHandles.ContainsKey(PID) Then
                        Dim NotepadMemory As Double = (component1.WorkingSet64 / 1024) / 1024
                        item1.SubItems.Add(FormatNumber(NotepadMemory.ToString(fmtRam, Form1.usa)))
                    Else
                        item1.SubItems.Add("0")
                    End If
                Catch ex As Exception
                    item1.SubItems.Add("0")
                End Try
                item1.SubItems.Add(RegionClass.CoordX(X).ToString(fmtXY, Form1.usa))
                item1.SubItems.Add(RegionClass.CoordY(X).ToString(fmtXY, Form1.usa))

                Dim size As String = ""
                If RegionClass.SizeX(X) = 256 Then
                    size = "1X1"
                ElseIf RegionClass.SizeX(X) = 512 Then
                    size = "2X2"
                ElseIf RegionClass.SizeX(X) = 768 Then
                    size = "3X3"
                ElseIf RegionClass.SizeX(X) = 1024 Then
                    size = "4X4"
                Else
                    size = RegionClass.SizeX(X).ToString(Form1.usa)
                End If
                item1.SubItems.Add(size)

                'Map
                If RegionClass.MapType(X).Length > 0 Then
                    item1.SubItems.Add(RegionClass.MapType(X))
                Else
                    item1.SubItems.Add(Form1.MySetting.MapType)
                End If

                ' physics
                Select Case RegionClass.Physics(X)
                    Case "0"
                        item1.SubItems.Add("None")
                    Case "1"
                        item1.SubItems.Add("ODE")
                    Case "2"
                        item1.SubItems.Add("Bullet")
                    Case "3"
                        item1.SubItems.Add("Bullet/Threaded")
                    Case "4"
                        item1.SubItems.Add("ubODE")
                    Case "5"
                        item1.SubItems.Add("ubODE Hybrid")
                    Case Else
                        Select Case Form1.MySetting.Physics
                            Case "0"
                                item1.SubItems.Add("None")
                            Case "1"
                                item1.SubItems.Add("ODE")
                            Case "2"
                                item1.SubItems.Add("Bullet")
                            Case "3"
                                item1.SubItems.Add("Bullet/Threaded")
                            Case "4"
                                item1.SubItems.Add("ubODE")
                            Case "5"
                                item1.SubItems.Add("ubODE Hybrid")
                            Case Else
                                item1.SubItems.Add("?")
                        End Select
                End Select

                'birds

                If RegionClass.Birds(X) = "True" Then
                    item1.SubItems.Add("Yes")
                Else
                    item1.SubItems.Add("")
                End If

                'Tides
                If RegionClass.Tides(X) = "True" Then
                    item1.SubItems.Add("Yes")
                Else
                    item1.SubItems.Add("")
                End If

                'teleport
                If RegionClass.Teleport(X) = "True" Then
                    item1.SubItems.Add("Yes")
                Else
                    item1.SubItems.Add("")
                End If

                If RegionClass.SmartStart(X) = "True" Then
                    item1.SubItems.Add("Yes")
                Else
                    item1.SubItems.Add("")
                End If


                ListView1.Items.AddRange(New ListViewItem() {item1})

            Next

            'Assign the ImageList objects to the ListView.
            ListView1.LargeImageList = imageListLarge
            ListView1.SmallImageList = imageListSmall

            Me.ListView1.TabIndex = 0

            For i As Integer = 0 To ListView1.Items.Count - 1
                If ListView1.Items(i).Checked Then
                    ListView1.Items(i).ForeColor = SystemColors.ControlText
                Else
                    ListView1.Items(i).ForeColor = SystemColors.GrayText
                End If
            Next i

            ListView1.EndUpdate()
            ViewNotBusy = True
            UpdateView() = False
        Catch ex As Exception
            Form1.Log("Error", " RegionList " & ex.Message)
            RegionClass.GetAllRegions()
        End Try

    End Sub

    Private Sub ShowAvatars()
        Try
            ViewNotBusy = False

            AvatarView.Show()
            ListView1.Hide()

            AvatarView.BeginUpdate()
            AvatarView.Items.Clear()

            Dim Index = 0
            Try
                ' Create items and subitems for each item.

                Dim L As New Dictionary(Of String, String)
                L = Form1.MysqlConn.GetAgentList()

                For Each Agent In L
                    Dim item1 As New ListViewItem(Agent.Key, Index)
                    item1.SubItems.Add(Agent.Value)
                    item1.SubItems.Add("Local")
                    AvatarView.Items.AddRange(New ListViewItem() {item1})
                    Index += 1
                Next

                If Index = 0 Then
                    Dim item1 As New ListViewItem("No Avatars", Index)
                    item1.SubItems.Add("-")
                    item1.SubItems.Add("Local Grid")
                    AvatarView.Items.AddRange(New ListViewItem() {item1})
                    Index += 1
                End If
            Catch ex As Exception
                Dim item1 As New ListViewItem("No Avatars", Index)
                item1.SubItems.Add("-")
                item1.SubItems.Add("Local Grid")
                AvatarView.Items.AddRange(New ListViewItem() {item1})
                Index += 1
            End Try

            ' Hypergrid
            Try
                ' Create items and subitems for each item.
                Dim L As New Dictionary(Of String, String)
                L = Form1.MysqlConn.GetHGAgentList()

                For Each Agent In L
                    If Agent.Value.Length > 0 Then
                        Dim item1 As New ListViewItem(Agent.Key, Index)
                        item1.SubItems.Add(Agent.Value)
                        item1.SubItems.Add("Hypergrid")
                        AvatarView.Items.AddRange(New ListViewItem() {item1})
                        Index += 1
                    End If
                Next

                If L.Count = 0 Then
                    Dim item1 As New ListViewItem("No Avatars", Index)
                    item1.SubItems.Add("-")
                    item1.SubItems.Add("Hypergrid")
                    AvatarView.Items.AddRange(New ListViewItem() {item1})
                End If
            Catch
                Dim item1 As New ListViewItem("No Avatars", Index)
                item1.SubItems.Add("-")
                item1.SubItems.Add("Hypergrid")
                AvatarView.Items.AddRange(New ListViewItem() {item1})
            End Try

            Me.AvatarView.TabIndex = 0
            AvatarView.EndUpdate()
            AvatarView.Show()

            ViewNotBusy = True
            UpdateView() = False
        Catch ex As Exception
            Form1.Log("Error", " RegionList " & ex.Message)
        End Try

    End Sub

    Shared Function LoadImage(S As String) As Image
        Dim bmp As Bitmap = Nothing
        Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(S)
        Try
            Dim response As System.Net.WebResponse = request.GetResponse()
            Dim responseStream As System.IO.Stream = response.GetResponseStream()
            bmp = New Bitmap(responseStream)

            'Dim s = bmp.Size
            'Debug.Print(s.Width.ToString(Form1.usa) + ":" + s.Height.ToString)

            responseStream.Dispose()
        Catch ex As Exception
            Form1.Log("Maps", ex.Message + ":" + S)
        End Try

        Return bmp

    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        RegionClass.GetAllRegions()
        LoadMyListView()

    End Sub

    Private Sub ListClick(sender As Object, e As EventArgs) Handles ListView1.Click
        Dim regions As ListView.SelectedListViewItemCollection = Me.ListView1.SelectedItems
        Dim item As ListViewItem

        For Each item In regions
            Dim RegionName = item.SubItems(0).Text
            Dim checked As Boolean = item.Checked
            Debug.Print("Clicked row " + RegionName)
            Dim R = RegionClass.FindRegionByName(RegionName)
            If R >= 0 Then
                StartStopEdit(checked, R, RegionName)
            End If
        Next

    End Sub

    Private Sub AvatarView_Click(sender As Object, e As EventArgs) Handles AvatarView.Click

        Dim regions As ListView.SelectedListViewItemCollection = Me.AvatarView.SelectedItems
        Dim item As ListViewItem
        '!!! help and double click msg
        For Each item In regions
            Try
                Dim RegionName = item.SubItems(1).Text
                Debug.Print("Clicked row " + RegionName)
                Dim R = RegionClass.FindRegionByName(RegionName)
                If R >= 0 Then
                    Dim webAddress As String = "hop://" & Form1.MySetting.DNSName & ":" & Form1.MySetting.HttpPort & "/" & RegionName
                    Dim result = Process.Start(webAddress)
                End If
            Catch ex As Exception
                ' Form1.Log("Error", ex.Message)
            End Try
        Next
        UpdateView() = True

    End Sub

    Private Sub StartStopEdit(checked As Boolean, n As Integer, RegionName As String)

        ' show it, stop it, start it, or edit it
        Dim hwnd = Form1.GetHwnd(RegionClass.GroupName(n))
        Form1.ShowDOSWindow(hwnd, Form1.SHOWWINDOWENUM.SWRESTORE)

        Dim Choices As New FormRegionPopup
        Dim chosen As String
        Choices.Init(RegionName)
        Choices.ShowDialog()
        Try
            ' Read the chosen sim name
            chosen = Choices.Choice()
            If chosen = "Start" Then

                ' it was stopped, and off, so we start up
                If Not Form1.StartMySQL() Then
                    Form1.ProgressBar1.Value = 0
                    Form1.ProgressBar1.Visible = True
                    Form1.Print("Stopped")
                End If
                Form1.StartRobust()
                Form1.Log("Starting", RegionClass.RegionName(n))
                Form1.CopyOpensimProto(RegionClass.RegionName(n))
                Form1.Boot(RegionClass, RegionClass.RegionName(n))
                Form1.Timer1.Start() 'Timer starts functioning
                UpdateView() = True ' force a refresh

            ElseIf chosen = "Stop" Then

                ' if any avatars in any region, give them a choice.
                Dim StopIt As Boolean = True
                For Each num In RegionClass.RegionListByGroupNum(RegionClass.GroupName(n))
                    ' Ask before killing any people
                    If RegionClass.AvatarCount(num) > 0 Then
                        Dim response As MsgBoxResult
                        If RegionClass.AvatarCount(num) = 1 Then
                            response = MsgBox("There is one avatar in " + RegionClass.RegionName(num) + ".  Do you still want to stop it?", vbYesNo)
                        Else
                            response = MsgBox("There are " + RegionClass.AvatarCount(num).ToString(Form1.usa) + " avatars in " + RegionClass.RegionName(num) + ".  Do you still want to stop it?", vbYesNo)
                        End If
                        If response = vbNo Then
                            StopIt = False
                        End If
                    End If
                Next

                If (StopIt) Then
                    Dim regionNum = RegionClass.FindRegionByName(RegionName)
                    Dim h As IntPtr = Form1.GetHwnd(RegionClass.GroupName(n))
                    If Form1.ShowDOSWindow(hwnd, Form1.SHOWWINDOWENUM.SWRESTORE) Then
                        Form1.SequentialPause()
                        Form1.ConsoleCommand(RegionClass.GroupName(regionNum), "q{ENTER}" + vbCrLf)
                        Form1.Print("Stopping " + RegionClass.GroupName(regionNum))
                        ' shut down all regions in the DOS box
                        For Each regionNum In RegionClass.RegionListByGroupNum(RegionClass.GroupName(regionNum))
                            RegionClass.Timer(regionNum) = RegionMaker.REGIONTIMER.Stopped
                            RegionClass.Status(regionNum) = RegionMaker.SIMSTATUSENUM.ShuttingDown ' request a recycle.
                        Next
                    Else
                        ' shut down all regions in the DOS box
                        For Each regionNum In RegionClass.RegionListByGroupNum(RegionClass.GroupName(regionNum))
                            RegionClass.Timer(regionNum) = RegionMaker.REGIONTIMER.Stopped
                            RegionClass.Status(regionNum) = RegionMaker.SIMSTATUSENUM.Stopped ' already shutting down
                        Next
                    End If

                    UpdateView = True ' make form refresh
                End If

                UpdateView = True ' make form refresh

            ElseIf chosen = "Edit" Then

                Dim RegionForm As New FormRegion
                RegionForm.Init(RegionClass.RegionName(n))
                RegionForm.Activate()
                RegionForm.Visible = True
                RegionForm.Select()
                ' UpdateView = True ' make form refresh

            ElseIf chosen = "Recycle" Then

                'Dim h As IntPtr = Form1.GetHwnd(RegionClass.GroupName(n))
                Form1.SequentialPause()
                Form1.ConsoleCommand(RegionClass.GroupName(n), "q{ENTER}" + vbCrLf)
                Form1.Print("Recycle " + RegionClass.GroupName(n))
                Form1.GRestartNow = True

                ' shut down all regions in the DOS box

                For Each RegionNum In RegionClass.RegionListByGroupNum(RegionClass.GroupName(n))
                    RegionClass.Timer(RegionNum) = RegionMaker.REGIONTIMER.Stopped
                    RegionClass.Status(RegionNum) = RegionMaker.SIMSTATUSENUM.RecyclingDown ' request a recycle.
                Next
                UpdateView = True ' make form refresh

            End If

            If chosen.Length > 0 Then
                Choices.Dispose()
            End If
        Catch

        End Try

    End Sub

    Private Sub StopRegionNum(num As Integer)
        Form1.SequentialPause()
        Dim hwnd = Form1.GetHwnd(RegionClass.GroupName(num))
        Form1.Log("Region", "Stopping Region " + RegionClass.GroupName(num))
        Form1.ConsoleCommand(RegionClass.GroupName(num), "q{ENTER}" + vbCrLf)
    End Sub

    Private Sub ListView1_ItemCheck1(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles ListView1.ItemCheck

        Dim Item As ListViewItem = ListView1.Items.Item(e.Index)
        Dim n As Integer = RegionClass.FindRegionByName(Item.Text)
        If n = -1 Then Return
        Dim GroupName = RegionClass.GroupName(n)
        For Each X In RegionClass.RegionListByGroupNum(GroupName)
            If ViewNotBusy Then
                If (e.CurrentValue = CheckState.Unchecked) Then
                    RegionClass.RegionEnabled(X) = True
                    ' and region file on disk
                    Form1.MySetting.LoadOtherIni(RegionClass.RegionPath(X), ";")
                    Form1.MySetting.SetOtherIni(RegionClass.RegionName(X), "Enabled", "true")
                    Form1.MySetting.SaveOtherINI()
                ElseIf (e.CurrentValue = CheckState.Checked) Then
                    RegionClass.RegionEnabled(X) = False
                    ' and region file on disk
                    Form1.MySetting.LoadOtherIni(RegionClass.RegionPath(X), ";")
                    Form1.MySetting.SetOtherIni(RegionClass.RegionName(X), "Enabled", "false")
                    Form1.MySetting.SaveOtherINI()
                End If
            End If
        Next

        UpdateView() = True ' force a refresh

    End Sub

    ' ColumnClick event handler.
    Private Sub ColumnClick(ByVal o As Object, ByVal e As ColumnClickEventArgs)

        ListView1.SuspendLayout()
        Me.ListView1.Sorting = SortOrder.None

        ' Set the ListViewItemSorter property to a new ListViewItemComparer
        ' object. Setting this property immediately sorts the
        ' ListView using the ListViewItemComparer object.
        Me.ListView1.ListViewItemSorter = New ListViewItemComparer(e.Column)

        ListView1.ResumeLayout()

    End Sub

    Private Sub Addregion_Click(sender As Object, e As EventArgs) Handles Addregion.Click

        Dim RegionForm As New FormRegion
        RegionForm.Init("")
        RegionForm.Activate()
        RegionForm.Visible = True

    End Sub

#End Region

#Region "DragDrop"

    Private Sub ListView1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles ListView1.DragEnter

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If

    End Sub

    Private Sub ListView1_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles ListView1.DragDrop

        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())

        Dim dirpathname = PickGroup()
        If dirpathname.Length = 0 Then
            Form1.Print("Aborted")
            Return
        End If

        For Each pathname As String In files
            pathname = pathname.Replace("\", "/")
            Dim extension As String = Path.GetExtension(pathname)
            extension = Mid(extension, 2, 5)
            If extension.ToLower(Form1.usa) = "ini" Then
                Dim filename = Path.GetFileNameWithoutExtension(pathname)
                Dim i = RegionClass.FindRegionByName(filename)
                If i >= 0 Then
                    MsgBox("Region name " + filename + " already exists", vbInformation, "Info")
                    Return
                End If

                If dirpathname.Length = 0 Then dirpathname = filename

                Dim NewFilepath = Form1.GOpensimBinPath & "bin\Regions\" + dirpathname + "\Region\"
                If Not Directory.Exists(NewFilepath) Then
                    Directory.CreateDirectory(Form1.GOpensimBinPath & "bin\Regions\" + dirpathname + "\Region")
                End If

                File.Copy(pathname, Form1.GOpensimBinPath & "bin\Regions\" + dirpathname + "\Region\" + filename + ".ini")
            Else
                Form1.Print("Unrecognized file type" + extension + ". Drag and drop any Region.ini files to add them to the system.")
            End If
        Next
        RegionClass.GetAllRegions()
        LoadMyListView()

    End Sub

    Private Function PickGroup() As String

        Dim Chooseform As New Choice ' form for choosing a set of regions
        ' Show testDialog as a modal dialog and determine if DialogResult = OK.

        Chooseform.FillGrid("Group")

        Dim chosen As String
        Chooseform.ShowDialog()
        Try
            ' Read the chosen GROUP name
            chosen = Chooseform.DataGridView.CurrentCell.Value.ToString()
            If chosen.Length > 0 Then
                Chooseform.Dispose()
            End If
        Catch ex As Exception
            chosen = ""
        End Try
        Return chosen

    End Function

    Private Sub AllNone_CheckedChanged(sender As Object, e As EventArgs) Handles AllNome.CheckedChanged

        For Each X As ListViewItem In ListView1.Items
            If ItemsAreChecked Then
                X.Checked = CType(CheckState.Unchecked, Boolean)
            Else
                X.Checked = CType(CheckState.Checked, Boolean)
            End If
        Next

        If ItemsAreChecked Then
            ItemsAreChecked = False
        Else
            ItemsAreChecked = True
        End If
        UpdateView = True ' make form refresh

    End Sub

    Private Sub RunAllButton_Click(sender As Object, e As EventArgs) Handles RunAllButton.Click

        Form1.Startup()

    End Sub

    Private Sub StopAllButton_Click(sender As Object, e As EventArgs) Handles StopAllButton.Click
        Form1.KillAll()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles RestartButton.Click

        For Each X As Integer In RegionClass.RegionNumbers

            If Form1.OpensimIsRunning() _
                And RegionClass.RegionEnabled(X) _
                And Not RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.ShuttingDown _
                And Not RegionClass.Status(X) = RegionMaker.SIMSTATUSENUM.RecyclingDown Then

                Dim hwnd = Form1.GetHwnd(RegionClass.GroupName(X))
                If Form1.ShowDOSWindow(hwnd, Form1.SHOWWINDOWENUM.SWRESTORE) Then
                    Form1.SequentialPause()
                    Form1.ConsoleCommand(RegionClass.GroupName(X), "q{ENTER}" + vbCrLf)
                    Form1.Print("Restarting " & RegionClass.GroupName(X))
                End If

                ' shut down all regions in the DOS box
                For Each Y In RegionClass.RegionListByGroupNum(RegionClass.GroupName(X))
                    RegionClass.Timer(Y) = RegionMaker.REGIONTIMER.Stopped
                    RegionClass.Status(Y) = RegionMaker.SIMSTATUSENUM.RecyclingDown
                Next
                Form1.GRestartNow = True

                UpdateView = True ' make form refresh
            End If
        Next
        UpdateView = True ' make form refresh
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        Form1.Help("RegionList")
    End Sub

    Private Sub DetailsToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Form1.MySetting.RegionListView() = ViewType.Details
        Form1.MySetting.SaveSettings()

        TheView = ViewType.Details
        ListView1.View = View.Details
        ListView1.Show()
        AvatarView.Hide()
        ListView1.CheckBoxes = True
        Timer1.Start()
        LoadMyListView()

    End Sub

    Private Sub SmallListToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Form1.MySetting.RegionListView() = ViewType.Icons
        Form1.MySetting.SaveSettings()
        TheView = ViewType.Icons
        ListView1.View = View.SmallIcon
        ListView1.Show()
        AvatarView.Hide()
        ListView1.CheckBoxes = False
        Timer1.Start()
        LoadMyListView()

    End Sub

    Private Sub MapsToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Form1.MySetting.RegionListView() = ViewType.Maps
        Form1.MySetting.SaveSettings()
        TheView = ViewType.Maps
        ListView1.View = View.LargeIcon
        ListView1.Show()
        AvatarView.Hide()
        ListView1.CheckBoxes = False
        Timer1.Stop()
        LoadMyListView()
    End Sub

    Private Sub AvatarsToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Form1.MySetting.RegionListView() = ViewType.Avatars
        Form1.MySetting.SaveSettings()
        TheView = ViewType.Avatars
        ListView1.View = View.Details
        ListView1.Hide()
        AvatarView.Show()
        LoadMyListView()
        Timer1.Start()
    End Sub

    Private Sub ViewDetail_Click(sender As Object, e As EventArgs) Handles ViewDetail.Click

        Form1.MySetting.RegionListView() = ViewType.Details
        Form1.MySetting.SaveSettings()
        TheView = ViewType.Details
        SetScreen(TheView)
        ListView1.View = View.Details
        ListView1.Show()
        AvatarView.Hide()
        ListView1.CheckBoxes = True
        Timer1.Start()
        LoadMyListView()
    End Sub

    Private Sub ViewCompact_Click(sender As Object, e As EventArgs) Handles ViewCompact.Click

        Form1.MySetting.RegionListView() = ViewType.Icons
        Form1.MySetting.SaveSettings()
        TheView = ViewType.Icons
        SetScreen(TheView)
        ListView1.View = View.SmallIcon
        ListView1.Show()
        AvatarView.Hide()
        ListView1.CheckBoxes = False
        Timer1.Start()
        LoadMyListView()
    End Sub

    Private Sub ViewMaps_Click(sender As Object, e As EventArgs) Handles ViewMaps.Click

        Form1.MySetting.RegionListView() = ViewType.Maps
        Form1.MySetting.SaveSettings()
        TheView = ViewType.Maps
        SetScreen(TheView)
        ListView1.View = View.LargeIcon
        ListView1.Show()
        AvatarView.Hide()
        ListView1.CheckBoxes = False
        Timer1.Stop()
        LoadMyListView()
    End Sub

    Private Sub ViewAvatars_Click(sender As Object, e As EventArgs) Handles ViewAvatars.Click

        Form1.MySetting.RegionListView() = ViewType.Avatars
        Form1.MySetting.SaveSettings()
        TheView = ViewType.Avatars
        SetScreen(TheView)
        ListView1.View = View.Details
        ListView1.Hide()
        AvatarView.Show()
        LoadMyListView()
        Timer1.Start()
    End Sub

#End Region

#Region "Mysql"

    Private Function PeopleInSim() As Integer

        Return 1

    End Function

#End Region

End Class

#Region "Compare"

' Implements the manual sorting of items by columns.
Class ListViewItemComparer
    Implements IComparer
#Disable Warning IDE0044 ' Add readonly modifier
    Private col As Integer
#Enable Warning IDE0044 ' Add readonly modifier

    Public Sub New()
        col = 1
    End Sub

    Public Sub New(ByVal column As Integer)
        col = column
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare

        Return [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)

    End Function

End Class

#End Region