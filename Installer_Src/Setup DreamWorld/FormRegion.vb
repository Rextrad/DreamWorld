#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.ComponentModel
Imports System.IO
Imports System.Text.RegularExpressions

Public Class FormRegion

#Region "Declarations"

    Dim _RegionUUID As String = ""
    Dim BoxSize As Integer = 256
    Dim changed As Boolean
    Dim initted As Boolean

    ' needed a flag to see if we are initted as the dialogs change on start. true if we need to save a form
    Dim isNew As Boolean

    Dim oldname As String = ""
    Dim RName As String

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        Advanced.Text = Global.Outworldz.My.Resources.Regions_word
        AllowGods.Text = Global.Outworldz.My.Resources.Allow_Or_Disallow_Gods_word
        BirdsCheckBox.Text = Global.Outworldz.My.Resources.Bird_Module_word
        ClampPrimLabel.Text = Global.Outworldz.My.Resources.Clamp_Prim_Size_word
        ConciergeCheckBox.Text = Global.Outworldz.My.Resources.Announce_visitors
        DeleteButton.Text = Global.Outworldz.My.Resources.Delete_word
        DeregisterButton.Text = Global.Outworldz.My.Resources.Deregister_word
        DisableGBCheckBox.Text = Global.Outworldz.My.Resources.Disable_Gloebits_word
        DisallowForeigners.Text = Global.Outworldz.My.Resources.Disable_Foreigners_word
        DisallowResidents.Text = Global.Outworldz.My.Resources.Disable_Residents
        EnabledCheckBox.Text = Global.Outworldz.My.Resources.Enabled_word
        FrameRateLabel.Text = Global.Outworldz.My.Resources.FrameRate
        Gods_Use_Default.Text = Global.Outworldz.My.Resources.Use_Default_word
        GroupBox1.Text = Global.Outworldz.My.Resources.Physics_word
        GroupBox2.Text = Global.Outworldz.My.Resources.Sim_Size_word
        GroupBox3.Text = My.Resources.Publicity_Word
        GroupBox4.Text = Global.Outworldz.My.Resources.Permissions_word
        GroupBox6.Text = Global.Outworldz.My.Resources.Region_Specific_Settings_word
        GroupBox7.Text = Global.Outworldz.My.Resources.Modules_word
        GroupBox8.Text = Global.Outworldz.My.Resources.Script_Engine_word  '
        Label13.Text = Global.Outworldz.My.Resources.Region_Specific_Settings_word
        Label16.Text = Global.Outworldz.My.Resources.Region_Port_word
        Label4.Text = Global.Outworldz.My.Resources.Maps_X
        ManagerGod.Text = Global.Outworldz.My.Resources.EstateManagerIsGod_word
        MapBest.Text = Global.Outworldz.My.Resources.Best_Prims
        MapBetter.Text = Global.Outworldz.My.Resources.Better_Prims
        MapBox.Text = Global.Outworldz.My.Resources.Maps_word
        MapGood.Text = Global.Outworldz.My.Resources.Good_Warp3D_word
        MapNone.Text = Global.Outworldz.My.Resources.None
        Maps_Use_Default.Text = Global.Outworldz.My.Resources.Use_Default_word
        MapSimple.Text = Global.Outworldz.My.Resources.Simple_but_Fast_word
        MaxMAvatarsLabel.Text = Global.Outworldz.My.Resources.Max_Avatars
        MaxNPrimsLabel.Text = Global.Outworldz.My.Resources.Max_NumPrims
        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        NonPhysPrimLabel.Text = Global.Outworldz.My.Resources.Nonphysical_Prim
        NoPublish.Text = Global.Outworldz.My.Resources.No_Publish_Items
        Physics_Default.Text = Global.Outworldz.My.Resources.Use_Default_word
        Physics_Separate.Text = Global.Outworldz.My.Resources.BP
        Physics_ubODE.Text = Global.Outworldz.My.Resources.UBODE_words
        PhysPrimLabel.Text = Global.Outworldz.My.Resources.Physical_Prim
        Publish.Text = Global.Outworldz.My.Resources.Publish_Items
        PublishDefault.Text = Global.Outworldz.My.Resources.Use_Default_word
        RegionGod.Text = Global.Outworldz.My.Resources.Region_Owner_Is_God_word
        SaveButton.Text = Global.Outworldz.My.Resources.Save_word
        ScriptDefaultButton.Text = Global.Outworldz.My.Resources.Use_Default_word
        ScriptRateLabel.Text = Global.Outworldz.My.Resources.Script_Timer_Rate
        SkipAutoCheckBox.Text = Global.Outworldz.My.Resources.Skip_Autobackup_word
        SmartStartCheckBox.Text = Global.Outworldz.My.Resources.Smart_Start_word
        Text = Global.Outworldz.My.Resources.Regions_word
        TidesCheckbox.Text = Global.Outworldz.My.Resources.Tide_Enable
        ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.question_and_answer
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word
        ToolTip1.SetToolTip(AllowGods, Global.Outworldz.My.Resources.AllowGodsTooltip)
        ToolTip1.SetToolTip(BirdsCheckBox, Global.Outworldz.My.Resources.GBoids)
        ToolTip1.SetToolTip(ClampPrimLabel, Global.Outworldz.My.Resources.ClampSize)
        ToolTip1.SetToolTip(ClampPrimSize, Global.Outworldz.My.Resources.ClampSize)
        ToolTip1.SetToolTip(CoordX, Global.Outworldz.My.Resources.Coordx)
        ToolTip1.SetToolTip(CoordY, Global.Outworldz.My.Resources.CoordY)
        ToolTip1.SetToolTip(DisableGBCheckBox, Global.Outworldz.My.Resources.Disable_Gloebits_text)
        ToolTip1.SetToolTip(DisallowForeigners, Global.Outworldz.My.Resources.No_HG)
        ToolTip1.SetToolTip(DisallowResidents, Global.Outworldz.My.Resources.Only_Owners)
        ToolTip1.SetToolTip(FrameRateLabel, Global.Outworldz.My.Resources.FRText)
        ToolTip1.SetToolTip(FrametimeBox, Global.Outworldz.My.Resources.FrameTime)
        ToolTip1.SetToolTip(GroupBox1, Global.Outworldz.My.Resources.Sim_Rate)
        ToolTip1.SetToolTip(ManagerGod, Global.Outworldz.My.Resources.EMGod)
        ToolTip1.SetToolTip(MaxAgents, Global.Outworldz.My.Resources.Max_Agents)
        ToolTip1.SetToolTip(MaxMAvatarsLabel, Global.Outworldz.My.Resources.Max_Agents)
        ToolTip1.SetToolTip(MaxNPrimsLabel, Global.Outworldz.My.Resources.Viewer_Stops_Counting)
        ToolTip1.SetToolTip(MaxPrims, Global.Outworldz.My.Resources.Not_Normal)
        ToolTip1.SetToolTip(NonphysicalPrimMax, Global.Outworldz.My.Resources.Normal_Prim)
        ToolTip1.SetToolTip(NonPhysPrimLabel, Global.Outworldz.My.Resources.Max_NonPhys)
        ToolTip1.SetToolTip(PhysicalPrimMax, Global.Outworldz.My.Resources.Max_Phys)
        ToolTip1.SetToolTip(PhysPrimLabel, Global.Outworldz.My.Resources.Max_Phys)
        ToolTip1.SetToolTip(RegionGod, Global.Outworldz.My.Resources.Region_Owner_Is_God_word)
        ToolTip1.SetToolTip(RegionName, Global.Outworldz.My.Resources.Region_Name)
        ToolTip1.SetToolTip(ScriptRateLabel, Global.Outworldz.My.Resources.Script_Timer_Text)
        ToolTip1.SetToolTip(ScriptRateLabel, Global.Outworldz.My.Resources.STComment)
        ToolTip1.SetToolTip(ScriptTimerTextBox, Global.Outworldz.My.Resources.STComment)
        ToolTip1.SetToolTip(SkipAutoCheckBox, Global.Outworldz.My.Resources.WillNotSave)
        ToolTip1.SetToolTip(SmartStartCheckBox, Global.Outworldz.My.Resources.GTide)
        ToolTip1.SetToolTip(TidesCheckbox, Global.Outworldz.My.Resources.GTide)
        ToolTip1.SetToolTip(TPCheckBox1, Global.Outworldz.My.Resources.Teleport_Tooltip)
        TPCheckBox1.Text = Global.Outworldz.My.Resources.Teleporter_Enable_word
        UUID.Name = Global.Outworldz.My.Resources.UUID
        XEngineButton.Text = Global.Outworldz.My.Resources.XEngine_word
        YEngineButton.Text = Global.Outworldz.My.Resources.YEngine_word
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Property Changed1 As Boolean
        Get
            Return changed
        End Get
        Set(value As Boolean)
            changed = value
        End Set
    End Property

    Public Property Initted1 As Boolean
        Get
            Return initted
        End Get
        Set(value As Boolean)
            initted = value
        End Set
    End Property

    Public Property IsNew1 As Boolean
        Get
            Return isNew
        End Get
        Set(value As Boolean)
            isNew = value
        End Set
    End Property

    Public Property Oldname1 As String
        Get
            Return oldname
        End Get
        Set(value As String)
            oldname = value
        End Set
    End Property

    Public Property RegionUUID As String
        Get
            Return _RegionUUID
        End Get
        Set(value As String)
            _RegionUUID = value
        End Set
    End Property

    Public Property RName1 As String
        Get
            Return RName
        End Get
        Set(value As String)
            RName = value
        End Set
    End Property

