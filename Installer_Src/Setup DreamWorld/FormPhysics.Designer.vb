﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormPhysics
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormPhysics))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Physics1_ODE = New System.Windows.Forms.RadioButton()
        Me.Physics_4UbODE = New System.Windows.Forms.RadioButton()
        Me.NinjaRagdoll = New System.Windows.Forms.CheckBox()
        Me.Physics5_Hybrid = New System.Windows.Forms.RadioButton()
        Me.Physics2_Bullet = New System.Windows.Forms.RadioButton()
        Me.Physics3_Separate = New System.Windows.Forms.RadioButton()
        Me.Physics0_None = New System.Windows.Forms.RadioButton()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DatabaseSetupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Physics1_ODE)
        Me.GroupBox1.Controls.Add(Me.Physics_4UbODE)
        Me.GroupBox1.Controls.Add(Me.NinjaRagdoll)
        Me.GroupBox1.Controls.Add(Me.Physics5_Hybrid)
        Me.GroupBox1.Controls.Add(Me.Physics2_Bullet)
        Me.GroupBox1.Controls.Add(Me.Physics3_Separate)
        Me.GroupBox1.Controls.Add(Me.Physics0_None)
        Me.GroupBox1.Location = New System.Drawing.Point(18, 50)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(323, 258)
        Me.GroupBox1.TabIndex = 43
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Physics Engine"
        '
        'Physics1_ODE
        '
        Me.Physics1_ODE.AutoSize = True
        Me.Physics1_ODE.Location = New System.Drawing.Point(9, 62)
        Me.Physics1_ODE.Margin = New System.Windows.Forms.Padding(4)
        Me.Physics1_ODE.Name = "Physics1_ODE"
        Me.Physics1_ODE.Size = New System.Drawing.Size(192, 24)
        Me.Physics1_ODE.TabIndex = 22
        Me.Physics1_ODE.TabStop = True
        Me.Physics1_ODE.Text = "Open Dynamic Engine"
        Me.Physics1_ODE.UseVisualStyleBackColor = True
        '
        'Physics_4UbODE
        '
        Me.Physics_4UbODE.AutoSize = True
        Me.Physics_4UbODE.Location = New System.Drawing.Point(9, 94)
        Me.Physics_4UbODE.Margin = New System.Windows.Forms.Padding(4)
        Me.Physics_4UbODE.Name = "Physics_4UbODE"
        Me.Physics_4UbODE.Size = New System.Drawing.Size(225, 24)
        Me.Physics_4UbODE.TabIndex = 21
        Me.Physics_4UbODE.TabStop = True
        Me.Physics_4UbODE.Text = Global.Outworldz.My.Resources.Resources.UBODE_words
        Me.Physics_4UbODE.UseVisualStyleBackColor = True
        '
        'NinjaRagdoll
        '
        Me.NinjaRagdoll.AutoSize = True
        Me.NinjaRagdoll.Location = New System.Drawing.Point(9, 234)
        Me.NinjaRagdoll.Name = "NinjaRagdoll"
        Me.NinjaRagdoll.Size = New System.Drawing.Size(89, 24)
        Me.NinjaRagdoll.TabIndex = 20
        Me.NinjaRagdoll.Text = "Ninja Ragdoll"
        Me.NinjaRagdoll.UseVisualStyleBackColor = True
        '
        'Physics5_Hybrid
        '
        Me.Physics5_Hybrid.AutoSize = True
        Me.Physics5_Hybrid.Location = New System.Drawing.Point(9, 190)
        Me.Physics5_Hybrid.Margin = New System.Windows.Forms.Padding(4)
        Me.Physics5_Hybrid.Name = "Physics5_Hybrid"
        Me.Physics5_Hybrid.Size = New System.Drawing.Size(79, 24)
        Me.Physics5_Hybrid.TabIndex = 16
        Me.Physics5_Hybrid.TabStop = True
        Me.Physics5_Hybrid.Text = "Hybrid"
        Me.Physics5_Hybrid.UseVisualStyleBackColor = True
        '
        'Physics2_Bullet
        '
        Me.Physics2_Bullet.AutoSize = True
        Me.Physics2_Bullet.Location = New System.Drawing.Point(9, 126)
        Me.Physics2_Bullet.Margin = New System.Windows.Forms.Padding(4)
        Me.Physics2_Bullet.Name = "Physics2_Bullet"
        Me.Physics2_Bullet.Size = New System.Drawing.Size(134, 24)
        Me.Physics2_Bullet.TabIndex = 15
        Me.Physics2_Bullet.TabStop = True
        Me.Physics2_Bullet.Text = "Bullet physics "
        Me.Physics2_Bullet.UseVisualStyleBackColor = True
        '
        'Physics3_Separate
        '
        Me.Physics3_Separate.AutoSize = True
        Me.Physics3_Separate.Location = New System.Drawing.Point(9, 158)
        Me.Physics3_Separate.Margin = New System.Windows.Forms.Padding(4)
        Me.Physics3_Separate.Name = "Physics3_Separate"
        Me.Physics3_Separate.Size = New System.Drawing.Size(263, 24)
        Me.Physics3_Separate.TabIndex = 13
        Me.Physics3_Separate.TabStop = True
        Me.Physics3_Separate.Text = Global.Outworldz.My.Resources.Resources.BP
        Me.Physics3_Separate.UseVisualStyleBackColor = True
        '
        'Physics0_None
        '
        Me.Physics0_None.AutoSize = True
        Me.Physics0_None.Location = New System.Drawing.Point(9, 30)
        Me.Physics0_None.Margin = New System.Windows.Forms.Padding(4)
        Me.Physics0_None.Name = "Physics0_None"
        Me.Physics0_None.Size = New System.Drawing.Size(72, 24)
        Me.Physics0_None.TabIndex = 9
        Me.Physics0_None.TabStop = True
        Me.Physics0_None.Text = Global.Outworldz.My.Resources.Resources.None
        Me.Physics0_None.UseVisualStyleBackColor = True
        '
        'MenuStrip2
        '
        Me.MenuStrip2.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem30})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Size = New System.Drawing.Size(354, 33)
        Me.MenuStrip2.TabIndex = 1891
        Me.MenuStrip2.Text = "0"
        '
        'ToolStripMenuItem30
        '
        Me.ToolStripMenuItem30.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DatabaseSetupToolStripMenuItem})
        Me.ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.Resources.question_and_answer
        Me.ToolStripMenuItem30.Name = "ToolStripMenuItem30"
        Me.ToolStripMenuItem30.Size = New System.Drawing.Size(89, 29)
        Me.ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'DatabaseSetupToolStripMenuItem
        '
        Me.DatabaseSetupToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.DatabaseSetupToolStripMenuItem.Name = "DatabaseSetupToolStripMenuItem"
        Me.DatabaseSetupToolStripMenuItem.Size = New System.Drawing.Size(151, 34)
        Me.DatabaseSetupToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'FormPhysics
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(354, 321)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "FormPhysics"
        Me.Text = "Physics"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Physics3_Separate As RadioButton
    Friend WithEvents Physics0_None As RadioButton
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents ToolStripMenuItem30 As ToolStripMenuItem
    Friend WithEvents DatabaseSetupToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NinjaRagdoll As CheckBox
    Friend WithEvents Physics5_Hybrid As RadioButton
    Friend WithEvents Physics2_Bullet As RadioButton
    Friend WithEvents Physics_4UbODE As RadioButton
    Friend WithEvents Physics1_ODE As RadioButton
End Class
