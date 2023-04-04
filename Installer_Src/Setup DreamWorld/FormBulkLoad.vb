#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Option Explicit On

Imports System.Text.RegularExpressions

Public Class FormBulkLoad

    Private _initialized As Boolean
    Private _StopLoading As String = "Stopped"
    Private CoordinateX As Integer
    Private CoordinateY As Integer

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
        'Me.Text = "Form screen position = " & Me.Location.ToString
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

#Region "Properties"

    Public Property Initialized As Boolean
        Get
            Return _initialized
        End Get
        Set(value As Boolean)
            _initialized = value
        End Set
    End Property

    Public Property StopLoading As String
        Get
            Return _StopLoading
        End Get
        Set(value As String)
            _StopLoading = value
        End Set
    End Property

#End Region

#Region "Loading"

    Public Sub StartLoading()

        StopLoading = "Stopped"

        If Settings.OarCount = 0 Then Return ' sanity check  as web server may be gone

        Dim Caution = MsgBox($"{My.Resources.CautionOARs2} {CStr(Settings.OarCount)}", vbYesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Critical, My.Resources.Caution_word)
        If Caution <> MsgBoxResult.Yes Then Return

        gEstateOwner = Settings.SurroundOwner

        If Not PropOpensimIsRunning() Then
            MysqlInterface.DeregisterRegions(False)
        End If

        FormSetup.Buttons(FormSetup.BusyButton)
        If Not StartMySQL() Then Return
        If Not StartRobust() Then Return
        FormSetup.StartTimer()

        If StopLoading = "StopRequested" Then
            ResetRun()
            Return
        End If

        ' setup parameters for the load
        Dim StartX = CoordinateX ' loop begin
        Dim MaxSizeThisRow As Integer  ' the largest region in a row
        Dim SizeRegion As Integer = 1 ' (1X1)

        StopLoading = "Running"
        Dim regionList As New Dictionary(Of String, String)
        Try
            For Each J In FormSetup.ContentOAR.GetJson
                If StopLoading = "StopRequested" Then
                    ResetRun()
                    Return
                End If

                Application.DoEvents()

                ' Get name from web site JSON
                Dim Name = J.Name
                Dim shortname = IO.Path.GetFileNameWithoutExtension(Name)
                Dim Index = shortname.IndexOf("(", StringComparison.OrdinalIgnoreCase)
                If (Index >= 0) Then
                    shortname = shortname.Substring(0, Index)
                End If

                If shortname.Length = 0 Then Return
                If shortname = Settings.WelcomeRegion Then Continue For

                Dim RegionUUID As String

                ' it may already exists
                Dim p = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{shortname}\Region\{shortname}.ini")
                If IO.File.Exists(p) Then
                    ' if so, check that it has prims
                    RegionUUID = FindRegionByName(shortname)

                    Dim o As New Guid
                    If Guid.TryParse(RegionUUID, o) Then
                        Dim Prims = GetPrimCount(RegionUUID)
                        If Prims > 0 Then
                            'TextPrint($"{J.Name} {My.Resources.Ok} ")
                            Continue For
                        Else
                            TextPrint($"{J.Name} needs content")
                            regionList.Add(J.Name, RegionUUID)
                        End If
                    Else
                        BreakPoint.Print("Bad Region UUID " & RegionUUID)
                        ResetRun()
                        Return
                    End If
                Else
                    ' its a new region
                    TextPrint($"{My.Resources.Add_Region_word} {Name} ")

                    If StopLoading = "StopRequested" Then
                        ResetRun()
                        Return
                    End If
                    RegionUUID = CreateRegionStruct(shortname, shortname)

                    ' bump across 50 regions, then move up the Max size of that row +1
                    If SizeRegion > MaxSizeThisRow Then
                        MaxSizeThisRow = SizeRegion ' remember the height
                    End If

                    ' read the size from the file name (1X1), (2X2)
                    Dim pattern1 = New Regex("(.*?)\((\d+)[xX](\d+)\)\.")

                    Dim match1 As Match = pattern1.Match(Name)
                    If match1.Success Then
                        SizeRegion = CInt("0" & match1.Groups(2).Value.Trim)
                        If SizeRegion = 0 Then
                            ErrorLog($"Cannot load OAR - bad size in {J.Name}")
                            ResetRun()
                            Return
                        End If
                    Else
                        ErrorLog($"Cannot load OAR {J.Name}")
                        ResetRun()
                        Return
                    End If

                    Coord_X(RegionUUID) = CoordinateX
                    Coord_Y(RegionUUID) = CoordinateY

                    Smart_Boot_Enabled(RegionUUID) = True
                    Teleport_Sign(RegionUUID) = True

                    SizeX(RegionUUID) = SizeRegion * 256
                    SizeY(RegionUUID) = SizeRegion * 256

                    Group_Name(RegionUUID) = shortname

                    RegionIniFilePath(RegionUUID) = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{shortname}\Region\{shortname}.ini")
                    RegionIniFolderPath(RegionUUID) = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{shortname}\Region")
                    OpensimIniPath(RegionUUID) = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{shortname}")

                    Dim port = GetPort(RegionUUID)
                    GroupPort(RegionUUID) = port
                    Region_Port(RegionUUID) = port
                    WriteRegionObject(shortname, shortname)
                    regionList.Add(J.Name, RegionUUID)
                    If CoordinateX > (StartX + 50) Then   ' if past right border,
                        CoordinateX = StartX              ' go back to left border
                        CoordinateY += MaxSizeThisRow + 1  ' Add the largest size +1 and move up
                        SizeRegion = 256            ' and reset the max height
                    Else     ' we move right the size of the last sim + 1
                        CoordinateX += SizeRegion + 1
                    End If

                End If
                If StopLoading = "StopRequested" Then
                    ResetRun()
                    Return
                End If
            Next
        Catch ex As Exception
            ResetRun()
            BreakPoint.Print(ex.Message)
        End Try

        Dim keys As List(Of String) = regionList.Keys.ToList

        ' Sort the keys.
        keys.Sort()

        Dim regionList2 As New Dictionary(Of String, String)
        ' Loop over the sorted keys.
        For Each str As String In keys
            regionList2.Add(str, regionList.Item(str))
        Next

        PropUpdateView = True ' make form refresh

        PropChangedRegionSettings = True
        GetAllRegions(True)

        Firewall.SetFirewall()

        Try
            For Each line In regionList2
                Application.DoEvents()

                If StopLoading = "StopRequested" Then
                    ResetRun()
                    Return
                End If

                If Not PropOpensimIsRunning Then
                    ResetRun()
                    Return
                End If

                Dim Region_Name = line.Key
                Dim RegionUUID = line.Value

                If RegionEnabled(RegionUUID) Then
                    TextPrint($"{My.Resources.Start_word} {Region_Name}")

                    Dim File = $"{PropHttpsDomain}/Outworldz_Installer/OAR/{Region_Name}"

                    License(File, RegionUUID)

                    Dim obj As New TaskObject With {
                        .TaskName = TaskName.LoadAllFreeOARs,
                        .Command = File
                    }

                    RebootAndRunTask(RegionUUID, obj)
                    AddToRegionMap(RegionUUID)
                    SequentialPause()

                End If

                If StopLoading = "StopRequested" Then
                    ResetRun()
                    Return
                End If

            Next
        Catch ex As Exception
            BreakPoint.Print(ex.Message)
        End Try
        StopLoading = "Stopped"
        ResetRun()

    End Sub

    Private Sub ResetRun()

        FormSetup.Buttons(FormSetup.StopButton)
        StopLoading = "Stopped"

    End Sub