#End Region

#Region "Start_Stop"

    Public Shared Function FilenameIsOK(ByVal fileName As String) As Boolean
        ' check for invalid chars in file name for INI file
        If fileName Is Nothing Then Return False
        Dim value As Boolean = False
        Try
            value = Not fileName.Intersect(Path.GetInvalidFileNameChars()).Any()
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
        End Try

        Return value

    End Function

    Shared Function RegionChosen(regionName As String) As String

        Dim result As DialogResult
        Dim chosen As String

        Using Chooseform As New FormChooser ' form for choosing a set of regions
            ' Show testDialog as a modal dialog and determine if DialogResult = OK.
            Chooseform.FillGrid("Group")  ' populate the grid with either Group or RegionName
            result = Chooseform.ShowDialog()
            If result = DialogResult.Cancel Then
                Return ""
            End If

            Try
                ' Read the chosen sim name
                chosen = Chooseform.DataGridView.CurrentCell.Value.ToString()
                If chosen = "! Add New Name" Then
                    chosen = InputBox(My.Resources.Enter_Dos_Name, "", regionName)
                End If
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
                chosen = ""
            End Try

        End Using
        Return chosen

    End Function

    Public Sub Init(Name As String)

        Me.Focus()

        If Name Is Nothing Then Return
        Name = Name.Trim() ' remove spaces

        PropRegionClass = RegionMaker.Instance()

        ' NEW REGION
        If Name.Length = 0 Then
            IsNew1 = True
            Gods_Use_Default.Checked = True
            RegionName.Text = Global.Outworldz.My.Resources.Name_of_Region_Word
            UUID.Text = Guid.NewGuid().ToString
            ConciergeCheckBox.Checked = False
            CoordX.Text = (PropRegionClass.LargestX() + 4).ToString(Globalization.CultureInfo.InvariantCulture)
            CoordY.Text = (PropRegionClass.LargestY() + 0).ToString(Globalization.CultureInfo.InvariantCulture)
            'RegionPort.Text = CStr(PropRegionClass.LargestPort())
            EnabledCheckBox.Checked = True
            RadioButton1.Checked = True
            SmartStartCheckBox.Checked = False
            NonphysicalPrimMax.Text = 1024.ToString(Globalization.CultureInfo.InvariantCulture)
            PhysicalPrimMax.Text = 64.ToString(Globalization.CultureInfo.InvariantCulture)
            ClampPrimSize.Checked = False
            MaxPrims.Text = 45000.ToString(Globalization.CultureInfo.InvariantCulture)
            MaxAgents.Text = 100.ToString(Globalization.CultureInfo.InvariantCulture)
            RegionUUID = PropRegionClass.CreateRegion("New Region")
            Gods_Use_Default.Checked = True
        Else
            ' OLD REGION EDITED all this is required to be filled in!
            IsNew1 = False
            RegionUUID = PropRegionClass.FindRegionByName(Name)
            Oldname1 = PropRegionClass.RegionName(RegionUUID) ' backup in case of rename
            EnabledCheckBox.Checked = PropRegionClass.RegionEnabled(RegionUUID)
            Me.Text = Name & " " & Global.Outworldz.My.Resources.Region_word ' on screen
            RegionName.Text = Name
            UUID.Text = RegionUUID
            NonphysicalPrimMax.Text = CStr(PropRegionClass.NonPhysicalPrimMax(RegionUUID))
            PhysicalPrimMax.Text = CStr(PropRegionClass.PhysicalPrimMax(RegionUUID))
            ClampPrimSize.Checked = PropRegionClass.ClampPrimSize(RegionUUID)
            MaxPrims.Text = PropRegionClass.MaxPrims(RegionUUID)
            MaxAgents.Text = PropRegionClass.MaxAgents(RegionUUID)
            RegionPort.Text = PropRegionClass.RegionPort(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture)
            ' Size buttons can be zero
            If PropRegionClass.SizeY(RegionUUID) = 0 Or PropRegionClass.SizeX(RegionUUID) = 0 Then
                RadioButton1.Checked = True
                BoxSize = 256
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 Then
                RadioButton1.Checked = True
                BoxSize = 256 * 1
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 2 Then
                RadioButton2.Checked = True
                BoxSize = 256 * 2
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 3 Then
                RadioButton3.Checked = True
                BoxSize = 256 * 3
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 4 Then
                RadioButton4.Checked = True
                BoxSize = 256 * 4
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 5 Then
                RadioButton5.Checked = True
                BoxSize = 256 * 5
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 6 Then
                RadioButton6.Checked = True
                BoxSize = 256 * 6
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 7 Then
                RadioButton7.Checked = True
                BoxSize = 256 * 7
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 8 Then
                RadioButton8.Checked = True
                BoxSize = 256 * 8
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 9 Then
                RadioButton9.Checked = True
                BoxSize = 256 * 9
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 10 Then
                RadioButton10.Checked = True
                BoxSize = 256 * 10
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 11 Then
                RadioButton11.Checked = True
                BoxSize = 256 * 11
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 12 Then
                RadioButton12.Checked = True
                BoxSize = 256 * 12
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 13 Then
                RadioButton13.Checked = True
                BoxSize = 256 * 14
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 14 Then
                RadioButton14.Checked = True
                BoxSize = 256 * 14
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 15 Then
                RadioButton15.Checked = True
                BoxSize = 256 * 15
            ElseIf PropRegionClass.SizeY(RegionUUID) = 256 * 16 Then
                RadioButton16.Checked = True
                BoxSize = 256 * 16
            Else
                RadioButton1.Checked = False
                RadioButton2.Checked = False
                RadioButton3.Checked = False
                RadioButton4.Checked = False
                RadioButton5.Checked = False
                RadioButton6.Checked = False
                RadioButton7.Checked = False
                RadioButton8.Checked = False
                RadioButton9.Checked = False
                RadioButton10.Checked = False
                RadioButton11.Checked = False
                RadioButton12.Checked = False
                RadioButton13.Checked = False
                RadioButton14.Checked = False
                RadioButton15.Checked = False
                RadioButton16.Checked = False
            End If

            ' global coordinates
            If PropRegionClass.CoordX(RegionUUID) <> 0 Then
                CoordX.Text = PropRegionClass.CoordX(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture)
            End If

            If PropRegionClass.CoordY(RegionUUID) <> 0 Then
                CoordY.Text = PropRegionClass.CoordY(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture)
            End If

        End If

        ' The following are all options.
        If PropRegionClass.DisallowResidents(RegionUUID) = "True" Then
            DisallowResidents.Checked = True
        End If

        If PropRegionClass.DisallowForeigners(RegionUUID) = "True" Then
            DisallowForeigners.Checked = True
        End If

        ScriptTimerTextBox.Text = PropRegionClass.MinTimerInterval(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture)
        FrametimeBox.Text = PropRegionClass.FrameTime(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture)

        If PropRegionClass.SkipAutobackup(RegionUUID) = "True" Then
            SkipAutoCheckBox.Checked = True
        End If
        If PropRegionClass.SmartStart(RegionUUID) = "True" Then
            SmartStartCheckBox.Checked = True
        End If

        Select Case PropRegionClass.DisableGloebits(RegionUUID)
            Case ""
                DisableGBCheckBox.Checked = False
            Case "False"
                DisableGBCheckBox.Checked = False
            Case "True"
                DisableGBCheckBox.Checked = True
        End Select

        RName1 = Name

        ''''''''''''''''''''''''''''' DREAMGRID REGION LOAD '''''''''''''''''

        If PropRegionClass.MapType(RegionUUID).Length = 0 Then
            Maps_Use_Default.Checked = True
        ElseIf PropRegionClass.MapType(RegionUUID) = "None" Then
            MapNone.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.blankbox
        ElseIf PropRegionClass.MapType(RegionUUID) = "Simple" Then
            MapSimple.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Simple
        ElseIf PropRegionClass.MapType(RegionUUID) = "Good" Then
            MapGood.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Good
        ElseIf PropRegionClass.MapType(RegionUUID) = "Better" Then
            MapBetter.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Better
        ElseIf PropRegionClass.MapType(RegionUUID) = "Best" Then
            MapBest.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Best
        End If

        Select Case PropRegionClass.Physics(RegionUUID)
            Case "" : Physics_Default.Checked = True
            Case "-1" : Physics_Default.Checked = True
            Case "0" : Physics_Default.Checked = True
            Case "1" : Physics_ubODE.Checked = True
            Case "2" : Physics_Bullet.Checked = True
            Case "3" : Physics_Separate.Checked = True
            Case "4" : Physics_ubODE.Checked = True
            Case "5" : Physics_Hybrid.Checked = True
            Case Else : Physics_Default.Checked = True
        End Select

        If PropRegionClass.GodDefault(RegionUUID) = "True" Then
            AllowGods.Checked = False
            RegionGod.Checked = False
            ManagerGod.Checked = False
            Gods_Use_Default.Checked = True
        Else
            Select Case PropRegionClass.AllowGods(RegionUUID)
                Case ""
                    AllowGods.Checked = False
                Case "False"
                    AllowGods.Checked = False
                    Gods_Use_Default.Checked = False
                Case "True"
                    AllowGods.Checked = True
                    Gods_Use_Default.Checked = False
            End Select

            Select Case PropRegionClass.RegionGod(RegionUUID)
                Case ""
                    RegionGod.Checked = False
                Case "False"
                    RegionGod.Checked = False
                    Gods_Use_Default.Checked = False
                Case "True"
                    RegionGod.Checked = True
                    Gods_Use_Default.Checked = False
            End Select

            Select Case PropRegionClass.ManagerGod(RegionUUID)
                Case ""
                    ManagerGod.Checked = False
                Case "False"
                    ManagerGod.Checked = False
                    Gods_Use_Default.Checked = False
                Case "True"
                    ManagerGod.Checked = True
                    Gods_Use_Default.Checked = False
            End Select

            ' if none selected, turn default on. This updates old code to new GodDefault global

            If PropRegionClass.AllowGods(RegionUUID).Length = 0 And
                 PropRegionClass.RegionGod(RegionUUID).Length = 0 And
                PropRegionClass.ManagerGod(RegionUUID).Length = 0 Then
                Gods_Use_Default.Checked = True
            End If
        End If

        Select Case PropRegionClass.RegionSnapShot(RegionUUID)
            Case ""
                PublishDefault.Checked = True
                NoPublish.Checked = False
                Publish.Checked = False
            Case "False"
                PublishDefault.Checked = False
                NoPublish.Checked = True
                Publish.Checked = False
            Case "True"
                PublishDefault.Checked = False
                NoPublish.Checked = False
                Publish.Checked = True
        End Select

        Select Case PropRegionClass.Birds(RegionUUID)
            Case ""
                BirdsCheckBox.Checked = False
            Case "False"
                BirdsCheckBox.Checked = False
            Case "True"
                BirdsCheckBox.Checked = True
        End Select

        Select Case PropRegionClass.Tides(RegionUUID)
            Case ""
                TidesCheckbox.Checked = False
            Case "False"
                TidesCheckbox.Checked = False
            Case "True"
                TidesCheckbox.Checked = True
        End Select

        Select Case PropRegionClass.Teleport(RegionUUID)
            Case ""
                TPCheckBox1.Checked = False
            Case "False"
                TPCheckBox1.Checked = False
            Case "True"
                TPCheckBox1.Checked = True
        End Select

        Select Case PropRegionClass.DisallowForeigners(RegionUUID)
            Case ""
                DisallowForeigners.Checked = False
            Case "False"
                DisallowForeigners.Checked = False
            Case "True"
                DisallowForeigners.Checked = True
        End Select

        Select Case PropRegionClass.DisallowResidents(RegionUUID)
            Case ""
                DisallowResidents.Checked = False
            Case "False"
                DisallowResidents.Checked = False
            Case "True"
                DisallowResidents.Checked = True
        End Select

        Select Case PropRegionClass.ScriptEngine(RegionUUID)
            Case ""
                ScriptDefaultButton.Checked = True
            Case "XEngine"
                XEngineButton.Checked = True
            Case "YEngine"
                YEngineButton.Checked = True
        End Select

        Select Case PropRegionClass.Concierge(RegionUUID)
            Case ""
                ConciergeCheckBox.Checked = False
            Case "True"
                ConciergeCheckBox.Checked = True
            Case "False"
                ConciergeCheckBox.Checked = False
        End Select

        Try
            Me.Show() ' time to show the results
            Me.Activate()

            Initted1 = True
            HelpOnce("Region")
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
        End Try

    End Sub

    Private Sub BirdsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles BirdsCheckBox.CheckedChanged

        If BirdsCheckBox.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " has birds enabled")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        FormSetup.PropChangedRegionSettings = True
        Dim message = RegionValidate()
        If Len(message) > 0 Then
            Dim v = MsgBox(message + vbCrLf + Global.Outworldz.My.Resources.Discard_Exit, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
            If v = vbYes Then
                Changed1 = False
            End If
        Else
            WriteRegion(RegionUUID)
            Firewall.SetFirewall()
            PropUpdateView() = True
            Changed1 = False
        End If
        Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles DeregisterButton.Click

        Dim response As MsgBoxResult = MsgBox(My.Resources.Another_Region, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
        If response = vbYes Then

            StartMySQL()
            StartRobust()

            Dim RegionUUID As String = PropRegionClass.FindRegionByName(RegionName.Text)
            If RegionUUID.Length > 0 Then
                If CheckPort(Settings.LANIP(), PropRegionClass.GroupPort(RegionUUID)) Then
                    ShutDown(RegionUUID)
                End If
                Dim loopctr = 120 ' wait 2 minutes
                While CheckPort(Settings.LANIP(), PropRegionClass.GroupPort(RegionUUID)) And loopctr > 0
                    Sleep(1000)
                    loopctr -= 1
                End While

                If loopctr > 0 Then
                    ConsoleCommand(RobustName(), "deregister region id " + RegionUUID)
                    TextPrint(My.Resources.Region_Removed)
                End If
            End If

        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles DisallowForeigners.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub CheckBox1_CheckedChanged_1(sender As Object, e As EventArgs) Handles DisallowResidents.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub ClampPrimSize_CheckedChanged(sender As Object, e As EventArgs) Handles ClampPrimSize.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub CoordX_TextChanged(sender As Object, e As EventArgs) Handles CoordX.TextChanged

        If Not initted Then Return
        Dim digitsOnly As Regex = New Regex("[^\d]")
        CoordX.Text = digitsOnly.Replace(CoordX.Text, "")
        Changed1 = True

    End Sub

    Private Sub Coordy_TextChanged(sender As Object, e As EventArgs) Handles CoordY.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d]")
        CoordY.Text = digitsOnly.Replace(CoordY.Text, "")
        If Initted1 And CoordY.Text.Length >= 0 Then
            Changed1 = True
        End If

    End Sub

    Private Sub DatabaseSetupToolStripMenuItem_Click(sender As Object, e As EventArgs)
        HelpManual("Region")
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        Dim msg = MsgBox(My.Resources.Are_you_Sure_Delete_Region, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
        If msg = vbYes Then
            DeleteFile(Settings.OpensimBinPath & "Regions\" + RegionName.Text + "\Region\" + RegionName.Text + ".bak")
            Try
                My.Computer.FileSystem.RenameFile(PropRegionClass.RegionIniFilePath(RegionUUID), RegionName.Text + ".bak")
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
            End Try
        End If

        If IsRobustRunning() Then
            ConsoleCommand(RobustName(), "deregister region id " + RegionUUID)
        End If
        PropRegionClass.DeleteRegion(RegionUUID)
        PropRegionClass.GetAllRegions()
        Changed1 = False
        PropUpdateView = True

        Me.Close()

    End Sub

    Private Sub EnabledCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles EnabledCheckBox.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub EnableMaxPrims_text(sender As Object, e As EventArgs) Handles MaxPrims.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d]")
        MaxPrims.Text = digitsOnly.Replace(MaxPrims.Text, "")
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub FormRegion_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        If Changed1 Then
            FormSetup.PropChangedRegionSettings = True
            Dim v = MsgBox(My.Resources.Save_changes_word, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Save_changes_word)
            If v = vbYes Then
                Dim message = RegionValidate()
                If Len(message) > 0 Then
                    v = MsgBox(message + vbCrLf + Global.Outworldz.My.Resources.Discard_Exit, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                    If v = vbYes Then
                        Me.Close()
                    End If
                Else
                    WriteRegion(RegionUUID)
                    Firewall.SetFirewall()
                    PropUpdateView() = True
                    Changed1 = False
                End If
            End If
        End If

    End Sub

#End Region

#Region "Private Methods"

    Private Sub GodHelp_Click(sender As Object, e As EventArgs)
        HelpManual("Permissions")
    End Sub

    Private Sub MapBest_CheckedChanged(sender As Object, e As EventArgs) Handles MapBest.CheckedChanged

        If MapBest.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Map Is set to Best")
            MapPicture.Image = Global.Outworldz.My.Resources.Best
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub MapBetter_CheckedChanged(sender As Object, e As EventArgs) Handles MapBetter.CheckedChanged

        If MapBetter.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Map Is set to Better")
            MapPicture.Image = Global.Outworldz.My.Resources.Better
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub MapGood_CheckedChanged(sender As Object, e As EventArgs) Handles MapGood.CheckedChanged

        If MapGood.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Map Is set to Good")
            MapPicture.Image = Global.Outworldz.My.Resources.Good
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub MapNone_CheckedChanged(sender As Object, e As EventArgs) Handles MapNone.CheckedChanged

        If MapNone.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Map Is set to None")
            MapPicture.Image = Global.Outworldz.My.Resources.blankbox
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub Maps_Use_Default_changed(sender As Object, e As EventArgs) Handles Maps_Use_Default.CheckedChanged

        If Maps_Use_Default.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Map Is set to Default")
            MapNone.Checked = False
            MapSimple.Checked = False
            MapGood.Checked = False
            MapBetter.Checked = False
            MapBest.Checked = False

            If Settings.MapType = "None" Then
                MapPicture.Image = Global.Outworldz.My.Resources.blankbox
            ElseIf Settings.MapType = "Simple" Then
                MapPicture.Image = Global.Outworldz.My.Resources.Simple
            ElseIf Settings.MapType = "Good" Then
                MapPicture.Image = Global.Outworldz.My.Resources.Good
            ElseIf Settings.MapType = "Better" Then
                MapPicture.Image = Global.Outworldz.My.Resources.Better
            ElseIf Settings.MapType = "Best" Then
                Settings.MapType = "Best"
            End If
        End If

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub MapSimple_CheckedChanged(sender As Object, e As EventArgs) Handles MapSimple.CheckedChanged

        If MapSimple.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Map Is set to Simple")
            MapPicture.Image = Global.Outworldz.My.Resources.Simple
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub MaxAgents_TextChanged(sender As Object, e As EventArgs) Handles MaxAgents.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d]")
        MaxAgents.Text = digitsOnly.Replace(MaxAgents.Text, "")
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub NonphysicalPrimMax_TextChanged(sender As Object, e As EventArgs) Handles NonphysicalPrimMax.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d]")
        NonphysicalPrimMax.Text = digitsOnly.Replace(NonphysicalPrimMax.Text, "")
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub NoPublish_CheckedChanged(sender As Object, e As EventArgs) Handles NoPublish.CheckedChanged

        If NoPublish.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Is Not set to publish snapshots")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub PhysicalPrimMax_TextChanged(sender As Object, e As EventArgs) Handles PhysicalPrimMax.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d]")
        PhysicalPrimMax.Text = digitsOnly.Replace(PhysicalPrimMax.Text, "")
        If Initted1 Then Changed1 = True

    End Sub

