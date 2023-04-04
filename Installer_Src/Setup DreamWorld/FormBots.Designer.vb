<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CampBots
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ConnectButton = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisconnectButton = New System.Windows.Forms.Button()
        Me.StartButton = New System.Windows.Forms.Button()
        Me.StopButton = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.CheckBoxNone = New System.Windows.Forms.CheckBox()
        Me.CheckBoxTeleport = New System.Windows.Forms.CheckBox()
        Me.CheckBoxGrab = New System.Windows.Forms.CheckBox()
        Me.CheckBoxPhysics = New System.Windows.Forms.CheckBox()
        Me.SitButton = New System.Windows.Forms.Button()
        Me.StandButton = New System.Windows.Forms.Button()
        Me.StatusButton = New System.Windows.Forms.Button()
        Me.RegionButton = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.SendAgentUpdatesCheckBox = New System.Windows.Forms.CheckBox()
        Me.RequestObjectTexturesCheckBox = New System.Windows.Forms.CheckBox()
        Me.MenuStrip1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ConnectButton
        '
        Me.ConnectButton.Location = New System.Drawing.Point(187, 3)
        Me.ConnectButton.Name = "ConnectButton"
        Me.ConnectButton.Size = New System.Drawing.Size(86, 23)
        Me.ConnectButton.TabIndex = 0
        Me.ConnectButton.Text = "Connect"
        Me.ConnectButton.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(12, 133)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(650, 305)
        Me.TextBox1.TabIndex = 5
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(674, 24)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'DisconnectButton
        '
        Me.DisconnectButton.Location = New System.Drawing.Point(279, 3)
        Me.DisconnectButton.Name = "DisconnectButton"
        Me.DisconnectButton.Size = New System.Drawing.Size(86, 23)
        Me.DisconnectButton.TabIndex = 8
        Me.DisconnectButton.Text = "Disconnect"
        Me.DisconnectButton.UseVisualStyleBackColor = True
        '
        'StartButton
        '
        Me.StartButton.Location = New System.Drawing.Point(3, 3)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(86, 23)
        Me.StartButton.TabIndex = 9
        Me.StartButton.Text = " Start"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'StopButton
        '
        Me.StopButton.Location = New System.Drawing.Point(95, 3)
        Me.StopButton.Name = "StopButton"
        Me.StopButton.Size = New System.Drawing.Size(86, 23)
        Me.StopButton.TabIndex = 10
        Me.StopButton.Text = "Stop"
        Me.StopButton.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.RequestObjectTexturesCheckBox, 4, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.CheckBoxNone, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.SendAgentUpdatesCheckBox, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.CheckBoxTeleport, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.CheckBoxGrab, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.CheckBoxPhysics, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.SitButton, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.StandButton, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.StatusButton, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.RegionButton, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.StopButton, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.StartButton, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ConnectButton, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.DisconnectButton, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBox1, 4, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 37)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.16518!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.16518!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.66964!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(518, 90)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'CheckBoxNone
        '
        Me.CheckBoxNone.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxNone.AutoSize = True
        Me.CheckBoxNone.Location = New System.Drawing.Point(279, 69)
        Me.CheckBoxNone.Name = "CheckBoxNone"
        Me.CheckBoxNone.Size = New System.Drawing.Size(86, 17)
        Me.CheckBoxNone.TabIndex = 17
        Me.CheckBoxNone.Text = "None"
        Me.CheckBoxNone.UseVisualStyleBackColor = True
        '
        'CheckBoxTeleport
        '
        Me.CheckBoxTeleport.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxTeleport.AutoSize = True
        Me.CheckBoxTeleport.Location = New System.Drawing.Point(187, 69)
        Me.CheckBoxTeleport.Name = "CheckBoxTeleport"
        Me.CheckBoxTeleport.Size = New System.Drawing.Size(86, 17)
        Me.CheckBoxTeleport.TabIndex = 16
        Me.CheckBoxTeleport.Text = "Teleport"
        Me.CheckBoxTeleport.UseVisualStyleBackColor = True
        '
        'CheckBoxGrab
        '
        Me.CheckBoxGrab.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxGrab.AutoSize = True
        Me.CheckBoxGrab.Location = New System.Drawing.Point(95, 69)
        Me.CheckBoxGrab.Name = "CheckBoxGrab"
        Me.CheckBoxGrab.Size = New System.Drawing.Size(86, 17)
        Me.CheckBoxGrab.TabIndex = 15
        Me.CheckBoxGrab.Text = "Grab"
        Me.CheckBoxGrab.UseVisualStyleBackColor = True
        '
        'CheckBoxPhysics
        '
        Me.CheckBoxPhysics.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxPhysics.AutoSize = True
        Me.CheckBoxPhysics.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxPhysics.Location = New System.Drawing.Point(3, 69)
        Me.CheckBoxPhysics.Name = "CheckBoxPhysics"
        Me.CheckBoxPhysics.Size = New System.Drawing.Size(86, 17)
        Me.CheckBoxPhysics.TabIndex = 8
        Me.CheckBoxPhysics.Text = "Physics"
        Me.CheckBoxPhysics.UseVisualStyleBackColor = True
        '
        'SitButton
        '
        Me.SitButton.Location = New System.Drawing.Point(279, 36)
        Me.SitButton.Name = "SitButton"
        Me.SitButton.Size = New System.Drawing.Size(86, 23)
        Me.SitButton.TabIndex = 14
        Me.SitButton.Text = " Sit"
        Me.SitButton.UseVisualStyleBackColor = True
        '
        'StandButton
        '
        Me.StandButton.Location = New System.Drawing.Point(187, 36)
        Me.StandButton.Name = "StandButton"
        Me.StandButton.Size = New System.Drawing.Size(86, 23)
        Me.StandButton.TabIndex = 13
        Me.StandButton.Text = " Stand"
        Me.StandButton.UseVisualStyleBackColor = True
        '
        'StatusButton
        '
        Me.StatusButton.Location = New System.Drawing.Point(3, 36)
        Me.StatusButton.Name = "StatusButton"
        Me.StatusButton.Size = New System.Drawing.Size(86, 23)
        Me.StatusButton.TabIndex = 12
        Me.StatusButton.Text = "Status"
        Me.StatusButton.UseVisualStyleBackColor = True
        '
        'RegionButton
        '
        Me.RegionButton.Location = New System.Drawing.Point(95, 36)
        Me.RegionButton.Name = "RegionButton"
        Me.RegionButton.Size = New System.Drawing.Size(86, 23)
        Me.RegionButton.TabIndex = 11
        Me.RegionButton.Text = "Regions"
        Me.RegionButton.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Home", "Last", "(Region Name)"})
        Me.ComboBox1.Location = New System.Drawing.Point(371, 3)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(134, 21)
        Me.ComboBox1.TabIndex = 8
        '
        'SendAgentUpdatesCheckBox
        '
        Me.SendAgentUpdatesCheckBox.AutoSize = True
        Me.SendAgentUpdatesCheckBox.Location = New System.Drawing.Point(371, 36)
        Me.SendAgentUpdatesCheckBox.Name = "SendAgentUpdatesCheckBox"
        Me.SendAgentUpdatesCheckBox.Size = New System.Drawing.Size(125, 17)
        Me.SendAgentUpdatesCheckBox.TabIndex = 19
        Me.SendAgentUpdatesCheckBox.Text = "Send Agent Updates"
        Me.SendAgentUpdatesCheckBox.UseVisualStyleBackColor = True
        '
        'RequestObjectTexturesCheckBox
        '
        Me.RequestObjectTexturesCheckBox.AutoSize = True
        Me.RequestObjectTexturesCheckBox.Location = New System.Drawing.Point(371, 69)
        Me.RequestObjectTexturesCheckBox.Name = "RequestObjectTexturesCheckBox"
        Me.RequestObjectTexturesCheckBox.Size = New System.Drawing.Size(144, 17)
        Me.RequestObjectTexturesCheckBox.TabIndex = 20
        Me.RequestObjectTexturesCheckBox.Text = "Request Object Textures"
        Me.RequestObjectTexturesCheckBox.UseVisualStyleBackColor = True
        '
        'CampBots
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(674, 450)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.TextBox1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "CampBots"
        Me.Text = "FormBots"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ConnectButton As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DisconnectButton As Button
    Friend WithEvents StartButton As Button
    Friend WithEvents StopButton As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents SitButton As Button
    Friend WithEvents StandButton As Button
    Friend WithEvents StatusButton As Button
    Friend WithEvents RegionButton As Button
    Friend WithEvents CheckBoxNone As CheckBox
    Friend WithEvents CheckBoxTeleport As CheckBox
    Friend WithEvents CheckBoxGrab As CheckBox
    Friend WithEvents CheckBoxPhysics As CheckBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents SendAgentUpdatesCheckBox As CheckBox
    Friend WithEvents RequestObjectTexturesCheckBox As CheckBox
End Class