#End Region

#Region "Form_boxes"

    Private Sub AviName_TextChanged(sender As Object, e As EventArgs) Handles RegionOwnerTextBox.TextChanged

        If Not Initialized Then Return

        Settings.BulkLoadOwner = RegionOwnerTextBox.Text

        If RegionOwnerTextBox.Text.Length = 0 Then
            RegionOwnerTextBox.BackColor = Color.Red
            Return
        End If

        If IsMySqlRunning() Then
            Dim AvatarUUID As String = ""
            Try
                AvatarUUID = GetAviUUUD(RegionOwnerTextBox.Text)
            Catch
            End Try
            If AvatarUUID.Length > 0 Then
                RegionOwnerTextBox.BackColor = Color.FromName("Window")
            End If
        End If

        Settings.SaveSettings()

    End Sub

    Private Sub TextBoxX_TextChanged(sender As Object, e As EventArgs) Handles CoordX.TextChanged

        If Not Integer.TryParse(CoordX.Text, CoordinateX) Then
            MsgBox(My.Resources.BadCoordinates, MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
        End If

    End Sub

    Private Sub TextBoxY_TextChanged(sender As Object, e As EventArgs) Handles CoordY.TextChanged

        If Not Integer.TryParse(CoordY.Text, CoordinateY) Then
            MsgBox(My.Resources.BadCoordinates, MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
        End If

    End Sub

#End Region

    Private Sub BulkLoadButton_Click(sender As Object, e As EventArgs) Handles BulkLoadButton.Click

        If Settings.EstateName.Length = 0 Then
            MsgBox(My.Resources.TryAgain)
            Return
        End If

        If Settings.BulkLoadOwner.Length = 0 Then
            MsgBox(My.Resources.TryAgain)
            Return
        End If

        StartBulkLoading()

    End Sub

    Private Sub EstateName_TextChanged(sender As Object, e As EventArgs) Handles EstateName.TextChanged

        Settings.EstateName = EstateName.Text
        Settings.SaveSettings()

    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        If StopLoading = "Running" Then
            If (MsgBox("Abort Loading?", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, "") = DialogResult.No) Then
                e.Cancel = True
            End If
        End If

        Settings.SaveSettings()

    End Sub

    Private Sub FormBulkLoad_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        My.Application.ChangeUICulture(Settings.Language)
        My.Application.ChangeCulture(Settings.Language)

        SetScreen()

        OwnerLabel.Text = My.Resources.OwnerofNewRegions
        RegionOwnerTextBox.Text = Settings.BulkLoadOwner
        Estatenamelabel.Text = My.Resources.WhatEstate

        If RegionOwnerTextBox.Text.Length = 0 Then
            RegionOwnerTextBox.BackColor = Color.Red
        End If
        With RegionOwnerTextBox
            .AutoCompleteCustomSource = MysqlInterface.GetAvatarList()
            .AutoCompleteMode = AutoCompleteMode.Suggest
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With

        StartingLabel.Text = My.Resources.StartingLocation

        CoordX.Text = CStr(LargestX())
        CoordY.Text = CStr(LargestY() + 18)

        HelpOnce("Bulk Region Loader")

        Initialized = True

    End Sub

    Private Sub RegionsToolStripMenuItem_Click(sender As Object, e As EventArgs)

        HelpManual("Bulk Load")

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")>
    Private Sub StartBulkLoading()

        If BulkLoadButton.Text = My.Resources.Abort Then
            BulkLoadButton.Text = My.Resources.Aborted_word
            StopLoading = "StopRequested"
            TextPrint(My.Resources.Aborting)
            Sleep(1000)
            BulkLoadButton.Text = My.Resources.BulkLoad
            Me.Close()
        End If

        BulkLoadButton.Text = My.Resources.Abort

        If CoordinateX <= 1 Or CoordinateY < 32 Then
            MsgBox($"{My.Resources.BadCoordinates} : X >= 1 And Y > 32", MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
            TextPrint(My.Resources.Aborting)
            Sleep(1000)
            BulkLoadButton.Text = My.Resources.BulkLoad
            Return
        End If

        StartLoading()

        If BulkLoadButton.Text = My.Resources.Aborting Then
            TextPrint(My.Resources.Aborted_word)
            Sleep(1000)
            Close()
        End If

        BulkLoadButton.Text = My.Resources.BulkLoad
        TextPrint("Stopped")

    End Sub

    Private Sub ToolStripLabel1_Click(sender As Object, e As EventArgs) Handles ToolStripLabel1.Click

        HelpManual("Bulk Load")

    End Sub

End Class