#End Region

#Region "Physics"

    Private Sub Bullet_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Bullet.CheckedChanged

        If Physics_Bullet.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Physics is set to Bullet")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub Physics_Default_CheckedChanged1(sender As Object, e As EventArgs) Handles Physics_Default.CheckedChanged

        If Physics_Default.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Physics Is set to default")
            Physics_ubODE.Checked = False
            Physics_Separate.Checked = False
        End If

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub PhysicsSeparate_CheckedChanged1(sender As Object, e As EventArgs) Handles Physics_Separate.CheckedChanged

        If Physics_Separate.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Physics is set to Bullet in a Thread")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub PhysicsubODE_CheckedChanged1(sender As Object, e As EventArgs) Handles Physics_ubODE.CheckedChanged

        If Physics_ubODE.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Physics is set to UbitODE")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub Publish_CheckedChanged(sender As Object, e As EventArgs) Handles Publish.CheckedChanged

        If Publish.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " is publishing snapshots")
        Else
            Log(My.Resources.Info_word, "Region " + Name + " is not publishing snapshots")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub PublishDefault_CheckedChanged(sender As Object, e As EventArgs) Handles PublishDefault.CheckedChanged

        If PublishDefault.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " is set to default for snapshots")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub RChanged(sender As Object, e As EventArgs) Handles RegionName.TextChanged
        If Len(RegionName.Text) > 0 And Initted1 Then
            If Not FilenameIsOK(RegionName.Text) Then
                MsgBox(My.Resources.Region_Names_Special & " < > : """" / \ | ? *", MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                Return
            End If
            If Initted1 Then Changed1 = True
        End If
    End Sub

    Private Function RegionValidate() As String

        Dim Message As String

        If Len(RegionName.Text) = 0 Then
            Message = Global.Outworldz.My.Resources.Region_name_must_not_be_blank_word
            Return Message
        End If

        ' UUID
        Dim result As Guid
        If Not Guid.TryParse(UUID.Text, result) Then
            Message = Global.Outworldz.My.Resources.Region_UUID_Is_invalid_word & " " & UUID.Text
            Return Message
        End If

        ' global coords
        If Convert.ToInt32("0" & CoordX.Text, Globalization.CultureInfo.InvariantCulture) < 0 Then
            Message = Global.Outworldz.My.Resources.Region_Coordinate_X_cannot_be_less_than_0_word
            Return Message
        ElseIf Convert.ToInt32("0" & CoordX.Text, Globalization.CultureInfo.InvariantCulture) > 65536 Then
            Message = Global.Outworldz.My.Resources.Region_Coordinate_X_is_too_large
            Return Message
        End If

        If Convert.ToInt32("0" & CoordY.Text, Globalization.CultureInfo.InvariantCulture) < 32 Then
            Message = Global.Outworldz.My.Resources.Region_Coordinate_Y_cannot_be_less_than_32
            Return Message
        ElseIf Convert.ToInt32("0" & CoordY.Text, Globalization.CultureInfo.InvariantCulture) > 65535 Then
            Message = Global.Outworldz.My.Resources.Region_Coordinate_Y_Is_too_large
            Return Message
        End If

        Dim aresult As Guid
        If Not Guid.TryParse(UUID.Text, aresult) Then
            Message = Global.Outworldz.My.Resources.UUID0
            Return Message
        End If

        Try
            If (NonphysicalPrimMax.Text.Length = 0) Or (CType(NonphysicalPrimMax.Text, Integer) <= 0) Then
                Message = Global.Outworldz.My.Resources.NVNonPhysPrim
                Return Message
            End If
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
            Message = Global.Outworldz.My.Resources.NVNonPhysPrim
            Return Message
        End Try

        Try
            If (PhysicalPrimMax.Text.Length = 0) Or (CType(PhysicalPrimMax.Text, Integer) <= 0) Then
                Message = Global.Outworldz.My.Resources.NVPhysPrim
                Return Message
            End If
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
            Message = Global.Outworldz.My.Resources.NVPhysPrim
            Return Message
        End Try

        Try
            If (MaxPrims.Text.Length = 0) Or (CType(MaxPrims.Text, Integer) <= 0) Then
                Message = Global.Outworldz.My.Resources.NVMaxPrim
                Return Message
            End If
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
            Message = Global.Outworldz.My.Resources.NVMaxPrim
            Return Message
        End Try
        Try
            If (MaxAgents.Text.Length = 0) Or (CType(MaxAgents.Text, Integer) <= 0) Then
                Message = Global.Outworldz.My.Resources.NVMaxAgents
                Return Message
            End If
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
            Message = Global.Outworldz.My.Resources.NVMaxAgents
            Return Message
        End Try
        Return ""
    End Function

    Private Sub RLost(sender As Object, e As EventArgs) Handles RegionName.LostFocus
        RegionName.Text = RegionName.Text.Trim() ' remove spaces
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub ScriptTimerTextBox_focusChanged(sender As Object, e As EventArgs) Handles ScriptTimerTextBox.LostFocus

        Try
            Dim value = Convert.ToDouble(ScriptTimerTextBox.Text, Globalization.CultureInfo.InvariantCulture)
            If value < 0.01 Then ScriptTimerTextBox.Text = ""
        Catch ex As Exception
            ScriptTimerTextBox.Text = ""
        End Try

        If Initted1 Then Changed1 = True
    End Sub

    Private Sub ScriptTimerTextBox_TextChanged(sender As Object, e As EventArgs) Handles ScriptTimerTextBox.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d\.]")
        ScriptTimerTextBox.Text = digitsOnly.Replace(ScriptTimerTextBox.Text, "")

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub SkipAutoCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SkipAutoCheckBox.CheckedChanged

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub SmartStartCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SmartStartCheckBox.CheckedChanged

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub StopHGCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DisableGBCheckBox.CheckedChanged

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub TextBox1_FocusChanged(sender As Object, e As EventArgs) Handles FrametimeBox.LostFocus

        Try
            Dim value = Convert.ToDouble(FrametimeBox.Text, Globalization.CultureInfo.InvariantCulture)
            If value < 0.01 Then FrametimeBox.Text = ""
        Catch ex As Exception
            FrametimeBox.Text = ""
        End Try

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles FrametimeBox.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d\.]")
        FrametimeBox.Text = digitsOnly.Replace(FrametimeBox.Text, "")
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub TidesCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles TidesCheckbox.CheckedChanged

        If TidesCheckbox.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " has tides enabled")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click

        HelpManual("Region")

    End Sub

    Private Sub TPCheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles TPCheckBox1.CheckedChanged

        If TPCheckBox1.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " has Teleport Board enabled")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub UUID_LostFocus(sender As Object, e As EventArgs) Handles UUID.LostFocus

        If UUID.Text <> UUID.Text And Initted1 Then
            Dim resp = MsgBox(My.Resources.Change_UUID, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
            If resp = vbYes Then
                Changed1 = True
                Dim result As Guid
                If Guid.TryParse(UUID.Text, result) Then
                Else
                    Dim ok = MsgBox(My.Resources.NotValidUUID, MsgBoxStyle.OkCancel Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                    If ok = vbOK Then
                        UUID.Text = System.Guid.NewGuid.ToString
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub UUID_TextChanged(sender As Object, e As EventArgs) Handles UUID.TextChanged

        If Initted1 Then Changed1 = True

    End Sub

    ''' <returns>false if it fails</returns>
    Private Function WriteRegion(RegionUUID As String) As Boolean

        ' save the Region File, choose an existing DOS box to put it in, or make a new one

        ' rename is possible
        If Oldname1 <> RegionName.Text And Not IsNew1 Then
            Try
                My.Computer.FileSystem.RenameFile(PropRegionClass.RegionIniFilePath(RegionUUID), RegionName.Text + ".ini")
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
                TextPrint(My.Resources.Aborted_word)
                Return False
            End Try

            ' rename it
            Dim RegionIniFolderPath = PropRegionClass.RegionIniFolderPath(RegionUUID)

            PropRegionClass.RegionIniFilePath(RegionUUID) = RegionIniFolderPath + "/" + RegionName.Text + ".ini"

        End If

        ' might be a new region, so give them a choice

        If IsNew1 Then
            Dim NewGroup As String

            NewGroup = RegionChosen(RegionName.Text)
            If NewGroup.Length = 0 Then
                TextPrint(My.Resources.Aborted_word)
                Return False
            End If

            If Not Directory.Exists(PropRegionClass.RegionIniFilePath(RegionUUID)) Or PropRegionClass.RegionIniFilePath(RegionUUID).Length = 0 Then
                Try
                    Directory.CreateDirectory(Settings.OpensimBinPath & "Regions\" + NewGroup + "\Region")
                Catch ex As Exception
                    BreakPoint.Show(ex.Message)
                    TextPrint(My.Resources.Aborted_word)
                    Return False
                End Try
            End If

            PropRegionClass.RegionIniFilePath(RegionUUID) = Settings.OpensimBinPath & "Regions\" + NewGroup + "\Region\" + RegionName.Text + ".ini"
            PropRegionClass.RegionIniFolderPath(RegionUUID) = System.IO.Path.GetDirectoryName(PropRegionClass.RegionIniFilePath(RegionUUID))
            PropRegionClass.GroupName(RegionUUID) = NewGroup

            Dim theEnd As Integer = PropRegionClass.RegionIniFolderPath(RegionUUID).LastIndexOf("\", StringComparison.InvariantCulture)
            PropRegionClass.OpensimIniPath(RegionUUID) = PropRegionClass.RegionIniFolderPath(RegionUUID).Substring(0, theEnd + 1)

        End If

        ' save the changes to the memory structure, then to disk

        PropRegionClass.CoordX(RegionUUID) = CInt("0" & CoordX.Text)
        PropRegionClass.CoordY(RegionUUID) = CInt("0" & CoordY.Text)

        PropRegionClass.RegionName(RegionUUID) = RegionName.Text

        PropRegionClass.RegionPort(RegionUUID) = PropRegionClass.LargestPort
        PropRegionClass.GroupPort(RegionUUID) = PropRegionClass.RegionPort(RegionUUID)
        PropRegionClass.SizeX(RegionUUID) = BoxSize
        PropRegionClass.SizeY(RegionUUID) = BoxSize
        PropRegionClass.RegionEnabled(RegionUUID) = EnabledCheckBox.Checked
        PropRegionClass.NonPhysicalPrimMax(RegionUUID) = NonphysicalPrimMax.Text
        PropRegionClass.PhysicalPrimMax(RegionUUID) = PhysicalPrimMax.Text
        PropRegionClass.ClampPrimSize(RegionUUID) = ClampPrimSize.Checked
        PropRegionClass.MaxAgents(RegionUUID) = MaxAgents.Text
        PropRegionClass.MaxPrims(RegionUUID) = MaxPrims.Text
        PropRegionClass.MinTimerInterval(RegionUUID) = ScriptTimerTextBox.Text
        PropRegionClass.FrameTime(RegionUUID) = FrametimeBox.Text

        Dim Snapshot As String = ""
        If PublishDefault.Checked Then
            Snapshot = ""
        ElseIf NoPublish.Checked Then
            Snapshot = "False"
        ElseIf Publish.Checked Then
            Snapshot = "True"
        End If

        PropRegionClass.RegionSnapShot(RegionUUID) = Snapshot

        Dim Map As String = ""
        If MapNone.Checked Then
            Map = ""
        ElseIf MapNone.Checked Then
            Map = "None"
        ElseIf MapSimple.Checked Then
            Map = "Simple"
        ElseIf MapGood.Checked Then
            Map = "Good"
        ElseIf MapBetter.Checked Then
            Map = "Better"
        ElseIf MapBest.Checked Then
            Map = "Best"
        End If

        PropRegionClass.MapType(RegionUUID) = Map

        Dim Phys As Integer = 2
        If Physics_Default.Checked Then
            Phys = -1
        ElseIf Physics_Separate.Checked Then
            Phys = 3
        ElseIf Physics_ubODE.Checked Then
            Phys = 4
        End If

        If Physics_Default.Checked Then
            PropRegionClass.Physics(RegionUUID) = CStr(Phys)
        End If

        If Gods_Use_Default.Checked Then
            PropRegionClass.GodDefault(RegionUUID) = "True"
            PropRegionClass.AllowGods(RegionUUID) = ""
            PropRegionClass.RegionGod(RegionUUID) = ""
            PropRegionClass.ManagerGod(RegionUUID) = ""
        Else
            PropRegionClass.GodDefault(RegionUUID) = "False"
            PropRegionClass.AllowGods(RegionUUID) = CStr(AllowGods.Checked)
            PropRegionClass.RegionGod(RegionUUID) = CStr(RegionGod.Checked)
            PropRegionClass.ManagerGod(RegionUUID) = CStr(ManagerGod.Checked)
        End If

        Dim Host = Settings.ExternalHostName

        If DisallowForeigners.Checked Then
            PropRegionClass.DisallowForeigners(RegionUUID) = "True"
        Else
            PropRegionClass.DisallowForeigners(RegionUUID) = ""
        End If

        If DisallowResidents.Checked Then
            PropRegionClass.DisallowResidents(RegionUUID) = "True"
        Else
            PropRegionClass.DisallowResidents(RegionUUID) = ""
        End If

        If SkipAutoCheckBox.Checked Then
            PropRegionClass.SkipAutobackup(RegionUUID) = "True"
        Else
            PropRegionClass.SkipAutobackup(RegionUUID) = ""
        End If

        If BirdsCheckBox.Checked Then
            PropRegionClass.Birds(RegionUUID) = "True"
        Else
            PropRegionClass.Birds(RegionUUID) = ""
        End If

        If TidesCheckbox.Checked Then
            PropRegionClass.Tides(RegionUUID) = "True"
        Else
            PropRegionClass.Tides(RegionUUID) = ""
        End If

        If TPCheckBox1.Checked Then
            PropRegionClass.Teleport(RegionUUID) = "True"
        Else
            PropRegionClass.Teleport(RegionUUID) = ""
        End If

        If DisableGBCheckBox.Checked Then
            PropRegionClass.DisableGloebits(RegionUUID) = "True"
        Else
            PropRegionClass.DisableGloebits(RegionUUID) = ""
        End If

        If NoPublish.Checked Then
            PropRegionClass.GDPR(RegionUUID) = "False"
        Else
            PropRegionClass.GDPR(RegionUUID) = ""
        End If

        If Publish.Checked Then
            PropRegionClass.GDPR(RegionUUID) = "True"
        Else
            PropRegionClass.GDPR(RegionUUID) = ""
        End If

        If SmartStartCheckBox.Checked Then
            PropRegionClass.SmartStart(RegionUUID) = "True"
        Else
            PropRegionClass.SmartStart(RegionUUID) = ""
        End If

        Dim ScriptEngine As String = ""
        PropRegionClass.ScriptEngine(RegionUUID) = ""
        If XEngineButton.Checked = True Then
            ScriptEngine = "XEngine"
            PropRegionClass.ScriptEngine(RegionUUID) = "XEngine"
        End If
        If YEngineButton.Checked = True Then
            ScriptEngine = "YEngine"
            PropRegionClass.ScriptEngine(RegionUUID) = "YEngine"
        End If

        Dim Region = "; * Regions configuration file" &
                            "; * This Is Your World. See Common Settings->[Region Settings]." & vbCrLf &
                            "; Automatically changed by Dreamworld" & vbCrLf &
                            "[" & RegionName.Text & "]" & vbCrLf &
                            "RegionUUID = " & UUID.Text & vbCrLf &
                            "Location = " & CoordX.Text & "," & CoordY.Text & vbCrLf &
                            "InternalAddress = 0.0.0.0" & vbCrLf &
                            "InternalPort = " & RegionPort.Text & vbCrLf &
                            "AllowAlternatePorts = False" & vbCrLf &
                            "ExternalHostName = " & Host & vbCrLf &
                            "SizeX = " & BoxSize & vbCrLf &
                            "SizeY = " & BoxSize & vbCrLf &
                            "Enabled = " & CStr(EnabledCheckBox.Checked) & vbCrLf &
                            "NonPhysicalPrimMax = " & NonphysicalPrimMax.Text & vbCrLf &
                            "PhysicalPrimMax = " & PhysicalPrimMax.Text & vbCrLf &
                            "ClampPrimSize = " & CStr(ClampPrimSize.Checked) & vbCrLf &
                            "MaxAgents = " & MaxAgents.Text & vbCrLf &
                            "MaxPrims = " & MaxPrims.Text & vbCrLf &
                            "RegionType = Estate" & vbCrLf & vbCrLf &
                            ";# Extended region properties from Dreamgrid" & vbCrLf &
                            "MinTimerInterval = " & ScriptTimerTextBox.Text & vbCrLf &
                            "FrameTime = " & FrametimeBox.Text & vbCrLf &
                            "RegionSnapShot = " & Snapshot & vbCrLf &
                            "MapType = " & Map & vbCrLf &
                            "Physics = " & Phys & vbCrLf &
                            "GodDefault = " & PropRegionClass.GodDefault(RegionUUID) & vbCrLf &
                            "AllowGods = " & PropRegionClass.AllowGods(RegionUUID) & vbCrLf &
                            "RegionGod = " & PropRegionClass.RegionGod(RegionUUID) & vbCrLf &
                            "ManagerGod = " & PropRegionClass.ManagerGod(RegionUUID) & vbCrLf &
                            "Birds = " & PropRegionClass.Birds(RegionUUID) & vbCrLf &
                            "Tides = " & PropRegionClass.Tides(RegionUUID) & vbCrLf &
                            "Teleport = " & PropRegionClass.Teleport(RegionUUID) & vbCrLf &
                            "DisableGloebits = " & PropRegionClass.DisableGloebits(RegionUUID) & vbCrLf &
                            "DisallowForeigners = " & PropRegionClass.DisallowForeigners(RegionUUID) & vbCrLf &
                            "DisallowResidents = " & PropRegionClass.DisallowResidents(RegionUUID) & vbCrLf &
                            "SkipAutoBackup = " & PropRegionClass.SkipAutobackup(RegionUUID) & vbCrLf &
                            "ScriptEngine = " & PropRegionClass.ScriptEngine(RegionUUID) & vbCrLf &
                            "Publicity = " & PropRegionClass.GDPR(RegionUUID) & vbCrLf &
                            "SmartStart = " & PropRegionClass.SmartStart(RegionUUID) & vbCrLf

        'Debug.Print(Region)

        Try
            Using outputFile As New StreamWriter(PropRegionClass.RegionIniFilePath(RegionUUID), False)
                outputFile.Write(Region)
            End Using
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
            Return False
            MsgBox(My.Resources.Cannot_save_region_word + ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.MsgBoxSetForeground)
        End Try

        If PropRegionClass.GetAllRegions() = -1 Then Return False

        PropUpdateView = True
        Oldname1 = RegionName.Text

        Return True

    End Function

#End Region

#Region "Radio"

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged

        If Initted1 And RadioButton1.Checked Then
            Changed1 = True
            BoxSize = 256
        End If

    End Sub

    Private Sub RadioButton10_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton9.CheckedChanged

        If Initted1 And RadioButton9.Checked Then
            Changed1 = True
            BoxSize = 256 * 9
        End If

    End Sub

    Private Sub RadioButton11_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton12.CheckedChanged

        If Initted1 And RadioButton12.Checked Then
            Changed1 = True
            BoxSize = 256 * 12
        End If

    End Sub

    Private Sub RadioButton12_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton11.CheckedChanged

        If Initted1 And RadioButton11.Checked Then
            Changed1 = True
            BoxSize = 256 * 11
        End If

    End Sub

    Private Sub RadioButton13_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton16.CheckedChanged

        If Initted1 And RadioButton16.Checked Then
            Changed1 = True
            BoxSize = 256 * 16
        End If

    End Sub

    Private Sub RadioButton14_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton15.CheckedChanged

        If Initted1 And RadioButton15.Checked Then
            Changed1 = True
            BoxSize = 256 * 15
        End If

    End Sub

    Private Sub RadioButton15_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton14.CheckedChanged

        If Initted1 And RadioButton14.Checked Then
            Changed1 = True
            BoxSize = 256 * 14
        End If

    End Sub

    Private Sub RadioButton16_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton13.CheckedChanged

        If Initted1 And RadioButton13.Checked Then
            Changed1 = True
            BoxSize = 256 * 13
        End If

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged

        If Initted1 And RadioButton2.Checked Then
            Changed1 = True
            BoxSize = 256 * 2
        End If

    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged

        If Initted1 And RadioButton3.Checked Then
            Changed1 = True
            BoxSize = 256 * 3
        End If

    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged

        If Initted1 And RadioButton4.Checked Then
            Changed1 = True
            BoxSize = 256 * 4
        End If

    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton8.CheckedChanged

        If Initted1 And RadioButton8.Checked Then
            Changed1 = True
            BoxSize = 256 * 8
        End If

    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton7.CheckedChanged

        If Initted1 And RadioButton7.Checked Then
            Changed1 = True
            BoxSize = 256 * 7
        End If

    End Sub

    Private Sub RadioButton7_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged

        If Initted1 And RadioButton6.Checked Then
            Changed1 = True
            BoxSize = 256 * 6
        End If

    End Sub

    Private Sub RadioButton8_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged

        If Initted1 And RadioButton5.Checked Then
            Changed1 = True
            BoxSize = 256 * 5
        End If

    End Sub

    Private Sub RadioButton9_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton10.CheckedChanged

        If Initted1 And RadioButton10.Checked Then
            Changed1 = True
            BoxSize = 256 * 10
        End If

    End Sub

#End Region

#Region "Gods"

    Private Sub AllowGods_CheckedChanged(sender As Object, e As EventArgs) Handles AllowGods.CheckedChanged

        If AllowGods.Checked Then
            Gods_Use_Default.Checked = False
            Log(My.Resources.Info_word, "Region " + Name + " Is allowing Gods")
        Else
            Log(My.Resources.Info_word, "Region " + Name + " is not allowing Region Gods")
        End If

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub Gods_Use_Default_CheckedChanged(sender As Object, e As EventArgs) Handles Gods_Use_Default.CheckedChanged

        If Gods_Use_Default.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Is set to default for Gods")
            AllowGods.Checked = False
            RegionGod.Checked = False
            ManagerGod.Checked = False
            Gods_Use_Default.Checked = True
        Else
            Log(My.Resources.Info_word, "Region " + Name + " is not allowing Region Gods")
        End If

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub ManagerGod_CheckedChanged(sender As Object, e As EventArgs) Handles ManagerGod.CheckedChanged

        If ManagerGod.Checked Then
            Gods_Use_Default.Checked = False
            Log(My.Resources.Info_word, "Region " + Name + " Is allowing Manager Gods")
        Else
            Log(My.Resources.Info_word, "Region " + Name + " Is Not allowing Manager Gods")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub RegionGod_CheckedChanged(sender As Object, e As EventArgs) Handles RegionGod.CheckedChanged

        If RegionGod.Checked Then
            Gods_Use_Default.Checked = False
            Log(My.Resources.Info_word, "Region " + Name + " is allowing Region Gods")
        Else
            Log(My.Resources.Info_word, "Region " + Name + " is not allowing Region Gods")
        End If

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub ScriptDefaultButton_CheckedChanged(sender As Object, e As EventArgs) Handles ScriptDefaultButton.CheckedChanged

        If ScriptDefaultButton.Checked Then
            XEngineButton.Checked = False
            YEngineButton.Checked = False
        End If

    End Sub

    Private Sub XEngineButton_CheckedChanged_1(sender As Object, e As EventArgs) Handles XEngineButton.CheckedChanged

        If XEngineButton.Checked Then
            ScriptDefaultButton.Checked = False
            YEngineButton.Checked = False
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub YEngineButton_CheckedChanged_1(sender As Object, e As EventArgs) Handles YEngineButton.CheckedChanged

        If YEngineButton.Checked Then
            ScriptDefaultButton.Checked = False
            XEngineButton.Checked = False
        End If
        If Initted1 Then Changed1 = True

    End Sub

#End Region

#Region "Physics"

    Private Sub ConciergeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ConciergeCheckBox.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Physics_Default_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Default.CheckedChanged

        If Physics_Default.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Physics Is set to default")
            Physics_ubODE.Checked = False
            Physics_Separate.Checked = False
        End If

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub Physics_Hybrid_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Hybrid.CheckedChanged

        If Physics_Hybrid.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Physics is set to Hybrid")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub PhysicsSeparate_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Separate.CheckedChanged

        If Physics_Separate.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Physics is set to Bullet in a Thread")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub PhysicsubODE_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_ubODE.CheckedChanged

        If Physics_ubODE.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Physics is set to Ubit ODE")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub RadioButton17_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Bullet.CheckedChanged

        If Physics_Bullet.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " Physics is set to Bullet")
        End If
        If Initted1 Then Changed1 = True

    End Sub

#End Region

End Class
