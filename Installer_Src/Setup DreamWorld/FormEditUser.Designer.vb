<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormEditUser
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
        Me.components = New System.ComponentModel.Container()
        Me.FnameTextBox = New System.Windows.Forms.TextBox()
        Me.LastNameTextBox = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LevelGroupBox = New System.Windows.Forms.GroupBox()
        Me.RadioNologin = New System.Windows.Forms.RadioButton()
        Me.RadioLogin = New System.Windows.Forms.RadioButton()
        Me.RadioDiva = New System.Windows.Forms.RadioButton()
        Me.RadioGod = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.EmailTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TitleTextBox = New System.Windows.Forms.TextBox()
        Me.UUIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.LevelGroupBox.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'FnameTextBox
        '
        Me.FnameTextBox.Location = New System.Drawing.Point(28, 46)
        Me.FnameTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.FnameTextBox.Name = "FnameTextBox"
        Me.FnameTextBox.Size = New System.Drawing.Size(181, 22)
        Me.FnameTextBox.TabIndex = 0
        '
        'LastNameTextBox
        '
        Me.LastNameTextBox.Location = New System.Drawing.Point(253, 46)
        Me.LastNameTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.LastNameTextBox.Name = "LastNameTextBox"
        Me.LastNameTextBox.Size = New System.Drawing.Size(256, 22)
        Me.LastNameTextBox.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.LevelGroupBox)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.EmailTextBox)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.TitleTextBox)
        Me.GroupBox1.Controls.Add(Me.UUIDTextBox)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.FnameTextBox)
        Me.GroupBox1.Controls.Add(Me.LastNameTextBox)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 54)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(561, 331)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "User"
        '
        'LevelGroupBox
        '
        Me.LevelGroupBox.Controls.Add(Me.RadioNologin)
        Me.LevelGroupBox.Controls.Add(Me.RadioLogin)
        Me.LevelGroupBox.Controls.Add(Me.RadioDiva)
        Me.LevelGroupBox.Controls.Add(Me.RadioGod)
        Me.LevelGroupBox.Location = New System.Drawing.Point(273, 150)
        Me.LevelGroupBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.LevelGroupBox.Name = "LevelGroupBox"
        Me.LevelGroupBox.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.LevelGroupBox.Size = New System.Drawing.Size(267, 165)
        Me.LevelGroupBox.TabIndex = 11
        Me.LevelGroupBox.TabStop = False
        Me.LevelGroupBox.Text = "Level"
        '
        'RadioNologin
        '
        Me.RadioNologin.AutoSize = True
        Me.RadioNologin.Location = New System.Drawing.Point(28, 23)
        Me.RadioNologin.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioNologin.Name = "RadioNologin"
        Me.RadioNologin.Size = New System.Drawing.Size(86, 21)
        Me.RadioNologin.TabIndex = 2
        Me.RadioNologin.TabStop = True
        Me.RadioNologin.Text = "No Login"
        Me.RadioNologin.UseVisualStyleBackColor = True
        '
        'RadioLogin
        '
        Me.RadioLogin.AutoSize = True
        Me.RadioLogin.Location = New System.Drawing.Point(28, 52)
        Me.RadioLogin.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioLogin.Name = "RadioLogin"
        Me.RadioLogin.Size = New System.Drawing.Size(115, 21)
        Me.RadioLogin.TabIndex = 3
        Me.RadioLogin.TabStop = True
        Me.RadioLogin.Text = "Login allowed"
        Me.RadioLogin.UseVisualStyleBackColor = True
        '
        'RadioDiva
        '
        Me.RadioDiva.AutoSize = True
        Me.RadioDiva.Location = New System.Drawing.Point(28, 80)
        Me.RadioDiva.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioDiva.Name = "RadioDiva"
        Me.RadioDiva.Size = New System.Drawing.Size(127, 21)
        Me.RadioDiva.TabIndex = 4
        Me.RadioDiva.TabStop = True
        Me.RadioDiva.Text = "Diva Wifi Admin"
        Me.RadioDiva.UseVisualStyleBackColor = True
        '
        'RadioGod
        '
        Me.RadioGod.AutoSize = True
        Me.RadioGod.Location = New System.Drawing.Point(28, 108)
        Me.RadioGod.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioGod.Name = "RadioGod"
        Me.RadioGod.Size = New System.Drawing.Size(150, 21)
        Me.RadioGod.TabIndex = 5
        Me.RadioGod.TabStop = True
        Me.RadioGod.Text = "Level God Enabled"
        Me.RadioGod.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(24, 130)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(42, 17)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Email"
        '
        'EmailTextBox
        '
        Me.EmailTextBox.Location = New System.Drawing.Point(28, 150)
        Me.EmailTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.EmailTextBox.Name = "EmailTextBox"
        Me.EmailTextBox.Size = New System.Drawing.Size(201, 22)
        Me.EmailTextBox.TabIndex = 15
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 78)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 17)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Title"
        '
        'TitleTextBox
        '
        Me.TitleTextBox.Location = New System.Drawing.Point(28, 97)
        Me.TitleTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TitleTextBox.Name = "TitleTextBox"
        Me.TitleTextBox.Size = New System.Drawing.Size(201, 22)
        Me.TitleTextBox.TabIndex = 13
        '
        'UUIDTextBox
        '
        Me.UUIDTextBox.Enabled = False
        Me.UUIDTextBox.Location = New System.Drawing.Point(253, 97)
        Me.UUIDTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.UUIDTextBox.Name = "UUIDTextBox"
        Me.UUIDTextBox.Size = New System.Drawing.Size(299, 22)
        Me.UUIDTextBox.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(249, 78)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 17)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "UUID"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(24, 26)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 17)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "First Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(249, 26)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 17)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Last Name"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(593, 30)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(75, 26)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(28, 203)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(136, 21)
        Me.CheckBox1.TabIndex = 17
        Me.CheckBox1.Text = "Auto Backup IAR"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'FormEditUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(593, 396)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "FormEditUser"
        Me.Text = "FormEditUser"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.LevelGroupBox.ResumeLayout(False)
        Me.LevelGroupBox.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FnameTextBox As TextBox
    Friend WithEvents LastNameTextBox As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents RadioGod As RadioButton
    Friend WithEvents RadioDiva As RadioButton
    Friend WithEvents RadioLogin As RadioButton
    Friend WithEvents RadioNologin As RadioButton
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents LevelGroupBox As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents TitleTextBox As TextBox
    Friend WithEvents UUIDTextBox As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents EmailTextBox As TextBox
    Friend WithEvents CheckBox1 As CheckBox
End Class
