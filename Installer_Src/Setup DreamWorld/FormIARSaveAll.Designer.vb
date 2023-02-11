<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormIarSaveAll
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.FilterGroupBox = New System.Windows.Forms.GroupBox()
        Me.CopyCheckBox = New System.Windows.Forms.CheckBox()
        Me.TransferCheckBox = New System.Windows.Forms.CheckBox()
        Me.ModifyCheckBox = New System.Windows.Forms.CheckBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ObjectNameBox = New System.Windows.Forms.TextBox()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FilterGroupBox.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'FilterGroupBox
        '
        Me.FilterGroupBox.Controls.Add(Me.CopyCheckBox)
        Me.FilterGroupBox.Controls.Add(Me.TransferCheckBox)
        Me.FilterGroupBox.Controls.Add(Me.ModifyCheckBox)
        Me.FilterGroupBox.Location = New System.Drawing.Point(540, 65)
        Me.FilterGroupBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.FilterGroupBox.Name = "FilterGroupBox"
        Me.FilterGroupBox.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.FilterGroupBox.Size = New System.Drawing.Size(151, 132)
        Me.FilterGroupBox.TabIndex = 7
        Me.FilterGroupBox.TabStop = False
        Me.FilterGroupBox.Text = "Filter"
        '
        'CopyCheckBox
        '
        Me.CopyCheckBox.AutoSize = True
        Me.CopyCheckBox.Checked = True
        Me.CopyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CopyCheckBox.Location = New System.Drawing.Point(16, 33)
        Me.CopyCheckBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CopyCheckBox.Name = "CopyCheckBox"
        Me.CopyCheckBox.Size = New System.Drawing.Size(62, 21)
        Me.CopyCheckBox.TabIndex = 0
        Me.CopyCheckBox.Text = "Copy"
        Me.CopyCheckBox.UseVisualStyleBackColor = True
        '
        'TransferCheckBox
        '
        Me.TransferCheckBox.AutoSize = True
        Me.TransferCheckBox.Checked = True
        Me.TransferCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TransferCheckBox.Location = New System.Drawing.Point(16, 87)
        Me.TransferCheckBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TransferCheckBox.Name = "TransferCheckBox"
        Me.TransferCheckBox.Size = New System.Drawing.Size(84, 21)
        Me.TransferCheckBox.TabIndex = 2
        Me.TransferCheckBox.Text = "Transfer"
        Me.TransferCheckBox.UseVisualStyleBackColor = True
        '
        'ModifyCheckBox
        '
        Me.ModifyCheckBox.AutoSize = True
        Me.ModifyCheckBox.Checked = True
        Me.ModifyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ModifyCheckBox.Location = New System.Drawing.Point(16, 60)
        Me.ModifyCheckBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ModifyCheckBox.Name = "ModifyCheckBox"
        Me.ModifyCheckBox.Size = New System.Drawing.Size(71, 21)
        Me.ModifyCheckBox.TabIndex = 1
        Me.ModifyCheckBox.Text = "Modify"
        Me.ModifyCheckBox.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(5, 1, 0, 1)
        Me.MenuStrip1.Size = New System.Drawing.Size(713, 34)
        Me.MenuStrip1.TabIndex = 4
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(47, 154)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(144, 28)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = Global.Outworldz.My.Resources.Resources.Save_IAR_word
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.ObjectNameBox)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 57)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(492, 90)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Save Inventory IAR"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(295, 33)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(149, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Object Path and name"
        '
        'ObjectNameBox
        '
        Me.ObjectNameBox.Location = New System.Drawing.Point(7, 31)
        Me.ObjectNameBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ObjectNameBox.Name = "ObjectNameBox"
        Me.ObjectNameBox.Size = New System.Drawing.Size(279, 22)
        Me.ObjectNameBox.TabIndex = 0
        Me.ObjectNameBox.Text = "/=everything, /Objects/Folder, etc."
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(83, 32)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'FormIarSaveAll
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(713, 208)
        Me.Controls.Add(Me.FilterGroupBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "FormIarSaveAll"
        Me.Text = "FormIARSaveAll"
        Me.FilterGroupBox.ResumeLayout(False)
        Me.FilterGroupBox.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FilterGroupBox As GroupBox
    Friend WithEvents CopyCheckBox As CheckBox
    Friend WithEvents TransferCheckBox As CheckBox
    Friend WithEvents ModifyCheckBox As CheckBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents Button1 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ObjectNameBox As TextBox
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
End Class
